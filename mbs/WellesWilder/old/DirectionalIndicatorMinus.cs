using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// The directional indicator was developed in 1978 by Welles Wilder as an indication of trend strength.
    /// <para />
    /// The calculation of the directional indicator (+DI and −DI) is as follows. One first calculates the directional movement (+DM and −DM):
    /// <para />
    /// ❶ UpMove = today's high − yesterday's high
    /// <para />
    /// ❷ DownMove = yesterday's low − today's low
    /// <para />
    /// ❸ if UpMove &gt; DownMove and UpMove &gt; 0, then +DM = UpMove, else +DM = 0
    /// <para />
    /// ❹ if DownMove &gt; UpMove and DownMove &gt; 0, then −DM = DownMove, else −DM = 0
    /// <para />
    /// After selecting the number of periods (Wilder used 14 days originally), +DI and −DI are:
    /// <para />
    /// ❶ +DI = 100 times +DM divided by average true range
    /// <para />
    /// ❷ −DI = 100 times −DM divided by average true range
    /// <para />
    /// See Welles Wilder, New Concepts in Technical Trading Systems, Greensboro, NC, Trend Research, ISBN 978-0894590276.
    /// </summary>
    [DataContract]
    public sealed class DirectionalIndicatorMinus : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region Length
        [DataMember]
        private readonly int length;
        /// <summary>
        /// The smoothing length, <c>ℓ</c>, (the number of time periods) of the Directional Indicator. The length of 1 means no smoothing.
        /// </summary>
        public int Length { get { return length; } }
        #endregion

        #region Value
        [DataMember]
        private double directionalIndicatorMinus = double.NaN;
        /// <summary>
        /// The current value of the Directional Indicator Minus or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (updateLock) { return directionalIndicatorMinus; } } }
        #endregion

        #region DirectionalMovementMinus
        /// <summary>
        /// The current value of the Directional Movement Minus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalMovementMinus { get { return directionalMovementMinus.Value; } }
        #endregion

        #region AverageTrueRange
        /// <summary>
        /// The current value of the Average True Range or <c>NaN</c> if not primed.
        /// </summary>
        public double AverageTrueRange { get { return averageTrueRange.Value; } }
        #endregion

        #region TrueRange
        /// <summary>
        /// The current value of the True Range or <c>NaN</c> if not primed.
        /// </summary>
        public double TrueRange { get { return averageTrueRange.TrueRange; } }
        #endregion

        #region ValueFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Directional Indicator Minus as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Indicator Minus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade ValueFacade
        {
            get
            {
                return new LineIndicatorFacade(dim, moniker, dimFull, () => IsPrimed, () => Value);
            }
        }
        #endregion

        #region DirectionalMovementMinusFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Directional Movement Minus as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Movement Minus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalMovementMinusFacade { get { return directionalMovementMinus.ValueFacade; } }
        #endregion

        #region AverageTrueRangeFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Average True Range as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of theAverage True Range from this instance, assuming it is already updated.
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
        [DataMember]
        private readonly DirectionalMovementMinus directionalMovementMinus;

        private const string dim = "-di";
        private const string dimFull = "Directional Indicator Minus";
        private const string argumentLength = "length";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="DirectionalIndicatorMinus"/> class.
        /// </summary>
        /// <param name="length">The smoothing length, <c>ℓ</c>, (the number of time periods) of the Directional Indicator. The length of 1 means no smoothing. The default value used by original indicator is 14.</param>
        public DirectionalIndicatorMinus(int length = 14)
            : base(dim, dimFull, OhlcvComponent.MedianPrice)
        {
            if (1 > length)
                throw new ArgumentOutOfRangeException(argumentLength);
            this.length = length;
            averageTrueRange = new AverageTrueRange(length);
            directionalMovementMinus = new DirectionalMovementMinus(length);
            moniker = string.Concat(dim, "(", length.ToString(CultureInfo.InvariantCulture), ")");
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
                averageTrueRange.Reset();
                directionalMovementMinus.Reset();
                directionalIndicatorMinus = double.NaN;
            }
        }
        #endregion

        #region Update
        internal double Update(double sampleClose, double sampleHigh, double sampleLow)
        {
            if (double.IsNaN(sampleClose) || double.IsNaN(sampleHigh) || double.IsNaN(sampleLow))
                return double.NaN;
            lock (updateLock)
            {
                const double epsilon = 0.00000001;
                double atr = averageTrueRange.Update(sampleClose, sampleHigh, sampleLow) * length;
                double dmm = directionalMovementMinus.Update(sampleHigh, sampleLow);
                if (primed)
                {
                    if (-epsilon < atr && atr < epsilon)
                        directionalIndicatorMinus = 0d;
                    else
                        directionalIndicatorMinus = 100d * dmm / atr;
                }
                else
                {
                    if (averageTrueRange.IsPrimed && directionalMovementMinus.IsPrimed)
                    {
                        if (-epsilon < atr && atr < epsilon)
                            directionalIndicatorMinus = 0d;
                        else
                            directionalIndicatorMinus = 100d * dmm / atr;
                        primed = true;
                    }
                }
                return directionalIndicatorMinus;
            }
        }

        /// <summary>
        /// The Directional Indicator Minus can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same sample value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            return Update(sample, sample, sample);
        }

        /// <summary>
        /// The Directional Indicator Minus can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same sample value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Scalar scalar)
        {
            double sample = scalar.Value;
            return new Scalar(scalar.Time, Update(sample, sample, sample));
        }

        /// <summary>
        /// Updates the value of the Directional Indicator Minus.
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
                v = directionalIndicatorMinus;
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