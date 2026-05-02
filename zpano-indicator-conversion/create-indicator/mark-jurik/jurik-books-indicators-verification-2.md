# Verification Pass 2: jurik-books-indicators-brief.md

**Date:** 2026-04-11  
**Artifact under review:** `outputs/.drafts/jurik-books-indicators-brief.md`  
**Mode:** Second-pass adversarial audit, focused on resolution of prior findings and regression check  
**Prior review:** First-pass verification identified W1 (FATAL), W2 (MAJOR), W3 (MAJOR), W4 (MAJOR)

---

## Summary

The brief has been substantially rewritten to treat *Advanced Computerized Trading Techniques* (1997, Irwin Professional Publishing) as a distinct, separate publication from *Computerized Trading* (1999, NYIF). The publication count has been raised from three to four. The report collection count has been corrected from "16+" to "17." The contributor count language has been refined. Overall, the rewrite resolves the FATAL issue and most MAJOR issues, though some residual imprecisions and one new concern have been introduced.

## Strengths

- [S1] The FATAL issue (W1) is properly resolved. The 1997 book is now treated as a separate entry with full metadata: ISBN, publisher, page count, OCLC number, and Goodreads entry. The brief correctly presents the evidence for its distinctness (different publisher, ISBN prefix, page count, title).
- [S2] The brief retains appropriate epistemic hedging — the caveat paragraph acknowledges limited online presence and the theoretical possibility of a cataloging artifact, which is honest given the evidence gap.
- [S3] The report count (W4) has been fixed: the brief now says "17 reports" with all 17 enumerated, matching the jurikres.com table of contents exactly.
- [S4] The contributor list for *Computerized Trading* is now fully enumerated with all 19 non-Jurik names, allowing independent verification against the TOC.
- [S5] The summary table is well-structured and accurately reflects the state of evidence for each publication.
- [S6] Source list is comprehensive (8 sources) with direct URLs, enabling independent verification.

## Weaknesses

- [W1] **MINOR:** The publication date for the 1997 book has a discrepancy between sources that the brief does not acknowledge. The brief states "Published: June 1997 [8]" citing only the Goodreads entry. However, the Open Library JSON record (source [3]) states `"publish_date": "April 1997"`. The brief should note this discrepancy or cite both sources. This matters because the provenance of these dates is uncertain — neither source has a MARC library record for this book.

- [W2] **MINOR:** The brief claims the OL record has an "OCLC: 230954642" but does not note that the OL record's `source_records` field is `["amazon:0786311711"]`, meaning the entire Open Library entry was imported from Amazon product data, not from a library cataloging record (MARC). By contrast, the 1999 *Computerized Trading* book has six source records including Library of Congress MARC data and Internet Archive scans. This provenance difference is material to the reliability assessment. An Amazon-sourced OL record with an OCLC number does not mean WorldCat independently catalogs this book — the OCLC number may itself be Amazon-derived. The brief should note this.

- [W3] **MINOR:** The Goodreads description is quoted with a silent correction: the original text reads "understnad" (a typo), which the brief silently corrects to "understand." While not misleading, best practice for an evidence brief is to either reproduce the exact text (with [sic]) or note the correction.

- [W4] **MINOR:** The brief says the jurikres.com description states "Mark Jurik taps into the minds of twenty experts" and then interprets this as "20 named contributors including Jurik himself." However, the natural reading of "taps into the minds of twenty experts" is that there are twenty *other* experts besides Jurik. The TOC lists 20 unique names total (19 others + Jurik). So either the "twenty experts" is rounded/approximate, or Jurik does not count himself among the "experts." The brief's interpretation — "20 named contributors including Jurik himself" — is defensible but should acknowledge the ambiguity rather than presenting it as settled fact. (This partially addresses W2 from the first review, but the ambiguity is not fully resolved — it is now stated as fact rather than acknowledged as ambiguous.)

- [W5] **MINOR:** The brief states the 1997 book's publisher is "Irwin Professional Publishing (ISBN prefix 0-7863; now part of McGraw-Hill)" [3, 7]. Source [3] (Open Library) actually lists the publisher as "Irwin Professional Pub" (abbreviated). The full name "Irwin Professional Publishing" is a reasonable expansion, but the "now part of McGraw-Hill" claim is not supported by any cited source. While historically accurate (Irwin was acquired by Times Mirror and eventually absorbed into McGraw-Hill's imprint family), neither source [3] nor [7] makes this claim. This is uncited original research presented as sourced fact.

