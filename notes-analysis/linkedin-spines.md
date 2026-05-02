# Spine Extractions — All 13 Articles

Extracted for downstream LinkedIn post adaptation. Each spine contains 5-7 verbatim load-bearing claims, a thesis, closing maxim, and 3 hook candidates.

---

## 1. EMA

```json
{
  "article": "EMA",
  "source": "testdata/ema/content.md",
  "thesis": "The EMA is a recursive IIR filter with exponentially decreasing weights, whose frequency response can be derived analytically via the Z-transform.",
  "claims": [
    {
      "text": "The parameter α, 0<α<1, determines the aggressiveness of the EMA.",
      "evidence_type": "claim",
      "section": "opener",
      "translatability": 5
    },
    {
      "text": "As α→0, the last sample makes no impact, making the EMA line appear smoother. As α→1, only the last sample determines the EMA value, so the input samples just pass through the filter.",
      "evidence_type": "analogy",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "In contrast with the Simple Moving Average, the EMA uses all historical samples, giving them the smaller and smaller weighting factors.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "From the digital signal processing (DSP) point of view, the EMA is an infinite impulse response (IIR) filter wich applies exponentially decresing weighting factors to the input samples.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "The step-up response doesn't overshoot and the step-down response doesn't undershoot the data.",
      "evidence_type": "data",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "I was not able to derive both equations (12) and (13), and (Mak, 2006, p.15) do not have the simplified formula for the equation (13).",
      "evidence_type": "confession",
      "section": "closer",
      "translatability": 3
    }
  ],
  "closing_maxim": "I was not able to derive both equations (12) and (13), and (Mak, 2006, p.15) do not have the simplified formula for the equation (13).",
  "best_hook_candidates": [
    "In contrast with the Simple Moving Average, the EMA uses all historical samples, giving them the smaller and smaller weighting factors.",
    "The parameter α, 0<α<1, determines the aggressiveness of the EMA.",
    "I was not able to derive both equations (12) and (13)."
  ]
}
```

## 2. SMA

```json
{
  "article": "SMA",
  "source": "testdata/sma/content.md",
  "thesis": "The SMA is a FIR filter with equal weight coefficients, linear step response, constant bar lag, and an analytically derivable frequency response.",
  "claims": [
    {
      "text": "Thus, the window of L samples \"moves\", which explains the \"moving average\" in its name.",
      "evidence_type": "analogy",
      "section": "opener",
      "translatability": 5
    },
    {
      "text": "A scientific way to say this is \"an indicator is not primed\".",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "From the digital signal processing (DSP) point of view, SMA is a finite impulse response (FIR) filter with all L weight coefficients equal.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "The step transition is clearly linear.",
      "evidence_type": "data",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "The step-up response doesn't overshoot and the step-down response doesn't undershoot the data.",
      "evidence_type": "data",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "The shape of the amplitude response is different for the even and odd values of L.",
      "evidence_type": "data",
      "section": "closer",
      "translatability": 4
    }
  ],
  "closing_maxim": "And, finally, the number of samples lagging behind the signal is independent of angular frequency ω.",
  "best_hook_candidates": [
    "Thus, the window of L samples \"moves\", which explains the \"moving average\" in its name.",
    "A scientific way to say this is \"an indicator is not primed\".",
    "The shape of the amplitude response is different for the even and odd values of L."
  ]
}
```

## 3. Frequency Response

