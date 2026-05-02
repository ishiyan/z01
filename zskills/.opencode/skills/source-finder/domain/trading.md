# Trading & Quantitative Finance Sources

Domain-specific sources for trading indicators, algorithmic trading, and quantitative finance.

## Forums & Communities

| Source | URL | Best for |
|--------|-----|----------|
| MQL5 Code | https://www.mql5.com/en/code | Indicator implementations (MQL4/5) |
| MQL5 Articles | https://www.mql5.com/en/articles | In-depth technical articles |
| MQL5 Forum | https://www.mql5.com/en/forum | Discussions, reverse engineering, comparisons |
| ForexFactory | https://www.forexfactory.com/forum | Trading strategies, indicator discussions |
| TradingView Scripts | https://www.tradingview.com/scripts/ | Pine Script implementations |
| useThinkScript | https://usethinkscript.com/ | ThinkOrSwim implementations |
| Elite Trader | https://www.elitetrader.com/et/forums/ | Professional trader discussions |
| Quantopian (archive) | https://quantopian.com/ | Algorithmic trading (archived) |

## Search Patterns

### MQL5

```
site:mql5.com/en "indicator name"
site:mql5.com/en/articles "topic"
site:mql5.com/en/code "indicator"
```

MQL5 has internal search — also try directly on the site.

### TradingView

```
site:tradingview.com/script "indicator name"
```

### ForexFactory

```
site:forexfactory.com "indicator name"
```

## Quality Signals (Trading-Specific)

| Signal | High quality | Low quality |
|--------|-------------|-------------|
| Code provided | Full source, commented | "Buy my indicator" |
| Backtesting | Results shown with methodology | Cherry-picked charts |
| Math explained | Formulas, DSP concepts | "Secret algorithm" |
| Author history | Long forum history, many posts | New account, single post |
| Community response | Expert discussion, corrections | No replies or only praise |

## Special Considerations

- Many trading indicators are **proprietary/closed-source** — forum discussions about reverse engineering are valuable primary sources
- **Decompiled code** posts are often the authoritative source for proprietary indicators (e.g., JMA)
- **Pine Script** implementations on TradingView are public and citable
- Beware **marketing content** disguised as analysis — many trading sites sell courses/subscriptions

## BibTeX Notes

For forum posts, use `@online` with:
- `author` = forum username (in braces if pseudonym)
- `title` = thread title
- `url` = direct link to specific post (not just thread)
- `note` = "Forum post" or "MQL5 Code Base"
- `urldate` = access date
