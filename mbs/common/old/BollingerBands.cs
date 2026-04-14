using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Bollinger bands are a type of price envelope invented by John Bollinger in the 1980s. Bollinger bands consist of:
    /// <para>❶ a middle band being an <c>ℓ</c>-period moving average (MA)</para>
    /// <para>❷ an upper band at <c>K</c> times an <c>ℓ</c>-period standard deviation <c>σ</c> above the middle band (<c>MA + Kσ</c>)</para>
    /// <para>❸ an lower band at <c>K</c> times an <c>ℓ</c>-period standard deviation <c>σ</c> below the middle band (<c>MA - Kσ</c>)</para>
    /// <para>Typical values for <c>ℓ</c> and <c>Kĸ</c> are 20 and 2, respectively. The default choice for the average is a simple moving average, but other types of averages can be employed as needed.</para>
    /// <para>Exponential moving averages are a common second choice. Usually the same period is used for both the middle band and the calculation of standard deviation.</para>
    /// </summary>
    [DataContract]
    public sealed class BollingerBands : Indicator, IBandIndicator
    {
        #region Members and accessors
        #region UpperValue
        [DataMember]
        private double upperValue = double.NaN;
        /// <summary>
        /// The upper value.
        /// </summary>
        public double UpperValue { get { lock (updateLock) { return upperValue; } } }
        #endregion

        #region MiddleValue
        [DataMember]
        private double middleValue = double.NaN;
        /// <summary>
        /// The middle value.
        /// </summary>
        public double MiddleValue { get { lock (updateLock) { return middleValue; } } }
        #endregion

        #region LowerValue
        [DataMember]
        private double lowerValue = double.NaN;
        /// <summary>
        /// The lower value.
        /// </summary>
        public double LowerValue { get { lock (updateLock) { return lowerValue; } } }
        #endregion

        #region UpperMultiplier
        [DataMember]
        private readonly double upperMultiplier;
        /// <summary>
        /// The upper multiplier.
        /// </summary>
        public double UpperMultiplier { get { lock (updateLock) { return upperMultiplier; } } }
        #endregion

        #region LowerMultiplier
        [DataMember]
        private readonly double lowerMultiplier;
        /// <summary>
        /// The lower multiplier.
        /// </summary>
        public double LowerMultiplier { get { lock (updateLock) { return lowerMultiplier; } } }
        #endregion

        #region BandWidthValue
        /// <summary>
        /// Tells how wide the Bollinger Bands are on a normalized basis: <c>BandWidth = (upper - lower) / middle</c>.
        /// </summary>
        public double BandWidthValue
        {
            get
            {
                lock (updateLock)
                {
                    return (Math.Abs(middleValue) < double.Epsilon) ?
                        1d : (upperValue - lowerValue) / middleValue;
                }
            }
        }
        #endregion

        #region PercentBandValue
        /// <summary>
        /// Equals 1 at the upper band and 0 at the lower band: <c>PercentBand = MiddleValue - LowerValue) / (UpperValue - LowerValue)</c>.
        /// </summary>
        public double PercentBandValue
        {
            get
            {
                lock (updateLock)
                {
                    double temp = upperValue - lowerValue;
                    return (Math.Abs(temp) < double.Epsilon) ? 1d :
                        (middleValue - lowerValue) / temp;
                }
            }
        }
        #endregion

        #region StandardDeviationValue
        /// <summary>
        /// The stadard deviation value.
        /// </summary>
        public double StandardDeviationValue
        {
            get
            {
                lock (updateLock)
                {
                    return standardDeviation.Value;
                }
            }
        }
        #endregion

        #region StandardDeviationLength
        [DataMember]
        private readonly int length;
        /// <summary>
        /// The standard deviation lookback length.
        /// </summary>
        public int StandardDeviationLength { get { return length; } }
        #endregion

        #region IsUnbiased
        /// <summary>
        /// If the estimate of the variance is the unbiased sample variance or the population variance. The default value is true.
        /// </summary>
        public bool IsUnbiased
        {
            get { return standardDeviation.IsUnbiased;}
        }
        #endregion

        #region SourceLineIndicator
        [DataMember]
        private readonly ILineIndicator sourceLineIndicator;
        /// <summary>
        /// The source line indicator.
        /// </summary>
        public ILineIndicator SourceLineIndicator { get { return sourceLineIndicator; } }
        #endregion

        [DataMember]
        private readonly bool hasUpperMultiplier;
        [DataMember]
        private readonly bool hasLowerMultiplier;
        [DataMember]
        private readonly StandardDeviation standardDeviation;

        private const string bb = "BBands";
        private const string bbFull = "Bollinger Bands";
        private const string argumentLength = "length";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="BollingerBands"/> object.
        /// </summary>
        /// <param name="sourceLineIndicator">The line indicator which serves as an input source.</param>
        /// <param name="length">The standard deviation lookback length.</param>
        /// <param name="multiplier">The lower and upper band multiplier.</param>
        /// <param name="unbiased">If the estimate of the variance is the unbiased sample variance or the population variance. The default value is true.</param>
        public BollingerBands(ILineIndicator sourceLineIndicator, int length, double multiplier, bool unbiased = true)
            : this(sourceLineIndicator, length, multiplier, multiplier, unbiased)
        {
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BollingerBands"/> object.
        /// </summary>
        /// <param name="ohlcvComponent">The ohlcv component.</param>
        /// <param name="length">The standard deviation lookback length.</param>
        /// <param name="multiplier">The lower and upper band multiplier.</param>
        public BollingerBands(OhlcvComponent ohlcvComponent, int length, double multiplier)
            : this(ohlcvComponent, length, multiplier, multiplier)
        {
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BollingerBands"/> object.
        /// </summary>
        /// <param name="ohlcvComponent">The ohlcv component.</param>
        /// <param name="length">The standard deviation lookback length.</param>
        /// <param name="multiplier">The lower and upper band multiplier.</param>
        /// <param name="unbiased">If the estimate of the variance is the unbiased sample variance or the population variance. The default value is true.</param>
        public BollingerBands(OhlcvComponent ohlcvComponent, int length, double multiplier, bool unbiased)
            : this(ohlcvComponent, length, multiplier, multiplier, unbiased)
        {
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BollingerBands"/> object.
        /// </summary>
        /// <param name="sourceLineIndicator">The line indicator which serves as an input source.</param>
        /// <param name="length">The standard deviation lookback length.</param>
        /// <param name="lowerMultiplier">The lower band multiplier.</param>
        /// <param name="upperMultiplier">The upper band multiplier.</param>
        /// <param name="unbiased">If the estimate of the variance is the unbiased sample variance or the population variance. The default value is true.</param>
        public BollingerBands(ILineIndicator sourceLineIndicator, int length, double lowerMultiplier = 1d, double upperMultiplier = 1d, bool unbiased = true)
            : base(bb, bbFull)
        {
            if (2 > length)
                throw new ArgumentOutOfRangeException(argumentLength);
            this.length = length;
            this.lowerMultiplier = lowerMultiplier;
            this.upperMultiplier = upperMultiplier;
            hasLowerMultiplier = Math.Abs(1d - lowerMultiplier) > double.Epsilon;
            hasUpperMultiplier = Math.Abs(1d - upperMultiplier) > double.Epsilon;
            this.sourceLineIndicator = sourceLineIndicator;
            standardDeviation = new StandardDeviation(length, unbiased);
            moniker = string.Concat(bb, length.ToString(CultureInfo.InvariantCulture), "[", sourceLineIndicator.Moniker, "]");
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="BollingerBands"/> object.
        /// </summary>
        /// <param name="ohlcvComponent">The ohlcv component.</param>
        /// <param name="length">The standard deviation lookback length.</param>
        /// <param name="lowerMultiplier">The lower band multiplier.</param>
        /// <param name="upperMultiplier">The upper band multiplier.</param>
        /// <param name="unbiased">If the estimate of the variance is the unbiased sample variance or the population variance. The default value is true.</param>
        public BollingerBands(OhlcvComponent ohlcvComponent, int length, double lowerMultiplier = 1d, double upperMultiplier = 1d, bool unbiased = true)
            : base(bb, bbFull, ohlcvComponent)
        {
            if (2 > length)
                throw new ArgumentOutOfRangeException(argumentLength);
            this.length = length;
            this.lowerMultiplier = lowerMultiplier;
            this.upperMultiplier = upperMultiplier;
            hasLowerMultiplier = Math.Abs(1d - lowerMultiplier) > double.Epsilon;
            hasUpperMultiplier = Math.Abs(1d - upperMultiplier) > double.Epsilon;
            standardDeviation = new StandardDeviation(length, unbiased);
            moniker = string.Concat(bb, length.ToString(CultureInfo.InvariantCulture));
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
                lowerValue = double.NaN;
                middleValue = double.NaN;
                upperValue = double.NaN;
                standardDeviation.Reset();
                if (null != sourceLineIndicator)
                    sourceLineIndicator.Reset();
            }
        }
        #endregion

        #region Update
        private Band DoUpdate(double sample, DateTime dateTime)
        {
            lock (updateLock)
            {
                if (double.IsNaN(sample))
                    return new Band(dateTime, double.NaN, double.NaN);
                middleValue = sample;
                double temp = standardDeviation.Update(sample);
                if (!primed)
                {
                    primed = standardDeviation.IsPrimed;
                    if (null != sourceLineIndicator)
                    {
                        if (primed && !sourceLineIndicator.IsPrimed)
                            primed = false;
                    }
                    if (!primed)
                        return new Band(dateTime, double.NaN, double.NaN);
                }
                lowerValue = sample - (hasLowerMultiplier ? temp * lowerMultiplier : temp);
                upperValue = sample + (hasUpperMultiplier ? temp * upperMultiplier : temp);
                return new Band(dateTime, upperValue, lowerValue);
            }
        }

        /// <summary>
        /// Updates the indicator.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The updated value of the indicator.</returns>
        public Band Update(double sample, DateTime dateTime)
        {
            if (null != sourceLineIndicator)
                sample = sourceLineIndicator.Update(sample);
            return DoUpdate(sample, dateTime);
        }

        /// <summary>
        /// Updates the indicator.
        /// </summary>
        /// <param name="sample">A new <c>scalar</c> sample.</param>
        /// <returns>The updated value of the indicator.</returns>
        public Band Update(Scalar sample)
        {
            if (null != sourceLineIndicator)
                sample = sourceLineIndicator.Update(sample);
            return DoUpdate(sample.Value, sample.Time);
        }

        /// <summary>
        /// Updates the indicator.
        /// </summary>
        /// <param name="sample">A new <c>ohlcv</c> sample.</param>
        /// <returns>The updated value of the indicator.</returns>
        public Band Update(Ohlcv sample)
        {
            if (null != sourceLineIndicator)
            {
                Scalar scalar = sourceLineIndicator.Update(sample);
                return DoUpdate(scalar.Value, scalar.Time);
            }
            return DoUpdate(sample.Component(ohlcvComponent), sample.Time);
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns the string that represents this object.
        /// </summary>
        /// <returns>Returns the string that represents this object.</returns>
        public override string ToString()
        {
            double lower, upper, middle; bool p;
            lock (updateLock)
            {
                p = primed;
                lower = lowerValue;
                upper = upperValue;
                middle = middleValue;
            }
            var sb = new StringBuilder();
            sb.Append("[M:");
            sb.Append(moniker);
            sb.Append(" P:");
            sb.Append(p);
            sb.Append(" L:");
            sb.Append(lower);
            sb.Append(" M:");
            sb.Append(middle);
            sb.Append(" U:");
            sb.Append(upper);
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