```json
{
  "article": "Frequency Response",
  "source": "testdata/frequency-response/content.md",
  "thesis": "Most trading indicators are discrete LTI digital filters whose behavior is completely characterized by their impulse response and frequency response.",
  "claims": [
    {
      "text": "In digital signal processing (DSP), most of trading indicators can be viewed as discrete digital filters.",
      "evidence_type": "claim",
      "section": "opener",
      "translatability": 5
    },
    {
      "text": "Linear filters have a superposition property.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "Such filters can be completely characterized by their impulse response.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "H(exp(iω)) describes the change in complex amplitude of a complex exponential input sequence as a function of the frequency ω.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "If a function f(t) contains no frequencies higher than W cps, it is completely determined by giving its ordinates at a series of points spaced 1/(2W) seconds apart.",
      "evidence_type": "paper",
      "section": "closer",
      "translatability": 5
    }
  ],
  "closing_maxim": "If a function f(t) contains no frequencies higher than W cps, it is completely determined by giving its ordinates at a series of points spaced 1/(2W) seconds apart.",
  "best_hook_candidates": [
    "In digital signal processing (DSP), most of trading indicators can be viewed as discrete digital filters.",
    "Such filters can be completely characterized by their impulse response.",
    "If a function f(t) contains no frequencies higher than W cps, it is completely determined by giving its ordinates at a series of points spaced 1/(2W) seconds apart."
  ]
}
```

## 4. WMA

```json
{
  "article": "WMA",
  "source": "testdata/wma/content.md",
  "thesis": "The WMA is a FIR filter with linearly descending coefficients that reduces lag compared to SMA by weighting recent data most heavily.",
  "claims": [
    {
      "text": "the multiplier weighting coefficients linearly descend from L at the last to 1 at the first element of the moving average window, so the earlier samples have less impact on the WMA.",
      "evidence_type": "claim",
      "section": "opener",
      "translatability": 5
    },
    {
      "text": "The WMA has a reduced lag which results from the most recent data being the most heavily weighted.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "The WMA coefficient values form a triangle across the width of the moving average window, resulting a center of the gravity being 1/3 across the window.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "That is, the bar lag of the WMA is L/3.",
      "evidence_type": "formula",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "From the digital signal processing (DSP) point of view, WMA is a finite impulse response (FIR) filter.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "The step-up response doesn't overshoot and the step-down response doesn't undershoot the data.",
      "evidence_type": "data",
      "section": "closer",
      "translatability": 4
    }
  ],
  "closing_maxim": "The step-up response doesn't overshoot and the step-down response doesn't undershoot the data.",
  "best_hook_candidates": [
    "The WMA has a reduced lag which results from the most recent data being the most heavily weighted.",
    "The WMA coefficient values form a triangle across the width of the moving average window, resulting a center of the gravity being 1/3 across the window.",
    "That is, the bar lag of the WMA is L/3."
  ]
}
```

## 5. DEMA

```json
{
  "article": "DEMA",
  "source": "testdata/dema/content.md",
  "thesis": "DEMA uses the DSP 'twicing' technique to subtract out lag from a double-EMA, producing a faster IIR filter that overshoots on step transitions.",
  "claims": [
    {
      "text": "He wanted to make an indicator \"faster\" (more sensitive to changes in the input data) than the Exponential Moving Average and to have a reduced lag.",
      "evidence_type": "claim",
      "section": "opener",
      "translatability": 5
    },
    {
      "text": "Details about Mulloy's personal life remain largely unknown, the only source of information being his two articles in the Technical Analysis of Stocks & Commodities journal mentioned above.",
      "evidence_type": "claim",
      "section": "opener",
      "translatability": 4
    },
    {
      "text": "I was not able to find any photo of him.",
      "evidence_type": "confession",
      "section": "opener",
      "translatability": 5
    },
    {
      "text": "Mulloy used a technique, well known in Digital Signal Processing, called \"twicing\" (Oppenheim et al., 2009 p.609), to decrease the lag of a Linear Time-Invariant (LTI) filter.",
      "evidence_type": "paper",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "Patrick Mulloy doesn't mention the \"twicing\" technique in his articles.",
      "evidence_type": "claim",
      "section": "pivot",
      "translatability": 5
    },
    {
      "text": "This \"nonrigorous derivation\" seems unclear to me.",
      "evidence_type": "confession",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "The step-up response overshoots and the step-down response undershoots the data.",
      "evidence_type": "data",
      "section": "closer",
      "translatability": 5
    }
  ],
  "closing_maxim": "The step-up response overshoots and the step-down response undershoots the data.",
  "best_hook_candidates": [
    "I was not able to find any photo of him.",
    "Patrick Mulloy doesn't mention the \"twicing\" technique in his articles.",
    "This \"nonrigorous derivation\" seems unclear to me."
  ]
}
```

