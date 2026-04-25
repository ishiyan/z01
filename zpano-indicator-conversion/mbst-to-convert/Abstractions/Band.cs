using System;
using System.Runtime.Serialization;
using System.Text;
using System.Globalization;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Holds two values and a time stamp.
    /// </summary>
    [DataContract]
    public class Band
    {
        #region Members and accessors
        #region FirstValue
        [DataMember]
        private readonly double firstValue = double.NaN;
        /// <summary>
        /// The first value.
        /// </summary>
        public double FirstValue
        {
            get { return firstValue; }
        }
        #endregion

        #region SecondValue
        [DataMember]
        private readonly double secondValue = double.NaN;
        /// <summary>
        /// The second value.
        /// </summary>
        public double SecondValue
        {
            get { return secondValue; }
        }
        #endregion

        #region Time
        [DataMember]
        private DateTime time;
        /// <summary>
        /// The time stamp.
        /// </summary>
        public DateTime Time
        {
            get { return time; }
        }
        #endregion

        #region IsEmpty
        /// <summary>
        /// Indicates if this <c>band</c> is not initialized.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return double.IsNaN(firstValue) || double.IsNaN(secondValue);
            }
        }
        #endregion
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new object.
        /// </summary>
        /// <param name="time">The time stamp of the <c>band</c>.</param>
        /// <param name="firstValue">The first value of the <c>band</c>.</param>
        /// <param name="secondValue">The second value of the <c>band</c>.</param>
        public Band(DateTime time, double firstValue, double secondValue)
        {
            if (!double.IsNaN(firstValue) && !double.IsNaN(secondValue))
            {
                this.firstValue = firstValue;
                this.secondValue = secondValue;
            }
            this.time = time;
        }

        /// <summary>
        /// Constructs a new object.
        /// </summary>
        /// <param name="time">The time stamp.</param>
        public Band(DateTime time)
        {
            this.time = time;
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
            sb.Append("[T:");
            sb.Append(time.ToCompactString());
            sb.Append(" 1:");
            sb.Append(firstValue.ToString(CultureInfo.InvariantCulture));
            sb.Append(" 2:");
            sb.Append(secondValue.ToString(CultureInfo.InvariantCulture));
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
