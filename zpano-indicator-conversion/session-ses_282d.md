# Organizing indicators folder by author vs indicator name

**Session ID:** ses_282d7ab06ffe49kTQWBc0eM8PR
**Created:** 4/11/2026, 5:28:08 PM
**Updated:** 4/11/2026, 7:15:08 PM

---

## User

I am planning to implement technical analysis trading indicators, like SMA, EMA, RSI etc. All indicators will be located in the "indicators" folder. If a particular indicator was developed by a known author, I will place it in the subfolder named after the author.
For instance, Mark Jurik created JMA and CFB indicators, so JMA implementation will be located in the indicators/mark-jurik/jma/ folder and CFB implementation will be located in the indicators/mark-jurik/cfb/ folder. However, for some well-known indixators, like SMA, the author is unknown. So should i just pit it into indicators/sma folder? Then author names will be mixed with indicator names. Also, I'm planning to develop my own indicators. Should i create an "author name"-like folder for them? 

---

## Assistant (Plan · claude-opus-4.6 · 280.1s)

_Thinking:_

The user is asking for advice on how to organize their folder structure for trading indicators. This is a design/architecture question, not a coding task yet. They have a specific concern:

1. They want to organize indicators by author when the author is known (e.g., `indicators/mark-jurik/jma/`)
2. For well-known indicators with unknown authors (like SMA), they're unsure where to put them — putting them directly in `indicators/sma/` would mix author names with indicator names at the same level
3. They're also wondering how to handle their own custom indicators

This is a good design question. Let me think through the options and give them clear advice.


The real issue here is that mixing author folders with indicator folders at the same level creates ambiguity — you can't immediately tell whether something is an author or an indicator name. I'm considering a few approaches: one option is to always use author folders and create an `unknown/` directory for indicators without clear attribution, so `sma/` and `ema/` would live under `indicators/unknown/`. Another approach would be to separate the structure entirely, keeping author-based indicators in one section and generic indicators in another. I'm weighing the remaining options—#1 and #2 both preserve author grouping, but #2 with a `common/` folder feels cleaner since it avoids the awkward "unknown" label and keeps the structure more semantic. Let me propose that as the solution and get their feedback on the tradeoff between simplicity and losing the physical grouping that metadata-based approaches would offer.

This is a good design question — the core issue is **namespace collision** at the `indicators/` level. If you put SMA directly in `indicators/sma/`, someone browsing the folder sees `mark-jurik/` next to `sma/` and can't tell whether a folder is an author or an indicator.

Here are the main options:

## Option A: Catch-all author folder for unknowns

```
indicators/
├── _common/              # or "classic", "standard", "unknown"
│   ├── sma/
│   ├── ema/
│   └── rsi/
├── mark-jurik/
│   ├── jma/
│   └── cfb/
└── <your-name>/          # your own indicators
    └── my-indicator/
```

**Pro:** Completely consistent — every folder under `indicators/` is an author/category, every indicator is always two levels deep. Easy to glob (`indicators/*/`= authors, `indicators/*/*/` = indicators).

**Con:** Adds a nesting level for the most common indicators. Naming the catch-all (`_common`, `classic`, `standard`) is a minor bikeshed.

## Option B: Flat indicators by default, opt-in author grouping

```
indicators/
├── sma/
├── ema/
├── rsi/
├── mark-jurik/
│   ├── jma/
│   └── cfb/
└── <your-name>/
    └── my-indicator/
```

**Pro:** Simpler for well-known indicators — no extra nesting.

**Con:** Inconsistent depth. You *will* have the namespace collision problem (is `mark-jurik/` an indicator or an author?). Code that walks the tree needs special-case logic.

## Option C: Flat everything, author as metadata

```
indicators/
├── sma/
├── ema/
├── jma/
├── cfb/
└── my-indicator/
```