## 6. TEMA

```json
{
  "article": "TEMA",
  "source": "testdata/tema/content.md",
  "thesis": "TEMA extends DEMA's double-smoothing with a triple-EMA cascade that reduces lag further but introduces overshoot/undershoot as a trade-off.",
  "claims": [
    {
      "text": "The Triple Exponential Moving Average (TEMA) was introduced by Patrick G. Mulloy as an extension of the double-smoothing technique to make the TEMA \"faster\" (more sensitive to changes in the input data) than the DEMA and to have a reduced lag.",
      "evidence_type": "claim",
      "section": "opener",
      "translatability": 4
    },
    {
      "text": "In Mulloy 1994 12(2), the formula for TEMA is defined without any derivation.",
      "evidence_type": "confession",
      "section": "body",
      "translatability": 3
    },
    {
      "text": "Here the first EMA smoothes the price, the second EMA smoothes the first EMA and the third EMA smoothes the second EMA.",
      "evidence_type": "analogy",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "From the digital signal processing (DSP) point of view, TEMA is a infinite impulse response (IIR) filter because it is based on EMA which applies exponentially decresing weighting factors to the input samples.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "The step-up response overshoot and the step-down response undershoot the data.",
      "evidence_type": "data",
      "section": "closer",
      "translatability": 5
    }
  ],
  "closing_maxim": "The step-up response overshoot and the step-down response undershoot the data.",
  "best_hook_candidates": [
    "The Triple Exponential Moving Average (TEMA) was introduced by Patrick G. Mulloy as an extension of the double-smoothing technique to make the TEMA \"faster\" than the DEMA and to have a reduced lag.",
    "In Mulloy 1994 12(2), the formula for TEMA is defined without any derivation.",
    "Here the first EMA smoothes the price, the second EMA smoothes the first EMA and the third EMA smoothes the second EMA."
  ]
}
```

## 7. T3EMA

```json
{
  "article": "T3EMA",
  "source": "testdata/t3ema/content.md",
  "thesis": "T3 takes Tillson's generalized DEMA concept to its logical extreme — three nested GD passes through six cascaded EMAs — trading increased overshoot for dramatically reduced lag.",
  "claims": [
    {
      "text": "T3 is a smoother version of the T2 Exponential Moving Average (T2).",
      "evidence_type": "claim",
      "section": "opener",
      "translatability": 5
    },
    {
      "text": "Tim Tillson defined the T3 as a GD of a GD of a GD",
      "evidence_type": "formula",
      "section": "body",
      "translatability": 3
    },
    {
      "text": "Regardless of the initialization approach, we always consider T3 to be primed only after the 6L-5 input data points have been fed into the indicator.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 2
    },
    {
      "text": "From the digital signal processing (DSP) point of view, T3 is a infinite impulse response (IIR) filter because it is based on EMA which applies exponentially decresing weighting factors to the input samples.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "Increasing the volume factor decreases the lag, but increases the overshooting and undershooting.",
      "evidence_type": "data",
      "section": "closer",
      "translatability": 5
    },
    {
      "text": "Increasing the length increases bothe the lag and the overshooting and undershooting.",
      "evidence_type": "data",
      "section": "closer",
      "translatability": 5
    }
  ],
  "closing_maxim": "Increasing the volume factor decreases the lag, but increases the overshooting and undershooting.",
  "best_hook_candidates": [
    "T3 is a smoother version of the T2 Exponential Moving Average (T2).",
    "Tim Tillson defined the T3 as a GD of a GD of a GD",
    "Increasing the volume factor decreases the lag, but increases the overshooting and undershooting."
  ]
}
```

## 8. T2EMA

