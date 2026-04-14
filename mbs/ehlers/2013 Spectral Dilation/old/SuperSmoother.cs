using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using Mbst.Numerics;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Given the shortest (<c>λ</c>) cycle period in bars, the Super Smoother filter
    /// attenuates cycle periods shorter than this shortest one.
    /// <para/>
    /// <c>γ₁ = 1-γ₂-γ₃</c>
    /// <para/>
    /// <c>γ₂ = 2cos(π√̅2̅/λ)exp(-π√̅2̅/λ)</c>
    /// <para/>
    /// <c>γ₃ = -exp(-π√̅2̅/λ)²</c>
    /// <para/>
    /// <c>SSᵢ = γ₁(xᵢ + xᵢ₋₁)/2 + γ₂SSᵢ₋₁ + γ₃SSᵢ₋₂</c>
    /// </summary>
    [DataContract]
    public sealed class SuperSmoother : Indicator, ILineIndicator
    {
        #region Members and accessors
        #region ShortestCyclePeriod
        [DataMember]
        private readonly int shortestCyclePeriod;
        /// <summary>
        /// The shortest cycle period in bars. The Super Smoother attenuates all cycle periods shorter than this one.
        /// </summary>
        public int ShortestCyclePeriod { get { return shortestCyclePeriod; } }
        #endregion

        #region Value
        [DataMember]
        private double value = double.NaN;
        /// <summary>
        /// The current value of the the Super Smoother, or <c>NaN</c> if not primed.
        /// The Super Smoother is not primed during the first 3 updates.
        /// </summary>
        public double Value { get { lock (updateLock) { return value; } } }
        #endregion

        #region SuperSmoother filter
        [DataMember]
        private double samplePrevious;
        [DataMember]
        private double superSmootherFilterPrevious;
        [DataMember]
        private double superSmootherFilterPrevious2;
        [DataMember]
        private readonly double superSmootherFilterCoeff1;
        [DataMember]
        private readonly double superSmootherFilterCoeff2;
        [DataMember]
        private readonly double superSmootherFilterCoeff3;
        #endregion

        [DataMember]
        private int count;

        private const string ssName = "SuperSmoother";
        private const string ssFull = "Super Smoother";
        private const string ssMonikerFormat = "SuperSmoother[({0}]";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="SuperSmoother"/> class.
        /// </summary>
        /// <param name="shortestCyclePeriod">The shortest cycle period in bars. The Super Smoother attenuates all cycle periods shorter than this one. The default value is 10 bars.</param>
        /// <param name="ohlcvComponent">The Ohlcv component. The default component is the median price.</param>
        public SuperSmoother(int shortestCyclePeriod = 10, OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
            : base(ssName, ssFull, ohlcvComponent)
        {
            if (2 > shortestCyclePeriod)
                throw new ArgumentOutOfRangeException("shortestCyclePeriod");
            this.shortestCyclePeriod = shortestCyclePeriod;

            double beta = Constants.Sqrt2 * Constants.Pi / shortestCyclePeriod;
            double alpha = Math.Exp(-beta);
            beta = 2 * alpha * Math.Cos(beta);
            superSmootherFilterCoeff2 = beta;
            superSmootherFilterCoeff3 = -alpha * alpha;
            superSmootherFilterCoeff1 = (1 - superSmootherFilterCoeff2 - superSmootherFilterCoeff3) / 2;

            moniker = string.Format(CultureInfo.InvariantCulture, ssMonikerFormat, shortestCyclePeriod);
        }
        #endregion

        #region Reset
        /// <summary>
        /// Resets the indicator.
        /// </summary>
        public override void Reset()
        {
            lock (updateLock)
            {
                primed = false;
                count = 0;
                value = double.NaN;
                samplePrevious = 0;
                superSmootherFilterPrevious = 0;
                superSmootherFilterPrevious2 = 0;
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates the value of the indicator.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public double Update(double sample)
        {
            if (double.IsNaN(sample))
                return sample;
            lock (updateLock)
            {
                double superSmootherFilter;
                if (primed)
                {
                    superSmootherFilter = superSmootherFilterCoeff1 * (sample + samplePrevious) +
                        superSmootherFilterCoeff2 * superSmootherFilterPrevious +
                        superSmootherFilterCoeff3 * superSmootherFilterPrevious2;
                    value = superSmootherFilter;
                }
                else // Not primed.
                {
                    if (1 == ++count)
                    {
                        samplePrevious = sample;
                        superSmootherFilterPrevious = sample;
                        superSmootherFilterPrevious2 = sample;
                    }
                    superSmootherFilter = superSmootherFilterCoeff1 * (sample + samplePrevious) +
                        superSmootherFilterCoeff2 * superSmootherFilterPrevious +
                        superSmootherFilterCoeff3 * superSmootherFilterPrevious2;
                    if (3 == count)
                    {
                        primed = true;
                        value = superSmootherFilter;
                    }
                }
                samplePrevious = sample;
                superSmootherFilterPrevious2 = superSmootherFilterPrevious;
                superSmootherFilterPrevious = superSmootherFilter;
                return value;
            }
        }

        /// <summary>
        /// Updates the value of the indicator.
        /// </summary>
        /// <param name="sample">A new sample.</param>
        /// <param name="dateTime">A date-time of the new sample.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(double sample, DateTime dateTime)
        {
            return new Scalar(dateTime, Update(sample));
        }

        /// <summary>
        /// Updates the value of the indicator.
        /// </summary>
        /// <param name="scalar">A new scalar.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Scalar scalar)
        {
            return new Scalar(scalar.Time, Update(scalar.Value));
        }

        /// <summary>
        /// Updates the value of the indicator.
        /// </summary>
        /// <param name="ohlcv">A new ohlcv.</param>
        /// <returns>The new value of the indicator.</returns>
        public Scalar Update(Ohlcv ohlcv)
        {
            return new Scalar(ohlcv.Time, Update(ohlcv.Component(ohlcvComponent)));
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns the string that represents this object.
        /// </summary>
        /// <returns>Returns the string that represents this object.</returns>
        public override string ToString()
        {
            double v; bool p;
            lock (updateLock)
            {
                p = primed;
                v = value;
            }
            var sb = new StringBuilder();
            sb.Append("{M:");
            sb.Append(moniker);
            sb.Append(" P:");
            sb.Append(p);
            sb.Append(" V:");
            sb.Append(v);
            sb.Append("]");
            return sb.ToString();
        }
        #endregion
    }
}
