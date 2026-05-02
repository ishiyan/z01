# T3EMA

[Chart: OHLCV]

The idea of the T3 Exponential Moving Average (T3) was presented by Tim Tillson in his article in
the Technical Analysis of Stocks & Commodities (Tillson 1998 16(1)).
T3 is a smoother version of the [T2 Exponential Moving Average](/t2emaNote.route) (T2).

Tim Tillson defined the T3 as a GD of a GD of a GD

$$\tag*{(1)}t3_v(x)=gd_v(gd_v(gd_v(x)))$$

saying it is equivalent to

$$\tag*{(2)}t3_v=((1+v)e-ve^2)^3, e \equiv E(x)$$

Here $gd_v(x)$ is the "Generalized DEMA" (GD),
see the [T2 Exponential Moving Average](/t2emaNote.route) where it is explained in details.

We can simplify the expression (2) recalling a well-known identity (Jeffrey and Dai, 2008, 1.2.1.2)

$$\tag*{}(a+b)^3=a^3+3a^2b+3ab^2+b^3$$

and denoting $a=(1+v)e$ and $b=-ve^2$:

$$\tag*{(3)}\begin{array}{lcl}t3_v&=&(1+v)^3e^3-3v(1+v)^2e^4+3v^2(1+v)e^5-v^3e^6\\ &=&-v^3e^6+3v^2(1+v)e^5-3v(1+v)^2e^4+(1+v)^3e^3\\ &=&c_1e^6+c_2e^5+c_3e^4+c_4e^3\end{array}$$

where

$$\tag*{(4)}\begin{array}{lcl}c_1&=&-v^3\\ c_2&=&3(1+v)v^2\\ c_3&=&-3v(1+v)^2\\ c_4 &=&(1+v)^3\end{array}$$

The calculation of the T3 goes as follows:

$$\tag*{(5)}\begin{array}{lcl}ema1_{k}&=&\alpha x_{k}+(1-\alpha)ema1_{k-1}\\ &=&ema_{k-1}+\alpha(x_{k}-ema1_{k-1})\end{array}$$

$$\tag*{(6)}\begin{array}{lcl}ema2_{k}&=&ema(ema1_{k})\\ &=&\alpha ema1_{k}+(1-\alpha)ema2_{k-1}\\ &=&ema2_{k-1}+\alpha(ema1_{k}-ema2_{k-1})\end{array}$$

$$\tag*{(7)}\begin{array}{lcl}ema3_{k}&=&ema(ema2_{k})\\ &=&\alpha ema2_{k}+(1-\alpha)ema3_{k-1}\\ &=&ema3_{k-1}+\alpha(ema2_{k}-ema3_{k-1})\end{array}$$

$$\tag*{(8)}\begin{array}{lcl}ema4_{k}&=&ema(ema3_{k})\\ &=&\alpha ema3_{k}+(1-\alpha)ema4_{k-1}\\ &=&ema4_{k-1}+\alpha(ema3_{k}-ema4_{k-1})\end{array}$$

$$\tag*{(9)}\begin{array}{lcl}ema5_{k}&=&ema(ema4_{k})\\ &=&\alpha ema4_{k}+(1-\alpha)ema5_{k-1}\\ &=&ema5_{k-1}+\alpha(ema4_{k}-ema5_{k-1})\end{array}$$

$$\tag*{(10)}\begin{array}{lcl}ema6_{k}&=&ema(ema5_{k})\\ &=&\alpha ema5_{k}+(1-\alpha)ema6_{k-1}\\ &=&ema6_{k-1}+\alpha(ema5_{k}-ema6_{k-1})\end{array}$$

$$\tag*{(11)}t3_{k}=c_1ema6_{k}+c_2ema5_{k}+c_3ema4_{k}+c_4ema3_{k}$$

As in T2, the initial EMA values may be calculated in two different ways.
The first approach is to use the [Simple Moving Average](/smaNote.route)
over the first

$$\tag*{(12)}L=\frac{2}{\alpha}-1$$

input data values, where $L$ is an equivalent length of the
smoothing parameter $\alpha$. We assume $\alpha$
to be the same for all EMAs. This approach is the most widely used one.

The second way to initialize the EMAs is to use the first input data point as the initial value.
It is used by the well known (in the past) Metastock trading software.

Regardless of the initialization approach, we always consider T3 to be primed only
after the $6L-5$ input data points have been fed into the indicator.

From the digital signal processing (DSP) point of view, T3 is a
infinite impulse response (IIR) filter because it is based on EMA which
applies exponentially decreasing weighting factors to the input samples.

## Step response
Two figures below demonstrate the response to the step-up and step-down data
for indicators shown above.
The transition is clearly not linear and you can see the overshooting and undershooting.

*Step-up response.*

*Step-down response.*

How the length and the volume factor affect the response?
In four figures below, we compare the step-up and step-down responses
for different lengths and volume factors.

Increasing the volume factor decreases the lag, but increases the overshooting and undershooting.
Increasing the length increases both the lag and the overshooting and undershooting.

*Step-up response for different lengths and equal volume factor.*

*Step-up response for different volume factor and equal length.*

*Step-down response for different lengths and equal volume factor.*

*Step-down response for different volume factor and equal length.*

## Frequency response

The figure below shows an amplitude and a phase lag of the unit sample response of a T3 as a function
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
*An amplitude (a, b) and a phase (c, d) as a function of a period of a cycle (a, c) and as a function of a normalized frequency of a cycle (b, d) for various T3 lengths.*

[Chart: (a) amplitudePct vs period]
[Chart: (b) amplitudePct vs frequency]
[Chart: (c) phaseDeg vs period]
[Chart: (d) phaseDeg vs frequency]
*An amplitude (a, b) and a phase (c, d) as a function of a period of a cycle (a, c) and as a function of a normalized frequency of a cycle (b, d) for various T3 volume factors.*

---

## References
Jeffrey, A., & Dai, H. H. (2008).
*Handbook of mathematical formulas and integrals*. (4th ed., p. 592). San Diego, CA: Elsevier/Academic Press.
[google books](https://books.google.com/books?id=JokQD5nK4LMC)

Tillson, Tim (1998).
Smoothing Techniques For More Accurate Signals. *Technical Analysis of Stocks & Commodities*, *16*(1), 33–37.
[traders.com](https://technical.traders.com/archive/volume-2014.asp?yr=1998#Jan)
