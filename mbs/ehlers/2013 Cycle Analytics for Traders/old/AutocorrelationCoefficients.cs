using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Plots a heatmap of estimated sample Pearson autocorrelation coefficients.
    /// </summary>
    [DataContract]
    public sealed class AutocorrelationCoefficients : Indicator, IHeatmapIndicator
    {
        #region Members and accessors
        #region MinParameterValue
        /// <summary>
        /// The minimum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns.
        /// </summary>
        public double MinParameterValue => estimator.MinLag;
        #endregion

        #region MaxParameterValue
        /// <summary>
        /// The maximum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns.
        /// </summary>
        public double MaxParameterValue => estimator.MaxLag;
        #endregion

        #region Length
        /// <summary>
        /// The length (the number of time periods) of the sample window.
        /// This determines also the maximum correlation lag to calculate. The minimal value is 2
        /// </summary>
        [DataMember]
        public int Length => estimator.Length;
        #endregion

        #region Default raster colors
        /// <summary>
        /// A default color associated with the minimal heatmap raster value.
        /// </summary>
        public static readonly Color DefaultMinRasterColor = Colors.Red;

        /// <summary>
        /// A default color associated with a middle heatmap raster value.
        /// </summary>
        public static readonly Color DefaultMidRasterColor = Colors.Yellow;

        /// <summary>
        /// A default color associated with the maximal heatmap raster value.
        /// </summary>
        public static readonly Color DefaultMaxRasterColor = Colors.Green;

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
        private AutocorrelationEstimator estimator;
        [DataMember]
        private readonly int lastIndex;
        [DataMember]
        private readonly double parameterRange;

        private const string acc = "acc";
        private const string accFull = "Autocorrelation Coefficients";
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
        /// <param name="length">The length of the input series window. This determines also the maximum lag to calculate. The minimal value is 2.</param>
        /// <param name="minLag">The minimum lag to compute correlation coefficients, must be less than <c>length</c>.</param>
        /// <param name="maxLag">The maximum lag to compute correlation coefficients, must be less or equal to <c>length</c>.</param>
        /// <param name="averagingLength">The number of last samples of the input series window which are used for averaging. The default value of 0 means the averaging length is equal to each lag. That way, the averaging contains the maximum number of data samples per lag.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public AutocorrelationCoefficients(Color minRasterColor, Color midRasterColor, Color maxRasterColor,
            ColorInterpolationType minMidColorInterpolationType, ColorInterpolationType midMaxColorInterpolationType,
            int length = 48, int minLag = 10, int maxLag = 47, int averagingLength = 0, OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
            : base(acc, accFull, ohlcvComponent)
        {
            if (2 > length)
                throw new ArgumentOutOfRangeException(argumentLength);
            estimator = new AutocorrelationEstimator(length, minLag, maxLag, averagingLength);
            lastIndex = estimator.Length - 1;
            moniker = string.Concat(acc, length.ToString(CultureInfo.InvariantCulture));
            parameterRange = estimator.MaxLag - estimator.MinLag;

            this.minRasterColor = minRasterColor;
            this.midRasterColor = midRasterColor;
            this.maxRasterColor = maxRasterColor;
            this.minMidColorInterpolationType = minMidColorInterpolationType;
            this.midMaxColorInterpolationType = midMaxColorInterpolationType;
        }

        /// <summary>
        /// Constructs a new instance of the class.
        /// </summary>
        /// <param name="length">The length of the input series window. This determines also the maximum lag to calculate. The minimal value is 2.</param>
        /// <param name="minLag">The minimum lag to compute correlation coefficients, must be less than <c>length</c>.</param>
        /// <param name="maxLag">The maximum lag to compute correlation coefficients, must be less or equal to <c>length</c>.</param>
        /// <param name="averagingLength">The number of last samples of the input series window which are used for averaging. The default value of 0 means the averaging length is equal to each lag. That way, the averaging contains the maximum number of data samples per lag.</param>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public AutocorrelationCoefficients(int length = 48, int minLag = 10, int maxLag = 47, int averagingLength = 0,
            OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
            : this(DefaultMinRasterColor, DefaultMidRasterColor, DefaultMaxRasterColor,
            DefaultMinMidColorInterpolationType, DefaultMidMaxColorInterpolationType,
            length, minLag, maxLag, averagingLength, ohlcvComponent)
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
                int correlationCoefficientsLength = estimator.CorrelationCoefficientsLength;
                intensity = new double[correlationCoefficientsLength];
                var gradientStopCollection = new GradientStopCollection(correlationCoefficientsLength);
                window = estimator.CorrelationCoefficients;
                for (int i = 0; i < correlationCoefficientsLength; ++i)
                {
                    double value = window[i];
                    intensity[i] = value;
                    // Re-range from [-1, 1] to [0, 1].
                    value = (value + 1) / 2;
                    double offset = (estimator.CorrelationLags[i] - estimator.MinLag) / parameterRange;
                    var gradientStop = value <= 0.5 ?
                        new GradientStop(ColorInterploation.InterpolateBetween(minRasterColor, midRasterColor, value, minMidColorInterpolationType), offset) :
                        new GradientStop(ColorInterploation.InterpolateBetween(midRasterColor, maxRasterColor, value, midMaxColorInterpolationType), offset);
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
