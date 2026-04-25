using System;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// A band indicator interface.
    /// </summary>
    public interface IBandIndicator : IIndicator
    {
        /// <summary>
        /// Updates the band indicator.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The updated value of the band indicator.</returns>
        Band Update(double sample, DateTime dateTime);

        /// <summary>
        /// Updates the band indicator.
        /// </summary>
        /// <param name="sample">A new <c>scalar</c> sample.</param>
        /// <returns>The updated value of the band indicator.</returns>
        Band Update(Scalar sample);

        /// <summary>
        /// Updates the band indicator.
        /// </summary>
        /// <param name="sample">A new <c>ohlcv</c> sample.</param>
        /// <returns>The updated value of the band indicator.</returns>
        Band Update(Ohlcv sample);
    }
}
