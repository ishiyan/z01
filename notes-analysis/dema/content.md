# DEMA

[Chart: OHLCV]

The Double Exponential Moving Average (DEMA) was introduced by Patrick G. Mulloy in
the Technical Analysis of Stocks & Commodities (Mulloy 1994 12(1), Mulloy 1994 12(2)).
He wanted to make an indicator "faster" (more sensitive to changes in the input data) than the
[Exponential Moving Average](/emaNote.route) and to have a reduced lag.

Details about Mulloy’s personal life remain largely unknown, the only source of information being his
two articles in the Technical Analysis of Stocks & Commodities journal mentioned above.
I wasn't able to find any photo of him.

As Tim Tillson mentions in (Tillson 1998), Mulloy used a technique, well known in Digital
Signal Processing, called "twicing" (Oppenheim et al., 2009 p.609), to decrease the lag of
a Linear Time-Invariant (LTI) filter. If $E$ is a low-pass LTI filter
operator, then

$$\tag*{}twice(x)=E(x)+E(x-E(x))$$

which is algebraically equivalent to

$$\tag*{(1)}=2E(x)-E(E(x))$$

Here we are adding the smoothed residials to the smoothed values.
Note that the equation (1) assumes $E$ to be an LTI filter, which
allows us to expand $E(x-E(x))$ to $E(x)-E(E(x))$.

Patrick Mulloy doesn't mention the "twicing" technique in his articles.
Instead, he describes a "nonrigorous derivation" (Mulloy 1994 12(1)), assuming the input
data samples $\{x_k\}$ to form a linear sequence

$$\tag*{}x_{k}=a+bk, \quad a=const, b=const$$

The EMA that lags $\{x_k\}$ by $l$ samples
can be written as

$$\tag*{}\begin{array}{lcl}ema_{k}&=&a+b(k-l)\\ &=&a+bk-bl\\ &=&x_{k}-bl\end{array}$$

Using the same logic, when applying the EMA over EMA,

$$\tag*{}ema2_{k}=ema_{k}-bl$$

Subtracting two previous equations gives

$$\tag*{}\begin{array}{lcl}ema_{k}-ema2_{k}&=&a+b(k-l)\\ &=&x_{k}-bl-(ema_{k}-bl)\\ &=&x_{k}-ema_{k}\end{array}$$

from which

$$\tag*{}x_{k}=2ema_{k}-ema2_{k}$$

Then, Patrick Mulloy defines DEMA to be the same as $x_k$

$$\tag*{}dema_{k}=2ema_{k}-ema2_{k}$$

This "nonrigorous derivation" seems unclear to me.

The calculation of DEMA goes as follows.

$$\tag*{(2)}\begin{array}{lcl}ema1_{k}&=&\alpha x_{k}+(1-\alpha)ema1_{k-1}\\ &=&ema_{k-1}+\alpha(x_{k}-ema1_{k-1})\end{array}$$

$$\tag*{(3)}\begin{array}{lcl}ema2_{k}&=&ema(ema1_{k})\\ &=&\alpha ema1_{k}+(1-\alpha)ema2_{k-1}\\ &=&ema2_{k-1}+\alpha(ema1_{k}-ema2_{k-1})\end{array}$$

$$\tag*{(4)}dema_{k}=2ema1_{k}-ema2_{k}$$

Here the first EMA smoothes the price and the second EMA smoothes the first EMA.
The equation (4) follows from the equation (1).

The initial EMA values may be calculated in two different ways.
The first approach is to use the [Simple Moving Average](/smaNote.route)
over the first

$$\tag*{(5)}L=\frac{2}{\alpha}-1$$

input data values, where $L$ is an equivalent length of the
smoothing parameter $\alpha$. We assume $\alpha$
to be the same for both EMAs. This approach is the most widely used one.

The second way to initialize the EMAs is to use the first input data point as the initial value.
It is used by the well known (in the past) Metastock trading software.

Regardless of the initialization approach, we always consider DEMA to be primed only
after the $2L-1$ input data points have been fed into the indicator.

From the digital signal processing (DSP) point of view, DEMA is a
infinite impulse response (IIR) filter because it is based on EMA which
applies exponentially decreasing weighting factors to the input samples.

## Step response

Two figures below demonstrate the response to the step-up and step-down data.
The transition is clearly not linear.
The step-up response overshoots and the step-down response undershoots the data.

[Chart: OHLCV]
*Step-up response.*

[Chart: OHLCV]
*Step-down response.*

## Frequency response

The figure below shows an amplitude and a phase lag of the unit sample response of a DEMA as a function
of a period of various signal frequencies.

A period is a duration of a cycle in samples.
The smallest possible period of a cycle is $2$ samples.
To understand this, imagine a cycle of a sinusoid which starts at zero, goes up and peaks at $1$,
continues down and bottoms at $-1$, and then returns back to zero.
We need at least two samples (peak and trough) to represent a cycle.
See more details in the [frequency response article](/frequency-response).

[Chart: (a) amplitudePct vs period]
[Chart: (b) phaseDeg vs period]

*An amplitude (a) and a phase lag (b) as a function of a period of a cycle.*

The same charts can be represented as a function of a cycle's frequency.
A period ($\tau$) is an inverse of the cycle's frequency
($\nu$): $\tau = \frac{1}{\nu}$.
The smallest period $\tau = 2$ corresponds to the Nyquist frequency
$\nu = \frac{1}{\tau} = \frac{1}{2}$which is the highest frequency
possible in a signal. Below we use the normalized frequency which has the value of $1$
at the Nyquist frequency.
That is, $0$ corresponds to the infinite $\tau$,
$1$ corresponds to the $\tau = 2$.

[Chart: (a) amplitudePct vs period]
[Chart: (b) amplitudePct vs frequency]
*An amplitude as a function of a period of a cycle (a) and as a function of a normalized frequency of a cycle (b) for various DEMA lengths.*

[Chart: (a) phaseDeg vs period]
[Chart: (b) phaseDeg vs frequency]
*A phase as a function of a period of a wave (a) and as a function of a normalized frequency of a wave for various DEMA lengths.*

---

## References
Oppenheim, A. V., Schafer, R. W., Yoder, M. A., & Padgett, W. T. (2009).
*Discrete-time signal processing*. (3rd ed., p. 1120). Upper Saddle River, NJ: Pearson.
[google books](https://books.google.com/books?id=EaMuAAAAQBAJ)

Mulloy, P. G. (1994).
Smoothing Data With Faster Moving Averages. *Technical Analysis of Stocks & Commodities*, *12*(1), 11–19.
[traders.com](https://technical.traders.com/archive/volume-2014.asp?yr=1994#Jan)

Mulloy, P. G. (1994).
Smoothing Data With Less Lag. *Technical Analysis of Stocks & Commodities*, *12*(2), 72–80.
[traders.com](https://technical.traders.com/archive/volume-2014.asp?yr=1994#Feb)

Tillson, Tim (1998).
Smoothing Techniques For More Accurate Signals. *Technical Analysis of Stocks & Commodities*, *16*(1), 33–37.
[traders.com](https://technical.traders.com/archive/volume-2014.asp?yr=1998#Jan)

Article Archive For Patrick G. Mulloy.
*Technical Analysis of Stocks & Commodities*.
[traders.com](https://technical.traders.com/archive/combo/display5.asp?author=Patrick%20G%20Mulloy)
