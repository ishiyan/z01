namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// A line indicator interface.
    /// </summary>
    public interface ILineIndicator : IIndicator
    {
        /// <summary>
        /// Updates the line indicator.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The updated value of the line indicator.</returns>
        double Update(double sample);

        /// <summary>
        /// Updates the indicator.
        /// </summary>
        /// <param name="sample">A new <c>scalar</c> sample.</param>
        /// <returns>The updated value of the line indicator.</returns>
        Scalar Update(Scalar sample);

        /// <summary>
        /// Updates the indicator.
        /// </summary>
        /// <param name="sample">A new <c>ohlcv</c> sample.</param>
        /// <returns>The updated value of the line indicator.</returns>
        Scalar Update(Ohlcv sample);
    }
}
