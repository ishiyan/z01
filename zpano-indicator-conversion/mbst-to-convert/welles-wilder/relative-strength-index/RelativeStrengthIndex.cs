using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Relative Strength Index (RSI) is a momentum oscillator measuring the velocity and magnitude of directional sample movements.
    /// <para>For each sample an upward change <c>Gain</c> or downward change <c>Loss</c> is calculated:</para>
    /// <para><c>Gainᵢ = Pᵢ - Pᵢ₋₁, Loss = 0</c> for <c>Pᵢ &gt; Pᵢ₋₁</c>,</para>
    /// <para><c>Lossᵢ = Pᵢ₋₁ - Pᵢ, Gain = 0</c> for <c>Pᵢ &lt; Pᵢ₋₁</c>.</para>
    /// An average for <c>Gain</c> is calculated with an exponential moving average using a given <c>ℓ</c>-period smoothing factor, and likewise for <c>Loss</c>.
    /// <para>The smoothing is implemented using the original Wilder's approach:</para>
    /// <para>❶ multiply the previous by <c>ℓ-1</c>,</para>
    /// <para>❷ add the current value,</para>
    /// <para>❸ divide by <c>ℓ</c>.</para>
    /// The RSI value is calculated as
    /// <para><c>RSIᵢ = 100 Gainᵢ / (Gainᵢ + Lossᵢ)</c>.</para>
    /// <para>Metastock is starting the calculation one sample earlier. To make this possible, they assume that the very first sample will be identical to the previous one (no gain or loss).</para>
    /// The Relative Strength Index was developed by Welles Wilder and published in a 1978 book, "New Concepts in Technical Trading Systems".
    /// <para>The indicator is not primed during the first <c>ℓ</c> updates.</para>
    /// </summary>
    [DataContract]
    public sealed class RelativeStrengthIndex : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region Length
        [DataMember]
        private readonly int length;
        /// <summary>
        /// The length (the number of time periods).
        /// </summary>
        public int Length { get { return length; } }
        #endregion

        #region Value
        [DataMember]
        private double value = double.NaN;
        /// <summary>
        /// The current value of the the relative strength index, or <c>NaN</c> if not primed.
        /// The indicator is not primed during the first <c>ℓ</c> updates, where <c>ℓ</c> is the length.
        /// </summary>
        public double Value { get { lock (updateLock) { return value; } } }
        #endregion

        [DataMember]
        private int count = -1;
        [DataMember]
        private readonly int length1;
        [DataMember]
        private double previousSample;
        [DataMember]
        private double previousGain;
        [DataMember]
        private double previousLoss;

        private const string rsi = "RSI";
        private const string rsiFull = "Relative Strength Index";
        private const string argumentLength = "length";
        private const double epsilon = 0.00000001;
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="RelativeStrengthIndex"/> class.
        /// </summary>
        /// <param name="length">The number of time periods, <c>ℓ</c>.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public RelativeStrengthIndex(int length, OhlcvComponent ohlcvComponent = OhlcvComponent.ClosingPrice)
            : base(rsi, rsiFull, ohlcvComponent)
        {
            if (2 > length)
                throw new ArgumentOutOfRangeException(argumentLength);
            this.length = length;
            length1 = length - 1;
            moniker = string.Concat(rsi, length.ToString(CultureInfo.InvariantCulture));
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
                count = -1;
                previousSample = 0d;
                previousGain = 0d;
                previousLoss = 0d;
                value = double.NaN;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the value of the relative strength index
        /// The indicator is not primed during the first <c>ℓ</c> updates.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            if (double.IsNaN(sample))
                return sample;
            lock (updateLock)
            {
                double temp;
                if (primed)
                {
                    temp = sample - previousSample;
                    previousSample = sample;
                    previousLoss *= length1;
                    previousGain *= length1;
                    if (0d > temp)
                        previousLoss -= temp;
                    else
                        previousGain += temp;
                    previousLoss /= length;
                    previousGain /= length;
                    temp = previousLoss + previousGain;
                    if (epsilon < Math.Abs(temp))
                    {
                        value = previousGain / temp;
                        value *= 100d;
                    }
                    else
                        value = 0d;
                }
                else // Not primed.
                {
                    if (0 == ++count)
                        previousSample = sample;
                    else
                    {
                        temp = sample - previousSample;
                        previousSample = sample;
                        if (0d > temp)
                            previousLoss -= temp;
                        else
                            previousGain += temp;
                        if (length == count)
                        {
                            primed = true;
                            previousGain /= length;
                            previousLoss /= length;
                            temp = previousLoss + previousGain;
                            if (epsilon < Math.Abs(temp))
                            {
                                value = previousGain / temp;
                                value *= 100d;
                            }
                            else
                                value = 0d;
                        }
                    }
                }
                return value;
            }
        }

        /// <summary>
        /// Updates the value of the relative strength index.
        /// The indicator is not primed during the first <c>ℓ</c> updates.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(double sample, DateTime dateTime)
        {
            return new Scalar(dateTime, Update(sample));
        }

        /// <summary>
        /// Updates the value of the relative strength index.
        /// The indicator is not primed during the first <c>ℓ</c> updates.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Scalar scalar)
        {
            return new Scalar(scalar.Time, Update(scalar.Value));
        }

        /// <summary>
        /// Updates the value of the relative strength index.
        /// The indicator is not primed during the first <c>ℓ</c> updates.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            return new Scalar(ohlcv.Time, Update(ohlcv.Component(ohlcvComponent)));
        }
        #endregion

        #region Calculate
        /// <summary>
        /// Calculates the relative strength index from the input array.
        /// The indicator is not primed during the first <c>ℓ</c> updates.
        /// </summary>
        /// <param name="sampleList">The sample list.</param>
        /// <param name="length">The number of time periods, <c>ℓ</c>.</param>
        /// <returns>A list of the relative strength index values.</returns>
        static public List<double> Calculate(List<double> sampleList, int length)
        {
            if (2 > length)
                throw new ArgumentOutOfRangeException(argumentLength);
            int i, count = sampleList.Count;
            var resultList = new List<double>(count);
            if (count < length)
            {
                for (i = 0; i < count; ++i)
                    resultList.Add(double.NaN);
            }
            else
            {
                double v, temp, length1 = length - 1;
                double previousSample = sampleList[0], previousLoss = 0d, previousGain = 0d;
                resultList.Add(double.NaN);
                for (i = 1; i < length; ++i)
                {
                    v = sampleList[i];
                    temp = v - previousSample;
                    previousSample = v;
                    if (0d > temp)
                        previousLoss -= temp;
                    else
                        previousGain += temp;
                    resultList.Add(double.NaN);
                }
                v = sampleList[i++];
                temp = v - previousSample;
                previousSample = v;
                if (0d > temp)
                    previousLoss -= temp;
                else
                    previousGain += temp;
                previousGain /= length;
                previousLoss /= length;
                temp = previousLoss + previousGain;
                if (epsilon < Math.Abs(temp))
                {
                    v = previousGain / temp;
                    v *= 100d;
                }
                else
                    v = 0d;
                resultList.Add(v);
                for (; i < count; ++i)
                {
                    v = sampleList[i];
                    temp = v - previousSample;
                    previousSample = v;
                    previousLoss *= length1;
                    previousGain *= length1;
                    if (0d > temp)
                        previousLoss -= temp;
                    else
                        previousGain += temp;
                    previousLoss /= length;
                    previousGain /= length;
                    temp = previousLoss + previousGain;
                    if (epsilon < Math.Abs(temp))
                    {
                        v = previousGain / temp;
                        v *= 100d;
                    }
                    else
                        v = 0d;
                    resultList.Add(v);
                }
            }
            return resultList;
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
