using System;
using System.Globalization;
using Mbs.Trading.Data;
using Mbs.Trading.Indicators.Abstractions;

namespace Mbs.Trading.Indicators
{
    /// <summary>
    /// Developed by George C. Lane in the late 1950s, the Stochastic Oscillator is a momentum indicator that shows
    /// the location of the last closing price relative to the previous high-low range over a number of periods.
    /// <para />
    /// The calculated values are:
    /// <para />
    /// ➊ %Kᵢʳ¹ = 100∙(Closeᵢ - Lᵢʳ¹) / (Hᵢʳ¹ - Lᵢʳ¹)
    /// <para />
    /// where Lᵢʳ¹ = min {Lowᵢ … Lowᵢ₋ᵣ₁}, Hᵢʳ¹ = max {Highᵢ … Highᵢ₋ᵣ₁}, and r₁ is the period in bars (K-length),
    /// <para />
    /// ➋ %Dᵢ = EMAʳ²(%Kᵢ) is a r₂-period (D-length) exponential moving average of %K,
    /// <para />
    /// ➌ %Dᵢˢˡᵒʷ = EMAʳ³(%Dᵢ) is a r₃-period (Dˢˡᵒʷ-length) exponential moving average of %D.
    /// <para />
    /// The default values are r₁ = 5, r₂ = 3, r₃ = 3.
    /// </summary>
    public sealed class StochasticOscillator : Indicator, ILineIndicator
    {
        #region Members and accessors
        /// <summary>
        /// The length (the number of time periods) of the <c>%K</c> line.
        /// </summary>
        public int KLength { get; }

        /// <summary>
        /// The equivalent length (the number of time periods) of the <c>%D</c> (the smoothed <c>%K</c>) line.
        /// </summary>
        public int DFastLength { get; }

        /// <summary>
        /// The equivalent length (the number of time periods) of the <c>%Dˢˡᵒʷ</c> (the smoothed <c>%D</c>) signal line.
        /// </summary>
        public int DSlowLength { get; }

        /// <summary>
        /// The <c>%D</c> moving average indicator used to smooth the <c>%K</c> line.
        /// </summary>
        public ILineIndicator DFastMovingAverageIndicator { get; }

        /// <summary>
        /// The <c>%Dˢˡᵒʷ</c> moving average indicator used to smooth the <c>%D</c> line.
        /// </summary>
        public ILineIndicator DSlowMovingAverageIndicator { get; }

        private double kValue = double.NaN;
        /// <summary>
        /// The current value of the <c>%K</c> or <c>NaN</c> if not primed.
        /// </summary>
        public double KValue { get { lock (Lock) { return kValue; } } }

        /// <summary>
        /// The current value of the <c>%K</c> or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (Lock) { return kValue; } } }

        private double dFastValue = double.NaN;
        /// <summary>
        /// The current value of the <c>%D</c> or <c>NaN</c> if not primed.
        /// </summary>
        public double DFastValue { get { lock (Lock) { return dFastValue; } } }

        private double dSlowValue = double.NaN;
        /// <summary>
        /// The current value of the <c>%Dˢˡᵒʷ</c> or <c>NaN</c> if not primed.
        /// </summary>
        public double DSlowValue { get { lock (Lock) { return dSlowValue; } } }

        /// <summary>
        /// The current value of the <c>%K</c> and <c>%D</c> band.
        /// The <c>first</c> band element is a <c>%K</c> value, the second band element is a <c>%D</c> value.
        /// </summary>
        public Band KdBand { get { lock(Lock) { return new Band(time, kValue, dFastValue); } } }

        /// <summary>
        /// The current value of the <c>%D</c> and <c>%Dˢˡᵒʷ</c> band.
        /// The <c>first</c> band element is a <c>%D</c> value, the second band element is a <c>%Dˢˡᵒʷ</c> value.
        /// </summary>
        public Band DdBand { get { lock (Lock) { return new Band(time, dFastValue, dSlowValue); } } }

        private DateTime time;
        private readonly int kLengthMinOne;
        private int circularInputIndex;
        private int circularInputCount;
        private readonly double[] lowCircularInput;
        private readonly double[] highCircularInput;

