using System;
using System.Globalization;
using Mbs.Trading.Data;
using Mbs.Trading.Indicators.Abstractions;

namespace Mbs.Trading.Indicators.WellesWilder
{
    /// <summary>
    /// The directional movement was developed in 1978 by Welles Wilder as an indication of trend strength.
    /// <para />
    /// The calculation of the directional movement (+DM and −DM) is as follows:
    /// <para />
    /// ❶ UpMove = today's high − yesterday's high
    /// <para />
    /// ❷ DownMove = yesterday's low − today's low
    /// <para />
    /// ❸ if UpMove &gt; DownMove and UpMove &gt; 0, then +DM = UpMove, else +DM = 0
    /// <para />
    /// ❹ if DownMove &gt; UpMove and DownMove &gt; 0, then −DM = DownMove, else −DM = 0
    /// <para />
    /// See Welles Wilder, New Concepts in Technical Trading Systems, Greensboro, NC, Trend Research, ISBN 978-0894590276.
    /// </summary>
    public sealed class DirectionalMovementPlus : Indicator, ILineIndicator
    {
        #region Members and accessors

        /// <summary>
        /// The smoothing length, <c>ℓ</c>, (the number of time periods) of the Directional Movement. The length of 1 means no smoothing.
        /// </summary>
        public int Length { get; }

        private double directionalMovementPlus = double.NaN;
        /// <summary>
        /// The current value of the Directional Movement Plus or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (Lock) { return directionalMovementPlus; } } }

        /// <summary>
        /// A line indicator façade to expose a value of the Directional Movement Plus as a separate line indicator with a fictitious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the Directional Movement Plus from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade ValueFacade => new LineIndicatorFacade(Dmp, Moniker, DmpFull, () => IsPrimed, () => Value);

        private int count;
        private readonly bool noSmoothing;
        private double previousLow;
        private double previousHigh;
        private double directionalMovementPlusPrevious;

        private const string Dmp = "+dm";
        private const string DmpFull = "Directional Movement Plus";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="DirectionalMovementPlus"/> class.
        /// </summary>
        /// <param name="length">The smoothing length, <c>ℓ</c>, (the number of time periods) of the Directional Movement. The length of 1 means no smoothing. The default value used by original indicator is 14.</param>
        public DirectionalMovementPlus(int length = 14)
            : base(Dmp, DmpFull, OhlcvComponent.MedianPrice)
        {
            if (1 > length)
                throw new ArgumentOutOfRangeException(nameof(length));
            Length = length;
            noSmoothing = length == 1;
            Moniker = string.Concat(Dmp, "(", length.ToString(CultureInfo.InvariantCulture), ")");
        }
        #endregion

