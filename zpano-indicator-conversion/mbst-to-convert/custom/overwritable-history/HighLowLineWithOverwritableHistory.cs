using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Mbst.Trading;
using Mbst.Trading.Indicators;

namespace Charts
{
    internal sealed class HighLowLineWithOverwritableHistory : Indicator, ILineIndicatorWithOverwritableHistory
    {
        private int windowCount;
        private readonly double[] windowHigh, windowLow;
        private readonly int length, lastIndex;
        private readonly int offsetMax, offsetMin;
        private readonly bool isVolume;
        public ReadOnlyCollection<Scalar> HistoricalValues { set; private get; }

        public HighLowLineWithOverwritableHistory(int offset1, int offset2, bool isVolume = false)
            : base("HiLoLine", "High-low line with overwritable history")
        {
            if (1 > offset1)
                throw new ArgumentOutOfRangeException(nameof(offset1));
            if (1 > offset2)
                throw new ArgumentOutOfRangeException(nameof(offset2));
            if (offset1 >= offset2)
            {
                offsetMax = offset1;
                offsetMin = offset2;
            }
            else
            {
                offsetMin = offset1;
                offsetMax = offset2;
            }
            length = offsetMax;
            lastIndex = length - 1;
            windowHigh = new double[length];
            windowLow = new double[length];
            this.isVolume = isVolume;
            moniker = string.Format(CultureInfo.InvariantCulture, "HiLoLine({0},{1})", offsetMax, offsetMin);
        }

        public override void Reset()
        {
            lock (updateLock)
            {
                primed = false;
                windowCount = 0;
            }
        }

        private double Update(double sampleHigh, double sampleLow)
        {
            if (double.IsNaN(sampleHigh) || double.IsNaN(sampleLow))
                return double.NaN;
            lock (updateLock)
            {
                if (!primed)
                {
                    windowHigh[windowCount] = sampleHigh;
                    windowLow[windowCount] = sampleLow;
                    if (length == ++windowCount)
                        primed = true;
                }
                else
                {
                    Array.Copy(windowHigh, 1, windowHigh, 0, lastIndex);
                    Array.Copy(windowLow, 1, windowLow, 0, lastIndex);
                    windowHigh[lastIndex] = sampleHigh;
                    windowLow[lastIndex] = sampleLow;
                    int historicalCount = HistoricalValues.Count;

                    HistoricalValues[Math.Max(historicalCount - offsetMax - 1, 0)].Value = double.NaN;
                    HistoricalValues[Math.Max(historicalCount - offsetMin - 1, 0)].Value = double.NaN;

                    HistoricalValues[historicalCount - offsetMax].Value = windowHigh[0];
                    HistoricalValues[historicalCount - offsetMin].Value = windowLow[offsetMax - offsetMin];
                }
                return double.NaN;
            }
        }

        public double Update(double sample)
        {
            return Update(sample, sample);
        }

        public Scalar Update(double sample, DateTime dateTime)
        {
            return new Scalar(dateTime, Update(sample, sample));
        }

        public Scalar Update(Scalar scalar)
        {
            return new Scalar(scalar.Time, Update(scalar.Value, scalar.Value));
        }

        public Scalar Update(Ohlcv ohlcv)
        {
            return isVolume ? new Scalar(ohlcv.Time, Update(ohlcv.Volume, ohlcv.Volume)) :
                new Scalar(ohlcv.Time, Update(ohlcv.High, ohlcv.Low));
        }
    }
}
