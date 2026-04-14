using System;
using Mbs.Trading.Data;
using Mbs.Trading.Indicators.Abstractions;

namespace Mbs.Trading.Indicators
{
    /// <summary>
    /// A facade class to represent the histogram band of the master Moving Average Convergence/Divergence indicator as a Value property and to simulate the update.
    /// <para />
    /// Assumes the master Moving Average Convergence/Divergence indicator is updated before the update on this class is called.
    /// </summary>
    public sealed class MovingAverageConvergenceDivergenceHistogramBand : Indicator, IBandIndicator
    {
        #region Members and accessors
        /// <summary>
        /// The current MACD histogram band.
        /// The <c>first</c> band element is a histogram value, the second band element is always zero.
        /// </summary>
        public Band Value => Parent.HistogramBand;

        /// <summary>
        /// The parent <c>MovingAverageConvergenceDivergence</c> instance.
        /// </summary>
        public MovingAverageConvergenceDivergence Parent { get; }

        private const string Macd = "MACDh";
        private const string MacdFull = "Moving Average Convergence/Divergence histogram band";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="MovingAverageConvergenceDivergenceHistogramBand"/> class.
        /// </summary>
        /// <param name="movingAverageConvergenceDivergence">The parent MovingAverageConvergenceDivergence line indicator.</param>
        public MovingAverageConvergenceDivergenceHistogramBand(MovingAverageConvergenceDivergence movingAverageConvergenceDivergence)
            : base(Macd, MacdFull, movingAverageConvergenceDivergence.OhlcvComponent)
        {
            this.Parent = movingAverageConvergenceDivergence;
            Moniker = movingAverageConvergenceDivergence.Moniker.Insert(4, "h");
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
        /// Updates the value of the Moving Average Convergence/Divergence histogram band.
        /// The <c>first</c> band element is a histogram value, the second band element is always zero.
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
                    Primed = Parent.IsPrimed;
            }
            return Parent.HistogramBand;
        }

        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence histogram band.
        /// The <c>first</c> band element is a histogram value, the second band element is always zero.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Band Update(Scalar scalar) => Update(scalar.Value, scalar.Time);

        /// <summary>
        /// Updates the value of the Moving Average Convergence/Divergence histogram band.
        /// The <c>first</c> band element is a histogram value, the second band element is always zero.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Band Update(Ohlcv ohlcv) => Update(ohlcv.Component(OhlcvComponent), ohlcv.Time);
        #endregion
    }
}
