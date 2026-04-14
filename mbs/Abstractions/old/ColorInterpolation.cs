using System;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Interpolates color between two color points.
    /// </summary>
    [DataContract]
    internal static class ColorInterploation
    {
        private static readonly Func<Color,byte> redSelector = color => color.R;
        private static readonly Func<Color, byte> greenSelector = color => color.G;
        private static readonly Func<Color, byte> blueSelector = color => color.B;
        private static readonly Func<Color, byte> alphaSelector = color => color.A;

        private static byte InterpolateComponent(Color endPoint1, Color endPoint2, double lambda, Func<Color, byte> selector)
        {
            return (byte)(selector(endPoint1) + (selector(endPoint2) - selector(endPoint1)) * lambda);
        }

        internal static Color InterpolateBetween(Color downPoint, Color upPoint, double lambda, ColorInterpolationType type)
        {
            if (lambda < 0 || lambda > 1)
                throw new ArgumentOutOfRangeException("lambda");
            switch (type)
            {
                case ColorInterpolationType.Quadratic:
                    lambda *= lambda;
                    break;
                case ColorInterpolationType.Cubic:
                    lambda *= lambda * lambda;
                    break;
                case ColorInterpolationType.InverseQuadratic:
                    lambda = 1 - lambda;
                    lambda = 1 - lambda * lambda;
                    break;
                case ColorInterpolationType.InverseCubic:
                    lambda = 1 - lambda;
                    lambda = 1 - lambda * lambda * lambda;
                    break;
                //case ColorInterpolationType.Linear:
                //default:
                //    break;
            }
            return Color.FromArgb(
                InterpolateComponent(downPoint, upPoint, lambda, alphaSelector),
                InterpolateComponent(downPoint, upPoint, lambda, redSelector),
                InterpolateComponent(downPoint, upPoint, lambda, greenSelector),
                InterpolateComponent(downPoint, upPoint, lambda, blueSelector));
        }
    }
}