Author is tracked in code/metadata (e.g., a field in each indicator's module), not in the folder structure.

**Pro:** Simplest structure, uniform depth, no ambiguity. **Con:** Loses the physical author grouping entirely.

---

## My recommendation

**Option A** is the cleanest. It gives you a consistent, predictable structure with no ambiguity. The extra nesting level is a small price for clarity. I'd suggest:

- `_common/` (leading underscore sorts it first and signals "this isn't a person's name")
- Your own name for your custom indicators (e.g., `indicators/john-doe/my-indicator/`) — this is consistent with how you treat every other author and avoids inventing a special `custom/` category

**Tool: question**

**Input:**
```json
{
  "questions": [
    {
      "question": "Which folder structure approach do you prefer?",
      "header": "Folder structure",
      "options": [
        {
          "label": "Option A (Recommended)",
          "description": "Catch-all like _common/ for authorless indicators. Consistent depth, no ambiguity."
        },
        {
          "label": "Option B",
          "description": "Flat by default, author subfolders only when needed. Simpler but inconsistent."
        },
        {
          "label": "Option C",
          "description": "Flat everything, track author in code/metadata only. Simplest but no physical grouping."
        }
      ]
    },
    {
      "question": "For your own custom indicators, how would you like to organize them?",
      "header": "Custom indicators",
      "options": [
        {
          "label": "Use your real name",
          "description": "e.g., indicators/john-doe/my-indicator/ — consistent with other authors"
        },
        {
          "label": "Use a special folder",
          "description": "e.g., indicators/_custom/ or indicators/_experimental/ — clearly separated from published authors"
        }
      ]
    }
  ]
}
```

**Output:**
```
User has answered your questions: "Which folder structure approach do you prefer?"="Option A (Recommended)", "For your own custom indicators, how would you like to organize them?"="Use a special folder". You can now continue with the user's answers in mind.
```

---

## Assistant (Plan · claude-opus-4.6 · 10.6s)

Good choices. So your structure will look like this:

```
indicators/
├── _common/              # Well-known indicators with unknown/no specific author
│   ├── sma/
│   ├── ema/
│   └── rsi/
├── _custom/              # Your own experimental/custom indicators
│   └── my-indicator/
├── mark-jurik/
│   ├── jma/
│   └── cfb/
└── john-bollinger/
    └── bollinger-bands/
```

A few notes on this design:

- **Leading underscores** on `_common/` and `_custom/` serve two purposes: they sort to the top in file listings, and they make it immediately obvious these are *categories*, not author names.
- **Consistent depth**: every indicator is always at `indicators/<group>/<indicator>/`, which makes programmatic discovery straightforward (e.g., a simple glob `indicators/*/*/`).
- **Scalable**: adding a new author or a new common indicator is just a new folder — no structural changes needed.

Let me know when you're ready to start implementing and I can help scaffold the folder structure and initial indicator modules.

---

## User

You mentioned choices for "_common": "_classic", "_standard", "_unknown". Which is better? Also "_custom" or "_experimental"?

---

## Assistant (Plan · claude-opus-4.6 · 29.4s)

_Thinking:_

The user is asking me to help them choose between naming options. This is a naming/semantics discussion. Let me think through the pros and cons of each.

For the "authorless indicators" folder:
- `_common` — implies these are frequently used/popular
- `_classic` — implies these are old/established
- `_standard` — implies these are official/canonical
- `_unknown` — implies the author is not known

For the "your own indicators" folder:
- `_custom` — implies user-created/bespoke
- `_experimental` — implies not yet proven/stable

Let me think about what's most accurate semantically.


The real issue is that these indicators are grouped because their authors are unknown or unattributed, not because they're old or official. "Unknown" is technically accurate but feels dismissive—SMA isn't obscure, it's foundational. "Classic" suggests age when some might just be unattributed. "Standard" works well for implying canonical status, though it might unfairly suggest author-named ones are less official. "Common" feels more neutral—it describes their widespread use without making claims about age or officialdom.

For custom indicators, "Custom" clearly signals user-made and tailored, staying neutral on quality. But "Experimental" might be too limiting—a custom indicator could become stable and battle-tested over time, yet still be custom-built. I'm leaning toward a clearer recommendation here.

These are different semantic claims, so the right choice depends on what the grouping *actually means*.

## `_common` vs `_classic` vs `_standard` vs `_unknown`

| Name | What it implies | Problem |
|---|---|---|
| `_unknown` | Author is not known | Technically accurate for *why* they're here, but sounds dismissive — SMA isn't "unknown," it's foundational |
| `_classic` | Old, established | Subjective and time-dependent. What if a recent indicator has no known author? It's not "classic" |
| `_standard` | Official, canonical | Implies author-attributed indicators are somehow *non*-standard, which isn't true |
| `_common` | Widely used, well-known | Neutral — doesn't claim age, quality, or officialness. Just "these are common, no specific author" |

**Recommendation: `_common`**. It's the most neutral and accurate. The grouping criterion is really "not attributed to a specific author," and `_common` communicates that without implying anything negative or making claims about age/status.

`_unknown` would be my second choice if you want to be strictly precise about the *reason* for the grouping (author unknown), but it reads poorly in a directory listing.

## `_custom` vs `_experimental`

| Name | What it implies | Problem |
|---|---|---|
| `_experimental` | Unstable, unproven, WIP | Too narrow. If your custom indicator matures and becomes battle-tested, it's no longer "experimental" but it's still yours. You'd feel pressure to move it somewhere else. |
| `_custom` | User-created, bespoke | Neutral about stability/maturity. A custom indicator can be experimental *or* production-ready. |

**Recommendation: `_custom`**. It describes the *origin* (you made it), not the *status* (stability). An indicator can graduate from experimental to stable without needing to change folders.

If you *also* want to track stability, that's better handled in code (e.g., a metadata field or a flag) rather than folder structure — mixing two different classification axes (authorship and maturity) into one folder hierarchy gets messy fast.

---

So the final structure stays as:

```
indicators/
├── _common/
├── _custom/
├── mark-jurik/
│   ...
```

Want to proceed with scaffolding this, or are there more design decisions to hash out first?

---

## User

Also, I need a folder containing indicator input and output types, enumerations and interfaces. I thought of indicators/_indicator. Are there better names?

---

## Assistant (Plan · claude-opus-4.6 · 15.3s)



Good question. The `_indicator` name is ambiguous — it looks like it could be an indicator called "indicator." Let me think through the options.

The purpose of this folder is to hold **shared types, interfaces, and enums** that define the contracts for all indicators. That's a different concern than the indicators themselves.

## Options

| Name | Pros | Cons |
|---|---|---|
| `_indicator` | Short | Ambiguous — looks like an indicator named "indicator". Doesn't communicate "shared types" |
| `_types` | Clear about contents | Might be too narrow if you later add shared utilities, base classes, or constants |
| `_core` | Implies foundational building blocks | Broad enough to grow, clearly not an indicator or author |
| `_shared` | Clear "used by everything" meaning | Generic, common in codebases but doesn't hint at *what* is shared |
| `_base` | Implies "base abstractions" | Could be confused with a base class specifically |
| `_contracts` | Precise — interfaces and types are contracts | Unusual naming, might feel overly formal |

## Recommendation: `_core`

It communicates "foundational stuff that everything depends on" without being too narrow (`_types`) or too vague (`_shared`). It naturally accommodates:

- Input/output types and interfaces
- Enums (e.g., signal direction, timeframe)
- Base classes or abstract indicator classes, if you ever need them
- Shared utility functions (e.g., validation, normalization)

The structure becomes:

```
indicators/
├── _common/           # Well-known indicators, no specific author
│   ├── sma/
│   ├── ema/
│   └── rsi/
├── _core/             # Shared types, interfaces, enums
│   ├── types.ts       # (or whatever your language is)
│   ├── enums.ts
│   └── interfaces.ts
├── _custom/           # Your own indicators
│   └── my-indicator/
├── mark-jurik/
│   ├── jma/
│   └── cfb/
```

`_types` would be my second choice if you're confident the folder will *only* ever contain type definitions and nothing else.

What's your preference? And what language are you working in — that might influence the file organization inside `_core`.

---

## User

I'm planning it in TypeScript, Go, Python, Zig and Rust. The proposed scaffolding is file for TypeScript, bur for Go, for instance, we cannot put dashes in the package names, so "mark-jurik" will be "markjurik" or just "jurik". But, of course, scaffolding can be different per language, I only want the basic ideology to be the same.

---

## Assistant (Plan · claude-opus-4.6 · 20.6s)



Good point. The core **ideology** (organizational principles) should be consistent across all languages, while the **scaffolding** (naming conventions, file structure) adapts to each language's idioms.

## Shared Ideology (language-agnostic)

1. **Three underscore-prefixed special folders**: `_core`, `_common`, `_custom`
2. **Author-named folders** for attributed indicators, containing per-indicator subfolders
3. **Consistent depth**: every indicator is at `<root>/<group>/<indicator>/`
4. **`_core` holds shared contracts** (types, interfaces, enums)

## Language-Specific Adaptations

| Concern | TypeScript | Go | Python | Rust | Zig |
|---|---|---|---|---|---|
| Folder naming | `kebab-case` (`mark-jurik`) | `lowercase` no dashes (`markjurik` or `jurik`) | `snake_case` (`mark_jurik`) | `snake_case` (`mark_jurik`) | `snake_case` (`mark_jurik`) |
| Special prefix | `_core`, `_common`, `_custom` | Leading underscore is unusual in Go — could drop it and use `core`, `common`, `custom` | `_core` works but leading underscores imply "private" in Python convention — worth considering | `_core` works, or just `core` | `_core` works, or just `core` |
| Package/module root | `indicators/` | `indicators/` (each subfolder is a package) | `indicators/` (with `__init__.py` files) | `indicators/` (workspace or crate) | `indicators/` |

## The Leading Underscore Problem

This is the biggest cross-language tension. The `_` prefix was chosen because it:
- Sorts special folders to the top
- Visually distinguishes them from author names

But it clashes with conventions in some languages:
- **Python**: `_` prefix means "private/internal" — `_core` would accidentally signal "don't import this," which is the opposite of what you want
- **Go**: unconventional, though technically allowed
- **Rust/Zig**: no strong convention against it, but not idiomatic

Two approaches:

**A) Keep the ideology, adapt the prefix per language.** Use `_core` in TypeScript, `core` in Go/Python/Rust/Zig. The *structure* is identical, only the naming adapts.

