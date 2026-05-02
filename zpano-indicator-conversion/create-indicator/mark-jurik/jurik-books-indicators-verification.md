# Verification Report: jurik-books-indicators-brief.md

**Date:** 2026-04-11
**Artifact reviewed:** `outputs/.drafts/jurik-books-indicators-brief.md`
**Mode:** Adversarial evidence audit (not venue-style peer review)
**Sources cross-checked:** jurikres.com catalog [1], jurikres.com publications page [2], Open Library author page [3], Google Books [4], jurikres.com tech reports [5], ThriftBooks [6], Open Library edition page for "Advanced Computerized Trading Techniques," Goodreads entry for same, books-by-isbn.com ISBN prefix registry

---

## Summary

The brief is a well-structured bibliographic analysis of Mark Jurik's publications and their relationship to his proprietary trading indicators. Most factual claims trace back to primary sources (jurikres.com, Google Books). The core thesis — that Jurik's books teach principles rather than documenting proprietary algorithms — is well-supported by the TOC evidence. However, there is one **FATAL** issue: the treatment of the Open Library "third work" significantly understates available evidence and reaches a conclusion contradicted by metadata now available. Several other issues range from MAJOR to MINOR.

---

## Part 1: Structured Review

### Strengths

- [S1] All major factual claims about *Computerized Trading* (ISBN, publisher, page count, year) are confirmed by multiple independent sources (Google Books, ThriftBooks, Open Library).
- [S2] The TOC analysis for all three publications faithfully mirrors the jurikres.com source page. Contributor names for *Computerized Trading* match the actual TOC.
- [S3] The core analytical distinction ("principles vs. products") is well-hedged and supported by the TOC evidence — none of the TOC entries reference JMA, RSX, etc. by name.
- [S4] The Application Note #2 reference ("compares the result with Jurik's moving average") is correctly quoted from source [2] and properly identified as the most direct indicator connection.
- [S5] Confidence language is generally appropriate: "indirect," "partially," "strongly foundational" rather than absolute claims.

### Weaknesses

- [W1] **FATAL:** The Open Library "third work" assessment concludes it is "likely" a cataloging artifact, an earlier edition, or a working title — but available metadata strongly contradicts this. The Open Library edition page for *Advanced Computerized Trading Techniques* (OL11045411M) contains: ISBN-10: 0786311711, ISBN-13: 9780786311712, page count: 320, format: Hardcover, OCLC: 230954642, Goodreads ID: 5405834. The Goodreads entry independently confirms "320 pages, Hardcover, First published June 1, 1997" with a full description mentioning backtesting, trading, and complex indicators. Crucially, the ISBN prefix 0-7863 belongs to **Irwin Professional Publishing** (now McGraw-Hill), NOT to New York Institute of Finance (which uses 0-7352). *Computerized Trading* (1999) has a completely different ISBN (9780735200777), different publisher (NYIF), different page count (415 vs 320), and different publication date (1999 vs 1997). The brief states "No ISBN or publisher details are available for it on Open Library" — this is factually wrong. The brief's inference that this is "likely an earlier or alternate edition of the same book" is contradicted by the differing ISBN prefix (different publisher), different page count, and existing Goodreads description. This may be a genuinely separate publication. The brief must be corrected to reflect the available metadata and withdraw the unsupported inference.

- [W2] **MAJOR:** The brief says Jurik produced "three publications sold through Jurik Research: two books and one audio seminar." If "Advanced Computerized Trading Techniques" is a separate work (as W1 evidence suggests), this count is wrong. Even if not sold through jurikres.com, failing to acknowledge it as a potential fourth work is a significant omission when the brief's stated purpose is bibliographic completeness.

- [W3] **MAJOR:** The brief claims *Computerized Trading* had "contributions from twenty experts" [citing sources 1, 2]. Counting the actual named contributors from the jurikres.com TOC yields: DiNapoli, Melancon, Vomund, Widner, Ang, Luisi, De La Maza, Stendahl, Kiev, Hayes, Kase, Antonacci, Strasser, May, Klimasauskas, Drake, Katz, Marder, Jurik, Alotta = 20 unique names. However, Jurik himself is one of the twenty, and the brief simultaneously says "Jurik organized the book and contributed appendix chapters" as though he is separate from the twenty — creating a logical inconsistency. The source page says "Mark Jurik taps into the minds of twenty experts" — implying 20 *other* experts plus Jurik as editor. The actual TOC lists 20 unique names including Jurik. The "twenty" count is ambiguous and the brief doesn't flag this ambiguity.

- [W4] **MAJOR:** The brief states the "Neural Networks & Financial Forecasting" collection includes "16+ reports" but lists only 17 items from the TOC. The jurikres.com TOC actually lists exactly 17 distinct entries. The "16+" phrasing is imprecise in the wrong direction when the source allows an exact count. More importantly, the brief provides no independent corroboration for this publication's existence, date, or contents beyond jurikres.com — it relies on a single primary source that is also the commercial seller.

