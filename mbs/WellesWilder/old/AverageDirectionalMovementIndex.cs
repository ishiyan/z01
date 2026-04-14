using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// The average directional movement index (ADX) was developed in 1978 by Welles Wilder as an indication of trend strength.
    /// <para />
    /// The calculation of the movement index is as follows. One first calculates the directional movement (+DM and −DM):
    /// <para />
    /// ❶ UpMove = today's high − yesterday's high
    /// <para />
    /// ❷ DownMove = yesterday's low − today's low
    /// <para />
    /// ❸ if UpMove &gt; DownMove and UpMove &gt; 0, then +DM = UpMove, else +DM = 0
    /// <para />
    /// ❹ if DownMove &gt; UpMove and DownMove &gt; 0, then −DM = DownMove, else −DM = 0
    /// <para />
    /// After selecting the number of periods (Wilder used 14 days originally), one calculates directional indices +DI and −DI:
    /// <para />
    /// ❶ +DI = 100 times +DM divided by average true range
    /// <para />
    /// ❷ −DI = 100 times −DM divided by average true range
    /// <para />
    /// The value of the DX is calculated as:
    /// <para />
    /// DX = 100 |-DI - +DI| / (-DI + +DI14)
    /// Then, the value of the ADX is calculated as:
    /// <para />
    /// ADX = (previousADX (14-1) + DX) / 14, where the very first ADX is the simple average of the first 14 DX values.
    /// <para />
    /// See Welles Wilder, New Concepts in Technical Trading Systems, Greensboro, NC, Trend Research, ISBN 978-0894590276.
    /// </summary>
    [DataContract]
    public sealed class AverageDirectionalMovementIndex : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region Length
        [DataMember]
        private readonly int length;
        /// <summary>
        /// The smoothing length, <c>ℓ</c>, (the number of time periods) of the Average Directional Movement Index. The length of 1 means no smoothing.
        /// </summary>
        public int Length { get { return length; } }
        #endregion

        #region Value
        [DataMember]
        private double averageDirectionalMovementIndex = double.NaN;
        /// <summary>
        /// The current value of the Average Directional Movement Index or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (updateLock) { return averageDirectionalMovementIndex; } } }
        #endregion

        #region DirectionalMovementIndex
        /// <summary>
        /// The current value of the Directional Movement Index or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalMovementIndex { get { return directionalMovementIndex.Value; } }
        #endregion

        #region DirectionalIndicatorPlus
        /// <summary>
        /// The current value of the Directional Indicator Plus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalIndicatorPlus { get { return directionalMovementIndex.DirectionalIndicatorPlus; } }
        #endregion

        #region DirectionalIndicatorMinus
        /// <summary>
        /// The current value of the Directional Indicator Minus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalIndicatorMinus { get { return directionalMovementIndex.DirectionalIndicatorMinus; } }
        #endregion

        #region DirectionalMovementPlus
        /// <summary>
        /// The current value of the Directional Movement Plus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalMovementPlus { get { return directionalMovementIndex.DirectionalMovementPlus; } }
        #endregion

        #region DirectionalMovementMinus
        /// <summary>
        /// The current value of the Directional Movement Minus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalMovementMinus { get { return directionalMovementIndex.DirectionalMovementMinus; } }
        #endregion

        #region TrueRange
        /// <summary>
        /// The current value of the True Range or <c>NaN</c> if not primed.
        /// </summary>
        public double TrueRange { get { return directionalMovementIndex.TrueRange; } }
        #endregion

        #region AverageTrueRange
        /// <summary>
        /// The current value of the Average True Range or <c>NaN</c> if not primed.
        /// </summary>
        public double AverageTrueRange { get { return directionalMovementIndex.AverageTrueRange; } }
        #endregion

        #region ValueFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Average Directional Movement Index as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Average Directional Movement Index from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade ValueFacade
        {
            get
            {
                return new LineIndicatorFacade(admi, moniker, admiFull, () => IsPrimed, () => Value);
            }
        }
        #endregion

        #region DirectionalMovementIndexFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Directional Movement Index as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Movement Index from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalMovementIndexFacade { get { return directionalMovementIndex.ValueFacade; } }
        #endregion

        #region DirectionalIndicatorPlusFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Directional Indicator Plus as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Indicator Plus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalIndicatorPlusFacade { get { return directionalMovementIndex.DirectionalIndicatorPlusFacade; } }
        #endregion

        #region DirectionalIndicatorMinusFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Directional Indicator Minus as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Indicator Minus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalIndicatorMinusFacade { get { return directionalMovementIndex.DirectionalIndicatorMinusFacade; } }
        #endregion

        #region DirectionalMovementPlusFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Directional Movement Plus as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Movement Plus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalMovementPlusFacade { get { return directionalMovementIndex.DirectionalMovementPlusFacade; } }
        #endregion

        #region DirectionalMovementMinusFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Directional Movement Minus as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Movement Minus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalMovementMinusFacade { get { return directionalMovementIndex.DirectionalMovementMinusFacade; } }
        #endregion

        #region TrueRangeFacade
        /// <summary>
        /// A line indicator façade to expose a value of the True Range as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the True Range from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade TrueRangeFacade { get { return directionalMovementIndex.TrueRangeFacade; } }
        #endregion

        #region AverageTrueRangeFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Average True Range as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Average True Range from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade AverageTrueRangeFacade { get { return directionalMovementIndex.AverageTrueRangeFacade; } }
        #endregion

        [DataMember]
        private readonly int lengthMinusOne;
        [DataMember]
        private int count;
        [DataMember]
        private double sum;
        [DataMember]
        private readonly DirectionalMovementIndex directionalMovementIndex;

        private const string admi = "adx";
        private const string admiFull = "Average Directional Movement Index";
        private const string argumentLength = "length";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="AverageDirectionalMovementIndex"/> class.
        /// </summary>
        /// <param name="length">The smoothing length, <c>ℓ</c>, (the number of time periods) of the Average Directional Movement Index. The length of 1 means no smoothing. The default value used by original indicator is 14.</param>
        public AverageDirectionalMovementIndex(int length = 14)
            : base(admi, admiFull, OhlcvComponent.MedianPrice)
        {
            if (1 > length)
                throw new ArgumentOutOfRangeException(argumentLength);
            this.length = length;
            lengthMinusOne = length - 1;
            directionalMovementIndex = new DirectionalMovementIndex(length);
            moniker = string.Concat(admi, "(", length.ToString(CultureInfo.InvariantCulture), ")");
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
                count = 0;
                sum = 0d;
                primed = false;
                directionalMovementIndex.Reset();
                averageDirectionalMovementIndex = double.NaN;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the Average Directional Movement Index.
        /// In this case, the same sample value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="sampleClose">A new closing price sample.</param>
        /// <param name="sampleHigh">A new high price sample.</param>
        /// <param name="sampleLow">A new low price sample.</param>
        /// <returns>The new value of the indicator.</returns>
        internal double Update(double sampleClose, double sampleHigh, double sampleLow)
        {
            if (double.IsNaN(sampleClose) || double.IsNaN(sampleHigh) || double.IsNaN(sampleLow))
                return double.NaN;
            lock (updateLock)
            {
                double dx = directionalMovementIndex.Update(sampleClose, sampleHigh, sampleLow);
                if (primed)
                {
                    averageDirectionalMovementIndex = (averageDirectionalMovementIndex * lengthMinusOne + dx) / length;
                }
                else
                {
                    if (directionalMovementIndex.IsPrimed)
                    {
                        sum += dx;
                        if (++count == length)
                        {
                            averageDirectionalMovementIndex = sum / length;
                            primed = true;
                        }
                    }
                }
                return averageDirectionalMovementIndex;
            }
        }

        /// <summary>
        /// The Average Directional Movement Index can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same sample value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            return Update(sample, sample, sample);
        }

        /// <summary>
        /// The Average Directional Movement Index can be updated only with <c>ohlcv</c> samples.
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
        /// Updates the value of the Average Directional Movement Index.
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
                v = averageDirectionalMovementIndex;
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