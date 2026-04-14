using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
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
    [DataContract]
    [KnownType(typeof(ExponentialMovingAverage))]
    public sealed class MovingAverageConvergenceDivergence : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region FastLength
        [DataMember]
        private readonly int fastLength;
        /// <summary>
        /// The length (the number of time periods) of the fast moving average.
        /// </summary>
        public int FastLength { get { return fastLength; } }
        #endregion

        #region SlowLength
        [DataMember]
        private readonly int slowLength;
        /// <summary>
        /// The length (the number of time periods) of the slow moving average.
        /// </summary>
        public int SlowLength { get { return slowLength; } }
        #endregion

        #region SignalLength
        [DataMember]
        private readonly int signalLength;
        /// <summary>
        /// The length (the number of time periods) of the signal moving average.
        /// </summary>
        public int SignalLength { get { return signalLength; } }
        #endregion

        #region FastMovingAverageIndicator
        [DataMember]
        private readonly ILineIndicator fastMovingAverage;
        /// <summary>
        /// The fast moving average indicator.
        /// </summary>
        public ILineIndicator FastMovingAverageIndicator { get { return fastMovingAverage; } }
        #endregion

        #region SlowMovingAverageIndicator
        [DataMember]
        private readonly ILineIndicator slowMovingAverage;
        /// <summary>
        /// The slow moving average indicator.
        /// </summary>
        public ILineIndicator SlowMovingAverageIndicator { get { return slowMovingAverage; } }
        #endregion

        #region SignalMovingAverageIndicator
        [DataMember]
        private readonly ILineIndicator signalMovingAverage;
        /// <summary>
        /// The signal moving average indicator.
        /// </summary>
        public ILineIndicator SignalMovingAverageIndicator { get { return signalMovingAverage; } }
        #endregion

        #region Value
        [DataMember]
        private double value = double.NaN;
        /// <summary>
        /// The current value of the Moving Average Convergence/Divergence or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (updateLock) { return value; } } }
        #endregion

        #region Signal
        [DataMember]
        private double signal = double.NaN;
        /// <summary>
        /// The current value of the MACD signal or <c>NaN</c> if not primed.
        /// </summary>
        public double Signal { get { lock (updateLock) { return signal; } } }
        #endregion

        #region ValueSignalBand
        /// <summary>
        /// The current MACD value and signal band.
        /// The <c>first</c> band element is a MACD value, the second band element is a signal value.
        /// </summary>
        public Band ValueSignalBand { get { lock(updateLock) { return new Band(time, value, signal); } } }
        #endregion

        #region Histogram
        [DataMember]
        private double histogram = double.NaN;
        /// <summary>
        /// The current value of the MACD histogram or <c>NaN</c> if not primed.
        /// </summary>
        public double Histogram { get { lock (updateLock) { return histogram; } } }
        #endregion

        #region HistogramBand
        /// <summary>
        /// The current MACD histogram band.
        /// The <c>first</c> band element is a histogram value, the second band element is always zero.
        /// </summary>
        public Band HistogramBand { get { lock(updateLock) { return new Band(time, histogram, 0d); } } }
        #endregion

        [DataMember]
        private DateTime time;

        private const double epsilon = 0.00000001;
        private const string macd = "MACD";
        private const string macdFull = "Moving Average Convergence/Divergence";
        private const string slowEx = "slowLength";
        private const string fastEx = "fastLength";
        private const string signalEx = "signalLength";
        private const string slowExA = "slowAlpha";
        private const string fastExA = "fastAlpha";
        private const string signalExA = "signalAlpha";
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
            : base(macd, macdFull, ohlcvComponent)
        {
            if (2 > slowLength)
                throw new ArgumentOutOfRangeException(slowEx);
            if (2 > fastLength)
                throw new ArgumentOutOfRangeException(fastEx);
            if (2 > signalLength)
                throw new ArgumentOutOfRangeException(signalEx);
            this.slowLength = slowLength;
            slowMovingAverage = new ExponentialMovingAverage(slowLength, firstIsAverage, ohlcvComponent);
            this.fastLength = fastLength;
            fastMovingAverage = new ExponentialMovingAverage(fastLength, firstIsAverage, ohlcvComponent);
            this.signalLength = signalLength;
            signalMovingAverage = new ExponentialMovingAverage(signalLength, firstIsAverage);
            moniker = string.Concat(macd, "(", fastLength.ToString(CultureInfo.InvariantCulture), ",", slowLength.ToString(CultureInfo.InvariantCulture), ",", signalLength.ToString(CultureInfo.InvariantCulture), ")");
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
            : base(macd, macdFull, ohlcvComponent)
        {
            if (0d > slowAlpha || 1d < slowAlpha)
                throw new ArgumentOutOfRangeException(slowExA);
            if (epsilon > slowAlpha)
                slowLength = int.MaxValue;
            else
                slowLength = (int)Math.Round(2d / slowAlpha) - 1;
            if (0d > fastAlpha || 1d < fastAlpha)
                throw new ArgumentOutOfRangeException(fastExA);
            if (epsilon > fastAlpha)
                fastLength = int.MaxValue;
            else
                fastLength = (int)Math.Round(2d / fastAlpha) - 1;
            if (0d > signalAlpha || 1d < signalAlpha)
                throw new ArgumentOutOfRangeException(signalExA);
            if (epsilon > signalAlpha)
                signalLength = int.MaxValue;
            else
                signalLength = (int)Math.Round(2d / signalAlpha) - 1;
            slowMovingAverage = new ExponentialMovingAverage(slowAlpha, firstIsAverage, ohlcvComponent);
            fastMovingAverage = new ExponentialMovingAverage(fastAlpha, firstIsAverage, ohlcvComponent);
            signalMovingAverage = new ExponentialMovingAverage(signalAlpha, firstIsAverage);
            moniker = string.Concat(macd, "(", fastLength.ToString(CultureInfo.InvariantCulture), ",", slowLength.ToString(CultureInfo.InvariantCulture), ",", signalLength.ToString(CultureInfo.InvariantCulture), ")");
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
            : base(macd, macdFull, ohlcvComponent)
        {
            if (2 > slowLength)
                throw new ArgumentOutOfRangeException(slowEx);
            if (2 > fastLength)
                throw new ArgumentOutOfRangeException(fastEx);
            if (2 > signalLength)
                throw new ArgumentOutOfRangeException(signalEx);
            this.slowLength = slowLength;
            slowMovingAverage = slowLineIndicator;
            this.fastLength = fastLength;
            fastMovingAverage = fastLineIndicator;
            this.signalLength = signalLength;
            signalMovingAverage = signalLineIndicator;
            moniker = string.Concat(macd, "(", fastMovingAverage.Moniker, ",", slowMovingAverage.Moniker, ",", signalMovingAverage.Moniker, ")");
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
                fastMovingAverage.Reset();
                slowMovingAverage.Reset();
                signalMovingAverage.Reset();
                value = double.NaN;
                signal = double.NaN;
                histogram = double.NaN;
                time = new DateTime(0L);
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            if (double.IsNaN(sample))
                return sample;
            lock (updateLock)
            {
                double slow = slowMovingAverage.Update(sample);
                double fast = fastMovingAverage.Update(sample);
                if (double.IsNaN(fast) || double.IsNaN(slow))
                {
                    value = double.NaN;
                    signal = double.NaN;
                    histogram = double.NaN;
                }
                else
                {
                    value = fast - slow;
                    signal = signalMovingAverage.Update(value);
                    histogram = value - signal;
                }
                if (!primed)
                    primed = signalMovingAverage.IsPrimed;
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

        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Scalar scalar)
        {
            time = scalar.Time;
            return new Scalar(time, Update(scalar.Value));
        }

        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            time = ohlcv.Time;
            return new Scalar(time, Update(ohlcv.Component(ohlcvComponent)));
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns the string that represents this object.
        /// </summary>
        /// <returns>Returns the string that represents this object.</returns>
        public override string ToString()
        {
            double v, s, h; bool p;
            lock (updateLock)
            {
                p = primed;
                v = value;
                s = signal;
                h = histogram;
            }
            var sb = new StringBuilder();
            sb.Append("[M:");
            sb.Append(moniker);
            sb.Append(" P:");
            sb.Append(p);
            sb.Append(" V:");
            sb.Append(v);
            sb.Append(" S:");
            sb.Append(s);
            sb.Append(" H:");
            sb.Append(h);
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