**B) Drop the underscore everywhere.** Use `core`, `common`, `custom` in all languages. Rely on the fact that these are clearly not author names. Consistent across all five languages.

I'd lean toward **B** — the names `core`, `common`, `custom` are unambiguous enough on their own, and you get true consistency across all five implementations without fighting language conventions.

```
indicators/
├── common/
│   ├── sma/
│   ├── ema/
│   └── rsi/
├── core/
│   └── ...
├── custom/
│   └── my_indicator/
├── mark_jurik/          # (or markjurik in Go, mark-jurik in TS)
│   ├── jma/
│   └── cfb/
```

The one downside: you lose top-sorting and visual separation. But in practice, `common`, `core`, `custom` all start with "c" and will cluster together alphabetically anyway, which is a happy accident.

What do you think — keep underscore prefix per language (A), or drop it everywhere (B)? And are there any other cross-language concerns you'd like to work through before we finalize the plan?

---

## User

Drop it everywhere (B) is more consistent

---

## Assistant (Plan · claude-opus-4.6 · 11.1s)



Agreed — dropping the underscore everywhere is the cleanest approach. Let me summarize the finalized ideology.

## Final Design

### Principles (all languages)

1. **Three special folders**: `core/`, `common/`, `custom/` — no underscore prefix
2. **Author folders** for attributed indicators, each containing per-indicator subfolders
3. **Consistent depth**: every indicator lives at `indicators/<group>/<indicator>/`
4. **`core/`** holds shared types, interfaces, enums, and foundational abstractions
5. **Naming adapts to language idioms**, structure stays the same

