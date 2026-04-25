using System.ComponentModel;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Enumerates color interpolation types.
    /// </summary>
    public enum ColorInterpolationType
    {
        /// <summary>Linear.</summary>
        [Description("Linear")]
        Linear = 0,

        /// <summary>Quadratic.</summary>
        [Description("Quadratic")]
        Quadratic,

        /// <summary>Cubic.</summary>
        [Description("Cubic")]
        Cubic,

        /// <summary>Inverse quadratic.</summary>
        [Description("Inverse quadratic")]
        InverseQuadratic,

        /// <summary>Inverse cubic.</summary>
        [Description("Cubic")]
        InverseCubic
    }
}
