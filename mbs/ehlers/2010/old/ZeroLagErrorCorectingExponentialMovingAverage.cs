using System;
using System.Runtime.Serialization;
using System.Globalization;

namespace Mbst.Trading.Indicators.JohnEhlers
{
    /// <summary>
    /// Adaptive zero-lag exponential moving average.
    /// <para/>
    /// See John Ehlers and Ric Way, 'Zero Lag (well, almost)', TASC, 2010, v28.11, pp30-35.
    /// </summary>
    [DataContract]
    public sealed class ZeroLagErrorCorectingExponentialMovingAverage : LineIndicator
    {
        #region Members and accessors
        /// <summary>
        /// The SMA length (<c>ℓ</c>) equivalent to the smoothing factor (<c>α</c>) of the EMA.<para/>
        /// <c>ℓ = 2/α - 1, 0 &lt; α ≤ 1, 1 ≤ ℓ</c>.
        /// </summary>
        public int Length => length;

        /// <summary>
        /// The smoothing factor (<c>α</c>) of the EMA.<para/>
        /// <c>α = 2/(ℓ + 1), 0 &lt; α ≤ 1, 1 ≤ ℓ</c> where <c>ℓ</c> is the equivalent SMA length.
        /// </summary>
        public double SmoothingFactor => alpha;

        /// <summary>
        /// Defines the range [-g, g] for finding the best gain factor.
        /// </summary>
        public double GainLimit => gainLimit;

        /// <summary>
        /// Defines the iteration step for finding the best gain factor.
        /// </summary>
        public double GainStep => gainStep;

        /// <summary>
        /// The default value of a SMA length equivalent to the smoothing factor of the EMA.
        /// </summary>
        public const int DefaultLength = 20;

        /// <summary>
        /// The default value of the smoothing factor of the EMA.
        /// </summary>
        public const double DefaultSmoothingFactor = 0.095;

        /// <summary>
        /// The default value of the range [-g, g] for finding the best gain factor.
        /// </summary>
        public const double DefaultGainLimit = 5;

        /// <summary>
        /// The default value of the iteration step for finding the best gain factor.
        /// </summary>
        public const double DefaultGainStep = 0.1;

        /// <summary>
        /// The current value of the indicator, or <c>NaN</c> if not primed.<para/>
        /// The indicator is not primed during two first updates.
        /// </summary>
        public double Value { get { lock (updateLock) { return primed ? value : double.NaN; } } }

        /// <summary>
        /// The current value of the underlying EMA, or <c>NaN</c> if not primed.<para/>
        /// The indicator is not primed during two first updates.
        /// </summary>
        public double EmaValue { get { lock (updateLock) { return primed ? emaValue : double.NaN; } } }

        [DataMember]
        private readonly double alpha;
        [DataMember]
        private readonly double oneMinAlpha;
        [DataMember]
        private readonly double gainLimit;
        [DataMember]
        private readonly double gainStep;
        [DataMember]
        private double value = double.NaN;
        [DataMember]
        private double emaValue = double.NaN;
        [DataMember]
        private readonly int length;
        [DataMember]
        private int count;

        private const double epsilon = 0.00000001;
        private const string zecema = "ehlersZecema";
        private const string zecemaFull = "John Ehlers Zero-lag Error-Correcting Exponential Moving Average";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the class.
        /// </summary>
        /// <param name="length">The SMA length (<c>ℓ</c>) equivalent to the smoothing factor (<c>α</c>) of the exponential moving average<para/>
        /// <c>ℓ = 2/α - 1, 0 &lt; α ≤ 1, 1 ≤ ℓ</c>.<para/>
        /// The default value is <c>DefaultLength</c>.
        /// </param>
        /// <param name="gainLimit">Defines the range [-g, g] for finding the best gain factor.<para/>
        /// The default value is <c>DefaultGainLimit</c>.
        /// </param>
        /// <param name="gainStep">Defines the iteration step for finding the best gain factor.<para/>
        /// The default value is <c>DefaultGainStep</c>.
        /// </param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public ZeroLagErrorCorectingExponentialMovingAverage(int length = DefaultLength,
            double gainLimit = DefaultGainLimit, double gainStep = DefaultGainStep, OhlcvComponent ohlcvComponent = OhlcvComponent.ClosingPrice)
            : this(length, double.NaN, gainLimit, gainStep, ohlcvComponent) {}

