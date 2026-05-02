# Boilerplate Passages Found Across testdata/ Articles

Extracted verbatim (or near-verbatim) recurring text blocks.

---

## 1. Nyquist / Period Explanation (appears in 10+ articles)

> A period is a duration of a cycle in samples.
> The smallest possible period of a cycle is $2$ samples.
> To understand this, imagine a cycle of a sinusoid which starts at zero, goes up and peaks at $1$,
> continues down and bottoms at $-1$, and then returns back to zero.
> We need at least two samples (peak and through) to represent a cycle.
> See more details in the .

Found in: SMA (53-61), EMA (100-105), DEMA (106-111), TEMA (66-70), WMA (110-116), KAMA (170-175), TRIMA (125-130), JMA (138-142), FRAMA (223-228), T2EMA (131-136), T3EMA (97-101).

---

## 2. Normalized Frequency Explanation (appears in 8+ articles)

> The same charts can be represented as a function of a cycle's frequency.
> A period ($\tau$) is an inverse of the cycle's frequency
> ($\nu$): $\tau = \frac{1}{\nu}$.
> The smallest period $\tau = 2$ corresponds to the Nyquist frequency
> $\nu = \frac{1}{\tau} = \frac{1}{2}$which is the highest frequency
> possible in a signal. Below we use the normalized frequency which has the value of $1$
> at the Nyquist frequency.
> That is, $0$ corresponds to the infinite $\tau$,
> $1$ corresponds to the $\tau = 2$.

Found in: SMA (63-71), EMA (112-120), DEMA (118-126), WMA (123-131), KAMA (182-190), TRIMA (132-140), JMA (149-157), FRAMA (242-250).

---

## 3. Step Response Introduction (appears in 8+ articles)

Variant A (no overshoot):

> Two figures below demonstrate the response to the step-up and step-down data.
> The transition is clearly not linear.
> Both responses touch the step data with the lag equal to the length $L$ of the filter.
> The step-up response doesn't overshoot and the step-down response doesn't undershoot the data.

Found in: WMA (95-98), TRIMA (109-112).

Variant B (SMA-specific, linear transition):

> Two figures below demonstrate the response of an SMA to the step-up and step-down data.
> The step transition is clearly linear.
> Both responses touch the step data with the lag equal to the length $L$ of the filter.
> The step-up response doesn't overshoot and the step-down response doesn't undershoot the data.

Found in: SMA (40-43).

Variant C (with overshoot):

> Two figures below demonstrate the response to the step-up and step-down data.
> The transition is clearly not linear.
> The step-up response overshoots and the step-down response undershoots the data.

Found in: DEMA (91-93).

Variant D (minimal):

> Two figures below demonstrate the response to the step-up and step-down data.
> The transition is clearly not linear.
> The step-up response doesn't overshoot and the step-down response doesn't undershoot the data.

Found in: EMA (38-40).

Variant E (T2/T3):

> Two figures below demonstrate the response to the step-up and step-down data
> for indicators shown above.
> The transition is clearly not linear and you can see the overshooting and undershooting.

Found in: T2EMA (98-100), T3EMA (69-71).

Variant F (TEMA, with typo):

> Two figures below demonstrate the response to the step-up and step-down data.
> The transition is clearly not linear.
> The step-up response overshoot and the step-down response undershoot the data.

Found in: TEMA (51-53).

---

## 4. Frequency Response Section Header + Intro (appears in 8+ articles)

> The figure below shows an amplitude and a phase lag of the unit sample response of a [INDICATOR] as a function
> of a period of various signal frequencies.

Found in: DEMA (103-104), TEMA (63-64), T2EMA (129-130), T3EMA (94-95).

Variant with "figures" (plural):

> The figures below show an amplitude and a phase lag of the unit sample response of the [INDICATOR] as a function
> of a period of various signal frequencies.

Found in: SMA (52-54), EMA (97-98), WMA (108-109), KAMA (166-167), TRIMA (122-123), JMA (133-134), FRAMA (219-220).

---

## 5. DSP Classification (appears in all articles)

FIR variant:

> From the digital signal processing (DSP) point of view, [INDICATOR] is a
> finite impulse response (FIR) filter with [description of weight coefficients].
> The filter is finite because only a finite number of $L$ last samples contribute to its value.

Found in: SMA (31-36), WMA (85-91), TRIMA (83-84).

IIR variant:

> From the digital signal processing (DSP) point of view, [INDICATOR] is a
> infinite impulse response (IIR) filter because it is based on EMA which
> applies exponentially decresing weighting factors to the input samples.

Found in: DEMA (85-87), TEMA (46-48), T2EMA (93-95), T3EMA (64-66).

IIR variant (adaptive indicators):

> From the digital signal processing (DSP) point of view, the [INDICATOR] is an infinite impulse response (IIR)
> filter since all previous input samples contribute to its value.

Found in: KAMA (102-103), JMA (76-77), FRAMA (158-159).

---

## 6. EMA Initialization Boilerplate (appears in 5 articles)

