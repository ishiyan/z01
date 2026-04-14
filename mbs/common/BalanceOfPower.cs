using System;
using Mbs.Trading.Data;
using Mbs.Trading.Indicators.Abstractions;

namespace Mbs.Trading.Indicators
{
    /// <summary>
    /// The Balance of Market Power, created by Igor Livshin, is an indicator that captures the struggles of bulls vs. bears throughout the trading day.
    /// It assigns scores to both bulls and bears based on how much they were able to move prices throughout the trading day.
    /// <para />
    /// The value is calculated as
    /// <para />
    /// BOPᵢ = (Closeᵢ - Openᵢ) / (Highᵢ - Lowᵢ)
    /// <para />
    /// Balance of Market Power is normally smoothed with a moving average.
    /// Livshin recommends a 14 bar simple moving average, but different periods and moving average types can be used for different markets.
    /// <para />
    /// During Bull markets, the indicator's tops usually cluster around the upper level of the range. This is reversed during Bear markets..
    /// <para />
    /// You can look for divergences between the indicator and the underlying price to spot potential trend reversals.
    /// </summary>
    public sealed class BalanceOfPower : Indicator, ILineIndicator
    {
        #region Members and accessors
        /// <summary>
        /// The equivalent length, <c>ℓ</c>, (the number of time periods) of the smoothing Moving Average
        /// or zero if the smoothing is not used (raw Balance of Power values).
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// The moving average indicator used to smooth the raw Balance of Power values
        /// or zero if the smoothing is not used.
        /// </summary>
        public ILineIndicator MovingAverageIndicator { get; }

        private double value = double.NaN;
        /// <summary>
        /// The current value of the Balance of Power or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (Lock) { return value; } } }

        private const double Epsilon = 1e-8;
        private const string Bop = "Bop";
        private const string BopFull = "Balance of Power";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="BalanceOfPower"/> class.
        /// </summary>
        public BalanceOfPower()
            : base(Bop, BopFull)
        {
            Moniker = Bop;
            Primed = true;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BalanceOfPower"/> class.
        /// <param name="length">The equivalent length (the number of time periods) of the smoothing Moving Average.</param>
        /// <param name="lineIndicator">The Moving Average line indicator to smooth the raw Balance of Power values.</param>
        /// </summary>
        public BalanceOfPower(int length, ILineIndicator lineIndicator)
            : base(Bop, BopFull)
        {
            if (2 > length)
                throw new ArgumentOutOfRangeException(nameof(length));
            Length = length;
            Moniker = string.Concat(Bop, "(", lineIndicator.Moniker, ")");
            MovingAverageIndicator = lineIndicator;
        }
        #endregion

        #region Reset
        /// <inheritdoc />
        public override void Reset()
        {
            lock (Lock)
            {
                Primed = true;
                value = double.NaN;
                MovingAverageIndicator?.Reset();
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// The Balance of Power indicator can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same sample value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            return Update(sample, sample, sample, sample);
        }

        /// <summary>
        /// The Balance of Power indicator can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same sample value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(double sample, DateTime dateTime)
        {
            return new Scalar(dateTime, Update(sample, sample, sample, sample));
        }

        /// <summary>
        /// The Balance of Power indicator can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same scalar value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Scalar scalar)
        {
            double sample = scalar.Value;
            return new Scalar(scalar.Time, Update(sample, sample, sample, sample));
        }

        /// <summary>
        /// Updates the value of the Balance of Power.
        /// </summary>
        /// <param name="sampleOpen">The <c>open</c> value of a new sample.</param>
        /// <param name="sampleClose">The <c>close</c> value of a new sample.</param>
        /// <param name="sampleHigh">The <c>high</c> value of a new sample.</param>
        /// <param name="sampleLow">The <c>low</c> value of a new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sampleOpen, double sampleHigh, double sampleLow, double sampleClose)
        {
            if (double.IsNaN(sampleOpen) || double.IsNaN(sampleClose) || double.IsNaN(sampleHigh) || double.IsNaN(sampleLow))
                return double.NaN;
            lock (Lock)
            {
                double range = sampleHigh - sampleLow;
                if (range < Epsilon)
                    value = 0d;
                else
                    value = (sampleClose - sampleOpen) / range;
                if (null != MovingAverageIndicator)
                {
                    value = MovingAverageIndicator.Update(value);
                    Primed = MovingAverageIndicator.IsPrimed;
                }
                return value;
            }
        }

        /// <summary>
        /// Updates the value of the Balance of Power.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            return new Scalar(ohlcv.Time, Update(ohlcv.Open, ohlcv.High, ohlcv.Low, ohlcv.Close));
        }
        #endregion
    }
}
