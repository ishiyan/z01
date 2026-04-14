using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// </summary>
    [DataContract]
    public sealed class CombBandPassSpectrum : Indicator, IHeatmapIndicator
    {
        #region Members and accessors
        #region MinParameterValue
        /// <summary>
        /// The minimum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns.
        /// </summary>
        public double MinParameterValue { get { return estimator.MinPeriod; } }
        #endregion

        #region MaxParameterValue
        /// <summary>
        /// The maximum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns.
        /// </summary>
        public double MaxParameterValue { get { return estimator.MaxPeriod; } }
        #endregion

        #region Length
        /// <summary>
        /// The length (the number of time periods) of the sample window.
        /// This also determines the maximum spectrum period.
        /// </summary>
        [DataMember]
        public int Length { get { return estimator.Length; } }
        #endregion

        #region Default raster colors
        /// <summary>
        /// A default color associated with the minimal heatmap raster value.
        /// </summary>
        public static readonly Color DefaultMinRasterColor = Colors.Black;

        /// <summary>
        /// A default color associated with a middle heatmap raster value.
        /// </summary>
        public static readonly Color DefaultMidRasterColor = Colors.Red;

        /// <summary>
        /// A default color associated with the maximal heatmap raster value.
        /// </summary>
        public static readonly Color DefaultMaxRasterColor = Colors.Yellow;

        /// <summary>
        /// A default interploation type between the minimal heatmap color and the middle heatmap color.
        /// </summary>
        public const ColorInterpolationType DefaultMinMidColorInterpolationType = ColorInterpolationType.Quadratic;

        /// <summary>
        /// A default interploation type between the middle heatmap color and the maximal heatmap color.
        /// </summary>
        public const ColorInterpolationType DefaultMidMaxColorInterpolationType = ColorInterpolationType.Quadratic;
        #endregion

        [DataMember]
        private readonly Color minRasterColor;
        [DataMember]
        private readonly Color midRasterColor;
        [DataMember]
        private readonly Color maxRasterColor;
        [DataMember]
        private readonly ColorInterpolationType minMidColorInterpolationType;
        [DataMember]
        private readonly ColorInterpolationType midMaxColorInterpolationType;
        [DataMember]
        private int windowCount;
        [DataMember]
        private CombBandPassSpectrumEstimator estimator;
        [DataMember]
        private readonly int lastIndex;
        [DataMember]
        private readonly double parameterRange;
        [DataMember]
        private readonly bool floatingNormalization;

        private const string cbps = "cbps";
        private const string cbpsFull = "Comb BandPass Spectrum";
        private const string argumentLength = "length";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the class.
        /// </summary>
        /// <param name="minRasterColor">The color associated with the minimal heatmap raster value.</param>
        /// <param name="midRasterColor">The color associated with a middle heatmap raster value.</param>
        /// <param name="maxRasterColor">The color associated with the maximal heatmap raster value.</param>
        /// <param name="minMidColorInterpolationType">An interploation type between the minimal heatmap color and the middle heatmap color.</param>
        /// <param name="midMaxColorInterpolationType">An interploation type between the middle heatmap color and the maximal heatmap color.</param>
        /// <param name="length">The number of time periods in the spectrum window.</param>
        /// <param name="minPeriod">The minimum period to calculate, must be less than <c>length</c>.</param>
        /// <param name="isSpectralDilationCompensation">Specifies if the spectral dilation should be compensated.</param>
        /// <param name="isAutomaticGainControl">Specifies if the <c>fast attack − slow decay</c> automatic gain control should be used.</param>
        /// <param name="automaticGainControlDecayFactor">Specifies the decay factor for the <c>fast attack − slow decay</c> automatic gain control.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        /// <param name="floatingNormalization">Specifies if to use the floating normalization.</param>
        public CombBandPassSpectrum(Color minRasterColor, Color midRasterColor, Color maxRasterColor,
            ColorInterpolationType minMidColorInterpolationType, ColorInterpolationType midMaxColorInterpolationType,
            int length = 48, int minPeriod = 10, bool isSpectralDilationCompensation = true,
            bool isAutomaticGainControl = true, double automaticGainControlDecayFactor = 0.991,
            OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice, bool floatingNormalization = true)
            : base(cbps, cbpsFull, ohlcvComponent)
        {
            if (2 > length)
                throw new ArgumentOutOfRangeException(argumentLength);
            estimator = new CombBandPassSpectrumEstimator(length, minPeriod,
                isSpectralDilationCompensation, isAutomaticGainControl, automaticGainControlDecayFactor);
            lastIndex = estimator.Length - 1;
            moniker = string.Concat(cbps, length.ToString(CultureInfo.InvariantCulture));
            parameterRange = estimator.MaxPeriod - estimator.MinPeriod;

            this.minRasterColor = minRasterColor;
            this.midRasterColor = midRasterColor;
            this.maxRasterColor = maxRasterColor;
            this.minMidColorInterpolationType = minMidColorInterpolationType;
            this.midMaxColorInterpolationType = midMaxColorInterpolationType;
            this.floatingNormalization = floatingNormalization;
        }

        /// <summary>
        /// Constructs a new instance of the class.
        /// </summary>
        /// <param name="length">The number of time periods in the spectrum window.</param>
        /// <param name="minPeriod">The minimum period to calculate, must be less than <c>length</c>.</param>
        /// <param name="isSpectralDilationCompensation">Specifies if the spectral dilation should be compensated.</param>
        /// <param name="isAutomaticGainControl">Specifies if the <c>fast attack − slow decay</c> automatic gain control should be used.</param>
        /// <param name="automaticGainControlDecayFactor">Specifies the decay factor for the <c>fast attack − slow decay</c> automatic gain control.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        /// <param name="floatingNormalization">Specifies if to use the floating normalization.</param>
        public CombBandPassSpectrum(int length = 48, int minPeriod = 10,
            bool isSpectralDilationCompensation = true, bool isAutomaticGainControl = true,
            double automaticGainControlDecayFactor = 0.991, OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice,
            bool floatingNormalization = true)
            : this(DefaultMinRasterColor, DefaultMidRasterColor, DefaultMaxRasterColor,
            DefaultMinMidColorInterpolationType, DefaultMidMaxColorInterpolationType,
            length, minPeriod,
            isSpectralDilationCompensation, isAutomaticGainControl, automaticGainControlDecayFactor, ohlcvComponent,
            floatingNormalization)
        {
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
                estimator.Reset();
            }
        }
        #endregion

        #region Update
        private Heatmap Update(double sample, DateTime dateTime)
        {
            if (double.IsNaN(sample))
                return null;
            double[] window = estimator.InputSeries;
            if (primed)
            {
                Array.Copy(window, 1, window, 0, lastIndex);
                //for (int i = 0; i < lastIndex; )
                //    window[i] = window[++i];
                window[lastIndex] = sample;
            }
            else // Not primed.
            {
                window[windowCount] = sample;
                if (estimator.Length == ++windowCount)
                    primed = true;
            }
            double[] intensity;
            Brush brush;
            if (primed)
            {
                estimator.Calculate();
                int lengthSpectrum = estimator.LengthSpectrum;
                intensity = new double[lengthSpectrum];
                var gradientStopCollection = new GradientStopCollection(lengthSpectrum);
                double min = estimator.SpectrumMin;
                double max = estimator.SpectrumMax;
                double spectrumRange = max - min;
                double mid = ((max + min) / 2 - min) / spectrumRange;
                if (!floatingNormalization)
                {
                    min = 0;
                    spectrumRange = max;
                    mid = 0.5;
                }
                window = estimator.Spectrum;
                for (int i = 0; i < lengthSpectrum; ++i)
                {
                    double value = (window[i] - min) / spectrumRange;
                    double offset = (estimator.Period[i] - estimator.MinPeriod) / parameterRange;
                    intensity[i] = value;
                    value /= mid;
                    GradientStop gradientStop;
                    if (value <= 1)
                        gradientStop = new GradientStop(ColorInterploation.InterpolateBetween(minRasterColor, midRasterColor, value, minMidColorInterpolationType), offset);
                    else // value > 1
                    {
                        --value;
                        if (value > 1)
                            value = 1;
                        gradientStop = new GradientStop(ColorInterploation.InterpolateBetween(midRasterColor, maxRasterColor, value, midMaxColorInterpolationType), offset);
                    }
                    gradientStopCollection.Add(gradientStop);
                }
                brush = new LinearGradientBrush(gradientStopCollection, new Point(0, 1), new Point(0, 0));
            }
            else
            {
                intensity = null;
                brush = null;
            }
            return new Heatmap(dateTime, brush, intensity);
        }

        /// <summary>
        /// Updates the value of the indicator.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new heatmap column of the indicator or <c>null</c>.</returns>
        public Heatmap Update(Ohlcv ohlcv)
        {
            lock (updateLock)
            {
                return Update(ohlcv.Component(ohlcvComponent), ohlcv.Time);
            }
        }

        /// <summary>
        /// Updates the value of the indicator.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new heatmap column of the indicator or <c>null</c>.</returns>
        public Heatmap Update(Scalar scalar)
        {
            lock (updateLock)
            {
                return Update(scalar.Value, scalar.Time);
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns the string that represents this object.
        /// </summary>
        /// <returns>Returns the string that represents this object.</returns>
        public override string ToString()
        {
            bool p; int l;
            lock (updateLock)
            {
                p = primed;
                l = estimator.Length;
            }
            var sb = new StringBuilder();
            sb.Append("[M:");
            sb.Append(moniker);
            sb.Append(" L:");
            sb.Append(l);
            sb.Append(" P:");
            sb.Append(p);
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
