using System;
using System.Globalization;
using Mbs.Trading.Data;
using Mbs.Trading.Indicators.Abstractions;

namespace Mbs.Trading.Indicators.WellesWilder
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
    public sealed class AverageDirectionalMovementIndex : Indicator, ILineIndicator
    {
        #region Members and accessors
        /// <summary>
        /// The smoothing length, <c>ℓ</c>, (the number of time periods) of the Average Directional Movement Index. The length of 1 means no smoothing.
        /// </summary>
        public int Length { get; }

        private double averageDirectionalMovementIndex = double.NaN;
        /// <summary>
        /// The current value of the Average Directional Movement Index or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (Lock) { return averageDirectionalMovementIndex; } } }

        /// <summary>
        /// The current value of the Directional Movement Index or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalMovementIndex => directionalMovementIndex.Value;

        /// <summary>
        /// The current value of the Directional Indicator Plus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalIndicatorPlus => directionalMovementIndex.DirectionalIndicatorPlus;

        /// <summary>
        /// The current value of the Directional Indicator Minus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalIndicatorMinus => directionalMovementIndex.DirectionalIndicatorMinus;

        /// <summary>
        /// The current value of the Directional Movement Plus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalMovementPlus => directionalMovementIndex.DirectionalMovementPlus;

        /// <summary>
        /// The current value of the Directional Movement Minus or <c>NaN</c> if not primed.
        /// </summary>
        public double DirectionalMovementMinus => directionalMovementIndex.DirectionalMovementMinus;

        /// <summary>
        /// The current value of the True Range or <c>NaN</c> if not primed.
        /// </summary>
        public double TrueRange => directionalMovementIndex.TrueRange;

        /// <summary>
        /// The current value of the Average True Range or <c>NaN</c> if not primed.
        /// </summary>
        public double AverageTrueRange => directionalMovementIndex.AverageTrueRange;

        /// <summary>
        /// A line indicator façade to expose a value of the Average Directional Movement Index as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Average Directional Movement Index from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade ValueFacade => new LineIndicatorFacade(Admi, Moniker, AdmiFull, () => IsPrimed, () => Value);

        /// <summary>
        /// A line indicator façade to expose a value of the Directional Movement Index as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Movement Index from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalMovementIndexFacade => directionalMovementIndex.ValueFacade;

        /// <summary>
        /// A line indicator façade to expose a value of the Directional Indicator Plus as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Indicator Plus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalIndicatorPlusFacade => directionalMovementIndex.DirectionalIndicatorPlusFacade;

        /// <summary>
        /// A line indicator façade to expose a value of the Directional Indicator Minus as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Indicator Minus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalIndicatorMinusFacade => directionalMovementIndex.DirectionalIndicatorMinusFacade;

        /// <summary>
        /// A line indicator façade to expose a value of the Directional Movement Plus as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Movement Plus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalMovementPlusFacade => directionalMovementIndex.DirectionalMovementPlusFacade;

        /// <summary>
        /// A line indicator façade to expose a value of the Directional Movement Minus as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Movement Minus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DirectionalMovementMinusFacade => directionalMovementIndex.DirectionalMovementMinusFacade;

        /// <summary>
        /// A line indicator façade to expose a value of the True Range as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the True Range from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade TrueRangeFacade => directionalMovementIndex.TrueRangeFacade;

        /// <summary>
        /// A line indicator façade to expose a value of the Average True Range as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Average True Range from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade AverageTrueRangeFacade => directionalMovementIndex.AverageTrueRangeFacade;

        private readonly int lengthMinusOne;
        private int count;
        private double sum;
        private readonly DirectionalMovementIndex directionalMovementIndex;

        private const string Admi = "adx";
        private const string AdmiFull = "Average Directional Movement Index";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="AverageDirectionalMovementIndex"/> class.
        /// </summary>
        /// <param name="length">The smoothing length, <c>ℓ</c>, (the number of time periods) of the Average Directional Movement Index. The length of 1 means no smoothing. The default value used by original indicator is 14.</param>
        public AverageDirectionalMovementIndex(int length = 14)
            : base(Admi, AdmiFull, OhlcvComponent.MedianPrice)
        {
            if (1 > length)
                throw new ArgumentOutOfRangeException(nameof(length));
            Length = length;
            lengthMinusOne = length - 1;
            directionalMovementIndex = new DirectionalMovementIndex(length);
            Moniker = string.Concat(Admi, "(", length.ToString(CultureInfo.InvariantCulture), ")");
        }
        #endregion

        #region Reset
        /// <inheritdoc />
        public override void Reset()
        {
            lock (Lock)
            {
                count = 0;
                sum = 0d;
                Primed = false;
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
            lock (Lock)
            {
                double dx = directionalMovementIndex.Update(sampleClose, sampleHigh, sampleLow);
                if (Primed)
                {
                    averageDirectionalMovementIndex = (averageDirectionalMovementIndex * lengthMinusOne + dx) / Length;
                }
                else
                {
                    if (directionalMovementIndex.IsPrimed)
                    {
                        sum += dx;
                        if (++count == Length)
                        {
                            averageDirectionalMovementIndex = sum / Length;
                            Primed = true;
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
    }
}