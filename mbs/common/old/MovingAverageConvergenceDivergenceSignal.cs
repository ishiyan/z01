using System;
using System.Runtime.Serialization;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// A facade class to represent the signal of the master Moving Average Convergence/Divergence indicator as a Value property and to simulate the update.
    /// <para />
    /// Assumes the master Moving Average Convergence/Divergence indicator is updated before the update on this class is called.
    /// </summary>
    [DataContract]
    [KnownType(typeof(MovingAverageConvergenceDivergence))]
    public sealed class MovingAverageConvergenceDivergenceSignal : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region Value
        /// <summary>
        /// The current value of the MACD signal or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { return movingAverageConvergenceDivergence.Signal; } }
        #endregion

        #region Parent
        [DataMember]
        private readonly MovingAverageConvergenceDivergence movingAverageConvergenceDivergence;
        /// <summary>
        /// The parent <c>MovingAverageConvergenceDivergence</c> instance.
        /// </summary>
        public MovingAverageConvergenceDivergence Parent { get { return movingAverageConvergenceDivergence; } }
        #endregion

        private const string macd = "MACDs";
        private const string macdFull = "Moving Average Convergence/Divergence signal";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="MovingAverageConvergenceDivergenceSignal"/> class.
        /// </summary>
        /// <param name="movingAverageConvergenceDivergence">The parent MovingAverageConvergenceDivergence line indicator.</param>
        public MovingAverageConvergenceDivergenceSignal(MovingAverageConvergenceDivergence movingAverageConvergenceDivergence)
            : base(macd, macdFull, movingAverageConvergenceDivergence.OhlcvComponent)
        {
            this.movingAverageConvergenceDivergence = movingAverageConvergenceDivergence;
            moniker = movingAverageConvergenceDivergence.Moniker.Insert(4, "s");
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
        /// Updates the value of the Moving Average Convergence/Divergence signal.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            if (double.IsNaN(sample))
                return double.NaN;
            lock (updateLock)
            {
                if (!primed)
                    primed = movingAverageConvergenceDivergence.IsPrimed;
            }
            return movingAverageConvergenceDivergence.Signal;
        }

        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence signal.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(double sample, DateTime dateTime)
        {
            return new Scalar(dateTime, Update(sample));
        }

        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence signal.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Scalar scalar)
        {
            return Update(scalar.Value, scalar.Time);
        }

        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence signal.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
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
