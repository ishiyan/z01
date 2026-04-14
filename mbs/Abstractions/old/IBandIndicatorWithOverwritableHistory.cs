using System.Collections.ObjectModel;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// A band indicator with overwritable history interface.
    /// </summary>
    public interface IBandIndicatorWithOverwritableHistory : IBandIndicator
    {
        /// <summary>
        /// Historical values of the indicator.
        /// </summary>
        ReadOnlyCollection<Band> HistoricalValues { set; }
    }
}
