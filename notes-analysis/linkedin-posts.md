# LinkedIn Posts — Moving Average Series

13 posts, one per article, ordered by content calendar schedule.

---

## Post 1: EMA (Week 1, Tuesday)

I couldn't derive two of the equations in my own EMA post. I published it anyway, because the recursive structure matters more than my algebra.

The EMA is deceptively simple on the surface. One parameter, alpha, controls how aggressively it discounts the past.

Unlike the SMA, the EMA never throws away data. It uses every historical sample you've ever fed it, weighting each one exponentially less. That's what makes it an infinite impulse response filter — it has memory that stretches back to the first bar.

As alpha approaches zero, the line smooths out and the last sample barely registers. As alpha approaches one, the input passes straight through. Most traders set alpha via the "period" shorthand and never examine what that tradeoff actually looks like in the frequency domain.

The step response is clean: no overshoot on the way up, no undershoot on the way down. That's a property you lose the moment you start stacking EMAs, as we'll see later in this series.

The part I couldn't close was the simplified frequency response formula. Mak (2006, p.15) didn't have it either. Sometimes the honest move is to publish what you've verified and flag what you haven't.

Full essay: {ema-url}

#technicalanalysis #signalprocessing

---

## Post 2: SMA (Week 1, Thursday)

The SMA has a constant bar lag no matter the frequency of the input signal. I didn't expect that when I first worked through the math.

Most people learn the SMA as "add up the last L prices, divide by L." That's correct but incomplete.

The window of L samples moves forward one bar at a time — that's literally why it's called a moving average. From a signal processing perspective, it's a finite impulse response filter with all weight coefficients equal. Every bar in the window contributes exactly the same amount.

Before the window fills, the filter isn't primed. I use that term deliberately because the output isn't yet meaningful — you're averaging fewer samples than the filter was designed for.

The step response is linear and clean. No overshoot, no undershoot. But the lag is fixed at (L-1)/2 bars regardless of what frequency content is in the signal. That constant lag surprised me because most filters show frequency-dependent delay.

One other detail worth noting: the amplitude response shape differs for even and odd values of L. The math is worth seeing side by side if you've never compared them.

Full essay: {sma-url}

#technicalanalysis #signalprocessing

---

## Post 3: Frequency Response (Week 2, Tuesday)

Most trading indicators you use daily are discrete digital filters. You can characterize them fully by their impulse response.

That's not a metaphor. If your indicator is linear and time-invariant, superposition holds. Feed it a unit impulse, record the output, and you know how it will respond to any input. That one property lets you predict its behavior completely.

The impulse response tells you the filter's memory. The frequency response — derived via the Z-transform or the DTFT — tells you which frequencies the filter passes and which it suppresses. The amplitude response shows you how much signal gets through at each frequency. The phase response shows you how much each frequency component gets delayed.

This matters for trading because price data contains a mix of frequencies: long-term trends, medium-term swings, and short-term noise. Your moving average is making decisions about which of those to keep and which to discard. The frequency response makes those decisions visible.

Shannon's sampling theorem sets a hard ceiling on what any indicator can resolve. If you're sampling daily bars, no algorithm can detect cycles shorter than two bars. That's the Nyquist frequency, and it's a physical constraint, not an implementation limit.

Full essay: {frequency-response-url}

#signalprocessing #quantitativetrading

---

## Post 4: WMA (Week 2, Thursday)

The WMA places its center of gravity at one-third of the window, not one-half like the SMA. That's where the reduced lag comes from.

The idea is straightforward: weight recent bars more heavily than older ones, using linearly descending coefficients. The most recent bar gets weight L, the next gets L-1, and so on down to 1 for the oldest bar in the window.

Those triangle-shaped weights shift the center of gravity forward. The SMA's lag is (L-1)/2 bars. The WMA's lag is L/3 bars. For a 30-bar window, that's 10 bars of lag instead of 14.5. Not a radical change, but not trivial either.

The step response stays clean — no overshoot, no undershoot. You get the lag reduction without paying for it in ringing or oscillation. That's a tradeoff profile that the multi-stage smoothers in this series won't be able to match.

From a filter design standpoint, the WMA is still a finite impulse response filter. It only looks at the last L bars. The linearly descending coefficients are a simple change over equal weights, but the lag reduction is real and measurable.

Full essay: {wma-url}

#technicalanalysis

---

## Post 5: DEMA (Week 3, Tuesday)

I couldn't find a single photo of Patrick Mulloy. I also couldn't follow his nonrigorous derivation of DEMA. I wrote up both problems.

Mulloy published DEMA in 1994 in Technical Analysis of Stocks & Commodities. He wanted an indicator faster than the EMA with reduced lag. Details about his personal life remain largely unknown — his two journal articles are essentially the only record.

The technique he used is well-known in DSP: it's called "twicing" (Oppenheim et al., 2009, p.609). You apply an EMA, then apply a second EMA to the output, and combine them to subtract out the lag. Mulloy never mentions the twicing technique by name in his articles. I found the DSP connection independently.

