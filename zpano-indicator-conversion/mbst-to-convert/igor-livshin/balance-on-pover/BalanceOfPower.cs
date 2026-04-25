using System;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
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
    [DataContract]
    public sealed class BalanceOfPower : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region Length
        [DataMember]
        private readonly int length;
        /// <summary>
        /// The equivalent length, <c>ℓ</c>, (the number of time periods) of the smoothing Moving Average
        /// or zero if the smoothing is not used (raw Balance of Power values).
        /// </summary>
        public int Length { get { return length; } }
        #endregion

        #region MovingAverageIndicator
        [DataMember]
        private readonly ILineIndicator movingAverageIndicator;
        /// <summary>
        /// The moving average indicator used to smooth the raw Balance of Power values
        /// or zero if the smoothing is not used.
        /// </summary>
        public ILineIndicator MovingAverageIndicator { get { return movingAverageIndicator; } }
        #endregion

        #region Value
        [DataMember]
        private double value = double.NaN;
        /// <summary>
        /// The current value of the Balance of Power or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (updateLock) { return value; } } }
        #endregion

        private const double epsilon = 1e-8;
        private const string bop = "Bop";
        private const string bopFull = "Balance of Power";
        private const string argumentLength = "length";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="BalanceOfPower"/> class.
        /// </summary>
        public BalanceOfPower()
            : base(bop, bopFull)
        {
            moniker = bop;
            primed = true;
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BalanceOfPower"/> class.
        /// <param name="length">The equivalent length (the number of time periods) of the smoothing Moving Average.</param>
        /// <param name="lineIndicator">The Moving Average line indicator to smooth the raw Balance of Power values.</param>
        /// </summary>
        public BalanceOfPower(int length, ILineIndicator lineIndicator)
            : base(bop, bopFull)
        {
            if (2 > length)
                throw new ArgumentOutOfRangeException(argumentLength);
            this.length = length;
            moniker = string.Concat(bop, "(", lineIndicator.Moniker, ")");
            movingAverageIndicator = lineIndicator;
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
                primed = true;
                value = double.NaN;
                if (null != movingAverageIndicator)
                    movingAverageIndicator.Reset();
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
            lock (updateLock)
            {
                double range = sampleHigh - sampleLow;
                if (range < epsilon)
                    value = 0d;
                else
                    value = (sampleClose - sampleOpen) / range;
                if (null != movingAverageIndicator)
                {
                    value = movingAverageIndicator.Update(value);
                    primed = movingAverageIndicator.IsPrimed;
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