### Canonical Structure

```
indicators/
├── common/                # Well-known, no specific author
│   ├── sma/
│   ├── ema/
│   └── rsi/
├── core/                  # Shared types, interfaces, enums
├── custom/                # Your own indicators
│   └── my-indicator/
├── mark-jurik/            # Author: Mark Jurik
│   ├── jma/
│   └── cfb/
└── john-bollinger/        # Author: John Bollinger
    └── bollinger-bands/
```

### Naming Convention per Language

| Language | Author folder | Indicator folder | Example path |
|---|---|---|---|
| TypeScript | `kebab-case` | `kebab-case` | `indicators/mark-jurik/jma/` |
| Go | `lowercase` (no separators) | `lowercase` | `indicators/markjurik/jma/` |
| Python | `snake_case` | `snake_case` | `indicators/mark_jurik/jma/` |
| Rust | `snake_case` | `snake_case` | `indicators/mark_jurik/jma/` |
| Zig | `snake_case` | `snake_case` | `indicators/mark_jurik/jma/` |

---

Are there other aspects of the design you want to think through before implementation? For example:

- **What goes inside each indicator folder?** (e.g., single file, or a folder with implementation + tests + types?)
- **How does `core/` get structured internally?** (single file per concern, or grouped differently?)
- **Which language do you want to start with?**