The cost of this lag reduction is overshoot. The step-up response overshoots and the step-down response undershoots. That's the fundamental tradeoff this entire series keeps circling back to: every technique that reduces lag below the EMA baseline introduces some form of ringing.

Mulloy's own derivation of why the formula works is what he calls "nonrigorous," and it seems unclear to me even after working through it multiple times. The twicing framework from DSP explains the same result more cleanly.

Full essay: {dema-url}

#signalprocessing #technicalanalysis

---

## Post 6: TEMA (Week 3, Thursday)

The formula for TEMA is defined without any derivation. Three cascaded EMAs, more lag reduction than DEMA, and more overshoot. That's the entire disclosure.

Mulloy introduced TEMA in the same 1994 journal issue as DEMA. The structure is an extension of the double-smoothing technique: the first EMA smooths the price, the second EMA smooths the first EMA, and the third EMA smooths the second EMA.

No derivation is provided for why this particular combination of three EMA outputs works. Mulloy states the formula and moves on. Coming after the DEMA post, this pattern should look familiar — each additional EMA cascade buys you lag reduction and costs you overshoot.

TEMA is an IIR filter because it's built on EMAs, which carry exponentially decreasing memory. The step response confirms the tradeoff: overshoot on the way up, undershoot on the way down, both more pronounced than DEMA.

If you've been following this series, the pattern is now clear. The foundations cluster showed how to measure lag and frequency response. The DEMA post showed the twicing mechanism. TEMA pushes the same mechanism one step further and pays a proportional price.

Full essay: {tema-url}

#technicalanalysis

---

## Post 7: T3EMA (Week 4, Tuesday)

T3 runs your data through six cascaded EMAs. A single volume factor lets you trade lag for overshoot continuously. It needs 6L-5 data points before it's primed.

Tim Tillson defined T3 as a generalized DEMA of a generalized DEMA of a generalized DEMA. That sounds recursive, and it is — three nested passes, each one a pair of cascaded EMAs. Six filters total.

The volume factor is what makes T3 interesting for practitioners. Instead of hardcoding the lag-overshoot tradeoff like DEMA and TEMA do, T3 gives you a continuous knob. Increase the volume factor: less lag, more overshoot. Decrease it: more lag, cleaner step response. Most indicators force you to accept a fixed point on that curve.

Increasing the length increases both the lag and the overshoot. That's expected, but it's worth verifying empirically because the interaction between length and volume factor isn't always intuitive.

The priming requirement is steep — 6L-5 bars before the filter output is meaningful. For a 10-bar T3, that's 55 bars of warm-up. Something to account for in backtesting.

Full essay: {t3ema-url}

#technicalanalysis #signalprocessing

---

## Post 8: T2EMA (Week 4, Thursday)

Tillson spent 15 years trading privately while managing projects at HP. His trick: "turn down the volume" of the derivative. Most people leave it at full blast.

Tim Tillson had degrees in Mathematics and Computer Science from Stanford. His insight was that DEMA overshoots because its derivative term runs at full strength. An EMA lags because it has no derivative correction at all. What if you could dial between the two?

That's what the volume factor does. Set v=1, you get DEMA. Set v=0, you get a plain EMA. Tillson suggested v=0.7 as the default — enough derivative to cut lag meaningfully, not so much that the step response rings.

He called this the "generalized DEMA" or GD. It's a simple idea with a lot of practical value. Instead of choosing between two fixed filters, you get a parametric family that spans the space between them.

The lag-overshoot tradeoff is the same one running through this entire series, but T2 makes it explicit and tunable. That's the contribution. Not a new filter topology, just an honest acknowledgment that practitioners need a knob, not a switch.

Full essay: {t2ema-url}

#technicalanalysis

---

## Post 9: TRIMA (Week 5, Tuesday)

I couldn't derive the TRIMA equation algebraically. I tried. The indicator itself is just an SMA of an SMA — triangle-shaped weights, no overshoot. But the closed form beat me.

The concept behind TRIMA is straightforward: run an SMA, then run another SMA on top of the output. The convolution of two rectangular windows produces a triangle. That's where the triangle-shaped weight coefficients come from — they fall out naturally from stacking two simple averages.

The weight values form two arithmetic progressions, one ascending and one descending, meeting at the center of the window. The center gets the most weight, the edges get the least. This is a finite impulse response filter, like the SMA and WMA before it.

The step response is clean — no overshoot, no undershoot. That puts TRIMA in the same category as SMA and WMA: filters that trade lag for smoothness without introducing ringing. The lag is higher than a single SMA of the same length, which is the price of the additional smoothing pass.

The closed-form equation for the weights is where I got stuck. I illustrated it working numerically but couldn't close the algebraic proof. Sometimes that's where you end up.

Full essay: {trima-url}

#technicalanalysis

---

## Post 10: KAMA (Week 5, Thursday)

