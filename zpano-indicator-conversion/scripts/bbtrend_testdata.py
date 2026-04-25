#!/usr/bin/env python3
"""Generate BBTrend test data using the same 252-element closing price array."""
import math

closing_price = [
    91.5000, 94.8150, 94.3750, 95.0950, 93.7800, 94.6250, 92.5300, 92.7500, 90.3150, 92.4700,
    96.1250, 97.2500, 98.5000, 89.8750, 91.0000, 92.8150, 89.1550, 89.3450, 91.6250, 89.8750,
    88.3750, 87.6250, 84.7800, 83.0000, 83.5000, 81.3750, 84.4400, 89.2500, 86.3750, 86.2500,
    85.2500, 87.1250, 85.8150, 88.9700, 88.4700, 86.8750, 86.8150, 84.8750, 84.1900, 83.8750,
    83.3750, 85.5000, 89.1900, 89.4400, 91.0950, 90.7500, 91.4400, 89.0000, 91.0000, 90.5000,
    89.0300, 88.8150, 84.2800, 83.5000, 82.6900, 84.7500, 85.6550, 86.1900, 88.9400, 89.2800,
    88.6250, 88.5000, 91.9700, 91.5000, 93.2500, 93.5000, 93.1550, 91.7200, 90.0000, 89.6900,
    88.8750, 85.1900, 83.3750, 84.8750, 85.9400, 97.2500, 99.8750, 104.9400, 106.0000, 102.5000,
    102.4050, 104.5950, 106.1250, 106.0000, 106.0650, 104.6250, 108.6250, 109.3150, 110.5000, 112.7500,
    123.0000, 119.6250, 118.7500, 119.2500, 117.9400, 116.4400, 115.1900, 111.8750, 110.5950, 118.1250,
    116.0000, 116.0000, 112.0000, 113.7500, 112.9400, 116.0000, 120.5000, 116.6200, 117.0000, 115.2500,
    114.3100, 115.5000, 115.8700, 120.6900, 120.1900, 120.7500, 124.7500, 123.3700, 122.9400, 122.5600,
    123.1200, 122.5600, 124.6200, 129.2500, 131.0000, 132.2500, 131.0000, 132.8100, 134.0000, 137.3800,
    137.8100, 137.8800, 137.2500, 136.3100, 136.2500, 134.6300, 128.2500, 129.0000, 123.8700, 124.8100,
    123.0000, 126.2500, 128.3800, 125.3700, 125.6900, 122.2500, 119.3700, 118.5000, 123.1900, 123.5000,
    122.1900, 119.3100, 123.3100, 121.1200, 123.3700, 127.3700, 128.5000, 123.8700, 122.9400, 121.7500,
    124.4400, 122.0000, 122.3700, 122.9400, 124.0000, 123.1900, 124.5600, 127.2500, 125.8700, 128.8600,
    132.0000, 130.7500, 134.7500, 135.0000, 132.3800, 133.3100, 131.9400, 130.0000, 125.3700, 130.1300,
    127.1200, 125.1900, 122.0000, 125.0000, 123.0000, 123.5000, 120.0600, 121.0000, 117.7500, 119.8700,
    122.0000, 119.1900, 116.3700, 113.5000, 114.2500, 110.0000, 105.0600, 107.0000, 107.8700, 107.0000,
    107.1200, 107.0000, 91.0000, 93.9400, 93.8700, 95.5000, 93.0000, 94.9400, 98.2500, 96.7500,
    94.8100, 94.3700, 91.5600, 90.2500, 93.9400, 93.6200, 97.0000, 95.0000, 95.8700, 94.0600,
    94.6200, 93.7500, 98.0000, 103.9400, 107.8700, 106.0600, 104.5000, 105.0000, 104.1900, 103.0600,
    103.4200, 105.2700, 111.8700, 116.0000, 116.6200, 118.2800, 113.3700, 109.0000, 109.7000, 109.2500,
    107.0000, 109.1900, 110.0000, 109.2000, 110.1200, 108.0000, 108.6200, 109.7500, 109.8100, 109.0000,
    108.7500, 107.8700,
]


