using System;
using Mbs.Trading.Data;
using Mbs.Trading.Indicators.Abstractions;

namespace Mbs.Trading.Indicators
{
    /// <summary>
    /// A facade class to represent the value-signal band of the master Moving Average Convergence/Divergence indicator as a Value property and to simulate the update.
    /// <para />
    /// Assumes the master Moving Average Convergence/Divergence indicator is updated before the update on this class is called.
    /// </summary>
    public sealed class MovingAverageConvergenceDivergenceBand : Indicator, IBandIndicator
    {
        #region Members and accessors
        /// <summary>
        /// The current MACD value and signal band.
        /// The <c>first</c> band element is a MACD value, the second band element is a signal value.
        /// </summary>
        public Band Value => movingAverageConvergenceDivergence.ValueSignalBand;

        private readonly MovingAverageConvergenceDivergence movingAverageConvergenceDivergence;
        /// <summary>
        /// The parent <c>MovingAverageConvergenceDivergence</c> instance.
        /// </summary>
        public MovingAverageConvergenceDivergence Parent => movingAverageConvergenceDivergence;

        private const string Macd = "MACD";
        private const string MacdFull = "Moving Average Convergence/Divergence band";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="MovingAverageConvergenceDivergenceBand"/> class.
        /// </summary>
        /// <param name="movingAverageConvergenceDivergence">The parent <see cref="MovingAverageConvergenceDivergence"/> line indicator.</param>
        public MovingAverageConvergenceDivergenceBand(MovingAverageConvergenceDivergence movingAverageConvergenceDivergence)
            : base(Macd, MacdFull, movingAverageConvergenceDivergence.OhlcvComponent)
        {
            this.movingAverageConvergenceDivergence = movingAverageConvergenceDivergence;
            Moniker = movingAverageConvergenceDivergence.Moniker;
        }
        #endregion

        #region Reset
        /// <inheritdoc />
        public override void Reset()
        {
            lock (Lock)
            {
                Primed = false;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence band.
        /// The <c>first</c> band element is a MACD value, the second band element is a signal value.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Band Update(double sample, DateTime dateTime)
        {
            if (double.IsNaN(sample))
                return new Band(dateTime);
            lock (Lock)
            {
                if (!Primed)
                    Primed = movingAverageConvergenceDivergence.IsPrimed;
            }
            return movingAverageConvergenceDivergence.ValueSignalBand;
        }

        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence band.
        /// The <c>first</c> band element is a MACD value, the second band element is a signal value.
        /// </summary>
        /// <param name="scalar">A new <see cref="Scalar"/>.</param>
        /// <returns>The new value of the indicator.</returns>
        public Band Update(Scalar scalar) => Update(scalar.Value, scalar.Time);

        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence band.
        /// The <c>first</c> band element is a MACD value, the second band element is a signal value.
        /// </summary>
        /// <param name="ohlcv">A new <see cref="Ohlcv"/>.</param>
        /// <returns>The new value of the indicator.</returns>
        public Band Update(Ohlcv ohlcv) => Update(ohlcv.Component(OhlcvComponent), ohlcv.Time);
        #endregion
    }
}
