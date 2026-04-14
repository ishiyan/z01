using System;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Computes the Slow Trend Line Momentum.
    /// </summary>
    [DataContract]
    public sealed class SlowTrendLineMomentum : Indicator, ILineIndicator
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
        /// The current value of the Slow Trend Line Momentum (STLM), or <c>NaN</c> if not primed.
        /// The indicator is not primed during the first <c>ℓ-1</c> updates, where <c>ℓ</c> is the length.
        /// </summary>
        public double Value { get { lock (updateLock) { return value; } } }
        #endregion

        #region SatlValue
        [DataMember]
        private double satlValue = double.NaN;
        /// <summary>
        /// The current value of the Slow Adaptive Trend Line (SATL), or <c>NaN</c> if not primed.
        /// </summary>
        public double SatlValue { get { lock (updateLock) { return satlValue; } } }
        #endregion

        #region RstlValue
        [DataMember]
        private double rstlValue = double.NaN;
        /// <summary>
        /// The current value of the Reference Slow Trend Line (RSTL), or <c>NaN</c> if not primed.
        /// </summary>
        public double RstlValue { get { lock (updateLock) { return rstlValue; } } }
        #endregion

        [DataMember]
        private SlowAdaptiveTrendLine satl;
        [DataMember]
        private ReferenceSlowTrendLine rstl;
        [DataMember]
        private int count;

        private const string stlm = "STLM";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="SlowTrendLineMomentum"/> class.
        /// </summary>
        public SlowTrendLineMomentum()
            : this(OhlcvComponent.ClosingPrice)
        {
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="SlowTrendLineMomentum"/> class.
        /// </summary>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public SlowTrendLineMomentum(OhlcvComponent ohlcvComponent)
            : base(stlm, "Slow Trend Line Momentum", ohlcvComponent)
        {
            moniker = stlm;
            satl = new SlowAdaptiveTrendLine(ohlcvComponent);
            rstl = new ReferenceSlowTrendLine(ohlcvComponent);
            length = rstl.Length;
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
                satl.Reset();
                rstl.Reset();
                value = double.NaN;
                satlValue = double.NaN;
                rstlValue = double.NaN;
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
                satlValue = satl.Update(sample);
                rstlValue = rstl.Update(sample);
                if (length < count) // primed.
                    value = satlValue - rstlValue;
                else // Not primed.
                {
                    if (length == ++count)
                    {
                        count++;
                        primed = true;
                        value = satlValue - rstlValue;
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
