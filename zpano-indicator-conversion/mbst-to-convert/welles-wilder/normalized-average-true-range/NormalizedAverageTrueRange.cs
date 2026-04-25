using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Normalized Average True Range (NATR) is calculated as
    /// <para><c>NATR = (ATR / CLOSE) * 100</c>, where <c>ATR</c> is the Average True Range.</para>
    /// This method have an unstable period comparable to an Exponential Moving Average.
    /// <para>The True Range is defined by Welles Wilder as the largest of:</para>
    /// <para>❶ the distance from today's <c>high</c> to today's <c>low</c></para>
    /// <para>❷ the distance from yesterday's <c>close</c> to today's <c>high</c></para>
    /// <para>❸ the distance from yesterday's <c>close</c> to today's <c>low</c>.</para>
    /// <para>The True Range is the basis of the Average True Range indicator.</para>
    /// <para>This implementation calculates the very first True Range value as the <c>high - low</c> of the first bar. However, the True Range is not primed untill the second bar.</para>
    /// The indicator accepts only <c>ohlcv</c> samples and is not primed during the first <c>ℓ</c> updates.
    /// <para>See "Cross-Market Evaluations With Normalized Average True Range" by John Forman, "Technical Analyses of Stocks &amp; Commodities" (TASC), May 2006, pp.60-63.</para>
    /// </summary>
    [DataContract]
    public sealed class NormalizedAverageTrueRange : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region Length
        [DataMember]
        private readonly int length;
        /// <summary>
        /// The length, <c>ℓ</c>, (the number of time periods) of the Normalized Average True Range.
        /// </summary>
        public int Length { get { return length; } }
        #endregion

        #region Value
        [DataMember]
        private double value = double.NaN;
        /// <summary>
        /// The current value of the the Normalized Average True Range, or <c>NaN</c> if not primed.
        /// The indicator is not primed during the first <c>ℓ</c> updates, where <c>ℓ</c> is the length.
        /// </summary>
        public double Value { get { lock (updateLock) { return primed ? value : double.NaN; } } }
        #endregion

        #region TrueRange
        /// <summary>
        /// The current value of the the True Range, or <c>NaN</c> if not primed.
        /// The indicator is not primed during the first <c>ℓ</c> updates, where <c>ℓ</c> is the length.
        /// </summary>
        public double TrueRange { get { return averageTrueRange.TrueRange; } }
        #endregion

        #region AverageTrueRange
        /// <summary>
        /// The current value of the the Average True Range, or <c>NaN</c> if not primed.
        /// The indicator is not primed during the first <c>ℓ</c> updates, where <c>ℓ</c> is the length.
        /// </summary>
        public double AverageTrueRange { get { return averageTrueRange.Value; } }
        #endregion

        #region ValueFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Normalized Average True Range as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Normalized Average True Range from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade ValueFacade
        {
            get
            {
                return new LineIndicatorFacade(natr, moniker, natrFull, () => IsPrimed, () => Value);
            }
        }
        #endregion

        #region AverageTrueRangeFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Average True Range as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Average True Range from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade AverageTrueRangeFacade { get { return averageTrueRange.ValueFacade; } }
        #endregion

        #region TrueRangeFacade
        /// <summary>
        /// A line indicator façade to expose a value of the True Range as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the True Range from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade TrueRangeFacade { get { return averageTrueRange.TrueRangeFacade; } }
        #endregion

        [DataMember]
        private readonly AverageTrueRange averageTrueRange;

        private const string natr = "natr";
        private const string natrFull = "Normalized Average True Range";
        private const string argumentLength = "length";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="NormalizedAverageTrueRange"/> class.
        /// </summary>
        /// <param name="length">The number of time periods of the Normalized Average True Range.</param>
        public NormalizedAverageTrueRange(int length)
            : base(natr, natrFull)
        {
            if (1 > length)
                throw new ArgumentOutOfRangeException(argumentLength);
            this.length = length;
            averageTrueRange = new AverageTrueRange(length);
            moniker = string.Concat(natr, "(", length.ToString(CultureInfo.InvariantCulture), ")");
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
                value = double.NaN;
                averageTrueRange.Reset();
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// The Normalized Average True Range indicator can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same sample value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            return Update(sample, sample, sample);
        }

        /// <summary>
        /// The Normalized Average True Range indicator can be updated only with <c>ohlcv</c> samples.
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
        /// The Normalized Average True Range indicator can be updated only with <c>ohlcv</c> samples.
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
        /// Updates the value of the Normalized Average True Range. The indicator is not primed during the first <c>ℓ</c> updates.
        /// </summary>
        /// <param name="sampleClose">The <c>close</c> value of a new sample.</param>
        /// <param name="sampleHigh">The <c>high</c> value of a new sample.</param>
        /// <param name="sampleLow">The <c>low</c> value of a new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sampleClose, double sampleHigh, double sampleLow)
        {
            if (double.IsNaN(sampleClose) || double.IsNaN(sampleHigh) || double.IsNaN(sampleLow))
                return double.NaN;
            lock (updateLock)
            {
                double averageTrueRangeValue = averageTrueRange.Update(sampleClose, sampleHigh, sampleLow);
                if (averageTrueRange.IsPrimed)
                {
                    primed = true;
                    value = (0d == sampleClose) ? 0d : (averageTrueRangeValue / sampleClose) * 100d;
                }
                return primed ? value : double.NaN;
            }
        }

        /// <summary>
        /// Updates the value of the Normalized Average True Range. The indicator is not primed during the first <c>ℓ</c> updates.
        /// </summary>
        /// <param name="sampleClose">The <c>close</c> value of a new sample.</param>
        /// <param name="sampleHigh">The <c>high</c> value of a new sample.</param>
        /// <param name="sampleLow">The <c>low</c> value of a new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(double sampleClose, double sampleHigh, double sampleLow, DateTime dateTime)
        {
            return new Scalar(dateTime, Update(sampleClose, sampleHigh, sampleLow));
        }

        /// <summary>
        /// Updates the value of the Normalized Average True Range. The indicator is not primed during the first <c>ℓ</c> updates.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            return new Scalar(ohlcv.Time, Update(ohlcv.Close, ohlcv.High, ohlcv.Low));
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