---

## User

Each indicator folder will contain indicator implementation, test, parameters interface and maybe some other files. In `core/` folder there will be the `core/frequency-response/` folder and `core/entities/` folder. The first will contain frequency response calculation utilities, the second will contain some output data types (band, heatmap). I doubt should i put band and heatmap there or in the `core/` folder.

---

## Assistant (Plan · claude-opus-4.6 · 17.5s)



Good question. This is about whether `core/entities/` earns its existence as a subfolder, or whether `band` and `heatmap` are simple enough to live directly in `core/`.

## The Tradeoff

**Put them in `core/entities/`:**
```
core/
├── entities/
│   ├── band
│   └── heatmap
└── frequency-response/
```

- Pro: Clean separation if entities grow (e.g., you add `signal`, `crossover`, `zone`, `level` later)
- Pro: `frequency-response/` and `entities/` are parallel subcategories — consistent structure
- Con: Extra nesting for potentially just 2 files

**Put them directly in `core/`:**
```
core/
├── band
├── heatmap
└── frequency-response/
```

- Pro: Simpler, less nesting
- Con: Mixes organizational concerns — `frequency-response/` is a *utility folder* while `band` and `heatmap` are *data type files*. As `core/` grows, it becomes unclear what belongs at the top level vs in a subfolder
- Con: If you later add more output types, `core/` gets cluttered and you'll want to group them anyway

## My Take

The deciding question is: **do you expect more entity types in the future?**

For trading indicators, I'd bet yes — you'll likely end up with types for things like signals, crossover events, histogram data, pivot points, etc. Given that, `core/entities/` is the safer bet. It costs you almost nothing now and prevents a reorganization later.

There's also a consistency argument: if `frequency-response/` gets its own folder because it's a *category of utilities*, then output data types are also a *category* and deserve the same treatment.

**Recommendation: use `core/entities/`.**

```
core/
├── entities/              # Output data types
│   ├── band/
│   └── heatmap/
└── frequency-response/    # Frequency response utilities
```

One more thing to consider — is `entities` the right name? In trading/DSP context, alternatives:

| Name | Implies |
|---|---|
| `entities` | Generic domain objects — broad |
| `outputs` | Specifically indicator output shapes |
| `models` | Data models — common but overloaded term |
| `types` | Type definitions — could clash with a `types` file |

If these are specifically **indicator output shapes** (i.e., "an indicator can output a band, a heatmap, etc."), then `outputs` might be more precise than `entities`. But if they're broader domain objects used for both input and output, `entities` is fine.

What's your thinking — will `band` and `heatmap` always be output types, or could they serve other purposes too?

---

## User

they will be output

---

## Assistant (Plan · claude-opus-4.6 · 25.1s)



Good — then `outputs` is more precise than `entities`. It directly communicates what these types represent.

## Finalized `core/` Structure

```
core/
├── outputs/               # Indicator output data types
│   ├── band/
│   └── heatmap/
└── frequency-response/    # Frequency response calculation utilities
```

## Full Picture

```
indicators/
├── common/                    # Well-known, no specific author
│   ├── sma/
│   │   ├── sma.ts             # Implementation
│   │   ├── sma.test.ts        # Test
│   │   └── sma.params.ts      # Parameters interface
│   ├── ema/
│   └── rsi/
├── core/                      # Shared foundations
│   ├── outputs/               # Output data types (band, heatmap, ...)
│   │   ├── band/
│   │   └── heatmap/
│   └── frequency-response/    # Frequency response utilities
├── custom/                    # Your own indicators
│   └── my-indicator/
├── mark-jurik/
│   ├── jma/
│   └── cfb/
└── john-bollinger/
    └── bollinger-bands/
```

Is there anything else you'd like to work through in the design — like how parameters interfaces relate to `core/`, barrel exports/module structure, or anything else — before we move to implementation?

