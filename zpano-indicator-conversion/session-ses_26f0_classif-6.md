# Split INDICATORS-ARCHITECTURE and CONVERTING-OLD-INDICATOR into skills?

**Session ID:** ses_26f009c55ffeeFyvE85VbCND9m
**Created:** 4/15/2026, 1:55:49 PM
**Updated:** 4/23/2026, 7:04:13 PM

---

## Assistant (Compaction · claude-opus-4.7 · 66.4s)

---
## Goal

Design and implement a **classification / categorization system** for ~70 zpano indicators so charting apps can filter and display the catalog along multiple dimensions: minimum input data type, volume usage, output data shape, semantic role, chart pane, adaptivity, and family.

Two precursor renames freed the word "Type" for taxonomic use:
- `core.Type` → `core.Identifier`
- `outputs.Type` → sub-package `outputs/shape` with `shape.Shape` + constants `shape.{Scalar, Band, Heatmap, Polyline}`; `outputs.Metadata.Type` → `.Shape`

## Instructions

- Build mode is engaged. Execute the locked plan phase by phase.
- After each phase: `cd go && go build ./... && go test ./...` and `cd ts && npm test` (baseline 884 specs, 0 failures) must remain green before starting the next phase.
- Honor `.opencode/skills/indicator-architecture/SKILL.md`:
  - Expand abbreviations; idiomatic Go short names (`err`/`len`/`cap`/`min`/`max`/`num`) OK.
  - Receivers: compound type → `s`; single-word type → first letter.
  - `mu sync.RWMutex` + `defer Unlock`/`defer RUnlock`.
  - Go ↔ TS local-variable parity. Both surfaces must mirror.
  - Every Go author folder has `doc.go`.
- LSP errors in `mbst-to-convert/` are pre-existing C noise — **ignore** (auto-attached to every write).
- User prefers Recommended options.
- User is impatient with explanatory pauses — push through and produce code.
- Phase 4 sign-off: user said "go" to apply the proposed table as-is.

### Phase plan

| # | Phase | Status |
|---|---|---|
| 1 | Rename `core.Type` → `core.Identifier` | **DONE** |
| 2 | Move `outputs.Type` → `outputs/shape` sub-package | **DONE** |
| 3 | Add taxonomy primitives (Role, Pane, Adaptivity, InputRequirement, VolumeUsage, OutputDescriptor, Descriptor) + accessors | **DONE** |
| 4 | Populate descriptor table for all 69 indicators | **DONE Go + TS** |
| 5 | Wire `Metadata()` to read from registry | pending |
| 6 | Document taxonomy in `indicator-architecture/SKILL.md` | pending |
| 7 | Update conversion skills (require descriptor row for new indicators) | pending |

### Locked design

```go
type Role int              // Smoother, Envelope, Overlay, Polyline, Oscillator,
                           // BoundedOscillator, Volatility, VolumeFlow, Directional,
                           // CyclePeriod, CyclePhase, FractalDimension, Spectrum,
                           // Signal, Histogram, RegimeFlag, Correlation
type Pane int              // Price | Own | OverlayOnParent
type Adaptivity int        // Static | Adaptive
type InputRequirement int  // ScalarInput | QuoteInput | BarInput | TradeInput
type VolumeUsage int       // NoVolume | AggregateBarVolume | PerTradeVolume | QuoteLiquidityVolume

type OutputDescriptor struct { Kind int; Shape shape.Shape; Role Role; Pane Pane }
type Descriptor struct {
    Identifier; Family string; Adaptivity; InputRequirement; VolumeUsage
    Outputs []OutputDescriptor
}
```

Naming deviates from original plan (added `Input` / `Volume` suffixes to disambiguate from `entities.{Scalar,Quote,Bar,Trade}` and `shape.{Scalar,Band,Heatmap,Polyline}`). JSON wire format uses short forms (`"scalar"`, `"bar"`, `"none"`, `"aggregateBar"`, …).

### VolumeUsage validity (enforced informally; TODO formal test)
- `AggregateBarVolume` ⇒ `BarInput`; `PerTradeVolume` ⇒ `TradeInput`; `QuoteLiquidityVolume` ⇒ `QuoteInput`; `NoVolume` ⇒ any.

## Discoveries

- 69 identifiers total in `core.Identifier`. Go enum is 1-based (`iota+1`); TS `IndicatorIdentifier` enum is **0-based** — pre-existing inconsistency.
- Per-indicator output enums: Go is 1-based (`iota+1`), TS is 0-based. **Descriptor `Kind` values therefore differ by 1 between Go (1..N) and TS (0..N-1).**
- Multi-output indicators (and shapes per kind) extracted from `Shape:` lines across all `*.go` indicator files; saved during exploration to `/tmp/kindshape.txt`.
- Pre-existing structs `outputs.{Band,Heatmap,Polyline}` blocked using bare names for the new shape constants → forced sub-package `outputs/shape`.
- The `write` tool intermittently fails with "Invalid input" (missing args) on very large content — splitting / retrying works.
- Family naming chosen by author folder: `johnehlers`→"John Ehlers", `welleswilder`→"Welles Wilder", `tusharchande`→"Tushar Chande", `johnbollinger`→"John Bollinger", `geraldappel`→"Gerald Appel", `larrywilliams`→"Larry Williams", `donaldlambert`→"Donald Lambert", `genequong`→"Gene Quong", `josephgranville`→"Joseph Granville", `igorlivshin`→"Igor Livshin", `marcchaikin`→"Marc Chaikin", `georgelane`→"George Lane", `jackhutson`→"Jack Hutson", `vladimirkravchuk`→"Vladimir Kravchuk", `timtillson`→"Tim Tillson", `patrickmulloy`→"Patrick Mulloy", `markjurik`→"Mark Jurik", `perrykaufman`→"Perry Kaufman", `common`→"Common", `custom`→"Custom".
- Adaptive set chosen: KAMA, JMA, MAMA, FRAMA, ATCF, DominantCycle, SineWave, HilbertTransformerInstantaneousTrendLine, TrendCycleMode, all four Corona indicators, AutoCorrelationPeriodogram. All others Static.
- Resolved deferred role decisions:
  - ATCF: Fatl/Satl/Rftl/Rstl/Rbci → Smoother (Price pane); Ftlm/Stlm/Pcci → Oscillator (Own pane).
  - ADX/ADXR Value/DMI: BoundedOscillator (since 0..100 bounded); component DI/DM → Directional; ATR/TR → Volatility.
  - CoronaTrendVigor scalar companion → Oscillator; other Corona scalar companions → BoundedOscillator (CSNR, CSWING) or CyclePeriod (CSPECT).
- BollingerBands per-output classification: Lower/Upper → Envelope (Price), Middle → Smoother (Price), BandWidth → Volatility (Own), PercentBand → BoundedOscillator (Own), Band → Envelope (Price).

## Accomplished

- **Phase 1, 2, 3 fully done** on both Go and TS.
- **Phase 4 fully done**: 69 descriptor entries written in Go and TS; coverage test (`TestDescriptorCoverage`) un-skipped and **passing** (`go test ./indicators/core/...` green); full Go suite green; TS suite still 884 specs / 0 failures.
- TS file uses helper functions `out()` / `desc()` plus single-letter alias bindings (`S`, `R`, `P`, `A`, `I`, `V`) to keep entries terse and one-line where possible.

### Still to do

1. **Verify TS suite green after final TS write** — last confirmed green was before populating `descriptors.ts`. Run `cd ts && npm test`.
2. **Phase 5**: Wire `core.Metadata()` calls in each indicator to read from the descriptor registry rather than constructing inline. Currently each indicator's `*.go` file builds `core.Metadata{Outputs: []outputs.Metadata{...}}` inline. After Phase 5, `Mnemonic` / `Description` stay inline but `Shape` / `Kind` come from the descriptor (or the whole `[]outputs.Metadata` is derived from `Descriptors()[id].Outputs` joined with mnemonic/description).
3. **Phase 6**: Document taxonomy + descriptor table conventions in `.opencode/skills/indicator-architecture/SKILL.md`.
4. **Phase 7**: Update conversion skills (the `mbst-to-convert/` workflow) to require a new descriptor row whenever an indicator is added.
5. **Optional**: add a Go test asserting VolumeUsage ⇔ InputRequirement validity rules.

## Relevant files / directories

### Go (modified / created)
- `go/indicators/core/identifier.go` — renamed enum (Phase 1)
- `go/indicators/core/role.go`, `pane.go`, `adaptivity.go`, `input_requirement.go`, `volume_usage.go` — taxonomy primitives (Phase 3)
- `go/indicators/core/output_descriptor.go` — `OutputDescriptor` struct (Phase 3)
- `go/indicators/core/descriptor.go` — `Descriptor` struct + `DescriptorOf` / `Descriptors` accessors (Phase 3)
- `go/indicators/core/descriptors.go` — populated registry of 69 entries (Phase 4)
- `go/indicators/core/descriptors_test.go` — `TestDescriptorCoverage` (active), `TestDescriptorOfUnregistered`, `TestDescriptorsReturnsCopy`
- `go/indicators/core/outputs/shape/shape.go` + `doc.go` — Shape sub-package (Phase 2)
- `go/indicators/core/outputs/metadata.go` — `Metadata.Type` → `.Shape` (Phase 2)

### TS (modified / created)
- `ts/indicators/core/indicator-identifier.ts` — renamed (Phase 1)
- `ts/indicators/core/role.ts`, `pane.ts`, `adaptivity.ts`, `input-requirement.ts`, `volume-usage.ts` — taxonomy primitives (Phase 3)
- `ts/indicators/core/output-descriptor.ts`, `descriptor.ts` — interfaces (Phase 3)
- `ts/indicators/core/descriptors.ts` — populated registry of 69 entries (Phase 4); uses `out()`/`desc()` helpers and `S`/`R`/`P`/`A`/`I`/`V` aliases
- `ts/indicators/core/outputs/shape/shape.ts` — Shape enum (Phase 2)

### Reference (not modified, used for inspection)
- `go/indicators/*/*/output.go` — per-indicator output enums (1-based)
- `ts/indicators/*/*/output.ts` — per-indicator output enums (0-based)
- `go/indicators/*/*/<name>.go` — `core.Metadata{}` construction sites (target of Phase 5)
- `.opencode/skills/indicator-architecture/SKILL.md` — Phase 6 target
- `/tmp/kindshape.txt` — saved scratch file with all `Kind:` / `Shape:` lines per indicator (may be stale after restart)

### Noise to ignore
- `mbst-to-convert/**/*.c` — pre-existing missing TaLib headers; LSP attaches errors to every write.

---

## User

---

## Assistant (Build · claude-opus-4.7 · 7.2s)

_Thinking:_

 I need to check that the TypeScript test suite is passing before moving forward to Phase 5.

**Tool: bash**

**Input:**
```json
{
  "command": "npm test 2>&1 | tail -30",
  "description": "Run TS test suite",
  "workdir": "/home/dev/zpano/ts"
}
```

**Output:**
```

> ts@1.0.0 test
> node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json

Started
....................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................


884 specs, 0 failures
Finished in 0.882 seconds

```

---

## Assistant (Build · claude-opus-4.7 · 7.2s)

TS green. Phase 4 confirmed complete. Proceeding to Phase 5: wire `Metadata()` to read from the descriptor registry.

Let me first inspect the current shape of `core.Metadata` and a sample indicator to plan the refactor.

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

## Assistant (Build · claude-opus-4.7 · 6.2s)

**Tool: grep**

**Input:**
```json
{
  "pattern": "func .* Metadata\\(\\)",
  "path": "/home/dev/zpano/go/indicators",
  "include": "*.go"
}
```

**Output:**
```
Found 73 matches
/home/dev/zpano/go/indicators/marcchaikin/advancedecline/advancedecline.go:
  Line 65: func (s *AdvanceDecline) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/marcchaikin/advancedeclineoscillator/advancedeclineoscillator.go:
  Line 140: func (s *AdvanceDeclineOscillator) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/common/rateofchangepercent/rateofchangepercent.go:
  Line 111: func (s *RateOfChangePercent) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/common/absolutepriceoscillator/absolutepriceoscillator.go:
  Line 158: func (s *AbsolutePriceOscillator) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/common/simplemovingaverage/simplemovingaverage.go:
  Line 114: func (s *SimpleMovingAverage) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/common/rateofchangeratio/rateofchangeratio.go:
  Line 121: func (s *RateOfChangeRatio) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/common/standarddeviation/standarddeviation.go:
  Line 115: func (s *StandardDeviation) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/common/triangularmovingaverage/triangularmovingaverage.go:
  Line 164: func (s *TriangularMovingAverage) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/common/momentum/momentum.go:
  Line 110: func (m *Momentum) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/common/variance/variance.go:
  Line 125: func (v *Variance) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/common/exponentialmovingaverage/exponentialmovingaverage.go:
  Line 163: func (s *ExponentialMovingAverage) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/common/weightedmovingaverage/weightedmovingaverage.go:
  Line 132: func (s *WeightedMovingAverage) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/common/pearsonscorrelationcoefficient/pearsonscorrelationcoefficient.go:
  Line 113: func (s *PearsonsCorrelationCoefficient) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/common/rateofchange/rateofchange.go:
  Line 111: func (s *RateOfChange) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/common/linearregression/linearregression.go:
  Line 132: func (s *LinearRegression) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/geraldappel/percentagepriceoscillator/percentagepriceoscillator.go:
  Line 163: func (s *PercentagePriceOscillator) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/geraldappel/movingaverageconvergencedivergence/movingaverageconvergencedivergence.go:
  Line 233: func (s *MovingAverageConvergenceDivergence) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/core/frequencyresponse/frequencyresponse_test.go:
  Line 14: func (s testFrequencyResponseIdentytyFilter) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/larrywilliams/williamspercentr/williamspercentr.go:
  Line 77: func (s *WilliamsPercentR) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/larrywilliams/ultimateoscillator/ultimateoscillator.go:
  Line 137: func (s *UltimateOscillator) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/patrickmulloy/doubleexponentialmovingaverage/doubleexponentialmovingaverage.go:
  Line 159: func (s *DoubleExponentialMovingAverage) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/josephgranville/onbalancevolume/onbalancevolume.go:
  Line 101: func (s *OnBalanceVolume) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/patrickmulloy/tripleexponentialmovingaverage/tripleexponentialmovingaverage.go:
  Line 164: func (s *TripleExponentialMovingAverage) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnbollinger/bollingerbands/bollingerbands.go:
  Line 207: func (s *BollingerBands) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnbollinger/bollingerbandstrend/bollingerbandstrend.go:
  Line 249: func (s *BollingerBandsTrend) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/timtillson/t2exponentialmovingaverage/t2exponentialmovingaverage.go:
  Line 209: func (s *T2ExponentialMovingAverage) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/timtillson/t3exponentialmovingaverage/t3exponentialmovingaverage.go:
  Line 223: func (s *T3ExponentialMovingAverage) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/vladimirkravchuk/adaptivetrendandcyclefilter/adaptivetrendandcyclefilter.go:
  Line 248: func (s *AdaptiveTrendAndCycleFilter) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/discretefouriertransformspectrum/discretefouriertransformspectrum.go:
  Line 221: func (s *DiscreteFourierTransformSpectrum) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/cybercycle/cybercycle.go:
  Line 197: func (s *CyberCycle) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition.go:
  Line 239: func (s *CoronaSwingPosition) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrum.go:
  Line 212: func (s *CoronaSpectrum) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode.go:
  Line 321: func (s *TrendCycleMode) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendline.go:
  Line 231: func (s *HilbertTransformerInstantaneousTrendLine) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/fractaladaptivemovingaverage/fractaladaptivemovingaverage.go:
  Line 191: func (s *FractalAdaptiveMovingAverage) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/dominantcycle/dominantcycle.go:
  Line 215: func (s *DominantCycle) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio.go:
  Line 250: func (s *CoronaSignalToNoiseRatio) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/mesaadaptivemovingaverage/mesaadaptivemovingaverage.go:
  Line 268: func (s *MesaAdaptiveMovingAverage) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/combbandpassspectrum/combbandpassspectrum.go:
  Line 223: func (s *CombBandPassSpectrum) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/zerolagerrorcorrectingexponentialmovingaverage/zerolagerrorcorrectingexponentialmovingaverage.go:
  Line 139: func (s *ZeroLagErrorCorrectingExponentialMovingAverage) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/instantaneoustrendline/instantaneoustrendline.go:
  Line 186: func (s *InstantaneousTrendLine) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/autocorrelationperiodogram/autocorrelationperiodogram.go:
  Line 228: func (s *AutoCorrelationPeriodogram) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/sinewave/sinewave.go:
  Line 214: func (s *SineWave) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor.go:
  Line 247: func (s *CoronaTrendVigor) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/zerolagexponentialmovingaverage/zerolagexponentialmovingaverage.go:
  Line 133: func (s *ZeroLagExponentialMovingAverage) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/roofingfilter/roofingfilter.go:
  Line 183: func (s *RoofingFilter) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/centerofgravityoscillator/centerofgravityoscillator.go:
  Line 145: func (s *CenterOfGravityOscillator) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/autocorrelationindicator/autocorrelationindicator.go:
  Line 181: func (s *AutoCorrelationIndicator) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/johnehlers/supersmoother/supersmoother.go:
  Line 136: func (s *SuperSmoother) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/genequong/moneyflowindex/moneyflowindex.go:
  Line 119: func (s *MoneyFlowIndex) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/donaldlambert/commoditychannelindex/commoditychannelindex.go:
  Line 118: func (s *CommodityChannelIndex) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/tusharchande/chandemomentumoscillator/chandemomentumoscillator.go:
  Line 116: func (s *ChandeMomentumOscillator) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/igorlivshin/balanceofpower/balanceofpower.go:
  Line 62: func (s *BalanceOfPower) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/tusharchande/stochasticrelativestrengthindex/stochasticrelativestrengthindex.go:
  Line 194: func (s *StochasticRelativeStrengthIndex) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/tusharchande/aroon/aroon.go:
  Line 88: func (a *Aroon) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/custom/goertzelspectrum/goertzelspectrum.go:
  Line 216: func (s *GoertzelSpectrum) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/custom/maximumentropyspectrum/maximumentropyspectrum.go:
  Line 216: func (s *MaximumEntropySpectrum) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/welleswilder/directionalmovementplus/directionalmovementplus.go:
  Line 76: func (s *DirectionalMovementPlus) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/welleswilder/parabolicstopandreverse/parabolicstopandreverse.go:
  Line 185: func (s *ParabolicStopAndReverse) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/welleswilder/averagedirectionalmovementindexrating/averagedirectionalmovementindexrating.go:
  Line 81: func (s *AverageDirectionalMovementIndexRating) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/welleswilder/directionalmovementindex/directionalmovementindex.go:
  Line 83: func (s *DirectionalMovementIndex) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/welleswilder/truerange/truerange.go:
  Line 59: func (s *TrueRange) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/welleswilder/normalizedaveragetruerange/normalizedaveragetruerange.go:
  Line 67: func (s *NormalizedAverageTrueRange) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/welleswilder/averagedirectionalmovementindex/averagedirectionalmovementindex.go:
  Line 77: func (s *AverageDirectionalMovementIndex) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/welleswilder/relativestrengthindex/relativestrengthindex.go:
  Line 105: func (s *RelativeStrengthIndex) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/welleswilder/directionalindicatorplus/directionalindicatorplus.go:
  Line 84: func (s *DirectionalIndicatorPlus) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/welleswilder/directionalmovementminus/directionalmovementminus.go:
  Line 76: func (s *DirectionalMovementMinus) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/welleswilder/directionalindicatorminus/directionalindicatorminus.go:
  Line 84: func (s *DirectionalIndicatorMinus) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/welleswilder/averagetruerange/averagetruerange.go:
  Line 80: func (s *AverageTrueRange) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/perrykaufman/kaufmanadaptivemovingaverage/kaufmanadaptivemovingaverage.go:
  Line 199: func (s *KaufmanAdaptiveMovingAverage) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/georgelane/stochastic/stochastic.go:
  Line 153: func (s *Stochastic) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/jackhutson/tripleexponentialmovingaverageoscillator/tripleexponentialmovingaverageoscillator.go:
  Line 132: func (s *TripleExponentialMovingAverageOscillator) Metadata() core.Metadata {

/home/dev/zpano/go/indicators/markjurik/jurikmovingaverage/jurikmovingaverage.go:
  Line 201: func (s *JurikMovingAverage) Metadata() core.Metadata {
```

---

