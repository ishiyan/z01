using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Developed by Larry Williams, Williams %R is a momentum indicator that is the inverse of the Stochastic Oscillator.
    /// As the Stochastic Oscillator, it reflects the level of the closing price relative to the previous high-low range over a number of periods.
    /// The oscillation ranges from 0 to -100; readings from 0 to -20 are considered overbought, readings from -80 to -100 are considered oversold.
    /// <para />
    /// The value is calculated as
    /// <para />
    /// %Rᵢʳ = -100∙(Hᵢʳ - Closeᵢ) / (Hᵢʳ - Lᵢʳ)
    /// <para />
    /// where Lᵢʳ = min {Lowᵢ … Lowᵢ₋ᵣ}, Hᵢʳ = max {Highᵢ … Highᵢ₋ᵣ}, and r is the period in bars.
    /// <para />
    /// The default value r = 14.
    /// </summary>
    [DataContract]
    public sealed class WilliamsPercentR : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region Length
        [DataMember]
        private readonly int length;
        /// <summary>
        /// The length (the number of time periods, <c>ℓ</c>) of the <c>%R</c>.
        /// </summary>
        public int Length { get { return length; } }
        #endregion

        #region Value
        [DataMember]
        private double value = double.NaN;
        /// <summary>
        /// The current value of the <c>%E</c> or <c>NaN</c> if not primed.
        /// </summary>
        public double Value { get { lock (updateLock) { return value; } } }
        #endregion

        [DataMember]
        private DateTime time;
        [DataMember]
        private readonly int lengthMinOne;
        [DataMember]
        private int circularInputIndex;
        [DataMember]
        private int circularInputCount;
        [DataMember]
        private readonly double[] lowCircularInput;
        [DataMember]
        private readonly double[] highCircularInput;

        private const string willr = "WILL%R";
        private const string willrFull = "Williams %R";
        private const string lengthArgument = "length";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="WilliamsPercentR"/> class.
        /// </summary>
        /// <param name="length">The length (the number of time periods, <c>ℓ</c>) of the <c>%R</c>. The typical value are 5, 9 or 14 periods. The default value used by Larry Williams is 14.</param>
        public WilliamsPercentR(int length = 14)
            : base(willr, willrFull)
        {
            if (2 > length)
                throw new ArgumentOutOfRangeException(lengthArgument);
            this.length = length;
            lengthMinOne = length - 1;
            moniker = string.Concat(willr, length.ToString(CultureInfo.InvariantCulture));
            lowCircularInput = new double[length];
            highCircularInput = new double[length];
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
                value = double.NaN;
                time = new DateTime(0L);
                circularInputIndex = 0;
                circularInputCount = 0;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// The Williams %R indicator can be updated only with <c>ohlcv</c> samples.
        /// In this case, the same sample value is used as a substitute for <c>High</c>, <c>Low</c> and <c>Close</c> of an Ohlcv.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            return Update(sample, sample, sample);
        }

        /// <summary>
        /// The Williams %R indicator can be updated only with <c>ohlcv</c> samples.
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
        /// The Williams %R indicator can be updated only with <c>ohlcv</c> samples.
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
        /// Updates the value of the Williams %R. The indicator is not primed during the first <c>ℓ</c> updates.
        /// </summary>
        /// <param name="sampleClose">The <c>close</c> value of a new sample.</param>
        /// <param name="sampleHigh">The <c>high</c> value of a new sample.</param>
        /// <param name="sampleLow">The <c>low</c> value of a new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sampleClose, double sampleHigh, double sampleLow)
        {
            if (double.IsNaN(sampleClose) || double.IsNaN(sampleHigh) || double.IsNaN(sampleLow))
                return double.NaN;
            lock (updateLock)
            {
                int index = circularInputIndex;
                lowCircularInput[index] = sampleLow;
                highCircularInput[index] = sampleHigh;
                // Update circular buffer index.
                if (++circularInputIndex > lengthMinOne)
                    circularInputIndex = 0;
                if (length > circularInputCount)
                {
                    if (lengthMinOne == circularInputCount)
                    {
                        double minLow = lowCircularInput[index];
                        double maxHigh = highCircularInput[index];
                        for (int i = 0; i < lengthMinOne; ++i)
                        {
                            // The value of the index is always positive here.
                            --index;
                            double temp = lowCircularInput[index];
                            if (minLow > temp)
                                minLow = temp;
                            temp = highCircularInput[index];
                            if (maxHigh < temp)
                                maxHigh = temp;
                        }
                        if (Math.Abs(maxHigh - minLow) < double.Epsilon)
                            value = 0d;
                        else
                            value = -100d * (maxHigh - sampleClose) / (maxHigh - minLow);
                        primed = true;
                    }
                    ++circularInputCount;
                }
                else
                {
                    double minLow = lowCircularInput[index];
                    double maxHigh = highCircularInput[index];
                    for (int i = 0; i < lengthMinOne; ++i)
                    {
                        if (index == 0)
                            index = lengthMinOne;
                        else
                            --index;
                        double temp = lowCircularInput[index];
                        if (minLow > temp)
                            minLow = temp;
                        temp = highCircularInput[index];
                        if (maxHigh < temp)
                            maxHigh = temp;
                    }
                    if (Math.Abs(maxHigh - minLow) < double.Epsilon)
                        value = 0d;
                    else
                        value = -100d * (maxHigh - sampleClose) / (maxHigh - minLow);
                }
                return value;
            }
        }

        /// <summary>
        /// Updates the value of the Williams %R.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            time = ohlcv.Time;
            return new Scalar(time, Update(ohlcv.Close, ohlcv.High, ohlcv.Low));
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns the string that represents this object.
        /// </summary>
        /// <returns>Returns the string that represents this object.</returns>
        public override string ToString()
        {
            double r; bool p;
            lock (updateLock)
            {
                p = primed;
                r = value;
            }
            var sb = new StringBuilder();
            sb.Append("[M:");
            sb.Append(moniker);
            sb.Append(" P:");
            sb.Append(p);
            sb.Append(" %R:");
            sb.Append(r);
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