- [W5] **MINOR:** The brief says the publisher of *Computerized Trading* is "New York Institute of Finance [4]" citing Google Books. Google Books confirms "New York Institute of Finance, 1999." The original draft (not the brief) adds "(Prentice Hall)" in parentheses, which is accurate context (NYIF was a Prentice Hall imprint) but is not in the brief under review. No issue here, but worth noting the brief dropped context present in the earlier draft.

- [W6] **MINOR:** The brief mentions "articles in *Futures* magazine and *Neurovest* journal [2]" — this claim comes entirely from Jurik's self-authored speaker bio on jurikres.com. "Neurovest journal" returns zero independent verification as a publication name. The brief repeats this claim without flagging that it rests on a single self-promotional source with no independent corroboration.

- [W7] **MINOR:** The "Space, Time, Cycles & Phase" section says it covers "data decorrelation" and links this to DDR. The jurikres.com description says "the correct way to decorrelate data" under the SPACE topic. The connection to DDR (the Jurik product) is an inference by the brief's author, not stated in any source. While plausible (DDR stands for "Data Decorrelation and Reduction"), this inference is not explicitly labeled as such.

- [W8] **MINOR:** The summary table lists *Computerized Trading* as "(1999) [4]" but Open Library has two entries — one "First published in 1998" and one "First published in 1999." Google Books says 1999. The brief discusses the discrepancy in the Open Library section but the summary table uses only 1999 without a note. This is not wrong but a completeness gap.

### Questions for Authors

- [Q1] Was the Open Library edition page for "Advanced Computerized Trading Techniques" (OL11045411M) actually visited during research? The brief claims "No ISBN or publisher details are available for it on Open Library" but the page contains ISBN, page count, OCLC number, and Goodreads link.
- [Q2] Was WorldCat (OCLC 230954642) consulted to verify the "Advanced Computerized Trading Techniques" entry? WorldCat would provide definitive library catalog evidence.
- [Q3] The brief says the *Neural Networks* collection has reports "also published in *AI in Finance*." Was any attempt made to verify this journal publication independently?
- [Q4] Were any library catalogs (WorldCat, Library of Congress) consulted to corroborate the *Neural Networks & Financial Forecasting* publication beyond jurikres.com?

### Verdict

The brief is **not ready for publication** in its current form due to W1 (FATAL). The "third work" section draws a strong inferential conclusion ("likely an earlier or alternate edition") that is contradicted by concrete, publicly available metadata (different ISBN prefix → different publisher, different page count, different date, independent Goodreads description). This error cascades into W2, potentially undermining the brief's core claim of bibliographic completeness.

After fixing W1, the brief would be a solid reference document with appropriate hedging on most claims.

### Revision Plan

1. **[MUST — W1]** Retrieve and incorporate the full Open Library edition metadata for "Advanced Computerized Trading Techniques" (ISBN 9780786311712, 320pp, Hardcover, Irwin Professional Publishing, 1997, OCLC 230954642). Revise the "third work" section to present this as likely a distinct publication from a different publisher, not a cataloging artifact. Explicitly note the different ISBN prefix (Irwin vs. NYIF).

2. **[MUST — W2]** Update the publication count. If evidence supports a separate fourth work, say "three publications sold through Jurik Research, plus a fourth book published by Irwin Professional Publishing in 1997 that does not appear in the jurikres.com catalog."