---

## User

In Go I have indicators/core/outputs/type.go containing
type Type int
const (
	Scalar Type = iota + 1
	Band
	Heatmap
) 
In TypeScript, will indicators/core/outputs/type.enum.ts containing
export enum Type {
  Scalar,
  Band,
  Heatmap
} 
be syntactically correct (enum Type)? What about Python, Zig and Rust? 

---

## Assistant (Plan · claude-opus-4.6 · 19.4s)



Good question. The concern is whether a bare `Type` name works well in each language, considering how it's imported and used at call sites.

## TypeScript

Syntactically correct, yes. But pragmatically problematic:

```typescript
import { Type } from './core/outputs/type.enum';

// At the call site:
Type.Band  // "Type" is extremely vague — type of what?
```

`Type` is also a very common name that could collide with other things. In TypeScript, a better convention:

```typescript
export enum OutputType {
  Scalar,
  Band,
  Heatmap,
}
```

Call site: `OutputType.Band` — immediately clear.

## Go

Your Go version works because Go's convention is `package.Symbol`, so:

```go
import "indicators/core/outputs"

outputs.Band  // reads clearly: "outputs dot Band"
```

The package name provides context, so `Type` is fine inside the `outputs` package. This is idiomatic Go.

## Python

```python
from enum import Enum

class OutputType(Enum):
    SCALAR = 1
    BAND = 2
    HEATMAP = 3
```

