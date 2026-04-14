using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// <para>The indicator is not primed during the first <c>ℓ-1</c> updates.</para>
    /// </summary>
    [DataContract]
    public sealed class CommodityChannelIndex : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region Length
        [DataMember]
        private readonly int length;
        /// <summary>
        /// The length (<c>ℓ</c>, the number of time periods) of the commodity channel index.
        /// </summary>
        public int Length { get { return length; } }
        #endregion

        #region Value
        [DataMember]
        private double value = double.NaN;
        /// <summary>
        /// The current value of the the commodity channel index, or <c>NaN</c> if not primed.
        /// The indicator is not primed during the first <c>ℓ-1</c> updates, where e <c>ℓ</c> is the length.
        /// </summary>
        public double Value { get { lock (updateLock) { return value; } } }
        #endregion

        #region InverseScalingFactor
        [DataMember]
        private readonly double inverseScalingFactor;
        /// <summary>
        /// The inverse scaling factor to provide more readable value numbers.
        /// The default value of 0.015 ensures that approximately 70 to 80 percent of CCI values would fall between -100 and +100.
        /// </summary>
        public double InverseScalingFactor { get { return inverseScalingFactor; } }
        #endregion

        [DataMember]
        private readonly int lastIndex;
        [DataMember]
        private int windowCount;
        [DataMember]
        private readonly double[] window;
        [DataMember]
        private double windowSum;
        [DataMember]
        private readonly double scalingFactor;

        /// <summary>
        /// The default inverse scaling factor to provide more readable value numbers.
        /// </summary>
        public const double DefaultInverseScalingFactor = 0.015;
        private const string cci = "CCI";
        private const string cciFull = "Commodity Channel Index";
        private const string argumentLength = "length";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="CommodityChannelIndex"/> class.
        /// </summary>
        /// <param name="length">The length, <c>ℓ</c>, of the commodity channel index.</param>
        /// <param name="inverseScalingFactor">The factor to provide more readable value numbers. The default value of 0.015 ensures that approximately 70 to 80 percent of CCI values would fall between -100 and +100.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public CommodityChannelIndex(int length, double inverseScalingFactor = DefaultInverseScalingFactor, OhlcvComponent ohlcvComponent = OhlcvComponent.TypicalPrice)
            : base(cci, cciFull, ohlcvComponent)
        {
            if (2 > length)
                throw new ArgumentOutOfRangeException(argumentLength);
            this.inverseScalingFactor = inverseScalingFactor;
            scalingFactor = length / inverseScalingFactor;
            this.length = length;
            lastIndex = length - 1;
            window = new double[length];
            moniker = string.Concat(cci, length.ToString(CultureInfo.InvariantCulture));
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
            lock (updateLock)
            {
                double average, temp;
                if (primed)
                {
                    windowSum += sample - window[0];
                    Array.Copy(window, 1, window, 0, lastIndex);
                    //for (int i = 0; i < lastIndex;)
                    //    window[i] = window[++i];
                    window[lastIndex] = sample;
                    average = windowSum / length;
                    temp = 0d;
                    for (int i = 0; i < length;)
                        temp += Math.Abs(window[i++] - average);
                    value = (Math.Abs(temp) < double.Epsilon) ? 0d : (scalingFactor * (sample - average) / temp);
                }
                else // Not primed.
                {
                    windowSum += sample;
                    window[windowCount] = sample;
                    if (length == ++windowCount)
                    {
                        primed = true;
                        average = windowSum / length;
                        temp = 0d;
                        for (int i = 0; i < length;)
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
            return new Scalar(ohlcv.Time, Update(ohlcv.Component(ohlcvComponent)));
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
            sb.Append(" L:");
            sb.Append(length);
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
