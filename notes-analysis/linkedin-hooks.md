# LinkedIn Hooks — All 13 Articles

Scored on Attention (1-5), Voice-fidelity (1-5), Truth-fidelity (1-5). Voice < 4 = discarded. Sorted by total score.

---

## 1. EMA

| # | Hook | Attn | Voice | Truth | Total |
|---|------|------|-------|-------|-------|
| 1 | I could not derive two of the equations in my own EMA post. Publishing it anyway, because the recursive structure matters more than my algebra. | 5 | 5 | 5 | 15 |
| 2 | The EMA never forgets. Unlike the SMA, it uses every historical sample you have ever fed it, weighting each one exponentially less. | 4 | 5 | 5 | 14 |
| 3 | Your EMA is an IIR filter. One coefficient, alpha, controls how aggressively it discounts the past. Most traders never examine what that means. | 4 | 4 | 5 | 13 |
| 4 | The SMA throws away data outside its window. The EMA keeps all of it. That single difference changes everything about the step response. | 4 | 4 | 5 | 13 |

## 2. SMA

| # | Hook | Attn | Voice | Truth | Total |
|---|------|------|-------|-------|-------|
| 1 | The SMA has a constant bar lag no matter the frequency of the input signal. I did not expect that when I first worked through the math. | 5 | 5 | 5 | 15 |
| 2 | The window of L samples moves. That is literally why it is called a moving average. Most explanations skip the FIR filter structure underneath. | 4 | 5 | 5 | 14 |
| 3 | An SMA with an even number of bars and an SMA with an odd number have different amplitude responses. The math is worth seeing side by side. | 4 | 5 | 5 | 14 |
| 4 | Before the SMA window fills, the filter is "not primed." I use that term deliberately because the output is not yet meaningful. | 3 | 5 | 5 | 13 |

## 3. Frequency Response

| # | Hook | Attn | Voice | Truth | Total |
|---|------|------|-------|-------|-------|
| 1 | Most trading indicators you use daily are discrete digital filters. You can characterize them fully by their impulse response. | 5 | 5 | 5 | 15 |
| 2 | If your indicator is linear and time-invariant, superposition holds. That one property lets you predict its behavior on any input. | 4 | 5 | 5 | 14 |
| 3 | Shannon's sampling theorem sets a hard ceiling on what your indicator can resolve. No algorithm gets around that constraint. | 4 | 4 | 5 | 13 |

## 4. WMA

| # | Hook | Attn | Voice | Truth | Total |
|---|------|------|-------|-------|-------|
| 1 | The WMA places its center of gravity at one-third of the window, not one-half like the SMA. That is where the reduced lag comes from. | 5 | 5 | 5 | 15 |
| 2 | Bar lag of L/3 instead of L/2. The WMA gets there by weighting recent data linearly more, with no overshoot. A clean tradeoff. | 4 | 5 | 5 | 14 |
| 3 | Linearly descending coefficients sound like a small change over equal weights. The lag reduction is not small. | 4 | 4 | 5 | 13 |

## 5. DEMA

| # | Hook | Attn | Voice | Truth | Total |
|---|------|------|-------|-------|-------|
| 1 | Mulloy never mentions the "twicing" technique in his 1994 articles. I found the DSP connection myself, and his original derivation still seems unclear to me. | 5 | 5 | 5 | 15 |
| 2 | I could not find a single photo of Patrick Mulloy. I could also not follow his nonrigorous derivation of DEMA. I wrote up both problems. | 5 | 5 | 5 | 15 |
| 3 | DEMA subtracts lag by applying an EMA twice and combining the outputs. The cost is overshoot. Mulloy does not derive why this works rigorously. | 4 | 5 | 5 | 14 |
| 4 | The "twicing" technique from DSP explains DEMA better than Mulloy's own paper does. That is not a criticism, just an observation after working through both. | 4 | 5 | 5 | 14 |

## 6. TEMA

| # | Hook | Attn | Voice | Truth | Total |
|---|------|------|-------|-------|-------|
| 1 | The formula for TEMA is defined without any derivation. Three cascaded EMAs, more lag reduction than DEMA, and more overshoot. That is the entire disclosure. | 5 | 5 | 5 | 15 |
| 2 | TEMA extends DEMA with a triple-EMA cascade. Less lag, more overshoot. The original paper offers no derivation for why the formula works. | 4 | 5 | 5 | 14 |
| 3 | Each additional EMA cascade buys you lag reduction and costs you overshoot. TEMA pushes that tradeoff one step further than DEMA. | 4 | 4 | 5 | 13 |

## 7. T3EMA

| # | Hook | Attn | Voice | Truth | Total |
|---|------|------|-------|-------|-------|
| 1 | T3 runs your data through six cascaded EMAs. A single volume factor lets you trade lag for overshoot continuously. It needs 6L-5 data points before it is primed. | 5 | 5 | 5 | 15 |
| 2 | Increasing the volume factor decreases the lag but increases the overshooting. T3 gives you a knob for a tradeoff most indicators hardcode. | 5 | 5 | 5 | 15 |
| 3 | GD of GD of GD. That is the structure of T3: three nested generalized DEMA passes. The elegance hides a six-EMA cascade underneath. | 4 | 5 | 5 | 14 |
| 4 | Most traders treat T3 as a black box. It is three layers of generalized DEMA, each one a pair of cascaded EMAs. Six filters total. | 4 | 4 | 5 | 13 |

