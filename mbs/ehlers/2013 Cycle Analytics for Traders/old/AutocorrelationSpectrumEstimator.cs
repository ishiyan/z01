using System;
using System.Runtime.Serialization;

using Mbst.Numerics;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Implements an autocorrelation to estimate the spectrum.
    /// </summary>
    [DataContract]
    internal sealed class AutocorrelationSpectrumEstimator
    {
        #region Members and accessors
        /// <summary>
        /// The length of the input series window.
        /// </summary>
        [DataMember]
        public readonly int Length;

        /// <summary>
        /// The length of the spectrum, <c>MaxPeriod - MinPeriod</c>.
        /// </summary>
        [DataMember]
        public readonly int LengthSpectrum;

        /// <summary>
        /// The minimal period.
        /// The value of 2 corresponds to the Nyquist (the maximum representable) frequency.
        /// </summary>
        public readonly int MinPeriod;

        /// <summary>
        /// The maximal period is equal to the observed time lapse (Lenth).
        /// </summary>
        [DataMember]
        public readonly int MaxPeriod;

        /// <summary>
        /// The number of last samples of the input series window which are used for averaging.
        /// The default value of 0 means the averaging length is equal to each lag.
        /// That way, the averaging contains the maximum number of data samples per lag.
        /// </summary>
        public int AveragingLength { get { return estimator.AveragingLength; } }

        /// <summary>
        /// An array of length <c>Length</c> containing input series window.
        /// </summary>
        [DataMember]
        public double[] InputSeries { get { return estimator.InputSeries; } }

        /// <summary>
        /// An array of length <c>LengthSpectrum</c> containing the estimated spectrum.
        /// </summary>
        [DataMember]
        public readonly double[] Spectrum;

        /// <summary>
        /// A minimum value of the estimated spectrum.
        /// </summary>
        [DataMember]
        public double SpectrumMin;

        /// <summary>
        /// A maximum value of the estimated spectrum.
        /// </summary>
        [DataMember]
        public double SpectrumMax;

        /// <summary>
        /// The spectum resolution (positive number).
        /// A value of 10 means that spectrum is evaluated at every 0.1 of periods amplitude.
        /// </summary>
        [DataMember]
        public readonly int SpectrumResolution;

        /// <summary>
        /// If the spectrum should is smoothed.
        /// </summary>
        [DataMember]
        public readonly bool IsSmoothed;

        /// <summary>
        /// An automatic gain control decay factor.
        /// </summary>
        [DataMember]
        public readonly double AutomaticGainControlDecayFactor;

        /// <summary>
        /// If the <c>fast attack − slow decay</c> automatic gain control is used.
        /// </summary>
        [DataMember]
        public readonly bool IsAutomaticGainControl;

        /// <summary>
        /// An array of length <c>LengthSpectrum</c> containing the frequencies corresponding to the estimated spectrum.
        /// </summary>
        [DataMember]
        public readonly double[] Frequency;

        /// <summary>
        /// An array of length <c>LengthSpectrum</c> containing the periods corresponding to the estimated spectrum.
        /// </summary>
        [DataMember]
        public readonly double[] Period;

        [DataMember]
        private readonly AutocorrelationEstimator estimator;
        [DataMember]
        private double previousSpectrumMax;
        [DataMember]
        private readonly double[] previousSpectrum;
        [DataMember]
        private readonly double[][] frequencySinOmega;
        [DataMember]
        private readonly double[][] frequencyCosOmega;
        [DataMember]
        private readonly int correlationCoefficientsLength;
        [DataMember]
        private readonly double[] correlationCoefficients;
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the class.
        /// </summary>
        /// <param name="length">The length of the input series window. This determines also the maximum period to calculate.</param>
        /// <param name="minPeriod">The minimum period to calculate, must be less than the <c>maxPeriod</c>.</param>
        /// <param name="maxPeriod">The maximum period to calculate, must be less than or equal to length.</param>
        /// <param name="spectrumResolution">The spectum resolution (positive number). A value of 10 means that spectrum is evaluated at every 0.1 of periods amplitude.</param>
        /// <param name="averagingLength">The number of last samples of the input series window which are used for averaging. The default value of 0 means the averaging length is equal to each lag. That way, the averaging contains the maximum number of data samples per lag.</param>
        /// <param name="isSmoothed">Specifies if the spectrum should be smoothed.</param>
        /// <param name="isAutomaticGainControl">Specifies if the <c>fast attack − slow decay</c> automatic gain control should be used.</param>
        /// <param name="automaticGainControlDecayFactor">Specifies the decay factor for the <c>fast attack − slow decay</c> automatic gain control.</param>
        public AutocorrelationSpectrumEstimator(int length = 48, int minPeriod = 3, int maxPeriod = 47, int spectrumResolution = 1,
            int averagingLength = 0, bool isSmoothed = true, bool isAutomaticGainControl = true, double automaticGainControlDecayFactor = 0.995)
        {
            if (2 > length)
                length = 2;
            Length = length;
            MinPeriod = 2 > minPeriod ? 2 : minPeriod;
            MaxPeriod = 2 > maxPeriod ? (MinPeriod + 1) : maxPeriod;
            estimator = new AutocorrelationEstimator(length, MinPeriod, MaxPeriod, averagingLength);
            //System.Diagnostics.Debug.Assert(length == estimator.Length);
            correlationCoefficientsLength = estimator.CorrelationCoefficientsLength;
            correlationCoefficients = estimator.CorrelationCoefficients;
            SpectrumResolution = spectrumResolution;
            LengthSpectrum = (MaxPeriod - MinPeriod) * spectrumResolution + 1;
            IsSmoothed = isSmoothed;
            IsAutomaticGainControl = isAutomaticGainControl;
            AutomaticGainControlDecayFactor = automaticGainControlDecayFactor;
            previousSpectrum = new double[LengthSpectrum];
            Spectrum = new double[LengthSpectrum];
            Frequency = new double[LengthSpectrum];
            frequencySinOmega = new double[LengthSpectrum][];
            frequencyCosOmega = new double[LengthSpectrum][];
            Period = new double[LengthSpectrum];

            // Frequency is calculated so that we can plot the spectrum as a funcion of period's length,
            // starting from the MaxPeriod down to the MinPeriod.
            for (int i = 0; i < LengthSpectrum; ++i)
            {
                previousSpectrum[i] = 0;
                double period = MaxPeriod - ((double)i / spectrumResolution);
                Period[i] = period;
                Frequency[i] = 1 / period;
                double theta = Constants.TwoPi / period;
                var sinOmega = new double[correlationCoefficientsLength];
                var cosOmega = new double[correlationCoefficientsLength];
                frequencySinOmega[i] = sinOmega;
                frequencyCosOmega[i] = cosOmega;
                for (int j = 0; j < correlationCoefficientsLength; ++j)
                {
                    double omega = j * theta;
                    sinOmega[j] = Math.Sin(omega);
                    cosOmega[j] = Math.Cos(omega);
                }
            }
        }
        #endregion

        #region Calculate
        /// <summary>
        /// Calculates the autocorrelation power spectrum estimation of the <c>InputSeries</c>.
        /// The <c>InputSeries</c> must be filled before this call.
        /// </summary>
        public void Calculate()
        {
            // The calculated correlation coefficients are in range [-1, 1].
            estimator.Calculate();

            // Create an array with the spectrum values.
            const double coeff1 = 0.2;
            const double coeff2 = 1 - coeff1;
            SpectrumMin = double.MaxValue;
            SpectrumMax = IsAutomaticGainControl ? AutomaticGainControlDecayFactor * previousSpectrumMax : double.MinValue;

            for (int i = 0; i < LengthSpectrum; ++i)
            {
                double[] sinOmega = frequencySinOmega[i];
                double[] cosOmega = frequencyCosOmega[i];
                double sumSin = 0, sumCos = 0;
                for (int j = 0; j < correlationCoefficientsLength; ++j)
                {
                    double sample = correlationCoefficients[j];
                    sumSin += sample * sinOmega[j];
                    sumCos += sample * cosOmega[j];
                }
                double spectrum = sumSin * sumSin + sumCos * sumCos;
                //spectrum *= spectrum;
                if (IsSmoothed)
                    spectrum = coeff1 * spectrum + coeff2 * previousSpectrum[i];
                previousSpectrum[i] = spectrum;
                Spectrum[i] = spectrum;
                if (SpectrumMax < spectrum)
                    SpectrumMax = spectrum;
                if (SpectrumMin > spectrum)
                    SpectrumMin = spectrum;
            }
            previousSpectrumMax = SpectrumMax;
        }
        #endregion

        #region Reset
        /// <summary>
        /// Resets the estimator.
        /// </summary>
        public void Reset()
        {
            previousSpectrumMax = 0;
            for (int i = 0; i < LengthSpectrum; ++i)
                previousSpectrum[i] = 0;
        }
        #endregion
    }
}
