using System;
using System.Runtime.Serialization;
//using System.Threading.Tasks;
using System.Windows.Media;

using Mbst.Numerics;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Corona.
    /// <para />
    /// </summary>
    [DataContract]
    internal sealed class Corona
    {
        #region Filter
        internal struct Filter
        {
            internal double Inphase, Inphase1;
            internal double Quadrature, Quadrature1;
            internal double Real, Real1;
            internal double Imaginary, Imaginary1;
            internal double AmplitudeSquared;
            internal double Decibels;

            internal void Clear()
            {
                Inphase = 0;
                Inphase1 = 0;
                Quadrature = 0;
                Quadrature1 = 0;
                Real = 0;
                Real1 = 0;
                Imaginary = 0;
                Imaginary1 = 0;
                AmplitudeSquared = 0;
                Decibels = 0;
            }
        }
        #endregion

        #region Members and accessors
        [DataMember]
        private long sampleCount;
        [DataMember]
        private double samplePrevious;
        [DataMember]
        internal bool IsPrimed;
        [DataMember]
        internal double MaximalAmplitudeSquared;

        #region Detrending (high-pass filter)
        private const int highPassFilterBufferSize = 6;
        private const int highPassFilterBufferSizeMinOne = highPassFilterBufferSize - 1;
        [DataMember]
        private readonly double[] highPassFilterBuffer = new double[highPassFilterBufferSize];

        /// <summary>
        /// α = (1 - sin(2π/λ)) / cos(2π/λ),
        /// <para>where λ is the high-pass filter cutoff  (detrending period).</para>
        /// </summary>
        [DataMember]
        private readonly double alpha;

        /// <summary>
        /// (1 + α)/2
        /// </summary>
        [DataMember]
        private readonly double halfOnePlusAlpha;

        /// <summary>
        /// The high-pass filter cutoff (detrending period). Suggested values are 20, 30, 100.
        /// </summary>
        [DataMember]
        internal readonly int HighPassFilterCutoff;
        #endregion

        #region Smoothing (low-pass 6-tap FIR filter)
        [DataMember]
        private double smoothedHighPassFilter;
        [DataMember]
        private double smoothedHighPassFilterPrevious;

        /// <summary>
        /// new int[highPassFilterBufferSize]
        /// </summary>
        private static readonly int[] highPassFilterSmoothingCoefficients = { 1, 2, 3, 3, 2, 1 };

        /// <summary>
        /// Sum of the coefficients of the <c>highPassFilterSmoothingCoefficients</c>.
        /// </summary>
        private const int highPassFilterSmoothingDenominator = 12;
        #endregion

        #region Filter bank
        private const double deltaLowerThreshold = 0.1; //0.1 or 0.15?
        private const double deltaFactor = -0.015;
        private const double deltaSummand = 0.5;
        private readonly double decibelsUpperThreshold = 20;

        /// <summary>
        /// The filter bank.
        /// </summary>
        [DataMember]
        internal readonly Filter[] FilterBank;

        /// <summary>
        /// The length of the filter bank, 2 * (maximal period - minimal period).
        /// </summary>
        [DataMember]
        internal readonly int FilterBankLength;

        /// <summary>
        /// 2 * minimal period. This is used to simulate 0.5 period step using integer indices.
        /// </summary>
        [DataMember]
        internal readonly int MinimalPeriodTimesTwo;

        /// <summary>
        /// 2 * maximal period. This is used to simulate 0.5 period step using integer indices.
        /// </summary>
        [DataMember]
        internal readonly int MaximalPeriodTimesTwo;

        /// <summary>
        /// The minimal period.
        /// </summary>
        [DataMember]
        internal readonly int MinimalPeriod;

        /// <summary>
        /// The maximal period.
        /// </summary>
        [DataMember]
        internal readonly int MaximalPeriod;

        /// <summary>
        /// Contains pre-calculated βᵢ=cos(2π/λᵢ), where λᵢ = {MinimalPeriodTimesTwo/2 ... MaximalPeriodTimesTwo/2}.
        /// </summary>
        [DataMember]
        private readonly double[] precalculatedBeta;
        #endregion

        #region Dominant cycle
        private readonly double decibelsLowerThreshold = 6;
        private const int dominantCycleMedianIndex = 2;
        private const int dominantCycleBufferSize = 5;
        private const int dominantCycleBufferSizeMinOne = dominantCycleBufferSize - 1;
        [DataMember]
        private readonly double[] dominantCycleBuffer = new double[dominantCycleBufferSize];

        /// <summary>
        /// Serialization is not needed.
        /// </summary>
        private readonly double[] dominantCycleBufferSorted = new double[dominantCycleBufferSize];

        /// <summary>
        /// The current value of the dominant cycle.
        /// </summary>
        [DataMember]
        internal double DominantCycle = double.MaxValue;

        /// <summary>
        /// The current value of the dominant cycle median.
        /// </summary>
        [DataMember]
        internal double DominantCycleMedian = double.MaxValue;
        #endregion
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the class.
        /// </summary>
        /// <param name="highPassFilterCutoff">The high-pass filter cutoff (detrending period). Suggested values are 20, 30, 100.</param>
        /// <param name="minimalPeriod">The minimal period. The default value is 6.</param>
        /// <param name="maximalPeriod">The maximal period. The default value is 30.</param>
        /// <param name="decibelsLowerThreshold">The lower threshold for decibels. The default value is 6.</param>
        /// <param name="decibelsUpperThreshold">The upper threshold for decibels. The default value is 20.</param>
        internal Corona(int highPassFilterCutoff = 30, int minimalPeriod = 6, int maximalPeriod = 30, double decibelsLowerThreshold = 6, double decibelsUpperThreshold = 20)
        {
            HighPassFilterCutoff = highPassFilterCutoff;

            double phi = Constants.TwoPi / highPassFilterCutoff;
            alpha = (1 - Math.Sin(phi)) / Math.Cos(phi);
            halfOnePlusAlpha = (1 + alpha) / 2;

            this.decibelsLowerThreshold = decibelsLowerThreshold;
            this.decibelsUpperThreshold = decibelsUpperThreshold;

            MinimalPeriod = minimalPeriod;
            MaximalPeriod = maximalPeriod;
            MinimalPeriodTimesTwo = minimalPeriod * 2;
            MaximalPeriodTimesTwo = maximalPeriod * 2;
            FilterBankLength = MaximalPeriodTimesTwo - MinimalPeriodTimesTwo + 1;
            FilterBank = new Filter[FilterBankLength];

            precalculatedBeta = new double[FilterBankLength];
            for (int i = MinimalPeriodTimesTwo; i <= MaximalPeriodTimesTwo; ++i)
                precalculatedBeta[i - MinimalPeriodTimesTwo] = Math.Cos(Constants.TwoPi * 2 / i);
        }
        #endregion

        #region Reset
        /// <summary>
        /// Resets the indicator.
        /// </summary>
        internal void Reset()
        {
            IsPrimed = false;
            sampleCount = 0;
            samplePrevious = 0;
            smoothedHighPassFilter = 0;
            DominantCycle = double.MaxValue;
            DominantCycleMedian = double.MaxValue;
            foreach (var filter in FilterBank)
                filter.Clear();
            for (int i = 0; i < highPassFilterBufferSize; ++i )
                highPassFilterBuffer[i] = 0;
            for (int i = 0; i < dominantCycleBufferSize; ++i)
                dominantCycleBuffer[i] = double.MaxValue;
        }
        #endregion

        #region Update
        internal bool Update(double sample)
        {
            if (1 == ++sampleCount)
            {
                samplePrevious = sample;
                return false;
            }

            // Detrend data by high-pass filtering with a selected period cutoff.
            double highPassFilter = alpha * highPassFilterBuffer[highPassFilterBufferSizeMinOne] + halfOnePlusAlpha * (sample - samplePrevious);
            samplePrevious = sample;
            for (int i = 0; i < highPassFilterBufferSizeMinOne;)
                highPassFilterBuffer[i] = highPassFilterBuffer[++i];
            highPassFilterBuffer[highPassFilterBufferSizeMinOne] = highPassFilter;

            // 6-tap low-pass FIR filter.
            smoothedHighPassFilter = 0;
            for (int i = 0; i < highPassFilterBufferSize; ++i)
                smoothedHighPassFilter += highPassFilterBuffer[i] * highPassFilterSmoothingCoefficients[i];
            smoothedHighPassFilter /= highPassFilterSmoothingDenominator;

            // Calculate the momentum(1) on the smoothed detrended data.
            double momentum = smoothedHighPassFilter - smoothedHighPassFilterPrevious;
            smoothedHighPassFilterPrevious = smoothedHighPassFilter;

            double delta = deltaFactor * sampleCount + deltaSummand;
            if (delta < deltaLowerThreshold)
                delta = deltaLowerThreshold;

            MaximalAmplitudeSquared = 0;
            for (int i = MinimalPeriodTimesTwo; i <= MaximalPeriodTimesTwo; ++i)
            //Parallel.For(MinimalPeriodTimesTwo, MaximalPeriodTimesTwo + 1, i =>
            {
                double gamma = 1 / Math.Cos(8 * Constants.Pi * delta / i);
                double a = gamma - Math.Sqrt(gamma * gamma - 1);
                double quadrature = momentum * (i / (4 * Constants.Pi));
                double inphase = smoothedHighPassFilter;
                double halfOneMinA = (1 - a) / 2;
                int index = i - MinimalPeriodTimesTwo;
                double betaOnePlusA = precalculatedBeta[index] * (1 + a);

                Filter filter = FilterBank[index];
                double real = halfOneMinA * (inphase - filter.Inphase1) + betaOnePlusA * filter.Real - a * filter.Real1;
                double imaginary = halfOneMinA * (quadrature - filter.Quadrature1) + betaOnePlusA * filter.Imaginary - a * filter.Imaginary1;
                filter.Inphase1 = filter.Inphase;
                filter.Inphase = inphase;
                filter.Quadrature1 = filter.Quadrature;
                filter.Quadrature = quadrature;
                filter.Real1 = filter.Real;
                filter.Real = real;
                filter.Imaginary1 = filter.Imaginary;
                filter.Imaginary = imaginary;

                double amplitudeSquared = real * real + imaginary * imaginary;
                if (amplitudeSquared > MaximalAmplitudeSquared)
                    MaximalAmplitudeSquared = amplitudeSquared;
                filter.AmplitudeSquared = amplitudeSquared;
                FilterBank[index] = filter;
            }//);

            double numerator = 0, denominator = 0;
            DominantCycle = 0;
            for (int i = MinimalPeriodTimesTwo; i <= MaximalPeriodTimesTwo; ++i)
            //Parallel.For(MinimalPeriodTimesTwo, MaximalPeriodTimesTwo + 1, i =>
            {
                int index = i - MinimalPeriodTimesTwo;
                Filter filter = FilterBank[index];

                // Calculate smoothed decibels.
                double normalized, decibels = 0;
                if (MaximalAmplitudeSquared > 0 && (normalized = filter.AmplitudeSquared / MaximalAmplitudeSquared) > 0)
                    decibels = 10 * Math.Log10((1 - 0.99 * normalized) / 0.01);
                decibels = 0.33 * decibels + 0.67 * filter.Decibels;
                if (decibels > decibelsUpperThreshold)
                    decibels = decibelsUpperThreshold;
                FilterBank[index].Decibels = decibels;

                // Dominant cycle calculation.
                if (decibels <= decibelsLowerThreshold)
                {
                    decibels = decibelsUpperThreshold - decibels;
                    numerator += i * decibels;
                    denominator += decibels;
                }
            }//);
            if (Math.Abs(denominator) > double.Epsilon)
                DominantCycle = 0.5 * numerator / denominator;
            if (DominantCycle < MinimalPeriod)
                DominantCycle = MinimalPeriod;

            // Dominant cycle median calculation.
            for (int i = 0; i < dominantCycleBufferSizeMinOne;)
                dominantCycleBuffer[i] = dominantCycleBuffer[++i];
            dominantCycleBuffer[dominantCycleBufferSizeMinOne] = DominantCycle;
            for (int i = 0; i < dominantCycleBufferSize; ++i)
                dominantCycleBufferSorted[i] = dominantCycleBuffer[i];
            Array.Sort(dominantCycleBufferSorted);
            DominantCycleMedian = dominantCycleBufferSorted[dominantCycleMedianIndex];
            if (DominantCycleMedian < MinimalPeriod)
                DominantCycleMedian = MinimalPeriod;

            if (MinimalPeriodTimesTwo > sampleCount)
                return false;
            IsPrimed = true;
            return true;
        }
        #endregion

        #region Color interpolation
        private static readonly Func<Color,byte> redSelector = color => color.R;
        private static readonly Func<Color, byte> greenSelector = color => color.G;
        private static readonly Func<Color, byte> blueSelector = color => color.B;
        private static readonly Func<Color, byte> alphaSelector = color => color.A;

        private static byte InterpolateComponent(Color endPoint1, Color endPoint2, double lambda, Func<Color, byte> selector)
        {
            return (byte)(selector(endPoint1) + (selector(endPoint2) - selector(endPoint1)) * lambda);
        }

        internal static Color InterpolateBetween(Color downPoint, Color upPoint, double lambda)
        {
            if (lambda < 0 || lambda > 1)
                throw new ArgumentOutOfRangeException("lambda");
            return Color.FromArgb(
                InterpolateComponent(downPoint, upPoint, lambda, alphaSelector),
                InterpolateComponent(downPoint, upPoint, lambda, redSelector),
                InterpolateComponent(downPoint, upPoint, lambda, greenSelector),
                InterpolateComponent(downPoint, upPoint, lambda, blueSelector));
        }
        #endregion
    }
}