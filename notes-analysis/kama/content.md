# KAMA

[Chart: OHLCV]

The KAMA lines are plotted in the price pane.
The lower pane shows the related efficiency ratios.

## How it works

The Kaufman Adaptive Moving Average (KAMA) was introduced by Perry J. Kaufman
in his book *Smarter Trading* (Kaufman 1995, chapter 8, pp.129-153) and later
popularized in *Trading Systems and Methods* (Kaufman 2013, chapter 17, pp. 779-783)
and in his article in *Technical Analysis of Stocks & Commodities* (Kaufman 2020, 38(11)).

KAMA is actually an
[Exponential Moving Average](/emaNote.route) (EMA)
with a variable smoothing factor $\alpha_k$
being changed with each new sample within the minimum and the maximun boundaries.

$$\tag*{(1)}kama_{k}=\alpha_{k} x_{k} + (1-\alpha_{k})kama_{k-1}$$

where

$$\tag*{(2)}\alpha_k=(er_{k}(\alpha_{fast}-\alpha_{slow})+\alpha_{slow})^2$$

and

$$\tag*{(3)}er_{k}=\frac{|x_k-x_{k-\ell_{er}}|}{\sum_{i=1}^{\ell_{er}-1}|x_{k-i+1}-x_{k-i}|}$$

If we recall that the EMA smoothing factor $\alpha$
can be expressed in terms of the EMA length $\ell$ as
(see [Exponential Moving Average](/emaNote.route))

$$\tag*{(4)}\alpha=\frac{2}{\ell+1}, \ell>1$$

we can specify the $\alpha_{slow}$ and $\alpha_{fast}$
using the $\ell_{slow}$ and $\ell_{fast}$,
which are the input parameters of the indicator.

![](assets/kaufman1.jpg)