        #region Reset
        /// <inheritdoc />
        public override void Reset()
        {
            lock (Lock)
            {
                Primed = false;
                directionalMovementPlus = double.NaN;
                directionalMovementPlusPrevious = 0d;
                previousLow = 0d;
                previousHigh = 0d;
                count = 0;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the value of the Directional Movement Plus indicator.
        /// </summary>
        /// <param name="sampleHigh">The high value.</param>
        /// <param name="sampleLow">The low value.</param>
        /// <returns>The new value of the indicator.</returns>
        internal double Update(double sampleHigh, double sampleLow)
        {
            // The DM1 (one period) is based on the largest part of today's range that is outside of yesterdays range.
            // The following 7 cases explain how the +DM and -DM are calculated on one period:
            //
            // Case 1:               Case 2:
            //    C|                A|
            //     |                 | C|
            //     | +DM1 = (C-A)   B|  | +DM1 = 0
            //     | -DM1 = 0           | -DM1 = (B-D)
            // A|  |                   D|
            //  | D|
            // B|
            //
            // Case 3:               Case 4:
            //    C|                   C|
            //     |                A|  |
            //     | +DM1 = (C-A)    |  | +DM1 = 0
            //     | -DM1 = 0       B|  | -DM1 = (B-D)
            // A|  |                    |
            //  |  |                   D|
            // B|  |
            //    D|
            //
            // Case 5:              Case 6:            Case 7:
            // A|                   A| C|                 C|
            //  | C| +DM1 = 0        |  |  +DM1 = 0    A|  |
            //  |  | -DM1 = 0        |  |  -DM1 = 0     |  | +DM=0
            //  | D|                 |  |              B|  | -DM=0
            // B|                   B| D|                 D|
            //
            // In case 3 and 4, the rule is that the smallest delta between (C-A) and (B-D) determine which of +DM or -DM is zero.
            // In case 7, (C-A) and (B-D) are equal, so both +DM and -DM are zero.
            // The rules remain the same when A=B and C=D (when the highs equal the lows).
            //
            // When calculating the DM over a period > 1, the one-period DM for the desired period is summated first.
            // In other words, for a +DM14, sum the +DM1 for the first 14 days (that's 13 values because there is no DM for the first day!).
            // Subsequent DM are calculated using the Wilder's smoothing approach:
            //
            //                                   Previous +DM14
            // Today's +DM14 = Previous +DM14 -  -------------- + Today's +DM1
            //                                         14

            if (double.IsNaN(sampleHigh) || double.IsNaN(sampleLow))
                return double.NaN;
            if (sampleHigh < sampleLow)
            {
                double temp = sampleLow;
                sampleLow = sampleHigh;
                sampleHigh = temp;
            }
            lock (Lock)
            {
                if (noSmoothing)
                {
                    if (Primed)
                    {
                        double deltaMinus = previousLow - sampleLow;
                        double deltaPlus = sampleHigh - previousHigh;
                        if (deltaPlus > 0 && deltaPlus > deltaMinus)
                            directionalMovementPlus = deltaPlus; // Case 1 and 3.
                        else
                            directionalMovementPlus = 0d;
                    }
                    else
                    {
                        if (0 < count++)
                        {
                            double deltaMinus = previousLow - sampleLow;
                            double deltaPlus = sampleHigh - previousHigh;
                            if (deltaPlus > 0 && deltaPlus > deltaMinus)
                                directionalMovementPlus = deltaPlus; // Case 1 and 3.
                            else
                                directionalMovementPlus = 0d;
                            Primed = true;
                        }
                    }
                }
                else
                {
                    if (Primed)
                    {
                        double deltaMinus = previousLow - sampleLow;
                        double deltaPlus = sampleHigh - previousHigh;
                        if (deltaPlus > 0 && deltaPlus > deltaMinus) // Case 1 and 3.
                            directionalMovementPlusPrevious += -directionalMovementPlusPrevious / Length + deltaPlus;
                        else // Case 2,4,5 and 7.
                            directionalMovementPlusPrevious += - directionalMovementPlusPrevious / Length;
                        directionalMovementPlus = directionalMovementPlusPrevious;
                    }
                    else
                    {
                        if (0 < count && Length >= count)
                        {
                            double deltaMinus = previousLow - sampleLow;
                            double deltaPlus = sampleHigh - previousHigh;
                            if (Length > count)
                            {
                                if (deltaPlus > 0 && deltaPlus > deltaMinus) // Case 1 and 3.
                                    directionalMovementPlusPrevious += deltaPlus;
                            }
                            else
                            {
                                if (deltaPlus > 0 && deltaPlus > deltaMinus) // Case 1 and 3.
                                    directionalMovementPlusPrevious += -directionalMovementPlusPrevious / Length + deltaPlus;
                                else // Case 2,4,5 and 7.
                                    directionalMovementPlusPrevious += - directionalMovementPlusPrevious / Length;
                                directionalMovementPlus = directionalMovementPlusPrevious;
                                Primed = true;
                            }
                
                        }
                        ++count;
                    }
                }
                previousLow = sampleLow;
                previousHigh = sampleHigh;
                return directionalMovementPlus;
            }
        }

        /// <summary>
        /// The Directional Movement Plus can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same sample value is used as a substitute for <c>High</c> and <c>Low</c> of an Ohlcv.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            return Update(sample, sample);
        }

        /// <summary>
        /// The Directional Movement Plus can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same sample value is used as a substitute for <c>High</c> and <c>Low</c> of an Ohlcv.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Scalar scalar)
        {
            double sample = scalar.Value;
            return new Scalar(scalar.Time, Update(sample, sample));
        }

        /// <summary>
        /// Updates the value of the Directional Movement Plus indicator.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            return new Scalar(ohlcv.Time, Update(ohlcv.High, ohlcv.Low));
        }
        #endregion
    }
}