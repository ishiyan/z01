using System;
using System.Runtime.Serialization;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// An abstract band indicator.
    /// </summary>
    [DataContract]
    public abstract class BandIndicator : Indicator, IBandIndicator
    {
        /// <summary>
        /// Constructs a new instance of the class.
        /// </summary>
        /// <param name="name">Identifies the indicator.</param>
        /// <param name="description">Describes the indicator.</param>
        /// <param name="ohlcvComponent">Ohlcv component.</param>
        protected BandIndicator(string name, string description, OhlcvComponent ohlcvComponent = OhlcvComponent.ClosingPrice)
            : base(name, description, ohlcvComponent) {}

        /// <summary>
        /// Updates the band indicator.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The updated value of the band indicator.</returns>
        public abstract Band Update(double sample, DateTime dateTime);

        /// <summary>
        /// Updates the band indicator.
        /// </summary>
        /// <param name="sample">A new <c>scalar</c> sample.</param>
        /// <returns>The updated value of the band indicator.</returns>
        public virtual Band Update(Scalar sample)
        {
            return Update(sample.Value, sample.Time);
        }

        /// <summary>
        /// Updates the band indicator.
        /// </summary>
        /// <param name="sample">A new <c>ohlcv</c> sample.</param>
        /// <returns>The updated value of the band indicator.</returns>
        public virtual Band Update(Ohlcv sample)
        {
            return Update(sample.Component(ohlcvComponent), sample.Time);
        }
    }
}