        /// <summary>
        /// Constructs a new instance of the class.
        /// </summary>
        /// <param name="smoothingFactor">The smoothing factor, <c>α, 0 &lt; α ≤ 1</c>, of the exponential moving average.<para/>
        /// <c>α = 2/(ℓ + 1), 0 &lt; α ≤ 1, 1 ≤ ℓ</c> where <c>ℓ</c> is the equivalent SMA length.<para/>
        /// The default value is <c>DefaultSmoothingFactor</c>.
        /// </param>
        /// <param name="gainLimit">Defines the range [-g, g] for finding the best gain factor.<para/>
        /// The default value is <c>DefaultGainLimit</c>.
        /// </param>
        /// <param name="gainStep">Defines the iteration step for finding the best gain factor.<para/>
        /// The default value is <c>DefaultGainStep</c>.
        /// </param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public ZeroLagErrorCorectingExponentialMovingAverage(double smoothingFactor /*= DefaultSmoothingFactor*/,
            double gainLimit = DefaultGainLimit, double gainStep = DefaultGainStep, OhlcvComponent ohlcvComponent = OhlcvComponent.ClosingPrice)
            : this(0, smoothingFactor, gainLimit, gainStep, ohlcvComponent) {}

        private ZeroLagErrorCorectingExponentialMovingAverage(int length, double smoothingFactor,
            double gainLimit, double gainStep, OhlcvComponent ohlcvComponent)
            : base(zecema, zecemaFull, ohlcvComponent)
        {
            if (double.IsNaN(smoothingFactor))
            {
                if (1 > length)
                    throw new ArgumentOutOfRangeException(nameof(length));
                this.length = length;
                alpha = 2d / (1 + length);
            }
            else
            {
                if (0d > smoothingFactor || 1d < smoothingFactor)
                    throw new ArgumentOutOfRangeException(nameof(smoothingFactor));
                alpha = smoothingFactor;
                if (epsilon > smoothingFactor)
                    length = int.MaxValue;
                else
                    length = (int)Math.Round(2d / smoothingFactor) - 1;
            }
            if (0 >= gainLimit)
                throw new ArgumentOutOfRangeException(nameof(gainLimit));
            if (0 >= gainStep)
                throw new ArgumentOutOfRangeException(nameof(gainStep));
            oneMinAlpha = 1 - alpha;
            this.gainLimit = gainLimit;
            this.gainStep = gainStep;
            moniker = string.Format(CultureInfo.InvariantCulture, "{0}(ℓ={1}(α={2:0.####}),gl={3:0.####},gs={4:0.####})",
                zecema, length, alpha, gainLimit, gainStep);
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
                count = 0;
                value = double.NaN;
                emaValue = double.NaN;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the value of the indicator.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public override double Update(double sample)
        {
            if (double.IsNaN(sample))
                return sample;
            lock (updateLock)
            {
                if (primed)
                    value= Calculate(sample);
                else
                {
                    if (++count == 1)
                        emaValue = sample;
                    else if (count == 2)
                    {
                        emaValue = CalculateEma(sample);
                        value = emaValue;
                    }
                    else
                    {
                        value= Calculate(sample);
                        primed = true;
                    }
                    if (!primed)
                        return double.NaN;
                }
                return value;
            }
        }
        #endregion

        #region Implementation
        private double CalculateEma(double sample)
        {
            return alpha * sample + oneMinAlpha * emaValue;
        }

        private double Calculate(double sample)
        {
            emaValue = CalculateEma(sample);
            double leastError = double.MaxValue;
            double bestErrorCorrection = 0;
            for (double gain = -gainLimit; gain <= gainLimit; gain += gainStep)
            {
                double errorCorrection = alpha * (emaValue + gain * (sample - value)) + oneMinAlpha * value;
                double error = Math.Abs(sample - errorCorrection); 
                if (leastError > error)
                {
                    leastError = error;
                    bestErrorCorrection = errorCorrection;
                }
            }
            return bestErrorCorrection;
        }
        #endregion
    }
}