- [W6] **MINOR:** The brief says the *Neural Networks & Financial Forecasting* collection is "attested only by jurikres.com" and that "No library catalog, review, or independent bookseller listing was found beyond the jurikres.com product page." This is a valuable caveat. However, it was not present in the original draft's treatment of the same publication — it appears to be a new addition in the rewrite. While welcome, the same standard of independent corroboration is not applied to the audio seminar *Space, Time, Cycles & Phase*, which also has no independent attestation beyond jurikres.com. The asymmetry is notable.

## Questions for Authors

- [Q1] Was any attempt made to look up OCLC 230954642 directly in WorldCat? The OCLC record would be the strongest independent evidence for the 1997 book's existence. WorldCat was blocked during this verification pass, but the question remains open.

- [Q2] The brief says the 1997 book "does not appear on the jurikres.com catalog." Has the Wayback Machine been checked for historical versions of the jurikres.com catalog? If the 1997 book was once listed and later removed (perhaps after the 1999 edition superseded it), that would be significant evidence for its authenticity.

- [Q3] The Goodreads description mentions "complex indicators" — given that the 1999 book has a chapter by May titled "Complex Indicators, Nonlinear Pricing and Reflexivity," is it possible the Goodreads description for the 1997 book was actually copied or derived from marketing material for the 1999 book? This would weaken the inference that the two are distinct publications.

## Verdict

**The FATAL issue (W1) from the first review is resolved.** The brief now correctly treats the 1997 book as a separate publication with appropriate evidence and caveats. The rewrite is a substantial improvement.

**The MAJOR issues are partially resolved:**
- W2 (contributor count): Partially addressed — the names are now enumerated (good), but the "twenty experts" ambiguity is now presented as settled rather than acknowledged (see W4 above). **Downgraded from MAJOR to MINOR.**
- W4 (report count): Fully resolved — corrected to 17 with complete enumeration. **Resolved.**

**No new FATAL or MAJOR issues introduced.** The remaining issues are all MINOR — they concern citation precision, provenance transparency, and consistency of epistemic standards. None would change the brief's conclusions.

**Remaining risk:** The strongest unresolved question is whether the 1997 Irwin book truly exists as a distinct physical publication or is a cataloging artifact that acquired its own metadata ecosystem (Amazon listing → Open Library import → Goodreads entry). The brief handles this appropriately with its caveat paragraph, but the provenance chain (Amazon → OL → Goodreads) is thinner than it appears at first glance because all three records may derive from a single Amazon product listing. A WorldCat lookup or Wayback Machine check of jurikres.com would substantially strengthen or weaken this entry.

## Revision Plan

1. **[W1, W2]** Add a note about the April vs. June 1997 date discrepancy and the Amazon-sourced provenance of the OL record. One sentence each.
2. **[W3]** Either add [sic] after "understnad" or add a parenthetical noting the correction. Trivial edit.
3. **[W4]** Change "20 named contributors including Jurik himself" to something like "The table of contents lists 20 unique named contributors (19 chapter authors plus Jurik, who contributed appendix entries); the jurikres.com description refers to 'twenty experts,' which may or may not include Jurik."
4. **[W5]** Either cite a source for the McGraw-Hill claim or remove "now part of McGraw-Hill."
5. **[W6]** Add the same "no independent corroboration" caveat to the audio seminar entry, for consistency.
6. **[Q1-Q3]** Consider adding WorldCat lookup and Wayback Machine checks to the Open Questions section as concrete next steps.

**Overall assessment:** The brief is now in good shape. All remaining issues are MINOR polish items. The core factual claims are well-sourced, the epistemic hedging is appropriate, and the conclusions are well-supported. Ready for use with the understanding that the 1997 book entry carries lower confidence than the other three publications.

---

## Inline Annotations

> "Mark Jurik produced at least four publications: three sold through Jurik Research (two books and one audio seminar) plus a separate book published by Irwin Professional Publishing in 1997 [1, 2, 3]."

**[S1] Resolved:** This correctly distinguishes the Irwin publication from the jurikres.com catalog items. The "at least" qualifier is appropriate given evidence gaps.

> "Published: June 1997 [8]"

**[W1] MINOR:** Open Library JSON (source [3]) says "April 1997". The brief cites only the Goodreads date. Both dates should be noted, or the discrepancy acknowledged.

