using System;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// The Percentage Price Oscillator (PPO) is calculated by subtracting the slower moving average from the faster moving average and then dividing the result by the slower moving average.
    /// </summary>
    [DataContract]
    [KnownType(typeof(SimpleMovingAverage))]
    public sealed class PercentagePriceOscillator : Indicator, ILineIndicator
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

        #region Value
        [DataMember]
        private double value = double.NaN;
        /// <summary>
        /// The current value of the Percentage Price Oscillator, or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (updateLock) { return value; } } }
        #endregion

        private const double epsilon = 0.00000001;
        private const string ppo = "PPO";
        private const string ppoFull = "Percentage Price Oscillator";
        private const string slowEx = "slowLength";
        private const string fastEx = "fastLength";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="PercentagePriceOscillator"/> class.
        /// </summary>
        /// <param name="slowLength">The length (the number of time periods) of the slow Simple Moving Average.</param>
        /// <param name="fastLength">The length (the number of time periods) of the fast Simple Moving Average.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public PercentagePriceOscillator(int slowLength, int fastLength, OhlcvComponent ohlcvComponent = OhlcvComponent.ClosingPrice)
            : base(ppo, ppoFull, ohlcvComponent)
        {
            if (2 > slowLength)
                throw new ArgumentOutOfRangeException(slowEx);
            if (2 > fastLength)
                throw new ArgumentOutOfRangeException(fastEx);
            this.slowLength = slowLength;
            slowMovingAverage = new SimpleMovingAverage(slowLength, ohlcvComponent);
            this.fastLength = fastLength;
            fastMovingAverage = new SimpleMovingAverage(fastLength, ohlcvComponent);
            moniker = string.Concat(ppo, "[", fastMovingAverage.Moniker, "/", slowMovingAverage.Moniker, "]");
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="PercentagePriceOscillator"/> class.
        /// </summary>
        /// <param name="slowLength">The length of the slow Moving Average line indicator.</param>
        /// <param name="slowLineIndicator">The slow Moving Average line indicator.</param>
        /// <param name="fastLength">The length of the fast Moving Average line indicator.</param>
        /// <param name="fastLineIndicator">The fast Moving Average line indicator.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public PercentagePriceOscillator(int slowLength, ILineIndicator slowLineIndicator, int fastLength, ILineIndicator fastLineIndicator, OhlcvComponent ohlcvComponent = OhlcvComponent.ClosingPrice)
            : base(ppo, ppoFull, ohlcvComponent)
        {
            if (2 > slowLength)
                throw new ArgumentOutOfRangeException(slowEx);
            if (2 > fastLength)
                throw new ArgumentOutOfRangeException(fastEx);
            this.slowLength = slowLength;
            slowMovingAverage = slowLineIndicator;
            this.fastLength = fastLength;
            fastMovingAverage = fastLineIndicator;
            moniker = string.Concat(ppo, "[", fastMovingAverage.Moniker, "/", slowMovingAverage.Moniker, "]");
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
                value = double.NaN;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the value of the Percentage Price Oscillator.
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
                primed = slowMovingAverage.IsPrimed && fastMovingAverage.IsPrimed;
                /*if (double.IsNaN(fast) || double.IsNaN(slow))
                    value = double.NaN;
                else*/ if (epsilon > Math.Abs(slow))
                    value = 0d;
                else
                    value = 100d * (fast - slow) / slow;
                return value;
            }
        }

        /// <summary>
        /// Updates the value of the Percentage Price Oscillator.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(double sample, DateTime dateTime)
        {
            return new Scalar(dateTime, Update(sample));
        }

        /// <summary>
        /// Updates the value of the Percentage Price Oscillator.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Scalar scalar)
        {
            return new Scalar(scalar.Time, Update(scalar.Value));
        }

        /// <summary>
        /// Updates the value of the Percentage Price Oscillator.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            return new Scalar(ohlcv.Time, Update(ohlcv.Component(ohlcvComponent)));
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns the string that represents this object.
        /// </summary>
        /// <returns>Returns the string that represents this object.</returns>
        public override string ToString()
        {
            double v; bool p;
            lock (updateLock)
            {
                p = primed;
                v = value;
            }
            var sb = new StringBuilder();
            sb.Append("[M:");
            sb.Append(moniker);
            sb.Append(" P:");
            sb.Append(p);
            sb.Append(" V:");
            sb.Append(v);
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
