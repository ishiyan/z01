using System;
using System.Globalization;
using Mbs.Trading.Data;
using Mbs.Trading.Indicators.Abstractions;

namespace Mbs.Trading.Indicators
{
    /// <summary>
    /// Developed by Gerald Appel in the late 1970s, Moving Average Convergence/Divergence (MACD) shows the difference between a fast and slow exponential moving average (EMA) of closing prices.
    /// The standard periods originally published by Gerald Appel are 12 and 26 days
    /// <para><c>MACDᵢ = EMA12ᵢ(close) - EMA26ᵢ(close)</c>.</para>
    /// A signal line (or trigger line) is then formed by smoothing this with a further EMA, functioning as a trigger for buy and sell signals. The standard period for this is 9 days,
    /// <para><c>SIGNALᵢ = EMA9ᵢ(MACD))</c>.</para>
    /// The difference between the MACD and the signal line is often calculated and shown not as a line, but a solid block histogram style. This construction was made by Thomas Aspray in 1986. The calculation is simply
    /// <para><c>HISTOGRAMᵢ = MACDᵢ - SIGNALᵢ</c>.</para>
    /// Thomas Aspray added a histogram to the MACD in 1986, as a means to anticipate MACD crossovers.
    /// <para />
    /// See
    /// <para>Appel, Gerald (1999). Technical Analysis Power Tools for Active Investors. Financial Times Prentice Hall. pp. 166. ISBN 0131479024.</para>
    /// <para>Murphy, John (1999). Technical Analysis of the Financial Markets. Prentice Hall Press. pp. 252–255. ISBN 0735200661.</para>
    /// <para>The very first EMA value (the seed for subsequent values) is calculated differently. This implementation allows for two algorithms for this seed.</para>
    /// <para>❶ Use a simple average of the first 'period'. This is the most widely documented approach.</para>
    /// <para>❷ Use first sample value as a seed. This is used in Metastock.</para>
    /// </summary>
    public sealed class MovingAverageConvergenceDivergence : Indicator, ILineIndicator
    {
        #region Members and accessors
        /// <summary>
        /// The length (the number of time periods) of the fast moving average.
        /// </summary>
        public int FastLength { get; }

        /// <summary>
        /// The length (the number of time periods) of the slow moving average.
        /// </summary>
        public int SlowLength { get; }

        /// <summary>
        /// The length (the number of time periods) of the signal moving average.
        /// </summary>
        public int SignalLength { get; }

        /// <summary>
        /// The fast moving average indicator.
        /// </summary>
        public ILineIndicator FastMovingAverageIndicator { get; }

        /// <summary>
        /// The slow moving average indicator.
        /// </summary>
        public ILineIndicator SlowMovingAverageIndicator { get; }

        /// <summary>
        /// The signal moving average indicator.
        /// </summary>
        public ILineIndicator SignalMovingAverageIndicator { get; }

        private double value = double.NaN;
        /// <summary>
        /// The current value of the Moving Average Convergence/Divergence or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (Lock) { return value; } } }

        private double signal = double.NaN;
        /// <summary>
        /// The current value of the MACD signal or <c>NaN</c> if not primed.
        /// </summary>
        public double Signal { get { lock (Lock) { return signal; } } }

        /// <summary>
        /// The current MACD value and signal band.
        /// The <c>first</c> band element is a MACD value, the second band element is a signal value.
        /// </summary>
        public Band ValueSignalBand { get { lock(Lock) { return new Band(time, value, signal); } } }

        private double histogram = double.NaN;
        /// <summary>
        /// The current value of the MACD histogram or <c>NaN</c> if not primed.
        /// </summary>
        public double Histogram { get { lock (Lock) { return histogram; } } }

        /// <summary>
        /// The current MACD histogram band.
        /// The <c>first</c> band element is a histogram value, the second band element is always zero.
        /// </summary>
        public Band HistogramBand { get { lock(Lock) { return new Band(time, histogram, 0d); } } }

        private DateTime time;

