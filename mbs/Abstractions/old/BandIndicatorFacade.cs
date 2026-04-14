using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// A façade class to expose two properties of a source indicator as a separate band indicator with a fictious update.
    /// <para />
    /// This may be used in a situation when a source indicator calculates several properties which we want to plot simultaneously on a chart.
    /// <para />
    /// The band indicator façade does not perform an actual update. It assumes that the source indicator is already updated, and just fetches values of two required properties.
    /// <para />
    /// Resetting the band indicator façade has no effect. The source indicator should be reset instead.
    /// </summary>
    [DataContract]
    public sealed class BandIndicatorFacade : IBandIndicator
    {
        #region Members and accessors
        #region Name
        [DataMember]
        private readonly string name;
        /// <summary>
        /// Identifies the band indicator façade.
        /// </summary>
        public string Name { get { return name; } }
        #endregion

        #region Moniker
        [DataMember]
        private readonly string moniker;
        /// <summary>
        /// Identifies an instance of the band indicator façade.
        /// </summary>
        public string Moniker { get { return moniker; } }
        #endregion

        #region Description
        [DataMember]
        private readonly string description;
        /// <summary>
        /// Describes the band indicator façade.
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

        [DataMember]
        private readonly Func<double> getFirstValue; 
        [DataMember]
        private readonly Func<double> getSecondValue; 
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="BandIndicatorFacade"/> class.
        /// </summary>
        /// <param name="name">Identifies the band indicator façade.</param>
        /// <param name="moniker">Identifies an instance of the band indicator façade.</param>
        /// <param name="description">Describes the band indicator façade.</param>
        /// <param name="isPrimed">An function to check if the source indicator is primed.</param>
        /// <param name="getFirstValue">An function to fetch the first value of the band from the source indicator.</param>
        /// <param name="getSecondValue">An function to fetch the second value of the band from the source indicator.</param>
        internal BandIndicatorFacade(string name, string moniker, string description, Func<bool> isPrimed, Func<double> getFirstValue, Func<double> getSecondValue)
        {
            this.name = name;
            this.moniker = moniker;
            this.description = description;
            this.isPrimed = isPrimed;
            this.getFirstValue = getFirstValue;
            this.getSecondValue = getSecondValue;
        }
        #endregion

        #region Reset
        /// <summary>
        /// Resetting the band indicator façade has no effect. The source indicator should be reset instead.
        /// </summary>
        public void Reset()
        {
        }
        #endregion

        #region Update
        /// <summary>
        /// Fetches both the first and and the second band values from the source indicator, which is assumed to be already updated.
        /// </summary>
        /// <param name="sample">A new sample. Not used.</param>
        /// <param name="dateTime">The date and time which is used as time stamp of the new <c>band</c> instance.</param>
        /// <returns>The new value of the band indicator façade.</returns>
        public Band Update(double sample, DateTime dateTime)
        {
            return new Band(dateTime, getFirstValue(), getSecondValue());
        }

        /// <summary>
        /// Fetches both the first and and the second band values from the source indicator, which is assumed to be already updated.
        /// </summary>
        /// <param name="scalar">A new <c>scalar</c>. The time stamp is used as time stamp of the new <c>band</c> instance.</param>
        /// <returns>The new value of the band indicator façade.</returns>
        public Band Update(Scalar scalar)
        {
            return new Band(scalar.Time, getFirstValue(), getSecondValue());
        }

        /// <summary>
        /// Fetches both the first and and the second band values from the source indicator, which is assumed to be already updated.
        /// </summary>
        /// <param name="ohlcv">A new <c>ohlcv</c>. The time stamp is used as time stamp of the new <c>band</c> instance.</param>
        /// <returns>The new value of the band indicator façade.</returns>
        public Band Update(Ohlcv ohlcv)
        {
            return new Band(ohlcv.Time, getFirstValue(), getSecondValue());
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
            sb.Append(" 1:");
            sb.Append(getFirstValue().ToString(CultureInfo.InvariantCulture));
            sb.Append(" 2:");
            sb.Append(getSecondValue().ToString(CultureInfo.InvariantCulture));
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
