using System;
using System.Globalization;
using Mbs.Numerics.RandomGenerators;
using Mbs.Trading.Data.Entities;
using Mbs.Trading.Time;
using Mbs.Trading.Time.Conventions;

// https://github.com/crodriguezvega/geometric-brownian-motion/blob/master/src/ViewModels/ViewModel.cs
namespace Mbs.Trading.Data.Generators.GeometricBrownianMotion
{
    /// <summary>
    /// The fractional Brownian motion waveform generator produces samples which form
    /// a fractional Brownian motion defined by the Hurst exponent, amplitude and range.
    /// An optional noise may be added to the samples.
    /// </summary>
    /// <typeparam name="T">A temporal entity type.</typeparam>
    public abstract class GeometricBrownianMotionDataGenerator<T> : WaveformDataGenerator<T>
        where T : TemporalEntity, new()
    {
        private readonly INormalRandomGenerator normalRandomGenerator;
        private readonly double[] samples;
        private readonly double factor1;
        private readonly double factor2;
        private int index;
        private bool isForward = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometricBrownianMotionDataGenerator{T}"/> class.
        /// </summary>
        /// <param name="timeParameters">The time-related input parameters for the <see cref="TemporalDataGenerator{T}"/>.</param>
        /// <param name="waveformParameters">The input parameters for the <see cref="WaveformDataGenerator{T}"/>.</param>
        /// <param name="geometricBrownianMotionParameters">The input parameters for the <see cref="GeometricBrownianMotionDataGenerator{T}"/>.</param>
        protected GeometricBrownianMotionDataGenerator(
            TimeParameters timeParameters,
            WaveformParameters waveformParameters,
            GeometricBrownianMotionParameters geometricBrownianMotionParameters)
            : base(timeParameters, waveformParameters)
        {
            normalRandomGenerator = geometricBrownianMotionParameters.NormalRandomGeneratorKind.NormalRandomGenerator(
                geometricBrownianMotionParameters.Seed,
                geometricBrownianMotionParameters.AssociatedUniformRandomGeneratorKind);
            SampleAmplitude = geometricBrownianMotionParameters.Amplitude;
            SampleMinimum = geometricBrownianMotionParameters.MinimalValue;
            Drift = geometricBrownianMotionParameters.Drift;
            Volatility = geometricBrownianMotionParameters.Volatility;

            samples = new double[WaveformSamples];
            double dt = 1.0 / (WaveformSamples - 1);
            factor1 = (Drift - Volatility * Volatility / 2) * dt;
            factor2 = Volatility * Math.Sqrt(dt);

            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeometricBrownianMotionDataGenerator{T}"/> class.
        /// </summary>
        /// <param name="sessionBeginTime">The time of the beginning of the trading session.</param>
        /// <param name="sessionEndTime">The end time of the trading session.</param>
        /// <param name="startDate">The date of the first data sample.</param>
        /// <param name="timeGranularity">The time granularity of data samples.</param>
        /// <param name="businessDayCalendar">The value specifying an exchange holiday schedule or a general country holiday schedule.</param>
        /// <param name="waveformSamples">The number of samples in the waveform, should be ≥ 2..</param>
        /// <param name="offsetSamples">The number of samples before the very first waveform. The value of zero means the waveform starts immediately.</param>
        /// <param name="repetitionsCount">The number of repetitions of the waveform. The value of zero means infinite.</param>
        /// <param name="noiseAmplitudeFraction">The amplitude of the noise as a fraction of the mid price.</param>
        /// <param name="noiseRandomGenerator">The uniformly distributed random generator to produce the noise.</param>
        /// <param name="normalRandomGenerator">The normal random generator.</param>
        /// <param name="sampleAmplitude">The amplitude of the geometric Brownian motion in sample units, should be positive.</param>
        /// <param name="sampleMinimum">The sample value corresponding to the minimum of the geometric Brownian motion, should be positive.</param>
        /// <param name="drift">The value of the drift, μ, of the geometric Brownian motion.</param>
        /// <param name="volatility">The value of the volatility, σ, of the geometric Brownian motion.</param>
        protected GeometricBrownianMotionDataGenerator(
            TimeSpan sessionBeginTime,
            TimeSpan sessionEndTime,
            DateTime startDate,
            TimeGranularity timeGranularity,
            BusinessDayCalendar businessDayCalendar,
            int waveformSamples,
            int offsetSamples,
            int repetitionsCount,
            double noiseAmplitudeFraction,
            IRandomGenerator noiseRandomGenerator,
            INormalRandomGenerator normalRandomGenerator,
            double sampleAmplitude,
            double sampleMinimum,
            double drift,
            double volatility)
            : base(
                sessionBeginTime,
                sessionEndTime,
                startDate,
                timeGranularity,
                businessDayCalendar,
                waveformSamples,
                offsetSamples,
                repetitionsCount,
                noiseAmplitudeFraction,
                noiseRandomGenerator)
        {
            this.normalRandomGenerator = normalRandomGenerator;
            SampleAmplitude = sampleAmplitude;
            SampleMinimum = sampleMinimum;
            Drift = drift;
            Volatility = volatility;

            samples = new double[WaveformSamples];
            double dt = 1.0 / (WaveformSamples - 1);
            factor1 = (drift - volatility * volatility / 2) * dt;
            factor2 = volatility * Math.Sqrt(dt);

            Initialize();
        }

        /// <summary>
        /// Gets the amplitude of the geometric Brownian motion in sample units, should be positive.
        /// </summary>
        public double SampleAmplitude { get; }

        /// <summary>
        /// Gets the sample value corresponding to the minimum of the geometric Brownian motion, should be positive.
        /// </summary>
        public double SampleMinimum { get; }

        /// <summary>
        /// Gets the value of the drift, μ, of the geometric Brownian motion.
        /// </summary>
        public double Drift { get; }

        /// <summary>
        /// Gets the value of the volatility, σ, of the geometric Brownian motion.
        /// </summary>
        public double Volatility { get; }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            index = 0;
            isForward = true;
            normalRandomGenerator.Reset();
        }

        /// <inheritdoc />
        protected override double OutOfWaveformSample()
        {
            return samples[0];
        }

        /// <inheritdoc />
        protected override double NextSample()
        {
            if (isForward)
            {
                if (++index >= WaveformSamples)
                {
                    isForward = false;
                    index = WaveformSamples - 2;
                }
            }
            else
            {
                if (--index < 0)
                {
                    isForward = true;
                    index = 1;
                }
            }

            return samples[index];
        }

        private void GenerateGbm()
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            double samplePrevious = SampleMinimum + SampleAmplitude / 2;
            for (int i = 0; i < WaveformSamples; ++i)
            {
                double z = normalRandomGenerator.NextDoubleStandard();
                double sample = samplePrevious * Math.Exp(factor1 + z * factor2);
                samplePrevious = sample;
                samples[i] = sample;

                if (min > sample)
                {
                    min = sample;
                }

                if (max < sample)
                {
                    max = sample;
                }
            }

            double delta = max - min;
            for (int i = 0; i < WaveformSamples; ++i)
            {
                samples[i] = SampleMinimum + SampleAmplitude * (samples[i] - min) / delta;
            }
        }

        private void Initialize()
        {
            Moniker = string.Format(
                CultureInfo.InvariantCulture,
                "gBm(l={0}, μ={1:0.####}, σ={2:0.####}) ∙ {3:0.####} + {4:0.####}",
                WaveformSamples,
                Drift,
                Volatility,
                SampleAmplitude,
                SampleMinimum);

            const double delta = 0.00005;
            if (HasNoise && NoiseAmplitudeFraction > delta)
            {
                Moniker = string.Format(CultureInfo.InvariantCulture, "{0} + noise(ρn={1:0.####})", Moniker, NoiseAmplitudeFraction);
            }

            if (OffsetSamples > 0)
            {
                Moniker = string.Format(CultureInfo.InvariantCulture, "{0}, off={1}", Moniker, OffsetSamples);
            }

            if (!IsRepetitionsInfinite)
            {
                Moniker = string.Format(CultureInfo.InvariantCulture, "{0}, rep={1}", Moniker, RepetitionsCount);
            }

            GenerateGbm();
        }
    }
}
