# FRAMA

[Chart: OHLCV]

The FRAMA lines are plotted in the price pane.
The lower pane shows the related box-counting fractal dimension $\mathcal{D}$.

## How it works

The Fractal Adaptive Moving Average (FRAMA) was introduced by John F. Ehlers
in his article in *Technical Analysis of Stocks & Commodities* (Ehlers 2005),
later made freely available on his MESA website (Ehlers 2006).

FRAMA is essentially an
[Exponential Moving Average](/emaNote.route) (EMA)
with a variable smoothing factor $\alpha_k$
that changes with each new sample, within minimum and maximum boundaries.

$$\tag*{(1)}frama_{k}=\alpha_{k} x_{k} + (1-\alpha_{k})frama_{k-1}$$

where $\alpha_{k}\in\left[\alpha_{slow}, 1\right]$.
Here $\alpha_{slow}$, the slowest smoothing factor,
is the parameter of the indicator.

The concept behind FRAMA is to link the fractal dimension $\mathcal{D}_{k}$,
calculated over a moving window of $\ell$ samples, to the EMA’s smoothing factor
$\alpha_k$, thus making the EMA adaptive.

$$\tag*{(2)}\alpha_k=e^{\omega(\mathcal{D}_{k}-1)}, \mathcal{D}_{k}\in\left[1,2\right]$$

where

$$\tag*{(3)}\omega=\ln(\alpha_{slow}), \alpha_{slow}\in\left(0,1\right)$$

The default value of $\alpha_{slow}$, recommended by Ehlers, is
$0.01$. This corresponds to an equivalent smoothing length of
$\ell_{slow} = \frac{2}{\alpha_{slow}}-1=199$ samples.

![](assets/ehlers1.jpg)

