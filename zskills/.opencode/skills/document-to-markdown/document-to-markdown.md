---
name: document-to-markdown
description: Use this skill whenever the user wants to extract content from a document file or web page into markdown, images, and code. Supports PDF, PPTX, PPT (legacy), DOCX, XLSX, HTML, MHTML, and live web pages (including SPAs). Handles the full pipeline: source detection, text extraction to markdown, image/figure extraction, code listing extraction, and produces a standardized output structure.
---

# Document-to-Markdown Extraction

## Purpose

Extract structured content (markdown text, images, code listings) from any supported source into a standardized output directory.

## Supported Sources

| Source type | Detection method |
|-------------|-----------------|
| PDF | Magic bytes `%PDF-` |
| EPUB | ZIP magic + `mimetype` entry containing `application/epub+zip` |
| PPTX / DOCX / XLSX | ZIP magic + internal paths |
| PPT / DOC (legacy OLE) | OLE2 magic bytes |
| HTML file | `<html` or `<!doctype` in first 500 bytes |
| MHTML file | `From:` or `MIME-Version:` header |
| Live web page (URL) | Starts with `http://` or `https://` |

## Standard Output Structure

```
<output_dir>/
  content.md          # Main markdown content
  summary.md          # Brief summary (title, key topics, source info)
  assets/             # Images, diagrams, figures, photos
    figure-01.png
    photo-author.jpg
  code/               # Extracted code listings (actual source code only)
    listing-01.py
    listing-02.go
  manifest.json       # Metadata about the extraction
```

### Rules

1. **Asset locality** — All images/assets referenced by the content MUST be copied into the output `assets/` directory. Update paths in `content.md` to use relative references (`![](assets/filename.png)`). Never leave references pointing to the original source location. This includes:
   - `<img src="...">` tags
   - Custom viewer components (e.g., `<mb-svg-viewer src="...">`) that embed SVG illustrations
   - Any other tag with a `src` attribute referencing a local asset file
   - For web pages: resolve relative URLs to absolute, then download

2. **SVG color compatibility** — SVGs from web apps often rely on CSS-inherited colors. When copying SVGs, inject a `<style>` block with `prefers-color-scheme` media queries:
   ```xml
   <style>
     svg { stroke: black; fill: black; }
     @media (prefers-color-scheme: dark) {
       svg { stroke: white; fill: white; }
     }
   </style>
   ```
   Skip if the SVG already has explicit color attributes.

3. **Math vs. code** — The `code/` directory is for actual source code listings only. Mathematical equations (LaTeX, KaTeX, MathJax) should remain inline in `content.md` using standard LaTeX notation (`$$...$$` for display, `$...$` for inline).

4. **Document what's lost** — If content cannot be extracted (interactive charts, video, canvas elements), note it in `manifest.json` under `"notes"` and add a placeholder comment in `content.md`:
   ```markdown
   <!-- Interactive chart: [description] — cannot be extracted as static image -->
   ```

## Step 1: Source Detection

### Local files
Use magic bytes to detect format. Do NOT trust file extensions alone. See `reference/format-detection.py` for implementation.

### URLs (web pages)
If the source is a URL:
1. Fetch with markdown conversion first (tools like WebFetch in markdown mode, or `trafilatura`). This works for **static HTML** pages.
2. If the result is mostly empty or just navigation chrome, the page is likely a **Single-Page Application (SPA)** rendered client-side by JavaScript.
3. For SPAs: note that a headless browser (Puppeteer, Playwright) would be needed for full extraction. Extract what's available from the HTTP response.

## Step 2: Content Extraction

### Decision tree

```
Source is a URL?
├─ YES → Fetch content
│   ├─ Static HTML → Strip boilerplate, extract main content
│   └─ SPA → Extract what's available, document limitations
└─ NO → Detect format from magic bytes
    ├─ PDF → Use markitdown for text, pdfplumber for images
    ├─ EPUB → Unzip, read spine order from OPF, convert XHTML chapters via html2text, extract images from images/ dir
    ├─ PPTX → Use markitdown for text, python-pptx for images
    ├─ PPT (legacy) → Convert via libreoffice if available, else olefile
    ├─ DOCX → Use markitdown for text, python-docx for images
    ├─ XLSX/XLSM → openpyxl: classify sheets, export tables as CSV + markdown summary
    ├─ HTML → Strip boilerplate, markitdown or beautifulsoup
    └─ MHTML → Parse MIME parts, extract HTML and embedded images
```

### Web page specifics

**Boilerplate stripping:** Web pages contain navigation, sidebars, headers, footers. To identify article content:
- Look for `<article>`, `<main>`, or `role="main"` elements
- The first `<h1>` usually marks the start of real content
- Repeated elements across pages from the same site are boilerplate
- Use trafilatura as a fallback for main content extraction

