using System;
using System.Globalization;
using Mbs.Trading.Data;
using Mbs.Trading.Indicators.Abstractions;

namespace Mbs.Trading.Indicators.WellesWilder
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
    public sealed class DirectionalMovementIndex : Indicator, ILineIndicator
    {
        #region Members and accessors
        /// <summary>
        /// The smoothing length, <c>ℓ</c>, (the number of time periods) of the Directional Movement Index. The length of 1 means no smoothing.
        /// </summary>
        public int Length { get; }

        private double directionalMovementIndex = double.NaN;
        /// <summary>
        /// The current value of the Directional Movement Index or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (Lock) { return directionalMovementIndex; } } }

        /// <summary>
        /// The current value of the Directional Indicator Plus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalIndicatorPlus => directionalIndicatorPlus.Value;

        /// <summary>
        /// The current value of the Directional Indicator Minus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalIndicatorMinus => directionalIndicatorMinus.Value;

        /// <summary>
        /// The current value of the Directional Movement Plus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalMovementPlus => directionalIndicatorPlus.DirectionalMovementPlus;

        /// <summary>
        /// The current value of the Directional Movement Minus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalMovementMinus => directionalIndicatorMinus.DirectionalMovementMinus;

        /// <summary>
        /// The current value of the Average True Range or <c>NaN</c> if not primed.
        /// </summary>
        public double AverageTrueRange => directionalIndicatorPlus.AverageTrueRange;

        /// <summary>
        /// The current value of the True Range or <c>NaN</c> if not primed.
        /// </summary>
        public double TrueRange => directionalIndicatorPlus.TrueRange;

        /// <summary>
        /// A line indicator façade to expose a value of the Directional Movement Index as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Movement Index from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade ValueFacade => new LineIndicatorFacade(Dmi, Moniker, DmiFull, () => IsPrimed, () => Value);

        /// <summary>
        /// A line indicator façade to expose a value of the Directional Indicator Plus as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Indicator Plus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalIndicatorPlusFacade => directionalIndicatorPlus.ValueFacade;

        /// <summary>
        /// A line indicator façade to expose a value of the Directional Indicator Minus as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Indicator Minus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalIndicatorMinusFacade => directionalIndicatorMinus.ValueFacade;

        /// <summary>
        /// A line indicator façade to expose a value of the Directional Movement Plus as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Movement Plus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalMovementPlusFacade => directionalIndicatorPlus.DirectionalMovementPlusFacade;

        /// <summary>
        /// A line indicator façade to expose a value of the Directional Movement Minus as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Movement Minus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalMovementMinusFacade => directionalIndicatorMinus.DirectionalMovementMinusFacade;

        /// <summary>
        /// A line indicator façade to expose a value of the Average True Range as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Average True Range from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade AverageTrueRangeFacade => directionalIndicatorPlus.AverageTrueRangeFacade;


        /// <summary>
        /// A line indicator façade to expose a value of the True Range as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the True Range from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade TrueRangeFacade => directionalIndicatorPlus.TrueRangeFacade;

        private readonly DirectionalIndicatorPlus directionalIndicatorPlus;
        private readonly DirectionalIndicatorMinus directionalIndicatorMinus;

        private const string Dmi = "dx";
        private const string DmiFull = "Directional Movement Index";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="DirectionalMovementIndex"/> class.
        /// </summary>
        /// <param name="length">The smoothing length, <c>ℓ</c>, (the number of time periods) of the Directional Movement Index. The length of 1 means no smoothing. The default value used by original indicator is 14.</param>
        public DirectionalMovementIndex(int length = 14)
            : base(Dmi, DmiFull, OhlcvComponent.MedianPrice)
        {
            if (1 > length)
                throw new ArgumentOutOfRangeException(nameof(length));
            Length = length;
            directionalIndicatorPlus = new DirectionalIndicatorPlus(length);
            directionalIndicatorMinus = new DirectionalIndicatorMinus(length);
            Moniker = string.Concat(Dmi, "(", length.ToString(CultureInfo.InvariantCulture), ")");
        }
        #endregion

        #region Reset
        /// <inheritdoc />
        public override void Reset()
        {
            lock (Lock)
            {
                Primed = false;
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
            lock (Lock)
            {
                double dip = directionalIndicatorPlus.Update(sampleClose, sampleHigh, sampleLow);
                double dim = directionalIndicatorMinus.Update(sampleClose, sampleHigh, sampleLow);
                const double epsilon = 0.00000001;
                if (Primed)
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
                        Primed = true;
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
    }
}