using System;
using System.Text;
using Mbst.Trading;
using Mbst.Trading.Indicators;

namespace Charts
{
    /// <summary>
    /// The simple high-low band. Use it to test <c>ohlcv</c> charts.
    /// </summary>
    internal sealed class HighLowBands : Indicator, IBandIndicator
    {
        #region Members and accessors
        #region LowerValue
        private double lowerValue = double.NaN;
        /// <summary>
        /// The lower value.
        /// </summary>
        public double LowerValue { get { lock (updateLock) { return lowerValue; } } }
        #endregion

        #region UpperValue
        private double upperValue = double.NaN;
        /// <summary>
        /// The upper value.
        /// </summary>
        public double UpperValue { get { lock (updateLock) { return upperValue; } } }
        #endregion

        private readonly int reversePeriod;
        private int reverseCount;
        private bool reverse;
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="HighLowBands"/> object.
        /// </summary>
        /// <param name="reversePeriod">The high-low reverse period, 0 means no reverse.</param>
        public HighLowBands(int reversePeriod)
            : base("HiLo", "High Low Bands")
        {
            moniker = "HiLo";
            this.reversePeriod = reversePeriod;
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
                lowerValue = double.NaN;
                upperValue = double.NaN;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the indicator.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The updated value of the indicator.</returns>
        public Band Update(double sample, DateTime dateTime)
        {
            lock (updateLock)
            {
                if (!primed)
                {
                    primed = true;
                    lowerValue = sample;
                    upperValue = sample;
                }
                else
                {
                    if (lowerValue > sample)
                        lowerValue = sample;
                    if (upperValue > sample)
                        upperValue = sample;
                }
                if (reversePeriod == reverseCount++)
                {
                    reverse = !reverse;
                    reverseCount = 0;
                }
                if (reverse)
                    return new Band(dateTime, lowerValue, upperValue);
                return new Band(dateTime, upperValue, lowerValue);
            }
        }

        /// <summary>
        /// Updates the indicator.
        /// </summary>
        /// <param name="sample">A new <c>scalar</c> sample.</param>
        /// <returns>The updated value of the indicator.</returns>
        public Band Update(Scalar sample)
        {
            return Update(sample.Value, sample.Time);
        }

        /// <summary>
        /// Updates the indicator.
        /// </summary>
        /// <param name="sample">A new <c>ohlcv</c> sample.</param>
        /// <returns>The updated value of the indicator.</returns>
        public Band Update(Ohlcv sample)
        {
            lock (updateLock)
            {
                if (!primed)
                    primed = true;
                lowerValue = sample.Low;
                upperValue = sample.High;
            }
            if (reversePeriod == reverseCount++)
            {
                reverse = !reverse;
                reverseCount = 0;
            }
            if (reverse)
                return new Band(sample.Time, sample.Low, sample.High);
            return new Band(sample.Time, sample.High, sample.Low);
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns the string that represents this object.
        /// </summary>
        /// <returns>Returns the string that represents this object.</returns>
        public override string ToString()
        {
            double vLow, vHigh; bool p;
            lock (updateLock)
            {
                p = primed;
                vLow = lowerValue;
                vHigh = upperValue;
            }
            var sb = new StringBuilder();
            sb.Append("[N:");
            sb.Append(Name);
            sb.Append(" M:");
            sb.Append(moniker);
            sb.Append(" P:");
            sb.Append(p);
            sb.Append(" H:");
            sb.Append(vHigh);
            sb.Append(" L:");
            sb.Append(vLow);
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
