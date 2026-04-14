using System;
using Mbs.Trading.Data;
using Mbs.Trading.Indicators.Abstractions;

namespace Mbs.Trading.Indicators
{
    /// <summary>
    /// The Absolute Price Oscillator (APO) is an indicator based on the difference between two moving averages, expressed in absolute terms.
    /// The indicator is calculated by subtracting the longer moving average from the shorter moving average.
    /// </summary>
    public sealed class AbsolutePriceOscillator : Indicator, ILineIndicator
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
        /// The fast moving average indicator.
        /// </summary>
        public ILineIndicator FastMovingAverageIndicator { get; }

        /// <summary>
        /// The slow moving average indicator.
        /// </summary>
        public ILineIndicator SlowMovingAverageIndicator { get; }

        private double value = double.NaN;
        /// <summary>
        /// The current value of the Absolute Price Oscillator, or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (Lock) { return value; } } }

        private const string Apo = "APO";
        private const string ApoFull = "Absolute Price Oscillator";
        private const string SlowEx = "slowLength";
        private const string FastEx = "fastLength";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="AbsolutePriceOscillator"/> class.
        /// </summary>
        /// <param name="slowLength">The length (the number of time periods) of the underlying slow Simple Moving Average.</param>
        /// <param name="fastLength">The length (the number of time periods) of the underlying fast Simple Moving Average.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public AbsolutePriceOscillator(int slowLength, int fastLength, OhlcvComponent ohlcvComponent = OhlcvComponent.ClosingPrice)
            : base(Apo, ApoFull, ohlcvComponent)
        {
            if (2 > slowLength)
                throw new ArgumentOutOfRangeException(SlowEx);
            if (2 > fastLength)
                throw new ArgumentOutOfRangeException(FastEx);
            SlowLength = slowLength;
            SlowMovingAverageIndicator = new SimpleMovingAverage(slowLength, ohlcvComponent);
            FastLength = fastLength;
            FastMovingAverageIndicator = new SimpleMovingAverage(fastLength, ohlcvComponent);
            Moniker = string.Concat(Apo, "[", FastMovingAverageIndicator.Moniker, "/", SlowMovingAverageIndicator.Moniker, "]");
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="AbsolutePriceOscillator"/> class.
        /// </summary>
        /// <param name="slowLength">The length of the slow Moving Average line indicator.</param>
        /// <param name="slowLineIndicator">The slow Moving Average line indicator.</param>
        /// <param name="fastLength">The length of the fast Moving Average line indicator.</param>
        /// <param name="fastLineIndicator">The fast Moving Average line indicator.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public AbsolutePriceOscillator(int slowLength, ILineIndicator slowLineIndicator, int fastLength, ILineIndicator fastLineIndicator, OhlcvComponent ohlcvComponent = OhlcvComponent.ClosingPrice)
            : base(Apo, ApoFull, ohlcvComponent)
        {
            if (2 > slowLength)
                throw new ArgumentOutOfRangeException(SlowEx);
            if (2 > fastLength)
                throw new ArgumentOutOfRangeException(FastEx);
            SlowLength = slowLength;
            SlowMovingAverageIndicator = slowLineIndicator;
            FastLength = fastLength;
            FastMovingAverageIndicator = fastLineIndicator;
            Moniker = string.Concat(Apo, "[", FastMovingAverageIndicator.Moniker, "/", SlowMovingAverageIndicator.Moniker, "]");
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
                value = double.NaN;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the value of the Absolute Price Oscillator.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            if (double.IsNaN(sample))
                return sample;
            lock (Lock)
            {
                double slow = SlowMovingAverageIndicator.Update(sample);
                double fast = FastMovingAverageIndicator.Update(sample);
                Primed = SlowMovingAverageIndicator.IsPrimed && FastMovingAverageIndicator.IsPrimed;
                value = fast - slow;
                return value;
            }
        }

        /// <summary>
        /// Updates the value of the Absolute Price Oscillator.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(double sample, DateTime dateTime)
        {
            return new Scalar(dateTime, Update(sample));
        }

        /// <summary>
        /// Updates the value of the Absolute Price Oscillator.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Scalar scalar)
        {
            return new Scalar(scalar.Time, Update(scalar.Value));
        }

        /// <summary>
        /// Updates the value of the Absolute Price Oscillator.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            return new Scalar(ohlcv.Time, Update(ohlcv.Component(OhlcvComponent)));
        }
        #endregion
    }
}