Perry Kaufman began his career in the aerospace industry, working on navigation
and control systems for Gemini, and later transitioned his expertise to the financial world.
[perrykaufman.com](https://perrykaufman.com/bio/).

The variable smoothing factor $\alpha_k$ reacts quickly when the market
is trending strongly and smoothly, but slows down and filters out noise when the market
is choppy or moving sideways.
The value of the $\alpha_k$ is limited within the boundaries
$[\alpha_{slow}, \alpha_{fast}]$.

The smoothing factor $\alpha_k$ depends on the
"efficiency of price movement" over a specific period, which is also
an input parameter of the indicator.
Perry Kaufman named this efficiency measure $er_k$
the Efficiency Ratio (ER).

The ER is a ratio of the absolute value of return (the net directional movement)
to the sum of absolute values of the individual returns (Kaufman calls it
"volatility or noise") over the certain "efficiency ratio period",
$\ell_{er}$, as shown in (3).
The ER value ranges between $0$ and $1$.

An ER close to $1$ indicates a highly efficient trend
(strong directional movement with low noise).
In this case, KAMA uses a larger $\alpha$, making it behave
like a faster, more responsive moving average to follow the clear trend.

An ER close to $0$ suggests a lot of noise relative to the
net price movement (sideways or choppy market).
The $\alpha$ is small, making KAMA behave
like a slower moving average, which helps to filter out the noise in choppy markets.

To summarize all this, the KAMA is calculated as follows.
The input parameters are:

- $\ell_{er}$ - the efficiency ratio period.
The default value is $10$ samples.

- $\ell_{fast}$ - the fast EMA length.
The default value is $2$ samples.
Calculate the $\alpha_{fast}$ using (4).

- $\ell_{slow}$ - the slow EMA length.
The default value is $30$ samples.
Calculate the $\alpha_{fslow}$ using (4).

Given the next sample $x_k$ and the previous KAMA value
$kama_{k-1}$,

- calculate the efficiency ratio using (3),
- calculate the smoothing factor using (2),
- calculate the KAMA using (1).

The KAMA indicator is not primed during the first $\ell_{er}$ samples.
You can see the full implementation of the KAMA in
[Golang](https://github.com/ishiyan/Mbg/blob/main/trading/indicators/kaufman/adaptivemovingaverage.go)
and in
[Typescript](https://github.com/ishiyan/Mbng/blob/main/projects/mb/src/lib/trading/indicators/perry-kaufman/kaufman-adaptive-moving-average/kaufman-adaptive-moving-average.ts)
on Github.

From the digital signal processing (DSP) point of view, the KAMA is an infinite impulse response (IIR)
filter since all previous input samples contribute to its value.

## Changing parameters

The influence of the parameters on the smoothiness and the responsiveness of the KAMA
is demonstrated on the three following figures.

The first figure shows different efficiency ratio lengths $\ell_{er}$ leaving
the fast and slow lengths to be default values.

The second figure shows different fast lengths $\ell_{fast}$ leaving
the efficiency ratio and slow lengths to be default values.

The third one shows different slow lengths $\ell_{slow}$ leaving
the fast and efficiency ratio lengths to be default values.

[Chart: OHLCV]
*Varying the efficiency ratio length.*

[Chart: OHLCV]
*Varying the fast length.*

[Chart: OHLCV]
*Varying the slow length.*

## Step response

It's easy to see that the step-up and step-down responses of the KAMA don't depend on
the efficiency ratio length $\ell_{er}$.

Indeed, when the step is outside of the window of samples,
all the samples have the same value.
The ER value is $0$, and, according to (2), the smoothing factor
$\alpha_k$ is equal to the $\alpha_{slow}$.
The value of the KAMA is a constant.

When the step is inside the window of samples, the numerator and the denominator
of the (3) are equal, and the ER value is $1$ (meaning the perfect "one-sample trend").
According to (2), the smoothing factor $\alpha_k$ is equal to
$\alpha_{fast}$ and doesn't depend on the $\ell_{er}$.

The first two figures below show that step responce doesn't depend on the
efficiency ratio length $\ell_{er}$.

The two next figures show the step responces of the KAMA with the different
$\ell_{fast}$.

The step responses are the same as the step responses of the EMA.

[Chart: OHLCV]
*Step-up response doesn't depend on $\ell_{er}$.*

[Chart: OHLCV]
*Step-down response doesn't depend on $\ell_{er}$.*

[Chart: OHLCV]
*Step-up response dependency on $\ell_{fast}$.*

[Chart: OHLCV]
*Step-down response dependency on $\ell_{fast}$.*

## Frequency response

The figures below show an amplitude and a phase lag of the unit sample response of the KAMA as a function
of a period of various signal frequencies.
We can't derive its frequency response analytically because of the adaptive nature of the KAMA.

A period is a duration of a cycle in samples.
The smallest possible period of a cycle is $2$ samples.
To understand this, imagine a cycle of a sinusoid which starts at zero, goes up and peaks at $1$,
continues down and bottoms at $-1$, and then returns back to zero.
We need at least two samples (peak and trough) to represent a cycle.
See more details in the [frequency response article](/frequency-response).

[Chart: (a) amplitudePct vs period]
[Chart: (b) phaseDegUnwrapped vs period]

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

The following charts show the frequency responses of the KAMA with different ER lengths
$\ell_{er}$.
The other indicator parameters are set to default values.

[Chart: (a) amplitudePct vs period]
[Chart: (b) amplitudePct vs frequency]
*An amplitude as a function of a period of a cycle (a) and as a function of a normalized frequency of a
cycle (b) for various KAMA ER lengths $\ell_{er}$.*

[Chart: (a) phaseDeg vs period]
[Chart: (b) phaseDeg vs frequency]
*A phase as a function of a period of a wave (a) and as a function of a normalized frequency of a wave for
various KAMA ER lengths $\ell_{er}$.*

The following charts show the frequency responses of the KAMA with different fast lengths
$\ell_{fast}$.
The other indicator parameters are set to default values.

[Chart: (a) amplitudePct vs period]
[Chart: (b) amplitudePct vs frequency]
*An amplitude as a function of a period of a cycle (a) and as a function of a normalized frequency of a
cycle (b) for various KAMA fast lengths $\ell_{fast}$.*

[Chart: (a) phaseDeg vs period]
[Chart: (b) phaseDeg vs frequency]
*A phase as a function of a period of a wave (a) and as a function of a normalized frequency of a wave for
various KAMA fast lengths $\ell_{fast}$.*

The following charts show the frequency responses of the KAMA with differetn slow lengths
$\ell_{slow}$.
The other indicator parameters are set to default values.
There is almost no difference between the frequency responses of the KAMA with slow lengths
$\ell_{slow} = 30$ and $\ell_{slow} = 300$.

[Chart: (a) amplitudePct vs period]
[Chart: (b) amplitudePct vs frequency]
*An amplitude as a function of a period of a cycle (a) and as a function of a normalized frequency of a
cycle (b) for various KAMA slow lengths $\ell_{slow}$.*

[Chart: (a) phaseDeg vs period]
[Chart: (b) phaseDeg vs frequency]
*A phase as a function of a period of a wave (a) and as a function of a normalized frequency of a wave for
various KAMA slow lengths $\ell_{slow}$.*

---

## References
Kaufman, Perry J. (1995).
*Smarter Trading: Improving Performance in Changing Markets*. (p. 252). United Kingdom: McGraw-Hill.
[google books](https://books.google.com/books?id=ndq_21wRJjEC)

Kaufman, Perry J. (2013).
*Trading Systems and Methods, + Website*. (5th ed., p. 1232). United Kingdom: Wiley.
[google books](https://books.google.com/books?id=4842MXNn5o4C)

Kaufman, Perry J. (2020).
Comparing Two Adaptive Trends. *Technical Analysis of Stocks & Commodities*, *38*(11), 20–24.
[traders.com](https://technical.traders.com/archive/volume-2014.asp?yr=2020#Nov)

Kaufman, Perry J. (2025).
*Perry Kaufman's Bio*. Retrieved March 10, 2025, from
[https://perrykaufman.com/bio/](https://perrykaufman.com/bio/)