`Type` alone would shadow the built-in `type` (Python is case-sensitive, so `Type` != `type`, but it's still confusing and some linters will flag it). Use `OutputType`.

Call site: `OutputType.BAND`

## Rust

```rust
pub enum OutputType {
    Scalar,
    Band,
    Heatmap,
}
```

Rust has a similar situation to Go — you can scope via the module:

```rust
use indicators::core::outputs::OutputType;
OutputType::Band
```

Technically you *could* name it `Type`, but `Type` is not idiomatic in Rust and feels too generic. `OutputType` is clearer.

## Zig

```zig
pub const OutputType = enum(u8) {
    scalar = 1,
    band,
    heatmap,
};
```

Same reasoning — `Type` is too generic. Zig uses `snake_case` for values, `PascalCase` for types.

## Summary

| Language | Bare `Type` works? | Recommendation |
|---|---|---|
| Go | Yes — package name (`outputs.Band`) provides context | Keep `Type` — idiomatic |
| TypeScript | Syntactically yes, but too vague at call sites | Use `OutputType` |
| Python | Shadows built-in `type`, confusing | Use `OutputType` |
| Rust | Legal but not idiomatic | Use `OutputType` |
| Zig | Legal but too generic | Use `OutputType` |

So Go is the outlier here — its package system makes bare `Type` idiomatic. For every other language, `OutputType` is the better choice.

This highlights a broader principle for your cross-language design: **the ideology (the enum exists in `core/outputs/` and describes output kinds) stays the same, but the naming adapts to what reads clearly at each language's call site.**

---

## User

In TypeScript I have naming as foo.enum.ts and bar.interface.ts
Are these `.enum` and `.interface` idiomatic in TypeScript?
In Go you write it just foo.go and bar.go
What about Python, Zig and Rust? 

---

## Assistant (Plan · claude-opus-4.6 · 18.6s)



The `.enum.ts` / `.interface.ts` convention comes from Angular's style guide (e.g., `user.model.ts`, `auth.service.ts`, `role.enum.ts`). It's **idiomatic in Angular projects** but **not standard in the broader TypeScript ecosystem**.

## TypeScript

Outside Angular, the common conventions are:

| Approach | Example | Used by |
|---|---|---|
| Angular-style dot notation | `output-type.enum.ts`, `indicator.interface.ts` | Angular projects |
| Plain descriptive names | `output-type.ts`, `indicator.ts` | Most TS projects (React, Node, libraries) |
| Grouped by concern | `types.ts`, `interfaces.ts`, `enums.ts` | Common for barrel files |

If you're not building an Angular app, the extra `.enum` / `.interface` suffix is unnecessary — TypeScript's type system already makes it clear what's inside a file from the exports. The standard approach is just `output-type.ts`.

That said, if you *like* the explicitness, it's not wrong — just know it's an Angular convention, not a universal TypeScript one.

## Go

As you noted — just `foo.go`. Go has no file-naming convention for content type. The package name provides context. Files are often named by *what they contain* semantically:

```
outputs/
├── type.go          # the OutputType enum (iota)
└── band.go          # Band struct
```

## Python

Standard convention — plain `snake_case.py`, no type suffixes:

```
outputs/
├── __init__.py
├── output_type.py   # the OutputType enum
└── band.py          # Band class
```

No `.enum.py` or `.class.py` convention exists in Python.

## Rust

Rust typically uses plain `snake_case.rs`. Modules are often organized by domain concept, not by language construct:

```
outputs/
├── mod.rs           # module declaration
├── output_type.rs   # the OutputType enum
└── band.rs          # Band struct
```

No `.enum.rs` or `.struct.rs` convention.

## Zig

Same as Rust — plain `snake_case.zig`:

```
outputs/
├── output_type.zig
└── band.zig
```

No type-suffix convention.

## Summary

| Language | File suffix convention? | Idiomatic naming |
|---|---|---|
| TypeScript (Angular) | Yes: `.enum.ts`, `.interface.ts` | `output-type.enum.ts` |
| TypeScript (general) | No | `output-type.ts` |
| Go | No | `type.go` |
| Python | No | `output_type.py` |
| Rust | No | `output_type.rs` |
| Zig | No | `output_type.zig` |

Since you're building a cross-language library (not an Angular app), I'd recommend **dropping the `.enum` / `.interface` suffixes** in TypeScript to stay consistent with the broader ecosystem and with your other four languages. The file name `output-type.ts` is self-explanatory enough.

---

## User

In TypeScript in indicators/core/ I have indicator-type.ts
export enum IndicatorType {
  SimpleMovingAverage
}
and in Go in indicators/core/  I have type.go
type Type int
const (
	SimpleMovingAverage Type = iota + 1
}
This was fine when core/ was indicator/, should I rename type.go to indicatortype.go
type IndicatorType int
const (
	SimpleMovingAverage Type = iota + 1
} 

---

## Assistant (Plan · claude-opus-4.6 · 29.6s)



No, I'd recommend keeping it as `type.go` with `Type` in Go. Here's why:

## Go Convention

In Go, the package name is part of the public API. Consumers write:

```go
import "indicators/core"

core.SimpleMovingAverage
core.Type
```

The package is `core`, and `core.Type` reads as "core type" — meaning "the type of indicator in the core package." Adding `Indicator` creates a stutter:

```go
core.IndicatorType  // "core indicator type" — redundant
```

This is the same reason the Go standard library has `http.Client` (not `http.HttpClient`) and `context.Context` (not `context.ContextContext`). The [Go convention explicitly discourages](https://go.dev/blog/package-names) repeating the package name in symbol names.

The file name `type.go` is also fine — Go doesn't derive any semantic meaning from filenames (unlike, say, Rust's `mod.rs`).

## TypeScript

In TypeScript, there's no package-name scoping at the import site. You import the symbol directly:

```typescript
import { IndicatorType } from './core/indicator-type';

IndicatorType.SimpleMovingAverage
```

So `IndicatorType` is correct here — without the `Indicator` prefix, a bare `Type` would be too vague.

## The Cross-Language Principle

This is the same pattern you already have with `OutputType` vs `outputs.Type`:

| Language | Symbol name | At call site | Reason |
|---|---|---|---|
| Go | `Type` | `core.Type` | Package provides context |
| TypeScript | `IndicatorType` | `IndicatorType` | No package scoping, needs self-describing name |
| Python | `IndicatorType` | `IndicatorType` | Same as TS |
| Rust | `IndicatorType` | `IndicatorType` (or `core::Type` if module-scoped) | Rust *can* use module scoping, but `IndicatorType` is more common |
| Zig | `IndicatorType` | `IndicatorType` | Same reasoning |

**Keep `type.go` / `Type` in Go. Keep `indicator-type.ts` / `IndicatorType` in TypeScript.** The ideology is the same (an enum in `core/` defining indicator kinds), the naming adapts to each language's scoping conventions. This is consistent with the decision you already made for `outputs.Type` vs `OutputType`.

---