        private const double Epsilon = 0.00000001;
        private const string Macd = "MACD";
        private const string MacdFull = "Moving Average Convergence/Divergence";
        private const string SlowEx = "slowLength";
        private const string FastEx = "fastLength";
        private const string SignalEx = "signalLength";
        private const string SlowExA = "slowAlpha";
        private const string FastExA = "fastAlpha";
        private const string SignalExA = "signalAlpha";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="MovingAverageConvergenceDivergence"/> class.
        /// </summary>
        /// <param name="slowLength">The length (the number of time periods) of the slow Exponential Moving Average. The default value is 26.</param>
        /// <param name="fastLength">The length (the number of time periods) of the fast Exponential Moving Average. The default value is 12.</param>
        /// <param name="signalLength">The length (the number of time periods) of the signal Exponential Moving Average. The default value is 9.</param>
        /// <param name="firstIsAverage">If the very first EMA value is a simple average of the first 'period' or the first input value.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public MovingAverageConvergenceDivergence(int slowLength = 26, int fastLength = 12, int signalLength = 9, bool firstIsAverage = true, OhlcvComponent ohlcvComponent = OhlcvComponent.ClosingPrice)
            : base(Macd, MacdFull, ohlcvComponent)
        {
            if (2 > slowLength)
                throw new ArgumentOutOfRangeException(SlowEx);
            if (2 > fastLength)
                throw new ArgumentOutOfRangeException(FastEx);
            if (2 > signalLength)
                throw new ArgumentOutOfRangeException(SignalEx);
            SlowLength = slowLength;
            SlowMovingAverageIndicator = new ExponentialMovingAverage(slowLength, firstIsAverage, ohlcvComponent);
            FastLength = fastLength;
            FastMovingAverageIndicator = new ExponentialMovingAverage(fastLength, firstIsAverage, ohlcvComponent);
            SignalLength = signalLength;
            SignalMovingAverageIndicator = new ExponentialMovingAverage(signalLength, firstIsAverage);
            Moniker = string.Concat(Macd, "(", fastLength.ToString(CultureInfo.InvariantCulture), ",", slowLength.ToString(CultureInfo.InvariantCulture), ",", signalLength.ToString(CultureInfo.InvariantCulture), ")");
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="MovingAverageConvergenceDivergence"/> class.
        /// </summary>
        /// <param name="slowAlpha">The smoothing factor, <c>α</c>, of the slow Exponential Moving Average. The equivalent length <c>ℓ</c> is <para><c>ℓ = 2/α - 1, 0 &lt; α ≤ 1, 1 ≤ ℓ</c></para></param>
        /// <param name="fastAlpha">The smoothing factor, <c>α</c>, of the fast Exponential Moving Average. The equivalent length <c>ℓ</c> is <para><c>ℓ = 2/α - 1, 0 &lt; α ≤ 1, 1 ≤ ℓ</c></para></param>
        /// <param name="signalAlpha">The smoothing factor, <c>α</c>, of the signal Exponential Moving Average. The equivalent length <c>ℓ</c> is <para><c>ℓ = 2/α - 1, 0 &lt; α ≤ 1, 1 ≤ ℓ</c></para></param>
        /// <param name="firstIsAverage">If the very first EMA value is a simple average of the first 'period' or the first input value.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public MovingAverageConvergenceDivergence(double slowAlpha, double fastAlpha, double signalAlpha, bool firstIsAverage = true, OhlcvComponent ohlcvComponent = OhlcvComponent.ClosingPrice)
            : base(Macd, MacdFull, ohlcvComponent)
        {
            if (0d > slowAlpha || 1d < slowAlpha)
                throw new ArgumentOutOfRangeException(SlowExA);
            if (Epsilon > slowAlpha)
                SlowLength = int.MaxValue;
            else
                SlowLength = (int)Math.Round(2d / slowAlpha) - 1;
            if (0d > fastAlpha || 1d < fastAlpha)
                throw new ArgumentOutOfRangeException(FastExA);
            if (Epsilon > fastAlpha)
                FastLength = int.MaxValue;
            else
                FastLength = (int)Math.Round(2d / fastAlpha) - 1;
            if (0d > signalAlpha || 1d < signalAlpha)
                throw new ArgumentOutOfRangeException(SignalExA);
            if (Epsilon > signalAlpha)
                SignalLength = int.MaxValue;
            else
                SignalLength = (int)Math.Round(2d / signalAlpha) - 1;
            SlowMovingAverageIndicator = new ExponentialMovingAverage(slowAlpha, firstIsAverage, ohlcvComponent);
            FastMovingAverageIndicator = new ExponentialMovingAverage(fastAlpha, firstIsAverage, ohlcvComponent);
            SignalMovingAverageIndicator = new ExponentialMovingAverage(signalAlpha, firstIsAverage);
            Moniker = string.Concat(Macd, "(", FastLength.ToString(CultureInfo.InvariantCulture), ",", SlowLength.ToString(CultureInfo.InvariantCulture), ",", SignalLength.ToString(CultureInfo.InvariantCulture), ")");
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="MovingAverageConvergenceDivergence"/> class.
        /// </summary>
        /// <param name="slowLength">The length of the slow Moving Average line indicator.</param>
        /// <param name="slowLineIndicator">The slow Moving Average line indicator.</param>
        /// <param name="fastLength">The length of the fast Moving Average line indicator.</param>
        /// <param name="fastLineIndicator">The fast Moving Average line indicator.</param>
        /// <param name="signalLength">The length of the signal Moving Average line indicator.</param>
        /// <param name="signalLineIndicator">The signal Moving Average line indicator.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public MovingAverageConvergenceDivergence(int slowLength, ILineIndicator slowLineIndicator, int fastLength, ILineIndicator fastLineIndicator, int signalLength, ILineIndicator signalLineIndicator, OhlcvComponent ohlcvComponent = OhlcvComponent.ClosingPrice)
            : base(Macd, MacdFull, ohlcvComponent)
        {
            if (2 > slowLength)
                throw new ArgumentOutOfRangeException(SlowEx);
            if (2 > fastLength)
                throw new ArgumentOutOfRangeException(FastEx);
            if (2 > signalLength)
                throw new ArgumentOutOfRangeException(SignalEx);
            SlowLength = slowLength;
            SlowMovingAverageIndicator = slowLineIndicator;
            FastLength = fastLength;
            FastMovingAverageIndicator = fastLineIndicator;
            SignalLength = signalLength;
            SignalMovingAverageIndicator = signalLineIndicator;
            Moniker = string.Concat(Macd, "(", FastMovingAverageIndicator.Moniker, ",", SlowMovingAverageIndicator.Moniker, ",", SignalMovingAverageIndicator.Moniker, ")");
        }
        #endregion

        #region Reset
        /// <inheritdoc />
        public override void Reset()
        {
            lock (Lock)
            {
                Primed = false;
                FastMovingAverageIndicator.Reset();
                SlowMovingAverageIndicator.Reset();
                SignalMovingAverageIndicator.Reset();
                value = double.NaN;
                signal = double.NaN;
                histogram = double.NaN;
                time = new DateTime(0L);
            }
        }
        #endregion

        #region Update
        /// <inheritdoc />
        public double Update(double sample)
        {
            if (double.IsNaN(sample))
                return sample;
            lock (Lock)
            {
                double slow = SlowMovingAverageIndicator.Update(sample);
                double fast = FastMovingAverageIndicator.Update(sample);
                if (double.IsNaN(fast) || double.IsNaN(slow))
                {
                    value = double.NaN;
                    signal = double.NaN;
                    histogram = double.NaN;
                }
                else
                {
                    value = fast - slow;
                    signal = SignalMovingAverageIndicator.Update(value);
                    histogram = value - signal;
                }
                if (!Primed)
                    Primed = SignalMovingAverageIndicator.IsPrimed;
                return value;
            }
        }

        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(double sample, DateTime dateTime)
        {
            time = dateTime;
            return new Scalar(time, Update(sample));
        }

        /// <inheritdoc />
        public Scalar Update(Scalar scalar)
        {
            time = scalar.Time;
            return new Scalar(time, Update(scalar.Value));
        }

        /// <inheritdoc />
        public Scalar Update(Ohlcv ohlcv)
        {
            time = ohlcv.Time;
            return new Scalar(time, Update(ohlcv.Component(OhlcvComponent)));
        }
        #endregion
    }
}