        private const double Epsilon = 0.00000001;
        private const string Sto = "STO";
        private const string StoFull = "Stochastic Oscillator";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="StochasticOscillator"/> class.
        /// </summary>
        /// <param name="kLength">The length (the number of time periods) of the <c>%K</c> line. The typical value are 5, 9 or 14 periods. The default value is 5.</param>
        /// <param name="dFastLength">The length (the number of time periods) of the <c>%D</c> Exponential Moving Average (smoothed <c>%K</c>). The default value is 3.</param>
        /// <param name="dSlowLength">The length (the number of time periods) of the <c>%Dˢˡᵒʷ</c> (signal) Exponential Moving Average (smoothed <c>%D</c>). The default value is 3.</param>
        /// <param name="firstIsAverage">If the very first EMA value is a simple average of the first 'period' (the most widely documented approach) or the first input value (used in Metastock).</param>
        public StochasticOscillator(int kLength = 5, int dFastLength = 3, int dSlowLength = 3, bool firstIsAverage = true)
            : base(Sto, StoFull)
        {
            if (2 > kLength)
                throw new ArgumentOutOfRangeException(nameof(kLength));
            if (2 > dFastLength)
                throw new ArgumentOutOfRangeException(nameof(dFastLength));
            if (2 > dSlowLength)
                throw new ArgumentOutOfRangeException(nameof(dSlowLength));
            KLength = kLength;
            DFastLength = dFastLength;
            DFastMovingAverageIndicator = new ExponentialMovingAverage(dFastLength, firstIsAverage, OhlcvComponent);
            DSlowLength = dSlowLength;
            DSlowMovingAverageIndicator = new ExponentialMovingAverage(dSlowLength, firstIsAverage);
            Moniker = string.Concat(Sto, "(", kLength.ToString(CultureInfo.InvariantCulture), ",", dFastLength.ToString(CultureInfo.InvariantCulture), ",", dSlowLength.ToString(CultureInfo.InvariantCulture), ")");
            kLengthMinOne = kLength - 1;
            lowCircularInput = new double[kLength];
            highCircularInput = new double[kLength];
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="StochasticOscillator"/> class.
        /// </summary>
        /// <param name="kLength">The length (the number of time periods) of the <c>%K</c> line. The typical value are 5, 9 or 14 periods.</param>
        /// <param name="dFastAlpha">The smoothing factor, <c>α</c>, of the <c>%D</c> Exponential Moving Average (smoothed <c>%K</c>). The equivalent length <c>ℓ</c> is <para><c>ℓ = 2/α - 1, 0 &lt; α ≤ 1, 1 ≤ ℓ</c>.</para><para>The default value is 0.5 (ℓ = 3).</para></param>
        /// <param name="dSlowAlpha">The smoothing factor, <c>α</c>, of the <c>%Dˢˡᵒʷ</c> (signal) Exponential Moving Average (smoothed <c>%D</c>). The equivalent length <c>ℓ</c> is <para><c>ℓ = 2/α - 1, 0 &lt; α ≤ 1, 1 ≤ ℓ</c>.</para><para>The default value is 0.5 (ℓ = 3).</para></param>
        /// <param name="firstIsAverage">If the very first EMA value is a simple average of the first 'period' (the most widely documented approach) or the first input value (used in Metastock).</param>
        public StochasticOscillator(int kLength, double dFastAlpha, double dSlowAlpha, bool firstIsAverage = true)
            : base(Sto, StoFull)
        {
            if (2 > kLength)
                throw new ArgumentOutOfRangeException(nameof(kLength));
            KLength = kLength;
            if (0d > dFastAlpha || 1d < dFastAlpha)
                throw new ArgumentOutOfRangeException(nameof(dFastAlpha));
            if (Epsilon > dFastAlpha)
                DFastLength = int.MaxValue;
            else
                DFastLength = (int)Math.Round(2d / dFastAlpha) - 1;
            if (0d > dSlowAlpha || 1d < dSlowAlpha)
                throw new ArgumentOutOfRangeException(nameof(dSlowAlpha));
            if (Epsilon > dSlowAlpha)
                DSlowLength = int.MaxValue;
            else
                DSlowLength = (int)Math.Round(2d / dSlowAlpha) - 1;
            DFastMovingAverageIndicator = new ExponentialMovingAverage(dFastAlpha, firstIsAverage, OhlcvComponent);
            DSlowMovingAverageIndicator = new ExponentialMovingAverage(dSlowAlpha, firstIsAverage, OhlcvComponent);
            Moniker = string.Concat(Sto, "(", kLength.ToString(CultureInfo.InvariantCulture), ",", DFastLength.ToString(CultureInfo.InvariantCulture), ",", DSlowLength.ToString(CultureInfo.InvariantCulture), ")");
            kLengthMinOne = kLength - 1;
            lowCircularInput = new double[kLength];
            highCircularInput = new double[kLength];
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="StochasticOscillator"/> class.
        /// </summary>
        /// <param name="kLength">The length (the number of time periods) of the <c>%K</c> line. The typical value are 5, 9 or 14 periods.</param>
        /// <param name="dFastLength">The equivalent length (the number of time periods) of the <c>%D</c> (smoothed <c>%K</c>) Moving Average. The default value is 3.</param>
        /// <param name="dFastLineIndicator">The Moving Average line indicator to calculate the <c>%D</c> (smoothed <c>%K</c>).</param>
        /// <param name="dSlowLength">The equivalent length (the number of time periods) of the <c>%Dˢˡᵒʷ</c> (smoothed <c>%D</c>) Moving Average. The default value is 3.</param>
        /// <param name="dSlowLineIndicator">The Moving Average line indicator to calculate the <c>%Dˢˡᵒʷ</c> (smoothed <c>%D</c>).</param>
        public StochasticOscillator(int kLength, int dFastLength, ILineIndicator dFastLineIndicator, int dSlowLength, ILineIndicator dSlowLineIndicator)
            : base(Sto, StoFull)
        {
            if (2 > kLength)
                throw new ArgumentOutOfRangeException(nameof(kLength));
            if (2 > dFastLength)
                throw new ArgumentOutOfRangeException(nameof(dFastLength));
            if (2 > dSlowLength)
                throw new ArgumentOutOfRangeException(nameof(dSlowLength));
            KLength = kLength;
            DFastLength = dFastLength;
            DFastMovingAverageIndicator = dFastLineIndicator;
            DSlowLength = dSlowLength;
            DSlowMovingAverageIndicator = dSlowLineIndicator;
            Moniker = string.Concat(Sto, "(", kLength.ToString(CultureInfo.InvariantCulture), ",", dFastLineIndicator.Moniker, ",", dSlowLineIndicator.Moniker, ")");
            kLengthMinOne = kLength - 1;
            lowCircularInput = new double[kLength];
            highCircularInput = new double[kLength];
        }
        #endregion

        #region Reset
        /// <inheritdoc />
        public override void Reset()
        {
            lock (Lock)
            {
                Primed = false;
                DFastMovingAverageIndicator.Reset();
                DSlowMovingAverageIndicator.Reset();
                kValue = double.NaN;
                dFastValue = double.NaN;
                dSlowValue = double.NaN;
                time = new DateTime(0L);
                circularInputIndex = 0;
                circularInputCount = 0;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// The Stochastic Oscillator indicator can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same sample value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            return Update(sample, sample, sample);
        }

        /// <summary>
        /// The Stochastic Oscillator indicator can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same sample value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(double sample, DateTime dateTime)
        {
            return new Scalar(dateTime, Update(sample, sample, sample));
        }

        /// <summary>
        /// The Stochastic Oscillator indicator can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same scalar value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Scalar scalar)
        {
            double sample = scalar.Value;
            return new Scalar(scalar.Time, Update(sample, sample, sample));
        }

        /// <summary>
        /// Updates the value of the Stochastic Oscillator. The indicator is not primed during the first <c>ℓ</c> updates.
        /// </summary>
        /// <param name="sampleClose">The <c>close</c> value of a new sample.</param>
        /// <param name="sampleHigh">The <c>high</c> value of a new sample.</param>
        /// <param name="sampleLow">The <c>low</c> value of a new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sampleClose, double sampleHigh, double sampleLow)
        {
            if (double.IsNaN(sampleClose) || double.IsNaN(sampleHigh) || double.IsNaN(sampleLow))
                return double.NaN;
            lock (Lock)
            {
                int index = circularInputIndex;
                lowCircularInput[index] = sampleLow;
                highCircularInput[index] = sampleHigh;
                // Update circular buffer index.
                if (++circularInputIndex > kLengthMinOne)
                    circularInputIndex = 0;
                if (KLength > circularInputCount)
                {
                    if (kLengthMinOne == circularInputCount)
                    {
                        double minLow = lowCircularInput[index];
                        double maxHigh = highCircularInput[index];
                        for (int i = 0; i < kLengthMinOne; ++i)
                        {
                            // The value of the index is always positive here.
                            --index;
                            double temp = lowCircularInput[index];
                            if (minLow > temp)
                                minLow = temp;
                            temp = highCircularInput[index];
                            if (maxHigh < temp)
                                maxHigh = temp;
                        }
                        if (Math.Abs(maxHigh - minLow) < double.Epsilon)
                            kValue = 100d;
                        else
                            kValue = 100d * (sampleClose - minLow) / (maxHigh - minLow);
                        dFastValue = DFastMovingAverageIndicator.Update(kValue);
                    }
                    ++circularInputCount;
                }
                else
                {
                    double minLow = lowCircularInput[index];
                    double maxHigh = highCircularInput[index];
                    for (int i = 0; i < kLengthMinOne; ++i)
                    {
                        if (index == 0)
                            index = kLengthMinOne;
                        else
                            --index;
                        double temp = lowCircularInput[index];
                        if (minLow > temp)
                            minLow = temp;
                        temp = highCircularInput[index];
                        if (maxHigh < temp)
                            maxHigh = temp;
                    }
                    if (Math.Abs(maxHigh - minLow) < double.Epsilon)
                        kValue = 100d;
                    else
                        kValue = 100d * (sampleClose - minLow) / (maxHigh - minLow);
                    dFastValue = DFastMovingAverageIndicator.Update(kValue);
                    dSlowValue = DSlowMovingAverageIndicator.Update(dFastValue);
                    if (!Primed)
                        Primed = DSlowMovingAverageIndicator.IsPrimed && DFastMovingAverageIndicator.IsPrimed;
                }
                return kValue;
            }
        }

        /// <summary>
        /// Updates the value of the Stochastic Oscillator.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            time = ohlcv.Time;
            return new Scalar(time, Update(ohlcv.Close, ohlcv.High, ohlcv.Low));
        }
        #endregion
    }
}
