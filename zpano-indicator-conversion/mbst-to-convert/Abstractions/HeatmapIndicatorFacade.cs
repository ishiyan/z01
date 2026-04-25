using System;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Media;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// A façade class to expose two properties of a source indicator as a separate heatmap indicator with a fictious update.
    /// <para />
    /// This may be used in a situation when a source indicator calculates several properties which we want to plot simultaneously on a chart.
    /// <para />
    /// The heatmap indicator façade does not perform an actual update. It assumes that the source indicator is already updated, and just fetches the values of required properties.
    /// <para />
    /// Resetting the heatmap indicator façade has no effect. The source indicator should be reset instead.
    /// </summary>
    [DataContract]
    public sealed class HeatmapIndicatorFacade : IHeatmapIndicator
    {
        #region Members and accessors
        #region Name
        [DataMember]
        private readonly string name;
        /// <summary>
        /// Identifies the heatmap indicator façade.
        /// </summary>
        public string Name { get { return name; } }
        #endregion

        #region Moniker
        [DataMember]
        private readonly string moniker;
        /// <summary>
        /// Identifies an instance of the heatmap indicator façade.
        /// </summary>
        public string Moniker { get { return moniker; } }
        #endregion

        #region Description
        [DataMember]
        private readonly string description;
        /// <summary>
        /// Describes the heatmap indicator façade.
        /// </summary>
        public string Description { get { return description; } }
        #endregion

        #region IsPrimed
        [DataMember]
        private readonly Func<bool> isPrimed;
        /// <summary>
        /// Is the source indicator primed.
        /// </summary>
        public bool IsPrimed { get { return isPrimed(); } }
        #endregion

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

        [DataMember]
        private readonly Func<Brush> getBrush;
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="HeatmapIndicatorFacade"/> class.
        /// </summary>
        /// <param name="name">Identifies the heatmap indicator façade.</param>
        /// <param name="moniker">Identifies an instance of the heatmap indicator façade.</param>
        /// <param name="description">Describes the heatmap indicator façade.</param>
        /// <param name="isPrimed">A function to check if the source indicator is primed.</param>
        /// <param name="getBrush">A function to fetch the brush of the heatmap from the source indicator.</param>
        /// <param name="minParameterValue">The minimum parameter value of the heatmap. This value is the same for all heatmap columns.</param>
        /// <param name="maxParameterValue">The maximum parameter value of the heatmap. This value is the same for all heatmap columns.</param>
        internal HeatmapIndicatorFacade(string name, string moniker, string description, Func<bool> isPrimed, Func<Brush> getBrush, double minParameterValue, double maxParameterValue)
        {
            this.name = name;
            this.moniker = moniker;
            this.description = description;
            this.isPrimed = isPrimed;
            this.getBrush = getBrush;
            this.minParameterValue = minParameterValue;
            this.maxParameterValue = maxParameterValue;
        }
        #endregion

        #region Reset
        /// <summary>
        /// Resetting the heatmap indicator façade has no effect. The source indicator should be reset instead.
        /// </summary>
        public void Reset()
        {
        }
        #endregion

        #region Update
        /// <summary>
        /// Fetches both the first and and the second heatmap values from the source indicator, which is assumed to be already updated.
        /// </summary>
        /// <param name="scalar">A new <c>scalar</c>. The time stamp is used as time stamp of the new <c>heatmap</c> instance.</param>
        /// <returns>The new value of the heatmap indicator façade.</returns>
        public Heatmap Update(Scalar scalar)
        {
            return new Heatmap(scalar.Time, getBrush());
        }

        /// <summary>
        /// Fetches both the first and and the second heatmap values from the source indicator, which is assumed to be already updated.
        /// </summary>
        /// <param name="ohlcv">A new <c>ohlcv</c>. The time stamp is used as time stamp of the new <c>heatmap</c> instance.</param>
        /// <returns>The new value of the heatmap indicator façade.</returns>
        public Heatmap Update(Ohlcv ohlcv)
        {
            return new Heatmap(ohlcv.Time, getBrush());
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns the string that represents this object.
        /// </summary>
        /// <returns>Returns the string that represents this object.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[M:");
            sb.Append(moniker);
            sb.Append(" P:");
            sb.Append(isPrimed());
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