## 8. T2EMA

| # | Hook | Attn | Voice | Truth | Total |
|---|------|------|-------|-------|-------|
| 1 | Tillson spent 15 years trading privately while managing projects at HP. His trick: "turn down the volume" of the derivative. Most people leave it at full blast. | 4 | 5 | 5 | 14 |
| 2 | Set v=1, you get DEMA. Set v=0, you get a plain EMA. Tillson's insight was that the sweet spot is somewhere around 0.7 — and he gave us the knob to find it. | 4 | 5 | 5 | 14 |
| 3 | DEMA overshoots. EMA lags. Tillson built a single parameter that lets you dial continuously between the two. He called it "turning down the volume." | 4 | 4 | 5 | 13 |

## 9. TRIMA

| # | Hook | Attn | Voice | Truth | Total |
|---|------|------|-------|-------|-------|
| 1 | I couldn't derive the TRIMA equation algebraically. I tried. The indicator itself is just an SMA of an SMA — triangle-shaped weights, no overshoot. But the closed form beat me. | 5 | 5 | 5 | 15 |
| 2 | TRIMA is an SMA applied to an SMA. That's it. The triangle-shaped weights fall out naturally. The part I couldn't do was prove the closed-form equation on paper. | 4 | 5 | 5 | 14 |
| 3 | Run an SMA, then run another SMA on top of it. You get triangle weights and zero overshoot. The algebra to prove the closed form? I couldn't get there. | 4 | 5 | 5 | 14 |

## 10. KAMA

| # | Hook | Attn | Voice | Truth | Total |
|---|------|------|-------|-------|-------|
| 1 | Perry Kaufman built navigation systems for the Gemini space program before he ever touched a price chart. His adaptive moving average borrows the same idea: measure how efficient the path is. | 5 | 5 | 5 | 15 |
| 2 | KAMA is an EMA that watches its own efficiency ratio. In a clean trend, it speeds up. In noise, it slows to a crawl. I can't derive its frequency response analytically. | 4 | 5 | 5 | 14 |
| 3 | The efficiency ratio is displacement over path length — the same concept you'd use in aerospace navigation. Kaufman literally came from aerospace. The connection is not a coincidence. | 4 | 4 | 5 | 13 |

## 11. FRAMA

| # | Hook | Attn | Voice | Truth | Total |
|---|------|------|-------|-------|-------|
| 1 | Ehlers justified FRAMA's exponential mapping with a log-normal argument. It's not entirely accurate. But it works as a practical simplification, and I've made my peace with that. | 5 | 5 | 5 | 15 |
| 2 | When fractal dimension hits 1, FRAMA follows price exactly. When it hits 2, the filter clamps down to maximum smoothing. The mapping between D and alpha is where things get hand-wavy. | 4 | 5 | 5 | 14 |
| 3 | FRAMA is an EMA with a variable smoothing factor driven by fractal dimension. The concept is clean. Ehlers' justification for the exponential mapping is where I push back. | 4 | 5 | 5 | 14 |

## 12. JMA

| # | Hook | Attn | Voice | Truth | Total |
|---|------|------|-------|-------|-------|
| 1 | Jurik's algorithm was closed-source, shipped as a binary DLL. The trading community only learned how it worked because someone decompiled the Delphi code. | 5 | 5 | 5 | 15 |
| 2 | Founded in 1988 applying military signal processing to trading. Distributed as a locked binary. Four design goals: min lag, min overshoot, min undershoot, max smoothness. We only know this from decompiled code. | 5 | 4 | 5 | 14 |
| 3 | JMA had four design goals: minimize lag, minimize overshoot, minimize undershoot, maximize smoothness. The algorithm stayed proprietary until someone cracked the DLL. | 4 | 5 | 5 | 14 |

## 13. HTCE

| # | Hook | Attn | Voice | Truth | Total |
|---|------|------|-------|-------|-------|
| 1 | I do not fully understand what Ehlers' homodyne discriminator does. But it is mathematically elegant, numerically stable, and it measures cycles better than the alternatives I've tested. | 5 | 5 | 5 | 15 |
| 2 | The ideal Hilbert Transform is non-causal and infinite in duration. Ehlers approximates it with a 7-tap FIR filter. That gap between theory and practice is where all the interesting decisions live. | 5 | 5 | 5 | 15 |
| 3 | Ehlers calls the homodyne discriminator "mathematically elegant, numerically stable." I'll admit I don't fully understand the derivation. It looks like standard practice in DSP, and it works. | 4 | 5 | 5 | 14 |
| 4 | Shifting all frequencies by exactly -90 degrees requires an infinite, non-causal filter. Obviously useless for live trading. Ehlers' 7-tap approximation is the pragmatic answer. | 4 | 4 | 5 | 13 |
