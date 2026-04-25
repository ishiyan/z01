using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// The directional movement index (DX) was developed in 1978 by Welles Wilder as an indication of trend strength.
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
    /// Then, the value of the DX is calculated as:
    /// <para />
    /// DX = 100 |-DI - +DI| / (-DI + +DI)
    /// <para />
    /// See Welles Wilder, New Concepts in Technical Trading Systems, Greensboro, NC, Trend Research, ISBN 978-0894590276.
    /// </summary>
    [DataContract]
    public sealed class DirectionalMovementIndex : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region Length
        [DataMember]
        private readonly int length;
        /// <summary>
        /// The smoothing length, <c>ℓ</c>, (the number of time periods) of the Directional Movement Index. The length of 1 means no smoothing.
        /// </summary>
        public int Length { get { return length; } }
        #endregion

        #region Value
        [DataMember]
        private double directionalMovementIndex = double.NaN;
        /// <summary>
        /// The current value of the Directional Movement Index or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (updateLock) { return directionalMovementIndex; } } }
        #endregion

        #region DirectionalIndicatorPlus
        /// <summary>
        /// The current value of the Directional Indicator Plus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalIndicatorPlus { get { return directionalIndicatorPlus.Value; } }
        #endregion

        #region DirectionalIndicatorMinus
        /// <summary>
        /// The current value of the Directional Indicator Minus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalIndicatorMinus { get { return directionalIndicatorMinus.Value; } }
        #endregion

        #region DirectionalMovementPlus
        /// <summary>
        /// The current value of the Directional Movement Plus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalMovementPlus { get { return directionalIndicatorPlus.DirectionalMovementPlus; } }
        #endregion

        #region DirectionalMovementMinus
        /// <summary>
        /// The current value of the Directional Movement Minus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalMovementMinus { get { return directionalIndicatorMinus.DirectionalMovementMinus; } }
        #endregion

        #region AverageTrueRange
        /// <summary>
        /// The current value of the Average True Range or <c>NaN</c> if not primed.
        /// </summary>
        public double AverageTrueRange { get { return directionalIndicatorPlus.AverageTrueRange; } }
        #endregion

        #region TrueRange
        /// <summary>
        /// The current value of the True Range or <c>NaN</c> if not primed.
        /// </summary>
        public double TrueRange { get { return directionalIndicatorPlus.TrueRange; } }
        #endregion

        #region ValueFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Directional Movement Index as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Movement Index from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade ValueFacade
        {
            get
            {
                return new LineIndicatorFacade(dmi, moniker, dmiFull, () => IsPrimed, () => Value);
            }
        }
        #endregion

        #region DirectionalIndicatorPlusFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Directional Indicator Plus as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Indicator Plus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalIndicatorPlusFacade { get { return directionalIndicatorPlus.ValueFacade; } }
        #endregion

        #region DirectionalIndicatorMinusFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Directional Indicator Minus as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Indicator Minus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalIndicatorMinusFacade { get { return directionalIndicatorMinus.ValueFacade; } }
        #endregion

        #region DirectionalMovementPlusFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Directional Movement Plus as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Movement Plus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalMovementPlusFacade { get { return directionalIndicatorPlus.DirectionalMovementPlusFacade; } }
        #endregion

        #region DirectionalMovementMinusFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Directional Movement Minus as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Movement Minus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalMovementMinusFacade { get { return directionalIndicatorMinus.DirectionalMovementMinusFacade; } }
        #endregion

        #region AverageTrueRangeFacade
        /// <summary>
        /// A line indicator façade to expose a value of the Average True Range as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Average True Range from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade AverageTrueRangeFacade { get { return directionalIndicatorPlus.AverageTrueRangeFacade; } }
        #endregion


        #region TrueRangeFacade
        /// <summary>
        /// A line indicator façade to expose a value of the True Range as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the True Range from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade TrueRangeFacade { get { return directionalIndicatorPlus.TrueRangeFacade; } }
        #endregion

        [DataMember]
        private readonly DirectionalIndicatorPlus directionalIndicatorPlus;
        [DataMember]
        private readonly DirectionalIndicatorMinus directionalIndicatorMinus;

        private const string dmi = "dx";
        private const string dmiFull = "Directional Movement Index";
        private const string argumentLength = "length";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="DirectionalMovementIndex"/> class.
        /// </summary>
        /// <param name="length">The smoothing length, <c>ℓ</c>, (the number of time periods) of the Directional Movement Index. The length of 1 means no smoothing. The default value used by original indicator is 14.</param>
        public DirectionalMovementIndex(int length = 14)
            : base(dmi, dmiFull, OhlcvComponent.MedianPrice)
        {
            if (1 > length)
                throw new ArgumentOutOfRangeException(argumentLength);
            this.length = length;
            directionalIndicatorPlus = new DirectionalIndicatorPlus(length);
            directionalIndicatorMinus = new DirectionalIndicatorMinus(length);
            moniker = string.Concat(dmi, "(", length.ToString(CultureInfo.InvariantCulture), ")");
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
                directionalIndicatorPlus.Reset();
                directionalIndicatorMinus.Reset();
                directionalMovementIndex = double.NaN;
            }
        }
        #endregion

        #region Update
        internal double Update(double sampleClose, double sampleHigh, double sampleLow)
        {
            if (double.IsNaN(sampleHigh) || double.IsNaN(sampleLow))
                return double.NaN;
            lock (updateLock)
            {
                double dip = directionalIndicatorPlus.Update(sampleClose, sampleHigh, sampleLow);
                double dim = directionalIndicatorMinus.Update(sampleClose, sampleHigh, sampleLow);
                const double epsilon = 0.00000001;
                if (primed)
                {
                    double sum = dip + dim;
                    if (-epsilon < sum && sum < epsilon)
                        directionalMovementIndex = 0d;
                    else
                        directionalMovementIndex = 100d * Math.Abs(dip - dim) / sum;
                }
                else
                {
                    if (directionalIndicatorPlus.IsPrimed && directionalIndicatorMinus.IsPrimed)
                    {
                        double sum = dip + dim;
                        if (-epsilon < sum && sum < epsilon)
                            directionalMovementIndex = 0d;
                        else
                            directionalMovementIndex = 100d * Math.Abs(dip - dim) / sum;
                        primed = true;
                    }
                }
                return directionalMovementIndex;
            }
        }

        /// <summary>
        /// The Directional Movement Index can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same sample value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            return Update(sample, sample, sample);
        }

        /// <summary>
        /// The Directional Movement Index can be updated only with <c>ohlcv</c> samples.
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
        /// Updates the value of the Directional Movement Index.
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
                v = directionalMovementIndex;
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