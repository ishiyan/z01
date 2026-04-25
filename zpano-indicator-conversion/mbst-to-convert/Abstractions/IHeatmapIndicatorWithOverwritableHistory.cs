using System.Collections.ObjectModel;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// A heat map indicator with overwritable history interface.
    /// </summary>
    public interface IHeatmapIndicatorWithOverwritableHistory : IHeatmapIndicator
    {
        /// <summary>
        /// Historical values of the indicator.
        /// </summary>
        ReadOnlyCollection<Heatmap> HistoricalValues { set; }
    }
}
