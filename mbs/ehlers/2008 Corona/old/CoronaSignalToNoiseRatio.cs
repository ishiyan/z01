using System;
using System.Runtime.Serialization;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Mbst.Charts;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// The Corona Signal to Noise Ratio is a measure of the cycle amplitude relative to noise.
    /// <para />
    /// The "noise" is chosen to be the average bar height because there is not much tradable information within the bar.
    /// <para />
    /// The vertical scale of the indicator is from 1 dB to 11 dB. The Signal to Noise Ratio starts
    /// to develop a “corona” below 4 dB, warning you that the ratio may become too low to safely
    /// swing trade on the basis of the cycle alone.
    /// </summary>
    [DataContract]
    public sealed class CoronaSignalToNoiseRatio : Indicator, IHeatmapIndicator
    {
        #region Members and accessors
        private const int highLowMedianIndex = 2;
        private const int highLowBufferSize = 5;
        private const int highLowBufferSizeMinOne = highLowBufferSize - 1;

        [DataMember]
        private readonly int rasterLength;
        [DataMember]
        private readonly double rasterStep;
        [DataMember]
        private bool isStarted;
        [DataMember]
        private readonly double[] highLowBuffer = new double[highLowBufferSize];
        [DataMember]
        private double noisePrevious;
        [DataMember]
        private double averageSamplePrevious;
        [DataMember]
        private double signalPrevious;

        /// <summary>
        /// Serialization is not needed.
        /// </summary>
        private readonly double[] highLowBufferSorted = new double[highLowBufferSize];

        /// <summary>
        /// The maximal raster value of the heatmap.
        /// </summary>
        [DataMember]
        private readonly double maxRasterValue;

        /// <summary>
        /// The middle raster value of the heatmap.
        /// </summary>
        [DataMember]
        private readonly double midRasterValue;

        /// <summary>
        /// The heatmap scaled to [0, maxRasterValue].
        /// </summary>
        [DataMember]
        private readonly double[] raster;

        #region MinParameterValue
        [DataMember]
        private readonly double minParameterValue;

        /// <summary>
        /// The minimum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns.
        /// </summary>
        public double MinParameterValue { get { return minParameterValue; } }
        #endregion

        #region MaxParameterValue
        [DataMember]
        private readonly double maxParameterValue;

        /// <summary>
        /// The maximum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns.
        /// </summary>
        public double MaxParameterValue { get { return maxParameterValue; } }
        #endregion

        #region SignalToNoiseRatio
        [DataMember]
        private double signalToNoiseRatio = double.NaN;

        /// <summary>
        /// The current value of the Corona signal to noise ratio in the range of [MinParameterValue, MaxParameterValue] or <c>NaN</c> if not primed.
        /// </summary>
        public double SignalToNoiseRatio { get { lock (updateLock) { return primed ? signalToNoiseRatio : double.NaN; } } }
        #endregion

        #region SignalToNoiseRatioFacade
        /// <summary>
        /// A line indicator façade to expose a value of the SignalToNoiseRatio as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the SignalToNoiseRatio from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade SignalToNoiseRatioFacade
        {
            get
            {
                return new LineIndicatorFacade(csnrName, csnrMoniker, csnrDescription, () => IsPrimed, () => SignalToNoiseRatio);
            }
        }
        #endregion

        #region Default raster colors
        /// <summary>
        /// A default color associated with the minimal heatmap raster value.
        /// </summary>
        public static readonly Color DefaultMinRasterColor = Color.FromArgb(255, 220, 255, 255);

        /// <summary>
        /// A default color associated with a middle heatmap raster value.
        /// </summary>
        public static readonly Color DefaultMidRasterColor = Color.FromArgb(255, 0, 185, 185);

        /// <summary>
        /// A default color associated with the maximal heatmap raster value.
        /// </summary>
        public static readonly Color DefaultMaxRasterColor = Colors.Black;

        /// <summary>
        /// A default interploation type between the minimal heatmap color and the middle heatmap color.
        /// </summary>
        public const ColorInterpolationType DefaultMinMidColorInterpolationType = ColorInterpolationType.Linear;

        /// <summary>
        /// A default interploation type between the middle heatmap color and the maximal heatmap color.
        /// </summary>
        public const ColorInterpolationType DefaultMidMaxColorInterpolationType = ColorInterpolationType.Linear;
        #endregion

        #region Default line styles
        /// <summary>
        /// A default signal to noise ratio line style.
        /// </summary>
        public const PredefinedLineStyle DefaultSignalToNoiseRatioLineStyle = PredefinedLineStyle.WhiteLine;
        #endregion

        [DataMember]
        private readonly bool isSlave;
        [DataMember]
        private readonly Corona corona;

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

        private const string csnrHeatmapName = "CoronaSignalToNoiseRatioHeatmap";
        private const string csnrHeatmapDescription = "Crona signal to noise ratio heatmap";
        private const string csnrHeatmapMoniker = "CoronaSNR(h)";

        private const string csnrName = "CoronaSignalToNoiseRatio";
        private const string csnrDescription = "Crona signal to noise ratio";
        private const string csnrMoniker = "CoronaSNR";
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
        /// <param name="rasterLength">The number of elements in the heatmap raster. The default value is 50.</param>
        /// <param name="maxRasterValue">The maximal raster value of the heatmap. The default value is 20.</param>
        /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is 1.</param>
        /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is 11.</param>
        /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
        /// <param name="highPassFilterCutoff">The high-pass filter cutoff (detrending period). Suggested values are 20, 30, 100.</param>
        /// <param name="minimalPeriod">The minimal period. The default value is 6.</param>
        /// <param name="maximalPeriod">The maximal period. The default value is 30.</param>
        public CoronaSignalToNoiseRatio(Color minRasterColor, Color midRasterColor, Color maxRasterColor,
            ColorInterpolationType minMidColorInterpolationType, ColorInterpolationType midMaxColorInterpolationType,
            int rasterLength = 50, double maxRasterValue = 20, double minParameterValue = 1, double maxParameterValue = 11,
            OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice,
            int highPassFilterCutoff = 30, int minimalPeriod = 6, int maximalPeriod = 30)
            : this(null, minRasterColor, midRasterColor, maxRasterColor, minMidColorInterpolationType, midMaxColorInterpolationType,
            rasterLength, maxRasterValue,
            minParameterValue, maxParameterValue, ohlcvComponent, highPassFilterCutoff, minimalPeriod, maximalPeriod)
        {
        }

        /// <summary>
        /// Constructs a new instance of the class.
        /// </summary>
        /// <param name="rasterLength">The number of elements in the heatmap raster. The default value is 50.</param>
        /// <param name="maxRasterValue">The maximal raster value of the heatmap. The default value is 20.</param>
        /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is 1.</param>
        /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is 11.</param>
        /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
        /// <param name="highPassFilterCutoff">The high-pass filter cutoff (detrending period). Suggested values are 20, 30, 100.</param>
        /// <param name="minimalPeriod">The minimal period. The default value is 6.</param>
        /// <param name="maximalPeriod">The maximal period. The default value is 30.</param>
        public CoronaSignalToNoiseRatio(int rasterLength = 50, double maxRasterValue = 20,
            double minParameterValue = 1, double maxParameterValue = 11,
            OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice,
            int highPassFilterCutoff = 30, int minimalPeriod = 6, int maximalPeriod = 30)
            : this(null, DefaultMinRasterColor, DefaultMidRasterColor, DefaultMaxRasterColor,
            DefaultMinMidColorInterpolationType, DefaultMidMaxColorInterpolationType, rasterLength, maxRasterValue,
            minParameterValue, maxParameterValue, ohlcvComponent, highPassFilterCutoff, minimalPeriod, maximalPeriod)
        {
        }

        /// <summary>
        /// Constructs a new instance of the class.
        /// </summary>
        /// <param name="coronaSpectrum">The parent <c>CoronaSpectrum</c> instance to re-use the corona engine.</param>
        /// <param name="minRasterColor">The color associated with the minimal heatmap raster value.</param>
        /// <param name="midRasterColor">The color associated with a middle heatmap raster value.</param>
        /// <param name="maxRasterColor">The color associated with the maximal heatmap raster value.</param>
        /// <param name="minMidColorInterpolationType">An interploation type between the minimal heatmap color and the middle heatmap color.</param>
        /// <param name="midMaxColorInterpolationType">An interploation type between the middle heatmap color and the maximal heatmap color.</param>
        /// <param name="rasterLength">The number of elements in the heatmap raster. The default value is 50.</param>
        /// <param name="maxRasterValue">The maximal raster value of the heatmap. The default value is 20.</param>
        /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is 1.</param>
        /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is 11.</param>
        /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
        public CoronaSignalToNoiseRatio(CoronaSpectrum coronaSpectrum, Color minRasterColor, Color midRasterColor, Color maxRasterColor,
            ColorInterpolationType minMidColorInterpolationType, ColorInterpolationType midMaxColorInterpolationType,
            int rasterLength = 50, double maxRasterValue = 20, double minParameterValue = 1, double maxParameterValue = 11,
            OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
            : this(coronaSpectrum.Corona, minRasterColor, midRasterColor, maxRasterColor, minMidColorInterpolationType, midMaxColorInterpolationType,
            rasterLength, maxRasterValue,
            minParameterValue, maxParameterValue, ohlcvComponent, 30, 6, 30)
        {
        }

        /// <summary>
        /// Constructs a new instance of the class.
        /// </summary>
        /// <param name="coronaSpectrum">The parent <c>CoronaSpectrum</c> instance to re-use the corona engine.</param>
        /// <param name="rasterLength">The number of elements in the heatmap raster. The default value is 50.</param>
        /// <param name="maxRasterValue">The maximal raster value of the heatmap. The default value is 20.</param>
        /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is 1.</param>
        /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is 11.</param>
        /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
        public CoronaSignalToNoiseRatio(CoronaSpectrum coronaSpectrum,
            int rasterLength = 50, double maxRasterValue = 20, double minParameterValue = 1, double maxParameterValue = 11,
            OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
            : this(coronaSpectrum.Corona, DefaultMinRasterColor, DefaultMidRasterColor, DefaultMaxRasterColor,
            DefaultMinMidColorInterpolationType, DefaultMidMaxColorInterpolationType, rasterLength, maxRasterValue,
            minParameterValue, maxParameterValue, ohlcvComponent, 30, 6, 30)
        {
        }

        private CoronaSignalToNoiseRatio(Corona corona, Color minRasterColor, Color midRasterColor, Color maxRasterColor,
            ColorInterpolationType minMidColorInterpolationType, ColorInterpolationType midMaxColorInterpolationType,
            int rasterLength, double maxRasterValue, double minParameterValue, double maxParameterValue,
            OhlcvComponent ohlcvComponent,
            int highPassFilterCutoff, int minimalPeriod, int maximalPeriod)
            : base(csnrHeatmapName, csnrHeatmapDescription, ohlcvComponent)
        {
            moniker = csnrHeatmapMoniker;
            if (null == corona)
                this.corona = new Corona(highPassFilterCutoff, minimalPeriod, maximalPeriod);
            else
            {
                this.corona = corona;
                isSlave = true;
            }
            this.rasterLength = rasterLength;
            this.maxRasterValue = maxRasterValue;
            this.minParameterValue = minParameterValue;
            this.maxParameterValue = maxParameterValue;
            midRasterValue = maxRasterValue / 2;
            rasterStep = maxRasterValue / rasterLength;
            raster = new double[rasterLength];
            for (int i = 0; i < rasterLength; ++i)
                raster[i] = 0;
            this.minRasterColor = minRasterColor;
            this.midRasterColor = midRasterColor;
            this.maxRasterColor = maxRasterColor;
            this.minMidColorInterpolationType = minMidColorInterpolationType;
            this.midMaxColorInterpolationType = midMaxColorInterpolationType;
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
                isStarted = false;
                averageSamplePrevious = 0;
                signalPrevious = 0;
                noisePrevious = 0;
                signalToNoiseRatio = double.NaN;
                for (int i = 0; i < rasterLength; ++i)
                    raster[i] = 0;
                if (!isSlave)
                    corona.Reset();
            }
        }
        #endregion

        #region Update
        private Heatmap Update(double sample, double sampleLow, double sampleHigh, DateTime dateTime)
        {
            if (double.IsNaN(sample))
                return null;
            primed = isSlave ? corona.IsPrimed : corona.Update(sample);
            if (!isStarted)
            {
                averageSamplePrevious = sample;
                highLowBuffer[highLowBufferSizeMinOne] = sampleHigh - sampleLow;
                isStarted = true;
                return null;
            }
            double maxAmplitudeSquared = corona.MaximalAmplitudeSquared;

            double averageSample = 0.1 * sample + 0.9 * averageSamplePrevious;
            averageSamplePrevious = averageSample;
            if (Math.Abs(averageSample) > double.Epsilon || maxAmplitudeSquared > 0)
                signalPrevious = 0.2 * Math.Sqrt(maxAmplitudeSquared) + 0.9 * signalPrevious;
            for (int i = 0; i < highLowBufferSizeMinOne; )
                highLowBuffer[i] = highLowBuffer[++i];
            highLowBuffer[highLowBufferSizeMinOne] = sampleHigh - sampleLow;
            double ratio = 0;
            if (Math.Abs(averageSample) > double.Epsilon)
            {
                for (int i = 0; i < highLowBufferSize; ++i)
                    highLowBufferSorted[i] = highLowBuffer[i];
                Array.Sort(highLowBufferSorted);
                noisePrevious = 0.1 * highLowBufferSorted[highLowMedianIndex] + 0.9 * noisePrevious;
                if (Math.Abs(noisePrevious) > double.Epsilon)
                {
                    ratio = 20 * Math.Log10(signalPrevious / noisePrevious) + 3.5;
                    if (ratio < 0)// <1
                        ratio = 0;
                    else if (ratio > 10)
                        ratio = 10;
                    // ∈ [0, 1].
                    ratio /= 10;
                }
            }
            signalToNoiseRatio = (maxParameterValue - minParameterValue) * ratio + minParameterValue;
            // width ∈ [0, 0.2].
            double width = ratio > 0.5 ? 0 : (0.2 - 0.4 * ratio);
            var ratioScaledToRasterLength = (int)Math.Round(ratio * rasterLength);
            double ratioScaledToMaxRasterValue = ratio * maxRasterValue;
            for (int i = 0; i < rasterLength; ++i)
            //Parallel.For(0, rasterLength, i =>
            {
                double value = raster[i];
                if (i == ratioScaledToRasterLength)
                    value *= 0.5;
                else
                {
                    double argument = ratioScaledToMaxRasterValue - rasterStep * i;
                    argument /= width;
                    if (i < ratioScaledToRasterLength)
                        value = 0.5 * (Math.Pow(argument, 0.8) + value);
                    else
                    {
                        argument = -argument;
                        if (argument > 1)
                            value = 0.5 * (Math.Pow(argument, 0.8) + value);
                        else
                            value = maxRasterValue;
                    }
                }
                if (value < 0)
                    value = 0;
                else if (value > maxRasterValue)
                    value = maxRasterValue;
                if (ratio > 0.5)
                    value = maxRasterValue;
                raster[i] = value;
            }//);

            double[] intensity;
            Brush brush;
            if (primed)
            {
                intensity = new double[rasterLength];//+1
                var gradientStopCollection = new GradientStopCollection(rasterLength);//+1
                // Min raster value is zero.
                double offset = 0, offsetStep = 1d / (rasterLength - 1);
                for (int i = 0; i < rasterLength; ++i, offset += offsetStep)
                {
                    double value = raster[i];
                    intensity[i] = value;
                    GradientStop gradientStop;
                    if (value <= midRasterValue)
                        gradientStop = new GradientStop(ColorInterploation.InterpolateBetween(minRasterColor, midRasterColor,
                            value / midRasterValue, minMidColorInterpolationType), offset);
                    else
                        gradientStop = new GradientStop(ColorInterploation.InterpolateBetween(midRasterColor, maxRasterColor,
                            value / midRasterValue - 1, midMaxColorInterpolationType), offset);
                    gradientStopCollection.Add(gradientStop);
                }
                //gradientStopCollection.Add(new GradientStop(Colors.White, ratio));
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
                return Update(ohlcv.Component(ohlcvComponent), ohlcv.Low, ohlcv.High, ohlcv.Time);
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
                double value = scalar.Value;
                return Update(value, value, value, scalar.Time);
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
            bool p;
            lock (updateLock)
            {
                p = primed;
            }
            var sb = new StringBuilder();
            sb.Append("[M:");
            sb.Append(moniker);
            sb.Append(" P:");
            sb.Append(p);
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}