> The initial EMA values may be calculated in two different ways.
> The first approach is to use the [Simple Moving Average](/smaNote.route)
> over the first
>
> $$L=\frac{2}{\alpha}-1$$
>
> input data values, where $L$ is an equivalent length of the
> smoothing parameter $\alpha$. We assume $\alpha$
> to be the same for [both/all] EMAs. This approach is the most widely used one.
>
> The second way to initialize the EMAs is to use the first input data point as the initial value.
> It is used by the well known (in the past) Metastock tradind software.

Found in: DEMA (69-80), TEMA (30-41), T2EMA (77-88), T3EMA (48-59).

---

## 7. Priming Statement (appears in all articles)

> [INDICATOR] is not primed during the first $[expression]$ updates.

Variants:
- "the SMA is not primed during the first $L-1$ updates" — SMA
- "The WMA is not primed during the first $L-1$ updates" — WMA
- "we always consider DEMA to be primed only after the $2L-1$ input data points" — DEMA
- "we always consider DEMA to be primed only after the $3L-1$ input data points" — TEMA (note: says "DEMA" but means "TEMA")
- "we always consider T2 to be primed only after the $4L-3$ input data points" — T2EMA
- "we always consider T3 to be primed only after the $6L-5$ input data points" — T3EMA
- "The KAMA indicator is not primed during the first $\ell_{er}$ samples" — KAMA
- "the JMA indicator is not primed during the first $30$ samples" — JMA
- "The FRAMA indicator is not primed during the first $\ell-1$ samples" — FRAMA

---

## 8. Geometric Series Derivation (appears in 3 articles)

> To simplify [...], we can recall a sum of geometric series
>
> $$\sum_{k=0}^{L-1}x^k = 1 + x + x^2 + \ldots + x^{L-1}$$
>
> Multiplying both sides by $1-x$ and simplifying
>
> $$(1-x)\sum_{k=0}^{L-1}x^k = (1-x)(1 + x + x^2 + \ldots + x^{L-1})$$
>
> $$= 1 + x + x^2 + \ldots + x^{L-1} - x - x^2 - \ldots - x^{L-1} - x^L$$
>
> $$= 1 - x^L$$
>
> gives a well-known identity for a sum of geometric series (Jeffrey and Dai, 2008, 1.2.2.2):
>
> $$\sum_{k=0}^{L-1}x^k = \begin{cases} (1-x^L)/(1-x), & x \neq 1 \\ L, & x = 1 \end{cases}$$

Found in: SMA (136-150), EMA (59-73).

---

## 9. Chart Caption Patterns (appears throughout)

> *An amplitude (a) and a phase lag (b) as a function of a period of a cycle.*

> *An amplitude as a function of a period of a cycle (a) and as a function of a normalized frequency of a cycle (b) for various [INDICATOR] lengths.*

> *A phase as a function of a period of a wave (a) and as a function of a normalized frequency of a wave for various [INDICATOR] lengths.*

> *Step-up response.*

> *Step-down response.*

Found in: all articles with charts.

---

## 10. Reference Block — Oppenheim (appears in 7 articles)

> Oppenheim, A. V., Schafer, R. W., Yoder, M. A., & Padgett, W. T. (2009).
> *Discrete-time signal processing*. (3rd ed., p. 1120). Upper Saddle River, NJ: Pearson.
> [google books](https://books.google.com/books?id=EaMuAAAAQBAJ)

Found in: SMA, EMA, DEMA, T2EMA, frequency-response, HTCE.

---

## 11. Reference Block — Jeffrey and Dai (appears in 5 articles)

> Jeffrey, A., & Dai, H. H. (2008).
> *Handbook of mathematical formulas and integrals*. (4th ed., p. 592). San Diego, CA: Elsevier/Academic Press.
> [google books](https://books.google.com/books?id=JokQD5nK4LMC)

Found in: SMA, EMA, T2EMA, T3EMA, HTCE, frequency-response.

---

## 12. Reference Block — Mulloy (appears in 4 articles)

> Mulloy, P. G. (1994).
> Smoothing Data With Faster Moving Averages. *Technical Analysis of Stocks & Commodities*, *12*(1), 11–19.
> [traders.com](https://technical.traders.com/archive/volume-2014.asp?yr=1994#Jan)
>
> Mulloy, P. G. (1994).
> Smoothing Data With Less Lag. *Technical Analysis of Stocks & Commodities*, *12*(2), 72–80.
> [traders.com](https://technical.traders.com/archive/volume-2014.asp?yr=1994#Feb)

Found in: DEMA, TEMA, T2EMA.

---

## 13. Tillson Reference (appears in 3 articles)

> Tillson, Tim (1998).
> Smoothing Techniques For More Accurate Signals. *Technical Analysis of Stocks & Commodities*, *16*(1), 33–37.
> [traders.com](https://technical.traders.com/archive/volume-2014.asp?yr=1998#Jan)

Found in: DEMA, T2EMA, T3EMA.
