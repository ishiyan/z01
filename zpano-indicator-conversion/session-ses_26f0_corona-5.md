# Split INDICATORS-ARCHITECTURE and CONVERTING-OLD-INDICATOR into skills?

**Session ID:** ses_26f009c55ffeeFyvE85VbCND9m
**Created:** 4/15/2026, 1:55:49 PM
**Updated:** 4/21/2026, 8:53:35 PM

---

## Assistant (Build · claude-opus-4.7 · 5.7s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts",
  "offset": 300,
  "limit": 180
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts</path>
<type>file</type>
<content>300:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 1.0001 })).toThrowError(/α for additional smoothing/);
301:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.5 })).not.toThrow();
302:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 1.0 })).not.toThrow();
303:   });
304: 
305:   it('should throw if trend line smoothing length is not 2, 3, or 4', () => {
306:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 1 }))
307:       .toThrowError(/trend line smoothing length/);
308:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 5 }))
309:       .toThrowError(/trend line smoothing length/);
310:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 2 })).not.toThrow();
311:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 3 })).not.toThrow();
312:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 4 })).not.toThrow();
313:   });
314: 
315:   it('should throw if cycle part multiplier is out of range (0, 10]', () => {
316:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, cyclePartMultiplier: 0 }))
317:       .toThrowError(/cycle part multiplier/);
318:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, cyclePartMultiplier: 10.00001 }))
319:       .toThrowError(/cycle part multiplier/);
320:   });
321: 
322:   it('should throw if separation percentage is out of range (0, 100]', () => {
323:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, separationPercentage: 0 }))
324:       .toThrowError(/separation percentage/);
325:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, separationPercentage: 100.00001 }))
326:       .toThrowError(/separation percentage/);
327:   });
328: 
329:   it('should return NaN 8-tuple for NaN input', () => {
330:     const x = TrendCycleMode.default();
331:     const out = x.update(Number.NaN);
332:     expect(out.length).toBe(8);
333:     for (const v of out) {
334:       expect(Number.isNaN(v)).toBe(true);
335:     }
336:   });
337: 
338:   it('should match reference period past settle window', () => {
339:     const x = TrendCycleMode.default();
340:     for (let i = skip; i < input.length; i++) {
341:       const [, , , , , , period] = x.update(input[i]);
342:       if (Number.isNaN(period) || i < settleSkip) continue;
343:       expect(Math.abs(expectedPeriod[i] - period))
344:         .withContext(`period[${i}]: expected ${expectedPeriod[i]}, actual ${period}`)
345:         .toBeLessThan(tolerance);
346:     }
347:   });
348: 
349:   it('should match reference phase (mod 360) past settle window', () => {
350:     const x = TrendCycleMode.default();
351:     for (let i = skip; i < input.length; i++) {
352:       const [, , , , , , , phase] = x.update(input[i]);
353:       if (Number.isNaN(phase) || Number.isNaN(expectedPhase[i]) || i < settleSkip) continue;
354:       const d = phaseDelta(expectedPhase[i], phase);
355:       expect(Math.abs(d))
356:         .withContext(`phase[${i}]: expected ${expectedPhase[i]}, actual ${phase}`)
357:         .toBeLessThan(tolerance);
358:     }
359:   });
360: 
361:   it('should match reference sine wave past settle window', () => {
362:     const x = TrendCycleMode.default();
363:     for (let i = skip; i < input.length; i++) {
364:       const [, , , , sine] = x.update(input[i]);
365:       if (Number.isNaN(sine) || Number.isNaN(expectedSine[i]) || i < settleSkip) continue;
366:       expect(Math.abs(expectedSine[i] - sine))
367:         .withContext(`sine[${i}]: expected ${expectedSine[i]}, actual ${sine}`)
368:         .toBeLessThan(tolerance);
369:     }
370:   });
371: 
372:   it('should match reference sine wave lead past settle window', () => {
373:     const x = TrendCycleMode.default();
374:     for (let i = skip; i < input.length; i++) {
375:       const [, , , , , sineLead] = x.update(input[i]);
376:       if (Number.isNaN(sineLead) || Number.isNaN(expectedSineLead[i]) || i < settleSkip) continue;
377:       expect(Math.abs(expectedSineLead[i] - sineLead))
378:         .withContext(`sineLead[${i}]: expected ${expectedSineLead[i]}, actual ${sineLead}`)
379:         .toBeLessThan(tolerance);
380:     }
381:   });
382: 
383:   it('should match reference instantaneous trend line past settle window', () => {
384:     const x = TrendCycleMode.default();
385:     for (let i = skip; i < input.length; i++) {
386:       const [, , , itl] = x.update(input[i]);
387:       if (Number.isNaN(itl) || Number.isNaN(expectedITL[i]) || i < settleSkip) continue;
388:       expect(Math.abs(expectedITL[i] - itl))
389:         .withContext(`itl[${i}]: expected ${expectedITL[i]}, actual ${itl}`)
390:         .toBeLessThan(tolerance);
391:     }
392:   });
393: 
394:   it('should match reference tcm value (except MBST known mismatches)', () => {
395:     const x = TrendCycleMode.default();
396:     const limit = expectedValue.length;
397:     for (let i = skip; i < input.length; i++) {
398:       const [value] = x.update(input[i]);
399:       if (i >= limit) continue;
400:       // MBST known mismatches.
401:       if (i === 70 || i === 71) continue;
402:       if (Number.isNaN(value) || Number.isNaN(expectedValue[i])) continue;
403:       expect(Math.abs(expectedValue[i] - value))
404:         .withContext(`value[${i}]: expected ${expectedValue[i]}, actual ${value}`)
405:         .toBeLessThan(tolerance);
406:     }
407:   });
408: 
409:   it('should have complementary is-trend / is-cycle flags (0/1) aligned with value sign', () => {
410:     const x = TrendCycleMode.default();
411:     for (let i = skip; i < input.length; i++) {
412:       const [value, trend, cycle] = x.update(input[i]);
413:       if (Number.isNaN(value)) continue;
414:       expect(trend + cycle).toBe(1);
415:       if (value > 0) expect(trend).toBe(1);
416:       if (value < 0) expect(trend).toBe(0);
417:     }
418:   });
419: 
420:   it('should become primed within the input sequence', () => {
421:     const x = TrendCycleMode.default();
422:     expect(x.isPrimed()).toBe(false);
423: 
424:     let primedAt = -1;
425:     for (let i = 0; i < input.length; i++) {
426:       x.update(input[i]);
427:       if (x.isPrimed() && primedAt < 0) primedAt = i;
428:     }
429:     expect(primedAt).toBeGreaterThanOrEqual(0);
430:     expect(x.isPrimed()).toBe(true);
431:   });
432: 
433:   const primeCount = 200;
434:   const value = 100.0;
435: 
436:   function checkOutput(out: any[]): void {
437:     expect(out.length).toBe(8);
438:     for (let i = 0; i < 8; i++) {
439:       const s = out[i] as Scalar;
440:       expect(s.time).toEqual(time);
441:     }
442:   }
443: 
444:   it('should produce 8-element scalar output via updateScalar', () => {
445:     const x = TrendCycleMode.default();
446:     for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);
447:     const s = new Scalar();
448:     s.time = time;
449:     s.value = value;
450:     checkOutput(x.updateScalar(s) as any[]);
451:   });
452: 
453:   it('should produce 8-element scalar output via updateBar', () => {
454:     const x = TrendCycleMode.default();
455:     for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);
456:     const bar = new Bar({ time, open: value, high: value, low: value, close: value, volume: 0 });
457:     checkOutput(x.updateBar(bar) as any[]);
458:   });
459: 
460:   it('should produce 8-element scalar output via updateQuote', () => {
461:     const x = TrendCycleMode.default();
462:     for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);
463:     const q = new Quote({ time, bid: value, ask: value, bidSize: 0, askSize: 0 });
464:     checkOutput(x.updateQuote(q) as any[]);
465:   });
466: 
467:   it('should produce 8-element scalar output via updateTrade', () => {
468:     const x = TrendCycleMode.default();
469:     for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);
470:     const t = new Trade({ time, price: value, volume: 0 });
471:     checkOutput(x.updateTrade(t) as any[]);
472:   });
473: });

(End of file - total 473 lines)
</content>
```

---

## User

---

## Assistant (Compaction · claude-opus-4.7 · 100.8s)

---
## Goal

Port MBST's **Ehlers Corona Charts suite** (TASC November 2008) from `/home/dev/zpano/mbst-to-convert/john-ehlers/corona/` into zpano:

- **Corona** shared helper (not registered as indicator)
- **CoronaSpectrum** — heatmap + DC + DCM scalars
- **CoronaSignalToNoiseRatio** — heatmap + SNR scalar
- **CoronaSwingPosition** — heatmap + SwingPosition scalar
- **CoronaTrendVigor** — heatmap + TrendVigor scalar

First heatmap-producing indicators in zpano. **Go phase complete** (all 4 + helper, all tests green). **Currently in Phase 6: TypeScript port.**

## Instructions

### Design decisions (confirmed)
- Unprimed bars → emit empty heatmap (equivalent of Go `outputs.NewEmptyHeatmap`); scalars NaN.
- Mnemonics: `cspect(…)`, `cspect-dc(…)`, `cspect-dcm(…)`, `csnr(…)`, `csnr-snr(…)`, `cswing(…)`, `cswing-sp(…)`, `ctv(…)`, `ctv-tv(…)`.
- User paused after Go phase for review; said "yes please" to continue → now in TS port.
- Snapshot tests reuse same 252-entry TA-Lib MAMA series with tolerance 1e-4.
- User requests: continue if next steps clear, stop/ask if unsure.

### Algorithm decisions (locked in Go, must mirror in TS)
- Swing BP2 momentum: 2-bar (`sample - samplePrev2`).
- TrendVigor lookback: `int(DCM - 1)`; guard `cyclePeriod >= 1` (MBST fix); clamp upper at `len(sampleBuffer)`.
- SNR signal EMA coefficients: 0.2/0.9 (sum 1.1, intentional per Ehlers).
- DC weighted average: N=12..60 indexing; DC=0.5·num/denom; clamp DC <minPeriod. DCM = median of 5-buffer; clamp <minPeriod.
- DominantCycleBuffer initialized to `math.MaxFloat64` sentinels.
- CSpectrum rounds `minParam` up (ceil), `maxParam` down (floor).
- CSNR/CSwing/CTV: first sample store-only ("isStarted" pattern) returns empty heatmap, NaN scalar.
- CSNR scalar path: H=L=sample ⇒ H-L=0 ⇒ SNR=MinParameterValue.

### Defaults / formulas
- Corona: `HighPassFilterCutoff=30, MinimalPeriod=6, MaximalPeriod=30, DecibelsLowerThreshold=6, DecibelsUpperThreshold=20`
- CSpectrum: `minRaster=6, maxRaster=20, minParam=6, maxParam=30, hpCutoff=30, BarComponent=Median`
- CSNR: `rasterLength=50, maxRaster=20, minParam=1, maxParam=11, BarComponent=Median`
- CSwing: `rasterLength=50, maxRaster=20, minParam=-5, maxParam=5` (only substitute defaults when both min and max are 0)
- CTV: `rasterLength=50, maxRaster=20, minParam=-10, maxParam=10` (ditto)
- `ParameterResolution = (rasterLength-1)/(maxParam-minParam)`:
  - CSpectrum: `48/24 = 2.0`
  - CSNR: `49/10 = 4.9`
  - CSwing: `49/10 = 4.9`
  - CTV: `49/20 = 2.45`

## Discoveries

### TS infrastructure verified (read during this session)
- `ts/indicators/core/indicator-type.ts` — ends at line 189 `TrendCycleMode,` (file ends line 190 with `}`). **TS enum indices do NOT align with Go indices**. Just append 4 entries before the closing `}`.
- `ts/indicators/core/outputs/heatmap.ts` — plain data class (23 lines), no constructor/factories. **Must add** static factory methods `NewHeatmap`/`NewEmptyHeatmap` mirroring Go `outputs/heatmap.go` semantics (empty: values=[], valueMin/Max=NaN).
- `ts/indicators/core/outputs/band.ts` — also plain data class (confirms convention).
- `ts/indicators/core/outputs/output-type.ts` — `OutputType.Heatmap` already exists.
- `ts/indicators/core/indicator-output.ts`: `IndicatorOutput = (Scalar | Band | Heatmap)[]`.
- `ts/indicators/core/component-triple-mnemonic.ts` — hides defaults; TS `DefaultBarComponent=Close`; passing `BarComponent.Median` explicitly adds `, hl/2`. Matches Go.
- `ts/entities/scalar.ts` — `Scalar` class with `time`, `value`; `new Scalar()` then assign.
- `ts/entities/bar-component.ts` — has `BarComponent.Median`, `barComponentValue`, `barComponentMnemonic` ('hl/2').

### TCM template analysis (read in full)
- `trend-cycle-mode.ts` (450 lines) — THE template. Uses `componentTripleMnemonic(bc, qc, tc)` with Median default, `static default()` + `static fromParams(params)`, private constructor. `update()` returns tuple, `updateEntity()` maps tuple→Scalar[].
- `trend-cycle-mode.spec.ts` (473 lines) — uses Jasmine (`import { } from 'jasmine'`), inline 252-entry input, `tolerance = 1e-4`, `TrendCycleMode.default()`, metadata tests, NaN tests, updateScalar/Bar/Quote/Trade tests with constructor signatures: `new Scalar()` then assign; `new Bar({time, open, high, low, close, volume})`; `new Quote({time, bid, ask, bidSize, askSize})`; `new Trade({time, price, volume})`.
- `trend-cycle-mode-output.ts` uses `= 0, = 1, ...` with JSDoc per value.

### Go sources verified (all packages fully read)
- `go/indicators/johnehlers/corona/{corona.go,params.go}` — helper.
- `go/indicators/johnehlers/coronaspectrum/` — all 5 files read incl. `coronaspectrum_test.go`.
- `go/indicators/johnehlers/coronasignaltonoiseratio/` — all 5 files incl. test.
- `go/indicators/johnehlers/coronaswingposition/coronaswingposition.go` — read.
- `go/indicators/johnehlers/coronatrendvigor/coronatrendvigor.go` — read.

### Snapshot values (from Go tests)

**CSpectrum** (`dc`, `dcm` at bars 11,12,50,100,150,200,251):
```
{11, 17.7604672565, 17.7604672565}
{12, 6.0000000000, 6.0000000000}
{50, 15.9989078712, 15.9989078712}
{100, 14.7455497547, 14.7455497547}
{150, 17.5000000000, 17.2826036069}
{200, 19.7557338512, 20.0000000000}
{251, 6.0000000000, 6.0000000000}
```

**CSNR** (uses `makeHL(i, sample)`: `frac=0.005+0.03*(1+sin(i*0.37))`, `half=sample*frac`, return `sample±half`). Snapshots (`snr`, `vmin`, `vmax`):
```
{11, 1.0, 0.0, 20.0}
{12, 1.0, 0.0, 20.0}
{50, 1.0, 0.0, 20.0}
{100, 2.9986583538, 4.2011609652, 20.0}
{150, 1.0, 0.0000000035, 20.0}
{200, 1.0, 0.0, 20.0}
{251, 1.0, 0.0000000026, 20.0}
```

**CSwing** (`sp`, `vmin`, `vmax`):
```
{11, 5.0, 20.0, 20.0}
{12, 5.0, 20.0, 20.0}
{50, 4.5384908349, 20.0, 20.0}
{100, -3.8183742675, 3.4957777081, 20.0}
{150, -1.8516194371, 5.3792287864, 20.0}
{200, -3.6944428668, 4.2580825738, 20.0}
{251, -0.8524812061, 4.4822539784, 20.0}
```

**CTV** (`tv`, `vmin`, `vmax`):
```
{11, 5.6512200755, 20.0, 20.0}
{12, 6.8379492897, 20.0, 20.0}
{50, 2.6145116709, 2.3773561485, 20.0}
{100, 2.7536803664, 2.4892742850, 20.0}
{150, -6.4606404251, 20.0, 20.0}
{200, -10.0, 20.0, 20.0}
{251, -0.1894989954, 0.5847573715, 20.0}
```

### Go heatmap factory semantics (to mirror in TS)
```go
NewEmptyHeatmap(time, first, last, res) → {time, first, last, res, NaN, NaN, []float64{}}
NewHeatmap(time, first, last, res, min, max, values) → populated
IsEmpty() → len(Values) < 1
```

## Accomplished

### ✅ Go Phase Complete (all tests green)
- Corona helper (`corona/`) with Params, NewCorona, Update.
- 4 `core.Type` constants (47-50, `type.go` lines 201-215) with JSON round-trip.
- CoronaSpectrum: `cspect(6, 20, 6, 30, 30, hl/2)`, `cspect-dc(30, hl/2)`, `cspect-dcm(30, hl/2)`.
- CoronaSignalToNoiseRatio: `csnr(50, 20, 1, 11, 30, hl/2)`, `csnr-snr(30, hl/2)`.
- CoronaSwingPosition: `cswing(50, 20, -5, 5, 30, hl/2)`, `cswing-sp(30, hl/2)`.
- CoronaTrendVigor: `ctv(50, 20, -10, 10, 30, hl/2)`, `ctv-tv(30, hl/2)`.

### 🔄 Phase 6 TypeScript port — investigation complete, no files created yet
- Loaded `mbst-indicator-conversion` skill.
- Read all Go sources (helper + 4 indicators + 2 test files as template).
- Read TCM template (impl + params + output + spec).
- Read TS heatmap.ts / band.ts / component-triple-mnemonic.ts / indicator-type.ts (tail).
- Confirmed Jasmine test style, class-with-assign Scalar pattern, constructor-object Bar/Quote/Trade pattern.
- **Ready to start writing TS files**.

### ⏳ Remaining TS work (execution order)
1. **Append 4 entries** to `ts/indicators/core/indicator-type.ts` before closing `}` (line 189): `CoronaSpectrum`, `CoronaSignalToNoiseRatio`, `CoronaSwingPosition`, `CoronaTrendVigor` with JSDoc.
2. **Add static factory methods** to `ts/indicators/core/outputs/heatmap.ts`:
   - `static newHeatmap(time, first, last, res, min, max, values): Heatmap`
   - `static newEmptyHeatmap(time, first, last, res): Heatmap` (min/max=NaN, values=[])
   - `isEmpty(): boolean` (values.length < 1)
3. **Create TS Corona helper**: `ts/indicators/john-ehlers/corona/{corona.ts,corona-params.ts,corona.spec.ts}`. Port `go/indicators/johnehlers/corona/corona.go` exactly: HP filter, FIR smoothing, momentum=smoothHP-smoothHPprev, adaptive delta, filter-bank update, dB normalization (0.33/0.67 EMA), weighted-avg DC (×0.5), 5-sample median DCM. Honor: reset `maximalAmplitudeSquared=0` per-bar; first-sample store-only `samplePrev=sample; return false`; reset `dominantCycle=0` before weighted-avg loop; init `dominantCycleBuffer` to `+Infinity`/`Number.MAX_VALUE`; clamp DC and DCM to `minimalPeriod`.
4. **Create 4 TS Corona indicator packages** (siblings under `ts/indicators/john-ehlers/`):
   - `corona-spectrum/` — `corona-spectrum.ts`, `corona-spectrum-params.ts`, `corona-spectrum-output.ts`, `corona-spectrum.spec.ts`
   - `corona-signal-to-noise-ratio/`
   - `corona-swing-position/`
   - `corona-trend-vigor/`
   - Use `componentTripleMnemonic` with `BarComponent.Median` default (yields `, hl/2`).
   - `update()` signatures:
     - CSpectrum: `(sample, time) → [Heatmap, number, number]`
     - CSNR: `(sample, low, high, time) → [Heatmap, number]`
     - CSwing/CTV: `(sample, time) → [Heatmap, number]`
   - `updateEntity` returns `IndicatorOutput` (always includes Heatmap object, uses empty heatmap for unprimed/NaN).
   - Mirror Go CSNR `updateScalar/Quote/Trade`: pass sample as both high and low.
5. **Port snapshot tests** using verbatim values above; tolerance 1e-4. Copy 252-entry input from TCM spec.
6. **Update `.opencode/skills/mbst-indicator-conversion/SKILL.md`** with Corona learnings: heatmap conventions, first-bar store-only pattern, DCM-1 lookback fix with `>=1` guard, parameter-resolution formula, empty-heatmap-axis-always-present invariant, CSNR coefficient sum=1.1 is intentional.

### TS-specific pitfalls
- Array left-shift loops: translate carefully; Go pattern `for i < N-1: buf[i]=buf[i+1]` → same loop in TS.
- `math.Sort` → `[...arr].sort((a,b)=>a-b)`.
- `math.MaxFloat64` → `Number.MAX_VALUE`.
- `math.Inf(1)/math.Inf(-1)` → `Number.POSITIVE_INFINITY/NEGATIVE_INFINITY`.
- `math.Round` (Go: half-away-from-zero) vs `Math.round` (JS: half-to-+∞). Potential discrepancy at negative x.5. Use `Math.sign(x)*Math.floor(Math.abs(x)+0.5)` if tests fail.
- Mutex (Go `sync.RWMutex`): drop in TS.

## Relevant files / directories

### Go packages (reference for TS port — all read in this session)
- `/home/dev/zpano/go/indicators/johnehlers/corona/{corona.go,params.go}` — helper
- `/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/` — all 5 files
- `/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/` — all 5 files
- `/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition.go`
- `/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor.go`
- `/home/dev/zpano/go/indicators/core/outputs/heatmap.go` — factory reference

### MBST source (not re-read this session but referenced)
- `/home/dev/zpano/mbst-to-convert/john-ehlers/corona/*.cs` (4 files)

### TS infrastructure (read this session)
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/` — all 4 files (template)
- `/home/dev/zpano/ts/indicators/core/indicator-type.ts` (line 189 end — needs 4 appends)
- `/home/dev/zpano/ts/indicators/core/outputs/heatmap.ts` (needs factories)
- `/home/dev/zpano/ts/indicators/core/outputs/band.ts`
- `/home/dev/zpano/ts/indicators/core/outputs/output-type.ts`
- `/home/dev/zpano/ts/indicators/core/component-triple-mnemonic.ts`
- `/home/dev/zpano/ts/indicators/core/` directory listing verified

### TS files to create (in execution order)
- `/home/dev/zpano/ts/indicators/john-ehlers/corona/` — `corona.ts`, `corona-params.ts`, `corona.spec.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/` — 4 files
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-signal-to-noise-ratio/` — 4 files
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-swing-position/` — 4 files
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-trend-vigor/` — 4 files

### Skills
- `.opencode/skills/mbst-indicator-conversion/SKILL.md` — loaded; to update after TS complete
- `.opencode/skills/indicator-architecture` — referenced in skill doc
- `.opencode/skills/indicator-conversion` — referenced in skill doc

### Immediate next action
Start writing TS files in the order above, beginning with:
1. Edit `ts/indicators/core/indicator-type.ts` to append 4 Corona enum entries.
2. Edit `ts/indicators/core/outputs/heatmap.ts` to add static factories + isEmpty.
3. Create `ts/indicators/john-ehlers/corona/` helper package.
4. Then the 4 indicator packages with their spec files.
5. Run `npm test` (or `npx jasmine`) after each indicator; fix `Math.round` discrepancies if tests fail.
6. Update `mbst-indicator-conversion` skill with Corona learnings.

---

## User

---

## Assistant (Build · claude-opus-4.7 · 4.5s)

_Thinking:_

 I'm starting the TypeScript port now and need to check the current state of the files I'll be working with.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/indicator-type.ts",
  "offset": 180
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/indicator-type.ts</path>
<type>file</type>
<content>180: 
181:   /** Identifies the Ehlers __Sine Wave__ (SW) indicator, exposing sine value, lead sine, band, dominant cycle period and phase. */
182:   SineWave,
183: 
184:   /** Identifies the Ehlers __Hilbert Transformer Instantaneous Trend Line__ (HTITL) indicator, exposing trend value and dominant cycle period. */
185:   HilbertTransformerInstantaneousTrendLine,
186: 
187:   /** Identifies the Ehlers __Trend / Cycle Mode__ (TCM) indicator, exposing the trend/cycle value (+1 in trend, −1 in cycle),
188:    * trend/cycle mode flags, instantaneous trend line, sine wave, lead sine wave, dominant cycle period and phase. */
189:   TrendCycleMode,
190: }

(End of file - total 190 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.6s)

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
23: }

(End of file - total 23 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.5s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/corona/corona.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/corona/corona.go</path>
<type>file</type>
<content>1: package corona
2: 
3: //nolint: gofumpt
4: import (
5: 	"fmt"
6: 	"math"
7: 	"sort"
8: )
9: 
10: const (
11: 	defaultHighPassFilterCutoff   = 30
12: 	defaultMinimalPeriod          = 6
13: 	defaultMaximalPeriod          = 30
14: 	defaultDecibelsLowerThreshold = 6.
15: 	defaultDecibelsUpperThreshold = 20.
16: 
17: 	highPassFilterBufferSize = 6
18: 	// Firs coefficients {1, 2, 3, 3, 2, 1} / 12. Index 0 = oldest, index 5 = current.
19: 	firCoefSum = 12.
20: 
21: 	deltaLowerThreshold = 0.1
22: 	deltaFactor         = -0.015
23: 	deltaSummand        = 0.5
24: 
25: 	dominantCycleBufferSize = 5
26: 	// Median index: 5 sorted values → take index 2 (middle).
27: 	dominantCycleMedianIndex = 2
28: 
29: 	decibelsSmoothingAlpha    = 0.33
30: 	decibelsSmoothingOneMinus = 0.67
31: 
32: 	// 1 - 0.99 * normalized amplitude, clamped away from zero by 0.01.
33: 	normalizedAmplitudeFactor = 0.99
34: 	decibelsFloor             = 0.01
35: 	decibelsGain              = 10.
36: )
37: 
38: // Filter holds the per-bin state of a single bandpass filter in the bank.
39: type Filter struct {
40: 	InPhase        float64
41: 	InPhasePrev    float64
42: 	Quadrature     float64
43: 	QuadraturePrev float64
44: 	Real           float64
45: 	RealPrev       float64
46: 	Imaginary      float64
47: 	ImaginaryPrev  float64
48: 
49: 	// AmplitudeSquared is the |Real + j·Imaginary|² of the most recent update.
50: 	AmplitudeSquared float64
51: 
52: 	// Decibels is the smoothed dB value of the most recent update.
53: 	Decibels float64
54: }
55: 
56: // Corona is the shared spectral-analysis engine. It is not an indicator on its
57: // own; it is consumed by the CoronaSpectrum, CoronaSignalToNoiseRatio,
58: // CoronaSwingPosition and CoronaTrendVigor indicators.
59: //
60: // Call Update(sample) once per bar. Read IsPrimed, DominantCycle,
61: // DominantCycleMedian, MaximalAmplitudeSquared and the FilterBank slice.
62: //
63: // Corona is not safe for concurrent use.
64: type Corona struct {
65: 	// Configuration (immutable after construction).
66: 	minimalPeriod          int
67: 	maximalPeriod          int
68: 	minimalPeriodTimesTwo  int
69: 	maximalPeriodTimesTwo  int
70: 	filterBankLength       int
71: 	decibelsLowerThreshold float64
72: 	decibelsUpperThreshold float64
73: 
74: 	// High-pass filter coefficients.
75: 	alpha            float64
76: 	halfOnePlusAlpha float64
77: 
78: 	// Pre-calculated cos(4π/n) for n = MinimalPeriodTimesTwo..MaximalPeriodTimesTwo.
79: 	preCalculatedBeta []float64
80: 
81: 	// HP ring buffer (oldest at index 0, current at index 5).
82: 	highPassBuffer [highPassFilterBufferSize]float64
83: 
84: 	// Previous raw sample and previous smoothed HP (for momentum = smoothHP - smoothHPprev).
85: 	samplePrev   float64
86: 	smoothHPPrev float64
87: 
88: 	// Filter bank.
89: 	filterBank []Filter
90: 
91: 	// Running maximum of amplitude-squared across all filter bins for the most
92: 	// recent Update. Reset to zero each bar, per MBST.
93: 	maximalAmplitudeSquared float64
94: 
95: 	// 5-sample ring buffer (oldest first) used for the dominant cycle median.
96: 	// Populated with math.MaxFloat64 sentinels on construction, matching MBST.
97: 	dominantCycleBuffer [dominantCycleBufferSize]float64
98: 
99: 	// Sample counter (number of Update calls seen).
100: 	sampleCount int
101: 
102: 	// Most recent dominant cycle estimate and its 5-sample median.
103: 	dominantCycle       float64
104: 	dominantCycleMedian float64
105: 
106: 	primed bool
107: }
108: 
109: // NewCorona creates a new Corona engine using the provided parameters.
110: // Zero-valued fields in p are replaced with the Ehlers defaults.
111: // Pass nil to use all defaults.
112: func NewCorona(p *Params) (*Corona, error) {
113: 	var cfg Params
114: 	if p != nil {
115: 		cfg = *p
116: 	}
117: 	applyDefaults(&cfg)
118: 
119: 	if err := verifyParameters(&cfg); err != nil {
120: 		return nil, err
121: 	}
122: 
123: 	c := &Corona{
124: 		minimalPeriod:          cfg.MinimalPeriod,
125: 		maximalPeriod:          cfg.MaximalPeriod,
126: 		minimalPeriodTimesTwo:  cfg.MinimalPeriod * 2,
127: 		maximalPeriodTimesTwo:  cfg.MaximalPeriod * 2,
128: 		decibelsLowerThreshold: cfg.DecibelsLowerThreshold,
129: 		decibelsUpperThreshold: cfg.DecibelsUpperThreshold,
130: 	}
131: 
132: 	c.filterBankLength = c.maximalPeriodTimesTwo - c.minimalPeriodTimesTwo + 1
133: 	c.filterBank = make([]Filter, c.filterBankLength)
134: 
135: 	// MBST initializes the dominant cycle buffer with MaxValue sentinels, so the
136: 	// median is meaningful only once the buffer is fully populated with real
137: 	// samples. We reproduce that behaviour exactly.
138: 	for i := range c.dominantCycleBuffer {
139: 		c.dominantCycleBuffer[i] = math.MaxFloat64
140: 	}
141: 	c.dominantCycle = math.MaxFloat64
142: 	c.dominantCycleMedian = math.MaxFloat64
143: 
144: 	// High-pass filter coefficients.
145: 	phi := 2. * math.Pi / float64(cfg.HighPassFilterCutoff)
146: 	c.alpha = (1. - math.Sin(phi)) / math.Cos(phi)
147: 	c.halfOnePlusAlpha = 0.5 * (1. + c.alpha)
148: 
149: 	// Pre-calculate β = cos(4π / n) for each half-period index n.
150: 	c.preCalculatedBeta = make([]float64, c.filterBankLength)
151: 	for idx := 0; idx < c.filterBankLength; idx++ {
152: 		n := c.minimalPeriodTimesTwo + idx
153: 		c.preCalculatedBeta[idx] = math.Cos(4. * math.Pi / float64(n))
154: 	}
155: 
156: 	return c, nil
157: }
158: 
159: // applyDefaults fills zero-valued Params fields with defaults in-place.
160: func applyDefaults(p *Params) {
161: 	if p.HighPassFilterCutoff <= 0 {
162: 		p.HighPassFilterCutoff = defaultHighPassFilterCutoff
163: 	}
164: 	if p.MinimalPeriod <= 0 {
165: 		p.MinimalPeriod = defaultMinimalPeriod
166: 	}
167: 	if p.MaximalPeriod <= 0 {
168: 		p.MaximalPeriod = defaultMaximalPeriod
169: 	}
170: 	if p.DecibelsLowerThreshold == 0 {
171: 		p.DecibelsLowerThreshold = defaultDecibelsLowerThreshold
172: 	}
173: 	if p.DecibelsUpperThreshold == 0 {
174: 		p.DecibelsUpperThreshold = defaultDecibelsUpperThreshold
175: 	}
176: }
177: 
178: func verifyParameters(p *Params) error {
179: 	const (
180: 		invalid = "invalid corona parameters"
181: 		fmts    = "%s: %s"
182: 	)
183: 
184: 	if p.HighPassFilterCutoff < 2 {
185: 		return fmt.Errorf(fmts, invalid, "HighPassFilterCutoff should be >= 2")
186: 	}
187: 	if p.MinimalPeriod < 2 {
188: 		return fmt.Errorf(fmts, invalid, "MinimalPeriod should be >= 2")
189: 	}
190: 	if p.MaximalPeriod <= p.MinimalPeriod {
191: 		return fmt.Errorf(fmts, invalid, "MaximalPeriod should be > MinimalPeriod")
192: 	}
193: 	if p.DecibelsLowerThreshold < 0 {
194: 		return fmt.Errorf(fmts, invalid, "DecibelsLowerThreshold should be >= 0")
195: 	}
196: 	if p.DecibelsUpperThreshold <= p.DecibelsLowerThreshold {
197: 		return fmt.Errorf(fmts, invalid, "DecibelsUpperThreshold should be > DecibelsLowerThreshold")
198: 	}
199: 
200: 	return nil
201: }
202: 
203: // MinimalPeriod returns the minimum cycle period covered by the filter bank.
204: func (c *Corona) MinimalPeriod() int { return c.minimalPeriod }
205: 
206: // MaximalPeriod returns the maximum cycle period covered by the filter bank.
207: func (c *Corona) MaximalPeriod() int { return c.maximalPeriod }
208: 
209: // MinimalPeriodTimesTwo returns the minimum filter-bank half-period index.
210: func (c *Corona) MinimalPeriodTimesTwo() int { return c.minimalPeriodTimesTwo }
211: 
212: // MaximalPeriodTimesTwo returns the maximum filter-bank half-period index.
213: func (c *Corona) MaximalPeriodTimesTwo() int { return c.maximalPeriodTimesTwo }
214: 
215: // FilterBankLength returns the number of filter bins (MaxPeriod*2 - MinPeriod*2 + 1).
216: func (c *Corona) FilterBankLength() int { return c.filterBankLength }
217: 
218: // FilterBank returns a read-only view of the filter bank. Do not mutate.
219: func (c *Corona) FilterBank() []Filter { return c.filterBank }
220: 
221: // IsPrimed reports whether the engine has seen enough samples to produce
222: // meaningful output (sampleCount >= MinimalPeriodTimesTwo).
223: func (c *Corona) IsPrimed() bool { return c.primed }
224: 
225: // DominantCycle returns the most recent weighted-center-of-gravity estimate of
226: // the dominant cycle period. Returns MinimalPeriod (as a float) before
227: // priming.
228: func (c *Corona) DominantCycle() float64 { return c.dominantCycle }
229: 
230: // DominantCycleMedian returns the 5-sample median of the most recent dominant
231: // cycle estimates.
232: func (c *Corona) DominantCycleMedian() float64 { return c.dominantCycleMedian }
233: 
234: // MaximalAmplitudeSquared returns the maximum amplitude-squared observed across
235: // the filter bank for the most recently processed sample. Matches MBST: reset
236: // to zero at the start of every Update call, not a running maximum across time.
237: func (c *Corona) MaximalAmplitudeSquared() float64 { return c.maximalAmplitudeSquared }
238: 
239: // Update feeds the next sample to the engine.
240: //
241: // The sample is assumed to be a raw price value (typically (High+Low)/2).
242: // NaN samples are treated as a no-op that leaves state unchanged and returns
243: // without marking progress.
244: //
245: // Returns true once IsPrimed has been reached and the current bar's outputs
246: // are meaningful.
247: func (c *Corona) Update(sample float64) bool {
248: 	if math.IsNaN(sample) {
249: 		return c.primed
250: 	}
251: 
252: 	c.sampleCount++
253: 
254: 	// First sample: MBST stores it as the prior-sample reference and returns
255: 	// with no further processing. Preserve that behaviour bit-for-bit.
256: 	if c.sampleCount == 1 {
257: 		c.samplePrev = sample
258: 
259: 		return false
260: 	}
261: 
262: 	// Step 1: High-pass filter.
263: 	// HP[new] = α · HP[prev] + halfOnePlusAlpha · (sample - samplePrev)
264: 	hp := c.alpha*c.highPassBuffer[highPassFilterBufferSize-1] +
265: 		c.halfOnePlusAlpha*(sample-c.samplePrev)
266: 	c.samplePrev = sample
267: 
268: 	// Shift buffer left: buffer[0] drops, newest goes to buffer[5].
269: 	for i := 0; i < highPassFilterBufferSize-1; i++ {
270: 		c.highPassBuffer[i] = c.highPassBuffer[i+1]
271: 	}
272: 	c.highPassBuffer[highPassFilterBufferSize-1] = hp
273: 
274: 	// Step 2: 6-tap FIR smoothing with coefficients {1, 2, 3, 3, 2, 1} / 12.
275: 	// buffer[0] is oldest → weight 1.
276: 	smoothHP := (c.highPassBuffer[0] +
277: 		2*c.highPassBuffer[1] +
278: 		3*c.highPassBuffer[2] +
279: 		3*c.highPassBuffer[3] +
280: 		2*c.highPassBuffer[4] +
281: 		c.highPassBuffer[5]) / firCoefSum
282: 
283: 	// Step 3: Momentum = current smoothHP − previous smoothHP.
284: 	momentum := smoothHP - c.smoothHPPrev
285: 	c.smoothHPPrev = smoothHP
286: 
287: 	// Step 4: Adaptive delta.
288: 	delta := deltaFactor*float64(c.sampleCount) + deltaSummand
289: 	if delta < deltaLowerThreshold {
290: 		delta = deltaLowerThreshold
291: 	}
292: 
293: 	// Step 5: Filter-bank update. Per MBST, MaximalAmplitudeSquared is reset
294: 	// to zero each bar and becomes the max across the current bar's bank.
295: 	c.maximalAmplitudeSquared = 0
296: 	for idx := 0; idx < c.filterBankLength; idx++ {
297: 		n := c.minimalPeriodTimesTwo + idx
298: 		nf := float64(n)
299: 
300: 		gamma := 1. / math.Cos(8.*math.Pi*delta/nf)
301: 		a := gamma - math.Sqrt(gamma*gamma-1.)
302: 
303: 		quadrature := momentum * (nf / (4. * math.Pi))
304: 		inPhase := smoothHP
305: 
306: 		halfOneMinA := 0.5 * (1. - a)
307: 		beta := c.preCalculatedBeta[idx]
308: 		betaOnePlusA := beta * (1. + a)
309: 
310: 		f := &c.filterBank[idx]
311: 
312: 		real := halfOneMinA*(inPhase-f.InPhasePrev) + betaOnePlusA*f.Real - a*f.RealPrev
313: 		imag := halfOneMinA*(quadrature-f.QuadraturePrev) + betaOnePlusA*f.Imaginary - a*f.ImaginaryPrev
314: 
315: 		ampSq := real*real + imag*imag
316: 
317: 		// Shift state: prev <- current, current <- new.
318: 		f.InPhasePrev = f.InPhase
319: 		f.InPhase = inPhase
320: 		f.QuadraturePrev = f.Quadrature
321: 		f.Quadrature = quadrature
322: 		f.RealPrev = f.Real
323: 		f.Real = real
324: 		f.ImaginaryPrev = f.Imaginary
325: 		f.Imaginary = imag
326: 		f.AmplitudeSquared = ampSq
327: 
328: 		if ampSq > c.maximalAmplitudeSquared {
329: 			c.maximalAmplitudeSquared = ampSq
330: 		}
331: 	}
332: 
333: 	// Step 6: dB normalization and dominant-cycle weighted average.
334: 	// MBST resets DominantCycle to 0 before the loop; if no bin qualifies it
335: 	// remains 0 and is subsequently clamped up to MinimalPeriod.
336: 	var numerator, denominator float64
337: 	c.dominantCycle = 0
338: 	for idx := 0; idx < c.filterBankLength; idx++ {
339: 		f := &c.filterBank[idx]
340: 
341: 		decibels := 0.
342: 		if c.maximalAmplitudeSquared > 0 {
343: 			normalized := f.AmplitudeSquared / c.maximalAmplitudeSquared
344: 			if normalized > 0 {
345: 				// dB = 10 · log10( (1 − 0.99·norm) / 0.01 )
346: 				arg := (1. - normalizedAmplitudeFactor*normalized) / decibelsFloor
347: 				if arg > 0 {
348: 					decibels = decibelsGain * math.Log10(arg)
349: 				}
350: 			}
351: 		}
352: 
353: 		// EMA smoothing: dB = 0.33·new + 0.67·old.
354: 		decibels = decibelsSmoothingAlpha*decibels + decibelsSmoothingOneMinus*f.Decibels
355: 		if decibels > c.decibelsUpperThreshold {
356: 			decibels = c.decibelsUpperThreshold
357: 		}
358: 		f.Decibels = decibels
359: 
360: 		// Only bins with dB at or below the lower threshold contribute.
361: 		if decibels <= c.decibelsLowerThreshold {
362: 			n := float64(c.minimalPeriodTimesTwo + idx)
363: 			adjusted := c.decibelsUpperThreshold - decibels
364: 			numerator += n * adjusted
365: 			denominator += adjusted
366: 		}
367: 	}
368: 
369: 	// Compute DC as 0.5 · num / denom. The factor 0.5 converts the
370: 	// half-period index space (12..60) back to period space (6..30).
371: 	if denominator != 0 {
372: 		c.dominantCycle = 0.5 * numerator / denominator
373: 	}
374: 	if c.dominantCycle < float64(c.minimalPeriod) {
375: 		c.dominantCycle = float64(c.minimalPeriod)
376: 	}
377: 
378: 	// Step 7: 5-sample median of dominant cycle.
379: 	// Shift left and push current; median is sorted[index 2] per MBST.
380: 	for i := 0; i < dominantCycleBufferSize-1; i++ {
381: 		c.dominantCycleBuffer[i] = c.dominantCycleBuffer[i+1]
382: 	}
383: 	c.dominantCycleBuffer[dominantCycleBufferSize-1] = c.dominantCycle
384: 
385: 	var sorted [dominantCycleBufferSize]float64
386: 	sorted = c.dominantCycleBuffer
387: 	sort.Float64s(sorted[:])
388: 	c.dominantCycleMedian = sorted[dominantCycleMedianIndex]
389: 	if c.dominantCycleMedian < float64(c.minimalPeriod) {
390: 		c.dominantCycleMedian = float64(c.minimalPeriod)
391: 	}
392: 
393: 	if c.sampleCount < c.minimalPeriodTimesTwo {
394: 		return false
395: 	}
396: 	c.primed = true
397: 
398: 	return true
399: }

(End of file - total 399 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.7s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/corona/params.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/corona/params.go</path>
<type>file</type>
<content>1: package corona
2: 
3: // Params configures a Corona spectral analysis engine.
4: //
5: // All fields have zero-value defaults: a zero value means "use the default".
6: // The defaults follow Ehlers' original TASC article (November 2008).
7: type Params struct {
8: 	// HighPassFilterCutoff is the cutoff period of the detrending high-pass
9: 	// filter, in bars. Default: 30. Zero or negative => default.
10: 	HighPassFilterCutoff int
11: 
12: 	// MinimalPeriod is the minimum cycle period (in bars) covered by the
13: 	// bandpass filter bank. Must be >= 2. Default: 6. Zero => default.
14: 	MinimalPeriod int
15: 
16: 	// MaximalPeriod is the maximum cycle period (in bars) covered by the
17: 	// bandpass filter bank. Must be > MinimalPeriod. Default: 30. Zero => default.
18: 	MaximalPeriod int
19: 
20: 	// DecibelsLowerThreshold: filter bins with smoothed dB value at or below
21: 	// this threshold contribute to the weighted dominant-cycle estimate.
22: 	// Default: 6. Zero => default.
23: 	DecibelsLowerThreshold float64
24: 
25: 	// DecibelsUpperThreshold: upper clamp on the smoothed dB value and
26: 	// reference value for the dominant-cycle weighting (weight = upper - dB).
27: 	// Default: 20. Zero => default.
28: 	DecibelsUpperThreshold float64
29: }
30: 
31: // DefaultParams returns a Params value populated with Ehlers defaults.
32: func DefaultParams() *Params {
33: 	return &Params{
34: 		HighPassFilterCutoff:   30,
35: 		MinimalPeriod:          6,
36: 		MaximalPeriod:          30,
37: 		DecibelsLowerThreshold: 6,
38: 		DecibelsUpperThreshold: 20,
39: 	}
40: }

(End of file - total 40 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 6.4s)

_Thinking:_

 I'm examining the TCM TS template structure and checking the trend-cycle-mode-params.ts file to understand the code style and organization I need to follow.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode</path>
<type>directory</type>
<entries>
trend-cycle-mode-output.ts
trend-cycle-mode-params.ts
trend-cycle-mode.spec.ts
trend-cycle-mode.ts

(4 entries)
</entries>
```

---

## Assistant (Build · claude-opus-4.7 · 3.5s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.ts</path>
<type>file</type>
<content>1: import { Bar } from '../../../entities/bar';
2: import { BarComponent, barComponentValue } from '../../../entities/bar-component';
3: import { Quote } from '../../../entities/quote';
4: import { DefaultQuoteComponent, quoteComponentValue } from '../../../entities/quote-component';
5: import { Scalar } from '../../../entities/scalar';
6: import { Trade } from '../../../entities/trade';
7: import { DefaultTradeComponent, tradeComponentValue } from '../../../entities/trade-component';
8: import { componentTripleMnemonic } from '../../core/component-triple-mnemonic';
9: import { Indicator } from '../../core/indicator';
10: import { IndicatorMetadata } from '../../core/indicator-metadata';
11: import { IndicatorOutput } from '../../core/indicator-output';
12: import { IndicatorType } from '../../core/indicator-type';
13: import { OutputType } from '../../core/outputs/output-type';
14: import { DominantCycle } from '../dominant-cycle/dominant-cycle';
15: import { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';
16: import { createEstimator, estimatorMoniker } from '../hilbert-transformer/hilbert-transformer-common';
17: import { TrendCycleModeOutput } from './trend-cycle-mode-output';
18: import { TrendCycleModeParams } from './trend-cycle-mode-params';
19: 
20: const DEFAULT_ALPHA_EMA_PERIOD_ADDITIONAL = 0.33;
21: const DEFAULT_SMOOTHING_LENGTH = 4;
22: const DEFAULT_ALPHA_EMA_QI = 0.2;
23: const DEFAULT_ALPHA_EMA_PERIOD = 0.2;
24: // MBST's DominantCyclePeriod default warm-up is MaxPeriod * 2 = 100.
25: const DEFAULT_WARM_UP_PERIOD = 100;
26: const DEFAULT_TREND_LINE_SMOOTHING_LENGTH = 4;
27: const DEFAULT_CYCLE_PART_MULTIPLIER = 1.0;
28: const DEFAULT_SEPARATION_PERCENTAGE = 1.5;
29: const MAX_CYCLE_PART_MULTIPLIER = 10.0;
30: const MAX_SEPARATION_PERCENTAGE = 100.0;
31: 
32: const DEG2RAD = Math.PI / 180.0;
33: const LEAD_OFFSET = 45.0;
34: const FULL_CYCLE = 360.0;
35: const EPSILON = 1e-308;
36: 
37: /** __Trend versus Cycle Mode__ (Ehlers) classifies the market as in-trend or in-cycle based
38:  * on the behaviour of the instantaneous dominant cycle period/phase and a WMA-smoothed
39:  * instantaneous trend line.
40:  *
41:  * It exposes eight outputs:
42:  *
43:  *	- Value: +1 in trend mode, -1 in cycle mode.
44:  *	- IsTrendMode: 1 if the trend mode is declared, 0 otherwise.
45:  *	- IsCycleMode: 1 if the cycle mode is declared, 0 otherwise (= 1 − IsTrendMode).
46:  *	- InstantaneousTrendLine: the WMA-smoothed instantaneous trend line.
47:  *	- SineWave: sin(phase·Deg2Rad).
48:  *	- SineWaveLead: sin((phase+45)·Deg2Rad).
49:  *	- DominantCyclePeriod: the smoothed dominant cycle period.
50:  *	- DominantCyclePhase: the dominant cycle phase, in degrees.
51:  *
52:  * Reference:
53:  *
54:  *	John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 113-118.
55:  */
56: export class TrendCycleMode implements Indicator {
57:   private readonly dc: DominantCycle;
58:   private readonly cyclePartMultiplier: number;
59:   private readonly separationFactor: number;
60:   private readonly coeff0: number;
61:   private readonly coeff1: number;
62:   private readonly coeff2: number;
63:   private readonly coeff3: number;
64:   private readonly input: number[];
65:   private readonly inputLength: number;
66:   private readonly inputLengthMin1: number;
67: 
68:   private trendline = Number.NaN;
69:   private trendAverage1 = 0;
70:   private trendAverage2 = 0;
71:   private trendAverage3 = 0;
72:   private sinWave = Number.NaN;
73:   private sinWaveLead = Number.NaN;
74:   private prevDcPhase = 0;
75:   private prevSineLeadWaveDifference = 0;
76:   private samplesInTrend = 0;
77:   private isTrendMode = true;
78:   private primed = false;
79: 
80:   private readonly mnemonicValue: string;
81:   private readonly descriptionValue: string;
82:   private readonly mnemonicTrend: string;
83:   private readonly descriptionTrend: string;
84:   private readonly mnemonicCycle: string;
85:   private readonly descriptionCycle: string;
86:   private readonly mnemonicITL: string;
87:   private readonly descriptionITL: string;
88:   private readonly mnemonicSine: string;
89:   private readonly descriptionSine: string;
90:   private readonly mnemonicSineLead: string;
91:   private readonly descriptionSineLead: string;
92:   private readonly mnemonicDCP: string;
93:   private readonly descriptionDCP: string;
94:   private readonly mnemonicDCPhase: string;
95:   private readonly descriptionDCPhase: string;
96: 
97:   private readonly barComponentFunc: (bar: Bar) => number;
98:   private readonly quoteComponentFunc: (quote: Quote) => number;
99:   private readonly tradeComponentFunc: (trade: Trade) => number;
100: 
101:   /** Creates an instance using default parameters (α=0.33, trendLineSmoothingLength=4,
102:    * cyclePartMultiplier=1.0, separationPercentage=1.5, HomodyneDiscriminator cycle estimator
103:    * with smoothingLength=4, αq=0.2, αp=0.2, warmUpPeriod=100, BarComponent.Median). */
104:   public static default(): TrendCycleMode {
105:     return new TrendCycleMode({
106:       alphaEmaPeriodAdditional: DEFAULT_ALPHA_EMA_PERIOD_ADDITIONAL,
107:       trendLineSmoothingLength: DEFAULT_TREND_LINE_SMOOTHING_LENGTH,
108:       cyclePartMultiplier: DEFAULT_CYCLE_PART_MULTIPLIER,
109:       separationPercentage: DEFAULT_SEPARATION_PERCENTAGE,
110:       estimatorType: HilbertTransformerCycleEstimatorType.HomodyneDiscriminator,
111:       estimatorParams: {
112:         smoothingLength: DEFAULT_SMOOTHING_LENGTH,
113:         alphaEmaQuadratureInPhase: DEFAULT_ALPHA_EMA_QI,
114:         alphaEmaPeriod: DEFAULT_ALPHA_EMA_PERIOD,
115:         warmUpPeriod: DEFAULT_WARM_UP_PERIOD,
116:       },
117:     });
118:   }
119: 
120:   /** Creates an instance based on the given parameters. */
121:   public static fromParams(params: TrendCycleModeParams): TrendCycleMode {
122:     return new TrendCycleMode(params);
123:   }
124: 
125:   private constructor(params: TrendCycleModeParams) {
126:     const alpha = params.alphaEmaPeriodAdditional;
127:     if (alpha <= 0 || alpha > 1) {
128:       throw new Error('invalid trend cycle mode parameters: α for additional smoothing should be in range (0, 1]');
129:     }
130: 
131:     const tlsl = params.trendLineSmoothingLength ?? DEFAULT_TREND_LINE_SMOOTHING_LENGTH;
132:     if (tlsl < 2 || tlsl > 4 || !Number.isInteger(tlsl)) {
133:       throw new Error('invalid trend cycle mode parameters: trend line smoothing length should be 2, 3, or 4');
134:     }
135: 
136:     const cpm = params.cyclePartMultiplier ?? DEFAULT_CYCLE_PART_MULTIPLIER;
137:     if (cpm <= 0 || cpm > MAX_CYCLE_PART_MULTIPLIER) {
138:       throw new Error('invalid trend cycle mode parameters: cycle part multiplier should be in range (0, 10]');
139:     }
140: 
141:     const sep = params.separationPercentage ?? DEFAULT_SEPARATION_PERCENTAGE;
142:     if (sep <= 0 || sep > MAX_SEPARATION_PERCENTAGE) {
143:       throw new Error('invalid trend cycle mode parameters: separation percentage should be in range (0, 100]');
144:     }
145: 
146:     this.cyclePartMultiplier = cpm;
147:     this.separationFactor = sep / 100.0;
148: 
149:     // TCM defaults to BarComponent.Median (MBST default; always shown in mnemonic).
150:     const bc = params.barComponent ?? BarComponent.Median;
151:     const qc = params.quoteComponent ?? DefaultQuoteComponent;
152:     const tc = params.tradeComponent ?? DefaultTradeComponent;
153: 
154:     this.barComponentFunc = barComponentValue(bc);
155:     this.quoteComponentFunc = quoteComponentValue(qc);
156:     this.tradeComponentFunc = tradeComponentValue(tc);
157: 
158:     // Inner DominantCycle built with explicit components so its mnemonic contains hl/2.
159:     this.dc = DominantCycle.fromParams({
160:       alphaEmaPeriodAdditional: alpha,
161:       estimatorType: params.estimatorType,
162:       estimatorParams: params.estimatorParams,
163:       barComponent: bc,
164:       quoteComponent: qc,
165:       tradeComponent: tc,
166:     });
167: 
168:     // Compose estimator moniker (same logic as DominantCycle / SineWave).
169:     const effectiveType = params.estimatorType ?? HilbertTransformerCycleEstimatorType.HomodyneDiscriminator;
170:     const htce = createEstimator(params.estimatorType, params.estimatorParams);
171:     let em = '';
172:     const isDefaultHd = effectiveType === HilbertTransformerCycleEstimatorType.HomodyneDiscriminator
173:       && htce.smoothingLength === DEFAULT_SMOOTHING_LENGTH
174:       && htce.alphaEmaQuadratureInPhase === DEFAULT_ALPHA_EMA_QI
175:       && htce.alphaEmaPeriod === DEFAULT_ALPHA_EMA_PERIOD;
176:     if (!isDefaultHd) {
177:       const moniker = estimatorMoniker(effectiveType, htce);
178:       if (moniker.length > 0) {
179:         em = ', ' + moniker;
180:       }
181:     }
182: 
183:     const cm = componentTripleMnemonic(bc, qc, tc);
184:     const a = alpha.toFixed(3);
185:     const c = cpm.toFixed(3);
186:     const s = sep.toFixed(3);
187:     const tail = `${a}, ${tlsl}, ${c}, ${s}%${em}${cm}`;
188: 
189:     this.mnemonicValue = `tcm(${tail})`;
190:     this.mnemonicTrend = `tcm-trend(${tail})`;
191:     this.mnemonicCycle = `tcm-cycle(${tail})`;
192:     this.mnemonicITL = `tcm-itl(${tail})`;
193:     this.mnemonicSine = `tcm-sine(${tail})`;
194:     this.mnemonicSineLead = `tcm-sineLead(${tail})`;
195:     this.mnemonicDCP = `dcp(${a}${em}${cm})`;
196:     this.mnemonicDCPhase = `dcph(${a}${em}${cm})`;
197: 
198:     this.descriptionValue = 'Trend versus cycle mode ' + this.mnemonicValue;
199:     this.descriptionTrend = 'Trend versus cycle mode, is-trend flag ' + this.mnemonicTrend;
200:     this.descriptionCycle = 'Trend versus cycle mode, is-cycle flag ' + this.mnemonicCycle;
201:     this.descriptionITL = 'Trend versus cycle mode instantaneous trend line ' + this.mnemonicITL;
202:     this.descriptionSine = 'Trend versus cycle mode sine wave ' + this.mnemonicSine;
203:     this.descriptionSineLead = 'Trend versus cycle mode sine wave lead ' + this.mnemonicSineLead;
204:     this.descriptionDCP = 'Dominant cycle period ' + this.mnemonicDCP;
205:     this.descriptionDCPhase = 'Dominant cycle phase ' + this.mnemonicDCPhase;
206: 
207:     // WMA coefficients.
208:     let c0 = 0, c1 = 0, c2 = 0, c3 = 0;
209:     if (tlsl === 2) {
210:       c0 = 2 / 3; c1 = 1 / 3;
211:     } else if (tlsl === 3) {
212:       c0 = 3 / 6; c1 = 2 / 6; c2 = 1 / 6;
213:     } else { // tlsl === 4
214:       c0 = 4 / 10; c1 = 3 / 10; c2 = 2 / 10; c3 = 1 / 10;
215:     }
216:     this.coeff0 = c0;
217:     this.coeff1 = c1;
218:     this.coeff2 = c2;
219:     this.coeff3 = c3;
220: 
221:     const maxPeriod = this.dc.maxPeriod;
222:     this.input = new Array<number>(maxPeriod).fill(0);
223:     this.inputLength = maxPeriod;
224:     this.inputLengthMin1 = maxPeriod - 1;
225:   }
226: 
227:   /** Indicates whether the indicator is primed. */
228:   public isPrimed(): boolean { return this.primed; }
229: 
230:   /** Describes the output data of the indicator. */
231:   public metadata(): IndicatorMetadata {
232:     return {
233:       type: IndicatorType.TrendCycleMode,
234:       mnemonic: this.mnemonicValue,
235:       description: this.descriptionValue,
236:       outputs: [
237:         {
238:           kind: TrendCycleModeOutput.Value,
239:           type: OutputType.Scalar,
240:           mnemonic: this.mnemonicValue,
241:           description: this.descriptionValue,
242:         },
243:         {
244:           kind: TrendCycleModeOutput.IsTrendMode,
245:           type: OutputType.Scalar,
246:           mnemonic: this.mnemonicTrend,
247:           description: this.descriptionTrend,
248:         },
249:         {
250:           kind: TrendCycleModeOutput.IsCycleMode,
251:           type: OutputType.Scalar,
252:           mnemonic: this.mnemonicCycle,
253:           description: this.descriptionCycle,
254:         },
255:         {
256:           kind: TrendCycleModeOutput.InstantaneousTrendLine,
257:           type: OutputType.Scalar,
258:           mnemonic: this.mnemonicITL,
259:           description: this.descriptionITL,
260:         },
261:         {
262:           kind: TrendCycleModeOutput.SineWave,
263:           type: OutputType.Scalar,
264:           mnemonic: this.mnemonicSine,
265:           description: this.descriptionSine,
266:         },
267:         {
268:           kind: TrendCycleModeOutput.SineWaveLead,
269:           type: OutputType.Scalar,
270:           mnemonic: this.mnemonicSineLead,
271:           description: this.descriptionSineLead,
272:         },
273:         {
274:           kind: TrendCycleModeOutput.DominantCyclePeriod,
275:           type: OutputType.Scalar,
276:           mnemonic: this.mnemonicDCP,
277:           description: this.descriptionDCP,
278:         },
279:         {
280:           kind: TrendCycleModeOutput.DominantCyclePhase,
281:           type: OutputType.Scalar,
282:           mnemonic: this.mnemonicDCPhase,
283:           description: this.descriptionDCPhase,
284:         },
285:       ],
286:     };
287:   }
288: 
289:   /** Updates the indicator given the next sample value. Returns the 8-tuple
290:    * (value, isTrendMode, isCycleMode, trendline, sineWave, sineWaveLead, period, phase).
291:    * isTrendMode / isCycleMode are encoded as 1 / 0 scalars. Returns all-NaN if not primed. */
292:   public update(sample: number): [number, number, number, number, number, number, number, number] {
293:     if (Number.isNaN(sample)) {
294:       return [sample, sample, sample, sample, sample, sample, sample, sample];
295:     }
296: 
297:     const [, period, phase] = this.dc.update(sample);
298:     const smoothedPrice = this.dc.smoothedPrice;
299: 
300:     this.pushInput(sample);
301: 
302:     if (this.primed) {
303:       const smoothedPeriod = period;
304:       const average = this.calculateTrendAverage(smoothedPeriod);
305:       this.trendline = this.coeff0 * average
306:         + this.coeff1 * this.trendAverage1
307:         + this.coeff2 * this.trendAverage2
308:         + this.coeff3 * this.trendAverage3;
309:       this.trendAverage3 = this.trendAverage2;
310:       this.trendAverage2 = this.trendAverage1;
311:       this.trendAverage1 = average;
312: 
313:       const diff = this.calculateSineLeadWaveDifference(phase);
314: 
315:       // Condition 1: a cycle mode exists for the half-period of a dominant cycle after
316:       // the SineWave vs SineWaveLead crossing.
317:       this.isTrendMode = true;
318: 
319:       if ((diff > 0 && this.prevSineLeadWaveDifference < 0)
320:         || (diff < 0 && this.prevSineLeadWaveDifference > 0)) {
321:         this.isTrendMode = false;
322:         this.samplesInTrend = 0;
323:       }
324: 
325:       this.prevSineLeadWaveDifference = diff;
326:       this.samplesInTrend++;
327: 
328:       if (this.samplesInTrend < 0.5 * smoothedPeriod) {
329:         this.isTrendMode = false;
330:       }
331: 
332:       // Condition 2: cycle mode if the measured phase rate of change is more than 2/3
333:       // the phase rate of change of the dominant cycle (360/period) and less than 1.5×.
334:       const phaseDelta = phase - this.prevDcPhase;
335:       this.prevDcPhase = phase;
336: 
337:       if (Math.abs(smoothedPeriod) > EPSILON) {
338:         const dcRate = FULL_CYCLE / smoothedPeriod;
339:         if (phaseDelta > (2.0 / 3.0) * dcRate && phaseDelta < 1.5 * dcRate) {
340:           this.isTrendMode = false;
341:         }
342:       }
343: 
344:       // Condition 3: if the WMA smoothed price is separated by more than the separation
345:       // percentage from the instantaneous trend line, force the trend mode.
346:       if (Math.abs(this.trendline) > EPSILON
347:         && Math.abs((smoothedPrice - this.trendline) / this.trendline) >= this.separationFactor) {
348:         this.isTrendMode = true;
349:       }
350: 
351:       return [
352:         this.mode(), this.isTrendFloat(), this.isCycleFloat(),
353:         this.trendline, this.sinWave, this.sinWaveLead, period, phase,
354:       ];
355:     }
356: 
357:     if (this.dc.isPrimed()) {
358:       this.primed = true;
359:       const smoothedPeriod = period;
360:       this.trendline = this.calculateTrendAverage(smoothedPeriod);
361:       this.trendAverage1 = this.trendline;
362:       this.trendAverage2 = this.trendline;
363:       this.trendAverage3 = this.trendline;
364: 
365:       this.prevDcPhase = phase;
366:       this.prevSineLeadWaveDifference = this.calculateSineLeadWaveDifference(phase);
367: 
368:       this.isTrendMode = true;
369:       this.samplesInTrend++;
370: 
371:       if (this.samplesInTrend < 0.5 * smoothedPeriod) {
372:         this.isTrendMode = false;
373:       }
374: 
375:       return [
376:         this.mode(), this.isTrendFloat(), this.isCycleFloat(),
377:         this.trendline, this.sinWave, this.sinWaveLead, period, phase,
378:       ];
379:     }
380: 
381:     const n = Number.NaN;
382:     return [n, n, n, n, n, n, n, n];
383:   }
384: 
385:   /** Updates an indicator given the next scalar sample. */
386:   public updateScalar(sample: Scalar): IndicatorOutput {
387:     return this.updateEntity(sample.time, sample.value);
388:   }
389: 
390:   /** Updates an indicator given the next bar sample. */
391:   public updateBar(sample: Bar): IndicatorOutput {
392:     return this.updateEntity(sample.time, this.barComponentFunc(sample));
393:   }
394: 
395:   /** Updates an indicator given the next quote sample. */
396:   public updateQuote(sample: Quote): IndicatorOutput {
397:     return this.updateEntity(sample.time, this.quoteComponentFunc(sample));
398:   }
399: 
400:   /** Updates an indicator given the next trade sample. */
401:   public updateTrade(sample: Trade): IndicatorOutput {
402:     return this.updateEntity(sample.time, this.tradeComponentFunc(sample));
403:   }
404: 
405:   private updateEntity(time: Date, sample: number): IndicatorOutput {
406:     const [value, trend, cycle, itl, sine, sineLead, period, phase] = this.update(sample);
407: 
408:     const out: Scalar[] = [];
409:     for (const v of [value, trend, cycle, itl, sine, sineLead, period, phase]) {
410:       const s = new Scalar();
411:       s.time = time;
412:       s.value = v;
413:       out.push(s);
414:     }
415:     return out;
416:   }
417: 
418:   private pushInput(value: number): void {
419:     for (let i = this.inputLengthMin1; i > 0; i--) {
420:       this.input[i] = this.input[i - 1];
421:     }
422:     this.input[0] = value;
423:   }
424: 
425:   private calculateTrendAverage(smoothedPeriod: number): number {
426:     let length = Math.floor(smoothedPeriod * this.cyclePartMultiplier + 0.5);
427:     if (length > this.inputLength) {
428:       length = this.inputLength;
429:     } else if (length < 1) {
430:       length = 1;
431:     }
432: 
433:     let sum = 0;
434:     for (let i = 0; i < length; i++) {
435:       sum += this.input[i];
436:     }
437:     return sum / length;
438:   }
439: 
440:   private calculateSineLeadWaveDifference(phase: number): number {
441:     const p = phase * DEG2RAD;
442:     this.sinWave = Math.sin(p);
443:     this.sinWaveLead = Math.sin(p + LEAD_OFFSET * DEG2RAD);
444:     return this.sinWave - this.sinWaveLead;
445:   }
446: 
447:   private mode(): number { return this.isTrendMode ? 1.0 : -1.0; }
448:   private isTrendFloat(): number { return this.isTrendMode ? 1.0 : 0.0; }
449:   private isCycleFloat(): number { return this.isTrendMode ? 0.0 : 1.0; }
450: }

(End of file - total 450 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.1s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-params.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-params.ts</path>
<type>file</type>
<content>1: import { BarComponent } from '../../../entities/bar-component';
2: import { QuoteComponent } from '../../../entities/quote-component';
3: import { TradeComponent } from '../../../entities/trade-component';
4: import { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';
5: import { HilbertTransformerCycleEstimatorParams } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-params';
6: 
7: /** Describes parameters to create an instance of the TrendCycleMode indicator. */
8: export interface TrendCycleModeParams {
9:   /** The type of cycle estimator to use.
10:    *
11:    * The default value is HilbertTransformerCycleEstimatorType.HomodyneDiscriminator.
12:    */
13:   estimatorType?: HilbertTransformerCycleEstimatorType;
14: 
15:   /** Parameters to create an instance of the Hilbert transformer cycle estimator. */
16:   estimatorParams?: HilbertTransformerCycleEstimatorParams;
17: 
18:   /** The value of α (0 < α ≤ 1) used in EMA for additional smoothing of the instantaneous period.
19:    *
20:    * The default value is 0.33.
21:    */
22:   alphaEmaPeriodAdditional: number;
23: 
24:   /** The additional WMA smoothing length used to smooth the trend line.
25:    *
26:    * The valid values are 2, 3, 4. The default value is 4.
27:    */
28:   trendLineSmoothingLength?: number;
29: 
30:   /** The multiplier to the dominant cycle period used to determine the window length to
31:    * calculate the trend line. Typical values are in [0.5, 1.5].
32:    *
33:    * The default value is 1.0. Valid range is (0, 10].
34:    */
35:   cyclePartMultiplier?: number;
36: 
37:   /** The threshold (in percent) above which a wide separation between the WMA-smoothed
38:    * price and the instantaneous trend line forces the trend mode.
39:    *
40:    * The default value is 1.5. Valid range is (0, 100].
41:    */
42:   separationPercentage?: number;
43: 
44:   /** A component of a bar to use when updating the indicator with a bar sample.
45:    *
46:    * If _undefined_, the default (BarComponent.Median, hl/2) is used. Since the default
47:    * differs from the framework default bar component, it is always shown in the mnemonic.
48:    */
49:   barComponent?: BarComponent;
50: 
51:   /** A component of a quote to use when updating the indicator with a quote sample.
52:    *
53:    * If _undefined_, a default value is used and the component is not shown in the indicator mnemonic.
54:    */
55:   quoteComponent?: QuoteComponent;
56: 
57:   /** A component of a trade to use when updating the indicator with a trade sample.
58:    *
59:    * If _undefined_, a default value is used and the component is not shown in the indicator mnemonic.
60:    */
61:   tradeComponent?: TradeComponent;
62: }

(End of file - total 62 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.5s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-output.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-output.ts</path>
<type>file</type>
<content>1: /** Enumerates outputs of the TrendCycleMode indicator. */
2: export enum TrendCycleModeOutput {
3:   /** +1 in trend mode, -1 in cycle mode. */
4:   Value = 0,
5:   /** 1 if the trend mode is declared, 0 otherwise. */
6:   IsTrendMode = 1,
7:   /** 1 if the cycle mode is declared, 0 otherwise (= 1 − IsTrendMode). */
8:   IsCycleMode = 2,
9:   /** The WMA-smoothed instantaneous trend line. */
10:   InstantaneousTrendLine = 3,
11:   /** The sine wave value, sin(phase·Deg2Rad). */
12:   SineWave = 4,
13:   /** The sine wave lead value, sin((phase+45)·Deg2Rad). */
14:   SineWaveLead = 5,
15:   /** The smoothed dominant cycle period. */
16:   DominantCyclePeriod = 6,
17:   /** The dominant cycle phase, in degrees. */
18:   DominantCyclePhase = 7,
19: }

(End of file - total 19 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.5s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts",
  "limit": 80
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts</path>
<type>file</type>
<content>1: import { } from 'jasmine';
2: 
3: import { Bar } from '../../../entities/bar';
4: import { Quote } from '../../../entities/quote';
5: import { Scalar } from '../../../entities/scalar';
6: import { Trade } from '../../../entities/trade';
7: import { IndicatorType } from '../../core/indicator-type';
8: import { OutputType } from '../../core/outputs/output-type';
9: import { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';
10: import { TrendCycleMode } from './trend-cycle-mode';
11: import { TrendCycleModeOutput } from './trend-cycle-mode-output';
12: 
13: /* eslint-disable max-len */
14: // Input from MBST TrendCycleModeTest.cs (TA-Lib test_MAMA.xsl, Price). 252 entries.
15: const input = [
16:   92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,
17:   94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,
18:   88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,
19:   85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,
20:   83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,
21:   89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,
22:   89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,
23:   88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,
24:   103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,
25:   120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,
26:   114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,
27:   114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,
28:   124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,
29:   137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,
30:   123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,
31:   122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,
32:   123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,
33:   130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,
34:   127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,
35:   121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,
36:   106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,
37:   95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,
38:   94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,
39:   103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,
40:   106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,
41:   109.5300, 108.0600,
42: ];
43: 
44: const expectedPeriod = [
45:   0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
46:   0, 0, 0.39600000000000, 0.97812000000000, 1.62158040000000, 2.25545086800000, 2.84234568156000, 3.36868456664520, 3.86776291565229, 4.36321983508703,
47:   4.87235783926831, 5.40838035704577, 5.98190550443027, 6.60199641969884, 7.27686930610184, 8.01438731048222, 8.82241286095647, 9.70906731606755, 10.68293087091460, 11.75320502957710,
48:   12.92985285048740, 14.22372743856440, 15.55272286275450, 16.77503611571540, 17.85814025111630, 18.72970387649220, 19.30387978646140, 19.63314544969620, 19.86256979430250, 20.09030968609300,
49:   20.24817834009410, 20.31132870798190, 20.52152604110820, 21.27119054536480, 22.10966835167300, 22.28715460952700, 21.91280773257140, 21.23923470724180, 20.70813161651310, 20.20449150221090,
50:   19.52863321263000, 18.73709250583170, 17.96311275281150, 17.33367762545960, 16.91743352044750, 16.64300564862120, 16.41952162419500, 16.27464914327850, 16.26425245094380, 16.33321577028600,
51:   16.39265551523990, 16.39976990202790, 16.38221107536320, 16.37405059271910, 16.35102942468120, 16.26839438425590, 16.12432207371240, 15.99529098667630, 15.96458956780100, 16.07977539207760,
52:   16.38360881255670, 16.79746341307210, 17.18753188776420, 17.58524022168910, 18.05888760471740, 18.46077773999830, 18.78691120238400, 19.07789869381110, 19.11803073417110, 18.72675385299730,
53:   18.07403190737810, 17.72999456892580, 18.00699920187680, 18.06680349806270, 17.73551482016550, 17.28467833183610, 16.97456900115070, 16.89386283663200, 17.00556420464730, 17.21986959021550,
54:   17.48251598471900, 17.79647268844360, 18.15809655229470, 18.56044162987590, 19.03705300462320, 19.61779465906120, 20.13838368155990, 20.58144279802850, 20.93554178712450, 21.09578565733870,
55:   21.19426268582890, 21.35270550953270, 21.46806615214910, 21.43420235778580, 21.27320458618770, 20.98617884905010, 20.62345107800030, 20.32165030848570, 20.09921951571820, 19.88214300840560,
56:   19.67081622699810, 19.55217428481160, 19.60485773311710, 19.77836095343260, 19.77886122563300, 19.59009982815140, 19.54609435364200, 19.77945658439880, 20.20526697824140, 20.80572859375930,
57:   21.45882440191380, 21.50916115262280, 21.07219135457730, 20.33979206665380, 19.60807769029340, 19.15831017112920, 19.03006205140340, 19.23359250887840, 19.84206353515510, 20.83692898803630,
58:   22.25776348341490, 23.50933063567320, 24.02857349775940, 24.28548086650010, 24.74576845262060, 25.45685387492870, 26.31998583396390, 27.14553013410700, 27.80677101851790, 28.50146147525040,
59:   29.16835938704370, 29.38723525724370, 29.55886298198770, 30.43981360336700, 30.70779370313880, 30.20667454311960, 29.23518282361370, 28.00037502954910, 26.95505291681000, 26.22399862702800,
60:   25.67716809996900, 25.19893752937060, 24.76924271120940, 24.40654607774420, 24.12997738279210, 24.02648590415090, 24.17912316847620, 24.26552607123530, 24.07565548132200, 23.81050493977940,
61:   23.75771490624360, 24.01627030476950, 24.42884190933990, 24.55867905189440, 24.41978729840000, 24.33536819272640, 24.11887925396970, 23.53741527509780, 22.66734716257270, 21.70419061052260,
62:   20.78848949032480, 19.92593130809770, 19.09528115584620, 18.35405205698280, 17.81539769318840, 17.53491540732180, 17.59552736216070, 18.09376127214910, 18.69300796204700, 19.10361709066390,
63:   19.40368660687600, 19.79324964337850, 20.14261316711870, 20.30292592814370, 20.37642955508450, 20.42856373321320, 20.36417897031590, 20.26870923265670, 20.31691792510900, 20.52593924664440,
64:   20.95797078839970, 21.70315565998060, 22.68588957914270, 23.95566588814480, 25.30036991408680, 26.49222048853470, 28.24485763802440, 30.46863151925310, 31.19661794415910, 30.97271495031300,
65:   30.15801520320610, 29.52193986806340, 28.48090879451130, 27.20913817575940, 25.84740758865390, 24.75875079095690, 24.57820671512040, 25.25622655282780, 26.58938264946150, 28.44936832011270,
66:   30.75900691394640, 31.63120735338530, 31.95156902113430, 32.19329221743080, 32.18129930292270, 31.78927079951340, 30.94427836437330, 29.74153261553520, 28.44319750131350, 27.27756983469050,
67:   26.30928991862760, 25.59706087830910, 25.19354035279110, 24.98183319418390, 24.66611779383150, 24.13629363553260, 23.59372342374540, 23.45943359521940, 24.13462330023790, 25.42868068174450,
68:   27.22154743441240, 28.85990121754770, 29.25658159944000, 28.86760790158470, 28.27077502042400, 27.83957963686970, 27.56292753489200, 27.31665028261770, 27.11537844471070, 27.05619511102920,
69:   26.72669604084850, 25.93839467294110, 24.88015320695530, 23.98089561843900, 23.51115215671300, 23.02173482203020, 22.29674643126940, 21.42162141795630, 20.54863761751100, 19.78167187971360,
70:   19.14387361712880, 18.61396641752300,
71: ];
72: 
73: const expectedPhase = [
74:   Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN,
75:   Number.NaN, Number.NaN, Number.NaN,
76:   639.09090909090900, 98.05299963194700, 132.00564338345400, 69.61331949528320, 72.53929158292410, 49.31988383698000, 70.57064052204580, 95.44397344939550, 15.53098102018940, -7.59977335936253,
77:   -0.43908926482948, 0.75159304711349, 6.81682854281200, 10.38041464097730, 14.60138862232650, 23.61952231391600, 34.25173964222910, 45.61939612889610, 54.93203281582900, 62.34730431858280,
78:   59.09332873285360, 64.74787861710400, 71.38960942346380, 78.26591034671290, 95.01873223899610, 100.81260420916600, 122.15965196914300, 155.92351856084900, 203.60327223472200, 237.36293513970900,
79:   244.38539212971100, 260.58654095568100, 254.66267427143100, 253.71116813655900, 126.51252837026600, 108.91084071926300, 120.62704116849900, 135.00432720483700, 148.37778407551400, 166.73764548652900,
80:   192.70016208445300, 227.19271648131900, 250.93069421530800, 272.91238631233800, 306.14557952390500, -30.56440912946640, -7.29779366562593, 15.79435681229920, 37.27143784018100, 57.28482204365770,

(Showing lines 1-80 of 473. Use offset=81 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.0s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts",
  "offset": 230
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts</path>
<type>file</type>
<content>230:   });
231: 
232:   it('should return expected mnemonic for default params', () => {
233:     const x = TrendCycleMode.default();
234:     expect(x.metadata().mnemonic).toBe('tcm(0.330, 4, 1.000, 1.500%, hl/2)');
235:   });
236: 
237:   it('should return expected mnemonic for phase accumulator, tlsl=3', () => {
238:     const x = TrendCycleMode.fromParams({
239:       alphaEmaPeriodAdditional: 0.5,
240:       estimatorType: HilbertTransformerCycleEstimatorType.PhaseAccumulator,
241:       estimatorParams: { smoothingLength: 4, alphaEmaQuadratureInPhase: 0.2, alphaEmaPeriod: 0.2 },
242:       trendLineSmoothingLength: 3,
243:       cyclePartMultiplier: 0.5,
244:       separationPercentage: 2.0,
245:     });
246:     expect(x.metadata().mnemonic).toBe('tcm(0.500, 3, 0.500, 2.000%, pa(4, 0.200, 0.200), hl/2)');
247:   });
248: 
249:   it('should return expected metadata', () => {
250:     const x = TrendCycleMode.default();
251:     const meta = x.metadata();
252:     const mn = 'tcm(0.330, 4, 1.000, 1.500%, hl/2)';
253:     const mnTrend = 'tcm-trend(0.330, 4, 1.000, 1.500%, hl/2)';
254:     const mnCycle = 'tcm-cycle(0.330, 4, 1.000, 1.500%, hl/2)';
255:     const mnITL = 'tcm-itl(0.330, 4, 1.000, 1.500%, hl/2)';
256:     const mnSine = 'tcm-sine(0.330, 4, 1.000, 1.500%, hl/2)';
257:     const mnSineLead = 'tcm-sineLead(0.330, 4, 1.000, 1.500%, hl/2)';
258:     const mnDCP = 'dcp(0.330, hl/2)';
259:     const mnDCPha = 'dcph(0.330, hl/2)';
260: 
261:     expect(meta.type).toBe(IndicatorType.TrendCycleMode);
262:     expect(meta.mnemonic).toBe(mn);
263:     expect(meta.description).toBe('Trend versus cycle mode ' + mn);
264:     expect(meta.outputs.length).toBe(8);
265: 
266:     expect(meta.outputs[0].kind).toBe(TrendCycleModeOutput.Value);
267:     expect(meta.outputs[0].type).toBe(OutputType.Scalar);
268:     expect(meta.outputs[0].mnemonic).toBe(mn);
269:     expect(meta.outputs[0].description).toBe('Trend versus cycle mode ' + mn);
270: 
271:     expect(meta.outputs[1].kind).toBe(TrendCycleModeOutput.IsTrendMode);
272:     expect(meta.outputs[1].mnemonic).toBe(mnTrend);
273:     expect(meta.outputs[1].description).toBe('Trend versus cycle mode, is-trend flag ' + mnTrend);
274: 
275:     expect(meta.outputs[2].kind).toBe(TrendCycleModeOutput.IsCycleMode);
276:     expect(meta.outputs[2].mnemonic).toBe(mnCycle);
277:     expect(meta.outputs[2].description).toBe('Trend versus cycle mode, is-cycle flag ' + mnCycle);
278: 
279:     expect(meta.outputs[3].kind).toBe(TrendCycleModeOutput.InstantaneousTrendLine);
280:     expect(meta.outputs[3].mnemonic).toBe(mnITL);
281: 
282:     expect(meta.outputs[4].kind).toBe(TrendCycleModeOutput.SineWave);
283:     expect(meta.outputs[4].mnemonic).toBe(mnSine);
284: 
285:     expect(meta.outputs[5].kind).toBe(TrendCycleModeOutput.SineWaveLead);
286:     expect(meta.outputs[5].mnemonic).toBe(mnSineLead);
287: 
288:     expect(meta.outputs[6].kind).toBe(TrendCycleModeOutput.DominantCyclePeriod);
289:     expect(meta.outputs[6].mnemonic).toBe(mnDCP);
290:     expect(meta.outputs[6].description).toBe('Dominant cycle period ' + mnDCP);
291: 
292:     expect(meta.outputs[7].kind).toBe(TrendCycleModeOutput.DominantCyclePhase);
293:     expect(meta.outputs[7].mnemonic).toBe(mnDCPha);
294:     expect(meta.outputs[7].description).toBe('Dominant cycle phase ' + mnDCPha);
295:   });
296: 
297:   it('should throw if α is out of range (0, 1]', () => {
298:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0 })).toThrowError(/α for additional smoothing/);
299:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: -0.1 })).toThrowError(/α for additional smoothing/);
300:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 1.0001 })).toThrowError(/α for additional smoothing/);
301:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.5 })).not.toThrow();
302:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 1.0 })).not.toThrow();
303:   });
304: 
305:   it('should throw if trend line smoothing length is not 2, 3, or 4', () => {
306:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 1 }))
307:       .toThrowError(/trend line smoothing length/);
308:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 5 }))
309:       .toThrowError(/trend line smoothing length/);
310:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 2 })).not.toThrow();
311:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 3 })).not.toThrow();
312:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 4 })).not.toThrow();
313:   });
314: 
315:   it('should throw if cycle part multiplier is out of range (0, 10]', () => {
316:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, cyclePartMultiplier: 0 }))
317:       .toThrowError(/cycle part multiplier/);
318:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, cyclePartMultiplier: 10.00001 }))
319:       .toThrowError(/cycle part multiplier/);
320:   });
321: 
322:   it('should throw if separation percentage is out of range (0, 100]', () => {
323:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, separationPercentage: 0 }))
324:       .toThrowError(/separation percentage/);
325:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, separationPercentage: 100.00001 }))
326:       .toThrowError(/separation percentage/);
327:   });
328: 
329:   it('should return NaN 8-tuple for NaN input', () => {
330:     const x = TrendCycleMode.default();
331:     const out = x.update(Number.NaN);
332:     expect(out.length).toBe(8);
333:     for (const v of out) {
334:       expect(Number.isNaN(v)).toBe(true);
335:     }
336:   });
337: 
338:   it('should match reference period past settle window', () => {
339:     const x = TrendCycleMode.default();
340:     for (let i = skip; i < input.length; i++) {
341:       const [, , , , , , period] = x.update(input[i]);
342:       if (Number.isNaN(period) || i < settleSkip) continue;
343:       expect(Math.abs(expectedPeriod[i] - period))
344:         .withContext(`period[${i}]: expected ${expectedPeriod[i]}, actual ${period}`)
345:         .toBeLessThan(tolerance);
346:     }
347:   });
348: 
349:   it('should match reference phase (mod 360) past settle window', () => {
350:     const x = TrendCycleMode.default();
351:     for (let i = skip; i < input.length; i++) {
352:       const [, , , , , , , phase] = x.update(input[i]);
353:       if (Number.isNaN(phase) || Number.isNaN(expectedPhase[i]) || i < settleSkip) continue;
354:       const d = phaseDelta(expectedPhase[i], phase);
355:       expect(Math.abs(d))
356:         .withContext(`phase[${i}]: expected ${expectedPhase[i]}, actual ${phase}`)
357:         .toBeLessThan(tolerance);
358:     }
359:   });
360: 
361:   it('should match reference sine wave past settle window', () => {
362:     const x = TrendCycleMode.default();
363:     for (let i = skip; i < input.length; i++) {
364:       const [, , , , sine] = x.update(input[i]);
365:       if (Number.isNaN(sine) || Number.isNaN(expectedSine[i]) || i < settleSkip) continue;
366:       expect(Math.abs(expectedSine[i] - sine))
367:         .withContext(`sine[${i}]: expected ${expectedSine[i]}, actual ${sine}`)
368:         .toBeLessThan(tolerance);
369:     }
370:   });
371: 
372:   it('should match reference sine wave lead past settle window', () => {
373:     const x = TrendCycleMode.default();
374:     for (let i = skip; i < input.length; i++) {
375:       const [, , , , , sineLead] = x.update(input[i]);
376:       if (Number.isNaN(sineLead) || Number.isNaN(expectedSineLead[i]) || i < settleSkip) continue;
377:       expect(Math.abs(expectedSineLead[i] - sineLead))
378:         .withContext(`sineLead[${i}]: expected ${expectedSineLead[i]}, actual ${sineLead}`)
379:         .toBeLessThan(tolerance);
380:     }
381:   });
382: 
383:   it('should match reference instantaneous trend line past settle window', () => {
384:     const x = TrendCycleMode.default();
385:     for (let i = skip; i < input.length; i++) {
386:       const [, , , itl] = x.update(input[i]);
387:       if (Number.isNaN(itl) || Number.isNaN(expectedITL[i]) || i < settleSkip) continue;
388:       expect(Math.abs(expectedITL[i] - itl))
389:         .withContext(`itl[${i}]: expected ${expectedITL[i]}, actual ${itl}`)
390:         .toBeLessThan(tolerance);
391:     }
392:   });
393: 
394:   it('should match reference tcm value (except MBST known mismatches)', () => {
395:     const x = TrendCycleMode.default();
396:     const limit = expectedValue.length;
397:     for (let i = skip; i < input.length; i++) {
398:       const [value] = x.update(input[i]);
399:       if (i >= limit) continue;
400:       // MBST known mismatches.
401:       if (i === 70 || i === 71) continue;
402:       if (Number.isNaN(value) || Number.isNaN(expectedValue[i])) continue;
403:       expect(Math.abs(expectedValue[i] - value))
404:         .withContext(`value[${i}]: expected ${expectedValue[i]}, actual ${value}`)
405:         .toBeLessThan(tolerance);
406:     }
407:   });
408: 
409:   it('should have complementary is-trend / is-cycle flags (0/1) aligned with value sign', () => {
410:     const x = TrendCycleMode.default();
411:     for (let i = skip; i < input.length; i++) {
412:       const [value, trend, cycle] = x.update(input[i]);
413:       if (Number.isNaN(value)) continue;
414:       expect(trend + cycle).toBe(1);
415:       if (value > 0) expect(trend).toBe(1);
416:       if (value < 0) expect(trend).toBe(0);
417:     }
418:   });
419: 
420:   it('should become primed within the input sequence', () => {
421:     const x = TrendCycleMode.default();
422:     expect(x.isPrimed()).toBe(false);
423: 
424:     let primedAt = -1;
425:     for (let i = 0; i < input.length; i++) {
426:       x.update(input[i]);
427:       if (x.isPrimed() && primedAt < 0) primedAt = i;
428:     }
429:     expect(primedAt).toBeGreaterThanOrEqual(0);
430:     expect(x.isPrimed()).toBe(true);
431:   });
432: 
433:   const primeCount = 200;
434:   const value = 100.0;
435: 
436:   function checkOutput(out: any[]): void {
437:     expect(out.length).toBe(8);
438:     for (let i = 0; i < 8; i++) {
439:       const s = out[i] as Scalar;
440:       expect(s.time).toEqual(time);
441:     }
442:   }
443: 
444:   it('should produce 8-element scalar output via updateScalar', () => {
445:     const x = TrendCycleMode.default();
446:     for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);
447:     const s = new Scalar();
448:     s.time = time;
449:     s.value = value;
450:     checkOutput(x.updateScalar(s) as any[]);
451:   });
452: 
453:   it('should produce 8-element scalar output via updateBar', () => {
454:     const x = TrendCycleMode.default();
455:     for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);
456:     const bar = new Bar({ time, open: value, high: value, low: value, close: value, volume: 0 });
457:     checkOutput(x.updateBar(bar) as any[]);
458:   });
459: 
460:   it('should produce 8-element scalar output via updateQuote', () => {
461:     const x = TrendCycleMode.default();
462:     for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);
463:     const q = new Quote({ time, bid: value, ask: value, bidSize: 0, askSize: 0 });
464:     checkOutput(x.updateQuote(q) as any[]);
465:   });
466: 
467:   it('should produce 8-element scalar output via updateTrade', () => {
468:     const x = TrendCycleMode.default();
469:     for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);
470:     const t = new Trade({ time, price: value, volume: 0 });
471:     checkOutput(x.updateTrade(t) as any[]);
472:   });
473: });

(End of file - total 473 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 6.5s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts",
  "offset": 205,
  "limit": 32
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts</path>
<type>file</type>
<content>205: 
206: const tolerance = 1e-4;
207: const skip = 9;
208: const settleSkip = 177;
209: 
210: // MBST wraps phase into (-180, 180]; zpano into [0, 360). Compare modulo 360.
211: function phaseDelta(expected: number, actual: number): number {
212:   let d = (expected - actual) % 360;
213:   if (d > 180) d -= 360;
214:   else if (d < -180) d += 360;
215:   return d;
216: }
217: 
218: describe('TrendCycleMode', () => {
219:   const time = new Date(2021, 3, 1);
220: 
221:   it('should have correct output enum values', () => {
222:     expect(TrendCycleModeOutput.Value).toBe(0);
223:     expect(TrendCycleModeOutput.IsTrendMode).toBe(1);
224:     expect(TrendCycleModeOutput.IsCycleMode).toBe(2);
225:     expect(TrendCycleModeOutput.InstantaneousTrendLine).toBe(3);
226:     expect(TrendCycleModeOutput.SineWave).toBe(4);
227:     expect(TrendCycleModeOutput.SineWaveLead).toBe(5);
228:     expect(TrendCycleModeOutput.DominantCyclePeriod).toBe(6);
229:     expect(TrendCycleModeOutput.DominantCyclePhase).toBe(7);
230:   });
231: 
232:   it('should return expected mnemonic for default params', () => {
233:     const x = TrendCycleMode.default();
234:     expect(x.metadata().mnemonic).toBe('tcm(0.330, 4, 1.000, 1.500%, hl/2)');
235:   });
236: 

(Showing lines 205-236 of 473. Use offset=237 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 6.6s)

_Thinking:_

 I need to check the Go tests for CSpectrum and CSNR to understand what parameters and mnemonic strings they expect, then look at the relevant Go packages to see how they're implemented.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaspectrum</path>
<type>directory</type>
<entries>
coronaspectrum_test.go
coronaspectrum.go
coronaspectrumoutput_test.go
coronaspectrumoutput.go
coronaspectrumparams.go

(5 entries)
</entries>
```

---

## Assistant (Build · claude-opus-4.7 · 3.4s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrum.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrum.go</path>
<type>file</type>
<content>1: // Package coronaspectrum implements Ehlers' Corona Spectrum heatmap indicator.
2: //
3: // The Corona Spectrum measures cyclic activity over a cycle period range
4: // (default 6..30 bars) in a bank of contiguous bandpass filters. The amplitude
5: // of each filter output is compared to the strongest signal and displayed, in
6: // decibels, as a heatmap column. The filter having the strongest output is
7: // selected as the current dominant cycle period.
8: //
9: // Reference: John Ehlers, "Measuring Cycle Periods", Stocks & Commodities,
10: // November 2008.
11: package coronaspectrum
12: 
13: //nolint: gofumpt
14: import (
15: 	"fmt"
16: 	"math"
17: 	"sync"
18: 	"time"
19: 
20: 	"zpano/entities"
21: 	"zpano/indicators/core"
22: 	"zpano/indicators/core/outputs"
23: 	"zpano/indicators/johnehlers/corona"
24: )
25: 
26: // CoronaSpectrum is Ehlers' Corona Spectrum heatmap indicator.
27: //
28: // It owns a private Corona spectral-analysis engine and exposes three outputs:
29: //
30: //   - Value: a per-bar heatmap column (decibels across the filter bank).
31: //   - DominantCycle: the weighted-center-of-gravity dominant cycle estimate.
32: //   - DominantCycleMedian: the 5-sample median of DominantCycle.
33: type CoronaSpectrum struct {
34: 	mu                  sync.RWMutex
35: 	mnemonic            string
36: 	description         string
37: 	mnemonicDC          string
38: 	descriptionDC       string
39: 	mnemonicDCM         string
40: 	descriptionDCM      string
41: 	c                   *corona.Corona
42: 	minParameterValue   float64
43: 	maxParameterValue   float64
44: 	parameterResolution float64
45: 	minRasterValue      float64
46: 	maxRasterValue      float64
47: 	barFunc             entities.BarFunc
48: 	quoteFunc           entities.QuoteFunc
49: 	tradeFunc           entities.TradeFunc
50: }
51: 
52: // NewCoronaSpectrumDefault returns an instance created with default parameters.
53: func NewCoronaSpectrumDefault() (*CoronaSpectrum, error) {
54: 	return NewCoronaSpectrumParams(&Params{})
55: }
56: 
57: // NewCoronaSpectrumParams returns an instance created with the supplied parameters.
58: //
59: //nolint:funlen,cyclop
60: func NewCoronaSpectrumParams(p *Params) (*CoronaSpectrum, error) {
61: 	const (
62: 		invalid      = "invalid corona spectrum parameters"
63: 		fmtMinRaster = "%s: MinRasterValue should be >= 0"
64: 		fmtMaxRaster = "%s: MaxRasterValue should be > MinRasterValue"
65: 		fmtMinParam  = "%s: MinParameterValue should be >= 2"
66: 		fmtMaxParam  = "%s: MaxParameterValue should be > MinParameterValue"
67: 		fmtHP        = "%s: HighPassFilterCutoff should be >= 2"
68: 		fmtw         = "%s: %w"
69: 		fmtnVal      = "cspect(%g, %g, %g, %g, %d%s)"
70: 		fmtnDC       = "cspect-dc(%d%s)"
71: 		fmtnDCM      = "cspect-dcm(%d%s)"
72: 		descrVal     = "Corona spectrum "
73: 		descrDC      = "Corona spectrum dominant cycle "
74: 		descrDCM     = "Corona spectrum dominant cycle median "
75: 		defMinRaster = 6.0
76: 		defMaxRaster = 20.0
77: 		defMinParam  = 6.0
78: 		defMaxParam  = 30.0
79: 		defHPCutoff  = 30
80: 	)
81: 
82: 	cfg := *p
83: 
84: 	if cfg.MinRasterValue == 0 {
85: 		cfg.MinRasterValue = defMinRaster
86: 	}
87: 
88: 	if cfg.MaxRasterValue == 0 {
89: 		cfg.MaxRasterValue = defMaxRaster
90: 	}
91: 
92: 	if cfg.MinParameterValue == 0 {
93: 		cfg.MinParameterValue = defMinParam
94: 	}
95: 
96: 	if cfg.MaxParameterValue == 0 {
97: 		cfg.MaxParameterValue = defMaxParam
98: 	}
99: 
100: 	if cfg.HighPassFilterCutoff == 0 {
101: 		cfg.HighPassFilterCutoff = defHPCutoff
102: 	}
103: 
104: 	if cfg.MinRasterValue < 0 {
105: 		return nil, fmt.Errorf(fmtMinRaster, invalid)
106: 	}
107: 
108: 	if cfg.MaxRasterValue <= cfg.MinRasterValue {
109: 		return nil, fmt.Errorf(fmtMaxRaster, invalid)
110: 	}
111: 
112: 	// MBST rounds min up and max down to integers.
113: 	minParam := math.Ceil(cfg.MinParameterValue)
114: 	maxParam := math.Floor(cfg.MaxParameterValue)
115: 
116: 	if minParam < 2 {
117: 		return nil, fmt.Errorf(fmtMinParam, invalid)
118: 	}
119: 
120: 	if maxParam <= minParam {
121: 		return nil, fmt.Errorf(fmtMaxParam, invalid)
122: 	}
123: 
124: 	if cfg.HighPassFilterCutoff < 2 {
125: 		return nil, fmt.Errorf(fmtHP, invalid)
126: 	}
127: 
128: 	// CoronaSpectrum mirrors Ehlers' reference: BarMedianPrice default.
129: 	bc := cfg.BarComponent
130: 	if bc == 0 {
131: 		bc = entities.BarMedianPrice
132: 	}
133: 
134: 	qc := cfg.QuoteComponent
135: 	if qc == 0 {
136: 		qc = entities.DefaultQuoteComponent
137: 	}
138: 
139: 	tc := cfg.TradeComponent
140: 	if tc == 0 {
141: 		tc = entities.DefaultTradeComponent
142: 	}
143: 
144: 	c, err := corona.NewCorona(&corona.Params{
145: 		HighPassFilterCutoff:   cfg.HighPassFilterCutoff,
146: 		MinimalPeriod:          int(minParam),
147: 		MaximalPeriod:          int(maxParam),
148: 		DecibelsLowerThreshold: cfg.MinRasterValue,
149: 		DecibelsUpperThreshold: cfg.MaxRasterValue,
150: 	})
151: 	if err != nil {
152: 		return nil, fmt.Errorf(fmtw, invalid, err)
153: 	}
154: 
155: 	barFunc, err := entities.BarComponentFunc(bc)
156: 	if err != nil {
157: 		return nil, fmt.Errorf(fmtw, invalid, err)
158: 	}
159: 
160: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
161: 	if err != nil {
162: 		return nil, fmt.Errorf(fmtw, invalid, err)
163: 	}
164: 
165: 	tradeFunc, err := entities.TradeComponentFunc(tc)
166: 	if err != nil {
167: 		return nil, fmt.Errorf(fmtw, invalid, err)
168: 	}
169: 
170: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
171: 
172: 	mnemonicVal := fmt.Sprintf(fmtnVal,
173: 		cfg.MinRasterValue, cfg.MaxRasterValue, minParam, maxParam, cfg.HighPassFilterCutoff, componentMnemonic)
174: 	mnemonicDC := fmt.Sprintf(fmtnDC, cfg.HighPassFilterCutoff, componentMnemonic)
175: 	mnemonicDCM := fmt.Sprintf(fmtnDCM, cfg.HighPassFilterCutoff, componentMnemonic)
176: 
177: 	// Parameter resolution: 2 samples per period (half-period stepping).
178: 	// Values slice length = (maxParam*2 - minParam*2 + 1). The first sample sits
179: 	// at minParam, the last at maxParam. Resolution satisfies:
180: 	//   min + (length-1)/resolution = max  =>  resolution = (length-1) / (max - min).
181: 	paramResolution := float64(c.FilterBankLength()-1) / (maxParam - minParam)
182: 
183: 	return &CoronaSpectrum{
184: 		mnemonic:            mnemonicVal,
185: 		description:         descrVal + mnemonicVal,
186: 		mnemonicDC:          mnemonicDC,
187: 		descriptionDC:       descrDC + mnemonicDC,
188: 		mnemonicDCM:         mnemonicDCM,
189: 		descriptionDCM:      descrDCM + mnemonicDCM,
190: 		c:                   c,
191: 		minParameterValue:   minParam,
192: 		maxParameterValue:   maxParam,
193: 		parameterResolution: paramResolution,
194: 		minRasterValue:      cfg.MinRasterValue,
195: 		maxRasterValue:      cfg.MaxRasterValue,
196: 		barFunc:             barFunc,
197: 		quoteFunc:           quoteFunc,
198: 		tradeFunc:           tradeFunc,
199: 	}, nil
200: }
201: 
202: // IsPrimed indicates whether the indicator is primed.
203: func (s *CoronaSpectrum) IsPrimed() bool {
204: 	s.mu.RLock()
205: 	defer s.mu.RUnlock()
206: 
207: 	return s.c.IsPrimed()
208: }
209: 
210: // Metadata describes the output data of the indicator.
211: func (s *CoronaSpectrum) Metadata() core.Metadata {
212: 	return core.Metadata{
213: 		Type:        core.CoronaSpectrum,
214: 		Mnemonic:    s.mnemonic,
215: 		Description: s.description,
216: 		Outputs: []outputs.Metadata{
217: 			{Kind: int(Value), Type: outputs.HeatmapType, Mnemonic: s.mnemonic, Description: s.description},
218: 			{Kind: int(DominantCycle), Type: outputs.ScalarType, Mnemonic: s.mnemonicDC, Description: s.descriptionDC},
219: 			{Kind: int(DominantCycleMedian), Type: outputs.ScalarType, Mnemonic: s.mnemonicDCM, Description: s.descriptionDCM},
220: 		},
221: 	}
222: }
223: 
224: // Update feeds the next sample to the engine and returns the heatmap column
225: // plus the current DominantCycle and DominantCycleMedian estimates.
226: //
227: // On unprimed bars the heatmap is an empty heatmap (with the indicator's
228: // parameter axis) and both scalar values are NaN. On a NaN input sample,
229: // state is left unchanged and all outputs are NaN / empty heatmap.
230: func (s *CoronaSpectrum) Update(sample float64, t time.Time) (*outputs.Heatmap, float64, float64) {
231: 	s.mu.Lock()
232: 	defer s.mu.Unlock()
233: 
234: 	if math.IsNaN(sample) {
235: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution),
236: 			math.NaN(), math.NaN()
237: 	}
238: 
239: 	primed := s.c.Update(sample)
240: 	if !primed {
241: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution),
242: 			math.NaN(), math.NaN()
243: 	}
244: 
245: 	bank := s.c.FilterBank()
246: 	values := make([]float64, len(bank))
247: 	valueMin := math.Inf(1)
248: 	valueMax := math.Inf(-1)
249: 
250: 	for i := range bank {
251: 		v := bank[i].Decibels
252: 		values[i] = v
253: 
254: 		if v < valueMin {
255: 			valueMin = v
256: 		}
257: 
258: 		if v > valueMax {
259: 			valueMax = v
260: 		}
261: 	}
262: 
263: 	heatmap := outputs.NewHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution,
264: 		valueMin, valueMax, values)
265: 
266: 	return heatmap, s.c.DominantCycle(), s.c.DominantCycleMedian()
267: }
268: 
269: // UpdateScalar updates the indicator given the next scalar sample.
270: func (s *CoronaSpectrum) UpdateScalar(sample *entities.Scalar) core.Output {
271: 	return s.updateEntity(sample.Time, sample.Value)
272: }
273: 
274: // UpdateBar updates the indicator given the next bar sample.
275: func (s *CoronaSpectrum) UpdateBar(sample *entities.Bar) core.Output {
276: 	return s.updateEntity(sample.Time, s.barFunc(sample))
277: }
278: 
279: // UpdateQuote updates the indicator given the next quote sample.
280: func (s *CoronaSpectrum) UpdateQuote(sample *entities.Quote) core.Output {
281: 	return s.updateEntity(sample.Time, s.quoteFunc(sample))
282: }
283: 
284: // UpdateTrade updates the indicator given the next trade sample.
285: func (s *CoronaSpectrum) UpdateTrade(sample *entities.Trade) core.Output {
286: 	return s.updateEntity(sample.Time, s.tradeFunc(sample))
287: }
288: 
289: func (s *CoronaSpectrum) updateEntity(t time.Time, sample float64) core.Output {
290: 	const length = 3
291: 
292: 	heatmap, dc, dcm := s.Update(sample, t)
293: 
294: 	output := make([]any, length)
295: 	output[0] = heatmap
296: 	output[1] = entities.Scalar{Time: t, Value: dc}
297: 	output[2] = entities.Scalar{Time: t, Value: dcm}
298: 
299: 	return output
300: }

(End of file - total 300 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrumparams.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrumparams.go</path>
<type>file</type>
<content>1: package coronaspectrum
2: 
3: import "zpano/entities"
4: 
5: // Params describes parameters to create an instance of the CoronaSpectrum indicator.
6: type Params struct {
7: 	// MinRasterValue is the minimal raster value (z) of the heatmap, in decibels.
8: 	// Corresponds to the CoronaLowerDecibels threshold. The default value is 6.
9: 	MinRasterValue float64
10: 
11: 	// MaxRasterValue is the maximal raster value (z) of the heatmap, in decibels.
12: 	// Corresponds to the CoronaUpperDecibels threshold. The default value is 20.
13: 	MaxRasterValue float64
14: 
15: 	// MinParameterValue is the minimal ordinate (y) value of the heatmap, representing
16: 	// the minimal cycle period covered by the filter bank. The default value is 6.
17: 	MinParameterValue float64
18: 
19: 	// MaxParameterValue is the maximal ordinate (y) value of the heatmap, representing
20: 	// the maximal cycle period covered by the filter bank. The default value is 30.
21: 	MaxParameterValue float64
22: 
23: 	// HighPassFilterCutoff is the high-pass filter cutoff (de-trending period) used by
24: 	// the inner Corona engine. Suggested values are 20, 30, 100. The default value is 30.
25: 	HighPassFilterCutoff int
26: 
27: 	// BarComponent indicates the component of a bar to use when updating the indicator with a bar sample.
28: 	//
29: 	// If zero, the default (BarMedianPrice) is used, matching Ehlers' reference which operates on
30: 	// (High+Low)/2. Since this differs from the framework-wide default, it is always shown in the
31: 	// indicator mnemonic.
32: 	BarComponent entities.BarComponent
33: 
34: 	// QuoteComponent indicates the component of a quote to use when updating the indicator with a quote sample.
35: 	//
36: 	// If zero, the default (QuoteMidPrice) is used and the component is not shown in the indicator mnemonic.
37: 	QuoteComponent entities.QuoteComponent
38: 
39: 	// TradeComponent indicates the component of a trade to use when updating the indicator with a trade sample.
40: 	//
41: 	// If zero, the default (TradePrice) is used and the component is not shown in the indicator mnemonic.
42: 	TradeComponent entities.TradeComponent
43: }

(End of file - total 43 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.5s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrumoutput.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrumoutput.go</path>
<type>file</type>
<content>1: package coronaspectrum
2: 
3: import (
4: 	"bytes"
5: 	"fmt"
6: )
7: 
8: // Output describes the outputs of the indicator.
9: type Output int
10: 
11: const (
12: 	// Value is the Corona spectrum heatmap column.
13: 	Value Output = iota + 1
14: 	// DominantCycle is the weighted-center-of-gravity dominant cycle estimate.
15: 	DominantCycle
16: 	// DominantCycleMedian is the 5-sample median of DominantCycle.
17: 	DominantCycleMedian
18: 	outputLast
19: )
20: 
21: const (
22: 	valueStr               = "value"
23: 	dominantCycleStr       = "dominantCycle"
24: 	dominantCycleMedianStr = "dominantCycleMedian"
25: 	unknownStr             = "unknown"
26: )
27: 
28: // String implements the Stringer interface.
29: func (o Output) String() string {
30: 	switch o {
31: 	case Value:
32: 		return valueStr
33: 	case DominantCycle:
34: 		return dominantCycleStr
35: 	case DominantCycleMedian:
36: 		return dominantCycleMedianStr
37: 	default:
38: 		return unknownStr
39: 	}
40: }
41: 
42: // IsKnown determines if this output is known.
43: func (o Output) IsKnown() bool {
44: 	return o >= Value && o < outputLast
45: }
46: 
47: // MarshalJSON implements the Marshaler interface.
48: func (o Output) MarshalJSON() ([]byte, error) {
49: 	const (
50: 		errFmt = "cannot marshal '%s': unknown corona spectrum output"
51: 		extra  = 2   // Two bytes for quotes.
52: 		dqc    = '"' // Double quote character.
53: 	)
54: 
55: 	s := o.String()
56: 	if s == unknownStr {
57: 		return nil, fmt.Errorf(errFmt, s)
58: 	}
59: 
60: 	b := make([]byte, 0, len(s)+extra)
61: 	b = append(b, dqc)
62: 	b = append(b, s...)
63: 	b = append(b, dqc)
64: 
65: 	return b, nil
66: }
67: 
68: // UnmarshalJSON implements the Unmarshaler interface.
69: func (o *Output) UnmarshalJSON(data []byte) error {
70: 	const (
71: 		errFmt = "cannot unmarshal '%s': unknown corona spectrum output"
72: 		dqs    = "\"" // Double quote string.
73: 	)
74: 
75: 	d := bytes.Trim(data, dqs)
76: 	s := string(d)
77: 
78: 	switch s {
79: 	case valueStr:
80: 		*o = Value
81: 	case dominantCycleStr:
82: 		*o = DominantCycle
83: 	case dominantCycleMedianStr:
84: 		*o = DominantCycleMedian
85: 	default:
86: 		return fmt.Errorf(errFmt, s)
87: 	}
88: 
89: 	return nil
90: }

(End of file - total 90 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.1s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrum_test.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrum_test.go</path>
<type>file</type>
<content>1: //nolint:testpackage
2: package coronaspectrum
3: 
4: import (
5: 	"math"
6: 	"testing"
7: 	"time"
8: 
9: 	"zpano/entities"
10: 	"zpano/indicators/core"
11: 	"zpano/indicators/core/outputs"
12: )
13: 
14: func testCSTime() time.Time {
15: 	return time.Date(2021, time.April, 1, 0, 0, 0, 0, &time.Location{})
16: }
17: 
18: // testCSInput is the 252-entry TA-Lib MAMA reference series (Price D5…D256),
19: // reused here so the coronaspectrum package has no test dependency on other
20: // packages.
21: //
22: //nolint:dupl
23: func testCSInput() []float64 {
24: 	return []float64{
25: 		92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,
26: 		94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,
27: 		88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,
28: 		85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,
29: 		83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,
30: 		89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,
31: 		89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,
32: 		88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,
33: 		103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,
34: 		120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,
35: 		114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,
36: 		114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,
37: 		124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,
38: 		137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,
39: 		123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,
40: 		122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,
41: 		123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,
42: 		130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,
43: 		127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,
44: 		121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,
45: 		106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,
46: 		95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,
47: 		94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,
48: 		103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,
49: 		106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,
50: 		109.5300, 108.0600,
51: 	}
52: }
53: 
54: const testCSTolerance = 1e-4
55: 
56: func TestCoronaSpectrumUpdate(t *testing.T) {
57: 	t.Parallel()
58: 
59: 	input := testCSInput()
60: 	t0 := testCSTime()
61: 
62: 	// Snapshot values captured from a first run and locked in here.
63: 	type snap struct {
64: 		i   int
65: 		dc  float64
66: 		dcm float64
67: 	}
68: 	snapshots := []snap{
69: 		{11, 17.7604672565, 17.7604672565},
70: 		{12, 6.0000000000, 6.0000000000},
71: 		{50, 15.9989078712, 15.9989078712},
72: 		{100, 14.7455497547, 14.7455497547},
73: 		{150, 17.5000000000, 17.2826036069},
74: 		{200, 19.7557338512, 20.0000000000},
75: 		{251, 6.0000000000, 6.0000000000},
76: 	}
77: 
78: 	x, err := NewCoronaSpectrumDefault()
79: 	if err != nil {
80: 		t.Fatalf("unexpected error: %v", err)
81: 	}
82: 
83: 	si := 0
84: 	for i := range input {
85: 		h, dc, dcm := x.Update(input[i], t0.Add(time.Duration(i)*time.Minute))
86: 
87: 		if h == nil {
88: 			t.Fatalf("[%d] heatmap must not be nil", i)
89: 		}
90: 
91: 		// Heatmap axis invariants are always present.
92: 		if h.ParameterFirst != 6 || h.ParameterLast != 30 || h.ParameterResolution != 2 {
93: 			t.Errorf("[%d] heatmap axis incorrect: first=%v last=%v res=%v",
94: 				i, h.ParameterFirst, h.ParameterLast, h.ParameterResolution)
95: 		}
96: 
97: 		if !x.IsPrimed() {
98: 			if !h.IsEmpty() {
99: 				t.Errorf("[%d] expected empty heatmap before priming, got len=%d", i, len(h.Values))
100: 			}
101: 
102: 			if !math.IsNaN(dc) || !math.IsNaN(dcm) {
103: 				t.Errorf("[%d] expected NaN scalars before priming, got dc=%v dcm=%v", i, dc, dcm)
104: 			}
105: 
106: 			continue
107: 		}
108: 
109: 		if len(h.Values) != 49 {
110: 			t.Errorf("[%d] heatmap values length: expected 49, got %d", i, len(h.Values))
111: 		}
112: 
113: 		if si < len(snapshots) && snapshots[si].i == i {
114: 			if math.Abs(snapshots[si].dc-dc) > testCSTolerance {
115: 				t.Errorf("[%d] dc: expected %v, got %v", i, snapshots[si].dc, dc)
116: 			}
117: 
118: 			if math.Abs(snapshots[si].dcm-dcm) > testCSTolerance {
119: 				t.Errorf("[%d] dcm: expected %v, got %v", i, snapshots[si].dcm, dcm)
120: 			}
121: 
122: 			si++
123: 		}
124: 	}
125: 
126: 	if si != len(snapshots) {
127: 		t.Errorf("did not hit all %d snapshots, reached %d", len(snapshots), si)
128: 	}
129: }
130: 
131: func TestCoronaSpectrumPrimesAtBar11(t *testing.T) {
132: 	t.Parallel()
133: 
134: 	x, _ := NewCoronaSpectrumDefault()
135: 
136: 	if x.IsPrimed() {
137: 		t.Error("expected not primed at start")
138: 	}
139: 
140: 	input := testCSInput()
141: 	t0 := testCSTime()
142: 	primedAt := -1
143: 
144: 	for i := range input {
145: 		x.Update(input[i], t0.Add(time.Duration(i)*time.Minute))
146: 
147: 		if x.IsPrimed() && primedAt < 0 {
148: 			primedAt = i
149: 		}
150: 	}
151: 
152: 	// MinimalPeriodTimesTwo = 12, so priming first happens at sample index 11 (0-indexed).
153: 	if primedAt != 11 {
154: 		t.Errorf("expected priming at index 11, got %d", primedAt)
155: 	}
156: }
157: 
158: func TestCoronaSpectrumNaNInput(t *testing.T) {
159: 	t.Parallel()
160: 
161: 	x, _ := NewCoronaSpectrumDefault()
162: 
163: 	h, dc, dcm := x.Update(math.NaN(), testCSTime())
164: 
165: 	if h == nil || !h.IsEmpty() {
166: 		t.Errorf("expected empty heatmap for NaN input, got %v", h)
167: 	}
168: 
169: 	if !math.IsNaN(dc) || !math.IsNaN(dcm) {
170: 		t.Errorf("expected NaN scalars for NaN input, got dc=%v dcm=%v", dc, dcm)
171: 	}
172: 
173: 	// NaN must not prime the indicator.
174: 	if x.IsPrimed() {
175: 		t.Error("NaN input must not prime the indicator")
176: 	}
177: }
178: 
179: func TestCoronaSpectrumMetadata(t *testing.T) {
180: 	t.Parallel()
181: 
182: 	x, _ := NewCoronaSpectrumDefault()
183: 	md := x.Metadata()
184: 
185: 	check := func(what string, exp, act any) {
186: 		t.Helper()
187: 
188: 		if exp != act {
189: 			t.Errorf("%s: expected %v, actual %v", what, exp, act)
190: 		}
191: 	}
192: 
193: 	mnVal := "cspect(6, 20, 6, 30, 30, hl/2)"
194: 	mnDC := "cspect-dc(30, hl/2)"
195: 	mnDCM := "cspect-dcm(30, hl/2)"
196: 
197: 	check("Type", core.CoronaSpectrum, md.Type)
198: 	check("Mnemonic", mnVal, md.Mnemonic)
199: 	check("Description", "Corona spectrum "+mnVal, md.Description)
200: 	check("len(Outputs)", 3, len(md.Outputs))
201: 
202: 	check("Outputs[0].Kind", int(Value), md.Outputs[0].Kind)
203: 	check("Outputs[0].Type", outputs.HeatmapType, md.Outputs[0].Type)
204: 	check("Outputs[0].Mnemonic", mnVal, md.Outputs[0].Mnemonic)
205: 
206: 	check("Outputs[1].Kind", int(DominantCycle), md.Outputs[1].Kind)
207: 	check("Outputs[1].Type", outputs.ScalarType, md.Outputs[1].Type)
208: 	check("Outputs[1].Mnemonic", mnDC, md.Outputs[1].Mnemonic)
209: 
210: 	check("Outputs[2].Kind", int(DominantCycleMedian), md.Outputs[2].Kind)
211: 	check("Outputs[2].Type", outputs.ScalarType, md.Outputs[2].Type)
212: 	check("Outputs[2].Mnemonic", mnDCM, md.Outputs[2].Mnemonic)
213: }
214: 
215: //nolint:funlen
216: func TestCoronaSpectrumUpdateEntity(t *testing.T) {
217: 	t.Parallel()
218: 
219: 	const (
220: 		primeCount = 50
221: 		inp        = 100.
222: 		outputLen  = 3
223: 	)
224: 
225: 	tm := testCSTime()
226: 	input := testCSInput()
227: 
228: 	check := func(act core.Output) {
229: 		t.Helper()
230: 
231: 		if len(act) != outputLen {
232: 			t.Errorf("len(output): expected %v, actual %v", outputLen, len(act))
233: 
234: 			return
235: 		}
236: 
237: 		h, ok := act[0].(*outputs.Heatmap)
238: 		if !ok {
239: 			t.Errorf("output[0] is not a heatmap: %T", act[0])
240: 		} else if h.Time != tm {
241: 			t.Errorf("output[0].Time: expected %v, actual %v", tm, h.Time)
242: 		}
243: 
244: 		for i := 1; i < outputLen; i++ {
245: 			s, ok := act[i].(entities.Scalar)
246: 			if !ok {
247: 				t.Errorf("output[%d] is not a scalar", i)
248: 
249: 				continue
250: 			}
251: 
252: 			if s.Time != tm {
253: 				t.Errorf("output[%d].Time: expected %v, actual %v", i, tm, s.Time)
254: 			}
255: 		}
256: 	}
257: 
258: 	t.Run("update scalar", func(t *testing.T) {
259: 		t.Parallel()
260: 
261: 		s := entities.Scalar{Time: tm, Value: inp}
262: 		x, _ := NewCoronaSpectrumDefault()
263: 
264: 		for i := 0; i < primeCount; i++ {
265: 			x.Update(input[i%len(input)], tm)
266: 		}
267: 
268: 		check(x.UpdateScalar(&s))
269: 	})
270: 
271: 	t.Run("update bar", func(t *testing.T) {
272: 		t.Parallel()
273: 
274: 		b := entities.Bar{Time: tm, High: inp, Low: inp, Close: inp}
275: 		x, _ := NewCoronaSpectrumDefault()
276: 
277: 		for i := 0; i < primeCount; i++ {
278: 			x.Update(input[i%len(input)], tm)
279: 		}
280: 
281: 		check(x.UpdateBar(&b))
282: 	})
283: 
284: 	t.Run("update quote", func(t *testing.T) {
285: 		t.Parallel()
286: 
287: 		q := entities.Quote{Time: tm, Bid: inp, Ask: inp}
288: 		x, _ := NewCoronaSpectrumDefault()
289: 
290: 		for i := 0; i < primeCount; i++ {
291: 			x.Update(input[i%len(input)], tm)
292: 		}
293: 
294: 		check(x.UpdateQuote(&q))
295: 	})
296: 
297: 	t.Run("update trade", func(t *testing.T) {
298: 		t.Parallel()
299: 
300: 		r := entities.Trade{Time: tm, Price: inp}
301: 		x, _ := NewCoronaSpectrumDefault()
302: 
303: 		for i := 0; i < primeCount; i++ {
304: 			x.Update(input[i%len(input)], tm)
305: 		}
306: 
307: 		check(x.UpdateTrade(&r))
308: 	})
309: }
310: 
311: //nolint:funlen
312: func TestNewCoronaSpectrum(t *testing.T) {
313: 	t.Parallel()
314: 
315: 	check := func(name string, exp, act any) {
316: 		t.Helper()
317: 
318: 		if exp != act {
319: 			t.Errorf("%s: expected %v, actual %v", name, exp, act)
320: 		}
321: 	}
322: 
323: 	t.Run("default", func(t *testing.T) {
324: 		t.Parallel()
325: 
326: 		x, err := NewCoronaSpectrumDefault()
327: 		check("err == nil", true, err == nil)
328: 		check("mnemonic", "cspect(6, 20, 6, 30, 30, hl/2)", x.mnemonic)
329: 		check("MinParameterValue", 6.0, x.minParameterValue)
330: 		check("MaxParameterValue", 30.0, x.maxParameterValue)
331: 		check("ParameterResolution", 2.0, x.parameterResolution)
332: 	})
333: 
334: 	t.Run("custom ranges round to integers", func(t *testing.T) {
335: 		t.Parallel()
336: 
337: 		x, err := NewCoronaSpectrumParams(&Params{
338: 			MinRasterValue:       4,
339: 			MaxRasterValue:       25,
340: 			MinParameterValue:    8.7,  // ceils to 9
341: 			MaxParameterValue:    40.4, // floors to 40
342: 			HighPassFilterCutoff: 20,
343: 		})
344: 		check("err == nil", true, err == nil)
345: 		check("MinParameterValue", 9.0, x.minParameterValue)
346: 		check("MaxParameterValue", 40.0, x.maxParameterValue)
347: 		check("mnemonic", "cspect(4, 25, 9, 40, 20, hl/2)", x.mnemonic)
348: 	})
349: 
350: 	t.Run("MaxRasterValue <= MinRasterValue", func(t *testing.T) {
351: 		t.Parallel()
352: 
353: 		_, err := NewCoronaSpectrumParams(&Params{
354: 			MinRasterValue:    10,
355: 			MaxRasterValue:    10,
356: 			MinParameterValue: 6,
357: 			MaxParameterValue: 30,
358: 		})
359: 		if err == nil || err.Error() !=
360: 			"invalid corona spectrum parameters: MaxRasterValue should be > MinRasterValue" {
361: 			t.Errorf("unexpected: %v", err)
362: 		}
363: 	})
364: 
365: 	t.Run("MinParameterValue < 2", func(t *testing.T) {
366: 		t.Parallel()
367: 
368: 		_, err := NewCoronaSpectrumParams(&Params{
369: 			MinParameterValue: 1,
370: 			MaxParameterValue: 30,
371: 		})
372: 		if err == nil || err.Error() !=
373: 			"invalid corona spectrum parameters: MinParameterValue should be >= 2" {
374: 			t.Errorf("unexpected: %v", err)
375: 		}
376: 	})
377: 
378: 	t.Run("MaxParameterValue <= MinParameterValue", func(t *testing.T) {
379: 		t.Parallel()
380: 
381: 		_, err := NewCoronaSpectrumParams(&Params{
382: 			MinParameterValue: 20,
383: 			MaxParameterValue: 20,
384: 		})
385: 		if err == nil || err.Error() !=
386: 			"invalid corona spectrum parameters: MaxParameterValue should be > MinParameterValue" {
387: 			t.Errorf("unexpected: %v", err)
388: 		}
389: 	})
390: 
391: 	t.Run("HighPassFilterCutoff < 2", func(t *testing.T) {
392: 		t.Parallel()
393: 
394: 		_, err := NewCoronaSpectrumParams(&Params{
395: 			HighPassFilterCutoff: 1,
396: 		})
397: 		if err == nil || err.Error() !=
398: 			"invalid corona spectrum parameters: HighPassFilterCutoff should be >= 2" {
399: 			t.Errorf("unexpected: %v", err)
400: 		}
401: 	})
402: 
403: 	t.Run("invalid bar component", func(t *testing.T) {
404: 		t.Parallel()
405: 
406: 		_, err := NewCoronaSpectrumParams(&Params{
407: 			BarComponent: entities.BarComponent(9999),
408: 		})
409: 		if err == nil {
410: 			t.Error("expected error")
411: 		}
412: 	})
413: }

(End of file - total 413 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 8.2s)

_Thinking:_

 Now I have a clear plan: I'll add the Corona entries to the type definitions, set up the factory methods, create the helper files, and build out the four indicator packages. I've already got the snapshot values documented, so I just need to review the core logic from the existing Go implementations to understand how each indicator calculates its values.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio.go</path>
<type>file</type>
<content>1: // Package coronasignaltonoiseratio implements Ehlers' Corona Signal-to-Noise
2: // Ratio heatmap indicator.
3: //
4: // The Corona Signal to Noise Ratio is a measure of the cycle amplitude relative
5: // to noise. The "noise" is chosen to be the average bar height because there is
6: // not much trade information within the bar.
7: //
8: // Reference: John Ehlers, "Measuring Cycle Periods", Stocks & Commodities,
9: // November 2008.
10: package coronasignaltonoiseratio
11: 
12: //nolint: gofumpt
13: import (
14: 	"fmt"
15: 	"math"
16: 	"sort"
17: 	"sync"
18: 	"time"
19: 
20: 	"zpano/entities"
21: 	"zpano/indicators/core"
22: 	"zpano/indicators/core/outputs"
23: 	"zpano/indicators/johnehlers/corona"
24: )
25: 
26: const (
27: 	highLowBufferSize       = 5
28: 	highLowBufferSizeMinOne = highLowBufferSize - 1
29: 	highLowMedianIndex      = 2
30: 	averageSampleAlpha      = 0.1
31: 	averageSampleOneMinus   = 0.9
32: 	signalEmaAlpha          = 0.2
33: 	signalEmaOneMinus       = 0.9 // Intentional: sums to 1.1, per Ehlers.
34: 	noiseEmaAlpha           = 0.1
35: 	noiseEmaOneMinus        = 0.9
36: 	ratioOffsetDb           = 3.5
37: 	ratioUpperDb            = 10.
38: 	dbGain                  = 20.
39: 	widthLowRatioThreshold  = 0.5
40: 	widthBaseline           = 0.2
41: 	widthSlope              = 0.4
42: 	rasterBlendExponent     = 0.8
43: 	rasterBlendHalf         = 0.5
44: 	rasterNegativeArgCutoff = 1.
45: )
46: 
47: // CoronaSignalToNoiseRatio is Ehlers' Corona Signal-to-Noise Ratio indicator.
48: //
49: // It owns a private Corona spectral-analysis engine and exposes two outputs:
50: //
51: //   - Value: a per-bar heatmap column (intensity raster).
52: //   - SignalToNoiseRatio: the current SNR value mapped into
53: //     [MinParameterValue, MaxParameterValue].
54: type CoronaSignalToNoiseRatio struct {
55: 	mu                  sync.RWMutex
56: 	mnemonic            string
57: 	description         string
58: 	mnemonicSNR         string
59: 	descriptionSNR      string
60: 	c                   *corona.Corona
61: 	rasterLength        int
62: 	rasterStep          float64
63: 	maxRasterValue      float64
64: 	minParameterValue   float64
65: 	maxParameterValue   float64
66: 	parameterResolution float64
67: 	raster              []float64
68: 	highLowBuffer       [highLowBufferSize]float64
69: 	hlSorted            [highLowBufferSize]float64
70: 	averageSamplePrev   float64
71: 	signalPrev          float64
72: 	noisePrev           float64
73: 	signalToNoiseRatio  float64
74: 	isStarted           bool
75: 	barFunc             entities.BarFunc
76: 	quoteFunc           entities.QuoteFunc
77: 	tradeFunc           entities.TradeFunc
78: }
79: 
80: // NewCoronaSignalToNoiseRatioDefault returns an instance created with default parameters.
81: func NewCoronaSignalToNoiseRatioDefault() (*CoronaSignalToNoiseRatio, error) {
82: 	return NewCoronaSignalToNoiseRatioParams(&Params{})
83: }
84: 
85: // NewCoronaSignalToNoiseRatioParams returns an instance created with the supplied parameters.
86: //
87: //nolint:funlen,cyclop
88: func NewCoronaSignalToNoiseRatioParams(p *Params) (*CoronaSignalToNoiseRatio, error) {
89: 	const (
90: 		invalid      = "invalid corona signal to noise ratio parameters"
91: 		fmtRaster    = "%s: RasterLength should be >= 2"
92: 		fmtMaxRaster = "%s: MaxRasterValue should be > 0"
93: 		fmtMinParam  = "%s: MinParameterValue should be >= 0"
94: 		fmtMaxParam  = "%s: MaxParameterValue should be > MinParameterValue"
95: 		fmtHP        = "%s: HighPassFilterCutoff should be >= 2"
96: 		fmtMinP      = "%s: MinimalPeriod should be >= 2"
97: 		fmtMaxP      = "%s: MaximalPeriod should be > MinimalPeriod"
98: 		fmtw         = "%s: %w"
99: 		fmtnVal      = "csnr(%d, %g, %g, %g, %d%s)"
100: 		fmtnSNR      = "csnr-snr(%d%s)"
101: 		descrVal     = "Corona signal to noise ratio "
102: 		descrSNR     = "Corona signal to noise ratio scalar "
103: 		defRaster    = 50
104: 		defMaxRast   = 20.0
105: 		defMinParam  = 1.0
106: 		defMaxParam  = 11.0
107: 		defHPCutoff  = 30
108: 		defMinPer    = 6
109: 		defMaxPer    = 30
110: 	)
111: 
112: 	cfg := *p
113: 
114: 	if cfg.RasterLength == 0 {
115: 		cfg.RasterLength = defRaster
116: 	}
117: 
118: 	if cfg.MaxRasterValue == 0 {
119: 		cfg.MaxRasterValue = defMaxRast
120: 	}
121: 
122: 	if cfg.MinParameterValue == 0 {
123: 		cfg.MinParameterValue = defMinParam
124: 	}
125: 
126: 	if cfg.MaxParameterValue == 0 {
127: 		cfg.MaxParameterValue = defMaxParam
128: 	}
129: 
130: 	if cfg.HighPassFilterCutoff == 0 {
131: 		cfg.HighPassFilterCutoff = defHPCutoff
132: 	}
133: 
134: 	if cfg.MinimalPeriod == 0 {
135: 		cfg.MinimalPeriod = defMinPer
136: 	}
137: 
138: 	if cfg.MaximalPeriod == 0 {
139: 		cfg.MaximalPeriod = defMaxPer
140: 	}
141: 
142: 	if cfg.RasterLength < 2 {
143: 		return nil, fmt.Errorf(fmtRaster, invalid)
144: 	}
145: 
146: 	if cfg.MaxRasterValue <= 0 {
147: 		return nil, fmt.Errorf(fmtMaxRaster, invalid)
148: 	}
149: 
150: 	if cfg.MinParameterValue < 0 {
151: 		return nil, fmt.Errorf(fmtMinParam, invalid)
152: 	}
153: 
154: 	if cfg.MaxParameterValue <= cfg.MinParameterValue {
155: 		return nil, fmt.Errorf(fmtMaxParam, invalid)
156: 	}
157: 
158: 	if cfg.HighPassFilterCutoff < 2 {
159: 		return nil, fmt.Errorf(fmtHP, invalid)
160: 	}
161: 
162: 	if cfg.MinimalPeriod < 2 {
163: 		return nil, fmt.Errorf(fmtMinP, invalid)
164: 	}
165: 
166: 	if cfg.MaximalPeriod <= cfg.MinimalPeriod {
167: 		return nil, fmt.Errorf(fmtMaxP, invalid)
168: 	}
169: 
170: 	// Ehlers reference uses (High+Low)/2.
171: 	bc := cfg.BarComponent
172: 	if bc == 0 {
173: 		bc = entities.BarMedianPrice
174: 	}
175: 
176: 	qc := cfg.QuoteComponent
177: 	if qc == 0 {
178: 		qc = entities.DefaultQuoteComponent
179: 	}
180: 
181: 	tc := cfg.TradeComponent
182: 	if tc == 0 {
183: 		tc = entities.DefaultTradeComponent
184: 	}
185: 
186: 	c, err := corona.NewCorona(&corona.Params{
187: 		HighPassFilterCutoff: cfg.HighPassFilterCutoff,
188: 		MinimalPeriod:        cfg.MinimalPeriod,
189: 		MaximalPeriod:        cfg.MaximalPeriod,
190: 	})
191: 	if err != nil {
192: 		return nil, fmt.Errorf(fmtw, invalid, err)
193: 	}
194: 
195: 	barFunc, err := entities.BarComponentFunc(bc)
196: 	if err != nil {
197: 		return nil, fmt.Errorf(fmtw, invalid, err)
198: 	}
199: 
200: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
201: 	if err != nil {
202: 		return nil, fmt.Errorf(fmtw, invalid, err)
203: 	}
204: 
205: 	tradeFunc, err := entities.TradeComponentFunc(tc)
206: 	if err != nil {
207: 		return nil, fmt.Errorf(fmtw, invalid, err)
208: 	}
209: 
210: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
211: 
212: 	mnemonicVal := fmt.Sprintf(fmtnVal,
213: 		cfg.RasterLength, cfg.MaxRasterValue, cfg.MinParameterValue, cfg.MaxParameterValue,
214: 		cfg.HighPassFilterCutoff, componentMnemonic)
215: 	mnemonicSNR := fmt.Sprintf(fmtnSNR, cfg.HighPassFilterCutoff, componentMnemonic)
216: 
217: 	// Resolution satisfies: min + (rasterLength-1)/resolution = max.
218: 	paramResolution := float64(cfg.RasterLength-1) / (cfg.MaxParameterValue - cfg.MinParameterValue)
219: 
220: 	return &CoronaSignalToNoiseRatio{
221: 		mnemonic:            mnemonicVal,
222: 		description:         descrVal + mnemonicVal,
223: 		mnemonicSNR:         mnemonicSNR,
224: 		descriptionSNR:      descrSNR + mnemonicSNR,
225: 		c:                   c,
226: 		rasterLength:        cfg.RasterLength,
227: 		rasterStep:          cfg.MaxRasterValue / float64(cfg.RasterLength),
228: 		maxRasterValue:      cfg.MaxRasterValue,
229: 		minParameterValue:   cfg.MinParameterValue,
230: 		maxParameterValue:   cfg.MaxParameterValue,
231: 		parameterResolution: paramResolution,
232: 		raster:              make([]float64, cfg.RasterLength),
233: 		signalToNoiseRatio:  math.NaN(),
234: 		barFunc:             barFunc,
235: 		quoteFunc:           quoteFunc,
236: 		tradeFunc:           tradeFunc,
237: 	}, nil
238: }
239: 
240: // IsPrimed indicates whether the indicator is primed.
241: func (s *CoronaSignalToNoiseRatio) IsPrimed() bool {
242: 	s.mu.RLock()
243: 	defer s.mu.RUnlock()
244: 
245: 	return s.c.IsPrimed()
246: }
247: 
248: // Metadata describes the output data of the indicator.
249: func (s *CoronaSignalToNoiseRatio) Metadata() core.Metadata {
250: 	return core.Metadata{
251: 		Type:        core.CoronaSignalToNoiseRatio,
252: 		Mnemonic:    s.mnemonic,
253: 		Description: s.description,
254: 		Outputs: []outputs.Metadata{
255: 			{Kind: int(Value), Type: outputs.HeatmapType, Mnemonic: s.mnemonic, Description: s.description},
256: 			{Kind: int(SignalToNoiseRatio), Type: outputs.ScalarType, Mnemonic: s.mnemonicSNR, Description: s.descriptionSNR},
257: 		},
258: 	}
259: }
260: 
261: // Update feeds the next sample plus bar extremes and returns the heatmap column
262: // and the current SignalToNoiseRatio. On unprimed bars the heatmap is empty and
263: // the scalar is NaN. On NaN sample input state is left unchanged.
264: //
265: //nolint:funlen,cyclop,gocognit
266: func (s *CoronaSignalToNoiseRatio) Update(sample, sampleLow, sampleHigh float64, t time.Time) (*outputs.Heatmap, float64) {
267: 	s.mu.Lock()
268: 	defer s.mu.Unlock()
269: 
270: 	if math.IsNaN(sample) {
271: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
272: 	}
273: 
274: 	primed := s.c.Update(sample)
275: 
276: 	if !s.isStarted {
277: 		s.averageSamplePrev = sample
278: 		s.highLowBuffer[highLowBufferSizeMinOne] = sampleHigh - sampleLow
279: 		s.isStarted = true
280: 
281: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
282: 	}
283: 
284: 	maxAmpSq := s.c.MaximalAmplitudeSquared()
285: 
286: 	averageSample := averageSampleAlpha*sample + averageSampleOneMinus*s.averageSamplePrev
287: 	s.averageSamplePrev = averageSample
288: 
289: 	if math.Abs(averageSample) > 0 || maxAmpSq > 0 {
290: 		s.signalPrev = signalEmaAlpha*math.Sqrt(maxAmpSq) + signalEmaOneMinus*s.signalPrev
291: 	}
292: 
293: 	// Shift H-L ring buffer left; push new value.
294: 	for i := 0; i < highLowBufferSizeMinOne; i++ {
295: 		s.highLowBuffer[i] = s.highLowBuffer[i+1]
296: 	}
297: 	s.highLowBuffer[highLowBufferSizeMinOne] = sampleHigh - sampleLow
298: 
299: 	ratio := 0.0
300: 	if math.Abs(averageSample) > 0 {
301: 		for i := 0; i < highLowBufferSize; i++ {
302: 			s.hlSorted[i] = s.highLowBuffer[i]
303: 		}
304: 
305: 		sort.Float64s(s.hlSorted[:])
306: 		s.noisePrev = noiseEmaAlpha*s.hlSorted[highLowMedianIndex] + noiseEmaOneMinus*s.noisePrev
307: 
308: 		if math.Abs(s.noisePrev) > 0 {
309: 			ratio = dbGain*math.Log10(s.signalPrev/s.noisePrev) + ratioOffsetDb
310: 			if ratio < 0 {
311: 				ratio = 0
312: 			} else if ratio > ratioUpperDb {
313: 				ratio = ratioUpperDb
314: 			}
315: 
316: 			ratio /= ratioUpperDb // ∈ [0, 1]
317: 		}
318: 	}
319: 
320: 	s.signalToNoiseRatio = (s.maxParameterValue-s.minParameterValue)*ratio + s.minParameterValue
321: 
322: 	// Raster update.
323: 	width := 0.0
324: 	if ratio <= widthLowRatioThreshold {
325: 		width = widthBaseline - widthSlope*ratio
326: 	}
327: 
328: 	ratioScaledToRasterLength := int(math.Round(ratio * float64(s.rasterLength)))
329: 	ratioScaledToMaxRasterValue := ratio * s.maxRasterValue
330: 
331: 	for i := 0; i < s.rasterLength; i++ {
332: 		value := s.raster[i]
333: 
334: 		switch {
335: 		case i == ratioScaledToRasterLength:
336: 			value *= 0.5
337: 		case width == 0:
338: 			// Above the high-ratio threshold: handled by the ratio>0.5 override below.
339: 		default:
340: 			argument := (ratioScaledToMaxRasterValue - s.rasterStep*float64(i)) / width
341: 			if i < ratioScaledToRasterLength {
342: 				value = rasterBlendHalf * (math.Pow(argument, rasterBlendExponent) + value)
343: 			} else {
344: 				argument = -argument
345: 				if argument > rasterNegativeArgCutoff {
346: 					value = rasterBlendHalf * (math.Pow(argument, rasterBlendExponent) + value)
347: 				} else {
348: 					value = s.maxRasterValue
349: 				}
350: 			}
351: 		}
352: 
353: 		if value < 0 {
354: 			value = 0
355: 		} else if value > s.maxRasterValue {
356: 			value = s.maxRasterValue
357: 		}
358: 
359: 		if ratio > widthLowRatioThreshold {
360: 			value = s.maxRasterValue
361: 		}
362: 
363: 		s.raster[i] = value
364: 	}
365: 
366: 	if !primed {
367: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
368: 	}
369: 
370: 	values := make([]float64, s.rasterLength)
371: 	valueMin := math.Inf(1)
372: 	valueMax := math.Inf(-1)
373: 
374: 	for i := 0; i < s.rasterLength; i++ {
375: 		v := s.raster[i]
376: 		values[i] = v
377: 
378: 		if v < valueMin {
379: 			valueMin = v
380: 		}
381: 
382: 		if v > valueMax {
383: 			valueMax = v
384: 		}
385: 	}
386: 
387: 	heatmap := outputs.NewHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution,
388: 		valueMin, valueMax, values)
389: 
390: 	return heatmap, s.signalToNoiseRatio
391: }
392: 
393: // UpdateScalar updates the indicator given the next scalar sample. Since no
394: // High/Low is available, the sample is used for both, yielding zero noise.
395: func (s *CoronaSignalToNoiseRatio) UpdateScalar(sample *entities.Scalar) core.Output {
396: 	return s.updateEntity(sample.Time, sample.Value, sample.Value, sample.Value)
397: }
398: 
399: // UpdateBar updates the indicator given the next bar sample.
400: func (s *CoronaSignalToNoiseRatio) UpdateBar(sample *entities.Bar) core.Output {
401: 	return s.updateEntity(sample.Time, s.barFunc(sample), sample.Low, sample.High)
402: }
403: 
404: // UpdateQuote updates the indicator given the next quote sample.
405: func (s *CoronaSignalToNoiseRatio) UpdateQuote(sample *entities.Quote) core.Output {
406: 	v := s.quoteFunc(sample)
407: 
408: 	return s.updateEntity(sample.Time, v, v, v)
409: }
410: 
411: // UpdateTrade updates the indicator given the next trade sample.
412: func (s *CoronaSignalToNoiseRatio) UpdateTrade(sample *entities.Trade) core.Output {
413: 	v := s.tradeFunc(sample)
414: 
415: 	return s.updateEntity(sample.Time, v, v, v)
416: }
417: 
418: func (s *CoronaSignalToNoiseRatio) updateEntity(t time.Time, sample, low, high float64) core.Output {
419: 	const length = 2
420: 
421: 	heatmap, snr := s.Update(sample, low, high, t)
422: 
423: 	output := make([]any, length)
424: 	output[0] = heatmap
425: 	output[1] = entities.Scalar{Time: t, Value: snr}
426: 
427: 	return output
428: }

(End of file - total 428 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.8s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition.go</path>
<type>file</type>
<content>1: // Package coronaswingposition implements Ehlers' Corona Swing Position heatmap
2: // indicator.
3: //
4: // The Corona swing position is estimated by correlating the prices with a
5: // perfect sine wave having the dominant cycle period. This correlation produces
6: // a smooth waveform that lets us better estimate the swing position and
7: // impending turning points.
8: //
9: // Reference: John Ehlers, "Measuring Cycle Periods", Stocks & Commodities,
10: // November 2008.
11: package coronaswingposition
12: 
13: //nolint: gofumpt
14: import (
15: 	"fmt"
16: 	"math"
17: 	"sync"
18: 	"time"
19: 
20: 	"zpano/entities"
21: 	"zpano/indicators/core"
22: 	"zpano/indicators/core/outputs"
23: 	"zpano/indicators/johnehlers/corona"
24: )
25: 
26: const (
27: 	maxLeadListCount     = 50
28: 	maxPositionListCount = 20
29: 
30: 	// 60° phase-lead coefficients: lead60 = 0.5·BP + sin(60°)·Q ≈ 0.5·BP + 0.866·Q.
31: 	lead60CoefBP = 0.5
32: 	lead60CoefQ  = 0.866
33: 
34: 	// Bandpass filter delta factor in γ = 1/cos(ω · 2 · 0.1).
35: 	bpDelta = 0.1
36: 
37: 	widthHighThreshold = 0.85
38: 	widthHighSaturate  = 0.8
39: 	widthNarrow        = 0.01
40: 	widthScale         = 0.15
41: 
42: 	rasterBlendExponent = 0.95
43: 	rasterBlendHalf     = 0.5
44: )
45: 
46: // CoronaSwingPosition is Ehlers' Corona Swing Position indicator.
47: type CoronaSwingPosition struct {
48: 	mu                  sync.RWMutex
49: 	mnemonic            string
50: 	description         string
51: 	mnemonicSP          string
52: 	descriptionSP       string
53: 	c                   *corona.Corona
54: 	rasterLength        int
55: 	rasterStep          float64
56: 	maxRasterValue      float64
57: 	minParameterValue   float64
58: 	maxParameterValue   float64
59: 	parameterResolution float64
60: 	raster              []float64
61: 	leadList            []float64
62: 	positionList        []float64
63: 	samplePrev          float64
64: 	samplePrev2         float64
65: 	bandPassPrev        float64
66: 	bandPassPrev2       float64
67: 	swingPosition       float64
68: 	isStarted           bool
69: 	barFunc             entities.BarFunc
70: 	quoteFunc           entities.QuoteFunc
71: 	tradeFunc           entities.TradeFunc
72: }
73: 
74: // NewCoronaSwingPositionDefault returns an instance created with default parameters.
75: func NewCoronaSwingPositionDefault() (*CoronaSwingPosition, error) {
76: 	return NewCoronaSwingPositionParams(&Params{})
77: }
78: 
79: // NewCoronaSwingPositionParams returns an instance created with the supplied parameters.
80: //
81: //nolint:funlen,cyclop
82: func NewCoronaSwingPositionParams(p *Params) (*CoronaSwingPosition, error) {
83: 	const (
84: 		invalid      = "invalid corona swing position parameters"
85: 		fmtRaster    = "%s: RasterLength should be >= 2"
86: 		fmtMaxRaster = "%s: MaxRasterValue should be > 0"
87: 		fmtMaxParam  = "%s: MaxParameterValue should be > MinParameterValue"
88: 		fmtHP        = "%s: HighPassFilterCutoff should be >= 2"
89: 		fmtMinP      = "%s: MinimalPeriod should be >= 2"
90: 		fmtMaxP      = "%s: MaximalPeriod should be > MinimalPeriod"
91: 		fmtw         = "%s: %w"
92: 		fmtnVal      = "cswing(%d, %g, %g, %g, %d%s)"
93: 		fmtnSP       = "cswing-sp(%d%s)"
94: 		descrVal     = "Corona swing position "
95: 		descrSP      = "Corona swing position scalar "
96: 		defRaster    = 50
97: 		defMaxRast   = 20.0
98: 		defMinParam  = -5.0
99: 		defMaxParam  = 5.0
100: 		defHPCutoff  = 30
101: 		defMinPer    = 6
102: 		defMaxPer    = 30
103: 	)
104: 
105: 	cfg := *p
106: 
107: 	if cfg.RasterLength == 0 {
108: 		cfg.RasterLength = defRaster
109: 	}
110: 
111: 	if cfg.MaxRasterValue == 0 {
112: 		cfg.MaxRasterValue = defMaxRast
113: 	}
114: 
115: 	// MinParameterValue and MaxParameterValue default to -5 and 5; since 0 is a
116: 	// valid user value for either, we only substitute when both are zero (the
117: 	// unconfigured case).
118: 	if cfg.MinParameterValue == 0 && cfg.MaxParameterValue == 0 {
119: 		cfg.MinParameterValue = defMinParam
120: 		cfg.MaxParameterValue = defMaxParam
121: 	}
122: 
123: 	if cfg.HighPassFilterCutoff == 0 {
124: 		cfg.HighPassFilterCutoff = defHPCutoff
125: 	}
126: 
127: 	if cfg.MinimalPeriod == 0 {
128: 		cfg.MinimalPeriod = defMinPer
129: 	}
130: 
131: 	if cfg.MaximalPeriod == 0 {
132: 		cfg.MaximalPeriod = defMaxPer
133: 	}
134: 
135: 	if cfg.RasterLength < 2 {
136: 		return nil, fmt.Errorf(fmtRaster, invalid)
137: 	}
138: 
139: 	if cfg.MaxRasterValue <= 0 {
140: 		return nil, fmt.Errorf(fmtMaxRaster, invalid)
141: 	}
142: 
143: 	if cfg.MaxParameterValue <= cfg.MinParameterValue {
144: 		return nil, fmt.Errorf(fmtMaxParam, invalid)
145: 	}
146: 
147: 	if cfg.HighPassFilterCutoff < 2 {
148: 		return nil, fmt.Errorf(fmtHP, invalid)
149: 	}
150: 
151: 	if cfg.MinimalPeriod < 2 {
152: 		return nil, fmt.Errorf(fmtMinP, invalid)
153: 	}
154: 
155: 	if cfg.MaximalPeriod <= cfg.MinimalPeriod {
156: 		return nil, fmt.Errorf(fmtMaxP, invalid)
157: 	}
158: 
159: 	bc := cfg.BarComponent
160: 	if bc == 0 {
161: 		bc = entities.BarMedianPrice
162: 	}
163: 
164: 	qc := cfg.QuoteComponent
165: 	if qc == 0 {
166: 		qc = entities.DefaultQuoteComponent
167: 	}
168: 
169: 	tc := cfg.TradeComponent
170: 	if tc == 0 {
171: 		tc = entities.DefaultTradeComponent
172: 	}
173: 
174: 	c, err := corona.NewCorona(&corona.Params{
175: 		HighPassFilterCutoff: cfg.HighPassFilterCutoff,
176: 		MinimalPeriod:        cfg.MinimalPeriod,
177: 		MaximalPeriod:        cfg.MaximalPeriod,
178: 	})
179: 	if err != nil {
180: 		return nil, fmt.Errorf(fmtw, invalid, err)
181: 	}
182: 
183: 	barFunc, err := entities.BarComponentFunc(bc)
184: 	if err != nil {
185: 		return nil, fmt.Errorf(fmtw, invalid, err)
186: 	}
187: 
188: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
189: 	if err != nil {
190: 		return nil, fmt.Errorf(fmtw, invalid, err)
191: 	}
192: 
193: 	tradeFunc, err := entities.TradeComponentFunc(tc)
194: 	if err != nil {
195: 		return nil, fmt.Errorf(fmtw, invalid, err)
196: 	}
197: 
198: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
199: 
200: 	mnemonicVal := fmt.Sprintf(fmtnVal,
201: 		cfg.RasterLength, cfg.MaxRasterValue, cfg.MinParameterValue, cfg.MaxParameterValue,
202: 		cfg.HighPassFilterCutoff, componentMnemonic)
203: 	mnemonicSP := fmt.Sprintf(fmtnSP, cfg.HighPassFilterCutoff, componentMnemonic)
204: 
205: 	paramResolution := float64(cfg.RasterLength-1) / (cfg.MaxParameterValue - cfg.MinParameterValue)
206: 
207: 	return &CoronaSwingPosition{
208: 		mnemonic:            mnemonicVal,
209: 		description:         descrVal + mnemonicVal,
210: 		mnemonicSP:          mnemonicSP,
211: 		descriptionSP:       descrSP + mnemonicSP,
212: 		c:                   c,
213: 		rasterLength:        cfg.RasterLength,
214: 		rasterStep:          cfg.MaxRasterValue / float64(cfg.RasterLength),
215: 		maxRasterValue:      cfg.MaxRasterValue,
216: 		minParameterValue:   cfg.MinParameterValue,
217: 		maxParameterValue:   cfg.MaxParameterValue,
218: 		parameterResolution: paramResolution,
219: 		raster:              make([]float64, cfg.RasterLength),
220: 		leadList:            make([]float64, 0, maxLeadListCount),
221: 		positionList:        make([]float64, 0, maxPositionListCount),
222: 		swingPosition:       math.NaN(),
223: 		barFunc:             barFunc,
224: 		quoteFunc:           quoteFunc,
225: 		tradeFunc:           tradeFunc,
226: 	}, nil
227: }
228: 
229: // IsPrimed indicates whether the indicator is primed.
230: func (s *CoronaSwingPosition) IsPrimed() bool {
231: 	s.mu.RLock()
232: 	defer s.mu.RUnlock()
233: 
234: 	return s.c.IsPrimed()
235: }
236: 
237: // Metadata describes the output data of the indicator.
238: func (s *CoronaSwingPosition) Metadata() core.Metadata {
239: 	return core.Metadata{
240: 		Type:        core.CoronaSwingPosition,
241: 		Mnemonic:    s.mnemonic,
242: 		Description: s.description,
243: 		Outputs: []outputs.Metadata{
244: 			{Kind: int(Value), Type: outputs.HeatmapType, Mnemonic: s.mnemonic, Description: s.description},
245: 			{Kind: int(SwingPosition), Type: outputs.ScalarType, Mnemonic: s.mnemonicSP, Description: s.descriptionSP},
246: 		},
247: 	}
248: }
249: 
250: // Update feeds the next sample and returns the heatmap column plus the current
251: // SwingPosition. On unprimed bars the heatmap is empty and the scalar is NaN.
252: //
253: //nolint:funlen,cyclop,gocognit
254: func (s *CoronaSwingPosition) Update(sample float64, t time.Time) (*outputs.Heatmap, float64) {
255: 	s.mu.Lock()
256: 	defer s.mu.Unlock()
257: 
258: 	if math.IsNaN(sample) {
259: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
260: 	}
261: 
262: 	primed := s.c.Update(sample)
263: 
264: 	if !s.isStarted {
265: 		s.samplePrev = sample
266: 		s.isStarted = true
267: 
268: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
269: 	}
270: 
271: 	// Bandpass InPhase filter at the dominant cycle median period.
272: 	omega := 2. * math.Pi / s.c.DominantCycleMedian()
273: 	beta2 := math.Cos(omega)
274: 	gamma2 := 1. / math.Cos(omega*2*bpDelta)
275: 	alpha2 := gamma2 - math.Sqrt(gamma2*gamma2-1.)
276: 	bandPass := 0.5*(1-alpha2)*(sample-s.samplePrev2) +
277: 		beta2*(1+alpha2)*s.bandPassPrev -
278: 		alpha2*s.bandPassPrev2
279: 
280: 	// Quadrature = derivative / omega.
281: 	quadrature2 := (bandPass - s.bandPassPrev) / omega
282: 
283: 	s.bandPassPrev2 = s.bandPassPrev
284: 	s.bandPassPrev = bandPass
285: 	s.samplePrev2 = s.samplePrev
286: 	s.samplePrev = sample
287: 
288: 	// 60° lead: lead60 = 0.5·BP_prev2 + 0.866·Q
289: 	lead60 := lead60CoefBP*s.bandPassPrev2 + lead60CoefQ*quadrature2
290: 
291: 	lowest, highest := appendRolling(&s.leadList, maxLeadListCount, lead60)
292: 
293: 	// Normalised lead position ∈ [0, 1].
294: 	position := highest - lowest
295: 	if position > 0 {
296: 		position = (lead60 - lowest) / position
297: 	}
298: 
299: 	lowest, highest = appendRolling(&s.positionList, maxPositionListCount, position)
300: 	highest -= lowest
301: 
302: 	width := 0.15 * highest
303: 	if highest > widthHighThreshold {
304: 		width = widthNarrow
305: 	}
306: 
307: 	s.swingPosition = (s.maxParameterValue-s.minParameterValue)*position + s.minParameterValue
308: 
309: 	positionScaledToRasterLength := int(math.Round(position * float64(s.rasterLength)))
310: 	positionScaledToMaxRasterValue := position * s.maxRasterValue
311: 
312: 	for i := 0; i < s.rasterLength; i++ {
313: 		value := s.raster[i]
314: 
315: 		if i == positionScaledToRasterLength {
316: 			value *= rasterBlendHalf
317: 		} else {
318: 			argument := positionScaledToMaxRasterValue - s.rasterStep*float64(i)
319: 			if i > positionScaledToRasterLength {
320: 				argument = -argument
321: 			}
322: 
323: 			if width > 0 {
324: 				value = rasterBlendHalf *
325: 					(math.Pow(argument/width, rasterBlendExponent) + rasterBlendHalf*value)
326: 			}
327: 		}
328: 
329: 		if value < 0 {
330: 			value = 0
331: 		} else if value > s.maxRasterValue {
332: 			value = s.maxRasterValue
333: 		}
334: 
335: 		if highest > widthHighSaturate {
336: 			value = s.maxRasterValue
337: 		}
338: 
339: 		if math.IsNaN(value) {
340: 			value = 0
341: 		}
342: 
343: 		s.raster[i] = value
344: 	}
345: 
346: 	if !primed {
347: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
348: 	}
349: 
350: 	values := make([]float64, s.rasterLength)
351: 	valueMin := math.Inf(1)
352: 	valueMax := math.Inf(-1)
353: 
354: 	for i := 0; i < s.rasterLength; i++ {
355: 		v := s.raster[i]
356: 		values[i] = v
357: 
358: 		if v < valueMin {
359: 			valueMin = v
360: 		}
361: 
362: 		if v > valueMax {
363: 			valueMax = v
364: 		}
365: 	}
366: 
367: 	heatmap := outputs.NewHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution,
368: 		valueMin, valueMax, values)
369: 
370: 	return heatmap, s.swingPosition
371: }
372: 
373: // UpdateScalar updates the indicator given the next scalar sample.
374: func (s *CoronaSwingPosition) UpdateScalar(sample *entities.Scalar) core.Output {
375: 	return s.updateEntity(sample.Time, sample.Value)
376: }
377: 
378: // UpdateBar updates the indicator given the next bar sample.
379: func (s *CoronaSwingPosition) UpdateBar(sample *entities.Bar) core.Output {
380: 	return s.updateEntity(sample.Time, s.barFunc(sample))
381: }
382: 
383: // UpdateQuote updates the indicator given the next quote sample.
384: func (s *CoronaSwingPosition) UpdateQuote(sample *entities.Quote) core.Output {
385: 	return s.updateEntity(sample.Time, s.quoteFunc(sample))
386: }
387: 
388: // UpdateTrade updates the indicator given the next trade sample.
389: func (s *CoronaSwingPosition) UpdateTrade(sample *entities.Trade) core.Output {
390: 	return s.updateEntity(sample.Time, s.tradeFunc(sample))
391: }
392: 
393: func (s *CoronaSwingPosition) updateEntity(t time.Time, sample float64) core.Output {
394: 	const length = 2
395: 
396: 	heatmap, sp := s.Update(sample, t)
397: 
398: 	output := make([]any, length)
399: 	output[0] = heatmap
400: 	output[1] = entities.Scalar{Time: t, Value: sp}
401: 
402: 	return output
403: }
404: 
405: // appendRolling appends v to the list, drops the oldest element once the list
406: // reaches maxCount, and returns the current (lowest, highest) values.
407: func appendRolling(list *[]float64, maxCount int, v float64) (float64, float64) {
408: 	if len(*list) >= maxCount {
409: 		*list = (*list)[1:]
410: 	}
411: 
412: 	*list = append(*list, v)
413: 
414: 	lowest := v
415: 	highest := v
416: 
417: 	for _, x := range *list {
418: 		if x < lowest {
419: 			lowest = x
420: 		}
421: 
422: 		if x > highest {
423: 			highest = x
424: 		}
425: 	}
426: 
427: 	return lowest, highest
428: }

(End of file - total 428 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.6s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor.go</path>
<type>file</type>
<content>1: // Package coronatrendvigor implements Ehlers' Corona Trend Vigor heatmap
2: // indicator.
3: //
4: // The Corona trend vigor is computed as the slope of the momentum taken over a
5: // full dominant cycle period, normalised by the cycle amplitude. The ratio is
6: // scaled into the range [-10, 10]. A value of ±2 means the trend slope equals
7: // twice the cycle amplitude; values between -2 and +2 form the "corona" and
8: // suggest the trend should not be traded.
9: //
10: // Reference: John Ehlers, "Measuring Cycle Periods", Stocks & Commodities,
11: // November 2008.
12: package coronatrendvigor
13: 
14: //nolint: gofumpt
15: import (
16: 	"fmt"
17: 	"math"
18: 	"sync"
19: 	"time"
20: 
21: 	"zpano/entities"
22: 	"zpano/indicators/core"
23: 	"zpano/indicators/core/outputs"
24: 	"zpano/indicators/johnehlers/corona"
25: )
26: 
27: const (
28: 	// Bandpass filter delta factor in γ = 1/cos(ω · 2 · 0.1).
29: 	bpDelta = 0.1
30: 
31: 	// Ratio EMA coefficients (0.33 * current + 0.67 * previous).
32: 	ratioNewCoef  = 0.33
33: 	ratioPrevCoef = 0.67
34: 
35: 	// Vigor band thresholds.
36: 	vigorMidLow  = 0.3
37: 	vigorMidHigh = 0.7
38: 	vigorMid     = 0.5
39: 	widthEdge    = 0.01
40: 
41: 	// Raster update blend factors.
42: 	rasterBlendScale    = 0.8
43: 	rasterBlendPrevious = 0.2
44: 	rasterBlendHalf     = 0.5
45: 	rasterBlendExponent = 0.85
46: 
47: 	// Ratio clamp bounds.
48: 	ratioLimit = 10.0
49: 
50: 	// vigor = vigorScale*(ratio+ratioLimit); with ratioLimit=10 and range 20,
51: 	// vigorScale=0.05 maps ratio ∈ [-10,10] to vigor ∈ [0,1].
52: 	vigorScale = 0.05
53: )
54: 
55: // CoronaTrendVigor is Ehlers' Corona Trend Vigor indicator.
56: type CoronaTrendVigor struct {
57: 	mu                  sync.RWMutex
58: 	mnemonic            string
59: 	description         string
60: 	mnemonicTV          string
61: 	descriptionTV       string
62: 	c                   *corona.Corona
63: 	rasterLength        int
64: 	rasterStep          float64
65: 	maxRasterValue      float64
66: 	minParameterValue   float64
67: 	maxParameterValue   float64
68: 	parameterResolution float64
69: 	raster              []float64
70: 	sampleBuffer        []float64
71: 	sampleCount         int
72: 	samplePrev          float64
73: 	samplePrev2         float64
74: 	bandPassPrev        float64
75: 	bandPassPrev2       float64
76: 	ratioPrev           float64
77: 	trendVigor          float64
78: 	barFunc             entities.BarFunc
79: 	quoteFunc           entities.QuoteFunc
80: 	tradeFunc           entities.TradeFunc
81: }
82: 
83: // NewCoronaTrendVigorDefault returns an instance created with default parameters.
84: func NewCoronaTrendVigorDefault() (*CoronaTrendVigor, error) {
85: 	return NewCoronaTrendVigorParams(&Params{})
86: }
87: 
88: // NewCoronaTrendVigorParams returns an instance created with the supplied parameters.
89: //
90: //nolint:funlen,cyclop
91: func NewCoronaTrendVigorParams(p *Params) (*CoronaTrendVigor, error) {
92: 	const (
93: 		invalid      = "invalid corona trend vigor parameters"
94: 		fmtRaster    = "%s: RasterLength should be >= 2"
95: 		fmtMaxRaster = "%s: MaxRasterValue should be > 0"
96: 		fmtMaxParam  = "%s: MaxParameterValue should be > MinParameterValue"
97: 		fmtHP        = "%s: HighPassFilterCutoff should be >= 2"
98: 		fmtMinP      = "%s: MinimalPeriod should be >= 2"
99: 		fmtMaxP      = "%s: MaximalPeriod should be > MinimalPeriod"
100: 		fmtw         = "%s: %w"
101: 		fmtnVal      = "ctv(%d, %g, %g, %g, %d%s)"
102: 		fmtnTV       = "ctv-tv(%d%s)"
103: 		descrVal     = "Corona trend vigor "
104: 		descrTV      = "Corona trend vigor scalar "
105: 		defRaster    = 50
106: 		defMaxRast   = 20.0
107: 		defMinParam  = -10.0
108: 		defMaxParam  = 10.0
109: 		defHPCutoff  = 30
110: 		defMinPer    = 6
111: 		defMaxPer    = 30
112: 	)
113: 
114: 	cfg := *p
115: 
116: 	if cfg.RasterLength == 0 {
117: 		cfg.RasterLength = defRaster
118: 	}
119: 
120: 	if cfg.MaxRasterValue == 0 {
121: 		cfg.MaxRasterValue = defMaxRast
122: 	}
123: 
124: 	// MinParameterValue and MaxParameterValue default to -10 and 10; since 0 is a
125: 	// valid user value for either, we only substitute when both are zero (the
126: 	// unconfigured case).
127: 	if cfg.MinParameterValue == 0 && cfg.MaxParameterValue == 0 {
128: 		cfg.MinParameterValue = defMinParam
129: 		cfg.MaxParameterValue = defMaxParam
130: 	}
131: 
132: 	if cfg.HighPassFilterCutoff == 0 {
133: 		cfg.HighPassFilterCutoff = defHPCutoff
134: 	}
135: 
136: 	if cfg.MinimalPeriod == 0 {
137: 		cfg.MinimalPeriod = defMinPer
138: 	}
139: 
140: 	if cfg.MaximalPeriod == 0 {
141: 		cfg.MaximalPeriod = defMaxPer
142: 	}
143: 
144: 	if cfg.RasterLength < 2 {
145: 		return nil, fmt.Errorf(fmtRaster, invalid)
146: 	}
147: 
148: 	if cfg.MaxRasterValue <= 0 {
149: 		return nil, fmt.Errorf(fmtMaxRaster, invalid)
150: 	}
151: 
152: 	if cfg.MaxParameterValue <= cfg.MinParameterValue {
153: 		return nil, fmt.Errorf(fmtMaxParam, invalid)
154: 	}
155: 
156: 	if cfg.HighPassFilterCutoff < 2 {
157: 		return nil, fmt.Errorf(fmtHP, invalid)
158: 	}
159: 
160: 	if cfg.MinimalPeriod < 2 {
161: 		return nil, fmt.Errorf(fmtMinP, invalid)
162: 	}
163: 
164: 	if cfg.MaximalPeriod <= cfg.MinimalPeriod {
165: 		return nil, fmt.Errorf(fmtMaxP, invalid)
166: 	}
167: 
168: 	bc := cfg.BarComponent
169: 	if bc == 0 {
170: 		bc = entities.BarMedianPrice
171: 	}
172: 
173: 	qc := cfg.QuoteComponent
174: 	if qc == 0 {
175: 		qc = entities.DefaultQuoteComponent
176: 	}
177: 
178: 	tc := cfg.TradeComponent
179: 	if tc == 0 {
180: 		tc = entities.DefaultTradeComponent
181: 	}
182: 
183: 	c, err := corona.NewCorona(&corona.Params{
184: 		HighPassFilterCutoff: cfg.HighPassFilterCutoff,
185: 		MinimalPeriod:        cfg.MinimalPeriod,
186: 		MaximalPeriod:        cfg.MaximalPeriod,
187: 	})
188: 	if err != nil {
189: 		return nil, fmt.Errorf(fmtw, invalid, err)
190: 	}
191: 
192: 	barFunc, err := entities.BarComponentFunc(bc)
193: 	if err != nil {
194: 		return nil, fmt.Errorf(fmtw, invalid, err)
195: 	}
196: 
197: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
198: 	if err != nil {
199: 		return nil, fmt.Errorf(fmtw, invalid, err)
200: 	}
201: 
202: 	tradeFunc, err := entities.TradeComponentFunc(tc)
203: 	if err != nil {
204: 		return nil, fmt.Errorf(fmtw, invalid, err)
205: 	}
206: 
207: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
208: 
209: 	mnemonicVal := fmt.Sprintf(fmtnVal,
210: 		cfg.RasterLength, cfg.MaxRasterValue, cfg.MinParameterValue, cfg.MaxParameterValue,
211: 		cfg.HighPassFilterCutoff, componentMnemonic)
212: 	mnemonicTV := fmt.Sprintf(fmtnTV, cfg.HighPassFilterCutoff, componentMnemonic)
213: 
214: 	paramResolution := float64(cfg.RasterLength-1) / (cfg.MaxParameterValue - cfg.MinParameterValue)
215: 
216: 	return &CoronaTrendVigor{
217: 		mnemonic:            mnemonicVal,
218: 		description:         descrVal + mnemonicVal,
219: 		mnemonicTV:          mnemonicTV,
220: 		descriptionTV:       descrTV + mnemonicTV,
221: 		c:                   c,
222: 		rasterLength:        cfg.RasterLength,
223: 		rasterStep:          cfg.MaxRasterValue / float64(cfg.RasterLength),
224: 		maxRasterValue:      cfg.MaxRasterValue,
225: 		minParameterValue:   cfg.MinParameterValue,
226: 		maxParameterValue:   cfg.MaxParameterValue,
227: 		parameterResolution: paramResolution,
228: 		raster:              make([]float64, cfg.RasterLength),
229: 		sampleBuffer:        make([]float64, c.MaximalPeriodTimesTwo()),
230: 		trendVigor:          math.NaN(),
231: 		barFunc:             barFunc,
232: 		quoteFunc:           quoteFunc,
233: 		tradeFunc:           tradeFunc,
234: 	}, nil
235: }
236: 
237: // IsPrimed indicates whether the indicator is primed.
238: func (s *CoronaTrendVigor) IsPrimed() bool {
239: 	s.mu.RLock()
240: 	defer s.mu.RUnlock()
241: 
242: 	return s.c.IsPrimed()
243: }
244: 
245: // Metadata describes the output data of the indicator.
246: func (s *CoronaTrendVigor) Metadata() core.Metadata {
247: 	return core.Metadata{
248: 		Type:        core.CoronaTrendVigor,
249: 		Mnemonic:    s.mnemonic,
250: 		Description: s.description,
251: 		Outputs: []outputs.Metadata{
252: 			{Kind: int(Value), Type: outputs.HeatmapType, Mnemonic: s.mnemonic, Description: s.description},
253: 			{Kind: int(TrendVigor), Type: outputs.ScalarType, Mnemonic: s.mnemonicTV, Description: s.descriptionTV},
254: 		},
255: 	}
256: }
257: 
258: // Update feeds the next sample and returns the heatmap column plus the current
259: // TrendVigor. On unprimed bars the heatmap is empty and the scalar is NaN.
260: //
261: //nolint:funlen,cyclop,gocognit
262: func (s *CoronaTrendVigor) Update(sample float64, t time.Time) (*outputs.Heatmap, float64) {
263: 	s.mu.Lock()
264: 	defer s.mu.Unlock()
265: 
266: 	if math.IsNaN(sample) {
267: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
268: 	}
269: 
270: 	primed := s.c.Update(sample)
271: 	s.sampleCount++
272: 
273: 	bufLast := len(s.sampleBuffer) - 1
274: 
275: 	if s.sampleCount == 1 {
276: 		s.samplePrev = sample
277: 		s.sampleBuffer[bufLast] = sample
278: 
279: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
280: 	}
281: 
282: 	// Bandpass InPhase filter at the dominant cycle median period.
283: 	omega := 2. * math.Pi / s.c.DominantCycleMedian()
284: 	beta2 := math.Cos(omega)
285: 	gamma2 := 1. / math.Cos(omega*2*bpDelta)
286: 	alpha2 := gamma2 - math.Sqrt(gamma2*gamma2-1.)
287: 	bandPass := 0.5*(1-alpha2)*(sample-s.samplePrev2) +
288: 		beta2*(1+alpha2)*s.bandPassPrev -
289: 		alpha2*s.bandPassPrev2
290: 
291: 	// Quadrature = derivative / omega.
292: 	quadrature2 := (bandPass - s.bandPassPrev) / omega
293: 
294: 	s.bandPassPrev2 = s.bandPassPrev
295: 	s.bandPassPrev = bandPass
296: 	s.samplePrev2 = s.samplePrev
297: 	s.samplePrev = sample
298: 
299: 	// Left-shift sampleBuffer and append the new sample.
300: 	for i := 0; i < bufLast; i++ {
301: 		s.sampleBuffer[i] = s.sampleBuffer[i+1]
302: 	}
303: 
304: 	s.sampleBuffer[bufLast] = sample
305: 
306: 	// Cycle amplitude.
307: 	amplitude2 := math.Sqrt(bandPass*bandPass + quadrature2*quadrature2)
308: 
309: 	// Trend amplitude taken over the cycle period. Use DominantCycleMedian-1
310: 	// directly (the MBST implementation clamps to sampleBuffer length which
311: 	// negates the intent; see the reference impls).
312: 	cyclePeriod := int(s.c.DominantCycleMedian() - 1)
313: 	if cyclePeriod > len(s.sampleBuffer) {
314: 		cyclePeriod = len(s.sampleBuffer)
315: 	}
316: 
317: 	if cyclePeriod < 1 {
318: 		cyclePeriod = 1
319: 	}
320: 
321: 	lookback := cyclePeriod
322: 	if s.sampleCount < lookback {
323: 		lookback = s.sampleCount
324: 	}
325: 
326: 	trend := sample - s.sampleBuffer[len(s.sampleBuffer)-lookback]
327: 
328: 	ratio := 0.0
329: 	if math.Abs(trend) > 0 && amplitude2 > 0 {
330: 		ratio = ratioNewCoef*trend/amplitude2 + ratioPrevCoef*s.ratioPrev
331: 	}
332: 
333: 	if ratio > ratioLimit {
334: 		ratio = ratioLimit
335: 	} else if ratio < -ratioLimit {
336: 		ratio = -ratioLimit
337: 	}
338: 
339: 	s.ratioPrev = ratio
340: 
341: 	// ratio ∈ [-10, 10] ⇒ vigor ∈ [0, 1].
342: 	vigor := vigorScale * (ratio + ratioLimit)
343: 
344: 	var width float64
345: 
346: 	switch {
347: 	case vigor >= vigorMidLow && vigor < vigorMid:
348: 		width = vigor - (vigorMidLow - widthEdge)
349: 	case vigor >= vigorMid && vigor <= vigorMidHigh:
350: 		width = (vigorMidHigh + widthEdge) - vigor
351: 	default:
352: 		width = widthEdge
353: 	}
354: 
355: 	s.trendVigor = (s.maxParameterValue-s.minParameterValue)*vigor + s.minParameterValue
356: 
357: 	vigorScaledToRasterLength := int(math.Round(float64(s.rasterLength) * vigor))
358: 	vigorScaledToMaxRasterValue := vigor * s.maxRasterValue
359: 
360: 	for i := 0; i < s.rasterLength; i++ {
361: 		value := s.raster[i]
362: 
363: 		if i == vigorScaledToRasterLength {
364: 			value *= rasterBlendHalf
365: 		} else {
366: 			argument := vigorScaledToMaxRasterValue - s.rasterStep*float64(i)
367: 			if i > vigorScaledToRasterLength {
368: 				argument = -argument
369: 			}
370: 
371: 			if width > 0 {
372: 				value = rasterBlendScale *
373: 					(math.Pow(argument/width, rasterBlendExponent) + rasterBlendPrevious*value)
374: 			}
375: 		}
376: 
377: 		switch {
378: 		case value < 0:
379: 			value = 0
380: 		case value > s.maxRasterValue, vigor < vigorMidLow, vigor > vigorMidHigh:
381: 			value = s.maxRasterValue
382: 		}
383: 
384: 		if math.IsNaN(value) {
385: 			value = 0
386: 		}
387: 
388: 		s.raster[i] = value
389: 	}
390: 
391: 	if !primed {
392: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
393: 	}
394: 
395: 	values := make([]float64, s.rasterLength)
396: 	valueMin := math.Inf(1)
397: 	valueMax := math.Inf(-1)
398: 
399: 	for i := 0; i < s.rasterLength; i++ {
400: 		v := s.raster[i]
401: 		values[i] = v
402: 
403: 		if v < valueMin {
404: 			valueMin = v
405: 		}
406: 
407: 		if v > valueMax {
408: 			valueMax = v
409: 		}
410: 	}
411: 
412: 	heatmap := outputs.NewHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution,
413: 		valueMin, valueMax, values)
414: 
415: 	return heatmap, s.trendVigor
416: }
417: 
418: // UpdateScalar updates the indicator given the next scalar sample.
419: func (s *CoronaTrendVigor) UpdateScalar(sample *entities.Scalar) core.Output {
420: 	return s.updateEntity(sample.Time, sample.Value)
421: }
422: 
423: // UpdateBar updates the indicator given the next bar sample.
424: func (s *CoronaTrendVigor) UpdateBar(sample *entities.Bar) core.Output {
425: 	return s.updateEntity(sample.Time, s.barFunc(sample))
426: }
427: 
428: // UpdateQuote updates the indicator given the next quote sample.
429: func (s *CoronaTrendVigor) UpdateQuote(sample *entities.Quote) core.Output {
430: 	return s.updateEntity(sample.Time, s.quoteFunc(sample))
431: }
432: 
433: // UpdateTrade updates the indicator given the next trade sample.
434: func (s *CoronaTrendVigor) UpdateTrade(sample *entities.Trade) core.Output {
435: 	return s.updateEntity(sample.Time, s.tradeFunc(sample))
436: }
437: 
438: func (s *CoronaTrendVigor) updateEntity(t time.Time, sample float64) core.Output {
439: 	const length = 2
440: 
441: 	heatmap, tv := s.Update(sample, t)
442: 
443: 	output := make([]any, length)
444: 	output[0] = heatmap
445: 	output[1] = entities.Scalar{Time: t, Value: tv}
446: 
447: 	return output
448: }

(End of file - total 448 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 6.1s)

_Thinking:_

 Looking at the Go test file to understand the makeHL function and how the tests are structured.

**Tool: glob**

**Input:**
```json
{
  "pattern": "go/indicators/johnehlers/coronasignaltonoiseratio/*_test.go"
}
```

**Output:**
```
/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go
/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratiooutput_test.go
```

---

## Assistant (Build · claude-opus-4.7 · 5.3s)

**Tool: grep**

**Input:**
```json
{
  "pattern": "makeHL|func make",
  "path": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go"
}
```

**Output:**
```
Found 4 matches
/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go:
  Line 57: func makeHL(i int, sample float64) (float64, float64) {
  Line 95: 		low, high := makeHL(i, input[i])
  Line 159: 		low, high := makeHL(i, input[i])
  Line 262: 			low, high := makeHL(i, input[i%len(input)])
```

---

## Assistant (Build · claude-opus-4.7 · 3.9s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go",
  "offset": 55,
  "limit": 65
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go</path>
<type>file</type>
<content>55: // exercise the full range of the SNR logic (narrow spreads → high SNR,
56: // wide spreads → low SNR).
57: func makeHL(i int, sample float64) (float64, float64) {
58: 	// Fraction cycles between ~0.5% and ~6% of sample.
59: 	frac := 0.005 + 0.03*(1+math.Sin(float64(i)*0.37))
60: 	half := sample * frac
61: 
62: 	return sample - half, sample + half
63: }
64: 
65: func TestCoronaSignalToNoiseRatioUpdate(t *testing.T) {
66: 	t.Parallel()
67: 
68: 	input := testCSNRInput()
69: 	t0 := testCSNRTime()
70: 
71: 	// Snapshot values captured from a first run and locked in here.
72: 	type snap struct {
73: 		i   int
74: 		snr float64
75: 		vmn float64
76: 		vmx float64
77: 	}
78: 	snapshots := []snap{
79: 		{11, 1.0000000000, 0.0000000000, 20.0000000000},
80: 		{12, 1.0000000000, 0.0000000000, 20.0000000000},
81: 		{50, 1.0000000000, 0.0000000000, 20.0000000000},
82: 		{100, 2.9986583538, 4.2011609652, 20.0000000000},
83: 		{150, 1.0000000000, 0.0000000035, 20.0000000000},
84: 		{200, 1.0000000000, 0.0000000000, 20.0000000000},
85: 		{251, 1.0000000000, 0.0000000026, 20.0000000000},
86: 	}
87: 
88: 	x, err := NewCoronaSignalToNoiseRatioDefault()
89: 	if err != nil {
90: 		t.Fatalf("unexpected error: %v", err)
91: 	}
92: 
93: 	si := 0
94: 	for i := range input {
95: 		low, high := makeHL(i, input[i])
96: 		h, snr := x.Update(input[i], low, high, t0.Add(time.Duration(i)*time.Minute))
97: 
98: 		if h == nil {
99: 			t.Fatalf("[%d] heatmap must not be nil", i)
100: 		}
101: 
102: 		if h.ParameterFirst != 1 || h.ParameterLast != 11 || math.Abs(h.ParameterResolution-4.9) > 1e-9 {
103: 			t.Errorf("[%d] heatmap axis incorrect: first=%v last=%v res=%v",
104: 				i, h.ParameterFirst, h.ParameterLast, h.ParameterResolution)
105: 		}
106: 
107: 		if !x.IsPrimed() {
108: 			if !h.IsEmpty() {
109: 				t.Errorf("[%d] expected empty heatmap before priming, got len=%d", i, len(h.Values))
110: 			}
111: 
112: 			if !math.IsNaN(snr) {
113: 				t.Errorf("[%d] expected NaN snr before priming, got %v", i, snr)
114: 			}
115: 
116: 			continue
117: 		}
118: 
119: 		if len(h.Values) != 50 {

(Showing lines 55-119 of 393. Use offset=120 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 6.1s)

_Thinking:_

 I've gathered what I need to write the TypeScript code, so let me check how the Bar entity handles the 'high' and 'low' properties to make sure I'm implementing this correctly.

**Tool: bash**

**Input:**
```json
{
  "command": "ls ts/indicators/john-ehlers/"
}
```

**Error:**
```
Error: The bash tool was called with invalid arguments: [
  {
    "expected": "string",
    "code": "invalid_type",
    "path": [
      "description"
    ],
    "message": "Invalid input: expected string, received undefined"
  }
].
Please rewrite the input so it satisfies the expected schema.
```

---

## Assistant (Build · claude-opus-4.7 · 3.8s)

**Tool: bash**

**Input:**
```json
{
  "command": "ls ts/indicators/john-ehlers/",
  "description": "List john-ehlers indicators"
}
```

**Output:**
```
center-of-gravity-oscillator
cyber-cycle
dominant-cycle
fractal-adaptive-moving-average
hilbert-transformer
hilbert-transformer-instantaneous-trend-line
instantaneous-trend-line
mesa-adaptive-moving-average
roofing-filter
sine-wave
super-smoother
trend-cycle-mode
zero-lag-error-correcting-exponential-moving-average
zero-lag-exponential-moving-average

```

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/entities/bar.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/entities/bar.ts</path>
<type>file</type>
<content>1: /** An [open, high, low, close, volume] bar. */
2: export class Bar {
3:   /** The date and time.
4:    *
5:    * For _ohlcv_ bar entities it corresponds to the closing time, so that an _ohlcv_ bar accumulates lower-level entities
6:    * up to the closing date and time.
7:    */
8:   time!: Date;
9: 
10:   /** The opening price. */
11:   open!: number;
12: 
13:   /** The highest price. */
14:   high!: number;
15: 
16:   /** The lowest price. */
17:   low!: number;
18: 
19:   /** The closing price. */
20:   close!: number;
21: 
22:   /** The volume. */
23:   volume!: number;
24: 
25:   constructor(data?: any) {
26:     if (data) {
27:       for (const property in data) {
28:         if (Object.prototype.hasOwnProperty.call(data, property)) {
29:           (this as any)[property] = data[property];
30:         }
31:       }
32:     }
33:   }
34: 
35:   /** Indicates whether this is a rising bar (open < close). */
36:   isRising(): boolean {
37:     return this.open < this.close;
38:   }
39: 
40:   /** Indicates whether this is a falling bar (close < open). */
41:   isFalling(): boolean {
42:     return this.close < this.open;
43:   }
44: 
45:   /** The median price, calculated as _(low + high) / 2_. */
46:   median(): number {
47:     return (this.low + this.high) / 2;
48:   }
49: 
50:   /** The typical price, calculated as _(low + high + close) / 3_. */
51:   typical(): number {
52:     return (this.low + this.high + this.close) / 3;
53:   }
54: 
55:   /** The weighted price, calculated as _(low + high + 2*close) / 4_. */
56:   weighted(): number {
57:     return (this.low + this.high + this.close + this.close) / 4;
58:   }
59: 
60:   /** The average price, calculated as _(low + high + open + close) / 4_. */
61:   average(): number {
62:     return (this.low + this.high + this.open + this.close) / 4;
63:   }
64: }

(End of file - total 64 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.8s)

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
2: import { IndicatorType } from './indicator-type';
3: 
4: /** Describes a type and requested outputs of an indicator. */
5: export interface IndicatorMetadata {
6:     /** Identifies a type this indicator. */
7:     type: IndicatorType;
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

## Assistant (Build · claude-opus-4.7 · 4.5s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/indicator.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/indicator.ts</path>
<type>file</type>
<content>1: import { Bar } from '../../entities/bar';
2: import { Quote } from '../../entities/quote';
3: import { Scalar } from '../../entities/scalar';
4: import { Trade } from '../../entities/trade';
5: import { IndicatorMetadata } from './indicator-metadata';
6: import { IndicatorOutput } from './indicator-output';
7: 
8: /** Describes a common indicator functionality. */
9: export interface Indicator {
10:     /** Indicates whether an indicator is primed. */
11:     isPrimed(): boolean;
12: 
13:     /** Describes a requested output data of an indicator. */
14:     metadata(): IndicatorMetadata;
15: 
16:     /** Updates an indicator given the next scalar sample. */
17:     updateScalar(sample: Scalar): IndicatorOutput;
18: 
19:     /** Updates an indicator given the next bar sample. */
20:     updateBar(sample: Bar): IndicatorOutput;
21: 
22:     /** Updates an indicator given the next quote sample. */
23:     updateQuote(sample: Quote): IndicatorOutput;
24: 
25:     /** Updates an indicator given the next trade sample. */
26:     updateTrade(sample: Trade): IndicatorOutput;
27: }

(End of file - total 27 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/component-triple-mnemonic.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/component-triple-mnemonic.ts</path>
<type>file</type>
<content>1: import { BarComponent, DefaultBarComponent, barComponentMnemonic } from '../../entities/bar-component';
2: import { QuoteComponent, DefaultQuoteComponent, quoteComponentMnemonic } from '../../entities/quote-component';
3: import { TradeComponent, DefaultTradeComponent, tradeComponentMnemonic } from '../../entities/trade-component';
4: 
5: /**
6:  * Function to calculate mnemonic for a component triple.
7:  *
8:  * Default components are omitted from the mnemonic: the same indicator always
9:  * produces the same mnemonic regardless of whether defaults were explicitly
10:  * specified or left undefined.
11:  */
12: export const componentTripleMnemonic = (barComponent?: BarComponent, quoteComponent?: QuoteComponent, tradeComponent?: TradeComponent): string => {
13:     let str = '';
14: 
15:     if (barComponent !== undefined && barComponent !== DefaultBarComponent) {
16:         str += ', ' + barComponentMnemonic(barComponent);
17:     }
18: 
19:     if (quoteComponent !== undefined && quoteComponent !== DefaultQuoteComponent) {
20:         str += ', ' + quoteComponentMnemonic(quoteComponent);
21:     }
22: 
23:     if (tradeComponent !== undefined && tradeComponent !== DefaultTradeComponent) {
24:         str += ', ' + tradeComponentMnemonic(tradeComponent);
25:     }
26: 
27:     return str;
28: };

(End of file - total 28 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 12.0s)

_Thinking:_

 I've got a clear plan now—I'll update the type definitions first, then add the helper utilities, create the indicator packages, and finish with tests. I'm going to follow the Go implementation closely, particularly how `componentTripleMnemonic` handles undefined BarComponent by returning an empty string.

Starting to write the files...

**Tool: todowrite**

**Input:**
```json
{
  "todos": [
    {
      "content": "Append 4 Corona entries to indicator-type.ts",
      "status": "in_progress",
      "priority": "high"
    },
    {
      "content": "Add static factories to heatmap.ts",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Create Corona helper (corona.ts, corona-params.ts)",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Create CoronaSpectrum indicator package",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Create CoronaSignalToNoiseRatio package",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Create CoronaSwingPosition package",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Create CoronaTrendVigor package",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Run TS tests and fix issues",
      "status": "pending",
      "priority": "high"
    },
    {
      "content": "Update mbst-indicator-conversion skill with Corona learnings",
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
    "content": "Append 4 Corona entries to indicator-type.ts",
    "status": "in_progress",
    "priority": "high"
  },
  {
    "content": "Add static factories to heatmap.ts",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Create Corona helper (corona.ts, corona-params.ts)",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Create CoronaSpectrum indicator package",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Create CoronaSignalToNoiseRatio package",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Create CoronaSwingPosition package",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Create CoronaTrendVigor package",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Run TS tests and fix issues",
    "status": "pending",
    "priority": "high"
  },
  {
    "content": "Update mbst-indicator-conversion skill with Corona learnings",
    "status": "pending",
    "priority": "medium"
  }
]
```

---

## Assistant (Build · claude-opus-4.7 · 12.1s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/indicator-type.ts",
  "oldString": "  /** Identifies the Ehlers __Trend / Cycle Mode__ (TCM) indicator, exposing the trend/cycle value (+1 in trend, −1 in cycle),\n   * trend/cycle mode flags, instantaneous trend line, sine wave, lead sine wave, dominant cycle period and phase. */\n  TrendCycleMode,\n}",
  "newString": "  /** Identifies the Ehlers __Trend / Cycle Mode__ (TCM) indicator, exposing the trend/cycle value (+1 in trend, −1 in cycle),\n   * trend/cycle mode flags, instantaneous trend line, sine wave, lead sine wave, dominant cycle period and phase. */\n  TrendCycleMode,\n\n  /** Identifies the Ehlers __Corona Spectrum__ (CSPECT) heatmap indicator, exposing the dB heatmap column,\n   * the weighted dominant cycle estimate and its 5-sample median. */\n  CoronaSpectrum,\n\n  /** Identifies the Ehlers __Corona Signal To Noise Ratio__ (CSNR) heatmap indicator, exposing the intensity\n   * raster heatmap column and the current SNR mapped into the parameter range. */\n  CoronaSignalToNoiseRatio,\n\n  /** Identifies the Ehlers __Corona Swing Position__ (CSWING) heatmap indicator, exposing the intensity raster\n   * heatmap column and the current swing position mapped into the parameter range. */\n  CoronaSwingPosition,\n\n  /** Identifies the Ehlers __Corona Trend Vigor__ (CTV) heatmap indicator, exposing the intensity raster heatmap\n   * column and the current trend vigor scaled into the parameter range. */\n  CoronaTrendVigor,\n}"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 12.5s)

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/outputs/heatmap.ts",
  "content": "/** Holds a time stamp (x) and an array of values (z) corresponding to parameter (y) range to paint a heatmap column. */\nexport class Heatmap {\n  /** The date and time (x). */\n  time!: Date;\n\n  /** The first parameter (y) value of the heatmap. This value is the same for all columns. */\n  parameterFirst!: number;\n\n  /** The last parameter (y) value of the heatmap. This value is the same for all columns. */\n  parameterLast!: number;\n\n  /** A parameter resolution (positive number). A value of 10 means that heatmap values are evaluated at every 0.1 of parameter range. */\n  parameterResolution!: number;\n\n  /** A minimal value (z) of the heatmap column. */\n  valueMin!: number;\n\n  /** A maximal value (z) of the heatmap column. */\n  valueMax!: number;\n\n  /** The values (z) of the heatmap column. */\n  values!: number[];\n\n  /** Creates a populated heatmap column. */\n  public static newHeatmap(\n    time: Date,\n    parameterFirst: number,\n    parameterLast: number,\n    parameterResolution: number,\n    valueMin: number,\n    valueMax: number,\n    values: number[],\n  ): Heatmap {\n    const h = new Heatmap();\n    h.time = time;\n    h.parameterFirst = parameterFirst;\n    h.parameterLast = parameterLast;\n    h.parameterResolution = parameterResolution;\n    h.valueMin = valueMin;\n    h.valueMax = valueMax;\n    h.values = values;\n    return h;\n  }\n\n  /** Creates an empty heatmap column with valid axis metadata but no values. */\n  public static newEmptyHeatmap(\n    time: Date,\n    parameterFirst: number,\n    parameterLast: number,\n    parameterResolution: number,\n  ): Heatmap {\n    const h = new Heatmap();\n    h.time = time;\n    h.parameterFirst = parameterFirst;\n    h.parameterLast = parameterLast;\n    h.parameterResolution = parameterResolution;\n    h.valueMin = Number.NaN;\n    h.valueMax = Number.NaN;\n    h.values = [];\n    return h;\n  }\n\n  /** Indicates whether this heatmap column has no values. */\n  public isEmpty(): boolean {\n    return !this.values || this.values.length < 1;\n  }\n}\n"
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

## Assistant (Build · claude-opus-4.7 · 6.6s)

_Thinking:_

 The LSP errors in the mbst-to-convert C files aren't relevant to what I'm working on, so I'll move forward with implementing the helper.

Now the Corona helper package.

**Tool: bash**

**Input:**
```json
{
  "command": "mkdir -p ts/indicators/john-ehlers/corona ts/indicators/john-ehlers/corona-spectrum ts/indicators/john-ehlers/corona-signal-to-noise-ratio ts/indicators/john-ehlers/corona-swing-position ts/indicators/john-ehlers/corona-trend-vigor",
  "description": "Create TS package directories"
}
```

---

## Assistant (Build · claude-opus-4.7 · 13.1s)

**Tool: write**

**Input:**
```json
{
  "content": "/** Configures a Corona spectral analysis engine.\n *\n * All fields have zero-value defaults: a zero or _undefined_ value means \"use the default\".\n * The defaults follow Ehlers' original TASC article (November 2008).\n */\nexport interface CoronaParams {\n  /** High-pass filter cutoff period (de-trending period), in bars. Must be >= 2.\n   *\n   * The default value is 30.\n   */\n  highPassFilterCutoff?: number;\n\n  /** Minimum cycle period (in bars) covered by the bandpass filter bank. Must be >= 2.\n   *\n   * The default value is 6.\n   */\n  minimalPeriod?: number;\n\n  /** Maximum cycle period (in bars) covered by the bandpass filter bank. Must be > minimalPeriod.\n   *\n   * The default value is 30.\n   */\n  maximalPeriod?: number;\n\n  /** Filter bins with smoothed dB value at or below this threshold contribute to the\n   * weighted dominant-cycle estimate.\n   *\n   * The default value is 6.\n   */\n  decibelsLowerThreshold?: number;\n\n  /** Upper clamp on the smoothed dB value and reference value for the dominant-cycle\n   * weighting (weight = upper − dB).\n   *\n   * The default value is 20.\n   */\n  decibelsUpperThreshold?: number;\n}\n\n/** Default Corona parameters (Ehlers TASC, November 2008). */\nexport const DefaultCoronaParams: Required<CoronaParams> = {\n  highPassFilterCutoff: 30,\n  minimalPeriod: 6,\n  maximalPeriod: 30,\n  decibelsLowerThreshold: 6,\n  decibelsUpperThreshold: 20,\n};\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona/corona-params.ts"
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

## Assistant (Build · claude-opus-4.7 · 59.4s)

_Thinking:_

 Those LSP errors are showing up in some C files from mbst-to-convert that aren't related to what I'm working on, so I'll just disregard them.

**Tool: write**

**Input:**
```json
{
  "content": "import { CoronaParams, DefaultCoronaParams } from './corona-params';\n\nconst DEFAULT_HIGH_PASS_FILTER_CUTOFF = 30;\nconst DEFAULT_MINIMAL_PERIOD = 6;\nconst DEFAULT_MAXIMAL_PERIOD = 30;\nconst DEFAULT_DECIBELS_LOWER_THRESHOLD = 6;\nconst DEFAULT_DECIBELS_UPPER_THRESHOLD = 20;\n\nconst HIGH_PASS_FILTER_BUFFER_SIZE = 6;\nconst FIR_COEF_SUM = 12;\n\nconst DELTA_LOWER_THRESHOLD = 0.1;\nconst DELTA_FACTOR = -0.015;\nconst DELTA_SUMMAND = 0.5;\n\nconst DOMINANT_CYCLE_BUFFER_SIZE = 5;\nconst DOMINANT_CYCLE_MEDIAN_INDEX = 2;\n\nconst DECIBELS_SMOOTHING_ALPHA = 0.33;\nconst DECIBELS_SMOOTHING_ONE_MINUS = 0.67;\n\nconst NORMALIZED_AMPLITUDE_FACTOR = 0.99;\nconst DECIBELS_FLOOR = 0.01;\nconst DECIBELS_GAIN = 10;\n\n/** Per-bin state of a single bandpass filter in the Corona bank. */\nexport interface CoronaFilter {\n  inPhase: number;\n  inPhasePrev: number;\n  quadrature: number;\n  quadraturePrev: number;\n  real: number;\n  realPrev: number;\n  imaginary: number;\n  imaginaryPrev: number;\n\n  /** |Real + j·Imaginary|² of the most recent update. */\n  amplitudeSquared: number;\n\n  /** Smoothed dB value of the most recent update. */\n  decibels: number;\n}\n\n/** Shared spectral-analysis engine consumed by the CoronaSpectrum, CoronaSignalToNoiseRatio,\n * CoronaSwingPosition and CoronaTrendVigor indicators. It is not an indicator on its own.\n *\n * Call _update(sample)_ once per bar; then read _isPrimed_, _dominantCycle_,\n * _dominantCycleMedian_, _maximalAmplitudeSquared_ and the _filterBank_ array.\n *\n * Reference: John Ehlers, \"Measuring Cycle Periods\", Stocks & Commodities, November 2008. */\nexport class Corona {\n  public readonly minimalPeriod: number;\n  public readonly maximalPeriod: number;\n  public readonly minimalPeriodTimesTwo: number;\n  public readonly maximalPeriodTimesTwo: number;\n  public readonly filterBankLength: number;\n\n  private readonly decibelsLowerThreshold: number;\n  private readonly decibelsUpperThreshold: number;\n  private readonly alpha: number;\n  private readonly halfOnePlusAlpha: number;\n  private readonly preCalculatedBeta: number[];\n\n  public readonly filterBank: CoronaFilter[];\n  private readonly highPassBuffer: number[];\n  private readonly dominantCycleBuffer: number[];\n\n  private samplePrev = 0;\n  private smoothHPPrev = 0;\n  private _maximalAmplitudeSquared = 0;\n  private sampleCount = 0;\n  private _dominantCycle: number;\n  private _dominantCycleMedian: number;\n  private _primed = false;\n\n  /** Creates a Corona engine. Zero/undefined parameters are replaced with the Ehlers defaults. */\n  public constructor(params?: CoronaParams) {\n    const hp = params?.highPassFilterCutoff;\n    const minP = params?.minimalPeriod;\n    const maxP = params?.maximalPeriod;\n    const dbLo = params?.decibelsLowerThreshold;\n    const dbHi = params?.decibelsUpperThreshold;\n\n    const cfg = {\n      highPassFilterCutoff: hp !== undefined && hp > 0 ? hp : DEFAULT_HIGH_PASS_FILTER_CUTOFF,\n      minimalPeriod: minP !== undefined && minP > 0 ? minP : DEFAULT_MINIMAL_PERIOD,\n      maximalPeriod: maxP !== undefined && maxP > 0 ? maxP : DEFAULT_MAXIMAL_PERIOD,\n      decibelsLowerThreshold: dbLo !== undefined && dbLo !== 0 ? dbLo : DEFAULT_DECIBELS_LOWER_THRESHOLD,\n      decibelsUpperThreshold: dbHi !== undefined && dbHi !== 0 ? dbHi : DEFAULT_DECIBELS_UPPER_THRESHOLD,\n    };\n\n    if (cfg.highPassFilterCutoff < 2) {\n      throw new Error('invalid corona parameters: HighPassFilterCutoff should be >= 2');\n    }\n    if (cfg.minimalPeriod < 2) {\n      throw new Error('invalid corona parameters: MinimalPeriod should be >= 2');\n    }\n    if (cfg.maximalPeriod <= cfg.minimalPeriod) {\n      throw new Error('invalid corona parameters: MaximalPeriod should be > MinimalPeriod');\n    }\n    if (cfg.decibelsLowerThreshold < 0) {\n      throw new Error('invalid corona parameters: DecibelsLowerThreshold should be >= 0');\n    }\n    if (cfg.decibelsUpperThreshold <= cfg.decibelsLowerThreshold) {\n      throw new Error('invalid corona parameters: DecibelsUpperThreshold should be > DecibelsLowerThreshold');\n    }\n\n    this.minimalPeriod = cfg.minimalPeriod;\n    this.maximalPeriod = cfg.maximalPeriod;\n    this.minimalPeriodTimesTwo = cfg.minimalPeriod * 2;\n    this.maximalPeriodTimesTwo = cfg.maximalPeriod * 2;\n    this.decibelsLowerThreshold = cfg.decibelsLowerThreshold;\n    this.decibelsUpperThreshold = cfg.decibelsUpperThreshold;\n\n    this.filterBankLength = this.maximalPeriodTimesTwo - this.minimalPeriodTimesTwo + 1;\n\n    this.filterBank = new Array<CoronaFilter>(this.filterBankLength);\n    for (let i = 0; i < this.filterBankLength; i++) {\n      this.filterBank[i] = {\n        inPhase: 0, inPhasePrev: 0,\n        quadrature: 0, quadraturePrev: 0,\n        real: 0, realPrev: 0,\n        imaginary: 0, imaginaryPrev: 0,\n        amplitudeSquared: 0,\n        decibels: 0,\n      };\n    }\n\n    this.highPassBuffer = new Array<number>(HIGH_PASS_FILTER_BUFFER_SIZE).fill(0);\n\n    // MBST initializes the dominant cycle buffer with MaxValue sentinels.\n    this.dominantCycleBuffer = new Array<number>(DOMINANT_CYCLE_BUFFER_SIZE).fill(Number.MAX_VALUE);\n    this._dominantCycle = Number.MAX_VALUE;\n    this._dominantCycleMedian = Number.MAX_VALUE;\n\n    const phi = (2 * Math.PI) / cfg.highPassFilterCutoff;\n    this.alpha = (1 - Math.sin(phi)) / Math.cos(phi);\n    this.halfOnePlusAlpha = 0.5 * (1 + this.alpha);\n\n    this.preCalculatedBeta = new Array<number>(this.filterBankLength);\n    for (let idx = 0; idx < this.filterBankLength; idx++) {\n      const n = this.minimalPeriodTimesTwo + idx;\n      this.preCalculatedBeta[idx] = Math.cos((4 * Math.PI) / n);\n    }\n  }\n\n  /** Indicates whether the engine has seen enough samples to produce meaningful output. */\n  public isPrimed(): boolean { return this._primed; }\n\n  /** The most recent weighted-center-of-gravity estimate of the dominant cycle period. */\n  public get dominantCycle(): number { return this._dominantCycle; }\n\n  /** The 5-sample median of the most recent dominant cycle estimates. */\n  public get dominantCycleMedian(): number { return this._dominantCycleMedian; }\n\n  /** The maximum amplitude-squared observed across the filter bank for the most recently\n   * processed sample. Reset to zero at the start of every update call. */\n  public get maximalAmplitudeSquared(): number { return this._maximalAmplitudeSquared; }\n\n  /** Feeds the next sample to the engine. NaN samples are a no-op. Returns true once primed. */\n  public update(sample: number): boolean {\n    if (Number.isNaN(sample)) {\n      return this._primed;\n    }\n\n    this.sampleCount++;\n\n    // First sample: store as prior reference and return without further processing.\n    if (this.sampleCount === 1) {\n      this.samplePrev = sample;\n      return false;\n    }\n\n    // Step 1: High-pass filter.\n    const hp = this.alpha * this.highPassBuffer[HIGH_PASS_FILTER_BUFFER_SIZE - 1]\n      + this.halfOnePlusAlpha * (sample - this.samplePrev);\n    this.samplePrev = sample;\n\n    for (let i = 0; i < HIGH_PASS_FILTER_BUFFER_SIZE - 1; i++) {\n      this.highPassBuffer[i] = this.highPassBuffer[i + 1];\n    }\n    this.highPassBuffer[HIGH_PASS_FILTER_BUFFER_SIZE - 1] = hp;\n\n    // Step 2: 6-tap FIR smoothing {1, 2, 3, 3, 2, 1} / 12.\n    const smoothHP = (this.highPassBuffer[0]\n      + 2 * this.highPassBuffer[1]\n      + 3 * this.highPassBuffer[2]\n      + 3 * this.highPassBuffer[3]\n      + 2 * this.highPassBuffer[4]\n      + this.highPassBuffer[5]) / FIR_COEF_SUM;\n\n    // Step 3: Momentum.\n    const momentum = smoothHP - this.smoothHPPrev;\n    this.smoothHPPrev = smoothHP;\n\n    // Step 4: Adaptive delta.\n    let delta = DELTA_FACTOR * this.sampleCount + DELTA_SUMMAND;\n    if (delta < DELTA_LOWER_THRESHOLD) {\n      delta = DELTA_LOWER_THRESHOLD;\n    }\n\n    // Step 5: Filter-bank update.\n    this._maximalAmplitudeSquared = 0;\n    for (let idx = 0; idx < this.filterBankLength; idx++) {\n      const n = this.minimalPeriodTimesTwo + idx;\n      const nf = n;\n\n      const gamma = 1 / Math.cos((8 * Math.PI * delta) / nf);\n      const a = gamma - Math.sqrt(gamma * gamma - 1);\n\n      const quadrature = momentum * (nf / (4 * Math.PI));\n      const inPhase = smoothHP;\n\n      const halfOneMinA = 0.5 * (1 - a);\n      const beta = this.preCalculatedBeta[idx];\n      const betaOnePlusA = beta * (1 + a);\n\n      const f = this.filterBank[idx];\n\n      const real = halfOneMinA * (inPhase - f.inPhasePrev) + betaOnePlusA * f.real - a * f.realPrev;\n      const imag = halfOneMinA * (quadrature - f.quadraturePrev) + betaOnePlusA * f.imaginary - a * f.imaginaryPrev;\n\n      const ampSq = real * real + imag * imag;\n\n      f.inPhasePrev = f.inPhase;\n      f.inPhase = inPhase;\n      f.quadraturePrev = f.quadrature;\n      f.quadrature = quadrature;\n      f.realPrev = f.real;\n      f.real = real;\n      f.imaginaryPrev = f.imaginary;\n      f.imaginary = imag;\n      f.amplitudeSquared = ampSq;\n\n      if (ampSq > this._maximalAmplitudeSquared) {\n        this._maximalAmplitudeSquared = ampSq;\n      }\n    }\n\n    // Step 6: dB normalization and dominant-cycle weighted average.\n    let numerator = 0;\n    let denominator = 0;\n    this._dominantCycle = 0;\n\n    for (let idx = 0; idx < this.filterBankLength; idx++) {\n      const f = this.filterBank[idx];\n\n      let decibels = 0;\n      if (this._maximalAmplitudeSquared > 0) {\n        const normalized = f.amplitudeSquared / this._maximalAmplitudeSquared;\n        if (normalized > 0) {\n          const arg = (1 - NORMALIZED_AMPLITUDE_FACTOR * normalized) / DECIBELS_FLOOR;\n          if (arg > 0) {\n            decibels = DECIBELS_GAIN * Math.log10(arg);\n          }\n        }\n      }\n\n      decibels = DECIBELS_SMOOTHING_ALPHA * decibels + DECIBELS_SMOOTHING_ONE_MINUS * f.decibels;\n      if (decibels > this.decibelsUpperThreshold) {\n        decibels = this.decibelsUpperThreshold;\n      }\n      f.decibels = decibels;\n\n      if (decibels <= this.decibelsLowerThreshold) {\n        const n = this.minimalPeriodTimesTwo + idx;\n        const adjusted = this.decibelsUpperThreshold - decibels;\n        numerator += n * adjusted;\n        denominator += adjusted;\n      }\n    }\n\n    if (denominator !== 0) {\n      this._dominantCycle = (0.5 * numerator) / denominator;\n    }\n    if (this._dominantCycle < this.minimalPeriod) {\n      this._dominantCycle = this.minimalPeriod;\n    }\n\n    // Step 7: 5-sample median of dominant cycle.\n    for (let i = 0; i < DOMINANT_CYCLE_BUFFER_SIZE - 1; i++) {\n      this.dominantCycleBuffer[i] = this.dominantCycleBuffer[i + 1];\n    }\n    this.dominantCycleBuffer[DOMINANT_CYCLE_BUFFER_SIZE - 1] = this._dominantCycle;\n\n    const sorted = [...this.dominantCycleBuffer].sort((a, b) => a - b);\n    this._dominantCycleMedian = sorted[DOMINANT_CYCLE_MEDIAN_INDEX];\n    if (this._dominantCycleMedian < this.minimalPeriod) {\n      this._dominantCycleMedian = this.minimalPeriod;\n    }\n\n    if (this.sampleCount < this.minimalPeriodTimesTwo) {\n      return false;\n    }\n    this._primed = true;\n    return true;\n  }\n}\n\n// Re-exports avoid unused-import warning in bundlers.\nexport { DefaultCoronaParams };\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona/corona.ts"
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

## User

---

## Assistant (Compaction · claude-opus-4.7 · 97.3s)

---
## Goal

Port MBST's **Ehlers Corona Charts suite** (TASC November 2008) from `/home/dev/zpano/mbst-to-convert/john-ehlers/corona/` into zpano:

- **Corona** shared helper (not registered as indicator)
- **CoronaSpectrum** — heatmap + DC + DCM scalars
- **CoronaSignalToNoiseRatio** — heatmap + SNR scalar
- **CoronaSwingPosition** — heatmap + SwingPosition scalar
- **CoronaTrendVigor** — heatmap + TrendVigor scalar

First heatmap-producing indicators in zpano. **Go phase complete** (all 4 + helper, tests green). **Currently in Phase 6: TypeScript port — helper + type enum + heatmap factories done. Next: 4 indicator packages.**

## Instructions

### Design decisions (confirmed)
- Unprimed bars → empty heatmap (axis metadata present, values=[]); scalars NaN.
- Mnemonics: `cspect(…)`, `cspect-dc(…)`, `cspect-dcm(…)`, `csnr(…)`, `csnr-snr(…)`, `cswing(…)`, `cswing-sp(…)`, `ctv(…)`, `ctv-tv(…)`.
- User paused after Go phase; said "yes please" to continue; then: "continue if you have next steps, or stop and ask for clarification if you are unsure how to proceed."
- Snapshot tests reuse 252-entry TA-Lib MAMA series with tolerance 1e-4.
- User requests: continue if next steps clear, stop/ask if unsure.
- **Ignore LSP errors from `/home/dev/zpano/mbst-to-convert/common/*.c` — unrelated C/ta-lib files, noise only.**

### Algorithm decisions (locked in Go, must mirror in TS)
- Swing BP2 momentum: 2-bar (`sample - samplePrev2`).
- TrendVigor lookback: `int(DCM - 1)`; guard `cyclePeriod >= 1` (MBST fix); clamp upper at `len(sampleBuffer)`.
- SNR signal EMA coefficients: 0.2/0.9 (sum 1.1, intentional per Ehlers).
- DC weighted average: N=12..60 indexing; DC=0.5·num/denom; clamp DC <minPeriod. DCM = median of 5-buffer; clamp <minPeriod.
- DominantCycleBuffer initialized to `Number.MAX_VALUE` sentinels (MBST).
- CSpectrum rounds `minParam` up (ceil), `maxParam` down (floor).
- CSNR/CSwing/CTV: first sample store-only ("isStarted" pattern) returns empty heatmap, NaN scalar.
- CSNR scalar path: H=L=sample ⇒ SNR=MinParameterValue.

### Defaults / formulas
- Corona: `HP=30, minPer=6, maxPer=30, dBlow=6, dBhigh=20`.
- CSpectrum: `minRaster=6, maxRaster=20, minParam=6, maxParam=30, hpCutoff=30, BarComponent=Median`.
- CSNR: `rasterLength=50, maxRaster=20, minParam=1, maxParam=11`.
- CSwing: `rasterLength=50, maxRaster=20, minParam=-5, maxParam=5` (substitute defaults only when both min and max are 0).
- CTV: `rasterLength=50, maxRaster=20, minParam=-10, maxParam=10` (ditto).
- `ParameterResolution = (rasterLength-1)/(maxParam-minParam)`:
  - CSpectrum: `48/24 = 2.0`
  - CSNR: `49/10 = 4.9`
  - CSwing: `49/10 = 4.9`
  - CTV: `49/20 = 2.45`

### TS conventions (discovered)
- Use `class X implements Indicator` with private constructor + `static default()` + `static fromParams(params)`.
- `componentTripleMnemonic(bc, qc, tc)` — pass `BarComponent.Median` explicitly to get `, hl/2` suffix (Median !== DefaultBarComponent=Close).
- `update()` returns tuple; `updateEntity()` maps tuple→`IndicatorOutput` (array of Scalar/Band/Heatmap).
- `new Scalar()` then assign `time`/`value`; `new Bar({...})`/`new Quote({...})`/`new Trade({...})` with constructor-object.
- `Heatmap` is plain class; call `Heatmap.newHeatmap(...)` / `Heatmap.newEmptyHeatmap(...)` / `.isEmpty()` (just added).
- Test framework: Jasmine, `import { } from 'jasmine'`, tolerance 1e-4.
- `Math.log10`, `[...arr].sort((a,b)=>a-b)`, `Number.MAX_VALUE` (for Go `math.MaxFloat64`), `Number.POSITIVE_INFINITY`/`NEGATIVE_INFINITY` for `math.Inf`.
- Drop Go `sync.RWMutex` entirely in TS.
- `Math.round` JS vs Go `math.Round`: differ only at negative half-integers; use `Math.sign(x)*Math.floor(Math.abs(x)+0.5)` if tests fail.

## Discoveries

### Snapshot values (from Go tests — locked reference)

**CSpectrum** (`dc`, `dcm` at bars 11,12,50,100,150,200,251):
```
{11, 17.7604672565, 17.7604672565}
{12, 6.0000000000, 6.0000000000}
{50, 15.9989078712, 15.9989078712}
{100, 14.7455497547, 14.7455497547}
{150, 17.5000000000, 17.2826036069}
{200, 19.7557338512, 20.0000000000}
{251, 6.0000000000, 6.0000000000}
```

**CSNR** uses `makeHL(i, sample)`:
```
frac = 0.005 + 0.03*(1 + Math.sin(i*0.37))
half = sample * frac
[low, high] = [sample - half, sample + half]
```
Snapshots (`snr`, `vmin`, `vmax`):
```
{11, 1.0, 0.0, 20.0}
{12, 1.0, 0.0, 20.0}
{50, 1.0, 0.0, 20.0}
{100, 2.9986583538, 4.2011609652, 20.0}
{150, 1.0, 0.0000000035, 20.0}
{200, 1.0, 0.0, 20.0}
{251, 1.0, 0.0000000026, 20.0}
```

**CSwing** (`sp`, `vmin`, `vmax`):
```
{11, 5.0, 20.0, 20.0}
{12, 5.0, 20.0, 20.0}
{50, 4.5384908349, 20.0, 20.0}
{100, -3.8183742675, 3.4957777081, 20.0}
{150, -1.8516194371, 5.3792287864, 20.0}
{200, -3.6944428668, 4.2580825738, 20.0}
{251, -0.8524812061, 4.4822539784, 20.0}
```

**CTV** (`tv`, `vmin`, `vmax`):
```
{11, 5.6512200755, 20.0, 20.0}
{12, 6.8379492897, 20.0, 20.0}
{50, 2.6145116709, 2.3773561485, 20.0}
{100, 2.7536803664, 2.4892742850, 20.0}
{150, -6.4606404251, 20.0, 20.0}
{200, -10.0, 20.0, 20.0}
{251, -0.1894989954, 0.5847573715, 20.0}
```

### Mnemonic expectations (from Go tests)
- CSpectrum default: `cspect(6, 20, 6, 30, 30, hl/2)`; `cspect-dc(30, hl/2)`; `cspect-dcm(30, hl/2)`.
- CSpectrum custom ex: `cspect(4, 25, 9, 40, 20, hl/2)` with `min=8.7`(→9 ceil), `max=40.4`(→40 floor).
- CSNR default: `csnr(50, 20, 1, 11, 30, hl/2)`; `csnr-snr(30, hl/2)`.
- CSwing default: `cswing(50, 20, -5, 5, 30, hl/2)`; `cswing-sp(30, hl/2)`.
- CTV default: `ctv(50, 20, -10, 10, 30, hl/2)`; `ctv-tv(30, hl/2)`.
- Format: `%g` in Go = JS number with trailing zeros stripped, e.g. `20`, `4.9`, `-5`; just use `.toString()` for integer-valued floats or rely on natural printing. Note: `0.005` prints as `0.005` in Go `%g`. Values are all simple here; use `String(n)`. Verify `-5` vs `-5.0`: Go `%g` prints `-5`, so TS should too.

### Go heatmap factory semantics (mirrored in TS)
- `Heatmap.newEmptyHeatmap(t, first, last, res)` → `{time, first, last, res, valueMin=NaN, valueMax=NaN, values=[]}`.
- `Heatmap.newHeatmap(t, first, last, res, min, max, values)` → populated.
- `isEmpty()` → `values.length < 1`.

## Accomplished

### ✅ Go Phase Complete (all tests green)
- Corona helper (`corona/`) with Params, NewCorona, Update.
- 4 `core.Type` constants (47-50, `type.go` lines 201-215) with JSON round-trip.
- CoronaSpectrum / CSNR / CSwing / CTV Go packages with tests.

### ✅ TS Phase 6 — partially done
1. **Appended 4 entries** to `ts/indicators/core/indicator-type.ts` (before closing `}`): `CoronaSpectrum`, `CoronaSignalToNoiseRatio`, `CoronaSwingPosition`, `CoronaTrendVigor` with JSDoc.
2. **Added static factories** to `ts/indicators/core/outputs/heatmap.ts`:
   - `static newHeatmap(time, first, last, res, min, max, values): Heatmap`
   - `static newEmptyHeatmap(time, first, last, res): Heatmap` (min/max=NaN, values=[])
   - `isEmpty(): boolean` (values.length < 1)
3. **Created TS Corona helper**:
   - `ts/indicators/john-ehlers/corona/corona-params.ts` — `CoronaParams` interface + `DefaultCoronaParams`.
   - `ts/indicators/john-ehlers/corona/corona.ts` — full Corona engine class. Public props: `minimalPeriod`, `maximalPeriod`, `minimalPeriodTimesTwo`, `maximalPeriodTimesTwo`, `filterBankLength`, `filterBank`; getters `dominantCycle`, `dominantCycleMedian`, `maximalAmplitudeSquared`; `isPrimed()`, `update(sample): boolean`. Constructor public `new Corona(params?: CoronaParams)`.

### ⏳ Remaining TS work (execution order)

4. **Create 4 TS Corona indicator packages** under `ts/indicators/john-ehlers/`:
   - `corona-spectrum/`: `corona-spectrum.ts`, `corona-spectrum-params.ts`, `corona-spectrum-output.ts`, `corona-spectrum.spec.ts`
   - `corona-signal-to-noise-ratio/`: same 4 files
   - `corona-swing-position/`: same 4 files
   - `corona-trend-vigor/`: same 4 files

   Directory skeletons already created via `mkdir -p`.

   Signatures:
   - CSpectrum: `update(sample, time) → [Heatmap, number, number]` (heatmap, dc, dcm)
   - CSNR: `update(sample, low, high, time) → [Heatmap, number]`
   - CSwing/CTV: `update(sample, time) → [Heatmap, number]`
   - `updateEntity` returns `IndicatorOutput` with Heatmap object at [0] (always, even unprimed) + Scalar(s) for scalar outputs.
   - CSNR `updateScalar/Quote/Trade`: pass sample as both high and low; `updateBar`: pass `bar.low`, `bar.high`.
   - **Kind enum start from 1** in Go but TS TCM uses `Value = 0`. Decision: follow the TS TCM convention (0-indexed enum).

5. **Port snapshot tests** using verbatim values above; tolerance 1e-4. Copy 252-entry input from TCM spec (at `ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts` lines 15-42). CSNR test must implement `makeHL(i, sample)` helper.

6. **Update `.opencode/skills/mbst-indicator-conversion/SKILL.md`** with Corona learnings: heatmap conventions, first-bar store-only pattern, DCM-1 lookback fix with `>=1` guard, parameter-resolution formula, empty-heatmap-axis-always-present invariant, CSNR coefficient sum=1.1 is intentional.

### Test/build commands
- `npx jasmine` or `npm test` (verify from `package.json`). Run after each indicator.

## Relevant files / directories

### Go packages (reference — all read)
- `/home/dev/zpano/go/indicators/johnehlers/corona/{corona.go,params.go}`
- `/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/` — all 5 files
- `/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio.go` + `coronasignaltonoiseratio_test.go` (makeHL at line 57)
- `/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition.go`
- `/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor.go`

### TS infrastructure (read)
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/` — all 4 files (template)
- `/home/dev/zpano/ts/indicators/core/indicator-type.ts` — 4 entries appended ✅
- `/home/dev/zpano/ts/indicators/core/outputs/heatmap.ts` — factories added ✅
- `/home/dev/zpano/ts/indicators/core/outputs/band.ts`, `output-type.ts` (has `Heatmap`)
- `/home/dev/zpano/ts/indicators/core/indicator.ts`, `indicator-metadata.ts`, `indicator-output.ts`
- `/home/dev/zpano/ts/indicators/core/component-triple-mnemonic.ts`
- `/home/dev/zpano/ts/entities/{bar.ts,scalar.ts,quote.ts,trade.ts,bar-component.ts}`

### TS files created this session
- `/home/dev/zpano/ts/indicators/john-ehlers/corona/corona-params.ts` ✅
- `/home/dev/zpano/ts/indicators/john-ehlers/corona/corona.ts` ✅

### TS files to create
- `/home/dev/zpano/ts/indicators/john-ehlers/corona/corona.spec.ts` (optional; Go test was minimal — can skip)
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/` (4 files)
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-signal-to-noise-ratio/` (4 files)
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-swing-position/` (4 files)
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-trend-vigor/` (4 files)

(Empty directories already created by `mkdir -p`.)

### Skills
- `.opencode/skills/mbst-indicator-conversion/SKILL.md` — to update after TS complete

### Immediate next action
Continue with Phase 6 step 4:
1. Create `corona-spectrum/` package (model after TCM layout + Go CoronaSpectrum impl).
2. Run `npx jasmine` or `npm test` to verify.
3. Repeat for CSNR, CSwing, CTV.
4. Update skill doc.

### Todos (in-progress state)
1. ✅ Append 4 Corona entries to indicator-type.ts
2. ✅ Add static factories to heatmap.ts
3. ✅ Create Corona helper (corona.ts, corona-params.ts)
4. ⏳ Create CoronaSpectrum indicator package (← current)
5. ⏳ Create CoronaSignalToNoiseRatio package
6. ⏳ Create CoronaSwingPosition package
7. ⏳ Create CoronaTrendVigor package
8. ⏳ Run TS tests and fix issues
9. ⏳ Update mbst-indicator-conversion skill with Corona learnings

---

## User

---

## Assistant (Build · claude-opus-4.7 · 4.5s)

_Thinking:_

 Moving on to Phase 6 step 4 now—I need to create the CoronaSpectrum indicator package, so let me first examine the existing file structure to understand how it's organized.

**Tool: bash**

**Input:**
```json
{
  "command": "ls ts/indicators/john-ehlers/",
  "description": "List john-ehlers TS dir"
}
```

**Output:**
```
center-of-gravity-oscillator
corona
corona-signal-to-noise-ratio
corona-spectrum
corona-swing-position
corona-trend-vigor
cyber-cycle
dominant-cycle
fractal-adaptive-moving-average
hilbert-transformer
hilbert-transformer-instantaneous-trend-line
instantaneous-trend-line
mesa-adaptive-moving-average
roofing-filter
sine-wave
super-smoother
trend-cycle-mode
zero-lag-error-correcting-exponential-moving-average
zero-lag-exponential-moving-average

```

---

## Assistant (Build · claude-opus-4.7 · 4.1s)

**Tool: bash**

**Input:**
```json
{
  "command": "ls ts/indicators/john-ehlers/trend-cycle-mode/ ts/indicators/john-ehlers/corona/",
  "description": "Check TCM template and corona"
}
```

**Output:**
```
ts/indicators/john-ehlers/corona/:
corona-params.ts
corona.ts

ts/indicators/john-ehlers/trend-cycle-mode/:
trend-cycle-mode-output.ts
trend-cycle-mode-params.ts
trend-cycle-mode.spec.ts
trend-cycle-mode.ts

```

---

## Assistant (Build · claude-opus-4.7 · 5.0s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.ts</path>
<type>file</type>
<content>1: import { Bar } from '../../../entities/bar';
2: import { BarComponent, barComponentValue } from '../../../entities/bar-component';
3: import { Quote } from '../../../entities/quote';
4: import { DefaultQuoteComponent, quoteComponentValue } from '../../../entities/quote-component';
5: import { Scalar } from '../../../entities/scalar';
6: import { Trade } from '../../../entities/trade';
7: import { DefaultTradeComponent, tradeComponentValue } from '../../../entities/trade-component';
8: import { componentTripleMnemonic } from '../../core/component-triple-mnemonic';
9: import { Indicator } from '../../core/indicator';
10: import { IndicatorMetadata } from '../../core/indicator-metadata';
11: import { IndicatorOutput } from '../../core/indicator-output';
12: import { IndicatorType } from '../../core/indicator-type';
13: import { OutputType } from '../../core/outputs/output-type';
14: import { DominantCycle } from '../dominant-cycle/dominant-cycle';
15: import { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';
16: import { createEstimator, estimatorMoniker } from '../hilbert-transformer/hilbert-transformer-common';
17: import { TrendCycleModeOutput } from './trend-cycle-mode-output';
18: import { TrendCycleModeParams } from './trend-cycle-mode-params';
19: 
20: const DEFAULT_ALPHA_EMA_PERIOD_ADDITIONAL = 0.33;
21: const DEFAULT_SMOOTHING_LENGTH = 4;
22: const DEFAULT_ALPHA_EMA_QI = 0.2;
23: const DEFAULT_ALPHA_EMA_PERIOD = 0.2;
24: // MBST's DominantCyclePeriod default warm-up is MaxPeriod * 2 = 100.
25: const DEFAULT_WARM_UP_PERIOD = 100;
26: const DEFAULT_TREND_LINE_SMOOTHING_LENGTH = 4;
27: const DEFAULT_CYCLE_PART_MULTIPLIER = 1.0;
28: const DEFAULT_SEPARATION_PERCENTAGE = 1.5;
29: const MAX_CYCLE_PART_MULTIPLIER = 10.0;
30: const MAX_SEPARATION_PERCENTAGE = 100.0;
31: 
32: const DEG2RAD = Math.PI / 180.0;
33: const LEAD_OFFSET = 45.0;
34: const FULL_CYCLE = 360.0;
35: const EPSILON = 1e-308;
36: 
37: /** __Trend versus Cycle Mode__ (Ehlers) classifies the market as in-trend or in-cycle based
38:  * on the behaviour of the instantaneous dominant cycle period/phase and a WMA-smoothed
39:  * instantaneous trend line.
40:  *
41:  * It exposes eight outputs:
42:  *
43:  *	- Value: +1 in trend mode, -1 in cycle mode.
44:  *	- IsTrendMode: 1 if the trend mode is declared, 0 otherwise.
45:  *	- IsCycleMode: 1 if the cycle mode is declared, 0 otherwise (= 1 − IsTrendMode).
46:  *	- InstantaneousTrendLine: the WMA-smoothed instantaneous trend line.
47:  *	- SineWave: sin(phase·Deg2Rad).
48:  *	- SineWaveLead: sin((phase+45)·Deg2Rad).
49:  *	- DominantCyclePeriod: the smoothed dominant cycle period.
50:  *	- DominantCyclePhase: the dominant cycle phase, in degrees.
51:  *
52:  * Reference:
53:  *
54:  *	John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 113-118.
55:  */
56: export class TrendCycleMode implements Indicator {
57:   private readonly dc: DominantCycle;
58:   private readonly cyclePartMultiplier: number;
59:   private readonly separationFactor: number;
60:   private readonly coeff0: number;
61:   private readonly coeff1: number;
62:   private readonly coeff2: number;
63:   private readonly coeff3: number;
64:   private readonly input: number[];
65:   private readonly inputLength: number;
66:   private readonly inputLengthMin1: number;
67: 
68:   private trendline = Number.NaN;
69:   private trendAverage1 = 0;
70:   private trendAverage2 = 0;
71:   private trendAverage3 = 0;
72:   private sinWave = Number.NaN;
73:   private sinWaveLead = Number.NaN;
74:   private prevDcPhase = 0;
75:   private prevSineLeadWaveDifference = 0;
76:   private samplesInTrend = 0;
77:   private isTrendMode = true;
78:   private primed = false;
79: 
80:   private readonly mnemonicValue: string;
81:   private readonly descriptionValue: string;
82:   private readonly mnemonicTrend: string;
83:   private readonly descriptionTrend: string;
84:   private readonly mnemonicCycle: string;
85:   private readonly descriptionCycle: string;
86:   private readonly mnemonicITL: string;
87:   private readonly descriptionITL: string;
88:   private readonly mnemonicSine: string;
89:   private readonly descriptionSine: string;
90:   private readonly mnemonicSineLead: string;
91:   private readonly descriptionSineLead: string;
92:   private readonly mnemonicDCP: string;
93:   private readonly descriptionDCP: string;
94:   private readonly mnemonicDCPhase: string;
95:   private readonly descriptionDCPhase: string;
96: 
97:   private readonly barComponentFunc: (bar: Bar) => number;
98:   private readonly quoteComponentFunc: (quote: Quote) => number;
99:   private readonly tradeComponentFunc: (trade: Trade) => number;
100: 
101:   /** Creates an instance using default parameters (α=0.33, trendLineSmoothingLength=4,
102:    * cyclePartMultiplier=1.0, separationPercentage=1.5, HomodyneDiscriminator cycle estimator
103:    * with smoothingLength=4, αq=0.2, αp=0.2, warmUpPeriod=100, BarComponent.Median). */
104:   public static default(): TrendCycleMode {
105:     return new TrendCycleMode({
106:       alphaEmaPeriodAdditional: DEFAULT_ALPHA_EMA_PERIOD_ADDITIONAL,
107:       trendLineSmoothingLength: DEFAULT_TREND_LINE_SMOOTHING_LENGTH,
108:       cyclePartMultiplier: DEFAULT_CYCLE_PART_MULTIPLIER,
109:       separationPercentage: DEFAULT_SEPARATION_PERCENTAGE,
110:       estimatorType: HilbertTransformerCycleEstimatorType.HomodyneDiscriminator,
111:       estimatorParams: {
112:         smoothingLength: DEFAULT_SMOOTHING_LENGTH,
113:         alphaEmaQuadratureInPhase: DEFAULT_ALPHA_EMA_QI,
114:         alphaEmaPeriod: DEFAULT_ALPHA_EMA_PERIOD,
115:         warmUpPeriod: DEFAULT_WARM_UP_PERIOD,
116:       },
117:     });
118:   }
119: 
120:   /** Creates an instance based on the given parameters. */
121:   public static fromParams(params: TrendCycleModeParams): TrendCycleMode {
122:     return new TrendCycleMode(params);
123:   }
124: 
125:   private constructor(params: TrendCycleModeParams) {
126:     const alpha = params.alphaEmaPeriodAdditional;
127:     if (alpha <= 0 || alpha > 1) {
128:       throw new Error('invalid trend cycle mode parameters: α for additional smoothing should be in range (0, 1]');
129:     }
130: 
131:     const tlsl = params.trendLineSmoothingLength ?? DEFAULT_TREND_LINE_SMOOTHING_LENGTH;
132:     if (tlsl < 2 || tlsl > 4 || !Number.isInteger(tlsl)) {
133:       throw new Error('invalid trend cycle mode parameters: trend line smoothing length should be 2, 3, or 4');
134:     }
135: 
136:     const cpm = params.cyclePartMultiplier ?? DEFAULT_CYCLE_PART_MULTIPLIER;
137:     if (cpm <= 0 || cpm > MAX_CYCLE_PART_MULTIPLIER) {
138:       throw new Error('invalid trend cycle mode parameters: cycle part multiplier should be in range (0, 10]');
139:     }
140: 
141:     const sep = params.separationPercentage ?? DEFAULT_SEPARATION_PERCENTAGE;
142:     if (sep <= 0 || sep > MAX_SEPARATION_PERCENTAGE) {
143:       throw new Error('invalid trend cycle mode parameters: separation percentage should be in range (0, 100]');
144:     }
145: 
146:     this.cyclePartMultiplier = cpm;
147:     this.separationFactor = sep / 100.0;
148: 
149:     // TCM defaults to BarComponent.Median (MBST default; always shown in mnemonic).
150:     const bc = params.barComponent ?? BarComponent.Median;
151:     const qc = params.quoteComponent ?? DefaultQuoteComponent;
152:     const tc = params.tradeComponent ?? DefaultTradeComponent;
153: 
154:     this.barComponentFunc = barComponentValue(bc);
155:     this.quoteComponentFunc = quoteComponentValue(qc);
156:     this.tradeComponentFunc = tradeComponentValue(tc);
157: 
158:     // Inner DominantCycle built with explicit components so its mnemonic contains hl/2.
159:     this.dc = DominantCycle.fromParams({
160:       alphaEmaPeriodAdditional: alpha,
161:       estimatorType: params.estimatorType,
162:       estimatorParams: params.estimatorParams,
163:       barComponent: bc,
164:       quoteComponent: qc,
165:       tradeComponent: tc,
166:     });
167: 
168:     // Compose estimator moniker (same logic as DominantCycle / SineWave).
169:     const effectiveType = params.estimatorType ?? HilbertTransformerCycleEstimatorType.HomodyneDiscriminator;
170:     const htce = createEstimator(params.estimatorType, params.estimatorParams);
171:     let em = '';
172:     const isDefaultHd = effectiveType === HilbertTransformerCycleEstimatorType.HomodyneDiscriminator
173:       && htce.smoothingLength === DEFAULT_SMOOTHING_LENGTH
174:       && htce.alphaEmaQuadratureInPhase === DEFAULT_ALPHA_EMA_QI
175:       && htce.alphaEmaPeriod === DEFAULT_ALPHA_EMA_PERIOD;
176:     if (!isDefaultHd) {
177:       const moniker = estimatorMoniker(effectiveType, htce);
178:       if (moniker.length > 0) {
179:         em = ', ' + moniker;
180:       }
181:     }
182: 
183:     const cm = componentTripleMnemonic(bc, qc, tc);
184:     const a = alpha.toFixed(3);
185:     const c = cpm.toFixed(3);
186:     const s = sep.toFixed(3);
187:     const tail = `${a}, ${tlsl}, ${c}, ${s}%${em}${cm}`;
188: 
189:     this.mnemonicValue = `tcm(${tail})`;
190:     this.mnemonicTrend = `tcm-trend(${tail})`;
191:     this.mnemonicCycle = `tcm-cycle(${tail})`;
192:     this.mnemonicITL = `tcm-itl(${tail})`;
193:     this.mnemonicSine = `tcm-sine(${tail})`;
194:     this.mnemonicSineLead = `tcm-sineLead(${tail})`;
195:     this.mnemonicDCP = `dcp(${a}${em}${cm})`;
196:     this.mnemonicDCPhase = `dcph(${a}${em}${cm})`;
197: 
198:     this.descriptionValue = 'Trend versus cycle mode ' + this.mnemonicValue;
199:     this.descriptionTrend = 'Trend versus cycle mode, is-trend flag ' + this.mnemonicTrend;
200:     this.descriptionCycle = 'Trend versus cycle mode, is-cycle flag ' + this.mnemonicCycle;
201:     this.descriptionITL = 'Trend versus cycle mode instantaneous trend line ' + this.mnemonicITL;
202:     this.descriptionSine = 'Trend versus cycle mode sine wave ' + this.mnemonicSine;
203:     this.descriptionSineLead = 'Trend versus cycle mode sine wave lead ' + this.mnemonicSineLead;
204:     this.descriptionDCP = 'Dominant cycle period ' + this.mnemonicDCP;
205:     this.descriptionDCPhase = 'Dominant cycle phase ' + this.mnemonicDCPhase;
206: 
207:     // WMA coefficients.
208:     let c0 = 0, c1 = 0, c2 = 0, c3 = 0;
209:     if (tlsl === 2) {
210:       c0 = 2 / 3; c1 = 1 / 3;
211:     } else if (tlsl === 3) {
212:       c0 = 3 / 6; c1 = 2 / 6; c2 = 1 / 6;
213:     } else { // tlsl === 4
214:       c0 = 4 / 10; c1 = 3 / 10; c2 = 2 / 10; c3 = 1 / 10;
215:     }
216:     this.coeff0 = c0;
217:     this.coeff1 = c1;
218:     this.coeff2 = c2;
219:     this.coeff3 = c3;
220: 
221:     const maxPeriod = this.dc.maxPeriod;
222:     this.input = new Array<number>(maxPeriod).fill(0);
223:     this.inputLength = maxPeriod;
224:     this.inputLengthMin1 = maxPeriod - 1;
225:   }
226: 
227:   /** Indicates whether the indicator is primed. */
228:   public isPrimed(): boolean { return this.primed; }
229: 
230:   /** Describes the output data of the indicator. */
231:   public metadata(): IndicatorMetadata {
232:     return {
233:       type: IndicatorType.TrendCycleMode,
234:       mnemonic: this.mnemonicValue,
235:       description: this.descriptionValue,
236:       outputs: [
237:         {
238:           kind: TrendCycleModeOutput.Value,
239:           type: OutputType.Scalar,
240:           mnemonic: this.mnemonicValue,
241:           description: this.descriptionValue,
242:         },
243:         {
244:           kind: TrendCycleModeOutput.IsTrendMode,
245:           type: OutputType.Scalar,
246:           mnemonic: this.mnemonicTrend,
247:           description: this.descriptionTrend,
248:         },
249:         {
250:           kind: TrendCycleModeOutput.IsCycleMode,
251:           type: OutputType.Scalar,
252:           mnemonic: this.mnemonicCycle,
253:           description: this.descriptionCycle,
254:         },
255:         {
256:           kind: TrendCycleModeOutput.InstantaneousTrendLine,
257:           type: OutputType.Scalar,
258:           mnemonic: this.mnemonicITL,
259:           description: this.descriptionITL,
260:         },
261:         {
262:           kind: TrendCycleModeOutput.SineWave,
263:           type: OutputType.Scalar,
264:           mnemonic: this.mnemonicSine,
265:           description: this.descriptionSine,
266:         },
267:         {
268:           kind: TrendCycleModeOutput.SineWaveLead,
269:           type: OutputType.Scalar,
270:           mnemonic: this.mnemonicSineLead,
271:           description: this.descriptionSineLead,
272:         },
273:         {
274:           kind: TrendCycleModeOutput.DominantCyclePeriod,
275:           type: OutputType.Scalar,
276:           mnemonic: this.mnemonicDCP,
277:           description: this.descriptionDCP,
278:         },
279:         {
280:           kind: TrendCycleModeOutput.DominantCyclePhase,
281:           type: OutputType.Scalar,
282:           mnemonic: this.mnemonicDCPhase,
283:           description: this.descriptionDCPhase,
284:         },
285:       ],
286:     };
287:   }
288: 
289:   /** Updates the indicator given the next sample value. Returns the 8-tuple
290:    * (value, isTrendMode, isCycleMode, trendline, sineWave, sineWaveLead, period, phase).
291:    * isTrendMode / isCycleMode are encoded as 1 / 0 scalars. Returns all-NaN if not primed. */
292:   public update(sample: number): [number, number, number, number, number, number, number, number] {
293:     if (Number.isNaN(sample)) {
294:       return [sample, sample, sample, sample, sample, sample, sample, sample];
295:     }
296: 
297:     const [, period, phase] = this.dc.update(sample);
298:     const smoothedPrice = this.dc.smoothedPrice;
299: 
300:     this.pushInput(sample);
301: 
302:     if (this.primed) {
303:       const smoothedPeriod = period;
304:       const average = this.calculateTrendAverage(smoothedPeriod);
305:       this.trendline = this.coeff0 * average
306:         + this.coeff1 * this.trendAverage1
307:         + this.coeff2 * this.trendAverage2
308:         + this.coeff3 * this.trendAverage3;
309:       this.trendAverage3 = this.trendAverage2;
310:       this.trendAverage2 = this.trendAverage1;
311:       this.trendAverage1 = average;
312: 
313:       const diff = this.calculateSineLeadWaveDifference(phase);
314: 
315:       // Condition 1: a cycle mode exists for the half-period of a dominant cycle after
316:       // the SineWave vs SineWaveLead crossing.
317:       this.isTrendMode = true;
318: 
319:       if ((diff > 0 && this.prevSineLeadWaveDifference < 0)
320:         || (diff < 0 && this.prevSineLeadWaveDifference > 0)) {
321:         this.isTrendMode = false;
322:         this.samplesInTrend = 0;
323:       }
324: 
325:       this.prevSineLeadWaveDifference = diff;
326:       this.samplesInTrend++;
327: 
328:       if (this.samplesInTrend < 0.5 * smoothedPeriod) {
329:         this.isTrendMode = false;
330:       }
331: 
332:       // Condition 2: cycle mode if the measured phase rate of change is more than 2/3
333:       // the phase rate of change of the dominant cycle (360/period) and less than 1.5×.
334:       const phaseDelta = phase - this.prevDcPhase;
335:       this.prevDcPhase = phase;
336: 
337:       if (Math.abs(smoothedPeriod) > EPSILON) {
338:         const dcRate = FULL_CYCLE / smoothedPeriod;
339:         if (phaseDelta > (2.0 / 3.0) * dcRate && phaseDelta < 1.5 * dcRate) {
340:           this.isTrendMode = false;
341:         }
342:       }
343: 
344:       // Condition 3: if the WMA smoothed price is separated by more than the separation
345:       // percentage from the instantaneous trend line, force the trend mode.
346:       if (Math.abs(this.trendline) > EPSILON
347:         && Math.abs((smoothedPrice - this.trendline) / this.trendline) >= this.separationFactor) {
348:         this.isTrendMode = true;
349:       }
350: 
351:       return [
352:         this.mode(), this.isTrendFloat(), this.isCycleFloat(),
353:         this.trendline, this.sinWave, this.sinWaveLead, period, phase,
354:       ];
355:     }
356: 
357:     if (this.dc.isPrimed()) {
358:       this.primed = true;
359:       const smoothedPeriod = period;
360:       this.trendline = this.calculateTrendAverage(smoothedPeriod);
361:       this.trendAverage1 = this.trendline;
362:       this.trendAverage2 = this.trendline;
363:       this.trendAverage3 = this.trendline;
364: 
365:       this.prevDcPhase = phase;
366:       this.prevSineLeadWaveDifference = this.calculateSineLeadWaveDifference(phase);
367: 
368:       this.isTrendMode = true;
369:       this.samplesInTrend++;
370: 
371:       if (this.samplesInTrend < 0.5 * smoothedPeriod) {
372:         this.isTrendMode = false;
373:       }
374: 
375:       return [
376:         this.mode(), this.isTrendFloat(), this.isCycleFloat(),
377:         this.trendline, this.sinWave, this.sinWaveLead, period, phase,
378:       ];
379:     }
380: 
381:     const n = Number.NaN;
382:     return [n, n, n, n, n, n, n, n];
383:   }
384: 
385:   /** Updates an indicator given the next scalar sample. */
386:   public updateScalar(sample: Scalar): IndicatorOutput {
387:     return this.updateEntity(sample.time, sample.value);
388:   }
389: 
390:   /** Updates an indicator given the next bar sample. */
391:   public updateBar(sample: Bar): IndicatorOutput {
392:     return this.updateEntity(sample.time, this.barComponentFunc(sample));
393:   }
394: 
395:   /** Updates an indicator given the next quote sample. */
396:   public updateQuote(sample: Quote): IndicatorOutput {
397:     return this.updateEntity(sample.time, this.quoteComponentFunc(sample));
398:   }
399: 
400:   /** Updates an indicator given the next trade sample. */
401:   public updateTrade(sample: Trade): IndicatorOutput {
402:     return this.updateEntity(sample.time, this.tradeComponentFunc(sample));
403:   }
404: 
405:   private updateEntity(time: Date, sample: number): IndicatorOutput {
406:     const [value, trend, cycle, itl, sine, sineLead, period, phase] = this.update(sample);
407: 
408:     const out: Scalar[] = [];
409:     for (const v of [value, trend, cycle, itl, sine, sineLead, period, phase]) {
410:       const s = new Scalar();
411:       s.time = time;
412:       s.value = v;
413:       out.push(s);
414:     }
415:     return out;
416:   }
417: 
418:   private pushInput(value: number): void {
419:     for (let i = this.inputLengthMin1; i > 0; i--) {
420:       this.input[i] = this.input[i - 1];
421:     }
422:     this.input[0] = value;
423:   }
424: 
425:   private calculateTrendAverage(smoothedPeriod: number): number {
426:     let length = Math.floor(smoothedPeriod * this.cyclePartMultiplier + 0.5);
427:     if (length > this.inputLength) {
428:       length = this.inputLength;
429:     } else if (length < 1) {
430:       length = 1;
431:     }
432: 
433:     let sum = 0;
434:     for (let i = 0; i < length; i++) {
435:       sum += this.input[i];
436:     }
437:     return sum / length;
438:   }
439: 
440:   private calculateSineLeadWaveDifference(phase: number): number {
441:     const p = phase * DEG2RAD;
442:     this.sinWave = Math.sin(p);
443:     this.sinWaveLead = Math.sin(p + LEAD_OFFSET * DEG2RAD);
444:     return this.sinWave - this.sinWaveLead;
445:   }
446: 
447:   private mode(): number { return this.isTrendMode ? 1.0 : -1.0; }
448:   private isTrendFloat(): number { return this.isTrendMode ? 1.0 : 0.0; }
449:   private isCycleFloat(): number { return this.isTrendMode ? 0.0 : 1.0; }
450: }

(End of file - total 450 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 6.6s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-output.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-output.ts</path>
<type>file</type>
<content>1: /** Enumerates outputs of the TrendCycleMode indicator. */
2: export enum TrendCycleModeOutput {
3:   /** +1 in trend mode, -1 in cycle mode. */
4:   Value = 0,
5:   /** 1 if the trend mode is declared, 0 otherwise. */
6:   IsTrendMode = 1,
7:   /** 1 if the cycle mode is declared, 0 otherwise (= 1 − IsTrendMode). */
8:   IsCycleMode = 2,
9:   /** The WMA-smoothed instantaneous trend line. */
10:   InstantaneousTrendLine = 3,
11:   /** The sine wave value, sin(phase·Deg2Rad). */
12:   SineWave = 4,
13:   /** The sine wave lead value, sin((phase+45)·Deg2Rad). */
14:   SineWaveLead = 5,
15:   /** The smoothed dominant cycle period. */
16:   DominantCyclePeriod = 6,
17:   /** The dominant cycle phase, in degrees. */
18:   DominantCyclePhase = 7,
19: }

(End of file - total 19 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.5s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-params.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-params.ts</path>
<type>file</type>
<content>1: import { BarComponent } from '../../../entities/bar-component';
2: import { QuoteComponent } from '../../../entities/quote-component';
3: import { TradeComponent } from '../../../entities/trade-component';
4: import { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';
5: import { HilbertTransformerCycleEstimatorParams } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-params';
6: 
7: /** Describes parameters to create an instance of the TrendCycleMode indicator. */
8: export interface TrendCycleModeParams {
9:   /** The type of cycle estimator to use.
10:    *
11:    * The default value is HilbertTransformerCycleEstimatorType.HomodyneDiscriminator.
12:    */
13:   estimatorType?: HilbertTransformerCycleEstimatorType;
14: 
15:   /** Parameters to create an instance of the Hilbert transformer cycle estimator. */
16:   estimatorParams?: HilbertTransformerCycleEstimatorParams;
17: 
18:   /** The value of α (0 < α ≤ 1) used in EMA for additional smoothing of the instantaneous period.
19:    *
20:    * The default value is 0.33.
21:    */
22:   alphaEmaPeriodAdditional: number;
23: 
24:   /** The additional WMA smoothing length used to smooth the trend line.
25:    *
26:    * The valid values are 2, 3, 4. The default value is 4.
27:    */
28:   trendLineSmoothingLength?: number;
29: 
30:   /** The multiplier to the dominant cycle period used to determine the window length to
31:    * calculate the trend line. Typical values are in [0.5, 1.5].
32:    *
33:    * The default value is 1.0. Valid range is (0, 10].
34:    */
35:   cyclePartMultiplier?: number;
36: 
37:   /** The threshold (in percent) above which a wide separation between the WMA-smoothed
38:    * price and the instantaneous trend line forces the trend mode.
39:    *
40:    * The default value is 1.5. Valid range is (0, 100].
41:    */
42:   separationPercentage?: number;
43: 
44:   /** A component of a bar to use when updating the indicator with a bar sample.
45:    *
46:    * If _undefined_, the default (BarComponent.Median, hl/2) is used. Since the default
47:    * differs from the framework default bar component, it is always shown in the mnemonic.
48:    */
49:   barComponent?: BarComponent;
50: 
51:   /** A component of a quote to use when updating the indicator with a quote sample.
52:    *
53:    * If _undefined_, a default value is used and the component is not shown in the indicator mnemonic.
54:    */
55:   quoteComponent?: QuoteComponent;
56: 
57:   /** A component of a trade to use when updating the indicator with a trade sample.
58:    *
59:    * If _undefined_, a default value is used and the component is not shown in the indicator mnemonic.
60:    */
61:   tradeComponent?: TradeComponent;
62: }

(End of file - total 62 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.5s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaspectrum</path>
<type>directory</type>
<entries>
coronaspectrum_test.go
coronaspectrum.go
coronaspectrumoutput_test.go
coronaspectrumoutput.go
coronaspectrumparams.go

(5 entries)
</entries>
```

---

## Assistant (Build · claude-opus-4.7 · 3.9s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrum.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrum.go</path>
<type>file</type>
<content>1: // Package coronaspectrum implements Ehlers' Corona Spectrum heatmap indicator.
2: //
3: // The Corona Spectrum measures cyclic activity over a cycle period range
4: // (default 6..30 bars) in a bank of contiguous bandpass filters. The amplitude
5: // of each filter output is compared to the strongest signal and displayed, in
6: // decibels, as a heatmap column. The filter having the strongest output is
7: // selected as the current dominant cycle period.
8: //
9: // Reference: John Ehlers, "Measuring Cycle Periods", Stocks & Commodities,
10: // November 2008.
11: package coronaspectrum
12: 
13: //nolint: gofumpt
14: import (
15: 	"fmt"
16: 	"math"
17: 	"sync"
18: 	"time"
19: 
20: 	"zpano/entities"
21: 	"zpano/indicators/core"
22: 	"zpano/indicators/core/outputs"
23: 	"zpano/indicators/johnehlers/corona"
24: )
25: 
26: // CoronaSpectrum is Ehlers' Corona Spectrum heatmap indicator.
27: //
28: // It owns a private Corona spectral-analysis engine and exposes three outputs:
29: //
30: //   - Value: a per-bar heatmap column (decibels across the filter bank).
31: //   - DominantCycle: the weighted-center-of-gravity dominant cycle estimate.
32: //   - DominantCycleMedian: the 5-sample median of DominantCycle.
33: type CoronaSpectrum struct {
34: 	mu                  sync.RWMutex
35: 	mnemonic            string
36: 	description         string
37: 	mnemonicDC          string
38: 	descriptionDC       string
39: 	mnemonicDCM         string
40: 	descriptionDCM      string
41: 	c                   *corona.Corona
42: 	minParameterValue   float64
43: 	maxParameterValue   float64
44: 	parameterResolution float64
45: 	minRasterValue      float64
46: 	maxRasterValue      float64
47: 	barFunc             entities.BarFunc
48: 	quoteFunc           entities.QuoteFunc
49: 	tradeFunc           entities.TradeFunc
50: }
51: 
52: // NewCoronaSpectrumDefault returns an instance created with default parameters.
53: func NewCoronaSpectrumDefault() (*CoronaSpectrum, error) {
54: 	return NewCoronaSpectrumParams(&Params{})
55: }
56: 
57: // NewCoronaSpectrumParams returns an instance created with the supplied parameters.
58: //
59: //nolint:funlen,cyclop
60: func NewCoronaSpectrumParams(p *Params) (*CoronaSpectrum, error) {
61: 	const (
62: 		invalid      = "invalid corona spectrum parameters"
63: 		fmtMinRaster = "%s: MinRasterValue should be >= 0"
64: 		fmtMaxRaster = "%s: MaxRasterValue should be > MinRasterValue"
65: 		fmtMinParam  = "%s: MinParameterValue should be >= 2"
66: 		fmtMaxParam  = "%s: MaxParameterValue should be > MinParameterValue"
67: 		fmtHP        = "%s: HighPassFilterCutoff should be >= 2"
68: 		fmtw         = "%s: %w"
69: 		fmtnVal      = "cspect(%g, %g, %g, %g, %d%s)"
70: 		fmtnDC       = "cspect-dc(%d%s)"
71: 		fmtnDCM      = "cspect-dcm(%d%s)"
72: 		descrVal     = "Corona spectrum "
73: 		descrDC      = "Corona spectrum dominant cycle "
74: 		descrDCM     = "Corona spectrum dominant cycle median "
75: 		defMinRaster = 6.0
76: 		defMaxRaster = 20.0
77: 		defMinParam  = 6.0
78: 		defMaxParam  = 30.0
79: 		defHPCutoff  = 30
80: 	)
81: 
82: 	cfg := *p
83: 
84: 	if cfg.MinRasterValue == 0 {
85: 		cfg.MinRasterValue = defMinRaster
86: 	}
87: 
88: 	if cfg.MaxRasterValue == 0 {
89: 		cfg.MaxRasterValue = defMaxRaster
90: 	}
91: 
92: 	if cfg.MinParameterValue == 0 {
93: 		cfg.MinParameterValue = defMinParam
94: 	}
95: 
96: 	if cfg.MaxParameterValue == 0 {
97: 		cfg.MaxParameterValue = defMaxParam
98: 	}
99: 
100: 	if cfg.HighPassFilterCutoff == 0 {
101: 		cfg.HighPassFilterCutoff = defHPCutoff
102: 	}
103: 
104: 	if cfg.MinRasterValue < 0 {
105: 		return nil, fmt.Errorf(fmtMinRaster, invalid)
106: 	}
107: 
108: 	if cfg.MaxRasterValue <= cfg.MinRasterValue {
109: 		return nil, fmt.Errorf(fmtMaxRaster, invalid)
110: 	}
111: 
112: 	// MBST rounds min up and max down to integers.
113: 	minParam := math.Ceil(cfg.MinParameterValue)
114: 	maxParam := math.Floor(cfg.MaxParameterValue)
115: 
116: 	if minParam < 2 {
117: 		return nil, fmt.Errorf(fmtMinParam, invalid)
118: 	}
119: 
120: 	if maxParam <= minParam {
121: 		return nil, fmt.Errorf(fmtMaxParam, invalid)
122: 	}
123: 
124: 	if cfg.HighPassFilterCutoff < 2 {
125: 		return nil, fmt.Errorf(fmtHP, invalid)
126: 	}
127: 
128: 	// CoronaSpectrum mirrors Ehlers' reference: BarMedianPrice default.
129: 	bc := cfg.BarComponent
130: 	if bc == 0 {
131: 		bc = entities.BarMedianPrice
132: 	}
133: 
134: 	qc := cfg.QuoteComponent
135: 	if qc == 0 {
136: 		qc = entities.DefaultQuoteComponent
137: 	}
138: 
139: 	tc := cfg.TradeComponent
140: 	if tc == 0 {
141: 		tc = entities.DefaultTradeComponent
142: 	}
143: 
144: 	c, err := corona.NewCorona(&corona.Params{
145: 		HighPassFilterCutoff:   cfg.HighPassFilterCutoff,
146: 		MinimalPeriod:          int(minParam),
147: 		MaximalPeriod:          int(maxParam),
148: 		DecibelsLowerThreshold: cfg.MinRasterValue,
149: 		DecibelsUpperThreshold: cfg.MaxRasterValue,
150: 	})
151: 	if err != nil {
152: 		return nil, fmt.Errorf(fmtw, invalid, err)
153: 	}
154: 
155: 	barFunc, err := entities.BarComponentFunc(bc)
156: 	if err != nil {
157: 		return nil, fmt.Errorf(fmtw, invalid, err)
158: 	}
159: 
160: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
161: 	if err != nil {
162: 		return nil, fmt.Errorf(fmtw, invalid, err)
163: 	}
164: 
165: 	tradeFunc, err := entities.TradeComponentFunc(tc)
166: 	if err != nil {
167: 		return nil, fmt.Errorf(fmtw, invalid, err)
168: 	}
169: 
170: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
171: 
172: 	mnemonicVal := fmt.Sprintf(fmtnVal,
173: 		cfg.MinRasterValue, cfg.MaxRasterValue, minParam, maxParam, cfg.HighPassFilterCutoff, componentMnemonic)
174: 	mnemonicDC := fmt.Sprintf(fmtnDC, cfg.HighPassFilterCutoff, componentMnemonic)
175: 	mnemonicDCM := fmt.Sprintf(fmtnDCM, cfg.HighPassFilterCutoff, componentMnemonic)
176: 
177: 	// Parameter resolution: 2 samples per period (half-period stepping).
178: 	// Values slice length = (maxParam*2 - minParam*2 + 1). The first sample sits
179: 	// at minParam, the last at maxParam. Resolution satisfies:
180: 	//   min + (length-1)/resolution = max  =>  resolution = (length-1) / (max - min).
181: 	paramResolution := float64(c.FilterBankLength()-1) / (maxParam - minParam)
182: 
183: 	return &CoronaSpectrum{
184: 		mnemonic:            mnemonicVal,
185: 		description:         descrVal + mnemonicVal,
186: 		mnemonicDC:          mnemonicDC,
187: 		descriptionDC:       descrDC + mnemonicDC,
188: 		mnemonicDCM:         mnemonicDCM,
189: 		descriptionDCM:      descrDCM + mnemonicDCM,
190: 		c:                   c,
191: 		minParameterValue:   minParam,
192: 		maxParameterValue:   maxParam,
193: 		parameterResolution: paramResolution,
194: 		minRasterValue:      cfg.MinRasterValue,
195: 		maxRasterValue:      cfg.MaxRasterValue,
196: 		barFunc:             barFunc,
197: 		quoteFunc:           quoteFunc,
198: 		tradeFunc:           tradeFunc,
199: 	}, nil
200: }
201: 
202: // IsPrimed indicates whether the indicator is primed.
203: func (s *CoronaSpectrum) IsPrimed() bool {
204: 	s.mu.RLock()
205: 	defer s.mu.RUnlock()
206: 
207: 	return s.c.IsPrimed()
208: }
209: 
210: // Metadata describes the output data of the indicator.
211: func (s *CoronaSpectrum) Metadata() core.Metadata {
212: 	return core.Metadata{
213: 		Type:        core.CoronaSpectrum,
214: 		Mnemonic:    s.mnemonic,
215: 		Description: s.description,
216: 		Outputs: []outputs.Metadata{
217: 			{Kind: int(Value), Type: outputs.HeatmapType, Mnemonic: s.mnemonic, Description: s.description},
218: 			{Kind: int(DominantCycle), Type: outputs.ScalarType, Mnemonic: s.mnemonicDC, Description: s.descriptionDC},
219: 			{Kind: int(DominantCycleMedian), Type: outputs.ScalarType, Mnemonic: s.mnemonicDCM, Description: s.descriptionDCM},
220: 		},
221: 	}
222: }
223: 
224: // Update feeds the next sample to the engine and returns the heatmap column
225: // plus the current DominantCycle and DominantCycleMedian estimates.
226: //
227: // On unprimed bars the heatmap is an empty heatmap (with the indicator's
228: // parameter axis) and both scalar values are NaN. On a NaN input sample,
229: // state is left unchanged and all outputs are NaN / empty heatmap.
230: func (s *CoronaSpectrum) Update(sample float64, t time.Time) (*outputs.Heatmap, float64, float64) {
231: 	s.mu.Lock()
232: 	defer s.mu.Unlock()
233: 
234: 	if math.IsNaN(sample) {
235: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution),
236: 			math.NaN(), math.NaN()
237: 	}
238: 
239: 	primed := s.c.Update(sample)
240: 	if !primed {
241: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution),
242: 			math.NaN(), math.NaN()
243: 	}
244: 
245: 	bank := s.c.FilterBank()
246: 	values := make([]float64, len(bank))
247: 	valueMin := math.Inf(1)
248: 	valueMax := math.Inf(-1)
249: 
250: 	for i := range bank {
251: 		v := bank[i].Decibels
252: 		values[i] = v
253: 
254: 		if v < valueMin {
255: 			valueMin = v
256: 		}
257: 
258: 		if v > valueMax {
259: 			valueMax = v
260: 		}
261: 	}
262: 
263: 	heatmap := outputs.NewHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution,
264: 		valueMin, valueMax, values)
265: 
266: 	return heatmap, s.c.DominantCycle(), s.c.DominantCycleMedian()
267: }
268: 
269: // UpdateScalar updates the indicator given the next scalar sample.
270: func (s *CoronaSpectrum) UpdateScalar(sample *entities.Scalar) core.Output {
271: 	return s.updateEntity(sample.Time, sample.Value)
272: }
273: 
274: // UpdateBar updates the indicator given the next bar sample.
275: func (s *CoronaSpectrum) UpdateBar(sample *entities.Bar) core.Output {
276: 	return s.updateEntity(sample.Time, s.barFunc(sample))
277: }
278: 
279: // UpdateQuote updates the indicator given the next quote sample.
280: func (s *CoronaSpectrum) UpdateQuote(sample *entities.Quote) core.Output {
281: 	return s.updateEntity(sample.Time, s.quoteFunc(sample))
282: }
283: 
284: // UpdateTrade updates the indicator given the next trade sample.
285: func (s *CoronaSpectrum) UpdateTrade(sample *entities.Trade) core.Output {
286: 	return s.updateEntity(sample.Time, s.tradeFunc(sample))
287: }
288: 
289: func (s *CoronaSpectrum) updateEntity(t time.Time, sample float64) core.Output {
290: 	const length = 3
291: 
292: 	heatmap, dc, dcm := s.Update(sample, t)
293: 
294: 	output := make([]any, length)
295: 	output[0] = heatmap
296: 	output[1] = entities.Scalar{Time: t, Value: dc}
297: 	output[2] = entities.Scalar{Time: t, Value: dcm}
298: 
299: 	return output
300: }

(End of file - total 300 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.2s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrumoutput.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrumoutput.go</path>
<type>file</type>
<content>1: package coronaspectrum
2: 
3: import (
4: 	"bytes"
5: 	"fmt"
6: )
7: 
8: // Output describes the outputs of the indicator.
9: type Output int
10: 
11: const (
12: 	// Value is the Corona spectrum heatmap column.
13: 	Value Output = iota + 1
14: 	// DominantCycle is the weighted-center-of-gravity dominant cycle estimate.
15: 	DominantCycle
16: 	// DominantCycleMedian is the 5-sample median of DominantCycle.
17: 	DominantCycleMedian
18: 	outputLast
19: )
20: 
21: const (
22: 	valueStr               = "value"
23: 	dominantCycleStr       = "dominantCycle"
24: 	dominantCycleMedianStr = "dominantCycleMedian"
25: 	unknownStr             = "unknown"
26: )
27: 
28: // String implements the Stringer interface.
29: func (o Output) String() string {
30: 	switch o {
31: 	case Value:
32: 		return valueStr
33: 	case DominantCycle:
34: 		return dominantCycleStr
35: 	case DominantCycleMedian:
36: 		return dominantCycleMedianStr
37: 	default:
38: 		return unknownStr
39: 	}
40: }
41: 
42: // IsKnown determines if this output is known.
43: func (o Output) IsKnown() bool {
44: 	return o >= Value && o < outputLast
45: }
46: 
47: // MarshalJSON implements the Marshaler interface.
48: func (o Output) MarshalJSON() ([]byte, error) {
49: 	const (
50: 		errFmt = "cannot marshal '%s': unknown corona spectrum output"
51: 		extra  = 2   // Two bytes for quotes.
52: 		dqc    = '"' // Double quote character.
53: 	)
54: 
55: 	s := o.String()
56: 	if s == unknownStr {
57: 		return nil, fmt.Errorf(errFmt, s)
58: 	}
59: 
60: 	b := make([]byte, 0, len(s)+extra)
61: 	b = append(b, dqc)
62: 	b = append(b, s...)
63: 	b = append(b, dqc)
64: 
65: 	return b, nil
66: }
67: 
68: // UnmarshalJSON implements the Unmarshaler interface.
69: func (o *Output) UnmarshalJSON(data []byte) error {
70: 	const (
71: 		errFmt = "cannot unmarshal '%s': unknown corona spectrum output"
72: 		dqs    = "\"" // Double quote string.
73: 	)
74: 
75: 	d := bytes.Trim(data, dqs)
76: 	s := string(d)
77: 
78: 	switch s {
79: 	case valueStr:
80: 		*o = Value
81: 	case dominantCycleStr:
82: 		*o = DominantCycle
83: 	case dominantCycleMedianStr:
84: 		*o = DominantCycleMedian
85: 	default:
86: 		return fmt.Errorf(errFmt, s)
87: 	}
88: 
89: 	return nil
90: }

(End of file - total 90 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.8s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrumparams.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrumparams.go</path>
<type>file</type>
<content>1: package coronaspectrum
2: 
3: import "zpano/entities"
4: 
5: // Params describes parameters to create an instance of the CoronaSpectrum indicator.
6: type Params struct {
7: 	// MinRasterValue is the minimal raster value (z) of the heatmap, in decibels.
8: 	// Corresponds to the CoronaLowerDecibels threshold. The default value is 6.
9: 	MinRasterValue float64
10: 
11: 	// MaxRasterValue is the maximal raster value (z) of the heatmap, in decibels.
12: 	// Corresponds to the CoronaUpperDecibels threshold. The default value is 20.
13: 	MaxRasterValue float64
14: 
15: 	// MinParameterValue is the minimal ordinate (y) value of the heatmap, representing
16: 	// the minimal cycle period covered by the filter bank. The default value is 6.
17: 	MinParameterValue float64
18: 
19: 	// MaxParameterValue is the maximal ordinate (y) value of the heatmap, representing
20: 	// the maximal cycle period covered by the filter bank. The default value is 30.
21: 	MaxParameterValue float64
22: 
23: 	// HighPassFilterCutoff is the high-pass filter cutoff (de-trending period) used by
24: 	// the inner Corona engine. Suggested values are 20, 30, 100. The default value is 30.
25: 	HighPassFilterCutoff int
26: 
27: 	// BarComponent indicates the component of a bar to use when updating the indicator with a bar sample.
28: 	//
29: 	// If zero, the default (BarMedianPrice) is used, matching Ehlers' reference which operates on
30: 	// (High+Low)/2. Since this differs from the framework-wide default, it is always shown in the
31: 	// indicator mnemonic.
32: 	BarComponent entities.BarComponent
33: 
34: 	// QuoteComponent indicates the component of a quote to use when updating the indicator with a quote sample.
35: 	//
36: 	// If zero, the default (QuoteMidPrice) is used and the component is not shown in the indicator mnemonic.
37: 	QuoteComponent entities.QuoteComponent
38: 
39: 	// TradeComponent indicates the component of a trade to use when updating the indicator with a trade sample.
40: 	//
41: 	// If zero, the default (TradePrice) is used and the component is not shown in the indicator mnemonic.
42: 	TradeComponent entities.TradeComponent
43: }

(End of file - total 43 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 6.0s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona/corona.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/corona/corona.ts</path>
<type>file</type>
<content>1: import { CoronaParams, DefaultCoronaParams } from './corona-params';
2: 
3: const DEFAULT_HIGH_PASS_FILTER_CUTOFF = 30;
4: const DEFAULT_MINIMAL_PERIOD = 6;
5: const DEFAULT_MAXIMAL_PERIOD = 30;
6: const DEFAULT_DECIBELS_LOWER_THRESHOLD = 6;
7: const DEFAULT_DECIBELS_UPPER_THRESHOLD = 20;
8: 
9: const HIGH_PASS_FILTER_BUFFER_SIZE = 6;
10: const FIR_COEF_SUM = 12;
11: 
12: const DELTA_LOWER_THRESHOLD = 0.1;
13: const DELTA_FACTOR = -0.015;
14: const DELTA_SUMMAND = 0.5;
15: 
16: const DOMINANT_CYCLE_BUFFER_SIZE = 5;
17: const DOMINANT_CYCLE_MEDIAN_INDEX = 2;
18: 
19: const DECIBELS_SMOOTHING_ALPHA = 0.33;
20: const DECIBELS_SMOOTHING_ONE_MINUS = 0.67;
21: 
22: const NORMALIZED_AMPLITUDE_FACTOR = 0.99;
23: const DECIBELS_FLOOR = 0.01;
24: const DECIBELS_GAIN = 10;
25: 
26: /** Per-bin state of a single bandpass filter in the Corona bank. */
27: export interface CoronaFilter {
28:   inPhase: number;
29:   inPhasePrev: number;
30:   quadrature: number;
31:   quadraturePrev: number;
32:   real: number;
33:   realPrev: number;
34:   imaginary: number;
35:   imaginaryPrev: number;
36: 
37:   /** |Real + j·Imaginary|² of the most recent update. */
38:   amplitudeSquared: number;
39: 
40:   /** Smoothed dB value of the most recent update. */
41:   decibels: number;
42: }
43: 
44: /** Shared spectral-analysis engine consumed by the CoronaSpectrum, CoronaSignalToNoiseRatio,
45:  * CoronaSwingPosition and CoronaTrendVigor indicators. It is not an indicator on its own.
46:  *
47:  * Call _update(sample)_ once per bar; then read _isPrimed_, _dominantCycle_,
48:  * _dominantCycleMedian_, _maximalAmplitudeSquared_ and the _filterBank_ array.
49:  *
50:  * Reference: John Ehlers, "Measuring Cycle Periods", Stocks & Commodities, November 2008. */
51: export class Corona {
52:   public readonly minimalPeriod: number;
53:   public readonly maximalPeriod: number;
54:   public readonly minimalPeriodTimesTwo: number;
55:   public readonly maximalPeriodTimesTwo: number;
56:   public readonly filterBankLength: number;
57: 
58:   private readonly decibelsLowerThreshold: number;
59:   private readonly decibelsUpperThreshold: number;
60:   private readonly alpha: number;
61:   private readonly halfOnePlusAlpha: number;
62:   private readonly preCalculatedBeta: number[];
63: 
64:   public readonly filterBank: CoronaFilter[];
65:   private readonly highPassBuffer: number[];
66:   private readonly dominantCycleBuffer: number[];
67: 
68:   private samplePrev = 0;
69:   private smoothHPPrev = 0;
70:   private _maximalAmplitudeSquared = 0;
71:   private sampleCount = 0;
72:   private _dominantCycle: number;
73:   private _dominantCycleMedian: number;
74:   private _primed = false;
75: 
76:   /** Creates a Corona engine. Zero/undefined parameters are replaced with the Ehlers defaults. */
77:   public constructor(params?: CoronaParams) {
78:     const hp = params?.highPassFilterCutoff;
79:     const minP = params?.minimalPeriod;
80:     const maxP = params?.maximalPeriod;
81:     const dbLo = params?.decibelsLowerThreshold;
82:     const dbHi = params?.decibelsUpperThreshold;
83: 
84:     const cfg = {
85:       highPassFilterCutoff: hp !== undefined && hp > 0 ? hp : DEFAULT_HIGH_PASS_FILTER_CUTOFF,
86:       minimalPeriod: minP !== undefined && minP > 0 ? minP : DEFAULT_MINIMAL_PERIOD,
87:       maximalPeriod: maxP !== undefined && maxP > 0 ? maxP : DEFAULT_MAXIMAL_PERIOD,
88:       decibelsLowerThreshold: dbLo !== undefined && dbLo !== 0 ? dbLo : DEFAULT_DECIBELS_LOWER_THRESHOLD,
89:       decibelsUpperThreshold: dbHi !== undefined && dbHi !== 0 ? dbHi : DEFAULT_DECIBELS_UPPER_THRESHOLD,
90:     };
91: 
92:     if (cfg.highPassFilterCutoff < 2) {
93:       throw new Error('invalid corona parameters: HighPassFilterCutoff should be >= 2');
94:     }
95:     if (cfg.minimalPeriod < 2) {
96:       throw new Error('invalid corona parameters: MinimalPeriod should be >= 2');
97:     }
98:     if (cfg.maximalPeriod <= cfg.minimalPeriod) {
99:       throw new Error('invalid corona parameters: MaximalPeriod should be > MinimalPeriod');
100:     }
101:     if (cfg.decibelsLowerThreshold < 0) {
102:       throw new Error('invalid corona parameters: DecibelsLowerThreshold should be >= 0');
103:     }
104:     if (cfg.decibelsUpperThreshold <= cfg.decibelsLowerThreshold) {
105:       throw new Error('invalid corona parameters: DecibelsUpperThreshold should be > DecibelsLowerThreshold');
106:     }
107: 
108:     this.minimalPeriod = cfg.minimalPeriod;
109:     this.maximalPeriod = cfg.maximalPeriod;
110:     this.minimalPeriodTimesTwo = cfg.minimalPeriod * 2;
111:     this.maximalPeriodTimesTwo = cfg.maximalPeriod * 2;
112:     this.decibelsLowerThreshold = cfg.decibelsLowerThreshold;
113:     this.decibelsUpperThreshold = cfg.decibelsUpperThreshold;
114: 
115:     this.filterBankLength = this.maximalPeriodTimesTwo - this.minimalPeriodTimesTwo + 1;
116: 
117:     this.filterBank = new Array<CoronaFilter>(this.filterBankLength);
118:     for (let i = 0; i < this.filterBankLength; i++) {
119:       this.filterBank[i] = {
120:         inPhase: 0, inPhasePrev: 0,
121:         quadrature: 0, quadraturePrev: 0,
122:         real: 0, realPrev: 0,
123:         imaginary: 0, imaginaryPrev: 0,
124:         amplitudeSquared: 0,
125:         decibels: 0,
126:       };
127:     }
128: 
129:     this.highPassBuffer = new Array<number>(HIGH_PASS_FILTER_BUFFER_SIZE).fill(0);
130: 
131:     // MBST initializes the dominant cycle buffer with MaxValue sentinels.
132:     this.dominantCycleBuffer = new Array<number>(DOMINANT_CYCLE_BUFFER_SIZE).fill(Number.MAX_VALUE);
133:     this._dominantCycle = Number.MAX_VALUE;
134:     this._dominantCycleMedian = Number.MAX_VALUE;
135: 
136:     const phi = (2 * Math.PI) / cfg.highPassFilterCutoff;
137:     this.alpha = (1 - Math.sin(phi)) / Math.cos(phi);
138:     this.halfOnePlusAlpha = 0.5 * (1 + this.alpha);
139: 
140:     this.preCalculatedBeta = new Array<number>(this.filterBankLength);
141:     for (let idx = 0; idx < this.filterBankLength; idx++) {
142:       const n = this.minimalPeriodTimesTwo + idx;
143:       this.preCalculatedBeta[idx] = Math.cos((4 * Math.PI) / n);
144:     }
145:   }
146: 
147:   /** Indicates whether the engine has seen enough samples to produce meaningful output. */
148:   public isPrimed(): boolean { return this._primed; }
149: 
150:   /** The most recent weighted-center-of-gravity estimate of the dominant cycle period. */
151:   public get dominantCycle(): number { return this._dominantCycle; }
152: 
153:   /** The 5-sample median of the most recent dominant cycle estimates. */
154:   public get dominantCycleMedian(): number { return this._dominantCycleMedian; }
155: 
156:   /** The maximum amplitude-squared observed across the filter bank for the most recently
157:    * processed sample. Reset to zero at the start of every update call. */
158:   public get maximalAmplitudeSquared(): number { return this._maximalAmplitudeSquared; }
159: 
160:   /** Feeds the next sample to the engine. NaN samples are a no-op. Returns true once primed. */
161:   public update(sample: number): boolean {
162:     if (Number.isNaN(sample)) {
163:       return this._primed;
164:     }
165: 
166:     this.sampleCount++;
167: 
168:     // First sample: store as prior reference and return without further processing.
169:     if (this.sampleCount === 1) {
170:       this.samplePrev = sample;
171:       return false;
172:     }
173: 
174:     // Step 1: High-pass filter.
175:     const hp = this.alpha * this.highPassBuffer[HIGH_PASS_FILTER_BUFFER_SIZE - 1]
176:       + this.halfOnePlusAlpha * (sample - this.samplePrev);
177:     this.samplePrev = sample;
178: 
179:     for (let i = 0; i < HIGH_PASS_FILTER_BUFFER_SIZE - 1; i++) {
180:       this.highPassBuffer[i] = this.highPassBuffer[i + 1];
181:     }
182:     this.highPassBuffer[HIGH_PASS_FILTER_BUFFER_SIZE - 1] = hp;
183: 
184:     // Step 2: 6-tap FIR smoothing {1, 2, 3, 3, 2, 1} / 12.
185:     const smoothHP = (this.highPassBuffer[0]
186:       + 2 * this.highPassBuffer[1]
187:       + 3 * this.highPassBuffer[2]
188:       + 3 * this.highPassBuffer[3]
189:       + 2 * this.highPassBuffer[4]
190:       + this.highPassBuffer[5]) / FIR_COEF_SUM;
191: 
192:     // Step 3: Momentum.
193:     const momentum = smoothHP - this.smoothHPPrev;
194:     this.smoothHPPrev = smoothHP;
195: 
196:     // Step 4: Adaptive delta.
197:     let delta = DELTA_FACTOR * this.sampleCount + DELTA_SUMMAND;
198:     if (delta < DELTA_LOWER_THRESHOLD) {
199:       delta = DELTA_LOWER_THRESHOLD;
200:     }
201: 
202:     // Step 5: Filter-bank update.
203:     this._maximalAmplitudeSquared = 0;
204:     for (let idx = 0; idx < this.filterBankLength; idx++) {
205:       const n = this.minimalPeriodTimesTwo + idx;
206:       const nf = n;
207: 
208:       const gamma = 1 / Math.cos((8 * Math.PI * delta) / nf);
209:       const a = gamma - Math.sqrt(gamma * gamma - 1);
210: 
211:       const quadrature = momentum * (nf / (4 * Math.PI));
212:       const inPhase = smoothHP;
213: 
214:       const halfOneMinA = 0.5 * (1 - a);
215:       const beta = this.preCalculatedBeta[idx];
216:       const betaOnePlusA = beta * (1 + a);
217: 
218:       const f = this.filterBank[idx];
219: 
220:       const real = halfOneMinA * (inPhase - f.inPhasePrev) + betaOnePlusA * f.real - a * f.realPrev;
221:       const imag = halfOneMinA * (quadrature - f.quadraturePrev) + betaOnePlusA * f.imaginary - a * f.imaginaryPrev;
222: 
223:       const ampSq = real * real + imag * imag;
224: 
225:       f.inPhasePrev = f.inPhase;
226:       f.inPhase = inPhase;
227:       f.quadraturePrev = f.quadrature;
228:       f.quadrature = quadrature;
229:       f.realPrev = f.real;
230:       f.real = real;
231:       f.imaginaryPrev = f.imaginary;
232:       f.imaginary = imag;
233:       f.amplitudeSquared = ampSq;
234: 
235:       if (ampSq > this._maximalAmplitudeSquared) {
236:         this._maximalAmplitudeSquared = ampSq;
237:       }
238:     }
239: 
240:     // Step 6: dB normalization and dominant-cycle weighted average.
241:     let numerator = 0;
242:     let denominator = 0;
243:     this._dominantCycle = 0;
244: 
245:     for (let idx = 0; idx < this.filterBankLength; idx++) {
246:       const f = this.filterBank[idx];
247: 
248:       let decibels = 0;
249:       if (this._maximalAmplitudeSquared > 0) {
250:         const normalized = f.amplitudeSquared / this._maximalAmplitudeSquared;
251:         if (normalized > 0) {
252:           const arg = (1 - NORMALIZED_AMPLITUDE_FACTOR * normalized) / DECIBELS_FLOOR;
253:           if (arg > 0) {
254:             decibels = DECIBELS_GAIN * Math.log10(arg);
255:           }
256:         }
257:       }
258: 
259:       decibels = DECIBELS_SMOOTHING_ALPHA * decibels + DECIBELS_SMOOTHING_ONE_MINUS * f.decibels;
260:       if (decibels > this.decibelsUpperThreshold) {
261:         decibels = this.decibelsUpperThreshold;
262:       }
263:       f.decibels = decibels;
264: 
265:       if (decibels <= this.decibelsLowerThreshold) {
266:         const n = this.minimalPeriodTimesTwo + idx;
267:         const adjusted = this.decibelsUpperThreshold - decibels;
268:         numerator += n * adjusted;
269:         denominator += adjusted;
270:       }
271:     }
272: 
273:     if (denominator !== 0) {
274:       this._dominantCycle = (0.5 * numerator) / denominator;
275:     }
276:     if (this._dominantCycle < this.minimalPeriod) {
277:       this._dominantCycle = this.minimalPeriod;
278:     }
279: 
280:     // Step 7: 5-sample median of dominant cycle.
281:     for (let i = 0; i < DOMINANT_CYCLE_BUFFER_SIZE - 1; i++) {
282:       this.dominantCycleBuffer[i] = this.dominantCycleBuffer[i + 1];
283:     }
284:     this.dominantCycleBuffer[DOMINANT_CYCLE_BUFFER_SIZE - 1] = this._dominantCycle;
285: 
286:     const sorted = [...this.dominantCycleBuffer].sort((a, b) => a - b);
287:     this._dominantCycleMedian = sorted[DOMINANT_CYCLE_MEDIAN_INDEX];
288:     if (this._dominantCycleMedian < this.minimalPeriod) {
289:       this._dominantCycleMedian = this.minimalPeriod;
290:     }
291: 
292:     if (this.sampleCount < this.minimalPeriodTimesTwo) {
293:       return false;
294:     }
295:     this._primed = true;
296:     return true;
297:   }
298: }
299: 
300: // Re-exports avoid unused-import warning in bundlers.
301: export { DefaultCoronaParams };

(End of file - total 301 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.3s)

_Thinking:_

 I'm checking how existing components like Heatmap handle their output enums and making sure I'm following the same mnemonic formatting conventions.

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

---

## Assistant (Build · claude-opus-4.7 · 4.7s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/component-triple-mnemonic.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/component-triple-mnemonic.ts</path>
<type>file</type>
<content>1: import { BarComponent, DefaultBarComponent, barComponentMnemonic } from '../../entities/bar-component';
2: import { QuoteComponent, DefaultQuoteComponent, quoteComponentMnemonic } from '../../entities/quote-component';
3: import { TradeComponent, DefaultTradeComponent, tradeComponentMnemonic } from '../../entities/trade-component';
4: 
5: /**
6:  * Function to calculate mnemonic for a component triple.
7:  *
8:  * Default components are omitted from the mnemonic: the same indicator always
9:  * produces the same mnemonic regardless of whether defaults were explicitly
10:  * specified or left undefined.
11:  */
12: export const componentTripleMnemonic = (barComponent?: BarComponent, quoteComponent?: QuoteComponent, tradeComponent?: TradeComponent): string => {
13:     let str = '';
14: 
15:     if (barComponent !== undefined && barComponent !== DefaultBarComponent) {
16:         str += ', ' + barComponentMnemonic(barComponent);
17:     }
18: 
19:     if (quoteComponent !== undefined && quoteComponent !== DefaultQuoteComponent) {
20:         str += ', ' + quoteComponentMnemonic(quoteComponent);
21:     }
22: 
23:     if (tradeComponent !== undefined && tradeComponent !== DefaultTradeComponent) {
24:         str += ', ' + tradeComponentMnemonic(tradeComponent);
25:     }
26: 
27:     return str;
28: };

(End of file - total 28 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.5s)

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

---

## Assistant (Build · claude-opus-4.7 · 3.9s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/outputs/output-type.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/outputs/output-type.ts</path>
<type>file</type>
<content>1: /** Enumerates indicator output types. */
2: export enum OutputType {
3: 
4:   /** Holds a single value. */
5:   Scalar,
6: 
7:   /** Holds two values representing lower and upper lines of a band. */
8:   Band,
9: 
10:   /** Holds an array of values representing a heat-map column. */
11:   Heatmap
12: }

(End of file - total 12 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 13.6s)

_Thinking:_

 I'm checking how `%g` formatting works in Go for the mnemonic function—it uses the shortest representation, so values like `6`, `20`, `6`, `30` should format correctly as integers when they don't need decimal places. Now I need to verify how JavaScript handles the same conversion to make sure the behavior matches.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/indicator-type.ts",
  "offset": 1,
  "limit": 30
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/indicator-type.ts</path>
<type>file</type>
<content>1: /** Enumerates indicator types. */
2: export enum IndicatorType {
3: 
4:   /** Identifies the __Simple Moving Average__ (SMA) indicator. */
5:   SimpleMovingAverage,
6: 
7:   /** Identifies the __Weighted Moving Average__ (WMA) indicator. */
8:   WeightedMovingAverage,
9: 
10:   /** Identifies the __Triangular Moving Average__ (TRIMA) indicator. */
11:   TriangularMovingAverage,
12: 
13:   /** Identifies the __Exponential Moving Average__ (EMA) indicator. */
14:   ExponentialMovingAverage,
15: 
16:   /** Identifies the __Double Exponential Moving Average__ (DEMA) indicator. */
17:   DoubleExponentialMovingAverage,
18: 
19:   /** Identifies the __Triple Exponential Moving Average__ (TEMA) indicator. */
20:   TripleExponentialMovingAverage,
21: 
22:   /** Identifies the __T2 Exponential Moving Average__ (T2EMA) indicator. */
23:   T2ExponentialMovingAverage,
24: 
25:   /** Identifies the __T3 Exponential Moving Average__ (T3EMA) indicator. */
26:   T3ExponentialMovingAverage,
27: 
28:   /** Identifies the __Kaufman Adaptive Moving Average__ (KAMA) indicator. */
29:   KaufmanAdaptiveMovingAverage,
30: 

(Showing lines 1-30 of 206. Use offset=31 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.0s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/indicator-type.ts",
  "offset": 180
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/indicator-type.ts</path>
<type>file</type>
<content>180: 
181:   /** Identifies the Ehlers __Sine Wave__ (SW) indicator, exposing sine value, lead sine, band, dominant cycle period and phase. */
182:   SineWave,
183: 
184:   /** Identifies the Ehlers __Hilbert Transformer Instantaneous Trend Line__ (HTITL) indicator, exposing trend value and dominant cycle period. */
185:   HilbertTransformerInstantaneousTrendLine,
186: 
187:   /** Identifies the Ehlers __Trend / Cycle Mode__ (TCM) indicator, exposing the trend/cycle value (+1 in trend, −1 in cycle),
188:    * trend/cycle mode flags, instantaneous trend line, sine wave, lead sine wave, dominant cycle period and phase. */
189:   TrendCycleMode,
190: 
191:   /** Identifies the Ehlers __Corona Spectrum__ (CSPECT) heatmap indicator, exposing the dB heatmap column,
192:    * the weighted dominant cycle estimate and its 5-sample median. */
193:   CoronaSpectrum,
194: 
195:   /** Identifies the Ehlers __Corona Signal To Noise Ratio__ (CSNR) heatmap indicator, exposing the intensity
196:    * raster heatmap column and the current SNR mapped into the parameter range. */
197:   CoronaSignalToNoiseRatio,
198: 
199:   /** Identifies the Ehlers __Corona Swing Position__ (CSWING) heatmap indicator, exposing the intensity raster
200:    * heatmap column and the current swing position mapped into the parameter range. */
201:   CoronaSwingPosition,
202: 
203:   /** Identifies the Ehlers __Corona Trend Vigor__ (CTV) heatmap indicator, exposing the intensity raster heatmap
204:    * column and the current trend vigor scaled into the parameter range. */
205:   CoronaTrendVigor,
206: }

(End of file - total 206 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.3s)

_Thinking:_

 Now I'm reviewing the Scalar, Bar, Quote, and Trade entity definitions to understand their structure and relationships.

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

## Assistant (Build · claude-opus-4.7 · 3.6s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/entities/bar.ts",
  "limit": 30
}
```

**Output:**
```
<path>/home/dev/zpano/ts/entities/bar.ts</path>
<type>file</type>
<content>1: /** An [open, high, low, close, volume] bar. */
2: export class Bar {
3:   /** The date and time.
4:    *
5:    * For _ohlcv_ bar entities it corresponds to the closing time, so that an _ohlcv_ bar accumulates lower-level entities
6:    * up to the closing date and time.
7:    */
8:   time!: Date;
9: 
10:   /** The opening price. */
11:   open!: number;
12: 
13:   /** The highest price. */
14:   high!: number;
15: 
16:   /** The lowest price. */
17:   low!: number;
18: 
19:   /** The closing price. */
20:   close!: number;
21: 
22:   /** The volume. */
23:   volume!: number;
24: 
25:   constructor(data?: any) {
26:     if (data) {
27:       for (const property in data) {
28:         if (Object.prototype.hasOwnProperty.call(data, property)) {
29:           (this as any)[property] = data[property];
30:         }

(Showing lines 1-30 of 64. Use offset=31 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.7s)

Now let me check the TCM spec for structure/test data:

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts</path>
<type>file</type>
<content>1: import { } from 'jasmine';
2: 
3: import { Bar } from '../../../entities/bar';
4: import { Quote } from '../../../entities/quote';
5: import { Scalar } from '../../../entities/scalar';
6: import { Trade } from '../../../entities/trade';
7: import { IndicatorType } from '../../core/indicator-type';
8: import { OutputType } from '../../core/outputs/output-type';
9: import { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';
10: import { TrendCycleMode } from './trend-cycle-mode';
11: import { TrendCycleModeOutput } from './trend-cycle-mode-output';
12: 
13: /* eslint-disable max-len */
14: // Input from MBST TrendCycleModeTest.cs (TA-Lib test_MAMA.xsl, Price). 252 entries.
15: const input = [
16:   92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,
17:   94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,
18:   88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,
19:   85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,
20:   83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,
21:   89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,
22:   89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,
23:   88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,
24:   103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,
25:   120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,
26:   114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,
27:   114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,
28:   124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,
29:   137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,
30:   123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,
31:   122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,
32:   123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,
33:   130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,
34:   127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,
35:   121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,
36:   106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,
37:   95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,
38:   94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,
39:   103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,
40:   106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,
41:   109.5300, 108.0600,
42: ];
43: 
44: const expectedPeriod = [
45:   0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
46:   0, 0, 0.39600000000000, 0.97812000000000, 1.62158040000000, 2.25545086800000, 2.84234568156000, 3.36868456664520, 3.86776291565229, 4.36321983508703,
47:   4.87235783926831, 5.40838035704577, 5.98190550443027, 6.60199641969884, 7.27686930610184, 8.01438731048222, 8.82241286095647, 9.70906731606755, 10.68293087091460, 11.75320502957710,
48:   12.92985285048740, 14.22372743856440, 15.55272286275450, 16.77503611571540, 17.85814025111630, 18.72970387649220, 19.30387978646140, 19.63314544969620, 19.86256979430250, 20.09030968609300,
49:   20.24817834009410, 20.31132870798190, 20.52152604110820, 21.27119054536480, 22.10966835167300, 22.28715460952700, 21.91280773257140, 21.23923470724180, 20.70813161651310, 20.20449150221090,
50:   19.52863321263000, 18.73709250583170, 17.96311275281150, 17.33367762545960, 16.91743352044750, 16.64300564862120, 16.41952162419500, 16.27464914327850, 16.26425245094380, 16.33321577028600,
51:   16.39265551523990, 16.39976990202790, 16.38221107536320, 16.37405059271910, 16.35102942468120, 16.26839438425590, 16.12432207371240, 15.99529098667630, 15.96458956780100, 16.07977539207760,
52:   16.38360881255670, 16.79746341307210, 17.18753188776420, 17.58524022168910, 18.05888760471740, 18.46077773999830, 18.78691120238400, 19.07789869381110, 19.11803073417110, 18.72675385299730,
53:   18.07403190737810, 17.72999456892580, 18.00699920187680, 18.06680349806270, 17.73551482016550, 17.28467833183610, 16.97456900115070, 16.89386283663200, 17.00556420464730, 17.21986959021550,
54:   17.48251598471900, 17.79647268844360, 18.15809655229470, 18.56044162987590, 19.03705300462320, 19.61779465906120, 20.13838368155990, 20.58144279802850, 20.93554178712450, 21.09578565733870,
55:   21.19426268582890, 21.35270550953270, 21.46806615214910, 21.43420235778580, 21.27320458618770, 20.98617884905010, 20.62345107800030, 20.32165030848570, 20.09921951571820, 19.88214300840560,
56:   19.67081622699810, 19.55217428481160, 19.60485773311710, 19.77836095343260, 19.77886122563300, 19.59009982815140, 19.54609435364200, 19.77945658439880, 20.20526697824140, 20.80572859375930,
57:   21.45882440191380, 21.50916115262280, 21.07219135457730, 20.33979206665380, 19.60807769029340, 19.15831017112920, 19.03006205140340, 19.23359250887840, 19.84206353515510, 20.83692898803630,
58:   22.25776348341490, 23.50933063567320, 24.02857349775940, 24.28548086650010, 24.74576845262060, 25.45685387492870, 26.31998583396390, 27.14553013410700, 27.80677101851790, 28.50146147525040,
59:   29.16835938704370, 29.38723525724370, 29.55886298198770, 30.43981360336700, 30.70779370313880, 30.20667454311960, 29.23518282361370, 28.00037502954910, 26.95505291681000, 26.22399862702800,
60:   25.67716809996900, 25.19893752937060, 24.76924271120940, 24.40654607774420, 24.12997738279210, 24.02648590415090, 24.17912316847620, 24.26552607123530, 24.07565548132200, 23.81050493977940,
61:   23.75771490624360, 24.01627030476950, 24.42884190933990, 24.55867905189440, 24.41978729840000, 24.33536819272640, 24.11887925396970, 23.53741527509780, 22.66734716257270, 21.70419061052260,
62:   20.78848949032480, 19.92593130809770, 19.09528115584620, 18.35405205698280, 17.81539769318840, 17.53491540732180, 17.59552736216070, 18.09376127214910, 18.69300796204700, 19.10361709066390,
63:   19.40368660687600, 19.79324964337850, 20.14261316711870, 20.30292592814370, 20.37642955508450, 20.42856373321320, 20.36417897031590, 20.26870923265670, 20.31691792510900, 20.52593924664440,
64:   20.95797078839970, 21.70315565998060, 22.68588957914270, 23.95566588814480, 25.30036991408680, 26.49222048853470, 28.24485763802440, 30.46863151925310, 31.19661794415910, 30.97271495031300,
65:   30.15801520320610, 29.52193986806340, 28.48090879451130, 27.20913817575940, 25.84740758865390, 24.75875079095690, 24.57820671512040, 25.25622655282780, 26.58938264946150, 28.44936832011270,
66:   30.75900691394640, 31.63120735338530, 31.95156902113430, 32.19329221743080, 32.18129930292270, 31.78927079951340, 30.94427836437330, 29.74153261553520, 28.44319750131350, 27.27756983469050,
67:   26.30928991862760, 25.59706087830910, 25.19354035279110, 24.98183319418390, 24.66611779383150, 24.13629363553260, 23.59372342374540, 23.45943359521940, 24.13462330023790, 25.42868068174450,
68:   27.22154743441240, 28.85990121754770, 29.25658159944000, 28.86760790158470, 28.27077502042400, 27.83957963686970, 27.56292753489200, 27.31665028261770, 27.11537844471070, 27.05619511102920,
69:   26.72669604084850, 25.93839467294110, 24.88015320695530, 23.98089561843900, 23.51115215671300, 23.02173482203020, 22.29674643126940, 21.42162141795630, 20.54863761751100, 19.78167187971360,
70:   19.14387361712880, 18.61396641752300,
71: ];
72: 
73: const expectedPhase = [
74:   Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN,
75:   Number.NaN, Number.NaN, Number.NaN,
76:   639.09090909090900, 98.05299963194700, 132.00564338345400, 69.61331949528320, 72.53929158292410, 49.31988383698000, 70.57064052204580, 95.44397344939550, 15.53098102018940, -7.59977335936253,
77:   -0.43908926482948, 0.75159304711349, 6.81682854281200, 10.38041464097730, 14.60138862232650, 23.61952231391600, 34.25173964222910, 45.61939612889610, 54.93203281582900, 62.34730431858280,
78:   59.09332873285360, 64.74787861710400, 71.38960942346380, 78.26591034671290, 95.01873223899610, 100.81260420916600, 122.15965196914300, 155.92351856084900, 203.60327223472200, 237.36293513970900,
79:   244.38539212971100, 260.58654095568100, 254.66267427143100, 253.71116813655900, 126.51252837026600, 108.91084071926300, 120.62704116849900, 135.00432720483700, 148.37778407551400, 166.73764548652900,
80:   192.70016208445300, 227.19271648131900, 250.93069421530800, 272.91238631233800, 306.14557952390500, -30.56440912946640, -7.29779366562593, 15.79435681229920, 37.27143784018100, 57.28482204365770,
81:   77.89434659794900, 100.26640790428300, 122.46792347519500, 143.34860922315100, 161.97099218663000, 179.04173949124300, 194.28337763382200, 207.12978658551400, 219.12056620855700, 228.99420937560000,
82:   246.39888033834300, 259.83330071373300, 279.43099672242300, 300.38982045382800, -42.95237520761510, 10.71059944380160, 67.91671263897760, 95.69661875079390, 116.17343846855600, 131.69663648791300,
83:   145.22435488266800, 157.96979483912800, 170.90824231177600, 190.12565895164300, 201.99948636973700, 211.84934448916700, 219.50941069822200, 223.55536694633000, 217.70267385839600, 208.61154857738600,
84:   187.20930268786500, 183.53325881754400, 173.77461459542900, 177.09324308654900, 177.36771678646600, 183.46254384786600, 190.38499136923400, 200.15247884572800, 210.26397611554500, 219.92876325408700,
85:   230.19440003342700, 241.78128922383700, 255.09398089013500, 269.90233026686000, 287.31443195349700, -22.67219169276530, 21.31042453594620, 55.99561073880190, 76.28592721136950, 93.25596317796150,
86:   111.58409337397600, 131.21106018529500, 148.25324978238800, 161.34668836868300, 167.25425018604100, 166.56119294454900, 164.79606018404900, 165.59339091940400, 167.71720854219100, 171.16337795073400,
87:   177.90397416349400, 180.76168462321900, 181.18022018527700, 182.90279528585400, 185.03420448405800, 187.45543006582800, 185.33720976352800, 183.30316508274000, 181.31467258646500, 177.91862390253700,
88:   179.98928550766400, 182.00429308763000, 183.62347330064100, 185.57299458485300, 189.35191538410900, 194.23220405450000, 200.11245439803000, 206.61598766532100, 214.60761785986900, 223.17826798412900,
89:   229.59983294953500, 238.43424165042300, 244.29554481830400, 259.55186325451500, 277.88404982202100, 297.46674550039700, -42.41381860502280, -22.05561416691630, -10.76874195668260, 4.55423903808060,
90:   13.05706378617280, 25.49753552893060, 31.99805128632640, 38.60334291217220, 45.70890152749460, 53.48688015996070, 60.82873486408750, 67.21350248296350, 74.98104788942620, 84.48479832948730,
91:   97.41668808537870, 101.86825371917200, 131.27099687518600, 148.43969963128300, 169.23915874881900, 186.79876808562800, 222.27683786450500, 226.80174114624100, 148.12023947867300, 119.78684089821000,
92:   123.91411010078200, 136.35086145099600, 148.39281918279000, 160.94079449625800, 171.89311802023800, 182.21130213571000, 189.94824024493000, 202.58335256387900, 215.72849327557900, 224.87477584909900,
93:   239.93794152235800, 257.07736337068300, 274.90021437724400, 292.81710751479900, 310.18755119542100, -34.09748283129430, -20.55209191423000, -17.48926436788570, -6.64084597196063, -3.41661969757843,
94:   -1.11706604110969, -0.61559202318233, -0.58343503934742, -0.97020892968976, -7.33789387098142, -13.71942088996000, -14.16303590443250, -9.85074940762922, -2.93111556519989, 0.35846302482548,
95:   -0.27797938543370, -4.11254536082527, -7.19786584394603, -8.12930435521150, -7.23274284499956, -5.60008181003320, -3.98246383052538, -1.93459828828531, -0.91376116945821, 1.11347590999549,
96:   3.48574296192987, 5.87739974191743, 8.51611669495514, 11.77045158406290, 16.98321519660290, 23.12127015453780, 32.37560208179040, 38.09489298723020, 44.37798569415560, 48.60625731428030,
97:   57.00174598372450, 65.55585833338270, 77.53688240972470, 96.95830008657520, 111.55989345666400, 124.32277400743800, 131.41492641407500, 137.73657404096000, 141.46543104438800, 142.80498887855200,
98:   149.73966957805500, 153.53899811794900, 157.79183782289500, 159.94501203849600, 162.96541156987000, 170.96418133015300, 176.71998519472000, 182.65581927371500, 189.05283471879000, 197.04766728263800,
99:   206.92813680000000, 217.32451174036000, 224.38800791349600, 232.20526011220900, 246.06238925337200, 271.09605242709400, 289.11924018406000, -31.18231981512890, 23.26913353342980, 47.27652706672060,
100: ];
101: 
102: const expectedSine = [
103:   Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN,
104:   Number.NaN, Number.NaN, Number.NaN,
105:   -0.9874388886763950, 0.9901389074597280, 0.7430789154373040, 0.9373629963767220, 0.9539229407379340, 0.7583605937417590, 0.9430523282622310, 0.9954894450161860, 0.2677593920226290, -0.1322524693787770,
106:   -0.0076634783681766, 0.0131173971015221, 0.1186956105503800, 0.1801829214581150, 0.2520928115991770, 0.4006612403521310, 0.5628300247542610, 0.7147094931388430, 0.8184710623910560, 0.8857771050315170,
107:   0.8580051054352940, 0.9044393510772560, 0.9477105512269880, 0.9791019835938800, 0.9961661502181070, 0.9822460058918840, 0.8465682109433450, 0.4079557291761370, -0.4004013665430420, -0.8421036878603060,
108:   -0.9017223344864390, -0.9865337683617420, -0.9643853118844240, -0.9598599815079740, 0.8037267766705520, 0.9460240546952540, 0.8605016852751490, 0.7070533756573690, 0.5243161160505560, 0.2294102723810260,
109:   -0.2198489640488240, -0.7336434870569610, -0.9451240721288890, -0.9987083965673280, -0.8075209150605320, -0.5085066436721070, -0.1270264128351010, 0.2721854747475370, 0.6055917784886910, 0.8413676398899490,
110:   0.9777625488346960, 0.9839896992562860, 0.8436921159626520, 0.5969447127483850, 0.3094984574050860, 0.0167240212732023, -0.2467178763145330, -0.4560076441356610, -0.6309543271436050, -0.7546432714599400,
111:   -0.9163549058535820, -0.9842983646399860, -0.9864836587318940, -0.8626035608908470, -0.6813902164376480, 0.1858483908516400, 0.9266383325265240, 0.9950614294774160, 0.8974629492620320, 0.7466772329212920,
112:   0.5703644678660200, 0.3750953336668390, 0.1580160207328650, -0.1758076016904010, -0.3745982816248290, -0.5276875478007350, -0.6362049492364280, -0.6890552092033720, -0.6115639640491650, -0.4788688150780810,
113:   -0.1254943141670760, -0.0616279223840421, 0.1084398125157010, 0.0507107190849194, 0.0459258492372133, -0.0603960127005548, -0.1802614924906120, -0.3445196936393600, -0.5039846767879240, -0.6418346779779860,
114:   -0.7682209569752370, -0.8811490866996920, -0.9663490613441720, -0.9999985470701830, -0.9546858649139780, -0.3854582462377580, 0.3634207386677970, 0.8289947319983170, 0.9714909193370010, 0.9983857647297360,
115:   0.9298786498974830, 0.7522877436715410, 0.5261656913352240, 0.3198410350076260, 0.2206250837743810, 0.2324067219830480, 0.2622555352188150, 0.2488016117878720, 0.2127369252136960, 0.1536174562004720,
116:   0.0365743929645966, -0.0132935129731492, -0.0205972714630062, -0.0506416644795282, -0.0877504363551195, -0.1297549156821220, -0.0930172242876989, -0.0576191763867261, -0.0229433518557876, 0.0363188766621527,
117:   0.0001870031678450, -0.0349743794742610, -0.0631993922084072, -0.0971138050129020, -0.1624979390656570, -0.2458522401310880, -0.3438638176192820, -0.4480085728769030, -0.5679531815591490, -0.6842705623709480,
118:   -0.7615364178876680, -0.8520399314791350, -0.9010432983130380, -0.9834194615045590, -0.9905476870988080, -0.8872786828920670, -0.6744804687446460, -0.3755063881128740, -0.1868453942460660, 0.0794027920237213,
119:   0.2259213674108930, 0.4304722733878400, 0.5298904205645140, 0.6239251933219030, 0.7158012315374360, 0.8037206344012380, 0.8731666378756390, 0.9219544489912210, 0.9658401621307640, 0.9953707336137810,
120:   0.9916336070524670, 0.9786230879164460, 0.7515981300962940, 0.5233956277754490, 0.1867099275394900, -0.1183826185655860, -0.6727134585677300, -0.7289894296051810, 0.5281384062949680, 0.8678795703185170,
121:   0.8298749053132090, 0.6902403611717530, 0.5240926478571590, 0.3265450140990430, 0.1410201458363470, -0.0385849224232995, -0.1727584531294190, -0.3840270653389320, -0.5839449895165980, -0.7055596595779030,
122:   -0.8654833335157070, -0.9746729156329320, -0.9963449765889340, -0.9217474051403660, -0.7639362508931000, -0.5606026149454520, -0.3510588357296890, -0.3005270942524180, -0.1156452933894830, -0.0595959289499457,
123:   -0.0194952452596040, -0.0107439009455394, -0.0101826864310280, -0.0169325310282176, -0.1277205927832380, -0.2371674472604240, -0.2446819018845580, -0.1710822516700410, -0.0511353061733825, 0.0062563192154928,
124:   -0.0048516364950122, -0.0717158401086350, -0.1252962791394740, -0.1414075684951130, -0.1259001773043880, -0.0975843207989124, -0.0694511521424164, -0.0337586943638060, -0.0159474649423128, 0.0194325975213332,
125:   0.0608001695138442, 0.1024001693533800, 0.1480876049487220, 0.2039912059729780, 0.2920915426229240, 0.3926785587852390, 0.5354672113127650, 0.6169657298756830, 0.6993887726683350, 0.7501832875129180,
126:   0.8386871644310250, 0.9103651277237890, 0.9764351312310820, 0.9926345853646890, 0.9300339420371430, 0.8258742381128520, 0.7499387624704700, 0.6725402422076670, 0.6229867037490120, 0.6045297568333980,
127:   0.5039297182908610, 0.4455885766999630, 0.3779726796046290, 0.3429218283962470, 0.2929489553699220, 0.1570518914741900, 0.0572157944020108, -0.0463361936582273, -0.1573451851308570, -0.2931672021738570,
128:   -0.4528725984930980, -0.6063286567677410, -0.6995137850617610, -0.7902112777605310, -0.9139878106324440, -0.9998170324167670, -0.9448389776183910, -0.5177630386017770, 0.3950506584043580, 0.7346367050507850,
129: ];
130: 
131: const expectedSineLead = [
132:   Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN,
133:   Number.NaN, Number.NaN, Number.NaN,
134:   -0.5865008756606930, 0.6010760134973850, 0.0522375953518313, 0.9091393120803100, 0.8866939724449600, 0.9971590529781770, 0.9020538171846020, 0.6368324474361340, 0.8706218323553310, 0.6073789821251400,
135:   0.7016671195580980, 0.7163213444559970, 0.7860384941578910, 0.8229422149880850, 0.8625259332042720, 0.9311800857903170, 0.9824560606058480, 0.9999415670942400, 0.9850130504072370, 0.9545149566415440,
136:   0.9699003741647460, 0.9411885260170180, 0.8957923799982550, 0.8361338691916270, 0.6425371253442400, 0.5619014189746250, 0.2222351486408030, -0.3571214380720770, -0.9310766529205810, -0.9768109870729720,
137:   -0.9433073135761630, -0.8132374819975520, -0.8689541000414960, -0.8770525415166500, 0.1475931480924170, 0.4397692495572380, 0.2482327294712000, -0.0000755239717352, -0.2313707009630830, -0.5260305525740280,
138:   -0.8452633448517760, -0.9992677882479890, -0.8993236485622310, -0.6702662015228300, -0.1539244027456790, 0.2492915015754510, 0.6115575080093740, 0.8728740226620090, 0.9909162839717420, 0.9771019730335230,
139:   0.8396734557306670, 0.5697614425887530, 0.2169861504951610, -0.1451956537134670, -0.4535393411417920, -0.6951822190514210, -0.8597041192450010, -0.9517540619903210, -0.9947396502168260, -0.9975710951353530,
140:   -0.9310629460274820, -0.8208173676831810, -0.5816830016568750, -0.2522412838591280, 0.0357301876330212, 0.8262025299696030, 0.9210718625890570, 0.6334265388777150, 0.3227045140978610, 0.0576226338781002,
141:   -0.1775030791814070, -0.3902458029200550, -0.5864888795958400, -0.8204080195335590, -0.9205013506909260, -0.9737752029989330, -0.9954119273819820, -0.9996821539441930, -0.9919003713144100, -0.9593708639942700,
142:   -0.7902545151018210, -0.7493402295361840, -0.6262584572592140, -0.6703391127989300, -0.6738861991115050, -0.7485223840691900, -0.8229875936131110, -0.9074292725287760, -0.9671080150009190, -0.9960855660349680,
143:   -0.9958932520932240, -0.9574138340514830, -0.8652041011461150, -0.7083111287566420, -0.4646190135134790, 0.3799051621364850, 0.9157357095569460, 0.9816417978833290, 0.8545864068220020, 0.6658040149144910,
144:   0.3974026651162090, 0.0660812869143209, -0.2292556000437840, -0.4438015597350790, -0.5336772503175370, -0.5234089018965820, -0.4969142899048420, -0.5089421255844810, -0.5404930402692640, -0.5900897578798300,
145:   -0.6807716781362360, -0.7164442325283580, -0.7215212414254330, -0.7420085481569570, -0.7664280384961050, -0.7928795495979390, -0.7698142295937170, -0.7466749292126270, -0.7231440467849940, -0.6809589449044260,
146:   -0.7069745376146330, -0.7314048006899320, -0.7503819371382670, -0.7724343179777090, -0.8126119368714450, -0.8592475635206920, -0.9071355135227490, -0.9489640525891650, -0.9835954633562350, -0.9994945745179860,
147:   -0.9967791122784410, -0.9726372051055730, -0.9438266487324060, -0.8236131488885440, -0.6034299982731380, -0.3012592866284200, 0.0451220549347129, 0.3898374562298360, 0.5625345127655470, 0.7610204246544270,
148:   0.8485754490678880, 0.9426271321408510, 0.9743624133056830, 0.9937744211043730, 0.9999234595478280, 0.9890496834828740, 0.9620813190845490, 0.9257815160887320, 0.8661907447689560, 0.7717933199927350,
149:   0.6099143741297350, 0.5465660379422910, 0.0650374448708587, -0.2324218737852860, -0.5626485140051470, -0.7858435966184150, -0.9988707502989160, -0.9995056053709990, -0.2269953459568470, 0.2624108067271960,
150:   0.1922802999868170, -0.0235747958138623, -0.2316259847404300, -0.4374421611852380, -0.6003241683068820, -0.7338638764656210, -0.8186335536900920, -0.9244352736103260, -0.9869359601928300, -0.9999976116346720,
151:   -0.9662055929589490, -0.8473318021865150, -0.6441207677355740, -0.3775643212631200, -0.0838943519776584, 0.1891385830833930, 0.4138657569596380, 0.4619148062355390, 0.6205889300081480, 0.6637092720958960,
152:   0.6931871751689060, 0.6994688836282570, 0.6998698746833480, 0.6950322988997850, 0.6110036127434860, 0.5192294559798360, 0.5125969121038930, 0.5757083087460280, 0.6700235759926480, 0.7115168281729820,
153:   0.7036678339992160, 0.6545752979714670, 0.6129364849611150, 0.6000111421090120, 0.6124554033545860, 0.6347294098505560, 0.6562899876128100, 0.6828327376881520, 0.6957402985880160, 0.7207141791649000,
154:   0.7487908163625680, 0.7757975773123150, 0.8040241461526770, 0.8364818144534760, 0.8828100232401670, 0.9279746557928580, 0.9758237828336110, 0.9927466296862280, 0.9999410720652520, 0.9980198650776760,
155:   0.9781412645524530, 0.9363303228513180, 0.8430454008691210, 0.6162348270656310, 0.3977902134753690, 0.1852760304055380, 0.0625305163973752, -0.0477440698701413, -0.1126037294757250, -0.1358018386690990,
156:   -0.2544275858052280, -0.3179500551407050, -0.3873842567862560, -0.4217482646499100, -0.4689384577141290, -0.5872793774815170, -0.6654907471409910, -0.7391119157698760, -0.8095586705039820, -0.8833378646001520,
157:   -0.9506681839279980, -0.9910404297342630, -0.9999429557041830, -0.9921031907559610, -0.9331896469980470, -0.6934514712647360, -0.4364996881408350, 0.2388331163456430, 0.9289332462002390, 0.9992107524543470,
158: ];
159: 
160: const expectedITL = [
161:   Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN,
162:   Number.NaN, Number.NaN, Number.NaN,
163:   0.0000000000000, 36.3130000000000, 63.1977500000000, 81.3542500000000, 90.5332083333333, 90.7274166666667, 90.8630416666667, 90.7904791666667, 90.5763791666666, 90.1190625000000,
164:   89.5976625000000, 88.9683964285714, 88.1875857142857, 87.4745535714286, 86.8648353174603, 86.5406809523809, 86.4262830808081, 86.4413116161616, 86.4596270104895, 86.4576728687979,
165:   86.5316151556777, 86.5976538663542, 86.6975181489262, 86.7923007288442, 86.7608821809425, 86.6849764619883, 86.4810361842105, 86.2045236842105, 85.9179875000000, 85.7217875000000,
166:   85.6731214285714, 85.7294946428571, 85.8820685064935, 86.1150389610390, 86.4291412337662, 86.8542435064935, 87.1897624458874, 87.4640642857143, 87.6829172619047, 87.8553064223057,
167:   87.9218177631579, 87.9011081656347, 87.8265154798762, 87.7802132352941, 87.8975367647059, 87.9932665441176, 88.0649329044118, 88.1058593750000, 88.0771718750000, 87.9885156250000,
168:   87.8919218750000, 87.8735468750000, 87.9217812500000, 88.0132343750000, 88.1667968750000, 88.3524062500000, 88.5845312500000, 88.8867187500000, 89.2125000000000, 89.3545928308824,
169:   89.4090386029412, 89.2706643178105, 89.1179705882353, 89.2742091503268, 89.6278143274854, 90.2226447368421, 90.9753333333333, 91.7409342105263, 92.5893245614035, 93.3576615497076,
170:   94.0969948830409, 94.8495138888889, 95.5591111111111, 96.4495179738562, 97.4016168300654, 98.4999640522876, 99.7313676470588, 101.1202794117650, 102.8572647058820, 104.2939607843140,
171:   105.8849452614380, 107.0814910990710, 108.1507799707600, 108.9417928362570, 109.6523348684210, 110.1277241228070, 110.5267148809520, 110.9164619047620, 111.3471666666670, 111.8653571428570,
172:   112.3597857142860, 112.7992380952380, 113.1747142857140, 113.5586666666670, 114.0577738095240, 114.6932047619050, 115.2846898809520, 115.7229029761900, 115.8414500000000, 115.7467000000000,
173:   115.6115500000000, 115.5303375000000, 115.5548000000000, 115.6614000000000, 115.9122375000000, 116.3321625000000, 116.8428375000000, 117.2830500000000, 117.7253922619050, 118.0727228896100,
174:   118.4984101731600, 119.2198956709960, 120.0234311688310, 120.9289954887220, 121.7905789473680, 122.5689105263160, 123.2356894736840, 123.8346386591480, 124.4085501367050, 124.8498862554110,
175:   125.4094954004330, 126.1065170454550, 126.7064925000000, 127.4240825000000, 127.8647176923080, 128.0896789173790, 128.1540621001220, 128.0788455784320, 128.1446725050170, 128.3183694581280,
176:   128.4362103448280, 128.5848821839080, 128.6186973303670, 128.7719790322580, 128.8848929180570, 128.9942553260240, 129.0945314906040, 129.1530999536860, 129.0838581603580, 128.8024011680910,
177:   128.4259584615380, 127.8496453846150, 127.2448900000000, 126.6843541666670, 126.1858333333330, 125.7495416666670, 125.2761041666670, 124.7606666666670, 124.3471666666670, 124.0095208333330,
178:   123.7731875000000, 123.6621125000000, 123.6056416666670, 123.5744416666670, 123.5148250000000, 123.4937083333330, 123.4761041666670, 123.6832443181820, 124.0308714826840, 124.5405766233770,
179:   125.2020811061750, 125.8966645363410, 126.4680156432750, 126.9272675438600, 127.4003611111110, 127.8605833333330, 128.1324342105260, 128.3722748538010, 128.6053611111110, 128.6804526315790,
180:   128.7627552631580, 128.7694289473680, 128.7595250000000, 128.7409750000000, 128.6613250000000, 128.4858250000000, 128.2154750000000, 127.8500845238100, 127.4645440476190, 127.1153875541130,
181:   126.7443323451910, 126.3035666290230, 125.8504999604740, 125.3674468450390, 124.8902561172160, 124.3766410622710, 123.8351980503370, 123.2633556835640, 122.5153494623660, 121.7438575268820,
182:   120.2481227342550, 118.4358502645500, 116.5046713064710, 114.5443467073670, 112.9054354415950, 111.4893484615380, 110.7192281481480, 110.1931396825400, 110.2034782445810, 110.1120396292030,
183:   109.5987628888250, 108.8005075604840, 107.8019843750000, 106.8111562500000, 105.6388140120970, 104.3560227486560, 102.7347166042630, 101.0735459592080, 99.5273900488400, 98.3256981583231,
184:   97.3580796011396, 96.7594046153846, 96.4602600000000, 96.1232166666667, 96.1488191666667, 96.4425624637681, 96.7984085144927, 97.1613937318841, 97.3868053381643, 97.5343687643678,
185:   97.8406773180077, 98.3334693486590, 99.0595665024631, 99.8837727832512, 100.6778885467980, 101.4226269841270, 102.0713564814810, 102.6847440476190, 103.2616851851850, 103.9931695156700,
186:   104.8224465811970, 105.7652606837610, 106.6112275641030, 107.5680471014490, 108.3253364624510, 108.8823498494260, 109.2757501411630, 109.6873409090910, 110.1830037593980, 110.4551917293230,
187: ];
188: 
189: // Expected tcm value array from MBST. First 63 entries NaN, then 138 ±1 entries (201 total).
190: const N = Number.NaN;
191: const expectedValue = [
192:   N, N, N, N, N, N, N, N, N, N,
193:   N, N, N, N, N, N, N, N, N, N,
194:   N, N, N, N, N, N, N, N, N, N,
195:   N, N, N, N, N, N, N, N, N, N,
196:   N, N, N, N, N, N, N, N, N, N,
197:   N, N, N, N, N, N, N, N, N, N,
198:   N, N, N,
199:   1, 1, 1, 1, 1, 1, 1, -1, -1, 1, 1, 1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
200:   1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, 1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
201:   1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
202:   1, 1, 1, -1, 1, 1, 1, -1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, 1, 1,
203:   1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
204: ];
205: 
206: const tolerance = 1e-4;
207: const skip = 9;
208: const settleSkip = 177;
209: 
210: // MBST wraps phase into (-180, 180]; zpano into [0, 360). Compare modulo 360.
211: function phaseDelta(expected: number, actual: number): number {
212:   let d = (expected - actual) % 360;
213:   if (d > 180) d -= 360;
214:   else if (d < -180) d += 360;
215:   return d;
216: }
217: 
218: describe('TrendCycleMode', () => {
219:   const time = new Date(2021, 3, 1);
220: 
221:   it('should have correct output enum values', () => {
222:     expect(TrendCycleModeOutput.Value).toBe(0);
223:     expect(TrendCycleModeOutput.IsTrendMode).toBe(1);
224:     expect(TrendCycleModeOutput.IsCycleMode).toBe(2);
225:     expect(TrendCycleModeOutput.InstantaneousTrendLine).toBe(3);
226:     expect(TrendCycleModeOutput.SineWave).toBe(4);
227:     expect(TrendCycleModeOutput.SineWaveLead).toBe(5);
228:     expect(TrendCycleModeOutput.DominantCyclePeriod).toBe(6);
229:     expect(TrendCycleModeOutput.DominantCyclePhase).toBe(7);
230:   });
231: 
232:   it('should return expected mnemonic for default params', () => {
233:     const x = TrendCycleMode.default();
234:     expect(x.metadata().mnemonic).toBe('tcm(0.330, 4, 1.000, 1.500%, hl/2)');
235:   });
236: 
237:   it('should return expected mnemonic for phase accumulator, tlsl=3', () => {
238:     const x = TrendCycleMode.fromParams({
239:       alphaEmaPeriodAdditional: 0.5,
240:       estimatorType: HilbertTransformerCycleEstimatorType.PhaseAccumulator,
241:       estimatorParams: { smoothingLength: 4, alphaEmaQuadratureInPhase: 0.2, alphaEmaPeriod: 0.2 },
242:       trendLineSmoothingLength: 3,
243:       cyclePartMultiplier: 0.5,
244:       separationPercentage: 2.0,
245:     });
246:     expect(x.metadata().mnemonic).toBe('tcm(0.500, 3, 0.500, 2.000%, pa(4, 0.200, 0.200), hl/2)');
247:   });
248: 
249:   it('should return expected metadata', () => {
250:     const x = TrendCycleMode.default();
251:     const meta = x.metadata();
252:     const mn = 'tcm(0.330, 4, 1.000, 1.500%, hl/2)';
253:     const mnTrend = 'tcm-trend(0.330, 4, 1.000, 1.500%, hl/2)';
254:     const mnCycle = 'tcm-cycle(0.330, 4, 1.000, 1.500%, hl/2)';
255:     const mnITL = 'tcm-itl(0.330, 4, 1.000, 1.500%, hl/2)';
256:     const mnSine = 'tcm-sine(0.330, 4, 1.000, 1.500%, hl/2)';
257:     const mnSineLead = 'tcm-sineLead(0.330, 4, 1.000, 1.500%, hl/2)';
258:     const mnDCP = 'dcp(0.330, hl/2)';
259:     const mnDCPha = 'dcph(0.330, hl/2)';
260: 
261:     expect(meta.type).toBe(IndicatorType.TrendCycleMode);
262:     expect(meta.mnemonic).toBe(mn);
263:     expect(meta.description).toBe('Trend versus cycle mode ' + mn);
264:     expect(meta.outputs.length).toBe(8);
265: 
266:     expect(meta.outputs[0].kind).toBe(TrendCycleModeOutput.Value);
267:     expect(meta.outputs[0].type).toBe(OutputType.Scalar);
268:     expect(meta.outputs[0].mnemonic).toBe(mn);
269:     expect(meta.outputs[0].description).toBe('Trend versus cycle mode ' + mn);
270: 
271:     expect(meta.outputs[1].kind).toBe(TrendCycleModeOutput.IsTrendMode);
272:     expect(meta.outputs[1].mnemonic).toBe(mnTrend);
273:     expect(meta.outputs[1].description).toBe('Trend versus cycle mode, is-trend flag ' + mnTrend);
274: 
275:     expect(meta.outputs[2].kind).toBe(TrendCycleModeOutput.IsCycleMode);
276:     expect(meta.outputs[2].mnemonic).toBe(mnCycle);
277:     expect(meta.outputs[2].description).toBe('Trend versus cycle mode, is-cycle flag ' + mnCycle);
278: 
279:     expect(meta.outputs[3].kind).toBe(TrendCycleModeOutput.InstantaneousTrendLine);
280:     expect(meta.outputs[3].mnemonic).toBe(mnITL);
281: 
282:     expect(meta.outputs[4].kind).toBe(TrendCycleModeOutput.SineWave);
283:     expect(meta.outputs[4].mnemonic).toBe(mnSine);
284: 
285:     expect(meta.outputs[5].kind).toBe(TrendCycleModeOutput.SineWaveLead);
286:     expect(meta.outputs[5].mnemonic).toBe(mnSineLead);
287: 
288:     expect(meta.outputs[6].kind).toBe(TrendCycleModeOutput.DominantCyclePeriod);
289:     expect(meta.outputs[6].mnemonic).toBe(mnDCP);
290:     expect(meta.outputs[6].description).toBe('Dominant cycle period ' + mnDCP);
291: 
292:     expect(meta.outputs[7].kind).toBe(TrendCycleModeOutput.DominantCyclePhase);
293:     expect(meta.outputs[7].mnemonic).toBe(mnDCPha);
294:     expect(meta.outputs[7].description).toBe('Dominant cycle phase ' + mnDCPha);
295:   });
296: 
297:   it('should throw if α is out of range (0, 1]', () => {
298:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0 })).toThrowError(/α for additional smoothing/);
299:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: -0.1 })).toThrowError(/α for additional smoothing/);
300:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 1.0001 })).toThrowError(/α for additional smoothing/);
301:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.5 })).not.toThrow();
302:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 1.0 })).not.toThrow();
303:   });
304: 
305:   it('should throw if trend line smoothing length is not 2, 3, or 4', () => {
306:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 1 }))
307:       .toThrowError(/trend line smoothing length/);
308:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 5 }))
309:       .toThrowError(/trend line smoothing length/);
310:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 2 })).not.toThrow();
311:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 3 })).not.toThrow();
312:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 4 })).not.toThrow();
313:   });
314: 
315:   it('should throw if cycle part multiplier is out of range (0, 10]', () => {
316:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, cyclePartMultiplier: 0 }))
317:       .toThrowError(/cycle part multiplier/);
318:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, cyclePartMultiplier: 10.00001 }))
319:       .toThrowError(/cycle part multiplier/);
320:   });
321: 
322:   it('should throw if separation percentage is out of range (0, 100]', () => {
323:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, separationPercentage: 0 }))
324:       .toThrowError(/separation percentage/);
325:     expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, separationPercentage: 100.00001 }))
326:       .toThrowError(/separation percentage/);
327:   });
328: 
329:   it('should return NaN 8-tuple for NaN input', () => {
330:     const x = TrendCycleMode.default();
331:     const out = x.update(Number.NaN);
332:     expect(out.length).toBe(8);
333:     for (const v of out) {
334:       expect(Number.isNaN(v)).toBe(true);
335:     }
336:   });
337: 
338:   it('should match reference period past settle window', () => {
339:     const x = TrendCycleMode.default();
340:     for (let i = skip; i < input.length; i++) {
341:       const [, , , , , , period] = x.update(input[i]);
342:       if (Number.isNaN(period) || i < settleSkip) continue;
343:       expect(Math.abs(expectedPeriod[i] - period))
344:         .withContext(`period[${i}]: expected ${expectedPeriod[i]}, actual ${period}`)
345:         .toBeLessThan(tolerance);
346:     }
347:   });
348: 
349:   it('should match reference phase (mod 360) past settle window', () => {
350:     const x = TrendCycleMode.default();
351:     for (let i = skip; i < input.length; i++) {
352:       const [, , , , , , , phase] = x.update(input[i]);
353:       if (Number.isNaN(phase) || Number.isNaN(expectedPhase[i]) || i < settleSkip) continue;
354:       const d = phaseDelta(expectedPhase[i], phase);
355:       expect(Math.abs(d))
356:         .withContext(`phase[${i}]: expected ${expectedPhase[i]}, actual ${phase}`)
357:         .toBeLessThan(tolerance);
358:     }
359:   });
360: 
361:   it('should match reference sine wave past settle window', () => {
362:     const x = TrendCycleMode.default();
363:     for (let i = skip; i < input.length; i++) {
364:       const [, , , , sine] = x.update(input[i]);
365:       if (Number.isNaN(sine) || Number.isNaN(expectedSine[i]) || i < settleSkip) continue;
366:       expect(Math.abs(expectedSine[i] - sine))
367:         .withContext(`sine[${i}]: expected ${expectedSine[i]}, actual ${sine}`)
368:         .toBeLessThan(tolerance);
369:     }
370:   });
371: 
372:   it('should match reference sine wave lead past settle window', () => {
373:     const x = TrendCycleMode.default();
374:     for (let i = skip; i < input.length; i++) {
375:       const [, , , , , sineLead] = x.update(input[i]);
376:       if (Number.isNaN(sineLead) || Number.isNaN(expectedSineLead[i]) || i < settleSkip) continue;
377:       expect(Math.abs(expectedSineLead[i] - sineLead))
378:         .withContext(`sineLead[${i}]: expected ${expectedSineLead[i]}, actual ${sineLead}`)
379:         .toBeLessThan(tolerance);
380:     }
381:   });
382: 
383:   it('should match reference instantaneous trend line past settle window', () => {
384:     const x = TrendCycleMode.default();
385:     for (let i = skip; i < input.length; i++) {
386:       const [, , , itl] = x.update(input[i]);
387:       if (Number.isNaN(itl) || Number.isNaN(expectedITL[i]) || i < settleSkip) continue;
388:       expect(Math.abs(expectedITL[i] - itl))
389:         .withContext(`itl[${i}]: expected ${expectedITL[i]}, actual ${itl}`)
390:         .toBeLessThan(tolerance);
391:     }
392:   });
393: 
394:   it('should match reference tcm value (except MBST known mismatches)', () => {
395:     const x = TrendCycleMode.default();
396:     const limit = expectedValue.length;
397:     for (let i = skip; i < input.length; i++) {
398:       const [value] = x.update(input[i]);
399:       if (i >= limit) continue;
400:       // MBST known mismatches.
401:       if (i === 70 || i === 71) continue;
402:       if (Number.isNaN(value) || Number.isNaN(expectedValue[i])) continue;
403:       expect(Math.abs(expectedValue[i] - value))
404:         .withContext(`value[${i}]: expected ${expectedValue[i]}, actual ${value}`)
405:         .toBeLessThan(tolerance);
406:     }
407:   });
408: 
409:   it('should have complementary is-trend / is-cycle flags (0/1) aligned with value sign', () => {
410:     const x = TrendCycleMode.default();
411:     for (let i = skip; i < input.length; i++) {
412:       const [value, trend, cycle] = x.update(input[i]);
413:       if (Number.isNaN(value)) continue;
414:       expect(trend + cycle).toBe(1);
415:       if (value > 0) expect(trend).toBe(1);
416:       if (value < 0) expect(trend).toBe(0);
417:     }
418:   });
419: 
420:   it('should become primed within the input sequence', () => {
421:     const x = TrendCycleMode.default();
422:     expect(x.isPrimed()).toBe(false);
423: 
424:     let primedAt = -1;
425:     for (let i = 0; i < input.length; i++) {
426:       x.update(input[i]);
427:       if (x.isPrimed() && primedAt < 0) primedAt = i;
428:     }
429:     expect(primedAt).toBeGreaterThanOrEqual(0);
430:     expect(x.isPrimed()).toBe(true);
431:   });
432: 
433:   const primeCount = 200;
434:   const value = 100.0;
435: 
436:   function checkOutput(out: any[]): void {
437:     expect(out.length).toBe(8);
438:     for (let i = 0; i < 8; i++) {
439:       const s = out[i] as Scalar;
440:       expect(s.time).toEqual(time);
441:     }
442:   }
443: 
444:   it('should produce 8-element scalar output via updateScalar', () => {
445:     const x = TrendCycleMode.default();
446:     for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);
447:     const s = new Scalar();
448:     s.time = time;
449:     s.value = value;
450:     checkOutput(x.updateScalar(s) as any[]);
451:   });
452: 
453:   it('should produce 8-element scalar output via updateBar', () => {
454:     const x = TrendCycleMode.default();
455:     for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);
456:     const bar = new Bar({ time, open: value, high: value, low: value, close: value, volume: 0 });
457:     checkOutput(x.updateBar(bar) as any[]);
458:   });
459: 
460:   it('should produce 8-element scalar output via updateQuote', () => {
461:     const x = TrendCycleMode.default();
462:     for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);
463:     const q = new Quote({ time, bid: value, ask: value, bidSize: 0, askSize: 0 });
464:     checkOutput(x.updateQuote(q) as any[]);
465:   });
466: 
467:   it('should produce 8-element scalar output via updateTrade', () => {
468:     const x = TrendCycleMode.default();
469:     for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);
470:     const t = new Trade({ time, price: value, volume: 0 });
471:     checkOutput(x.updateTrade(t) as any[]);
472:   });
473: });

(End of file - total 473 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 7.3s)

_Thinking:_

 I'm looking at the Go snapshot test file pattern to understand how to reuse the 252-entry input format, and I want to check if the CoronaSpectrum test has any mnemonic expectations I should verify against.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrum_test.go",
  "limit": 80
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrum_test.go</path>
<type>file</type>
<content>1: //nolint:testpackage
2: package coronaspectrum
3: 
4: import (
5: 	"math"
6: 	"testing"
7: 	"time"
8: 
9: 	"zpano/entities"
10: 	"zpano/indicators/core"
11: 	"zpano/indicators/core/outputs"
12: )
13: 
14: func testCSTime() time.Time {
15: 	return time.Date(2021, time.April, 1, 0, 0, 0, 0, &time.Location{})
16: }
17: 
18: // testCSInput is the 252-entry TA-Lib MAMA reference series (Price D5…D256),
19: // reused here so the coronaspectrum package has no test dependency on other
20: // packages.
21: //
22: //nolint:dupl
23: func testCSInput() []float64 {
24: 	return []float64{
25: 		92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,
26: 		94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,
27: 		88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,
28: 		85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,
29: 		83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,
30: 		89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,
31: 		89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,
32: 		88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,
33: 		103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,
34: 		120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,
35: 		114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,
36: 		114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,
37: 		124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,
38: 		137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,
39: 		123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,
40: 		122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,
41: 		123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,
42: 		130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,
43: 		127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,
44: 		121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,
45: 		106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,
46: 		95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,
47: 		94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,
48: 		103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,
49: 		106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,
50: 		109.5300, 108.0600,
51: 	}
52: }
53: 
54: const testCSTolerance = 1e-4
55: 
56: func TestCoronaSpectrumUpdate(t *testing.T) {
57: 	t.Parallel()
58: 
59: 	input := testCSInput()
60: 	t0 := testCSTime()
61: 
62: 	// Snapshot values captured from a first run and locked in here.
63: 	type snap struct {
64: 		i   int
65: 		dc  float64
66: 		dcm float64
67: 	}
68: 	snapshots := []snap{
69: 		{11, 17.7604672565, 17.7604672565},
70: 		{12, 6.0000000000, 6.0000000000},
71: 		{50, 15.9989078712, 15.9989078712},
72: 		{100, 14.7455497547, 14.7455497547},
73: 		{150, 17.5000000000, 17.2826036069},
74: 		{200, 19.7557338512, 20.0000000000},
75: 		{251, 6.0000000000, 6.0000000000},
76: 	}
77: 
78: 	x, err := NewCoronaSpectrumDefault()
79: 	if err != nil {
80: 		t.Fatalf("unexpected error: %v", err)

(Showing lines 1-80 of 413. Use offset=81 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.6s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrum_test.go",
  "offset": 80,
  "limit": 350
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrum_test.go</path>
<type>file</type>
<content>80: 		t.Fatalf("unexpected error: %v", err)
81: 	}
82: 
83: 	si := 0
84: 	for i := range input {
85: 		h, dc, dcm := x.Update(input[i], t0.Add(time.Duration(i)*time.Minute))
86: 
87: 		if h == nil {
88: 			t.Fatalf("[%d] heatmap must not be nil", i)
89: 		}
90: 
91: 		// Heatmap axis invariants are always present.
92: 		if h.ParameterFirst != 6 || h.ParameterLast != 30 || h.ParameterResolution != 2 {
93: 			t.Errorf("[%d] heatmap axis incorrect: first=%v last=%v res=%v",
94: 				i, h.ParameterFirst, h.ParameterLast, h.ParameterResolution)
95: 		}
96: 
97: 		if !x.IsPrimed() {
98: 			if !h.IsEmpty() {
99: 				t.Errorf("[%d] expected empty heatmap before priming, got len=%d", i, len(h.Values))
100: 			}
101: 
102: 			if !math.IsNaN(dc) || !math.IsNaN(dcm) {
103: 				t.Errorf("[%d] expected NaN scalars before priming, got dc=%v dcm=%v", i, dc, dcm)
104: 			}
105: 
106: 			continue
107: 		}
108: 
109: 		if len(h.Values) != 49 {
110: 			t.Errorf("[%d] heatmap values length: expected 49, got %d", i, len(h.Values))
111: 		}
112: 
113: 		if si < len(snapshots) && snapshots[si].i == i {
114: 			if math.Abs(snapshots[si].dc-dc) > testCSTolerance {
115: 				t.Errorf("[%d] dc: expected %v, got %v", i, snapshots[si].dc, dc)
116: 			}
117: 
118: 			if math.Abs(snapshots[si].dcm-dcm) > testCSTolerance {
119: 				t.Errorf("[%d] dcm: expected %v, got %v", i, snapshots[si].dcm, dcm)
120: 			}
121: 
122: 			si++
123: 		}
124: 	}
125: 
126: 	if si != len(snapshots) {
127: 		t.Errorf("did not hit all %d snapshots, reached %d", len(snapshots), si)
128: 	}
129: }
130: 
131: func TestCoronaSpectrumPrimesAtBar11(t *testing.T) {
132: 	t.Parallel()
133: 
134: 	x, _ := NewCoronaSpectrumDefault()
135: 
136: 	if x.IsPrimed() {
137: 		t.Error("expected not primed at start")
138: 	}
139: 
140: 	input := testCSInput()
141: 	t0 := testCSTime()
142: 	primedAt := -1
143: 
144: 	for i := range input {
145: 		x.Update(input[i], t0.Add(time.Duration(i)*time.Minute))
146: 
147: 		if x.IsPrimed() && primedAt < 0 {
148: 			primedAt = i
149: 		}
150: 	}
151: 
152: 	// MinimalPeriodTimesTwo = 12, so priming first happens at sample index 11 (0-indexed).
153: 	if primedAt != 11 {
154: 		t.Errorf("expected priming at index 11, got %d", primedAt)
155: 	}
156: }
157: 
158: func TestCoronaSpectrumNaNInput(t *testing.T) {
159: 	t.Parallel()
160: 
161: 	x, _ := NewCoronaSpectrumDefault()
162: 
163: 	h, dc, dcm := x.Update(math.NaN(), testCSTime())
164: 
165: 	if h == nil || !h.IsEmpty() {
166: 		t.Errorf("expected empty heatmap for NaN input, got %v", h)
167: 	}
168: 
169: 	if !math.IsNaN(dc) || !math.IsNaN(dcm) {
170: 		t.Errorf("expected NaN scalars for NaN input, got dc=%v dcm=%v", dc, dcm)
171: 	}
172: 
173: 	// NaN must not prime the indicator.
174: 	if x.IsPrimed() {
175: 		t.Error("NaN input must not prime the indicator")
176: 	}
177: }
178: 
179: func TestCoronaSpectrumMetadata(t *testing.T) {
180: 	t.Parallel()
181: 
182: 	x, _ := NewCoronaSpectrumDefault()
183: 	md := x.Metadata()
184: 
185: 	check := func(what string, exp, act any) {
186: 		t.Helper()
187: 
188: 		if exp != act {
189: 			t.Errorf("%s: expected %v, actual %v", what, exp, act)
190: 		}
191: 	}
192: 
193: 	mnVal := "cspect(6, 20, 6, 30, 30, hl/2)"
194: 	mnDC := "cspect-dc(30, hl/2)"
195: 	mnDCM := "cspect-dcm(30, hl/2)"
196: 
197: 	check("Type", core.CoronaSpectrum, md.Type)
198: 	check("Mnemonic", mnVal, md.Mnemonic)
199: 	check("Description", "Corona spectrum "+mnVal, md.Description)
200: 	check("len(Outputs)", 3, len(md.Outputs))
201: 
202: 	check("Outputs[0].Kind", int(Value), md.Outputs[0].Kind)
203: 	check("Outputs[0].Type", outputs.HeatmapType, md.Outputs[0].Type)
204: 	check("Outputs[0].Mnemonic", mnVal, md.Outputs[0].Mnemonic)
205: 
206: 	check("Outputs[1].Kind", int(DominantCycle), md.Outputs[1].Kind)
207: 	check("Outputs[1].Type", outputs.ScalarType, md.Outputs[1].Type)
208: 	check("Outputs[1].Mnemonic", mnDC, md.Outputs[1].Mnemonic)
209: 
210: 	check("Outputs[2].Kind", int(DominantCycleMedian), md.Outputs[2].Kind)
211: 	check("Outputs[2].Type", outputs.ScalarType, md.Outputs[2].Type)
212: 	check("Outputs[2].Mnemonic", mnDCM, md.Outputs[2].Mnemonic)
213: }
214: 
215: //nolint:funlen
216: func TestCoronaSpectrumUpdateEntity(t *testing.T) {
217: 	t.Parallel()
218: 
219: 	const (
220: 		primeCount = 50
221: 		inp        = 100.
222: 		outputLen  = 3
223: 	)
224: 
225: 	tm := testCSTime()
226: 	input := testCSInput()
227: 
228: 	check := func(act core.Output) {
229: 		t.Helper()
230: 
231: 		if len(act) != outputLen {
232: 			t.Errorf("len(output): expected %v, actual %v", outputLen, len(act))
233: 
234: 			return
235: 		}
236: 
237: 		h, ok := act[0].(*outputs.Heatmap)
238: 		if !ok {
239: 			t.Errorf("output[0] is not a heatmap: %T", act[0])
240: 		} else if h.Time != tm {
241: 			t.Errorf("output[0].Time: expected %v, actual %v", tm, h.Time)
242: 		}
243: 
244: 		for i := 1; i < outputLen; i++ {
245: 			s, ok := act[i].(entities.Scalar)
246: 			if !ok {
247: 				t.Errorf("output[%d] is not a scalar", i)
248: 
249: 				continue
250: 			}
251: 
252: 			if s.Time != tm {
253: 				t.Errorf("output[%d].Time: expected %v, actual %v", i, tm, s.Time)
254: 			}
255: 		}
256: 	}
257: 
258: 	t.Run("update scalar", func(t *testing.T) {
259: 		t.Parallel()
260: 
261: 		s := entities.Scalar{Time: tm, Value: inp}
262: 		x, _ := NewCoronaSpectrumDefault()
263: 
264: 		for i := 0; i < primeCount; i++ {
265: 			x.Update(input[i%len(input)], tm)
266: 		}
267: 
268: 		check(x.UpdateScalar(&s))
269: 	})
270: 
271: 	t.Run("update bar", func(t *testing.T) {
272: 		t.Parallel()
273: 
274: 		b := entities.Bar{Time: tm, High: inp, Low: inp, Close: inp}
275: 		x, _ := NewCoronaSpectrumDefault()
276: 
277: 		for i := 0; i < primeCount; i++ {
278: 			x.Update(input[i%len(input)], tm)
279: 		}
280: 
281: 		check(x.UpdateBar(&b))
282: 	})
283: 
284: 	t.Run("update quote", func(t *testing.T) {
285: 		t.Parallel()
286: 
287: 		q := entities.Quote{Time: tm, Bid: inp, Ask: inp}
288: 		x, _ := NewCoronaSpectrumDefault()
289: 
290: 		for i := 0; i < primeCount; i++ {
291: 			x.Update(input[i%len(input)], tm)
292: 		}
293: 
294: 		check(x.UpdateQuote(&q))
295: 	})
296: 
297: 	t.Run("update trade", func(t *testing.T) {
298: 		t.Parallel()
299: 
300: 		r := entities.Trade{Time: tm, Price: inp}
301: 		x, _ := NewCoronaSpectrumDefault()
302: 
303: 		for i := 0; i < primeCount; i++ {
304: 			x.Update(input[i%len(input)], tm)
305: 		}
306: 
307: 		check(x.UpdateTrade(&r))
308: 	})
309: }
310: 
311: //nolint:funlen
312: func TestNewCoronaSpectrum(t *testing.T) {
313: 	t.Parallel()
314: 
315: 	check := func(name string, exp, act any) {
316: 		t.Helper()
317: 
318: 		if exp != act {
319: 			t.Errorf("%s: expected %v, actual %v", name, exp, act)
320: 		}
321: 	}
322: 
323: 	t.Run("default", func(t *testing.T) {
324: 		t.Parallel()
325: 
326: 		x, err := NewCoronaSpectrumDefault()
327: 		check("err == nil", true, err == nil)
328: 		check("mnemonic", "cspect(6, 20, 6, 30, 30, hl/2)", x.mnemonic)
329: 		check("MinParameterValue", 6.0, x.minParameterValue)
330: 		check("MaxParameterValue", 30.0, x.maxParameterValue)
331: 		check("ParameterResolution", 2.0, x.parameterResolution)
332: 	})
333: 
334: 	t.Run("custom ranges round to integers", func(t *testing.T) {
335: 		t.Parallel()
336: 
337: 		x, err := NewCoronaSpectrumParams(&Params{
338: 			MinRasterValue:       4,
339: 			MaxRasterValue:       25,
340: 			MinParameterValue:    8.7,  // ceils to 9
341: 			MaxParameterValue:    40.4, // floors to 40
342: 			HighPassFilterCutoff: 20,
343: 		})
344: 		check("err == nil", true, err == nil)
345: 		check("MinParameterValue", 9.0, x.minParameterValue)
346: 		check("MaxParameterValue", 40.0, x.maxParameterValue)
347: 		check("mnemonic", "cspect(4, 25, 9, 40, 20, hl/2)", x.mnemonic)
348: 	})
349: 
350: 	t.Run("MaxRasterValue <= MinRasterValue", func(t *testing.T) {
351: 		t.Parallel()
352: 
353: 		_, err := NewCoronaSpectrumParams(&Params{
354: 			MinRasterValue:    10,
355: 			MaxRasterValue:    10,
356: 			MinParameterValue: 6,
357: 			MaxParameterValue: 30,
358: 		})
359: 		if err == nil || err.Error() !=
360: 			"invalid corona spectrum parameters: MaxRasterValue should be > MinRasterValue" {
361: 			t.Errorf("unexpected: %v", err)
362: 		}
363: 	})
364: 
365: 	t.Run("MinParameterValue < 2", func(t *testing.T) {
366: 		t.Parallel()
367: 
368: 		_, err := NewCoronaSpectrumParams(&Params{
369: 			MinParameterValue: 1,
370: 			MaxParameterValue: 30,
371: 		})
372: 		if err == nil || err.Error() !=
373: 			"invalid corona spectrum parameters: MinParameterValue should be >= 2" {
374: 			t.Errorf("unexpected: %v", err)
375: 		}
376: 	})
377: 
378: 	t.Run("MaxParameterValue <= MinParameterValue", func(t *testing.T) {
379: 		t.Parallel()
380: 
381: 		_, err := NewCoronaSpectrumParams(&Params{
382: 			MinParameterValue: 20,
383: 			MaxParameterValue: 20,
384: 		})
385: 		if err == nil || err.Error() !=
386: 			"invalid corona spectrum parameters: MaxParameterValue should be > MinParameterValue" {
387: 			t.Errorf("unexpected: %v", err)
388: 		}
389: 	})
390: 
391: 	t.Run("HighPassFilterCutoff < 2", func(t *testing.T) {
392: 		t.Parallel()
393: 
394: 		_, err := NewCoronaSpectrumParams(&Params{
395: 			HighPassFilterCutoff: 1,
396: 		})
397: 		if err == nil || err.Error() !=
398: 			"invalid corona spectrum parameters: HighPassFilterCutoff should be >= 2" {
399: 			t.Errorf("unexpected: %v", err)
400: 		}
401: 	})
402: 
403: 	t.Run("invalid bar component", func(t *testing.T) {
404: 		t.Parallel()
405: 
406: 		_, err := NewCoronaSpectrumParams(&Params{
407: 			BarComponent: entities.BarComponent(9999),
408: 		})
409: 		if err == nil {
410: 			t.Error("expected error")
411: 		}
412: 	})
413: }

(End of file - total 413 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.8s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona/corona-params.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/corona/corona-params.ts</path>
<type>file</type>
<content>1: /** Configures a Corona spectral analysis engine.
2:  *
3:  * All fields have zero-value defaults: a zero or _undefined_ value means "use the default".
4:  * The defaults follow Ehlers' original TASC article (November 2008).
5:  */
6: export interface CoronaParams {
7:   /** High-pass filter cutoff period (de-trending period), in bars. Must be >= 2.
8:    *
9:    * The default value is 30.
10:    */
11:   highPassFilterCutoff?: number;
12: 
13:   /** Minimum cycle period (in bars) covered by the bandpass filter bank. Must be >= 2.
14:    *
15:    * The default value is 6.
16:    */
17:   minimalPeriod?: number;
18: 
19:   /** Maximum cycle period (in bars) covered by the bandpass filter bank. Must be > minimalPeriod.
20:    *
21:    * The default value is 30.
22:    */
23:   maximalPeriod?: number;
24: 
25:   /** Filter bins with smoothed dB value at or below this threshold contribute to the
26:    * weighted dominant-cycle estimate.
27:    *
28:    * The default value is 6.
29:    */
30:   decibelsLowerThreshold?: number;
31: 
32:   /** Upper clamp on the smoothed dB value and reference value for the dominant-cycle
33:    * weighting (weight = upper − dB).
34:    *
35:    * The default value is 20.
36:    */
37:   decibelsUpperThreshold?: number;
38: }
39: 
40: /** Default Corona parameters (Ehlers TASC, November 2008). */
41: export const DefaultCoronaParams: Required<CoronaParams> = {
42:   highPassFilterCutoff: 30,
43:   minimalPeriod: 6,
44:   maximalPeriod: 30,
45:   decibelsLowerThreshold: 6,
46:   decibelsUpperThreshold: 20,
47: };

(End of file - total 47 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.4s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/entities/bar-component.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/entities/bar-component.ts</path>
<type>file</type>
<content>1: import { Bar } from './bar';
2: 
3: /** Enumerates price components of a _Bar_. */
4: export enum BarComponent {
5:   /** The opening price. */
6:   Open,
7: 
8:   /** The highest price. */
9:   High,
10: 
11:   /** The lowest price. */
12:   Low,
13: 
14:   /** The closing price. */
15:   Close,
16: 
17:   /** The volume. */
18:   Volume,
19: 
20:   /** The median price, calculated as _(high + low) / 2_. */
21:   Median,
22: 
23:   /** The typical price, calculated as _(high + low + close) / 3_. */
24:   Typical,
25: 
26:   /** The weighted price, calculated as _(high + low + 2*close) / 4_. */
27:   Weighted,
28: 
29:   /** The average price, calculated as _(open + high + low + close) / 4_. */
30:   Average,
31: }
32: 
33: /** The default bar component used when none is specified. */
34: export const DefaultBarComponent = BarComponent.Close;
35: 
36: /** Function for calculating a price component of a _Bar_. */
37: export const barComponentValue = (component: BarComponent): (bar: Bar) => number => {
38:   switch (component) {
39:     case BarComponent.Open:
40:       return (b: Bar) => b.open;
41:     case BarComponent.High:
42:       return (b: Bar) => b.high;
43:     case BarComponent.Low:
44:       return (b: Bar) => b.low;
45:     case BarComponent.Close:
46:       return (b: Bar) => b.close;
47:     case BarComponent.Volume:
48:       return (b: Bar) => b.volume;
49:     case BarComponent.Median:
50:       return (b: Bar) => b.median();
51:     case BarComponent.Typical:
52:       return (b: Bar) => b.typical();
53:     case BarComponent.Weighted:
54:       return (b: Bar) => b.weighted();
55:     case BarComponent.Average:
56:       return (b: Bar) => b.average();
57:     default: // Default to close.
58:       return (b: Bar) => b.close;
59:   }
60: };
61: 
62: /** The mnemonic of a price component of a _Bar_. */
63: export const barComponentMnemonic = (component: BarComponent): string => {
64:   switch (component) {
65:     case BarComponent.Open:
66:       return 'o';
67:     case BarComponent.High:
68:       return 'h';
69:     case BarComponent.Low:
70:       return 'l';
71:     case BarComponent.Close:
72:       return 'c';
73:     case BarComponent.Volume:
74:       return 'v';
75:     case BarComponent.Median:
76:       return 'hl/2';
77:     case BarComponent.Typical:
78:       return 'hlc/3';
79:     case BarComponent.Weighted:
80:       return 'hlcc/4';
81:     case BarComponent.Average:
82:       return 'ohlc/4';
83:     default:
84:       return '??';
85:   }
86: };

(End of file - total 86 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 9.9s)

_Thinking:_

 I'm organizing the test data for the CoronaSpectrum TypeScript package—I'll duplicate the 252-entry MBST price input across each spec file to match how the Go tests handle it, rather than creating a shared module.

Now I'm writing out the four CoronaSpectrum package files...

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum-output.ts",
  "content": "/** Enumerates outputs of the CoronaSpectrum indicator. */\nexport enum CoronaSpectrumOutput {\n  /** The Corona spectrum heatmap column (decibels across the filter bank). */\n  Value = 0,\n  /** The weighted-center-of-gravity dominant cycle estimate. */\n  DominantCycle = 1,\n  /** The 5-sample median of DominantCycle. */\n  DominantCycleMedian = 2,\n}\n"
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

## Assistant (Build · claude-opus-4.7 · 15.6s)

(Ignoring the unrelated C LSP noise.)

**Tool: write**

**Input:**
```json
{
  "content": "import { BarComponent } from '../../../entities/bar-component';\nimport { QuoteComponent } from '../../../entities/quote-component';\nimport { TradeComponent } from '../../../entities/trade-component';\n\n/** Describes parameters to create an instance of the CoronaSpectrum indicator. */\nexport interface CoronaSpectrumParams {\n  /** Minimal raster value (z) of the heatmap, in decibels. Corresponds to the\n   * CoronaLowerDecibels threshold.\n   *\n   * The default value is 6. A zero value is treated as \"use default\".\n   */\n  minRasterValue?: number;\n\n  /** Maximal raster value (z) of the heatmap, in decibels. Corresponds to the\n   * CoronaUpperDecibels threshold.\n   *\n   * The default value is 20. A zero value is treated as \"use default\".\n   */\n  maxRasterValue?: number;\n\n  /** Minimal ordinate (y) value of the heatmap, representing the minimal cycle\n   * period covered by the filter bank. Rounded up to the nearest integer.\n   *\n   * The default value is 6. A zero value is treated as \"use default\".\n   */\n  minParameterValue?: number;\n\n  /** Maximal ordinate (y) value of the heatmap, representing the maximal cycle\n   * period covered by the filter bank. Rounded down to the nearest integer.\n   *\n   * The default value is 30. A zero value is treated as \"use default\".\n   */\n  maxParameterValue?: number;\n\n  /** High-pass filter cutoff (de-trending period) used by the inner Corona engine.\n   * Suggested values are 20, 30, 100.\n   *\n   * The default value is 30. A zero value is treated as \"use default\".\n   */\n  highPassFilterCutoff?: number;\n\n  /** A component of a bar to use when updating the indicator with a bar sample.\n   *\n   * If _undefined_, the default (BarComponent.Median, hl/2) is used, matching\n   * Ehlers' reference which operates on (High+Low)/2. Since this differs from\n   * the framework default, it is always shown in the mnemonic.\n   */\n  barComponent?: BarComponent;\n\n  /** A component of a quote to use when updating the indicator with a quote sample.\n   *\n   * If _undefined_, a default value is used and the component is not shown in the indicator mnemonic.\n   */\n  quoteComponent?: QuoteComponent;\n\n  /** A component of a trade to use when updating the indicator with a trade sample.\n   *\n   * If _undefined_, a default value is used and the component is not shown in the indicator mnemonic.\n   */\n  tradeComponent?: TradeComponent;\n}\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum-params.ts"
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

## Assistant (Build · claude-opus-4.7 · 45.0s)

**Tool: write**

**Input:**
```json
{
  "content": "import { Bar } from '../../../entities/bar';\nimport { BarComponent, barComponentValue } from '../../../entities/bar-component';\nimport { Quote } from '../../../entities/quote';\nimport { DefaultQuoteComponent, quoteComponentValue } from '../../../entities/quote-component';\nimport { Scalar } from '../../../entities/scalar';\nimport { Trade } from '../../../entities/trade';\nimport { DefaultTradeComponent, tradeComponentValue } from '../../../entities/trade-component';\nimport { componentTripleMnemonic } from '../../core/component-triple-mnemonic';\nimport { Indicator } from '../../core/indicator';\nimport { IndicatorMetadata } from '../../core/indicator-metadata';\nimport { IndicatorOutput } from '../../core/indicator-output';\nimport { IndicatorType } from '../../core/indicator-type';\nimport { Heatmap } from '../../core/outputs/heatmap';\nimport { OutputType } from '../../core/outputs/output-type';\nimport { Corona } from '../corona/corona';\nimport { CoronaSpectrumOutput } from './corona-spectrum-output';\nimport { CoronaSpectrumParams } from './corona-spectrum-params';\n\nconst DEFAULT_MIN_RASTER = 6;\nconst DEFAULT_MAX_RASTER = 20;\nconst DEFAULT_MIN_PARAM = 6;\nconst DEFAULT_MAX_PARAM = 30;\nconst DEFAULT_HP_CUTOFF = 30;\n\n/** __Corona Spectrum__ (Ehlers) heatmap indicator.\n *\n * The Corona Spectrum measures cyclic activity over a cycle period range (default 6..30 bars)\n * in a bank of contiguous bandpass filters. The amplitude of each filter output is compared to\n * the strongest signal and displayed, in decibels, as a heatmap column. The filter having the\n * strongest output is selected as the current dominant cycle period.\n *\n * It exposes three outputs:\n *\n *\t- Value: a per-bar heatmap column (decibels across the filter bank).\n *\t- DominantCycle: the weighted-center-of-gravity dominant cycle estimate.\n *\t- DominantCycleMedian: the 5-sample median of DominantCycle.\n *\n * Reference:\n *\n *\tJohn Ehlers, \"Measuring Cycle Periods\", Stocks & Commodities, November 2008.\n */\nexport class CoronaSpectrum implements Indicator {\n  private readonly c: Corona;\n  private readonly minParameterValue: number;\n  private readonly maxParameterValue: number;\n  private readonly parameterResolution: number;\n\n  private readonly mnemonicValue: string;\n  private readonly descriptionValue: string;\n  private readonly mnemonicDC: string;\n  private readonly descriptionDC: string;\n  private readonly mnemonicDCM: string;\n  private readonly descriptionDCM: string;\n\n  private readonly barComponentFunc: (bar: Bar) => number;\n  private readonly quoteComponentFunc: (quote: Quote) => number;\n  private readonly tradeComponentFunc: (trade: Trade) => number;\n\n  /** Creates an instance with default parameters\n   * (minRaster=6, maxRaster=20, minParam=6, maxParam=30, hpCutoff=30, BarComponent.Median). */\n  public static default(): CoronaSpectrum {\n    return new CoronaSpectrum({});\n  }\n\n  /** Creates an instance based on the given parameters. */\n  public static fromParams(params: CoronaSpectrumParams): CoronaSpectrum {\n    return new CoronaSpectrum(params);\n  }\n\n  private constructor(params: CoronaSpectrumParams) {\n    const invalid = 'invalid corona spectrum parameters';\n\n    const minRaster = params.minRasterValue !== undefined && params.minRasterValue !== 0\n      ? params.minRasterValue : DEFAULT_MIN_RASTER;\n    const maxRaster = params.maxRasterValue !== undefined && params.maxRasterValue !== 0\n      ? params.maxRasterValue : DEFAULT_MAX_RASTER;\n    const minParamRaw = params.minParameterValue !== undefined && params.minParameterValue !== 0\n      ? params.minParameterValue : DEFAULT_MIN_PARAM;\n    const maxParamRaw = params.maxParameterValue !== undefined && params.maxParameterValue !== 0\n      ? params.maxParameterValue : DEFAULT_MAX_PARAM;\n    const hpCutoff = params.highPassFilterCutoff !== undefined && params.highPassFilterCutoff !== 0\n      ? params.highPassFilterCutoff : DEFAULT_HP_CUTOFF;\n\n    if (minRaster < 0) {\n      throw new Error(`${invalid}: MinRasterValue should be >= 0`);\n    }\n    if (maxRaster <= minRaster) {\n      throw new Error(`${invalid}: MaxRasterValue should be > MinRasterValue`);\n    }\n\n    // MBST rounds min up and max down to integers.\n    const minParam = Math.ceil(minParamRaw);\n    const maxParam = Math.floor(maxParamRaw);\n\n    if (minParam < 2) {\n      throw new Error(`${invalid}: MinParameterValue should be >= 2`);\n    }\n    if (maxParam <= minParam) {\n      throw new Error(`${invalid}: MaxParameterValue should be > MinParameterValue`);\n    }\n    if (hpCutoff < 2) {\n      throw new Error(`${invalid}: HighPassFilterCutoff should be >= 2`);\n    }\n\n    // CoronaSpectrum mirrors Ehlers' reference: BarComponent.Median default.\n    const bc = params.barComponent ?? BarComponent.Median;\n    const qc = params.quoteComponent ?? DefaultQuoteComponent;\n    const tc = params.tradeComponent ?? DefaultTradeComponent;\n\n    this.barComponentFunc = barComponentValue(bc);\n    this.quoteComponentFunc = quoteComponentValue(qc);\n    this.tradeComponentFunc = tradeComponentValue(tc);\n\n    this.c = new Corona({\n      highPassFilterCutoff: hpCutoff,\n      minimalPeriod: minParam,\n      maximalPeriod: maxParam,\n      decibelsLowerThreshold: minRaster,\n      decibelsUpperThreshold: maxRaster,\n    });\n\n    this.minParameterValue = minParam;\n    this.maxParameterValue = maxParam;\n\n    // Values slice length = filterBankLength; first sample at minParam, last at maxParam.\n    this.parameterResolution = (this.c.filterBankLength - 1) / (maxParam - minParam);\n\n    const cm = componentTripleMnemonic(bc, qc, tc);\n    this.mnemonicValue = `cspect(${minRaster}, ${maxRaster}, ${minParam}, ${maxParam}, ${hpCutoff}${cm})`;\n    this.mnemonicDC = `cspect-dc(${hpCutoff}${cm})`;\n    this.mnemonicDCM = `cspect-dcm(${hpCutoff}${cm})`;\n\n    this.descriptionValue = 'Corona spectrum ' + this.mnemonicValue;\n    this.descriptionDC = 'Corona spectrum dominant cycle ' + this.mnemonicDC;\n    this.descriptionDCM = 'Corona spectrum dominant cycle median ' + this.mnemonicDCM;\n  }\n\n  /** Indicates whether the indicator is primed. */\n  public isPrimed(): boolean { return this.c.isPrimed(); }\n\n  /** Describes the output data of the indicator. */\n  public metadata(): IndicatorMetadata {\n    return {\n      type: IndicatorType.CoronaSpectrum,\n      mnemonic: this.mnemonicValue,\n      description: this.descriptionValue,\n      outputs: [\n        {\n          kind: CoronaSpectrumOutput.Value,\n          type: OutputType.Heatmap,\n          mnemonic: this.mnemonicValue,\n          description: this.descriptionValue,\n        },\n        {\n          kind: CoronaSpectrumOutput.DominantCycle,\n          type: OutputType.Scalar,\n          mnemonic: this.mnemonicDC,\n          description: this.descriptionDC,\n        },\n        {\n          kind: CoronaSpectrumOutput.DominantCycleMedian,\n          type: OutputType.Scalar,\n          mnemonic: this.mnemonicDCM,\n          description: this.descriptionDCM,\n        },\n      ],\n    };\n  }\n\n  /** Feeds the next sample to the engine and returns the heatmap column plus the current\n   * DominantCycle and DominantCycleMedian estimates.\n   *\n   * On unprimed bars the heatmap is an empty heatmap (with the indicator's parameter axis)\n   * and both scalar values are NaN. On NaN input, state is left unchanged and all outputs\n   * are NaN / empty heatmap. */\n  public update(sample: number, time: Date): [Heatmap, number, number] {\n    if (Number.isNaN(sample)) {\n      return [\n        Heatmap.newEmptyHeatmap(time, this.minParameterValue, this.maxParameterValue, this.parameterResolution),\n        Number.NaN, Number.NaN,\n      ];\n    }\n\n    const primed = this.c.update(sample);\n    if (!primed) {\n      return [\n        Heatmap.newEmptyHeatmap(time, this.minParameterValue, this.maxParameterValue, this.parameterResolution),\n        Number.NaN, Number.NaN,\n      ];\n    }\n\n    const bank = this.c.filterBank;\n    const values = new Array<number>(bank.length);\n    let valueMin = Number.POSITIVE_INFINITY;\n    let valueMax = Number.NEGATIVE_INFINITY;\n\n    for (let i = 0; i < bank.length; i++) {\n      const v = bank[i].decibels;\n      values[i] = v;\n      if (v < valueMin) valueMin = v;\n      if (v > valueMax) valueMax = v;\n    }\n\n    const heatmap = Heatmap.newHeatmap(\n      time, this.minParameterValue, this.maxParameterValue, this.parameterResolution,\n      valueMin, valueMax, values,\n    );\n\n    return [heatmap, this.c.dominantCycle, this.c.dominantCycleMedian];\n  }\n\n  /** Updates the indicator given the next scalar sample. */\n  public updateScalar(sample: Scalar): IndicatorOutput {\n    return this.updateEntity(sample.time, sample.value);\n  }\n\n  /** Updates the indicator given the next bar sample. */\n  public updateBar(sample: Bar): IndicatorOutput {\n    return this.updateEntity(sample.time, this.barComponentFunc(sample));\n  }\n\n  /** Updates the indicator given the next quote sample. */\n  public updateQuote(sample: Quote): IndicatorOutput {\n    return this.updateEntity(sample.time, this.quoteComponentFunc(sample));\n  }\n\n  /** Updates the indicator given the next trade sample. */\n  public updateTrade(sample: Trade): IndicatorOutput {\n    return this.updateEntity(sample.time, this.tradeComponentFunc(sample));\n  }\n\n  private updateEntity(time: Date, sample: number): IndicatorOutput {\n    const [heatmap, dc, dcm] = this.update(sample, time);\n\n    const sDc = new Scalar();\n    sDc.time = time;\n    sDc.value = dc;\n\n    const sDcm = new Scalar();\n    sDcm.time = time;\n    sDcm.value = dcm;\n\n    return [heatmap, sDc, sDcm];\n  }\n}\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum.ts"
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

## Assistant (Build · claude-opus-4.7 · 7.5s)

_Thinking:_

 Checking that the Indicator interface and its implementations like Scalar and Bar are properly defined, and verifying whether the Bar component needs a median() method or if the utility already calculates (high+low)/2 directly.

**Tool: bash**

**Input:**
```json
{
  "command": "rg \"median\\(\\)\" ts/entities/bar.ts",
  "description": "Check Bar has median method"
}
```

**Output:**
```
/bin/bash: line 1: rg: command not found

```

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: grep**

**Input:**
```json
{
  "pattern": "median|typical|weighted|average",
  "path": "/home/dev/zpano/ts/entities/bar.ts"
}
```

**Output:**
```
Found 8 matches
/home/dev/zpano/ts/entities/bar.ts:
  Line 45:   /** The median price, calculated as _(low + high) / 2_. */
  Line 46:   median(): number {
  Line 50:   /** The typical price, calculated as _(low + high + close) / 3_. */
  Line 51:   typical(): number {
  Line 55:   /** The weighted price, calculated as _(low + high + 2*close) / 4_. */
  Line 56:   weighted(): number {
  Line 60:   /** The average price, calculated as _(low + high + open + close) / 4_. */
  Line 61:   average(): number {
```

---

## Assistant (Build · claude-opus-4.7 · 5.0s)

Good. Now the CoronaSpectrum spec file:

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/indicator.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/core/indicator.ts</path>
<type>file</type>
<content>1: import { Bar } from '../../entities/bar';
2: import { Quote } from '../../entities/quote';
3: import { Scalar } from '../../entities/scalar';
4: import { Trade } from '../../entities/trade';
5: import { IndicatorMetadata } from './indicator-metadata';
6: import { IndicatorOutput } from './indicator-output';
7: 
8: /** Describes a common indicator functionality. */
9: export interface Indicator {
10:     /** Indicates whether an indicator is primed. */
11:     isPrimed(): boolean;
12: 
13:     /** Describes a requested output data of an indicator. */
14:     metadata(): IndicatorMetadata;
15: 
16:     /** Updates an indicator given the next scalar sample. */
17:     updateScalar(sample: Scalar): IndicatorOutput;
18: 
19:     /** Updates an indicator given the next bar sample. */
20:     updateBar(sample: Bar): IndicatorOutput;
21: 
22:     /** Updates an indicator given the next quote sample. */
23:     updateQuote(sample: Quote): IndicatorOutput;
24: 
25:     /** Updates an indicator given the next trade sample. */
26:     updateTrade(sample: Trade): IndicatorOutput;
27: }

(End of file - total 27 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 44.3s)

_Thinking:_

 I'm going to consolidate the test inputs into a shared helper file in the corona folder to avoid repeating 252 entries across multiple specs, then import it where needed.

**Tool: write**

**Input:**
```json
{
  "content": "/* eslint-disable max-len */\n/** The 252-entry TA-Lib MAMA reference series (Price D5…D256) used by the Corona\n * indicator snapshot tests. Kept here to avoid duplicating it across the four\n * Corona indicator spec files.\n *\n * This module is not part of the public indicator API.\n */\nexport const coronaTestInput: readonly number[] = [\n  92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,\n  94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,\n  88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,\n  85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,\n  83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,\n  89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,\n  89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,\n  88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,\n  103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,\n  120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,\n  114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,\n  114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,\n  124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,\n  137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,\n  123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,\n  122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,\n  123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,\n  130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,\n  127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,\n  121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,\n  106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,\n  95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,\n  94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,\n  103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,\n  106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,\n  109.5300, 108.0600,\n];\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona/corona-test-input.ts"
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

## Assistant (Build · claude-opus-4.7 · 39.5s)

**Tool: write**

**Input:**
```json
{
  "content": "import { } from 'jasmine';\n\nimport { Bar } from '../../../entities/bar';\nimport { Quote } from '../../../entities/quote';\nimport { Scalar } from '../../../entities/scalar';\nimport { Trade } from '../../../entities/trade';\nimport { IndicatorType } from '../../core/indicator-type';\nimport { Heatmap } from '../../core/outputs/heatmap';\nimport { OutputType } from '../../core/outputs/output-type';\nimport { coronaTestInput } from '../corona/corona-test-input';\nimport { CoronaSpectrum } from './corona-spectrum';\nimport { CoronaSpectrumOutput } from './corona-spectrum-output';\n\nconst tolerance = 1e-4;\n\ndescribe('CoronaSpectrum', () => {\n  const time = new Date(2021, 3, 1);\n\n  it('should have correct output enum values', () => {\n    expect(CoronaSpectrumOutput.Value).toBe(0);\n    expect(CoronaSpectrumOutput.DominantCycle).toBe(1);\n    expect(CoronaSpectrumOutput.DominantCycleMedian).toBe(2);\n  });\n\n  it('should return expected mnemonic for default params', () => {\n    const x = CoronaSpectrum.default();\n    expect(x.metadata().mnemonic).toBe('cspect(6, 20, 6, 30, 30, hl/2)');\n  });\n\n  it('should round custom param ranges (ceil min, floor max)', () => {\n    const x = CoronaSpectrum.fromParams({\n      minRasterValue: 4,\n      maxRasterValue: 25,\n      minParameterValue: 8.7,   // ceils to 9\n      maxParameterValue: 40.4,  // floors to 40\n      highPassFilterCutoff: 20,\n    });\n    expect(x.metadata().mnemonic).toBe('cspect(4, 25, 9, 40, 20, hl/2)');\n  });\n\n  it('should return expected metadata', () => {\n    const x = CoronaSpectrum.default();\n    const meta = x.metadata();\n    const mn = 'cspect(6, 20, 6, 30, 30, hl/2)';\n    const mnDC = 'cspect-dc(30, hl/2)';\n    const mnDCM = 'cspect-dcm(30, hl/2)';\n\n    expect(meta.type).toBe(IndicatorType.CoronaSpectrum);\n    expect(meta.mnemonic).toBe(mn);\n    expect(meta.description).toBe('Corona spectrum ' + mn);\n    expect(meta.outputs.length).toBe(3);\n\n    expect(meta.outputs[0].kind).toBe(CoronaSpectrumOutput.Value);\n    expect(meta.outputs[0].type).toBe(OutputType.Heatmap);\n    expect(meta.outputs[0].mnemonic).toBe(mn);\n\n    expect(meta.outputs[1].kind).toBe(CoronaSpectrumOutput.DominantCycle);\n    expect(meta.outputs[1].type).toBe(OutputType.Scalar);\n    expect(meta.outputs[1].mnemonic).toBe(mnDC);\n    expect(meta.outputs[1].description).toBe('Corona spectrum dominant cycle ' + mnDC);\n\n    expect(meta.outputs[2].kind).toBe(CoronaSpectrumOutput.DominantCycleMedian);\n    expect(meta.outputs[2].type).toBe(OutputType.Scalar);\n    expect(meta.outputs[2].mnemonic).toBe(mnDCM);\n    expect(meta.outputs[2].description).toBe('Corona spectrum dominant cycle median ' + mnDCM);\n  });\n\n  it('should throw for invalid params', () => {\n    expect(() => CoronaSpectrum.fromParams({ minRasterValue: -1 })).toThrowError(/MinRasterValue/);\n    expect(() => CoronaSpectrum.fromParams({ minRasterValue: 10, maxRasterValue: 10 })).toThrowError(/MaxRasterValue/);\n    expect(() => CoronaSpectrum.fromParams({ minParameterValue: 1 })).toThrowError(/MinParameterValue/);\n    expect(() => CoronaSpectrum.fromParams({ minParameterValue: 20, maxParameterValue: 20 })).toThrowError(/MaxParameterValue/);\n    expect(() => CoronaSpectrum.fromParams({ highPassFilterCutoff: 1 })).toThrowError(/HighPassFilterCutoff/);\n  });\n\n  it('should return empty heatmap and NaN scalars for NaN input', () => {\n    const x = CoronaSpectrum.default();\n    const [h, dc, dcm] = x.update(Number.NaN, time);\n    expect(h instanceof Heatmap).toBe(true);\n    expect(h.isEmpty()).toBe(true);\n    expect(h.parameterFirst).toBe(6);\n    expect(h.parameterLast).toBe(30);\n    expect(h.parameterResolution).toBe(2);\n    expect(Number.isNaN(dc)).toBe(true);\n    expect(Number.isNaN(dcm)).toBe(true);\n    expect(x.isPrimed()).toBe(false);\n  });\n\n  it('should prime at sample index 11 (MinimalPeriodTimesTwo = 12)', () => {\n    const x = CoronaSpectrum.default();\n    expect(x.isPrimed()).toBe(false);\n    let primedAt = -1;\n    for (let i = 0; i < coronaTestInput.length; i++) {\n      x.update(coronaTestInput[i], new Date(time.getTime() + i * 60_000));\n      if (x.isPrimed() && primedAt < 0) primedAt = i;\n    }\n    expect(primedAt).toBe(11);\n  });\n\n  it('should match reference dc / dcm snapshots', () => {\n    const snapshots: Array<[number, number, number]> = [\n      [11, 17.7604672565, 17.7604672565],\n      [12, 6.0000000000, 6.0000000000],\n      [50, 15.9989078712, 15.9989078712],\n      [100, 14.7455497547, 14.7455497547],\n      [150, 17.5000000000, 17.2826036069],\n      [200, 19.7557338512, 20.0000000000],\n      [251, 6.0000000000, 6.0000000000],\n    ];\n\n    const x = CoronaSpectrum.default();\n    let si = 0;\n    for (let i = 0; i < coronaTestInput.length; i++) {\n      const t = new Date(time.getTime() + i * 60_000);\n      const [h, dc, dcm] = x.update(coronaTestInput[i], t);\n\n      expect(h.parameterFirst).toBe(6);\n      expect(h.parameterLast).toBe(30);\n      expect(h.parameterResolution).toBe(2);\n\n      if (!x.isPrimed()) {\n        expect(h.isEmpty()).withContext(`bar ${i}`).toBe(true);\n        expect(Number.isNaN(dc)).toBe(true);\n        expect(Number.isNaN(dcm)).toBe(true);\n        continue;\n      }\n\n      expect(h.values.length).toBe(49);\n\n      if (si < snapshots.length && snapshots[si][0] === i) {\n        expect(Math.abs(snapshots[si][1] - dc))\n          .withContext(`dc[${i}]: expected ${snapshots[si][1]}, actual ${dc}`)\n          .toBeLessThan(tolerance);\n        expect(Math.abs(snapshots[si][2] - dcm))\n          .withContext(`dcm[${i}]: expected ${snapshots[si][2]}, actual ${dcm}`)\n          .toBeLessThan(tolerance);\n        si++;\n      }\n    }\n    expect(si).toBe(snapshots.length);\n  });\n\n  const primeCount = 50;\n  const value = 100.0;\n\n  function checkOutput(out: any[]): void {\n    expect(out.length).toBe(3);\n    expect(out[0] instanceof Heatmap).toBe(true);\n    expect((out[0] as Heatmap).time).toEqual(time);\n    for (let i = 1; i < 3; i++) {\n      const s = out[i] as Scalar;\n      expect(s.time).toEqual(time);\n    }\n  }\n\n  it('should produce 3-element output via updateScalar', () => {\n    const x = CoronaSpectrum.default();\n    for (let i = 0; i < primeCount; i++) {\n      x.update(coronaTestInput[i % coronaTestInput.length], time);\n    }\n    const s = new Scalar();\n    s.time = time;\n    s.value = value;\n    checkOutput(x.updateScalar(s) as any[]);\n  });\n\n  it('should produce 3-element output via updateBar', () => {\n    const x = CoronaSpectrum.default();\n    for (let i = 0; i < primeCount; i++) {\n      x.update(coronaTestInput[i % coronaTestInput.length], time);\n    }\n    const bar = new Bar({ time, open: value, high: value, low: value, close: value, volume: 0 });\n    checkOutput(x.updateBar(bar) as any[]);\n  });\n\n  it('should produce 3-element output via updateQuote', () => {\n    const x = CoronaSpectrum.default();\n    for (let i = 0; i < primeCount; i++) {\n      x.update(coronaTestInput[i % coronaTestInput.length], time);\n    }\n    const q = new Quote({ time, bid: value, ask: value, bidSize: 0, askSize: 0 });\n    checkOutput(x.updateQuote(q) as any[]);\n  });\n\n  it('should produce 3-element output via updateTrade', () => {\n    const x = CoronaSpectrum.default();\n    for (let i = 0; i < primeCount; i++) {\n      x.update(coronaTestInput[i % coronaTestInput.length], time);\n    }\n    const t = new Trade({ time, price: value, volume: 0 });\n    checkOutput(x.updateTrade(t) as any[]);\n  });\n});\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum.spec.ts"
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

## Assistant (Build · claude-opus-4.7 · 5.4s)

Let me run the Jasmine test to verify CSpectrum before moving on to the other three:

**Tool: bash**

**Input:**
```json
{
  "command": "cat ts/package.json 2>/dev/null | head -50; ls ts/ 2>/dev/null | head -30",
  "description": "Find package.json and test command"
}
```

**Output:**
```
{
  "name": "ts",
  "version": "1.0.0",
  "description": "",
  "main": "index.js",
  "scripts": {
    "test": "node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json"
  },
  "keywords": [],
  "author": "",
  "license": "ISC",
  "type": "module",
  "devDependencies": {
    "@types/jasmine": "^6.0.0",
    "@types/node": "^25.6.0",
    "jasmine": "^6.2.0",
    "ts-node": "^10.9.2",
    "tsx": "^4.21.0",
    "typescript": "^6.0.2"
  }
}
entities
indicators
jasmine.json
node_modules
package-lock.json
package.json
spec
tsconfig.json

```

---

## Assistant (Build · claude-opus-4.7 · 5.9s)

**Tool: bash**

**Input:**
```json
{
  "command": "npm test -- --filter=CoronaSpectrum 2>&1 | tail -80",
  "description": "Run CoronaSpectrum tests",
  "workdir": "/home/dev/zpano/ts"
}
```

**Output:**
```

> ts@1.0.0 test
> node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=CoronaSpectrum

Started
............


Ran 12 of 761 specs
12 specs, 0 failures
Finished in 0.054 seconds

```

---

## Assistant (Build · claude-opus-4.7 · 6.1s)

_Thinking:_

 Great, all the CSpectrum tests passed without issues. Now I'm moving on to check the CSNR implementation in Go.

All 12 CSpectrum specs pass. Now CSNR:

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio.go</path>
<type>file</type>
<content>1: // Package coronasignaltonoiseratio implements Ehlers' Corona Signal-to-Noise
2: // Ratio heatmap indicator.
3: //
4: // The Corona Signal to Noise Ratio is a measure of the cycle amplitude relative
5: // to noise. The "noise" is chosen to be the average bar height because there is
6: // not much trade information within the bar.
7: //
8: // Reference: John Ehlers, "Measuring Cycle Periods", Stocks & Commodities,
9: // November 2008.
10: package coronasignaltonoiseratio
11: 
12: //nolint: gofumpt
13: import (
14: 	"fmt"
15: 	"math"
16: 	"sort"
17: 	"sync"
18: 	"time"
19: 
20: 	"zpano/entities"
21: 	"zpano/indicators/core"
22: 	"zpano/indicators/core/outputs"
23: 	"zpano/indicators/johnehlers/corona"
24: )
25: 
26: const (
27: 	highLowBufferSize       = 5
28: 	highLowBufferSizeMinOne = highLowBufferSize - 1
29: 	highLowMedianIndex      = 2
30: 	averageSampleAlpha      = 0.1
31: 	averageSampleOneMinus   = 0.9
32: 	signalEmaAlpha          = 0.2
33: 	signalEmaOneMinus       = 0.9 // Intentional: sums to 1.1, per Ehlers.
34: 	noiseEmaAlpha           = 0.1
35: 	noiseEmaOneMinus        = 0.9
36: 	ratioOffsetDb           = 3.5
37: 	ratioUpperDb            = 10.
38: 	dbGain                  = 20.
39: 	widthLowRatioThreshold  = 0.5
40: 	widthBaseline           = 0.2
41: 	widthSlope              = 0.4
42: 	rasterBlendExponent     = 0.8
43: 	rasterBlendHalf         = 0.5
44: 	rasterNegativeArgCutoff = 1.
45: )
46: 
47: // CoronaSignalToNoiseRatio is Ehlers' Corona Signal-to-Noise Ratio indicator.
48: //
49: // It owns a private Corona spectral-analysis engine and exposes two outputs:
50: //
51: //   - Value: a per-bar heatmap column (intensity raster).
52: //   - SignalToNoiseRatio: the current SNR value mapped into
53: //     [MinParameterValue, MaxParameterValue].
54: type CoronaSignalToNoiseRatio struct {
55: 	mu                  sync.RWMutex
56: 	mnemonic            string
57: 	description         string
58: 	mnemonicSNR         string
59: 	descriptionSNR      string
60: 	c                   *corona.Corona
61: 	rasterLength        int
62: 	rasterStep          float64
63: 	maxRasterValue      float64
64: 	minParameterValue   float64
65: 	maxParameterValue   float64
66: 	parameterResolution float64
67: 	raster              []float64
68: 	highLowBuffer       [highLowBufferSize]float64
69: 	hlSorted            [highLowBufferSize]float64
70: 	averageSamplePrev   float64
71: 	signalPrev          float64
72: 	noisePrev           float64
73: 	signalToNoiseRatio  float64
74: 	isStarted           bool
75: 	barFunc             entities.BarFunc
76: 	quoteFunc           entities.QuoteFunc
77: 	tradeFunc           entities.TradeFunc
78: }
79: 
80: // NewCoronaSignalToNoiseRatioDefault returns an instance created with default parameters.
81: func NewCoronaSignalToNoiseRatioDefault() (*CoronaSignalToNoiseRatio, error) {
82: 	return NewCoronaSignalToNoiseRatioParams(&Params{})
83: }
84: 
85: // NewCoronaSignalToNoiseRatioParams returns an instance created with the supplied parameters.
86: //
87: //nolint:funlen,cyclop
88: func NewCoronaSignalToNoiseRatioParams(p *Params) (*CoronaSignalToNoiseRatio, error) {
89: 	const (
90: 		invalid      = "invalid corona signal to noise ratio parameters"
91: 		fmtRaster    = "%s: RasterLength should be >= 2"
92: 		fmtMaxRaster = "%s: MaxRasterValue should be > 0"
93: 		fmtMinParam  = "%s: MinParameterValue should be >= 0"
94: 		fmtMaxParam  = "%s: MaxParameterValue should be > MinParameterValue"
95: 		fmtHP        = "%s: HighPassFilterCutoff should be >= 2"
96: 		fmtMinP      = "%s: MinimalPeriod should be >= 2"
97: 		fmtMaxP      = "%s: MaximalPeriod should be > MinimalPeriod"
98: 		fmtw         = "%s: %w"
99: 		fmtnVal      = "csnr(%d, %g, %g, %g, %d%s)"
100: 		fmtnSNR      = "csnr-snr(%d%s)"
101: 		descrVal     = "Corona signal to noise ratio "
102: 		descrSNR     = "Corona signal to noise ratio scalar "
103: 		defRaster    = 50
104: 		defMaxRast   = 20.0
105: 		defMinParam  = 1.0
106: 		defMaxParam  = 11.0
107: 		defHPCutoff  = 30
108: 		defMinPer    = 6
109: 		defMaxPer    = 30
110: 	)
111: 
112: 	cfg := *p
113: 
114: 	if cfg.RasterLength == 0 {
115: 		cfg.RasterLength = defRaster
116: 	}
117: 
118: 	if cfg.MaxRasterValue == 0 {
119: 		cfg.MaxRasterValue = defMaxRast
120: 	}
121: 
122: 	if cfg.MinParameterValue == 0 {
123: 		cfg.MinParameterValue = defMinParam
124: 	}
125: 
126: 	if cfg.MaxParameterValue == 0 {
127: 		cfg.MaxParameterValue = defMaxParam
128: 	}
129: 
130: 	if cfg.HighPassFilterCutoff == 0 {
131: 		cfg.HighPassFilterCutoff = defHPCutoff
132: 	}
133: 
134: 	if cfg.MinimalPeriod == 0 {
135: 		cfg.MinimalPeriod = defMinPer
136: 	}
137: 
138: 	if cfg.MaximalPeriod == 0 {
139: 		cfg.MaximalPeriod = defMaxPer
140: 	}
141: 
142: 	if cfg.RasterLength < 2 {
143: 		return nil, fmt.Errorf(fmtRaster, invalid)
144: 	}
145: 
146: 	if cfg.MaxRasterValue <= 0 {
147: 		return nil, fmt.Errorf(fmtMaxRaster, invalid)
148: 	}
149: 
150: 	if cfg.MinParameterValue < 0 {
151: 		return nil, fmt.Errorf(fmtMinParam, invalid)
152: 	}
153: 
154: 	if cfg.MaxParameterValue <= cfg.MinParameterValue {
155: 		return nil, fmt.Errorf(fmtMaxParam, invalid)
156: 	}
157: 
158: 	if cfg.HighPassFilterCutoff < 2 {
159: 		return nil, fmt.Errorf(fmtHP, invalid)
160: 	}
161: 
162: 	if cfg.MinimalPeriod < 2 {
163: 		return nil, fmt.Errorf(fmtMinP, invalid)
164: 	}
165: 
166: 	if cfg.MaximalPeriod <= cfg.MinimalPeriod {
167: 		return nil, fmt.Errorf(fmtMaxP, invalid)
168: 	}
169: 
170: 	// Ehlers reference uses (High+Low)/2.
171: 	bc := cfg.BarComponent
172: 	if bc == 0 {
173: 		bc = entities.BarMedianPrice
174: 	}
175: 
176: 	qc := cfg.QuoteComponent
177: 	if qc == 0 {
178: 		qc = entities.DefaultQuoteComponent
179: 	}
180: 
181: 	tc := cfg.TradeComponent
182: 	if tc == 0 {
183: 		tc = entities.DefaultTradeComponent
184: 	}
185: 
186: 	c, err := corona.NewCorona(&corona.Params{
187: 		HighPassFilterCutoff: cfg.HighPassFilterCutoff,
188: 		MinimalPeriod:        cfg.MinimalPeriod,
189: 		MaximalPeriod:        cfg.MaximalPeriod,
190: 	})
191: 	if err != nil {
192: 		return nil, fmt.Errorf(fmtw, invalid, err)
193: 	}
194: 
195: 	barFunc, err := entities.BarComponentFunc(bc)
196: 	if err != nil {
197: 		return nil, fmt.Errorf(fmtw, invalid, err)
198: 	}
199: 
200: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
201: 	if err != nil {
202: 		return nil, fmt.Errorf(fmtw, invalid, err)
203: 	}
204: 
205: 	tradeFunc, err := entities.TradeComponentFunc(tc)
206: 	if err != nil {
207: 		return nil, fmt.Errorf(fmtw, invalid, err)
208: 	}
209: 
210: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
211: 
212: 	mnemonicVal := fmt.Sprintf(fmtnVal,
213: 		cfg.RasterLength, cfg.MaxRasterValue, cfg.MinParameterValue, cfg.MaxParameterValue,
214: 		cfg.HighPassFilterCutoff, componentMnemonic)
215: 	mnemonicSNR := fmt.Sprintf(fmtnSNR, cfg.HighPassFilterCutoff, componentMnemonic)
216: 
217: 	// Resolution satisfies: min + (rasterLength-1)/resolution = max.
218: 	paramResolution := float64(cfg.RasterLength-1) / (cfg.MaxParameterValue - cfg.MinParameterValue)
219: 
220: 	return &CoronaSignalToNoiseRatio{
221: 		mnemonic:            mnemonicVal,
222: 		description:         descrVal + mnemonicVal,
223: 		mnemonicSNR:         mnemonicSNR,
224: 		descriptionSNR:      descrSNR + mnemonicSNR,
225: 		c:                   c,
226: 		rasterLength:        cfg.RasterLength,
227: 		rasterStep:          cfg.MaxRasterValue / float64(cfg.RasterLength),
228: 		maxRasterValue:      cfg.MaxRasterValue,
229: 		minParameterValue:   cfg.MinParameterValue,
230: 		maxParameterValue:   cfg.MaxParameterValue,
231: 		parameterResolution: paramResolution,
232: 		raster:              make([]float64, cfg.RasterLength),
233: 		signalToNoiseRatio:  math.NaN(),
234: 		barFunc:             barFunc,
235: 		quoteFunc:           quoteFunc,
236: 		tradeFunc:           tradeFunc,
237: 	}, nil
238: }
239: 
240: // IsPrimed indicates whether the indicator is primed.
241: func (s *CoronaSignalToNoiseRatio) IsPrimed() bool {
242: 	s.mu.RLock()
243: 	defer s.mu.RUnlock()
244: 
245: 	return s.c.IsPrimed()
246: }
247: 
248: // Metadata describes the output data of the indicator.
249: func (s *CoronaSignalToNoiseRatio) Metadata() core.Metadata {
250: 	return core.Metadata{
251: 		Type:        core.CoronaSignalToNoiseRatio,
252: 		Mnemonic:    s.mnemonic,
253: 		Description: s.description,
254: 		Outputs: []outputs.Metadata{
255: 			{Kind: int(Value), Type: outputs.HeatmapType, Mnemonic: s.mnemonic, Description: s.description},
256: 			{Kind: int(SignalToNoiseRatio), Type: outputs.ScalarType, Mnemonic: s.mnemonicSNR, Description: s.descriptionSNR},
257: 		},
258: 	}
259: }
260: 
261: // Update feeds the next sample plus bar extremes and returns the heatmap column
262: // and the current SignalToNoiseRatio. On unprimed bars the heatmap is empty and
263: // the scalar is NaN. On NaN sample input state is left unchanged.
264: //
265: //nolint:funlen,cyclop,gocognit
266: func (s *CoronaSignalToNoiseRatio) Update(sample, sampleLow, sampleHigh float64, t time.Time) (*outputs.Heatmap, float64) {
267: 	s.mu.Lock()
268: 	defer s.mu.Unlock()
269: 
270: 	if math.IsNaN(sample) {
271: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
272: 	}
273: 
274: 	primed := s.c.Update(sample)
275: 
276: 	if !s.isStarted {
277: 		s.averageSamplePrev = sample
278: 		s.highLowBuffer[highLowBufferSizeMinOne] = sampleHigh - sampleLow
279: 		s.isStarted = true
280: 
281: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
282: 	}
283: 
284: 	maxAmpSq := s.c.MaximalAmplitudeSquared()
285: 
286: 	averageSample := averageSampleAlpha*sample + averageSampleOneMinus*s.averageSamplePrev
287: 	s.averageSamplePrev = averageSample
288: 
289: 	if math.Abs(averageSample) > 0 || maxAmpSq > 0 {
290: 		s.signalPrev = signalEmaAlpha*math.Sqrt(maxAmpSq) + signalEmaOneMinus*s.signalPrev
291: 	}
292: 
293: 	// Shift H-L ring buffer left; push new value.
294: 	for i := 0; i < highLowBufferSizeMinOne; i++ {
295: 		s.highLowBuffer[i] = s.highLowBuffer[i+1]
296: 	}
297: 	s.highLowBuffer[highLowBufferSizeMinOne] = sampleHigh - sampleLow
298: 
299: 	ratio := 0.0
300: 	if math.Abs(averageSample) > 0 {
301: 		for i := 0; i < highLowBufferSize; i++ {
302: 			s.hlSorted[i] = s.highLowBuffer[i]
303: 		}
304: 
305: 		sort.Float64s(s.hlSorted[:])
306: 		s.noisePrev = noiseEmaAlpha*s.hlSorted[highLowMedianIndex] + noiseEmaOneMinus*s.noisePrev
307: 
308: 		if math.Abs(s.noisePrev) > 0 {
309: 			ratio = dbGain*math.Log10(s.signalPrev/s.noisePrev) + ratioOffsetDb
310: 			if ratio < 0 {
311: 				ratio = 0
312: 			} else if ratio > ratioUpperDb {
313: 				ratio = ratioUpperDb
314: 			}
315: 
316: 			ratio /= ratioUpperDb // ∈ [0, 1]
317: 		}
318: 	}
319: 
320: 	s.signalToNoiseRatio = (s.maxParameterValue-s.minParameterValue)*ratio + s.minParameterValue
321: 
322: 	// Raster update.
323: 	width := 0.0
324: 	if ratio <= widthLowRatioThreshold {
325: 		width = widthBaseline - widthSlope*ratio
326: 	}
327: 
328: 	ratioScaledToRasterLength := int(math.Round(ratio * float64(s.rasterLength)))
329: 	ratioScaledToMaxRasterValue := ratio * s.maxRasterValue
330: 
331: 	for i := 0; i < s.rasterLength; i++ {
332: 		value := s.raster[i]
333: 
334: 		switch {
335: 		case i == ratioScaledToRasterLength:
336: 			value *= 0.5
337: 		case width == 0:
338: 			// Above the high-ratio threshold: handled by the ratio>0.5 override below.
339: 		default:
340: 			argument := (ratioScaledToMaxRasterValue - s.rasterStep*float64(i)) / width
341: 			if i < ratioScaledToRasterLength {
342: 				value = rasterBlendHalf * (math.Pow(argument, rasterBlendExponent) + value)
343: 			} else {
344: 				argument = -argument
345: 				if argument > rasterNegativeArgCutoff {
346: 					value = rasterBlendHalf * (math.Pow(argument, rasterBlendExponent) + value)
347: 				} else {
348: 					value = s.maxRasterValue
349: 				}
350: 			}
351: 		}
352: 
353: 		if value < 0 {
354: 			value = 0
355: 		} else if value > s.maxRasterValue {
356: 			value = s.maxRasterValue
357: 		}
358: 
359: 		if ratio > widthLowRatioThreshold {
360: 			value = s.maxRasterValue
361: 		}
362: 
363: 		s.raster[i] = value
364: 	}
365: 
366: 	if !primed {
367: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
368: 	}
369: 
370: 	values := make([]float64, s.rasterLength)
371: 	valueMin := math.Inf(1)
372: 	valueMax := math.Inf(-1)
373: 
374: 	for i := 0; i < s.rasterLength; i++ {
375: 		v := s.raster[i]
376: 		values[i] = v
377: 
378: 		if v < valueMin {
379: 			valueMin = v
380: 		}
381: 
382: 		if v > valueMax {
383: 			valueMax = v
384: 		}
385: 	}
386: 
387: 	heatmap := outputs.NewHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution,
388: 		valueMin, valueMax, values)
389: 
390: 	return heatmap, s.signalToNoiseRatio
391: }
392: 
393: // UpdateScalar updates the indicator given the next scalar sample. Since no
394: // High/Low is available, the sample is used for both, yielding zero noise.
395: func (s *CoronaSignalToNoiseRatio) UpdateScalar(sample *entities.Scalar) core.Output {
396: 	return s.updateEntity(sample.Time, sample.Value, sample.Value, sample.Value)
397: }
398: 
399: // UpdateBar updates the indicator given the next bar sample.
400: func (s *CoronaSignalToNoiseRatio) UpdateBar(sample *entities.Bar) core.Output {
401: 	return s.updateEntity(sample.Time, s.barFunc(sample), sample.Low, sample.High)
402: }
403: 
404: // UpdateQuote updates the indicator given the next quote sample.
405: func (s *CoronaSignalToNoiseRatio) UpdateQuote(sample *entities.Quote) core.Output {
406: 	v := s.quoteFunc(sample)
407: 
408: 	return s.updateEntity(sample.Time, v, v, v)
409: }
410: 
411: // UpdateTrade updates the indicator given the next trade sample.
412: func (s *CoronaSignalToNoiseRatio) UpdateTrade(sample *entities.Trade) core.Output {
413: 	v := s.tradeFunc(sample)
414: 
415: 	return s.updateEntity(sample.Time, v, v, v)
416: }
417: 
418: func (s *CoronaSignalToNoiseRatio) updateEntity(t time.Time, sample, low, high float64) core.Output {
419: 	const length = 2
420: 
421: 	heatmap, snr := s.Update(sample, low, high, t)
422: 
423: 	output := make([]any, length)
424: 	output[0] = heatmap
425: 	output[1] = entities.Scalar{Time: t, Value: snr}
426: 
427: 	return output
428: }

(End of file - total 428 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 7.7s)

_Thinking:_

 I need to add a public getter for `maximalAmplitudeSquared` to `signalPrev` to match what the TS Corona implementation has, then I'm going to write out the remaining three packages together—CSNR, CSwing, and CTV.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go",
  "offset": 1,
  "limit": 100
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go</path>
<type>file</type>
<content>1: //nolint:testpackage
2: package coronasignaltonoiseratio
3: 
4: import (
5: 	"math"
6: 	"testing"
7: 	"time"
8: 
9: 	"zpano/entities"
10: 	"zpano/indicators/core"
11: 	"zpano/indicators/core/outputs"
12: )
13: 
14: func testCSNRTime() time.Time {
15: 	return time.Date(2021, time.April, 1, 0, 0, 0, 0, &time.Location{})
16: }
17: 
18: // testCSNRInput is the 252-entry TA-Lib MAMA reference series.
19: //
20: //nolint:dupl
21: func testCSNRInput() []float64 {
22: 	return []float64{
23: 		92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,
24: 		94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,
25: 		88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,
26: 		85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,
27: 		83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,
28: 		89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,
29: 		89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,
30: 		88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,
31: 		103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,
32: 		120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,
33: 		114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,
34: 		114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,
35: 		124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,
36: 		137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,
37: 		123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,
38: 		122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,
39: 		123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,
40: 		130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,
41: 		127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,
42: 		121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,
43: 		106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,
44: 		95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,
45: 		94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,
46: 		103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,
47: 		106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,
48: 		109.5300, 108.0600,
49: 	}
50: }
51: 
52: const testCSNRTolerance = 1e-4
53: 
54: // Produce synthetic High/Low around the sample. The spread is varied to
55: // exercise the full range of the SNR logic (narrow spreads → high SNR,
56: // wide spreads → low SNR).
57: func makeHL(i int, sample float64) (float64, float64) {
58: 	// Fraction cycles between ~0.5% and ~6% of sample.
59: 	frac := 0.005 + 0.03*(1+math.Sin(float64(i)*0.37))
60: 	half := sample * frac
61: 
62: 	return sample - half, sample + half
63: }
64: 
65: func TestCoronaSignalToNoiseRatioUpdate(t *testing.T) {
66: 	t.Parallel()
67: 
68: 	input := testCSNRInput()
69: 	t0 := testCSNRTime()
70: 
71: 	// Snapshot values captured from a first run and locked in here.
72: 	type snap struct {
73: 		i   int
74: 		snr float64
75: 		vmn float64
76: 		vmx float64
77: 	}
78: 	snapshots := []snap{
79: 		{11, 1.0000000000, 0.0000000000, 20.0000000000},
80: 		{12, 1.0000000000, 0.0000000000, 20.0000000000},
81: 		{50, 1.0000000000, 0.0000000000, 20.0000000000},
82: 		{100, 2.9986583538, 4.2011609652, 20.0000000000},
83: 		{150, 1.0000000000, 0.0000000035, 20.0000000000},
84: 		{200, 1.0000000000, 0.0000000000, 20.0000000000},
85: 		{251, 1.0000000000, 0.0000000026, 20.0000000000},
86: 	}
87: 
88: 	x, err := NewCoronaSignalToNoiseRatioDefault()
89: 	if err != nil {
90: 		t.Fatalf("unexpected error: %v", err)
91: 	}
92: 
93: 	si := 0
94: 	for i := range input {
95: 		low, high := makeHL(i, input[i])
96: 		h, snr := x.Update(input[i], low, high, t0.Add(time.Duration(i)*time.Minute))
97: 
98: 		if h == nil {
99: 			t.Fatalf("[%d] heatmap must not be nil", i)
100: 		}

(Showing lines 1-100 of 393. Use offset=101 to continue.)
</content>
```

---

## User

---

## Assistant (Compaction · claude-opus-4.7 · 96.6s)

---
## Goal

Port MBST's **Ehlers Corona Charts suite** (TASC November 2008) from `/home/dev/zpano/mbst-to-convert/john-ehlers/corona/` into zpano:

- **Corona** shared helper (not registered as indicator)
- **CoronaSpectrum** — heatmap + DC + DCM scalars
- **CoronaSignalToNoiseRatio** — heatmap + SNR scalar
- **CoronaSwingPosition** — heatmap + SwingPosition scalar
- **CoronaTrendVigor** — heatmap + TrendVigor scalar

First heatmap-producing indicators in zpano. **Go phase complete.** **Currently in Phase 6: TypeScript port — helper, type enum, heatmap factories, shared test input, and CoronaSpectrum package (with all 12 specs passing) are done. Next: 3 remaining indicator packages.**

## Instructions

### Design decisions (confirmed)
- Unprimed bars → empty heatmap (axis metadata present, values=[]); scalars NaN.
- Mnemonics: `cspect(…)`, `cspect-dc(…)`, `cspect-dcm(…)`, `csnr(…)`, `csnr-snr(…)`, `cswing(…)`, `cswing-sp(…)`, `ctv(…)`, `ctv-tv(…)`.
- User paused after Go phase; said "yes please" to continue; then: "continue if you have next steps, or stop and ask for clarification if you are unsure how to proceed."
- Snapshot tests reuse 252-entry TA-Lib MAMA series with tolerance 1e-4.
- **Ignore LSP errors from `/home/dev/zpano/mbst-to-convert/common/*.c` — unrelated C/ta-lib noise.**
- Test command: `npm test` from `/home/dev/zpano/ts` (or `npm test -- --filter=X`).

### Algorithm decisions (locked in Go, mirrored in TS)
- Swing BP2 momentum: 2-bar (`sample - samplePrev2`).
- TrendVigor lookback: `int(DCM - 1)`; guard `cyclePeriod >= 1` (MBST fix); clamp upper at `len(sampleBuffer)`.
- SNR signal EMA coefficients: 0.2/0.9 (sum 1.1, intentional per Ehlers).
- DC weighted average: N=12..60 indexing; DC=0.5·num/denom; clamp DC <minPeriod. DCM = median of 5-buffer; clamp <minPeriod.
- DominantCycleBuffer initialized to `Number.MAX_VALUE` sentinels.
- CSpectrum rounds `minParam` up (ceil), `maxParam` down (floor).
- CSNR/CSwing/CTV: first sample store-only ("isStarted") returns empty heatmap, NaN scalar.
- CSNR scalar path: H=L=sample ⇒ SNR=MinParameterValue.

### Defaults / formulas
- Corona: `HP=30, minPer=6, maxPer=30, dBlow=6, dBhigh=20`.
- CSpectrum: `minRaster=6, maxRaster=20, minParam=6, maxParam=30, hpCutoff=30, BarComponent=Median`.
- CSNR: `rasterLength=50, maxRaster=20, minParam=1, maxParam=11, HP=30, minPer=6, maxPer=30`.
- CSwing: `rasterLength=50, maxRaster=20, minParam=-5, maxParam=5` (substitute defaults only when both min and max are 0).
- CTV: `rasterLength=50, maxRaster=20, minParam=-10, maxParam=10` (ditto).
- `ParameterResolution = (rasterLength-1)/(maxParam-minParam)`:
  - CSpectrum: `48/24 = 2.0`
  - CSNR: `49/10 = 4.9`
  - CSwing: `49/10 = 4.9`
  - CTV: `49/20 = 2.45`

### TS conventions (discovered)
- Use `class X implements Indicator` with private constructor + `static default()` + `static fromParams(params)`.
- `componentTripleMnemonic(bc, qc, tc)` — pass `BarComponent.Median` explicitly to get `, hl/2` suffix.
- `update()` returns tuple; `updateEntity()` maps tuple → `IndicatorOutput` (array of Scalar/Band/Heatmap).
- `new Scalar()` then assign `time`/`value`; `new Bar({...})`/`new Quote({...})`/`new Trade({...})` with constructor-object.
- `Heatmap` has `Heatmap.newHeatmap(...)`, `Heatmap.newEmptyHeatmap(...)`, `.isEmpty()`.
- Test framework: Jasmine, `import { } from 'jasmine'`, tolerance 1e-4.
- `Math.log10`, `[...arr].sort((a,b)=>a-b)`, `Number.MAX_VALUE`, `Number.POSITIVE/NEGATIVE_INFINITY`.
- Go `%g` prints integer-valued floats without decimal (`6` not `6.0`); JS `String(n)` or template literal matches this perfectly for our values.
- Enum values start from 0 in TS (TCM convention), not 1 like Go.

## Discoveries

### Snapshot values (from Go tests — locked reference)

**CSpectrum** (`dc`, `dcm` at bars 11,12,50,100,150,200,251):
```
{11, 17.7604672565, 17.7604672565}
{12, 6.0000000000, 6.0000000000}
{50, 15.9989078712, 15.9989078712}
{100, 14.7455497547, 14.7455497547}
{150, 17.5000000000, 17.2826036069}
{200, 19.7557338512, 20.0000000000}
{251, 6.0000000000, 6.0000000000}
```

**CSNR** uses `makeHL(i, sample)`:
```go
frac = 0.005 + 0.03*(1 + Math.sin(i*0.37))
half = sample * frac
[low, high] = [sample - half, sample + half]
```
Snapshots (`snr`, `vmin`, `vmax`):
```
{11, 1.0, 0.0, 20.0}
{12, 1.0, 0.0, 20.0}
{50, 1.0, 0.0, 20.0}
{100, 2.9986583538, 4.2011609652, 20.0}
{150, 1.0, 0.0000000035, 20.0}
{200, 1.0, 0.0, 20.0}
{251, 1.0, 0.0000000026, 20.0}
```

**CSwing** (`sp`, `vmin`, `vmax`):
```
{11, 5.0, 20.0, 20.0}
{12, 5.0, 20.0, 20.0}
{50, 4.5384908349, 20.0, 20.0}
{100, -3.8183742675, 3.4957777081, 20.0}
{150, -1.8516194371, 5.3792287864, 20.0}
{200, -3.6944428668, 4.2580825738, 20.0}
{251, -0.8524812061, 4.4822539784, 20.0}
```

**CTV** (`tv`, `vmin`, `vmax`):
```
{11, 5.6512200755, 20.0, 20.0}
{12, 6.8379492897, 20.0, 20.0}
{50, 2.6145116709, 2.3773561485, 20.0}
{100, 2.7536803664, 2.4892742850, 20.0}
{150, -6.4606404251, 20.0, 20.0}
{200, -10.0, 20.0, 20.0}
{251, -0.1894989954, 0.5847573715, 20.0}
```

### Mnemonic expectations (from Go tests — confirmed passing for CSpectrum)
- CSpectrum default: `cspect(6, 20, 6, 30, 30, hl/2)`; `cspect-dc(30, hl/2)`; `cspect-dcm(30, hl/2)`.
- CSpectrum custom: `cspect(4, 25, 9, 40, 20, hl/2)` with `min=8.7`(→9 ceil), `max=40.4`(→40 floor).
- CSNR default: `csnr(50, 20, 1, 11, 30, hl/2)`; `csnr-snr(30, hl/2)`.
- CSwing default: `cswing(50, 20, -5, 5, 30, hl/2)`; `cswing-sp(30, hl/2)`.
- CTV default: `ctv(50, 20, -10, 10, 30, hl/2)`; `ctv-tv(30, hl/2)`.

### CSNR Go update signature nuance
`Update(sample, sampleLow, sampleHigh, time)` — note order `low, high`. `UpdateBar` calls `updateEntity(sample.Time, s.barFunc(sample), sample.Low, sample.High)`. `UpdateScalar/Quote/Trade` pass `(v, v, v)`.

### CSNR mnemonic format
`fmt.Sprintf("csnr(%d, %g, %g, %g, %d%s)", RasterLength, MaxRasterValue, MinParam, MaxParam, HPCutoff, cm)`. So for defaults: `csnr(50, 20, 1, 11, 30, hl/2)`.

## Accomplished

### ✅ Go Phase Complete (all tests green)
- Corona helper, 4 `core.Type` constants, 4 indicator packages with tests.

### ✅ TS Phase 6 — progress so far
1. **IndicatorType enum**: 4 entries appended (`CoronaSpectrum`, `CoronaSignalToNoiseRatio`, `CoronaSwingPosition`, `CoronaTrendVigor`).
2. **Heatmap class**: added `newHeatmap`, `newEmptyHeatmap`, `isEmpty()`.
3. **TS Corona helper**:
   - `corona/corona-params.ts` — `CoronaParams` interface + `DefaultCoronaParams`.
   - `corona/corona.ts` — full Corona engine class.
4. **Shared test input**: `corona/corona-test-input.ts` — 252-entry `coronaTestInput` readonly array, imported by spec files to avoid duplication.
5. **CoronaSpectrum package** (✅ all 12 specs passing):
   - `corona-spectrum/corona-spectrum-output.ts`
   - `corona-spectrum/corona-spectrum-params.ts`
   - `corona-spectrum/corona-spectrum.ts`
   - `corona-spectrum/corona-spectrum.spec.ts`

Test run: `Ran 12 of 761 specs, 12 specs, 0 failures, Finished in 0.054 seconds`.

### ⏳ Remaining TS work (execution order)

6. **CoronaSignalToNoiseRatio package** (← current):
   - `corona-signal-to-noise-ratio/corona-signal-to-noise-ratio-output.ts` (`Value=0, SignalToNoiseRatio=1`)
   - `corona-signal-to-noise-ratio/corona-signal-to-noise-ratio-params.ts` — fields: `rasterLength, maxRasterValue, minParameterValue, maxParameterValue, highPassFilterCutoff, minimalPeriod, maximalPeriod, barComponent, quoteComponent, tradeComponent`.
   - `corona-signal-to-noise-ratio/corona-signal-to-noise-ratio.ts` — port from `/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio.go`. Key: `update(sample, low, high, time) → [Heatmap, number]`. Uses `Corona.maximalAmplitudeSquared` getter. Pre-allocated `raster: number[]` of length rasterLength. Constants (see Go file lines 26-45): `highLowBufferSize=5, averageSampleAlpha=0.1, signalEmaAlpha=0.2, signalEmaOneMinus=0.9 (sum=1.1 intentional), noiseEmaAlpha=0.1, ratioOffsetDb=3.5, ratioUpperDb=10, dbGain=20, widthLowRatioThreshold=0.5, widthBaseline=0.2, widthSlope=0.4, rasterBlendExponent=0.8, rasterBlendHalf=0.5, rasterNegativeArgCutoff=1`. Note the `!primed` return-after-raster-update branch.
   - `corona-signal-to-noise-ratio/corona-signal-to-noise-ratio.spec.ts` — import shared `coronaTestInput`, implement `makeHL(i, sample)` helper, use snapshots above.

7. **CoronaSwingPosition package** (same 4-file layout).
8. **CoronaTrendVigor package** (same 4-file layout).
9. Run `npm test` after each; all specs must pass.
10. Update `.opencode/skills/mbst-indicator-conversion/SKILL.md` with Corona learnings (heatmap conventions, first-bar store-only pattern, DCM-1 lookback fix with `>=1` guard, parameter-resolution formula, empty-heatmap-axis-always-present invariant, CSNR coefficient sum=1.1 is intentional).

## Relevant files / directories

### Go reference (already read/ported)
- `/home/dev/zpano/go/indicators/johnehlers/corona/`
- `/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/` (5 files — fully ported)
- `/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio.go` — read (next to port). `_test.go` also read.
- `/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition.go` — to read next.
- `/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor.go` — to read next.

### TS infrastructure (read/edited)
- `/home/dev/zpano/ts/indicators/core/indicator-type.ts` ✅ (4 entries appended)
- `/home/dev/zpano/ts/indicators/core/outputs/heatmap.ts` ✅ (factories + isEmpty added)
- `/home/dev/zpano/ts/indicators/core/outputs/output-type.ts` (has `Heatmap`)
- `/home/dev/zpano/ts/indicators/core/component-triple-mnemonic.ts`
- `/home/dev/zpano/ts/indicators/core/indicator.ts` + `indicator-metadata.ts` + `indicator-output.ts`
- `/home/dev/zpano/ts/entities/{bar,scalar,quote,trade,bar-component}.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/` — 4 files (template reference)
- `/home/dev/zpano/ts/package.json` (test script: `npm test`)

### TS files created this session ✅
- `/home/dev/zpano/ts/indicators/john-ehlers/corona/corona-params.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/corona/corona.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/corona/corona-test-input.ts` — 252-entry shared test input
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum-output.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum-params.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum.spec.ts` (12/12 specs pass)

### TS files to create next
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-signal-to-noise-ratio/` (4 files)
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-swing-position/` (4 files)
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-trend-vigor/` (4 files)

Empty directories already exist (created via `mkdir -p`).

### Skill to update after TS complete
- `/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md`

### Immediate next action
Read `/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go` lines 100+ for Metadata/UpdateEntity/NewX tests to mirror, then create the 4 CSNR TS files modeled on the CSpectrum package. Run `npm test -- --filter=CoronaSignalToNoiseRatio` to verify before moving on.

### Todos
1. ✅ Append 4 Corona entries to indicator-type.ts
2. ✅ Add static factories to heatmap.ts
3. ✅ Create Corona helper (corona.ts, corona-params.ts)
4. ✅ Create CoronaSpectrum indicator package (12/12 specs pass)
5. ⏳ Create CoronaSignalToNoiseRatio package (← current)
6. ⏳ Create CoronaSwingPosition package
7. ⏳ Create CoronaTrendVigor package
8. ⏳ Run all TS tests and fix issues
9. ⏳ Update mbst-indicator-conversion skill with Corona learnings

---

## User

---

## Assistant (Build · claude-opus-4.7 · 6.8s)

_Thinking:_

 Moving on to the CoronaSignalToNoiseRatio package now—I'll examine the Go source files and use the CSpectrum TypeScript package as a reference template.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio.go</path>
<type>file</type>
<content>1: // Package coronasignaltonoiseratio implements Ehlers' Corona Signal-to-Noise
2: // Ratio heatmap indicator.
3: //
4: // The Corona Signal to Noise Ratio is a measure of the cycle amplitude relative
5: // to noise. The "noise" is chosen to be the average bar height because there is
6: // not much trade information within the bar.
7: //
8: // Reference: John Ehlers, "Measuring Cycle Periods", Stocks & Commodities,
9: // November 2008.
10: package coronasignaltonoiseratio
11: 
12: //nolint: gofumpt
13: import (
14: 	"fmt"
15: 	"math"
16: 	"sort"
17: 	"sync"
18: 	"time"
19: 
20: 	"zpano/entities"
21: 	"zpano/indicators/core"
22: 	"zpano/indicators/core/outputs"
23: 	"zpano/indicators/johnehlers/corona"
24: )
25: 
26: const (
27: 	highLowBufferSize       = 5
28: 	highLowBufferSizeMinOne = highLowBufferSize - 1
29: 	highLowMedianIndex      = 2
30: 	averageSampleAlpha      = 0.1
31: 	averageSampleOneMinus   = 0.9
32: 	signalEmaAlpha          = 0.2
33: 	signalEmaOneMinus       = 0.9 // Intentional: sums to 1.1, per Ehlers.
34: 	noiseEmaAlpha           = 0.1
35: 	noiseEmaOneMinus        = 0.9
36: 	ratioOffsetDb           = 3.5
37: 	ratioUpperDb            = 10.
38: 	dbGain                  = 20.
39: 	widthLowRatioThreshold  = 0.5
40: 	widthBaseline           = 0.2
41: 	widthSlope              = 0.4
42: 	rasterBlendExponent     = 0.8
43: 	rasterBlendHalf         = 0.5
44: 	rasterNegativeArgCutoff = 1.
45: )
46: 
47: // CoronaSignalToNoiseRatio is Ehlers' Corona Signal-to-Noise Ratio indicator.
48: //
49: // It owns a private Corona spectral-analysis engine and exposes two outputs:
50: //
51: //   - Value: a per-bar heatmap column (intensity raster).
52: //   - SignalToNoiseRatio: the current SNR value mapped into
53: //     [MinParameterValue, MaxParameterValue].
54: type CoronaSignalToNoiseRatio struct {
55: 	mu                  sync.RWMutex
56: 	mnemonic            string
57: 	description         string
58: 	mnemonicSNR         string
59: 	descriptionSNR      string
60: 	c                   *corona.Corona
61: 	rasterLength        int
62: 	rasterStep          float64
63: 	maxRasterValue      float64
64: 	minParameterValue   float64
65: 	maxParameterValue   float64
66: 	parameterResolution float64
67: 	raster              []float64
68: 	highLowBuffer       [highLowBufferSize]float64
69: 	hlSorted            [highLowBufferSize]float64
70: 	averageSamplePrev   float64
71: 	signalPrev          float64
72: 	noisePrev           float64
73: 	signalToNoiseRatio  float64
74: 	isStarted           bool
75: 	barFunc             entities.BarFunc
76: 	quoteFunc           entities.QuoteFunc
77: 	tradeFunc           entities.TradeFunc
78: }
79: 
80: // NewCoronaSignalToNoiseRatioDefault returns an instance created with default parameters.
81: func NewCoronaSignalToNoiseRatioDefault() (*CoronaSignalToNoiseRatio, error) {
82: 	return NewCoronaSignalToNoiseRatioParams(&Params{})
83: }
84: 
85: // NewCoronaSignalToNoiseRatioParams returns an instance created with the supplied parameters.
86: //
87: //nolint:funlen,cyclop
88: func NewCoronaSignalToNoiseRatioParams(p *Params) (*CoronaSignalToNoiseRatio, error) {
89: 	const (
90: 		invalid      = "invalid corona signal to noise ratio parameters"
91: 		fmtRaster    = "%s: RasterLength should be >= 2"
92: 		fmtMaxRaster = "%s: MaxRasterValue should be > 0"
93: 		fmtMinParam  = "%s: MinParameterValue should be >= 0"
94: 		fmtMaxParam  = "%s: MaxParameterValue should be > MinParameterValue"
95: 		fmtHP        = "%s: HighPassFilterCutoff should be >= 2"
96: 		fmtMinP      = "%s: MinimalPeriod should be >= 2"
97: 		fmtMaxP      = "%s: MaximalPeriod should be > MinimalPeriod"
98: 		fmtw         = "%s: %w"
99: 		fmtnVal      = "csnr(%d, %g, %g, %g, %d%s)"
100: 		fmtnSNR      = "csnr-snr(%d%s)"
101: 		descrVal     = "Corona signal to noise ratio "
102: 		descrSNR     = "Corona signal to noise ratio scalar "
103: 		defRaster    = 50
104: 		defMaxRast   = 20.0
105: 		defMinParam  = 1.0
106: 		defMaxParam  = 11.0
107: 		defHPCutoff  = 30
108: 		defMinPer    = 6
109: 		defMaxPer    = 30
110: 	)
111: 
112: 	cfg := *p
113: 
114: 	if cfg.RasterLength == 0 {
115: 		cfg.RasterLength = defRaster
116: 	}
117: 
118: 	if cfg.MaxRasterValue == 0 {
119: 		cfg.MaxRasterValue = defMaxRast
120: 	}
121: 
122: 	if cfg.MinParameterValue == 0 {
123: 		cfg.MinParameterValue = defMinParam
124: 	}
125: 
126: 	if cfg.MaxParameterValue == 0 {
127: 		cfg.MaxParameterValue = defMaxParam
128: 	}
129: 
130: 	if cfg.HighPassFilterCutoff == 0 {
131: 		cfg.HighPassFilterCutoff = defHPCutoff
132: 	}
133: 
134: 	if cfg.MinimalPeriod == 0 {
135: 		cfg.MinimalPeriod = defMinPer
136: 	}
137: 
138: 	if cfg.MaximalPeriod == 0 {
139: 		cfg.MaximalPeriod = defMaxPer
140: 	}
141: 
142: 	if cfg.RasterLength < 2 {
143: 		return nil, fmt.Errorf(fmtRaster, invalid)
144: 	}
145: 
146: 	if cfg.MaxRasterValue <= 0 {
147: 		return nil, fmt.Errorf(fmtMaxRaster, invalid)
148: 	}
149: 
150: 	if cfg.MinParameterValue < 0 {
151: 		return nil, fmt.Errorf(fmtMinParam, invalid)
152: 	}
153: 
154: 	if cfg.MaxParameterValue <= cfg.MinParameterValue {
155: 		return nil, fmt.Errorf(fmtMaxParam, invalid)
156: 	}
157: 
158: 	if cfg.HighPassFilterCutoff < 2 {
159: 		return nil, fmt.Errorf(fmtHP, invalid)
160: 	}
161: 
162: 	if cfg.MinimalPeriod < 2 {
163: 		return nil, fmt.Errorf(fmtMinP, invalid)
164: 	}
165: 
166: 	if cfg.MaximalPeriod <= cfg.MinimalPeriod {
167: 		return nil, fmt.Errorf(fmtMaxP, invalid)
168: 	}
169: 
170: 	// Ehlers reference uses (High+Low)/2.
171: 	bc := cfg.BarComponent
172: 	if bc == 0 {
173: 		bc = entities.BarMedianPrice
174: 	}
175: 
176: 	qc := cfg.QuoteComponent
177: 	if qc == 0 {
178: 		qc = entities.DefaultQuoteComponent
179: 	}
180: 
181: 	tc := cfg.TradeComponent
182: 	if tc == 0 {
183: 		tc = entities.DefaultTradeComponent
184: 	}
185: 
186: 	c, err := corona.NewCorona(&corona.Params{
187: 		HighPassFilterCutoff: cfg.HighPassFilterCutoff,
188: 		MinimalPeriod:        cfg.MinimalPeriod,
189: 		MaximalPeriod:        cfg.MaximalPeriod,
190: 	})
191: 	if err != nil {
192: 		return nil, fmt.Errorf(fmtw, invalid, err)
193: 	}
194: 
195: 	barFunc, err := entities.BarComponentFunc(bc)
196: 	if err != nil {
197: 		return nil, fmt.Errorf(fmtw, invalid, err)
198: 	}
199: 
200: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
201: 	if err != nil {
202: 		return nil, fmt.Errorf(fmtw, invalid, err)
203: 	}
204: 
205: 	tradeFunc, err := entities.TradeComponentFunc(tc)
206: 	if err != nil {
207: 		return nil, fmt.Errorf(fmtw, invalid, err)
208: 	}
209: 
210: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
211: 
212: 	mnemonicVal := fmt.Sprintf(fmtnVal,
213: 		cfg.RasterLength, cfg.MaxRasterValue, cfg.MinParameterValue, cfg.MaxParameterValue,
214: 		cfg.HighPassFilterCutoff, componentMnemonic)
215: 	mnemonicSNR := fmt.Sprintf(fmtnSNR, cfg.HighPassFilterCutoff, componentMnemonic)
216: 
217: 	// Resolution satisfies: min + (rasterLength-1)/resolution = max.
218: 	paramResolution := float64(cfg.RasterLength-1) / (cfg.MaxParameterValue - cfg.MinParameterValue)
219: 
220: 	return &CoronaSignalToNoiseRatio{
221: 		mnemonic:            mnemonicVal,
222: 		description:         descrVal + mnemonicVal,
223: 		mnemonicSNR:         mnemonicSNR,
224: 		descriptionSNR:      descrSNR + mnemonicSNR,
225: 		c:                   c,
226: 		rasterLength:        cfg.RasterLength,
227: 		rasterStep:          cfg.MaxRasterValue / float64(cfg.RasterLength),
228: 		maxRasterValue:      cfg.MaxRasterValue,
229: 		minParameterValue:   cfg.MinParameterValue,
230: 		maxParameterValue:   cfg.MaxParameterValue,
231: 		parameterResolution: paramResolution,
232: 		raster:              make([]float64, cfg.RasterLength),
233: 		signalToNoiseRatio:  math.NaN(),
234: 		barFunc:             barFunc,
235: 		quoteFunc:           quoteFunc,
236: 		tradeFunc:           tradeFunc,
237: 	}, nil
238: }
239: 
240: // IsPrimed indicates whether the indicator is primed.
241: func (s *CoronaSignalToNoiseRatio) IsPrimed() bool {
242: 	s.mu.RLock()
243: 	defer s.mu.RUnlock()
244: 
245: 	return s.c.IsPrimed()
246: }
247: 
248: // Metadata describes the output data of the indicator.
249: func (s *CoronaSignalToNoiseRatio) Metadata() core.Metadata {
250: 	return core.Metadata{
251: 		Type:        core.CoronaSignalToNoiseRatio,
252: 		Mnemonic:    s.mnemonic,
253: 		Description: s.description,
254: 		Outputs: []outputs.Metadata{
255: 			{Kind: int(Value), Type: outputs.HeatmapType, Mnemonic: s.mnemonic, Description: s.description},
256: 			{Kind: int(SignalToNoiseRatio), Type: outputs.ScalarType, Mnemonic: s.mnemonicSNR, Description: s.descriptionSNR},
257: 		},
258: 	}
259: }
260: 
261: // Update feeds the next sample plus bar extremes and returns the heatmap column
262: // and the current SignalToNoiseRatio. On unprimed bars the heatmap is empty and
263: // the scalar is NaN. On NaN sample input state is left unchanged.
264: //
265: //nolint:funlen,cyclop,gocognit
266: func (s *CoronaSignalToNoiseRatio) Update(sample, sampleLow, sampleHigh float64, t time.Time) (*outputs.Heatmap, float64) {
267: 	s.mu.Lock()
268: 	defer s.mu.Unlock()
269: 
270: 	if math.IsNaN(sample) {
271: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
272: 	}
273: 
274: 	primed := s.c.Update(sample)
275: 
276: 	if !s.isStarted {
277: 		s.averageSamplePrev = sample
278: 		s.highLowBuffer[highLowBufferSizeMinOne] = sampleHigh - sampleLow
279: 		s.isStarted = true
280: 
281: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
282: 	}
283: 
284: 	maxAmpSq := s.c.MaximalAmplitudeSquared()
285: 
286: 	averageSample := averageSampleAlpha*sample + averageSampleOneMinus*s.averageSamplePrev
287: 	s.averageSamplePrev = averageSample
288: 
289: 	if math.Abs(averageSample) > 0 || maxAmpSq > 0 {
290: 		s.signalPrev = signalEmaAlpha*math.Sqrt(maxAmpSq) + signalEmaOneMinus*s.signalPrev
291: 	}
292: 
293: 	// Shift H-L ring buffer left; push new value.
294: 	for i := 0; i < highLowBufferSizeMinOne; i++ {
295: 		s.highLowBuffer[i] = s.highLowBuffer[i+1]
296: 	}
297: 	s.highLowBuffer[highLowBufferSizeMinOne] = sampleHigh - sampleLow
298: 
299: 	ratio := 0.0
300: 	if math.Abs(averageSample) > 0 {
301: 		for i := 0; i < highLowBufferSize; i++ {
302: 			s.hlSorted[i] = s.highLowBuffer[i]
303: 		}
304: 
305: 		sort.Float64s(s.hlSorted[:])
306: 		s.noisePrev = noiseEmaAlpha*s.hlSorted[highLowMedianIndex] + noiseEmaOneMinus*s.noisePrev
307: 
308: 		if math.Abs(s.noisePrev) > 0 {
309: 			ratio = dbGain*math.Log10(s.signalPrev/s.noisePrev) + ratioOffsetDb
310: 			if ratio < 0 {
311: 				ratio = 0
312: 			} else if ratio > ratioUpperDb {
313: 				ratio = ratioUpperDb
314: 			}
315: 
316: 			ratio /= ratioUpperDb // ∈ [0, 1]
317: 		}
318: 	}
319: 
320: 	s.signalToNoiseRatio = (s.maxParameterValue-s.minParameterValue)*ratio + s.minParameterValue
321: 
322: 	// Raster update.
323: 	width := 0.0
324: 	if ratio <= widthLowRatioThreshold {
325: 		width = widthBaseline - widthSlope*ratio
326: 	}
327: 
328: 	ratioScaledToRasterLength := int(math.Round(ratio * float64(s.rasterLength)))
329: 	ratioScaledToMaxRasterValue := ratio * s.maxRasterValue
330: 
331: 	for i := 0; i < s.rasterLength; i++ {
332: 		value := s.raster[i]
333: 
334: 		switch {
335: 		case i == ratioScaledToRasterLength:
336: 			value *= 0.5
337: 		case width == 0:
338: 			// Above the high-ratio threshold: handled by the ratio>0.5 override below.
339: 		default:
340: 			argument := (ratioScaledToMaxRasterValue - s.rasterStep*float64(i)) / width
341: 			if i < ratioScaledToRasterLength {
342: 				value = rasterBlendHalf * (math.Pow(argument, rasterBlendExponent) + value)
343: 			} else {
344: 				argument = -argument
345: 				if argument > rasterNegativeArgCutoff {
346: 					value = rasterBlendHalf * (math.Pow(argument, rasterBlendExponent) + value)
347: 				} else {
348: 					value = s.maxRasterValue
349: 				}
350: 			}
351: 		}
352: 
353: 		if value < 0 {
354: 			value = 0
355: 		} else if value > s.maxRasterValue {
356: 			value = s.maxRasterValue
357: 		}
358: 
359: 		if ratio > widthLowRatioThreshold {
360: 			value = s.maxRasterValue
361: 		}
362: 
363: 		s.raster[i] = value
364: 	}
365: 
366: 	if !primed {
367: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
368: 	}
369: 
370: 	values := make([]float64, s.rasterLength)
371: 	valueMin := math.Inf(1)
372: 	valueMax := math.Inf(-1)
373: 
374: 	for i := 0; i < s.rasterLength; i++ {
375: 		v := s.raster[i]
376: 		values[i] = v
377: 
378: 		if v < valueMin {
379: 			valueMin = v
380: 		}
381: 
382: 		if v > valueMax {
383: 			valueMax = v
384: 		}
385: 	}
386: 
387: 	heatmap := outputs.NewHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution,
388: 		valueMin, valueMax, values)
389: 
390: 	return heatmap, s.signalToNoiseRatio
391: }
392: 
393: // UpdateScalar updates the indicator given the next scalar sample. Since no
394: // High/Low is available, the sample is used for both, yielding zero noise.
395: func (s *CoronaSignalToNoiseRatio) UpdateScalar(sample *entities.Scalar) core.Output {
396: 	return s.updateEntity(sample.Time, sample.Value, sample.Value, sample.Value)
397: }
398: 
399: // UpdateBar updates the indicator given the next bar sample.
400: func (s *CoronaSignalToNoiseRatio) UpdateBar(sample *entities.Bar) core.Output {
401: 	return s.updateEntity(sample.Time, s.barFunc(sample), sample.Low, sample.High)
402: }
403: 
404: // UpdateQuote updates the indicator given the next quote sample.
405: func (s *CoronaSignalToNoiseRatio) UpdateQuote(sample *entities.Quote) core.Output {
406: 	v := s.quoteFunc(sample)
407: 
408: 	return s.updateEntity(sample.Time, v, v, v)
409: }
410: 
411: // UpdateTrade updates the indicator given the next trade sample.
412: func (s *CoronaSignalToNoiseRatio) UpdateTrade(sample *entities.Trade) core.Output {
413: 	v := s.tradeFunc(sample)
414: 
415: 	return s.updateEntity(sample.Time, v, v, v)
416: }
417: 
418: func (s *CoronaSignalToNoiseRatio) updateEntity(t time.Time, sample, low, high float64) core.Output {
419: 	const length = 2
420: 
421: 	heatmap, snr := s.Update(sample, low, high, t)
422: 
423: 	output := make([]any, length)
424: 	output[0] = heatmap
425: 	output[1] = entities.Scalar{Time: t, Value: snr}
426: 
427: 	return output
428: }

(End of file - total 428 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go</path>
<type>file</type>
<content>1: //nolint:testpackage
2: package coronasignaltonoiseratio
3: 
4: import (
5: 	"math"
6: 	"testing"
7: 	"time"
8: 
9: 	"zpano/entities"
10: 	"zpano/indicators/core"
11: 	"zpano/indicators/core/outputs"
12: )
13: 
14: func testCSNRTime() time.Time {
15: 	return time.Date(2021, time.April, 1, 0, 0, 0, 0, &time.Location{})
16: }
17: 
18: // testCSNRInput is the 252-entry TA-Lib MAMA reference series.
19: //
20: //nolint:dupl
21: func testCSNRInput() []float64 {
22: 	return []float64{
23: 		92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,
24: 		94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,
25: 		88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,
26: 		85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,
27: 		83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,
28: 		89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,
29: 		89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,
30: 		88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,
31: 		103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,
32: 		120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,
33: 		114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,
34: 		114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,
35: 		124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,
36: 		137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,
37: 		123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,
38: 		122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,
39: 		123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,
40: 		130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,
41: 		127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,
42: 		121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,
43: 		106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,
44: 		95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,
45: 		94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,
46: 		103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,
47: 		106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,
48: 		109.5300, 108.0600,
49: 	}
50: }
51: 
52: const testCSNRTolerance = 1e-4
53: 
54: // Produce synthetic High/Low around the sample. The spread is varied to
55: // exercise the full range of the SNR logic (narrow spreads → high SNR,
56: // wide spreads → low SNR).
57: func makeHL(i int, sample float64) (float64, float64) {
58: 	// Fraction cycles between ~0.5% and ~6% of sample.
59: 	frac := 0.005 + 0.03*(1+math.Sin(float64(i)*0.37))
60: 	half := sample * frac
61: 
62: 	return sample - half, sample + half
63: }
64: 
65: func TestCoronaSignalToNoiseRatioUpdate(t *testing.T) {
66: 	t.Parallel()
67: 
68: 	input := testCSNRInput()
69: 	t0 := testCSNRTime()
70: 
71: 	// Snapshot values captured from a first run and locked in here.
72: 	type snap struct {
73: 		i   int
74: 		snr float64
75: 		vmn float64
76: 		vmx float64
77: 	}
78: 	snapshots := []snap{
79: 		{11, 1.0000000000, 0.0000000000, 20.0000000000},
80: 		{12, 1.0000000000, 0.0000000000, 20.0000000000},
81: 		{50, 1.0000000000, 0.0000000000, 20.0000000000},
82: 		{100, 2.9986583538, 4.2011609652, 20.0000000000},
83: 		{150, 1.0000000000, 0.0000000035, 20.0000000000},
84: 		{200, 1.0000000000, 0.0000000000, 20.0000000000},
85: 		{251, 1.0000000000, 0.0000000026, 20.0000000000},
86: 	}
87: 
88: 	x, err := NewCoronaSignalToNoiseRatioDefault()
89: 	if err != nil {
90: 		t.Fatalf("unexpected error: %v", err)
91: 	}
92: 
93: 	si := 0
94: 	for i := range input {
95: 		low, high := makeHL(i, input[i])
96: 		h, snr := x.Update(input[i], low, high, t0.Add(time.Duration(i)*time.Minute))
97: 
98: 		if h == nil {
99: 			t.Fatalf("[%d] heatmap must not be nil", i)
100: 		}
101: 
102: 		if h.ParameterFirst != 1 || h.ParameterLast != 11 || math.Abs(h.ParameterResolution-4.9) > 1e-9 {
103: 			t.Errorf("[%d] heatmap axis incorrect: first=%v last=%v res=%v",
104: 				i, h.ParameterFirst, h.ParameterLast, h.ParameterResolution)
105: 		}
106: 
107: 		if !x.IsPrimed() {
108: 			if !h.IsEmpty() {
109: 				t.Errorf("[%d] expected empty heatmap before priming, got len=%d", i, len(h.Values))
110: 			}
111: 
112: 			if !math.IsNaN(snr) {
113: 				t.Errorf("[%d] expected NaN snr before priming, got %v", i, snr)
114: 			}
115: 
116: 			continue
117: 		}
118: 
119: 		if len(h.Values) != 50 {
120: 			t.Errorf("[%d] heatmap values length: expected 50, got %d", i, len(h.Values))
121: 		}
122: 
123: 		if si < len(snapshots) && snapshots[si].i == i {
124: 			if math.Abs(snapshots[si].snr-snr) > testCSNRTolerance {
125: 				t.Errorf("[%d] snr: expected %v, got %v", i, snapshots[si].snr, snr)
126: 			}
127: 
128: 			if math.Abs(snapshots[si].vmn-h.ValueMin) > testCSNRTolerance {
129: 				t.Errorf("[%d] vmin: expected %v, got %v", i, snapshots[si].vmn, h.ValueMin)
130: 			}
131: 
132: 			if math.Abs(snapshots[si].vmx-h.ValueMax) > testCSNRTolerance {
133: 				t.Errorf("[%d] vmax: expected %v, got %v", i, snapshots[si].vmx, h.ValueMax)
134: 			}
135: 
136: 			si++
137: 		}
138: 	}
139: 
140: 	if si != len(snapshots) {
141: 		t.Errorf("did not hit all %d snapshots, reached %d", len(snapshots), si)
142: 	}
143: }
144: 
145: func TestCoronaSignalToNoiseRatioPrimesAtBar11(t *testing.T) {
146: 	t.Parallel()
147: 
148: 	x, _ := NewCoronaSignalToNoiseRatioDefault()
149: 
150: 	if x.IsPrimed() {
151: 		t.Error("expected not primed at start")
152: 	}
153: 
154: 	input := testCSNRInput()
155: 	t0 := testCSNRTime()
156: 	primedAt := -1
157: 
158: 	for i := range input {
159: 		low, high := makeHL(i, input[i])
160: 		x.Update(input[i], low, high, t0.Add(time.Duration(i)*time.Minute))
161: 
162: 		if x.IsPrimed() && primedAt < 0 {
163: 			primedAt = i
164: 		}
165: 	}
166: 
167: 	if primedAt != 11 {
168: 		t.Errorf("expected priming at index 11, got %d", primedAt)
169: 	}
170: }
171: 
172: func TestCoronaSignalToNoiseRatioNaNInput(t *testing.T) {
173: 	t.Parallel()
174: 
175: 	x, _ := NewCoronaSignalToNoiseRatioDefault()
176: 
177: 	h, snr := x.Update(math.NaN(), math.NaN(), math.NaN(), testCSNRTime())
178: 
179: 	if h == nil || !h.IsEmpty() {
180: 		t.Errorf("expected empty heatmap for NaN input, got %v", h)
181: 	}
182: 
183: 	if !math.IsNaN(snr) {
184: 		t.Errorf("expected NaN snr for NaN input, got %v", snr)
185: 	}
186: 
187: 	if x.IsPrimed() {
188: 		t.Error("NaN input must not prime the indicator")
189: 	}
190: }
191: 
192: func TestCoronaSignalToNoiseRatioMetadata(t *testing.T) {
193: 	t.Parallel()
194: 
195: 	x, _ := NewCoronaSignalToNoiseRatioDefault()
196: 	md := x.Metadata()
197: 
198: 	check := func(what string, exp, act any) {
199: 		t.Helper()
200: 
201: 		if exp != act {
202: 			t.Errorf("%s: expected %v, actual %v", what, exp, act)
203: 		}
204: 	}
205: 
206: 	mnVal := "csnr(50, 20, 1, 11, 30, hl/2)"
207: 	mnSNR := "csnr-snr(30, hl/2)"
208: 
209: 	check("Type", core.CoronaSignalToNoiseRatio, md.Type)
210: 	check("Mnemonic", mnVal, md.Mnemonic)
211: 	check("Description", "Corona signal to noise ratio "+mnVal, md.Description)
212: 	check("len(Outputs)", 2, len(md.Outputs))
213: 
214: 	check("Outputs[0].Kind", int(Value), md.Outputs[0].Kind)
215: 	check("Outputs[0].Type", outputs.HeatmapType, md.Outputs[0].Type)
216: 	check("Outputs[0].Mnemonic", mnVal, md.Outputs[0].Mnemonic)
217: 
218: 	check("Outputs[1].Kind", int(SignalToNoiseRatio), md.Outputs[1].Kind)
219: 	check("Outputs[1].Type", outputs.ScalarType, md.Outputs[1].Type)
220: 	check("Outputs[1].Mnemonic", mnSNR, md.Outputs[1].Mnemonic)
221: }
222: 
223: //nolint:funlen
224: func TestCoronaSignalToNoiseRatioUpdateEntity(t *testing.T) {
225: 	t.Parallel()
226: 
227: 	const (
228: 		primeCount = 50
229: 		inp        = 100.
230: 		outputLen  = 2
231: 	)
232: 
233: 	tm := testCSNRTime()
234: 	input := testCSNRInput()
235: 
236: 	check := func(act core.Output) {
237: 		t.Helper()
238: 
239: 		if len(act) != outputLen {
240: 			t.Errorf("len(output): expected %v, actual %v", outputLen, len(act))
241: 
242: 			return
243: 		}
244: 
245: 		h, ok := act[0].(*outputs.Heatmap)
246: 		if !ok {
247: 			t.Errorf("output[0] is not a heatmap: %T", act[0])
248: 		} else if h.Time != tm {
249: 			t.Errorf("output[0].Time: expected %v, actual %v", tm, h.Time)
250: 		}
251: 
252: 		s, ok := act[1].(entities.Scalar)
253: 		if !ok {
254: 			t.Errorf("output[1] is not a scalar")
255: 		} else if s.Time != tm {
256: 			t.Errorf("output[1].Time: expected %v, actual %v", tm, s.Time)
257: 		}
258: 	}
259: 
260: 	prime := func(x *CoronaSignalToNoiseRatio) {
261: 		for i := 0; i < primeCount; i++ {
262: 			low, high := makeHL(i, input[i%len(input)])
263: 			x.Update(input[i%len(input)], low, high, tm)
264: 		}
265: 	}
266: 
267: 	t.Run("update scalar", func(t *testing.T) {
268: 		t.Parallel()
269: 
270: 		s := entities.Scalar{Time: tm, Value: inp}
271: 		x, _ := NewCoronaSignalToNoiseRatioDefault()
272: 		prime(x)
273: 		check(x.UpdateScalar(&s))
274: 	})
275: 
276: 	t.Run("update bar", func(t *testing.T) {
277: 		t.Parallel()
278: 
279: 		b := entities.Bar{Time: tm, High: inp * 1.005, Low: inp * 0.995, Close: inp}
280: 		x, _ := NewCoronaSignalToNoiseRatioDefault()
281: 		prime(x)
282: 		check(x.UpdateBar(&b))
283: 	})
284: 
285: 	t.Run("update quote", func(t *testing.T) {
286: 		t.Parallel()
287: 
288: 		q := entities.Quote{Time: tm, Bid: inp, Ask: inp}
289: 		x, _ := NewCoronaSignalToNoiseRatioDefault()
290: 		prime(x)
291: 		check(x.UpdateQuote(&q))
292: 	})
293: 
294: 	t.Run("update trade", func(t *testing.T) {
295: 		t.Parallel()
296: 
297: 		r := entities.Trade{Time: tm, Price: inp}
298: 		x, _ := NewCoronaSignalToNoiseRatioDefault()
299: 		prime(x)
300: 		check(x.UpdateTrade(&r))
301: 	})
302: }
303: 
304: //nolint:funlen
305: func TestNewCoronaSignalToNoiseRatio(t *testing.T) {
306: 	t.Parallel()
307: 
308: 	check := func(name string, exp, act any) {
309: 		t.Helper()
310: 
311: 		if exp != act {
312: 			t.Errorf("%s: expected %v, actual %v", name, exp, act)
313: 		}
314: 	}
315: 
316: 	t.Run("default", func(t *testing.T) {
317: 		t.Parallel()
318: 
319: 		x, err := NewCoronaSignalToNoiseRatioDefault()
320: 		check("err == nil", true, err == nil)
321: 		check("mnemonic", "csnr(50, 20, 1, 11, 30, hl/2)", x.mnemonic)
322: 		check("MinParameterValue", 1.0, x.minParameterValue)
323: 		check("MaxParameterValue", 11.0, x.maxParameterValue)
324: 		check("RasterLength", 50, x.rasterLength)
325: 	})
326: 
327: 	t.Run("RasterLength < 2", func(t *testing.T) {
328: 		t.Parallel()
329: 
330: 		_, err := NewCoronaSignalToNoiseRatioParams(&Params{RasterLength: 1})
331: 		if err == nil || err.Error() !=
332: 			"invalid corona signal to noise ratio parameters: RasterLength should be >= 2" {
333: 			t.Errorf("unexpected: %v", err)
334: 		}
335: 	})
336: 
337: 	t.Run("MaxParameterValue <= MinParameterValue", func(t *testing.T) {
338: 		t.Parallel()
339: 
340: 		_, err := NewCoronaSignalToNoiseRatioParams(&Params{
341: 			MinParameterValue: 5,
342: 			MaxParameterValue: 5,
343: 		})
344: 		if err == nil || err.Error() !=
345: 			"invalid corona signal to noise ratio parameters: MaxParameterValue should be > MinParameterValue" {
346: 			t.Errorf("unexpected: %v", err)
347: 		}
348: 	})
349: 
350: 	t.Run("HighPassFilterCutoff < 2", func(t *testing.T) {
351: 		t.Parallel()
352: 
353: 		_, err := NewCoronaSignalToNoiseRatioParams(&Params{HighPassFilterCutoff: 1})
354: 		if err == nil || err.Error() !=
355: 			"invalid corona signal to noise ratio parameters: HighPassFilterCutoff should be >= 2" {
356: 			t.Errorf("unexpected: %v", err)
357: 		}
358: 	})
359: 
360: 	t.Run("MinimalPeriod < 2", func(t *testing.T) {
361: 		t.Parallel()
362: 
363: 		_, err := NewCoronaSignalToNoiseRatioParams(&Params{MinimalPeriod: 1})
364: 		if err == nil || err.Error() !=
365: 			"invalid corona signal to noise ratio parameters: MinimalPeriod should be >= 2" {
366: 			t.Errorf("unexpected: %v", err)
367: 		}
368: 	})
369: 
370: 	t.Run("MaximalPeriod <= MinimalPeriod", func(t *testing.T) {
371: 		t.Parallel()
372: 
373: 		_, err := NewCoronaSignalToNoiseRatioParams(&Params{
374: 			MinimalPeriod: 10,
375: 			MaximalPeriod: 10,
376: 		})
377: 		if err == nil || err.Error() !=
378: 			"invalid corona signal to noise ratio parameters: MaximalPeriod should be > MinimalPeriod" {
379: 			t.Errorf("unexpected: %v", err)
380: 		}
381: 	})
382: 
383: 	t.Run("invalid bar component", func(t *testing.T) {
384: 		t.Parallel()
385: 
386: 		_, err := NewCoronaSignalToNoiseRatioParams(&Params{
387: 			BarComponent: entities.BarComponent(9999),
388: 		})
389: 		if err == nil {
390: 			t.Error("expected error")
391: 		}
392: 	})
393: }

(End of file - total 393 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum.ts</path>
<type>file</type>
<content>1: import { Bar } from '../../../entities/bar';
2: import { BarComponent, barComponentValue } from '../../../entities/bar-component';
3: import { Quote } from '../../../entities/quote';
4: import { DefaultQuoteComponent, quoteComponentValue } from '../../../entities/quote-component';
5: import { Scalar } from '../../../entities/scalar';
6: import { Trade } from '../../../entities/trade';
7: import { DefaultTradeComponent, tradeComponentValue } from '../../../entities/trade-component';
8: import { componentTripleMnemonic } from '../../core/component-triple-mnemonic';
9: import { Indicator } from '../../core/indicator';
10: import { IndicatorMetadata } from '../../core/indicator-metadata';
11: import { IndicatorOutput } from '../../core/indicator-output';
12: import { IndicatorType } from '../../core/indicator-type';
13: import { Heatmap } from '../../core/outputs/heatmap';
14: import { OutputType } from '../../core/outputs/output-type';
15: import { Corona } from '../corona/corona';
16: import { CoronaSpectrumOutput } from './corona-spectrum-output';
17: import { CoronaSpectrumParams } from './corona-spectrum-params';
18: 
19: const DEFAULT_MIN_RASTER = 6;
20: const DEFAULT_MAX_RASTER = 20;
21: const DEFAULT_MIN_PARAM = 6;
22: const DEFAULT_MAX_PARAM = 30;
23: const DEFAULT_HP_CUTOFF = 30;
24: 
25: /** __Corona Spectrum__ (Ehlers) heatmap indicator.
26:  *
27:  * The Corona Spectrum measures cyclic activity over a cycle period range (default 6..30 bars)
28:  * in a bank of contiguous bandpass filters. The amplitude of each filter output is compared to
29:  * the strongest signal and displayed, in decibels, as a heatmap column. The filter having the
30:  * strongest output is selected as the current dominant cycle period.
31:  *
32:  * It exposes three outputs:
33:  *
34:  *	- Value: a per-bar heatmap column (decibels across the filter bank).
35:  *	- DominantCycle: the weighted-center-of-gravity dominant cycle estimate.
36:  *	- DominantCycleMedian: the 5-sample median of DominantCycle.
37:  *
38:  * Reference:
39:  *
40:  *	John Ehlers, "Measuring Cycle Periods", Stocks & Commodities, November 2008.
41:  */
42: export class CoronaSpectrum implements Indicator {
43:   private readonly c: Corona;
44:   private readonly minParameterValue: number;
45:   private readonly maxParameterValue: number;
46:   private readonly parameterResolution: number;
47: 
48:   private readonly mnemonicValue: string;
49:   private readonly descriptionValue: string;
50:   private readonly mnemonicDC: string;
51:   private readonly descriptionDC: string;
52:   private readonly mnemonicDCM: string;
53:   private readonly descriptionDCM: string;
54: 
55:   private readonly barComponentFunc: (bar: Bar) => number;
56:   private readonly quoteComponentFunc: (quote: Quote) => number;
57:   private readonly tradeComponentFunc: (trade: Trade) => number;
58: 
59:   /** Creates an instance with default parameters
60:    * (minRaster=6, maxRaster=20, minParam=6, maxParam=30, hpCutoff=30, BarComponent.Median). */
61:   public static default(): CoronaSpectrum {
62:     return new CoronaSpectrum({});
63:   }
64: 
65:   /** Creates an instance based on the given parameters. */
66:   public static fromParams(params: CoronaSpectrumParams): CoronaSpectrum {
67:     return new CoronaSpectrum(params);
68:   }
69: 
70:   private constructor(params: CoronaSpectrumParams) {
71:     const invalid = 'invalid corona spectrum parameters';
72: 
73:     const minRaster = params.minRasterValue !== undefined && params.minRasterValue !== 0
74:       ? params.minRasterValue : DEFAULT_MIN_RASTER;
75:     const maxRaster = params.maxRasterValue !== undefined && params.maxRasterValue !== 0
76:       ? params.maxRasterValue : DEFAULT_MAX_RASTER;
77:     const minParamRaw = params.minParameterValue !== undefined && params.minParameterValue !== 0
78:       ? params.minParameterValue : DEFAULT_MIN_PARAM;
79:     const maxParamRaw = params.maxParameterValue !== undefined && params.maxParameterValue !== 0
80:       ? params.maxParameterValue : DEFAULT_MAX_PARAM;
81:     const hpCutoff = params.highPassFilterCutoff !== undefined && params.highPassFilterCutoff !== 0
82:       ? params.highPassFilterCutoff : DEFAULT_HP_CUTOFF;
83: 
84:     if (minRaster < 0) {
85:       throw new Error(`${invalid}: MinRasterValue should be >= 0`);
86:     }
87:     if (maxRaster <= minRaster) {
88:       throw new Error(`${invalid}: MaxRasterValue should be > MinRasterValue`);
89:     }
90: 
91:     // MBST rounds min up and max down to integers.
92:     const minParam = Math.ceil(minParamRaw);
93:     const maxParam = Math.floor(maxParamRaw);
94: 
95:     if (minParam < 2) {
96:       throw new Error(`${invalid}: MinParameterValue should be >= 2`);
97:     }
98:     if (maxParam <= minParam) {
99:       throw new Error(`${invalid}: MaxParameterValue should be > MinParameterValue`);
100:     }
101:     if (hpCutoff < 2) {
102:       throw new Error(`${invalid}: HighPassFilterCutoff should be >= 2`);
103:     }
104: 
105:     // CoronaSpectrum mirrors Ehlers' reference: BarComponent.Median default.
106:     const bc = params.barComponent ?? BarComponent.Median;
107:     const qc = params.quoteComponent ?? DefaultQuoteComponent;
108:     const tc = params.tradeComponent ?? DefaultTradeComponent;
109: 
110:     this.barComponentFunc = barComponentValue(bc);
111:     this.quoteComponentFunc = quoteComponentValue(qc);
112:     this.tradeComponentFunc = tradeComponentValue(tc);
113: 
114:     this.c = new Corona({
115:       highPassFilterCutoff: hpCutoff,
116:       minimalPeriod: minParam,
117:       maximalPeriod: maxParam,
118:       decibelsLowerThreshold: minRaster,
119:       decibelsUpperThreshold: maxRaster,
120:     });
121: 
122:     this.minParameterValue = minParam;
123:     this.maxParameterValue = maxParam;
124: 
125:     // Values slice length = filterBankLength; first sample at minParam, last at maxParam.
126:     this.parameterResolution = (this.c.filterBankLength - 1) / (maxParam - minParam);
127: 
128:     const cm = componentTripleMnemonic(bc, qc, tc);
129:     this.mnemonicValue = `cspect(${minRaster}, ${maxRaster}, ${minParam}, ${maxParam}, ${hpCutoff}${cm})`;
130:     this.mnemonicDC = `cspect-dc(${hpCutoff}${cm})`;
131:     this.mnemonicDCM = `cspect-dcm(${hpCutoff}${cm})`;
132: 
133:     this.descriptionValue = 'Corona spectrum ' + this.mnemonicValue;
134:     this.descriptionDC = 'Corona spectrum dominant cycle ' + this.mnemonicDC;
135:     this.descriptionDCM = 'Corona spectrum dominant cycle median ' + this.mnemonicDCM;
136:   }
137: 
138:   /** Indicates whether the indicator is primed. */
139:   public isPrimed(): boolean { return this.c.isPrimed(); }
140: 
141:   /** Describes the output data of the indicator. */
142:   public metadata(): IndicatorMetadata {
143:     return {
144:       type: IndicatorType.CoronaSpectrum,
145:       mnemonic: this.mnemonicValue,
146:       description: this.descriptionValue,
147:       outputs: [
148:         {
149:           kind: CoronaSpectrumOutput.Value,
150:           type: OutputType.Heatmap,
151:           mnemonic: this.mnemonicValue,
152:           description: this.descriptionValue,
153:         },
154:         {
155:           kind: CoronaSpectrumOutput.DominantCycle,
156:           type: OutputType.Scalar,
157:           mnemonic: this.mnemonicDC,
158:           description: this.descriptionDC,
159:         },
160:         {
161:           kind: CoronaSpectrumOutput.DominantCycleMedian,
162:           type: OutputType.Scalar,
163:           mnemonic: this.mnemonicDCM,
164:           description: this.descriptionDCM,
165:         },
166:       ],
167:     };
168:   }
169: 
170:   /** Feeds the next sample to the engine and returns the heatmap column plus the current
171:    * DominantCycle and DominantCycleMedian estimates.
172:    *
173:    * On unprimed bars the heatmap is an empty heatmap (with the indicator's parameter axis)
174:    * and both scalar values are NaN. On NaN input, state is left unchanged and all outputs
175:    * are NaN / empty heatmap. */
176:   public update(sample: number, time: Date): [Heatmap, number, number] {
177:     if (Number.isNaN(sample)) {
178:       return [
179:         Heatmap.newEmptyHeatmap(time, this.minParameterValue, this.maxParameterValue, this.parameterResolution),
180:         Number.NaN, Number.NaN,
181:       ];
182:     }
183: 
184:     const primed = this.c.update(sample);
185:     if (!primed) {
186:       return [
187:         Heatmap.newEmptyHeatmap(time, this.minParameterValue, this.maxParameterValue, this.parameterResolution),
188:         Number.NaN, Number.NaN,
189:       ];
190:     }
191: 
192:     const bank = this.c.filterBank;
193:     const values = new Array<number>(bank.length);
194:     let valueMin = Number.POSITIVE_INFINITY;
195:     let valueMax = Number.NEGATIVE_INFINITY;
196: 
197:     for (let i = 0; i < bank.length; i++) {
198:       const v = bank[i].decibels;
199:       values[i] = v;
200:       if (v < valueMin) valueMin = v;
201:       if (v > valueMax) valueMax = v;
202:     }
203: 
204:     const heatmap = Heatmap.newHeatmap(
205:       time, this.minParameterValue, this.maxParameterValue, this.parameterResolution,
206:       valueMin, valueMax, values,
207:     );
208: 
209:     return [heatmap, this.c.dominantCycle, this.c.dominantCycleMedian];
210:   }
211: 
212:   /** Updates the indicator given the next scalar sample. */
213:   public updateScalar(sample: Scalar): IndicatorOutput {
214:     return this.updateEntity(sample.time, sample.value);
215:   }
216: 
217:   /** Updates the indicator given the next bar sample. */
218:   public updateBar(sample: Bar): IndicatorOutput {
219:     return this.updateEntity(sample.time, this.barComponentFunc(sample));
220:   }
221: 
222:   /** Updates the indicator given the next quote sample. */
223:   public updateQuote(sample: Quote): IndicatorOutput {
224:     return this.updateEntity(sample.time, this.quoteComponentFunc(sample));
225:   }
226: 
227:   /** Updates the indicator given the next trade sample. */
228:   public updateTrade(sample: Trade): IndicatorOutput {
229:     return this.updateEntity(sample.time, this.tradeComponentFunc(sample));
230:   }
231: 
232:   private updateEntity(time: Date, sample: number): IndicatorOutput {
233:     const [heatmap, dc, dcm] = this.update(sample, time);
234: 
235:     const sDc = new Scalar();
236:     sDc.time = time;
237:     sDc.value = dc;
238: 
239:     const sDcm = new Scalar();
240:     sDcm.time = time;
241:     sDcm.value = dcm;
242: 
243:     return [heatmap, sDc, sDcm];
244:   }
245: }

(End of file - total 245 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum.spec.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum.spec.ts</path>
<type>file</type>
<content>1: import { } from 'jasmine';
2: 
3: import { Bar } from '../../../entities/bar';
4: import { Quote } from '../../../entities/quote';
5: import { Scalar } from '../../../entities/scalar';
6: import { Trade } from '../../../entities/trade';
7: import { IndicatorType } from '../../core/indicator-type';
8: import { Heatmap } from '../../core/outputs/heatmap';
9: import { OutputType } from '../../core/outputs/output-type';
10: import { coronaTestInput } from '../corona/corona-test-input';
11: import { CoronaSpectrum } from './corona-spectrum';
12: import { CoronaSpectrumOutput } from './corona-spectrum-output';
13: 
14: const tolerance = 1e-4;
15: 
16: describe('CoronaSpectrum', () => {
17:   const time = new Date(2021, 3, 1);
18: 
19:   it('should have correct output enum values', () => {
20:     expect(CoronaSpectrumOutput.Value).toBe(0);
21:     expect(CoronaSpectrumOutput.DominantCycle).toBe(1);
22:     expect(CoronaSpectrumOutput.DominantCycleMedian).toBe(2);
23:   });
24: 
25:   it('should return expected mnemonic for default params', () => {
26:     const x = CoronaSpectrum.default();
27:     expect(x.metadata().mnemonic).toBe('cspect(6, 20, 6, 30, 30, hl/2)');
28:   });
29: 
30:   it('should round custom param ranges (ceil min, floor max)', () => {
31:     const x = CoronaSpectrum.fromParams({
32:       minRasterValue: 4,
33:       maxRasterValue: 25,
34:       minParameterValue: 8.7,   // ceils to 9
35:       maxParameterValue: 40.4,  // floors to 40
36:       highPassFilterCutoff: 20,
37:     });
38:     expect(x.metadata().mnemonic).toBe('cspect(4, 25, 9, 40, 20, hl/2)');
39:   });
40: 
41:   it('should return expected metadata', () => {
42:     const x = CoronaSpectrum.default();
43:     const meta = x.metadata();
44:     const mn = 'cspect(6, 20, 6, 30, 30, hl/2)';
45:     const mnDC = 'cspect-dc(30, hl/2)';
46:     const mnDCM = 'cspect-dcm(30, hl/2)';
47: 
48:     expect(meta.type).toBe(IndicatorType.CoronaSpectrum);
49:     expect(meta.mnemonic).toBe(mn);
50:     expect(meta.description).toBe('Corona spectrum ' + mn);
51:     expect(meta.outputs.length).toBe(3);
52: 
53:     expect(meta.outputs[0].kind).toBe(CoronaSpectrumOutput.Value);
54:     expect(meta.outputs[0].type).toBe(OutputType.Heatmap);
55:     expect(meta.outputs[0].mnemonic).toBe(mn);
56: 
57:     expect(meta.outputs[1].kind).toBe(CoronaSpectrumOutput.DominantCycle);
58:     expect(meta.outputs[1].type).toBe(OutputType.Scalar);
59:     expect(meta.outputs[1].mnemonic).toBe(mnDC);
60:     expect(meta.outputs[1].description).toBe('Corona spectrum dominant cycle ' + mnDC);
61: 
62:     expect(meta.outputs[2].kind).toBe(CoronaSpectrumOutput.DominantCycleMedian);
63:     expect(meta.outputs[2].type).toBe(OutputType.Scalar);
64:     expect(meta.outputs[2].mnemonic).toBe(mnDCM);
65:     expect(meta.outputs[2].description).toBe('Corona spectrum dominant cycle median ' + mnDCM);
66:   });
67: 
68:   it('should throw for invalid params', () => {
69:     expect(() => CoronaSpectrum.fromParams({ minRasterValue: -1 })).toThrowError(/MinRasterValue/);
70:     expect(() => CoronaSpectrum.fromParams({ minRasterValue: 10, maxRasterValue: 10 })).toThrowError(/MaxRasterValue/);
71:     expect(() => CoronaSpectrum.fromParams({ minParameterValue: 1 })).toThrowError(/MinParameterValue/);
72:     expect(() => CoronaSpectrum.fromParams({ minParameterValue: 20, maxParameterValue: 20 })).toThrowError(/MaxParameterValue/);
73:     expect(() => CoronaSpectrum.fromParams({ highPassFilterCutoff: 1 })).toThrowError(/HighPassFilterCutoff/);
74:   });
75: 
76:   it('should return empty heatmap and NaN scalars for NaN input', () => {
77:     const x = CoronaSpectrum.default();
78:     const [h, dc, dcm] = x.update(Number.NaN, time);
79:     expect(h instanceof Heatmap).toBe(true);
80:     expect(h.isEmpty()).toBe(true);
81:     expect(h.parameterFirst).toBe(6);
82:     expect(h.parameterLast).toBe(30);
83:     expect(h.parameterResolution).toBe(2);
84:     expect(Number.isNaN(dc)).toBe(true);
85:     expect(Number.isNaN(dcm)).toBe(true);
86:     expect(x.isPrimed()).toBe(false);
87:   });
88: 
89:   it('should prime at sample index 11 (MinimalPeriodTimesTwo = 12)', () => {
90:     const x = CoronaSpectrum.default();
91:     expect(x.isPrimed()).toBe(false);
92:     let primedAt = -1;
93:     for (let i = 0; i < coronaTestInput.length; i++) {
94:       x.update(coronaTestInput[i], new Date(time.getTime() + i * 60_000));
95:       if (x.isPrimed() && primedAt < 0) primedAt = i;
96:     }
97:     expect(primedAt).toBe(11);
98:   });
99: 
100:   it('should match reference dc / dcm snapshots', () => {
101:     const snapshots: Array<[number, number, number]> = [
102:       [11, 17.7604672565, 17.7604672565],
103:       [12, 6.0000000000, 6.0000000000],
104:       [50, 15.9989078712, 15.9989078712],
105:       [100, 14.7455497547, 14.7455497547],
106:       [150, 17.5000000000, 17.2826036069],
107:       [200, 19.7557338512, 20.0000000000],
108:       [251, 6.0000000000, 6.0000000000],
109:     ];
110: 
111:     const x = CoronaSpectrum.default();
112:     let si = 0;
113:     for (let i = 0; i < coronaTestInput.length; i++) {
114:       const t = new Date(time.getTime() + i * 60_000);
115:       const [h, dc, dcm] = x.update(coronaTestInput[i], t);
116: 
117:       expect(h.parameterFirst).toBe(6);
118:       expect(h.parameterLast).toBe(30);
119:       expect(h.parameterResolution).toBe(2);
120: 
121:       if (!x.isPrimed()) {
122:         expect(h.isEmpty()).withContext(`bar ${i}`).toBe(true);
123:         expect(Number.isNaN(dc)).toBe(true);
124:         expect(Number.isNaN(dcm)).toBe(true);
125:         continue;
126:       }
127: 
128:       expect(h.values.length).toBe(49);
129: 
130:       if (si < snapshots.length && snapshots[si][0] === i) {
131:         expect(Math.abs(snapshots[si][1] - dc))
132:           .withContext(`dc[${i}]: expected ${snapshots[si][1]}, actual ${dc}`)
133:           .toBeLessThan(tolerance);
134:         expect(Math.abs(snapshots[si][2] - dcm))
135:           .withContext(`dcm[${i}]: expected ${snapshots[si][2]}, actual ${dcm}`)
136:           .toBeLessThan(tolerance);
137:         si++;
138:       }
139:     }
140:     expect(si).toBe(snapshots.length);
141:   });
142: 
143:   const primeCount = 50;
144:   const value = 100.0;
145: 
146:   function checkOutput(out: any[]): void {
147:     expect(out.length).toBe(3);
148:     expect(out[0] instanceof Heatmap).toBe(true);
149:     expect((out[0] as Heatmap).time).toEqual(time);
150:     for (let i = 1; i < 3; i++) {
151:       const s = out[i] as Scalar;
152:       expect(s.time).toEqual(time);
153:     }
154:   }
155: 
156:   it('should produce 3-element output via updateScalar', () => {
157:     const x = CoronaSpectrum.default();
158:     for (let i = 0; i < primeCount; i++) {
159:       x.update(coronaTestInput[i % coronaTestInput.length], time);
160:     }
161:     const s = new Scalar();
162:     s.time = time;
163:     s.value = value;
164:     checkOutput(x.updateScalar(s) as any[]);
165:   });
166: 
167:   it('should produce 3-element output via updateBar', () => {
168:     const x = CoronaSpectrum.default();
169:     for (let i = 0; i < primeCount; i++) {
170:       x.update(coronaTestInput[i % coronaTestInput.length], time);
171:     }
172:     const bar = new Bar({ time, open: value, high: value, low: value, close: value, volume: 0 });
173:     checkOutput(x.updateBar(bar) as any[]);
174:   });
175: 
176:   it('should produce 3-element output via updateQuote', () => {
177:     const x = CoronaSpectrum.default();
178:     for (let i = 0; i < primeCount; i++) {
179:       x.update(coronaTestInput[i % coronaTestInput.length], time);
180:     }
181:     const q = new Quote({ time, bid: value, ask: value, bidSize: 0, askSize: 0 });
182:     checkOutput(x.updateQuote(q) as any[]);
183:   });
184: 
185:   it('should produce 3-element output via updateTrade', () => {
186:     const x = CoronaSpectrum.default();
187:     for (let i = 0; i < primeCount; i++) {
188:       x.update(coronaTestInput[i % coronaTestInput.length], time);
189:     }
190:     const t = new Trade({ time, price: value, volume: 0 });
191:     checkOutput(x.updateTrade(t) as any[]);
192:   });
193: });

(End of file - total 193 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.7s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum-params.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum-params.ts</path>
<type>file</type>
<content>1: import { BarComponent } from '../../../entities/bar-component';
2: import { QuoteComponent } from '../../../entities/quote-component';
3: import { TradeComponent } from '../../../entities/trade-component';
4: 
5: /** Describes parameters to create an instance of the CoronaSpectrum indicator. */
6: export interface CoronaSpectrumParams {
7:   /** Minimal raster value (z) of the heatmap, in decibels. Corresponds to the
8:    * CoronaLowerDecibels threshold.
9:    *
10:    * The default value is 6. A zero value is treated as "use default".
11:    */
12:   minRasterValue?: number;
13: 
14:   /** Maximal raster value (z) of the heatmap, in decibels. Corresponds to the
15:    * CoronaUpperDecibels threshold.
16:    *
17:    * The default value is 20. A zero value is treated as "use default".
18:    */
19:   maxRasterValue?: number;
20: 
21:   /** Minimal ordinate (y) value of the heatmap, representing the minimal cycle
22:    * period covered by the filter bank. Rounded up to the nearest integer.
23:    *
24:    * The default value is 6. A zero value is treated as "use default".
25:    */
26:   minParameterValue?: number;
27: 
28:   /** Maximal ordinate (y) value of the heatmap, representing the maximal cycle
29:    * period covered by the filter bank. Rounded down to the nearest integer.
30:    *
31:    * The default value is 30. A zero value is treated as "use default".
32:    */
33:   maxParameterValue?: number;
34: 
35:   /** High-pass filter cutoff (de-trending period) used by the inner Corona engine.
36:    * Suggested values are 20, 30, 100.
37:    *
38:    * The default value is 30. A zero value is treated as "use default".
39:    */
40:   highPassFilterCutoff?: number;
41: 
42:   /** A component of a bar to use when updating the indicator with a bar sample.
43:    *
44:    * If _undefined_, the default (BarComponent.Median, hl/2) is used, matching
45:    * Ehlers' reference which operates on (High+Low)/2. Since this differs from
46:    * the framework default, it is always shown in the mnemonic.
47:    */
48:   barComponent?: BarComponent;
49: 
50:   /** A component of a quote to use when updating the indicator with a quote sample.
51:    *
52:    * If _undefined_, a default value is used and the component is not shown in the indicator mnemonic.
53:    */
54:   quoteComponent?: QuoteComponent;
55: 
56:   /** A component of a trade to use when updating the indicator with a trade sample.
57:    *
58:    * If _undefined_, a default value is used and the component is not shown in the indicator mnemonic.
59:    */
60:   tradeComponent?: TradeComponent;
61: }

(End of file - total 61 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum-output.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/corona-spectrum-output.ts</path>
<type>file</type>
<content>1: /** Enumerates outputs of the CoronaSpectrum indicator. */
2: export enum CoronaSpectrumOutput {
3:   /** The Corona spectrum heatmap column (decibels across the filter bank). */
4:   Value = 0,
5:   /** The weighted-center-of-gravity dominant cycle estimate. */
6:   DominantCycle = 1,
7:   /** The 5-sample median of DominantCycle. */
8:   DominantCycleMedian = 2,
9: }

(End of file - total 9 lines)
</content>
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona/corona.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/corona/corona.ts</path>
<type>file</type>
<content>1: import { CoronaParams, DefaultCoronaParams } from './corona-params';
2: 
3: const DEFAULT_HIGH_PASS_FILTER_CUTOFF = 30;
4: const DEFAULT_MINIMAL_PERIOD = 6;
5: const DEFAULT_MAXIMAL_PERIOD = 30;
6: const DEFAULT_DECIBELS_LOWER_THRESHOLD = 6;
7: const DEFAULT_DECIBELS_UPPER_THRESHOLD = 20;
8: 
9: const HIGH_PASS_FILTER_BUFFER_SIZE = 6;
10: const FIR_COEF_SUM = 12;
11: 
12: const DELTA_LOWER_THRESHOLD = 0.1;
13: const DELTA_FACTOR = -0.015;
14: const DELTA_SUMMAND = 0.5;
15: 
16: const DOMINANT_CYCLE_BUFFER_SIZE = 5;
17: const DOMINANT_CYCLE_MEDIAN_INDEX = 2;
18: 
19: const DECIBELS_SMOOTHING_ALPHA = 0.33;
20: const DECIBELS_SMOOTHING_ONE_MINUS = 0.67;
21: 
22: const NORMALIZED_AMPLITUDE_FACTOR = 0.99;
23: const DECIBELS_FLOOR = 0.01;
24: const DECIBELS_GAIN = 10;
25: 
26: /** Per-bin state of a single bandpass filter in the Corona bank. */
27: export interface CoronaFilter {
28:   inPhase: number;
29:   inPhasePrev: number;
30:   quadrature: number;
31:   quadraturePrev: number;
32:   real: number;
33:   realPrev: number;
34:   imaginary: number;
35:   imaginaryPrev: number;
36: 
37:   /** |Real + j·Imaginary|² of the most recent update. */
38:   amplitudeSquared: number;
39: 
40:   /** Smoothed dB value of the most recent update. */
41:   decibels: number;
42: }
43: 
44: /** Shared spectral-analysis engine consumed by the CoronaSpectrum, CoronaSignalToNoiseRatio,
45:  * CoronaSwingPosition and CoronaTrendVigor indicators. It is not an indicator on its own.
46:  *
47:  * Call _update(sample)_ once per bar; then read _isPrimed_, _dominantCycle_,
48:  * _dominantCycleMedian_, _maximalAmplitudeSquared_ and the _filterBank_ array.
49:  *
50:  * Reference: John Ehlers, "Measuring Cycle Periods", Stocks & Commodities, November 2008. */
51: export class Corona {
52:   public readonly minimalPeriod: number;
53:   public readonly maximalPeriod: number;
54:   public readonly minimalPeriodTimesTwo: number;
55:   public readonly maximalPeriodTimesTwo: number;
56:   public readonly filterBankLength: number;
57: 
58:   private readonly decibelsLowerThreshold: number;
59:   private readonly decibelsUpperThreshold: number;
60:   private readonly alpha: number;
61:   private readonly halfOnePlusAlpha: number;
62:   private readonly preCalculatedBeta: number[];
63: 
64:   public readonly filterBank: CoronaFilter[];
65:   private readonly highPassBuffer: number[];
66:   private readonly dominantCycleBuffer: number[];
67: 
68:   private samplePrev = 0;
69:   private smoothHPPrev = 0;
70:   private _maximalAmplitudeSquared = 0;
71:   private sampleCount = 0;
72:   private _dominantCycle: number;
73:   private _dominantCycleMedian: number;
74:   private _primed = false;
75: 
76:   /** Creates a Corona engine. Zero/undefined parameters are replaced with the Ehlers defaults. */
77:   public constructor(params?: CoronaParams) {
78:     const hp = params?.highPassFilterCutoff;
79:     const minP = params?.minimalPeriod;
80:     const maxP = params?.maximalPeriod;
81:     const dbLo = params?.decibelsLowerThreshold;
82:     const dbHi = params?.decibelsUpperThreshold;
83: 
84:     const cfg = {
85:       highPassFilterCutoff: hp !== undefined && hp > 0 ? hp : DEFAULT_HIGH_PASS_FILTER_CUTOFF,
86:       minimalPeriod: minP !== undefined && minP > 0 ? minP : DEFAULT_MINIMAL_PERIOD,
87:       maximalPeriod: maxP !== undefined && maxP > 0 ? maxP : DEFAULT_MAXIMAL_PERIOD,
88:       decibelsLowerThreshold: dbLo !== undefined && dbLo !== 0 ? dbLo : DEFAULT_DECIBELS_LOWER_THRESHOLD,
89:       decibelsUpperThreshold: dbHi !== undefined && dbHi !== 0 ? dbHi : DEFAULT_DECIBELS_UPPER_THRESHOLD,
90:     };
91: 
92:     if (cfg.highPassFilterCutoff < 2) {
93:       throw new Error('invalid corona parameters: HighPassFilterCutoff should be >= 2');
94:     }
95:     if (cfg.minimalPeriod < 2) {
96:       throw new Error('invalid corona parameters: MinimalPeriod should be >= 2');
97:     }
98:     if (cfg.maximalPeriod <= cfg.minimalPeriod) {
99:       throw new Error('invalid corona parameters: MaximalPeriod should be > MinimalPeriod');
100:     }
101:     if (cfg.decibelsLowerThreshold < 0) {
102:       throw new Error('invalid corona parameters: DecibelsLowerThreshold should be >= 0');
103:     }
104:     if (cfg.decibelsUpperThreshold <= cfg.decibelsLowerThreshold) {
105:       throw new Error('invalid corona parameters: DecibelsUpperThreshold should be > DecibelsLowerThreshold');
106:     }
107: 
108:     this.minimalPeriod = cfg.minimalPeriod;
109:     this.maximalPeriod = cfg.maximalPeriod;
110:     this.minimalPeriodTimesTwo = cfg.minimalPeriod * 2;
111:     this.maximalPeriodTimesTwo = cfg.maximalPeriod * 2;
112:     this.decibelsLowerThreshold = cfg.decibelsLowerThreshold;
113:     this.decibelsUpperThreshold = cfg.decibelsUpperThreshold;
114: 
115:     this.filterBankLength = this.maximalPeriodTimesTwo - this.minimalPeriodTimesTwo + 1;
116: 
117:     this.filterBank = new Array<CoronaFilter>(this.filterBankLength);
118:     for (let i = 0; i < this.filterBankLength; i++) {
119:       this.filterBank[i] = {
120:         inPhase: 0, inPhasePrev: 0,
121:         quadrature: 0, quadraturePrev: 0,
122:         real: 0, realPrev: 0,
123:         imaginary: 0, imaginaryPrev: 0,
124:         amplitudeSquared: 0,
125:         decibels: 0,
126:       };
127:     }
128: 
129:     this.highPassBuffer = new Array<number>(HIGH_PASS_FILTER_BUFFER_SIZE).fill(0);
130: 
131:     // MBST initializes the dominant cycle buffer with MaxValue sentinels.
132:     this.dominantCycleBuffer = new Array<number>(DOMINANT_CYCLE_BUFFER_SIZE).fill(Number.MAX_VALUE);
133:     this._dominantCycle = Number.MAX_VALUE;
134:     this._dominantCycleMedian = Number.MAX_VALUE;
135: 
136:     const phi = (2 * Math.PI) / cfg.highPassFilterCutoff;
137:     this.alpha = (1 - Math.sin(phi)) / Math.cos(phi);
138:     this.halfOnePlusAlpha = 0.5 * (1 + this.alpha);
139: 
140:     this.preCalculatedBeta = new Array<number>(this.filterBankLength);
141:     for (let idx = 0; idx < this.filterBankLength; idx++) {
142:       const n = this.minimalPeriodTimesTwo + idx;
143:       this.preCalculatedBeta[idx] = Math.cos((4 * Math.PI) / n);
144:     }
145:   }
146: 
147:   /** Indicates whether the engine has seen enough samples to produce meaningful output. */
148:   public isPrimed(): boolean { return this._primed; }
149: 
150:   /** The most recent weighted-center-of-gravity estimate of the dominant cycle period. */
151:   public get dominantCycle(): number { return this._dominantCycle; }
152: 
153:   /** The 5-sample median of the most recent dominant cycle estimates. */
154:   public get dominantCycleMedian(): number { return this._dominantCycleMedian; }
155: 
156:   /** The maximum amplitude-squared observed across the filter bank for the most recently
157:    * processed sample. Reset to zero at the start of every update call. */
158:   public get maximalAmplitudeSquared(): number { return this._maximalAmplitudeSquared; }
159: 
160:   /** Feeds the next sample to the engine. NaN samples are a no-op. Returns true once primed. */
161:   public update(sample: number): boolean {
162:     if (Number.isNaN(sample)) {
163:       return this._primed;
164:     }
165: 
166:     this.sampleCount++;
167: 
168:     // First sample: store as prior reference and return without further processing.
169:     if (this.sampleCount === 1) {
170:       this.samplePrev = sample;
171:       return false;
172:     }
173: 
174:     // Step 1: High-pass filter.
175:     const hp = this.alpha * this.highPassBuffer[HIGH_PASS_FILTER_BUFFER_SIZE - 1]
176:       + this.halfOnePlusAlpha * (sample - this.samplePrev);
177:     this.samplePrev = sample;
178: 
179:     for (let i = 0; i < HIGH_PASS_FILTER_BUFFER_SIZE - 1; i++) {
180:       this.highPassBuffer[i] = this.highPassBuffer[i + 1];
181:     }
182:     this.highPassBuffer[HIGH_PASS_FILTER_BUFFER_SIZE - 1] = hp;
183: 
184:     // Step 2: 6-tap FIR smoothing {1, 2, 3, 3, 2, 1} / 12.
185:     const smoothHP = (this.highPassBuffer[0]
186:       + 2 * this.highPassBuffer[1]
187:       + 3 * this.highPassBuffer[2]
188:       + 3 * this.highPassBuffer[3]
189:       + 2 * this.highPassBuffer[4]
190:       + this.highPassBuffer[5]) / FIR_COEF_SUM;
191: 
192:     // Step 3: Momentum.
193:     const momentum = smoothHP - this.smoothHPPrev;
194:     this.smoothHPPrev = smoothHP;
195: 
196:     // Step 4: Adaptive delta.
197:     let delta = DELTA_FACTOR * this.sampleCount + DELTA_SUMMAND;
198:     if (delta < DELTA_LOWER_THRESHOLD) {
199:       delta = DELTA_LOWER_THRESHOLD;
200:     }
201: 
202:     // Step 5: Filter-bank update.
203:     this._maximalAmplitudeSquared = 0;
204:     for (let idx = 0; idx < this.filterBankLength; idx++) {
205:       const n = this.minimalPeriodTimesTwo + idx;
206:       const nf = n;
207: 
208:       const gamma = 1 / Math.cos((8 * Math.PI * delta) / nf);
209:       const a = gamma - Math.sqrt(gamma * gamma - 1);
210: 
211:       const quadrature = momentum * (nf / (4 * Math.PI));
212:       const inPhase = smoothHP;
213: 
214:       const halfOneMinA = 0.5 * (1 - a);
215:       const beta = this.preCalculatedBeta[idx];
216:       const betaOnePlusA = beta * (1 + a);
217: 
218:       const f = this.filterBank[idx];
219: 
220:       const real = halfOneMinA * (inPhase - f.inPhasePrev) + betaOnePlusA * f.real - a * f.realPrev;
221:       const imag = halfOneMinA * (quadrature - f.quadraturePrev) + betaOnePlusA * f.imaginary - a * f.imaginaryPrev;
222: 
223:       const ampSq = real * real + imag * imag;
224: 
225:       f.inPhasePrev = f.inPhase;
226:       f.inPhase = inPhase;
227:       f.quadraturePrev = f.quadrature;
228:       f.quadrature = quadrature;
229:       f.realPrev = f.real;
230:       f.real = real;
231:       f.imaginaryPrev = f.imaginary;
232:       f.imaginary = imag;
233:       f.amplitudeSquared = ampSq;
234: 
235:       if (ampSq > this._maximalAmplitudeSquared) {
236:         this._maximalAmplitudeSquared = ampSq;
237:       }
238:     }
239: 
240:     // Step 6: dB normalization and dominant-cycle weighted average.
241:     let numerator = 0;
242:     let denominator = 0;
243:     this._dominantCycle = 0;
244: 
245:     for (let idx = 0; idx < this.filterBankLength; idx++) {
246:       const f = this.filterBank[idx];
247: 
248:       let decibels = 0;
249:       if (this._maximalAmplitudeSquared > 0) {
250:         const normalized = f.amplitudeSquared / this._maximalAmplitudeSquared;
251:         if (normalized > 0) {
252:           const arg = (1 - NORMALIZED_AMPLITUDE_FACTOR * normalized) / DECIBELS_FLOOR;
253:           if (arg > 0) {
254:             decibels = DECIBELS_GAIN * Math.log10(arg);
255:           }
256:         }
257:       }
258: 
259:       decibels = DECIBELS_SMOOTHING_ALPHA * decibels + DECIBELS_SMOOTHING_ONE_MINUS * f.decibels;
260:       if (decibels > this.decibelsUpperThreshold) {
261:         decibels = this.decibelsUpperThreshold;
262:       }
263:       f.decibels = decibels;
264: 
265:       if (decibels <= this.decibelsLowerThreshold) {
266:         const n = this.minimalPeriodTimesTwo + idx;
267:         const adjusted = this.decibelsUpperThreshold - decibels;
268:         numerator += n * adjusted;
269:         denominator += adjusted;
270:       }
271:     }
272: 
273:     if (denominator !== 0) {
274:       this._dominantCycle = (0.5 * numerator) / denominator;
275:     }
276:     if (this._dominantCycle < this.minimalPeriod) {
277:       this._dominantCycle = this.minimalPeriod;
278:     }
279: 
280:     // Step 7: 5-sample median of dominant cycle.
281:     for (let i = 0; i < DOMINANT_CYCLE_BUFFER_SIZE - 1; i++) {
282:       this.dominantCycleBuffer[i] = this.dominantCycleBuffer[i + 1];
283:     }
284:     this.dominantCycleBuffer[DOMINANT_CYCLE_BUFFER_SIZE - 1] = this._dominantCycle;
285: 
286:     const sorted = [...this.dominantCycleBuffer].sort((a, b) => a - b);
287:     this._dominantCycleMedian = sorted[DOMINANT_CYCLE_MEDIAN_INDEX];
288:     if (this._dominantCycleMedian < this.minimalPeriod) {
289:       this._dominantCycleMedian = this.minimalPeriod;
290:     }
291: 
292:     if (this.sampleCount < this.minimalPeriodTimesTwo) {
293:       return false;
294:     }
295:     this._primed = true;
296:     return true;
297:   }
298: }
299: 
300: // Re-exports avoid unused-import warning in bundlers.
301: export { DefaultCoronaParams };

(End of file - total 301 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 16.1s)

_Thinking:_

 Now I'm setting up the CSNR file structure.

Now I have enough to create the CSNR package.

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-signal-to-noise-ratio/corona-signal-to-noise-ratio-output.ts",
  "content": "/** Enumerates outputs of the CoronaSignalToNoiseRatio indicator. */\nexport enum CoronaSignalToNoiseRatioOutput {\n  /** The Corona signal-to-noise ratio heatmap column. */\n  Value = 0,\n  /** The current signal-to-noise ratio scalar, mapped to [MinParameterValue, MaxParameterValue]. */\n  SignalToNoiseRatio = 1,\n}\n"
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

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-signal-to-noise-ratio/corona-signal-to-noise-ratio-params.ts",
  "content": "import { BarComponent } from '../../../entities/bar-component';\nimport { QuoteComponent } from '../../../entities/quote-component';\nimport { TradeComponent } from '../../../entities/trade-component';\n\n/** Describes parameters to create an instance of the CoronaSignalToNoiseRatio indicator. */\nexport interface CoronaSignalToNoiseRatioParams {\n  /** Length of the heatmap raster (number of intensity bins). Default 50. A zero value is treated as \"use default\". */\n  rasterLength?: number;\n\n  /** Maximum raster intensity value. Default 20. A zero value is treated as \"use default\". */\n  maxRasterValue?: number;\n\n  /** Minimum ordinate (y) value of the heatmap — lower bound of the mapped SNR. Default 1. A zero value is treated as \"use default\". */\n  minParameterValue?: number;\n\n  /** Maximum ordinate (y) value of the heatmap — upper bound of the mapped SNR. Default 11. A zero value is treated as \"use default\". */\n  maxParameterValue?: number;\n\n  /** High-pass filter cutoff used by the inner Corona engine. Default 30. A zero value is treated as \"use default\". */\n  highPassFilterCutoff?: number;\n\n  /** Minimal cycle period covered by the filter bank. Default 6. A zero value is treated as \"use default\". */\n  minimalPeriod?: number;\n\n  /** Maximal cycle period covered by the filter bank. Default 30. A zero value is treated as \"use default\". */\n  maximalPeriod?: number;\n\n  /** A component of a bar to use when updating the indicator with a bar sample.\n   *\n   * If _undefined_, the default (BarComponent.Median, hl/2) is used, matching\n   * Ehlers' reference which operates on (High+Low)/2.\n   */\n  barComponent?: BarComponent;\n\n  /** A component of a quote to use when updating the indicator with a quote sample. */\n  quoteComponent?: QuoteComponent;\n\n  /** A component of a trade to use when updating the indicator with a trade sample. */\n  tradeComponent?: TradeComponent;\n}\n"
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

## Assistant (Build · claude-opus-4.7 · 70.3s)

_Thinking:_

 I'll set aside those C LSP errors and move on to building out the main class structure.

**Tool: write**

**Input:**
```json
{
  "content": "import { Bar } from '../../../entities/bar';\nimport { BarComponent, barComponentValue } from '../../../entities/bar-component';\nimport { Quote } from '../../../entities/quote';\nimport { DefaultQuoteComponent, quoteComponentValue } from '../../../entities/quote-component';\nimport { Scalar } from '../../../entities/scalar';\nimport { Trade } from '../../../entities/trade';\nimport { DefaultTradeComponent, tradeComponentValue } from '../../../entities/trade-component';\nimport { componentTripleMnemonic } from '../../core/component-triple-mnemonic';\nimport { Indicator } from '../../core/indicator';\nimport { IndicatorMetadata } from '../../core/indicator-metadata';\nimport { IndicatorOutput } from '../../core/indicator-output';\nimport { IndicatorType } from '../../core/indicator-type';\nimport { Heatmap } from '../../core/outputs/heatmap';\nimport { OutputType } from '../../core/outputs/output-type';\nimport { Corona } from '../corona/corona';\nimport { CoronaSignalToNoiseRatioOutput } from './corona-signal-to-noise-ratio-output';\nimport { CoronaSignalToNoiseRatioParams } from './corona-signal-to-noise-ratio-params';\n\nconst DEFAULT_RASTER_LENGTH = 50;\nconst DEFAULT_MAX_RASTER = 20;\nconst DEFAULT_MIN_PARAM = 1;\nconst DEFAULT_MAX_PARAM = 11;\nconst DEFAULT_HP_CUTOFF = 30;\nconst DEFAULT_MIN_PERIOD = 6;\nconst DEFAULT_MAX_PERIOD = 30;\n\nconst HIGH_LOW_BUFFER_SIZE = 5;\nconst HIGH_LOW_BUFFER_SIZE_MIN_ONE = HIGH_LOW_BUFFER_SIZE - 1;\nconst HIGH_LOW_MEDIAN_INDEX = 2;\nconst AVERAGE_SAMPLE_ALPHA = 0.1;\nconst AVERAGE_SAMPLE_ONE_MINUS = 0.9;\nconst SIGNAL_EMA_ALPHA = 0.2;\nconst SIGNAL_EMA_ONE_MINUS = 0.9; // Intentional: sums to 1.1, per Ehlers.\nconst NOISE_EMA_ALPHA = 0.1;\nconst NOISE_EMA_ONE_MINUS = 0.9;\nconst RATIO_OFFSET_DB = 3.5;\nconst RATIO_UPPER_DB = 10;\nconst DB_GAIN = 20;\nconst WIDTH_LOW_RATIO_THRESHOLD = 0.5;\nconst WIDTH_BASELINE = 0.2;\nconst WIDTH_SLOPE = 0.4;\nconst RASTER_BLEND_EXPONENT = 0.8;\nconst RASTER_BLEND_HALF = 0.5;\nconst RASTER_NEGATIVE_ARG_CUTOFF = 1;\n\n/** __Corona Signal-to-Noise Ratio__ (Ehlers) heatmap indicator.\n *\n * Measures cycle amplitude relative to noise, where \"noise\" is the average bar height\n * (there is not much trade information within a bar).\n *\n * It exposes two outputs:\n *\n *  - Value: a per-bar heatmap column (intensity raster).\n *  - SignalToNoiseRatio: the current SNR value mapped into [MinParameterValue, MaxParameterValue].\n *\n * Reference: John Ehlers, \"Measuring Cycle Periods\", Stocks & Commodities, November 2008.\n */\nexport class CoronaSignalToNoiseRatio implements Indicator {\n  private readonly c: Corona;\n  private readonly rasterLength: number;\n  private readonly rasterStep: number;\n  private readonly maxRasterValue: number;\n  private readonly minParameterValue: number;\n  private readonly maxParameterValue: number;\n  private readonly parameterResolution: number;\n  private readonly raster: number[];\n  private readonly highLowBuffer: number[];\n  private readonly hlSorted: number[];\n\n  private readonly mnemonicValue: string;\n  private readonly descriptionValue: string;\n  private readonly mnemonicSNR: string;\n  private readonly descriptionSNR: string;\n\n  private readonly barComponentFunc: (bar: Bar) => number;\n  private readonly quoteComponentFunc: (quote: Quote) => number;\n  private readonly tradeComponentFunc: (trade: Trade) => number;\n\n  private averageSamplePrev = 0;\n  private signalPrev = 0;\n  private noisePrev = 0;\n  private signalToNoiseRatio = Number.NaN;\n  private isStarted = false;\n\n  /** Creates an instance with default parameters. */\n  public static default(): CoronaSignalToNoiseRatio {\n    return new CoronaSignalToNoiseRatio({});\n  }\n\n  /** Creates an instance based on the given parameters. */\n  public static fromParams(params: CoronaSignalToNoiseRatioParams): CoronaSignalToNoiseRatio {\n    return new CoronaSignalToNoiseRatio(params);\n  }\n\n  private constructor(params: CoronaSignalToNoiseRatioParams) {\n    const invalid = 'invalid corona signal to noise ratio parameters';\n\n    const rasterLength = params.rasterLength !== undefined && params.rasterLength !== 0\n      ? params.rasterLength : DEFAULT_RASTER_LENGTH;\n    const maxRaster = params.maxRasterValue !== undefined && params.maxRasterValue !== 0\n      ? params.maxRasterValue : DEFAULT_MAX_RASTER;\n    const minParam = params.minParameterValue !== undefined && params.minParameterValue !== 0\n      ? params.minParameterValue : DEFAULT_MIN_PARAM;\n    const maxParam = params.maxParameterValue !== undefined && params.maxParameterValue !== 0\n      ? params.maxParameterValue : DEFAULT_MAX_PARAM;\n    const hpCutoff = params.highPassFilterCutoff !== undefined && params.highPassFilterCutoff !== 0\n      ? params.highPassFilterCutoff : DEFAULT_HP_CUTOFF;\n    const minPeriod = params.minimalPeriod !== undefined && params.minimalPeriod !== 0\n      ? params.minimalPeriod : DEFAULT_MIN_PERIOD;\n    const maxPeriod = params.maximalPeriod !== undefined && params.maximalPeriod !== 0\n      ? params.maximalPeriod : DEFAULT_MAX_PERIOD;\n\n    if (rasterLength < 2) {\n      throw new Error(`${invalid}: RasterLength should be >= 2`);\n    }\n    if (maxRaster <= 0) {\n      throw new Error(`${invalid}: MaxRasterValue should be > 0`);\n    }\n    if (minParam < 0) {\n      throw new Error(`${invalid}: MinParameterValue should be >= 0`);\n    }\n    if (maxParam <= minParam) {\n      throw new Error(`${invalid}: MaxParameterValue should be > MinParameterValue`);\n    }\n    if (hpCutoff < 2) {\n      throw new Error(`${invalid}: HighPassFilterCutoff should be >= 2`);\n    }\n    if (minPeriod < 2) {\n      throw new Error(`${invalid}: MinimalPeriod should be >= 2`);\n    }\n    if (maxPeriod <= minPeriod) {\n      throw new Error(`${invalid}: MaximalPeriod should be > MinimalPeriod`);\n    }\n\n    const bc = params.barComponent ?? BarComponent.Median;\n    const qc = params.quoteComponent ?? DefaultQuoteComponent;\n    const tc = params.tradeComponent ?? DefaultTradeComponent;\n\n    this.barComponentFunc = barComponentValue(bc);\n    this.quoteComponentFunc = quoteComponentValue(qc);\n    this.tradeComponentFunc = tradeComponentValue(tc);\n\n    this.c = new Corona({\n      highPassFilterCutoff: hpCutoff,\n      minimalPeriod: minPeriod,\n      maximalPeriod: maxPeriod,\n    });\n\n    this.rasterLength = rasterLength;\n    this.rasterStep = maxRaster / rasterLength;\n    this.maxRasterValue = maxRaster;\n    this.minParameterValue = minParam;\n    this.maxParameterValue = maxParam;\n    this.parameterResolution = (rasterLength - 1) / (maxParam - minParam);\n\n    this.raster = new Array<number>(rasterLength).fill(0);\n    this.highLowBuffer = new Array<number>(HIGH_LOW_BUFFER_SIZE).fill(0);\n    this.hlSorted = new Array<number>(HIGH_LOW_BUFFER_SIZE).fill(0);\n\n    const cm = componentTripleMnemonic(bc, qc, tc);\n    this.mnemonicValue = `csnr(${rasterLength}, ${maxRaster}, ${minParam}, ${maxParam}, ${hpCutoff}${cm})`;\n    this.mnemonicSNR = `csnr-snr(${hpCutoff}${cm})`;\n    this.descriptionValue = 'Corona signal to noise ratio ' + this.mnemonicValue;\n    this.descriptionSNR = 'Corona signal to noise ratio scalar ' + this.mnemonicSNR;\n  }\n\n  /** Indicates whether the indicator is primed. */\n  public isPrimed(): boolean { return this.c.isPrimed(); }\n\n  /** Describes the output data of the indicator. */\n  public metadata(): IndicatorMetadata {\n    return {\n      type: IndicatorType.CoronaSignalToNoiseRatio,\n      mnemonic: this.mnemonicValue,\n      description: this.descriptionValue,\n      outputs: [\n        {\n          kind: CoronaSignalToNoiseRatioOutput.Value,\n          type: OutputType.Heatmap,\n          mnemonic: this.mnemonicValue,\n          description: this.descriptionValue,\n        },\n        {\n          kind: CoronaSignalToNoiseRatioOutput.SignalToNoiseRatio,\n          type: OutputType.Scalar,\n          mnemonic: this.mnemonicSNR,\n          description: this.descriptionSNR,\n        },\n      ],\n    };\n  }\n\n  /** Feeds the next sample plus bar extremes and returns the heatmap column and the\n   * current SignalToNoiseRatio. On unprimed bars the heatmap is empty and the scalar\n   * is NaN. On NaN sample input state is left unchanged. */\n  public update(sample: number, sampleLow: number, sampleHigh: number, time: Date): [Heatmap, number] {\n    if (Number.isNaN(sample)) {\n      return [\n        Heatmap.newEmptyHeatmap(time, this.minParameterValue, this.maxParameterValue, this.parameterResolution),\n        Number.NaN,\n      ];\n    }\n\n    const primed = this.c.update(sample);\n\n    if (!this.isStarted) {\n      this.averageSamplePrev = sample;\n      this.highLowBuffer[HIGH_LOW_BUFFER_SIZE_MIN_ONE] = sampleHigh - sampleLow;\n      this.isStarted = true;\n\n      return [\n        Heatmap.newEmptyHeatmap(time, this.minParameterValue, this.maxParameterValue, this.parameterResolution),\n        Number.NaN,\n      ];\n    }\n\n    const maxAmpSq = this.c.maximalAmplitudeSquared;\n\n    const averageSample = AVERAGE_SAMPLE_ALPHA * sample + AVERAGE_SAMPLE_ONE_MINUS * this.averageSamplePrev;\n    this.averageSamplePrev = averageSample;\n\n    if (Math.abs(averageSample) > 0 || maxAmpSq > 0) {\n      this.signalPrev = SIGNAL_EMA_ALPHA * Math.sqrt(maxAmpSq) + SIGNAL_EMA_ONE_MINUS * this.signalPrev;\n    }\n\n    // Shift H-L ring buffer left; push new value.\n    for (let i = 0; i < HIGH_LOW_BUFFER_SIZE_MIN_ONE; i++) {\n      this.highLowBuffer[i] = this.highLowBuffer[i + 1];\n    }\n    this.highLowBuffer[HIGH_LOW_BUFFER_SIZE_MIN_ONE] = sampleHigh - sampleLow;\n\n    let ratio = 0;\n    if (Math.abs(averageSample) > 0) {\n      for (let i = 0; i < HIGH_LOW_BUFFER_SIZE; i++) {\n        this.hlSorted[i] = this.highLowBuffer[i];\n      }\n      this.hlSorted.sort((a, b) => a - b);\n      this.noisePrev = NOISE_EMA_ALPHA * this.hlSorted[HIGH_LOW_MEDIAN_INDEX] + NOISE_EMA_ONE_MINUS * this.noisePrev;\n\n      if (Math.abs(this.noisePrev) > 0) {\n        ratio = DB_GAIN * Math.log10(this.signalPrev / this.noisePrev) + RATIO_OFFSET_DB;\n        if (ratio < 0) {\n          ratio = 0;\n        } else if (ratio > RATIO_UPPER_DB) {\n          ratio = RATIO_UPPER_DB;\n        }\n        ratio /= RATIO_UPPER_DB; // ∈ [0, 1]\n      }\n    }\n\n    this.signalToNoiseRatio = (this.maxParameterValue - this.minParameterValue) * ratio + this.minParameterValue;\n\n    // Raster update.\n    let width = 0;\n    if (ratio <= WIDTH_LOW_RATIO_THRESHOLD) {\n      width = WIDTH_BASELINE - WIDTH_SLOPE * ratio;\n    }\n\n    const ratioScaledToRasterLength = Math.round(ratio * this.rasterLength);\n    const ratioScaledToMaxRasterValue = ratio * this.maxRasterValue;\n\n    for (let i = 0; i < this.rasterLength; i++) {\n      let value = this.raster[i];\n\n      if (i === ratioScaledToRasterLength) {\n        value *= 0.5;\n      } else if (width === 0) {\n        // Above the high-ratio threshold: handled by the ratio>0.5 override below.\n      } else {\n        let argument = (ratioScaledToMaxRasterValue - this.rasterStep * i) / width;\n        if (i < ratioScaledToRasterLength) {\n          value = RASTER_BLEND_HALF * (Math.pow(argument, RASTER_BLEND_EXPONENT) + value);\n        } else {\n          argument = -argument;\n          if (argument > RASTER_NEGATIVE_ARG_CUTOFF) {\n            value = RASTER_BLEND_HALF * (Math.pow(argument, RASTER_BLEND_EXPONENT) + value);\n          } else {\n            value = this.maxRasterValue;\n          }\n        }\n      }\n\n      if (value < 0) {\n        value = 0;\n      } else if (value > this.maxRasterValue) {\n        value = this.maxRasterValue;\n      }\n\n      if (ratio > WIDTH_LOW_RATIO_THRESHOLD) {\n        value = this.maxRasterValue;\n      }\n\n      this.raster[i] = value;\n    }\n\n    if (!primed) {\n      return [\n        Heatmap.newEmptyHeatmap(time, this.minParameterValue, this.maxParameterValue, this.parameterResolution),\n        Number.NaN,\n      ];\n    }\n\n    const values = new Array<number>(this.rasterLength);\n    let valueMin = Number.POSITIVE_INFINITY;\n    let valueMax = Number.NEGATIVE_INFINITY;\n\n    for (let i = 0; i < this.rasterLength; i++) {\n      const v = this.raster[i];\n      values[i] = v;\n      if (v < valueMin) valueMin = v;\n      if (v > valueMax) valueMax = v;\n    }\n\n    const heatmap = Heatmap.newHeatmap(\n      time, this.minParameterValue, this.maxParameterValue, this.parameterResolution,\n      valueMin, valueMax, values,\n    );\n\n    return [heatmap, this.signalToNoiseRatio];\n  }\n\n  /** Updates the indicator given the next scalar sample. Since no High/Low is\n   * available, the sample is used for both, yielding zero noise. */\n  public updateScalar(sample: Scalar): IndicatorOutput {\n    return this.updateEntity(sample.time, sample.value, sample.value, sample.value);\n  }\n\n  /** Updates the indicator given the next bar sample. */\n  public updateBar(sample: Bar): IndicatorOutput {\n    return this.updateEntity(sample.time, this.barComponentFunc(sample), sample.low, sample.high);\n  }\n\n  /** Updates the indicator given the next quote sample. */\n  public updateQuote(sample: Quote): IndicatorOutput {\n    const v = this.quoteComponentFunc(sample);\n    return this.updateEntity(sample.time, v, v, v);\n  }\n\n  /** Updates the indicator given the next trade sample. */\n  public updateTrade(sample: Trade): IndicatorOutput {\n    const v = this.tradeComponentFunc(sample);\n    return this.updateEntity(sample.time, v, v, v);\n  }\n\n  private updateEntity(time: Date, sample: number, low: number, high: number): IndicatorOutput {\n    const [heatmap, snr] = this.update(sample, low, high, time);\n\n    const s = new Scalar();\n    s.time = time;\n    s.value = snr;\n\n    return [heatmap, s];\n  }\n}\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-signal-to-noise-ratio/corona-signal-to-noise-ratio.ts"
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

## Assistant (Build · claude-opus-4.7 · 37.7s)

Now the spec file:

**Tool: write**

**Input:**
```json
{
  "content": "import { } from 'jasmine';\n\nimport { Bar } from '../../../entities/bar';\nimport { Quote } from '../../../entities/quote';\nimport { Scalar } from '../../../entities/scalar';\nimport { Trade } from '../../../entities/trade';\nimport { IndicatorType } from '../../core/indicator-type';\nimport { Heatmap } from '../../core/outputs/heatmap';\nimport { OutputType } from '../../core/outputs/output-type';\nimport { coronaTestInput } from '../corona/corona-test-input';\nimport { CoronaSignalToNoiseRatio } from './corona-signal-to-noise-ratio';\nimport { CoronaSignalToNoiseRatioOutput } from './corona-signal-to-noise-ratio-output';\n\nconst tolerance = 1e-4;\n\n// Produce synthetic High/Low around the sample matching the Go reference.\nfunction makeHL(i: number, sample: number): [number, number] {\n  const frac = 0.005 + 0.03 * (1 + Math.sin(i * 0.37));\n  const half = sample * frac;\n  return [sample - half, sample + half];\n}\n\ndescribe('CoronaSignalToNoiseRatio', () => {\n  const time = new Date(2021, 3, 1);\n\n  it('should have correct output enum values', () => {\n    expect(CoronaSignalToNoiseRatioOutput.Value).toBe(0);\n    expect(CoronaSignalToNoiseRatioOutput.SignalToNoiseRatio).toBe(1);\n  });\n\n  it('should return expected mnemonic for default params', () => {\n    const x = CoronaSignalToNoiseRatio.default();\n    expect(x.metadata().mnemonic).toBe('csnr(50, 20, 1, 11, 30, hl/2)');\n  });\n\n  it('should return expected metadata', () => {\n    const x = CoronaSignalToNoiseRatio.default();\n    const meta = x.metadata();\n    const mn = 'csnr(50, 20, 1, 11, 30, hl/2)';\n    const mnSNR = 'csnr-snr(30, hl/2)';\n\n    expect(meta.type).toBe(IndicatorType.CoronaSignalToNoiseRatio);\n    expect(meta.mnemonic).toBe(mn);\n    expect(meta.description).toBe('Corona signal to noise ratio ' + mn);\n    expect(meta.outputs.length).toBe(2);\n\n    expect(meta.outputs[0].kind).toBe(CoronaSignalToNoiseRatioOutput.Value);\n    expect(meta.outputs[0].type).toBe(OutputType.Heatmap);\n    expect(meta.outputs[0].mnemonic).toBe(mn);\n\n    expect(meta.outputs[1].kind).toBe(CoronaSignalToNoiseRatioOutput.SignalToNoiseRatio);\n    expect(meta.outputs[1].type).toBe(OutputType.Scalar);\n    expect(meta.outputs[1].mnemonic).toBe(mnSNR);\n  });\n\n  it('should throw for invalid params', () => {\n    expect(() => CoronaSignalToNoiseRatio.fromParams({ rasterLength: 1 })).toThrowError(/RasterLength/);\n    expect(() => CoronaSignalToNoiseRatio.fromParams({ minParameterValue: 5, maxParameterValue: 5 }))\n      .toThrowError(/MaxParameterValue/);\n    expect(() => CoronaSignalToNoiseRatio.fromParams({ highPassFilterCutoff: 1 })).toThrowError(/HighPassFilterCutoff/);\n    expect(() => CoronaSignalToNoiseRatio.fromParams({ minimalPeriod: 1 })).toThrowError(/MinimalPeriod/);\n    expect(() => CoronaSignalToNoiseRatio.fromParams({ minimalPeriod: 10, maximalPeriod: 10 }))\n      .toThrowError(/MaximalPeriod/);\n  });\n\n  it('should return empty heatmap and NaN for NaN input', () => {\n    const x = CoronaSignalToNoiseRatio.default();\n    const [h, snr] = x.update(Number.NaN, Number.NaN, Number.NaN, time);\n    expect(h instanceof Heatmap).toBe(true);\n    expect(h.isEmpty()).toBe(true);\n    expect(Number.isNaN(snr)).toBe(true);\n    expect(x.isPrimed()).toBe(false);\n  });\n\n  it('should prime at sample index 11', () => {\n    const x = CoronaSignalToNoiseRatio.default();\n    expect(x.isPrimed()).toBe(false);\n    let primedAt = -1;\n    for (let i = 0; i < coronaTestInput.length; i++) {\n      const [low, high] = makeHL(i, coronaTestInput[i]);\n      x.update(coronaTestInput[i], low, high, new Date(time.getTime() + i * 60_000));\n      if (x.isPrimed() && primedAt < 0) primedAt = i;\n    }\n    expect(primedAt).toBe(11);\n  });\n\n  it('should match reference snr / vmin / vmax snapshots', () => {\n    const snapshots: Array<[number, number, number, number]> = [\n      [11, 1.0000000000, 0.0000000000, 20.0000000000],\n      [12, 1.0000000000, 0.0000000000, 20.0000000000],\n      [50, 1.0000000000, 0.0000000000, 20.0000000000],\n      [100, 2.9986583538, 4.2011609652, 20.0000000000],\n      [150, 1.0000000000, 0.0000000035, 20.0000000000],\n      [200, 1.0000000000, 0.0000000000, 20.0000000000],\n      [251, 1.0000000000, 0.0000000026, 20.0000000000],\n    ];\n\n    const x = CoronaSignalToNoiseRatio.default();\n    let si = 0;\n    for (let i = 0; i < coronaTestInput.length; i++) {\n      const t = new Date(time.getTime() + i * 60_000);\n      const [low, high] = makeHL(i, coronaTestInput[i]);\n      const [h, snr] = x.update(coronaTestInput[i], low, high, t);\n\n      expect(h.parameterFirst).toBe(1);\n      expect(h.parameterLast).toBe(11);\n      expect(Math.abs(h.parameterResolution - 4.9)).toBeLessThan(1e-9);\n\n      if (!x.isPrimed()) {\n        expect(h.isEmpty()).withContext(`bar ${i}`).toBe(true);\n        expect(Number.isNaN(snr)).toBe(true);\n        continue;\n      }\n\n      expect(h.values.length).toBe(50);\n\n      if (si < snapshots.length && snapshots[si][0] === i) {\n        expect(Math.abs(snapshots[si][1] - snr))\n          .withContext(`snr[${i}]: expected ${snapshots[si][1]}, actual ${snr}`)\n          .toBeLessThan(tolerance);\n        expect(Math.abs(snapshots[si][2] - h.valueMin))\n          .withContext(`vmin[${i}]: expected ${snapshots[si][2]}, actual ${h.valueMin}`)\n          .toBeLessThan(tolerance);\n        expect(Math.abs(snapshots[si][3] - h.valueMax))\n          .withContext(`vmax[${i}]: expected ${snapshots[si][3]}, actual ${h.valueMax}`)\n          .toBeLessThan(tolerance);\n        si++;\n      }\n    }\n    expect(si).toBe(snapshots.length);\n  });\n\n  const primeCount = 50;\n  const value = 100.0;\n\n  function prime(x: CoronaSignalToNoiseRatio): void {\n    for (let i = 0; i < primeCount; i++) {\n      const s = coronaTestInput[i % coronaTestInput.length];\n      const [low, high] = makeHL(i, s);\n      x.update(s, low, high, time);\n    }\n  }\n\n  function checkOutput(out: any[]): void {\n    expect(out.length).toBe(2);\n    expect(out[0] instanceof Heatmap).toBe(true);\n    expect((out[0] as Heatmap).time).toEqual(time);\n    const s = out[1] as Scalar;\n    expect(s.time).toEqual(time);\n  }\n\n  it('should produce 2-element output via updateScalar', () => {\n    const x = CoronaSignalToNoiseRatio.default();\n    prime(x);\n    const s = new Scalar();\n    s.time = time;\n    s.value = value;\n    checkOutput(x.updateScalar(s) as any[]);\n  });\n\n  it('should produce 2-element output via updateBar', () => {\n    const x = CoronaSignalToNoiseRatio.default();\n    prime(x);\n    const bar = new Bar({ time, open: value, high: value * 1.005, low: value * 0.995, close: value, volume: 0 });\n    checkOutput(x.updateBar(bar) as any[]);\n  });\n\n  it('should produce 2-element output via updateQuote', () => {\n    const x = CoronaSignalToNoiseRatio.default();\n    prime(x);\n    const q = new Quote({ time, bid: value, ask: value, bidSize: 0, askSize: 0 });\n    checkOutput(x.updateQuote(q) as any[]);\n  });\n\n  it('should produce 2-element output via updateTrade', () => {\n    const x = CoronaSignalToNoiseRatio.default();\n    prime(x);\n    const t = new Trade({ time, price: value, volume: 0 });\n    checkOutput(x.updateTrade(t) as any[]);\n  });\n});\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-signal-to-noise-ratio/corona-signal-to-noise-ratio.spec.ts"
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

## Assistant (Build · claude-opus-4.7 · 13.2s)

**Tool: bash**

**Input:**
```json
{
  "command": "npm test -- --filter=CoronaSignalToNoiseRatio 2>&1 | tail -60",
  "workdir": "/home/dev/zpano/ts",
  "description": "Run CoronaSignalToNoiseRatio tests"
}
```

**Output:**
```

> ts@1.0.0 test
> node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=CoronaSignalToNoiseRatio

Started
...........


Ran 11 of 772 specs
11 specs, 0 failures
Finished in 0.058 seconds

```

---

## Assistant (Build · claude-opus-4.7 · 4.9s)

All 11 CSNR specs pass. Now CoronaSwingPosition.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition.go</path>
<type>file</type>
<content>1: // Package coronaswingposition implements Ehlers' Corona Swing Position heatmap
2: // indicator.
3: //
4: // The Corona swing position is estimated by correlating the prices with a
5: // perfect sine wave having the dominant cycle period. This correlation produces
6: // a smooth waveform that lets us better estimate the swing position and
7: // impending turning points.
8: //
9: // Reference: John Ehlers, "Measuring Cycle Periods", Stocks & Commodities,
10: // November 2008.
11: package coronaswingposition
12: 
13: //nolint: gofumpt
14: import (
15: 	"fmt"
16: 	"math"
17: 	"sync"
18: 	"time"
19: 
20: 	"zpano/entities"
21: 	"zpano/indicators/core"
22: 	"zpano/indicators/core/outputs"
23: 	"zpano/indicators/johnehlers/corona"
24: )
25: 
26: const (
27: 	maxLeadListCount     = 50
28: 	maxPositionListCount = 20
29: 
30: 	// 60° phase-lead coefficients: lead60 = 0.5·BP + sin(60°)·Q ≈ 0.5·BP + 0.866·Q.
31: 	lead60CoefBP = 0.5
32: 	lead60CoefQ  = 0.866
33: 
34: 	// Bandpass filter delta factor in γ = 1/cos(ω · 2 · 0.1).
35: 	bpDelta = 0.1
36: 
37: 	widthHighThreshold = 0.85
38: 	widthHighSaturate  = 0.8
39: 	widthNarrow        = 0.01
40: 	widthScale         = 0.15
41: 
42: 	rasterBlendExponent = 0.95
43: 	rasterBlendHalf     = 0.5
44: )
45: 
46: // CoronaSwingPosition is Ehlers' Corona Swing Position indicator.
47: type CoronaSwingPosition struct {
48: 	mu                  sync.RWMutex
49: 	mnemonic            string
50: 	description         string
51: 	mnemonicSP          string
52: 	descriptionSP       string
53: 	c                   *corona.Corona
54: 	rasterLength        int
55: 	rasterStep          float64
56: 	maxRasterValue      float64
57: 	minParameterValue   float64
58: 	maxParameterValue   float64
59: 	parameterResolution float64
60: 	raster              []float64
61: 	leadList            []float64
62: 	positionList        []float64
63: 	samplePrev          float64
64: 	samplePrev2         float64
65: 	bandPassPrev        float64
66: 	bandPassPrev2       float64
67: 	swingPosition       float64
68: 	isStarted           bool
69: 	barFunc             entities.BarFunc
70: 	quoteFunc           entities.QuoteFunc
71: 	tradeFunc           entities.TradeFunc
72: }
73: 
74: // NewCoronaSwingPositionDefault returns an instance created with default parameters.
75: func NewCoronaSwingPositionDefault() (*CoronaSwingPosition, error) {
76: 	return NewCoronaSwingPositionParams(&Params{})
77: }
78: 
79: // NewCoronaSwingPositionParams returns an instance created with the supplied parameters.
80: //
81: //nolint:funlen,cyclop
82: func NewCoronaSwingPositionParams(p *Params) (*CoronaSwingPosition, error) {
83: 	const (
84: 		invalid      = "invalid corona swing position parameters"
85: 		fmtRaster    = "%s: RasterLength should be >= 2"
86: 		fmtMaxRaster = "%s: MaxRasterValue should be > 0"
87: 		fmtMaxParam  = "%s: MaxParameterValue should be > MinParameterValue"
88: 		fmtHP        = "%s: HighPassFilterCutoff should be >= 2"
89: 		fmtMinP      = "%s: MinimalPeriod should be >= 2"
90: 		fmtMaxP      = "%s: MaximalPeriod should be > MinimalPeriod"
91: 		fmtw         = "%s: %w"
92: 		fmtnVal      = "cswing(%d, %g, %g, %g, %d%s)"
93: 		fmtnSP       = "cswing-sp(%d%s)"
94: 		descrVal     = "Corona swing position "
95: 		descrSP      = "Corona swing position scalar "
96: 		defRaster    = 50
97: 		defMaxRast   = 20.0
98: 		defMinParam  = -5.0
99: 		defMaxParam  = 5.0
100: 		defHPCutoff  = 30
101: 		defMinPer    = 6
102: 		defMaxPer    = 30
103: 	)
104: 
105: 	cfg := *p
106: 
107: 	if cfg.RasterLength == 0 {
108: 		cfg.RasterLength = defRaster
109: 	}
110: 
111: 	if cfg.MaxRasterValue == 0 {
112: 		cfg.MaxRasterValue = defMaxRast
113: 	}
114: 
115: 	// MinParameterValue and MaxParameterValue default to -5 and 5; since 0 is a
116: 	// valid user value for either, we only substitute when both are zero (the
117: 	// unconfigured case).
118: 	if cfg.MinParameterValue == 0 && cfg.MaxParameterValue == 0 {
119: 		cfg.MinParameterValue = defMinParam
120: 		cfg.MaxParameterValue = defMaxParam
121: 	}
122: 
123: 	if cfg.HighPassFilterCutoff == 0 {
124: 		cfg.HighPassFilterCutoff = defHPCutoff
125: 	}
126: 
127: 	if cfg.MinimalPeriod == 0 {
128: 		cfg.MinimalPeriod = defMinPer
129: 	}
130: 
131: 	if cfg.MaximalPeriod == 0 {
132: 		cfg.MaximalPeriod = defMaxPer
133: 	}
134: 
135: 	if cfg.RasterLength < 2 {
136: 		return nil, fmt.Errorf(fmtRaster, invalid)
137: 	}
138: 
139: 	if cfg.MaxRasterValue <= 0 {
140: 		return nil, fmt.Errorf(fmtMaxRaster, invalid)
141: 	}
142: 
143: 	if cfg.MaxParameterValue <= cfg.MinParameterValue {
144: 		return nil, fmt.Errorf(fmtMaxParam, invalid)
145: 	}
146: 
147: 	if cfg.HighPassFilterCutoff < 2 {
148: 		return nil, fmt.Errorf(fmtHP, invalid)
149: 	}
150: 
151: 	if cfg.MinimalPeriod < 2 {
152: 		return nil, fmt.Errorf(fmtMinP, invalid)
153: 	}
154: 
155: 	if cfg.MaximalPeriod <= cfg.MinimalPeriod {
156: 		return nil, fmt.Errorf(fmtMaxP, invalid)
157: 	}
158: 
159: 	bc := cfg.BarComponent
160: 	if bc == 0 {
161: 		bc = entities.BarMedianPrice
162: 	}
163: 
164: 	qc := cfg.QuoteComponent
165: 	if qc == 0 {
166: 		qc = entities.DefaultQuoteComponent
167: 	}
168: 
169: 	tc := cfg.TradeComponent
170: 	if tc == 0 {
171: 		tc = entities.DefaultTradeComponent
172: 	}
173: 
174: 	c, err := corona.NewCorona(&corona.Params{
175: 		HighPassFilterCutoff: cfg.HighPassFilterCutoff,
176: 		MinimalPeriod:        cfg.MinimalPeriod,
177: 		MaximalPeriod:        cfg.MaximalPeriod,
178: 	})
179: 	if err != nil {
180: 		return nil, fmt.Errorf(fmtw, invalid, err)
181: 	}
182: 
183: 	barFunc, err := entities.BarComponentFunc(bc)
184: 	if err != nil {
185: 		return nil, fmt.Errorf(fmtw, invalid, err)
186: 	}
187: 
188: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
189: 	if err != nil {
190: 		return nil, fmt.Errorf(fmtw, invalid, err)
191: 	}
192: 
193: 	tradeFunc, err := entities.TradeComponentFunc(tc)
194: 	if err != nil {
195: 		return nil, fmt.Errorf(fmtw, invalid, err)
196: 	}
197: 
198: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
199: 
200: 	mnemonicVal := fmt.Sprintf(fmtnVal,
201: 		cfg.RasterLength, cfg.MaxRasterValue, cfg.MinParameterValue, cfg.MaxParameterValue,
202: 		cfg.HighPassFilterCutoff, componentMnemonic)
203: 	mnemonicSP := fmt.Sprintf(fmtnSP, cfg.HighPassFilterCutoff, componentMnemonic)
204: 
205: 	paramResolution := float64(cfg.RasterLength-1) / (cfg.MaxParameterValue - cfg.MinParameterValue)
206: 
207: 	return &CoronaSwingPosition{
208: 		mnemonic:            mnemonicVal,
209: 		description:         descrVal + mnemonicVal,
210: 		mnemonicSP:          mnemonicSP,
211: 		descriptionSP:       descrSP + mnemonicSP,
212: 		c:                   c,
213: 		rasterLength:        cfg.RasterLength,
214: 		rasterStep:          cfg.MaxRasterValue / float64(cfg.RasterLength),
215: 		maxRasterValue:      cfg.MaxRasterValue,
216: 		minParameterValue:   cfg.MinParameterValue,
217: 		maxParameterValue:   cfg.MaxParameterValue,
218: 		parameterResolution: paramResolution,
219: 		raster:              make([]float64, cfg.RasterLength),
220: 		leadList:            make([]float64, 0, maxLeadListCount),
221: 		positionList:        make([]float64, 0, maxPositionListCount),
222: 		swingPosition:       math.NaN(),
223: 		barFunc:             barFunc,
224: 		quoteFunc:           quoteFunc,
225: 		tradeFunc:           tradeFunc,
226: 	}, nil
227: }
228: 
229: // IsPrimed indicates whether the indicator is primed.
230: func (s *CoronaSwingPosition) IsPrimed() bool {
231: 	s.mu.RLock()
232: 	defer s.mu.RUnlock()
233: 
234: 	return s.c.IsPrimed()
235: }
236: 
237: // Metadata describes the output data of the indicator.
238: func (s *CoronaSwingPosition) Metadata() core.Metadata {
239: 	return core.Metadata{
240: 		Type:        core.CoronaSwingPosition,
241: 		Mnemonic:    s.mnemonic,
242: 		Description: s.description,
243: 		Outputs: []outputs.Metadata{
244: 			{Kind: int(Value), Type: outputs.HeatmapType, Mnemonic: s.mnemonic, Description: s.description},
245: 			{Kind: int(SwingPosition), Type: outputs.ScalarType, Mnemonic: s.mnemonicSP, Description: s.descriptionSP},
246: 		},
247: 	}
248: }
249: 
250: // Update feeds the next sample and returns the heatmap column plus the current
251: // SwingPosition. On unprimed bars the heatmap is empty and the scalar is NaN.
252: //
253: //nolint:funlen,cyclop,gocognit
254: func (s *CoronaSwingPosition) Update(sample float64, t time.Time) (*outputs.Heatmap, float64) {
255: 	s.mu.Lock()
256: 	defer s.mu.Unlock()
257: 
258: 	if math.IsNaN(sample) {
259: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
260: 	}
261: 
262: 	primed := s.c.Update(sample)
263: 
264: 	if !s.isStarted {
265: 		s.samplePrev = sample
266: 		s.isStarted = true
267: 
268: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
269: 	}
270: 
271: 	// Bandpass InPhase filter at the dominant cycle median period.
272: 	omega := 2. * math.Pi / s.c.DominantCycleMedian()
273: 	beta2 := math.Cos(omega)
274: 	gamma2 := 1. / math.Cos(omega*2*bpDelta)
275: 	alpha2 := gamma2 - math.Sqrt(gamma2*gamma2-1.)
276: 	bandPass := 0.5*(1-alpha2)*(sample-s.samplePrev2) +
277: 		beta2*(1+alpha2)*s.bandPassPrev -
278: 		alpha2*s.bandPassPrev2
279: 
280: 	// Quadrature = derivative / omega.
281: 	quadrature2 := (bandPass - s.bandPassPrev) / omega
282: 
283: 	s.bandPassPrev2 = s.bandPassPrev
284: 	s.bandPassPrev = bandPass
285: 	s.samplePrev2 = s.samplePrev
286: 	s.samplePrev = sample
287: 
288: 	// 60° lead: lead60 = 0.5·BP_prev2 + 0.866·Q
289: 	lead60 := lead60CoefBP*s.bandPassPrev2 + lead60CoefQ*quadrature2
290: 
291: 	lowest, highest := appendRolling(&s.leadList, maxLeadListCount, lead60)
292: 
293: 	// Normalised lead position ∈ [0, 1].
294: 	position := highest - lowest
295: 	if position > 0 {
296: 		position = (lead60 - lowest) / position
297: 	}
298: 
299: 	lowest, highest = appendRolling(&s.positionList, maxPositionListCount, position)
300: 	highest -= lowest
301: 
302: 	width := 0.15 * highest
303: 	if highest > widthHighThreshold {
304: 		width = widthNarrow
305: 	}
306: 
307: 	s.swingPosition = (s.maxParameterValue-s.minParameterValue)*position + s.minParameterValue
308: 
309: 	positionScaledToRasterLength := int(math.Round(position * float64(s.rasterLength)))
310: 	positionScaledToMaxRasterValue := position * s.maxRasterValue
311: 
312: 	for i := 0; i < s.rasterLength; i++ {
313: 		value := s.raster[i]
314: 
315: 		if i == positionScaledToRasterLength {
316: 			value *= rasterBlendHalf
317: 		} else {
318: 			argument := positionScaledToMaxRasterValue - s.rasterStep*float64(i)
319: 			if i > positionScaledToRasterLength {
320: 				argument = -argument
321: 			}
322: 
323: 			if width > 0 {
324: 				value = rasterBlendHalf *
325: 					(math.Pow(argument/width, rasterBlendExponent) + rasterBlendHalf*value)
326: 			}
327: 		}
328: 
329: 		if value < 0 {
330: 			value = 0
331: 		} else if value > s.maxRasterValue {
332: 			value = s.maxRasterValue
333: 		}
334: 
335: 		if highest > widthHighSaturate {
336: 			value = s.maxRasterValue
337: 		}
338: 
339: 		if math.IsNaN(value) {
340: 			value = 0
341: 		}
342: 
343: 		s.raster[i] = value
344: 	}
345: 
346: 	if !primed {
347: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
348: 	}
349: 
350: 	values := make([]float64, s.rasterLength)
351: 	valueMin := math.Inf(1)
352: 	valueMax := math.Inf(-1)
353: 
354: 	for i := 0; i < s.rasterLength; i++ {
355: 		v := s.raster[i]
356: 		values[i] = v
357: 
358: 		if v < valueMin {
359: 			valueMin = v
360: 		}
361: 
362: 		if v > valueMax {
363: 			valueMax = v
364: 		}
365: 	}
366: 
367: 	heatmap := outputs.NewHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution,
368: 		valueMin, valueMax, values)
369: 
370: 	return heatmap, s.swingPosition
371: }
372: 
373: // UpdateScalar updates the indicator given the next scalar sample.
374: func (s *CoronaSwingPosition) UpdateScalar(sample *entities.Scalar) core.Output {
375: 	return s.updateEntity(sample.Time, sample.Value)
376: }
377: 
378: // UpdateBar updates the indicator given the next bar sample.
379: func (s *CoronaSwingPosition) UpdateBar(sample *entities.Bar) core.Output {
380: 	return s.updateEntity(sample.Time, s.barFunc(sample))
381: }
382: 
383: // UpdateQuote updates the indicator given the next quote sample.
384: func (s *CoronaSwingPosition) UpdateQuote(sample *entities.Quote) core.Output {
385: 	return s.updateEntity(sample.Time, s.quoteFunc(sample))
386: }
387: 
388: // UpdateTrade updates the indicator given the next trade sample.
389: func (s *CoronaSwingPosition) UpdateTrade(sample *entities.Trade) core.Output {
390: 	return s.updateEntity(sample.Time, s.tradeFunc(sample))
391: }
392: 
393: func (s *CoronaSwingPosition) updateEntity(t time.Time, sample float64) core.Output {
394: 	const length = 2
395: 
396: 	heatmap, sp := s.Update(sample, t)
397: 
398: 	output := make([]any, length)
399: 	output[0] = heatmap
400: 	output[1] = entities.Scalar{Time: t, Value: sp}
401: 
402: 	return output
403: }
404: 
405: // appendRolling appends v to the list, drops the oldest element once the list
406: // reaches maxCount, and returns the current (lowest, highest) values.
407: func appendRolling(list *[]float64, maxCount int, v float64) (float64, float64) {
408: 	if len(*list) >= maxCount {
409: 		*list = (*list)[1:]
410: 	}
411: 
412: 	*list = append(*list, v)
413: 
414: 	lowest := v
415: 	highest := v
416: 
417: 	for _, x := range *list {
418: 		if x < lowest {
419: 			lowest = x
420: 		}
421: 
422: 		if x > highest {
423: 			highest = x
424: 		}
425: 	}
426: 
427: 	return lowest, highest
428: }

(End of file - total 428 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.8s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor.go</path>
<type>file</type>
<content>1: // Package coronatrendvigor implements Ehlers' Corona Trend Vigor heatmap
2: // indicator.
3: //
4: // The Corona trend vigor is computed as the slope of the momentum taken over a
5: // full dominant cycle period, normalised by the cycle amplitude. The ratio is
6: // scaled into the range [-10, 10]. A value of ±2 means the trend slope equals
7: // twice the cycle amplitude; values between -2 and +2 form the "corona" and
8: // suggest the trend should not be traded.
9: //
10: // Reference: John Ehlers, "Measuring Cycle Periods", Stocks & Commodities,
11: // November 2008.
12: package coronatrendvigor
13: 
14: //nolint: gofumpt
15: import (
16: 	"fmt"
17: 	"math"
18: 	"sync"
19: 	"time"
20: 
21: 	"zpano/entities"
22: 	"zpano/indicators/core"
23: 	"zpano/indicators/core/outputs"
24: 	"zpano/indicators/johnehlers/corona"
25: )
26: 
27: const (
28: 	// Bandpass filter delta factor in γ = 1/cos(ω · 2 · 0.1).
29: 	bpDelta = 0.1
30: 
31: 	// Ratio EMA coefficients (0.33 * current + 0.67 * previous).
32: 	ratioNewCoef  = 0.33
33: 	ratioPrevCoef = 0.67
34: 
35: 	// Vigor band thresholds.
36: 	vigorMidLow  = 0.3
37: 	vigorMidHigh = 0.7
38: 	vigorMid     = 0.5
39: 	widthEdge    = 0.01
40: 
41: 	// Raster update blend factors.
42: 	rasterBlendScale    = 0.8
43: 	rasterBlendPrevious = 0.2
44: 	rasterBlendHalf     = 0.5
45: 	rasterBlendExponent = 0.85
46: 
47: 	// Ratio clamp bounds.
48: 	ratioLimit = 10.0
49: 
50: 	// vigor = vigorScale*(ratio+ratioLimit); with ratioLimit=10 and range 20,
51: 	// vigorScale=0.05 maps ratio ∈ [-10,10] to vigor ∈ [0,1].
52: 	vigorScale = 0.05
53: )
54: 
55: // CoronaTrendVigor is Ehlers' Corona Trend Vigor indicator.
56: type CoronaTrendVigor struct {
57: 	mu                  sync.RWMutex
58: 	mnemonic            string
59: 	description         string
60: 	mnemonicTV          string
61: 	descriptionTV       string
62: 	c                   *corona.Corona
63: 	rasterLength        int
64: 	rasterStep          float64
65: 	maxRasterValue      float64
66: 	minParameterValue   float64
67: 	maxParameterValue   float64
68: 	parameterResolution float64
69: 	raster              []float64
70: 	sampleBuffer        []float64
71: 	sampleCount         int
72: 	samplePrev          float64
73: 	samplePrev2         float64
74: 	bandPassPrev        float64
75: 	bandPassPrev2       float64
76: 	ratioPrev           float64
77: 	trendVigor          float64
78: 	barFunc             entities.BarFunc
79: 	quoteFunc           entities.QuoteFunc
80: 	tradeFunc           entities.TradeFunc
81: }
82: 
83: // NewCoronaTrendVigorDefault returns an instance created with default parameters.
84: func NewCoronaTrendVigorDefault() (*CoronaTrendVigor, error) {
85: 	return NewCoronaTrendVigorParams(&Params{})
86: }
87: 
88: // NewCoronaTrendVigorParams returns an instance created with the supplied parameters.
89: //
90: //nolint:funlen,cyclop
91: func NewCoronaTrendVigorParams(p *Params) (*CoronaTrendVigor, error) {
92: 	const (
93: 		invalid      = "invalid corona trend vigor parameters"
94: 		fmtRaster    = "%s: RasterLength should be >= 2"
95: 		fmtMaxRaster = "%s: MaxRasterValue should be > 0"
96: 		fmtMaxParam  = "%s: MaxParameterValue should be > MinParameterValue"
97: 		fmtHP        = "%s: HighPassFilterCutoff should be >= 2"
98: 		fmtMinP      = "%s: MinimalPeriod should be >= 2"
99: 		fmtMaxP      = "%s: MaximalPeriod should be > MinimalPeriod"
100: 		fmtw         = "%s: %w"
101: 		fmtnVal      = "ctv(%d, %g, %g, %g, %d%s)"
102: 		fmtnTV       = "ctv-tv(%d%s)"
103: 		descrVal     = "Corona trend vigor "
104: 		descrTV      = "Corona trend vigor scalar "
105: 		defRaster    = 50
106: 		defMaxRast   = 20.0
107: 		defMinParam  = -10.0
108: 		defMaxParam  = 10.0
109: 		defHPCutoff  = 30
110: 		defMinPer    = 6
111: 		defMaxPer    = 30
112: 	)
113: 
114: 	cfg := *p
115: 
116: 	if cfg.RasterLength == 0 {
117: 		cfg.RasterLength = defRaster
118: 	}
119: 
120: 	if cfg.MaxRasterValue == 0 {
121: 		cfg.MaxRasterValue = defMaxRast
122: 	}
123: 
124: 	// MinParameterValue and MaxParameterValue default to -10 and 10; since 0 is a
125: 	// valid user value for either, we only substitute when both are zero (the
126: 	// unconfigured case).
127: 	if cfg.MinParameterValue == 0 && cfg.MaxParameterValue == 0 {
128: 		cfg.MinParameterValue = defMinParam
129: 		cfg.MaxParameterValue = defMaxParam
130: 	}
131: 
132: 	if cfg.HighPassFilterCutoff == 0 {
133: 		cfg.HighPassFilterCutoff = defHPCutoff
134: 	}
135: 
136: 	if cfg.MinimalPeriod == 0 {
137: 		cfg.MinimalPeriod = defMinPer
138: 	}
139: 
140: 	if cfg.MaximalPeriod == 0 {
141: 		cfg.MaximalPeriod = defMaxPer
142: 	}
143: 
144: 	if cfg.RasterLength < 2 {
145: 		return nil, fmt.Errorf(fmtRaster, invalid)
146: 	}
147: 
148: 	if cfg.MaxRasterValue <= 0 {
149: 		return nil, fmt.Errorf(fmtMaxRaster, invalid)
150: 	}
151: 
152: 	if cfg.MaxParameterValue <= cfg.MinParameterValue {
153: 		return nil, fmt.Errorf(fmtMaxParam, invalid)
154: 	}
155: 
156: 	if cfg.HighPassFilterCutoff < 2 {
157: 		return nil, fmt.Errorf(fmtHP, invalid)
158: 	}
159: 
160: 	if cfg.MinimalPeriod < 2 {
161: 		return nil, fmt.Errorf(fmtMinP, invalid)
162: 	}
163: 
164: 	if cfg.MaximalPeriod <= cfg.MinimalPeriod {
165: 		return nil, fmt.Errorf(fmtMaxP, invalid)
166: 	}
167: 
168: 	bc := cfg.BarComponent
169: 	if bc == 0 {
170: 		bc = entities.BarMedianPrice
171: 	}
172: 
173: 	qc := cfg.QuoteComponent
174: 	if qc == 0 {
175: 		qc = entities.DefaultQuoteComponent
176: 	}
177: 
178: 	tc := cfg.TradeComponent
179: 	if tc == 0 {
180: 		tc = entities.DefaultTradeComponent
181: 	}
182: 
183: 	c, err := corona.NewCorona(&corona.Params{
184: 		HighPassFilterCutoff: cfg.HighPassFilterCutoff,
185: 		MinimalPeriod:        cfg.MinimalPeriod,
186: 		MaximalPeriod:        cfg.MaximalPeriod,
187: 	})
188: 	if err != nil {
189: 		return nil, fmt.Errorf(fmtw, invalid, err)
190: 	}
191: 
192: 	barFunc, err := entities.BarComponentFunc(bc)
193: 	if err != nil {
194: 		return nil, fmt.Errorf(fmtw, invalid, err)
195: 	}
196: 
197: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
198: 	if err != nil {
199: 		return nil, fmt.Errorf(fmtw, invalid, err)
200: 	}
201: 
202: 	tradeFunc, err := entities.TradeComponentFunc(tc)
203: 	if err != nil {
204: 		return nil, fmt.Errorf(fmtw, invalid, err)
205: 	}
206: 
207: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
208: 
209: 	mnemonicVal := fmt.Sprintf(fmtnVal,
210: 		cfg.RasterLength, cfg.MaxRasterValue, cfg.MinParameterValue, cfg.MaxParameterValue,
211: 		cfg.HighPassFilterCutoff, componentMnemonic)
212: 	mnemonicTV := fmt.Sprintf(fmtnTV, cfg.HighPassFilterCutoff, componentMnemonic)
213: 
214: 	paramResolution := float64(cfg.RasterLength-1) / (cfg.MaxParameterValue - cfg.MinParameterValue)
215: 
216: 	return &CoronaTrendVigor{
217: 		mnemonic:            mnemonicVal,
218: 		description:         descrVal + mnemonicVal,
219: 		mnemonicTV:          mnemonicTV,
220: 		descriptionTV:       descrTV + mnemonicTV,
221: 		c:                   c,
222: 		rasterLength:        cfg.RasterLength,
223: 		rasterStep:          cfg.MaxRasterValue / float64(cfg.RasterLength),
224: 		maxRasterValue:      cfg.MaxRasterValue,
225: 		minParameterValue:   cfg.MinParameterValue,
226: 		maxParameterValue:   cfg.MaxParameterValue,
227: 		parameterResolution: paramResolution,
228: 		raster:              make([]float64, cfg.RasterLength),
229: 		sampleBuffer:        make([]float64, c.MaximalPeriodTimesTwo()),
230: 		trendVigor:          math.NaN(),
231: 		barFunc:             barFunc,
232: 		quoteFunc:           quoteFunc,
233: 		tradeFunc:           tradeFunc,
234: 	}, nil
235: }
236: 
237: // IsPrimed indicates whether the indicator is primed.
238: func (s *CoronaTrendVigor) IsPrimed() bool {
239: 	s.mu.RLock()
240: 	defer s.mu.RUnlock()
241: 
242: 	return s.c.IsPrimed()
243: }
244: 
245: // Metadata describes the output data of the indicator.
246: func (s *CoronaTrendVigor) Metadata() core.Metadata {
247: 	return core.Metadata{
248: 		Type:        core.CoronaTrendVigor,
249: 		Mnemonic:    s.mnemonic,
250: 		Description: s.description,
251: 		Outputs: []outputs.Metadata{
252: 			{Kind: int(Value), Type: outputs.HeatmapType, Mnemonic: s.mnemonic, Description: s.description},
253: 			{Kind: int(TrendVigor), Type: outputs.ScalarType, Mnemonic: s.mnemonicTV, Description: s.descriptionTV},
254: 		},
255: 	}
256: }
257: 
258: // Update feeds the next sample and returns the heatmap column plus the current
259: // TrendVigor. On unprimed bars the heatmap is empty and the scalar is NaN.
260: //
261: //nolint:funlen,cyclop,gocognit
262: func (s *CoronaTrendVigor) Update(sample float64, t time.Time) (*outputs.Heatmap, float64) {
263: 	s.mu.Lock()
264: 	defer s.mu.Unlock()
265: 
266: 	if math.IsNaN(sample) {
267: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
268: 	}
269: 
270: 	primed := s.c.Update(sample)
271: 	s.sampleCount++
272: 
273: 	bufLast := len(s.sampleBuffer) - 1
274: 
275: 	if s.sampleCount == 1 {
276: 		s.samplePrev = sample
277: 		s.sampleBuffer[bufLast] = sample
278: 
279: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
280: 	}
281: 
282: 	// Bandpass InPhase filter at the dominant cycle median period.
283: 	omega := 2. * math.Pi / s.c.DominantCycleMedian()
284: 	beta2 := math.Cos(omega)
285: 	gamma2 := 1. / math.Cos(omega*2*bpDelta)
286: 	alpha2 := gamma2 - math.Sqrt(gamma2*gamma2-1.)
287: 	bandPass := 0.5*(1-alpha2)*(sample-s.samplePrev2) +
288: 		beta2*(1+alpha2)*s.bandPassPrev -
289: 		alpha2*s.bandPassPrev2
290: 
291: 	// Quadrature = derivative / omega.
292: 	quadrature2 := (bandPass - s.bandPassPrev) / omega
293: 
294: 	s.bandPassPrev2 = s.bandPassPrev
295: 	s.bandPassPrev = bandPass
296: 	s.samplePrev2 = s.samplePrev
297: 	s.samplePrev = sample
298: 
299: 	// Left-shift sampleBuffer and append the new sample.
300: 	for i := 0; i < bufLast; i++ {
301: 		s.sampleBuffer[i] = s.sampleBuffer[i+1]
302: 	}
303: 
304: 	s.sampleBuffer[bufLast] = sample
305: 
306: 	// Cycle amplitude.
307: 	amplitude2 := math.Sqrt(bandPass*bandPass + quadrature2*quadrature2)
308: 
309: 	// Trend amplitude taken over the cycle period. Use DominantCycleMedian-1
310: 	// directly (the MBST implementation clamps to sampleBuffer length which
311: 	// negates the intent; see the reference impls).
312: 	cyclePeriod := int(s.c.DominantCycleMedian() - 1)
313: 	if cyclePeriod > len(s.sampleBuffer) {
314: 		cyclePeriod = len(s.sampleBuffer)
315: 	}
316: 
317: 	if cyclePeriod < 1 {
318: 		cyclePeriod = 1
319: 	}
320: 
321: 	lookback := cyclePeriod
322: 	if s.sampleCount < lookback {
323: 		lookback = s.sampleCount
324: 	}
325: 
326: 	trend := sample - s.sampleBuffer[len(s.sampleBuffer)-lookback]
327: 
328: 	ratio := 0.0
329: 	if math.Abs(trend) > 0 && amplitude2 > 0 {
330: 		ratio = ratioNewCoef*trend/amplitude2 + ratioPrevCoef*s.ratioPrev
331: 	}
332: 
333: 	if ratio > ratioLimit {
334: 		ratio = ratioLimit
335: 	} else if ratio < -ratioLimit {
336: 		ratio = -ratioLimit
337: 	}
338: 
339: 	s.ratioPrev = ratio
340: 
341: 	// ratio ∈ [-10, 10] ⇒ vigor ∈ [0, 1].
342: 	vigor := vigorScale * (ratio + ratioLimit)
343: 
344: 	var width float64
345: 
346: 	switch {
347: 	case vigor >= vigorMidLow && vigor < vigorMid:
348: 		width = vigor - (vigorMidLow - widthEdge)
349: 	case vigor >= vigorMid && vigor <= vigorMidHigh:
350: 		width = (vigorMidHigh + widthEdge) - vigor
351: 	default:
352: 		width = widthEdge
353: 	}
354: 
355: 	s.trendVigor = (s.maxParameterValue-s.minParameterValue)*vigor + s.minParameterValue
356: 
357: 	vigorScaledToRasterLength := int(math.Round(float64(s.rasterLength) * vigor))
358: 	vigorScaledToMaxRasterValue := vigor * s.maxRasterValue
359: 
360: 	for i := 0; i < s.rasterLength; i++ {
361: 		value := s.raster[i]
362: 
363: 		if i == vigorScaledToRasterLength {
364: 			value *= rasterBlendHalf
365: 		} else {
366: 			argument := vigorScaledToMaxRasterValue - s.rasterStep*float64(i)
367: 			if i > vigorScaledToRasterLength {
368: 				argument = -argument
369: 			}
370: 
371: 			if width > 0 {
372: 				value = rasterBlendScale *
373: 					(math.Pow(argument/width, rasterBlendExponent) + rasterBlendPrevious*value)
374: 			}
375: 		}
376: 
377: 		switch {
378: 		case value < 0:
379: 			value = 0
380: 		case value > s.maxRasterValue, vigor < vigorMidLow, vigor > vigorMidHigh:
381: 			value = s.maxRasterValue
382: 		}
383: 
384: 		if math.IsNaN(value) {
385: 			value = 0
386: 		}
387: 
388: 		s.raster[i] = value
389: 	}
390: 
391: 	if !primed {
392: 		return outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()
393: 	}
394: 
395: 	values := make([]float64, s.rasterLength)
396: 	valueMin := math.Inf(1)
397: 	valueMax := math.Inf(-1)
398: 
399: 	for i := 0; i < s.rasterLength; i++ {
400: 		v := s.raster[i]
401: 		values[i] = v
402: 
403: 		if v < valueMin {
404: 			valueMin = v
405: 		}
406: 
407: 		if v > valueMax {
408: 			valueMax = v
409: 		}
410: 	}
411: 
412: 	heatmap := outputs.NewHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution,
413: 		valueMin, valueMax, values)
414: 
415: 	return heatmap, s.trendVigor
416: }
417: 
418: // UpdateScalar updates the indicator given the next scalar sample.
419: func (s *CoronaTrendVigor) UpdateScalar(sample *entities.Scalar) core.Output {
420: 	return s.updateEntity(sample.Time, sample.Value)
421: }
422: 
423: // UpdateBar updates the indicator given the next bar sample.
424: func (s *CoronaTrendVigor) UpdateBar(sample *entities.Bar) core.Output {
425: 	return s.updateEntity(sample.Time, s.barFunc(sample))
426: }
427: 
428: // UpdateQuote updates the indicator given the next quote sample.
429: func (s *CoronaTrendVigor) UpdateQuote(sample *entities.Quote) core.Output {
430: 	return s.updateEntity(sample.Time, s.quoteFunc(sample))
431: }
432: 
433: // UpdateTrade updates the indicator given the next trade sample.
434: func (s *CoronaTrendVigor) UpdateTrade(sample *entities.Trade) core.Output {
435: 	return s.updateEntity(sample.Time, s.tradeFunc(sample))
436: }
437: 
438: func (s *CoronaTrendVigor) updateEntity(t time.Time, sample float64) core.Output {
439: 	const length = 2
440: 
441: 	heatmap, tv := s.Update(sample, t)
442: 
443: 	output := make([]any, length)
444: 	output[0] = heatmap
445: 	output[1] = entities.Scalar{Time: t, Value: tv}
446: 
447: 	return output
448: }

(End of file - total 448 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 18.4s)

_Thinking:_

 I see the Corona class does expose maximalPeriodTimesTwo as a public property, so that's available. Now I'm setting up CoronaSwingPosition with the resolution parameter calculated as 4.9 based on the range from -5 to 50.

**Tool: write**

**Input:**
```json
{
  "content": "/** Enumerates outputs of the CoronaSwingPosition indicator. */\nexport enum CoronaSwingPositionOutput {\n  /** The Corona swing position heatmap column. */\n  Value = 0,\n  /** The current swing position scalar, mapped to [MinParameterValue, MaxParameterValue]. */\n  SwingPosition = 1,\n}\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-swing-position/corona-swing-position-output.ts"
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

**Tool: write**

**Input:**
```json
{
  "content": "import { BarComponent } from '../../../entities/bar-component';\nimport { QuoteComponent } from '../../../entities/quote-component';\nimport { TradeComponent } from '../../../entities/trade-component';\n\n/** Describes parameters to create an instance of the CoronaSwingPosition indicator. */\nexport interface CoronaSwingPositionParams {\n  /** Length of the heatmap raster. Default 50. A zero value is treated as \"use default\". */\n  rasterLength?: number;\n\n  /** Maximum raster intensity value. Default 20. A zero value is treated as \"use default\". */\n  maxRasterValue?: number;\n\n  /** Minimum ordinate (y) value of the heatmap — lower bound of the mapped swing position.\n   *  Default -5. Only substituted when both Min and Max are 0 (unconfigured). */\n  minParameterValue?: number;\n\n  /** Maximum ordinate (y) value of the heatmap — upper bound of the mapped swing position.\n   *  Default 5. Only substituted when both Min and Max are 0 (unconfigured). */\n  maxParameterValue?: number;\n\n  /** High-pass filter cutoff. Default 30. A zero value is treated as \"use default\". */\n  highPassFilterCutoff?: number;\n\n  /** Minimal cycle period. Default 6. A zero value is treated as \"use default\". */\n  minimalPeriod?: number;\n\n  /** Maximal cycle period. Default 30. A zero value is treated as \"use default\". */\n  maximalPeriod?: number;\n\n  /** A component of a bar to use when updating the indicator with a bar sample.\n   *  Default BarComponent.Median (hl/2). */\n  barComponent?: BarComponent;\n\n  /** A component of a quote to use when updating the indicator with a quote sample. */\n  quoteComponent?: QuoteComponent;\n\n  /** A component of a trade to use when updating the indicator with a trade sample. */\n  tradeComponent?: TradeComponent;\n}\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-swing-position/corona-swing-position-params.ts"
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

## Assistant (Build · claude-opus-4.7 · 83.4s)

**Tool: write**

**Input:**
```json
{
  "content": "import { Bar } from '../../../entities/bar';\nimport { BarComponent, barComponentValue } from '../../../entities/bar-component';\nimport { Quote } from '../../../entities/quote';\nimport { DefaultQuoteComponent, quoteComponentValue } from '../../../entities/quote-component';\nimport { Scalar } from '../../../entities/scalar';\nimport { Trade } from '../../../entities/trade';\nimport { DefaultTradeComponent, tradeComponentValue } from '../../../entities/trade-component';\nimport { componentTripleMnemonic } from '../../core/component-triple-mnemonic';\nimport { Indicator } from '../../core/indicator';\nimport { IndicatorMetadata } from '../../core/indicator-metadata';\nimport { IndicatorOutput } from '../../core/indicator-output';\nimport { IndicatorType } from '../../core/indicator-type';\nimport { Heatmap } from '../../core/outputs/heatmap';\nimport { OutputType } from '../../core/outputs/output-type';\nimport { Corona } from '../corona/corona';\nimport { CoronaSwingPositionOutput } from './corona-swing-position-output';\nimport { CoronaSwingPositionParams } from './corona-swing-position-params';\n\nconst DEFAULT_RASTER_LENGTH = 50;\nconst DEFAULT_MAX_RASTER = 20;\nconst DEFAULT_MIN_PARAM = -5;\nconst DEFAULT_MAX_PARAM = 5;\nconst DEFAULT_HP_CUTOFF = 30;\nconst DEFAULT_MIN_PERIOD = 6;\nconst DEFAULT_MAX_PERIOD = 30;\n\nconst MAX_LEAD_LIST_COUNT = 50;\nconst MAX_POSITION_LIST_COUNT = 20;\n\n// 60° phase-lead coefficients.\nconst LEAD60_COEF_BP = 0.5;\nconst LEAD60_COEF_Q = 0.866;\n\nconst BP_DELTA = 0.1;\n\nconst WIDTH_HIGH_THRESHOLD = 0.85;\nconst WIDTH_HIGH_SATURATE = 0.8;\nconst WIDTH_NARROW = 0.01;\n\nconst RASTER_BLEND_EXPONENT = 0.95;\nconst RASTER_BLEND_HALF = 0.5;\n\n/** __Corona Swing Position__ (Ehlers) heatmap indicator.\n *\n * Correlates prices with a perfect sine wave having the dominant cycle period, producing a\n * smooth waveform that lets us better estimate the swing position and impending turning points.\n *\n * Reference: John Ehlers, \"Measuring Cycle Periods\", Stocks & Commodities, November 2008.\n */\nexport class CoronaSwingPosition implements Indicator {\n  private readonly c: Corona;\n  private readonly rasterLength: number;\n  private readonly rasterStep: number;\n  private readonly maxRasterValue: number;\n  private readonly minParameterValue: number;\n  private readonly maxParameterValue: number;\n  private readonly parameterResolution: number;\n  private readonly raster: number[];\n  private readonly leadList: number[] = [];\n  private readonly positionList: number[] = [];\n\n  private readonly mnemonicValue: string;\n  private readonly descriptionValue: string;\n  private readonly mnemonicSP: string;\n  private readonly descriptionSP: string;\n\n  private readonly barComponentFunc: (bar: Bar) => number;\n  private readonly quoteComponentFunc: (quote: Quote) => number;\n  private readonly tradeComponentFunc: (trade: Trade) => number;\n\n  private samplePrev = 0;\n  private samplePrev2 = 0;\n  private bandPassPrev = 0;\n  private bandPassPrev2 = 0;\n  private swingPosition = Number.NaN;\n  private isStarted = false;\n\n  public static default(): CoronaSwingPosition {\n    return new CoronaSwingPosition({});\n  }\n\n  public static fromParams(params: CoronaSwingPositionParams): CoronaSwingPosition {\n    return new CoronaSwingPosition(params);\n  }\n\n  private constructor(params: CoronaSwingPositionParams) {\n    const invalid = 'invalid corona swing position parameters';\n\n    const rasterLength = params.rasterLength !== undefined && params.rasterLength !== 0\n      ? params.rasterLength : DEFAULT_RASTER_LENGTH;\n    const maxRaster = params.maxRasterValue !== undefined && params.maxRasterValue !== 0\n      ? params.maxRasterValue : DEFAULT_MAX_RASTER;\n\n    // Only substitute Min/Max when both are 0 (unconfigured), since 0 is a valid user value.\n    let minParam = params.minParameterValue ?? 0;\n    let maxParam = params.maxParameterValue ?? 0;\n    if (minParam === 0 && maxParam === 0) {\n      minParam = DEFAULT_MIN_PARAM;\n      maxParam = DEFAULT_MAX_PARAM;\n    }\n\n    const hpCutoff = params.highPassFilterCutoff !== undefined && params.highPassFilterCutoff !== 0\n      ? params.highPassFilterCutoff : DEFAULT_HP_CUTOFF;\n    const minPeriod = params.minimalPeriod !== undefined && params.minimalPeriod !== 0\n      ? params.minimalPeriod : DEFAULT_MIN_PERIOD;\n    const maxPeriod = params.maximalPeriod !== undefined && params.maximalPeriod !== 0\n      ? params.maximalPeriod : DEFAULT_MAX_PERIOD;\n\n    if (rasterLength < 2) throw new Error(`${invalid}: RasterLength should be >= 2`);\n    if (maxRaster <= 0) throw new Error(`${invalid}: MaxRasterValue should be > 0`);\n    if (maxParam <= minParam) throw new Error(`${invalid}: MaxParameterValue should be > MinParameterValue`);\n    if (hpCutoff < 2) throw new Error(`${invalid}: HighPassFilterCutoff should be >= 2`);\n    if (minPeriod < 2) throw new Error(`${invalid}: MinimalPeriod should be >= 2`);\n    if (maxPeriod <= minPeriod) throw new Error(`${invalid}: MaximalPeriod should be > MinimalPeriod`);\n\n    const bc = params.barComponent ?? BarComponent.Median;\n    const qc = params.quoteComponent ?? DefaultQuoteComponent;\n    const tc = params.tradeComponent ?? DefaultTradeComponent;\n\n    this.barComponentFunc = barComponentValue(bc);\n    this.quoteComponentFunc = quoteComponentValue(qc);\n    this.tradeComponentFunc = tradeComponentValue(tc);\n\n    this.c = new Corona({\n      highPassFilterCutoff: hpCutoff,\n      minimalPeriod: minPeriod,\n      maximalPeriod: maxPeriod,\n    });\n\n    this.rasterLength = rasterLength;\n    this.rasterStep = maxRaster / rasterLength;\n    this.maxRasterValue = maxRaster;\n    this.minParameterValue = minParam;\n    this.maxParameterValue = maxParam;\n    this.parameterResolution = (rasterLength - 1) / (maxParam - minParam);\n    this.raster = new Array<number>(rasterLength).fill(0);\n\n    const cm = componentTripleMnemonic(bc, qc, tc);\n    this.mnemonicValue = `cswing(${rasterLength}, ${maxRaster}, ${minParam}, ${maxParam}, ${hpCutoff}${cm})`;\n    this.mnemonicSP = `cswing-sp(${hpCutoff}${cm})`;\n    this.descriptionValue = 'Corona swing position ' + this.mnemonicValue;\n    this.descriptionSP = 'Corona swing position scalar ' + this.mnemonicSP;\n  }\n\n  public isPrimed(): boolean { return this.c.isPrimed(); }\n\n  public metadata(): IndicatorMetadata {\n    return {\n      type: IndicatorType.CoronaSwingPosition,\n      mnemonic: this.mnemonicValue,\n      description: this.descriptionValue,\n      outputs: [\n        { kind: CoronaSwingPositionOutput.Value, type: OutputType.Heatmap, mnemonic: this.mnemonicValue, description: this.descriptionValue },\n        { kind: CoronaSwingPositionOutput.SwingPosition, type: OutputType.Scalar, mnemonic: this.mnemonicSP, description: this.descriptionSP },\n      ],\n    };\n  }\n\n  /** Feeds the next sample and returns the heatmap column plus the current SwingPosition. */\n  public update(sample: number, time: Date): [Heatmap, number] {\n    if (Number.isNaN(sample)) {\n      return [\n        Heatmap.newEmptyHeatmap(time, this.minParameterValue, this.maxParameterValue, this.parameterResolution),\n        Number.NaN,\n      ];\n    }\n\n    const primed = this.c.update(sample);\n\n    if (!this.isStarted) {\n      this.samplePrev = sample;\n      this.isStarted = true;\n      return [\n        Heatmap.newEmptyHeatmap(time, this.minParameterValue, this.maxParameterValue, this.parameterResolution),\n        Number.NaN,\n      ];\n    }\n\n    // Bandpass InPhase filter at dominant cycle median period.\n    const omega = 2 * Math.PI / this.c.dominantCycleMedian;\n    const beta2 = Math.cos(omega);\n    const gamma2 = 1 / Math.cos(omega * 2 * BP_DELTA);\n    const alpha2 = gamma2 - Math.sqrt(gamma2 * gamma2 - 1);\n    const bandPass = 0.5 * (1 - alpha2) * (sample - this.samplePrev2)\n      + beta2 * (1 + alpha2) * this.bandPassPrev\n      - alpha2 * this.bandPassPrev2;\n\n    const quadrature2 = (bandPass - this.bandPassPrev) / omega;\n\n    this.bandPassPrev2 = this.bandPassPrev;\n    this.bandPassPrev = bandPass;\n    this.samplePrev2 = this.samplePrev;\n    this.samplePrev = sample;\n\n    // 60° lead.\n    const lead60 = LEAD60_COEF_BP * this.bandPassPrev2 + LEAD60_COEF_Q * quadrature2;\n\n    let [lowest, highest] = appendRolling(this.leadList, MAX_LEAD_LIST_COUNT, lead60);\n\n    let position = highest - lowest;\n    if (position > 0) {\n      position = (lead60 - lowest) / position;\n    }\n\n    [lowest, highest] = appendRolling(this.positionList, MAX_POSITION_LIST_COUNT, position);\n    highest -= lowest;\n\n    let width = 0.15 * highest;\n    if (highest > WIDTH_HIGH_THRESHOLD) {\n      width = WIDTH_NARROW;\n    }\n\n    this.swingPosition = (this.maxParameterValue - this.minParameterValue) * position + this.minParameterValue;\n\n    const positionScaledToRasterLength = Math.round(position * this.rasterLength);\n    const positionScaledToMaxRasterValue = position * this.maxRasterValue;\n\n    for (let i = 0; i < this.rasterLength; i++) {\n      let value = this.raster[i];\n\n      if (i === positionScaledToRasterLength) {\n        value *= RASTER_BLEND_HALF;\n      } else {\n        let argument = positionScaledToMaxRasterValue - this.rasterStep * i;\n        if (i > positionScaledToRasterLength) {\n          argument = -argument;\n        }\n        if (width > 0) {\n          value = RASTER_BLEND_HALF * (Math.pow(argument / width, RASTER_BLEND_EXPONENT) + RASTER_BLEND_HALF * value);\n        }\n      }\n\n      if (value < 0) {\n        value = 0;\n      } else if (value > this.maxRasterValue) {\n        value = this.maxRasterValue;\n      }\n\n      if (highest > WIDTH_HIGH_SATURATE) {\n        value = this.maxRasterValue;\n      }\n\n      if (Number.isNaN(value)) {\n        value = 0;\n      }\n\n      this.raster[i] = value;\n    }\n\n    if (!primed) {\n      return [\n        Heatmap.newEmptyHeatmap(time, this.minParameterValue, this.maxParameterValue, this.parameterResolution),\n        Number.NaN,\n      ];\n    }\n\n    const values = new Array<number>(this.rasterLength);\n    let valueMin = Number.POSITIVE_INFINITY;\n    let valueMax = Number.NEGATIVE_INFINITY;\n    for (let i = 0; i < this.rasterLength; i++) {\n      const v = this.raster[i];\n      values[i] = v;\n      if (v < valueMin) valueMin = v;\n      if (v > valueMax) valueMax = v;\n    }\n\n    const heatmap = Heatmap.newHeatmap(\n      time, this.minParameterValue, this.maxParameterValue, this.parameterResolution,\n      valueMin, valueMax, values,\n    );\n\n    return [heatmap, this.swingPosition];\n  }\n\n  public updateScalar(sample: Scalar): IndicatorOutput { return this.updateEntity(sample.time, sample.value); }\n  public updateBar(sample: Bar): IndicatorOutput { return this.updateEntity(sample.time, this.barComponentFunc(sample)); }\n  public updateQuote(sample: Quote): IndicatorOutput { return this.updateEntity(sample.time, this.quoteComponentFunc(sample)); }\n  public updateTrade(sample: Trade): IndicatorOutput { return this.updateEntity(sample.time, this.tradeComponentFunc(sample)); }\n\n  private updateEntity(time: Date, sample: number): IndicatorOutput {\n    const [heatmap, sp] = this.update(sample, time);\n    const s = new Scalar();\n    s.time = time;\n    s.value = sp;\n    return [heatmap, s];\n  }\n}\n\n/** Appends v to the list, drops the oldest once len reaches maxCount, returns [lowest, highest]. */\nfunction appendRolling(list: number[], maxCount: number, v: number): [number, number] {\n  if (list.length >= maxCount) {\n    list.shift();\n  }\n  list.push(v);\n\n  let lowest = v;\n  let highest = v;\n  for (let i = 0; i < list.length; i++) {\n    const x = list[i];\n    if (x < lowest) lowest = x;\n    if (x > highest) highest = x;\n  }\n  return [lowest, highest];\n}\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-swing-position/corona-swing-position.ts"
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

**Tool: write**

**Input:**
```json
{
  "content": "import { } from 'jasmine';\n\nimport { Bar } from '../../../entities/bar';\nimport { Quote } from '../../../entities/quote';\nimport { Scalar } from '../../../entities/scalar';\nimport { Trade } from '../../../entities/trade';\nimport { IndicatorType } from '../../core/indicator-type';\nimport { Heatmap } from '../../core/outputs/heatmap';\nimport { OutputType } from '../../core/outputs/output-type';\nimport { coronaTestInput } from '../corona/corona-test-input';\nimport { CoronaSwingPosition } from './corona-swing-position';\nimport { CoronaSwingPositionOutput } from './corona-swing-position-output';\n\nconst tolerance = 1e-4;\n\ndescribe('CoronaSwingPosition', () => {\n  const time = new Date(2021, 3, 1);\n\n  it('should have correct output enum values', () => {\n    expect(CoronaSwingPositionOutput.Value).toBe(0);\n    expect(CoronaSwingPositionOutput.SwingPosition).toBe(1);\n  });\n\n  it('should return expected mnemonic for default params', () => {\n    const x = CoronaSwingPosition.default();\n    expect(x.metadata().mnemonic).toBe('cswing(50, 20, -5, 5, 30, hl/2)');\n  });\n\n  it('should return expected metadata', () => {\n    const x = CoronaSwingPosition.default();\n    const meta = x.metadata();\n    const mn = 'cswing(50, 20, -5, 5, 30, hl/2)';\n    const mnSP = 'cswing-sp(30, hl/2)';\n\n    expect(meta.type).toBe(IndicatorType.CoronaSwingPosition);\n    expect(meta.mnemonic).toBe(mn);\n    expect(meta.description).toBe('Corona swing position ' + mn);\n    expect(meta.outputs.length).toBe(2);\n\n    expect(meta.outputs[0].kind).toBe(CoronaSwingPositionOutput.Value);\n    expect(meta.outputs[0].type).toBe(OutputType.Heatmap);\n    expect(meta.outputs[1].kind).toBe(CoronaSwingPositionOutput.SwingPosition);\n    expect(meta.outputs[1].type).toBe(OutputType.Scalar);\n    expect(meta.outputs[1].mnemonic).toBe(mnSP);\n  });\n\n  it('should throw for invalid params', () => {\n    expect(() => CoronaSwingPosition.fromParams({ rasterLength: 1 })).toThrowError(/RasterLength/);\n    expect(() => CoronaSwingPosition.fromParams({ minParameterValue: 5, maxParameterValue: 5 }))\n      .toThrowError(/MaxParameterValue/);\n    expect(() => CoronaSwingPosition.fromParams({ highPassFilterCutoff: 1 })).toThrowError(/HighPassFilterCutoff/);\n    expect(() => CoronaSwingPosition.fromParams({ minimalPeriod: 1 })).toThrowError(/MinimalPeriod/);\n  });\n\n  it('should return empty heatmap and NaN for NaN input', () => {\n    const x = CoronaSwingPosition.default();\n    const [h, sp] = x.update(Number.NaN, time);\n    expect(h.isEmpty()).toBe(true);\n    expect(Number.isNaN(sp)).toBe(true);\n    expect(x.isPrimed()).toBe(false);\n  });\n\n  it('should prime at sample index 11', () => {\n    const x = CoronaSwingPosition.default();\n    let primedAt = -1;\n    for (let i = 0; i < coronaTestInput.length; i++) {\n      x.update(coronaTestInput[i], new Date(time.getTime() + i * 60_000));\n      if (x.isPrimed() && primedAt < 0) primedAt = i;\n    }\n    expect(primedAt).toBe(11);\n  });\n\n  it('should match reference sp / vmin / vmax snapshots', () => {\n    const snapshots: Array<[number, number, number, number]> = [\n      [11, 5.0000000000, 20.0000000000, 20.0000000000],\n      [12, 5.0000000000, 20.0000000000, 20.0000000000],\n      [50, 4.5384908349, 20.0000000000, 20.0000000000],\n      [100, -3.8183742675, 3.4957777081, 20.0000000000],\n      [150, -1.8516194371, 5.3792287864, 20.0000000000],\n      [200, -3.6944428668, 4.2580825738, 20.0000000000],\n      [251, -0.8524812061, 4.4822539784, 20.0000000000],\n    ];\n\n    const x = CoronaSwingPosition.default();\n    let si = 0;\n    for (let i = 0; i < coronaTestInput.length; i++) {\n      const t = new Date(time.getTime() + i * 60_000);\n      const [h, sp] = x.update(coronaTestInput[i], t);\n\n      expect(h.parameterFirst).toBe(-5);\n      expect(h.parameterLast).toBe(5);\n      expect(Math.abs(h.parameterResolution - 4.9)).toBeLessThan(1e-9);\n\n      if (!x.isPrimed()) {\n        expect(h.isEmpty()).withContext(`bar ${i}`).toBe(true);\n        expect(Number.isNaN(sp)).toBe(true);\n        continue;\n      }\n\n      expect(h.values.length).toBe(50);\n\n      if (si < snapshots.length && snapshots[si][0] === i) {\n        expect(Math.abs(snapshots[si][1] - sp))\n          .withContext(`sp[${i}]: expected ${snapshots[si][1]}, actual ${sp}`).toBeLessThan(tolerance);\n        expect(Math.abs(snapshots[si][2] - h.valueMin))\n          .withContext(`vmin[${i}]: expected ${snapshots[si][2]}, actual ${h.valueMin}`).toBeLessThan(tolerance);\n        expect(Math.abs(snapshots[si][3] - h.valueMax))\n          .withContext(`vmax[${i}]: expected ${snapshots[si][3]}, actual ${h.valueMax}`).toBeLessThan(tolerance);\n        si++;\n      }\n    }\n    expect(si).toBe(snapshots.length);\n  });\n\n  const primeCount = 50;\n  const value = 100.0;\n\n  function prime(x: CoronaSwingPosition): void {\n    for (let i = 0; i < primeCount; i++) {\n      x.update(coronaTestInput[i % coronaTestInput.length], time);\n    }\n  }\n\n  function checkOutput(out: any[]): void {\n    expect(out.length).toBe(2);\n    expect(out[0] instanceof Heatmap).toBe(true);\n    expect((out[0] as Heatmap).time).toEqual(time);\n    expect((out[1] as Scalar).time).toEqual(time);\n  }\n\n  it('should produce 2-element output via updateScalar', () => {\n    const x = CoronaSwingPosition.default();\n    prime(x);\n    const s = new Scalar(); s.time = time; s.value = value;\n    checkOutput(x.updateScalar(s) as any[]);\n  });\n\n  it('should produce 2-element output via updateBar', () => {\n    const x = CoronaSwingPosition.default();\n    prime(x);\n    const bar = new Bar({ time, open: value, high: value, low: value, close: value, volume: 0 });\n    checkOutput(x.updateBar(bar) as any[]);\n  });\n\n  it('should produce 2-element output via updateQuote', () => {\n    const x = CoronaSwingPosition.default();\n    prime(x);\n    const q = new Quote({ time, bid: value, ask: value, bidSize: 0, askSize: 0 });\n    checkOutput(x.updateQuote(q) as any[]);\n  });\n\n  it('should produce 2-element output via updateTrade', () => {\n    const x = CoronaSwingPosition.default();\n    prime(x);\n    const t = new Trade({ time, price: value, volume: 0 });\n    checkOutput(x.updateTrade(t) as any[]);\n  });\n});\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-swing-position/corona-swing-position.spec.ts"
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

## Assistant (Build · claude-opus-4.7 · 6.7s)

**Tool: bash**

**Input:**
```json
{
  "command": "npm test -- --filter=CoronaSwingPosition 2>&1 | tail -40",
  "description": "Run CoronaSwingPosition tests",
  "workdir": "/home/dev/zpano/ts"
}
```

**Output:**
```

> ts@1.0.0 test
> node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=CoronaSwingPosition

Started
...........


Ran 11 of 783 specs
11 specs, 0 failures
Finished in 0.056 seconds

```

---

## Assistant (Build · claude-opus-4.7 · 13.7s)

All 11 CSwing specs pass. Now CoronaTrendVigor.

**Tool: write**

**Input:**
```json
{
  "content": "/** Enumerates outputs of the CoronaTrendVigor indicator. */\nexport enum CoronaTrendVigorOutput {\n  /** The Corona trend vigor heatmap column. */\n  Value = 0,\n  /** The current trend vigor scalar, mapped to [MinParameterValue, MaxParameterValue]. */\n  TrendVigor = 1,\n}\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-trend-vigor/corona-trend-vigor-output.ts"
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

**Tool: write**

**Input:**
```json
{
  "content": "import { BarComponent } from '../../../entities/bar-component';\nimport { QuoteComponent } from '../../../entities/quote-component';\nimport { TradeComponent } from '../../../entities/trade-component';\n\n/** Describes parameters to create an instance of the CoronaTrendVigor indicator. */\nexport interface CoronaTrendVigorParams {\n  /** Length of the heatmap raster. Default 50. A zero value is treated as \"use default\". */\n  rasterLength?: number;\n\n  /** Maximum raster intensity value. Default 20. A zero value is treated as \"use default\". */\n  maxRasterValue?: number;\n\n  /** Minimum ordinate (y) value. Default -10. Only substituted when both Min and Max are 0 (unconfigured). */\n  minParameterValue?: number;\n\n  /** Maximum ordinate (y) value. Default 10. Only substituted when both Min and Max are 0 (unconfigured). */\n  maxParameterValue?: number;\n\n  /** High-pass filter cutoff. Default 30. A zero value is treated as \"use default\". */\n  highPassFilterCutoff?: number;\n\n  /** Minimal cycle period. Default 6. A zero value is treated as \"use default\". */\n  minimalPeriod?: number;\n\n  /** Maximal cycle period. Default 30. A zero value is treated as \"use default\". */\n  maximalPeriod?: number;\n\n  /** A component of a bar to use (default BarComponent.Median). */\n  barComponent?: BarComponent;\n\n  /** A component of a quote to use. */\n  quoteComponent?: QuoteComponent;\n\n  /** A component of a trade to use. */\n  tradeComponent?: TradeComponent;\n}\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-trend-vigor/corona-trend-vigor-params.ts"
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

## Assistant (Build · claude-opus-4.7 · 86.9s)

**Tool: write**

**Input:**
```json
{
  "content": "import { Bar } from '../../../entities/bar';\nimport { BarComponent, barComponentValue } from '../../../entities/bar-component';\nimport { Quote } from '../../../entities/quote';\nimport { DefaultQuoteComponent, quoteComponentValue } from '../../../entities/quote-component';\nimport { Scalar } from '../../../entities/scalar';\nimport { Trade } from '../../../entities/trade';\nimport { DefaultTradeComponent, tradeComponentValue } from '../../../entities/trade-component';\nimport { componentTripleMnemonic } from '../../core/component-triple-mnemonic';\nimport { Indicator } from '../../core/indicator';\nimport { IndicatorMetadata } from '../../core/indicator-metadata';\nimport { IndicatorOutput } from '../../core/indicator-output';\nimport { IndicatorType } from '../../core/indicator-type';\nimport { Heatmap } from '../../core/outputs/heatmap';\nimport { OutputType } from '../../core/outputs/output-type';\nimport { Corona } from '../corona/corona';\nimport { CoronaTrendVigorOutput } from './corona-trend-vigor-output';\nimport { CoronaTrendVigorParams } from './corona-trend-vigor-params';\n\nconst DEFAULT_RASTER_LENGTH = 50;\nconst DEFAULT_MAX_RASTER = 20;\nconst DEFAULT_MIN_PARAM = -10;\nconst DEFAULT_MAX_PARAM = 10;\nconst DEFAULT_HP_CUTOFF = 30;\nconst DEFAULT_MIN_PERIOD = 6;\nconst DEFAULT_MAX_PERIOD = 30;\n\nconst BP_DELTA = 0.1;\nconst RATIO_NEW_COEF = 0.33;\nconst RATIO_PREV_COEF = 0.67;\n\nconst VIGOR_MID_LOW = 0.3;\nconst VIGOR_MID_HIGH = 0.7;\nconst VIGOR_MID = 0.5;\nconst WIDTH_EDGE = 0.01;\n\nconst RASTER_BLEND_SCALE = 0.8;\nconst RASTER_BLEND_PREVIOUS = 0.2;\nconst RASTER_BLEND_HALF = 0.5;\nconst RASTER_BLEND_EXPONENT = 0.85;\n\nconst RATIO_LIMIT = 10;\nconst VIGOR_SCALE = 0.05;\n\n/** __Corona Trend Vigor__ (Ehlers) heatmap indicator.\n *\n * Slope of momentum over a full dominant cycle period, normalized by the cycle amplitude and\n * scaled into [-10, 10]. Values between -2 and +2 form the \"corona\" (do not trade the trend).\n *\n * Reference: John Ehlers, \"Measuring Cycle Periods\", Stocks & Commodities, November 2008.\n */\nexport class CoronaTrendVigor implements Indicator {\n  private readonly c: Corona;\n  private readonly rasterLength: number;\n  private readonly rasterStep: number;\n  private readonly maxRasterValue: number;\n  private readonly minParameterValue: number;\n  private readonly maxParameterValue: number;\n  private readonly parameterResolution: number;\n  private readonly raster: number[];\n  private readonly sampleBuffer: number[];\n\n  private readonly mnemonicValue: string;\n  private readonly descriptionValue: string;\n  private readonly mnemonicTV: string;\n  private readonly descriptionTV: string;\n\n  private readonly barComponentFunc: (bar: Bar) => number;\n  private readonly quoteComponentFunc: (quote: Quote) => number;\n  private readonly tradeComponentFunc: (trade: Trade) => number;\n\n  private sampleCount = 0;\n  private samplePrev = 0;\n  private samplePrev2 = 0;\n  private bandPassPrev = 0;\n  private bandPassPrev2 = 0;\n  private ratioPrev = 0;\n  private trendVigor = Number.NaN;\n\n  public static default(): CoronaTrendVigor {\n    return new CoronaTrendVigor({});\n  }\n\n  public static fromParams(params: CoronaTrendVigorParams): CoronaTrendVigor {\n    return new CoronaTrendVigor(params);\n  }\n\n  private constructor(params: CoronaTrendVigorParams) {\n    const invalid = 'invalid corona trend vigor parameters';\n\n    const rasterLength = params.rasterLength !== undefined && params.rasterLength !== 0\n      ? params.rasterLength : DEFAULT_RASTER_LENGTH;\n    const maxRaster = params.maxRasterValue !== undefined && params.maxRasterValue !== 0\n      ? params.maxRasterValue : DEFAULT_MAX_RASTER;\n\n    let minParam = params.minParameterValue ?? 0;\n    let maxParam = params.maxParameterValue ?? 0;\n    if (minParam === 0 && maxParam === 0) {\n      minParam = DEFAULT_MIN_PARAM;\n      maxParam = DEFAULT_MAX_PARAM;\n    }\n\n    const hpCutoff = params.highPassFilterCutoff !== undefined && params.highPassFilterCutoff !== 0\n      ? params.highPassFilterCutoff : DEFAULT_HP_CUTOFF;\n    const minPeriod = params.minimalPeriod !== undefined && params.minimalPeriod !== 0\n      ? params.minimalPeriod : DEFAULT_MIN_PERIOD;\n    const maxPeriod = params.maximalPeriod !== undefined && params.maximalPeriod !== 0\n      ? params.maximalPeriod : DEFAULT_MAX_PERIOD;\n\n    if (rasterLength < 2) throw new Error(`${invalid}: RasterLength should be >= 2`);\n    if (maxRaster <= 0) throw new Error(`${invalid}: MaxRasterValue should be > 0`);\n    if (maxParam <= minParam) throw new Error(`${invalid}: MaxParameterValue should be > MinParameterValue`);\n    if (hpCutoff < 2) throw new Error(`${invalid}: HighPassFilterCutoff should be >= 2`);\n    if (minPeriod < 2) throw new Error(`${invalid}: MinimalPeriod should be >= 2`);\n    if (maxPeriod <= minPeriod) throw new Error(`${invalid}: MaximalPeriod should be > MinimalPeriod`);\n\n    const bc = params.barComponent ?? BarComponent.Median;\n    const qc = params.quoteComponent ?? DefaultQuoteComponent;\n    const tc = params.tradeComponent ?? DefaultTradeComponent;\n\n    this.barComponentFunc = barComponentValue(bc);\n    this.quoteComponentFunc = quoteComponentValue(qc);\n    this.tradeComponentFunc = tradeComponentValue(tc);\n\n    this.c = new Corona({\n      highPassFilterCutoff: hpCutoff,\n      minimalPeriod: minPeriod,\n      maximalPeriod: maxPeriod,\n    });\n\n    this.rasterLength = rasterLength;\n    this.rasterStep = maxRaster / rasterLength;\n    this.maxRasterValue = maxRaster;\n    this.minParameterValue = minParam;\n    this.maxParameterValue = maxParam;\n    this.parameterResolution = (rasterLength - 1) / (maxParam - minParam);\n    this.raster = new Array<number>(rasterLength).fill(0);\n    this.sampleBuffer = new Array<number>(this.c.maximalPeriodTimesTwo).fill(0);\n\n    const cm = componentTripleMnemonic(bc, qc, tc);\n    this.mnemonicValue = `ctv(${rasterLength}, ${maxRaster}, ${minParam}, ${maxParam}, ${hpCutoff}${cm})`;\n    this.mnemonicTV = `ctv-tv(${hpCutoff}${cm})`;\n    this.descriptionValue = 'Corona trend vigor ' + this.mnemonicValue;\n    this.descriptionTV = 'Corona trend vigor scalar ' + this.mnemonicTV;\n  }\n\n  public isPrimed(): boolean { return this.c.isPrimed(); }\n\n  public metadata(): IndicatorMetadata {\n    return {\n      type: IndicatorType.CoronaTrendVigor,\n      mnemonic: this.mnemonicValue,\n      description: this.descriptionValue,\n      outputs: [\n        { kind: CoronaTrendVigorOutput.Value, type: OutputType.Heatmap, mnemonic: this.mnemonicValue, description: this.descriptionValue },\n        { kind: CoronaTrendVigorOutput.TrendVigor, type: OutputType.Scalar, mnemonic: this.mnemonicTV, description: this.descriptionTV },\n      ],\n    };\n  }\n\n  /** Feeds the next sample and returns the heatmap column plus the current TrendVigor. */\n  public update(sample: number, time: Date): [Heatmap, number] {\n    if (Number.isNaN(sample)) {\n      return [\n        Heatmap.newEmptyHeatmap(time, this.minParameterValue, this.maxParameterValue, this.parameterResolution),\n        Number.NaN,\n      ];\n    }\n\n    const primed = this.c.update(sample);\n    this.sampleCount++;\n\n    const bufLast = this.sampleBuffer.length - 1;\n\n    if (this.sampleCount === 1) {\n      this.samplePrev = sample;\n      this.sampleBuffer[bufLast] = sample;\n      return [\n        Heatmap.newEmptyHeatmap(time, this.minParameterValue, this.maxParameterValue, this.parameterResolution),\n        Number.NaN,\n      ];\n    }\n\n    // Bandpass InPhase filter at dominant cycle median period.\n    const omega = 2 * Math.PI / this.c.dominantCycleMedian;\n    const beta2 = Math.cos(omega);\n    const gamma2 = 1 / Math.cos(omega * 2 * BP_DELTA);\n    const alpha2 = gamma2 - Math.sqrt(gamma2 * gamma2 - 1);\n    const bandPass = 0.5 * (1 - alpha2) * (sample - this.samplePrev2)\n      + beta2 * (1 + alpha2) * this.bandPassPrev\n      - alpha2 * this.bandPassPrev2;\n\n    const quadrature2 = (bandPass - this.bandPassPrev) / omega;\n\n    this.bandPassPrev2 = this.bandPassPrev;\n    this.bandPassPrev = bandPass;\n    this.samplePrev2 = this.samplePrev;\n    this.samplePrev = sample;\n\n    // Left-shift sampleBuffer and append the new sample.\n    for (let i = 0; i < bufLast; i++) {\n      this.sampleBuffer[i] = this.sampleBuffer[i + 1];\n    }\n    this.sampleBuffer[bufLast] = sample;\n\n    const amplitude2 = Math.sqrt(bandPass * bandPass + quadrature2 * quadrature2);\n\n    // DominantCycleMedian-1 directly; clamp to [1, sampleBuffer.length].\n    let cyclePeriod = Math.trunc(this.c.dominantCycleMedian - 1);\n    if (cyclePeriod > this.sampleBuffer.length) cyclePeriod = this.sampleBuffer.length;\n    if (cyclePeriod < 1) cyclePeriod = 1;\n\n    let lookback = cyclePeriod;\n    if (this.sampleCount < lookback) lookback = this.sampleCount;\n\n    const trend = sample - this.sampleBuffer[this.sampleBuffer.length - lookback];\n\n    let ratio = 0;\n    if (Math.abs(trend) > 0 && amplitude2 > 0) {\n      ratio = RATIO_NEW_COEF * trend / amplitude2 + RATIO_PREV_COEF * this.ratioPrev;\n    }\n\n    if (ratio > RATIO_LIMIT) ratio = RATIO_LIMIT;\n    else if (ratio < -RATIO_LIMIT) ratio = -RATIO_LIMIT;\n\n    this.ratioPrev = ratio;\n\n    const vigor = VIGOR_SCALE * (ratio + RATIO_LIMIT);\n\n    let width: number;\n    if (vigor >= VIGOR_MID_LOW && vigor < VIGOR_MID) {\n      width = vigor - (VIGOR_MID_LOW - WIDTH_EDGE);\n    } else if (vigor >= VIGOR_MID && vigor <= VIGOR_MID_HIGH) {\n      width = (VIGOR_MID_HIGH + WIDTH_EDGE) - vigor;\n    } else {\n      width = WIDTH_EDGE;\n    }\n\n    this.trendVigor = (this.maxParameterValue - this.minParameterValue) * vigor + this.minParameterValue;\n\n    const vigorScaledToRasterLength = Math.round(this.rasterLength * vigor);\n    const vigorScaledToMaxRasterValue = vigor * this.maxRasterValue;\n\n    for (let i = 0; i < this.rasterLength; i++) {\n      let value = this.raster[i];\n\n      if (i === vigorScaledToRasterLength) {\n        value *= RASTER_BLEND_HALF;\n      } else {\n        let argument = vigorScaledToMaxRasterValue - this.rasterStep * i;\n        if (i > vigorScaledToRasterLength) argument = -argument;\n        if (width > 0) {\n          value = RASTER_BLEND_SCALE * (Math.pow(argument / width, RASTER_BLEND_EXPONENT) + RASTER_BLEND_PREVIOUS * value);\n        }\n      }\n\n      if (value < 0) {\n        value = 0;\n      } else if (value > this.maxRasterValue || vigor < VIGOR_MID_LOW || vigor > VIGOR_MID_HIGH) {\n        value = this.maxRasterValue;\n      }\n\n      if (Number.isNaN(value)) value = 0;\n\n      this.raster[i] = value;\n    }\n\n    if (!primed) {\n      return [\n        Heatmap.newEmptyHeatmap(time, this.minParameterValue, this.maxParameterValue, this.parameterResolution),\n        Number.NaN,\n      ];\n    }\n\n    const values = new Array<number>(this.rasterLength);\n    let valueMin = Number.POSITIVE_INFINITY;\n    let valueMax = Number.NEGATIVE_INFINITY;\n    for (let i = 0; i < this.rasterLength; i++) {\n      const v = this.raster[i];\n      values[i] = v;\n      if (v < valueMin) valueMin = v;\n      if (v > valueMax) valueMax = v;\n    }\n\n    const heatmap = Heatmap.newHeatmap(\n      time, this.minParameterValue, this.maxParameterValue, this.parameterResolution,\n      valueMin, valueMax, values,\n    );\n\n    return [heatmap, this.trendVigor];\n  }\n\n  public updateScalar(sample: Scalar): IndicatorOutput { return this.updateEntity(sample.time, sample.value); }\n  public updateBar(sample: Bar): IndicatorOutput { return this.updateEntity(sample.time, this.barComponentFunc(sample)); }\n  public updateQuote(sample: Quote): IndicatorOutput { return this.updateEntity(sample.time, this.quoteComponentFunc(sample)); }\n  public updateTrade(sample: Trade): IndicatorOutput { return this.updateEntity(sample.time, this.tradeComponentFunc(sample)); }\n\n  private updateEntity(time: Date, sample: number): IndicatorOutput {\n    const [heatmap, tv] = this.update(sample, time);\n    const s = new Scalar();\n    s.time = time;\n    s.value = tv;\n    return [heatmap, s];\n  }\n}\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-trend-vigor/corona-trend-vigor.ts"
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

**Tool: write**

**Input:**
```json
{
  "content": "import { } from 'jasmine';\n\nimport { Bar } from '../../../entities/bar';\nimport { Quote } from '../../../entities/quote';\nimport { Scalar } from '../../../entities/scalar';\nimport { Trade } from '../../../entities/trade';\nimport { IndicatorType } from '../../core/indicator-type';\nimport { Heatmap } from '../../core/outputs/heatmap';\nimport { OutputType } from '../../core/outputs/output-type';\nimport { coronaTestInput } from '../corona/corona-test-input';\nimport { CoronaTrendVigor } from './corona-trend-vigor';\nimport { CoronaTrendVigorOutput } from './corona-trend-vigor-output';\n\nconst tolerance = 1e-4;\n\ndescribe('CoronaTrendVigor', () => {\n  const time = new Date(2021, 3, 1);\n\n  it('should have correct output enum values', () => {\n    expect(CoronaTrendVigorOutput.Value).toBe(0);\n    expect(CoronaTrendVigorOutput.TrendVigor).toBe(1);\n  });\n\n  it('should return expected mnemonic for default params', () => {\n    const x = CoronaTrendVigor.default();\n    expect(x.metadata().mnemonic).toBe('ctv(50, 20, -10, 10, 30, hl/2)');\n  });\n\n  it('should return expected metadata', () => {\n    const x = CoronaTrendVigor.default();\n    const meta = x.metadata();\n    const mn = 'ctv(50, 20, -10, 10, 30, hl/2)';\n    const mnTV = 'ctv-tv(30, hl/2)';\n\n    expect(meta.type).toBe(IndicatorType.CoronaTrendVigor);\n    expect(meta.mnemonic).toBe(mn);\n    expect(meta.outputs.length).toBe(2);\n    expect(meta.outputs[0].kind).toBe(CoronaTrendVigorOutput.Value);\n    expect(meta.outputs[0].type).toBe(OutputType.Heatmap);\n    expect(meta.outputs[1].kind).toBe(CoronaTrendVigorOutput.TrendVigor);\n    expect(meta.outputs[1].type).toBe(OutputType.Scalar);\n    expect(meta.outputs[1].mnemonic).toBe(mnTV);\n  });\n\n  it('should throw for invalid params', () => {\n    expect(() => CoronaTrendVigor.fromParams({ rasterLength: 1 })).toThrowError(/RasterLength/);\n    expect(() => CoronaTrendVigor.fromParams({ minParameterValue: 5, maxParameterValue: 5 }))\n      .toThrowError(/MaxParameterValue/);\n    expect(() => CoronaTrendVigor.fromParams({ highPassFilterCutoff: 1 })).toThrowError(/HighPassFilterCutoff/);\n    expect(() => CoronaTrendVigor.fromParams({ minimalPeriod: 1 })).toThrowError(/MinimalPeriod/);\n  });\n\n  it('should return empty heatmap and NaN for NaN input', () => {\n    const x = CoronaTrendVigor.default();\n    const [h, tv] = x.update(Number.NaN, time);\n    expect(h.isEmpty()).toBe(true);\n    expect(Number.isNaN(tv)).toBe(true);\n    expect(x.isPrimed()).toBe(false);\n  });\n\n  it('should prime at sample index 11', () => {\n    const x = CoronaTrendVigor.default();\n    let primedAt = -1;\n    for (let i = 0; i < coronaTestInput.length; i++) {\n      x.update(coronaTestInput[i], new Date(time.getTime() + i * 60_000));\n      if (x.isPrimed() && primedAt < 0) primedAt = i;\n    }\n    expect(primedAt).toBe(11);\n  });\n\n  it('should match reference tv / vmin / vmax snapshots', () => {\n    const snapshots: Array<[number, number, number, number]> = [\n      [11, 5.6512200755, 20.0000000000, 20.0000000000],\n      [12, 6.8379492897, 20.0000000000, 20.0000000000],\n      [50, 2.6145116709, 2.3773561485, 20.0000000000],\n      [100, 2.7536803664, 2.4892742850, 20.0000000000],\n      [150, -6.4606404251, 20.0000000000, 20.0000000000],\n      [200, -10.0000000000, 20.0000000000, 20.0000000000],\n      [251, -0.1894989954, 0.5847573715, 20.0000000000],\n    ];\n\n    const x = CoronaTrendVigor.default();\n    let si = 0;\n    for (let i = 0; i < coronaTestInput.length; i++) {\n      const t = new Date(time.getTime() + i * 60_000);\n      const [h, tv] = x.update(coronaTestInput[i], t);\n\n      expect(h.parameterFirst).toBe(-10);\n      expect(h.parameterLast).toBe(10);\n      expect(Math.abs(h.parameterResolution - 2.45)).toBeLessThan(1e-9);\n\n      if (!x.isPrimed()) {\n        expect(h.isEmpty()).toBe(true);\n        expect(Number.isNaN(tv)).toBe(true);\n        continue;\n      }\n\n      expect(h.values.length).toBe(50);\n\n      if (si < snapshots.length && snapshots[si][0] === i) {\n        expect(Math.abs(snapshots[si][1] - tv))\n          .withContext(`tv[${i}]: expected ${snapshots[si][1]}, actual ${tv}`).toBeLessThan(tolerance);\n        expect(Math.abs(snapshots[si][2] - h.valueMin))\n          .withContext(`vmin[${i}]: expected ${snapshots[si][2]}, actual ${h.valueMin}`).toBeLessThan(tolerance);\n        expect(Math.abs(snapshots[si][3] - h.valueMax))\n          .withContext(`vmax[${i}]: expected ${snapshots[si][3]}, actual ${h.valueMax}`).toBeLessThan(tolerance);\n        si++;\n      }\n    }\n    expect(si).toBe(snapshots.length);\n  });\n\n  const primeCount = 50;\n  const value = 100.0;\n\n  function prime(x: CoronaTrendVigor): void {\n    for (let i = 0; i < primeCount; i++) {\n      x.update(coronaTestInput[i % coronaTestInput.length], time);\n    }\n  }\n\n  function checkOutput(out: any[]): void {\n    expect(out.length).toBe(2);\n    expect(out[0] instanceof Heatmap).toBe(true);\n    expect((out[0] as Heatmap).time).toEqual(time);\n    expect((out[1] as Scalar).time).toEqual(time);\n  }\n\n  it('should produce 2-element output via updateScalar', () => {\n    const x = CoronaTrendVigor.default();\n    prime(x);\n    const s = new Scalar(); s.time = time; s.value = value;\n    checkOutput(x.updateScalar(s) as any[]);\n  });\n\n  it('should produce 2-element output via updateBar', () => {\n    const x = CoronaTrendVigor.default();\n    prime(x);\n    const bar = new Bar({ time, open: value, high: value, low: value, close: value, volume: 0 });\n    checkOutput(x.updateBar(bar) as any[]);\n  });\n\n  it('should produce 2-element output via updateQuote', () => {\n    const x = CoronaTrendVigor.default();\n    prime(x);\n    const q = new Quote({ time, bid: value, ask: value, bidSize: 0, askSize: 0 });\n    checkOutput(x.updateQuote(q) as any[]);\n  });\n\n  it('should produce 2-element output via updateTrade', () => {\n    const x = CoronaTrendVigor.default();\n    prime(x);\n    const t = new Trade({ time, price: value, volume: 0 });\n    checkOutput(x.updateTrade(t) as any[]);\n  });\n});\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/corona-trend-vigor/corona-trend-vigor.spec.ts"
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

## User

---

## Assistant (Compaction · claude-opus-4.7 · 72.6s)

---
## Goal

Port MBST's **Ehlers Corona Charts suite** (TASC November 2008) from `/home/dev/zpano/mbst-to-convert/john-ehlers/corona/` into zpano:

- **Corona** shared helper (not registered as indicator)
- **CoronaSpectrum** — heatmap + DC + DCM scalars
- **CoronaSignalToNoiseRatio** — heatmap + SNR scalar
- **CoronaSwingPosition** — heatmap + SwingPosition scalar
- **CoronaTrendVigor** — heatmap + TrendVigor scalar

First heatmap-producing indicators in zpano. **Go phase complete.** **TS Phase 6 nearly complete: helper, type enum, heatmap factories, shared test input, and all 4 indicator packages written. CoronaSpectrum (12/12), CoronaSignalToNoiseRatio (11/11), CoronaSwingPosition (11/11) all pass. CoronaTrendVigor just written — needs test run.**

## Instructions

### Design decisions (confirmed)
- Unprimed bars → empty heatmap (axis metadata present, values=[]); scalars NaN.
- Mnemonics: `cspect(…)`, `cspect-dc(…)`, `cspect-dcm(…)`, `csnr(…)`, `csnr-snr(…)`, `cswing(…)`, `cswing-sp(…)`, `ctv(…)`, `ctv-tv(…)`.
- User said: "continue if you have next steps, or stop and ask for clarification if you are unsure how to proceed." — proceed autonomously.
- Snapshot tests reuse 252-entry TA-Lib MAMA series with tolerance 1e-4.
- **Ignore LSP errors from `/home/dev/zpano/mbst-to-convert/common/*.c` — unrelated C/ta-lib noise.**
- Test command: `npm test -- --filter=X` or `npm test` from `/home/dev/zpano/ts`.

### Algorithm decisions (locked)
- Swing BP2 momentum: 2-bar (`sample - samplePrev2`).
- TrendVigor lookback: `int(DCM - 1)`; guard `cyclePeriod >= 1` (MBST fix); clamp upper at `sampleBuffer.length`.
- SNR signal EMA coefficients: 0.2/0.9 (sum 1.1, intentional per Ehlers).
- DC weighted average: N=12..60; DC=0.5·num/denom; clamp DC<minPeriod. DCM = median of 5-buffer; clamp <minPeriod.
- DominantCycleBuffer initialized to `Number.MAX_VALUE` sentinels.
- CSpectrum rounds `minParam` up (ceil), `maxParam` down (floor).
- CSNR/CSwing/CTV: first sample store-only ("isStarted") returns empty heatmap, NaN scalar.
- CSNR scalar path: H=L=sample ⇒ SNR=MinParameterValue.
- CSwing/CTV: substitute defaults for Min/Max params **only when both are 0** (unconfigured).

### Defaults / formulas
- Corona: `HP=30, minPer=6, maxPer=30, dBlow=6, dBhigh=20`.
- CSpectrum: `minRaster=6, maxRaster=20, minParam=6, maxParam=30, hpCutoff=30, BarComponent=Median`; res=(len-1)/24=2.0.
- CSNR: `rasterLength=50, maxRaster=20, minParam=1, maxParam=11, HP=30`; res=49/10=4.9.
- CSwing: `rasterLength=50, maxRaster=20, minParam=-5, maxParam=5`; res=49/10=4.9.
- CTV: `rasterLength=50, maxRaster=20, minParam=-10, maxParam=10`; res=49/20=2.45.

### TS conventions
- `class X implements Indicator` with private constructor + `static default()` + `static fromParams(params)`.
- `componentTripleMnemonic(bc, qc, tc)` — pass `BarComponent.Median` explicitly for `, hl/2` suffix.
- `update()` returns tuple; `updateEntity()` maps tuple → `IndicatorOutput`.
- `new Scalar()` then assign `time`/`value`; `new Bar({...})`/`new Quote({...})`/`new Trade({...})` constructor-object.
- `Heatmap.newHeatmap(...)`, `Heatmap.newEmptyHeatmap(...)`, `.isEmpty()`.
- Test framework: Jasmine, `import { } from 'jasmine'`, tolerance 1e-4.
- Template literal `${n}` for integer-valued floats (matches Go `%g`).
- Enum values start from 0 in TS.
- Corona exposes public `maximalPeriodTimesTwo` (used by CTV's sampleBuffer).

## Discoveries

### Snapshot values (Go reference — locked)

**CSpectrum** (`dc`, `dcm`): {11, 17.7605, 17.7605}, {12, 6, 6}, {50, 15.9989, 15.9989}, {100, 14.7455, 14.7455}, {150, 17.5, 17.2826}, {200, 19.7557, 20}, {251, 6, 6}

**CSNR** uses `makeHL(i, sample)`:
```ts
const frac = 0.005 + 0.03 * (1 + Math.sin(i * 0.37));
const half = sample * frac;
return [sample - half, sample + half];
```
Snapshots (`snr`, `vmin`, `vmax`): {11,1,0,20}, {12,1,0,20}, {50,1,0,20}, {100,2.9987,4.2012,20}, {150,1,~0,20}, {200,1,0,20}, {251,1,~0,20}

**CSwing** (`sp`, `vmin`, `vmax`): {11,5,20,20}, {12,5,20,20}, {50,4.5385,20,20}, {100,-3.8184,3.4958,20}, {150,-1.8516,5.3792,20}, {200,-3.6944,4.2581,20}, {251,-0.8525,4.4823,20}

**CTV** (`tv`, `vmin`, `vmax`): {11,5.6512,20,20}, {12,6.8379,20,20}, {50,2.6145,2.3774,20}, {100,2.7537,2.4893,20}, {150,-6.4606,20,20}, {200,-10,20,20}, {251,-0.1895,0.5848,20}

### Mnemonic expectations
- CSpectrum default: `cspect(6, 20, 6, 30, 30, hl/2)`; custom: `cspect(4, 25, 9, 40, 20, hl/2)` (min=8.7→ceil 9, max=40.4→floor 40).
- CSNR: `csnr(50, 20, 1, 11, 30, hl/2)`; `csnr-snr(30, hl/2)`.
- CSwing: `cswing(50, 20, -5, 5, 30, hl/2)`; `cswing-sp(30, hl/2)`.
- CTV: `ctv(50, 20, -10, 10, 30, hl/2)`; `ctv-tv(30, hl/2)`.

### CSNR nuance
`update(sample, sampleLow, sampleHigh, time)` — note order `low, high`. `updateBar` passes `(barFunc(sample), sample.low, sample.high)`. `updateScalar/Quote/Trade` pass `(v, v, v)`.

## Accomplished

### ✅ Go Phase Complete (all tests green)
- Corona helper, 4 `core.Type` constants, 4 indicator packages with tests.

### ✅ TS Phase 6 — progress
1. **IndicatorType enum**: 4 entries appended.
2. **Heatmap class**: `newHeatmap`, `newEmptyHeatmap`, `isEmpty()` added.
3. **Corona helper**: `corona/corona-params.ts` + `corona/corona.ts` (exposes `maximalPeriodTimesTwo`, `dominantCycleMedian`, `maximalAmplitudeSquared`, `filterBank`, `filterBankLength`).
4. **Shared test input**: `corona/corona-test-input.ts` — 252-entry array.
5. **CoronaSpectrum** ✅ 12/12 specs pass.
6. **CoronaSignalToNoiseRatio** ✅ 11/11 specs pass.
7. **CoronaSwingPosition** ✅ 11/11 specs pass.
8. **CoronaTrendVigor** ⏳ Just written — **needs `npm test -- --filter=CoronaTrendVigor`**.

### ⏳ Remaining work
1. Run `npm test -- --filter=CoronaTrendVigor`. If failing, debug (most likely suspects: `Math.trunc` vs Go's `int(…)` for negative floats — Go truncates toward zero, `Math.trunc` also truncates toward zero ✓; sampleBuffer shift direction; `lookback` edge cases).
2. Run full `npm test` to ensure no regressions across all 783+ specs.
3. Update `/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md` with Corona learnings (heatmap conventions, first-bar store-only pattern, DCM-1 lookback fix with `>=1` guard, parameter-resolution formula, empty-heatmap-axis-always-present invariant, CSNR coefficient sum=1.1 is intentional, CSwing/CTV "both 0 ⇒ substitute defaults" pattern).

## Relevant files / directories

### Go reference
- `/home/dev/zpano/go/indicators/johnehlers/corona/`
- `/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/`
- `/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/` (read)
- `/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition.go` (read)
- `/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor.go` (read)

### TS infrastructure (edited/read)
- `/home/dev/zpano/ts/indicators/core/indicator-type.ts` ✅
- `/home/dev/zpano/ts/indicators/core/outputs/heatmap.ts` ✅
- `/home/dev/zpano/ts/indicators/core/component-triple-mnemonic.ts`
- `/home/dev/zpano/ts/entities/{bar,scalar,quote,trade,bar-component,quote-component,trade-component}.ts`

### TS files created ✅
- `/home/dev/zpano/ts/indicators/john-ehlers/corona/{corona-params.ts, corona.ts, corona-test-input.ts}`
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-spectrum/` (4 files, 12/12 pass)
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-signal-to-noise-ratio/` (4 files, 11/11 pass)
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-swing-position/` (4 files, 11/11 pass)
- `/home/dev/zpano/ts/indicators/john-ehlers/corona-trend-vigor/` (4 files ⏳ not yet tested):
  - `corona-trend-vigor-output.ts`
  - `corona-trend-vigor-params.ts`
  - `corona-trend-vigor.ts`
  - `corona-trend-vigor.spec.ts`

### Skill to update after TS complete
- `/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md`

### Immediate next action
Run `npm test -- --filter=CoronaTrendVigor` from `/home/dev/zpano/ts`. If green, run `npm test` for full regression, then update the skill doc.

### Todos
1. ✅ Append 4 Corona entries to indicator-type.ts
2. ✅ Add static factories to heatmap.ts
3. ✅ Create Corona helper
4. ✅ Create CoronaSpectrum (12/12 pass)
5. ✅ Create CoronaSignalToNoiseRatio (11/11 pass)
6. ✅ Create CoronaSwingPosition (11/11 pass)
7. ⏳ Test CoronaTrendVigor (files written, not yet run)
8. ⏳ Run full `npm test` regression
9. ⏳ Update mbst-indicator-conversion skill with Corona learnings

---

## User

---

## Assistant (Build · claude-opus-4.7 · 5.5s)

_Thinking:_

 Let me run the CoronaTrendVigor tests to check what's happening.

**Tool: bash**

**Input:**
```json
{
  "command": "npm test -- --filter=CoronaTrendVigor 2>&1 | tail -80",
  "description": "Run CoronaTrendVigor tests",
  "workdir": "/home/dev/zpano/ts"
}
```

**Output:**
```

> ts@1.0.0 test
> node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=CoronaTrendVigor

Started
...........


Ran 11 of 794 specs
11 specs, 0 failures
Finished in 0.054 seconds

```

---

## Assistant (Build · claude-opus-4.7 · 5.8s)

All 11 CoronaTrendVigor specs pass. Now full regression.

**Tool: bash**

**Input:**
```json
{
  "command": "npm test 2>&1 | tail -20",
  "description": "Full regression test",
  "workdir": "/home/dev/zpano/ts"
}
```

**Output:**
```

> ts@1.0.0 test
> node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json

Started
..........................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................


794 specs, 0 failures
Finished in 0.688 seconds

```

---

## Assistant (Build · claude-opus-4.7 · 4.9s)

All 794 specs pass. Now updating the skill doc with Corona learnings.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md"
}
```

**Output:**
```
<path>/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md</path>
<type>file</type>
<content>1: ---
2: name: mbst-indicator-conversion
3: description: Step-by-step guide for converting MBST C# indicators to zpano Go and TypeScript. Load when converting an MBST indicator or understanding the mapping between MBST and zpano patterns.
4: ---
5: 
6: # Converting MBST C# Indicators to Zpano
7: 
8: This guide provides recipes and tips for converting indicators from the MBST C# codebase
9: (`Mbst.Trading.Indicators`) to the zpano multi-language library (Go and TypeScript).
10: 
11: Load the `indicator-architecture` and `indicator-conversion` skills alongside this one
12: for the full zpano architecture reference and internal conversion patterns.
13: 
14: Load the `mbst-indicator-architecture` skill for the full MBST type hierarchy reference.
15: 
16: ---
17: 
18: ## Table of Contents
19: 
20: 1. [Overview](#overview)
21: 2. [Determine the Indicator Pattern](#determine-the-indicator-pattern)
22: 3. [Component Mapping](#component-mapping)
23: 4. [Constructor Conversion](#constructor-conversion)
24: 5. [Algorithm Conversion](#algorithm-conversion)
25: 6. [Multi-Output Indicators](#multi-output-indicators)
26: 7. [Metadata Conversion](#metadata-conversion)
27: 8. [What to Drop](#what-to-drop)
28: 9. [Test Conversion](#test-conversion)
29: 10. [Worked Example: CenterOfGravityOscillator](#worked-example-centerofgravityoscillator)
30: 
31: ---
32: 
33: ## Overview
34: 
35: MBST indicators live in `mbst-to-convert/<author>/<indicator>/` as C# classes.
36: Each indicator has a `.cs` implementation and a `Test.cs` test file.
37: 
38: The conversion produces:
39: - **Go:** `go/indicators/<author>/<indicator>/` (5 files: params, output, output_test, impl, impl_test)
40: - **TS:** `ts/indicators/<author>/<indicator>/` (4 files: params, output, impl, impl.spec)
41: 
42: Always convert Go first, then TypeScript.
43: 
44: ---
45: 
46: ## Determine the Indicator Pattern
47: 
48: Read the MBST source and identify which base class/interface the indicator uses:
49: 
50: | MBST Pattern | Zpano Pattern |
51: |---|---|
52: | `class X : LineIndicator` | Single output. Use `LineIndicator` embedding (Go) / `extends LineIndicator` (TS). Only implement `Update(float64)` / `update(number)`. |
53: | `class X : Indicator, ILineIndicator` | Multi-output (e.g., value + trigger). Implement `Indicator` interface directly (Go) / `implements Indicator` (TS). Must write `UpdateScalar/Bar/Quote/Trade` manually. |
54: | `class X : BandIndicator` | Band output. Map to zpano's band indicator pattern. |
55: | `class X : Indicator, IBandIndicator` | Custom band output. Implement `Indicator` interface directly. |
56: | `class X : ... IHeatmapIndicator` | Heatmap output. Handle case-by-case. |
57: 
58: ### How to tell if it's multi-output
59: 
60: Look for:
61: 1. **Facade properties** (`ValueFacade`, `TriggerFacade`, etc.) — the indicator exposes
62:    multiple outputs via facades.
63: 2. **Multiple named value fields** (e.g., `value` + `valuePrevious` both exposed publicly).
64: 3. **The class extends `Indicator` directly** (not `LineIndicator`) but implements
65:    `ILineIndicator` — this is a strong signal of multi-output.
66: 
67: **Exception:** Some MBST indicators use `: Indicator, ILineIndicator` but only have a single
68: output (`Value`). Example: `SuperSmoother` has this signature but no facades or additional
69: outputs. In this case, use `LineIndicator` embedding/inheritance in zpano (not the direct
70: `Indicator` interface). Always check the actual outputs — the class hierarchy alone is not
71: sufficient to determine multi-output vs single-output.
72: 
73: If multi-output: use the direct `Indicator` interface approach (like FRAMA, CoG).
74: If single-output: use `LineIndicator` embedding/inheritance.
75: 
76: ---
77: 
78: ## Component Mapping
79: 
80: ### OhlcvComponent to Component Triple
81: 
82: MBST uses a single `OhlcvComponent` for OHLCV bars only. Zpano uses three separate
83: component types: `BarComponent`, `QuoteComponent`, `TradeComponent`.
84: 
85: | MBST `OhlcvComponent` | Zpano `BarComponent` | Go Constant | TS Enum |
86: |---|---|---|---|
87: | `ClosingPrice` (default) | `Close` (default) | `entities.BarClosePrice` | `BarComponent.Close` |
88: | `OpeningPrice` | `Open` | `entities.BarOpenPrice` | `BarComponent.Open` |
89: | `HighPrice` | `High` | `entities.BarHighPrice` | `BarComponent.High` |
90: | `LowPrice` | `Low` | `entities.BarLowPrice` | `BarComponent.Low` |
91: | `MedianPrice` | `Median` | `entities.BarMedianPrice` | `BarComponent.Median` |
92: | `TypicalPrice` | `Typical` | `entities.BarTypicalPrice` | `BarComponent.Typical` |
93: | `WeightedPrice` | `Weighted` | `entities.BarWeightedPrice` | `BarComponent.Weighted` |
94: | `Volume` | `Volume` | `entities.BarVolume` | `BarComponent.Volume` |
95: 
96: ### Default Component Handling
97: 
98: - **MBST default:** `OhlcvComponent.ClosingPrice` — set in `Indicator` base class constructor.
99: - **Zpano default:** `BarClosePrice` / `BarComponent.Close` — aka `DefaultBarComponent`.
100: - **Some indicators override the default** (e.g., CoG defaults to `MedianPrice` instead of
101:   `ClosingPrice`). Check the MBST constructor signature for non-default values.
102: 
103: When converting, QuoteComponent and TradeComponent are **always new** (MBST doesn't have
104: them). Use the zpano defaults (`DefaultQuoteComponent`, `DefaultTradeComponent`) when the
105: params zero-value/undefined is provided.
106: 
107: ### Non-Default Bar Component Rule
108: 
109: If the MBST indicator uses a non-default `OhlcvComponent` (anything other than
110: `ClosingPrice`), the zpano constructor must resolve the zero-value to that component
111: instead of `DefaultBarComponent`. This causes `ComponentTripleMnemonic` to include the
112: component in the mnemonic even for default parameters.
113: 
114: **Go example (CoG defaults to MedianPrice):**
115: ```go
116: bc := params.BarComponent
117: if bc == 0 {
118:     bc = entities.BarMedianPrice  // NOT entities.DefaultBarComponent
119: }
120: ```
121: 
122: **TS example:**
123: ```typescript
124: const bc = params.barComponent ?? BarComponent.Median;  // NOT DefaultBarComponent
125: ```
126: 
127: ---
128: 
129: ## Constructor Conversion
130: 
131: ### MBST Constructor Pattern
132: 
133: ```csharp
134: public CenterOfGravityOscillator(int length = 10,
135:     OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
136:     : base(cog, cogFull, ohlcvComponent)
137: {
138:     if (1 > length) throw new ArgumentOutOfRangeException(argumentLength);
139:     this.length = length;
140:     // ...
141:     moniker = string.Concat(cog, "(", length.ToString(...), ")");
142: }
143: ```
144: 
145: ### Zpano Conversion Rules
146: 
147: 1. **Parameters become a struct/interface** — not individual constructor args.
148: 2. **Validation stays** — translate `ArgumentOutOfRangeException` to `error` (Go) / `throw Error` (TS).
149: 3. **`moniker` becomes `mnemonic`** — built using `ComponentTripleMnemonic` for the triple.
150: 4. **`description` is auto-generated** — typically `"<Full Name> " + mnemonic`.
151: 5. **`base(name, description, component)` call is dropped** — zpano doesn't have a base
152:    class constructor chain for this.
153: 6. **Component function resolution** — create `barFunc`/`quoteFunc`/`tradeFunc` in constructor.
154: 
155: ---
156: 
157: ## Algorithm Conversion
158: 
159: ### The Only Method That Matters
160: 
161: In MBST, `Update(double sample)` contains the entire algorithm. All other `Update`
162: overloads (`Update(Scalar)`, `Update(Ohlcv)`) are pure delegation boilerplate.
163: 
164: **Only convert `Update(double sample)`.** The entity-level update methods are generated
165: by `LineIndicator` embedding (for single-output) or written manually using a simple
166: `updateEntity` helper (for multi-output).
167: 
168: ### C# to Go/TS Algorithm Translation
169: 
170: | C# | Go | TS |
171: |---|---|---|
172: | `double.NaN` | `math.NaN()` | `Number.NaN` |
173: | `double.IsNaN(x)` | `math.IsNaN(x)` | `Number.isNaN(x)` |
174: | `double.Epsilon` | `math.SmallestNonzeroFloat64` | `Number.MIN_VALUE` |
175: | `Math.Abs(x)` | `math.Abs(x)` | `Math.abs(x)` |
176: | `Array.Copy(src, srcIdx, dst, dstIdx, len)` | Manual loop or `copy()` | Manual loop |
177: | `lock (updateLock) { ... }` | `s.mu.Lock(); defer s.mu.Unlock()` | Drop (single-threaded) |
178: 
179: ### Priming Logic
180: 
181: MBST `primed` field is set inside `Update()` after enough samples.
182: Convert directly — zpano uses the same `primed` boolean pattern.
183: 
184: ### NaN Guard
185: 
186: MBST typically has `if (double.IsNaN(sample)) return sample;` at the top of `Update`.
187: Preserve this — it's important for correctness.
188: 
189: ---
190: 
191: ## Multi-Output Indicators
192: 
193: ### MBST: Facade Pattern
194: 
195: MBST exposes multiple outputs via facade classes:
196: 
197: ```csharp
198: // In MBST indicator:
199: public LineIndicatorFacade ValueFacade =>
200:     new LineIndicatorFacade(cog, moniker, cogFull, () => IsPrimed, () => Value);
201: public LineIndicatorFacade TriggerFacade =>
202:     new LineIndicatorFacade(cogTrig, ...moniker..., cogTrigFull, () => IsPrimed, () => Trigger);
203: ```
204: 
205: ### Zpano: Output Array
206: 
207: Zpano replaces facades with an `Output` array (Go `core.Output` / TS `IndicatorOutput`):
208: 
209: **Go:**
210: ```go
211: func (s *X) updateEntity(time time.Time, sample float64) core.Output {
212:     output := make([]any, 2)
213:     cog := s.Update(sample)
214:     trig := s.valuePrevious
215:     if math.IsNaN(cog) { trig = math.NaN() }
216:     output[0] = entities.Scalar{Time: time, Value: cog}
217:     output[1] = entities.Scalar{Time: time, Value: trig}
218:     return output
219: }
220: ```
221: 
222: **TS:**
223: ```typescript
224: private updateEntity(time: Date, sample: number): IndicatorOutput {
225:     const cog = this.update(sample);
226:     let trig = this.valuePrevious;
227:     if (Number.isNaN(cog)) { trig = Number.NaN; }
228:     const s1 = new Scalar(); s1.time = time; s1.value = cog;
229:     const s2 = new Scalar(); s2.time = time; s2.value = trig;
230:     return [s1, s2];
231: }
232: ```
233: 
234: ### Output Enum
235: 
236: Each multi-output indicator gets a per-indicator output enum:
237: 
238: **Go:** `centerofgravityoscillatoroutput.go`
239: ```go
240: type Output int
241: const ( Value Output = iota; Trigger )
242: ```
243: 
244: **TS:** `center-of-gravity-oscillator-output.ts`
245: ```typescript
246: export enum CenterOfGravityOscillatorOutput { Value = 0, Trigger = 1 }
247: ```
248: 
249: ### Metadata for Multi-Output
250: 
251: Each output gets its own entry in `Metadata().Outputs`:
252: 
253: ```go
254: Outputs: []outputs.Metadata{
255:     { Kind: int(Value), Type: outputs.ScalarType, Mnemonic: s.mnemonic, Description: s.description },
256:     { Kind: int(Trigger), Type: outputs.ScalarType, Mnemonic: s.mnemonicTrig, Description: s.descriptionTrig },
257: },
258: ```
259: 
260: Facade-specific mnemonic patterns (like `cogTrig(10)`) are preserved as the Trigger
261: output mnemonic.
262: 
263: ---
264: 
265: ## Metadata Conversion
266: 
267: | MBST | Zpano |
268: |---|---|
269: | `Name` (e.g., `"cog"`) | `Metadata().Type` — use the registered `core.IndicatorType` enum |
270: | `Moniker` (e.g., `"cog(10)"`) | `Metadata().Mnemonic` — includes component triple |
271: | `Description` (e.g., `"Center of Gravity oscillator"`) | `Metadata().Description` — typically `"Full Name " + mnemonic` |
272: 
273: ### Register the Indicator Type
274: 
275: Before implementing, register the indicator in:
276: - **Go:** `go/indicators/core/type.go` — add enum constant, string, `String()`, `MarshalJSON`, `UnmarshalJSON`
277: - **TS:** `ts/indicators/core/indicator-type.ts` — add enum member
278: 
279: ---
280: 
281: ## What to Drop
282: 
283: ### From the Implementation
284: 
285: | MBST Element | Action |
286: |---|---|
287: | `[DataContract]`, `[DataMember]` | Drop — no serialization |
288: | `Reset()` method | Drop — zpano indicators are immutable |
289: | `ToString()` override | Drop — debug formatting |
290: | `lock (updateLock)` | Go: replace with `sync.RWMutex`. TS: drop entirely |
291: | `Update(Scalar)`, `Update(Ohlcv)` | Drop — handled by `LineIndicator` or `updateEntity` |
292: | `Update(double, DateTime)` | Drop — convenience overload |
293: | Facade properties (`ValueFacade`, `TriggerFacade`) | Drop — replaced by Output array |
294: | `OhlcvComponent` property setter | Drop — component is immutable after construction |
295: | C# regions (`#region ... #endregion`) | Drop — organizational noise |
296: | XML doc comments (`/// <summary>`) | Convert to Go doc comments / JSDoc |
297: 
298: ### From Tests
299: 
300: | MBST Test | Action |
301: |---|---|
302: | `ToStringTest()` | Drop |
303: | `SerializeTo()` / `SerializeFrom()` | Drop |
304: | `SerializationTest()` | Drop |
305: | Facade-specific tests | Drop (test via Output array instead) |
306: | `Reset()` tests | Drop |
307: | `[TestMethod]` / `[TestClass]` | Convert to Go `Test*` / TS Jasmine `describe`/`it` |
308: | `Assert.IsTrue(x == y)` | Go: require assertions. TS: Jasmine `expect(x).toBe(y)` |
309: 
310: ---
311: 
312: ## Test Conversion
313: 
314: ### Reference Data
315: 
316: MBST tests typically include large arrays of reference data (from TA-Lib Excel
317: simulations). **Preserve this data exactly** — it's the ground truth for numerical
318: verification.
319: 
320: Typical MBST test structure:
321: ```csharp
322: private readonly List<double> rawInput = new List<double> { ... };  // 252 entries
323: private readonly List<double> expected = new List<double> { ... };  // expected output
324: ```
325: 
326: Convert to:
327: - **Go:** `var` block with `[]float64` slices
328: - **TS:** `const` arrays of `number`
329: 
330: ### Test Categories to Convert
331: 
332: 1. **Output value tests** — Feed `rawInput`, compare against `expected` values.
333:    Use tolerance comparison: Go `require.InDelta(t, expected, actual, 1e-10)`,
334:    TS `expect(Math.abs(actual - expected)).toBeLessThan(1e-10)`.
335: 
336: 2. **IsPrimed tests** — Verify `IsPrimed()` is false during warmup and true after.
337: 
338: 3. **Metadata tests** — Verify `Metadata()` returns correct type, mnemonic, description, outputs.
339: 
340: 4. **Constructor validation** — Verify invalid params (e.g., `length < 1`) produce errors.
341: 
342: 5. **UpdateEntity tests** — Test `UpdateScalar`, `UpdateBar`, `UpdateQuote`, `UpdateTrade`
343:    with a few samples to verify entity routing works.
344: 
345: 6. **NaN handling** — Verify that NaN input produces NaN output without corrupting state.
346: 
347: ### Test for Multi-Output
348: 
349: For multi-output indicators, test **both** (all) outputs against reference data:
350: ```go
351: // After feeding sample:
352: output := indicator.UpdateScalar(scalar)
353: cogScalar := output[int(Value)].(entities.Scalar)
354: trigScalar := output[int(Trigger)].(entities.Scalar)
355: require.InDelta(t, expectedCog, cogScalar.Value, tolerance)
356: require.InDelta(t, expectedTrig, trigScalar.Value, tolerance)
357: ```
358: 
359: ### MBST Test Data with Separate High/Low Arrays
360: 
361: Some MBST tests provide raw high and low arrays separately, then compute median price
362: in the test. In zpano tests, either:
363: - Pre-compute the median values and store as a single input array, or
364: - Feed `Bar` entities with high/low set and test via `UpdateBar`.
365: 
366: The CoG tests use a pre-computed `rawInput` array of median prices (already `(high + low) / 2`).
367: 
368: ---
369: 
370: ## Worked Example: CenterOfGravityOscillator
371: 
372: ### Source Analysis
373: 
374: ```csharp
375: // MBST: extends Indicator directly, implements ILineIndicator
376: // Has Value + Trigger facades = multi-output
377: // Default OhlcvComponent: MedianPrice (non-default!)
378: public sealed class CenterOfGravityOscillator : Indicator, ILineIndicator
379: ```
380: 
381: ### Decision: Multi-output, direct Indicator interface
382: 
383: Because CoG has two outputs (Value, Trigger) and extends `Indicator` directly (not
384: `LineIndicator`), it uses the direct `Indicator` interface pattern in zpano.
385: 
386: ### Key Conversion Points
387: 
388: 1. **Default component**: `OhlcvComponent.MedianPrice` -> `BarMedianPrice` / `BarComponent.Median`.
389:    Not the framework default, so it always appears in the mnemonic (`, hl/2`).
390: 
391: 2. **Facades dropped**: `ValueFacade` and `TriggerFacade` replaced by Output array
392:    `[Scalar{cog}, Scalar{trigger}]`.
393: 
394: 3. **Output enum created**: `Value = 0`, `Trigger = 1`.
395: 
396: 4. **Algorithm**: `Update(double)` converted line-by-line. `Calculate()` helper preserved.
397:    `Array.Copy` replaced with explicit loop.
398: 
399: 5. **Priming**: Requires `length + 1` samples. First `length` fill the window; at index
400:    `length - 1` the initial CoG is computed and stored as `valuePrevious`; at index
401:    `length` the indicator becomes primed.
402: 
403: 6. **Mnemonic**: `cog(10, hl/2)` for default params. The `, hl/2` comes from
404:    `BarMedianPrice` being non-default relative to `DefaultBarComponent` (which is
405:    `BarClosePrice`).
406: 
407: 7. **Tests**: 252-entry reference data preserved. Serialization/ToString/Reset tests dropped.
408:    Added entity update tests, metadata tests, NaN tests.
409: 
410: ### Files Produced
411: 
412: **Go (5 files):**
413: - `centerofgravityoscillatorparams.go` — `Params` struct with `Length`, `BarComponent`, `QuoteComponent`, `TradeComponent`
414: - `centerofgravityoscillatoroutput.go` — `Output` enum: `Value`, `Trigger`
415: - `centerofgravityoscillatoroutput_test.go` — Output enum string tests
416: - `centerofgravityoscillator.go` — Main implementation (273 lines)
417: - `centerofgravityoscillator_test.go` — Tests with 252-entry data (659 lines)
418: 
419: **TS (4 files):**
420: - `center-of-gravity-oscillator-params.ts` — `CenterOfGravityOscillatorParams` interface
421: - `center-of-gravity-oscillator-output.ts` — `CenterOfGravityOscillatorOutput` enum
422: - `center-of-gravity-oscillator.ts` — Main implementation (227 lines)
423: - `center-of-gravity-oscillator.spec.ts` — Tests with 252-entry data
424: 
425: ---
426: 
427: ## Bar-Based Indicators (Non-LineIndicator Pattern)
428: 
429: Some indicators require bar data (high, low, close) rather than a single scalar component.
430: These indicators implement the `Indicator` interface directly without using `LineIndicator`
431: embedding/inheritance. Example: **TrueRange**.
432: 
433: ### Key Differences from LineIndicator
434: 
435: | Aspect | LineIndicator | Bar-Based (e.g., TrueRange) |
436: |---|---|---|
437: | Base type | Embeds `LineIndicator` (Go) / `extends LineIndicator` (TS) | Implements `Indicator` directly |
438: | Core method | `Update(float64)` / `update(number)` — single scalar | `Update(close, high, low float64)` — multiple values |
439: | Bar handling | Extracts one component via `barComponentFunc` | Extracts H, L, C directly from bar |
440: | Scalar/Quote/Trade | Routed through component function | Use single value as H=L=C substitute |
441: | Params | Typically has `Length`, component fields | May be empty (parameterless) |
442: | Components | `BarComponent`, `QuoteComponent`, `TradeComponent` | None — bar fields accessed directly |
443: 
444: ### Go Pattern
445: 
446: ```go
447: type TrueRange struct {
448:     mu            sync.RWMutex
449:     previousClose float64
450:     value         float64
451:     primed        bool
452: }
453: 
454: func (tr *TrueRange) Update(close, high, low float64) float64 { ... }
455: func (tr *TrueRange) UpdateSample(sample float64) float64 {
456:     return tr.Update(sample, sample, sample)
457: }
458: func (tr *TrueRange) UpdateBar(sample *entities.Bar) core.Output {
459:     output := make([]any, 1)
460:     output[0] = entities.Scalar{Time: sample.Time, Value: tr.Update(sample.Close, sample.High, sample.Low)}
461:     return output
462: }
463: func (tr *TrueRange) UpdateScalar(sample *entities.Scalar) core.Output {
464:     v := sample.Value
465:     output := make([]any, 1)
466:     output[0] = entities.Scalar{Time: sample.Time, Value: tr.Update(v, v, v)}
467:     return output
468: }
469: ```
470: 
471: ### TS Pattern
472: 
473: ```typescript
474: export class TrueRange implements Indicator {
475:     public update(close: number, high: number, low: number): number { ... }
476:     public updateSample(sample: number): number {
477:         return this.update(sample, sample, sample);
478:     }
479:     public updateBar(sample: Bar): IndicatorOutput {
480:         const scalar = new Scalar();
481:         scalar.time = sample.time;
482:         scalar.value = this.update(sample.close, sample.high, sample.low);
483:         return [scalar];
484:     }
485: }
486: ```
487: 
488: ### Test Data Extraction
489: 
490: For large test datasets (e.g., 252-entry TA-Lib arrays), extract data **programmatically**
491: from the C# test file using a Python script rather than manual transcription:
492: 
493: ```python
494: import re
495: pattern = rf'readonly List<double> {name} = new List<double>\s*\{{(.*?)\}};'
496: # Remove C-style comments before extracting numbers
497: body = re.sub(r'/\*.*?\*/', '', body)
498: ```
499: 
500: This avoids transcription errors that can be very hard to debug.
501: 
502: ---
503: 
504: ## Composite Indicators (Indicator-inside-Indicator Pattern)
505: 
506: Some indicators internally create and use other indicator instances. Example: **AverageTrueRange**
507: creates an internal **TrueRange** instance and delegates bar processing to it.
508: 
509: ### Key Points
510: 
511: - The inner indicator is a private field, created in the constructor
512: - The outer indicator calls the inner indicator's `Update()` method and processes the result
513: - Both Go and TS follow the same pattern: import the inner indicator's package and instantiate it
514: - The outer indicator manages its own priming state independently of the inner indicator
515: - In Go, the inner indicator's mutex is separate — no nested locking issues because `Update()`
516:   on the inner indicator acquires/releases its own lock before the outer lock is held
517: 
518: ### Go Example (AverageTrueRange)
519: 
520: ```go
521: import "zpano/indicators/welleswilder/truerange"
522: 
523: type AverageTrueRange struct {
524:     mu        sync.RWMutex
525:     trueRange *truerange.TrueRange  // internal indicator instance
526:     // ... other fields
527: }
528: 
529: func NewAverageTrueRange(length int) (*AverageTrueRange, error) {
530:     return &AverageTrueRange{
531:         trueRange: truerange.NewTrueRange(),
532:         // ...
533:     }, nil
534: }
535: 
536: func (a *AverageTrueRange) Update(close, high, low float64) float64 {
537:     trueRangeValue := a.trueRange.Update(close, high, low)  // delegate to inner
538:     // ... apply Wilder smoothing to trueRangeValue
539: }
540: ```
541: 
542: ### TS Example (AverageTrueRange)
543: 
544: ```typescript
545: import { TrueRange } from '../true-range/true-range';
546: 
547: export class AverageTrueRange implements Indicator {
548:     private readonly trueRange: TrueRange;
549: 
550:     constructor(length: number) {
551:         this.trueRange = new TrueRange();
552:     }
553: 
554:     public update(close: number, high: number, low: number): number {
555:         const trueRangeValue = this.trueRange.update(close, high, low);
556:         // ... apply Wilder smoothing to trueRangeValue
557:     }
558: }
559: ```
560: 
561: **Important:** Call the inner indicator's `Update()` **before** acquiring the outer lock in Go,
562: or call it inside the lock if the inner indicator uses its own separate mutex (as TrueRange does).
563: In the ATR implementation, `trueRange.Update()` is called inside the outer lock — this works
564: because TrueRange's mutex is independent.
565: 
566: ### Composing a Multi-Output Inner Indicator
567: 
568: When the inner indicator is itself multi-output (implements `Indicator` directly and
569: returns a tuple), the outer indicator typically wants to **re-expose some of the inner
570: outputs alongside its own**. Example: **SineWave** composes **DominantCycle** and
571: publishes 5 outputs: `Value`, `Lead`, `Band` (its own) + `DominantCyclePeriod`,
572: `DominantCyclePhase` (forwarded from the inner).
573: 
574: Rules:
575: 
576: 1. **Instantiate the inner using the `Params` constructor**, not `Default`, so the outer
577:    can forward parameters correctly (estimator params, `alphaEmaPeriodAdditional`,
578:    components, etc.).
579: 2. **Resolve component defaults in the outer first**, then pass the **resolved** values
580:    explicitly to the inner. This keeps mnemonics aligned across outputs — e.g., if the
581:    user overrides `BarComponent = Median`, **both** `sw(0.330, hl/2)` and the inner's
582:    `dcp(0.330, hl/2)` carry the suffix. If you skip this, the inner resolves its own
583:    defaults and the two mnemonics drift apart.
584: 3. **Call the inner's `Update()` before acquiring the outer lock** (inner manages its
585:    own mutex).
586: 4. **Forward outputs by index** into the outer's output tuple — don't re-compute. The
587:    outer's `updateEntity` assembles all N `Scalar`/`Band` entities from the inner's
588:    period/phase plus its own Value/Lead/Band.
589: 5. **Keep a single top-level `Mnemonic` / `Description`** on the outer's `Metadata`
590:    (the primary output's). Each forwarded inner output carries its own per-output
591:    mnemonic unchanged.
592: 
593: ```go
594: // Go — inside newSineWave, after resolving bc/qc/tc to non-zero defaults:
595: dc, err := dominantcycle.NewDominantCycleParams(&dominantcycle.Params{
596:     AlphaEmaPeriodAdditional: p.AlphaEmaPeriodAdditional,
597:     EstimatorType:            p.EstimatorType,
598:     EstimatorParams:          p.EstimatorParams,
599:     BarComponent:             bc, // resolved, not p.BarComponent
600:     QuoteComponent:           qc,
601:     TradeComponent:           tc,
602: })
603: ```
604: 
605: ```ts
606: // TS — inside the private SineWave constructor:
607: this.dominantCycle = DominantCycle.fromParams({
608:     alphaEmaPeriodAdditional: params.alphaEmaPeriodAdditional,
609:     estimatorType: params.estimatorType,
610:     estimatorParams: params.estimatorParams,
611:     barComponent: bc, quoteComponent: qc, tradeComponent: tc,
612: });
613: ```
614: 
615: ### Composing a Raw HTCE vs Wrapping `DominantCycle`
616: 
617: When porting an MBST indicator that internally owns a `HilbertTransformerCycleEstimator`
618: (HTCE), you must decide whether to:
619: 
620: **(a) Compose the already-merged `DominantCycle`** — when the MBST indicator feeds the
621: HTCE's **smoothed** output (`htce.Smoothed`) into its own buffers, same as MBST's
622: `DominantCyclePeriod`/`DominantCyclePhase` do. SineWave is the canonical example.
623: 
624: **(b) Compose a raw `HilbertTransformerCycleEstimator` directly** — when the MBST
625: indicator pushes the **raw, unsmoothed** input sample into its own buffer, not
626: `htce.Smoothed`. `HilbertTransformerInstantaneousTrendLine` (HTITL) is the canonical
627: example: its buffer holds raw prices and only uses the HTCE for its period output.
628: 
629: ### How to Tell Which Pattern Applies
630: 
631: Read the MBST indicator's `Update(double sample)` carefully. Look at what gets pushed
632: into the indicator's own circular buffer:
633: 
634: | MBST buffer fed with… | Zpano pattern |
635: |---|---|
636: | `htce.Smoothed` (WMA-smoothed price) | Compose `DominantCycle` — the smoothing is already inside DC |
637: | Raw `sample` (the un-smoothed input) | Compose raw HTCE via `hilberttransformer.NewCycleEstimator` / `createEstimator` |
638: | A different transform (e.g., hi-low range) | Compose raw HTCE; replicate transform in the outer |
639: 
640: ### Implications for Pattern (b)
641: 
642: When composing a raw HTCE:
643: 
644: 1. **Replicate the α-EMA period smoothing inline** — DC does `Periodᵢ = α·RawPeriodᵢ +
645:    (1−α)·Periodᵢ₋₁` internally; when you're not wrapping DC you must write that loop in
646:    the outer. Fields: `alphaEmaPeriodAdditional`, `oneMinAlphaEmaPeriodAdditional`,
647:    `smoothedPeriod`.
648: 2. **Expose `DominantCyclePeriod` (smoothed) directly** — the outer owns the smoothed
649:    period and publishes it as its own output, not forwarded from DC. Mnemonic template
650:    stays `dcp(α%s%s)` for consistency with DC/SineWave.
651: 3. **Use `htce.Period()` on each bar past priming** — no `htce.Smoothed()` involvement
652:    in the averaging window; that's pattern (a).
653: 4. **Warm-up period is still `MaxPeriod*2 = 100`** — MBST convention for anything
654:    driven by an HTCE with the default auto-warmup. See "Warm-Up Period Defaults When
655:    Wrapping an HTCE" below.
656: 5. **The component convention from the containing MBST class still applies** — e.g.,
657:    HTITL defaults to `BarMedianPrice` (like CoG/SineWave), so the component always shows
658:    in the mnemonic: `htitl(0.330, 4, 1.000, hl/2)`.
659: 
660: ### Go Sketch (HTITL-style)
661: 
662: ```go
663: type X struct {
664:     htce                           hilberttransformer.CycleEstimator
665:     alphaEmaPeriodAdditional       float64
666:     oneMinAlphaEmaPeriodAdditional float64
667:     smoothedPeriod                 float64
668:     input                          []float64 // RAW samples, not smoothed
669:     // ...
670: }
671: 
672: func (s *X) Update(sample float64) (float64, float64) {
673:     s.htce.Update(sample)
674:     s.pushInput(sample) // RAW sample, not s.htce.Smoothed()
675: 
676:     if s.primed {
677:         s.smoothedPeriod = s.alphaEmaPeriodAdditional*s.htce.Period() +
678:             s.oneMinAlphaEmaPeriodAdditional*s.smoothedPeriod
679:         // ... use s.input buffer + s.smoothedPeriod ...
680:     }
681: }
682: ```
683: 
684: ### TS Sketch
685: 
686: ```typescript
687: this.htce = createEstimator(params.estimatorType, params.estimatorParams);
688: // ...
689: this.htce.update(sample);
690: this.pushInput(sample); // RAW
691: 
692: if (this.primed) {
693:     this.smoothedPeriod = this.alpha * this.htce.period + this.oneMinAlpha * this.smoothedPeriod;
694:     // ...
695: }
696: ```
697: 
698: ### Don't Mix Patterns
699: 
700: Do **not** wrap `DominantCycle` and then also create a separate `HilbertTransformerCycleEstimator`
701: to get a raw input path — that would double the HTCE cost and desync the period
702: smoothing. Pick one pattern per indicator based on what MBST's buffer holds.
703: 
704: ---
705: 
706: ### Band Output Semantics When Wrapping MBST's `Band`
707: 
708: MBST's `Band(DateTime, firstValue, secondValue)` carries no upper/lower semantic — it's
709: just a pair. When a merged zpano indicator emits a `Band` output, the **wrapping
710: indicator must choose** which value becomes `Upper` and which becomes `Lower`.
711: 
712: Convention: **primary/fast value → `Upper`, secondary/slow value → `Lower`**. This
713: matches MAMA (`Upper: mama, Lower: fama`) and SineWave (`Upper: value, Lower: lead`).
714: Document the choice in a short comment above the `Band{}` construction so reviewers
715: don't have to cross-reference MBST to confirm ordering.
716: 
717: ---
718: 
719: ## Volume-Aware Indicators (UpdateWithVolume Pattern)
720: 
721: Some indicators require both a price sample and volume. Since `LineIndicator` only
722: supports `Update(float64)` / `update(number)`, these indicators need special handling.
723: 
724: **Example: MoneyFlowIndex (MFI)**
725: 
726: ### Pattern
727: 
728: 1. **Embed `LineIndicator`** as usual (single scalar output).
729: 2. **Add `UpdateWithVolume(sample, volume float64)`** as the real computation method.
730: 3. **`Update(sample)`** delegates to `UpdateWithVolume(sample, 1)` (volume=1 fallback).
731: 4. **Shadow/override `UpdateBar`** to extract both price (via `barFunc`) AND volume from
732:    the bar, then call `UpdateWithVolume`.
733: 
734: ### Go: Shadow `UpdateBar`
735: 
736: ```go
737: // MoneyFlowIndex shadows LineIndicator.UpdateBar to extract volume.
738: func (s *MoneyFlowIndex) UpdateBar(sample *entities.Bar) core.Output {
739:     s.mu.Lock()
740:     defer s.mu.Unlock()
741:     price := s.barFunc(sample)
742:     v := s.updateWithVolume(price, sample.Volume)
743:     output := make([]any, 1)
744:     output[0] = entities.Scalar{Time: sample.Time, Value: v}
745:     return output
746: }
747: ```
748: 
749: The shadowed `UpdateBar` on the concrete type takes precedence over `LineIndicator.UpdateBar`
750: when called on `*MoneyFlowIndex` directly or through the `Indicator` interface.
751: 
752: ### TS: Override `updateBar`
753: 
754: ```typescript
755: public override updateBar(sample: Bar): IndicatorOutput {
756:     const price = this.barFunc(sample);
757:     const v = this.updateWithVolume(price, sample.volume);
758:     const scalar = new Scalar();
759:     scalar.time = sample.time;
760:     scalar.value = v;
761:     return [scalar];
762: }
763: ```
764: 
765: ### Default BarComponent
766: 
767: MFI defaults to `BarTypicalPrice` / `BarComponent.Typical` (not `BarClosePrice`),
768: matching the C# default of `OhlcvComponent.TypicalPrice`.
769: 
770: ### Mnemonic
771: 
772: MFI uses `mfi(LENGTH)` — no component suffix, matching C# behavior.
773: 
774: ### Testing Volume-Aware Indicators
775: 
776: Test with two datasets:
777: 1. **Real volume** — `updateWithVolume(price, volume)` against expected values.
778: 2. **Volume=1** — `update(price)` (which uses volume=1) against a separate expected dataset.
779: 3. **UpdateBar test** — Feed `Bar` entities with real OHLCV data, verify first computed
780:    value matches the real-volume expected data.
781: 
782: ---
783: 
784: ## Test Data from Julia/CSV Reference (No C# Tests)
785: 
786: Some MBST indicators lack C# unit tests but have Julia reference implementations and CSV
787: test data files. Example: `SuperSmoother` has `ehlers_super_smoother.jl` and
788: `test_3-3_Supersmoother.csv`.
789: 
790: ### Key Differences: MBST vs Julia Priming
791: 
792: - **MBST priming:** Seeds previous filter values to the first sample value on count==1.
793: - **Julia priming:** Seeds previous filter values to zero (array initialized to zeros).
794: - After sufficient samples (~30-60), both converge to the same values.
795: 
796: ### CSV Test Data Strategy
797: 
798: 1. **Go tests:** Read the CSV file at test time using `encoding/csv`. Reference the CSV
799:    via a relative path (e.g., `../../../../mbst-to-convert/.../file.csv`).
800: 2. **TS tests:** Embed a representative subset (e.g., first 200 rows) directly in the
801:    spec file as `const` arrays (CSV reading at test time is more complex in TS).
802: 3. **Skip early rows:** Due to priming differences, skip the first N rows (e.g., 60)
803:    where MBST and Julia outputs diverge.
804: 4. **Tolerance:** Use a generous tolerance (e.g., 2.5) to account for:
805:    - CSV data rounded to 2 decimal places
806:    - Different priming initialization (MBST seeds to first sample, Julia to zero)
807:    - Convergence lag in the early samples
808: 5. **Julia test reference:** Check the Julia test file (`*_test.jl`) for the skip count
809:    and rounding precision used in the original Julia validation.
810: 
811: ---
812: 
813: ## Merging Multiple MBST Indicators Into One Zpano Indicator
814: 
815: Some MBST indicators are closely related (share a common internal computation) but are
816: exposed as **separate** C# classes. In zpano they should be merged into a **single
817: multi-output indicator** to avoid duplicated state/warm-up and simplify the API.
818: 
819: **Example: MBST `DominantCyclePeriod` + `DominantCyclePhase` → zpano `DominantCycle`** with
820: three outputs: `RawPeriod`, `Period` (EMA-smoothed), `Phase`.
821: 
822: ### When to Merge
823: 
824: Merge when all of the following hold:
825: 1. The MBST classes each own their own `HilbertTransformerCycleEstimator` (or other
826:    expensive internal state) but conceptually **compute the same thing**.
827: 2. They share the same primary parameter(s) (e.g., `alphaEmaPeriodAdditional` and
828:    estimator params).
829: 3. Users typically want multiple outputs together (reading phase requires the period
830:    anyway).
831: 
832: ### Pattern
833: 
834: - **One class, N outputs** — the indicator implements `Indicator` directly (MAMA-style),
835:   owns `barFunc`/`quoteFunc`/`tradeFunc`, and has an `updateEntity` helper emitting N
836:   `Scalar` entities (or `Band`, etc.) in the fixed output order.
837: - **One estimator instance**, one priming flag, one warm-up period for the whole
838:   indicator. The `primed` transition seeds all derived state in one shot.
839: - **`Update(sample)` returns a tuple** (Go: multi-return `(a, b, c)`, TS:
840:   `[a, b, c]` tuple). Each output has its own mnemonic; the indicator's top-level
841:   `Mnemonic()` returns the "primary" output's mnemonic (e.g., Period, not RawPeriod).
842: - **Output enum** lists all N outputs. Go: `RawPeriod Output = iota + 1, Period, Phase,
843:   outputLast`. TS: numeric enum with explicit `= 0, = 1, = 2`.
844: - **Single entry in `core.IndicatorType`** — one `DominantCycle` constant, not three.
845: 
846: ### Constructor Flavors
847: 
848: Follow MAMA: provide `NewDominantCycleDefault` / `static default()` and
849: `NewDominantCycleParams` / `static fromParams()`. A private constructor does the actual
850: work. Avoid length-vs-smoothing-factor duplication when the indicator has only a single
851: scalar parameter — use one `fromParams` flavor.
852: 
853: ### Merging on Top of Another Merged Indicator
854: 
855: The MBST indicators you're merging may themselves each instantiate an already-merged
856: zpano indicator (e.g., `SineWave`, `SineWaveLead`, `SineWaveBand` each own a
857: `DominantCyclePeriod`/`DominantCyclePhase` pair — already merged in zpano as
858: `DominantCycle`). In that case:
859: 
860: - **Don't duplicate the inner estimator in the outer.** Compose the already-merged
861:   inner indicator (see "Composing a Multi-Output Inner Indicator" above).
862: - **Forward the inner's relevant outputs** as additional outputs of the outer, so users
863:   don't need to instantiate both. SineWave's 5 outputs = its own 3 (Value/Lead/Band) +
864:   2 forwarded from DominantCycle (Period/Phase).
865: - **Reuse the inner's reference test arrays** where MBST's tests happen to embed them
866:   (MBST's SineWave test file contains the same `dcPeriod`/`dcPhase` arrays as the
867:   DominantCycle tests — port them once into the outer's `_test`).
868: 
869: ---
870: 
871: ## Warm-Up Period Defaults When Wrapping an HTCE
872: 
873: The `HilbertTransformerCycleEstimator` has two different default warm-up values depending
874: on how it's created:
875: 
876: | Code path | Default `WarmUpPeriod` |
877: |---|---|
878: | MBST C# constructor when caller passes `0` | `MaxPeriod * 2 = 100` |
879: | zpano HTCE internal auto-default (Go/TS) when caller passes `0` | `smoothingLengthPlus3HtLength ≈ 25` |
880: 
881: **This means the zpano HTCE primes earlier by default than MBST.** When porting an MBST
882: indicator that wraps HTCE and relies on the MBST default (`warmUpPeriod = MaxPeriod * 2`),
883: the zpano indicator's default factory **must explicitly set `warmUpPeriod: 100`** in the
884: estimator params — otherwise the port primes ~75 samples earlier and diverges from the
885: MBST reference test data.
886: 
887: ```go
888: // Go: inside NewDominantCycleDefault
889: &hilberttransformer.CycleEstimatorParams{
890:     SmoothingLength:           4,
891:     AlphaEmaQuadratureInPhase: 0.2,
892:     AlphaEmaPeriod:            0.2,
893:     WarmUpPeriod:              100, // NOT 0 — MBST default is MaxPeriod*2
894: }
895: ```
896: 
897: ```typescript
898: // TS: inside static default()
899: estimatorParams: {
900:     smoothingLength: 4,
901:     alphaEmaQuadratureInPhase: 0.2,
902:     alphaEmaPeriod: 0.2,
903:     warmUpPeriod: 100, // NOT omitted — MBST default is MaxPeriod*2
904: }
905: ```
906: 
907: When writing tests that call `NewDominantCycleParams(...)` with arbitrary estimator
908: params, passing `WarmUpPeriod: 0` is fine — the HTCE auto-default kicks in and the test
909: isn't comparing against MBST reference data anyway (it's comparing mnemonic shape, error
910: handling, etc.).
911: 
912: ---
913: 
914: ## Handling Structurally-Divergent Reference Data
915: 
916: Occasionally the reference data you're validating against comes from an implementation
917: that is **structurally different** from MBST's — e.g., TA-Lib Excel templates that
918: produce output from bar 0 by smoothing through zeros, while the MBST port follows a
919: strict priming convention (NaN until HTCE primed, then seed-EMA).
920: 
921: When this happens, an exact match is **algorithmically impossible** for the early portion
922: of the output series. Both implementations converge later because each subsequent EMA
923: step shrinks the seed error by `(1-α)`.
924: 
925: ### Detection
926: 
927: - You've ported the algorithm line-by-line from MBST/C# and the code is correct.
928: - Early output values diverge from reference by a lot (e.g., 20+ units); late values
929:   match to 1e-5 or better.
930: - Error decays geometrically, not randomly.
931: 
932: ### Test Strategy
933: 
934: 1. **Add a `settleSkip` constant** (tuned empirically) — the index past which
935:    convergence is tight enough to assert.
936: 2. **Sanity-check earlier indices** — assert output is finite / non-NaN, but skip the
937:    value comparison.
938: 3. **Use a realistic tolerance** (e.g., `1e-4`) that accommodates both residual seed
939:    error at `settleSkip` and late-series floating-point accumulation.
940: 4. **Document the rationale in a block comment** above `settleSkip` — future maintainers
941:    must understand this is algorithmic, not a port bug.
942: 
943: ```go
944: const (
945:     skip       = 9   // TradeStation convention — skip first N bars.
946:     settleSkip = 177 // Samples required for EMA to converge past structural reference mismatch.
947: )
948: 
949: for i := skip; i < len(input); i++ {
950:     _, period, _ := dc.Update(input[i])
951:     if math.IsNaN(period) || i < settleSkip {
952:         continue
953:     }
954:     if math.Abs(expPeriod[i]-period) > tolerance {
955:         t.Errorf(...)
956:     }
957: }
958: ```
959: 
960: ### Phase/Angle Comparisons — Modulo 360
961: 
962: If the reference produces phase values **outside** the MBST port's `(-90°, 360°]` range
963: (e.g., 639.09°, -42.95°), you cannot compare directly. Use a `phaseDiff` helper that
964: computes the shortest signed angular difference modulo 360:
965: 
966: ```go
967: func phaseDiff(a, b float64) float64 {
968:     d := math.Mod(a-b, 360)
969:     if d > 180 {
970:         d -= 360
971:     } else if d <= -180 {
972:         d += 360
973:     }
974:     return d
975: }
976: ```
977: 
978: Then assert `math.Abs(phaseDiff(expected, actual)) < tolerance`.
979: 
980: Same helper applies in TS, using `(a - b) % 360` (JavaScript's `%` is remainder, which
981: preserves the sign of the dividend — same semantics as `math.Mod` for this purpose).
982: 
983: ---
984: 
985: ## Exposing Wrapped-Indicator Internals via Accessors
986: 
987: When a new indicator wraps an existing merged indicator (e.g., `TrendCycleMode` wraps
988: `DominantCycle`) and needs access to internal state the inner indicator already computes
989: — **add read-only accessors on the inner indicator** rather than duplicating the
990: computation in the outer.
991: 
992: Common examples on `DominantCycle`:
993: 
994: | Need | Add to inner | Avoid |
995: |---|---|---|
996: | WMA-smoothed price (`htce.Smoothed`) for the trendline | `SmoothedPrice() float64` / `get smoothedPrice(): number` | Re-instantiating a second HTCE in the outer |
997: | `MaxPeriod` constant for sizing the raw-input buffer | `MaxPeriod() int` / `get maxPeriod(): number` | Hard-coding `50` in the outer |
998: 
999: Rules:
1000: 
1001: 1. Accessors are **read-only** and do not acquire locks when returning immutable
1002:    constants (e.g., `MaxPeriod`). For mutable state (e.g., `SmoothedPrice` updated each
1003:    `Update` call), use a `RLock`/`RUnlock` in Go; TS has no locking.
1004: 2. Add matching unit tests on the inner indicator confirming the accessor returns the
1005:    expected value after priming.
1006: 3. Prefer accessors over widening the inner's `Update()` return tuple — adding more
1007:    return values would force every existing caller to change.
1008: 
1009: ---
1010: 
1011: ## Go Error-Wrapping Depth When Wrapping Sub-Indicators
1012: 
1013: When an outer constructor (e.g., `newTrendCycleMode`) internally calls another
1014: indicator's constructor (`dominantcycle.NewDominantCycleParams`), any validation error
1015: returned by the inner gets **double-wrapped** by the outer's own `fmt.Errorf("invalid X
1016: parameters: %w", err)` prefix.
1017: 
1018: For component errors (bar/quote/trade), the expected error string in tests becomes:
1019: 
1020: ```
1021: invalid trend cycle mode parameters: invalid dominant cycle parameters: 9999: unknown bar component
1022: ```
1023: 
1024: Not:
1025: ```
1026: invalid trend cycle mode parameters: 9999: unknown bar component
1027: ```
1028: 
1029: Write the Go tests' expected error constants with the full double-prefix so the assertion
1030: matches reality. TypeScript doesn't have this issue because TS component helpers
1031: silently default to close (see next section).
1032: 
1033: ---
1034: 
1035: ## TS Component Helpers Don't Throw
1036: 
1037: A cross-platform portability trap: Go's `entities.BarComponentValue(c)`,
1038: `QuoteComponentValue(c)`, `TradeComponentValue(c)` return an error for unknown
1039: component enum values (typically `9999` in tests). **TypeScript's equivalents
1040: (`barComponentValue`, `quoteComponentValue`, `tradeComponentValue`) do not throw** — they
1041: silently fall back to the close/mid/price default.
1042: 
1043: Implications:
1044: 
1045: 1. **Skip "invalid bar/quote/trade component" tests in TS specs** — there's no error
1046:    path to exercise. Port all other param validation tests (α, length, ranges, etc.)
1047:    as usual.
1048: 2. **Existing TS specs follow this convention** (SineWave, DominantCycle, CenterOfGravity
1049:    all omit invalid-component tests). Don't add them to new indicators either.
1050: 3. **Go specs should keep the component error tests** — they exercise a real code path.
1051: 
1052: ---
1053: 
1054: ## Tuple-Output Indicator Spec Template (8 Outputs, TrendCycleMode Pattern)
1055: 
1056: When the outer indicator emits more than the typical 3-5 outputs (e.g., TrendCycleMode's
1057: 8-output tuple), the TS spec pattern is:
1058: 
1059: 1. **Destructure with positional blanks** — use `const [, , , , sine] = x.update(v)` for
1060:    5th element; avoid naming unused tuple elements.
1061: 2. **Single `checkOutput(out: any[])` helper** verifies `out.length === 8` and each is a
1062:    `Scalar` with matching `time`. No `Band`-specific branch needed when all outputs are
1063:    Scalar.
1064: 3. **Data arrays ported verbatim from Go test file** — replace `math.NaN()` with
1065:    `Number.NaN`, drop Go `//nolint` tags.
1066: 4. **Phase array comparison uses mod-360 helper** (see above). Don't skip NaN-expected
1067:    indices globally; skip per-element with `if (Number.isNaN(expected[i])) continue`.
1068: 5. **Reference-value array can be shorter than input array** — e.g., TCM's `expectedValue`
1069:    has 201 entries while `input` has 252. Guard with `if (i >= limit) continue` inside the
1070:    loop instead of shortening the outer iteration range (keeps the indicator fed with the
1071:    full input sequence).

(End of file - total 1071 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 62.1s)

_Thinking:_

 I'm adding a Corona/Heatmap section to summarize the key insights from the analysis.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "oldString": "5. **Reference-value array can be shorter than input array** — e.g., TCM's `expectedValue`\n   has 201 entries while `input` has 252. Guard with `if (i >= limit) continue` inside the\n   loop instead of shortening the outer iteration range (keeps the indicator fed with the\n   full input sequence).",
  "newString": "5. **Reference-value array can be shorter than input array** — e.g., TCM's `expectedValue`\n   has 201 entries while `input` has 252. Guard with `if (i >= limit) continue` inside the\n   loop instead of shortening the outer iteration range (keeps the indicator fed with the\n   full input sequence).\n\n---\n\n## Heatmap-Output Indicators (Corona Suite Pattern)\n\nSome Ehlers indicators emit a **`Heatmap`** output (a 2-D intensity grid over time) in\naddition to scalar outputs. The MBST Corona family (`CoronaSpectrum`,\n`CoronaSignalToNoiseRatio`, `CoronaSwingPosition`, `CoronaTrendVigor`) is the canonical\nreference. Key conventions learned porting them to zpano:\n\n### Shared Helper (Not a Registered Indicator)\n\nThe MBST `Corona` base class isn't an indicator itself — it's a reusable helper\nencapsulating the highpass filter, filter-bank, amplitude-squared matrix, and dominant-\ncycle-median logic. Port it as a plain class/struct in a `corona/` subpackage, **do not\nregister a `core.IndicatorType`** for it, and do not give it an `update…Entity` path.\nDownstream indicators instantiate the helper by composition.\n\nExpose the internals the wrappers need as **public accessors**, same rule as\n\"Exposing Wrapped-Indicator Internals via Accessors\":\n\n| Wrapper needs | Helper exposes |\n|---|---|\n| Sizing the sample buffer (CTV) | `maximalPeriodTimesTwo` (Go: `MaximalPeriodTimesTwo()`, TS: `maximalPeriodTimesTwo` getter) |\n| DC median for scalar output | `dominantCycleMedian` |\n| Filter-bank amplitude matrix | `maximalAmplitudeSquared` |\n| Per-period filter outputs | `filterBank`, `filterBankLength` |\n\n### Empty Heatmap Invariant\n\nWhen the indicator isn't primed yet (or the first sample for store-only indicators),\n`update()` must still return a **well-formed heatmap with axis metadata but empty\nvalues**:\n\n- Go: `outputs.NewEmptyHeatmap(xAxis, yAxis, resolution, time)` — populates axes, leaves\n  `Values` nil/empty, `IsEmpty()` returns true.\n- TS: `Heatmap.newEmptyHeatmap(xAxis, yAxis, resolution, time)` — same semantics, plus a\n  `.isEmpty()` method.\n\n**Never return a null/zero heatmap.** Consumers rely on axes/resolution being present\nacross all bars so the UI can size the grid correctly from bar 0.\n\nScalar outputs during warm-up return `NaN` (Go `math.NaN()`, TS `Number.NaN`).\n\n### First-Sample Store-Only Pattern\n\nCSNR/CSwing/CTV all share an \"isStarted\" flag: the **very first** sample is stored into\nthe buffer but no computation happens (empty heatmap + NaN scalar). From the second\nsample onward, normal primed-or-not logic takes over.\n\nTested with the standard 252-entry TA-Lib MAMA series; snapshot indices `{11, 12, 50,\n100, 150, 200, 251}` give good coverage of warm-up, early-primed, mid-series, and\ntail-series behavior.\n\n### Parameter Resolution Quirks (Per Indicator)\n\n- **CSpectrum** rounds user-supplied `minParam` **up** (`ceil`) and `maxParam` **down**\n  (`floor`) — preserves the integer raster count. Example: `min=8.7, max=40.4` →\n  `9, 40`.\n- **CSwing / CTV** substitute their defaults (`±5` / `±10`) for Min/Max **only when\n  both are zero** (unconfigured sentinel). If either is non-zero, both user values are\n  honored. The \"both zero\" detection matches MBST's `if (min == 0 && max == 0)`.\n- **CSNR** coefficient sum in the signal EMA is `0.2 + 0.9 = 1.1`, **not** 1.0. This is\n  intentional per Ehlers; don't \"fix\" it.\n\n### Heatmap Resolution Formula\n\nResolution is `(rasterLength - 1) / (maxRaster - minRaster)` for raster-based heatmaps,\nor `(length - 1) / (maxParam - minParam)` for parameter-indexed heatmaps. Examples:\n\n| Indicator | rasterLength | min/max | resolution |\n|---|---|---|---|\n| CSpectrum | maxParam-minParam+1 = 25 | 6..30 | (25-1)/24 = 1.0 (but the indicator uses raster=24, producing (len-1)/24 = 2.0 over a len-49 raster) |\n| CSNR | 50 | 1..11 | 49/10 = 4.9 |\n| CSwing | 50 | -5..5 | 49/10 = 4.9 |\n| CTV | 50 | -10..10 | 49/20 = 2.45 |\n\nWhen in doubt, **replicate the Go reference value exactly** — the formula varies\nslightly per indicator and the Go implementation is the source of truth.\n\n### TrendVigor Lookback Edge Case\n\nCTV computes a lookback window as `int(DCM - 1)` where `DCM` is the dominant-cycle\nmedian. Two guards are required (MBST has the first but missed the second):\n\n1. **Lower bound:** guard `cyclePeriod >= 1` before using it (avoid zero-length loop).\n2. **Upper bound:** clamp at `sampleBuffer.length` — otherwise a long cycle period\n   during warm-up can over-index the buffer.\n\nGo's `int(x)` and TS's `Math.trunc(x)` both truncate toward zero, so they match for\npositive floats. For negative floats they also match (both truncate toward zero, unlike\n`Math.floor` which rounds toward -∞). Safe to translate directly.\n\n### DominantCycleBuffer Sentinel\n\nInitialize the DCM 5-element median buffer to `math.MaxFloat64` / `Number.MAX_VALUE`\nsentinels (MBST convention) so the partial median is well-defined before the buffer\nfills. Once primed, all 5 slots hold real values.\n\n### Heatmap Snapshot Testing\n\nFor heatmap outputs, snapshot tests validate:\n\n1. **Axis metadata** — `xAxis`, `yAxis` arrays, `resolution`, `time`.\n2. **`isEmpty()` transition** — empty during warm-up, non-empty once primed.\n3. **Scalar co-outputs** — DC/DCM/SNR/SP/TV values at fixed indices against Go\n   reference (tolerance `1e-4`).\n\nDon't snapshot the full `Values` grid verbatim — it's high-dimensional and noisy. Trust\nthe scalar co-output snapshots + axis/resolution invariants + `isEmpty()` transitions.\n\n### Component Triple Mnemonic on the Main Output\n\nCorona indicators default to `BarMedianPrice` (hl/2). The main heatmap output's\nmnemonic always carries the `, hl/2` suffix (pass `BarComponent.Median` explicitly to\n`ComponentTripleMnemonic`). Scalar co-outputs reuse the same component suffix —\nthey're computed from the same component, so the mnemonic must match.\n\nExamples (defaults):\n- `cspect(6, 20, 6, 30, 30, hl/2)` / `cspect-dc(30, hl/2)` / `cspect-dcm(30, hl/2)`\n- `csnr(50, 20, 1, 11, 30, hl/2)` / `csnr-snr(30, hl/2)`\n- `cswing(50, 20, -5, 5, 30, hl/2)` / `cswing-sp(30, hl/2)`\n- `ctv(50, 20, -10, 10, 30, hl/2)` / `ctv-tv(30, hl/2)`\n\n### Update Signature Quirk: CSNR Takes (sample, low, high)\n\n`CoronaSignalToNoiseRatio.update(sample, sampleLow, sampleHigh, time)` — note the\n**low-then-high** order, not high-then-low. `updateBar` naturally has both, so it\npasses `(barFunc(bar), bar.low, bar.high)`. `updateScalar`/`updateQuote`/`updateTrade`\nhave no high/low, so they pass `(v, v, v)` — collapsing H=L=sample causes SNR to fall\nback to `MinParameterValue` (SNR=0 would be invalid logarithmically)."
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 14.6s)

All work complete:

## Summary

**Phase 6 (TS) — complete, all green:**
- CoronaTrendVigor: 11/11 pass
- Full regression: **794/794 specs pass**

**Skill updated:** Added a "Heatmap-Output Indicators (Corona Suite Pattern)" section to `mbst-indicator-conversion/SKILL.md` covering:
- Shared helper pattern (not a registered indicator, accessors exposed)
- Empty heatmap invariant (axes always present, `isEmpty()` transition)
- First-sample store-only pattern
- Parameter resolution quirks (CSpectrum ceil/floor, CSwing/CTV both-zero substitution, CSNR intentional 1.1-sum)
- Resolution formulas per indicator
- TrendVigor lookback guards (`>=1` lower, `sampleBuffer.length` upper)
- DCM sentinel initialization
- Heatmap snapshot testing strategy
- Component triple mnemonic (all four carry `, hl/2`)
- CSNR signature quirk (low-then-high; H=L=sample fallback)

The entire Corona suite (Go + TS) is now shipped and documented. Nothing else outstanding.

---

