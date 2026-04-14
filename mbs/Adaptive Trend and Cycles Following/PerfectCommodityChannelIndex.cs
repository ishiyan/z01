using System;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Computes the Perfect Commodity Channel Index.
    /// </summary>
    [DataContract]
    public sealed class PerfectCommodityChannelIndex : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region Length
        [DataMember]
        private readonly int length;
        /// <summary>
        /// The effective length <c>ℓ</c> (the number of time periods).
        /// </summary>
        internal int Length { get { return length; } }
        #endregion

        #region Value
        [DataMember]
        private double value = double.NaN;
        /// <summary>
        /// The current value of the PerfectCommodityChannelIndex (PCCI), or <c>NaN</c> if not primed.
        /// The indicator is not primed during the first <c>ℓ-1</c> updates, where <c>ℓ</c> is the length.
        /// </summary>
        public double Value { get { lock (updateLock) { return value; } } }
        #endregion

        #region FatlValue
        [DataMember]
        private double fatlValue = double.NaN;
        /// <summary>
        /// The current value of the Fast Adaptive Trend Line (FATL), or <c>NaN</c> if not primed.
        /// </summary>
        public double FatlValue { get { lock (updateLock) { return fatlValue; } } }
        #endregion

        [DataMember]
        private FastAdaptiveTrendLine fatl;
        [DataMember]
        private int count;

        private const string pcci = "PCCI";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="PerfectCommodityChannelIndex"/> class.
        /// </summary>
        public PerfectCommodityChannelIndex()
            : this(OhlcvComponent.ClosingPrice)
        {
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="PerfectCommodityChannelIndex"/> class.
        /// </summary>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public PerfectCommodityChannelIndex(OhlcvComponent ohlcvComponent)
            : base(pcci, "Perfect Commodity Channel Index", ohlcvComponent)
        {
            moniker = pcci;
            fatl = new FastAdaptiveTrendLine(ohlcvComponent);
            length = fatl.Length;
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
                count = 0;
                fatl.Reset();
                value = double.NaN;
                fatlValue = double.NaN;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the value of the indicator.
        /// The indicator is not primed during the first<c>ℓ-1</c> updates.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            if (double.IsNaN(sample))
                return sample;
            lock (updateLock)
            {
                fatlValue = fatl.Update(sample);
                if (length < count) // primed.
                    value = sample - fatlValue;
                else // Not primed.
                {
                    if (length == ++count)
                    {
                        count++;
                        primed = true;
                        value = sample - fatlValue;
                    }
                }
                return value;
            }
        }

        /// <summary>
        /// Updates the value of the indicator.
        /// The indicator is not primed during the first <c>ℓ-1</c> updates.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(double sample, DateTime dateTime)
        {
            return new Scalar(dateTime, Update(sample));
        }

        /// <summary>
        /// Updates the value of the indicator.
        /// The indicator is not primed during the first <c>ℓ-1</c> updates.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Scalar scalar)
        {
            return new Scalar(scalar.Time, Update(scalar.Value));
        }

        /// <summary>
        /// Updates the value of the indicator.
        /// The indicator is not primed during the first <c>ℓ-1</c> updates.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            return new Scalar(ohlcv.Time, Update(ohlcv.Component(ohlcvComponent)));
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
            StringBuilder sb = new StringBuilder();
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