> "OCLC: 230954642 [3]"

**[W2] MINOR:** The OCLC number comes from an Open Library record whose sole `source_records` entry is `"amazon:0786311711"`. This means the OCLC number was likely imported alongside Amazon product data, not independently cataloged by a library. The brief presents this as equivalent provenance to library-sourced records. Suggest adding: "Note: The OL record for this edition was imported from Amazon product data, not from a library catalog record."

> "The Goodreads description states it 'demonstrates how to backtest a system once it is in place; discusses trading in the real world; and explains how to understand and use complex indicators' [8]."

**[W3] MINOR:** The actual Goodreads text reads "understnad" (typo), not "understand." Silent correction without [sic] or note.

> "Mark Jurik taps into the minds of twenty experts" [2], and the table of contents lists 20 named contributors including Jurik himself"

**[W4] MINOR:** The phrasing "taps into the minds of twenty experts" naturally implies 20 *other* people. The TOC has 20 unique names total (19 + Jurik). Either the "twenty" is approximate, or Jurik doesn't count himself. The brief should acknowledge this ambiguity rather than asserting "20 named contributors including Jurik himself."

> "Publisher: Irwin Professional Publishing (ISBN prefix 0-7863; now part of McGraw-Hill) [3, 7]"

**[W5] MINOR:** Source [3] says "Irwin Professional Pub." Source [7] (OL author page) does not mention McGraw-Hill. The "now part of McGraw-Hill" claim is uncited. While historically accurate, it's not supported by the referenced sources.

> "Independent corroboration: This publication is attested only by jurikres.com."

**[S2/W6] Mixed:** Good addition for the report collection, but the same standard is not applied to *Space, Time, Cycles & Phase*, which also lacks independent corroboration.

> "It is possible (though unlikely, given the different publisher and ISBN) that this is a cataloging artifact related to the 1999 book."

**[S2] Good:** Appropriate hedging. The parenthetical "though unlikely" is defensible given the different publisher and ISBN prefix, but per [W2] above, the provenance chain is thinner than it appears.

> "Contents include 17 reports [2]"

**[S3] Resolved:** Matches the exact count from the jurikres.com table of contents. All 17 are enumerated in the brief.

> "Application Note #2 explicitly 'compares the result with Jurik's moving average' [2] — a direct early reference to what became JMA."

**[S1] Good:** Accurately quoted from jurikres.com. The inference about JMA is reasonable given JMA is Jurik's moving average product.

> "The seminar's 'data decorrelation' topic is also topically related to DDR (Data Decorrelation and Reduction), though the seminar predates DDR's release and does not mention it by name — this connection is an inference based on shared subject matter."

**[S2] Good:** Excellent transparency about the inferential nature of this connection.

> "Articles in Futures magazine and Neurovest journal (per Jurik's self-authored bio on jurikres.com [2]; 'Neurovest journal' could not be independently verified as a publication name)"

**[S2] Good:** Appropriate caveat. Web search confirms "Neurovest" exists as a modern Swiss trading platform but not as a historical journal. The publication name remains unverified.

---

## Sources

- Open Library edition page (1997 book): https://openlibrary.org/books/OL11045411M
- Open Library edition JSON (1997 book): https://openlibrary.org/books/OL11045411M.json — confirms `source_records: ["amazon:0786311711"]`, `publish_date: "April 1997"`, `publishers: ["Irwin Professional Pub"]`
- Open Library edition JSON (1999 book): https://openlibrary.org/books/OL379103M.json — confirms 6 MARC source records including Library of Congress
- Goodreads (1997 book): https://www.goodreads.com/book/show/5405834 — confirms "320 pages, Hardcover, First published June 1, 1997", description with "understnad" typo
- jurikres.com publications page: http://jurikres.com/catalog1/cat_pub.htm — full TOC for all three jurikres.com publications
- jurikres.com catalog page: http://www.jurikres.com/catalog1/catalog.htm — product catalog listing all indicators and publications
- jurikres.com tech reports: http://www.jurikres.com/faq1/reports.htm — free PDF reports
- Open Library author page: https://openlibrary.org/authors/OL2803515A — confirms 3 works attributed to Mark Jurik
- Open Library publisher page (Irwin Professional Publishing): https://openlibrary.org/publishers/Irwin_Professional_Publishing — 448 works listed
