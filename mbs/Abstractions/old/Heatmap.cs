using System;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Holds a time stamp and a brush to paint a heatmap column.
    /// </summary>
    [DataContract]
    public sealed class Heatmap
    {
        #region Members and accessors
        #region Brush
        [DataMember]
        private readonly Brush brush;
        /// <summary>
        /// A brush to paint a heatmap column.
        /// </summary>
        public Brush Brush
        {
            get { return brush; }
        }
        #endregion

        #region Intensity
        [DataMember]
        private readonly double[] intensity;
        /// <summary>
        /// An intensity of a heatmap column.
        /// </summary>
        public double[] Intensity
        {
            get { return intensity; }
        }
        #endregion

        #region Time
        [DataMember]
        private readonly DateTime time;
        /// <summary>
        /// A time stamp.
        /// </summary>
        public DateTime Time
        {
            get { return time; }
        }
        #endregion

        #region IsEmpty
        /// <summary>
        /// Indicates if this <c>heatmap</c> is not initialized.
        /// </summary>
        public bool IsEmpty
        {
            get { return null == brush; }
        }
        #endregion
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new object.
        /// </summary>
        /// <param name="time">The time stamp of the <c>heatmap</c>.</param>
        /// <param name="brush">The brush to paint a <c>heatmap</c> column.</param>
        /// <param name="intensity">The intensities of a <c>heatmap</c> column.</param>
        public Heatmap(DateTime time, Brush brush, double[] intensity = null)
        {
            this.brush = brush;
            this.time = time;
            this.intensity = intensity;
        }
        #endregion
    }
}
