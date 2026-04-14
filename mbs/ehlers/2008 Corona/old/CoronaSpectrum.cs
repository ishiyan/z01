using System;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Media;

using Mbst.Charts;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// The Corona Spectrum measures cyclic activity over a cycle period range (default from 6 bars to 30 bars)
    /// in a bank of contiguous filters.
    /// <para />
    /// Longer cycle periods are not considered because they can be considered trend segments. Shorter cycle
    /// periods are not considered because of the aliasing noise encountered with the data sampling process.
    /// <para />
    /// The amplitude of each filter output is compared to the strongest signal and the result is displayed
    /// over an amplitude range of 20 decibels in the form of a heatmap.
    /// <para />
    /// The filter having the strongest output is selected as the current dominant cycle period.
    /// <para />
    /// See “Measuring Cycle Periods” in the March 2008 issue of Stocks &amp; Commodities Magazine.
    /// </summary>
    [DataContract]
    public sealed class CoronaSpectrum : Indicator, IHeatmapIndicator
    {
        #region Members and accessors
        #region MinParameterValue
        private readonly double minParameterValue;
        /// <summary>
        /// The minimum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns.
        /// </summary>
        public double MinParameterValue { get { return minParameterValue; } }
        #endregion

        #region MaxParameterValue
        private readonly double maxParameterValue;
        /// <summary>
        /// The maximum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns.
        /// </summary>
        public double MaxParameterValue { get { return maxParameterValue; } }
        #endregion

        /// <summary>
        /// The middle raster value of the heatmap.
        /// </summary>
        [DataMember]
        private readonly double midRasterValue;

        #region DominantCycle
        /// <summary>
        /// The current value of the Corona dominant cycle or <c>NaN</c> if not primed.
        /// </summary>
        public double DominantCycle { get { lock (updateLock) { return primed ? corona.DominantCycle : double.NaN; } } }
        #endregion

        #region DominantCycleMedian
        /// <summary>
        /// The current value of the Corona dominant cycle median or <c>NaN</c> if not primed.
        /// </summary>
        public double DominantCycleMedian { get { lock (updateLock) { return primed ? corona.DominantCycleMedian : double.NaN; } } }
        #endregion

        #region DominantCycleFacade
        /// <summary>
        /// A line indicator façade to expose a value of the DominantCycle as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the DominantCycle from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DominantCycleFacade
        {
            get
            {
                return new LineIndicatorFacade(cdcName, cdcMoniker, cdcDescription, () => IsPrimed, () => DominantCycle);
            }
        }
        #endregion

        #region DominantCycleMedianFacade
        /// <summary>
        /// A line indicator façade to expose a value of the DominantCycleMedian as a separate line indicator with a fictious update.
        /// <para />
        /// The line indicator façade do not perform actual updates. It just gets a current value of the DominantCycleMedian from this instance, assuming it is already updated.
        /// <para />
        /// Resetting the line indicator façade has no effect. This instance should be reset instead.
        /// </summary>
        public LineIndicatorFacade DominantCycleMedianFacade
        {
            get
            {
                return new LineIndicatorFacade(cdcmName, cdcmMoniker, cdcmDescription, () => IsPrimed, () => DominantCycleMedian);
            }
        }
        #endregion

        #region Default raster colors
        /// <summary>
        /// A default color associated with the minimal heatmap raster value.
        /// </summary>
        public static readonly Color DefaultMinRasterColor = Colors.Yellow;

        /// <summary>
        /// A default color associated with a middle heatmap raster value.
        /// </summary>
        public static readonly Color DefaultMidRasterColor = Colors.Red;

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
        /// A default dominant cycle line style.
        /// </summary>
        public const PredefinedLineStyle DefaultDominantCycleLineStyle = PredefinedLineStyle.WhiteLine;

        /// <summary>
        /// A default dominant cycle median line style.
        /// </summary>
        public const PredefinedLineStyle DefaultDominantCycleMedianLineStyle = PredefinedLineStyle.YellowLine;
        #endregion

        [DataMember]
        private readonly Corona corona;
        /// <summary>
        /// The associated corona engine.
        /// </summary>
        internal Corona Corona { get { return corona; } }

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

        private const string csName = "CoronaSpectrum";
        private const string csDescription = "Crona spectrum heatmap";
        private const string csMoniker = "CoronaS";

        private const string cdcName = "CoronaDominantCycle";
        private const string cdcDescription = "Crona dominant cycle";
        private const string cdcMoniker = "CoronaDC";

        private const string cdcmName = "CoronaDominantCycleMedian";
        private const string cdcmDescription = "Crona dominant cycle median";
        private const string cdcmMoniker = "CoronaDCM";

        private readonly double parameterRange;
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
        /// <param name="minRasterValue">The minimal raster value of the heatmap. The default value is 6.</param>
        /// <param name="maxRasterValue">The maximal raster value of the heatmap. The default value is 20.</param>
        /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heatmap, representing the minimal period. This value is the same for all heatmap columns. The default value is 6.</param>
        /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heatmap, representing the maximal period. This value is the same for all heatmap columns. The default value is 30.</param>
        /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
        /// <param name="highPassFilterCutoff">The high-pass filter cutoff (detrending period). Suggested values are 20, 30, 100.</param>
        public CoronaSpectrum(Color minRasterColor, Color midRasterColor, Color maxRasterColor,
            ColorInterpolationType minMidColorInterpolationType, ColorInterpolationType midMaxColorInterpolationType,
            double minRasterValue = 6, double maxRasterValue = 20, double minParameterValue = 6, double maxParameterValue = 30,
            OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice, int highPassFilterCutoff = 30)
            : base(csName, csDescription, ohlcvComponent)
        {
            moniker = csMoniker;
            minParameterValue = Math.Ceiling(minParameterValue);
            maxParameterValue = Math.Floor(maxParameterValue);
            parameterRange = (maxParameterValue - minParameterValue);
            corona = new Corona(highPassFilterCutoff, (int)minParameterValue, (int)maxParameterValue, minRasterValue, maxRasterValue);

            this.minParameterValue = minParameterValue;
            this.maxParameterValue = maxParameterValue;

            midRasterValue = 10;// maxRasterValue / 2;

            this.minRasterColor = minRasterColor;
            this.midRasterColor = midRasterColor;
            this.maxRasterColor = maxRasterColor;
            this.minMidColorInterpolationType = minMidColorInterpolationType;
            this.midMaxColorInterpolationType = midMaxColorInterpolationType;
        }

        /// <summary>
        /// Constructs a new instance of the class.
        /// </summary>
        /// <param name="minRasterValue">The minimal raster value of the heatmap. The default value is 6.</param>
        /// <param name="maxRasterValue">The maximal raster value of the heatmap. The default value is 20.</param>
        /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heatmap, representing the minimal period. This value is the same for all heatmap columns. The default value is 6.</param>
        /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heatmap, representing the maximal period. This value is the same for all heatmap columns. The default value is 30.</param>
        /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
        /// <param name="highPassFilterCutoff">The high-pass filter cutoff (detrending period). Suggested values are 20, 30, 100.</param>
        public CoronaSpectrum(double minRasterValue = 6, double maxRasterValue = 20,
            double minParameterValue = 6, double maxParameterValue = 30,
            OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice,
            int highPassFilterCutoff = 30)
            : this(DefaultMinRasterColor, DefaultMidRasterColor, DefaultMaxRasterColor,
            DefaultMinMidColorInterpolationType, DefaultMidMaxColorInterpolationType, minRasterValue, maxRasterValue,
            minParameterValue, maxParameterValue, ohlcvComponent, highPassFilterCutoff)
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
                corona.Reset();
            }
        }
        #endregion

        #region Update
        private Heatmap Update(double sample, DateTime dateTime)
        {
            if (double.IsNaN(sample))
                return null;
            primed = corona.Update(sample);
            double[] intensity;
            Brush brush;
            if (primed)
            {
                intensity = new double[corona.FilterBankLength];//+1
                var gradientStopCollection = new GradientStopCollection(corona.FilterBankLength);//+1
                int min = corona.MinimalPeriodTimesTwo;
                int max = corona.MaximalPeriodTimesTwo;
                for (int i = min; i <= max; ++i)
                {
                    // Here i/2d is a period in steps of 0.5. The minParameterValue is the minimal period.
                    double offset = (i / 2d - minParameterValue) / parameterRange;
                    double value = corona.FilterBank[i - min].Decibels;
                    intensity[i - min] = value;
                    GradientStop gradientStop;
                    if (value <= midRasterValue)
                        gradientStop = new GradientStop(ColorInterploation.InterpolateBetween(minRasterColor, midRasterColor,
                            value / midRasterValue, minMidColorInterpolationType), offset);
                    else
                        gradientStop = new GradientStop(ColorInterploation.InterpolateBetween(midRasterColor, maxRasterColor,
                            value / midRasterValue - 1, midMaxColorInterpolationType), offset);
                    gradientStopCollection.Add(gradientStop);
                }
                //gradientStopCollection.Add(new GradientStop(Colors.LightYellow, (corona.DominantCycleMedian - minParameterValue) / parameterRange));
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