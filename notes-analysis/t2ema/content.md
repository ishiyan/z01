# T2EMA

[Chart: OHLCV]

The idea of the T2 Exponential Moving Average (T2) was presented by Tim Tillson in his article in
the Technical Analysis of Stocks & Commodities (Tillson 1998 16(1)).
He reflects on [Double Exponential Moving Average](/demaNote.route)
(described by Patrick G. Mulloy in the same journal in 1994, see Mulloy 1994 12(1) and Mulloy 1994 12(2)),
noticing that it uses a technique, well known in Digital Signal Processing,
called "twicing" (Oppenheim et al., 2009 p.609), to decrease the lag of a Linear Time-Invariant (LTI) filter.
If $E$ is a low-pass LTI filter operator, then

$$\tag*{}twice(x)=E(x)+E(x-E(x))$$

which is algebraically equivalent to

$$\tag*{(1)}twice(x)=2E(x)-E(E(x))$$

Here we are adding the smoothed residials to the smoothed values.
Note that the equation (1) assumes $E$ to be an LTI filter, which
allows us to expand $E(x-E(x))$ to $E(x)-E(E(x))$.

![](assets/tillson.jpg)
*Tim Tillson is a software project manager at Hewlett-Packard,
with degrees in Mathematics and Computer Science. He has privately
traded options and equities for 15 years. See his
[Linked-In](https://www.linkedin.com/in/timtillson/) profile.

DEMA applies this "twicing" technique to the [Exponential Moving Average](/emaNote.route) (EMA)
to make EMA smoother and to reduce its lag. The drawback of DEMA is that it overshoots or undershoots
the input price data during fast price movements.

Looking at the definition of the DEMA in equation (1) and noticing that the second term adds
a smoothed derivative to the EMA value, contributing to the overshooting and undershooting,
he suggested to "turn down the volume" of the derivative using "volume factor"
$v$, $0\le v\le 1$:

$$\tag*{(2)}gd_v(x)=(1+v)E(x)-vE(E(x))$$

Here $gd_v(x)$ is the "Generalized DEMA" (GD), as Tim Tillson named it.
When $v=1$, GD becomes DEMA. When $v=0$, GD becomes a simple EMA.
In between, GD is a "cooler" DEMA with reduced overshooting and undershooting.
The default value of $v$ is $0.7$.

Tim Tillson defined the T2 Exponential Moving Average as a GD of a GD

$$\tag*{(3)}t2_v(x)=gd_v(gd_v(x))$$

saying it is equivalent to

$$\tag*{(4)}t2_v=((1+v)e-ve^2)^2, e \equiv E(x)$$

We can simplify this expression recalling a well-known identity (Jeffrey and Dai, 2008, 1.2.1.2)

$$\tag*{}(a+b)^2=a^2+2ab+b^2$$

and denoting $a=(1+v)e$ and $b=-ve^2$:

$$\tag*{(5)}\begin{array}{lcl}t2_v&=&(1+v)^2e^2-2v(1+v)e^3+v^2e^4\\ &=&v^2e^4-2v(1+v)e^3+(1+v)^2e^2\\ &=&c_1e^4+c_2e^3+c_3e^2\end{array}$$

where

$$\tag*{(6)}\begin{array}{lcl}c_1&=&v^2\\ c_2&=&-2v(1+v)\\ c_3&=&(1+v)^2\end{array}$$

The calculation of the T2 goes as follows:

$$\tag*{(7)}\begin{array}{lcl}ema1_{k}&=&\alpha x_{k}+(1-\alpha)ema1_{k-1}\\ &=&ema_{k-1}+\alpha(x_{k}-ema1_{k-1})\end{array}$$

$$\tag*{(8)}\begin{array}{lcl}ema2_{k}&=&ema(ema1_{k})\\ &=&\alpha ema1_{k}+(1-\alpha)ema2_{k-1}\\ &=&ema2_{k-1}+\alpha(ema1_{k}-ema2_{k-1})\end{array}$$

$$\tag*{(9)}\begin{array}{lcl}ema3_{k}&=&ema(ema2_{k})\\ &=&\alpha ema2_{k}+(1-\alpha)ema3_{k-1}\\ &=&ema3_{k-1}+\alpha(ema2_{k}-ema3_{k-1})\end{array}$$

$$\tag*{(10)}\begin{array}{lcl}ema4_{k}&=&ema(ema3_{k})\\ &=&\alpha ema3_{k}+(1-\alpha)ema4_{k-1}\\ &=&ema4_{k-1}+\alpha(ema3_{k}-ema4_{k-1})\end{array}$$

$$\tag*{(11)}t2_{k}=c_1ema4_{k}+c_2ema3_{k}+c_3ema2_{k}$$

The initial EMA values may be calculated in two different ways.
The first approach is to use the [Simple Moving Average](/smaNote.route)
over the first

$$\tag*{(12)}L=\frac{2}{\alpha}-1$$

input data values, where $L$ is an equivalent length of the
smoothing parameter $\alpha$. We assume $\alpha$
to be the same for all EMAs. This approach is the most widely used one.

The second way to initialize the EMAs is to use the first input data point as the initial value.
It is used by the well known (in the past) Metastock trading software.

Regardless of the initialization approach, we always consider T2 to be primed only
after the $4L-3$ input data points have been fed into the indicator.

From the digital signal processing (DSP) point of view, T2 is a
infinite impulse response (IIR) filter because it is based on EMA which
applies exponentially decreasing weighting factors to the input samples.

## Step response
Two figures below demonstrate the response to the step-up and step-down data
for indicators shown above.
The transition is clearly not linear and you can see the overshooting and undershooting.

[Chart: OHLCV]
*Step-up response.*

[Chart: OHLCV]
*Step-down response.*

How the length and the volume factor affect the response?
In four figures below, we compare the step-up and step-down responses
for different lengths and volume factors.

Increasing the volume factor decreases the lag, but increases the overshooting and undershooting.
Increasing the length increases both the lag and the overshooting and undershooting.

[Chart: OHLCV]
*Step-up response for different lengths and equal volume factor.*

[Chart: OHLCV]
*Step-up response for different volume factor and equal length.*

[Chart: OHLCV]
*Step-down response for different lengths and equal volume factor.*

[Chart: OHLCV]
*Step-down response for different volume factor and equal length.*

## Frequency response

The figure below shows an amplitude and a phase lag of the unit sample response of a T2 as a function
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

How the length and the volume factor affect the frequency response?
In two figures below, we compare the amplitude and phase lag of the unit sample response
for different lengths and volume factors.

[Chart: (a) amplitudePct vs period]
[Chart: (b) amplitudePct vs frequency]
[Chart: (c) phaseDeg vs period]
[Chart: (d) phaseDeg vs frequency]
*An amplitude (a, b) and a phase (c, d) as a function of a period of a cycle (a, c) and as a function of a normalized frequency of a cycle (b, d) for various T2 lengths.*

[Chart: (a) amplitudePct vs period]
[Chart: (b) amplitudePct vs frequency]
[Chart: (c) phaseDeg vs period]
[Chart: (d) phaseDeg vs frequency]
*An amplitude (a, b) and a phase (c, d) as a function of a period of a cycle (a, c) and as a function of a normalized frequency of a cycle (b, d) for various T2 volume factors.*

---

## References
Jeffrey, A., & Dai, H. H. (2008).
*Handbook of mathematical formulas and integrals*. (4th ed., p. 592). San Diego, CA: Elsevier/Academic Press.
[google books](https://books.google.com/books?id=JokQD5nK4LMC)

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

Tillson, Tim.
*Tim Tillson - Fort Collins, Colorado, United States | professional profile | linkedin*, accessed March 2025.
[linkedin.com](https://www.linkedin.com/in/timtillson/)

Article Archive For Patrick G. Mulloy.
*Technical Analysis of Stocks & Commodities*.
[traders.com](https://technical.traders.com/archive/combo/display5.asp?author=Patrick%20G%20Mulloy)
