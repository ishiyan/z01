# Dr. Manfred G. Dürschner

## Executive Summary

Dr. Manfred G. Dürschner is a German physicist and independent technical analyst who applies signal processing theory and nonlinear mathematics to financial markets. He is best known for developing the **3rd Generation Moving Average** (also called the New Moving Average / NMA), which uses the Nyquist-Shannon sampling theorem to eliminate temporal lag from moving averages [1][2]. He received the First Prize of the VTAD Award (German Association of Technical Analysts) in 2011 for this work [3][5]. He also authored the book *Technische Analyse mit EMD* (Wiley-VCH, 2014), which applies NASA's Empirical Mode Decomposition to financial time series analysis [3][7].

## Biography

### Academic Background

Dürschner holds a doctorate in physics ("promovierte Physiker") [3] and uses the title "Dr." [16]. His own presentation slides describe him as "Diplom-Physiker, Privatanleger" (diploma physicist, private investor) [16], indicating he completed the German Diplom degree in physics and works as an independent/private investor rather than in an institutional role. No specific university affiliation has been publicly documented. His approach to financial markets is grounded in physics and signal processing theory — he explicitly frames his contributions as applying physical models and reasoning to investment decisions [3]. He is based in the Nuremberg area, serving as Regionalmanager of the VTAD-Regionalgruppe Nürnberg [16].

### Career and Affiliations

- **Active trader:** Has been trading successfully on stock markets for an extended period, with several years of practical experience in market technics and trading systems [3].
- **VTAD membership:** Member of the Vereinigung Technischer Analysten Deutschlands e.V. (German Association of Technical Analysts) [3][5].
- **IFTA contributor:** Published in the International Federation of Technical Analysts (IFTA) Journal [2].
- **Independent researcher:** No known institutional affiliation; appears to work independently [inferred].

### Awards

- **2011 VTAD Award, First Prize** — for the paper "Gleitende Durchschnitte 3.0" (Moving Averages 3.0), which introduced the Nyquist-Shannon-based moving average method [3][5].

### VTAD Role

Dürschner served as **Regionalmanager of the VTAD-Regionalgruppe Nürnberg** (Nuremberg regional chapter of the German Association of Technical Analysts) [16]. He describes himself as a "Diplom-Physiker, Privatanleger" (diploma physicist, private investor) [16].

### Contact

- Email: madicon@t-online.de [16]

### Public Presence and Photos

A photograph of Dr. Dürschner appears on the introductory slide (slide 2) of his VTAD Online Tutorial presentations, visible at approximately 01:00 in both "Digitale Indikatoren I" [16] and "Viccao — Ein profitables Handelssystem" [17]. The photo shows a middle-aged man with glasses wearing a dark suit and tie. Local copies: `inputs/manfred-dürschner/dürschner.png` (cropped), `inputs/manfred-dürschner/dürschner-full-slide.png` (full slide).

## Publications

### Papers

| Year | Title | Venue | Language |
|------|-------|-------|----------|
| 2011 | Gleitende Durchschnitte 3.0 | VTAD (research paper) [5][9] | German |
| 2012 | Moving Averages 3.0 | IFTA Journal, 2012 Edition, pp. 14–19 [2] | English |
| ~2019 | EMD for Technical Analysis | ResearchGate [4] | English |

### Books

| Year | Title | Publisher | ISBN | Pages |
|------|-------|-----------|------|-------|
| 2014 | Technische Analyse mit EMD: die Anwendung der EMD auf Indizes, Rohstoffe, Währungen und Aktien | Wiley-VCH (Weinheim), Wiley Trading series [3][7][8] | 978-3-527-50718-4 | 220 |

**Book structure** (based on reviews and publisher description) [8]:
- **Part 1:** Fundamental analysis, macroeconomics, behavioral finance overview
- **Part 2:** Technical analysis indicators — includes novel indicators developed by the author
- **Part 3:** Empirical Mode Decomposition (EMD) — mathematical foundations, application to price series, trading examples. Implementation uses Wolfram Mathematica.

**Contributor:** Michael Lehnert [7].

**Reception:** Mixed (2.5/5 on Amazon.de with 3 reviews). Criticized for not providing implementable code for the EMD portion; praised for Part 2's indicator innovations [8].

