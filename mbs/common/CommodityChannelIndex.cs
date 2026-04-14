using System;
using System.Globalization;
using Mbs.Trading.Data;
using Mbs.Trading.Indicators.Abstractions;

namespace Mbs.Trading.Indicators
{
    /// <summary>
    /// <para>The indicator is not primed during the first <c>ℓ-1</c> updates.</para>
    /// </summary>
    public sealed class CommodityChannelIndex : Indicator, ILineIndicator
    {
        #region Members and accessors

        /// <summary>
        /// The length (<c>ℓ</c>, the number of time periods) of the commodity channel index.
        /// </summary>
        public int Length { get; }

        private double value = double.NaN;
        /// <summary>
        /// The current value of the the commodity channel index, or <c>NaN</c> if not primed.
        /// The indicator is not primed during the first <c>ℓ-1</c> updates, where e <c>ℓ</c> is the length.
        /// </summary>
        public double Value { get { lock (Lock) { return value; } } }

        /// <summary>
        /// The inverse scaling factor to provide more readable value numbers.
        /// The default value of 0.015 ensures that approximately 70 to 80 percent of CCI values would fall between -100 and +100.
        /// </summary>
        public double InverseScalingFactor { get; }

        private readonly int lastIndex;
        private int windowCount;
        private readonly double[] window;
        private double windowSum;
        private readonly double scalingFactor;

        /// <summary>
        /// The default inverse scaling factor to provide more readable value numbers.
        /// </summary>
        public const double DefaultInverseScalingFactor = 0.015;

        private const string Cci = "CCI";
        private const string CciFull = "Commodity Channel Index";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="CommodityChannelIndex"/> class.
        /// </summary>
        /// <param name="length">The length, <c>ℓ</c>, of the commodity channel index.</param>
        /// <param name="inverseScalingFactor">The factor to provide more readable value numbers. The default value of 0.015 ensures that approximately 70 to 80 percent of CCI values would fall between -100 and +100.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public CommodityChannelIndex(int length, double inverseScalingFactor = DefaultInverseScalingFactor, OhlcvComponent ohlcvComponent = OhlcvComponent.TypicalPrice)
            : base(Cci, CciFull, ohlcvComponent)
        {
            if (2 > length)
                throw new ArgumentOutOfRangeException(nameof(length));
            InverseScalingFactor = inverseScalingFactor;
            scalingFactor = length / inverseScalingFactor;
            Length = length;
            lastIndex = length - 1;
            window = new double[length];
            Moniker = string.Concat(Cci, length.ToString(CultureInfo.InvariantCulture));
        }
        #endregion

        #region Reset
        /// <inheritdoc />
        public override void Reset()
        {
            lock (Lock)
            {
                Primed = false;
                windowCount = 0;
                windowSum = 0d;
                value = double.NaN;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the value of the commodity channel index.
        /// The indicator is not primed during the first <c>ℓ-1</c> updates.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            if (double.IsNaN(sample))
                return sample;
            lock (Lock)
            {
                double average, temp;
                if (Primed)
                {
                    windowSum += sample - window[0];
                    Array.Copy(window, 1, window, 0, lastIndex);
                    //for (int i = 0; i < lastIndex;)
                    //    window[i] = window[++i];
                    window[lastIndex] = sample;
                    average = windowSum / Length;
                    temp = 0d;
                    for (int i = 0; i < Length;)
                        temp += Math.Abs(window[i++] - average);
                    value = (Math.Abs(temp) < double.Epsilon) ? 0d : (scalingFactor * (sample - average) / temp);
                }
                else // Not primed.
                {
                    windowSum += sample;
                    window[windowCount] = sample;
                    if (Length == ++windowCount)
                    {
                        Primed = true;
                        average = windowSum / Length;
                        temp = 0d;
                        for (int i = 0; i < Length;)
                            temp += Math.Abs(window[i++] - average);
                        value = (Math.Abs(temp) < double.Epsilon) ? 0d : (scalingFactor * (sample - average) / temp);
                    }
                }
                return value;
            }
        }

        /// <summary>
        /// Updates the value of the commodity channel index.
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
        /// Updates the value of the commodity channel index.
        /// The indicator is not primed during the first <c>ℓ-1</c> updates.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Scalar scalar)
        {
            return new Scalar(scalar.Time, Update(scalar.Value));
        }

        /// <summary>
        /// Updates the value of the commodity channel index.
        /// The indicator is not primed during the first <c>ℓ-1</c> updates.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            return new Scalar(ohlcv.Time, Update(ohlcv.Component(OhlcvComponent)));
        }
        #endregion
    }
}
