using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using Mbst.Charts;
using Mbst.Numerics;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// The Corona swing position is estimated by correlating the prices with a perfect sine wave
    /// having the dominant cycle period. This correlation produces a smooth waveform that lets us
    /// better estimate the swing position and impending turning points.
    /// <para />
    /// The vertical axis is on an arbitrary scale from -5 to 5.
    /// Since the purpose of this indicator is to anticipate the turning points,
    /// regions near the minimum and maximum are the strongest.
    /// <para />
    /// The indicator develops a “corona” near the center of the range – signifying that a turning point is not imminent.
    /// You will also notice a corona being displayed when the market is in a trend and there is very little cyclic swing.
    /// </summary>
    [DataContract]
    public sealed class CoronaSwingPosition : Indicator, IHeatmapIndicator
    {
        #region Members and accessors
        private const int maxLeadListCount = 50;
        private const int maxPositionListCount = 20;

        [DataMember]
        private readonly List<double> leadList = new List<double>(maxLeadListCount);
        [DataMember]
        private readonly List<double> positionList = new List<double>(maxPositionListCount);

        [DataMember]
        private readonly int rasterLength;
        [DataMember]
        private readonly double rasterStep;
        [DataMember]
        private bool isStarted;
        [DataMember]
        private double bandPassPrevious;
        [DataMember]
        private double bandPassPrevious2;
        [DataMember]
        private double samplePrevious;
        [DataMember]
        private double samplePrevious2;

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

        #region SwingPosition
        [DataMember]
        private double swingPosition = double.NaN;

        /// <summary>
        /// The current value of the Corona swing position in the range of [MinParameterValue, MaxParameterValue] or <c>NaN</c> if not primed.
        /// </summary>
        public double SwingPosition { get { lock (updateLock) { return primed ? swingPosition : double.NaN; } } }
        #endregion

        #region SwingPositionFacade
        /// <summary>
        /// A line indicator façade to expose a value of the SwingPosition as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the SwingPosition from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade SwingPositionFacade
        {
            get
            {
                return new LineIndicatorFacade(cspName, cspMoniker, cspDescription, () => IsPrimed, () => SwingPosition);
            }
        }
        #endregion

        #region Default raster colors
        /// <summary>
        /// A default color associated with the minimal heatmap raster value.
        /// </summary>
        public static readonly Color DefaultMinRasterColor = Color.FromArgb(255, 180, 255, 210);

        /// <summary>
        /// A default color associated with a middle heatmap raster value.
        /// </summary>
        public static readonly Color DefaultMidRasterColor = Color.FromArgb(255, 0, 175, 60);

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
        /// A default swing position line style.
        /// </summary>
        public const PredefinedLineStyle DefaultSwingPositionLineStyle = PredefinedLineStyle.AquamarineLine;
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

        private const string cspHeatmapName = "CoronaSwingPositionHeatmap";
        private const string cspHeatmapDescription = "Corona swing position heatmap";
        private const string cspHeatmapMoniker = "CoronaSP(h)";

        private const string cspName = "CoronaSwingPosition";
        private const string cspDescription = "Crona swing position";
        private const string cspMoniker = "CoronaSP";
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
        /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is -5.</param>
        /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is 5.</param>
        /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
        /// <param name="highPassFilterCutoff">The high-pass filter cutoff (detrending period). Suggested values are 20, 30, 100.</param>
        /// <param name="minimalPeriod">The minimal period. The default value is 6.</param>
        /// <param name="maximalPeriod">The maximal period. The default value is 30.</param>
        public CoronaSwingPosition(Color minRasterColor, Color midRasterColor, Color maxRasterColor,
            ColorInterpolationType minMidColorInterpolationType, ColorInterpolationType midMaxColorInterpolationType,
            int rasterLength = 50, double maxRasterValue = 20, double minParameterValue = -5, double maxParameterValue = 5,
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
        /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is -5.</param>
        /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is 5.</param>
        /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
        /// <param name="highPassFilterCutoff">The high-pass filter cutoff (detrending period). Suggested values are 20, 30, 100.</param>
        /// <param name="minimalPeriod">The minimal period. The default value is 6.</param>
        /// <param name="maximalPeriod">The maximal period. The default value is 30.</param>
        public CoronaSwingPosition(int rasterLength = 50, double maxRasterValue = 20,
            double minParameterValue = -5, double maxParameterValue = 5,
            OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice,
            int highPassFilterCutoff = 30, int minimalPeriod = 6, int maximalPeriod = 30)
            : this(null, DefaultMinRasterColor, DefaultMidRasterColor, DefaultMaxRasterColor,
            DefaultMinMidColorInterpolationType, DefaultMidMaxColorInterpolationType,
            rasterLength, maxRasterValue,
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
        /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is -5.</param>
        /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is 5.</param>
        /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
        public CoronaSwingPosition(CoronaSpectrum coronaSpectrum, Color minRasterColor, Color midRasterColor, Color maxRasterColor,
            ColorInterpolationType minMidColorInterpolationType, ColorInterpolationType midMaxColorInterpolationType,
            int rasterLength = 50, double maxRasterValue = 20, double minParameterValue = -5, double maxParameterValue = 5,
            OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
            : this(coronaSpectrum.Corona, minRasterColor, midRasterColor, maxRasterColor,
            minMidColorInterpolationType, midMaxColorInterpolationType,
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
        /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is -5.</param>
        /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns. The default value is 5.</param>
        /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
        public CoronaSwingPosition(CoronaSpectrum coronaSpectrum,
            int rasterLength = 50, double maxRasterValue = 20, double minParameterValue = -5, double maxParameterValue = 5,
            OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
            : this(coronaSpectrum.Corona, DefaultMinRasterColor, DefaultMidRasterColor, DefaultMaxRasterColor,
            DefaultMinMidColorInterpolationType, DefaultMidMaxColorInterpolationType,
            rasterLength, maxRasterValue,
            minParameterValue, maxParameterValue, ohlcvComponent, 30, 6, 30)
        {
        }

        private CoronaSwingPosition(Corona corona, Color minRasterColor, Color midRasterColor, Color maxRasterColor,
            ColorInterpolationType minMidColorInterpolationType, ColorInterpolationType midMaxColorInterpolationType,
            int rasterLength, double maxRasterValue, double minParameterValue, double maxParameterValue,
            OhlcvComponent ohlcvComponent,
            int highPassFilterCutoff, int minimalPeriod, int maximalPeriod)
            : base(cspHeatmapName, cspHeatmapDescription, ohlcvComponent)
        {
            moniker = cspHeatmapMoniker;
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
                samplePrevious = 0;
                samplePrevious2 = 0;
                bandPassPrevious = 0;
                bandPassPrevious2 = 0;
                swingPosition = double.NaN;
                for (int i = 0; i < rasterLength; ++i)
                    raster[i] = 0;
                leadList.Clear();
                positionList.Clear();
                if (!isSlave)
                    corona.Reset();
            }
        }
        #endregion

        #region UpdatePhaseList
        private void UpdatePhaseList(List<double> list, int maximalCount, double lead, out double lowest, out double highest)
        {
            // Keep no more than maximal allowed number of elements.
            if (list.Count >= maximalCount)
                list.RemoveAt(0);
            list.Add(lead);

            lowest = lead;
            highest = lead;
            foreach (var value in list)
            {
                if (lowest > value)
                    lowest = value;
                if (highest < value)
                    highest = value;
            }
        }
        #endregion

        #region Update
        private Heatmap Update(double sample, DateTime dateTime)
        {
            if (double.IsNaN(sample))
                return null;
            primed = isSlave ? corona.IsPrimed : corona.Update(sample);
            if (!isStarted)
            {
                samplePrevious = sample;
                isStarted = true;
                return null;
            }

            // Filter bandpass InPhase component.
            double omega = Constants.TwoPi / corona.DominantCycleMedian;
            double beta2 = Math.Cos(omega);
            double gamma2 = 1 / Math.Cos(omega * 2 * 0.1);
            double alpha2 = gamma2 - Math.Sqrt(gamma2 * gamma2 - 1);
            double bandPass = 0.5 * (1 - alpha2) * (sample - samplePrevious2) + beta2 * (1 + alpha2) * bandPassPrevious - alpha2 * bandPassPrevious2;
            // Quadrature component is derivative of InPhase component divided by omega.
            double quadrature2 = (bandPass - bandPassPrevious) / omega;
            bandPassPrevious2 = bandPassPrevious;
            bandPassPrevious = bandPass;
            samplePrevious2 = samplePrevious;
            samplePrevious = sample;

            // Swing position calculation.
            double lead60 = 0.5 * bandPassPrevious2 + 0.866 * quadrature2;
            double highest, lowest;
            UpdatePhaseList(leadList, maxLeadListCount, lead60, out lowest, out highest);
            // The value will be in range [0, 1].
            double position = highest - lowest;
            // The value of the swing position is not negative, so the absolute value is not necessary.
            if (position > double.Epsilon)
                position = (lead60 - lowest) / position;
            UpdatePhaseList(positionList, maxPositionListCount, position, out lowest, out highest);
            highest -= lowest;
            double width = highest > 0.85 ? 0.01 : (0.15 * highest);
            swingPosition = (maxParameterValue - minParameterValue) * position + minParameterValue;
            var positionScaledToRasterLength = (int)Math.Round(position * rasterLength);
            double positionScaledToMaxRasterValue = position * maxRasterValue;
            for (int i = 0; i < rasterLength; ++i)
            //Parallel.For(0, rasterLength, i =>
            {
                double value = raster[i];
                if (i == positionScaledToRasterLength)
                    value *= 0.5;
                else
                {
                    double argument = positionScaledToMaxRasterValue - rasterStep * i;
                    if (i > positionScaledToRasterLength)
                        argument = -argument;
                    value = 0.5 * (Math.Pow(argument / width, 0.95) + 0.5 * value);
                }
                if (value < 0)
                    value = 0;
                else if (value > maxRasterValue)
                    value = maxRasterValue;
                if (highest > 0.8)
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
                //gradientStopCollection.Add(new GradientStop(Colors.Aquamarine, position));
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