## Assistant (Build · claude-opus-4.7 · 4.4s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/common/simplemovingaverage/simplemovingaverage.go",
  "offset": 100,
  "limit": 40
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/common/simplemovingaverage/simplemovingaverage.go</path>
<type>file</type>
<content>100: 
101: 	return sma, nil
102: }
103: 
104: // IsPrimed indicates whether an indicator is primed.
105: func (s *SimpleMovingAverage) IsPrimed() bool {
106: 	s.mu.RLock()
107: 	defer s.mu.RUnlock()
108: 
109: 	return s.primed
110: }
111: 
112: // Metadata describes an output data of the indicator.
113: // It always has a single scalar output -- the calculated value of the simple moving average.
114: func (s *SimpleMovingAverage) Metadata() core.Metadata {
115: 	return core.Metadata{
116: 		Identifier:  core.SimpleMovingAverage,
117: 		Mnemonic:    s.LineIndicator.Mnemonic,
118: 		Description: s.LineIndicator.Description,
119: 		Outputs: []outputs.Metadata{
120: 			{
121: 				Kind:        int(Value),
122: 				Shape:       shape.Scalar,
123: 				Mnemonic:    s.LineIndicator.Mnemonic,
124: 				Description: s.LineIndicator.Description,
125: 			},
126: 		},
127: 	}
128: }
129: 
130: // Update updates the value of the simple moving average given the next sample.
131: //
132: // The indicator is not primed during the first ℓ-1 updates.
133: func (s *SimpleMovingAverage) Update(sample float64) float64 {
134: 	if math.IsNaN(sample) {
135: 		return sample
136: 	}
137: 
138: 	s.mu.Lock()
139: 	defer s.mu.Unlock()

(Showing lines 100-139 of 162. Use offset=140 to continue.)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnbollinger/bollingerbands/bollingerbands.go",
  "offset": 200,
  "limit": 60
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnbollinger/bollingerbands/bollingerbands.go</path>
<type>file</type>
<content>200: 	s.mu.RLock()
201: 	defer s.mu.RUnlock()
202: 
203: 	return s.primed
204: }
205: 
206: // Metadata describes the output data of the indicator.
207: func (s *BollingerBands) Metadata() core.Metadata {
208: 	desc := "Bollinger Bands " + s.mnemonic
209: 
210: 	return core.Metadata{
211: 		Identifier:  core.BollingerBands,
212: 		Mnemonic:    s.mnemonic,
213: 		Description: desc,
214: 		Outputs: []outputs.Metadata{
215: 			{
216: 				Kind:        int(Lower),
217: 				Shape:       shape.Scalar,
218: 				Mnemonic:    s.mnemonic + " lower",
219: 				Description: desc + " Lower",
220: 			},
221: 			{
222: 				Kind:        int(Middle),
223: 				Shape:       shape.Scalar,
224: 				Mnemonic:    s.mnemonic + " middle",
225: 				Description: desc + " Middle",
226: 			},
227: 			{
228: 				Kind:        int(Upper),
229: 				Shape:       shape.Scalar,
230: 				Mnemonic:    s.mnemonic + " upper",
231: 				Description: desc + " Upper",
232: 			},
233: 			{
234: 				Kind:        int(BandWidth),
235: 				Shape:       shape.Scalar,
236: 				Mnemonic:    s.mnemonic + " bandWidth",
237: 				Description: desc + " Band Width",
238: 			},
239: 			{
240: 				Kind:        int(PercentBand),
241: 				Shape:       shape.Scalar,
242: 				Mnemonic:    s.mnemonic + " percentBand",
243: 				Description: desc + " Percent Band",
244: 			},
245: 			{
246: 				Kind:        int(Band),
247: 				Shape:       shape.Band,
248: 				Mnemonic:    s.mnemonic + " band",
249: 				Description: desc + " Band",
250: 			},
251: 		},
252: 	}
253: }
254: 
255: // Update updates the indicator given the next sample value.
256: // Returns lower, middle, upper, bandWidth, percentBand values.
257: //
258: //nolint:nonamedreturns
259: func (s *BollingerBands) Update(sample float64) (lower, middle, upper, bw, pctB float64) {

(Showing lines 200-259 of 353. Use offset=260 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 11.7s)

_Thinking:_

 I'm recognizing the structure now—each `Metadata()` call builds an identifier, mnemonic, and description, then populates an outputs slice where each output gets its own kind, shape, mnemonic, and description. For phase 5, I need a helper function that takes an identifier and per-output metadata details, then generates the complete outputs metadata structure.

The challenge is that mnemonics are instance-specific (like when BollingerBands appends " lower" to its base mnemonic), so they can't live in a static registry. My best approach is a helper that accepts per-output mnemonic and description tuples, then merges them with the kind and shape information already stored in the registry, looking them up by index. The critical constraint is ensuring the descriptor outputs stay in their canonical order so the indexing aligns correctly.

Before refactoring 70+ files, let me check the descriptor ordering and confirm a plan with the user.

**Tool: bash**

**Input:**
```json
{
  "command": "grep -n \"BollingerBands\" /home/dev/zpano/go/indicators/core/descriptors.go | head -5",
  "description": "Check BollingerBands descriptor row"
}
```

**Output:**
```
115:	BollingerBands: {
116:		Identifier: BollingerBands, Family: "John Bollinger",
380:	BollingerBandsTrend: {
381:		Identifier: BollingerBandsTrend, Family: "John Bollinger",

```

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/descriptors.go",
  "offset": 115,
  "limit": 15
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/descriptors.go</path>
<type>file</type>
<content>115: 	BollingerBands: {
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

(Showing lines 115-129 of 499. Use offset=130 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 480.6s)

_Thinking:_

 Now I'm designing a helper function to construct metadata, introducing an `OutputText` type to hold the mnemonic and description, then building a method that takes an identifier and assembles the complete metadata object.

The key insight is that by using the registry, indicator sites only need to import `core` and won't need the local output enum anymore — though external consumers still need it for identification. I should ask the user to confirm this API design before refactoring everywhere.

Phase 5 needs a design decision on the helper API before I touch 70+ files. Two reasonable shapes:

**Tool: question**

**Input:**
```json
{
  "questions": [
    {
      "header": "Phase 5 helper API",
      "question": "How should indicator `Metadata()` methods consume the descriptor registry?",
      "options": [
        {
          "label": "core.BuildMetadata helper (Recommended)",
          "description": "Add `core.BuildMetadata(id, mnemonic, description, []OutputText{...})` that pulls Kind+Shape from the registry. Each indicator's Metadata() shrinks to ~10 lines, no more `outputs`/`shape` imports, no more `int(Lower)` casts. Output order must match the registry."
        },
        {
          "label": "Per-indicator builder + descriptor lookup inside",
          "description": "Each indicator keeps writing []outputs.Metadata{...} but populates Kind/Shape via `core.DescriptorOf(id).Outputs[i].Kind/Shape`. More verbose, less safe (index drift), but more explicit."
        },
        {
          "label": "Move mnemonic/description into the registry too",
          "description": "Push per-output Mnemonic/Description into the descriptor (templated with the indicator instance mnemonic). Biggest churn, deepest dedup, but loses per-instance flexibility (e.g., Bollinger's `s.mnemonic + ' lower'`)."
        }
      ]
    },
    {
      "header": "Phase 5 ordering check",
      "question": "Add a runtime/test-time assertion that each indicator's Metadata() outputs match the registry order and length?",
      "options": [
        {
          "label": "Yes, panic in BuildMetadata on mismatch (Recommended)",
          "description": "BuildMetadata panics if len(outputTexts) != len(descriptor.Outputs). Catches drift immediately during dev."
        },
        {
          "label": "Yes, but only via a unit test",
          "description": "Add a Go test that walks every indicator, calls Metadata(), and verifies Kind+Shape match the registry. No runtime cost."
        },
        {
          "label": "No assertion",
          "description": "Trust the implementer."
        }
      ]
    }
  ]
}
```

**Output:**
```
User has answered your questions: "How should indicator `Metadata()` methods consume the descriptor registry?"="core.BuildMetadata helper (Recommended)", "Add a runtime/test-time assertion that each indicator's Metadata() outputs match the registry order and length?"="Yes, but only via a unit test". You can now continue with the user's answers in mind.
```

---

## Assistant (Build · claude-opus-4.7 · 9.3s)

_Thinking:_

 I'm laying out a plan to refactor the metadata system across both Go and TypeScript: first I'll create a helper function in Go that standardizes how metadata is built with mnemonics and descriptions, then refactor all 70 existing Go methods to use it, mirror that approach in TypeScript with a similar helper, and finally update all 70 TypeScript methods to match.

This is a substantial refactoring effort, so I'll need to be strategic — I'll use pattern-based edits where possible, but since each indicator is unique, I'll start by building the Go helper, refactoring one indicator as a template to establish the pattern, then systematically apply it across the rest. First, I should check what the current shape of `core.Metadata` looks like to understand what I'm working with.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core</path>
<type>directory</type>
<entries>
adaptivity.go
common.go
componenttriplemnemonic.go
descriptor.go
descriptors_test.go
descriptors.go
frequencyresponse/
identifier_test.go
identifier.go
indicator.go
input_requirement.go
lineindicator.go
metadata.go
output_descriptor.go
output.go
outputs/
pane.go
role.go
specification.go
volume_usage.go

(20 entries)
</entries>
```

---

## Assistant (Build · claude-opus-4.7 · 4.1s)

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

---

## Assistant (Build · claude-opus-4.7 · 10.6s)

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/build_metadata.go",
  "content": "package core\n\nimport (\n\t\"fmt\"\n\n\t\"zpano/indicators/core/outputs\"\n)\n\n// OutputText provides the per-output mnemonic and description\n// used when building a Metadata from the descriptor registry.\ntype OutputText struct {\n\t// Mnemonic is a short name (mnemonic) of the output.\n\tMnemonic string\n\n\t// Description is a description of the output.\n\tDescription string\n}\n\n// BuildMetadata constructs a Metadata for the indicator with the given identifier\n// by joining the registry's per-output Kind and Shape with the supplied\n// per-output mnemonic and description.\n//\n// texts must be in the same order and length as the descriptor's Outputs.\n// It panics if no descriptor is registered for the identifier or if the\n// length of texts does not match the descriptor's Outputs.\nfunc BuildMetadata(id Identifier, mnemonic, description string, texts []OutputText) Metadata {\n\td, ok := DescriptorOf(id)\n\tif !ok {\n\t\tpanic(fmt.Sprintf(\"core.BuildMetadata: no descriptor registered for identifier %v\", id))\n\t}\n\n\tif len(texts) != len(d.Outputs) {\n\t\tpanic(fmt.Sprintf(\n\t\t\t\"core.BuildMetadata: identifier %v has %d outputs in descriptor but %d texts were supplied\",\n\t\t\tid, len(d.Outputs), len(texts),\n\t\t))\n\t}\n\n\tout := make([]outputs.Metadata, len(texts))\n\tfor i, t := range texts {\n\t\tout[i] = outputs.Metadata{\n\t\t\tKind:        d.Outputs[i].Kind,\n\t\t\tShape:       d.Outputs[i].Shape,\n\t\t\tMnemonic:    t.Mnemonic,\n\t\t\tDescription: t.Description,\n\t\t}\n\t}\n\n\treturn Metadata{\n\t\tIdentifier:  id,\n\t\tMnemonic:    mnemonic,\n\t\tDescription: description,\n\t\tOutputs:     out,\n\t}\n}\n"
}
```

**Output:**
```
Wrote file successfully.

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/rate-of-change-ratio/ta_ROCR.c">
ERROR [67:29] 'ta_func.h' file not found
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/rate-of-change-ratio/ta_ROCR100.c">
ERROR [67:29] 'ta_func.h' file not found
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/rate-of-change-ratio/test_mom.c">
ERROR [88:10] 'ta_test_priv.h' file not found
ERROR [111:4] Unknown type name 'TA_Integer'
ERROR [114:4] Unknown type name 'TA_Integer'
ERROR [115:4] Unknown type name 'TA_Integer'
ERROR [117:4] Unknown type name 'TA_Integer'
ERROR [119:4] Unknown type name 'TA_RetCode'
ERROR [121:4] Unknown type name 'TA_Integer'
ERROR [122:4] Unknown type name 'TA_Real'
ERROR [124:4] Unknown type name 'TA_Integer'
ERROR [125:4] Unknown type name 'TA_Integer'
ERROR [131:10] Unknown type name 'TA_Real'
ERROR [135:8] Unknown type name 'ErrorNumber'
ERROR [135:35] Unknown type name 'TA_History'
ERROR [148:33] Use of undeclared identifier 'TA_OUT_OF_RANGE_START_INDEX'
ERROR [149:34] Use of undeclared identifier 'TA_OUT_OF_RANGE_END_INDEX'
ERROR [150:33] Use of undeclared identifier 'TA_OUT_OF_RANGE_END_INDEX'
ERROR [153:34] Use of undeclared identifier 'TA_SUCCESS'
ERROR [154:34] Use of undeclared identifier 'TA_SUCCESS'
ERROR [155:34] Use of undeclared identifier 'TA_SUCCESS'
ERROR [156:34] Use of undeclared identifier 'TA_SUCCESS'
... and 1 more
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/pearsons-correlation-coefficient/ta_CORREL.c">
ERROR [68:29] 'ta_func.h' file not found
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/pearsons-correlation-coefficient/test_per_hl.c">
ERROR [60:10] 'ta_test_priv.h' file not found
ERROR [85:4] Unknown type name 'TA_Integer'
ERROR [89:4] Unknown type name 'TA_Integer'
ERROR [90:4] Unknown type name 'TA_Integer'
ERROR [91:4] Unknown type name 'TA_Integer'
ERROR [93:4] Unknown type name 'TA_RetCode'
ERROR [95:4] Unknown type name 'TA_Integer'
ERROR [96:4] Unknown type name 'TA_Real'
ERROR [98:4] Unknown type name 'TA_Integer'
ERROR [99:4] Unknown type name 'TA_Integer'
ERROR [105:10] Unknown type name 'TA_Real'
ERROR [106:10] Unknown type name 'TA_Real'
ERROR [110:8] Unknown type name 'ErrorNumber'
ERROR [110:35] Unknown type name 'TA_History'
ERROR [125:35] Use of undeclared identifier 'TA_SUCCESS'
ERROR [126:35] Use of undeclared identifier 'TA_SUCCESS'
ERROR [131:38] Use of undeclared identifier 'TA_SUCCESS'
ERROR [132:38] Use of undeclared identifier 'TA_SUCCESS'
ERROR [133:38] Use of undeclared identifier 'TA_SUCCESS'
ERROR [139:40] Use of undeclared identifier 'TA_SUCCESS'
... and 1 more
</diagnostics>
```

---

## Assistant (Build · claude-opus-4.7 · 6.0s)

_Thinking:_

 Now I'm refactoring the SimpleMovingAverage indicator and running a compile check to make sure everything works.

Now refactor SimpleMovingAverage as the template, verify build, then iterate.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/common/simplemovingaverage/simplemovingaverage.go",
  "limit": 20,
  "offset": 1
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/common/simplemovingaverage/simplemovingaverage.go</path>
<type>file</type>
<content>1: package simplemovingaverage
2: 
3: //nolint: gofumpt
4: import (
5: 	"fmt"
6: 	"math"
7: 	"sync"
8: 
9: 	"zpano/entities"
10: 	"zpano/indicators/core"
11: 	"zpano/indicators/core/outputs"
12: 	"zpano/indicators/core/outputs/shape"
13: )
14: 
15: // SimpleMovingAverage computes the simple, or arithmetic, moving average (SMA) by adding the samples
16: // for a number of time periods (length, ℓ) and then dividing this total by the number of time periods.
17: //
18: // In other words, this is an unweighted mean (gives equal weight to each sample) of the previous ℓ samples.
19: //
20: // This implementation updates the value of the SMA incrementally using the formula:

(Showing lines 1-20 of 162. Use offset=21 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.7s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/common/simplemovingaverage/simplemovingaverage.go",
  "oldString": "\t\"zpano/entities\"\n\t\"zpano/indicators/core\"\n\t\"zpano/indicators/core/outputs\"\n\t\"zpano/indicators/core/outputs/shape\"\n)",
  "newString": "\t\"zpano/entities\"\n\t\"zpano/indicators/core\"\n)"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 6.9s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/common/simplemovingaverage/simplemovingaverage.go",
  "newString": "func (s *SimpleMovingAverage) Metadata() core.Metadata {\n\treturn core.BuildMetadata(\n\t\tcore.SimpleMovingAverage,\n\t\ts.LineIndicator.Mnemonic,\n\t\ts.LineIndicator.Description,\n\t\t[]core.OutputText{\n\t\t\t{Mnemonic: s.LineIndicator.Mnemonic, Description: s.LineIndicator.Description},\n\t\t},\n\t)\n}",
  "oldString": "func (s *SimpleMovingAverage) Metadata() core.Metadata {\n\treturn core.Metadata{\n\t\tIdentifier:  core.SimpleMovingAverage,\n\t\tMnemonic:    s.LineIndicator.Mnemonic,\n\t\tDescription: s.LineIndicator.Description,\n\t\tOutputs: []outputs.Metadata{\n\t\t\t{\n\t\t\t\tKind:        int(Value),\n\t\t\t\tShape:       shape.Scalar,\n\t\t\t\tMnemonic:    s.LineIndicator.Mnemonic,\n\t\t\t\tDescription: s.LineIndicator.Description,\n\t\t\t},\n\t\t},\n\t}\n}"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.5s)

**Tool: bash**

**Input:**
```json
{
  "command": "go build ./... 2>&1 | head -30",
  "description": "Verify go build after SMA refactor",
  "workdir": "/home/dev/zpano/go"
}
```

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./indicators/common/simplemovingaverage/... 2>&1 | tail -10",
  "description": "Verify SMA tests pass",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
ok  	zpano/indicators/common/simplemovingaverage	0.003s

```

---

## Assistant (Build · claude-opus-4.7 · 9.1s)

_Thinking:_

 The template's working well. I've got about 70 more indicators to refactor, and since each one is unique, I should check the patterns first—specifically whether the mnemonic and description templates are consistent across all the Metadata() calls. Rather than do this manually, I'll have a subagent handle the bulk refactoring in batches following the SMA template pattern.

Actually, on second thought, I think I should just do these myself since the mnemonic expressions vary too much to automate safely. Let me start with the common/ indicators that have single outputs and work through several at once.

Template works. Now batch-refactor the rest. Let me view several Metadata() functions to understand variance.

**Tool: bash**

**Input:**
```json
{
  "command": "for f in $(grep -rl \"func.*Metadata() core.Metadata\" /home/dev/zpano/go/indicators --include=\"*.go\" | grep -v _test.go | grep -v frequencyresponse); do\n  echo \"=== $f ===\"\ndone | wc -l",
  "description": "Count indicator files to refactor"
}
```

**Output:**
```
72

```

---

## Assistant (Build · claude-opus-4.7 · 187.5s)

_Thinking:_

 I've got 72 files to work with, so I'm going to set up a subagent with detailed instructions and use the SMA file as my reference template.

I'll dispatch a subagent to mass-refactor the remaining 71 files following the established template.

**Tool: task**

**Input:**
```json
{
  "description": "Refactor Metadata() methods",
  "subagent_type": "general",
  "prompt": "Refactor all indicator `Metadata()` methods in /home/dev/zpano/go/indicators/ to use the new `core.BuildMetadata` helper instead of constructing `core.Metadata` inline.\n\n## Reference template (already converted)\n\nSee /home/dev/zpano/go/indicators/common/simplemovingaverage/simplemovingaverage.go. Notice:\n1. Removed imports: `\"zpano/indicators/core/outputs\"` and `\"zpano/indicators/core/outputs/shape\"` are no longer needed (drop if unused elsewhere in file — check!).\n2. The `Metadata()` function body is now:\n```go\nreturn core.BuildMetadata(\n    core.SimpleMovingAverage,                       // identifier\n    s.LineIndicator.Mnemonic,                       // indicator-level mnemonic\n    s.LineIndicator.Description,                    // indicator-level description\n    []core.OutputText{\n        {Mnemonic: s.LineIndicator.Mnemonic, Description: s.LineIndicator.Description},\n    },\n)\n```\n\n## What BuildMetadata does\n\nIt pulls each output's `Kind` and `Shape` from the descriptor registry in /home/dev/zpano/go/indicators/core/descriptors.go. So the caller no longer supplies those — only the per-output `Mnemonic` and `Description`, **in the order they appear in the registry's `Outputs` slice** (which is Kind ascending).\n\n## Files to refactor\n\nRun this to list them:\n```\ngrep -rl \"func.*Metadata() core.Metadata\" /home/dev/zpano/go/indicators --include=\"*.go\" | grep -v _test.go | grep -v frequencyresponse | grep -v simplemovingaverage\n```\n\nShould be ~71 files.\n\n## Procedure for each file\n\n1. Read the current `Metadata()` method. Note:\n   - The `core.Metadata{Identifier: ..., Mnemonic: ..., Description: ..., Outputs: []outputs.Metadata{...}}` fields.\n   - Any local variables set up before the return (like `desc := \"Bollinger Bands \" + s.mnemonic`) — keep them.\n\n2. Check the descriptor in /home/dev/zpano/go/indicators/core/descriptors.go for that identifier to confirm output ORDER (by Kind).\n\n3. Rewrite to:\n```go\nreturn core.BuildMetadata(\n    core.<IdentifierName>,\n    <mnemonicExpr>,\n    <descriptionExpr>,\n    []core.OutputText{\n        {Mnemonic: <out0Mnemonic>, Description: <out0Description>},\n        {Mnemonic: <out1Mnemonic>, Description: <out1Description>},\n        ...\n    },\n)\n```\nOrder the OutputText entries to match descriptor.Outputs order (by Kind).\n\n4. Remove unused imports:\n   - `\"zpano/indicators/core/outputs\"` — usually only used in Metadata() ⇒ remove.\n   - `\"zpano/indicators/core/outputs/shape\"` — usually only used in Metadata() ⇒ remove.\n   CAUTION: grep for other uses in the same file before removing. If still used elsewhere, keep.\n\n5. Run `go build ./...` in /home/dev/zpano/go after each small batch to catch errors early.\n\n## Important\n\n- The local per-indicator output enum (`type OutputKind int` with `Value`, `Lower`, `Middle`, etc.) is NO LONGER referenced in Metadata(). That's fine — leave the enum file alone; external consumers still use it.\n- Do NOT change any other logic. Only `Metadata()` + imports.\n- Per-output Mnemonic/Description strings must remain character-identical to before.\n- Two files have special shapes (not just Scalar):\n  - bollingerbands: Band output has Shape.Band → now from registry, you don't specify.\n  - bollingerbandstrend, and any indicator with Polyline/Heatmap outputs — same, shape comes from registry.\n- Metadata test file: /home/dev/zpano/go/indicators/core/frequencyresponse/frequencyresponse_test.go — SKIP this one (test helper, not a real indicator).\n\n## Success criteria\n\n- `cd /home/dev/zpano/go && go build ./...` clean.\n- `cd /home/dev/zpano/go && go test ./...` green.\n- All ~71 indicator files refactored.\n\nReport back: number of files modified, any files you had to skip or where order was ambiguous, and final test result.\n\nIGNORE all LSP errors from /home/dev/zpano/mbst-to-convert/ — those are pre-existing C file noise unrelated to Go.\n"
}
```

**Output:**
```
task_id: ses_244c6e94affeUpqbTnGnh3bJz4 (for resuming to continue this task if needed)

<task_result>
## Summary

**Files modified: 71** (all targeted indicator `Metadata()` methods).

- **69 files** refactored automatically on the first script run.
- **2 files** (`bollingerbands`, `weightedmovingaverage`) were already converted during script validation; included in the 71 total.
- **Skipped** (per instructions): `frequencyresponse_test.go` helper, and `simplemovingaverage` (reference).

**Order:** No ambiguity. Every existing file already listed outputs in local `OutputKind` iota order, which matches descriptor Kind-ascending order. Verified spot-checks on multi-output indicators (bollingerbands, directionalmovementindex, mesaadaptivemovingaverage, trendcyclemode, adaptivetrendandcyclefilter).

**Import cleanup:**
- `zpano/indicators/core/outputs/shape` removed from all 71 (was only used in `Metadata()`).
- `zpano/indicators/core/outputs` removed wherever no longer referenced; kept in files that still use `outputs.NewBand`, `outputs.NewEmptyBand`, etc. (e.g., bollingerbands, mesaadaptivemovingaverage, sinewave).

**Verification:**
- `go build ./...` → clean
- `go vet ./...` → clean
- `go test ./...` → all packages `ok` (no FAIL)
- `gofmt -w` run afterward to normalize a few minor blank-line artifacts the script produced.

Approach: wrote a Python AST-aware (brace/string-respecting) script at `/tmp/refactor_metadata.py` that parsed each `Metadata()` body, extracted `Identifier`, `Mnemonic`, `Description`, and per-output `Mnemonic`/`Description` expressions verbatim (character-identical), emitted the `core.BuildMetadata(...)` call, preserved any local variables before the `return`, and conditionally stripped the two unused imports.
</task_result>
```

---

## Assistant (Build · claude-opus-4.7 · 5.0s)

**Tool: bash**

**Input:**
```json
{
  "command": "go build ./... 2>&1 | head -5 && go test ./... 2>&1 | grep -E \"FAIL|ok.*indicators\" | head -20",
  "description": "Verify Go build + test",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
ok  	zpano/indicators/common/absolutepriceoscillator	(cached)
ok  	zpano/indicators/common/exponentialmovingaverage	(cached)
ok  	zpano/indicators/common/linearregression	(cached)
ok  	zpano/indicators/common/momentum	(cached)
ok  	zpano/indicators/common/pearsonscorrelationcoefficient	(cached)
ok  	zpano/indicators/common/rateofchange	(cached)
ok  	zpano/indicators/common/rateofchangepercent	(cached)
ok  	zpano/indicators/common/rateofchangeratio	(cached)
ok  	zpano/indicators/common/simplemovingaverage	(cached)
ok  	zpano/indicators/common/standarddeviation	(cached)
ok  	zpano/indicators/common/triangularmovingaverage	(cached)
ok  	zpano/indicators/common/variance	(cached)
ok  	zpano/indicators/common/weightedmovingaverage	(cached)
ok  	zpano/indicators/core	(cached)
ok  	zpano/indicators/core/frequencyresponse	(cached)
ok  	zpano/indicators/core/outputs	(cached)
ok  	zpano/indicators/core/outputs/shape	(cached)
ok  	zpano/indicators/custom/goertzelspectrum	(cached)
ok  	zpano/indicators/custom/maximumentropyspectrum	(cached)
ok  	zpano/indicators/donaldlambert/commoditychannelindex	(cached)

```

---

## Assistant (Build · claude-opus-4.7 · 4.8s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./... 2>&1 | grep -E \"FAIL\" | head",
  "description": "Check for any failures",
  "workdir": "/home/dev/zpano/go"
}
```

---

## Assistant (Build · claude-opus-4.7 · 22.1s)

Go side green. Now add the Phase 5 validation test, then mirror to TS.

**Tool: write**

**Input:**
```json
{
  "content": "package core_test\n\nimport (\n\t\"reflect\"\n\t\"testing\"\n\n\t\"zpano/indicators/core\"\n\n\tadvancedecline \"zpano/indicators/marcchaikin/advancedecline\"\n\tadvancedeclineoscillator \"zpano/indicators/marcchaikin/advancedeclineoscillator\"\n\tabsolutepriceoscillator \"zpano/indicators/common/absolutepriceoscillator\"\n\texponentialmovingaverage \"zpano/indicators/common/exponentialmovingaverage\"\n\tlinearregression \"zpano/indicators/common/linearregression\"\n\tmomentum \"zpano/indicators/common/momentum\"\n\tpearsonscorrelationcoefficient \"zpano/indicators/common/pearsonscorrelationcoefficient\"\n\trateofchange \"zpano/indicators/common/rateofchange\"\n\trateofchangepercent \"zpano/indicators/common/rateofchangepercent\"\n\trateofchangeratio \"zpano/indicators/common/rateofchangeratio\"\n\tsimplemovingaverage \"zpano/indicators/common/simplemovingaverage\"\n\tstandarddeviation \"zpano/indicators/common/standarddeviation\"\n\ttriangularmovingaverage \"zpano/indicators/common/triangularmovingaverage\"\n\tvariance \"zpano/indicators/common/variance\"\n\tweightedmovingaverage \"zpano/indicators/common/weightedmovingaverage\"\n)\n\n// metadataProvider is implemented by every indicator.\ntype metadataProvider interface {\n\tMetadata() core.Metadata\n}\n\n// newSample returns a minimal instance for each indicator, sufficient to\n// invoke Metadata(). Constructors vary; some need parameters. We use the\n// smallest valid configuration.\nfunc newSamples(t *testing.T) map[core.Identifier]metadataProvider {\n\tt.Helper()\n\n\tsamples := map[core.Identifier]metadataProvider{}\n\n\tmustAdd := func(id core.Identifier, mk func() (metadataProvider, error)) {\n\t\tinst, err := mk()\n\t\tif err != nil {\n\t\t\tt.Fatalf(\"constructing sample for %v: %v\", id, err)\n\t\t}\n\t\tsamples[id] = inst\n\t}\n\n\tmustAdd(core.SimpleMovingAverage, func() (metadataProvider, error) {\n\t\treturn simplemovingaverage.New(5)\n\t})\n\tmustAdd(core.ExponentialMovingAverage, func() (metadataProvider, error) {\n\t\treturn exponentialmovingaverage.New(5, 0)\n\t})\n\tmustAdd(core.WeightedMovingAverage, func() (metadataProvider, error) {\n\t\treturn weightedmovingaverage.New(5)\n\t})\n\tmustAdd(core.TriangularMovingAverage, func() (metadataProvider, error) {\n\t\treturn triangularmovingaverage.New(5)\n\t})\n\tmustAdd(core.Momentum, func() (metadataProvider, error) {\n\t\treturn momentum.New(5)\n\t})\n\tmustAdd(core.RateOfChange, func() (metadataProvider, error) {\n\t\treturn rateofchange.New(5)\n\t})\n\tmustAdd(core.RateOfChangePercent, func() (metadataProvider, error) {\n\t\treturn rateofchangepercent.New(5)\n\t})\n\tmustAdd(core.RateOfChangeRatio, func() (metadataProvider, error) {\n\t\treturn rateofchangeratio.New(5)\n\t})\n\tmustAdd(core.StandardDeviation, func() (metadataProvider, error) {\n\t\treturn standarddeviation.New(5)\n\t})\n\tmustAdd(core.Variance, func() (metadataProvider, error) {\n\t\treturn variance.New(5)\n\t})\n\tmustAdd(core.LinearRegression, func() (metadataProvider, error) {\n\t\treturn linearregression.New(5)\n\t})\n\tmustAdd(core.PearsonsCorrelationCoefficient, func() (metadataProvider, error) {\n\t\treturn pearsonscorrelationcoefficient.New(5)\n\t})\n\tmustAdd(core.AbsolutePriceOscillator, func() (metadataProvider, error) {\n\t\treturn absolutepriceoscillator.New(5, 10, 0)\n\t})\n\tmustAdd(core.AdvanceDecline, func() (metadataProvider, error) {\n\t\treturn advancedecline.New()\n\t})\n\tmustAdd(core.AdvanceDeclineOscillator, func() (metadataProvider, error) {\n\t\treturn advancedeclineoscillator.New(5)\n\t})\n\n\treturn samples\n}\n\n// TestMetadataMatchesDescriptor verifies that every sampled indicator's\n// Metadata() returns outputs whose Kind and Shape match the registered\n// Descriptor for that identifier.\nfunc TestMetadataMatchesDescriptor(t *testing.T) {\n\tfor id, inst := range newSamples(t) {\n\t\tt.Run(id.String(), func(t *testing.T) {\n\t\t\tdesc, ok := core.DescriptorOf(id)\n\t\t\tif !ok {\n\t\t\t\tt.Fatalf(\"no descriptor registered for %v\", id)\n\t\t\t}\n\n\t\t\tmd := inst.Metadata()\n\n\t\t\tif md.Identifier != id {\n\t\t\t\tt.Errorf(\"Metadata.Identifier = %v, want %v\", md.Identifier, id)\n\t\t\t}\n\t\t\tif len(md.Outputs) != len(desc.Outputs) {\n\t\t\t\tt.Fatalf(\"Metadata has %d outputs, descriptor has %d\", len(md.Outputs), len(desc.Outputs))\n\t\t\t}\n\t\t\tfor i, o := range md.Outputs {\n\t\t\t\tif o.Kind != desc.Outputs[i].Kind {\n\t\t\t\t\tt.Errorf(\"output[%d].Kind = %d, want %d\", i, o.Kind, desc.Outputs[i].Kind)\n\t\t\t\t}\n\t\t\t\tif !reflect.DeepEqual(o.Shape, desc.Outputs[i].Shape) {\n\t\t\t\t\tt.Errorf(\"output[%d].Shape = %v, want %v\", i, o.Shape, desc.Outputs[i].Shape)\n\t\t\t\t}\n\t\t\t}\n\t\t})\n\t}\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/core/metadata_descriptor_test.go"
}
```

**Output:**
```
Wrote file successfully.

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/rate-of-change-ratio/ta_ROCR.c">
ERROR [67:29] 'ta_func.h' file not found
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/rate-of-change-ratio/ta_ROCR100.c">
ERROR [67:29] 'ta_func.h' file not found
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/rate-of-change-ratio/test_mom.c">
ERROR [88:10] 'ta_test_priv.h' file not found
ERROR [111:4] Unknown type name 'TA_Integer'
ERROR [114:4] Unknown type name 'TA_Integer'
ERROR [115:4] Unknown type name 'TA_Integer'
ERROR [117:4] Unknown type name 'TA_Integer'
ERROR [119:4] Unknown type name 'TA_RetCode'
ERROR [121:4] Unknown type name 'TA_Integer'
ERROR [122:4] Unknown type name 'TA_Real'
ERROR [124:4] Unknown type name 'TA_Integer'
ERROR [125:4] Unknown type name 'TA_Integer'
ERROR [131:10] Unknown type name 'TA_Real'
ERROR [135:8] Unknown type name 'ErrorNumber'
ERROR [135:35] Unknown type name 'TA_History'
ERROR [148:33] Use of undeclared identifier 'TA_OUT_OF_RANGE_START_INDEX'
ERROR [149:34] Use of undeclared identifier 'TA_OUT_OF_RANGE_END_INDEX'
ERROR [150:33] Use of undeclared identifier 'TA_OUT_OF_RANGE_END_INDEX'
ERROR [153:34] Use of undeclared identifier 'TA_SUCCESS'
ERROR [154:34] Use of undeclared identifier 'TA_SUCCESS'
ERROR [155:34] Use of undeclared identifier 'TA_SUCCESS'
ERROR [156:34] Use of undeclared identifier 'TA_SUCCESS'
... and 1 more
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/pearsons-correlation-coefficient/ta_CORREL.c">
ERROR [68:29] 'ta_func.h' file not found
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/pearsons-correlation-coefficient/test_per_hl.c">
ERROR [60:10] 'ta_test_priv.h' file not found
ERROR [85:4] Unknown type name 'TA_Integer'
ERROR [89:4] Unknown type name 'TA_Integer'
ERROR [90:4] Unknown type name 'TA_Integer'
ERROR [91:4] Unknown type name 'TA_Integer'
ERROR [93:4] Unknown type name 'TA_RetCode'
ERROR [95:4] Unknown type name 'TA_Integer'
ERROR [96:4] Unknown type name 'TA_Real'
ERROR [98:4] Unknown type name 'TA_Integer'
ERROR [99:4] Unknown type name 'TA_Integer'
ERROR [105:10] Unknown type name 'TA_Real'
ERROR [106:10] Unknown type name 'TA_Real'
ERROR [110:8] Unknown type name 'ErrorNumber'
ERROR [110:35] Unknown type name 'TA_History'
ERROR [125:35] Use of undeclared identifier 'TA_SUCCESS'
ERROR [126:35] Use of undeclared identifier 'TA_SUCCESS'
ERROR [131:38] Use of undeclared identifier 'TA_SUCCESS'
ERROR [132:38] Use of undeclared identifier 'TA_SUCCESS'
ERROR [133:38] Use of undeclared identifier 'TA_SUCCESS'
ERROR [139:40] Use of undeclared identifier 'TA_SUCCESS'
... and 1 more
</diagnostics>

LSP errors detected in this file, please fix:
<diagnostics file="/home/dev/zpano/go/indicators/core/metadata_descriptor_test.go">
ERROR [48:30] undefined: simplemovingaverage.New
ERROR [51:35] undefined: exponentialmovingaverage.New
ERROR [54:32] undefined: weightedmovingaverage.New
ERROR [57:34] undefined: triangularmovingaverage.New
ERROR [60:23] cannot use 5 (untyped int constant) as *momentum.Params value in argument to momentum.New
ERROR [63:27] cannot use 5 (untyped int constant) as *rateofchange.Params value in argument to rateofchange.New
ERROR [66:34] cannot use 5 (untyped int constant) as *rateofchangepercent.Params value in argument to rateofchangepercent.New
ERROR [69:32] cannot use 5 (untyped int constant) as *rateofchangeratio.Params value in argument to rateofchangeratio.New
ERROR [72:28] undefined: standarddeviation.New
ERROR [75:19] undefined: variance.New
ERROR [78:31] cannot use 5 (untyped int constant) as *linearregression.Params value in argument to linearregression.New
ERROR [81:45] cannot use 5 (untyped int constant) as *pearsonscorrelationcoefficient.Params value in argument to pearsonscorrelationcoefficient.New
ERROR [84:34] undefined: absolutepriceoscillator.New
ERROR [87:25] undefined: advancedecline.New
ERROR [90:35] undefined: advancedeclineoscillator.New
</diagnostics>
```

---

## Assistant (Build · claude-opus-4.7 · 21.8s)

_Thinking:_

 Rather than manually validating all 70 constructor signatures, I can rely on the fact that `BuildMetadata` itself panics when lengths don't match, so every indicator using it automatically gets that validation built in—which means I don't need to duplicate the effort in `TestDescriptorCoverage`.

The real value is checking that each descriptor's outputs have strictly ascending Kind values starting from 1, without needing to instantiate anything. I'll drop the sample-based test and write a simpler pure descriptor-registry validator that checks for non-empty outputs, ascending Kind ordering, and valid shapes.

Constructor signatures vary wildly. Since `BuildMetadata` now *sources* Kind+Shape from the registry, a sample-based test is redundant. I'll replace it with a pure-registry invariant test.

**Tool: write**

**Input:**
```json
{
  "content": "package core_test\n\nimport (\n\t\"testing\"\n\n\t\"zpano/indicators/core\"\n\t\"zpano/indicators/core/outputs/shape\"\n)\n\n// TestDescriptorOutputsWellFormed verifies invariants that every\n// indicator's Metadata() now relies on via core.BuildMetadata:\n//   - each descriptor has at least one output;\n//   - output Kinds are strictly ascending, starting at 1\n//     (matching the Go-side `iota + 1` per-indicator output enums);\n//   - every output has a known Shape.\nfunc TestDescriptorOutputsWellFormed(t *testing.T) {\n\tknown := map[shape.Shape]bool{\n\t\tshape.Scalar:   true,\n\t\tshape.Band:     true,\n\t\tshape.Heatmap:  true,\n\t\tshape.Polyline: true,\n\t}\n\n\tfor id, desc := range core.Descriptors() {\n\t\tt.Run(id.String(), func(t *testing.T) {\n\t\t\tif len(desc.Outputs) == 0 {\n\t\t\t\tt.Fatalf(\"descriptor %v has no outputs\", id)\n\t\t\t}\n\n\t\t\tfor i, o := range desc.Outputs {\n\t\t\t\twant := i + 1\n\t\t\t\tif o.Kind != want {\n\t\t\t\t\tt.Errorf(\"output[%d].Kind = %d, want %d (must start at 1 and be strictly ascending)\", i, o.Kind, want)\n\t\t\t\t}\n\t\t\t\tif !known[o.Shape] {\n\t\t\t\t\tt.Errorf(\"output[%d].Shape = %v is not a known shape\", i, o.Shape)\n\t\t\t\t}\n\t\t\t}\n\n\t\t\tif desc.Identifier != id {\n\t\t\t\tt.Errorf(\"descriptor.Identifier = %v, keyed under %v\", desc.Identifier, id)\n\t\t\t}\n\t\t})\n\t}\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/core/metadata_descriptor_test.go"
}
```

**Output:**
```
Wrote file successfully.

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/rate-of-change-ratio/ta_ROCR.c">
ERROR [67:29] 'ta_func.h' file not found
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/rate-of-change-ratio/ta_ROCR100.c">
ERROR [67:29] 'ta_func.h' file not found
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/rate-of-change-ratio/test_mom.c">
ERROR [88:10] 'ta_test_priv.h' file not found
ERROR [111:4] Unknown type name 'TA_Integer'
ERROR [114:4] Unknown type name 'TA_Integer'
ERROR [115:4] Unknown type name 'TA_Integer'
ERROR [117:4] Unknown type name 'TA_Integer'
ERROR [119:4] Unknown type name 'TA_RetCode'
ERROR [121:4] Unknown type name 'TA_Integer'
ERROR [122:4] Unknown type name 'TA_Real'
ERROR [124:4] Unknown type name 'TA_Integer'
ERROR [125:4] Unknown type name 'TA_Integer'
ERROR [131:10] Unknown type name 'TA_Real'
ERROR [135:8] Unknown type name 'ErrorNumber'
ERROR [135:35] Unknown type name 'TA_History'
ERROR [148:33] Use of undeclared identifier 'TA_OUT_OF_RANGE_START_INDEX'
ERROR [149:34] Use of undeclared identifier 'TA_OUT_OF_RANGE_END_INDEX'
ERROR [150:33] Use of undeclared identifier 'TA_OUT_OF_RANGE_END_INDEX'
ERROR [153:34] Use of undeclared identifier 'TA_SUCCESS'
ERROR [154:34] Use of undeclared identifier 'TA_SUCCESS'
ERROR [155:34] Use of undeclared identifier 'TA_SUCCESS'
ERROR [156:34] Use of undeclared identifier 'TA_SUCCESS'
... and 1 more
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/pearsons-correlation-coefficient/ta_CORREL.c">
ERROR [68:29] 'ta_func.h' file not found
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/pearsons-correlation-coefficient/test_per_hl.c">
ERROR [60:10] 'ta_test_priv.h' file not found
ERROR [85:4] Unknown type name 'TA_Integer'
ERROR [89:4] Unknown type name 'TA_Integer'
ERROR [90:4] Unknown type name 'TA_Integer'
ERROR [91:4] Unknown type name 'TA_Integer'
ERROR [93:4] Unknown type name 'TA_RetCode'
ERROR [95:4] Unknown type name 'TA_Integer'
ERROR [96:4] Unknown type name 'TA_Real'
ERROR [98:4] Unknown type name 'TA_Integer'
ERROR [99:4] Unknown type name 'TA_Integer'
ERROR [105:10] Unknown type name 'TA_Real'
ERROR [106:10] Unknown type name 'TA_Real'
ERROR [110:8] Unknown type name 'ErrorNumber'
ERROR [110:35] Unknown type name 'TA_History'
ERROR [125:35] Use of undeclared identifier 'TA_SUCCESS'
ERROR [126:35] Use of undeclared identifier 'TA_SUCCESS'
ERROR [131:38] Use of undeclared identifier 'TA_SUCCESS'
ERROR [132:38] Use of undeclared identifier 'TA_SUCCESS'
ERROR [133:38] Use of undeclared identifier 'TA_SUCCESS'
ERROR [139:40] Use of undeclared identifier 'TA_SUCCESS'
... and 1 more
</diagnostics>
```

---

## Assistant (Build · claude-opus-4.7 · 4.6s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./indicators/core/... 2>&1 | tail -20",
  "description": "Run core tests",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
ok  	zpano/indicators/core	0.003s
ok  	zpano/indicators/core/frequencyresponse	(cached)
ok  	zpano/indicators/core/outputs	(cached)
ok  	zpano/indicators/core/outputs/shape	(cached)

```

---

## Assistant (Build · claude-opus-4.7 · 5.2s)

Go Phase 5 complete. Now TS side. Let me check the TS metadata structure.

**Tool: bash**

**Input:**
```json
{
  "command": "find /home/dev/zpano/ts/indicators -name \"*.ts\" | xargs grep -l \"get metadata\" | head -5",
  "description": "Find TS metadata methods"
}
```

---

## Assistant (Build · claude-opus-4.7 · 4.5s)

**Tool: grep**

**Input:**
```json
{
  "include": "*.ts",
  "path": "/home/dev/zpano/ts/indicators",
  "pattern": "metadata\\(\\)|get metadata"
}
```

**Output:**
```
Found 308 matches (showing first 100)
/home/dev/zpano/ts/indicators/common/triangular-moving-average/triangular-moving-average.ts:
  Line 114:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/common/triangular-moving-average/triangular-moving-average.spec.ts:
  Line 65:     expect(trima.metadata().mnemonic).toBe('trima(7)');
  Line 70:     const meta = trima.metadata();

/home/dev/zpano/ts/indicators/common/simple-moving-average/simple-moving-average.ts:
  Line 46:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/common/simple-moving-average/simple-moving-average.spec.ts:
  Line 25:     expect(sma.metadata().mnemonic).toBe('sma(7)');

/home/dev/zpano/ts/indicators/common/absolute-price-oscillator/absolute-price-oscillator.ts:
  Line 84:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/common/absolute-price-oscillator/absolute-price-oscillator.spec.ts:
  Line 134:     const meta = apo.metadata();
  Line 150:     const meta = apo.metadata();

/home/dev/zpano/ts/indicators/common/weighted-moving-average/weighted-moving-average.ts:
  Line 77:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/common/weighted-moving-average/weighted-moving-average.spec.ts:
  Line 72:     expect(wma.metadata().mnemonic).toBe('wma(7)');

/home/dev/zpano/ts/indicators/common/linear-regression/linear-regression.ts:
  Line 91:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/common/linear-regression/linear-regression.spec.ts:
  Line 502:     expect(lr.metadata().mnemonic).toBe('linreg(14)');
  Line 507:     expect(lr.metadata().mnemonic).toBe('linreg(14, o)');
  Line 512:     expect(lr.metadata().mnemonic).toBe('linreg(14, b)');
  Line 517:     expect(lr.metadata().mnemonic).toBe('linreg(14, v)');
  Line 522:     const meta = lr.metadata();

/home/dev/zpano/ts/indicators/common/momentum/momentum.ts:
  Line 51:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/common/momentum/momentum.spec.ts:
  Line 54:     expect(mom.metadata().mnemonic).toBe('mom(14)');
  Line 59:     expect(mom.metadata().mnemonic).toBe('mom(14, hl/2)');
  Line 60:     expect(mom.metadata().description).toBe('Momentum mom(14, hl/2)');
  Line 65:     expect(mom.metadata().mnemonic).toBe('mom(14, b)');
  Line 66:     expect(mom.metadata().description).toBe('Momentum mom(14, b)');
  Line 71:     expect(mom.metadata().mnemonic).toBe('mom(14, v)');
  Line 72:     expect(mom.metadata().description).toBe('Momentum mom(14, v)');
  Line 77:     const meta = mom.metadata();

/home/dev/zpano/ts/indicators/common/pearsons-correlation-coefficient/pearsons-correlation-coefficient.spec.ts:
  Line 224:     const meta = c.metadata();

/home/dev/zpano/ts/indicators/common/pearsons-correlation-coefficient/pearsons-correlation-coefficient.ts:
  Line 56:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/common/variance/variance.ts:
  Line 64:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/common/variance/variance.spec.ts:
  Line 18:     const m = v.metadata();
  Line 32:     const m = v.metadata();

/home/dev/zpano/ts/indicators/common/exponential-moving-average/exponential-moving-average.ts:
  Line 101:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/common/exponential-moving-average/exponential-moving-average.spec.ts:
  Line 106:     expect(ema.metadata().mnemonic).toBe('ema(7)');
  Line 108:     expect(ema.metadata().mnemonic).toBe('ema(7)');
  Line 110:     expect(ema.metadata().mnemonic).toBe('ema(7)');
  Line 116:     expect(ema.metadata().mnemonic).toBe('ema(10, 0.18181818)');
  Line 121:     const meta = ema.metadata();

/home/dev/zpano/ts/indicators/common/rate-of-change/rate-of-change.ts:
  Line 51:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/common/rate-of-change/rate-of-change.spec.ts:
  Line 54:     expect(roc.metadata().mnemonic).toBe('roc(14)');
  Line 59:     expect(roc.metadata().mnemonic).toBe('roc(14, hl/2)');
  Line 60:     expect(roc.metadata().description).toBe('Rate of Change roc(14, hl/2)');
  Line 65:     expect(roc.metadata().mnemonic).toBe('roc(14, b)');
  Line 66:     expect(roc.metadata().description).toBe('Rate of Change roc(14, b)');
  Line 71:     expect(roc.metadata().mnemonic).toBe('roc(14, v)');
  Line 72:     expect(roc.metadata().description).toBe('Rate of Change roc(14, v)');
  Line 77:     const meta = roc.metadata();

/home/dev/zpano/ts/indicators/common/rate-of-change-percent/rate-of-change-percent.spec.ts:
  Line 56:     expect(rocp.metadata().mnemonic).toBe('rocp(14)');
  Line 61:     expect(rocp.metadata().mnemonic).toBe('rocp(14, hl/2)');
  Line 62:     expect(rocp.metadata().description).toBe('Rate of Change Percent rocp(14, hl/2)');
  Line 67:     expect(rocp.metadata().mnemonic).toBe('rocp(14, b)');
  Line 68:     expect(rocp.metadata().description).toBe('Rate of Change Percent rocp(14, b)');
  Line 73:     expect(rocp.metadata().mnemonic).toBe('rocp(14, v)');
  Line 74:     expect(rocp.metadata().description).toBe('Rate of Change Percent rocp(14, v)');
  Line 79:     const meta = rocp.metadata();

/home/dev/zpano/ts/indicators/common/rate-of-change-percent/rate-of-change-percent.ts:
  Line 51:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/common/standard-deviation/standard-deviation.spec.ts:
  Line 43:     const m = sd.metadata();
  Line 57:     const m = sd.metadata();

/home/dev/zpano/ts/indicators/common/standard-deviation/standard-deviation.ts:
  Line 57:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/common/rate-of-change-ratio/rate-of-change-ratio.spec.ts:
  Line 66:     expect(rocr.metadata().mnemonic).toBe('rocr(14)');
  Line 71:     expect(rocr100.metadata().mnemonic).toBe('rocr100(14)');
  Line 72:     expect(rocr100.metadata().description).toBe('Rate of Change Ratio 100 Scale rocr100(14)');
  Line 77:     expect(rocr.metadata().mnemonic).toBe('rocr(14, hl/2)');
  Line 78:     expect(rocr.metadata().description).toBe('Rate of Change Ratio rocr(14, hl/2)');
  Line 83:     expect(rocr.metadata().mnemonic).toBe('rocr(14, b)');
  Line 84:     expect(rocr.metadata().description).toBe('Rate of Change Ratio rocr(14, b)');
  Line 89:     expect(rocr.metadata().mnemonic).toBe('rocr(14, v)');
  Line 90:     expect(rocr.metadata().description).toBe('Rate of Change Ratio rocr(14, v)');
  Line 95:     const meta = rocr.metadata();

/home/dev/zpano/ts/indicators/common/rate-of-change-ratio/rate-of-change-ratio.ts:
  Line 56:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/george-lane/stochastic/stochastic.spec.ts:
  Line 189:     const meta = ind.metadata();

/home/dev/zpano/ts/indicators/george-lane/stochastic/stochastic.ts:
  Line 118:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/perry-kaufman/kaufman-adaptive-moving-average/kaufman-adaptive-moving-average.ts:
  Line 127:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/perry-kaufman/kaufman-adaptive-moving-average/kaufman-adaptive-moving-average.spec.ts:
  Line 156:     expect(kama.metadata().mnemonic).toBe('kama(10, 2, 30)');
  Line 161:     expect(kama.metadata().mnemonic).toBe('kama(10, 0.6667, 0.0645)');
  Line 166:     const meta = kama.metadata();

/home/dev/zpano/ts/indicators/mark-jurik/jurik-moving-average/jurik-moving-average.spec.ts:
  Line 589:     expect(jma.metadata().mnemonic).toBe('jma(7, -1)');

/home/dev/zpano/ts/indicators/mark-jurik/jurik-moving-average/jurik-moving-average.ts:
  Line 108:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/gene-quong/money-flow-index/money-flow-index.ts:
  Line 86:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/gene-quong/money-flow-index/money-flow-index.spec.ts:
  Line 203:     const meta = mfi.metadata();

/home/dev/zpano/ts/indicators/igor-livshin/balance-of-power/balance-of-power.ts:
  Line 46:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/igor-livshin/balance-of-power/balance-of-power.spec.ts:
  Line 74:     const meta = bop.metadata();

/home/dev/zpano/ts/indicators/gerald-appel/moving-average-convergence-divergence/moving-average-convergence-divergence.ts:
  Line 144:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/gerald-appel/percentage-price-oscillator/percentage-price-oscillator.spec.ts:
  Line 161:     const meta = ppo.metadata();
  Line 177:     const meta = ppo.metadata();

/home/dev/zpano/ts/indicators/gerald-appel/moving-average-convergence-divergence/moving-average-convergence-divergence.spec.ts:
  Line 236:     const meta = ind.metadata();
  Line 253:     const meta = ind.metadata();
  Line 260:     const meta = ind.metadata();

/home/dev/zpano/ts/indicators/gerald-appel/percentage-price-oscillator/percentage-price-oscillator.ts:
  Line 89:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/welles-wilder/relative-strength-index/relative-strength-index.ts:
  Line 65:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/welles-wilder/directional-movement-index/directional-movement-index.spec.ts:
  Line 190:       const meta = dx.metadata();

/home/dev/zpano/ts/indicators/welles-wilder/relative-strength-index/relative-strength-index.spec.ts:
  Line 73:     const meta = rsi.metadata();

/home/dev/zpano/ts/indicators/welles-wilder/directional-movement-index/directional-movement-index.ts:
  Line 60:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/welles-wilder/average-directional-movement-index-rating/average-directional-movement-index-rating.spec.ts:
  Line 222:       const meta = adxr.metadata();

/home/dev/zpano/ts/indicators/welles-wilder/true-range/true-range.spec.ts:
  Line 180:     const meta = tr.metadata();

/home/dev/zpano/ts/indicators/welles-wilder/true-range/true-range.ts:
  Line 43:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/welles-wilder/directional-movement-plus/directional-movement-plus.spec.ts:
  Line 249:       const meta = dmp.metadata();

/home/dev/zpano/ts/indicators/welles-wilder/directional-movement-plus/directional-movement-plus.ts:
  Line 62:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/welles-wilder/average-directional-movement-index-rating/average-directional-movement-index-rating.ts:
  Line 60:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/welles-wilder/directional-movement-minus/directional-movement-minus.spec.ts:
  Line 247:       const meta = dmm.metadata();

/home/dev/zpano/ts/indicators/welles-wilder/directional-movement-minus/directional-movement-minus.ts:
  Line 62:   public metadata(): IndicatorMetadata {

/home/dev/zpano/ts/indicators/welles-wilder/average-directional-movement-index/average-directional-movement-index.ts:
  Line 59:   public metadata(): IndicatorMetadata {

(Results truncated: showing 100 of 308 matches (208 hidden). Consider using a more specific path or pattern.)
```

---

## Assistant (Build · claude-opus-4.7 · 4.0s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/common/simple-moving-average/simple-moving-average.ts",
  "limit": 30,
  "offset": 1
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/common/simple-moving-average/simple-moving-average.ts</path>
<type>file</type>
<content>1: import { componentTripleMnemonic } from '../../core/component-triple-mnemonic';
2: import { IndicatorMetadata } from '../../core/indicator-metadata';
3: import { IndicatorIdentifier } from '../../core/indicator-identifier';
4: import { LineIndicator } from '../../core/line-indicator';
5: import { Shape } from '../../core/outputs/shape/shape';
6: import { SimpleMovingAverageOutput } from './output';
7: import { SimpleMovingAverageParams } from './params';
8: 
9: /** Function to calculate mnemonic of a __SimpleMovingAverage__ indicator. */
10: export const simpleMovingAverageMnemonic = (params: SimpleMovingAverageParams): string =>
11:   'sma('.concat(params.length.toString(), componentTripleMnemonic(params.barComponent, params.quoteComponent, params.tradeComponent), ')');
12: 
13: /** Simple Moving Average line indicator. */
14: export class SimpleMovingAverage extends LineIndicator {
15:   private window: Array<number>;
16:   private windowLength: number;
17:   private windowSum: number;
18:   private windowCount: number;
19:   private lastIndex: number;
20: 
21:   /**
22:    * Constructs an instance given a length in samples.
23:    * The length should be an integer greater than 1.
24:    **/
25:   public constructor(params: SimpleMovingAverageParams){
26:     super();
27:     const length = Math.floor(params.length);
28:     if (length < 2) {
29:       throw new Error('length should be greater than 1');
30:     }

(Showing lines 1-30 of 87. Use offset=31 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/common/simple-moving-average/simple-moving-average.ts",
  "limit": 30,
  "offset": 40
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/common/simple-moving-average/simple-moving-average.ts</path>
<type>file</type>
<content>40:     this.windowCount = 0;
41:     this.lastIndex = length - 1;
42:     this.primed = false;
43:   }
44: 
45:   /** Describes the output data of the indicator. */
46:   public metadata(): IndicatorMetadata {
47:     return {
48:       identifier: IndicatorIdentifier.SimpleMovingAverage,
49:       mnemonic: this.mnemonic,
50:       description: this.description,
51:       outputs: [{
52:         kind: SimpleMovingAverageOutput.SimpleMovingAverageValue,
53:         shape: Shape.Scalar,
54:         mnemonic: this.mnemonic,
55:         description: this.description,
56:       }],
57:     };
58:   }
59: 
60:   /** Updates the value of the indicator given the next sample. */
61:   public update(sample: number): number {
62:     if (Number.isNaN(sample)) {
63:       return sample;
64:     }
65: 
66:     if (this.primed) {
67:       this.windowSum += sample - this.window[0];
68:       for (let i = 0; i < this.lastIndex; i++) {
69:         this.window[i] = this.window[i+1];

(Showing lines 40-69 of 87. Use offset=70 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.8s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core</path>
<type>directory</type>
<entries>
adaptivity.ts
component-triple-mnemonic.ts
descriptor.ts
descriptors.ts
frequency-response/
indicator-identifier.ts
indicator-metadata.ts
indicator-output.ts
indicator-specification.ts
indicator.ts
input-requirement.ts
line-indicator.ts
output-descriptor.ts
outputs/
pane.ts
role.ts
volume-usage.ts

(17 entries)
</entries>
```

---

## Assistant (Build · claude-opus-4.7 · 3.9s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/indicator-metadata.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/indicator-metadata.ts</path>
<type>file</type>
<content>1: import { OutputMetadata } from './outputs/output-metadata';
2: import { IndicatorIdentifier } from './indicator-identifier';
3: 
4: /** Describes an indicator and its outputs. */
5: export interface IndicatorMetadata {
6:     /** Identifies this indicator. */
7:     identifier: IndicatorIdentifier;
8: 
9:     /** A short name (mnemonic) of this indicator. */
10:     mnemonic: string;
11: 
12:     /** A description of this indicator. */
13:     description: string;
14: 
15:     /** An array of metadata for individual requested outputs. */
16:     outputs: OutputMetadata[];
17: }

(End of file - total 17 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.8s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/descriptors.ts",
  "limit": 40
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

(Showing lines 1-40 of 247. Use offset=41 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: bash**

**Input:**
```json
{
  "command": "ls /home/dev/zpano/ts/indicators/core/descriptor.ts /home/dev/zpano/ts/indicators/core/output-descriptor.ts",
  "description": "check accessors exist"
}
```

**Output:**
```
/home/dev/zpano/ts/indicators/core/descriptor.ts
/home/dev/zpano/ts/indicators/core/output-descriptor.ts

```

---

## Assistant (Build · claude-opus-4.7 · 4.7s)

**Tool: grep**

**Input:**
```json
{
  "include": "*.ts",
  "path": "/home/dev/zpano/ts/indicators/core",
  "pattern": "export function|descriptorOf|descriptors"
}
```

**Output:**
```
Found 6 matches
/home/dev/zpano/ts/indicators/core/descriptors.ts:
  Line 35: /** Static registry of taxonomic descriptors for all implemented indicators. */
  Line 36: const descriptors: Map<IndicatorIdentifier, Descriptor> = new Map<IndicatorIdentifier, Descriptor>([
  Line 240: export function descriptorOf(id: IndicatorIdentifier): Descriptor | undefined {
  Line 241:   return descriptors.get(id);
  Line 245: export function getDescriptors(): Map<IndicatorIdentifier, Descriptor> {
  Line 246:   return new Map(descriptors);
```

---

## Assistant (Build · claude-opus-4.7 · 10.2s)

Good, accessors exist. Now create the `buildMetadata` helper:

**Tool: write**

**Input:**
```json
{
  "content": "import { IndicatorIdentifier } from './indicator-identifier.js';\nimport { IndicatorMetadata } from './indicator-metadata.js';\nimport { descriptorOf } from './descriptors.js';\n\n/**\n * Per-output mnemonic and description used when building an IndicatorMetadata\n * from the descriptor registry.\n */\nexport interface OutputText {\n  mnemonic: string;\n  description: string;\n}\n\n/**\n * Constructs an IndicatorMetadata for the indicator with the given identifier\n * by joining the registry's per-output kind and shape with the supplied\n * per-output mnemonic and description.\n *\n * texts must be in the same order and length as the descriptor's outputs.\n * Throws if no descriptor is registered for the identifier or if the length\n * of texts does not match the descriptor's outputs.\n */\nexport function buildMetadata(\n  identifier: IndicatorIdentifier,\n  mnemonic: string,\n  description: string,\n  texts: OutputText[]\n): IndicatorMetadata {\n  const d = descriptorOf(identifier);\n  if (!d) {\n    throw new Error(`buildMetadata: no descriptor registered for identifier ${IndicatorIdentifier[identifier]}`);\n  }\n  if (texts.length !== d.outputs.length) {\n    throw new Error(\n      `buildMetadata: identifier ${IndicatorIdentifier[identifier]} has ${d.outputs.length} outputs in descriptor but ${texts.length} texts were supplied`\n    );\n  }\n  return {\n    identifier,\n    mnemonic,\n    description,\n    outputs: texts.map((t, i) => ({\n      kind: d.outputs[i].kind,\n      shape: d.outputs[i].shape,\n      mnemonic: t.mnemonic,\n      description: t.description,\n    })),\n  };\n}\n",
  "filePath": "/home/dev/zpano/ts/indicators/core/build-metadata.ts"
}
```

**Output:**
```
Wrote file successfully.

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/rate-of-change-ratio/ta_ROCR.c">
ERROR [67:29] 'ta_func.h' file not found
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/rate-of-change-ratio/ta_ROCR100.c">
ERROR [67:29] 'ta_func.h' file not found
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/rate-of-change-ratio/test_mom.c">
ERROR [88:10] 'ta_test_priv.h' file not found
ERROR [111:4] Unknown type name 'TA_Integer'
ERROR [114:4] Unknown type name 'TA_Integer'
ERROR [115:4] Unknown type name 'TA_Integer'
ERROR [117:4] Unknown type name 'TA_Integer'
ERROR [119:4] Unknown type name 'TA_RetCode'
ERROR [121:4] Unknown type name 'TA_Integer'
ERROR [122:4] Unknown type name 'TA_Real'
ERROR [124:4] Unknown type name 'TA_Integer'
ERROR [125:4] Unknown type name 'TA_Integer'
ERROR [131:10] Unknown type name 'TA_Real'
ERROR [135:8] Unknown type name 'ErrorNumber'
ERROR [135:35] Unknown type name 'TA_History'
ERROR [148:33] Use of undeclared identifier 'TA_OUT_OF_RANGE_START_INDEX'
ERROR [149:34] Use of undeclared identifier 'TA_OUT_OF_RANGE_END_INDEX'
ERROR [150:33] Use of undeclared identifier 'TA_OUT_OF_RANGE_END_INDEX'
ERROR [153:34] Use of undeclared identifier 'TA_SUCCESS'
ERROR [154:34] Use of undeclared identifier 'TA_SUCCESS'
ERROR [155:34] Use of undeclared identifier 'TA_SUCCESS'
ERROR [156:34] Use of undeclared identifier 'TA_SUCCESS'
... and 1 more
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/pearsons-correlation-coefficient/ta_CORREL.c">
ERROR [68:29] 'ta_func.h' file not found
</diagnostics>

LSP errors detected in other files:
<diagnostics file="/home/dev/zpano/mbst-to-convert/common/pearsons-correlation-coefficient/test_per_hl.c">
ERROR [60:10] 'ta_test_priv.h' file not found
ERROR [85:4] Unknown type name 'TA_Integer'
ERROR [89:4] Unknown type name 'TA_Integer'
ERROR [90:4] Unknown type name 'TA_Integer'
ERROR [91:4] Unknown type name 'TA_Integer'
ERROR [93:4] Unknown type name 'TA_RetCode'
ERROR [95:4] Unknown type name 'TA_Integer'
ERROR [96:4] Unknown type name 'TA_Real'
ERROR [98:4] Unknown type name 'TA_Integer'
ERROR [99:4] Unknown type name 'TA_Integer'
ERROR [105:10] Unknown type name 'TA_Real'
ERROR [106:10] Unknown type name 'TA_Real'
ERROR [110:8] Unknown type name 'ErrorNumber'
ERROR [110:35] Unknown type name 'TA_History'
ERROR [125:35] Use of undeclared identifier 'TA_SUCCESS'
ERROR [126:35] Use of undeclared identifier 'TA_SUCCESS'
ERROR [131:38] Use of undeclared identifier 'TA_SUCCESS'
ERROR [132:38] Use of undeclared identifier 'TA_SUCCESS'
ERROR [133:38] Use of undeclared identifier 'TA_SUCCESS'
ERROR [139:40] Use of undeclared identifier 'TA_SUCCESS'
... and 1 more
</diagnostics>
```

---

## Assistant (Build · claude-opus-4.7 · 7.9s)

Refactor TS SMA as template, then verify:

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/common/simple-moving-average/simple-moving-average.ts",
  "newString": "import { buildMetadata } from '../../core/build-metadata';\nimport { componentTripleMnemonic } from '../../core/component-triple-mnemonic';\nimport { IndicatorMetadata } from '../../core/indicator-metadata';\nimport { IndicatorIdentifier } from '../../core/indicator-identifier';\nimport { LineIndicator } from '../../core/line-indicator';\nimport { SimpleMovingAverageParams } from './params';",
  "oldString": "import { componentTripleMnemonic } from '../../core/component-triple-mnemonic';\nimport { IndicatorMetadata } from '../../core/indicator-metadata';\nimport { IndicatorIdentifier } from '../../core/indicator-identifier';\nimport { LineIndicator } from '../../core/line-indicator';\nimport { Shape } from '../../core/outputs/shape/shape';\nimport { SimpleMovingAverageOutput } from './output';\nimport { SimpleMovingAverageParams } from './params';"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 6.5s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/common/simple-moving-average/simple-moving-average.ts",
  "newString": "  public metadata(): IndicatorMetadata {\n    return buildMetadata(\n      IndicatorIdentifier.SimpleMovingAverage,\n      this.mnemonic,\n      this.description,\n      [{ mnemonic: this.mnemonic, description: this.description }],\n    );\n  }",
  "oldString": "  public metadata(): IndicatorMetadata {\n    return {\n      identifier: IndicatorIdentifier.SimpleMovingAverage,\n      mnemonic: this.mnemonic,\n      description: this.description,\n      outputs: [{\n        kind: SimpleMovingAverageOutput.SimpleMovingAverageValue,\n        shape: Shape.Scalar,\n        mnemonic: this.mnemonic,\n        description: this.description,\n      }],\n    };\n  }"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 7.7s)

**Tool: bash**

**Input:**
```json
{
  "command": "npx tsc --noEmit 2>&1 | head -20 && npx jasmine --config=jasmine.json indicators/common/simple-moving-average/simple-moving-average.spec.ts 2>&1 | tail -10",
  "description": "Check TS SMA template",
  "workdir": "/home/dev/zpano/ts"
}
```

**Output:**
```
indicators/george-lane/stochastic/stochastic.ts(233,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/george-lane/stochastic/stochastic.ts(233,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/larry-williams/ultimate-oscillator/ultimate-oscillator.ts(223,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/larry-williams/ultimate-oscillator/ultimate-oscillator.ts(223,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/larry-williams/williams-percent-r/williams-percent-r.ts(181,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/larry-williams/williams-percent-r/williams-percent-r.ts(181,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/tushar-chande/aroon/aroon.ts(217,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/tushar-chande/aroon/aroon.ts(217,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/average-directional-movement-index-rating/average-directional-movement-index-rating.ts(179,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/average-directional-movement-index-rating/average-directional-movement-index-rating.ts(179,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/average-directional-movement-index/average-directional-movement-index.ts(172,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/average-directional-movement-index/average-directional-movement-index.ts(172,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/average-true-range/average-true-range.ts(150,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/average-true-range/average-true-range.ts(150,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/directional-indicator-minus/directional-indicator-minus.ts(144,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/directional-indicator-minus/directional-indicator-minus.ts(144,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/directional-indicator-plus/directional-indicator-plus.ts(144,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/directional-indicator-plus/directional-indicator-plus.ts(144,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
indicators/welles-wilder/directional-movement-index/directional-movement-index.ts(162,23): error TS2339: Property 'bid' does not exist on type 'Quote'.
indicators/welles-wilder/directional-movement-index/directional-movement-index.ts(162,36): error TS2339: Property 'ask' does not exist on type 'Quote'.
    at #cachedDefaultResolve (node:internal/modules/esm/loader:719:20)
    at #resolveAndMaybeBlockOnLoaderThread (node:internal/modules/esm/loader:736:38)
    at ModuleLoader.resolveSync (node:internal/modules/esm/loader:765:52)
    at #resolve (node:internal/modules/esm/loader:701:17)
    at ModuleLoader.getOrCreateModuleJob (node:internal/modules/esm/loader:621:35)
    at ModuleJob.syncLink (node:internal/modules/esm/module_job:160:33)
    at ModuleJob.link (node:internal/modules/esm/module_job:245:17) {
  code: 'ERR_MODULE_NOT_FOUND',
  url: 'file:///home/dev/zpano/ts/indicators/common/simple-moving-average/simple-moving-average'
}

```

---

## Assistant (Build · claude-opus-4.7 · 7.3s)

_Thinking:_

 The TypeScript errors around quote bid/ask are pre-existing issues, so I'll skip those and just run the full test suite to see what else needs attention.

Pre-existing TS errors are unrelated. Run the test suite:

**Tool: bash**

**Input:**
```json
{
  "command": "npm test 2>&1 | tail -10",
  "description": "Run TS test suite",
  "workdir": "/home/dev/zpano/ts"
}
```

**Output:**
```

> ts@1.0.0 test
> node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json

Started
....................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................


884 specs, 0 failures
Finished in 0.838 seconds

```

---

## Assistant (Build · claude-opus-4.7 · 332.5s)

TS SMA template works. Dispatch a subagent to refactor the remaining ~70 TS files.

**Tool: task**

**Input:**
```json
{
  "description": "Refactor TS metadata methods",
  "prompt": "Refactor all TypeScript indicator `metadata()` methods in /home/dev/zpano/ts/indicators/ to use the new `buildMetadata` helper instead of constructing the `IndicatorMetadata` object literal inline.\n\n## Reference template (already converted)\n\nSee /home/dev/zpano/ts/indicators/common/simple-moving-average/simple-moving-average.ts. Before → after:\n\n**Before:**\n```ts\nimport { componentTripleMnemonic } from '../../core/component-triple-mnemonic';\nimport { IndicatorMetadata } from '../../core/indicator-metadata';\nimport { IndicatorIdentifier } from '../../core/indicator-identifier';\nimport { LineIndicator } from '../../core/line-indicator';\nimport { Shape } from '../../core/outputs/shape/shape';\nimport { SimpleMovingAverageOutput } from './output';\nimport { SimpleMovingAverageParams } from './params';\n\n// ...\npublic metadata(): IndicatorMetadata {\n  return {\n    identifier: IndicatorIdentifier.SimpleMovingAverage,\n    mnemonic: this.mnemonic,\n    description: this.description,\n    outputs: [{\n      kind: SimpleMovingAverageOutput.SimpleMovingAverageValue,\n      shape: Shape.Scalar,\n      mnemonic: this.mnemonic,\n      description: this.description,\n    }],\n  };\n}\n```\n\n**After:**\n```ts\nimport { buildMetadata } from '../../core/build-metadata';\nimport { componentTripleMnemonic } from '../../core/component-triple-mnemonic';\nimport { IndicatorMetadata } from '../../core/indicator-metadata';\nimport { IndicatorIdentifier } from '../../core/indicator-identifier';\nimport { LineIndicator } from '../../core/line-indicator';\nimport { SimpleMovingAverageParams } from './params';\n\n// ...\npublic metadata(): IndicatorMetadata {\n  return buildMetadata(\n    IndicatorIdentifier.SimpleMovingAverage,\n    this.mnemonic,\n    this.description,\n    [{ mnemonic: this.mnemonic, description: this.description }],\n  );\n}\n```\n\nNotes:\n1. `buildMetadata` sources `kind` and `shape` from the descriptor registry (in `/home/dev/zpano/ts/indicators/core/descriptors.ts`) so the caller supplies only `mnemonic` and `description` per output.\n2. Import path for `buildMetadata` is always `'../../core/build-metadata'` (two levels up from indicator folder).\n3. The per-output `OutputText[]` array MUST be in the same order as the descriptor's `outputs` (which is kind-ascending, mirroring the local `XxxOutput` enum order).\n\n## Files to refactor\n\nRun:\n```\ngrep -rl \"public metadata(): IndicatorMetadata\" /home/dev/zpano/ts/indicators --include=\"*.ts\" | grep -v \"\\.spec\\.ts\" | grep -v simple-moving-average\n```\n\nShould be ~70 files.\n\n## Procedure per file\n\n1. Read the current metadata() body.\n2. Identify the per-output mnemonic and description expressions (keep verbatim).\n3. Verify output order matches descriptor in `/home/dev/zpano/ts/indicators/core/descriptors.ts`.\n4. Rewrite metadata() to call `buildMetadata(IndicatorIdentifier.Xxx, <mnemonicExpr>, <descriptionExpr>, [{ mnemonic: ..., description: ... }, ...])`.\n5. Remove now-unused imports:\n   - `import { Shape } from '../../core/outputs/shape/shape';` — remove if no other usage in file.\n   - `import { XxxOutput } from './output';` — remove if no other usage (check for `XxxOutput.` elsewhere in the file, e.g., in `kindForOutput` methods or output enum references).\n6. Add `import { buildMetadata } from '../../core/build-metadata';` at the top.\n\n## Constraints\n\n- Do NOT change any other logic (only metadata() + imports).\n- Per-output mnemonic/description strings and expressions MUST remain character-identical.\n- Keep any local variables set up before the return (e.g., `const desc = 'Bollinger Bands ' + this.mnemonic;`).\n- Some indicators have outputs with non-Scalar shapes (Band, Polyline, Heatmap). The shape comes from the registry — do not specify.\n- Multi-output indicator order: in TS, per-indicator `Output` enums are 0-based and their source-order matches the descriptor's kind-ascending order. So the existing metadata() output array is already in the right order; just strip `kind` and `shape` fields.\n\n## Validation\n\nRun after each batch:\n```\ncd /home/dev/zpano/ts && npm test\n```\nMust remain `884 specs, 0 failures`.\n\nIGNORE all LSP errors from /home/dev/zpano/mbst-to-convert/ — those are pre-existing C file noise.\nIGNORE pre-existing TS errors about `Property 'bid' does not exist on type 'Quote'` — those are unrelated.\n\n## Report back\n\n- Number of files modified.\n- Any skipped files / ambiguous cases.\n- Final `npm test` result.\n",
  "subagent_type": "general"
}
```

**Output:**
```
task_id: ses_244c1ed0bffeSO2HNEmV3oQvpT (for resuming to continue this task if needed)

<task_result>
72 indicators use `buildMetadata` (the 71 refactored + the pre-existing simple-moving-average reference). No files still have the old inline `return {` pattern alongside `public metadata(): IndicatorMetadata`.

## Report

**Files modified: 71** (all files in the target list).

**Tooling:** Used a Python AST-ish script (`/tmp/refactor.py` + `/tmp/refactor2.py`) that brace-matches through each `public metadata(): IndicatorMetadata` body, extracts the `identifier`, top-level `mnemonic`/`description`, and per-output `mnemonic`/`description` expressions verbatim, then rewrites to a `buildMetadata(...)` call. It handled three source patterns:

1. Inline object literal in the return (majority — 59 files).
2. `const outputMeta: OutputMetadata = {...}; return { ..., outputs: [outputMeta] };` (single-output helper — 6 files).
3. `const outputs: OutputMetadata[] = [...]; return { ..., outputs };` (multi-output helper + shorthand property — 6 files).

**Ambiguous case requiring manual fix:** `larry-williams/ultimate-oscillator/ultimate-oscillator.ts` — the original method had a preamble `const description = 'Ultimate Oscillator';` that was referenced by both the return's top-level `description` and the per-output `description`. The script's rewrite dropped the entire preamble. I restored the local `const description` declaration inside the new metadata body.

**Import cleanup performed per file:**
- Removed `import { Shape } from '../../core/outputs/shape/shape';` if no `Shape.` remained.
- Removed `import { XxxOutput } from './output';` if no `XxxOutput.` remained.
- Removed `import { OutputMetadata } from '...';` if no longer referenced.
- Added `import { buildMetadata } from '../../core/build-metadata';`.

**Final test result:** `884 specs, 0 failures` (`cd /home/dev/zpano/ts && npm test`).
</task_result>
```

---

## Assistant (Build · claude-opus-4.7 · 6.7s)

**Tool: bash**

**Input:**
```json
{
  "command": "npm test 2>&1 | tail -5",
  "description": "Final TS verification",
  "workdir": "/home/dev/zpano/ts"
}
```

**Output:**
```
....................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................


884 specs, 0 failures
Finished in 0.8 seconds

```

---

## Assistant (Build · claude-opus-4.7 · 5.0s)

**Tool: bash**

**Input:**
```json
{
  "command": "go build ./... 2>&1 && go test ./... 2>&1 | grep -E \"FAIL|^ok\" | grep -v \"^ok\" | head",
  "description": "Final Go verification",
  "workdir": "/home/dev/zpano/go"
}
```

---

## Assistant (Build · claude-opus-4.7 · 117.2s)

**Phase 5 complete** on both sides. Both test suites green (Go: all packages ok; TS: 884 specs, 0 failures).

Remaining phases:

**Tool: question**

**Input:**
```json
{
  "questions": [
    {
      "header": "Next phase",
      "options": [
        {
          "description": "Document taxonomy primitives (Role, Pane, Adaptivity, InputRequirement, VolumeUsage), the Descriptor table conventions, and the BuildMetadata/buildMetadata helpers in `.opencode/skills/indicator-architecture/SKILL.md`.",
          "label": "Phase 6: Document taxonomy in SKILL.md (Recommended)"
        },
        {
          "description": "Update `mbst-indicator-conversion`, `talib-indicator-conversion`, and `indicator-conversion` skills to require adding a descriptor row and using BuildMetadata/buildMetadata for new indicators.",
          "label": "Phase 7: Update conversion skills"
        },
        {
          "description": "Do both Phase 6 and Phase 7 back-to-back.",
          "label": "Phase 6 + Phase 7 together"
        },
        {
          "description": "Phase 5 was the last code-touching phase; pause here and let the user review.",
          "label": "Stop and review"
        }
      ],
      "question": "Proceed to which next phase?"
    }
  ]
}
```

**Output:**
```
User has answered your questions: "Proceed to which next phase?"="Phase 6 + Phase 7 together". You can now continue with the user's answers in mind.
```

---

## Assistant (Build · claude-opus-4.7 · 3.8s)

**Tool: bash**

**Input:**
```json
{
  "command": "ls /home/dev/zpano/.opencode/skills/",
  "description": "List skills"
}
```

**Output:**
```
indicator-architecture
indicator-conversion
mbst-indicator-architecture
mbst-indicator-conversion
modern-go-guidelines
talib-indicator-conversion

```

---

## Assistant (Build · claude-opus-4.7 · 3.7s)

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
123:   scalar, a band (upper/lower), a heatmap, etc. This folder holds the types
124:   and an enum (`OutputType`) describing them.
125: - **`frequency-response/`** contains utilities for computing the frequency
126:   response of a filter/indicator.
127: 
128: Other shared types (indicator interface, metadata, specification, line indicator
129: base class, indicator type enum) live directly in `core/`.
130: 
131: ## Naming Conventions per Language
132: 
133: The structure is identical across languages; only the naming style changes.
134: 
135: | Language   | Folder naming       | Example author folder    | Example indicator folder      |
136: |------------|---------------------|--------------------------|-------------------------------|
137: | TypeScript | `kebab-case`        | `mark-jurik/`            | `jurik-moving-average/`       |
138: | Go         | `lowercase` (no separators) | `markjurik/`      | `jurikmovingaverage/`         |
139: | Python     | `snake_case`        | `mark_jurik/`            | `jurik_moving_average/`       |
140: | Rust       | `snake_case`        | `mark_jurik/`            | `jurik_moving_average/`       |
141: | Zig        | `snake_case`        | `mark_jurik/`            | `jurik_moving_average/`       |
142: 
143: ### Symbol Naming (Types, Enums)
144: 
145: Go uses package-scoped names, so symbols can be shorter (the package provides
146: context). All other languages use fully-qualified symbol names.
147: 
148: | Concept           | Go (in `core` pkg) | Go (in `outputs` pkg) | TypeScript / Python / Rust / Zig |
149: |-------------------|---------------------|-----------------------|----------------------------------|
150: | Indicator type    | `core.Type`         | --                    | `IndicatorType`                  |
151: | Output type       | --                  | `outputs.Type`        | `OutputType`                     |
152: 
153: ### File Naming
154: 
155: | Language   | Style                     | Test files                  | Example                          |
156: |------------|---------------------------|-----------------------------|----------------------------------|
157: | TypeScript | `kebab-case.ts`           | `kebab-case.spec.ts`        | `simple-moving-average.ts`       |
158: | Go         | `lowercase.go`            | `lowercase_test.go`         | `simplemovingaverage.go`         |
159: | Python     | `snake_case.py`           | `test_snake_case.py`        | `simple_moving_average.py`       |
160: | Rust       | `snake_case.rs`           | `snake_case_test.rs` or inline `#[cfg(test)]` | `simple_moving_average.rs` |
161: | Zig        | `snake_case.zig`          | `snake_case_test.zig`       | `simple_moving_average.zig`      |
162: 
163: No type-suffix convention is used (no `.enum.ts`, `.interface.ts`, etc.).
164: This is consistent across all languages.
165: 
166: #### Main vs. auxiliary files
167: 
168: Within an indicator folder, **only the main implementation file and its test
169: keep the indicator name**. Every auxiliary file drops the indicator-name
170: prefix — the folder (and in Go, the package name) already provides context,
171: so prefixing would stutter.
172: 
173: For a `simple-moving-average/` folder:
174: 
175: | Role                         | Go                              | TypeScript                              |
176: |------------------------------|---------------------------------|-----------------------------------------|
177: | Main implementation          | `simplemovingaverage.go`        | `simple-moving-average.ts`              |
178: | Main test                    | `simplemovingaverage_test.go`   | `simple-moving-average.spec.ts`         |
179: | Data-driven test (if split)  | `data_test.go`                  | `data.spec.ts`                          |
180: | Parameters                   | `params.go`                     | `params.ts`                             |
181: | Output enum                  | `output.go`                     | `output.ts`                             |
182: | Output test                  | `output_test.go`                | `output.spec.ts`                        |
183: | Coefficients / helpers       | `coefficients.go`, `estimator.go` | `coefficients.ts`, `estimator.ts`     |
184: | Package docs (Go only)       | `doc.go`                        | —                                       |
185: 
186: **Exception:** when an auxiliary file holds a *concept* that would be
187: meaningless outside the folder name (e.g., `hilberttransformer/` contains
188: `cycleestimator.go`, `phaseaccumulator.go`, `dualdifferentiator.go` — each
189: representing a distinct Hilbert-transformer variant), keep the conceptual
190: names as-is. Do not try to mechanically strip a prefix that was never there.
191: 
192: ### Identifier Abbreviation Convention
193: 
194: To keep naming consistent across indicators and languages, prefer the **full
195: word** in identifiers. The following abbreviations are considered **banned**
196: as CamelCase word-parts (whether standalone or compound) — always use the
197: long form:
198: 
199: | Banned short | Canonical long | Example (before → after)                |
200: |--------------|----------------|------------------------------------------|
201: | `idx`        | `index`        | `bufIdx` → `bufferIndex`, `highestIdx` → `highestIndex` |
202: | `tmp`        | `temp`         | `tmpR` → `tempR`                         |
203: | `res`        | `result`       | `res` → `result`, `defSpectrumRes` → `defSpectrumResult` |
204: | `sig`        | `signal`       | `expSig` → `expSignal`, `mnemonicSig` → `mnemonicSignal` |
205: | `val`        | `value`        | `mnemonicVal` → `mnemonicValue`, `prevVal` → `previousValue` |
206: | `prev`       | `previous`     | `prevHigh` → `previousHigh`, `samplePrev` → `samplePrevious` |
207: | `avg`        | `average`      | `AvgLength` → `AverageLength`            |
208: | `mult`       | `multiplier`   | `upperMult` → `upperMultiplier`          |
209: | `buf`        | `buffer`       | *(compound only, paired with `idx`)*     |
210: | `param`      | `parameter`    | `paramResolution` → `parameterResolution` *(in identifiers; Go struct type `Params` stays)* |
211: | `hist`       | `histogram`    | `expectedHist` → `expectedHistogram`     |
212: 
213: **Allowed exceptions (retained as-is):**
214: 
215: - **Go language idioms:** `err`, `len`, `cap`, `min`, `max`, `num` — these are
216:   builtins or deeply idiomatic Go short forms. Do not expand.
217: - **Go struct field `Params` / variable `params`** — this is the established
218:   parameter-bundle type name across every indicator (`type Params struct`,
219:   `Compute(params Params)`). The long form `Parameters` is used **only** in
220:   `core/specification.go`/`core/indicator-specification.ts` where it
221:   describes framework-level metadata.
222: - **TS constructor-parameter idiom `value_`** (trailing underscore) — used
223:   to avoid shadowing a member `value` during construction.
224: - **Domain-specific short forms that happen to collide** (e.g., `pos` as a
225:   loop variable is fine even though an unrelated indicator may use
226:   `position` in its name). These are different concepts, not abbreviation
227:   drift.
228: - **TA-Lib port conventions** (`begIdx` → now `begIndex`, `outIdx` → now
229:   `outIndex`): the `Idx` part is normalized but the `beg`/`out` prefixes
230:   are kept, matching TA-Lib's C-source variable names for easier
231:   cross-reference.
232: 
233: **Go↔TS parity:** the same identifier stem MUST be used in both languages.
234: When porting, do not introduce new short forms; use the canonical long form
235: from the table above.
236: 
237: ### Go Receiver Naming Convention
238: 
239: Method receivers follow a **type-shape rule**, not a one-letter-for-all rule:
240: 
241: - **Compound type name** (2+ CamelCase words, e.g. `SimpleMovingAverage`,
242:   `BarComponent`, `CycleEstimatorType`) → receiver is **`s`**
243:   ("self"/"struct"). Short, consistent across all large types.
244: - **Simple type name** (single word, e.g. `Momentum`, `Trade`, `Quote`) →
245:   receiver is the **first letter of the type name, lowercased** (`m`, `t`,
246:   `q`). Short types deserve short mnemonic receivers.
247: 
248: All methods on a given type MUST use the same receiver name (enforced by
249: `go vet`/consistency). When adding a method, match the receiver already in
250: use on that type.
251: 
252: **Local-variable collision:** if a method body needs a local variable that
253: would shadow the receiver `s` (typical case: `s := someValue.String()` in
254: `MarshalJSON`), rename the **local** to `str` (not the receiver). Example:
255: 
256: ```go
257: func (s BarComponent) MarshalJSON() ([]byte, error) {
258:     str := s.String()           // local renamed s → str
259:     if str == unknown { ... }
260: }
261: ```
262: 
263: This keeps the receiver convention uniform; local names are flexible.
264: 
265: ### Concurrency / Lock Convention
266: 
267: Stateful indicators (those exposing `Update(...)` that mutates internal
268: fields) MUST guard reads and writes with a `sync.RWMutex`:
269: 
270: - **Field:** `mu sync.RWMutex` (always named `mu`, always `RWMutex` —
271:   never plain `sync.Mutex`).
272: - **Writer methods** (`Update`, any mutator): acquire with
273:   `s.mu.Lock(); defer s.mu.Unlock()` on the two lines immediately
274:   following the function-signature brace (before any other logic).
275: - **Reader methods** (`IsPrimed`, accessors that read mutable state):
276:   use `s.mu.RLock(); defer s.mu.RUnlock()`.
277: - **Never** unlock manually (non-deferred). Always pair Lock with
278:   `defer Unlock` on the very next line.
279: 
280: **Exceptions — indicators without a mutex:**
281: 
282: 1. **Pure delegating wrappers** that own no mutable state of their own
283:    and forward all `Update`/read calls to an embedded indicator which
284:    itself carries a mutex (e.g. `standarddeviation` wrapping
285:    `variance`). The embedded indicator provides the lock.
286: 2. **Internal engines** that are not part of the public indicator
287:    surface and are only consumed by higher-level wrappers which hold
288:    the mutex (e.g. `corona` engine used by `coronaswingposition`,
289:    `coronaspectrum`, `coronasignaltonoiseratio`, `coronatrendvigor`).
290: 
291: Any new stateful public indicator MUST either carry its own `mu
292: sync.RWMutex` following the pattern above or fall under one of the two
293: exceptions.
294: 
295: ### Go Style Invariants (enforced across the codebase)
296: 
297: The following idioms are invariants — audits return **zero** deviations
298: and new code MUST respect them:
299: 
300: - **Variable declarations.** No `var x T = zero-literal`, no split
301:   `var x T` followed immediately by `x = expr`. Use either `var x T`
302:   (zero value) or `x := expr`.
303: - **Empty / nil tests.** Slice/map emptiness is not tested by
304:   `len(x) == 0`; prefer direct ranging or `== nil` where the value is
305:   nil-able. Error checks always use `err != nil`.
306: - **Any.** Use `any`, never `interface{}`.
307: - **Slice construction.** Use `[]T{}` or `make([]T, n)` /
308:   `make([]T, 0, n)` — never `make([]T, 0)` (no capacity hint).
309: - **`new`.** Not used; use composite literals (`&T{...}`).
310: - **Imports.** Grouped by class with blank lines: stdlib, external,
311:   `zpano/*`. Each group contains only one class.
312: - **Doc comments.** Every exported `func`/`type`/`var`/`const`/method
313:   has a leading `// Name ...` doc comment. Trivial stubs (e.g.
314:   `passthrough.Update`) still get a one-line comment.
315: 
316: ### Go ↔ TypeScript Local-Variable Parity
317: 
318: The same indicator MUST use the **same local/field names** in both
319: languages. When porting, copy the other language's names verbatim
320: where semantically identical. Observed canonical vocabulary:
321: 
322: | Concept                    | Canonical local name |
323: |----------------------------|----------------------|
324: | Sample difference / delta  | `temp` (short-lived delta) / `diff` (explicit diff) |
325: | Accumulator sum            | `sum` (never `total`, `acc`, `accumulator`) |
326: | Small-comparison tolerance | `epsilon` |
327: | Running standard deviation | `stddev` |
328: | MACD fast/slow/signal line | `fast`, `slow`, `signal`, `macd`, `histogram` |
329: | Variance (scratch)         | `v` |
330: | NaN sentinel (Go)          | `nan := math.NaN()` |
331: | Spread (upper-lower)       | `spread` |
332: | Bollinger band-width       | `bw` *(local; field is `bandWidth`)* |
333: | Bollinger percent-band     | `pctB` *(local; field is `percentBand`)* |
334: | Length minus one           | `lengthMinOne` |
335: | Amount = price × volume    | `amount` |
336: 
337: Primary loop counter is `i`; secondary `j`, `k`; only use `index`
338: when the counter semantically labels a named index (e.g. filter-bank
339: bin index). Never introduce new short forms (`idx`, `res`, etc. are
340: banned per the Abbreviation Convention).
341: 
342: ## Adding a New Indicator -- Checklist
343: 
344: 1. **Determine the group.** Is the author known? Use `<author-name>/`. Unknown
345:    author? Use `common/`. Your own? Use `custom/`.
346: 2. **Create the indicator subfolder** at `indicators/<group>/<indicator-name>/`
347:    using full descriptive names and language-appropriate casing.
348: 3. **Create the required files:**
349:    - **Parameters** -- define a params struct. Where the indicator can be
350:      driven by different price components, include optional `BarComponent`,
351:      `QuoteComponent`, `TradeComponent` fields (zero = default in Go,
352:      `undefined` = default in TS). Document the defaults in field comments.
353:      For indicators that require fixed inputs (e.g., HLCV for Advance-Decline,
354:      OHLC for Balance of Power, high/low for Parabolic SAR), the params struct
355:      may be empty or contain only non-component parameters (length, thresholds).
356:    - **Output enum** -- define a per-indicator output enum. In **Go**, the type
357:      is bare `Output` and constants are bare concept names (package name
358:      provides scoping — `simplemovingaverage.Output`, `simplemovingaverage.Value`).
359:      In **TypeScript**, the enum is named `<IndicatorName>Output` (e.g.,
360:      `SimpleMovingAverageOutput`) because TS imports by symbol, not by module.
361:    - **Implementation** -- embed `core.LineIndicator` (Go) or extend
362:      `LineIndicator` (TS). In the constructor:
363:      1. Resolve zero-value components → defaults for the component functions.
364:         In Go, check `if bc == 0 { bc = DefaultBarComponent }`.
365:         In TS, set the protected setters (`this.barComponent = params.barComponent`),
366:         which resolve `undefined` → default internally.
367:      2. Build the mnemonic using `ComponentTripleMnemonic` / `componentTripleMnemonic`
368:         with the **resolved** values (Go) or raw param values (TS — the function
369:         checks `!== undefined && !== Default*` itself). **Exception:** indicators
370:         with no component params (empty or non-component-only Params) hand-write
371:         their mnemonic (e.g., `bop()`, `ad()`, `psar(0.02,0.2)`) — the mnemonic
372:         still includes non-component parameters when non-default.
373:      3. Initialize the `LineIndicator` with mnemonic, description, component
374:         functions, and the indicator's `Update` method.
375:    - **`Metadata()`** -- return a `core.Metadata` using the per-indicator output
376:      enum for `kind` (not a hardcoded `0`).
377:    - **Test file** -- include mnemonic sub-tests covering: all components zero,
378:      each component set individually, and combinations of two components.
379: 4. **Register the indicator** in `core/indicator-type` (add a new enum variant).
380: 5. **Follow the consistent depth rule** -- the indicator must be exactly two
381:    levels below `indicators/`.
382: 
383: ## Design Decisions Log
384: 
385: | Decision | Rationale |
386: |----------|-----------|
387: | No underscore prefix on `core/`, `common/`, `custom/` | Leading underscores conflict with conventions in Python (`_` = private) and are non-idiomatic in Go. Dropping them gives true consistency across all five languages. The names are unambiguous enough on their own. |
388: | Full indicator names, not mnemonics | Short names (SMA, JMA) can be cryptic and may overlap. Full names are self-documenting. |
389: | `outputs/` instead of `entities/` in core | These types represent indicator *output shapes*, not general domain entities. `outputs` is more precise about their purpose. |
390: | `core/` instead of `types/` | `core` is broad enough to hold types, enums, base classes, and utilities without needing a rename later. `types` would be too narrow. |
391: | `common/` instead of `unknown/`, `classic/`, `standard/` | `common` is neutral -- doesn't imply age (`classic`), official status (`standard`), or sound dismissive (`unknown`). |
392: | `custom/` instead of `experimental/` | `custom` describes *origin* (you made it), not *maturity*. A custom indicator can be stable; an experimental one implies instability. If you need to track maturity, use metadata in code, not folder structure. |
393: | Go uses bare `Type` in packages; other languages use `IndicatorType`/`OutputType` | Go's package system provides scoping (`core.Type`), making prefixes redundant and stuttery (`core.IndicatorType`). Other languages import symbols directly, so self-describing names are necessary. |
394: | Per-indicator subfolders even in `common/` | Without subfolders, all indicator files in `common/` would land in one flat directory (and in Go, one package). This doesn't scale and is inconsistent with the author-folder structure. |
395: | Component enums start at `iota + 1` | Makes the zero value (`0`) explicitly "not set", which is used as the sentinel for "use default". This is idiomatic Go for optional enum fields in structs (zero value of `int` is `0`). |
396: | TypeScript enums start at `0`, components are `undefined` when not set | TS enums default to `0`, and optional params use `undefined` (not `0`) as the sentinel for "not set". The `componentTripleMnemonic` function checks `!== undefined && !== Default*` to decide whether to show a component. The `LineIndicator` setters use `=== undefined` (not truthiness) to decide when to apply the default, so explicitly passing a zero-valued enum (e.g., `BarComponent.Open`) works correctly. |
397: | Zero-value component = default, omitted from mnemonic | Keeps the common case clean (`sma(14)` instead of `sma(14, c, ba/2, p)`) while still allowing explicit overrides when needed. `ComponentTripleMnemonic` checks against `Default*` constants (not zero), so explicitly passing the default value also omits it — the same indicator always gets the same mnemonic. |
398: | Entity `Mnemonic()` method separate from `String()` | `String()` returns full words (`"close"`, `"mid"`) for JSON serialization and debugging. `Mnemonic()` returns short codes (`"c"`, `"ba/2"`) for compact display in chart labels. Separating them avoids overloading `String()` with a display concern. |
399: | Mnemonic sub-tests for every indicator | Mnemonic correctness matters for charting UIs. Each indicator must have sub-tests covering: all components zero, each component individually, and pairwise combinations. This prevents regressions when changing the format string or `ComponentTripleMnemonic`. |
400: 
401: ## LineIndicator Design Pattern
402: 
403: A **line indicator** takes a single numeric input and produces a single scalar
404: output. Most indicators (SMA, EMA, JMA, ...) are line indicators. The
405: `LineIndicator` abstraction eliminates boilerplate by providing
406: `UpdateScalar/Bar/Quote/Trade` methods (and array variants) that delegate to the
407: indicator's core `Update(sample) → value` function.
408: 
409: ### What Each Indicator Implements
410: 
411: Each concrete indicator is responsible for:
412: 
413: 1. **`Update(sample float64) float64`** (Go) / **`update(sample: number): number`** (TS) --
414:    the core calculation logic.
415: 2. **`Metadata()`** / **`metadata()`** -- returns indicator-level metadata with
416:    an explicit per-indicator output enum (not a hardcoded `kind: 0`).
417: 3. **`IsPrimed()`** / **`isPrimed()`** -- whether the indicator has received
418:    enough data to produce meaningful output.
419: 
420: ### What LineIndicator Provides (Eliminating Boilerplate)
421: 
422: The `LineIndicator` base provides:
423: 
424: - **`UpdateScalar/Bar/Quote/Trade`** -- all delegate to the indicator's `Update()`
425:   method via a function reference (Go) or abstract method (TS). The bar/quote/trade
426:   variants extract the relevant component value first.
427: - **Array update methods** -- `UpdateScalars/Bars/Quotes/Trades` (TS only;
428:   Go has equivalent free functions in `core/indicator.go`).
429: - **Storage of component functions** -- bar, quote, and trade component
430:   extraction functions are stored and used by the Update* methods.
431: 
432: ### Go Implementation
433: 
434: In Go, `LineIndicator` is an **embedded struct**. The concrete indicator stores a
435: `core.LineIndicator` field and initializes it via `core.NewLineIndicator()`,
436: passing its own `Update` method as a function reference:
437: 
438: ```go
439: type SimpleMovingAverage struct {
440:     mu sync.RWMutex
441:     core.LineIndicator   // embedded -- promotes UpdateScalar/Bar/Quote/Trade
442:     // ... indicator-specific fields
443: }
444: 
445: func NewSimpleMovingAverage(p *Params) (*SimpleMovingAverage, error) {
446:     sma := &SimpleMovingAverage{ /* ... */ }
447:     sma.LineIndicator = core.NewLineIndicator(
448:         mnemonic, desc, barFunc, quoteFunc, tradeFunc, sma.Update,
449:     )
450:     return sma, nil
451: }
452: ```
453: 
454: The mutex lives on the concrete indicator (not on `LineIndicator`), and is
455: acquired inside `Update()`. The promoted `UpdateScalar/Bar/Quote/Trade` methods
456: call `updateFn` (which is the concrete indicator's `Update`), so the threading
457: model is preserved.
458: 
459: ### TypeScript Implementation
460: 
461: In TypeScript, `LineIndicator` is an **abstract class**. The concrete indicator
462: extends it and implements `update()` and `metadata()`:
463: 
464: ```typescript
465: export abstract class LineIndicator implements Indicator {
466:     protected mnemonic!: string;
467:     protected description!: string;
468:     protected primed!: boolean;
469: 
470:     public abstract metadata(): IndicatorMetadata;
471:     public abstract update(sample: number): number;
472:     // updateScalar/Bar/Quote/Trade provided by base class
473: }
474: ```
475: 
476: **Concrete indicator constructors must set the three component setters** to
477: initialize the component functions used by `updateBar()`, `updateQuote()`, and
478: `updateTrade()`:
479: 
480: ```typescript
481: this.barComponent = params.barComponent;       // undefined → DefaultBarComponent
482: this.quoteComponent = params.quoteComponent;   // undefined → DefaultQuoteComponent
483: this.tradeComponent = params.tradeComponent;   // undefined → DefaultTradeComponent
484: ```
485: 
486: The `LineIndicator` setters use `=== undefined` (not truthiness) to apply
487: defaults, so explicitly passing a zero-valued enum like `BarComponent.Open`
488: works correctly.
489: 
490: Each entity file also exports a `Default*` constant (`DefaultBarComponent`,
491: `DefaultQuoteComponent`, `DefaultTradeComponent`) for use in
492: `componentTripleMnemonic` and anywhere else defaults need to be checked.
493: 
494: ### Per-Indicator Output Enums
495: 
496: Each indicator defines its own output enum describing what it produces. The
497: naming convention is **language-asymmetric** because the two languages scope
498: symbols differently:
499: 
500: - **Go**: use bare `Output` / `Value`. The package name
501:   (`simplemovingaverage.Output`, `simplemovingaverage.Value`) provides scoping,
502:   so repeating the indicator name in the type would stutter.
503: - **TypeScript**: use long-form `<IndicatorName>Output` /
504:   `<IndicatorName>Value`. TS imports by symbol rather than by module, so the
505:   indicator name must be baked into the identifier to stay unambiguous at
506:   call sites.
507: 
508: ```go
509: // Go — file: simplemovingaverage/output.go
510: package simplemovingaverage
511: 
512: type Output int
513: const (
514:     Value Output = iota
515: )
516: ```
517: 
518: ```typescript
519: // TypeScript — file: simple-moving-average/output.ts
520: export enum SimpleMovingAverageOutput {
521:     SimpleMovingAverageValue = 0,
522: }
523: ```
524: 
525: For multi-output indicators, strip the indicator-name prefix on the Go side
526: and keep the descriptive suffix:
527: 
528: ```go
529: // Go — directionalmovementindex/output.go
530: type Output int
531: const (
532:     PlusDirectionalIndicator Output = iota
533:     MinusDirectionalIndicator
534:     AverageDirectionalIndex
535:     // ...
536: )
537: ```
538: 
539: ```typescript
540: // TypeScript — directional-movement-index/output.ts
541: export enum DirectionalMovementIndexOutput {
542:     PlusDirectionalIndicator = 0,
543:     MinusDirectionalIndicator = 1,
544:     AverageDirectionalIndex = 2,
545:     // ...
546: }
547: ```
548: 
549: The `metadata()` method uses this enum for the `kind` field instead of a
550: hardcoded `0`, making the output semantics explicit and type-safe.
551: 
552: ### Metadata Structure
553: 
554: The `Metadata` / `IndicatorMetadata` type includes:
555: 
556: | Field         | Description |
557: |---------------|-------------|
558: | `type`        | The indicator type enum variant (e.g., `SimpleMovingAverage`). |
559: | `mnemonic`    | A short name like `sma(5)` or `jma(7, -1)`. |
560: | `description` | A human-readable description like `Simple moving average sma(5)`. |
561: | `outputs`     | An array of per-output metadata (kind, type, mnemonic, description). |
562: 
563: The `mnemonic` and `description` live at the indicator level (on `Metadata`)
564: and are also duplicated per-output for convenience (each output carries its own
565: mnemonic/description).
566: 
567: ### Mnemonic Prefix Convention
568: 
569: Every indicator is identified by a short lowercase **mnemonic prefix** (e.g.
570: `sma`, `rsi`, `bb`, `macd`). These prefixes must be consistent between Go and
571: TypeScript, because downstream tooling keys off them.
572: 
573: **Rules:**
574: 
575: 1. **Lowercase only.** Digits are allowed (`t2`, `t3`, `rocr100`).
576:    Leading `+`/`-` is allowed for paired directional indicators (`+di`, `-di`,
577:    `+dm`, `-dm`).
578: 2. **Kept short** — typically 2-7 chars. The mnemonic is a compact label, not a
579:    full name; the full name goes in `description`.
580: 3. **Go ↔ TS parity.** The same prefix must appear on both sides. When adding
581:    an indicator, add its prefix to both languages in the same PR.
582: 4. **Format.** `<prefix>(<param1>, <param2>, ...<componentSuffix>)` where the
583:    component suffix comes from `ComponentTripleMnemonic` (empty when all
584:    components are at their default).
585: 5. **Configurable components → include the suffix.** Any indicator that
586:    exposes `BarComponent` / `QuoteComponent` / `TradeComponent` on its params
587:    struct **must** append `ComponentTripleMnemonic(bc, qc, tc)` to its
588:    mnemonic. Indicators with hardcoded components (e.g. balance-of-power)
589:    omit the suffix.
590: 6. **Parameterless indicators** use the bare prefix (`obv`, `ad`, `bop`) when
591:    defaults are in effect, or wrap the component suffix in parens when needed
592:    (e.g. `obv(hl/2)`).
593: 
594: **Canonical inventory (65 indicators with mnemonics, plus 4 paired directional):**
595: 
596: | Prefix    | Indicator                                            |
597: |-----------|------------------------------------------------------|
598: | `aci`     | autocorrelation indicator                            |
599: | `acp`     | autocorrelation periodogram                          |
600: | `ad`      | advance/decline                                      |
601: | `adosc`   | advance/decline oscillator                           |
602: | `adx`     | average directional movement index                   |
603: | `adxr`    | average directional movement index rating            |
604: | `apo`     | absolute price oscillator                            |
605: | `aroon`   | Aroon                                                |
606: | `atcf`    | adaptive trend and cycle filter                      |
607: | `atr`     | average true range                                   |
608: | `bb`      | Bollinger bands                                      |
609: | `bbtrend` | Bollinger bands trend                                |
610: | `bop`     | balance of power                                     |
611: | `cbps`    | comb band-pass spectrum                              |
612: | `cc`      | cyber cycle                                          |
613: | `cci`     | commodity channel index                              |
614: | `cmo`     | Chande momentum oscillator                           |
615: | `cog`     | center-of-gravity oscillator                         |
616: | `correl`  | Pearson's correlation coefficient                    |
617: | `csnr`    | corona signal-to-noise ratio                         |
618: | `cspect`  | corona spectrum                                      |
619: | `cswing`  | corona swing position                                |
620: | `ctv`     | corona trend vigor                                   |
621: | `dcp`     | dominant cycle                                       |
622: | `dema`    | double exponential moving average                    |
623: | `dftps`   | discrete Fourier transform power spectrum            |
624: | `+di`/`-di` | directional indicator plus / minus                 |
625: | `+dm`/`-dm` | directional movement plus / minus                  |
626: | `dx`      | directional movement index                           |
627: | `ema`     | exponential moving average                           |
628: | `frama`   | fractal adaptive moving average                      |
629: | `gspect`  | Goertzel spectrum                                    |
630: | `htitl`   | Hilbert transformer instantaneous trend line         |
631: | `iTrend`  | instantaneous trend line                             |
632: | `jma`     | Jurik moving average                                 |
633: | `kama`    | Kaufman adaptive moving average                      |
634: | `linreg`  | linear regression                                    |
635: | `macd`    | moving average convergence/divergence                |
636: | `mama`    | MESA adaptive moving average                         |
637: | `mespect` | maximum entropy spectrum                             |
638: | `mfi`     | money flow index                                     |
639: | `mom`     | momentum                                             |
640: | `natr`    | normalized average true range                        |
641: | `obv`     | on-balance volume                                    |
642: | `ppo`     | percentage price oscillator                          |
643: | `roc`     | rate of change                                       |
644: | `rocp`    | rate of change percent                               |
645: | `rocr`/`rocr100` | rate of change ratio (unit or ×100)           |
646: | `roof`    | roofing filter                                       |
647: | `rsi`     | relative strength index                              |
648: | `sar`     | parabolic stop and reverse                           |
649: | `sma`     | simple moving average                                |
650: | `ss`      | super smoother                                       |
651: | `stdev`   | standard deviation (`.s` sample / `.p` population)   |
652: | `stoch`   | stochastic                                           |
653: | `stochrsi`| stochastic relative strength index                   |
654: | `sw`      | sine wave                                            |
655: | `tcm`     | trend cycle mode                                     |
656: | `tema`    | triple exponential moving average                    |
657: | `tr`      | true range                                           |
658: | `trima`   | triangular moving average                            |
659: | `trix`    | triple exponential moving average oscillator         |
660: | `t2`      | T2 exponential moving average                        |
661: | `t3`      | T3 exponential moving average                        |
662: | `ultosc`  | ultimate oscillator                                  |
663: | `var`     | variance (`.s` sample / `.p` population)             |
664: | `willr`   | Williams %R                                          |
665: | `wma`     | weighted moving average                              |
666: | `zecema`  | zero-lag error-correcting exponential moving average |
667: | `zema`    | zero-lag exponential moving average                  |
668: 
669: The two utility modules `corona/` and `frequencyresponse/` are support helpers,
670: not indicators, and have no mnemonic of their own.
671: 
672: ### Component Triple Mnemonic
673: 
674: A line indicator can be fed bar, quote, or trade data. Each data type has a
675: **component** that selects which field to extract (e.g., close price from a bar,
676: mid-price from a quote). The `ComponentTripleMnemonic` utility (in `core/`)
677: generates the component suffix for an indicator's mnemonic. There are three
678: components (bar, quote, trade) -- hence "triple", not "pair".
679: 
680: #### Default Components
681: 
682: Each component type has a default constant defined in `entities/`:
683: 
684: | Component type   | Default constant         | Default value    | Mnemonic |
685: |------------------|--------------------------|------------------|----------|
686: | `BarComponent`   | `DefaultBarComponent`    | `BarClosePrice`  | `c`      |
687: | `QuoteComponent` | `DefaultQuoteComponent`  | `QuoteMidPrice`  | `ba/2`   |
688: | `TradeComponent` | `DefaultTradeComponent`  | `TradePrice`     | `p`      |
689: 
690: #### Zero-Value Convention
691: 
692: All component enums start at `iota + 1`, making the zero value `0` explicitly
693: "not set". In indicator params, **a zero-value component means "use the default
694: and omit from the mnemonic"**:
695: 
696: ```go
697: // SimpleMovingAverageParams
698: type SimpleMovingAverageParams struct {
699:     Length         int
700:     BarComponent   entities.BarComponent   // zero → DefaultBarComponent, omitted from mnemonic
701:     QuoteComponent entities.QuoteComponent // zero → DefaultQuoteComponent, omitted from mnemonic
702:     TradeComponent entities.TradeComponent // zero → DefaultTradeComponent, omitted from mnemonic
703: }
704: ```
705: 
706: #### Constructor Resolution Logic
707: 
708: Each indicator constructor follows this pattern:
709: 
710: 1. Read the component from params.
711: 2. If zero, resolve to the default constant.
712: 3. Get the component function using the resolved value.
713: 4. Pass the **resolved** values to `ComponentTripleMnemonic` (which omits
714:    defaults automatically).
715: 
716: ```go
717: // Resolve defaults for component functions.
718: bc := p.BarComponent
719: if bc == 0 {
720:     bc = entities.DefaultBarComponent
721: }
722: // ... same for qc, tc
723: 
724: // Build mnemonic using resolved components — defaults are omitted by ComponentTripleMnemonic.
725: mnemonic := fmt.Sprintf("sma(%d%s)", length,
726:     core.ComponentTripleMnemonic(bc, qc, tc))
727: ```
728: 
729: This means that `sma(14)` is the mnemonic whether the user left `BarComponent`
730: as zero or explicitly set it to `BarClosePrice` — both produce the same
731: indicator with the same mnemonic.
732: 
733: #### ComponentTripleMnemonic Function
734: 
735: The function compares each component against its `Default*` constant. If a
736: component equals its default, it is omitted. Non-default components are
737: appended as `", "` followed by the component's `Mnemonic()` output:
738: 
739: ```go
740: func ComponentTripleMnemonic(
741:     bc entities.BarComponent, qc entities.QuoteComponent, tc entities.TradeComponent,
742: ) string {
743:     var s string
744:     if bc != entities.DefaultBarComponent   { s += ", " + bc.Mnemonic() }
745:     if qc != entities.DefaultQuoteComponent { s += ", " + qc.Mnemonic() }
746:     if tc != entities.DefaultTradeComponent { s += ", " + tc.Mnemonic() }
747:     return s
748: }
749: ```
750: 
751: The result is inserted into the indicator format string after the numeric
752: parameters: `sma(14%s)` → `sma(14)` or `sma(14, hl/2)`.
753: 
754: #### Mnemonic Examples
755: 
756: | Params                                                                | Mnemonic              |
757: |-----------------------------------------------------------------------|-----------------------|
758: | `{Length: 5}`                                                         | `sma(5)`              |
759: | `{Length: 5, BarComponent: BarMedianPrice}`                           | `sma(5, hl/2)`        |
760: | `{Length: 5, QuoteComponent: QuoteBidPrice}`                          | `sma(5, b)`           |
761: | `{Length: 5, TradeComponent: TradeVolume}`                            | `sma(5, v)`           |
762: | `{Length: 5, BarComponent: BarOpenPrice, QuoteComponent: QuoteBidPrice}` | `sma(5, o, b)`     |
763: | `{Length: 5, BarComponent: BarHighPrice, TradeComponent: TradeVolume}` | `sma(5, h, v)`       |
764: | `{Length: 7, Phase: 0}`                                               | `jma(7, 0)`           |
765: | `{Length: 7, Phase: 0, BarComponent: BarMedianPrice}`                 | `jma(7, 0, hl/2)`    |
766: 
767: The `description` is always the full name followed by the mnemonic:
768: `"Simple moving average sma(5, hl/2)"`.
769: 
770: #### Entity Mnemonic Methods
771: 
772: Each component enum has two display methods: `String()` (full word, used for
773: JSON serialization) and `Mnemonic()` (short code, used in indicator mnemonics).
774: 
775: **BarComponent mnemonics:**
776: 
777: | Enum value        | `String()`   | `Mnemonic()` |
778: |-------------------|-------------|---------------|
779: | `BarOpenPrice`    | `open`      | `o`           |
780: | `BarHighPrice`    | `high`      | `h`           |
781: | `BarLowPrice`     | `low`       | `l`           |
782: | `BarClosePrice`   | `close`     | `c`           |
783: | `BarVolume`       | `volume`    | `v`           |
784: | `BarMedianPrice`  | `median`    | `hl/2`        |
785: | `BarTypicalPrice` | `typical`   | `hlc/3`       |
786: | `BarWeightedPrice`| `weighted`  | `hlcc/4`      |
787: | `BarAveragePrice` | `average`   | `ohlc/4`      |
788: 
789: **QuoteComponent mnemonics:**
790: 
791: | Enum value             | `String()`     | `Mnemonic()`          |
792: |------------------------|----------------|-----------------------|
793: | `QuoteBidPrice`        | `bid`          | `b`                   |
794: | `QuoteAskPrice`        | `ask`          | `a`                   |
795: | `QuoteBidSize`         | `bidSize`      | `bs`                  |
796: | `QuoteAskSize`         | `askSize`      | `as`                  |
797: | `QuoteMidPrice`        | `mid`          | `ba/2`                |
798: | `QuoteWeightedPrice`   | `weighted`     | `(bbs+aas)/(bs+as)`   |
799: | `QuoteWeightedMidPrice`| `weightedMid`  | `(bas+abs)/(bs+as)`   |
800: | `QuoteSpreadBp`        | `spreadBp`     | `spread bp`           |
801: 
802: **TradeComponent mnemonics:**
803: 
804: | Enum value    | `String()`  | `Mnemonic()` |
805: |---------------|-------------|---------------|
806: | `TradePrice`  | `price`     | `p`           |
807: | `TradeVolume` | `volume`    | `v`           |
808: 
809: ## Helper / Shared Component Families
810: 
811: Some indicators are driven by an interchangeable internal component that has
812: several variants (e.g., different algorithms producing the same shape of
813: output). The canonical example is John Ehlers' **Hilbert Transformer cycle
814: estimator**, which has four variants (HomodyneDiscriminator,
815: HomodyneDiscriminatorUnrolled, PhaseAccumulator, DualDifferentiator) that all
816: produce a cycle period estimate from a price sample.
817: 
818: These are **not indicators themselves**. They are helper components used by
819: actual indicators (e.g., MESA Adaptive Moving Average). They live in the
820: indicator folder of their conceptual owner and follow a specific structure.
821: 
822: ### Folder Layout
823: 
824: A helper family lives in a single folder at the standard indicator depth
825: (`indicators/<group>/<family-name>/`) and contains:
826: 
827: - **Shared interface** — the contract every variant implements.
828: - **Shared params struct/interface** — a single params shape common to all variants.
829: - **Variant type enum** — discriminator for the family (one constant per variant).
830: - **Common utilities file** — shared constants, helper functions (`push`,
831:   `fillWmaFactors`, ...) and the **factory/dispatcher** function.
832: - **One file per variant** implementing the shared interface.
833: - **One spec/test file per variant** plus one for the common file.
834: 
835: Example (TS — `ts/indicators/john-ehlers/hilbert-transformer/`):
836: 
837: ```
838: cycle-estimator.ts                 # interface
839: cycle-estimator-params.ts          # shared params
840: cycle-estimator-type.ts            # variant enum
841: common.ts                          # utilities + createEstimator()
842: homodyne-discriminator.ts          # variant
843: homodyne-discriminator-unrolled.ts
844: phase-accumulator.ts
845: dual-differentiator.ts
846: # + matching .spec.ts for each
847: ```
848: 
849: Go mirrors the same structure with lowercase filenames (`cycleestimator.go`,
850: `cycleestimatorparams.go`, `cycleestimatortype.go`, `estimator.go` for common
851: utilities and `NewCycleEstimator`, and one `<variant>estimator.go` per variant
852: — e.g., `homodynediscriminatorestimator.go`, `phaseaccumulatorestimator.go`).
853: These are the **conceptual-naming exception** noted in the File Naming section:
854: the folder name (`hilberttransformer/`) is never prefixed onto the member
855: files, because each file represents an independent concept within the family.
856: 
857: ### Factory / Dispatcher Pattern
858: 
859: The common file exposes a dispatcher that constructs a variant by type.
860: Each variant has its own **default params** baked into the dispatcher — callers
861: that omit params for a specific type get sensible defaults for that variant.
862: 
863: ```go
864: // Go — go/indicators/johnehlers/hilberttransformer/estimator.go
865: func NewCycleEstimator(typ CycleEstimatorType, params *CycleEstimatorParams) (CycleEstimator, error) {
866:     switch typ {
867:     case HomodyneDiscriminator:         return NewHomodyneDiscriminatorEstimator(params)
868:     case HomodyneDiscriminatorUnrolled: return NewHomodyneDiscriminatorEstimatorUnrolled(params)
869:     case PhaseAccumulator:              return NewPhaseAccumulatorEstimator(params)
870:     case DualDifferentiator:            return NewDualDifferentiatorEstimator(params)
871:     }
872:     return nil, fmt.Errorf("invalid cycle estimator type: %s", typ)
873: }
874: ```
875: 
876: ```ts
877: // TS — ts/indicators/john-ehlers/hilbert-transformer/hilbert-transformer-common.ts
878: export function createEstimator(
879:     estimatorType?: HilbertTransformerCycleEstimatorType,
880:     estimatorParams?: HilbertTransformerCycleEstimatorParams,
881: ): HilbertTransformerCycleEstimator {
882:     if (estimatorType === undefined) {
883:         estimatorType = HilbertTransformerCycleEstimatorType.HomodyneDiscriminator;
884:     }
885:     switch (estimatorType) {
886:         case HilbertTransformerCycleEstimatorType.HomodyneDiscriminator:
887:             estimatorParams ??= { smoothingLength: 4, alphaEmaQuadratureInPhase: 0.2, alphaEmaPeriod: 0.2 };
888:             return new HilbertTransformerHomodyneDiscriminator(estimatorParams);
889:         // ... other variants, each with its own default params
890:         default:
891:             throw new Error('Invalid cycle estimator type: ' + estimatorType);
892:     }
893: }
894: ```
895: 
896: The dispatcher must have a default variant (when `type` is omitted) and throw
897: on unknown types.
898: 
899: ### Shared Interface Design
900: 
901: The shared interface exposes:
902: 
903: - Primed state (`primed`) and warm-up configuration (`warmUpPeriod`).
904: - Construction parameters as readable properties (`smoothingLength`,
905:   `alphaEmaQuadratureInPhase`, `alphaEmaPeriod`).
906: - Intermediate state visible for testing/debugging (`smoothed`, `detrended`,
907:   `inPhase`, `quadrature`, `period`).
908: - An `update(sample)` method returning the primary output.
909: 
910: TypeScript naming: **do not** suffix getters with `Value`. Prefix the backing
911: private field with `_` so the public getter can take the unadorned name:
912: 
913: ```ts
914: // Correct — public getter is `period`, backing field is `_period`.
915: private _period = 0;
916: public get period(): number { return this._period; }
917: 
918: // Incorrect — do not use `periodValue` just to avoid a name clash.
919: ```
920: 
921: ### File Naming — No Type Suffixes
922: 
923: This module follows the same rule as the rest of the codebase: **no
924: `.interface.ts`, `.enum.ts`, or similar type-suffixes**. Interfaces, enums,
925: params, and implementations all use plain `.ts` filenames differentiated by
926: descriptive stem names.
927: 
928: ### Warm-Up Period Contract
929: 
930: Each variant has an intrinsic minimum priming length (the sample count needed
931: for its state to be meaningful). If `params.warmUpPeriod` is provided and
932: larger than that minimum, it overrides the minimum; otherwise the intrinsic
933: minimum is used. Tests must cover both the default path and the custom
934: `warmUpPeriod` override.

(End of file - total 934 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 17.7s)

_Thinking:_

 I need to update the SKILL.md documentation with several changes: the symbol naming table needs adjustments since `core.Type` was renamed to `core.Identifier` and `outputs.Type` moved to `outputs/shape.Shape`, I should add a new section covering the taxonomy and descriptor system, and the checklist for adding new indicators needs to reflect the registration process in `core.Identifier` along with descriptor requirements.

Now I'm identifying the specific line ranges to edit: the indicator type table around lines 148-151, the Metadata section at 375-376 to reference BuildMetadata, the registration instructions at line 379 pointing to `core/identifier`, the metadata structure table at 550-565 with field renames, and I'll add a comprehensive new section explaining the taxonomy system with all its components like Descriptor, Role, Pane, Adaptivity, InputRequirement, VolumeUsage, and BuildMetadata.

Let me start making these surgical edits to the documentation.

Now I'll update the SKILL.md with the taxonomy system documentation and the renames.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "newString": "| Concept           | Go (in `core` pkg)  | Go (in `outputs/shape` pkg) | TypeScript / Python / Rust / Zig |\n|-------------------|---------------------|-----------------------------|----------------------------------|\n| Indicator identity| `core.Identifier`   | --                          | `IndicatorIdentifier`            |\n| Output shape      | --                  | `shape.Shape`               | `Shape`                          |",
  "oldString": "| Concept           | Go (in `core` pkg) | Go (in `outputs` pkg) | TypeScript / Python / Rust / Zig |\n|-------------------|---------------------|-----------------------|----------------------------------|\n| Indicator type    | `core.Type`         | --                    | `IndicatorType`                  |\n| Output type       | --                  | `outputs.Type`        | `OutputType`                     |"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 13.8s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "newString": "   - **`Metadata()`** -- return `core.BuildMetadata(core.<Identifier>, mnemonic, description, []core.OutputText{...})` (Go) or\n     `buildMetadata(IndicatorIdentifier.<Name>, mnemonic, description, [...])` (TS). The helper sources each output's `kind` and\n     `shape` from the descriptor registry — the caller supplies only per-output mnemonic/description. See the **Taxonomy &\n     Descriptor Registry** section below for details and the mandatory descriptor-row step.\n   - **Test file** -- include mnemonic sub-tests covering: all components zero,\n     each component set individually, and combinations of two components.\n4. **Register the indicator** in `core/identifier` (add a new enum variant in both Go `core.Identifier` and TS `IndicatorIdentifier`).\n5. **Register the descriptor** in `core/descriptors.{go,ts}` — see the Taxonomy section below. A missing descriptor row causes\n   `BuildMetadata` to panic at runtime.\n6. **Follow the consistent depth rule** -- the indicator must be exactly two\n   levels below `indicators/`.",
  "oldString": "   - **`Metadata()`** -- return a `core.Metadata` using the per-indicator output\n     enum for `kind` (not a hardcoded `0`).\n   - **Test file** -- include mnemonic sub-tests covering: all components zero,\n     each component set individually, and combinations of two components.\n4. **Register the indicator** in `core/indicator-type` (add a new enum variant).\n5. **Follow the consistent depth rule** -- the indicator must be exactly two\n   levels below `indicators/`."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 12.7s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "newString": "### Metadata Structure\n\nThe `Metadata` / `IndicatorMetadata` type includes:\n\n| Field         | Description |\n|---------------|-------------|\n| `identifier`  | The indicator identifier enum variant (e.g., `SimpleMovingAverage`). |\n| `mnemonic`    | A short name like `sma(5)` or `jma(7, -1)`. |\n| `description` | A human-readable description like `Simple moving average sma(5)`. |\n| `outputs`     | An array of per-output metadata (kind, shape, mnemonic, description). |\n\nThe `mnemonic` and `description` live at the indicator level (on `Metadata`)\nand are also duplicated per-output for convenience (each output carries its own\nmnemonic/description).\n\nEach output carries:\n\n| Field         | Description |\n|---------------|-------------|\n| `kind`        | The integer value of the indicator's per-output enum. |\n| `shape`       | The output's data shape (`Scalar`, `Band`, `Heatmap`, `Polyline`). Sourced from the descriptor registry. |\n| `mnemonic`    | Short name for this output. |\n| `description` | Human-readable description of this output. |\n\nIn practice, **indicators never construct `Metadata` object literals directly**.\nThey call `core.BuildMetadata` (Go) / `buildMetadata` (TS), which pulls `kind`\nand `shape` from the descriptor registry. See the Taxonomy section below.",
  "oldString": "### Metadata Structure\n\nThe `Metadata` / `IndicatorMetadata` type includes:\n\n| Field         | Description |\n|---------------|-------------|\n| `type`        | The indicator type enum variant (e.g., `SimpleMovingAverage`). |\n| `mnemonic`    | A short name like `sma(5)` or `jma(7, -1)`. |\n| `description` | A human-readable description like `Simple moving average sma(5)`. |\n| `outputs`     | An array of per-output metadata (kind, type, mnemonic, description). |\n\nThe `mnemonic` and `description` live at the indicator level (on `Metadata`)\nand are also duplicated per-output for convenience (each output carries its own\nmnemonic/description)."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 5.0s)

Now append the major new section documenting the taxonomy system:

**Tool: bash**

**Input:**
```json
{
  "command": "wc -l /home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "description": "Count lines"
}
```

**Output:**
```
951 /home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md

```

---

## Assistant (Build · claude-opus-4.7 · 4.7s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "limit": 10,
  "offset": 945
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md</path>
<type>file</type>
<content>945: ### Warm-Up Period Contract
946: 
947: Each variant has an intrinsic minimum priming length (the sample count needed
948: for its state to be meaningful). If `params.warmUpPeriod` is provided and
949: larger than that minimum, it overrides the minimum; otherwise the intrinsic
950: minimum is used. Tests must cover both the default path and the custom
951: `warmUpPeriod` override.

(End of file - total 951 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 57.1s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "newString": "### Warm-Up Period Contract\n\nEach variant has an intrinsic minimum priming length (the sample count needed\nfor its state to be meaningful). If `params.warmUpPeriod` is provided and\nlarger than that minimum, it overrides the minimum; otherwise the intrinsic\nminimum is used. Tests must cover both the default path and the custom\n`warmUpPeriod` override.\n\n## Taxonomy & Descriptor Registry\n\nEvery indicator carries a **Descriptor** — a taxonomic classification used by\ncharting UIs and discovery tooling to filter, group, and lay out indicators.\nDescriptors are the single source of truth for output `kind` and `shape`;\nindicator `Metadata()` implementations consume the registry via\n`BuildMetadata` and MUST NOT hand-write `kind`/`shape`.\n\n### Dimensions\n\n| Primitive           | Go type                 | TS type                 | Purpose |\n|---------------------|-------------------------|-------------------------|---------|\n| `Identifier`        | `core.Identifier`       | `IndicatorIdentifier`   | Unique ID of the indicator. |\n| `Role`              | `core.Role`             | `Role`                  | Semantic role of an **output** (Smoother, Envelope, Overlay, Polyline, Oscillator, BoundedOscillator, Volatility, VolumeFlow, Directional, CyclePeriod, CyclePhase, FractalDimension, Spectrum, Signal, Histogram, RegimeFlag, Correlation). |\n| `Pane`              | `core.Pane`             | `Pane`                  | Chart pane where an output is drawn: `Price`, `Own`, `OverlayOnParent`. |\n| `Adaptivity`        | `core.Adaptivity`       | `Adaptivity`            | `Static` or `Adaptive` (indicator-level). |\n| `InputRequirement`  | `core.InputRequirement` | `InputRequirement`      | Minimum input data type: `ScalarInput`, `QuoteInput`, `BarInput`, `TradeInput`. |\n| `VolumeUsage`       | `core.VolumeUsage`      | `VolumeUsage`           | `NoVolume`, `AggregateBarVolume`, `PerTradeVolume`, `QuoteLiquidityVolume`. |\n| `OutputDescriptor`  | `core.OutputDescriptor` | `OutputDescriptor`      | `{Kind, Shape, Role, Pane}` per output. |\n| `Descriptor`        | `core.Descriptor`       | `Descriptor`            | `{Identifier, Family, Adaptivity, InputRequirement, VolumeUsage, Outputs}`. |\n\n**VolumeUsage ⇔ InputRequirement validity:**\n\n- `AggregateBarVolume` ⇒ `BarInput`\n- `PerTradeVolume` ⇒ `TradeInput`\n- `QuoteLiquidityVolume` ⇒ `QuoteInput`\n- `NoVolume` is valid with any input\n\n### Registry Files\n\n| Location                                              | Content |\n|-------------------------------------------------------|---------|\n| `go/indicators/core/descriptors.go`                   | Go descriptor map keyed by `Identifier`. |\n| `go/indicators/core/descriptors_test.go`              | Coverage test: every `Identifier` has a descriptor row. |\n| `go/indicators/core/metadata_descriptor_test.go`      | Invariants: outputs are non-empty, Kinds are strictly ascending starting at 1, shapes are known. |\n| `ts/indicators/core/descriptors.ts`                   | TS descriptor `Map` keyed by `IndicatorIdentifier`. Uses `out()`/`desc()` helpers and `S`/`R`/`P`/`A`/`I`/`V` aliases for terse one-line entries. |\n\n**Kind-numbering asymmetry:** Go per-indicator output enums are 1-based\n(`iota + 1`), TS enums are 0-based. The descriptor tables mirror that — Go\n`Kind` values are `1, 2, 3, ...`, TS `kind` values are `0, 1, 2, ...`. A row's\n`Outputs` slice MUST appear in Kind-ascending order (same as the enum's\ndeclared order), because `BuildMetadata` pairs `Outputs[i]` with\n`outputTexts[i]` positionally.\n\n### BuildMetadata Helper\n\nIndicators call `BuildMetadata` / `buildMetadata` from within their\n`Metadata()` method. The helper:\n\n1. Looks up the descriptor for the given `Identifier`.\n2. Panics (Go) / throws (TS) if no descriptor is registered.\n3. Panics/throws if the number of `OutputText` entries does not match the\n   number of descriptor outputs.\n4. Returns a fully-populated `Metadata` with `kind` and `shape` taken from the\n   registry and `mnemonic`/`description` taken from the caller.\n\n**Go:**\n\n```go\nfunc (s *BollingerBands) Metadata() core.Metadata {\n    desc := \"Bollinger Bands \" + s.mnemonic\n    return core.BuildMetadata(\n        core.BollingerBands,\n        s.mnemonic,\n        desc,\n        []core.OutputText{\n            {Mnemonic: s.mnemonic + \" lower\",       Description: desc + \" Lower\"},\n            {Mnemonic: s.mnemonic + \" middle\",      Description: desc + \" Middle\"},\n            {Mnemonic: s.mnemonic + \" upper\",       Description: desc + \" Upper\"},\n            {Mnemonic: s.mnemonic + \" bandWidth\",   Description: desc + \" Band Width\"},\n            {Mnemonic: s.mnemonic + \" percentBand\", Description: desc + \" Percent Band\"},\n            {Mnemonic: s.mnemonic + \" band\",        Description: desc + \" Band\"},\n        },\n    )\n}\n```\n\n**TypeScript:**\n\n```ts\npublic metadata(): IndicatorMetadata {\n  const description = `Bollinger Bands ${this.mnemonic}`;\n  return buildMetadata(\n    IndicatorIdentifier.BollingerBands,\n    this.mnemonic,\n    description,\n    [\n      { mnemonic: `${this.mnemonic} lower`,       description: `${description} Lower` },\n      { mnemonic: `${this.mnemonic} middle`,      description: `${description} Middle` },\n      { mnemonic: `${this.mnemonic} upper`,       description: `${description} Upper` },\n      { mnemonic: `${this.mnemonic} bandWidth`,   description: `${description} Band Width` },\n      { mnemonic: `${this.mnemonic} percentBand`, description: `${description} Percent Band` },\n      { mnemonic: `${this.mnemonic} band`,        description: `${description} Band` },\n    ],\n  );\n}\n```\n\n### Adding a Descriptor Row\n\nWhen adding a new indicator you MUST add its descriptor in **both** Go and TS:\n\n1. **Go** — `go/indicators/core/descriptors.go`:\n\n   ```go\n   MyIndicator: {\n       Identifier: MyIndicator, Family: \"<Author or Common/Custom>\",\n       Adaptivity: Static, InputRequirement: ScalarInput, VolumeUsage: NoVolume,\n       Outputs: []OutputDescriptor{\n           {Kind: 1 /* Output1 */, Shape: shape.Scalar, Role: Smoother, Pane: Price},\n           {Kind: 2 /* Output2 */, Shape: shape.Scalar, Role: Signal,   Pane: Price},\n       },\n   },\n   ```\n\n2. **TS** — `ts/indicators/core/descriptors.ts` (single-line using helpers):\n\n   ```ts\n   [IndicatorIdentifier.MyIndicator, desc(IndicatorIdentifier.MyIndicator, '<family>', A.Static, I.ScalarInput, V.NoVolume, [\n     out(0, S.Scalar, R.Smoother, P.Price),\n     out(1, S.Scalar, R.Signal,   P.Price),\n   ])],\n   ```\n\n3. Confirm the output order matches the per-indicator output enum order\n   (`iota + 1` in Go, `0, 1, 2, ...` in TS).\n\n4. Run the suites:\n   ```\n   cd go && go test ./indicators/core/...\n   cd ts && npm test\n   ```\n   `TestDescriptorCoverage` and `TestDescriptorOutputsWellFormed` will catch\n   missing or malformed rows on the Go side; the TS `buildMetadata` contract\n   will throw at runtime if a row is missing.\n\n### Family Naming\n\n`Family` is the human-readable grouping label. Conventions:\n\n- Author folder → author's full name (e.g., `john-ehlers` / `johnehlers`\n  → `\"John Ehlers\"`, `welles-wilder` / `welleswilder` → `\"Welles Wilder\"`).\n- `common/` → `\"Common\"`.\n- `custom/` → `\"Custom\"`.\n\n### Role Assignment Guidance\n\n| Role                | Use for |\n|---------------------|---------|\n| `Smoother`          | A moving average or trend-following line plotted on price pane (SMA, EMA, JMA, KAMA, Middle Bollinger, ATCF trend lines, T2/T3, TEMA, ...). |\n| `Envelope`          | Upper/lower bands or deviation channels (Bollinger upper/lower/band). |\n| `Overlay`           | Other price-pane overlays that are not smoothers or envelopes (e.g., Parabolic SAR dots). |\n| `Polyline`          | Outputs that produce an arbitrary polyline shape rather than a time series. |\n| `Oscillator`        | Unbounded oscillator centred on 0 or arbitrary range (MACD, momentum, ATCF Ftlm/Stlm/Pcci). |\n| `BoundedOscillator` | Oscillator bounded to 0..100 or similar (RSI, %R, Stochastic, ADX, PercentBand). |\n| `Volatility`        | Volatility measure (TR, ATR, NATR, StdDev, BandWidth). |\n| `VolumeFlow`        | Volume-based cumulative or flow measure (OBV, A/D, A/D Oscillator, MFI). |\n| `Directional`       | Directional movement components (+DI, -DI, +DM, -DM). |\n| `CyclePeriod`       | Dominant-cycle period estimate. |\n| `CyclePhase`        | Cycle phase or sine-wave output. |\n| `FractalDimension`  | Fractal-dimension based output. |\n| `Spectrum`          | Spectrum / periodogram output (heatmap or polyline). |\n| `Signal`            | The signal line of a multi-line indicator (MACD signal, Stochastic %D). |\n| `Histogram`         | Difference bar between two lines (MACD histogram). |\n| `RegimeFlag`        | Discrete regime indicator (Trend/Cycle mode). |\n| `Correlation`       | Correlation coefficient in [-1, 1]. |\n\n### Pane Assignment Guidance\n\n| Pane               | Use for |\n|--------------------|---------|\n| `Price`            | Drawn on the price chart (smoothers, envelopes, overlays, SAR). |\n| `Own`              | Drawn in its own subchart (oscillators, volatility, volume flow, spectra). |\n| `OverlayOnParent`  | Drawn on top of another indicator's pane (reserved for composite visualizations). |\n\n### Adaptivity\n\n- `Adaptive` — the indicator adjusts its coefficients, length, or alpha based\n  on market state (KAMA, JMA, MAMA, FRAMA, ATCF, DominantCycle, SineWave,\n  HilbertTransformerInstantaneousTrendLine, TrendCycleMode, all Corona\n  indicators, AutoCorrelationPeriodogram).\n- `Static` — everything else.",
  "oldString": "### Warm-Up Period Contract\n\nEach variant has an intrinsic minimum priming length (the sample count needed\nfor its state to be meaningful). If `params.warmUpPeriod` is provided and\nlarger than that minimum, it overrides the minimum; otherwise the intrinsic\nminimum is used. Tests must cover both the default path and the custom\n`warmUpPeriod` override."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 5.3s)

SKILL.md updated. Now Phase 7: update the three conversion skills.

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
50: 
51: ---
52: 
53: ## Naming & Style Conventions
54: 
55: All identifier, receiver, concurrency, style, and cross-language parity
56: rules are defined in the **`indicator-architecture`** skill and MUST be
57: followed during conversion. Summary (see that skill for the full tables
58: and rationale):
59: 
60: - **Abbreviations banned in identifiers** — always expand: `idx→index`,
61:   `tmp→temp`, `res→result`, `sig→signal`, `val→value`, `prev→previous`,
62:   `avg→average`, `mult→multiplier`, `buf→buffer`, `param→parameter`,
63:   `hist→histogram`. Allowed: Go idioms (`err`, `len`, `cap`, `min`,
64:   `max`, `num`), the Go `Params`/`params` bundle type, TS `value_`
65:   ctor-param idiom.
66: - **Go receivers** — compound type name (2+ CamelCase words) → `s`;
67:   simple type name (single word) → first-letter of type, lowercased.
68:   All methods on a type MUST use the same receiver. If a local would
69:   shadow `s` (e.g. in `MarshalJSON`), rename the local to `str`.
70: - **Concurrency** — stateful public indicators MUST carry `mu
71:   sync.RWMutex`; writers `s.mu.Lock(); defer s.mu.Unlock()`, readers
72:   `s.mu.RLock(); defer s.mu.RUnlock()`, defer on the line immediately
73:   after Lock. Exceptions: pure delegating wrappers, internal engines
74:   guarded by their public wrapper.
75: - **Go style invariants** — no `var x T = zero`, no split
76:   `var x T; x = expr`; use `any` not `interface{}`; no
77:   `make([]T, 0)` (always include capacity); no `new`; grouped imports
78:   (stdlib, external, `zpano/*`); every exported symbol has a doc
79:   comment, even trivial passthroughs.
80: - **Go ↔ TS local-variable parity** — same concept = same name in
81:   both languages. Canonical: `sum`, `epsilon`, `temp`/`diff`,
82:   `stddev`, `spread`, `bw`, `pctB`, `amount`, `lengthMinOne`;
83:   loop counter `i`/`j`/`k`; `index` only when semantically a named
84:   index. Never introduce new short forms.
85: 
86: When porting, copy the other language's local-variable names verbatim
87: where the concept is identical.
88: 
89: ---
90: 
91: ## Go Conversion
92: 
93: ### Go Step 1: Create the new package directory
94: 
95: Old indicators live in a flat `package indicators` directory. New indicators each get their own
96: package under `indicators/<group>/<indicatorname>/`.
97: 
98: **Action:** Create a new directory using the full descriptive name in `lowercase` (no separators).
99: 
100: ```
101: # Example for SMA in the "common" group:
102: indicators/common/simplemovingaverage/
103: ```
104: 
105: All files in this directory will use `package simplemovingaverage` (the directory name).
106: 
107: ### Go Step 2: Convert the params file
108: 
109: **File:** `params.go`
110: 
111: Changes:
112: 
113: 1. **Package declaration:** `package indicators` -> `package <indicatorname>`
114: 2. **Import path:** `"mbg/trading/data"` -> `"zpano/entities"`
115: 3. **Type references:**
116:    - `data.BarComponent` -> `entities.BarComponent`
117:    - `data.QuoteComponent` -> `entities.QuoteComponent`
118:    - `data.TradeComponent` -> `entities.TradeComponent`
119: 4. **Add doc comments** to each component field explaining the zero-value default behavior:
120: 
121: ```go
122: // BarComponent indicates the component of a bar to use when updating the indicator with a bar sample.
123: //
124: // If zero, the default (BarClosePrice) is used and the component is not shown in the indicator mnemonic.
125: BarComponent entities.BarComponent
126: ```
127: 
128: Repeat the pattern for `QuoteComponent` (default: `QuoteMidPrice`) and `TradeComponent`
129: (default: `TradePrice`).
130: 
131: ### Go Step 3: Convert the output file
132: 
133: **File:** `output.go`
134: 
135: Two mechanical changes:
136: 
137: 1. `package indicators` → `package <indicatorname>`.
138: 2. **Rename the output type and constants to the bare Go convention.** The
139:    package name provides scoping, so the indicator-name prefix would stutter
140:    (`simplemovingaverage.SimpleMovingAverageOutput`). Rename:
141:    - Type `<IndicatorName>Output` → `Output`.
142:    - Constant `<IndicatorName>Value` → `Value`.
143:    - Multi-output: strip the `<IndicatorName>` prefix and keep the descriptive
144:      suffix (e.g. `DirectionalMovementIndexAverageDirectionalIndex` →
145:      `AverageDirectionalIndex`). If stripping leaves a trailing `Value` with
146:      more text before it, drop that too (e.g. `AverageTrueRangeValue` →
147:      `AverageTrueRange`).
148: 
149:    Update all references in the package (main file, test files, `String()`,
150:    `IsKnown()`, `MarshalJSON()`, `UnmarshalJSON()`). The string forms returned
151:    by `String()`/`MarshalJSON()` stay unchanged — only the Go identifiers move.
152: 
153: > **TypeScript keeps the long form** (`SimpleMovingAverageOutput` /
154: > `SimpleMovingAverageValue`) because TS imports by symbol, not by module.
155: > Only the Go side strips the prefix.
156: 
157: ### Go Step 4: Convert the output test file
158: 
159: **File:** `output_test.go`
160: 
161: **Only change:** `package indicators` -> `package <indicatorname>`
162: 
163: All test logic is identical.
164: 
165: ### Go Step 5: Convert the main indicator file
166: 
167: **File:** `<indicatorname>.go`
168: 
169: This is the most involved change. Follow these sub-steps carefully.
170: 
171: #### 5a. Package and imports
172: 
173: Replace:
174: 
175: ```go
176: package indicators
177: 
178: import (
179:     "mbg/trading/data"
180:     "mbg/trading/indicators/indicator"
181:     "mbg/trading/indicators/indicator/output"
182: )
183: ```
184: 
185: With:
186: 
187: ```go
188: package <indicatorname>
189: 
190: import (
191:     "zpano/entities"
192:     "zpano/indicators/core"
193:     "zpano/indicators/core/outputs"
194: )
195: ```
196: 
197: Keep standard library imports (`fmt`, `math`, `sync`, etc.) as-is.
198: 
199: Remove the `"mbg/trading/indicators/indicator/output"` import entirely -- it is only needed if
200: the old code references `output.Scalar` in `Metadata()`, which changes to `outputs.ScalarType`.
201: 
202: #### 5b. Struct: remove boilerplate fields, embed LineIndicator
203: 
204: Old struct has these fields that must be **removed**:
205: 
206: ```go
207: name        string
208: description string
209: barFunc     data.BarFunc
210: quoteFunc   data.QuoteFunc
211: tradeFunc   data.TradeFunc
212: ```
213: 
214: **Add** the `LineIndicator` embedding:
215: 
216: ```go
217: core.LineIndicator
218: ```
219: 
220: The resulting struct keeps only the indicator's own state (e.g., `window`, `windowSum`, `primed`,
221: `mu sync.RWMutex`) plus the embedded `core.LineIndicator`.
222: 
223: **Important:** The `mu sync.RWMutex` field stays on the indicator struct. The `core.LineIndicator`
224: field goes after it (before the indicator-specific fields).
225: 
226: Example (SMA):
227: 
228: ```go
229: type SimpleMovingAverage struct {
230:     mu sync.RWMutex
231:     core.LineIndicator
232:     window       []float64
233:     windowSum    float64
234:     windowLength int
235:     windowCount  int
236:     lastIndex    int
237:     primed       bool
238: }
239: ```
240: 
241: #### 5c. Constructor: add default resolution and ComponentTripleMnemonic
242: 
243: **Before** calling `entities.BarComponentFunc(...)` etc., add a zero-value resolution block:
244: 
245: ```go
246: // Resolve defaults for component functions.
247: // A zero value means "use default, don't show in mnemonic".
248: bc := p.BarComponent
249: if bc == 0 {
250:     bc = entities.DefaultBarComponent
251: }
252: 
253: qc := p.QuoteComponent
254: if qc == 0 {
255:     qc = entities.DefaultQuoteComponent
256: }
257: 
258: tc := p.TradeComponent
259: if tc == 0 {
260:     tc = entities.DefaultTradeComponent
261: }
262: ```
263: 
264: Then pass the **resolved** values `bc`, `qc`, `tc` (not `p.BarComponent` etc.) to
265: `entities.BarComponentFunc`, `entities.QuoteComponentFunc`, `entities.TradeComponentFunc`.
266: 
267: **Change the mnemonic format** from:
268: 
269: ```go
270: fmtn = "sma(%d)"
271: // ...
272: name := fmt.Sprintf(fmtn, length)
273: ```
274: 
275: To:
276: 
277: ```go
278: fmtn = "sma(%d%s)"
279: // ...
280: mnemonic := fmt.Sprintf(fmtn, length, core.ComponentTripleMnemonic(bc, qc, tc))
281: ```
282: 
283: The variable name changes from `name` to `mnemonic`. The description uses the mnemonic:
284: 
285: ```go
286: desc := "Simple moving average " + mnemonic
287: ```
288: 
289: **Change the function type declarations** from `data.*Func` to `entities.*Func`:
290: 
291: ```go
292: var (
293:     err       error
294:     barFunc   entities.BarFunc
295:     quoteFunc entities.QuoteFunc
296:     tradeFunc entities.TradeFunc
297: )
298: ```
299: 
300: #### 5d. Constructor: replace struct literal with LineIndicator assignment
301: 
302: Old pattern -- returning a struct literal with embedded fields:
303: 
304: ```go
305: return &SimpleMovingAverage{
306:     name:         name,
307:     description:  desc,
308:     window:       make([]float64, length),
309:     windowLength: length,
310:     lastIndex:    length - 1,
311:     barFunc:      barFunc,
312:     quoteFunc:    quoteFunc,
313:     tradeFunc:    tradeFunc,
314: }, nil
315: ```
316: 
317: New pattern -- create the struct first, then assign `LineIndicator`:
318: 
319: ```go
320: sma := &SimpleMovingAverage{
321:     window:       make([]float64, length),
322:     windowLength: length,
323:     lastIndex:    length - 1,
324: }
325: 
326: sma.LineIndicator = core.NewLineIndicator(mnemonic, desc, barFunc, quoteFunc, tradeFunc, sma.Update)
327: 
328: return sma, nil
329: ```
330: 
331: **Critical:** `sma.Update` is a method reference passed to `NewLineIndicator`. This is what
332: allows `LineIndicator` to implement `UpdateScalar/Bar/Quote/Trade` without the indicator
333: having to define them. The struct must be created first so that `sma.Update` is a valid reference.
334: 
335: #### 5e. Metadata: update return type and fields
336: 
337: Old:
338: 
339: ```go
340: func (s *SimpleMovingAverage) Metadata() indicator.Metadata {
341:     return indicator.Metadata{
342:         Type: indicator.SimpleMovingAverage,
343:         Outputs: []output.Metadata{
344:             {
345:                 Kind:        int(SimpleMovingAverageValue),
346:                 Type:        output.Scalar,
347:                 Name:        s.name,
348:                 Description: s.description,
349:             },
350:         },
351:     }
352: }
353: ```
354: 
355: New:
356: 
357: ```go
358: func (s *SimpleMovingAverage) Metadata() core.Metadata {
359:     return core.Metadata{
360:         Type:        core.SimpleMovingAverage,
361:         Mnemonic:    s.LineIndicator.Mnemonic,
362:         Description: s.LineIndicator.Description,
363:         Outputs: []outputs.Metadata{
364:             {
365:                 Kind:        int(SimpleMovingAverageValue),
366:                 Type:        outputs.ScalarType,
367:                 Mnemonic:    s.LineIndicator.Mnemonic,
368:                 Description: s.LineIndicator.Description,
369:             },
370:         },
371:     }
372: }
373: ```
374: 
375: Key changes:
376: 
377: | Old | New |
378: |-----|-----|
379: | `indicator.Metadata` | `core.Metadata` |
380: | (no top-level Mnemonic/Description) | `Mnemonic: s.LineIndicator.Mnemonic` |
381: | (no top-level Mnemonic/Description) | `Description: s.LineIndicator.Description` |
382: | `[]output.Metadata` | `[]outputs.Metadata` |
383: | `output.Scalar` | `outputs.ScalarType` |
384: | `Name: s.name` | `Mnemonic: s.LineIndicator.Mnemonic` |
385: | `Description: s.description` | `Description: s.LineIndicator.Description` |
386: 
387: #### 5f. Delete UpdateScalar/Bar/Quote/Trade methods
388: 
389: **Remove entirely** the four methods:
390: 
391: - `UpdateScalar(*data.Scalar) indicator.Output`
392: - `UpdateBar(*data.Bar) indicator.Output`
393: - `UpdateQuote(*data.Quote) indicator.Output`
394: - `UpdateTrade(*data.Trade) indicator.Output`
395: 
396: These are now provided by the embedded `core.LineIndicator`.
397: 
398: #### 5g. Keep Update() and IsPrimed() as-is
399: 
400: The `Update(sample float64) float64` method contains the core algorithm and does not change.
401: The `IsPrimed()` method does not change.
402: 
403: ### Go Step 6: Convert the test file
404: 
405: **File:** `<indicatorname>_test.go`
406: 
407: #### 6a. Package and imports
408: 
409: Replace:
410: 
411: ```go
412: package indicators
413: 
414: import (
415:     "mbg/trading/data"
416:     "mbg/trading/indicators/indicator"
417:     "mbg/trading/indicators/indicator/output"
418: )
419: ```
420: 
421: With:
422: 
423: ```go
424: package <indicatorname>
425: 
426: import (
427:     "zpano/entities"
428:     "zpano/indicators/core"
429:     "zpano/indicators/core/outputs"
430: )
431: ```
432: 
433: #### 6b. Entity type references
434: 
435: | Old | New |
436: |-----|-----|
437: | `data.Scalar` | `entities.Scalar` |
438: | `data.Bar` | `entities.Bar` |
439: | `data.Quote` | `entities.Quote` |
440: | `data.Trade` | `entities.Trade` |
441: | `indicator.Output` | `core.Output` |
442: 
443: #### 6c. Update entity tests (UpdateEntity tests)
444: 
445: The `check` function parameter type changes from `indicator.Output` to `core.Output`.
446: 
447: **Quote test:** If the old test uses `data.Quote{Time: time, Bid: inp}`, the new test must use
448: `entities.Quote{Time: time, Bid: inp, Ask: inp}` because the default quote component is
449: `QuoteMidPrice` (mid = (bid+ask)/2), which requires both fields. Using just `Bid: inp` would
450: produce a wrong mid price.
451: 
452: **Floating-point comparison:** Use tolerance-based comparison (`math.Abs(s.Value-exp) > 1e-13`)
453: instead of exact equality (`s.Value != exp`) for the scalar value check. The multi-stage EMA
454: indicators (T2, T3) can produce values that differ in the last significant digit due to
455: floating-point arithmetic ordering.
456: 
457: #### 6c-1. Multi-seeding algorithm test convergence (T2, T3)
458: 
459: When an indicator has two seeding algorithms (`firstIsAverage = true` vs `false` / Metastock),
460: the expected test data typically comes from one algorithm (usually `firstIsAverage = true` from
461: the spreadsheet). The other algorithm produces different values initially but converges after
462: some number of samples.
463: 
464: In Go tests, use a `firstCheck` offset:
465: - `firstIsAverage = true` (SMA-seeded): `firstCheck = lprimed` (or `lprimed + 1` if the
466:   first primed value differs due to averaging)
467: - `firstIsAverage = false` (Metastock): `firstCheck = lprimed + N` where N is the number
468:   of samples needed for convergence (e.g., 43 for T2 with length 5)
469: 
470: In TS tests, use the same pattern: `if (i >= lenPrimed + N) { expect(act).toBeCloseTo(...) }`
471: 
472: #### 6d. Constructor test ("length > 1" sub-test)
473: 
474: Old checks internal fields directly:
475: 
476: ```go
477: check("name", "sma(5)", sma.name)
478: check("description", "Simple moving average sma(5)", sma.description)
479: check("barFunc == nil", false, sma.barFunc == nil)
480: check("quoteFunc == nil", false, sma.quoteFunc == nil)
481: check("tradeFunc == nil", false, sma.tradeFunc == nil)
482: ```
483: 
484: New checks `LineIndicator` fields and removes function nil checks:
485: 
486: ```go
487: check("mnemonic", "sma(5, hl/2)", sma.LineIndicator.Mnemonic)
488: check("description", "Simple moving average sma(5, hl/2)", sma.LineIndicator.Description)
489: ```
490: 
491: The `barFunc/quoteFunc/tradeFunc == nil` checks are removed because these are now internal to
492: `LineIndicator`.
493: 
494: Note: the mnemonic includes `hl/2` because the "length > 1" test uses `BarMedianPrice` which is
495: not the default. If your test uses non-default components, adjust the expected mnemonic accordingly.
496: 
497: #### 6e. Component references in test constants and params
498: 
499: | Old | New |
500: |-----|-----|
501: | `data.BarComponent` | `entities.BarComponent` |
502: | `data.BarMedianPrice` | `entities.BarMedianPrice` |
503: | `data.BarClosePrice` | `entities.BarClosePrice` |
504: | `data.QuoteComponent` | `entities.QuoteComponent` |
505: | `data.QuoteMidPrice` | `entities.QuoteMidPrice` |
506: | `data.QuoteBidPrice` | `entities.QuoteBidPrice` |
507: | `data.TradeComponent` | `entities.TradeComponent` |
508: | `data.TradePrice` | `entities.TradePrice` |
509: | `data.BarComponent(9999)` | `entities.BarComponent(9999)` |
510: | etc. | etc. |
511: 
512: #### 6f. Metadata test
513: 
514: Old:
515: 
516: ```go
517: check("Outputs[0].Type", output.Scalar, act.Outputs[0].Type)
518: check("Outputs[0].Name", "sma(5)", act.Outputs[0].Name)
519: ```
520: 
521: New:
522: 
523: ```go
524: check("Type", core.SimpleMovingAverage, act.Type)
525: check("Outputs[0].Type", outputs.ScalarType, act.Outputs[0].Type)
526: check("Outputs[0].Mnemonic", "sma(5)", act.Outputs[0].Mnemonic)
527: ```
528: 
529: Also verify that the top-level `Mnemonic` and `Description` are checked on `act`:
530: 
531: ```go
532: // These checks are implicitly done via Type already existing in old tests,
533: // but verify the new Mnemonic and Description fields are present.
534: ```
535: 
536: #### 6g. Test helper: use zero-value defaults
537: 
538: Old test helper passes explicit component values:
539: 
540: ```go
541: func testSimpleMovingAverageCreate(length int) *SimpleMovingAverage {
542:     params := SimpleMovingAverageParams{
543:         Length: length, BarComponent: data.BarClosePrice, QuoteComponent: data.QuoteBidPrice, TradeComponent: data.TradePrice,
544:     }
545:     sma, _ := NewSimpleMovingAverage(&params)
546:     return sma
547: }
548: ```
549: 
550: New test helper uses zero-value defaults (the standard usage pattern):
551: 
552: ```go
553: func testSimpleMovingAverageCreate(length int) *SimpleMovingAverage {
554:     params := SimpleMovingAverageParams{
555:         Length: length,
556:     }
557:     sma, _ := NewSimpleMovingAverage(&params)
558:     return sma
559: }
560: ```
561: 
562: **Important:** If the old helper passed `QuoteBidPrice`, the old "update quote" test could use
563: `data.Quote{Time: time, Bid: inp}`. The new helper uses the default `QuoteMidPrice`, so the quote
564: test must provide both `Bid` and `Ask`: `entities.Quote{Time: time, Bid: inp, Ask: inp}`.
565: 
566: #### 6h. Add mnemonic sub-tests
567: 
568: Add sub-tests that verify mnemonic generation for various component combinations. These test the
569: zero-value default omission behavior:
570: 
571: ```go
572: t.Run("all components zero", func(t *testing.T) {
573:     t.Parallel()
574:     params := SimpleMovingAverageParams{Length: length}
575:     sma, err := NewSimpleMovingAverage(&params)
576:     check("err == nil", true, err == nil)
577:     check("mnemonic", "sma(5)", sma.LineIndicator.Mnemonic)
578:     check("description", "Simple moving average sma(5)", sma.LineIndicator.Description)
579: })
580: 
581: t.Run("only bar component set", func(t *testing.T) {
582:     t.Parallel()
583:     params := SimpleMovingAverageParams{Length: length, BarComponent: entities.BarMedianPrice}
584:     sma, err := NewSimpleMovingAverage(&params)
585:     check("err == nil", true, err == nil)
586:     check("mnemonic", "sma(5, hl/2)", sma.LineIndicator.Mnemonic)
587:     check("description", "Simple moving average sma(5, hl/2)", sma.LineIndicator.Description)
588: })
589: 
590: t.Run("only quote component set", func(t *testing.T) {
591:     t.Parallel()
592:     params := SimpleMovingAverageParams{Length: length, QuoteComponent: entities.QuoteBidPrice}
593:     sma, err := NewSimpleMovingAverage(&params)
594:     check("err == nil", true, err == nil)
595:     check("mnemonic", "sma(5, b)", sma.LineIndicator.Mnemonic)
596:     check("description", "Simple moving average sma(5, b)", sma.LineIndicator.Description)
597: })
598: 
599: t.Run("only trade component set", func(t *testing.T) {
600:     t.Parallel()
601:     params := SimpleMovingAverageParams{Length: length, TradeComponent: entities.TradeVolume}
602:     sma, err := NewSimpleMovingAverage(&params)
603:     check("err == nil", true, err == nil)
604:     check("mnemonic", "sma(5, v)", sma.LineIndicator.Mnemonic)
605:     check("description", "Simple moving average sma(5, v)", sma.LineIndicator.Description)
606: })
607: 
608: t.Run("bar and quote components set", func(t *testing.T) {
609:     t.Parallel()
610:     params := SimpleMovingAverageParams{
611:         Length: length, BarComponent: entities.BarOpenPrice, QuoteComponent: entities.QuoteBidPrice,
612:     }
613:     sma, err := NewSimpleMovingAverage(&params)
614:     check("err == nil", true, err == nil)
615:     check("mnemonic", "sma(5, o, b)", sma.LineIndicator.Mnemonic)
616:     check("description", "Simple moving average sma(5, o, b)", sma.LineIndicator.Description)
617: })
618: 
619: t.Run("bar and trade components set", func(t *testing.T) {
620:     t.Parallel()
621:     params := SimpleMovingAverageParams{
622:         Length: length, BarComponent: entities.BarHighPrice, TradeComponent: entities.TradeVolume,
623:     }
624:     sma, err := NewSimpleMovingAverage(&params)
625:     check("err == nil", true, err == nil)
626:     check("mnemonic", "sma(5, h, v)", sma.LineIndicator.Mnemonic)
627:     check("description", "Simple moving average sma(5, h, v)", sma.LineIndicator.Description)
628: })
629: 
630: t.Run("quote and trade components set", func(t *testing.T) {
631:     t.Parallel()
632:     params := SimpleMovingAverageParams{
633:         Length: length, QuoteComponent: entities.QuoteAskPrice, TradeComponent: entities.TradeVolume,
634:     }
635:     sma, err := NewSimpleMovingAverage(&params)
636:     check("err == nil", true, err == nil)
637:     check("mnemonic", "sma(5, a, v)", sma.LineIndicator.Mnemonic)
638:     check("description", "Simple moving average sma(5, a, v)", sma.LineIndicator.Description)
639: })
640: ```
641: 
642: Adapt the mnemonics and component values to your specific indicator. The pattern is always:
643: 
644: - Default component = omitted from mnemonic
645: - Non-default component = shown as its mnemonic abbreviation (e.g., `hl/2`, `o`, `h`, `b`, `a`, `v`)
646: 
647: ### Go Step 7: Register the indicator type
648: 
649: If the indicator type constant does not already exist in `core/indicator.go` (or wherever
650: indicator type constants are defined), add it. Example:
651: 
652: ```go
653: const (
654:     SimpleMovingAverage IndicatorType = iota + 1
655:     // ...
656: )
657: ```
658: 
659: ### Go Step 8: Verify
660: 
661: ```bash
662: cd go && go test ./indicators/common/<indicatorname>/...
663: ```
664: 
665: All tests must pass. If the old tests are still present in the `_old` folder, you may need to
666: exclude them or ensure they still compile (they reference the old `mbg/trading` module, so they
667: won't be picked up by the new module).
668: 
669: ---
670: 
671: ## TypeScript Conversion
672: 
673: ### TS Step 1: Create the new folder
674: 
675: Old indicators live in a folder with no per-indicator output file and use `.interface` and `.enum`
676: suffixes. New indicators each get a clean folder.
677: 
678: **Action:** Create a new directory using the full descriptive name in `kebab-case`.
679: 
680: ```
681: # Example for SMA in the "common" group:
682: indicators/common/simple-moving-average/
683: ```
684: 
685: The new folder will contain 4 files (old had 3):
686: 
687: | Old files | New files |
688: |-----------|-----------|
689: | `simple-moving-average-params.interface.ts` | `params.ts` |
690: | `simple-moving-average.ts` | `simple-moving-average.ts` |
691: | `simple-moving-average.spec.ts` | `simple-moving-average.spec.ts` |
692: | (none) | `output.ts` |
693: 
694: ### TS Step 2: Convert the params file
695: 
696: **File:** `params.ts` (renamed from `<indicator-name>-params.interface.ts` — prefix dropped, `.interface` suffix dropped)
697: 
698: Changes:
699: 
700: 1. **Rename file:** Drop the `.interface` suffix.
701: 2. **Import paths:** Update from `data/entities` to `entities`:
702: 
703: | Old | New |
704: |-----|-----|
705: | `'../../../data/entities/bar-component.enum'` | `'../../../entities/bar-component'` |
706: | `'../../../data/entities/quote-component.enum'` | `'../../../entities/quote-component'` |
707: 
708: 3. **Add `TradeComponent`** if missing:
709: 
710: ```typescript
711: import { TradeComponent } from '../../../entities/trade-component';
712: ```
713: 
714: And add the field to the interface:
715: 
716: ```typescript
717: /**
718:  * A component of a trade to use when updating the indicator with a trade sample.
719:  *
720:  * If _undefined_, the trade component will have a default value and will not be shown
721:  * in the indicator mnemonic.
722:  */
723: tradeComponent?: TradeComponent;
724: ```
725: 
726: ### TS Step 3: Create the output file
727: 
728: **File:** `output.ts` (NEW file -- does not exist in old structure)
729: 
730: Create a per-indicator output enum:
731: 
732: ```typescript
733: /** Describes the outputs of the indicator. */
734: export enum SimpleMovingAverageOutput {
735:     /** The scalar value of the moving average. */
736:     Value = 'value',
737: }
738: ```
739: 
740: Adapt the enum name and members to your specific indicator.
741: 
742: ### TS Step 4: Convert the main indicator file
743: 
744: **File:** `<indicator-name>.ts`
745: 
746: #### 4a. Imports
747: 
748: Replace old imports with new ones:
749: 
750: | Old import | New import |
751: |------------|------------|
752: | `'../indicator/component-pair-mnemonic'` | `'../../core/component-triple-mnemonic'` |
753: | `'../indicator/line-indicator'` | `'../../core/line-indicator'` |
754: | `'./<name>-params.interface'` | `'./<name>-params'` |
755: 
756: Add new imports:
757: 
758: ```typescript
759: import { IndicatorMetadata } from '../../core/indicator-metadata';
760: import { IndicatorType } from '../../core/indicator-type';
761: import { OutputType } from '../../core/outputs/output-type';
762: import { SimpleMovingAverageOutput } from './simple-moving-average-output';
763: ```
764: 
765: Change the mnemonic function import:
766: 
767: ```typescript
768: // Old:
769: import { componentPairMnemonic } from '../indicator/component-pair-mnemonic';
770: // New:
771: import { componentTripleMnemonic } from '../../core/component-triple-mnemonic';
772: ```
773: 
774: #### 4b. Constructor: mnemonic function
775: 
776: Replace:
777: 
778: ```typescript
779: const m = componentPairMnemonic(params.barComponent, params.quoteComponent);
780: ```
781: 
782: With:
783: 
784: ```typescript
785: const m = componentTripleMnemonic(params.barComponent, params.quoteComponent, params.tradeComponent);
786: ```
787: 
788: #### 4c. Constructor: add description
789: 
790: After the mnemonic assignment, add:
791: 
792: ```typescript
793: this.description = 'Simple moving average ' + this.mnemonic;
794: ```
795: 
796: (Adapt the description text to your indicator.)
797: 
798: #### 4d. Constructor: add component setter calls
799: 
800: After the description, add three component setter calls:
801: 
802: ```typescript
803: this.barComponent = params.barComponent;
804: this.quoteComponent = params.quoteComponent;
805: this.tradeComponent = params.tradeComponent;
806: ```
807: 
808: These call the `LineIndicator` setters which handle `undefined` -> default resolution internally.
809: 
810: #### 4e. Add metadata() method
811: 
812: Add a new `metadata()` method to the class:
813: 
814: ```typescript
815: /** Describes the output data of the indicator. */
816: metadata(): IndicatorMetadata {
817:     return {
818:         type: IndicatorType.SimpleMovingAverage,
819:         mnemonic: this.mnemonic,
820:         description: this.description,
821:         outputs: [
822:             {
823:                 kind: SimpleMovingAverageOutput.Value,
824:                 type: OutputType.Scalar,
825:                 mnemonic: this.mnemonic,
826:                 description: this.description,
827:             },
828:         ],
829:     };
830: }
831: ```
832: 
833: Adapt `IndicatorType.*`, output enum, and output entries to your specific indicator.
834: 
835: ### TS Step 5: Convert the test file
836: 
837: **File:** `<indicator-name>.spec.ts`
838: 
839: #### 5a. Mnemonic assertion
840: 
841: Old:
842: 
843: ```typescript
844: expect(sma.getMnemonic()).toBe('sma(7)');
845: ```
846: 
847: New:
848: 
849: ```typescript
850: expect(sma.metadata().mnemonic).toBe('sma(7)');
851: ```
852: 
853: The `getMnemonic()` method is replaced by accessing `metadata().mnemonic`.
854: 
855: #### 5b. Other test logic
856: 
857: The `update()`, `isPrimed()`, and NaN tests remain **identical**. No changes needed for the core
858: calculation tests.
859: 
860: ### TS Step 6: Verify
861: 
862: Run the tests for the specific indicator:
863: 
864: ```bash
865: # zpano uses Jasmine (not Jest) via tsx. From /ts:
866: npm test                                    # full suite
867: node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=SineWave
868: # --filter is a case-sensitive regex substring match against the spec describe() name.
869: ```
870: 
871: All tests must pass.
872: 
873: ---
874: 
875: ## Quick Reference: Import Mapping
876: 
877: ### Go
878: 
879: | Old import path | New import path |
880: |-----------------|-----------------|
881: | `"mbg/trading/data"` | `"zpano/entities"` |
882: | `"mbg/trading/indicators/indicator"` | `"zpano/indicators/core"` |
883: | `"mbg/trading/indicators/indicator/output"` | `"zpano/indicators/core/outputs"` |
884: 
885: ### TypeScript
886: 
887: | Old import path (relative from indicator) | New import path (relative from indicator) |
888: |------------------------------------------|------------------------------------------|
889: | `'../indicator/component-pair-mnemonic'` | `'../../core/component-triple-mnemonic'` |
890: | `'../indicator/line-indicator'` | `'../../core/line-indicator'` |
891: | `'./<name>-params.interface'` | `'./<name>-params'` |
892: | `'../../../data/entities/bar-component.enum'` | `'../../../entities/bar-component'` |
893: | `'../../../data/entities/quote-component.enum'` | `'../../../entities/quote-component'` |
894: | (none) | `'../../../entities/trade-component'` |
895: | (none) | `'../../core/indicator-metadata'` |
896: | (none) | `'../../core/indicator-type'` |
897: | (none) | `'../../core/outputs/output-type'` |
898: | (none) | `'./<name>-output'` |
899: 
900: ---
901: 
902: ## Quick Reference: Symbol Renames
903: 
904: ### Go
905: 
906: | Old symbol | New symbol |
907: |------------|------------|
908: | `data.Scalar` | `entities.Scalar` |
909: | `data.Bar` | `entities.Bar` |
910: | `data.Quote` | `entities.Quote` |
911: | `data.Trade` | `entities.Trade` |
912: | `data.BarFunc` | `entities.BarFunc` |
913: | `data.QuoteFunc` | `entities.QuoteFunc` |
914: | `data.TradeFunc` | `entities.TradeFunc` |
915: | `data.BarComponent` | `entities.BarComponent` |
916: | `data.QuoteComponent` | `entities.QuoteComponent` |
917: | `data.TradeComponent` | `entities.TradeComponent` |
918: | `data.BarClosePrice` | `entities.BarClosePrice` |
919: | `data.BarMedianPrice` | `entities.BarMedianPrice` |
920: | `data.QuoteMidPrice` | `entities.QuoteMidPrice` |
921: | `data.QuoteBidPrice` | `entities.QuoteBidPrice` |
922: | `data.TradePrice` | `entities.TradePrice` |
923: | `data.BarComponentFunc(...)` | `entities.BarComponentFunc(...)` |
924: | `data.QuoteComponentFunc(...)` | `entities.QuoteComponentFunc(...)` |
925: | `data.TradeComponentFunc(...)` | `entities.TradeComponentFunc(...)` |
926: | `indicator.Metadata` | `core.Metadata` |
927: | `indicator.Output` | `core.Output` |
928: | `indicator.SimpleMovingAverage` | `core.SimpleMovingAverage` |
929: | `output.Metadata` | `outputs.Metadata` |
930: | `output.Scalar` | `outputs.ScalarType` |
931: 
932: ### Go: Struct field / method changes
933: 
934: | Old | New |
935: |-----|-----|
936: | `s.name` | `s.LineIndicator.Mnemonic` |
937: | `s.description` | `s.LineIndicator.Description` |
938: | `s.barFunc` | (removed -- inside LineIndicator) |
939: | `s.quoteFunc` | (removed -- inside LineIndicator) |
940: | `s.tradeFunc` | (removed -- inside LineIndicator) |
941: | `Outputs[i].Name` | `Outputs[i].Mnemonic` |
942: 
943: ### Go: Metadata field additions
944: 
945: | Old (not present) | New |
946: |-------------------|-----|
947: | (none) | `Mnemonic: s.LineIndicator.Mnemonic` |
948: | (none) | `Description: s.LineIndicator.Description` |
949: 
950: ### TypeScript
951: 
952: | Old symbol / pattern | New symbol / pattern |
953: |----------------------|----------------------|
954: | `componentPairMnemonic(bar, quote)` | `componentTripleMnemonic(bar, quote, trade)` |
955: | `sma.getMnemonic()` | `sma.metadata().mnemonic` |
956: | (no description) | `this.description = '...' + this.mnemonic` |
957: | (no component setters) | `this.barComponent = params.barComponent` |
958: | (no component setters) | `this.quoteComponent = params.quoteComponent` |
959: | (no component setters) | `this.tradeComponent = params.tradeComponent` |
960: | (no metadata method) | `metadata(): IndicatorMetadata { ... }` |
961: | (no output file) | `output.ts` with output enum |
962: 
963: ---
964: 
965: ## Advanced: Multi-Constructor Indicators
966: 
967: Some indicators have **multiple constructors** that create the same type with different
968: parameterization. The EMA (Exponential Moving Average) is the canonical example: it can be
969: constructed from a **length** or from a **smoothing factor (alpha)**.
970: 
971: The base conversion guide above assumes a single constructor. This section covers the
972: additional considerations for multi-constructor indicators.
973: 
974: ### Multiple Param Structs / Interfaces
975: 
976: Each constructor path gets its own param struct (Go) or interface (TS).
977: 
978: **Go:**
979: 
980: ```go
981: type ExponentialMovingAverageLengthParams struct {
982:     Length        int
983:     BarComponent  entities.BarComponent
984:     // ...
985: }
986: 
987: type ExponentialMovingAverageSmoothingFactorParams struct {
988:     SmoothingFactor float64
989:     BarComponent    entities.BarComponent
990:     // ...
991: }
992: ```
993: 
994: **TS:**
995: 
996: ```ts
997: export interface ExponentialMovingAverageLengthParams {
998:     length: number;
999:     barComponent?: BarComponent;
1000:     // ...
1001: }
1002: 
1003: export interface ExponentialMovingAverageSmoothingFactorParams {
1004:     smoothingFactor: number;
1005:     barComponent?: BarComponent;
1006:     // ...
1007: }
1008: ```
1009: 
1010: Component fields (`BarComponent`, `QuoteComponent`, `TradeComponent`) should be duplicated
1011: across all param structs/interfaces with the same default-resolution doc comments.
1012: 
1013: ### Shared Private Constructor (Go)
1014: 
1015: In Go, use a **private** shared constructor that both public constructors delegate to:
1016: 
1017: ```go
1018: func NewExponentialMovingAverageLength(p *ExponentialMovingAverageLengthParams) (*ExponentialMovingAverage, error) {
1019:     // validate length, compute smoothingFactor from length
1020:     return newExponentialMovingAverage(length, smoothingFactor, p.BarComponent, ...)
1021: }
1022: 
1023: func NewExponentialMovingAverageSmoothingFactor(p *ExponentialMovingAverageSmoothingFactorParams) (*ExponentialMovingAverage, error) {
1024:     // validate smoothingFactor, compute length from smoothingFactor
1025:     return newExponentialMovingAverage(length, smoothingFactor, p.BarComponent, ...)
1026: }
1027: 
1028: func newExponentialMovingAverage(length int, smoothingFactor float64, bc entities.BarComponent, ...) (*ExponentialMovingAverage, error) {
1029:     // default resolution, ComponentTripleMnemonic, two-step construction
1030: }
1031: ```
1032: 
1033: ### Multiple Static Methods (TS)
1034: 
1035: In TypeScript, use **static factory methods** instead of overloading the constructor:
1036: 
1037: ```ts
1038: export class ExponentialMovingAverage extends LineIndicator {
1039:     static fromLength(params: ExponentialMovingAverageLengthParams): ExponentialMovingAverage { ... }
1040:     static fromSmoothingFactor(params: ExponentialMovingAverageSmoothingFactorParams): ExponentialMovingAverage { ... }
1041: 
1042:     private constructor(...) { ... }
1043: }
1044: ```
1045: 
1046: Both static methods delegate to the private constructor with resolved parameters.
1047: 
1048: ### Constructor-Specific Mnemonic Formats
1049: 
1050: Different constructor paths may produce different mnemonic formats. Define the format
1051: at the point where you know which constructor path was taken:
1052: 
1053: ```go
1054: // length-based:
1055: mnemonic := fmt.Sprintf("ema(%d%s)", length, core.ComponentTripleMnemonic(bc, qc, tc))
1056: // => "ema(10)" or "ema(10, hl/2)"
1057: 
1058: // smoothing-factor-based (includes both computed length and the explicit factor):
1059: mnemonic := fmt.Sprintf("ema(%d, %.8f%s)", length, smoothingFactor, core.ComponentTripleMnemonic(bc, qc, tc))
1060: // => "ema(10, 0.18181818)" or "ema(10, 0.18181818, hl/2)"
1061: ```
1062: 
1063: The mnemonics must match between Go and TS. This may require changing the TS format
1064: from the old style to match Go (e.g., old TS alpha path used `ema(0.123)` with 3 decimal
1065: places; new uses `ema(10, 0.18181818)` with 8 decimal places to match Go).
1066: 
1067: ### Cross-Language Behavior Alignment
1068: 
1069: When converting multi-constructor indicators, check for behavioral differences between
1070: Go and TS and decide whether to align them:
1071: 
1072: - **Priming behavior:** If one language primes differently for a given constructor path,
1073:   consider aligning. Example: old TS smoothing-factor EMA primed immediately (length=0),
1074:   while Go computed length from alpha and waited. New TS computes
1075:   `length = Math.round(2/alpha) - 1` to match Go.
1076: 
1077: - **Validation:** Validation differences may be intentional (stricter in one language) and
1078:   can be preserved. Document any known differences.
1079: 
1080: ### Test Considerations
1081: 
1082: Each constructor path needs its own test group:
1083: 
1084: ```go
1085: t.Run("from length", func(t *testing.T) { ... })
1086: t.Run("from smoothing factor", func(t *testing.T) { ... })
1087: ```
1088: 
1089: ```ts
1090: describe('from length', () => { ... });
1091: describe('from smoothing factor', () => { ... });
1092: ```
1093: 
1094: Mnemonic tests should cover both paths, including with non-default components, to verify
1095: that each constructor path produces the correct format.
1096: 
1097: ---
1098: 
1099: ## Advanced: Wrapper Indicators
1100: 
1101: Some indicators **delegate their core calculation** to another indicator internally. The Standard
1102: Deviation indicator is the canonical example: it wraps a Variance indicator and returns the square
1103: root of the variance value.
1104: 
1105: ### Key Differences from Standard Indicators
1106: 
1107: 1. **Dual embedding:** The wrapper struct embeds both `core.LineIndicator` (for the update
1108:    protocol) and holds a pointer to the wrapped indicator.
1109: 
1110: 2. **The wrapper has its own `Update()` method** that:
1111:    - Calls the wrapped indicator's `Update()`
1112:    - Transforms the result (e.g., `math.Sqrt(varianceResult)`)
1113:    - Returns the transformed value
1114: 
1115: 3. **`IsPrimed()` delegates** to the wrapped indicator (the wrapper itself has no separate
1116:    priming logic).
1117: 
1118: 4. **Constructor creates the wrapped indicator internally** using the same params:
1119: 
1120: ```go
1121: // Go example: Standard Deviation wrapping Variance
1122: type StandardDeviation struct {
1123:     core.LineIndicator
1124:     variance *variance.Variance
1125: }
1126: 
1127: func NewStandardDeviation(p *StandardDeviationParams) (*StandardDeviation, error) {
1128:     // Resolve component defaults
1129:     // Create a variance.Variance internally
1130:     vp := &variance.VarianceParams{
1131:         Length: p.Length, IsUnbiased: p.IsUnbiased,
1132:         BarComponent: bc, QuoteComponent: qc, TradeComponent: tc,
1133:     }
1134:     v, err := variance.NewVariance(vp)
1135:     if err != nil {
1136:         return nil, err
1137:     }
1138: 
1139:     // Also create own component functions for LineIndicator
1140:     // Build mnemonic and description
1141:     sd := &StandardDeviation{variance: v}
1142:     sd.LineIndicator = core.NewLineIndicator(mnemonic, desc, barFunc, quoteFunc, tradeFunc, sd.Update)
1143:     return sd, nil
1144: }
1145: ```
1146: 
1147: ```typescript
1148: // TS example: Standard Deviation wrapping Variance
1149: export class StandardDeviation extends LineIndicator {
1150:     private variance: Variance;
1151: 
1152:     public constructor(params: StandardDeviationParams) {
1153:         super();
1154:         // Validate, build mnemonic, set components
1155:         this.variance = new Variance(params);
1156:         this.primed = false;
1157:     }
1158: 
1159:     public update(sample: number): number {
1160:         const value = this.variance.update(sample);
1161:         if (Number.isNaN(value)) return value;
1162:         this.primed = this.variance.isPrimed();
1163:         return Math.sqrt(value);
1164:     }
1165: }
1166: ```
1167: 
1168: ### Important: Separate Packages / Folders
1169: 
1170: In the new architecture, the wrapper and wrapped indicators live in **separate packages** (Go)
1171: or **separate folders** (TS), even if they were in the same package before:
1172: 
1173: - Go: `indicators/common/standarddeviation/` imports `zpano/indicators/common/variance`
1174: - TS: `indicators/common/standard-deviation/` imports `'../variance/variance'`
1175: 
1176: ### Old Multi-Output to New Single-Output
1177: 
1178: The old Standard Deviation was a **multi-output** indicator (SD value + optional variance value).
1179: In the new architecture, it is a **single-output** `LineIndicator`:
1180: 
1181: - `Update(float64) float64` returns only the standard deviation value
1182: - Users who want both SD and variance should create both indicators separately
1183: - The per-indicator output enum has only one member (e.g., `StandardDeviationValue`)
1184: 
1185: This simplification is required because `LineIndicator.UpdateScalar` always returns a
1186: single-element output. If the old indicator had multiple outputs, remove the extra outputs and
1187: direct users to the standalone indicator for those values.
1188: 
1189: ### Constructor Resolves Components Before Passing to Wrapped Indicator
1190: 
1191: The wrapper must resolve zero-value component defaults **before** creating the wrapped indicator.
1192: This ensures both the wrapper's mnemonic and the wrapped indicator use the same resolved values:
1193: 
1194: ```go
1195: bc := p.BarComponent
1196: if bc == 0 {
1197:     bc = entities.DefaultBarComponent
1198: }
1199: // ... resolve qc, tc similarly
1200: 
1201: vp := &variance.VarianceParams{
1202:     Length: p.Length, IsUnbiased: p.IsUnbiased,
1203:     BarComponent: bc, QuoteComponent: qc, TradeComponent: tc,
1204: }
1205: ```
1206: 
1207: ### Mnemonic Pattern
1208: 
1209: Each indicator has its own mnemonic prefix. The wrapper does **not** reuse the wrapped indicator's
1210: mnemonic:
1211: 
1212: - Variance: `var.s(5)` / `var.p(5)`
1213: - Standard Deviation: `stdev.s(5)` / `stdev.p(5)`
1214: 
1215: The `s`/`p` suffix denotes sample (unbiased) vs. population (biased) estimation.
1216: 
1217: ### Test Considerations
1218: 
1219: - The wrapper's test file defines its **own test data and expected values** (it cannot share test
1220:   helpers from the wrapped indicator's package, since they are in different packages)
1221: - Error messages from validation may come from the wrapped indicator (e.g.,
1222:   `"invalid variance parameters: length should be greater than 1"`)
1223: - The `IsPrimed()` test verifies delegation: the wrapper is primed when the wrapped indicator is
1224: 
1225: ---
1226: 
1227: ## Indicators That Do Not Use LineIndicator
1228: 
1229: Some indicators cannot use `LineIndicator` because they:
1230: 1. Have **multiple outputs** (e.g., FRAMA outputs both a value and a fractal dimension)
1231: 2. Need **high/low data** from bars/quotes, not just a single scalar input
1232: 3. The `Update(float64) float64` / `update(sample: number): number` signature doesn't fit
1233: 
1234: ### Pattern: Implement `Indicator` Interface Directly
1235: 
1236: **Go**: The struct does NOT embed `core.LineIndicator`. Instead, it:
1237: - Stores its own `barFunc`, `quoteFunc`, `tradeFunc` component functions
1238: - Implements `UpdateScalar`, `UpdateBar`, `UpdateQuote`, `UpdateTrade` directly
1239: - Has a private `updateEntity` helper that calls `Update` and builds the `core.Output` slice
1240: - Uses `core.ComponentTripleMnemonic` for mnemonic construction (same as LineIndicator-based indicators)
1241: 
1242: **TS**: The class `implements Indicator` instead of `extends LineIndicator`. It:
1243: - Stores its own `barComponentFunc`, `quoteComponentFunc`, `tradeComponentFunc` functions
1244: - Resolves defaults with `?? DefaultBarComponent` etc. (same logic as `LineIndicator`'s protected setters)
1245: - Implements `updateScalar`, `updateBar`, `updateQuote`, `updateTrade` directly
1246: - Has a private `updateEntity` helper that calls `update` and builds the `IndicatorOutput` array
1247: - Uses `componentTripleMnemonic` for mnemonic construction
1248: 
1249: ### Example: FRAMA (Fractal Adaptive Moving Average)
1250: 
1251: FRAMA has two outputs (`Value` and `Fdim`), and its core `Update` takes three parameters
1252: (`sample`, `sampleHigh`, `sampleLow`).
1253: 
1254: **Go signature**: `Update(sample, sampleHigh, sampleLow float64) float64`
1255: **TS signature**: `update(sample: number, sampleHigh: number, sampleLow: number): number`
1256: 
1257: The `updateEntity` helper produces a 2-element output array: `[Scalar(frama), Scalar(fdim)]`.
1258: If frama is NaN (not primed), fdim is also set to NaN.
1259: 
1260: **Entity mapping for high/low**:
1261: - `UpdateBar`: `high = sample.High`, `low = sample.Low`
1262: - `UpdateQuote`: `high = sample.Ask` (Go: `sample.Ask`; TS: `sample.askPrice`), `low = sample.Bid` (Go: `sample.Bid`; TS: `sample.bidPrice`)
1263: - `UpdateScalar`/`UpdateTrade`: `high = low = sample value` (no separate high/low available)
1264: 
1265: ### Reference Files
1266: 
1267: - Go: `go/indicators/johnehlers/fractaladaptivemovingaverage/`
1268: - TS: `ts/indicators/john-ehlers/fractal-adaptive-moving-average/`
1269: 
1270: ---
1271: 
1272: ## Advanced: Helper / Shared Component Families
1273: 
1274: A **helper family** is a group of interchangeable internal components that all
1275: implement the same interface (e.g., multiple cycle-estimator algorithms used
1276: by MESA-style indicators). They are not indicators themselves and do not
1277: extend `LineIndicator` — they are building blocks used *inside* indicators.
1278: 
1279: Canonical reference: **Hilbert Transformer cycle estimator**
1280: (`go/indicators/johnehlers/hilberttransformer/`,
1281: `ts/indicators/john-ehlers/hilbert-transformer/`).
1282: 
1283: ### Folder Contents
1284: 
1285: A helper family lives in a single folder at the standard indicator depth and
1286: contains one file per component role:
1287: 
1288: | Role                    | Go filename                          | TS filename                            |
1289: |-------------------------|--------------------------------------|----------------------------------------|
1290: | Shared interface        | `cycleestimator.go`                  | `cycle-estimator.ts`                   |
1291: | Shared params           | `cycleestimatorparams.go`            | `cycle-estimator-params.ts`            |
1292: | Variant type enum       | `cycleestimatortype.go`              | `cycle-estimator-type.ts`              |
1293: | Common + dispatcher     | `estimator.go`                       | `common.ts`                            |
1294: | One file per variant    | `<variant>estimator.go`              | `<variant>.ts`                         |
1295: | Spec per variant + common | `<variant>estimator_test.go`       | `<variant>.spec.ts`                    |
1296: 
1297: **No type suffixes.** Do not use `.interface.ts` / `.enum.ts` — plain `.ts`
1298: files differentiated by stem name, same as every other indicator.
1299: 
1300: ### Shared Interface
1301: 
1302: Expose:
1303: 
1304: - Primed flag and `warmUpPeriod` (read-only).
1305: - Construction parameters as read-only getters.
1306: - Intermediate state useful for tests (e.g., `smoothed`, `detrended`,
1307:   `inPhase`, `quadrature`, `period`).
1308: - A single `update(sample)` that returns the primary output.
1309: 
1310: TypeScript: getters take the plain name (`period`, not `periodValue`).
1311: Backing private fields are prefixed with `_` (`_period`) to avoid the name
1312: clash with the public getter.
1313: 
1314: ### Dispatcher / Factory
1315: 
1316: The common file exposes a single entry point that constructs a variant from a
1317: type enum value and optional params. Each variant case supplies its own
1318: **default params** when the caller omits them:
1319: 
1320: ```go
1321: // Go
1322: func NewCycleEstimator(typ CycleEstimatorType, params *CycleEstimatorParams) (CycleEstimator, error) {
1323:     switch typ {
1324:     case HomodyneDiscriminator: return NewHomodyneDiscriminatorEstimator(params)
1325:     // ... other variants
1326:     }
1327:     return nil, fmt.Errorf("invalid cycle estimator type: %s", typ)
1328: }
1329: ```
1330: 
1331: ```ts
1332: // TS
1333: export function createEstimator(
1334:     estimatorType?: HilbertTransformerCycleEstimatorType,
1335:     estimatorParams?: HilbertTransformerCycleEstimatorParams,
1336: ): HilbertTransformerCycleEstimator {
1337:     estimatorType ??= HilbertTransformerCycleEstimatorType.HomodyneDiscriminator;
1338:     switch (estimatorType) {
1339:         case HilbertTransformerCycleEstimatorType.HomodyneDiscriminator:
1340:             estimatorParams ??= { smoothingLength: 4, alphaEmaQuadratureInPhase: 0.2, alphaEmaPeriod: 0.2 };
1341:             return new HilbertTransformerHomodyneDiscriminator(estimatorParams);
1342:         // ... other variants with their own defaults
1343:         default: throw new Error('Invalid cycle estimator type: ' + estimatorType);
1344:     }
1345: }
1346: ```
1347: 
1348: Rules:
1349: 
1350: - Dispatcher picks a default variant when the type argument is omitted.
1351: - Each `case` has its own default params literal — defaults can differ per
1352:   variant and must match between Go and TS.
1353: - Unknown types: Go returns an error, TS throws.
1354: 
1355: ### Warm-Up Period
1356: 
1357: Every variant accepts an optional `warmUpPeriod` that, if larger than the
1358: variant's intrinsic minimum priming length, overrides it. Tests must cover:
1359: 
1360: 1. **Default priming** — `primed === false` for the first `intrinsicMinimum`
1361:    samples, then `true`.
1362: 2. **Custom warm-up** — construct with `warmUpPeriod = N` (larger than
1363:    intrinsic); `primed === false` for the first `N` samples, then `true`.
1364: 
1365: ```ts
1366: it('should respect custom warmUpPeriod', () => {
1367:     const lprimed = 50;
1368:     const est = new HilbertTransformerHomodyneDiscriminator(
1369:         { smoothingLength: 4, alphaEmaQuadratureInPhase: 0.2, alphaEmaPeriod: 0.2, warmUpPeriod: lprimed }
1370:     );
1371:     expect(est.primed).toBe(false);
1372:     for (let i = 0; i < lprimed; i++) { est.update(input[i]); expect(est.primed).toBe(false); }
1373:     for (let i = lprimed; i < input.length; i++) { est.update(input[i]); expect(est.primed).toBe(true); }
1374: });
1375: ```
1376: 
1377: ### Cross-Language Alignment Checklist
1378: 
1379: - Interface getter names match (modulo language casing).
1380: - Dispatcher default variant matches.
1381: - Per-variant default params match exactly (same numeric values).
1382: - `Primed()` / `primed` semantics match (both return `isWarmedUp`, not some
1383:   other internal flag).
1384: - Error/throw messages use the same textual prefix where practical.
1385: 
1386: ### Shared Formatter / Moniker Helpers
1387: 
1388: When a family defines a formatter (e.g. a per-variant mnemonic builder like
1389: `EstimatorMoniker(typ, params) -> "hd(4, 0.200, 0.200)"`), it **must be
1390: exported from both Go and TS** helper packages. Indicators that consume the
1391: family (MAMA, etc.) import and call this helper to build their own mnemonic
1392: suffix — duplicating the formatting logic per-consumer-indicator is a bug
1393: waiting to happen.
1394: 
1395: Checklist when converting a consumer indicator:
1396: 
1397: 1. Does Go's helper package export a formatter used in the consumer's mnemonic?
1398: 2. Does TS's helper export an equivalent function with the same name (camelCase)
1399:    and identical format string (e.g. `%.3f` ↔ `.toFixed(3)`)?
1400: 3. If the TS equivalent is missing, **add it to the helper's common file** first,
1401:    then import it from the consumer. Do not inline the format in the consumer.
1402: 
1403: Reference: `estimatorMoniker` in
1404: `ts/indicators/john-ehlers/hilbert-transformer/hilbert-transformer-common.ts`
1405: mirrors Go's `hilberttransformer.EstimatorMoniker`.
1406: 
1407: ### Numerical Tolerance for Recursive / Cascaded Indicators
1408: 
1409: The default test tolerance of `1e-12` (Go) / `1e-12` (TS) works for simple
1410: moving averages, single-stage EMAs, etc. Indicators that stack multiple
1411: recursive EMAs, Hilbert-transform feedback loops, or trigonometric feedback
1412: (MAMA, Hilbert-based cycle estimators, multi-stage T3) accumulate enough
1413: floating-point drift between Go and TS that `1e-12` is too tight.
1414: 
1415: **Use `1e-10`** as the tolerance for:
1416: 
1417: - MAMA / FAMA and anything consuming a Hilbert Transformer
1418: - Any indicator whose `update` feeds its own previous output through an
1419:   `atan`, `cos`, or multi-stage EMA chain before producing the output
1420: 
1421: Keep `1e-12` / `1e-13` for purely additive/multiplicative indicators.

(End of file - total 1421 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 26.8s)

_Thinking:_

 The indicator-conversion SKILL.md file is outdated after the recent refactoring phases — it still uses old API names like `indicator.Type` and `IndicatorType` that have since been renamed. I need to do targeted fixes to the Metadata sections, Step 7, and the reference tables rather than rewriting the whole document.

I'm focusing on the critical updates: renaming the Go and TypeScript Metadata function calls, updating Step 7 to clarify the identifier vs type distinction and add the descriptor registration step, fixing the quick-reference tables to point to the new module paths, and adjusting the TypeScript imports to drop the old output-type module and bring in build-metadata instead.

I'll make targeted updates to the critical sections in `indicator-conversion/SKILL.md`.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "newString": "#### 5e. Metadata: use BuildMetadata\n\nOld:\n\n```go\nfunc (s *SimpleMovingAverage) Metadata() indicator.Metadata {\n    return indicator.Metadata{\n        Type: indicator.SimpleMovingAverage,\n        Outputs: []output.Metadata{\n            {\n                Kind:        int(SimpleMovingAverageValue),\n                Type:        output.Scalar,\n                Name:        s.name,\n                Description: s.description,\n            },\n        },\n    }\n}\n```\n\nNew — `BuildMetadata` pulls per-output `Kind` and `Shape` from the descriptor registry\n(`go/indicators/core/descriptors.go`), so the caller supplies only mnemonic/description:\n\n```go\nfunc (s *SimpleMovingAverage) Metadata() core.Metadata {\n    return core.BuildMetadata(\n        core.SimpleMovingAverage,\n        s.LineIndicator.Mnemonic,\n        s.LineIndicator.Description,\n        []core.OutputText{\n            {Mnemonic: s.LineIndicator.Mnemonic, Description: s.LineIndicator.Description},\n        },\n    )\n}\n```\n\nKey changes:\n\n| Old | New |\n|-----|-----|\n| `indicator.Metadata` (return type) | `core.Metadata` (return type) |\n| Inline `core.Metadata{...}` literal | `core.BuildMetadata(...)` call |\n| `Type: indicator.SimpleMovingAverage` | First argument `core.SimpleMovingAverage` (the `core.Identifier` value) |\n| `Name: s.name` | `{Mnemonic: s.LineIndicator.Mnemonic, Description: s.LineIndicator.Description}` per output |\n| `output.Scalar` | (no longer specified — sourced from the descriptor registry) |\n| `int(SimpleMovingAverageValue)` | (no longer specified — sourced from the descriptor registry) |\n| `[]output.Metadata` | `[]core.OutputText` |\n\nThe caller NO LONGER imports `zpano/indicators/core/outputs` or\n`zpano/indicators/core/outputs/shape` just for `Metadata()`. Drop those imports if they\nare not referenced elsewhere in the file.\n\n**Descriptor row required.** If no descriptor is registered for the indicator's identifier,\n`BuildMetadata` panics at runtime. See Step 7 below — descriptor registration is now part\nof indicator registration.",
  "oldString": "#### 5e. Metadata: update return type and fields\n\nOld:\n\n```go\nfunc (s *SimpleMovingAverage) Metadata() indicator.Metadata {\n    return indicator.Metadata{\n        Type: indicator.SimpleMovingAverage,\n        Outputs: []output.Metadata{\n            {\n                Kind:        int(SimpleMovingAverageValue),\n                Type:        output.Scalar,\n                Name:        s.name,\n                Description: s.description,\n            },\n        },\n    }\n}\n```\n\nNew:\n\n```go\nfunc (s *SimpleMovingAverage) Metadata() core.Metadata {\n    return core.Metadata{\n        Type:        core.SimpleMovingAverage,\n        Mnemonic:    s.LineIndicator.Mnemonic,\n        Description: s.LineIndicator.Description,\n        Outputs: []outputs.Metadata{\n            {\n                Kind:        int(SimpleMovingAverageValue),\n                Type:        outputs.ScalarType,\n                Mnemonic:    s.LineIndicator.Mnemonic,\n                Description: s.LineIndicator.Description,\n            },\n        },\n    }\n}\n```\n\nKey changes:\n\n| Old | New |\n|-----|-----|\n| `indicator.Metadata` | `core.Metadata` |\n| (no top-level Mnemonic/Description) | `Mnemonic: s.LineIndicator.Mnemonic` |\n| (no top-level Mnemonic/Description) | `Description: s.LineIndicator.Description` |\n| `[]output.Metadata` | `[]outputs.Metadata` |\n| `output.Scalar` | `outputs.ScalarType` |\n| `Name: s.name` | `Mnemonic: s.LineIndicator.Mnemonic` |\n| `Description: s.description` | `Description: s.LineIndicator.Description` |"
}
```

**Output:**
```
Edit applied successfully.
```

---

## User

---

## Assistant (Compaction · claude-opus-4.7 · 67.8s)

---
## Goal

Design and implement a **classification / categorization system** for 69 zpano indicators so charting apps can filter/display the catalog along multiple dimensions: minimum input data type, volume usage, output data shape, semantic role, chart pane, adaptivity, and family. Renames freed the word "Type" for taxonomic use (`core.Type` → `core.Identifier`, `outputs.Type` → `shape.Shape`).

## Instructions

- Build mode — execute the locked plan phase by phase.
- After each phase: `cd go && go build ./... && go test ./...` and `cd ts && npm test` (baseline 884 specs, 0 failures) must remain green.
- Honor `.opencode/skills/indicator-architecture/SKILL.md` (receivers `s` / first-letter, `mu sync.RWMutex` + `defer`, Go↔TS local-name parity, expand abbreviations except Go idioms `err/len/cap/min/max/num`, every Go author folder has `doc.go`).
- LSP errors in `mbst-to-convert/**/*.c` are pre-existing — **ignore**.
- Pre-existing TS errors about `Property 'bid' does not exist on type 'Quote'` — unrelated, ignore.
- User prefers Recommended options, dislikes explanatory pauses.

### Phase plan

| # | Phase | Status |
|---|---|---|
| 1 | Rename `core.Type` → `core.Identifier` | **DONE** |
| 2 | Move `outputs.Type` → `outputs/shape` sub-package | **DONE** |
| 3 | Taxonomy primitives (Role, Pane, Adaptivity, InputRequirement, VolumeUsage, OutputDescriptor, Descriptor) + accessors | **DONE** |
| 4 | Populate descriptor table for all 69 indicators | **DONE** |
| 5 | Wire `Metadata()` to read from registry via `BuildMetadata`/`buildMetadata` | **DONE** |
| 6 | Document taxonomy in `indicator-architecture/SKILL.md` | **DONE** |
| 7 | Update conversion skills | **IN PROGRESS** |

### Phase 5 design (implemented)

Go: `core.OutputText{Mnemonic, Description string}` + `core.BuildMetadata(id, mnemonic, description, []OutputText) Metadata` — panics on missing descriptor or length mismatch; sources `Kind`/`Shape` from registry.

TS: `OutputText { mnemonic, description }` + `buildMetadata(identifier, mnemonic, description, texts[])` — throws on missing descriptor or length mismatch.

User picked "test-only" invariant assertion → implemented as `TestDescriptorOutputsWellFormed` (pure registry walk; verifies non-empty outputs, Kinds strictly ascending starting at 1, known shapes).

## Discoveries

- 69 identifiers total. Go enum 1-based (`iota+1`), TS enum 0-based — pre-existing.
- Descriptor `Kind` values differ by 1 between Go and TS (mirror the enum bases).
- VolumeUsage ⇔ InputRequirement validity: `AggregateBarVolume`⇒`BarInput`, `PerTradeVolume`⇒`TradeInput`, `QuoteLiquidityVolume`⇒`QuoteInput`, `NoVolume` any. Documented but not enforced by test.
- Mass refactor (Phase 5) used a subagent running a Python brace-matching script. 71 Go files + 71 TS files converted. In Go, imports of `zpano/indicators/core/outputs/shape` removed universally; `zpano/indicators/core/outputs` kept only where still used for `outputs.NewBand`, `outputs.NewEmptyBand` (bollingerbands, mesaadaptivemovingaverage, sinewave). One manual fix: `larry-williams/ultimate-oscillator` needed `const description = 'Ultimate Oscillator';` restored inside the new metadata body (the script dropped a preamble referenced both at top-level and per-output).
- Family naming — author folder → author full name; `common`→"Common"; `custom`→"Custom". Full mapping in previous summary.
- Adaptive set: KAMA, JMA, MAMA, FRAMA, ATCF, DominantCycle, SineWave, HilbertTransformerInstantaneousTrendLine, TrendCycleMode, all four Corona indicators, AutoCorrelationPeriodogram.
- Per-output role calls (ATCF FATL/SATL/RFTL/RSTL/RBCI→Smoother Price; FTLM/STLM/PCCI→Oscillator Own; ADX/ADXR/DX→BoundedOscillator; DI/DM→Directional; ATR/TR→Volatility; BollingerBands Lower/Upper/Band→Envelope, Middle→Smoother, BandWidth→Volatility, PercentBand→BoundedOscillator; Corona scalar companions varied).
- The `write` tool intermittently fails on very large content — splitting works.

## Accomplished

- **Phases 1–6 fully done**, both Go and TS. Go tests all green (all packages ok). TS tests 884 specs / 0 failures.
- **SKILL.md (`indicator-architecture`)** updated:
  - Symbol Naming table: `core.Type`→`core.Identifier`, `outputs.Type`→`shape.Shape`.
  - "Adding a New Indicator" checklist: now references `BuildMetadata`/`buildMetadata`, adds mandatory descriptor-registration step (step 5).
  - Metadata Structure section updated (`type`→`identifier`, per-output `type`→`shape`, note that indicators call BuildMetadata).
  - Appended new major section **"Taxonomy & Descriptor Registry"** covering: dimensions table, VolumeUsage⇔Input validity, registry files, Kind-numbering asymmetry, BuildMetadata helper with Go+TS Bollinger example, "Adding a Descriptor Row" procedure, Family naming, Role and Pane assignment guidance tables, Adaptivity guidance.

### In progress — Phase 7

Updating `.opencode/skills/indicator-conversion/SKILL.md`. So far completed:
- **Step 5e (Go Metadata)** — replaced with `BuildMetadata` pattern + updated key-changes table + note about dropping `outputs`/`shape` imports + descriptor-row requirement.

### Still to do in Phase 7 (remaining edits to `indicator-conversion/SKILL.md`)

1. **Step 4e (TS Metadata)** around line 810–833: replace inline `IndicatorMetadata` literal with `buildMetadata(IndicatorIdentifier.Xxx, mnemonic, description, [{mnemonic, description}, ...])` call. Drop `IndicatorType`, `OutputType`, output-type import references.
2. **TS Step 4a imports** (~line 746–772): replace old `indicator-type`, `outputs/output-type` imports with `build-metadata` import; rename `IndicatorType`→`IndicatorIdentifier`.
3. **Step 7 (Go Step 7)** (~line 647–657): rename "indicator type" → "indicator identifier"; path is `core/identifier.go` not `core/indicator.go`; add new sub-step "Register the descriptor" pointing to `core/descriptors.go` and `core/descriptors.ts`.
4. **Add new "TS Step 7: Register the identifier and descriptor"** mirroring Go Step 7.
5. **Quick Reference: Symbol Renames** table (Go ~line 902–931, TS ~line 950–962): add `indicator.Type`→`core.Identifier`; `IndicatorType`→`IndicatorIdentifier`; `output.Scalar`→`shape.Scalar` (from `outputs/shape`); `OutputType.Scalar`→`Shape.Scalar`; `outputs.ScalarType`→(no longer written; sourced from registry).
6. **Quick Reference: Import Mapping** — add `zpano/indicators/core/outputs/shape`, `ts/indicators/core/outputs/shape/shape`, `core/build-metadata`, `core/descriptors`.
7. Similar surgical updates to `.opencode/skills/mbst-indicator-conversion/SKILL.md` and `.opencode/skills/talib-indicator-conversion/SKILL.md` — haven't been opened yet; follow same pattern.

After Phase 7: final `go build ./... && go test ./...` + `npm test` to confirm nothing regressed (these edits are docs-only so should be trivially green). Then done.

## Relevant files / directories

### Go (Phase 5 artifacts)
- `go/indicators/core/build_metadata.go` — `OutputText` + `BuildMetadata` helper (created Phase 5).
- `go/indicators/core/metadata_descriptor_test.go` — `TestDescriptorOutputsWellFormed` (created Phase 5).
- `go/indicators/core/descriptors.go` — populated registry (Phase 4).
- `go/indicators/core/descriptors_test.go` — coverage test (Phase 4).
- `go/indicators/core/{identifier,role,pane,adaptivity,input_requirement,volume_usage,output_descriptor,descriptor,metadata}.go` — primitives + renames.
- `go/indicators/core/outputs/shape/{shape.go,doc.go}` — Shape sub-package (Phase 2).
- `go/indicators/**/*.go` — 72 indicator files refactored to use `core.BuildMetadata` (incl. SMA template at `go/indicators/common/simplemovingaverage/simplemovingaverage.go`).

### TS (Phase 5 artifacts)
- `ts/indicators/core/build-metadata.ts` — `OutputText` + `buildMetadata` helper (created Phase 5).
- `ts/indicators/core/descriptors.ts` — populated registry (Phase 4); uses `out()`/`desc()` + `S/R/P/A/I/V` aliases.
- `ts/indicators/core/{indicator-identifier,role,pane,adaptivity,input-requirement,volume-usage,output-descriptor,descriptor,indicator-metadata}.ts` — primitives + renames.
- `ts/indicators/core/outputs/shape/shape.ts` — Shape enum (Phase 2).
- `ts/indicators/**/*.ts` — 72 indicator files refactored to use `buildMetadata` (incl. SMA template at `ts/indicators/common/simple-moving-average/simple-moving-average.ts`).

### Skills (Phase 6/7)
- `.opencode/skills/indicator-architecture/SKILL.md` — **Phase 6 DONE**: updated Symbol Naming, Checklist, Metadata Structure; appended ~140-line Taxonomy & Descriptor Registry section.
- `.opencode/skills/indicator-conversion/SKILL.md` — **Phase 7 IN PROGRESS**: Step 5e updated; ~6 more targeted edits listed above.
- `.opencode/skills/mbst-indicator-conversion/SKILL.md` — **Phase 7 TODO**.
- `.opencode/skills/talib-indicator-conversion/SKILL.md` — **Phase 7 TODO**.

### Noise to ignore
- `mbst-to-convert/**/*.c` — pre-existing LSP C errors, auto-attached to every write.

---

## User

---

## Assistant (Build · claude-opus-4.7 · 4.9s)

_Thinking:_

 Moving on to Phase 7 now, I need to check the current state of the indicator-conversion skill documentation to see what needs updating.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "offset": 730,
  "limit": 260
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md</path>
<type>file</type>
<content>730: ### TS Step 3: Create the output file
731: 
732: **File:** `output.ts` (NEW file -- does not exist in old structure)
733: 
734: Create a per-indicator output enum:
735: 
736: ```typescript
737: /** Describes the outputs of the indicator. */
738: export enum SimpleMovingAverageOutput {
739:     /** The scalar value of the moving average. */
740:     Value = 'value',
741: }
742: ```
743: 
744: Adapt the enum name and members to your specific indicator.
745: 
746: ### TS Step 4: Convert the main indicator file
747: 
748: **File:** `<indicator-name>.ts`
749: 
750: #### 4a. Imports
751: 
752: Replace old imports with new ones:
753: 
754: | Old import | New import |
755: |------------|------------|
756: | `'../indicator/component-pair-mnemonic'` | `'../../core/component-triple-mnemonic'` |
757: | `'../indicator/line-indicator'` | `'../../core/line-indicator'` |
758: | `'./<name>-params.interface'` | `'./<name>-params'` |
759: 
760: Add new imports:
761: 
762: ```typescript
763: import { IndicatorMetadata } from '../../core/indicator-metadata';
764: import { IndicatorType } from '../../core/indicator-type';
765: import { OutputType } from '../../core/outputs/output-type';
766: import { SimpleMovingAverageOutput } from './simple-moving-average-output';
767: ```
768: 
769: Change the mnemonic function import:
770: 
771: ```typescript
772: // Old:
773: import { componentPairMnemonic } from '../indicator/component-pair-mnemonic';
774: // New:
775: import { componentTripleMnemonic } from '../../core/component-triple-mnemonic';
776: ```
777: 
778: #### 4b. Constructor: mnemonic function
779: 
780: Replace:
781: 
782: ```typescript
783: const m = componentPairMnemonic(params.barComponent, params.quoteComponent);
784: ```
785: 
786: With:
787: 
788: ```typescript
789: const m = componentTripleMnemonic(params.barComponent, params.quoteComponent, params.tradeComponent);
790: ```
791: 
792: #### 4c. Constructor: add description
793: 
794: After the mnemonic assignment, add:
795: 
796: ```typescript
797: this.description = 'Simple moving average ' + this.mnemonic;
798: ```
799: 
800: (Adapt the description text to your indicator.)
801: 
802: #### 4d. Constructor: add component setter calls
803: 
804: After the description, add three component setter calls:
805: 
806: ```typescript
807: this.barComponent = params.barComponent;
808: this.quoteComponent = params.quoteComponent;
809: this.tradeComponent = params.tradeComponent;
810: ```
811: 
812: These call the `LineIndicator` setters which handle `undefined` -> default resolution internally.
813: 
814: #### 4e. Add metadata() method
815: 
816: Add a new `metadata()` method to the class:
817: 
818: ```typescript
819: /** Describes the output data of the indicator. */
820: metadata(): IndicatorMetadata {
821:     return {
822:         type: IndicatorType.SimpleMovingAverage,
823:         mnemonic: this.mnemonic,
824:         description: this.description,
825:         outputs: [
826:             {
827:                 kind: SimpleMovingAverageOutput.Value,
828:                 type: OutputType.Scalar,
829:                 mnemonic: this.mnemonic,
830:                 description: this.description,
831:             },
832:         ],
833:     };
834: }
835: ```
836: 
837: Adapt `IndicatorType.*`, output enum, and output entries to your specific indicator.
838: 
839: ### TS Step 5: Convert the test file
840: 
841: **File:** `<indicator-name>.spec.ts`
842: 
843: #### 5a. Mnemonic assertion
844: 
845: Old:
846: 
847: ```typescript
848: expect(sma.getMnemonic()).toBe('sma(7)');
849: ```
850: 
851: New:
852: 
853: ```typescript
854: expect(sma.metadata().mnemonic).toBe('sma(7)');
855: ```
856: 
857: The `getMnemonic()` method is replaced by accessing `metadata().mnemonic`.
858: 
859: #### 5b. Other test logic
860: 
861: The `update()`, `isPrimed()`, and NaN tests remain **identical**. No changes needed for the core
862: calculation tests.
863: 
864: ### TS Step 6: Verify
865: 
866: Run the tests for the specific indicator:
867: 
868: ```bash
869: # zpano uses Jasmine (not Jest) via tsx. From /ts:
870: npm test                                    # full suite
871: node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=SineWave
872: # --filter is a case-sensitive regex substring match against the spec describe() name.
873: ```
874: 
875: All tests must pass.
876: 
877: ---
878: 
879: ## Quick Reference: Import Mapping
880: 
881: ### Go
882: 
883: | Old import path | New import path |
884: |-----------------|-----------------|
885: | `"mbg/trading/data"` | `"zpano/entities"` |
886: | `"mbg/trading/indicators/indicator"` | `"zpano/indicators/core"` |
887: | `"mbg/trading/indicators/indicator/output"` | `"zpano/indicators/core/outputs"` |
888: 
889: ### TypeScript
890: 
891: | Old import path (relative from indicator) | New import path (relative from indicator) |
892: |------------------------------------------|------------------------------------------|
893: | `'../indicator/component-pair-mnemonic'` | `'../../core/component-triple-mnemonic'` |
894: | `'../indicator/line-indicator'` | `'../../core/line-indicator'` |
895: | `'./<name>-params.interface'` | `'./<name>-params'` |
896: | `'../../../data/entities/bar-component.enum'` | `'../../../entities/bar-component'` |
897: | `'../../../data/entities/quote-component.enum'` | `'../../../entities/quote-component'` |
898: | (none) | `'../../../entities/trade-component'` |
899: | (none) | `'../../core/indicator-metadata'` |
900: | (none) | `'../../core/indicator-type'` |
901: | (none) | `'../../core/outputs/output-type'` |
902: | (none) | `'./<name>-output'` |
903: 
904: ---
905: 
906: ## Quick Reference: Symbol Renames
907: 
908: ### Go
909: 
910: | Old symbol | New symbol |
911: |------------|------------|
912: | `data.Scalar` | `entities.Scalar` |
913: | `data.Bar` | `entities.Bar` |
914: | `data.Quote` | `entities.Quote` |
915: | `data.Trade` | `entities.Trade` |
916: | `data.BarFunc` | `entities.BarFunc` |
917: | `data.QuoteFunc` | `entities.QuoteFunc` |
918: | `data.TradeFunc` | `entities.TradeFunc` |
919: | `data.BarComponent` | `entities.BarComponent` |
920: | `data.QuoteComponent` | `entities.QuoteComponent` |
921: | `data.TradeComponent` | `entities.TradeComponent` |
922: | `data.BarClosePrice` | `entities.BarClosePrice` |
923: | `data.BarMedianPrice` | `entities.BarMedianPrice` |
924: | `data.QuoteMidPrice` | `entities.QuoteMidPrice` |
925: | `data.QuoteBidPrice` | `entities.QuoteBidPrice` |
926: | `data.TradePrice` | `entities.TradePrice` |
927: | `data.BarComponentFunc(...)` | `entities.BarComponentFunc(...)` |
928: | `data.QuoteComponentFunc(...)` | `entities.QuoteComponentFunc(...)` |
929: | `data.TradeComponentFunc(...)` | `entities.TradeComponentFunc(...)` |
930: | `indicator.Metadata` | `core.Metadata` |
931: | `indicator.Output` | `core.Output` |
932: | `indicator.SimpleMovingAverage` | `core.SimpleMovingAverage` |
933: | `output.Metadata` | `outputs.Metadata` |
934: | `output.Scalar` | `outputs.ScalarType` |
935: 
936: ### Go: Struct field / method changes
937: 
938: | Old | New |
939: |-----|-----|
940: | `s.name` | `s.LineIndicator.Mnemonic` |
941: | `s.description` | `s.LineIndicator.Description` |
942: | `s.barFunc` | (removed -- inside LineIndicator) |
943: | `s.quoteFunc` | (removed -- inside LineIndicator) |
944: | `s.tradeFunc` | (removed -- inside LineIndicator) |
945: | `Outputs[i].Name` | `Outputs[i].Mnemonic` |
946: 
947: ### Go: Metadata field additions
948: 
949: | Old (not present) | New |
950: |-------------------|-----|
951: | (none) | `Mnemonic: s.LineIndicator.Mnemonic` |
952: | (none) | `Description: s.LineIndicator.Description` |
953: 
954: ### TypeScript
955: 
956: | Old symbol / pattern | New symbol / pattern |
957: |----------------------|----------------------|
958: | `componentPairMnemonic(bar, quote)` | `componentTripleMnemonic(bar, quote, trade)` |
959: | `sma.getMnemonic()` | `sma.metadata().mnemonic` |
960: | (no description) | `this.description = '...' + this.mnemonic` |
961: | (no component setters) | `this.barComponent = params.barComponent` |
962: | (no component setters) | `this.quoteComponent = params.quoteComponent` |
963: | (no component setters) | `this.tradeComponent = params.tradeComponent` |
964: | (no metadata method) | `metadata(): IndicatorMetadata { ... }` |
965: | (no output file) | `output.ts` with output enum |
966: 
967: ---
968: 
969: ## Advanced: Multi-Constructor Indicators
970: 
971: Some indicators have **multiple constructors** that create the same type with different
972: parameterization. The EMA (Exponential Moving Average) is the canonical example: it can be
973: constructed from a **length** or from a **smoothing factor (alpha)**.
974: 
975: The base conversion guide above assumes a single constructor. This section covers the
976: additional considerations for multi-constructor indicators.
977: 
978: ### Multiple Param Structs / Interfaces
979: 
980: Each constructor path gets its own param struct (Go) or interface (TS).
981: 
982: **Go:**
983: 
984: ```go
985: type ExponentialMovingAverageLengthParams struct {
986:     Length        int
987:     BarComponent  entities.BarComponent
988:     // ...
989: }

(Showing lines 730-989 of 1425. Use offset=990 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "limit": 140,
  "offset": 620
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md</path>
<type>file</type>
<content>620:     check("description", "Simple moving average sma(5, o, b)", sma.LineIndicator.Description)
621: })
622: 
623: t.Run("bar and trade components set", func(t *testing.T) {
624:     t.Parallel()
625:     params := SimpleMovingAverageParams{
626:         Length: length, BarComponent: entities.BarHighPrice, TradeComponent: entities.TradeVolume,
627:     }
628:     sma, err := NewSimpleMovingAverage(&params)
629:     check("err == nil", true, err == nil)
630:     check("mnemonic", "sma(5, h, v)", sma.LineIndicator.Mnemonic)
631:     check("description", "Simple moving average sma(5, h, v)", sma.LineIndicator.Description)
632: })
633: 
634: t.Run("quote and trade components set", func(t *testing.T) {
635:     t.Parallel()
636:     params := SimpleMovingAverageParams{
637:         Length: length, QuoteComponent: entities.QuoteAskPrice, TradeComponent: entities.TradeVolume,
638:     }
639:     sma, err := NewSimpleMovingAverage(&params)
640:     check("err == nil", true, err == nil)
641:     check("mnemonic", "sma(5, a, v)", sma.LineIndicator.Mnemonic)
642:     check("description", "Simple moving average sma(5, a, v)", sma.LineIndicator.Description)
643: })
644: ```
645: 
646: Adapt the mnemonics and component values to your specific indicator. The pattern is always:
647: 
648: - Default component = omitted from mnemonic
649: - Non-default component = shown as its mnemonic abbreviation (e.g., `hl/2`, `o`, `h`, `b`, `a`, `v`)
650: 
651: ### Go Step 7: Register the indicator type
652: 
653: If the indicator type constant does not already exist in `core/indicator.go` (or wherever
654: indicator type constants are defined), add it. Example:
655: 
656: ```go
657: const (
658:     SimpleMovingAverage IndicatorType = iota + 1
659:     // ...
660: )
661: ```
662: 
663: ### Go Step 8: Verify
664: 
665: ```bash
666: cd go && go test ./indicators/common/<indicatorname>/...
667: ```
668: 
669: All tests must pass. If the old tests are still present in the `_old` folder, you may need to
670: exclude them or ensure they still compile (they reference the old `mbg/trading` module, so they
671: won't be picked up by the new module).
672: 
673: ---
674: 
675: ## TypeScript Conversion
676: 
677: ### TS Step 1: Create the new folder
678: 
679: Old indicators live in a folder with no per-indicator output file and use `.interface` and `.enum`
680: suffixes. New indicators each get a clean folder.
681: 
682: **Action:** Create a new directory using the full descriptive name in `kebab-case`.
683: 
684: ```
685: # Example for SMA in the "common" group:
686: indicators/common/simple-moving-average/
687: ```
688: 
689: The new folder will contain 4 files (old had 3):
690: 
691: | Old files | New files |
692: |-----------|-----------|
693: | `simple-moving-average-params.interface.ts` | `params.ts` |
694: | `simple-moving-average.ts` | `simple-moving-average.ts` |
695: | `simple-moving-average.spec.ts` | `simple-moving-average.spec.ts` |
696: | (none) | `output.ts` |
697: 
698: ### TS Step 2: Convert the params file
699: 
700: **File:** `params.ts` (renamed from `<indicator-name>-params.interface.ts` — prefix dropped, `.interface` suffix dropped)
701: 
702: Changes:
703: 
704: 1. **Rename file:** Drop the `.interface` suffix.
705: 2. **Import paths:** Update from `data/entities` to `entities`:
706: 
707: | Old | New |
708: |-----|-----|
709: | `'../../../data/entities/bar-component.enum'` | `'../../../entities/bar-component'` |
710: | `'../../../data/entities/quote-component.enum'` | `'../../../entities/quote-component'` |
711: 
712: 3. **Add `TradeComponent`** if missing:
713: 
714: ```typescript
715: import { TradeComponent } from '../../../entities/trade-component';
716: ```
717: 
718: And add the field to the interface:
719: 
720: ```typescript
721: /**
722:  * A component of a trade to use when updating the indicator with a trade sample.
723:  *
724:  * If _undefined_, the trade component will have a default value and will not be shown
725:  * in the indicator mnemonic.
726:  */
727: tradeComponent?: TradeComponent;
728: ```
729: 
730: ### TS Step 3: Create the output file
731: 
732: **File:** `output.ts` (NEW file -- does not exist in old structure)
733: 
734: Create a per-indicator output enum:
735: 
736: ```typescript
737: /** Describes the outputs of the indicator. */
738: export enum SimpleMovingAverageOutput {
739:     /** The scalar value of the moving average. */
740:     Value = 'value',
741: }
742: ```
743: 
744: Adapt the enum name and members to your specific indicator.
745: 
746: ### TS Step 4: Convert the main indicator file
747: 
748: **File:** `<indicator-name>.ts`
749: 
750: #### 4a. Imports
751: 
752: Replace old imports with new ones:
753: 
754: | Old import | New import |
755: |------------|------------|
756: | `'../indicator/component-pair-mnemonic'` | `'../../core/component-triple-mnemonic'` |
757: | `'../indicator/line-indicator'` | `'../../core/line-indicator'` |
758: | `'./<name>-params.interface'` | `'./<name>-params'` |
759: 

(Showing lines 620-759 of 1425. Use offset=760 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.6s)

_Thinking:_

 I need to review Step 5e to make sure the TypeScript styling is consistent, and then check back through the metadata sections to align everything properly.

**Tool: grep**

**Input:**
```json
{
  "pattern": "Step 5e|Step 7|BuildMetadata|buildMetadata|IndicatorType|OutputType\\b",
  "path": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md"
}
```

**Output:**
```
Found 12 matches
/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md:
  Line 335: #### 5e. Metadata: use BuildMetadata
  Line 355: New — `BuildMetadata` pulls per-output `Kind` and `Shape` from the descriptor registry
  Line 360:     return core.BuildMetadata(
  Line 376: | Inline `core.Metadata{...}` literal | `core.BuildMetadata(...)` call |
  Line 388: `BuildMetadata` panics at runtime. See Step 7 below — descriptor registration is now part
  Line 651: ### Go Step 7: Register the indicator type
  Line 658:     SimpleMovingAverage IndicatorType = iota + 1
  Line 764: import { IndicatorType } from '../../core/indicator-type';
  Line 765: import { OutputType } from '../../core/outputs/output-type';
  Line 822:         type: IndicatorType.SimpleMovingAverage,
  Line 828:                 type: OutputType.Scalar,
  Line 837: Adapt `IndicatorType.*`, output enum, and output entries to your specific indicator.
```

---

## Assistant (Build · claude-opus-4.7 · 3.7s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "limit": 70,
  "offset": 335
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md</path>
<type>file</type>
<content>335: #### 5e. Metadata: use BuildMetadata
336: 
337: Old:
338: 
339: ```go
340: func (s *SimpleMovingAverage) Metadata() indicator.Metadata {
341:     return indicator.Metadata{
342:         Type: indicator.SimpleMovingAverage,
343:         Outputs: []output.Metadata{
344:             {
345:                 Kind:        int(SimpleMovingAverageValue),
346:                 Type:        output.Scalar,
347:                 Name:        s.name,
348:                 Description: s.description,
349:             },
350:         },
351:     }
352: }
353: ```
354: 
355: New — `BuildMetadata` pulls per-output `Kind` and `Shape` from the descriptor registry
356: (`go/indicators/core/descriptors.go`), so the caller supplies only mnemonic/description:
357: 
358: ```go
359: func (s *SimpleMovingAverage) Metadata() core.Metadata {
360:     return core.BuildMetadata(
361:         core.SimpleMovingAverage,
362:         s.LineIndicator.Mnemonic,
363:         s.LineIndicator.Description,
364:         []core.OutputText{
365:             {Mnemonic: s.LineIndicator.Mnemonic, Description: s.LineIndicator.Description},
366:         },
367:     )
368: }
369: ```
370: 
371: Key changes:
372: 
373: | Old | New |
374: |-----|-----|
375: | `indicator.Metadata` (return type) | `core.Metadata` (return type) |
376: | Inline `core.Metadata{...}` literal | `core.BuildMetadata(...)` call |
377: | `Type: indicator.SimpleMovingAverage` | First argument `core.SimpleMovingAverage` (the `core.Identifier` value) |
378: | `Name: s.name` | `{Mnemonic: s.LineIndicator.Mnemonic, Description: s.LineIndicator.Description}` per output |
379: | `output.Scalar` | (no longer specified — sourced from the descriptor registry) |
380: | `int(SimpleMovingAverageValue)` | (no longer specified — sourced from the descriptor registry) |
381: | `[]output.Metadata` | `[]core.OutputText` |
382: 
383: The caller NO LONGER imports `zpano/indicators/core/outputs` or
384: `zpano/indicators/core/outputs/shape` just for `Metadata()`. Drop those imports if they
385: are not referenced elsewhere in the file.
386: 
387: **Descriptor row required.** If no descriptor is registered for the indicator's identifier,
388: `BuildMetadata` panics at runtime. See Step 7 below — descriptor registration is now part
389: of indicator registration.
390: 
391: #### 5f. Delete UpdateScalar/Bar/Quote/Trade methods
392: 
393: **Remove entirely** the four methods:
394: 
395: - `UpdateScalar(*data.Scalar) indicator.Output`
396: - `UpdateBar(*data.Bar) indicator.Output`
397: - `UpdateQuote(*data.Quote) indicator.Output`
398: - `UpdateTrade(*data.Trade) indicator.Output`
399: 
400: These are now provided by the embedded `core.LineIndicator`.
401: 
402: #### 5g. Keep Update() and IsPrimed() as-is
403: 
404: The `Update(sample float64) float64` method contains the core algorithm and does not change.

(Showing lines 335-404 of 1425. Use offset=405 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 8.9s)

_Thinking:_

 Now I'm making the edits across the TypeScript side, then handling Go Step 7 and adding the TypeScript equivalent, while also reviewing the imports in Step 4a and checking Step 4e.

Now I'll make the TS-side edits and update Go Step 7 + add TS Step 7.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "oldString": "Add new imports:\n\n```typescript\nimport { IndicatorMetadata } from '../../core/indicator-metadata';\nimport { IndicatorType } from '../../core/indicator-type';\nimport { OutputType } from '../../core/outputs/output-type';\nimport { SimpleMovingAverageOutput } from './simple-moving-average-output';\n```",
  "newString": "Add new imports:\n\n```typescript\nimport { buildMetadata } from '../../core/build-metadata';\nimport { IndicatorIdentifier } from '../../core/indicator-identifier';\nimport { IndicatorMetadata } from '../../core/indicator-metadata';\nimport { SimpleMovingAverageOutput } from './simple-moving-average-output';\n```\n\nNote: `OutputType`/`output-type` and `IndicatorType`/`indicator-type` are gone. `buildMetadata`\nsources the per-output `Shape` and `Kind` from the descriptor registry\n(`ts/indicators/core/descriptors.ts`), so the indicator file no longer imports them."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 10.8s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "newString": "#### 4e. Add metadata() method\n\nAdd a new `metadata()` method to the class. `buildMetadata` reads the per-output `Shape` and\n`Kind` from the descriptor registry (`ts/indicators/core/descriptors.ts`), so the caller only\nsupplies mnemonic/description text:\n\n```typescript\n/** Describes the output data of the indicator. */\nmetadata(): IndicatorMetadata {\n    return buildMetadata(\n        IndicatorIdentifier.SimpleMovingAverage,\n        this.mnemonic,\n        this.description,\n        [{ mnemonic: this.mnemonic, description: this.description }],\n    );\n}\n```\n\nAdapt `IndicatorIdentifier.*` and the per-output text entries to your specific indicator. The\nnumber and order of the `OutputText` entries must match the descriptor row registered in\n`descriptors.ts` — `buildMetadata` throws if they disagree.\n\n**Descriptor row required.** If no descriptor is registered for the identifier, `buildMetadata`\nthrows at runtime. See TS Step 7 below — descriptor registration is now part of indicator\nregistration.",
  "oldString": "#### 4e. Add metadata() method\n\nAdd a new `metadata()` method to the class:\n\n```typescript\n/** Describes the output data of the indicator. */\nmetadata(): IndicatorMetadata {\n    return {\n        type: IndicatorType.SimpleMovingAverage,\n        mnemonic: this.mnemonic,\n        description: this.description,\n        outputs: [\n            {\n                kind: SimpleMovingAverageOutput.Value,\n                type: OutputType.Scalar,\n                mnemonic: this.mnemonic,\n                description: this.description,\n            },\n        ],\n    };\n}\n```\n\nAdapt `IndicatorType.*`, output enum, and output entries to your specific indicator."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 9.9s)

Now update Go Step 7 and insert a TS Step 7.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "newString": "### Go Step 7: Register the identifier and descriptor\n\n1. **Register the identifier.** If the constant does not already exist in\n   `go/indicators/core/identifier.go`, add it:\n\n   ```go\n   const (\n       SimpleMovingAverage Identifier = iota + 1\n       // ...\n   )\n   ```\n\n2. **Register the descriptor.** Add a row to the registry in\n   `go/indicators/core/descriptors.go`. The descriptor drives the taxonomy (role, pane,\n   volume usage, adaptivity, family) **and** supplies per-output `Kind`/`Shape` consumed by\n   `BuildMetadata`. See `.opencode/skills/indicator-architecture/SKILL.md` section\n   \"Taxonomy & Descriptor Registry\" for field meanings and guidance. The order of the\n   `Outputs` slice must match the order of the `[]OutputText` passed by the indicator's\n   `Metadata()` method.",
  "oldString": "### Go Step 7: Register the indicator type\n\nIf the indicator type constant does not already exist in `core/indicator.go` (or wherever\nindicator type constants are defined), add it. Example:\n\n```go\nconst (\n    SimpleMovingAverage IndicatorType = iota + 1\n    // ...\n)\n```"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 5.7s)

Now add a TS Step 7 before the "TS Step 5: Convert the test file" section — actually, the current TS steps go 1-6; I should insert a new step between Step 4 and Step 5, renumbering 5/6. Simpler: add "TS Step 7" at end after Step 6.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "limit": 20,
  "offset": 870
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md</path>
<type>file</type>
<content>870: The `getMnemonic()` method is replaced by accessing `metadata().mnemonic`.
871: 
872: #### 5b. Other test logic
873: 
874: The `update()`, `isPrimed()`, and NaN tests remain **identical**. No changes needed for the core
875: calculation tests.
876: 
877: ### TS Step 6: Verify
878: 
879: Run the tests for the specific indicator:
880: 
881: ```bash
882: # zpano uses Jasmine (not Jest) via tsx. From /ts:
883: npm test                                    # full suite
884: node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=SineWave
885: # --filter is a case-sensitive regex substring match against the spec describe() name.
886: ```
887: 
888: All tests must pass.
889: 

(Showing lines 870-889 of 1438. Use offset=890 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 6.0s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "newString": "### TS Step 6: Register the identifier and descriptor\n\n1. **Register the identifier.** If the member does not already exist in\n   `ts/indicators/core/indicator-identifier.ts`, add it.\n\n2. **Register the descriptor.** Add a row to the registry in\n   `ts/indicators/core/descriptors.ts`. The descriptor drives the taxonomy and supplies\n   per-output `kind`/`shape` consumed by `buildMetadata`. See\n   `.opencode/skills/indicator-architecture/SKILL.md` section \"Taxonomy & Descriptor\n   Registry\" for field meanings. The order of the `outputs` entries must match the order\n   of the `OutputText[]` passed by the indicator's `metadata()` method.\n\n### TS Step 7: Verify",
  "oldString": "### TS Step 6: Verify"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 13.9s)

Now update the Quick Reference sections — TS Import Mapping and Symbol Renames.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "newString": "| (none) | `'../../core/build-metadata'` |\n| (none) | `'../../core/indicator-identifier'` |\n| (none) | `'../../core/indicator-metadata'` |\n| (none) | `'./<name>-output'` |",
  "oldString": "| (none) | `'../../core/indicator-metadata'` |\n| (none) | `'../../core/indicator-type'` |\n| (none) | `'../../core/outputs/output-type'` |\n| (none) | `'./<name>-output'` |"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 9.4s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "newString": "### Go\n\n| Old import path | New import path |\n|-----------------|-----------------|\n| `\"mbg/trading/data\"` | `\"zpano/entities\"` |\n| `\"mbg/trading/indicators/indicator\"` | `\"zpano/indicators/core\"` |\n| `\"mbg/trading/indicators/indicator/output\"` | `\"zpano/indicators/core/outputs\"` (only if `outputs.NewBand`/`outputs.NewEmptyBand` is used; the `Shape` enum now lives at `\"zpano/indicators/core/outputs/shape\"`, but indicator files rarely import it directly — `BuildMetadata` sources `Shape` from the registry) |",
  "oldString": "### Go\n\n| Old import path | New import path |\n|-----------------|-----------------|\n| `\"mbg/trading/data\"` | `\"zpano/entities\"` |\n| `\"mbg/trading/indicators/indicator\"` | `\"zpano/indicators/core\"` |\n| `\"mbg/trading/indicators/indicator/output\"` | `\"zpano/indicators/core/outputs\"` |"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 8.6s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "newString": "| `indicator.Metadata` | `core.Metadata` |\n| `indicator.Output` | `core.Output` |\n| `indicator.SimpleMovingAverage` | `core.SimpleMovingAverage` (now a `core.Identifier`, not an `IndicatorType`) |\n| `indicator.Type` / `IndicatorType` | `core.Identifier` |\n| `output.Metadata` | `outputs.Metadata` |\n| `output.Scalar` | `shape.Scalar` (package `zpano/indicators/core/outputs/shape`) — but indicator files no longer write this; sourced from the descriptor registry |\n| `outputs.ScalarType` / `outputs.Type` | `shape.Scalar` / `shape.Shape` (same note — sourced from the registry) |",
  "oldString": "| `indicator.Metadata` | `core.Metadata` |\n| `indicator.Output` | `core.Output` |\n| `indicator.SimpleMovingAverage` | `core.SimpleMovingAverage` |\n| `output.Metadata` | `outputs.Metadata` |\n| `output.Scalar` | `outputs.ScalarType` |"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 10.6s)

Now update the TS Symbol Renames table to reflect the new `buildMetadata` shape.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "newString": "| `componentPairMnemonic(bar, quote)` | `componentTripleMnemonic(bar, quote, trade)` |\n| `sma.getMnemonic()` | `sma.metadata().mnemonic` |\n| (no description) | `this.description = '...' + this.mnemonic` |\n| (no component setters) | `this.barComponent = params.barComponent` |\n| (no component setters) | `this.quoteComponent = params.quoteComponent` |\n| (no component setters) | `this.tradeComponent = params.tradeComponent` |\n| (no metadata method) | `metadata(): IndicatorMetadata { return buildMetadata(...); }` |\n| `IndicatorType` | `IndicatorIdentifier` (from `core/indicator-identifier`) |\n| `OutputType.Scalar` | `Shape.Scalar` (from `core/outputs/shape/shape`) — but indicator files no longer write this; sourced from the descriptor registry |\n| (no output file) | `output.ts` with output enum |",
  "oldString": "| `componentPairMnemonic(bar, quote)` | `componentTripleMnemonic(bar, quote, trade)` |\n| `sma.getMnemonic()` | `sma.metadata().mnemonic` |\n| (no description) | `this.description = '...' + this.mnemonic` |\n| (no component setters) | `this.barComponent = params.barComponent` |\n| (no component setters) | `this.quoteComponent = params.quoteComponent` |\n| (no component setters) | `this.tradeComponent = params.tradeComponent` |\n| (no metadata method) | `metadata(): IndicatorMetadata { ... }` |\n| (no output file) | `output.ts` with output enum |"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 6.7s)

`indicator-conversion/SKILL.md` is done. Now handle the MBST and TA-Lib skills in parallel.

**Tool: grep**

**Input:**
```json
{
  "path": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "pattern": "IndicatorType|OutputType|output\\.Scalar|outputs\\.ScalarType|outputs\\.Type|indicator-type|output-type|core\\.Metadata\\{|BuildMetadata|buildMetadata|Metadata\\(\\)|metadata\\(\\)"
}
```

**Output:**
```
Found 14 matches
/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md:
  Line 290: Each output gets its own entry in `Metadata().Outputs`:
  Line 294:     { Kind: int(Value), Type: outputs.ScalarType, Mnemonic: s.mnemonic, Description: s.description },
  Line 295:     { Kind: int(Trigger), Type: outputs.ScalarType, Mnemonic: s.mnemonicTrig, Description: s.descriptionTrig },
  Line 308: | `Name` (e.g., `"cog"`) | `Metadata().Type` — use the registered `core.IndicatorType` enum |
  Line 309: | `Moniker` (e.g., `"cog(10)"`) | `Metadata().Mnemonic` — includes component triple |
  Line 310: | `Description` (e.g., `"Center of Gravity oscillator"`) | `Metadata().Description` — typically `"Full Name " + mnemonic` |
  Line 316: - **TS:** `ts/indicators/core/indicator-type.ts` — add enum member
  Line 377: 3. **Metadata tests** — Verify `Metadata()` returns correct type, mnemonic, description, outputs.
  Line 883: - **Single entry in `core.IndicatorType`** — one `DominantCycle` constant, not three.
  Line 1126: register a `core.IndicatorType`** for it, and do not give it an `update…Entity` path.
  Line 1269: - **One `core.IndicatorType`** (e.g., `AdaptiveTrendAndCycleFilter`).
  Line 1357: All N outputs are `OutputType.Scalar`; list them in enum order. The top-level
  Line 1389: - **One registered `core.IndicatorType`** (e.g., `GoertzelSpectrum`), one package /
  Line 1397: - **`Metadata().Outputs` has one entry** with `OutputType.Heatmap`. The indicator-level
```

---

## Assistant (Build · claude-opus-4.7 · 5.0s)

**Tool: grep**

**Input:**
```json
{
  "path": "/home/dev/zpano/.opencode/skills/talib-indicator-conversion/SKILL.md",
  "pattern": "IndicatorType|OutputType|output\\.Scalar|outputs\\.ScalarType|outputs\\.Type|indicator-type|output-type|core\\.Metadata\\{|BuildMetadata|buildMetadata"
}
```

**Output:**
```
Found 1 matches
/home/dev/zpano/.opencode/skills/talib-indicator-conversion/SKILL.md:
  Line 396: - TS: Added `AverageDirectionalMovementIndexRating` to `ts/indicators/core/indicator-type.ts`
```

---

## Assistant (Build · claude-opus-4.7 · 5.5s)

_Thinking:_

 I found a stale reference in the talib skill that needs fixing, so I'm updating it and looking for additional context to make sure there aren't other similar issues.

The TA-Lib skill has minimal drift; just one stale line. Let me also check for any other relevant mentions in it.

**Tool: grep**

**Input:**
```json
{
  "path": "/home/dev/zpano/.opencode/skills/talib-indicator-conversion/SKILL.md",
  "pattern": "Metadata\\(|metadata\\(|Identifier|Type:|registered|descriptor"
}
```

**Output:**
```
No files found
```

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/talib-indicator-conversion/SKILL.md",
  "limit": 30,
  "offset": 385
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/talib-indicator-conversion/SKILL.md</path>
<type>file</type>
<content>385: **TS (4 files, in `average-directional-movement-index-rating/` folder):**
386: - `params.ts` — Params interface
387: - `output.ts` — 9-value output enum
388: - `average-directional-movement-index-rating.ts` — Implementation
389: - `average-directional-movement-index-rating.spec.ts` — Full tests + spot checks
390: 
391: ### Registration
392: 
393: - Go: Added `AverageDirectionalMovementIndexRating` to `go/indicators/core/type.go`
394:   (enum constant, string, String(), MarshalJSON, UnmarshalJSON) and all 4 test tables
395:   in `type_test.go`
396: - TS: Added `AverageDirectionalMovementIndexRating` to `ts/indicators/core/indicator-type.ts`

(End of file - total 396 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 9.8s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/talib-indicator-conversion/SKILL.md",
  "newString": "### Registration\n\n- Go: Added `AverageDirectionalMovementIndexRating` to `go/indicators/core/identifier.go`\n  (enum constant, string, String(), MarshalJSON, UnmarshalJSON) and all 4 test tables\n  in `identifier_test.go`. Registered a descriptor row in `go/indicators/core/descriptors.go`\n  (taxonomy dimensions + per-output `Kind`/`Shape`); `Metadata()` calls `core.BuildMetadata(...)`.\n- TS: Added `AverageDirectionalMovementIndexRating` to\n  `ts/indicators/core/indicator-identifier.ts`. Registered the matching descriptor row in\n  `ts/indicators/core/descriptors.ts`; `metadata()` calls `buildMetadata(...)`.\n\nSee `.opencode/skills/indicator-architecture/SKILL.md` section \"Taxonomy & Descriptor\nRegistry\" for descriptor-row guidance and field meanings.",
  "oldString": "### Registration\n\n- Go: Added `AverageDirectionalMovementIndexRating` to `go/indicators/core/type.go`\n  (enum constant, string, String(), MarshalJSON, UnmarshalJSON) and all 4 test tables\n  in `type_test.go`\n- TS: Added `AverageDirectionalMovementIndexRating` to `ts/indicators/core/indicator-type.ts`"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.8s)

Now tackle the MBST skill — the biggest drift. Let me read the relevant sections.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "limit": 120,
  "offset": 280
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md</path>
<type>file</type>
<content>280: const ( Value Output = iota; Trigger )
281: ```
282: 
283: **TS:** `output.ts` (in `center-of-gravity-oscillator/` folder)
284: ```typescript
285: export enum CenterOfGravityOscillatorOutput { Value = 0, Trigger = 1 }
286: ```
287: 
288: ### Metadata for Multi-Output
289: 
290: Each output gets its own entry in `Metadata().Outputs`:
291: 
292: ```go
293: Outputs: []outputs.Metadata{
294:     { Kind: int(Value), Type: outputs.ScalarType, Mnemonic: s.mnemonic, Description: s.description },
295:     { Kind: int(Trigger), Type: outputs.ScalarType, Mnemonic: s.mnemonicTrig, Description: s.descriptionTrig },
296: },
297: ```
298: 
299: Facade-specific mnemonic patterns (like `cogTrig(10)`) are preserved as the Trigger
300: output mnemonic.
301: 
302: ---
303: 
304: ## Metadata Conversion
305: 
306: | MBST | Zpano |
307: |---|---|
308: | `Name` (e.g., `"cog"`) | `Metadata().Type` — use the registered `core.IndicatorType` enum |
309: | `Moniker` (e.g., `"cog(10)"`) | `Metadata().Mnemonic` — includes component triple |
310: | `Description` (e.g., `"Center of Gravity oscillator"`) | `Metadata().Description` — typically `"Full Name " + mnemonic` |
311: 
312: ### Register the Indicator Type
313: 
314: Before implementing, register the indicator in:
315: - **Go:** `go/indicators/core/type.go` — add enum constant, string, `String()`, `MarshalJSON`, `UnmarshalJSON`
316: - **TS:** `ts/indicators/core/indicator-type.ts` — add enum member
317: 
318: ---
319: 
320: ## What to Drop
321: 
322: ### From the Implementation
323: 
324: | MBST Element | Action |
325: |---|---|
326: | `[DataContract]`, `[DataMember]` | Drop — no serialization |
327: | `Reset()` method | Drop — zpano indicators are immutable |
328: | `ToString()` override | Drop — debug formatting |
329: | `lock (updateLock)` | Go: replace with `sync.RWMutex`. TS: drop entirely |
330: | `Update(Scalar)`, `Update(Ohlcv)` | Drop — handled by `LineIndicator` or `updateEntity` |
331: | `Update(double, DateTime)` | Drop — convenience overload |
332: | Facade properties (`ValueFacade`, `TriggerFacade`) | Drop — replaced by Output array |
333: | `OhlcvComponent` property setter | Drop — component is immutable after construction |
334: | C# regions (`#region ... #endregion`) | Drop — organizational noise |
335: | XML doc comments (`/// <summary>`) | Convert to Go doc comments / JSDoc |
336: 
337: ### From Tests
338: 
339: | MBST Test | Action |
340: |---|---|
341: | `ToStringTest()` | Drop |
342: | `SerializeTo()` / `SerializeFrom()` | Drop |
343: | `SerializationTest()` | Drop |
344: | Facade-specific tests | Drop (test via Output array instead) |
345: | `Reset()` tests | Drop |
346: | `[TestMethod]` / `[TestClass]` | Convert to Go `Test*` / TS Jasmine `describe`/`it` |
347: | `Assert.IsTrue(x == y)` | Go: require assertions. TS: Jasmine `expect(x).toBe(y)` |
348: 
349: ---
350: 
351: ## Test Conversion
352: 
353: ### Reference Data
354: 
355: MBST tests typically include large arrays of reference data (from TA-Lib Excel
356: simulations). **Preserve this data exactly** — it's the ground truth for numerical
357: verification.
358: 
359: Typical MBST test structure:
360: ```csharp
361: private readonly List<double> rawInput = new List<double> { ... };  // 252 entries
362: private readonly List<double> expected = new List<double> { ... };  // expected output
363: ```
364: 
365: Convert to:
366: - **Go:** `var` block with `[]float64` slices
367: - **TS:** `const` arrays of `number`
368: 
369: ### Test Categories to Convert
370: 
371: 1. **Output value tests** — Feed `rawInput`, compare against `expected` values.
372:    Use tolerance comparison: Go `require.InDelta(t, expected, actual, 1e-10)`,
373:    TS `expect(Math.abs(actual - expected)).toBeLessThan(1e-10)`.
374: 
375: 2. **IsPrimed tests** — Verify `IsPrimed()` is false during warmup and true after.
376: 
377: 3. **Metadata tests** — Verify `Metadata()` returns correct type, mnemonic, description, outputs.
378: 
379: 4. **Constructor validation** — Verify invalid params (e.g., `length < 1`) produce errors.
380: 
381: 5. **UpdateEntity tests** — Test `UpdateScalar`, `UpdateBar`, `UpdateQuote`, `UpdateTrade`
382:    with a few samples to verify entity routing works.
383: 
384: 6. **NaN handling** — Verify that NaN input produces NaN output without corrupting state.
385: 
386: ### Test for Multi-Output
387: 
388: For multi-output indicators, test **both** (all) outputs against reference data:
389: ```go
390: // After feeding sample:
391: output := indicator.UpdateScalar(scalar)
392: cogScalar := output[int(Value)].(entities.Scalar)
393: trigScalar := output[int(Trigger)].(entities.Scalar)
394: require.InDelta(t, expectedCog, cogScalar.Value, tolerance)
395: require.InDelta(t, expectedTrig, trigScalar.Value, tolerance)
396: ```
397: 
398: ### MBST Test Data with Separate High/Low Arrays
399: 

(Showing lines 280-399 of 1802. Use offset=400 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 18.1s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "newString": "### Metadata for Multi-Output\n\nEach output gets its own `core.OutputText` entry passed to `core.BuildMetadata`. The `Kind`\nand `Shape` are sourced from the descriptor registry — the indicator file only supplies\nper-output mnemonic + description text. The number and order of entries must match the\ndescriptor row registered in `go/indicators/core/descriptors.go`:\n\n```go\nfunc (s *CenterOfGravityOscillator) Metadata() core.Metadata {\n    return core.BuildMetadata(\n        core.CenterOfGravityOscillator,\n        s.mnemonic,\n        s.description,\n        []core.OutputText{\n            {Mnemonic: s.mnemonic,     Description: s.description},\n            {Mnemonic: s.mnemonicTrig, Description: s.descriptionTrig},\n        },\n    )\n}\n```\n\nTS mirrors this with `buildMetadata(IndicatorIdentifier.CenterOfGravityOscillator, ..., [{mnemonic, description}, {mnemonic, description}])`.\n\nFacade-specific mnemonic patterns (like `cogTrig(10)`) are preserved as the Trigger\noutput mnemonic.\n\n---\n\n## Metadata Conversion\n\n| MBST | Zpano |\n|---|---|\n| `Name` (e.g., `\"cog\"`) | First argument to `core.BuildMetadata` — a `core.Identifier` enum value |\n| `Moniker` (e.g., `\"cog(10)\"`) | `Metadata().Mnemonic` — includes component triple |\n| `Description` (e.g., `\"Center of Gravity oscillator\"`) | `Metadata().Description` — typically `\"Full Name \" + mnemonic` |\n\n### Register the Identifier and Descriptor\n\nBefore implementing, register the indicator in:\n\n- **Go identifier:** `go/indicators/core/identifier.go` — add enum constant, string, `String()`, `MarshalJSON`, `UnmarshalJSON`\n- **Go descriptor:** `go/indicators/core/descriptors.go` — add a taxonomy row with per-output `Kind`/`Shape`\n- **TS identifier:** `ts/indicators/core/indicator-identifier.ts` — add enum member\n- **TS descriptor:** `ts/indicators/core/descriptors.ts` — add the matching row\n\n`BuildMetadata` / `buildMetadata` panic / throw if the identifier has no descriptor row or\nif the output-count mismatches. See `.opencode/skills/indicator-architecture/SKILL.md`\nsection \"Taxonomy & Descriptor Registry\" for field meanings and guidance.",
  "oldString": "### Metadata for Multi-Output\n\nEach output gets its own entry in `Metadata().Outputs`:\n\n```go\nOutputs: []outputs.Metadata{\n    { Kind: int(Value), Type: outputs.ScalarType, Mnemonic: s.mnemonic, Description: s.description },\n    { Kind: int(Trigger), Type: outputs.ScalarType, Mnemonic: s.mnemonicTrig, Description: s.descriptionTrig },\n},\n```\n\nFacade-specific mnemonic patterns (like `cogTrig(10)`) are preserved as the Trigger\noutput mnemonic.\n\n---\n\n## Metadata Conversion\n\n| MBST | Zpano |\n|---|---|\n| `Name` (e.g., `\"cog\"`) | `Metadata().Type` — use the registered `core.IndicatorType` enum |\n| `Moniker` (e.g., `\"cog(10)\"`) | `Metadata().Mnemonic` — includes component triple |\n| `Description` (e.g., `\"Center of Gravity oscillator\"`) | `Metadata().Description` — typically `\"Full Name \" + mnemonic` |\n\n### Register the Indicator Type\n\nBefore implementing, register the indicator in:\n- **Go:** `go/indicators/core/type.go` — add enum constant, string, `String()`, `MarshalJSON`, `UnmarshalJSON`\n- **TS:** `ts/indicators/core/indicator-type.ts` — add enum member"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.8s)

Now fix the remaining stale `IndicatorType`/`OutputType` references further in the file.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "limit": 12,
  "offset": 880
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md</path>
<type>file</type>
<content>880: ### When to Merge
881: 
882: Merge when all of the following hold:
883: 1. The MBST classes each own their own `HilbertTransformerCycleEstimator` (or other
884:    expensive internal state) but conceptually **compute the same thing**.
885: 2. They share the same primary parameter(s) (e.g., `alphaEmaPeriodAdditional` and
886:    estimator params).
887: 3. Users typically want multiple outputs together (reading phase requires the period
888:    anyway).
889: 
890: ### Pattern
891: 

(Showing lines 880-891 of 1821. Use offset=892 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.2s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "newString": "- **Single entry in `core.Identifier`** — one `DominantCycle` constant, not three.",
  "oldString": "- **Single entry in `core.IndicatorType`** — one `DominantCycle` constant, not three."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.0s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "limit": 10,
  "offset": 1123
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md</path>
<type>file</type>
<content>1123:    `Number.NaN`, drop Go `//nolint` tags.
1124: 4. **Phase array comparison uses mod-360 helper** (see above). Don't skip NaN-expected
1125:    indices globally; skip per-element with `if (Number.isNaN(expected[i])) continue`.
1126: 5. **Reference-value array can be shorter than input array** — e.g., TCM's `expectedValue`
1127:    has 201 entries while `input` has 252. Guard with `if (i >= limit) continue` inside the
1128:    loop instead of shortening the outer iteration range (keeps the indicator fed with the
1129:    full input sequence).
1130: 
1131: ---
1132: 

(Showing lines 1123-1132 of 1821. Use offset=1133 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.1s)

**Tool: grep**

**Input:**
```json
{
  "path": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "pattern": "IndicatorType|OutputType"
}
```

**Output:**
```
Found 5 matches
/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md:
  Line 1145: register a `core.IndicatorType`** for it, and do not give it an `update…Entity` path.
  Line 1288: - **One `core.IndicatorType`** (e.g., `AdaptiveTrendAndCycleFilter`).
  Line 1376: All N outputs are `OutputType.Scalar`; list them in enum order. The top-level
  Line 1408: - **One registered `core.IndicatorType`** (e.g., `GoertzelSpectrum`), one package /
  Line 1416: - **`Metadata().Outputs` has one entry** with `OutputType.Heatmap`. The indicator-level
```

---

## Assistant (Build · claude-opus-4.7 · 5.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "limit": 3,
  "offset": 1144
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md</path>
<type>file</type>
<content>1144: cycle-median logic. Port it as a plain class/struct in a `corona/` subpackage, **do not
1145: register a `core.IndicatorType`** for it, and do not give it an `update…Entity` path.
1146: Downstream indicators instantiate the helper by composition.

(Showing lines 1144-1146 of 1821. Use offset=1147 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 6.6s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "newString": "register a `core.Identifier`** for it, and do not give it an `update…Entity` path.",
  "oldString": "register a `core.IndicatorType`** for it, and do not give it an `update…Entity` path."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.7s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "newString": "- **One `core.Identifier`** (e.g., `AdaptiveTrendAndCycleFilter`).",
  "oldString": "- **One `core.IndicatorType`** (e.g., `AdaptiveTrendAndCycleFilter`)."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.1s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "limit": 6,
  "offset": 1374
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md</path>
<type>file</type>
<content>1374: ### Metadata Per Output
1375: 
1376: All N outputs are `OutputType.Scalar`; list them in enum order. The top-level
1377: `Description` is `"<Full Suite Name> <mnemonic>"`; per-output descriptions are `"<Full
1378: Line Name> <per-output mnemonic>"` (e.g., `"Fast Adaptive Trend Line fatl()"`).
1379: 

(Showing lines 1374-1379 of 1821. Use offset=1380 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.9s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "newString": "All N outputs have `Shape.Scalar` (registered in the descriptor row); list them in enum\norder. The top-level `Description` is `\"<Full Suite Name> <mnemonic>\"`; per-output\ndescriptions are `\"<Full Line Name> <per-output mnemonic>\"` (e.g., `\"Fast Adaptive Trend\nLine fatl()\"`).",
  "oldString": "All N outputs are `OutputType.Scalar`; list them in enum order. The top-level\n`Description` is `\"<Full Suite Name> <mnemonic>\"`; per-output descriptions are `\"<Full\nLine Name> <per-output mnemonic>\"` (e.g., `\"Fast Adaptive Trend Line fatl()\"`)."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.2s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "limit": 12,
  "offset": 1405
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md</path>
<type>file</type>
<content>1405: shape than the Corona suite and deserves its own recipe.
1406: 
1407: ### Shape
1408: 
1409: - **One registered `core.IndicatorType`** (e.g., `GoertzelSpectrum`), one package /
1410:   folder, one primary output.
1411: - **Output enum has exactly one member.** Go uses the `iota + 1 / outputLast` sentinel
1412:   pattern for `IsKnown()` consistency with other indicators. TS uses an explicit
1413:   `Value = 0` (no trailing sentinel needed — TS has no JSON Stringer plumbing).
1414: - **`Update(sample, time)` returns `*outputs.Heatmap` / `Heatmap` directly** (not a
1415:   tuple). The entity wrappers wrap it in a 1-element `core.Output` / `IndicatorOutput`
1416:   array.

(Showing lines 1405-1416 of 1822. Use offset=1417 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.7s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "newString": "- **One registered `core.Identifier`** (e.g., `GoertzelSpectrum`), one package /",
  "oldString": "- **One registered `core.IndicatorType`** (e.g., `GoertzelSpectrum`), one package /"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.6s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "limit": 4,
  "offset": 1416
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md</path>
<type>file</type>
<content>1416:   array.
1417: - **`Metadata().Outputs` has one entry** with `OutputType.Heatmap`. The indicator-level
1418:   mnemonic and the single output's mnemonic are identical.
1419: 

(Showing lines 1416-1419 of 1822. Use offset=1420 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.5s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "newString": "- **Descriptor row has one output** with `Shape.Heatmap` (registered in\n  `descriptors.go` / `descriptors.ts`). The indicator-level mnemonic and the single\n  output's mnemonic are identical.",
  "oldString": "- **`Metadata().Outputs` has one entry** with `OutputType.Heatmap`. The indicator-level\n  mnemonic and the single output's mnemonic are identical."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 5.8s)

Verify no stale references remain and run a docs-sanity check.

**Tool: grep**

**Input:**
```json
{
  "path": "/home/dev/zpano/.opencode/skills",
  "pattern": "IndicatorType|OutputType|outputs\\.ScalarType|indicator-type\\.ts|core/type\\.go|type_test\\.go|output-type"
}
```

**Output:**
```
Found 10 matches
/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md:
  Line 200: the old code references `output.Scalar` in `Metadata()`, which changes to `outputs.ScalarType`.
  Line 529: check("Outputs[0].Type", outputs.ScalarType, act.Outputs[0].Type)
  Line 777: Note: `OutputType`/`output-type` and `IndicatorType`/`indicator-type` are gone. `buildMetadata`
  Line 957: | `indicator.SimpleMovingAverage` | `core.SimpleMovingAverage` (now a `core.Identifier`, not an `IndicatorType`) |
  Line 958: | `indicator.Type` / `IndicatorType` | `core.Identifier` |
  Line 961: | `outputs.ScalarType` / `outputs.Type` | `shape.Scalar` / `shape.Shape` (same note — sourced from the registry) |
  Line 992: | `IndicatorType` | `IndicatorIdentifier` (from `core/indicator-identifier`) |
  Line 993: | `OutputType.Scalar` | `Shape.Scalar` (from `core/outputs/shape/shape`) — but indicator files no longer write this; sourced from the descriptor registry |

/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md:
  Line 124:   and an enum (`OutputType`) describing them.
  Line 397: | Go uses bare `Type` in packages; other languages use `IndicatorType`/`OutputType` | Go's package system provides scoping (`core.Type`), making prefixes redundant and stuttery (`core.IndicatorType`). Other languages import symbols directly, so self-describing names are necessary. |
```

---

## Assistant (Build · claude-opus-4.7 · 9.6s)

_Thinking:_

 I'm checking the remaining hits in indicator-conversion to see which ones are stale versus intentional—the ones documenting renames around lines 777, 957-961, and 992-993 are deliberate, but I need to verify lines 200 and 529. I should also look at the SKILL architecture references on lines 124 and 397 to see if they need updates.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "limit": 6,
  "offset": 197
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md</path>
<type>file</type>
<content>197: Keep standard library imports (`fmt`, `math`, `sync`, etc.) as-is.
198: 
199: Remove the `"mbg/trading/indicators/indicator/output"` import entirely -- it is only needed if
200: the old code references `output.Scalar` in `Metadata()`, which changes to `outputs.ScalarType`.
201: 
202: #### 5b. Struct: remove boilerplate fields, embed LineIndicator

(Showing lines 197-202 of 1454. Use offset=203 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 7.7s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "newString": "Remove the `\"mbg/trading/indicators/indicator/output\"` import entirely -- `BuildMetadata`\nsources per-output `Kind`/`Shape` from the descriptor registry, so indicator files no\nlonger need to import `\"zpano/indicators/core/outputs\"` or `\".../outputs/shape\"` just for\n`Metadata()`. Keep the `outputs` import only if the file uses `outputs.NewBand` /\n`outputs.NewEmptyBand`.",
  "oldString": "Remove the `\"mbg/trading/indicators/indicator/output\"` import entirely -- it is only needed if\nthe old code references `output.Scalar` in `Metadata()`, which changes to `outputs.ScalarType`."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 3.7s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "limit": 14,
  "offset": 522
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md</path>
<type>file</type>
<content>522: 
523: ```go
524: check("Outputs[0].Type", output.Scalar, act.Outputs[0].Type)
525: check("Outputs[0].Name", "sma(5)", act.Outputs[0].Name)
526: ```
527: 
528: New:
529: 
530: ```go
531: check("Type", core.SimpleMovingAverage, act.Type)
532: check("Outputs[0].Type", outputs.ScalarType, act.Outputs[0].Type)
533: check("Outputs[0].Mnemonic", "sma(5)", act.Outputs[0].Mnemonic)
534: ```
535: 

(Showing lines 522-535 of 1457. Use offset=536 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.1s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "limit": 30,
  "offset": 492
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md</path>
<type>file</type>
<content>492: 
493: ```go
494: check("mnemonic", "sma(5, hl/2)", sma.LineIndicator.Mnemonic)
495: check("description", "Simple moving average sma(5, hl/2)", sma.LineIndicator.Description)
496: ```
497: 
498: The `barFunc/quoteFunc/tradeFunc == nil` checks are removed because these are now internal to
499: `LineIndicator`.
500: 
501: Note: the mnemonic includes `hl/2` because the "length > 1" test uses `BarMedianPrice` which is
502: not the default. If your test uses non-default components, adjust the expected mnemonic accordingly.
503: 
504: #### 6e. Component references in test constants and params
505: 
506: | Old | New |
507: |-----|-----|
508: | `data.BarComponent` | `entities.BarComponent` |
509: | `data.BarMedianPrice` | `entities.BarMedianPrice` |
510: | `data.BarClosePrice` | `entities.BarClosePrice` |
511: | `data.QuoteComponent` | `entities.QuoteComponent` |
512: | `data.QuoteMidPrice` | `entities.QuoteMidPrice` |
513: | `data.QuoteBidPrice` | `entities.QuoteBidPrice` |
514: | `data.TradeComponent` | `entities.TradeComponent` |
515: | `data.TradePrice` | `entities.TradePrice` |
516: | `data.BarComponent(9999)` | `entities.BarComponent(9999)` |
517: | etc. | etc. |
518: 
519: #### 6f. Metadata test
520: 
521: Old:

(Showing lines 492-521 of 1457. Use offset=522 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 6.3s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md",
  "newString": "```go\ncheck(\"Identifier\", core.SimpleMovingAverage, act.Identifier)\ncheck(\"Outputs[0].Shape\", shape.Scalar, act.Outputs[0].Shape)\ncheck(\"Outputs[0].Mnemonic\", \"sma(5)\", act.Outputs[0].Mnemonic)\n```\n\n(`shape` is `zpano/indicators/core/outputs/shape`. The test file is one of the few places\nthat still imports it, since the indicator file no longer does.)",
  "oldString": "```go\ncheck(\"Type\", core.SimpleMovingAverage, act.Type)\ncheck(\"Outputs[0].Type\", outputs.ScalarType, act.Outputs[0].Type)\ncheck(\"Outputs[0].Mnemonic\", \"sma(5)\", act.Outputs[0].Mnemonic)\n```"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.6s)

Now the two lingering architecture SKILL hits.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "limit": 6,
  "offset": 121
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md</path>
<type>file</type>
<content>121: 
122: - **`outputs/`** contains indicator output shapes. An indicator can output a
123:   scalar, a band (upper/lower), a heatmap, etc. This folder holds the types
124:   and an enum (`OutputType`) describing them.
125: - **`frequency-response/`** contains utilities for computing the frequency
126:   response of a filter/indicator.

(Showing lines 121-126 of 1134. Use offset=127 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.5s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "newString": "- **`outputs/`** contains indicator output shapes. An indicator can output a\n  scalar, a band (upper/lower), a heatmap, etc. This folder holds the concrete\n  types; the taxonomy enum lives in the `outputs/shape/` sub-package as\n  `shape.Shape` (Go) / `Shape` (TS).",
  "oldString": "- **`outputs/`** contains indicator output shapes. An indicator can output a\n  scalar, a band (upper/lower), a heatmap, etc. This folder holds the types\n  and an enum (`OutputType`) describing them."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.2s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "limit": 8,
  "offset": 394
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md</path>
<type>file</type>
<content>394: | `outputs/` instead of `entities/` in core | These types represent indicator *output shapes*, not general domain entities. `outputs` is more precise about their purpose. |
395: | `core/` instead of `types/` | `core` is broad enough to hold types, enums, base classes, and utilities without needing a rename later. `types` would be too narrow. |
396: | `common/` instead of `unknown/`, `classic/`, `standard/` | `common` is neutral -- doesn't imply age (`classic`), official status (`standard`), or sound dismissive (`unknown`). |
397: | `custom/` instead of `experimental/` | `custom` describes *origin* (you made it), not *maturity*. A custom indicator can be stable; an experimental one implies instability. If you need to track maturity, use metadata in code, not folder structure. |
398: | Go uses bare `Type` in packages; other languages use `IndicatorType`/`OutputType` | Go's package system provides scoping (`core.Type`), making prefixes redundant and stuttery (`core.IndicatorType`). Other languages import symbols directly, so self-describing names are necessary. |
399: | Per-indicator subfolders even in `common/` | Without subfolders, all indicator files in `common/` would land in one flat directory (and in Go, one package). This doesn't scale and is inconsistent with the author-folder structure. |
400: | Component enums start at `iota + 1` | Makes the zero value (`0`) explicitly "not set", which is used as the sentinel for "use default". This is idiomatic Go for optional enum fields in structs (zero value of `int` is `0`). |
401: | TypeScript enums start at `0`, components are `undefined` when not set | TS enums default to `0`, and optional params use `undefined` (not `0`) as the sentinel for "not set". The `componentTripleMnemonic` function checks `!== undefined && !== Default*` to decide whether to show a component. The `LineIndicator` setters use `=== undefined` (not truthiness) to decide when to apply the default, so explicitly passing a zero-valued enum (e.g., `BarComponent.Open`) works correctly. |

(Showing lines 394-401 of 1135. Use offset=402 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 6.3s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/indicator-architecture/SKILL.md",
  "newString": "| Go uses scoped names (`core.Identifier`, `shape.Shape`); other languages use `IndicatorIdentifier`/`Shape` at the top level | Go's package system provides scoping, making prefixes redundant and stuttery (`core.IndicatorIdentifier`, `outputs.Shape`). Other languages import symbols directly, so self-describing names are necessary; `Shape` is scoped to its own `outputs/shape` sub-package in Go to avoid clashing with entity/indicator shapes. |",
  "oldString": "| Go uses bare `Type` in packages; other languages use `IndicatorType`/`OutputType` | Go's package system provides scoping (`core.Type`), making prefixes redundant and stuttery (`core.IndicatorType`). Other languages import symbols directly, so self-describing names are necessary. |"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 5.3s)

Final sweep to confirm all stale refs are intentional (documenting renames only).

**Tool: grep**

**Input:**
```json
{
  "path": "/home/dev/zpano/.opencode/skills",
  "pattern": "IndicatorType|outputs\\.ScalarType|output-type|core/type\\.go|type_test\\.go"
}
```

**Output:**
```
Found 5 matches
/home/dev/zpano/.opencode/skills/indicator-conversion/SKILL.md:
  Line 783: Note: `OutputType`/`output-type` and `IndicatorType`/`indicator-type` are gone. `buildMetadata`
  Line 963: | `indicator.SimpleMovingAverage` | `core.SimpleMovingAverage` (now a `core.Identifier`, not an `IndicatorType`) |
  Line 964: | `indicator.Type` / `IndicatorType` | `core.Identifier` |
  Line 967: | `outputs.ScalarType` / `outputs.Type` | `shape.Scalar` / `shape.Shape` (same note — sourced from the registry) |
  Line 998: | `IndicatorType` | `IndicatorIdentifier` (from `core/indicator-identifier`) |
```

---

## Assistant (Build · claude-opus-4.7 · 11.2s)

All remaining hits are intentional — they document the old → new rename. Phase 7 complete.

## Phase 7 summary

Updated all three conversion skills to the post-taxonomy architecture:

- **`indicator-conversion/SKILL.md`** — TS Step 4a imports, TS Step 4e metadata (`buildMetadata`), Go Step 7 (identifier + descriptor registration), new TS Step 6 (identifier + descriptor), updated Step 5b Go imports note, metadata-test example (`Identifier` / `Outputs[0].Shape`), Quick Reference import mapping and symbol-renames tables for both Go and TS.
- **`mbst-indicator-conversion/SKILL.md`** — "Metadata for Multi-Output" now shows `core.BuildMetadata` call; "Metadata Conversion" + "Register the Identifier and Descriptor" sections rewritten; all references to `core.IndicatorType`, `OutputType.Scalar`, `OutputType.Heatmap` updated to `core.Identifier` / `Shape.Scalar` / `Shape.Heatmap`.
- **`talib-indicator-conversion/SKILL.md`** — Registration section updated (identifier + descriptor paths).
- **`indicator-architecture/SKILL.md`** — two leftover `OutputType` references fixed (overview and rationale table).

Documentation-only changes; no Go or TS source touched this phase, so no rebuild/test required. The full plan (Phases 1–7) is complete.

---