**Asset resolution for web pages:**
- Resolve relative URLs (e.g., `assets/photo.jpg`) against the page's base URL
- Download each referenced image/asset into the output `assets/` directory
- Update references in `content.md` to point to local copies

**Interactive/dynamic content (charts, visualizations):**
- JavaScript-rendered charts (D3, Chart.js, Angular/React components) cannot be extracted without a headless browser
- Document their existence in manifest.json and as comments in content.md
- If a headless browser is available, use `page.screenshot()` to capture chart regions

### EPUB specifics

EPUB files are ZIP archives containing XHTML chapters, images, and metadata:

1. **Read `META-INF/container.xml`** to find the OPF file path (usually `OEBPS/package.opf`)
2. **Parse the OPF** to get the spine (reading order) and manifest (id→href map)
3. **Process chapters in spine order** — read each XHTML file, convert to markdown with `html2text` (set `body_width=0` to avoid line wrapping)
4. **Fix image paths** — XHTML references like `../images/fig.jpg` must be rewritten to `assets/fig.jpg`
5. **Extract images** — copy all files from the images directory into `assets/`
6. **Join chapters** with `---` separators into a single `content.md`

Key settings for `html2text`:
- `body_width = 0` (no wrapping)
- `unicode_snob = True` (preserve unicode)
- `images_to_alt = False` (keep image references)

### Excel specifics (XLSX, XLSM, XLS)

Excel workbooks contain multiple sheets with mixed content types. Use `openpyxl` with `data_only=True` to read computed values.

#### Sheet classification

Scan each sheet and classify it before converting:

| Sheet type | Detection heuristic | Conversion strategy |
|---|---|---|
| **Prose/notes** | Few columns, mostly text, sparse cells | Extract as paragraphs |
| **Tabular data** | Clear header row + many uniform data rows | Markdown table (truncated) + full CSV export |
| **Parameter/control** | Scattered labels and values, no uniform rows | Key-value list or indented block |
| **Mixed** | Has both free-form areas and data grids | Split into subsections at the boundary |

**Header detection heuristic:** Scan the first 30 rows. The header row is typically the first row where >50% of cells contain non-empty strings AND the next row has similar column count with data values.

#### CSV export (mandatory for data sheets)

Every sheet with tabular data (>10 rows) MUST be exported as a CSV file in `assets/`:

```
assets/
  sheet-inputpricedata.csv
  sheet-calculations.csv
```

In `content.md`, show a truncated preview (first 20 + last 5 rows as markdown table) and reference the full CSV:

```markdown
*Full data: [assets/sheet-inputpricedata.csv](assets/sheet-inputpricedata.csv) (12,700 rows)*
```

#### Data formatting rules for CSV export

1. **Dates** — Always format as `YYYY-MM-DD`. Never use locale-specific formats (MM/DD/YYYY, DD.MM.YYYY). For datetime values, use `YYYY-MM-DD HH:MM:SS`. Apply this in both CSV and markdown tables.

2. **Floating-point numbers** — Use full precision up to 14 significant digits. Strip trailing zeros only after the significant digits are exhausted. The goal is to preserve the exact value Excel stores (IEEE 754 double = ~15.9 significant digits).
   ```python
   def format_float(val):
       """Format float with full precision, strip trailing zeros."""
       if val == int(val) and abs(val) < 1e15:
           return str(int(val))
       # repr gives 17 significant digits (round-trip safe)
       s = f'{val:.14g}'
       return s
   ```
   Examples:
   - `0.034920867792589204` → `0.034920867792589` (14 significant digits)
   - `1.0` → `1` (integer)
   - `0.005` → `0.005`
   - `209.83` → `209.83`

3. **Integer values** — Format without decimal point (`5652`, not `5652.0`).

4. **Empty cells** — Empty string in CSV, empty cell in markdown table.

#### Large data truncation

If a data table exceeds 100 rows in markdown:
- Show first 20 rows + last 5 rows in `content.md`
- Insert a `| ... |` separator row between them
- Note the total row count
- The full data is always available in the CSV export

#### Embedded charts

Use openpyxl to read chart metadata (title, type, data series references). Document in `content.md` as HTML comments:
```markdown
<!-- Chart: "SPY Price" (LineChart) — data from CalculationsAndCharts!E31:E877 -->
```
If LibreOffice is available, render to PNG: `libreoffice --headless --convert-to png file.xlsx`

#### VBA macros (XLSM only)

- Note presence of macros in `manifest.json` under `"has_vba": true`
- If `oletools` (`olevba`) is available, decompile and save to `code/macros.vba`
- Otherwise, just document their existence

#### Multi-sheet output

Produce a single `content.md` with `## Sheet: Name` sections. Each sheet gets its own heading. The CSV exports in `assets/` provide the full data.


