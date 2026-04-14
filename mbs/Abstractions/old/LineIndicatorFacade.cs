using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// A façade class to expose a property of a source indicator as a separate line indicator with a fictious update.
    /// <para />
    /// This may be used in a situation when a source indicator calculates several properties which we want to plot simultaneously on a chart.
    /// <para />
    /// The line indicator façade does not perform an actual update. It assumes that the source indicator is already updated, and just gets a value of the required property.
    /// <para />
    /// Resetting the line indicator façade has no effect. The source indicator should be reset instead.
    /// </summary>
    [DataContract]
    public sealed class LineIndicatorFacade : ILineIndicator
    {
        #region Members and accessors
        #region Name
        [DataMember]
        private readonly string name;
        /// <summary>
        /// Identifies the line indicator façade.
        /// </summary>
        public string Name { get { return name; } }
        #endregion

        #region Moniker
        [DataMember]
        private readonly string moniker;
        /// <summary>
        /// Identifies an instance of the line indicator façade.
        /// </summary>
        public string Moniker { get { return moniker; } }
        #endregion

        #region Description
        [DataMember]
        private readonly string description;
        /// <summary>
        /// Describes the line indicator façade.
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
        private readonly Func<double> getValue;
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="LineIndicatorFacade"/> class.
        /// </summary>
        /// <param name="name">Identifies the line indicator façade.</param>
        /// <param name="moniker">Identifies an instance of the line indicator façade.</param>
        /// <param name="description">Describes the line indicator façade.</param>
        /// <param name="isPrimed">A function to check if the source indicator is primed.</param>
        /// <param name="getValue">A function to to fetch the current value from the source indicator.</param>
        internal LineIndicatorFacade(string name, string moniker, string description, Func<bool> isPrimed, Func<double> getValue)
        {
            this.name = name;
            this.moniker = moniker;
            this.description = description;
            this.isPrimed = isPrimed;
            this.getValue = getValue;
        }
        #endregion

        #region Reset
        /// <summary>
        /// Resetting the line indicator façade has no effect. The source indicator should be reset instead.
        /// </summary>
        public void Reset()
        {
        }
        #endregion

        #region Update
        /// <summary>
        /// Fetches the current value from the source indicator, which is assumed to be already updated.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the line indicator façade.</returns>
        public double Update(double sample)
        {
            return getValue();
        }

        /// <summary>
        /// Fetches the current value from the source indicator, which is assumed to be already updated.
        /// </summary>
        /// <param name="scalar">A new <c>scalar</c>. The time stamp is used as time stamp of the new <c>scalar</c> instance.</param>
        /// <returns>The new value of the line indicator façade.</returns>
        public Scalar Update(Scalar scalar)
        {
            return new Scalar(scalar.Time, getValue());
        }

        /// <summary>
        /// Fetches the current value from the source indicator, which is assumed to be already updated.
        /// </summary>
        /// <param name="ohlcv">A new <c>ohlcv</c>. The time stamp is used as time stamp of the new <c>scalar</c> instance.</param>
        /// <returns>The new value of the line indicator façade.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            return new Scalar(ohlcv.Time, getValue());
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
            sb.Append(" V:");
            sb.Append(getValue().ToString(CultureInfo.InvariantCulture));
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