class SMA:
    def __init__(self, length):
        self.length = length
        self.buffer = []
        self.sum = 0.0
        self.primed = False

    def update(self, sample):
        self.buffer.append(sample)
        self.sum += sample
        if len(self.buffer) > self.length:
            self.sum -= self.buffer[0]
            self.buffer.pop(0)
        if len(self.buffer) == self.length:
            self.primed = True
            return self.sum / self.length
        return float('nan')


class Variance:
    def __init__(self, length, unbiased=True):
        self.length = length
        self.unbiased = unbiased
        self.buffer = []
        self.primed = False

    def update(self, sample):
        self.buffer.append(sample)
        if len(self.buffer) > self.length:
            self.buffer.pop(0)
        if len(self.buffer) < self.length:
            return float('nan')
        self.primed = True
        mean = sum(self.buffer) / self.length
        ss = sum((x - mean) ** 2 for x in self.buffer)
        divisor = (self.length - 1) if self.unbiased else self.length
        return ss / divisor


class BollingerBandsLine:
    """Simplified BB that returns (lower, middle, upper) for BBTrend composition."""
    def __init__(self, length, upper_mult=2.0, lower_mult=2.0, unbiased=True):
        self.ma = SMA(length)
        self.var = Variance(length, unbiased)
        self.upper_mult = upper_mult
        self.lower_mult = lower_mult
        self.primed = False

    def update(self, sample):
        middle = self.ma.update(sample)
        v = self.var.update(sample)
        self.primed = self.ma.primed and self.var.primed
        if math.isnan(middle) or math.isnan(v):
            return float('nan'), float('nan'), float('nan')
        stddev = math.sqrt(v)
        upper = middle + self.upper_mult * stddev
        lower = middle - self.lower_mult * stddev
        return lower, middle, upper


def compute_bbtrend(data, fast_length=20, slow_length=50, upper_mult=2.0, lower_mult=2.0, unbiased=True):
    fast_bb = BollingerBandsLine(fast_length, upper_mult, lower_mult, unbiased)
    slow_bb = BollingerBandsLine(slow_length, upper_mult, lower_mult, unbiased)
    results = []
    for sample in data:
        fl, fm, fu = fast_bb.update(sample)
        sl, sm, su = slow_bb.update(sample)
        if not (fast_bb.primed and slow_bb.primed):
            results.append(float('nan'))
            continue
        lower_diff = abs(fl - sl)
        upper_diff = abs(fu - su)
        epsilon = 1e-10
        if abs(fm) < epsilon:
            results.append(0.0)
        else:
            results.append((lower_diff - upper_diff) / fm)
    return results


def fmt(v):
    if math.isnan(v):
        return "nan"
    return f"{v:.10f}"


# Sample (unbiased) variant
sample_results = compute_bbtrend(closing_price, 20, 50, 2.0, 2.0, True)
# Population variant
pop_results = compute_bbtrend(closing_price, 20, 50, 2.0, 2.0, False)

print("// Sample (unbiased) BBTrend expected values")
print("// fastLength=20, slowLength=50, multiplier=2, unbiased=true")
for i, v in enumerate(sample_results):
    print(f"  {i}: {fmt(v)}")

print()
print("// Population BBTrend expected values")
print("// fastLength=20, slowLength=50, multiplier=2, unbiased=false")
for i, v in enumerate(pop_results):
    print(f"  {i}: {fmt(v)}")

# Also output as Go array format
print("\n\n// === Go array format (sample/unbiased) ===")
lines = []
for i in range(0, len(sample_results), 5):
    chunk = sample_results[i:i+5]
    parts = []
    for v in chunk:
        if math.isnan(v):
            parts.append("nan")
        else:
            parts.append(f"{v:.10f}")
    lines.append(", ".join(parts) + ",")
for line in lines:
    print(f"\t\t{line}")

print("\n// === Go array format (population) ===")
lines = []
for i in range(0, len(pop_results), 5):
    chunk = pop_results[i:i+5]
    parts = []
    for v in chunk:
        if math.isnan(v):
            parts.append("nan")
        else:
            parts.append(f"{v:.10f}")
    lines.append(", ".join(parts) + ",")
for line in lines:
    print(f"\t\t{line}")