### Format-specific extraction

See reference implementations in the `reference/` subdirectory:
- `reference/format-detection.py` — Magic byte detection
- `reference/extract-pdf.py` — PDF text + image extraction
- `reference/extract-pptx.py` — PPTX text + image extraction
- `reference/extract-ppt-legacy.py` — Legacy PPT via OLE
- `reference/extract-html.py` — HTML content extraction
- `reference/extract-mhtml.py` — MHTML parsing
- `reference/extract-docx.py` — DOCX text + image extraction
- `reference/extract-xlsx.py` — XLSX text extraction

## Step 3: Generate Manifest

```json
{
  "source": "<filepath or URL>",
  "format": "<detected format>",
  "extracted_at": "<ISO 8601 timestamp>",
  "assets_count": 3,
  "code_listings_count": 1,
  "assets": ["figure-01.png", "photo-author.jpg", "diagram.svg"],
  "code": ["listing-01.py"],
  "notes": "<any limitations, lost content, or issues>"
}
```

## Step 4: Batch Conversion

When converting multiple related documents from the same source:

1. **Shared assets** — If the same image (e.g., author photo) appears in multiple documents, duplicate it into each output directory. Each extraction should be self-contained.
2. **Cross-references** — If documents link to each other, preserve the links using relative paths between output directories (e.g., `../sma/content.md`).
3. **Common boilerplate** — When processing multiple pages from the same website, identify the common navigation/chrome once and strip it consistently from all pages.

## Limitations by Source Type

| Source | What's extractable | What's NOT extractable |
|--------|-------------------|----------------------|
| PDF | Text, embedded images, tables | Scanned text (needs OCR + tesseract) |
| EPUB | Text, images, code (indented blocks), metadata | DRM-protected content, embedded fonts, audio/video |
| XLSX/XLSM | Cell data, formulas (computed values), sheet structure, chart metadata | Charts as images (needs LibreOffice), VBA source (needs oletools), conditional formatting, pivot tables |
| PPTX | Text, embedded images, speaker notes | Animations, transitions, linked media |
| PPT (legacy) | Images (via OLE), rough text | Precise text layout (use libreoffice conversion) |
| HTML (static) | Text, images, code blocks | — |
| HTML (SPA) | Text if server-rendered | Client-rendered content, interactive charts |
| MHTML | Text, embedded images | External resources not included in archive |
| DOCX | Text, embedded images | Complex layouts, embedded OLE objects |
| XLSX | Cell data as markdown tables | Charts, pivot tables, macros |

## Anti-Patterns (Common Mistakes)

These pitfalls were identified from failed extraction attempts:

1. **Don't write extraction scripts** — Use the available tools directly (WebFetch, Read, Write). Writing Python scripts to `requests.get()` a URL adds complexity and fails on SPAs. The agent already has tools that handle HTTP fetching and markdown conversion.

2. **Don't install headless browsers** — Playwright/Puppeteer require system-level installation and are overkill for most extractions. If WebFetch in markdown mode returns useful content, use it. Document what's missing rather than attempting fragile browser automation.

3. **Watch for backslash corruption** — When LaTeX content passes through Python string handling, `\t` becomes tab, `\n` becomes newline, `\b` becomes backspace. If writing any intermediary scripts, always use raw strings (`r"..."`) or read/write in binary mode. Prefer direct tool-based extraction to avoid this entirely.

4. **Strip rendering artifacts** — SPAs often leave DOM artifacts in extracted text (trailing `#` from anchor links, navigation breadcrumbs, repeated section IDs). Clean these in the final `content.md`.

5. **Don't create empty directories** — If there are no code listings, omit the `code/` directory. If there are no downloadable assets, omit `assets/`. Empty scaffolding adds noise.

6. **Don't use placeholders for missing content** — Instead of `[Chart: screenshot needed]` or `![](assets/placeholder.png)`, use HTML comments that describe what was there: `<!-- Interactive chart: step response for L=5,10,20 -->`. Placeholders look like broken content; comments are invisible to readers.

7. **Prioritize content over tooling** — The goal is readable markdown. Spending time on SVG color-scheme fixes, asset post-processing pipelines, or screenshot checklists is wasted effort if the core text content is corrupted or poorly formatted.

## Required Tools

### System packages (optional, enhance quality)
```bash
sudo apt install poppler-utils tesseract-ocr pandoc libreoffice
```

| Package | Purpose |
|---------|---------|
| `poppler-utils` | PDF rendering (`pdftotext`, `pdftoppm`, `pdfimages`) |
| `tesseract-ocr` | OCR for scanned/image-based PDFs |
| `pandoc` | Universal document converter |
| `libreoffice` | Legacy .ppt/.doc/.xls → modern format conversion |

### Python packages
See `reference/requirements.txt` for the full list.
