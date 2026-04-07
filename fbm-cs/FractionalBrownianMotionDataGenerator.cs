using System;
using System.Globalization;
using Mbs.Numerics.RandomGenerators;
using Mbs.Numerics.RandomGenerators.FractionalBrownianMotion;
using Mbs.Trading.Data.Entities;
using Mbs.Trading.Time;
using Mbs.Trading.Time.Conventions;

namespace Mbs.Trading.Data.Generators.FractionalBrownianMotion
{
    /// <summary>
    /// The fractional Brownian motion waveform generator produces samples which form
    /// a fractional Brownian motion defined by the Hurst exponent, amplitude and range.
    /// An optional noise may be added to the samples.
    /// </summary>
    /// <typeparam name="T">A temporal entity type.</typeparam>
    public abstract class FractionalBrownianMotionDataGenerator<T> : WaveformDataGenerator<T>
        where T : TemporalEntity, new()
    {
        private readonly double[] samples;
        private readonly INormalRandomGenerator normalRandomGenerator;
        private int index;
        private bool isForward = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="FractionalBrownianMotionDataGenerator{T}"/> class.
        /// </summary>
        /// <param name="timeParameters">The time-related input parameters for the <see cref="TemporalDataGenerator{T}"/>.</param>
        /// <param name="waveformParameters">The input parameters for the <see cref="WaveformDataGenerator{T}"/>.</param>
        /// <param name="fractionalBrownianMotionParameters">The input parameters for the <see cref="FractionalBrownianMotionDataGenerator{T}"/>.</param>
        protected FractionalBrownianMotionDataGenerator(
            TimeParameters timeParameters,
            WaveformParameters waveformParameters,
            FractionalBrownianMotionParameters fractionalBrownianMotionParameters)
            : base(timeParameters, waveformParameters)
        {
            Algorithm = fractionalBrownianMotionParameters.Algorithm;
            normalRandomGenerator = fractionalBrownianMotionParameters.NormalRandomGeneratorKind.NormalRandomGenerator(
                fractionalBrownianMotionParameters.Seed,
                fractionalBrownianMotionParameters.AssociatedUniformRandomGeneratorKind);
            SampleAmplitude = fractionalBrownianMotionParameters.Amplitude;
            SampleMinimum = fractionalBrownianMotionParameters.MinimalValue;
            HurstExponent = fractionalBrownianMotionParameters.HurstExponent;
            samples = new double[WaveformSamples];
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FractionalBrownianMotionDataGenerator{T}"/> class.
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
        /// <param name="algorithm">The fractional Brownian motion algorithm.</param>
        /// <param name="normalRandomGenerator">The normal random generator.</param>
        /// <param name="sampleAmplitude">The amplitude of the fractional Brownian motion in sample units, should be positive.</param>
        /// <param name="sampleMinimum">The sample value corresponding to the minimum of the fractional Brownian motion, should be positive.</param>
        /// <param name="hurstExponent">The value of the Hurst exponent of the fractional Brownian motion; H∈[0, 1].</param>
        protected FractionalBrownianMotionDataGenerator(
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
            FractionalBrownianMotionAlgorithm algorithm,
            INormalRandomGenerator normalRandomGenerator,
            double sampleAmplitude,
            double sampleMinimum,
            double hurstExponent)
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
            Algorithm = algorithm;
            this.normalRandomGenerator = normalRandomGenerator;
            SampleAmplitude = sampleAmplitude;
            SampleMinimum = sampleMinimum;
            HurstExponent = hurstExponent;
            samples = new double[waveformSamples];
            Initialize();
        }

        /// <summary>
        /// Gets the fractional Brownian motion algorithm.
        /// </summary>
        public FractionalBrownianMotionAlgorithm Algorithm { get; }

        /// <summary>
        /// Gets the amplitude of the fractional Brownian motion in sample units, should be positive.
        /// </summary>
        public double SampleAmplitude { get; }

        /// <summary>
        /// Gets the sample value corresponding to the minimum of the fractional Brownian motion, should be positive.
        /// </summary>
        public double SampleMinimum { get; }

        /// <summary>
        /// Gets the value of the Hurst exponent of the fractional Brownian motion; H∈[0, 1].
        /// </summary>
        public double HurstExponent { get; }

        /// <inheritdoc />
        public override void Reset()
        {
            base.Reset();
            index = 0;
            isForward = true;
            normalRandomGenerator.Reset();
            GenerateFbm();
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

        private void GenerateFbm()
        {
            FractionalBrownianMotionGenerator.Generate(Algorithm, normalRandomGenerator, WaveformSamples, samples, SampleAmplitude, HurstExponent, true);

            double min = double.MaxValue;
            double max = double.MinValue;
            foreach (double sample in samples)
            {
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
                "fBm({0}, l={1}, H={2:0.####}) ∙ {3:0.####} + {4:0.####}",
                Algorithm,
                WaveformSamples,
                HurstExponent,
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

            GenerateFbm();
        }
    }
}