```json
{
  "article": "T2EMA",
  "source": "testdata/t2ema/content.md",
  "thesis": "T2 generalizes DEMA by introducing a volume factor that lets traders dial between a pure EMA (no overshoot) and full DEMA (minimum lag), revealing that lag reduction and overshoot are two ends of the same knob.",
  "claims": [
    {
      "text": "Tim Tillson is a software project manager at Hewlett-Packard, with degrees in Mathematics and Computer Science. He has privately traded options and equities for 15 years.",
      "evidence_type": "confession",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "Looking at the definition of the DEMA and noticing that the second term adds a smoothed derivative to the EMA value, contributing to the overshooting and undershooting, he suggested to \"turn down the volume\" of the derivative using \"volume factor\" v.",
      "evidence_type": "analogy",
      "section": "pivot",
      "translatability": 4
    },
    {
      "text": "When v=1, GD becomes DEMA. When v=0, GD becomes a simple EMA.",
      "evidence_type": "formula",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "The default value of v is 0.7.",
      "evidence_type": "data",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "Increasing the volume factor decreases the lag, but increases the overshooting and undershooting.",
      "evidence_type": "data",
      "section": "closer",
      "translatability": 5
    },
    {
      "text": "From the digital signal processing (DSP) point of view, T2 is a infinite impulse response (IIR) filter because it is based on EMA which applies exponentially decresing weighting factors to the input samples.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 4
    }
  ],
  "closing_maxim": "Increasing the volume factor decreases the lag, but increases the overshooting and undershooting.",
  "best_hook_candidates": [
    "Tim Tillson is a software project manager at Hewlett-Packard, with degrees in Mathematics and Computer Science. He has privately traded options and equities for 15 years.",
    "he suggested to \"turn down the volume\" of the derivative using \"volume factor\" v.",
    "When v=1, GD becomes DEMA. When v=0, GD becomes a simple EMA."
  ]
}
```

## 9. TRIMA

```json
{
  "article": "TRIMA",
  "source": "testdata/trima/content.md",
  "thesis": "TRIMA achieves smooth, overshoot-free filtering by stacking two SMAs — a conceptually simple FIR design whose triangle-shaped weights emerge naturally from the convolution.",
  "claims": [
    {
      "text": "The TRIMA is equivalent to doing an SMA of an SMA.",
      "evidence_type": "claim",
      "section": "pivot",
      "translatability": 5
    },
    {
      "text": "I couldn't derive the equation (6) algebraically, but its working is illustrated on the figure 3.",
      "evidence_type": "confession",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "The TRIMA is a finite impulse response (FIR) filter because only a finite number of L last samples contribute to its value.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "It is easy to see that w_m values form two arithmetic progressions, one ascending and one descending.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "The step-up response doesn't overshoot and the step-down response doesn't undershoot the data.",
      "evidence_type": "data",
      "section": "closer",
      "translatability": 5
    },
    {
      "text": "It is interesting that the amplitude response poles of the TRIMA are formed by the lengthes of the inner and the outer SMAs.",
      "evidence_type": "data",
      "section": "body",
      "translatability": 3
    }
  ],
  "closing_maxim": "The step-up response doesn't overshoot and the step-down response doesn't undershoot the data.",
  "best_hook_candidates": [
    "The TRIMA is equivalent to doing an SMA of an SMA.",
    "I couldn't derive the equation (6) algebraically, but its working is illustrated on the figure 3.",
    "The step-up response doesn't overshoot and the step-down response doesn't undershoot the data."
  ]
}
```

## 10. KAMA