## Technical Indicators Created

### 1. New Moving Average (NMA) / 3rd Generation Moving Average

**The core innovation.** A lag-free moving average derived from signal processing theory [1][2][6].

**Problem solved:** All conventional moving averages (SMA, EMA, WMA) introduce temporal lag proportional to their period. This causes delayed signals at trend reversals [1].

**Method:** Apply two moving averages in cascade, then geometrically extrapolate to cancel the combined lag [1][2]:

$$\text{NMA}[n_1, n_2] = (1 + \alpha) \cdot \text{MA}_1[\text{price} \mid n_1] - \alpha \cdot \text{MA}_2[\text{MA}_1 \mid n_2]$$

where:

$$\alpha = \frac{\lambda \cdot (n_1 - 1)}{n_1 - \lambda}, \quad \lambda = \left\lfloor \frac{n_1}{n_2} \right\rfloor \geq 2$$

**The Nyquist constraint:** When a moving average is applied to already-smoothed data (resampling), the Nyquist-Shannon theorem requires: $n_1 \geq 2 \cdot n_2$. Violating this (as Ehlers' "Moving Averages 2.0" does with $n_1 = n_2$) can produce aliasing artifacts [1][2].

**Key properties:**
- Theoretical lag: **zero** bars [1][2]
- Recommended MA type: Linear Weighted Moving Average (LWMA) — smallest inherent lag [1]
- Best lag reduction at $\lambda = 2$ [6]; higher $\lambda$ increases similarity to classic MA
- When $\lambda = 1$ (degenerate case), formula reduces to Ehlers' MMA [1]

**Generations framework** [1][2]:
- Moving Averages 1.0: SMA, EMA, WMA (standard)
- Moving Averages 2.0: Ehlers' MMA, Mulloy's TEMA (lag reduction without Nyquist constraint)
- Moving Averages 3.0: Dürschner's NMA (Nyquist-constrained lag elimination)

### 2. NWMA Trading System (Medium-Term)

A complete trading system using the NMA as foundation [1][2]:

- **Signal chain:** Price → NWMA(89, 21) → Aroon Oscillator(5) → Inverse Fisher Transform
- **Rules:** Buy when IFT > 0, sell when IFT < 0
- **Timeframe:** Daily charts, stocks
- **Backtesting results** (104 stocks, 11 years, Jan 2000 – Jan 2011) [1]:
  - NWMA(89,21): +4,697% net profit (avg per share, from EUR 1,000)
  - NWMA(100,25): +3,229%
  - MWMA(89) [Ehlers]: +1,232%
  - WMA(89): +839%
  - SMA(89): +455%
  - Buy & Hold: +110%

### 3. StochRSI Intraday System (Short-Term)

- **Signal chain:** Price → NWMA(8, 3) → RSI(5) → Stochastic(3) → Inverse Fisher Transform [1][5]
- **Timeframe:** M15, designed for DAX intraday trading
- **Purpose:** Short-term scalping with lag-free price proxy

### 4. EMD-Based Technical Analysis ("Technical Analysis 2.0")

Dürschner's 2014 book applies NASA's Empirical Mode Decomposition (developed by Norden Huang) to financial time series [3][7]. The EMD decomposes a nonlinear, non-stationary signal into Intrinsic Mode Functions (IMFs) without requiring predefined basis functions.

**Application to markets** [3][7]:
- Decompose price series into oscillatory components (IMFs) + residual trend
- Each IMF represents a different time-scale of market behavior
- The residual represents the underlying trend stripped of noise
- Enables construction of modified Bollinger Bands, trend indicators, and timing signals

**Implementation:** Wolfram Mathematica [8]. No source code was published in the book — only mathematical formulations.

**Indicators mentioned in book context** (based on book keywords) [7]:

| Indicator | Base Available (MQL5) | EMD-Combined Version |
|-----------|----------------------|---------------------|
| EMD Trend extraction | [CEMDecomp class](https://www.mql5.com/en/articles/439) (article #439, 2012) | Residual from EMD sifting = trend. Implemented in CEMDecomp. |
| Modified Bollinger Bands (EMD-based) | [Bollinger Bands](https://www.mql5.com/en/code/94) (standard) | Custom: apply bands to EMD residual. Requires CEMDecomp + custom wrapper. |
| MACD variants using EMD decomposition | [MACD](https://www.mql5.com/en/code/8) (standard) | Custom: MACD on selected IMF components. No known public implementation. |
| RAVI (Range Action Verification Index) with EMD | [RAVI](https://www.mql5.com/en/code/7795) (standard) | Custom: RAVI on EMD-filtered series. No known public implementation. |
| Aroon Oscillator on EMD output | [Aroon Oscillator](https://www.mql5.com/en/code/7778) (standard) | Custom: Aroon on EMD residual. No known public implementation. |
| SRSI (Stochastic RSI) on EMD output | [Stochastic RSI](https://www.mql5.com/en/code/10972) (standard) | Custom: StochRSI on EMD-smoothed price. No known public implementation. |
| Efficiency line ("Effizienzlinie") | — | Dürschner-specific concept. No known public implementation. |

**Implementation note:** The EMD-combined variants are Dürschner's unique contributions from his 2014 book. The base indicators exist as standard MQL5 implementations. To replicate Dürschner's approach, one would combine the `CEMDecomp` class from [MQL5 article #439](https://www.mql5.com/en/articles/439) (by Victor, 2012) with standard indicator logic. The book's own implementation uses Wolfram Mathematica, not MQL [8].

## VTAD Online Tutorials

In addition to his published papers and book, Dürschner authored **four video tutorials** for the VTAD's online education platform [12]:

| Tutorial | Topic |
|----------|-------|
| Digitale Indikatoren I | First method for constructing digital (step-shaped) indicators with precise signal detection |
| Digitale Indikatoren II | Two additional methods for digital indicators; combined examples form a complete trading system |
| Digitale Indikatoren III | Complete trading system from digital indicators; backtesting results and real trading outcomes |
| Viccao — ein profitables Handelssystem | A medium-term trend-following system based on digital indicators; derivation, indicator interplay, trading rules, backtesting, and live results |

The **Viccao** system and the "Digital Indicators" series represent a body of work beyond the NMA paper, showing Dürschner's ongoing development of systematic trading approaches using signal-theoretic methods [12].

## Community Reception and Impact

### Implementations Across Platforms

The 3rd Generation Moving Average has been independently implemented in multiple trading platforms [5][6][10][11]:

| Platform | Author | Date | Notes |
|----------|--------|------|-------|
| MQL4 | Jürgen Moeck (simplex) | July 2013 | First known implementation [5] |
| MQL5 (MetaTrader 5) | Nikolay Kositsin (GODZILLA) | Nov 14, 2012 | "3rd Generation XMA" — supports 10 smoothing methods, λ=2. 9,632 views, rating 22/22. Credits "EarnForex" as real author [11] |
| TradingView (Pine Script) | everget (Pine Wizard) | Oct 6, 2018 | Open-source, 14,985 favorites. Title: "Moving Average 3.0 (3rd Generation)" [10] |
| MetaTrader 4 | ThirdBrainFx | Unknown | Commercial indicator distribution [6] |
| Python | Community | Unknown | Local copy in inputs [5] |

### Academic Citations

Dürschner's work has been cited in academic mathematics research:
- **Prof. S. Maier-Paape, RWTH Aachen University** — cited as `Duerschner2013` in a 2013 preprint on "Basic Statistical Properties of Trends" (period length analysis, cross-correlations, trend quality measurement for FDAX, EUR/USD, Gold, Oil) [14]

A preprint titled "EMD for Technical Analysis" is hosted on juergen-abel.info, likely representing an English-language preprint of Dürschner's EMD work predating or accompanying the 2014 Wiley-VCH book [13].

### Positive
- The NMA/3rd Generation Moving Average has been independently implemented in MQL4, MQL5, Pine Script, Python, and other platforms by community members [5][6][10][11]
- The TradingView implementation alone has nearly 15,000 favorites [10]
- The MQL5 implementation (Nov 2012) may predate the IFTA Journal publication (2012), suggesting early draft circulation [11]
- The "Moving Averages 3.0" paper won the VTAD's top research award [3][5]
- One Amazon reviewer described Part 2 of the EMD book as the best practical inspiration they'd encountered across extensive trading literature [8]
- Cited in academic mathematics research at RWTH Aachen [14]

### Critical
- The EMD book received criticism for not providing implementable code [8]
- Reviewers noted the EMD section occupies only ~90 of 300 pages, with the remainder covering standard topics [8]
- The backtesting methodology (equal periods across different MA types) was questioned by independent analysts as potentially unfair comparison [5]
- Forum members found the NMA's practical edge over conventional short-period MAs to be modest (1-2 bars faster, but more whipsaws) [5]

## Open Questions

- No confirmed photo or detailed personal biography beyond "PhD physicist, active trader"
- Exact university where doctorate was obtained is unknown
- Whether he continues to publish or trade actively (post-2019) is unclear
- The full content of the "EMD for Technical Analysis" ResearchGate paper could not be accessed (blocked); a preprint may be the juergen-abel.info PDF [13]
- Relationship between the book's EMD approach and any commercial products/services is unknown
- The Viccao trading system and Digital Indicators tutorials represent additional unpublished work — full details are behind VTAD member access [12]
- The VTAD EMD presentation by "Reiß" [15] may be related to Dürschner's work or independent

## Sources

| # | Source | Status |
|---|--------|--------|
| [1] | Dürschner, M. G. (2012). Moving Averages 3.0. *IFTA Journal*, 2012 Ed., pp. 14–19. Local copy: `inputs/manfred-dürschner/moving-averages-3.0/Moving-Averages-3.0.md` | verified |
| [2] | Dürschner, M. G. (2011). Gleitende Durchschnitte 3.0. IFTA Journal URL: https://www.ifta.org/assets/docs/d_ifta_journal_12.pdf | unverified (too large to fetch) |
| [3] | Google Books "Technische Analyse mit EMD" — author bio and metadata. https://books.google.com/books/about/Technische_Analyse_mit_EMD.html?id=p_w5BAAAQBAJ | verified |
| [4] | ResearchGate: "EMD for Technical Analysis." https://www.researchgate.net/publication/338105724_EMD_for_Technical_Analysis | blocked (403) |
| [5] | Moeck, J. (2013). Nyquist-Shannon Moving Average [Forum]. https://www.stevehopwoodforex.com/phpBB3/viewtopic.php?t=2637 | verified |
| [6] | ThirdBrainFx. 3rd Generation Moving Average. https://www.thirdbrainfx.com/indicator/3rd-generation-moving-average/ | verified |
| [7] | EconBiz library record. https://www.econbiz.de/Record/technische-analyse-mit-emd-die-anwendung-der-emd-auf-indizes-rohstoffe-währungen-und-aktien-dürschner-manfred/10010218571 | unverified (not fetched) |
| [8] | Amazon.com product page and reviews. https://www.amazon.com/Technische-Analyse-mit-EMD-Anwendung/dp/3527507183 | verified |
| [9] | Web Archive — VTAD paper. https://web.archive.org/web/20200109020131/http://www.vtad.de/sites/files/forschung/M_Duerschner_Gleitende_Durchschnnitte_3.pdf | unverified (not fetched) |
| [10] | TradingView: "Moving Average 3.0 (3rd Generation)" by everget (Oct 2018). https://www.tradingview.com/script/is0c2vTu-Moving-Average-3-0-3rd-Generation/ | verified |
| [11] | MQL5 CodeBase: "3rd Generation XMA" by Nikolay Kositsin (Nov 2012). https://www.mql5.com/en/code/1032 | verified |
| [12] | VTAD Online Tutorials page — lists 4 tutorials by Dr. Manfred G. Dürschner. https://www.vtad.de/online-tutorials/ | verified |
| [13] | Preprint: "EMD for Technical Analysis" (hosted by Jürgen Abel). https://www.juergen-abel.info/files/preprints/preprint_emd_for_technical_analysis_pdf_00.pdf | unverified (PDF binary, not text-extractable) |
| [14] | Maier-Paape, S. (2013). Preprint on statistical properties of trends, RWTH Aachen. Cites Duerschner2013. http://www.instmath.rwth-aachen.de/Preprints/maierpaape20130415.pdf | verified (structure only) |
| [15] | VTAD: Reiß, "Empirical Mode Decomposition" presentation PDF. https://www.vtad.de/wp-content/uploads/2017/02/Rei%C3%9F_EmpiricalModeDecomposition.pdf | blocked (403) |
| [16] | VTAD Online Tutorial video: "Digitale Indikatoren I" — introductory slide at ~01:00 shows photo, email, credentials, VTAD role. https://www.vtad.de/vvideo/digitale-indikatoren-i/ | verified (screenshot) |
| [17] | VTAD Online Tutorial video: "Viccao — Ein profitables Handelssystem" — same introductory slide at ~01:00. https://www.vtad.de/vvideo/viccao-ein-profitables-handelssystem/ | verified (screenshot) |
| [18] | futures.io attachment 41061. https://futures.io/attachments/41061d1308131522 | blocked (403) |
| [19] | Victor (2012). "Introduction to the Empirical Mode Decomposition Method." MQL5 article #439. Includes CEMDecomp class source code. https://www.mql5.com/en/articles/439 | verified |

## BibTeX

```bibtex
@article{durschner2012moving,
  author  = {Dürschner, Manfred G.},
  title   = {Moving Averages 3.0},
  journal = {IFTA Journal},
  year    = {2012},
  pages   = {14--19},
  note    = {Local copy: inputs/manfred-dürschner/moving-averages-3.0/Moving-Averages-3.0.md}
}

@online{durschner2011ifta_pdf,
  author  = {Dürschner, Manfred G.},
  title   = {Gleitende Durchschnitte 3.0},
  year    = {2011},
  url     = {https://www.ifta.org/assets/docs/d_ifta_journal_12.pdf},
  note    = {IFTA Journal 2012 edition PDF}
}

@online{durschner2014googlebooks,
  author  = {Dürschner, Manfred G.},
  title   = {Technische Analyse mit EMD},
  year    = {2014},
  url     = {https://books.google.com/books/about/Technische_Analyse_mit_EMD.html?id=p_w5BAAAQBAJ},
  note    = {Google Books metadata and author bio}
}

@online{durschner2019researchgate,
  author  = {Dürschner, Manfred G.},
  title   = {EMD for Technical Analysis},
  year    = {2019},
  url     = {https://www.researchgate.net/publication/338105724_EMD_for_Technical_Analysis},
  note    = {Blocked (403)}
}

@online{moeck2013forum,
  author  = {Moeck, Jürgen},
  title   = {Nyquist-Shannon Moving Average},
  year    = {2013},
  url     = {https://www.stevehopwoodforex.com/phpBB3/viewtopic.php?t=2637},
  note    = {Forum thread with MQL4 implementation and discussion}
}

@online{thirdbrainfx_3gma,
  author  = {{ThirdBrainFx}},
  title   = {3rd Generation Moving Average},
  url     = {https://www.thirdbrainfx.com/indicator/3rd-generation-moving-average/},
  note    = {Commercial MetaTrader 4 indicator}
}

@online{durschner2014econbiz,
  author  = {Dürschner, Manfred G.},
  title   = {Technische Analyse mit EMD: die Anwendung der EMD auf Indizes, Rohstoffe, Währungen und Aktien},
  year    = {2014},
  url     = {https://www.econbiz.de/Record/technische-analyse-mit-emd-die-anwendung-der-emd-auf-indizes-rohstoffe-w%C3%A4hrungen-und-aktien-d%C3%BCrschner-manfred/10010218571},
  note    = {EconBiz library record}
}

@online{durschner2014amazon,
  author  = {Dürschner, Manfred G.},
  title   = {Technische Analyse mit EMD: Anwendung},
  year    = {2014},
  url     = {https://www.amazon.com/Technische-Analyse-mit-EMD-Anwendung/dp/3527507183},
  note    = {Amazon product page and reviews}
}

@online{durschner2011vtad_archive,
  author  = {Dürschner, Manfred G.},
  title   = {Gleitende Durchschnitte 3.0},
  year    = {2011},
  url     = {https://web.archive.org/web/20200109020131/http://www.vtad.de/sites/files/forschung/M_Duerschner_Gleitende_Durchschnnitte_3.pdf},
  note    = {Web Archive copy of original VTAD paper}
}

@online{everget2018tradingview,
  author  = {everget},
  title   = {Moving Average 3.0 (3rd Generation)},
  year    = {2018},
  month   = oct,
  url     = {https://www.tradingview.com/script/is0c2vTu-Moving-Average-3-0-3rd-Generation/},
  note    = {Pine Script implementation, 14985 favorites}
}

@online{kositsin2012mql5,
  author  = {Kositsin, Nikolay},
  title   = {3rd Generation XMA},
  year    = {2012},
  month   = nov,
  url     = {https://www.mql5.com/en/code/1032},
  note    = {MQL5 CodeBase, supports 10 smoothing methods}
}

@online{vtad_tutorials,
  author  = {{VTAD}},
  title   = {Online Tutorials},
  url     = {https://www.vtad.de/online-tutorials/},
  note    = {Lists 4 tutorials by Dr. Manfred G. Dürschner}
}

@online{durschner_emd_preprint,
  author  = {Dürschner, Manfred G.},
  title   = {EMD for Technical Analysis},
  url     = {https://www.juergen-abel.info/files/preprints/preprint_emd_for_technical_analysis_pdf_00.pdf},
  note    = {Preprint hosted by Jürgen Abel}
}

@techreport{maierpaape2013trends,
  author      = {Maier-Paape, Stanislaus},
  title       = {Basic Statistical Properties of Trends},
  institution = {RWTH Aachen University},
  year        = {2013},
  url         = {http://www.instmath.rwth-aachen.de/Preprints/maierpaape20130415.pdf},
  note        = {Cites Duerschner2013}
}

@online{reiss_emd_vtad,
  author  = {Reiß},
  title   = {Empirical Mode Decomposition},
  url     = {https://www.vtad.de/wp-content/uploads/2017/02/Rei%C3%9F_EmpiricalModeDecomposition.pdf},
  note    = {VTAD presentation PDF, blocked (403)}
}

@online{vtad_digitale_indikatoren_i,
  author  = {Dürschner, Manfred G.},
  title   = {Digitale Indikatoren I},
  url     = {https://www.vtad.de/vvideo/digitale-indikatoren-i/},
  note    = {VTAD Online Tutorial video}
}

@online{vtad_viccao,
  author  = {Dürschner, Manfred G.},
  title   = {Viccao — Ein profitables Handelssystem},
  url     = {https://www.vtad.de/vvideo/viccao-ein-profitables-handelssystem/},
  note    = {VTAD Online Tutorial video}
}

@online{futuresio_attachment,
  title   = {Attachment 41061},
  url     = {https://futures.io/attachments/41061d1308131522},
  note    = {Blocked (403)}
}

@article{victor2012emd_mql5,
  author  = {Victor},
  title   = {Introduction to the Empirical Mode Decomposition Method},
  year    = {2012},
  url     = {https://www.mql5.com/en/articles/439},
  note    = {MQL5 article with CEMDecomp class source code}
}

@online{mql5_bollinger,
  title   = {Bollinger Bands},
  url     = {https://www.mql5.com/en/code/94},
  note    = {Standard MQL5 CodeBase indicator}
}

@online{mql5_macd,
  title   = {MACD},
  url     = {https://www.mql5.com/en/code/8},
  note    = {Standard MQL5 CodeBase indicator}
}

@online{mql5_ravi,
  title   = {RAVI (Range Action Verification Index)},
  url     = {https://www.mql5.com/en/code/7795},
  note    = {Standard MQL5 CodeBase indicator}
}

@online{mql5_aroon,
  title   = {Aroon Oscillator},
  url     = {https://www.mql5.com/en/code/7778},
  note    = {Standard MQL5 CodeBase indicator}
}

@online{mql5_stochrsi,
  title   = {Stochastic RSI},
  url     = {https://www.mql5.com/en/code/10972},
  note    = {Standard MQL5 CodeBase indicator}
}

@book{durschner2014emd,
  author    = {Dürschner, Manfred G.},
  title     = {Technische Analyse mit EMD: die Anwendung der EMD auf Indizes, Rohstoffe, Währungen und Aktien},
  publisher = {Wiley-VCH},
  address   = {Weinheim},
  year      = {2014},
  isbn      = {978-3-527-50718-4},
  pages     = {220},
  series    = {Wiley Trading}
}
```
