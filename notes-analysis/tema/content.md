# TEMA

[Chart: OHLCV]

The Triple Exponential Moving Average (TEMA) was introduced by Patrick G. Mulloy in
the Technical Analysis of Stocks & Commodities (Mulloy 1994 12(1), Mulloy 1994 12(2))
as an extension of the double-smoothing tecnique described in
[Double Exponential Moving Average](/demaNote.route) (DEMA) to make
the TEMA "faster" (more sensitive to changes in the input data) than the DEMA and to have
a reduced lag.

In Mulloy 1994 12(2), the formula for TEMA is defined without any derivation as

$$\tag*{(1)}tema(x)=3E(x)-3E(E(x))+E(E(E(x)))$$

The calculation of TEMA goes as follows.

$$\tag*{(2)}\begin{array}{lcl}ema1_{k}&=&\alpha x_{k}+(1-\alpha)ema1_{k-1}\\ &=&ema_{k-1}+\alpha(x_{k}-ema1_{k-1})\end{array}$$

$$\tag*{(3)}\begin{array}{lcl}ema2_{k}&=&ema(ema1_{k})\\ &=&\alpha ema1_{k}+(1-\alpha)ema2_{k-1}\\ &=&ema2_{k-1}+\alpha(ema1_{k}-ema2_{k-1})\end{array}$$

$$\tag*{(4)}\begin{array}{lcl}ema3_{k}&=&ema(ema2_{k})\\ &=&\alpha ema2_{k}+(1-\alpha)ema3_{k-1}\\ &=&ema3_{k-1}+\alpha(ema2_{k}-ema3_{k-1})\end{array}$$

$$\tag*{(5)}tema_{k}=3(ema1_{k}-ema2_{k})+ema3_{k}$$

Here the first EMA smoothes the price, the second EMA smoothes the first EMA
and the third EMA smoothes the second EMA.
The equation (5) follows from the equation (1).

As in DEMA, the initial EMA values may be calculated in two different ways.
The first approach is to use the [Simple Moving Average](/smaNote.route)
over the first

$$\tag*{(6)}L=\frac{2}{\alpha}-1$$

input data values, where $L$ is an equivalent length of the
smoothing parameter $\alpha$. We assume $\alpha$
to be the same for both EMAs. This approach is the most widely used one.

The second way to initialize the EMAs is to use the first input data point as the initial value.
It is used by the well known (in the past) Metastock trading software.

Regardless of the initialization approach, we always consider TEMA to be primed only
after the $3L-1$ input data points have been fed into the indicator.

From the digital signal processing (DSP) point of view, TEMA is a
infinite impulse response (IIR) filter because it is based on EMA which
applies exponentially decreasing weighting factors to the input samples.

## Step response
Two figures below demonstrate the response to the step-up and step-down data.
The transition is clearly not linear.
The step-up response overshoot and the step-down response undershoot the data.

[Chart: OHLCV]
*Step-up response.*

[Chart: OHLCV]
*Step-down response.*

## Frequency response

The figure below shows an amplitude and a phase lag of the unit sample response of a TEMA as a function
of a period of various signal frequencies.

A period is a duration of a cycle in samples.
The smallest possible period of a cycle is $2$ samples.
To understand this, imagine a cycle of a sinusoid which starts at zero, goes up and peaks at $1$,
continues down and bottoms at $-1$, and then returns back to zero.
We need at least two samples (peak and trough) to represent a cycle.

[Chart: (a) amplitudePct vs period]
[Chart: (b) phaseDeg vs period]

*An amplitude (a) and a phase lag (b) as a function of a period of a cycle.*

A period ($\tau$) is an inverse of the cycle's frequency
($\nu$): $\tau = \frac{1}{\nu}$.
The smallest period $\tau = 2$ corresponds to the Nyquist frequency
$\nu = \frac{1}{\tau} = \frac{1}{2}$ which is the highest frequency possible in a signal.

[Chart: (a) amplitudePct vs period]
[Chart: (b) amplitudePct vs frequency]
*An amplitude as a function of a period of a cycle (a) and as a function of a normalized frequency of a cycle (b) for various TEMA lengths.*

[Chart: (a) phaseDeg vs period]
[Chart: (b) phaseDeg vs frequency]
*A phase as a function of a period of a wave (a) and as a function of a normalized frequency of a wave for various TEMA lengths.*

---

## References
Mulloy, P. G. (1994).
Smoothing Data With Faster Moving Averages. *Technical Analysis of Stocks & Commodities*, *12*(1), 11–19.
[traders.com](https://technical.traders.com/archive/volume-2014.asp?yr=1994#Jan)

Mulloy, P. G. (1994).
Smoothing Data With Less Lag. *Technical Analysis of Stocks & Commodities*, *12*(2), 72–80.
[traders.com](https://technical.traders.com/archive/volume-2014.asp?yr=1994#Feb)

Article Archive For Patrick G. Mulloy.
*Technical Analysis of Stocks & Commodities*.
[traders.com](https://technical.traders.com/archive/combo/display5.asp?author=Patrick%20G%20Mulloy)
