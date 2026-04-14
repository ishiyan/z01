using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
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
    [DataContract]
    [KnownType(typeof(ExponentialMovingAverage))]
    public sealed class StochasticOscillator : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region KLength
        [DataMember]
        private readonly int kLength;
        /// <summary>
        /// The length (the number of time periods) of the <c>%K</c> line.
        /// </summary>
        public int KLength { get { return kLength; } }
        #endregion

        #region DFastLength
        [DataMember]
        private readonly int dFastLength;
        /// <summary>
        /// The equivalent length (the number of time periods) of the <c>%D</c> (the smoothed <c>%K</c>) line.
        /// </summary>
        public int DFastLength { get { return dFastLength; } }
        #endregion

        #region DSlowLength
        [DataMember]
        private readonly int dSlowLength;
        /// <summary>
        /// The equivalent length (the number of time periods) of the <c>%Dˢˡᵒʷ</c> (the smoothed <c>%D</c>) signal line.
        /// </summary>
        public int DSlowLength { get { return dSlowLength; } }
        #endregion

        #region DFastMovingAverageIndicator
        [DataMember]
        private readonly ILineIndicator dFastMovingAverage;
        /// <summary>
        /// The <c>%D</c> moving average indicator used to smooth the <c>%K</c> line.
        /// </summary>
        public ILineIndicator DFastMovingAverageIndicator { get { return dFastMovingAverage; } }
        #endregion

        #region DSlowMovingAverageIndicator
        [DataMember]
        private readonly ILineIndicator dSlowMovingAverage;
        /// <summary>
        /// The <c>%Dˢˡᵒʷ</c> moving average indicator used to smooth the <c>%D</c> line.
        /// </summary>
        public ILineIndicator DSlowMovingAverageIndicator { get { return dSlowMovingAverage; } }
        #endregion

        #region KValue
        [DataMember]
        private double kValue = double.NaN;
        /// <summary>
        /// The current value of the <c>%K</c> or <c>NaN</c> if not primed.
        /// </summary>
        public double KValue { get { lock (updateLock) { return kValue; } } }
        #endregion

        #region Value
        /// <summary>
        /// The current value of the <c>%K</c> or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (updateLock) { return kValue; } } }
        #endregion

        #region DFastValue
        [DataMember]
        private double dFastValue = double.NaN;
        /// <summary>
        /// The current value of the <c>%D</c> or <c>NaN</c> if not primed.
        /// </summary>
        public double DFastValue { get { lock (updateLock) { return dFastValue; } } }
        #endregion

        #region DSlowValue
        [DataMember]
        private double dSlowValue = double.NaN;
        /// <summary>
        /// The current value of the <c>%Dˢˡᵒʷ</c> or <c>NaN</c> if not primed.
        /// </summary>
        public double DSlowValue { get { lock (updateLock) { return dSlowValue; } } }
        #endregion

        #region KdBand
        /// <summary>
        /// The current value of the <c>%K</c> and <c>%D</c> band.
        /// The <c>first</c> band element is a <c>%K</c> value, the second band element is a <c>%D</c> value.
        /// </summary>
        public Band KdBand { get { lock(updateLock) { return new Band(time, kValue, dFastValue); } } }
        #endregion

        #region DdBand
        /// <summary>
        /// The current value of the <c>%D</c> and <c>%Dˢˡᵒʷ</c> band.
        /// The <c>first</c> band element is a <c>%D</c> value, the second band element is a <c>%Dˢˡᵒʷ</c> value.
        /// </summary>
        public Band DdBand { get { lock (updateLock) { return new Band(time, dFastValue, dSlowValue); } } }
        #endregion

        [DataMember]
        private DateTime time;
        [DataMember]
        private readonly int kLengthMinOne;
        [DataMember]
        private int circularInputIndex;
        [DataMember]
        private int circularInputCount;
        [DataMember]
        private readonly double[] lowCircularInput;
        [DataMember]
        private readonly double[] highCircularInput;

        private const double epsilon = 0.00000001;
        private const string sto = "STO";
        private const string stoFull = "Stochastic Oscillator";
        private const string kLengthArgument = "kLength";
        private const string dLengthArgument = "dFastLength";
        private const string dSlowLengthArgument = "dSlowLength";
        private const string dAlphaArgument = "dfastAlpha";
        private const string dSlowAlphaArgument = "dSlowAlpha";
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
            : base(sto, stoFull)
        {
            if (2 > kLength)
                throw new ArgumentOutOfRangeException(kLengthArgument);
            if (2 > dFastLength)
                throw new ArgumentOutOfRangeException(dLengthArgument);
            if (2 > dSlowLength)
                throw new ArgumentOutOfRangeException(dSlowLengthArgument);
            this.kLength = kLength;
            this.dFastLength = dFastLength;
            dFastMovingAverage = new ExponentialMovingAverage(dFastLength, firstIsAverage, ohlcvComponent);
            this.dSlowLength = dSlowLength;
            dSlowMovingAverage = new ExponentialMovingAverage(dSlowLength, firstIsAverage);
            moniker = string.Concat(sto, "(", kLength.ToString(CultureInfo.InvariantCulture), ",", dFastLength.ToString(CultureInfo.InvariantCulture), ",", dSlowLength.ToString(CultureInfo.InvariantCulture), ")");
            kLengthMinOne = kLength - 1;
            lowCircularInput = new double[kLength];
            highCircularInput = new double[kLength];
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="StochasticOscillator"/> class.
        /// </summary>
        /// <param name="kLength">The length (the number of time periods) of the <c>%K</c> line. The typical value are 5, 9 or 14 periods.</param>
        /// <param name="dfastAlpha">The smoothing factor, <c>α</c>, of the <c>%D</c> Exponential Moving Average (smoothed <c>%K</c>). The equivalent length <c>ℓ</c> is <para><c>ℓ = 2/α - 1, 0 &lt; α ≤ 1, 1 ≤ ℓ</c>.</para><para>The default value is 0.5 (ℓ = 3).</para></param>
        /// <param name="dSlowAlpha">The smoothing factor, <c>α</c>, of the <c>%Dˢˡᵒʷ</c> (signal) Exponential Moving Average (smoothed <c>%D</c>). The equivalent length <c>ℓ</c> is <para><c>ℓ = 2/α - 1, 0 &lt; α ≤ 1, 1 ≤ ℓ</c>.</para><para>The default value is 0.5 (ℓ = 3).</para></param>
        /// <param name="firstIsAverage">If the very first EMA value is a simple average of the first 'period' (the most widely documented approach) or the first input value (used in Metastock).</param>
        public StochasticOscillator(int kLength, double dfastAlpha, double dSlowAlpha, bool firstIsAverage = true)
            : base(sto, stoFull)
        {
            if (2 > kLength)
                throw new ArgumentOutOfRangeException(kLengthArgument);
            this.kLength = kLength;
            if (0d > dfastAlpha || 1d < dfastAlpha)
                throw new ArgumentOutOfRangeException(dAlphaArgument);
            if (epsilon > dfastAlpha)
                dFastLength = int.MaxValue;
            else
                dFastLength = (int)Math.Round(2d / dfastAlpha) - 1;
            if (0d > dSlowAlpha || 1d < dSlowAlpha)
                throw new ArgumentOutOfRangeException(dSlowAlphaArgument);
            if (epsilon > dSlowAlpha)
                dSlowLength = int.MaxValue;
            else
                dSlowLength = (int)Math.Round(2d / dSlowAlpha) - 1;
            dFastMovingAverage = new ExponentialMovingAverage(dfastAlpha, firstIsAverage, ohlcvComponent);
            dSlowMovingAverage = new ExponentialMovingAverage(dSlowAlpha, firstIsAverage, ohlcvComponent);
            moniker = string.Concat(sto, "(", kLength.ToString(CultureInfo.InvariantCulture), ",", dFastLength.ToString(CultureInfo.InvariantCulture), ",", dSlowLength.ToString(CultureInfo.InvariantCulture), ")");
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
            : base(sto, stoFull)
        {
            if (2 > kLength)
                throw new ArgumentOutOfRangeException(kLengthArgument);
            if (2 > dFastLength)
                throw new ArgumentOutOfRangeException(dLengthArgument);
            if (2 > dSlowLength)
                throw new ArgumentOutOfRangeException(dSlowLengthArgument);
            this.kLength = kLength;
            this.dFastLength = dFastLength;
            dFastMovingAverage = dFastLineIndicator;
            this.dSlowLength = dSlowLength;
            dSlowMovingAverage = dSlowLineIndicator;
            moniker = string.Concat(sto, "(", kLength.ToString(CultureInfo.InvariantCulture), ",", dFastLineIndicator.Moniker, ",", dSlowLineIndicator.Moniker, ")");
            kLengthMinOne = kLength - 1;
            lowCircularInput = new double[kLength];
            highCircularInput = new double[kLength];
        }
        #endregion

        #region Reset
        /// <summary>
        /// Resets the indicator.
        /// </summary>
        public override void Reset()
        {
            lock (updateLock)
            {
                primed = false;
                dFastMovingAverage.Reset();
                dSlowMovingAverage.Reset();
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
            lock (updateLock)
            {
                int index = circularInputIndex;
                lowCircularInput[index] = sampleLow;
                highCircularInput[index] = sampleHigh;
                // Update circular buffer index.
                if (++circularInputIndex > kLengthMinOne)
                    circularInputIndex = 0;
                if (kLength > circularInputCount)
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
                        dFastValue = dFastMovingAverage.Update(kValue);
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
                    dFastValue = dFastMovingAverage.Update(kValue);
                    dSlowValue = dSlowMovingAverage.Update(dFastValue);
                    if (!primed)
                        primed = dSlowMovingAverage.IsPrimed && dFastMovingAverage.IsPrimed;
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

        #region Overrides
        /// <summary>
        /// Returns the string that represents this object.
        /// </summary>
        /// <returns>Returns the string that represents this object.</returns>
        public override string ToString()
        {
            double k, df, ds; bool p;
            lock (updateLock)
            {
                p = primed;
                k = kValue;
                df = dFastValue;
                ds = dSlowValue;
            }
            var sb = new StringBuilder();
            sb.Append("[M:");
            sb.Append(moniker);
            sb.Append(" P:");
            sb.Append(p);
            sb.Append(" K:");
            sb.Append(k);
            sb.Append(" DF:");
            sb.Append(df);
            sb.Append(" DS:");
            sb.Append(ds);
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
