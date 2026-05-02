# JMA

[Chart: OHLCV]

The JMA lines are plotted in the price pane.

## How it works

The Jurik Moving Average (JMA) is one of the proprietary indicators developed by Mark Jurik
around 1998. His company
[Jurik Research](http://jurikres.com/)
was founded in 1988, applying signal processing techniques originally intended for military
projects to trading. Now, the company is "winded down" and doesn't sell its products anymore.

![](assets/jurik.png)

Mark Jurik.
[jurikres.com](http://jurikres.com/about/company.htm)

The suite of technical indicators sold by Jurik Research was closed source and distributed
in binary form. The only sources of information are several documents available on the
Jurik Research website.

JMA represents a step forward from classic moving averages
([SMA](/smaNote.route),
[EMA](/emaNote.route),
[WMA](/wmaNote.route)) and other
advanced or adaptive ones
([DEMA](/demaNote.route),
[TEMA](/temaNote.route),
[T2](/t2emaNote.route),
[T3](/t3emaNote.route),
[KAMA](/kamaNote.route)) available around the late 1990s
(jurik1999evolution).

It is designed (jurik1999why) as a low-lag, nonlinear adaptive filter to remove noise from
price series data while accurately tracking the underlying price movement and momentum.
The design aims for an optimal balance of four features: (1) minimum lag, (2) minimum overshoot,
(3) minimum undershoot, i.e. quick convergence after gaps, and (4) maximum smoothness.

Unlike non-adaptive filters, JMA can distinguish between random background noise and
significant price gaps. It filters out the noise but adapts quickly to gaps, converging
to new price levels faster than many other moving averages.

JMA moves beyond traditional linear, frequency-based filtering, using techniques,
described as adaptive non-linear filtering and information theory, "analogous to
military technology for tracking moving targets through noise" (jurik1999how).
It views the price series as a noisy image of the "true" underlying smooth price and
attempts to estimate that smooth price's location.

JMA has two parameters (jurik1999how) allowing to fine-tune JMA's responsiveness
for different strategies, such as optimizing crossover timing.

- $\ell$ - length of the JMA, which is a number of samples
used to calculate the JMA value. Typical values range from $2$
up to $80$. This parameter primarily controls the smoothness
and speed of the JMA line. Increasing the $\ell$ makes JMA
move slower, which increases smoothness and reduces noise, but at the cost of
adding more lag.

- $\phi$ - phase of the JMA. Ranges from $-100$
to $+100$. This parameter controls the
JMA's "inertia," essentially managing the trade-off between lag and overshoot.
The highest value $\phi=100$ (low interia) minimises
the lag (allowing the filter to turn more quickly), but maximizes the tendency
to overshoot price reversals.
The lowest value $\phi=-100$ (high interia) maximises
the lag (making the filter slower to change direction and reducing noise),
but minimises potential overshoot.
The default value $\phi=0$ provides a balance between
lag and overshoot.

Irrespective from the parameters values, the JMA indicator is not primed
during the first $30$ samples.

From the digital signal processing (DSP) point of view, the JMA is an infinite impulse response (IIR)
filter since all previous input samples contribute to its value.

In early 2000s, on the various trading forums there were numerous posts
about reverse engineering and re-implementing the JMA algorithm (
[mql5](https://www.mql5.com/en/forum/173010),
[regtrading](https://regtrading.com/the-real-jurik-moving-average-jma/),
[useThinkScript](https://usethinkscript.com/threads/jurik-moving-average.9817/),
[tradingView](https://www.tradingview.com/script/W1DjDb8h-Jurik-Moving-Average-JMA/),
[forexFactory](https://www.forexfactory.com/thread/696822-jurik-indicators)).
The decompiled Delphi (Pascal) code of the original Jurik's DLL for Wealth-Lab
became a root source of the JMA imitations in MQL4, EasyLang, and others.

This implementation is also based on the decompiled code
([tslab.com, 2010](https://forum.tslab.ru/ubb/ubbthreads.php?ubb=showflat&Number=5796#Post5796)).
You can see the full source in
[Golang](https://github.com/ishiyan/Mbg/blob/main/trading/indicators/jurik/movingaverage.go)
and in
[Typescript](https://github.com/ishiyan/Mbng/blob/main/projects/mb/src/lib/trading/indicators/mark-jurik/jurik-moving-average/jurik-moving-average.ts)
on Github.

## Changing parameters

The influence of the parameters on the smoothiness and the responsiveness of the JMA
is demonstrated on the two following figures.

The first figure shows different lengths $\ell$ leaving
the phase to be $0$.

The second figure shows different phases $\phi$ leaving
the length to be $10$.

[Chart: OHLCV]
*Varying the length.*

[Chart: OHLCV]
*Varying the phase.*

## Step response

The first two figures below show the step responces of the JMA with the different
length parameter $\ell$. Increasing the length increases the lag
and the overshoot, but gives the indicator line more "inertiia".

The two next figures show the step responces of the JMA with the different phase
parameter $\phi$. Increasing the phase increases the overshoot.

*Step-up response dependency on $\ell$.*

*Step-down response dependency on $\ell$.*

*Step-up response dependency on $\phi$.*

*Step-down response dependency on $\phi$.*

## Frequency response

The figures below show an amplitude and a phase lag of the unit sample response of the JMA as a function
of a period of various signal frequencies.
We can't derive its frequency response analytically because of the adaptive nature of the JMA.

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

The following charts show the frequency responses of the JMA with different lengths
$\ell$.
The phase parameter is set to default value.

[Chart: (a) amplitudePct vs period]
[Chart: (b) amplitudePct vs frequency]
*An amplitude as a function of a period of a cycle (a) and as a function of a normalized frequency of a
cycle (b) for various JMA lengths $\ell$.*

[Chart: (a) phaseDeg vs period]
[Chart: (b) phaseDeg vs frequency]
*A phase as a function of a period of a wave (a) and as a function of a normalized frequency of a wave for
various JMA lengths $\ell$.*

The following charts show the frequency responses of the JMA with different phases
$\phi$.
The length parameter are set to default value.

[Chart: (a) amplitudePct vs period]
[Chart: (b) amplitudePct vs frequency]
*An amplitude as a function of a period of a cycle (a) and as a function of a normalized frequency of a
cycle (b) for various JMA phases $\phi$.*

[Chart: (a) phaseDeg vs period]
[Chart: (b) phaseDeg vs frequency]
*A phase as a function of a period of a wave (a) and as a function of a normalized frequency of a wave for
various JMA phases $\phi$.*

---

## References
Mark Jurik (1999).
*Overview*. Retrieved April 12, 2025, from
[jurikres.com](http://jurikres.com/down__/overview.pdf)

Mark Jurik (1999).
*Evolution of Moving Averages*. Retrieved April 12, 2025, from
[jurikres.com](http://jurikres.com/down__/ma_evolv.pdf)

Mark Jurik (1999).
*Why Use JMA ?*. Retrieved April 12, 2025, from
[jurikres.com](http://jurikres.com/down__/why_jma.pdf)

Mark Jurik (1999).
*How it works*. Retrieved April 12, 2025, from
[jurikres.com](http://jurikres.com/faq1/faq_ama.htm#how_work)

MQL5 forum (2005).
*Jurik*. Retrieved April 12, 2025, from
[mql5.com](https://www.mql5.com/en/forum/173010)

MQL5 forum (2008).
*Good news - JMA's algorithm revealed!*. Retrieved April 12, 2025, from
[mql5.com](https://www.mql5.com/en/forum/179011)

MQL5 forum (2008).
*Good news for all of you - JMA's algorihm revealed! (pdf)*. Retrieved April 12, 2025, from
[mql5.com](https://c.mql5.com/forextsd/forum/164/jurik_1.pdf)

Regtrading (2018).
*The Real Jurik Moving Average (JMA)*. Retrieved April 12, 2025, from
[regtrading.com](https://regtrading.com/the-real-jurik-moving-average-jma/)

useThinkScript (2022).
*Jurik Moving Average ?*. Retrieved April 12, 2025, from
[usethinkscript.com](https://usethinkscript.com/threads/jurik-moving-average.9817/)

useThinkScript (2022).
*Jurik Moving Average Crossover For ThinkOrSwim*. Retrieved April 12, 2025, from
[usethinkscript.com](https://usethinkscript.com/threads/jurik-moving-average-crossover-for-thinkorswim.12527/)

TradingView (2025).
*Jurik Moving Average (JMA)*. Retrieved April 12, 2025, from
[tradingview.com](https://www.tradingview.com/script/W1DjDb8h-Jurik-Moving-Average-JMA/)

ForexFactory (2017).
*Jurik indicators*. Retrieved April 12, 2025, from
[forexfactory.com](https://www.forexfactory.com/thread/696822-jurik-indicators)

tsLab (2010).
*JMA code*. Retrieved April 12, 2025, from
[tslab.com](https://forum.tslab.ru/ubb/ubbthreads.php?ubb=showflat&Number=5796#Post5796)
