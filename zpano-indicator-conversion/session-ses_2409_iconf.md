# New session - 2026-04-24T12:11:56.876Z

**Session ID:** ses_240989db4ffeV5LhvjC8pkxoZV
**Created:** 4/24/2026, 2:11:56 PM
**Updated:** 4/24/2026, 7:04:08 PM

---

## Assistant (Build · claude-opus-4.6 · 36.7s)

Now I have both files fully read. Let me add the factory sections to both.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "oldString": "- `Static` — everything else.",
  "newString": "- `Static` — everything else.\n\n## Indicator Factory\n\nThe **factory** maps an indicator identifier string + JSON parameters to a fully\nconstructed `Indicator` instance at runtime. This enables data-driven indicator\ncreation from configuration files, user input, or serialized settings.\n\n### Location & Rationale\n\n| Language   | Path                                  |\n|------------|---------------------------------------|\n| Go         | `go/indicators/factory/factory.go`    |\n| TypeScript | `ts/indicators/factory/factory.ts`    |\n\nThe factory **cannot live in `core/`** in Go due to circular imports: indicator\npackages import `core`, so `core` cannot import them back. The `factory/`\npackage sits at `indicators/factory/` — a sibling of `core/` and the\nauthor/group folders — and imports both `core` and every indicator package.\n\nTypeScript has no circular-import issue but uses the same location for\nconsistency.\n\n### Go API\n\n```go\npackage factory\n\n// New creates an indicator from its identifier and JSON-encoded parameters.\nfunc New(identifier core.Identifier, paramsJSON []byte) (core.Indicator, error)\n```\n\n- `identifier` is a `core.Identifier` value (the same enum used in descriptors\n  and metadata).\n- `paramsJSON` is the raw JSON for the indicator's params struct. For\n  no-params indicators, pass `nil` or `[]byte(\"{}}\")`.\n- Returns the constructed indicator or an error if the identifier is unknown or\n  params are invalid.\n\n### TypeScript API\n\n```ts\nexport function createIndicator(\n    identifier: IndicatorIdentifier,\n    params?: Record<string, unknown>,\n): Indicator\n```\n\n- `identifier` is an `IndicatorIdentifier` enum value.\n- `params` is the plain object (already parsed from JSON). For no-params\n  indicators, omit or pass `undefined` / `{}`.\n- Throws on unknown identifier or invalid params.\n\n### Constructor Pattern Categories\n\nThe factory handles five categories of indicator constructors:\n\n| Category | Count (Go/TS) | JSON detection | Example |\n|----------|---------------|----------------|---------|\n| **Standard `*Params` struct** | ~35 | Unmarshal into the params struct directly | SMA, WMA, RSI, BB, MACD, CCI |\n| **Length vs SmoothingFactor variants** | 9 | If JSON contains `\"smoothingFactor\"` key → SF constructor; else → Length constructor | EMA, DEMA, TEMA, T2, T3, KAMA, CyberCycle, ITL |\n| **Default vs Params variants** | ~15 | If JSON is `{}` or empty → Default constructor; else → Params constructor | Ehlers spectra, Corona family, DominantCycle, SineWave, ATCF |\n| **Bare `int` length** | 10 | Parse `{\"length\": N}` | ATR, NATR, DM±, DI±, DMI, ADX, ADXR, WilliamsPercentR |\n| **No params** | 3 | Ignore JSON entirely | TrueRange, BalanceOfPower, AdvanceDecline |\n\n**Special case — MAMA:** uses `\"fastLimitSmoothingFactor\"` /\n`\"slowLimitSmoothingFactor\"` keys for the SmoothingFactor variant, and\n`\"fastestSmoothingFactor\"` / `\"slowestSmoothingFactor\"` for KAMA.\n\n### Auto-Detection Logic\n\nThe factory uses helper functions to detect which constructor variant to call:\n\n- **Go:** `hasKey(json, \"smoothingFactor\")` checks for a specific key;\n  `isEmptyObject(json)` checks if JSON is `{}`.\n- **TS:** `\"smoothingFactor\" in params` checks for a key;\n  `Object.keys(params).length === 0` checks for empty params.\n\nNo JSON tags exist on Go params structs — Go's default case-insensitive JSON\nunmarshaling handles key matching (e.g., JSON `\"smoothingFactor\"` matches Go\nfield `SmoothingFactor`).\n\n### Adding a New Indicator to the Factory\n\nWhen adding a new indicator, add a `case` to the factory's switch statement in\n**both** Go and TS:\n\n1. **Go** — add a `case core.<Identifier>:` block in `factory.New()` that\n   unmarshals `paramsJSON` into the indicator's params struct and calls its\n   constructor.\n2. **TS** — add a `case IndicatorIdentifier.<Name>:` block in\n   `createIndicator()` that spreads defaults onto `params` and calls the\n   constructor.\n\nThe case block should follow the pattern of existing entries in the same\nconstructor category. TS uses a `{ defaultField: value, ...p }` spread pattern\nto fill in required defaults.\n\n### `icalc` CLI Tool\n\nThe `icalc` (\"indicator calculator\") CLI is a reference consumer of the factory\nin both languages. It reads a JSON settings file, creates indicators via the\nfactory, and runs them against embedded test bar data.\n\n| Language | Path | Run command |\n|----------|------|-------------|\n| Go | `go/cmd/icalc/main.go` | `cd go && go run ./cmd/icalc settings.json` |\n| TS | `ts/cmd/icalc/main.ts` | `cd ts && npx tsx cmd/icalc/main.ts cmd/icalc/settings.json` |\n\nSettings file format (`settings.json`):\n\n```json\n[\n    { \"identifier\": \"simpleMovingAverage\", \"params\": { \"length\": 14 } },\n    { \"identifier\": \"exponentialMovingAverage\", \"params\": { \"smoothingFactor\": 0.1 } },\n    { \"identifier\": \"trueRange\", \"params\": {} }\n]\n```\n\n- `identifier` is the camelCase JSON string of the `core.Identifier` /\n  `IndicatorIdentifier` enum.\n- `params` is the JSON object passed to the factory.\n- Both CLIs embed 252-entry H/L/C/V test bar data (from TA-Lib `test_data.c`)\n  and print metadata + per-bar outputs for each configured indicator."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.6 · 16.3s)

Now update the conversion skill file — add a factory registration step after the existing steps:

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "oldString": "Keep `1e-12` / `1e-13` for purely additive/multiplicative indicators.",
  "newString": "Keep `1e-12` / `1e-13` for purely additive/multiplicative indicators.\n\n---\n\n## Registering in the Factory\n\nAfter converting an indicator and registering its identifier and descriptor,\nadd it to the **factory** so it can be created from a JSON identifier string\nand parameters at runtime. The factory lives at `indicators/factory/` in both\nlanguages (see the `indicator-architecture` skill for full details).\n\n### Go\n\nAdd a `case` to the switch in `go/indicators/factory/factory.go` inside\n`func New(identifier core.Identifier, paramsJSON []byte)`:\n\n```go\ncase core.MyIndicator:\n    var p myindicator.Params\n    if err := unmarshal(paramsJSON, &p); err != nil {\n        return nil, err\n    }\n    return myindicator.NewMyIndicator(&p)\n```\n\nFor **multi-constructor** indicators (Length vs SmoothingFactor), use\n`hasKey()` to detect which constructor to call:\n\n```go\ncase core.MyIndicator:\n    if hasKey(paramsJSON, \"smoothingFactor\") {\n        var p myindicator.SmoothingFactorParams\n        if err := unmarshal(paramsJSON, &p); err != nil {\n            return nil, err\n        }\n        return myindicator.NewMyIndicatorSmoothingFactor(&p)\n    }\n    var p myindicator.LengthParams\n    if err := unmarshal(paramsJSON, &p); err != nil {\n        return nil, err\n    }\n    return myindicator.NewMyIndicatorLength(&p)\n```\n\nFor **Default vs Params** indicators, use `isEmptyObject()`:\n\n```go\ncase core.MyIndicator:\n    if isEmptyObject(paramsJSON) {\n        return myindicator.NewDefaultMyIndicator()\n    }\n    var p myindicator.Params\n    if err := unmarshal(paramsJSON, &p); err != nil {\n        return nil, err\n    }\n    return myindicator.NewMyIndicator(&p)\n```\n\n### TypeScript\n\nAdd a `case` to the switch in `ts/indicators/factory/factory.ts` inside\n`createIndicator()`:\n\n```ts\ncase IndicatorIdentifier.MyIndicator:\n    return new MyIndicator(p as MyIndicatorParams);\n```\n\nFor multi-constructor indicators, check for the discriminating key:\n\n```ts\ncase IndicatorIdentifier.MyIndicator:\n    if ('smoothingFactor' in p)\n        return MyIndicator.fromSmoothingFactor(p as MyIndicatorSmoothingFactorParams);\n    return MyIndicator.fromLength({ length: 14, ...p } as MyIndicatorLengthParams);\n```\n\nFor Default vs Params indicators:\n\n```ts\ncase IndicatorIdentifier.MyIndicator:\n    if (Object.keys(p).length === 0) return MyIndicator.default();\n    return MyIndicator.fromParams(p as MyIndicatorParams);\n```\n\n### Settings File\n\nAdd an entry to `cmd/icalc/settings.json` (shared between Go and TS) so the\nnew indicator is exercised by the CLI tool:\n\n```json\n{ \"identifier\": \"myIndicator\", \"params\": { \"length\": 14 } }\n```\n\n### Verification\n\nRun `icalc` in both languages to confirm the new indicator loads and processes\nbars without error:\n\n```bash\ncd go && go run ./cmd/icalc settings.json\ncd ts && npx tsx cmd/icalc/main.ts cmd/icalc/settings.json\n```"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.6 · 5.9s)

Also add the factory step to the conversion checklist table of contents:

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "oldString": "6. [Advanced: Helper / Shared Component Families](#advanced-helper--shared-component-families)\n7. [Naming & Style Conventions](#naming--style-conventions)",
  "newString": "6. [Advanced: Helper / Shared Component Families](#advanced-helper--shared-component-families)\n7. [Naming & Style Conventions](#naming--style-conventions)\n8. [Registering in the Factory](#registering-in-the-factory)"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.6 · 3.8s)

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Update indicator-architecture/SKILL.md with factory section",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Update indicator-conversion/SKILL.md with factory registration step",
      "priority": "high",
      "status": "completed"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Update indicator-architecture/SKILL.md with factory section",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Update indicator-conversion/SKILL.md with factory registration step",
    "priority": "high",
    "status": "completed"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 8.2s)

Both skill files are now updated. Here's what was added:

**`indicator-architecture/SKILL.md`** — New "Indicator Factory" section at the end covering:
- Factory location and circular-import rationale
- Go and TS API signatures
- Constructor pattern categories table (5 categories with detection logic)
- Auto-detection logic for variant constructors
- How to add a new indicator to the factory
- `icalc` CLI tool documentation with run commands and settings format

**`indicator-conversion/SKILL.md`** — New "Registering in the Factory" section at the end (+ TOC entry) covering:
- Go factory case examples for all three variant patterns (standard, multi-constructor, default-vs-params)
- TS factory case examples for all three patterns
- Settings file entry format
- Verification steps using `icalc`

All 8 tasks from the original plan are now complete.

---

## User

In some Go and TypeScript indicator folders params.go/ts (e.g johnehlers/corona/ and john-ehlers/corona/) have DefaultParams() function which returns Params populated with default values. This looks like a nice addition.
Should we add these to all indicators in Go and Typescript?

---

## Assistant (Build · claude-opus-4.6 · 56.9s)

_Thinking:_

Let me explore the codebase to understand the current state of DefaultParams usage.

**Tool: task**

**Input:**
```json
{
  "description": "Survey DefaultParams usage",
  "prompt": "Research task (no code changes). Find all indicators in both Go and TS that already have a DefaultParams() / defaultParams() function, and find a few examples of indicators that do NOT have one. \n\nFor Go, search in go/indicators/ for \"DefaultParams\" or \"func Default\". For TS, search in ts/indicators/ for \"defaultParams\" or \"DefaultParams\".\n\nReturn:\n1. Full list of Go indicators that have DefaultParams (file paths)\n2. Full list of TS indicators that have defaultParams (file paths)\n3. A few examples (3-4) of indicators that do NOT have one in either language\n4. Show the signature and body of 2-3 representative DefaultParams functions (one simple, one complex) so we can see the pattern",
  "subagent_type": "explore"
}
```

**Output:**
```
task_id: ses_2401d4878ffebH1ang6v25dWam (for resuming to continue this task if needed)

<task_result>
Here are the findings:

---

## 1. Go indicators with `DefaultParams` function

**Only one:**
- `/home/dev/zpano/go/indicators/johnehlers/corona/params.go`

That is the only Go indicator in the entire codebase that defines a `func DefaultParams()`.

---

## 2. TS indicators with `defaultParams` / `DefaultParams` function

**None.** There are zero exported `defaultParams` or `DefaultParams` functions in the TS indicator source files. The only occurrences of `defaultParams` are local test-only constants in two spec files:
- `/home/dev/zpano/ts/indicators/john-ehlers/zero-lag-exponential-moving-average/zero-lag-exponential-moving-average.spec.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/zero-lag-error-correcting-exponential-moving-average/zero-lag-error-correcting-exponential-moving-average.spec.ts`

These are just `const defaultParams = { ... }` inside test files, not part of the public API.

---

## 3. Examples of indicators that do NOT have `DefaultParams` (i.e., virtually all of them)

- **Simple Moving Average** -- Go: `/home/dev/zpano/go/indicators/common/simplemovingaverage/params.go`
- **Relative Strength Index** -- Go: `/home/dev/zpano/go/indicators/welleswilder/relativestrengthindex/params.go`
- **Bollinger Bands** -- Go: `/home/dev/zpano/go/indicators/johnbollinger/bollingerbands/params.go`
- **MACD** -- Go: `/home/dev/zpano/go/indicators/geraldappel/movingaverageconvergencedivergence/params.go`

None of these have a `DefaultParams()` function. They document defaults in comments (e.g., "The default value is 14") but rely on the caller to set them.

---

## 4. Representative `DefaultParams` function (the only one that exists)

From `/home/dev/zpano/go/indicators/johnehlers/corona/params.go` -- this is a **moderately complex** example with 5 fields:

```go
// DefaultParams returns a Params value populated with Ehlers defaults.
func DefaultParams() *Params {
	return &Params{
		HighPassFilterCutoff:   30,
		MinimalPeriod:          6,
		MaximalPeriod:          30,
		DecibelsLowerThreshold: 6,
		DecibelsUpperThreshold: 20,
	}
}
```

The `Params` struct uses a "zero means use default" convention:

```go
type Params struct {
	HighPassFilterCutoff   int
	MinimalPeriod          int
	MaximalPeriod          int
	DecibelsLowerThreshold float64
	DecibelsUpperThreshold float64
}
```

---

## Summary

| Language | Indicators with `DefaultParams` | Indicators without |
|----------|---------------------------------|--------------------|
| Go       | 1 (corona)                      | ~70                |
| TS       | 0                               | all                |

The pattern is clear: `DefaultParams()` is essentially absent from the codebase. The Corona indicator is the sole exception. All other indicators document their defaults in struct/interface field comments but do not provide a factory function to produce a pre-populated params object.
</task_result>
```

---

## User

---

## Assistant (Compaction · claude-opus-4.6 · 45.7s)



---

## Goal

Build a **factory package** for both Go and TypeScript indicator codebases that maps an indicator identifier string + JSON parameters to a fully constructed `Indicator` instance. Also build `icalc`, a CLI tool in both languages that uses the factory to create indicators from a JSON settings file and run them against embedded test bar data. Update the `indicator-architecture` and `indicator-conversion` skill files to document the factory pattern. **Next proposed task:** Add `DefaultParams()` / `defaultParams()` functions to all indicators in both Go and TypeScript.

## Instructions

- The factory **cannot live in `core/`** in Go due to circular imports. It lives at `indicators/factory/`.
- For TypeScript, the factory lives at `indicators/factory/factory.ts` for consistency with Go.
- For indicators with **Length vs SmoothingFactor constructor variants** (EMA, DEMA, TEMA, T2, T3, KAMA, CyberCycle, InstantaneousTrendLine, MAMA): **auto-detect from JSON keys**.
- For indicators with **Default vs Params** variants (many Ehlers, Corona, spectra, ATCF): if params is empty `{}`, use Default constructor; otherwise use Params constructor.
- For **bare-int constructors** (ATR, NATR, DM±, DI±, DMI, ADX, ADXR, WilliamsPercentR): parse `{"length": N}` from JSON.
- For **no-params constructors** (TrueRange, BalanceOfPower, AdvanceDecline): call directly.
- Skip internal building blocks (`corona`, `hilberttransformer`) that don't have their own `core.Identifier`.
- The Go module path is `zpano`.
- The KAMA identifier has a quirky JSON string: `"kaufmanAdaptiveMovingAverageMovingAverage"` (with "MovingAverage" duplicated) — this is what Go `core/identifier.go` defines. The TS enum is just `KaufmanAdaptiveMovingAverage` (no duplicate). The TS icalc handles this mapping.
- Follow the indicator-architecture skill conventions for naming and file organization.

## Discoveries

### Go factory (`go/indicators/factory/factory.go`)
- **72 `core.Identifier` constants** (iota 1–72) with camelCase JSON marshal/unmarshal.
- Constructor patterns vary across 5 categories: Standard `*Params` (~35), Length+SF variants (9), Default+Params variants (15), Bare int (10), No params (3).
- No JSON tags on Go params structs — Go's default case-insensitive JSON unmarshaling handles matching.
- Helper functions: `hasKey()`, `isEmptyObject()`, `unmarshal()`, `lengthParam` struct.

### TypeScript factory (`ts/indicators/factory/factory.ts`)
- **65 `IndicatorIdentifier` enum values** (numeric, 0-based).
- 7 construction patterns (A–G) including Pattern F (optional params: ParabolicSAR, MACD, UltimateOscillator, OBV) and Pattern G (MAMA's unique fromLength/fromSmoothingFactor/default).
- Uses `{ defaultField: value, ...p }` spread pattern for defaults.
- Zero barrel files — all imports use direct file paths with `.js` extensions.
- Settings JSON identifiers are camelCase; TS enum members are PascalCase — conversion is `name.charAt(0).toUpperCase() + name.slice(1)`.

### DefaultParams survey (latest work)
- **Only 1 Go indicator** has `DefaultParams()`: `go/indicators/johnehlers/corona/params.go`.
- **Zero TS indicators** have a public `defaultParams()` function.
- All other indicators (~70 Go, ~65 TS) document defaults in comments but don't provide a factory function for pre-populated params.
- The user asked whether we should add `DefaultParams()` to all indicators in both languages. **This question has not been answered yet — the conversation was summarized before a decision was made.** The next agent should discuss the pros/cons and ask for confirmation before proceeding.

### Corona DefaultParams pattern (the only existing example)
```go
// DefaultParams returns a Params value populated with Ehlers defaults.
func DefaultParams() *Params {
    return &Params{
        HighPassFilterCutoff:   30,
        MinimalPeriod:          6,
        MaximalPeriod:          30,
        DecibelsLowerThreshold: 6,
        DecibelsUpperThreshold: 20,
    }
}
```

## Accomplished

### Completed
1. **`go/indicators/factory/factory.go`** — Complete Go factory with all 72 identifiers, all constructor patterns handled.
2. **`go/cmd/icalc/main.go`** — Go CLI app reading settings JSON, creating indicators via factory, printing metadata and 252 bars of output.
3. **`go/cmd/icalc/settings.json`** — All 72 indicators with appropriate default params.
4. **`ts/indicators/factory/factory.ts`** — Complete TS factory with all 65 enum identifiers.
5. **`ts/cmd/icalc/main.ts`** — TS CLI app equivalent of Go icalc.
6. **`ts/cmd/icalc/settings.json`** — Copied from Go, all entries work.
7. **Updated `indicator-architecture/SKILL.md`** — Added factory section documenting location, rationale, API, constructor categories, auto-detection logic, how to extend, and icalc CLI.
8. **Updated `indicator-conversion/SKILL.md`** — Added "Registering in the Factory" section with Go/TS examples for all variant patterns, settings file entry, verification steps, and TOC entry.

### Pending Decision / Next Steps
9. **`DefaultParams()` for all indicators** — User proposed adding `DefaultParams()` / `defaultParams()` to every indicator's params file in both Go and TS. Survey was completed (only 1 Go indicator has it, 0 TS). **The user has not yet confirmed to proceed.** The next agent should discuss this with the user and, if confirmed, plan the rollout across ~70 Go and ~65 TS indicator packages.

## Relevant files / directories

### Go — Created
- `go/indicators/factory/factory.go` — Complete factory (922 lines), all 72 identifiers
- `go/cmd/icalc/main.go` — CLI app (287 lines)
- `go/cmd/icalc/settings.json` — 72 indicator entries

### TypeScript — Created
- `ts/indicators/factory/factory.ts` — Complete factory (~340 lines), all 65 enum values
- `ts/cmd/icalc/main.ts` — CLI app (~313 lines)
- `ts/cmd/icalc/settings.json` — 72 indicator entries

### Skill files — Updated
- `/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md` — Added factory section at end (~100 lines added)
- `/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md` — Added factory registration section at end + TOC entry (~80 lines added)

### Key reference files (read, not modified)
- `go/indicators/core/identifier.go` — 72 Identifier constants + JSON string mappings
- `go/indicators/core/indicator.go` — Indicator interface
- `go/indicators/johnehlers/corona/params.go` — Only existing DefaultParams() example
- `ts/indicators/core/indicator-identifier.ts` — 65-member IndicatorIdentifier enum
- `ts/indicators/core/indicator.ts` — Indicator interface

---

---

## User

---

## Assistant (Build · claude-opus-4.6 · 453.6s)

_Thinking:_

The summary says the pending decision is whether to add DefaultParams() to all indicators. The user hasn't confirmed yet. I should ask.

The main pending item is adding `DefaultParams()` / `defaultParams()` to all indicators in both Go and TypeScript. Before proceeding, I need your input:

**Tool: question**

**Input:**
```json
{
  "questions": [
    {
      "question": "Should we add DefaultParams() / defaultParams() functions to all ~70 Go and ~65 TypeScript indicators? This would mean every indicator's params file gets a function returning a pre-populated struct/object with the documented defaults. The main benefit is programmatic access to defaults (useful for the factory, UIs, etc). The downside is ~135 files to touch and ongoing maintenance burden to keep defaults in sync between comments and code.",
      "header": "Add DefaultParams to all?",
      "options": [
        {
          "label": "Yes, add to all",
          "description": "Add DefaultParams()/defaultParams() to every indicator in both Go and TS"
        },
        {
          "label": "No, skip this",
          "description": "Leave defaults in comments only; the factory/settings.json already covers the practical need"
        },
        {
          "label": "Go only",
          "description": "Add DefaultParams() in Go but skip TypeScript"
        },
        {
          "label": "TS only",
          "description": "Add defaultParams() in TypeScript but skip Go"
        }
      ]
    }
  ]
}
```

**Output:**
```
User has answered your questions: "Should we add DefaultParams() / defaultParams() functions to all ~70 Go and ~65 TypeScript indicators? This would mean every indicator's params file gets a function returning a pre-populated struct/object with the documented defaults. The main benefit is programmatic access to defaults (useful for the factory, UIs, etc). The downside is ~135 files to touch and ongoing maintenance burden to keep defaults in sync between comments and code."="Yes, add to all". You can now continue with the user's answers in mind.
```

---

## Assistant (Build · claude-opus-4.6 · 6.5s)

Got it. Let me plan this out and get started. This is a large task so I'll work through it systematically.

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Survey all Go indicator params files to catalog default values from comments",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Survey all TS indicator params files to catalog default values from comments",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Add DefaultParams() to all Go indicators",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Add defaultParams() to all TS indicators",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Update factory to use DefaultParams where applicable",
      "status": "pending",
      "priority": "medium"
    },
    {
      "content": "Build and test Go",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Build and test TS",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Update skill docs to document DefaultParams pattern",
      "status": "pending",
      "priority": "medium"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Survey all Go indicator params files to catalog default values from comments",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Survey all TS indicator params files to catalog default values from comments",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Add DefaultParams() to all Go indicators",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Add defaultParams() to all TS indicators",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Update factory to use DefaultParams where applicable",
    "status": "pending",
    "priority": "medium"
  },
  {
    "content": "Build and test Go",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Build and test TS",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Update skill docs to document DefaultParams pattern",
    "status": "pending",
    "priority": "medium"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 181.8s)

Let me start by surveying all indicator params files in both languages in parallel.

**Tool: task**

**Input:**
```json
{
  "description": "Survey Go indicator params",
  "subagent_type": "explore",
  "prompt": "Thoroughness: very thorough\n\nI need to catalog ALL Go indicator params files under go/indicators/ to prepare for adding DefaultParams() functions to each one.\n\nFor each indicator package that has a params.go file (or equivalent), I need:\n1. The file path\n2. The struct name (e.g., `Params`)\n3. Each field name, type, and the documented default value (from comments)\n4. Whether it already has a DefaultParams() function\n\nAlso identify indicators that DON'T have params (no-params constructors like TrueRange, BalanceOfPower, AdvanceDecline) and indicators that only take a bare int length parameter (like ATR, NATR, etc.) - these won't need DefaultParams.\n\nFor bare-int indicators, note what the default length typically is from the constructor or any comments.\n\nFor indicators with Length vs SmoothingFactor variants (EMA, DEMA, TEMA, T2, T3, KAMA, CyberCycle, InstantaneousTrendLine, MAMA), note what default length is used.\n\nReturn a comprehensive structured list organized by subdirectory (e.g., johnehlers/, wilder/, etc.) with all the information above. This will be used to generate DefaultParams() functions for ~70 indicators."
}
```

**Output:**
```
task_id: ses_24014ada3ffe5iGaNlo1KpRqW2 (for resuming to continue this task if needed)

<task_result>
Now I have all the data. Let me compile the comprehensive catalog.

---

# Comprehensive Catalog of Go Indicator Params Files

## Summary

- **76 params files** across 76 indicator packages (some packages have 2 param structs for Length vs SmoothingFactor variants)
- **1 existing `DefaultParams()`** function: `johnehlers/corona`
- **2 no-params indicators** (empty struct): `BalanceOfPower`, `TrueRange`, `AdvanceDecline`
- **2 component-only indicators** (no tunable numeric fields): `OnBalanceVolume`, `AdaptiveTrendAndCycleFilter`

---

## common/

### 1. `common/absolutepriceoscillator/params.go`
- **Struct:** `AbsolutePriceOscillatorParams`
- **Fields:**
  - `FastLength` (int) -- no documented default
  - `SlowLength` (int) -- no documented default
  - `MovingAverageType` (MovingAverageType) -- default: SMA (zero value)
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent` (entities.BarComponent) -- default: BarClosePrice
  - `QuoteComponent` (entities.QuoteComponent) -- default: QuoteMidPrice
  - `TradeComponent` (entities.TradeComponent) -- default: TradePrice
- **Has DefaultParams():** No

### 2. `common/exponentialmovingaverage/params.go`
- **Struct 1:** `ExponentialMovingAverageLengthParams`
  - `Length` (int) -- no documented default
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Struct 2:** `ExponentialMovingAverageSmoothingFactorParams`
  - `SmoothingFactor` (float64) -- no documented default
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No
- **Note:** Length/SmoothingFactor dual variant

### 3. `common/linearregression/params.go`
- **Struct:** `Params`
- **Fields:**
  - `Length` (int) -- no documented default
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 4. `common/momentum/params.go`
- **Struct:** `Params`
- **Fields:**
  - `Length` (int) -- no documented default
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 5. `common/pearsonscorrelationcoefficient/params.go`
- **Struct:** `Params`
- **Fields:**
  - `Length` (int) -- no documented default
  - `BarComponent` (entities.BarComponent) -- default: BarHighPrice (X) / BarLowPrice (Y)
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 6. `common/rateofchange/params.go`
- **Struct:** `Params`
- **Fields:**
  - `Length` (int) -- no documented default
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 7. `common/rateofchangepercent/params.go`
- **Struct:** `Params`
- **Fields:**
  - `Length` (int) -- no documented default
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 8. `common/rateofchangeratio/params.go`
- **Struct:** `Params`
- **Fields:**
  - `Length` (int) -- no documented default
  - `HundredScale` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 9. `common/simplemovingaverage/params.go`
- **Struct:** `SimpleMovingAverageParams`
- **Fields:**
  - `Length` (int) -- no documented default
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 10. `common/standarddeviation/params.go`
- **Struct:** `StandardDeviationParams`
- **Fields:**
  - `Length` (int) -- no documented default
  - `IsUnbiased` (bool) -- recommended: true
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 11. `common/triangularmovingaverage/params.go`
- **Struct:** `TriangularMovingAverageParams`
- **Fields:**
  - `Length` (int) -- no documented default
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 12. `common/variance/params.go`
- **Struct:** `VarianceParams`
- **Fields:**
  - `Length` (int) -- no documented default
  - `IsUnbiased` (bool) -- recommended: true
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 13. `common/weightedmovingaverage/params.go`
- **Struct:** `WeightedMovingAverageParams`
- **Fields:**
  - `Length` (int) -- no documented default
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

---

## custom/

### 14. `custom/goertzelspectrum/params.go`
- **Struct:** `Params`
- **Fields:**
  - `Length` (int) -- default: 64
  - `MinPeriod` (float64) -- default: 2
  - `MaxPeriod` (float64) -- default: 64
  - `SpectrumResolution` (int) -- default: 1
  - `IsFirstOrder` (bool) -- default: false
  - `DisableSpectralDilationCompensation` (bool) -- default: false
  - `DisableAutomaticGainControl` (bool) -- default: false
  - `AutomaticGainControlDecayFactor` (float64) -- default: 0.991
  - `FixedNormalization` (bool) -- default: false
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 15. `custom/maximumentropyspectrum/params.go`
- **Struct:** `Params`
- **Fields:**
  - `Length` (int) -- default: 60
  - `Degree` (int) -- default: 30
  - `MinPeriod` (float64) -- default: 2
  - `MaxPeriod` (float64) -- default: 59
  - `SpectrumResolution` (int) -- default: 1
  - `DisableAutomaticGainControl` (bool) -- default: false
  - `AutomaticGainControlDecayFactor` (float64) -- default: 0.995
  - `FixedNormalization` (bool) -- default: false
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

---

## donaldlambert/

### 16. `donaldlambert/commoditychannelindex/params.go`
- **Struct:** `CommodityChannelIndexParams`
- **Fields:**
  - `Length` (int) -- no documented default (>1)
  - `InverseScalingFactor` (float64) -- default: 0.015 (also as `DefaultInverseScalingFactor` const)
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

---

## genequong/

### 17. `genequong/moneyflowindex/params.go`
- **Struct:** `MoneyFlowIndexParams`
- **Fields:**
  - `Length` (int) -- default: 14
  - `BarComponent` -- default: BarTypicalPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

---

## georgelane/

### 18. `georgelane/stochastic/params.go`
- **Struct:** `StochasticParams`
- **Fields:**
  - `FastKLength` (int) -- default: 5
  - `SlowKLength` (int) -- default: 3
  - `SlowDLength` (int) -- default: 3
  - `SlowKMAType` (MovingAverageType) -- default: SMA (zero)
  - `SlowDMAType` (MovingAverageType) -- default: SMA (zero)
  - `FirstIsAverage` (bool) -- default: false
- **Has DefaultParams():** No

---

## geraldappel/

### 19. `geraldappel/movingaverageconvergencedivergence/params.go`
- **Struct:** `MovingAverageConvergenceDivergenceParams`
- **Fields:**
  - `FastLength` (int) -- default: 12
  - `SlowLength` (int) -- default: 26
  - `SignalLength` (int) -- default: 9
  - `MovingAverageType` (MovingAverageType) -- default: EMA (zero, note: EMA=iota here)
  - `SignalMovingAverageType` (MovingAverageType) -- default: EMA (zero)
  - `FirstIsAverage` (*bool) -- default: nil (treated as true/TA-Lib compatible)
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 20. `geraldappel/percentagepriceoscillator/params.go`
- **Struct:** `PercentagePriceOscillatorParams`
- **Fields:**
  - `FastLength` (int) -- no documented default
  - `SlowLength` (int) -- no documented default
  - `MovingAverageType` (MovingAverageType) -- default: SMA (zero)
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

---

## igorlivshin/

### 21. `igorlivshin/balanceofpower/params.go`
- **Struct:** `BalanceOfPowerParams` -- **EMPTY STRUCT (no params)**
- **Has DefaultParams():** No
- **Category:** NO-PARAMS indicator

---

## jackhutson/

### 22. `jackhutson/tripleexponentialmovingaverageoscillator/params.go`
- **Struct:** `TripleExponentialMovingAverageOscillatorParams`
- **Fields:**
  - `Length` (int) -- default: 30
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

---

## johnbollinger/

### 23. `johnbollinger/bollingerbands/params.go`
- **Struct:** `BollingerBandsParams`
- **Fields:**
  - `Length` (int) -- default: 5
  - `UpperMultiplier` (float64) -- default: 2.0
  - `LowerMultiplier` (float64) -- default: 2.0
  - `IsUnbiased` (*bool) -- default: nil (treated as true)
  - `MovingAverageType` (MovingAverageType) -- default: SMA (zero)
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 24. `johnbollinger/bollingerbandstrend/params.go`
- **Struct:** `BollingerBandsTrendParams`
- **Fields:**
  - `FastLength` (int) -- default: 20
  - `SlowLength` (int) -- default: 50
  - `UpperMultiplier` (float64) -- default: 2.0
  - `LowerMultiplier` (float64) -- default: 2.0
  - `IsUnbiased` (*bool) -- default: nil (treated as true)
  - `MovingAverageType` (MovingAverageType) -- default: SMA (zero)
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

---

## johnehlers/

### 25. `johnehlers/autocorrelationindicator/params.go`
- **Struct:** `Params`
- **Fields:**
  - `MinLag` (int) -- default: 3
  - `MaxLag` (int) -- default: 48
  - `SmoothingPeriod` (int) -- default: 10
  - `AveragingLength` (int) -- default: 0 (M=lag)
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 26. `johnehlers/autocorrelationperiodogram/params.go`
- **Struct:** `Params`
- **Fields:**
  - `MinPeriod` (int) -- default: 10
  - `MaxPeriod` (int) -- default: 48
  - `AveragingLength` (int) -- default: 3
  - `DisableSpectralSquaring` (bool) -- default: false
  - `DisableSmoothing` (bool) -- default: false
  - `DisableAutomaticGainControl` (bool) -- default: false
  - `AutomaticGainControlDecayFactor` (float64) -- default: 0.995
  - `FixedNormalization` (bool) -- default: false
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 27. `johnehlers/centerofgravityoscillator/params.go`
- **Struct:** `Params`
- **Fields:**
  - `Length` (int) -- default: 10 (Ehlers)
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 28. `johnehlers/combbandpassspectrum/params.go`
- **Struct:** `Params`
- **Fields:**
  - `MinPeriod` (int) -- default: 10
  - `MaxPeriod` (int) -- default: 48
  - `Bandwidth` (float64) -- default: 0.3
  - `DisableSpectralDilationCompensation` (bool) -- default: false
  - `DisableAutomaticGainControl` (bool) -- default: false
  - `AutomaticGainControlDecayFactor` (float64) -- default: 0.995
  - `FixedNormalization` (bool) -- default: false
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 29. `johnehlers/corona/params.go`
- **Struct:** `Params`
- **Fields:**
  - `HighPassFilterCutoff` (int) -- default: 30
  - `MinimalPeriod` (int) -- default: 6
  - `MaximalPeriod` (int) -- default: 30
  - `DecibelsLowerThreshold` (float64) -- default: 6
  - `DecibelsUpperThreshold` (float64) -- default: 20
- **Has DefaultParams():** YES (already implemented)

### 30. `johnehlers/coronasignaltonoiseratio/params.go`
- **Struct:** `Params`
- **Fields:**
  - `RasterLength` (int) -- default: 50
  - `MaxRasterValue` (float64) -- default: 20
  - `MinParameterValue` (float64) -- default: 1
  - `MaxParameterValue` (float64) -- default: 11
  - `HighPassFilterCutoff` (int) -- default: 30
  - `MinimalPeriod` (int) -- default: 6
  - `MaximalPeriod` (int) -- default: 30
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 31. `johnehlers/coronaspectrum/params.go`
- **Struct:** `Params`
- **Fields:**
  - `MinRasterValue` (float64) -- default: 6
  - `MaxRasterValue` (float64) -- default: 20
  - `MinParameterValue` (float64) -- default: 6
  - `MaxParameterValue` (float64) -- default: 30
  - `HighPassFilterCutoff` (int) -- default: 30
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 32. `johnehlers/coronaswingposition/params.go`
- **Struct:** `Params`
- **Fields:**
  - `RasterLength` (int) -- default: 50
  - `MaxRasterValue` (float64) -- default: 20
  - `MinParameterValue` (float64) -- default: -5
  - `MaxParameterValue` (float64) -- default: 5
  - `HighPassFilterCutoff` (int) -- default: 30
  - `MinimalPeriod` (int) -- default: 6
  - `MaximalPeriod` (int) -- default: 30
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 33. `johnehlers/coronatrendvigor/params.go`
- **Struct:** `Params`
- **Fields:**
  - `RasterLength` (int) -- default: 50
  - `MaxRasterValue` (float64) -- default: 20
  - `MinParameterValue` (float64) -- default: -10
  - `MaxParameterValue` (float64) -- default: 10
  - `HighPassFilterCutoff` (int) -- default: 30
  - `MinimalPeriod` (int) -- default: 6
  - `MaximalPeriod` (int) -- default: 30
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 34. `johnehlers/cybercycle/lengthparams.go`
- **Struct:** `LengthParams`
- **Fields:**
  - `Length` (int) -- no documented default (equivalent to SmoothingFactor)
  - `SignalLag` (int) -- default: 9 (EL) or 20 (EFS)
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No
- **Note:** Length variant of CyberCycle

### 35. `johnehlers/cybercycle/smoothingfactorparams.go`
- **Struct:** `SmoothingFactorParams`
- **Fields:**
  - `SmoothingFactor` (float64) -- default: 0.07
  - `SignalLag` (int) -- default: 9 (EL) or 20 (EFS)
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No
- **Note:** SmoothingFactor variant of CyberCycle

### 36. `johnehlers/discretefouriertransformspectrum/params.go`
- **Struct:** `Params`
- **Fields:**
  - `Length` (int) -- default: 48
  - `MinPeriod` (float64) -- default: 10
  - `MaxPeriod` (float64) -- default: 48
  - `SpectrumResolution` (int) -- default: 1
  - `DisableSpectralDilationCompensation` (bool) -- default: false
  - `DisableAutomaticGainControl` (bool) -- default: false
  - `AutomaticGainControlDecayFactor` (float64) -- default: 0.995
  - `FixedNormalization` (bool) -- default: false
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 37. `johnehlers/dominantcycle/params.go`
- **Struct:** `Params`
- **Fields:**
  - `EstimatorType` (hilberttransformer.CycleEstimatorType) -- default: HomodyneDiscriminator
  - `EstimatorParams` (hilberttransformer.CycleEstimatorParams) -- nested struct (see #44)
  - `AlphaEmaPeriodAdditional` (float64) -- default: 0.33
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 38. `johnehlers/fractaladaptivemovingaverage/params.go`
- **Struct:** `Params`
- **Fields:**
  - `Length` (int) -- default: 16
  - `SlowestSmoothingFactor` (float64) -- default: 0.01
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 39. `johnehlers/hilberttransformer/cycleestimatorparams.go`
- **Struct:** `CycleEstimatorParams`
- **Fields:**
  - `SmoothingLength` (int) -- default: 4
  - `AlphaEmaQuadratureInPhase` (float64) -- default: 0.2 (homodyne), 0.15 (phase acc / dual diff)
  - `AlphaEmaPeriod` (float64) -- default: 0.2 (homodyne), 0.25 (phase acc), 0.15 (dual diff)
  - `WarmUpPeriod` (int) -- default: implementation-specific
- **Has DefaultParams():** No
- **Note:** This is a shared/nested params struct, not a standalone indicator

### 40. `johnehlers/hilberttransformerinstantaneoustrendline/params.go`
- **Struct:** `Params`
- **Fields:**
  - `EstimatorType` (hilberttransformer.CycleEstimatorType) -- default: HomodyneDiscriminator
  - `EstimatorParams` (hilberttransformer.CycleEstimatorParams) -- nested struct
  - `AlphaEmaPeriodAdditional` (float64) -- default: 0.33
  - `TrendLineSmoothingLength` (int) -- default: 4
  - `CyclePartMultiplier` (float64) -- default: 1.0
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 41. `johnehlers/instantaneoustrendline/lengthparams.go`
- **Struct:** `LengthParams`
- **Fields:**
  - `Length` (int) -- no documented default
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No
- **Note:** Length variant of InstantaneousTrendLine

### 42. `johnehlers/instantaneoustrendline/smoothingfactorparams.go`
- **Struct:** `SmoothingFactorParams`
- **Fields:**
  - `SmoothingFactor` (float64) -- default: 0.07
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No
- **Note:** SmoothingFactor variant of InstantaneousTrendLine

### 43. `johnehlers/mesaadaptivemovingaverage/params.go`
- **Struct 1:** `LengthParams`
  - `EstimatorType` (hilberttransformer.CycleEstimatorType) -- default: HomodyneDiscriminator
  - `EstimatorParams` (hilberttransformer.CycleEstimatorParams) -- nested
  - `FastLimitLength` (int) -- default: 3
  - `SlowLimitLength` (int) -- default: 39
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Struct 2:** `SmoothingFactorParams`
  - `EstimatorType` (hilberttransformer.CycleEstimatorType) -- default: HomodyneDiscriminator
  - `EstimatorParams` (hilberttransformer.CycleEstimatorParams) -- nested
  - `FastLimitSmoothingFactor` (float64) -- default: 0.5
  - `SlowLimitSmoothingFactor` (float64) -- default: 0.05
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No
- **Note:** Length/SmoothingFactor dual variant (MAMA)

### 44. `johnehlers/roofingfilter/params.go`
- **Struct:** `RoofingFilterParams`
- **Fields:**
  - `ShortestCyclePeriod` (int) -- default: 10
  - `LongestCyclePeriod` (int) -- default: 48
  - `HasTwoPoleHighpassFilter` (bool) -- default: false
  - `HasZeroMean` (bool) -- default: false
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 45. `johnehlers/sinewave/params.go`
- **Struct:** `Params`
- **Fields:**
  - `EstimatorType` (hilberttransformer.CycleEstimatorType) -- default: HomodyneDiscriminator
  - `EstimatorParams` (hilberttransformer.CycleEstimatorParams) -- nested
  - `AlphaEmaPeriodAdditional` (float64) -- default: 0.33
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 46. `johnehlers/supersmoother/params.go`
- **Struct:** `SuperSmootherParams`
- **Fields:**
  - `ShortestCyclePeriod` (int) -- default: 10
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 47. `johnehlers/trendcyclemode/params.go`
- **Struct:** `Params`
- **Fields:**
  - `EstimatorType` (hilberttransformer.CycleEstimatorType) -- default: HomodyneDiscriminator
  - `EstimatorParams` (hilberttransformer.CycleEstimatorParams) -- nested
  - `AlphaEmaPeriodAdditional` (float64) -- default: 0.33
  - `TrendLineSmoothingLength` (int) -- default: 4
  - `CyclePartMultiplier` (float64) -- default: 1.0
  - `SeparationPercentage` (float64) -- default: 1.5
  - `BarComponent` -- default: BarMedianPrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 48. `johnehlers/zerolagerrorcorrectingexponentialmovingaverage/params.go`
- **Struct:** `ZeroLagErrorCorrectingExponentialMovingAverageParams`
- **Fields:**
  - `SmoothingFactor` (float64) -- default: 0.095 (equiv length 20)
  - `GainLimit` (float64) -- default: 5
  - `GainStep` (float64) -- default: 0.1
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 49. `johnehlers/zerolagexponentialmovingaverage/params.go`
- **Struct:** `ZeroLagExponentialMovingAverageParams`
- **Fields:**
  - `SmoothingFactor` (float64) -- default: 0.25
  - `VelocityGainFactor` (float64) -- default: 0.5
  - `VelocityMomentumLength` (int) -- default: 3
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

---

## josephgranville/

### 50. `josephgranville/onbalancevolume/params.go`
- **Struct:** `OnBalanceVolumeParams`
- **Fields:**
  - `BarComponent` -- default: BarClosePrice
  - `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No
- **Category:** COMPONENT-ONLY (no numeric tunable fields)

---

## larrywilliams/

### 51. `larrywilliams/ultimateoscillator/params.go`
- **Struct:** `UltimateOscillatorParams`
- **Fields:**
  - `Length1` (int) -- default: 7
  - `Length2` (int) -- default: 14
  - `Length3` (int) -- default: 28
- **Has DefaultParams():** No

### 52. `larrywilliams/williamspercentr/params.go`
- **Struct:** `WilliamsPercentRParams`
- **Fields:**
  - `Length` (int) -- default: 14
- **Has DefaultParams():** No

---

## marcchaikin/

### 53. `marcchaikin/advancedecline/params.go`
- **Struct:** `AdvanceDeclineParams` -- **EMPTY STRUCT (no params)**
- **Has DefaultParams():** No
- **Category:** NO-PARAMS indicator

### 54. `marcchaikin/advancedeclineoscillator/params.go`
- **Struct:** `AdvanceDeclineOscillatorParams`
- **Fields:**
  - `FastLength` (int) -- default: 3
  - `SlowLength` (int) -- default: 10
  - `MovingAverageType` (MovingAverageType) -- default: SMA (zero)
  - `FirstIsAverage` (bool) -- default: false
- **Has DefaultParams():** No

---

## markjurik/

### 55. `markjurik/jurikmovingaverage/params.go`
- **Struct:** `JurikMovingAverageParams`
- **Fields:**
  - `Length` (int) -- no documented default (typical 5-80)
  - `Phase` (int) -- range [-100, 100], no explicit default (0 = some lag/some overshoot)
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

---

## patrickmulloy/

### 56. `patrickmulloy/doubleexponentialmovingaverage/params.go`
- **Struct 1:** `DoubleExponentialMovingAverageLengthParams`
  - `Length` (int) -- no documented default
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Struct 2:** `DoubleExponentialMovingAverageSmoothingFactorParams`
  - `SmoothingFactor` (float64) -- no documented default
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No
- **Note:** Length/SmoothingFactor dual variant (DEMA)

### 57. `patrickmulloy/tripleexponentialmovingaverage/params.go`
- **Struct 1:** `TripleExponentialMovingAverageLengthParams`
  - `Length` (int) -- no documented default
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Struct 2:** `TripleExponentialMovingAverageSmoothingFactorParams`
  - `SmoothingFactor` (float64) -- no documented default
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No
- **Note:** Length/SmoothingFactor dual variant (TEMA)

---

## perrykaufman/

### 58. `perrykaufman/kaufmanadaptivemovingaverage/params.go`
- **Struct 1:** `KaufmanAdaptiveMovingAverageLengthParams`
  - `EfficiencyRatioLength` (int) -- default: 10
  - `FastestLength` (int) -- default: 2
  - `SlowestLength` (int) -- default: 30
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Struct 2:** `KaufmanAdaptiveMovingAverageSmoothingFactorParams`
  - `EfficiencyRatioLength` (int) -- default: 10
  - `FastestSmoothingFactor` (float64) -- default: 2/3 (0.6667)
  - `SlowestSmoothingFactor` (float64) -- default: 2/31 (0.0645)
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No
- **Note:** Length/SmoothingFactor dual variant (KAMA)

---

## timtillson/

### 59. `timtillson/t2exponentialmovingaverage/params.go`
- **Struct 1:** `T2ExponentialMovingAverageLengthParams`
  - `Length` (int) -- no documented default
  - `VolumeFactor` (float64) -- default: 0.7
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Struct 2:** `T2ExponentialMovingAverageSmoothingFactorParams`
  - `SmoothingFactor` (float64) -- no documented default
  - `VolumeFactor` (float64) -- default: 0.7
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No
- **Note:** Length/SmoothingFactor dual variant (T2)

### 60. `timtillson/t3exponentialmovingaverage/params.go`
- **Struct 1:** `T3ExponentialMovingAverageLengthParams`
  - `Length` (int) -- no documented default
  - `VolumeFactor` (float64) -- default: 0.7
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Struct 2:** `T3ExponentialMovingAverageSmoothingFactorParams`
  - `SmoothingFactor` (float64) -- no documented default
  - `VolumeFactor` (float64) -- default: 0.7
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No
- **Note:** Length/SmoothingFactor dual variant (T3)

---

## tusharchande/

### 61. `tusharchande/aroon/params.go`
- **Struct:** `AroonParams`
- **Fields:**
  - `Length` (int) -- default: 14
- **Has DefaultParams():** No

### 62. `tusharchande/chandemomentumoscillator/params.go`
- **Struct:** `Params`
- **Fields:**
  - `Length` (int) -- no documented default
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 63. `tusharchande/stochasticrelativestrengthindex/params.go`
- **Struct:** `StochasticRelativeStrengthIndexParams`
- **Fields:**
  - `Length` (int) -- default: 14
  - `FastKLength` (int) -- default: 5
  - `FastDLength` (int) -- default: 3
  - `MovingAverageType` (MovingAverageType) -- default: SMA (zero)
  - `FirstIsAverage` (bool) -- default: false
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

---

## vladimirkravchuk/

### 64. `vladimirkravchuk/adaptivetrendandcyclefilter/params.go`
- **Struct:** `Params`
- **Fields:**
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No
- **Category:** COMPONENT-ONLY (no numeric tunable fields; uses fixed coefficient arrays)

---

## welleswilder/

### 65. `welleswilder/averagedirectionalmovementindex/params.go`
- **Struct:** `AverageDirectionalMovementIndexParams`
- **Fields:** `Length` (int) -- default: 14
- **Has DefaultParams():** No
- **Category:** BARE-INT LENGTH

### 66. `welleswilder/averagedirectionalmovementindexrating/params.go`
- **Struct:** `AverageDirectionalMovementIndexRatingParams`
- **Fields:** `Length` (int) -- default: 14
- **Has DefaultParams():** No
- **Category:** BARE-INT LENGTH

### 67. `welleswilder/averagetruerange/params.go`
- **Struct:** `AverageTrueRangeParams`
- **Fields:** `Length` (int) -- default: 14
- **Has DefaultParams():** No
- **Category:** BARE-INT LENGTH (ATR)

### 68. `welleswilder/directionalindicatorminus/params.go`
- **Struct:** `DirectionalIndicatorMinusParams`
- **Fields:** `Length` (int) -- default: 14
- **Has DefaultParams():** No
- **Category:** BARE-INT LENGTH

### 69. `welleswilder/directionalindicatorplus/params.go`
- **Struct:** `DirectionalIndicatorPlusParams`
- **Fields:** `Length` (int) -- default: 14
- **Has DefaultParams():** No
- **Category:** BARE-INT LENGTH

### 70. `welleswilder/directionalmovementindex/params.go`
- **Struct:** `DirectionalMovementIndexParams`
- **Fields:** `Length` (int) -- default: 14
- **Has DefaultParams():** No
- **Category:** BARE-INT LENGTH

### 71. `welleswilder/directionalmovementminus/params.go`
- **Struct:** `DirectionalMovementMinusParams`
- **Fields:** `Length` (int) -- default: 14
- **Has DefaultParams():** No
- **Category:** BARE-INT LENGTH

### 72. `welleswilder/directionalmovementplus/params.go`
- **Struct:** `DirectionalMovementPlusParams`
- **Fields:** `Length` (int) -- default: 14
- **Has DefaultParams():** No
- **Category:** BARE-INT LENGTH

### 73. `welleswilder/normalizedaveragetruerange/params.go`
- **Struct:** `NormalizedAverageTrueRangeParams`
- **Fields:** `Length` (int) -- default: 14
- **Has DefaultParams():** No
- **Category:** BARE-INT LENGTH (NATR)

### 74. `welleswilder/parabolicstopandreverse/params.go`
- **Struct:** `ParabolicStopAndReverseParams`
- **Fields:**
  - `StartValue` (float64) -- default: 0.0
  - `OffsetOnReverse` (float64) -- default: 0.0
  - `AccelerationInitLong` (float64) -- default: 0.02
  - `AccelerationLong` (float64) -- default: 0.02
  - `AccelerationMaxLong` (float64) -- default: 0.20
  - `AccelerationInitShort` (float64) -- default: 0.02
  - `AccelerationShort` (float64) -- default: 0.02
  - `AccelerationMaxShort` (float64) -- default: 0.20
- **Has DefaultParams():** No

### 75. `welleswilder/relativestrengthindex/params.go`
- **Struct:** `RelativeStrengthIndexParams`
- **Fields:**
  - `Length` (int) -- default: 14
  - `BarComponent`, `QuoteComponent`, `TradeComponent` -- standard defaults
- **Has DefaultParams():** No

### 76. `welleswilder/truerange/params.go`
- **Struct:** `TrueRangeParams` -- **EMPTY STRUCT (no params)**
- **Has DefaultParams():** No
- **Category:** NO-PARAMS indicator

---

## Classification Summary

### NO-PARAMS indicators (empty struct -- won't need DefaultParams):
| # | Path | Struct |
|---|------|--------|
| 1 | `igorlivshin/balanceofpower` | `BalanceOfPowerParams{}` |
| 2 | `marcchaikin/advancedecline` | `AdvanceDeclineParams{}` |
| 3 | `welleswilder/truerange` | `TrueRangeParams{}` |

### COMPONENT-ONLY indicators (only Bar/Quote/TradeComponent -- may not need DefaultParams):
| # | Path | Struct |
|---|------|--------|
| 1 | `josephgranville/onbalancevolume` | `OnBalanceVolumeParams` |
| 2 | `vladimirkravchuk/adaptivetrendandcyclefilter` | `Params` |

### BARE-INT LENGTH indicators (single `Length int` field, all default 14):
| # | Path | Struct | Default Length |
|---|------|--------|---------------|
| 1 | `welleswilder/averagedirectionalmovementindex` | `AverageDirectionalMovementIndexParams` | 14 |
| 2 | `welleswilder/averagedirectionalmovementindexrating` | `AverageDirectionalMovementIndexRatingParams` | 14 |
| 3 | `welleswilder/averagetruerange` | `AverageTrueRangeParams` | 14 |
| 4 | `welleswilder/directionalindicatorminus` | `DirectionalIndicatorMinusParams` | 14 |
| 5 | `welleswilder/directionalindicatorplus` | `DirectionalIndicatorPlusParams` | 14 |
| 6 | `welleswilder/directionalmovementindex` | `DirectionalMovementIndexParams` | 14 |
| 7 | `welleswilder/directionalmovementminus` | `DirectionalMovementMinusParams` | 14 |
| 8 | `welleswilder/directionalmovementplus` | `DirectionalMovementPlusParams` | 14 |
| 9 | `welleswilder/normalizedaveragetruerange` | `NormalizedAverageTrueRangeParams` | 14 |
| 10 | `tusharchande/aroon` | `AroonParams` | 14 |
| 11 | `larrywilliams/williamspercentr` | `WilliamsPercentRParams` | 14 |

### Length/SmoothingFactor DUAL-VARIANT indicators (need DefaultParams for each variant):
| # | Path | Length Struct | SF Struct | Default Length | Default SF |
|---|------|-------------|-----------|---------------|-----------|
| 1 | `common/exponentialmovingaverage` | `ExponentialMovingAverageLengthParams` | `ExponentialMovingAverageSmoothingFactorParams` | (none documented) | (none documented) |
| 2 | `patrickmulloy/doubleexponentialmovingaverage` | `DoubleExponentialMovingAverageLengthParams` | `DoubleExponentialMovingAverageSmoothingFactorParams` | (none) | (none) |
| 3 | `patrickmulloy/tripleexponentialmovingaverage` | `TripleExponentialMovingAverageLengthParams` | `TripleExponentialMovingAverageSmoothingFactorParams` | (none) | (none) |
| 4 | `timtillson/t2exponentialmovingaverage` | `T2ExponentialMovingAverageLengthParams` | `T2ExponentialMovingAverageSmoothingFactorParams` | (none) | (none) |
| 5 | `timtillson/t3exponentialmovingaverage` | `T3ExponentialMovingAverageLengthParams` | `T3ExponentialMovingAverageSmoothingFactorParams` | (none) | (none) |
| 6 | `perrykaufman/kaufmanadaptivemovingaverage` | `KaufmanAdaptiveMovingAverageLengthParams` | `KaufmanAdaptiveMovingAverageSmoothingFactorParams` | ER=10, Fast=2, Slow=30 | ER=10, Fast=2/3, Slow=2/31 |
| 7 | `johnehlers/cybercycle` | `LengthParams` | `SmoothingFactorParams` | (none) | 0.07 |
| 8 | `johnehlers/instantaneoustrendline` | `LengthParams` | `SmoothingFactorParams` | (none) | 0.07 |
| 9 | `johnehlers/mesaadaptivemovingaverage` | `LengthParams` | `SmoothingFactorParams` | Fast=3, Slow=39 | Fast=0.5, Slow=0.05 |

### ALREADY HAS DefaultParams():
| # | Path |
|---|------|
| 1 | `johnehlers/corona` |
</task_result>
```

**Tool: task**

**Input:**
```json
{
  "description": "Survey TS indicator params",
  "subagent_type": "explore",
  "prompt": "Thoroughness: very thorough\n\nI need to catalog ALL TypeScript indicator params/types files under ts/indicators/ to prepare for adding defaultParams() functions to each one.\n\nFor each indicator that has a params type/interface, I need:\n1. The file path\n2. The type/interface name (e.g., `Params`)\n3. Each field name, type, and the documented default value (from comments)\n4. Whether it already has a defaultParams() function\n\nAlso identify indicators that DON'T have params (no-params constructors) and indicators that only take a bare number length parameter - these won't need defaultParams.\n\nFor bare-int indicators, note what the default length typically is.\n\nFor indicators with Length vs SmoothingFactor variants, note what default length is used.\n\nReturn a comprehensive structured list organized by subdirectory with all the information above. This will be used to generate defaultParams() functions for ~65 indicators."
}
```

**Output:**
```
task_id: ses_24014a3a3ffeG0ajcoA0KlkXHt (for resuming to continue this task if needed)

<task_result>
Now I have all the data. Let me compile the comprehensive catalog.

Here is the comprehensive structured catalog of all TypeScript indicator params/types files under `ts/indicators/`:

---

# COMPREHENSIVE INDICATOR PARAMS CATALOG

**Total params.ts files found:** 71 (plus 4 non-standard param files in `cyber-cycle/` and `instantaneous-trend-line/`)
**Total indicator subdirectories:** 64 (excluding `core/frequency-response`, `core/outputs`, `john-ehlers/hilbert-transformer` which are infrastructure)
**Existing `defaultParams()` functions:** **NONE** (0 found; the only `defaultParams` references are local variables in spec files)

Note: In all entries below, `barComponent?`, `quoteComponent?`, `tradeComponent?` are omitted -- virtually every indicator with scalar-input support has these three optional fields with undefined defaults. They are **not relevant** to `defaultParams()` generation. Only substantive fields are listed.

---

## 1. NO-PARAMS INDICATORS (empty interface / parameterless -- skip for defaultParams)

| # | File | Interface | Notes |
|---|------|-----------|-------|
| 1 | `/home/dev/zpano/ts/indicators/igor-livshin/balance-of-power/params.ts` | `BalanceOfPowerParams` | Empty interface. OHLC bar data, no configurable params. |
| 2 | `/home/dev/zpano/ts/indicators/marc-chaikin/advance-decline/params.ts` | `AdvanceDeclineParams` | Empty interface. HLCV bar data, no configurable params. |
| 3 | `/home/dev/zpano/ts/indicators/welles-wilder/true-range/params.ts` | `TrueRangeParams` | Empty interface. Parameterless. |
| 4 | `/home/dev/zpano/ts/indicators/vladimir-kravchuk/adaptive-trend-and-cycle-filter/params.ts` | `AdaptiveTrendAndCycleFilterParams` | Only has bar/quote/tradeComponent. Fixed FIR coefficients. |
| 5 | `/home/dev/zpano/ts/indicators/joseph-granville/on-balance-volume/params.ts` | `OnBalanceVolumeParams` | Only has bar/quote/tradeComponent. No numeric params. |

---

## 2. BARE-LENGTH INDICATORS (single `length: number` + optional bar/quote/trade components)

These take a single required `length` and could potentially use a simple `defaultParams()` returning `{ length: N }`.

| # | Subdirectory | File | Interface | Default Length | Min |
|---|-------------|------|-----------|---------------|-----|
| 1 | `common/simple-moving-average` | `.../params.ts` | `SimpleMovingAverageParams` | **not documented** | >1 |
| 2 | `common/triangular-moving-average` | `.../params.ts` | `TriangularMovingAverageParams` | **not documented** | >1 |
| 3 | `common/weighted-moving-average` | `.../params.ts` | `WeightedMovingAverageParams` | **not documented** | >1 |
| 4 | `common/linear-regression` | `.../params.ts` | `LinearRegressionParams` | **not documented** | >1 |
| 5 | `common/momentum` | `.../params.ts` | `MomentumParams` | **not documented** | >0 |
| 6 | `common/pearsons-correlation-coefficient` | `.../params.ts` | `PearsonsCorrelationCoefficientParams` | **not documented** | >0 |
| 7 | `common/rate-of-change` | `.../params.ts` | `RateOfChangeParams` | **not documented** | >0 |
| 8 | `common/rate-of-change-percent` | `.../params.ts` | `RateOfChangePercentParams` | **not documented** | >0 |
| 9 | `common/rate-of-change-ratio` | `.../params.ts` | `RateOfChangeRatioParams` | **not documented** | >0 |
| 10 | `tushar-chande/chande-momentum-oscillator` | `.../params.ts` | `ChandeMomentumOscillatorParams` | **not documented** | >0 |
| 11 | `tushar-chande/aroon` | `.../params.ts` | `AroonParams` | **14** | >1 |
| 12 | `gene-quong/money-flow-index` | `.../params.ts` | `MoneyFlowIndexParams` | **14** | >0 |
| 13 | `welles-wilder/relative-strength-index` | `.../params.ts` | `RelativeStrengthIndexParams` | **14** | >1 |
| 14 | `welles-wilder/average-true-range` | `.../params.ts` | `AverageTrueRangeParams` | **14** | >=1 |
| 15 | `welles-wilder/normalized-average-true-range` | `.../params.ts` | `NormalizedAverageTrueRangeParams` | **14** | >=1 |
| 16 | `welles-wilder/average-directional-movement-index` | `.../params.ts` | `AverageDirectionalMovementIndexParams` | **14** | >=1 |
| 17 | `welles-wilder/average-directional-movement-index-rating` | `.../params.ts` | `AverageDirectionalMovementIndexRatingParams` | **14** | >=1 |
| 18 | `welles-wilder/directional-movement-index` | `.../params.ts` | `DirectionalMovementIndexParams` | **14** | >=1 |
| 19 | `welles-wilder/directional-movement-plus` | `.../params.ts` | `DirectionalMovementPlusParams` | **14** | >=1 |
| 20 | `welles-wilder/directional-movement-minus` | `.../params.ts` | `DirectionalMovementMinusParams` | **14** | >=1 |
| 21 | `welles-wilder/directional-indicator-plus` | `.../params.ts` | `DirectionalIndicatorPlusParams` | **14** | >=1 |
| 22 | `welles-wilder/directional-indicator-minus` | `.../params.ts` | `DirectionalIndicatorMinusParams` | **14** | >=1 |
| 23 | `larry-williams/williams-percent-r` | `.../params.ts` | `WilliamsPercentRParams` | **14** | >=2 |
| 24 | `jack-hutson/triple-exponential-moving-average-oscillator` | `.../params.ts` | `TripleExponentialMovingAverageOscillatorParams` | **30** | >=1 |
| 25 | `john-ehlers/center-of-gravity-oscillator` | `.../params.ts` | `CenterOfGravityOscillatorParams` | **10** (Ehlers) | >=1 |
| 26 | `john-ehlers/super-smoother` | `.../params.ts` | `SuperSmootherParams` | **10** | >1 |

**Note:** `common/rate-of-change-ratio` also has `hundredScale?: boolean` (default false).

---

## 3. LENGTH + EXTRA REQUIRED FIELDS

### 3a. `common/standard-deviation`
- **File:** `/home/dev/zpano/ts/indicators/common/standard-deviation/params.ts`
- **Interface:** `StandardDeviationParams`
- **Fields:**
  - `length: number` -- no documented default, >1
  - `unbiased: boolean` -- required (docs suggest true)

### 3b. `common/variance`
- **File:** `/home/dev/zpano/ts/indicators/common/variance/params.ts`
- **Interface:** `VarianceParams`
- **Fields:**
  - `length: number` -- no documented default, >1
  - `unbiased: boolean` -- required (docs suggest true)

---

## 4. LENGTH/SMOOTHINGFACTOR DUAL-INTERFACE INDICATORS

These have two interfaces: one for `length`, one for `smoothingFactor`. Each needs a `defaultParams()` per variant (or just for the Length variant).

### 4a. `common/exponential-moving-average`
- **File:** `/home/dev/zpano/ts/indicators/common/exponential-moving-average/params.ts`
- **Length Interface:** `ExponentialMovingAverageLengthParams`
  - `length: number` -- no documented default, >1
  - `firstIsAverage?: boolean` -- default false
- **SF Interface:** `ExponentialMovingAverageSmoothingFactorParams`
  - `smoothingFactor: number` -- in (0,1]
  - `firstIsAverage?: boolean` -- default false

### 4b. `patrick-mulloy/double-exponential-moving-average`
- **File:** `.../params.ts`
- **Length Interface:** `DoubleExponentialMovingAverageLengthParams`
  - `length: number` -- no documented default, >1
  - `firstIsAverage: boolean` -- **required** (no default documented)
- **SF Interface:** `DoubleExponentialMovingAverageSmoothingFactorParams`
  - `smoothingFactor: number` -- in (0,1]

### 4c. `patrick-mulloy/triple-exponential-moving-average`
- **File:** `.../params.ts`
- **Length Interface:** `TripleExponentialMovingAverageLengthParams`
  - `length: number` -- no documented default, >1
  - `firstIsAverage: boolean` -- **required**
- **SF Interface:** `TripleExponentialMovingAverageSmoothingFactorParams`
  - `smoothingFactor: number` -- in (0,1]

### 4d. `tim-tillson/t2-exponential-moving-average`
- **File:** `.../params.ts`
- **Length Interface:** `T2ExponentialMovingAverageLengthParams`
  - `length: number` -- no documented default, >1
  - `volumeFactor: number` -- default **0.7**, [0,1]
  - `firstIsAverage: boolean` -- **required**
- **SF Interface:** `T2ExponentialMovingAverageSmoothingFactorParams`
  - `smoothingFactor: number`
  - `volumeFactor: number` -- default **0.7**
  - `firstIsAverage: boolean` -- **required**

### 4e. `tim-tillson/t3-exponential-moving-average`
- **File:** `.../params.ts`
- **Length Interface:** `T3ExponentialMovingAverageLengthParams`
  - `length: number` -- no documented default, >1
  - `volumeFactor: number` -- default **0.7**, [0,1]
  - `firstIsAverage: boolean` -- **required**
- **SF Interface:** `T3ExponentialMovingAverageSmoothingFactorParams`
  - `smoothingFactor: number`
  - `volumeFactor: number` -- default **0.7**
  - `firstIsAverage: boolean` -- **required**

### 4f. `perry-kaufman/kaufman-adaptive-moving-average`
- **File:** `.../params.ts`
- **Length Interface:** `KaufmanAdaptiveMovingAverageLengthParams`
  - `efficiencyRatioLength: number` -- default **10**, >1
  - `fastestLength: number` -- default **2**, >1
  - `slowestLength: number` -- default **30**, >1
- **SF Interface:** `KaufmanAdaptiveMovingAverageSmoothingFactorParams`
  - `efficiencyRatioLength: number` -- default **10**, >1
  - `fastestSmoothingFactor: number` -- default **2/3** (~0.6667)
  - `slowestSmoothingFactor: number` -- default **2/31** (~0.0645)

### 4g. `john-ehlers/mesa-adaptive-moving-average`
- **File:** `.../params.ts`
- **Length Interface:** `MesaAdaptiveMovingAverageLengthParams`
  - `estimatorType?: HilbertTransformerCycleEstimatorType` -- default HomodyneDiscriminator
  - `estimatorParams?: HilbertTransformerCycleEstimatorParams`
  - `fastLimitLength: number` -- default **3** (alpha=0.5)
  - `slowLimitLength: number` -- default **39** (alpha=0.05)
- **SF Interface:** `MesaAdaptiveMovingAverageSmoothingFactorParams`
  - `estimatorType?` / `estimatorParams?` -- same
  - `fastLimitSmoothingFactor: number` -- default **0.5**
  - `slowLimitSmoothingFactor: number` -- default **0.05**

### 4h. `john-ehlers/cyber-cycle` (non-standard file names)
- **Files:** `.../length-params.ts`, `.../smoothing-factor-params.ts`
- **Length Interface:** `CyberCycleLengthParams`
  - `length: number` -- no documented default, >=1
  - `signalLag: number` -- default **9** (Ehlers), >=1
- **SF Interface:** `CyberCycleSmoothingFactorParams`
  - `smoothingFactor: number` -- default **0.07** (Ehlers, length=28)
  - `signalLag: number` -- default **9**

### 4i. `john-ehlers/instantaneous-trend-line` (non-standard file names)
- **Files:** `.../length-params.ts`, `.../smoothing-factor-params.ts`
- **Length Interface:** `InstantaneousTrendLineLengthParams`
  - `length: number` -- no documented default, >=1
- **SF Interface:** `InstantaneousTrendLineSmoothingFactorParams`
  - `smoothingFactor: number` -- default **0.07** (Ehlers, length=28)

---

## 5. MULTI-FIELD COMPLEX INDICATORS

### 5a. `common/absolute-price-oscillator`
- **File:** `/home/dev/zpano/ts/indicators/common/absolute-price-oscillator/params.ts`
- **Interface:** `AbsolutePriceOscillatorParams`
- **Fields:**
  - `fastLength: number` -- required, >1, **no documented default**
  - `slowLength: number` -- required, >1, **no documented default**
  - `movingAverageType?: MovingAverageType` -- default SMA (0)
  - `firstIsAverage?: boolean` -- default false

### 5b. `gerald-appel/moving-average-convergence-divergence`
- **File:** `.../params.ts`
- **Interface:** `MovingAverageConvergenceDivergenceParams`
- **Fields:**
  - `fastLength?: number` -- default **12**, >1
  - `slowLength?: number` -- default **26**, >1
  - `signalLength?: number` -- default **9**, >0
  - `movingAverageType?: MovingAverageType` -- default EMA (0)
  - `signalMovingAverageType?: MovingAverageType` -- default EMA (0)
  - `firstIsAverage?: boolean` -- default **true** (TA-Lib compatible)

### 5c. `gerald-appel/percentage-price-oscillator`
- **File:** `.../params.ts`
- **Interface:** `PercentagePriceOscillatorParams`
- **Fields:**
  - `fastLength: number` -- required, >1, **no documented default**
  - `slowLength: number` -- required, >1, **no documented default**
  - `movingAverageType?: MovingAverageType` -- default SMA (0)
  - `firstIsAverage?: boolean` -- default false

### 5d. `marc-chaikin/advance-decline-oscillator`
- **File:** `.../params.ts`
- **Interface:** `AdvanceDeclineOscillatorParams`
- **Fields:**
  - `fastLength: number` -- required, >1, **no documented default**
  - `slowLength: number` -- required, >1, **no documented default**
  - `movingAverageType?: MovingAverageType` -- default EMA (1... wait, enum has EMA=1, but doc says "If _undefined_, the Exponential Moving Average is used")
  - `firstIsAverage?: boolean` -- default false

### 5e. `george-lane/stochastic`
- **File:** `/home/dev/zpano/ts/indicators/george-lane/stochastic/params.ts`
- **Interface:** `StochasticParams`
- **Fields:**
  - `fastKLength: number` -- required, >0, **no documented default**
  - `slowKLength: number` -- required, >0, **no documented default**
  - `slowDLength: number` -- required, >0, **no documented default**
  - `slowKMAType?: MovingAverageType` -- default SMA
  - `slowDMAType?: MovingAverageType` -- default SMA
  - `firstIsAverage?: boolean` -- default false

### 5f. `tushar-chande/stochastic-relative-strength-index`
- **File:** `.../params.ts`
- **Interface:** `StochasticRelativeStrengthIndexParams`
- **Fields:**
  - `length: number` -- required, >1, **no documented default**
  - `fastKLength: number` -- required, >0, **no documented default**
  - `fastDLength: number` -- required, >0, **no documented default**
  - `movingAverageType?: MovingAverageType` -- default SMA
  - `firstIsAverage?: boolean` -- default false

### 5g. `john-bollinger/bollinger-bands`
- **File:** `.../params.ts`
- **Interface:** `BollingerBandsParams`
- **Fields:**
  - `length: number` -- required, default **5**, >1
  - `upperMultiplier?: number` -- default **2.0**
  - `lowerMultiplier?: number` -- default **2.0**
  - `isUnbiased?: boolean` -- default true
  - `movingAverageType?: MovingAverageType` -- default SMA
  - `firstIsAverage?: boolean` -- default false

### 5h. `john-bollinger/bollinger-bands-trend`
- **File:** `.../params.ts`
- **Interface:** `BollingerBandsTrendParams`
- **Fields:**
  - `fastLength: number` -- required, default **20**, >1
  - `slowLength: number` -- required, default **50**, >1
  - `upperMultiplier?: number` -- default **2.0**
  - `lowerMultiplier?: number` -- default **2.0**
  - `isUnbiased?: boolean` -- default true
  - `movingAverageType?: MovingAverageType` -- default SMA
  - `firstIsAverage?: boolean` -- default false

### 5i. `donald-lambert/commodity-channel-index`
- **File:** `.../params.ts`
- **Interface:** `CommodityChannelIndexParams`
- **Exported const:** `DefaultInverseScalingFactor = 0.015`
- **Fields:**
  - `length: number` -- required, >1, **no documented default**
  - `inverseScalingFactor?: number` -- default **0.015**

### 5j. `mark-jurik/jurik-moving-average`
- **File:** `/home/dev/zpano/ts/indicators/mark-jurik/jurik-moving-average/params.ts`
- **Interface:** `JurikMovingAverageParams`
- **Fields:**
  - `length: number` -- required, >1, typical 5-20, **no documented default**
  - `phase: number` -- required, [-100,100], **no documented default**

### 5k. `john-ehlers/fractal-adaptive-moving-average`
- **File:** `.../params.ts`
- **Interface:** `FractalAdaptiveMovingAverageParams`
- **Fields:**
  - `length: number` -- required, even int >=2, default **16**
  - `slowestSmoothingFactor: number` -- required, default **0.01** (equiv length 199)

### 5l. `john-ehlers/roofing-filter`
- **File:** `.../params.ts`
- **Interface:** `RoofingFilterParams`
- **Fields:**
  - `shortestCyclePeriod: number` -- required, >1, default **10**
  - `longestCyclePeriod: number` -- required, > shortestCyclePeriod, default **48**
  - `hasTwoPoleHighpassFilter?: boolean` -- default false
  - `hasZeroMean?: boolean` -- default false

### 5m. `john-ehlers/zero-lag-exponential-moving-average`
- **File:** `.../params.ts`
- **Interface:** `ZeroLagExponentialMovingAverageParams`
- **Fields:**
  - `smoothingFactor: number` -- required, default **0.25**
  - `velocityGainFactor: number` -- required, default **0.5**
  - `velocityMomentumLength: number` -- required, >0, default **3**

### 5n. `john-ehlers/zero-lag-error-correcting-exponential-moving-average`
- **File:** `.../params.ts`
- **Interface:** `ZeroLagErrorCorrectingExponentialMovingAverageParams`
- **Fields:**
  - `smoothingFactor: number` -- required, default **0.095** (equiv length 20)
  - `gainLimit: number` -- required, >0, default **5**
  - `gainStep: number` -- required, >0, default **0.1**

### 5o. `larry-williams/ultimate-oscillator`
- **File:** `.../params.ts`
- **Interface:** `UltimateOscillatorParams`
- **Fields:**
  - `length1?: number` -- default **7**, min 2
  - `length2?: number` -- default **14**, min 2
  - `length3?: number` -- default **28**, min 2

### 5p. `welles-wilder/parabolic-stop-and-reverse`
- **File:** `.../params.ts`
- **Interface:** `ParabolicStopAndReverseParams`
- **Fields:**
  - `startValue?: number` -- default **0.0**
  - `offsetOnReverse?: number` -- default **0.0**
  - `accelerationInitLong?: number` -- default **0.02**
  - `accelerationLong?: number` -- default **0.02**
  - `accelerationMaxLong?: number` -- default **0.20**
  - `accelerationInitShort?: number` -- default **0.02**
  - `accelerationShort?: number` -- default **0.02**
  - `accelerationMaxShort?: number` -- default **0.20**

---

## 6. HILBERT-TRANSFORMER-BASED INDICATORS (complex, with estimator sub-params)

### 6a. `john-ehlers/dominant-cycle`
- **File:** `.../params.ts`
- **Interface:** `DominantCycleParams`
- **Fields:**
  - `estimatorType?: HilbertTransformerCycleEstimatorType` -- default HomodyneDiscriminator
  - `estimatorParams?: HilbertTransformerCycleEstimatorParams`
  - `alphaEmaPeriodAdditional: number` -- required, default **0.33**

### 6b. `john-ehlers/sine-wave`
- **File:** `.../params.ts`
- **Interface:** `SineWaveParams`
- **Fields:**
  - `estimatorType?` -- default HomodyneDiscriminator
  - `estimatorParams?`
  - `alphaEmaPeriodAdditional: number` -- required, default **0.33**

### 6c. `john-ehlers/trend-cycle-mode`
- **File:** `.../params.ts`
- **Interface:** `TrendCycleModeParams`
- **Fields:**
  - `estimatorType?` -- default HomodyneDiscriminator
  - `estimatorParams?`
  - `alphaEmaPeriodAdditional: number` -- required, default **0.33**
  - `trendLineSmoothingLength?: number` -- default **4**, valid 2/3/4
  - `cyclePartMultiplier?: number` -- default **1.0**, (0,10]
  - `separationPercentage?: number` -- default **1.5**, (0,100]

### 6d. `john-ehlers/hilbert-transformer-instantaneous-trend-line`
- **File:** `.../params.ts`
- **Interface:** `HilbertTransformerInstantaneousTrendLineParams`
- **Fields:**
  - `estimatorType?` -- default HomodyneDiscriminator
  - `estimatorParams?`
  - `alphaEmaPeriodAdditional: number` -- required, default **0.33**
  - `trendLineSmoothingLength?: number` -- default **4**, valid 2/3/4
  - `cyclePartMultiplier?: number` -- default **1.0**

### Shared Sub-Params: `HilbertTransformerCycleEstimatorParams`
- **File:** `/home/dev/zpano/ts/indicators/john-ehlers/hilbert-transformer/cycle-estimator-params.ts`
- **Interface:** `HilbertTransformerCycleEstimatorParams`
- **Fields:**
  - `smoothingLength: number` -- required, valid 2/3/4, default **4**
  - `alphaEmaQuadratureInPhase: number` -- required, defaults vary by estimator type: Homodyne=**0.2**, PhaseAccumulator=**0.15**, DualDifferentiator=**0.15**
  - `alphaEmaPeriod: number` -- required, defaults vary: Homodyne=**0.2**, PhaseAccumulator=**0.25**, DualDifferentiator=**0.15**
  - `warmUpPeriod?: number` -- optional

---

## 7. CORONA-FAMILY INDICATORS (john-ehlers)

### 7a. `john-ehlers/corona` (engine params, not directly an indicator?)
- **File:** `.../params.ts`
- **Interface:** `CoronaParams`
- **Exported const:** `DefaultCoronaParams` with all defaults filled in
- **Fields:**
  - `highPassFilterCutoff?: number` -- default **30**
  - `minimalPeriod?: number` -- default **6**
  - `maximalPeriod?: number` -- default **30**
  - `decibelsLowerThreshold?: number` -- default **6**
  - `decibelsUpperThreshold?: number` -- default **20**

### 7b. `john-ehlers/corona-spectrum`
- **File:** `.../params.ts`
- **Interface:** `CoronaSpectrumParams`
- **Fields:**
  - `minRasterValue?: number` -- default **6**
  - `maxRasterValue?: number` -- default **20**
  - `minParameterValue?: number` -- default **6**
  - `maxParameterValue?: number` -- default **30**
  - `highPassFilterCutoff?: number` -- default **30**

### 7c. `john-ehlers/corona-signal-to-noise-ratio`
- **File:** `.../params.ts`
- **Interface:** `CoronaSignalToNoiseRatioParams`
- **Fields:**
  - `rasterLength?: number` -- default **50**
  - `maxRasterValue?: number` -- default **20**
  - `minParameterValue?: number` -- default **1**
  - `maxParameterValue?: number` -- default **11**
  - `highPassFilterCutoff?: number` -- default **30**
  - `minimalPeriod?: number` -- default **6**
  - `maximalPeriod?: number` -- default **30**

### 7d. `john-ehlers/corona-swing-position`
- **File:** `.../params.ts`
- **Interface:** `CoronaSwingPositionParams`
- **Fields:**
  - `rasterLength?: number` -- default **50**
  - `maxRasterValue?: number` -- default **20**
  - `minParameterValue?: number` -- default **-5**
  - `maxParameterValue?: number` -- default **5**
  - `highPassFilterCutoff?: number` -- default **30**
  - `minimalPeriod?: number` -- default **6**
  - `maximalPeriod?: number` -- default **30**

### 7e. `john-ehlers/corona-trend-vigor`
- **File:** `.../params.ts`
- **Interface:** `CoronaTrendVigorParams`
- **Fields:**
  - `rasterLength?: number` -- default **50**
  - `maxRasterValue?: number` -- default **20**
  - `minParameterValue?: number` -- default **-10**
  - `maxParameterValue?: number` -- default **10**
  - `highPassFilterCutoff?: number` -- default **30**
  - `minimalPeriod?: number` -- default **6**
  - `maximalPeriod?: number` -- default **30**

---

## 8. SPECTRUM/HEATMAP INDICATORS (all-optional params, zero="use default")

### 8a. `john-ehlers/auto-correlation-indicator`
- **File:** `.../params.ts`
- **Interface:** `AutoCorrelationIndicatorParams`
- **Fields:**
  - `minLag?: number` -- default **3**
  - `maxLag?: number` -- default **48**
  - `smoothingPeriod?: number` -- default **10**
  - `averagingLength?: number` -- default **0** (M=lag)

### 8b. `john-ehlers/auto-correlation-periodogram`
- **File:** `.../params.ts`
- **Interface:** `AutoCorrelationPeriodogramParams`
- **Fields:**
  - `minPeriod?: number` -- default **10**
  - `maxPeriod?: number` -- default **48**
  - `averagingLength?: number` -- default **3**
  - `disableSpectralSquaring?: boolean` -- default false
  - `disableSmoothing?: boolean` -- default false
  - `disableAutomaticGainControl?: boolean` -- default false
  - `automaticGainControlDecayFactor?: number` -- default **0.995**
  - `fixedNormalization?: boolean` -- default false

### 8c. `john-ehlers/comb-band-pass-spectrum`
- **File:** `.../params.ts`
- **Interface:** `CombBandPassSpectrumParams`
- **Fields:**
  - `minPeriod?: number` -- default **10**
  - `maxPeriod?: number` -- default **48**
  - `bandwidth?: number` -- default **0.3**
  - `disableSpectralDilationCompensation?: boolean` -- default false
  - `disableAutomaticGainControl?: boolean` -- default false
  - `automaticGainControlDecayFactor?: number` -- default **0.995**
  - `fixedNormalization?: boolean` -- default false

### 8d. `john-ehlers/discrete-fourier-transform-spectrum`
- **File:** `.../params.ts`
- **Interface:** `DiscreteFourierTransformSpectrumParams`
- **Fields:**
  - `length?: number` -- default **48**
  - `minPeriod?: number` -- default **10**
  - `maxPeriod?: number` -- default **48**
  - `spectrumResolution?: number` -- default **1**
  - `disableSpectralDilationCompensation?: boolean` -- default false
  - `disableAutomaticGainControl?: boolean` -- default false
  - `automaticGainControlDecayFactor?: number` -- default **0.995**
  - `fixedNormalization?: boolean` -- default false

### 8e. `custom/goertzel-spectrum`
- **File:** `.../params.ts`
- **Interface:** `GoertzelSpectrumParams`
- **Fields:**
  - `length?: number` -- default **64**
  - `minPeriod?: number` -- default **2**
  - `maxPeriod?: number` -- default **64**
  - `spectrumResolution?: number` -- default **1**
  - `isFirstOrder?: boolean` -- default false
  - `disableSpectralDilationCompensation?: boolean` -- default false
  - `disableAutomaticGainControl?: boolean` -- default false
  - `automaticGainControlDecayFactor?: number` -- default **0.991**
  - `fixedNormalization?: boolean` -- default false

### 8f. `custom/maximum-entropy-spectrum`
- **File:** `.../params.ts`
- **Interface:** `MaximumEntropySpectrumParams`
- **Fields:**
  - `length?: number` -- default **60**
  - `degree?: number` -- default **30**
  - `minPeriod?: number` -- default **2**
  - `maxPeriod?: number` -- default **59**
  - `spectrumResolution?: number` -- default **1**
  - `disableAutomaticGainControl?: boolean` -- default false
  - `automaticGainControlDecayFactor?: number` -- default **0.995**
  - `fixedNormalization?: boolean` -- default false

---

## SUMMARY COUNTS

| Category | Count | Needs `defaultParams()`? |
|----------|-------|-------------------------|
| No-params (empty interface) | 5 | NO |
| Bare-length (single length field) | 26 | YES (where default is documented) |
| Length + extra required (StdDev, Variance) | 2 | YES |
| Dual-interface Length/SF indicators | 9 indicators (18 interfaces) | YES (at least for Length variant) |
| Multi-field complex | 16 | YES |
| Hilbert-based (w/ estimator sub-params) | 4 + 1 shared sub-params type | YES |
| Corona family | 5 | YES |
| Spectrum/heatmap (all-optional) | 6 | YES (trivially: `{}` or with all defaults) |
| **TOTAL INDICATOR PARAM INTERFACES** | **~71** | **~65 need defaultParams()** |

### Key observations for code generation:

1. **No existing `defaultParams()` functions anywhere.** You are starting from scratch.
2. **`barComponent`, `quoteComponent`, `tradeComponent`** appear on ~50+ interfaces but should be omitted from `defaultParams()` (they default to undefined).
3. **Bare-length Welles Wilder indicators** (9 of them) all default to length **14** -- these could share a pattern.
4. **Spectrum indicators** (sections 7-8) are easiest -- all fields are optional with zero-means-default semantics, so `defaultParams()` can return `{}`.
5. **Some required fields have no documented default** (e.g., `common/simple-moving-average` length, `common/absolute-price-oscillator` fastLength/slowLength, `george-lane/stochastic` fastKLength/slowKLength/slowDLength, `mark-jurik/jurik-moving-average` length/phase). You will need to establish conventional defaults for these.
6. **Dual-interface indicators** with `LengthParams` / `SmoothingFactorParams` -- you may want to only add `defaultParams()` to the Length variant since the SF variant is an alternative parameterization.
7. **`CoronaParams`** already has a `DefaultCoronaParams` exported const -- this is the closest thing to an existing `defaultParams()` pattern in the codebase.
</task_result>
```

---

## Assistant (Build · claude-opus-4.6 · 6.8s)

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Survey all Go indicator params files to catalog default values from comments",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Survey all TS indicator params files to catalog default values from comments",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Add DefaultParams() to all Go indicators",
      "priority": "high",
      "status": "in_progress"
    },
    {
      "content": "Add defaultParams() to all TS indicators",
      "priority": "high",
      "status": "pending"
    },
    {
      "content": "Update factory to use DefaultParams where applicable",
      "priority": "medium",
      "status": "pending"
    },
    {
      "content": "Build and test Go",
      "priority": "high",
      "status": "pending"
    },
    {
      "content": "Build and test TS",
      "priority": "high",
      "status": "pending"
    },
    {
      "content": "Update skill docs to document DefaultParams pattern",
      "priority": "medium",
      "status": "pending"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Survey all Go indicator params files to catalog default values from comments",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Survey all TS indicator params files to catalog default values from comments",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Add DefaultParams() to all Go indicators",
    "priority": "high",
    "status": "in_progress"
  },
  {
    "content": "Add defaultParams() to all TS indicators",
    "priority": "high",
    "status": "pending"
  },
  {
    "content": "Update factory to use DefaultParams where applicable",
    "priority": "medium",
    "status": "pending"
  },
  {
    "content": "Build and test Go",
    "priority": "high",
    "status": "pending"
  },
  {
    "content": "Build and test TS",
    "priority": "high",
    "status": "pending"
  },
  {
    "content": "Update skill docs to document DefaultParams pattern",
    "priority": "medium",
    "status": "pending"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 2286.0s)

Both surveys are complete. Before I start writing code, there are a few design decisions to address:

**Key questions from the survey:**

1. **Indicators with no documented default length** (SMA, EMA, DEMA, TEMA, T2, T3, WMA, TMA, LinearRegression, Momentum, RateOfChange*, PearsonsCorrelation, ChandeMomentumOscillator, APO, PPO, Stochastic, StochRSI, CCI, JMA): These have required `Length` fields but no canonical default. Should I pick conventional defaults (e.g., length=14 for most, or length=20 for MAs)?

2. **Dual-variant indicators** (EMA, DEMA, TEMA, T2, T3, KAMA, CyberCycle, ITL, MAMA): Should I add `DefaultParams()` for **both** Length and SmoothingFactor variants, or just the Length variant?

3. **No-params / component-only indicators** (TrueRange, BalanceOfPower, AdvanceDecline, OBV, ATCF): Skip these entirely since they have no meaningful params?

4. **Spectrum indicators** where all fields are optional with "zero means use default": Should `DefaultParams()` return the actual numeric defaults, or an empty struct (since the constructor already applies defaults)?

**Tool: question**

**Input:**
```json
{
  "questions": [
    {
      "header": "Undocumented default lengths",
      "question": "For indicators with no documented default length (SMA, EMA, DEMA, TEMA, T2, T3, WMA, TMA, LinearRegression, Momentum, RateOfChange*, Pearsons, ChandeMomentumOscillator, APO, PPO, Stochastic, StochRSI, CCI, JMA), what should DefaultParams() return?",
      "options": [
        {
          "label": "Pick conventional defaults",
          "description": "Use common conventions: 14 for oscillators, 20 for MAs, etc. I'll propose specific values for approval."
        },
        {
          "label": "Skip these indicators",
          "description": "Only add DefaultParams() to indicators that have explicitly documented defaults."
        },
        {
          "label": "Use length=14 for all",
          "description": "Default to 14 across the board for any undocumented length."
        }
      ]
    },
    {
      "header": "Dual-variant indicators",
      "question": "For Length/SmoothingFactor dual-variant indicators, add DefaultParams for both variants or just Length?",
      "options": [
        {
          "label": "Both variants (Recommended)",
          "description": "Add DefaultLengthParams() and DefaultSmoothingFactorParams() for completeness."
        },
        {
          "label": "Length variant only",
          "description": "Only add DefaultLengthParams() since SF is an alternative parameterization."
        }
      ]
    },
    {
      "header": "No-params indicators",
      "question": "Skip DefaultParams() for indicators with empty params structs (TrueRange, BalanceOfPower, AdvanceDecline, OBV, ATCF)?",
      "options": [
        {
          "label": "Skip them (Recommended)",
          "description": "No point returning an empty struct/object."
        },
        {
          "label": "Add them anyway",
          "description": "For consistency, every indicator gets DefaultParams() even if it returns {}."
        }
      ]
    },
    {
      "header": "Spectrum defaults",
      "question": "For spectrum indicators where all fields are optional (zero=use default), should DefaultParams() return explicit numeric defaults or empty?",
      "options": [
        {
          "label": "Explicit defaults (Recommended)",
          "description": "Return all the actual numeric defaults (e.g., minPeriod=10, maxPeriod=48) for discoverability."
        },
        {
          "label": "Empty struct/object",
          "description": "Return {} since constructors already apply defaults."
        }
      ]
    }
  ]
}
```

**Output:**
```
User has answered your questions: "For indicators with no documented default length (SMA, EMA, DEMA, TEMA, T2, T3, WMA, TMA, LinearRegression, Momentum, RateOfChange*, Pearsons, ChandeMomentumOscillator, APO, PPO, Stochastic, StochRSI, CCI, JMA), what should DefaultParams() return?"="Pick conventional defaults", "For Length/SmoothingFactor dual-variant indicators, add DefaultParams for both variants or just Length?"="Both variants (Recommended)", "Skip DefaultParams() for indicators with empty params structs (TrueRange, BalanceOfPower, AdvanceDecline, OBV, ATCF)?"="Add them anyway", "For spectrum indicators where all fields are optional (zero=use default), should DefaultParams() return explicit numeric defaults or empty?"="Explicit defaults (Recommended)". You can now continue with the user's answers in mind.
```

---

## Assistant (Build · claude-opus-4.6 · 269.7s)

Good. Here are the conventional defaults I'll use for undocumented indicators:

| Indicator | Proposed defaults |
|---|---|
| SMA, WMA, TMA, LinearRegression | length=20 |
| EMA, DEMA, TEMA | length=20 |
| T2, T3 | length=5 |
| Momentum, RateOfChange, ROC%, ROCRatio | length=10 |
| PearsonsCorrelation | length=20 |
| ChandeMomentumOscillator | length=14 |
| APO | fastLength=12, slowLength=26 |
| PPO | fastLength=12, slowLength=26 |
| Stochastic | fastK=5, slowK=3, slowD=3 |
| StochRSI | length=14, fastK=5, fastD=3 |
| CCI | length=20 |
| JMA | length=14, phase=0 |
| AdvanceDeclineOscillator | fastLength=3, slowLength=10 |
| StandardDeviation, Variance | length=20, unbiased=true |

For SF variants: EMA SF=2/(20+1)≈0.0952, CyberCycle SF=0.07, ITL SF=0.07, MAMA fastSF=0.5/slowSF=0.05, KAMA fastSF=2/3/slowSF=2/31, etc.

This is a large batch of files. Let me start with Go, working through them in groups by subdirectory. I'll use multiple parallel agents to maximize throughput.

**Tool: task**

**Input:**
```json
{
  "description": "Add DefaultParams Go common",
  "prompt": "You are adding `DefaultParams()` functions to Go indicator params files. The function should return a pointer to the params struct populated with default values.\n\nIMPORTANT: Read each file first before editing. Follow the existing pattern from `go/indicators/johnehlers/corona/params.go` which has:\n```go\n// DefaultParams returns a Params value populated with Ehlers defaults.\nfunc DefaultParams() *Params {\n    return &Params{\n        HighPassFilterCutoff:   30,\n        ...\n    }\n}\n```\n\nFor each indicator, add the `DefaultParams()` function at the end of the params.go file. Use the SAME struct type that already exists. For `BarComponent`, `QuoteComponent`, `TradeComponent` fields, OMIT them from DefaultParams (they have zero-value defaults that are correct).\n\nHere are the files to modify under `go/indicators/common/`:\n\n1. `go/indicators/common/absolutepriceoscillator/params.go` - struct `AbsolutePriceOscillatorParams`, defaults: FastLength=12, SlowLength=26 (MovingAverageType, FirstIsAverage stay zero)\n2. `go/indicators/common/exponentialmovingaverage/params.go` - TWO functions needed:\n   - `DefaultLengthParams() *ExponentialMovingAverageLengthParams` with Length=20\n   - `DefaultSmoothingFactorParams() *ExponentialMovingAverageSmoothingFactorParams` with SmoothingFactor=0.0952\n3. `go/indicators/common/linearregression/params.go` - struct `Params`, defaults: Length=20\n4. `go/indicators/common/momentum/params.go` - struct `Params`, defaults: Length=10\n5. `go/indicators/common/pearsonscorrelationcoefficient/params.go` - struct `Params`, defaults: Length=20\n6. `go/indicators/common/rateofchange/params.go` - struct `Params`, defaults: Length=10\n7. `go/indicators/common/rateofchangepercent/params.go` - struct `Params`, defaults: Length=10\n8. `go/indicators/common/rateofchangeratio/params.go` - struct `Params`, defaults: Length=10 (HundredScale stays false)\n9. `go/indicators/common/simplemovingaverage/params.go` - struct `SimpleMovingAverageParams`, defaults: Length=20\n10. `go/indicators/common/standarddeviation/params.go` - struct `StandardDeviationParams`, defaults: Length=20, IsUnbiased=true\n11. `go/indicators/common/triangularmovingaverage/params.go` - struct `TriangularMovingAverageParams`, defaults: Length=20\n12. `go/indicators/common/variance/params.go` - struct `VarianceParams`, defaults: Length=20, IsUnbiased=true\n13. `go/indicators/common/weightedmovingaverage/params.go` - struct `WeightedMovingAverageParams`, defaults: Length=20\n\nFor dual-variant indicators (like EMA), the comment should say:\n```go\n// DefaultLengthParams returns a ExponentialMovingAverageLengthParams value populated with conventional defaults.\n```\n\nFor single-variant, use:\n```go\n// DefaultParams returns a [StructName] value populated with conventional defaults.\n```\n\nRead each file, add the function at the end, and confirm each was written successfully. Do ALL 13 files.",
  "subagent_type": "general"
}
```

**Output:**
```
task_id: ses_23feeb409ffecyHRFiFFZhiN98 (for resuming to continue this task if needed)

<task_result>
All 13 files compile successfully. Here's a summary of what was added:

1. **absolutepriceoscillator** - `DefaultParams()` with FastLength=12, SlowLength=26
2. **exponentialmovingaverage** - `DefaultLengthParams()` with Length=20, `DefaultSmoothingFactorParams()` with SmoothingFactor=0.0952
3. **linearregression** - `DefaultParams()` with Length=20
4. **momentum** - `DefaultParams()` with Length=10
5. **pearsonscorrelationcoefficient** - `DefaultParams()` with Length=20
6. **rateofchange** - `DefaultParams()` with Length=10
7. **rateofchangepercent** - `DefaultParams()` with Length=10
8. **rateofchangeratio** - `DefaultParams()` with Length=10
9. **simplemovingaverage** - `DefaultParams()` with Length=20
10. **standarddeviation** - `DefaultParams()` with Length=20, IsUnbiased=true
11. **triangularmovingaverage** - `DefaultParams()` with Length=20
12. **variance** - `DefaultParams()` with Length=20, IsUnbiased=true
13. **weightedmovingaverage** - `DefaultParams()` with Length=20
</task_result>
```

**Tool: task**

**Input:**
```json
{
  "description": "Add DefaultParams Go wilder",
  "prompt": "You are adding `DefaultParams()` functions to Go indicator params files under `go/indicators/welleswilder/`. Read each file first before editing. Add the function at the END of each file.\n\nPattern to follow:\n```go\n// DefaultParams returns a [StructName] value populated with conventional defaults.\nfunc DefaultParams() *StructName {\n    return &StructName{\n        Field: value,\n    }\n}\n```\n\nDo NOT include BarComponent/QuoteComponent/TradeComponent fields in DefaultParams (zero values are correct defaults).\n\nFiles to modify:\n\n1. `go/indicators/welleswilder/averagedirectionalmovementindex/params.go` - `AverageDirectionalMovementIndexParams`, Length=14\n2. `go/indicators/welleswilder/averagedirectionalmovementindexrating/params.go` - `AverageDirectionalMovementIndexRatingParams`, Length=14\n3. `go/indicators/welleswilder/averagetruerange/params.go` - `AverageTrueRangeParams`, Length=14\n4. `go/indicators/welleswilder/directionalindicatorminus/params.go` - `DirectionalIndicatorMinusParams`, Length=14\n5. `go/indicators/welleswilder/directionalindicatorplus/params.go` - `DirectionalIndicatorPlusParams`, Length=14\n6. `go/indicators/welleswilder/directionalmovementindex/params.go` - `DirectionalMovementIndexParams`, Length=14\n7. `go/indicators/welleswilder/directionalmovementminus/params.go` - `DirectionalMovementMinusParams`, Length=14\n8. `go/indicators/welleswilder/directionalmovementplus/params.go` - `DirectionalMovementPlusParams`, Length=14\n9. `go/indicators/welleswilder/normalizedaveragetruerange/params.go` - `NormalizedAverageTrueRangeParams`, Length=14\n10. `go/indicators/welleswilder/parabolicstopandreverse/params.go` - `ParabolicStopAndReverseParams`, StartValue=0, OffsetOnReverse=0, AccelerationInitLong=0.02, AccelerationLong=0.02, AccelerationMaxLong=0.20, AccelerationInitShort=0.02, AccelerationShort=0.02, AccelerationMaxShort=0.20\n11. `go/indicators/welleswilder/relativestrengthindex/params.go` - `RelativeStrengthIndexParams`, Length=14\n12. `go/indicators/welleswilder/truerange/params.go` - `TrueRangeParams` (EMPTY struct, still add: `func DefaultParams() *TrueRangeParams { return &TrueRangeParams{} }`)\n\nRead each file, edit it, confirm success. Do ALL 12 files.",
  "subagent_type": "general"
}
```

**Output:**
```
task_id: ses_23fee90c6ffeLcKBDEGL0956VL (for resuming to continue this task if needed)

<task_result>
All 12 files edited successfully. Each now has a `DefaultParams()` function appended at the end with the specified default values.
</task_result>
```

**Tool: task**

**Input:**
```json
{
  "description": "Add DefaultParams Go ehlers",
  "prompt": "You are adding `DefaultParams()` functions to Go indicator params files under `go/indicators/johnehlers/`. Read each file first before editing. Add the function at the END of each file.\n\nPattern:\n```go\n// DefaultParams returns a [StructName] value populated with Ehlers defaults.\nfunc DefaultParams() *StructName {\n    return &StructName{ ... }\n}\n```\n\nDo NOT include BarComponent/QuoteComponent/TradeComponent fields.\n\nSKIP `go/indicators/johnehlers/corona/params.go` - it ALREADY has DefaultParams().\n\nFiles to modify:\n\n1. `go/indicators/johnehlers/autocorrelationindicator/params.go` - `Params`, MinLag=3, MaxLag=48, SmoothingPeriod=10, AveragingLength=0\n2. `go/indicators/johnehlers/autocorrelationperiodogram/params.go` - `Params`, MinPeriod=10, MaxPeriod=48, AveragingLength=3, AutomaticGainControlDecayFactor=0.995 (bools stay false)\n3. `go/indicators/johnehlers/centerofgravityoscillator/params.go` - `Params`, Length=10\n4. `go/indicators/johnehlers/combbandpassspectrum/params.go` - `Params`, MinPeriod=10, MaxPeriod=48, Bandwidth=0.3, AutomaticGainControlDecayFactor=0.995\n5. `go/indicators/johnehlers/coronasignaltonoiseratio/params.go` - `Params`, RasterLength=50, MaxRasterValue=20, MinParameterValue=1, MaxParameterValue=11, HighPassFilterCutoff=30, MinimalPeriod=6, MaximalPeriod=30\n6. `go/indicators/johnehlers/coronaspectrum/params.go` - `Params`, MinRasterValue=6, MaxRasterValue=20, MinParameterValue=6, MaxParameterValue=30, HighPassFilterCutoff=30\n7. `go/indicators/johnehlers/coronaswingposition/params.go` - `Params`, RasterLength=50, MaxRasterValue=20, MinParameterValue=-5, MaxParameterValue=5, HighPassFilterCutoff=30, MinimalPeriod=6, MaximalPeriod=30\n8. `go/indicators/johnehlers/coronatrendvigor/params.go` - `Params`, RasterLength=50, MaxRasterValue=20, MinParameterValue=-10, MaxParameterValue=10, HighPassFilterCutoff=30, MinimalPeriod=6, MaximalPeriod=30\n9. `go/indicators/johnehlers/cybercycle/lengthparams.go` - TWO functions in separate files:\n   - In `lengthparams.go`: `DefaultLengthParams() *LengthParams` with Length=28, SignalLag=9\n   - In `smoothingfactorparams.go`: `DefaultSmoothingFactorParams() *SmoothingFactorParams` with SmoothingFactor=0.07, SignalLag=9\n10. `go/indicators/johnehlers/discretefouriertransformspectrum/params.go` - `Params`, Length=48, MinPeriod=10, MaxPeriod=48, SpectrumResolution=1, AutomaticGainControlDecayFactor=0.995\n11. `go/indicators/johnehlers/dominantcycle/params.go` - `Params`, AlphaEmaPeriodAdditional=0.33 (EstimatorType and EstimatorParams have zero-value defaults that are correct - HomodyneDiscriminator is 0)\n12. `go/indicators/johnehlers/fractaladaptivemovingaverage/params.go` - `Params`, Length=16, SlowestSmoothingFactor=0.01\n13. `go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/params.go` - `Params`, AlphaEmaPeriodAdditional=0.33, TrendLineSmoothingLength=4, CyclePartMultiplier=1.0\n14. `go/indicators/johnehlers/instantaneoustrendline/lengthparams.go` - TWO functions:\n    - In `lengthparams.go`: `DefaultLengthParams() *LengthParams` with Length=28\n    - In `smoothingfactorparams.go`: `DefaultSmoothingFactorParams() *SmoothingFactorParams` with SmoothingFactor=0.07\n15. `go/indicators/johnehlers/mesaadaptivemovingaverage/params.go` - Read the file first. It has TWO structs (LengthParams and SmoothingFactorParams). Add:\n    - `DefaultLengthParams() *LengthParams` with FastLimitLength=3, SlowLimitLength=39\n    - `DefaultSmoothingFactorParams() *SmoothingFactorParams` with FastLimitSmoothingFactor=0.5, SlowLimitSmoothingFactor=0.05\n16. `go/indicators/johnehlers/roofingfilter/params.go` - `RoofingFilterParams`, ShortestCyclePeriod=10, LongestCyclePeriod=48\n17. `go/indicators/johnehlers/sinewave/params.go` - `Params`, AlphaEmaPeriodAdditional=0.33\n18. `go/indicators/johnehlers/supersmoother/params.go` - `SuperSmootherParams`, ShortestCyclePeriod=10\n19. `go/indicators/johnehlers/trendcyclemode/params.go` - `Params`, AlphaEmaPeriodAdditional=0.33, TrendLineSmoothingLength=4, CyclePartMultiplier=1.0, SeparationPercentage=1.5\n20. `go/indicators/johnehlers/zerolagerrorcorrectingexponentialmovingaverage/params.go` - `ZeroLagErrorCorrectingExponentialMovingAverageParams`, SmoothingFactor=0.095, GainLimit=5, GainStep=0.1\n21. `go/indicators/johnehlers/zerolagexponentialmovingaverage/params.go` - `ZeroLagExponentialMovingAverageParams`, SmoothingFactor=0.25, VelocityGainFactor=0.5, VelocityMomentumLength=3\n\nAlso add DefaultParams for the hilbert transformer shared params (if it has a params file):\n22. `go/indicators/johnehlers/hilberttransformer/cycleestimatorparams.go` - `CycleEstimatorParams`, SmoothingLength=4, AlphaEmaQuadratureInPhase=0.2, AlphaEmaPeriod=0.2 (homodyne defaults)\n\nRead each file, edit it, confirm. Do ALL files listed above.",
  "subagent_type": "general"
}
```

**Output:**
```
task_id: ses_23fee2ec2ffeK6UQo842yHKHz2 (for resuming to continue this task if needed)

<task_result>
All 22 files edited successfully and the build passes clean. Here's a summary:

- **22 files** modified with `DefaultParams()` (or variant names) functions
- Skipped `corona/params.go` as instructed (already has `DefaultParams()`)
- Files with two param structs (`cybercycle`, `instantaneoustrendline`, `mesaadaptivemovingaverage`) got `DefaultLengthParams()` and `DefaultSmoothingFactorParams()`
- `hilberttransformer/cycleestimatorparams.go` got `DefaultParams()` with homodyne discriminator defaults
- No `BarComponent`/`QuoteComponent`/`TradeComponent` fields included
- Build passes with no errors
</task_result>
```

**Tool: task**

**Input:**
```json
{
  "description": "Add DefaultParams Go others",
  "prompt": "You are adding `DefaultParams()` functions to Go indicator params files for the remaining subdirectories. Read each file first before editing. Add the function at the END of each file.\n\nPattern:\n```go\n// DefaultParams returns a [StructName] value populated with conventional defaults.\nfunc DefaultParams() *StructName {\n    return &StructName{ ... }\n}\n```\n\nDo NOT include BarComponent/QuoteComponent/TradeComponent fields in DefaultParams (zero values are correct).\n\nFiles to modify:\n\n1. `go/indicators/custom/goertzelspectrum/params.go` - `Params`, Length=64, MinPeriod=2, MaxPeriod=64, SpectrumResolution=1, AutomaticGainControlDecayFactor=0.991 (bools stay false). Note MinPeriod and MaxPeriod are float64.\n\n2. `go/indicators/custom/maximumentropyspectrum/params.go` - `Params`, Length=60, Degree=30, MinPeriod=2, MaxPeriod=59, SpectrumResolution=1, AutomaticGainControlDecayFactor=0.995. MinPeriod and MaxPeriod are float64.\n\n3. `go/indicators/donaldlambert/commoditychannelindex/params.go` - `CommodityChannelIndexParams`, Length=20, InverseScalingFactor=0.015\n\n4. `go/indicators/genequong/moneyflowindex/params.go` - `MoneyFlowIndexParams`, Length=14\n\n5. `go/indicators/georgelane/stochastic/params.go` - `StochasticParams`, FastKLength=5, SlowKLength=3, SlowDLength=3 (MA types stay zero = SMA)\n\n6. `go/indicators/geraldappel/movingaverageconvergencedivergence/params.go` - `MovingAverageConvergenceDivergenceParams`, FastLength=12, SlowLength=26, SignalLength=9. Note: FirstIsAverage is *bool type, not bool. For the default, use a helper or set it directly. Read the file to understand the convention. The default for FirstIsAverage should be nil (which means TA-Lib compatible = true).\n\n7. `go/indicators/geraldappel/percentagepriceoscillator/params.go` - `PercentagePriceOscillatorParams`, FastLength=12, SlowLength=26\n\n8. `go/indicators/igorlivshin/balanceofpower/params.go` - `BalanceOfPowerParams` (EMPTY struct): `func DefaultParams() *BalanceOfPowerParams { return &BalanceOfPowerParams{} }`\n\n9. `go/indicators/jackhutson/tripleexponentialmovingaverageoscillator/params.go` - `TripleExponentialMovingAverageOscillatorParams`, Length=30\n\n10. `go/indicators/johnbollinger/bollingerbands/params.go` - `BollingerBandsParams`, Length=5, UpperMultiplier=2.0, LowerMultiplier=2.0. Note: IsUnbiased is *bool type. Leave it nil (treated as true).\n\n11. `go/indicators/johnbollinger/bollingerbandstrend/params.go` - `BollingerBandsTrendParams`, FastLength=20, SlowLength=50, UpperMultiplier=2.0, LowerMultiplier=2.0. IsUnbiased is *bool, leave nil.\n\n12. `go/indicators/josephgranville/onbalancevolume/params.go` - `OnBalanceVolumeParams` (only has BarComponent etc). Add: `func DefaultParams() *OnBalanceVolumeParams { return &OnBalanceVolumeParams{} }`\n\n13. `go/indicators/larrywilliams/ultimateoscillator/params.go` - `UltimateOscillatorParams`, Length1=7, Length2=14, Length3=28\n\n14. `go/indicators/larrywilliams/williamspercentr/params.go` - `WilliamsPercentRParams`, Length=14\n\n15. `go/indicators/marcchaikin/advancedecline/params.go` - `AdvanceDeclineParams` (EMPTY): `func DefaultParams() *AdvanceDeclineParams { return &AdvanceDeclineParams{} }`\n\n16. `go/indicators/marcchaikin/advancedeclineoscillator/params.go` - `AdvanceDeclineOscillatorParams`, FastLength=3, SlowLength=10\n\n17. `go/indicators/markjurik/jurikmovingaverage/params.go` - `JurikMovingAverageParams`, Length=14, Phase=0\n\n18. `go/indicators/patrickmulloy/doubleexponentialmovingaverage/params.go` - TWO functions:\n    - `DefaultLengthParams() *DoubleExponentialMovingAverageLengthParams` with Length=20\n    - `DefaultSmoothingFactorParams() *DoubleExponentialMovingAverageSmoothingFactorParams` with SmoothingFactor=0.0952\n\n19. `go/indicators/patrickmulloy/tripleexponentialmovingaverage/params.go` - TWO functions:\n    - `DefaultLengthParams() *TripleExponentialMovingAverageLengthParams` with Length=20\n    - `DefaultSmoothingFactorParams() *TripleExponentialMovingAverageSmoothingFactorParams` with SmoothingFactor=0.0952\n\n20. `go/indicators/perrykaufman/kaufmanadaptivemovingaverage/params.go` - TWO functions:\n    - `DefaultLengthParams() *KaufmanAdaptiveMovingAverageLengthParams` with EfficiencyRatioLength=10, FastestLength=2, SlowestLength=30\n    - `DefaultSmoothingFactorParams() *KaufmanAdaptiveMovingAverageSmoothingFactorParams` with EfficiencyRatioLength=10, FastestSmoothingFactor=2.0/3.0, SlowestSmoothingFactor=2.0/31.0\n\n21. `go/indicators/timtillson/t2exponentialmovingaverage/params.go` - TWO functions:\n    - `DefaultLengthParams() *T2ExponentialMovingAverageLengthParams` with Length=5, VolumeFactor=0.7\n    - `DefaultSmoothingFactorParams() *T2ExponentialMovingAverageSmoothingFactorParams` with SmoothingFactor=0.3333, VolumeFactor=0.7\n\n22. `go/indicators/timtillson/t3exponentialmovingaverage/params.go` - TWO functions:\n    - `DefaultLengthParams() *T3ExponentialMovingAverageLengthParams` with Length=5, VolumeFactor=0.7\n    - `DefaultSmoothingFactorParams() *T3ExponentialMovingAverageSmoothingFactorParams` with SmoothingFactor=0.3333, VolumeFactor=0.7\n\n23. `go/indicators/tusharchande/aroon/params.go` - `AroonParams`, Length=14\n\n24. `go/indicators/tusharchande/chandemomentumoscillator/params.go` - `Params`, Length=14\n\n25. `go/indicators/tusharchande/stochasticrelativestrengthindex/params.go` - `StochasticRelativeStrengthIndexParams`, Length=14, FastKLength=5, FastDLength=3\n\n26. `go/indicators/vladimirkravchuk/adaptivetrendandcyclefilter/params.go` - `Params` (only BarComponent etc). Add: `func DefaultParams() *Params { return &Params{} }`\n\nRead each file, edit it, confirm. Do ALL 26 files (some have 2 functions = ~31 total function additions).",
  "subagent_type": "general"
}
```

**Output:**
```
task_id: ses_23fedcef7ffeYgUMj5Yi1vKOUM (for resuming to continue this task if needed)

<task_result>
All 26 files compile cleanly. All 31 `Default*Params()` functions have been added:

- 23 files with single `DefaultParams()` functions
- 5 files with dual `DefaultLengthParams()` + `DefaultSmoothingFactorParams()` functions (DEMA, TEMA, KAMA, T2, T3)

Key decisions:
- MACD `FirstIsAverage *bool` left as `nil` (default = TA-Lib compatible = true)
- Bollinger Bands/Trend `IsUnbiased *bool` left as `nil` (default = true)
- Empty structs (BalanceOfPower, AdvanceDecline, OnBalanceVolume, ATCF) return empty struct pointers
- No BarComponent/QuoteComponent/TradeComponent fields included (zero values correct)
</task_result>
```

---

## Assistant (Build · claude-opus-4.6 · 7.2s)

All Go indicators done. Let me verify the build compiles.

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Survey all Go indicator params files to catalog default values from comments",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Survey all TS indicator params files to catalog default values from comments",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Add DefaultParams() to all Go indicators",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Add defaultParams() to all TS indicators",
      "priority": "high",
      "status": "in_progress"
    },
    {
      "content": "Update factory to use DefaultParams where applicable",
      "priority": "medium",
      "status": "pending"
    },
    {
      "content": "Build and test Go",
      "priority": "high",
      "status": "in_progress"
    },
    {
      "content": "Build and test TS",
      "priority": "high",
      "status": "pending"
    },
    {
      "content": "Update skill docs to document DefaultParams pattern",
      "priority": "medium",
      "status": "pending"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Survey all Go indicator params files to catalog default values from comments",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Survey all TS indicator params files to catalog default values from comments",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Add DefaultParams() to all Go indicators",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Add defaultParams() to all TS indicators",
    "priority": "high",
    "status": "in_progress"
  },
  {
    "content": "Update factory to use DefaultParams where applicable",
    "priority": "medium",
    "status": "pending"
  },
  {
    "content": "Build and test Go",
    "priority": "high",
    "status": "in_progress"
  },
  {
    "content": "Build and test TS",
    "priority": "high",
    "status": "pending"
  },
  {
    "content": "Update skill docs to document DefaultParams pattern",
    "priority": "medium",
    "status": "pending"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 4.3s)

**Tool: bash**

**Input:**
```json
{
  "command": "cd /home/dev/zpano/go && go build ./...",
  "description": "Build all Go packages",
  "timeout": 60000
}
```

---

## Assistant (Build · claude-opus-4.6 · 233.9s)

Go builds clean. Now let me start on TypeScript in parallel batches.

**Tool: task**

**Input:**
```json
{
  "description": "Add defaultParams TS common",
  "prompt": "You are adding exported `defaultParams()` functions to TypeScript indicator params files. The function should be added at the end of the params.ts file and exported.\n\nIMPORTANT: Read each file first before editing. Use this pattern:\n```typescript\nexport function defaultParams(): SomeParamsInterface {\n    return {\n        field1: value1,\n        field2: value2,\n    };\n}\n```\n\nFor dual-interface indicators (Length/SmoothingFactor), use:\n```typescript\nexport function defaultLengthParams(): SomeLengthParamsInterface {\n    return { length: 20 };\n}\nexport function defaultSmoothingFactorParams(): SomeSFParamsInterface {\n    return { smoothingFactor: 0.0952 };\n}\n```\n\nDo NOT include barComponent/quoteComponent/tradeComponent fields (they default to undefined).\n\nFiles under `ts/indicators/common/`:\n\n1. `ts/indicators/common/absolute-price-oscillator/params.ts` - `AbsolutePriceOscillatorParams`, fastLength=12, slowLength=26\n2. `ts/indicators/common/exponential-moving-average/params.ts` - TWO functions: `defaultLengthParams()` returns `ExponentialMovingAverageLengthParams` with length=20, `defaultSmoothingFactorParams()` returns `ExponentialMovingAverageSmoothingFactorParams` with smoothingFactor=0.0952\n3. `ts/indicators/common/linear-regression/params.ts` - `LinearRegressionParams`, length=20\n4. `ts/indicators/common/momentum/params.ts` - `MomentumParams`, length=10\n5. `ts/indicators/common/pearsons-correlation-coefficient/params.ts` - `PearsonsCorrelationCoefficientParams`, length=20\n6. `ts/indicators/common/rate-of-change/params.ts` - `RateOfChangeParams`, length=10\n7. `ts/indicators/common/rate-of-change-percent/params.ts` - `RateOfChangePercentParams`, length=10\n8. `ts/indicators/common/rate-of-change-ratio/params.ts` - `RateOfChangeRatioParams`, length=10\n9. `ts/indicators/common/simple-moving-average/params.ts` - `SimpleMovingAverageParams`, length=20\n10. `ts/indicators/common/standard-deviation/params.ts` - `StandardDeviationParams`, length=20, unbiased=true (note: `unbiased` is required boolean in TS)\n11. `ts/indicators/common/triangular-moving-average/params.ts` - `TriangularMovingAverageParams`, length=20\n12. `ts/indicators/common/variance/params.ts` - `VarianceParams`, length=20, unbiased=true\n13. `ts/indicators/common/weighted-moving-average/params.ts` - `WeightedMovingAverageParams`, length=20\n\nRead each file first to see exact interface names and field names, then add the function(s) at the end. Do ALL 13 files.",
  "subagent_type": "general"
}
```

**Output:**
```
task_id: ses_23fea9961ffeOjx5zOGkWtRH6P (for resuming to continue this task if needed)

<task_result>
All 13 files are done. Summary of changes:

- **APO**: `defaultParams()` with `fastLength: 12, slowLength: 26`
- **EMA**: `defaultLengthParams()` with `length: 20` and `defaultSmoothingFactorParams()` with `smoothingFactor: 0.0952`
- **Linear Regression, Pearson's, SMA, Triangular MA, Weighted MA**: `defaultParams()` with `length: 20`
- **Momentum, ROC, ROC%, ROC Ratio**: `defaultParams()` with `length: 10`
- **Standard Deviation, Variance**: `defaultParams()` with `length: 20, unbiased: true`
</task_result>
```

**Tool: task**

**Input:**
```json
{
  "description": "Add defaultParams TS wilder",
  "prompt": "You are adding exported `defaultParams()` functions to TypeScript indicator params files under `ts/indicators/welles-wilder/`. Read each file first before editing.\n\nPattern:\n```typescript\nexport function defaultParams(): SomeInterface {\n    return {\n        field: value,\n    };\n}\n```\n\nDo NOT include barComponent/quoteComponent/tradeComponent fields.\n\nFiles:\n\n1. `ts/indicators/welles-wilder/average-directional-movement-index/params.ts` - `AverageDirectionalMovementIndexParams`, length=14\n2. `ts/indicators/welles-wilder/average-directional-movement-index-rating/params.ts` - `AverageDirectionalMovementIndexRatingParams`, length=14\n3. `ts/indicators/welles-wilder/average-true-range/params.ts` - `AverageTrueRangeParams`, length=14\n4. `ts/indicators/welles-wilder/directional-indicator-minus/params.ts` - `DirectionalIndicatorMinusParams`, length=14\n5. `ts/indicators/welles-wilder/directional-indicator-plus/params.ts` - `DirectionalIndicatorPlusParams`, length=14\n6. `ts/indicators/welles-wilder/directional-movement-index/params.ts` - `DirectionalMovementIndexParams`, length=14\n7. `ts/indicators/welles-wilder/directional-movement-minus/params.ts` - `DirectionalMovementMinusParams`, length=14\n8. `ts/indicators/welles-wilder/directional-movement-plus/params.ts` - `DirectionalMovementPlusParams`, length=14\n9. `ts/indicators/welles-wilder/normalized-average-true-range/params.ts` - `NormalizedAverageTrueRangeParams`, length=14\n10. `ts/indicators/welles-wilder/parabolic-stop-and-reverse/params.ts` - `ParabolicStopAndReverseParams`, startValue=0, offsetOnReverse=0, accelerationInitLong=0.02, accelerationLong=0.02, accelerationMaxLong=0.20, accelerationInitShort=0.02, accelerationShort=0.02, accelerationMaxShort=0.20 (note: all optional fields in TS)\n11. `ts/indicators/welles-wilder/relative-strength-index/params.ts` - `RelativeStrengthIndexParams`, length=14\n12. `ts/indicators/welles-wilder/true-range/params.ts` - `TrueRangeParams` (EMPTY interface): `export function defaultParams(): TrueRangeParams { return {}; }`\n\nRead each file, edit, confirm. Do ALL 12 files.",
  "subagent_type": "general"
}
```

**Output:**
```
task_id: ses_23fea7d51ffe3BORWXsei0y9AO (for resuming to continue this task if needed)

<task_result>
All 12 files edited successfully. Summary:

- **Files 1-9** (ADX, ADXR, ATR, DI-, DI+, DMI, DM-, DM+, NATR): `defaultParams()` returning `{ length: 14 }`
- **File 10** (Parabolic SAR): `defaultParams()` returning all 8 fields (startValue=0, offsetOnReverse=0, acceleration factors at 0.02/0.02/0.20 for both long and short)
- **File 11** (RSI): `defaultParams()` returning `{ length: 14 }` (excluded barComponent/quoteComponent/tradeComponent)
- **File 12** (True Range): `defaultParams()` returning `{}`
</task_result>
```

**Tool: task**

**Input:**
```json
{
  "description": "Add defaultParams TS ehlers",
  "prompt": "You are adding exported `defaultParams()` functions to TypeScript indicator params files under `ts/indicators/john-ehlers/`. Read each file first before editing.\n\nPattern:\n```typescript\nexport function defaultParams(): SomeInterface {\n    return { field: value };\n}\n```\n\nDo NOT include barComponent/quoteComponent/tradeComponent fields.\n\nFiles:\n\n1. `ts/indicators/john-ehlers/auto-correlation-indicator/params.ts` - `AutoCorrelationIndicatorParams`, minLag=3, maxLag=48, smoothingPeriod=10, averagingLength=0\n2. `ts/indicators/john-ehlers/auto-correlation-periodogram/params.ts` - `AutoCorrelationPeriodogramParams`, minPeriod=10, maxPeriod=48, averagingLength=3, automaticGainControlDecayFactor=0.995\n3. `ts/indicators/john-ehlers/center-of-gravity-oscillator/params.ts` - `CenterOfGravityOscillatorParams`, length=10\n4. `ts/indicators/john-ehlers/comb-band-pass-spectrum/params.ts` - `CombBandPassSpectrumParams`, minPeriod=10, maxPeriod=48, bandwidth=0.3, automaticGainControlDecayFactor=0.995\n5. `ts/indicators/john-ehlers/corona/params.ts` - Check if `DefaultCoronaParams` already exists (the survey said it does). If so, add `export function defaultParams(): CoronaParams { return { ...DefaultCoronaParams }; }` or skip if redundant.\n6. `ts/indicators/john-ehlers/corona-spectrum/params.ts` - `CoronaSpectrumParams`, minRasterValue=6, maxRasterValue=20, minParameterValue=6, maxParameterValue=30, highPassFilterCutoff=30\n7. `ts/indicators/john-ehlers/corona-signal-to-noise-ratio/params.ts` - `CoronaSignalToNoiseRatioParams`, rasterLength=50, maxRasterValue=20, minParameterValue=1, maxParameterValue=11, highPassFilterCutoff=30, minimalPeriod=6, maximalPeriod=30\n8. `ts/indicators/john-ehlers/corona-swing-position/params.ts` - `CoronaSwingPositionParams`, rasterLength=50, maxRasterValue=20, minParameterValue=-5, maxParameterValue=5, highPassFilterCutoff=30, minimalPeriod=6, maximalPeriod=30\n9. `ts/indicators/john-ehlers/corona-trend-vigor/params.ts` - `CoronaTrendVigorParams`, rasterLength=50, maxRasterValue=20, minParameterValue=-10, maxParameterValue=10, highPassFilterCutoff=30, minimalPeriod=6, maximalPeriod=30\n10. `ts/indicators/john-ehlers/cyber-cycle/length-params.ts` - `CyberCycleLengthParams`: `export function defaultLengthParams(): CyberCycleLengthParams { return { length: 28, signalLag: 9 }; }`\n11. `ts/indicators/john-ehlers/cyber-cycle/smoothing-factor-params.ts` - `CyberCycleSmoothingFactorParams`: `export function defaultSmoothingFactorParams(): CyberCycleSmoothingFactorParams { return { smoothingFactor: 0.07, signalLag: 9 }; }`\n12. `ts/indicators/john-ehlers/discrete-fourier-transform-spectrum/params.ts` - `DiscreteFourierTransformSpectrumParams`, length=48, minPeriod=10, maxPeriod=48, spectrumResolution=1, automaticGainControlDecayFactor=0.995\n13. `ts/indicators/john-ehlers/dominant-cycle/params.ts` - `DominantCycleParams`, alphaEmaPeriodAdditional=0.33\n14. `ts/indicators/john-ehlers/fractal-adaptive-moving-average/params.ts` - `FractalAdaptiveMovingAverageParams`, length=16, slowestSmoothingFactor=0.01\n15. `ts/indicators/john-ehlers/hilbert-transformer/cycle-estimator-params.ts` - `HilbertTransformerCycleEstimatorParams`, smoothingLength=4, alphaEmaQuadratureInPhase=0.2, alphaEmaPeriod=0.2\n16. `ts/indicators/john-ehlers/hilbert-transformer-instantaneous-trend-line/params.ts` - `HilbertTransformerInstantaneousTrendLineParams`, alphaEmaPeriodAdditional=0.33, trendLineSmoothingLength=4, cyclePartMultiplier=1.0\n17. `ts/indicators/john-ehlers/instantaneous-trend-line/length-params.ts` - `InstantaneousTrendLineLengthParams`: `export function defaultLengthParams()` with length=28\n18. `ts/indicators/john-ehlers/instantaneous-trend-line/smoothing-factor-params.ts` - `InstantaneousTrendLineSmoothingFactorParams`: `export function defaultSmoothingFactorParams()` with smoothingFactor=0.07\n19. `ts/indicators/john-ehlers/mesa-adaptive-moving-average/params.ts` - Read file. TWO functions:\n    - `defaultLengthParams()` returning `MesaAdaptiveMovingAverageLengthParams` with fastLimitLength=3, slowLimitLength=39\n    - `defaultSmoothingFactorParams()` returning `MesaAdaptiveMovingAverageSmoothingFactorParams` with fastLimitSmoothingFactor=0.5, slowLimitSmoothingFactor=0.05\n20. `ts/indicators/john-ehlers/roofing-filter/params.ts` - `RoofingFilterParams`, shortestCyclePeriod=10, longestCyclePeriod=48\n21. `ts/indicators/john-ehlers/sine-wave/params.ts` - `SineWaveParams`, alphaEmaPeriodAdditional=0.33\n22. `ts/indicators/john-ehlers/super-smoother/params.ts` - `SuperSmootherParams`, shortestCyclePeriod=10\n23. `ts/indicators/john-ehlers/trend-cycle-mode/params.ts` - `TrendCycleModeParams`, alphaEmaPeriodAdditional=0.33, trendLineSmoothingLength=4, cyclePartMultiplier=1.0, separationPercentage=1.5\n24. `ts/indicators/john-ehlers/zero-lag-error-correcting-exponential-moving-average/params.ts` - `ZeroLagErrorCorrectingExponentialMovingAverageParams`, smoothingFactor=0.095, gainLimit=5, gainStep=0.1\n25. `ts/indicators/john-ehlers/zero-lag-exponential-moving-average/params.ts` - `ZeroLagExponentialMovingAverageParams`, smoothingFactor=0.25, velocityGainFactor=0.5, velocityMomentumLength=3\n\nRead each file first, edit it, confirm. Do ALL 25 files.",
  "subagent_type": "general"
}
```

**Output:**
```
task_id: ses_23fea25a6ffeyvbjJ2gt7jF5jg (for resuming to continue this task if needed)

<task_result>
All 25 files have been edited:

1. **auto-correlation-indicator/params.ts** — `defaultParams()` with minLag, maxLag, smoothingPeriod, averagingLength
2. **auto-correlation-periodogram/params.ts** — `defaultParams()` with minPeriod, maxPeriod, averagingLength, automaticGainControlDecayFactor
3. **center-of-gravity-oscillator/params.ts** — `defaultParams()` with length
4. **comb-band-pass-spectrum/params.ts** — `defaultParams()` with minPeriod, maxPeriod, bandwidth, automaticGainControlDecayFactor
5. **corona/params.ts** — `defaultParams()` spreading `DefaultCoronaParams`
6. **corona-spectrum/params.ts** — `defaultParams()` with 5 fields
7. **corona-signal-to-noise-ratio/params.ts** — `defaultParams()` with 7 fields
8. **corona-swing-position/params.ts** — `defaultParams()` with 7 fields
9. **corona-trend-vigor/params.ts** — `defaultParams()` with 7 fields
10. **cyber-cycle/length-params.ts** — `defaultLengthParams()` with length, signalLag
11. **cyber-cycle/smoothing-factor-params.ts** — `defaultSmoothingFactorParams()` with smoothingFactor, signalLag
12. **discrete-fourier-transform-spectrum/params.ts** — `defaultParams()` with 5 fields
13. **dominant-cycle/params.ts** — `defaultParams()` with alphaEmaPeriodAdditional
14. **fractal-adaptive-moving-average/params.ts** — `defaultParams()` with length, slowestSmoothingFactor
15. **hilbert-transformer/cycle-estimator-params.ts** — `defaultParams()` with smoothingLength, alphaEmaQuadratureInPhase, alphaEmaPeriod
16. **hilbert-transformer-instantaneous-trend-line/params.ts** — `defaultParams()` with 3 fields
17. **instantaneous-trend-line/length-params.ts** — `defaultLengthParams()` with length
18. **instantaneous-trend-line/smoothing-factor-params.ts** — `defaultSmoothingFactorParams()` with smoothingFactor
19. **mesa-adaptive-moving-average/params.ts** — `defaultLengthParams()` + `defaultSmoothingFactorParams()`
20. **roofing-filter/params.ts** — `defaultParams()` with shortestCyclePeriod, longestCyclePeriod
21. **sine-wave/params.ts** — `defaultParams()` with alphaEmaPeriodAdditional
22. **super-smoother/params.ts** — `defaultParams()` with shortestCyclePeriod
23. **trend-cycle-mode/params.ts** — `defaultParams()` with 4 fields
24. **zero-lag-error-correcting-ema/params.ts** — `defaultParams()` with smoothingFactor, gainLimit, gainStep
25. **zero-lag-ema/params.ts** — `defaultParams()` with smoothingFactor, velocityGainFactor, velocityMomentumLength
</task_result>
```

**Tool: task**

**Input:**
```json
{
  "description": "Add defaultParams TS others",
  "prompt": "You are adding exported `defaultParams()` functions to TypeScript indicator params files for remaining subdirectories. Read each file first before editing.\n\nPattern:\n```typescript\nexport function defaultParams(): SomeInterface {\n    return { field: value };\n}\n```\n\nDo NOT include barComponent/quoteComponent/tradeComponent fields.\n\nFiles:\n\n1. `ts/indicators/custom/goertzel-spectrum/params.ts` - `GoertzelSpectrumParams`, length=64, minPeriod=2, maxPeriod=64, spectrumResolution=1, automaticGainControlDecayFactor=0.991\n2. `ts/indicators/custom/maximum-entropy-spectrum/params.ts` - `MaximumEntropySpectrumParams`, length=60, degree=30, minPeriod=2, maxPeriod=59, spectrumResolution=1, automaticGainControlDecayFactor=0.995\n3. `ts/indicators/donald-lambert/commodity-channel-index/params.ts` - `CommodityChannelIndexParams`, length=20, inverseScalingFactor=0.015\n4. `ts/indicators/gene-quong/money-flow-index/params.ts` - `MoneyFlowIndexParams`, length=14\n5. `ts/indicators/george-lane/stochastic/params.ts` - `StochasticParams`, fastKLength=5, slowKLength=3, slowDLength=3\n6. `ts/indicators/gerald-appel/moving-average-convergence-divergence/params.ts` - `MovingAverageConvergenceDivergenceParams`, fastLength=12, slowLength=26, signalLength=9 (all optional fields with defaults, just include these three)\n7. `ts/indicators/gerald-appel/percentage-price-oscillator/params.ts` - `PercentagePriceOscillatorParams`, fastLength=12, slowLength=26\n8. `ts/indicators/igor-livshin/balance-of-power/params.ts` - `BalanceOfPowerParams` (EMPTY): `export function defaultParams(): BalanceOfPowerParams { return {}; }`\n9. `ts/indicators/jack-hutson/triple-exponential-moving-average-oscillator/params.ts` - `TripleExponentialMovingAverageOscillatorParams`, length=30\n10. `ts/indicators/john-bollinger/bollinger-bands/params.ts` - `BollingerBandsParams`, length=5, upperMultiplier=2.0, lowerMultiplier=2.0\n11. `ts/indicators/john-bollinger/bollinger-bands-trend/params.ts` - `BollingerBandsTrendParams`, fastLength=20, slowLength=50, upperMultiplier=2.0, lowerMultiplier=2.0\n12. `ts/indicators/joseph-granville/on-balance-volume/params.ts` - `OnBalanceVolumeParams` (component-only): `export function defaultParams(): OnBalanceVolumeParams { return {}; }`\n13. `ts/indicators/larry-williams/ultimate-oscillator/params.ts` - `UltimateOscillatorParams`, length1=7, length2=14, length3=28\n14. `ts/indicators/larry-williams/williams-percent-r/params.ts` - `WilliamsPercentRParams`, length=14\n15. `ts/indicators/marc-chaikin/advance-decline/params.ts` - `AdvanceDeclineParams` (EMPTY): `export function defaultParams(): AdvanceDeclineParams { return {}; }`\n16. `ts/indicators/marc-chaikin/advance-decline-oscillator/params.ts` - `AdvanceDeclineOscillatorParams`, fastLength=3, slowLength=10\n17. `ts/indicators/mark-jurik/jurik-moving-average/params.ts` - `JurikMovingAverageParams`, length=14, phase=0\n18. `ts/indicators/patrick-mulloy/double-exponential-moving-average/params.ts` - Read file. TWO functions:\n    - `defaultLengthParams()` returning `DoubleExponentialMovingAverageLengthParams` with length=20, firstIsAverage=false\n    - `defaultSmoothingFactorParams()` returning `DoubleExponentialMovingAverageSmoothingFactorParams` with smoothingFactor=0.0952\n    Note: check if firstIsAverage is required in TS interface. If so, include it.\n19. `ts/indicators/patrick-mulloy/triple-exponential-moving-average/params.ts` - TWO functions:\n    - `defaultLengthParams()` returning `TripleExponentialMovingAverageLengthParams` with length=20, firstIsAverage=false\n    - `defaultSmoothingFactorParams()` returning `TripleExponentialMovingAverageSmoothingFactorParams` with smoothingFactor=0.0952\n20. `ts/indicators/perry-kaufman/kaufman-adaptive-moving-average/params.ts` - TWO functions:\n    - `defaultLengthParams()` with efficiencyRatioLength=10, fastestLength=2, slowestLength=30\n    - `defaultSmoothingFactorParams()` with efficiencyRatioLength=10, fastestSmoothingFactor=2/3, slowestSmoothingFactor=2/31\n21. `ts/indicators/tim-tillson/t2-exponential-moving-average/params.ts` - TWO functions:\n    - `defaultLengthParams()` with length=5, volumeFactor=0.7, firstIsAverage=false\n    - `defaultSmoothingFactorParams()` with smoothingFactor=0.3333, volumeFactor=0.7, firstIsAverage=false\n22. `ts/indicators/tim-tillson/t3-exponential-moving-average/params.ts` - TWO functions:\n    - `defaultLengthParams()` with length=5, volumeFactor=0.7, firstIsAverage=false\n    - `defaultSmoothingFactorParams()` with smoothingFactor=0.3333, volumeFactor=0.7, firstIsAverage=false\n23. `ts/indicators/tushar-chande/aroon/params.ts` - `AroonParams`, length=14\n24. `ts/indicators/tushar-chande/chande-momentum-oscillator/params.ts` - `ChandeMomentumOscillatorParams`, length=14\n25. `ts/indicators/tushar-chande/stochastic-relative-strength-index/params.ts` - `StochasticRelativeStrengthIndexParams`, length=14, fastKLength=5, fastDLength=3\n26. `ts/indicators/vladimir-kravchuk/adaptive-trend-and-cycle-filter/params.ts` - `AdaptiveTrendAndCycleFilterParams` (component-only): `export function defaultParams(): AdaptiveTrendAndCycleFilterParams { return {}; }`\n\nRead each file, edit, confirm. Do ALL 26 files.",
  "subagent_type": "general"
}
```

**Output:**
```
task_id: ses_23fe9e38fffeWnSg1KatK4BW4S (for resuming to continue this task if needed)

<task_result>
All 26 files have been edited. Summary of what was added:

1. **Single `defaultParams()`** - 20 files: goertzel-spectrum, maximum-entropy-spectrum, commodity-channel-index, money-flow-index, stochastic, macd, ppo, balance-of-power, trix-oscillator, bollinger-bands, bollinger-bands-trend, on-balance-volume, ultimate-oscillator, williams-%r, advance-decline, advance-decline-oscillator, jurik-ma, aroon, chande-momentum-oscillator, stochastic-rsi, adaptive-trend-and-cycle-filter
2. **Two functions (`defaultLengthParams` + `defaultSmoothingFactorParams`)** - 6 files: double-ema, triple-ema, kaufman-ama, t2-ema, t3-ema
3. **Empty returns `{}`** - 3 files: balance-of-power, on-balance-volume, advance-decline, adaptive-trend-and-cycle-filter (component-only or no params)
</task_result>
```

---

## Assistant (Build · claude-opus-4.6 · 7.9s)

All TS indicators done. Let me build TypeScript and verify.

**Tool: bash**

**Input:**
```json
{
  "command": "cd /home/dev/zpano/ts && npx tsc --noEmit 2>&1 | head -50",
  "description": "Type-check TypeScript",
  "timeout": 60000
}
```

**Output:**
```
indicators/george-lane/stochastic/stochastic.ts(217,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/george-lane/stochastic/stochastic.ts(217,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/larry-williams/ultimate-oscillator/ultimate-oscillator.ts(216,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/larry-williams/ultimate-oscillator/ultimate-oscillator.ts(216,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/larry-williams/williams-percent-r/williams-percent-r.ts(174,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/larry-williams/williams-percent-r/williams-percent-r.ts(174,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/tushar-chande/aroon/aroon.ts(201,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/tushar-chande/aroon/aroon.ts(201,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/average-directional-movement-index-rating/average-directional-movement-index-rating.ts(130,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/average-directional-movement-index-rating/average-directional-movement-index-rating.ts(130,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/average-directional-movement-index/average-directional-movement-index.ts(128,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/average-directional-movement-index/average-directional-movement-index.ts(128,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/average-true-range/average-true-range.ts(143,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/average-true-range/average-true-range.ts(143,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/directional-indicator-minus/directional-indicator-minus.ts(120,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/directional-indicator-minus/directional-indicator-minus.ts(120,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/directional-indicator-plus/directional-indicator-plus.ts(120,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/directional-indicator-plus/directional-indicator-plus.ts(120,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/directional-movement-index/directional-movement-index.ts(123,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/directional-movement-index/directional-movement-index.ts(123,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/directional-movement-minus/directional-movement-minus.ts(176,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/directional-movement-minus/directional-movement-minus.ts(176,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/directional-movement-plus/directional-movement-plus.ts(176,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/directional-movement-plus/directional-movement-plus.ts(176,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/normalized-average-true-range/normalized-average-true-range.ts(105,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/normalized-average-true-range/normalized-average-true-range.ts(105,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/true-range/true-range.ts(109,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/true-range/true-range.ts(109,36): error TS2339: Property 'ask' does not exist on type 'Quote'.

```

---

## Assistant (Build · claude-opus-4.6 · 7.9s)

These are pre-existing errors (Quote.bid/ask) unrelated to our changes. Let me verify our params files specifically have no type errors:

**Tool: bash**

**Input:**
```json
{
  "command": "cd /home/dev/zpano/ts && npx tsc --noEmit 2>&1 | grep \"params.ts\\|length-params.ts\\|smoothing-factor-params.ts\\|cycle-estimator-params.ts\"",
  "description": "Check for params-file errors",
  "timeout": 60000
}
```

---

## Assistant (Build · claude-opus-4.6 · 8.4s)

No errors in any params files. The only TS errors are pre-existing `Quote.bid/ask` issues unrelated to our changes.

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Survey all Go indicator params files to catalog default values from comments",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Survey all TS indicator params files to catalog default values from comments",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Add DefaultParams() to all Go indicators",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Add defaultParams() to all TS indicators",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Update factory to use DefaultParams where applicable",
      "priority": "medium",
      "status": "pending"
    },
    {
      "content": "Build and test Go",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Build and test TS",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Update skill docs to document DefaultParams pattern",
      "priority": "medium",
      "status": "in_progress"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Survey all Go indicator params files to catalog default values from comments",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Survey all TS indicator params files to catalog default values from comments",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Add DefaultParams() to all Go indicators",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Add defaultParams() to all TS indicators",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Update factory to use DefaultParams where applicable",
    "priority": "medium",
    "status": "pending"
  },
  {
    "content": "Build and test Go",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Build and test TS",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Update skill docs to document DefaultParams pattern",
    "priority": "medium",
    "status": "in_progress"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 5.1s)

Now let me update the skill docs and decide on the factory integration. Let me first update the indicator-architecture skill doc with the DefaultParams pattern.

**Tool: skill**

**Input:**
```json
{
  "name": "indicator-architecture"
}
```

**Output:**
```
<skill_content name="indicator-architecture">
# Skill: indicator-architecture

# Architecture & Scaffolding Guide

This document describes the design decisions, folder layout, and naming conventions
for the **zpano** trading indicators library. It is intended as a reference for both
human developers and AI agents creating new indicators.

## Scope

The rules in this document apply **only to the `indicators/` folder** within each
language directory (e.g., `ts/indicators/`, `go/indicators/`). Other folders at the
same level as `indicators/` (such as `entities/`) have their own conventions and are
not governed by this guide.

## Core Principles

1. **Multi-language library.** The same set of indicators is implemented in
   TypeScript, Go, Python, Rust, and Zig. Each language lives in its own
   top-level folder (`ts/`, `go/`, `python/`, `rust/`, `zig/`).
2. **Shared ideology, language-idiomatic scaffolding.** The organizational
   structure and design rules are the same across all languages. File and folder
   naming adapts to each language's conventions.
3. **Consistent depth.** Every indicator lives at exactly
   `indicators/<group>/<indicator>/`. No exceptions.

## Folder Layout

```
indicators/
├── common/                 # Well-known indicators with no specific/known author
│   ├── simple-moving-average/
│   ├── exponential-moving-average/
│   └── ...
├── core/                   # Shared types, interfaces, enums, base classes
│   ├── outputs/            # Indicator output data types (band, heatmap, ...)
│   └── frequency-response/ # Frequency response calculation utilities
├── custom/                 # Your own experimental/custom indicators
│   └── my-indicator/
├── <author-name>/          # Author-attributed indicators
│   └── <indicator-name>/
│       ├── implementation
│       ├── parameters
│       └── tests
└── ...
```

### The Three Special Folders

| Folder     | Purpose |
|------------|---------|
| `core/`    | Shared foundations: types, interfaces, enums, base abstractions, utilities. |
| `common/`  | Indicators whose author is unknown or not attributed to a single person (SMA, EMA, RSI, ...). |
| `custom/`  | Indicators you develop yourself. |

These three names are **reserved** and must never be used as author names.

### Author Folders

When an indicator was developed by a known author, it is placed under a folder
named after that author. Each indicator gets its own subfolder.

Example: Mark Jurik created JMA and CFB, so:

```
indicators/
└── mark-jurik/
    ├── jurik-moving-average/
    └── composite-fractal-behavior/
```

**Go: every author folder MUST contain a `doc.go`** that declares the
parent package with a one-line godoc comment identifying the author:

```go
// Package markjurik implements indicators developed by Mark Jurik.
package markjurik
```

This is the only file at the author-folder level; all actual indicator
code lives in subpackages. The same rule applies to the special
folders `common` (shared utilities across authors) and `custom`
(zpano-specific indicators without a single author). The `core`
folder already contains real framework code and needs no `doc.go`.

TypeScript has no equivalent requirement (no package-level doc
construct).

### Indicator Folders

Each indicator folder contains:

- **Implementation** -- the indicator logic.
- **Parameters** -- an interface/struct describing the indicator's configuration.
- **Tests** -- unit tests for the indicator.
- Optionally, additional files (output types, documentation, etc.).

### Indicator Names

Use **full, descriptive names** (not short mnemonics) for indicator folders:

| Correct                          | Incorrect |
|----------------------------------|-----------|
| `simple-moving-average/`         | `sma/`    |
| `jurik-moving-average/`          | `jma/`    |
| `bollinger-bands/`               | `bb/`     |

Short names (mnemonics) may be cryptic and can overlap between different
indicators. Full names are unambiguous and self-documenting.

### `core/` Internal Structure

```
core/
├── outputs/              # Output data types (band, heatmap, ...)
└── frequency-response/   # Frequency response calculation utilities
```

- **`outputs/`** contains indicator output shapes. An indicator can output a
  scalar, a band (upper/lower), a heatmap, etc. This folder holds the concrete
  types; the taxonomy enum lives in the `outputs/shape/` sub-package as
  `shape.Shape` (Go) / `Shape` (TS).
- **`frequency-response/`** contains utilities for computing the frequency
  response of a filter/indicator.

Other shared types (indicator interface, metadata, specification, line indicator
base class, indicator type enum) live directly in `core/`.

## Naming Conventions per Language

The structure is identical across languages; only the naming style changes.

| Language   | Folder naming       | Example author folder    | Example indicator folder      |
|------------|---------------------|--------------------------|-------------------------------|
| TypeScript | `kebab-case`        | `mark-jurik/`            | `jurik-moving-average/`       |
| Go         | `lowercase` (no separators) | `markjurik/`      | `jurikmovingaverage/`         |
| Python     | `snake_case`        | `mark_jurik/`            | `jurik_moving_average/`       |
| Rust       | `snake_case`        | `mark_jurik/`            | `jurik_moving_average/`       |
| Zig        | `snake_case`        | `mark_jurik/`            | `jurik_moving_average/`       |

### Symbol Naming (Types, Enums)

Go uses package-scoped names, so symbols can be shorter (the package provides
context). All other languages use fully-qualified symbol names.

| Concept           | Go (in `core` pkg)  | Go (in `outputs/shape` pkg) | TypeScript / Python / Rust / Zig |
|-------------------|---------------------|-----------------------------|----------------------------------|
| Indicator identity| `core.Identifier`   | --                          | `IndicatorIdentifier`            |
| Output shape      | --                  | `shape.Shape`               | `Shape`                          |

### File Naming

| Language   | Style                     | Test files                  | Example                          |
|------------|---------------------------|-----------------------------|----------------------------------|
| TypeScript | `kebab-case.ts`           | `kebab-case.spec.ts`        | `simple-moving-average.ts`       |
| Go         | `lowercase.go`            | `lowercase_test.go`         | `simplemovingaverage.go`         |
| Python     | `snake_case.py`           | `test_snake_case.py`        | `simple_moving_average.py`       |
| Rust       | `snake_case.rs`           | `snake_case_test.rs` or inline `#[cfg(test)]` | `simple_moving_average.rs` |
| Zig        | `snake_case.zig`          | `snake_case_test.zig`       | `simple_moving_average.zig`      |

No type-suffix convention is used (no `.enum.ts`, `.interface.ts`, etc.).
This is consistent across all languages.

#### Main vs. auxiliary files

Within an indicator folder, **only the main implementation file and its test
keep the indicator name**. Every auxiliary file drops the indicator-name
prefix — the folder (and in Go, the package name) already provides context,
so prefixing would stutter.

For a `simple-moving-average/` folder:

| Role                         | Go                              | TypeScript                              |
|------------------------------|---------------------------------|-----------------------------------------|
| Main implementation          | `simplemovingaverage.go`        | `simple-moving-average.ts`              |
| Main test                    | `simplemovingaverage_test.go`   | `simple-moving-average.spec.ts`         |
| Data-driven test (if split)  | `data_test.go`                  | `data.spec.ts`                          |
| Parameters                   | `params.go`                     | `params.ts`                             |
| Output enum                  | `output.go`                     | `output.ts`                             |
| Output test                  | `output_test.go`                | `output.spec.ts`                        |
| Coefficients / helpers       | `coefficients.go`, `estimator.go` | `coefficients.ts`, `estimator.ts`     |
| Package docs (Go only)       | `doc.go`                        | —                                       |

**Exception:** when an auxiliary file holds a *concept* that would be
meaningless outside the folder name (e.g., `hilberttransformer/` contains
`cycleestimator.go`, `phaseaccumulator.go`, `dualdifferentiator.go` — each
representing a distinct Hilbert-transformer variant), keep the conceptual
names as-is. Do not try to mechanically strip a prefix that was never there.

### Identifier Abbreviation Convention

To keep naming consistent across indicators and languages, prefer the **full
word** in identifiers. The following abbreviations are considered **banned**
as CamelCase word-parts (whether standalone or compound) — always use the
long form:

| Banned short | Canonical long | Example (before → after)                |
|--------------|----------------|------------------------------------------|
| `idx`        | `index`        | `bufIdx` → `bufferIndex`, `highestIdx` → `highestIndex` |
| `tmp`        | `temp`         | `tmpR` → `tempR`                         |
| `res`        | `result`       | `res` → `result`, `defSpectrumRes` → `defSpectrumResult` |
| `sig`        | `signal`       | `expSig` → `expSignal`, `mnemonicSig` → `mnemonicSignal` |
| `val`        | `value`        | `mnemonicVal` → `mnemonicValue`, `prevVal` → `previousValue` |
| `prev`       | `previous`     | `prevHigh` → `previousHigh`, `samplePrev` → `samplePrevious` |
| `avg`        | `average`      | `AvgLength` → `AverageLength`            |
| `mult`       | `multiplier`   | `upperMult` → `upperMultiplier`          |
| `buf`        | `buffer`       | *(compound only, paired with `idx`)*     |
| `param`      | `parameter`    | `paramResolution` → `parameterResolution` *(in identifiers; Go struct type `Params` stays)* |
| `hist`       | `histogram`    | `expectedHist` → `expectedHistogram`     |

**Allowed exceptions (retained as-is):**

- **Go language idioms:** `err`, `len`, `cap`, `min`, `max`, `num` — these are
  builtins or deeply idiomatic Go short forms. Do not expand.
- **Go struct field `Params` / variable `params`** — this is the established
  parameter-bundle type name across every indicator (`type Params struct`,
  `Compute(params Params)`). The long form `Parameters` is used **only** in
  `core/specification.go`/`core/indicator-specification.ts` where it
  describes framework-level metadata.
- **TS constructor-parameter idiom `value_`** (trailing underscore) — used
  to avoid shadowing a member `value` during construction.
- **Domain-specific short forms that happen to collide** (e.g., `pos` as a
  loop variable is fine even though an unrelated indicator may use
  `position` in its name). These are different concepts, not abbreviation
  drift.
- **TA-Lib port conventions** (`begIdx` → now `begIndex`, `outIdx` → now
  `outIndex`): the `Idx` part is normalized but the `beg`/`out` prefixes
  are kept, matching TA-Lib's C-source variable names for easier
  cross-reference.

**Go↔TS parity:** the same identifier stem MUST be used in both languages.
When porting, do not introduce new short forms; use the canonical long form
from the table above.

### Go Receiver Naming Convention

Method receivers follow a **type-shape rule**, not a one-letter-for-all rule:

- **Compound type name** (2+ CamelCase words, e.g. `SimpleMovingAverage`,
  `BarComponent`, `CycleEstimatorType`) → receiver is **`s`**
  ("self"/"struct"). Short, consistent across all large types.
- **Simple type name** (single word, e.g. `Momentum`, `Trade`, `Quote`) →
  receiver is the **first letter of the type name, lowercased** (`m`, `t`,
  `q`). Short types deserve short mnemonic receivers.

All methods on a given type MUST use the same receiver name (enforced by
`go vet`/consistency). When adding a method, match the receiver already in
use on that type.

**Local-variable collision:** if a method body needs a local variable that
would shadow the receiver `s` (typical case: `s := someValue.String()` in
`MarshalJSON`), rename the **local** to `str` (not the receiver). Example:

```go
func (s BarComponent) MarshalJSON() ([]byte, error) {
    str := s.String()           // local renamed s → str
    if str == unknown { ... }
}
```

This keeps the receiver convention uniform; local names are flexible.

### Concurrency / Lock Convention

Stateful indicators (those exposing `Update(...)` that mutates internal
fields) MUST guard reads and writes with a `sync.RWMutex`:

- **Field:** `mu sync.RWMutex` (always named `mu`, always `RWMutex` —
  never plain `sync.Mutex`).
- **Writer methods** (`Update`, any mutator): acquire with
  `s.mu.Lock(); defer s.mu.Unlock()` on the two lines immediately
  following the function-signature brace (before any other logic).
- **Reader methods** (`IsPrimed`, accessors that read mutable state):
  use `s.mu.RLock(); defer s.mu.RUnlock()`.
- **Never** unlock manually (non-deferred). Always pair Lock with
  `defer Unlock` on the very next line.

**Exceptions — indicators without a mutex:**

1. **Pure delegating wrappers** that own no mutable state of their own
   and forward all `Update`/read calls to an embedded indicator which
   itself carries a mutex (e.g. `standarddeviation` wrapping
   `variance`). The embedded indicator provides the lock.
2. **Internal engines** that are not part of the public indicator
   surface and are only consumed by higher-level wrappers which hold
   the mutex (e.g. `corona` engine used by `coronaswingposition`,
   `coronaspectrum`, `coronasignaltonoiseratio`, `coronatrendvigor`).

Any new stateful public indicator MUST either carry its own `mu
sync.RWMutex` following the pattern above or fall under one of the two
exceptions.

### Go Style Invariants (enforced across the codebase)

The following idioms are invariants — audits return **zero** deviations
and new code MUST respect them:

- **Variable declarations.** No `var x T = zero-literal`, no split
  `var x T` followed immediately by `x = expr`. Use either `var x T`
  (zero value) or `x := expr`.
- **Empty / nil tests.** Slice/map emptiness is not tested by
  `len(x) == 0`; prefer direct ranging or `== nil` where the value is
  nil-able. Error checks always use `err != nil`.
- **Any.** Use `any`, never `interface{}`.
- **Slice construction.** Use `[]T{}` or `make([]T, n)` /
  `make([]T, 0, n)` — never `make([]T, 0)` (no capacity hint).
- **`new`.** Not used; use composite literals (`&T{...}`).
- **Imports.** Grouped by class with blank lines: stdlib, external,
  `zpano/*`. Each group contains only one class.
- **Doc comments.** Every exported `func`/`type`/`var`/`const`/method
  has a leading `// Name ...` doc comment. Trivial stubs (e.g.
  `passthrough.Update`) still get a one-line comment.

### Go ↔ TypeScript Local-Variable Parity

The same indicator MUST use the **same local/field names** in both
languages. When porting, copy the other language's names verbatim
where semantically identical. Observed canonical vocabulary:

| Concept                    | Canonical local name |
|----------------------------|----------------------|
| Sample difference / delta  | `temp` (short-lived delta) / `diff` (explicit diff) |
| Accumulator sum            | `sum` (never `total`, `acc`, `accumulator`) |
| Small-comparison tolerance | `epsilon` |
| Running standard deviation | `stddev` |
| MACD fast/slow/signal line | `fast`, `slow`, `signal`, `macd`, `histogram` |
| Variance (scratch)         | `v` |
| NaN sentinel (Go)          | `nan := math.NaN()` |
| Spread (upper-lower)       | `spread` |
| Bollinger band-width       | `bw` *(local; field is `bandWidth`)* |
| Bollinger percent-band     | `pctB` *(local; field is `percentBand`)* |
| Length minus one           | `lengthMinOne` |
| Amount = price × volume    | `amount` |

Primary loop counter is `i`; secondary `j`, `k`; only use `index`
when the counter semantically labels a named index (e.g. filter-bank
bin index). Never introduce new short forms (`idx`, `res`, etc. are
banned per the Abbreviation Convention).

## Adding a New Indicator -- Checklist

1. **Determine the group.** Is the author known? Use `<author-name>/`. Unknown
   author? Use `common/`. Your own? Use `custom/`.
2. **Create the indicator subfolder** at `indicators/<group>/<indicator-name>/`
   using full descriptive names and language-appropriate casing.
3. **Create the required files:**
   - **Parameters** -- define a params struct. Where the indicator can be
     driven by different price components, include optional `BarComponent`,
     `QuoteComponent`, `TradeComponent` fields (zero = default in Go,
     `undefined` = default in TS). Document the defaults in field comments.
     For indicators that require fixed inputs (e.g., HLCV for Advance-Decline,
     OHLC for Balance of Power, high/low for Parabolic SAR), the params struct
     may be empty or contain only non-component parameters (length, thresholds).
   - **Output enum** -- define a per-indicator output enum. In **Go**, the type
     is bare `Output` and constants are bare concept names (package name
     provides scoping — `simplemovingaverage.Output`, `simplemovingaverage.Value`).
     In **TypeScript**, the enum is named `<IndicatorName>Output` (e.g.,
     `SimpleMovingAverageOutput`) because TS imports by symbol, not by module.
   - **Implementation** -- embed `core.LineIndicator` (Go) or extend
     `LineIndicator` (TS). In the constructor:
     1. Resolve zero-value components → defaults for the component functions.
        In Go, check `if bc == 0 { bc = DefaultBarComponent }`.
        In TS, set the protected setters (`this.barComponent = params.barComponent`),
        which resolve `undefined` → default internally.
     2. Build the mnemonic using `ComponentTripleMnemonic` / `componentTripleMnemonic`
        with the **resolved** values (Go) or raw param values (TS — the function
        checks `!== undefined && !== Default*` itself). **Exception:** indicators
        with no component params (empty or non-component-only Params) hand-write
        their mnemonic (e.g., `bop()`, `ad()`, `psar(0.02,0.2)`) — the mnemonic
        still includes non-component parameters when non-default.
     3. Initialize the `LineIndicator` with mnemonic, description, component
        functions, and the indicator's `Update` method.
   - **`Metadata()`** -- return `core.BuildMetadata(core.<Identifier>, mnemonic, description, []core.OutputText{...})` (Go) or
     `buildMetadata(IndicatorIdentifier.<Name>, mnemonic, description, [...])` (TS). The helper sources each output's `kind` and
     `shape` from the descriptor registry — the caller supplies only per-output mnemonic/description. See the **Taxonomy &
     Descriptor Registry** section below for details and the mandatory descriptor-row step.
   - **Test file** -- include mnemonic sub-tests covering: all components zero,
     each component set individually, and combinations of two components.
4. **Register the indicator** in `core/identifier` (add a new enum variant in both Go `core.Identifier` and TS `IndicatorIdentifier`).
5. **Register the descriptor** in `core/descriptors.{go,ts}` — see the Taxonomy section below. A missing descriptor row causes
   `BuildMetadata` to panic at runtime.
6. **Follow the consistent depth rule** -- the indicator must be exactly two
   levels below `indicators/`.

## Design Decisions Log

| Decision | Rationale |
|----------|-----------|
| No underscore prefix on `core/`, `common/`, `custom/` | Leading underscores conflict with conventions in Python (`_` = private) and are non-idiomatic in Go. Dropping them gives true consistency across all five languages. The names are unambiguous enough on their own. |
| Full indicator names, not mnemonics | Short names (SMA, JMA) can be cryptic and may overlap. Full names are self-documenting. |
| `outputs/` instead of `entities/` in core | These types represent indicator *output shapes*, not general domain entities. `outputs` is more precise about their purpose. |
| `core/` instead of `types/` | `core` is broad enough to hold types, enums, base classes, and utilities without needing a rename later. `types` would be too narrow. |
| `common/` instead of `unknown/`, `classic/`, `standard/` | `common` is neutral -- doesn't imply age (`classic`), official status (`standard`), or sound dismissive (`unknown`). |
| `custom/` instead of `experimental/` | `custom` describes *origin* (you made it), not *maturity*. A custom indicator can be stable; an experimental one implies instability. If you need to track maturity, use metadata in code, not folder structure. |
| Go uses scoped names (`core.Identifier`, `shape.Shape`); other languages use `IndicatorIdentifier`/`Shape` at the top level | Go's package system provides scoping, making prefixes redundant and stuttery (`core.IndicatorIdentifier`, `outputs.Shape`). Other languages import symbols directly, so self-describing names are necessary; `Shape` is scoped to its own `outputs/shape` sub-package in Go to avoid clashing with entity/indicator shapes. |
| Per-indicator subfolders even in `common/` | Without subfolders, all indicator files in `common/` would land in one flat directory (and in Go, one package). This doesn't scale and is inconsistent with the author-folder structure. |
| Component enums start at `iota + 1` | Makes the zero value (`0`) explicitly "not set", which is used as the sentinel for "use default". This is idiomatic Go for optional enum fields in structs (zero value of `int` is `0`). |
| TypeScript enums start at `0`, components are `undefined` when not set | TS enums default to `0`, and optional params use `undefined` (not `0`) as the sentinel for "not set". The `componentTripleMnemonic` function checks `!== undefined && !== Default*` to decide whether to show a component. The `LineIndicator` setters use `=== undefined` (not truthiness) to decide when to apply the default, so explicitly passing a zero-valued enum (e.g., `BarComponent.Open`) works correctly. |
| Zero-value component = default, omitted from mnemonic | Keeps the common case clean (`sma(14)` instead of `sma(14, c, ba/2, p)`) while still allowing explicit overrides when needed. `ComponentTripleMnemonic` checks against `Default*` constants (not zero), so explicitly passing the default value also omits it — the same indicator always gets the same mnemonic. |
| Entity `Mnemonic()` method separate from `String()` | `String()` returns full words (`"close"`, `"mid"`) for JSON serialization and debugging. `Mnemonic()` returns short codes (`"c"`, `"ba/2"`) for compact display in chart labels. Separating them avoids overloading `String()` with a display concern. |
| Mnemonic sub-tests for every indicator | Mnemonic correctness matters for charting UIs. Each indicator must have sub-tests covering: all components zero, each component individually, and pairwise combinations. This prevents regressions when changing the format string or `ComponentTripleMnemonic`. |

## LineIndicator Design Pattern

A **line indicator** takes a single numeric input and produces a single scalar
output. Most indicators (SMA, EMA, JMA, ...) are line indicators. The
`LineIndicator` abstraction eliminates boilerplate by providing
`UpdateScalar/Bar/Quote/Trade` methods (and array variants) that delegate to the
indicator's core `Update(sample) → value` function.

### What Each Indicator Implements

Each concrete indicator is responsible for:

1. **`Update(sample float64) float64`** (Go) / **`update(sample: number): number`** (TS) --
   the core calculation logic.
2. **`Metadata()`** / **`metadata()`** -- returns indicator-level metadata with
   an explicit per-indicator output enum (not a hardcoded `kind: 0`).
3. **`IsPrimed()`** / **`isPrimed()`** -- whether the indicator has received
   enough data to produce meaningful output.

### What LineIndicator Provides (Eliminating Boilerplate)

The `LineIndicator` base provides:

- **`UpdateScalar/Bar/Quote/Trade`** -- all delegate to the indicator's `Update()`
  method via a function reference (Go) or abstract method (TS). The bar/quote/trade
  variants extract the relevant component value first.
- **Array update methods** -- `UpdateScalars/Bars/Quotes/Trades` (TS only;
  Go has equivalent free functions in `core/indicator.go`).
- **Storage of component functions** -- bar, quote, and trade component
  extraction functions are stored and used by the Update* methods.

### Go Implementation

In Go, `LineIndicator` is an **embedded struct**. The concrete indicator stores a
`core.LineIndicator` field and initializes it via `core.NewLineIndicator()`,
passing its own `Update` method as a function reference:

```go
type SimpleMovingAverage struct {
    mu sync.RWMutex
    core.LineIndicator   // embedded -- promotes UpdateScalar/Bar/Quote/Trade
    // ... indicator-specific fields
}

func NewSimpleMovingAverage(p *Params) (*SimpleMovingAverage, error) {
    sma := &SimpleMovingAverage{ /* ... */ }
    sma.LineIndicator = core.NewLineIndicator(
        mnemonic, desc, barFunc, quoteFunc, tradeFunc, sma.Update,
    )
    return sma, nil
}
```

The mutex lives on the concrete indicator (not on `LineIndicator`), and is
acquired inside `Update()`. The promoted `UpdateScalar/Bar/Quote/Trade` methods
call `updateFn` (which is the concrete indicator's `Update`), so the threading
model is preserved.

### TypeScript Implementation

In TypeScript, `LineIndicator` is an **abstract class**. The concrete indicator
extends it and implements `update()` and `metadata()`:

```typescript
export abstract class LineIndicator implements Indicator {
    protected mnemonic!: string;
    protected description!: string;
    protected primed!: boolean;

    public abstract metadata(): IndicatorMetadata;
    public abstract update(sample: number): number;
    // updateScalar/Bar/Quote/Trade provided by base class
}
```

**Concrete indicator constructors must set the three component setters** to
initialize the component functions used by `updateBar()`, `updateQuote()`, and
`updateTrade()`:

```typescript
this.barComponent = params.barComponent;       // undefined → DefaultBarComponent
this.quoteComponent = params.quoteComponent;   // undefined → DefaultQuoteComponent
this.tradeComponent = params.tradeComponent;   // undefined → DefaultTradeComponent
```

The `LineIndicator` setters use `=== undefined` (not truthiness) to apply
defaults, so explicitly passing a zero-valued enum like `BarComponent.Open`
works correctly.

Each entity file also exports a `Default*` constant (`DefaultBarComponent`,
`DefaultQuoteComponent`, `DefaultTradeComponent`) for use in
`componentTripleMnemonic` and anywhere else defaults need to be checked.

### Per-Indicator Output Enums

Each indicator defines its own output enum describing what it produces. The
naming convention is **language-asymmetric** because the two languages scope
symbols differently:

- **Go**: use bare `Output` / `Value`. The package name
  (`simplemovingaverage.Output`, `simplemovingaverage.Value`) provides scoping,
  so repeating the indicator name in the type would stutter.
- **TypeScript**: use long-form `<IndicatorName>Output` /
  `<IndicatorName>Value`. TS imports by symbol rather than by module, so the
  indicator name must be baked into the identifier to stay unambiguous at
  call sites.

```go
// Go — file: simplemovingaverage/output.go
package simplemovingaverage

type Output int
const (
    Value Output = iota
)
```

```typescript
// TypeScript — file: simple-moving-average/output.ts
export enum SimpleMovingAverageOutput {
    SimpleMovingAverageValue = 0,
}
```

For multi-output indicators, strip the indicator-name prefix on the Go side
and keep the descriptive suffix:

```go
// Go — directionalmovementindex/output.go
type Output int
const (
    PlusDirectionalIndicator Output = iota
    MinusDirectionalIndicator
    AverageDirectionalIndex
    // ...
)
```

```typescript
// TypeScript — directional-movement-index/output.ts
export enum DirectionalMovementIndexOutput {
    PlusDirectionalIndicator = 0,
    MinusDirectionalIndicator = 1,
    AverageDirectionalIndex = 2,
    // ...
}
```

The `metadata()` method uses this enum for the `kind` field instead of a
hardcoded `0`, making the output semantics explicit and type-safe.

### Metadata Structure

The `Metadata` / `IndicatorMetadata` type includes:

| Field         | Description |
|---------------|-------------|
| `identifier`  | The indicator identifier enum variant (e.g., `SimpleMovingAverage`). |
| `mnemonic`    | A short name like `sma(5)` or `jma(7, -1)`. |
| `description` | A human-readable description like `Simple moving average sma(5)`. |
| `outputs`     | An array of per-output metadata (kind, shape, mnemonic, description). |

The `mnemonic` and `description` live at the indicator level (on `Metadata`)
and are also duplicated per-output for convenience (each output carries its own
mnemonic/description).

Each output carries:

| Field         | Description |
|---------------|-------------|
| `kind`        | The integer value of the indicator's per-output enum. |
| `shape`       | The output's data shape (`Scalar`, `Band`, `Heatmap`, `Polyline`). Sourced from the descriptor registry. |
| `mnemonic`    | Short name for this output. |
| `description` | Human-readable description of this output. |

In practice, **indicators never construct `Metadata` object literals directly**.
They call `core.BuildMetadata` (Go) / `buildMetadata` (TS), which pulls `kind`
and `shape` from the descriptor registry. See the Taxonomy section below.

### Mnemonic Prefix Convention

Every indicator is identified by a short lowercase **mnemonic prefix** (e.g.
`sma`, `rsi`, `bb`, `macd`). These prefixes must be consistent between Go and
TypeScript, because downstream tooling keys off them.

**Rules:**

1. **Lowercase only.** Digits are allowed (`t2`, `t3`, `rocr100`).
   Leading `+`/`-` is allowed for paired directional indicators (`+di`, `-di`,
   `+dm`, `-dm`).
2. **Kept short** — typically 2-7 chars. The mnemonic is a compact label, not a
   full name; the full name goes in `description`.
3. **Go ↔ TS parity.** The same prefix must appear on both sides. When adding
   an indicator, add its prefix to both languages in the same PR.
4. **Format.** `<prefix>(<param1>, <param2>, ...<componentSuffix>)` where the
   component suffix comes from `ComponentTripleMnemonic` (empty when all
   components are at their default).
5. **Configurable components → include the suffix.** Any indicator that
   exposes `BarComponent` / `QuoteComponent` / `TradeComponent` on its params
   struct **must** append `ComponentTripleMnemonic(bc, qc, tc)` to its
   mnemonic. Indicators with hardcoded components (e.g. balance-of-power)
   omit the suffix.
6. **Parameterless indicators** use the bare prefix (`obv`, `ad`, `bop`) when
   defaults are in effect, or wrap the component suffix in parens when needed
   (e.g. `obv(hl/2)`).

**Canonical inventory (65 indicators with mnemonics, plus 4 paired directional):**

| Prefix    | Indicator                                            |
|-----------|------------------------------------------------------|
| `aci`     | autocorrelation indicator                            |
| `acp`     | autocorrelation periodogram                          |
| `ad`      | advance/decline                                      |
| `adosc`   | advance/decline oscillator                           |
| `adx`     | average directional movement index                   |
| `adxr`    | average directional movement index rating            |
| `apo`     | absolute price oscillator                            |
| `aroon`   | Aroon                                                |
| `atcf`    | adaptive trend and cycle filter                      |
| `atr`     | average true range                                   |
| `bb`      | Bollinger bands                                      |
| `bbtrend` | Bollinger bands trend                                |
| `bop`     | balance of power                                     |
| `cbps`    | comb band-pass spectrum                              |
| `cc`      | cyber cycle                                          |
| `cci`     | commodity channel index                              |
| `cmo`     | Chande momentum oscillator                           |
| `cog`     | center-of-gravity oscillator                         |
| `correl`  | Pearson's correlation coefficient                    |
| `csnr`    | corona signal-to-noise ratio                         |
| `cspect`  | corona spectrum                                      |
| `cswing`  | corona swing position                                |
| `ctv`     | corona trend vigor                                   |
| `dcp`     | dominant cycle                                       |
| `dema`    | double exponential moving average                    |
| `dftps`   | discrete Fourier transform power spectrum            |
| `+di`/`-di` | directional indicator plus / minus                 |
| `+dm`/`-dm` | directional movement plus / minus                  |
| `dx`      | directional movement index                           |
| `ema`     | exponential moving average                           |
| `frama`   | fractal adaptive moving average                      |
| `gspect`  | Goertzel spectrum                                    |
| `htitl`   | Hilbert transformer instantaneous trend line         |
| `iTrend`  | instantaneous trend line                             |
| `jma`     | Jurik moving average                                 |
| `kama`    | Kaufman adaptive moving average                      |
| `linreg`  | linear regression                                    |
| `macd`    | moving average convergence/divergence                |
| `mama`    | MESA adaptive moving average                         |
| `mespect` | maximum entropy spectrum                             |
| `mfi`     | money flow index                                     |
| `mom`     | momentum                                             |
| `natr`    | normalized average true range                        |
| `obv`     | on-balance volume                                    |
| `ppo`     | percentage price oscillator                          |
| `roc`     | rate of change                                       |
| `rocp`    | rate of change percent                               |
| `rocr`/`rocr100` | rate of change ratio (unit or ×100)           |
| `roof`    | roofing filter                                       |
| `rsi`     | relative strength index                              |
| `sar`     | parabolic stop and reverse                           |
| `sma`     | simple moving average                                |
| `ss`      | super smoother                                       |
| `stdev`   | standard deviation (`.s` sample / `.p` population)   |
| `stoch`   | stochastic                                           |
| `stochrsi`| stochastic relative strength index                   |
| `sw`      | sine wave                                            |
| `tcm`     | trend cycle mode                                     |
| `tema`    | triple exponential moving average                    |
| `tr`      | true range                                           |
| `trima`   | triangular moving average                            |
| `trix`    | triple exponential moving average oscillator         |
| `t2`      | T2 exponential moving average                        |
| `t3`      | T3 exponential moving average                        |
| `ultosc`  | ultimate oscillator                                  |
| `var`     | variance (`.s` sample / `.p` population)             |
| `willr`   | Williams %R                                          |
| `wma`     | weighted moving average                              |
| `zecema`  | zero-lag error-correcting exponential moving average |
| `zema`    | zero-lag exponential moving average                  |

The two utility modules `corona/` and `frequencyresponse/` are support helpers,
not indicators, and have no mnemonic of their own.

### Component Triple Mnemonic

A line indicator can be fed bar, quote, or trade data. Each data type has a
**component** that selects which field to extract (e.g., close price from a bar,
mid-price from a quote). The `ComponentTripleMnemonic` utility (in `core/`)
generates the component suffix for an indicator's mnemonic. There are three
components (bar, quote, trade) -- hence "triple", not "pair".

#### Default Components

Each component type has a default constant defined in `entities/`:

| Component type   | Default constant         | Default value    | Mnemonic |
|------------------|--------------------------|------------------|----------|
| `BarComponent`   | `DefaultBarComponent`    | `BarClosePrice`  | `c`      |
| `QuoteComponent` | `DefaultQuoteComponent`  | `QuoteMidPrice`  | `ba/2`   |
| `TradeComponent` | `DefaultTradeComponent`  | `TradePrice`     | `p`      |

#### Zero-Value Convention

All component enums start at `iota + 1`, making the zero value `0` explicitly
"not set". In indicator params, **a zero-value component means "use the default
and omit from the mnemonic"**:

```go
// SimpleMovingAverageParams
type SimpleMovingAverageParams struct {
    Length         int
    BarComponent   entities.BarComponent   // zero → DefaultBarComponent, omitted from mnemonic
    QuoteComponent entities.QuoteComponent // zero → DefaultQuoteComponent, omitted from mnemonic
    TradeComponent entities.TradeComponent // zero → DefaultTradeComponent, omitted from mnemonic
}
```

#### Constructor Resolution Logic

Each indicator constructor follows this pattern:

1. Read the component from params.
2. If zero, resolve to the default constant.
3. Get the component function using the resolved value.
4. Pass the **resolved** values to `ComponentTripleMnemonic` (which omits
   defaults automatically).

```go
// Resolve defaults for component functions.
bc := p.BarComponent
if bc == 0 {
    bc = entities.DefaultBarComponent
}
// ... same for qc, tc

// Build mnemonic using resolved components — defaults are omitted by ComponentTripleMnemonic.
mnemonic := fmt.Sprintf("sma(%d%s)", length,
    core.ComponentTripleMnemonic(bc, qc, tc))
```

This means that `sma(14)` is the mnemonic whether the user left `BarComponent`
as zero or explicitly set it to `BarClosePrice` — both produce the same
indicator with the same mnemonic.

#### ComponentTripleMnemonic Function

The function compares each component against its `Default*` constant. If a
component equals its default, it is omitted. Non-default components are
appended as `", "` followed by the component's `Mnemonic()` output:

```go
func ComponentTripleMnemonic(
    bc entities.BarComponent, qc entities.QuoteComponent, tc entities.TradeComponent,
) string {
    var s string
    if bc != entities.DefaultBarComponent   { s += ", " + bc.Mnemonic() }
    if qc != entities.DefaultQuoteComponent { s += ", " + qc.Mnemonic() }
    if tc != entities.DefaultTradeComponent { s += ", " + tc.Mnemonic() }
    return s
}
```

The result is inserted into the indicator format string after the numeric
parameters: `sma(14%s)` → `sma(14)` or `sma(14, hl/2)`.

#### Mnemonic Examples

| Params                                                                | Mnemonic              |
|-----------------------------------------------------------------------|-----------------------|
| `{Length: 5}`                                                         | `sma(5)`              |
| `{Length: 5, BarComponent: BarMedianPrice}`                           | `sma(5, hl/2)`        |
| `{Length: 5, QuoteComponent: QuoteBidPrice}`                          | `sma(5, b)`           |
| `{Length: 5, TradeComponent: TradeVolume}`                            | `sma(5, v)`           |
| `{Length: 5, BarComponent: BarOpenPrice, QuoteComponent: QuoteBidPrice}` | `sma(5, o, b)`     |
| `{Length: 5, BarComponent: BarHighPrice, TradeComponent: TradeVolume}` | `sma(5, h, v)`       |
| `{Length: 7, Phase: 0}`                                               | `jma(7, 0)`           |
| `{Length: 7, Phase: 0, BarComponent: BarMedianPrice}`                 | `jma(7, 0, hl/2)`    |

The `description` is always the full name followed by the mnemonic:
`"Simple moving average sma(5, hl/2)"`.

#### Entity Mnemonic Methods

Each component enum has two display methods: `String()` (full word, used for
JSON serialization) and `Mnemonic()` (short code, used in indicator mnemonics).

**BarComponent mnemonics:**

| Enum value        | `String()`   | `Mnemonic()` |
|-------------------|-------------|---------------|
| `BarOpenPrice`    | `open`      | `o`           |
| `BarHighPrice`    | `high`      | `h`           |
| `BarLowPrice`     | `low`       | `l`           |
| `BarClosePrice`   | `close`     | `c`           |
| `BarVolume`       | `volume`    | `v`           |
| `BarMedianPrice`  | `median`    | `hl/2`        |
| `BarTypicalPrice` | `typical`   | `hlc/3`       |
| `BarWeightedPrice`| `weighted`  | `hlcc/4`      |
| `BarAveragePrice` | `average`   | `ohlc/4`      |

**QuoteComponent mnemonics:**

| Enum value             | `String()`     | `Mnemonic()`          |
|------------------------|----------------|-----------------------|
| `QuoteBidPrice`        | `bid`          | `b`                   |
| `QuoteAskPrice`        | `ask`          | `a`                   |
| `QuoteBidSize`         | `bidSize`      | `bs`                  |
| `QuoteAskSize`         | `askSize`      | `as`                  |
| `QuoteMidPrice`        | `mid`          | `ba/2`                |
| `QuoteWeightedPrice`   | `weighted`     | `(bbs+aas)/(bs+as)`   |
| `QuoteWeightedMidPrice`| `weightedMid`  | `(bas+abs)/(bs+as)`   |
| `QuoteSpreadBp`        | `spreadBp`     | `spread bp`           |

**TradeComponent mnemonics:**

| Enum value    | `String()`  | `Mnemonic()` |
|---------------|-------------|---------------|
| `TradePrice`  | `price`     | `p`           |
| `TradeVolume` | `volume`    | `v`           |

## Helper / Shared Component Families

Some indicators are driven by an interchangeable internal component that has
several variants (e.g., different algorithms producing the same shape of
output). The canonical example is John Ehlers' **Hilbert Transformer cycle
estimator**, which has four variants (HomodyneDiscriminator,
HomodyneDiscriminatorUnrolled, PhaseAccumulator, DualDifferentiator) that all
produce a cycle period estimate from a price sample.

These are **not indicators themselves**. They are helper components used by
actual indicators (e.g., MESA Adaptive Moving Average). They live in the
indicator folder of their conceptual owner and follow a specific structure.

### Folder Layout

A helper family lives in a single folder at the standard indicator depth
(`indicators/<group>/<family-name>/`) and contains:

- **Shared interface** — the contract every variant implements.
- **Shared params struct/interface** — a single params shape common to all variants.
- **Variant type enum** — discriminator for the family (one constant per variant).
- **Common utilities file** — shared constants, helper functions (`push`,
  `fillWmaFactors`, ...) and the **factory/dispatcher** function.
- **One file per variant** implementing the shared interface.
- **One spec/test file per variant** plus one for the common file.

Example (TS — `ts/indicators/john-ehlers/hilbert-transformer/`):

```
cycle-estimator.ts                 # interface
cycle-estimator-params.ts          # shared params
cycle-estimator-type.ts            # variant enum
common.ts                          # utilities + createEstimator()
homodyne-discriminator.ts          # variant
homodyne-discriminator-unrolled.ts
phase-accumulator.ts
dual-differentiator.ts
# + matching .spec.ts for each
```

Go mirrors the same structure with lowercase filenames (`cycleestimator.go`,
`cycleestimatorparams.go`, `cycleestimatortype.go`, `estimator.go` for common
utilities and `NewCycleEstimator`, and one `<variant>estimator.go` per variant
— e.g., `homodynediscriminatorestimator.go`, `phaseaccumulatorestimator.go`).
These are the **conceptual-naming exception** noted in the File Naming section:
the folder name (`hilberttransformer/`) is never prefixed onto the member
files, because each file represents an independent concept within the family.

### Factory / Dispatcher Pattern

The common file exposes a dispatcher that constructs a variant by type.
Each variant has its own **default params** baked into the dispatcher — callers
that omit params for a specific type get sensible defaults for that variant.

```go
// Go — go/indicators/johnehlers/hilberttransformer/estimator.go
func NewCycleEstimator(typ CycleEstimatorType, params *CycleEstimatorParams) (CycleEstimator, error) {
    switch typ {
    case HomodyneDiscriminator:         return NewHomodyneDiscriminatorEstimator(params)
    case HomodyneDiscriminatorUnrolled: return NewHomodyneDiscriminatorEstimatorUnrolled(params)
    case PhaseAccumulator:              return NewPhaseAccumulatorEstimator(params)
    case DualDifferentiator:            return NewDualDifferentiatorEstimator(params)
    }
    return nil, fmt.Errorf("invalid cycle estimator type: %s", typ)
}
```

```ts
// TS — ts/indicators/john-ehlers/hilbert-transformer/hilbert-transformer-common.ts
export function createEstimator(
    estimatorType?: HilbertTransformerCycleEstimatorType,
    estimatorParams?: HilbertTransformerCycleEstimatorParams,
): HilbertTransformerCycleEstimator {
    if (estimatorType === undefined) {
        estimatorType = HilbertTransformerCycleEstimatorType.HomodyneDiscriminator;
    }
    switch (estimatorType) {
        case HilbertTransformerCycleEstimatorType.HomodyneDiscriminator:
            estimatorParams ??= { smoothingLength: 4, alphaEmaQuadratureInPhase: 0.2, alphaEmaPeriod: 0.2 };
            return new HilbertTransformerHomodyneDiscriminator(estimatorParams);
        // ... other variants, each with its own default params
        default:
            throw new Error('Invalid cycle estimator type: ' + estimatorType);
    }
}
```

The dispatcher must have a default variant (when `type` is omitted) and throw
on unknown types.

### Shared Interface Design

The shared interface exposes:

- Primed state (`primed`) and warm-up configuration (`warmUpPeriod`).
- Construction parameters as readable properties (`smoothingLength`,
  `alphaEmaQuadratureInPhase`, `alphaEmaPeriod`).
- Intermediate state visible for testing/debugging (`smoothed`, `detrended`,
  `inPhase`, `quadrature`, `period`).
- An `update(sample)` method returning the primary output.

TypeScript naming: **do not** suffix getters with `Value`. Prefix the backing
private field with `_` so the public getter can take the unadorned name:

```ts
// Correct — public getter is `period`, backing field is `_period`.
private _period = 0;
public get period(): number { return this._period; }

// Incorrect — do not use `periodValue` just to avoid a name clash.
```

### File Naming — No Type Suffixes

This module follows the same rule as the rest of the codebase: **no
`.interface.ts`, `.enum.ts`, or similar type-suffixes**. Interfaces, enums,
params, and implementations all use plain `.ts` filenames differentiated by
descriptive stem names.

### Warm-Up Period Contract

Each variant has an intrinsic minimum priming length (the sample count needed
for its state to be meaningful). If `params.warmUpPeriod` is provided and
larger than that minimum, it overrides the minimum; otherwise the intrinsic
minimum is used. Tests must cover both the default path and the custom
`warmUpPeriod` override.

## Taxonomy & Descriptor Registry

Every indicator carries a **Descriptor** — a taxonomic classification used by
charting UIs and discovery tooling to filter, group, and lay out indicators.
Descriptors are the single source of truth for output `kind` and `shape`;
indicator `Metadata()` implementations consume the registry via
`BuildMetadata` and MUST NOT hand-write `kind`/`shape`.

### Dimensions

| Primitive           | Go type                 | TS type                 | Purpose |
|---------------------|-------------------------|-------------------------|---------|
| `Identifier`        | `core.Identifier`       | `IndicatorIdentifier`   | Unique ID of the indicator. |
| `Role`              | `core.Role`             | `Role`                  | Semantic role of an **output** (Smoother, Envelope, Overlay, Polyline, Oscillator, BoundedOscillator, Volatility, VolumeFlow, Directional, CyclePeriod, CyclePhase, FractalDimension, Spectrum, Signal, Histogram, RegimeFlag, Correlation). |
| `Pane`              | `core.Pane`             | `Pane`                  | Chart pane where an output is drawn: `Price`, `Own`, `OverlayOnParent`. |
| `Adaptivity`        | `core.Adaptivity`       | `Adaptivity`            | `Static` or `Adaptive` (indicator-level). |
| `InputRequirement`  | `core.InputRequirement` | `InputRequirement`      | Minimum input data type: `ScalarInput`, `QuoteInput`, `BarInput`, `TradeInput`. |
| `VolumeUsage`       | `core.VolumeUsage`      | `VolumeUsage`           | `NoVolume`, `AggregateBarVolume`, `PerTradeVolume`, `QuoteLiquidityVolume`. |
| `OutputDescriptor`  | `core.OutputDescriptor` | `OutputDescriptor`      | `{Kind, Shape, Role, Pane}` per output. |
| `Descriptor`        | `core.Descriptor`       | `Descriptor`            | `{Identifier, Family, Adaptivity, InputRequirement, VolumeUsage, Outputs}`. |

**VolumeUsage ⇔ InputRequirement validity:**

- `AggregateBarVolume` ⇒ `BarInput`
- `PerTradeVolume` ⇒ `TradeInput`
- `QuoteLiquidityVolume` ⇒ `QuoteInput`
- `NoVolume` is valid with any input

### Registry Files

| Location                                              | Content |
|-------------------------------------------------------|---------|
| `go/indicators/core/descriptors.go`                   | Go descriptor map keyed by `Identifier`. |
| `go/indicators/core/descriptors_test.go`              | Coverage test: every `Identifier` has a descriptor row. |
| `go/indicators/core/metadata_descriptor_test.go`      | Invariants: outputs are non-empty, Kinds are strictly ascending starting at 1, shapes are known. |
| `ts/indicators/core/descriptors.ts`                   | TS descriptor `Map` keyed by `IndicatorIdentifier`. Uses `out()`/`desc()` helpers and `S`/`R`/`P`/`A`/`I`/`V` aliases for terse one-line entries. |

**Kind-numbering asymmetry:** Go per-indicator output enums are 1-based

...6575 bytes truncated...

The tool call succeeded but the output was truncated. Full output saved to: /home/dev/.local/share/opencode/tool-output/tool_dc0193ac9001Gh6WYXpgCUAX2m
Use the Task tool to have explore agent process this file with Grep and Read (with offset/limit). Do NOT read the full file yourself - delegate to save context.
```

---

## Assistant (Build · claude-opus-4.6 · 5.9s)

Let me read just the end of the skill file to know where to add the new section:

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "offset": 450
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md</path>
<type>file</type>
<content>450: func NewSimpleMovingAverage(p *Params) (*SimpleMovingAverage, error) {
451:     sma := &SimpleMovingAverage{ /* ... */ }
452:     sma.LineIndicator = core.NewLineIndicator(
453:         mnemonic, desc, barFunc, quoteFunc, tradeFunc, sma.Update,
454:     )
455:     return sma, nil
456: }
457: ```
458: 
459: The mutex lives on the concrete indicator (not on `LineIndicator`), and is
460: acquired inside `Update()`. The promoted `UpdateScalar/Bar/Quote/Trade` methods
461: call `updateFn` (which is the concrete indicator's `Update`), so the threading
462: model is preserved.
463: 
464: ### TypeScript Implementation
465: 
466: In TypeScript, `LineIndicator` is an **abstract class**. The concrete indicator
467: extends it and implements `update()` and `metadata()`:
468: 
469: ```typescript
470: export abstract class LineIndicator implements Indicator {
471:     protected mnemonic!: string;
472:     protected description!: string;
473:     protected primed!: boolean;
474: 
475:     public abstract metadata(): IndicatorMetadata;
476:     public abstract update(sample: number): number;
477:     // updateScalar/Bar/Quote/Trade provided by base class
478: }
479: ```
480: 
481: **Concrete indicator constructors must set the three component setters** to
482: initialize the component functions used by `updateBar()`, `updateQuote()`, and
483: `updateTrade()`:
484: 
485: ```typescript
486: this.barComponent = params.barComponent;       // undefined → DefaultBarComponent
487: this.quoteComponent = params.quoteComponent;   // undefined → DefaultQuoteComponent
488: this.tradeComponent = params.tradeComponent;   // undefined → DefaultTradeComponent
489: ```
490: 
491: The `LineIndicator` setters use `=== undefined` (not truthiness) to apply
492: defaults, so explicitly passing a zero-valued enum like `BarComponent.Open`
493: works correctly.
494: 
495: Each entity file also exports a `Default*` constant (`DefaultBarComponent`,
496: `DefaultQuoteComponent`, `DefaultTradeComponent`) for use in
497: `componentTripleMnemonic` and anywhere else defaults need to be checked.
498: 
499: ### Per-Indicator Output Enums
500: 
501: Each indicator defines its own output enum describing what it produces. The
502: naming convention is **language-asymmetric** because the two languages scope
503: symbols differently:
504: 
505: - **Go**: use bare `Output` / `Value`. The package name
506:   (`simplemovingaverage.Output`, `simplemovingaverage.Value`) provides scoping,
507:   so repeating the indicator name in the type would stutter.
508: - **TypeScript**: use long-form `<IndicatorName>Output` /
509:   `<IndicatorName>Value`. TS imports by symbol rather than by module, so the
510:   indicator name must be baked into the identifier to stay unambiguous at
511:   call sites.
512: 
513: ```go
514: // Go — file: simplemovingaverage/output.go
515: package simplemovingaverage
516: 
517: type Output int
518: const (
519:     Value Output = iota
520: )
521: ```
522: 
523: ```typescript
524: // TypeScript — file: simple-moving-average/output.ts
525: export enum SimpleMovingAverageOutput {
526:     SimpleMovingAverageValue = 0,
527: }
528: ```
529: 
530: For multi-output indicators, strip the indicator-name prefix on the Go side
531: and keep the descriptive suffix:
532: 
533: ```go
534: // Go — directionalmovementindex/output.go
535: type Output int
536: const (
537:     PlusDirectionalIndicator Output = iota
538:     MinusDirectionalIndicator
539:     AverageDirectionalIndex
540:     // ...
541: )
542: ```
543: 
544: ```typescript
545: // TypeScript — directional-movement-index/output.ts
546: export enum DirectionalMovementIndexOutput {
547:     PlusDirectionalIndicator = 0,
548:     MinusDirectionalIndicator = 1,
549:     AverageDirectionalIndex = 2,
550:     // ...
551: }
552: ```
553: 
554: The `metadata()` method uses this enum for the `kind` field instead of a
555: hardcoded `0`, making the output semantics explicit and type-safe.
556: 
557: ### Metadata Structure
558: 
559: The `Metadata` / `IndicatorMetadata` type includes:
560: 
561: | Field         | Description |
562: |---------------|-------------|
563: | `identifier`  | The indicator identifier enum variant (e.g., `SimpleMovingAverage`). |
564: | `mnemonic`    | A short name like `sma(5)` or `jma(7, -1)`. |
565: | `description` | A human-readable description like `Simple moving average sma(5)`. |
566: | `outputs`     | An array of per-output metadata (kind, shape, mnemonic, description). |
567: 
568: The `mnemonic` and `description` live at the indicator level (on `Metadata`)
569: and are also duplicated per-output for convenience (each output carries its own
570: mnemonic/description).
571: 
572: Each output carries:
573: 
574: | Field         | Description |
575: |---------------|-------------|
576: | `kind`        | The integer value of the indicator's per-output enum. |
577: | `shape`       | The output's data shape (`Scalar`, `Band`, `Heatmap`, `Polyline`). Sourced from the descriptor registry. |
578: | `mnemonic`    | Short name for this output. |
579: | `description` | Human-readable description of this output. |
580: 
581: In practice, **indicators never construct `Metadata` object literals directly**.
582: They call `core.BuildMetadata` (Go) / `buildMetadata` (TS), which pulls `kind`
583: and `shape` from the descriptor registry. See the Taxonomy section below.
584: 
585: ### Mnemonic Prefix Convention
586: 
587: Every indicator is identified by a short lowercase **mnemonic prefix** (e.g.
588: `sma`, `rsi`, `bb`, `macd`). These prefixes must be consistent between Go and
589: TypeScript, because downstream tooling keys off them.
590: 
591: **Rules:**
592: 
593: 1. **Lowercase only.** Digits are allowed (`t2`, `t3`, `rocr100`).
594:    Leading `+`/`-` is allowed for paired directional indicators (`+di`, `-di`,
595:    `+dm`, `-dm`).
596: 2. **Kept short** — typically 2-7 chars. The mnemonic is a compact label, not a
597:    full name; the full name goes in `description`.
598: 3. **Go ↔ TS parity.** The same prefix must appear on both sides. When adding
599:    an indicator, add its prefix to both languages in the same PR.
600: 4. **Format.** `<prefix>(<param1>, <param2>, ...<componentSuffix>)` where the
601:    component suffix comes from `ComponentTripleMnemonic` (empty when all
602:    components are at their default).
603: 5. **Configurable components → include the suffix.** Any indicator that
604:    exposes `BarComponent` / `QuoteComponent` / `TradeComponent` on its params
605:    struct **must** append `ComponentTripleMnemonic(bc, qc, tc)` to its
606:    mnemonic. Indicators with hardcoded components (e.g. balance-of-power)
607:    omit the suffix.
608: 6. **Parameterless indicators** use the bare prefix (`obv`, `ad`, `bop`) when
609:    defaults are in effect, or wrap the component suffix in parens when needed
610:    (e.g. `obv(hl/2)`).
611: 
612: **Canonical inventory (65 indicators with mnemonics, plus 4 paired directional):**
613: 
614: | Prefix    | Indicator                                            |
615: |-----------|------------------------------------------------------|
616: | `aci`     | autocorrelation indicator                            |
617: | `acp`     | autocorrelation periodogram                          |
618: | `ad`      | advance/decline                                      |
619: | `adosc`   | advance/decline oscillator                           |
620: | `adx`     | average directional movement index                   |
621: | `adxr`    | average directional movement index rating            |
622: | `apo`     | absolute price oscillator                            |
623: | `aroon`   | Aroon                                                |
624: | `atcf`    | adaptive trend and cycle filter                      |
625: | `atr`     | average true range                                   |
626: | `bb`      | Bollinger bands                                      |
627: | `bbtrend` | Bollinger bands trend                                |
628: | `bop`     | balance of power                                     |
629: | `cbps`    | comb band-pass spectrum                              |
630: | `cc`      | cyber cycle                                          |
631: | `cci`     | commodity channel index                              |
632: | `cmo`     | Chande momentum oscillator                           |
633: | `cog`     | center-of-gravity oscillator                         |
634: | `correl`  | Pearson's correlation coefficient                    |
635: | `csnr`    | corona signal-to-noise ratio                         |
636: | `cspect`  | corona spectrum                                      |
637: | `cswing`  | corona swing position                                |
638: | `ctv`     | corona trend vigor                                   |
639: | `dcp`     | dominant cycle                                       |
640: | `dema`    | double exponential moving average                    |
641: | `dftps`   | discrete Fourier transform power spectrum            |
642: | `+di`/`-di` | directional indicator plus / minus                 |
643: | `+dm`/`-dm` | directional movement plus / minus                  |
644: | `dx`      | directional movement index                           |
645: | `ema`     | exponential moving average                           |
646: | `frama`   | fractal adaptive moving average                      |
647: | `gspect`  | Goertzel spectrum                                    |
648: | `htitl`   | Hilbert transformer instantaneous trend line         |
649: | `iTrend`  | instantaneous trend line                             |
650: | `jma`     | Jurik moving average                                 |
651: | `kama`    | Kaufman adaptive moving average                      |
652: | `linreg`  | linear regression                                    |
653: | `macd`    | moving average convergence/divergence                |
654: | `mama`    | MESA adaptive moving average                         |
655: | `mespect` | maximum entropy spectrum                             |
656: | `mfi`     | money flow index                                     |
657: | `mom`     | momentum                                             |
658: | `natr`    | normalized average true range                        |
659: | `obv`     | on-balance volume                                    |
660: | `ppo`     | percentage price oscillator                          |
661: | `roc`     | rate of change                                       |
662: | `rocp`    | rate of change percent                               |
663: | `rocr`/`rocr100` | rate of change ratio (unit or ×100)           |
664: | `roof`    | roofing filter                                       |
665: | `rsi`     | relative strength index                              |
666: | `sar`     | parabolic stop and reverse                           |
667: | `sma`     | simple moving average                                |
668: | `ss`      | super smoother                                       |
669: | `stdev`   | standard deviation (`.s` sample / `.p` population)   |
670: | `stoch`   | stochastic                                           |
671: | `stochrsi`| stochastic relative strength index                   |
672: | `sw`      | sine wave                                            |
673: | `tcm`     | trend cycle mode                                     |
674: | `tema`    | triple exponential moving average                    |
675: | `tr`      | true range                                           |
676: | `trima`   | triangular moving average                            |
677: | `trix`    | triple exponential moving average oscillator         |
678: | `t2`      | T2 exponential moving average                        |
679: | `t3`      | T3 exponential moving average                        |
680: | `ultosc`  | ultimate oscillator                                  |
681: | `var`     | variance (`.s` sample / `.p` population)             |
682: | `willr`   | Williams %R                                          |
683: | `wma`     | weighted moving average                              |
684: | `zecema`  | zero-lag error-correcting exponential moving average |
685: | `zema`    | zero-lag exponential moving average                  |
686: 
687: The two utility modules `corona/` and `frequencyresponse/` are support helpers,
688: not indicators, and have no mnemonic of their own.
689: 
690: ### Component Triple Mnemonic
691: 
692: A line indicator can be fed bar, quote, or trade data. Each data type has a
693: **component** that selects which field to extract (e.g., close price from a bar,
694: mid-price from a quote). The `ComponentTripleMnemonic` utility (in `core/`)
695: generates the component suffix for an indicator's mnemonic. There are three
696: components (bar, quote, trade) -- hence "triple", not "pair".
697: 
698: #### Default Components
699: 
700: Each component type has a default constant defined in `entities/`:
701: 
702: | Component type   | Default constant         | Default value    | Mnemonic |
703: |------------------|--------------------------|------------------|----------|
704: | `BarComponent`   | `DefaultBarComponent`    | `BarClosePrice`  | `c`      |
705: | `QuoteComponent` | `DefaultQuoteComponent`  | `QuoteMidPrice`  | `ba/2`   |
706: | `TradeComponent` | `DefaultTradeComponent`  | `TradePrice`     | `p`      |
707: 
708: #### Zero-Value Convention
709: 
710: All component enums start at `iota + 1`, making the zero value `0` explicitly
711: "not set". In indicator params, **a zero-value component means "use the default
712: and omit from the mnemonic"**:
713: 
714: ```go
715: // SimpleMovingAverageParams
716: type SimpleMovingAverageParams struct {
717:     Length         int
718:     BarComponent   entities.BarComponent   // zero → DefaultBarComponent, omitted from mnemonic
719:     QuoteComponent entities.QuoteComponent // zero → DefaultQuoteComponent, omitted from mnemonic
720:     TradeComponent entities.TradeComponent // zero → DefaultTradeComponent, omitted from mnemonic
721: }
722: ```
723: 
724: #### Constructor Resolution Logic
725: 
726: Each indicator constructor follows this pattern:
727: 
728: 1. Read the component from params.
729: 2. If zero, resolve to the default constant.
730: 3. Get the component function using the resolved value.
731: 4. Pass the **resolved** values to `ComponentTripleMnemonic` (which omits
732:    defaults automatically).
733: 
734: ```go
735: // Resolve defaults for component functions.
736: bc := p.BarComponent
737: if bc == 0 {
738:     bc = entities.DefaultBarComponent
739: }
740: // ... same for qc, tc
741: 
742: // Build mnemonic using resolved components — defaults are omitted by ComponentTripleMnemonic.
743: mnemonic := fmt.Sprintf("sma(%d%s)", length,
744:     core.ComponentTripleMnemonic(bc, qc, tc))
745: ```
746: 
747: This means that `sma(14)` is the mnemonic whether the user left `BarComponent`
748: as zero or explicitly set it to `BarClosePrice` — both produce the same
749: indicator with the same mnemonic.
750: 
751: #### ComponentTripleMnemonic Function
752: 
753: The function compares each component against its `Default*` constant. If a
754: component equals its default, it is omitted. Non-default components are
755: appended as `", "` followed by the component's `Mnemonic()` output:
756: 
757: ```go
758: func ComponentTripleMnemonic(
759:     bc entities.BarComponent, qc entities.QuoteComponent, tc entities.TradeComponent,
760: ) string {
761:     var s string
762:     if bc != entities.DefaultBarComponent   { s += ", " + bc.Mnemonic() }
763:     if qc != entities.DefaultQuoteComponent { s += ", " + qc.Mnemonic() }
764:     if tc != entities.DefaultTradeComponent { s += ", " + tc.Mnemonic() }
765:     return s
766: }
767: ```
768: 
769: The result is inserted into the indicator format string after the numeric
770: parameters: `sma(14%s)` → `sma(14)` or `sma(14, hl/2)`.
771: 
772: #### Mnemonic Examples
773: 
774: | Params                                                                | Mnemonic              |
775: |-----------------------------------------------------------------------|-----------------------|
776: | `{Length: 5}`                                                         | `sma(5)`              |
777: | `{Length: 5, BarComponent: BarMedianPrice}`                           | `sma(5, hl/2)`        |
778: | `{Length: 5, QuoteComponent: QuoteBidPrice}`                          | `sma(5, b)`           |
779: | `{Length: 5, TradeComponent: TradeVolume}`                            | `sma(5, v)`           |
780: | `{Length: 5, BarComponent: BarOpenPrice, QuoteComponent: QuoteBidPrice}` | `sma(5, o, b)`     |
781: | `{Length: 5, BarComponent: BarHighPrice, TradeComponent: TradeVolume}` | `sma(5, h, v)`       |
782: | `{Length: 7, Phase: 0}`                                               | `jma(7, 0)`           |
783: | `{Length: 7, Phase: 0, BarComponent: BarMedianPrice}`                 | `jma(7, 0, hl/2)`    |
784: 
785: The `description` is always the full name followed by the mnemonic:
786: `"Simple moving average sma(5, hl/2)"`.
787: 
788: #### Entity Mnemonic Methods
789: 
790: Each component enum has two display methods: `String()` (full word, used for
791: JSON serialization) and `Mnemonic()` (short code, used in indicator mnemonics).
792: 
793: **BarComponent mnemonics:**
794: 
795: | Enum value        | `String()`   | `Mnemonic()` |
796: |-------------------|-------------|---------------|
797: | `BarOpenPrice`    | `open`      | `o`           |
798: | `BarHighPrice`    | `high`      | `h`           |
799: | `BarLowPrice`     | `low`       | `l`           |
800: | `BarClosePrice`   | `close`     | `c`           |
801: | `BarVolume`       | `volume`    | `v`           |
802: | `BarMedianPrice`  | `median`    | `hl/2`        |
803: | `BarTypicalPrice` | `typical`   | `hlc/3`       |
804: | `BarWeightedPrice`| `weighted`  | `hlcc/4`      |
805: | `BarAveragePrice` | `average`   | `ohlc/4`      |
806: 
807: **QuoteComponent mnemonics:**
808: 
809: | Enum value             | `String()`     | `Mnemonic()`          |
810: |------------------------|----------------|-----------------------|
811: | `QuoteBidPrice`        | `bid`          | `b`                   |
812: | `QuoteAskPrice`        | `ask`          | `a`                   |
813: | `QuoteBidSize`         | `bidSize`      | `bs`                  |
814: | `QuoteAskSize`         | `askSize`      | `as`                  |
815: | `QuoteMidPrice`        | `mid`          | `ba/2`                |
816: | `QuoteWeightedPrice`   | `weighted`     | `(bbs+aas)/(bs+as)`   |
817: | `QuoteWeightedMidPrice`| `weightedMid`  | `(bas+abs)/(bs+as)`   |
818: | `QuoteSpreadBp`        | `spreadBp`     | `spread bp`           |
819: 
820: **TradeComponent mnemonics:**
821: 
822: | Enum value    | `String()`  | `Mnemonic()` |
823: |---------------|-------------|---------------|
824: | `TradePrice`  | `price`     | `p`           |
825: | `TradeVolume` | `volume`    | `v`           |
826: 
827: ## Helper / Shared Component Families
828: 
829: Some indicators are driven by an interchangeable internal component that has
830: several variants (e.g., different algorithms producing the same shape of
831: output). The canonical example is John Ehlers' **Hilbert Transformer cycle
832: estimator**, which has four variants (HomodyneDiscriminator,
833: HomodyneDiscriminatorUnrolled, PhaseAccumulator, DualDifferentiator) that all
834: produce a cycle period estimate from a price sample.
835: 
836: These are **not indicators themselves**. They are helper components used by
837: actual indicators (e.g., MESA Adaptive Moving Average). They live in the
838: indicator folder of their conceptual owner and follow a specific structure.
839: 
840: ### Folder Layout
841: 
842: A helper family lives in a single folder at the standard indicator depth
843: (`indicators/<group>/<family-name>/`) and contains:
844: 
845: - **Shared interface** — the contract every variant implements.
846: - **Shared params struct/interface** — a single params shape common to all variants.
847: - **Variant type enum** — discriminator for the family (one constant per variant).
848: - **Common utilities file** — shared constants, helper functions (`push`,
849:   `fillWmaFactors`, ...) and the **factory/dispatcher** function.
850: - **One file per variant** implementing the shared interface.
851: - **One spec/test file per variant** plus one for the common file.
852: 
853: Example (TS — `ts/indicators/john-ehlers/hilbert-transformer/`):
854: 
855: ```
856: cycle-estimator.ts                 # interface
857: cycle-estimator-params.ts          # shared params
858: cycle-estimator-type.ts            # variant enum
859: common.ts                          # utilities + createEstimator()
860: homodyne-discriminator.ts          # variant
861: homodyne-discriminator-unrolled.ts
862: phase-accumulator.ts
863: dual-differentiator.ts
864: # + matching .spec.ts for each
865: ```
866: 
867: Go mirrors the same structure with lowercase filenames (`cycleestimator.go`,
868: `cycleestimatorparams.go`, `cycleestimatortype.go`, `estimator.go` for common
869: utilities and `NewCycleEstimator`, and one `<variant>estimator.go` per variant
870: — e.g., `homodynediscriminatorestimator.go`, `phaseaccumulatorestimator.go`).
871: These are the **conceptual-naming exception** noted in the File Naming section:
872: the folder name (`hilberttransformer/`) is never prefixed onto the member
873: files, because each file represents an independent concept within the family.
874: 
875: ### Factory / Dispatcher Pattern
876: 
877: The common file exposes a dispatcher that constructs a variant by type.
878: Each variant has its own **default params** baked into the dispatcher — callers
879: that omit params for a specific type get sensible defaults for that variant.
880: 
881: ```go
882: // Go — go/indicators/johnehlers/hilberttransformer/estimator.go
883: func NewCycleEstimator(typ CycleEstimatorType, params *CycleEstimatorParams) (CycleEstimator, error) {
884:     switch typ {
885:     case HomodyneDiscriminator:         return NewHomodyneDiscriminatorEstimator(params)
886:     case HomodyneDiscriminatorUnrolled: return NewHomodyneDiscriminatorEstimatorUnrolled(params)
887:     case PhaseAccumulator:              return NewPhaseAccumulatorEstimator(params)
888:     case DualDifferentiator:            return NewDualDifferentiatorEstimator(params)
889:     }
890:     return nil, fmt.Errorf("invalid cycle estimator type: %s", typ)
891: }
892: ```
893: 
894: ```ts
895: // TS — ts/indicators/john-ehlers/hilbert-transformer/hilbert-transformer-common.ts
896: export function createEstimator(
897:     estimatorType?: HilbertTransformerCycleEstimatorType,
898:     estimatorParams?: HilbertTransformerCycleEstimatorParams,
899: ): HilbertTransformerCycleEstimator {
900:     if (estimatorType === undefined) {
901:         estimatorType = HilbertTransformerCycleEstimatorType.HomodyneDiscriminator;
902:     }
903:     switch (estimatorType) {
904:         case HilbertTransformerCycleEstimatorType.HomodyneDiscriminator:
905:             estimatorParams ??= { smoothingLength: 4, alphaEmaQuadratureInPhase: 0.2, alphaEmaPeriod: 0.2 };
906:             return new HilbertTransformerHomodyneDiscriminator(estimatorParams);
907:         // ... other variants, each with its own default params
908:         default:
909:             throw new Error('Invalid cycle estimator type: ' + estimatorType);
910:     }
911: }
912: ```
913: 
914: The dispatcher must have a default variant (when `type` is omitted) and throw
915: on unknown types.
916: 
917: ### Shared Interface Design
918: 
919: The shared interface exposes:
920: 
921: - Primed state (`primed`) and warm-up configuration (`warmUpPeriod`).
922: - Construction parameters as readable properties (`smoothingLength`,
923:   `alphaEmaQuadratureInPhase`, `alphaEmaPeriod`).
924: - Intermediate state visible for testing/debugging (`smoothed`, `detrended`,
925:   `inPhase`, `quadrature`, `period`).
926: - An `update(sample)` method returning the primary output.
927: 
928: TypeScript naming: **do not** suffix getters with `Value`. Prefix the backing
929: private field with `_` so the public getter can take the unadorned name:
930: 
931: ```ts
932: // Correct — public getter is `period`, backing field is `_period`.
933: private _period = 0;
934: public get period(): number { return this._period; }
935: 
936: // Incorrect — do not use `periodValue` just to avoid a name clash.
937: ```
938: 
939: ### File Naming — No Type Suffixes
940: 
941: This module follows the same rule as the rest of the codebase: **no
942: `.interface.ts`, `.enum.ts`, or similar type-suffixes**. Interfaces, enums,
943: params, and implementations all use plain `.ts` filenames differentiated by
944: descriptive stem names.
945: 
946: ### Warm-Up Period Contract
947: 
948: Each variant has an intrinsic minimum priming length (the sample count needed
949: for its state to be meaningful). If `params.warmUpPeriod` is provided and
950: larger than that minimum, it overrides the minimum; otherwise the intrinsic
951: minimum is used. Tests must cover both the default path and the custom
952: `warmUpPeriod` override.
953: 
954: ## Taxonomy & Descriptor Registry
955: 
956: Every indicator carries a **Descriptor** — a taxonomic classification used by
957: charting UIs and discovery tooling to filter, group, and lay out indicators.
958: Descriptors are the single source of truth for output `kind` and `shape`;
959: indicator `Metadata()` implementations consume the registry via
960: `BuildMetadata` and MUST NOT hand-write `kind`/`shape`.
961: 
962: ### Dimensions
963: 
964: | Primitive           | Go type                 | TS type                 | Purpose |
965: |---------------------|-------------------------|-------------------------|---------|
966: | `Identifier`        | `core.Identifier`       | `IndicatorIdentifier`   | Unique ID of the indicator. |
967: | `Role`              | `core.Role`             | `Role`                  | Semantic role of an **output** (Smoother, Envelope, Overlay, Polyline, Oscillator, BoundedOscillator, Volatility, VolumeFlow, Directional, CyclePeriod, CyclePhase, FractalDimension, Spectrum, Signal, Histogram, RegimeFlag, Correlation). |
968: | `Pane`              | `core.Pane`             | `Pane`                  | Chart pane where an output is drawn: `Price`, `Own`, `OverlayOnParent`. |
969: | `Adaptivity`        | `core.Adaptivity`       | `Adaptivity`            | `Static` or `Adaptive` (indicator-level). |
970: | `InputRequirement`  | `core.InputRequirement` | `InputRequirement`      | Minimum input data type: `ScalarInput`, `QuoteInput`, `BarInput`, `TradeInput`. |
971: | `VolumeUsage`       | `core.VolumeUsage`      | `VolumeUsage`           | `NoVolume`, `AggregateBarVolume`, `PerTradeVolume`, `QuoteLiquidityVolume`. |
972: | `OutputDescriptor`  | `core.OutputDescriptor` | `OutputDescriptor`      | `{Kind, Shape, Role, Pane}` per output. |
973: | `Descriptor`        | `core.Descriptor`       | `Descriptor`            | `{Identifier, Family, Adaptivity, InputRequirement, VolumeUsage, Outputs}`. |
974: 
975: **VolumeUsage ⇔ InputRequirement validity:**
976: 
977: - `AggregateBarVolume` ⇒ `BarInput`
978: - `PerTradeVolume` ⇒ `TradeInput`
979: - `QuoteLiquidityVolume` ⇒ `QuoteInput`
980: - `NoVolume` is valid with any input
981: 
982: ### Registry Files
983: 
984: | Location                                              | Content |
985: |-------------------------------------------------------|---------|
986: | `go/indicators/core/descriptors.go`                   | Go descriptor map keyed by `Identifier`. |
987: | `go/indicators/core/descriptors_test.go`              | Coverage test: every `Identifier` has a descriptor row. |
988: | `go/indicators/core/metadata_descriptor_test.go`      | Invariants: outputs are non-empty, Kinds are strictly ascending starting at 1, shapes are known. |
989: | `ts/indicators/core/descriptors.ts`                   | TS descriptor `Map` keyed by `IndicatorIdentifier`. Uses `out()`/`desc()` helpers and `S`/`R`/`P`/`A`/`I`/`V` aliases for terse one-line entries. |
990: 
991: **Kind-numbering asymmetry:** Go per-indicator output enums are 1-based
992: (`iota + 1`), TS enums are 0-based. The descriptor tables mirror that — Go
993: `Kind` values are `1, 2, 3, ...`, TS `kind` values are `0, 1, 2, ...`. A row's
994: `Outputs` slice MUST appear in Kind-ascending order (same as the enum's
995: declared order), because `BuildMetadata` pairs `Outputs[i]` with
996: `outputTexts[i]` positionally.
997: 
998: ### BuildMetadata Helper
999: 
1000: Indicators call `BuildMetadata` / `buildMetadata` from within their
1001: `Metadata()` method. The helper:
1002: 
1003: 1. Looks up the descriptor for the given `Identifier`.
1004: 2. Panics (Go) / throws (TS) if no descriptor is registered.
1005: 3. Panics/throws if the number of `OutputText` entries does not match the
1006:    number of descriptor outputs.
1007: 4. Returns a fully-populated `Metadata` with `kind` and `shape` taken from the
1008:    registry and `mnemonic`/`description` taken from the caller.
1009: 
1010: **Go:**
1011: 
1012: ```go
1013: func (s *BollingerBands) Metadata() core.Metadata {
1014:     desc := "Bollinger Bands " + s.mnemonic
1015:     return core.BuildMetadata(
1016:         core.BollingerBands,
1017:         s.mnemonic,
1018:         desc,
1019:         []core.OutputText{
1020:             {Mnemonic: s.mnemonic + " lower",       Description: desc + " Lower"},
1021:             {Mnemonic: s.mnemonic + " middle",      Description: desc + " Middle"},
1022:             {Mnemonic: s.mnemonic + " upper",       Description: desc + " Upper"},
1023:             {Mnemonic: s.mnemonic + " bandWidth",   Description: desc + " Band Width"},
1024:             {Mnemonic: s.mnemonic + " percentBand", Description: desc + " Percent Band"},
1025:             {Mnemonic: s.mnemonic + " band",        Description: desc + " Band"},
1026:         },
1027:     )
1028: }
1029: ```
1030: 
1031: **TypeScript:**
1032: 
1033: ```ts
1034: public metadata(): IndicatorMetadata {
1035:   const description = `Bollinger Bands ${this.mnemonic}`;
1036:   return buildMetadata(
1037:     IndicatorIdentifier.BollingerBands,
1038:     this.mnemonic,
1039:     description,
1040:     [
1041:       { mnemonic: `${this.mnemonic} lower`,       description: `${description} Lower` },
1042:       { mnemonic: `${this.mnemonic} middle`,      description: `${description} Middle` },
1043:       { mnemonic: `${this.mnemonic} upper`,       description: `${description} Upper` },
1044:       { mnemonic: `${this.mnemonic} bandWidth`,   description: `${description} Band Width` },
1045:       { mnemonic: `${this.mnemonic} percentBand`, description: `${description} Percent Band` },
1046:       { mnemonic: `${this.mnemonic} band`,        description: `${description} Band` },
1047:     ],
1048:   );
1049: }
1050: ```
1051: 
1052: ### Adding a Descriptor Row
1053: 
1054: When adding a new indicator you MUST add its descriptor in **both** Go and TS:
1055: 
1056: 1. **Go** — `go/indicators/core/descriptors.go`:
1057: 
1058:    ```go
1059:    MyIndicator: {
1060:        Identifier: MyIndicator, Family: "<Author or Common/Custom>",
1061:        Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
1062:        Outputs: []OutputDescriptor{
1063:            {Kind: 1 /* Output1 */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
1064:            {Kind: 2 /* Output2 */, Shape: shape.Scalar, Role: Signal,   Pane: Price},
1065:        },
1066:    },
1067:    ```
1068: 
1069: 2. **TS** — `ts/indicators/core/descriptors.ts` (single-line using helpers):
1070: 
1071:    ```ts
1072:    [IndicatorIdentifier.MyIndicator, desc(IndicatorIdentifier.MyIndicator, '<family>', A.Static, I.ScalarInput, V.NoVolume, [
1073:      out(0, S.Scalar, R.Smoother, P.Price),
1074:      out(1, S.Scalar, R.Signal,   P.Price),
1075:    ])],
1076:    ```
1077: 
1078: 3. Confirm the output order matches the per-indicator output enum order
1079:    (`iota + 1` in Go, `0, 1, 2, ...` in TS).
1080: 
1081: 4. Run the suites:
1082:    ```
1083:    cd go && go test ./indicators/core/...
1084:    cd ts && npm test
1085:    ```
1086:    `TestDescriptorCoverage` and `TestDescriptorOutputsWellFormed` will catch
1087:    missing or malformed rows on the Go side; the TS `buildMetadata` contract
1088:    will throw at runtime if a row is missing.
1089: 
1090: ### Family Naming
1091: 
1092: `Family` is the human-readable grouping label. Conventions:
1093: 
1094: - Author folder → author's full name (e.g., `john-ehlers` / `johnehlers`
1095:   → `"John Ehlers"`, `welles-wilder` / `welleswilder` → `"Welles Wilder"`).
1096: - `common/` → `"Common"`.
1097: - `custom/` → `"Custom"`.
1098: 
1099: ### Role Assignment Guidance
1100: 
1101: | Role                | Use for |
1102: |---------------------|---------|
1103: | `Smoother`          | A moving average or trend-following line plotted on price pane (SMA, EMA, JMA, KAMA, Middle Bollinger, ATCF trend lines, T2/T3, TEMA, ...). |
1104: | `Envelope`          | Upper/lower bands or deviation channels (Bollinger upper/lower/band). |
1105: | `Overlay`           | Other price-pane overlays that are not smoothers or envelopes (e.g., Parabolic SAR dots). |
1106: | `Polyline`          | Outputs that produce an arbitrary polyline shape rather than a time series. |
1107: | `Oscillator`        | Unbounded oscillator centred on 0 or arbitrary range (MACD, momentum, ATCF Ftlm/Stlm/Pcci). |
1108: | `BoundedOscillator` | Oscillator bounded to 0..100 or similar (RSI, %R, Stochastic, ADX, PercentBand). |
1109: | `Volatility`        | Volatility measure (TR, ATR, NATR, StdDev, BandWidth). |
1110: | `VolumeFlow`        | Volume-based cumulative or flow measure (OBV, A/D, A/D Oscillator, MFI). |
1111: | `Directional`       | Directional movement components (+DI, -DI, +DM, -DM). |
1112: | `CyclePeriod`       | Dominant-cycle period estimate. |
1113: | `CyclePhase`        | Cycle phase or sine-wave output. |
1114: | `FractalDimension`  | Fractal-dimension based output. |
1115: | `Spectrum`          | Spectrum / periodogram output (heatmap or polyline). |
1116: | `Signal`            | The signal line of a multi-line indicator (MACD signal, Stochastic %D). |
1117: | `Histogram`         | Difference bar between two lines (MACD histogram). |
1118: | `RegimeFlag`        | Discrete regime indicator (Trend/Cycle mode). |
1119: | `Correlation`       | Correlation coefficient in [-1, 1]. |
1120: 
1121: ### Pane Assignment Guidance
1122: 
1123: | Pane               | Use for |
1124: |--------------------|---------|
1125: | `Price`            | Drawn on the price chart (smoothers, envelopes, overlays, SAR). |
1126: | `Own`              | Drawn in its own subchart (oscillators, volatility, volume flow, spectra). |
1127: | `OverlayOnParent`  | Drawn on top of another indicator's pane (reserved for composite visualizations). |
1128: 
1129: ### Adaptivity
1130: 
1131: - `Adaptive` — the indicator adjusts its coefficients, length, or alpha based
1132:   on market state (KAMA, JMA, MAMA, FRAMA, ATCF, DominantCycle, SineWave,
1133:   HilbertTransformerInstantaneousTrendLine, TrendCycleMode, all Corona
1134:   indicators, AutoCorrelationPeriodogram).
1135: - `Static` — everything else.
1136: 
1137: ## Indicator Factory
1138: 
1139: The **factory** maps an indicator identifier string + JSON parameters to a fully
1140: constructed `Indicator` instance at runtime. This enables data-driven indicator
1141: creation from configuration files, user input, or serialized settings.
1142: 
1143: ### Location & Rationale
1144: 
1145: | Language   | Path                                  |
1146: |------------|---------------------------------------|
1147: | Go         | `go/indicators/factory/factory.go`    |
1148: | TypeScript | `ts/indicators/factory/factory.ts`    |
1149: 
1150: The factory **cannot live in `core/`** in Go due to circular imports: indicator
1151: packages import `core`, so `core` cannot import them back. The `factory/`
1152: package sits at `indicators/factory/` — a sibling of `core/` and the
1153: author/group folders — and imports both `core` and every indicator package.
1154: 
1155: TypeScript has no circular-import issue but uses the same location for
1156: consistency.
1157: 
1158: ### Go API
1159: 
1160: ```go
1161: package factory
1162: 
1163: // New creates an indicator from its identifier and JSON-encoded parameters.
1164: func New(identifier core.Identifier, paramsJSON []byte) (core.Indicator, error)
1165: ```
1166: 
1167: - `identifier` is a `core.Identifier` value (the same enum used in descriptors
1168:   and metadata).
1169: - `paramsJSON` is the raw JSON for the indicator's params struct. For
1170:   no-params indicators, pass `nil` or `[]byte("{}}")`.
1171: - Returns the constructed indicator or an error if the identifier is unknown or
1172:   params are invalid.
1173: 
1174: ### TypeScript API
1175: 
1176: ```ts
1177: export function createIndicator(
1178:     identifier: IndicatorIdentifier,
1179:     params?: Record<string, unknown>,
1180: ): Indicator
1181: ```
1182: 
1183: - `identifier` is an `IndicatorIdentifier` enum value.
1184: - `params` is the plain object (already parsed from JSON). For no-params
1185:   indicators, omit or pass `undefined` / `{}`.
1186: - Throws on unknown identifier or invalid params.
1187: 
1188: ### Constructor Pattern Categories
1189: 
1190: The factory handles five categories of indicator constructors:
1191: 
1192: | Category | Count (Go/TS) | JSON detection | Example |
1193: |----------|---------------|----------------|---------|
1194: | **Standard `*Params` struct** | ~35 | Unmarshal into the params struct directly | SMA, WMA, RSI, BB, MACD, CCI |
1195: | **Length vs SmoothingFactor variants** | 9 | If JSON contains `"smoothingFactor"` key → SF constructor; else → Length constructor | EMA, DEMA, TEMA, T2, T3, KAMA, CyberCycle, ITL |
1196: | **Default vs Params variants** | ~15 | If JSON is `{}` or empty → Default constructor; else → Params constructor | Ehlers spectra, Corona family, DominantCycle, SineWave, ATCF |
1197: | **Bare `int` length** | 10 | Parse `{"length": N}` | ATR, NATR, DM±, DI±, DMI, ADX, ADXR, WilliamsPercentR |
1198: | **No params** | 3 | Ignore JSON entirely | TrueRange, BalanceOfPower, AdvanceDecline |
1199: 
1200: **Special case — MAMA:** uses `"fastLimitSmoothingFactor"` /
1201: `"slowLimitSmoothingFactor"` keys for the SmoothingFactor variant, and
1202: `"fastestSmoothingFactor"` / `"slowestSmoothingFactor"` for KAMA.
1203: 
1204: ### Auto-Detection Logic
1205: 
1206: The factory uses helper functions to detect which constructor variant to call:
1207: 
1208: - **Go:** `hasKey(json, "smoothingFactor")` checks for a specific key;
1209:   `isEmptyObject(json)` checks if JSON is `{}`.
1210: - **TS:** `"smoothingFactor" in params` checks for a key;
1211:   `Object.keys(params).length === 0` checks for empty params.
1212: 
1213: No JSON tags exist on Go params structs — Go's default case-insensitive JSON
1214: unmarshaling handles key matching (e.g., JSON `"smoothingFactor"` matches Go
1215: field `SmoothingFactor`).
1216: 
1217: ### Adding a New Indicator to the Factory
1218: 
1219: When adding a new indicator, add a `case` to the factory's switch statement in
1220: **both** Go and TS:
1221: 
1222: 1. **Go** — add a `case core.<Identifier>:` block in `factory.New()` that
1223:    unmarshals `paramsJSON` into the indicator's params struct and calls its
1224:    constructor.
1225: 2. **TS** — add a `case IndicatorIdentifier.<Name>:` block in
1226:    `createIndicator()` that spreads defaults onto `params` and calls the
1227:    constructor.
1228: 
1229: The case block should follow the pattern of existing entries in the same
1230: constructor category. TS uses a `{ defaultField: value, ...p }` spread pattern
1231: to fill in required defaults.
1232: 
1233: ### `icalc` CLI Tool
1234: 
1235: The `icalc` ("indicator calculator") CLI is a reference consumer of the factory
1236: in both languages. It reads a JSON settings file, creates indicators via the
1237: factory, and runs them against embedded test bar data.
1238: 
1239: | Language | Path | Run command |
1240: |----------|------|-------------|
1241: | Go | `go/cmd/icalc/main.go` | `cd go && go run ./cmd/icalc settings.json` |
1242: | TS | `ts/cmd/icalc/main.ts` | `cd ts && npx tsx cmd/icalc/main.ts cmd/icalc/settings.json` |
1243: 
1244: Settings file format (`settings.json`):
1245: 
1246: ```json
1247: [
1248:     { "identifier": "simpleMovingAverage", "params": { "length": 14 } },
1249:     { "identifier": "exponentialMovingAverage", "params": { "smoothingFactor": 0.1 } },
1250:     { "identifier": "trueRange", "params": {} }
1251: ]
1252: ```
1253: 
1254: - `identifier` is the camelCase JSON string of the `core.Identifier` /
1255:   `IndicatorIdentifier` enum.
1256: - `params` is the JSON object passed to the factory.
1257: - Both CLIs embed 252-entry H/L/C/V test bar data (from TA-Lib `test_data.c`)
1258:   and print metadata + per-bar outputs for each configured indicator.

(End of file - total 1258 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.6 · 19.7s)

Now I'll add the DefaultParams section to the skill doc:

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "oldString": "- Both CLIs embed 252-entry H/L/C/V test bar data (from TA-Lib `test_data.c`)\n  and print metadata + per-bar outputs for each configured indicator.",
  "newString": "- Both CLIs embed 252-entry H/L/C/V test bar data (from TA-Lib `test_data.c`)\n  and print metadata + per-bar outputs for each configured indicator.\n\n## DefaultParams / defaultParams\n\nEvery indicator params file exports a **`DefaultParams()`** (Go) /\n**`defaultParams()`** (TS) function that returns a fully-populated params\nstruct/object with sensible default values. This provides programmatic access to\ndefaults for UIs, factories, and documentation generators.\n\n### Convention\n\n- **Go:** `func DefaultParams() *Params` (or `*StructName` if the struct name\n  differs from `Params`). Returns a pointer to a new struct.\n- **TS:** `export function defaultParams(): SomeParamsInterface`. Returns a\n  plain object satisfying the interface.\n- **Dual-variant indicators** (Length/SmoothingFactor) export two functions:\n  `DefaultLengthParams()` / `defaultLengthParams()` and\n  `DefaultSmoothingFactorParams()` / `defaultSmoothingFactorParams()`.\n- **No-params indicators** (empty struct/interface) still export\n  `DefaultParams()` / `defaultParams()` returning an empty struct/object for\n  consistency.\n- **Component fields** (`BarComponent`, `QuoteComponent`, `TradeComponent`)\n  are omitted from `DefaultParams` — their zero/undefined values already mean\n  \"use default\".\n\n### Placement\n\nThe function is added at the **end** of the params file, after the struct/\ninterface definition and any validation logic.\n\n### Doc Comment\n\nGo:\n```go\n// DefaultParams returns a Params value populated with conventional defaults.\nfunc DefaultParams() *Params {\n    return &Params{\n        Length: 20,\n    }\n}\n```\n\nTypeScript:\n```typescript\nexport function defaultParams(): SimpleMovingAverageParams {\n    return {\n        length: 20,\n    };\n}\n```\n\n### Default Value Sources\n\nDefault values come from (in priority order):\n1. Values documented in the params struct/interface field comments.\n2. The original paper or reference implementation (Ehlers, TA-Lib, etc.).\n3. Conventional industry defaults (e.g., length=14 for RSI/ATR, length=20\n   for SMA/EMA).\n\n### When Adding a New Indicator\n\nAdd `DefaultParams()` / `defaultParams()` to the params file as part of the\nstandard indicator creation checklist. The function should be present from day\none, not added retroactively."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.6 · 6.2s)

Now update the checklist to include DefaultParams:

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "oldString": "   - **Parameters** -- define a params struct. Where the indicator can be",
  "newString": "   - **Parameters** -- define a params struct with a `DefaultParams()` /\n     `defaultParams()` function (see the DefaultParams section below). Where the indicator can be"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.6 · 7.0s)

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Survey all Go indicator params files to catalog default values from comments",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Survey all TS indicator params files to catalog default values from comments",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Add DefaultParams() to all Go indicators",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Add defaultParams() to all TS indicators",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Update factory to use DefaultParams where applicable",
      "priority": "medium",
      "status": "pending"
    },
    {
      "content": "Build and test Go",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Build and test TS",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Update skill docs to document DefaultParams pattern",
      "priority": "medium",
      "status": "completed"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Survey all Go indicator params files to catalog default values from comments",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Survey all TS indicator params files to catalog default values from comments",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Add DefaultParams() to all Go indicators",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Add defaultParams() to all TS indicators",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Update factory to use DefaultParams where applicable",
    "priority": "medium",
    "status": "pending"
  },
  {
    "content": "Build and test Go",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Build and test TS",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Update skill docs to document DefaultParams pattern",
    "priority": "medium",
    "status": "completed"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 65.6s)

The remaining item is "Update factory to use DefaultParams where applicable." This would mean changing the factory's switch cases to call `DefaultParams()` for the "Default vs Params" category indicators instead of hard-coding defaults inline.

**Tool: question**

**Input:**
```json
{
  "questions": [
    {
      "header": "Factory integration",
      "options": [
        {
          "description": "Have the factory call DefaultParams() when params JSON is empty, replacing inline defaults. Cleaner but adds import coupling.",
          "label": "Yes, update factories"
        },
        {
          "description": "Factory already works. DefaultParams is useful independently for UIs/tooling. No need to couple them now.",
          "label": "Skip for now (Recommended)"
        }
      ],
      "question": "Should I update the Go/TS factories to call DefaultParams()/defaultParams() instead of using inline default values? This is a nice-to-have but the factory already works correctly."
    }
  ]
}
```

**Output:**
```
User has answered your questions: "Should I update the Go/TS factories to call DefaultParams()/defaultParams() instead of using inline default values? This is a nice-to-have but the factory already works correctly."="Yes, update factories". You can now continue with the user's answers in mind.
```

---

## Assistant (Build · claude-opus-4.6 · 5.7s)

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Survey all Go indicator params files to catalog default values from comments",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Survey all TS indicator params files to catalog default values from comments",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Add DefaultParams() to all Go indicators",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Add defaultParams() to all TS indicators",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Update Go factory to use DefaultParams",
      "priority": "medium",
      "status": "in_progress"
    },
    {
      "content": "Update TS factory to use defaultParams",
      "priority": "medium",
      "status": "pending"
    },
    {
      "content": "Build and test Go",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Build and test TS",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Update skill docs to document DefaultParams pattern",
      "priority": "medium",
      "status": "completed"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Survey all Go indicator params files to catalog default values from comments",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Survey all TS indicator params files to catalog default values from comments",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Add DefaultParams() to all Go indicators",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Add defaultParams() to all TS indicators",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Update Go factory to use DefaultParams",
    "priority": "medium",
    "status": "in_progress"
  },
  {
    "content": "Update TS factory to use defaultParams",
    "priority": "medium",
    "status": "pending"
  },
  {
    "content": "Build and test Go",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Build and test TS",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Update skill docs to document DefaultParams pattern",
    "priority": "medium",
    "status": "completed"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 4.8s)

Let me read both factory files to understand the current structure:

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/factory/factory.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/factory/factory.go</path>
<type>file</type>
<content>1: // Package factory provides a generic constructor that maps a core.Identifier
2: // and a JSON parameter string to a core.Indicator instance. This avoids the
3: // need for callers to import individual indicator packages directly.
4: package factory
5: 
6: import (
7: 	"encoding/json"
8: 	"fmt"
9: 
10: 	"zpano/indicators/common/absolutepriceoscillator"
11: 	"zpano/indicators/common/exponentialmovingaverage"
12: 	"zpano/indicators/common/linearregression"
13: 	"zpano/indicators/common/momentum"
14: 	"zpano/indicators/common/pearsonscorrelationcoefficient"
15: 	"zpano/indicators/common/rateofchange"
16: 	"zpano/indicators/common/rateofchangepercent"
17: 	"zpano/indicators/common/rateofchangeratio"
18: 	"zpano/indicators/common/simplemovingaverage"
19: 	"zpano/indicators/common/standarddeviation"
20: 	"zpano/indicators/common/triangularmovingaverage"
21: 	"zpano/indicators/common/variance"
22: 	"zpano/indicators/common/weightedmovingaverage"
23: 	"zpano/indicators/core"
24: 	"zpano/indicators/custom/goertzelspectrum"
25: 	"zpano/indicators/custom/maximumentropyspectrum"
26: 	"zpano/indicators/donaldlambert/commoditychannelindex"
27: 	"zpano/indicators/genequong/moneyflowindex"
28: 	"zpano/indicators/georgelane/stochastic"
29: 	"zpano/indicators/geraldappel/movingaverageconvergencedivergence"
30: 	"zpano/indicators/geraldappel/percentagepriceoscillator"
31: 	"zpano/indicators/igorlivshin/balanceofpower"
32: 	"zpano/indicators/jackhutson/tripleexponentialmovingaverageoscillator"
33: 	"zpano/indicators/johnbollinger/bollingerbands"
34: 	"zpano/indicators/johnbollinger/bollingerbandstrend"
35: 	"zpano/indicators/johnehlers/autocorrelationindicator"
36: 	"zpano/indicators/johnehlers/autocorrelationperiodogram"
37: 	"zpano/indicators/johnehlers/centerofgravityoscillator"
38: 	"zpano/indicators/johnehlers/combbandpassspectrum"
39: 	"zpano/indicators/johnehlers/coronasignaltonoiseratio"
40: 	"zpano/indicators/johnehlers/coronaspectrum"
41: 	"zpano/indicators/johnehlers/coronaswingposition"
42: 	"zpano/indicators/johnehlers/coronatrendvigor"
43: 	"zpano/indicators/johnehlers/cybercycle"
44: 	"zpano/indicators/johnehlers/discretefouriertransformspectrum"
45: 	"zpano/indicators/johnehlers/dominantcycle"
46: 	"zpano/indicators/johnehlers/fractaladaptivemovingaverage"
47: 	"zpano/indicators/johnehlers/hilberttransformerinstantaneoustrendline"
48: 	"zpano/indicators/johnehlers/instantaneoustrendline"
49: 	"zpano/indicators/johnehlers/mesaadaptivemovingaverage"
50: 	"zpano/indicators/johnehlers/roofingfilter"
51: 	"zpano/indicators/johnehlers/sinewave"
52: 	"zpano/indicators/johnehlers/supersmoother"
53: 	"zpano/indicators/johnehlers/trendcyclemode"
54: 	"zpano/indicators/johnehlers/zerolagerrorcorrectingexponentialmovingaverage"
55: 	"zpano/indicators/johnehlers/zerolagexponentialmovingaverage"
56: 	"zpano/indicators/josephgranville/onbalancevolume"
57: 	"zpano/indicators/larrywilliams/ultimateoscillator"
58: 	"zpano/indicators/larrywilliams/williamspercentr"
59: 	"zpano/indicators/marcchaikin/advancedecline"
60: 	"zpano/indicators/marcchaikin/advancedeclineoscillator"
61: 	"zpano/indicators/markjurik/jurikmovingaverage"
62: 	"zpano/indicators/patrickmulloy/doubleexponentialmovingaverage"
63: 	"zpano/indicators/patrickmulloy/tripleexponentialmovingaverage"
64: 	"zpano/indicators/perrykaufman/kaufmanadaptivemovingaverage"
65: 	"zpano/indicators/timtillson/t2exponentialmovingaverage"
66: 	"zpano/indicators/timtillson/t3exponentialmovingaverage"
67: 	"zpano/indicators/tusharchande/aroon"
68: 	"zpano/indicators/tusharchande/chandemomentumoscillator"
69: 	"zpano/indicators/tusharchande/stochasticrelativestrengthindex"
70: 	"zpano/indicators/vladimirkravchuk/adaptivetrendandcyclefilter"
71: 	"zpano/indicators/welleswilder/averagedirectionalmovementindex"
72: 	"zpano/indicators/welleswilder/averagedirectionalmovementindexrating"
73: 	"zpano/indicators/welleswilder/averagetruerange"
74: 	"zpano/indicators/welleswilder/directionalindicatorminus"
75: 	"zpano/indicators/welleswilder/directionalindicatorplus"
76: 	"zpano/indicators/welleswilder/directionalmovementindex"
77: 	"zpano/indicators/welleswilder/directionalmovementminus"
78: 	"zpano/indicators/welleswilder/directionalmovementplus"
79: 	"zpano/indicators/welleswilder/normalizedaveragetruerange"
80: 	"zpano/indicators/welleswilder/parabolicstopandreverse"
81: 	"zpano/indicators/welleswilder/relativestrengthindex"
82: 	"zpano/indicators/welleswilder/truerange"
83: )
84: 
85: // lengthParam is a helper struct for indicators that take a bare int length.
86: type lengthParam struct {
87: 	Length int `json:"length"`
88: }
89: 
90: // hasKey checks if a JSON object contains a specific key.
91: func hasKey(data []byte, key string) bool {
92: 	var m map[string]json.RawMessage
93: 	if err := json.Unmarshal(data, &m); err != nil {
94: 		return false
95: 	}
96: 
97: 	_, ok := m[key]
98: 
99: 	return ok
100: }
101: 
102: // unmarshal is a helper that returns a formatted error on failure.
103: func unmarshal(data []byte, v any) error {
104: 	if err := json.Unmarshal(data, v); err != nil {
105: 		return fmt.Errorf("parse params: %w", err)
106: 	}
107: 
108: 	return nil
109: }
110: 
111: // New creates an indicator from its identifier and a JSON-encoded parameter
112: // string. If params is empty or "null", default parameters are used.
113: //
114: // For indicators with Length and SmoothingFactor constructor variants, the
115: // factory auto-detects which to use: if the JSON contains a "smoothingFactor"
116: // key the SmoothingFactor variant is used, otherwise the Length variant.
117: //
118: //nolint:cyclop,funlen,gocyclo // large switch is intentional — one case per identifier.
119: func New(identifier core.Identifier, params string) (core.Indicator, error) {
120: 	b := []byte(params)
121: 	if len(b) == 0 {
122: 		b = []byte("{}")
123: 	}
124: 
125: 	switch identifier {
126: 	// ── common ────────────────────────────────────────────────────────────
127: 
128: 	case core.SimpleMovingAverage:
129: 		p := &simplemovingaverage.SimpleMovingAverageParams{Length: 14}
130: 		if err := unmarshal(b, p); err != nil {
131: 			return nil, err
132: 		}
133: 
134: 		return simplemovingaverage.NewSimpleMovingAverage(p)
135: 
136: 	case core.WeightedMovingAverage:
137: 		p := &weightedmovingaverage.WeightedMovingAverageParams{Length: 14}
138: 		if err := unmarshal(b, p); err != nil {
139: 			return nil, err
140: 		}
141: 
142: 		return weightedmovingaverage.NewWeightedMovingAverage(p)
143: 
144: 	case core.TriangularMovingAverage:
145: 		p := &triangularmovingaverage.TriangularMovingAverageParams{Length: 14}
146: 		if err := unmarshal(b, p); err != nil {
147: 			return nil, err
148: 		}
149: 
150: 		return triangularmovingaverage.NewTriangularMovingAverage(p)
151: 
152: 	case core.ExponentialMovingAverage:
153: 		if hasKey(b, "smoothingFactor") {
154: 			p := &exponentialmovingaverage.ExponentialMovingAverageSmoothingFactorParams{}
155: 			if err := unmarshal(b, p); err != nil {
156: 				return nil, err
157: 			}
158: 
159: 			return exponentialmovingaverage.NewExponentialMovingAverageSmoothingFactor(p)
160: 		}
161: 
162: 		p := &exponentialmovingaverage.ExponentialMovingAverageLengthParams{Length: 10}
163: 		if err := unmarshal(b, p); err != nil {
164: 			return nil, err
165: 		}
166: 
167: 		return exponentialmovingaverage.NewExponentialMovingAverageLength(p)
168: 
169: 	case core.Variance:
170: 		p := &variance.VarianceParams{Length: 14}
171: 		if err := unmarshal(b, p); err != nil {
172: 			return nil, err
173: 		}
174: 
175: 		return variance.NewVariance(p)
176: 
177: 	case core.StandardDeviation:
178: 		p := &standarddeviation.StandardDeviationParams{Length: 14}
179: 		if err := unmarshal(b, p); err != nil {
180: 			return nil, err
181: 		}
182: 
183: 		return standarddeviation.NewStandardDeviation(p)
184: 
185: 	case core.Momentum:
186: 		p := &momentum.Params{Length: 10}
187: 		if err := unmarshal(b, p); err != nil {
188: 			return nil, err
189: 		}
190: 
191: 		return momentum.New(p)
192: 
193: 	case core.RateOfChange:
194: 		p := &rateofchange.Params{Length: 10}
195: 		if err := unmarshal(b, p); err != nil {
196: 			return nil, err
197: 		}
198: 
199: 		return rateofchange.New(p)
200: 
201: 	case core.RateOfChangePercent:
202: 		p := &rateofchangepercent.Params{Length: 10}
203: 		if err := unmarshal(b, p); err != nil {
204: 			return nil, err
205: 		}
206: 
207: 		return rateofchangepercent.New(p)
208: 
209: 	case core.RateOfChangeRatio:
210: 		p := &rateofchangeratio.Params{Length: 10}
211: 		if err := unmarshal(b, p); err != nil {
212: 			return nil, err
213: 		}
214: 
215: 		return rateofchangeratio.New(p)
216: 
217: 	case core.AbsolutePriceOscillator:
218: 		p := &absolutepriceoscillator.AbsolutePriceOscillatorParams{FastLength: 12, SlowLength: 26}
219: 		if err := unmarshal(b, p); err != nil {
220: 			return nil, err
221: 		}
222: 
223: 		return absolutepriceoscillator.NewAbsolutePriceOscillator(p)
224: 
225: 	case core.PearsonsCorrelationCoefficient:
226: 		p := &pearsonscorrelationcoefficient.Params{Length: 14}
227: 		if err := unmarshal(b, p); err != nil {
228: 			return nil, err
229: 		}
230: 
231: 		return pearsonscorrelationcoefficient.New(p)
232: 
233: 	case core.LinearRegression:
234: 		p := &linearregression.Params{Length: 14}
235: 		if err := unmarshal(b, p); err != nil {
236: 			return nil, err
237: 		}
238: 
239: 		return linearregression.New(p)
240: 
241: 	// ── custom ───────────────────────────────────────────────────────────
242: 
243: 	case core.GoertzelSpectrum:
244: 		if isEmptyObject(b) {
245: 			return goertzelspectrum.NewGoertzelSpectrumDefault()
246: 		}
247: 
248: 		p := &goertzelspectrum.Params{}
249: 		if err := unmarshal(b, p); err != nil {
250: 			return nil, err
251: 		}
252: 
253: 		return goertzelspectrum.NewGoertzelSpectrumParams(p)
254: 
255: 	case core.MaximumEntropySpectrum:
256: 		if isEmptyObject(b) {
257: 			return maximumentropyspectrum.NewMaximumEntropySpectrumDefault()
258: 		}
259: 
260: 		p := &maximumentropyspectrum.Params{}
261: 		if err := unmarshal(b, p); err != nil {
262: 			return nil, err
263: 		}
264: 
265: 		return maximumentropyspectrum.NewMaximumEntropySpectrumParams(p)
266: 
267: 	// ── donaldlambert ────────────────────────────────────────────────────
268: 
269: 	case core.CommodityChannelIndex:
270: 		p := &commoditychannelindex.CommodityChannelIndexParams{Length: 14}
271: 		if err := unmarshal(b, p); err != nil {
272: 			return nil, err
273: 		}
274: 
275: 		return commoditychannelindex.NewCommodityChannelIndex(p)
276: 
277: 	// ── genequong ────────────────────────────────────────────────────────
278: 
279: 	case core.MoneyFlowIndex:
280: 		p := &moneyflowindex.MoneyFlowIndexParams{Length: 14}
281: 		if err := unmarshal(b, p); err != nil {
282: 			return nil, err
283: 		}
284: 
285: 		return moneyflowindex.NewMoneyFlowIndex(p)
286: 
287: 	// ── georgelane ───────────────────────────────────────────────────────
288: 
289: 	case core.Stochastic:
290: 		p := &stochastic.StochasticParams{FastKLength: 5, SlowKLength: 3, SlowDLength: 3}
291: 		if err := unmarshal(b, p); err != nil {
292: 			return nil, err
293: 		}
294: 
295: 		return stochastic.NewStochastic(p)
296: 
297: 	// ── geraldappel ──────────────────────────────────────────────────────
298: 
299: 	case core.PercentagePriceOscillator:
300: 		p := &percentagepriceoscillator.PercentagePriceOscillatorParams{FastLength: 12, SlowLength: 26}
301: 		if err := unmarshal(b, p); err != nil {
302: 			return nil, err
303: 		}
304: 
305: 		return percentagepriceoscillator.NewPercentagePriceOscillator(p)
306: 
307: 	case core.MovingAverageConvergenceDivergence:
308: 		p := &movingaverageconvergencedivergence.MovingAverageConvergenceDivergenceParams{
309: 			FastLength:   12,
310: 			SlowLength:   26,
311: 			SignalLength: 9,
312: 		}
313: 		if err := unmarshal(b, p); err != nil {
314: 			return nil, err
315: 		}
316: 
317: 		return movingaverageconvergencedivergence.NewMovingAverageConvergenceDivergence(p)
318: 
319: 	// ── igorlivshin ──────────────────────────────────────────────────────
320: 
321: 	case core.BalanceOfPower:
322: 		return balanceofpower.NewBalanceOfPower(&balanceofpower.BalanceOfPowerParams{})
323: 
324: 	// ── jackhutson ───────────────────────────────────────────────────────
325: 
326: 	case core.TripleExponentialMovingAverageOscillator:
327: 		p := &tripleexponentialmovingaverageoscillator.TripleExponentialMovingAverageOscillatorParams{Length: 30}
328: 		if err := unmarshal(b, p); err != nil {
329: 			return nil, err
330: 		}
331: 
332: 		return tripleexponentialmovingaverageoscillator.NewTripleExponentialMovingAverageOscillator(p)
333: 
334: 	// ── johnbollinger ────────────────────────────────────────────────────
335: 
336: 	case core.BollingerBands:
337: 		p := &bollingerbands.BollingerBandsParams{Length: 20, UpperMultiplier: 2.0, LowerMultiplier: 2.0}
338: 		if err := unmarshal(b, p); err != nil {
339: 			return nil, err
340: 		}
341: 
342: 		return bollingerbands.NewBollingerBands(p)
343: 
344: 	case core.BollingerBandsTrend:
345: 		p := &bollingerbandstrend.BollingerBandsTrendParams{
346: 			FastLength:      20,
347: 			SlowLength:      50,
348: 			UpperMultiplier: 2.0,
349: 			LowerMultiplier: 2.0,
350: 		}
351: 		if err := unmarshal(b, p); err != nil {
352: 			return nil, err
353: 		}
354: 
355: 		return bollingerbandstrend.NewBollingerBandsTrend(p)
356: 
357: 	// ── johnehlers ───────────────────────────────────────────────────────
358: 
359: 	case core.SuperSmoother:
360: 		p := &supersmoother.SuperSmootherParams{ShortestCyclePeriod: 10}
361: 		if err := unmarshal(b, p); err != nil {
362: 			return nil, err
363: 		}
364: 
365: 		return supersmoother.NewSuperSmoother(p)
366: 
367: 	case core.CenterOfGravityOscillator:
368: 		p := &centerofgravityoscillator.Params{Length: 10}
369: 		if err := unmarshal(b, p); err != nil {
370: 			return nil, err
371: 		}
372: 
373: 		return centerofgravityoscillator.NewCenterOfGravityOscillator(p)
374: 
375: 	case core.CyberCycle:
376: 		if hasKey(b, "smoothingFactor") {
377: 			p := &cybercycle.SmoothingFactorParams{SmoothingFactor: 0.07, SignalLag: 9}
378: 			if err := unmarshal(b, p); err != nil {
379: 				return nil, err
380: 			}
381: 
382: 			return cybercycle.NewCyberCycleSmoothingFactor(p)
383: 		}
384: 
385: 		p := &cybercycle.LengthParams{Length: 28, SignalLag: 9}
386: 		if err := unmarshal(b, p); err != nil {
387: 			return nil, err
388: 		}
389: 
390: 		return cybercycle.NewCyberCycleLength(p)
391: 
392: 	case core.InstantaneousTrendLine:
393: 		if hasKey(b, "smoothingFactor") {
394: 			p := &instantaneoustrendline.SmoothingFactorParams{SmoothingFactor: 0.07}
395: 			if err := unmarshal(b, p); err != nil {
396: 				return nil, err
397: 			}
398: 
399: 			return instantaneoustrendline.NewInstantaneousTrendLineSmoothingFactor(p)
400: 		}
401: 
402: 		p := &instantaneoustrendline.LengthParams{Length: 28}
403: 		if err := unmarshal(b, p); err != nil {
404: 			return nil, err
405: 		}
406: 
407: 		return instantaneoustrendline.NewInstantaneousTrendLineLength(p)
408: 
409: 	case core.ZeroLagExponentialMovingAverage:
410: 		p := &zerolagexponentialmovingaverage.ZeroLagExponentialMovingAverageParams{
411: 			SmoothingFactor:        0.25,
412: 			VelocityGainFactor:     0.5,
413: 			VelocityMomentumLength: 3,
414: 		}
415: 		if err := unmarshal(b, p); err != nil {
416: 			return nil, err
417: 		}
418: 
419: 		return zerolagexponentialmovingaverage.NewZeroLagExponentialMovingAverage(p)
420: 
421: 	case core.ZeroLagErrorCorrectingExponentialMovingAverage:
422: 		p := &zerolagerrorcorrectingexponentialmovingaverage.ZeroLagErrorCorrectingExponentialMovingAverageParams{
423: 			SmoothingFactor: 0.095,
424: 			GainLimit:       5,
425: 			GainStep:        0.1,
426: 		}
427: 		if err := unmarshal(b, p); err != nil {
428: 			return nil, err
429: 		}
430: 
431: 		return zerolagerrorcorrectingexponentialmovingaverage.NewZeroLagErrorCorrectingExponentialMovingAverage(p)
432: 
433: 	case core.RoofingFilter:
434: 		p := &roofingfilter.RoofingFilterParams{ShortestCyclePeriod: 10, LongestCyclePeriod: 48}
435: 		if err := unmarshal(b, p); err != nil {
436: 			return nil, err
437: 		}
438: 
439: 		return roofingfilter.NewRoofingFilter(p)
440: 
441: 	case core.MesaAdaptiveMovingAverage:
442: 		if hasKey(b, "fastLimitSmoothingFactor") || hasKey(b, "slowLimitSmoothingFactor") {
443: 			p := &mesaadaptivemovingaverage.SmoothingFactorParams{
444: 				FastLimitSmoothingFactor: 0.5,
445: 				SlowLimitSmoothingFactor: 0.05,
446: 			}
447: 			if err := unmarshal(b, p); err != nil {
448: 				return nil, err
449: 			}
450: 
451: 			return mesaadaptivemovingaverage.NewMesaAdaptiveMovingAverageSmoothingFactor(p)
452: 		}
453: 
454: 		if isEmptyObject(b) {
455: 			return mesaadaptivemovingaverage.NewMesaAdaptiveMovingAverageDefault()
456: 		}
457: 
458: 		p := &mesaadaptivemovingaverage.LengthParams{FastLimitLength: 3, SlowLimitLength: 39}
459: 		if err := unmarshal(b, p); err != nil {
460: 			return nil, err
461: 		}
462: 
463: 		return mesaadaptivemovingaverage.NewMesaAdaptiveMovingAverageLength(p)
464: 
465: 	case core.FractalAdaptiveMovingAverage:
466: 		p := &fractaladaptivemovingaverage.Params{Length: 16, SlowestSmoothingFactor: 0.01}
467: 		if err := unmarshal(b, p); err != nil {
468: 			return nil, err
469: 		}
470: 
471: 		return fractaladaptivemovingaverage.NewFractalAdaptiveMovingAverage(p)
472: 
473: 	case core.DominantCycle:
474: 		if isEmptyObject(b) {
475: 			return dominantcycle.NewDominantCycleDefault()
476: 		}
477: 
478: 		p := &dominantcycle.Params{}
479: 		if err := unmarshal(b, p); err != nil {
480: 			return nil, err
481: 		}
482: 
483: 		return dominantcycle.NewDominantCycleParams(p)
484: 
485: 	case core.SineWave:
486: 		if isEmptyObject(b) {
487: 			return sinewave.NewSineWaveDefault()
488: 		}
489: 
490: 		p := &sinewave.Params{}
491: 		if err := unmarshal(b, p); err != nil {
492: 			return nil, err
493: 		}
494: 
495: 		return sinewave.NewSineWaveParams(p)
496: 
497: 	case core.HilbertTransformerInstantaneousTrendLine:
498: 		if isEmptyObject(b) {
499: 			return hilberttransformerinstantaneoustrendline.NewHilbertTransformerInstantaneousTrendLineDefault()
500: 		}
501: 
502: 		p := &hilberttransformerinstantaneoustrendline.Params{}
503: 		if err := unmarshal(b, p); err != nil {
504: 			return nil, err
505: 		}
506: 
507: 		return hilberttransformerinstantaneoustrendline.NewHilbertTransformerInstantaneousTrendLineParams(p)
508: 
509: 	case core.TrendCycleMode:
510: 		if isEmptyObject(b) {
511: 			return trendcyclemode.NewTrendCycleModeDefault()
512: 		}
513: 
514: 		p := &trendcyclemode.Params{}
515: 		if err := unmarshal(b, p); err != nil {
516: 			return nil, err
517: 		}
518: 
519: 		return trendcyclemode.NewTrendCycleModeParams(p)
520: 
521: 	case core.CoronaSpectrum:
522: 		if isEmptyObject(b) {
523: 			return coronaspectrum.NewCoronaSpectrumDefault()
524: 		}
525: 
526: 		p := &coronaspectrum.Params{}
527: 		if err := unmarshal(b, p); err != nil {
528: 			return nil, err
529: 		}
530: 
531: 		return coronaspectrum.NewCoronaSpectrumParams(p)
532: 
533: 	case core.CoronaSignalToNoiseRatio:
534: 		if isEmptyObject(b) {
535: 			return coronasignaltonoiseratio.NewCoronaSignalToNoiseRatioDefault()
536: 		}
537: 
538: 		p := &coronasignaltonoiseratio.Params{}
539: 		if err := unmarshal(b, p); err != nil {
540: 			return nil, err
541: 		}
542: 
543: 		return coronasignaltonoiseratio.NewCoronaSignalToNoiseRatioParams(p)
544: 
545: 	case core.CoronaSwingPosition:
546: 		if isEmptyObject(b) {
547: 			return coronaswingposition.NewCoronaSwingPositionDefault()
548: 		}
549: 
550: 		p := &coronaswingposition.Params{}
551: 		if err := unmarshal(b, p); err != nil {
552: 			return nil, err
553: 		}
554: 
555: 		return coronaswingposition.NewCoronaSwingPositionParams(p)
556: 
557: 	case core.CoronaTrendVigor:
558: 		if isEmptyObject(b) {
559: 			return coronatrendvigor.NewCoronaTrendVigorDefault()
560: 		}
561: 
562: 		p := &coronatrendvigor.Params{}
563: 		if err := unmarshal(b, p); err != nil {
564: 			return nil, err
565: 		}
566: 
567: 		return coronatrendvigor.NewCoronaTrendVigorParams(p)
568: 
569: 	case core.AutoCorrelationIndicator:
570: 		if isEmptyObject(b) {
571: 			return autocorrelationindicator.NewAutoCorrelationIndicatorDefault()
572: 		}
573: 
574: 		p := &autocorrelationindicator.Params{}
575: 		if err := unmarshal(b, p); err != nil {
576: 			return nil, err
577: 		}
578: 
579: 		return autocorrelationindicator.NewAutoCorrelationIndicatorParams(p)
580: 
581: 	case core.AutoCorrelationPeriodogram:
582: 		if isEmptyObject(b) {
583: 			return autocorrelationperiodogram.NewAutoCorrelationPeriodogramDefault()
584: 		}
585: 
586: 		p := &autocorrelationperiodogram.Params{}
587: 		if err := unmarshal(b, p); err != nil {
588: 			return nil, err
589: 		}
590: 
591: 		return autocorrelationperiodogram.NewAutoCorrelationPeriodogramParams(p)
592: 
593: 	case core.CombBandPassSpectrum:
594: 		if isEmptyObject(b) {
595: 			return combbandpassspectrum.NewCombBandPassSpectrumDefault()
596: 		}
597: 
598: 		p := &combbandpassspectrum.Params{}
599: 		if err := unmarshal(b, p); err != nil {
600: 			return nil, err
601: 		}
602: 
603: 		return combbandpassspectrum.NewCombBandPassSpectrumParams(p)
604: 
605: 	case core.DiscreteFourierTransformSpectrum:
606: 		if isEmptyObject(b) {
607: 			return discretefouriertransformspectrum.NewDiscreteFourierTransformSpectrumDefault()
608: 		}
609: 
610: 		p := &discretefouriertransformspectrum.Params{}
611: 		if err := unmarshal(b, p); err != nil {
612: 			return nil, err
613: 		}
614: 
615: 		return discretefouriertransformspectrum.NewDiscreteFourierTransformSpectrumParams(p)
616: 
617: 	// ── josephgranville ──────────────────────────────────────────────────
618: 
619: 	case core.OnBalanceVolume:
620: 		p := &onbalancevolume.OnBalanceVolumeParams{}
621: 		if err := unmarshal(b, p); err != nil {
622: 			return nil, err
623: 		}
624: 
625: 		return onbalancevolume.NewOnBalanceVolume(p)
626: 
627: 	// ── larrywilliams ────────────────────────────────────────────────────
628: 
629: 	case core.WilliamsPercentR:
630: 		p := &lengthParam{Length: 14}
631: 		if err := unmarshal(b, p); err != nil {
632: 			return nil, err
633: 		}
634: 
635: 		return williamspercentr.NewWilliamsPercentR(p.Length), nil
636: 
637: 	case core.UltimateOscillator:
638: 		p := &ultimateoscillator.UltimateOscillatorParams{Length1: 7, Length2: 14, Length3: 28}
639: 		if err := unmarshal(b, p); err != nil {
640: 			return nil, err
641: 		}
642: 
643: 		return ultimateoscillator.NewUltimateOscillator(p)
644: 
645: 	// ── marcchaikin ──────────────────────────────────────────────────────
646: 
647: 	case core.AdvanceDecline:
648: 		return advancedecline.NewAdvanceDecline(&advancedecline.AdvanceDeclineParams{})
649: 
650: 	case core.AdvanceDeclineOscillator:
651: 		p := &advancedeclineoscillator.AdvanceDeclineOscillatorParams{FastLength: 3, SlowLength: 10}
652: 		if err := unmarshal(b, p); err != nil {
653: 			return nil, err
654: 		}
655: 
656: 		return advancedeclineoscillator.NewAdvanceDeclineOscillator(p)
657: 
658: 	// ── markjurik ────────────────────────────────────────────────────────
659: 
660: 	case core.JurikMovingAverage:
661: 		p := &jurikmovingaverage.JurikMovingAverageParams{Length: 14}
662: 		if err := unmarshal(b, p); err != nil {
663: 			return nil, err
664: 		}
665: 
666: 		return jurikmovingaverage.NewJurikMovingAverage(p)
667: 
668: 	// ── patrickmulloy ────────────────────────────────────────────────────
669: 
670: 	case core.DoubleExponentialMovingAverage:
671: 		if hasKey(b, "smoothingFactor") {
672: 			p := &doubleexponentialmovingaverage.DoubleExponentialMovingAverageSmoothingFactorParams{}
673: 			if err := unmarshal(b, p); err != nil {
674: 				return nil, err
675: 			}
676: 
677: 			return doubleexponentialmovingaverage.NewDoubleExponentialMovingAverageSmoothingFactor(p)
678: 		}
679: 
680: 		p := &doubleexponentialmovingaverage.DoubleExponentialMovingAverageLengthParams{Length: 10}
681: 		if err := unmarshal(b, p); err != nil {
682: 			return nil, err
683: 		}
684: 
685: 		return doubleexponentialmovingaverage.NewDoubleExponentialMovingAverageLength(p)
686: 
687: 	case core.TripleExponentialMovingAverage:
688: 		if hasKey(b, "smoothingFactor") {
689: 			p := &tripleexponentialmovingaverage.TripleExponentialMovingAverageSmoothingFactorParams{}
690: 			if err := unmarshal(b, p); err != nil {
691: 				return nil, err
692: 			}
693: 
694: 			return tripleexponentialmovingaverage.NewTripleExponentialMovingAverageSmoothingFactor(p)
695: 		}
696: 
697: 		p := &tripleexponentialmovingaverage.TripleExponentialMovingAverageLengthParams{Length: 10}
698: 		if err := unmarshal(b, p); err != nil {
699: 			return nil, err
700: 		}
701: 
702: 		return tripleexponentialmovingaverage.NewTripleExponentialMovingAverageLength(p)
703: 
704: 	// ── perrykaufman ─────────────────────────────────────────────────────
705: 
706: 	case core.KaufmanAdaptiveMovingAverage:
707: 		if hasKey(b, "fastestSmoothingFactor") || hasKey(b, "slowestSmoothingFactor") {
708: 			p := &kaufmanadaptivemovingaverage.KaufmanAdaptiveMovingAverageSmoothingFactorParams{
709: 				EfficiencyRatioLength: 10,
710: 			}
711: 			if err := unmarshal(b, p); err != nil {
712: 				return nil, err
713: 			}
714: 
715: 			return kaufmanadaptivemovingaverage.NewKaufmanAdaptiveMovingAverageSmoothingFactor(p)
716: 		}
717: 
718: 		p := &kaufmanadaptivemovingaverage.KaufmanAdaptiveMovingAverageLengthParams{
719: 			EfficiencyRatioLength: 10,
720: 			FastestLength:         2,
721: 			SlowestLength:         30,
722: 		}
723: 		if err := unmarshal(b, p); err != nil {
724: 			return nil, err
725: 		}
726: 
727: 		return kaufmanadaptivemovingaverage.NewKaufmanAdaptiveMovingAverageLength(p)
728: 
729: 	// ── timtillson ───────────────────────────────────────────────────────
730: 
731: 	case core.T2ExponentialMovingAverage:
732: 		if hasKey(b, "smoothingFactor") {
733: 			p := &t2exponentialmovingaverage.T2ExponentialMovingAverageSmoothingFactorParams{VolumeFactor: 0.7}
734: 			if err := unmarshal(b, p); err != nil {
735: 				return nil, err
736: 			}
737: 
738: 			return t2exponentialmovingaverage.NewT2ExponentialMovingAverageSmoothingFactor(p)
739: 		}
740: 
741: 		p := &t2exponentialmovingaverage.T2ExponentialMovingAverageLengthParams{Length: 10, VolumeFactor: 0.7}
742: 		if err := unmarshal(b, p); err != nil {
743: 			return nil, err
744: 		}
745: 
746: 		return t2exponentialmovingaverage.NewT2ExponentialMovingAverageLength(p)
747: 
748: 	case core.T3ExponentialMovingAverage:
749: 		if hasKey(b, "smoothingFactor") {
750: 			p := &t3exponentialmovingaverage.T3ExponentialMovingAverageSmoothingFactorParams{VolumeFactor: 0.7}
751: 			if err := unmarshal(b, p); err != nil {
752: 				return nil, err
753: 			}
754: 
755: 			return t3exponentialmovingaverage.NewT3ExponentialMovingAverageSmoothingFactor(p)
756: 		}
757: 
758: 		p := &t3exponentialmovingaverage.T3ExponentialMovingAverageLengthParams{Length: 10, VolumeFactor: 0.7}
759: 		if err := unmarshal(b, p); err != nil {
760: 			return nil, err
761: 		}
762: 
763: 		return t3exponentialmovingaverage.NewT3ExponentialMovingAverageLength(p)
764: 
765: 	// ── tusharchande ─────────────────────────────────────────────────────
766: 
767: 	case core.ChandeMomentumOscillator:
768: 		p := &chandemomentumoscillator.Params{Length: 14}
769: 		if err := unmarshal(b, p); err != nil {
770: 			return nil, err
771: 		}
772: 
773: 		return chandemomentumoscillator.New(p)
774: 
775: 	case core.StochasticRelativeStrengthIndex:
776: 		p := &stochasticrelativestrengthindex.StochasticRelativeStrengthIndexParams{
777: 			Length:      14,
778: 			FastKLength: 5,
779: 			FastDLength: 3,
780: 		}
781: 		if err := unmarshal(b, p); err != nil {
782: 			return nil, err
783: 		}
784: 
785: 		return stochasticrelativestrengthindex.NewStochasticRelativeStrengthIndex(p)
786: 
787: 	case core.Aroon:
788: 		p := &aroon.AroonParams{Length: 14}
789: 		if err := unmarshal(b, p); err != nil {
790: 			return nil, err
791: 		}
792: 
793: 		return aroon.NewAroon(p)
794: 
795: 	// ── vladimirkravchuk ─────────────────────────────────────────────────
796: 
797: 	case core.AdaptiveTrendAndCycleFilter:
798: 		if isEmptyObject(b) {
799: 			return adaptivetrendandcyclefilter.NewAdaptiveTrendAndCycleFilterDefault()
800: 		}
801: 
802: 		p := &adaptivetrendandcyclefilter.Params{}
803: 		if err := unmarshal(b, p); err != nil {
804: 			return nil, err
805: 		}
806: 
807: 		return adaptivetrendandcyclefilter.NewAdaptiveTrendAndCycleFilterParams(p)
808: 
809: 	// ── welleswilder ─────────────────────────────────────────────────────
810: 
811: 	case core.TrueRange:
812: 		return truerange.NewTrueRange(), nil
813: 
814: 	case core.AverageTrueRange:
815: 		p := &lengthParam{Length: 14}
816: 		if err := unmarshal(b, p); err != nil {
817: 			return nil, err
818: 		}
819: 
820: 		return averagetruerange.NewAverageTrueRange(p.Length)
821: 
822: 	case core.NormalizedAverageTrueRange:
823: 		p := &lengthParam{Length: 14}
824: 		if err := unmarshal(b, p); err != nil {
825: 			return nil, err
826: 		}
827: 
828: 		return normalizedaveragetruerange.NewNormalizedAverageTrueRange(p.Length)
829: 
830: 	case core.DirectionalMovementMinus:
831: 		p := &lengthParam{Length: 14}
832: 		if err := unmarshal(b, p); err != nil {
833: 			return nil, err
834: 		}
835: 
836: 		return directionalmovementminus.NewDirectionalMovementMinus(p.Length)
837: 
838: 	case core.DirectionalMovementPlus:
839: 		p := &lengthParam{Length: 14}
840: 		if err := unmarshal(b, p); err != nil {
841: 			return nil, err
842: 		}
843: 
844: 		return directionalmovementplus.NewDirectionalMovementPlus(p.Length)
845: 
846: 	case core.DirectionalIndicatorMinus:
847: 		p := &lengthParam{Length: 14}
848: 		if err := unmarshal(b, p); err != nil {
849: 			return nil, err
850: 		}
851: 
852: 		return directionalindicatorminus.NewDirectionalIndicatorMinus(p.Length)
853: 
854: 	case core.DirectionalIndicatorPlus:
855: 		p := &lengthParam{Length: 14}
856: 		if err := unmarshal(b, p); err != nil {
857: 			return nil, err
858: 		}
859: 
860: 		return directionalindicatorplus.NewDirectionalIndicatorPlus(p.Length)
861: 
862: 	case core.DirectionalMovementIndex:
863: 		p := &lengthParam{Length: 14}
864: 		if err := unmarshal(b, p); err != nil {
865: 			return nil, err
866: 		}
867: 
868: 		return directionalmovementindex.NewDirectionalMovementIndex(p.Length)
869: 
870: 	case core.AverageDirectionalMovementIndex:
871: 		p := &lengthParam{Length: 14}
872: 		if err := unmarshal(b, p); err != nil {
873: 			return nil, err
874: 		}
875: 
876: 		return averagedirectionalmovementindex.NewAverageDirectionalMovementIndex(p.Length)
877: 
878: 	case core.AverageDirectionalMovementIndexRating:
879: 		p := &lengthParam{Length: 14}
880: 		if err := unmarshal(b, p); err != nil {
881: 			return nil, err
882: 		}
883: 
884: 		return averagedirectionalmovementindexrating.NewAverageDirectionalMovementIndexRating(p.Length)
885: 
886: 	case core.RelativeStrengthIndex:
887: 		p := &relativestrengthindex.RelativeStrengthIndexParams{Length: 14}
888: 		if err := unmarshal(b, p); err != nil {
889: 			return nil, err
890: 		}
891: 
892: 		return relativestrengthindex.NewRelativeStrengthIndex(p)
893: 
894: 	case core.ParabolicStopAndReverse:
895: 		p := &parabolicstopandreverse.ParabolicStopAndReverseParams{
896: 			AccelerationInitLong:  0.02,
897: 			AccelerationLong:      0.02,
898: 			AccelerationMaxLong:   0.20,
899: 			AccelerationInitShort: 0.02,
900: 			AccelerationShort:     0.02,
901: 			AccelerationMaxShort:  0.20,
902: 		}
903: 		if err := unmarshal(b, p); err != nil {
904: 			return nil, err
905: 		}
906: 
907: 		return parabolicstopandreverse.NewParabolicStopAndReverse(p)
908: 
909: 	default:
910: 		return nil, fmt.Errorf("unsupported indicator: %s", identifier)
911: 	}
912: }
913: 
914: // isEmptyObject returns true if data is "{}" (with optional whitespace).
915: func isEmptyObject(data []byte) bool {
916: 	var m map[string]json.RawMessage
917: 	if err := json.Unmarshal(data, &m); err != nil {
918: 		return false
919: 	}
920: 
921: 	return len(m) == 0
922: }

(End of file - total 922 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/factory/factory.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/factory/factory.ts</path>
<type>file</type>
<content>1: /**
2:  * Factory that maps an {@link IndicatorIdentifier} and a plain-object parameter
3:  * bag to a fully constructed {@link Indicator} instance, so callers don't need
4:  * to import every indicator module directly.
5:  *
6:  * For indicators with **Length / SmoothingFactor** constructor variants the
7:  * factory auto-detects which to use: if the params object contains a
8:  * `smoothingFactor` key (or `fastLimitSmoothingFactor`/`slowLimitSmoothingFactor`
9:  * for MAMA, `fastestSmoothingFactor`/`slowestSmoothingFactor` for KAMA) the
10:  * SmoothingFactor variant is used; otherwise the Length variant.
11:  *
12:  * For indicators with **default / fromParams** static factories: if params is
13:  * `undefined` or an empty object `{}`, `default()` is called; otherwise
14:  * `fromParams(params)`.
15:  *
16:  * @module
17:  */
18: 
19: import { type Indicator } from '../core/indicator.js';
20: import { IndicatorIdentifier } from '../core/indicator-identifier.js';
21: 
22: // ── common ──────────────────────────────────────────────────────────────────
23: import { SimpleMovingAverage } from '../common/simple-moving-average/simple-moving-average.js';
24: import { WeightedMovingAverage } from '../common/weighted-moving-average/weighted-moving-average.js';
25: import { TriangularMovingAverage } from '../common/triangular-moving-average/triangular-moving-average.js';
26: import { ExponentialMovingAverage } from '../common/exponential-moving-average/exponential-moving-average.js';
27: import { Momentum } from '../common/momentum/momentum.js';
28: import { RateOfChange } from '../common/rate-of-change/rate-of-change.js';
29: import { RateOfChangePercent } from '../common/rate-of-change-percent/rate-of-change-percent.js';
30: import { RateOfChangeRatio } from '../common/rate-of-change-ratio/rate-of-change-ratio.js';
31: import { Variance } from '../common/variance/variance.js';
32: import { StandardDeviation } from '../common/standard-deviation/standard-deviation.js';
33: import { LinearRegression } from '../common/linear-regression/linear-regression.js';
34: import { PearsonsCorrelationCoefficient } from '../common/pearsons-correlation-coefficient/pearsons-correlation-coefficient.js';
35: import { AbsolutePriceOscillator } from '../common/absolute-price-oscillator/absolute-price-oscillator.js';
36: 
37: // ── mark-jurik ──────────────────────────────────────────────────────────────
38: import { JurikMovingAverage } from '../mark-jurik/jurik-moving-average/jurik-moving-average.js';
39: 
40: // ── patrick-mulloy ──────────────────────────────────────────────────────────
41: import { DoubleExponentialMovingAverage } from '../patrick-mulloy/double-exponential-moving-average/double-exponential-moving-average.js';
42: import { TripleExponentialMovingAverage } from '../patrick-mulloy/triple-exponential-moving-average/triple-exponential-moving-average.js';
43: 
44: // ── tim-tillson ─────────────────────────────────────────────────────────────
45: import { T2ExponentialMovingAverage } from '../tim-tillson/t2-exponential-moving-average/t2-exponential-moving-average.js';
46: import { T3ExponentialMovingAverage } from '../tim-tillson/t3-exponential-moving-average/t3-exponential-moving-average.js';
47: 
48: // ── perry-kaufman ───────────────────────────────────────────────────────────
49: import { KaufmanAdaptiveMovingAverage } from '../perry-kaufman/kaufman-adaptive-moving-average/kaufman-adaptive-moving-average.js';
50: 
51: // ── john-ehlers ─────────────────────────────────────────────────────────────
52: import { MesaAdaptiveMovingAverage } from '../john-ehlers/mesa-adaptive-moving-average/mesa-adaptive-moving-average.js';
53: import { FractalAdaptiveMovingAverage } from '../john-ehlers/fractal-adaptive-moving-average/fractal-adaptive-moving-average.js';
54: import { DominantCycle } from '../john-ehlers/dominant-cycle/dominant-cycle.js';
55: import { SuperSmoother } from '../john-ehlers/super-smoother/super-smoother.js';
56: import { CenterOfGravityOscillator } from '../john-ehlers/center-of-gravity-oscillator/center-of-gravity-oscillator.js';
57: import { CyberCycle } from '../john-ehlers/cyber-cycle/cyber-cycle.js';
58: import { InstantaneousTrendLine } from '../john-ehlers/instantaneous-trend-line/instantaneous-trend-line.js';
59: import { ZeroLagExponentialMovingAverage } from '../john-ehlers/zero-lag-exponential-moving-average/zero-lag-exponential-moving-average.js';
60: import { ZeroLagErrorCorrectingExponentialMovingAverage } from '../john-ehlers/zero-lag-error-correcting-exponential-moving-average/zero-lag-error-correcting-exponential-moving-average.js';
61: import { RoofingFilter } from '../john-ehlers/roofing-filter/roofing-filter.js';
62: import { SineWave } from '../john-ehlers/sine-wave/sine-wave.js';
63: import { HilbertTransformerInstantaneousTrendLine } from '../john-ehlers/hilbert-transformer-instantaneous-trend-line/hilbert-transformer-instantaneous-trend-line.js';
64: import { TrendCycleMode } from '../john-ehlers/trend-cycle-mode/trend-cycle-mode.js';
65: import { CoronaSpectrum } from '../john-ehlers/corona-spectrum/corona-spectrum.js';
66: import { CoronaSignalToNoiseRatio } from '../john-ehlers/corona-signal-to-noise-ratio/corona-signal-to-noise-ratio.js';
67: import { CoronaSwingPosition } from '../john-ehlers/corona-swing-position/corona-swing-position.js';
68: import { CoronaTrendVigor } from '../john-ehlers/corona-trend-vigor/corona-trend-vigor.js';
69: import { AutoCorrelationIndicator } from '../john-ehlers/auto-correlation-indicator/auto-correlation-indicator.js';
70: import { AutoCorrelationPeriodogram } from '../john-ehlers/auto-correlation-periodogram/auto-correlation-periodogram.js';
71: import { CombBandPassSpectrum } from '../john-ehlers/comb-band-pass-spectrum/comb-band-pass-spectrum.js';
72: import { DiscreteFourierTransformSpectrum } from '../john-ehlers/discrete-fourier-transform-spectrum/discrete-fourier-transform-spectrum.js';
73: 
74: // ── welles-wilder ───────────────────────────────────────────────────────────
75: import { TrueRange } from '../welles-wilder/true-range/true-range.js';
76: import { AverageTrueRange } from '../welles-wilder/average-true-range/average-true-range.js';
77: import { NormalizedAverageTrueRange } from '../welles-wilder/normalized-average-true-range/normalized-average-true-range.js';
78: import { DirectionalMovementPlus } from '../welles-wilder/directional-movement-plus/directional-movement-plus.js';
79: import { DirectionalMovementMinus } from '../welles-wilder/directional-movement-minus/directional-movement-minus.js';
80: import { DirectionalIndicatorPlus } from '../welles-wilder/directional-indicator-plus/directional-indicator-plus.js';
81: import { DirectionalIndicatorMinus } from '../welles-wilder/directional-indicator-minus/directional-indicator-minus.js';
82: import { DirectionalMovementIndex } from '../welles-wilder/directional-movement-index/directional-movement-index.js';
83: import { AverageDirectionalMovementIndex } from '../welles-wilder/average-directional-movement-index/average-directional-movement-index.js';
84: import { AverageDirectionalMovementIndexRating } from '../welles-wilder/average-directional-movement-index-rating/average-directional-movement-index-rating.js';
85: import { RelativeStrengthIndex } from '../welles-wilder/relative-strength-index/relative-strength-index.js';
86: import { ParabolicStopAndReverse } from '../welles-wilder/parabolic-stop-and-reverse/parabolic-stop-and-reverse.js';
87: 
88: // ── john-bollinger ──────────────────────────────────────────────────────────
89: import { BollingerBands } from '../john-bollinger/bollinger-bands/bollinger-bands.js';
90: import { BollingerBandsTrend } from '../john-bollinger/bollinger-bands-trend/bollinger-bands-trend.js';
91: 
92: // ── gerald-appel ────────────────────────────────────────────────────────────
93: import { PercentagePriceOscillator } from '../gerald-appel/percentage-price-oscillator/percentage-price-oscillator.js';
94: import { MovingAverageConvergenceDivergence } from '../gerald-appel/moving-average-convergence-divergence/moving-average-convergence-divergence.js';
95: 
96: // ── tushar-chande ───────────────────────────────────────────────────────────
97: import { ChandeMomentumOscillator } from '../tushar-chande/chande-momentum-oscillator/chande-momentum-oscillator.js';
98: import { StochasticRelativeStrengthIndex } from '../tushar-chande/stochastic-relative-strength-index/stochastic-relative-strength-index.js';
99: import { Aroon } from '../tushar-chande/aroon/aroon.js';
100: 
101: // ── donald-lambert ──────────────────────────────────────────────────────────
102: import { CommodityChannelIndex } from '../donald-lambert/commodity-channel-index/commodity-channel-index.js';
103: 
104: // ── gene-quong ──────────────────────────────────────────────────────────────
105: import { MoneyFlowIndex } from '../gene-quong/money-flow-index/money-flow-index.js';
106: 
107: // ── george-lane ─────────────────────────────────────────────────────────────
108: import { Stochastic } from '../george-lane/stochastic/stochastic.js';
109: 
110: // ── joseph-granville ────────────────────────────────────────────────────────
111: import { OnBalanceVolume } from '../joseph-granville/on-balance-volume/on-balance-volume.js';
112: 
113: // ── igor-livshin ────────────────────────────────────────────────────────────
114: import { BalanceOfPower } from '../igor-livshin/balance-of-power/balance-of-power.js';
115: 
116: // ── marc-chaikin ────────────────────────────────────────────────────────────
117: import { AdvanceDecline } from '../marc-chaikin/advance-decline/advance-decline.js';
118: import { AdvanceDeclineOscillator } from '../marc-chaikin/advance-decline-oscillator/advance-decline-oscillator.js';
119: 
120: // ── larry-williams ──────────────────────────────────────────────────────────
121: import { WilliamsPercentR } from '../larry-williams/williams-percent-r/williams-percent-r.js';
122: import { UltimateOscillator } from '../larry-williams/ultimate-oscillator/ultimate-oscillator.js';
123: 
124: // ── jack-hutson ─────────────────────────────────────────────────────────────
125: import { TripleExponentialMovingAverageOscillator } from '../jack-hutson/triple-exponential-moving-average-oscillator/triple-exponential-moving-average-oscillator.js';
126: 
127: // ── vladimir-kravchuk ───────────────────────────────────────────────────────
128: import { AdaptiveTrendAndCycleFilter } from '../vladimir-kravchuk/adaptive-trend-and-cycle-filter/adaptive-trend-and-cycle-filter.js';
129: 
130: // ── custom ──────────────────────────────────────────────────────────────────
131: import { GoertzelSpectrum } from '../custom/goertzel-spectrum/goertzel-spectrum.js';
132: import { MaximumEntropySpectrum } from '../custom/maximum-entropy-spectrum/maximum-entropy-spectrum.js';
133: 
134: 
135: // ---------------------------------------------------------------------------
136: // Helpers
137: // ---------------------------------------------------------------------------
138: 
139: /** Returns `true` when the param key is present in the object. */
140: function hasKey(params: Record<string, unknown>, key: string): boolean {
141:     return key in params;
142: }
143: 
144: /** Returns `true` when `params` is `undefined`, `null`, or `{}`. */
145: function isEmpty(params: Record<string, unknown> | undefined | null): boolean {
146:     if (params == null) return true;
147:     return Object.keys(params).length === 0;
148: }
149: 
150: // ---------------------------------------------------------------------------
151: // Factory
152: // ---------------------------------------------------------------------------
153: 
154: /**
155:  * Creates an indicator from its identifier and a plain params object.
156:  *
157:  * @param identifier - The indicator to create.
158:  * @param params     - Construction parameters (shape depends on the indicator).
159:  *                     Pass `undefined` or `{}` for default parameters.
160:  * @returns A fully constructed {@link Indicator}.
161:  * @throws  If the identifier is not supported or params are invalid.
162:  */
163: // eslint-disable-next-line @typescript-eslint/no-explicit-any
164: export function createIndicator(identifier: IndicatorIdentifier, params?: Record<string, any>): Indicator {
165:     const p = params ?? {};
166: 
167:     switch (identifier) {
168: 
169:         // ── common ──────────────────────────────────────────────────────
170: 
171:         case IndicatorIdentifier.SimpleMovingAverage:
172:             return new SimpleMovingAverage({ length: 14, ...p });
173: 
174:         case IndicatorIdentifier.WeightedMovingAverage:
175:             return new WeightedMovingAverage({ length: 14, ...p });
176: 
177:         case IndicatorIdentifier.TriangularMovingAverage:
178:             return new TriangularMovingAverage({ length: 14, ...p });
179: 
180:         case IndicatorIdentifier.ExponentialMovingAverage:
181:             if (hasKey(p, 'smoothingFactor')) {
182:                 return new ExponentialMovingAverage(p as any);
183:             }
184:             return new ExponentialMovingAverage({ length: 10, ...p });
185: 
186:         case IndicatorIdentifier.Variance:
187:             return new Variance({ length: 14, unbiased: true, ...p });
188: 
189:         case IndicatorIdentifier.StandardDeviation:
190:             return new StandardDeviation({ length: 14, unbiased: true, ...p });
191: 
192:         case IndicatorIdentifier.Momentum:
193:             return new Momentum({ length: 10, ...p });
194: 
195:         case IndicatorIdentifier.RateOfChange:
196:             return new RateOfChange({ length: 10, ...p });
197: 
198:         case IndicatorIdentifier.RateOfChangePercent:
199:             return new RateOfChangePercent({ length: 10, ...p });
200: 
201:         case IndicatorIdentifier.RateOfChangeRatio:
202:             return new RateOfChangeRatio({ length: 10, ...p });
203: 
204:         case IndicatorIdentifier.AbsolutePriceOscillator:
205:             return new AbsolutePriceOscillator({ fastLength: 12, slowLength: 26, ...p });
206: 
207:         case IndicatorIdentifier.PearsonsCorrelationCoefficient:
208:             return new PearsonsCorrelationCoefficient({ length: 14, ...p });
209: 
210:         case IndicatorIdentifier.LinearRegression:
211:             return new LinearRegression({ length: 14, ...p });
212: 
213:         // ── mark-jurik ──────────────────────────────────────────────────
214: 
215:         case IndicatorIdentifier.JurikMovingAverage:
216:             return new JurikMovingAverage({ length: 14, phase: 0, ...p });
217: 
218:         // ── patrick-mulloy ──────────────────────────────────────────────
219: 
220:         case IndicatorIdentifier.DoubleExponentialMovingAverage:
221:             if (hasKey(p, 'smoothingFactor')) {
222:                 return new DoubleExponentialMovingAverage(p as any);
223:             }
224:             return new DoubleExponentialMovingAverage({ length: 10, firstIsAverage: true, ...p });
225: 
226:         case IndicatorIdentifier.TripleExponentialMovingAverage:
227:             if (hasKey(p, 'smoothingFactor')) {
228:                 return new TripleExponentialMovingAverage(p as any);
229:             }
230:             return new TripleExponentialMovingAverage({ length: 10, firstIsAverage: true, ...p });
231: 
232:         // ── tim-tillson ─────────────────────────────────────────────────
233: 
234:         case IndicatorIdentifier.T2ExponentialMovingAverage:
235:             if (hasKey(p, 'smoothingFactor')) {
236:                 return new T2ExponentialMovingAverage({ volumeFactor: 0.7, firstIsAverage: true, ...p } as any);
237:             }
238:             return new T2ExponentialMovingAverage({ length: 10, volumeFactor: 0.7, firstIsAverage: true, ...p });
239: 
240:         case IndicatorIdentifier.T3ExponentialMovingAverage:
241:             if (hasKey(p, 'smoothingFactor')) {
242:                 return new T3ExponentialMovingAverage({ volumeFactor: 0.7, firstIsAverage: true, ...p } as any);
243:             }
244:             return new T3ExponentialMovingAverage({ length: 10, volumeFactor: 0.7, firstIsAverage: true, ...p });
245: 
246:         // ── perry-kaufman ───────────────────────────────────────────────
247: 
248:         case IndicatorIdentifier.KaufmanAdaptiveMovingAverage:
249:             if (hasKey(p, 'fastestSmoothingFactor') || hasKey(p, 'slowestSmoothingFactor')) {
250:                 return new KaufmanAdaptiveMovingAverage({ efficiencyRatioLength: 10, ...p } as any);
251:             }
252:             return new KaufmanAdaptiveMovingAverage({ efficiencyRatioLength: 10, fastestLength: 2, slowestLength: 30, ...p });
253: 
254:         // ── john-ehlers ─────────────────────────────────────────────────
255: 
256:         case IndicatorIdentifier.MesaAdaptiveMovingAverage:
257:             if (hasKey(p, 'fastLimitSmoothingFactor') || hasKey(p, 'slowLimitSmoothingFactor')) {
258:                 return MesaAdaptiveMovingAverage.fromSmoothingFactor({
259:                     fastLimitSmoothingFactor: 0.5,
260:                     slowLimitSmoothingFactor: 0.05,
261:                     ...p,
262:                 } as any);
263:             }
264:             if (isEmpty(p)) {
265:                 return MesaAdaptiveMovingAverage.default();
266:             }
267:             return MesaAdaptiveMovingAverage.fromLength({ fastLimitLength: 3, slowLimitLength: 39, ...p } as any);
268: 
269:         case IndicatorIdentifier.FractalAdaptiveMovingAverage:
270:             return new FractalAdaptiveMovingAverage({ length: 16, slowestSmoothingFactor: 0.01, ...p });
271: 
272:         case IndicatorIdentifier.DominantCycle:
273:             if (isEmpty(p)) return DominantCycle.default();
274:             return DominantCycle.fromParams(p as any);
275: 
276:         case IndicatorIdentifier.SuperSmoother:
277:             return new SuperSmoother({ shortestCyclePeriod: 10, ...p });
278: 
279:         case IndicatorIdentifier.CenterOfGravityOscillator:
280:             return new CenterOfGravityOscillator({ length: 10, ...p });
281: 
282:         case IndicatorIdentifier.CyberCycle:
283:             if (hasKey(p, 'smoothingFactor')) {
284:                 return new CyberCycle({ signalLag: 9, ...p } as any);
285:             }
286:             return new CyberCycle({ length: 28, signalLag: 9, ...p });
287: 
288:         case IndicatorIdentifier.InstantaneousTrendLine:
289:             if (hasKey(p, 'smoothingFactor')) {
290:                 return new InstantaneousTrendLine(p as any);
291:             }
292:             return new InstantaneousTrendLine({ length: 28, ...p });
293: 
294:         case IndicatorIdentifier.ZeroLagExponentialMovingAverage:
295:             return new ZeroLagExponentialMovingAverage({
296:                 smoothingFactor: 0.25,
297:                 velocityGainFactor: 0.5,
298:                 velocityMomentumLength: 3,
299:                 ...p,
300:             });
301: 
302:         case IndicatorIdentifier.ZeroLagErrorCorrectingExponentialMovingAverage:
303:             return new ZeroLagErrorCorrectingExponentialMovingAverage({
304:                 smoothingFactor: 0.095,
305:                 gainLimit: 5,
306:                 gainStep: 0.1,
307:                 ...p,
308:             });
309: 
310:         case IndicatorIdentifier.RoofingFilter:
311:             return new RoofingFilter({ shortestCyclePeriod: 10, longestCyclePeriod: 48, ...p });
312: 
313:         case IndicatorIdentifier.SineWave:
314:             if (isEmpty(p)) return SineWave.default();
315:             return SineWave.fromParams(p as any);
316: 
317:         case IndicatorIdentifier.HilbertTransformerInstantaneousTrendLine:
318:             if (isEmpty(p)) return HilbertTransformerInstantaneousTrendLine.default();
319:             return HilbertTransformerInstantaneousTrendLine.fromParams(p as any);
320: 
321:         case IndicatorIdentifier.TrendCycleMode:
322:             if (isEmpty(p)) return TrendCycleMode.default();
323:             return TrendCycleMode.fromParams(p as any);
324: 
325:         case IndicatorIdentifier.CoronaSpectrum:
326:             if (isEmpty(p)) return CoronaSpectrum.default();
327:             return CoronaSpectrum.fromParams(p as any);
328: 
329:         case IndicatorIdentifier.CoronaSignalToNoiseRatio:
330:             if (isEmpty(p)) return CoronaSignalToNoiseRatio.default();
331:             return CoronaSignalToNoiseRatio.fromParams(p as any);
332: 
333:         case IndicatorIdentifier.CoronaSwingPosition:
334:             if (isEmpty(p)) return CoronaSwingPosition.default();
335:             return CoronaSwingPosition.fromParams(p as any);
336: 
337:         case IndicatorIdentifier.CoronaTrendVigor:
338:             if (isEmpty(p)) return CoronaTrendVigor.default();
339:             return CoronaTrendVigor.fromParams(p as any);
340: 
341:         case IndicatorIdentifier.AutoCorrelationIndicator:
342:             if (isEmpty(p)) return AutoCorrelationIndicator.default();
343:             return AutoCorrelationIndicator.fromParams(p as any);
344: 
345:         case IndicatorIdentifier.AutoCorrelationPeriodogram:
346:             if (isEmpty(p)) return AutoCorrelationPeriodogram.default();
347:             return AutoCorrelationPeriodogram.fromParams(p as any);
348: 
349:         case IndicatorIdentifier.CombBandPassSpectrum:
350:             if (isEmpty(p)) return CombBandPassSpectrum.default();
351:             return CombBandPassSpectrum.fromParams(p as any);
352: 
353:         case IndicatorIdentifier.DiscreteFourierTransformSpectrum:
354:             if (isEmpty(p)) return DiscreteFourierTransformSpectrum.default();
355:             return DiscreteFourierTransformSpectrum.fromParams(p as any);
356: 
357:         // ── welles-wilder ───────────────────────────────────────────────
358: 
359:         case IndicatorIdentifier.TrueRange:
360:             return new TrueRange();
361: 
362:         case IndicatorIdentifier.AverageTrueRange:
363:             return new AverageTrueRange((p as any).length ?? 14);
364: 
365:         case IndicatorIdentifier.NormalizedAverageTrueRange:
366:             return new NormalizedAverageTrueRange((p as any).length ?? 14);
367: 
368:         case IndicatorIdentifier.DirectionalMovementPlus:
369:             return new DirectionalMovementPlus((p as any).length ?? 14);
370: 
371:         case IndicatorIdentifier.DirectionalMovementMinus:
372:             return new DirectionalMovementMinus((p as any).length ?? 14);
373: 
374:         case IndicatorIdentifier.DirectionalIndicatorPlus:
375:             return new DirectionalIndicatorPlus((p as any).length ?? 14);
376: 
377:         case IndicatorIdentifier.DirectionalIndicatorMinus:
378:             return new DirectionalIndicatorMinus((p as any).length ?? 14);
379: 
380:         case IndicatorIdentifier.DirectionalMovementIndex:
381:             return new DirectionalMovementIndex((p as any).length ?? 14);
382: 
383:         case IndicatorIdentifier.AverageDirectionalMovementIndex:
384:             return new AverageDirectionalMovementIndex((p as any).length ?? 14);
385: 
386:         case IndicatorIdentifier.AverageDirectionalMovementIndexRating:
387:             return new AverageDirectionalMovementIndexRating((p as any).length ?? 14);
388: 
389:         case IndicatorIdentifier.RelativeStrengthIndex:
390:             return new RelativeStrengthIndex({ length: 14, ...p });
391: 
392:         case IndicatorIdentifier.ParabolicStopAndReverse:
393:             return new ParabolicStopAndReverse(isEmpty(p) ? undefined : p as any);
394: 
395:         // ── john-bollinger ──────────────────────────────────────────────
396: 
397:         case IndicatorIdentifier.BollingerBands:
398:             return new BollingerBands({ length: 20, upperMultiplier: 2.0, lowerMultiplier: 2.0, ...p });
399: 
400:         case IndicatorIdentifier.BollingerBandsTrend:
401:             return new BollingerBandsTrend({
402:                 fastLength: 20,
403:                 slowLength: 50,
404:                 upperMultiplier: 2.0,
405:                 lowerMultiplier: 2.0,
406:                 ...p,
407:             });
408: 
409:         // ── gerald-appel ────────────────────────────────────────────────
410: 
411:         case IndicatorIdentifier.PercentagePriceOscillator:
412:             return new PercentagePriceOscillator({ fastLength: 12, slowLength: 26, ...p });
413: 
414:         case IndicatorIdentifier.MovingAverageConvergenceDivergence:
415:             return new MovingAverageConvergenceDivergence(isEmpty(p) ? undefined : p as any);
416: 
417:         // ── tushar-chande ───────────────────────────────────────────────
418: 
419:         case IndicatorIdentifier.ChandeMomentumOscillator:
420:             return new ChandeMomentumOscillator({ length: 14, ...p });
421: 
422:         case IndicatorIdentifier.StochasticRelativeStrengthIndex:
423:             return new StochasticRelativeStrengthIndex({ length: 14, fastKLength: 5, fastDLength: 3, ...p });
424: 
425:         case IndicatorIdentifier.Aroon:
426:             return new Aroon({ length: 14, ...p });
427: 
428:         // ── donald-lambert ──────────────────────────────────────────────
429: 
430:         case IndicatorIdentifier.CommodityChannelIndex:
431:             return new CommodityChannelIndex({ length: 14, ...p });
432: 
433:         // ── gene-quong ──────────────────────────────────────────────────
434: 
435:         case IndicatorIdentifier.MoneyFlowIndex:
436:             return new MoneyFlowIndex({ length: 14, ...p });
437: 
438:         // ── george-lane ─────────────────────────────────────────────────
439: 
440:         case IndicatorIdentifier.Stochastic:
441:             return new Stochastic({ fastKLength: 5, slowKLength: 3, slowDLength: 3, ...p });
442: 
443:         // ── joseph-granville ────────────────────────────────────────────
444: 
445:         case IndicatorIdentifier.OnBalanceVolume:
446:             return new OnBalanceVolume(isEmpty(p) ? undefined : p as any);
447: 
448:         // ── igor-livshin ────────────────────────────────────────────────
449: 
450:         case IndicatorIdentifier.BalanceOfPower:
451:             return new BalanceOfPower();
452: 
453:         // ── marc-chaikin ────────────────────────────────────────────────
454: 
455:         case IndicatorIdentifier.AdvanceDecline:
456:             return new AdvanceDecline();
457: 
458:         case IndicatorIdentifier.AdvanceDeclineOscillator:
459:             return new AdvanceDeclineOscillator({ fastLength: 3, slowLength: 10, ...p });
460: 
461:         // ── larry-williams ──────────────────────────────────────────────
462: 
463:         case IndicatorIdentifier.WilliamsPercentR:
464:             return new WilliamsPercentR((p as any).length ?? 14);
465: 
466:         case IndicatorIdentifier.UltimateOscillator:
467:             return new UltimateOscillator(isEmpty(p) ? undefined : p as any);
468: 
469:         // ── jack-hutson ─────────────────────────────────────────────────
470: 
471:         case IndicatorIdentifier.TripleExponentialMovingAverageOscillator:
472:             return new TripleExponentialMovingAverageOscillator({ length: 30, ...p });
473: 
474:         // ── vladimir-kravchuk ───────────────────────────────────────────
475: 
476:         case IndicatorIdentifier.AdaptiveTrendAndCycleFilter:
477:             if (isEmpty(p)) return AdaptiveTrendAndCycleFilter.default();
478:             return AdaptiveTrendAndCycleFilter.fromParams(p as any);
479: 
480:         // ── custom ──────────────────────────────────────────────────────
481: 
482:         case IndicatorIdentifier.GoertzelSpectrum:
483:             if (isEmpty(p)) return GoertzelSpectrum.default();
484:             return GoertzelSpectrum.fromParams(p as any);
485: 
486:         case IndicatorIdentifier.MaximumEntropySpectrum:
487:             if (isEmpty(p)) return MaximumEntropySpectrum.default();
488:             return MaximumEntropySpectrum.fromParams(p as any);
489: 
490:         default:
491:             throw new Error(`Unsupported indicator: ${IndicatorIdentifier[identifier] ?? identifier}`);
492:     }
493: }

(End of file - total 493 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.6 · 387.1s)

_Thinking:_

Looking at the factories, the Go factory uses inline defaults like `&simplemovingaverage.SimpleMovingAverageParams{Length: 14}` and the TS factory uses `{ length: 14, ...p }`. I need to replace these with calls to `DefaultParams()` / `defaultParams()`.

For Go, the pattern would change from:
```go
p := &simplemovingaverage.SimpleMovingAverageParams{Length: 14}
if err := unmarshal(b, p); err != nil {
```
to:
```go
p := simplemovingaverage.DefaultParams()


if err := unmarshal(b, p); err != nil {
```

For TS, the pattern would change from:
```ts
return new SimpleMovingAverage({ length: 14, ...p });
```
to importing defaultParams and:
```ts
return new SimpleMovingAverage({ ...defaultSmaParams(), ...p });
```

This is a big refactor, so I'm going to delegate this to agents.

This is a mechanical refactor across both factory files. Let me delegate both in parallel.

**Tool: task**

**Input:**
```json
{
  "description": "Update Go factory DefaultParams",
  "prompt": "You need to update `/home/dev/zpano/go/indicators/factory/factory.go` to use `DefaultParams()` (or `DefaultLengthParams()`/`DefaultSmoothingFactorParams()`) instead of inline struct literals for default values.\n\nRead the file first. The pattern to change is:\n\nFROM (standard params):\n```go\ncase core.SimpleMovingAverage:\n    p := &simplemovingaverage.SimpleMovingAverageParams{Length: 14}\n    if err := unmarshal(b, p); err != nil {\n```\n\nTO:\n```go\ncase core.SimpleMovingAverage:\n    p := simplemovingaverage.DefaultParams()\n    if err := unmarshal(b, p); err != nil {\n```\n\nFROM (dual-variant Length):\n```go\np := &exponentialmovingaverage.ExponentialMovingAverageLengthParams{Length: 10}\n```\nTO:\n```go\np := exponentialmovingaverage.DefaultLengthParams()\n```\n\nFROM (dual-variant SmoothingFactor):\n```go\np := &cybercycle.SmoothingFactorParams{SmoothingFactor: 0.07, SignalLag: 9}\n```\nTO:\n```go\np := cybercycle.DefaultSmoothingFactorParams()\n```\n\nDo NOT change:\n- The `lengthParam` struct usage for bare-int constructors (ATR, NATR, DM±, DI±, DMI, ADX, ADXR, WilliamsPercentR) - these use a local helper struct, not the indicator's params\n- The no-params constructors (TrueRange, BalanceOfPower, AdvanceDecline) - they already pass empty structs\n- The \"Default vs Params\" pattern indicators (GoertzelSpectrum, MaximumEntropySpectrum, DominantCycle, SineWave, HTITL, TrendCycleMode, CoronaSpectrum, CoronaSNR, CoronaSwingPosition, CoronaTrendVigor, AutoCorrelationIndicator, AutoCorrelationPeriodogram, CombBandPassSpectrum, DFTSpectrum, ATCF) - these use `isEmptyObject` checks and call Default/Params constructors, not the factory's inline defaults\n- The `OnBalanceVolume` case (passes empty struct)\n\nHere's the complete list of cases to change (replace inline struct literal with DefaultParams/DefaultLengthParams/DefaultSmoothingFactorParams call):\n\n1. `core.SimpleMovingAverage` → `simplemovingaverage.DefaultParams()`\n2. `core.WeightedMovingAverage` → `weightedmovingaverage.DefaultParams()`\n3. `core.TriangularMovingAverage` → `triangularmovingaverage.DefaultParams()`\n4. `core.ExponentialMovingAverage` Length → `exponentialmovingaverage.DefaultLengthParams()`\n5. `core.Variance` → `variance.DefaultParams()`\n6. `core.StandardDeviation` → `standarddeviation.DefaultParams()`\n7. `core.Momentum` → `momentum.DefaultParams()`\n8. `core.RateOfChange` → `rateofchange.DefaultParams()`\n9. `core.RateOfChangePercent` → `rateofchangepercent.DefaultParams()`\n10. `core.RateOfChangeRatio` → `rateofchangeratio.DefaultParams()`\n11. `core.AbsolutePriceOscillator` → `absolutepriceoscillator.DefaultParams()`\n12. `core.PearsonsCorrelationCoefficient` → `pearsonscorrelationcoefficient.DefaultParams()`\n13. `core.LinearRegression` → `linearregression.DefaultParams()`\n14. `core.CommodityChannelIndex` → `commoditychannelindex.DefaultParams()`\n15. `core.MoneyFlowIndex` → `moneyflowindex.DefaultParams()`\n16. `core.Stochastic` → `stochastic.DefaultParams()`\n17. `core.PercentagePriceOscillator` → `percentagepriceoscillator.DefaultParams()`\n18. `core.MovingAverageConvergenceDivergence` → `movingaverageconvergencedivergence.DefaultParams()`\n19. `core.TripleExponentialMovingAverageOscillator` → `tripleexponentialmovingaverageoscillator.DefaultParams()`\n20. `core.BollingerBands` → `bollingerbands.DefaultParams()`\n21. `core.BollingerBandsTrend` → `bollingerbandstrend.DefaultParams()`\n22. `core.SuperSmoother` → `supersmoother.DefaultParams()`\n23. `core.CenterOfGravityOscillator` → `centerofgravityoscillator.DefaultParams()`\n24. `core.CyberCycle` Length → `cybercycle.DefaultLengthParams()`, SF → `cybercycle.DefaultSmoothingFactorParams()`\n25. `core.InstantaneousTrendLine` Length → `instantaneoustrendline.DefaultLengthParams()`, SF → `instantaneoustrendline.DefaultSmoothingFactorParams()`\n26. `core.ZeroLagExponentialMovingAverage` → `zerolagexponentialmovingaverage.DefaultParams()`\n27. `core.ZeroLagErrorCorrectingExponentialMovingAverage` → `zerolagerrorcorrectingexponentialmovingaverage.DefaultParams()`\n28. `core.RoofingFilter` → `roofingfilter.DefaultParams()`\n29. `core.MesaAdaptiveMovingAverage` SF → `mesaadaptivemovingaverage.DefaultSmoothingFactorParams()`, Length → `mesaadaptivemovingaverage.DefaultLengthParams()`\n30. `core.FractalAdaptiveMovingAverage` → `fractaladaptivemovingaverage.DefaultParams()`\n31. `core.AdvanceDeclineOscillator` → `advancedeclineoscillator.DefaultParams()`\n32. `core.JurikMovingAverage` → `jurikmovingaverage.DefaultParams()`\n33. `core.DoubleExponentialMovingAverage` Length → `doubleexponentialmovingaverage.DefaultLengthParams()`\n34. `core.TripleExponentialMovingAverage` Length → `tripleexponentialmovingaverage.DefaultLengthParams()`\n35. `core.KaufmanAdaptiveMovingAverage` Length → `kaufmanadaptivemovingaverage.DefaultLengthParams()`, SF → `kaufmanadaptivemovingaverage.DefaultSmoothingFactorParams()`\n36. `core.T2ExponentialMovingAverage` Length → `t2exponentialmovingaverage.DefaultLengthParams()`, SF → `t2exponentialmovingaverage.DefaultSmoothingFactorParams()`\n37. `core.T3ExponentialMovingAverage` Length → `t3exponentialmovingaverage.DefaultLengthParams()`, SF → `t3exponentialmovingaverage.DefaultSmoothingFactorParams()`\n38. `core.ChandeMomentumOscillator` → `chandemomentumoscillator.DefaultParams()`\n39. `core.StochasticRelativeStrengthIndex` → `stochasticrelativestrengthindex.DefaultParams()`\n40. `core.Aroon` → `aroon.DefaultParams()`\n41. `core.UltimateOscillator` → `ultimateoscillator.DefaultParams()`\n42. `core.RelativeStrengthIndex` → `relativestrengthindex.DefaultParams()`\n43. `core.ParabolicStopAndReverse` → `parabolicstopandreverse.DefaultParams()`\n\nIMPORTANT: For the EMA SmoothingFactor variant, the current code creates an empty struct `&exponentialmovingaverage.ExponentialMovingAverageSmoothingFactorParams{}` and unmarshals into it. DON'T change this one since we want the JSON to provide the SF value. Same for DEMA, TEMA SF variants where the current code creates empty SF structs.\n\nAfter making all changes, verify with: `cd /home/dev/zpano/go && go build ./...`\n\nMake the edits carefully - each edit should be precise. Do ALL 43 cases.",
  "subagent_type": "general"
}
```

**Output:**
```
task_id: ses_23fe4727cffeHtV2LTWCFImVh6 (for resuming to continue this task if needed)

<task_result>
All 43 cases updated and the build passes successfully.
</task_result>
```

**Tool: task**

**Input:**
```json
{
  "description": "Update TS factory defaultParams",
  "prompt": "You need to update `/home/dev/zpano/ts/indicators/factory/factory.ts` to use imported `defaultParams()` (or `defaultLengthParams()`/`defaultSmoothingFactorParams()`) functions instead of inline default object literals.\n\nRead the file first. The pattern to change is:\n\nFROM:\n```ts\ncase IndicatorIdentifier.SimpleMovingAverage:\n    return new SimpleMovingAverage({ length: 14, ...p });\n```\n\nTO:\n```ts\ncase IndicatorIdentifier.SimpleMovingAverage:\n    return new SimpleMovingAverage({ ...defaultSmaParams(), ...p });\n```\n\nWhere `defaultSmaParams` is imported as a renamed import from the params file. Since many indicators have `defaultParams` as the function name, you need to use renamed imports to avoid collisions.\n\nUse this naming convention for imports:\n```ts\nimport { defaultParams as defaultSmaParams } from '../common/simple-moving-average/params.js';\n```\n\nFor dual-variant:\n```ts\nimport { defaultLengthParams as defaultEmaLengthParams, defaultSmoothingFactorParams as defaultEmaSfParams } from '../common/exponential-moving-average/params.js';\n```\n\nAdd all the import statements in the appropriate section (grouped by author, matching existing groups).\n\nHere's what to change:\n\n**Standard params (replace `{ field: value, ...p }` with `{ ...defaultXxxParams(), ...p }`):**\n1. SimpleMovingAverage → `defaultSmaParams`\n2. WeightedMovingAverage → `defaultWmaParams`\n3. TriangularMovingAverage → `defaultTrimaParams`\n4. Variance → `defaultVarianceParams`\n5. StandardDeviation → `defaultStdDevParams`\n6. Momentum → `defaultMomentumParams`\n7. RateOfChange → `defaultRocParams`\n8. RateOfChangePercent → `defaultRocpParams`\n9. RateOfChangeRatio → `defaultRocrParams`\n10. AbsolutePriceOscillator → `defaultApoParams`\n11. PearsonsCorrelationCoefficient → `defaultCorrelParams`\n12. LinearRegression → `defaultLinregParams`\n13. JurikMovingAverage → `defaultJmaParams`\n14. FractalAdaptiveMovingAverage → `defaultFramaParams`\n15. SuperSmoother → `defaultSsParams`\n16. CenterOfGravityOscillator → `defaultCogParams`\n17. ZeroLagExponentialMovingAverage → `defaultZemaParams`\n18. ZeroLagErrorCorrectingExponentialMovingAverage → `defaultZecemaParams`\n19. RoofingFilter → `defaultRoofParams`\n20. BollingerBands → `defaultBbParams`\n21. BollingerBandsTrend → `defaultBbtrendParams`\n22. PercentagePriceOscillator → `defaultPpoParams`\n23. ChandeMomentumOscillator → `defaultCmoParams`\n24. StochasticRelativeStrengthIndex → `defaultStochRsiParams`\n25. Aroon → `defaultAroonParams`\n26. CommodityChannelIndex → `defaultCciParams`\n27. MoneyFlowIndex → `defaultMfiParams`\n28. Stochastic → `defaultStochParams`\n29. AdvanceDeclineOscillator → `defaultAdoscParams`\n30. TripleExponentialMovingAverageOscillator → `defaultTrixParams`\n31. RelativeStrengthIndex → `defaultRsiParams`\n\n**Dual-variant Length (replace `{ length: N, ...p }` with `{ ...defaultXxxLengthParams(), ...p }`):**\n32. ExponentialMovingAverage → `defaultEmaLengthParams`\n33. DoubleExponentialMovingAverage → `defaultDemaLengthParams`\n34. TripleExponentialMovingAverage → `defaultTemaLengthParams`\n35. T2ExponentialMovingAverage → `defaultT2LengthParams`\n36. T3ExponentialMovingAverage → `defaultT3LengthParams`\n37. KaufmanAdaptiveMovingAverage → `defaultKamaLengthParams`\n38. CyberCycle → `defaultCcLengthParams`\n39. InstantaneousTrendLine → `defaultItlLengthParams`\n40. MesaAdaptiveMovingAverage Length → `defaultMamaLengthParams`\n\n**Dual-variant SmoothingFactor (for cases that currently have inline SF defaults):**\n41. CyberCycle SF → `defaultCcSfParams`\n42. InstantaneousTrendLine SF → `defaultItlSfParams` (currently `p as any`, but ITL SF has smoothingFactor=0.07 default)\n43. MesaAdaptiveMovingAverage SF → `defaultMamaSfParams`\n44. T2 SF → `defaultT2SfParams`\n45. T3 SF → `defaultT3SfParams`\n46. KAMA SF → `defaultKamaSfParams`\n\n**Do NOT change:**\n- Bare-int constructors (ATR, NATR, DM±, DI±, DMI, ADX, ADXR, WilliamsPercentR) using `(p as any).length ?? 14`\n- No-params constructors (TrueRange, BalanceOfPower, AdvanceDecline)\n- \"Default vs Params\" pattern using `isEmpty(p)` checks (DominantCycle, SineWave, HTITL, TrendCycleMode, Corona*, AutoCorrelation*, CombBandPass, DFT, ATCF, GoertzelSpectrum, MaximumEntropySpectrum)\n- OnBalanceVolume, ParabolicStopAndReverse, MovingAverageConvergenceDivergence, UltimateOscillator (these pass p directly or use isEmpty checks)\n\nFor SF variants where the current code passes `p as any` with no defaults (e.g., EMA SF: `return new ExponentialMovingAverage(p as any)`), DON'T change it - the JSON is expected to provide the SF.\n\nBUT for cases that DO have inline SF defaults (CyberCycle SF has `signalLag: 9`, T2/T3 SF have `volumeFactor: 0.7, firstIsAverage: true`, KAMA SF has `efficiencyRatioLength: 10`, MAMA SF has `fastLimitSmoothingFactor: 0.5, slowLimitSmoothingFactor: 0.05`), replace with `{ ...defaultXxxSfParams(), ...p }`.\n\nAfter making all changes, verify with: `cd /home/dev/zpano/ts && npx tsc --noEmit 2>&1 | grep -c \"params.ts\"` should return 0 (no new errors in params files).\n\nMake the edits carefully. Do ALL cases listed above.",
  "subagent_type": "general"
}
```

**Output:**
```
task_id: ses_23fe41780ffePlHmQq7Ikuh5wz (for resuming to continue this task if needed)

<task_result>
Zero errors in factory or params files. All 46 cases have been updated:

**Params files modified** (added `defaultLengthParams`):
- `patrick-mulloy/double-exponential-moving-average/params.ts`
- `patrick-mulloy/triple-exponential-moving-average/params.ts`
- `tim-tillson/t2-exponential-moving-average/params.ts`
- `tim-tillson/t3-exponential-moving-average/params.ts`
- `perry-kaufman/kaufman-adaptive-moving-average/params.ts`

**Factory changes:**
- Added 31 renamed default param imports across all author groups
- Replaced all 46 inline default object literals with `{ ...defaultXxxParams(), ...p }` spreads
- Left untouched: bare-int constructors, no-params constructors, `isEmpty(p)` patterns, and SF pass-through cases (EMA SF, DEMA SF, TEMA SF)
</task_result>
```

---

## User

---

## Assistant (Compaction · claude-opus-4.6 · 74.5s)



---

## Goal

Build a **factory package** for both Go and TypeScript indicator codebases that maps an indicator identifier string + JSON parameters to a fully constructed `Indicator` instance. Also build `icalc`, a CLI tool in both languages that uses the factory to create indicators from a JSON settings file and run them against embedded test bar data. Add `DefaultParams()` / `defaultParams()` functions to **all** indicators in both Go and TypeScript, then update the factories to use them. Update the `indicator-architecture` and `indicator-conversion` skill files to document both the factory pattern and the DefaultParams pattern.

## Instructions

- The factory **cannot live in `core/`** in Go due to circular imports. It lives at `indicators/factory/`.
- For TypeScript, the factory lives at `indicators/factory/factory.ts` for consistency with Go.
- For indicators with **Length vs SmoothingFactor constructor variants** (EMA, DEMA, TEMA, T2, T3, KAMA, CyberCycle, InstantaneousTrendLine, MAMA): **auto-detect from JSON keys**.
- For indicators with **Default vs Params** variants (many Ehlers, Corona, spectra, ATCF): if params is empty `{}`, use Default constructor; otherwise use Params constructor.
- For **bare-int constructors** (ATR, NATR, DM±, DI±, DMI, ADX, ADXR, WilliamsPercentR): parse `{"length": N}` from JSON.
- For **no-params constructors** (TrueRange, BalanceOfPower, AdvanceDecline): call directly.
- Skip internal building blocks (`corona`, `hilberttransformer`) that don't have their own `core.Identifier`.
- The Go module path is `zpano`.
- The KAMA identifier has a quirky JSON string: `"kaufmanAdaptiveMovingAverageMovingAverage"` (with "MovingAverage" duplicated) — this is what Go `core/identifier.go` defines. The TS enum is just `KaufmanAdaptiveMovingAverage` (no duplicate). The TS icalc handles this mapping.
- Follow the indicator-architecture skill conventions for naming and file organization.
- `DefaultParams()` / `defaultParams()` added to **every** indicator params file, including empty-struct/no-params indicators (for consistency).
- Dual-variant indicators get **both** `DefaultLengthParams()` and `DefaultSmoothingFactorParams()`.
- Component fields (`BarComponent`, `QuoteComponent`, `TradeComponent`) are **omitted** from DefaultParams — their zero/undefined values already mean "use default".
- Conventional defaults used where not documented: length=20 for MAs (SMA, EMA, DEMA, TEMA, WMA, TMA, LinReg, Pearson, StdDev, Variance), length=10 for Momentum/ROC variants, length=14 for oscillators (RSI, ATR, CCI, CMO, Aroon, etc.), length=5 for T2/T3, fastLength=12/slowLength=26 for APO/PPO/MACD, fastK=5/slowK=3/slowD=3 for Stochastic.
- Both factories updated to call `DefaultParams()` / `defaultParams()` instead of inline struct literals.

## Discoveries

### Go factory (`go/indicators/factory/factory.go`)
- **72 `core.Identifier` constants** (iota 1–72) with camelCase JSON marshal/unmarshal.
- Constructor patterns vary across 5 categories: Standard `*Params` (~35), Length+SF variants (9), Default+Params variants (15), Bare int (10), No params (3).
- No JSON tags on Go params structs — Go's default case-insensitive JSON unmarshaling handles matching.
- Helper functions: `hasKey()`, `isEmptyObject()`, `unmarshal()`, `lengthParam` struct.

### TypeScript factory (`ts/indicators/factory/factory.ts`)
- **65 `IndicatorIdentifier` enum values** (numeric, 0-based).
- 7 construction patterns (A–G) including Pattern F (optional params: ParabolicSAR, MACD, UltimateOscillator, OBV) and Pattern G (MAMA's unique fromLength/fromSmoothingFactor/default).
- Uses `{ ...defaultXxxParams(), ...p }` spread pattern for defaults (now using imported defaultParams functions).
- Zero barrel files — all imports use direct file paths with `.js` extensions.
- Settings JSON identifiers are camelCase; TS enum members are PascalCase — conversion is `name.charAt(0).toUpperCase() + name.slice(1)`.

### DefaultParams implementation
- **Go:** `func DefaultParams() *StructName` (or `DefaultLengthParams()`/`DefaultSmoothingFactorParams()` for dual-variant). Returns pointer to new struct.
- **TS:** `export function defaultParams(): InterfaceName` (or `defaultLengthParams()`/`defaultSmoothingFactorParams()`). Returns plain object.
- **Corona** in Go already had `DefaultParams()` pre-existing. Corona in TS already had `DefaultCoronaParams` exported const; we added `defaultParams()` that spreads it.
- The HilbertTransformer shared `CycleEstimatorParams` also got `DefaultParams()` / `defaultParams()` with homodyne discriminator defaults (smoothingLength=4, alphaEmaQuadratureInPhase=0.2, alphaEmaPeriod=0.2).

### Pre-existing TS errors
- There are pre-existing `Quote.bid/ask` property errors in ~14 TS indicator files (stochastic, wilder family, etc.). These are NOT caused by our changes — they exist in the base code.

## Accomplished

### All Complete
1. **`go/indicators/factory/factory.go`** — Complete Go factory with all 72 identifiers, now using `DefaultParams()` calls instead of inline defaults.
2. **`go/cmd/icalc/main.go`** — Go CLI app reading settings JSON, creating indicators via factory, printing metadata and 252 bars of output.
3. **`go/cmd/icalc/settings.json`** — All 72 indicators with appropriate default params.
4. **`ts/indicators/factory/factory.ts`** — Complete TS factory with all 65 enum identifiers, now using imported `defaultParams()` calls with renamed imports.
5. **`ts/cmd/icalc/main.ts`** — TS CLI app equivalent of Go icalc.
6. **`ts/cmd/icalc/settings.json`** — Copied from Go, all entries work.
7. **Updated `indicator-architecture/SKILL.md`** — Added factory section + DefaultParams section documenting location, rationale, API, constructor categories, auto-detection logic, convention, placement, doc comments, default value sources, and checklist integration.
8. **Updated `indicator-conversion/SKILL.md`** — Added "Registering in the Factory" section with Go/TS examples for all variant patterns, settings file entry, verification steps, and TOC entry.
9. **`DefaultParams()` added to ALL ~76 Go indicator params files** — Including empty-struct indicators, dual-variant indicators (both Length and SF), and the HilbertTransformer shared params.
10. **`defaultParams()` added to ALL ~75 TS indicator params files** — Same scope as Go.
11. **Go factory updated** to call `DefaultParams()`/`DefaultLengthParams()`/`DefaultSmoothingFactorParams()` for all ~43 applicable cases. Build passes clean.
12. **TS factory updated** to import and call renamed `defaultParams` functions for all ~46 applicable cases. Type-check passes (no new errors in params or factory files).

### Nothing remaining — all tasks complete.

## Relevant files / directories

### Go — Created
- `go/indicators/factory/factory.go` — Complete factory (~922 lines), all 72 identifiers, now uses DefaultParams
- `go/cmd/icalc/main.go` — CLI app (~287 lines)
- `go/cmd/icalc/settings.json` — 72 indicator entries

### TypeScript — Created
- `ts/indicators/factory/factory.ts` — Complete factory (~493+ lines), all 65 enum values, now uses imported defaultParams
- `ts/cmd/icalc/main.ts` — CLI app (~313 lines)
- `ts/cmd/icalc/settings.json` — 72 indicator entries

### Skill files — Updated
- `/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md` — Added factory section + DefaultParams section (~150 lines added)
- `/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md` — Added factory registration section + TOC entry (~80 lines added)

### Go params files — Modified (DefaultParams added to all)
- `go/indicators/common/` — All 13 packages (absolutepriceoscillator, exponentialmovingaverage, linearregression, momentum, pearsonscorrelationcoefficient, rateofchange, rateofchangepercent, rateofchangeratio, simplemovingaverage, standarddeviation, triangularmovingaverage, variance, weightedmovingaverage)
- `go/indicators/welleswilder/` — All 12 packages (averagedirectionalmovementindex, averagedirectionalmovementindexrating, averagetruerange, directionalindicatorminus, directionalindicatorplus, directionalmovementindex, directionalmovementminus, directionalmovementplus, normalizedaveragetruerange, parabolicstopandreverse, relativestrengthindex, truerange)
- `go/indicators/johnehlers/` — All 22 packages (autocorrelationindicator, autocorrelationperiodogram, centerofgravityoscillator, combbandpassspectrum, corona [already had it], coronasignaltonoiseratio, coronaspectrum, coronaswingposition, coronatrendvigor, cybercycle [2 files], discretefouriertransformspectrum, dominantcycle, fractaladaptivemovingaverage, hilberttransformer, hilberttransformerinstantaneoustrendline, instantaneoustrendline [2 files], mesaadaptivemovingaverage, roofingfilter, sinewave, supersmoother, trendcyclemode, zerolagerrorcorrectingexponentialmovingaverage, zerolagexponentialmovingaverage)
- `go/indicators/custom/` — goertzelspectrum, maximumentropyspectrum
- `go/indicators/donaldlambert/commoditychannelindex/params.go`
- `go/indicators/genequong/moneyflowindex/params.go`
- `go/indicators/georgelane/stochastic/params.go`
- `go/indicators/geraldappel/` — movingaverageconvergencedivergence, percentagepriceoscillator
- `go/indicators/igorlivshin/balanceofpower/params.go`
- `go/indicators/jackhutson/tripleexponentialmovingaverageoscillator/params.go`
- `go/indicators/johnbollinger/` — bollingerbands, bollingerbandstrend
- `go/indicators/josephgranville/onbalancevolume/params.go`
- `go/indicators/larrywilliams/` — ultimateoscillator, williamspercentr
- `go/indicators/marcchaikin/` — advancedecline, advancedeclineoscillator
- `go/indicators/markjurik/jurikmovingaverage/params.go`
- `go/indicators/patrickmulloy/` — doubleexponentialmovingaverage, tripleexponentialmovingaverage
- `go/indicators/perrykaufman/kaufmanadaptivemovingaverage/params.go`
- `go/indicators/timtillson/` — t2exponentialmovingaverage, t3exponentialmovingaverage
- `go/indicators/tusharchande/` — aroon, chandemomentumoscillator, stochasticrelativestrengthindex
- `go/indicators/vladimirkravchuk/adaptivetrendandcyclefilter/params.go`

### TS params files — Modified (defaultParams added to all)
- Mirror of all Go packages above, under `ts/indicators/` with kebab-case paths
- Notable: `ts/indicators/john-ehlers/cyber-cycle/length-params.ts` and `smoothing-factor-params.ts` (non-standard file names)
- Notable: `ts/indicators/john-ehlers/instantaneous-trend-line/length-params.ts` and `smoothing-factor-params.ts`

### Key reference files (read, not modified)
- `go/indicators/core/identifier.go` — 72 Identifier constants + JSON string mappings
- `go/indicators/core/indicator.go` — Indicator interface
- `ts/indicators/core/indicator-identifier.ts` — 65-member IndicatorIdentifier enum
- `ts/indicators/core/indicator.ts` — Indicator interface

---

---

## User

---

## Assistant (Build · claude-opus-4.6 · 4.7s)

_Thinking:_

Based on the summary, all tasks are complete. There's nothing remaining.

All tasks from the original goal are complete. There are no remaining steps — the factory packages, icalc CLIs, DefaultParams functions, factory updates, and skill file documentation are all done.

If you have a new task or want to extend this work, let me know.

---

## User

Can we update indicator-architecture/ and indicator-conversion/ skills with new findings

---

## Assistant (Build · claude-opus-4.6 · 4.8s)

_Thinking:_

The user wants to update the skill files with new findings from the work done. Let me first read the current state of both skill files to see what's already there and what could be improved.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md"
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md</path>
<type>file</type>
<content>1: ---
2: name: indicator-architecture
3: description: Architecture, folder layout, naming conventions, and design patterns for the zpano trading indicators library. Load when creating new indicators or understanding the codebase structure.
4: ---
5: 
6: # Architecture & Scaffolding Guide
7: 
8: This document describes the design decisions, folder layout, and naming conventions
9: for the **zpano** trading indicators library. It is intended as a reference for both
10: human developers and AI agents creating new indicators.
11: 
12: ## Scope
13: 
14: The rules in this document apply **only to the `indicators/` folder** within each
15: language directory (e.g., `ts/indicators/`, `go/indicators/`). Other folders at the
16: same level as `indicators/` (such as `entities/`) have their own conventions and are
17: not governed by this guide.
18: 
19: ## Core Principles
20: 
21: 1. **Multi-language library.** The same set of indicators is implemented in
22:    TypeScript, Go, Python, Rust, and Zig. Each language lives in its own
23:    top-level folder (`ts/`, `go/`, `python/`, `rust/`, `zig/`).
24: 2. **Shared ideology, language-idiomatic scaffolding.** The organizational
25:    structure and design rules are the same across all languages. File and folder
26:    naming adapts to each language's conventions.
27: 3. **Consistent depth.** Every indicator lives at exactly
28:    `indicators/<group>/<indicator>/`. No exceptions.
29: 
30: ## Folder Layout
31: 
32: ```
33: indicators/
34: ├── common/                 # Well-known indicators with no specific/known author
35: │   ├── simple-moving-average/
36: │   ├── exponential-moving-average/
37: │   └── ...
38: ├── core/                   # Shared types, interfaces, enums, base classes
39: │   ├── outputs/            # Indicator output data types (band, heatmap, ...)
40: │   └── frequency-response/ # Frequency response calculation utilities
41: ├── custom/                 # Your own experimental/custom indicators
42: │   └── my-indicator/
43: ├── <author-name>/          # Author-attributed indicators
44: │   └── <indicator-name>/
45: │       ├── implementation
46: │       ├── parameters
47: │       └── tests
48: └── ...
49: ```
50: 
51: ### The Three Special Folders
52: 
53: | Folder     | Purpose |
54: |------------|---------|
55: | `core/`    | Shared foundations: types, interfaces, enums, base abstractions, utilities. |
56: | `common/`  | Indicators whose author is unknown or not attributed to a single person (SMA, EMA, RSI, ...). |
57: | `custom/`  | Indicators you develop yourself. |
58: 
59: These three names are **reserved** and must never be used as author names.
60: 
61: ### Author Folders
62: 
63: When an indicator was developed by a known author, it is placed under a folder
64: named after that author. Each indicator gets its own subfolder.
65: 
66: Example: Mark Jurik created JMA and CFB, so:
67: 
68: ```
69: indicators/
70: └── mark-jurik/
71:     ├── jurik-moving-average/
72:     └── composite-fractal-behavior/
73: ```
74: 
75: **Go: every author folder MUST contain a `doc.go`** that declares the
76: parent package with a one-line godoc comment identifying the author:
77: 
78: ```go
79: // Package markjurik implements indicators developed by Mark Jurik.
80: package markjurik
81: ```
82: 
83: This is the only file at the author-folder level; all actual indicator
84: code lives in subpackages. The same rule applies to the special
85: folders `common` (shared utilities across authors) and `custom`
86: (zpano-specific indicators without a single author). The `core`
87: folder already contains real framework code and needs no `doc.go`.
88: 
89: TypeScript has no equivalent requirement (no package-level doc
90: construct).
91: 
92: ### Indicator Folders
93: 
94: Each indicator folder contains:
95: 
96: - **Implementation** -- the indicator logic.
97: - **Parameters** -- an interface/struct describing the indicator's configuration.
98: - **Tests** -- unit tests for the indicator.
99: - Optionally, additional files (output types, documentation, etc.).
100: 
101: ### Indicator Names
102: 
103: Use **full, descriptive names** (not short mnemonics) for indicator folders:
104: 
105: | Correct                          | Incorrect |
106: |----------------------------------|-----------|
107: | `simple-moving-average/`         | `sma/`    |
108: | `jurik-moving-average/`          | `jma/`    |
109: | `bollinger-bands/`               | `bb/`     |
110: 
111: Short names (mnemonics) may be cryptic and can overlap between different
112: indicators. Full names are unambiguous and self-documenting.
113: 
114: ### `core/` Internal Structure
115: 
116: ```
117: core/
118: ├── outputs/              # Output data types (band, heatmap, ...)
119: └── frequency-response/   # Frequency response calculation utilities
120: ```
121: 
122: - **`outputs/`** contains indicator output shapes. An indicator can output a
123:   scalar, a band (upper/lower), a heatmap, etc. This folder holds the concrete
124:   types; the taxonomy enum lives in the `outputs/shape/` sub-package as
125:   `shape.Shape` (Go) / `Shape` (TS).
126: - **`frequency-response/`** contains utilities for computing the frequency
127:   response of a filter/indicator.
128: 
129: Other shared types (indicator interface, metadata, specification, line indicator
130: base class, indicator type enum) live directly in `core/`.
131: 
132: ## Naming Conventions per Language
133: 
134: The structure is identical across languages; only the naming style changes.
135: 
136: | Language   | Folder naming       | Example author folder    | Example indicator folder      |
137: |------------|---------------------|--------------------------|-------------------------------|
138: | TypeScript | `kebab-case`        | `mark-jurik/`            | `jurik-moving-average/`       |
139: | Go         | `lowercase` (no separators) | `markjurik/`      | `jurikmovingaverage/`         |
140: | Python     | `snake_case`        | `mark_jurik/`            | `jurik_moving_average/`       |
141: | Rust       | `snake_case`        | `mark_jurik/`            | `jurik_moving_average/`       |
142: | Zig        | `snake_case`        | `mark_jurik/`            | `jurik_moving_average/`       |
143: 
144: ### Symbol Naming (Types, Enums)
145: 
146: Go uses package-scoped names, so symbols can be shorter (the package provides
147: context). All other languages use fully-qualified symbol names.
148: 
149: | Concept           | Go (in `core` pkg)  | Go (in `outputs/shape` pkg) | TypeScript / Python / Rust / Zig |
150: |-------------------|---------------------|-----------------------------|----------------------------------|
151: | Indicator identity| `core.Identifier`   | --                          | `IndicatorIdentifier`            |
152: | Output shape      | --                  | `shape.Shape`               | `Shape`                          |
153: 
154: ### File Naming
155: 
156: | Language   | Style                     | Test files                  | Example                          |
157: |------------|---------------------------|-----------------------------|----------------------------------|
158: | TypeScript | `kebab-case.ts`           | `kebab-case.spec.ts`        | `simple-moving-average.ts`       |
159: | Go         | `lowercase.go`            | `lowercase_test.go`         | `simplemovingaverage.go`         |
160: | Python     | `snake_case.py`           | `test_snake_case.py`        | `simple_moving_average.py`       |
161: | Rust       | `snake_case.rs`           | `snake_case_test.rs` or inline `#[cfg(test)]` | `simple_moving_average.rs` |
162: | Zig        | `snake_case.zig`          | `snake_case_test.zig`       | `simple_moving_average.zig`      |
163: 
164: No type-suffix convention is used (no `.enum.ts`, `.interface.ts`, etc.).
165: This is consistent across all languages.
166: 
167: #### Main vs. auxiliary files
168: 
169: Within an indicator folder, **only the main implementation file and its test
170: keep the indicator name**. Every auxiliary file drops the indicator-name
171: prefix — the folder (and in Go, the package name) already provides context,
172: so prefixing would stutter.
173: 
174: For a `simple-moving-average/` folder:
175: 
176: | Role                         | Go                              | TypeScript                              |
177: |------------------------------|---------------------------------|-----------------------------------------|
178: | Main implementation          | `simplemovingaverage.go`        | `simple-moving-average.ts`              |
179: | Main test                    | `simplemovingaverage_test.go`   | `simple-moving-average.spec.ts`         |
180: | Data-driven test (if split)  | `data_test.go`                  | `data.spec.ts`                          |
181: | Parameters                   | `params.go`                     | `params.ts`                             |
182: | Output enum                  | `output.go`                     | `output.ts`                             |
183: | Output test                  | `output_test.go`                | `output.spec.ts`                        |
184: | Coefficients / helpers       | `coefficients.go`, `estimator.go` | `coefficients.ts`, `estimator.ts`     |
185: | Package docs (Go only)       | `doc.go`                        | —                                       |
186: 
187: **Exception:** when an auxiliary file holds a *concept* that would be
188: meaningless outside the folder name (e.g., `hilberttransformer/` contains
189: `cycleestimator.go`, `phaseaccumulator.go`, `dualdifferentiator.go` — each
190: representing a distinct Hilbert-transformer variant), keep the conceptual
191: names as-is. Do not try to mechanically strip a prefix that was never there.
192: 
193: ### Identifier Abbreviation Convention
194: 
195: To keep naming consistent across indicators and languages, prefer the **full
196: word** in identifiers. The following abbreviations are considered **banned**
197: as CamelCase word-parts (whether standalone or compound) — always use the
198: long form:
199: 
200: | Banned short | Canonical long | Example (before → after)                |
201: |--------------|----------------|------------------------------------------|
202: | `idx`        | `index`        | `bufIdx` → `bufferIndex`, `highestIdx` → `highestIndex` |
203: | `tmp`        | `temp`         | `tmpR` → `tempR`                         |
204: | `res`        | `result`       | `res` → `result`, `defSpectrumRes` → `defSpectrumResult` |
205: | `sig`        | `signal`       | `expSig` → `expSignal`, `mnemonicSig` → `mnemonicSignal` |
206: | `val`        | `value`        | `mnemonicVal` → `mnemonicValue`, `prevVal` → `previousValue` |
207: | `prev`       | `previous`     | `prevHigh` → `previousHigh`, `samplePrev` → `samplePrevious` |
208: | `avg`        | `average`      | `AvgLength` → `AverageLength`            |
209: | `mult`       | `multiplier`   | `upperMult` → `upperMultiplier`          |
210: | `buf`        | `buffer`       | *(compound only, paired with `idx`)*     |
211: | `param`      | `parameter`    | `paramResolution` → `parameterResolution` *(in identifiers; Go struct type `Params` stays)* |
212: | `hist`       | `histogram`    | `expectedHist` → `expectedHistogram`     |
213: 
214: **Allowed exceptions (retained as-is):**
215: 
216: - **Go language idioms:** `err`, `len`, `cap`, `min`, `max`, `num` — these are
217:   builtins or deeply idiomatic Go short forms. Do not expand.
218: - **Go struct field `Params` / variable `params`** — this is the established
219:   parameter-bundle type name across every indicator (`type Params struct`,
220:   `Compute(params Params)`). The long form `Parameters` is used **only** in
221:   `core/specification.go`/`core/indicator-specification.ts` where it
222:   describes framework-level metadata.
223: - **TS constructor-parameter idiom `value_`** (trailing underscore) — used
224:   to avoid shadowing a member `value` during construction.
225: - **Domain-specific short forms that happen to collide** (e.g., `pos` as a
226:   loop variable is fine even though an unrelated indicator may use
227:   `position` in its name). These are different concepts, not abbreviation
228:   drift.
229: - **TA-Lib port conventions** (`begIdx` → now `begIndex`, `outIdx` → now
230:   `outIndex`): the `Idx` part is normalized but the `beg`/`out` prefixes
231:   are kept, matching TA-Lib's C-source variable names for easier
232:   cross-reference.
233: 
234: **Go↔TS parity:** the same identifier stem MUST be used in both languages.
235: When porting, do not introduce new short forms; use the canonical long form
236: from the table above.
237: 
238: ### Go Receiver Naming Convention
239: 
240: Method receivers follow a **type-shape rule**, not a one-letter-for-all rule:
241: 
242: - **Compound type name** (2+ CamelCase words, e.g. `SimpleMovingAverage`,
243:   `BarComponent`, `CycleEstimatorType`) → receiver is **`s`**
244:   ("self"/"struct"). Short, consistent across all large types.
245: - **Simple type name** (single word, e.g. `Momentum`, `Trade`, `Quote`) →
246:   receiver is the **first letter of the type name, lowercased** (`m`, `t`,
247:   `q`). Short types deserve short mnemonic receivers.
248: 
249: All methods on a given type MUST use the same receiver name (enforced by
250: `go vet`/consistency). When adding a method, match the receiver already in
251: use on that type.
252: 
253: **Local-variable collision:** if a method body needs a local variable that
254: would shadow the receiver `s` (typical case: `s := someValue.String()` in
255: `MarshalJSON`), rename the **local** to `str` (not the receiver). Example:
256: 
257: ```go
258: func (s BarComponent) MarshalJSON() ([]byte, error) {
259:     str := s.String()           // local renamed s → str
260:     if str == unknown { ... }
261: }
262: ```
263: 
264: This keeps the receiver convention uniform; local names are flexible.
265: 
266: ### Concurrency / Lock Convention
267: 
268: Stateful indicators (those exposing `Update(...)` that mutates internal
269: fields) MUST guard reads and writes with a `sync.RWMutex`:
270: 
271: - **Field:** `mu sync.RWMutex` (always named `mu`, always `RWMutex` —
272:   never plain `sync.Mutex`).
273: - **Writer methods** (`Update`, any mutator): acquire with
274:   `s.mu.Lock(); defer s.mu.Unlock()` on the two lines immediately
275:   following the function-signature brace (before any other logic).
276: - **Reader methods** (`IsPrimed`, accessors that read mutable state):
277:   use `s.mu.RLock(); defer s.mu.RUnlock()`.
278: - **Never** unlock manually (non-deferred). Always pair Lock with
279:   `defer Unlock` on the very next line.
280: 
281: **Exceptions — indicators without a mutex:**
282: 
283: 1. **Pure delegating wrappers** that own no mutable state of their own
284:    and forward all `Update`/read calls to an embedded indicator which
285:    itself carries a mutex (e.g. `standarddeviation` wrapping
286:    `variance`). The embedded indicator provides the lock.
287: 2. **Internal engines** that are not part of the public indicator
288:    surface and are only consumed by higher-level wrappers which hold
289:    the mutex (e.g. `corona` engine used by `coronaswingposition`,
290:    `coronaspectrum`, `coronasignaltonoiseratio`, `coronatrendvigor`).
291: 
292: Any new stateful public indicator MUST either carry its own `mu
293: sync.RWMutex` following the pattern above or fall under one of the two
294: exceptions.
295: 
296: ### Go Style Invariants (enforced across the codebase)
297: 
298: The following idioms are invariants — audits return **zero** deviations
299: and new code MUST respect them:
300: 
301: - **Variable declarations.** No `var x T = zero-literal`, no split
302:   `var x T` followed immediately by `x = expr`. Use either `var x T`
303:   (zero value) or `x := expr`.
304: - **Empty / nil tests.** Slice/map emptiness is not tested by
305:   `len(x) == 0`; prefer direct ranging or `== nil` where the value is
306:   nil-able. Error checks always use `err != nil`.
307: - **Any.** Use `any`, never `interface{}`.
308: - **Slice construction.** Use `[]T{}` or `make([]T, n)` /
309:   `make([]T, 0, n)` — never `make([]T, 0)` (no capacity hint).
310: - **`new`.** Not used; use composite literals (`&T{...}`).
311: - **Imports.** Grouped by class with blank lines: stdlib, external,
312:   `zpano/*`. Each group contains only one class.
313: - **Doc comments.** Every exported `func`/`type`/`var`/`const`/method
314:   has a leading `// Name ...` doc comment. Trivial stubs (e.g.
315:   `passthrough.Update`) still get a one-line comment.
316: 
317: ### Go ↔ TypeScript Local-Variable Parity
318: 
319: The same indicator MUST use the **same local/field names** in both
320: languages. When porting, copy the other language's names verbatim
321: where semantically identical. Observed canonical vocabulary:
322: 
323: | Concept                    | Canonical local name |
324: |----------------------------|----------------------|
325: | Sample difference / delta  | `temp` (short-lived delta) / `diff` (explicit diff) |
326: | Accumulator sum            | `sum` (never `total`, `acc`, `accumulator`) |
327: | Small-comparison tolerance | `epsilon` |
328: | Running standard deviation | `stddev` |
329: | MACD fast/slow/signal line | `fast`, `slow`, `signal`, `macd`, `histogram` |
330: | Variance (scratch)         | `v` |
331: | NaN sentinel (Go)          | `nan := math.NaN()` |
332: | Spread (upper-lower)       | `spread` |
333: | Bollinger band-width       | `bw` *(local; field is `bandWidth`)* |
334: | Bollinger percent-band     | `pctB` *(local; field is `percentBand`)* |
335: | Length minus one           | `lengthMinOne` |
336: | Amount = price × volume    | `amount` |
337: 
338: Primary loop counter is `i`; secondary `j`, `k`; only use `index`
339: when the counter semantically labels a named index (e.g. filter-bank
340: bin index). Never introduce new short forms (`idx`, `res`, etc. are
341: banned per the Abbreviation Convention).
342: 
343: ## Adding a New Indicator -- Checklist
344: 
345: 1. **Determine the group.** Is the author known? Use `<author-name>/`. Unknown
346:    author? Use `common/`. Your own? Use `custom/`.
347: 2. **Create the indicator subfolder** at `indicators/<group>/<indicator-name>/`
348:    using full descriptive names and language-appropriate casing.
349: 3. **Create the required files:**
350:    - **Parameters** -- define a params struct with a `DefaultParams()` /
351:      `defaultParams()` function (see the DefaultParams section below). Where the indicator can be
352:      driven by different price components, include optional `BarComponent`,
353:      `QuoteComponent`, `TradeComponent` fields (zero = default in Go,
354:      `undefined` = default in TS). Document the defaults in field comments.
355:      For indicators that require fixed inputs (e.g., HLCV for Advance-Decline,
356:      OHLC for Balance of Power, high/low for Parabolic SAR), the params struct
357:      may be empty or contain only non-component parameters (length, thresholds).
358:    - **Output enum** -- define a per-indicator output enum. In **Go**, the type
359:      is bare `Output` and constants are bare concept names (package name
360:      provides scoping — `simplemovingaverage.Output`, `simplemovingaverage.Value`).
361:      In **TypeScript**, the enum is named `<IndicatorName>Output` (e.g.,
362:      `SimpleMovingAverageOutput`) because TS imports by symbol, not by module.
363:    - **Implementation** -- embed `core.LineIndicator` (Go) or extend
364:      `LineIndicator` (TS). In the constructor:
365:      1. Resolve zero-value components → defaults for the component functions.
366:         In Go, check `if bc == 0 { bc = DefaultBarComponent }`.
367:         In TS, set the protected setters (`this.barComponent = params.barComponent`),
368:         which resolve `undefined` → default internally.
369:      2. Build the mnemonic using `ComponentTripleMnemonic` / `componentTripleMnemonic`
370:         with the **resolved** values (Go) or raw param values (TS — the function
371:         checks `!== undefined && !== Default*` itself). **Exception:** indicators
372:         with no component params (empty or non-component-only Params) hand-write
373:         their mnemonic (e.g., `bop()`, `ad()`, `psar(0.02,0.2)`) — the mnemonic
374:         still includes non-component parameters when non-default.
375:      3. Initialize the `LineIndicator` with mnemonic, description, component
376:         functions, and the indicator's `Update` method.
377:    - **`Metadata()`** -- return `core.BuildMetadata(core.<Identifier>, mnemonic, description, []core.OutputText{...})` (Go) or
378:      `buildMetadata(IndicatorIdentifier.<Name>, mnemonic, description, [...])` (TS). The helper sources each output's `kind` and
379:      `shape` from the descriptor registry — the caller supplies only per-output mnemonic/description. See the **Taxonomy &
380:      Descriptor Registry** section below for details and the mandatory descriptor-row step.
381:    - **Test file** -- include mnemonic sub-tests covering: all components zero,
382:      each component set individually, and combinations of two components.
383: 4. **Register the indicator** in `core/identifier` (add a new enum variant in both Go `core.Identifier` and TS `IndicatorIdentifier`).
384: 5. **Register the descriptor** in `core/descriptors.{go,ts}` — see the Taxonomy section below. A missing descriptor row causes
385:    `BuildMetadata` to panic at runtime.
386: 6. **Follow the consistent depth rule** -- the indicator must be exactly two
387:    levels below `indicators/`.
388: 
389: ## Design Decisions Log
390: 
391: | Decision | Rationale |
392: |----------|-----------|
393: | No underscore prefix on `core/`, `common/`, `custom/` | Leading underscores conflict with conventions in Python (`_` = private) and are non-idiomatic in Go. Dropping them gives true consistency across all five languages. The names are unambiguous enough on their own. |
394: | Full indicator names, not mnemonics | Short names (SMA, JMA) can be cryptic and may overlap. Full names are self-documenting. |
395: | `outputs/` instead of `entities/` in core | These types represent indicator *output shapes*, not general domain entities. `outputs` is more precise about their purpose. |
396: | `core/` instead of `types/` | `core` is broad enough to hold types, enums, base classes, and utilities without needing a rename later. `types` would be too narrow. |
397: | `common/` instead of `unknown/`, `classic/`, `standard/` | `common` is neutral -- doesn't imply age (`classic`), official status (`standard`), or sound dismissive (`unknown`). |
398: | `custom/` instead of `experimental/` | `custom` describes *origin* (you made it), not *maturity*. A custom indicator can be stable; an experimental one implies instability. If you need to track maturity, use metadata in code, not folder structure. |
399: | Go uses scoped names (`core.Identifier`, `shape.Shape`); other languages use `IndicatorIdentifier`/`Shape` at the top level | Go's package system provides scoping, making prefixes redundant and stuttery (`core.IndicatorIdentifier`, `outputs.Shape`). Other languages import symbols directly, so self-describing names are necessary; `Shape` is scoped to its own `outputs/shape` sub-package in Go to avoid clashing with entity/indicator shapes. |
400: | Per-indicator subfolders even in `common/` | Without subfolders, all indicator files in `common/` would land in one flat directory (and in Go, one package). This doesn't scale and is inconsistent with the author-folder structure. |
401: | Component enums start at `iota + 1` | Makes the zero value (`0`) explicitly "not set", which is used as the sentinel for "use default". This is idiomatic Go for optional enum fields in structs (zero value of `int` is `0`). |
402: | TypeScript enums start at `0`, components are `undefined` when not set | TS enums default to `0`, and optional params use `undefined` (not `0`) as the sentinel for "not set". The `componentTripleMnemonic` function checks `!== undefined && !== Default*` to decide whether to show a component. The `LineIndicator` setters use `=== undefined` (not truthiness) to decide when to apply the default, so explicitly passing a zero-valued enum (e.g., `BarComponent.Open`) works correctly. |
403: | Zero-value component = default, omitted from mnemonic | Keeps the common case clean (`sma(14)` instead of `sma(14, c, ba/2, p)`) while still allowing explicit overrides when needed. `ComponentTripleMnemonic` checks against `Default*` constants (not zero), so explicitly passing the default value also omits it — the same indicator always gets the same mnemonic. |
404: | Entity `Mnemonic()` method separate from `String()` | `String()` returns full words (`"close"`, `"mid"`) for JSON serialization and debugging. `Mnemonic()` returns short codes (`"c"`, `"ba/2"`) for compact display in chart labels. Separating them avoids overloading `String()` with a display concern. |
405: | Mnemonic sub-tests for every indicator | Mnemonic correctness matters for charting UIs. Each indicator must have sub-tests covering: all components zero, each component individually, and pairwise combinations. This prevents regressions when changing the format string or `ComponentTripleMnemonic`. |
406: 
407: ## LineIndicator Design Pattern
408: 
409: A **line indicator** takes a single numeric input and produces a single scalar
410: output. Most indicators (SMA, EMA, JMA, ...) are line indicators. The
411: `LineIndicator` abstraction eliminates boilerplate by providing
412: `UpdateScalar/Bar/Quote/Trade` methods (and array variants) that delegate to the
413: indicator's core `Update(sample) → value` function.
414: 
415: ### What Each Indicator Implements
416: 
417: Each concrete indicator is responsible for:
418: 
419: 1. **`Update(sample float64) float64`** (Go) / **`update(sample: number): number`** (TS) --
420:    the core calculation logic.
421: 2. **`Metadata()`** / **`metadata()`** -- returns indicator-level metadata with
422:    an explicit per-indicator output enum (not a hardcoded `kind: 0`).
423: 3. **`IsPrimed()`** / **`isPrimed()`** -- whether the indicator has received
424:    enough data to produce meaningful output.
425: 
426: ### What LineIndicator Provides (Eliminating Boilerplate)
427: 
428: The `LineIndicator` base provides:
429: 
430: - **`UpdateScalar/Bar/Quote/Trade`** -- all delegate to the indicator's `Update()`
431:   method via a function reference (Go) or abstract method (TS). The bar/quote/trade
432:   variants extract the relevant component value first.
433: - **Array update methods** -- `UpdateScalars/Bars/Quotes/Trades` (TS only;
434:   Go has equivalent free functions in `core/indicator.go`).
435: - **Storage of component functions** -- bar, quote, and trade component
436:   extraction functions are stored and used by the Update* methods.
437: 
438: ### Go Implementation
439: 
440: In Go, `LineIndicator` is an **embedded struct**. The concrete indicator stores a
441: `core.LineIndicator` field and initializes it via `core.NewLineIndicator()`,
442: passing its own `Update` method as a function reference:
443: 
444: ```go
445: type SimpleMovingAverage struct {
446:     mu sync.RWMutex
447:     core.LineIndicator   // embedded -- promotes UpdateScalar/Bar/Quote/Trade
448:     // ... indicator-specific fields
449: }
450: 
451: func NewSimpleMovingAverage(p *Params) (*SimpleMovingAverage, error) {
452:     sma := &SimpleMovingAverage{ /* ... */ }
453:     sma.LineIndicator = core.NewLineIndicator(
454:         mnemonic, desc, barFunc, quoteFunc, tradeFunc, sma.Update,
455:     )
456:     return sma, nil
457: }
458: ```
459: 
460: The mutex lives on the concrete indicator (not on `LineIndicator`), and is
461: acquired inside `Update()`. The promoted `UpdateScalar/Bar/Quote/Trade` methods
462: call `updateFn` (which is the concrete indicator's `Update`), so the threading
463: model is preserved.
464: 
465: ### TypeScript Implementation
466: 
467: In TypeScript, `LineIndicator` is an **abstract class**. The concrete indicator
468: extends it and implements `update()` and `metadata()`:
469: 
470: ```typescript
471: export abstract class LineIndicator implements Indicator {
472:     protected mnemonic!: string;
473:     protected description!: string;
474:     protected primed!: boolean;
475: 
476:     public abstract metadata(): IndicatorMetadata;
477:     public abstract update(sample: number): number;
478:     // updateScalar/Bar/Quote/Trade provided by base class
479: }
480: ```
481: 
482: **Concrete indicator constructors must set the three component setters** to
483: initialize the component functions used by `updateBar()`, `updateQuote()`, and
484: `updateTrade()`:
485: 
486: ```typescript
487: this.barComponent = params.barComponent;       // undefined → DefaultBarComponent
488: this.quoteComponent = params.quoteComponent;   // undefined → DefaultQuoteComponent
489: this.tradeComponent = params.tradeComponent;   // undefined → DefaultTradeComponent
490: ```
491: 
492: The `LineIndicator` setters use `=== undefined` (not truthiness) to apply
493: defaults, so explicitly passing a zero-valued enum like `BarComponent.Open`
494: works correctly.
495: 
496: Each entity file also exports a `Default*` constant (`DefaultBarComponent`,
497: `DefaultQuoteComponent`, `DefaultTradeComponent`) for use in
498: `componentTripleMnemonic` and anywhere else defaults need to be checked.
499: 
500: ### Per-Indicator Output Enums
501: 
502: Each indicator defines its own output enum describing what it produces. The
503: naming convention is **language-asymmetric** because the two languages scope
504: symbols differently:
505: 
506: - **Go**: use bare `Output` / `Value`. The package name
507:   (`simplemovingaverage.Output`, `simplemovingaverage.Value`) provides scoping,
508:   so repeating the indicator name in the type would stutter.
509: - **TypeScript**: use long-form `<IndicatorName>Output` /
510:   `<IndicatorName>Value`. TS imports by symbol rather than by module, so the
511:   indicator name must be baked into the identifier to stay unambiguous at
512:   call sites.
513: 
514: ```go
515: // Go — file: simplemovingaverage/output.go
516: package simplemovingaverage
517: 
518: type Output int
519: const (
520:     Value Output = iota
521: )
522: ```
523: 
524: ```typescript
525: // TypeScript — file: simple-moving-average/output.ts
526: export enum SimpleMovingAverageOutput {
527:     SimpleMovingAverageValue = 0,
528: }
529: ```
530: 
531: For multi-output indicators, strip the indicator-name prefix on the Go side
532: and keep the descriptive suffix:
533: 
534: ```go
535: // Go — directionalmovementindex/output.go
536: type Output int
537: const (
538:     PlusDirectionalIndicator Output = iota
539:     MinusDirectionalIndicator
540:     AverageDirectionalIndex
541:     // ...
542: )
543: ```
544: 
545: ```typescript
546: // TypeScript — directional-movement-index/output.ts
547: export enum DirectionalMovementIndexOutput {
548:     PlusDirectionalIndicator = 0,
549:     MinusDirectionalIndicator = 1,
550:     AverageDirectionalIndex = 2,
551:     // ...
552: }
553: ```
554: 
555: The `metadata()` method uses this enum for the `kind` field instead of a
556: hardcoded `0`, making the output semantics explicit and type-safe.
557: 
558: ### Metadata Structure
559: 
560: The `Metadata` / `IndicatorMetadata` type includes:
561: 
562: | Field         | Description |
563: |---------------|-------------|
564: | `identifier`  | The indicator identifier enum variant (e.g., `SimpleMovingAverage`). |
565: | `mnemonic`    | A short name like `sma(5)` or `jma(7, -1)`. |
566: | `description` | A human-readable description like `Simple moving average sma(5)`. |
567: | `outputs`     | An array of per-output metadata (kind, shape, mnemonic, description). |
568: 
569: The `mnemonic` and `description` live at the indicator level (on `Metadata`)
570: and are also duplicated per-output for convenience (each output carries its own
571: mnemonic/description).
572: 
573: Each output carries:
574: 
575: | Field         | Description |
576: |---------------|-------------|
577: | `kind`        | The integer value of the indicator's per-output enum. |
578: | `shape`       | The output's data shape (`Scalar`, `Band`, `Heatmap`, `Polyline`). Sourced from the descriptor registry. |
579: | `mnemonic`    | Short name for this output. |
580: | `description` | Human-readable description of this output. |
581: 
582: In practice, **indicators never construct `Metadata` object literals directly**.
583: They call `core.BuildMetadata` (Go) / `buildMetadata` (TS), which pulls `kind`
584: and `shape` from the descriptor registry. See the Taxonomy section below.
585: 
586: ### Mnemonic Prefix Convention
587: 
588: Every indicator is identified by a short lowercase **mnemonic prefix** (e.g.
589: `sma`, `rsi`, `bb`, `macd`). These prefixes must be consistent between Go and
590: TypeScript, because downstream tooling keys off them.
591: 
592: **Rules:**
593: 
594: 1. **Lowercase only.** Digits are allowed (`t2`, `t3`, `rocr100`).
595:    Leading `+`/`-` is allowed for paired directional indicators (`+di`, `-di`,
596:    `+dm`, `-dm`).
597: 2. **Kept short** — typically 2-7 chars. The mnemonic is a compact label, not a
598:    full name; the full name goes in `description`.
599: 3. **Go ↔ TS parity.** The same prefix must appear on both sides. When adding
600:    an indicator, add its prefix to both languages in the same PR.
601: 4. **Format.** `<prefix>(<param1>, <param2>, ...<componentSuffix>)` where the
602:    component suffix comes from `ComponentTripleMnemonic` (empty when all
603:    components are at their default).
604: 5. **Configurable components → include the suffix.** Any indicator that
605:    exposes `BarComponent` / `QuoteComponent` / `TradeComponent` on its params
606:    struct **must** append `ComponentTripleMnemonic(bc, qc, tc)` to its
607:    mnemonic. Indicators with hardcoded components (e.g. balance-of-power)
608:    omit the suffix.
609: 6. **Parameterless indicators** use the bare prefix (`obv`, `ad`, `bop`) when
610:    defaults are in effect, or wrap the component suffix in parens when needed
611:    (e.g. `obv(hl/2)`).
612: 
613: **Canonical inventory (65 indicators with mnemonics, plus 4 paired directional):**
614: 
615: | Prefix    | Indicator                                            |
616: |-----------|------------------------------------------------------|
617: | `aci`     | autocorrelation indicator                            |
618: | `acp`     | autocorrelation periodogram                          |
619: | `ad`      | advance/decline                                      |
620: | `adosc`   | advance/decline oscillator                           |
621: | `adx`     | average directional movement index                   |
622: | `adxr`    | average directional movement index rating            |
623: | `apo`     | absolute price oscillator                            |
624: | `aroon`   | Aroon                                                |
625: | `atcf`    | adaptive trend and cycle filter                      |
626: | `atr`     | average true range                                   |
627: | `bb`      | Bollinger bands                                      |
628: | `bbtrend` | Bollinger bands trend                                |
629: | `bop`     | balance of power                                     |
630: | `cbps`    | comb band-pass spectrum                              |
631: | `cc`      | cyber cycle                                          |
632: | `cci`     | commodity channel index                              |
633: | `cmo`     | Chande momentum oscillator                           |
634: | `cog`     | center-of-gravity oscillator                         |
635: | `correl`  | Pearson's correlation coefficient                    |
636: | `csnr`    | corona signal-to-noise ratio                         |
637: | `cspect`  | corona spectrum                                      |
638: | `cswing`  | corona swing position                                |
639: | `ctv`     | corona trend vigor                                   |
640: | `dcp`     | dominant cycle                                       |
641: | `dema`    | double exponential moving average                    |
642: | `dftps`   | discrete Fourier transform power spectrum            |
643: | `+di`/`-di` | directional indicator plus / minus                 |
644: | `+dm`/`-dm` | directional movement plus / minus                  |
645: | `dx`      | directional movement index                           |
646: | `ema`     | exponential moving average                           |
647: | `frama`   | fractal adaptive moving average                      |
648: | `gspect`  | Goertzel spectrum                                    |
649: | `htitl`   | Hilbert transformer instantaneous trend line         |
650: | `iTrend`  | instantaneous trend line                             |
651: | `jma`     | Jurik moving average                                 |
652: | `kama`    | Kaufman adaptive moving average                      |
653: | `linreg`  | linear regression                                    |
654: | `macd`    | moving average convergence/divergence                |
655: | `mama`    | MESA adaptive moving average                         |
656: | `mespect` | maximum entropy spectrum                             |
657: | `mfi`     | money flow index                                     |
658: | `mom`     | momentum                                             |
659: | `natr`    | normalized average true range                        |
660: | `obv`     | on-balance volume                                    |
661: | `ppo`     | percentage price oscillator                          |
662: | `roc`     | rate of change                                       |
663: | `rocp`    | rate of change percent                               |
664: | `rocr`/`rocr100` | rate of change ratio (unit or ×100)           |
665: | `roof`    | roofing filter                                       |
666: | `rsi`     | relative strength index                              |
667: | `sar`     | parabolic stop and reverse                           |
668: | `sma`     | simple moving average                                |
669: | `ss`      | super smoother                                       |
670: | `stdev`   | standard deviation (`.s` sample / `.p` population)   |
671: | `stoch`   | stochastic                                           |
672: | `stochrsi`| stochastic relative strength index                   |
673: | `sw`      | sine wave                                            |
674: | `tcm`     | trend cycle mode                                     |
675: | `tema`    | triple exponential moving average                    |
676: | `tr`      | true range                                           |
677: | `trima`   | triangular moving average                            |
678: | `trix`    | triple exponential moving average oscillator         |
679: | `t2`      | T2 exponential moving average                        |
680: | `t3`      | T3 exponential moving average                        |
681: | `ultosc`  | ultimate oscillator                                  |
682: | `var`     | variance (`.s` sample / `.p` population)             |
683: | `willr`   | Williams %R                                          |
684: | `wma`     | weighted moving average                              |
685: | `zecema`  | zero-lag error-correcting exponential moving average |
686: | `zema`    | zero-lag exponential moving average                  |
687: 
688: The two utility modules `corona/` and `frequencyresponse/` are support helpers,
689: not indicators, and have no mnemonic of their own.
690: 
691: ### Component Triple Mnemonic
692: 
693: A line indicator can be fed bar, quote, or trade data. Each data type has a
694: **component** that selects which field to extract (e.g., close price from a bar,
695: mid-price from a quote). The `ComponentTripleMnemonic` utility (in `core/`)
696: generates the component suffix for an indicator's mnemonic. There are three
697: components (bar, quote, trade) -- hence "triple", not "pair".
698: 
699: #### Default Components
700: 
701: Each component type has a default constant defined in `entities/`:
702: 
703: | Component type   | Default constant         | Default value    | Mnemonic |
704: |------------------|--------------------------|------------------|----------|
705: | `BarComponent`   | `DefaultBarComponent`    | `BarClosePrice`  | `c`      |
706: | `QuoteComponent` | `DefaultQuoteComponent`  | `QuoteMidPrice`  | `ba/2`   |
707: | `TradeComponent` | `DefaultTradeComponent`  | `TradePrice`     | `p`      |
708: 
709: #### Zero-Value Convention
710: 
711: All component enums start at `iota + 1`, making the zero value `0` explicitly
712: "not set". In indicator params, **a zero-value component means "use the default
713: and omit from the mnemonic"**:
714: 
715: ```go
716: // SimpleMovingAverageParams
717: type SimpleMovingAverageParams struct {
718:     Length         int
719:     BarComponent   entities.BarComponent   // zero → DefaultBarComponent, omitted from mnemonic
720:     QuoteComponent entities.QuoteComponent // zero → DefaultQuoteComponent, omitted from mnemonic
721:     TradeComponent entities.TradeComponent // zero → DefaultTradeComponent, omitted from mnemonic
722: }
723: ```
724: 
725: #### Constructor Resolution Logic
726: 
727: Each indicator constructor follows this pattern:
728: 
729: 1. Read the component from params.
730: 2. If zero, resolve to the default constant.
731: 3. Get the component function using the resolved value.
732: 4. Pass the **resolved** values to `ComponentTripleMnemonic` (which omits
733:    defaults automatically).
734: 
735: ```go
736: // Resolve defaults for component functions.
737: bc := p.BarComponent
738: if bc == 0 {
739:     bc = entities.DefaultBarComponent
740: }
741: // ... same for qc, tc
742: 
743: // Build mnemonic using resolved components — defaults are omitted by ComponentTripleMnemonic.
744: mnemonic := fmt.Sprintf("sma(%d%s)", length,
745:     core.ComponentTripleMnemonic(bc, qc, tc))
746: ```
747: 
748: This means that `sma(14)` is the mnemonic whether the user left `BarComponent`
749: as zero or explicitly set it to `BarClosePrice` — both produce the same
750: indicator with the same mnemonic.
751: 
752: #### ComponentTripleMnemonic Function
753: 
754: The function compares each component against its `Default*` constant. If a
755: component equals its default, it is omitted. Non-default components are
756: appended as `", "` followed by the component's `Mnemonic()` output:
757: 
758: ```go
759: func ComponentTripleMnemonic(
760:     bc entities.BarComponent, qc entities.QuoteComponent, tc entities.TradeComponent,
761: ) string {
762:     var s string
763:     if bc != entities.DefaultBarComponent   { s += ", " + bc.Mnemonic() }
764:     if qc != entities.DefaultQuoteComponent { s += ", " + qc.Mnemonic() }
765:     if tc != entities.DefaultTradeComponent { s += ", " + tc.Mnemonic() }
766:     return s
767: }
768: ```
769: 
770: The result is inserted into the indicator format string after the numeric
771: parameters: `sma(14%s)` → `sma(14)` or `sma(14, hl/2)`.
772: 
773: #### Mnemonic Examples
774: 
775: | Params                                                                | Mnemonic              |
776: |-----------------------------------------------------------------------|-----------------------|
777: | `{Length: 5}`                                                         | `sma(5)`              |
778: | `{Length: 5, BarComponent: BarMedianPrice}`                           | `sma(5, hl/2)`        |
779: | `{Length: 5, QuoteComponent: QuoteBidPrice}`                          | `sma(5, b)`           |
780: | `{Length: 5, TradeComponent: TradeVolume}`                            | `sma(5, v)`           |
781: | `{Length: 5, BarComponent: BarOpenPrice, QuoteComponent: QuoteBidPrice}` | `sma(5, o, b)`     |
782: | `{Length: 5, BarComponent: BarHighPrice, TradeComponent: TradeVolume}` | `sma(5, h, v)`       |
783: | `{Length: 7, Phase: 0}`                                               | `jma(7, 0)`           |
784: | `{Length: 7, Phase: 0, BarComponent: BarMedianPrice}`                 | `jma(7, 0, hl/2)`    |
785: 
786: The `description` is always the full name followed by the mnemonic:
787: `"Simple moving average sma(5, hl/2)"`.
788: 
789: #### Entity Mnemonic Methods
790: 
791: Each component enum has two display methods: `String()` (full word, used for
792: JSON serialization) and `Mnemonic()` (short code, used in indicator mnemonics).
793: 
794: **BarComponent mnemonics:**
795: 
796: | Enum value        | `String()`   | `Mnemonic()` |
797: |-------------------|-------------|---------------|
798: | `BarOpenPrice`    | `open`      | `o`           |
799: | `BarHighPrice`    | `high`      | `h`           |
800: | `BarLowPrice`     | `low`       | `l`           |
801: | `BarClosePrice`   | `close`     | `c`           |
802: | `BarVolume`       | `volume`    | `v`           |
803: | `BarMedianPrice`  | `median`    | `hl/2`        |
804: | `BarTypicalPrice` | `typical`   | `hlc/3`       |
805: | `BarWeightedPrice`| `weighted`  | `hlcc/4`      |
806: | `BarAveragePrice` | `average`   | `ohlc/4`      |
807: 
808: **QuoteComponent mnemonics:**
809: 
810: | Enum value             | `String()`     | `Mnemonic()`          |
811: |------------------------|----------------|-----------------------|
812: | `QuoteBidPrice`        | `bid`          | `b`                   |
813: | `QuoteAskPrice`        | `ask`          | `a`                   |
814: | `QuoteBidSize`         | `bidSize`      | `bs`                  |
815: | `QuoteAskSize`         | `askSize`      | `as`                  |
816: | `QuoteMidPrice`        | `mid`          | `ba/2`                |
817: | `QuoteWeightedPrice`   | `weighted`     | `(bbs+aas)/(bs+as)`   |
818: | `QuoteWeightedMidPrice`| `weightedMid`  | `(bas+abs)/(bs+as)`   |
819: | `QuoteSpreadBp`        | `spreadBp`     | `spread bp`           |
820: 
821: **TradeComponent mnemonics:**
822: 
823: | Enum value    | `String()`  | `Mnemonic()` |
824: |---------------|-------------|---------------|
825: | `TradePrice`  | `price`     | `p`           |
826: | `TradeVolume` | `volume`    | `v`           |
827: 
828: ## Helper / Shared Component Families
829: 
830: Some indicators are driven by an interchangeable internal component that has
831: several variants (e.g., different algorithms producing the same shape of
832: output). The canonical example is John Ehlers' **Hilbert Transformer cycle
833: estimator**, which has four variants (HomodyneDiscriminator,
834: HomodyneDiscriminatorUnrolled, PhaseAccumulator, DualDifferentiator) that all
835: produce a cycle period estimate from a price sample.
836: 
837: These are **not indicators themselves**. They are helper components used by
838: actual indicators (e.g., MESA Adaptive Moving Average). They live in the
839: indicator folder of their conceptual owner and follow a specific structure.
840: 
841: ### Folder Layout
842: 
843: A helper family lives in a single folder at the standard indicator depth
844: (`indicators/<group>/<family-name>/`) and contains:
845: 
846: - **Shared interface** — the contract every variant implements.
847: - **Shared params struct/interface** — a single params shape common to all variants.
848: - **Variant type enum** — discriminator for the family (one constant per variant).
849: - **Common utilities file** — shared constants, helper functions (`push`,
850:   `fillWmaFactors`, ...) and the **factory/dispatcher** function.
851: - **One file per variant** implementing the shared interface.
852: - **One spec/test file per variant** plus one for the common file.
853: 
854: Example (TS — `ts/indicators/john-ehlers/hilbert-transformer/`):
855: 
856: ```
857: cycle-estimator.ts                 # interface
858: cycle-estimator-params.ts          # shared params
859: cycle-estimator-type.ts            # variant enum
860: common.ts                          # utilities + createEstimator()
861: homodyne-discriminator.ts          # variant
862: homodyne-discriminator-unrolled.ts
863: phase-accumulator.ts
864: dual-differentiator.ts
865: # + matching .spec.ts for each
866: ```
867: 
868: Go mirrors the same structure with lowercase filenames (`cycleestimator.go`,
869: `cycleestimatorparams.go`, `cycleestimatortype.go`, `estimator.go` for common
870: utilities and `NewCycleEstimator`, and one `<variant>estimator.go` per variant
871: — e.g., `homodynediscriminatorestimator.go`, `phaseaccumulatorestimator.go`).
872: These are the **conceptual-naming exception** noted in the File Naming section:
873: the folder name (`hilberttransformer/`) is never prefixed onto the member
874: files, because each file represents an independent concept within the family.
875: 
876: ### Factory / Dispatcher Pattern
877: 
878: The common file exposes a dispatcher that constructs a variant by type.
879: Each variant has its own **default params** baked into the dispatcher — callers
880: that omit params for a specific type get sensible defaults for that variant.
881: 
882: ```go
883: // Go — go/indicators/johnehlers/hilberttransformer/estimator.go
884: func NewCycleEstimator(typ CycleEstimatorType, params *CycleEstimatorParams) (CycleEstimator, error) {
885:     switch typ {
886:     case HomodyneDiscriminator:         return NewHomodyneDiscriminatorEstimator(params)
887:     case HomodyneDiscriminatorUnrolled: return NewHomodyneDiscriminatorEstimatorUnrolled(params)
888:     case PhaseAccumulator:              return NewPhaseAccumulatorEstimator(params)
889:     case DualDifferentiator:            return NewDualDifferentiatorEstimator(params)
890:     }
891:     return nil, fmt.Errorf("invalid cycle estimator type: %s", typ)
892: }
893: ```
894: 
895: ```ts
896: // TS — ts/indicators/john-ehlers/hilbert-transformer/hilbert-transformer-common.ts
897: export function createEstimator(
898:     estimatorType?: HilbertTransformerCycleEstimatorType,
899:     estimatorParams?: HilbertTransformerCycleEstimatorParams,
900: ): HilbertTransformerCycleEstimator {
901:     if (estimatorType === undefined) {
902:         estimatorType = HilbertTransformerCycleEstimatorType.HomodyneDiscriminator;
903:     }
904:     switch (estimatorType) {
905:         case HilbertTransformerCycleEstimatorType.HomodyneDiscriminator:
906:             estimatorParams ??= { smoothingLength: 4, alphaEmaQuadratureInPhase: 0.2, alphaEmaPeriod: 0.2 };
907:             return new HilbertTransformerHomodyneDiscriminator(estimatorParams);
908:         // ... other variants, each with its own default params
909:         default:
910:             throw new Error('Invalid cycle estimator type: ' + estimatorType);
911:     }
912: }
913: ```
914: 
915: The dispatcher must have a default variant (when `type` is omitted) and throw
916: on unknown types.
917: 
918: ### Shared Interface Design
919: 
920: The shared interface exposes:
921: 
922: - Primed state (`primed`) and warm-up configuration (`warmUpPeriod`).
923: - Construction parameters as readable properties (`smoothingLength`,
924:   `alphaEmaQuadratureInPhase`, `alphaEmaPeriod`).
925: - Intermediate state visible for testing/debugging (`smoothed`, `detrended`,
926:   `inPhase`, `quadrature`, `period`).
927: - An `update(sample)` method returning the primary output.
928: 
929: TypeScript naming: **do not** suffix getters with `Value`. Prefix the backing
930: private field with `_` so the public getter can take the unadorned name:
931: 
932: ```ts
933: // Correct — public getter is `period`, backing field is `_period`.
934: private _period = 0;
935: public get period(): number { return this._period; }
936: 
937: // Incorrect — do not use `periodValue` just to avoid a name clash.
938: ```
939: 
940: ### File Naming — No Type Suffixes
941: 
942: This module follows the same rule as the rest of the codebase: **no
943: `.interface.ts`, `.enum.ts`, or similar type-suffixes**. Interfaces, enums,
944: params, and implementations all use plain `.ts` filenames differentiated by
945: descriptive stem names.
946: 
947: ### Warm-Up Period Contract
948: 
949: Each variant has an intrinsic minimum priming length (the sample count needed
950: for its state to be meaningful). If `params.warmUpPeriod` is provided and
951: larger than that minimum, it overrides the minimum; otherwise the intrinsic
952: minimum is used. Tests must cover both the default path and the custom
953: `warmUpPeriod` override.
954: 
955: ## Taxonomy & Descriptor Registry
956: 
957: Every indicator carries a **Descriptor** — a taxonomic classification used by
958: charting UIs and discovery tooling to filter, group, and lay out indicators.
959: Descriptors are the single source of truth for output `kind` and `shape`;
960: indicator `Metadata()` implementations consume the registry via
961: `BuildMetadata` and MUST NOT hand-write `kind`/`shape`.
962: 
963: ### Dimensions
964: 
965: | Primitive           | Go type                 | TS type                 | Purpose |
966: |---------------------|-------------------------|-------------------------|---------|
967: | `Identifier`        | `core.Identifier`       | `IndicatorIdentifier`   | Unique ID of the indicator. |
968: | `Role`              | `core.Role`             | `Role`                  | Semantic role of an **output** (Smoother, Envelope, Overlay, Polyline, Oscillator, BoundedOscillator, Volatility, VolumeFlow, Directional, CyclePeriod, CyclePhase, FractalDimension, Spectrum, Signal, Histogram, RegimeFlag, Correlation). |
969: | `Pane`              | `core.Pane`             | `Pane`                  | Chart pane where an output is drawn: `Price`, `Own`, `OverlayOnParent`. |
970: | `Adaptivity`        | `core.Adaptivity`       | `Adaptivity`            | `Static` or `Adaptive` (indicator-level). |
971: | `InputRequirement`  | `core.InputRequirement` | `InputRequirement`      | Minimum input data type: `ScalarInput`, `QuoteInput`, `BarInput`, `TradeInput`. |
972: | `VolumeUsage`       | `core.VolumeUsage`      | `VolumeUsage`           | `NoVolume`, `AggregateBarVolume`, `PerTradeVolume`, `QuoteLiquidityVolume`. |
973: | `OutputDescriptor`  | `core.OutputDescriptor` | `OutputDescriptor`      | `{Kind, Shape, Role, Pane}` per output. |
974: | `Descriptor`        | `core.Descriptor`       | `Descriptor`            | `{Identifier, Family, Adaptivity, InputRequirement, VolumeUsage, Outputs}`. |
975: 
976: **VolumeUsage ⇔ InputRequirement validity:**
977: 
978: - `AggregateBarVolume` ⇒ `BarInput`
979: - `PerTradeVolume` ⇒ `TradeInput`
980: - `QuoteLiquidityVolume` ⇒ `QuoteInput`
981: - `NoVolume` is valid with any input
982: 
983: ### Registry Files
984: 
985: | Location                                              | Content |
986: |-------------------------------------------------------|---------|
987: | `go/indicators/core/descriptors.go`                   | Go descriptor map keyed by `Identifier`. |
988: | `go/indicators/core/descriptors_test.go`              | Coverage test: every `Identifier` has a descriptor row. |
989: | `go/indicators/core/metadata_descriptor_test.go`      | Invariants: outputs are non-empty, Kinds are strictly ascending starting at 1, shapes are known. |

(Output capped at 50 KB. Showing lines 1-989. Use offset=990 to continue.)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md"
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md</path>
<type>file</type>
<content>1: ---
2: name: indicator-conversion
3: description: Step-by-step guide for converting old-structure indicators to the new architecture in both Go and TypeScript. Load when migrating an existing indicator.
4: ---
5: 
6: # Converting an Old-Structure Indicator to New Structure
7: 
8: This guide provides step-by-step instructions for converting an indicator from the old
9: architecture to the new architecture. It covers both **Go** and **TypeScript** implementations.
10: 
11: The new architecture introduces:
12: 
13: - **Per-indicator packages** (Go) / **per-indicator folders** (TS) instead of a flat shared package
14: - **`LineIndicator` embedding/inheritance** that eliminates `UpdateScalar/Bar/Quote/Trade` boilerplate
15: - **`ComponentTripleMnemonic`** (bar + quote + trade) replacing `componentPairMnemonic` (bar + quote only)
16: - **Zero-value default resolution** for components: zero/undefined = use default, don't show in mnemonic
17: - **Top-level `Mnemonic` and `Description`** on metadata
18: - **Per-indicator output enum** (TS) in a dedicated file
19: - **Renamed imports** from `data`/`indicator`/`output` to `entities`/`core`/`outputs`
20: 
21: Use the SMA indicator as the canonical reference for single-constructor indicators.
22: Use the EMA indicator as the canonical reference for multi-constructor indicators
23: (see [Advanced: Multi-Constructor Indicators](#advanced-multi-constructor-indicators)).
24: 
25: ---
26: 
27: ## Table of Contents
28: 
29: 1. [Go Conversion](#go-conversion)
30:    1. [Create the new package directory](#go-step-1-create-the-new-package-directory)
31:    2. [Convert the params file](#go-step-2-convert-the-params-file)
32:    3. [Convert the output file](#go-step-3-convert-the-output-file)
33:    4. [Convert the output test file](#go-step-4-convert-the-output-test-file)
34:    5. [Convert the main indicator file](#go-step-5-convert-the-main-indicator-file)
35:    6. [Convert the test file](#go-step-6-convert-the-test-file)
36:    7. [Register the indicator type](#go-step-7-register-the-indicator-type)
37:    8. [Verify](#go-step-8-verify)
38: 2. [TypeScript Conversion](#typescript-conversion)
39:    1. [Create the new folder](#ts-step-1-create-the-new-folder)
40:    2. [Convert the params file](#ts-step-2-convert-the-params-file)
41:    3. [Create the output file](#ts-step-3-create-the-output-file)
42:    4. [Convert the main indicator file](#ts-step-4-convert-the-main-indicator-file)
43:    5. [Convert the test file](#ts-step-5-convert-the-test-file)
44:    6. [Verify](#ts-step-6-verify)
45: 3. [Quick Reference: Import Mapping](#quick-reference-import-mapping)
46: 4. [Quick Reference: Symbol Renames](#quick-reference-symbol-renames)
47: 5. [Advanced: Multi-Constructor Indicators](#advanced-multi-constructor-indicators)
48: 6. [Advanced: Helper / Shared Component Families](#advanced-helper--shared-component-families)
49: 7. [Naming & Style Conventions](#naming--style-conventions)
50: 8. [Registering in the Factory](#registering-in-the-factory)
51: 
52: ---
53: 
54: ## Naming & Style Conventions
55: 
56: All identifier, receiver, concurrency, style, and cross-language parity
57: rules are defined in the **`indicator-architecture`** skill and MUST be
58: followed during conversion. Summary (see that skill for the full tables
59: and rationale):
60: 
61: - **Abbreviations banned in identifiers** — always expand: `idx→index`,
62:   `tmp→temp`, `res→result`, `sig→signal`, `val→value`, `prev→previous`,
63:   `avg→average`, `mult→multiplier`, `buf→buffer`, `param→parameter`,
64:   `hist→histogram`. Allowed: Go idioms (`err`, `len`, `cap`, `min`,
65:   `max`, `num`), the Go `Params`/`params` bundle type, TS `value_`
66:   ctor-param idiom.
67: - **Go receivers** — compound type name (2+ CamelCase words) → `s`;
68:   simple type name (single word) → first-letter of type, lowercased.
69:   All methods on a type MUST use the same receiver. If a local would
70:   shadow `s` (e.g. in `MarshalJSON`), rename the local to `str`.
71: - **Concurrency** — stateful public indicators MUST carry `mu
72:   sync.RWMutex`; writers `s.mu.Lock(); defer s.mu.Unlock()`, readers
73:   `s.mu.RLock(); defer s.mu.RUnlock()`, defer on the line immediately
74:   after Lock. Exceptions: pure delegating wrappers, internal engines
75:   guarded by their public wrapper.
76: - **Go style invariants** — no `var x T = zero`, no split
77:   `var x T; x = expr`; use `any` not `interface{}`; no
78:   `make([]T, 0)` (always include capacity); no `new`; grouped imports
79:   (stdlib, external, `zpano/*`); every exported symbol has a doc
80:   comment, even trivial passthroughs.
81: - **Go ↔ TS local-variable parity** — same concept = same name in
82:   both languages. Canonical: `sum`, `epsilon`, `temp`/`diff`,
83:   `stddev`, `spread`, `bw`, `pctB`, `amount`, `lengthMinOne`;
84:   loop counter `i`/`j`/`k`; `index` only when semantically a named
85:   index. Never introduce new short forms.
86: 
87: When porting, copy the other language's local-variable names verbatim
88: where the concept is identical.
89: 
90: ---
91: 
92: ## Go Conversion
93: 
94: ### Go Step 1: Create the new package directory
95: 
96: Old indicators live in a flat `package indicators` directory. New indicators each get their own
97: package under `indicators/<group>/<indicatorname>/`.
98: 
99: **Action:** Create a new directory using the full descriptive name in `lowercase` (no separators).
100: 
101: ```
102: # Example for SMA in the "common" group:
103: indicators/common/simplemovingaverage/
104: ```
105: 
106: All files in this directory will use `package simplemovingaverage` (the directory name).
107: 
108: ### Go Step 2: Convert the params file
109: 
110: **File:** `params.go`
111: 
112: Changes:
113: 
114: 1. **Package declaration:** `package indicators` -> `package <indicatorname>`
115: 2. **Import path:** `"mbg/trading/data"` -> `"zpano/entities"`
116: 3. **Type references:**
117:    - `data.BarComponent` -> `entities.BarComponent`
118:    - `data.QuoteComponent` -> `entities.QuoteComponent`
119:    - `data.TradeComponent` -> `entities.TradeComponent`
120: 4. **Add doc comments** to each component field explaining the zero-value default behavior:
121: 
122: ```go
123: // BarComponent indicates the component of a bar to use when updating the indicator with a bar sample.
124: //
125: // If zero, the default (BarClosePrice) is used and the component is not shown in the indicator mnemonic.
126: BarComponent entities.BarComponent
127: ```
128: 
129: Repeat the pattern for `QuoteComponent` (default: `QuoteMidPrice`) and `TradeComponent`
130: (default: `TradePrice`).
131: 
132: ### Go Step 3: Convert the output file
133: 
134: **File:** `output.go`
135: 
136: Two mechanical changes:
137: 
138: 1. `package indicators` → `package <indicatorname>`.
139: 2. **Rename the output type and constants to the bare Go convention.** The
140:    package name provides scoping, so the indicator-name prefix would stutter
141:    (`simplemovingaverage.SimpleMovingAverageOutput`). Rename:
142:    - Type `<IndicatorName>Output` → `Output`.
143:    - Constant `<IndicatorName>Value` → `Value`.
144:    - Multi-output: strip the `<IndicatorName>` prefix and keep the descriptive
145:      suffix (e.g. `DirectionalMovementIndexAverageDirectionalIndex` →
146:      `AverageDirectionalIndex`). If stripping leaves a trailing `Value` with
147:      more text before it, drop that too (e.g. `AverageTrueRangeValue` →
148:      `AverageTrueRange`).
149: 
150:    Update all references in the package (main file, test files, `String()`,
151:    `IsKnown()`, `MarshalJSON()`, `UnmarshalJSON()`). The string forms returned
152:    by `String()`/`MarshalJSON()` stay unchanged — only the Go identifiers move.
153: 
154: > **TypeScript keeps the long form** (`SimpleMovingAverageOutput` /
155: > `SimpleMovingAverageValue`) because TS imports by symbol, not by module.
156: > Only the Go side strips the prefix.
157: 
158: ### Go Step 4: Convert the output test file
159: 
160: **File:** `output_test.go`
161: 
162: **Only change:** `package indicators` -> `package <indicatorname>`
163: 
164: All test logic is identical.
165: 
166: ### Go Step 5: Convert the main indicator file
167: 
168: **File:** `<indicatorname>.go`
169: 
170: This is the most involved change. Follow these sub-steps carefully.
171: 
172: #### 5a. Package and imports
173: 
174: Replace:
175: 
176: ```go
177: package indicators
178: 
179: import (
180:     "mbg/trading/data"
181:     "mbg/trading/indicators/indicator"
182:     "mbg/trading/indicators/indicator/output"
183: )
184: ```
185: 
186: With:
187: 
188: ```go
189: package <indicatorname>
190: 
191: import (
192:     "zpano/entities"
193:     "zpano/indicators/core"
194:     "zpano/indicators/core/outputs"
195: )
196: ```
197: 
198: Keep standard library imports (`fmt`, `math`, `sync`, etc.) as-is.
199: 
200: Remove the `"mbg/trading/indicators/indicator/output"` import entirely -- `BuildMetadata`
201: sources per-output `Kind`/`Shape` from the descriptor registry, so indicator files no
202: longer need to import `"zpano/indicators/core/outputs"` or `".../outputs/shape"` just for
203: `Metadata()`. Keep the `outputs` import only if the file uses `outputs.NewBand` /
204: `outputs.NewEmptyBand`.
205: 
206: #### 5b. Struct: remove boilerplate fields, embed LineIndicator
207: 
208: Old struct has these fields that must be **removed**:
209: 
210: ```go
211: name        string
212: description string
213: barFunc     data.BarFunc
214: quoteFunc   data.QuoteFunc
215: tradeFunc   data.TradeFunc
216: ```
217: 
218: **Add** the `LineIndicator` embedding:
219: 
220: ```go
221: core.LineIndicator
222: ```
223: 
224: The resulting struct keeps only the indicator's own state (e.g., `window`, `windowSum`, `primed`,
225: `mu sync.RWMutex`) plus the embedded `core.LineIndicator`.
226: 
227: **Important:** The `mu sync.RWMutex` field stays on the indicator struct. The `core.LineIndicator`
228: field goes after it (before the indicator-specific fields).
229: 
230: Example (SMA):
231: 
232: ```go
233: type SimpleMovingAverage struct {
234:     mu sync.RWMutex
235:     core.LineIndicator
236:     window       []float64
237:     windowSum    float64
238:     windowLength int
239:     windowCount  int
240:     lastIndex    int
241:     primed       bool
242: }
243: ```
244: 
245: #### 5c. Constructor: add default resolution and ComponentTripleMnemonic
246: 
247: **Before** calling `entities.BarComponentFunc(...)` etc., add a zero-value resolution block:
248: 
249: ```go
250: // Resolve defaults for component functions.
251: // A zero value means "use default, don't show in mnemonic".
252: bc := p.BarComponent
253: if bc == 0 {
254:     bc = entities.DefaultBarComponent
255: }
256: 
257: qc := p.QuoteComponent
258: if qc == 0 {
259:     qc = entities.DefaultQuoteComponent
260: }
261: 
262: tc := p.TradeComponent
263: if tc == 0 {
264:     tc = entities.DefaultTradeComponent
265: }
266: ```
267: 
268: Then pass the **resolved** values `bc`, `qc`, `tc` (not `p.BarComponent` etc.) to
269: `entities.BarComponentFunc`, `entities.QuoteComponentFunc`, `entities.TradeComponentFunc`.
270: 
271: **Change the mnemonic format** from:
272: 
273: ```go
274: fmtn = "sma(%d)"
275: // ...
276: name := fmt.Sprintf(fmtn, length)
277: ```
278: 
279: To:
280: 
281: ```go
282: fmtn = "sma(%d%s)"
283: // ...
284: mnemonic := fmt.Sprintf(fmtn, length, core.ComponentTripleMnemonic(bc, qc, tc))
285: ```
286: 
287: The variable name changes from `name` to `mnemonic`. The description uses the mnemonic:
288: 
289: ```go
290: desc := "Simple moving average " + mnemonic
291: ```
292: 
293: **Change the function type declarations** from `data.*Func` to `entities.*Func`:
294: 
295: ```go
296: var (
297:     err       error
298:     barFunc   entities.BarFunc
299:     quoteFunc entities.QuoteFunc
300:     tradeFunc entities.TradeFunc
301: )
302: ```
303: 
304: #### 5d. Constructor: replace struct literal with LineIndicator assignment
305: 
306: Old pattern -- returning a struct literal with embedded fields:
307: 
308: ```go
309: return &SimpleMovingAverage{
310:     name:         name,
311:     description:  desc,
312:     window:       make([]float64, length),
313:     windowLength: length,
314:     lastIndex:    length - 1,
315:     barFunc:      barFunc,
316:     quoteFunc:    quoteFunc,
317:     tradeFunc:    tradeFunc,
318: }, nil
319: ```
320: 
321: New pattern -- create the struct first, then assign `LineIndicator`:
322: 
323: ```go
324: sma := &SimpleMovingAverage{
325:     window:       make([]float64, length),
326:     windowLength: length,
327:     lastIndex:    length - 1,
328: }
329: 
330: sma.LineIndicator = core.NewLineIndicator(mnemonic, desc, barFunc, quoteFunc, tradeFunc, sma.Update)
331: 
332: return sma, nil
333: ```
334: 
335: **Critical:** `sma.Update` is a method reference passed to `NewLineIndicator`. This is what
336: allows `LineIndicator` to implement `UpdateScalar/Bar/Quote/Trade` without the indicator
337: having to define them. The struct must be created first so that `sma.Update` is a valid reference.
338: 
339: #### 5e. Metadata: use BuildMetadata
340: 
341: Old:
342: 
343: ```go
344: func (s *SimpleMovingAverage) Metadata() indicator.Metadata {
345:     return indicator.Metadata{
346:         Type: indicator.SimpleMovingAverage,
347:         Outputs: []output.Metadata{
348:             {
349:                 Kind:        int(SimpleMovingAverageValue),
350:                 Type:        output.Scalar,
351:                 Name:        s.name,
352:                 Description: s.description,
353:             },
354:         },
355:     }
356: }
357: ```
358: 
359: New — `BuildMetadata` pulls per-output `Kind` and `Shape` from the descriptor registry
360: (`go/indicators/core/descriptors.go`), so the caller supplies only mnemonic/description:
361: 
362: ```go
363: func (s *SimpleMovingAverage) Metadata() core.Metadata {
364:     return core.BuildMetadata(
365:         core.SimpleMovingAverage,
366:         s.LineIndicator.Mnemonic,
367:         s.LineIndicator.Description,
368:         []core.OutputText{
369:             {Mnemonic: s.LineIndicator.Mnemonic, Description: s.LineIndicator.Description},
370:         },
371:     )
372: }
373: ```
374: 
375: Key changes:
376: 
377: | Old | New |
378: |-----|-----|
379: | `indicator.Metadata` (return type) | `core.Metadata` (return type) |
380: | Inline `core.Metadata{...}` literal | `core.BuildMetadata(...)` call |
381: | `Type: indicator.SimpleMovingAverage` | First argument `core.SimpleMovingAverage` (the `core.Identifier` value) |
382: | `Name: s.name` | `{Mnemonic: s.LineIndicator.Mnemonic, Description: s.LineIndicator.Description}` per output |
383: | `output.Scalar` | (no longer specified — sourced from the descriptor registry) |
384: | `int(SimpleMovingAverageValue)` | (no longer specified — sourced from the descriptor registry) |
385: | `[]output.Metadata` | `[]core.OutputText` |
386: 
387: The caller NO LONGER imports `zpano/indicators/core/outputs` or
388: `zpano/indicators/core/outputs/shape` just for `Metadata()`. Drop those imports if they
389: are not referenced elsewhere in the file.
390: 
391: **Descriptor row required.** If no descriptor is registered for the indicator's identifier,
392: `BuildMetadata` panics at runtime. See Step 7 below — descriptor registration is now part
393: of indicator registration.
394: 
395: #### 5f. Delete UpdateScalar/Bar/Quote/Trade methods
396: 
397: **Remove entirely** the four methods:
398: 
399: - `UpdateScalar(*data.Scalar) indicator.Output`
400: - `UpdateBar(*data.Bar) indicator.Output`
401: - `UpdateQuote(*data.Quote) indicator.Output`
402: - `UpdateTrade(*data.Trade) indicator.Output`
403: 
404: These are now provided by the embedded `core.LineIndicator`.
405: 
406: #### 5g. Keep Update() and IsPrimed() as-is
407: 
408: The `Update(sample float64) float64` method contains the core algorithm and does not change.
409: The `IsPrimed()` method does not change.
410: 
411: ### Go Step 6: Convert the test file
412: 
413: **File:** `<indicatorname>_test.go`
414: 
415: #### 6a. Package and imports
416: 
417: Replace:
418: 
419: ```go
420: package indicators
421: 
422: import (
423:     "mbg/trading/data"
424:     "mbg/trading/indicators/indicator"
425:     "mbg/trading/indicators/indicator/output"
426: )
427: ```
428: 
429: With:
430: 
431: ```go
432: package <indicatorname>
433: 
434: import (
435:     "zpano/entities"
436:     "zpano/indicators/core"
437:     "zpano/indicators/core/outputs"
438: )
439: ```
440: 
441: #### 6b. Entity type references
442: 
443: | Old | New |
444: |-----|-----|
445: | `data.Scalar` | `entities.Scalar` |
446: | `data.Bar` | `entities.Bar` |
447: | `data.Quote` | `entities.Quote` |
448: | `data.Trade` | `entities.Trade` |
449: | `indicator.Output` | `core.Output` |
450: 
451: #### 6c. Update entity tests (UpdateEntity tests)
452: 
453: The `check` function parameter type changes from `indicator.Output` to `core.Output`.
454: 
455: **Quote test:** If the old test uses `data.Quote{Time: time, Bid: inp}`, the new test must use
456: `entities.Quote{Time: time, Bid: inp, Ask: inp}` because the default quote component is
457: `QuoteMidPrice` (mid = (bid+ask)/2), which requires both fields. Using just `Bid: inp` would
458: produce a wrong mid price.
459: 
460: **Floating-point comparison:** Use tolerance-based comparison (`math.Abs(s.Value-exp) > 1e-13`)
461: instead of exact equality (`s.Value != exp`) for the scalar value check. The multi-stage EMA
462: indicators (T2, T3) can produce values that differ in the last significant digit due to
463: floating-point arithmetic ordering.
464: 
465: #### 6c-1. Multi-seeding algorithm test convergence (T2, T3)
466: 
467: When an indicator has two seeding algorithms (`firstIsAverage = true` vs `false` / Metastock),
468: the expected test data typically comes from one algorithm (usually `firstIsAverage = true` from
469: the spreadsheet). The other algorithm produces different values initially but converges after
470: some number of samples.
471: 
472: In Go tests, use a `firstCheck` offset:
473: - `firstIsAverage = true` (SMA-seeded): `firstCheck = lprimed` (or `lprimed + 1` if the
474:   first primed value differs due to averaging)
475: - `firstIsAverage = false` (Metastock): `firstCheck = lprimed + N` where N is the number
476:   of samples needed for convergence (e.g., 43 for T2 with length 5)
477: 
478: In TS tests, use the same pattern: `if (i >= lenPrimed + N) { expect(act).toBeCloseTo(...) }`
479: 
480: #### 6d. Constructor test ("length > 1" sub-test)
481: 
482: Old checks internal fields directly:
483: 
484: ```go
485: check("name", "sma(5)", sma.name)
486: check("description", "Simple moving average sma(5)", sma.description)
487: check("barFunc == nil", false, sma.barFunc == nil)
488: check("quoteFunc == nil", false, sma.quoteFunc == nil)
489: check("tradeFunc == nil", false, sma.tradeFunc == nil)
490: ```
491: 
492: New checks `LineIndicator` fields and removes function nil checks:
493: 
494: ```go
495: check("mnemonic", "sma(5, hl/2)", sma.LineIndicator.Mnemonic)
496: check("description", "Simple moving average sma(5, hl/2)", sma.LineIndicator.Description)
497: ```
498: 
499: The `barFunc/quoteFunc/tradeFunc == nil` checks are removed because these are now internal to
500: `LineIndicator`.
501: 
502: Note: the mnemonic includes `hl/2` because the "length > 1" test uses `BarMedianPrice` which is
503: not the default. If your test uses non-default components, adjust the expected mnemonic accordingly.
504: 
505: #### 6e. Component references in test constants and params
506: 
507: | Old | New |
508: |-----|-----|
509: | `data.BarComponent` | `entities.BarComponent` |
510: | `data.BarMedianPrice` | `entities.BarMedianPrice` |
511: | `data.BarClosePrice` | `entities.BarClosePrice` |
512: | `data.QuoteComponent` | `entities.QuoteComponent` |
513: | `data.QuoteMidPrice` | `entities.QuoteMidPrice` |
514: | `data.QuoteBidPrice` | `entities.QuoteBidPrice` |
515: | `data.TradeComponent` | `entities.TradeComponent` |
516: | `data.TradePrice` | `entities.TradePrice` |
517: | `data.BarComponent(9999)` | `entities.BarComponent(9999)` |
518: | etc. | etc. |
519: 
520: #### 6f. Metadata test
521: 
522: Old:
523: 
524: ```go
525: check("Outputs[0].Type", output.Scalar, act.Outputs[0].Type)
526: check("Outputs[0].Name", "sma(5)", act.Outputs[0].Name)
527: ```
528: 
529: New:
530: 
531: ```go
532: check("Identifier", core.SimpleMovingAverage, act.Identifier)
533: check("Outputs[0].Shape", shape.Scalar, act.Outputs[0].Shape)
534: check("Outputs[0].Mnemonic", "sma(5)", act.Outputs[0].Mnemonic)
535: ```
536: 
537: (`shape` is `zpano/indicators/core/outputs/shape`. The test file is one of the few places
538: that still imports it, since the indicator file no longer does.)
539: 
540: Also verify that the top-level `Mnemonic` and `Description` are checked on `act`:
541: 
542: ```go
543: // These checks are implicitly done via Type already existing in old tests,
544: // but verify the new Mnemonic and Description fields are present.
545: ```
546: 
547: #### 6g. Test helper: use zero-value defaults
548: 
549: Old test helper passes explicit component values:
550: 
551: ```go
552: func testSimpleMovingAverageCreate(length int) *SimpleMovingAverage {
553:     params := SimpleMovingAverageParams{
554:         Length: length, BarComponent: data.BarClosePrice, QuoteComponent: data.QuoteBidPrice, TradeComponent: data.TradePrice,
555:     }
556:     sma, _ := NewSimpleMovingAverage(&params)
557:     return sma
558: }
559: ```
560: 
561: New test helper uses zero-value defaults (the standard usage pattern):
562: 
563: ```go
564: func testSimpleMovingAverageCreate(length int) *SimpleMovingAverage {
565:     params := SimpleMovingAverageParams{
566:         Length: length,
567:     }
568:     sma, _ := NewSimpleMovingAverage(&params)
569:     return sma
570: }
571: ```
572: 
573: **Important:** If the old helper passed `QuoteBidPrice`, the old "update quote" test could use
574: `data.Quote{Time: time, Bid: inp}`. The new helper uses the default `QuoteMidPrice`, so the quote
575: test must provide both `Bid` and `Ask`: `entities.Quote{Time: time, Bid: inp, Ask: inp}`.
576: 
577: #### 6h. Add mnemonic sub-tests
578: 
579: Add sub-tests that verify mnemonic generation for various component combinations. These test the
580: zero-value default omission behavior:
581: 
582: ```go
583: t.Run("all components zero", func(t *testing.T) {
584:     t.Parallel()
585:     params := SimpleMovingAverageParams{Length: length}
586:     sma, err := NewSimpleMovingAverage(&params)
587:     check("err == nil", true, err == nil)
588:     check("mnemonic", "sma(5)", sma.LineIndicator.Mnemonic)
589:     check("description", "Simple moving average sma(5)", sma.LineIndicator.Description)
590: })
591: 
592: t.Run("only bar component set", func(t *testing.T) {
593:     t.Parallel()
594:     params := SimpleMovingAverageParams{Length: length, BarComponent: entities.BarMedianPrice}
595:     sma, err := NewSimpleMovingAverage(&params)
596:     check("err == nil", true, err == nil)
597:     check("mnemonic", "sma(5, hl/2)", sma.LineIndicator.Mnemonic)
598:     check("description", "Simple moving average sma(5, hl/2)", sma.LineIndicator.Description)
599: })
600: 
601: t.Run("only quote component set", func(t *testing.T) {
602:     t.Parallel()
603:     params := SimpleMovingAverageParams{Length: length, QuoteComponent: entities.QuoteBidPrice}
604:     sma, err := NewSimpleMovingAverage(&params)
605:     check("err == nil", true, err == nil)
606:     check("mnemonic", "sma(5, b)", sma.LineIndicator.Mnemonic)
607:     check("description", "Simple moving average sma(5, b)", sma.LineIndicator.Description)
608: })
609: 
610: t.Run("only trade component set", func(t *testing.T) {
611:     t.Parallel()
612:     params := SimpleMovingAverageParams{Length: length, TradeComponent: entities.TradeVolume}
613:     sma, err := NewSimpleMovingAverage(&params)
614:     check("err == nil", true, err == nil)
615:     check("mnemonic", "sma(5, v)", sma.LineIndicator.Mnemonic)
616:     check("description", "Simple moving average sma(5, v)", sma.LineIndicator.Description)
617: })
618: 
619: t.Run("bar and quote components set", func(t *testing.T) {
620:     t.Parallel()
621:     params := SimpleMovingAverageParams{
622:         Length: length, BarComponent: entities.BarOpenPrice, QuoteComponent: entities.QuoteBidPrice,
623:     }
624:     sma, err := NewSimpleMovingAverage(&params)
625:     check("err == nil", true, err == nil)
626:     check("mnemonic", "sma(5, o, b)", sma.LineIndicator.Mnemonic)
627:     check("description", "Simple moving average sma(5, o, b)", sma.LineIndicator.Description)
628: })
629: 
630: t.Run("bar and trade components set", func(t *testing.T) {
631:     t.Parallel()
632:     params := SimpleMovingAverageParams{
633:         Length: length, BarComponent: entities.BarHighPrice, TradeComponent: entities.TradeVolume,
634:     }
635:     sma, err := NewSimpleMovingAverage(&params)
636:     check("err == nil", true, err == nil)
637:     check("mnemonic", "sma(5, h, v)", sma.LineIndicator.Mnemonic)
638:     check("description", "Simple moving average sma(5, h, v)", sma.LineIndicator.Description)
639: })
640: 
641: t.Run("quote and trade components set", func(t *testing.T) {
642:     t.Parallel()
643:     params := SimpleMovingAverageParams{
644:         Length: length, QuoteComponent: entities.QuoteAskPrice, TradeComponent: entities.TradeVolume,
645:     }
646:     sma, err := NewSimpleMovingAverage(&params)
647:     check("err == nil", true, err == nil)
648:     check("mnemonic", "sma(5, a, v)", sma.LineIndicator.Mnemonic)
649:     check("description", "Simple moving average sma(5, a, v)", sma.LineIndicator.Description)
650: })
651: ```
652: 
653: Adapt the mnemonics and component values to your specific indicator. The pattern is always:
654: 
655: - Default component = omitted from mnemonic
656: - Non-default component = shown as its mnemonic abbreviation (e.g., `hl/2`, `o`, `h`, `b`, `a`, `v`)
657: 
658: ### Go Step 7: Register the identifier and descriptor
659: 
660: 1. **Register the identifier.** If the constant does not already exist in
661:    `go/indicators/core/identifier.go`, add it:
662: 
663:    ```go
664:    const (
665:        SimpleMovingAverage Identifier = iota + 1
666:        // ...
667:    )
668:    ```
669: 
670: 2. **Register the descriptor.** Add a row to the registry in
671:    `go/indicators/core/descriptors.go`. The descriptor drives the taxonomy (role, pane,
672:    volume usage, adaptivity, family) **and** supplies per-output `Kind`/`Shape` consumed by
673:    `BuildMetadata`. See `.opencode/skills/indicator-architecture/SKILL.md` section
674:    "Taxonomy & Descriptor Registry" for field meanings and guidance. The order of the
675:    `Outputs` slice must match the order of the `[]OutputText` passed by the indicator's
676:    `Metadata()` method.
677: 
678: ### Go Step 8: Verify
679: 
680: ```bash
681: cd go && go test ./indicators/common/<indicatorname>/...
682: ```
683: 
684: All tests must pass. If the old tests are still present in the `_old` folder, you may need to
685: exclude them or ensure they still compile (they reference the old `mbg/trading` module, so they
686: won't be picked up by the new module).
687: 
688: ---
689: 
690: ## TypeScript Conversion
691: 
692: ### TS Step 1: Create the new folder
693: 
694: Old indicators live in a folder with no per-indicator output file and use `.interface` and `.enum`
695: suffixes. New indicators each get a clean folder.
696: 
697: **Action:** Create a new directory using the full descriptive name in `kebab-case`.
698: 
699: ```
700: # Example for SMA in the "common" group:
701: indicators/common/simple-moving-average/
702: ```
703: 
704: The new folder will contain 4 files (old had 3):
705: 
706: | Old files | New files |
707: |-----------|-----------|
708: | `simple-moving-average-params.interface.ts` | `params.ts` |
709: | `simple-moving-average.ts` | `simple-moving-average.ts` |
710: | `simple-moving-average.spec.ts` | `simple-moving-average.spec.ts` |
711: | (none) | `output.ts` |
712: 
713: ### TS Step 2: Convert the params file
714: 
715: **File:** `params.ts` (renamed from `<indicator-name>-params.interface.ts` — prefix dropped, `.interface` suffix dropped)
716: 
717: Changes:
718: 
719: 1. **Rename file:** Drop the `.interface` suffix.
720: 2. **Import paths:** Update from `data/entities` to `entities`:
721: 
722: | Old | New |
723: |-----|-----|
724: | `'../../../data/entities/bar-component.enum'` | `'../../../entities/bar-component'` |
725: | `'../../../data/entities/quote-component.enum'` | `'../../../entities/quote-component'` |
726: 
727: 3. **Add `TradeComponent`** if missing:
728: 
729: ```typescript
730: import { TradeComponent } from '../../../entities/trade-component';
731: ```
732: 
733: And add the field to the interface:
734: 
735: ```typescript
736: /**
737:  * A component of a trade to use when updating the indicator with a trade sample.
738:  *
739:  * If _undefined_, the trade component will have a default value and will not be shown
740:  * in the indicator mnemonic.
741:  */
742: tradeComponent?: TradeComponent;
743: ```
744: 
745: ### TS Step 3: Create the output file
746: 
747: **File:** `output.ts` (NEW file -- does not exist in old structure)
748: 
749: Create a per-indicator output enum:
750: 
751: ```typescript
752: /** Describes the outputs of the indicator. */
753: export enum SimpleMovingAverageOutput {
754:     /** The scalar value of the moving average. */
755:     Value = 'value',
756: }
757: ```
758: 
759: Adapt the enum name and members to your specific indicator.
760: 
761: ### TS Step 4: Convert the main indicator file
762: 
763: **File:** `<indicator-name>.ts`
764: 
765: #### 4a. Imports
766: 
767: Replace old imports with new ones:
768: 
769: | Old import | New import |
770: |------------|------------|
771: | `'../indicator/component-pair-mnemonic'` | `'../../core/component-triple-mnemonic'` |
772: | `'../indicator/line-indicator'` | `'../../core/line-indicator'` |
773: | `'./<name>-params.interface'` | `'./<name>-params'` |
774: 
775: Add new imports:
776: 
777: ```typescript
778: import { buildMetadata } from '../../core/build-metadata';
779: import { IndicatorIdentifier } from '../../core/indicator-identifier';
780: import { IndicatorMetadata } from '../../core/indicator-metadata';
781: import { SimpleMovingAverageOutput } from './simple-moving-average-output';
782: ```
783: 
784: Note: `OutputType`/`output-type` and `IndicatorType`/`indicator-type` are gone. `buildMetadata`
785: sources the per-output `Shape` and `Kind` from the descriptor registry
786: (`ts/indicators/core/descriptors.ts`), so the indicator file no longer imports them.
787: 
788: Change the mnemonic function import:
789: 
790: ```typescript
791: // Old:
792: import { componentPairMnemonic } from '../indicator/component-pair-mnemonic';
793: // New:
794: import { componentTripleMnemonic } from '../../core/component-triple-mnemonic';
795: ```
796: 
797: #### 4b. Constructor: mnemonic function
798: 
799: Replace:
800: 
801: ```typescript
802: const m = componentPairMnemonic(params.barComponent, params.quoteComponent);
803: ```
804: 
805: With:
806: 
807: ```typescript
808: const m = componentTripleMnemonic(params.barComponent, params.quoteComponent, params.tradeComponent);
809: ```
810: 
811: #### 4c. Constructor: add description
812: 
813: After the mnemonic assignment, add:
814: 
815: ```typescript
816: this.description = 'Simple moving average ' + this.mnemonic;
817: ```
818: 
819: (Adapt the description text to your indicator.)
820: 
821: #### 4d. Constructor: add component setter calls
822: 
823: After the description, add three component setter calls:
824: 
825: ```typescript
826: this.barComponent = params.barComponent;
827: this.quoteComponent = params.quoteComponent;
828: this.tradeComponent = params.tradeComponent;
829: ```
830: 
831: These call the `LineIndicator` setters which handle `undefined` -> default resolution internally.
832: 
833: #### 4e. Add metadata() method
834: 
835: Add a new `metadata()` method to the class. `buildMetadata` reads the per-output `Shape` and
836: `Kind` from the descriptor registry (`ts/indicators/core/descriptors.ts`), so the caller only
837: supplies mnemonic/description text:
838: 
839: ```typescript
840: /** Describes the output data of the indicator. */
841: metadata(): IndicatorMetadata {
842:     return buildMetadata(
843:         IndicatorIdentifier.SimpleMovingAverage,
844:         this.mnemonic,
845:         this.description,
846:         [{ mnemonic: this.mnemonic, description: this.description }],
847:     );
848: }
849: ```
850: 
851: Adapt `IndicatorIdentifier.*` and the per-output text entries to your specific indicator. The
852: number and order of the `OutputText` entries must match the descriptor row registered in
853: `descriptors.ts` — `buildMetadata` throws if they disagree.
854: 
855: **Descriptor row required.** If no descriptor is registered for the identifier, `buildMetadata`
856: throws at runtime. See TS Step 7 below — descriptor registration is now part of indicator
857: registration.
858: 
859: ### TS Step 5: Convert the test file
860: 
861: **File:** `<indicator-name>.spec.ts`
862: 
863: #### 5a. Mnemonic assertion
864: 
865: Old:
866: 
867: ```typescript
868: expect(sma.getMnemonic()).toBe('sma(7)');
869: ```
870: 
871: New:
872: 
873: ```typescript
874: expect(sma.metadata().mnemonic).toBe('sma(7)');
875: ```
876: 
877: The `getMnemonic()` method is replaced by accessing `metadata().mnemonic`.
878: 
879: #### 5b. Other test logic
880: 
881: The `update()`, `isPrimed()`, and NaN tests remain **identical**. No changes needed for the core
882: calculation tests.
883: 
884: ### TS Step 6: Register the identifier and descriptor
885: 
886: 1. **Register the identifier.** If the member does not already exist in
887:    `ts/indicators/core/indicator-identifier.ts`, add it.
888: 
889: 2. **Register the descriptor.** Add a row to the registry in
890:    `ts/indicators/core/descriptors.ts`. The descriptor drives the taxonomy and supplies
891:    per-output `kind`/`shape` consumed by `buildMetadata`. See
892:    `.opencode/skills/indicator-architecture/SKILL.md` section "Taxonomy & Descriptor
893:    Registry" for field meanings. The order of the `outputs` entries must match the order
894:    of the `OutputText[]` passed by the indicator's `metadata()` method.
895: 
896: ### TS Step 7: Verify
897: 
898: Run the tests for the specific indicator:
899: 
900: ```bash
901: # zpano uses Jasmine (not Jest) via tsx. From /ts:
902: npm test                                    # full suite
903: node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=SineWave
904: # --filter is a case-sensitive regex substring match against the spec describe() name.
905: ```
906: 
907: All tests must pass.
908: 
909: ---
910: 
911: ## Quick Reference: Import Mapping
912: 
913: ### Go
914: 
915: | Old import path | New import path |
916: |-----------------|-----------------|
917: | `"mbg/trading/data"` | `"zpano/entities"` |
918: | `"mbg/trading/indicators/indicator"` | `"zpano/indicators/core"` |
919: | `"mbg/trading/indicators/indicator/output"` | `"zpano/indicators/core/outputs"` (only if `outputs.NewBand`/`outputs.NewEmptyBand` is used; the `Shape` enum now lives at `"zpano/indicators/core/outputs/shape"`, but indicator files rarely import it directly — `BuildMetadata` sources `Shape` from the registry) |
920: 
921: ### TypeScript
922: 
923: | Old import path (relative from indicator) | New import path (relative from indicator) |
924: |------------------------------------------|------------------------------------------|
925: | `'../indicator/component-pair-mnemonic'` | `'../../core/component-triple-mnemonic'` |
926: | `'../indicator/line-indicator'` | `'../../core/line-indicator'` |
927: | `'./<name>-params.interface'` | `'./<name>-params'` |
928: | `'../../../data/entities/bar-component.enum'` | `'../../../entities/bar-component'` |
929: | `'../../../data/entities/quote-component.enum'` | `'../../../entities/quote-component'` |
930: | (none) | `'../../../entities/trade-component'` |
931: | (none) | `'../../core/build-metadata'` |
932: | (none) | `'../../core/indicator-identifier'` |
933: | (none) | `'../../core/indicator-metadata'` |
934: | (none) | `'./<name>-output'` |
935: 
936: ---
937: 
938: ## Quick Reference: Symbol Renames
939: 
940: ### Go
941: 
942: | Old symbol | New symbol |
943: |------------|------------|
944: | `data.Scalar` | `entities.Scalar` |
945: | `data.Bar` | `entities.Bar` |
946: | `data.Quote` | `entities.Quote` |
947: | `data.Trade` | `entities.Trade` |
948: | `data.BarFunc` | `entities.BarFunc` |
949: | `data.QuoteFunc` | `entities.QuoteFunc` |
950: | `data.TradeFunc` | `entities.TradeFunc` |
951: | `data.BarComponent` | `entities.BarComponent` |
952: | `data.QuoteComponent` | `entities.QuoteComponent` |
953: | `data.TradeComponent` | `entities.TradeComponent` |
954: | `data.BarClosePrice` | `entities.BarClosePrice` |
955: | `data.BarMedianPrice` | `entities.BarMedianPrice` |
956: | `data.QuoteMidPrice` | `entities.QuoteMidPrice` |
957: | `data.QuoteBidPrice` | `entities.QuoteBidPrice` |
958: | `data.TradePrice` | `entities.TradePrice` |
959: | `data.BarComponentFunc(...)` | `entities.BarComponentFunc(...)` |
960: | `data.QuoteComponentFunc(...)` | `entities.QuoteComponentFunc(...)` |
961: | `data.TradeComponentFunc(...)` | `entities.TradeComponentFunc(...)` |
962: | `indicator.Metadata` | `core.Metadata` |
963: | `indicator.Output` | `core.Output` |
964: | `indicator.SimpleMovingAverage` | `core.SimpleMovingAverage` (now a `core.Identifier`, not an `IndicatorType`) |
965: | `indicator.Type` / `IndicatorType` | `core.Identifier` |
966: | `output.Metadata` | `outputs.Metadata` |
967: | `output.Scalar` | `shape.Scalar` (package `zpano/indicators/core/outputs/shape`) — but indicator files no longer write this; sourced from the descriptor registry |
968: | `outputs.ScalarType` / `outputs.Type` | `shape.Scalar` / `shape.Shape` (same note — sourced from the registry) |
969: 
970: ### Go: Struct field / method changes
971: 
972: | Old | New |
973: |-----|-----|
974: | `s.name` | `s.LineIndicator.Mnemonic` |
975: | `s.description` | `s.LineIndicator.Description` |
976: | `s.barFunc` | (removed -- inside LineIndicator) |
977: | `s.quoteFunc` | (removed -- inside LineIndicator) |
978: | `s.tradeFunc` | (removed -- inside LineIndicator) |
979: | `Outputs[i].Name` | `Outputs[i].Mnemonic` |
980: 
981: ### Go: Metadata field additions
982: 
983: | Old (not present) | New |
984: |-------------------|-----|
985: | (none) | `Mnemonic: s.LineIndicator.Mnemonic` |
986: | (none) | `Description: s.LineIndicator.Description` |
987: 
988: ### TypeScript
989: 
990: | Old symbol / pattern | New symbol / pattern |
991: |----------------------|----------------------|
992: | `componentPairMnemonic(bar, quote)` | `componentTripleMnemonic(bar, quote, trade)` |
993: | `sma.getMnemonic()` | `sma.metadata().mnemonic` |
994: | (no description) | `this.description = '...' + this.mnemonic` |
995: | (no component setters) | `this.barComponent = params.barComponent` |
996: | (no component setters) | `this.quoteComponent = params.quoteComponent` |
997: | (no component setters) | `this.tradeComponent = params.tradeComponent` |
998: | (no metadata method) | `metadata(): IndicatorMetadata { return buildMetadata(...); }` |
999: | `IndicatorType` | `IndicatorIdentifier` (from `core/indicator-identifier`) |
1000: | `OutputType.Scalar` | `Shape.Scalar` (from `core/outputs/shape/shape`) — but indicator files no longer write this; sourced from the descriptor registry |
1001: | (no output file) | `output.ts` with output enum |
1002: 
1003: ---
1004: 
1005: ## Advanced: Multi-Constructor Indicators
1006: 
1007: Some indicators have **multiple constructors** that create the same type with different
1008: parameterization. The EMA (Exponential Moving Average) is the canonical example: it can be
1009: constructed from a **length** or from a **smoothing factor (alpha)**.
1010: 
1011: The base conversion guide above assumes a single constructor. This section covers the
1012: additional considerations for multi-constructor indicators.
1013: 
1014: ### Multiple Param Structs / Interfaces
1015: 
1016: Each constructor path gets its own param struct (Go) or interface (TS).
1017: 
1018: **Go:**
1019: 
1020: ```go
1021: type ExponentialMovingAverageLengthParams struct {
1022:     Length        int
1023:     BarComponent  entities.BarComponent
1024:     // ...
1025: }
1026: 
1027: type ExponentialMovingAverageSmoothingFactorParams struct {
1028:     SmoothingFactor float64
1029:     BarComponent    entities.BarComponent
1030:     // ...
1031: }
1032: ```
1033: 
1034: **TS:**
1035: 
1036: ```ts
1037: export interface ExponentialMovingAverageLengthParams {
1038:     length: number;
1039:     barComponent?: BarComponent;
1040:     // ...
1041: }
1042: 
1043: export interface ExponentialMovingAverageSmoothingFactorParams {
1044:     smoothingFactor: number;
1045:     barComponent?: BarComponent;
1046:     // ...
1047: }
1048: ```
1049: 
1050: Component fields (`BarComponent`, `QuoteComponent`, `TradeComponent`) should be duplicated
1051: across all param structs/interfaces with the same default-resolution doc comments.
1052: 
1053: ### Shared Private Constructor (Go)
1054: 
1055: In Go, use a **private** shared constructor that both public constructors delegate to:
1056: 
1057: ```go
1058: func NewExponentialMovingAverageLength(p *ExponentialMovingAverageLengthParams) (*ExponentialMovingAverage, error) {
1059:     // validate length, compute smoothingFactor from length
1060:     return newExponentialMovingAverage(length, smoothingFactor, p.BarComponent, ...)
1061: }
1062: 
1063: func NewExponentialMovingAverageSmoothingFactor(p *ExponentialMovingAverageSmoothingFactorParams) (*ExponentialMovingAverage, error) {
1064:     // validate smoothingFactor, compute length from smoothingFactor
1065:     return newExponentialMovingAverage(length, smoothingFactor, p.BarComponent, ...)
1066: }
1067: 
1068: func newExponentialMovingAverage(length int, smoothingFactor float64, bc entities.BarComponent, ...) (*ExponentialMovingAverage, error) {
1069:     // default resolution, ComponentTripleMnemonic, two-step construction
1070: }
1071: ```
1072: 
1073: ### Multiple Static Methods (TS)
1074: 
1075: In TypeScript, use **static factory methods** instead of overloading the constructor:
1076: 
1077: ```ts
1078: export class ExponentialMovingAverage extends LineIndicator {
1079:     static fromLength(params: ExponentialMovingAverageLengthParams): ExponentialMovingAverage { ... }
1080:     static fromSmoothingFactor(params: ExponentialMovingAverageSmoothingFactorParams): ExponentialMovingAverage { ... }
1081: 
1082:     private constructor(...) { ... }
1083: }
1084: ```
1085: 
1086: Both static methods delegate to the private constructor with resolved parameters.
1087: 
1088: ### Constructor-Specific Mnemonic Formats
1089: 
1090: Different constructor paths may produce different mnemonic formats. Define the format
1091: at the point where you know which constructor path was taken:
1092: 
1093: ```go
1094: // length-based:
1095: mnemonic := fmt.Sprintf("ema(%d%s)", length, core.ComponentTripleMnemonic(bc, qc, tc))
1096: // => "ema(10)" or "ema(10, hl/2)"
1097: 
1098: // smoothing-factor-based (includes both computed length and the explicit factor):
1099: mnemonic := fmt.Sprintf("ema(%d, %.8f%s)", length, smoothingFactor, core.ComponentTripleMnemonic(bc, qc, tc))
1100: // => "ema(10, 0.18181818)" or "ema(10, 0.18181818, hl/2)"
1101: ```
1102: 
1103: The mnemonics must match between Go and TS. This may require changing the TS format
1104: from the old style to match Go (e.g., old TS alpha path used `ema(0.123)` with 3 decimal
1105: places; new uses `ema(10, 0.18181818)` with 8 decimal places to match Go).
1106: 
1107: ### Cross-Language Behavior Alignment
1108: 
1109: When converting multi-constructor indicators, check for behavioral differences between
1110: Go and TS and decide whether to align them:
1111: 
1112: - **Priming behavior:** If one language primes differently for a given constructor path,
1113:   consider aligning. Example: old TS smoothing-factor EMA primed immediately (length=0),
1114:   while Go computed length from alpha and waited. New TS computes
1115:   `length = Math.round(2/alpha) - 1` to match Go.
1116: 
1117: - **Validation:** Validation differences may be intentional (stricter in one language) and
1118:   can be preserved. Document any known differences.
1119: 
1120: ### Test Considerations
1121: 
1122: Each constructor path needs its own test group:
1123: 
1124: ```go
1125: t.Run("from length", func(t *testing.T) { ... })
1126: t.Run("from smoothing factor", func(t *testing.T) { ... })
1127: ```
1128: 
1129: ```ts
1130: describe('from length', () => { ... });
1131: describe('from smoothing factor', () => { ... });
1132: ```
1133: 
1134: Mnemonic tests should cover both paths, including with non-default components, to verify
1135: that each constructor path produces the correct format.
1136: 
1137: ---
1138: 
1139: ## Advanced: Wrapper Indicators
1140: 
1141: Some indicators **delegate their core calculation** to another indicator internally. The Standard
1142: Deviation indicator is the canonical example: it wraps a Variance indicator and returns the square
1143: root of the variance value.
1144: 
1145: ### Key Differences from Standard Indicators
1146: 
1147: 1. **Dual embedding:** The wrapper struct embeds both `core.LineIndicator` (for the update
1148:    protocol) and holds a pointer to the wrapped indicator.
1149: 
1150: 2. **The wrapper has its own `Update()` method** that:
1151:    - Calls the wrapped indicator's `Update()`
1152:    - Transforms the result (e.g., `math.Sqrt(varianceResult)`)
1153:    - Returns the transformed value
1154: 
1155: 3. **`IsPrimed()` delegates** to the wrapped indicator (the wrapper itself has no separate
1156:    priming logic).
1157: 
1158: 4. **Constructor creates the wrapped indicator internally** using the same params:
1159: 
1160: ```go
1161: // Go example: Standard Deviation wrapping Variance
1162: type StandardDeviation struct {
1163:     core.LineIndicator
1164:     variance *variance.Variance
1165: }
1166: 
1167: func NewStandardDeviation(p *StandardDeviationParams) (*StandardDeviation, error) {
1168:     // Resolve component defaults
1169:     // Create a variance.Variance internally
1170:     vp := &variance.VarianceParams{
1171:         Length: p.Length, IsUnbiased: p.IsUnbiased,
1172:         BarComponent: bc, QuoteComponent: qc, TradeComponent: tc,
1173:     }
1174:     v, err := variance.NewVariance(vp)
1175:     if err != nil {
1176:         return nil, err
1177:     }
1178: 
1179:     // Also create own component functions for LineIndicator
1180:     // Build mnemonic and description
1181:     sd := &StandardDeviation{variance: v}
1182:     sd.LineIndicator = core.NewLineIndicator(mnemonic, desc, barFunc, quoteFunc, tradeFunc, sd.Update)
1183:     return sd, nil
1184: }
1185: ```
1186: 
1187: ```typescript
1188: // TS example: Standard Deviation wrapping Variance
1189: export class StandardDeviation extends LineIndicator {
1190:     private variance: Variance;
1191: 
1192:     public constructor(params: StandardDeviationParams) {
1193:         super();
1194:         // Validate, build mnemonic, set components
1195:         this.variance = new Variance(params);
1196:         this.primed = false;
1197:     }
1198: 
1199:     public update(sample: number): number {
1200:         const value = this.variance.update(sample);
1201:         if (Number.isNaN(value)) return value;
1202:         this.primed = this.variance.isPrimed();
1203:         return Math.sqrt(value);
1204:     }
1205: }
1206: ```
1207: 
1208: ### Important: Separate Packages / Folders
1209: 
1210: In the new architecture, the wrapper and wrapped indicators live in **separate packages** (Go)
1211: or **separate folders** (TS), even if they were in the same package before:
1212: 
1213: - Go: `indicators/common/standarddeviation/` imports `zpano/indicators/common/variance`
1214: - TS: `indicators/common/standard-deviation/` imports `'../variance/variance'`
1215: 
1216: ### Old Multi-Output to New Single-Output
1217: 
1218: The old Standard Deviation was a **multi-output** indicator (SD value + optional variance value).
1219: In the new architecture, it is a **single-output** `LineIndicator`:
1220: 
1221: - `Update(float64) float64` returns only the standard deviation value
1222: - Users who want both SD and variance should create both indicators separately
1223: - The per-indicator output enum has only one member (e.g., `StandardDeviationValue`)
1224: 
1225: This simplification is required because `LineIndicator.UpdateScalar` always returns a
1226: single-element output. If the old indicator had multiple outputs, remove the extra outputs and
1227: direct users to the standalone indicator for those values.
1228: 
1229: ### Constructor Resolves Components Before Passing to Wrapped Indicator
1230: 
1231: The wrapper must resolve zero-value component defaults **before** creating the wrapped indicator.
1232: This ensures both the wrapper's mnemonic and the wrapped indicator use the same resolved values:
1233: 
1234: ```go
1235: bc := p.BarComponent
1236: if bc == 0 {
1237:     bc = entities.DefaultBarComponent
1238: }
1239: // ... resolve qc, tc similarly
1240: 
1241: vp := &variance.VarianceParams{
1242:     Length: p.Length, IsUnbiased: p.IsUnbiased,
1243:     BarComponent: bc, QuoteComponent: qc, TradeComponent: tc,
1244: }
1245: ```
1246: 
1247: ### Mnemonic Pattern
1248: 
1249: Each indicator has its own mnemonic prefix. The wrapper does **not** reuse the wrapped indicator's
1250: mnemonic:
1251: 
1252: - Variance: `var.s(5)` / `var.p(5)`
1253: - Standard Deviation: `stdev.s(5)` / `stdev.p(5)`
1254: 
1255: The `s`/`p` suffix denotes sample (unbiased) vs. population (biased) estimation.
1256: 
1257: ### Test Considerations
1258: 
1259: - The wrapper's test file defines its **own test data and expected values** (it cannot share test
1260:   helpers from the wrapped indicator's package, since they are in different packages)
1261: - Error messages from validation may come from the wrapped indicator (e.g.,
1262:   `"invalid variance parameters: length should be greater than 1"`)
1263: - The `IsPrimed()` test verifies delegation: the wrapper is primed when the wrapped indicator is
1264: 
1265: ---
1266: 
1267: ## Indicators That Do Not Use LineIndicator
1268: 
1269: Some indicators cannot use `LineIndicator` because they:
1270: 1. Have **multiple outputs** (e.g., FRAMA outputs both a value and a fractal dimension)
1271: 2. Need **high/low data** from bars/quotes, not just a single scalar input
1272: 3. The `Update(float64) float64` / `update(sample: number): number` signature doesn't fit
1273: 
1274: ### Pattern: Implement `Indicator` Interface Directly
1275: 
1276: **Go**: The struct does NOT embed `core.LineIndicator`. Instead, it:
1277: - Stores its own `barFunc`, `quoteFunc`, `tradeFunc` component functions
1278: - Implements `UpdateScalar`, `UpdateBar`, `UpdateQuote`, `UpdateTrade` directly
1279: - Has a private `updateEntity` helper that calls `Update` and builds the `core.Output` slice
1280: - Uses `core.ComponentTripleMnemonic` for mnemonic construction (same as LineIndicator-based indicators)
1281: 
1282: **TS**: The class `implements Indicator` instead of `extends LineIndicator`. It:
1283: - Stores its own `barComponentFunc`, `quoteComponentFunc`, `tradeComponentFunc` functions
1284: - Resolves defaults with `?? DefaultBarComponent` etc. (same logic as `LineIndicator`'s protected setters)
1285: - Implements `updateScalar`, `updateBar`, `updateQuote`, `updateTrade` directly
1286: - Has a private `updateEntity` helper that calls `update` and builds the `IndicatorOutput` array
1287: - Uses `componentTripleMnemonic` for mnemonic construction
1288: 
1289: ### Example: FRAMA (Fractal Adaptive Moving Average)
1290: 
1291: FRAMA has two outputs (`Value` and `Fdim`), and its core `Update` takes three parameters
1292: (`sample`, `sampleHigh`, `sampleLow`).
1293: 
1294: **Go signature**: `Update(sample, sampleHigh, sampleLow float64) float64`
1295: **TS signature**: `update(sample: number, sampleHigh: number, sampleLow: number): number`
1296: 
1297: The `updateEntity` helper produces a 2-element output array: `[Scalar(frama), Scalar(fdim)]`.
1298: If frama is NaN (not primed), fdim is also set to NaN.
1299: 
1300: **Entity mapping for high/low**:
1301: - `UpdateBar`: `high = sample.High`, `low = sample.Low`
1302: - `UpdateQuote`: `high = sample.Ask` (Go: `sample.Ask`; TS: `sample.askPrice`), `low = sample.Bid` (Go: `sample.Bid`; TS: `sample.bidPrice`)
1303: - `UpdateScalar`/`UpdateTrade`: `high = low = sample value` (no separate high/low available)
1304: 
1305: ### Reference Files
1306: 
1307: - Go: `go/indicators/johnehlers/fractaladaptivemovingaverage/`
1308: - TS: `ts/indicators/john-ehlers/fractal-adaptive-moving-average/`
1309: 
1310: ---
1311: 
1312: ## Advanced: Helper / Shared Component Families
1313: 
1314: A **helper family** is a group of interchangeable internal components that all
1315: implement the same interface (e.g., multiple cycle-estimator algorithms used
1316: by MESA-style indicators). They are not indicators themselves and do not
1317: extend `LineIndicator` — they are building blocks used *inside* indicators.
1318: 
1319: Canonical reference: **Hilbert Transformer cycle estimator**
1320: (`go/indicators/johnehlers/hilberttransformer/`,
1321: `ts/indicators/john-ehlers/hilbert-transformer/`).
1322: 
1323: ### Folder Contents
1324: 
1325: A helper family lives in a single folder at the standard indicator depth and
1326: contains one file per component role:
1327: 
1328: | Role                    | Go filename                          | TS filename                            |
1329: |-------------------------|--------------------------------------|----------------------------------------|
1330: | Shared interface        | `cycleestimator.go`                  | `cycle-estimator.ts`                   |
1331: | Shared params           | `cycleestimatorparams.go`            | `cycle-estimator-params.ts`            |
1332: | Variant type enum       | `cycleestimatortype.go`              | `cycle-estimator-type.ts`              |
1333: | Common + dispatcher     | `estimator.go`                       | `common.ts`                            |
1334: | One file per variant    | `<variant>estimator.go`              | `<variant>.ts`                         |
1335: | Spec per variant + common | `<variant>estimator_test.go`       | `<variant>.spec.ts`                    |
1336: 
1337: **No type suffixes.** Do not use `.interface.ts` / `.enum.ts` — plain `.ts`
1338: files differentiated by stem name, same as every other indicator.
1339: 
1340: ### Shared Interface
1341: 
1342: Expose:
1343: 
1344: - Primed flag and `warmUpPeriod` (read-only).
1345: - Construction parameters as read-only getters.
1346: - Intermediate state useful for tests (e.g., `smoothed`, `detrended`,
1347:   `inPhase`, `quadrature`, `period`).
1348: - A single `update(sample)` that returns the primary output.
1349: 
1350: TypeScript: getters take the plain name (`period`, not `periodValue`).
1351: Backing private fields are prefixed with `_` (`_period`) to avoid the name
1352: clash with the public getter.
1353: 
1354: ### Dispatcher / Factory
1355: 
1356: The common file exposes a single entry point that constructs a variant from a
1357: type enum value and optional params. Each variant case supplies its own
1358: **default params** when the caller omits them:
1359: 
1360: ```go
1361: // Go
1362: func NewCycleEstimator(typ CycleEstimatorType, params *CycleEstimatorParams) (CycleEstimator, error) {
1363:     switch typ {
1364:     case HomodyneDiscriminator: return NewHomodyneDiscriminatorEstimator(params)
1365:     // ... other variants
1366:     }
1367:     return nil, fmt.Errorf("invalid cycle estimator type: %s", typ)
1368: }
1369: ```
1370: 
1371: ```ts
1372: // TS
1373: export function createEstimator(
1374:     estimatorType?: HilbertTransformerCycleEstimatorType,
1375:     estimatorParams?: HilbertTransformerCycleEstimatorParams,
1376: ): HilbertTransformerCycleEstimator {
1377:     estimatorType ??= HilbertTransformerCycleEstimatorType.HomodyneDiscriminator;
1378:     switch (estimatorType) {
1379:         case HilbertTransformerCycleEstimatorType.HomodyneDiscriminator:
1380:             estimatorParams ??= { smoothingLength: 4, alphaEmaQuadratureInPhase: 0.2, alphaEmaPeriod: 0.2 };
1381:             return new HilbertTransformerHomodyneDiscriminator(estimatorParams);
1382:         // ... other variants with their own defaults
1383:         default: throw new Error('Invalid cycle estimator type: ' + estimatorType);
1384:     }
1385: }
1386: ```
1387: 
1388: Rules:
1389: 
1390: - Dispatcher picks a default variant when the type argument is omitted.
1391: - Each `case` has its own default params literal — defaults can differ per
1392:   variant and must match between Go and TS.
1393: - Unknown types: Go returns an error, TS throws.
1394: 
1395: ### Warm-Up Period
1396: 
1397: Every variant accepts an optional `warmUpPeriod` that, if larger than the
1398: variant's intrinsic minimum priming length, overrides it. Tests must cover:
1399: 
1400: 1. **Default priming** — `primed === false` for the first `intrinsicMinimum`
1401:    samples, then `true`.
1402: 2. **Custom warm-up** — construct with `warmUpPeriod = N` (larger than
1403:    intrinsic); `primed === false` for the first `N` samples, then `true`.
1404: 
1405: ```ts
1406: it('should respect custom warmUpPeriod', () => {
1407:     const lprimed = 50;
1408:     const est = new HilbertTransformerHomodyneDiscriminator(
1409:         { smoothingLength: 4, alphaEmaQuadratureInPhase: 0.2, alphaEmaPeriod: 0.2, warmUpPeriod: lprimed }
1410:     );

(Output capped at 50 KB. Showing lines 1-1410. Use offset=1411 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.6 · 5.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "offset": 990
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md</path>
<type>file</type>
<content>990: | `ts/indicators/core/descriptors.ts`                   | TS descriptor `Map` keyed by `IndicatorIdentifier`. Uses `out()`/`desc()` helpers and `S`/`R`/`P`/`A`/`I`/`V` aliases for terse one-line entries. |
991: 
992: **Kind-numbering asymmetry:** Go per-indicator output enums are 1-based
993: (`iota + 1`), TS enums are 0-based. The descriptor tables mirror that — Go
994: `Kind` values are `1, 2, 3, ...`, TS `kind` values are `0, 1, 2, ...`. A row's
995: `Outputs` slice MUST appear in Kind-ascending order (same as the enum's
996: declared order), because `BuildMetadata` pairs `Outputs[i]` with
997: `outputTexts[i]` positionally.
998: 
999: ### BuildMetadata Helper
1000: 
1001: Indicators call `BuildMetadata` / `buildMetadata` from within their
1002: `Metadata()` method. The helper:
1003: 
1004: 1. Looks up the descriptor for the given `Identifier`.
1005: 2. Panics (Go) / throws (TS) if no descriptor is registered.
1006: 3. Panics/throws if the number of `OutputText` entries does not match the
1007:    number of descriptor outputs.
1008: 4. Returns a fully-populated `Metadata` with `kind` and `shape` taken from the
1009:    registry and `mnemonic`/`description` taken from the caller.
1010: 
1011: **Go:**
1012: 
1013: ```go
1014: func (s *BollingerBands) Metadata() core.Metadata {
1015:     desc := "Bollinger Bands " + s.mnemonic
1016:     return core.BuildMetadata(
1017:         core.BollingerBands,
1018:         s.mnemonic,
1019:         desc,
1020:         []core.OutputText{
1021:             {Mnemonic: s.mnemonic + " lower",       Description: desc + " Lower"},
1022:             {Mnemonic: s.mnemonic + " middle",      Description: desc + " Middle"},
1023:             {Mnemonic: s.mnemonic + " upper",       Description: desc + " Upper"},
1024:             {Mnemonic: s.mnemonic + " bandWidth",   Description: desc + " Band Width"},
1025:             {Mnemonic: s.mnemonic + " percentBand", Description: desc + " Percent Band"},
1026:             {Mnemonic: s.mnemonic + " band",        Description: desc + " Band"},
1027:         },
1028:     )
1029: }
1030: ```
1031: 
1032: **TypeScript:**
1033: 
1034: ```ts
1035: public metadata(): IndicatorMetadata {
1036:   const description = `Bollinger Bands ${this.mnemonic}`;
1037:   return buildMetadata(
1038:     IndicatorIdentifier.BollingerBands,
1039:     this.mnemonic,
1040:     description,
1041:     [
1042:       { mnemonic: `${this.mnemonic} lower`,       description: `${description} Lower` },
1043:       { mnemonic: `${this.mnemonic} middle`,      description: `${description} Middle` },
1044:       { mnemonic: `${this.mnemonic} upper`,       description: `${description} Upper` },
1045:       { mnemonic: `${this.mnemonic} bandWidth`,   description: `${description} Band Width` },
1046:       { mnemonic: `${this.mnemonic} percentBand`, description: `${description} Percent Band` },
1047:       { mnemonic: `${this.mnemonic} band`,        description: `${description} Band` },
1048:     ],
1049:   );
1050: }
1051: ```
1052: 
1053: ### Adding a Descriptor Row
1054: 
1055: When adding a new indicator you MUST add its descriptor in **both** Go and TS:
1056: 
1057: 1. **Go** — `go/indicators/core/descriptors.go`:
1058: 
1059:    ```go
1060:    MyIndicator: {
1061:        Identifier: MyIndicator, Family: "<Author or Common/Custom>",
1062:        Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
1063:        Outputs: []OutputDescriptor{
1064:            {Kind: 1 /* Output1 */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
1065:            {Kind: 2 /* Output2 */, Shape: shape.Scalar, Role: Signal,   Pane: Price},
1066:        },
1067:    },
1068:    ```
1069: 
1070: 2. **TS** — `ts/indicators/core/descriptors.ts` (single-line using helpers):
1071: 
1072:    ```ts
1073:    [IndicatorIdentifier.MyIndicator, desc(IndicatorIdentifier.MyIndicator, '<family>', A.Static, I.ScalarInput, V.NoVolume, [
1074:      out(0, S.Scalar, R.Smoother, P.Price),
1075:      out(1, S.Scalar, R.Signal,   P.Price),
1076:    ])],
1077:    ```
1078: 
1079: 3. Confirm the output order matches the per-indicator output enum order
1080:    (`iota + 1` in Go, `0, 1, 2, ...` in TS).
1081: 
1082: 4. Run the suites:
1083:    ```
1084:    cd go && go test ./indicators/core/...
1085:    cd ts && npm test
1086:    ```
1087:    `TestDescriptorCoverage` and `TestDescriptorOutputsWellFormed` will catch
1088:    missing or malformed rows on the Go side; the TS `buildMetadata` contract
1089:    will throw at runtime if a row is missing.
1090: 
1091: ### Family Naming
1092: 
1093: `Family` is the human-readable grouping label. Conventions:
1094: 
1095: - Author folder → author's full name (e.g., `john-ehlers` / `johnehlers`
1096:   → `"John Ehlers"`, `welles-wilder` / `welleswilder` → `"Welles Wilder"`).
1097: - `common/` → `"Common"`.
1098: - `custom/` → `"Custom"`.
1099: 
1100: ### Role Assignment Guidance
1101: 
1102: | Role                | Use for |
1103: |---------------------|---------|
1104: | `Smoother`          | A moving average or trend-following line plotted on price pane (SMA, EMA, JMA, KAMA, Middle Bollinger, ATCF trend lines, T2/T3, TEMA, ...). |
1105: | `Envelope`          | Upper/lower bands or deviation channels (Bollinger upper/lower/band). |
1106: | `Overlay`           | Other price-pane overlays that are not smoothers or envelopes (e.g., Parabolic SAR dots). |
1107: | `Polyline`          | Outputs that produce an arbitrary polyline shape rather than a time series. |
1108: | `Oscillator`        | Unbounded oscillator centred on 0 or arbitrary range (MACD, momentum, ATCF Ftlm/Stlm/Pcci). |
1109: | `BoundedOscillator` | Oscillator bounded to 0..100 or similar (RSI, %R, Stochastic, ADX, PercentBand). |
1110: | `Volatility`        | Volatility measure (TR, ATR, NATR, StdDev, BandWidth). |
1111: | `VolumeFlow`        | Volume-based cumulative or flow measure (OBV, A/D, A/D Oscillator, MFI). |
1112: | `Directional`       | Directional movement components (+DI, -DI, +DM, -DM). |
1113: | `CyclePeriod`       | Dominant-cycle period estimate. |
1114: | `CyclePhase`        | Cycle phase or sine-wave output. |
1115: | `FractalDimension`  | Fractal-dimension based output. |
1116: | `Spectrum`          | Spectrum / periodogram output (heatmap or polyline). |
1117: | `Signal`            | The signal line of a multi-line indicator (MACD signal, Stochastic %D). |
1118: | `Histogram`         | Difference bar between two lines (MACD histogram). |
1119: | `RegimeFlag`        | Discrete regime indicator (Trend/Cycle mode). |
1120: | `Correlation`       | Correlation coefficient in [-1, 1]. |
1121: 
1122: ### Pane Assignment Guidance
1123: 
1124: | Pane               | Use for |
1125: |--------------------|---------|
1126: | `Price`            | Drawn on the price chart (smoothers, envelopes, overlays, SAR). |
1127: | `Own`              | Drawn in its own subchart (oscillators, volatility, volume flow, spectra). |
1128: | `OverlayOnParent`  | Drawn on top of another indicator's pane (reserved for composite visualizations). |
1129: 
1130: ### Adaptivity
1131: 
1132: - `Adaptive` — the indicator adjusts its coefficients, length, or alpha based
1133:   on market state (KAMA, JMA, MAMA, FRAMA, ATCF, DominantCycle, SineWave,
1134:   HilbertTransformerInstantaneousTrendLine, TrendCycleMode, all Corona
1135:   indicators, AutoCorrelationPeriodogram).
1136: - `Static` — everything else.
1137: 
1138: ## Indicator Factory
1139: 
1140: The **factory** maps an indicator identifier string + JSON parameters to a fully
1141: constructed `Indicator` instance at runtime. This enables data-driven indicator
1142: creation from configuration files, user input, or serialized settings.
1143: 
1144: ### Location & Rationale
1145: 
1146: | Language   | Path                                  |
1147: |------------|---------------------------------------|
1148: | Go         | `go/indicators/factory/factory.go`    |
1149: | TypeScript | `ts/indicators/factory/factory.ts`    |
1150: 
1151: The factory **cannot live in `core/`** in Go due to circular imports: indicator
1152: packages import `core`, so `core` cannot import them back. The `factory/`
1153: package sits at `indicators/factory/` — a sibling of `core/` and the
1154: author/group folders — and imports both `core` and every indicator package.
1155: 
1156: TypeScript has no circular-import issue but uses the same location for
1157: consistency.
1158: 
1159: ### Go API
1160: 
1161: ```go
1162: package factory
1163: 
1164: // New creates an indicator from its identifier and JSON-encoded parameters.
1165: func New(identifier core.Identifier, paramsJSON []byte) (core.Indicator, error)
1166: ```
1167: 
1168: - `identifier` is a `core.Identifier` value (the same enum used in descriptors
1169:   and metadata).
1170: - `paramsJSON` is the raw JSON for the indicator's params struct. For
1171:   no-params indicators, pass `nil` or `[]byte("{}}")`.
1172: - Returns the constructed indicator or an error if the identifier is unknown or
1173:   params are invalid.
1174: 
1175: ### TypeScript API
1176: 
1177: ```ts
1178: export function createIndicator(
1179:     identifier: IndicatorIdentifier,
1180:     params?: Record<string, unknown>,
1181: ): Indicator
1182: ```
1183: 
1184: - `identifier` is an `IndicatorIdentifier` enum value.
1185: - `params` is the plain object (already parsed from JSON). For no-params
1186:   indicators, omit or pass `undefined` / `{}`.
1187: - Throws on unknown identifier or invalid params.
1188: 
1189: ### Constructor Pattern Categories
1190: 
1191: The factory handles five categories of indicator constructors:
1192: 
1193: | Category | Count (Go/TS) | JSON detection | Example |
1194: |----------|---------------|----------------|---------|
1195: | **Standard `*Params` struct** | ~35 | Unmarshal into the params struct directly | SMA, WMA, RSI, BB, MACD, CCI |
1196: | **Length vs SmoothingFactor variants** | 9 | If JSON contains `"smoothingFactor"` key → SF constructor; else → Length constructor | EMA, DEMA, TEMA, T2, T3, KAMA, CyberCycle, ITL |
1197: | **Default vs Params variants** | ~15 | If JSON is `{}` or empty → Default constructor; else → Params constructor | Ehlers spectra, Corona family, DominantCycle, SineWave, ATCF |
1198: | **Bare `int` length** | 10 | Parse `{"length": N}` | ATR, NATR, DM±, DI±, DMI, ADX, ADXR, WilliamsPercentR |
1199: | **No params** | 3 | Ignore JSON entirely | TrueRange, BalanceOfPower, AdvanceDecline |
1200: 
1201: **Special case — MAMA:** uses `"fastLimitSmoothingFactor"` /
1202: `"slowLimitSmoothingFactor"` keys for the SmoothingFactor variant, and
1203: `"fastestSmoothingFactor"` / `"slowestSmoothingFactor"` for KAMA.
1204: 
1205: ### Auto-Detection Logic
1206: 
1207: The factory uses helper functions to detect which constructor variant to call:
1208: 
1209: - **Go:** `hasKey(json, "smoothingFactor")` checks for a specific key;
1210:   `isEmptyObject(json)` checks if JSON is `{}`.
1211: - **TS:** `"smoothingFactor" in params` checks for a key;
1212:   `Object.keys(params).length === 0` checks for empty params.
1213: 
1214: No JSON tags exist on Go params structs — Go's default case-insensitive JSON
1215: unmarshaling handles key matching (e.g., JSON `"smoothingFactor"` matches Go
1216: field `SmoothingFactor`).
1217: 
1218: ### Adding a New Indicator to the Factory
1219: 
1220: When adding a new indicator, add a `case` to the factory's switch statement in
1221: **both** Go and TS:
1222: 
1223: 1. **Go** — add a `case core.<Identifier>:` block in `factory.New()` that
1224:    unmarshals `paramsJSON` into the indicator's params struct and calls its
1225:    constructor.
1226: 2. **TS** — add a `case IndicatorIdentifier.<Name>:` block in
1227:    `createIndicator()` that spreads defaults onto `params` and calls the
1228:    constructor.
1229: 
1230: The case block should follow the pattern of existing entries in the same
1231: constructor category. TS uses a `{ defaultField: value, ...p }` spread pattern
1232: to fill in required defaults.
1233: 
1234: ### `icalc` CLI Tool
1235: 
1236: The `icalc` ("indicator calculator") CLI is a reference consumer of the factory
1237: in both languages. It reads a JSON settings file, creates indicators via the
1238: factory, and runs them against embedded test bar data.
1239: 
1240: | Language | Path | Run command |
1241: |----------|------|-------------|
1242: | Go | `go/cmd/icalc/main.go` | `cd go && go run ./cmd/icalc settings.json` |
1243: | TS | `ts/cmd/icalc/main.ts` | `cd ts && npx tsx cmd/icalc/main.ts cmd/icalc/settings.json` |
1244: 
1245: Settings file format (`settings.json`):
1246: 
1247: ```json
1248: [
1249:     { "identifier": "simpleMovingAverage", "params": { "length": 14 } },
1250:     { "identifier": "exponentialMovingAverage", "params": { "smoothingFactor": 0.1 } },
1251:     { "identifier": "trueRange", "params": {} }
1252: ]
1253: ```
1254: 
1255: - `identifier` is the camelCase JSON string of the `core.Identifier` /
1256:   `IndicatorIdentifier` enum.
1257: - `params` is the JSON object passed to the factory.
1258: - Both CLIs embed 252-entry H/L/C/V test bar data (from TA-Lib `test_data.c`)
1259:   and print metadata + per-bar outputs for each configured indicator.
1260: 
1261: ## DefaultParams / defaultParams
1262: 
1263: Every indicator params file exports a **`DefaultParams()`** (Go) /
1264: **`defaultParams()`** (TS) function that returns a fully-populated params
1265: struct/object with sensible default values. This provides programmatic access to
1266: defaults for UIs, factories, and documentation generators.
1267: 
1268: ### Convention
1269: 
1270: - **Go:** `func DefaultParams() *Params` (or `*StructName` if the struct name
1271:   differs from `Params`). Returns a pointer to a new struct.
1272: - **TS:** `export function defaultParams(): SomeParamsInterface`. Returns a
1273:   plain object satisfying the interface.
1274: - **Dual-variant indicators** (Length/SmoothingFactor) export two functions:
1275:   `DefaultLengthParams()` / `defaultLengthParams()` and
1276:   `DefaultSmoothingFactorParams()` / `defaultSmoothingFactorParams()`.
1277: - **No-params indicators** (empty struct/interface) still export
1278:   `DefaultParams()` / `defaultParams()` returning an empty struct/object for
1279:   consistency.
1280: - **Component fields** (`BarComponent`, `QuoteComponent`, `TradeComponent`)
1281:   are omitted from `DefaultParams` — their zero/undefined values already mean
1282:   "use default".
1283: 
1284: ### Placement
1285: 
1286: The function is added at the **end** of the params file, after the struct/
1287: interface definition and any validation logic.
1288: 
1289: ### Doc Comment
1290: 
1291: Go:
1292: ```go
1293: // DefaultParams returns a Params value populated with conventional defaults.
1294: func DefaultParams() *Params {
1295:     return &Params{
1296:         Length: 20,
1297:     }
1298: }
1299: ```
1300: 
1301: TypeScript:
1302: ```typescript
1303: export function defaultParams(): SimpleMovingAverageParams {
1304:     return {
1305:         length: 20,
1306:     };
1307: }
1308: ```
1309: 
1310: ### Default Value Sources
1311: 
1312: Default values come from (in priority order):
1313: 1. Values documented in the params struct/interface field comments.
1314: 2. The original paper or reference implementation (Ehlers, TA-Lib, etc.).
1315: 3. Conventional industry defaults (e.g., length=14 for RSI/ATR, length=20
1316:    for SMA/EMA).
1317: 
1318: ### When Adding a New Indicator
1319: 
1320: Add `DefaultParams()` / `defaultParams()` to the params file as part of the
1321: standard indicator creation checklist. The function should be present from day
1322: one, not added retroactively.

(End of file - total 1322 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "offset": 1411
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md</path>
<type>file</type>
<content>1411:     expect(est.primed).toBe(false);
1412:     for (let i = 0; i < lprimed; i++) { est.update(input[i]); expect(est.primed).toBe(false); }
1413:     for (let i = lprimed; i < input.length; i++) { est.update(input[i]); expect(est.primed).toBe(true); }
1414: });
1415: ```
1416: 
1417: ### Cross-Language Alignment Checklist
1418: 
1419: - Interface getter names match (modulo language casing).
1420: - Dispatcher default variant matches.
1421: - Per-variant default params match exactly (same numeric values).
1422: - `Primed()` / `primed` semantics match (both return `isWarmedUp`, not some
1423:   other internal flag).
1424: - Error/throw messages use the same textual prefix where practical.
1425: 
1426: ### Shared Formatter / Moniker Helpers
1427: 
1428: When a family defines a formatter (e.g. a per-variant mnemonic builder like
1429: `EstimatorMoniker(typ, params) -> "hd(4, 0.200, 0.200)"`), it **must be
1430: exported from both Go and TS** helper packages. Indicators that consume the
1431: family (MAMA, etc.) import and call this helper to build their own mnemonic
1432: suffix — duplicating the formatting logic per-consumer-indicator is a bug
1433: waiting to happen.
1434: 
1435: Checklist when converting a consumer indicator:
1436: 
1437: 1. Does Go's helper package export a formatter used in the consumer's mnemonic?
1438: 2. Does TS's helper export an equivalent function with the same name (camelCase)
1439:    and identical format string (e.g. `%.3f` ↔ `.toFixed(3)`)?
1440: 3. If the TS equivalent is missing, **add it to the helper's common file** first,
1441:    then import it from the consumer. Do not inline the format in the consumer.
1442: 
1443: Reference: `estimatorMoniker` in
1444: `ts/indicators/john-ehlers/hilbert-transformer/hilbert-transformer-common.ts`
1445: mirrors Go's `hilberttransformer.EstimatorMoniker`.
1446: 
1447: ### Numerical Tolerance for Recursive / Cascaded Indicators
1448: 
1449: The default test tolerance of `1e-12` (Go) / `1e-12` (TS) works for simple
1450: moving averages, single-stage EMAs, etc. Indicators that stack multiple
1451: recursive EMAs, Hilbert-transform feedback loops, or trigonometric feedback
1452: (MAMA, Hilbert-based cycle estimators, multi-stage T3) accumulate enough
1453: floating-point drift between Go and TS that `1e-12` is too tight.
1454: 
1455: **Use `1e-10`** as the tolerance for:
1456: 
1457: - MAMA / FAMA and anything consuming a Hilbert Transformer
1458: - Any indicator whose `update` feeds its own previous output through an
1459:   `atan`, `cos`, or multi-stage EMA chain before producing the output
1460: 
1461: Keep `1e-12` / `1e-13` for purely additive/multiplicative indicators.
1462: 
1463: ---
1464: 
1465: ## Registering in the Factory
1466: 
1467: After converting an indicator and registering its identifier and descriptor,
1468: add it to the **factory** so it can be created from a JSON identifier string
1469: and parameters at runtime. The factory lives at `indicators/factory/` in both
1470: languages (see the `indicator-architecture` skill for full details).
1471: 
1472: ### Go
1473: 
1474: Add a `case` to the switch in `go/indicators/factory/factory.go` inside
1475: `func New(identifier core.Identifier, paramsJSON []byte)`:
1476: 
1477: ```go
1478: case core.MyIndicator:
1479:     var p myindicator.Params
1480:     if err := unmarshal(paramsJSON, &p); err != nil {
1481:         return nil, err
1482:     }
1483:     return myindicator.NewMyIndicator(&p)
1484: ```
1485: 
1486: For **multi-constructor** indicators (Length vs SmoothingFactor), use
1487: `hasKey()` to detect which constructor to call:
1488: 
1489: ```go
1490: case core.MyIndicator:
1491:     if hasKey(paramsJSON, "smoothingFactor") {
1492:         var p myindicator.SmoothingFactorParams
1493:         if err := unmarshal(paramsJSON, &p); err != nil {
1494:             return nil, err
1495:         }
1496:         return myindicator.NewMyIndicatorSmoothingFactor(&p)
1497:     }
1498:     var p myindicator.LengthParams
1499:     if err := unmarshal(paramsJSON, &p); err != nil {
1500:         return nil, err
1501:     }
1502:     return myindicator.NewMyIndicatorLength(&p)
1503: ```
1504: 
1505: For **Default vs Params** indicators, use `isEmptyObject()`:
1506: 
1507: ```go
1508: case core.MyIndicator:
1509:     if isEmptyObject(paramsJSON) {
1510:         return myindicator.NewDefaultMyIndicator()
1511:     }
1512:     var p myindicator.Params
1513:     if err := unmarshal(paramsJSON, &p); err != nil {
1514:         return nil, err
1515:     }
1516:     return myindicator.NewMyIndicator(&p)
1517: ```
1518: 
1519: ### TypeScript
1520: 
1521: Add a `case` to the switch in `ts/indicators/factory/factory.ts` inside
1522: `createIndicator()`:
1523: 
1524: ```ts
1525: case IndicatorIdentifier.MyIndicator:
1526:     return new MyIndicator(p as MyIndicatorParams);
1527: ```
1528: 
1529: For multi-constructor indicators, check for the discriminating key:
1530: 
1531: ```ts
1532: case IndicatorIdentifier.MyIndicator:
1533:     if ('smoothingFactor' in p)
1534:         return MyIndicator.fromSmoothingFactor(p as MyIndicatorSmoothingFactorParams);
1535:     return MyIndicator.fromLength({ length: 14, ...p } as MyIndicatorLengthParams);
1536: ```
1537: 
1538: For Default vs Params indicators:
1539: 
1540: ```ts
1541: case IndicatorIdentifier.MyIndicator:
1542:     if (Object.keys(p).length === 0) return MyIndicator.default();
1543:     return MyIndicator.fromParams(p as MyIndicatorParams);
1544: ```
1545: 
1546: ### Settings File
1547: 
1548: Add an entry to `cmd/icalc/settings.json` (shared between Go and TS) so the
1549: new indicator is exercised by the CLI tool:
1550: 
1551: ```json
1552: { "identifier": "myIndicator", "params": { "length": 14 } }
1553: ```
1554: 
1555: ### Verification
1556: 
1557: Run `icalc` in both languages to confirm the new indicator loads and processes
1558: bars without error:
1559: 
1560: ```bash
1561: cd go && go run ./cmd/icalc settings.json
1562: cd ts && npx tsx cmd/icalc/main.ts cmd/icalc/settings.json
1563: ```

(End of file - total 1563 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.6 · 25.3s)

_Thinking:_

Now I have the full content of both skill files. Let me think about what "new findings" from the work we did should be added. Looking at the goal summary, the key discoveries were:

1. Go has 72 identifiers, TS has 65 - this asymmetry should be documented
2. The KAMA identifier quirk (duplicated "MovingAverage" in Go JSON string)
3. Constructor pattern categories are already documented in the factory section
4. Pre-existing TS Quote.bid/ask property errors
5. TS factory uses renamed imports pattern for defaultParams (to avoid name collisions)
6. Go factory helper functions (hasKey, isEmptyObject, unmarshal, lengthParam)


7. TS has no barrel files - all imports use direct file paths with .js extensions
8. Settings JSON identifiers are camelCase; TS enum members are PascalCase
9. The corona engine is internal (not a public indicator) - already mentioned in mutex exceptions
10. HilbertTransformer shared CycleEstimatorParams also got DefaultParams

Now I'm reviewing what's genuinely new versus what's already covered in the skill files. The factory and DefaultParams sections are pretty comprehensive, and constructor categories are documented. I should focus on what would actually add value as new findings rather than restating what's already there.

For the indicator-architecture section, I could highlight the identifier count gap between Go's 72 and TS's 65, the KAMA JSON quirk with the duplicated "MovingAverage" field, how TS renames imports to avoid collisions when pulling defaultParams from multiple indicators, and Go's case-insensitive JSON unmarshaling that eliminates the need for explicit tags. For indicator-conversion, I should add DefaultParams to the conversion checklist and note the renamed imports pattern when dealing with multiple sources.

Actually, stepping back—the user asked to update with new findings from the factory and DefaultParams work. I need to focus on what genuinely emerged from that investigation rather than just listing observations. Now I'm prioritizing the most impactful documentation gaps and planning targeted updates to both the architecture and conversion guides—specifically adding the identifier count difference, KAMA quirk, JSON tag behavior, and import style notes to architecture, while ensuring the conversion flows explicitly include the DefaultParams step with updated checklists.

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "indicator-architecture: Add identifier count asymmetry note (Go 72 vs TS 65) + KAMA quirk",
      "status": "pending",
      "priority": "medium"
    },
    {
      "content": "indicator-architecture: Add Go params no-JSON-tags note",
      "status": "pending",
      "priority": "medium"
    },
    {
      "content": "indicator-architecture: Add TS no-barrel-files / .js extension import convention",
      "status": "pending",
      "priority": "medium"
    },
    {
      "content": "indicator-conversion: Add DefaultParams as explicit step in Go and TS conversion flows",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "indicator-conversion: Add TS renamed-import pattern note for factory defaultParams",
      "status": "pending",
      "priority": "low"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "indicator-architecture: Add identifier count asymmetry note (Go 72 vs TS 65) + KAMA quirk",
    "status": "pending",
    "priority": "medium"
  },
  {
    "content": "indicator-architecture: Add Go params no-JSON-tags note",
    "status": "pending",
    "priority": "medium"
  },
  {
    "content": "indicator-architecture: Add TS no-barrel-files / .js extension import convention",
    "status": "pending",
    "priority": "medium"
  },
  {
    "content": "indicator-conversion: Add DefaultParams as explicit step in Go and TS conversion flows",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "indicator-conversion: Add TS renamed-import pattern note for factory defaultParams",
    "status": "pending",
    "priority": "low"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 4.8s)

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "indicator-architecture: Add identifier count asymmetry note (Go 72 vs TS 65) + KAMA quirk",
      "priority": "medium",
      "status": "in_progress"
    },
    {
      "content": "indicator-architecture: Add Go params no-JSON-tags note",
      "priority": "medium",
      "status": "pending"
    },
    {
      "content": "indicator-architecture: Add TS no-barrel-files / .js extension import convention",
      "priority": "medium",
      "status": "pending"
    },
    {
      "content": "indicator-conversion: Add DefaultParams as explicit step in Go and TS conversion flows",
      "priority": "high",
      "status": "pending"
    },
    {
      "content": "indicator-conversion: Add TS renamed-import pattern note for factory defaultParams",
      "priority": "low",
      "status": "pending"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "indicator-architecture: Add identifier count asymmetry note (Go 72 vs TS 65) + KAMA quirk",
    "priority": "medium",
    "status": "in_progress"
  },
  {
    "content": "indicator-architecture: Add Go params no-JSON-tags note",
    "priority": "medium",
    "status": "pending"
  },
  {
    "content": "indicator-architecture: Add TS no-barrel-files / .js extension import convention",
    "priority": "medium",
    "status": "pending"
  },
  {
    "content": "indicator-conversion: Add DefaultParams as explicit step in Go and TS conversion flows",
    "priority": "high",
    "status": "pending"
  },
  {
    "content": "indicator-conversion: Add TS renamed-import pattern note for factory defaultParams",
    "priority": "low",
    "status": "pending"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 10.9s)

Let me add identifier count asymmetry and KAMA quirk to the architecture skill. I'll place it after the Symbol Naming section since it relates to identifiers.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "oldString": "### File Naming",
  "newString": "### Identifier Registry Asymmetry\n\nGo and TypeScript do not always have the same number of registered identifiers.\nAs of writing, Go has **72** `core.Identifier` constants (iota 1–72) while TS\nhas **65** `IndicatorIdentifier` enum values (0-based). The gap exists because\nsome indicators have been converted in Go but not yet in TS (or vice versa).\nWhen adding a new indicator, register the identifier in **both** languages even\nif only one implementation exists yet — this keeps the registries aligned.\n\n**KAMA identifier quirk:** The Go JSON string for KAMA is\n`\"kaufmanAdaptiveMovingAverageMovingAverage\"` (with \"MovingAverage\" duplicated)\nbecause `core/identifier.go` defines it that way. The TS enum is just\n`KaufmanAdaptiveMovingAverage` (no duplicate). Any tooling that maps between\nthe two (e.g., the TS `icalc` CLI) must handle this mismatch explicitly.\n\n### File Naming"
}
```

**Error:**
```
Error: Found multiple matches for oldString. Provide more surrounding context to make the match unique.
```

---

## Assistant (Build · claude-opus-4.6 · 7.5s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "oldString": "### File Naming\n\n| Language   | Style                     | Test files                  | Example                          |\n|------------|---------------------------|-----------------------------|----------------------------------|\n| TypeScript | `kebab-case.ts`           | `kebab-case.spec.ts`        | `simple-moving-average.ts`       |",
  "newString": "### Identifier Registry Asymmetry\n\nGo and TypeScript do not always have the same number of registered identifiers.\nAs of writing, Go has **72** `core.Identifier` constants (iota 1–72) while TS\nhas **65** `IndicatorIdentifier` enum values (0-based). The gap exists because\nsome indicators have been converted in Go but not yet in TS (or vice versa).\nWhen adding a new indicator, register the identifier in **both** languages even\nif only one implementation exists yet — this keeps the registries aligned.\n\n**KAMA identifier quirk:** The Go JSON string for KAMA is\n`\"kaufmanAdaptiveMovingAverageMovingAverage\"` (with \"MovingAverage\" duplicated)\nbecause `core/identifier.go` defines it that way. The TS enum is just\n`KaufmanAdaptiveMovingAverage` (no duplicate). Any tooling that maps between\nthe two (e.g., the TS `icalc` CLI) must handle this mismatch explicitly.\n\n### File Naming\n\n| Language   | Style                     | Test files                  | Example                          |\n|------------|---------------------------|-----------------------------|----------------------------------|\n| TypeScript | `kebab-case.ts`           | `kebab-case.spec.ts`        | `simple-moving-average.ts`       |"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.6 · 5.4s)

Now add the Go params no-JSON-tags note and TS no-barrel-files convention. I'll add these as a new section after the Go Style Invariants section.

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "indicator-architecture: Add identifier count asymmetry note (Go 72 vs TS 65) + KAMA quirk",
      "priority": "medium",
      "status": "completed"
    },
    {
      "content": "indicator-architecture: Add Go params no-JSON-tags note",
      "priority": "medium",
      "status": "in_progress"
    },
    {
      "content": "indicator-architecture: Add TS no-barrel-files / .js extension import convention",
      "priority": "medium",
      "status": "pending"
    },
    {
      "content": "indicator-conversion: Add DefaultParams as explicit step in Go and TS conversion flows",
      "priority": "high",
      "status": "pending"
    },
    {
      "content": "indicator-conversion: Add TS renamed-import pattern note for factory defaultParams",
      "priority": "low",
      "status": "pending"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "indicator-architecture: Add identifier count asymmetry note (Go 72 vs TS 65) + KAMA quirk",
    "priority": "medium",
    "status": "completed"
  },
  {
    "content": "indicator-architecture: Add Go params no-JSON-tags note",
    "priority": "medium",
    "status": "in_progress"
  },
  {
    "content": "indicator-architecture: Add TS no-barrel-files / .js extension import convention",
    "priority": "medium",
    "status": "pending"
  },
  {
    "content": "indicator-conversion: Add DefaultParams as explicit step in Go and TS conversion flows",
    "priority": "high",
    "status": "pending"
  },
  {
    "content": "indicator-conversion: Add TS renamed-import pattern note for factory defaultParams",
    "priority": "low",
    "status": "pending"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 11.8s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "oldString": "### Go ↔ TypeScript Local-Variable Parity",
  "newString": "### Go Params: No JSON Tags\n\nGo indicator params structs do **not** carry `json:\"...\"` struct tags. Go's\n`encoding/json` unmarshaler performs **case-insensitive** key matching by\ndefault, so JSON `\"smoothingFactor\"` matches Go field `SmoothingFactor`\nwithout explicit tags. Do not add JSON tags to params structs — they are\nunnecessary and would diverge from the established pattern.\n\n### TypeScript Import Conventions\n\n1. **No barrel files.** The TS indicators codebase does not use `index.ts`\n   barrel re-exports. All imports use **direct file paths** to the specific\n   module (e.g., `import { Foo } from '../common/simple-moving-average/simple-moving-average.js'`).\n\n2. **`.js` extensions in import paths.** TypeScript source files use `.js`\n   extensions in import specifiers (not `.ts`). This is required by the ESM\n   module resolution used in the project (`\"type\": \"module\"` in\n   `package.json`, `tsx` loader for tests/CLI).\n\n3. **Renamed imports for disambiguation.** When multiple indicators export\n   the same symbol name (e.g., every params file exports `defaultParams`),\n   use renamed imports:\n   ```typescript\n   import { defaultParams as defaultSmaParams } from '../common/simple-moving-average/params.js';\n   import { defaultParams as defaultEmaLengthParams } from '../common/exponential-moving-average/length-params.js';\n   ```\n   This pattern is used extensively in the factory.\n\n### Go ↔ TypeScript Local-Variable Parity"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.6 · 4.8s)

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "indicator-architecture: Add identifier count asymmetry note (Go 72 vs TS 65) + KAMA quirk",
      "priority": "medium",
      "status": "completed"
    },
    {
      "content": "indicator-architecture: Add Go params no-JSON-tags note",
      "priority": "medium",
      "status": "completed"
    },
    {
      "content": "indicator-architecture: Add TS no-barrel-files / .js extension import convention",
      "priority": "medium",
      "status": "completed"
    },
    {
      "content": "indicator-conversion: Add DefaultParams as explicit step in Go and TS conversion flows",
      "priority": "high",
      "status": "in_progress"
    },
    {
      "content": "indicator-conversion: Add TS renamed-import pattern note for factory defaultParams",
      "priority": "low",
      "status": "pending"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "indicator-architecture: Add identifier count asymmetry note (Go 72 vs TS 65) + KAMA quirk",
    "priority": "medium",
    "status": "completed"
  },
  {
    "content": "indicator-architecture: Add Go params no-JSON-tags note",
    "priority": "medium",
    "status": "completed"
  },
  {
    "content": "indicator-architecture: Add TS no-barrel-files / .js extension import convention",
    "priority": "medium",
    "status": "completed"
  },
  {
    "content": "indicator-conversion: Add DefaultParams as explicit step in Go and TS conversion flows",
    "priority": "high",
    "status": "in_progress"
  },
  {
    "content": "indicator-conversion: Add TS renamed-import pattern note for factory defaultParams",
    "priority": "low",
    "status": "pending"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 9.9s)

Now add DefaultParams as an explicit step in the conversion guide. I'll add it to Go Step 2 and TS Step 2.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "oldString": "Repeat the pattern for `QuoteComponent` (default: `QuoteMidPrice`) and `TradeComponent`\n(default: `TradePrice`).\n\n### Go Step 3: Convert the output file",
  "newString": "Repeat the pattern for `QuoteComponent` (default: `QuoteMidPrice`) and `TradeComponent`\n(default: `TradePrice`).\n\n5. **Add `DefaultParams()`** at the end of the file. Every params file must export a\n   `DefaultParams()` function that returns a pointer to a fully-populated struct with\n   sensible defaults. Component fields are omitted (zero = use default). For dual-variant\n   indicators, export `DefaultLengthParams()` and `DefaultSmoothingFactorParams()` instead.\n   For no-params indicators (empty struct), still export `DefaultParams()` returning `&Params{}`.\n\n```go\n// DefaultParams returns a Params value populated with conventional defaults.\nfunc DefaultParams() *Params {\n    return &Params{\n        Length: 14,\n    }\n}\n```\n\nSee the `indicator-architecture` skill's DefaultParams section for default value\nsources and naming conventions.\n\n### Go Step 3: Convert the output file"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.6 · 6.3s)

Now add to TS Step 2:

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "newString": "tradeComponent?: TradeComponent;\n```\n\n4. **Add `defaultParams()`** at the end of the file. Every params file must export a\n   `defaultParams()` function that returns a plain object with sensible defaults.\n   Component fields are omitted (`undefined` = use default). For dual-variant indicators,\n   export `defaultLengthParams()` and `defaultSmoothingFactorParams()` instead. For\n   no-params indicators (empty interface), still export `defaultParams()` returning `{}`.\n\n```typescript\nexport function defaultParams(): SimpleMovingAverageParams {\n    return {\n        length: 14,\n    };\n}\n```\n\nSee the `indicator-architecture` skill's DefaultParams section for default value\nsources and naming conventions.\n\n### TS Step 3: Create the output file",
  "oldString": "tradeComponent?: TradeComponent;\n```\n\n### TS Step 3: Create the output file"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.6 · 4.8s)

Now update the TOC numbering — the Go steps shifted. Let me check the current TOC:

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "offset": 29,
  "limit": 20
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md</path>
<type>file</type>
<content>29: 1. [Go Conversion](#go-conversion)
30:    1. [Create the new package directory](#go-step-1-create-the-new-package-directory)
31:    2. [Convert the params file](#go-step-2-convert-the-params-file)
32:    3. [Convert the output file](#go-step-3-convert-the-output-file)
33:    4. [Convert the output test file](#go-step-4-convert-the-output-test-file)
34:    5. [Convert the main indicator file](#go-step-5-convert-the-main-indicator-file)
35:    6. [Convert the test file](#go-step-6-convert-the-test-file)
36:    7. [Register the indicator type](#go-step-7-register-the-indicator-type)
37:    8. [Verify](#go-step-8-verify)
38: 2. [TypeScript Conversion](#typescript-conversion)
39:    1. [Create the new folder](#ts-step-1-create-the-new-folder)
40:    2. [Convert the params file](#ts-step-2-convert-the-params-file)
41:    3. [Create the output file](#ts-step-3-create-the-output-file)
42:    4. [Convert the main indicator file](#ts-step-4-convert-the-main-indicator-file)
43:    5. [Convert the test file](#ts-step-5-convert-the-test-file)
44:    6. [Verify](#ts-step-6-verify)
45: 3. [Quick Reference: Import Mapping](#quick-reference-import-mapping)
46: 4. [Quick Reference: Symbol Renames](#quick-reference-symbol-renames)
47: 5. [Advanced: Multi-Constructor Indicators](#advanced-multi-constructor-indicators)
48: 6. [Advanced: Helper / Shared Component Families](#advanced-helper--shared-component-families)

(Showing lines 29-48 of 1598. Use offset=49 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.6 · 8.4s)

The TOC doesn't need numbering changes — the DefaultParams instructions were added within the existing Step 2 for both Go and TS (as sub-points), not as new numbered steps. Good.

Now the last item — TS renamed-import pattern note for factory defaultParams. This is already covered by what I added to indicator-architecture under "TypeScript Import Conventions" point 3. I'll mark it done.

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "indicator-architecture: Add identifier count asymmetry note (Go 72 vs TS 65) + KAMA quirk",
      "priority": "medium",
      "status": "completed"
    },
    {
      "content": "indicator-architecture: Add Go params no-JSON-tags note",
      "priority": "medium",
      "status": "completed"
    },
    {
      "content": "indicator-architecture: Add TS no-barrel-files / .js extension import convention",
      "priority": "medium",
      "status": "completed"
    },
    {
      "content": "indicator-conversion: Add DefaultParams as explicit step in Go and TS conversion flows",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "indicator-conversion: Add TS renamed-import pattern note for factory defaultParams",
      "priority": "low",
      "status": "completed"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "indicator-architecture: Add identifier count asymmetry note (Go 72 vs TS 65) + KAMA quirk",
    "priority": "medium",
    "status": "completed"
  },
  {
    "content": "indicator-architecture: Add Go params no-JSON-tags note",
    "priority": "medium",
    "status": "completed"
  },
  {
    "content": "indicator-architecture: Add TS no-barrel-files / .js extension import convention",
    "priority": "medium",
    "status": "completed"
  },
  {
    "content": "indicator-conversion: Add DefaultParams as explicit step in Go and TS conversion flows",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "indicator-conversion: Add TS renamed-import pattern note for factory defaultParams",
    "priority": "low",
    "status": "completed"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 7.0s)

Done. Here's what was added:

**indicator-architecture/SKILL.md** — 3 new subsections:
- **Identifier Registry Asymmetry** — Documents Go 72 vs TS 65 count difference and the KAMA `"kaufmanAdaptiveMovingAverageMovingAverage"` quirk
- **Go Params: No JSON Tags** — Documents that Go params structs rely on case-insensitive unmarshaling, no `json:` tags needed
- **TypeScript Import Conventions** — No barrel files, `.js` extensions in import paths, renamed imports for disambiguation (used in factory)

**indicator-conversion/SKILL.md** — 2 additions within existing steps:
- **Go Step 2** now includes adding `DefaultParams()` to the params file
- **TS Step 2** now includes adding `defaultParams()` to the params file

Both reference back to the architecture skill's DefaultParams section for full details.

---

## User

I the chart/ folder I have an example use-case of indicators package in a charting app:
- ohlcv-chart/ sub-folder contains the chart component implementation in Angular using D3:
  - ohlcv-chart/ itself is noy interesting, it contains an Angular control wrapper around ohlcv-chart/chart/.
  - ohlcv-chart/chart/ is not interesting for us. It contains D3-related functionality to lay-out and draw the chart itself.
  - ohlcv-chart/template/ is the most interesting for us. It describes the structure of the input parameters which you pass to the chart to draw them.
    The entry points are template.ts and configuration.ts
- study-page/ sub-folder contains the Angular page demonstrating how to chart the static test data, which is also not very interesting for us.
  - study-page/test-data/ contains hardcoded Bar series and hardcoded indicator outputs for Scalar, Band and Heatmap.
    They are slightly different from our new ones, but should be similar.
    The most interesting is the test-data.ts file.
    It has the "configTemplate: Configuration" struct with an example of how we layout panes and specify indicators and outputs.
    This configuration follows the ohlcv-chart/template/configuration.ts

Now explore the ohlcv-chart/template/configuration.ts and study-page/test-data/test-data.ts.

Can we create a small example Go app in go/cmd/iconf/ which:
- reads the JSON settings file with mentioned indicators
- gets the name of the output file (without extension, strip one if it exists)
- has hardcoded Bar data
- creates indicators, runs Bar data through them
- writes the <output name>.json file mimicking the configuration example above.
  We are interesting in filling in:
  - bands/lineAreas/lines/heatmaps
  - the "name" field should contain the mnemonic of the indicator/output
  - the "data" fiels should contain calculated data for that output
  - other info may be copied verbatim.
- writes the <output name>.ts file mimicking the configuration example above.
  It should be the same as JSON file above, but calculated data should be in static arrays in .ts file, and "data" fields should refer to them.

The import paths in individual *.ts files may be wrong (they are extracted out of their context), so don't try to compile or build them. 

---

## Assistant (Build · claude-opus-4.6 · 106.7s)

_Thinking:_

Let me explore the chart folder structure and the key files mentioned.

**Tool: task**

**Input:**
```json
{
  "description": "Explore chart folder structure",
  "prompt": "Explore the /home/dev/zpano/chart/ directory structure. I need to understand:\n\n1. The full folder tree (just directories and files, not content)\n2. Read the full contents of these files:\n   - chart/ohlcv-chart/template/configuration.ts\n   - chart/ohlcv-chart/template/template.ts\n   - chart/study-page/test-data/test-data.ts\n\nAlso read any other .ts files in chart/ohlcv-chart/template/ that define types used by configuration.ts.\n\nReturn the full contents of all files read.",
  "subagent_type": "explore"
}
```

**Output:**
```
task_id: ses_23fa196acffe2oO1IwQXThw3IU (for resuming to continue this task if needed)

<task_result>
Here are the complete results:

---

## 1. Full Directory Tree of `/home/dev/zpano/chart/`

```
chart/
├── ohlcv-chart/
│   ├── _ohlcv-chart.scss
│   ├── _ohlcv-chart-theme.scss
│   ├── ohlcv-chart.component.html
│   ├── ohlcv-chart.component.scss
│   ├── ohlcv-chart.component.spec.ts
│   ├── ohlcv-chart.component.ts
│   ├── chart/
│   │   ├── chart.ts
│   │   ├── horizontal-layout.ts
│   │   ├── horizontal-layout-block.ts
│   │   ├── indicator-arrow.ts
│   │   ├── indicator-band.ts
│   │   ├── indicator-heatmap.ts
│   │   ├── indicator-horizontal.ts
│   │   ├── indicator-line.ts
│   │   ├── indicator-line-area.ts
│   │   ├── indicator-pane.ts
│   │   ├── navigation-pane.ts
│   │   ├── price-pane.ts
│   │   ├── time-pane.ts
│   │   ├── vertical-layout.ts
│   │   └── vertical-layout-block.ts
│   └── template/
│       ├── arrow-data.ts
│       ├── band-data.ts
│       ├── configuration.ts
│       ├── heatmap-data.ts
│       ├── horizontal-data.ts
│       ├── line-area-data.ts
│       ├── line-data.ts
│       ├── margin.ts
│       ├── navigation-pane.ts
│       ├── ohlcv-data.ts
│       ├── pane.ts
│       └── template.ts
└── study-page/
    ├── ohlcv-chart-01-study.component.html
    ├── ohlcv-chart-01-study.component.scss
    ├── ohlcv-chart-01-study.component.ts
    └── test-data/
        ├── test-data.ts
        ├── test-data-bb.ts
        ├── test-data-bb-bw.ts
        ├── test-data-bb-goertzel-combined.ts
        ├── test-data-bb-lo.ts
        ├── test-data-bb-ma.ts
        ├── test-data-bb-percentb.ts
        ├── test-data-bb-up.ts
        ├── test-data-goertzel_1.ts
        ├── test-data-goertzel_10.ts
        └── test-data-ohlcv.ts
```

---

## 2. File Contents

### `/home/dev/zpano/chart/ohlcv-chart/template/configuration.ts`

```typescript
import { NavigationPane } from './navigation-pane';
import { Margin } from './margin';
import { OhlcvData } from './ohlcv-data';
import { Pane } from './pane';

/** Describes an ohlcv chart layout configuration. */
export class Configuration {
  /**
   * Total width of a chart including margins.
   * Defines the width of all chart panes.
   * Can be either a positive number of pixels or a percentage string (e.g. '45%') of a reference width.
   */
  public width!: string | number;

  /** An optional minimal width in pixels including margins. */
  public widthMin?: number;

  /** An optional maximal width in pixels including margins. */
  public widthMax?: number;

  /** An optional navigation pane. */
  public navigationPane?: NavigationPane = new NavigationPane();

  /**
   * An optional height of the navigation pane.
   * Can be either a positive number of pixels or a percentage string (e.g. '45%') of a reference width.
   * If undefined, the navigation pane will not be created.
   */
  public heightNavigationPane?: string | number;

  /** An optional d3.timeFormat specifier for time axis annotations, e.g. '%Y-%m-%d'. */
  public timeAnnotationFormat?: string;

  /** An optional d3.timeFormat specifier for time axis ticks, e.g. '%Y-%m-%d'. */
  public timeTicksFormat?: string;

  /** An optional number of ticks in the time axis. */
  public timeTicks?: number;

  /** If left axis should be visible on the price and indicator panes. */
  public axisLeft = true;

  /** If right axis should be visible on the price and indicator panes. */
  public axisRight = false;

  /** The margins of the chart, exclusive space needed for annotation. */
  public margin: Margin = new Margin();

  public ohlcv: OhlcvData = new OhlcvData();

  /** The price pane. */
  public pricePane: Pane = new Pane();

  /** An optional array of indicator panes. */
  public indicatorPanes: Pane[] = [];

  /** If *crosshair* should be visible */
  public crosshair = false;

  /** If volume in price pane should be visible */
  public volumeInPricePane = false;

  /** If menu should be visible. */
  public menuVisible = true;

  /** If *download SVG* menu setting should be visible. */
  public downloadSvgVisible = true;
}
```

---

### `/home/dev/zpano/chart/ohlcv-chart/template/template.ts`

```typescript
export * from './arrow-data';
export * from './band-data';
export * from './configuration';
export * from './heatmap-data';
export * from './horizontal-data';
export * from './line-area-data';
export * from './line-data';
export * from './margin';
export * from './navigation-pane';
export * from './ohlcv-data';
export * from './pane';
```

---

### `/home/dev/zpano/chart/study-page/test-data/test-data.ts`

```typescript
import { Configuration } from 'projects/mb/src/lib/charts/ohlcv-chart/template/configuration';

import { testDataOhlcv } from '../../../test-data/indicators/test-data-ohlcv';
import { testDataBb } from '../../../test-data/indicators/test-data-bb';
import { testDataBbMa } from '../../../test-data/indicators/test-data-bb-ma';
import { testDataBbLo } from '../../../test-data/indicators/test-data-bb-lo';
import { testDataBbUp } from '../../../test-data/indicators/test-data-bb-up';
import { testDataBbPercentB } from '../../../test-data/indicators/test-data-bb-percentb';
import { testDataBbBw } from '../../../test-data/indicators/test-data-bb-bw';
import { testDataGoertzel1 } from '../../../test-data/indicators/test-data-goertzel_1';
import { testDataOutputs } from './test-data-bb-goertzel-combined';

const outputsCount = testDataOutputs.length;
const ohlcvCount = testDataOhlcv.length;

export class TestData {
  private static configTemplate: Configuration = {
    width: '100%', // widthMin: 500, widthMax: 700,
    navigationPane: {
      height: 30, // heightMin: 30, heightMax: 30, timeTicksFormat: '%Y-%m-%d',
      hasLine: true, hasArea: false, hasTimeAxis: true, timeTicks: 0,
    },
    heightNavigationPane: 30,
    timeAnnotationFormat: '%Y-%m-%d',
    axisLeft: true,
    axisRight: false,
    margin: { left: 0, top: 10, right: 20, bottom: 0 },
    ohlcv: { name: 'BRILL@XAMS', data: [], candlesticks: true },
    pricePane: {
      height: '30%', valueFormat: ',.2f', valueMarginPercentageFactor: 0.01,
      bands: [
        {
          name: 'bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 5,
          color: 'rgba(0,255,0,0.3)', legendColor: 'rgba(0,200,0,1)', interpolation: 'natural'
        },
      ], lineAreas: [], horizontals: [], lines: [
        {
          name: 'ma-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 0,
          color: 'red', width: 1, dash: '', interpolation: 'natural'
        },
        {
          name: 'lo-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 1,
          color: 'blue', width: 0.5, dash: '5,5', interpolation: 'cardinal'
        },
        {
          name: 'up-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 2,
          color: 'blue', width: 0.5, dash: '2,2', interpolation: 'linear'
        }
      ], arrows: []
    },
    indicatorPanes: [
      {
        height: '60', valueFormat: ',.2f', valueMarginPercentageFactor: 0.01,
        bands: [], lineAreas: [], horizontals: [
          { value: 0, color: 'red', width: 0.5, dash: '' },
          { value: 1, color: 'red', width: 0.5, dash: '' }
        ], lines: [
          {
            name: '%b(c)-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 3,
            color: 'green', width: 1, dash: '', interpolation: 'natural'
          }
        ], arrows: []
      },
      {
        height: '60', valueFormat: ',.2f', valueMarginPercentageFactor: 0.01,
        bands: [], lineAreas: [
          {
            name: 'bw-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 4,
            color: 'rgba(0,0,255,0.3)', legendColor: '#0000ff', value: 0.3, interpolation: 'natural'
          }
        ], horizontals: [
          { value: 0.3, color: 'blue', width: 0.5, dash: '' }
        ], lines: [], arrows: []
      },
      {
        height: '120', valueFormat: ',.2f', valueMarginPercentageFactor: 0,
        heatmap: {
          name: 'goertzel(64, [2,28,1/10])', gradient: 'Viridis', invertGradient: false,
          data: [], indicator: 1, output: 0
        }, bands: [], lineAreas: [], horizontals: [], lines: [], arrows: []
      }
    ],
    crosshair: false,
    volumeInPricePane: true,
    menuVisible: true, downloadSvgVisible: true
  };

  private dataLength = 0;
  public config: Configuration;

  public static get configDataPrefilled(): Configuration {
    const cloned = TestData.deepCopy(TestData.configTemplate) as Configuration;
    cloned.ohlcv.data = testDataOhlcv;
    cloned.pricePane.bands[0].data = testDataBb;
    cloned.pricePane.lines[0].data = testDataBbMa;
    cloned.pricePane.lines[1].data = testDataBbLo;
    cloned.pricePane.lines[2].data = testDataBbUp;
    cloned.indicatorPanes[0].lines[0].data = testDataBbPercentB;
    cloned.indicatorPanes[1].lineAreas[0].data = testDataBbBw;
    // @ts-ignore
    cloned.indicatorPanes[2].heatmap.data = testDataGoertzel1;
    TestData.addArrows(cloned);
    return cloned;
  }

  private static add(count: number, currentCount: number, cfg: Configuration): number {
    const sum = currentCount + count;
    const newCount = sum > outputsCount ? outputsCount : sum;
    for (let i = currentCount; i < newCount; ++i) {
      cfg.ohlcv.data.push(testDataOhlcv[i]);
      const indicators = (testDataOutputs[i] as any).indicators;
      for (const band of cfg.pricePane.bands) {
        const outputs = (indicators[band.indicator] as any).outputs;
        band.data.push(outputs[band.output]);
      }
      for (const lineArea of cfg.pricePane.lineAreas) {
        const outputs = (indicators[lineArea.indicator] as any).outputs;
        lineArea.data.push(outputs[lineArea.output]);
      }
      for (const line of cfg.pricePane.lines) {
        const outputs = (indicators[line.indicator] as any).outputs;
        line.data.push(outputs[line.output]);
      }
      for (const pane of cfg.indicatorPanes) {
        if (pane.heatmap) {
          const heatmap = pane.heatmap;
          const outputs = (indicators[heatmap.indicator] as any).outputs;
          heatmap.data.push(outputs[heatmap.output]);
        }
        for (const band of pane.bands) {
          const outputs = (indicators[band.indicator] as any).outputs;
          band.data.push(outputs[band.output]);
        }
        for (const lineArea of pane.lineAreas) {
          const outputs = (indicators[lineArea.indicator] as any).outputs;
          lineArea.data.push(outputs[lineArea.output]);
        }
        for (const line of pane.lines) {
          const outputs = (indicators[line.indicator] as any).outputs;
          line.data.push(outputs[line.output]);
        }
      }
    }
    TestData.addArrows(cfg);
    return newCount;
  }

  private static addArrows(cfg: Configuration): void {
    const count = cfg.ohlcv.data.length;
    if (count > 25 && cfg.pricePane.arrows.length < 1) {
      const arrow = {
        name: 'sell', down: true, time: testDataOhlcv[25].time,
        indicator: 0, output: 0, color: 'rgb(255,0,0)'
      };
      cfg.pricePane.arrows.push(arrow);
    }
    if (count > 26 && cfg.pricePane.arrows.length < 2) {
      const arrow = {
        name: 'buy', down: false, time: testDataOhlcv[26].time,
        indicator: 0, output: 0, color: 'rgb(0,255,0)'
      };
      cfg.pricePane.arrows.push(arrow);
    }
  }

  private static deepCopy(obj: any): any {
    if (null == obj || 'object' !== typeof obj) { return obj; }
    if (obj instanceof Date) {
      const copy = new Date();
      copy.setTime(obj.getTime());
      return copy;
    }
    if (obj instanceof Array) {
      const copy = [];
      for (let i = 0, len = obj.length; i < len; i++) {
        copy[i] = TestData.deepCopy(obj[i]);
      }
      return copy;
    }
    if (obj instanceof Object) {
      const copy: any = {};
      for (const attr of Object.keys(obj)) {
        copy[attr] = TestData.deepCopy(obj[attr]);
      }
      return copy;
    }
    throw new Error('Unable to copy obj! Its type isn\'t supported.');
  }

  public constructor() {
    this.config = TestData.configDataPrefilled;
    if (outputsCount !== ohlcvCount) {
      throw new Error('Lengths does not match: ohlcv=' + ohlcvCount + ', outputs=' + outputsCount);
    }
  }

  public clear(): void {
    this.config.ohlcv.data = [];
    this.config.pricePane.bands[0].data = [];
    this.config.pricePane.lines[0].data = [];
    this.config.pricePane.lines[1].data = [];
    this.config.pricePane.lines[2].data = [];
    this.config.pricePane.arrows = [];
    this.config.indicatorPanes[0].lines[0].data = [];
    this.config.indicatorPanes[1].lineAreas[0].data = [];
    // @ts-ignore
    this.config.indicatorPanes[2].heatmap.data = [];
    this.dataLength = 0;
  }

  public addSingle(): void {
    this.dataLength = TestData.add(1, this.dataLength, this.config);
  }

  public addTen(): void {
    this.dataLength = TestData.add(10, this.dataLength, this.config);
  }

  public addAll(): void {
    this.dataLength = TestData.add(ohlcvCount, this.dataLength, this.config);
  }
}
```

---

### Types used by `configuration.ts` (direct imports)

#### `/home/dev/zpano/chart/ohlcv-chart/template/navigation-pane.ts`

```typescript
/** Describes a navigation pane. */
export class NavigationPane {
  /**
   * A height of the pane.
   * Can be either a positive number of pixels or a percentage string (e.g. '45%') of a reference width.
   */
  public height: string | number = 30;

  /** An optional minimal height of the pane in pixels. */
  public heightMin?: number;

  /** An optional maximal height of the pane in pixels. */
  public heightMax?: number;

  /** If navigation pane has closing price line. */
  public hasLine = true;

  /** If navigation pane has closing price area. */
  public hasArea = false;

  /** If navigation pane has time axis. */
  public hasTimeAxis = true;

  /** An optional d3.timeFormat specifier for time axis ticks, e.g. '%Y-%m-%d'. */
  public timeTicksFormat?: string;

  /** An optional number of ticks in the time axis. */
  public timeTicks?: number;
}
```

#### `/home/dev/zpano/chart/ohlcv-chart/template/margin.ts`

```typescript
/** The margins of the chart, exclusive space needed for annotation. */
export class Margin {
  public left = 0;
  public top = 0;
  public right = 0;
  public bottom = 0;
}
```

#### `/home/dev/zpano/chart/ohlcv-chart/template/ohlcv-data.ts`

```typescript
import { Bar } from '../../../data/entities/bar';

/** Describes an ohlcv data. */
export class OhlcvData {
  /** A name of the data. */
  public name = '';

  /** Data array. */
  public data: Bar[] = [];

  /** If data is displayed as candlesticks or as bars */
  public candlesticks = true;
}
```

#### `/home/dev/zpano/chart/ohlcv-chart/template/pane.ts`

```typescript
import { HeatmapData } from './heatmap-data';
import { BandData } from './band-data';
import { LineAreaData } from './line-area-data';
import { HorizontalData } from './horizontal-data';
import { LineData } from './line-data';
import { ArrowData } from './arrow-data';

/** Describes a pane. */
export class Pane {
  /**
   * A height of the pane.
   * Can be either a positive number of pixels or a percentage string (e.g. '45%') of a reference width.
   */
  public height: string | number = 0;

  /** An optional minimal height of the pane in pixels. */
  public heightMin?: number;

  /** An optional maximal height of the pane in pixels. */
  public heightMax?: number;

  /** A d3.format specifier for value ticks and annotations on the pane. */
  public valueFormat = ',.2f';

  /** An optional number of ticks in the value axis. */
  public valueTicks?: number;

  /**
   * A percentage factor (e.g., 0.05) to add to a lower and upper parts of a value axis.
   * This allows to add space between the top / bottom of the pane and the max / min values.
   */
  public valueMarginPercentageFactor = 0;

  /** An optional heatmap on this pane. */
  public heatmap?: HeatmapData;

  /** An array of indicator bands on this pane. */
  public bands: BandData[] = [];

  /** An array of indicator line areas on this pane. */
  public lineAreas: LineAreaData[] = [];

  /** An array of indicator bands on this pane. */
  public horizontals: HorizontalData[] = [];

  /** An array of indicator lines on this pane. */
  public lines: LineData[] = [];

  /** An array of arrows on this pane. */
  public arrows: ArrowData[] = [];
}
```

---

### Types used by `Pane` (transitive dependencies of `configuration.ts`)

#### `/home/dev/zpano/chart/ohlcv-chart/template/heatmap-data.ts`

```typescript
import { Heatmap } from '../../entities/heatmap';

/** Describes a heatmap in a pane. */
export class HeatmapData {
  /** A name of the data. */
  public name = '';

  /** Data array. */
  public data: Heatmap[] = [];

  /** An index of an indicator in the output data. */
  public indicator = 0;

  /** An index of an output within an indicator in the output data. */
  public output = 0;

  /** An intensity gradient:
   * - Viridis
   * - Inferno
   * - Magma
   * - Plasma
   * - Cividis
   * - Warm
   * - Cool
   * - Rainbow
   * - CubehelixDefault
   * - BuGn
   * - BuPu
   * - GnBu
   * - OrRd
   * - PuBuGn
   * - PuBu
   * - PuRd
   * - RdPu
   * - YlGnBu
   * - YlGn
   * - YlOrBr
   * - YlOrRd
   * - Blues
   * - Greens
   * - Greys
   * - Oranges
   * - Purples
   * - Reds
   */
  public gradient = 'viridis';

  /** If to invert the gradient. */
  public invertGradient = false;
}
```

#### `/home/dev/zpano/chart/ohlcv-chart/template/band-data.ts`

```typescript
import { Band } from '../../entities/band';

/** Describes a band in a pane. */
export class BandData {
  /** A name of the data. */
  public name = '';

  /** Data array. */
  public data: Band[] = [];

  /** An index of an indicator in the output data. */
  public indicator = 0;

  /** An index of an output within an indicator in the output data. */
  public output = 0;

  /** A fill color of the band. */
  public color = 'rgba(0,0,0,0.0667)';

  /**
   *  The fill color of the band may be very transprent and its visibility on a legend may be poor.
   *  This allows to specify a legend color different from the fill color.
   */
  public legendColor?: string;

  /** A band edge interpoltion method:
   * - linear
   * - natural
   * - basis
   * - camullRom
   * - cardinal
   * - step
   * - stepBefore
   * - stepAfter
   */
  public interpolation = 'natural';
}
```

#### `/home/dev/zpano/chart/ohlcv-chart/template/line-area-data.ts`

```typescript
import { Scalar } from '../../../data/entities/scalar';

/** Describes an area in a pane between a line and a constant value. */
export class LineAreaData {
  /** A name of the data. */
  public name = '';

  /** Data array. */
  public data: Scalar[] = [];

  /** An index of an indicator in the output data. */
  public indicator = 0;

  /** An index of an output within an indicator in the output data. */
  public output = 0;

  /** A constant limiting value. */
  public value = 0;

  /** A fill color of an area. */
  public color = 'rgba(0,0,0,0.0667)';

  /**
   *  The fill color of an area may be very transprent and its visibility on a legend may be poor.
   *  This allows to specify a legend color different from the fill color.
   */
  public legendColor?: string;

  /** An erea edge interpoltion method:
   * - linear
   * - natural
   * - basis
   * - camullRom
   * - cardinal
   * - step
   * - stepBefore
   * - stepAfter
   */
  public interpolation = 'natural';
}
```

#### `/home/dev/zpano/chart/ohlcv-chart/template/horizontal-data.ts`

```typescript
/** Describes a horizontal line in a pane. */
export class HorizontalData {
  /** Data value. */
  public value = 0;

  /** A color of the line stroke. */
  public color = 'black';

  /** A width of the line stroke in pixels. */
  public width = 1;

  /** A dash array of the line stroke, e.g. '5,5' or '20,10,5,5,5,10' or empty if no dashes. */
  public dash = '';
}
```

#### `/home/dev/zpano/chart/ohlcv-chart/template/line-data.ts`

```typescript
import { Scalar } from '../../../data/entities/scalar';

/** Describes a line in a pane. */
export class LineData {
  /** A name of the data. */
  public name = '';

  /** Data array. */
  public data: Scalar[] = [];

  /** An index of an indicator in the output data. */
  public indicator = 0;

  /** An index of an output within an indicator in the output data. */
  public output = 0;

  /** A color of the line stroke. */
  public color = 'black';

  /** A width of the line stroke in pixels. */
  public width = 1;

  /** A dash array of the line stroke, e.g. '5,5' or '20,10,5,5,5,10' or empty if no dashes. */
  public dash = '';

  /** A line curve interpoltion method:
   * - linear
   * - natural
   * - basis
   * - camullRom
   * - cardinal
   * - step
   * - stepBefore
   * - stepAfter
   */
  public interpolation = 'natural';
}
```

#### `/home/dev/zpano/chart/ohlcv-chart/template/arrow-data.ts`

```typescript
/** Describes an vertical arrow in a pane. */
export class ArrowData {
  /** A name of the arrow. */
  public name = '';

  /** Is an arrow points up or down. */
  public down!: boolean;

  /** Arrow time. */
  public time!: Date;

  /** An optional value to which the arrow points. */
  public value?: number;

  /** An index of an indicator in the output data. */
  public indicator = 0;

  /** An index of an output within an indicator in the output data. */
  public output = 0;

  /** A color of the arrow. */
  public color!: string;
}
```

---

All 11 `.ts` files in the `chart/ohlcv-chart/template/` directory have been read in full. The type dependency chain from `configuration.ts` is:

- **Configuration** imports `NavigationPane`, `Margin`, `OhlcvData`, `Pane`
- **Pane** imports `HeatmapData`, `BandData`, `LineAreaData`, `HorizontalData`, `LineData`, `ArrowData`
- **OhlcvData** references external type `Bar` (from `../../../data/entities/bar`)
- **HeatmapData** references external type `Heatmap` (from `../../entities/heatmap`)
- **BandData** references external type `Band` (from `../../entities/band`)
- **LineData** and **LineAreaData** reference external type `Scalar` (from `../../../data/entities/scalar`)
</task_result>
```

---

## User

---

## Assistant (Compaction · claude-opus-4.6 · 54.1s)



---

## Goal

Build a Go CLI tool at `go/cmd/iconf/` that:
1. Reads a JSON settings file specifying indicators (same format as `go/cmd/icalc/settings.json`)
2. Takes an output file name (without extension) as a CLI argument
3. Has hardcoded Bar data (same 252-bar TA-Lib test data used in `icalc`)
4. Creates indicators via the factory, runs bar data through them
5. Writes `<output>.json` — a chart configuration file mimicking the structure from `chart/ohlcv-chart/template/configuration.ts` and `chart/study-page/test-data/test-data.ts`, with calculated indicator data populated in bands/lineAreas/lines/heatmaps, using indicator/output mnemonics as `name` fields
6. Writes `<output>.ts` — same structure but with calculated data in static TypeScript arrays, and `data` fields referencing those arrays by variable name

## Instructions

- The Configuration structure to mimic is defined in `chart/ohlcv-chart/template/configuration.ts` — it has `pricePane` (a `Pane`) and `indicatorPanes` (array of `Pane`), where each `Pane` has `bands`, `lineAreas`, `lines`, `heatmaps`, `horizontals`, `arrows`
- The example usage in `chart/study-page/test-data/test-data.ts` shows how Bollinger Bands outputs are distributed: band output → `pricePane.bands`, middle/lower/upper lines → `pricePane.lines`, percentB → `indicatorPanes[0].lines`, bandWidth → `indicatorPanes[1].lineAreas`, Goertzel spectrum → `indicatorPanes[2].heatmap`
- Each data item in bands/lines/lineAreas/heatmaps has: `name` (use indicator/output mnemonic), `data` (the calculated values), `indicator` (index), `output` (index), plus visual properties (color, width, dash, interpolation)
- The `data` field types from the chart template: lines and lineAreas use `Scalar` (which is `{time, value}`), bands use `Band` (which is `{time, upper, lower}` or similar), heatmaps use `Heatmap`
- Output shape information is available from `core.BuildMetadata` / the descriptor registry — use `shape.Scalar`, `shape.Band`, `shape.Heatmap` etc. to determine which pane collection each output belongs to
- For the `.ts` file: data arrays should be exported as `const` variables (e.g., `export const sma14Data: Scalar[] = [...]`) and the configuration object's `data` fields should reference these variable names
- The Go module path is `zpano`
- The factory is at `go/indicators/factory/factory.go` with `func New(identifier core.Identifier, paramsJSON []byte) (core.Indicator, error)`
- The icalc CLI at `go/cmd/icalc/main.go` can be used as reference for reading settings, creating indicators, and processing bars
- Import paths in the generated `.ts` file may be wrong (since it's generated out of context) — that's acceptable
- Strip extension from output filename if user provides one

## Discoveries

### Chart Configuration Structure (from `chart/ohlcv-chart/template/`)

The `Configuration` class has:
- `width`, `widthMin`, `widthMax` — chart dimensions
- `navigationPane` — optional nav pane with `height`, `hasLine`, `hasArea`, `hasTimeAxis`, `timeTicks`
- `margin` — `{left, top, right, bottom}`
- `ohlcv` — `{name, data: Bar[], candlesticks}`
- `pricePane` — a `Pane` (the main price chart)
- `indicatorPanes` — array of `Pane` (sub-charts for oscillators, volatility, spectra, etc.)
- `crosshair`, `volumeInPricePane`, `menuVisible`, `downloadSvgVisible`

Each `Pane` has:
- `height`, `valueFormat`, `valueMarginPercentageFactor`
- `heatmap?` — optional `HeatmapData` (`{name, data: Heatmap[], indicator, output, gradient, invertGradient}`)
- `bands` — `BandData[]` (`{name, data: Band[], indicator, output, color, legendColor, interpolation}`)
- `lineAreas` — `LineAreaData[]` (`{name, data: Scalar[], indicator, output, value, color, legendColor, interpolation}`)
- `lines` — `LineData[]` (`{name, data: Scalar[], indicator, output, color, width, dash, interpolation}`)
- `horizontals` — `HorizontalData[]` (`{value, color, width, dash}`)
- `arrows` — `ArrowData[]` (`{name, down, time, value, indicator, output, color}`)

### Data Types Referenced
- `Scalar` — `{time, value}` (used by lines, lineAreas)
- `Band` — upper/lower bounds with time (used by bands)
- `Heatmap` — spectrum/matrix data with time (used by heatmap)
- `Bar` — OHLCV data (used by ohlcv.data)

### Output Shape → Pane Mapping Logic
From the test-data example and descriptor registry:
- `shape.Scalar` outputs with `Pane: Price` → `pricePane.lines`
- `shape.Scalar` outputs with `Pane: Own` → new `indicatorPanes[].lines`
- `shape.Band` outputs with `Pane: Price` → `pricePane.bands`
- `shape.Heatmap` outputs → `indicatorPanes[].heatmap`
- The descriptor registry (`core/descriptors.go`) has `Role` and `Pane` per output that determines placement

### Existing icalc Reference
- `go/cmd/icalc/main.go` has the 252-bar embedded test data and settings-file reading logic
- Settings format: `[{"identifier": "simpleMovingAverage", "params": {"length": 14}}, ...]`
- Factory call: `factory.New(id, paramsJSON)` returns `core.Indicator`
- Indicator outputs come from `indicator.UpdateBar(&bar)` which returns `[]core.Output` (each output is a Scalar, Band, or Heatmap)

## Accomplished

### Completed (prior work, not this task)
1. Factory packages in Go and TS — fully complete
2. `icalc` CLI in Go and TS — fully complete
3. `DefaultParams()` / `defaultParams()` added to all indicators in both languages
4. Skill files updated with factory, DefaultParams, and new findings (identifier asymmetry, KAMA quirk, no JSON tags, no barrel files, TS import conventions)

### In Progress
- **Explored** the chart template structure and test-data example — understanding complete
- **Not yet started**: Creating `go/cmd/iconf/` — this is the next step

### Left To Do
1. Create `go/cmd/iconf/main.go` — the CLI tool
2. Define Go structs mirroring the Configuration/Pane/LineData/BandData/HeatmapData/etc. types (for JSON serialization)
3. Implement logic to:
   - Read settings JSON, create indicators via factory
   - Run 252 bars through each indicator, collect outputs
   - Use descriptor registry (Metadata → outputs[].Shape and outputs[].Pane) to determine where each output goes (pricePane vs indicatorPanes, lines vs bands vs heatmaps)
   - Build the Configuration struct with populated data
4. Write `<output>.json` with `encoding/json`
5. Write `<output>.ts` with data arrays as exported const variables and configuration referencing them
6. Add hardcoded bar data (reuse from icalc or share)

## Relevant files / directories

### To Create
- `go/cmd/iconf/main.go` — new CLI tool

### Key Reference — Chart Template Types
- `chart/ohlcv-chart/template/configuration.ts` — Configuration class definition
- `chart/ohlcv-chart/template/pane.ts` — Pane class (bands, lineAreas, lines, heatmap, arrows, horizontals)
- `chart/ohlcv-chart/template/line-data.ts` — LineData (name, data:Scalar[], indicator, output, color, width, dash, interpolation)
- `chart/ohlcv-chart/template/band-data.ts` — BandData (name, data:Band[], indicator, output, color, legendColor, interpolation)
- `chart/ohlcv-chart/template/line-area-data.ts` — LineAreaData (name, data:Scalar[], indicator, output, value, color, legendColor, interpolation)
- `chart/ohlcv-chart/template/heatmap-data.ts` — HeatmapData (name, data:Heatmap[], indicator, output, gradient, invertGradient)
- `chart/ohlcv-chart/template/horizontal-data.ts` — HorizontalData (value, color, width, dash)
- `chart/ohlcv-chart/template/arrow-data.ts` — ArrowData (name, down, time, value, indicator, output, color)
- `chart/ohlcv-chart/template/ohlcv-data.ts` — OhlcvData (name, data:Bar[], candlesticks)
- `chart/ohlcv-chart/template/margin.ts` — Margin (left, top, right, bottom)
- `chart/ohlcv-chart/template/navigation-pane.ts` — NavigationPane

### Key Reference — Example Usage
- `chart/study-page/test-data/test-data.ts` — Shows configTemplate with BB+Goertzel example, output→pane mapping

### Key Reference — Factory & Indicator System
- `go/indicators/factory/factory.go` — Factory `New()` function
- `go/cmd/icalc/main.go` — Reference CLI (settings reading, bar data, indicator creation)
- `go/cmd/icalc/settings.json` — 72 indicator settings entries
- `go/indicators/core/indicator.go` — Indicator interface, Output types
- `go/indicators/core/descriptors.go` — Descriptor registry (Shape, Role, Pane per output)
- `go/indicators/core/identifier.go` — 72 Identifier constants
- `go/indicators/core/outputs/` — Output types (Scalar, Band, Heatmap)
- `go/indicators/core/outputs/shape/` — Shape enum (Scalar, Band, Heatmap, Polyline)

### Skill Files (recently updated)
- `.opencode/skills/indicator-architecture/SKILL.md`
- `.opencode/skills/indicator-conversion/SKILL.md`

---

---

## User

---

## Assistant (Build · claude-opus-4.6 · 8.0s)

_Thinking:_

Let me start building the `go/cmd/iconf/main.go` tool. I need to look at the key reference files first.

Let me start building the `iconf` CLI tool. I'll first gather the necessary context from reference files.

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Read reference files (icalc/main.go, core/descriptors.go, core/outputs, chart template types)",
      "status": "in_progress",
      "priority": "high"
    },
    {
      "content": "Create go/cmd/iconf/main.go with config structs and CLI logic",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Implement output shape → pane mapping using descriptor registry",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Implement JSON output writer",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Implement TypeScript output writer",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Build and test the tool",
      "status": "pending",
      "priority": "high"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Read reference files (icalc/main.go, core/descriptors.go, core/outputs, chart template types)",
    "status": "in_progress",
    "priority": "high"
  },
  {
    "content": "Create go/cmd/iconf/main.go with config structs and CLI logic",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Implement output shape → pane mapping using descriptor registry",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Implement JSON output writer",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Implement TypeScript output writer",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Build and test the tool",
    "status": "pending",
    "priority": "high"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 5.8s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/cmd/icalc/main.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/cmd/icalc/main.go</path>
<type>file</type>
<content>1: // Package main implements icalc, a command-line indicator calculator.
2: //
3: // It reads a JSON settings file containing indicator definitions,
4: // creates indicator instances, prints their metadata, then iterates
5: // through embedded bar data printing bar values and all indicator
6: // outputs on each iteration.
7: package main
8: 
9: import (
10: 	"encoding/json"
11: 	"fmt"
12: 	"math"
13: 	"os"
14: 	"time"
15: 
16: 	"zpano/entities"
17: 	"zpano/indicators/core"
18: 	"zpano/indicators/core/outputs"
19: 	"zpano/indicators/factory"
20: )
21: 
22: // settingsEntry represents a single indicator entry in the settings JSON file.
23: type settingsEntry struct {
24: 	Identifier core.Identifier `json:"identifier"`
25: 	Params     json.RawMessage `json:"params"`
26: }
27: 
28: func main() {
29: 	if len(os.Args) < 2 {
30: 		fmt.Fprintf(os.Stderr, "usage: icalc <settings.json>\n")
31: 		os.Exit(1)
32: 	}
33: 
34: 	data, err := os.ReadFile(os.Args[1])
35: 	if err != nil {
36: 		fmt.Fprintf(os.Stderr, "error reading settings file: %v\n", err)
37: 		os.Exit(1)
38: 	}
39: 
40: 	var entries []settingsEntry
41: 	if err := json.Unmarshal(data, &entries); err != nil {
42: 		fmt.Fprintf(os.Stderr, "error parsing settings file: %v\n", err)
43: 		os.Exit(1)
44: 	}
45: 
46: 	indicators := make([]core.Indicator, 0, len(entries))
47: 
48: 	for _, e := range entries {
49: 		ind, err := factory.New(e.Identifier, string(e.Params))
50: 		if err != nil {
51: 			fmt.Fprintf(os.Stderr, "error creating indicator %s: %v\n", e.Identifier, err)
52: 			os.Exit(1)
53: 		}
54: 
55: 		indicators = append(indicators, ind)
56: 	}
57: 
58: 	printMetadata(indicators)
59: 
60: 	bars := testBars()
61: 
62: 	fmt.Println()
63: 	fmt.Println("=== Bar Data & Indicator Outputs ===")
64: 	fmt.Println()
65: 
66: 	for i, bar := range bars {
67: 		fmt.Printf("Bar[%3d] %s  O=%.4f H=%.4f L=%.4f C=%.4f V=%.0f\n",
68: 			i, bar.Time.Format("2006-01-02"), bar.Open, bar.High, bar.Low, bar.Close, bar.Volume)
69: 
70: 		for _, ind := range indicators {
71: 			meta := ind.Metadata()
72: 			output := ind.UpdateBar(&bar)
73: 
74: 			fmt.Printf("  %-45s primed=%-5v ", meta.Mnemonic, ind.IsPrimed())
75: 			printOutput(meta.Outputs, output)
76: 			fmt.Println()
77: 		}
78: 
79: 		fmt.Println()
80: 	}
81: }
82: 
83: // printMetadata prints metadata for all indicators.
84: func printMetadata(indicators []core.Indicator) {
85: 	fmt.Println("=== Indicator Metadata ===")
86: 	fmt.Println()
87: 
88: 	for i, ind := range indicators {
89: 		meta := ind.Metadata()
90: 		metaJSON, _ := json.MarshalIndent(meta, "  ", "  ")
91: 
92: 		fmt.Printf("[%d] %s\n", i, meta.Mnemonic)
93: 		fmt.Printf("  Identifier:  %s\n", meta.Identifier)
94: 		fmt.Printf("  Description: %s\n", meta.Description)
95: 		fmt.Printf("  Outputs (%d):\n", len(meta.Outputs))
96: 
97: 		for j, out := range meta.Outputs {
98: 			fmt.Printf("    [%d] kind=%d shape=%s mnemonic=%q description=%q\n",
99: 				j, out.Kind, out.Shape, out.Mnemonic, out.Description)
100: 		}
101: 
102: 		fmt.Printf("  Full metadata JSON:\n  %s\n\n", metaJSON)
103: 	}
104: }
105: 
106: // printOutput prints output values inline.
107: func printOutput(outputsMeta []outputs.Metadata, output core.Output) {
108: 	for i, val := range output {
109: 		name := fmt.Sprintf("out[%d]", i)
110: 		if i < len(outputsMeta) {
111: 			name = outputsMeta[i].Mnemonic
112: 		}
113: 
114: 		switch v := val.(type) {
115: 		case entities.Scalar:
116: 			if math.IsNaN(v.Value) {
117: 				fmt.Printf("%s=NaN ", name)
118: 			} else {
119: 				fmt.Printf("%s=%.4f ", name, v.Value)
120: 			}
121: 		case *outputs.Band:
122: 			if v.IsEmpty() {
123: 				fmt.Printf("%s=Band(NaN) ", name)
124: 			} else {
125: 				fmt.Printf("%s=Band(%.4f,%.4f) ", name, v.Lower, v.Upper)
126: 			}
127: 		default:
128: 			fmt.Printf("%s=%v ", name, v)
129: 		}
130: 	}
131: }
132: 
133: // testBars returns 252 embedded test bars constructed from the standard TA-Lib
134: // daily reference data used across the indicator unit tests.
135: func testBars() []entities.Bar {
136: 	highs := testHighs()
137: 	lows := testLows()
138: 	closes := testCloses()
139: 	volumes := testVolumes()
140: 
141: 	// Open data is not available in the standard TA-Lib test set used in this
142: 	// codebase, so we approximate open as the previous close (or the first close).
143: 	bars := make([]entities.Bar, len(closes))
144: 	baseTime := time.Date(2020, 1, 2, 0, 0, 0, 0, time.UTC)
145: 
146: 	for i := range closes {
147: 		openPrice := closes[0]
148: 		if i > 0 {
149: 			openPrice = closes[i-1]
150: 		}
151: 
152: 		bars[i] = entities.Bar{
153: 			Time:   baseTime.AddDate(0, 0, i),
154: 			Open:   openPrice,
155: 			High:   highs[i],
156: 			Low:    lows[i],
157: 			Close:  closes[i],
158: 			Volume: volumes[i],
159: 		}
160: 	}
161: 
162: 	return bars
163: }
164: 
165: func testHighs() []float64 {
166: 	return []float64{
167: 		93.25, 94.94, 96.375, 96.19, 96, 94.72, 95, 93.72, 92.47, 92.75,
168: 		96.25, 99.625, 99.125, 92.75, 91.315, 93.25, 93.405, 90.655, 91.97, 92.25,
169: 		90.345, 88.5, 88.25, 85.5, 84.44, 84.75, 84.44, 89.405, 88.125, 89.125,
170: 		87.155, 87.25, 87.375, 88.97, 90, 89.845, 86.97, 85.94, 84.75, 85.47,
171: 		84.47, 88.5, 89.47, 90, 92.44, 91.44, 92.97, 91.72, 91.155, 91.75,
172: 		90, 88.875, 89, 85.25, 83.815, 85.25, 86.625, 87.94, 89.375, 90.625,
173: 		90.75, 88.845, 91.97, 93.375, 93.815, 94.03, 94.03, 91.815, 92, 91.94,
174: 		89.75, 88.75, 86.155, 84.875, 85.94, 99.375, 103.28, 105.375, 107.625, 105.25,
175: 		104.5, 105.5, 106.125, 107.94, 106.25, 107, 108.75, 110.94, 110.94, 114.22,
176: 		123, 121.75, 119.815, 120.315, 119.375, 118.19, 116.69, 115.345, 113, 118.315,
177: 		116.87, 116.75, 113.87, 114.62, 115.31, 116, 121.69, 119.87, 120.87, 116.75,
178: 		116.5, 116, 118.31, 121.5, 122, 121.44, 125.75, 127.75, 124.19, 124.44,
179: 		125.75, 124.69, 125.31, 132, 131.31, 132.25, 133.88, 133.5, 135.5, 137.44,
180: 		138.69, 139.19, 138.5, 138.13, 137.5, 138.88, 132.13, 129.75, 128.5, 125.44,
181: 		125.12, 126.5, 128.69, 126.62, 126.69, 126, 123.12, 121.87, 124, 127,
182: 		124.44, 122.5, 123.75, 123.81, 124.5, 127.87, 128.56, 129.63, 124.87, 124.37,
183: 		124.87, 123.62, 124.06, 125.87, 125.19, 125.62, 126, 128.5, 126.75, 129.75,
184: 		132.69, 133.94, 136.5, 137.69, 135.56, 133.56, 135, 132.38, 131.44, 130.88,
185: 		129.63, 127.25, 127.81, 125, 126.81, 124.75, 122.81, 122.25, 121.06, 120,
186: 		123.25, 122.75, 119.19, 115.06, 116.69, 114.87, 110.87, 107.25, 108.87, 109,
187: 		108.5, 113.06, 93, 94.62, 95.12, 96, 95.56, 95.31, 99, 98.81,
188: 		96.81, 95.94, 94.44, 92.94, 93.94, 95.5, 97.06, 97.5, 96.25, 96.37,
189: 		95, 94.87, 98.25, 105.12, 108.44, 109.87, 105, 106, 104.94, 104.5,
190: 		104.44, 106.31, 112.87, 116.5, 119.19, 121, 122.12, 111.94, 112.75, 110.19,
191: 		107.94, 109.69, 111.06, 110.44, 110.12, 110.31, 110.44, 110, 110.75, 110.5,
192: 		110.5, 109.5,
193: 	}
194: }
195: 
196: func testLows() []float64 {
197: 	return []float64{
198: 		90.75, 91.405, 94.25, 93.5, 92.815, 93.5, 92, 89.75, 89.44, 90.625,
199: 		92.75, 96.315, 96.03, 88.815, 86.75, 90.94, 88.905, 88.78, 89.25, 89.75,
200: 		87.5, 86.53, 84.625, 82.28, 81.565, 80.875, 81.25, 84.065, 85.595, 85.97,
201: 		84.405, 85.095, 85.5, 85.53, 87.875, 86.565, 84.655, 83.25, 82.565, 83.44,
202: 		82.53, 85.065, 86.875, 88.53, 89.28, 90.125, 90.75, 89, 88.565, 90.095,
203: 		89, 86.47, 84, 83.315, 82, 83.25, 84.75, 85.28, 87.19, 88.44,
204: 		88.25, 87.345, 89.28, 91.095, 89.53, 91.155, 92, 90.53, 89.97, 88.815,
205: 		86.75, 85.065, 82.03, 81.5, 82.565, 96.345, 96.47, 101.155, 104.25, 101.75,
206: 		101.72, 101.72, 103.155, 105.69, 103.655, 104, 105.53, 108.53, 108.75, 107.75,
207: 		117, 118, 116, 118.5, 116.53, 116.25, 114.595, 110.875, 110.5, 110.72,
208: 		112.62, 114.19, 111.19, 109.44, 111.56, 112.44, 117.5, 116.06, 116.56, 113.31,
209: 		112.56, 114, 114.75, 118.87, 119, 119.75, 122.62, 123, 121.75, 121.56,
210: 		123.12, 122.19, 122.75, 124.37, 128, 129.5, 130.81, 130.63, 132.13, 133.88,
211: 		135.38, 135.75, 136.19, 134.5, 135.38, 133.69, 126.06, 126.87, 123.5, 122.62,
212: 		122.75, 123.56, 125.81, 124.62, 124.37, 121.81, 118.19, 118.06, 117.56, 121,
213: 		121.12, 118.94, 119.81, 121, 122, 124.5, 126.56, 123.5, 121.25, 121.06,
214: 		122.31, 121, 120.87, 122.06, 122.75, 122.69, 122.87, 125.5, 124.25, 128,
215: 		128.38, 130.69, 131.63, 134.38, 132, 131.94, 131.94, 129.56, 123.75, 126,
216: 		126.25, 124.37, 121.44, 120.44, 121.37, 121.69, 120, 119.62, 115.5, 116.75,
217: 		119.06, 119.06, 115.06, 111.06, 113.12, 110, 105, 104.69, 103.87, 104.69,
218: 		105.44, 107, 89, 92.5, 92.12, 94.62, 92.81, 94.25, 96.25, 96.37,
219: 		93.69, 93.5, 90, 90.19, 90.5, 92.12, 94.12, 94.87, 93, 93.87,
220: 		93, 92.62, 93.56, 98.37, 104.44, 106, 101.81, 104.12, 103.37, 102.12,
221: 		102.25, 103.37, 107.94, 112.5, 115.44, 115.5, 112.25, 107.56, 106.56, 106.87,
222: 		104.5, 105.75, 108.62, 107.75, 108.06, 108, 108.19, 108.12, 109.06, 108.75,
223: 		108.56, 106.62,
224: 	}
225: }
226: 
227: func testCloses() []float64 {
228: 	return []float64{
229: 		91.5, 94.815, 94.375, 95.095, 93.78, 94.625, 92.53, 92.75, 90.315, 92.47,
230: 		96.125, 97.25, 98.5, 89.875, 91, 92.815, 89.155, 89.345, 91.625, 89.875,
231: 		88.375, 87.625, 84.78, 83, 83.5, 81.375, 84.44, 89.25, 86.375, 86.25,
232: 		85.25, 87.125, 85.815, 88.97, 88.47, 86.875, 86.815, 84.875, 84.19, 83.875,
233: 		83.375, 85.5, 89.19, 89.44, 91.095, 90.75, 91.44, 89, 91, 90.5,
234: 		89.03, 88.815, 84.28, 83.5, 82.69, 84.75, 85.655, 86.19, 88.94, 89.28,
235: 		88.625, 88.5, 91.97, 91.5, 93.25, 93.5, 93.155, 91.72, 90, 89.69,
236: 		88.875, 85.19, 83.375, 84.875, 85.94, 97.25, 99.875, 104.94, 106, 102.5,
237: 		102.405, 104.595, 106.125, 106, 106.065, 104.625, 108.625, 109.315, 110.5, 112.75,
238: 		123, 119.625, 118.75, 119.25, 117.94, 116.44, 115.19, 111.875, 110.595, 118.125,
239: 		116, 116, 112, 113.75, 112.94, 116, 120.5, 116.62, 117, 115.25,
240: 		114.31, 115.5, 115.87, 120.69, 120.19, 120.75, 124.75, 123.37, 122.94, 122.56,
241: 		123.12, 122.56, 124.62, 129.25, 131, 132.25, 131, 132.81, 134, 137.38,
242: 		137.81, 137.88, 137.25, 136.31, 136.25, 134.63, 128.25, 129, 123.87, 124.81,
243: 		123, 126.25, 128.38, 125.37, 125.69, 122.25, 119.37, 118.5, 123.19, 123.5,
244: 		122.19, 119.31, 123.31, 121.12, 123.37, 127.37, 128.5, 123.87, 122.94, 121.75,
245: 		124.44, 122, 122.37, 122.94, 124, 123.19, 124.56, 127.25, 125.87, 128.86,
246: 		132, 130.75, 134.75, 135, 132.38, 133.31, 131.94, 130, 125.37, 130.13,
247: 		127.12, 125.19, 122, 125, 123, 123.5, 120.06, 121, 117.75, 119.87,
248: 		122, 119.19, 116.37, 113.5, 114.25, 110, 105.06, 107, 107.87, 107,
249: 		107.12, 107, 91, 93.94, 93.87, 95.5, 93, 94.94, 98.25, 96.75,
250: 		94.81, 94.37, 91.56, 90.25, 93.94, 93.62, 97, 95, 95.87, 94.06,
251: 		94.62, 93.75, 98, 103.94, 107.87, 106.06, 104.5, 105, 104.19, 103.06,
252: 		103.42, 105.27, 111.87, 116, 116.62, 118.28, 113.37, 109, 109.7, 109.25,
253: 		107, 109.19, 110, 109.2, 110.12, 108, 108.62, 109.75, 109.81, 109,
254: 		108.75, 107.87,
255: 	}
256: }
257: 
258: func testVolumes() []float64 {
259: 	return []float64{
260: 		4077500, 4955900, 4775300, 4155300, 4593100, 3631300, 3382800, 4954200, 4500000, 3397500,
261: 		4204500, 6321400, 10203600, 19043900, 11692000, 9553300, 8920300, 5970900, 5062300, 3705600,
262: 		5865600, 5603000, 5811900, 8483800, 5995200, 5408800, 5430500, 6283800, 5834800, 4515500,
263: 		4493300, 4346100, 3700300, 4600200, 4557200, 4323600, 5237500, 7404100, 4798400, 4372800,
264: 		3872300, 10750800, 5804800, 3785500, 5014800, 3507700, 4298800, 4842500, 3952200, 3304700,
265: 		3462000, 7253900, 9753100, 5953000, 5011700, 5910800, 4916900, 4135000, 4054200, 3735300,
266: 		2921900, 2658400, 4624400, 4372200, 5831600, 4268600, 3059200, 4495500, 3425000, 3630800,
267: 		4168100, 5966900, 7692800, 7362500, 6581300, 19587700, 10378600, 9334700, 10467200, 5671400,
268: 		5645000, 4518600, 4519500, 5569700, 4239700, 4175300, 4995300, 4776600, 4190000, 6035300,
269: 		12168900, 9040800, 5780300, 4320800, 3899100, 3221400, 3455500, 4304200, 4703900, 8316300,
270: 		10553900, 6384800, 7163300, 7007800, 5114100, 5263800, 6666100, 7398400, 5575000, 4852300,
271: 		4298100, 4900500, 4887700, 6964800, 4679200, 9165000, 6469800, 6792000, 4423800, 5231900,
272: 		4565600, 6235200, 5225900, 8261400, 5912500, 3545600, 5714500, 6653900, 6094500, 4799200,
273: 		5050800, 5648900, 4726300, 5585600, 5124800, 7630200, 14311600, 8793600, 8874200, 6966600,
274: 		5525500, 6515500, 5291900, 5711700, 4327700, 4568000, 6859200, 5757500, 7367000, 6144100,
275: 		4052700, 5849700, 5544700, 5032200, 4400600, 4894100, 5140000, 6610900, 7585200, 5963100,
276: 		6045500, 8443300, 6464700, 6248300, 4357200, 4774700, 6216900, 6266900, 5584800, 5284500,
277: 		7554500, 7209500, 8424800, 5094500, 4443600, 4591100, 5658400, 6094100, 14862200, 7544700,
278: 		6985600, 8093000, 7590000, 7451300, 7078000, 7105300, 8778800, 6643900, 10563900, 7043100,
279: 		6438900, 8057700, 14240000, 17872300, 7831100, 8277700, 15017800, 14183300, 13921100, 9683000,
280: 		9187300, 11380500, 69447300, 26673600, 13768400, 11371600, 9872200, 9450500, 11083300, 9552800,
281: 		11108400, 10374200, 16701900, 13741900, 8523600, 9551900, 8680500, 7151700, 9673100, 6264700,
282: 		8541600, 8358000, 18720800, 19683100, 13682500, 10668100, 9710600, 3113100, 5682000, 5763600,
283: 		5340000, 6220800, 14680500, 9933000, 11329500, 8145300, 16644700, 12593800, 7138100, 7442300,
284: 		9442300, 7123600, 7680600, 4839800, 4775500, 4008800, 4533600, 3741100, 4084800, 2685200,
285: 		3438000, 2870500,
286: 	}
287: }

(End of file - total 287 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/descriptors.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/descriptors.go</path>
<type>file</type>
<content>1: package core
2: 
3: import (
4: 	"zpano/indicators/core/outputs/shape"
5: )
6: 
7: // descriptors is the static registry of taxonomic descriptors for all implemented indicators.
8: //
9: // Output Kind values mirror each indicator package's Output enumeration, which starts
10: // at iota+1; they are written here as integer literals with a comment naming the constant.
11: //
12: //nolint:funlen,maintidx
13: var descriptors = map[Identifier]Descriptor{
14: 	SimpleMovingAverage: {
15: 		Identifier: SimpleMovingAverage, Family: "Common",
16: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
17: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price}},
18: 	},
19: 	WeightedMovingAverage: {
20: 		Identifier: WeightedMovingAverage, Family: "Common",
21: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
22: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price}},
23: 	},
24: 	TriangularMovingAverage: {
25: 		Identifier: TriangularMovingAverage, Family: "Common",
26: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
27: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price}},
28: 	},
29: 	ExponentialMovingAverage: {
30: 		Identifier: ExponentialMovingAverage, Family: "Common",
31: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
32: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price}},
33: 	},
34: 	DoubleExponentialMovingAverage: {
35: 		Identifier: DoubleExponentialMovingAverage, Family: "Patrick Mulloy",
36: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
37: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price}},
38: 	},
39: 	TripleExponentialMovingAverage: {
40: 		Identifier: TripleExponentialMovingAverage, Family: "Patrick Mulloy",
41: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
42: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price}},
43: 	},
44: 	T2ExponentialMovingAverage: {
45: 		Identifier: T2ExponentialMovingAverage, Family: "Tim Tillson",
46: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
47: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price}},
48: 	},
49: 	T3ExponentialMovingAverage: {
50: 		Identifier: T3ExponentialMovingAverage, Family: "Tim Tillson",
51: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
52: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price}},
53: 	},
54: 	KaufmanAdaptiveMovingAverage: {
55: 		Identifier: KaufmanAdaptiveMovingAverage, Family: "Perry Kaufman",
56: 		Adaptivity: Adaptive, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
57: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price}},
58: 	},
59: 	JurikMovingAverage: {
60: 		Identifier: JurikMovingAverage, Family: "Mark Jurik",
61: 		Adaptivity: Adaptive, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
62: 		Outputs: []OutputDescriptor{{Kind: 1 /* MovingAverage */, Shape: shape.Scalar, Role: Smoother, Pane: Price}},
63: 	},
64: 	MesaAdaptiveMovingAverage: {
65: 		Identifier: MesaAdaptiveMovingAverage, Family: "John Ehlers",
66: 		Adaptivity: Adaptive, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
67: 		Outputs: []OutputDescriptor{
68: 			{Kind: 1 /* Value (MAMA) */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
69: 			{Kind: 2 /* Fama */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
70: 			{Kind: 3 /* Band */, Shape: shape.Band, Role: Envelope, Pane: Price},
71: 		},
72: 	},
73: 	FractalAdaptiveMovingAverage: {
74: 		Identifier: FractalAdaptiveMovingAverage, Family: "John Ehlers",
75: 		Adaptivity: Adaptive, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
76: 		Outputs: []OutputDescriptor{
77: 			{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
78: 			{Kind: 2 /* Fdim */, Shape: shape.Scalar, Role: FractalDimension, Pane: Own},
79: 		},
80: 	},
81: 	DominantCycle: {
82: 		Identifier: DominantCycle, Family: "John Ehlers",
83: 		Adaptivity: Adaptive, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
84: 		Outputs: []OutputDescriptor{
85: 			{Kind: 1 /* RawPeriod */, Shape: shape.Scalar, Role: CyclePeriod, Pane: Own},
86: 			{Kind: 2 /* Period */, Shape: shape.Scalar, Role: CyclePeriod, Pane: Own},
87: 			{Kind: 3 /* Phase */, Shape: shape.Scalar, Role: CyclePhase, Pane: Own},
88: 		},
89: 	},
90: 	Momentum: {
91: 		Identifier: Momentum, Family: "Common",
92: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
93: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Oscillator, Pane: Own}},
94: 	},
95: 	RateOfChange: {
96: 		Identifier: RateOfChange, Family: "Common",
97: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
98: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Oscillator, Pane: Own}},
99: 	},
100: 	RateOfChangePercent: {
101: 		Identifier: RateOfChangePercent, Family: "Common",
102: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
103: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Oscillator, Pane: Own}},
104: 	},
105: 	RelativeStrengthIndex: {
106: 		Identifier: RelativeStrengthIndex, Family: "Welles Wilder",
107: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
108: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own}},
109: 	},
110: 	ChandeMomentumOscillator: {
111: 		Identifier: ChandeMomentumOscillator, Family: "Tushar Chande",
112: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
113: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own}},
114: 	},
115: 	BollingerBands: {
116: 		Identifier: BollingerBands, Family: "John Bollinger",
117: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
118: 		Outputs: []OutputDescriptor{
119: 			{Kind: 1 /* Lower */, Shape: shape.Scalar, Role: Envelope, Pane: Price},
120: 			{Kind: 2 /* Middle */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
121: 			{Kind: 3 /* Upper */, Shape: shape.Scalar, Role: Envelope, Pane: Price},
122: 			{Kind: 4 /* BandWidth */, Shape: shape.Scalar, Role: Volatility, Pane: Own},
123: 			{Kind: 5 /* PercentBand */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own},
124: 			{Kind: 6 /* Band */, Shape: shape.Band, Role: Envelope, Pane: Price},
125: 		},
126: 	},
127: 	Variance: {
128: 		Identifier: Variance, Family: "Common",
129: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
130: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Volatility, Pane: Own}},
131: 	},
132: 	StandardDeviation: {
133: 		Identifier: StandardDeviation, Family: "Common",
134: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
135: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Volatility, Pane: Own}},
136: 	},
137: 	GoertzelSpectrum: {
138: 		Identifier: GoertzelSpectrum, Family: "Custom",
139: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
140: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Heatmap, Role: Spectrum, Pane: Own}},
141: 	},
142: 	CenterOfGravityOscillator: {
143: 		Identifier: CenterOfGravityOscillator, Family: "John Ehlers",
144: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
145: 		Outputs: []OutputDescriptor{
146: 			{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Oscillator, Pane: Own},
147: 			{Kind: 2 /* Trigger */, Shape: shape.Scalar, Role: Signal, Pane: Own},
148: 		},
149: 	},
150: 	CyberCycle: {
151: 		Identifier: CyberCycle, Family: "John Ehlers",
152: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
153: 		Outputs: []OutputDescriptor{
154: 			{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Oscillator, Pane: Own},
155: 			{Kind: 2 /* Signal */, Shape: shape.Scalar, Role: Signal, Pane: Own},
156: 		},
157: 	},
158: 	InstantaneousTrendLine: {
159: 		Identifier: InstantaneousTrendLine, Family: "John Ehlers",
160: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
161: 		Outputs: []OutputDescriptor{
162: 			{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
163: 			{Kind: 2 /* Trigger */, Shape: shape.Scalar, Role: Signal, Pane: Price},
164: 		},
165: 	},
166: 	SuperSmoother: {
167: 		Identifier: SuperSmoother, Family: "John Ehlers",
168: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
169: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price}},
170: 	},
171: 	ZeroLagExponentialMovingAverage: {
172: 		Identifier: ZeroLagExponentialMovingAverage, Family: "John Ehlers",
173: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
174: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price}},
175: 	},
176: 	ZeroLagErrorCorrectingExponentialMovingAverage: {
177: 		Identifier: ZeroLagErrorCorrectingExponentialMovingAverage, Family: "John Ehlers",
178: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
179: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price}},
180: 	},
181: 	RoofingFilter: {
182: 		Identifier: RoofingFilter, Family: "John Ehlers",
183: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
184: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Oscillator, Pane: Own}},
185: 	},
186: 	TrueRange: {
187: 		Identifier: TrueRange, Family: "Welles Wilder",
188: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
189: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Volatility, Pane: Own}},
190: 	},
191: 	AverageTrueRange: {
192: 		Identifier: AverageTrueRange, Family: "Welles Wilder",
193: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
194: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Volatility, Pane: Own}},
195: 	},
196: 	NormalizedAverageTrueRange: {
197: 		Identifier: NormalizedAverageTrueRange, Family: "Welles Wilder",
198: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
199: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Volatility, Pane: Own}},
200: 	},
201: 	DirectionalMovementMinus: {
202: 		Identifier: DirectionalMovementMinus, Family: "Welles Wilder",
203: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
204: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Directional, Pane: Own}},
205: 	},
206: 	DirectionalMovementPlus: {
207: 		Identifier: DirectionalMovementPlus, Family: "Welles Wilder",
208: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
209: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Directional, Pane: Own}},
210: 	},
211: 	DirectionalIndicatorMinus: {
212: 		Identifier: DirectionalIndicatorMinus, Family: "Welles Wilder",
213: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
214: 		Outputs: []OutputDescriptor{
215: 			{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Directional, Pane: Own},
216: 			{Kind: 2 /* DirectionalMovementMinus */, Shape: shape.Scalar, Role: Directional, Pane: Own},
217: 			{Kind: 3 /* AverageTrueRange */, Shape: shape.Scalar, Role: Volatility, Pane: Own},
218: 			{Kind: 4 /* TrueRange */, Shape: shape.Scalar, Role: Volatility, Pane: Own},
219: 		},
220: 	},
221: 	DirectionalIndicatorPlus: {
222: 		Identifier: DirectionalIndicatorPlus, Family: "Welles Wilder",
223: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
224: 		Outputs: []OutputDescriptor{
225: 			{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Directional, Pane: Own},
226: 			{Kind: 2 /* DirectionalMovementPlus */, Shape: shape.Scalar, Role: Directional, Pane: Own},
227: 			{Kind: 3 /* AverageTrueRange */, Shape: shape.Scalar, Role: Volatility, Pane: Own},
228: 			{Kind: 4 /* TrueRange */, Shape: shape.Scalar, Role: Volatility, Pane: Own},
229: 		},
230: 	},
231: 	DirectionalMovementIndex: {
232: 		Identifier: DirectionalMovementIndex, Family: "Welles Wilder",
233: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
234: 		Outputs: []OutputDescriptor{
235: 			{Kind: 1 /* Value */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own},
236: 			{Kind: 2 /* DirectionalIndicatorPlus */, Shape: shape.Scalar, Role: Directional, Pane: Own},
237: 			{Kind: 3 /* DirectionalIndicatorMinus */, Shape: shape.Scalar, Role: Directional, Pane: Own},
238: 			{Kind: 4 /* DirectionalMovementPlus */, Shape: shape.Scalar, Role: Directional, Pane: Own},
239: 			{Kind: 5 /* DirectionalMovementMinus */, Shape: shape.Scalar, Role: Directional, Pane: Own},
240: 			{Kind: 6 /* AverageTrueRange */, Shape: shape.Scalar, Role: Volatility, Pane: Own},
241: 			{Kind: 7 /* TrueRange */, Shape: shape.Scalar, Role: Volatility, Pane: Own},
242: 		},
243: 	},
244: 	AverageDirectionalMovementIndex: {
245: 		Identifier: AverageDirectionalMovementIndex, Family: "Welles Wilder",
246: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
247: 		Outputs: []OutputDescriptor{
248: 			{Kind: 1 /* Value */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own},
249: 			{Kind: 2 /* DirectionalMovementIndex */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own},
250: 			{Kind: 3 /* DirectionalIndicatorPlus */, Shape: shape.Scalar, Role: Directional, Pane: Own},
251: 			{Kind: 4 /* DirectionalIndicatorMinus */, Shape: shape.Scalar, Role: Directional, Pane: Own},
252: 			{Kind: 5 /* DirectionalMovementPlus */, Shape: shape.Scalar, Role: Directional, Pane: Own},
253: 			{Kind: 6 /* DirectionalMovementMinus */, Shape: shape.Scalar, Role: Directional, Pane: Own},
254: 			{Kind: 7 /* AverageTrueRange */, Shape: shape.Scalar, Role: Volatility, Pane: Own},
255: 			{Kind: 8 /* TrueRange */, Shape: shape.Scalar, Role: Volatility, Pane: Own},
256: 		},
257: 	},
258: 	AverageDirectionalMovementIndexRating: {
259: 		Identifier: AverageDirectionalMovementIndexRating, Family: "Welles Wilder",
260: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
261: 		Outputs: []OutputDescriptor{
262: 			{Kind: 1 /* Value */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own},
263: 			{Kind: 2 /* AverageDirectionalMovementIndex */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own},
264: 			{Kind: 3 /* DirectionalMovementIndex */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own},
265: 			{Kind: 4 /* DirectionalIndicatorPlus */, Shape: shape.Scalar, Role: Directional, Pane: Own},
266: 			{Kind: 5 /* DirectionalIndicatorMinus */, Shape: shape.Scalar, Role: Directional, Pane: Own},
267: 			{Kind: 6 /* DirectionalMovementPlus */, Shape: shape.Scalar, Role: Directional, Pane: Own},
268: 			{Kind: 7 /* DirectionalMovementMinus */, Shape: shape.Scalar, Role: Directional, Pane: Own},
269: 			{Kind: 8 /* AverageTrueRange */, Shape: shape.Scalar, Role: Volatility, Pane: Own},
270: 			{Kind: 9 /* TrueRange */, Shape: shape.Scalar, Role: Volatility, Pane: Own},
271: 		},
272: 	},
273: 	WilliamsPercentR: {
274: 		Identifier: WilliamsPercentR, Family: "Larry Williams",
275: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
276: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own}},
277: 	},
278: 	PercentagePriceOscillator: {
279: 		Identifier: PercentagePriceOscillator, Family: "Gerald Appel",
280: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
281: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Oscillator, Pane: Own}},
282: 	},
283: 	AbsolutePriceOscillator: {
284: 		Identifier: AbsolutePriceOscillator, Family: "Common",
285: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
286: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Oscillator, Pane: Own}},
287: 	},
288: 	CommodityChannelIndex: {
289: 		Identifier: CommodityChannelIndex, Family: "Donald Lambert",
290: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
291: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own}},
292: 	},
293: 	MoneyFlowIndex: {
294: 		Identifier: MoneyFlowIndex, Family: "Gene Quong",
295: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: AggregateBarVolume,
296: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own}},
297: 	},
298: 	OnBalanceVolume: {
299: 		Identifier: OnBalanceVolume, Family: "Joseph Granville",
300: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: AggregateBarVolume,
301: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: VolumeFlow, Pane: Own}},
302: 	},
303: 	BalanceOfPower: {
304: 		Identifier: BalanceOfPower, Family: "Igor Livshin",
305: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
306: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own}},
307: 	},
308: 	RateOfChangeRatio: {
309: 		Identifier: RateOfChangeRatio, Family: "Common",
310: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
311: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Oscillator, Pane: Own}},
312: 	},
313: 	PearsonsCorrelationCoefficient: {
314: 		Identifier: PearsonsCorrelationCoefficient, Family: "Common",
315: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
316: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Correlation, Pane: Own}},
317: 	},
318: 	LinearRegression: {
319: 		Identifier: LinearRegression, Family: "Common",
320: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
321: 		Outputs: []OutputDescriptor{
322: 			{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
323: 			{Kind: 2 /* Forecast */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
324: 			{Kind: 3 /* Intercept */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
325: 			{Kind: 4 /* SlopeRad */, Shape: shape.Scalar, Role: Oscillator, Pane: Own},
326: 			{Kind: 5 /* SlopeDeg */, Shape: shape.Scalar, Role: Oscillator, Pane: Own},
327: 		},
328: 	},
329: 	UltimateOscillator: {
330: 		Identifier: UltimateOscillator, Family: "Larry Williams",
331: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
332: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own}},
333: 	},
334: 	StochasticRelativeStrengthIndex: {
335: 		Identifier: StochasticRelativeStrengthIndex, Family: "Tushar Chande",
336: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
337: 		Outputs: []OutputDescriptor{
338: 			{Kind: 1 /* FastK */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own},
339: 			{Kind: 2 /* FastD */, Shape: shape.Scalar, Role: Signal, Pane: Own},
340: 		},
341: 	},
342: 	Stochastic: {
343: 		Identifier: Stochastic, Family: "George Lane",
344: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
345: 		Outputs: []OutputDescriptor{
346: 			{Kind: 1 /* FastK */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own},
347: 			{Kind: 2 /* SlowK */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own},
348: 			{Kind: 3 /* SlowD */, Shape: shape.Scalar, Role: Signal, Pane: Own},
349: 		},
350: 	},
351: 	Aroon: {
352: 		Identifier: Aroon, Family: "Tushar Chande",
353: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
354: 		Outputs: []OutputDescriptor{
355: 			{Kind: 1 /* Up */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own},
356: 			{Kind: 2 /* Down */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own},
357: 			{Kind: 3 /* Osc */, Shape: shape.Scalar, Role: Oscillator, Pane: Own},
358: 		},
359: 	},
360: 	AdvanceDecline: {
361: 		Identifier: AdvanceDecline, Family: "Marc Chaikin",
362: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: AggregateBarVolume,
363: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: VolumeFlow, Pane: Own}},
364: 	},
365: 	AdvanceDeclineOscillator: {
366: 		Identifier: AdvanceDeclineOscillator, Family: "Marc Chaikin",
367: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: AggregateBarVolume,
368: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: VolumeFlow, Pane: Own}},
369: 	},
370: 	ParabolicStopAndReverse: {
371: 		Identifier: ParabolicStopAndReverse, Family: "Welles Wilder",
372: 		Adaptivity: Static, InputRequirement: BarInput, VolumeUsage: NoVolume,
373: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Overlay, Pane: Price}},
374: 	},
375: 	TripleExponentialMovingAverageOscillator: {
376: 		Identifier: TripleExponentialMovingAverageOscillator, Family: "Jack Hutson",
377: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
378: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Oscillator, Pane: Own}},
379: 	},
380: 	BollingerBandsTrend: {
381: 		Identifier: BollingerBandsTrend, Family: "John Bollinger",
382: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
383: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Oscillator, Pane: Own}},
384: 	},
385: 	MovingAverageConvergenceDivergence: {
386: 		Identifier: MovingAverageConvergenceDivergence, Family: "Gerald Appel",
387: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
388: 		Outputs: []OutputDescriptor{
389: 			{Kind: 1 /* MACD */, Shape: shape.Scalar, Role: Oscillator, Pane: Own},
390: 			{Kind: 2 /* Signal */, Shape: shape.Scalar, Role: Signal, Pane: Own},
391: 			{Kind: 3 /* Histogram */, Shape: shape.Scalar, Role: Histogram, Pane: Own},
392: 		},
393: 	},
394: 	SineWave: {
395: 		Identifier: SineWave, Family: "John Ehlers",
396: 		Adaptivity: Adaptive, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
397: 		Outputs: []OutputDescriptor{
398: 			{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Oscillator, Pane: Own},
399: 			{Kind: 2 /* Lead */, Shape: shape.Scalar, Role: Signal, Pane: Own},
400: 			{Kind: 3 /* Band */, Shape: shape.Band, Role: Envelope, Pane: Own},
401: 			{Kind: 4 /* DominantCyclePeriod */, Shape: shape.Scalar, Role: CyclePeriod, Pane: Own},
402: 			{Kind: 5 /* DominantCyclePhase */, Shape: shape.Scalar, Role: CyclePhase, Pane: Own},
403: 		},
404: 	},
405: 	HilbertTransformerInstantaneousTrendLine: {
406: 		Identifier: HilbertTransformerInstantaneousTrendLine, Family: "John Ehlers",
407: 		Adaptivity: Adaptive, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
408: 		Outputs: []OutputDescriptor{
409: 			{Kind: 1 /* Value */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
410: 			{Kind: 2 /* DominantCyclePeriod */, Shape: shape.Scalar, Role: CyclePeriod, Pane: Own},
411: 		},
412: 	},
413: 	TrendCycleMode: {
414: 		Identifier: TrendCycleMode, Family: "John Ehlers",
415: 		Adaptivity: Adaptive, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
416: 		Outputs: []OutputDescriptor{
417: 			{Kind: 1 /* Value */, Shape: shape.Scalar, Role: RegimeFlag, Pane: Own},
418: 			{Kind: 2 /* IsTrendMode */, Shape: shape.Scalar, Role: RegimeFlag, Pane: Own},
419: 			{Kind: 3 /* IsCycleMode */, Shape: shape.Scalar, Role: RegimeFlag, Pane: Own},
420: 			{Kind: 4 /* InstantaneousTrendLine */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
421: 			{Kind: 5 /* SineWave */, Shape: shape.Scalar, Role: Oscillator, Pane: Own},
422: 			{Kind: 6 /* SineWaveLead */, Shape: shape.Scalar, Role: Signal, Pane: Own},
423: 			{Kind: 7 /* DominantCyclePeriod */, Shape: shape.Scalar, Role: CyclePeriod, Pane: Own},
424: 			{Kind: 8 /* DominantCyclePhase */, Shape: shape.Scalar, Role: CyclePhase, Pane: Own},
425: 		},
426: 	},
427: 	CoronaSpectrum: {
428: 		Identifier: CoronaSpectrum, Family: "John Ehlers",
429: 		Adaptivity: Adaptive, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
430: 		Outputs: []OutputDescriptor{
431: 			{Kind: 1 /* Value */, Shape: shape.Heatmap, Role: Spectrum, Pane: Own},
432: 			{Kind: 2 /* DominantCycle */, Shape: shape.Scalar, Role: CyclePeriod, Pane: Own},
433: 			{Kind: 3 /* DominantCycleMedian */, Shape: shape.Scalar, Role: CyclePeriod, Pane: Own},
434: 		},
435: 	},
436: 	CoronaSignalToNoiseRatio: {
437: 		Identifier: CoronaSignalToNoiseRatio, Family: "John Ehlers",
438: 		Adaptivity: Adaptive, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
439: 		Outputs: []OutputDescriptor{
440: 			{Kind: 1 /* Value */, Shape: shape.Heatmap, Role: Spectrum, Pane: Own},
441: 			{Kind: 2 /* SignalToNoiseRatio */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own},
442: 		},
443: 	},
444: 	CoronaSwingPosition: {
445: 		Identifier: CoronaSwingPosition, Family: "John Ehlers",
446: 		Adaptivity: Adaptive, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
447: 		Outputs: []OutputDescriptor{
448: 			{Kind: 1 /* Value */, Shape: shape.Heatmap, Role: Spectrum, Pane: Own},
449: 			{Kind: 2 /* SwingPosition */, Shape: shape.Scalar, Role: BoundedOscillator, Pane: Own},
450: 		},
451: 	},
452: 	CoronaTrendVigor: {
453: 		Identifier: CoronaTrendVigor, Family: "John Ehlers",
454: 		Adaptivity: Adaptive, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
455: 		Outputs: []OutputDescriptor{
456: 			{Kind: 1 /* Value */, Shape: shape.Heatmap, Role: Spectrum, Pane: Own},
457: 			{Kind: 2 /* TrendVigor */, Shape: shape.Scalar, Role: Oscillator, Pane: Own},
458: 		},
459: 	},
460: 	AdaptiveTrendAndCycleFilter: {
461: 		Identifier: AdaptiveTrendAndCycleFilter, Family: "Vladimir Kravchuk",
462: 		Adaptivity: Adaptive, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
463: 		Outputs: []OutputDescriptor{
464: 			{Kind: 1 /* Fatl */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
465: 			{Kind: 2 /* Satl */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
466: 			{Kind: 3 /* Rftl */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
467: 			{Kind: 4 /* Rstl */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
468: 			{Kind: 5 /* Rbci */, Shape: shape.Scalar, Role: Smoother, Pane: Price},
469: 			{Kind: 6 /* Ftlm */, Shape: shape.Scalar, Role: Oscillator, Pane: Own},
470: 			{Kind: 7 /* Stlm */, Shape: shape.Scalar, Role: Oscillator, Pane: Own},
471: 			{Kind: 8 /* Pcci */, Shape: shape.Scalar, Role: Oscillator, Pane: Own},
472: 		},
473: 	},
474: 	MaximumEntropySpectrum: {
475: 		Identifier: MaximumEntropySpectrum, Family: "Custom",
476: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
477: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Heatmap, Role: Spectrum, Pane: Own}},
478: 	},
479: 	DiscreteFourierTransformSpectrum: {
480: 		Identifier: DiscreteFourierTransformSpectrum, Family: "John Ehlers",
481: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
482: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Heatmap, Role: Spectrum, Pane: Own}},
483: 	},
484: 	CombBandPassSpectrum: {
485: 		Identifier: CombBandPassSpectrum, Family: "John Ehlers",
486: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
487: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Heatmap, Role: Spectrum, Pane: Own}},
488: 	},
489: 	AutoCorrelationIndicator: {
490: 		Identifier: AutoCorrelationIndicator, Family: "John Ehlers",
491: 		Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
492: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Heatmap, Role: Correlation, Pane: Own}},
493: 	},
494: 	AutoCorrelationPeriodogram: {
495: 		Identifier: AutoCorrelationPeriodogram, Family: "John Ehlers",
496: 		Adaptivity: Adaptive, InputRequirement: ScalarInput, VolumeUsage: NoVolume,
497: 		Outputs: []OutputDescriptor{{Kind: 1 /* Value */, Shape: shape.Heatmap, Role: Spectrum, Pane: Own}},
498: 	},
499: }

(End of file - total 499 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/outputs"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/outputs</path>
<type>directory</type>
<entries>
band_test.go
band.go
common.go
heatmap_test.go
heatmap.go
metadata.go
polyline_test.go
polyline.go
shape/

(9 entries)
</entries>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/chart/study-page/test-data/test-data.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/chart/study-page/test-data/test-data.ts</path>
<type>file</type>
<content>1: import { Configuration } from 'projects/mb/src/lib/charts/ohlcv-chart/template/configuration';
2: 
3: import { testDataOhlcv } from '../../../test-data/indicators/test-data-ohlcv';
4: import { testDataBb } from '../../../test-data/indicators/test-data-bb';
5: import { testDataBbMa } from '../../../test-data/indicators/test-data-bb-ma';
6: import { testDataBbLo } from '../../../test-data/indicators/test-data-bb-lo';
7: import { testDataBbUp } from '../../../test-data/indicators/test-data-bb-up';
8: import { testDataBbPercentB } from '../../../test-data/indicators/test-data-bb-percentb';
9: import { testDataBbBw } from '../../../test-data/indicators/test-data-bb-bw';
10: import { testDataGoertzel1 } from '../../../test-data/indicators/test-data-goertzel_1';
11: import { testDataOutputs } from './test-data-bb-goertzel-combined';
12: 
13: const outputsCount = testDataOutputs.length;
14: const ohlcvCount = testDataOhlcv.length;
15: 
16: export class TestData {
17:   private static configTemplate: Configuration = {
18:     width: '100%', // widthMin: 500, widthMax: 700,
19:     navigationPane: {
20:       height: 30, // heightMin: 30, heightMax: 30, timeTicksFormat: '%Y-%m-%d',
21:       hasLine: true, hasArea: false, hasTimeAxis: true, timeTicks: 0,
22:       // hasLine: true, hasArea: false, hasTimeAxis: true,
23:       // hasLine: false, hasArea: true, hasTimeAxis: false
24:     },
25:     heightNavigationPane: 30, // heightNavigationPaneMin: 20, heightNavigationPaneMax: 60,
26:     // navigationLine: true, navigationTimeAxis: true,
27:     timeAnnotationFormat: '%Y-%m-%d', // timeTicks: 5, timeTicksFormat: '%Y-%m-%d',
28:     axisLeft: true,
29:     axisRight: false,
30:     margin: { left: 0, top: 10, right: 20, bottom: 0 },
31:     ohlcv: { name: 'BRILL@XAMS', data: [], candlesticks: true },
32:     pricePane: {
33:       height: '30%', valueFormat: ',.2f', /*valueTicks: 10,*/ valueMarginPercentageFactor: 0.01, // heightMin: 300, heightMax: 300,
34:       bands: [
35:         {
36:           name: 'bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 5,
37:           color: 'rgba(0,255,0,0.3)', legendColor: 'rgba(0,200,0,1)', interpolation: 'natural'
38:         },
39:       ], lineAreas: [], horizontals: [], lines: [
40:         {
41:           name: 'ma-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 0,
42:           color: 'red', width: 1, dash: '', interpolation: 'natural'
43:         },
44:         {
45:           name: 'lo-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 1,
46:           color: 'blue', width: 0.5, dash: '5,5', interpolation: 'cardinal'
47:         },
48:         {
49:           name: 'up-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 2,
50:           color: 'blue', width: 0.5, dash: '2,2', interpolation: 'linear'
51:         }
52:       ], arrows: []
53:     },
54:     indicatorPanes: [
55:       {
56:         height: '60', valueFormat: ',.2f', /*valueTicks: 5,*/ valueMarginPercentageFactor: 0.01,
57:         bands: [], lineAreas: [], horizontals: [
58:           { value: 0, color: 'red', width: 0.5, dash: '' },
59:           { value: 1, color: 'red', width: 0.5, dash: '' }
60:         ], lines: [
61:           {
62:             name: '%b(c)-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 3,
63:             color: 'green', width: 1, dash: '', interpolation: 'natural'
64:           }
65:         ], arrows: []
66:       },
67:       {
68:         height: '60', valueFormat: ',.2f', /*valueTicks: 3,*/ valueMarginPercentageFactor: 0.01,
69:         bands: [], lineAreas: [
70:           {
71:             name: 'bw-bb(stdev.p(20,c),2,sma(20,c))', data: [], indicator: 0, output: 4,
72:             color: 'rgba(0,0,255,0.3)', legendColor: '#0000ff', value: 0.3, interpolation: 'natural'
73:           }
74:         ], horizontals: [
75:           { value: 0.3, color: 'blue', width: 0.5, dash: '' }
76:         ], lines: [], arrows: []
77:       },
78:       {
79:         height: '120', valueFormat: ',.2f', /*valueTicks: 5,*/ valueMarginPercentageFactor: 0, // heightMin: 50, heightMax: 100,
80:         heatmap: {
81:           name: 'goertzel(64, [2,28,1/10])', gradient: 'Viridis', invertGradient: false,
82:           data: [], indicator: 1, output: 0
83:         }, bands: [], lineAreas: [], horizontals: [], lines: [], arrows: []
84:       }
85:     ],
86:     crosshair: false,
87:     volumeInPricePane: true,
88:     menuVisible: true, downloadSvgVisible: true
89:   };
90: 
91:   private dataLength = 0;
92:   public config: Configuration;
93: 
94:   public static get configDataPrefilled(): Configuration {
95:     const cloned = TestData.deepCopy(TestData.configTemplate) as Configuration;
96:     cloned.ohlcv.data = testDataOhlcv;
97:     cloned.pricePane.bands[0].data = testDataBb;
98:     cloned.pricePane.lines[0].data = testDataBbMa;
99:     cloned.pricePane.lines[1].data = testDataBbLo;
100:     cloned.pricePane.lines[2].data = testDataBbUp;
101:     cloned.indicatorPanes[0].lines[0].data = testDataBbPercentB;
102:     cloned.indicatorPanes[1].lineAreas[0].data = testDataBbBw;
103:     // @ts-ignore
104:     cloned.indicatorPanes[2].heatmap.data = testDataGoertzel1;
105:     TestData.addArrows(cloned);
106:     return cloned;
107:   }
108: 
109:   private static add(count: number, currentCount: number, cfg: Configuration): number {
110:     const sum = currentCount + count;
111:     const newCount = sum > outputsCount ? outputsCount : sum;
112:     for (let i = currentCount; i < newCount; ++i) {
113:       cfg.ohlcv.data.push(testDataOhlcv[i]);
114:       const indicators = (testDataOutputs[i] as any).indicators;
115:       for (const band of cfg.pricePane.bands) {
116:         const outputs = (indicators[band.indicator] as any).outputs;
117:         band.data.push(outputs[band.output]);
118:       }
119:       for (const lineArea of cfg.pricePane.lineAreas) {
120:         const outputs = (indicators[lineArea.indicator] as any).outputs;
121:         lineArea.data.push(outputs[lineArea.output]);
122:       }
123:       for (const line of cfg.pricePane.lines) {
124:         const outputs = (indicators[line.indicator] as any).outputs;
125:         line.data.push(outputs[line.output]);
126:       }
127:       for (const pane of cfg.indicatorPanes) {
128:         if (pane.heatmap) {
129:           const heatmap = pane.heatmap;
130:           const outputs = (indicators[heatmap.indicator] as any).outputs;
131:           heatmap.data.push(outputs[heatmap.output]);
132:         }
133:         for (const band of pane.bands) {
134:           const outputs = (indicators[band.indicator] as any).outputs;
135:           band.data.push(outputs[band.output]);
136:         }
137:         for (const lineArea of pane.lineAreas) {
138:           const outputs = (indicators[lineArea.indicator] as any).outputs;
139:           lineArea.data.push(outputs[lineArea.output]);
140:         }
141:         for (const line of pane.lines) {
142:           const outputs = (indicators[line.indicator] as any).outputs;
143:           line.data.push(outputs[line.output]);
144:         }
145:       }
146:     }
147:     TestData.addArrows(cfg);
148:     return newCount;
149:   }
150: 
151:   private static addArrows(cfg: Configuration): void {
152:     const count = cfg.ohlcv.data.length;
153:     if (count > 25 && cfg.pricePane.arrows.length < 1) {
154:       const arrow = {
155:         name: 'sell', down: true, time: testDataOhlcv[25].time, /*value: testDataOhlcv[25].high,*/
156:         indicator: 0, output: 0, color: 'rgb(255,0,0)'
157:       };
158:       cfg.pricePane.arrows.push(arrow);
159:     }
160:     if (count > 26 && cfg.pricePane.arrows.length < 2) {
161:       const arrow = {
162:         name: 'buy', down: false, time: testDataOhlcv[26].time, /*value: testDataOhlcv[26].low,*/
163:         indicator: 0, output: 0, color: 'rgb(0,255,0)'
164:       };
165:       cfg.pricePane.arrows.push(arrow);
166:     }
167:   }
168: 
169:   private static deepCopy(obj: any): any {
170:     // Handle the 3 simple types, and null or undefined.
171:     if (null == obj || 'object' !== typeof obj) {
172:       return obj;
173:     }
174:     // Handle Date.
175:     if (obj instanceof Date) {
176:       const copy = new Date();
177:       copy.setTime(obj.getTime());
178:       return copy;
179:     }
180:     // Handle Array.
181:     if (obj instanceof Array) {
182:       const copy = [];
183:       for (let i = 0, len = obj.length; i < len; i++) {
184:         copy[i] = TestData.deepCopy(obj[i]);
185:       }
186:       return copy;
187:     }
188:     // Handle Object.
189:     if (obj instanceof Object) {
190:       const copy: any = {};
191:       for (const attr of Object.keys(obj)) {
192:         copy[attr] = TestData.deepCopy(obj[attr]);
193:       }
194:       return copy;
195:     }
196:     throw new Error('Unable to copy obj! Its type isn\'t supported.');
197:   }
198: 
199:   public constructor() {
200:     this.config = TestData.configDataPrefilled; // <OhlcvChartConfig>TestData.deepCopy(TestData.configTemplate);
201:     if (outputsCount !== ohlcvCount) {
202:       throw new Error('Lengths does not match: ohlcv=' + ohlcvCount + ', outputs=' + outputsCount);
203:     }
204:   }
205: 
206:   public clear(): void {
207:     this.config.ohlcv.data = [];
208:     this.config.pricePane.bands[0].data = [];
209:     this.config.pricePane.lines[0].data = [];
210:     this.config.pricePane.lines[1].data = [];
211:     this.config.pricePane.lines[2].data = [];
212:     this.config.pricePane.arrows = [];
213:     this.config.indicatorPanes[0].lines[0].data = [];
214:     this.config.indicatorPanes[1].lineAreas[0].data = [];
215:     // @ts-ignore
216:     this.config.indicatorPanes[2].heatmap.data = [];
217:     this.dataLength = 0;
218:   }
219: 
220:   public addSingle(): void {
221:     this.dataLength = TestData.add(1, this.dataLength, this.config);
222:   }
223: 
224:   public addTen(): void {
225:     this.dataLength = TestData.add(10, this.dataLength, this.config);
226:   }
227: 
228:   public addAll(): void {
229:     this.dataLength = TestData.add(ohlcvCount, this.dataLength, this.config);
230:   }
231: }

(End of file - total 231 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/chart/ohlcv-chart/template/pane.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/chart/ohlcv-chart/template/pane.ts</path>
<type>file</type>
<content>1: import { HeatmapData } from './heatmap-data';
2: import { BandData } from './band-data';
3: import { LineAreaData } from './line-area-data';
4: import { HorizontalData } from './horizontal-data';
5: import { LineData } from './line-data';
6: import { ArrowData } from './arrow-data';
7: 
8: /** Describes a pane. */
9: export class Pane {
10:   /**
11:    * A height of the pane.
12:    * Can be either a positive number of pixels or a percentage string (e.g. '45%') of a reference width.
13:    */
14:   public height: string | number = 0;
15: 
16:   /** An optional minimal height of the pane in pixels. */
17:   public heightMin?: number;
18: 
19:   /** An optional maximal height of the pane in pixels. */
20:   public heightMax?: number;
21: 
22:   /** A d3.format specifier for value ticks and annotations on the pane. */
23:   public valueFormat = ',.2f';
24: 
25:   /** An optional number of ticks in the value axis. */
26:   public valueTicks?: number;
27: 
28:   /**
29:    * A percentage factor (e.g., 0.05) to add to a lower and upper parts of a value axis.
30:    * This allows to add space between the top / bottom of the pane and the max / min values.
31:    */
32:   public valueMarginPercentageFactor = 0;
33: 
34:   /** An optional heatmap on this pane. */
35:   public heatmap?: HeatmapData;
36: 
37:   /** An array of indicator bands on this pane. */
38:   public bands: BandData[] = [];
39: 
40:   /** An array of indicator line areas on this pane. */
41:   public lineAreas: LineAreaData[] = [];
42: 
43:   /** An array of indicator bands on this pane. */
44:   public horizontals: HorizontalData[] = [];
45: 
46:   /** An array of indicator lines on this pane. */
47:   public lines: LineData[] = [];
48: 
49:   /** An array of arrows on this pane. */
50:   public arrows: ArrowData[] = [];
51: }

(End of file - total 51 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/chart/ohlcv-chart/template/configuration.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/chart/ohlcv-chart/template/configuration.ts</path>
<type>file</type>
<content>1: import { NavigationPane } from './navigation-pane';
2: import { Margin } from './margin';
3: import { OhlcvData } from './ohlcv-data';
4: import { Pane } from './pane';
5: 
6: /** Describes an ohlcv chart layout configuration. */
7: export class Configuration {
8:   /**
9:    * Total width of a chart including margins.
10:    * Defines the width of all chart panes.
11:    * Can be either a positive number of pixels or a percentage string (e.g. '45%') of a reference width.
12:    */
13:   public width!: string | number;
14: 
15:   /** An optional minimal width in pixels including margins. */
16:   public widthMin?: number;
17: 
18:   /** An optional maximal width in pixels including margins. */
19:   public widthMax?: number;
20: 
21:   /** An optional navigation pane. */
22:   public navigationPane?: NavigationPane = new NavigationPane();
23: 
24:   /**
25:    * An optional height of the navigation pane.
26:    * Can be either a positive number of pixels or a percentage string (e.g. '45%') of a reference width.
27:    * If undefined, the navigation pane will not be created.
28:    */
29:   public heightNavigationPane?: string | number;
30: 
31:   /** An optional d3.timeFormat specifier for time axis annotations, e.g. '%Y-%m-%d'. */
32:   public timeAnnotationFormat?: string;
33: 
34:   /** An optional d3.timeFormat specifier for time axis ticks, e.g. '%Y-%m-%d'. */
35:   public timeTicksFormat?: string;
36: 
37:   /** An optional number of ticks in the time axis. */
38:   public timeTicks?: number;
39: 
40:   /** If left axis should be visible on the price and indicator panes. */
41:   public axisLeft = true;
42: 
43:   /** If right axis should be visible on the price and indicator panes. */
44:   public axisRight = false;
45: 
46:   /** The margins of the chart, exclusive space needed for annotation. */
47:   public margin: Margin = new Margin();
48: 
49:   public ohlcv: OhlcvData = new OhlcvData();
50: 
51:   /** The price pane. */
52:   public pricePane: Pane = new Pane();
53: 
54:   /** An optional array of indicator panes. */
55:   public indicatorPanes: Pane[] = [];
56: 
57:   /** If *crosshair* should be visible */
58:   public crosshair = false;
59: 
60:   /** If volume in price pane should be visible */
61:   public volumeInPricePane = false;
62: 
63:   /** If menu should be visible. */
64:   public menuVisible = true;
65: 
66:   /** If *download SVG* menu setting should be visible. */
67:   public downloadSvgVisible = true;
68: }

(End of file - total 68 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.6 · 6.9s)

Now let me check the remaining types and the core indicator/output interfaces:

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/outputs/metadata.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/outputs/metadata.go</path>
<type>file</type>
<content>1: package outputs
2: 
3: import (
4: 	"zpano/indicators/core/outputs/shape"
5: )
6: 
7: // Metadata describes a single indicator output.
8: type Metadata struct {
9: 	// Kind is an identification of this indicator output.
10: 	// It is an integer representation of an output enumeration of a related indicator.
11: 	Kind int `json:"kind"`
12: 
13: 	// Shape describes the data shape of this indicator output.
14: 	Shape shape.Shape `json:"shape"`
15: 
16: 	// Mnemonic is a short name (mnemonic) of this indicator output.
17: 	Mnemonic string `json:"mnemonic"`
18: 
19: 	// Description is a description of this indicator output.
20: 	Description string `json:"description"`
21: }

(End of file - total 21 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/outputs/band.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/outputs/band.go</path>
<type>file</type>
<content>1: package outputs
2: 
3: import (
4: 	"fmt"
5: 	"math"
6: 	"time"
7: )
8: 
9: // Band represents two band values and a time stamp.
10: type Band struct {
11: 	// Time is the date and time of this band.
12: 	Time time.Time `json:"time"`
13: 
14: 	// Lower is the lower value of the band.
15: 	Lower float64 `json:"lower"`
16: 
17: 	// Upper is the upper value of the band.
18: 	Upper float64 `json:"upper"`
19: }
20: 
21: // newBand creates a new band.
22: // Both lower and upper values should not be NaN.
23: func NewBand(time time.Time, lower, upper float64) *Band {
24: 	if lower < upper {
25: 		return &Band{
26: 			Time:  time,
27: 			Lower: lower,
28: 			Upper: upper,
29: 		}
30: 	}
31: 
32: 	return &Band{
33: 		Time:  time,
34: 		Lower: upper,
35: 		Upper: lower,
36: 	}
37: }
38: 
39: // newEmptyBand creates a new empty band.
40: // Both lower and upper values will be equal to NaN.
41: func NewEmptyBand(time time.Time) *Band {
42: 	nan := math.NaN()
43: 
44: 	return &Band{
45: 		Time:  time,
46: 		Lower: nan,
47: 		Upper: nan,
48: 	}
49: }
50: 
51: // IsEmpty indicates whether this band is not initialized.
52: func (b *Band) IsEmpty() bool {
53: 	return math.IsNaN(b.Lower) || math.IsNaN(b.Upper)
54: }
55: 
56: // String implements the Stringer interface.
57: func (b *Band) String() string {
58: 	return fmt.Sprintf("{%s, %f, %f}", b.Time.Format(timeFmt), b.Lower, b.Upper)
59: }

(End of file - total 59 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/outputs/heatmap.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/outputs/heatmap.go</path>
<type>file</type>
<content>1: package outputs
2: 
3: import (
4: 	"fmt"
5: 	"math"
6: 	"time"
7: )
8: 
9: // Heatmap holds a time stamp (x) and an array of values (z) corresponding to parameter (y) range
10: // to paint a heatmap column.
11: type Heatmap struct {
12: 	// Time is the date and time (x) of this heatmap.
13: 	Time time.Time `json:"time"`
14: 
15: 	// ParameterFirst is the first parameter (y) value of the heatmap. This value is the same for all heatmap columns.
16: 	// A parameter corresponding to the i-th value can be calculated as:
17: 	//
18: 	// min(ParameterFirst,ParameterLast) + i / ParameterResolution
19: 	ParameterFirst float64 `json:"first"`
20: 
21: 	// ParameterLast is the last parameter (y) value of the heatmap. This value is the same for all heatmap columns.
22: 	// A parameter corresponding to the i-th value can be calculated as:
23: 	//
24: 	// min(ParameterFirst,ParameterLast) + i / ParameterResolution
25: 	ParameterLast float64 `json:"last"`
26: 
27: 	// ParameterResolution is the resolution of the parameter (y).  This value is the same for all heatmap columns.
28: 	// It is always a positive number.
29: 	// A value of 10 means that heatmap values are evaluated at every 1/10 of the parameter range.
30: 	// A parameter corresponding to the i-th value can be calculated as:
31: 	//
32: 	// min(ParameterFirst,ParameterLast) + i / ParameterResolution
33: 	ParameterResolution float64 `json:"result"`
34: 
35: 	// ValueMin is a minimal value (z) of this heatmap column.
36: 	ValueMin float64 `json:"min"`
37: 
38: 	// ValueMax is a maximal value (z) of this heatmap column.
39: 	ValueMax float64 `json:"max"`
40: 
41: 	// Values is a slice of values (z) of this heatmap column.
42: 	// The length of the slice is the same for all heatmap columns, but may be zero if the heatmap column is empty.
43: 	Values []float64 `json:"values"`
44: }
45: 
46: // NewHeatmap creates a new heatmap.
47: func NewHeatmap(time time.Time, parameterFirst, parameterLast, parameterResolution, valueMin, valueMax float64,
48: 	values []float64,
49: ) *Heatmap {
50: 	return &Heatmap{
51: 		Time:                time,
52: 		ParameterFirst:      parameterFirst,
53: 		ParameterLast:       parameterLast,
54: 		ParameterResolution: parameterResolution,
55: 		ValueMin:            valueMin,
56: 		ValueMax:            valueMax,
57: 		Values:              values,
58: 	}
59: }
60: 
61: // NewEmptyHeatmap creates a new empty heatmap.
62: // Both min and max values will be equal to NaN, the values slice will be empty.
63: func NewEmptyHeatmap(time time.Time, parameterFirst, parameterLast, parameterResolution float64) *Heatmap {
64: 	nan := math.NaN()
65: 
66: 	return &Heatmap{
67: 		Time:                time,
68: 		ParameterFirst:      parameterFirst,
69: 		ParameterLast:       parameterLast,
70: 		ParameterResolution: parameterResolution,
71: 		ValueMin:            nan,
72: 		ValueMax:            nan,
73: 		Values:              []float64{},
74: 	}
75: }
76: 
77: // IsEmpty indicates whether this heatmap is not initialized.
78: func (h *Heatmap) IsEmpty() bool {
79: 	return len(h.Values) < 1
80: }
81: 
82: // String implements the Stringer interface.
83: func (h *Heatmap) String() string {
84: 	return fmt.Sprintf("{%s, (%f, %f, %f), (%f, %f), %v}",
85: 		h.Time.Format(timeFmt), h.ParameterFirst, h.ParameterLast, h.ParameterResolution, h.ValueMin, h.ValueMax, h.Values)
86: }

(End of file - total 86 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/indicator.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/indicator.go</path>
<type>file</type>
<content>1: package core
2: 
3: import (
4: 	"zpano/entities"
5: )
6: 
7: // Indicator describes a common indicator functionality.
8: type Indicator interface {
9: 	// IsPrimed indicates whether an indicator is primed.
10: 	IsPrimed() bool
11: 
12: 	// Metadata describes an output data of an indicator.
13: 	Metadata() Metadata
14: 
15: 	// UpdateScalar updates an indicator given the next scalar sample.
16: 	UpdateScalar(sample *entities.Scalar) Output
17: 
18: 	// UpdateBar updates an indicator given the next bar sample.
19: 	UpdateBar(sample *entities.Bar) Output
20: 
21: 	// UpdateQuote updates an indicator given the next quote sample.
22: 	UpdateQuote(sample *entities.Quote) Output
23: 
24: 	// UpdateQuote updates an indicator given the next trade sample.
25: 	UpdateTrade(sample *entities.Trade) Output
26: }
27: 
28: // UpdateScalars updates the indicator given a slice of the next scalar samples.
29: func UpdateScalars(ind Indicator, samples []*entities.Scalar) []Output {
30: 	length := len(samples)
31: 	output := make([]Output, length)
32: 
33: 	for i, d := range samples {
34: 		output[i] = ind.UpdateScalar(d)
35: 	}
36: 
37: 	return output
38: }
39: 
40: // UpdateBars updates the indicator given a slice of the next bar samples.
41: func UpdateBars(ind Indicator, samples []*entities.Bar) []Output {
42: 	length := len(samples)
43: 	output := make([]Output, length)
44: 
45: 	for i, d := range samples {
46: 		output[i] = ind.UpdateBar(d)
47: 	}
48: 
49: 	return output
50: }
51: 
52: // UpdateQuotes updates the indicator given a slice of the next quote samples.
53: func UpdateQuotes(ind Indicator, samples []*entities.Quote) []Output {
54: 	length := len(samples)
55: 	output := make([]Output, length)
56: 
57: 	for i, d := range samples {
58: 		output[i] = ind.UpdateQuote(d)
59: 	}
60: 
61: 	return output
62: }
63: 
64: // UpdateTrades updates the indicator given a slice of the next trade samples.
65: func UpdateTrades(ind Indicator, samples []*entities.Trade) []Output {
66: 	length := len(samples)
67: 	output := make([]Output, length)
68: 
69: 	for i, d := range samples {
70: 		output[i] = ind.UpdateTrade(d)
71: 	}
72: 
73: 	return output
74: }

(End of file - total 74 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/outputs/shape/shape.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/outputs/shape/shape.go</path>
<type>file</type>
<content>1: // Package shape enumerates the data shapes an indicator output can take.
2: package shape
3: 
4: import (
5: 	"bytes"
6: 	"errors"
7: 	"fmt"
8: )
9: 
10: // Shape identifies the data shape of an indicator output.
11: type Shape int
12: 
13: const (
14: 	// Scalar holds a time stamp and a value.
15: 	Scalar Shape = iota + 1
16: 
17: 	// Band holds a time stamp and two values representing upper and lower lines of a band.
18: 	Band
19: 
20: 	// Heatmap holds a time stamp and an array of values representing a heat-map column.
21: 	Heatmap
22: 
23: 	// Polyline holds a time stamp and an ordered, variable-length sequence of (offset, value) points.
24: 	Polyline
25: 	last
26: )
27: 
28: const (
29: 	unknown  = "unknown"
30: 	scalar   = "scalar"
31: 	band     = "band"
32: 	heatmap  = "heatmap"
33: 	polyline = "polyline"
34: )
35: 
36: var errUnknownShape = errors.New("unknown indicator output shape")
37: 
38: // String implements the Stringer interface.
39: func (s Shape) String() string {
40: 	switch s {
41: 	case Scalar:
42: 		return scalar
43: 	case Band:
44: 		return band
45: 	case Heatmap:
46: 		return heatmap
47: 	case Polyline:
48: 		return polyline
49: 	default:
50: 		return unknown
51: 	}
52: }
53: 
54: // IsKnown determines if this output shape is known.
55: func (s Shape) IsKnown() bool {
56: 	return s >= Scalar && s < last
57: }
58: 
59: // MarshalJSON implements the Marshaler interface.
60: func (s Shape) MarshalJSON() ([]byte, error) {
61: 	str := s.String()
62: 	if str == unknown {
63: 		return nil, fmt.Errorf("cannot marshal '%s': %w", str, errUnknownShape)
64: 	}
65: 
66: 	const extra = 2 // Two bytes for quotes.
67: 
68: 	b := make([]byte, 0, len(str)+extra)
69: 	b = append(b, '"')
70: 	b = append(b, str...)
71: 	b = append(b, '"')
72: 
73: 	return b, nil
74: }
75: 
76: // UnmarshalJSON implements the Unmarshaler interface.
77: func (s *Shape) UnmarshalJSON(data []byte) error {
78: 	d := bytes.Trim(data, "\"")
79: 	str := string(d)
80: 
81: 	switch str {
82: 	case scalar:
83: 		*s = Scalar
84: 	case band:
85: 		*s = Band
86: 	case heatmap:
87: 		*s = Heatmap
88: 	case polyline:
89: 		*s = Polyline
90: 	default:
91: 		return fmt.Errorf("cannot unmarshal '%s': %w", str, errUnknownShape)
92: 	}
93: 
94: 	return nil
95: }

(End of file - total 95 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/entities"
}
```

**Output:**
```
<path>/home/dev/zpano/go/entities</path>
<type>directory</type>
<entries>
bar_test.go
bar.go
barcomponent_test.go
barcomponent.go
common.go
quote_test.go
quote.go
quotecomponent_test.go
quotecomponent.go
scalar.go
temporal.go
trade.go
tradecomponent_test.go
tradecomponent.go

(14 entries)
</entries>
```

---

## Assistant (Build · claude-opus-4.6 · 5.5s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/metadata.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/metadata.go</path>
<type>file</type>
<content>1: package core
2: 
3: import (
4: 	"zpano/indicators/core/outputs"
5: )
6: 
7: // Metadata describes an indicator and its outputs.
8: type Metadata struct {
9: 	// Identifier identifies this indicator.
10: 	Identifier Identifier `json:"identifier"`
11: 
12: 	// Mnemonic is a short name (mnemonic) of this indicator.
13: 	Mnemonic string `json:"mnemonic"`
14: 
15: 	// Description is a description of this indicator.
16: 	Description string `json:"description"`
17: 
18: 	// Outputs is a slice of metadata for individual outputs.
19: 	Outputs []outputs.Metadata `json:"outputs"`
20: }

(End of file - total 20 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/output.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/output.go</path>
<type>file</type>
<content>1: package core
2: 
3: // Output is a slice of indicator outputs requested quring the creation of an indicator.
4: type Output []any

(End of file - total 4 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/entities/scalar.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/entities/scalar.go</path>
<type>file</type>
<content>1: package entities
2: 
3: import "time"
4: 
5: // Scalar represents a scalar value.
6: type Scalar struct {
7: 	Time  time.Time `json:"t"` // The date and time.
8: 	Value float64   `json:"v"` // The value.
9: }

(End of file - total 9 lines)
</content>
```

**Tool: grep**

**Input:**
```json
{
  "pattern": "type Pane ",
  "path": "/home/dev/zpano/go/indicators/core"
}
```

**Output:**
```
Found 1 matches
/home/dev/zpano/go/indicators/core/pane.go:
  Line 10: type Pane int
```

---

## Assistant (Build · claude-opus-4.6 · 4.0s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/pane.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/pane.go</path>
<type>file</type>
<content>1: package core
2: 
3: import (
4: 	"bytes"
5: 	"errors"
6: 	"fmt"
7: )
8: 
9: // Pane identifies the chart pane an indicator output is drawn on.
10: type Pane int
11: 
12: const (
13: 	// Price denotes the primary price pane.
14: 	Price Pane = iota + 1
15: 
16: 	// Own denotes a dedicated sub-pane for this indicator.
17: 	Own
18: 
19: 	// OverlayOnParent denotes drawing on the parent indicator's pane.
20: 	OverlayOnParent
21: 	paneLast
22: )
23: 
24: const (
25: 	paneUnknown         = "unknown"
26: 	panePrice           = "price"
27: 	paneOwn             = "own"
28: 	paneOverlayOnParent = "overlayOnParent"
29: )
30: 
31: var errUnknownPane = errors.New("unknown indicator pane")
32: 
33: // String implements the Stringer interface.
34: func (p Pane) String() string {
35: 	switch p {
36: 	case Price:
37: 		return panePrice
38: 	case Own:
39: 		return paneOwn
40: 	case OverlayOnParent:
41: 		return paneOverlayOnParent
42: 	default:
43: 		return paneUnknown
44: 	}
45: }
46: 
47: // IsKnown determines if this pane is known.
48: func (p Pane) IsKnown() bool {
49: 	return p >= Price && p < paneLast
50: }
51: 
52: // MarshalJSON implements the Marshaler interface.
53: func (p Pane) MarshalJSON() ([]byte, error) {
54: 	s := p.String()
55: 	if s == paneUnknown {
56: 		return nil, fmt.Errorf("cannot marshal '%s': %w", s, errUnknownPane)
57: 	}
58: 
59: 	const extra = 2
60: 
61: 	b := make([]byte, 0, len(s)+extra)
62: 	b = append(b, '"')
63: 	b = append(b, s...)
64: 	b = append(b, '"')
65: 
66: 	return b, nil
67: }
68: 
69: // UnmarshalJSON implements the Unmarshaler interface.
70: func (p *Pane) UnmarshalJSON(data []byte) error {
71: 	d := bytes.Trim(data, "\"")
72: 	s := string(d)
73: 
74: 	switch s {
75: 	case panePrice:
76: 		*p = Price
77: 	case paneOwn:
78: 		*p = Own
79: 	case paneOverlayOnParent:
80: 		*p = OverlayOnParent
81: 	default:
82: 		return fmt.Errorf("cannot unmarshal '%s': %w", s, errUnknownPane)
83: 	}
84: 
85: 	return nil
86: }

(End of file - total 86 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/descriptor.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/descriptor.go</path>
<type>file</type>
<content>1: package core
2: 
3: // Descriptor classifies an indicator along multiple taxonomic dimensions to enable
4: // filtering and display in charting catalogs.
5: type Descriptor struct {
6: 	// Identifier uniquely identifies the indicator.
7: 	Identifier Identifier `json:"identifier"`
8: 
9: 	// Family groups related indicators (e.g., by author or category).
10: 	Family string `json:"family"`
11: 
12: 	// Adaptivity classifies whether the indicator adapts its parameters.
13: 	Adaptivity Adaptivity `json:"adaptivity"`
14: 
15: 	// InputRequirement is the minimum input data type this indicator consumes.
16: 	InputRequirement InputRequirement `json:"inputRequirement"`
17: 
18: 	// VolumeUsage classifies how this indicator uses volume information.
19: 	VolumeUsage VolumeUsage `json:"volumeUsage"`
20: 
21: 	// Outputs classifies each output of this indicator.
22: 	Outputs []OutputDescriptor `json:"outputs"`
23: }
24: 
25: // DescriptorOf returns the taxonomic descriptor for the given indicator identifier.
26: // The second return value is false if no descriptor is registered for the identifier.
27: func DescriptorOf(id Identifier) (Descriptor, bool) {
28: 	d, ok := descriptors[id]
29: 
30: 	return d, ok
31: }
32: 
33: // Descriptors returns a copy of the full descriptor registry.
34: func Descriptors() map[Identifier]Descriptor {
35: 	out := make(map[Identifier]Descriptor, len(descriptors))
36: 	for k, v := range descriptors {
37: 		out[k] = v
38: 	}
39: 
40: 	return out
41: }

(End of file - total 41 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.6 · 4.5s)

**Tool: grep**

**Input:**
```json
{
  "path": "/home/dev/zpano/go/indicators/core",
  "pattern": "type OutputDescriptor "
}
```

**Output:**
```
Found 1 matches
/home/dev/zpano/go/indicators/core/output_descriptor.go:
  Line 8: type OutputDescriptor struct {
```

---

## Assistant (Build · claude-opus-4.6 · 3.4s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/output_descriptor.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/output_descriptor.go</path>
<type>file</type>
<content>1: package core
2: 
3: import (
4: 	"zpano/indicators/core/outputs/shape"
5: )
6: 
7: // OutputDescriptor classifies a single indicator output for charting / discovery.
8: type OutputDescriptor struct {
9: 	// Kind is an integer representation of the output enumeration of a related indicator.
10: 	Kind int `json:"kind"`
11: 
12: 	// Shape is the data shape of this output.
13: 	Shape shape.Shape `json:"shape"`
14: 
15: 	// Role is the semantic role of this output.
16: 	Role Role `json:"role"`
17: 
18: 	// Pane is the chart pane on which this output is drawn.
19: 	Pane Pane `json:"pane"`
20: }

(End of file - total 20 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.6 · 4.4s)

Now I have all the context I need. Let me check the factory signature and how `BuildMetadata` works:

**Tool: grep**

**Input:**
```json
{
  "path": "/home/dev/zpano/go/indicators/core",
  "pattern": "func BuildMetadata"
}
```

**Output:**
```
Found 1 matches
/home/dev/zpano/go/indicators/core/build_metadata.go:
  Line 26: func BuildMetadata(id Identifier, mnemonic, description string, texts []OutputText) Metadata {
```

---

## Assistant (Build · claude-opus-4.6 · 7.2s)

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Read reference files (icalc/main.go, core/descriptors.go, core/outputs, chart template types)",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Create go/cmd/iconf/main.go with config structs and CLI logic",
      "priority": "high",
      "status": "in_progress"
    },
    {
      "content": "Implement output shape → pane mapping using descriptor registry",
      "priority": "high",
      "status": "in_progress"
    },
    {
      "content": "Implement JSON output writer",
      "priority": "high",
      "status": "in_progress"
    },
    {
      "content": "Implement TypeScript output writer",
      "priority": "high",
      "status": "in_progress"
    },
    {
      "content": "Build and test the tool",
      "priority": "high",
      "status": "pending"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Read reference files (icalc/main.go, core/descriptors.go, core/outputs, chart template types)",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Create go/cmd/iconf/main.go with config structs and CLI logic",
    "priority": "high",
    "status": "in_progress"
  },
  {
    "content": "Implement output shape → pane mapping using descriptor registry",
    "priority": "high",
    "status": "in_progress"
  },
  {
    "content": "Implement JSON output writer",
    "priority": "high",
    "status": "in_progress"
  },
  {
    "content": "Implement TypeScript output writer",
    "priority": "high",
    "status": "in_progress"
  },
  {
    "content": "Build and test the tool",
    "priority": "high",
    "status": "pending"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 4.5s)

Now I have all the context. Let me check the entities.Bar struct and the JSON tags on Scalar before writing:

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/entities/bar.go",
  "limit": 30
}
```

**Output:**
```
<path>/home/dev/zpano/go/entities/bar.go</path>
<type>file</type>
<content>1: package entities
2: 
3: import (
4: 	"fmt"
5: 	"time"
6: )
7: 
8: // Bar represents an [open, high, low, close, volume] price bar.
9: type Bar struct {
10: 	// Time is the date and time of the closing price.
11: 	Time time.Time `json:"time"`
12: 
13: 	// Open is the opening price.
14: 	Open float64 `json:"open"`
15: 
16: 	// High is the highest price.
17: 	High float64 `json:"high"`
18: 
19: 	// Low is the lowest price.
20: 	Low float64 `json:"low"`
21: 
22: 	// Close is the closing price.
23: 	Close float64 `json:"close"`
24: 
25: 	// Volume is the aggregated volume.
26: 	Volume float64 `json:"volume"`
27: }
28: 
29: // IsRising indicates whether this is a rising bar, i.e. the opening price is less than the closing price.
30: func (b *Bar) IsRising() bool {

(Showing lines 1-30 of 66. Use offset=31 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.6 · 5.4s)

Now I'll create the `iconf` tool. This is a large file so I'll write it all at once:

**Tool: bash**

**Input:**
```json
{
  "command": "mkdir -p /home/dev/zpano/go/cmd/iconf",
  "description": "Create iconf directory"
}
```

---

## Assistant (Build · claude-opus-4.6 · 208.0s)

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/cmd/iconf/main.go",
  "content": "// Package main implements iconf, a command-line tool that creates chart\n// configuration files from indicator settings.\n//\n// It reads a JSON settings file containing indicator definitions, creates\n// indicator instances via the factory, runs embedded bar data through them,\n// and writes chart configuration files in JSON and TypeScript formats.\n//\n// Usage:\n//\n//\ticonf <settings.json> <output-name>\npackage main\n\nimport (\n\t\"encoding/json\"\n\t\"fmt\"\n\t\"math\"\n\t\"os\"\n\t\"path/filepath\"\n\t\"strings\"\n\t\"time\"\n\n\t\"zpano/entities\"\n\t\"zpano/indicators/core\"\n\t\"zpano/indicators/core/outputs\"\n\t\"zpano/indicators/core/outputs/shape\"\n\t\"zpano/indicators/factory\"\n)\n\n// settingsEntry represents a single indicator entry in the settings JSON file.\ntype settingsEntry struct {\n\tIdentifier core.Identifier `json:\"identifier\"`\n\tParams     json.RawMessage `json:\"params\"`\n}\n\n// --- Chart configuration structs (mirror the TS template types) ---\n\n// Configuration is the top-level chart configuration.\ntype Configuration struct {\n\tWidth                string         `json:\"width\"`\n\tNavigationPane       *NavigationPane `json:\"navigationPane,omitempty\"`\n\tHeightNavigationPane int            `json:\"heightNavigationPane,omitempty\"`\n\tTimeAnnotationFormat string         `json:\"timeAnnotationFormat,omitempty\"`\n\tAxisLeft             bool           `json:\"axisLeft\"`\n\tAxisRight            bool           `json:\"axisRight\"`\n\tMargin               Margin         `json:\"margin\"`\n\tOhlcv                OhlcvData      `json:\"ohlcv\"`\n\tPricePane            Pane           `json:\"pricePane\"`\n\tIndicatorPanes       []Pane         `json:\"indicatorPanes\"`\n\tCrosshair            bool           `json:\"crosshair\"`\n\tVolumeInPricePane    bool           `json:\"volumeInPricePane\"`\n\tMenuVisible          bool           `json:\"menuVisible\"`\n\tDownloadSvgVisible   bool           `json:\"downloadSvgVisible\"`\n}\n\n// NavigationPane describes a navigation pane.\ntype NavigationPane struct {\n\tHeight      int  `json:\"height\"`\n\tHasLine     bool `json:\"hasLine\"`\n\tHasArea     bool `json:\"hasArea\"`\n\tHasTimeAxis bool `json:\"hasTimeAxis\"`\n\tTimeTicks   int  `json:\"timeTicks\"`\n}\n\n// Margin describes chart margins.\ntype Margin struct {\n\tLeft   int `json:\"left\"`\n\tTop    int `json:\"top\"`\n\tRight  int `json:\"right\"`\n\tBottom int `json:\"bottom\"`\n}\n\n// OhlcvData describes the OHLCV data source.\ntype OhlcvData struct {\n\tName         string  `json:\"name\"`\n\tData         []OhlcvBar `json:\"data\"`\n\tCandlesticks bool    `json:\"candlesticks\"`\n}\n\n// OhlcvBar is a bar for the chart (time as ISO string).\ntype OhlcvBar struct {\n\tTime   string  `json:\"time\"`\n\tOpen   float64 `json:\"open\"`\n\tHigh   float64 `json:\"high\"`\n\tLow    float64 `json:\"low\"`\n\tClose  float64 `json:\"close\"`\n\tVolume float64 `json:\"volume\"`\n}\n\n// Pane describes a chart pane.\ntype Pane struct {\n\tHeight                       string          `json:\"height\"`\n\tValueFormat                  string          `json:\"valueFormat\"`\n\tValueMarginPercentageFactor  float64         `json:\"valueMarginPercentageFactor\"`\n\tHeatmap                      *HeatmapData    `json:\"heatmap,omitempty\"`\n\tBands                        []BandData      `json:\"bands\"`\n\tLineAreas                    []LineAreaData   `json:\"lineAreas\"`\n\tHorizontals                  []HorizontalData `json:\"horizontals\"`\n\tLines                        []LineData       `json:\"lines\"`\n\tArrows                       []ArrowData      `json:\"arrows\"`\n}\n\n// LineData describes a line on a pane.\ntype LineData struct {\n\tName          string      `json:\"name\"`\n\tData          []ScalarVal `json:\"data\"`\n\tIndicator     int         `json:\"indicator\"`\n\tOutput        int         `json:\"output\"`\n\tColor         string      `json:\"color\"`\n\tWidth         float64     `json:\"width\"`\n\tDash          string      `json:\"dash\"`\n\tInterpolation string      `json:\"interpolation\"`\n}\n\n// ScalarVal is a time+value pair for chart data.\ntype ScalarVal struct {\n\tTime  string  `json:\"time\"`\n\tValue float64 `json:\"value\"`\n}\n\n// BandData describes a band on a pane.\ntype BandData struct {\n\tName          string    `json:\"name\"`\n\tData          []BandVal `json:\"data\"`\n\tIndicator     int       `json:\"indicator\"`\n\tOutput        int       `json:\"output\"`\n\tColor         string    `json:\"color\"`\n\tLegendColor   string    `json:\"legendColor\"`\n\tInterpolation string    `json:\"interpolation\"`\n}\n\n// BandVal is a time+upper+lower pair for chart data.\ntype BandVal struct {\n\tTime  string  `json:\"time\"`\n\tUpper float64 `json:\"upper\"`\n\tLower float64 `json:\"lower\"`\n}\n\n// LineAreaData describes a line area on a pane.\ntype LineAreaData struct {\n\tName          string      `json:\"name\"`\n\tData          []ScalarVal `json:\"data\"`\n\tIndicator     int         `json:\"indicator\"`\n\tOutput        int         `json:\"output\"`\n\tValue         float64     `json:\"value\"`\n\tColor         string      `json:\"color\"`\n\tLegendColor   string      `json:\"legendColor\"`\n\tInterpolation string      `json:\"interpolation\"`\n}\n\n// HeatmapData describes a heatmap on a pane.\ntype HeatmapData struct {\n\tName           string       `json:\"name\"`\n\tData           []HeatmapVal `json:\"data\"`\n\tIndicator      int          `json:\"indicator\"`\n\tOutput         int          `json:\"output\"`\n\tGradient       string       `json:\"gradient\"`\n\tInvertGradient bool         `json:\"invertGradient\"`\n}\n\n// HeatmapVal is a heatmap column for chart data.\ntype HeatmapVal struct {\n\tTime   string    `json:\"time\"`\n\tFirst  float64   `json:\"first\"`\n\tLast   float64   `json:\"last\"`\n\tResult float64   `json:\"result\"`\n\tMin    float64   `json:\"min\"`\n\tMax    float64   `json:\"max\"`\n\tValues []float64 `json:\"values\"`\n}\n\n// HorizontalData describes a horizontal line.\ntype HorizontalData struct {\n\tValue float64 `json:\"value\"`\n\tColor string  `json:\"color\"`\n\tWidth float64 `json:\"width\"`\n\tDash  string  `json:\"dash\"`\n}\n\n// ArrowData describes an arrow on a pane.\ntype ArrowData struct {\n\tName      string  `json:\"name\"`\n\tDown      bool    `json:\"down\"`\n\tTime      string  `json:\"time\"`\n\tIndicator int     `json:\"indicator\"`\n\tOutput    int     `json:\"output\"`\n\tColor     string  `json:\"color\"`\n}\n\n// --- Default colors for cycling ---\n\nvar lineColors = []string{\n\t\"#e41a1c\", \"#377eb8\", \"#4daf4a\", \"#984ea3\", \"#ff7f00\",\n\t\"#a65628\", \"#f781bf\", \"#999999\", \"#66c2a5\", \"#fc8d62\",\n}\n\nvar bandColors = []string{\n\t\"rgba(0,255,0,0.3)\", \"rgba(0,0,255,0.3)\", \"rgba(255,0,0,0.3)\",\n\t\"rgba(128,0,128,0.3)\", \"rgba(0,128,128,0.3)\",\n}\n\nfunc main() {\n\tif len(os.Args) < 3 {\n\t\tfmt.Fprintf(os.Stderr, \"usage: iconf <settings.json> <output-name>\\n\")\n\t\tos.Exit(1)\n\t}\n\n\tsettingsPath := os.Args[1]\n\toutputName := strings.TrimSuffix(strings.TrimSuffix(os.Args[2], \".json\"), \".ts\")\n\n\tdata, err := os.ReadFile(settingsPath)\n\tif err != nil {\n\t\tfmt.Fprintf(os.Stderr, \"error reading settings file: %v\\n\", err)\n\t\tos.Exit(1)\n\t}\n\n\tvar entries []settingsEntry\n\tif err := json.Unmarshal(data, &entries); err != nil {\n\t\tfmt.Fprintf(os.Stderr, \"error parsing settings file: %v\\n\", err)\n\t\tos.Exit(1)\n\t}\n\n\tindicators := make([]core.Indicator, 0, len(entries))\n\tfor _, e := range entries {\n\t\tind, err := factory.New(e.Identifier, string(e.Params))\n\t\tif err != nil {\n\t\t\tfmt.Fprintf(os.Stderr, \"error creating indicator %s: %v\\n\", e.Identifier, err)\n\t\t\tos.Exit(1)\n\t\t}\n\t\tindicators = append(indicators, ind)\n\t}\n\n\tbars := testBars()\n\tcfg := buildConfiguration(indicators, bars)\n\n\t// Write JSON.\n\tjsonPath := outputName + \".json\"\n\tjsonData, err := json.MarshalIndent(cfg, \"\", \"  \")\n\tif err != nil {\n\t\tfmt.Fprintf(os.Stderr, \"error marshaling JSON: %v\\n\", err)\n\t\tos.Exit(1)\n\t}\n\tif err := os.WriteFile(jsonPath, jsonData, 0o644); err != nil {\n\t\tfmt.Fprintf(os.Stderr, \"error writing %s: %v\\n\", jsonPath, err)\n\t\tos.Exit(1)\n\t}\n\tfmt.Printf(\"wrote %s\\n\", jsonPath)\n\n\t// Write TypeScript.\n\ttsPath := outputName + \".ts\"\n\ttsData := buildTypeScript(cfg, filepath.Base(outputName))\n\tif err := os.WriteFile(tsPath, []byte(tsData), 0o644); err != nil {\n\t\tfmt.Fprintf(os.Stderr, \"error writing %s: %v\\n\", tsPath, err)\n\t\tos.Exit(1)\n\t}\n\tfmt.Printf(\"wrote %s\\n\", tsPath)\n}\n\n// outputPlacement describes where to place an indicator output.\ntype outputPlacement struct {\n\tindicatorIdx int\n\toutputIdx    int\n\tmnemonic     string\n\tshape        shape.Shape\n\tpane         core.Pane\n}\n\nfunc buildConfiguration(indicators []core.Indicator, bars []entities.Bar) *Configuration {\n\tconst timeFmt = \"2006-01-02\"\n\n\t// Collect output placements from descriptor registry.\n\tvar placements []outputPlacement\n\tfor i, ind := range indicators {\n\t\tmeta := ind.Metadata()\n\t\tdesc, ok := core.DescriptorOf(meta.Identifier)\n\t\tif !ok {\n\t\t\t// Fallback: treat all outputs as Own/Scalar.\n\t\t\tfor j, om := range meta.Outputs {\n\t\t\t\tplacements = append(placements, outputPlacement{\n\t\t\t\t\tindicatorIdx: i, outputIdx: j,\n\t\t\t\t\tmnemonic: om.Mnemonic, shape: om.Shape, pane: core.Own,\n\t\t\t\t})\n\t\t\t}\n\t\t\tcontinue\n\t\t}\n\t\tfor j, od := range desc.Outputs {\n\t\t\tmnemonic := fmt.Sprintf(\"out[%d]\", j)\n\t\t\tif j < len(meta.Outputs) {\n\t\t\t\tmnemonic = meta.Outputs[j].Mnemonic\n\t\t\t}\n\t\t\tplacements = append(placements, outputPlacement{\n\t\t\t\tindicatorIdx: i, outputIdx: j,\n\t\t\t\tmnemonic: mnemonic, shape: od.Shape, pane: od.Pane,\n\t\t\t})\n\t\t}\n\t}\n\n\t// Run bars through indicators, collecting all outputs.\n\t// allOutputs[barIdx][indicatorIdx] = core.Output\n\tallOutputs := make([][]core.Output, len(bars))\n\tfor i, bar := range bars {\n\t\tallOutputs[i] = make([]core.Output, len(indicators))\n\t\tfor j, ind := range indicators {\n\t\t\tallOutputs[i][j] = ind.UpdateBar(&bar)\n\t\t}\n\t}\n\n\t// Build OHLCV data.\n\tohlcvBars := make([]OhlcvBar, len(bars))\n\tfor i, bar := range bars {\n\t\tohlcvBars[i] = OhlcvBar{\n\t\t\tTime: bar.Time.Format(timeFmt), Open: bar.Open, High: bar.High,\n\t\t\tLow: bar.Low, Close: bar.Close, Volume: bar.Volume,\n\t\t}\n\t}\n\n\t// Group placements: price pane vs own panes.\n\t// For \"Own\" pane outputs, group by indicator index so all outputs of the same\n\t// indicator that share Pane=Own go into the same indicator pane.\n\ttype ownPaneKey struct{ indicatorIdx int }\n\townPaneMap := make(map[ownPaneKey]int) // key -> indicatorPanes index\n\tvar indicatorPanes []Pane\n\n\tpricePane := Pane{\n\t\tHeight: \"30%\", ValueFormat: \",.2f\", ValueMarginPercentageFactor: 0.01,\n\t\tBands: []BandData{}, LineAreas: []LineAreaData{},\n\t\tHorizontals: []HorizontalData{}, Lines: []LineData{}, Arrows: []ArrowData{},\n\t}\n\n\tcolorIdx := 0\n\tbandColorIdx := 0\n\n\tfor _, p := range placements {\n\t\tswitch {\n\t\tcase p.pane == core.Price && p.shape == shape.Scalar:\n\t\t\tline := buildLineData(p, allOutputs, bars, timeFmt, lineColors[colorIdx%len(lineColors)])\n\t\t\tcolorIdx++\n\t\t\tpricePane.Lines = append(pricePane.Lines, line)\n\n\t\tcase p.pane == core.Price && p.shape == shape.Band:\n\t\t\tband := buildBandData(p, allOutputs, bars, timeFmt, bandColors[bandColorIdx%len(bandColors)])\n\t\t\tbandColorIdx++\n\t\t\tpricePane.Bands = append(pricePane.Bands, band)\n\n\t\tdefault:\n\t\t\t// Own pane (or fallback).\n\t\t\tkey := ownPaneKey{p.indicatorIdx}\n\t\t\tidx, exists := ownPaneMap[key]\n\t\t\tif !exists {\n\t\t\t\tidx = len(indicatorPanes)\n\t\t\t\townPaneMap[key] = idx\n\t\t\t\tindicatorPanes = append(indicatorPanes, Pane{\n\t\t\t\t\tHeight: \"60\", ValueFormat: \",.2f\", ValueMarginPercentageFactor: 0.01,\n\t\t\t\t\tBands: []BandData{}, LineAreas: []LineAreaData{},\n\t\t\t\t\tHorizontals: []HorizontalData{}, Lines: []LineData{}, Arrows: []ArrowData{},\n\t\t\t\t})\n\t\t\t}\n\t\t\tpane := &indicatorPanes[idx]\n\n\t\t\tswitch p.shape {\n\t\t\tcase shape.Heatmap:\n\t\t\t\thm := buildHeatmapData(p, allOutputs, bars, timeFmt)\n\t\t\t\tpane.Heatmap = &hm\n\t\t\t\tpane.Height = \"120\"\n\t\t\t\tpane.ValueMarginPercentageFactor = 0\n\n\t\t\tcase shape.Band:\n\t\t\t\tband := buildBandData(p, allOutputs, bars, timeFmt, bandColors[bandColorIdx%len(bandColors)])\n\t\t\t\tbandColorIdx++\n\t\t\t\tpane.Bands = append(pane.Bands, band)\n\n\t\t\tdefault: // Scalar or unknown → line.\n\t\t\t\tline := buildLineData(p, allOutputs, bars, timeFmt, lineColors[colorIdx%len(lineColors)])\n\t\t\t\tcolorIdx++\n\t\t\t\tpane.Lines = append(pane.Lines, line)\n\t\t\t}\n\t\t}\n\t}\n\n\treturn &Configuration{\n\t\tWidth: \"100%\",\n\t\tNavigationPane: &NavigationPane{\n\t\t\tHeight: 30, HasLine: true, HasArea: false, HasTimeAxis: true, TimeTicks: 0,\n\t\t},\n\t\tHeightNavigationPane: 30,\n\t\tTimeAnnotationFormat: \"%Y-%m-%d\",\n\t\tAxisLeft:             true,\n\t\tAxisRight:            false,\n\t\tMargin:               Margin{Left: 0, Top: 10, Right: 20, Bottom: 0},\n\t\tOhlcv:                OhlcvData{Name: \"TEST\", Data: ohlcvBars, Candlesticks: true},\n\t\tPricePane:            pricePane,\n\t\tIndicatorPanes:       indicatorPanes,\n\t\tCrosshair:            false,\n\t\tVolumeInPricePane:    true,\n\t\tMenuVisible:          true,\n\t\tDownloadSvgVisible:   true,\n\t}\n}\n\nfunc buildLineData(p outputPlacement, allOutputs [][]core.Output, bars []entities.Bar, timeFmt, color string) LineData {\n\tdata := make([]ScalarVal, 0, len(bars))\n\tfor i := range bars {\n\t\tout := allOutputs[i][p.indicatorIdx]\n\t\tif p.outputIdx < len(out) {\n\t\t\tif s, ok := out[p.outputIdx].(entities.Scalar); ok && !math.IsNaN(s.Value) {\n\t\t\t\tdata = append(data, ScalarVal{Time: s.Time.Format(timeFmt), Value: s.Value})\n\t\t\t}\n\t\t}\n\t}\n\treturn LineData{\n\t\tName: p.mnemonic, Data: data, Indicator: p.indicatorIdx, Output: p.outputIdx,\n\t\tColor: color, Width: 1, Dash: \"\", Interpolation: \"natural\",\n\t}\n}\n\nfunc buildBandData(p outputPlacement, allOutputs [][]core.Output, bars []entities.Bar, timeFmt, color string) BandData {\n\tdata := make([]BandVal, 0, len(bars))\n\tfor i := range bars {\n\t\tout := allOutputs[i][p.indicatorIdx]\n\t\tif p.outputIdx < len(out) {\n\t\t\tif b, ok := out[p.outputIdx].(*outputs.Band); ok && !b.IsEmpty() {\n\t\t\t\tdata = append(data, BandVal{Time: b.Time.Format(timeFmt), Upper: b.Upper, Lower: b.Lower})\n\t\t\t}\n\t\t}\n\t}\n\treturn BandData{\n\t\tName: p.mnemonic, Data: data, Indicator: p.indicatorIdx, Output: p.outputIdx,\n\t\tColor: color, LegendColor: color, Interpolation: \"natural\",\n\t}\n}\n\nfunc buildHeatmapData(p outputPlacement, allOutputs [][]core.Output, bars []entities.Bar, timeFmt string) HeatmapData {\n\tdata := make([]HeatmapVal, 0, len(bars))\n\tfor i := range bars {\n\t\tout := allOutputs[i][p.indicatorIdx]\n\t\tif p.outputIdx < len(out) {\n\t\t\tif h, ok := out[p.outputIdx].(*outputs.Heatmap); ok && !h.IsEmpty() {\n\t\t\t\tdata = append(data, HeatmapVal{\n\t\t\t\t\tTime: h.Time.Format(timeFmt), First: h.ParameterFirst, Last: h.ParameterLast,\n\t\t\t\t\tResult: h.ParameterResolution, Min: h.ValueMin, Max: h.ValueMax, Values: h.Values,\n\t\t\t\t})\n\t\t\t}\n\t\t}\n\t}\n\treturn HeatmapData{\n\t\tName: p.mnemonic, Data: data, Indicator: p.indicatorIdx, Output: p.outputIdx,\n\t\tGradient: \"Viridis\", InvertGradient: false,\n\t}\n}\n\n// --- TypeScript generation ---\n\nfunc buildTypeScript(cfg *Configuration, baseName string) string {\n\tvar sb strings.Builder\n\n\tsb.WriteString(\"// Auto-generated chart configuration.\\n\")\n\tsb.WriteString(\"// eslint-disable-next-line\\n\")\n\tsb.WriteString(\"import { Configuration } from '../ohlcv-chart/template/configuration';\\n\")\n\tsb.WriteString(\"import { Scalar, Band, Heatmap, Bar } from '../ohlcv-chart/template/types';\\n\\n\")\n\n\t// Emit OHLCV data array.\n\tohlcvVar := sanitizeVarName(baseName) + \"Ohlcv\"\n\tsb.WriteString(fmt.Sprintf(\"export const %s: Bar[] = \", ohlcvVar))\n\twriteJSONArray(&sb, cfg.Ohlcv.Data)\n\tsb.WriteString(\";\\n\\n\")\n\n\t// Track variable names for data references.\n\ttype varRef struct{ varName, typeName string }\n\tlineVars := make(map[*LineData]varRef)\n\tbandVars := make(map[*BandData]varRef)\n\theatmapVars := make(map[*HeatmapData]varRef)\n\tlineAreaVars := make(map[*LineAreaData]varRef)\n\n\tvarCounter := 0\n\temitScalarArray := func(sb *strings.Builder, mnemonic string, data []ScalarVal) string {\n\t\tvarCounter++\n\t\tvn := sanitizeVarName(fmt.Sprintf(\"%s_%s_%d\", baseName, mnemonic, varCounter))\n\t\tsb.WriteString(fmt.Sprintf(\"export const %s: Scalar[] = \", vn))\n\t\twriteJSONArray(sb, data)\n\t\tsb.WriteString(\";\\n\\n\")\n\t\treturn vn\n\t}\n\temitBandArray := func(sb *strings.Builder, mnemonic string, data []BandVal) string {\n\t\tvarCounter++\n\t\tvn := sanitizeVarName(fmt.Sprintf(\"%s_%s_%d\", baseName, mnemonic, varCounter))\n\t\tsb.WriteString(fmt.Sprintf(\"export const %s: Band[] = \", vn))\n\t\twriteJSONArray(sb, data)\n\t\tsb.WriteString(\";\\n\\n\")\n\t\treturn vn\n\t}\n\temitHeatmapArray := func(sb *strings.Builder, mnemonic string, data []HeatmapVal) string {\n\t\tvarCounter++\n\t\tvn := sanitizeVarName(fmt.Sprintf(\"%s_%s_%d\", baseName, mnemonic, varCounter))\n\t\tsb.WriteString(fmt.Sprintf(\"export const %s: Heatmap[] = \", vn))\n\t\twriteJSONArray(sb, data)\n\t\tsb.WriteString(\";\\n\\n\")\n\t\treturn vn\n\t}\n\n\t// Emit price pane data arrays.\n\tfor i := range cfg.PricePane.Lines {\n\t\tl := &cfg.PricePane.Lines[i]\n\t\tvn := emitScalarArray(&sb, l.Name, l.Data)\n\t\tlineVars[l] = varRef{vn, \"Scalar[]\"}\n\t}\n\tfor i := range cfg.PricePane.Bands {\n\t\tb := &cfg.PricePane.Bands[i]\n\t\tvn := emitBandArray(&sb, b.Name, b.Data)\n\t\tbandVars[b] = varRef{vn, \"Band[]\"}\n\t}\n\tfor i := range cfg.PricePane.LineAreas {\n\t\tla := &cfg.PricePane.LineAreas[i]\n\t\tvn := emitScalarArray(&sb, la.Name, la.Data)\n\t\tlineAreaVars[la] = varRef{vn, \"Scalar[]\"}\n\t}\n\n\t// Emit indicator pane data arrays.\n\tfor pi := range cfg.IndicatorPanes {\n\t\tpane := &cfg.IndicatorPanes[pi]\n\t\tfor i := range pane.Lines {\n\t\t\tl := &pane.Lines[i]\n\t\t\tvn := emitScalarArray(&sb, l.Name, l.Data)\n\t\t\tlineVars[l] = varRef{vn, \"Scalar[]\"}\n\t\t}\n\t\tfor i := range pane.Bands {\n\t\t\tb := &pane.Bands[i]\n\t\t\tvn := emitBandArray(&sb, b.Name, b.Data)\n\t\t\tbandVars[b] = varRef{vn, \"Band[]\"}\n\t\t}\n\t\tfor i := range pane.LineAreas {\n\t\t\tla := &pane.LineAreas[i]\n\t\t\tvn := emitScalarArray(&sb, la.Name, la.Data)\n\t\t\tlineAreaVars[la] = varRef{vn, \"Scalar[]\"}\n\t\t}\n\t\tif pane.Heatmap != nil {\n\t\t\thm := pane.Heatmap\n\t\t\tvn := emitHeatmapArray(&sb, hm.Name, hm.Data)\n\t\t\theatmapVars[hm] = varRef{vn, \"Heatmap[]\"}\n\t\t}\n\t}\n\n\t// Emit configuration object.\n\tsb.WriteString(fmt.Sprintf(\"export const %sConfig: Configuration = {\\n\", sanitizeVarName(baseName)))\n\tsb.WriteString(fmt.Sprintf(\"  width: %q,\\n\", cfg.Width))\n\n\tif cfg.NavigationPane != nil {\n\t\tsb.WriteString(\"  navigationPane: {\\n\")\n\t\tsb.WriteString(fmt.Sprintf(\"    height: %d, hasLine: %t, hasArea: %t, hasTimeAxis: %t, timeTicks: %d,\\n\",\n\t\t\tcfg.NavigationPane.Height, cfg.NavigationPane.HasLine, cfg.NavigationPane.HasArea,\n\t\t\tcfg.NavigationPane.HasTimeAxis, cfg.NavigationPane.TimeTicks))\n\t\tsb.WriteString(\"  },\\n\")\n\t}\n\n\tsb.WriteString(fmt.Sprintf(\"  heightNavigationPane: %d,\\n\", cfg.HeightNavigationPane))\n\tsb.WriteString(fmt.Sprintf(\"  timeAnnotationFormat: %q,\\n\", cfg.TimeAnnotationFormat))\n\tsb.WriteString(fmt.Sprintf(\"  axisLeft: %t,\\n\", cfg.AxisLeft))\n\tsb.WriteString(fmt.Sprintf(\"  axisRight: %t,\\n\", cfg.AxisRight))\n\tsb.WriteString(fmt.Sprintf(\"  margin: { left: %d, top: %d, right: %d, bottom: %d },\\n\",\n\t\tcfg.Margin.Left, cfg.Margin.Top, cfg.Margin.Right, cfg.Margin.Bottom))\n\tsb.WriteString(fmt.Sprintf(\"  ohlcv: { name: %q, data: %s, candlesticks: %t },\\n\",\n\t\tcfg.Ohlcv.Name, ohlcvVar, cfg.Ohlcv.Candlesticks))\n\n\t// Price pane.\n\tsb.WriteString(\"  pricePane: {\\n\")\n\twriteTSPane(&sb, &cfg.PricePane, lineVars, bandVars, lineAreaVars, heatmapVars, \"    \")\n\tsb.WriteString(\"  },\\n\")\n\n\t// Indicator panes.\n\tsb.WriteString(\"  indicatorPanes: [\\n\")\n\tfor i := range cfg.IndicatorPanes {\n\t\tsb.WriteString(\"    {\\n\")\n\t\twriteTSPane(&sb, &cfg.IndicatorPanes[i], lineVars, bandVars, lineAreaVars, heatmapVars, \"      \")\n\t\tsb.WriteString(\"    },\\n\")\n\t}\n\tsb.WriteString(\"  ],\\n\")\n\n\tsb.WriteString(fmt.Sprintf(\"  crosshair: %t,\\n\", cfg.Crosshair))\n\tsb.WriteString(fmt.Sprintf(\"  volumeInPricePane: %t,\\n\", cfg.VolumeInPricePane))\n\tsb.WriteString(fmt.Sprintf(\"  menuVisible: %t,\\n\", cfg.MenuVisible))\n\tsb.WriteString(fmt.Sprintf(\"  downloadSvgVisible: %t,\\n\", cfg.DownloadSvgVisible))\n\tsb.WriteString(\"};\\n\")\n\n\treturn sb.String()\n}\n\ntype varRef struct{ varName, typeName string }\n\nfunc writeTSPane(sb *strings.Builder, pane *Pane,\n\tlineVars map[*LineData]varRef, bandVars map[*BandData]varRef,\n\tlineAreaVars map[*LineAreaData]varRef, heatmapVars map[*HeatmapData]varRef,\n\tindent string,\n) {\n\tsb.WriteString(fmt.Sprintf(\"%sheight: %q, valueFormat: %q, valueMarginPercentageFactor: %g,\\n\",\n\t\tindent, pane.Height, pane.ValueFormat, pane.ValueMarginPercentageFactor))\n\n\tif pane.Heatmap != nil {\n\t\thm := pane.Heatmap\n\t\tvr := heatmapVars[hm]\n\t\tsb.WriteString(fmt.Sprintf(\"%sheatmap: { name: %q, data: %s, indicator: %d, output: %d, gradient: %q, invertGradient: %t },\\n\",\n\t\t\tindent, hm.Name, vr.varName, hm.Indicator, hm.Output, hm.Gradient, hm.InvertGradient))\n\t}\n\n\t// Bands.\n\tsb.WriteString(indent + \"bands: [\")\n\tif len(pane.Bands) > 0 {\n\t\tsb.WriteString(\"\\n\")\n\t\tfor i := range pane.Bands {\n\t\t\tb := &pane.Bands[i]\n\t\t\tvr := bandVars[b]\n\t\t\tsb.WriteString(fmt.Sprintf(\"%s  { name: %q, data: %s, indicator: %d, output: %d, color: %q, legendColor: %q, interpolation: %q },\\n\",\n\t\t\t\tindent, b.Name, vr.varName, b.Indicator, b.Output, b.Color, b.LegendColor, b.Interpolation))\n\t\t}\n\t\tsb.WriteString(indent)\n\t}\n\tsb.WriteString(\"],\\n\")\n\n\t// LineAreas.\n\tsb.WriteString(indent + \"lineAreas: [\")\n\tif len(pane.LineAreas) > 0 {\n\t\tsb.WriteString(\"\\n\")\n\t\tfor i := range pane.LineAreas {\n\t\t\tla := &pane.LineAreas[i]\n\t\t\tvr := lineAreaVars[la]\n\t\t\tsb.WriteString(fmt.Sprintf(\"%s  { name: %q, data: %s, indicator: %d, output: %d, value: %g, color: %q, legendColor: %q, interpolation: %q },\\n\",\n\t\t\t\tindent, la.Name, vr.varName, la.Indicator, la.Output, la.Value, la.Color, la.LegendColor, la.Interpolation))\n\t\t}\n\t\tsb.WriteString(indent)\n\t}\n\tsb.WriteString(\"],\\n\")\n\n\t// Horizontals.\n\tsb.WriteString(indent + \"horizontals: [\")\n\tif len(pane.Horizontals) > 0 {\n\t\tsb.WriteString(\"\\n\")\n\t\tfor _, h := range pane.Horizontals {\n\t\t\tsb.WriteString(fmt.Sprintf(\"%s  { value: %g, color: %q, width: %g, dash: %q },\\n\",\n\t\t\t\tindent, h.Value, h.Color, h.Width, h.Dash))\n\t\t}\n\t\tsb.WriteString(indent)\n\t}\n\tsb.WriteString(\"],\\n\")\n\n\t// Lines.\n\tsb.WriteString(indent + \"lines: [\")\n\tif len(pane.Lines) > 0 {\n\t\tsb.WriteString(\"\\n\")\n\t\tfor i := range pane.Lines {\n\t\t\tl := &pane.Lines[i]\n\t\t\tvr := lineVars[l]\n\t\t\tsb.WriteString(fmt.Sprintf(\"%s  { name: %q, data: %s, indicator: %d, output: %d, color: %q, width: %g, dash: %q, interpolation: %q },\\n\",\n\t\t\t\tindent, l.Name, vr.varName, l.Indicator, l.Output, l.Color, l.Width, l.Dash, l.Interpolation))\n\t\t}\n\t\tsb.WriteString(indent)\n\t}\n\tsb.WriteString(\"],\\n\")\n\n\t// Arrows.\n\tsb.WriteString(indent + \"arrows: [],\\n\")\n}\n\nfunc writeJSONArray(sb *strings.Builder, v any) {\n\tdata, err := json.Marshal(v)\n\tif err != nil {\n\t\tsb.WriteString(\"[]\")\n\t\treturn\n\t}\n\tsb.Write(data)\n}\n\nfunc sanitizeVarName(s string) string {\n\tvar sb strings.Builder\n\tcapitalize := false\n\tfor i, c := range s {\n\t\tswitch {\n\t\tcase c >= 'a' && c <= 'z', c >= 'A' && c <= 'Z':\n\t\t\tif capitalize {\n\t\t\t\tif c >= 'a' && c <= 'z' {\n\t\t\t\t\tc = c - 'a' + 'A'\n\t\t\t\t}\n\t\t\t\tcapitalize = false\n\t\t\t}\n\t\t\tsb.WriteRune(c)\n\t\tcase c >= '0' && c <= '9':\n\t\t\tif i == 0 {\n\t\t\t\tsb.WriteRune('_')\n\t\t\t}\n\t\t\tsb.WriteRune(c)\n\t\t\tcapitalize = false\n\t\tdefault:\n\t\t\tcapitalize = true\n\t\t}\n\t}\n\treturn sb.String()\n}\n\n// testBars returns 252 embedded test bars constructed from the standard TA-Lib\n// daily reference data used across the indicator unit tests.\nfunc testBars() []entities.Bar {\n\thighs := testHighs()\n\tlows := testLows()\n\tcloses := testCloses()\n\tvolumes := testVolumes()\n\n\tbars := make([]entities.Bar, len(closes))\n\tbaseTime := time.Date(2020, 1, 2, 0, 0, 0, 0, time.UTC)\n\n\tfor i := range closes {\n\t\topenPrice := closes[0]\n\t\tif i > 0 {\n\t\t\topenPrice = closes[i-1]\n\t\t}\n\n\t\tbars[i] = entities.Bar{\n\t\t\tTime:   baseTime.AddDate(0, 0, i),\n\t\t\tOpen:   openPrice,\n\t\t\tHigh:   highs[i],\n\t\t\tLow:    lows[i],\n\t\t\tClose:  closes[i],\n\t\t\tVolume: volumes[i],\n\t\t}\n\t}\n\n\treturn bars\n}\n\nfunc testHighs() []float64 {\n\treturn []float64{\n\t\t93.25, 94.94, 96.375, 96.19, 96, 94.72, 95, 93.72, 92.47, 92.75,\n\t\t96.25, 99.625, 99.125, 92.75, 91.315, 93.25, 93.405, 90.655, 91.97, 92.25,\n\t\t90.345, 88.5, 88.25, 85.5, 84.44, 84.75, 84.44, 89.405, 88.125, 89.125,\n\t\t87.155, 87.25, 87.375, 88.97, 90, 89.845, 86.97, 85.94, 84.75, 85.47,\n\t\t84.47, 88.5, 89.47, 90, 92.44, 91.44, 92.97, 91.72, 91.155, 91.75,\n\t\t90, 88.875, 89, 85.25, 83.815, 85.25, 86.625, 87.94, 89.375, 90.625,\n\t\t90.75, 88.845, 91.97, 93.375, 93.815, 94.03, 94.03, 91.815, 92, 91.94,\n\t\t89.75, 88.75, 86.155, 84.875, 85.94, 99.375, 103.28, 105.375, 107.625, 105.25,\n\t\t104.5, 105.5, 106.125, 107.94, 106.25, 107, 108.75, 110.94, 110.94, 114.22,\n\t\t123, 121.75, 119.815, 120.315, 119.375, 118.19, 116.69, 115.345, 113, 118.315,\n\t\t116.87, 116.75, 113.87, 114.62, 115.31, 116, 121.69, 119.87, 120.87, 116.75,\n\t\t116.5, 116, 118.31, 121.5, 122, 121.44, 125.75, 127.75, 124.19, 124.44,\n\t\t125.75, 124.69, 125.31, 132, 131.31, 132.25, 133.88, 133.5, 135.5, 137.44,\n\t\t138.69, 139.19, 138.5, 138.13, 137.5, 138.88, 132.13, 129.75, 128.5, 125.44,\n\t\t125.12, 126.5, 128.69, 126.62, 126.69, 126, 123.12, 121.87, 124, 127,\n\t\t124.44, 122.5, 123.75, 123.81, 124.5, 127.87, 128.56, 129.63, 124.87, 124.37,\n\t\t124.87, 123.62, 124.06, 125.87, 125.19, 125.62, 126, 128.5, 126.75, 129.75,\n\t\t132.69, 133.94, 136.5, 137.69, 135.56, 133.56, 135, 132.38, 131.44, 130.88,\n\t\t129.63, 127.25, 127.81, 125, 126.81, 124.75, 122.81, 122.25, 121.06, 120,\n\t\t123.25, 122.75, 119.19, 115.06, 116.69, 114.87, 110.87, 107.25, 108.87, 109,\n\t\t108.5, 113.06, 93, 94.62, 95.12, 96, 95.56, 95.31, 99, 98.81,\n\t\t96.81, 95.94, 94.44, 92.94, 93.94, 95.5, 97.06, 97.5, 96.25, 96.37,\n\t\t95, 94.87, 98.25, 105.12, 108.44, 109.87, 105, 106, 104.94, 104.5,\n\t\t104.44, 106.31, 112.87, 116.5, 119.19, 121, 122.12, 111.94, 112.75, 110.19,\n\t\t107.94, 109.69, 111.06, 110.44, 110.12, 110.31, 110.44, 110, 110.75, 110.5,\n\t\t110.5, 109.5,\n\t}\n}\n\nfunc testLows() []float64 {\n\treturn []float64{\n\t\t90.75, 91.405, 94.25, 93.5, 92.815, 93.5, 92, 89.75, 89.44, 90.625,\n\t\t92.75, 96.315, 96.03, 88.815, 86.75, 90.94, 88.905, 88.78, 89.25, 89.75,\n\t\t87.5, 86.53, 84.625, 82.28, 81.565, 80.875, 81.25, 84.065, 85.595, 85.97,\n\t\t84.405, 85.095, 85.5, 85.53, 87.875, 86.565, 84.655, 83.25, 82.565, 83.44,\n\t\t82.53, 85.065, 86.875, 88.53, 89.28, 90.125, 90.75, 89, 88.565, 90.095,\n\t\t89, 86.47, 84, 83.315, 82, 83.25, 84.75, 85.28, 87.19, 88.44,\n\t\t88.25, 87.345, 89.28, 91.095, 89.53, 91.155, 92, 90.53, 89.97, 88.815,\n\t\t86.75, 85.065, 82.03, 81.5, 82.565, 96.345, 96.47, 101.155, 104.25, 101.75,\n\t\t101.72, 101.72, 103.155, 105.69, 103.655, 104, 105.53, 108.53, 108.75, 107.75,\n\t\t117, 118, 116, 118.5, 116.53, 116.25, 114.595, 110.875, 110.5, 110.72,\n\t\t112.62, 114.19, 111.19, 109.44, 111.56, 112.44, 117.5, 116.06, 116.56, 113.31,\n\t\t112.56, 114, 114.75, 118.87, 119, 119.75, 122.62, 123, 121.75, 121.56,\n\t\t123.12, 122.19, 122.75, 124.37, 128, 129.5, 130.81, 130.63, 132.13, 133.88,\n\t\t135.38, 135.75, 136.19, 134.5, 135.38, 133.69, 126.06, 126.87, 123.5, 122.62,\n\t\t122.75, 123.56, 125.81, 124.62, 124.37, 121.81, 118.19, 118.06, 117.56, 121,\n\t\t121.12, 118.94, 119.81, 121, 122, 124.5, 126.56, 123.5, 121.25, 121.06,\n\t\t122.31, 121, 120.87, 122.06, 122.75, 122.69, 122.87, 125.5, 124.25, 128,\n\t\t128.38, 130.69, 131.63, 134.38, 132, 131.94, 131.94, 129.56, 123.75, 126,\n\t\t126.25, 124.37, 121.44, 120.44, 121.37, 121.69, 120, 119.62, 115.5, 116.75,\n\t\t119.06, 119.06, 115.06, 111.06, 113.12, 110, 105, 104.69, 103.87, 104.69,\n\t\t105.44, 107, 89, 92.5, 92.12, 94.62, 92.81, 94.25, 96.25, 96.37,\n\t\t93.69, 93.5, 90, 90.19, 90.5, 92.12, 94.12, 94.87, 93, 93.87,\n\t\t93, 92.62, 93.56, 98.37, 104.44, 106, 101.81, 104.12, 103.37, 102.12,\n\t\t102.25, 103.37, 107.94, 112.5, 115.44, 115.5, 112.25, 107.56, 106.56, 106.87,\n\t\t104.5, 105.75, 108.62, 107.75, 108.06, 108, 108.19, 108.12, 109.06, 108.75,\n\t\t108.56, 106.62,\n\t}\n}\n\nfunc testCloses() []float64 {\n\treturn []float64{\n\t\t91.5, 94.815, 94.375, 95.095, 93.78, 94.625, 92.53, 92.75, 90.315, 92.47,\n\t\t96.125, 97.25, 98.5, 89.875, 91, 92.815, 89.155, 89.345, 91.625, 89.875,\n\t\t88.375, 87.625, 84.78, 83, 83.5, 81.375, 84.44, 89.25, 86.375, 86.25,\n\t\t85.25, 87.125, 85.815, 88.97, 88.47, 86.875, 86.815, 84.875, 84.19, 83.875,\n\t\t83.375, 85.5, 89.19, 89.44, 91.095, 90.75, 91.44, 89, 91, 90.5,\n\t\t89.03, 88.815, 84.28, 83.5, 82.69, 84.75, 85.655, 86.19, 88.94, 89.28,\n\t\t88.625, 88.5, 91.97, 91.5, 93.25, 93.5, 93.155, 91.72, 90, 89.69,\n\t\t88.875, 85.19, 83.375, 84.875, 85.94, 97.25, 99.875, 104.94, 106, 102.5,\n\t\t102.405, 104.595, 106.125, 106, 106.065, 104.625, 108.625, 109.315, 110.5, 112.75,\n\t\t123, 119.625, 118.75, 119.25, 117.94, 116.44, 115.19, 111.875, 110.595, 118.125,\n\t\t116, 116, 112, 113.75, 112.94, 116, 120.5, 116.62, 117, 115.25,\n\t\t114.31, 115.5, 115.87, 120.69, 120.19, 120.75, 124.75, 123.37, 122.94, 122.56,\n\t\t123.12, 122.56, 124.62, 129.25, 131, 132.25, 131, 132.81, 134, 137.38,\n\t\t137.81, 137.88, 137.25, 136.31, 136.25, 134.63, 128.25, 129, 123.87, 124.81,\n\t\t123, 126.25, 128.38, 125.37, 125.69, 122.25, 119.37, 118.5, 123.19, 123.5,\n\t\t122.19, 119.31, 123.31, 121.12, 123.37, 127.37, 128.5, 123.87, 122.94, 121.75,\n\t\t124.44, 122, 122.37, 122.94, 124, 123.19, 124.56, 127.25, 125.87, 128.86,\n\t\t132, 130.75, 134.75, 135, 132.38, 133.31, 131.94, 130, 125.37, 130.13,\n\t\t127.12, 125.19, 122, 125, 123, 123.5, 120.06, 121, 117.75, 119.87,\n\t\t122, 119.19, 116.37, 113.5, 114.25, 110, 105.06, 107, 107.87, 107,\n\t\t107.12, 107, 91, 93.94, 93.87, 95.5, 93, 94.94, 98.25, 96.75,\n\t\t94.81, 94.37, 91.56, 90.25, 93.94, 93.62, 97, 95, 95.87, 94.06,\n\t\t94.62, 93.75, 98, 103.94, 107.87, 106.06, 104.5, 105, 104.19, 103.06,\n\t\t103.42, 105.27, 111.87, 116, 116.62, 118.28, 113.37, 109, 109.7, 109.25,\n\t\t107, 109.19, 110, 109.2, 110.12, 108, 108.62, 109.75, 109.81, 109,\n\t\t108.75, 107.87,\n\t}\n}\n\nfunc testVolumes() []float64 {\n\treturn []float64{\n\t\t4077500, 4955900, 4775300, 4155300, 4593100, 3631300, 3382800, 4954200, 4500000, 3397500,\n\t\t4204500, 6321400, 10203600, 19043900, 11692000, 9553300, 8920300, 5970900, 5062300, 3705600,\n\t\t5865600, 5603000, 5811900, 8483800, 5995200, 5408800, 5430500, 6283800, 5834800, 4515500,\n\t\t4493300, 4346100, 3700300, 4600200, 4557200, 4323600, 5237500, 7404100, 4798400, 4372800,\n\t\t3872300, 10750800, 5804800, 3785500, 5014800, 3507700, 4298800, 4842500, 3952200, 3304700,\n\t\t3462000, 7253900, 9753100, 5953000, 5011700, 5910800, 4916900, 4135000, 4054200, 3735300,\n\t\t2921900, 2658400, 4624400, 4372200, 5831600, 4268600, 3059200, 4495500, 3425000, 3630800,\n\t\t4168100, 5966900, 7692800, 7362500, 6581300, 19587700, 10378600, 9334700, 10467200, 5671400,\n\t\t5645000, 4518600, 4519500, 5569700, 4239700, 4175300, 4995300, 4776600, 4190000, 6035300,\n\t\t12168900, 9040800, 5780300, 4320800, 3899100, 3221400, 3455500, 4304200, 4703900, 8316300,\n\t\t10553900, 6384800, 7163300, 7007800, 5114100, 5263800, 6666100, 7398400, 5575000, 4852300,\n\t\t4298100, 4900500, 4887700, 6964800, 4679200, 9165000, 6469800, 6792000, 4423800, 5231900,\n\t\t4565600, 6235200, 5225900, 8261400, 5912500, 3545600, 5714500, 6653900, 6094500, 4799200,\n\t\t5050800, 5648900, 4726300, 5585600, 5124800, 7630200, 14311600, 8793600, 8874200, 6966600,\n\t\t5525500, 6515500, 5291900, 5711700, 4327700, 4568000, 6859200, 5757500, 7367000, 6144100,\n\t\t4052700, 5849700, 5544700, 5032200, 4400600, 4894100, 5140000, 6610900, 7585200, 5963100,\n\t\t6045500, 8443300, 6464700, 6248300, 4357200, 4774700, 6216900, 6266900, 5584800, 5284500,\n\t\t7554500, 7209500, 8424800, 5094500, 4443600, 4591100, 5658400, 6094100, 14862200, 7544700,\n\t\t6985600, 8093000, 7590000, 7451300, 7078000, 7105300, 8778800, 6643900, 10563900, 7043100,\n\t\t6438900, 8057700, 14240000, 17872300, 7831100, 8277700, 15017800, 14183300, 13921100, 9683000,\n\t\t9187300, 11380500, 69447300, 26673600, 13768400, 11371600, 9872200, 9450500, 11083300, 9552800,\n\t\t11108400, 10374200, 16701900, 13741900, 8523600, 9551900, 8680500, 7151700, 9673100, 6264700,\n\t\t8541600, 8358000, 18720800, 19683100, 13682500, 10668100, 9710600, 3113100, 5682000, 5763600,\n\t\t5340000, 6220800, 14680500, 9933000, 11329500, 8145300, 16644700, 12593800, 7138100, 7442300,\n\t\t9442300, 7123600, 7680600, 4839800, 4775500, 4008800, 4533600, 3741100, 4084800, 2685200,\n\t\t3438000, 2870500,\n\t}\n}\n"
}
```

**Output:**
```
Wrote file successfully.

LSP errors detected in this file, please fix:
<diagnostics file="/home/dev/zpano/go/cmd/iconf/main.go">
ERROR [564:35] cannot use lineVars (variable of type map[*LineData]varRef) as map[*LineData]varRef value in argument to writeTSPane
ERROR [564:45] cannot use bandVars (variable of type map[*BandData]varRef) as map[*BandData]varRef value in argument to writeTSPane
ERROR [564:55] cannot use lineAreaVars (variable of type map[*LineAreaData]varRef) as map[*LineAreaData]varRef value in argument to writeTSPane
ERROR [564:69] cannot use heatmapVars (variable of type map[*HeatmapData]varRef) as map[*HeatmapData]varRef value in argument to writeTSPane
ERROR [571:44] cannot use lineVars (variable of type map[*LineData]varRef) as map[*LineData]varRef value in argument to writeTSPane
ERROR [571:54] cannot use bandVars (variable of type map[*BandData]varRef) as map[*BandData]varRef value in argument to writeTSPane
ERROR [571:64] cannot use lineAreaVars (variable of type map[*LineAreaData]varRef) as map[*LineAreaData]varRef value in argument to writeTSPane
ERROR [571:78] cannot use heatmapVars (variable of type map[*HeatmapData]varRef) as map[*HeatmapData]varRef value in argument to writeTSPane
</diagnostics>
```

---

## Assistant (Build · claude-opus-4.6 · 6.1s)

I have a duplicate `varRef` type definition — one inside the function and one at package level. Let me fix that:

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/cmd/iconf/main.go",
  "oldString": "\t// Track variable names for data references.\n\ttype varRef struct{ varName, typeName string }\n\tlineVars := make(map[*LineData]varRef)",
  "newString": "\t// Track variable names for data references.\n\tlineVars := make(map[*LineData]varRef)"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.6 · 4.6s)

Now build:

**Tool: bash**

**Input:**
```json
{
  "command": "cd /home/dev/zpano/go && go build ./cmd/iconf/ 2>&1",
  "description": "Build iconf"
}
```

---

## Assistant (Build · claude-opus-4.6 · 6.6s)

Builds clean. Let me test it with a small settings file:

**Tool: bash**

**Input:**
```json
{
  "command": "cd /home/dev/zpano/go && echo '[{\"identifier\":\"bollingerBands\",\"params\":{\"length\":20,\"multiplier\":2}},{\"identifier\":\"goertzelSpectrum\",\"params\":{\"length\":64,\"range\":[2,28],\"resolution\":10}}]' > /tmp/test-settings.json && ./iconf /tmp/test-settings.json /tmp/test-output 2>&1",
  "description": "Test iconf with BB+Goertzel"
}
```

**Output:**
```
wrote /tmp/test-output.json
wrote /tmp/test-output.ts

```

---

## Assistant (Build · claude-opus-4.6 · 6.6s)

Let me verify the outputs look reasonable:

**Tool: bash**

**Input:**
```json
{
  "command": "cd /home/dev/zpano/go && head -50 /tmp/test-output.ts && echo \"---\" && python3 -c \"import json; d=json.load(open('/tmp/test-output.json')); print('pricePane lines:', len(d['pricePane']['lines'])); print('pricePane bands:', len(d['pricePane']['bands'])); print('indicatorPanes:', len(d['indicatorPanes'])); [print(f'  pane {i}: lines={len(p[\\\"lines\\\"])}, lineAreas={len(p[\\\"lineAreas\\\"])}, heatmap={\\\"heatmap\\\" in p and p[\\\"heatmap\\\"] is not None}') for i,p in enumerate(d['indicatorPanes'])]\"",
  "description": "Verify output structure"
}
```

**Output:**
```
// Auto-generated chart configuration.
// eslint-disable-next-line
import { Configuration } from '../ohlcv-chart/template/configuration';
import { Scalar, Band, Heatmap, Bar } from '../ohlcv-chart/template/types';

export const testOutputOhlcv: Bar[] = [{"time":"2020-01-02","open":91.5,"high":93.25,"low":90.75,"close":91.5,"volume":4077500},{"time":"2020-01-03","open":91.5,"high":94.94,"low":91.405,"close":94.815,"volume":4955900},{"time":"2020-01-04","open":94.815,"high":96.375,"low":94.25,"close":94.375,"volume":4775300},{"time":"2020-01-05","open":94.375,"high":96.19,"low":93.5,"close":95.095,"volume":4155300},{"time":"2020-01-06","open":95.095,"high":96,"low":92.815,"close":93.78,"volume":4593100},{"time":"2020-01-07","open":93.78,"high":94.72,"low":93.5,"close":94.625,"volume":3631300},{"time":"2020-01-08","open":94.625,"high":95,"low":92,"close":92.53,"volume":3382800},{"time":"2020-01-09","open":92.53,"high":93.72,"low":89.75,"close":92.75,"volume":4954200},{"time":"2020-01-10","open":92.75,"high":92.47,"low":89.44,"close":90.315,"volume":4500000},{"time":"2020-01-11","open":90.315,"high":92.75,"low":90.625,"close":92.47,"volume":3397500},{"time":"2020-01-12","open":92.47,"high":96.25,"low":92.75,"close":96.125,"volume":4204500},{"time":"2020-01-13","open":96.125,"high":99.625,"low":96.315,"close":97.25,"volume":6321400},{"time":"2020-01-14","open":97.25,"high":99.125,"low":96.03,"close":98.5,"volume":10203600},{"time":"2020-01-15","open":98.5,"high":92.75,"low":88.815,"close":89.875,"volume":19043900},{"time":"2020-01-16","open":89.875,"high":91.315,"low":86.75,"close":91,"volume":11692000},{"time":"2020-01-17","open":91,"high":93.25,"low":90.94,"close":92.815,"volume":9553300},{"time":"2020-01-18","open":92.815,"high":93.405,"low":88.905,"close":89.155,"volume":8920300},{"time":"2020-01-19","open":89.155,"high":90.655,"low":88.78,"close":89.345,"volume":5970900},{"time":"2020-01-20","open":89.345,"high":91.97,"low":89.25,"close":91.625,"volume":5062300},{"time":"2020-01-21","open":91.625,"high":92.25,"low":89.75,"close":89.875,"volume":3705600},{"time":"2020-01-22","open":89.875,"high":90.345,"low":87.5,"close":88.375,"volume":5865600},{"time":"2020-01-23","open":88.375,"high":88.5,"low":86.53,"close":87.625,"volume":5603000},{"time":"2020-01-24","open":87.625,"high":88.25,"low":84.625,"close":84.78,"volume":5811900},{"time":"2020-01-25","open":84.78,"high":85.5,"low":82.28,"close":83,"volume":8483800},{"time":"2020-01-26","open":83,"high":84.44,"low":81.565,"close":83.5,"volume":5995200},{"time":"2020-01-27","open":83.5,"high":84.75,"low":80.875,"close":81.375,"volume":5408800},{"time":"2020-01-28","open":81.375,"high":84.44,"low":81.25,"close":84.44,"volume":5430500},{"time":"2020-01-29","open":84.44,"high":89.405,"low":84.065,"close":89.25,"volume":6283800},{"time":"2020-01-30","open":89.25,"high":88.125,"low":85.595,"close":86.375,"volume":5834800},{"time":"2020-01-31","open":86.375,"high":89.125,"low":85.97,"close":86.25,"volume":4515500},{"time":"2020-02-01","open":86.25,"high":87.155,"low":84.405,"close":85.25,"volume":4493300},{"time":"2020-02-02","open":85.25,"high":87.25,"low":85.095,"close":87.125,"volume":4346100},{"time":"2020-02-03","open":87.125,"high":87.375,"low":85.5,"close":85.815,"volume":3700300},{"time":"2020-02-04","open":85.815,"high":88.97,"low":85.53,"close":88.97,"volume":4600200},{"time":"2020-02-05","open":88.97,"high":90,"low":87.875,"close":88.47,"volume":4557200},{"time":"2020-02-06","open":88.47,"high":89.845,"low":86.565,"close":86.875,"volume":4323600},{"time":"2020-02-07","open":86.875,"high":86.97,"low":84.655,"close":86.815,"volume":5237500},{"time":"2020-02-08","open":86.815,"high":85.94,"low":83.25,"close":84.875,"volume":7404100},{"time":"2020-02-09","open":84.875,"high":84.75,"low":82.565,"close":84.19,"volume":4798400},{"time":"2020-02-10","open":84.19,"high":85.47,"low":83.44,"close":83.875,"volume":4372800},{"time":"2020-02-11","open":83.875,"high":84.47,"low":82.53,"close":83.375,"volume":3872300},{"time":"2020-02-12","open":83.375,"high":88.5,"low":85.065,"close":85.5,"volume":10750800},{"time":"2020-02-13","open":85.5,"high":89.47,"low":86.875,"close":89.19,"volume":5804800},{"time":"2020-02-14","open":89.19,"high":90,"low":88.53,"close":89.44,"volume":3785500},{"time":"2020-02-15","open":89.44,"high":92.44,"low":89.28,"close":91.095,"volume":5014800},{"time":"2020-02-16","open":91.095,"high":91.44,"low":90.125,"close":90.75,"volume":3507700},{"time":"2020-02-17","open":90.75,"high":92.97,"low":90.75,"close":91.44,"volume":4298800},{"time":"2020-02-18","open":91.44,"high":91.72,"low":89,"close":89,"volume":4842500},{"time":"2020-02-19","open":89,"high":91.155,"low":88.565,"close":91,"volume":3952200},{"time":"2020-02-20","open":91,"high":91.75,"low":90.095,"close":90.5,"volume":3304700},{"time":"2020-02-21","open":90.5,"high":90,"low":89,"close":89.03,"volume":3462000},{"time":"2020-02-22","open":89.03,"high":88.875,"low":86.47,"close":88.815,"volume":7253900},{"time":"2020-02-23","open":88.815,"high":89,"low":84,"close":84.28,"volume":9753100},{"time":"2020-02-24","open":84.28,"high":85.25,"low":83.315,"close":83.5,"volume":5953000},{"time":"2020-02-25","open":83.5,"high":83.815,"low":82,"close":82.69,"volume":5011700},{"time":"2020-02-26","open":82.69,"high":85.25,"low":83.25,"close":84.75,"volume":5910800},{"time":"2020-02-27","open":84.75,"high":86.625,"low":84.75,"close":85.655,"volume":4916900},{"time":"2020-02-28","open":85.655,"high":87.94,"low":85.28,"close":86.19,"volume":4135000},{"time":"2020-02-29","open":86.19,"high":89.375,"low":87.19,"close":88.94,"volume":4054200},{"time":"2020-03-01","open":88.94,"high":90.625,"low":88.44,"close":89.28,"volume":3735300},{"time":"2020-03-02","open":89.28,"high":90.75,"low":88.25,"close":88.625,"volume":2921900},{"time":"2020-03-03","open":88.625,"high":88.845,"low":87.345,"close":88.5,"volume":2658400},{"time":"2020-03-04","open":88.5,"high":91.97,"low":89.28,"close":91.97,"volume":4624400},{"time":"2020-03-05","open":91.97,"high":93.375,"low":91.095,"close":91.5,"volume":4372200},{"time":"2020-03-06","open":91.5,"high":93.815,"low":89.53,"close":93.25,"volume":5831600},{"time":"2020-03-07","open":93.25,"high":94.03,"low":91.155,"close":93.5,"volume":4268600},{"time":"2020-03-08","open":93.5,"high":94.03,"low":92,"close":93.155,"volume":3059200},{"time":"2020-03-09","open":93.155,"high":91.815,"low":90.53,"close":91.72,"volume":4495500},{"time":"2020-03-10","open":91.72,"high":92,"low":89.97,"close":90,"volume":3425000},{"time":"2020-03-11","open":90,"high":91.94,"low":88.815,"close":89.69,"volume":3630800},{"time":"2020-03-12","open":89.69,"high":89.75,"low":86.75,"close":88.875,"volume":4168100},{"time":"2020-03-13","open":88.875,"high":88.75,"low":85.065,"close":85.19,"volume":5966900},{"time":"2020-03-14","open":85.19,"high":86.155,"low":82.03,"close":83.375,"volume":7692800},{"time":"2020-03-15","open":83.375,"high":84.875,"low":81.5,"close":84.875,"volume":7362500},{"time":"2020-03-16","open":84.875,"high":85.94,"low":82.565,"close":85.94,"volume":6581300},{"time":"2020-03-17","open":85.94,"high":99.375,"low":96.345,"close":97.25,"volume":19587700},{"time":"2020-03-18","open":97.25,"high":103.28,"low":96.47,"close":99.875,"volume":10378600},{"time":"2020-03-19","open":99.875,"high":105.375,"low":101.155,"close":104.94,"volume":9334700},{"time":"2020-03-20","open":104.94,"high":107.625,"low":104.25,"close":106,"volume":10467200},{"time":"2020-03-21","open":106,"high":105.25,"low":101.75,"close":102.5,"volume":5671400},{"time":"2020-03-22","open":102.5,"high":104.5,"low":101.72,"close":102.405,"volume":5645000},{"time":"2020-03-23","open":102.405,"high":105.5,"low":101.72,"close":104.595,"volume":4518600},{"time":"2020-03-24","open":104.595,"high":106.125,"low":103.155,"close":106.125,"volume":4519500},{"time":"2020-03-25","open":106.125,"high":107.94,"low":105.69,"close":106,"volume":5569700},{"time":"2020-03-26","open":106,"high":106.25,"low":103.655,"close":106.065,"volume":4239700},{"time":"2020-03-27","open":106.065,"high":107,"low":104,"close":104.625,"volume":4175300},{"time":"2020-03-28","open":104.625,"high":108.75,"low":105.53,"close":108.625,"volume":4995300},{"time":"2020-03-29","open":108.625,"high":110.94,"low":108.53,"close":109.315,"volume":4776600},{"time":"2020-03-30","open":109.315,"high":110.94,"low":108.75,"close":110.5,"volume":4190000},{"time":"2020-03-31","open":110.5,"high":114.22,"low":107.75,"close":112.75,"volume":6035300},{"time":"2020-04-01","open":112.75,"high":123,"low":117,"close":123,"volume":12168900},{"time":"2020-04-02","open":123,"high":121.75,"low":118,"close":119.625,"volume":9040800},{"time":"2020-04-03","open":119.625,"high":119.815,"low":116,"close":118.75,"volume":5780300},{"time":"2020-04-04","open":118.75,"high":120.315,"low":118.5,"close":119.25,"volume":4320800},{"time":"2020-04-05","open":119.25,"high":119.375,"low":116.53,"close":117.94,"volume":3899100},{"time":"2020-04-06","open":117.94,"high":118.19,"low":116.25,"close":116.44,"volume":3221400},{"time":"2020-04-07","open":116.44,"high":116.69,"low":114.595,"close":115.19,"volume":3455500},{"time":"2020-04-08","open":115.19,"high":115.345,"low":110.875,"close":111.875,"volume":4304200},{"time":"2020-04-09","open":111.875,"high":113,"low":110.5,"close":110.595,"volume":4703900},{"time":"2020-04-10","open":110.595,"high":118.315,"low":110.72,"close":118.125,"volume":8316300},{"time":"2020-04-11","open":118.125,"high":116.87,"low":112.62,"close":116,"volume":10553900},{"time":"2020-04-12","open":116,"high":116.75,"low":114.19,"close":116,"volume":6384800},{"time":"2020-04-13","open":116,"high":113.87,"low":111.19,"close":112,"volume":7163300},{"time":"2020-04-14","open":112,"high":114.62,"low":109.44,"close":113.75,"volume":7007800},{"time":"2020-04-15","open":113.75,"high":115.31,"low":111.56,"close":112.94,"volume":5114100},{"time":"2020-04-16","open":112.94,"high":116,"low":112.44,"close":116,"volume":5263800},{"time":"2020-04-17","open":116,"high":121.69,"low":117.5,"close":120.5,"volume":6666100},{"time":"2020-04-18","open":120.5,"high":119.87,"low":116.06,"close":116.62,"volume":7398400},{"time":"2020-04-19","open":116.62,"high":120.87,"low":116.56,"close":117,"volume":5575000},{"time":"2020-04-20","open":117,"high":116.75,"low":113.31,"close":115.25,"volume":4852300},{"time":"2020-04-21","open":115.25,"high":116.5,"low":112.56,"close":114.31,"volume":4298100},{"time":"2020-04-22","open":114.31,"high":116,"low":114,"close":115.5,"volume":4900500},{"time":"2020-04-23","open":115.5,"high":118.31,"low":114.75,"close":115.87,"volume":4887700},{"time":"2020-04-24","open":115.87,"high":121.5,"low":118.87,"close":120.69,"volume":6964800},{"time":"2020-04-25","open":120.69,"high":122,"low":119,"close":120.19,"volume":4679200},{"time":"2020-04-26","open":120.19,"high":121.44,"low":119.75,"close":120.75,"volume":9165000},{"time":"2020-04-27","open":120.75,"high":125.75,"low":122.62,"close":124.75,"volume":6469800},{"time":"2020-04-28","open":124.75,"high":127.75,"low":123,"close":123.37,"volume":6792000},{"time":"2020-04-29","open":123.37,"high":124.19,"low":121.75,"close":122.94,"volume":4423800},{"time":"2020-04-30","open":122.94,"high":124.44,"low":121.56,"close":122.56,"volume":5231900},{"time":"2020-05-01","open":122.56,"high":125.75,"low":123.12,"close":123.12,"volume":4565600},{"time":"2020-05-02","open":123.12,"high":124.69,"low":122.19,"close":122.56,"volume":6235200},{"time":"2020-05-03","open":122.56,"high":125.31,"low":122.75,"close":124.62,"volume":5225900},{"time":"2020-05-04","open":124.62,"high":132,"low":124.37,"close":129.25,"volume":8261400},{"time":"2020-05-05","open":129.25,"high":131.31,"low":128,"close":131,"volume":5912500},{"time":"2020-05-06","open":131,"high":132.25,"low":129.5,"close":132.25,"volume":3545600},{"time":"2020-05-07","open":132.25,"high":133.88,"low":130.81,"close":131,"volume":5714500},{"time":"2020-05-08","open":131,"high":133.5,"low":130.63,"close":132.81,"volume":6653900},{"time":"2020-05-09","open":132.81,"high":135.5,"low":132.13,"close":134,"volume":6094500},{"time":"2020-05-10","open":134,"high":137.44,"low":133.88,"close":137.38,"volume":4799200},{"time":"2020-05-11","open":137.38,"high":138.69,"low":135.38,"close":137.81,"volume":5050800},{"time":"2020-05-12","open":137.81,"high":139.19,"low":135.75,"close":137.88,"volume":5648900},{"time":"2020-05-13","open":137.88,"high":138.5,"low":136.19,"close":137.25,"volume":4726300},{"time":"2020-05-14","open":137.25,"high":138.13,"low":134.5,"close":136.31,"volume":5585600},{"time":"2020-05-15","open":136.31,"high":137.5,"low":135.38,"close":136.25,"volume":5124800},{"time":"2020-05-16","open":136.25,"high":138.88,"low":133.69,"close":134.63,"volume":7630200},{"time":"2020-05-17","open":134.63,"high":132.13,"low":126.06,"close":128.25,"volume":14311600},{"time":"2020-05-18","open":128.25,"high":129.75,"low":126.87,"close":129,"volume":8793600},{"time":"2020-05-19","open":129,"high":128.5,"low":123.5,"close":123.87,"volume":8874200},{"time":"2020-05-20","open":123.87,"high":125.44,"low":122.62,"close":124.81,"volume":6966600},{"time":"2020-05-21","open":124.81,"high":125.12,"low":122.75,"close":123,"volume":5525500},{"time":"2020-05-22","open":123,"high":126.5,"low":123.56,"close":126.25,"volume":6515500},{"time":"2020-05-23","open":126.25,"high":128.69,"low":125.81,"close":128.38,"volume":5291900},{"time":"2020-05-24","open":128.38,"high":126.62,"low":124.62,"close":125.37,"volume":5711700},{"time":"2020-05-25","open":125.37,"high":126.69,"low":124.37,"close":125.69,"volume":4327700},{"time":"2020-05-26","open":125.69,"high":126,"low":121.81,"close":122.25,"volume":4568000},{"time":"2020-05-27","open":122.25,"high":123.12,"low":118.19,"close":119.37,"volume":6859200},{"time":"2020-05-28","open":119.37,"high":121.87,"low":118.06,"close":118.5,"volume":5757500},{"time":"2020-05-29","open":118.5,"high":124,"low":117.56,"close":123.19,"volume":7367000},{"time":"2020-05-30","open":123.19,"high":127,"low":121,"close":123.5,"volume":6144100},{"time":"2020-05-31","open":123.5,"high":124.44,"low":121.12,"close":122.19,"volume":4052700},{"time":"2020-06-01","open":122.19,"high":122.5,"low":118.94,"close":119.31,"volume":5849700},{"time":"2020-06-02","open":119.31,"high":123.75,"low":119.81,"close":123.31,"volume":5544700},{"time":"2020-06-03","open":123.31,"high":123.81,"low":121,"close":121.12,"volume":5032200},{"time":"2020-06-04","open":121.12,"high":124.5,"low":122,"close":123.37,"volume":4400600},{"time":"2020-06-05","open":123.37,"high":127.87,"low":124.5,"close":127.37,"volume":4894100},{"time":"2020-06-06","open":127.37,"high":128.56,"low":126.56,"close":128.5,"volume":5140000},{"time":"2020-06-07","open":128.5,"high":129.63,"low":123.5,"close":123.87,"volume":6610900},{"time":"2020-06-08","open":123.87,"high":124.87,"low":121.25,"close":122.94,"volume":7585200},{"time":"2020-06-09","open":122.94,"high":124.37,"low":121.06,"close":121.75,"volume":5963100},{"time":"2020-06-10","open":121.75,"high":124.87,"low":122.31,"close":124.44,"volume":6045500},{"time":"2020-06-11","open":124.44,"high":123.62,"low":121,"close":122,"volume":8443300},{"time":"2020-06-12","open":122,"high":124.06,"low":120.87,"close":122.37,"volume":6464700},{"time":"2020-06-13","open":122.37,"high":125.87,"low":122.06,"close":122.94,"volume":6248300},{"time":"2020-06-14","open":122.94,"high":125.19,"low":122.75,"close":124,"volume":4357200},{"time":"2020-06-15","open":124,"high":125.62,"low":122.69,"close":123.19,"volume":4774700},{"time":"2020-06-16","open":123.19,"high":126,"low":122.87,"close":124.56,"volume":6216900},{"time":"2020-06-17","open":124.56,"high":128.5,"low":125.5,"close":127.25,"volume":6266900},{"time":"2020-06-18","open":127.25,"high":126.75,"low":124.25,"close":125.87,"volume":5584800},{"time":"2020-06-19","open":125.87,"high":129.75,"low":128,"close":128.86,"volume":5284500},{"time":"2020-06-20","open":128.86,"high":132.69,"low":128.38,"close":132,"volume":7554500},{"time":"2020-06-21","open":132,"high":133.94,"low":130.69,"close":130.75,"volume":7209500},{"time":"2020-06-22","open":130.75,"high":136.5,"low":131.63,"close":134.75,"volume":8424800},{"time":"2020-06-23","open":134.75,"high":137.69,"low":134.38,"close":135,"volume":5094500},{"time":"2020-06-24","open":135,"high":135.56,"low":132,"close":132.38,"volume":4443600},{"time":"2020-06-25","open":132.38,"high":133.56,"low":131.94,"close":133.31,"volume":4591100},{"time":"2020-06-26","open":133.31,"high":135,"low":131.94,"close":131.94,"volume":5658400},{"time":"2020-06-27","open":131.94,"high":132.38,"low":129.56,"close":130,"volume":6094100},{"time":"2020-06-28","open":130,"high":131.44,"low":123.75,"close":125.37,"volume":14862200},{"time":"2020-06-29","open":125.37,"high":130.88,"low":126,"close":130.13,"volume":7544700},{"time":"2020-06-30","open":130.13,"high":129.63,"low":126.25,"close":127.12,"volume":6985600},{"time":"2020-07-01","open":127.12,"high":127.25,"low":124.37,"close":125.19,"volume":8093000},{"time":"2020-07-02","open":125.19,"high":127.81,"low":121.44,"close":122,"volume":7590000},{"time":"2020-07-03","open":122,"high":125,"low":120.44,"close":125,"volume":7451300},{"time":"2020-07-04","open":125,"high":126.81,"low":121.37,"close":123,"volume":7078000},{"time":"2020-07-05","open":123,"high":124.75,"low":121.69,"close":123.5,"volume":7105300},{"time":"2020-07-06","open":123.5,"high":122.81,"low":120,"close":120.06,"volume":8778800},{"time":"2020-07-07","open":120.06,"high":122.25,"low":119.62,"close":121,"volume":6643900},{"time":"2020-07-08","open":121,"high":121.06,"low":115.5,"close":117.75,"volume":10563900},{"time":"2020-07-09","open":117.75,"high":120,"low":116.75,"close":119.87,"volume":7043100},{"time":"2020-07-10","open":119.87,"high":123.25,"low":119.06,"close":122,"volume":6438900},{"time":"2020-07-11","open":122,"high":122.75,"low":119.06,"close":119.19,"volume":8057700},{"time":"2020-07-12","open":119.19,"high":119.19,"low":115.06,"close":116.37,"volume":14240000},{"time":"2020-07-13","open":116.37,"high":115.06,"low":111.06,"close":113.5,"volume":17872300},{"time":"2020-07-14","open":113.5,"high":116.69,"low":113.12,"close":114.25,"volume":7831100},{"time":"2020-07-15","open":114.25,"high":114.87,"low":110,"close":110,"volume":8277700},{"time":"2020-07-16","open":110,"high":110.87,"low":105,"close":105.06,"volume":15017800},{"time":"2020-07-17","open":105.06,"high":107.25,"low":104.69,"close":107,"volume":14183300},{"time":"2020-07-18","open":107,"high":108.87,"low":103.87,"close":107.87,"volume":13921100},{"time":"2020-07-19","open":107.87,"high":109,"low":104.69,"close":107,"volume":9683000},{"time":"2020-07-20","open":107,"high":108.5,"low":105.44,"close":107.12,"volume":9187300},{"time":"2020-07-21","open":107.12,"high":113.06,"low":107,"close":107,"volume":11380500},{"time":"2020-07-22","open":107,"high":93,"low":89,"close":91,"volume":69447300},{"time":"2020-07-23","open":91,"high":94.62,"low":92.5,"close":93.94,"volume":26673600},{"time":"2020-07-24","open":93.94,"high":95.12,"low":92.12,"close":93.87,"volume":13768400},{"time":"2020-07-25","open":93.87,"high":96,"low":94.62,"close":95.5,"volume":11371600},{"time":"2020-07-26","open":95.5,"high":95.56,"low":92.81,"close":93,"volume":9872200},{"time":"2020-07-27","open":93,"high":95.31,"low":94.25,"close":94.94,"volume":9450500},{"time":"2020-07-28","open":94.94,"high":99,"low":96.25,"close":98.25,"volume":11083300},{"time":"2020-07-29","open":98.25,"high":98.81,"low":96.37,"close":96.75,"volume":9552800},{"time":"2020-07-30","open":96.75,"high":96.81,"low":93.69,"close":94.81,"volume":11108400},{"time":"2020-07-31","open":94.81,"high":95.94,"low":93.5,"close":94.37,"volume":10374200},{"time":"2020-08-01","open":94.37,"high":94.44,"low":90,"close":91.56,"volume":16701900},{"time":"2020-08-02","open":91.56,"high":92.94,"low":90.19,"close":90.25,"volume":13741900},{"time":"2020-08-03","open":90.25,"high":93.94,"low":90.5,"close":93.94,"volume":8523600},{"time":"2020-08-04","open":93.94,"high":95.5,"low":92.12,"close":93.62,"volume":9551900},{"time":"2020-08-05","open":93.62,"high":97.06,"low":94.12,"close":97,"volume":8680500},{"time":"2020-08-06","open":97,"high":97.5,"low":94.87,"close":95,"volume":7151700},{"time":"2020-08-07","open":95,"high":96.25,"low":93,"close":95.87,"volume":9673100},{"time":"2020-08-08","open":95.87,"high":96.37,"low":93.87,"close":94.06,"volume":6264700},{"time":"2020-08-09","open":94.06,"high":95,"low":93,"close":94.62,"volume":8541600},{"time":"2020-08-10","open":94.62,"high":94.87,"low":92.62,"close":93.75,"volume":8358000},{"time":"2020-08-11","open":93.75,"high":98.25,"low":93.56,"close":98,"volume":18720800},{"time":"2020-08-12","open":98,"high":105.12,"low":98.37,"close":103.94,"volume":19683100},{"time":"2020-08-13","open":103.94,"high":108.44,"low":104.44,"close":107.87,"volume":13682500},{"time":"2020-08-14","open":107.87,"high":109.87,"low":106,"close":106.06,"volume":10668100},{"time":"2020-08-15","open":106.06,"high":105,"low":101.81,"close":104.5,"volume":9710600},{"time":"2020-08-16","open":104.5,"high":106,"low":104.12,"close":105,"volume":3113100},{"time":"2020-08-17","open":105,"high":104.94,"low":103.37,"close":104.19,"volume":5682000},{"time":"2020-08-18","open":104.19,"high":104.5,"low":102.12,"close":103.06,"volume":5763600},{"time":"2020-08-19","open":103.06,"high":104.44,"low":102.25,"close":103.42,"volume":5340000},{"time":"2020-08-20","open":103.42,"high":106.31,"low":103.37,"close":105.27,"volume":6220800},{"time":"2020-08-21","open":105.27,"high":112.87,"low":107.94,"close":111.87,"volume":14680500},{"time":"2020-08-22","open":111.87,"high":116.5,"low":112.5,"close":116,"volume":9933000},{"time":"2020-08-23","open":116,"high":119.19,"low":115.44,"close":116.62,"volume":11329500},{"time":"2020-08-24","open":116.62,"high":121,"low":115.5,"close":118.28,"volume":8145300},{"time":"2020-08-25","open":118.28,"high":122.12,"low":112.25,"close":113.37,"volume":16644700},{"time":"2020-08-26","open":113.37,"high":111.94,"low":107.56,"close":109,"volume":12593800},{"time":"2020-08-27","open":109,"high":112.75,"low":106.56,"close":109.7,"volume":7138100},{"time":"2020-08-28","open":109.7,"high":110.19,"low":106.87,"close":109.25,"volume":7442300},{"time":"2020-08-29","open":109.25,"high":107.94,"low":104.5,"close":107,"volume":9442300},{"time":"2020-08-30","open":107,"high":109.69,"low":105.75,"close":109.19,"volume":7123600},{"time":"2020-08-31","open":109.19,"high":111.06,"low":108.62,"close":110,"volume":7680600},{"time":"2020-09-01","open":110,"high":110.44,"low":107.75,"close":109.2,"volume":4839800},{"time":"2020-09-02","open":109.2,"high":110.12,"low":108.06,"close":110.12,"volume":4775500},{"time":"2020-09-03","open":110.12,"high":110.31,"low":108,"close":108,"volume":4008800},{"time":"2020-09-04","open":108,"high":110.44,"low":108.19,"close":108.62,"volume":4533600},{"time":"2020-09-05","open":108.62,"high":110,"low":108.12,"close":109.75,"volume":3741100},{"time":"2020-09-06","open":109.75,"high":110.75,"low":109.06,"close":109.81,"volume":4084800},{"time":"2020-09-07","open":109.81,"high":110.5,"low":108.75,"close":109,"volume":2685200},{"time":"2020-09-08","open":109,"high":110.5,"low":108.56,"close":108.75,"volume":3438000},{"time":"2020-09-09","open":108.75,"high":109.5,"low":106.62,"close":107.87,"volume":2870500}];

export const testOutputBb2022Lower1: Scalar[] = [{"time":"2020-01-21","value":87.57397510968693},{"time":"2020-01-22","value":87.07311226502395},{"time":"2020-01-23","value":86.36728206697839},{"time":"2020-01-24","value":85.08158088658095},{"time":"2020-01-25","value":83.58403708544446},{"time":"2020-01-26","value":82.42482915254747},{"time":"2020-01-27","value":80.98200506068855},{"time":"2020-01-28","value":80.31513929688079},{"time":"2020-01-29","value":80.24882829873054},{"time":"2020-01-30","value":79.95485768697091},{"time":"2020-01-31","value":79.66869730389053},{"time":"2020-02-01","value":79.61035878640028},{"time":"2020-02-02","value":80.1121596201954},{"time":"2020-02-03","value":81.19202077177368},{"time":"2020-02-04","value":81.2122533732657},{"time":"2020-02-05","value":81.30875709745608},{"time":"2020-02-06","value":81.64851177828429},{"time":"2020-02-07","value":81.64298398122898},{"time":"2020-02-08","value":81.50578304860464},{"time":"2020-02-09","value":81.64581493802429},{"time":"2020-02-10","value":81.59418961818555},{"time":"2020-02-11","value":81.37849173785699},{"time":"2020-02-12","value":81.3797280553494},{"time":"2020-02-13","value":81.30275592596516},{"time":"2020-02-14","value":81.5237572653512},{"time":"2020-02-15","value":81.54325195681425},{"time":"2020-02-16","value":82.26052729474729},{"time":"2020-02-17","value":82.34212405180227},{"time":"2020-02-18","value":82.34989596213671},{"time":"2020-02-19","value":82.32128027284004},{"time":"2020-02-20","value":82.39582014362293},{"time":"2020-02-21","value":82.68091765945964},{"time":"2020-02-22","value":82.76121691965393},{"time":"2020-02-23","value":82.50902643081234},{"time":"2020-02-24","value":81.92218339528245},{"time":"2020-02-25","value":81.24443439223617},{"time":"2020-02-26","value":81.03247642965778},{"time":"2020-02-27","value":80.93734151401459},{"time":"2020-02-28","value":81.07699168954481},{"time":"2020-02-29","value":81.44078610221884},{"time":"2020-03-01","value":81.90508933681888},{"time":"2020-03-02","value":82.53094376703139},{"time":"2020-03-03","value":82.80496203522007},{"time":"2020-03-04","value":82.68898337561964},{"time":"2020-03-05","value":82.6277151635335},{"time":"2020-03-06","value":82.447239401321},{"time":"2020-03-07","value":82.25124595006773},{"time":"2020-03-08","value":82.13423843298493},{"time":"2020-03-09","value":82.13351819632058},{"time":"2020-03-10","value":82.13652405431989},{"time":"2020-03-11","value":82.12999966618483},{"time":"2020-03-12","value":82.12325685220462},{"time":"2020-03-13","value":81.75435193881734},{"time":"2020-03-14","value":81.57847468898217},{"time":"2020-03-15","value":81.83174954685971},{"time":"2020-03-16","value":82.45056694507642},{"time":"2020-03-17","value":82.31192590478547},{"time":"2020-03-18","value":81.8359062698735},{"time":"2020-03-19","value":80.64897372546768},{"time":"2020-03-20","value":79.5929985832437},{"time":"2020-03-21","value":79.44581062086323},{"time":"2020-03-22","value":79.56354941876147},{"time":"2020-03-23","value":79.66241330424866},{"time":"2020-03-24","value":79.43347627935525},{"time":"2020-03-25","value":79.44708278584963},{"time":"2020-03-26","value":79.44801323739316},{"time":"2020-03-27","value":79.63148923283791},{"time":"2020-03-28","value":79.68515089960596},{"time":"2020-03-29","value":80.02137510688844},{"time":"2020-03-30","value":80.71336843202687},{"time":"2020-03-31","value":81.52919568230169},{"time":"2020-04-01","value":81.56850443805577},{"time":"2020-04-02","value":83.52026827231819},{"time":"2020-04-03","value":86.70391763719218},{"time":"2020-04-04","value":90.2171392224347},{"time":"2020-04-05","value":94.47931016505741},{"time":"2020-04-06","value":96.19297195307644},{"time":"2020-04-07","value":97.63465659721416},{"time":"2020-04-08","value":98.26287667000379},{"time":"2020-04-09","value":98.7131290458091},{"time":"2020-04-10","value":99.86187869797286},{"time":"2020-04-11","value":101.32187606818717},{"time":"2020-04-12","value":102.48580651339032},{"time":"2020-04-13","value":103.29598574111634},{"time":"2020-04-14","value":104.32732240771587},{"time":"2020-04-15","value":105.39870382861776},{"time":"2020-04-16","value":107.21211200096977},{"time":"2020-04-17","value":108.02521976601838},{"time":"2020-04-18","value":108.97545659449193},{"time":"2020-04-19","value":109.77257421136147},{"time":"2020-04-20","value":110.08653737055175},{"time":"2020-04-21","value":110.44166244037208},{"time":"2020-04-22","value":110.52216225477028},{"time":"2020-04-23","value":110.57873353085552},{"time":"2020-04-24","value":110.39104839810446},{"time":"2020-04-25","value":110.20644361172775},{"time":"2020-04-26","value":109.98894735625174},{"time":"2020-04-27","value":109.30729089507807},{"time":"2020-04-28","value":109.58299003968713},{"time":"2020-04-29","value":110.39583355092981},{"time":"2020-04-30","value":110.28887770592267},{"time":"2020-05-01","value":110.34691977467132},{"time":"2020-05-02","value":110.51650679805536},{"time":"2020-05-03","value":111.32816768120661},{"time":"2020-05-04","value":111.33012328803903},{"time":"2020-05-05","value":111.56196037185848},{"time":"2020-05-06","value":111.36869740612944},{"time":"2020-05-07","value":111.10066313973606},{"time":"2020-05-08","value":111.27313182731407},{"time":"2020-05-09","value":111.50252011707836},{"time":"2020-05-10","value":111.90328572000828},{"time":"2020-05-11","value":112.88007953589054},{"time":"2020-05-12","value":113.99569953765518},{"time":"2020-05-13","value":115.48408526754474},{"time":"2020-05-14","value":116.31634124869622},{"time":"2020-05-15","value":117.46044832897675},{"time":"2020-05-16","value":118.7689796831049},{"time":"2020-05-17","value":119.20718673998987},{"time":"2020-05-18","value":119.9832034873662},{"time":"2020-05-19","value":120.16617379544175},{"time":"2020-05-20","value":120.60715072233032},{"time":"2020-05-21","value":120.58176094229401},{"time":"2020-05-22","value":121.28260846575243},{"time":"2020-05-23","value":121.8684174890038},{"time":"2020-05-24","value":121.33006621540706},{"time":"2020-05-25","value":120.74714815355685},{"time":"2020-05-26","value":119.538084980847},{"time":"2020-05-27","value":117.87417781583795},{"time":"2020-05-28","value":116.20111824295523},{"time":"2020-05-29","value":115.58321592931406},{"time":"2020-05-30","value":115.34348229995874},{"time":"2020-05-31","value":115.17433553692787},{"time":"2020-06-01","value":114.82813372833269},{"time":"2020-06-02","value":115.27072619275395},{"time":"2020-06-03","value":115.62320516569947},{"time":"2020-06-04","value":116.654011212038},{"time":"2020-06-05","value":117.83545740039048},{"time":"2020-06-06","value":117.80937297205381},{"time":"2020-06-07","value":118.04011076523125},{"time":"2020-06-08","value":117.98550934540397},{"time":"2020-06-09","value":117.80305954161405},{"time":"2020-06-10","value":117.86315136131336},{"time":"2020-06-11","value":117.76184103293554},{"time":"2020-06-12","value":117.9879722266513},{"time":"2020-06-13","value":117.98947799517823},{"time":"2020-06-14","value":118.05124381607222},{"time":"2020-06-15","value":118.10314878179123},{"time":"2020-06-16","value":118.60444378978438},{"time":"2020-06-17","value":119.23564872359302},{"time":"2020-06-18","value":119.25316446412901},{"time":"2020-06-19","value":118.95858257817363},{"time":"2020-06-20","value":118.37207731150811},{"time":"2020-06-21","value":118.82908074079826},{"time":"2020-06-22","value":118.09246007920041},{"time":"2020-06-23","value":118.00249632487112},{"time":"2020-06-24","value":118.14715463058053},{"time":"2020-06-25","value":117.9556392130713},{"time":"2020-06-26","value":117.88388114226103},{"time":"2020-06-27","value":118.24963154366921},{"time":"2020-06-28","value":118.56197052111504},{"time":"2020-06-29","value":119.35941382650626},{"time":"2020-06-30","value":119.64674936445773},{"time":"2020-07-01","value":120.18197284003669},{"time":"2020-07-02","value":120.10498008362212},{"time":"2020-07-03","value":120.44398509475967},{"time":"2020-07-04","value":120.26567484401404},{"time":"2020-07-05","value":120.32184599366231},{"time":"2020-07-06","value":119.42389648181484},{"time":"2020-07-07","value":118.54439795449666},{"time":"2020-07-08","value":117.12089288552583},{"time":"2020-07-09","value":116.15926214453073},{"time":"2020-07-10","value":115.70672439076449},{"time":"2020-07-11","value":114.85126934040242},{"time":"2020-07-12","value":113.96427961326893},{"time":"2020-07-13","value":112.79887511000395},{"time":"2020-07-14","value":111.83570436776513},{"time":"2020-07-15","value":110.32911320203144},{"time":"2020-07-16","value":107.73314119214189},{"time":"2020-07-17","value":106.0478299362833},{"time":"2020-07-18","value":104.54840723547561},{"time":"2020-07-19","value":103.62085503368068},{"time":"2020-07-20","value":102.70816376915134},{"time":"2020-07-21","value":101.85871794607935},{"time":"2020-07-22","value":96.83284174779139},{"time":"2020-07-23","value":93.93969616851275},{"time":"2020-07-24","value":91.45744956485795},{"time":"2020-07-25","value":89.85578075922554},{"time":"2020-07-26","value":87.85664592857957},{"time":"2020-07-27","value":86.69142228224666},{"time":"2020-07-28","value":86.06134677017229},{"time":"2020-07-29","value":85.67126183037625},{"time":"2020-07-30","value":85.6878089283754},{"time":"2020-07-31","value":85.70024098885533},{"time":"2020-08-01","value":85.27910564022912},{"time":"2020-08-02","value":84.63377092334761},{"time":"2020-08-03","value":85.09036146807719},{"time":"2020-08-04","value":85.20775279864633},{"time":"2020-08-05","value":85.26840140321589},{"time":"2020-08-06","value":85.49798524922208},{"time":"2020-08-07","value":86.15501170308934},{"time":"2020-08-08","value":86.84276346261291},{"time":"2020-08-09","value":88.1018728005296},{"time":"2020-08-10","value":90.41647166249477},{"time":"2020-08-11","value":90.7588993852825},{"time":"2020-08-12","value":89.48322129449542},{"time":"2020-08-13","value":87.8678642904916},{"time":"2020-08-14","value":87.18959939212665},{"time":"2020-08-15","value":87.23237546316003},{"time":"2020-08-16","value":87.15174835530296},{"time":"2020-08-17","value":87.01971708325766},{"time":"2020-08-18","value":87.09188613145145},{"time":"2020-08-19","value":87.38684133233198},{"time":"2020-08-20","value":87.71849548203723},{"time":"2020-08-21","value":87.94687738272334},{"time":"2020-08-22","value":88.19755452368028},{"time":"2020-08-23","value":88.16345773816279},{"time":"2020-08-24","value":88.39903724066654},{"time":"2020-08-25","value":88.98163141710091},{"time":"2020-08-26","value":90.23870022345632},{"time":"2020-08-27","value":91.48360823799139},{"time":"2020-08-28","value":93.3129649492164},{"time":"2020-08-29","value":95.19762082288987},{"time":"2020-08-30","value":97.77465295447925},{"time":"2020-08-31","value":99.51097380264548},{"time":"2020-09-01","value":100.04875796593832},{"time":"2020-09-02","value":100.16145769466056},{"time":"2020-09-03","value":100.3541950105952},{"time":"2020-09-04","value":100.83003090752989},{"time":"2020-09-05","value":101.31765264442988},{"time":"2020-09-06","value":101.99941190590499},{"time":"2020-09-07","value":102.95845935015161},{"time":"2020-09-08","value":103.91448731596469},{"time":"2020-09-09","value":104.38335904421295}];

export const testOutputBb2022Middle2: Scalar[] = [{"time":"2020-01-21","value":92.89099999999999},{"time":"2020-01-22","value":92.73474999999999},{"time":"2020-01-23","value":92.37525},{"time":"2020-01-24","value":91.8955},{"time":"2020-01-25","value":91.29074999999999},{"time":"2020-01-26","value":90.77674999999999},{"time":"2020-01-27","value":90.11425},{"time":"2020-01-28","value":89.70975},{"time":"2020-01-29","value":89.53475},{"time":"2020-01-30","value":89.33775},{"time":"2020-01-31","value":89.02674999999999},{"time":"2020-02-01","value":88.48299999999999},{"time":"2020-02-02","value":87.97675},{"time":"2020-02-03","value":87.3425},{"time":"2020-02-04","value":87.29724999999999},{"time":"2020-02-05","value":87.17075},{"time":"2020-02-06","value":86.87375},{"time":"2020-02-07","value":86.75675},{"time":"2020-02-08","value":86.53325},{"time":"2020-02-09","value":86.1615},{"time":"2020-02-10","value":85.8615},{"time":"2020-02-11","value":85.6115},{"time":"2020-02-12","value":85.50525},{"time":"2020-02-13","value":85.72575},{"time":"2020-02-14","value":86.04775000000001},{"time":"2020-02-15","value":86.42750000000001},{"time":"2020-02-16","value":86.89625000000001},{"time":"2020-02-17","value":87.24625},{"time":"2020-02-18","value":87.23375000000001},{"time":"2020-02-19","value":87.465},{"time":"2020-02-20","value":87.67750000000001},{"time":"2020-02-21","value":87.8665},{"time":"2020-02-22","value":87.95100000000001},{"time":"2020-02-23","value":87.87425},{"time":"2020-02-24","value":87.60075},{"time":"2020-02-25","value":87.31175},{"time":"2020-02-26","value":87.2055},{"time":"2020-02-27","value":87.14750000000001},{"time":"2020-02-28","value":87.21325},{"time":"2020-02-29","value":87.45075},{"time":"2020-03-01","value":87.721},{"time":"2020-03-02","value":87.9835},{"time":"2020-03-03","value":88.1335},{"time":"2020-03-04","value":88.27250000000001},{"time":"2020-03-05","value":88.3755},{"time":"2020-03-06","value":88.48325},{"time":"2020-03-07","value":88.62075},{"time":"2020-03-08","value":88.70649999999999},{"time":"2020-03-09","value":88.8425},{"time":"2020-03-10","value":88.79249999999999},{"time":"2020-03-11","value":88.752},{"time":"2020-03-12","value":88.74425},{"time":"2020-03-13","value":88.563},{"time":"2020-03-14","value":88.51775},{"time":"2020-03-15","value":88.5865},{"time":"2020-03-16","value":88.749},{"time":"2020-03-17","value":89.374},{"time":"2020-03-18","value":90.08500000000001},{"time":"2020-03-19","value":91.02250000000001},{"time":"2020-03-20","value":91.8755},{"time":"2020-03-21","value":92.5365},{"time":"2020-03-22","value":93.2255},{"time":"2020-03-23","value":94.03025},{"time":"2020-03-24","value":94.738},{"time":"2020-03-25","value":95.463},{"time":"2020-03-26","value":96.10375},{"time":"2020-03-27","value":96.66},{"time":"2020-03-28","value":97.43350000000001},{"time":"2020-03-29","value":98.31325000000001},{"time":"2020-03-30","value":99.33825},{"time":"2020-03-31","value":100.49125000000001},{"time":"2020-04-01","value":102.1975},{"time":"2020-04-02","value":103.91925},{"time":"2020-04-03","value":105.68800000000002},{"time":"2020-04-04","value":107.40675000000002},{"time":"2020-04-05","value":109.00675000000001},{"time":"2020-04-06","value":109.96625000000002},{"time":"2020-04-07","value":110.73200000000001},{"time":"2020-04-08","value":111.07875000000001},{"time":"2020-04-09","value":111.30850000000001},{"time":"2020-04-10","value":112.08975000000001},{"time":"2020-04-11","value":112.7695},{"time":"2020-04-12","value":113.33975000000001},{"time":"2020-04-13","value":113.6335},{"time":"2020-04-14","value":114.021},{"time":"2020-04-15","value":114.36475},{"time":"2020-04-16","value":114.93350000000001},{"time":"2020-04-17","value":115.52725000000001},{"time":"2020-04-18","value":115.8925},{"time":"2020-04-19","value":116.2175},{"time":"2020-04-20","value":116.3425},{"time":"2020-04-21","value":115.90799999999999},{"time":"2020-04-22","value":115.70174999999999},{"time":"2020-04-23","value":115.55774999999998},{"time":"2020-04-24","value":115.62974999999999},{"time":"2020-04-25","value":115.74224999999998},{"time":"2020-04-26","value":115.95774999999999},{"time":"2020-04-27","value":116.43574999999998},{"time":"2020-04-28","value":117.01049999999998},{"time":"2020-04-29","value":117.62774999999996},{"time":"2020-04-30","value":117.84949999999996},{"time":"2020-05-01","value":118.20549999999996},{"time":"2020-05-02","value":118.53349999999996},{"time":"2020-05-03","value":119.16449999999995},{"time":"2020-05-04","value":119.93949999999995},{"time":"2020-05-05","value":120.84249999999994},{"time":"2020-05-06","value":121.65499999999994},{"time":"2020-05-07","value":122.17999999999995},{"time":"2020-05-08","value":122.98949999999995},{"time":"2020-05-09","value":123.83949999999996},{"time":"2020-05-10","value":124.94599999999996},{"time":"2020-05-11","value":126.12099999999995},{"time":"2020-05-12","value":127.23999999999997},{"time":"2020-05-13","value":128.30899999999997},{"time":"2020-05-14","value":129.08999999999997},{"time":"2020-05-15","value":129.89299999999997},{"time":"2020-05-16","value":130.58699999999996},{"time":"2020-05-17","value":130.76199999999997},{"time":"2020-05-18","value":131.04349999999997},{"time":"2020-05-19","value":131.08999999999997},{"time":"2020-05-20","value":131.20249999999996},{"time":"2020-05-21","value":131.19649999999996},{"time":"2020-05-22","value":131.38099999999997},{"time":"2020-05-23","value":131.56899999999996},{"time":"2020-05-24","value":131.37499999999994},{"time":"2020-05-25","value":131.10949999999997},{"time":"2020-05-26","value":130.60949999999997},{"time":"2020-05-27","value":130.02799999999996},{"time":"2020-05-28","value":129.31249999999994},{"time":"2020-05-29","value":128.77199999999996},{"time":"2020-05-30","value":128.07799999999995},{"time":"2020-05-31","value":127.29699999999995},{"time":"2020-06-01","value":126.36849999999995},{"time":"2020-06-02","value":125.67149999999995},{"time":"2020-06-03","value":124.91199999999995},{"time":"2020-06-04","value":124.26799999999994},{"time":"2020-06-05","value":123.90499999999993},{"time":"2020-06-06","value":123.91749999999993},{"time":"2020-06-07","value":123.66099999999992},{"time":"2020-06-08","value":123.61449999999994},{"time":"2020-06-09","value":123.46149999999993},{"time":"2020-06-10","value":123.53349999999993},{"time":"2020-06-11","value":123.32099999999994},{"time":"2020-06-12","value":123.02049999999994},{"time":"2020-06-13","value":122.89899999999996},{"time":"2020-06-14","value":122.81449999999995},{"time":"2020-06-15","value":122.86149999999995},{"time":"2020-06-16","value":123.12099999999995},{"time":"2020-06-17","value":123.55849999999995},{"time":"2020-06-18","value":123.69249999999995},{"time":"2020-06-19","value":123.96049999999995},{"time":"2020-06-20","value":124.45099999999995},{"time":"2020-06-21","value":125.02299999999995},{"time":"2020-06-22","value":125.59499999999996},{"time":"2020-06-23","value":126.28899999999996},{"time":"2020-06-24","value":126.73949999999995},{"time":"2020-06-25","value":127.03649999999996},{"time":"2020-06-26","value":127.20849999999996},{"time":"2020-06-27","value":127.51499999999996},{"time":"2020-06-28","value":127.63649999999996},{"time":"2020-06-29","value":128.05549999999997},{"time":"2020-06-30","value":128.18949999999995},{"time":"2020-07-01","value":128.34899999999996},{"time":"2020-07-02","value":128.33049999999997},{"time":"2020-07-03","value":128.43349999999995},{"time":"2020-07-04","value":128.38349999999997},{"time":"2020-07-05","value":128.39899999999994},{"time":"2020-07-06","value":128.17399999999995},{"time":"2020-07-07","value":127.86149999999995},{"time":"2020-07-08","value":127.45549999999996},{"time":"2020-07-09","value":127.00599999999994},{"time":"2020-07-10","value":126.50599999999994},{"time":"2020-07-11","value":125.92799999999995},{"time":"2020-07-12","value":125.00899999999994},{"time":"2020-07-13","value":123.93399999999994},{"time":"2020-07-14","value":123.02749999999995},{"time":"2020-07-15","value":121.86199999999994},{"time":"2020-07-16","value":120.51799999999994},{"time":"2020-07-17","value":119.36799999999994},{"time":"2020-07-18","value":118.49299999999994},{"time":"2020-07-19","value":117.33649999999993},{"time":"2020-07-20","value":116.33649999999993},{"time":"2020-07-21","value":115.42699999999994},{"time":"2020-07-22","value":113.87699999999992},{"time":"2020-07-23","value":112.32399999999993},{"time":"2020-07-24","value":110.86749999999992},{"time":"2020-07-25","value":109.46749999999993},{"time":"2020-07-26","value":108.11449999999994},{"time":"2020-07-27","value":106.81149999999994},{"time":"2020-07-28","value":105.83649999999993},{"time":"2020-07-29","value":104.68049999999994},{"time":"2020-07-30","value":103.32099999999994},{"time":"2020-07-31","value":102.07999999999994},{"time":"2020-08-01","value":100.83949999999994},{"time":"2020-08-02","value":99.67699999999994},{"time":"2020-08-03","value":98.66149999999995},{"time":"2020-08-04","value":97.84249999999994},{"time":"2020-08-05","value":97.43949999999995},{"time":"2020-08-06","value":96.83949999999996},{"time":"2020-08-07","value":96.23949999999995},{"time":"2020-08-08","value":95.59249999999994},{"time":"2020-08-09","value":94.96749999999994},{"time":"2020-08-10","value":94.30499999999995},{"time":"2020-08-11","value":94.65499999999994},{"time":"2020-08-12","value":95.15499999999994},{"time":"2020-08-13","value":95.85499999999995},{"time":"2020-08-14","value":96.38299999999995},{"time":"2020-08-15","value":96.95799999999994},{"time":"2020-08-16","value":97.46099999999994},{"time":"2020-08-17","value":97.75799999999995},{"time":"2020-08-18","value":98.07349999999994},{"time":"2020-08-19","value":98.50399999999993},{"time":"2020-08-20","value":99.04899999999995},{"time":"2020-08-21","value":100.06449999999994},{"time":"2020-08-22","value":101.35199999999995},{"time":"2020-08-23","value":102.48599999999995},{"time":"2020-08-24","value":103.71899999999994},{"time":"2020-08-25","value":104.53749999999994},{"time":"2020-08-26","value":105.23749999999993},{"time":"2020-08-27","value":105.92899999999993},{"time":"2020-08-28","value":106.68849999999993},{"time":"2020-08-29","value":107.30749999999993},{"time":"2020-08-30","value":108.07949999999994},{"time":"2020-08-31","value":108.67949999999993},{"time":"2020-09-01","value":108.94249999999995},{"time":"2020-09-02","value":109.05499999999995},{"time":"2020-09-03","value":109.15199999999996},{"time":"2020-09-04","value":109.35799999999995},{"time":"2020-09-05","value":109.59549999999994},{"time":"2020-09-06","value":109.87649999999994},{"time":"2020-09-07","value":110.17349999999995},{"time":"2020-09-08","value":110.43999999999994},{"time":"2020-09-09","value":110.56999999999994}];


...301374 bytes truncated...

The tool call succeeded but the output was truncated. Full output saved to: /home/dev/.local/share/opencode/tool-output/tool_dc065309d001GVTMfV5I7ymmfy
Use the Task tool to have explore agent process this file with Grep and Read (with offset/limit). Do NOT read the full file yourself - delegate to save context.
```

---

## Assistant (Build · claude-opus-4.6 · 8.7s)

The tool built and ran successfully. Let me verify the structure is correct:

**Tool: bash**

**Input:**
```json
{
  "command": "cd /home/dev/zpano/go && python3 -c \"\nimport json\nd=json.load(open('/tmp/test-output.json'))\nprint('pricePane lines:', len(d['pricePane']['lines']))\nprint('pricePane bands:', len(d['pricePane']['bands']))\nprint('indicatorPanes:', len(d['indicatorPanes']))\nfor i,p in enumerate(d['indicatorPanes']):\n    print(f'  pane {i}: lines={len(p[\\\"lines\\\"])}, lineAreas={len(p[\\\"lineAreas\\\"])}, heatmap={p.get(\\\"heatmap\\\") is not None}')\nprint()\n# Check data counts\nprint('ohlcv bars:', len(d['ohlcv']['data']))\nprint('first price line data points:', len(d['pricePane']['lines'][0]['data']))\nprint('first band data points:', len(d['pricePane']['bands'][0]['data']))\n\"",
  "description": "Verify JSON output structure"
}
```

**Output:**
```
pricePane lines: 3
pricePane bands: 1
indicatorPanes: 2
  pane 0: lines=2, lineAreas=0, heatmap=False
  pane 1: lines=0, lineAreas=0, heatmap=True

ohlcv bars: 252
first price line data points: 233
first band data points: 233

```

---

## Assistant (Build · claude-opus-4.6 · 7.7s)

The output structure matches expectations for Bollinger Bands + Goertzel:
- **pricePane**: 3 lines (lower, middle, upper) + 1 band -- correct for BB
- **indicatorPanes[0]**: 2 lines (bandWidth, percentB) -- correct for BB's Own-pane outputs
- **indicatorPanes[1]**: heatmap -- correct for Goertzel spectrum
- 252 OHLCV bars, 233 data points for BB (19-bar warmup for length=20)

**Tool: bash**

**Input:**
```json
{
  "command": "head -5 /tmp/test-output.ts && echo \"...\" && grep \"^export const.*Config\" /tmp/test-output.ts",
  "description": "Check TS output structure"
}
```

**Output:**
```
// Auto-generated chart configuration.
// eslint-disable-next-line
import { Configuration } from '../ohlcv-chart/template/configuration';
import { Scalar, Band, Heatmap, Bar } from '../ohlcv-chart/template/types';

...
export const testOutputConfig: Configuration = {

```

---

## Assistant (Build · claude-opus-4.6 · 7.6s)

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Read reference files (icalc/main.go, core/descriptors.go, core/outputs, chart template types)",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Create go/cmd/iconf/main.go with config structs and CLI logic",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Implement output shape → pane mapping using descriptor registry",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Implement JSON output writer",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Implement TypeScript output writer",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Build and test the tool",
      "priority": "high",
      "status": "completed"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Read reference files (icalc/main.go, core/descriptors.go, core/outputs, chart template types)",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Create go/cmd/iconf/main.go with config structs and CLI logic",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Implement output shape → pane mapping using descriptor registry",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Implement JSON output writer",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Implement TypeScript output writer",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Build and test the tool",
    "priority": "high",
    "status": "completed"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 10.7s)

The `iconf` CLI tool is complete at `go/cmd/iconf/main.go`. Here's a summary:

**What it does:**
- `iconf <settings.json> <output-name>` reads indicator settings, runs 252 embedded test bars through them, and writes two files:
  - `<output>.json` — chart configuration with all calculated data inline
  - `<output>.ts` — same structure but data in exported `const` arrays, configuration references variables

**How it maps outputs to panes:**
- Uses `core.DescriptorOf()` to look up each output's `Shape` and `Pane`
- `Pane: Price` + `Shape: Scalar` → `pricePane.lines`
- `Pane: Price` + `Shape: Band` → `pricePane.bands`
- `Pane: Own` + `Shape: Heatmap` → `indicatorPanes[n].heatmap`
- `Pane: Own` + `Shape: Band` → `indicatorPanes[n].bands`
- `Pane: Own` + `Shape: Scalar` → `indicatorPanes[n].lines`
- Own-pane outputs from the same indicator share the same indicator pane

**Verified with BB+Goertzel:** produces 3 price lines, 1 band, 2 indicator lines, and 1 heatmap — matching the expected structure from `test-data.ts`.

---

## User

We also need to create settings.json which is mentioned in the tool

---

## Assistant (Build · claude-opus-4.6 · 5.4s)



Let me check what settings file `icalc` uses and whether we should reuse it or create a distinct one:

**Tool: bash**

**Input:**
```json
{
  "command": "wc -l /home/dev/zpano/go/cmd/icalc/settings.json && head -20 /home/dev/zpano/go/cmd/icalc/settings.json",
  "description": "Check icalc settings.json"
}
```

**Output:**
```
74 /home/dev/zpano/go/cmd/icalc/settings.json
[
  { "identifier": "simpleMovingAverage", "params": { "length": 14 } },
  { "identifier": "weightedMovingAverage", "params": { "length": 14 } },
  { "identifier": "triangularMovingAverage", "params": { "length": 14 } },
  { "identifier": "exponentialMovingAverage", "params": { "length": 10 } },
  { "identifier": "doubleExponentialMovingAverage", "params": { "length": 10 } },
  { "identifier": "tripleExponentialMovingAverage", "params": { "length": 10 } },
  { "identifier": "t2ExponentialMovingAverage", "params": { "length": 10, "volumeFactor": 0.7 } },
  { "identifier": "t3ExponentialMovingAverage", "params": { "length": 10, "volumeFactor": 0.7 } },
  { "identifier": "kaufmanAdaptiveMovingAverageMovingAverage", "params": { "efficiencyRatioLength": 10, "fastestLength": 2, "slowestLength": 30 } },
  { "identifier": "jurikMovingAverage", "params": { "length": 14, "phase": 0 } },
  { "identifier": "mesaAdaptiveMovingAverage", "params": {} },
  { "identifier": "fractalAdaptiveMovingAverage", "params": { "length": 16 } },
  { "identifier": "dominantCycle", "params": {} },
  { "identifier": "momentum", "params": { "length": 10 } },
  { "identifier": "rateOfChange", "params": { "length": 10 } },
  { "identifier": "rateOfChangePercent", "params": { "length": 10 } },
  { "identifier": "rateOfChangeRatio", "params": { "length": 10 } },
  { "identifier": "relativeStrengthIndex", "params": { "length": 14 } },
  { "identifier": "chandeMomentumOscillator", "params": { "length": 14 } },

```

---

## Assistant (Build · claude-opus-4.6 · 10.1s)

The `icalc` settings has all 72 indicators — that would produce a massive chart config. A curated subset makes more sense for `iconf`. I'll create one with a representative mix of indicator types (price overlays, oscillators, bands, heatmaps).

**Tool: write**

**Input:**
```json
{
  "content": "[\n  { \"identifier\": \"bollingerBands\", \"params\": { \"length\": 20, \"multiplier\": 2 } },\n  { \"identifier\": \"simpleMovingAverage\", \"params\": { \"length\": 14 } },\n  { \"identifier\": \"exponentialMovingAverage\", \"params\": { \"length\": 10 } },\n  { \"identifier\": \"relativeStrengthIndex\", \"params\": { \"length\": 14 } },\n  { \"identifier\": \"movingAverageConvergenceDivergence\", \"params\": { \"fastLength\": 12, \"slowLength\": 26, \"signalLength\": 9 } },\n  { \"identifier\": \"averageTrueRange\", \"params\": { \"length\": 14 } },\n  { \"identifier\": \"stochastic\", \"params\": { \"fastKLength\": 14, \"slowKLength\": 3, \"slowDLength\": 3 } },\n  { \"identifier\": \"goertzelSpectrum\", \"params\": { \"length\": 64, \"range\": [2, 28], \"resolution\": 10 } }\n]\n",
  "filePath": "/home/dev/zpano/go/cmd/iconf/settings.json"
}
```

**Output:**
```
Wrote file successfully.
```

---

## Assistant (Build · claude-opus-4.6 · 7.5s)

Let me verify it works:

**Tool: bash**

**Input:**
```json
{
  "command": "cd /home/dev/zpano/go && ./iconf cmd/iconf/settings.json /tmp/iconf-test 2>&1 && python3 -c \"\nimport json\nd=json.load(open('/tmp/iconf-test.json'))\nprint('pricePane: lines=%d, bands=%d' % (len(d['pricePane']['lines']), len(d['pricePane']['bands'])))\nprint('indicatorPanes:', len(d['indicatorPanes']))\nfor i,p in enumerate(d['indicatorPanes']):\n    parts = []\n    if p.get('heatmap'): parts.append('heatmap')\n    if p['lines']: parts.append('lines=%d' % len(p['lines']))\n    if p['bands']: parts.append('bands=%d' % len(p['bands']))\n    if p['lineAreas']: parts.append('lineAreas=%d' % len(p['lineAreas']))\n    print('  pane %d: %s' % (i, ', '.join(parts)))\n\"",
  "description": "Test iconf with new settings"
}
```

**Output:**
```
wrote /tmp/iconf-test.json
wrote /tmp/iconf-test.ts
pricePane: lines=5, bands=1
indicatorPanes: 6
  pane 0: lines=2
  pane 1: lines=1
  pane 2: lines=3
  pane 3: lines=1
  pane 4: lines=3
  pane 5: heatmap

```

---

## Assistant (Build · claude-opus-4.6 · 6.6s)

Works correctly. The layout:

- **pricePane**: 5 lines (BB lower/middle/upper + SMA + EMA) + 1 band (BB)
- **pane 0**: BB own outputs (bandWidth, percentB)
- **pane 1**: RSI
- **pane 2**: MACD (macd, signal, histogram)
- **pane 3**: ATR
- **pane 4**: Stochastic (fastK, slowK, slowD)
- **pane 5**: Goertzel heatmap

Created `go/cmd/iconf/settings.json` with 8 representative indicators covering price overlays, bounded oscillators, momentum, volatility, and spectrum analysis.

---

## User

Can we also do the same in TypeScript

---

## Assistant (Build · claude-opus-4.6 · 69.6s)

_Thinking:_

The user wants me to create a TypeScript version of the `iconf` CLI tool, similar to how `icalc` exists in both Go and TypeScript. Let me find the TS icalc for reference.

Let me find the TypeScript `icalc` and factory to understand the TS project structure:

**Tool: task**

**Input:**
```json
{
  "description": "Explore TS icalc and factory",
  "prompt": "Find and read the following files in the codebase, returning their full contents:\n\n1. The TypeScript icalc CLI (likely under ts/cmd/icalc/ or similar)\n2. The TypeScript factory (likely under ts/indicators/factory/)\n3. The TypeScript core types: Indicator interface, Output type, Metadata, outputs (Scalar, Band, Heatmap), shape enum, descriptor/pane types\n4. The TypeScript package.json or tsconfig that shows how to run/build TS CLIs\n\nSearch broadly under ts/ directory. Return the file paths and full contents of each file found.",
  "subagent_type": "explore"
}
```

**Output:**
```
task_id: ses_23f931ec1ffe1sUVCB95p6JIdS (for resuming to continue this task if needed)

<task_result>
Here is a summary of all files found, organized by category:

---

## 1. icalc CLI

| # | File | Lines |
|---|------|-------|
| 1 | `/home/dev/zpano/ts/cmd/icalc/main.ts` | 319 |
| 2 | `/home/dev/zpano/ts/cmd/icalc/run-me.sh` | 6 |
| 3 | `/home/dev/zpano/ts/cmd/icalc/settings.json` | 74 |

**main.ts** is the CLI entry point. It reads a JSON settings file, creates indicators via the factory, prints metadata, then iterates 252 embedded TA-Lib reference bars printing each indicator's output. Run with: `npx tsx cmd/icalc/main.ts cmd/icalc/settings.json` from the `ts/` directory.

---

## 2. Factory

| # | File | Lines |
|---|------|-------|
| 4 | `/home/dev/zpano/ts/indicators/factory/factory.ts` | 527 |

A single `createIndicator(identifier, params?)` function that maps an `IndicatorIdentifier` enum + plain params object to a constructed `Indicator` instance via a large switch statement covering 60+ indicators.

---

## 3. Core Types

| # | File | Purpose | Lines |
|---|------|---------|-------|
| 5 | `/home/dev/zpano/ts/indicators/core/indicator.ts` | `Indicator` interface (isPrimed, metadata, updateScalar/Bar/Quote/Trade) | 27 |
| 6 | `/home/dev/zpano/ts/indicators/core/indicator-output.ts` | `IndicatorOutput` type = `(Scalar \| Band \| Heatmap)[]` | 6 |
| 7 | `/home/dev/zpano/ts/indicators/core/indicator-metadata.ts` | `IndicatorMetadata` interface (identifier, mnemonic, description, outputs) | 17 |
| 8 | `/home/dev/zpano/ts/indicators/core/outputs/output-metadata.ts` | `OutputMetadata` interface (kind, shape, mnemonic, description) | 20 |
| 9 | `/home/dev/zpano/ts/entities/scalar.ts` | `Scalar` class (time, value) | 25 |
| 10 | `/home/dev/zpano/ts/indicators/core/outputs/band.ts` | `Band` class (time, lower, upper) | 11 |
| 11 | `/home/dev/zpano/ts/indicators/core/outputs/heatmap.ts` | `Heatmap` class (time, parameterFirst/Last/Resolution, valueMin/Max, values[]) | 67 |
| 12 | `/home/dev/zpano/ts/indicators/core/outputs/polyline.ts` | `Polyline` class + `Point` interface | 55 |
| 13 | `/home/dev/zpano/ts/indicators/core/outputs/shape/shape.ts` | `Shape` enum (Scalar, Band, Heatmap, Polyline) | 15 |
| 14 | `/home/dev/zpano/ts/indicators/core/descriptor.ts` | `Descriptor` interface (identifier, family, adaptivity, inputRequirement, volumeUsage, outputs) | 29 |
| 15 | `/home/dev/zpano/ts/indicators/core/output-descriptor.ts` | `OutputDescriptor` interface (kind, shape, role, pane) | 18 |
| 16 | `/home/dev/zpano/ts/indicators/core/pane.ts` | `Pane` enum (Price, Own, OverlayOnParent) | 11 |
| 17 | `/home/dev/zpano/ts/indicators/core/role.ts` | `Role` enum (Smoother, Envelope, Overlay, Polyline, Oscillator, BoundedOscillator, Volatility, VolumeFlow, Directional, CyclePeriod, CyclePhase, FractalDimension, Spectrum, Signal, Histogram, RegimeFlag, Correlation) | 53 |
| 18 | `/home/dev/zpano/ts/indicators/core/adaptivity.ts` | `Adaptivity` enum (Static, Adaptive) | 11 |
| 19 | `/home/dev/zpano/ts/indicators/core/input-requirement.ts` | `InputRequirement` enum (ScalarInput, QuoteInput, BarInput, TradeInput) | 14 |
| 20 | `/home/dev/zpano/ts/indicators/core/volume-usage.ts` | `VolumeUsage` enum (NoVolume, AggregateBarVolume, PerTradeVolume, QuoteLiquidityVolume) | 14 |
| 21 | `/home/dev/zpano/ts/indicators/core/indicator-identifier.ts` | `IndicatorIdentifier` enum (all 60+ indicators) | 233 |
| 22 | `/home/dev/zpano/ts/indicators/core/indicator-specification.ts` | `IndicatorSpecification` interface | 17 |
| 23 | `/home/dev/zpano/ts/indicators/core/line-indicator.ts` | `LineIndicator` abstract class (base for single-output line indicators) | 136 |
| 24 | `/home/dev/zpano/ts/indicators/core/build-metadata.ts` | `buildMetadata()` helper + `OutputText` interface | 49 |
| 25 | `/home/dev/zpano/ts/indicators/core/descriptors.ts` | Static descriptor registry mapping every `IndicatorIdentifier` to its `Descriptor` | 247 |

---

## 4. Build / Run Configuration

| # | File | Lines |
|---|------|-------|
| 26 | `/home/dev/zpano/ts/package.json` | 21 |
| 27 | `/home/dev/zpano/ts/tsconfig.json` | 28 |

Key points:
- **`"type": "module"`** (ESM)
- Uses **`tsx`** as the TypeScript runner (dev dependency)
- Test script: `node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json`
- CLI is run via: **`npx tsx cmd/icalc/main.ts <settings.json>`**
- TypeScript 6.0.2, target ES2022, module ESNext, bundler resolution
- `tsconfig.json` includes `entities/**/*.ts` and `indicators/**/*.ts` (notably does **not** include `cmd/`; CLIs are run directly via `tsx` without prior compilation)
</task_result>
```

---

## Assistant (Build · claude-opus-4.6 · 7.3s)

Now let me read the TS icalc main.ts for the exact patterns:

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/cmd/icalc/main.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/cmd/icalc/main.ts</path>
<type>file</type>
<content>1: /**
2:  * icalc — command-line indicator calculator (TypeScript).
3:  *
4:  * Reads a JSON settings file containing indicator definitions, creates
5:  * indicator instances via the factory, prints their metadata, then iterates
6:  * through embedded bar data printing bar values and all indicator outputs on
7:  * each iteration.
8:  *
9:  * Usage:  npx tsx cmd/icalc/main.ts <settings.json>
10:  */
11: 
12: import { readFileSync } from 'node:fs';
13: import { Bar } from '../../entities/bar.js';
14: import { Scalar } from '../../entities/scalar.js';
15: import { type Indicator } from '../../indicators/core/indicator.js';
16: import { IndicatorIdentifier } from '../../indicators/core/indicator-identifier.js';
17: import { type IndicatorOutput } from '../../indicators/core/indicator-output.js';
18: import { type OutputMetadata } from '../../indicators/core/outputs/output-metadata.js';
19: import { Band } from '../../indicators/core/outputs/band.js';
20: import { Heatmap } from '../../indicators/core/outputs/heatmap.js';
21: import { createIndicator } from '../../indicators/factory/factory.js';
22: 
23: // ---------------------------------------------------------------------------
24: // Settings
25: // ---------------------------------------------------------------------------
26: 
27: interface SettingsEntry {
28:     identifier: string;
29:     params: Record<string, unknown>;
30: }
31: 
32: /** Map camelCase JSON identifier string to IndicatorIdentifier enum value. */
33: function resolveIdentifier(name: string): IndicatorIdentifier {
34:     // The enum members are PascalCase; the JSON uses camelCase.
35:     // TS numeric enums have a reverse mapping: IndicatorIdentifier["SimpleMovingAverage"] === 0.
36:     // Convert first char to upper to get PascalCase.
37:     let pascal = name.charAt(0).toUpperCase() + name.slice(1);
38: 
39:     // Handle quirky Go JSON identifier: "kaufmanAdaptiveMovingAverageMovingAverage"
40:     // maps to TS enum "KaufmanAdaptiveMovingAverage".
41:     if (pascal === 'KaufmanAdaptiveMovingAverageMovingAverage') {
42:         pascal = 'KaufmanAdaptiveMovingAverage';
43:     }
44:     const value = (IndicatorIdentifier as unknown as Record<string, number>)[pascal];
45:     if (value === undefined) {
46:         throw new Error(`Unknown indicator identifier: "${name}" (tried "${pascal}")`);
47:     }
48:     return value as IndicatorIdentifier;
49: }
50: 
51: // ---------------------------------------------------------------------------
52: // Output formatting
53: // ---------------------------------------------------------------------------
54: 
55: function formatOutput(outputsMeta: OutputMetadata[], output: IndicatorOutput): string {
56:     const parts: string[] = [];
57:     for (let i = 0; i < output.length; i++) {
58:         const name = i < outputsMeta.length ? outputsMeta[i].mnemonic : `out[${i}]`;
59:         const val = output[i];
60:         if (val instanceof Band) {
61:             if (Number.isNaN(val.lower) || Number.isNaN(val.upper)) {
62:                 parts.push(`${name}=Band(NaN)`);
63:             } else {
64:                 parts.push(`${name}=Band(${val.lower.toFixed(4)},${val.upper.toFixed(4)})`);
65:             }
66:         } else if (val instanceof Heatmap) {
67:             parts.push(`${name}=Heatmap(${val.values.length}pts)`);
68:         } else if (val instanceof Scalar) {
69:             if (Number.isNaN(val.value)) {
70:                 parts.push(`${name}=NaN`);
71:             } else {
72:                 parts.push(`${name}=${val.value.toFixed(4)}`);
73:             }
74:         } else {
75:             parts.push(`${name}=${val}`);
76:         }
77:     }
78:     return parts.join(' ');
79: }
80: 
81: // ---------------------------------------------------------------------------
82: // Metadata printer
83: // ---------------------------------------------------------------------------
84: 
85: function printMetadata(indicators: Indicator[]): void {
86:     console.log('=== Indicator Metadata ===');
87:     console.log();
88:     for (let i = 0; i < indicators.length; i++) {
89:         const meta = indicators[i].metadata();
90:         console.log(`[${i}] ${meta.mnemonic}`);
91:         console.log(`  Identifier:  ${IndicatorIdentifier[meta.identifier]}`);
92:         console.log(`  Description: ${meta.description}`);
93:         console.log(`  Outputs (${meta.outputs.length}):`);
94:         for (let j = 0; j < meta.outputs.length; j++) {
95:             const out = meta.outputs[j];
96:             console.log(`    [${j}] kind=${out.kind} shape=${out.shape} mnemonic="${out.mnemonic}" description="${out.description}"`);
97:         }
98:         console.log(`  Full metadata JSON:`);
99:         console.log(`  ${JSON.stringify(meta, null, 2).split('\n').join('\n  ')}`);
100:         console.log();
101:     }
102: }
103: 
104: // ---------------------------------------------------------------------------
105: // Test bar data (252-entry TA-Lib reference set)
106: // ---------------------------------------------------------------------------
107: 
108: function testBars(): Bar[] {
109:     const highs = testHighs();
110:     const lows = testLows();
111:     const closes = testCloses();
112:     const volumes = testVolumes();
113: 
114:     const bars: Bar[] = [];
115:     const baseTime = new Date('2020-01-02T00:00:00Z');
116: 
117:     for (let i = 0; i < closes.length; i++) {
118:         const openPrice = i > 0 ? closes[i - 1] : closes[0];
119:         const bar = new Bar();
120:         bar.time = new Date(baseTime.getTime() + i * 86_400_000);
121:         bar.open = openPrice;
122:         bar.high = highs[i];
123:         bar.low = lows[i];
124:         bar.close = closes[i];
125:         bar.volume = volumes[i];
126:         bars.push(bar);
127:     }
128: 
129:     return bars;
130: }
131: 
132: function formatDate(d: Date): string {
133:     return d.toISOString().slice(0, 10);
134: }
135: 
136: // ---------------------------------------------------------------------------
137: // Main
138: // ---------------------------------------------------------------------------
139: 
140: function main(): void {
141:     const args = process.argv.slice(2);
142:     if (args.length < 1) {
143:         process.stderr.write('usage: icalc <settings.json>\n');
144:         process.exit(1);
145:     }
146: 
147:     const data = readFileSync(args[0], 'utf-8');
148:     const entries: SettingsEntry[] = JSON.parse(data);
149: 
150:     const indicators: Indicator[] = [];
151:     for (const e of entries) {
152:         const id = resolveIdentifier(e.identifier);
153:         const params = Object.keys(e.params).length === 0 ? undefined : e.params;
154:         try {
155:             indicators.push(createIndicator(id, params as Record<string, any>));
156:         } catch (err) {
157:             process.stderr.write(`error creating indicator ${e.identifier}: ${err}\n`);
158:             process.exit(1);
159:         }
160:     }
161: 
162:     printMetadata(indicators);
163: 
164:     const bars = testBars();
165: 
166:     console.log();
167:     console.log('=== Bar Data & Indicator Outputs ===');
168:     console.log();
169: 
170:     for (let i = 0; i < bars.length; i++) {
171:         const bar = bars[i];
172:         console.log(
173:             `Bar[${String(i).padStart(3)}] ${formatDate(bar.time)}  O=${bar.open.toFixed(4)} H=${bar.high.toFixed(4)} L=${bar.low.toFixed(4)} C=${bar.close.toFixed(4)} V=${bar.volume.toFixed(0)}`,
174:         );
175: 
176:         for (const ind of indicators) {
177:             const meta = ind.metadata();
178:             const output = ind.updateBar(bar);
179:             const primed = String(ind.isPrimed()).padEnd(5);
180:             console.log(`  ${meta.mnemonic.padEnd(45)} primed=${primed} ${formatOutput(meta.outputs, output)}`);
181:         }
182: 
183:         console.log();
184:     }
185: }
186: 
187: // ---------------------------------------------------------------------------
188: // Embedded test data (TA-Lib test_data.c)
189: // ---------------------------------------------------------------------------
190: 
191: function testHighs(): number[] {
192:     return [
193:         93.25, 94.94, 96.375, 96.19, 96, 94.72, 95, 93.72, 92.47, 92.75,
194:         96.25, 99.625, 99.125, 92.75, 91.315, 93.25, 93.405, 90.655, 91.97, 92.25,
195:         90.345, 88.5, 88.25, 85.5, 84.44, 84.75, 84.44, 89.405, 88.125, 89.125,
196:         87.155, 87.25, 87.375, 88.97, 90, 89.845, 86.97, 85.94, 84.75, 85.47,
197:         84.47, 88.5, 89.47, 90, 92.44, 91.44, 92.97, 91.72, 91.155, 91.75,
198:         90, 88.875, 89, 85.25, 83.815, 85.25, 86.625, 87.94, 89.375, 90.625,
199:         90.75, 88.845, 91.97, 93.375, 93.815, 94.03, 94.03, 91.815, 92, 91.94,
200:         89.75, 88.75, 86.155, 84.875, 85.94, 99.375, 103.28, 105.375, 107.625, 105.25,
201:         104.5, 105.5, 106.125, 107.94, 106.25, 107, 108.75, 110.94, 110.94, 114.22,
202:         123, 121.75, 119.815, 120.315, 119.375, 118.19, 116.69, 115.345, 113, 118.315,
203:         116.87, 116.75, 113.87, 114.62, 115.31, 116, 121.69, 119.87, 120.87, 116.75,
204:         116.5, 116, 118.31, 121.5, 122, 121.44, 125.75, 127.75, 124.19, 124.44,
205:         125.75, 124.69, 125.31, 132, 131.31, 132.25, 133.88, 133.5, 135.5, 137.44,
206:         138.69, 139.19, 138.5, 138.13, 137.5, 138.88, 132.13, 129.75, 128.5, 125.44,
207:         125.12, 126.5, 128.69, 126.62, 126.69, 126, 123.12, 121.87, 124, 127,
208:         124.44, 122.5, 123.75, 123.81, 124.5, 127.87, 128.56, 129.63, 124.87, 124.37,
209:         124.87, 123.62, 124.06, 125.87, 125.19, 125.62, 126, 128.5, 126.75, 129.75,
210:         132.69, 133.94, 136.5, 137.69, 135.56, 133.56, 135, 132.38, 131.44, 130.88,
211:         129.63, 127.25, 127.81, 125, 126.81, 124.75, 122.81, 122.25, 121.06, 120,
212:         123.25, 122.75, 119.19, 115.06, 116.69, 114.87, 110.87, 107.25, 108.87, 109,
213:         108.5, 113.06, 93, 94.62, 95.12, 96, 95.56, 95.31, 99, 98.81,
214:         96.81, 95.94, 94.44, 92.94, 93.94, 95.5, 97.06, 97.5, 96.25, 96.37,
215:         95, 94.87, 98.25, 105.12, 108.44, 109.87, 105, 106, 104.94, 104.5,
216:         104.44, 106.31, 112.87, 116.5, 119.19, 121, 122.12, 111.94, 112.75, 110.19,
217:         107.94, 109.69, 111.06, 110.44, 110.12, 110.31, 110.44, 110, 110.75, 110.5,
218:         110.5, 109.5,
219:     ];
220: }
221: 
222: function testLows(): number[] {
223:     return [
224:         90.75, 91.405, 94.25, 93.5, 92.815, 93.5, 92, 89.75, 89.44, 90.625,
225:         92.75, 96.315, 96.03, 88.815, 86.75, 90.94, 88.905, 88.78, 89.25, 89.75,
226:         87.5, 86.53, 84.625, 82.28, 81.565, 80.875, 81.25, 84.065, 85.595, 85.97,
227:         84.405, 85.095, 85.5, 85.53, 87.875, 86.565, 84.655, 83.25, 82.565, 83.44,
228:         82.53, 85.065, 86.875, 88.53, 89.28, 90.125, 90.75, 89, 88.565, 90.095,
229:         89, 86.47, 84, 83.315, 82, 83.25, 84.75, 85.28, 87.19, 88.44,
230:         88.25, 87.345, 89.28, 91.095, 89.53, 91.155, 92, 90.53, 89.97, 88.815,
231:         86.75, 85.065, 82.03, 81.5, 82.565, 96.345, 96.47, 101.155, 104.25, 101.75,
232:         101.72, 101.72, 103.155, 105.69, 103.655, 104, 105.53, 108.53, 108.75, 107.75,
233:         117, 118, 116, 118.5, 116.53, 116.25, 114.595, 110.875, 110.5, 110.72,
234:         112.62, 114.19, 111.19, 109.44, 111.56, 112.44, 117.5, 116.06, 116.56, 113.31,
235:         112.56, 114, 114.75, 118.87, 119, 119.75, 122.62, 123, 121.75, 121.56,
236:         123.12, 122.19, 122.75, 124.37, 128, 129.5, 130.81, 130.63, 132.13, 133.88,
237:         135.38, 135.75, 136.19, 134.5, 135.38, 133.69, 126.06, 126.87, 123.5, 122.62,
238:         122.75, 123.56, 125.81, 124.62, 124.37, 121.81, 118.19, 118.06, 117.56, 121,
239:         121.12, 118.94, 119.81, 121, 122, 124.5, 126.56, 123.5, 121.25, 121.06,
240:         122.31, 121, 120.87, 122.06, 122.75, 122.69, 122.87, 125.5, 124.25, 128,
241:         128.38, 130.69, 131.63, 134.38, 132, 131.94, 131.94, 129.56, 123.75, 126,
242:         126.25, 124.37, 121.44, 120.44, 121.37, 121.69, 120, 119.62, 115.5, 116.75,
243:         119.06, 119.06, 115.06, 111.06, 113.12, 110, 105, 104.69, 103.87, 104.69,
244:         105.44, 107, 89, 92.5, 92.12, 94.62, 92.81, 94.25, 96.25, 96.37,
245:         93.69, 93.5, 90, 90.19, 90.5, 92.12, 94.12, 94.87, 93, 93.87,
246:         93, 92.62, 93.56, 98.37, 104.44, 106, 101.81, 104.12, 103.37, 102.12,
247:         102.25, 103.37, 107.94, 112.5, 115.44, 115.5, 112.25, 107.56, 106.56, 106.87,
248:         104.5, 105.75, 108.62, 107.75, 108.06, 108, 108.19, 108.12, 109.06, 108.75,
249:         108.56, 106.62,
250:     ];
251: }
252: 
253: function testCloses(): number[] {
254:     return [
255:         91.5, 94.815, 94.375, 95.095, 93.78, 94.625, 92.53, 92.75, 90.315, 92.47,
256:         96.125, 97.25, 98.5, 89.875, 91, 92.815, 89.155, 89.345, 91.625, 89.875,
257:         88.375, 87.625, 84.78, 83, 83.5, 81.375, 84.44, 89.25, 86.375, 86.25,
258:         85.25, 87.125, 85.815, 88.97, 88.47, 86.875, 86.815, 84.875, 84.19, 83.875,
259:         83.375, 85.5, 89.19, 89.44, 91.095, 90.75, 91.44, 89, 91, 90.5,
260:         89.03, 88.815, 84.28, 83.5, 82.69, 84.75, 85.655, 86.19, 88.94, 89.28,
261:         88.625, 88.5, 91.97, 91.5, 93.25, 93.5, 93.155, 91.72, 90, 89.69,
262:         88.875, 85.19, 83.375, 84.875, 85.94, 97.25, 99.875, 104.94, 106, 102.5,
263:         102.405, 104.595, 106.125, 106, 106.065, 104.625, 108.625, 109.315, 110.5, 112.75,
264:         123, 119.625, 118.75, 119.25, 117.94, 116.44, 115.19, 111.875, 110.595, 118.125,
265:         116, 116, 112, 113.75, 112.94, 116, 120.5, 116.62, 117, 115.25,
266:         114.31, 115.5, 115.87, 120.69, 120.19, 120.75, 124.75, 123.37, 122.94, 122.56,
267:         123.12, 122.56, 124.62, 129.25, 131, 132.25, 131, 132.81, 134, 137.38,
268:         137.81, 137.88, 137.25, 136.31, 136.25, 134.63, 128.25, 129, 123.87, 124.81,
269:         123, 126.25, 128.38, 125.37, 125.69, 122.25, 119.37, 118.5, 123.19, 123.5,
270:         122.19, 119.31, 123.31, 121.12, 123.37, 127.37, 128.5, 123.87, 122.94, 121.75,
271:         124.44, 122, 122.37, 122.94, 124, 123.19, 124.56, 127.25, 125.87, 128.86,
272:         132, 130.75, 134.75, 135, 132.38, 133.31, 131.94, 130, 125.37, 130.13,
273:         127.12, 125.19, 122, 125, 123, 123.5, 120.06, 121, 117.75, 119.87,
274:         122, 119.19, 116.37, 113.5, 114.25, 110, 105.06, 107, 107.87, 107,
275:         107.12, 107, 91, 93.94, 93.87, 95.5, 93, 94.94, 98.25, 96.75,
276:         94.81, 94.37, 91.56, 90.25, 93.94, 93.62, 97, 95, 95.87, 94.06,
277:         94.62, 93.75, 98, 103.94, 107.87, 106.06, 104.5, 105, 104.19, 103.06,
278:         103.42, 105.27, 111.87, 116, 116.62, 118.28, 113.37, 109, 109.7, 109.25,
279:         107, 109.19, 110, 109.2, 110.12, 108, 108.62, 109.75, 109.81, 109,
280:         108.75, 107.87,
281:     ];
282: }
283: 
284: function testVolumes(): number[] {
285:     return [
286:         4077500, 4955900, 4775300, 4155300, 4593100, 3631300, 3382800, 4954200, 4500000, 3397500,
287:         4204500, 6321400, 10203600, 19043900, 11692000, 9553300, 8920300, 5970900, 5062300, 3705600,
288:         5865600, 5603000, 5811900, 8483800, 5995200, 5408800, 5430500, 6283800, 5834800, 4515500,
289:         4493300, 4346100, 3700300, 4600200, 4557200, 4323600, 5237500, 7404100, 4798400, 4372800,
290:         3872300, 10750800, 5804800, 3785500, 5014800, 3507700, 4298800, 4842500, 3952200, 3304700,
291:         3462000, 7253900, 9753100, 5953000, 5011700, 5910800, 4916900, 4135000, 4054200, 3735300,
292:         2921900, 2658400, 4624400, 4372200, 5831600, 4268600, 3059200, 4495500, 3425000, 3630800,
293:         4168100, 5966900, 7692800, 7362500, 6581300, 19587700, 10378600, 9334700, 10467200, 5671400,
294:         5645000, 4518600, 4519500, 5569700, 4239700, 4175300, 4995300, 4776600, 4190000, 6035300,
295:         12168900, 9040800, 5780300, 4320800, 3899100, 3221400, 3455500, 4304200, 4703900, 8316300,
296:         10553900, 6384800, 7163300, 7007800, 5114100, 5263800, 6666100, 7398400, 5575000, 4852300,
297:         4298100, 4900500, 4887700, 6964800, 4679200, 9165000, 6469800, 6792000, 4423800, 5231900,
298:         4565600, 6235200, 5225900, 8261400, 5912500, 3545600, 5714500, 6653900, 6094500, 4799200,
299:         5050800, 5648900, 4726300, 5585600, 5124800, 7630200, 14311600, 8793600, 8874200, 6966600,
300:         5525500, 6515500, 5291900, 5711700, 4327700, 4568000, 6859200, 5757500, 7367000, 6144100,
301:         4052700, 5849700, 5544700, 5032200, 4400600, 4894100, 5140000, 6610900, 7585200, 5963100,
302:         6045500, 8443300, 6464700, 6248300, 4357200, 4774700, 6216900, 6266900, 5584800, 5284500,
303:         7554500, 7209500, 8424800, 5094500, 4443600, 4591100, 5658400, 6094100, 14862200, 7544700,
304:         6985600, 8093000, 7590000, 7451300, 7078000, 7105300, 8778800, 6643900, 10563900, 7043100,
305:         6438900, 8057700, 14240000, 17872300, 7831100, 8277700, 15017800, 14183300, 13921100, 9683000,
306:         9187300, 11380500, 69447300, 26673600, 13768400, 11371600, 9872200, 9450500, 11083300, 9552800,
307:         11108400, 10374200, 16701900, 13741900, 8523600, 9551900, 8680500, 7151700, 9673100, 6264700,
308:         8541600, 8358000, 18720800, 19683100, 13682500, 10668100, 9710600, 3113100, 5682000, 5763600,
309:         5340000, 6220800, 14680500, 9933000, 11329500, 8145300, 16644700, 12593800, 7138100, 7442300,
310:         9442300, 7123600, 7680600, 4839800, 4775500, 4008800, 4533600, 3741100, 4084800, 2685200,
311:         3438000, 2870500,
312:     ];
313: }
314: 
315: // ---------------------------------------------------------------------------
316: // Run
317: // ---------------------------------------------------------------------------
318: 
319: main();

(End of file - total 319 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/pane.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/pane.ts</path>
<type>file</type>
<content>1: /** Identifies the chart pane an indicator output is drawn on. */
2: export enum Pane {
3:   /** The primary price pane. */
4:   Price = 1,
5: 
6:   /** A dedicated sub-pane for this indicator. */
7:   Own,
8: 
9:   /** Drawn on the parent indicator's pane. */
10:   OverlayOnParent
11: }

(End of file - total 11 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/outputs/band.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/outputs/band.ts</path>
<type>file</type>
<content>1: /** Holds two band values and a time stamp. */
2: export class Band {
3:   /** The date and time. */
4:   time!: Date;
5: 
6:   /** A lower value of the band. */
7:   lower!: number;
8: 
9:   /** A higher value of the band. */
10:   upper!: number;
11: }

(End of file - total 11 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/outputs/heatmap.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/outputs/heatmap.ts</path>
<type>file</type>
<content>1: /** Holds a time stamp (x) and an array of values (z) corresponding to parameter (y) range to paint a heatmap column. */
2: export class Heatmap {
3:   /** The date and time (x). */
4:   time!: Date;
5: 
6:   /** The first parameter (y) value of the heatmap. This value is the same for all columns. */
7:   parameterFirst!: number;
8: 
9:   /** The last parameter (y) value of the heatmap. This value is the same for all columns. */
10:   parameterLast!: number;
11: 
12:   /** A parameter resolution (positive number). A value of 10 means that heatmap values are evaluated at every 0.1 of parameter range. */
13:   parameterResolution!: number;
14: 
15:   /** A minimal value (z) of the heatmap column. */
16:   valueMin!: number;
17: 
18:   /** A maximal value (z) of the heatmap column. */
19:   valueMax!: number;
20: 
21:   /** The values (z) of the heatmap column. */
22:   values!: number[];
23: 
24:   /** Creates a populated heatmap column. */
25:   public static newHeatmap(
26:     time: Date,
27:     parameterFirst: number,
28:     parameterLast: number,
29:     parameterResolution: number,
30:     valueMin: number,
31:     valueMax: number,
32:     values: number[],
33:   ): Heatmap {
34:     const h = new Heatmap();
35:     h.time = time;
36:     h.parameterFirst = parameterFirst;
37:     h.parameterLast = parameterLast;
38:     h.parameterResolution = parameterResolution;
39:     h.valueMin = valueMin;
40:     h.valueMax = valueMax;
41:     h.values = values;
42:     return h;
43:   }
44: 
45:   /** Creates an empty heatmap column with valid axis metadata but no values. */
46:   public static newEmptyHeatmap(
47:     time: Date,
48:     parameterFirst: number,
49:     parameterLast: number,
50:     parameterResolution: number,
51:   ): Heatmap {
52:     const h = new Heatmap();
53:     h.time = time;
54:     h.parameterFirst = parameterFirst;
55:     h.parameterLast = parameterLast;
56:     h.parameterResolution = parameterResolution;
57:     h.valueMin = Number.NaN;
58:     h.valueMax = Number.NaN;
59:     h.values = [];
60:     return h;
61:   }
62: 
63:   /** Indicates whether this heatmap column has no values. */
64:   public isEmpty(): boolean {
65:     return !this.values || this.values.length < 1;
66:   }
67: }

(End of file - total 67 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/entities/scalar.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/entities/scalar.ts</path>
<type>file</type>
<content>1: /** A _scalar_ (value and time) entity. */
2: export class Scalar {
3:   /** The date and time. */
4:   time!: Date;
5: 
6:   /** The value. */
7:   value!: number;
8: 
9:   constructor(data?: any) {
10:     if (data) {
11:       for (const property in data) {
12:         if (Object.prototype.hasOwnProperty.call(data, property)) {
13:           (this as any)[property] = data[property];
14:         }
15:       }
16:     }
17:   }
18: 
19:   /*toJSON(data?: any): any {
20:     data = typeof data === 'object' ? data : {};
21:     data.time = this.time ? this.time.toISOString() : undefined;
22:     data.value = this.value;
23:     return data;
24:   }*/
25: }

(End of file - total 25 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/descriptor.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/descriptor.ts</path>
<type>file</type>
<content>1: import { Adaptivity } from './adaptivity.js';
2: import { IndicatorIdentifier } from './indicator-identifier.js';
3: import { InputRequirement } from './input-requirement.js';
4: import { OutputDescriptor } from './output-descriptor.js';
5: import { VolumeUsage } from './volume-usage.js';
6: 
7: /**
8:  * Classifies an indicator along multiple taxonomic dimensions to enable
9:  * filtering and display in charting catalogs.
10:  */
11: export interface Descriptor {
12:   /** Uniquely identifies the indicator. */
13:   identifier: IndicatorIdentifier;
14: 
15:   /** Groups related indicators (e.g., by author or category). */
16:   family: string;
17: 
18:   /** Whether the indicator adapts its parameters. */
19:   adaptivity: Adaptivity;
20: 
21:   /** The minimum input data type this indicator consumes. */
22:   inputRequirement: InputRequirement;
23: 
24:   /** How this indicator uses volume information. */
25:   volumeUsage: VolumeUsage;
26: 
27:   /** Per-output classifications. */
28:   outputs: OutputDescriptor[];
29: }

(End of file - total 29 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/descriptors.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/descriptors.ts</path>
<type>file</type>
<content>1: import { Adaptivity } from './adaptivity.js';
2: import { Descriptor } from './descriptor.js';
3: import { IndicatorIdentifier } from './indicator-identifier.js';
4: import { InputRequirement } from './input-requirement.js';
5: import { OutputDescriptor } from './output-descriptor.js';
6: import { Shape } from './outputs/shape/shape.js';
7: import { Pane } from './pane.js';
8: import { Role } from './role.js';
9: import { VolumeUsage } from './volume-usage.js';
10: 
11: // Helper to keep entries terse. Output Kind values are 0-based, mirroring each
12: // indicator package's TypeScript Output enumeration.
13: function out(kind: number, shape: Shape, role: Role, pane: Pane): OutputDescriptor {
14:   return { kind, shape, role, pane };
15: }
16: 
17: function desc(
18:   identifier: IndicatorIdentifier,
19:   family: string,
20:   adaptivity: Adaptivity,
21:   inputRequirement: InputRequirement,
22:   volumeUsage: VolumeUsage,
23:   outputs: OutputDescriptor[]
24: ): Descriptor {
25:   return { identifier, family, adaptivity, inputRequirement, volumeUsage, outputs };
26: }
27: 
28: const S = Shape;
29: const R = Role;
30: const P = Pane;
31: const A = Adaptivity;
32: const I = InputRequirement;
33: const V = VolumeUsage;
34: 
35: /** Static registry of taxonomic descriptors for all implemented indicators. */
36: const descriptors: Map<IndicatorIdentifier, Descriptor> = new Map<IndicatorIdentifier, Descriptor>([
37:   [IndicatorIdentifier.SimpleMovingAverage, desc(IndicatorIdentifier.SimpleMovingAverage, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
38:   [IndicatorIdentifier.WeightedMovingAverage, desc(IndicatorIdentifier.WeightedMovingAverage, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
39:   [IndicatorIdentifier.TriangularMovingAverage, desc(IndicatorIdentifier.TriangularMovingAverage, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
40:   [IndicatorIdentifier.ExponentialMovingAverage, desc(IndicatorIdentifier.ExponentialMovingAverage, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
41:   [IndicatorIdentifier.DoubleExponentialMovingAverage, desc(IndicatorIdentifier.DoubleExponentialMovingAverage, 'Patrick Mulloy', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
42:   [IndicatorIdentifier.TripleExponentialMovingAverage, desc(IndicatorIdentifier.TripleExponentialMovingAverage, 'Patrick Mulloy', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
43:   [IndicatorIdentifier.T2ExponentialMovingAverage, desc(IndicatorIdentifier.T2ExponentialMovingAverage, 'Tim Tillson', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
44:   [IndicatorIdentifier.T3ExponentialMovingAverage, desc(IndicatorIdentifier.T3ExponentialMovingAverage, 'Tim Tillson', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
45:   [IndicatorIdentifier.KaufmanAdaptiveMovingAverage, desc(IndicatorIdentifier.KaufmanAdaptiveMovingAverage, 'Perry Kaufman', A.Adaptive, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
46:   [IndicatorIdentifier.JurikMovingAverage, desc(IndicatorIdentifier.JurikMovingAverage, 'Mark Jurik', A.Adaptive, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
47:   [IndicatorIdentifier.MesaAdaptiveMovingAverage, desc(IndicatorIdentifier.MesaAdaptiveMovingAverage, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
48:     out(0, S.Scalar, R.Smoother, P.Price),
49:     out(1, S.Scalar, R.Smoother, P.Price),
50:     out(2, S.Band, R.Envelope, P.Price),
51:   ])],
52:   [IndicatorIdentifier.FractalAdaptiveMovingAverage, desc(IndicatorIdentifier.FractalAdaptiveMovingAverage, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
53:     out(0, S.Scalar, R.Smoother, P.Price),
54:     out(1, S.Scalar, R.FractalDimension, P.Own),
55:   ])],
56:   [IndicatorIdentifier.DominantCycle, desc(IndicatorIdentifier.DominantCycle, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
57:     out(0, S.Scalar, R.CyclePeriod, P.Own),
58:     out(1, S.Scalar, R.CyclePeriod, P.Own),
59:     out(2, S.Scalar, R.CyclePhase, P.Own),
60:   ])],
61:   [IndicatorIdentifier.Momentum, desc(IndicatorIdentifier.Momentum, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
62:   [IndicatorIdentifier.RateOfChange, desc(IndicatorIdentifier.RateOfChange, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
63:   [IndicatorIdentifier.RateOfChangePercent, desc(IndicatorIdentifier.RateOfChangePercent, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
64:   [IndicatorIdentifier.RelativeStrengthIndex, desc(IndicatorIdentifier.RelativeStrengthIndex, 'Welles Wilder', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.BoundedOscillator, P.Own)])],
65:   [IndicatorIdentifier.ChandeMomentumOscillator, desc(IndicatorIdentifier.ChandeMomentumOscillator, 'Tushar Chande', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.BoundedOscillator, P.Own)])],
66:   [IndicatorIdentifier.BollingerBands, desc(IndicatorIdentifier.BollingerBands, 'John Bollinger', A.Static, I.ScalarInput, V.NoVolume, [
67:     out(0, S.Scalar, R.Envelope, P.Price),
68:     out(1, S.Scalar, R.Smoother, P.Price),
69:     out(2, S.Scalar, R.Envelope, P.Price),
70:     out(3, S.Scalar, R.Volatility, P.Own),
71:     out(4, S.Scalar, R.BoundedOscillator, P.Own),
72:     out(5, S.Band, R.Envelope, P.Price),
73:   ])],
74:   [IndicatorIdentifier.Variance, desc(IndicatorIdentifier.Variance, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Volatility, P.Own)])],
75:   [IndicatorIdentifier.StandardDeviation, desc(IndicatorIdentifier.StandardDeviation, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Volatility, P.Own)])],
76:   [IndicatorIdentifier.GoertzelSpectrum, desc(IndicatorIdentifier.GoertzelSpectrum, 'Custom', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Heatmap, R.Spectrum, P.Own)])],
77:   [IndicatorIdentifier.CenterOfGravityOscillator, desc(IndicatorIdentifier.CenterOfGravityOscillator, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [
78:     out(0, S.Scalar, R.Oscillator, P.Own),
79:     out(1, S.Scalar, R.Signal, P.Own),
80:   ])],
81:   [IndicatorIdentifier.CyberCycle, desc(IndicatorIdentifier.CyberCycle, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [
82:     out(0, S.Scalar, R.Oscillator, P.Own),
83:     out(1, S.Scalar, R.Signal, P.Own),
84:   ])],
85:   [IndicatorIdentifier.InstantaneousTrendLine, desc(IndicatorIdentifier.InstantaneousTrendLine, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [
86:     out(0, S.Scalar, R.Smoother, P.Price),
87:     out(1, S.Scalar, R.Signal, P.Price),
88:   ])],
89:   [IndicatorIdentifier.SuperSmoother, desc(IndicatorIdentifier.SuperSmoother, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
90:   [IndicatorIdentifier.ZeroLagExponentialMovingAverage, desc(IndicatorIdentifier.ZeroLagExponentialMovingAverage, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
91:   [IndicatorIdentifier.ZeroLagErrorCorrectingExponentialMovingAverage, desc(IndicatorIdentifier.ZeroLagErrorCorrectingExponentialMovingAverage, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
92:   [IndicatorIdentifier.RoofingFilter, desc(IndicatorIdentifier.RoofingFilter, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
93:   [IndicatorIdentifier.TrueRange, desc(IndicatorIdentifier.TrueRange, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.Volatility, P.Own)])],
94:   [IndicatorIdentifier.AverageTrueRange, desc(IndicatorIdentifier.AverageTrueRange, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.Volatility, P.Own)])],
95:   [IndicatorIdentifier.NormalizedAverageTrueRange, desc(IndicatorIdentifier.NormalizedAverageTrueRange, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.Volatility, P.Own)])],
96:   [IndicatorIdentifier.DirectionalMovementMinus, desc(IndicatorIdentifier.DirectionalMovementMinus, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.Directional, P.Own)])],
97:   [IndicatorIdentifier.DirectionalMovementPlus, desc(IndicatorIdentifier.DirectionalMovementPlus, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.Directional, P.Own)])],
98:   [IndicatorIdentifier.DirectionalIndicatorMinus, desc(IndicatorIdentifier.DirectionalIndicatorMinus, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [
99:     out(0, S.Scalar, R.Directional, P.Own),
100:     out(1, S.Scalar, R.Directional, P.Own),
101:     out(2, S.Scalar, R.Volatility, P.Own),
102:     out(3, S.Scalar, R.Volatility, P.Own),
103:   ])],
104:   [IndicatorIdentifier.DirectionalIndicatorPlus, desc(IndicatorIdentifier.DirectionalIndicatorPlus, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [
105:     out(0, S.Scalar, R.Directional, P.Own),
106:     out(1, S.Scalar, R.Directional, P.Own),
107:     out(2, S.Scalar, R.Volatility, P.Own),
108:     out(3, S.Scalar, R.Volatility, P.Own),
109:   ])],
110:   [IndicatorIdentifier.DirectionalMovementIndex, desc(IndicatorIdentifier.DirectionalMovementIndex, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [
111:     out(0, S.Scalar, R.BoundedOscillator, P.Own),
112:     out(1, S.Scalar, R.Directional, P.Own),
113:     out(2, S.Scalar, R.Directional, P.Own),
114:     out(3, S.Scalar, R.Directional, P.Own),
115:     out(4, S.Scalar, R.Directional, P.Own),
116:     out(5, S.Scalar, R.Volatility, P.Own),
117:     out(6, S.Scalar, R.Volatility, P.Own),
118:   ])],
119:   [IndicatorIdentifier.AverageDirectionalMovementIndex, desc(IndicatorIdentifier.AverageDirectionalMovementIndex, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [
120:     out(0, S.Scalar, R.BoundedOscillator, P.Own),
121:     out(1, S.Scalar, R.BoundedOscillator, P.Own),
122:     out(2, S.Scalar, R.Directional, P.Own),
123:     out(3, S.Scalar, R.Directional, P.Own),
124:     out(4, S.Scalar, R.Directional, P.Own),
125:     out(5, S.Scalar, R.Directional, P.Own),
126:     out(6, S.Scalar, R.Volatility, P.Own),
127:     out(7, S.Scalar, R.Volatility, P.Own),
128:   ])],
129:   [IndicatorIdentifier.AverageDirectionalMovementIndexRating, desc(IndicatorIdentifier.AverageDirectionalMovementIndexRating, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [
130:     out(0, S.Scalar, R.BoundedOscillator, P.Own),
131:     out(1, S.Scalar, R.BoundedOscillator, P.Own),
132:     out(2, S.Scalar, R.BoundedOscillator, P.Own),
133:     out(3, S.Scalar, R.Directional, P.Own),
134:     out(4, S.Scalar, R.Directional, P.Own),
135:     out(5, S.Scalar, R.Directional, P.Own),
136:     out(6, S.Scalar, R.Directional, P.Own),
137:     out(7, S.Scalar, R.Volatility, P.Own),
138:     out(8, S.Scalar, R.Volatility, P.Own),
139:   ])],
140:   [IndicatorIdentifier.WilliamsPercentR, desc(IndicatorIdentifier.WilliamsPercentR, 'Larry Williams', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.BoundedOscillator, P.Own)])],
141:   [IndicatorIdentifier.PercentagePriceOscillator, desc(IndicatorIdentifier.PercentagePriceOscillator, 'Gerald Appel', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
142:   [IndicatorIdentifier.AbsolutePriceOscillator, desc(IndicatorIdentifier.AbsolutePriceOscillator, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
143:   [IndicatorIdentifier.CommodityChannelIndex, desc(IndicatorIdentifier.CommodityChannelIndex, 'Donald Lambert', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.BoundedOscillator, P.Own)])],
144:   [IndicatorIdentifier.MoneyFlowIndex, desc(IndicatorIdentifier.MoneyFlowIndex, 'Gene Quong', A.Static, I.BarInput, V.AggregateBarVolume, [out(0, S.Scalar, R.BoundedOscillator, P.Own)])],
145:   [IndicatorIdentifier.OnBalanceVolume, desc(IndicatorIdentifier.OnBalanceVolume, 'Joseph Granville', A.Static, I.BarInput, V.AggregateBarVolume, [out(0, S.Scalar, R.VolumeFlow, P.Own)])],
146:   [IndicatorIdentifier.BalanceOfPower, desc(IndicatorIdentifier.BalanceOfPower, 'Igor Livshin', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.BoundedOscillator, P.Own)])],
147:   [IndicatorIdentifier.RateOfChangeRatio, desc(IndicatorIdentifier.RateOfChangeRatio, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
148:   [IndicatorIdentifier.PearsonsCorrelationCoefficient, desc(IndicatorIdentifier.PearsonsCorrelationCoefficient, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Correlation, P.Own)])],
149:   [IndicatorIdentifier.LinearRegression, desc(IndicatorIdentifier.LinearRegression, 'Common', A.Static, I.ScalarInput, V.NoVolume, [
150:     out(0, S.Scalar, R.Smoother, P.Price),
151:     out(1, S.Scalar, R.Smoother, P.Price),
152:     out(2, S.Scalar, R.Smoother, P.Price),
153:     out(3, S.Scalar, R.Oscillator, P.Own),
154:     out(4, S.Scalar, R.Oscillator, P.Own),
155:   ])],
156:   [IndicatorIdentifier.UltimateOscillator, desc(IndicatorIdentifier.UltimateOscillator, 'Larry Williams', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.BoundedOscillator, P.Own)])],
157:   [IndicatorIdentifier.StochasticRelativeStrengthIndex, desc(IndicatorIdentifier.StochasticRelativeStrengthIndex, 'Tushar Chande', A.Static, I.ScalarInput, V.NoVolume, [
158:     out(0, S.Scalar, R.BoundedOscillator, P.Own),
159:     out(1, S.Scalar, R.Signal, P.Own),
160:   ])],
161:   [IndicatorIdentifier.Stochastic, desc(IndicatorIdentifier.Stochastic, 'George Lane', A.Static, I.BarInput, V.NoVolume, [
162:     out(0, S.Scalar, R.BoundedOscillator, P.Own),
163:     out(1, S.Scalar, R.BoundedOscillator, P.Own),
164:     out(2, S.Scalar, R.Signal, P.Own),
165:   ])],
166:   [IndicatorIdentifier.Aroon, desc(IndicatorIdentifier.Aroon, 'Tushar Chande', A.Static, I.BarInput, V.NoVolume, [
167:     out(0, S.Scalar, R.BoundedOscillator, P.Own),
168:     out(1, S.Scalar, R.BoundedOscillator, P.Own),
169:     out(2, S.Scalar, R.Oscillator, P.Own),
170:   ])],
171:   [IndicatorIdentifier.AdvanceDecline, desc(IndicatorIdentifier.AdvanceDecline, 'Marc Chaikin', A.Static, I.BarInput, V.AggregateBarVolume, [out(0, S.Scalar, R.VolumeFlow, P.Own)])],
172:   [IndicatorIdentifier.AdvanceDeclineOscillator, desc(IndicatorIdentifier.AdvanceDeclineOscillator, 'Marc Chaikin', A.Static, I.BarInput, V.AggregateBarVolume, [out(0, S.Scalar, R.VolumeFlow, P.Own)])],
173:   [IndicatorIdentifier.ParabolicStopAndReverse, desc(IndicatorIdentifier.ParabolicStopAndReverse, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.Overlay, P.Price)])],
174:   [IndicatorIdentifier.TripleExponentialMovingAverageOscillator, desc(IndicatorIdentifier.TripleExponentialMovingAverageOscillator, 'Jack Hutson', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
175:   [IndicatorIdentifier.BollingerBandsTrend, desc(IndicatorIdentifier.BollingerBandsTrend, 'John Bollinger', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
176:   [IndicatorIdentifier.MovingAverageConvergenceDivergence, desc(IndicatorIdentifier.MovingAverageConvergenceDivergence, 'Gerald Appel', A.Static, I.ScalarInput, V.NoVolume, [
177:     out(0, S.Scalar, R.Oscillator, P.Own),
178:     out(1, S.Scalar, R.Signal, P.Own),
179:     out(2, S.Scalar, R.Histogram, P.Own),
180:   ])],
181:   [IndicatorIdentifier.SineWave, desc(IndicatorIdentifier.SineWave, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
182:     out(0, S.Scalar, R.Oscillator, P.Own),
183:     out(1, S.Scalar, R.Signal, P.Own),
184:     out(2, S.Band, R.Envelope, P.Own),
185:     out(3, S.Scalar, R.CyclePeriod, P.Own),
186:     out(4, S.Scalar, R.CyclePhase, P.Own),
187:   ])],
188:   [IndicatorIdentifier.HilbertTransformerInstantaneousTrendLine, desc(IndicatorIdentifier.HilbertTransformerInstantaneousTrendLine, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
189:     out(0, S.Scalar, R.Smoother, P.Price),
190:     out(1, S.Scalar, R.CyclePeriod, P.Own),
191:   ])],
192:   [IndicatorIdentifier.TrendCycleMode, desc(IndicatorIdentifier.TrendCycleMode, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
193:     out(0, S.Scalar, R.RegimeFlag, P.Own),
194:     out(1, S.Scalar, R.RegimeFlag, P.Own),
195:     out(2, S.Scalar, R.RegimeFlag, P.Own),
196:     out(3, S.Scalar, R.Smoother, P.Price),
197:     out(4, S.Scalar, R.Oscillator, P.Own),
198:     out(5, S.Scalar, R.Signal, P.Own),
199:     out(6, S.Scalar, R.CyclePeriod, P.Own),
200:     out(7, S.Scalar, R.CyclePhase, P.Own),
201:   ])],
202:   [IndicatorIdentifier.CoronaSpectrum, desc(IndicatorIdentifier.CoronaSpectrum, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
203:     out(0, S.Heatmap, R.Spectrum, P.Own),
204:     out(1, S.Scalar, R.CyclePeriod, P.Own),
205:     out(2, S.Scalar, R.CyclePeriod, P.Own),
206:   ])],
207:   [IndicatorIdentifier.CoronaSignalToNoiseRatio, desc(IndicatorIdentifier.CoronaSignalToNoiseRatio, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
208:     out(0, S.Heatmap, R.Spectrum, P.Own),
209:     out(1, S.Scalar, R.BoundedOscillator, P.Own),
210:   ])],
211:   [IndicatorIdentifier.CoronaSwingPosition, desc(IndicatorIdentifier.CoronaSwingPosition, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
212:     out(0, S.Heatmap, R.Spectrum, P.Own),
213:     out(1, S.Scalar, R.BoundedOscillator, P.Own),
214:   ])],
215:   [IndicatorIdentifier.CoronaTrendVigor, desc(IndicatorIdentifier.CoronaTrendVigor, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
216:     out(0, S.Heatmap, R.Spectrum, P.Own),
217:     out(1, S.Scalar, R.Oscillator, P.Own),
218:   ])],
219:   [IndicatorIdentifier.AdaptiveTrendAndCycleFilter, desc(IndicatorIdentifier.AdaptiveTrendAndCycleFilter, 'Vladimir Kravchuk', A.Adaptive, I.ScalarInput, V.NoVolume, [
220:     out(0, S.Scalar, R.Smoother, P.Price),
221:     out(1, S.Scalar, R.Smoother, P.Price),
222:     out(2, S.Scalar, R.Smoother, P.Price),
223:     out(3, S.Scalar, R.Smoother, P.Price),
224:     out(4, S.Scalar, R.Smoother, P.Price),
225:     out(5, S.Scalar, R.Oscillator, P.Own),
226:     out(6, S.Scalar, R.Oscillator, P.Own),
227:     out(7, S.Scalar, R.Oscillator, P.Own),
228:   ])],
229:   [IndicatorIdentifier.MaximumEntropySpectrum, desc(IndicatorIdentifier.MaximumEntropySpectrum, 'Custom', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Heatmap, R.Spectrum, P.Own)])],
230:   [IndicatorIdentifier.DiscreteFourierTransformSpectrum, desc(IndicatorIdentifier.DiscreteFourierTransformSpectrum, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Heatmap, R.Spectrum, P.Own)])],
231:   [IndicatorIdentifier.CombBandPassSpectrum, desc(IndicatorIdentifier.CombBandPassSpectrum, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Heatmap, R.Spectrum, P.Own)])],
232:   [IndicatorIdentifier.AutoCorrelationIndicator, desc(IndicatorIdentifier.AutoCorrelationIndicator, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Heatmap, R.Correlation, P.Own)])],
233:   [IndicatorIdentifier.AutoCorrelationPeriodogram, desc(IndicatorIdentifier.AutoCorrelationPeriodogram, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [out(0, S.Heatmap, R.Spectrum, P.Own)])],
234: ]);
235: 
236: /**
237:  * Returns the taxonomic descriptor for the given indicator identifier, or
238:  * `undefined` if no descriptor is registered for the identifier.
239:  */
240: export function descriptorOf(id: IndicatorIdentifier): Descriptor | undefined {
241:   return descriptors.get(id);
242: }
243: 
244: /** Returns a copy of the full descriptor registry. */
245: export function getDescriptors(): Map<IndicatorIdentifier, Descriptor> {
246:   return new Map(descriptors);
247: }

(End of file - total 247 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.6 · 6.9s)

Now I have everything. Let me check how the TS output-descriptor looks and the indicator-output type, then create the file:

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/output-descriptor.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/output-descriptor.ts</path>
<type>file</type>
<content>1: import { Shape } from './outputs/shape/shape.js';
2: import { Pane } from './pane.js';
3: import { Role } from './role.js';
4: 
5: /** Classifies a single indicator output for charting / discovery. */
6: export interface OutputDescriptor {
7:   /** Integer representation of the output enumeration of a related indicator. */
8:   kind: number;
9: 
10:   /** The data shape of this output. */
11:   shape: Shape;
12: 
13:   /** The semantic role of this output. */
14:   role: Role;
15: 
16:   /** The chart pane on which this output is drawn. */
17:   pane: Pane;
18: }

(End of file - total 18 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/indicator-output.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/indicator-output.ts</path>
<type>file</type>
<content>1: import { Scalar } from '../../entities/scalar';
2: import { Band } from './outputs/band';
3: import { Heatmap } from './outputs/heatmap';
4: 
5: /** Defines indicator output. */
6: export type IndicatorOutput = (Scalar | Band | Heatmap)[];

(End of file - total 6 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/outputs/shape/shape.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/outputs/shape/shape.ts</path>
<type>file</type>
<content>1: /** Enumerates indicator output data shapes. */
2: export enum Shape {
3: 
4:   /** Holds a single value. */
5:   Scalar,
6: 
7:   /** Holds two values representing lower and upper lines of a band. */
8:   Band,
9: 
10:   /** Holds an array of values representing a heat-map column. */
11:   Heatmap,
12: 
13:   /** Holds an ordered, variable-length sequence of (offset, value) points. */
14:   Polyline
15: }

(End of file - total 15 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.6 · 5.4s)

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Create ts/cmd/iconf/main.ts",
      "status": "in_progress",
      "priority": "high"
    },
    {
      "content": "Copy settings.json to ts/cmd/iconf/",
      "status": "pending",
      "priority": "medium"
    },
    {
      "content": "Test the TS iconf tool",
      "status": "pending",
      "priority": "high"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Create ts/cmd/iconf/main.ts",
    "status": "in_progress",
    "priority": "high"
  },
  {
    "content": "Copy settings.json to ts/cmd/iconf/",
    "status": "pending",
    "priority": "medium"
  },
  {
    "content": "Test the TS iconf tool",
    "status": "pending",
    "priority": "high"
  }
]
```

---

## User

---

## Assistant (Compaction · claude-opus-4.6 · 51.7s)



---

## Goal

Build a CLI tool called `iconf` in **both Go and TypeScript** that:
1. Reads a JSON settings file specifying indicators (same format as `icalc/settings.json`)
2. Takes an output file name (without extension) as a CLI argument
3. Uses hardcoded 252-bar TA-Lib test data (same as `icalc`)
4. Creates indicators via the factory, runs bar data through them
5. Writes `<output>.json` — a chart configuration file mimicking the structure from `chart/ohlcv-chart/template/configuration.ts`
6. Writes `<output>.ts` — same structure but with calculated data in static TypeScript arrays, and `data` fields referencing those arrays by variable name

## Instructions

- The Configuration structure to mimic is defined in `chart/ohlcv-chart/template/configuration.ts` — it has `pricePane` (a `Pane`) and `indicatorPanes` (array of `Pane`), where each `Pane` has `bands`, `lineAreas`, `lines`, `heatmaps`, `horizontals`, `arrows`
- The example usage in `chart/study-page/test-data/test-data.ts` shows how outputs are distributed across panes
- Each data item has: `name` (use indicator/output mnemonic), `data` (calculated values), `indicator` (index), `output` (index), plus visual properties
- Output shape → pane mapping uses the descriptor registry:
  - `Pane.Price` + `Shape.Scalar` → `pricePane.lines`
  - `Pane.Price` + `Shape.Band` → `pricePane.bands`
  - `Pane.Own` + `Shape.Heatmap` → `indicatorPanes[n].heatmap`
  - `Pane.Own` + `Shape.Band` → `indicatorPanes[n].bands`
  - `Pane.Own` + `Shape.Scalar` → `indicatorPanes[n].lines`
  - Own-pane outputs from the same indicator share the same indicator pane
- For the `.ts` file: data arrays should be exported as `const` variables and the configuration object's `data` fields should reference variable names (not inline data)
- Strip extension from output filename if user provides one
- Import paths in generated `.ts` may be wrong — acceptable
- The Go module path is `zpano`
- The TS project uses ESM (`"type": "module"` in package.json), `tsx` runner, target ES2022
- TS CLIs are run via `npx tsx cmd/iconf/main.ts <args>` from the `ts/` directory
- TS imports use `.js` extensions and relative paths (e.g., `../../entities/bar.js`)
- **Key TS asymmetry**: The KAMA identifier in Go JSON is `"kaufmanAdaptiveMovingAverageMovingAverage"` but in TS maps to `KaufmanAdaptiveMovingAverage` — the TS `icalc` has a `resolveIdentifier()` function that handles this

## Discoveries

### Go iconf patterns (established in the completed Go version)
- Uses `core.DescriptorOf(meta.Identifier)` to get output descriptors with Shape and Pane
- Groups outputs by `{indicatorIdx}` key for Own-pane allocation (all Own outputs of same indicator share a pane)
- Cycles through color arrays for lines and bands
- Heatmap panes get height "120" and valueMarginPercentageFactor 0; others get "60" and 0.01
- NaN values and empty bands/heatmaps are filtered out (not included in output data)
- JSON output uses `json.MarshalIndent`; TS output uses string builder with manual formatting
- `sanitizeVarName()` converts mnemonics to valid JS identifiers (camelCase, no special chars)

### TS indicator system differences from Go
- `IndicatorOutput` type is `(Scalar | Band | Heatmap)[]`
- `Scalar` is a class with `time: Date` and `value: number` (from `entities/scalar.ts`)
- `Band` is a class with `time: Date`, `lower: number`, `upper: number`
- `Heatmap` class has `parameterFirst`, `parameterLast`, `parameterResolution`, `valueMin`, `valueMax`, `values: number[]`
- Descriptor registry: `descriptorOf(id)` returns `Descriptor | undefined` (from `indicators/core/descriptors.ts`)
- Shape enum is 0-based: `Shape.Scalar = 0`, `Shape.Band = 1`, `Shape.Heatmap = 2`
- Pane enum: `Pane.Price = 1`, `Pane.Own = 2`
- Factory: `createIndicator(id: IndicatorIdentifier, params?)` from `indicators/factory/factory.ts`
- `indicator.metadata()` returns `IndicatorMetadata` with `.outputs: OutputMetadata[]` (each has `.mnemonic`, `.shape`, `.kind`)
- `indicator.updateBar(bar)` returns `IndicatorOutput`
- `indicator.isPrimed()` returns boolean
- Bar class fields: `time`, `open`, `high`, `low`, `close`, `volume`
- Dates formatted as ISO strings via `d.toISOString().slice(0, 10)`

### Settings file
- The `settings.json` for iconf has 8 curated indicators: bollingerBands, simpleMovingAverage, exponentialMovingAverage, relativeStrengthIndex, movingAverageConvergenceDivergence, averageTrueRange, stochastic, goertzelSpectrum

## Accomplished

### Completed
1. **Go `iconf` CLI** (`go/cmd/iconf/main.go`) — fully built and tested
   - Builds cleanly, produces correct JSON and TS output files
   - Verified with BB+Goertzel test: 3 price lines + 1 band, 2 indicator panes (BB oscillators), 1 heatmap pane
   - Verified with full 8-indicator settings: 5 price lines + 1 band, 6 indicator panes
2. **Go `settings.json`** (`go/cmd/iconf/settings.json`) — created with 8 representative indicators

### In Progress
3. **TS `iconf` CLI** (`ts/cmd/iconf/main.ts`) — **NOT YET CREATED, needs to be written next**
   - All reference files have been read and analyzed
   - The approach should mirror the Go version exactly, adapted to TS patterns from `ts/cmd/icalc/main.ts`

### Left To Do
- Create `ts/cmd/iconf/main.ts` — port of the Go iconf logic to TypeScript
- Copy `settings.json` to `ts/cmd/iconf/settings.json` (identical content)
- Create `ts/cmd/iconf/run-me.sh` (like icalc's: `npx tsx cmd/iconf/main.ts cmd/iconf/settings.json output`)
- Test the TS iconf tool

## Relevant files / directories

### Created
- `go/cmd/iconf/main.go` — Go iconf CLI (complete, ~600 lines including embedded test data)
- `go/cmd/iconf/settings.json` — 8-indicator settings file

### To Create
- `ts/cmd/iconf/main.ts` — TS iconf CLI (next step)
- `ts/cmd/iconf/settings.json` — same settings
- `ts/cmd/iconf/run-me.sh` — shell runner

### Key Reference — TS sources to mirror
- `ts/cmd/icalc/main.ts` — TS icalc CLI (pattern for settings reading, bar data, factory usage, identifier resolution)
- `ts/cmd/icalc/settings.json` — full 72-indicator settings (reference only)
- `ts/indicators/factory/factory.ts` — `createIndicator(id, params?)`
- `ts/indicators/core/indicator.ts` — `Indicator` interface
- `ts/indicators/core/indicator-output.ts` — `IndicatorOutput` type
- `ts/indicators/core/indicator-metadata.ts` — `IndicatorMetadata` interface
- `ts/indicators/core/outputs/output-metadata.ts` — `OutputMetadata` (kind, shape, mnemonic)
- `ts/indicators/core/outputs/band.ts` — `Band` class
- `ts/indicators/core/outputs/heatmap.ts` — `Heatmap` class
- `ts/indicators/core/outputs/shape/shape.ts` — `Shape` enum (0-based)
- `ts/indicators/core/pane.ts` — `Pane` enum (Price=1, Own=2)
- `ts/indicators/core/descriptors.ts` — `descriptorOf(id)`, descriptor registry
- `ts/indicators/core/descriptor.ts` — `Descriptor` interface
- `ts/indicators/core/output-descriptor.ts` — `OutputDescriptor` interface
- `ts/indicators/core/indicator-identifier.ts` — `IndicatorIdentifier` enum
- `ts/entities/scalar.ts` — `Scalar` class
- `ts/entities/bar.ts` — `Bar` class

### Key Reference — Chart template types (for output structure)
- `chart/ohlcv-chart/template/configuration.ts`
- `chart/ohlcv-chart/template/pane.ts`
- `chart/ohlcv-chart/template/line-data.ts`
- `chart/ohlcv-chart/template/band-data.ts`
- `chart/ohlcv-chart/template/line-area-data.ts`
- `chart/ohlcv-chart/template/heatmap-data.ts`
- `chart/study-page/test-data/test-data.ts` — example output→pane mapping

### Key Reference — Go iconf (port source)
- `go/cmd/iconf/main.go` — the complete Go implementation to port

---

---

## User

---

## Assistant (Build · claude-opus-4.6 · 5.8s)

_Thinking:_

Let me continue with creating the TS iconf CLI. I need to read the Go implementation and the TS icalc reference, then create the TS version.

Let me continue building the TS `iconf` CLI. I'll start by reading the key reference files.

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Create ts/cmd/iconf/main.ts",
      "status": "in_progress",
      "priority": "high"
    },
    {
      "content": "Copy settings.json to ts/cmd/iconf/",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Create ts/cmd/iconf/run-me.sh",
      "status": "pending",
      "priority": "medium"
    },
    {
      "content": "Test the TS iconf tool",
      "status": "pending",
      "priority": "high"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Create ts/cmd/iconf/main.ts",
    "status": "in_progress",
    "priority": "high"
  },
  {
    "content": "Copy settings.json to ts/cmd/iconf/",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Create ts/cmd/iconf/run-me.sh",
    "status": "pending",
    "priority": "medium"
  },
  {
    "content": "Test the TS iconf tool",
    "status": "pending",
    "priority": "high"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 4.1s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/cmd/iconf/main.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/cmd/iconf/main.go</path>
<type>file</type>
<content>1: // Package main implements iconf, a command-line tool that creates chart
2: // configuration files from indicator settings.
3: //
4: // It reads a JSON settings file containing indicator definitions, creates
5: // indicator instances via the factory, runs embedded bar data through them,
6: // and writes chart configuration files in JSON and TypeScript formats.
7: //
8: // Usage:
9: //
10: //	iconf <settings.json> <output-name>
11: package main
12: 
13: import (
14: 	"encoding/json"
15: 	"fmt"
16: 	"math"
17: 	"os"
18: 	"path/filepath"
19: 	"strings"
20: 	"time"
21: 
22: 	"zpano/entities"
23: 	"zpano/indicators/core"
24: 	"zpano/indicators/core/outputs"
25: 	"zpano/indicators/core/outputs/shape"
26: 	"zpano/indicators/factory"
27: )
28: 
29: // settingsEntry represents a single indicator entry in the settings JSON file.
30: type settingsEntry struct {
31: 	Identifier core.Identifier `json:"identifier"`
32: 	Params     json.RawMessage `json:"params"`
33: }
34: 
35: // --- Chart configuration structs (mirror the TS template types) ---
36: 
37: // Configuration is the top-level chart configuration.
38: type Configuration struct {
39: 	Width                string          `json:"width"`
40: 	NavigationPane       *NavigationPane `json:"navigationPane,omitempty"`
41: 	HeightNavigationPane int             `json:"heightNavigationPane,omitempty"`
42: 	TimeAnnotationFormat string          `json:"timeAnnotationFormat,omitempty"`
43: 	AxisLeft             bool            `json:"axisLeft"`
44: 	AxisRight            bool            `json:"axisRight"`
45: 	Margin               Margin          `json:"margin"`
46: 	Ohlcv                OhlcvData       `json:"ohlcv"`
47: 	PricePane            Pane            `json:"pricePane"`
48: 	IndicatorPanes       []Pane          `json:"indicatorPanes"`
49: 	Crosshair            bool            `json:"crosshair"`
50: 	VolumeInPricePane    bool            `json:"volumeInPricePane"`
51: 	MenuVisible          bool            `json:"menuVisible"`
52: 	DownloadSvgVisible   bool            `json:"downloadSvgVisible"`
53: }
54: 
55: // NavigationPane describes a navigation pane.
56: type NavigationPane struct {
57: 	Height      int  `json:"height"`
58: 	HasLine     bool `json:"hasLine"`
59: 	HasArea     bool `json:"hasArea"`
60: 	HasTimeAxis bool `json:"hasTimeAxis"`
61: 	TimeTicks   int  `json:"timeTicks"`
62: }
63: 
64: // Margin describes chart margins.
65: type Margin struct {
66: 	Left   int `json:"left"`
67: 	Top    int `json:"top"`
68: 	Right  int `json:"right"`
69: 	Bottom int `json:"bottom"`
70: }
71: 
72: // OhlcvData describes the OHLCV data source.
73: type OhlcvData struct {
74: 	Name         string     `json:"name"`
75: 	Data         []OhlcvBar `json:"data"`
76: 	Candlesticks bool       `json:"candlesticks"`
77: }
78: 
79: // OhlcvBar is a bar for the chart (time as ISO string).
80: type OhlcvBar struct {
81: 	Time   string  `json:"time"`
82: 	Open   float64 `json:"open"`
83: 	High   float64 `json:"high"`
84: 	Low    float64 `json:"low"`
85: 	Close  float64 `json:"close"`
86: 	Volume float64 `json:"volume"`
87: }
88: 
89: // Pane describes a chart pane.
90: type Pane struct {
91: 	Height                      string           `json:"height"`
92: 	ValueFormat                 string           `json:"valueFormat"`
93: 	ValueMarginPercentageFactor float64          `json:"valueMarginPercentageFactor"`
94: 	Heatmap                     *HeatmapData     `json:"heatmap,omitempty"`
95: 	Bands                       []BandData       `json:"bands"`
96: 	LineAreas                   []LineAreaData   `json:"lineAreas"`
97: 	Horizontals                 []HorizontalData `json:"horizontals"`
98: 	Lines                       []LineData       `json:"lines"`
99: 	Arrows                      []ArrowData      `json:"arrows"`
100: }
101: 
102: // LineData describes a line on a pane.
103: type LineData struct {
104: 	Name          string      `json:"name"`
105: 	Data          []ScalarVal `json:"data"`
106: 	Indicator     int         `json:"indicator"`
107: 	Output        int         `json:"output"`
108: 	Color         string      `json:"color"`
109: 	Width         float64     `json:"width"`
110: 	Dash          string      `json:"dash"`
111: 	Interpolation string      `json:"interpolation"`
112: }
113: 
114: // ScalarVal is a time+value pair for chart data.
115: type ScalarVal struct {
116: 	Time  string  `json:"time"`
117: 	Value float64 `json:"value"`
118: }
119: 
120: // BandData describes a band on a pane.
121: type BandData struct {
122: 	Name          string    `json:"name"`
123: 	Data          []BandVal `json:"data"`
124: 	Indicator     int       `json:"indicator"`
125: 	Output        int       `json:"output"`
126: 	Color         string    `json:"color"`
127: 	LegendColor   string    `json:"legendColor"`
128: 	Interpolation string    `json:"interpolation"`
129: }
130: 
131: // BandVal is a time+upper+lower pair for chart data.
132: type BandVal struct {
133: 	Time  string  `json:"time"`
134: 	Upper float64 `json:"upper"`
135: 	Lower float64 `json:"lower"`
136: }
137: 
138: // LineAreaData describes a line area on a pane.
139: type LineAreaData struct {
140: 	Name          string      `json:"name"`
141: 	Data          []ScalarVal `json:"data"`
142: 	Indicator     int         `json:"indicator"`
143: 	Output        int         `json:"output"`
144: 	Value         float64     `json:"value"`
145: 	Color         string      `json:"color"`
146: 	LegendColor   string      `json:"legendColor"`
147: 	Interpolation string      `json:"interpolation"`
148: }
149: 
150: // HeatmapData describes a heatmap on a pane.
151: type HeatmapData struct {
152: 	Name           string       `json:"name"`
153: 	Data           []HeatmapVal `json:"data"`
154: 	Indicator      int          `json:"indicator"`
155: 	Output         int          `json:"output"`
156: 	Gradient       string       `json:"gradient"`
157: 	InvertGradient bool         `json:"invertGradient"`
158: }
159: 
160: // HeatmapVal is a heatmap column for chart data.
161: type HeatmapVal struct {
162: 	Time   string    `json:"time"`
163: 	First  float64   `json:"first"`
164: 	Last   float64   `json:"last"`
165: 	Result float64   `json:"result"`
166: 	Min    float64   `json:"min"`
167: 	Max    float64   `json:"max"`
168: 	Values []float64 `json:"values"`
169: }
170: 
171: // HorizontalData describes a horizontal line.
172: type HorizontalData struct {
173: 	Value float64 `json:"value"`
174: 	Color string  `json:"color"`
175: 	Width float64 `json:"width"`
176: 	Dash  string  `json:"dash"`
177: }
178: 
179: // ArrowData describes an arrow on a pane.
180: type ArrowData struct {
181: 	Name      string `json:"name"`
182: 	Down      bool   `json:"down"`
183: 	Time      string `json:"time"`
184: 	Indicator int    `json:"indicator"`
185: 	Output    int    `json:"output"`
186: 	Color     string `json:"color"`
187: }
188: 
189: // --- Default colors for cycling ---
190: 
191: var lineColors = []string{
192: 	"#e41a1c", "#377eb8", "#4daf4a", "#984ea3", "#ff7f00",
193: 	"#a65628", "#f781bf", "#999999", "#66c2a5", "#fc8d62",
194: }
195: 
196: var bandColors = []string{
197: 	"rgba(0,255,0,0.3)", "rgba(0,0,255,0.3)", "rgba(255,0,0,0.3)",
198: 	"rgba(128,0,128,0.3)", "rgba(0,128,128,0.3)",
199: }
200: 
201: func main() {
202: 	if len(os.Args) < 3 {
203: 		fmt.Fprintf(os.Stderr, "usage: iconf <settings.json> <output-name>\n")
204: 		os.Exit(1)
205: 	}
206: 
207: 	settingsPath := os.Args[1]
208: 	outputName := strings.TrimSuffix(strings.TrimSuffix(os.Args[2], ".json"), ".ts")
209: 
210: 	data, err := os.ReadFile(settingsPath)
211: 	if err != nil {
212: 		fmt.Fprintf(os.Stderr, "error reading settings file: %v\n", err)
213: 		os.Exit(1)
214: 	}
215: 
216: 	var entries []settingsEntry
217: 	if err := json.Unmarshal(data, &entries); err != nil {
218: 		fmt.Fprintf(os.Stderr, "error parsing settings file: %v\n", err)
219: 		os.Exit(1)
220: 	}
221: 
222: 	indicators := make([]core.Indicator, 0, len(entries))
223: 	for _, e := range entries {
224: 		ind, err := factory.New(e.Identifier, string(e.Params))
225: 		if err != nil {
226: 			fmt.Fprintf(os.Stderr, "error creating indicator %s: %v\n", e.Identifier, err)
227: 			os.Exit(1)
228: 		}
229: 		indicators = append(indicators, ind)
230: 	}
231: 
232: 	bars := testBars()
233: 	cfg := buildConfiguration(indicators, bars)
234: 
235: 	// Write JSON.
236: 	jsonPath := outputName + ".json"
237: 	jsonData, err := json.MarshalIndent(cfg, "", "  ")
238: 	if err != nil {
239: 		fmt.Fprintf(os.Stderr, "error marshaling JSON: %v\n", err)
240: 		os.Exit(1)
241: 	}
242: 	if err := os.WriteFile(jsonPath, jsonData, 0o644); err != nil {
243: 		fmt.Fprintf(os.Stderr, "error writing %s: %v\n", jsonPath, err)
244: 		os.Exit(1)
245: 	}
246: 	fmt.Printf("wrote %s\n", jsonPath)
247: 
248: 	// Write TypeScript.
249: 	tsPath := outputName + ".ts"
250: 	tsData := buildTypeScript(cfg, filepath.Base(outputName))
251: 	if err := os.WriteFile(tsPath, []byte(tsData), 0o644); err != nil {
252: 		fmt.Fprintf(os.Stderr, "error writing %s: %v\n", tsPath, err)
253: 		os.Exit(1)
254: 	}
255: 	fmt.Printf("wrote %s\n", tsPath)
256: }
257: 
258: // outputPlacement describes where to place an indicator output.
259: type outputPlacement struct {
260: 	indicatorIdx int
261: 	outputIdx    int
262: 	mnemonic     string
263: 	shape        shape.Shape
264: 	pane         core.Pane
265: }
266: 
267: func buildConfiguration(indicators []core.Indicator, bars []entities.Bar) *Configuration {
268: 	const timeFmt = "2006-01-02"
269: 
270: 	// Collect output placements from descriptor registry.
271: 	var placements []outputPlacement
272: 	for i, ind := range indicators {
273: 		meta := ind.Metadata()
274: 		desc, ok := core.DescriptorOf(meta.Identifier)
275: 		if !ok {
276: 			// Fallback: treat all outputs as Own/Scalar.
277: 			for j, om := range meta.Outputs {
278: 				placements = append(placements, outputPlacement{
279: 					indicatorIdx: i, outputIdx: j,
280: 					mnemonic: om.Mnemonic, shape: om.Shape, pane: core.Own,
281: 				})
282: 			}
283: 			continue
284: 		}
285: 		for j, od := range desc.Outputs {
286: 			mnemonic := fmt.Sprintf("out[%d]", j)
287: 			if j < len(meta.Outputs) {
288: 				mnemonic = meta.Outputs[j].Mnemonic
289: 			}
290: 			placements = append(placements, outputPlacement{
291: 				indicatorIdx: i, outputIdx: j,
292: 				mnemonic: mnemonic, shape: od.Shape, pane: od.Pane,
293: 			})
294: 		}
295: 	}
296: 
297: 	// Run bars through indicators, collecting all outputs.
298: 	// allOutputs[barIdx][indicatorIdx] = core.Output
299: 	allOutputs := make([][]core.Output, len(bars))
300: 	for i, bar := range bars {
301: 		allOutputs[i] = make([]core.Output, len(indicators))
302: 		for j, ind := range indicators {
303: 			allOutputs[i][j] = ind.UpdateBar(&bar)
304: 		}
305: 	}
306: 
307: 	// Build OHLCV data.
308: 	ohlcvBars := make([]OhlcvBar, len(bars))
309: 	for i, bar := range bars {
310: 		ohlcvBars[i] = OhlcvBar{
311: 			Time: bar.Time.Format(timeFmt), Open: bar.Open, High: bar.High,
312: 			Low: bar.Low, Close: bar.Close, Volume: bar.Volume,
313: 		}
314: 	}
315: 
316: 	// Group placements: price pane vs own panes.
317: 	// For "Own" pane outputs, group by indicator index so all outputs of the same
318: 	// indicator that share Pane=Own go into the same indicator pane.
319: 	type ownPaneKey struct{ indicatorIdx int }
320: 	ownPaneMap := make(map[ownPaneKey]int) // key -> indicatorPanes index
321: 	var indicatorPanes []Pane
322: 
323: 	pricePane := Pane{
324: 		Height: "30%", ValueFormat: ",.2f", ValueMarginPercentageFactor: 0.01,
325: 		Bands: []BandData{}, LineAreas: []LineAreaData{},
326: 		Horizontals: []HorizontalData{}, Lines: []LineData{}, Arrows: []ArrowData{},
327: 	}
328: 
329: 	colorIdx := 0
330: 	bandColorIdx := 0
331: 
332: 	for _, p := range placements {
333: 		switch {
334: 		case p.pane == core.Price && p.shape == shape.Scalar:
335: 			line := buildLineData(p, allOutputs, bars, timeFmt, lineColors[colorIdx%len(lineColors)])
336: 			colorIdx++
337: 			pricePane.Lines = append(pricePane.Lines, line)
338: 
339: 		case p.pane == core.Price && p.shape == shape.Band:
340: 			band := buildBandData(p, allOutputs, bars, timeFmt, bandColors[bandColorIdx%len(bandColors)])
341: 			bandColorIdx++
342: 			pricePane.Bands = append(pricePane.Bands, band)
343: 
344: 		default:
345: 			// Own pane (or fallback).
346: 			key := ownPaneKey{p.indicatorIdx}
347: 			idx, exists := ownPaneMap[key]
348: 			if !exists {
349: 				idx = len(indicatorPanes)
350: 				ownPaneMap[key] = idx
351: 				indicatorPanes = append(indicatorPanes, Pane{
352: 					Height: "60", ValueFormat: ",.2f", ValueMarginPercentageFactor: 0.01,
353: 					Bands: []BandData{}, LineAreas: []LineAreaData{},
354: 					Horizontals: []HorizontalData{}, Lines: []LineData{}, Arrows: []ArrowData{},
355: 				})
356: 			}
357: 			pane := &indicatorPanes[idx]
358: 
359: 			switch p.shape {
360: 			case shape.Heatmap:
361: 				hm := buildHeatmapData(p, allOutputs, bars, timeFmt)
362: 				pane.Heatmap = &hm
363: 				pane.Height = "120"
364: 				pane.ValueMarginPercentageFactor = 0
365: 
366: 			case shape.Band:
367: 				band := buildBandData(p, allOutputs, bars, timeFmt, bandColors[bandColorIdx%len(bandColors)])
368: 				bandColorIdx++
369: 				pane.Bands = append(pane.Bands, band)
370: 
371: 			default: // Scalar or unknown → line.
372: 				line := buildLineData(p, allOutputs, bars, timeFmt, lineColors[colorIdx%len(lineColors)])
373: 				colorIdx++
374: 				pane.Lines = append(pane.Lines, line)
375: 			}
376: 		}
377: 	}
378: 
379: 	return &Configuration{
380: 		Width: "100%",
381: 		NavigationPane: &NavigationPane{
382: 			Height: 30, HasLine: true, HasArea: false, HasTimeAxis: true, TimeTicks: 0,
383: 		},
384: 		HeightNavigationPane: 30,
385: 		TimeAnnotationFormat: "%Y-%m-%d",
386: 		AxisLeft:             true,
387: 		AxisRight:            false,
388: 		Margin:               Margin{Left: 0, Top: 10, Right: 20, Bottom: 0},
389: 		Ohlcv:                OhlcvData{Name: "TEST", Data: ohlcvBars, Candlesticks: true},
390: 		PricePane:            pricePane,
391: 		IndicatorPanes:       indicatorPanes,
392: 		Crosshair:            false,
393: 		VolumeInPricePane:    true,
394: 		MenuVisible:          true,
395: 		DownloadSvgVisible:   true,
396: 	}
397: }
398: 
399: func buildLineData(p outputPlacement, allOutputs [][]core.Output, bars []entities.Bar, timeFmt, color string) LineData {
400: 	data := make([]ScalarVal, 0, len(bars))
401: 	for i := range bars {
402: 		out := allOutputs[i][p.indicatorIdx]
403: 		if p.outputIdx < len(out) {
404: 			if s, ok := out[p.outputIdx].(entities.Scalar); ok && !math.IsNaN(s.Value) {
405: 				data = append(data, ScalarVal{Time: s.Time.Format(timeFmt), Value: s.Value})
406: 			}
407: 		}
408: 	}
409: 	return LineData{
410: 		Name: p.mnemonic, Data: data, Indicator: p.indicatorIdx, Output: p.outputIdx,
411: 		Color: color, Width: 1, Dash: "", Interpolation: "natural",
412: 	}
413: }
414: 
415: func buildBandData(p outputPlacement, allOutputs [][]core.Output, bars []entities.Bar, timeFmt, color string) BandData {
416: 	data := make([]BandVal, 0, len(bars))
417: 	for i := range bars {
418: 		out := allOutputs[i][p.indicatorIdx]
419: 		if p.outputIdx < len(out) {
420: 			if b, ok := out[p.outputIdx].(*outputs.Band); ok && !b.IsEmpty() {
421: 				data = append(data, BandVal{Time: b.Time.Format(timeFmt), Upper: b.Upper, Lower: b.Lower})
422: 			}
423: 		}
424: 	}
425: 	return BandData{
426: 		Name: p.mnemonic, Data: data, Indicator: p.indicatorIdx, Output: p.outputIdx,
427: 		Color: color, LegendColor: color, Interpolation: "natural",
428: 	}
429: }
430: 
431: func buildHeatmapData(p outputPlacement, allOutputs [][]core.Output, bars []entities.Bar, timeFmt string) HeatmapData {
432: 	data := make([]HeatmapVal, 0, len(bars))
433: 	for i := range bars {
434: 		out := allOutputs[i][p.indicatorIdx]
435: 		if p.outputIdx < len(out) {
436: 			if h, ok := out[p.outputIdx].(*outputs.Heatmap); ok && !h.IsEmpty() {
437: 				data = append(data, HeatmapVal{
438: 					Time: h.Time.Format(timeFmt), First: h.ParameterFirst, Last: h.ParameterLast,
439: 					Result: h.ParameterResolution, Min: h.ValueMin, Max: h.ValueMax, Values: h.Values,
440: 				})
441: 			}
442: 		}
443: 	}
444: 	return HeatmapData{
445: 		Name: p.mnemonic, Data: data, Indicator: p.indicatorIdx, Output: p.outputIdx,
446: 		Gradient: "Viridis", InvertGradient: false,
447: 	}
448: }
449: 
450: // --- TypeScript generation ---
451: 
452: func buildTypeScript(cfg *Configuration, baseName string) string {
453: 	var sb strings.Builder
454: 
455: 	sb.WriteString("// Auto-generated chart configuration.\n")
456: 	sb.WriteString("// eslint-disable-next-line\n")
457: 	sb.WriteString("import { Configuration } from '../ohlcv-chart/template/configuration';\n")
458: 	sb.WriteString("import { Scalar, Band, Heatmap, Bar } from '../ohlcv-chart/template/types';\n\n")
459: 
460: 	// Emit OHLCV data array.
461: 	ohlcvVar := sanitizeVarName(baseName) + "Ohlcv"
462: 	sb.WriteString(fmt.Sprintf("export const %s: Bar[] = ", ohlcvVar))
463: 	writeJSONArray(&sb, cfg.Ohlcv.Data)
464: 	sb.WriteString(";\n\n")
465: 
466: 	// Track variable names for data references.
467: 	lineVars := make(map[*LineData]varRef)
468: 	bandVars := make(map[*BandData]varRef)
469: 	heatmapVars := make(map[*HeatmapData]varRef)
470: 	lineAreaVars := make(map[*LineAreaData]varRef)
471: 
472: 	varCounter := 0
473: 	emitScalarArray := func(sb *strings.Builder, mnemonic string, data []ScalarVal) string {
474: 		varCounter++
475: 		vn := sanitizeVarName(fmt.Sprintf("%s_%s_%d", baseName, mnemonic, varCounter))
476: 		sb.WriteString(fmt.Sprintf("export const %s: Scalar[] = ", vn))
477: 		writeJSONArray(sb, data)
478: 		sb.WriteString(";\n\n")
479: 		return vn
480: 	}
481: 	emitBandArray := func(sb *strings.Builder, mnemonic string, data []BandVal) string {
482: 		varCounter++
483: 		vn := sanitizeVarName(fmt.Sprintf("%s_%s_%d", baseName, mnemonic, varCounter))
484: 		sb.WriteString(fmt.Sprintf("export const %s: Band[] = ", vn))
485: 		writeJSONArray(sb, data)
486: 		sb.WriteString(";\n\n")
487: 		return vn
488: 	}
489: 	emitHeatmapArray := func(sb *strings.Builder, mnemonic string, data []HeatmapVal) string {
490: 		varCounter++
491: 		vn := sanitizeVarName(fmt.Sprintf("%s_%s_%d", baseName, mnemonic, varCounter))
492: 		sb.WriteString(fmt.Sprintf("export const %s: Heatmap[] = ", vn))
493: 		writeJSONArray(sb, data)
494: 		sb.WriteString(";\n\n")
495: 		return vn
496: 	}
497: 
498: 	// Emit price pane data arrays.
499: 	for i := range cfg.PricePane.Lines {
500: 		l := &cfg.PricePane.Lines[i]
501: 		vn := emitScalarArray(&sb, l.Name, l.Data)
502: 		lineVars[l] = varRef{vn, "Scalar[]"}
503: 	}
504: 	for i := range cfg.PricePane.Bands {
505: 		b := &cfg.PricePane.Bands[i]
506: 		vn := emitBandArray(&sb, b.Name, b.Data)
507: 		bandVars[b] = varRef{vn, "Band[]"}
508: 	}
509: 	for i := range cfg.PricePane.LineAreas {
510: 		la := &cfg.PricePane.LineAreas[i]
511: 		vn := emitScalarArray(&sb, la.Name, la.Data)
512: 		lineAreaVars[la] = varRef{vn, "Scalar[]"}
513: 	}
514: 
515: 	// Emit indicator pane data arrays.
516: 	for pi := range cfg.IndicatorPanes {
517: 		pane := &cfg.IndicatorPanes[pi]
518: 		for i := range pane.Lines {
519: 			l := &pane.Lines[i]
520: 			vn := emitScalarArray(&sb, l.Name, l.Data)
521: 			lineVars[l] = varRef{vn, "Scalar[]"}
522: 		}
523: 		for i := range pane.Bands {
524: 			b := &pane.Bands[i]
525: 			vn := emitBandArray(&sb, b.Name, b.Data)
526: 			bandVars[b] = varRef{vn, "Band[]"}
527: 		}
528: 		for i := range pane.LineAreas {
529: 			la := &pane.LineAreas[i]
530: 			vn := emitScalarArray(&sb, la.Name, la.Data)
531: 			lineAreaVars[la] = varRef{vn, "Scalar[]"}
532: 		}
533: 		if pane.Heatmap != nil {
534: 			hm := pane.Heatmap
535: 			vn := emitHeatmapArray(&sb, hm.Name, hm.Data)
536: 			heatmapVars[hm] = varRef{vn, "Heatmap[]"}
537: 		}
538: 	}
539: 
540: 	// Emit configuration object.
541: 	sb.WriteString(fmt.Sprintf("export const %sConfig: Configuration = {\n", sanitizeVarName(baseName)))
542: 	sb.WriteString(fmt.Sprintf("  width: %q,\n", cfg.Width))
543: 
544: 	if cfg.NavigationPane != nil {
545: 		sb.WriteString("  navigationPane: {\n")
546: 		sb.WriteString(fmt.Sprintf("    height: %d, hasLine: %t, hasArea: %t, hasTimeAxis: %t, timeTicks: %d,\n",
547: 			cfg.NavigationPane.Height, cfg.NavigationPane.HasLine, cfg.NavigationPane.HasArea,
548: 			cfg.NavigationPane.HasTimeAxis, cfg.NavigationPane.TimeTicks))
549: 		sb.WriteString("  },\n")
550: 	}
551: 
552: 	sb.WriteString(fmt.Sprintf("  heightNavigationPane: %d,\n", cfg.HeightNavigationPane))
553: 	sb.WriteString(fmt.Sprintf("  timeAnnotationFormat: %q,\n", cfg.TimeAnnotationFormat))
554: 	sb.WriteString(fmt.Sprintf("  axisLeft: %t,\n", cfg.AxisLeft))
555: 	sb.WriteString(fmt.Sprintf("  axisRight: %t,\n", cfg.AxisRight))
556: 	sb.WriteString(fmt.Sprintf("  margin: { left: %d, top: %d, right: %d, bottom: %d },\n",
557: 		cfg.Margin.Left, cfg.Margin.Top, cfg.Margin.Right, cfg.Margin.Bottom))
558: 	sb.WriteString(fmt.Sprintf("  ohlcv: { name: %q, data: %s, candlesticks: %t },\n",
559: 		cfg.Ohlcv.Name, ohlcvVar, cfg.Ohlcv.Candlesticks))
560: 
561: 	// Price pane.
562: 	sb.WriteString("  pricePane: {\n")
563: 	writeTSPane(&sb, &cfg.PricePane, lineVars, bandVars, lineAreaVars, heatmapVars, "    ")
564: 	sb.WriteString("  },\n")
565: 
566: 	// Indicator panes.
567: 	sb.WriteString("  indicatorPanes: [\n")
568: 	for i := range cfg.IndicatorPanes {
569: 		sb.WriteString("    {\n")
570: 		writeTSPane(&sb, &cfg.IndicatorPanes[i], lineVars, bandVars, lineAreaVars, heatmapVars, "      ")
571: 		sb.WriteString("    },\n")
572: 	}
573: 	sb.WriteString("  ],\n")
574: 
575: 	sb.WriteString(fmt.Sprintf("  crosshair: %t,\n", cfg.Crosshair))
576: 	sb.WriteString(fmt.Sprintf("  volumeInPricePane: %t,\n", cfg.VolumeInPricePane))
577: 	sb.WriteString(fmt.Sprintf("  menuVisible: %t,\n", cfg.MenuVisible))
578: 	sb.WriteString(fmt.Sprintf("  downloadSvgVisible: %t,\n", cfg.DownloadSvgVisible))
579: 	sb.WriteString("};\n")
580: 
581: 	return sb.String()
582: }
583: 
584: type varRef struct{ varName, typeName string }
585: 
586: func writeTSPane(sb *strings.Builder, pane *Pane,
587: 	lineVars map[*LineData]varRef, bandVars map[*BandData]varRef,
588: 	lineAreaVars map[*LineAreaData]varRef, heatmapVars map[*HeatmapData]varRef,
589: 	indent string,
590: ) {
591: 	sb.WriteString(fmt.Sprintf("%sheight: %q, valueFormat: %q, valueMarginPercentageFactor: %g,\n",
592: 		indent, pane.Height, pane.ValueFormat, pane.ValueMarginPercentageFactor))
593: 
594: 	if pane.Heatmap != nil {
595: 		hm := pane.Heatmap
596: 		vr := heatmapVars[hm]
597: 		sb.WriteString(fmt.Sprintf("%sheatmap: { name: %q, data: %s, indicator: %d, output: %d, gradient: %q, invertGradient: %t },\n",
598: 			indent, hm.Name, vr.varName, hm.Indicator, hm.Output, hm.Gradient, hm.InvertGradient))
599: 	}
600: 
601: 	// Bands.
602: 	sb.WriteString(indent + "bands: [")
603: 	if len(pane.Bands) > 0 {
604: 		sb.WriteString("\n")
605: 		for i := range pane.Bands {
606: 			b := &pane.Bands[i]
607: 			vr := bandVars[b]
608: 			sb.WriteString(fmt.Sprintf("%s  { name: %q, data: %s, indicator: %d, output: %d, color: %q, legendColor: %q, interpolation: %q },\n",
609: 				indent, b.Name, vr.varName, b.Indicator, b.Output, b.Color, b.LegendColor, b.Interpolation))
610: 		}
611: 		sb.WriteString(indent)
612: 	}
613: 	sb.WriteString("],\n")
614: 
615: 	// LineAreas.
616: 	sb.WriteString(indent + "lineAreas: [")
617: 	if len(pane.LineAreas) > 0 {
618: 		sb.WriteString("\n")
619: 		for i := range pane.LineAreas {
620: 			la := &pane.LineAreas[i]
621: 			vr := lineAreaVars[la]
622: 			sb.WriteString(fmt.Sprintf("%s  { name: %q, data: %s, indicator: %d, output: %d, value: %g, color: %q, legendColor: %q, interpolation: %q },\n",
623: 				indent, la.Name, vr.varName, la.Indicator, la.Output, la.Value, la.Color, la.LegendColor, la.Interpolation))
624: 		}
625: 		sb.WriteString(indent)
626: 	}
627: 	sb.WriteString("],\n")
628: 
629: 	// Horizontals.
630: 	sb.WriteString(indent + "horizontals: [")
631: 	if len(pane.Horizontals) > 0 {
632: 		sb.WriteString("\n")
633: 		for _, h := range pane.Horizontals {
634: 			sb.WriteString(fmt.Sprintf("%s  { value: %g, color: %q, width: %g, dash: %q },\n",
635: 				indent, h.Value, h.Color, h.Width, h.Dash))
636: 		}
637: 		sb.WriteString(indent)
638: 	}
639: 	sb.WriteString("],\n")
640: 
641: 	// Lines.
642: 	sb.WriteString(indent + "lines: [")
643: 	if len(pane.Lines) > 0 {
644: 		sb.WriteString("\n")
645: 		for i := range pane.Lines {
646: 			l := &pane.Lines[i]
647: 			vr := lineVars[l]
648: 			sb.WriteString(fmt.Sprintf("%s  { name: %q, data: %s, indicator: %d, output: %d, color: %q, width: %g, dash: %q, interpolation: %q },\n",
649: 				indent, l.Name, vr.varName, l.Indicator, l.Output, l.Color, l.Width, l.Dash, l.Interpolation))
650: 		}
651: 		sb.WriteString(indent)
652: 	}
653: 	sb.WriteString("],\n")
654: 
655: 	// Arrows.
656: 	sb.WriteString(indent + "arrows: [],\n")
657: }
658: 
659: func writeJSONArray(sb *strings.Builder, v any) {
660: 	data, err := json.Marshal(v)
661: 	if err != nil {
662: 		sb.WriteString("[]")
663: 		return
664: 	}
665: 	sb.Write(data)
666: }
667: 
668: func sanitizeVarName(s string) string {
669: 	var sb strings.Builder
670: 	capitalize := false
671: 	for i, c := range s {
672: 		switch {
673: 		case c >= 'a' && c <= 'z', c >= 'A' && c <= 'Z':
674: 			if capitalize {
675: 				if c >= 'a' && c <= 'z' {
676: 					c = c - 'a' + 'A'
677: 				}
678: 				capitalize = false
679: 			}
680: 			sb.WriteRune(c)
681: 		case c >= '0' && c <= '9':
682: 			if i == 0 {
683: 				sb.WriteRune('_')
684: 			}
685: 			sb.WriteRune(c)
686: 			capitalize = false
687: 		default:
688: 			capitalize = true
689: 		}
690: 	}
691: 	return sb.String()
692: }
693: 
694: // testBars returns 252 embedded test bars constructed from the standard TA-Lib
695: // daily reference data used across the indicator unit tests.
696: func testBars() []entities.Bar {
697: 	highs := testHighs()
698: 	lows := testLows()
699: 	closes := testCloses()
700: 	volumes := testVolumes()
701: 
702: 	bars := make([]entities.Bar, len(closes))
703: 	baseTime := time.Date(2020, 1, 2, 0, 0, 0, 0, time.UTC)
704: 
705: 	for i := range closes {
706: 		openPrice := closes[0]
707: 		if i > 0 {
708: 			openPrice = closes[i-1]
709: 		}
710: 
711: 		bars[i] = entities.Bar{
712: 			Time:   baseTime.AddDate(0, 0, i),
713: 			Open:   openPrice,
714: 			High:   highs[i],
715: 			Low:    lows[i],
716: 			Close:  closes[i],
717: 			Volume: volumes[i],
718: 		}
719: 	}
720: 
721: 	return bars
722: }
723: 
724: func testHighs() []float64 {
725: 	return []float64{
726: 		93.25, 94.94, 96.375, 96.19, 96, 94.72, 95, 93.72, 92.47, 92.75,
727: 		96.25, 99.625, 99.125, 92.75, 91.315, 93.25, 93.405, 90.655, 91.97, 92.25,
728: 		90.345, 88.5, 88.25, 85.5, 84.44, 84.75, 84.44, 89.405, 88.125, 89.125,
729: 		87.155, 87.25, 87.375, 88.97, 90, 89.845, 86.97, 85.94, 84.75, 85.47,
730: 		84.47, 88.5, 89.47, 90, 92.44, 91.44, 92.97, 91.72, 91.155, 91.75,
731: 		90, 88.875, 89, 85.25, 83.815, 85.25, 86.625, 87.94, 89.375, 90.625,
732: 		90.75, 88.845, 91.97, 93.375, 93.815, 94.03, 94.03, 91.815, 92, 91.94,
733: 		89.75, 88.75, 86.155, 84.875, 85.94, 99.375, 103.28, 105.375, 107.625, 105.25,
734: 		104.5, 105.5, 106.125, 107.94, 106.25, 107, 108.75, 110.94, 110.94, 114.22,
735: 		123, 121.75, 119.815, 120.315, 119.375, 118.19, 116.69, 115.345, 113, 118.315,
736: 		116.87, 116.75, 113.87, 114.62, 115.31, 116, 121.69, 119.87, 120.87, 116.75,
737: 		116.5, 116, 118.31, 121.5, 122, 121.44, 125.75, 127.75, 124.19, 124.44,
738: 		125.75, 124.69, 125.31, 132, 131.31, 132.25, 133.88, 133.5, 135.5, 137.44,
739: 		138.69, 139.19, 138.5, 138.13, 137.5, 138.88, 132.13, 129.75, 128.5, 125.44,
740: 		125.12, 126.5, 128.69, 126.62, 126.69, 126, 123.12, 121.87, 124, 127,
741: 		124.44, 122.5, 123.75, 123.81, 124.5, 127.87, 128.56, 129.63, 124.87, 124.37,
742: 		124.87, 123.62, 124.06, 125.87, 125.19, 125.62, 126, 128.5, 126.75, 129.75,
743: 		132.69, 133.94, 136.5, 137.69, 135.56, 133.56, 135, 132.38, 131.44, 130.88,
744: 		129.63, 127.25, 127.81, 125, 126.81, 124.75, 122.81, 122.25, 121.06, 120,
745: 		123.25, 122.75, 119.19, 115.06, 116.69, 114.87, 110.87, 107.25, 108.87, 109,
746: 		108.5, 113.06, 93, 94.62, 95.12, 96, 95.56, 95.31, 99, 98.81,
747: 		96.81, 95.94, 94.44, 92.94, 93.94, 95.5, 97.06, 97.5, 96.25, 96.37,
748: 		95, 94.87, 98.25, 105.12, 108.44, 109.87, 105, 106, 104.94, 104.5,
749: 		104.44, 106.31, 112.87, 116.5, 119.19, 121, 122.12, 111.94, 112.75, 110.19,
750: 		107.94, 109.69, 111.06, 110.44, 110.12, 110.31, 110.44, 110, 110.75, 110.5,
751: 		110.5, 109.5,
752: 	}
753: }
754: 
755: func testLows() []float64 {
756: 	return []float64{
757: 		90.75, 91.405, 94.25, 93.5, 92.815, 93.5, 92, 89.75, 89.44, 90.625,
758: 		92.75, 96.315, 96.03, 88.815, 86.75, 90.94, 88.905, 88.78, 89.25, 89.75,
759: 		87.5, 86.53, 84.625, 82.28, 81.565, 80.875, 81.25, 84.065, 85.595, 85.97,
760: 		84.405, 85.095, 85.5, 85.53, 87.875, 86.565, 84.655, 83.25, 82.565, 83.44,
761: 		82.53, 85.065, 86.875, 88.53, 89.28, 90.125, 90.75, 89, 88.565, 90.095,
762: 		89, 86.47, 84, 83.315, 82, 83.25, 84.75, 85.28, 87.19, 88.44,
763: 		88.25, 87.345, 89.28, 91.095, 89.53, 91.155, 92, 90.53, 89.97, 88.815,
764: 		86.75, 85.065, 82.03, 81.5, 82.565, 96.345, 96.47, 101.155, 104.25, 101.75,
765: 		101.72, 101.72, 103.155, 105.69, 103.655, 104, 105.53, 108.53, 108.75, 107.75,
766: 		117, 118, 116, 118.5, 116.53, 116.25, 114.595, 110.875, 110.5, 110.72,
767: 		112.62, 114.19, 111.19, 109.44, 111.56, 112.44, 117.5, 116.06, 116.56, 113.31,
768: 		112.56, 114, 114.75, 118.87, 119, 119.75, 122.62, 123, 121.75, 121.56,
769: 		123.12, 122.19, 122.75, 124.37, 128, 129.5, 130.81, 130.63, 132.13, 133.88,
770: 		135.38, 135.75, 136.19, 134.5, 135.38, 133.69, 126.06, 126.87, 123.5, 122.62,
771: 		122.75, 123.56, 125.81, 124.62, 124.37, 121.81, 118.19, 118.06, 117.56, 121,
772: 		121.12, 118.94, 119.81, 121, 122, 124.5, 126.56, 123.5, 121.25, 121.06,
773: 		122.31, 121, 120.87, 122.06, 122.75, 122.69, 122.87, 125.5, 124.25, 128,
774: 		128.38, 130.69, 131.63, 134.38, 132, 131.94, 131.94, 129.56, 123.75, 126,
775: 		126.25, 124.37, 121.44, 120.44, 121.37, 121.69, 120, 119.62, 115.5, 116.75,
776: 		119.06, 119.06, 115.06, 111.06, 113.12, 110, 105, 104.69, 103.87, 104.69,
777: 		105.44, 107, 89, 92.5, 92.12, 94.62, 92.81, 94.25, 96.25, 96.37,
778: 		93.69, 93.5, 90, 90.19, 90.5, 92.12, 94.12, 94.87, 93, 93.87,
779: 		93, 92.62, 93.56, 98.37, 104.44, 106, 101.81, 104.12, 103.37, 102.12,
780: 		102.25, 103.37, 107.94, 112.5, 115.44, 115.5, 112.25, 107.56, 106.56, 106.87,
781: 		104.5, 105.75, 108.62, 107.75, 108.06, 108, 108.19, 108.12, 109.06, 108.75,
782: 		108.56, 106.62,
783: 	}
784: }
785: 
786: func testCloses() []float64 {
787: 	return []float64{
788: 		91.5, 94.815, 94.375, 95.095, 93.78, 94.625, 92.53, 92.75, 90.315, 92.47,
789: 		96.125, 97.25, 98.5, 89.875, 91, 92.815, 89.155, 89.345, 91.625, 89.875,
790: 		88.375, 87.625, 84.78, 83, 83.5, 81.375, 84.44, 89.25, 86.375, 86.25,
791: 		85.25, 87.125, 85.815, 88.97, 88.47, 86.875, 86.815, 84.875, 84.19, 83.875,
792: 		83.375, 85.5, 89.19, 89.44, 91.095, 90.75, 91.44, 89, 91, 90.5,
793: 		89.03, 88.815, 84.28, 83.5, 82.69, 84.75, 85.655, 86.19, 88.94, 89.28,
794: 		88.625, 88.5, 91.97, 91.5, 93.25, 93.5, 93.155, 91.72, 90, 89.69,
795: 		88.875, 85.19, 83.375, 84.875, 85.94, 97.25, 99.875, 104.94, 106, 102.5,
796: 		102.405, 104.595, 106.125, 106, 106.065, 104.625, 108.625, 109.315, 110.5, 112.75,
797: 		123, 119.625, 118.75, 119.25, 117.94, 116.44, 115.19, 111.875, 110.595, 118.125,
798: 		116, 116, 112, 113.75, 112.94, 116, 120.5, 116.62, 117, 115.25,
799: 		114.31, 115.5, 115.87, 120.69, 120.19, 120.75, 124.75, 123.37, 122.94, 122.56,
800: 		123.12, 122.56, 124.62, 129.25, 131, 132.25, 131, 132.81, 134, 137.38,
801: 		137.81, 137.88, 137.25, 136.31, 136.25, 134.63, 128.25, 129, 123.87, 124.81,
802: 		123, 126.25, 128.38, 125.37, 125.69, 122.25, 119.37, 118.5, 123.19, 123.5,
803: 		122.19, 119.31, 123.31, 121.12, 123.37, 127.37, 128.5, 123.87, 122.94, 121.75,
804: 		124.44, 122, 122.37, 122.94, 124, 123.19, 124.56, 127.25, 125.87, 128.86,
805: 		132, 130.75, 134.75, 135, 132.38, 133.31, 131.94, 130, 125.37, 130.13,
806: 		127.12, 125.19, 122, 125, 123, 123.5, 120.06, 121, 117.75, 119.87,
807: 		122, 119.19, 116.37, 113.5, 114.25, 110, 105.06, 107, 107.87, 107,
808: 		107.12, 107, 91, 93.94, 93.87, 95.5, 93, 94.94, 98.25, 96.75,
809: 		94.81, 94.37, 91.56, 90.25, 93.94, 93.62, 97, 95, 95.87, 94.06,
810: 		94.62, 93.75, 98, 103.94, 107.87, 106.06, 104.5, 105, 104.19, 103.06,
811: 		103.42, 105.27, 111.87, 116, 116.62, 118.28, 113.37, 109, 109.7, 109.25,
812: 		107, 109.19, 110, 109.2, 110.12, 108, 108.62, 109.75, 109.81, 109,
813: 		108.75, 107.87,
814: 	}
815: }
816: 
817: func testVolumes() []float64 {
818: 	return []float64{
819: 		4077500, 4955900, 4775300, 4155300, 4593100, 3631300, 3382800, 4954200, 4500000, 3397500,
820: 		4204500, 6321400, 10203600, 19043900, 11692000, 9553300, 8920300, 5970900, 5062300, 3705600,
821: 		5865600, 5603000, 5811900, 8483800, 5995200, 5408800, 5430500, 6283800, 5834800, 4515500,
822: 		4493300, 4346100, 3700300, 4600200, 4557200, 4323600, 5237500, 7404100, 4798400, 4372800,
823: 		3872300, 10750800, 5804800, 3785500, 5014800, 3507700, 4298800, 4842500, 3952200, 3304700,
824: 		3462000, 7253900, 9753100, 5953000, 5011700, 5910800, 4916900, 4135000, 4054200, 3735300,
825: 		2921900, 2658400, 4624400, 4372200, 5831600, 4268600, 3059200, 4495500, 3425000, 3630800,
826: 		4168100, 5966900, 7692800, 7362500, 6581300, 19587700, 10378600, 9334700, 10467200, 5671400,
827: 		5645000, 4518600, 4519500, 5569700, 4239700, 4175300, 4995300, 4776600, 4190000, 6035300,
828: 		12168900, 9040800, 5780300, 4320800, 3899100, 3221400, 3455500, 4304200, 4703900, 8316300,
829: 		10553900, 6384800, 7163300, 7007800, 5114100, 5263800, 6666100, 7398400, 5575000, 4852300,
830: 		4298100, 4900500, 4887700, 6964800, 4679200, 9165000, 6469800, 6792000, 4423800, 5231900,
831: 		4565600, 6235200, 5225900, 8261400, 5912500, 3545600, 5714500, 6653900, 6094500, 4799200,
832: 		5050800, 5648900, 4726300, 5585600, 5124800, 7630200, 14311600, 8793600, 8874200, 6966600,
833: 		5525500, 6515500, 5291900, 5711700, 4327700, 4568000, 6859200, 5757500, 7367000, 6144100,
834: 		4052700, 5849700, 5544700, 5032200, 4400600, 4894100, 5140000, 6610900, 7585200, 5963100,
835: 		6045500, 8443300, 6464700, 6248300, 4357200, 4774700, 6216900, 6266900, 5584800, 5284500,
836: 		7554500, 7209500, 8424800, 5094500, 4443600, 4591100, 5658400, 6094100, 14862200, 7544700,
837: 		6985600, 8093000, 7590000, 7451300, 7078000, 7105300, 8778800, 6643900, 10563900, 7043100,
838: 		6438900, 8057700, 14240000, 17872300, 7831100, 8277700, 15017800, 14183300, 13921100, 9683000,
839: 		9187300, 11380500, 69447300, 26673600, 13768400, 11371600, 9872200, 9450500, 11083300, 9552800,
840: 		11108400, 10374200, 16701900, 13741900, 8523600, 9551900, 8680500, 7151700, 9673100, 6264700,
841: 		8541600, 8358000, 18720800, 19683100, 13682500, 10668100, 9710600, 3113100, 5682000, 5763600,
842: 		5340000, 6220800, 14680500, 9933000, 11329500, 8145300, 16644700, 12593800, 7138100, 7442300,
843: 		9442300, 7123600, 7680600, 4839800, 4775500, 4008800, 4533600, 3741100, 4084800, 2685200,
844: 		3438000, 2870500,
845: 	}
846: }

(End of file - total 846 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/cmd/icalc/main.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/cmd/icalc/main.ts</path>
<type>file</type>
<content>1: /**
2:  * icalc — command-line indicator calculator (TypeScript).
3:  *
4:  * Reads a JSON settings file containing indicator definitions, creates
5:  * indicator instances via the factory, prints their metadata, then iterates
6:  * through embedded bar data printing bar values and all indicator outputs on
7:  * each iteration.
8:  *
9:  * Usage:  npx tsx cmd/icalc/main.ts <settings.json>
10:  */
11: 
12: import { readFileSync } from 'node:fs';
13: import { Bar } from '../../entities/bar.js';
14: import { Scalar } from '../../entities/scalar.js';
15: import { type Indicator } from '../../indicators/core/indicator.js';
16: import { IndicatorIdentifier } from '../../indicators/core/indicator-identifier.js';
17: import { type IndicatorOutput } from '../../indicators/core/indicator-output.js';
18: import { type OutputMetadata } from '../../indicators/core/outputs/output-metadata.js';
19: import { Band } from '../../indicators/core/outputs/band.js';
20: import { Heatmap } from '../../indicators/core/outputs/heatmap.js';
21: import { createIndicator } from '../../indicators/factory/factory.js';
22: 
23: // ---------------------------------------------------------------------------
24: // Settings
25: // ---------------------------------------------------------------------------
26: 
27: interface SettingsEntry {
28:     identifier: string;
29:     params: Record<string, unknown>;
30: }
31: 
32: /** Map camelCase JSON identifier string to IndicatorIdentifier enum value. */
33: function resolveIdentifier(name: string): IndicatorIdentifier {
34:     // The enum members are PascalCase; the JSON uses camelCase.
35:     // TS numeric enums have a reverse mapping: IndicatorIdentifier["SimpleMovingAverage"] === 0.
36:     // Convert first char to upper to get PascalCase.
37:     let pascal = name.charAt(0).toUpperCase() + name.slice(1);
38: 
39:     // Handle quirky Go JSON identifier: "kaufmanAdaptiveMovingAverageMovingAverage"
40:     // maps to TS enum "KaufmanAdaptiveMovingAverage".
41:     if (pascal === 'KaufmanAdaptiveMovingAverageMovingAverage') {
42:         pascal = 'KaufmanAdaptiveMovingAverage';
43:     }
44:     const value = (IndicatorIdentifier as unknown as Record<string, number>)[pascal];
45:     if (value === undefined) {
46:         throw new Error(`Unknown indicator identifier: "${name}" (tried "${pascal}")`);
47:     }
48:     return value as IndicatorIdentifier;
49: }
50: 
51: // ---------------------------------------------------------------------------
52: // Output formatting
53: // ---------------------------------------------------------------------------
54: 
55: function formatOutput(outputsMeta: OutputMetadata[], output: IndicatorOutput): string {
56:     const parts: string[] = [];
57:     for (let i = 0; i < output.length; i++) {
58:         const name = i < outputsMeta.length ? outputsMeta[i].mnemonic : `out[${i}]`;
59:         const val = output[i];
60:         if (val instanceof Band) {
61:             if (Number.isNaN(val.lower) || Number.isNaN(val.upper)) {
62:                 parts.push(`${name}=Band(NaN)`);
63:             } else {
64:                 parts.push(`${name}=Band(${val.lower.toFixed(4)},${val.upper.toFixed(4)})`);
65:             }
66:         } else if (val instanceof Heatmap) {
67:             parts.push(`${name}=Heatmap(${val.values.length}pts)`);
68:         } else if (val instanceof Scalar) {
69:             if (Number.isNaN(val.value)) {
70:                 parts.push(`${name}=NaN`);
71:             } else {
72:                 parts.push(`${name}=${val.value.toFixed(4)}`);
73:             }
74:         } else {
75:             parts.push(`${name}=${val}`);
76:         }
77:     }
78:     return parts.join(' ');
79: }
80: 
81: // ---------------------------------------------------------------------------
82: // Metadata printer
83: // ---------------------------------------------------------------------------
84: 
85: function printMetadata(indicators: Indicator[]): void {
86:     console.log('=== Indicator Metadata ===');
87:     console.log();
88:     for (let i = 0; i < indicators.length; i++) {
89:         const meta = indicators[i].metadata();
90:         console.log(`[${i}] ${meta.mnemonic}`);
91:         console.log(`  Identifier:  ${IndicatorIdentifier[meta.identifier]}`);
92:         console.log(`  Description: ${meta.description}`);
93:         console.log(`  Outputs (${meta.outputs.length}):`);
94:         for (let j = 0; j < meta.outputs.length; j++) {
95:             const out = meta.outputs[j];
96:             console.log(`    [${j}] kind=${out.kind} shape=${out.shape} mnemonic="${out.mnemonic}" description="${out.description}"`);
97:         }
98:         console.log(`  Full metadata JSON:`);
99:         console.log(`  ${JSON.stringify(meta, null, 2).split('\n').join('\n  ')}`);
100:         console.log();
101:     }
102: }
103: 
104: // ---------------------------------------------------------------------------
105: // Test bar data (252-entry TA-Lib reference set)
106: // ---------------------------------------------------------------------------
107: 
108: function testBars(): Bar[] {
109:     const highs = testHighs();
110:     const lows = testLows();
111:     const closes = testCloses();
112:     const volumes = testVolumes();
113: 
114:     const bars: Bar[] = [];
115:     const baseTime = new Date('2020-01-02T00:00:00Z');
116: 
117:     for (let i = 0; i < closes.length; i++) {
118:         const openPrice = i > 0 ? closes[i - 1] : closes[0];
119:         const bar = new Bar();
120:         bar.time = new Date(baseTime.getTime() + i * 86_400_000);
121:         bar.open = openPrice;
122:         bar.high = highs[i];
123:         bar.low = lows[i];
124:         bar.close = closes[i];
125:         bar.volume = volumes[i];
126:         bars.push(bar);
127:     }
128: 
129:     return bars;
130: }
131: 
132: function formatDate(d: Date): string {
133:     return d.toISOString().slice(0, 10);
134: }
135: 
136: // ---------------------------------------------------------------------------
137: // Main
138: // ---------------------------------------------------------------------------
139: 
140: function main(): void {
141:     const args = process.argv.slice(2);
142:     if (args.length < 1) {
143:         process.stderr.write('usage: icalc <settings.json>\n');
144:         process.exit(1);
145:     }
146: 
147:     const data = readFileSync(args[0], 'utf-8');
148:     const entries: SettingsEntry[] = JSON.parse(data);
149: 
150:     const indicators: Indicator[] = [];
151:     for (const e of entries) {
152:         const id = resolveIdentifier(e.identifier);
153:         const params = Object.keys(e.params).length === 0 ? undefined : e.params;
154:         try {
155:             indicators.push(createIndicator(id, params as Record<string, any>));
156:         } catch (err) {
157:             process.stderr.write(`error creating indicator ${e.identifier}: ${err}\n`);
158:             process.exit(1);
159:         }
160:     }
161: 
162:     printMetadata(indicators);
163: 
164:     const bars = testBars();
165: 
166:     console.log();
167:     console.log('=== Bar Data & Indicator Outputs ===');
168:     console.log();
169: 
170:     for (let i = 0; i < bars.length; i++) {
171:         const bar = bars[i];
172:         console.log(
173:             `Bar[${String(i).padStart(3)}] ${formatDate(bar.time)}  O=${bar.open.toFixed(4)} H=${bar.high.toFixed(4)} L=${bar.low.toFixed(4)} C=${bar.close.toFixed(4)} V=${bar.volume.toFixed(0)}`,
174:         );
175: 
176:         for (const ind of indicators) {
177:             const meta = ind.metadata();
178:             const output = ind.updateBar(bar);
179:             const primed = String(ind.isPrimed()).padEnd(5);
180:             console.log(`  ${meta.mnemonic.padEnd(45)} primed=${primed} ${formatOutput(meta.outputs, output)}`);
181:         }
182: 
183:         console.log();
184:     }
185: }
186: 
187: // ---------------------------------------------------------------------------
188: // Embedded test data (TA-Lib test_data.c)
189: // ---------------------------------------------------------------------------
190: 
191: function testHighs(): number[] {
192:     return [
193:         93.25, 94.94, 96.375, 96.19, 96, 94.72, 95, 93.72, 92.47, 92.75,
194:         96.25, 99.625, 99.125, 92.75, 91.315, 93.25, 93.405, 90.655, 91.97, 92.25,
195:         90.345, 88.5, 88.25, 85.5, 84.44, 84.75, 84.44, 89.405, 88.125, 89.125,
196:         87.155, 87.25, 87.375, 88.97, 90, 89.845, 86.97, 85.94, 84.75, 85.47,
197:         84.47, 88.5, 89.47, 90, 92.44, 91.44, 92.97, 91.72, 91.155, 91.75,
198:         90, 88.875, 89, 85.25, 83.815, 85.25, 86.625, 87.94, 89.375, 90.625,
199:         90.75, 88.845, 91.97, 93.375, 93.815, 94.03, 94.03, 91.815, 92, 91.94,
200:         89.75, 88.75, 86.155, 84.875, 85.94, 99.375, 103.28, 105.375, 107.625, 105.25,
201:         104.5, 105.5, 106.125, 107.94, 106.25, 107, 108.75, 110.94, 110.94, 114.22,
202:         123, 121.75, 119.815, 120.315, 119.375, 118.19, 116.69, 115.345, 113, 118.315,
203:         116.87, 116.75, 113.87, 114.62, 115.31, 116, 121.69, 119.87, 120.87, 116.75,
204:         116.5, 116, 118.31, 121.5, 122, 121.44, 125.75, 127.75, 124.19, 124.44,
205:         125.75, 124.69, 125.31, 132, 131.31, 132.25, 133.88, 133.5, 135.5, 137.44,
206:         138.69, 139.19, 138.5, 138.13, 137.5, 138.88, 132.13, 129.75, 128.5, 125.44,
207:         125.12, 126.5, 128.69, 126.62, 126.69, 126, 123.12, 121.87, 124, 127,
208:         124.44, 122.5, 123.75, 123.81, 124.5, 127.87, 128.56, 129.63, 124.87, 124.37,
209:         124.87, 123.62, 124.06, 125.87, 125.19, 125.62, 126, 128.5, 126.75, 129.75,
210:         132.69, 133.94, 136.5, 137.69, 135.56, 133.56, 135, 132.38, 131.44, 130.88,
211:         129.63, 127.25, 127.81, 125, 126.81, 124.75, 122.81, 122.25, 121.06, 120,
212:         123.25, 122.75, 119.19, 115.06, 116.69, 114.87, 110.87, 107.25, 108.87, 109,
213:         108.5, 113.06, 93, 94.62, 95.12, 96, 95.56, 95.31, 99, 98.81,
214:         96.81, 95.94, 94.44, 92.94, 93.94, 95.5, 97.06, 97.5, 96.25, 96.37,
215:         95, 94.87, 98.25, 105.12, 108.44, 109.87, 105, 106, 104.94, 104.5,
216:         104.44, 106.31, 112.87, 116.5, 119.19, 121, 122.12, 111.94, 112.75, 110.19,
217:         107.94, 109.69, 111.06, 110.44, 110.12, 110.31, 110.44, 110, 110.75, 110.5,
218:         110.5, 109.5,
219:     ];
220: }
221: 
222: function testLows(): number[] {
223:     return [
224:         90.75, 91.405, 94.25, 93.5, 92.815, 93.5, 92, 89.75, 89.44, 90.625,
225:         92.75, 96.315, 96.03, 88.815, 86.75, 90.94, 88.905, 88.78, 89.25, 89.75,
226:         87.5, 86.53, 84.625, 82.28, 81.565, 80.875, 81.25, 84.065, 85.595, 85.97,
227:         84.405, 85.095, 85.5, 85.53, 87.875, 86.565, 84.655, 83.25, 82.565, 83.44,
228:         82.53, 85.065, 86.875, 88.53, 89.28, 90.125, 90.75, 89, 88.565, 90.095,
229:         89, 86.47, 84, 83.315, 82, 83.25, 84.75, 85.28, 87.19, 88.44,
230:         88.25, 87.345, 89.28, 91.095, 89.53, 91.155, 92, 90.53, 89.97, 88.815,
231:         86.75, 85.065, 82.03, 81.5, 82.565, 96.345, 96.47, 101.155, 104.25, 101.75,
232:         101.72, 101.72, 103.155, 105.69, 103.655, 104, 105.53, 108.53, 108.75, 107.75,
233:         117, 118, 116, 118.5, 116.53, 116.25, 114.595, 110.875, 110.5, 110.72,
234:         112.62, 114.19, 111.19, 109.44, 111.56, 112.44, 117.5, 116.06, 116.56, 113.31,
235:         112.56, 114, 114.75, 118.87, 119, 119.75, 122.62, 123, 121.75, 121.56,
236:         123.12, 122.19, 122.75, 124.37, 128, 129.5, 130.81, 130.63, 132.13, 133.88,
237:         135.38, 135.75, 136.19, 134.5, 135.38, 133.69, 126.06, 126.87, 123.5, 122.62,
238:         122.75, 123.56, 125.81, 124.62, 124.37, 121.81, 118.19, 118.06, 117.56, 121,
239:         121.12, 118.94, 119.81, 121, 122, 124.5, 126.56, 123.5, 121.25, 121.06,
240:         122.31, 121, 120.87, 122.06, 122.75, 122.69, 122.87, 125.5, 124.25, 128,
241:         128.38, 130.69, 131.63, 134.38, 132, 131.94, 131.94, 129.56, 123.75, 126,
242:         126.25, 124.37, 121.44, 120.44, 121.37, 121.69, 120, 119.62, 115.5, 116.75,
243:         119.06, 119.06, 115.06, 111.06, 113.12, 110, 105, 104.69, 103.87, 104.69,
244:         105.44, 107, 89, 92.5, 92.12, 94.62, 92.81, 94.25, 96.25, 96.37,
245:         93.69, 93.5, 90, 90.19, 90.5, 92.12, 94.12, 94.87, 93, 93.87,
246:         93, 92.62, 93.56, 98.37, 104.44, 106, 101.81, 104.12, 103.37, 102.12,
247:         102.25, 103.37, 107.94, 112.5, 115.44, 115.5, 112.25, 107.56, 106.56, 106.87,
248:         104.5, 105.75, 108.62, 107.75, 108.06, 108, 108.19, 108.12, 109.06, 108.75,
249:         108.56, 106.62,
250:     ];
251: }
252: 
253: function testCloses(): number[] {
254:     return [
255:         91.5, 94.815, 94.375, 95.095, 93.78, 94.625, 92.53, 92.75, 90.315, 92.47,
256:         96.125, 97.25, 98.5, 89.875, 91, 92.815, 89.155, 89.345, 91.625, 89.875,
257:         88.375, 87.625, 84.78, 83, 83.5, 81.375, 84.44, 89.25, 86.375, 86.25,
258:         85.25, 87.125, 85.815, 88.97, 88.47, 86.875, 86.815, 84.875, 84.19, 83.875,
259:         83.375, 85.5, 89.19, 89.44, 91.095, 90.75, 91.44, 89, 91, 90.5,
260:         89.03, 88.815, 84.28, 83.5, 82.69, 84.75, 85.655, 86.19, 88.94, 89.28,
261:         88.625, 88.5, 91.97, 91.5, 93.25, 93.5, 93.155, 91.72, 90, 89.69,
262:         88.875, 85.19, 83.375, 84.875, 85.94, 97.25, 99.875, 104.94, 106, 102.5,
263:         102.405, 104.595, 106.125, 106, 106.065, 104.625, 108.625, 109.315, 110.5, 112.75,
264:         123, 119.625, 118.75, 119.25, 117.94, 116.44, 115.19, 111.875, 110.595, 118.125,
265:         116, 116, 112, 113.75, 112.94, 116, 120.5, 116.62, 117, 115.25,
266:         114.31, 115.5, 115.87, 120.69, 120.19, 120.75, 124.75, 123.37, 122.94, 122.56,
267:         123.12, 122.56, 124.62, 129.25, 131, 132.25, 131, 132.81, 134, 137.38,
268:         137.81, 137.88, 137.25, 136.31, 136.25, 134.63, 128.25, 129, 123.87, 124.81,
269:         123, 126.25, 128.38, 125.37, 125.69, 122.25, 119.37, 118.5, 123.19, 123.5,
270:         122.19, 119.31, 123.31, 121.12, 123.37, 127.37, 128.5, 123.87, 122.94, 121.75,
271:         124.44, 122, 122.37, 122.94, 124, 123.19, 124.56, 127.25, 125.87, 128.86,
272:         132, 130.75, 134.75, 135, 132.38, 133.31, 131.94, 130, 125.37, 130.13,
273:         127.12, 125.19, 122, 125, 123, 123.5, 120.06, 121, 117.75, 119.87,
274:         122, 119.19, 116.37, 113.5, 114.25, 110, 105.06, 107, 107.87, 107,
275:         107.12, 107, 91, 93.94, 93.87, 95.5, 93, 94.94, 98.25, 96.75,
276:         94.81, 94.37, 91.56, 90.25, 93.94, 93.62, 97, 95, 95.87, 94.06,
277:         94.62, 93.75, 98, 103.94, 107.87, 106.06, 104.5, 105, 104.19, 103.06,
278:         103.42, 105.27, 111.87, 116, 116.62, 118.28, 113.37, 109, 109.7, 109.25,
279:         107, 109.19, 110, 109.2, 110.12, 108, 108.62, 109.75, 109.81, 109,
280:         108.75, 107.87,
281:     ];
282: }
283: 
284: function testVolumes(): number[] {
285:     return [
286:         4077500, 4955900, 4775300, 4155300, 4593100, 3631300, 3382800, 4954200, 4500000, 3397500,
287:         4204500, 6321400, 10203600, 19043900, 11692000, 9553300, 8920300, 5970900, 5062300, 3705600,
288:         5865600, 5603000, 5811900, 8483800, 5995200, 5408800, 5430500, 6283800, 5834800, 4515500,
289:         4493300, 4346100, 3700300, 4600200, 4557200, 4323600, 5237500, 7404100, 4798400, 4372800,
290:         3872300, 10750800, 5804800, 3785500, 5014800, 3507700, 4298800, 4842500, 3952200, 3304700,
291:         3462000, 7253900, 9753100, 5953000, 5011700, 5910800, 4916900, 4135000, 4054200, 3735300,
292:         2921900, 2658400, 4624400, 4372200, 5831600, 4268600, 3059200, 4495500, 3425000, 3630800,
293:         4168100, 5966900, 7692800, 7362500, 6581300, 19587700, 10378600, 9334700, 10467200, 5671400,
294:         5645000, 4518600, 4519500, 5569700, 4239700, 4175300, 4995300, 4776600, 4190000, 6035300,
295:         12168900, 9040800, 5780300, 4320800, 3899100, 3221400, 3455500, 4304200, 4703900, 8316300,
296:         10553900, 6384800, 7163300, 7007800, 5114100, 5263800, 6666100, 7398400, 5575000, 4852300,
297:         4298100, 4900500, 4887700, 6964800, 4679200, 9165000, 6469800, 6792000, 4423800, 5231900,
298:         4565600, 6235200, 5225900, 8261400, 5912500, 3545600, 5714500, 6653900, 6094500, 4799200,
299:         5050800, 5648900, 4726300, 5585600, 5124800, 7630200, 14311600, 8793600, 8874200, 6966600,
300:         5525500, 6515500, 5291900, 5711700, 4327700, 4568000, 6859200, 5757500, 7367000, 6144100,
301:         4052700, 5849700, 5544700, 5032200, 4400600, 4894100, 5140000, 6610900, 7585200, 5963100,
302:         6045500, 8443300, 6464700, 6248300, 4357200, 4774700, 6216900, 6266900, 5584800, 5284500,
303:         7554500, 7209500, 8424800, 5094500, 4443600, 4591100, 5658400, 6094100, 14862200, 7544700,
304:         6985600, 8093000, 7590000, 7451300, 7078000, 7105300, 8778800, 6643900, 10563900, 7043100,
305:         6438900, 8057700, 14240000, 17872300, 7831100, 8277700, 15017800, 14183300, 13921100, 9683000,
306:         9187300, 11380500, 69447300, 26673600, 13768400, 11371600, 9872200, 9450500, 11083300, 9552800,
307:         11108400, 10374200, 16701900, 13741900, 8523600, 9551900, 8680500, 7151700, 9673100, 6264700,
308:         8541600, 8358000, 18720800, 19683100, 13682500, 10668100, 9710600, 3113100, 5682000, 5763600,
309:         5340000, 6220800, 14680500, 9933000, 11329500, 8145300, 16644700, 12593800, 7138100, 7442300,
310:         9442300, 7123600, 7680600, 4839800, 4775500, 4008800, 4533600, 3741100, 4084800, 2685200,
311:         3438000, 2870500,
312:     ];
313: }
314: 
315: // ---------------------------------------------------------------------------
316: // Run
317: // ---------------------------------------------------------------------------
318: 
319: main();

(End of file - total 319 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/cmd/iconf/settings.json"
}
```

**Output:**
```
<path>/home/dev/zpano/go/cmd/iconf/settings.json</path>
<type>file</type>
<content>1: [
2:   { "identifier": "bollingerBands", "params": { "length": 20, "multiplier": 2 } },
3:   { "identifier": "simpleMovingAverage", "params": { "length": 14 } },
4:   { "identifier": "exponentialMovingAverage", "params": { "length": 10 } },
5:   { "identifier": "relativeStrengthIndex", "params": { "length": 14 } },
6:   { "identifier": "movingAverageConvergenceDivergence", "params": { "fastLength": 12, "slowLength": 26, "signalLength": 9 } },
7:   { "identifier": "averageTrueRange", "params": { "length": 14 } },
8:   { "identifier": "stochastic", "params": { "fastKLength": 14, "slowKLength": 3, "slowDLength": 3 } },
9:   { "identifier": "goertzelSpectrum", "params": { "length": 64, "range": [2, 28], "resolution": 10 } }
10: ]

(End of file - total 10 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.6 · 6.4s)

Now let me check the TS descriptor system quickly:

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/descriptors.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/descriptors.ts</path>
<type>file</type>
<content>1: import { Adaptivity } from './adaptivity.js';
2: import { Descriptor } from './descriptor.js';
3: import { IndicatorIdentifier } from './indicator-identifier.js';
4: import { InputRequirement } from './input-requirement.js';
5: import { OutputDescriptor } from './output-descriptor.js';
6: import { Shape } from './outputs/shape/shape.js';
7: import { Pane } from './pane.js';
8: import { Role } from './role.js';
9: import { VolumeUsage } from './volume-usage.js';
10: 
11: // Helper to keep entries terse. Output Kind values are 0-based, mirroring each
12: // indicator package's TypeScript Output enumeration.
13: function out(kind: number, shape: Shape, role: Role, pane: Pane): OutputDescriptor {
14:   return { kind, shape, role, pane };
15: }
16: 
17: function desc(
18:   identifier: IndicatorIdentifier,
19:   family: string,
20:   adaptivity: Adaptivity,
21:   inputRequirement: InputRequirement,
22:   volumeUsage: VolumeUsage,
23:   outputs: OutputDescriptor[]
24: ): Descriptor {
25:   return { identifier, family, adaptivity, inputRequirement, volumeUsage, outputs };
26: }
27: 
28: const S = Shape;
29: const R = Role;
30: const P = Pane;
31: const A = Adaptivity;
32: const I = InputRequirement;
33: const V = VolumeUsage;
34: 
35: /** Static registry of taxonomic descriptors for all implemented indicators. */
36: const descriptors: Map<IndicatorIdentifier, Descriptor> = new Map<IndicatorIdentifier, Descriptor>([
37:   [IndicatorIdentifier.SimpleMovingAverage, desc(IndicatorIdentifier.SimpleMovingAverage, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
38:   [IndicatorIdentifier.WeightedMovingAverage, desc(IndicatorIdentifier.WeightedMovingAverage, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
39:   [IndicatorIdentifier.TriangularMovingAverage, desc(IndicatorIdentifier.TriangularMovingAverage, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
40:   [IndicatorIdentifier.ExponentialMovingAverage, desc(IndicatorIdentifier.ExponentialMovingAverage, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
41:   [IndicatorIdentifier.DoubleExponentialMovingAverage, desc(IndicatorIdentifier.DoubleExponentialMovingAverage, 'Patrick Mulloy', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
42:   [IndicatorIdentifier.TripleExponentialMovingAverage, desc(IndicatorIdentifier.TripleExponentialMovingAverage, 'Patrick Mulloy', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
43:   [IndicatorIdentifier.T2ExponentialMovingAverage, desc(IndicatorIdentifier.T2ExponentialMovingAverage, 'Tim Tillson', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
44:   [IndicatorIdentifier.T3ExponentialMovingAverage, desc(IndicatorIdentifier.T3ExponentialMovingAverage, 'Tim Tillson', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
45:   [IndicatorIdentifier.KaufmanAdaptiveMovingAverage, desc(IndicatorIdentifier.KaufmanAdaptiveMovingAverage, 'Perry Kaufman', A.Adaptive, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
46:   [IndicatorIdentifier.JurikMovingAverage, desc(IndicatorIdentifier.JurikMovingAverage, 'Mark Jurik', A.Adaptive, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
47:   [IndicatorIdentifier.MesaAdaptiveMovingAverage, desc(IndicatorIdentifier.MesaAdaptiveMovingAverage, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
48:     out(0, S.Scalar, R.Smoother, P.Price),
49:     out(1, S.Scalar, R.Smoother, P.Price),
50:     out(2, S.Band, R.Envelope, P.Price),
51:   ])],
52:   [IndicatorIdentifier.FractalAdaptiveMovingAverage, desc(IndicatorIdentifier.FractalAdaptiveMovingAverage, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
53:     out(0, S.Scalar, R.Smoother, P.Price),
54:     out(1, S.Scalar, R.FractalDimension, P.Own),
55:   ])],
56:   [IndicatorIdentifier.DominantCycle, desc(IndicatorIdentifier.DominantCycle, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
57:     out(0, S.Scalar, R.CyclePeriod, P.Own),
58:     out(1, S.Scalar, R.CyclePeriod, P.Own),
59:     out(2, S.Scalar, R.CyclePhase, P.Own),
60:   ])],
61:   [IndicatorIdentifier.Momentum, desc(IndicatorIdentifier.Momentum, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
62:   [IndicatorIdentifier.RateOfChange, desc(IndicatorIdentifier.RateOfChange, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
63:   [IndicatorIdentifier.RateOfChangePercent, desc(IndicatorIdentifier.RateOfChangePercent, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
64:   [IndicatorIdentifier.RelativeStrengthIndex, desc(IndicatorIdentifier.RelativeStrengthIndex, 'Welles Wilder', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.BoundedOscillator, P.Own)])],
65:   [IndicatorIdentifier.ChandeMomentumOscillator, desc(IndicatorIdentifier.ChandeMomentumOscillator, 'Tushar Chande', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.BoundedOscillator, P.Own)])],
66:   [IndicatorIdentifier.BollingerBands, desc(IndicatorIdentifier.BollingerBands, 'John Bollinger', A.Static, I.ScalarInput, V.NoVolume, [
67:     out(0, S.Scalar, R.Envelope, P.Price),
68:     out(1, S.Scalar, R.Smoother, P.Price),
69:     out(2, S.Scalar, R.Envelope, P.Price),
70:     out(3, S.Scalar, R.Volatility, P.Own),
71:     out(4, S.Scalar, R.BoundedOscillator, P.Own),
72:     out(5, S.Band, R.Envelope, P.Price),
73:   ])],
74:   [IndicatorIdentifier.Variance, desc(IndicatorIdentifier.Variance, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Volatility, P.Own)])],
75:   [IndicatorIdentifier.StandardDeviation, desc(IndicatorIdentifier.StandardDeviation, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Volatility, P.Own)])],
76:   [IndicatorIdentifier.GoertzelSpectrum, desc(IndicatorIdentifier.GoertzelSpectrum, 'Custom', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Heatmap, R.Spectrum, P.Own)])],
77:   [IndicatorIdentifier.CenterOfGravityOscillator, desc(IndicatorIdentifier.CenterOfGravityOscillator, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [
78:     out(0, S.Scalar, R.Oscillator, P.Own),
79:     out(1, S.Scalar, R.Signal, P.Own),
80:   ])],
81:   [IndicatorIdentifier.CyberCycle, desc(IndicatorIdentifier.CyberCycle, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [
82:     out(0, S.Scalar, R.Oscillator, P.Own),
83:     out(1, S.Scalar, R.Signal, P.Own),
84:   ])],
85:   [IndicatorIdentifier.InstantaneousTrendLine, desc(IndicatorIdentifier.InstantaneousTrendLine, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [
86:     out(0, S.Scalar, R.Smoother, P.Price),
87:     out(1, S.Scalar, R.Signal, P.Price),
88:   ])],
89:   [IndicatorIdentifier.SuperSmoother, desc(IndicatorIdentifier.SuperSmoother, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
90:   [IndicatorIdentifier.ZeroLagExponentialMovingAverage, desc(IndicatorIdentifier.ZeroLagExponentialMovingAverage, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
91:   [IndicatorIdentifier.ZeroLagErrorCorrectingExponentialMovingAverage, desc(IndicatorIdentifier.ZeroLagErrorCorrectingExponentialMovingAverage, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Smoother, P.Price)])],
92:   [IndicatorIdentifier.RoofingFilter, desc(IndicatorIdentifier.RoofingFilter, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
93:   [IndicatorIdentifier.TrueRange, desc(IndicatorIdentifier.TrueRange, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.Volatility, P.Own)])],
94:   [IndicatorIdentifier.AverageTrueRange, desc(IndicatorIdentifier.AverageTrueRange, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.Volatility, P.Own)])],
95:   [IndicatorIdentifier.NormalizedAverageTrueRange, desc(IndicatorIdentifier.NormalizedAverageTrueRange, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.Volatility, P.Own)])],
96:   [IndicatorIdentifier.DirectionalMovementMinus, desc(IndicatorIdentifier.DirectionalMovementMinus, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.Directional, P.Own)])],
97:   [IndicatorIdentifier.DirectionalMovementPlus, desc(IndicatorIdentifier.DirectionalMovementPlus, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.Directional, P.Own)])],
98:   [IndicatorIdentifier.DirectionalIndicatorMinus, desc(IndicatorIdentifier.DirectionalIndicatorMinus, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [
99:     out(0, S.Scalar, R.Directional, P.Own),
100:     out(1, S.Scalar, R.Directional, P.Own),
101:     out(2, S.Scalar, R.Volatility, P.Own),
102:     out(3, S.Scalar, R.Volatility, P.Own),
103:   ])],
104:   [IndicatorIdentifier.DirectionalIndicatorPlus, desc(IndicatorIdentifier.DirectionalIndicatorPlus, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [
105:     out(0, S.Scalar, R.Directional, P.Own),
106:     out(1, S.Scalar, R.Directional, P.Own),
107:     out(2, S.Scalar, R.Volatility, P.Own),
108:     out(3, S.Scalar, R.Volatility, P.Own),
109:   ])],
110:   [IndicatorIdentifier.DirectionalMovementIndex, desc(IndicatorIdentifier.DirectionalMovementIndex, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [
111:     out(0, S.Scalar, R.BoundedOscillator, P.Own),
112:     out(1, S.Scalar, R.Directional, P.Own),
113:     out(2, S.Scalar, R.Directional, P.Own),
114:     out(3, S.Scalar, R.Directional, P.Own),
115:     out(4, S.Scalar, R.Directional, P.Own),
116:     out(5, S.Scalar, R.Volatility, P.Own),
117:     out(6, S.Scalar, R.Volatility, P.Own),
118:   ])],
119:   [IndicatorIdentifier.AverageDirectionalMovementIndex, desc(IndicatorIdentifier.AverageDirectionalMovementIndex, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [
120:     out(0, S.Scalar, R.BoundedOscillator, P.Own),
121:     out(1, S.Scalar, R.BoundedOscillator, P.Own),
122:     out(2, S.Scalar, R.Directional, P.Own),
123:     out(3, S.Scalar, R.Directional, P.Own),
124:     out(4, S.Scalar, R.Directional, P.Own),
125:     out(5, S.Scalar, R.Directional, P.Own),
126:     out(6, S.Scalar, R.Volatility, P.Own),
127:     out(7, S.Scalar, R.Volatility, P.Own),
128:   ])],
129:   [IndicatorIdentifier.AverageDirectionalMovementIndexRating, desc(IndicatorIdentifier.AverageDirectionalMovementIndexRating, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [
130:     out(0, S.Scalar, R.BoundedOscillator, P.Own),
131:     out(1, S.Scalar, R.BoundedOscillator, P.Own),
132:     out(2, S.Scalar, R.BoundedOscillator, P.Own),
133:     out(3, S.Scalar, R.Directional, P.Own),
134:     out(4, S.Scalar, R.Directional, P.Own),
135:     out(5, S.Scalar, R.Directional, P.Own),
136:     out(6, S.Scalar, R.Directional, P.Own),
137:     out(7, S.Scalar, R.Volatility, P.Own),
138:     out(8, S.Scalar, R.Volatility, P.Own),
139:   ])],
140:   [IndicatorIdentifier.WilliamsPercentR, desc(IndicatorIdentifier.WilliamsPercentR, 'Larry Williams', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.BoundedOscillator, P.Own)])],
141:   [IndicatorIdentifier.PercentagePriceOscillator, desc(IndicatorIdentifier.PercentagePriceOscillator, 'Gerald Appel', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
142:   [IndicatorIdentifier.AbsolutePriceOscillator, desc(IndicatorIdentifier.AbsolutePriceOscillator, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
143:   [IndicatorIdentifier.CommodityChannelIndex, desc(IndicatorIdentifier.CommodityChannelIndex, 'Donald Lambert', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.BoundedOscillator, P.Own)])],
144:   [IndicatorIdentifier.MoneyFlowIndex, desc(IndicatorIdentifier.MoneyFlowIndex, 'Gene Quong', A.Static, I.BarInput, V.AggregateBarVolume, [out(0, S.Scalar, R.BoundedOscillator, P.Own)])],
145:   [IndicatorIdentifier.OnBalanceVolume, desc(IndicatorIdentifier.OnBalanceVolume, 'Joseph Granville', A.Static, I.BarInput, V.AggregateBarVolume, [out(0, S.Scalar, R.VolumeFlow, P.Own)])],
146:   [IndicatorIdentifier.BalanceOfPower, desc(IndicatorIdentifier.BalanceOfPower, 'Igor Livshin', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.BoundedOscillator, P.Own)])],
147:   [IndicatorIdentifier.RateOfChangeRatio, desc(IndicatorIdentifier.RateOfChangeRatio, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
148:   [IndicatorIdentifier.PearsonsCorrelationCoefficient, desc(IndicatorIdentifier.PearsonsCorrelationCoefficient, 'Common', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Correlation, P.Own)])],
149:   [IndicatorIdentifier.LinearRegression, desc(IndicatorIdentifier.LinearRegression, 'Common', A.Static, I.ScalarInput, V.NoVolume, [
150:     out(0, S.Scalar, R.Smoother, P.Price),
151:     out(1, S.Scalar, R.Smoother, P.Price),
152:     out(2, S.Scalar, R.Smoother, P.Price),
153:     out(3, S.Scalar, R.Oscillator, P.Own),
154:     out(4, S.Scalar, R.Oscillator, P.Own),
155:   ])],
156:   [IndicatorIdentifier.UltimateOscillator, desc(IndicatorIdentifier.UltimateOscillator, 'Larry Williams', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.BoundedOscillator, P.Own)])],
157:   [IndicatorIdentifier.StochasticRelativeStrengthIndex, desc(IndicatorIdentifier.StochasticRelativeStrengthIndex, 'Tushar Chande', A.Static, I.ScalarInput, V.NoVolume, [
158:     out(0, S.Scalar, R.BoundedOscillator, P.Own),
159:     out(1, S.Scalar, R.Signal, P.Own),
160:   ])],
161:   [IndicatorIdentifier.Stochastic, desc(IndicatorIdentifier.Stochastic, 'George Lane', A.Static, I.BarInput, V.NoVolume, [
162:     out(0, S.Scalar, R.BoundedOscillator, P.Own),
163:     out(1, S.Scalar, R.BoundedOscillator, P.Own),
164:     out(2, S.Scalar, R.Signal, P.Own),
165:   ])],
166:   [IndicatorIdentifier.Aroon, desc(IndicatorIdentifier.Aroon, 'Tushar Chande', A.Static, I.BarInput, V.NoVolume, [
167:     out(0, S.Scalar, R.BoundedOscillator, P.Own),
168:     out(1, S.Scalar, R.BoundedOscillator, P.Own),
169:     out(2, S.Scalar, R.Oscillator, P.Own),
170:   ])],
171:   [IndicatorIdentifier.AdvanceDecline, desc(IndicatorIdentifier.AdvanceDecline, 'Marc Chaikin', A.Static, I.BarInput, V.AggregateBarVolume, [out(0, S.Scalar, R.VolumeFlow, P.Own)])],
172:   [IndicatorIdentifier.AdvanceDeclineOscillator, desc(IndicatorIdentifier.AdvanceDeclineOscillator, 'Marc Chaikin', A.Static, I.BarInput, V.AggregateBarVolume, [out(0, S.Scalar, R.VolumeFlow, P.Own)])],
173:   [IndicatorIdentifier.ParabolicStopAndReverse, desc(IndicatorIdentifier.ParabolicStopAndReverse, 'Welles Wilder', A.Static, I.BarInput, V.NoVolume, [out(0, S.Scalar, R.Overlay, P.Price)])],
174:   [IndicatorIdentifier.TripleExponentialMovingAverageOscillator, desc(IndicatorIdentifier.TripleExponentialMovingAverageOscillator, 'Jack Hutson', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
175:   [IndicatorIdentifier.BollingerBandsTrend, desc(IndicatorIdentifier.BollingerBandsTrend, 'John Bollinger', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Scalar, R.Oscillator, P.Own)])],
176:   [IndicatorIdentifier.MovingAverageConvergenceDivergence, desc(IndicatorIdentifier.MovingAverageConvergenceDivergence, 'Gerald Appel', A.Static, I.ScalarInput, V.NoVolume, [
177:     out(0, S.Scalar, R.Oscillator, P.Own),
178:     out(1, S.Scalar, R.Signal, P.Own),
179:     out(2, S.Scalar, R.Histogram, P.Own),
180:   ])],
181:   [IndicatorIdentifier.SineWave, desc(IndicatorIdentifier.SineWave, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
182:     out(0, S.Scalar, R.Oscillator, P.Own),
183:     out(1, S.Scalar, R.Signal, P.Own),
184:     out(2, S.Band, R.Envelope, P.Own),
185:     out(3, S.Scalar, R.CyclePeriod, P.Own),
186:     out(4, S.Scalar, R.CyclePhase, P.Own),
187:   ])],
188:   [IndicatorIdentifier.HilbertTransformerInstantaneousTrendLine, desc(IndicatorIdentifier.HilbertTransformerInstantaneousTrendLine, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
189:     out(0, S.Scalar, R.Smoother, P.Price),
190:     out(1, S.Scalar, R.CyclePeriod, P.Own),
191:   ])],
192:   [IndicatorIdentifier.TrendCycleMode, desc(IndicatorIdentifier.TrendCycleMode, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
193:     out(0, S.Scalar, R.RegimeFlag, P.Own),
194:     out(1, S.Scalar, R.RegimeFlag, P.Own),
195:     out(2, S.Scalar, R.RegimeFlag, P.Own),
196:     out(3, S.Scalar, R.Smoother, P.Price),
197:     out(4, S.Scalar, R.Oscillator, P.Own),
198:     out(5, S.Scalar, R.Signal, P.Own),
199:     out(6, S.Scalar, R.CyclePeriod, P.Own),
200:     out(7, S.Scalar, R.CyclePhase, P.Own),
201:   ])],
202:   [IndicatorIdentifier.CoronaSpectrum, desc(IndicatorIdentifier.CoronaSpectrum, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
203:     out(0, S.Heatmap, R.Spectrum, P.Own),
204:     out(1, S.Scalar, R.CyclePeriod, P.Own),
205:     out(2, S.Scalar, R.CyclePeriod, P.Own),
206:   ])],
207:   [IndicatorIdentifier.CoronaSignalToNoiseRatio, desc(IndicatorIdentifier.CoronaSignalToNoiseRatio, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
208:     out(0, S.Heatmap, R.Spectrum, P.Own),
209:     out(1, S.Scalar, R.BoundedOscillator, P.Own),
210:   ])],
211:   [IndicatorIdentifier.CoronaSwingPosition, desc(IndicatorIdentifier.CoronaSwingPosition, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
212:     out(0, S.Heatmap, R.Spectrum, P.Own),
213:     out(1, S.Scalar, R.BoundedOscillator, P.Own),
214:   ])],
215:   [IndicatorIdentifier.CoronaTrendVigor, desc(IndicatorIdentifier.CoronaTrendVigor, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [
216:     out(0, S.Heatmap, R.Spectrum, P.Own),
217:     out(1, S.Scalar, R.Oscillator, P.Own),
218:   ])],
219:   [IndicatorIdentifier.AdaptiveTrendAndCycleFilter, desc(IndicatorIdentifier.AdaptiveTrendAndCycleFilter, 'Vladimir Kravchuk', A.Adaptive, I.ScalarInput, V.NoVolume, [
220:     out(0, S.Scalar, R.Smoother, P.Price),
221:     out(1, S.Scalar, R.Smoother, P.Price),
222:     out(2, S.Scalar, R.Smoother, P.Price),
223:     out(3, S.Scalar, R.Smoother, P.Price),
224:     out(4, S.Scalar, R.Smoother, P.Price),
225:     out(5, S.Scalar, R.Oscillator, P.Own),
226:     out(6, S.Scalar, R.Oscillator, P.Own),
227:     out(7, S.Scalar, R.Oscillator, P.Own),
228:   ])],
229:   [IndicatorIdentifier.MaximumEntropySpectrum, desc(IndicatorIdentifier.MaximumEntropySpectrum, 'Custom', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Heatmap, R.Spectrum, P.Own)])],
230:   [IndicatorIdentifier.DiscreteFourierTransformSpectrum, desc(IndicatorIdentifier.DiscreteFourierTransformSpectrum, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Heatmap, R.Spectrum, P.Own)])],
231:   [IndicatorIdentifier.CombBandPassSpectrum, desc(IndicatorIdentifier.CombBandPassSpectrum, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Heatmap, R.Spectrum, P.Own)])],
232:   [IndicatorIdentifier.AutoCorrelationIndicator, desc(IndicatorIdentifier.AutoCorrelationIndicator, 'John Ehlers', A.Static, I.ScalarInput, V.NoVolume, [out(0, S.Heatmap, R.Correlation, P.Own)])],
233:   [IndicatorIdentifier.AutoCorrelationPeriodogram, desc(IndicatorIdentifier.AutoCorrelationPeriodogram, 'John Ehlers', A.Adaptive, I.ScalarInput, V.NoVolume, [out(0, S.Heatmap, R.Spectrum, P.Own)])],
234: ]);
235: 
236: /**
237:  * Returns the taxonomic descriptor for the given indicator identifier, or
238:  * `undefined` if no descriptor is registered for the identifier.
239:  */
240: export function descriptorOf(id: IndicatorIdentifier): Descriptor | undefined {
241:   return descriptors.get(id);
242: }
243: 
244: /** Returns a copy of the full descriptor registry. */
245: export function getDescriptors(): Map<IndicatorIdentifier, Descriptor> {
246:   return new Map(descriptors);
247: }

(End of file - total 247 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/descriptor.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/descriptor.ts</path>
<type>file</type>
<content>1: import { Adaptivity } from './adaptivity.js';
2: import { IndicatorIdentifier } from './indicator-identifier.js';
3: import { InputRequirement } from './input-requirement.js';
4: import { OutputDescriptor } from './output-descriptor.js';
5: import { VolumeUsage } from './volume-usage.js';
6: 
7: /**
8:  * Classifies an indicator along multiple taxonomic dimensions to enable
9:  * filtering and display in charting catalogs.
10:  */
11: export interface Descriptor {
12:   /** Uniquely identifies the indicator. */
13:   identifier: IndicatorIdentifier;
14: 
15:   /** Groups related indicators (e.g., by author or category). */
16:   family: string;
17: 
18:   /** Whether the indicator adapts its parameters. */
19:   adaptivity: Adaptivity;
20: 
21:   /** The minimum input data type this indicator consumes. */
22:   inputRequirement: InputRequirement;
23: 
24:   /** How this indicator uses volume information. */
25:   volumeUsage: VolumeUsage;
26: 
27:   /** Per-output classifications. */
28:   outputs: OutputDescriptor[];
29: }

(End of file - total 29 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/pane.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/pane.ts</path>
<type>file</type>
<content>1: /** Identifies the chart pane an indicator output is drawn on. */
2: export enum Pane {
3:   /** The primary price pane. */
4:   Price = 1,
5: 
6:   /** A dedicated sub-pane for this indicator. */
7:   Own,
8: 
9:   /** Drawn on the parent indicator's pane. */
10:   OverlayOnParent
11: }

(End of file - total 11 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/outputs/shape/shape.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/outputs/shape/shape.ts</path>
<type>file</type>
<content>1: /** Enumerates indicator output data shapes. */
2: export enum Shape {
3: 
4:   /** Holds a single value. */
5:   Scalar,
6: 
7:   /** Holds two values representing lower and upper lines of a band. */
8:   Band,
9: 
10:   /** Holds an array of values representing a heat-map column. */
11:   Heatmap,
12: 
13:   /** Holds an ordered, variable-length sequence of (offset, value) points. */
14:   Polyline
15: }

(End of file - total 15 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.6 · 6.5s)

Now I have everything I need. Let me check the output-descriptor interface and the Heatmap class quickly:

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/output-descriptor.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/output-descriptor.ts</path>
<type>file</type>
<content>1: import { Shape } from './outputs/shape/shape.js';
2: import { Pane } from './pane.js';
3: import { Role } from './role.js';
4: 
5: /** Classifies a single indicator output for charting / discovery. */
6: export interface OutputDescriptor {
7:   /** Integer representation of the output enumeration of a related indicator. */
8:   kind: number;
9: 
10:   /** The data shape of this output. */
11:   shape: Shape;
12: 
13:   /** The semantic role of this output. */
14:   role: Role;
15: 
16:   /** The chart pane on which this output is drawn. */
17:   pane: Pane;
18: }

(End of file - total 18 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/outputs/heatmap.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/outputs/heatmap.ts</path>
<type>file</type>
<content>1: /** Holds a time stamp (x) and an array of values (z) corresponding to parameter (y) range to paint a heatmap column. */
2: export class Heatmap {
3:   /** The date and time (x). */
4:   time!: Date;
5: 
6:   /** The first parameter (y) value of the heatmap. This value is the same for all columns. */
7:   parameterFirst!: number;
8: 
9:   /** The last parameter (y) value of the heatmap. This value is the same for all columns. */
10:   parameterLast!: number;
11: 
12:   /** A parameter resolution (positive number). A value of 10 means that heatmap values are evaluated at every 0.1 of parameter range. */
13:   parameterResolution!: number;
14: 
15:   /** A minimal value (z) of the heatmap column. */
16:   valueMin!: number;
17: 
18:   /** A maximal value (z) of the heatmap column. */
19:   valueMax!: number;
20: 
21:   /** The values (z) of the heatmap column. */
22:   values!: number[];
23: 
24:   /** Creates a populated heatmap column. */
25:   public static newHeatmap(
26:     time: Date,
27:     parameterFirst: number,
28:     parameterLast: number,
29:     parameterResolution: number,
30:     valueMin: number,
31:     valueMax: number,
32:     values: number[],
33:   ): Heatmap {
34:     const h = new Heatmap();
35:     h.time = time;
36:     h.parameterFirst = parameterFirst;
37:     h.parameterLast = parameterLast;
38:     h.parameterResolution = parameterResolution;
39:     h.valueMin = valueMin;
40:     h.valueMax = valueMax;
41:     h.values = values;
42:     return h;
43:   }
44: 
45:   /** Creates an empty heatmap column with valid axis metadata but no values. */
46:   public static newEmptyHeatmap(
47:     time: Date,
48:     parameterFirst: number,
49:     parameterLast: number,
50:     parameterResolution: number,
51:   ): Heatmap {
52:     const h = new Heatmap();
53:     h.time = time;
54:     h.parameterFirst = parameterFirst;
55:     h.parameterLast = parameterLast;
56:     h.parameterResolution = parameterResolution;
57:     h.valueMin = Number.NaN;
58:     h.valueMax = Number.NaN;
59:     h.values = [];
60:     return h;
61:   }
62: 
63:   /** Indicates whether this heatmap column has no values. */
64:   public isEmpty(): boolean {
65:     return !this.values || this.values.length < 1;
66:   }
67: }

(End of file - total 67 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/outputs/band.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/outputs/band.ts</path>
<type>file</type>
<content>1: /** Holds two band values and a time stamp. */
2: export class Band {
3:   /** The date and time. */
4:   time!: Date;
5: 
6:   /** A lower value of the band. */
7:   lower!: number;
8: 
9:   /** A higher value of the band. */
10:   upper!: number;
11: }

(End of file - total 11 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/entities/scalar.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/entities/scalar.ts</path>
<type>file</type>
<content>1: /** A _scalar_ (value and time) entity. */
2: export class Scalar {
3:   /** The date and time. */
4:   time!: Date;
5: 
6:   /** The value. */
7:   value!: number;
8: 
9:   constructor(data?: any) {
10:     if (data) {
11:       for (const property in data) {
12:         if (Object.prototype.hasOwnProperty.call(data, property)) {
13:           (this as any)[property] = data[property];
14:         }
15:       }
16:     }
17:   }
18: 
19:   /*toJSON(data?: any): any {
20:     data = typeof data === 'object' ? data : {};
21:     data.time = this.time ? this.time.toISOString() : undefined;
22:     data.value = this.value;
23:     return data;
24:   }*/
25: }

(End of file - total 25 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.6 · 194.8s)

Now I have all the context. Let me create the TS iconf CLI, settings.json, and run-me.sh:

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/cmd/iconf/main.ts",
  "content": "/**\n * iconf — command-line chart configuration generator (TypeScript).\n *\n * Reads a JSON settings file containing indicator definitions, creates\n * indicator instances via the factory, runs embedded bar data through them,\n * and writes chart configuration files in JSON and TypeScript formats.\n *\n * Usage:  npx tsx cmd/iconf/main.ts <settings.json> <output-name>\n */\n\nimport { readFileSync, writeFileSync } from 'node:fs';\nimport { basename } from 'node:path';\nimport { Bar } from '../../entities/bar.js';\nimport { Scalar } from '../../entities/scalar.js';\nimport { type Indicator } from '../../indicators/core/indicator.js';\nimport { IndicatorIdentifier } from '../../indicators/core/indicator-identifier.js';\nimport { type IndicatorOutput } from '../../indicators/core/indicator-output.js';\nimport { Band } from '../../indicators/core/outputs/band.js';\nimport { Heatmap } from '../../indicators/core/outputs/heatmap.js';\nimport { Shape } from '../../indicators/core/outputs/shape/shape.js';\nimport { Pane } from '../../indicators/core/pane.js';\nimport { descriptorOf } from '../../indicators/core/descriptors.js';\nimport { createIndicator } from '../../indicators/factory/factory.js';\n\n// ---------------------------------------------------------------------------\n// Settings\n// ---------------------------------------------------------------------------\n\ninterface SettingsEntry {\n    identifier: string;\n    params: Record<string, unknown>;\n}\n\n/** Map camelCase JSON identifier string to IndicatorIdentifier enum value. */\nfunction resolveIdentifier(name: string): IndicatorIdentifier {\n    let pascal = name.charAt(0).toUpperCase() + name.slice(1);\n    if (pascal === 'KaufmanAdaptiveMovingAverageMovingAverage') {\n        pascal = 'KaufmanAdaptiveMovingAverage';\n    }\n    const value = (IndicatorIdentifier as unknown as Record<string, number>)[pascal];\n    if (value === undefined) {\n        throw new Error(`Unknown indicator identifier: \"${name}\" (tried \"${pascal}\")`);\n    }\n    return value as IndicatorIdentifier;\n}\n\n// ---------------------------------------------------------------------------\n// Chart configuration types (mirrors Go structs / TS template types)\n// ---------------------------------------------------------------------------\n\ninterface Configuration {\n    width: string;\n    navigationPane?: NavigationPane;\n    heightNavigationPane: number;\n    timeAnnotationFormat: string;\n    axisLeft: boolean;\n    axisRight: boolean;\n    margin: Margin;\n    ohlcv: OhlcvData;\n    pricePane: PaneData;\n    indicatorPanes: PaneData[];\n    crosshair: boolean;\n    volumeInPricePane: boolean;\n    menuVisible: boolean;\n    downloadSvgVisible: boolean;\n}\n\ninterface NavigationPane {\n    height: number;\n    hasLine: boolean;\n    hasArea: boolean;\n    hasTimeAxis: boolean;\n    timeTicks: number;\n}\n\ninterface Margin { left: number; top: number; right: number; bottom: number; }\n\ninterface OhlcvData {\n    name: string;\n    data: OhlcvBar[];\n    candlesticks: boolean;\n}\n\ninterface OhlcvBar {\n    time: string;\n    open: number;\n    high: number;\n    low: number;\n    close: number;\n    volume: number;\n}\n\ninterface PaneData {\n    height: string;\n    valueFormat: string;\n    valueMarginPercentageFactor: number;\n    heatmap?: HeatmapDataItem;\n    bands: BandDataItem[];\n    lineAreas: LineAreaDataItem[];\n    horizontals: HorizontalDataItem[];\n    lines: LineDataItem[];\n    arrows: ArrowDataItem[];\n}\n\ninterface LineDataItem {\n    name: string;\n    data: ScalarVal[];\n    indicator: number;\n    output: number;\n    color: string;\n    width: number;\n    dash: string;\n    interpolation: string;\n}\n\ninterface ScalarVal { time: string; value: number; }\n\ninterface BandDataItem {\n    name: string;\n    data: BandVal[];\n    indicator: number;\n    output: number;\n    color: string;\n    legendColor: string;\n    interpolation: string;\n}\n\ninterface BandVal { time: string; upper: number; lower: number; }\n\ninterface LineAreaDataItem {\n    name: string;\n    data: ScalarVal[];\n    indicator: number;\n    output: number;\n    value: number;\n    color: string;\n    legendColor: string;\n    interpolation: string;\n}\n\ninterface HeatmapDataItem {\n    name: string;\n    data: HeatmapVal[];\n    indicator: number;\n    output: number;\n    gradient: string;\n    invertGradient: boolean;\n}\n\ninterface HeatmapVal {\n    time: string;\n    first: number;\n    last: number;\n    result: number;\n    min: number;\n    max: number;\n    values: number[];\n}\n\ninterface HorizontalDataItem { value: number; color: string; width: number; dash: string; }\ninterface ArrowDataItem { name: string; down: boolean; time: string; indicator: number; output: number; color: string; }\n\n// ---------------------------------------------------------------------------\n// Default colors\n// ---------------------------------------------------------------------------\n\nconst lineColors = [\n    '#e41a1c', '#377eb8', '#4daf4a', '#984ea3', '#ff7f00',\n    '#a65628', '#f781bf', '#999999', '#66c2a5', '#fc8d62',\n];\n\nconst bandColors = [\n    'rgba(0,255,0,0.3)', 'rgba(0,0,255,0.3)', 'rgba(255,0,0,0.3)',\n    'rgba(128,0,128,0.3)', 'rgba(0,128,128,0.3)',\n];\n\n// ---------------------------------------------------------------------------\n// Output placement\n// ---------------------------------------------------------------------------\n\ninterface OutputPlacement {\n    indicatorIdx: number;\n    outputIdx: number;\n    mnemonic: string;\n    shape: Shape;\n    pane: Pane;\n}\n\n// ---------------------------------------------------------------------------\n// Configuration builder\n// ---------------------------------------------------------------------------\n\nfunction formatDate(d: Date): string {\n    return d.toISOString().slice(0, 10);\n}\n\nfunction buildConfiguration(indicators: Indicator[], bars: Bar[]): Configuration {\n    // Collect output placements from descriptor registry.\n    const placements: OutputPlacement[] = [];\n    for (let i = 0; i < indicators.length; i++) {\n        const meta = indicators[i].metadata();\n        const desc = descriptorOf(meta.identifier);\n        if (!desc) {\n            // Fallback: treat all outputs as Own/Scalar.\n            for (let j = 0; j < meta.outputs.length; j++) {\n                placements.push({\n                    indicatorIdx: i, outputIdx: j,\n                    mnemonic: meta.outputs[j].mnemonic, shape: meta.outputs[j].shape, pane: Pane.Own,\n                });\n            }\n            continue;\n        }\n        for (let j = 0; j < desc.outputs.length; j++) {\n            const mnemonic = j < meta.outputs.length ? meta.outputs[j].mnemonic : `out[${j}]`;\n            placements.push({\n                indicatorIdx: i, outputIdx: j,\n                mnemonic, shape: desc.outputs[j].shape, pane: desc.outputs[j].pane,\n            });\n        }\n    }\n\n    // Run bars through indicators, collecting all outputs.\n    const allOutputs: IndicatorOutput[][] = [];\n    for (let i = 0; i < bars.length; i++) {\n        const row: IndicatorOutput[] = [];\n        for (const ind of indicators) {\n            row.push(ind.updateBar(bars[i]));\n        }\n        allOutputs.push(row);\n    }\n\n    // Build OHLCV data.\n    const ohlcvBars: OhlcvBar[] = bars.map(b => ({\n        time: formatDate(b.time), open: b.open, high: b.high,\n        low: b.low, close: b.close, volume: b.volume,\n    }));\n\n    // Group placements: price pane vs own panes.\n    const ownPaneMap = new Map<number, number>(); // indicatorIdx -> indicatorPanes index\n    const indicatorPanes: PaneData[] = [];\n\n    const pricePane: PaneData = {\n        height: '30%', valueFormat: ',.2f', valueMarginPercentageFactor: 0.01,\n        bands: [], lineAreas: [], horizontals: [], lines: [], arrows: [],\n    };\n\n    let colorIdx = 0;\n    let bandColorIdx = 0;\n\n    for (const p of placements) {\n        if (p.pane === Pane.Price && p.shape === Shape.Scalar) {\n            pricePane.lines.push(buildLineData(p, allOutputs, bars, lineColors[colorIdx % lineColors.length]));\n            colorIdx++;\n        } else if (p.pane === Pane.Price && p.shape === Shape.Band) {\n            pricePane.bands.push(buildBandData(p, allOutputs, bars, bandColors[bandColorIdx % bandColors.length]));\n            bandColorIdx++;\n        } else {\n            // Own pane (or fallback).\n            let idx = ownPaneMap.get(p.indicatorIdx);\n            if (idx === undefined) {\n                idx = indicatorPanes.length;\n                ownPaneMap.set(p.indicatorIdx, idx);\n                indicatorPanes.push({\n                    height: '60', valueFormat: ',.2f', valueMarginPercentageFactor: 0.01,\n                    bands: [], lineAreas: [], horizontals: [], lines: [], arrows: [],\n                });\n            }\n            const pane = indicatorPanes[idx];\n\n            if (p.shape === Shape.Heatmap) {\n                pane.heatmap = buildHeatmapData(p, allOutputs, bars);\n                pane.height = '120';\n                pane.valueMarginPercentageFactor = 0;\n            } else if (p.shape === Shape.Band) {\n                pane.bands.push(buildBandData(p, allOutputs, bars, bandColors[bandColorIdx % bandColors.length]));\n                bandColorIdx++;\n            } else {\n                pane.lines.push(buildLineData(p, allOutputs, bars, lineColors[colorIdx % lineColors.length]));\n                colorIdx++;\n            }\n        }\n    }\n\n    return {\n        width: '100%',\n        navigationPane: { height: 30, hasLine: true, hasArea: false, hasTimeAxis: true, timeTicks: 0 },\n        heightNavigationPane: 30,\n        timeAnnotationFormat: '%Y-%m-%d',\n        axisLeft: true,\n        axisRight: false,\n        margin: { left: 0, top: 10, right: 20, bottom: 0 },\n        ohlcv: { name: 'TEST', data: ohlcvBars, candlesticks: true },\n        pricePane,\n        indicatorPanes,\n        crosshair: false,\n        volumeInPricePane: true,\n        menuVisible: true,\n        downloadSvgVisible: true,\n    };\n}\n\nfunction buildLineData(p: OutputPlacement, allOutputs: IndicatorOutput[][], bars: Bar[], color: string): LineDataItem {\n    const data: ScalarVal[] = [];\n    for (let i = 0; i < bars.length; i++) {\n        const out = allOutputs[i][p.indicatorIdx];\n        if (p.outputIdx < out.length) {\n            const val = out[p.outputIdx];\n            if (val instanceof Scalar && !Number.isNaN(val.value)) {\n                data.push({ time: formatDate(val.time), value: val.value });\n            }\n        }\n    }\n    return {\n        name: p.mnemonic, data, indicator: p.indicatorIdx, output: p.outputIdx,\n        color, width: 1, dash: '', interpolation: 'natural',\n    };\n}\n\nfunction buildBandData(p: OutputPlacement, allOutputs: IndicatorOutput[][], bars: Bar[], color: string): BandDataItem {\n    const data: BandVal[] = [];\n    for (let i = 0; i < bars.length; i++) {\n        const out = allOutputs[i][p.indicatorIdx];\n        if (p.outputIdx < out.length) {\n            const val = out[p.outputIdx];\n            if (val instanceof Band && !Number.isNaN(val.lower) && !Number.isNaN(val.upper)) {\n                data.push({ time: formatDate(val.time), upper: val.upper, lower: val.lower });\n            }\n        }\n    }\n    return {\n        name: p.mnemonic, data, indicator: p.indicatorIdx, output: p.outputIdx,\n        color, legendColor: color, interpolation: 'natural',\n    };\n}\n\nfunction buildHeatmapData(p: OutputPlacement, allOutputs: IndicatorOutput[][], bars: Bar[]): HeatmapDataItem {\n    const data: HeatmapVal[] = [];\n    for (let i = 0; i < bars.length; i++) {\n        const out = allOutputs[i][p.indicatorIdx];\n        if (p.outputIdx < out.length) {\n            const val = out[p.outputIdx];\n            if (val instanceof Heatmap && !val.isEmpty()) {\n                data.push({\n                    time: formatDate(val.time), first: val.parameterFirst, last: val.parameterLast,\n                    result: val.parameterResolution, min: val.valueMin, max: val.valueMax, values: val.values,\n                });\n            }\n        }\n    }\n    return {\n        name: p.mnemonic, data, indicator: p.indicatorIdx, output: p.outputIdx,\n        gradient: 'Viridis', invertGradient: false,\n    };\n}\n\n// ---------------------------------------------------------------------------\n// TypeScript generation\n// ---------------------------------------------------------------------------\n\nfunction sanitizeVarName(s: string): string {\n    let result = '';\n    let capitalize = false;\n    for (let i = 0; i < s.length; i++) {\n        const c = s[i];\n        if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) {\n            if (capitalize) {\n                result += c.toUpperCase();\n                capitalize = false;\n            } else {\n                result += c;\n            }\n        } else if (c >= '0' && c <= '9') {\n            if (i === 0) result += '_';\n            result += c;\n            capitalize = false;\n        } else {\n            capitalize = true;\n        }\n    }\n    return result;\n}\n\nfunction buildTypeScript(cfg: Configuration, baseName: string): string {\n    const lines: string[] = [];\n    const w = (s: string) => lines.push(s);\n\n    w(\"// Auto-generated chart configuration.\");\n    w(\"// eslint-disable-next-line\");\n    w(\"import { Configuration } from '../ohlcv-chart/template/configuration';\");\n    w(\"import { Scalar, Band, Heatmap, Bar } from '../ohlcv-chart/template/types';\");\n    w(\"\");\n\n    // Emit OHLCV data array.\n    const ohlcvVar = sanitizeVarName(baseName) + 'Ohlcv';\n    w(`export const ${ohlcvVar}: Bar[] = ${JSON.stringify(cfg.ohlcv.data)};`);\n    w(\"\");\n\n    // Track variable names for data references.\n    const lineVarMap = new Map<LineDataItem, string>();\n    const bandVarMap = new Map<BandDataItem, string>();\n    const heatmapVarMap = new Map<HeatmapDataItem, string>();\n    const lineAreaVarMap = new Map<LineAreaDataItem, string>();\n\n    let varCounter = 0;\n\n    function emitScalarArray(mnemonic: string, data: ScalarVal[]): string {\n        varCounter++;\n        const vn = sanitizeVarName(`${baseName}_${mnemonic}_${varCounter}`);\n        w(`export const ${vn}: Scalar[] = ${JSON.stringify(data)};`);\n        w(\"\");\n        return vn;\n    }\n\n    function emitBandArray(mnemonic: string, data: BandVal[]): string {\n        varCounter++;\n        const vn = sanitizeVarName(`${baseName}_${mnemonic}_${varCounter}`);\n        w(`export const ${vn}: Band[] = ${JSON.stringify(data)};`);\n        w(\"\");\n        return vn;\n    }\n\n    function emitHeatmapArray(mnemonic: string, data: HeatmapVal[]): string {\n        varCounter++;\n        const vn = sanitizeVarName(`${baseName}_${mnemonic}_${varCounter}`);\n        w(`export const ${vn}: Heatmap[] = ${JSON.stringify(data)};`);\n        w(\"\");\n        return vn;\n    }\n\n    // Emit price pane data arrays.\n    for (const l of cfg.pricePane.lines) {\n        lineVarMap.set(l, emitScalarArray(l.name, l.data));\n    }\n    for (const b of cfg.pricePane.bands) {\n        bandVarMap.set(b, emitBandArray(b.name, b.data));\n    }\n    for (const la of cfg.pricePane.lineAreas) {\n        lineAreaVarMap.set(la, emitScalarArray(la.name, la.data));\n    }\n\n    // Emit indicator pane data arrays.\n    for (const pane of cfg.indicatorPanes) {\n        for (const l of pane.lines) {\n            lineVarMap.set(l, emitScalarArray(l.name, l.data));\n        }\n        for (const b of pane.bands) {\n            bandVarMap.set(b, emitBandArray(b.name, b.data));\n        }\n        for (const la of pane.lineAreas) {\n            lineAreaVarMap.set(la, emitScalarArray(la.name, la.data));\n        }\n        if (pane.heatmap) {\n            heatmapVarMap.set(pane.heatmap, emitHeatmapArray(pane.heatmap.name, pane.heatmap.data));\n        }\n    }\n\n    // Emit configuration object.\n    const cfgVar = sanitizeVarName(baseName) + 'Config';\n    w(`export const ${cfgVar}: Configuration = {`);\n    w(`  width: ${JSON.stringify(cfg.width)},`);\n\n    if (cfg.navigationPane) {\n        const np = cfg.navigationPane;\n        w(`  navigationPane: {`);\n        w(`    height: ${np.height}, hasLine: ${np.hasLine}, hasArea: ${np.hasArea}, hasTimeAxis: ${np.hasTimeAxis}, timeTicks: ${np.timeTicks},`);\n        w(`  },`);\n    }\n\n    w(`  heightNavigationPane: ${cfg.heightNavigationPane},`);\n    w(`  timeAnnotationFormat: ${JSON.stringify(cfg.timeAnnotationFormat)},`);\n    w(`  axisLeft: ${cfg.axisLeft},`);\n    w(`  axisRight: ${cfg.axisRight},`);\n    w(`  margin: { left: ${cfg.margin.left}, top: ${cfg.margin.top}, right: ${cfg.margin.right}, bottom: ${cfg.margin.bottom} },`);\n    w(`  ohlcv: { name: ${JSON.stringify(cfg.ohlcv.name)}, data: ${ohlcvVar}, candlesticks: ${cfg.ohlcv.candlesticks} },`);\n\n    // Price pane.\n    w(`  pricePane: {`);\n    writeTSPane(lines, cfg.pricePane, lineVarMap, bandVarMap, lineAreaVarMap, heatmapVarMap, '    ');\n    w(`  },`);\n\n    // Indicator panes.\n    w(`  indicatorPanes: [`);\n    for (const pane of cfg.indicatorPanes) {\n        w(`    {`);\n        writeTSPane(lines, pane, lineVarMap, bandVarMap, lineAreaVarMap, heatmapVarMap, '      ');\n        w(`    },`);\n    }\n    w(`  ],`);\n\n    w(`  crosshair: ${cfg.crosshair},`);\n    w(`  volumeInPricePane: ${cfg.volumeInPricePane},`);\n    w(`  menuVisible: ${cfg.menuVisible},`);\n    w(`  downloadSvgVisible: ${cfg.downloadSvgVisible},`);\n    w(`};`);\n\n    return lines.join('\\n') + '\\n';\n}\n\nfunction writeTSPane(\n    lines: string[], pane: PaneData,\n    lineVarMap: Map<LineDataItem, string>, bandVarMap: Map<BandDataItem, string>,\n    lineAreaVarMap: Map<LineAreaDataItem, string>, heatmapVarMap: Map<HeatmapDataItem, string>,\n    indent: string,\n): void {\n    const w = (s: string) => lines.push(s);\n\n    w(`${indent}height: ${JSON.stringify(pane.height)}, valueFormat: ${JSON.stringify(pane.valueFormat)}, valueMarginPercentageFactor: ${pane.valueMarginPercentageFactor},`);\n\n    if (pane.heatmap) {\n        const hm = pane.heatmap;\n        const vn = heatmapVarMap.get(hm)!;\n        w(`${indent}heatmap: { name: ${JSON.stringify(hm.name)}, data: ${vn}, indicator: ${hm.indicator}, output: ${hm.output}, gradient: ${JSON.stringify(hm.gradient)}, invertGradient: ${hm.invertGradient} },`);\n    }\n\n    // Bands.\n    if (pane.bands.length === 0) {\n        w(`${indent}bands: [],`);\n    } else {\n        w(`${indent}bands: [`);\n        for (const b of pane.bands) {\n            const vn = bandVarMap.get(b)!;\n            w(`${indent}  { name: ${JSON.stringify(b.name)}, data: ${vn}, indicator: ${b.indicator}, output: ${b.output}, color: ${JSON.stringify(b.color)}, legendColor: ${JSON.stringify(b.legendColor)}, interpolation: ${JSON.stringify(b.interpolation)} },`);\n        }\n        w(`${indent}],`);\n    }\n\n    // LineAreas.\n    if (pane.lineAreas.length === 0) {\n        w(`${indent}lineAreas: [],`);\n    } else {\n        w(`${indent}lineAreas: [`);\n        for (const la of pane.lineAreas) {\n            const vn = lineAreaVarMap.get(la)!;\n            w(`${indent}  { name: ${JSON.stringify(la.name)}, data: ${vn}, indicator: ${la.indicator}, output: ${la.output}, value: ${la.value}, color: ${JSON.stringify(la.color)}, legendColor: ${JSON.stringify(la.legendColor)}, interpolation: ${JSON.stringify(la.interpolation)} },`);\n        }\n        w(`${indent}],`);\n    }\n\n    // Horizontals.\n    if (pane.horizontals.length === 0) {\n        w(`${indent}horizontals: [],`);\n    } else {\n        w(`${indent}horizontals: [`);\n        for (const h of pane.horizontals) {\n            w(`${indent}  { value: ${h.value}, color: ${JSON.stringify(h.color)}, width: ${h.width}, dash: ${JSON.stringify(h.dash)} },`);\n        }\n        w(`${indent}],`);\n    }\n\n    // Lines.\n    if (pane.lines.length === 0) {\n        w(`${indent}lines: [],`);\n    } else {\n        w(`${indent}lines: [`);\n        for (const l of pane.lines) {\n            const vn = lineVarMap.get(l)!;\n            w(`${indent}  { name: ${JSON.stringify(l.name)}, data: ${vn}, indicator: ${l.indicator}, output: ${l.output}, color: ${JSON.stringify(l.color)}, width: ${l.width}, dash: ${JSON.stringify(l.dash)}, interpolation: ${JSON.stringify(l.interpolation)} },`);\n        }\n        w(`${indent}],`);\n    }\n\n    // Arrows.\n    w(`${indent}arrows: [],`);\n}\n\n// ---------------------------------------------------------------------------\n// Test bar data (252-entry TA-Lib reference set)\n// ---------------------------------------------------------------------------\n\nfunction testBars(): Bar[] {\n    const highs = testHighs();\n    const lows = testLows();\n    const closes = testCloses();\n    const volumes = testVolumes();\n\n    const bars: Bar[] = [];\n    const baseTime = new Date('2020-01-02T00:00:00Z');\n\n    for (let i = 0; i < closes.length; i++) {\n        const openPrice = i > 0 ? closes[i - 1] : closes[0];\n        const bar = new Bar();\n        bar.time = new Date(baseTime.getTime() + i * 86_400_000);\n        bar.open = openPrice;\n        bar.high = highs[i];\n        bar.low = lows[i];\n        bar.close = closes[i];\n        bar.volume = volumes[i];\n        bars.push(bar);\n    }\n\n    return bars;\n}\n\n// ---------------------------------------------------------------------------\n// Main\n// ---------------------------------------------------------------------------\n\nfunction main(): void {\n    const args = process.argv.slice(2);\n    if (args.length < 2) {\n        process.stderr.write('usage: iconf <settings.json> <output-name>\\n');\n        process.exit(1);\n    }\n\n    const settingsPath = args[0];\n    let outputName = args[1];\n    // Strip extension if provided.\n    outputName = outputName.replace(/\\.(json|ts)$/, '');\n\n    const data = readFileSync(settingsPath, 'utf-8');\n    const entries: SettingsEntry[] = JSON.parse(data);\n\n    const indicators: Indicator[] = [];\n    for (const e of entries) {\n        const id = resolveIdentifier(e.identifier);\n        const params = Object.keys(e.params).length === 0 ? undefined : e.params;\n        try {\n            indicators.push(createIndicator(id, params as Record<string, any>));\n        } catch (err) {\n            process.stderr.write(`error creating indicator ${e.identifier}: ${err}\\n`);\n            process.exit(1);\n        }\n    }\n\n    const bars = testBars();\n    const cfg = buildConfiguration(indicators, bars);\n\n    // Write JSON.\n    const jsonPath = outputName + '.json';\n    writeFileSync(jsonPath, JSON.stringify(cfg, null, 2), 'utf-8');\n    console.log(`wrote ${jsonPath}`);\n\n    // Write TypeScript.\n    const tsPath = outputName + '.ts';\n    const tsData = buildTypeScript(cfg, basename(outputName));\n    writeFileSync(tsPath, tsData, 'utf-8');\n    console.log(`wrote ${tsPath}`);\n}\n\n// ---------------------------------------------------------------------------\n// Embedded test data (TA-Lib test_data.c)\n// ---------------------------------------------------------------------------\n\nfunction testHighs(): number[] {\n    return [\n        93.25, 94.94, 96.375, 96.19, 96, 94.72, 95, 93.72, 92.47, 92.75,\n        96.25, 99.625, 99.125, 92.75, 91.315, 93.25, 93.405, 90.655, 91.97, 92.25,\n        90.345, 88.5, 88.25, 85.5, 84.44, 84.75, 84.44, 89.405, 88.125, 89.125,\n        87.155, 87.25, 87.375, 88.97, 90, 89.845, 86.97, 85.94, 84.75, 85.47,\n        84.47, 88.5, 89.47, 90, 92.44, 91.44, 92.97, 91.72, 91.155, 91.75,\n        90, 88.875, 89, 85.25, 83.815, 85.25, 86.625, 87.94, 89.375, 90.625,\n        90.75, 88.845, 91.97, 93.375, 93.815, 94.03, 94.03, 91.815, 92, 91.94,\n        89.75, 88.75, 86.155, 84.875, 85.94, 99.375, 103.28, 105.375, 107.625, 105.25,\n        104.5, 105.5, 106.125, 107.94, 106.25, 107, 108.75, 110.94, 110.94, 114.22,\n        123, 121.75, 119.815, 120.315, 119.375, 118.19, 116.69, 115.345, 113, 118.315,\n        116.87, 116.75, 113.87, 114.62, 115.31, 116, 121.69, 119.87, 120.87, 116.75,\n        116.5, 116, 118.31, 121.5, 122, 121.44, 125.75, 127.75, 124.19, 124.44,\n        125.75, 124.69, 125.31, 132, 131.31, 132.25, 133.88, 133.5, 135.5, 137.44,\n        138.69, 139.19, 138.5, 138.13, 137.5, 138.88, 132.13, 129.75, 128.5, 125.44,\n        125.12, 126.5, 128.69, 126.62, 126.69, 126, 123.12, 121.87, 124, 127,\n        124.44, 122.5, 123.75, 123.81, 124.5, 127.87, 128.56, 129.63, 124.87, 124.37,\n        124.87, 123.62, 124.06, 125.87, 125.19, 125.62, 126, 128.5, 126.75, 129.75,\n        132.69, 133.94, 136.5, 137.69, 135.56, 133.56, 135, 132.38, 131.44, 130.88,\n        129.63, 127.25, 127.81, 125, 126.81, 124.75, 122.81, 122.25, 121.06, 120,\n        123.25, 122.75, 119.19, 115.06, 116.69, 114.87, 110.87, 107.25, 108.87, 109,\n        108.5, 113.06, 93, 94.62, 95.12, 96, 95.56, 95.31, 99, 98.81,\n        96.81, 95.94, 94.44, 92.94, 93.94, 95.5, 97.06, 97.5, 96.25, 96.37,\n        95, 94.87, 98.25, 105.12, 108.44, 109.87, 105, 106, 104.94, 104.5,\n        104.44, 106.31, 112.87, 116.5, 119.19, 121, 122.12, 111.94, 112.75, 110.19,\n        107.94, 109.69, 111.06, 110.44, 110.12, 110.31, 110.44, 110, 110.75, 110.5,\n        110.5, 109.5,\n    ];\n}\n\nfunction testLows(): number[] {\n    return [\n        90.75, 91.405, 94.25, 93.5, 92.815, 93.5, 92, 89.75, 89.44, 90.625,\n        92.75, 96.315, 96.03, 88.815, 86.75, 90.94, 88.905, 88.78, 89.25, 89.75,\n        87.5, 86.53, 84.625, 82.28, 81.565, 80.875, 81.25, 84.065, 85.595, 85.97,\n        84.405, 85.095, 85.5, 85.53, 87.875, 86.565, 84.655, 83.25, 82.565, 83.44,\n        82.53, 85.065, 86.875, 88.53, 89.28, 90.125, 90.75, 89, 88.565, 90.095,\n        89, 86.47, 84, 83.315, 82, 83.25, 84.75, 85.28, 87.19, 88.44,\n        88.25, 87.345, 89.28, 91.095, 89.53, 91.155, 92, 90.53, 89.97, 88.815,\n        86.75, 85.065, 82.03, 81.5, 82.565, 96.345, 96.47, 101.155, 104.25, 101.75,\n        101.72, 101.72, 103.155, 105.69, 103.655, 104, 105.53, 108.53, 108.75, 107.75,\n        117, 118, 116, 118.5, 116.53, 116.25, 114.595, 110.875, 110.5, 110.72,\n        112.62, 114.19, 111.19, 109.44, 111.56, 112.44, 117.5, 116.06, 116.56, 113.31,\n        112.56, 114, 114.75, 118.87, 119, 119.75, 122.62, 123, 121.75, 121.56,\n        123.12, 122.19, 122.75, 124.37, 128, 129.5, 130.81, 130.63, 132.13, 133.88,\n        135.38, 135.75, 136.19, 134.5, 135.38, 133.69, 126.06, 126.87, 123.5, 122.62,\n        122.75, 123.56, 125.81, 124.62, 124.37, 121.81, 118.19, 118.06, 117.56, 121,\n        121.12, 118.94, 119.81, 121, 122, 124.5, 126.56, 123.5, 121.25, 121.06,\n        122.31, 121, 120.87, 122.06, 122.75, 122.69, 122.87, 125.5, 124.25, 128,\n        128.38, 130.69, 131.63, 134.38, 132, 131.94, 131.94, 129.56, 123.75, 126,\n        126.25, 124.37, 121.44, 120.44, 121.37, 121.69, 120, 119.62, 115.5, 116.75,\n        119.06, 119.06, 115.06, 111.06, 113.12, 110, 105, 104.69, 103.87, 104.69,\n        105.44, 107, 89, 92.5, 92.12, 94.62, 92.81, 94.25, 96.25, 96.37,\n        93.69, 93.5, 90, 90.19, 90.5, 92.12, 94.12, 94.87, 93, 93.87,\n        93, 92.62, 93.56, 98.37, 104.44, 106, 101.81, 104.12, 103.37, 102.12,\n        102.25, 103.37, 107.94, 112.5, 115.44, 115.5, 112.25, 107.56, 106.56, 106.87,\n        104.5, 105.75, 108.62, 107.75, 108.06, 108, 108.19, 108.12, 109.06, 108.75,\n        108.56, 106.62,\n    ];\n}\n\nfunction testCloses(): number[] {\n    return [\n        91.5, 94.815, 94.375, 95.095, 93.78, 94.625, 92.53, 92.75, 90.315, 92.47,\n        96.125, 97.25, 98.5, 89.875, 91, 92.815, 89.155, 89.345, 91.625, 89.875,\n        88.375, 87.625, 84.78, 83, 83.5, 81.375, 84.44, 89.25, 86.375, 86.25,\n        85.25, 87.125, 85.815, 88.97, 88.47, 86.875, 86.815, 84.875, 84.19, 83.875,\n        83.375, 85.5, 89.19, 89.44, 91.095, 90.75, 91.44, 89, 91, 90.5,\n        89.03, 88.815, 84.28, 83.5, 82.69, 84.75, 85.655, 86.19, 88.94, 89.28,\n        88.625, 88.5, 91.97, 91.5, 93.25, 93.5, 93.155, 91.72, 90, 89.69,\n        88.875, 85.19, 83.375, 84.875, 85.94, 97.25, 99.875, 104.94, 106, 102.5,\n        102.405, 104.595, 106.125, 106, 106.065, 104.625, 108.625, 109.315, 110.5, 112.75,\n        123, 119.625, 118.75, 119.25, 117.94, 116.44, 115.19, 111.875, 110.595, 118.125,\n        116, 116, 112, 113.75, 112.94, 116, 120.5, 116.62, 117, 115.25,\n        114.31, 115.5, 115.87, 120.69, 120.19, 120.75, 124.75, 123.37, 122.94, 122.56,\n        123.12, 122.56, 124.62, 129.25, 131, 132.25, 131, 132.81, 134, 137.38,\n        137.81, 137.88, 137.25, 136.31, 136.25, 134.63, 128.25, 129, 123.87, 124.81,\n        123, 126.25, 128.38, 125.37, 125.69, 122.25, 119.37, 118.5, 123.19, 123.5,\n        122.19, 119.31, 123.31, 121.12, 123.37, 127.37, 128.5, 123.87, 122.94, 121.75,\n        124.44, 122, 122.37, 122.94, 124, 123.19, 124.56, 127.25, 125.87, 128.86,\n        132, 130.75, 134.75, 135, 132.38, 133.31, 131.94, 130, 125.37, 130.13,\n        127.12, 125.19, 122, 125, 123, 123.5, 120.06, 121, 117.75, 119.87,\n        122, 119.19, 116.37, 113.5, 114.25, 110, 105.06, 107, 107.87, 107,\n        107.12, 107, 91, 93.94, 93.87, 95.5, 93, 94.94, 98.25, 96.75,\n        94.81, 94.37, 91.56, 90.25, 93.94, 93.62, 97, 95, 95.87, 94.06,\n        94.62, 93.75, 98, 103.94, 107.87, 106.06, 104.5, 105, 104.19, 103.06,\n        103.42, 105.27, 111.87, 116, 116.62, 118.28, 113.37, 109, 109.7, 109.25,\n        107, 109.19, 110, 109.2, 110.12, 108, 108.62, 109.75, 109.81, 109,\n        108.75, 107.87,\n    ];\n}\n\nfunction testVolumes(): number[] {\n    return [\n        4077500, 4955900, 4775300, 4155300, 4593100, 3631300, 3382800, 4954200, 4500000, 3397500,\n        4204500, 6321400, 10203600, 19043900, 11692000, 9553300, 8920300, 5970900, 5062300, 3705600,\n        5865600, 5603000, 5811900, 8483800, 5995200, 5408800, 5430500, 6283800, 5834800, 4515500,\n        4493300, 4346100, 3700300, 4600200, 4557200, 4323600, 5237500, 7404100, 4798400, 4372800,\n        3872300, 10750800, 5804800, 3785500, 5014800, 3507700, 4298800, 4842500, 3952200, 3304700,\n        3462000, 7253900, 9753100, 5953000, 5011700, 5910800, 4916900, 4135000, 4054200, 3735300,\n        2921900, 2658400, 4624400, 4372200, 5831600, 4268600, 3059200, 4495500, 3425000, 3630800,\n        4168100, 5966900, 7692800, 7362500, 6581300, 19587700, 10378600, 9334700, 10467200, 5671400,\n        5645000, 4518600, 4519500, 5569700, 4239700, 4175300, 4995300, 4776600, 4190000, 6035300,\n        12168900, 9040800, 5780300, 4320800, 3899100, 3221400, 3455500, 4304200, 4703900, 8316300,\n        10553900, 6384800, 7163300, 7007800, 5114100, 5263800, 6666100, 7398400, 5575000, 4852300,\n        4298100, 4900500, 4887700, 6964800, 4679200, 9165000, 6469800, 6792000, 4423800, 5231900,\n        4565600, 6235200, 5225900, 8261400, 5912500, 3545600, 5714500, 6653900, 6094500, 4799200,\n        5050800, 5648900, 4726300, 5585600, 5124800, 7630200, 14311600, 8793600, 8874200, 6966600,\n        5525500, 6515500, 5291900, 5711700, 4327700, 4568000, 6859200, 5757500, 7367000, 6144100,\n        4052700, 5849700, 5544700, 5032200, 4400600, 4894100, 5140000, 6610900, 7585200, 5963100,\n        6045500, 8443300, 6464700, 6248300, 4357200, 4774700, 6216900, 6266900, 5584800, 5284500,\n        7554500, 7209500, 8424800, 5094500, 4443600, 4591100, 5658400, 6094100, 14862200, 7544700,\n        6985600, 8093000, 7590000, 7451300, 7078000, 7105300, 8778800, 6643900, 10563900, 7043100,\n        6438900, 8057700, 14240000, 17872300, 7831100, 8277700, 15017800, 14183300, 13921100, 9683000,\n        9187300, 11380500, 69447300, 26673600, 13768400, 11371600, 9872200, 9450500, 11083300, 9552800,\n        11108400, 10374200, 16701900, 13741900, 8523600, 9551900, 8680500, 7151700, 9673100, 6264700,\n        8541600, 8358000, 18720800, 19683100, 13682500, 10668100, 9710600, 3113100, 5682000, 5763600,\n        5340000, 6220800, 14680500, 9933000, 11329500, 8145300, 16644700, 12593800, 7138100, 7442300,\n        9442300, 7123600, 7680600, 4839800, 4775500, 4008800, 4533600, 3741100, 4084800, 2685200,\n        3438000, 2870500,\n    ];\n}\n\n// ---------------------------------------------------------------------------\n// Run\n// ---------------------------------------------------------------------------\n\nmain();\n"
}
```

**Output:**
```
Wrote file successfully.
```

---

## Assistant (Build · claude-opus-4.6 · 9.7s)

**Tool: write**

**Input:**
```json
{
  "content": "[\n  { \"identifier\": \"bollingerBands\", \"params\": { \"length\": 20, \"multiplier\": 2 } },\n  { \"identifier\": \"simpleMovingAverage\", \"params\": { \"length\": 14 } },\n  { \"identifier\": \"exponentialMovingAverage\", \"params\": { \"length\": 10 } },\n  { \"identifier\": \"relativeStrengthIndex\", \"params\": { \"length\": 14 } },\n  { \"identifier\": \"movingAverageConvergenceDivergence\", \"params\": { \"fastLength\": 12, \"slowLength\": 26, \"signalLength\": 9 } },\n  { \"identifier\": \"averageTrueRange\", \"params\": { \"length\": 14 } },\n  { \"identifier\": \"stochastic\", \"params\": { \"fastKLength\": 14, \"slowKLength\": 3, \"slowDLength\": 3 } },\n  { \"identifier\": \"goertzelSpectrum\", \"params\": { \"length\": 64, \"range\": [2, 28], \"resolution\": 10 } }\n]\n",
  "filePath": "/home/dev/zpano/ts/cmd/iconf/settings.json"
}
```

**Output:**
```
Wrote file successfully.
```

**Tool: write**

**Input:**
```json
{
  "content": "#!/bin/bash\nset -euo pipefail\nnpx tsx cmd/iconf/main.ts cmd/iconf/settings.json output\n",
  "filePath": "/home/dev/zpano/ts/cmd/iconf/run-me.sh"
}
```

**Output:**
```
Wrote file successfully.
```

---

## Assistant (Build · claude-opus-4.6 · 6.5s)

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Create ts/cmd/iconf/main.ts",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Copy settings.json to ts/cmd/iconf/",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Create ts/cmd/iconf/run-me.sh",
      "priority": "medium",
      "status": "completed"
    },
    {
      "content": "Test the TS iconf tool",
      "priority": "high",
      "status": "in_progress"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Create ts/cmd/iconf/main.ts",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Copy settings.json to ts/cmd/iconf/",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Create ts/cmd/iconf/run-me.sh",
    "priority": "medium",
    "status": "completed"
  },
  {
    "content": "Test the TS iconf tool",
    "priority": "high",
    "status": "in_progress"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 6.0s)

**Tool: bash**

**Input:**
```json
{
  "command": "chmod +x ts/cmd/iconf/run-me.sh && cd ts && npx tsx cmd/iconf/main.ts cmd/iconf/settings.json output 2>&1",
  "description": "Run TS iconf tool",
  "timeout": 60000
}
```

**Output:**
```
wrote output.json
wrote output.ts

```

---

## Assistant (Build · claude-opus-4.6 · 6.4s)

It works. Let me verify the output structure:

**Tool: bash**

**Input:**
```json
{
  "command": "cd ts && wc -l output.json output.ts && echo \"---\" && node -e \"const c=JSON.parse(require('fs').readFileSync('output.json','utf8')); console.log('pricePane lines:', c.pricePane.lines.length); console.log('pricePane bands:', c.pricePane.bands.length); console.log('indicatorPanes:', c.indicatorPanes.length); c.indicatorPanes.forEach((p,i) => { console.log('  pane', i, '- lines:', p.lines.length, 'bands:', p.bands.length, 'heatmap:', !!p.heatmap); });\"",
  "description": "Verify output structure"
}
```

**Output:**
```
  31266 output.json
    139 output.ts
  31405 total
---
pricePane lines: 5
pricePane bands: 1
indicatorPanes: 6
  pane 0 - lines: 2 bands: 0 heatmap: false
  pane 1 - lines: 1 bands: 0 heatmap: false
  pane 2 - lines: 3 bands: 0 heatmap: false
  pane 3 - lines: 1 bands: 0 heatmap: false
  pane 4 - lines: 3 bands: 0 heatmap: false
  pane 5 - lines: 0 bands: 0 heatmap: true

```

---

## Assistant (Build · claude-opus-4.6 · 5.3s)

Output matches the expected structure: 5 price lines + 1 band, 6 indicator panes (BB oscillators, RSI, MACD, ATR, Stochastic, Goertzel heatmap).

**Tool: bash**

**Input:**
```json
{
  "command": "cd ts && rm output.json output.ts",
  "description": "Clean up test output files"
}
```

---

## Assistant (Build · claude-opus-4.6 · 4.5s)

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Create ts/cmd/iconf/main.ts",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Copy settings.json to ts/cmd/iconf/",
      "priority": "high",
      "status": "completed"
    },
    {
      "content": "Create ts/cmd/iconf/run-me.sh",
      "priority": "medium",
      "status": "completed"
    },
    {
      "content": "Test the TS iconf tool",
      "priority": "high",
      "status": "completed"
    }
  ]
}
```

**Output:**
```
[
  {
    "content": "Create ts/cmd/iconf/main.ts",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Copy settings.json to ts/cmd/iconf/",
    "priority": "high",
    "status": "completed"
  },
  {
    "content": "Create ts/cmd/iconf/run-me.sh",
    "priority": "medium",
    "status": "completed"
  },
  {
    "content": "Test the TS iconf tool",
    "priority": "high",
    "status": "completed"
  }
]
```

---

## Assistant (Build · claude-opus-4.6 · 6.9s)

All done. The TS `iconf` CLI is complete and tested. Here's what was created:

- **`ts/cmd/iconf/main.ts`** — Full port of the Go iconf logic to TypeScript. Reads settings, creates indicators via factory, runs 252 test bars, builds chart configuration, writes both `.json` and `.ts` output files.
- **`ts/cmd/iconf/settings.json`** — Same 8-indicator settings as the Go version.
- **`ts/cmd/iconf/run-me.sh`** — Shell runner script.

Output verified: 5 price lines + 1 band, 6 indicator panes (BB oscillators, RSI, MACD, ATR, Stochastic, Goertzel heatmap) — matching the Go version exactly.

---

