using System;
using System.Runtime.Serialization;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// A facade class to represent the histogram band of the master Moving Average Convergence/Divergence indicator as a Value property and to simulate the update.
    /// <para />
    /// Assumes the master Moving Average Convergence/Divergence indicator is updated before the update on this class is called.
    /// </summary>
    [DataContract]
    [KnownType(typeof(MovingAverageConvergenceDivergence))]
    public sealed class MovingAverageConvergenceDivergenceHistogramBand : Indicator, IBandIndicator
    {
        #region Members and accessors
        #region Value
        /// <summary>
        /// The current MACD histogram band.
        /// The <c>first</c> band element is a histogram value, the second band element is always zero.
        /// </summary>
        public Band Value { get { return movingAverageConvergenceDivergence.HistogramBand; } }
        #endregion

        #region Parent
        [DataMember]
        private readonly MovingAverageConvergenceDivergence movingAverageConvergenceDivergence;
        /// <summary>
        /// The parent <c>MovingAverageConvergenceDivergence</c> instance.
        /// </summary>
        public MovingAverageConvergenceDivergence Parent { get { return movingAverageConvergenceDivergence; } }
        #endregion

        private const string macd = "MACDh";
        private const string macdFull = "Moving Average Convergence/Divergence histogram band";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="MovingAverageConvergenceDivergenceHistogramBand"/> class.
        /// </summary>
        /// <param name="movingAverageConvergenceDivergence">The parent MovingAverageConvergenceDivergence line indicator.</param>
        public MovingAverageConvergenceDivergenceHistogramBand(MovingAverageConvergenceDivergence movingAverageConvergenceDivergence)
            : base(macd, macdFull, movingAverageConvergenceDivergence.OhlcvComponent)
        {
            this.movingAverageConvergenceDivergence = movingAverageConvergenceDivergence;
            moniker = movingAverageConvergenceDivergence.Moniker.Insert(4, "h");
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
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence histogram band.
        /// The <c>first</c> band element is a histogram value, the second band element is always zero.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Band Update(double sample, DateTime dateTime)
        {
            if (double.IsNaN(sample))
                return new Band(dateTime, double.NaN, double.NaN);
            lock (updateLock)
            {
                if (!primed)
                    primed = movingAverageConvergenceDivergence.IsPrimed;
            }
            return movingAverageConvergenceDivergence.HistogramBand;
        }

        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence histogram band.
        /// The <c>first</c> band element is a histogram value, the second band element is always zero.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Band Update(Scalar scalar)
        {
            return Update(scalar.Value, scalar.Time);
        }

        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence histogram band.
        /// The <c>first</c> band element is a histogram value, the second band element is always zero.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Band Update(Ohlcv ohlcv)
        {
            return Update(ohlcv.Component(ohlcvComponent), ohlcv.Time);
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns the string that represents this object.
        /// </summary>
        /// <returns>Returns the string that represents this object.</returns>
        public override string ToString()
        {
            return movingAverageConvergenceDivergence.ToString();
        }
        #endregion
    }
}
