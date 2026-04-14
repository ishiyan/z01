using System;
using Mbs.Trading.Data;
using Mbs.Trading.Indicators.Abstractions;

namespace Mbs.Trading.Indicators
{
    /// <summary>
    /// On-Balance Volume (OBV) is a momentum indicator that relates price change to a volume. It adds a period's volume when the close is up and subtracts the period's volume when the close is down. A cumulative total of the volume additions and subtractions forms the OBV line.
    /// <para><c>OBVᵢ = OBVᵢ₋₁ +</c></para>
    /// <para>❶│ <c> volumeᵢ, closeᵢ &gt; closeᵢ₋₁</c>,</para>
    /// <para>❷│ <c>       0, closeᵢ = closeᵢ₋₁</c>,</para>
    /// <para>❸│ <c>-volumeᵢ, closeᵢ &lt; closeᵢ₋₁</c>.</para>
    /// <para>The idea behind the OBV indicator is that changes in the volume will precede price changes. A rising volume can indicate the presence of smart money flowing into a security. Then once the public follows suit, the security's price will likewise rise. </para>
    /// <para>The technique was presented by Joseph E. Granville in his 1963 book "Granville's New Strategy of Daily Stock Market Timing for Maximum Profit", Prentice-Hall, Inc., 1976. ISBN 0-13-363432-9.</para>
    /// The indicator accepts only <c>ohlcv</c> samples and is primed after the first update.
    /// </summary>
    public sealed class OnBalanceVolume : Indicator, ILineIndicator
    {
        #region Members and accessors
        private double value = double.NaN;
        /// <summary>
        /// The current value of the the On-Balance Volume, or <c>NaN</c> if not primed.
        /// The indicator is not primed until the first update.
        /// </summary>
        public double Value { get { lock (Lock) { return value; } } }

        private double previousPrice = double.NaN;

        private const string Obv = "OBV";
        private const string ObvFull = "On-Balance Volume";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="OnBalanceVolume"/> class.
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        /// </summary>
        public OnBalanceVolume(OhlcvComponent ohlcvComponent = OhlcvComponent.ClosingPrice)
            : base(Obv, ObvFull, ohlcvComponent)
        {
            Moniker = Obv;
        }
        #endregion

        #region Reset
        /// <inheritdoc />
        public override void Reset()
        {
            lock (Lock)
            {
                Primed = false;
                previousPrice = double.NaN;
                value = double.NaN;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// The On-Balance Volume indicator can be updated only with <c>ohlcv</c> samples.
        /// In this case, the NaN value is used as a substitute for the volume.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            return Update(sample, double.NaN);
        }

        /// <summary>
        /// The On-Balance Volume indicator can be updated only with <c>ohlcv</c> samples.
        /// In this case, the NaN value is used as a substitute for the volume.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(double sample, DateTime dateTime)
        {
            return new Scalar(dateTime, Update(sample, double.NaN));
        }

        /// <summary>
        /// The On-Balance Volume indicator can be updated only with <c>ohlcv</c> samples.
        /// In this case, the NaN value is used as a substitute for the volume.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Scalar scalar)
        {
            return new Scalar(scalar.Time, Update(scalar.Value, double.NaN));
        }

        /// <summary>
        /// Updates the value of the On-Balance Volume. The indicator is primed after the first update.
        /// </summary>
        /// <param name="samplePrice">The price value of a new sample.</param>
        /// <param name="sampleVolume">The volume value of a new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double samplePrice, double sampleVolume)
        {
            if (double.IsNaN(samplePrice) || double.IsNaN(sampleVolume))
                return double.NaN;
            lock (Lock)
            {
                if (!Primed)
                {
                    Primed = true;
                    value = sampleVolume;
                }
                else
                {
                    if (samplePrice > previousPrice)
                        value += sampleVolume;
                    else if (samplePrice < previousPrice)
                        value -= sampleVolume;
                }
                previousPrice = samplePrice;
                return value;
            }
        }

        /// <summary>
        /// Updates the value of the On-Balance Volume. The indicator is primed after the first update.
        /// </summary>
        /// <param name="samplePrice">The price value of a new sample.</param>
        /// <param name="sampleVolume">The volume value of a new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(double samplePrice, double sampleVolume, DateTime dateTime)
        {
            return new Scalar(dateTime, Update(samplePrice, sampleVolume));
        }

        /// <summary>
        /// Updates the value of the On-Balance Volume. The indicator is primed after the first update.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            return new Scalar(ohlcv.Time, Update(ohlcv.Component(OhlcvComponent), ohlcv.Volume));
        }
        #endregion
    }
}