```json
{
  "article": "KAMA",
  "source": "testdata/kama/content.md",
  "thesis": "KAMA turns the EMA into an adaptive filter by modulating its smoothing factor with an efficiency ratio — speeding up in clean trends and slowing down in noise — but this adaptivity makes analytical frequency response impossible.",
  "claims": [
    {
      "text": "KAMA is actually an Exponential Moving Average (EMA) with a variable smoothing factor α_k being changed with each new sample within the minimum and the maximun boundaries.",
      "evidence_type": "claim",
      "section": "opener",
      "translatability": 5
    },
    {
      "text": "Perry Kaufman began his career in the aerospace industry, working on navigation and control systems for Gemini, and later transitioned his expertise to the financial world.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "The variable smoothing factor α_k reacts quickly when the market is trending strongly and smoothly, but slows down and filters out noise when the market is choppy or moving sideways.",
      "evidence_type": "claim",
      "section": "pivot",
      "translatability": 5
    },
    {
      "text": "An ER close to 1 indicates a highly efficient trend (strong directional movement with low noise).",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "We cannot derive its frequency response analytically because of the adaptive nature of the KAMA.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "There is almost no difference between the frequency responses of the KAMA with slow lengths ℓ_slow = 30 and ℓ_slow = 300.",
      "evidence_type": "data",
      "section": "closer",
      "translatability": 4
    }
  ],
  "closing_maxim": "There is almost no difference between the frequency responses of the KAMA with slow lengths ℓ_slow = 30 and ℓ_slow = 300.",
  "best_hook_candidates": [
    "Perry Kaufman began his career in the aerospace industry, working on navigation and control systems for Gemini, and later transitioned his expertise to the financial world.",
    "KAMA is actually an Exponential Moving Average (EMA) with a variable smoothing factor α_k being changed with each new sample within the minimum and the maximun boundaries.",
    "We cannot derive its frequency response analytically because of the adaptive nature of the KAMA."
  ]
}
```

## 11. FRAMA

```json
{
  "article": "FRAMA",
  "source": "testdata/frama/content.md",
  "thesis": "FRAMA adapts an EMA's smoothing factor via fractal dimension, becoming maximally responsive in trends and maximally filtering in noise — yet its frequency response is nearly transparent.",
  "claims": [
    {
      "text": "FRAMA is essentially an Exponential Moving Average (EMA) with a variable smoothing factor α_k that changes with each new sample, within minimum and maximum boundaries.",
      "evidence_type": "claim",
      "section": "opener",
      "translatability": 5
    },
    {
      "text": "The concept behind FRAMA is to link the fractal dimension D_k, calculated over a moving window of ℓ samples, to the EMA's smoothing factor α_k, thus making the EMA adaptive.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "\"Since the prices are log-normal, it seems reasonable to use an exponential function to relate the fractal dimension to alpha.\" (Ehlers 2005, p82)",
      "evidence_type": "paper",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "This is not entirely accurate (prices follow a log-normal distribution only if returns are normally distributed), but it serves as a practical simplification.",
      "evidence_type": "confession",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "When D_k=1, indicating a smooth trending market, α_k=exp(0)=1, so the filter responds instantly, making the FRAMA follow price exactly.",
      "evidence_type": "formula",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "When D_k=2, indicating extremely noisy market behavior, α_k=exp(ω)=α_slow, so the FRAMA is very slow, filtering out noise.",
      "evidence_type": "formula",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "We can see from both charts that the amplitude and phase lag remain nearly constant at 100% and 0 degrees, indicating that the δ-signal passes through the FRAMA with almost no change.",
      "evidence_type": "data",
      "section": "closer",
      "translatability": 3
    }
  ],
  "closing_maxim": "This is not entirely accurate (prices follow a log-normal distribution only if returns are normally distributed), but it serves as a practical simplification.",
  "best_hook_candidates": [
    "FRAMA is essentially an Exponential Moving Average (EMA) with a variable smoothing factor α_k that changes with each new sample, within minimum and maximum boundaries.",
    "\"Since the prices are log-normal, it seems reasonable to use an exponential function to relate the fractal dimension to alpha.\"",
    "This is not entirely accurate (prices follow a log-normal distribution only if returns are normally distributed), but it serves as a practical simplification."
  ]
}
```

## 12. JMA

