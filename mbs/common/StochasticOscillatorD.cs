using System;
using Mbs.Trading.Data;
using Mbs.Trading.Indicators.Abstractions;

namespace Mbs.Trading.Indicators
{
    /// <summary>
    /// A facade class to represent the %D (fast) value of the parent Stochastic Oscillator indicator instance as a Value property and to simulate the update.
    /// <para />
    /// Assumes the parent StochasticOscillator indicator instance is updated before the update on this class is called.
    /// </summary>
    public sealed class StochasticOscillatorD : Indicator, ILineIndicator
    {
        #region Members and accessors
        /// <summary>
        /// The current value of the %D (fast) or <c>NaN</c> if not primed.
        /// </summary>
        public double Value => Parent.DFastValue;

        /// <summary>
        /// The parent <c>StochasticOscillator</c> instance.
        /// </summary>
        public StochasticOscillator Parent { get; }

        private const string Sto = "STO%D";
        private const string StoFull = "Stochastic Oscillator %D";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="StochasticOscillatorD"/> class.
        /// </summary>
        /// <param name="stochasticOscillator">The parent StochasticOscillator line indicator.</param>
        public StochasticOscillatorD(StochasticOscillator stochasticOscillator)
            : base(Sto, StoFull, stochasticOscillator.OhlcvComponent)
        {
            Parent = stochasticOscillator;
            Moniker = stochasticOscillator.Moniker.Insert(3, "%D");
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
        /// Updates the value of the Stochastic Oscillator %D (fast).
        /// </summary>
        /// <param name="sampleClose">The <c>close</c> value of a new sample.</param>
        /// <param name="sampleHigh">The <c>high</c> value of a new sample.</param>
        /// <param name="sampleLow">The <c>low</c> value of a new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sampleClose, double sampleHigh, double sampleLow)
        {
            if (double.IsNaN(sampleClose) || double.IsNaN(sampleHigh) || double.IsNaN(sampleLow))
                return double.NaN;

            lock (Lock)
            {
                if (!Primed)
                    Primed = Parent.IsPrimed;
            }
            return Parent.DFastValue;
        }

        /// <summary>
        /// Updates the value of the Stochastic Oscillator %D (fast). The indicator is not primed during the first <c>ℓ</c> updates.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            return new Scalar(ohlcv.Time, Update(ohlcv.Close, ohlcv.High, ohlcv.Low));
        }
        #endregion
    }
}
