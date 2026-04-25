namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// A heat map indicator interface.
    /// </summary>
    public interface IHeatmapIndicator : IIndicator
    {
        /// <summary>
        /// Updates the heat map indicator.
        /// </summary>
        /// <param name="sample">A new <c>scalar</c> sample.</param>
        /// <returns>The updated value of the heat map indicator.</returns>
        Heatmap Update(Scalar sample);

        /// <summary>
        /// Updates the heat map indicator.
        /// </summary>
        /// <param name="sample">A new <c>ohlcv</c> sample.</param>
        /// <returns>The updated value of the heat map indicator.</returns>
        Heatmap Update(Ohlcv sample);

        /// <summary>
        /// The minimum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns.
        /// </summary>
        double MinParameterValue { get; }

        /// <summary>
        /// The maximum ordinate (parameter) value of the heatmap. This value is the same for all heatmap columns.
        /// </summary>
        double MaxParameterValue { get; }
    }
}
