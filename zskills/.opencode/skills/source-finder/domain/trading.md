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

## Technical Analysis of Stocks & Commodities (TASC)

Published by Traders.com since 1982. The definitive magazine for trading indicators, DSP-based methods, and system development. Article PDFs require subscription, but extensive metadata is freely available.

Base domain: `technical.traders.com` and `traders.com`

### Free Resources

#### TOC Archive (XML, every issue since 1982)

URL pattern: `https://traders.com/Mobile/Archive/{MON}{YYYY}.XML`

- `{MON}` = 3-letter uppercase month abbreviation: JAN, FEB, MAR, APR, MAY, JUN, JUL, AUG, SEP, OCT, NOV, DEC
- `{YYYY}` = 4-digit year
- There is a 13th "bonus" issue each year using `BON` as the month code

Examples:
- January 2022: `https://traders.com/Mobile/Archive/JAN2022.XML`
- Bonus 2022: `https://traders.com/Mobile/Archive/BON2022.XML`
- October 1982: `https://traders.com/Mobile/Archive/OCT1982.XML`

XML structure (key elements per `<Article>`):
- `<Name>` — article title
- `<Author>` — author name
- `<Year>` — publication year
- `<Subject>` — topic category
- `<More>` — article synopsis/abstract
- `<To>` — PDF path (e.g., `\V40\C01\353KAUF.pdf`). Extract the filename part to search for freely available copies online.
- `<charmonth>` — 3-letter month code

Note: Most browsers fail to render these XMLs due to a missing XSL reference. Fetch raw XML and parse directly.

#### Author Search

URL pattern: `http://technical.traders.com/archive/combo/display5.asp?author={Name}`

- URL-encode the author name (spaces as `%20`, drop periods)
- Example: `http://technical.traders.com/archive/combo/display5.asp?author=John%20F%20Ehlers`

Returns HTML with article entries containing:
- Article title (in `<h3 class="articleTitle">`)
- Synopsis (in `<p class="caption">`)
- Author, date, subject (in `<p class="byline">`)
- PDF link (in `<div class="link">`)

#### Complete Author List

`https://technical.traders.com/archive/combo/authorlist.asp`

#### Complete Title Archive (Alphabetical)

`https://technical.traders.com/archive/combo/title/titlelist.asp?word=atitlelist`

#### Magazine Covers

Full resolution (1000px PNG):
`https://technical.traders.com/images/New-2014/1000px-png/{YYMM}.png`

Mini-cover:
`https://technical.traders.com/images/New-2014/{YYYY}/{YYMM}_Minicover.png`

Format for `{YYMM}`:
- March 2025 → `2503`
- January 2001 → `0101`
- January 2000 → `0001`
- January 1999 → `9901`
- October 1982 → `8210`
- Bonus issues use `13` as month: `2513` for bonus 2025

The `New-2014` path segment is constant (not year-related). The `{YYYY}` folder in mini-cover URLs is the 4-digit year.

#### Traders' Tips (Free Reference Implementations)

URL pattern: `http://traders.com/Documentation/FEEDbk_docs/{YYYY}/{MM}/TradersTips.html`

- `{YYYY}` = 4-digit year, `{MM}` = 2-digit month (zero-padded)
- First available issue: 2009/01
- Example: `http://traders.com/Documentation/FEEDbk_docs/2009/01/TradersTips.html`

Each page contains reference implementations of indicators from the current or previous TASC issue, contributed by platform vendors (TradeStation, MetaStock, AmiBroker, NinjaTrader, etc.).

When processing a Traders' Tips page:
1. **Download linked ZIP files** — these contain full source code projects
2. **Download linked XLSM/Excel files** — these contain spreadsheet implementations
3. **Extract inline source listings** — code blocks embedded directly in the HTML (EasyLanguage, AFL, C#, Pine Script, etc.)

### Search Patterns

```
site:technical.traders.com "indicator name"
site:traders.com "author name"
```

To find freely available TASC PDFs elsewhere:
```
"V40\C01\353KAUF" filetype:pdf
"Technical Analysis of Stocks and Commodities" "article title"
```

Extract the PDF path from the `<To>` element and search for the filename portion.

### BibTeX Template

```bibtex
@article{AuthorYYYYkeyword,
  author    = {Kaufman, Perry J.},
  title     = {Trading A Moving Average System: Important Choices},
  journal   = {Technical Analysis of Stocks \& Commodities},
  year      = {2022},
  month     = jan,
  volume    = {40},
  number    = {1},
  note      = {PDF: V40/C01/353KAUF.pdf},
  url       = {https://technical.traders.com/archive/article.asp?file=\V40\C01\353KAUF.pdf},
}
```

Volume/issue can be derived from the PDF path: `V40\C01` → volume 40, issue 1.

---

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
