using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Computes the Finite Impulse Responce (FIR) filter values. The impulse response (the filter's response to a Kronecker delta input)
    /// of an <c>ℓ</c>th-order FIR filter lasts for <c>ℓ+1</c> samples, and then dies to zero.
    /// The difference equation that defines the output of an FIR filter in terms of its input is a convolution of the coefficient sequence
    /// bi with the input signal.
    /// </summary>
    [DataContract]
    public class FiniteImpulseResponceFilter : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region Length
        [DataMember]
        private readonly int length;
        /// <summary>
        /// The length <c>ℓ</c> (the number of time periods).
        /// </summary>
        public int Length { get { return length; } }
        #endregion

        #region Value
        [DataMember]
        private double value = double.NaN;
        /// <summary>
        /// The current value of the Finite Impulse Responce filter, or <c>NaN</c> if not primed.
        /// The indicator is not primed during the first <c>ℓ-1</c> updates, where <c>ℓ</c> is the length.
        /// </summary>
        public double Value { get { lock (updateLock) { return value; } } }
        #endregion

        [DataMember]
        private readonly int lastIndex;
        [DataMember]
        private int windowCount;
        [DataMember]
        private double[] window;
        [DataMember]
        private double[] coefficients;
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="FiniteImpulseResponceFilter"/> class.
        /// </summary>
        /// <param name="name">The name of the filter.</param>
        /// <param name="moniker">The moniker of the filter.</param>
        /// <param name="description">The description of the filter.</param>
        /// <param name="coefficients">The coefficient values.</param>
        internal FiniteImpulseResponceFilter(string name, string moniker, string description, double[] coefficients)
            : this(name, moniker, description, coefficients, OhlcvComponent.ClosingPrice)
        {
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="FiniteImpulseResponceFilter"/> class.
        /// </summary>
        /// <param name="name">The name of the filter.</param>
        /// <param name="moniker">The moniker of the filter.</param>
        /// <param name="description">The description of the filter.</param>
        /// <param name="coefficients">The coefficient values.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        internal FiniteImpulseResponceFilter(string name, string moniker, string description, double[] coefficients, OhlcvComponent ohlcvComponent)
            : base(name, description, ohlcvComponent)
        {
            length = coefficients.Length;
            lastIndex = length - 1;
            window = new double[length];
            this.coefficients = coefficients;
            base.moniker = moniker;
        }
        #endregion

        #region Normalize
        /// <summary>
        /// Normalizes the coefficients to make the total sum equal 1.
        /// </summary>
        /// <returns>The value of the total sum before the normalization.</returns>
        // ReSharper disable once ParameterHidesMember
        protected double Normalize(out double[] coefficients, out double total)
        {
            double sum = 0d;
            for (int i = 0; i < length; i++)
                sum += this.coefficients[i];
            if (Math.Abs(1d - sum) < double.Epsilon)
            {
                coefficients = this.coefficients;
                total = sum;
            }
            else
            {
                coefficients = new double[length];
                for (int i = 0; i < length; i++)
                    coefficients[i] = this.coefficients[i] / sum;
                total = 0;
                for (int i = 0; i < length; i++)
                    total += coefficients[i];
            }
            return sum;
        }
        /// <summary>
        /// Normalizes the coefficients to make the total sum equal 1.
        /// </summary>
        /// <returns>The value of the total sum before the normalization.</returns>
        // ReSharper disable once ParameterHidesMember
        protected decimal Normalize(out decimal[] coefficients, out decimal total)
        {
            decimal sum = 0;
            coefficients = new decimal[length];
            for (int i = 0; i < length; i++)
                sum += new decimal(this.coefficients[i]);
            if (decimal.One == sum)
            {
                total = sum;
                for (int i = 0; i < length; i++)
                    coefficients[i] = new decimal(this.coefficients[i]);
            }
            else
            {
                for (int i = 0; i < length; i++)
                    coefficients[i] = new decimal(this.coefficients[i]) / sum;
                total = 0;
                for (int i = 0; i < length; i++)
                    total += coefficients[i];
            }
            return sum;
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
                value = double.NaN;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the value of the indicator.
        /// The indicator is not primed during the first<c>ℓ-1</c> updates.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            if (double.IsNaN(sample))
                return sample;
            lock (updateLock)
            {
                if (primed)
                {
                    Array.Copy(window, 1, window, 0, lastIndex);
                    //for (int i = 0; i < lastIndex; )
                    //    window[i] = window[++i];
                    window[lastIndex] = sample;
                    double temp = 0d;
                    for (int i = 0; i < length; i++)
                        temp += window[i] * coefficients[i];
                    value = temp;
                }
                else // Not primed.
                {
                    window[windowCount] = sample;
                    if (length == ++windowCount)
                    {
                        primed = true;
                        double temp = 0d;
                        for (int i = 0; i < length; i++)
                            temp += window[i] * coefficients[i];
                        value = temp;
                    }
                }
                return value;
            }
        }

        /// <summary>
        /// Updates the value of the indicator.
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
        /// Updates the value of the indicator.
        /// The indicator is not primed during the first <c>ℓ-1</c> updates.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Scalar scalar)
        {
            return new Scalar(scalar.Time, Update(scalar.Value));
        }

        /// <summary>
        /// Updates the value of the indicator.
        /// The indicator is not primed during the first <c>ℓ-1</c> updates.
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
        /// Updates the value of the indicator from the input array.
        /// The indicator is not primed during the first <c>ℓ-1</c> updates.
        /// </summary>
        /// <param name="sampleList">The sample list.</param>
        /// <param name="coefficients">The coefficient values.</param>
        /// <returns>A list of indicator values.</returns>
        static public List<double> Calculate(List<double> sampleList, double[] coefficients)
        {
            int i = 0, count = sampleList.Count, length = coefficients.Length, lastIndex = length - 1;
            var resultList = new List<double>(count);
            if (count < length)
            {
                for (; i < count; i++)
                    resultList.Add(double.NaN);
            }
            else
            {
                double v = 0d;
                // Normalize coefficients.
                for (i = 0; i < length; )
                    v += coefficients[i++];
                if (Math.Abs(1d - v) > double.Epsilon)
                {
                    var d = new double[length];
                    for (i = 0; i < length; i++)
                        d[i] = coefficients[i] / v;
                    coefficients = d;
                }
                for (i = 0; i < lastIndex; i++)
                    resultList.Add(double.NaN);
                int j, len = 0;
                for (; i < length; i++, len++)
                {
                    for (j = len; j < i; j++)
                        v += coefficients[j] * sampleList[j];
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
            StringBuilder sb = new StringBuilder();
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