John F. Ehlers (1933 -).
President at [MESA Software](https://mesasoftware.com/);
chief scientist at [StockSpotter](https://stockspotter.co);
contributing editor for [S&C Magazine](https://traders.com/).

To explain why he adopted the exponential function in (2), Ehlers comments:

"Since the prices are log-normal, it seems reasonable to use an exponential function
to relate the fractal dimension to alpha." (Ehlers 2005, p82)

This isn't entirely accurate (prices follow a log-normal distribution only if returns are normally
distributed), but it serves as a practical simplification.

Equation (2) maps the bounded range $\mathcal{D}_{k}\in\left[1,2\right]$
to a convenient and interpretable range $\alpha_{k}\in\left[\alpha{slow},1\right]$.

When $\mathcal{D}_{k}=1$, indicating a smooth trending market,
$\alpha_{k}=\exp(0)=1$, so the filter responds instantly, making the
FRAMA follow price exactly: $frama_{k}=x_{k}$.

When $\mathcal{D}_{k}=2$, indicating extremely noisy market behavior,
$\alpha_{k}=\exp(\omega)=\alpha_{slow}$, so the FRAMA is very slow,
filtering out noise: $frama_{k}=\alpha_{slow} x_{k} + (1-\alpha_{slow})frama_{k-1}$.

The fractal dimension is estimated using a “box counting” method (Falconer 2014, chapter 2).
This box-counting dimension is sometimes called the Minkowski–Bouligand dimension, named after
Hermann Minkowski and Georges Bouligand.

One could measure the fractal dimension of prices by covering the "price curve" with a series of
small boxes, but that is tedious.
Because price samples are typically uniformly spaced, the box count can be approximated
by the average slope of the "price curve": the highest price minus the lowest price within
an interval, divided by that interval’s length.
The equation for the box count (approximated by slope) is:

$$\tag*{}N=\frac{\max(P_{1...\ell}) - \min(P_{1...\ell})}{\ell}$$

where $P_{1...\ell}$ are the $\ell$ prices in the interval.

![](assets/frama-box-counting.svg)
*Three box-counting intervals used to calculate the fractal dimension.*

We split the lookback window of length $\ell$ into two half-intervals
of length $\ell/2$ and calculate two box counts $N_{1}$
and $N_{2}$. Then we calculate the box count $N_{3}$
for the full interval $\ell$ and calculate the fractal box-counting
dimension $\mathcal{D}$.

$$\tag*{}N_{1}=\frac{\max(P_{1...\ell/2}) - \min(P_{1...\ell/2})}{\ell/2}$$

$$\tag*{}N_{2}=\frac{\max(P_{\ell/2+1...\ell}) - \min(P_{\ell/2+1...\ell})}{\ell/2}$$

$$\tag*{}N_{3}=\frac{\max(P_{1...\ell}) - \min(P_{1...\ell})}{\ell}$$

$$\tag*{}\mathcal{D}=\frac{\log(N_{1}+N_{2})-\log N_{3}}{\log 2}$$

How did Ehlers arrive at this formula for $\mathcal{D}$?

According to (Falconer 2014, p. 28, equation 2.4), the box counting dimension

$$\tag*{}\mathcal{D}_{P}=\lim_{\delta\to 0}\frac{\log N_{\delta}(P)}{-\log(\delta)}$$

where $N_{\delta}(P)$ is the number of boxes of size $\delta$
covering the price set $P$. As $\delta\to 0$,
we zoom in infinitely, revealing more details of a true fractal (like the Mandelbrot set,
the coastline of Britain, etc.), which exhibits self-similarity at all scales.

In parctice, we only have finite-resolution data (a fixed-length lookback window in this case),
so we can't directly use the limit formula above. Instead, we approximate it discretely with
two scales: the full window range $\ell$ and half window range
$\ell/2$. Hence,

$$\tag*{}\mathcal{D}\approx\frac{\log N\left(\ell/2\right) - \log
N\left(\ell\right)}{\log\left(1/\left(\ell/2\right)\right) - \log\left(1/\ell\right)}$$

$$\tag*{}=\frac{\log\left(\frac{N\left(\ell/2\right)}{N\left(\ell\right)}\right)}{\log\left(2/\ell\right)
- \log\left(1/\ell\right)}$$

$$\tag*{}=\frac{\log\left(\frac{N\left(\ell/2\right)}{N\left(\ell\right)}\right)}{\log 2}$$

Recalling that $N\left(\ell/2\right) = N_{1}+N_{2}$ and
$N\left(\ell\right) = N_{3}$, we get

$$\tag*{}\mathcal{D}\approx\frac{\log\left(\frac{N_{1}+N_{2}}{N_{3}}\right)}{\log 2}$$

$$\tag*{(4)}=\frac{\log\left(N_{1}+N_{2}\right)-\log N_{3}}{\log 2}$$

which is the formula Ehlers uses for the box-counting fractal dimension $\mathcal{D}$.

To summarize, FRAMA is computed as follows.
The input parameters are:

- $\ell$ - the box-counting fractal dimension period;
it should be an even integre, as we split it into two half-intervals.
The default value is $16$ samples.

- $\alpha_{slow}$ - the lower bound on
$\alpha$, calculated from the box-counting fractal dimension.
The default Ehlers value is $0.01$, corresponding to
an equivalent smoothing length of
$\ell_{slow} = \frac{2}{\alpha_{slow}}-1=199$ samples.

Given the next sample $x_k$ and the previous FRAMA value
$frama_{k-1}$,

- Update the lookback window of $\ell$ samples.
- Compute the box-counting fractal dimension $\mathcal{D}$ using (4),
- Compute the smoothing factor $\alpha$ using (3) and (2),
- Compute the next $frama_{k}$ value using (1).

The FRAMA indicator is not primed during the first $\ell-1$ samples.
You can find its full implementation in
[Golang](https://github.com/ishiyan/Mbg/blob/main/trading/indicators/ehlers/fractaladaptivemovingaverage.go)
and in
[Typescript](https://github.com/ishiyan/Mbng/blob/main/projects/mb/src/lib/trading/indicators/john-ehlers/fractal-adaptive-moving-average/fractal-adaptive-moving-average.ts)
on Github.

From the digital signal processing (DSP) point of view, the FRAMA is an Infinite Impulse Response (IIR)
filter, since all past input samples continue to affect its output.

## Changing parameters

The influence of the parameters on the smoothness and responsiveness of the FRAMA
is demonstrated in the following two figures.

The first figure shows different window lengths $\ell$ while keeping
the slowest smoothing factor at its default value. You can see that larger window lengths
produce more consistent fractal dimension lines compared to the jagged, less correlated lines
that result from shorter windows in the top figure.

The second figure shows different slowest smoothing factors $\alpha_{slowest}$,
with the window length set to its default value. Because the box-counting fractal dimension
$\mathcal{D}$ doesn't depend on $\alpha_{slowest}$,
all three lines in the lower pane coincide. Increasing $\alpha_{slowest}$
makes the FRAMA line more responsive but less adaptive to price volatility.

[Chart: OHLCV]
*Varying the window length $\ell$.*
The lower pane shows the box-counting fractal dimension $\mathcal{D}$.

[Chart: OHLCV]
*Varying the slowest smoothing factor $\alpha_{slowest}$.*
The lower pane shows the box-counting fractal dimension $\mathcal{D}$.

## Step response

The first two figures show the step responces of the FRAMA with the different
lengths $\ell$. We can see that the responses almost overlap,
showing no dependency on $\ell$.

This is intuitively clear because the generated step data uses constant (not random) high
and low price bar values, so the price ranges in all three box-counting intervals remain the same.
This causes the "price curve" to appear to fill the entire area between the high and low boundaries,
resulting in a fractal dimension value of 2. When the window enters the price step, the distance
between the high and low boundaries increases substantially, yielding a fractal dimension value of 1.
When the window exits the step, the situation reverts and the fractal dimension becomes 2 again.

The following two figures show the step responses of the FRAMA with different
slowest smoothing factors $\alpha_{slowest}$.

Because $\alpha_{slowest}$ truncates the EMA's smoothing factor
at its slower (lowest) range, the step response is more reactive to step data
when $\alpha_{slowest}$ is smaller, mirroring the EMA's behavior.

[Chart: OHLCV]
*Step-up response shows almost no dependency on $\ell$.*

[Chart: OHLCV]
*Step-down response shows almost no dependency on $\ell$.*

[Chart: OHLCV]
*Step-up response dependency on $\alpha_{slowest}$.*

[Chart: OHLCV]
*Step-down response dependency on $\alpha_{slowest}$.*

## Frequency response

The figures below show the amplitude and a phase lag of the FRAMA’s unit sample response as a function
of a period of various signal frequencies. Because FRAMA is essentially an exponential moving average,
its amplitude and phase lag closely resemble those of an [EMA](/emaNote.route).

A period is the duration of one cycle, measured in samples.
The smallest possible period for a cycle is $2$ samples.
To illustrate this, consider a sinusoid that starts at zero, rises to a peak of $1$,
then descends to a trough of $-1$, and finally returns to zero.
At least two samples (peak and trough) are required to capture one complete cycle.
For more details, see .

We can see from both charts that the amplitude and phase lag remain nearly constant at
$100$% and $0$ degrees, indicating that the
$\delta$-signal passes through the FRAMA with almost no change.
At the smallest period of $2$ samples, the amplitude is only
$99.97$%, and at the period of $4$ samples,
the phase lag is only $0.07$ degrees.

[Chart: (a) amplitudePct vs period]
[Chart: (b) phaseDegUnwrapped vs period]

*An amplitude (a) and a phase lag (b) as a function of a period of a cycle.*

The same charts can also be viewed as functions of a cycle’s frequency.
A period ($\tau$) is the reciprocal of the cycle’s frequency
($\nu$): $\tau = \frac{1}{\nu}$.
The smallest period $\tau = 2$ corresponds to the Nyquist frequency
$\nu = \frac{1}{\tau} = \frac{1}{2}$, which is the highest frequency
possible in a signal. Below, we use the normalized frequency, where a value of $1$
represents the Nyquist frequency.
Thus, $0$ corresponds to an infinite $\tau$,
while $1$ corresponds to the $\tau = 2$.

The following charts illustrate the frequency responses of the FRAMA with different
window lengths $\ell$.
The slowest smoothing factor parameter $\alpha_{slow}$ remains
at its default value.

We can see that the amplitude and phase lag don't depend on the window length
parameter $\ell$. This is intuitively clear because of the
way we approximate the discrete $\delta$-signal data.

Indeed, before the $\delta$-spike, we feed FRAMA zero sample values
together with a small constant value as the “highest” sample value and zero as the
“lowest” sample value. This makes FRAMA assume the “price curve” fills the entire area
between the highest and lowest horizontal sample lines. Hence, the box-counting fractal
dimension $\mathcal{D}=2$ and $\alpha=\alpha_{slow}$.
Because the sample values are zero, the FRAMA output is also zero.

The $\delta$-spike is represented by a single high-value sample,
for example, 1000. This sets the distance between the highest and lowest sample lines
to the spike’s value. The box-counting fractal dimension $\mathcal{D}=1$
and $\alpha=1$, so FRAMA outputs the spike’s value.

Next, we feed FRAMA the same zero samples as before.
Until the $\delta$-sample leaves the moving average window,
$\mathcal{D}=1$ and $\alpha=1$, causing FRAMA
to output zero for the current sample. Once the $\delta$-sample is
out of the window, we revert to the initial scenario:
$\mathcal{D}=2$, $\alpha=\alpha_{slow}$,
and FRAMA again outputs zero.

[Chart: (a) amplitudePct vs period]
[Chart: (b) amplitudePct vs frequency]
*An amplitude as a function of a period of a cycle (a) and as a function of a normalized frequency of a
cycle (b) for various FRAMA lengths $\ell$.*

[Chart: (a) phaseDeg vs period]
[Chart: (b) phaseDeg vs frequency]
*A phase as a function of a period of a wave (a) and as a function of a normalized frequency of a wave for
various FRAMA lengths $\ell$.*

The following charts show the frequency responses of the FRAMA with different
slowest smoothing factors $\alpha_{slow}$.
The length parameter $\ell$ remains at its default value.

There is a subtle difference in amplitude and phase lag among FRAMA versions
with different $\alpha_{slow}$.
These charts align with those of an [EMA](/emaNote.route).

[Chart: (a) amplitudePct vs period]
[Chart: (b) amplitudePct vs frequency]
*An amplitude as a function of a period of a cycle (a) and as a function of a normalized frequency of a
cycle (b) for various FRAMA slowest smoothing factors $\alpha_{slow}$.*

[Chart: (a) phaseDeg vs period]
[Chart: (b) phaseDeg vs frequency]
*A phase as a function of a period of a wave (a) and as a function of a normalized frequency of a wave for
various FRAMA slowest smoothing factors $\alpha_{slow}$.*

---

## References
Falconer, Kenneth (2014).
*Fractal Geometry: Mathematical Foundations and Applications*. (3 ed, p. 400). Wiley.
[google books](https://books.google.com/books?id=CaSsAQAAQBAJ)

Ehlers, John F (2005).
Fractal Adaptive Moving Average. *Technical Analysis of Stocks & Commodities*, *23*(10), 81–82.
[traders.com](https://technical.traders.com/archive/volume-2014.asp?yr=2005#Oct)

Ehlers, John F (2006).
*FRAMA – Fractal Adaptive Moving Average*. Retrieved April 10, 2025, from
[mesasoftware.com/papers/FRAMA.pdf](https://www.mesasoftware.com/papers/FRAMA.pdf)

MESA Software (2025).
*Left-brained concepts for traders in theit right minds*. Retrieved April 10, 2025, from
[mesasoftware.com](https://mesasoftware.com/)

Stockspotter (2025).
*Cycle, Vigor & Trend*. Retrieved April 10, 2025, from
[stockspotter.co](https://stockspotter.co)

Ehlers, John F.
*Professional profile | linkedin*, accessed April 10 2025.
[linkedin.com](https://www.linkedin.com/in/john-ehlers-8017874/)
