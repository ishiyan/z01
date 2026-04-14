using System;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Computes the Fast Trend Line Momentum.
    /// </summary>
    [DataContract]
    public sealed class FastTrendLineMomentum : Indicator, ILineIndicator
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
        /// The current value of the Fast Trend Line Momentum (FTLM), or <c>NaN</c> if not primed.
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

        #region RftlValue
        [DataMember]
        private double rftlValue = double.NaN;
        /// <summary>
        /// The current value of the Reference Fast Trend Line (RFTL), or <c>NaN</c> if not primed.
        /// </summary>
        public double RftlValue { get { lock (updateLock) { return rftlValue; } } }
        #endregion

        [DataMember]
        private FastAdaptiveTrendLine fatl;
        [DataMember]
        private ReferenceFastTrendLine rftl;
        [DataMember]
        private int count;

        private const string ftlm = "FTLM";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="FastTrendLineMomentum"/> class.
        /// </summary>
        public FastTrendLineMomentum()
            : this(OhlcvComponent.ClosingPrice)
        {
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="FastTrendLineMomentum"/> class.
        /// </summary>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public FastTrendLineMomentum(OhlcvComponent ohlcvComponent)
            : base(ftlm, "Fast Trend Line Momentum", ohlcvComponent)
        {
            moniker = ftlm;
            fatl = new FastAdaptiveTrendLine(ohlcvComponent);
            rftl = new ReferenceFastTrendLine(ohlcvComponent);
            length = rftl.Length;
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
                rftl.Reset();
                value = double.NaN;
                fatlValue = double.NaN;
                rftlValue = double.NaN;
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
                rftlValue = rftl.Update(sample);
                if (length < count) // primed.
                    value = fatlValue - rftlValue;
                else // Not primed.
                {
                    if (length == ++count)
                    {
                        count++;
                        primed = true;
                        value = fatlValue - rftlValue;
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
