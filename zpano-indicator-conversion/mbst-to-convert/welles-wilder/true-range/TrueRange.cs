using System;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// The True Range is defined by Welles Wilder as the largest of:
    /// <para>❶ the distance from today's <c>high</c> to today's <c>low</c></para>
    /// <para>❷ the distance from yesterday's <c>close</c> to today's <c>high</c></para>
    /// <para>❸ the distance from yesterday's <c>close</c> to today's <c>low</c>.</para>
    /// <para>This implementation calculates the very first True Range value as the <c>high - low</c> of the first bar. However, the indicator is not primed untill the second bar.</para>
    /// The indicator accepts only <c>ohlcv</c> samples.
    /// </summary>
    [DataContract]
    public sealed class TrueRange : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region Value
        [DataMember]
        private double value = double.NaN;
        /// <summary>
        /// The current value of the the True Range, or <c>NaN</c> if not primed.
        /// The indicator is not primed until the second update.
        /// </summary>
        public double Value { get { lock (updateLock) { return value; } } }
        #endregion

        #region ValueFacade
        /// <summary>
        /// A line indicator façade to expose a value of the True Range as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the True Range from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade ValueFacade
        {
            get
            {
                return new LineIndicatorFacade(tr, moniker, trFull, () => IsPrimed, () => Value);
            }
        }
        #endregion

        [DataMember]
        private double previousClose = double.NaN;

        private const string tr = "tr";
        private const string trFull = "True Range";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="TrueRange"/> class.
        /// </summary>
        public TrueRange()
            : base(tr, trFull)
        {
            moniker = tr;
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
                previousClose = double.NaN;
                value = double.NaN;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// The True Range indicator can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same sample value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            return Update(sample, sample, sample);
        }

        /// <summary>
        /// The True Range indicator can be updated only with <c>ohlcv</c> samples.
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
        /// The True Range indicator can be updated only with <c>ohlcv</c> samples.
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
        /// Updates the value of the True Range. The indicator is not primed until the second update.
        /// </summary>
        /// <param name="sampleClose">The <c>close</c> value of a new sample.</param>
        /// <param name="sampleHigh">The <c>high</c> value of a new sample.</param>
        /// <param name="sampleLow">The <c>low</c> value of a new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        internal double Update(double sampleClose, double sampleHigh, double sampleLow)
        {
            if (double.IsNaN(sampleClose) || double.IsNaN(sampleHigh) || double.IsNaN(sampleLow))
                return double.NaN;
            lock (updateLock)
            {
                if (!primed)
                {
                    if (double.IsNaN(previousClose))
                    {
                        previousClose = sampleClose;
                        return double.NaN;
                    }
                    primed = true;
                }
                double greatest = sampleHigh - sampleLow;
                double temp = Math.Abs(sampleHigh - previousClose);
                if (greatest < temp)
                    greatest = temp;
                temp = Math.Abs(sampleLow - previousClose);
                if (greatest < temp)
                    greatest = temp;
                value = greatest;
                previousClose = sampleClose;
                return value;
            }
        }

        /// <summary>
        /// Updates the value of the True Range. The indicator is not primed until the second update.
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
        /// Updates the value of the True Range. The indicator is not primed until the second update.
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