```json
{
  "article": "JMA",
  "source": "testdata/jma/content.md",
  "thesis": "JMA was a proprietary, military-derived adaptive filter designed for optimal balance of lag, overshoot, undershoot, and smoothness — whose algorithm only became public through decompilation of its binary distribution.",
  "claims": [
    {
      "text": "His company Jurik Research was founded in 1988, applying signal processing techniques originally intended for military projects to trading.",
      "evidence_type": "claim",
      "section": "opener",
      "translatability": 5
    },
    {
      "text": "The suite of technical indicators selled by Jurik Research was closed source and distributed in binary form.",
      "evidence_type": "claim",
      "section": "opener",
      "translatability": 5
    },
    {
      "text": "The design aims for an optimal balance of four features: (1) minimum lag, (2) minimum overshoot, (3) minimum undershoot, i.e. quick convergence after gaps, and (4) maximum smoothness.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "JMA moves beyond traditional linear, frequency-based filtering, using techniques, described as adaptive non-linear filtering and information theory, \"analogous to military technology for tracking moving targets through noise\".",
      "evidence_type": "analogy",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "In early 2000s, on the various trading forums there were numerious posts about reverse engineering and re-implementing the JMA algorithm.",
      "evidence_type": "claim",
      "section": "pivot",
      "translatability": 5
    },
    {
      "text": "The decompiled Delphi (Pascal) code of the original Jurik's DLL for Wealth-Lab became a root source of the JMA imitations in MQL4, EasyLang, and others.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 5
    }
  ],
  "closing_maxim": "The decompiled Delphi (Pascal) code of the original Jurik's DLL for Wealth-Lab became a root source of the JMA imitations in MQL4, EasyLang, and others.",
  "best_hook_candidates": [
    "His company Jurik Research was founded in 1988, applying signal processing techniques originally intended for military projects to trading.",
    "The decompiled Delphi (Pascal) code of the original Jurik's DLL for Wealth-Lab became a root source of the JMA imitations in MQL4, EasyLang, and others.",
    "The design aims for an optimal balance of four features: (1) minimum lag, (2) minimum overshoot, (3) minimum undershoot, i.e. quick convergence after gaps, and (4) maximum smoothness."
  ]
}
```

## 13. HTCE

```json
{
  "article": "HTCE",
  "source": "testdata/htce/content.md",
  "thesis": "Ehlers applies the Hilbert Transform — approximated with a 7-tap windowed FIR filter — and three discriminator techniques to extract instantaneous dominant cycle periods from market data, with the homodyne discriminator being his favored method.",
  "claims": [
    {
      "text": "The Hilbert transform produces a version of a signal (price data) with all frequency components phase-shifted by −90°.",
      "evidence_type": "claim",
      "section": "opener",
      "translatability": 4
    },
    {
      "text": "This makes it possible to construct an analytic signal, from which we can extract the instantaneous phase, amplitude and frequency.",
      "evidence_type": "claim",
      "section": "opener",
      "translatability": 5
    },
    {
      "text": "The ideal Hilbert Transform is non-causal and infinite in duration, making it impractical for real-time applications like trading.",
      "evidence_type": "claim",
      "section": "pivot",
      "translatability": 5
    },
    {
      "text": "Ehlers approximates the Hilbert transform using a 7-tap FIR filter with a fixed set of coefficients.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 4
    },
    {
      "text": "This method is mathematically elegant, numerically stable, and well-suited to financial data — that's why Ehlers favors it.",
      "evidence_type": "claim",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "I do not fully understand what this all means, but it looks like a common practice in digital signal processing.",
      "evidence_type": "confession",
      "section": "body",
      "translatability": 5
    },
    {
      "text": "By combining multiple lags, we average out noise and phase quantization error, this makes it less sensitive to spikes than raw single-point phase differences.",
      "evidence_type": "claim",
      "section": "closer",
      "translatability": 4
    }
  ],
  "closing_maxim": "I do not fully understand what this all means, but it looks like a common practice in digital signal processing.",
  "best_hook_candidates": [
    "The ideal Hilbert Transform is non-causal and infinite in duration, making it impractical for real-time applications like trading.",
    "I do not fully understand what this all means, but it looks like a common practice in digital signal processing.",
    "This method is mathematically elegant, numerically stable, and well-suited to financial data — that's why Ehlers favors it."
  ],
  "notes": "CRITICAL BUG: Lines 297-301 of source contain KAMA text/links instead of HTCE content (copy-paste error)."
}
```
