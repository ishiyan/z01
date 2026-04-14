using System;
using Mbs.Trading.Data;
using Mbs.Trading.Indicators.Abstractions;

namespace Mbs.Trading.Indicators
{
    /// <summary>
    /// A facade class to represent the %K-%D(fast) band of the parent Stochastic Oscillator indicator as a Value property and to simulate the update.
    /// <para />
    /// Assumes the parent Stochastic Oscillator indicator is updated before the update on this class is called.
    /// </summary>
    public sealed class StochasticOscillatorKdBand : Indicator, IBandIndicator
    {
        #region Members and accessors
        /// <summary>
        /// The current Stochastic Oscillator %K-%D(fast) band.
        /// The <c>first</c> band element is the %K value, the <c>second</c> band element is the %D(fast) value.
        /// </summary>
        public Band Value => Parent.KdBand;

        /// <summary>
        /// The parent <c>StochasticOscillator</c> instance.
        /// </summary>
        public StochasticOscillator Parent { get; }

        private const string Sto = "STO%K%D";
        private const string StoFull = "Stochastic Oscillator %K - %D (fast) band";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="StochasticOscillatorKdBand"/> class.
        /// </summary>
        /// <param name="stochasticOscillator">The parent StochasticOscillator line indicator.</param>
        public StochasticOscillatorKdBand(StochasticOscillator stochasticOscillator)
            : base(Sto, StoFull, stochasticOscillator.OhlcvComponent)
        {
            Parent = stochasticOscillator;
            Moniker = stochasticOscillator.Moniker.Insert(3, "%K%D");
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
        /// Updates the Stochastic Oscillator %K-%D(fast) band.
        /// The <c>first</c> band element is the %K value, the <c>second</c> band element is the %D(fast) value.
        /// The Stochastic Oscillator indicator can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same sample value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Band Update(double sample, DateTime dateTime)
        {
            return Update(sample, sample, sample, dateTime);
        }

        /// <summary>
        /// Updates the Stochastic Oscillator %K-%D(fast) band.
        /// The <c>first</c> band element is the %K value, the <c>second</c> band element is the %D(fast) value.
        /// The Stochastic Oscillator indicator can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same scalar value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Band Update(Scalar scalar)
        {
            double sample = scalar.Value;
            return Update(sample, sample, sample, scalar.Time);
        }

        /// <summary>
        /// Updates the Stochastic Oscillator %K-%D(fast) band.
        /// The <c>first</c> band element is the %K value, the <c>second</c> band element is the %D(fast) value.
        /// </summary>
        /// <param name="sampleClose">The <c>close</c> value of a new sample.</param>
        /// <param name="sampleHigh">The <c>high</c> value of a new sample.</param>
        /// <param name="sampleLow">The <c>low</c> value of a new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Band Update(double sampleClose, double sampleHigh, double sampleLow, DateTime dateTime)
        {
            if (double.IsNaN(sampleClose) || double.IsNaN(sampleHigh) || double.IsNaN(sampleLow))
                return new Band(dateTime);

            lock (Lock)
            {
                if (!Primed)
                    Primed = Parent.IsPrimed;
            }
            return Parent.KdBand;
        }

        /// <summary>
        /// Updates the Stochastic Oscillator %K-%D(fast) band.
        /// The <c>first</c> band element is the %K value, the <c>second</c> band element is the %D(fast) value.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Band Update(Ohlcv ohlcv)
        {
            return Update(ohlcv.Close, ohlcv.High, ohlcv.Low, ohlcv.Time);
        }
        #endregion
    }
}
