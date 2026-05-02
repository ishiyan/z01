# Boilerplate — LinkedIn / Trading Practitioner Rewrites

Adapted from `boilerplate-original.md` for the "normal trading" audience on LinkedIn. Derivations dropped, DSP jargon replaced with trading-context language, results stated without proofs.

---

## 1. Nyquist / Period Explanation

**Original**: 6-line explanation with sinusoid imagery, "peak and trough," reference to external article.

**LinkedIn rewrite**:

> The shortest cycle any indicator can detect is 2 bars. If you're on daily bars, you can't measure anything shorter than a 2-day cycle. That's a hard sampling constraint, not a software limitation.

**Notes**: Dropped the sinusoid walkthrough entirely. "2 bars" is the only number practitioners need. The word "Nyquist" is omitted — the concept is preserved without the label.

---

## 2. Normalized Frequency Explanation

**Original**: 7-line block with LaTeX for period/frequency relationship, normalized frequency definition.

**LinkedIn rewrite**:

> Frequency is just the inverse of period. A 10-bar cycle has a frequency of 0.1. The highest possible frequency — a 2-bar cycle — is the Nyquist frequency. When charts below show "normalized frequency," 1.0 means the Nyquist limit and 0 means an infinitely long cycle.

**Notes**: One sentence instead of seven. No LaTeX. "Normalized frequency" is defined in plain English for anyone who encounters it in the full article.

---

## 3. Step Response Introduction

**Original**: 4-6 variants describing step-up/step-down response, overshoot/undershoot behavior.

**LinkedIn rewrite (no overshoot)**:

> When the price jumps from one level to another, this filter catches up smoothly without overshooting. It reaches the new level with a delay equal to its length — no ringing, no false signals on the transition.

**LinkedIn rewrite (with overshoot)**:

> When the price jumps from one level to another, this filter overshoots before settling. That overshoot is the cost of reduced lag — the filter reacts faster but briefly exaggerates the move.

**Notes**: "Step response" replaced with "when the price jumps." Overshoot framed as a tradeoff practitioners can evaluate. No need for "step-up" vs "step-down" distinction — the behavior is symmetric, so one sentence covers both.

---

## 4. Frequency Response Section Header + Intro

**Original**: "The figure(s) below show(s) an amplitude and a phase lag of the unit sample response of a [INDICATOR] as a function of a period of various signal frequencies."

**LinkedIn rewrite**:

> The charts below show how much signal the filter passes at each cycle length (amplitude) and how much delay it introduces (phase lag).

**Notes**: "Unit sample response" dropped — practitioners care about what the chart shows, not the formal name for the test signal. "Various signal frequencies" replaced with "each cycle length."

---

## 5. DSP Classification

**Original**: 3 variants (FIR, IIR, IIR-adaptive) each 2-3 lines with "from the DSP point of view" framing.

**LinkedIn rewrite (FIR)**:

> This filter only looks at the last L bars. Once a bar falls outside the window, it has zero influence on the output.

**LinkedIn rewrite (IIR)**:

> This filter has memory that stretches back to the first bar. Every historical price contributes, but older bars matter exponentially less.

**LinkedIn rewrite (IIR-adaptive)**:

> This filter remembers every bar you've ever fed it, but it adjusts how much attention it pays to recent vs. old data based on current market conditions.

**Notes**: "FIR" and "IIR" labels dropped. The behavioral difference (finite window vs. infinite memory) is what matters to practitioners. The adaptive variant adds the market-conditions angle.

---

## 6. EMA Initialization

**Original**: 8-line block covering two initialization approaches (SMA-based and first-data-point), with LaTeX formula and Metastock reference.

**LinkedIn rewrite**:

> The filter needs a starting value. The standard approach is to average the first L bars as a seed. An older approach, used by Metastock, just takes the first price. Both methods converge to the same output after enough bars — the difference only matters in short backtests.

**Notes**: Formula for L dropped — practitioners don't need it for LinkedIn. "Both methods converge" is the actionable insight. Metastock reference preserved because it's a real attribution.

---

## 7. Priming Statement

**Original**: One-line per indicator stating how many bars before the filter is primed, with LaTeX expressions.

**LinkedIn rewrite (template)**:

> This filter isn't meaningful until it has seen [N] bars of data. Before that, the output is based on incomplete information. Account for this warm-up period in backtesting.

**Notes**: "Not primed" replaced with "isn't meaningful" — same concept, more intuitive. The backtesting implication is stated explicitly because that's where practitioners encounter this issue.

---

## 8. Geometric Series Derivation

**Original**: 10-line algebraic proof of the geometric series sum formula, with reference to Jeffrey and Dai (2008).

**LinkedIn rewrite**:

> [OMIT ENTIRELY]

**Notes**: This is pure algebra with no trading implication. The result (a closed-form sum) is used internally by the filter math but has no practitioner-facing meaning. Drop it on LinkedIn. If someone needs it, the full article link is right there.

---

## 9. Chart Caption Patterns

**Original**: Standardized captions like "An amplitude (a) and a phase lag (b) as a function of a period of a cycle."

**LinkedIn rewrite**:

> [NOT APPLICABLE — LinkedIn posts don't include charts. Full article link serves as the path to visual content.]

**Notes**: LinkedIn is text-only for this series. Chart references in post body should describe the finding, not the chart.

---

## 10-13. Reference Blocks (Oppenheim, Jeffrey & Dai, Mulloy, Tillson)

**Original**: Full bibliographic entries with edition, page count, publisher, Google Books links.

**LinkedIn rewrite (inline attribution)**:

> Oppenheim et al. (2009) → "Oppenheim's DSP textbook (2009)"
> Jeffrey and Dai (2008) → [OMIT — only cited for geometric series, which is omitted]
> Mulloy (1994) → "Mulloy's 1994 articles in Technical Analysis of Stocks & Commodities"
> Tillson (1998) → "Tillson's 1998 article in Technical Analysis of Stocks & Commodities"

**Notes**: Full bibliographic blocks are inappropriate for LinkedIn's format. Inline attributions preserve Author + Year + Publication. Page numbers and Google Books links belong in the full article, which is linked at the bottom of every post.
