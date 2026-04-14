using System.Collections.ObjectModel;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// A line indicator with overwritable history interface.
    /// </summary>
    public interface ILineIndicatorWithOverwritableHistory : ILineIndicator
    {
        /// <summary>
        /// Historical values of the indicator.
        /// </summary>
        ReadOnlyCollection<Scalar> HistoricalValues { set; }
    }
}