Perry Kaufman built navigation systems for the Gemini space program before he ever touched a price chart. His adaptive moving average borrows the same idea: measure how efficient the path is.

KAMA is an EMA with a variable smoothing factor. The smoothing factor changes with each new bar, bounded between a fast and a slow limit. What drives the adaptation is the efficiency ratio: displacement over path length. The same concept you'd use to evaluate how directly a spacecraft moved from point A to point B.

When the efficiency ratio is close to 1, the market is trending cleanly — large displacement, short path. KAMA speeds up and follows closely. When the ratio drops toward zero, the market is chopping — small net displacement, long path. KAMA slows down and filters aggressively.

Because the smoothing factor is data-dependent, you can't derive KAMA's frequency response analytically. That's a real limitation if you're used to characterizing filters in the frequency domain. You can estimate it empirically, but the closed-form tools from the frequency response post don't apply here.

One surprising finding: there's almost no difference between KAMA's frequency response with slow lengths of 30 and 300. The fast length matters far more in practice.

Full essay: {kama-url}

#quantitativetrading #signalprocessing

---

## Post 11: FRAMA (Week 6, Tuesday)

Ehlers justified FRAMA's exponential mapping with a log-normal argument. It's not entirely accurate. But it works as a practical simplification, and I've made my peace with that.

FRAMA is an EMA with a variable smoothing factor driven by fractal dimension. The idea is clean: estimate the fractal dimension of the price series over a moving window, then map that dimension to the EMA's alpha parameter.

When fractal dimension hits 1, indicating a smooth trend, alpha goes to 1 and FRAMA follows price exactly. When dimension hits 2, indicating maximum roughness, alpha drops to its slowest setting and the filter clamps down.

The mapping between dimension and alpha is where things get hand-wavy. Ehlers (2005, p.82) argues: "Since the prices are log-normal, it seems reasonable to use an exponential function to relate the fractal dimension to alpha." Prices follow a log-normal distribution only if returns are normally distributed. That's an approximation, not a fact. But the exponential mapping produces a filter that works well in practice, so the shaky theoretical foundation hasn't stopped adoption.

The frequency response is nearly transparent — the amplitude stays close to 100% and the phase lag stays near zero. The delta signal passes through FRAMA with almost no change, which means its behavior is dominated by the adaptive mechanism rather than any fixed frequency characteristic.

Full essay: {frama-url}

#signalprocessing

---

## Post 12: JMA (Week 6, Thursday)

Jurik's algorithm was closed-source, shipped as a binary DLL. The trading community only learned how it worked because someone decompiled the Delphi code.

Jurik Research was founded in 1988, applying signal processing techniques originally intended for military projects to financial markets. The company sold a suite of proprietary indicators in binary form — you could use them, but you couldn't see how they worked.

JMA had four design goals: minimize lag, minimize overshoot, minimize undershoot (quick convergence after gaps), and maximize smoothness. It moves beyond traditional linear filtering, using techniques described as adaptive nonlinear filtering and information theory, "analogous to military technology for tracking moving targets through noise."

In the early 2000s, trading forums lit up with reverse engineering attempts. The breakthrough came when someone decompiled the original Jurik DLL for Wealth-Lab from Delphi (Pascal). That decompiled code became the root source of JMA implementations in MQL4, EasyLanguage, and other trading platforms.

Whether the open-source implementations match the original is a separate question. But the design goals — balancing lag, overshoot, undershoot, and smoothness simultaneously — represent the most ambitious target specification of any filter in this series.

Full essay: {jma-url}

#quantitativetrading #signalprocessing

---

## Post 13: HTCE (Week 7, Tuesday)

I don't fully understand what Ehlers' homodyne discriminator does. But it's mathematically elegant, numerically stable, and it measures cycles better than the alternatives I've tested.

The Hilbert Transform produces a version of a signal with all frequency components phase-shifted by -90 degrees. From this, you can construct an analytic signal and extract instantaneous phase, amplitude, and frequency. For trading, that means estimating the dominant cycle period in real time.

The problem is that the ideal Hilbert Transform is non-causal and infinite in duration. Obviously useless for live trading. Ehlers approximates it with a 7-tap FIR filter — a fixed set of coefficients applied to the most recent bars.

Ehlers presents three methods for extracting the cycle period from the analytic signal: the homodyne discriminator, the dual differential discriminator, and the phase accumulator. He favors the homodyne discriminator, calling it "mathematically elegant, numerically stable, and well-suited to financial data."

I'll admit I don't fully understand the derivation of the homodyne discriminator. It looks like standard practice in DSP — computing phase differences between consecutive bars and combining multiple lags to average out noise and phase quantization error. The result is less sensitive to spikes than raw single-point phase differences.

This is the most theoretically dense topic in the series. The gap between the ideal Hilbert Transform and Ehlers' practical approximation is where all the interesting engineering decisions live.

Full essay: {htce-url}

#signalprocessing #quantitativetrading