3. **[SHOULD — W3]** Clarify the "twenty experts" ambiguity. Either count excluding Jurik (19 contributing authors + Jurik as editor = "twenty" per the source's framing) or note the discrepancy.

4. **[SHOULD — W4]** Change "16+" to "17" (the exact count from the source TOC). Note that this publication's existence is attested only by jurikres.com and piracy/course-sharing sites — no independent library catalog or review source was found.

5. **[COULD — W6]** Add a note that "Neurovest journal" is attested only in Jurik's own bio copy on jurikres.com and could not be independently verified as a publication.

6. **[COULD — W7]** Label the DDR connection as an inference: "DDR (Data Decorrelation and Reduction) appears topically related to the seminar's 'data decorrelation' content, though the seminar predates DDR's release and does not mention it by name."

7. **[COULD — W8]** Add a footnote to the summary table noting the 1998/1999 date ambiguity for *Computerized Trading*.

---

## Part 2: Inline Annotations

> "No ISBN or publisher details are available for it on Open Library [3]."
**[W1] FATAL:** This is factually incorrect. The Open Library edition page (OL11045411M) contains: ISBN-10: 0786311711, ISBN-13: 9780786311712, 320 pages, Hardcover format, OCLC: 230954642, Goodreads: 5405834. The Goodreads entry independently confirms "320 pages, Hardcover, First published June 1, 1997" with a substantive description. The ISBN prefix 0-7863 identifies the publisher as Irwin Professional Publishing (now McGraw-Hill) — a different company from the New York Institute of Finance (ISBN prefix 0-7352) that published *Computerized Trading*.

> "**Inference:** This is likely an earlier or alternate edition of the same 'Computerized Trading' book, not a separate work. The 1997 date would align with pre-publication or a preliminary edition."
**[W1] FATAL:** This inference is contradicted by the available evidence. A "preliminary edition" or "working title" would not have: (a) a different publisher's ISBN prefix, (b) a different page count (320 vs 415), (c) a substantively different title, and (d) an independent Goodreads entry with its own description that does not mention "day trading" (the subtitle focus of the 1999 book). The evidence is more consistent with this being a separate publication, possibly an earlier book by Jurik that was superseded or not continued through jurikres.com. This inference must be retracted.

> "Mark Jurik produced three publications sold through Jurik Research: two books and one audio seminar [1, 2]."
**[W2] MAJOR:** This count may be incomplete. If "Advanced Computerized Trading Techniques" (1997, Irwin Professional Publishing) is a separate work, Jurik produced at least four publications — three sold through jurikres.com and one published through a mainstream publisher. The brief should qualify this as "three publications *listed on jurikres.com*" or investigate whether the Irwin book is a fourth work.

> "contributions from twenty experts [1, 2]"
**[W3] MAJOR:** The jurikres.com page says "Mark Jurik taps into the minds of twenty experts" — implying 20 others plus Jurik. However, the actual TOC lists exactly 20 unique names *including* Jurik (who contributed two appendix entries). The brief then says "Jurik organized the book and contributed appendix chapters" as if separate from the twenty, which contradicts the count. Either 19 other experts contributed (and "twenty" in the source includes Jurik), or 20 others contributed plus Jurik. The brief doesn't resolve this.

> "Contents include 16+ reports [2]"
**[W4] MAJOR:** The source TOC on jurikres.com lists exactly 17 distinct report titles. Using "16+" is unnecessarily imprecise when the source allows an exact count. The hedging direction is also wrong — "16+" leaves open the possibility there could be fewer than 17 while actually the count is at least 17.

> "articles in *Futures* magazine and *Neurovest* journal [2]"
**[W6] MINOR:** The sole source for "Neurovest journal" is Jurik's own speaker bio on jurikres.com. No independent verification of a journal by this name was found. The brief should note this is a self-reported claim.

> "**data decorrelation** (related to DDR)"
**[W7] MINOR:** The 1995 seminar covers "the correct way to decorrelate data" (source [2]). The link to DDR (a Jurik product) is an analytical inference — DDR stands for "Data Decorrelation and Reduction" and addresses the same conceptual domain. However, the seminar predates DDR's release, and no source connects the two. This inference should be explicitly labeled as such rather than presented as a stated relationship.

> "The seminar is based on Jurik's published article 'Developing Indicators for Financial Trading' [2]"
**[S4] VERIFIED:** This is directly stated on jurikres.com: "This material is based on our published article 'Developing Indicators for Financial Trading'." Correctly attributed.

> "Application Note #2 explicitly 'compares the result with Jurik's moving average' — an early reference to JMA [2]"
**[S4] VERIFIED:** The jurikres.com TOC entry for Application Note #2 states: "Shows how to increase the smoothness of an exponential moving average without inducing significant lag, and then compares the result with Jurik's moving average." Correctly quoted with appropriate analytical commentary.

> "**Publisher:** New York Institute of Finance [4]"
**[S1] VERIFIED:** Google Books confirms "New York Institute of Finance, 1999 - 415 sider [pages]." Also confirmed by ThriftBooks and Open Library.

> "**ISBN:** 9780735200777 [6]"
**[S1] VERIFIED:** Confirmed by ThriftBooks, Open Library, Amazon, and eBay listings.

---

## Sources

- jurikres.com catalog: http://www.jurikres.com/catalog1/catalog.htm (fetched 2026-04-11)
- jurikres.com publications: http://jurikres.com/catalog1/cat_pub.htm (fetched 2026-04-11)
- Open Library — Mark Jurik author page: https://openlibrary.org/authors/OL2803515A (fetched 2026-04-11)
- Open Library — "Advanced Computerized Trading Techniques" edition page: https://openlibrary.org/books/OL11045411M (fetched 2026-04-11)
- Goodreads — "Advanced Computerized Trading Techniques": https://www.goodreads.com/book/show/5405834 (fetched 2026-04-11)
- Google Books — "Computerized Trading": https://books.google.com/books/about/Computerized_Trading.html?id=JdgpAQAAMAAJ (fetched 2026-04-11)
- ThriftBooks — "Computerized Trading": https://www.thriftbooks.com/w/computerized-trading-maximizing-day-trading-and-overnight-profits-new-york-institute-of-finance_mark-jurik/414015/ (fetched 2026-04-11)
- jurikres.com tech reports: http://www.jurikres.com/faq1/reports.htm (fetched 2026-04-11)
- books-by-isbn.com — ISBN prefix 0-7863 (Irwin Professional Publishing): https://www.books-by-isbn.com/0-7863/ (fetched 2026-04-11)
- SNNS User Manual — Backpercolation: https://www.inf.ufsc.br/~aldo.vw/patrec/SNNS/UserManual/node154.html (fetched 2026-04-11)
- ThriftBooks — *Virtual Trading* by Jess Lederman: https://www.thriftbooks.com/w/virtual-trading-how-any-trader-with-a-pc-can-use-the-power-of-neural-nets-and-expert-systems-to-boost-trading-profits_jess-lederman/1242832/ (fetched 2026-04-11)
