using System;
using Mbs.Trading.Data;
using Mbs.Trading.Indicators.Abstractions;

namespace Mbs.Trading.Indicators.WellesWilder
{
    /// <summary>
    /// The True Range is defined by Welles Wilder as the largest of:
    /// <para>❶ the distance from today's <c>high</c> to today's <c>low</c></para>
    /// <para>❷ the distance from yesterday's <c>close</c> to today's <c>high</c></para>
    /// <para>❸ the distance from yesterday's <c>close</c> to today's <c>low</c>.</para>
    /// <para>This implementation calculates the very first True Range value as the <c>high - low</c> of the first bar. However, the indicator is not primed until the second bar.</para>
    /// The indicator accepts only <c>ohlcv</c> samples.
    /// </summary>
    public sealed class TrueRange : Indicator, ILineIndicator
    {
        #region Members and accessors
        private double value = double.NaN;
        /// <summary>
        /// The current value of the the True Range, or <c>NaN</c> if not primed.
        /// The indicator is not primed until the second update.
        /// </summary>
        public double Value { get { lock (Lock) { return value; } } }

        /// <summary>
        /// A line indicator façade to expose a value of the True Range as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the True Range from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade ValueFacade => new LineIndicatorFacade(Tr, Moniker, TrFull, () => IsPrimed, () => Value);

        private double previousClose = double.NaN;

        private const string Tr = "tr";
        private const string TrFull = "True Range";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="TrueRange"/> class.
        /// </summary>
        public TrueRange()
            : base(Tr, TrFull)
        {
            Moniker = Tr;
        }
        #endregion

        #region Reset
        /// <inheritdoc />
        public override void Reset()
        {
            lock (Lock)
            {
                Primed = false;
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
            lock (Lock)
            {
                if (!Primed)
                {
                    if (double.IsNaN(previousClose))
                    {
                        previousClose = sampleClose;
                        return double.NaN;
                    }
                    Primed = true;
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
    }
}
