using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// A drawing indicator interface.
    /// </summary>
    public interface IDrawingIndicator : IIndicator
    {
        /// <summary>
        /// Updates the drawing indicator.
        /// </summary>
        /// <param name="value">The value of a new sample.</param>
        /// <param name="dateTime">The date and time of a new sample.</param>
        void Update(double value, DateTime dateTime);

        /// <summary>
        /// Updates the drawing indicator.
        /// </summary>
        /// <param name="sample">A new <c>scalar</c> sample.</param>
        void Update(Scalar sample);

        /// <summary>
        /// Updates the drawing indicator.
        /// </summary>
        /// <param name="sample">A new <c>ohlcv</c> sample.</param>
        void Update(Ohlcv sample);

        /// <summary>
        /// Updates the references to the updated lower and the upper value bound values.
        /// </summary>
        /// <param name="lower">The lower value reference.</param>
        /// <param name="upper">The upper value reference.</param>
        void ValueBounds(ref double lower, ref double upper);

        /// <summary>
        /// Renders the indicator.
        /// </summary>
        /// <param name="dispatcher">The dispatcher to render on UI thread.</param>
        /// <param name="renderOpen">Opens a DrawingContext.</param>
        /// <param name="convertAbscissa">Converts a fractionalItem to an x-coordinate.</param>
        /// <param name="convertOrdinate">Converts a value to an y-coordinate.</param>
        /// <param name="actualWidth">A width of the visible part to render.</param>
        /// <param name="actualHeight">A height of the visible part to render.</param>
        void Render(Dispatcher dispatcher, Func<DrawingContext> renderOpen, Func<double, double> convertAbscissa,
            Func<double, double> convertOrdinate, double actualWidth, double actualHeight);
    }
}
