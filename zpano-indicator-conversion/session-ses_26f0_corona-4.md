# Split INDICATORS-ARCHITECTURE and CONVERTING-OLD-INDICATOR into skills?

**Session ID:** ses_26f009c55ffeeFyvE85VbCND9m
**Created:** 4/15/2026, 1:55:49 PM
**Updated:** 4/21/2026, 8:19:57 PM

---

## Assistant (Build · claude-opus-4.7 · 9.1s)

_Thinking:_

 I need to pause here because the heatmap indicators present a design challenge I haven't encountered before in zpano—they'll need to accept full OHLCV data in their Update method (since CoronaSignalToNoiseRatio uses High/Low for noise calculation) while also returning a Heatmap output, which is a different pattern than what I've been working with.

Let me check how the existing core.Indicator interface handles multi-output cases by looking at TCM as a reference.

Let me examine the TCM template and existing core indicator conventions before building the first heatmap-producing indicator.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode.go</path>
<type>file</type>
<content>1: package trendcyclemode
2: 
3: //nolint: gofumpt
4: import (
5: 	"fmt"
6: 	"math"
7: 	"sync"
8: 	"time"
9: 
10: 	"zpano/entities"
11: 	"zpano/indicators/core"
12: 	"zpano/indicators/core/outputs"
13: 	"zpano/indicators/johnehlers/dominantcycle"
14: 	"zpano/indicators/johnehlers/hilberttransformer"
15: )
16: 
17: // Deg2Rad converts degrees to radians.
18: const deg2Rad = math.Pi / 180.0
19: 
20: // TrendCycleMode is Ehlers' Trend-versus-Cycle Mode indicator.
21: //
22: // It wraps a DominantCycle (for instantaneous period / phase / WMA-smoothed price)
23: // and exposes eight outputs:
24: //
25: //   - Value: +1 in trend mode, -1 in cycle mode.
26: //   - IsTrendMode: 1 if trend mode is declared, 0 otherwise.
27: //   - IsCycleMode: 1 if cycle mode is declared, 0 otherwise.
28: //   - InstantaneousTrendLine: WMA-smoothed trend line.
29: //   - SineWave: sin(phase·Deg2Rad).
30: //   - SineWaveLead: sin((phase+45)·Deg2Rad).
31: //   - DominantCyclePeriod: smoothed dominant cycle period.
32: //   - DominantCyclePhase: dominant cycle phase, in degrees.
33: //
34: // Reference:
35: // John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 113-118.
36: type TrendCycleMode struct {
37: 	mu                         sync.RWMutex
38: 	mnemonic                   string
39: 	description                string
40: 	mnemonicTrend              string
41: 	descriptionTrend           string
42: 	mnemonicCycle              string
43: 	descriptionCycle           string
44: 	mnemonicITL                string
45: 	descriptionITL             string
46: 	mnemonicSine               string
47: 	descriptionSine            string
48: 	mnemonicSineLead           string
49: 	descriptionSineLead        string
50: 	mnemonicDCP                string
51: 	descriptionDCP             string
52: 	mnemonicDCPhase            string
53: 	descriptionDCPhase         string
54: 	dc                         *dominantcycle.DominantCycle
55: 	cyclePartMultiplier        float64
56: 	separationPercentage       float64
57: 	separationFactor           float64
58: 	trendLineSmoothingLength   int
59: 	coeff0                     float64
60: 	coeff1                     float64
61: 	coeff2                     float64
62: 	coeff3                     float64
63: 	trendline                  float64
64: 	trendAverage1              float64
65: 	trendAverage2              float64
66: 	trendAverage3              float64
67: 	sinWave                    float64
68: 	sinWaveLead                float64
69: 	prevDcPhase                float64
70: 	prevSineLeadWaveDifference float64
71: 	samplesInTrend             int
72: 	isTrendMode                bool
73: 	input                      []float64
74: 	inputLength                int
75: 	inputLengthMin1            int
76: 	primed                     bool
77: 	barFunc                    entities.BarFunc
78: 	quoteFunc                  entities.QuoteFunc
79: 	tradeFunc                  entities.TradeFunc
80: }
81: 
82: // NewTrendCycleModeDefault returns an instance of the indicator
83: // created using default values of the parameters.
84: func NewTrendCycleModeDefault() (*TrendCycleMode, error) {
85: 	const (
86: 		smoothingLength           = 4
87: 		alphaEmaQuadratureInPhase = 0.2
88: 		alphaEmaPeriod            = 0.2
89: 		alphaEmaPeriodAdditional  = 0.33
90: 		warmUpPeriod              = 100
91: 		trendLineSmoothingLength  = 4
92: 		cyclePartMultiplier       = 1.0
93: 		separationPercentage      = 1.5
94: 	)
95: 
96: 	return newTrendCycleMode(
97: 		hilberttransformer.HomodyneDiscriminator,
98: 		&hilberttransformer.CycleEstimatorParams{
99: 			SmoothingLength:           smoothingLength,
100: 			AlphaEmaQuadratureInPhase: alphaEmaQuadratureInPhase,
101: 			AlphaEmaPeriod:            alphaEmaPeriod,
102: 			WarmUpPeriod:              warmUpPeriod,
103: 		},
104: 		alphaEmaPeriodAdditional,
105: 		trendLineSmoothingLength,
106: 		cyclePartMultiplier,
107: 		separationPercentage,
108: 		0, 0, 0)
109: }
110: 
111: // NewTrendCycleModeParams returns an instance of the indicator created using supplied parameters.
112: func NewTrendCycleModeParams(p *Params) (*TrendCycleMode, error) {
113: 	return newTrendCycleMode(
114: 		p.EstimatorType, &p.EstimatorParams,
115: 		p.AlphaEmaPeriodAdditional,
116: 		p.TrendLineSmoothingLength,
117: 		p.CyclePartMultiplier,
118: 		p.SeparationPercentage,
119: 		p.BarComponent, p.QuoteComponent, p.TradeComponent)
120: }
121: 
122: //nolint:funlen,cyclop
123: func newTrendCycleMode(
124: 	estimatorType hilberttransformer.CycleEstimatorType,
125: 	estimatorParams *hilberttransformer.CycleEstimatorParams,
126: 	alphaEmaPeriodAdditional float64,
127: 	trendLineSmoothingLength int,
128: 	cyclePartMultiplier float64,
129: 	separationPercentage float64,
130: 	bc entities.BarComponent, qc entities.QuoteComponent, tc entities.TradeComponent,
131: ) (*TrendCycleMode, error) {
132: 	const (
133: 		invalid       = "invalid trend cycle mode parameters"
134: 		fmta          = "%s: α for additional smoothing should be in range (0, 1]"
135: 		fmttlsl       = "%s: trend line smoothing length should be 2, 3, or 4"
136: 		fmtcpm        = "%s: cycle part multiplier should be in range (0, 10]"
137: 		fmtsep        = "%s: separation percentage should be in range (0, 100]"
138: 		fmtw          = "%s: %w"
139: 		fmtnVal       = "tcm(%.3f, %d, %.3f, %.3f%%%s%s)"
140: 		fmtnTrend     = "tcm-trend(%.3f, %d, %.3f, %.3f%%%s%s)"
141: 		fmtnCycle     = "tcm-cycle(%.3f, %d, %.3f, %.3f%%%s%s)"
142: 		fmtnITL       = "tcm-itl(%.3f, %d, %.3f, %.3f%%%s%s)"
143: 		fmtnSine      = "tcm-sine(%.3f, %d, %.3f, %.3f%%%s%s)"
144: 		fmtnSineLead  = "tcm-sineLead(%.3f, %d, %.3f, %.3f%%%s%s)"
145: 		fmtnDCP       = "dcp(%.3f%s%s)"
146: 		fmtnDCPha     = "dcph(%.3f%s%s)"
147: 		four          = 4
148: 		alpha         = 0.2
149: 		descrVal      = "Trend versus cycle mode "
150: 		descrTrend    = "Trend versus cycle mode, is-trend flag "
151: 		descrCycle    = "Trend versus cycle mode, is-cycle flag "
152: 		descrITL      = "Trend versus cycle mode instantaneous trend line "
153: 		descrSine     = "Trend versus cycle mode sine wave "
154: 		descrSineLead = "Trend versus cycle mode sine wave lead "
155: 		descrDCP      = "Dominant cycle period "
156: 		descrDCPha    = "Dominant cycle phase "
157: 		tlslTwo       = 2
158: 		tlslThree     = 3
159: 		tlslFour      = 4
160: 		cpmMax        = 10.0
161: 		sepMax        = 100.0
162: 		sepDenom      = 100.0
163: 	)
164: 
165: 	if alphaEmaPeriodAdditional <= 0. || alphaEmaPeriodAdditional > 1. {
166: 		return nil, fmt.Errorf(fmta, invalid)
167: 	}
168: 
169: 	if trendLineSmoothingLength < tlslTwo || trendLineSmoothingLength > tlslFour {
170: 		return nil, fmt.Errorf(fmttlsl, invalid)
171: 	}
172: 
173: 	if cyclePartMultiplier <= 0. || cyclePartMultiplier > cpmMax {
174: 		return nil, fmt.Errorf(fmtcpm, invalid)
175: 	}
176: 
177: 	if separationPercentage <= 0. || separationPercentage > sepMax {
178: 		return nil, fmt.Errorf(fmtsep, invalid)
179: 	}
180: 
181: 	// SineWave-style default: BarMedianPrice always shown in mnemonic.
182: 	if bc == 0 {
183: 		bc = entities.BarMedianPrice
184: 	}
185: 
186: 	if qc == 0 {
187: 		qc = entities.DefaultQuoteComponent
188: 	}
189: 
190: 	if tc == 0 {
191: 		tc = entities.DefaultTradeComponent
192: 	}
193: 
194: 	// Build the inner DominantCycle with explicit components.
195: 	dcParams := &dominantcycle.Params{
196: 		EstimatorType:            estimatorType,
197: 		EstimatorParams:          *estimatorParams,
198: 		AlphaEmaPeriodAdditional: alphaEmaPeriodAdditional,
199: 		BarComponent:             bc,
200: 		QuoteComponent:           qc,
201: 		TradeComponent:           tc,
202: 	}
203: 
204: 	dc, err := dominantcycle.NewDominantCycleParams(dcParams)
205: 	if err != nil {
206: 		return nil, fmt.Errorf(fmtw, invalid, err)
207: 	}
208: 
209: 	// Compose the estimator moniker (same logic as DominantCycle / HTITL).
210: 	estimator, err := hilberttransformer.NewCycleEstimator(estimatorType, estimatorParams)
211: 	if err != nil {
212: 		return nil, fmt.Errorf(fmtw, invalid, err)
213: 	}
214: 
215: 	estimatorMoniker := ""
216: 	if estimatorType != hilberttransformer.HomodyneDiscriminator ||
217: 		estimatorParams.SmoothingLength != four ||
218: 		estimatorParams.AlphaEmaQuadratureInPhase != alpha ||
219: 		estimatorParams.AlphaEmaPeriod != alpha {
220: 		estimatorMoniker = hilberttransformer.EstimatorMoniker(estimatorType, estimator)
221: 		if len(estimatorMoniker) > 0 {
222: 			estimatorMoniker = ", " + estimatorMoniker
223: 		}
224: 	}
225: 
226: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
227: 
228: 	fmtArgs := []any{
229: 		alphaEmaPeriodAdditional, trendLineSmoothingLength,
230: 		cyclePartMultiplier, separationPercentage,
231: 		estimatorMoniker, componentMnemonic,
232: 	}
233: 
234: 	mnemonicVal := fmt.Sprintf(fmtnVal, fmtArgs...)
235: 	mnemonicTrend := fmt.Sprintf(fmtnTrend, fmtArgs...)
236: 	mnemonicCycle := fmt.Sprintf(fmtnCycle, fmtArgs...)
237: 	mnemonicITL := fmt.Sprintf(fmtnITL, fmtArgs...)
238: 	mnemonicSine := fmt.Sprintf(fmtnSine, fmtArgs...)
239: 	mnemonicSineLead := fmt.Sprintf(fmtnSineLead, fmtArgs...)
240: 	mnemonicDCP := fmt.Sprintf(fmtnDCP, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
241: 	mnemonicDCPha := fmt.Sprintf(fmtnDCPha, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
242: 
243: 	barFunc, err := entities.BarComponentFunc(bc)
244: 	if err != nil {
245: 		return nil, fmt.Errorf(fmtw, invalid, err)
246: 	}
247: 
248: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
249: 	if err != nil {
250: 		return nil, fmt.Errorf(fmtw, invalid, err)
251: 	}
252: 
253: 	tradeFunc, err := entities.TradeComponentFunc(tc)
254: 	if err != nil {
255: 		return nil, fmt.Errorf(fmtw, invalid, err)
256: 	}
257: 
258: 	var c0, c1, c2, c3 float64
259: 
260: 	switch trendLineSmoothingLength {
261: 	case tlslTwo:
262: 		c0, c1 = 2.0/3.0, 1.0/3.0
263: 	case tlslThree:
264: 		c0, c1, c2 = 3.0/6.0, 2.0/6.0, 1.0/6.0
265: 	default: // tlslFour
266: 		c0, c1, c2, c3 = 4.0/10.0, 3.0/10.0, 2.0/10.0, 1.0/10.0
267: 	}
268: 
269: 	maxPeriod := dc.MaxPeriod()
270: 	nan := math.NaN()
271: 
272: 	return &TrendCycleMode{
273: 		mnemonic:                 mnemonicVal,
274: 		description:              descrVal + mnemonicVal,
275: 		mnemonicTrend:            mnemonicTrend,
276: 		descriptionTrend:         descrTrend + mnemonicTrend,
277: 		mnemonicCycle:            mnemonicCycle,
278: 		descriptionCycle:         descrCycle + mnemonicCycle,
279: 		mnemonicITL:              mnemonicITL,
280: 		descriptionITL:           descrITL + mnemonicITL,
281: 		mnemonicSine:             mnemonicSine,
282: 		descriptionSine:          descrSine + mnemonicSine,
283: 		mnemonicSineLead:         mnemonicSineLead,
284: 		descriptionSineLead:      descrSineLead + mnemonicSineLead,
285: 		mnemonicDCP:              mnemonicDCP,
286: 		descriptionDCP:           descrDCP + mnemonicDCP,
287: 		mnemonicDCPhase:          mnemonicDCPha,
288: 		descriptionDCPhase:       descrDCPha + mnemonicDCPha,
289: 		dc:                       dc,
290: 		cyclePartMultiplier:      cyclePartMultiplier,
291: 		separationPercentage:     separationPercentage,
292: 		separationFactor:         separationPercentage / sepDenom,
293: 		trendLineSmoothingLength: trendLineSmoothingLength,
294: 		coeff0:                   c0,
295: 		coeff1:                   c1,
296: 		coeff2:                   c2,
297: 		coeff3:                   c3,
298: 		trendline:                nan,
299: 		sinWave:                  nan,
300: 		sinWaveLead:              nan,
301: 		isTrendMode:              true,
302: 		input:                    make([]float64, maxPeriod),
303: 		inputLength:              maxPeriod,
304: 		inputLengthMin1:          maxPeriod - 1,
305: 		barFunc:                  barFunc,
306: 		quoteFunc:                quoteFunc,
307: 		tradeFunc:                tradeFunc,
308: 	}, nil
309: }
310: 
311: // IsPrimed indicates whether an indicator is primed.
312: func (s *TrendCycleMode) IsPrimed() bool {
313: 	s.mu.RLock()
314: 	defer s.mu.RUnlock()
315: 
316: 	return s.primed
317: }
318: 
319: // Metadata describes an output data of the indicator.
320: func (s *TrendCycleMode) Metadata() core.Metadata {
321: 	return core.Metadata{
322: 		Type:        core.TrendCycleMode,
323: 		Mnemonic:    s.mnemonic,
324: 		Description: s.description,
325: 		Outputs: []outputs.Metadata{
326: 			{Kind: int(Value), Type: outputs.ScalarType, Mnemonic: s.mnemonic, Description: s.description},
327: 			{Kind: int(IsTrendMode), Type: outputs.ScalarType, Mnemonic: s.mnemonicTrend, Description: s.descriptionTrend},
328: 			{Kind: int(IsCycleMode), Type: outputs.ScalarType, Mnemonic: s.mnemonicCycle, Description: s.descriptionCycle},
329: 			{Kind: int(InstantaneousTrendLine), Type: outputs.ScalarType, Mnemonic: s.mnemonicITL, Description: s.descriptionITL},
330: 			{Kind: int(SineWave), Type: outputs.ScalarType, Mnemonic: s.mnemonicSine, Description: s.descriptionSine},
331: 			{Kind: int(SineWaveLead), Type: outputs.ScalarType, Mnemonic: s.mnemonicSineLead, Description: s.descriptionSineLead},
332: 			{Kind: int(DominantCyclePeriod), Type: outputs.ScalarType, Mnemonic: s.mnemonicDCP, Description: s.descriptionDCP},
333: 			{Kind: int(DominantCyclePhase), Type: outputs.ScalarType, Mnemonic: s.mnemonicDCPhase, Description: s.descriptionDCPhase},
334: 		},
335: 	}
336: }
337: 
338: // Update updates the value of the indicator given the next sample, returning the
339: // (value, isTrendMode, isCycleMode, trendline, sineWave, sineWaveLead, period, phase) tuple.
340: // The isTrendMode / isCycleMode values are encoded as 1 / 0 scalars. Returns NaN for all
341: // outputs if the indicator is not yet primed.
342: //
343: //nolint:funlen,cyclop,gocognit
344: func (s *TrendCycleMode) Update(sample float64) (float64, float64, float64, float64, float64, float64, float64, float64) {
345: 	if math.IsNaN(sample) {
346: 		return sample, sample, sample, sample, sample, sample, sample, sample
347: 	}
348: 
349: 	// Delegate to the inner DominantCycle. Its own mutex is independent.
350: 	_, period, phase := s.dc.Update(sample)
351: 	// SmoothedPrice tracks the WMA smoothed price inside the Hilbert transformer.
352: 	smoothedPrice := s.dc.SmoothedPrice()
353: 
354: 	s.mu.Lock()
355: 	defer s.mu.Unlock()
356: 
357: 	s.pushInput(sample)
358: 
359: 	if s.primed {
360: 		smoothedPeriod := period
361: 		average := s.calculateTrendAverage(smoothedPeriod)
362: 		s.trendline = s.coeff0*average + s.coeff1*s.trendAverage1 +
363: 			s.coeff2*s.trendAverage2 + s.coeff3*s.trendAverage3
364: 		s.trendAverage3 = s.trendAverage2
365: 		s.trendAverage2 = s.trendAverage1
366: 		s.trendAverage1 = average
367: 
368: 		diff := s.calculateSineLeadWaveDifference(phase)
369: 
370: 		// Condition 1: a cycle mode exists for the half-period of a dominant cycle
371: 		// after the SineWave vs SineWaveLead crossing.
372: 		s.isTrendMode = true
373: 
374: 		if (diff > 0 && s.prevSineLeadWaveDifference < 0) ||
375: 			(diff < 0 && s.prevSineLeadWaveDifference > 0) {
376: 			s.isTrendMode = false
377: 			s.samplesInTrend = 0
378: 		}
379: 
380: 		s.prevSineLeadWaveDifference = diff
381: 		s.samplesInTrend++
382: 
383: 		const half = 0.5
384: 		if float64(s.samplesInTrend) < half*smoothedPeriod {
385: 			s.isTrendMode = false
386: 		}
387: 
388: 		// Condition 2: cycle mode if the measured phase rate of change is more than 2/3
389: 		// the phase rate of change of the dominant cycle (360/period) and less than 1.5 times it.
390: 		phaseDelta := phase - s.prevDcPhase
391: 		s.prevDcPhase = phase
392: 
393: 		if math.Abs(smoothedPeriod) > epsilon {
394: 			const (
395: 				minFactor = 2.0 / 3.0
396: 				maxFactor = 1.5
397: 				fullCycle = 360.0
398: 			)
399: 
400: 			dcRate := fullCycle / smoothedPeriod
401: 			if phaseDelta > minFactor*dcRate && phaseDelta < maxFactor*dcRate {
402: 				s.isTrendMode = false
403: 			}
404: 		}
405: 
406: 		// Condition 3: if the WMA smoothed price is separated by more than the separation
407: 		// percentage from the instantaneous trend line, force the trend mode.
408: 		if math.Abs(s.trendline) > epsilon &&
409: 			math.Abs((smoothedPrice-s.trendline)/s.trendline) >= s.separationFactor {
410: 			s.isTrendMode = true
411: 		}
412: 
413: 		return s.mode(), s.isTrendFloat(), s.isCycleFloat(), s.trendline, s.sinWave, s.sinWaveLead, period, phase
414: 	}
415: 
416: 	if s.dc.IsPrimed() {
417: 		s.primed = true
418: 		smoothedPeriod := period
419: 		s.trendline = s.calculateTrendAverage(smoothedPeriod)
420: 		s.trendAverage1 = s.trendline
421: 		s.trendAverage2 = s.trendline
422: 		s.trendAverage3 = s.trendline
423: 
424: 		s.prevDcPhase = phase
425: 		s.prevSineLeadWaveDifference = s.calculateSineLeadWaveDifference(phase)
426: 
427: 		s.isTrendMode = true
428: 		s.samplesInTrend++
429: 
430: 		const half = 0.5
431: 		if float64(s.samplesInTrend) < half*smoothedPeriod {
432: 			s.isTrendMode = false
433: 		}
434: 
435: 		return s.mode(), s.isTrendFloat(), s.isCycleFloat(), s.trendline, s.sinWave, s.sinWaveLead, period, phase
436: 	}
437: 
438: 	nan := math.NaN()
439: 
440: 	return nan, nan, nan, nan, nan, nan, nan, nan
441: }
442: 
443: // UpdateScalar updates the indicator given the next scalar sample.
444: func (s *TrendCycleMode) UpdateScalar(sample *entities.Scalar) core.Output {
445: 	return s.updateEntity(sample.Time, sample.Value)
446: }
447: 
448: // UpdateBar updates the indicator given the next bar sample.
449: func (s *TrendCycleMode) UpdateBar(sample *entities.Bar) core.Output {
450: 	return s.updateEntity(sample.Time, s.barFunc(sample))
451: }
452: 
453: // UpdateQuote updates the indicator given the next quote sample.
454: func (s *TrendCycleMode) UpdateQuote(sample *entities.Quote) core.Output {
455: 	return s.updateEntity(sample.Time, s.quoteFunc(sample))
456: }
457: 
458: // UpdateTrade updates the indicator given the next trade sample.
459: func (s *TrendCycleMode) UpdateTrade(sample *entities.Trade) core.Output {
460: 	return s.updateEntity(sample.Time, s.tradeFunc(sample))
461: }
462: 
463: func (s *TrendCycleMode) updateEntity(t time.Time, sample float64) core.Output {
464: 	const length = 8
465: 
466: 	value, trend, cycle, itl, sine, sineLead, period, phase := s.Update(sample)
467: 
468: 	output := make([]any, length)
469: 	i := 0
470: 	output[i] = entities.Scalar{Time: t, Value: value}
471: 	i++
472: 	output[i] = entities.Scalar{Time: t, Value: trend}
473: 	i++
474: 	output[i] = entities.Scalar{Time: t, Value: cycle}
475: 	i++
476: 	output[i] = entities.Scalar{Time: t, Value: itl}
477: 	i++
478: 	output[i] = entities.Scalar{Time: t, Value: sine}
479: 	i++
480: 	output[i] = entities.Scalar{Time: t, Value: sineLead}
481: 	i++
482: 	output[i] = entities.Scalar{Time: t, Value: period}
483: 	i++
484: 	output[i] = entities.Scalar{Time: t, Value: phase}
485: 
486: 	return output
487: }
488: 
489: // epsilon is the floating-point equality threshold (matches C#'s double.Epsilon behaviour for zero checks).
490: const epsilon = 1e-308
491: 
492: func (s *TrendCycleMode) pushInput(value float64) {
493: 	copy(s.input[1:], s.input[:s.inputLengthMin1])
494: 	s.input[0] = value
495: }
496: 
497: func (s *TrendCycleMode) calculateTrendAverage(smoothedPeriod float64) float64 {
498: 	const half = 0.5
499: 
500: 	length := int(math.Floor(smoothedPeriod*s.cyclePartMultiplier + half))
501: 	if length > s.inputLength {
502: 		length = s.inputLength
503: 	} else if length < 1 {
504: 		length = 1
505: 	}
506: 
507: 	var sum float64
508: 	for i := 0; i < length; i++ {
509: 		sum += s.input[i]
510: 	}
511: 
512: 	return sum / float64(length)
513: }
514: 
515: func (s *TrendCycleMode) calculateSineLeadWaveDifference(phase float64) float64 {
516: 	const leadOffset = 45.0
517: 
518: 	p := phase * deg2Rad
519: 	s.sinWave = math.Sin(p)
520: 	s.sinWaveLead = math.Sin(p + leadOffset*deg2Rad)
521: 
522: 	return s.sinWave - s.sinWaveLead
523: }
524: 
525: func (s *TrendCycleMode) mode() float64 {
526: 	if s.isTrendMode {
527: 		return 1.0
528: 	}
529: 
530: 	return -1.0
531: }
532: 
533: func (s *TrendCycleMode) isTrendFloat() float64 {
534: 	if s.isTrendMode {
535: 		return 1.0
536: 	}
537: 
538: 	return 0.0
539: }
540: 
541: func (s *TrendCycleMode) isCycleFloat() float64 {
542: 	if s.isTrendMode {
543: 		return 0.0
544: 	}
545: 
546: 	return 1.0
547: }

(End of file - total 547 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 11.3s)

_Thinking:_

 I'm mapping out the structure for CoronaSpectrum — it needs a Params struct with raster and parameter value ranges, a high-pass filter cutoff, and components for bar/quote/trade data, plus an Output kind that produces a heatmap value and scalar outputs for dominant cycle metrics.

Now I should verify the outputs.Metadata and outputs.HeatmapType infrastructure exists, then check how indicators register with core.Type to establish the right pattern for this first heatmap indicator.

**Tool: bash**

**Input:**
```json
{
  "command": "ls go/indicators/core/outputs/ && rg -n \"HeatmapType|ScalarType\" go/indicators/core/outputs/ 2>&1 | head -20",
  "description": "Check output types"
}
```

**Output:**
```
band.go
band_test.go
common.go
heatmap.go
heatmap_test.go
metadata.go
type.go
type_test.go
/bin/bash: line 1: rg: command not found

```

---

## Assistant (Build · claude-opus-4.7 · 3.7s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/outputs/type.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/outputs/type.go</path>
<type>file</type>
<content>1: // Package output encapsuletes info related to outputs of an indicator.
2: package outputs
3: 
4: import (
5: 	"bytes"
6: 	"errors"
7: 	"fmt"
8: )
9: 
10: // Type identifies a type of an output of an indicator by enumerating all possible output types.
11: type Type int
12: 
13: const (
14: 	// Holds a time stamp and a value.
15: 	ScalarType Type = iota + 1
16: 
17: 	// Holds a time stamp and two values representing upper and lower lines of a band.
18: 	BandType
19: 
20: 	// Holds a time stamp and an array of values representing a heat-map column.
21: 	HeatmapType
22: 	last
23: )
24: 
25: const (
26: 	unknown = "unknown"
27: 	scalar  = "scalar"
28: 	band    = "band"
29: 	heatmap = "heatmap"
30: )
31: 
32: var errUnknownType = errors.New("unknown indicator output type")
33: 
34: // String implements the Stringer interface.
35: func (t Type) String() string {
36: 	switch t {
37: 	case ScalarType:
38: 		return scalar
39: 	case BandType:
40: 		return band
41: 	case HeatmapType:
42: 		return heatmap
43: 	default:
44: 		return unknown
45: 	}
46: }
47: 
48: // IsKnown determines if this output type is known.
49: func (t Type) IsKnown() bool {
50: 	return t >= ScalarType && t < last
51: }
52: 
53: // MarshalJSON implements the Marshaler interface.
54: func (t Type) MarshalJSON() ([]byte, error) {
55: 	s := t.String()
56: 	if s == unknown {
57: 		return nil, fmt.Errorf("cannot marshal '%s': %w", s, errUnknownType)
58: 	}
59: 
60: 	const extra = 2 // Two bytes for quotes.
61: 
62: 	b := make([]byte, 0, len(s)+extra)
63: 	b = append(b, '"')
64: 	b = append(b, s...)
65: 	b = append(b, '"')
66: 
67: 	return b, nil
68: }
69: 
70: // UnmarshalJSON implements the Unmarshaler interface.
71: func (t *Type) UnmarshalJSON(data []byte) error {
72: 	d := bytes.Trim(data, "\"")
73: 	s := string(d)
74: 
75: 	switch s {
76: 	case scalar:
77: 		*t = ScalarType
78: 	case band:
79: 		*t = BandType
80: 	case heatmap:
81: 		*t = HeatmapType
82: 	default:
83: 		return fmt.Errorf("cannot unmarshal '%s': %w", s, errUnknownType)
84: 	}
85: 
86: 	return nil
87: }

(End of file - total 87 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 482.8s)

_Thinking:_

 I've got HeatmapType ready at the output level, so the foundation is there. Before I move forward with CoronaSpectrum though, I should check in with the user — this would be zpano's first heatmap indicator, and I'm about to establish a new pattern combining heatmap and scalar outputs. I want to validate the approach rather than assume I'm on the right track, especially around whether the heatmap should be generated per-bar.

Corona helper is done and tested. Before building the 4 indicators on top, I need design confirmations on a few decisions that will set the pattern for zpano's first heatmap indicators.

## Status
- ✅ Phase 1 complete: Corona helper at `go/indicators/johnehlers/corona/` — compiles, 3 unit tests pass (smoke + NaN + invalid params)
- Fixed 4 bugs vs MBST during review (per-bar max-amp² reset, first-sample skip, DC reset-to-0-each-bar, median-buffer MaxValue sentinel init)
- Spot checks show DC oscillating realistically across the 252-entry TA-Lib series (6.0, 17.8, 15.2, 14.7, 17.5, 19.8, 6.0 at sampled bars)

Now I need to pick a few conventions before building CoronaSpectrum (first zpano heatmap indicator) and the other three.

**Tool: question**

**Input:**
```json
{
  "questions": [
    {
      "header": "Unprimed heatmap output",
      "question": "For bars where the indicator is not yet primed, what should the Heatmap output be? (Scalars will be NaN in all cases, matching TCM.)",
      "options": [
        {
          "label": "NewEmptyHeatmap (Recommended)",
          "description": "Empty values slice but correct Time + parameter range; consumers can easily skip via IsEmpty(). Matches the spirit of MBST's `return new HeatMap(dateTime, null)`."
        },
        {
          "label": "nil",
          "description": "Return a nil *Heatmap for unprimed bars. Simpler but forces every consumer to nil-check."
        },
        {
          "label": "Fully NaN-filled heatmap",
          "description": "Same length as primed output but every value is NaN. More uniform array shape; slightly wasteful."
        }
      ]
    },
    {
      "header": "Indicator mnemonic style",
      "question": "How should the Corona indicator mnemonics be formatted? MBST uses CamelCase like `CoronaS`, `CoronaDC`; zpano indicators like TCM use `tcm(...)`, `tcm-itl(...)` style.",
      "options": [
        {
          "label": "zpano style (Recommended)",
          "description": "Lowercase with params, e.g. `cspect(6, 20, 6, 30, 30, median)`, `cspect-dc(...)`, `cspect-dcm(...)`. Matches TCM / sinewave / dominantcycle conventions."
        },
        {
          "label": "MBST style verbatim",
          "description": "Keep `CoronaS`, `CoronaDC`, `CoronaDCM`, `CoronaSNR`, `CoronaSW`, `CoronaTV` without parameters. Preserves MBST intent but breaks zpano mnemonic conventions."
        },
        {
          "label": "Hybrid",
          "description": "zpano-style function-with-params mnemonic but with CamelCase base, e.g. `CoronaS(6, 20, 6, 30, 30, median)`."
        }
      ]
    },
    {
      "header": "Registration in core.Type",
      "question": "Add 4 new `core.Type` constants for the 4 Corona indicators?",
      "options": [
        {
          "label": "Yes, register all 4 (Recommended)",
          "description": "`CoronaSpectrum`, `CoronaSignalToNoiseRatio`, `CoronaSwingPosition`, `CoronaTrendVigor` added to `go/indicators/core/type.go` with JSON marshalling + tests. Standard pattern."
        },
        {
          "label": "Register only as needed",
          "description": "Defer registration; add types only if a test or consumer needs them. Not recommended — breaks the convention."
        }
      ]
    },
    {
      "header": "TS port timing",
      "question": "After the 4 Go indicators are complete + tested, should I proceed directly to the TypeScript port, or pause for review first?",
      "options": [
        {
          "label": "Pause after Go (Recommended)",
          "description": "Stop after all 4 Go indicators are done + tests green; let you review before I spend the context on the TS port."
        },
        {
          "label": "Continue straight into TS",
          "description": "Port each indicator to TS immediately after its Go counterpart is green."
        },
        {
          "label": "Finish all Go, then all TS in one pass",
          "description": "Complete all 4 Go indicators first, then all 4 TS ports back-to-back without pausing."
        }
      ]
    }
  ]
}
```

**Output:**
```
User has answered your questions: "For bars where the indicator is not yet primed, what should the Heatmap output be? (Scalars will be NaN in all cases, matching TCM.)"="NewEmptyHeatmap (Recommended)", "How should the Corona indicator mnemonics be formatted? MBST uses CamelCase like `CoronaS`, `CoronaDC`; zpano indicators like TCM use `tcm(...)`, `tcm-itl(...)` style."="zpano style (Recommended)", "Add 4 new `core.Type` constants for the 4 Corona indicators?"="Yes, register all 4 (Recommended)", "After the 4 Go indicators are complete + tested, should I proceed directly to the TypeScript port, or pause for review first?"="Pause after Go (Recommended)". You can now continue with the user's answers in mind.
```

---

## Assistant (Build · claude-opus-4.7 · 5.9s)

All recommended. Continuing with Phase 2: CoronaSpectrum (Go). Let me first register the 4 new core.Type constants, then study output kinds in TCM, then build CoronaSpectrum.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/type.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/type.go</path>
<type>file</type>
<content>1: package core
2: 
3: import (
4: 	"bytes"
5: 	"fmt"
6: )
7: 
8: // Type Identifies an indicator by enumerating all implemented indicators.
9: type Type int
10: 
11: const (
12: 	// SimpleMovingAverage identifies the Simple Moving Average (SMA) indicator.
13: 	SimpleMovingAverage Type = iota + 1
14: 
15: 	// WeightedMovingAverage identifies the Weighted Moving Average (WMA) indicator.
16: 	WeightedMovingAverage
17: 
18: 	// TriangularMovingAverage identifies the Triangular Moving Average (TRIMA) indicator.
19: 	TriangularMovingAverage
20: 
21: 	// ExponentialMovingAverage identifies the Exponential Moving Average (EMA) indicator.
22: 	ExponentialMovingAverage
23: 
24: 	// DoubleExponentialMovingAverage identifies the Double Exponential Moving Average (DEMA) indicator.
25: 	DoubleExponentialMovingAverage
26: 
27: 	// TripleExponentialMovingAverage identifies the Triple Exponential Moving Average (TEMA) indicator.
28: 	TripleExponentialMovingAverage
29: 
30: 	// T2ExponentialMovingAverage identifies the T2 Exponential Moving Average (T2) indicator.
31: 	T2ExponentialMovingAverage
32: 
33: 	// T3ExponentialMovingAverage identifies the T3 Exponential Moving Average (T3) indicator.
34: 	T3ExponentialMovingAverage
35: 
36: 	// KaufmanAdaptiveMovingAverage identifies the Kaufman Adaptive Moving Average (KAMA) indicator.
37: 	KaufmanAdaptiveMovingAverage
38: 
39: 	// JurikMovingAverage identifies the Jurik Moving Average (JMA) indicator.
40: 	JurikMovingAverage
41: 
42: 	// MesaAdaptiveMovingAverage identifies the Ehlers MESA Adaptive Moving Average (MAMA) indicator.
43: 	MesaAdaptiveMovingAverage
44: 
45: 	// FractalAdaptiveMovingAverage identifies the Ehlers Fractal Adaptive Moving Average (FRAMA) indicator.
46: 	FractalAdaptiveMovingAverage
47: 
48: 	// DominantCycle identifies the Ehlers Dominant Cycle (DC) indicator, exposing raw period, smoothed period and phase.
49: 	DominantCycle
50: 
51: 	// Momentum identifies the momentum (MOM) indicator.
52: 	Momentum
53: 
54: 	// RateOfChange identifies the Rate of Change (ROC) indicator.
55: 	RateOfChange
56: 
57: 	// RateOfChangePercent identifies the Rate of Change Percent (ROCP) indicator.
58: 	RateOfChangePercent
59: 
60: 	// RelativeStrengthIndex identifies the Relative Strength Index (RSI) indicator.
61: 	RelativeStrengthIndex
62: 
63: 	// ChandeMomentumOscillator identifies the Chande Momentum Oscillator (CMO) indicator.
64: 	ChandeMomentumOscillator
65: 
66: 	// BollingerBands identifies the Bollinger Bands (BB) indicator.
67: 	BollingerBands
68: 
69: 	// Variance identifies the Variance (VAR) indicator.
70: 	Variance
71: 
72: 	// StandardDeviation identifies the Standard Deviation (STDEV) indicator.
73: 	StandardDeviation
74: 
75: 	// GoertzelSpectrum identifies the Goertzel power spectrum (GOERTZEL) indicator.
76: 	GoertzelSpectrum
77: 
78: 	// CenterOfGravityOscillator identifies the Ehlers Center of Gravity (COG) oscillator indicator.
79: 	CenterOfGravityOscillator
80: 
81: 	// CyberCycle identifies the Ehlers Cyber Cycle (CC) indicator.
82: 	CyberCycle
83: 
84: 	// InstantaneousTrendLine identifies the Ehlers Instantaneous Trend Line (iTrend) indicator.
85: 	InstantaneousTrendLine
86: 
87: 	// SuperSmoother identifies the Ehlers Super Smoother (SS) indicator.
88: 	SuperSmoother
89: 
90: 	// ZeroLagExponentialMovingAverage identifies the Ehlers Zero-lag Exponential Moving Average (ZEMA) indicator.
91: 	ZeroLagExponentialMovingAverage
92: 
93: 	// ZeroLagErrorCorrectingExponentialMovingAverage identifies the Ehlers Zero-lag Error-Correcting Exponential Moving Average (ZECEMA) indicator.
94: 	ZeroLagErrorCorrectingExponentialMovingAverage
95: 
96: 	// RoofingFilter identifies the Ehlers Roofing Filter indicator.
97: 	RoofingFilter
98: 
99: 	// TrueRange identifies the Welles Wilder True Range (TR) indicator.
100: 	TrueRange
101: 
102: 	// AverageTrueRange identifies the Welles Wilder Average True Range (ATR) indicator.
103: 	AverageTrueRange
104: 
105: 	// NormalizedAverageTrueRange identifies the Welles Wilder Normalized Average True Range (NATR) indicator.
106: 	NormalizedAverageTrueRange
107: 
108: 	// DirectionalMovementMinus identifies the Welles Wilder Directional Movement Minus (-DM) indicator.
109: 	DirectionalMovementMinus
110: 
111: 	// DirectionalMovementPlus identifies the Welles Wilder Directional Movement Plus (+DM) indicator.
112: 	DirectionalMovementPlus
113: 
114: 	// DirectionalIndicatorMinus identifies the Welles Wilder Directional Indicator Minus (-DI) indicator.
115: 	DirectionalIndicatorMinus
116: 
117: 	// DirectionalIndicatorPlus identifies the Welles Wilder Directional Indicator Plus (+DI) indicator.
118: 	DirectionalIndicatorPlus
119: 
120: 	// DirectionalMovementIndex identifies the Welles Wilder Directional Movement Index (DX) indicator.
121: 	DirectionalMovementIndex
122: 
123: 	// AverageDirectionalMovementIndex identifies the Welles Wilder Average Directional Movement Index (ADX) indicator.
124: 	AverageDirectionalMovementIndex
125: 
126: 	// AverageDirectionalMovementIndexRating identifies the Welles Wilder Average Directional Movement Index Rating (ADXR) indicator.
127: 	AverageDirectionalMovementIndexRating
128: 
129: 	// WilliamsPercentR identifies the Larry Williams Williams %R (WILL%R) indicator.
130: 	WilliamsPercentR
131: 
132: 	// PercentagePriceOscillator identifies the Gerald Appel Percentage Price Oscillator (PPO) indicator.
133: 	PercentagePriceOscillator
134: 
135: 	// AbsolutePriceOscillator identifies the Absolute Price Oscillator (APO) indicator.
136: 	AbsolutePriceOscillator
137: 
138: 	// CommodityChannelIndex identifies the Donald Lambert Commodity Channel Index (CCI) indicator.
139: 	CommodityChannelIndex
140: 
141: 	// MoneyFlowIndex identifies the Gene Quong Money Flow Index (MFI) indicator.
142: 	MoneyFlowIndex
143: 
144: 	// OnBalanceVolume identifies the Joseph Granville On-Balance Volume (OBV) indicator.
145: 	OnBalanceVolume
146: 
147: 	// BalanceOfPower identifies the Igor Livshin Balance of Power (BOP) indicator.
148: 	BalanceOfPower
149: 
150: 	// RateOfChangeRatio identifies the Rate of Change Ratio (ROCR / ROCR100) indicator.
151: 	RateOfChangeRatio
152: 
153: 	// PearsonsCorrelationCoefficient identifies the Pearson's Correlation Coefficient (CORREL) indicator.
154: 	PearsonsCorrelationCoefficient
155: 
156: 	// LinearRegression identifies the Linear Regression (LINEARREG) indicator.
157: 	LinearRegression
158: 
159: 	// UltimateOscillator identifies the Larry Williams Ultimate Oscillator (ULTOSC) indicator.
160: 	UltimateOscillator
161: 
162: 	// StochasticRelativeStrengthIndex identifies the Tushar Chande Stochastic RSI (STOCHRSI) indicator.
163: 	StochasticRelativeStrengthIndex
164: 
165: 	// Stochastic identifies the George Lane Stochastic Oscillator (STOCH) indicator.
166: 	Stochastic
167: 
168: 	// Aroon identifies the Tushar Chande Aroon (AROON) indicator.
169: 	Aroon
170: 
171: 	// AdvanceDecline identifies the Marc Chaikin Advance-Decline (AD) indicator.
172: 	AdvanceDecline
173: 
174: 	// AdvanceDeclineOscillator identifies the Marc Chaikin Advance-Decline Oscillator (ADOSC) indicator.
175: 	AdvanceDeclineOscillator
176: 
177: 	// ParabolicStopAndReverse identifies the Welles Wilder Parabolic Stop And Reverse (SAR) indicator.
178: 	ParabolicStopAndReverse
179: 
180: 	// TripleExponentialMovingAverageOscillator identifies Jack Hutson's Triple Exponential Moving Average Oscillator (TRIX) indicator.
181: 	TripleExponentialMovingAverageOscillator
182: 
183: 	// BollingerBandsTrend identifies John Bollinger's Bollinger Bands Trend (BBTrend) indicator.
184: 	BollingerBandsTrend
185: 
186: 	// MovingAverageConvergenceDivergence identifies Gerald Appel's Moving Average Convergence Divergence (MACD) indicator.
187: 	MovingAverageConvergenceDivergence
188: 
189: 	// SineWave identifies the Ehlers Sine Wave (SW) indicator, exposing sine value, lead sine, band, dominant cycle period and phase.
190: 	SineWave
191: 
192: 	// HilbertTransformerInstantaneousTrendLine identifies the Ehlers Hilbert Transformer Instantaneous Trend Line (HTITL) indicator,
193: 	// exposing the trend line value and the smoothed dominant cycle period.
194: 	HilbertTransformerInstantaneousTrendLine
195: 
196: 	// TrendCycleMode identifies the Ehlers Trend / Cycle Mode (TCM) indicator, exposing the trend/cycle value
197: 	// (+1 in trend, −1 in cycle), trend/cycle mode flags, instantaneous trend line, sine wave, lead sine wave,
198: 	// dominant cycle period and phase.
199: 	TrendCycleMode
200: 	last
201: )
202: 
203: const (
204: 	unknown                                        = "unknown"
205: 	simpleMovingAverage                            = "simpleMovingAverage"
206: 	weightedMovingAverage                          = "weightedMovingAverage"
207: 	triangularMovingAverage                        = "triangularMovingAverage"
208: 	exponentialMovingAverage                       = "exponentialMovingAverage"
209: 	doubleExponentialMovingAverage                 = "doubleExponentialMovingAverage"
210: 	tripleExponentialMovingAverage                 = "tripleExponentialMovingAverage"
211: 	t2ExponentialMovingAverage                     = "t2ExponentialMovingAverage"
212: 	t3ExponentialMovingAverage                     = "t3ExponentialMovingAverage"
213: 	kaufmanAdaptiveMovingAverage                   = "kaufmanAdaptiveMovingAverageMovingAverage"
214: 	jurikMovingAverage                             = "jurikMovingAverage"
215: 	mesaAdaptiveMovingAverage                      = "mesaAdaptiveMovingAverage"
216: 	fractalAdaptiveMovingAverage                   = "fractalAdaptiveMovingAverage"
217: 	dominantCycle                                  = "dominantCycle"
218: 	momentum                                       = "momentum"
219: 	rateOfChange                                   = "rateOfChange"
220: 	rateOfChangePercent                            = "rateOfChangePercent"
221: 	relativeStrengthIndex                          = "relativeStrengthIndex"
222: 	chandeMomentumOscillator                       = "chandeMomentumOscillator"
223: 	bollingerBands                                 = "bollingerBands"
224: 	variance                                       = "variance"
225: 	standardDeviation                              = "standardDeviation"
226: 	goertzelSpectrum                               = "goertzelSpectrum"
227: 	centerOfGravityOscillator                      = "centerOfGravityOscillator"
228: 	cyberCycle                                     = "cyberCycle"
229: 	instantaneousTrendLine                         = "instantaneousTrendLine"
230: 	superSmoother                                  = "superSmoother"
231: 	zeroLagExponentialMovingAverage                = "zeroLagExponentialMovingAverage"
232: 	zeroLagErrorCorrectingExponentialMovingAverage = "zeroLagErrorCorrectingExponentialMovingAverage"
233: 	roofingFilter                                  = "roofingFilter"
234: 	trueRange                                      = "trueRange"
235: 	averageTrueRange                               = "averageTrueRange"
236: 	normalizedAverageTrueRange                     = "normalizedAverageTrueRange"
237: 	directionalMovementMinus                       = "directionalMovementMinus"
238: 	directionalMovementPlus                        = "directionalMovementPlus"
239: 	directionalIndicatorMinus                      = "directionalIndicatorMinus"
240: 	directionalIndicatorPlus                       = "directionalIndicatorPlus"
241: 	directionalMovementIndex                       = "directionalMovementIndex"
242: 	averageDirectionalMovementIndex                = "averageDirectionalMovementIndex"
243: 	averageDirectionalMovementIndexRating          = "averageDirectionalMovementIndexRating"
244: 	williamsPercentR                               = "williamsPercentR"
245: 	percentagePriceOscillator                      = "percentagePriceOscillator"
246: 	absolutePriceOscillator                        = "absolutePriceOscillator"
247: 	commodityChannelIndex                          = "commodityChannelIndex"
248: 	moneyFlowIndex                                 = "moneyFlowIndex"
249: 	onBalanceVolume                                = "onBalanceVolume"
250: 	balanceOfPower                                 = "balanceOfPower"
251: 	rateOfChangeRatio                              = "rateOfChangeRatio"
252: 	pearsonsCorrelationCoefficient                 = "pearsonsCorrelationCoefficient"
253: 	linearRegression                               = "linearRegression"
254: 	ultimateOscillator                             = "ultimateOscillator"
255: 	stochasticRelativeStrengthIndex                = "stochasticRelativeStrengthIndex"
256: 	stochastic                                     = "stochastic"
257: 	aroon                                          = "aroon"
258: 	advanceDecline                                 = "advanceDecline"
259: 	advanceDeclineOscillator                       = "advanceDeclineOscillator"
260: 	parabolicStopAndReverse                        = "parabolicStopAndReverse"
261: 	tripleExponentialMovingAverageOscillator       = "tripleExponentialMovingAverageOscillator"
262: 	bollingerBandsTrend                            = "bollingerBandsTrend"
263: 	movingAverageConvergenceDivergence             = "movingAverageConvergenceDivergence"
264: 	sineWave                                       = "sineWave"
265: 	hilbertTransformerInstantaneousTrendLine       = "hilbertTransformerInstantaneousTrendLine"
266: 	trendCycleMode                                 = "trendCycleMode"
267: )
268: 
269: // String implements the Stringer interface.
270: //
271: //nolint:exhaustive,cyclop,funlen
272: func (t Type) String() string {
273: 	switch t {
274: 	case SimpleMovingAverage:
275: 		return simpleMovingAverage
276: 	case WeightedMovingAverage:
277: 		return weightedMovingAverage
278: 	case TriangularMovingAverage:
279: 		return triangularMovingAverage
280: 	case ExponentialMovingAverage:
281: 		return exponentialMovingAverage
282: 	case DoubleExponentialMovingAverage:
283: 		return doubleExponentialMovingAverage
284: 	case TripleExponentialMovingAverage:
285: 		return tripleExponentialMovingAverage
286: 	case T2ExponentialMovingAverage:
287: 		return t2ExponentialMovingAverage
288: 	case T3ExponentialMovingAverage:
289: 		return t3ExponentialMovingAverage
290: 	case KaufmanAdaptiveMovingAverage:
291: 		return kaufmanAdaptiveMovingAverage
292: 	case JurikMovingAverage:
293: 		return jurikMovingAverage
294: 	case MesaAdaptiveMovingAverage:
295: 		return mesaAdaptiveMovingAverage
296: 	case FractalAdaptiveMovingAverage:
297: 		return fractalAdaptiveMovingAverage
298: 	case DominantCycle:
299: 		return dominantCycle
300: 	case Momentum:
301: 		return momentum
302: 	case RateOfChange:
303: 		return rateOfChange
304: 	case RateOfChangePercent:
305: 		return rateOfChangePercent
306: 	case RelativeStrengthIndex:
307: 		return relativeStrengthIndex
308: 	case ChandeMomentumOscillator:
309: 		return chandeMomentumOscillator
310: 	case BollingerBands:
311: 		return bollingerBands
312: 	case Variance:
313: 		return variance
314: 	case StandardDeviation:
315: 		return standardDeviation
316: 	case GoertzelSpectrum:
317: 		return goertzelSpectrum
318: 	case CenterOfGravityOscillator:
319: 		return centerOfGravityOscillator
320: 	case CyberCycle:
321: 		return cyberCycle
322: 	case InstantaneousTrendLine:
323: 		return instantaneousTrendLine
324: 	case SuperSmoother:
325: 		return superSmoother
326: 	case ZeroLagExponentialMovingAverage:
327: 		return zeroLagExponentialMovingAverage
328: 	case ZeroLagErrorCorrectingExponentialMovingAverage:
329: 		return zeroLagErrorCorrectingExponentialMovingAverage
330: 	case RoofingFilter:
331: 		return roofingFilter
332: 	case TrueRange:
333: 		return trueRange
334: 	case AverageTrueRange:
335: 		return averageTrueRange
336: 	case NormalizedAverageTrueRange:
337: 		return normalizedAverageTrueRange
338: 	case DirectionalMovementMinus:
339: 		return directionalMovementMinus
340: 	case DirectionalMovementPlus:
341: 		return directionalMovementPlus
342: 	case DirectionalIndicatorMinus:
343: 		return directionalIndicatorMinus
344: 	case DirectionalIndicatorPlus:
345: 		return directionalIndicatorPlus
346: 	case DirectionalMovementIndex:
347: 		return directionalMovementIndex
348: 	case AverageDirectionalMovementIndex:
349: 		return averageDirectionalMovementIndex
350: 	case AverageDirectionalMovementIndexRating:
351: 		return averageDirectionalMovementIndexRating
352: 	case WilliamsPercentR:
353: 		return williamsPercentR
354: 	case PercentagePriceOscillator:
355: 		return percentagePriceOscillator
356: 	case AbsolutePriceOscillator:
357: 		return absolutePriceOscillator
358: 	case CommodityChannelIndex:
359: 		return commodityChannelIndex
360: 	case MoneyFlowIndex:
361: 		return moneyFlowIndex
362: 	case OnBalanceVolume:
363: 		return onBalanceVolume
364: 	case BalanceOfPower:
365: 		return balanceOfPower
366: 	case RateOfChangeRatio:
367: 		return rateOfChangeRatio
368: 	case PearsonsCorrelationCoefficient:
369: 		return pearsonsCorrelationCoefficient
370: 	case LinearRegression:
371: 		return linearRegression
372: 	case UltimateOscillator:
373: 		return ultimateOscillator
374: 	case StochasticRelativeStrengthIndex:
375: 		return stochasticRelativeStrengthIndex
376: 	case Stochastic:
377: 		return stochastic
378: 	case Aroon:
379: 		return aroon
380: 	case AdvanceDecline:
381: 		return advanceDecline
382: 	case AdvanceDeclineOscillator:
383: 		return advanceDeclineOscillator
384: 	case ParabolicStopAndReverse:
385: 		return parabolicStopAndReverse
386: 	case TripleExponentialMovingAverageOscillator:
387: 		return tripleExponentialMovingAverageOscillator
388: 	case BollingerBandsTrend:
389: 		return bollingerBandsTrend
390: 	case MovingAverageConvergenceDivergence:
391: 		return movingAverageConvergenceDivergence
392: 	case SineWave:
393: 		return sineWave
394: 	case HilbertTransformerInstantaneousTrendLine:
395: 		return hilbertTransformerInstantaneousTrendLine
396: 	case TrendCycleMode:
397: 		return trendCycleMode
398: 	default:
399: 		return unknown
400: 	}
401: }
402: 
403: // IsKnown determines if this indicator type is known.
404: func (t Type) IsKnown() bool {
405: 	return t >= SimpleMovingAverage && t < last
406: }
407: 
408: // MarshalJSON implements the Marshaler interface.
409: func (t Type) MarshalJSON() ([]byte, error) {
410: 	const (
411: 		errFmt = "cannot marshal '%s': unknown indicator type"
412: 		extra  = 2   // Two bytes for quotes.
413: 		dqc    = '"' // Double quote character.
414: 	)
415: 
416: 	s := t.String()
417: 	if s == unknown {
418: 		return nil, fmt.Errorf(errFmt, s)
419: 	}
420: 
421: 	b := make([]byte, 0, len(s)+extra)
422: 	b = append(b, dqc)
423: 	b = append(b, s...)
424: 	b = append(b, dqc)
425: 
426: 	return b, nil
427: }
428: 
429: // UnmarshalJSON implements the Unmarshaler interface.
430: //
431: //nolint:cyclop,funlen
432: func (t *Type) UnmarshalJSON(data []byte) error {
433: 	const (
434: 		errFmt = "cannot unmarshal '%s': unknown indicator type"
435: 		dqs    = "\"" // Double quote string.
436: 	)
437: 
438: 	d := bytes.Trim(data, dqs)
439: 	s := string(d)
440: 
441: 	switch s {
442: 	case simpleMovingAverage:
443: 		*t = SimpleMovingAverage
444: 	case weightedMovingAverage:
445: 		*t = WeightedMovingAverage
446: 	case triangularMovingAverage:
447: 		*t = TriangularMovingAverage
448: 	case exponentialMovingAverage:
449: 		*t = ExponentialMovingAverage
450: 	case doubleExponentialMovingAverage:
451: 		*t = DoubleExponentialMovingAverage
452: 	case tripleExponentialMovingAverage:
453: 		*t = TripleExponentialMovingAverage
454: 	case t2ExponentialMovingAverage:
455: 		*t = T2ExponentialMovingAverage
456: 	case t3ExponentialMovingAverage:
457: 		*t = T3ExponentialMovingAverage
458: 	case kaufmanAdaptiveMovingAverage:
459: 		*t = KaufmanAdaptiveMovingAverage
460: 	case jurikMovingAverage:
461: 		*t = JurikMovingAverage
462: 	case mesaAdaptiveMovingAverage:
463: 		*t = MesaAdaptiveMovingAverage
464: 	case fractalAdaptiveMovingAverage:
465: 		*t = FractalAdaptiveMovingAverage
466: 	case dominantCycle:
467: 		*t = DominantCycle
468: 	case momentum:
469: 		*t = Momentum
470: 	case rateOfChange:
471: 		*t = RateOfChange
472: 	case rateOfChangePercent:
473: 		*t = RateOfChangePercent
474: 	case relativeStrengthIndex:
475: 		*t = RelativeStrengthIndex
476: 	case chandeMomentumOscillator:
477: 		*t = ChandeMomentumOscillator
478: 	case bollingerBands:
479: 		*t = BollingerBands
480: 	case variance:
481: 		*t = Variance
482: 	case standardDeviation:
483: 		*t = StandardDeviation
484: 	case goertzelSpectrum:
485: 		*t = GoertzelSpectrum
486: 	case centerOfGravityOscillator:
487: 		*t = CenterOfGravityOscillator
488: 	case cyberCycle:
489: 		*t = CyberCycle
490: 	case instantaneousTrendLine:
491: 		*t = InstantaneousTrendLine
492: 	case superSmoother:
493: 		*t = SuperSmoother
494: 	case zeroLagExponentialMovingAverage:
495: 		*t = ZeroLagExponentialMovingAverage
496: 	case zeroLagErrorCorrectingExponentialMovingAverage:
497: 		*t = ZeroLagErrorCorrectingExponentialMovingAverage
498: 	case roofingFilter:
499: 		*t = RoofingFilter
500: 	case trueRange:
501: 		*t = TrueRange
502: 	case averageTrueRange:
503: 		*t = AverageTrueRange
504: 	case normalizedAverageTrueRange:
505: 		*t = NormalizedAverageTrueRange
506: 	case directionalMovementMinus:
507: 		*t = DirectionalMovementMinus
508: 	case directionalMovementPlus:
509: 		*t = DirectionalMovementPlus
510: 	case directionalIndicatorMinus:
511: 		*t = DirectionalIndicatorMinus
512: 	case directionalIndicatorPlus:
513: 		*t = DirectionalIndicatorPlus
514: 	case directionalMovementIndex:
515: 		*t = DirectionalMovementIndex
516: 	case averageDirectionalMovementIndex:
517: 		*t = AverageDirectionalMovementIndex
518: 	case averageDirectionalMovementIndexRating:
519: 		*t = AverageDirectionalMovementIndexRating
520: 	case williamsPercentR:
521: 		*t = WilliamsPercentR
522: 	case percentagePriceOscillator:
523: 		*t = PercentagePriceOscillator
524: 	case absolutePriceOscillator:
525: 		*t = AbsolutePriceOscillator
526: 	case commodityChannelIndex:
527: 		*t = CommodityChannelIndex
528: 	case moneyFlowIndex:
529: 		*t = MoneyFlowIndex
530: 	case onBalanceVolume:
531: 		*t = OnBalanceVolume
532: 	case balanceOfPower:
533: 		*t = BalanceOfPower
534: 	case rateOfChangeRatio:
535: 		*t = RateOfChangeRatio
536: 	case pearsonsCorrelationCoefficient:
537: 		*t = PearsonsCorrelationCoefficient
538: 	case linearRegression:
539: 		*t = LinearRegression
540: 	case ultimateOscillator:
541: 		*t = UltimateOscillator
542: 	case stochasticRelativeStrengthIndex:
543: 		*t = StochasticRelativeStrengthIndex
544: 	case stochastic:
545: 		*t = Stochastic
546: 	case aroon:
547: 		*t = Aroon
548: 	case advanceDecline:
549: 		*t = AdvanceDecline
550: 	case advanceDeclineOscillator:
551: 		*t = AdvanceDeclineOscillator
552: 	case parabolicStopAndReverse:
553: 		*t = ParabolicStopAndReverse
554: 	case tripleExponentialMovingAverageOscillator:
555: 		*t = TripleExponentialMovingAverageOscillator
556: 	case bollingerBandsTrend:
557: 		*t = BollingerBandsTrend
558: 	case movingAverageConvergenceDivergence:
559: 		*t = MovingAverageConvergenceDivergence
560: 	case sineWave:
561: 		*t = SineWave
562: 	case hilbertTransformerInstantaneousTrendLine:
563: 		*t = HilbertTransformerInstantaneousTrendLine
564: 	case trendCycleMode:
565: 		*t = TrendCycleMode
566: 	default:
567: 		return fmt.Errorf(errFmt, s)
568: 	}
569: 
570: 	return nil
571: }

(End of file - total 571 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 14.6s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/type.go",
  "newString": "\t// TrendCycleMode identifies the Ehlers Trend / Cycle Mode (TCM) indicator, exposing the trend/cycle value\n\t// (+1 in trend, −1 in cycle), trend/cycle mode flags, instantaneous trend line, sine wave, lead sine wave,\n\t// dominant cycle period and phase.\n\tTrendCycleMode\n\n\t// CoronaSpectrum identifies the Ehlers Corona Spectrum (CSPECT) indicator, a heat-map of cyclic activity\n\t// over a cycle-period range together with the dominant cycle period and its 5-sample median.\n\tCoronaSpectrum\n\n\t// CoronaSignalToNoiseRatio identifies the Ehlers Corona Signal-to-Noise Ratio (CSNR) indicator, a heat-map\n\t// of SNR plus a smoothed SNR scalar line.\n\tCoronaSignalToNoiseRatio\n\n\t// CoronaSwingPosition identifies the Ehlers Corona Swing Position (CSWING) indicator, a heat-map of swing\n\t// position with a scalar swing-position line.\n\tCoronaSwingPosition\n\n\t// CoronaTrendVigor identifies the Ehlers Corona Trend Vigor (CTV) indicator, a heat-map of trend vigor\n\t// with a scalar trend-vigor line.\n\tCoronaTrendVigor\n\tlast\n)",
  "oldString": "\t// TrendCycleMode identifies the Ehlers Trend / Cycle Mode (TCM) indicator, exposing the trend/cycle value\n\t// (+1 in trend, −1 in cycle), trend/cycle mode flags, instantaneous trend line, sine wave, lead sine wave,\n\t// dominant cycle period and phase.\n\tTrendCycleMode\n\tlast\n)"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 5.2s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/type.go",
  "newString": "\ttrendCycleMode                                 = \"trendCycleMode\"\n\tcoronaSpectrum                                 = \"coronaSpectrum\"\n\tcoronaSignalToNoiseRatio                       = \"coronaSignalToNoiseRatio\"\n\tcoronaSwingPosition                            = \"coronaSwingPosition\"\n\tcoronaTrendVigor                               = \"coronaTrendVigor\"\n)",
  "oldString": "\ttrendCycleMode                                 = \"trendCycleMode\"\n)"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 5.0s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/type.go",
  "newString": "\tcase TrendCycleMode:\n\t\treturn trendCycleMode\n\tcase CoronaSpectrum:\n\t\treturn coronaSpectrum\n\tcase CoronaSignalToNoiseRatio:\n\t\treturn coronaSignalToNoiseRatio\n\tcase CoronaSwingPosition:\n\t\treturn coronaSwingPosition\n\tcase CoronaTrendVigor:\n\t\treturn coronaTrendVigor\n\tdefault:",
  "oldString": "\tcase TrendCycleMode:\n\t\treturn trendCycleMode\n\tdefault:"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 5.0s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/type.go",
  "newString": "\tcase trendCycleMode:\n\t\t*t = TrendCycleMode\n\tcase coronaSpectrum:\n\t\t*t = CoronaSpectrum\n\tcase coronaSignalToNoiseRatio:\n\t\t*t = CoronaSignalToNoiseRatio\n\tcase coronaSwingPosition:\n\t\t*t = CoronaSwingPosition\n\tcase coronaTrendVigor:\n\t\t*t = CoronaTrendVigor\n\tdefault:",
  "oldString": "\tcase trendCycleMode:\n\t\t*t = TrendCycleMode\n\tdefault:"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 3.4s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/type_test.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/type_test.go</path>
<type>file</type>
<content>1: //nolint:testpackage
2: package core
3: 
4: import (
5: 	"testing"
6: )
7: 
8: func TestTypeString(t *testing.T) {
9: 	t.Parallel()
10: 
11: 	tests := []struct {
12: 		t    Type
13: 		text string
14: 	}{
15: 		{SimpleMovingAverage, simpleMovingAverage},
16: 		{WeightedMovingAverage, weightedMovingAverage},
17: 		{TriangularMovingAverage, triangularMovingAverage},
18: 		{ExponentialMovingAverage, exponentialMovingAverage},
19: 		{DoubleExponentialMovingAverage, doubleExponentialMovingAverage},
20: 		{TripleExponentialMovingAverage, tripleExponentialMovingAverage},
21: 		{T3ExponentialMovingAverage, t3ExponentialMovingAverage},
22: 		{KaufmanAdaptiveMovingAverage, kaufmanAdaptiveMovingAverage},
23: 		{JurikMovingAverage, jurikMovingAverage},
24: 		{MesaAdaptiveMovingAverage, mesaAdaptiveMovingAverage},
25: 		{FractalAdaptiveMovingAverage, fractalAdaptiveMovingAverage},
26: 		{Momentum, momentum},
27: 		{RateOfChange, rateOfChange},
28: 		{RateOfChangePercent, rateOfChangePercent},
29: 		{RelativeStrengthIndex, relativeStrengthIndex},
30: 		{ChandeMomentumOscillator, chandeMomentumOscillator},
31: 		{BollingerBands, bollingerBands},
32: 		{Variance, variance},
33: 		{StandardDeviation, standardDeviation},
34: 		{GoertzelSpectrum, goertzelSpectrum},
35: 		{CenterOfGravityOscillator, centerOfGravityOscillator},
36: 		{CyberCycle, cyberCycle},
37: 		{InstantaneousTrendLine, instantaneousTrendLine},
38: 		{SuperSmoother, superSmoother},
39: 		{ZeroLagExponentialMovingAverage, zeroLagExponentialMovingAverage},
40: 		{ZeroLagErrorCorrectingExponentialMovingAverage, zeroLagErrorCorrectingExponentialMovingAverage},
41: 		{RoofingFilter, roofingFilter},
42: 		{TrueRange, trueRange},
43: 		{AverageTrueRange, averageTrueRange},
44: 		{NormalizedAverageTrueRange, normalizedAverageTrueRange},
45: 		{DirectionalMovementMinus, directionalMovementMinus},
46: 		{DirectionalMovementPlus, directionalMovementPlus},
47: 		{DirectionalIndicatorMinus, directionalIndicatorMinus},
48: 		{DirectionalIndicatorPlus, directionalIndicatorPlus},
49: 		{DirectionalMovementIndex, directionalMovementIndex},
50: 		{AverageDirectionalMovementIndex, averageDirectionalMovementIndex},
51: 		{AverageDirectionalMovementIndexRating, averageDirectionalMovementIndexRating},
52: 		{WilliamsPercentR, williamsPercentR},
53: 		{PercentagePriceOscillator, percentagePriceOscillator},
54: 		{AbsolutePriceOscillator, absolutePriceOscillator},
55: 		{CommodityChannelIndex, commodityChannelIndex},
56: 		{MoneyFlowIndex, moneyFlowIndex},
57: 		{OnBalanceVolume, onBalanceVolume},
58: 		{BalanceOfPower, balanceOfPower},
59: 		{RateOfChangeRatio, rateOfChangeRatio},
60: 		{PearsonsCorrelationCoefficient, pearsonsCorrelationCoefficient},
61: 		{LinearRegression, linearRegression},
62: 		{UltimateOscillator, ultimateOscillator},
63: 		{StochasticRelativeStrengthIndex, stochasticRelativeStrengthIndex},
64: 		{Stochastic, stochastic},
65: 		{Aroon, aroon},
66: 		{AdvanceDecline, advanceDecline},
67: 		{AdvanceDeclineOscillator, advanceDeclineOscillator},
68: 		{ParabolicStopAndReverse, parabolicStopAndReverse},
69: 		{TripleExponentialMovingAverageOscillator, tripleExponentialMovingAverageOscillator},
70: 		{BollingerBandsTrend, bollingerBandsTrend},
71: 		{MovingAverageConvergenceDivergence, movingAverageConvergenceDivergence},
72: 		{SineWave, sineWave},
73: 		{HilbertTransformerInstantaneousTrendLine, hilbertTransformerInstantaneousTrendLine},
74: 		{TrendCycleMode, trendCycleMode},
75: 		{DominantCycle, dominantCycle},
76: 		{last, unknown},
77: 		{Type(0), unknown},
78: 		{Type(9999), unknown},
79: 		{Type(-9999), unknown},
80: 	}
81: 
82: 	for _, tt := range tests {
83: 		exp := tt.text
84: 		act := tt.t.String()
85: 
86: 		if exp != act {
87: 			t.Errorf("'%v'.String(): expected '%v', actual '%v'", tt.t, exp, act)
88: 		}
89: 	}
90: }
91: 
92: func TestTypeIsKnown(t *testing.T) {
93: 	t.Parallel()
94: 
95: 	tests := []struct {
96: 		t       Type
97: 		boolean bool
98: 	}{
99: 		{SimpleMovingAverage, true},
100: 		{WeightedMovingAverage, true},
101: 		{TriangularMovingAverage, true},
102: 		{ExponentialMovingAverage, true},
103: 		{DoubleExponentialMovingAverage, true},
104: 		{TripleExponentialMovingAverage, true},
105: 		{T3ExponentialMovingAverage, true},
106: 		{KaufmanAdaptiveMovingAverage, true},
107: 		{JurikMovingAverage, true},
108: 		{MesaAdaptiveMovingAverage, true},
109: 		{FractalAdaptiveMovingAverage, true},
110: 		{Momentum, true},
111: 		{RateOfChange, true},
112: 		{RateOfChangePercent, true},
113: 		{RelativeStrengthIndex, true},
114: 		{ChandeMomentumOscillator, true},
115: 		{BollingerBands, true},
116: 		{Variance, true},
117: 		{StandardDeviation, true},
118: 		{GoertzelSpectrum, true},
119: 		{CenterOfGravityOscillator, true},
120: 		{CyberCycle, true},
121: 		{InstantaneousTrendLine, true},
122: 		{SuperSmoother, true},
123: 		{ZeroLagExponentialMovingAverage, true},
124: 		{ZeroLagErrorCorrectingExponentialMovingAverage, true},
125: 		{RoofingFilter, true},
126: 		{TrueRange, true},
127: 		{AverageTrueRange, true},
128: 		{NormalizedAverageTrueRange, true},
129: 		{DirectionalMovementMinus, true},
130: 		{DirectionalMovementPlus, true},
131: 		{DirectionalIndicatorMinus, true},
132: 		{DirectionalIndicatorPlus, true},
133: 		{DirectionalMovementIndex, true},
134: 		{AverageDirectionalMovementIndex, true},
135: 		{AverageDirectionalMovementIndexRating, true},
136: 		{WilliamsPercentR, true},
137: 		{PercentagePriceOscillator, true},
138: 		{AbsolutePriceOscillator, true},
139: 		{CommodityChannelIndex, true},
140: 		{MoneyFlowIndex, true},
141: 		{OnBalanceVolume, true},
142: 		{BalanceOfPower, true},
143: 		{RateOfChangeRatio, true},
144: 		{PearsonsCorrelationCoefficient, true},
145: 		{LinearRegression, true},
146: 		{UltimateOscillator, true},
147: 		{StochasticRelativeStrengthIndex, true},
148: 		{Stochastic, true},
149: 		{Aroon, true},
150: 		{AdvanceDecline, true},
151: 		{AdvanceDeclineOscillator, true},
152: 		{ParabolicStopAndReverse, true},
153: 		{TripleExponentialMovingAverageOscillator, true},
154: 		{BollingerBandsTrend, true},
155: 		{MovingAverageConvergenceDivergence, true},
156: 		{SineWave, true},
157: 		{HilbertTransformerInstantaneousTrendLine, true},
158: 		{TrendCycleMode, true},
159: 		{DominantCycle, true},
160: 		{last, false},
161: 		{Type(0), false},
162: 		{Type(9999), false},
163: 		{Type(-9999), false},
164: 	}
165: 
166: 	for _, tt := range tests {
167: 		exp := tt.boolean
168: 		act := tt.t.IsKnown()
169: 
170: 		if exp != act {
171: 			t.Errorf("'%v'.IsKnown(): expected '%v', actual '%v'", tt.t, exp, act)
172: 		}
173: 	}
174: }
175: 
176: func TestTypeMarshalJSON(t *testing.T) {
177: 	t.Parallel()
178: 
179: 	const dqs = "\""
180: 
181: 	var nilstr string
182: 	tests := []struct {
183: 		t         Type
184: 		json      string
185: 		succeeded bool
186: 	}{
187: 		{SimpleMovingAverage, dqs + simpleMovingAverage + dqs, true},
188: 		{WeightedMovingAverage, dqs + weightedMovingAverage + dqs, true},
189: 		{TriangularMovingAverage, dqs + triangularMovingAverage + dqs, true},
190: 		{ExponentialMovingAverage, dqs + exponentialMovingAverage + dqs, true},
191: 		{DoubleExponentialMovingAverage, dqs + doubleExponentialMovingAverage + dqs, true},
192: 		{TripleExponentialMovingAverage, dqs + tripleExponentialMovingAverage + dqs, true},
193: 		{T3ExponentialMovingAverage, dqs + t3ExponentialMovingAverage + dqs, true},
194: 		{KaufmanAdaptiveMovingAverage, dqs + kaufmanAdaptiveMovingAverage + dqs, true},
195: 		{JurikMovingAverage, dqs + jurikMovingAverage + dqs, true},
196: 		{MesaAdaptiveMovingAverage, dqs + mesaAdaptiveMovingAverage + dqs, true},
197: 		{FractalAdaptiveMovingAverage, dqs + fractalAdaptiveMovingAverage + dqs, true},
198: 		{Momentum, dqs + momentum + dqs, true},
199: 		{RateOfChange, dqs + rateOfChange + dqs, true},
200: 		{RateOfChangePercent, dqs + rateOfChangePercent + dqs, true},
201: 		{RelativeStrengthIndex, dqs + relativeStrengthIndex + dqs, true},
202: 		{ChandeMomentumOscillator, dqs + chandeMomentumOscillator + dqs, true},
203: 		{BollingerBands, dqs + bollingerBands + dqs, true},
204: 		{Variance, dqs + variance + dqs, true},
205: 		{StandardDeviation, dqs + standardDeviation + dqs, true},
206: 		{GoertzelSpectrum, dqs + goertzelSpectrum + dqs, true},
207: 		{CenterOfGravityOscillator, dqs + centerOfGravityOscillator + dqs, true},
208: 		{CyberCycle, dqs + cyberCycle + dqs, true},
209: 		{InstantaneousTrendLine, dqs + instantaneousTrendLine + dqs, true},
210: 		{SuperSmoother, dqs + superSmoother + dqs, true},
211: 		{ZeroLagExponentialMovingAverage, dqs + zeroLagExponentialMovingAverage + dqs, true},
212: 		{ZeroLagErrorCorrectingExponentialMovingAverage, dqs + zeroLagErrorCorrectingExponentialMovingAverage + dqs, true},
213: 		{RoofingFilter, dqs + roofingFilter + dqs, true},
214: 		{TrueRange, dqs + trueRange + dqs, true},
215: 		{AverageTrueRange, dqs + averageTrueRange + dqs, true},
216: 		{NormalizedAverageTrueRange, dqs + normalizedAverageTrueRange + dqs, true},
217: 		{DirectionalMovementMinus, dqs + directionalMovementMinus + dqs, true},
218: 		{DirectionalMovementPlus, dqs + directionalMovementPlus + dqs, true},
219: 		{DirectionalIndicatorMinus, dqs + directionalIndicatorMinus + dqs, true},
220: 		{DirectionalIndicatorPlus, dqs + directionalIndicatorPlus + dqs, true},
221: 		{DirectionalMovementIndex, dqs + directionalMovementIndex + dqs, true},
222: 		{AverageDirectionalMovementIndex, dqs + averageDirectionalMovementIndex + dqs, true},
223: 		{AverageDirectionalMovementIndexRating, dqs + averageDirectionalMovementIndexRating + dqs, true},
224: 		{WilliamsPercentR, dqs + williamsPercentR + dqs, true},
225: 		{PercentagePriceOscillator, dqs + percentagePriceOscillator + dqs, true},
226: 		{AbsolutePriceOscillator, dqs + absolutePriceOscillator + dqs, true},
227: 		{CommodityChannelIndex, dqs + commodityChannelIndex + dqs, true},
228: 		{MoneyFlowIndex, dqs + moneyFlowIndex + dqs, true},
229: 		{OnBalanceVolume, dqs + onBalanceVolume + dqs, true},
230: 		{BalanceOfPower, dqs + balanceOfPower + dqs, true},
231: 		{RateOfChangeRatio, dqs + rateOfChangeRatio + dqs, true},
232: 		{PearsonsCorrelationCoefficient, dqs + pearsonsCorrelationCoefficient + dqs, true},
233: 		{LinearRegression, dqs + linearRegression + dqs, true},
234: 		{UltimateOscillator, dqs + ultimateOscillator + dqs, true},
235: 		{StochasticRelativeStrengthIndex, dqs + stochasticRelativeStrengthIndex + dqs, true},
236: 		{Stochastic, dqs + stochastic + dqs, true},
237: 		{Aroon, dqs + aroon + dqs, true},
238: 		{AdvanceDecline, dqs + advanceDecline + dqs, true},
239: 		{AdvanceDeclineOscillator, dqs + advanceDeclineOscillator + dqs, true},
240: 		{ParabolicStopAndReverse, dqs + parabolicStopAndReverse + dqs, true},
241: 		{TripleExponentialMovingAverageOscillator, dqs + tripleExponentialMovingAverageOscillator + dqs, true},
242: 		{BollingerBandsTrend, dqs + bollingerBandsTrend + dqs, true},
243: 		{MovingAverageConvergenceDivergence, dqs + movingAverageConvergenceDivergence + dqs, true},
244: 		{SineWave, dqs + sineWave + dqs, true},
245: 		{HilbertTransformerInstantaneousTrendLine, dqs + hilbertTransformerInstantaneousTrendLine + dqs, true},
246: 		{TrendCycleMode, dqs + trendCycleMode + dqs, true},
247: 		{DominantCycle, dqs + dominantCycle + dqs, true},
248: 		{last, nilstr, false},
249: 		{Type(9999), nilstr, false},
250: 		{Type(-9999), nilstr, false},
251: 		{Type(0), nilstr, false},
252: 	}
253: 
254: 	for _, tt := range tests {
255: 		exp := tt.json
256: 		bs, err := tt.t.MarshalJSON()
257: 
258: 		if err != nil && tt.succeeded {
259: 			t.Errorf("'%v'.MarshalJSON(): expected success '%v', got error %v", tt.t, exp, err)
260: 
261: 			continue
262: 		}
263: 
264: 		if err == nil && !tt.succeeded {
265: 			t.Errorf("'%v'.MarshalJSON(): expected error, got success", tt.t)
266: 
267: 			continue
268: 		}
269: 
270: 		act := string(bs)
271: 		if exp != act {
272: 			t.Errorf("'%v'.MarshalJSON(): expected '%v', actual '%v'", tt.t, exp, act)
273: 		}
274: 	}
275: }
276: 
277: func TestTypeUnmarshalJSON(t *testing.T) {
278: 	t.Parallel()
279: 
280: 	const dqs = "\""
281: 
282: 	var zero Type
283: 	tests := []struct {
284: 		t         Type
285: 		json      string
286: 		succeeded bool
287: 	}{
288: 		{SimpleMovingAverage, dqs + simpleMovingAverage + dqs, true},
289: 		{WeightedMovingAverage, dqs + weightedMovingAverage + dqs, true},
290: 		{TriangularMovingAverage, dqs + triangularMovingAverage + dqs, true},
291: 		{ExponentialMovingAverage, dqs + exponentialMovingAverage + dqs, true},
292: 		{DoubleExponentialMovingAverage, dqs + doubleExponentialMovingAverage + dqs, true},
293: 		{TripleExponentialMovingAverage, dqs + tripleExponentialMovingAverage + dqs, true},
294: 		{T3ExponentialMovingAverage, dqs + t3ExponentialMovingAverage + dqs, true},
295: 		{KaufmanAdaptiveMovingAverage, dqs + kaufmanAdaptiveMovingAverage + dqs, true},
296: 		{JurikMovingAverage, dqs + jurikMovingAverage + dqs, true},
297: 		{MesaAdaptiveMovingAverage, dqs + mesaAdaptiveMovingAverage + dqs, true},
298: 		{FractalAdaptiveMovingAverage, dqs + fractalAdaptiveMovingAverage + dqs, true},
299: 		{Momentum, dqs + momentum + dqs, true},
300: 		{RateOfChange, dqs + rateOfChange + dqs, true},
301: 		{RateOfChangePercent, dqs + rateOfChangePercent + dqs, true},
302: 		{RelativeStrengthIndex, dqs + relativeStrengthIndex + dqs, true},
303: 		{ChandeMomentumOscillator, dqs + chandeMomentumOscillator + dqs, true},
304: 		{BollingerBands, dqs + bollingerBands + dqs, true},
305: 		{Variance, dqs + variance + dqs, true},
306: 		{StandardDeviation, dqs + standardDeviation + dqs, true},
307: 		{GoertzelSpectrum, dqs + goertzelSpectrum + dqs, true},
308: 		{CenterOfGravityOscillator, dqs + centerOfGravityOscillator + dqs, true},
309: 		{CyberCycle, dqs + cyberCycle + dqs, true},
310: 		{InstantaneousTrendLine, dqs + instantaneousTrendLine + dqs, true},
311: 		{SuperSmoother, dqs + superSmoother + dqs, true},
312: 		{ZeroLagExponentialMovingAverage, dqs + zeroLagExponentialMovingAverage + dqs, true},
313: 		{ZeroLagErrorCorrectingExponentialMovingAverage, dqs + zeroLagErrorCorrectingExponentialMovingAverage + dqs, true},
314: 		{RoofingFilter, dqs + roofingFilter + dqs, true},
315: 		{TrueRange, dqs + trueRange + dqs, true},
316: 		{AverageTrueRange, dqs + averageTrueRange + dqs, true},
317: 		{NormalizedAverageTrueRange, dqs + normalizedAverageTrueRange + dqs, true},
318: 		{DirectionalMovementMinus, dqs + directionalMovementMinus + dqs, true},
319: 		{DirectionalMovementPlus, dqs + directionalMovementPlus + dqs, true},
320: 		{DirectionalIndicatorMinus, dqs + directionalIndicatorMinus + dqs, true},
321: 		{DirectionalIndicatorPlus, dqs + directionalIndicatorPlus + dqs, true},
322: 		{DirectionalMovementIndex, dqs + directionalMovementIndex + dqs, true},
323: 		{AverageDirectionalMovementIndex, dqs + averageDirectionalMovementIndex + dqs, true},
324: 		{AverageDirectionalMovementIndexRating, dqs + averageDirectionalMovementIndexRating + dqs, true},
325: 		{WilliamsPercentR, dqs + williamsPercentR + dqs, true},
326: 		{PercentagePriceOscillator, dqs + percentagePriceOscillator + dqs, true},
327: 		{AbsolutePriceOscillator, dqs + absolutePriceOscillator + dqs, true},
328: 		{CommodityChannelIndex, dqs + commodityChannelIndex + dqs, true},
329: 		{MoneyFlowIndex, dqs + moneyFlowIndex + dqs, true},
330: 		{OnBalanceVolume, dqs + onBalanceVolume + dqs, true},
331: 		{BalanceOfPower, dqs + balanceOfPower + dqs, true},
332: 		{RateOfChangeRatio, dqs + rateOfChangeRatio + dqs, true},
333: 		{PearsonsCorrelationCoefficient, dqs + pearsonsCorrelationCoefficient + dqs, true},
334: 		{LinearRegression, dqs + linearRegression + dqs, true},
335: 		{UltimateOscillator, dqs + ultimateOscillator + dqs, true},
336: 		{StochasticRelativeStrengthIndex, dqs + stochasticRelativeStrengthIndex + dqs, true},
337: 		{Stochastic, dqs + stochastic + dqs, true},
338: 		{Aroon, dqs + aroon + dqs, true},
339: 		{AdvanceDecline, dqs + advanceDecline + dqs, true},
340: 		{AdvanceDeclineOscillator, dqs + advanceDeclineOscillator + dqs, true},
341: 		{ParabolicStopAndReverse, dqs + parabolicStopAndReverse + dqs, true},
342: 		{TripleExponentialMovingAverageOscillator, dqs + tripleExponentialMovingAverageOscillator + dqs, true},
343: 		{BollingerBandsTrend, dqs + bollingerBandsTrend + dqs, true},
344: 		{MovingAverageConvergenceDivergence, dqs + movingAverageConvergenceDivergence + dqs, true},
345: 		{SineWave, dqs + sineWave + dqs, true},
346: 		{HilbertTransformerInstantaneousTrendLine, dqs + hilbertTransformerInstantaneousTrendLine + dqs, true},
347: 		{TrendCycleMode, dqs + trendCycleMode + dqs, true},
348: 		{DominantCycle, dqs + dominantCycle + dqs, true},
349: 		{zero, "\"unknown\"", false},
350: 		{zero, "\"foobar\"", false},
351: 	}
352: 
353: 	for _, tt := range tests {
354: 		exp := tt.t
355: 		bs := []byte(tt.json)
356: 
357: 		var act Type
358: 
359: 		err := act.UnmarshalJSON(bs)
360: 		if err != nil && tt.succeeded {
361: 			t.Errorf("UnmarshalJSON('%v'): expected success '%v', got error %v", tt.json, exp, err)
362: 
363: 			continue
364: 		}
365: 
366: 		if err == nil && !tt.succeeded {
367: 			t.Errorf("MarshalJSON('%v'): expected error, got success", tt.json)
368: 
369: 			continue
370: 		}
371: 
372: 		if exp != act {
373: 			t.Errorf("MarshalJSON('%v'): expected '%v', actual '%v'", tt.json, exp, act)
374: 		}
375: 	}
376: }

(End of file - total 376 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 7.7s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/type_test.go",
  "newString": "\t\t{TrendCycleMode, trendCycleMode},\n\t\t{CoronaSpectrum, coronaSpectrum},\n\t\t{CoronaSignalToNoiseRatio, coronaSignalToNoiseRatio},\n\t\t{CoronaSwingPosition, coronaSwingPosition},\n\t\t{CoronaTrendVigor, coronaTrendVigor},\n\t\t{DominantCycle, dominantCycle},\n\t\t{last, unknown},",
  "oldString": "\t\t{TrendCycleMode, trendCycleMode},\n\t\t{DominantCycle, dominantCycle},\n\t\t{last, unknown},"
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
  "filePath": "/home/dev/zpano/go/indicators/core/type_test.go",
  "newString": "\t\t{TrendCycleMode, true},\n\t\t{CoronaSpectrum, true},\n\t\t{CoronaSignalToNoiseRatio, true},\n\t\t{CoronaSwingPosition, true},\n\t\t{CoronaTrendVigor, true},\n\t\t{DominantCycle, true},",
  "oldString": "\t\t{TrendCycleMode, true},\n\t\t{DominantCycle, true},"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 5.7s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/type_test.go",
  "newString": "\t\t{TrendCycleMode, dqs + trendCycleMode + dqs, true},\n\t\t{CoronaSpectrum, dqs + coronaSpectrum + dqs, true},\n\t\t{CoronaSignalToNoiseRatio, dqs + coronaSignalToNoiseRatio + dqs, true},\n\t\t{CoronaSwingPosition, dqs + coronaSwingPosition + dqs, true},\n\t\t{CoronaTrendVigor, dqs + coronaTrendVigor + dqs, true},\n\t\t{DominantCycle, dqs + dominantCycle + dqs, true},\n\t\t{last, nilstr, false},",
  "oldString": "\t\t{TrendCycleMode, dqs + trendCycleMode + dqs, true},\n\t\t{DominantCycle, dqs + dominantCycle + dqs, true},\n\t\t{last, nilstr, false},"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 5.7s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/type_test.go",
  "newString": "\t\t{TrendCycleMode, dqs + trendCycleMode + dqs, true},\n\t\t{CoronaSpectrum, dqs + coronaSpectrum + dqs, true},\n\t\t{CoronaSignalToNoiseRatio, dqs + coronaSignalToNoiseRatio + dqs, true},\n\t\t{CoronaSwingPosition, dqs + coronaSwingPosition + dqs, true},\n\t\t{CoronaTrendVigor, dqs + coronaTrendVigor + dqs, true},\n\t\t{DominantCycle, dqs + dominantCycle + dqs, true},\n\t\t{zero, \"\\\"unknown\\\"\", false},",
  "oldString": "\t\t{TrendCycleMode, dqs + trendCycleMode + dqs, true},\n\t\t{DominantCycle, dqs + dominantCycle + dqs, true},\n\t\t{zero, \"\\\"unknown\\\"\", false},"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 5.0s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./indicators/core/... 2>&1 | tail -15",
  "description": "Run core type tests",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
ok  	zpano/indicators/core	0.002s
ok  	zpano/indicators/core/frequencyresponse	0.002s
ok  	zpano/indicators/core/outputs	(cached)

```

---

## Assistant (Build · claude-opus-4.7 · 27.8s)

_Thinking:_

 The core tests are passing, so now I'm examining the output kind patterns in the trend cycle mode files to understand how to structure CoronaSpectrum properly. I've completed the initial Corona helper setup and type registration, which is a solid foundation to build from.

Looking at the scope ahead, building out all four Corona indicators with their full test suites and parameter structures would be a significant undertaking—each one likely spanning 300-500 lines of code plus tests. Since I've hit a natural checkpoint with meaningful progress and the user asked me to check in when I reach decision points, I should report back on what's been accomplished and let them decide whether to continue pushing forward or wrap up here.

Good stopping point. Let me report progress:

## Progress this turn

### Completed
1. **Fixed 4 bugs in Corona helper** during review vs MBST source:
   - `MaximalAmplitudeSquared` reset to 0 per-bar (was running max across time)
   - First sample is stored-only, no processing (matches MBST `sampleCount==1` short-circuit)
   - `DominantCycle` reset to 0 each bar before the weighted-avg loop
   - Dominant-cycle median buffer initialized to `MaxFloat64` sentinels (matches MBST partial-fill median behavior)
2. **Corona helper fully tested** — 3 unit tests pass:
   - `TestCoronaDefaultSmoke` (priming at sample 12, DC oscillates 6.0→19.8 across 252 bars)
   - `TestCoronaNaNInputIsNoOp`
   - `TestCoronaInvalidParams` (5 sub-cases)
3. **4 new `core.Type` constants registered** — `CoronaSpectrum`, `CoronaSignalToNoiseRatio`, `CoronaSwingPosition`, `CoronaTrendVigor` — with JSON marshal/unmarshal + all 3 existing core type tests extended. All passing.

### Design decisions locked (from user answers)
- Unprimed heatmap → `NewEmptyHeatmap(time, paramFirst, paramLast, paramRes)`
- Mnemonic style → zpano convention: `cspect(...)`, `cspect-dc(...)`, `csnr(...)`, etc.
- Register all 4 `core.Type` constants ✅ done
- Port to TS only after all 4 Go indicators are green (pause for review)

### Files touched
- `go/indicators/johnehlers/corona/corona.go` (bug fixes)
- `go/indicators/johnehlers/corona/corona_smoke_test.go` (new)
- `go/indicators/core/type.go` (4 new constants)
- `go/indicators/core/type_test.go` (extended 3 tables)

### Next (Phase 2)
Build **CoronaSpectrum** indicator — first zpano heatmap indicator. Pattern will be:
- `coronaspectrum/params.go` (MinRasterValue, MaxRasterValue, MinParameterValue, MaxParameterValue, HighPassFilterCutoff, Bar/Quote/Trade components)
- `coronaspectrum/output.go` (output kinds: Value=Heatmap, DominantCycle=Scalar, DominantCycleMedian=Scalar)
- `coronaspectrum/coronaspectrum.go` (~300–400 lines: mnemonic building, metadata, Update(sample)→(heatmap, dc, dcMed), Update{Scalar,Bar,Quote,Trade} mixed output slice)
- Tests: construction, mnemonic, metadata, NaN handling, numeric snapshot across 252-bar TA-Lib series

Shall I continue with CoronaSpectrum, or pause here for review? Given the volume (4 indicators + tests still ahead), now is a reasonable check-in point if you'd like to review what's been committed so far.

---

## User

please continue

---

## Assistant (Build · claude-opus-4.7 · 7.3s)

_Thinking:_

 Moving into Phase 2 with CoronaSpectrum, I need to examine TCM's structure closely and check whether GoertzelSpectrum already has a heatmap implementation that I can reference, since CoronaSpectrum will be the first indicator using that output type.

**Tool: bash**

**Input:**
```json
{
  "command": "ls go/indicators/johnehlers/ 2>&1; echo ---; find go/indicators -name \"goertzel*\" 2>&1",
  "description": "Look for existing heatmap indicator examples"
}
```

**Output:**
```
centerofgravityoscillator
corona
cybercycle
doc.go
dominantcycle
fractaladaptivemovingaverage
hilberttransformer
hilberttransformerinstantaneoustrendline
instantaneoustrendline
mesaadaptivemovingaverage
roofingfilter
sinewave
supersmoother
trendcyclemode
zerolagerrorcorrectingexponentialmovingaverage
zerolagexponentialmovingaverage
---

```

---

## Assistant (Build · claude-opus-4.7 · 5.6s)

_Thinking:_

 I notice that GoertzelSpectrum hasn't been implemented yet, so CoronaSpectrum is actually the first heatmap indicator. Now I'm examining TCM's output and parameters to understand how it works.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput.go</path>
<type>file</type>
<content>1: package trendcyclemode
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
12: 	// Value is the Trend-versus-Cycle Mode value: +1 in trend mode, -1 in cycle mode.
13: 	Value Output = iota + 1
14: 	// IsTrendMode is 1 if the trend mode is declared, 0 otherwise.
15: 	IsTrendMode
16: 	// IsCycleMode is 1 if the cycle mode is declared, 0 otherwise (= 1 - IsTrendMode).
17: 	IsCycleMode
18: 	// InstantaneousTrendLine is the WMA-smoothed instantaneous trend line.
19: 	InstantaneousTrendLine
20: 	// SineWave is the sine wave value, sin(phase·Deg2Rad).
21: 	SineWave
22: 	// SineWaveLead is the sine wave lead value, sin((phase+45)·Deg2Rad).
23: 	SineWaveLead
24: 	// DominantCyclePeriod is the smoothed dominant cycle period.
25: 	DominantCyclePeriod
26: 	// DominantCyclePhase is the dominant cycle phase, in degrees.
27: 	DominantCyclePhase
28: 	outputLast
29: )
30: 
31: const (
32: 	valueStr                  = "value"
33: 	isTrendModeStr            = "isTrendMode"
34: 	isCycleModeStr            = "isCycleMode"
35: 	instantaneousTrendLineStr = "instantaneousTrendLine"
36: 	sineWaveStr               = "sineWave"
37: 	sineWaveLeadStr           = "sineWaveLead"
38: 	dominantCyclePeriodStr    = "dominantCyclePeriod"
39: 	dominantCyclePhaseStr     = "dominantCyclePhase"
40: 	unknownStr                = "unknown"
41: )
42: 
43: // String implements the Stringer interface.
44: func (o Output) String() string {
45: 	switch o {
46: 	case Value:
47: 		return valueStr
48: 	case IsTrendMode:
49: 		return isTrendModeStr
50: 	case IsCycleMode:
51: 		return isCycleModeStr
52: 	case InstantaneousTrendLine:
53: 		return instantaneousTrendLineStr
54: 	case SineWave:
55: 		return sineWaveStr
56: 	case SineWaveLead:
57: 		return sineWaveLeadStr
58: 	case DominantCyclePeriod:
59: 		return dominantCyclePeriodStr
60: 	case DominantCyclePhase:
61: 		return dominantCyclePhaseStr
62: 	default:
63: 		return unknownStr
64: 	}
65: }
66: 
67: // IsKnown determines if this output is known.
68: func (o Output) IsKnown() bool {
69: 	return o >= Value && o < outputLast
70: }
71: 
72: // MarshalJSON implements the Marshaler interface.
73: func (o Output) MarshalJSON() ([]byte, error) {
74: 	const (
75: 		errFmt = "cannot marshal '%s': unknown trend cycle mode output"
76: 		extra  = 2   // Two bytes for quotes.
77: 		dqc    = '"' // Double quote character.
78: 	)
79: 
80: 	s := o.String()
81: 	if s == unknownStr {
82: 		return nil, fmt.Errorf(errFmt, s)
83: 	}
84: 
85: 	b := make([]byte, 0, len(s)+extra)
86: 	b = append(b, dqc)
87: 	b = append(b, s...)
88: 	b = append(b, dqc)
89: 
90: 	return b, nil
91: }
92: 
93: // UnmarshalJSON implements the Unmarshaler interface.
94: func (o *Output) UnmarshalJSON(data []byte) error {
95: 	const (
96: 		errFmt = "cannot unmarshal '%s': unknown trend cycle mode output"
97: 		dqs    = "\"" // Double quote string.
98: 	)
99: 
100: 	d := bytes.Trim(data, dqs)
101: 	s := string(d)
102: 
103: 	switch s {
104: 	case valueStr:
105: 		*o = Value
106: 	case isTrendModeStr:
107: 		*o = IsTrendMode
108: 	case isCycleModeStr:
109: 		*o = IsCycleMode
110: 	case instantaneousTrendLineStr:
111: 		*o = InstantaneousTrendLine
112: 	case sineWaveStr:
113: 		*o = SineWave
114: 	case sineWaveLeadStr:
115: 		*o = SineWaveLead
116: 	case dominantCyclePeriodStr:
117: 		*o = DominantCyclePeriod
118: 	case dominantCyclePhaseStr:
119: 		*o = DominantCyclePhase
120: 	default:
121: 		return fmt.Errorf(errFmt, s)
122: 	}
123: 
124: 	return nil
125: }

(End of file - total 125 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.7s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeparams.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeparams.go</path>
<type>file</type>
<content>1: package trendcyclemode
2: 
3: import (
4: 	"zpano/entities"
5: 	"zpano/indicators/johnehlers/hilberttransformer"
6: )
7: 
8: // Params describes parameters to create an instance of the indicator.
9: type Params struct {
10: 	// EstimatorType is the type of cycle estimator to use.
11: 	// The default value is hilberttransformer.HomodyneDiscriminator.
12: 	EstimatorType hilberttransformer.CycleEstimatorType
13: 
14: 	// EstimatorParams describes parameters to create an instance
15: 	// of the Hilbert transformer cycle estimator.
16: 	EstimatorParams hilberttransformer.CycleEstimatorParams
17: 
18: 	// AlphaEmaPeriodAdditional is the value of α (0 < α ≤ 1) used in EMA
19: 	// for additional smoothing of the instantaneous period.
20: 	//
21: 	// The default value is 0.33.
22: 	AlphaEmaPeriodAdditional float64
23: 
24: 	// TrendLineSmoothingLength is the additional WMA smoothing length used to smooth the trend line.
25: 	// The valid values are 2, 3, 4. The default value is 4.
26: 	TrendLineSmoothingLength int
27: 
28: 	// CyclePartMultiplier is the multiplier to the dominant cycle period used to determine
29: 	// the window length to calculate the trend line. The typical values are in [0.5, 1.5].
30: 	// The default value is 1.0. Valid range is (0, 10].
31: 	CyclePartMultiplier float64
32: 
33: 	// SeparationPercentage is the threshold (in percent) above which a wide separation between
34: 	// the WMA-smoothed price and the instantaneous trend line forces the trend mode.
35: 	//
36: 	// The default value is 1.5. Valid range is (0, 100].
37: 	SeparationPercentage float64
38: 
39: 	// BarComponent indicates the component of a bar to use when updating the indicator with a bar sample.
40: 	//
41: 	// If zero, the default (BarMedianPrice) is used. Since the default differs from the framework
42: 	// default bar component, it is always shown in the indicator mnemonic.
43: 	BarComponent entities.BarComponent
44: 
45: 	// QuoteComponent indicates the component of a quote to use when updating the indicator with a quote sample.
46: 	//
47: 	// If zero, the default (QuoteMidPrice) is used and the component is not shown in the indicator mnemonic.
48: 	QuoteComponent entities.QuoteComponent
49: 
50: 	// TradeComponent indicates the component of a trade to use when updating the indicator with a trade sample.
51: 	//
52: 	// If zero, the default (TradePrice) is used and the component is not shown in the indicator mnemonic.
53: 	TradeComponent entities.TradeComponent
54: }

(End of file - total 54 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput_test.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput_test.go</path>
<type>file</type>
<content>1: //nolint:testpackage
2: package trendcyclemode
3: 
4: import (
5: 	"testing"
6: )
7: 
8: func TestOutputString(t *testing.T) {
9: 	t.Parallel()
10: 
11: 	tests := []struct {
12: 		o    Output
13: 		text string
14: 	}{
15: 		{Value, valueStr},
16: 		{IsTrendMode, isTrendModeStr},
17: 		{IsCycleMode, isCycleModeStr},
18: 		{InstantaneousTrendLine, instantaneousTrendLineStr},
19: 		{SineWave, sineWaveStr},
20: 		{SineWaveLead, sineWaveLeadStr},
21: 		{DominantCyclePeriod, dominantCyclePeriodStr},
22: 		{DominantCyclePhase, dominantCyclePhaseStr},
23: 		{outputLast, unknownStr},
24: 		{Output(0), unknownStr},
25: 		{Output(9999), unknownStr},
26: 		{Output(-9999), unknownStr},
27: 	}
28: 
29: 	for _, tt := range tests {
30: 		exp := tt.text
31: 		act := tt.o.String()
32: 
33: 		if exp != act {
34: 			t.Errorf("'%v'.String(): expected '%v', actual '%v'", tt.o, exp, act)
35: 		}
36: 	}
37: }
38: 
39: func TestOutputIsKnown(t *testing.T) {
40: 	t.Parallel()
41: 
42: 	tests := []struct {
43: 		o       Output
44: 		boolean bool
45: 	}{
46: 		{Value, true},
47: 		{IsTrendMode, true},
48: 		{IsCycleMode, true},
49: 		{InstantaneousTrendLine, true},
50: 		{SineWave, true},
51: 		{SineWaveLead, true},
52: 		{DominantCyclePeriod, true},
53: 		{DominantCyclePhase, true},
54: 		{outputLast, false},
55: 		{Output(0), false},
56: 		{Output(9999), false},
57: 		{Output(-9999), false},
58: 	}
59: 
60: 	for _, tt := range tests {
61: 		exp := tt.boolean
62: 		act := tt.o.IsKnown()
63: 
64: 		if exp != act {
65: 			t.Errorf("'%v'.IsKnown(): expected '%v', actual '%v'", tt.o, exp, act)
66: 		}
67: 	}
68: }
69: 
70: func TestOutputMarshalJSON(t *testing.T) {
71: 	t.Parallel()
72: 
73: 	const dqs = "\""
74: 
75: 	var nilstr string
76: 	tests := []struct {
77: 		o         Output
78: 		json      string
79: 		succeeded bool
80: 	}{
81: 		{Value, dqs + valueStr + dqs, true},
82: 		{IsTrendMode, dqs + isTrendModeStr + dqs, true},
83: 		{IsCycleMode, dqs + isCycleModeStr + dqs, true},
84: 		{InstantaneousTrendLine, dqs + instantaneousTrendLineStr + dqs, true},
85: 		{SineWave, dqs + sineWaveStr + dqs, true},
86: 		{SineWaveLead, dqs + sineWaveLeadStr + dqs, true},
87: 		{DominantCyclePeriod, dqs + dominantCyclePeriodStr + dqs, true},
88: 		{DominantCyclePhase, dqs + dominantCyclePhaseStr + dqs, true},
89: 		{outputLast, nilstr, false},
90: 		{Output(9999), nilstr, false},
91: 		{Output(-9999), nilstr, false},
92: 		{Output(0), nilstr, false},
93: 	}
94: 
95: 	for _, tt := range tests {
96: 		exp := tt.json
97: 		bs, err := tt.o.MarshalJSON()
98: 
99: 		if err != nil && tt.succeeded {
100: 			t.Errorf("'%v'.MarshalJSON(): expected success '%v', got error %v", tt.o, exp, err)
101: 
102: 			continue
103: 		}
104: 
105: 		if err == nil && !tt.succeeded {
106: 			t.Errorf("'%v'.MarshalJSON(): expected error, got success", tt.o)
107: 
108: 			continue
109: 		}
110: 
111: 		act := string(bs)
112: 		if exp != act {
113: 			t.Errorf("'%v'.MarshalJSON(): expected '%v', actual '%v'", tt.o, exp, act)
114: 		}
115: 	}
116: }
117: 
118: func TestOutputUnmarshalJSON(t *testing.T) {
119: 	t.Parallel()
120: 
121: 	const dqs = "\""
122: 
123: 	var zero Output
124: 	tests := []struct {
125: 		o         Output
126: 		json      string
127: 		succeeded bool
128: 	}{
129: 		{Value, dqs + valueStr + dqs, true},
130: 		{IsTrendMode, dqs + isTrendModeStr + dqs, true},
131: 		{IsCycleMode, dqs + isCycleModeStr + dqs, true},
132: 		{InstantaneousTrendLine, dqs + instantaneousTrendLineStr + dqs, true},
133: 		{SineWave, dqs + sineWaveStr + dqs, true},
134: 		{SineWaveLead, dqs + sineWaveLeadStr + dqs, true},
135: 		{DominantCyclePeriod, dqs + dominantCyclePeriodStr + dqs, true},
136: 		{DominantCyclePhase, dqs + dominantCyclePhaseStr + dqs, true},
137: 		{zero, dqs + unknownStr + dqs, false},
138: 		{zero, dqs + "foobar" + dqs, false},
139: 	}
140: 
141: 	for _, tt := range tests {
142: 		exp := tt.o
143: 		bs := []byte(tt.json)
144: 
145: 		var o Output
146: 
147: 		err := o.UnmarshalJSON(bs)
148: 		if err != nil && tt.succeeded {
149: 			t.Errorf("UnmarshalJSON('%v'): expected success '%v', got error %v", tt.json, exp, err)
150: 
151: 			continue
152: 		}
153: 
154: 		if err == nil && !tt.succeeded {
155: 			t.Errorf("MarshalJSON('%v'): expected error, got success", tt.json)
156: 
157: 			continue
158: 		}
159: 
160: 		if exp != o {
161: 			t.Errorf("MarshalJSON('%v'): expected '%v', actual '%v'", tt.json, exp, o)
162: 		}
163: 	}
164: }

(End of file - total 164 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 20.0s)

_Thinking:_

 I've reviewed the existing test patterns and the relevant entity types, so I have enough context to start building CoronaSpectrum. I'll create the params file with the necessary constants like MinRasterValue, MaxRasterValue, and the parameter bounds.

Now I'm laying out the remaining files: the output enum with its three variants, the main component struct with its initialization and mnemonic building logic, metadata handling, and the update method. I'll also write tests for marshaling behavior and snapshot validation to ensure everything works correctly.

Good, I have the patterns. Let me build CoronaSpectrum. First the params file:

**Tool: write**

**Input:**
```json
{
  "content": "package coronaspectrum\n\nimport \"zpano/entities\"\n\n// Params describes parameters to create an instance of the CoronaSpectrum indicator.\ntype Params struct {\n\t// MinRasterValue is the minimal raster value (z) of the heatmap, in decibels.\n\t// Corresponds to the CoronaLowerDecibels threshold. The default value is 6.\n\tMinRasterValue float64\n\n\t// MaxRasterValue is the maximal raster value (z) of the heatmap, in decibels.\n\t// Corresponds to the CoronaUpperDecibels threshold. The default value is 20.\n\tMaxRasterValue float64\n\n\t// MinParameterValue is the minimal ordinate (y) value of the heatmap, representing\n\t// the minimal cycle period covered by the filter bank. The default value is 6.\n\tMinParameterValue float64\n\n\t// MaxParameterValue is the maximal ordinate (y) value of the heatmap, representing\n\t// the maximal cycle period covered by the filter bank. The default value is 30.\n\tMaxParameterValue float64\n\n\t// HighPassFilterCutoff is the high-pass filter cutoff (de-trending period) used by\n\t// the inner Corona engine. Suggested values are 20, 30, 100. The default value is 30.\n\tHighPassFilterCutoff int\n\n\t// BarComponent indicates the component of a bar to use when updating the indicator with a bar sample.\n\t//\n\t// If zero, the default (BarMedianPrice) is used, matching Ehlers' reference which operates on\n\t// (High+Low)/2. Since this differs from the framework-wide default, it is always shown in the\n\t// indicator mnemonic.\n\tBarComponent entities.BarComponent\n\n\t// QuoteComponent indicates the component of a quote to use when updating the indicator with a quote sample.\n\t//\n\t// If zero, the default (QuoteMidPrice) is used and the component is not shown in the indicator mnemonic.\n\tQuoteComponent entities.QuoteComponent\n\n\t// TradeComponent indicates the component of a trade to use when updating the indicator with a trade sample.\n\t//\n\t// If zero, the default (TradePrice) is used and the component is not shown in the indicator mnemonic.\n\tTradeComponent entities.TradeComponent\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrumparams.go"
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

## Assistant (Compaction · claude-opus-4.7 · 76.3s)

---
## Goal

Port MBST's **Ehlers Corona Charts suite** (TASC November 2008) from `/home/dev/zpano/mbst-to-convert/john-ehlers/corona/` into zpano:

- **Corona** shared helper (not a registered indicator)
- **CoronaSpectrum** — heatmap + DominantCycle + DominantCycleMedian scalars
- **CoronaSignalToNoiseRatio** — heatmap + SNR scalar (needs OHLCV: uses High/Low for noise)
- **CoronaSwingPosition** — heatmap + SwingPosition scalar
- **CoronaTrendVigor** — heatmap + TrendVigor scalar

Deliverables in Go (`go/indicators/johnehlers/<pkg>/`) then TypeScript (`ts/indicators/john-ehlers/<pkg>/`). These will be the **first heatmap-producing indicators in zpano**.

## Instructions

### Design decisions (confirmed with user)
- Unprimed bars → emit `outputs.NewEmptyHeatmap(time, paramFirst, paramLast, paramRes)` (scalars are NaN, matching TCM).
- Mnemonic style → **zpano convention**: `cspect(…)`, `cspect-dc(…)`, `cspect-dcm(…)`, `csnr(…)`, `cswing(…)`, `ctv(…)`. Not MBST CamelCase.
- All 4 `core.Type` constants registered now (done).
- After all 4 Go indicators are green + tested → **pause for user review** before TS port.

### Architecture
- Corona is a helper package (like `hilberttransformer/`); each of the 4 indicators **owns its own Corona instance** (no slave/shared pattern).
- Follow `trendcyclemode/` as the multi-output indicator template.

### Algorithm decisions from 3-way reference cross-validation (MBST + eSignal + NinjaTrader + WealthLab)
- **Swing BP2 momentum**: 2-bar (`sample - samplePrev2`) — MBST/WealthLab/NinjaTrader. eSignal 1-bar is a bug.
- **TrendVigor lookback**: FIX the MBST bug — use `DomCycMedian - 1` directly (floor at reasonable value).
- **SNR signal EMA coefficients**: Keep 0.2/0.9 (sums to 1.1) — Ehlers' intentional code.
- **DC weighted average**: N=12..60 indexing (NinjaTrader's n=11..59 is off-by-one).
- **DominantCycleMedian clamp**: Apply `< minPeriod` floor to the **median**, not to raw DC.

### Defaults
- Corona: `HighPassFilterCutoff=30, MinimalPeriod=6, MaximalPeriod=30, DecibelsLowerThreshold=6, DecibelsUpperThreshold=20`
- CSpectrum: `minRasterValue=6, maxRasterValue=20, minParameterValue=6, maxParameterValue=30, highPassFilterCutoff=30, BarComponent=Median`
- CSNR: `rasterLength=50, maxRasterValue=20, minParameterValue=1, maxParameterValue=11, BarComponent=Median`
- CSwing: `rasterLength=50, maxRasterValue=20, minParameterValue=-5, maxParameterValue=5`
- CTV: same structure, range `[-10, 10]`

### Test strategy
- Reuse 252-entry TA-Lib MAMA data (copied locally into `corona_smoke_test.go` as `talibInput()`).
- Tolerance 1e-4 (matches TCM).
- Per-indicator standard tests: metadata, mnemonics, param validation, NaN no-op, numeric snapshots.

## Discoveries

### Corona implementation bug fixes (vs my initial port)
During verification against MBST `Corona.cs` I found and fixed **4 bugs** in my initial helper:
1. `MaximalAmplitudeSquared` must reset to 0 per-bar (was running max across time)
2. First sample is store-only (MBST `if (1 == ++sampleCount)` short-circuit) — no HP / FIR / filter-bank processing
3. `DominantCycle` must reset to 0 each bar **before** the weighted-average loop; denom==0 ⇒ remains 0 then clamped up to MinimalPeriod
4. `dominantCycleBuffer` must be initialized to `math.MaxFloat64` sentinels so partial-fill median returns MaxValue (not a real-sample median) until buffer has ≥3 real samples

### zpano infrastructure
- `outputs.HeatmapType` already exists in `go/indicators/core/outputs/type.go`; `outputs.NewHeatmap(time, parameterFirst, parameterLast, parameterResolution, valueMin, valueMax, values)` / `outputs.NewEmptyHeatmap(...)`.
- No existing indicator uses Heatmap yet — Corona Charts are establishing the pattern.
- TCM pattern (`trendcyclemode/`): separate `*output.go` (Output enum + JSON marshal/unmarshal), `*params.go`, main file with `Update(sample) (…)` returning tuple, plus `UpdateScalar/Bar/Quote/Trade` returning `core.Output` slice of `entities.Scalar`s / `*outputs.Heatmap`. `core.ComponentTripleMnemonic(bc, qc, tc)` composes the component suffix.

### Smoke test results (Corona helper)
- Primes at sample 12 (= MinimalPeriod*2) ✓
- DC oscillates realistically across 252-bar TA-Lib series: 17.8, 15.2, 14.7, 17.5, 19.8, 6.0 at sampled bars
- 3 unit tests pass: `TestCoronaDefaultSmoke`, `TestCoronaNaNInputIsNoOp`, `TestCoronaInvalidParams` (5 sub-cases)

## Accomplished

### ✅ Completed
1. Full 14-file analysis of `mbst-to-convert/john-ehlers/corona/` (MBST + 3 reference impls).
2. Cross-reference bug identification across impls.
3. Implementation plan (Phases 0–6) with defaults + test strategy.
4. **Phase 1: Corona helper package — done, tested.**
   - `go/indicators/johnehlers/corona/doc.go`
   - `go/indicators/johnehlers/corona/params.go`
   - `go/indicators/johnehlers/corona/corona.go` (compiled, 4 bugs fixed)
   - `go/indicators/johnehlers/corona/corona_smoke_test.go` (3 tests passing)
5. **4 new `core.Type` constants registered** in `go/indicators/core/type.go`:
   - `CoronaSpectrum`, `CoronaSignalToNoiseRatio`, `CoronaSwingPosition`, `CoronaTrendVigor`
   - JSON marshal/unmarshal + updated `go/indicators/core/type_test.go` tables (string / IsKnown / MarshalJSON / UnmarshalJSON)
   - `go test ./indicators/core/...` passes.
6. **Phase 2 started: CoronaSpectrum params file written** — `go/indicators/johnehlers/coronaspectrum/coronaspectrumparams.go` (Params struct with MinRasterValue / MaxRasterValue / MinParameterValue / MaxParameterValue / HighPassFilterCutoff + Bar/Quote/Trade components).

### 🔄 In progress (Phase 2: CoronaSpectrum)
Next files to create in `go/indicators/johnehlers/coronaspectrum/`:
- `coronaspectrumoutput.go` — `Output` enum: `Value` (Heatmap), `DominantCycle` (Scalar), `DominantCycleMedian` (Scalar) + String / IsKnown / MarshalJSON / UnmarshalJSON (pattern from `trendcyclemode/trendcyclemodeoutput.go`)
- `coronaspectrumoutput_test.go` — 4-table tests mirroring `trendcyclemodeoutput_test.go`
- `coronaspectrum.go` — the indicator: owns a `*corona.Corona`, mnemonic `cspect(minRaster, maxRaster, minParam, maxParam, hpCutoff, componentTriple)` + `cspect-dc(hpCutoff, componentTriple)` + `cspect-dcm(hpCutoff, componentTriple)`; `Metadata()` lists 3 outputs (HeatmapType + 2 ScalarType); `Update(sample) (*outputs.Heatmap, float64, float64)`; `UpdateScalar/Bar/Quote/Trade` returning `core.Output` 3-slice.
  - Unprimed bars: return `outputs.NewEmptyHeatmap(time, minParameterValue, maxParameterValue, paramResolution)` and NaN scalars.
  - Primed: build intensity slice from `c.FilterBank()[i].Decibels`, length = `FilterBankLength()`. ParameterResolution = `(FilterBankLength-1) / (maxParam - minParam)` (2 samples per period since half-period stepping).
  - `core.ComponentTripleMnemonic(bc, qc, tc)` for suffix.
- `coronaspectrum_test.go` — construction, mnemonic format, Metadata output kinds/types, NaN no-op, numeric snapshot using the 252-entry TA-Lib input (must capture golden values from a first run; tolerance 1e-4).

### ⏳ Remaining (after CoronaSpectrum)
- Phase 3: CoronaSignalToNoiseRatio (takes full OHLCV because noise = (High-Low)² smoothed; need to decide how `UpdateScalar` handles missing H/L — likely falls back to sample-as-price with zero noise or returns NaN).
- Phase 4: CoronaSwingPosition.
- Phase 5: CoronaTrendVigor (apply DomCyc-1 lookback fix).
- **Pause for user review.**
- Phase 6: TS port of all 4 to `ts/indicators/john-ehlers/<pkg>/`.
- Update `.opencode/skills/mbst-indicator-conversion/SKILL.md` with Corona learnings (first heatmap indicator, multi-ref cross-validation).

## Relevant files / directories

### Created / modified this task
- `/home/dev/zpano/go/indicators/johnehlers/corona/doc.go`
- `/home/dev/zpano/go/indicators/johnehlers/corona/params.go`
- `/home/dev/zpano/go/indicators/johnehlers/corona/corona.go` — Corona helper (4 bug fixes applied)
- `/home/dev/zpano/go/indicators/johnehlers/corona/corona_smoke_test.go` — 3 tests, contains `talibInput()` 252-entry reference data
- `/home/dev/zpano/go/indicators/core/type.go` — 4 new constants + strings + switch cases
- `/home/dev/zpano/go/indicators/core/type_test.go` — extended 4 tables
- `/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrumparams.go` — written, compiles

### MBST source being ported
- `/home/dev/zpano/mbst-to-convert/john-ehlers/corona/` — entire directory:
  - `Corona.cs` (done), `CoronaSpectrum.cs` (porting now), `CoronaSignalToNoiseRatio.cs`, `CoronaSwingPosition.cs`, `CoronaTrendVigor.cs`
  - Reference impls: `e-signal-*.efs` (4), `ninja-trader-*.cs` (4), `wealth-lab-implementation.cs`

### Reference templates in zpano to follow
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/` — **primary template** for multi-output indicator (trendcyclemode.go, output.go, params.go + tests)
- `/home/dev/zpano/go/indicators/johnehlers/hilberttransformer/` — helper-package pattern
- `/home/dev/zpano/go/indicators/johnehlers/dominantcycle/dominantcycle.go` — wrapping a helper, multi-output
- `/home/dev/zpano/go/indicators/johnehlers/sinewave/sinewave_test.go` — origin of TA-Lib test data
- `/home/dev/zpano/go/indicators/core/outputs/heatmap.go` — Heatmap output type (first-use in CoronaSpectrum)
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/` — TS template for multi-output (for Phase 6)

### Skills
- `.opencode/skills/indicator-architecture` — loaded
- `.opencode/skills/mbst-indicator-conversion` — loaded; to be updated after completion

### Immediate next action
Create `go/indicators/johnehlers/coronaspectrum/coronaspectrumoutput.go` (mirror TCM output file with 3 outputs: Value / DominantCycle / DominantCycleMedian) + its test file, then write `coronaspectrum.go` with the heatmap emission pattern. Verify with `cd /home/dev/zpano/go && go test ./indicators/johnehlers/coronaspectrum/...` before moving to Phase 3.

---

## User

---

## Assistant (Build · claude-opus-4.7 · 4.7s)

_Thinking:_

 Now I'm moving on to set up the output file for CoronaSpectrum along with its corresponding test and main indicator files.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/trendcyclemode</path>
<type>directory</type>
<entries>
trendcyclemode_test.go
trendcyclemode.go
trendcyclemodeoutput_test.go
trendcyclemodeoutput.go
trendcyclemodeparams.go

(5 entries)
</entries>
```

---

## Assistant (Build · claude-opus-4.7 · 3.2s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput.go</path>
<type>file</type>
<content>1: package trendcyclemode
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
12: 	// Value is the Trend-versus-Cycle Mode value: +1 in trend mode, -1 in cycle mode.
13: 	Value Output = iota + 1
14: 	// IsTrendMode is 1 if the trend mode is declared, 0 otherwise.
15: 	IsTrendMode
16: 	// IsCycleMode is 1 if the cycle mode is declared, 0 otherwise (= 1 - IsTrendMode).
17: 	IsCycleMode
18: 	// InstantaneousTrendLine is the WMA-smoothed instantaneous trend line.
19: 	InstantaneousTrendLine
20: 	// SineWave is the sine wave value, sin(phase·Deg2Rad).
21: 	SineWave
22: 	// SineWaveLead is the sine wave lead value, sin((phase+45)·Deg2Rad).
23: 	SineWaveLead
24: 	// DominantCyclePeriod is the smoothed dominant cycle period.
25: 	DominantCyclePeriod
26: 	// DominantCyclePhase is the dominant cycle phase, in degrees.
27: 	DominantCyclePhase
28: 	outputLast
29: )
30: 
31: const (
32: 	valueStr                  = "value"
33: 	isTrendModeStr            = "isTrendMode"
34: 	isCycleModeStr            = "isCycleMode"
35: 	instantaneousTrendLineStr = "instantaneousTrendLine"
36: 	sineWaveStr               = "sineWave"
37: 	sineWaveLeadStr           = "sineWaveLead"
38: 	dominantCyclePeriodStr    = "dominantCyclePeriod"
39: 	dominantCyclePhaseStr     = "dominantCyclePhase"
40: 	unknownStr                = "unknown"
41: )
42: 
43: // String implements the Stringer interface.
44: func (o Output) String() string {
45: 	switch o {
46: 	case Value:
47: 		return valueStr
48: 	case IsTrendMode:
49: 		return isTrendModeStr
50: 	case IsCycleMode:
51: 		return isCycleModeStr
52: 	case InstantaneousTrendLine:
53: 		return instantaneousTrendLineStr
54: 	case SineWave:
55: 		return sineWaveStr
56: 	case SineWaveLead:
57: 		return sineWaveLeadStr
58: 	case DominantCyclePeriod:
59: 		return dominantCyclePeriodStr
60: 	case DominantCyclePhase:
61: 		return dominantCyclePhaseStr
62: 	default:
63: 		return unknownStr
64: 	}
65: }
66: 
67: // IsKnown determines if this output is known.
68: func (o Output) IsKnown() bool {
69: 	return o >= Value && o < outputLast
70: }
71: 
72: // MarshalJSON implements the Marshaler interface.
73: func (o Output) MarshalJSON() ([]byte, error) {
74: 	const (
75: 		errFmt = "cannot marshal '%s': unknown trend cycle mode output"
76: 		extra  = 2   // Two bytes for quotes.
77: 		dqc    = '"' // Double quote character.
78: 	)
79: 
80: 	s := o.String()
81: 	if s == unknownStr {
82: 		return nil, fmt.Errorf(errFmt, s)
83: 	}
84: 
85: 	b := make([]byte, 0, len(s)+extra)
86: 	b = append(b, dqc)
87: 	b = append(b, s...)
88: 	b = append(b, dqc)
89: 
90: 	return b, nil
91: }
92: 
93: // UnmarshalJSON implements the Unmarshaler interface.
94: func (o *Output) UnmarshalJSON(data []byte) error {
95: 	const (
96: 		errFmt = "cannot unmarshal '%s': unknown trend cycle mode output"
97: 		dqs    = "\"" // Double quote string.
98: 	)
99: 
100: 	d := bytes.Trim(data, dqs)
101: 	s := string(d)
102: 
103: 	switch s {
104: 	case valueStr:
105: 		*o = Value
106: 	case isTrendModeStr:
107: 		*o = IsTrendMode
108: 	case isCycleModeStr:
109: 		*o = IsCycleMode
110: 	case instantaneousTrendLineStr:
111: 		*o = InstantaneousTrendLine
112: 	case sineWaveStr:
113: 		*o = SineWave
114: 	case sineWaveLeadStr:
115: 		*o = SineWaveLead
116: 	case dominantCyclePeriodStr:
117: 		*o = DominantCyclePeriod
118: 	case dominantCyclePhaseStr:
119: 		*o = DominantCyclePhase
120: 	default:
121: 		return fmt.Errorf(errFmt, s)
122: 	}
123: 
124: 	return nil
125: }

(End of file - total 125 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.2s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode.go</path>
<type>file</type>
<content>1: package trendcyclemode
2: 
3: //nolint: gofumpt
4: import (
5: 	"fmt"
6: 	"math"
7: 	"sync"
8: 	"time"
9: 
10: 	"zpano/entities"
11: 	"zpano/indicators/core"
12: 	"zpano/indicators/core/outputs"
13: 	"zpano/indicators/johnehlers/dominantcycle"
14: 	"zpano/indicators/johnehlers/hilberttransformer"
15: )
16: 
17: // Deg2Rad converts degrees to radians.
18: const deg2Rad = math.Pi / 180.0
19: 
20: // TrendCycleMode is Ehlers' Trend-versus-Cycle Mode indicator.
21: //
22: // It wraps a DominantCycle (for instantaneous period / phase / WMA-smoothed price)
23: // and exposes eight outputs:
24: //
25: //   - Value: +1 in trend mode, -1 in cycle mode.
26: //   - IsTrendMode: 1 if trend mode is declared, 0 otherwise.
27: //   - IsCycleMode: 1 if cycle mode is declared, 0 otherwise.
28: //   - InstantaneousTrendLine: WMA-smoothed trend line.
29: //   - SineWave: sin(phase·Deg2Rad).
30: //   - SineWaveLead: sin((phase+45)·Deg2Rad).
31: //   - DominantCyclePeriod: smoothed dominant cycle period.
32: //   - DominantCyclePhase: dominant cycle phase, in degrees.
33: //
34: // Reference:
35: // John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 113-118.
36: type TrendCycleMode struct {
37: 	mu                         sync.RWMutex
38: 	mnemonic                   string
39: 	description                string
40: 	mnemonicTrend              string
41: 	descriptionTrend           string
42: 	mnemonicCycle              string
43: 	descriptionCycle           string
44: 	mnemonicITL                string
45: 	descriptionITL             string
46: 	mnemonicSine               string
47: 	descriptionSine            string
48: 	mnemonicSineLead           string
49: 	descriptionSineLead        string
50: 	mnemonicDCP                string
51: 	descriptionDCP             string
52: 	mnemonicDCPhase            string
53: 	descriptionDCPhase         string
54: 	dc                         *dominantcycle.DominantCycle
55: 	cyclePartMultiplier        float64
56: 	separationPercentage       float64
57: 	separationFactor           float64
58: 	trendLineSmoothingLength   int
59: 	coeff0                     float64
60: 	coeff1                     float64
61: 	coeff2                     float64
62: 	coeff3                     float64
63: 	trendline                  float64
64: 	trendAverage1              float64
65: 	trendAverage2              float64
66: 	trendAverage3              float64
67: 	sinWave                    float64
68: 	sinWaveLead                float64
69: 	prevDcPhase                float64
70: 	prevSineLeadWaveDifference float64
71: 	samplesInTrend             int
72: 	isTrendMode                bool
73: 	input                      []float64
74: 	inputLength                int
75: 	inputLengthMin1            int
76: 	primed                     bool
77: 	barFunc                    entities.BarFunc
78: 	quoteFunc                  entities.QuoteFunc
79: 	tradeFunc                  entities.TradeFunc
80: }
81: 
82: // NewTrendCycleModeDefault returns an instance of the indicator
83: // created using default values of the parameters.
84: func NewTrendCycleModeDefault() (*TrendCycleMode, error) {
85: 	const (
86: 		smoothingLength           = 4
87: 		alphaEmaQuadratureInPhase = 0.2
88: 		alphaEmaPeriod            = 0.2
89: 		alphaEmaPeriodAdditional  = 0.33
90: 		warmUpPeriod              = 100
91: 		trendLineSmoothingLength  = 4
92: 		cyclePartMultiplier       = 1.0
93: 		separationPercentage      = 1.5
94: 	)
95: 
96: 	return newTrendCycleMode(
97: 		hilberttransformer.HomodyneDiscriminator,
98: 		&hilberttransformer.CycleEstimatorParams{
99: 			SmoothingLength:           smoothingLength,
100: 			AlphaEmaQuadratureInPhase: alphaEmaQuadratureInPhase,
101: 			AlphaEmaPeriod:            alphaEmaPeriod,
102: 			WarmUpPeriod:              warmUpPeriod,
103: 		},
104: 		alphaEmaPeriodAdditional,
105: 		trendLineSmoothingLength,
106: 		cyclePartMultiplier,
107: 		separationPercentage,
108: 		0, 0, 0)
109: }
110: 
111: // NewTrendCycleModeParams returns an instance of the indicator created using supplied parameters.
112: func NewTrendCycleModeParams(p *Params) (*TrendCycleMode, error) {
113: 	return newTrendCycleMode(
114: 		p.EstimatorType, &p.EstimatorParams,
115: 		p.AlphaEmaPeriodAdditional,
116: 		p.TrendLineSmoothingLength,
117: 		p.CyclePartMultiplier,
118: 		p.SeparationPercentage,
119: 		p.BarComponent, p.QuoteComponent, p.TradeComponent)
120: }
121: 
122: //nolint:funlen,cyclop
123: func newTrendCycleMode(
124: 	estimatorType hilberttransformer.CycleEstimatorType,
125: 	estimatorParams *hilberttransformer.CycleEstimatorParams,
126: 	alphaEmaPeriodAdditional float64,
127: 	trendLineSmoothingLength int,
128: 	cyclePartMultiplier float64,
129: 	separationPercentage float64,
130: 	bc entities.BarComponent, qc entities.QuoteComponent, tc entities.TradeComponent,
131: ) (*TrendCycleMode, error) {
132: 	const (
133: 		invalid       = "invalid trend cycle mode parameters"
134: 		fmta          = "%s: α for additional smoothing should be in range (0, 1]"
135: 		fmttlsl       = "%s: trend line smoothing length should be 2, 3, or 4"
136: 		fmtcpm        = "%s: cycle part multiplier should be in range (0, 10]"
137: 		fmtsep        = "%s: separation percentage should be in range (0, 100]"
138: 		fmtw          = "%s: %w"
139: 		fmtnVal       = "tcm(%.3f, %d, %.3f, %.3f%%%s%s)"
140: 		fmtnTrend     = "tcm-trend(%.3f, %d, %.3f, %.3f%%%s%s)"
141: 		fmtnCycle     = "tcm-cycle(%.3f, %d, %.3f, %.3f%%%s%s)"
142: 		fmtnITL       = "tcm-itl(%.3f, %d, %.3f, %.3f%%%s%s)"
143: 		fmtnSine      = "tcm-sine(%.3f, %d, %.3f, %.3f%%%s%s)"
144: 		fmtnSineLead  = "tcm-sineLead(%.3f, %d, %.3f, %.3f%%%s%s)"
145: 		fmtnDCP       = "dcp(%.3f%s%s)"
146: 		fmtnDCPha     = "dcph(%.3f%s%s)"
147: 		four          = 4
148: 		alpha         = 0.2
149: 		descrVal      = "Trend versus cycle mode "
150: 		descrTrend    = "Trend versus cycle mode, is-trend flag "
151: 		descrCycle    = "Trend versus cycle mode, is-cycle flag "
152: 		descrITL      = "Trend versus cycle mode instantaneous trend line "
153: 		descrSine     = "Trend versus cycle mode sine wave "
154: 		descrSineLead = "Trend versus cycle mode sine wave lead "
155: 		descrDCP      = "Dominant cycle period "
156: 		descrDCPha    = "Dominant cycle phase "
157: 		tlslTwo       = 2
158: 		tlslThree     = 3
159: 		tlslFour      = 4
160: 		cpmMax        = 10.0
161: 		sepMax        = 100.0
162: 		sepDenom      = 100.0
163: 	)
164: 
165: 	if alphaEmaPeriodAdditional <= 0. || alphaEmaPeriodAdditional > 1. {
166: 		return nil, fmt.Errorf(fmta, invalid)
167: 	}
168: 
169: 	if trendLineSmoothingLength < tlslTwo || trendLineSmoothingLength > tlslFour {
170: 		return nil, fmt.Errorf(fmttlsl, invalid)
171: 	}
172: 
173: 	if cyclePartMultiplier <= 0. || cyclePartMultiplier > cpmMax {
174: 		return nil, fmt.Errorf(fmtcpm, invalid)
175: 	}
176: 
177: 	if separationPercentage <= 0. || separationPercentage > sepMax {
178: 		return nil, fmt.Errorf(fmtsep, invalid)
179: 	}
180: 
181: 	// SineWave-style default: BarMedianPrice always shown in mnemonic.
182: 	if bc == 0 {
183: 		bc = entities.BarMedianPrice
184: 	}
185: 
186: 	if qc == 0 {
187: 		qc = entities.DefaultQuoteComponent
188: 	}
189: 
190: 	if tc == 0 {
191: 		tc = entities.DefaultTradeComponent
192: 	}
193: 
194: 	// Build the inner DominantCycle with explicit components.
195: 	dcParams := &dominantcycle.Params{
196: 		EstimatorType:            estimatorType,
197: 		EstimatorParams:          *estimatorParams,
198: 		AlphaEmaPeriodAdditional: alphaEmaPeriodAdditional,
199: 		BarComponent:             bc,
200: 		QuoteComponent:           qc,
201: 		TradeComponent:           tc,
202: 	}
203: 
204: 	dc, err := dominantcycle.NewDominantCycleParams(dcParams)
205: 	if err != nil {
206: 		return nil, fmt.Errorf(fmtw, invalid, err)
207: 	}
208: 
209: 	// Compose the estimator moniker (same logic as DominantCycle / HTITL).
210: 	estimator, err := hilberttransformer.NewCycleEstimator(estimatorType, estimatorParams)
211: 	if err != nil {
212: 		return nil, fmt.Errorf(fmtw, invalid, err)
213: 	}
214: 
215: 	estimatorMoniker := ""
216: 	if estimatorType != hilberttransformer.HomodyneDiscriminator ||
217: 		estimatorParams.SmoothingLength != four ||
218: 		estimatorParams.AlphaEmaQuadratureInPhase != alpha ||
219: 		estimatorParams.AlphaEmaPeriod != alpha {
220: 		estimatorMoniker = hilberttransformer.EstimatorMoniker(estimatorType, estimator)
221: 		if len(estimatorMoniker) > 0 {
222: 			estimatorMoniker = ", " + estimatorMoniker
223: 		}
224: 	}
225: 
226: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
227: 
228: 	fmtArgs := []any{
229: 		alphaEmaPeriodAdditional, trendLineSmoothingLength,
230: 		cyclePartMultiplier, separationPercentage,
231: 		estimatorMoniker, componentMnemonic,
232: 	}
233: 
234: 	mnemonicVal := fmt.Sprintf(fmtnVal, fmtArgs...)
235: 	mnemonicTrend := fmt.Sprintf(fmtnTrend, fmtArgs...)
236: 	mnemonicCycle := fmt.Sprintf(fmtnCycle, fmtArgs...)
237: 	mnemonicITL := fmt.Sprintf(fmtnITL, fmtArgs...)
238: 	mnemonicSine := fmt.Sprintf(fmtnSine, fmtArgs...)
239: 	mnemonicSineLead := fmt.Sprintf(fmtnSineLead, fmtArgs...)
240: 	mnemonicDCP := fmt.Sprintf(fmtnDCP, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
241: 	mnemonicDCPha := fmt.Sprintf(fmtnDCPha, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
242: 
243: 	barFunc, err := entities.BarComponentFunc(bc)
244: 	if err != nil {
245: 		return nil, fmt.Errorf(fmtw, invalid, err)
246: 	}
247: 
248: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
249: 	if err != nil {
250: 		return nil, fmt.Errorf(fmtw, invalid, err)
251: 	}
252: 
253: 	tradeFunc, err := entities.TradeComponentFunc(tc)
254: 	if err != nil {
255: 		return nil, fmt.Errorf(fmtw, invalid, err)
256: 	}
257: 
258: 	var c0, c1, c2, c3 float64
259: 
260: 	switch trendLineSmoothingLength {
261: 	case tlslTwo:
262: 		c0, c1 = 2.0/3.0, 1.0/3.0
263: 	case tlslThree:
264: 		c0, c1, c2 = 3.0/6.0, 2.0/6.0, 1.0/6.0
265: 	default: // tlslFour
266: 		c0, c1, c2, c3 = 4.0/10.0, 3.0/10.0, 2.0/10.0, 1.0/10.0
267: 	}
268: 
269: 	maxPeriod := dc.MaxPeriod()
270: 	nan := math.NaN()
271: 
272: 	return &TrendCycleMode{
273: 		mnemonic:                 mnemonicVal,
274: 		description:              descrVal + mnemonicVal,
275: 		mnemonicTrend:            mnemonicTrend,
276: 		descriptionTrend:         descrTrend + mnemonicTrend,
277: 		mnemonicCycle:            mnemonicCycle,
278: 		descriptionCycle:         descrCycle + mnemonicCycle,
279: 		mnemonicITL:              mnemonicITL,
280: 		descriptionITL:           descrITL + mnemonicITL,
281: 		mnemonicSine:             mnemonicSine,
282: 		descriptionSine:          descrSine + mnemonicSine,
283: 		mnemonicSineLead:         mnemonicSineLead,
284: 		descriptionSineLead:      descrSineLead + mnemonicSineLead,
285: 		mnemonicDCP:              mnemonicDCP,
286: 		descriptionDCP:           descrDCP + mnemonicDCP,
287: 		mnemonicDCPhase:          mnemonicDCPha,
288: 		descriptionDCPhase:       descrDCPha + mnemonicDCPha,
289: 		dc:                       dc,
290: 		cyclePartMultiplier:      cyclePartMultiplier,
291: 		separationPercentage:     separationPercentage,
292: 		separationFactor:         separationPercentage / sepDenom,
293: 		trendLineSmoothingLength: trendLineSmoothingLength,
294: 		coeff0:                   c0,
295: 		coeff1:                   c1,
296: 		coeff2:                   c2,
297: 		coeff3:                   c3,
298: 		trendline:                nan,
299: 		sinWave:                  nan,
300: 		sinWaveLead:              nan,
301: 		isTrendMode:              true,
302: 		input:                    make([]float64, maxPeriod),
303: 		inputLength:              maxPeriod,
304: 		inputLengthMin1:          maxPeriod - 1,
305: 		barFunc:                  barFunc,
306: 		quoteFunc:                quoteFunc,
307: 		tradeFunc:                tradeFunc,
308: 	}, nil
309: }
310: 
311: // IsPrimed indicates whether an indicator is primed.
312: func (s *TrendCycleMode) IsPrimed() bool {
313: 	s.mu.RLock()
314: 	defer s.mu.RUnlock()
315: 
316: 	return s.primed
317: }
318: 
319: // Metadata describes an output data of the indicator.
320: func (s *TrendCycleMode) Metadata() core.Metadata {
321: 	return core.Metadata{
322: 		Type:        core.TrendCycleMode,
323: 		Mnemonic:    s.mnemonic,
324: 		Description: s.description,
325: 		Outputs: []outputs.Metadata{
326: 			{Kind: int(Value), Type: outputs.ScalarType, Mnemonic: s.mnemonic, Description: s.description},
327: 			{Kind: int(IsTrendMode), Type: outputs.ScalarType, Mnemonic: s.mnemonicTrend, Description: s.descriptionTrend},
328: 			{Kind: int(IsCycleMode), Type: outputs.ScalarType, Mnemonic: s.mnemonicCycle, Description: s.descriptionCycle},
329: 			{Kind: int(InstantaneousTrendLine), Type: outputs.ScalarType, Mnemonic: s.mnemonicITL, Description: s.descriptionITL},
330: 			{Kind: int(SineWave), Type: outputs.ScalarType, Mnemonic: s.mnemonicSine, Description: s.descriptionSine},
331: 			{Kind: int(SineWaveLead), Type: outputs.ScalarType, Mnemonic: s.mnemonicSineLead, Description: s.descriptionSineLead},
332: 			{Kind: int(DominantCyclePeriod), Type: outputs.ScalarType, Mnemonic: s.mnemonicDCP, Description: s.descriptionDCP},
333: 			{Kind: int(DominantCyclePhase), Type: outputs.ScalarType, Mnemonic: s.mnemonicDCPhase, Description: s.descriptionDCPhase},
334: 		},
335: 	}
336: }
337: 
338: // Update updates the value of the indicator given the next sample, returning the
339: // (value, isTrendMode, isCycleMode, trendline, sineWave, sineWaveLead, period, phase) tuple.
340: // The isTrendMode / isCycleMode values are encoded as 1 / 0 scalars. Returns NaN for all
341: // outputs if the indicator is not yet primed.
342: //
343: //nolint:funlen,cyclop,gocognit
344: func (s *TrendCycleMode) Update(sample float64) (float64, float64, float64, float64, float64, float64, float64, float64) {
345: 	if math.IsNaN(sample) {
346: 		return sample, sample, sample, sample, sample, sample, sample, sample
347: 	}
348: 
349: 	// Delegate to the inner DominantCycle. Its own mutex is independent.
350: 	_, period, phase := s.dc.Update(sample)
351: 	// SmoothedPrice tracks the WMA smoothed price inside the Hilbert transformer.
352: 	smoothedPrice := s.dc.SmoothedPrice()
353: 
354: 	s.mu.Lock()
355: 	defer s.mu.Unlock()
356: 
357: 	s.pushInput(sample)
358: 
359: 	if s.primed {
360: 		smoothedPeriod := period
361: 		average := s.calculateTrendAverage(smoothedPeriod)
362: 		s.trendline = s.coeff0*average + s.coeff1*s.trendAverage1 +
363: 			s.coeff2*s.trendAverage2 + s.coeff3*s.trendAverage3
364: 		s.trendAverage3 = s.trendAverage2
365: 		s.trendAverage2 = s.trendAverage1
366: 		s.trendAverage1 = average
367: 
368: 		diff := s.calculateSineLeadWaveDifference(phase)
369: 
370: 		// Condition 1: a cycle mode exists for the half-period of a dominant cycle
371: 		// after the SineWave vs SineWaveLead crossing.
372: 		s.isTrendMode = true
373: 
374: 		if (diff > 0 && s.prevSineLeadWaveDifference < 0) ||
375: 			(diff < 0 && s.prevSineLeadWaveDifference > 0) {
376: 			s.isTrendMode = false
377: 			s.samplesInTrend = 0
378: 		}
379: 
380: 		s.prevSineLeadWaveDifference = diff
381: 		s.samplesInTrend++
382: 
383: 		const half = 0.5
384: 		if float64(s.samplesInTrend) < half*smoothedPeriod {
385: 			s.isTrendMode = false
386: 		}
387: 
388: 		// Condition 2: cycle mode if the measured phase rate of change is more than 2/3
389: 		// the phase rate of change of the dominant cycle (360/period) and less than 1.5 times it.
390: 		phaseDelta := phase - s.prevDcPhase
391: 		s.prevDcPhase = phase
392: 
393: 		if math.Abs(smoothedPeriod) > epsilon {
394: 			const (
395: 				minFactor = 2.0 / 3.0
396: 				maxFactor = 1.5
397: 				fullCycle = 360.0
398: 			)
399: 
400: 			dcRate := fullCycle / smoothedPeriod
401: 			if phaseDelta > minFactor*dcRate && phaseDelta < maxFactor*dcRate {
402: 				s.isTrendMode = false
403: 			}
404: 		}
405: 
406: 		// Condition 3: if the WMA smoothed price is separated by more than the separation
407: 		// percentage from the instantaneous trend line, force the trend mode.
408: 		if math.Abs(s.trendline) > epsilon &&
409: 			math.Abs((smoothedPrice-s.trendline)/s.trendline) >= s.separationFactor {
410: 			s.isTrendMode = true
411: 		}
412: 
413: 		return s.mode(), s.isTrendFloat(), s.isCycleFloat(), s.trendline, s.sinWave, s.sinWaveLead, period, phase
414: 	}
415: 
416: 	if s.dc.IsPrimed() {
417: 		s.primed = true
418: 		smoothedPeriod := period
419: 		s.trendline = s.calculateTrendAverage(smoothedPeriod)
420: 		s.trendAverage1 = s.trendline
421: 		s.trendAverage2 = s.trendline
422: 		s.trendAverage3 = s.trendline
423: 
424: 		s.prevDcPhase = phase
425: 		s.prevSineLeadWaveDifference = s.calculateSineLeadWaveDifference(phase)
426: 
427: 		s.isTrendMode = true
428: 		s.samplesInTrend++
429: 
430: 		const half = 0.5
431: 		if float64(s.samplesInTrend) < half*smoothedPeriod {
432: 			s.isTrendMode = false
433: 		}
434: 
435: 		return s.mode(), s.isTrendFloat(), s.isCycleFloat(), s.trendline, s.sinWave, s.sinWaveLead, period, phase
436: 	}
437: 
438: 	nan := math.NaN()
439: 
440: 	return nan, nan, nan, nan, nan, nan, nan, nan
441: }
442: 
443: // UpdateScalar updates the indicator given the next scalar sample.
444: func (s *TrendCycleMode) UpdateScalar(sample *entities.Scalar) core.Output {
445: 	return s.updateEntity(sample.Time, sample.Value)
446: }
447: 
448: // UpdateBar updates the indicator given the next bar sample.
449: func (s *TrendCycleMode) UpdateBar(sample *entities.Bar) core.Output {
450: 	return s.updateEntity(sample.Time, s.barFunc(sample))
451: }
452: 
453: // UpdateQuote updates the indicator given the next quote sample.
454: func (s *TrendCycleMode) UpdateQuote(sample *entities.Quote) core.Output {
455: 	return s.updateEntity(sample.Time, s.quoteFunc(sample))
456: }
457: 
458: // UpdateTrade updates the indicator given the next trade sample.
459: func (s *TrendCycleMode) UpdateTrade(sample *entities.Trade) core.Output {
460: 	return s.updateEntity(sample.Time, s.tradeFunc(sample))
461: }
462: 
463: func (s *TrendCycleMode) updateEntity(t time.Time, sample float64) core.Output {
464: 	const length = 8
465: 
466: 	value, trend, cycle, itl, sine, sineLead, period, phase := s.Update(sample)
467: 
468: 	output := make([]any, length)
469: 	i := 0
470: 	output[i] = entities.Scalar{Time: t, Value: value}
471: 	i++
472: 	output[i] = entities.Scalar{Time: t, Value: trend}
473: 	i++
474: 	output[i] = entities.Scalar{Time: t, Value: cycle}
475: 	i++
476: 	output[i] = entities.Scalar{Time: t, Value: itl}
477: 	i++
478: 	output[i] = entities.Scalar{Time: t, Value: sine}
479: 	i++
480: 	output[i] = entities.Scalar{Time: t, Value: sineLead}
481: 	i++
482: 	output[i] = entities.Scalar{Time: t, Value: period}
483: 	i++
484: 	output[i] = entities.Scalar{Time: t, Value: phase}
485: 
486: 	return output
487: }
488: 
489: // epsilon is the floating-point equality threshold (matches C#'s double.Epsilon behaviour for zero checks).
490: const epsilon = 1e-308
491: 
492: func (s *TrendCycleMode) pushInput(value float64) {
493: 	copy(s.input[1:], s.input[:s.inputLengthMin1])
494: 	s.input[0] = value
495: }
496: 
497: func (s *TrendCycleMode) calculateTrendAverage(smoothedPeriod float64) float64 {
498: 	const half = 0.5
499: 
500: 	length := int(math.Floor(smoothedPeriod*s.cyclePartMultiplier + half))
501: 	if length > s.inputLength {
502: 		length = s.inputLength
503: 	} else if length < 1 {
504: 		length = 1
505: 	}
506: 
507: 	var sum float64
508: 	for i := 0; i < length; i++ {
509: 		sum += s.input[i]
510: 	}
511: 
512: 	return sum / float64(length)
513: }
514: 
515: func (s *TrendCycleMode) calculateSineLeadWaveDifference(phase float64) float64 {
516: 	const leadOffset = 45.0
517: 
518: 	p := phase * deg2Rad
519: 	s.sinWave = math.Sin(p)
520: 	s.sinWaveLead = math.Sin(p + leadOffset*deg2Rad)
521: 
522: 	return s.sinWave - s.sinWaveLead
523: }
524: 
525: func (s *TrendCycleMode) mode() float64 {
526: 	if s.isTrendMode {
527: 		return 1.0
528: 	}
529: 
530: 	return -1.0
531: }
532: 
533: func (s *TrendCycleMode) isTrendFloat() float64 {
534: 	if s.isTrendMode {
535: 		return 1.0
536: 	}
537: 
538: 	return 0.0
539: }
540: 
541: func (s *TrendCycleMode) isCycleFloat() float64 {
542: 	if s.isTrendMode {
543: 		return 0.0
544: 	}
545: 
546: 	return 1.0
547: }

(End of file - total 547 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.8s)

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
33: 	ParameterResolution float64 `json:"res"`
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

---

## Assistant (Build · claude-opus-4.7 · 3.6s)

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

## Assistant (Build · claude-opus-4.7 · 3.3s)

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

## Assistant (Build · claude-opus-4.7 · 5.7s)

_Thinking:_

 I need to examine the MBST CoronaSpectrum output structure, then review the trendcyclemode parameters file and its corresponding test file to understand how they're organized.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/mbst-to-convert/john-ehlers/corona/CoronaSpectrum.cs"
}
```

**Output:**
```
<path>/home/dev/zpano/mbst-to-convert/john-ehlers/corona/CoronaSpectrum.cs</path>
<type>file</type>
<content>1: ﻿using System;
2: using Mbs.Trading.Data;
3: using Mbs.Trading.Indicators.Abstractions;
4: 
5: // ReSharper disable once CheckNamespace
6: namespace Mbs.Trading.Indicators.JohnEhlers
7: {
8:     /// <summary>
9:     /// The Corona Spectrum measures cyclic activity over a cycle period range (default from 6 bars to 30 bars)
10:     /// in a bank of contiguous filters.
11:     /// <para />
12:     /// Longer cycle periods are not considered because they can be considered trend segments. Shorter cycle
13:     /// periods are not considered because of the aliasing noise encountered with the data sampling process.
14:     /// <para />
15:     /// The amplitude of each filter output is compared to the strongest signal and the result is displayed
16:     /// over an amplitude range of 20 decibels in the form of a heat-map.
17:     /// <para />
18:     /// The filter having the strongest output is selected as the current dominant cycle period.
19:     /// <para />
20:     /// See “Measuring Cycle Periods” in the March 2008 issue of Stocks &amp; Commodities Magazine.
21:     /// </summary>
22:     public sealed class CoronaSpectrum : Indicator, IHeatMapIndicator
23:     {
24:         #region Members and accessors
25:         /// <summary>
26:         /// The minimum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns.
27:         /// </summary>
28:         public double MinParameterValue { get; }
29: 
30:         /// <summary>
31:         /// The maximum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns.
32:         /// </summary>
33:         public double MaxParameterValue { get; }
34: 
35:         /// <summary>
36:         /// The middle raster value of the heat-map.
37:         /// </summary>
38:         private readonly double midRasterValue;
39: 
40:         /// <summary>
41:         /// The current value of the Corona dominant cycle or <c>NaN</c> if not primed.
42:         /// </summary>
43:         public double DominantCycle { get { lock (Lock) { return Primed ? Corona.DominantCycle : double.NaN; } } }
44: 
45:         /// <summary>
46:         /// The current value of the Corona dominant cycle median or <c>NaN</c> if not primed.
47:         /// </summary>
48:         public double DominantCycleMedian { get { lock (Lock) { return Primed ? Corona.DominantCycleMedian : double.NaN; } } }
49: 
50:         /// <summary>
51:         /// A line indicator façade to expose a value of the DominantCycle as a separate line indicator with a fictitious update.
52:         /// <para />
53:         /// The line indicator façade do not perform actual updates. It just gets a current value of the DominantCycle from this instance, assuming it is already updated.
54:         /// <para />
55:         /// Resetting the line indicator façade has no effect. This instance should be reset instead.
56:         /// </summary>
57:         public LineIndicatorFacade DominantCycleFacade => new LineIndicatorFacade(CdcName, CdcMoniker, CdcDescription, () => IsPrimed, () => DominantCycle);
58: 
59:         /// <summary>
60:         /// A line indicator façade to expose a value of the DominantCycleMedian as a separate line indicator with a fictitious update.
61:         /// <para />
62:         /// The line indicator façade do not perform actual updates. It just gets a current value of the DominantCycleMedian from this instance, assuming it is already updated.
63:         /// <para />
64:         /// Resetting the line indicator façade has no effect. This instance should be reset instead.
65:         /// </summary>
66:         public LineIndicatorFacade DominantCycleMedianFacade => new LineIndicatorFacade(CdcmName, CdcmMoniker, CdcmDescription, () => IsPrimed, () => DominantCycleMedian);
67: 
68:         /// <summary>
69:         /// The associated corona engine.
70:         /// </summary>
71:         internal Corona Corona { get; }
72: 
73:         private const string CsName = "CoronaSpectrum";
74:         private const string CsDescription = "Crona spectrum heatmap";
75:         private const string CsMoniker = "CoronaS";
76: 
77:         private const string CdcName = "CoronaDominantCycle";
78:         private const string CdcDescription = "Crona dominant cycle";
79:         private const string CdcMoniker = "CoronaDC";
80: 
81:         private const string CdcmName = "CoronaDominantCycleMedian";
82:         private const string CdcmDescription = "Crona dominant cycle median";
83:         private const string CdcmMoniker = "CoronaDCM";
84: 
85:         private readonly double parameterRange;
86:         #endregion
87: 
88:         #region Construction
89:         /// <summary>
90:         /// Constructs a new instance of the class.
91:         /// </summary>
92:         /// <param name="minRasterValue">The minimal raster value of the heat-map. The default value is 6.</param>
93:         /// <param name="maxRasterValue">The maximal raster value of the heat-map. The default value is 20.</param>
94:         /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heat-map, representing the minimal period. This value is the same for all heat-map columns. The default value is 6.</param>
95:         /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heat-map, representing the maximal period. This value is the same for all heat-map columns. The default value is 30.</param>
96:         /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
97:         /// <param name="highPassFilterCutoff">The high-pass filter cutoff (de-trending period). Suggested values are 20, 30, 100.</param>
98:         public CoronaSpectrum(double minRasterValue = 6, double maxRasterValue = 20, double minParameterValue = 6, double maxParameterValue = 30,
99:             OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice, int highPassFilterCutoff = 30)
100:             : base(CsName, CsDescription, ohlcvComponent)
101:         {
102:             Moniker = CsMoniker;
103:             minParameterValue = Math.Ceiling(minParameterValue);
104:             maxParameterValue = Math.Floor(maxParameterValue);
105:             parameterRange = (maxParameterValue - minParameterValue);
106:             Corona = new Corona(highPassFilterCutoff, (int)minParameterValue, (int)maxParameterValue, minRasterValue, maxRasterValue);
107: 
108:             this.MinParameterValue = minParameterValue;
109:             this.MaxParameterValue = maxParameterValue;
110: 
111:             midRasterValue = 10;// maxRasterValue / 2;
112:         }
113:         #endregion
114: 
115:         #region Reset
116:         /// <inheritdoc />
117:         public override void Reset()
118:         {
119:             lock (Lock)
120:             {
121:                 Primed = false;
122:                 Corona.Reset();
123:             }
124:         }
125:         #endregion
126: 
127:         #region Update
128:         private HeatMap Update(double sample, DateTime dateTime)
129:         {
130:             if (double.IsNaN(sample))
131:                 return null;
132:             Primed = Corona.Update(sample);
133:             double[] intensity;
134:             if (Primed)
135:             {
136:                 intensity = new double[Corona.FilterBankLength];//+1
137:                 int min = Corona.MinimalPeriodTimesTwo;
138:                 int max = Corona.MaximalPeriodTimesTwo;
139:                 for (int i = min; i <= max; ++i)
140:                 {
141:                     double value = Corona.FilterBank[i - min].Decibels;
142:                     intensity[i - min] = value;
143:                 }
144:             }
145:             else
146:             {
147:                 intensity = null;
148:             }
149:             return new HeatMap(dateTime, intensity);
150:         }
151: 
152:         /// <summary>
153:         /// Updates the value of the indicator.
154:         /// </summary>
155:         /// <param name="ohlcv">A new ohlcv.</param>
156:         /// <returns>The new heat-map column of the indicator or <c>null</c>.</returns>
157:         public HeatMap Update(Ohlcv ohlcv)
158:         {
159:             lock (Lock)
160:             {
161:                 return Update(ohlcv.Component(OhlcvComponent), ohlcv.Time);
162:             }
163:         }
164: 
165:         /// <summary>
166:         /// Updates the value of the indicator.
167:         /// </summary>
168:         /// <param name="scalar">A new scalar.</param>
169:         /// <returns>The new heat-map column of the indicator or <c>null</c>.</returns>
170:         public HeatMap Update(Scalar scalar)
171:         {
172:             lock (Lock)
173:             {
174:                 return Update(scalar.Value, scalar.Time);
175:             }
176:         }
177:         #endregion
178:     }
179: }

(End of file - total 179 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 7.7s)

_Thinking:_

 I'm working through how the y-axis resolution should be calculated. Looking at zpano's approach, the ParameterResolution is derived from the filter bank length and the parameter range—with the defaults giving a resolution of 2 samples per unit.

Now I need to check the trendcyclemode tests and the EstimatorParams to understand the test patterns better.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go</path>
<type>file</type>
<content>1: //nolint:testpackage
2: package trendcyclemode
3: 
4: //nolint: gofumpt
5: import (
6: 	"math"
7: 	"testing"
8: 	"time"
9: 
10: 	"zpano/entities"
11: 	"zpano/indicators/core"
12: 	"zpano/indicators/core/outputs"
13: 	"zpano/indicators/johnehlers/hilberttransformer"
14: )
15: 
16: func testTCMTime() time.Time {
17: 	return time.Date(2021, time.April, 1, 0, 0, 0, 0, &time.Location{})
18: }
19: 
20: // testTCMInput is the rawInput from MBST TrendCycleModeTest.cs (TA-Lib test_MAMA.xsl, Price).
21: //
22: //nolint:dupl
23: func testTCMInput() []float64 {
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
54: // testTCMExpectedPeriod is the dcPeriod from MBST TrendCycleModeTest.cs.
55: //
56: //nolint:dupl
57: func testTCMExpectedPeriod() []float64 {
58: 	return []float64{
59: 		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
60: 		0, 0, 0.39600000000000, 0.97812000000000, 1.62158040000000, 2.25545086800000, 2.84234568156000, 3.36868456664520, 3.86776291565229, 4.36321983508703,
61: 		4.87235783926831, 5.40838035704577, 5.98190550443027, 6.60199641969884, 7.27686930610184, 8.01438731048222, 8.82241286095647, 9.70906731606755, 10.68293087091460, 11.75320502957710,
62: 		12.92985285048740, 14.22372743856440, 15.55272286275450, 16.77503611571540, 17.85814025111630, 18.72970387649220, 19.30387978646140, 19.63314544969620, 19.86256979430250, 20.09030968609300,
63: 		20.24817834009410, 20.31132870798190, 20.52152604110820, 21.27119054536480, 22.10966835167300, 22.28715460952700, 21.91280773257140, 21.23923470724180, 20.70813161651310, 20.20449150221090,
64: 		19.52863321263000, 18.73709250583170, 17.96311275281150, 17.33367762545960, 16.91743352044750, 16.64300564862120, 16.41952162419500, 16.27464914327850, 16.26425245094380, 16.33321577028600,
65: 		16.39265551523990, 16.39976990202790, 16.38221107536320, 16.37405059271910, 16.35102942468120, 16.26839438425590, 16.12432207371240, 15.99529098667630, 15.96458956780100, 16.07977539207760,
66: 		16.38360881255670, 16.79746341307210, 17.18753188776420, 17.58524022168910, 18.05888760471740, 18.46077773999830, 18.78691120238400, 19.07789869381110, 19.11803073417110, 18.72675385299730,
67: 		18.07403190737810, 17.72999456892580, 18.00699920187680, 18.06680349806270, 17.73551482016550, 17.28467833183610, 16.97456900115070, 16.89386283663200, 17.00556420464730, 17.21986959021550,
68: 		17.48251598471900, 17.79647268844360, 18.15809655229470, 18.56044162987590, 19.03705300462320, 19.61779465906120, 20.13838368155990, 20.58144279802850, 20.93554178712450, 21.09578565733870,
69: 		21.19426268582890, 21.35270550953270, 21.46806615214910, 21.43420235778580, 21.27320458618770, 20.98617884905010, 20.62345107800030, 20.32165030848570, 20.09921951571820, 19.88214300840560,
70: 		19.67081622699810, 19.55217428481160, 19.60485773311710, 19.77836095343260, 19.77886122563300, 19.59009982815140, 19.54609435364200, 19.77945658439880, 20.20526697824140, 20.80572859375930,
71: 		21.45882440191380, 21.50916115262280, 21.07219135457730, 20.33979206665380, 19.60807769029340, 19.15831017112920, 19.03006205140340, 19.23359250887840, 19.84206353515510, 20.83692898803630,
72: 		22.25776348341490, 23.50933063567320, 24.02857349775940, 24.28548086650010, 24.74576845262060, 25.45685387492870, 26.31998583396390, 27.14553013410700, 27.80677101851790, 28.50146147525040,
73: 		29.16835938704370, 29.38723525724370, 29.55886298198770, 30.43981360336700, 30.70779370313880, 30.20667454311960, 29.23518282361370, 28.00037502954910, 26.95505291681000, 26.22399862702800,
74: 		25.67716809996900, 25.19893752937060, 24.76924271120940, 24.40654607774420, 24.12997738279210, 24.02648590415090, 24.17912316847620, 24.26552607123530, 24.07565548132200, 23.81050493977940,
75: 		23.75771490624360, 24.01627030476950, 24.42884190933990, 24.55867905189440, 24.41978729840000, 24.33536819272640, 24.11887925396970, 23.53741527509780, 22.66734716257270, 21.70419061052260,
76: 		20.78848949032480, 19.92593130809770, 19.09528115584620, 18.35405205698280, 17.81539769318840, 17.53491540732180, 17.59552736216070, 18.09376127214910, 18.69300796204700, 19.10361709066390,
77: 		19.40368660687600, 19.79324964337850, 20.14261316711870, 20.30292592814370, 20.37642955508450, 20.42856373321320, 20.36417897031590, 20.26870923265670, 20.31691792510900, 20.52593924664440,
78: 		20.95797078839970, 21.70315565998060, 22.68588957914270, 23.95566588814480, 25.30036991408680, 26.49222048853470, 28.24485763802440, 30.46863151925310, 31.19661794415910, 30.97271495031300,
79: 		30.15801520320610, 29.52193986806340, 28.48090879451130, 27.20913817575940, 25.84740758865390, 24.75875079095690, 24.57820671512040, 25.25622655282780, 26.58938264946150, 28.44936832011270,
80: 		30.75900691394640, 31.63120735338530, 31.95156902113430, 32.19329221743080, 32.18129930292270, 31.78927079951340, 30.94427836437330, 29.74153261553520, 28.44319750131350, 27.27756983469050,
81: 		26.30928991862760, 25.59706087830910, 25.19354035279110, 24.98183319418390, 24.66611779383150, 24.13629363553260, 23.59372342374540, 23.45943359521940, 24.13462330023790, 25.42868068174450,
82: 		27.22154743441240, 28.85990121754770, 29.25658159944000, 28.86760790158470, 28.27077502042400, 27.83957963686970, 27.56292753489200, 27.31665028261770, 27.11537844471070, 27.05619511102920,
83: 		26.72669604084850, 25.93839467294110, 24.88015320695530, 23.98089561843900, 23.51115215671300, 23.02173482203020, 22.29674643126940, 21.42162141795630, 20.54863761751100, 19.78167187971360,
84: 		19.14387361712880, 18.61396641752300,
85: 	}
86: }
87: 
88: // testTCMExpectedPhase is the dcPhase from MBST TrendCycleModeTest.cs.
89: //
90: //nolint:dupl
91: func testTCMExpectedPhase() []float64 {
92: 	return []float64{
93: 		math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(),
94: 		math.NaN(), math.NaN(), math.NaN(),
95: 		639.09090909090900, 98.05299963194700, 132.00564338345400, 69.61331949528320, 72.53929158292410, 49.31988383698000, 70.57064052204580, 95.44397344939550, 15.53098102018940, -7.59977335936253,
96: 		-0.43908926482948, 0.75159304711349, 6.81682854281200, 10.38041464097730, 14.60138862232650, 23.61952231391600, 34.25173964222910, 45.61939612889610, 54.93203281582900, 62.34730431858280,
97: 		59.09332873285360, 64.74787861710400, 71.38960942346380, 78.26591034671290, 95.01873223899610, 100.81260420916600, 122.15965196914300, 155.92351856084900, 203.60327223472200, 237.36293513970900,
98: 		244.38539212971100, 260.58654095568100, 254.66267427143100, 253.71116813655900, 126.51252837026600, 108.91084071926300, 120.62704116849900, 135.00432720483700, 148.37778407551400, 166.73764548652900,
99: 		192.70016208445300, 227.19271648131900, 250.93069421530800, 272.91238631233800, 306.14557952390500, -30.56440912946640, -7.29779366562593, 15.79435681229920, 37.27143784018100, 57.28482204365770,
100: 		77.89434659794900, 100.26640790428300, 122.46792347519500, 143.34860922315100, 161.97099218663000, 179.04173949124300, 194.28337763382200, 207.12978658551400, 219.12056620855700, 228.99420937560000,
101: 		246.39888033834300, 259.83330071373300, 279.43099672242300, 300.38982045382800, -42.95237520761510, 10.71059944380160, 67.91671263897760, 95.69661875079390, 116.17343846855600, 131.69663648791300,
102: 		145.22435488266800, 157.96979483912800, 170.90824231177600, 190.12565895164300, 201.99948636973700, 211.84934448916700, 219.50941069822200, 223.55536694633000, 217.70267385839600, 208.61154857738600,
103: 		187.20930268786500, 183.53325881754400, 173.77461459542900, 177.09324308654900, 177.36771678646600, 183.46254384786600, 190.38499136923400, 200.15247884572800, 210.26397611554500, 219.92876325408700,
104: 		230.19440003342700, 241.78128922383700, 255.09398089013500, 269.90233026686000, 287.31443195349700, -22.67219169276530, 21.31042453594620, 55.99561073880190, 76.28592721136950, 93.25596317796150,
105: 		111.58409337397600, 131.21106018529500, 148.25324978238800, 161.34668836868300, 167.25425018604100, 166.56119294454900, 164.79606018404900, 165.59339091940400, 167.71720854219100, 171.16337795073400,
106: 		177.90397416349400, 180.76168462321900, 181.18022018527700, 182.90279528585400, 185.03420448405800, 187.45543006582800, 185.33720976352800, 183.30316508274000, 181.31467258646500, 177.91862390253700,
107: 		179.98928550766400, 182.00429308763000, 183.62347330064100, 185.57299458485300, 189.35191538410900, 194.23220405450000, 200.11245439803000, 206.61598766532100, 214.60761785986900, 223.17826798412900,
108: 		229.59983294953500, 238.43424165042300, 244.29554481830400, 259.55186325451500, 277.88404982202100, 297.46674550039700, -42.41381860502280, -22.05561416691630, -10.76874195668260, 4.55423903808060,
109: 		13.05706378617280, 25.49753552893060, 31.99805128632640, 38.60334291217220, 45.70890152749460, 53.48688015996070, 60.82873486408750, 67.21350248296350, 74.98104788942620, 84.48479832948730,
110: 		97.41668808537870, 101.86825371917200, 131.27099687518600, 148.43969963128300, 169.23915874881900, 186.79876808562800, 222.27683786450500, 226.80174114624100, 148.12023947867300, 119.78684089821000,
111: 		123.91411010078200, 136.35086145099600, 148.39281918279000, 160.94079449625800, 171.89311802023800, 182.21130213571000, 189.94824024493000, 202.58335256387900, 215.72849327557900, 224.87477584909900,
112: 		239.93794152235800, 257.07736337068300, 274.90021437724400, 292.81710751479900, 310.18755119542100, -34.09748283129430, -20.55209191423000, -17.48926436788570, -6.64084597196063, -3.41661969757843,
113: 		-1.11706604110969, -0.61559202318233, -0.58343503934742, -0.97020892968976, -7.33789387098142, -13.71942088996000, -14.16303590443250, -9.85074940762922, -2.93111556519989, 0.35846302482548,
114: 		-0.27797938543370, -4.11254536082527, -7.19786584394603, -8.12930435521150, -7.23274284499956, -5.60008181003320, -3.98246383052538, -1.93459828828531, -0.91376116945821, 1.11347590999549,
115: 		3.48574296192987, 5.87739974191743, 8.51611669495514, 11.77045158406290, 16.98321519660290, 23.12127015453780, 32.37560208179040, 38.09489298723020, 44.37798569415560, 48.60625731428030,
116: 		57.00174598372450, 65.55585833338270, 77.53688240972470, 96.95830008657520, 111.55989345666400, 124.32277400743800, 131.41492641407500, 137.73657404096000, 141.46543104438800, 142.80498887855200,
117: 		149.73966957805500, 153.53899811794900, 157.79183782289500, 159.94501203849600, 162.96541156987000, 170.96418133015300, 176.71998519472000, 182.65581927371500, 189.05283471879000, 197.04766728263800,
118: 		206.92813680000000, 217.32451174036000, 224.38800791349600, 232.20526011220900, 246.06238925337200, 271.09605242709400, 289.11924018406000, -31.18231981512890, 23.26913353342980, 47.27652706672060,
119: 	}
120: }
121: 
122: // testTCMExpectedSine is the sine array from MBST TrendCycleModeTest.cs.
123: //
124: //nolint:dupl
125: func testTCMExpectedSine() []float64 {
126: 	return []float64{
127: 		math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(),
128: 		math.NaN(), math.NaN(), math.NaN(),
129: 		-0.9874388886763950, 0.9901389074597280, 0.7430789154373040, 0.9373629963767220, 0.9539229407379340, 0.7583605937417590, 0.9430523282622310, 0.9954894450161860, 0.2677593920226290, -0.1322524693787770,
130: 		-0.0076634783681766, 0.0131173971015221, 0.1186956105503800, 0.1801829214581150, 0.2520928115991770, 0.4006612403521310, 0.5628300247542610, 0.7147094931388430, 0.8184710623910560, 0.8857771050315170,
131: 		0.8580051054352940, 0.9044393510772560, 0.9477105512269880, 0.9791019835938800, 0.9961661502181070, 0.9822460058918840, 0.8465682109433450, 0.4079557291761370, -0.4004013665430420, -0.8421036878603060,
132: 		-0.9017223344864390, -0.9865337683617420, -0.9643853118844240, -0.9598599815079740, 0.8037267766705520, 0.9460240546952540, 0.8605016852751490, 0.7070533756573690, 0.5243161160505560, 0.2294102723810260,
133: 		-0.2198489640488240, -0.7336434870569610, -0.9451240721288890, -0.9987083965673280, -0.8075209150605320, -0.5085066436721070, -0.1270264128351010, 0.2721854747475370, 0.6055917784886910, 0.8413676398899490,
134: 		0.9777625488346960, 0.9839896992562860, 0.8436921159626520, 0.5969447127483850, 0.3094984574050860, 0.0167240212732023, -0.2467178763145330, -0.4560076441356610, -0.6309543271436050, -0.7546432714599400,
135: 		-0.9163549058535820, -0.9842983646399860, -0.9864836587318940, -0.8626035608908470, -0.6813902164376480, 0.1858483908516400, 0.9266383325265240, 0.9950614294774160, 0.8974629492620320, 0.7466772329212920,
136: 		0.5703644678660200, 0.3750953336668390, 0.1580160207328650, -0.1758076016904010, -0.3745982816248290, -0.5276875478007350, -0.6362049492364280, -0.6890552092033720, -0.6115639640491650, -0.4788688150780810,
137: 		-0.1254943141670760, -0.0616279223840421, 0.1084398125157010, 0.0507107190849194, 0.0459258492372133, -0.0603960127005548, -0.1802614924906120, -0.3445196936393600, -0.5039846767879240, -0.6418346779779860,
138: 		-0.7682209569752370, -0.8811490866996920, -0.9663490613441720, -0.9999985470701830, -0.9546858649139780, -0.3854582462377580, 0.3634207386677970, 0.8289947319983170, 0.9714909193370010, 0.9983857647297360,
139: 		0.9298786498974830, 0.7522877436715410, 0.5261656913352240, 0.3198410350076260, 0.2206250837743810, 0.2324067219830480, 0.2622555352188150, 0.2488016117878720, 0.2127369252136960, 0.1536174562004720,
140: 		0.0365743929645966, -0.0132935129731492, -0.0205972714630062, -0.0506416644795282, -0.0877504363551195, -0.1297549156821220, -0.0930172242876989, -0.0576191763867261, -0.0229433518557876, 0.0363188766621527,
141: 		0.0001870031678450, -0.0349743794742610, -0.0631993922084072, -0.0971138050129020, -0.1624979390656570, -0.2458522401310880, -0.3438638176192820, -0.4480085728769030, -0.5679531815591490, -0.6842705623709480,
142: 		-0.7615364178876680, -0.8520399314791350, -0.9010432983130380, -0.9834194615045590, -0.9905476870988080, -0.8872786828920670, -0.6744804687446460, -0.3755063881128740, -0.1868453942460660, 0.0794027920237213,
143: 		0.2259213674108930, 0.4304722733878400, 0.5298904205645140, 0.6239251933219030, 0.7158012315374360, 0.8037206344012380, 0.8731666378756390, 0.9219544489912210, 0.9658401621307640, 0.9953707336137810,
144: 		0.9916336070524670, 0.9786230879164460, 0.7515981300962940, 0.5233956277754490, 0.1867099275394900, -0.1183826185655860, -0.6727134585677300, -0.7289894296051810, 0.5281384062949680, 0.8678795703185170,
145: 		0.8298749053132090, 0.6902403611717530, 0.5240926478571590, 0.3265450140990430, 0.1410201458363470, -0.0385849224232995, -0.1727584531294190, -0.3840270653389320, -0.5839449895165980, -0.7055596595779030,
146: 		-0.8654833335157070, -0.9746729156329320, -0.9963449765889340, -0.9217474051403660, -0.7639362508931000, -0.5606026149454520, -0.3510588357296890, -0.3005270942524180, -0.1156452933894830, -0.0595959289499457,
147: 		-0.0194952452596040, -0.0107439009455394, -0.0101826864310280, -0.0169325310282176, -0.1277205927832380, -0.2371674472604240, -0.2446819018845580, -0.1710822516700410, -0.0511353061733825, 0.0062563192154928,
148: 		-0.0048516364950122, -0.0717158401086350, -0.1252962791394740, -0.1414075684951130, -0.1259001773043880, -0.0975843207989124, -0.0694511521424164, -0.0337586943638060, -0.0159474649423128, 0.0194325975213332,
149: 		0.0608001695138442, 0.1024001693533800, 0.1480876049487220, 0.2039912059729780, 0.2920915426229240, 0.3926785587852390, 0.5354672113127650, 0.6169657298756830, 0.6993887726683350, 0.7501832875129180,
150: 		0.8386871644310250, 0.9103651277237890, 0.9764351312310820, 0.9926345853646890, 0.9300339420371430, 0.8258742381128520, 0.7499387624704700, 0.6725402422076670, 0.6229867037490120, 0.6045297568333980,
151: 		0.5039297182908610, 0.4455885766999630, 0.3779726796046290, 0.3429218283962470, 0.2929489553699220, 0.1570518914741900, 0.0572157944020108, -0.0463361936582273, -0.1573451851308570, -0.2931672021738570,
152: 		-0.4528725984930980, -0.6063286567677410, -0.6995137850617610, -0.7902112777605310, -0.9139878106324440, -0.9998170324167670, -0.9448389776183910, -0.5177630386017770, 0.3950506584043580, 0.7346367050507850,
153: 	}
154: }
155: 
156: // testTCMExpectedSineLead is the sineLead array from MBST TrendCycleModeTest.cs.
157: //
158: //nolint:dupl
159: func testTCMExpectedSineLead() []float64 {
160: 	return []float64{
161: 		math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(),
162: 		math.NaN(), math.NaN(), math.NaN(),
163: 		-0.5865008756606930, 0.6010760134973850, 0.0522375953518313, 0.9091393120803100, 0.8866939724449600, 0.9971590529781770, 0.9020538171846020, 0.6368324474361340, 0.8706218323553310, 0.6073789821251400,
164: 		0.7016671195580980, 0.7163213444559970, 0.7860384941578910, 0.8229422149880850, 0.8625259332042720, 0.9311800857903170, 0.9824560606058480, 0.9999415670942400, 0.9850130504072370, 0.9545149566415440,
165: 		0.9699003741647460, 0.9411885260170180, 0.8957923799982550, 0.8361338691916270, 0.6425371253442400, 0.5619014189746250, 0.2222351486408030, -0.3571214380720770, -0.9310766529205810, -0.9768109870729720,
166: 		-0.9433073135761630, -0.8132374819975520, -0.8689541000414960, -0.8770525415166500, 0.1475931480924170, 0.4397692495572380, 0.2482327294712000, -0.0000755239717352, -0.2313707009630830, -0.5260305525740280,
167: 		-0.8452633448517760, -0.9992677882479890, -0.8993236485622310, -0.6702662015228300, -0.1539244027456790, 0.2492915015754510, 0.6115575080093740, 0.8728740226620090, 0.9909162839717420, 0.9771019730335230,
168: 		0.8396734557306670, 0.5697614425887530, 0.2169861504951610, -0.1451956537134670, -0.4535393411417920, -0.6951822190514210, -0.8597041192450010, -0.9517540619903210, -0.9947396502168260, -0.9975710951353530,
169: 		-0.9310629460274820, -0.8208173676831810, -0.5816830016568750, -0.2522412838591280, 0.0357301876330212, 0.8262025299696030, 0.9210718625890570, 0.6334265388777150, 0.3227045140978610, 0.0576226338781002,
170: 		-0.1775030791814070, -0.3902458029200550, -0.5864888795958400, -0.8204080195335590, -0.9205013506909260, -0.9737752029989330, -0.9954119273819820, -0.9996821539441930, -0.9919003713144100, -0.9593708639942700,
171: 		-0.7902545151018210, -0.7493402295361840, -0.6262584572592140, -0.6703391127989300, -0.6738861991115050, -0.7485223840691900, -0.8229875936131110, -0.9074292725287760, -0.9671080150009190, -0.9960855660349680,
172: 		-0.9958932520932240, -0.9574138340514830, -0.8652041011461150, -0.7083111287566420, -0.4646190135134790, 0.3799051621364850, 0.9157357095569460, 0.9816417978833290, 0.8545864068220020, 0.6658040149144910,
173: 		0.3974026651162090, 0.0660812869143209, -0.2292556000437840, -0.4438015597350790, -0.5336772503175370, -0.5234089018965820, -0.4969142899048420, -0.5089421255844810, -0.5404930402692640, -0.5900897578798300,
174: 		-0.6807716781362360, -0.7164442325283580, -0.7215212414254330, -0.7420085481569570, -0.7664280384961050, -0.7928795495979390, -0.7698142295937170, -0.7466749292126270, -0.7231440467849940, -0.6809589449044260,
175: 		-0.7069745376146330, -0.7314048006899320, -0.7503819371382670, -0.7724343179777090, -0.8126119368714450, -0.8592475635206920, -0.9071355135227490, -0.9489640525891650, -0.9835954633562350, -0.9994945745179860,
176: 		-0.9967791122784410, -0.9726372051055730, -0.9438266487324060, -0.8236131488885440, -0.6034299982731380, -0.3012592866284200, 0.0451220549347129, 0.3898374562298360, 0.5625345127655470, 0.7610204246544270,
177: 		0.8485754490678880, 0.9426271321408510, 0.9743624133056830, 0.9937744211043730, 0.9999234595478280, 0.9890496834828740, 0.9620813190845490, 0.9257815160887320, 0.8661907447689560, 0.7717933199927350,
178: 		0.6099143741297350, 0.5465660379422910, 0.0650374448708587, -0.2324218737852860, -0.5626485140051470, -0.7858435966184150, -0.9988707502989160, -0.9995056053709990, -0.2269953459568470, 0.2624108067271960,
179: 		0.1922802999868170, -0.0235747958138623, -0.2316259847404300, -0.4374421611852380, -0.6003241683068820, -0.7338638764656210, -0.8186335536900920, -0.9244352736103260, -0.9869359601928300, -0.9999976116346720,
180: 		-0.9662055929589490, -0.8473318021865150, -0.6441207677355740, -0.3775643212631200, -0.0838943519776584, 0.1891385830833930, 0.4138657569596380, 0.4619148062355390, 0.6205889300081480, 0.6637092720958960,
181: 		0.6931871751689060, 0.6994688836282570, 0.6998698746833480, 0.6950322988997850, 0.6110036127434860, 0.5192294559798360, 0.5125969121038930, 0.5757083087460280, 0.6700235759926480, 0.7115168281729820,
182: 		0.7036678339992160, 0.6545752979714670, 0.6129364849611150, 0.6000111421090120, 0.6124554033545860, 0.6347294098505560, 0.6562899876128100, 0.6828327376881520, 0.6957402985880160, 0.7207141791649000,
183: 		0.7487908163625680, 0.7757975773123150, 0.8040241461526770, 0.8364818144534760, 0.8828100232401670, 0.9279746557928580, 0.9758237828336110, 0.9927466296862280, 0.9999410720652520, 0.9980198650776760,
184: 		0.9781412645524530, 0.9363303228513180, 0.8430454008691210, 0.6162348270656310, 0.3977902134753690, 0.1852760304055380, 0.0625305163973752, -0.0477440698701413, -0.1126037294757250, -0.1358018386690990,
185: 		-0.2544275858052280, -0.3179500551407050, -0.3873842567862560, -0.4217482646499100, -0.4689384577141290, -0.5872793774815170, -0.6654907471409910, -0.7391119157698760, -0.8095586705039820, -0.8833378646001520,
186: 		-0.9506681839279980, -0.9910404297342630, -0.9999429557041830, -0.9921031907559610, -0.9331896469980470, -0.6934514712647360, -0.4364996881408350, 0.2388331163456430, 0.9289332462002390, 0.9992107524543470,
187: 	}
188: }
189: 
190: // testTCMExpectedITL is the itl array from MBST TrendCycleModeTest.cs.
191: //
192: //nolint:dupl
193: func testTCMExpectedITL() []float64 {
194: 	return []float64{
195: 		math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(),
196: 		math.NaN(), math.NaN(), math.NaN(),
197: 		0.0000000000000, 36.3130000000000, 63.1977500000000, 81.3542500000000, 90.5332083333333, 90.7274166666667, 90.8630416666667, 90.7904791666667, 90.5763791666666, 90.1190625000000,
198: 		89.5976625000000, 88.9683964285714, 88.1875857142857, 87.4745535714286, 86.8648353174603, 86.5406809523809, 86.4262830808081, 86.4413116161616, 86.4596270104895, 86.4576728687979,
199: 		86.5316151556777, 86.5976538663542, 86.6975181489262, 86.7923007288442, 86.7608821809425, 86.6849764619883, 86.4810361842105, 86.2045236842105, 85.9179875000000, 85.7217875000000,
200: 		85.6731214285714, 85.7294946428571, 85.8820685064935, 86.1150389610390, 86.4291412337662, 86.8542435064935, 87.1897624458874, 87.4640642857143, 87.6829172619047, 87.8553064223057,
201: 		87.9218177631579, 87.9011081656347, 87.8265154798762, 87.7802132352941, 87.8975367647059, 87.9932665441176, 88.0649329044118, 88.1058593750000, 88.0771718750000, 87.9885156250000,
202: 		87.8919218750000, 87.8735468750000, 87.9217812500000, 88.0132343750000, 88.1667968750000, 88.3524062500000, 88.5845312500000, 88.8867187500000, 89.2125000000000, 89.3545928308824,
203: 		89.4090386029412, 89.2706643178105, 89.1179705882353, 89.2742091503268, 89.6278143274854, 90.2226447368421, 90.9753333333333, 91.7409342105263, 92.5893245614035, 93.3576615497076,
204: 		94.0969948830409, 94.8495138888889, 95.5591111111111, 96.4495179738562, 97.4016168300654, 98.4999640522876, 99.7313676470588, 101.1202794117650, 102.8572647058820, 104.2939607843140,
205: 		105.8849452614380, 107.0814910990710, 108.1507799707600, 108.9417928362570, 109.6523348684210, 110.1277241228070, 110.5267148809520, 110.9164619047620, 111.3471666666670, 111.8653571428570,
206: 		112.3597857142860, 112.7992380952380, 113.1747142857140, 113.5586666666670, 114.0577738095240, 114.6932047619050, 115.2846898809520, 115.7229029761900, 115.8414500000000, 115.7467000000000,
207: 		115.6115500000000, 115.5303375000000, 115.5548000000000, 115.6614000000000, 115.9122375000000, 116.3321625000000, 116.8428375000000, 117.2830500000000, 117.7253922619050, 118.0727228896100,
208: 		118.4984101731600, 119.2198956709960, 120.0234311688310, 120.9289954887220, 121.7905789473680, 122.5689105263160, 123.2356894736840, 123.8346386591480, 124.4085501367050, 124.8498862554110,
209: 		125.4094954004330, 126.1065170454550, 126.7064925000000, 127.4240825000000, 127.8647176923080, 128.0896789173790, 128.1540621001220, 128.0788455784320, 128.1446725050170, 128.3183694581280,
210: 		128.4362103448280, 128.5848821839080, 128.6186973303670, 128.7719790322580, 128.8848929180570, 128.9942553260240, 129.0945314906040, 129.1530999536860, 129.0838581603580, 128.8024011680910,
211: 		128.4259584615380, 127.8496453846150, 127.2448900000000, 126.6843541666670, 126.1858333333330, 125.7495416666670, 125.2761041666670, 124.7606666666670, 124.3471666666670, 124.0095208333330,
212: 		123.7731875000000, 123.6621125000000, 123.6056416666670, 123.5744416666670, 123.5148250000000, 123.4937083333330, 123.4761041666670, 123.6832443181820, 124.0308714826840, 124.5405766233770,
213: 		125.2020811061750, 125.8966645363410, 126.4680156432750, 126.9272675438600, 127.4003611111110, 127.8605833333330, 128.1324342105260, 128.3722748538010, 128.6053611111110, 128.6804526315790,
214: 		128.7627552631580, 128.7694289473680, 128.7595250000000, 128.7409750000000, 128.6613250000000, 128.4858250000000, 128.2154750000000, 127.8500845238100, 127.4645440476190, 127.1153875541130,
215: 		126.7443323451910, 126.3035666290230, 125.8504999604740, 125.3674468450390, 124.8902561172160, 124.3766410622710, 123.8351980503370, 123.2633556835640, 122.5153494623660, 121.7438575268820,
216: 		120.2481227342550, 118.4358502645500, 116.5046713064710, 114.5443467073670, 112.9054354415950, 111.4893484615380, 110.7192281481480, 110.1931396825400, 110.2034782445810, 110.1120396292030,
217: 		109.5987628888250, 108.8005075604840, 107.8019843750000, 106.8111562500000, 105.6388140120970, 104.3560227486560, 102.7347166042630, 101.0735459592080, 99.5273900488400, 98.3256981583231,
218: 		97.3580796011396, 96.7594046153846, 96.4602600000000, 96.1232166666667, 96.1488191666667, 96.4425624637681, 96.7984085144927, 97.1613937318841, 97.3868053381643, 97.5343687643678,
219: 		97.8406773180077, 98.3334693486590, 99.0595665024631, 99.8837727832512, 100.6778885467980, 101.4226269841270, 102.0713564814810, 102.6847440476190, 103.2616851851850, 103.9931695156700,
220: 		104.8224465811970, 105.7652606837610, 106.6112275641030, 107.5680471014490, 108.3253364624510, 108.8823498494260, 109.2757501411630, 109.6873409090910, 110.1830037593980, 110.4551917293230,
221: 	}
222: }
223: 
224: // testTCMExpectedValue is the tcm array from MBST TrendCycleModeTest.cs (first 63 entries NaN).
225: //
226: //nolint:dupl
227: func testTCMExpectedValue() []float64 {
228: 	n := math.NaN()
229: 
230: 	return []float64{
231: 		n, n, n, n, n, n, n, n, n, n,
232: 		n, n, n, n, n, n, n, n, n, n,
233: 		n, n, n, n, n, n, n, n, n, n,
234: 		n, n, n, n, n, n, n, n, n, n,
235: 		n, n, n, n, n, n, n, n, n, n,
236: 		n, n, n, n, n, n, n, n, n, n,
237: 		n, n, n,
238: 		1, 1, 1, 1, 1, 1, 1, -1, -1, 1, 1, 1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
239: 		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, 1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
240: 		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
241: 		1, 1, 1, -1, 1, 1, 1, -1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, 1, 1,
242: 		1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
243: 	}
244: }
245: 
246: const testTCMTolerance = 1e-4
247: 
248: //nolint:funlen
249: func TestTrendCycleModeUpdate(t *testing.T) {
250: 	t.Parallel()
251: 
252: 	input := testTCMInput()
253: 	expPeriod := testTCMExpectedPeriod()
254: 	expPhase := testTCMExpectedPhase()
255: 	expSine := testTCMExpectedSine()
256: 	expSineLead := testTCMExpectedSineLead()
257: 	expITL := testTCMExpectedITL()
258: 	expValue := testTCMExpectedValue()
259: 
260: 	const (
261: 		skip       = 9   // TradeStation implementation skips first 9 bars.
262: 		settleSkip = 177 // Samples required for the EMA to converge past structural reference mismatch.
263: 	)
264: 
265: 	t.Run("reference period", func(t *testing.T) {
266: 		t.Parallel()
267: 
268: 		x := testTCMCreateDefault()
269: 
270: 		for i := skip; i < len(input); i++ {
271: 			_, _, _, _, _, _, period, _ := x.Update(input[i])
272: 			if math.IsNaN(period) || i < settleSkip {
273: 				continue
274: 			}
275: 
276: 			if math.Abs(expPeriod[i]-period) > testTCMTolerance {
277: 				t.Errorf("[%v] period is incorrect: expected %v, actual %v", i, expPeriod[i], period)
278: 			}
279: 		}
280: 	})
281: 
282: 	t.Run("reference phase", func(t *testing.T) {
283: 		t.Parallel()
284: 
285: 		x := testTCMCreateDefault()
286: 
287: 		for i := skip; i < len(input); i++ {
288: 			_, _, _, _, _, _, _, phase := x.Update(input[i])
289: 			if math.IsNaN(phase) || math.IsNaN(expPhase[i]) || i < settleSkip {
290: 				continue
291: 			}
292: 
293: 			// MBST wraps phase into (-180, 180]; zpano into [0, 360). Compare modulo 360.
294: 			d := math.Mod(expPhase[i]-phase, 360.0)
295: 			if d > 180 {
296: 				d -= 360
297: 			} else if d < -180 {
298: 				d += 360
299: 			}
300: 
301: 			if math.Abs(d) > testTCMTolerance {
302: 				t.Errorf("[%v] phase is incorrect: expected %v, actual %v", i, expPhase[i], phase)
303: 			}
304: 		}
305: 	})
306: 
307: 	t.Run("reference sine wave", func(t *testing.T) {
308: 		t.Parallel()
309: 
310: 		x := testTCMCreateDefault()
311: 
312: 		for i := skip; i < len(input); i++ {
313: 			_, _, _, _, sine, _, _, _ := x.Update(input[i])
314: 			if math.IsNaN(sine) || math.IsNaN(expSine[i]) || i < settleSkip {
315: 				continue
316: 			}
317: 
318: 			if math.Abs(expSine[i]-sine) > testTCMTolerance {
319: 				t.Errorf("[%v] sine is incorrect: expected %v, actual %v", i, expSine[i], sine)
320: 			}
321: 		}
322: 	})
323: 
324: 	t.Run("reference sine wave lead", func(t *testing.T) {
325: 		t.Parallel()
326: 
327: 		x := testTCMCreateDefault()
328: 
329: 		for i := skip; i < len(input); i++ {
330: 			_, _, _, _, _, sineLead, _, _ := x.Update(input[i])
331: 			if math.IsNaN(sineLead) || math.IsNaN(expSineLead[i]) || i < settleSkip {
332: 				continue
333: 			}
334: 
335: 			if math.Abs(expSineLead[i]-sineLead) > testTCMTolerance {
336: 				t.Errorf("[%v] sineLead is incorrect: expected %v, actual %v", i, expSineLead[i], sineLead)
337: 			}
338: 		}
339: 	})
340: 
341: 	t.Run("reference instantaneous trend line", func(t *testing.T) {
342: 		t.Parallel()
343: 
344: 		x := testTCMCreateDefault()
345: 
346: 		for i := skip; i < len(input); i++ {
347: 			_, _, _, itl, _, _, _, _ := x.Update(input[i])
348: 			if math.IsNaN(itl) || math.IsNaN(expITL[i]) || i < settleSkip {
349: 				continue
350: 			}
351: 
352: 			if math.Abs(expITL[i]-itl) > testTCMTolerance {
353: 				t.Errorf("[%v] itl is incorrect: expected %v, actual %v", i, expITL[i], itl)
354: 			}
355: 		}
356: 	})
357: 
358: 	t.Run("reference value (tcm)", func(t *testing.T) {
359: 		t.Parallel()
360: 
361: 		x := testTCMCreateDefault()
362: 		limit := len(expValue)
363: 
364: 		for i := skip; i < len(input); i++ {
365: 			value, _, _, _, _, _, _, _ := x.Update(input[i])
366: 			if i >= limit {
367: 				continue
368: 			}
369: 			// MBST known mismatches.
370: 			if i == 70 || i == 71 {
371: 				continue
372: 			}
373: 
374: 			if math.IsNaN(value) || math.IsNaN(expValue[i]) {
375: 				continue
376: 			}
377: 
378: 			if math.Abs(expValue[i]-value) > testTCMTolerance {
379: 				t.Errorf("[%v] value is incorrect: expected %v, actual %v", i, expValue[i], value)
380: 			}
381: 		}
382: 	})
383: 
384: 	t.Run("is-trend / is-cycle are complementary 0/1", func(t *testing.T) {
385: 		t.Parallel()
386: 
387: 		x := testTCMCreateDefault()
388: 
389: 		for i := skip; i < len(input); i++ {
390: 			value, trend, cycle, _, _, _, _, _ := x.Update(input[i])
391: 			if math.IsNaN(value) {
392: 				continue
393: 			}
394: 
395: 			if trend+cycle != 1 {
396: 				t.Errorf("[%v] trend+cycle is incorrect: expected 1, actual %v", i, trend+cycle)
397: 			}
398: 
399: 			if (value > 0 && trend != 1) || (value < 0 && trend != 0) {
400: 				t.Errorf("[%v] value/trend mismatch: value=%v trend=%v", i, value, trend)
401: 			}
402: 		}
403: 	})
404: 
405: 	t.Run("NaN input returns NaN tuple", func(t *testing.T) {
406: 		t.Parallel()
407: 
408: 		x := testTCMCreateDefault()
409: 		value, trend, cycle, itl, sine, sineLead, period, phase := x.Update(math.NaN())
410: 
411: 		if !math.IsNaN(value) || !math.IsNaN(trend) || !math.IsNaN(cycle) ||
412: 			!math.IsNaN(itl) || !math.IsNaN(sine) || !math.IsNaN(sineLead) ||
413: 			!math.IsNaN(period) || !math.IsNaN(phase) {
414: 			t.Errorf("expected all NaN, got (%v, %v, %v, %v, %v, %v, %v, %v)",
415: 				value, trend, cycle, itl, sine, sineLead, period, phase)
416: 		}
417: 	})
418: }
419: 
420: func TestTrendCycleModeIsPrimed(t *testing.T) {
421: 	t.Parallel()
422: 
423: 	input := testTCMInput()
424: 
425: 	t.Run("primes somewhere in the input sequence", func(t *testing.T) {
426: 		t.Parallel()
427: 
428: 		x := testTCMCreateDefault()
429: 
430: 		if x.IsPrimed() {
431: 			t.Error("expected not primed at start")
432: 		}
433: 
434: 		primedAt := -1
435: 
436: 		for i := range input {
437: 			x.Update(input[i])
438: 
439: 			if x.IsPrimed() && primedAt < 0 {
440: 				primedAt = i
441: 			}
442: 		}
443: 
444: 		if primedAt < 0 {
445: 			t.Error("expected indicator to become primed within the input sequence")
446: 		}
447: 
448: 		if !x.IsPrimed() {
449: 			t.Error("expected primed at end")
450: 		}
451: 	})
452: }
453: 
454: //nolint:funlen
455: func TestTrendCycleModeMetadata(t *testing.T) {
456: 	t.Parallel()
457: 
458: 	const (
459: 		descrVal      = "Trend versus cycle mode "
460: 		descrTrend    = "Trend versus cycle mode, is-trend flag "
461: 		descrCycle    = "Trend versus cycle mode, is-cycle flag "
462: 		descrITL      = "Trend versus cycle mode instantaneous trend line "
463: 		descrSine     = "Trend versus cycle mode sine wave "
464: 		descrSineLead = "Trend versus cycle mode sine wave lead "
465: 		descrDCP      = "Dominant cycle period "
466: 		descrDCPha    = "Dominant cycle phase "
467: 	)
468: 
469: 	x := testTCMCreateDefault()
470: 	act := x.Metadata()
471: 
472: 	check := func(what string, exp, a any) {
473: 		t.Helper()
474: 
475: 		if exp != a {
476: 			t.Errorf("%s is incorrect: expected %v, actual %v", what, exp, a)
477: 		}
478: 	}
479: 
480: 	mnVal := "tcm(0.330, 4, 1.000, 1.500%, hl/2)"
481: 	mnTrend := "tcm-trend(0.330, 4, 1.000, 1.500%, hl/2)"
482: 	mnCycle := "tcm-cycle(0.330, 4, 1.000, 1.500%, hl/2)"
483: 	mnITL := "tcm-itl(0.330, 4, 1.000, 1.500%, hl/2)"
484: 	mnSine := "tcm-sine(0.330, 4, 1.000, 1.500%, hl/2)"
485: 	mnSineLead := "tcm-sineLead(0.330, 4, 1.000, 1.500%, hl/2)"
486: 	mnDCP := "dcp(0.330, hl/2)"
487: 	mnDCPha := "dcph(0.330, hl/2)"
488: 
489: 	check("Type", core.TrendCycleMode, act.Type)
490: 	check("Mnemonic", mnVal, act.Mnemonic)
491: 	check("Description", descrVal+mnVal, act.Description)
492: 	check("len(Outputs)", 8, len(act.Outputs))
493: 
494: 	check("Outputs[0].Kind", int(Value), act.Outputs[0].Kind)
495: 	check("Outputs[0].Type", outputs.ScalarType, act.Outputs[0].Type)
496: 	check("Outputs[0].Mnemonic", mnVal, act.Outputs[0].Mnemonic)
497: 	check("Outputs[0].Description", descrVal+mnVal, act.Outputs[0].Description)
498: 
499: 	check("Outputs[1].Kind", int(IsTrendMode), act.Outputs[1].Kind)
500: 	check("Outputs[1].Mnemonic", mnTrend, act.Outputs[1].Mnemonic)
501: 	check("Outputs[1].Description", descrTrend+mnTrend, act.Outputs[1].Description)
502: 
503: 	check("Outputs[2].Kind", int(IsCycleMode), act.Outputs[2].Kind)
504: 	check("Outputs[2].Mnemonic", mnCycle, act.Outputs[2].Mnemonic)
505: 
506: 	check("Outputs[3].Kind", int(InstantaneousTrendLine), act.Outputs[3].Kind)
507: 	check("Outputs[3].Mnemonic", mnITL, act.Outputs[3].Mnemonic)
508: 
509: 	check("Outputs[4].Kind", int(SineWave), act.Outputs[4].Kind)
510: 	check("Outputs[4].Mnemonic", mnSine, act.Outputs[4].Mnemonic)
511: 
512: 	check("Outputs[5].Kind", int(SineWaveLead), act.Outputs[5].Kind)
513: 	check("Outputs[5].Mnemonic", mnSineLead, act.Outputs[5].Mnemonic)
514: 
515: 	check("Outputs[6].Kind", int(DominantCyclePeriod), act.Outputs[6].Kind)
516: 	check("Outputs[6].Mnemonic", mnDCP, act.Outputs[6].Mnemonic)
517: 	check("Outputs[6].Description", descrDCP+mnDCP, act.Outputs[6].Description)
518: 
519: 	check("Outputs[7].Kind", int(DominantCyclePhase), act.Outputs[7].Kind)
520: 	check("Outputs[7].Mnemonic", mnDCPha, act.Outputs[7].Mnemonic)
521: 	check("Outputs[7].Description", descrDCPha+mnDCPha, act.Outputs[7].Description)
522: }
523: 
524: //nolint:funlen
525: func TestTrendCycleModeUpdateEntity(t *testing.T) {
526: 	t.Parallel()
527: 
528: 	const (
529: 		primeCount = 200
530: 		inp        = 100.
531: 		outputLen  = 8
532: 	)
533: 
534: 	tm := testTCMTime()
535: 	check := func(act core.Output) {
536: 		t.Helper()
537: 
538: 		if len(act) != outputLen {
539: 			t.Errorf("len(output) is incorrect: expected %v, actual %v", outputLen, len(act))
540: 
541: 			return
542: 		}
543: 
544: 		for i := 0; i < outputLen; i++ {
545: 			s, ok := act[i].(entities.Scalar)
546: 			if !ok {
547: 				t.Errorf("output[%d] is not a scalar", i)
548: 
549: 				continue
550: 			}
551: 
552: 			if s.Time != tm {
553: 				t.Errorf("output[%d] time is incorrect: expected %v, actual %v", i, tm, s.Time)
554: 			}
555: 		}
556: 	}
557: 
558: 	input := testTCMInput()
559: 
560: 	t.Run("update scalar", func(t *testing.T) {
561: 		t.Parallel()
562: 
563: 		s := entities.Scalar{Time: tm, Value: inp}
564: 		x := testTCMCreateDefault()
565: 
566: 		for i := 0; i < primeCount; i++ {
567: 			x.Update(input[i%len(input)])
568: 		}
569: 
570: 		check(x.UpdateScalar(&s))
571: 	})
572: 
573: 	t.Run("update bar", func(t *testing.T) {
574: 		t.Parallel()
575: 
576: 		b := entities.Bar{Time: tm, High: inp, Low: inp, Close: inp}
577: 		x := testTCMCreateDefault()
578: 
579: 		for i := 0; i < primeCount; i++ {
580: 			x.Update(input[i%len(input)])
581: 		}
582: 
583: 		check(x.UpdateBar(&b))
584: 	})
585: 
586: 	t.Run("update quote", func(t *testing.T) {
587: 		t.Parallel()
588: 
589: 		q := entities.Quote{Time: tm, Bid: inp, Ask: inp}
590: 		x := testTCMCreateDefault()
591: 
592: 		for i := 0; i < primeCount; i++ {
593: 			x.Update(input[i%len(input)])
594: 		}
595: 
596: 		check(x.UpdateQuote(&q))
597: 	})
598: 
599: 	t.Run("update trade", func(t *testing.T) {
600: 		t.Parallel()
601: 
602: 		r := entities.Trade{Time: tm, Price: inp}
603: 		x := testTCMCreateDefault()
604: 
605: 		for i := 0; i < primeCount; i++ {
606: 			x.Update(input[i%len(input)])
607: 		}
608: 
609: 		check(x.UpdateTrade(&r))
610: 	})
611: }
612: 
613: //nolint:funlen,maintidx
614: func TestNewTrendCycleMode(t *testing.T) {
615: 	t.Parallel()
616: 
617: 	const (
618: 		bc entities.BarComponent   = entities.BarClosePrice
619: 		qc entities.QuoteComponent = entities.QuoteMidPrice
620: 		tc entities.TradeComponent = entities.TradePrice
621: 
622: 		errAlpha = "invalid trend cycle mode parameters: " +
623: 			"α for additional smoothing should be in range (0, 1]"
624: 		errTLSL = "invalid trend cycle mode parameters: " +
625: 			"trend line smoothing length should be 2, 3, or 4"
626: 		errCPM = "invalid trend cycle mode parameters: " +
627: 			"cycle part multiplier should be in range (0, 10]"
628: 		errSep = "invalid trend cycle mode parameters: " +
629: 			"separation percentage should be in range (0, 100]"
630: 		errBC = "invalid trend cycle mode parameters: " +
631: 			"invalid dominant cycle parameters: 9999: unknown bar component"
632: 		errQC = "invalid trend cycle mode parameters: " +
633: 			"invalid dominant cycle parameters: 9999: unknown quote component"
634: 		errTC = "invalid trend cycle mode parameters: " +
635: 			"invalid dominant cycle parameters: 9999: unknown trade component"
636: 	)
637: 
638: 	check := func(name string, exp, act any) {
639: 		t.Helper()
640: 
641: 		if exp != act {
642: 			t.Errorf("%s is incorrect: expected %v, actual %v", name, exp, act)
643: 		}
644: 	}
645: 
646: 	t.Run("default", func(t *testing.T) {
647: 		t.Parallel()
648: 
649: 		x, err := NewTrendCycleModeDefault()
650: 		check("err == nil", true, err == nil)
651: 		check("mnemonic", "tcm(0.330, 4, 1.000, 1.500%, hl/2)", x.mnemonic)
652: 		check("primed", false, x.primed)
653: 		check("trendLineSmoothingLength", 4, x.trendLineSmoothingLength)
654: 		check("separationFactor", 0.015, x.separationFactor)
655: 	})
656: 
657: 	t.Run("tlsl=2", func(t *testing.T) {
658: 		t.Parallel()
659: 
660: 		params := Params{
661: 			AlphaEmaPeriodAdditional: 0.33,
662: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
663: 			EstimatorParams:          testTCMCreateCycleEstimatorParams(),
664: 			TrendLineSmoothingLength: 2,
665: 			CyclePartMultiplier:      1.0,
666: 			SeparationPercentage:     1.5,
667: 			BarComponent:             bc, QuoteComponent: qc, TradeComponent: tc,
668: 		}
669: 
670: 		x, err := NewTrendCycleModeParams(&params)
671: 		check("err == nil", true, err == nil)
672: 		check("mnemonic", "tcm(0.330, 2, 1.000, 1.500%)", x.mnemonic)
673: 		check("coeff0", 2.0/3.0, x.coeff0)
674: 		check("coeff1", 1.0/3.0, x.coeff1)
675: 	})
676: 
677: 	t.Run("tlsl=3, phase accumulator", func(t *testing.T) {
678: 		t.Parallel()
679: 
680: 		params := Params{
681: 			AlphaEmaPeriodAdditional: 0.5,
682: 			EstimatorType:            hilberttransformer.PhaseAccumulator,
683: 			EstimatorParams:          testTCMCreateCycleEstimatorParams(),
684: 			TrendLineSmoothingLength: 3,
685: 			CyclePartMultiplier:      0.5,
686: 			SeparationPercentage:     2.0,
687: 			BarComponent:             bc, QuoteComponent: qc, TradeComponent: tc,
688: 		}
689: 
690: 		x, err := NewTrendCycleModeParams(&params)
691: 		check("err == nil", true, err == nil)
692: 		check("mnemonic", "tcm(0.500, 3, 0.500, 2.000%, pa(4, 0.200, 0.200))", x.mnemonic)
693: 	})
694: 
695: 	t.Run("α ≤ 0", func(t *testing.T) {
696: 		t.Parallel()
697: 
698: 		params := Params{
699: 			AlphaEmaPeriodAdditional: 0.0,
700: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
701: 			EstimatorParams:          testTCMCreateCycleEstimatorParams(),
702: 			TrendLineSmoothingLength: 4,
703: 			CyclePartMultiplier:      1.0,
704: 			SeparationPercentage:     1.5,
705: 		}
706: 
707: 		x, err := NewTrendCycleModeParams(&params)
708: 		check("x == nil", true, x == nil)
709: 		check("err", errAlpha, err.Error())
710: 	})
711: 
712: 	t.Run("α > 1", func(t *testing.T) {
713: 		t.Parallel()
714: 
715: 		params := Params{
716: 			AlphaEmaPeriodAdditional: 1.00000001,
717: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
718: 			EstimatorParams:          testTCMCreateCycleEstimatorParams(),
719: 			TrendLineSmoothingLength: 4,
720: 			CyclePartMultiplier:      1.0,
721: 			SeparationPercentage:     1.5,
722: 		}
723: 
724: 		x, err := NewTrendCycleModeParams(&params)
725: 		check("x == nil", true, x == nil)
726: 		check("err", errAlpha, err.Error())
727: 	})
728: 
729: 	t.Run("tlsl < 2", func(t *testing.T) {
730: 		t.Parallel()
731: 
732: 		params := Params{
733: 			AlphaEmaPeriodAdditional: 0.33,
734: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
735: 			EstimatorParams:          testTCMCreateCycleEstimatorParams(),
736: 			TrendLineSmoothingLength: 1,
737: 			CyclePartMultiplier:      1.0,
738: 			SeparationPercentage:     1.5,
739: 		}
740: 
741: 		x, err := NewTrendCycleModeParams(&params)
742: 		check("x == nil", true, x == nil)
743: 		check("err", errTLSL, err.Error())
744: 	})
745: 
746: 	t.Run("tlsl > 4", func(t *testing.T) {
747: 		t.Parallel()
748: 
749: 		params := Params{
750: 			AlphaEmaPeriodAdditional: 0.33,
751: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
752: 			EstimatorParams:          testTCMCreateCycleEstimatorParams(),
753: 			TrendLineSmoothingLength: 5,
754: 			CyclePartMultiplier:      1.0,
755: 			SeparationPercentage:     1.5,
756: 		}
757: 
758: 		x, err := NewTrendCycleModeParams(&params)
759: 		check("x == nil", true, x == nil)
760: 		check("err", errTLSL, err.Error())
761: 	})
762: 
763: 	t.Run("cpMul ≤ 0", func(t *testing.T) {
764: 		t.Parallel()
765: 
766: 		params := Params{
767: 			AlphaEmaPeriodAdditional: 0.33,
768: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
769: 			EstimatorParams:          testTCMCreateCycleEstimatorParams(),
770: 			TrendLineSmoothingLength: 4,
771: 			CyclePartMultiplier:      0.0,
772: 			SeparationPercentage:     1.5,
773: 		}
774: 
775: 		x, err := NewTrendCycleModeParams(&params)
776: 		check("x == nil", true, x == nil)
777: 		check("err", errCPM, err.Error())
778: 	})
779: 
780: 	t.Run("cpMul > 10", func(t *testing.T) {
781: 		t.Parallel()
782: 
783: 		params := Params{
784: 			AlphaEmaPeriodAdditional: 0.33,
785: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
786: 			EstimatorParams:          testTCMCreateCycleEstimatorParams(),
787: 			TrendLineSmoothingLength: 4,
788: 			CyclePartMultiplier:      10.00001,
789: 			SeparationPercentage:     1.5,
790: 		}
791: 
792: 		x, err := NewTrendCycleModeParams(&params)
793: 		check("x == nil", true, x == nil)
794: 		check("err", errCPM, err.Error())
795: 	})
796: 
797: 	t.Run("sep ≤ 0", func(t *testing.T) {
798: 		t.Parallel()
799: 
800: 		params := Params{
801: 			AlphaEmaPeriodAdditional: 0.33,
802: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
803: 			EstimatorParams:          testTCMCreateCycleEstimatorParams(),
804: 			TrendLineSmoothingLength: 4,
805: 			CyclePartMultiplier:      1.0,
806: 			SeparationPercentage:     0.0,
807: 		}
808: 
809: 		x, err := NewTrendCycleModeParams(&params)
810: 		check("x == nil", true, x == nil)
811: 		check("err", errSep, err.Error())
812: 	})
813: 
814: 	t.Run("sep > 100", func(t *testing.T) {
815: 		t.Parallel()
816: 
817: 		params := Params{
818: 			AlphaEmaPeriodAdditional: 0.33,
819: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
820: 			EstimatorParams:          testTCMCreateCycleEstimatorParams(),
821: 			TrendLineSmoothingLength: 4,
822: 			CyclePartMultiplier:      1.0,
823: 			SeparationPercentage:     100.00001,
824: 		}
825: 
826: 		x, err := NewTrendCycleModeParams(&params)
827: 		check("x == nil", true, x == nil)
828: 		check("err", errSep, err.Error())
829: 	})
830: 
831: 	t.Run("invalid bar component", func(t *testing.T) {
832: 		t.Parallel()
833: 
834: 		params := Params{
835: 			AlphaEmaPeriodAdditional: 0.33,
836: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
837: 			EstimatorParams:          testTCMCreateCycleEstimatorParams(),
838: 			TrendLineSmoothingLength: 4,
839: 			CyclePartMultiplier:      1.0,
840: 			SeparationPercentage:     1.5,
841: 			BarComponent:             entities.BarComponent(9999), QuoteComponent: qc, TradeComponent: tc,
842: 		}
843: 
844: 		x, err := NewTrendCycleModeParams(&params)
845: 		check("x == nil", true, x == nil)
846: 		check("err", errBC, err.Error())
847: 	})
848: 
849: 	t.Run("invalid quote component", func(t *testing.T) {
850: 		t.Parallel()
851: 
852: 		params := Params{
853: 			AlphaEmaPeriodAdditional: 0.33,
854: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
855: 			EstimatorParams:          testTCMCreateCycleEstimatorParams(),
856: 			TrendLineSmoothingLength: 4,
857: 			CyclePartMultiplier:      1.0,
858: 			SeparationPercentage:     1.5,
859: 			BarComponent:             bc, QuoteComponent: entities.QuoteComponent(9999), TradeComponent: tc,
860: 		}
861: 
862: 		x, err := NewTrendCycleModeParams(&params)
863: 		check("x == nil", true, x == nil)
864: 		check("err", errQC, err.Error())
865: 	})
866: 
867: 	t.Run("invalid trade component", func(t *testing.T) {
868: 		t.Parallel()
869: 
870: 		params := Params{
871: 			AlphaEmaPeriodAdditional: 0.33,
872: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
873: 			EstimatorParams:          testTCMCreateCycleEstimatorParams(),
874: 			TrendLineSmoothingLength: 4,
875: 			CyclePartMultiplier:      1.0,
876: 			SeparationPercentage:     1.5,
877: 			BarComponent:             bc, QuoteComponent: qc, TradeComponent: entities.TradeComponent(9999),
878: 		}
879: 
880: 		x, err := NewTrendCycleModeParams(&params)
881: 		check("x == nil", true, x == nil)
882: 		check("err", errTC, err.Error())
883: 	})
884: }
885: 
886: func testTCMCreateCycleEstimatorParams() hilberttransformer.CycleEstimatorParams {
887: 	const (
888: 		smoothingLength           = 4
889: 		alphaEmaQuadratureInPhase = 0.2
890: 		alphaEmaPeriod            = 0.2
891: 		warmUpPeriod              = 100
892: 	)
893: 
894: 	return hilberttransformer.CycleEstimatorParams{
895: 		SmoothingLength:           smoothingLength,
896: 		AlphaEmaQuadratureInPhase: alphaEmaQuadratureInPhase,
897: 		AlphaEmaPeriod:            alphaEmaPeriod,
898: 		WarmUpPeriod:              warmUpPeriod,
899: 	}
900: }
901: 
902: func testTCMCreateDefault() *TrendCycleMode {
903: 	x, _ := NewTrendCycleModeDefault()
904: 
905: 	return x
906: }

(End of file - total 906 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 6.0s)

_Thinking:_

 Let me quickly verify the trendcyclemode output_test and params files, then cross-check the doc.go and params.go in the corona helper to confirm the function signatures match what I need.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput_test.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput_test.go</path>
<type>file</type>
<content>1: //nolint:testpackage
2: package trendcyclemode
3: 
4: import (
5: 	"testing"
6: )
7: 
8: func TestOutputString(t *testing.T) {
9: 	t.Parallel()
10: 
11: 	tests := []struct {
12: 		o    Output
13: 		text string
14: 	}{
15: 		{Value, valueStr},
16: 		{IsTrendMode, isTrendModeStr},
17: 		{IsCycleMode, isCycleModeStr},
18: 		{InstantaneousTrendLine, instantaneousTrendLineStr},
19: 		{SineWave, sineWaveStr},
20: 		{SineWaveLead, sineWaveLeadStr},
21: 		{DominantCyclePeriod, dominantCyclePeriodStr},
22: 		{DominantCyclePhase, dominantCyclePhaseStr},
23: 		{outputLast, unknownStr},
24: 		{Output(0), unknownStr},
25: 		{Output(9999), unknownStr},
26: 		{Output(-9999), unknownStr},
27: 	}
28: 
29: 	for _, tt := range tests {
30: 		exp := tt.text
31: 		act := tt.o.String()
32: 
33: 		if exp != act {
34: 			t.Errorf("'%v'.String(): expected '%v', actual '%v'", tt.o, exp, act)
35: 		}
36: 	}
37: }
38: 
39: func TestOutputIsKnown(t *testing.T) {
40: 	t.Parallel()
41: 
42: 	tests := []struct {
43: 		o       Output
44: 		boolean bool
45: 	}{
46: 		{Value, true},
47: 		{IsTrendMode, true},
48: 		{IsCycleMode, true},
49: 		{InstantaneousTrendLine, true},
50: 		{SineWave, true},
51: 		{SineWaveLead, true},
52: 		{DominantCyclePeriod, true},
53: 		{DominantCyclePhase, true},
54: 		{outputLast, false},
55: 		{Output(0), false},
56: 		{Output(9999), false},
57: 		{Output(-9999), false},
58: 	}
59: 
60: 	for _, tt := range tests {
61: 		exp := tt.boolean
62: 		act := tt.o.IsKnown()
63: 
64: 		if exp != act {
65: 			t.Errorf("'%v'.IsKnown(): expected '%v', actual '%v'", tt.o, exp, act)
66: 		}
67: 	}
68: }
69: 
70: func TestOutputMarshalJSON(t *testing.T) {
71: 	t.Parallel()
72: 
73: 	const dqs = "\""
74: 
75: 	var nilstr string
76: 	tests := []struct {
77: 		o         Output
78: 		json      string
79: 		succeeded bool
80: 	}{
81: 		{Value, dqs + valueStr + dqs, true},
82: 		{IsTrendMode, dqs + isTrendModeStr + dqs, true},
83: 		{IsCycleMode, dqs + isCycleModeStr + dqs, true},
84: 		{InstantaneousTrendLine, dqs + instantaneousTrendLineStr + dqs, true},
85: 		{SineWave, dqs + sineWaveStr + dqs, true},
86: 		{SineWaveLead, dqs + sineWaveLeadStr + dqs, true},
87: 		{DominantCyclePeriod, dqs + dominantCyclePeriodStr + dqs, true},
88: 		{DominantCyclePhase, dqs + dominantCyclePhaseStr + dqs, true},
89: 		{outputLast, nilstr, false},
90: 		{Output(9999), nilstr, false},
91: 		{Output(-9999), nilstr, false},
92: 		{Output(0), nilstr, false},
93: 	}
94: 
95: 	for _, tt := range tests {
96: 		exp := tt.json
97: 		bs, err := tt.o.MarshalJSON()
98: 
99: 		if err != nil && tt.succeeded {
100: 			t.Errorf("'%v'.MarshalJSON(): expected success '%v', got error %v", tt.o, exp, err)
101: 
102: 			continue
103: 		}
104: 
105: 		if err == nil && !tt.succeeded {
106: 			t.Errorf("'%v'.MarshalJSON(): expected error, got success", tt.o)
107: 
108: 			continue
109: 		}
110: 
111: 		act := string(bs)
112: 		if exp != act {
113: 			t.Errorf("'%v'.MarshalJSON(): expected '%v', actual '%v'", tt.o, exp, act)
114: 		}
115: 	}
116: }
117: 
118: func TestOutputUnmarshalJSON(t *testing.T) {
119: 	t.Parallel()
120: 
121: 	const dqs = "\""
122: 
123: 	var zero Output
124: 	tests := []struct {
125: 		o         Output
126: 		json      string
127: 		succeeded bool
128: 	}{
129: 		{Value, dqs + valueStr + dqs, true},
130: 		{IsTrendMode, dqs + isTrendModeStr + dqs, true},
131: 		{IsCycleMode, dqs + isCycleModeStr + dqs, true},
132: 		{InstantaneousTrendLine, dqs + instantaneousTrendLineStr + dqs, true},
133: 		{SineWave, dqs + sineWaveStr + dqs, true},
134: 		{SineWaveLead, dqs + sineWaveLeadStr + dqs, true},
135: 		{DominantCyclePeriod, dqs + dominantCyclePeriodStr + dqs, true},
136: 		{DominantCyclePhase, dqs + dominantCyclePhaseStr + dqs, true},
137: 		{zero, dqs + unknownStr + dqs, false},
138: 		{zero, dqs + "foobar" + dqs, false},
139: 	}
140: 
141: 	for _, tt := range tests {
142: 		exp := tt.o
143: 		bs := []byte(tt.json)
144: 
145: 		var o Output
146: 
147: 		err := o.UnmarshalJSON(bs)
148: 		if err != nil && tt.succeeded {
149: 			t.Errorf("UnmarshalJSON('%v'): expected success '%v', got error %v", tt.json, exp, err)
150: 
151: 			continue
152: 		}
153: 
154: 		if err == nil && !tt.succeeded {
155: 			t.Errorf("MarshalJSON('%v'): expected error, got success", tt.json)
156: 
157: 			continue
158: 		}
159: 
160: 		if exp != o {
161: 			t.Errorf("MarshalJSON('%v'): expected '%v', actual '%v'", tt.json, exp, o)
162: 		}
163: 	}
164: }

(End of file - total 164 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.9s)

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

## Assistant (Build · claude-opus-4.7 · 4.5s)

**Tool: bash**

**Input:**
```json
{
  "command": "ls go/indicators/johnehlers/coronaspectrum/ 2>/dev/null; echo \"---smoke---\"; cat go/indicators/johnehlers/corona/corona_smoke_test.go | head -40",
  "description": "List coronaspectrum dir and smoke test header"
}
```

**Output:**
```
coronaspectrumparams.go
---smoke---
//nolint:testpackage
package corona

import (
	"math"
	"testing"
)

// talibInput is the 252-entry MAMA/Ehlers test series reused from the sinewave
// indicator tests (ultimately TA-Lib test_MAMA.xsl, Price D5…D256). Kept here
// so the corona package has no test dependency on sinewave.
//
//nolint:dupl
func talibInput() []float64 {
	return []float64{
		92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,
		94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,
		88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,
		85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,
		83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,
		89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,
		89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,
		88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,
		103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,
		120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,
		114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,
		114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,
		124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,
		137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,
		123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,
		122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,
		123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,
		130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,
		127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,
		121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,
		106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,
		95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,
		94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,
		103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,
		106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,

```

---

## Assistant (Build · claude-opus-4.7 · 21.5s)

_Thinking:_

 I need to verify how `core.ComponentTripleMnemonic` handles the default BarMedianPrice value, and I'm also noticing that MBST uses ceiling and floor operations on parameter bounds—CoronaSpectrum's params are float64, so I'll keep those as floats for the heatmap axis but cast them to integers where needed.

Now I'm ready to write the output file, main indicator, and tests, following TCM's mnemonic style with three output types: Value for the heatmap, DominantCycle, and DominantCycleMedian. I'm also working through the ParameterResolution convention—if the heatmap's i-th value maps to `min + i / ParameterResolution`, then with 49 filter bank bins stepping by 0.5 period across the range 6 to 30, the resolution should be 2.0.

Writing the implementation files...

Now I have sufficient context. Let me write the three remaining files for CoronaSpectrum.

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrumoutput.go",
  "content": "package coronaspectrum\n\nimport (\n\t\"bytes\"\n\t\"fmt\"\n)\n\n// Output describes the outputs of the indicator.\ntype Output int\n\nconst (\n\t// Value is the Corona spectrum heatmap column.\n\tValue Output = iota + 1\n\t// DominantCycle is the weighted-center-of-gravity dominant cycle estimate.\n\tDominantCycle\n\t// DominantCycleMedian is the 5-sample median of DominantCycle.\n\tDominantCycleMedian\n\toutputLast\n)\n\nconst (\n\tvalueStr               = \"value\"\n\tdominantCycleStr       = \"dominantCycle\"\n\tdominantCycleMedianStr = \"dominantCycleMedian\"\n\tunknownStr             = \"unknown\"\n)\n\n// String implements the Stringer interface.\nfunc (o Output) String() string {\n\tswitch o {\n\tcase Value:\n\t\treturn valueStr\n\tcase DominantCycle:\n\t\treturn dominantCycleStr\n\tcase DominantCycleMedian:\n\t\treturn dominantCycleMedianStr\n\tdefault:\n\t\treturn unknownStr\n\t}\n}\n\n// IsKnown determines if this output is known.\nfunc (o Output) IsKnown() bool {\n\treturn o >= Value && o < outputLast\n}\n\n// MarshalJSON implements the Marshaler interface.\nfunc (o Output) MarshalJSON() ([]byte, error) {\n\tconst (\n\t\terrFmt = \"cannot marshal '%s': unknown corona spectrum output\"\n\t\textra  = 2   // Two bytes for quotes.\n\t\tdqc    = '\"' // Double quote character.\n\t)\n\n\ts := o.String()\n\tif s == unknownStr {\n\t\treturn nil, fmt.Errorf(errFmt, s)\n\t}\n\n\tb := make([]byte, 0, len(s)+extra)\n\tb = append(b, dqc)\n\tb = append(b, s...)\n\tb = append(b, dqc)\n\n\treturn b, nil\n}\n\n// UnmarshalJSON implements the Unmarshaler interface.\nfunc (o *Output) UnmarshalJSON(data []byte) error {\n\tconst (\n\t\terrFmt = \"cannot unmarshal '%s': unknown corona spectrum output\"\n\t\tdqs    = \"\\\"\" // Double quote string.\n\t)\n\n\td := bytes.Trim(data, dqs)\n\ts := string(d)\n\n\tswitch s {\n\tcase valueStr:\n\t\t*o = Value\n\tcase dominantCycleStr:\n\t\t*o = DominantCycle\n\tcase dominantCycleMedianStr:\n\t\t*o = DominantCycleMedian\n\tdefault:\n\t\treturn fmt.Errorf(errFmt, s)\n\t}\n\n\treturn nil\n}\n"
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

## Assistant (Build · claude-opus-4.7 · 17.4s)

_Thinking:_

 I can ignore those LSP errors in the conversion directory and move on to writing the output_test.go file.

**Tool: write**

**Input:**
```json
{
  "content": "//nolint:testpackage\npackage coronaspectrum\n\nimport (\n\t\"testing\"\n)\n\nfunc TestOutputString(t *testing.T) {\n\tt.Parallel()\n\n\ttests := []struct {\n\t\to    Output\n\t\ttext string\n\t}{\n\t\t{Value, valueStr},\n\t\t{DominantCycle, dominantCycleStr},\n\t\t{DominantCycleMedian, dominantCycleMedianStr},\n\t\t{outputLast, unknownStr},\n\t\t{Output(0), unknownStr},\n\t\t{Output(9999), unknownStr},\n\t\t{Output(-9999), unknownStr},\n\t}\n\n\tfor _, tt := range tests {\n\t\texp := tt.text\n\t\tact := tt.o.String()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"'%v'.String(): expected '%v', actual '%v'\", tt.o, exp, act)\n\t\t}\n\t}\n}\n\nfunc TestOutputIsKnown(t *testing.T) {\n\tt.Parallel()\n\n\ttests := []struct {\n\t\to       Output\n\t\tboolean bool\n\t}{\n\t\t{Value, true},\n\t\t{DominantCycle, true},\n\t\t{DominantCycleMedian, true},\n\t\t{outputLast, false},\n\t\t{Output(0), false},\n\t\t{Output(9999), false},\n\t\t{Output(-9999), false},\n\t}\n\n\tfor _, tt := range tests {\n\t\texp := tt.boolean\n\t\tact := tt.o.IsKnown()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"'%v'.IsKnown(): expected '%v', actual '%v'\", tt.o, exp, act)\n\t\t}\n\t}\n}\n\nfunc TestOutputMarshalJSON(t *testing.T) {\n\tt.Parallel()\n\n\tconst dqs = \"\\\"\"\n\n\tvar nilstr string\n\ttests := []struct {\n\t\to         Output\n\t\tjson      string\n\t\tsucceeded bool\n\t}{\n\t\t{Value, dqs + valueStr + dqs, true},\n\t\t{DominantCycle, dqs + dominantCycleStr + dqs, true},\n\t\t{DominantCycleMedian, dqs + dominantCycleMedianStr + dqs, true},\n\t\t{outputLast, nilstr, false},\n\t\t{Output(9999), nilstr, false},\n\t\t{Output(-9999), nilstr, false},\n\t\t{Output(0), nilstr, false},\n\t}\n\n\tfor _, tt := range tests {\n\t\texp := tt.json\n\t\tbs, err := tt.o.MarshalJSON()\n\n\t\tif err != nil && tt.succeeded {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected success '%v', got error %v\", tt.o, exp, err)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif err == nil && !tt.succeeded {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected error, got success\", tt.o)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tact := string(bs)\n\t\tif exp != act {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected '%v', actual '%v'\", tt.o, exp, act)\n\t\t}\n\t}\n}\n\nfunc TestOutputUnmarshalJSON(t *testing.T) {\n\tt.Parallel()\n\n\tconst dqs = \"\\\"\"\n\n\tvar zero Output\n\ttests := []struct {\n\t\to         Output\n\t\tjson      string\n\t\tsucceeded bool\n\t}{\n\t\t{Value, dqs + valueStr + dqs, true},\n\t\t{DominantCycle, dqs + dominantCycleStr + dqs, true},\n\t\t{DominantCycleMedian, dqs + dominantCycleMedianStr + dqs, true},\n\t\t{zero, dqs + unknownStr + dqs, false},\n\t\t{zero, dqs + \"foobar\" + dqs, false},\n\t}\n\n\tfor _, tt := range tests {\n\t\texp := tt.o\n\t\tbs := []byte(tt.json)\n\n\t\tvar o Output\n\n\t\terr := o.UnmarshalJSON(bs)\n\t\tif err != nil && tt.succeeded {\n\t\t\tt.Errorf(\"UnmarshalJSON('%v'): expected success '%v', got error %v\", tt.json, exp, err)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif err == nil && !tt.succeeded {\n\t\t\tt.Errorf(\"MarshalJSON('%v'): expected error, got success\", tt.json)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif exp != o {\n\t\t\tt.Errorf(\"MarshalJSON('%v'): expected '%v', actual '%v'\", tt.json, exp, o)\n\t\t}\n\t}\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrumoutput_test.go"
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

## Assistant (Build · claude-opus-4.7 · 7.4s)

_Thinking:_

 I need to quickly review the entity defaults and understand how core.ComponentTripleMnemonic works.

**Tool: grep**

**Input:**
```json
{
  "pattern": "ComponentTripleMnemonic|DefaultQuoteComponent|DefaultTradeComponent|BarMedianPrice",
  "path": "/home/dev/zpano/go/indicators/core"
}
```

**Output:**
```
Found 5 matches
/home/dev/zpano/go/indicators/core/componenttriplemnemonic.go:
  Line 20: // ComponentTripleMnemonic builds a mnemonic suffix string from bar, quote and trade
  Line 23: // For example, if bar component is BarMedianPrice (non-default), the result is ", hl/2".
  Line 25: func ComponentTripleMnemonic(
  Line 34: 	if qc != entities.DefaultQuoteComponent {
  Line 38: 	if tc != entities.DefaultTradeComponent {
```

---

## Assistant (Build · claude-opus-4.7 · 3.5s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/componenttriplemnemonic.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/componenttriplemnemonic.go</path>
<type>file</type>
<content>1: package core
2: 
3: import "zpano/entities"
4: 
5: // BarComponentMnemonic returns a short mnemonic for the given bar component.
6: func BarComponentMnemonic(c entities.BarComponent) string {
7: 	return c.Mnemonic()
8: }
9: 
10: // QuoteComponentMnemonic returns a short mnemonic for the given quote component.
11: func QuoteComponentMnemonic(c entities.QuoteComponent) string {
12: 	return c.Mnemonic()
13: }
14: 
15: // TradeComponentMnemonic returns a short mnemonic for the given trade component.
16: func TradeComponentMnemonic(c entities.TradeComponent) string {
17: 	return c.Mnemonic()
18: }
19: 
20: // ComponentTripleMnemonic builds a mnemonic suffix string from bar, quote and trade
21: // components. A component equal to its default value is omitted from the mnemonic.
22: //
23: // For example, if bar component is BarMedianPrice (non-default), the result is ", hl/2".
24: // If bar component is BarClosePrice (default), it is omitted.
25: func ComponentTripleMnemonic(
26: 	bc entities.BarComponent, qc entities.QuoteComponent, tc entities.TradeComponent,
27: ) string {
28: 	var s string
29: 
30: 	if bc != entities.DefaultBarComponent {
31: 		s += ", " + bc.Mnemonic()
32: 	}
33: 
34: 	if qc != entities.DefaultQuoteComponent {
35: 		s += ", " + qc.Mnemonic()
36: 	}
37: 
38: 	if tc != entities.DefaultTradeComponent {
39: 		s += ", " + tc.Mnemonic()
40: 	}
41: 
42: 	return s
43: }

(End of file - total 43 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 52.2s)

_Thinking:_

 Now I'm setting up the main coronaspectrum.go file with constructor functions following the established pattern—`New` for defaults and `NewWithParams` for custom configurations—along with a mnemonic value format that encodes the raster ranges, parameter bounds, cutoff threshold, and component triple for easy reference.

**Tool: write**

**Input:**
```json
{
  "content": "// Package coronaspectrum implements Ehlers' Corona Spectrum heatmap indicator.\n//\n// The Corona Spectrum measures cyclic activity over a cycle period range\n// (default 6..30 bars) in a bank of contiguous bandpass filters. The amplitude\n// of each filter output is compared to the strongest signal and displayed, in\n// decibels, as a heatmap column. The filter having the strongest output is\n// selected as the current dominant cycle period.\n//\n// Reference: John Ehlers, \"Measuring Cycle Periods\", Stocks & Commodities,\n// November 2008.\npackage coronaspectrum\n\n//nolint: gofumpt\nimport (\n\t\"fmt\"\n\t\"math\"\n\t\"sync\"\n\t\"time\"\n\n\t\"zpano/entities\"\n\t\"zpano/indicators/core\"\n\t\"zpano/indicators/core/outputs\"\n\t\"zpano/indicators/johnehlers/corona\"\n)\n\n// CoronaSpectrum is Ehlers' Corona Spectrum heatmap indicator.\n//\n// It owns a private Corona spectral-analysis engine and exposes three outputs:\n//\n//   - Value: a per-bar heatmap column (decibels across the filter bank).\n//   - DominantCycle: the weighted-center-of-gravity dominant cycle estimate.\n//   - DominantCycleMedian: the 5-sample median of DominantCycle.\ntype CoronaSpectrum struct {\n\tmu                  sync.RWMutex\n\tmnemonic            string\n\tdescription         string\n\tmnemonicDC          string\n\tdescriptionDC       string\n\tmnemonicDCM         string\n\tdescriptionDCM      string\n\tc                   *corona.Corona\n\tminParameterValue   float64\n\tmaxParameterValue   float64\n\tparameterResolution float64\n\tminRasterValue      float64\n\tmaxRasterValue      float64\n\tbarFunc             entities.BarFunc\n\tquoteFunc           entities.QuoteFunc\n\ttradeFunc           entities.TradeFunc\n}\n\n// NewCoronaSpectrumDefault returns an instance created with default parameters.\nfunc NewCoronaSpectrumDefault() (*CoronaSpectrum, error) {\n\treturn NewCoronaSpectrumParams(&Params{})\n}\n\n// NewCoronaSpectrumParams returns an instance created with the supplied parameters.\n//\n//nolint:funlen,cyclop\nfunc NewCoronaSpectrumParams(p *Params) (*CoronaSpectrum, error) {\n\tconst (\n\t\tinvalid       = \"invalid corona spectrum parameters\"\n\t\tfmtMinRaster  = \"%s: MinRasterValue should be >= 0\"\n\t\tfmtMaxRaster  = \"%s: MaxRasterValue should be > MinRasterValue\"\n\t\tfmtMinParam   = \"%s: MinParameterValue should be >= 2\"\n\t\tfmtMaxParam   = \"%s: MaxParameterValue should be > MinParameterValue\"\n\t\tfmtHP         = \"%s: HighPassFilterCutoff should be >= 2\"\n\t\tfmtw          = \"%s: %w\"\n\t\tfmtnVal       = \"cspect(%g, %g, %g, %g, %d%s)\"\n\t\tfmtnDC        = \"cspect-dc(%d%s)\"\n\t\tfmtnDCM       = \"cspect-dcm(%d%s)\"\n\t\tdescrVal      = \"Corona spectrum \"\n\t\tdescrDC       = \"Corona spectrum dominant cycle \"\n\t\tdescrDCM      = \"Corona spectrum dominant cycle median \"\n\t\tdefMinRaster  = 6.0\n\t\tdefMaxRaster  = 20.0\n\t\tdefMinParam   = 6.0\n\t\tdefMaxParam   = 30.0\n\t\tdefHPCutoff   = 30\n\t)\n\n\tcfg := *p\n\n\tif cfg.MinRasterValue == 0 {\n\t\tcfg.MinRasterValue = defMinRaster\n\t}\n\n\tif cfg.MaxRasterValue == 0 {\n\t\tcfg.MaxRasterValue = defMaxRaster\n\t}\n\n\tif cfg.MinParameterValue == 0 {\n\t\tcfg.MinParameterValue = defMinParam\n\t}\n\n\tif cfg.MaxParameterValue == 0 {\n\t\tcfg.MaxParameterValue = defMaxParam\n\t}\n\n\tif cfg.HighPassFilterCutoff == 0 {\n\t\tcfg.HighPassFilterCutoff = defHPCutoff\n\t}\n\n\tif cfg.MinRasterValue < 0 {\n\t\treturn nil, fmt.Errorf(fmtMinRaster, invalid)\n\t}\n\n\tif cfg.MaxRasterValue <= cfg.MinRasterValue {\n\t\treturn nil, fmt.Errorf(fmtMaxRaster, invalid)\n\t}\n\n\t// MBST rounds min up and max down to integers.\n\tminParam := math.Ceil(cfg.MinParameterValue)\n\tmaxParam := math.Floor(cfg.MaxParameterValue)\n\n\tif minParam < 2 {\n\t\treturn nil, fmt.Errorf(fmtMinParam, invalid)\n\t}\n\n\tif maxParam <= minParam {\n\t\treturn nil, fmt.Errorf(fmtMaxParam, invalid)\n\t}\n\n\tif cfg.HighPassFilterCutoff < 2 {\n\t\treturn nil, fmt.Errorf(fmtHP, invalid)\n\t}\n\n\t// CoronaSpectrum mirrors Ehlers' reference: BarMedianPrice default.\n\tbc := cfg.BarComponent\n\tif bc == 0 {\n\t\tbc = entities.BarMedianPrice\n\t}\n\n\tqc := cfg.QuoteComponent\n\tif qc == 0 {\n\t\tqc = entities.DefaultQuoteComponent\n\t}\n\n\ttc := cfg.TradeComponent\n\tif tc == 0 {\n\t\ttc = entities.DefaultTradeComponent\n\t}\n\n\tc, err := corona.NewCorona(&corona.Params{\n\t\tHighPassFilterCutoff:   cfg.HighPassFilterCutoff,\n\t\tMinimalPeriod:          int(minParam),\n\t\tMaximalPeriod:          int(maxParam),\n\t\tDecibelsLowerThreshold: cfg.MinRasterValue,\n\t\tDecibelsUpperThreshold: cfg.MaxRasterValue,\n\t})\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\tbarFunc, err := entities.BarComponentFunc(bc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\tquoteFunc, err := entities.QuoteComponentFunc(qc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\ttradeFunc, err := entities.TradeComponentFunc(tc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\tcomponentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)\n\n\tmnemonicVal := fmt.Sprintf(fmtnVal,\n\t\tcfg.MinRasterValue, cfg.MaxRasterValue, minParam, maxParam, cfg.HighPassFilterCutoff, componentMnemonic)\n\tmnemonicDC := fmt.Sprintf(fmtnDC, cfg.HighPassFilterCutoff, componentMnemonic)\n\tmnemonicDCM := fmt.Sprintf(fmtnDCM, cfg.HighPassFilterCutoff, componentMnemonic)\n\n\t// Parameter resolution: 2 samples per period (half-period stepping).\n\t// Values slice length = (maxParam*2 - minParam*2 + 1). The first sample sits\n\t// at minParam, the last at maxParam. Resolution satisfies:\n\t//   min + (length-1)/resolution = max  =>  resolution = (length-1) / (max - min).\n\tparamResolution := float64(c.FilterBankLength()-1) / (maxParam - minParam)\n\n\treturn &CoronaSpectrum{\n\t\tmnemonic:            mnemonicVal,\n\t\tdescription:         descrVal + mnemonicVal,\n\t\tmnemonicDC:          mnemonicDC,\n\t\tdescriptionDC:       descrDC + mnemonicDC,\n\t\tmnemonicDCM:         mnemonicDCM,\n\t\tdescriptionDCM:      descrDCM + mnemonicDCM,\n\t\tc:                   c,\n\t\tminParameterValue:   minParam,\n\t\tmaxParameterValue:   maxParam,\n\t\tparameterResolution: paramResolution,\n\t\tminRasterValue:      cfg.MinRasterValue,\n\t\tmaxRasterValue:      cfg.MaxRasterValue,\n\t\tbarFunc:             barFunc,\n\t\tquoteFunc:           quoteFunc,\n\t\ttradeFunc:           tradeFunc,\n\t}, nil\n}\n\n// IsPrimed indicates whether the indicator is primed.\nfunc (s *CoronaSpectrum) IsPrimed() bool {\n\ts.mu.RLock()\n\tdefer s.mu.RUnlock()\n\n\treturn s.c.IsPrimed()\n}\n\n// Metadata describes the output data of the indicator.\nfunc (s *CoronaSpectrum) Metadata() core.Metadata {\n\treturn core.Metadata{\n\t\tType:        core.CoronaSpectrum,\n\t\tMnemonic:    s.mnemonic,\n\t\tDescription: s.description,\n\t\tOutputs: []outputs.Metadata{\n\t\t\t{Kind: int(Value), Type: outputs.HeatmapType, Mnemonic: s.mnemonic, Description: s.description},\n\t\t\t{Kind: int(DominantCycle), Type: outputs.ScalarType, Mnemonic: s.mnemonicDC, Description: s.descriptionDC},\n\t\t\t{Kind: int(DominantCycleMedian), Type: outputs.ScalarType, Mnemonic: s.mnemonicDCM, Description: s.descriptionDCM},\n\t\t},\n\t}\n}\n\n// Update feeds the next sample to the engine and returns the heatmap column\n// plus the current DominantCycle and DominantCycleMedian estimates.\n//\n// On unprimed bars the heatmap is an empty heatmap (with the indicator's\n// parameter axis) and both scalar values are NaN. On a NaN input sample,\n// state is left unchanged and all outputs are NaN / empty heatmap.\nfunc (s *CoronaSpectrum) Update(sample float64, t time.Time) (*outputs.Heatmap, float64, float64) {\n\ts.mu.Lock()\n\tdefer s.mu.Unlock()\n\n\tif math.IsNaN(sample) {\n\t\treturn outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution),\n\t\t\tmath.NaN(), math.NaN()\n\t}\n\n\tprimed := s.c.Update(sample)\n\tif !primed {\n\t\treturn outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution),\n\t\t\tmath.NaN(), math.NaN()\n\t}\n\n\tbank := s.c.FilterBank()\n\tvalues := make([]float64, len(bank))\n\tvalueMin := math.Inf(1)\n\tvalueMax := math.Inf(-1)\n\n\tfor i := range bank {\n\t\tv := bank[i].Decibels\n\t\tvalues[i] = v\n\n\t\tif v < valueMin {\n\t\t\tvalueMin = v\n\t\t}\n\n\t\tif v > valueMax {\n\t\t\tvalueMax = v\n\t\t}\n\t}\n\n\theatmap := outputs.NewHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution,\n\t\tvalueMin, valueMax, values)\n\n\treturn heatmap, s.c.DominantCycle(), s.c.DominantCycleMedian()\n}\n\n// UpdateScalar updates the indicator given the next scalar sample.\nfunc (s *CoronaSpectrum) UpdateScalar(sample *entities.Scalar) core.Output {\n\treturn s.updateEntity(sample.Time, sample.Value)\n}\n\n// UpdateBar updates the indicator given the next bar sample.\nfunc (s *CoronaSpectrum) UpdateBar(sample *entities.Bar) core.Output {\n\treturn s.updateEntity(sample.Time, s.barFunc(sample))\n}\n\n// UpdateQuote updates the indicator given the next quote sample.\nfunc (s *CoronaSpectrum) UpdateQuote(sample *entities.Quote) core.Output {\n\treturn s.updateEntity(sample.Time, s.quoteFunc(sample))\n}\n\n// UpdateTrade updates the indicator given the next trade sample.\nfunc (s *CoronaSpectrum) UpdateTrade(sample *entities.Trade) core.Output {\n\treturn s.updateEntity(sample.Time, s.tradeFunc(sample))\n}\n\nfunc (s *CoronaSpectrum) updateEntity(t time.Time, sample float64) core.Output {\n\tconst length = 3\n\n\theatmap, dc, dcm := s.Update(sample, t)\n\n\toutput := make([]any, length)\n\toutput[0] = heatmap\n\toutput[1] = entities.Scalar{Time: t, Value: dc}\n\toutput[2] = entities.Scalar{Time: t, Value: dcm}\n\n\treturn output\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrum.go"
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

## Assistant (Build · claude-opus-4.7 · 4.5s)

Let me compile and check.

**Tool: bash**

**Input:**
```json
{
  "command": "go build ./indicators/johnehlers/coronaspectrum/... 2>&1",
  "workdir": "/home/dev/zpano/go",
  "description": "Build coronaspectrum package"
}
```

---

## Assistant (Build · claude-opus-4.7 · 4.1s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./indicators/johnehlers/coronaspectrum/... 2>&1 | tail -50",
  "description": "Run coronaspectrum tests",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
ok  	zpano/indicators/johnehlers/coronaspectrum	0.002s

```

---

## Assistant (Build · claude-opus-4.7 · 44.3s)

Output+params tests pass. Now I need a main indicator test file. Let me capture numeric snapshot values by running a small program first.

**Tool: bash**

**Input:**
```json
{
  "command": "cat > /tmp/cs_snapshot.go <<'EOF'\npackage main\n\nimport (\n\t\"fmt\"\n\t\"time\"\n\n\t\"zpano/indicators/johnehlers/coronaspectrum\"\n)\n\nfunc talibInput() []float64 {\n\treturn []float64{\n\t\t92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,\n\t\t94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,\n\t\t88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,\n\t\t85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,\n\t\t83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,\n\t\t89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,\n\t\t89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,\n\t\t88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,\n\t\t103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,\n\t\t120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,\n\t\t114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,\n\t\t114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,\n\t\t124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,\n\t\t137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,\n\t\t123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,\n\t\t122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,\n\t\t123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,\n\t\t130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,\n\t\t127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,\n\t\t121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,\n\t\t106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,\n\t\t95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,\n\t\t94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,\n\t\t103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,\n\t\t106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,\n\t\t109.5300, 108.0600,\n\t}\n}\n\nfunc main() {\n\tx, err := coronaspectrum.NewCoronaSpectrumDefault()\n\tif err != nil { panic(err) }\n\tinp := talibInput()\n\tt0 := time.Date(2021, 1, 1, 0, 0, 0, 0, time.UTC)\n\tprimedAt := -1\n\tfor i, v := range inp {\n\t\t_, dc, dcm := x.Update(v, t0.Add(time.Duration(i)*time.Minute))\n\t\tif x.IsPrimed() && primedAt < 0 { primedAt = i }\n\t\tif i == 11 || i == 12 || i == 50 || i == 100 || i == 150 || i == 200 || i == 251 {\n\t\t\tfmt.Printf(\"[%d] dc=%.10f dcm=%.10f primed=%v\\n\", i, dc, dcm, x.IsPrimed())\n\t\t}\n\t}\n\tfmt.Println(\"primedAt:\", primedAt)\n\n\t// Heatmap length and some sample values\n\th, _, _ := x.Update(inp[0], t0)\n\t_ = h\n\t// Re-run to get a primed heatmap\n\tx2, _ := coronaspectrum.NewCoronaSpectrumDefault()\n\tvar lastH any\n\tfor i, v := range inp {\n\t\th, _, _ := x2.Update(v, t0.Add(time.Duration(i)*time.Minute))\n\t\tif i == 251 { lastH = h; fmt.Printf(\"[251] len(values)=%d pmin=%v pmax=%v pres=%v vmin=%.6f vmax=%.6f\\n\",\n\t\t\tlen(h.Values), h.ParameterFirst, h.ParameterLast, h.ParameterResolution, h.ValueMin, h.ValueMax) }\n\t}\n\t_ = lastH\n}\nEOF\ngo run /tmp/cs_snapshot.go 2>&1 | tail -20",
  "description": "Produce numeric snapshots for coronaspectrum",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
[11] dc=17.7604672565 dcm=17.7604672565 primed=true
[12] dc=6.0000000000 dcm=6.0000000000 primed=true
[50] dc=15.9989078712 dcm=15.9989078712 primed=true
[100] dc=14.7455497547 dcm=14.7455497547 primed=true
[150] dc=17.5000000000 dcm=17.2826036069 primed=true
[200] dc=19.7557338512 dcm=20.0000000000 primed=true
[251] dc=6.0000000000 dcm=6.0000000000 primed=true
primedAt: 11
[251] len(values)=49 pmin=6 pmax=30 pres=2 vmin=8.123934 vmax=19.941383

```

---

## Assistant (Build · claude-opus-4.7 · 23.6s)

_Thinking:_

 I've captured some sample values and now I need to grab a couple of heatmap readings at the i=251 position to establish the reference values.

**Tool: bash**

**Input:**
```json
{
  "command": "cat >> /tmp/cs_snapshot.go <<'EOF'\nEOF\n\ncat > /tmp/cs_snapshot2.go <<'EOF'\npackage main\n\nimport (\n\t\"fmt\"\n\t\"time\"\n\n\t\"zpano/indicators/johnehlers/coronaspectrum\"\n)\n\nfunc talibInput() []float64 {\n\treturn []float64{\n\t\t92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,\n\t\t94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,\n\t\t88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,\n\t\t85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,\n\t\t83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,\n\t\t89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,\n\t\t89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,\n\t\t88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,\n\t\t103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,\n\t\t120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,\n\t\t114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,\n\t\t114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,\n\t\t124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,\n\t\t137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,\n\t\t123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,\n\t\t122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,\n\t\t123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,\n\t\t130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,\n\t\t127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,\n\t\t121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,\n\t\t106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,\n\t\t95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,\n\t\t94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,\n\t\t103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,\n\t\t106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,\n\t\t109.5300, 108.0600,\n\t}\n}\n\nfunc main() {\n\tx, _ := coronaspectrum.NewCoronaSpectrumDefault()\n\tinp := talibInput()\n\tt0 := time.Date(2021, 1, 1, 0, 0, 0, 0, time.UTC)\n\tvar lastH any\n\tfor i, v := range inp {\n\t\th, _, _ := x.Update(v, t0.Add(time.Duration(i)*time.Minute))\n\t\tlastH = h\n\t\t_ = i\n\t}\n\th := lastH.(*struct{}) // placeholder; replaced below\n\t_ = h\n}\nEOF\n",
  "description": "Setup script placeholder",
  "workdir": "/home/dev/zpano/go"
}
```

---

## Assistant (Build · claude-opus-4.7 · 62.0s)

_Thinking:_

 Writing the test file...

**Tool: write**

**Input:**
```json
{
  "content": "//nolint:testpackage\npackage coronaspectrum\n\nimport (\n\t\"math\"\n\t\"testing\"\n\t\"time\"\n\n\t\"zpano/entities\"\n\t\"zpano/indicators/core\"\n\t\"zpano/indicators/core/outputs\"\n)\n\nfunc testCSTime() time.Time {\n\treturn time.Date(2021, time.April, 1, 0, 0, 0, 0, &time.Location{})\n}\n\n// testCSInput is the 252-entry TA-Lib MAMA reference series (Price D5…D256),\n// reused here so the coronaspectrum package has no test dependency on other\n// packages.\n//\n//nolint:dupl\nfunc testCSInput() []float64 {\n\treturn []float64{\n\t\t92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,\n\t\t94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,\n\t\t88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,\n\t\t85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,\n\t\t83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,\n\t\t89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,\n\t\t89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,\n\t\t88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,\n\t\t103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,\n\t\t120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,\n\t\t114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,\n\t\t114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,\n\t\t124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,\n\t\t137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,\n\t\t123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,\n\t\t122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,\n\t\t123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,\n\t\t130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,\n\t\t127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,\n\t\t121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,\n\t\t106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,\n\t\t95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,\n\t\t94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,\n\t\t103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,\n\t\t106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,\n\t\t109.5300, 108.0600,\n\t}\n}\n\nconst testCSTolerance = 1e-4\n\nfunc TestCoronaSpectrumUpdate(t *testing.T) {\n\tt.Parallel()\n\n\tinput := testCSInput()\n\tt0 := testCSTime()\n\n\t// Snapshot values captured from a first run and locked in here.\n\ttype snap struct {\n\t\ti   int\n\t\tdc  float64\n\t\tdcm float64\n\t}\n\tsnapshots := []snap{\n\t\t{11, 17.7604672565, 17.7604672565},\n\t\t{12, 6.0000000000, 6.0000000000},\n\t\t{50, 15.9989078712, 15.9989078712},\n\t\t{100, 14.7455497547, 14.7455497547},\n\t\t{150, 17.5000000000, 17.2826036069},\n\t\t{200, 19.7557338512, 20.0000000000},\n\t\t{251, 6.0000000000, 6.0000000000},\n\t}\n\n\tx, err := NewCoronaSpectrumDefault()\n\tif err != nil {\n\t\tt.Fatalf(\"unexpected error: %v\", err)\n\t}\n\n\tsi := 0\n\tfor i := range input {\n\t\th, dc, dcm := x.Update(input[i], t0.Add(time.Duration(i)*time.Minute))\n\n\t\tif h == nil {\n\t\t\tt.Fatalf(\"[%d] heatmap must not be nil\", i)\n\t\t}\n\n\t\t// Heatmap axis invariants are always present.\n\t\tif h.ParameterFirst != 6 || h.ParameterLast != 30 || h.ParameterResolution != 2 {\n\t\t\tt.Errorf(\"[%d] heatmap axis incorrect: first=%v last=%v res=%v\",\n\t\t\t\ti, h.ParameterFirst, h.ParameterLast, h.ParameterResolution)\n\t\t}\n\n\t\tif !x.IsPrimed() {\n\t\t\tif !h.IsEmpty() {\n\t\t\t\tt.Errorf(\"[%d] expected empty heatmap before priming, got len=%d\", i, len(h.Values))\n\t\t\t}\n\n\t\t\tif !math.IsNaN(dc) || !math.IsNaN(dcm) {\n\t\t\t\tt.Errorf(\"[%d] expected NaN scalars before priming, got dc=%v dcm=%v\", i, dc, dcm)\n\t\t\t}\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif len(h.Values) != 49 {\n\t\t\tt.Errorf(\"[%d] heatmap values length: expected 49, got %d\", i, len(h.Values))\n\t\t}\n\n\t\tif si < len(snapshots) && snapshots[si].i == i {\n\t\t\tif math.Abs(snapshots[si].dc-dc) > testCSTolerance {\n\t\t\t\tt.Errorf(\"[%d] dc: expected %v, got %v\", i, snapshots[si].dc, dc)\n\t\t\t}\n\n\t\t\tif math.Abs(snapshots[si].dcm-dcm) > testCSTolerance {\n\t\t\t\tt.Errorf(\"[%d] dcm: expected %v, got %v\", i, snapshots[si].dcm, dcm)\n\t\t\t}\n\n\t\t\tsi++\n\t\t}\n\t}\n\n\tif si != len(snapshots) {\n\t\tt.Errorf(\"did not hit all %d snapshots, reached %d\", len(snapshots), si)\n\t}\n}\n\nfunc TestCoronaSpectrumPrimesAtBar11(t *testing.T) {\n\tt.Parallel()\n\n\tx, _ := NewCoronaSpectrumDefault()\n\n\tif x.IsPrimed() {\n\t\tt.Error(\"expected not primed at start\")\n\t}\n\n\tinput := testCSInput()\n\tt0 := testCSTime()\n\tprimedAt := -1\n\n\tfor i := range input {\n\t\tx.Update(input[i], t0.Add(time.Duration(i)*time.Minute))\n\n\t\tif x.IsPrimed() && primedAt < 0 {\n\t\t\tprimedAt = i\n\t\t}\n\t}\n\n\t// MinimalPeriodTimesTwo = 12, so priming first happens at sample index 11 (0-indexed).\n\tif primedAt != 11 {\n\t\tt.Errorf(\"expected priming at index 11, got %d\", primedAt)\n\t}\n}\n\nfunc TestCoronaSpectrumNaNInput(t *testing.T) {\n\tt.Parallel()\n\n\tx, _ := NewCoronaSpectrumDefault()\n\n\th, dc, dcm := x.Update(math.NaN(), testCSTime())\n\n\tif h == nil || !h.IsEmpty() {\n\t\tt.Errorf(\"expected empty heatmap for NaN input, got %v\", h)\n\t}\n\n\tif !math.IsNaN(dc) || !math.IsNaN(dcm) {\n\t\tt.Errorf(\"expected NaN scalars for NaN input, got dc=%v dcm=%v\", dc, dcm)\n\t}\n\n\t// NaN must not prime the indicator.\n\tif x.IsPrimed() {\n\t\tt.Error(\"NaN input must not prime the indicator\")\n\t}\n}\n\nfunc TestCoronaSpectrumMetadata(t *testing.T) {\n\tt.Parallel()\n\n\tx, _ := NewCoronaSpectrumDefault()\n\tmd := x.Metadata()\n\n\tcheck := func(what string, exp, act any) {\n\t\tt.Helper()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"%s: expected %v, actual %v\", what, exp, act)\n\t\t}\n\t}\n\n\tmnVal := \"cspect(6, 20, 6, 30, 30, hl/2)\"\n\tmnDC := \"cspect-dc(30, hl/2)\"\n\tmnDCM := \"cspect-dcm(30, hl/2)\"\n\n\tcheck(\"Type\", core.CoronaSpectrum, md.Type)\n\tcheck(\"Mnemonic\", mnVal, md.Mnemonic)\n\tcheck(\"Description\", \"Corona spectrum \"+mnVal, md.Description)\n\tcheck(\"len(Outputs)\", 3, len(md.Outputs))\n\n\tcheck(\"Outputs[0].Kind\", int(Value), md.Outputs[0].Kind)\n\tcheck(\"Outputs[0].Type\", outputs.HeatmapType, md.Outputs[0].Type)\n\tcheck(\"Outputs[0].Mnemonic\", mnVal, md.Outputs[0].Mnemonic)\n\n\tcheck(\"Outputs[1].Kind\", int(DominantCycle), md.Outputs[1].Kind)\n\tcheck(\"Outputs[1].Type\", outputs.ScalarType, md.Outputs[1].Type)\n\tcheck(\"Outputs[1].Mnemonic\", mnDC, md.Outputs[1].Mnemonic)\n\n\tcheck(\"Outputs[2].Kind\", int(DominantCycleMedian), md.Outputs[2].Kind)\n\tcheck(\"Outputs[2].Type\", outputs.ScalarType, md.Outputs[2].Type)\n\tcheck(\"Outputs[2].Mnemonic\", mnDCM, md.Outputs[2].Mnemonic)\n}\n\n//nolint:funlen\nfunc TestCoronaSpectrumUpdateEntity(t *testing.T) {\n\tt.Parallel()\n\n\tconst (\n\t\tprimeCount = 50\n\t\tinp        = 100.\n\t\toutputLen  = 3\n\t)\n\n\ttm := testCSTime()\n\tinput := testCSInput()\n\n\tcheck := func(act core.Output) {\n\t\tt.Helper()\n\n\t\tif len(act) != outputLen {\n\t\t\tt.Errorf(\"len(output): expected %v, actual %v\", outputLen, len(act))\n\n\t\t\treturn\n\t\t}\n\n\t\th, ok := act[0].(*outputs.Heatmap)\n\t\tif !ok {\n\t\t\tt.Errorf(\"output[0] is not a heatmap: %T\", act[0])\n\t\t} else if h.Time != tm {\n\t\t\tt.Errorf(\"output[0].Time: expected %v, actual %v\", tm, h.Time)\n\t\t}\n\n\t\tfor i := 1; i < outputLen; i++ {\n\t\t\ts, ok := act[i].(entities.Scalar)\n\t\t\tif !ok {\n\t\t\t\tt.Errorf(\"output[%d] is not a scalar\", i)\n\n\t\t\t\tcontinue\n\t\t\t}\n\n\t\t\tif s.Time != tm {\n\t\t\t\tt.Errorf(\"output[%d].Time: expected %v, actual %v\", i, tm, s.Time)\n\t\t\t}\n\t\t}\n\t}\n\n\tt.Run(\"update scalar\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\ts := entities.Scalar{Time: tm, Value: inp}\n\t\tx, _ := NewCoronaSpectrumDefault()\n\n\t\tfor i := 0; i < primeCount; i++ {\n\t\t\tx.Update(input[i%len(input)], tm)\n\t\t}\n\n\t\tcheck(x.UpdateScalar(&s))\n\t})\n\n\tt.Run(\"update bar\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tb := entities.Bar{Time: tm, High: inp, Low: inp, Close: inp}\n\t\tx, _ := NewCoronaSpectrumDefault()\n\n\t\tfor i := 0; i < primeCount; i++ {\n\t\t\tx.Update(input[i%len(input)], tm)\n\t\t}\n\n\t\tcheck(x.UpdateBar(&b))\n\t})\n\n\tt.Run(\"update quote\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tq := entities.Quote{Time: tm, Bid: inp, Ask: inp}\n\t\tx, _ := NewCoronaSpectrumDefault()\n\n\t\tfor i := 0; i < primeCount; i++ {\n\t\t\tx.Update(input[i%len(input)], tm)\n\t\t}\n\n\t\tcheck(x.UpdateQuote(&q))\n\t})\n\n\tt.Run(\"update trade\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tr := entities.Trade{Time: tm, Price: inp}\n\t\tx, _ := NewCoronaSpectrumDefault()\n\n\t\tfor i := 0; i < primeCount; i++ {\n\t\t\tx.Update(input[i%len(input)], tm)\n\t\t}\n\n\t\tcheck(x.UpdateTrade(&r))\n\t})\n}\n\n//nolint:funlen\nfunc TestNewCoronaSpectrum(t *testing.T) {\n\tt.Parallel()\n\n\tcheck := func(name string, exp, act any) {\n\t\tt.Helper()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"%s: expected %v, actual %v\", name, exp, act)\n\t\t}\n\t}\n\n\tt.Run(\"default\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx, err := NewCoronaSpectrumDefault()\n\t\tcheck(\"err == nil\", true, err == nil)\n\t\tcheck(\"mnemonic\", \"cspect(6, 20, 6, 30, 30, hl/2)\", x.mnemonic)\n\t\tcheck(\"MinParameterValue\", 6.0, x.minParameterValue)\n\t\tcheck(\"MaxParameterValue\", 30.0, x.maxParameterValue)\n\t\tcheck(\"ParameterResolution\", 2.0, x.parameterResolution)\n\t})\n\n\tt.Run(\"custom ranges round to integers\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx, err := NewCoronaSpectrumParams(&Params{\n\t\t\tMinRasterValue:       4,\n\t\t\tMaxRasterValue:       25,\n\t\t\tMinParameterValue:    8.7, // ceils to 9\n\t\t\tMaxParameterValue:    40.4, // floors to 40\n\t\t\tHighPassFilterCutoff: 20,\n\t\t})\n\t\tcheck(\"err == nil\", true, err == nil)\n\t\tcheck(\"MinParameterValue\", 9.0, x.minParameterValue)\n\t\tcheck(\"MaxParameterValue\", 40.0, x.maxParameterValue)\n\t\tcheck(\"mnemonic\", \"cspect(4, 25, 9, 40, 20, hl/2)\", x.mnemonic)\n\t})\n\n\tt.Run(\"MaxRasterValue <= MinRasterValue\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSpectrumParams(&Params{\n\t\t\tMinRasterValue:    10,\n\t\t\tMaxRasterValue:    10,\n\t\t\tMinParameterValue: 6,\n\t\t\tMaxParameterValue: 30,\n\t\t})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona spectrum parameters: MaxRasterValue should be > MinRasterValue\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"MinParameterValue < 2\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSpectrumParams(&Params{\n\t\t\tMinParameterValue: 1,\n\t\t\tMaxParameterValue: 30,\n\t\t})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona spectrum parameters: MinParameterValue should be >= 2\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"MaxParameterValue <= MinParameterValue\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSpectrumParams(&Params{\n\t\t\tMinParameterValue: 20,\n\t\t\tMaxParameterValue: 20,\n\t\t})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona spectrum parameters: MaxParameterValue should be > MinParameterValue\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"HighPassFilterCutoff < 2\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSpectrumParams(&Params{\n\t\t\tHighPassFilterCutoff: 1,\n\t\t})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona spectrum parameters: HighPassFilterCutoff should be >= 2\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"invalid bar component\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSpectrumParams(&Params{\n\t\t\tBarComponent: entities.BarComponent(9999),\n\t\t})\n\t\tif err == nil {\n\t\t\tt.Error(\"expected error\")\n\t\t}\n\t})\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrum_test.go"
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

## Assistant (Build · claude-opus-4.7 · 39.6s)

**Tool: bash**

**Input:**
```json
{
  "command": "rm -f /tmp/cs_snapshot.go /tmp/cs_snapshot2.go; go test ./indicators/johnehlers/coronaspectrum/... -v 2>&1 | tail -80",
  "description": "Run coronaspectrum tests verbose",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
=== RUN   TestCoronaSpectrumMetadata
=== PAUSE TestCoronaSpectrumMetadata
=== RUN   TestCoronaSpectrumUpdateEntity
=== PAUSE TestCoronaSpectrumUpdateEntity
=== RUN   TestNewCoronaSpectrum
=== PAUSE TestNewCoronaSpectrum
=== RUN   TestOutputString
=== PAUSE TestOutputString
=== RUN   TestOutputIsKnown
=== PAUSE TestOutputIsKnown
=== RUN   TestOutputMarshalJSON
=== PAUSE TestOutputMarshalJSON
=== RUN   TestOutputUnmarshalJSON
=== PAUSE TestOutputUnmarshalJSON
=== CONT  TestCoronaSpectrumUpdate
=== CONT  TestNewCoronaSpectrum
=== CONT  TestCoronaSpectrumMetadata
=== RUN   TestNewCoronaSpectrum/default
=== CONT  TestOutputIsKnown
=== CONT  TestOutputString
=== PAUSE TestNewCoronaSpectrum/default
=== CONT  TestCoronaSpectrumUpdateEntity
=== RUN   TestCoronaSpectrumUpdateEntity/update_scalar
=== RUN   TestNewCoronaSpectrum/custom_ranges_round_to_integers
=== PAUSE TestNewCoronaSpectrum/custom_ranges_round_to_integers
=== CONT  TestCoronaSpectrumNaNInput
=== CONT  TestCoronaSpectrumPrimesAtBar11
=== CONT  TestOutputMarshalJSON
=== CONT  TestOutputUnmarshalJSON
--- PASS: TestCoronaSpectrumMetadata (0.00s)
--- PASS: TestOutputIsKnown (0.00s)
--- PASS: TestOutputString (0.00s)
--- PASS: TestCoronaSpectrumNaNInput (0.00s)
--- PASS: TestOutputMarshalJSON (0.00s)
--- PASS: TestOutputUnmarshalJSON (0.00s)
--- PASS: TestCoronaSpectrumUpdate (0.00s)
=== PAUSE TestCoronaSpectrumUpdateEntity/update_scalar
=== RUN   TestCoronaSpectrumUpdateEntity/update_bar
=== PAUSE TestCoronaSpectrumUpdateEntity/update_bar
=== RUN   TestCoronaSpectrumUpdateEntity/update_quote
=== RUN   TestNewCoronaSpectrum/MaxRasterValue_<=_MinRasterValue
=== PAUSE TestCoronaSpectrumUpdateEntity/update_quote
=== PAUSE TestNewCoronaSpectrum/MaxRasterValue_<=_MinRasterValue
=== RUN   TestNewCoronaSpectrum/MinParameterValue_<_2
=== PAUSE TestNewCoronaSpectrum/MinParameterValue_<_2
=== RUN   TestCoronaSpectrumUpdateEntity/update_trade
=== PAUSE TestCoronaSpectrumUpdateEntity/update_trade
=== CONT  TestCoronaSpectrumUpdateEntity/update_scalar
=== CONT  TestCoronaSpectrumUpdateEntity/update_trade
=== RUN   TestNewCoronaSpectrum/MaxParameterValue_<=_MinParameterValue
=== PAUSE TestNewCoronaSpectrum/MaxParameterValue_<=_MinParameterValue
=== RUN   TestNewCoronaSpectrum/HighPassFilterCutoff_<_2
=== PAUSE TestNewCoronaSpectrum/HighPassFilterCutoff_<_2
=== CONT  TestCoronaSpectrumUpdateEntity/update_bar
=== CONT  TestCoronaSpectrumUpdateEntity/update_quote
=== RUN   TestNewCoronaSpectrum/invalid_bar_component
=== PAUSE TestNewCoronaSpectrum/invalid_bar_component
=== CONT  TestNewCoronaSpectrum/default
=== CONT  TestNewCoronaSpectrum/invalid_bar_component
=== CONT  TestNewCoronaSpectrum/HighPassFilterCutoff_<_2
=== CONT  TestNewCoronaSpectrum/custom_ranges_round_to_integers
=== CONT  TestNewCoronaSpectrum/MinParameterValue_<_2
--- PASS: TestCoronaSpectrumUpdateEntity (0.00s)
    --- PASS: TestCoronaSpectrumUpdateEntity/update_scalar (0.00s)
    --- PASS: TestCoronaSpectrumUpdateEntity/update_trade (0.00s)
    --- PASS: TestCoronaSpectrumUpdateEntity/update_bar (0.00s)
    --- PASS: TestCoronaSpectrumUpdateEntity/update_quote (0.00s)
--- PASS: TestCoronaSpectrumPrimesAtBar11 (0.00s)
=== CONT  TestNewCoronaSpectrum/MaxRasterValue_<=_MinRasterValue
=== CONT  TestNewCoronaSpectrum/MaxParameterValue_<=_MinParameterValue
--- PASS: TestNewCoronaSpectrum (0.00s)
    --- PASS: TestNewCoronaSpectrum/default (0.00s)
    --- PASS: TestNewCoronaSpectrum/invalid_bar_component (0.00s)
    --- PASS: TestNewCoronaSpectrum/HighPassFilterCutoff_<_2 (0.00s)
    --- PASS: TestNewCoronaSpectrum/custom_ranges_round_to_integers (0.00s)
    --- PASS: TestNewCoronaSpectrum/MinParameterValue_<_2 (0.00s)
    --- PASS: TestNewCoronaSpectrum/MaxRasterValue_<=_MinRasterValue (0.00s)
    --- PASS: TestNewCoronaSpectrum/MaxParameterValue_<=_MinParameterValue (0.00s)
PASS
ok  	zpano/indicators/johnehlers/coronaspectrum	0.005s

```

---

## User

---

## Assistant (Compaction · claude-opus-4.7 · 68.6s)

---
## Goal

Port MBST's **Ehlers Corona Charts suite** (TASC November 2008) from `/home/dev/zpano/mbst-to-convert/john-ehlers/corona/` into zpano:

- **Corona** shared helper (not a registered indicator)
- **CoronaSpectrum** — heatmap + DominantCycle + DominantCycleMedian scalars
- **CoronaSignalToNoiseRatio** — heatmap + SNR scalar (uses High/Low for noise)
- **CoronaSwingPosition** — heatmap + SwingPosition scalar
- **CoronaTrendVigor** — heatmap + TrendVigor scalar

Deliverables in Go (`go/indicators/johnehlers/<pkg>/`) then TypeScript (`ts/indicators/john-ehlers/<pkg>/`). These are the **first heatmap-producing indicators in zpano**.

## Instructions

### Design decisions (confirmed with user)
- Unprimed bars → emit `outputs.NewEmptyHeatmap(time, paramFirst, paramLast, paramRes)`; scalars are NaN (matches TCM).
- Mnemonic style → **zpano convention**: `cspect(…)`, `cspect-dc(…)`, `cspect-dcm(…)`, `csnr(…)`, `cswing(…)`, `ctv(…)`.
- All 4 `core.Type` constants registered (done).
- After all 4 Go indicators are green + tested → **pause for user review** before TS port.

### Architecture
- Corona is a helper package (like `hilberttransformer/`); each of the 4 indicators owns its own Corona instance.
- Follow `trendcyclemode/` as the multi-output indicator template.

### Algorithm decisions from 3-way reference cross-validation
- **Swing BP2 momentum**: 2-bar (`sample - samplePrev2`) — MBST/WealthLab/NinjaTrader. eSignal 1-bar is a bug.
- **TrendVigor lookback**: FIX the MBST bug — use `DomCycMedian - 1` directly.
- **SNR signal EMA coefficients**: Keep 0.2/0.9 (sums to 1.1) — Ehlers' intentional code.
- **DC weighted average**: N=12..60 indexing.
- **DominantCycleMedian clamp**: Apply `< minPeriod` floor to the median, not to raw DC.

### Defaults
- Corona: `HighPassFilterCutoff=30, MinimalPeriod=6, MaximalPeriod=30, DecibelsLowerThreshold=6, DecibelsUpperThreshold=20`
- CSpectrum: `minRaster=6, maxRaster=20, minParam=6, maxParam=30, hpCutoff=30, BarComponent=Median`
- CSNR: `rasterLength=50, maxRaster=20, minParam=1, maxParam=11, BarComponent=Median`
- CSwing: `rasterLength=50, maxRaster=20, minParam=-5, maxParam=5`
- CTV: `rasterLength=50, maxRaster=20, minParam=-10, maxParam=10`

### Test strategy
- Reuse 252-entry TA-Lib MAMA data (copied locally into each package's test file).
- Tolerance 1e-4.
- Standard tests: metadata, mnemonics, param validation, NaN no-op, numeric snapshots.

## Discoveries

### Corona helper bug fixes (vs initial port)
1. `MaximalAmplitudeSquared` must reset to 0 per-bar.
2. First sample is store-only (MBST `if (1 == ++sampleCount)` short-circuit).
3. `DominantCycle` must reset to 0 each bar before the weighted-average loop.
4. `dominantCycleBuffer` must be initialized to `math.MaxFloat64` sentinels.

### Heatmap conventions
- `ParameterResolution = (FilterBankLength - 1) / (maxParam - minParam)` so `min + i/res = max`. For defaults: `(49-1)/(30-6) = 2.0` (two samples per period, half-period stepping).
- Heatmap axis is emitted even on unprimed bars; only `Values` slice is empty and `ValueMin`/`ValueMax` are NaN.

### CoronaSpectrum snapshot values (captured, now locked in tests)
With TA-Lib input, default params: primes at bar 11 (MinimalPeriodTimesTwo=12). DC samples at indices 11/12/50/100/150/200/251: `17.7605, 6.0, 15.9989, 14.7455, 17.5, 19.7557, 6.0`. DCM samples: `17.7605, 6.0, 15.9989, 14.7455, 17.2826, 20.0, 6.0`. Last heatmap: 49 values, vmin=8.12, vmax=19.94.

### zpano infrastructure
- `outputs.NewHeatmap(time, parameterFirst, parameterLast, parameterResolution, valueMin, valueMax, values)` / `outputs.NewEmptyHeatmap(...)`.
- `core.ComponentTripleMnemonic(bc, qc, tc)` composes the suffix (omits components at their defaults).
- TCM pattern: separate `*output.go` (Output enum + JSON), `*params.go`, main file with `Update(sample) (…)` tuple + `UpdateScalar/Bar/Quote/Trade` returning `core.Output` slice.
- TCM uses `entities.BarMedianPrice` as default when `bc == 0` (Ehlers convention). Corona indicators follow same pattern.

## Accomplished

### ✅ Completed
1. Full analysis of MBST + 3 reference impls + bug cross-validation.
2. Implementation plan (Phases 0–6).
3. **Phase 1: Corona helper package** — done, 3 tests pass.
4. **4 new `core.Type` constants** registered (`CoronaSpectrum`, `CoronaSignalToNoiseRatio`, `CoronaSwingPosition`, `CoronaTrendVigor`) + JSON round-trip + type_test.go updated.
5. **Phase 2: CoronaSpectrum — DONE, all tests pass:**
   - `coronaspectrumparams.go`
   - `coronaspectrumoutput.go` + `coronaspectrumoutput_test.go` (4-table tests)
   - `coronaspectrum.go` — main indicator (owns `*corona.Corona`; 3 outputs; mnemonic `cspect(6, 20, 6, 30, 30, hl/2)`; emits heatmap axis even when unprimed)
   - `coronaspectrum_test.go` — numeric snapshot, prime-at-11, NaN no-op, Metadata, UpdateScalar/Bar/Quote/Trade, 7 param-validation sub-tests
   - `go test ./indicators/johnehlers/coronaspectrum/...` ✅ PASS

### 🔄 Next up (Phase 3: CoronaSignalToNoiseRatio)
Implementation notes for CSNR (takes full OHLCV — noise = (High-Low)² smoothed):
- Params should include `RasterLength` (default 50), `MaxRasterValue` (20), `MinParameterValue` (1), `MaxParameterValue` (11).
- Two outputs: heatmap `Value` + scalar `SignalToNoiseRatio`.
- Mnemonic: `csnr(rasterLength, maxRaster, minParam, maxParam, hpCutoff, componentTriple)` + `csnr-snr(...)`.
- SNR uses **intentional** 0.2/0.9 EMA coefficients (sum = 1.1).
- For `UpdateScalar` where H/L unavailable: use sample value as both High and Low ⇒ noise = 0; document this. Alternatively return NaN. **Decision pending** — probably sample-as-price with NaN SNR until bar-native path.

### ⏳ Remaining
- Phase 3: CoronaSignalToNoiseRatio
- Phase 4: CoronaSwingPosition
- Phase 5: CoronaTrendVigor (apply DomCyc-1 lookback fix)
- **Pause for user review.**
- Phase 6: TS port of all 4 to `ts/indicators/john-ehlers/<pkg>/`.
- Update `.opencode/skills/mbst-indicator-conversion/SKILL.md` with Corona learnings.

## Relevant files / directories

### Created / modified this task
- `/home/dev/zpano/go/indicators/johnehlers/corona/` — helper package (doc.go, params.go, corona.go, corona_smoke_test.go) ✅
- `/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/` — ✅ COMPLETE, all tests pass:
  - `coronaspectrumparams.go`
  - `coronaspectrumoutput.go`
  - `coronaspectrumoutput_test.go`
  - `coronaspectrum.go`
  - `coronaspectrum_test.go` (contains `testCSInput()` 252-entry TA-Lib series)
- `/home/dev/zpano/go/indicators/core/type.go` — 4 new constants + String/IsKnown/MarshalJSON/UnmarshalJSON
- `/home/dev/zpano/go/indicators/core/type_test.go` — 4 tables extended

### MBST source to port (remaining)
- `/home/dev/zpano/mbst-to-convert/john-ehlers/corona/CoronaSignalToNoiseRatio.cs`
- `/home/dev/zpano/mbst-to-convert/john-ehlers/corona/CoronaSwingPosition.cs`
- `/home/dev/zpano/mbst-to-convert/john-ehlers/corona/CoronaTrendVigor.cs`
- Reference impls: `e-signal-*.efs`, `ninja-trader-*.cs`, `wealth-lab-implementation.cs`

### Reference templates in zpano
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/` — **primary template** for multi-output indicator
- `/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/` — **NEW template** for heatmap+scalars pattern (mirror for CSNR/CSwing/CTV)
- `/home/dev/zpano/go/indicators/core/outputs/heatmap.go` — Heatmap output type
- `/home/dev/zpano/go/indicators/core/componenttriplemnemonic.go` — mnemonic suffix helper

### Skills
- `.opencode/skills/indicator-architecture` — loaded
- `.opencode/skills/mbst-indicator-conversion` — loaded; to be updated after all phases complete

### Immediate next action
Start **Phase 3: CoronaSignalToNoiseRatio**. Create `go/indicators/johnehlers/coronasignaltonoiseratio/` with the 5-file pattern mirroring `coronaspectrum/`. Key differences: 2 outputs instead of 3 (Value heatmap + SignalToNoiseRatio scalar), requires High/Low access (design the bar path first; scalar path uses sample-as-price with noise=0 OR returns NaN SNR — confirm with user if unclear). Verify with `cd /home/dev/zpano/go && go test ./indicators/johnehlers/coronasignaltonoiseratio/...`.

---

## User

---

## Assistant (Build · claude-opus-4.7 · 4.7s)

_Thinking:_

 Now I'm moving on to Phase 3 and need to examine the MBST source code along with the existing coronaspectrum template to understand how to implement the CoronaSignalToNoiseRatio calculation.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/mbst-to-convert/john-ehlers/corona/CoronaSignalToNoiseRatio.cs"
}
```

**Output:**
```
<path>/home/dev/zpano/mbst-to-convert/john-ehlers/corona/CoronaSignalToNoiseRatio.cs</path>
<type>file</type>
<content>1: ﻿using System;
2: using Mbs.Trading.Data;
3: using Mbs.Trading.Indicators.Abstractions;
4: 
5: // ReSharper disable once CheckNamespace
6: namespace Mbs.Trading.Indicators.JohnEhlers
7: {
8:     /// <summary>
9:     /// The Corona Signal to Noise Ratio is a measure of the cycle amplitude relative to noise.
10:     /// <para />
11:     /// The "noise" is chosen to be the average bar height because there is not much trade information within the bar.
12:     /// <para />
13:     /// The vertical scale of the indicator is from 1 dB to 11 dB. The Signal to Noise Ratio starts
14:     /// to develop a “corona” below 4 dB, warning you that the ratio may become too low to safely
15:     /// swing trade on the basis of the cycle alone.
16:     /// </summary>
17:     public sealed class CoronaSignalToNoiseRatio : Indicator, IHeatMapIndicator
18:     {
19:         #region Members and accessors
20:         private const int HighLowMedianIndex = 2;
21:         private const int HighLowBufferSize = 5;
22:         private const int HighLowBufferSizeMinOne = HighLowBufferSize - 1;
23: 
24:         private readonly int rasterLength;
25:         private readonly double rasterStep;
26:         private bool isStarted;
27:         private readonly double[] highLowBuffer = new double[HighLowBufferSize];
28:         private double noisePrevious;
29:         private double averageSamplePrevious;
30:         private double signalPrevious;
31: 
32:         /// <summary>
33:         /// Serialization is not needed.
34:         /// </summary>
35:         private readonly double[] highLowBufferSorted = new double[HighLowBufferSize];
36: 
37:         /// <summary>
38:         /// The maximal raster value of the heat-map.
39:         /// </summary>
40:         private readonly double maxRasterValue;
41: 
42:         /// <summary>
43:         /// The middle raster value of the heat-map.
44:         /// </summary>
45:         private readonly double midRasterValue;
46: 
47:         /// <summary>
48:         /// The heat-map scaled to [0, maxRasterValue].
49:         /// </summary>
50:         private readonly double[] raster;
51: 
52:         /// <summary>
53:         /// The minimum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns.
54:         /// </summary>
55:         public double MinParameterValue { get; }
56: 
57:         /// <summary>
58:         /// The maximum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns.
59:         /// </summary>
60:         public double MaxParameterValue { get; }
61: 
62:         private double signalToNoiseRatio = double.NaN;
63: 
64:         /// <summary>
65:         /// The current value of the Corona signal to noise ratio in the range of [MinParameterValue, MaxParameterValue] or <c>NaN</c> if not primed.
66:         /// </summary>
67:         public double SignalToNoiseRatio { get { lock (Lock) { return Primed ? signalToNoiseRatio : double.NaN; } } }
68: 
69:         /// <summary>
70:         /// A line indicator façade to expose a value of the SignalToNoiseRatio as a separate line indicator with a fictitious update.
71:         /// <para />
72:         /// The line indicator façade do not perform actual updates. It just gets a current value of the SignalToNoiseRatio from this instance, assuming it is already updated.
73:         /// <para />
74:         /// Resetting the line indicator façade has no effect. This instance should be reset instead.
75:         /// </summary>
76:         public LineIndicatorFacade SignalToNoiseRatioFacade => new LineIndicatorFacade(CsnrName, CsnrMoniker, CsnrDescription, () => IsPrimed, () => SignalToNoiseRatio);
77: 
78:         private readonly bool isSlave;
79:         private readonly Corona corona;
80: 
81:         private const string CsnrHeatMapName = "CoronaSignalToNoiseRatioHeatmap";
82:         private const string CsnrHeatMapDescription = "Crona signal to noise ratio heatmap";
83:         private const string CsnrHeatMapMoniker = "CoronaSNR(h)";
84: 
85:         private const string CsnrName = "CoronaSignalToNoiseRatio";
86:         private const string CsnrDescription = "Crona signal to noise ratio";
87:         private const string CsnrMoniker = "CoronaSNR";
88:         #endregion
89: 
90:         #region Construction
91:         /// <summary>
92:         /// Constructs a new instance of the class.
93:         /// </summary>
94:         /// <param name="rasterLength">The number of elements in the heat-map raster. The default value is 50.</param>
95:         /// <param name="maxRasterValue">The maximal raster value of the heat-map. The default value is 20.</param>
96:         /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns. The default value is 1.</param>
97:         /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns. The default value is 11.</param>
98:         /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
99:         /// <param name="highPassFilterCutoff">The high-pass filter cutoff (de-trending period). Suggested values are 20, 30, 100.</param>
100:         /// <param name="minimalPeriod">The minimal period. The default value is 6.</param>
101:         /// <param name="maximalPeriod">The maximal period. The default value is 30.</param>
102:         public CoronaSignalToNoiseRatio(int rasterLength = 50, double maxRasterValue = 20, double minParameterValue = 1, double maxParameterValue = 11,
103:             OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice,
104:             int highPassFilterCutoff = 30, int minimalPeriod = 6, int maximalPeriod = 30)
105:             : this(null, rasterLength, maxRasterValue,
106:             minParameterValue, maxParameterValue, ohlcvComponent, highPassFilterCutoff, minimalPeriod, maximalPeriod)
107:         {
108:         }
109: 
110:         /// <summary>
111:         /// Constructs a new instance of the class.
112:         /// </summary>
113:         /// <param name="coronaSpectrum">The parent <c>CoronaSpectrum</c> instance to re-use the corona engine.</param>
114:         /// <param name="rasterLength">The number of elements in the heat-map raster. The default value is 50.</param>
115:         /// <param name="maxRasterValue">The maximal raster value of the heat-map. The default value is 20.</param>
116:         /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns. The default value is 1.</param>
117:         /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns. The default value is 11.</param>
118:         /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
119:         public CoronaSignalToNoiseRatio(CoronaSpectrum coronaSpectrum, int rasterLength = 50, double maxRasterValue = 20, double minParameterValue = 1, double maxParameterValue = 11,
120:             OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
121:             : this(coronaSpectrum.Corona, rasterLength, maxRasterValue, minParameterValue, maxParameterValue, ohlcvComponent, 30, 6, 30)
122:         {
123:         }
124: 
125:         private CoronaSignalToNoiseRatio(Corona corona, int rasterLength, double maxRasterValue, double minParameterValue, double maxParameterValue,
126:             OhlcvComponent ohlcvComponent, int highPassFilterCutoff, int minimalPeriod, int maximalPeriod)
127:             : base(CsnrHeatMapName, CsnrHeatMapDescription, ohlcvComponent)
128:         {
129:             Moniker = CsnrHeatMapMoniker;
130:             if (null == corona)
131:                 this.corona = new Corona(highPassFilterCutoff, minimalPeriod, maximalPeriod);
132:             else
133:             {
134:                 this.corona = corona;
135:                 isSlave = true;
136:             }
137:             this.rasterLength = rasterLength;
138:             this.maxRasterValue = maxRasterValue;
139:             MinParameterValue = minParameterValue;
140:             MaxParameterValue = maxParameterValue;
141:             midRasterValue = maxRasterValue / 2;
142:             rasterStep = maxRasterValue / rasterLength;
143:             raster = new double[rasterLength];
144:             for (int i = 0; i < rasterLength; ++i)
145:                 raster[i] = 0;
146:         }
147:         #endregion
148: 
149:         #region Reset
150:         /// <inheritdoc />
151:         public override void Reset()
152:         {
153:             lock (Lock)
154:             {
155:                 Primed = false;
156:                 isStarted = false;
157:                 averageSamplePrevious = 0;
158:                 signalPrevious = 0;
159:                 noisePrevious = 0;
160:                 signalToNoiseRatio = double.NaN;
161:                 for (int i = 0; i < rasterLength; ++i)
162:                     raster[i] = 0;
163:                 if (!isSlave)
164:                     corona.Reset();
165:             }
166:         }
167:         #endregion
168: 
169:         #region Update
170:         private HeatMap Update(double sample, double sampleLow, double sampleHigh, DateTime dateTime)
171:         {
172:             if (double.IsNaN(sample))
173:                 return null;
174:             Primed = isSlave ? corona.IsPrimed : corona.Update(sample);
175:             if (!isStarted)
176:             {
177:                 averageSamplePrevious = sample;
178:                 highLowBuffer[HighLowBufferSizeMinOne] = sampleHigh - sampleLow;
179:                 isStarted = true;
180:                 return null;
181:             }
182:             double maxAmplitudeSquared = corona.MaximalAmplitudeSquared;
183: 
184:             double averageSample = 0.1 * sample + 0.9 * averageSamplePrevious;
185:             averageSamplePrevious = averageSample;
186:             if (Math.Abs(averageSample) > double.Epsilon || maxAmplitudeSquared > 0)
187:                 signalPrevious = 0.2 * Math.Sqrt(maxAmplitudeSquared) + 0.9 * signalPrevious;
188:             for (int i = 0; i < HighLowBufferSizeMinOne; )
189:                 highLowBuffer[i] = highLowBuffer[++i];
190:             highLowBuffer[HighLowBufferSizeMinOne] = sampleHigh - sampleLow;
191:             double ratio = 0;
192:             if (Math.Abs(averageSample) > double.Epsilon)
193:             {
194:                 for (int i = 0; i < HighLowBufferSize; ++i)
195:                     highLowBufferSorted[i] = highLowBuffer[i];
196:                 Array.Sort(highLowBufferSorted);
197:                 noisePrevious = 0.1 * highLowBufferSorted[HighLowMedianIndex] + 0.9 * noisePrevious;
198:                 if (Math.Abs(noisePrevious) > double.Epsilon)
199:                 {
200:                     ratio = 20 * Math.Log10(signalPrevious / noisePrevious) + 3.5;
201:                     if (ratio < 0)// <1
202:                         ratio = 0;
203:                     else if (ratio > 10)
204:                         ratio = 10;
205:                     // ∈ [0, 1].
206:                     ratio /= 10;
207:                 }
208:             }
209:             signalToNoiseRatio = (MaxParameterValue - MinParameterValue) * ratio + MinParameterValue;
210:             // width ∈ [0, 0.2].
211:             double width = ratio > 0.5 ? 0 : (0.2 - 0.4 * ratio);
212:             var ratioScaledToRasterLength = (int)Math.Round(ratio * rasterLength);
213:             double ratioScaledToMaxRasterValue = ratio * maxRasterValue;
214:             for (int i = 0; i < rasterLength; ++i)
215:             //Parallel.For(0, rasterLength, i =>
216:             {
217:                 double value = raster[i];
218:                 if (i == ratioScaledToRasterLength)
219:                     value *= 0.5;
220:                 else
221:                 {
222:                     double argument = ratioScaledToMaxRasterValue - rasterStep * i;
223:                     argument /= width;
224:                     if (i < ratioScaledToRasterLength)
225:                         value = 0.5 * (Math.Pow(argument, 0.8) + value);
226:                     else
227:                     {
228:                         argument = -argument;
229:                         if (argument > 1)
230:                             value = 0.5 * (Math.Pow(argument, 0.8) + value);
231:                         else
232:                             value = maxRasterValue;
233:                     }
234:                 }
235:                 if (value < 0)
236:                     value = 0;
237:                 else if (value > maxRasterValue)
238:                     value = maxRasterValue;
239:                 if (ratio > 0.5)
240:                     value = maxRasterValue;
241:                 raster[i] = value;
242:             }//);
243: 
244:             double[] intensity;
245:             if (Primed)
246:             {
247:                 intensity = new double[rasterLength];//+1
248:                 // Min raster value is zero.
249:                 for (int i = 0; i < rasterLength; ++i)
250:                 {
251:                     double value = raster[i];
252:                     intensity[i] = value;
253:                 }
254:             }
255:             else
256:             {
257:                 intensity = null;
258:             }
259:             return new HeatMap(dateTime, intensity);
260:         }
261: 
262:         /// <summary>
263:         /// Updates the value of the indicator.
264:         /// </summary>
265:         /// <param name="ohlcv">A new ohlcv.</param>
266:         /// <returns>The new heat-map column of the indicator or <c>null</c>.</returns>
267:         public HeatMap Update(Ohlcv ohlcv)
268:         {
269:             lock (Lock)
270:             {
271:                 return Update(ohlcv.Component(OhlcvComponent), ohlcv.Low, ohlcv.High, ohlcv.Time);
272:             }
273:         }
274: 
275:         /// <summary>
276:         /// Updates the value of the indicator.
277:         /// </summary>
278:         /// <param name="scalar">A new scalar.</param>
279:         /// <returns>The new heat-map column of the indicator or <c>null</c>.</returns>
280:         public HeatMap Update(Scalar scalar)
281:         {
282:             lock (Lock)
283:             {
284:                 double value = scalar.Value;
285:                 return Update(value, value, value, scalar.Time);
286:             }
287:         }
288:         #endregion
289:     }
290: }

(End of file - total 290 lines)
</content>
```

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

## Assistant (Build · claude-opus-4.7 · 7.6s)

_Thinking:_

 I'm checking the existing helper files for the spectrum module — looking at the params, output, and test files to understand the current structure.

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

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrumoutput_test.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/coronaspectrumoutput_test.go</path>
<type>file</type>
<content>1: //nolint:testpackage
2: package coronaspectrum
3: 
4: import (
5: 	"testing"
6: )
7: 
8: func TestOutputString(t *testing.T) {
9: 	t.Parallel()
10: 
11: 	tests := []struct {
12: 		o    Output
13: 		text string
14: 	}{
15: 		{Value, valueStr},
16: 		{DominantCycle, dominantCycleStr},
17: 		{DominantCycleMedian, dominantCycleMedianStr},
18: 		{outputLast, unknownStr},
19: 		{Output(0), unknownStr},
20: 		{Output(9999), unknownStr},
21: 		{Output(-9999), unknownStr},
22: 	}
23: 
24: 	for _, tt := range tests {
25: 		exp := tt.text
26: 		act := tt.o.String()
27: 
28: 		if exp != act {
29: 			t.Errorf("'%v'.String(): expected '%v', actual '%v'", tt.o, exp, act)
30: 		}
31: 	}
32: }
33: 
34: func TestOutputIsKnown(t *testing.T) {
35: 	t.Parallel()
36: 
37: 	tests := []struct {
38: 		o       Output
39: 		boolean bool
40: 	}{
41: 		{Value, true},
42: 		{DominantCycle, true},
43: 		{DominantCycleMedian, true},
44: 		{outputLast, false},
45: 		{Output(0), false},
46: 		{Output(9999), false},
47: 		{Output(-9999), false},
48: 	}
49: 
50: 	for _, tt := range tests {
51: 		exp := tt.boolean
52: 		act := tt.o.IsKnown()
53: 
54: 		if exp != act {
55: 			t.Errorf("'%v'.IsKnown(): expected '%v', actual '%v'", tt.o, exp, act)
56: 		}
57: 	}
58: }
59: 
60: func TestOutputMarshalJSON(t *testing.T) {
61: 	t.Parallel()
62: 
63: 	const dqs = "\""
64: 
65: 	var nilstr string
66: 	tests := []struct {
67: 		o         Output
68: 		json      string
69: 		succeeded bool
70: 	}{
71: 		{Value, dqs + valueStr + dqs, true},
72: 		{DominantCycle, dqs + dominantCycleStr + dqs, true},
73: 		{DominantCycleMedian, dqs + dominantCycleMedianStr + dqs, true},
74: 		{outputLast, nilstr, false},
75: 		{Output(9999), nilstr, false},
76: 		{Output(-9999), nilstr, false},
77: 		{Output(0), nilstr, false},
78: 	}
79: 
80: 	for _, tt := range tests {
81: 		exp := tt.json
82: 		bs, err := tt.o.MarshalJSON()
83: 
84: 		if err != nil && tt.succeeded {
85: 			t.Errorf("'%v'.MarshalJSON(): expected success '%v', got error %v", tt.o, exp, err)
86: 
87: 			continue
88: 		}
89: 
90: 		if err == nil && !tt.succeeded {
91: 			t.Errorf("'%v'.MarshalJSON(): expected error, got success", tt.o)
92: 
93: 			continue
94: 		}
95: 
96: 		act := string(bs)
97: 		if exp != act {
98: 			t.Errorf("'%v'.MarshalJSON(): expected '%v', actual '%v'", tt.o, exp, act)
99: 		}
100: 	}
101: }
102: 
103: func TestOutputUnmarshalJSON(t *testing.T) {
104: 	t.Parallel()
105: 
106: 	const dqs = "\""
107: 
108: 	var zero Output
109: 	tests := []struct {
110: 		o         Output
111: 		json      string
112: 		succeeded bool
113: 	}{
114: 		{Value, dqs + valueStr + dqs, true},
115: 		{DominantCycle, dqs + dominantCycleStr + dqs, true},
116: 		{DominantCycleMedian, dqs + dominantCycleMedianStr + dqs, true},
117: 		{zero, dqs + unknownStr + dqs, false},
118: 		{zero, dqs + "foobar" + dqs, false},
119: 	}
120: 
121: 	for _, tt := range tests {
122: 		exp := tt.o
123: 		bs := []byte(tt.json)
124: 
125: 		var o Output
126: 
127: 		err := o.UnmarshalJSON(bs)
128: 		if err != nil && tt.succeeded {
129: 			t.Errorf("UnmarshalJSON('%v'): expected success '%v', got error %v", tt.json, exp, err)
130: 
131: 			continue
132: 		}
133: 
134: 		if err == nil && !tt.succeeded {
135: 			t.Errorf("MarshalJSON('%v'): expected error, got success", tt.json)
136: 
137: 			continue
138: 		}
139: 
140: 		if exp != o {
141: 			t.Errorf("MarshalJSON('%v'): expected '%v', actual '%v'", tt.json, exp, o)
142: 		}
143: 	}
144: }

(End of file - total 144 lines)
</content>
```

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

## Assistant (Build · claude-opus-4.7 · 5.5s)

_Thinking:_

 I need to examine the coronaspectrum test file to understand how the tests are structured and where the 252-entry TA-Lib dataset comes from, plus figure out the source of the High and Low values.

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

## Assistant (Build · claude-opus-4.7 · 15.2s)

_Thinking:_

 I'm seeing the pattern now—I need to implement CoronaSignalToNoiseRatio with two outputs: a heatmap value and a scalar ratio. The noise calculation uses the high-low spread, but for scalar updates where there's no range, I'll treat the sample as both bounds so noise becomes zero. I'll set up the parameters with rasterLength at 50, maxRasterValue at 20, minParam at 1, maxParam at 11, and hpCutoff at 30.

Now I'm working through the heatmap structure itself. It's a raster indicator where each of the 50 elements maps to an intensity value between 0 and maxRasterValue. The parameter axis spans from minParam to maxParam (1 to 11), and I need to map the ratio from 0 to 1 across the raster length. Looking at the MBST implementation, the rasterStep is maxRasterValue divided by rasterLength (0.4 in this case), and the ratio gets scaled to an index by multiplying by rasterLength and rounding. The heatmap Values array holds those 50 intensity samples, with the parameter axis linearly distributed across them.

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
33: 	ParameterResolution float64 `json:"res"`
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

**Tool: grep**

**Input:**
```json
{
  "pattern": "ComponentTripleMnemonic",
  "path": "/home/dev/zpano/go/indicators/core"
}
```

**Output:**
```
Found 2 matches
/home/dev/zpano/go/indicators/core/componenttriplemnemonic.go:
  Line 20: // ComponentTripleMnemonic builds a mnemonic suffix string from bar, quote and trade
  Line 25: func ComponentTripleMnemonic(
```

---

## Assistant (Build · claude-opus-4.7 · 4.5s)

_Thinking:_

 I'm checking whether `CoronaSignalToNoiseRatio` is already registered in `core.Type` based on the task status.

**Tool: grep**

**Input:**
```json
{
  "path": "/home/dev/zpano/go/indicators/core/type.go",
  "pattern": "Corona"
}
```

**Output:**
```
Found 16 matches
/home/dev/zpano/go/indicators/core/type.go:
  Line 201: 	// CoronaSpectrum identifies the Ehlers Corona Spectrum (CSPECT) indicator, a heat-map of cyclic activity
  Line 203: 	CoronaSpectrum
  Line 205: 	// CoronaSignalToNoiseRatio identifies the Ehlers Corona Signal-to-Noise Ratio (CSNR) indicator, a heat-map
  Line 207: 	CoronaSignalToNoiseRatio
  Line 209: 	// CoronaSwingPosition identifies the Ehlers Corona Swing Position (CSWING) indicator, a heat-map of swing
  Line 211: 	CoronaSwingPosition
  Line 213: 	// CoronaTrendVigor identifies the Ehlers Corona Trend Vigor (CTV) indicator, a heat-map of trend vigor
  Line 215: 	CoronaTrendVigor
  Line 418: 	case CoronaSpectrum:
  Line 420: 	case CoronaSignalToNoiseRatio:
  Line 422: 	case CoronaSwingPosition:
  Line 424: 	case CoronaTrendVigor:
  Line 595: 		*t = CoronaSpectrum
  Line 597: 		*t = CoronaSignalToNoiseRatio
  Line 599: 		*t = CoronaSwingPosition
  Line 601: 		*t = CoronaTrendVigor
```

---

## Assistant (Build · claude-opus-4.7 · 20.5s)

_Thinking:_

 Now I'm implementing the CSNR files with a specific design pattern: for scalar updates, the sample serves double duty for both H and L values, which keeps the noise at zero and maintains a low ratio while still updating the raster. For the SignalToNoiseRatio scalar output, when the ratio hits zero it returns the minimum parameter value when primed, but NaN when unprimed. Let me check how MBST handles this...

The calculation uses the median of five H-L differences to compute noise, so if all differences are zero the noise stays zero and the ratio becomes zero, mapping to the minimum parameter value. For the raster heatmap axis with a default range of 1 to 11 and 50 raster positions, the resolution works out to 4.9, which correctly maps the parameter space across all 50 elements.

Implementing the CSNR tests now...

**Tool: bash**

**Input:**
```json
{
  "command": "mkdir -p /home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio",
  "description": "Create CSNR package directory"
}
```

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/core/componenttriplemnemonic.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/core/componenttriplemnemonic.go</path>
<type>file</type>
<content>1: package core
2: 
3: import "zpano/entities"
4: 
5: // BarComponentMnemonic returns a short mnemonic for the given bar component.
6: func BarComponentMnemonic(c entities.BarComponent) string {
7: 	return c.Mnemonic()
8: }
9: 
10: // QuoteComponentMnemonic returns a short mnemonic for the given quote component.
11: func QuoteComponentMnemonic(c entities.QuoteComponent) string {
12: 	return c.Mnemonic()
13: }
14: 
15: // TradeComponentMnemonic returns a short mnemonic for the given trade component.
16: func TradeComponentMnemonic(c entities.TradeComponent) string {
17: 	return c.Mnemonic()
18: }
19: 
20: // ComponentTripleMnemonic builds a mnemonic suffix string from bar, quote and trade
21: // components. A component equal to its default value is omitted from the mnemonic.
22: //
23: // For example, if bar component is BarMedianPrice (non-default), the result is ", hl/2".
24: // If bar component is BarClosePrice (default), it is omitted.
25: func ComponentTripleMnemonic(
26: 	bc entities.BarComponent, qc entities.QuoteComponent, tc entities.TradeComponent,
27: ) string {
28: 	var s string
29: 
30: 	if bc != entities.DefaultBarComponent {
31: 		s += ", " + bc.Mnemonic()
32: 	}
33: 
34: 	if qc != entities.DefaultQuoteComponent {
35: 		s += ", " + qc.Mnemonic()
36: 	}
37: 
38: 	if tc != entities.DefaultTradeComponent {
39: 		s += ", " + tc.Mnemonic()
40: 	}
41: 
42: 	return s
43: }

(End of file - total 43 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 32.8s)

Now I'll create the 5 CSNR files.

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratioparams.go",
  "content": "package coronasignaltonoiseratio\n\nimport \"zpano/entities\"\n\n// Params describes parameters to create an instance of the CoronaSignalToNoiseRatio indicator.\ntype Params struct {\n\t// RasterLength is the number of elements in the heatmap raster. The default value is 50.\n\tRasterLength int\n\n\t// MaxRasterValue is the maximal raster value (z) of the heatmap. The default value is 20.\n\tMaxRasterValue float64\n\n\t// MinParameterValue is the minimal ordinate (y) value of the heatmap. The default value is 1.\n\tMinParameterValue float64\n\n\t// MaxParameterValue is the maximal ordinate (y) value of the heatmap. The default value is 11.\n\tMaxParameterValue float64\n\n\t// HighPassFilterCutoff is the high-pass filter cutoff (de-trending period) used by\n\t// the inner Corona engine. Suggested values are 20, 30, 100. The default value is 30.\n\tHighPassFilterCutoff int\n\n\t// MinimalPeriod is the minimal period of the inner Corona engine. The default value is 6.\n\tMinimalPeriod int\n\n\t// MaximalPeriod is the maximal period of the inner Corona engine. The default value is 30.\n\tMaximalPeriod int\n\n\t// BarComponent indicates the component of a bar to use when updating the indicator with a bar sample.\n\t//\n\t// If zero, the default (BarMedianPrice) is used, matching Ehlers' reference which operates on\n\t// (High+Low)/2. Since this differs from the framework-wide default, it is always shown in the\n\t// indicator mnemonic.\n\tBarComponent entities.BarComponent\n\n\t// QuoteComponent indicates the component of a quote to use when updating the indicator with a quote sample.\n\t//\n\t// If zero, the default (QuoteMidPrice) is used and the component is not shown in the indicator mnemonic.\n\tQuoteComponent entities.QuoteComponent\n\n\t// TradeComponent indicates the component of a trade to use when updating the indicator with a trade sample.\n\t//\n\t// If zero, the default (TradePrice) is used and the component is not shown in the indicator mnemonic.\n\tTradeComponent entities.TradeComponent\n}\n"
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
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratiooutput.go",
  "content": "package coronasignaltonoiseratio\n\nimport (\n\t\"bytes\"\n\t\"fmt\"\n)\n\n// Output describes the outputs of the indicator.\ntype Output int\n\nconst (\n\t// Value is the Corona signal-to-noise ratio heatmap column.\n\tValue Output = iota + 1\n\t// SignalToNoiseRatio is the scalar signal-to-noise ratio value in the range of [MinParameterValue, MaxParameterValue].\n\tSignalToNoiseRatio\n\toutputLast\n)\n\nconst (\n\tvalueStr              = \"value\"\n\tsignalToNoiseRatioStr = \"signalToNoiseRatio\"\n\tunknownStr            = \"unknown\"\n)\n\n// String implements the Stringer interface.\nfunc (o Output) String() string {\n\tswitch o {\n\tcase Value:\n\t\treturn valueStr\n\tcase SignalToNoiseRatio:\n\t\treturn signalToNoiseRatioStr\n\tdefault:\n\t\treturn unknownStr\n\t}\n}\n\n// IsKnown determines if this output is known.\nfunc (o Output) IsKnown() bool {\n\treturn o >= Value && o < outputLast\n}\n\n// MarshalJSON implements the Marshaler interface.\nfunc (o Output) MarshalJSON() ([]byte, error) {\n\tconst (\n\t\terrFmt = \"cannot marshal '%s': unknown corona signal to noise ratio output\"\n\t\textra  = 2   // Two bytes for quotes.\n\t\tdqc    = '\"' // Double quote character.\n\t)\n\n\ts := o.String()\n\tif s == unknownStr {\n\t\treturn nil, fmt.Errorf(errFmt, s)\n\t}\n\n\tb := make([]byte, 0, len(s)+extra)\n\tb = append(b, dqc)\n\tb = append(b, s...)\n\tb = append(b, dqc)\n\n\treturn b, nil\n}\n\n// UnmarshalJSON implements the Unmarshaler interface.\nfunc (o *Output) UnmarshalJSON(data []byte) error {\n\tconst (\n\t\terrFmt = \"cannot unmarshal '%s': unknown corona signal to noise ratio output\"\n\t\tdqs    = \"\\\"\" // Double quote string.\n\t)\n\n\td := bytes.Trim(data, dqs)\n\ts := string(d)\n\n\tswitch s {\n\tcase valueStr:\n\t\t*o = Value\n\tcase signalToNoiseRatioStr:\n\t\t*o = SignalToNoiseRatio\n\tdefault:\n\t\treturn fmt.Errorf(errFmt, s)\n\t}\n\n\treturn nil\n}\n"
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
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratiooutput_test.go",
  "content": "//nolint:testpackage\npackage coronasignaltonoiseratio\n\nimport (\n\t\"testing\"\n)\n\nfunc TestOutputString(t *testing.T) {\n\tt.Parallel()\n\n\ttests := []struct {\n\t\to    Output\n\t\ttext string\n\t}{\n\t\t{Value, valueStr},\n\t\t{SignalToNoiseRatio, signalToNoiseRatioStr},\n\t\t{outputLast, unknownStr},\n\t\t{Output(0), unknownStr},\n\t\t{Output(9999), unknownStr},\n\t\t{Output(-9999), unknownStr},\n\t}\n\n\tfor _, tt := range tests {\n\t\texp := tt.text\n\t\tact := tt.o.String()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"'%v'.String(): expected '%v', actual '%v'\", tt.o, exp, act)\n\t\t}\n\t}\n}\n\nfunc TestOutputIsKnown(t *testing.T) {\n\tt.Parallel()\n\n\ttests := []struct {\n\t\to       Output\n\t\tboolean bool\n\t}{\n\t\t{Value, true},\n\t\t{SignalToNoiseRatio, true},\n\t\t{outputLast, false},\n\t\t{Output(0), false},\n\t\t{Output(9999), false},\n\t\t{Output(-9999), false},\n\t}\n\n\tfor _, tt := range tests {\n\t\texp := tt.boolean\n\t\tact := tt.o.IsKnown()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"'%v'.IsKnown(): expected '%v', actual '%v'\", tt.o, exp, act)\n\t\t}\n\t}\n}\n\nfunc TestOutputMarshalJSON(t *testing.T) {\n\tt.Parallel()\n\n\tconst dqs = \"\\\"\"\n\n\tvar nilstr string\n\ttests := []struct {\n\t\to         Output\n\t\tjson      string\n\t\tsucceeded bool\n\t}{\n\t\t{Value, dqs + valueStr + dqs, true},\n\t\t{SignalToNoiseRatio, dqs + signalToNoiseRatioStr + dqs, true},\n\t\t{outputLast, nilstr, false},\n\t\t{Output(9999), nilstr, false},\n\t\t{Output(-9999), nilstr, false},\n\t\t{Output(0), nilstr, false},\n\t}\n\n\tfor _, tt := range tests {\n\t\texp := tt.json\n\t\tbs, err := tt.o.MarshalJSON()\n\n\t\tif err != nil && tt.succeeded {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected success '%v', got error %v\", tt.o, exp, err)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif err == nil && !tt.succeeded {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected error, got success\", tt.o)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tact := string(bs)\n\t\tif exp != act {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected '%v', actual '%v'\", tt.o, exp, act)\n\t\t}\n\t}\n}\n\nfunc TestOutputUnmarshalJSON(t *testing.T) {\n\tt.Parallel()\n\n\tconst dqs = \"\\\"\"\n\n\tvar zero Output\n\ttests := []struct {\n\t\to         Output\n\t\tjson      string\n\t\tsucceeded bool\n\t}{\n\t\t{Value, dqs + valueStr + dqs, true},\n\t\t{SignalToNoiseRatio, dqs + signalToNoiseRatioStr + dqs, true},\n\t\t{zero, dqs + unknownStr + dqs, false},\n\t\t{zero, dqs + \"foobar\" + dqs, false},\n\t}\n\n\tfor _, tt := range tests {\n\t\texp := tt.o\n\t\tbs := []byte(tt.json)\n\n\t\tvar o Output\n\n\t\terr := o.UnmarshalJSON(bs)\n\t\tif err != nil && tt.succeeded {\n\t\t\tt.Errorf(\"UnmarshalJSON('%v'): expected success '%v', got error %v\", tt.json, exp, err)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif err == nil && !tt.succeeded {\n\t\t\tt.Errorf(\"MarshalJSON('%v'): expected error, got success\", tt.json)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif exp != o {\n\t\t\tt.Errorf(\"MarshalJSON('%v'): expected '%v', actual '%v'\", tt.json, exp, o)\n\t\t}\n\t}\n}\n"
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

## Assistant (Build · claude-opus-4.7 · 2067.5s)

_Thinking:_

 Those LSP errors are just noise from vendor files I can ignore. For the main indicator file, I need to set up an `Update` method with the signature taking sample, sampleLow, sampleHigh, and time, then create wrapper methods for different data types—scalar, bar, quote, and trade—each handling their specific input patterns. The method returns both a heatmap output and an SNR value as a float.

Now I'm implementing the wrapper methods where UpdateScalar treats the value as both high and low, UpdateBar extracts the close function and uses the high/low from the bar directly, and UpdateQuote and Trade both use the value for both high and low bounds.

**Tool: write**

**Input:**
```json
{}
```

**Error:**
```
Tool execution aborted
```

---

## User

looks like preparing write takes too long time.....

---

## Assistant (Build · claude-opus-4.7 · 69.6s)

_Thinking:_

 The user is concerned about performance, so I should break this into smaller chunks or just move forward with a streamlined approach rather than trying to write everything at once.

Understood — let me write it in one shot but keep it focused.

**Tool: write**

**Input:**
```json
{
  "content": "// Package coronasignaltonoiseratio implements Ehlers' Corona Signal-to-Noise\n// Ratio heatmap indicator.\n//\n// The Corona Signal to Noise Ratio is a measure of the cycle amplitude relative\n// to noise. The \"noise\" is chosen to be the average bar height because there is\n// not much trade information within the bar.\n//\n// Reference: John Ehlers, \"Measuring Cycle Periods\", Stocks & Commodities,\n// November 2008.\npackage coronasignaltonoiseratio\n\n//nolint: gofumpt\nimport (\n\t\"fmt\"\n\t\"math\"\n\t\"sort\"\n\t\"sync\"\n\t\"time\"\n\n\t\"zpano/entities\"\n\t\"zpano/indicators/core\"\n\t\"zpano/indicators/core/outputs\"\n\t\"zpano/indicators/johnehlers/corona\"\n)\n\nconst (\n\thighLowBufferSize        = 5\n\thighLowBufferSizeMinOne  = highLowBufferSize - 1\n\thighLowMedianIndex       = 2\n\taverageSampleAlpha       = 0.1\n\taverageSampleOneMinus    = 0.9\n\tsignalEmaAlpha           = 0.2\n\tsignalEmaOneMinus        = 0.9 // Intentional: sums to 1.1, per Ehlers.\n\tnoiseEmaAlpha            = 0.1\n\tnoiseEmaOneMinus         = 0.9\n\tratioOffsetDb            = 3.5\n\tratioUpperDb             = 10.\n\tdbGain                   = 20.\n\twidthLowRatioThreshold   = 0.5\n\twidthBaseline            = 0.2\n\twidthSlope               = 0.4\n\trasterBlendExponent      = 0.8\n\trasterBlendHalf          = 0.5\n\trasterNegativeArgCutoff  = 1.\n)\n\n// CoronaSignalToNoiseRatio is Ehlers' Corona Signal-to-Noise Ratio indicator.\n//\n// It owns a private Corona spectral-analysis engine and exposes two outputs:\n//\n//   - Value: a per-bar heatmap column (intensity raster).\n//   - SignalToNoiseRatio: the current SNR value mapped into\n//     [MinParameterValue, MaxParameterValue].\ntype CoronaSignalToNoiseRatio struct {\n\tmu                  sync.RWMutex\n\tmnemonic            string\n\tdescription         string\n\tmnemonicSNR         string\n\tdescriptionSNR      string\n\tc                   *corona.Corona\n\trasterLength        int\n\trasterStep          float64\n\tmaxRasterValue      float64\n\tminParameterValue   float64\n\tmaxParameterValue   float64\n\tparameterResolution float64\n\traster              []float64\n\thighLowBuffer       [highLowBufferSize]float64\n\thlSorted            [highLowBufferSize]float64\n\taverageSamplePrev   float64\n\tsignalPrev          float64\n\tnoisePrev           float64\n\tsignalToNoiseRatio  float64\n\tisStarted           bool\n\tbarFunc             entities.BarFunc\n\tquoteFunc           entities.QuoteFunc\n\ttradeFunc           entities.TradeFunc\n}\n\n// NewCoronaSignalToNoiseRatioDefault returns an instance created with default parameters.\nfunc NewCoronaSignalToNoiseRatioDefault() (*CoronaSignalToNoiseRatio, error) {\n\treturn NewCoronaSignalToNoiseRatioParams(&Params{})\n}\n\n// NewCoronaSignalToNoiseRatioParams returns an instance created with the supplied parameters.\n//\n//nolint:funlen,cyclop\nfunc NewCoronaSignalToNoiseRatioParams(p *Params) (*CoronaSignalToNoiseRatio, error) {\n\tconst (\n\t\tinvalid      = \"invalid corona signal to noise ratio parameters\"\n\t\tfmtRaster    = \"%s: RasterLength should be >= 2\"\n\t\tfmtMaxRaster = \"%s: MaxRasterValue should be > 0\"\n\t\tfmtMinParam  = \"%s: MinParameterValue should be >= 0\"\n\t\tfmtMaxParam  = \"%s: MaxParameterValue should be > MinParameterValue\"\n\t\tfmtHP        = \"%s: HighPassFilterCutoff should be >= 2\"\n\t\tfmtMinP      = \"%s: MinimalPeriod should be >= 2\"\n\t\tfmtMaxP      = \"%s: MaximalPeriod should be > MinimalPeriod\"\n\t\tfmtw         = \"%s: %w\"\n\t\tfmtnVal      = \"csnr(%d, %g, %g, %g, %d%s)\"\n\t\tfmtnSNR      = \"csnr-snr(%d%s)\"\n\t\tdescrVal     = \"Corona signal to noise ratio \"\n\t\tdescrSNR     = \"Corona signal to noise ratio scalar \"\n\t\tdefRaster    = 50\n\t\tdefMaxRast   = 20.0\n\t\tdefMinParam  = 1.0\n\t\tdefMaxParam  = 11.0\n\t\tdefHPCutoff  = 30\n\t\tdefMinPer    = 6\n\t\tdefMaxPer    = 30\n\t)\n\n\tcfg := *p\n\n\tif cfg.RasterLength == 0 {\n\t\tcfg.RasterLength = defRaster\n\t}\n\n\tif cfg.MaxRasterValue == 0 {\n\t\tcfg.MaxRasterValue = defMaxRast\n\t}\n\n\tif cfg.MinParameterValue == 0 {\n\t\tcfg.MinParameterValue = defMinParam\n\t}\n\n\tif cfg.MaxParameterValue == 0 {\n\t\tcfg.MaxParameterValue = defMaxParam\n\t}\n\n\tif cfg.HighPassFilterCutoff == 0 {\n\t\tcfg.HighPassFilterCutoff = defHPCutoff\n\t}\n\n\tif cfg.MinimalPeriod == 0 {\n\t\tcfg.MinimalPeriod = defMinPer\n\t}\n\n\tif cfg.MaximalPeriod == 0 {\n\t\tcfg.MaximalPeriod = defMaxPer\n\t}\n\n\tif cfg.RasterLength < 2 {\n\t\treturn nil, fmt.Errorf(fmtRaster, invalid)\n\t}\n\n\tif cfg.MaxRasterValue <= 0 {\n\t\treturn nil, fmt.Errorf(fmtMaxRaster, invalid)\n\t}\n\n\tif cfg.MinParameterValue < 0 {\n\t\treturn nil, fmt.Errorf(fmtMinParam, invalid)\n\t}\n\n\tif cfg.MaxParameterValue <= cfg.MinParameterValue {\n\t\treturn nil, fmt.Errorf(fmtMaxParam, invalid)\n\t}\n\n\tif cfg.HighPassFilterCutoff < 2 {\n\t\treturn nil, fmt.Errorf(fmtHP, invalid)\n\t}\n\n\tif cfg.MinimalPeriod < 2 {\n\t\treturn nil, fmt.Errorf(fmtMinP, invalid)\n\t}\n\n\tif cfg.MaximalPeriod <= cfg.MinimalPeriod {\n\t\treturn nil, fmt.Errorf(fmtMaxP, invalid)\n\t}\n\n\t// Ehlers reference uses (High+Low)/2.\n\tbc := cfg.BarComponent\n\tif bc == 0 {\n\t\tbc = entities.BarMedianPrice\n\t}\n\n\tqc := cfg.QuoteComponent\n\tif qc == 0 {\n\t\tqc = entities.DefaultQuoteComponent\n\t}\n\n\ttc := cfg.TradeComponent\n\tif tc == 0 {\n\t\ttc = entities.DefaultTradeComponent\n\t}\n\n\tc, err := corona.NewCorona(&corona.Params{\n\t\tHighPassFilterCutoff: cfg.HighPassFilterCutoff,\n\t\tMinimalPeriod:        cfg.MinimalPeriod,\n\t\tMaximalPeriod:        cfg.MaximalPeriod,\n\t})\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\tbarFunc, err := entities.BarComponentFunc(bc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\tquoteFunc, err := entities.QuoteComponentFunc(qc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\ttradeFunc, err := entities.TradeComponentFunc(tc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\tcomponentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)\n\n\tmnemonicVal := fmt.Sprintf(fmtnVal,\n\t\tcfg.RasterLength, cfg.MaxRasterValue, cfg.MinParameterValue, cfg.MaxParameterValue,\n\t\tcfg.HighPassFilterCutoff, componentMnemonic)\n\tmnemonicSNR := fmt.Sprintf(fmtnSNR, cfg.HighPassFilterCutoff, componentMnemonic)\n\n\t// Resolution satisfies: min + (rasterLength-1)/resolution = max.\n\tparamResolution := float64(cfg.RasterLength-1) / (cfg.MaxParameterValue - cfg.MinParameterValue)\n\n\treturn &CoronaSignalToNoiseRatio{\n\t\tmnemonic:            mnemonicVal,\n\t\tdescription:         descrVal + mnemonicVal,\n\t\tmnemonicSNR:         mnemonicSNR,\n\t\tdescriptionSNR:      descrSNR + mnemonicSNR,\n\t\tc:                   c,\n\t\trasterLength:        cfg.RasterLength,\n\t\trasterStep:          cfg.MaxRasterValue / float64(cfg.RasterLength),\n\t\tmaxRasterValue:      cfg.MaxRasterValue,\n\t\tminParameterValue:   cfg.MinParameterValue,\n\t\tmaxParameterValue:   cfg.MaxParameterValue,\n\t\tparameterResolution: paramResolution,\n\t\traster:              make([]float64, cfg.RasterLength),\n\t\tsignalToNoiseRatio:  math.NaN(),\n\t\tbarFunc:             barFunc,\n\t\tquoteFunc:           quoteFunc,\n\t\ttradeFunc:           tradeFunc,\n\t}, nil\n}\n\n// IsPrimed indicates whether the indicator is primed.\nfunc (s *CoronaSignalToNoiseRatio) IsPrimed() bool {\n\ts.mu.RLock()\n\tdefer s.mu.RUnlock()\n\n\treturn s.c.IsPrimed()\n}\n\n// Metadata describes the output data of the indicator.\nfunc (s *CoronaSignalToNoiseRatio) Metadata() core.Metadata {\n\treturn core.Metadata{\n\t\tType:        core.CoronaSignalToNoiseRatio,\n\t\tMnemonic:    s.mnemonic,\n\t\tDescription: s.description,\n\t\tOutputs: []outputs.Metadata{\n\t\t\t{Kind: int(Value), Type: outputs.HeatmapType, Mnemonic: s.mnemonic, Description: s.description},\n\t\t\t{Kind: int(SignalToNoiseRatio), Type: outputs.ScalarType, Mnemonic: s.mnemonicSNR, Description: s.descriptionSNR},\n\t\t},\n\t}\n}\n\n// Update feeds the next sample plus bar extremes and returns the heatmap column\n// and the current SignalToNoiseRatio. On unprimed bars the heatmap is empty and\n// the scalar is NaN. On NaN sample input state is left unchanged.\n//\n//nolint:funlen,cyclop,gocognit\nfunc (s *CoronaSignalToNoiseRatio) Update(sample, sampleLow, sampleHigh float64, t time.Time) (*outputs.Heatmap, float64) {\n\ts.mu.Lock()\n\tdefer s.mu.Unlock()\n\n\tif math.IsNaN(sample) {\n\t\treturn outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()\n\t}\n\n\tprimed := s.c.Update(sample)\n\n\tif !s.isStarted {\n\t\ts.averageSamplePrev = sample\n\t\ts.highLowBuffer[highLowBufferSizeMinOne] = sampleHigh - sampleLow\n\t\ts.isStarted = true\n\n\t\treturn outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()\n\t}\n\n\tmaxAmpSq := s.c.MaximalAmplitudeSquared()\n\n\taverageSample := averageSampleAlpha*sample + averageSampleOneMinus*s.averageSamplePrev\n\ts.averageSamplePrev = averageSample\n\n\tif math.Abs(averageSample) > 0 || maxAmpSq > 0 {\n\t\ts.signalPrev = signalEmaAlpha*math.Sqrt(maxAmpSq) + signalEmaOneMinus*s.signalPrev\n\t}\n\n\t// Shift H-L ring buffer left; push new value.\n\tfor i := 0; i < highLowBufferSizeMinOne; i++ {\n\t\ts.highLowBuffer[i] = s.highLowBuffer[i+1]\n\t}\n\ts.highLowBuffer[highLowBufferSizeMinOne] = sampleHigh - sampleLow\n\n\tratio := 0.0\n\tif math.Abs(averageSample) > 0 {\n\t\tfor i := 0; i < highLowBufferSize; i++ {\n\t\t\ts.hlSorted[i] = s.highLowBuffer[i]\n\t\t}\n\n\t\tsort.Float64s(s.hlSorted[:])\n\t\ts.noisePrev = noiseEmaAlpha*s.hlSorted[highLowMedianIndex] + noiseEmaOneMinus*s.noisePrev\n\n\t\tif math.Abs(s.noisePrev) > 0 {\n\t\t\tratio = dbGain*math.Log10(s.signalPrev/s.noisePrev) + ratioOffsetDb\n\t\t\tif ratio < 0 {\n\t\t\t\tratio = 0\n\t\t\t} else if ratio > ratioUpperDb {\n\t\t\t\tratio = ratioUpperDb\n\t\t\t}\n\n\t\t\tratio /= ratioUpperDb // ∈ [0, 1]\n\t\t}\n\t}\n\n\ts.signalToNoiseRatio = (s.maxParameterValue-s.minParameterValue)*ratio + s.minParameterValue\n\n\t// Raster update.\n\twidth := 0.0\n\tif ratio <= widthLowRatioThreshold {\n\t\twidth = widthBaseline - widthSlope*ratio\n\t}\n\n\tratioScaledToRasterLength := int(math.Round(ratio * float64(s.rasterLength)))\n\tratioScaledToMaxRasterValue := ratio * s.maxRasterValue\n\n\tfor i := 0; i < s.rasterLength; i++ {\n\t\tvalue := s.raster[i]\n\n\t\tswitch {\n\t\tcase i == ratioScaledToRasterLength:\n\t\t\tvalue *= 0.5\n\t\tcase width == 0:\n\t\t\t// Above the high-ratio threshold: handled by the ratio>0.5 override below.\n\t\tdefault:\n\t\t\targument := (ratioScaledToMaxRasterValue - s.rasterStep*float64(i)) / width\n\t\t\tif i < ratioScaledToRasterLength {\n\t\t\t\tvalue = rasterBlendHalf * (math.Pow(argument, rasterBlendExponent) + value)\n\t\t\t} else {\n\t\t\t\targument = -argument\n\t\t\t\tif argument > rasterNegativeArgCutoff {\n\t\t\t\t\tvalue = rasterBlendHalf * (math.Pow(argument, rasterBlendExponent) + value)\n\t\t\t\t} else {\n\t\t\t\t\tvalue = s.maxRasterValue\n\t\t\t\t}\n\t\t\t}\n\t\t}\n\n\t\tif value < 0 {\n\t\t\tvalue = 0\n\t\t} else if value > s.maxRasterValue {\n\t\t\tvalue = s.maxRasterValue\n\t\t}\n\n\t\tif ratio > widthLowRatioThreshold {\n\t\t\tvalue = s.maxRasterValue\n\t\t}\n\n\t\ts.raster[i] = value\n\t}\n\n\tif !primed {\n\t\treturn outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()\n\t}\n\n\tvalues := make([]float64, s.rasterLength)\n\tvalueMin := math.Inf(1)\n\tvalueMax := math.Inf(-1)\n\n\tfor i := 0; i < s.rasterLength; i++ {\n\t\tv := s.raster[i]\n\t\tvalues[i] = v\n\n\t\tif v < valueMin {\n\t\t\tvalueMin = v\n\t\t}\n\n\t\tif v > valueMax {\n\t\t\tvalueMax = v\n\t\t}\n\t}\n\n\theatmap := outputs.NewHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution,\n\t\tvalueMin, valueMax, values)\n\n\treturn heatmap, s.signalToNoiseRatio\n}\n\n// UpdateScalar updates the indicator given the next scalar sample. Since no\n// High/Low is available, the sample is used for both, yielding zero noise.\nfunc (s *CoronaSignalToNoiseRatio) UpdateScalar(sample *entities.Scalar) core.Output {\n\treturn s.updateEntity(sample.Time, sample.Value, sample.Value, sample.Value)\n}\n\n// UpdateBar updates the indicator given the next bar sample.\nfunc (s *CoronaSignalToNoiseRatio) UpdateBar(sample *entities.Bar) core.Output {\n\treturn s.updateEntity(sample.Time, s.barFunc(sample), sample.Low, sample.High)\n}\n\n// UpdateQuote updates the indicator given the next quote sample.\nfunc (s *CoronaSignalToNoiseRatio) UpdateQuote(sample *entities.Quote) core.Output {\n\tv := s.quoteFunc(sample)\n\n\treturn s.updateEntity(sample.Time, v, v, v)\n}\n\n// UpdateTrade updates the indicator given the next trade sample.\nfunc (s *CoronaSignalToNoiseRatio) UpdateTrade(sample *entities.Trade) core.Output {\n\tv := s.tradeFunc(sample)\n\n\treturn s.updateEntity(sample.Time, v, v, v)\n}\n\nfunc (s *CoronaSignalToNoiseRatio) updateEntity(t time.Time, sample, low, high float64) core.Output {\n\tconst length = 2\n\n\theatmap, snr := s.Update(sample, low, high, t)\n\n\toutput := make([]any, length)\n\toutput[0] = heatmap\n\toutput[1] = entities.Scalar{Time: t, Value: snr}\n\n\treturn output\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio.go"
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

## Assistant (Build · claude-opus-4.7 · 5.1s)

_Thinking:_

 I should run a build check to catch any errors before moving on to the test file.

**Tool: bash**

**Input:**
```json
{
  "command": "go build ./indicators/johnehlers/coronasignaltonoiseratio/...",
  "description": "Build CSNR package",
  "workdir": "/home/dev/zpano/go"
}
```

---

## Assistant (Build · claude-opus-4.7 · 81.9s)

_Thinking:_

 Writing test snapshots...

Now let me write a minimal test file that captures snapshots, run it to dump values, then lock them in.

**Tool: write**

**Input:**
```json
{
  "content": "//nolint:testpackage\npackage coronasignaltonoiseratio\n\nimport (\n\t\"math\"\n\t\"testing\"\n\t\"time\"\n\n\t\"zpano/entities\"\n\t\"zpano/indicators/core\"\n\t\"zpano/indicators/core/outputs\"\n)\n\nfunc testCSNRTime() time.Time {\n\treturn time.Date(2021, time.April, 1, 0, 0, 0, 0, &time.Location{})\n}\n\n// testCSNRInput is the 252-entry TA-Lib MAMA reference series.\n//\n//nolint:dupl\nfunc testCSNRInput() []float64 {\n\treturn []float64{\n\t\t92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,\n\t\t94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,\n\t\t88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,\n\t\t85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,\n\t\t83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,\n\t\t89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,\n\t\t89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,\n\t\t88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,\n\t\t103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,\n\t\t120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,\n\t\t114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,\n\t\t114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,\n\t\t124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,\n\t\t137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,\n\t\t123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,\n\t\t122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,\n\t\t123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,\n\t\t130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,\n\t\t127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,\n\t\t121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,\n\t\t106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,\n\t\t95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,\n\t\t94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,\n\t\t103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,\n\t\t106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,\n\t\t109.5300, 108.0600,\n\t}\n}\n\nconst testCSNRTolerance = 1e-4\n\n// Produce synthetic High/Low around the sample to avoid zero-noise pathology.\n// Shape: H = sample * 1.005, L = sample * 0.995 → H-L = sample * 0.01.\nfunc makeHL(sample float64) (float64, float64) {\n\treturn sample * 0.995, sample * 1.005\n}\n\nfunc TestCoronaSignalToNoiseRatioUpdate(t *testing.T) {\n\tt.Parallel()\n\n\tinput := testCSNRInput()\n\tt0 := testCSNRTime()\n\n\t// SNAPSHOTS: values captured from a first run — will be filled in then locked.\n\ttype snap struct {\n\t\ti   int\n\t\tsnr float64\n\t\tvmn float64\n\t\tvmx float64\n\t}\n\tsnapshots := []snap{}\n\n\tx, err := NewCoronaSignalToNoiseRatioDefault()\n\tif err != nil {\n\t\tt.Fatalf(\"unexpected error: %v\", err)\n\t}\n\n\tsi := 0\n\tfor i := range input {\n\t\tlow, high := makeHL(input[i])\n\t\th, snr := x.Update(input[i], low, high, t0.Add(time.Duration(i)*time.Minute))\n\n\t\tif h == nil {\n\t\t\tt.Fatalf(\"[%d] heatmap must not be nil\", i)\n\t\t}\n\n\t\tif h.ParameterFirst != 1 || h.ParameterLast != 11 || math.Abs(h.ParameterResolution-4.9) > 1e-9 {\n\t\t\tt.Errorf(\"[%d] heatmap axis incorrect: first=%v last=%v res=%v\",\n\t\t\t\ti, h.ParameterFirst, h.ParameterLast, h.ParameterResolution)\n\t\t}\n\n\t\tif !x.IsPrimed() {\n\t\t\tif !h.IsEmpty() {\n\t\t\t\tt.Errorf(\"[%d] expected empty heatmap before priming, got len=%d\", i, len(h.Values))\n\t\t\t}\n\n\t\t\tif !math.IsNaN(snr) {\n\t\t\t\tt.Errorf(\"[%d] expected NaN snr before priming, got %v\", i, snr)\n\t\t\t}\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif len(h.Values) != 50 {\n\t\t\tt.Errorf(\"[%d] heatmap values length: expected 50, got %d\", i, len(h.Values))\n\t\t}\n\n\t\t// Print for capture during first run (commented out once snapshots filled in).\n\t\tif i == 11 || i == 12 || i == 50 || i == 100 || i == 150 || i == 200 || i == 251 {\n\t\t\tt.Logf(\"SNAP {%d, %.10f, %.10f, %.10f},\", i, snr, h.ValueMin, h.ValueMax)\n\t\t}\n\n\t\tif si < len(snapshots) && snapshots[si].i == i {\n\t\t\tif math.Abs(snapshots[si].snr-snr) > testCSNRTolerance {\n\t\t\t\tt.Errorf(\"[%d] snr: expected %v, got %v\", i, snapshots[si].snr, snr)\n\t\t\t}\n\n\t\t\tif math.Abs(snapshots[si].vmn-h.ValueMin) > testCSNRTolerance {\n\t\t\t\tt.Errorf(\"[%d] vmin: expected %v, got %v\", i, snapshots[si].vmn, h.ValueMin)\n\t\t\t}\n\n\t\t\tif math.Abs(snapshots[si].vmx-h.ValueMax) > testCSNRTolerance {\n\t\t\t\tt.Errorf(\"[%d] vmax: expected %v, got %v\", i, snapshots[si].vmx, h.ValueMax)\n\t\t\t}\n\n\t\t\tsi++\n\t\t}\n\t}\n\n\tif si != len(snapshots) {\n\t\tt.Errorf(\"did not hit all %d snapshots, reached %d\", len(snapshots), si)\n\t}\n}\n\nfunc TestCoronaSignalToNoiseRatioPrimesAtBar11(t *testing.T) {\n\tt.Parallel()\n\n\tx, _ := NewCoronaSignalToNoiseRatioDefault()\n\n\tif x.IsPrimed() {\n\t\tt.Error(\"expected not primed at start\")\n\t}\n\n\tinput := testCSNRInput()\n\tt0 := testCSNRTime()\n\tprimedAt := -1\n\n\tfor i := range input {\n\t\tlow, high := makeHL(input[i])\n\t\tx.Update(input[i], low, high, t0.Add(time.Duration(i)*time.Minute))\n\n\t\tif x.IsPrimed() && primedAt < 0 {\n\t\t\tprimedAt = i\n\t\t}\n\t}\n\n\tif primedAt != 11 {\n\t\tt.Errorf(\"expected priming at index 11, got %d\", primedAt)\n\t}\n}\n\nfunc TestCoronaSignalToNoiseRatioNaNInput(t *testing.T) {\n\tt.Parallel()\n\n\tx, _ := NewCoronaSignalToNoiseRatioDefault()\n\n\th, snr := x.Update(math.NaN(), math.NaN(), math.NaN(), testCSNRTime())\n\n\tif h == nil || !h.IsEmpty() {\n\t\tt.Errorf(\"expected empty heatmap for NaN input, got %v\", h)\n\t}\n\n\tif !math.IsNaN(snr) {\n\t\tt.Errorf(\"expected NaN snr for NaN input, got %v\", snr)\n\t}\n\n\tif x.IsPrimed() {\n\t\tt.Error(\"NaN input must not prime the indicator\")\n\t}\n}\n\nfunc TestCoronaSignalToNoiseRatioMetadata(t *testing.T) {\n\tt.Parallel()\n\n\tx, _ := NewCoronaSignalToNoiseRatioDefault()\n\tmd := x.Metadata()\n\n\tcheck := func(what string, exp, act any) {\n\t\tt.Helper()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"%s: expected %v, actual %v\", what, exp, act)\n\t\t}\n\t}\n\n\tmnVal := \"csnr(50, 20, 1, 11, 30, hl/2)\"\n\tmnSNR := \"csnr-snr(30, hl/2)\"\n\n\tcheck(\"Type\", core.CoronaSignalToNoiseRatio, md.Type)\n\tcheck(\"Mnemonic\", mnVal, md.Mnemonic)\n\tcheck(\"Description\", \"Corona signal to noise ratio \"+mnVal, md.Description)\n\tcheck(\"len(Outputs)\", 2, len(md.Outputs))\n\n\tcheck(\"Outputs[0].Kind\", int(Value), md.Outputs[0].Kind)\n\tcheck(\"Outputs[0].Type\", outputs.HeatmapType, md.Outputs[0].Type)\n\tcheck(\"Outputs[0].Mnemonic\", mnVal, md.Outputs[0].Mnemonic)\n\n\tcheck(\"Outputs[1].Kind\", int(SignalToNoiseRatio), md.Outputs[1].Kind)\n\tcheck(\"Outputs[1].Type\", outputs.ScalarType, md.Outputs[1].Type)\n\tcheck(\"Outputs[1].Mnemonic\", mnSNR, md.Outputs[1].Mnemonic)\n}\n\n//nolint:funlen\nfunc TestCoronaSignalToNoiseRatioUpdateEntity(t *testing.T) {\n\tt.Parallel()\n\n\tconst (\n\t\tprimeCount = 50\n\t\tinp        = 100.\n\t\toutputLen  = 2\n\t)\n\n\ttm := testCSNRTime()\n\tinput := testCSNRInput()\n\n\tcheck := func(act core.Output) {\n\t\tt.Helper()\n\n\t\tif len(act) != outputLen {\n\t\t\tt.Errorf(\"len(output): expected %v, actual %v\", outputLen, len(act))\n\n\t\t\treturn\n\t\t}\n\n\t\th, ok := act[0].(*outputs.Heatmap)\n\t\tif !ok {\n\t\t\tt.Errorf(\"output[0] is not a heatmap: %T\", act[0])\n\t\t} else if h.Time != tm {\n\t\t\tt.Errorf(\"output[0].Time: expected %v, actual %v\", tm, h.Time)\n\t\t}\n\n\t\ts, ok := act[1].(entities.Scalar)\n\t\tif !ok {\n\t\t\tt.Errorf(\"output[1] is not a scalar\")\n\t\t} else if s.Time != tm {\n\t\t\tt.Errorf(\"output[1].Time: expected %v, actual %v\", tm, s.Time)\n\t\t}\n\t}\n\n\tprime := func(x *CoronaSignalToNoiseRatio) {\n\t\tfor i := 0; i < primeCount; i++ {\n\t\t\tlow, high := makeHL(input[i%len(input)])\n\t\t\tx.Update(input[i%len(input)], low, high, tm)\n\t\t}\n\t}\n\n\tt.Run(\"update scalar\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\ts := entities.Scalar{Time: tm, Value: inp}\n\t\tx, _ := NewCoronaSignalToNoiseRatioDefault()\n\t\tprime(x)\n\t\tcheck(x.UpdateScalar(&s))\n\t})\n\n\tt.Run(\"update bar\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tb := entities.Bar{Time: tm, High: inp * 1.005, Low: inp * 0.995, Close: inp}\n\t\tx, _ := NewCoronaSignalToNoiseRatioDefault()\n\t\tprime(x)\n\t\tcheck(x.UpdateBar(&b))\n\t})\n\n\tt.Run(\"update quote\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tq := entities.Quote{Time: tm, Bid: inp, Ask: inp}\n\t\tx, _ := NewCoronaSignalToNoiseRatioDefault()\n\t\tprime(x)\n\t\tcheck(x.UpdateQuote(&q))\n\t})\n\n\tt.Run(\"update trade\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tr := entities.Trade{Time: tm, Price: inp}\n\t\tx, _ := NewCoronaSignalToNoiseRatioDefault()\n\t\tprime(x)\n\t\tcheck(x.UpdateTrade(&r))\n\t})\n}\n\n//nolint:funlen\nfunc TestNewCoronaSignalToNoiseRatio(t *testing.T) {\n\tt.Parallel()\n\n\tcheck := func(name string, exp, act any) {\n\t\tt.Helper()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"%s: expected %v, actual %v\", name, exp, act)\n\t\t}\n\t}\n\n\tt.Run(\"default\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx, err := NewCoronaSignalToNoiseRatioDefault()\n\t\tcheck(\"err == nil\", true, err == nil)\n\t\tcheck(\"mnemonic\", \"csnr(50, 20, 1, 11, 30, hl/2)\", x.mnemonic)\n\t\tcheck(\"MinParameterValue\", 1.0, x.minParameterValue)\n\t\tcheck(\"MaxParameterValue\", 11.0, x.maxParameterValue)\n\t\tcheck(\"RasterLength\", 50, x.rasterLength)\n\t})\n\n\tt.Run(\"RasterLength < 2\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSignalToNoiseRatioParams(&Params{RasterLength: 1})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona signal to noise ratio parameters: RasterLength should be >= 2\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"MaxParameterValue <= MinParameterValue\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSignalToNoiseRatioParams(&Params{\n\t\t\tMinParameterValue: 5,\n\t\t\tMaxParameterValue: 5,\n\t\t})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona signal to noise ratio parameters: MaxParameterValue should be > MinParameterValue\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"HighPassFilterCutoff < 2\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSignalToNoiseRatioParams(&Params{HighPassFilterCutoff: 1})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona signal to noise ratio parameters: HighPassFilterCutoff should be >= 2\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"MinimalPeriod < 2\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSignalToNoiseRatioParams(&Params{MinimalPeriod: 1})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona signal to noise ratio parameters: MinimalPeriod should be >= 2\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"MaximalPeriod <= MinimalPeriod\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSignalToNoiseRatioParams(&Params{\n\t\t\tMinimalPeriod: 10,\n\t\t\tMaximalPeriod: 10,\n\t\t})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona signal to noise ratio parameters: MaximalPeriod should be > MinimalPeriod\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"invalid bar component\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSignalToNoiseRatioParams(&Params{\n\t\t\tBarComponent: entities.BarComponent(9999),\n\t\t})\n\t\tif err == nil {\n\t\t\tt.Error(\"expected error\")\n\t\t}\n\t})\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go"
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

## Assistant (Build · claude-opus-4.7 · 5.1s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test -v -run TestCoronaSignalToNoiseRatioUpdate$ ./indicators/johnehlers/coronasignaltonoiseratio/ 2>&1 | tail -50",
  "description": "Run CSNR update test to capture snapshot values",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
=== RUN   TestCoronaSignalToNoiseRatioUpdate
=== PAUSE TestCoronaSignalToNoiseRatioUpdate
=== CONT  TestCoronaSignalToNoiseRatioUpdate
    coronasignaltonoiseratio_test.go:112: SNAP {11, 10.7729838066, 20.0000000000, 20.0000000000},
    coronasignaltonoiseratio_test.go:112: SNAP {12, 10.9637603453, 20.0000000000, 20.0000000000},
    coronasignaltonoiseratio_test.go:112: SNAP {50, 11.0000000000, 20.0000000000, 20.0000000000},
    coronasignaltonoiseratio_test.go:112: SNAP {100, 11.0000000000, 20.0000000000, 20.0000000000},
    coronasignaltonoiseratio_test.go:112: SNAP {150, 11.0000000000, 20.0000000000, 20.0000000000},
    coronasignaltonoiseratio_test.go:112: SNAP {200, 11.0000000000, 20.0000000000, 20.0000000000},
    coronasignaltonoiseratio_test.go:112: SNAP {251, 11.0000000000, 20.0000000000, 20.0000000000},
--- PASS: TestCoronaSignalToNoiseRatioUpdate (0.00s)
PASS
ok  	zpano/indicators/johnehlers/coronasignaltonoiseratio	0.003s

```

---

## Assistant (Build · claude-opus-4.7 · 23.9s)

_Thinking:_

 I'm noticing the SNR values are hitting a ceiling around 11 across the primed bars because my synthetic high-low range is too tight, creating unrealistically low noise and saturated signals. I need to use more realistic price ranges that reflect actual market movement to get a proper distribution of results.

Let me widen the high-low spread — I'll try something like 10% of the sample value to generate higher noise and exercise the full range of the code, which will give me more meaningful variation in the test snapshots.

The narrow synthetic H-L produces saturated SNR everywhere. Let me use a wider, variable band to exercise more code paths.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go",
  "oldString": "// Produce synthetic High/Low around the sample to avoid zero-noise pathology.\n// Shape: H = sample * 1.005, L = sample * 0.995 → H-L = sample * 0.01.\nfunc makeHL(sample float64) (float64, float64) {\n\treturn sample * 0.995, sample * 1.005\n}",
  "newString": "// Produce synthetic High/Low around the sample. The spread is varied to\n// exercise the full range of the SNR logic (narrow spreads → high SNR,\n// wide spreads → low SNR).\nfunc makeHL(i int, sample float64) (float64, float64) {\n\t// Fraction cycles between ~0.5% and ~6% of sample.\n\tfrac := 0.005 + 0.03*(1+math.Sin(float64(i)*0.37))\n\thalf := sample * frac\n\n\treturn sample - half, sample + half\n}"
}
```

**Output:**
```
Edit applied successfully.

LSP errors detected in this file, please fix:
<diagnostics file="/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go">
ERROR [87:31] not enough arguments in call to makeHL
	have (float64)
	want (int, float64)
ERROR [156:31] not enough arguments in call to makeHL
	have (float64)
	want (int, float64)
ERROR [259:43] not enough arguments in call to makeHL
	have (float64)
	want (int, float64)
</diagnostics>
```

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go",
  "oldString": "\tsi := 0\n\tfor i := range input {\n\t\tlow, high := makeHL(input[i])",
  "newString": "\tsi := 0\n\tfor i := range input {\n\t\tlow, high := makeHL(i, input[i])"
}
```

**Output:**
```
Edit applied successfully.

LSP errors detected in this file, please fix:
<diagnostics file="/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go">
ERROR [156:31] not enough arguments in call to makeHL
	have (float64)
	want (int, float64)
ERROR [259:43] not enough arguments in call to makeHL
	have (float64)
	want (int, float64)
</diagnostics>
```

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go",
  "oldString": "\tfor i := range input {\n\t\tlow, high := makeHL(input[i])\n\t\tx.Update(input[i], low, high, t0.Add(time.Duration(i)*time.Minute))",
  "newString": "\tfor i := range input {\n\t\tlow, high := makeHL(i, input[i])\n\t\tx.Update(input[i], low, high, t0.Add(time.Duration(i)*time.Minute))"
}
```

**Output:**
```
Edit applied successfully.

LSP errors detected in this file, please fix:
<diagnostics file="/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go">
ERROR [259:43] not enough arguments in call to makeHL
	have (float64)
	want (int, float64)
</diagnostics>
```

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go",
  "oldString": "\tprime := func(x *CoronaSignalToNoiseRatio) {\n\t\tfor i := 0; i < primeCount; i++ {\n\t\t\tlow, high := makeHL(input[i%len(input)])\n\t\t\tx.Update(input[i%len(input)], low, high, tm)\n\t\t}\n\t}",
  "newString": "\tprime := func(x *CoronaSignalToNoiseRatio) {\n\t\tfor i := 0; i < primeCount; i++ {\n\t\t\tlow, high := makeHL(i, input[i%len(input)])\n\t\t\tx.Update(input[i%len(input)], low, high, tm)\n\t\t}\n\t}"
}
```

**Output:**
```
Edit applied successfully.
```

**Tool: bash**

**Input:**
```json
{
  "command": "go test -v -run TestCoronaSignalToNoiseRatioUpdate$ ./indicators/johnehlers/coronasignaltonoiseratio/ 2>&1 | tail -20",
  "description": "Capture new snapshot values with varied spread",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
=== RUN   TestCoronaSignalToNoiseRatioUpdate
=== PAUSE TestCoronaSignalToNoiseRatioUpdate
=== CONT  TestCoronaSignalToNoiseRatioUpdate
    coronasignaltonoiseratio_test.go:117: SNAP {11, 1.0000000000, 0.0000000000, 20.0000000000},
    coronasignaltonoiseratio_test.go:117: SNAP {12, 1.0000000000, 0.0000000000, 20.0000000000},
    coronasignaltonoiseratio_test.go:117: SNAP {50, 1.0000000000, 0.0000000000, 20.0000000000},
    coronasignaltonoiseratio_test.go:117: SNAP {100, 2.9986583538, 4.2011609652, 20.0000000000},
    coronasignaltonoiseratio_test.go:117: SNAP {150, 1.0000000000, 0.0000000035, 20.0000000000},
    coronasignaltonoiseratio_test.go:117: SNAP {200, 1.0000000000, 0.0000000000, 20.0000000000},
    coronasignaltonoiseratio_test.go:117: SNAP {251, 1.0000000000, 0.0000000026, 20.0000000000},
--- PASS: TestCoronaSignalToNoiseRatioUpdate (0.00s)
PASS
ok  	zpano/indicators/johnehlers/coronasignaltonoiseratio	0.004s

```

---

## Assistant (Build · claude-opus-4.7 · 12.1s)

_Thinking:_

 I'm setting these snapshot values and locking them in place.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go",
  "newString": "\t// Snapshot values captured from a first run and locked in here.\n\ttype snap struct {\n\t\ti   int\n\t\tsnr float64\n\t\tvmn float64\n\t\tvmx float64\n\t}\n\tsnapshots := []snap{\n\t\t{11, 1.0000000000, 0.0000000000, 20.0000000000},\n\t\t{12, 1.0000000000, 0.0000000000, 20.0000000000},\n\t\t{50, 1.0000000000, 0.0000000000, 20.0000000000},\n\t\t{100, 2.9986583538, 4.2011609652, 20.0000000000},\n\t\t{150, 1.0000000000, 0.0000000035, 20.0000000000},\n\t\t{200, 1.0000000000, 0.0000000000, 20.0000000000},\n\t\t{251, 1.0000000000, 0.0000000026, 20.0000000000},\n\t}",
  "oldString": "\t// SNAPSHOTS: values captured from a first run — will be filled in then locked.\n\ttype snap struct {\n\t\ti   int\n\t\tsnr float64\n\t\tvmn float64\n\t\tvmx float64\n\t}\n\tsnapshots := []snap{}"
}
```

**Output:**
```
Edit applied successfully.
```

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/coronasignaltonoiseratio_test.go",
  "newString": "\t\tif si < len(snapshots) && snapshots[si].i == i {",
  "oldString": "\t\t// Print for capture during first run (commented out once snapshots filled in).\n\t\tif i == 11 || i == 12 || i == 50 || i == 100 || i == 150 || i == 200 || i == 251 {\n\t\t\tt.Logf(\"SNAP {%d, %.10f, %.10f, %.10f},\", i, snr, h.ValueMin, h.ValueMax)\n\t\t}\n\n\t\tif si < len(snapshots) && snapshots[si].i == i {"
}
```

**Output:**
```
Edit applied successfully.
```

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./indicators/johnehlers/coronasignaltonoiseratio/... 2>&1 | tail -20",
  "description": "Run all CSNR tests",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
ok  	zpano/indicators/johnehlers/coronasignaltonoiseratio	0.004s

```

---

## Assistant (Build · claude-opus-4.7 · 4.8s)

**Tool: bash**

**Input:**
```json
{
  "command": "go vet ./indicators/johnehlers/coronasignaltonoiseratio/... && go test ./indicators/johnehlers/... 2>&1 | tail -20",
  "description": "Vet CSNR and run all johnehlers tests",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
?   	zpano/indicators/johnehlers	[no test files]
ok  	zpano/indicators/johnehlers/centerofgravityoscillator	0.013s
ok  	zpano/indicators/johnehlers/corona	0.009s
ok  	zpano/indicators/johnehlers/coronasignaltonoiseratio	(cached)
ok  	zpano/indicators/johnehlers/coronaspectrum	0.014s
ok  	zpano/indicators/johnehlers/cybercycle	0.003s
ok  	zpano/indicators/johnehlers/dominantcycle	0.004s
ok  	zpano/indicators/johnehlers/fractaladaptivemovingaverage	0.003s
ok  	zpano/indicators/johnehlers/hilberttransformer	(cached)
ok  	zpano/indicators/johnehlers/hilberttransformerinstantaneoustrendline	0.003s
ok  	zpano/indicators/johnehlers/instantaneoustrendline	0.004s
ok  	zpano/indicators/johnehlers/mesaadaptivemovingaverage	0.005s
ok  	zpano/indicators/johnehlers/roofingfilter	0.005s
ok  	zpano/indicators/johnehlers/sinewave	0.006s
ok  	zpano/indicators/johnehlers/supersmoother	0.003s
ok  	zpano/indicators/johnehlers/trendcyclemode	0.003s
ok  	zpano/indicators/johnehlers/zerolagerrorcorrectingexponentialmovingaverage	0.003s
ok  	zpano/indicators/johnehlers/zerolagexponentialmovingaverage	0.003s

```

---

## Assistant (Build · claude-opus-4.7 · 12.3s)

✅ Phase 3 (CoronaSignalToNoiseRatio) done — all tests pass, full `johnehlers/...` suite green.

**Summary:**
- Created 5 files in `go/indicators/johnehlers/coronasignaltonoiseratio/`: params, output + output_test, main indicator, main test
- Mnemonics: `csnr(50, 20, 1, 11, 30, hl/2)` and `csnr-snr(30, hl/2)`
- Heatmap: 50-element raster, axis 1..11, resolution 4.9
- UpdateScalar/Quote/Trade: uses sample as H=L (noise=0 branch)
- UpdateBar: real bar High/Low
- Snapshots locked with varied synthetic H-L spreads (0.5–6%) to exercise ratio variation — caught one non-trivial ratio point (bar 100: SNR=2.998, vmin=4.20)

**Next:** Phase 4 (CoronaSwingPosition). Shall I proceed?

---

## User

yes

---

## Assistant (Build · claude-opus-4.7 · 5.2s)

_Thinking:_

 I'm looking at the MBST CoronaSwingPosition source code to understand how it works.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/mbst-to-convert/john-ehlers/corona/CoronaSwingPosition.cs"
}
```

**Output:**
```
<path>/home/dev/zpano/mbst-to-convert/john-ehlers/corona/CoronaSwingPosition.cs</path>
<type>file</type>
<content>1: ﻿using System;
2: using System.Collections.Generic;
3: 
4: using Mbs.Numerics;
5: using Mbs.Trading.Data;
6: using Mbs.Trading.Indicators.Abstractions;
7: 
8: // ReSharper disable once CheckNamespace
9: namespace Mbs.Trading.Indicators.JohnEhlers
10: {
11:     /// <summary>
12:     /// The Corona swing position is estimated by correlating the prices with a perfect sine wave
13:     /// having the dominant cycle period. This correlation produces a smooth waveform that lets us
14:     /// better estimate the swing position and impending turning points.
15:     /// <para />
16:     /// The vertical axis is on an arbitrary scale from -5 to 5.
17:     /// Since the purpose of this indicator is to anticipate the turning points,
18:     /// regions near the minimum and maximum are the strongest.
19:     /// <para />
20:     /// The indicator develops a “corona” near the center of the range – signifying that a turning point is not imminent.
21:     /// You will also notice a corona being displayed when the market is in a trend and there is very little cyclic swing.
22:     /// </summary>
23:     public sealed class CoronaSwingPosition : Indicator, IHeatMapIndicator
24:     {
25:         #region Members and accessors
26:         private const int MaxLeadListCount = 50;
27:         private const int MaxPositionListCount = 20;
28: 
29:         private readonly List<double> leadList = new List<double>(MaxLeadListCount);
30:         private readonly List<double> positionList = new List<double>(MaxPositionListCount);
31: 
32:         private readonly int rasterLength;
33:         private readonly double rasterStep;
34:         private bool isStarted;
35:         private double bandPassPrevious;
36:         private double bandPassPrevious2;
37:         private double samplePrevious;
38:         private double samplePrevious2;
39: 
40:         /// <summary>
41:         /// The maximal raster value of the heat-map.
42:         /// </summary>
43:         private readonly double maxRasterValue;
44: 
45:         /// <summary>
46:         /// The middle raster value of the heat-map.
47:         /// </summary>
48:         private readonly double midRasterValue;
49: 
50:         /// <summary>
51:         /// The heat-map scaled to [0, maxRasterValue].
52:         /// </summary>
53:         private readonly double[] raster;
54: 
55:         /// <summary>
56:         /// The minimum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns.
57:         /// </summary>
58:         public double MinParameterValue { get; }
59: 
60:         /// <summary>
61:         /// The maximum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns.
62:         /// </summary>
63:         public double MaxParameterValue { get; }
64: 
65:         private double swingPosition = double.NaN;
66: 
67:         /// <summary>
68:         /// The current value of the Corona swing position in the range of [MinParameterValue, MaxParameterValue] or <c>NaN</c> if not primed.
69:         /// </summary>
70:         public double SwingPosition { get { lock (Lock) { return Primed ? swingPosition : double.NaN; } } }
71: 
72:         /// <summary>
73:         /// A line indicator façade to expose a value of the SwingPosition as a separate line indicator with a fictitious update.
74:         /// <para />
75:         /// The line indicator façade do not perform actual updates. It just gets a current value of the SwingPosition from this instance, assuming it is already updated.
76:         /// <para />
77:         /// Resetting the line indicator façade has no effect. This instance should be reset instead.
78:         /// </summary>
79:         public LineIndicatorFacade SwingPositionFacade => new LineIndicatorFacade(CspName, CspMoniker, CspDescription, () => IsPrimed, () => SwingPosition);
80: 
81:         private readonly bool isSlave;
82:         private readonly Corona corona;
83: 
84:         private const string CspHeatMapName = "CoronaSwingPositionHeatmap";
85:         private const string CspHeatMapDescription = "Corona swing position heatmap";
86:         private const string CspHeatMapMoniker = "CoronaSP(h)";
87:         private const string CspName = "CoronaSwingPosition";
88:         private const string CspDescription = "Crona swing position";
89:         private const string CspMoniker = "CoronaSP";
90:         #endregion
91: 
92:         #region Construction
93:         /// <summary>
94:         /// Constructs a new instance of the class.
95:         /// </summary>
96:         /// <param name="rasterLength">The number of elements in the heat-map raster. The default value is 50.</param>
97:         /// <param name="maxRasterValue">The maximal raster value of the heat-map. The default value is 20.</param>
98:         /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns. The default value is -5.</param>
99:         /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns. The default value is 5.</param>
100:         /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
101:         /// <param name="highPassFilterCutoff">The high-pass filter cutoff (de-trending period). Suggested values are 20, 30, 100.</param>
102:         /// <param name="minimalPeriod">The minimal period. The default value is 6.</param>
103:         /// <param name="maximalPeriod">The maximal period. The default value is 30.</param>
104:         public CoronaSwingPosition(int rasterLength = 50, double maxRasterValue = 20,
105:             double minParameterValue = -5, double maxParameterValue = 5,
106:             OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice,
107:             int highPassFilterCutoff = 30, int minimalPeriod = 6, int maximalPeriod = 30)
108:             : this(null, rasterLength, maxRasterValue,
109:             minParameterValue, maxParameterValue, ohlcvComponent, highPassFilterCutoff, minimalPeriod, maximalPeriod)
110:         {
111:         }
112: 
113:         /// <summary>
114:         /// Constructs a new instance of the class.
115:         /// </summary>
116:         /// <param name="coronaSpectrum">The parent <c>CoronaSpectrum</c> instance to re-use the corona engine.</param>
117:         /// <param name="rasterLength">The number of elements in the heat-map raster. The default value is 50.</param>
118:         /// <param name="maxRasterValue">The maximal raster value of the heat-map. The default value is 20.</param>
119:         /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns. The default value is -5.</param>
120:         /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns. The default value is 5.</param>
121:         /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
122:         public CoronaSwingPosition(CoronaSpectrum coronaSpectrum,
123:             int rasterLength = 50, double maxRasterValue = 20, double minParameterValue = -5, double maxParameterValue = 5,
124:             OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
125:             : this(coronaSpectrum.Corona, rasterLength, maxRasterValue,
126:             minParameterValue, maxParameterValue, ohlcvComponent, 30, 6, 30)
127:         {
128:         }
129: 
130:         private CoronaSwingPosition(Corona corona, int rasterLength, double maxRasterValue, double minParameterValue, double maxParameterValue,
131:             OhlcvComponent ohlcvComponent,
132:             int highPassFilterCutoff, int minimalPeriod, int maximalPeriod)
133:             : base(CspHeatMapName, CspHeatMapDescription, ohlcvComponent)
134:         {
135:             Moniker = CspHeatMapMoniker;
136:             if (null == corona)
137:                 this.corona = new Corona(highPassFilterCutoff, minimalPeriod, maximalPeriod);
138:             else
139:             {
140:                 this.corona = corona;
141:                 isSlave = true;
142:             }
143:             this.rasterLength = rasterLength;
144:             this.maxRasterValue = maxRasterValue;
145:             MinParameterValue = minParameterValue;
146:             MaxParameterValue = maxParameterValue;
147:             midRasterValue = maxRasterValue / 2;
148:             rasterStep = maxRasterValue / rasterLength;
149:             raster = new double[rasterLength];
150:             for (int i = 0; i < rasterLength; ++i)
151:                 raster[i] = 0;
152:         }
153:         #endregion
154: 
155:         #region Reset
156:         /// <inheritdoc />
157:         public override void Reset()
158:         {
159:             lock (Lock)
160:             {
161:                 Primed = false;
162:                 isStarted = false;
163:                 samplePrevious = 0;
164:                 samplePrevious2 = 0;
165:                 bandPassPrevious = 0;
166:                 bandPassPrevious2 = 0;
167:                 swingPosition = double.NaN;
168:                 for (int i = 0; i < rasterLength; ++i)
169:                     raster[i] = 0;
170:                 leadList.Clear();
171:                 positionList.Clear();
172:                 if (!isSlave)
173:                     corona.Reset();
174:             }
175:         }
176:         #endregion
177: 
178:         #region Update
179:         private HeatMap Update(double sample, DateTime dateTime)
180:         {
181:             if (double.IsNaN(sample))
182:                 return null;
183:             Primed = isSlave ? corona.IsPrimed : corona.Update(sample);
184:             if (!isStarted)
185:             {
186:                 samplePrevious = sample;
187:                 isStarted = true;
188:                 return null;
189:             }
190: 
191:             // Filter bandpass InPhase component.
192:             double omega = Constants.TwoPi / corona.DominantCycleMedian;
193:             double beta2 = Math.Cos(omega);
194:             double gamma2 = 1 / Math.Cos(omega * 2 * 0.1);
195:             double alpha2 = gamma2 - Math.Sqrt(gamma2 * gamma2 - 1);
196:             double bandPass = 0.5 * (1 - alpha2) * (sample - samplePrevious2) + beta2 * (1 + alpha2) * bandPassPrevious - alpha2 * bandPassPrevious2;
197:             // Quadrature component is derivative of InPhase component divided by omega.
198:             double quadrature2 = (bandPass - bandPassPrevious) / omega;
199:             bandPassPrevious2 = bandPassPrevious;
200:             bandPassPrevious = bandPass;
201:             samplePrevious2 = samplePrevious;
202:             samplePrevious = sample;
203: 
204:             // Swing position calculation.
205:             double lead60 = 0.5 * bandPassPrevious2 + 0.866 * quadrature2;
206:             UpdatePhaseList(leadList, MaxLeadListCount, lead60, out var lowest, out var highest);
207:             // The value will be in range [0, 1].
208:             double position = highest - lowest;
209:             // The value of the swing position is not negative, so the absolute value is not necessary.
210:             if (position > double.Epsilon)
211:                 position = (lead60 - lowest) / position;
212:             UpdatePhaseList(positionList, MaxPositionListCount, position, out lowest, out highest);
213:             highest -= lowest;
214:             double width = highest > 0.85 ? 0.01 : (0.15 * highest);
215:             swingPosition = (MaxParameterValue - MinParameterValue) * position + MinParameterValue;
216:             var positionScaledToRasterLength = (int)Math.Round(position * rasterLength);
217:             double positionScaledToMaxRasterValue = position * maxRasterValue;
218:             for (int i = 0; i < rasterLength; ++i)
219:             //Parallel.For(0, rasterLength, i =>
220:             {
221:                 double value = raster[i];
222:                 if (i == positionScaledToRasterLength)
223:                     value *= 0.5;
224:                 else
225:                 {
226:                     double argument = positionScaledToMaxRasterValue - rasterStep * i;
227:                     if (i > positionScaledToRasterLength)
228:                         argument = -argument;
229:                     value = 0.5 * (Math.Pow(argument / width, 0.95) + 0.5 * value);
230:                 }
231:                 if (value < 0)
232:                     value = 0;
233:                 else if (value > maxRasterValue)
234:                     value = maxRasterValue;
235:                 if (highest > 0.8)
236:                     value = maxRasterValue;
237:                 raster[i] = value;
238:             }//);
239: 
240:             double[] intensity;
241:             if (Primed)
242:             {
243:                 intensity = new double[rasterLength];//+1
244:                 for (int i = 0; i < rasterLength; ++i)
245:                 {
246:                     double value = raster[i];
247:                     intensity[i] = value;
248:                 }
249:             }
250:             else
251:             {
252:                 intensity = null;
253:             }
254:             return new HeatMap(dateTime, intensity);
255:         }
256: 
257:         /// <summary>
258:         /// Updates the value of the indicator.
259:         /// </summary>
260:         /// <param name="ohlcv">A new ohlcv.</param>
261:         /// <returns>The new heat-map column of the indicator or <c>null</c>.</returns>
262:         public HeatMap Update(Ohlcv ohlcv)
263:         {
264:             lock (Lock)
265:             {
266:                 return Update(ohlcv.Component(OhlcvComponent), ohlcv.Time);
267:             }
268:         }
269: 
270:         /// <summary>
271:         /// Updates the value of the indicator.
272:         /// </summary>
273:         /// <param name="scalar">A new scalar.</param>
274:         /// <returns>The new heat-map column of the indicator or <c>null</c>.</returns>
275:         public HeatMap Update(Scalar scalar)
276:         {
277:             lock (Lock)
278:             {
279:                 return Update(scalar.Value, scalar.Time);
280:             }
281:         }
282:         #endregion
283: 
284:         private void UpdatePhaseList(List<double> list, int maximalCount, double lead, out double lowest, out double highest)
285:         {
286:             // Keep no more than maximal allowed number of elements.
287:             if (list.Count >= maximalCount)
288:                 list.RemoveAt(0);
289:             list.Add(lead);
290: 
291:             lowest = lead;
292:             highest = lead;
293:             foreach (var value in list)
294:             {
295:                 if (lowest > value)
296:                     lowest = value;
297:                 if (highest < value)
298:                     highest = value;
299:             }
300:         }
301:     }
302: }

(End of file - total 302 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 10.1s)

_Thinking:_

 Looking at the CoronaSwingPosition indicator, it produces a heatmap and a scalar swing position value using just the sample data with specific raster and parameter ranges, then applies a dominant cycle median calculation to derive the BP2 component.

The implementation uses two rolling window lists to track values across different time periods, making it simpler than CSNR since it only needs a single sample input rather than high/low data. I'm setting up the package structure with the appropriate mnemonics for both the main indicator and its swing position variant.

Writing the package files...

**Tool: bash**

**Input:**
```json
{
  "command": "mkdir -p /home/dev/zpano/go/indicators/johnehlers/coronaswingposition",
  "description": "Create CSwing package dir"
}
```

---

## Assistant (Build · claude-opus-4.7 · 31.2s)

**Tool: write**

**Input:**
```json
{
  "content": "package coronaswingposition\n\nimport \"zpano/entities\"\n\n// Params describes parameters to create an instance of the CoronaSwingPosition indicator.\ntype Params struct {\n\t// RasterLength is the number of elements in the heatmap raster. The default value is 50.\n\tRasterLength int\n\n\t// MaxRasterValue is the maximal raster value (z) of the heatmap. The default value is 20.\n\tMaxRasterValue float64\n\n\t// MinParameterValue is the minimal ordinate (y) value of the heatmap. The default value is -5.\n\tMinParameterValue float64\n\n\t// MaxParameterValue is the maximal ordinate (y) value of the heatmap. The default value is 5.\n\tMaxParameterValue float64\n\n\t// HighPassFilterCutoff is the high-pass filter cutoff (de-trending period) used by\n\t// the inner Corona engine. Suggested values are 20, 30, 100. The default value is 30.\n\tHighPassFilterCutoff int\n\n\t// MinimalPeriod is the minimal period of the inner Corona engine. The default value is 6.\n\tMinimalPeriod int\n\n\t// MaximalPeriod is the maximal period of the inner Corona engine. The default value is 30.\n\tMaximalPeriod int\n\n\t// BarComponent indicates the component of a bar to use when updating the indicator with a bar sample.\n\t//\n\t// If zero, the default (BarMedianPrice) is used, matching Ehlers' reference which operates on\n\t// (High+Low)/2. Since this differs from the framework-wide default, it is always shown in the\n\t// indicator mnemonic.\n\tBarComponent entities.BarComponent\n\n\t// QuoteComponent indicates the component of a quote to use when updating the indicator with a quote sample.\n\t//\n\t// If zero, the default (QuoteMidPrice) is used and the component is not shown in the indicator mnemonic.\n\tQuoteComponent entities.QuoteComponent\n\n\t// TradeComponent indicates the component of a trade to use when updating the indicator with a trade sample.\n\t//\n\t// If zero, the default (TradePrice) is used and the component is not shown in the indicator mnemonic.\n\tTradeComponent entities.TradeComponent\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingpositionparams.go"
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
  "content": "package coronaswingposition\n\nimport (\n\t\"bytes\"\n\t\"fmt\"\n)\n\n// Output describes the outputs of the indicator.\ntype Output int\n\nconst (\n\t// Value is the Corona swing position heatmap column.\n\tValue Output = iota + 1\n\t// SwingPosition is the scalar swing position in [MinParameterValue, MaxParameterValue].\n\tSwingPosition\n\toutputLast\n)\n\nconst (\n\tvalueStr         = \"value\"\n\tswingPositionStr = \"swingPosition\"\n\tunknownStr       = \"unknown\"\n)\n\n// String implements the Stringer interface.\nfunc (o Output) String() string {\n\tswitch o {\n\tcase Value:\n\t\treturn valueStr\n\tcase SwingPosition:\n\t\treturn swingPositionStr\n\tdefault:\n\t\treturn unknownStr\n\t}\n}\n\n// IsKnown determines if this output is known.\nfunc (o Output) IsKnown() bool {\n\treturn o >= Value && o < outputLast\n}\n\n// MarshalJSON implements the Marshaler interface.\nfunc (o Output) MarshalJSON() ([]byte, error) {\n\tconst (\n\t\terrFmt = \"cannot marshal '%s': unknown corona swing position output\"\n\t\textra  = 2\n\t\tdqc    = '\"'\n\t)\n\n\ts := o.String()\n\tif s == unknownStr {\n\t\treturn nil, fmt.Errorf(errFmt, s)\n\t}\n\n\tb := make([]byte, 0, len(s)+extra)\n\tb = append(b, dqc)\n\tb = append(b, s...)\n\tb = append(b, dqc)\n\n\treturn b, nil\n}\n\n// UnmarshalJSON implements the Unmarshaler interface.\nfunc (o *Output) UnmarshalJSON(data []byte) error {\n\tconst (\n\t\terrFmt = \"cannot unmarshal '%s': unknown corona swing position output\"\n\t\tdqs    = \"\\\"\"\n\t)\n\n\td := bytes.Trim(data, dqs)\n\ts := string(d)\n\n\tswitch s {\n\tcase valueStr:\n\t\t*o = Value\n\tcase swingPositionStr:\n\t\t*o = SwingPosition\n\tdefault:\n\t\treturn fmt.Errorf(errFmt, s)\n\t}\n\n\treturn nil\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingpositionoutput.go"
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
  "content": "//nolint:testpackage\npackage coronaswingposition\n\nimport \"testing\"\n\nfunc TestOutputString(t *testing.T) {\n\tt.Parallel()\n\n\ttests := []struct {\n\t\to    Output\n\t\ttext string\n\t}{\n\t\t{Value, valueStr},\n\t\t{SwingPosition, swingPositionStr},\n\t\t{outputLast, unknownStr},\n\t\t{Output(0), unknownStr},\n\t\t{Output(9999), unknownStr},\n\t\t{Output(-9999), unknownStr},\n\t}\n\n\tfor _, tt := range tests {\n\t\tif tt.text != tt.o.String() {\n\t\t\tt.Errorf(\"'%v'.String(): expected '%v', actual '%v'\", tt.o, tt.text, tt.o.String())\n\t\t}\n\t}\n}\n\nfunc TestOutputIsKnown(t *testing.T) {\n\tt.Parallel()\n\n\ttests := []struct {\n\t\to  Output\n\t\tok bool\n\t}{\n\t\t{Value, true},\n\t\t{SwingPosition, true},\n\t\t{outputLast, false},\n\t\t{Output(0), false},\n\t\t{Output(9999), false},\n\t\t{Output(-9999), false},\n\t}\n\n\tfor _, tt := range tests {\n\t\tif tt.ok != tt.o.IsKnown() {\n\t\t\tt.Errorf(\"'%v'.IsKnown(): expected %v, actual %v\", tt.o, tt.ok, tt.o.IsKnown())\n\t\t}\n\t}\n}\n\nfunc TestOutputMarshalJSON(t *testing.T) {\n\tt.Parallel()\n\n\tconst dqs = \"\\\"\"\n\n\tvar nilstr string\n\ttests := []struct {\n\t\to         Output\n\t\tjson      string\n\t\tsucceeded bool\n\t}{\n\t\t{Value, dqs + valueStr + dqs, true},\n\t\t{SwingPosition, dqs + swingPositionStr + dqs, true},\n\t\t{outputLast, nilstr, false},\n\t\t{Output(9999), nilstr, false},\n\t\t{Output(-9999), nilstr, false},\n\t\t{Output(0), nilstr, false},\n\t}\n\n\tfor _, tt := range tests {\n\t\tbs, err := tt.o.MarshalJSON()\n\n\t\tif err != nil && tt.succeeded {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected success, got error %v\", tt.o, err)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif err == nil && !tt.succeeded {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected error, got success\", tt.o)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif string(bs) != tt.json {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected '%v', actual '%v'\", tt.o, tt.json, string(bs))\n\t\t}\n\t}\n}\n\nfunc TestOutputUnmarshalJSON(t *testing.T) {\n\tt.Parallel()\n\n\tconst dqs = \"\\\"\"\n\n\tvar zero Output\n\ttests := []struct {\n\t\to         Output\n\t\tjson      string\n\t\tsucceeded bool\n\t}{\n\t\t{Value, dqs + valueStr + dqs, true},\n\t\t{SwingPosition, dqs + swingPositionStr + dqs, true},\n\t\t{zero, dqs + unknownStr + dqs, false},\n\t\t{zero, dqs + \"foobar\" + dqs, false},\n\t}\n\n\tfor _, tt := range tests {\n\t\tvar o Output\n\n\t\terr := o.UnmarshalJSON([]byte(tt.json))\n\t\tif err != nil && tt.succeeded {\n\t\t\tt.Errorf(\"UnmarshalJSON('%v'): expected success, got error %v\", tt.json, err)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif err == nil && !tt.succeeded {\n\t\t\tt.Errorf(\"UnmarshalJSON('%v'): expected error, got success\", tt.json)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif tt.o != o {\n\t\t\tt.Errorf(\"UnmarshalJSON('%v'): expected %v, actual %v\", tt.json, tt.o, o)\n\t\t}\n\t}\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingpositionoutput_test.go"
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

## Assistant (Build · claude-opus-4.7 · 63.8s)

Now the main indicator file:

**Tool: write**

**Input:**
```json
{
  "content": "// Package coronaswingposition implements Ehlers' Corona Swing Position heatmap\n// indicator.\n//\n// The Corona swing position is estimated by correlating the prices with a\n// perfect sine wave having the dominant cycle period. This correlation produces\n// a smooth waveform that lets us better estimate the swing position and\n// impending turning points.\n//\n// Reference: John Ehlers, \"Measuring Cycle Periods\", Stocks & Commodities,\n// November 2008.\npackage coronaswingposition\n\n//nolint: gofumpt\nimport (\n\t\"fmt\"\n\t\"math\"\n\t\"sync\"\n\t\"time\"\n\n\t\"zpano/entities\"\n\t\"zpano/indicators/core\"\n\t\"zpano/indicators/core/outputs\"\n\t\"zpano/indicators/johnehlers/corona\"\n)\n\nconst (\n\tmaxLeadListCount     = 50\n\tmaxPositionListCount = 20\n\n\t// 60° phase-lead coefficients: lead60 = 0.5·BP + sin(60°)·Q ≈ 0.5·BP + 0.866·Q.\n\tlead60CoefBP = 0.5\n\tlead60CoefQ  = 0.866\n\n\t// Bandpass filter delta factor in γ = 1/cos(ω · 2 · 0.1).\n\tbpDelta = 0.1\n\n\twidthHighThreshold = 0.85\n\twidthHighSaturate  = 0.8\n\twidthNarrow        = 0.01\n\twidthScale         = 0.15\n\n\trasterBlendExponent = 0.95\n\trasterBlendHalf     = 0.5\n)\n\n// CoronaSwingPosition is Ehlers' Corona Swing Position indicator.\ntype CoronaSwingPosition struct {\n\tmu                  sync.RWMutex\n\tmnemonic            string\n\tdescription         string\n\tmnemonicSP          string\n\tdescriptionSP       string\n\tc                   *corona.Corona\n\trasterLength        int\n\trasterStep          float64\n\tmaxRasterValue      float64\n\tminParameterValue   float64\n\tmaxParameterValue   float64\n\tparameterResolution float64\n\traster              []float64\n\tleadList            []float64\n\tpositionList        []float64\n\tsamplePrev          float64\n\tsamplePrev2         float64\n\tbandPassPrev        float64\n\tbandPassPrev2       float64\n\tswingPosition       float64\n\tisStarted           bool\n\tbarFunc             entities.BarFunc\n\tquoteFunc           entities.QuoteFunc\n\ttradeFunc           entities.TradeFunc\n}\n\n// NewCoronaSwingPositionDefault returns an instance created with default parameters.\nfunc NewCoronaSwingPositionDefault() (*CoronaSwingPosition, error) {\n\treturn NewCoronaSwingPositionParams(&Params{})\n}\n\n// NewCoronaSwingPositionParams returns an instance created with the supplied parameters.\n//\n//nolint:funlen,cyclop\nfunc NewCoronaSwingPositionParams(p *Params) (*CoronaSwingPosition, error) {\n\tconst (\n\t\tinvalid      = \"invalid corona swing position parameters\"\n\t\tfmtRaster    = \"%s: RasterLength should be >= 2\"\n\t\tfmtMaxRaster = \"%s: MaxRasterValue should be > 0\"\n\t\tfmtMaxParam  = \"%s: MaxParameterValue should be > MinParameterValue\"\n\t\tfmtHP        = \"%s: HighPassFilterCutoff should be >= 2\"\n\t\tfmtMinP      = \"%s: MinimalPeriod should be >= 2\"\n\t\tfmtMaxP      = \"%s: MaximalPeriod should be > MinimalPeriod\"\n\t\tfmtw         = \"%s: %w\"\n\t\tfmtnVal      = \"cswing(%d, %g, %g, %g, %d%s)\"\n\t\tfmtnSP       = \"cswing-sp(%d%s)\"\n\t\tdescrVal     = \"Corona swing position \"\n\t\tdescrSP      = \"Corona swing position scalar \"\n\t\tdefRaster    = 50\n\t\tdefMaxRast   = 20.0\n\t\tdefMinParam  = -5.0\n\t\tdefMaxParam  = 5.0\n\t\tdefHPCutoff  = 30\n\t\tdefMinPer    = 6\n\t\tdefMaxPer    = 30\n\t)\n\n\tcfg := *p\n\n\tif cfg.RasterLength == 0 {\n\t\tcfg.RasterLength = defRaster\n\t}\n\n\tif cfg.MaxRasterValue == 0 {\n\t\tcfg.MaxRasterValue = defMaxRast\n\t}\n\n\t// MinParameterValue and MaxParameterValue default to -5 and 5; since 0 is a\n\t// valid user value for either, we only substitute when both are zero (the\n\t// unconfigured case).\n\tif cfg.MinParameterValue == 0 && cfg.MaxParameterValue == 0 {\n\t\tcfg.MinParameterValue = defMinParam\n\t\tcfg.MaxParameterValue = defMaxParam\n\t}\n\n\tif cfg.HighPassFilterCutoff == 0 {\n\t\tcfg.HighPassFilterCutoff = defHPCutoff\n\t}\n\n\tif cfg.MinimalPeriod == 0 {\n\t\tcfg.MinimalPeriod = defMinPer\n\t}\n\n\tif cfg.MaximalPeriod == 0 {\n\t\tcfg.MaximalPeriod = defMaxPer\n\t}\n\n\tif cfg.RasterLength < 2 {\n\t\treturn nil, fmt.Errorf(fmtRaster, invalid)\n\t}\n\n\tif cfg.MaxRasterValue <= 0 {\n\t\treturn nil, fmt.Errorf(fmtMaxRaster, invalid)\n\t}\n\n\tif cfg.MaxParameterValue <= cfg.MinParameterValue {\n\t\treturn nil, fmt.Errorf(fmtMaxParam, invalid)\n\t}\n\n\tif cfg.HighPassFilterCutoff < 2 {\n\t\treturn nil, fmt.Errorf(fmtHP, invalid)\n\t}\n\n\tif cfg.MinimalPeriod < 2 {\n\t\treturn nil, fmt.Errorf(fmtMinP, invalid)\n\t}\n\n\tif cfg.MaximalPeriod <= cfg.MinimalPeriod {\n\t\treturn nil, fmt.Errorf(fmtMaxP, invalid)\n\t}\n\n\tbc := cfg.BarComponent\n\tif bc == 0 {\n\t\tbc = entities.BarMedianPrice\n\t}\n\n\tqc := cfg.QuoteComponent\n\tif qc == 0 {\n\t\tqc = entities.DefaultQuoteComponent\n\t}\n\n\ttc := cfg.TradeComponent\n\tif tc == 0 {\n\t\ttc = entities.DefaultTradeComponent\n\t}\n\n\tc, err := corona.NewCorona(&corona.Params{\n\t\tHighPassFilterCutoff: cfg.HighPassFilterCutoff,\n\t\tMinimalPeriod:        cfg.MinimalPeriod,\n\t\tMaximalPeriod:        cfg.MaximalPeriod,\n\t})\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\tbarFunc, err := entities.BarComponentFunc(bc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\tquoteFunc, err := entities.QuoteComponentFunc(qc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\ttradeFunc, err := entities.TradeComponentFunc(tc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\tcomponentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)\n\n\tmnemonicVal := fmt.Sprintf(fmtnVal,\n\t\tcfg.RasterLength, cfg.MaxRasterValue, cfg.MinParameterValue, cfg.MaxParameterValue,\n\t\tcfg.HighPassFilterCutoff, componentMnemonic)\n\tmnemonicSP := fmt.Sprintf(fmtnSP, cfg.HighPassFilterCutoff, componentMnemonic)\n\n\tparamResolution := float64(cfg.RasterLength-1) / (cfg.MaxParameterValue - cfg.MinParameterValue)\n\n\treturn &CoronaSwingPosition{\n\t\tmnemonic:            mnemonicVal,\n\t\tdescription:         descrVal + mnemonicVal,\n\t\tmnemonicSP:          mnemonicSP,\n\t\tdescriptionSP:       descrSP + mnemonicSP,\n\t\tc:                   c,\n\t\trasterLength:        cfg.RasterLength,\n\t\trasterStep:          cfg.MaxRasterValue / float64(cfg.RasterLength),\n\t\tmaxRasterValue:      cfg.MaxRasterValue,\n\t\tminParameterValue:   cfg.MinParameterValue,\n\t\tmaxParameterValue:   cfg.MaxParameterValue,\n\t\tparameterResolution: paramResolution,\n\t\traster:              make([]float64, cfg.RasterLength),\n\t\tleadList:            make([]float64, 0, maxLeadListCount),\n\t\tpositionList:        make([]float64, 0, maxPositionListCount),\n\t\tswingPosition:       math.NaN(),\n\t\tbarFunc:             barFunc,\n\t\tquoteFunc:           quoteFunc,\n\t\ttradeFunc:           tradeFunc,\n\t}, nil\n}\n\n// IsPrimed indicates whether the indicator is primed.\nfunc (s *CoronaSwingPosition) IsPrimed() bool {\n\ts.mu.RLock()\n\tdefer s.mu.RUnlock()\n\n\treturn s.c.IsPrimed()\n}\n\n// Metadata describes the output data of the indicator.\nfunc (s *CoronaSwingPosition) Metadata() core.Metadata {\n\treturn core.Metadata{\n\t\tType:        core.CoronaSwingPosition,\n\t\tMnemonic:    s.mnemonic,\n\t\tDescription: s.description,\n\t\tOutputs: []outputs.Metadata{\n\t\t\t{Kind: int(Value), Type: outputs.HeatmapType, Mnemonic: s.mnemonic, Description: s.description},\n\t\t\t{Kind: int(SwingPosition), Type: outputs.ScalarType, Mnemonic: s.mnemonicSP, Description: s.descriptionSP},\n\t\t},\n\t}\n}\n\n// Update feeds the next sample and returns the heatmap column plus the current\n// SwingPosition. On unprimed bars the heatmap is empty and the scalar is NaN.\n//\n//nolint:funlen,cyclop,gocognit\nfunc (s *CoronaSwingPosition) Update(sample float64, t time.Time) (*outputs.Heatmap, float64) {\n\ts.mu.Lock()\n\tdefer s.mu.Unlock()\n\n\tif math.IsNaN(sample) {\n\t\treturn outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()\n\t}\n\n\tprimed := s.c.Update(sample)\n\n\tif !s.isStarted {\n\t\ts.samplePrev = sample\n\t\ts.isStarted = true\n\n\t\treturn outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()\n\t}\n\n\t// Bandpass InPhase filter at the dominant cycle median period.\n\tomega := 2. * math.Pi / s.c.DominantCycleMedian()\n\tbeta2 := math.Cos(omega)\n\tgamma2 := 1. / math.Cos(omega*2*bpDelta)\n\talpha2 := gamma2 - math.Sqrt(gamma2*gamma2-1.)\n\tbandPass := 0.5*(1-alpha2)*(sample-s.samplePrev2) +\n\t\tbeta2*(1+alpha2)*s.bandPassPrev -\n\t\talpha2*s.bandPassPrev2\n\n\t// Quadrature = derivative / omega.\n\tquadrature2 := (bandPass - s.bandPassPrev) / omega\n\n\ts.bandPassPrev2 = s.bandPassPrev\n\ts.bandPassPrev = bandPass\n\ts.samplePrev2 = s.samplePrev\n\ts.samplePrev = sample\n\n\t// 60° lead: lead60 = 0.5·BP_prev2 + 0.866·Q\n\tlead60 := lead60CoefBP*s.bandPassPrev2 + lead60CoefQ*quadrature2\n\n\tlowest, highest := appendRolling(&s.leadList, maxLeadListCount, lead60)\n\n\t// Normalised lead position ∈ [0, 1].\n\tposition := highest - lowest\n\tif position > 0 {\n\t\tposition = (lead60 - lowest) / position\n\t}\n\n\tlowest, highest = appendRolling(&s.positionList, maxPositionListCount, position)\n\thighest -= lowest\n\n\twidth := 0.15 * highest\n\tif highest > widthHighThreshold {\n\t\twidth = widthNarrow\n\t}\n\n\ts.swingPosition = (s.maxParameterValue-s.minParameterValue)*position + s.minParameterValue\n\n\tpositionScaledToRasterLength := int(math.Round(position * float64(s.rasterLength)))\n\tpositionScaledToMaxRasterValue := position * s.maxRasterValue\n\n\tfor i := 0; i < s.rasterLength; i++ {\n\t\tvalue := s.raster[i]\n\n\t\tif i == positionScaledToRasterLength {\n\t\t\tvalue *= rasterBlendHalf\n\t\t} else {\n\t\t\targument := positionScaledToMaxRasterValue - s.rasterStep*float64(i)\n\t\t\tif i > positionScaledToRasterLength {\n\t\t\t\targument = -argument\n\t\t\t}\n\n\t\t\tif width > 0 {\n\t\t\t\tvalue = rasterBlendHalf *\n\t\t\t\t\t(math.Pow(argument/width, rasterBlendExponent) + rasterBlendHalf*value)\n\t\t\t}\n\t\t}\n\n\t\tif value < 0 {\n\t\t\tvalue = 0\n\t\t} else if value > s.maxRasterValue {\n\t\t\tvalue = s.maxRasterValue\n\t\t}\n\n\t\tif highest > widthHighSaturate {\n\t\t\tvalue = s.maxRasterValue\n\t\t}\n\n\t\tif math.IsNaN(value) {\n\t\t\tvalue = 0\n\t\t}\n\n\t\ts.raster[i] = value\n\t}\n\n\tif !primed {\n\t\treturn outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()\n\t}\n\n\tvalues := make([]float64, s.rasterLength)\n\tvalueMin := math.Inf(1)\n\tvalueMax := math.Inf(-1)\n\n\tfor i := 0; i < s.rasterLength; i++ {\n\t\tv := s.raster[i]\n\t\tvalues[i] = v\n\n\t\tif v < valueMin {\n\t\t\tvalueMin = v\n\t\t}\n\n\t\tif v > valueMax {\n\t\t\tvalueMax = v\n\t\t}\n\t}\n\n\theatmap := outputs.NewHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution,\n\t\tvalueMin, valueMax, values)\n\n\treturn heatmap, s.swingPosition\n}\n\n// UpdateScalar updates the indicator given the next scalar sample.\nfunc (s *CoronaSwingPosition) UpdateScalar(sample *entities.Scalar) core.Output {\n\treturn s.updateEntity(sample.Time, sample.Value)\n}\n\n// UpdateBar updates the indicator given the next bar sample.\nfunc (s *CoronaSwingPosition) UpdateBar(sample *entities.Bar) core.Output {\n\treturn s.updateEntity(sample.Time, s.barFunc(sample))\n}\n\n// UpdateQuote updates the indicator given the next quote sample.\nfunc (s *CoronaSwingPosition) UpdateQuote(sample *entities.Quote) core.Output {\n\treturn s.updateEntity(sample.Time, s.quoteFunc(sample))\n}\n\n// UpdateTrade updates the indicator given the next trade sample.\nfunc (s *CoronaSwingPosition) UpdateTrade(sample *entities.Trade) core.Output {\n\treturn s.updateEntity(sample.Time, s.tradeFunc(sample))\n}\n\nfunc (s *CoronaSwingPosition) updateEntity(t time.Time, sample float64) core.Output {\n\tconst length = 2\n\n\theatmap, sp := s.Update(sample, t)\n\n\toutput := make([]any, length)\n\toutput[0] = heatmap\n\toutput[1] = entities.Scalar{Time: t, Value: sp}\n\n\treturn output\n}\n\n// appendRolling appends v to the list, drops the oldest element once the list\n// reaches maxCount, and returns the current (lowest, highest) values.\nfunc appendRolling(list *[]float64, maxCount int, v float64) (float64, float64) {\n\tif len(*list) >= maxCount {\n\t\t*list = (*list)[1:]\n\t}\n\n\t*list = append(*list, v)\n\n\tlowest := v\n\thighest := v\n\n\tfor _, x := range *list {\n\t\tif x < lowest {\n\t\t\tlowest = x\n\t\t}\n\n\t\tif x > highest {\n\t\t\thighest = x\n\t\t}\n\t}\n\n\treturn lowest, highest\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition.go"
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

## Assistant (Compaction · claude-opus-4.7 · 80.1s)

---
## Goal

Port MBST's **Ehlers Corona Charts suite** (TASC November 2008) from `/home/dev/zpano/mbst-to-convert/john-ehlers/corona/` into zpano:

- **Corona** shared helper (not a registered indicator) ✅
- **CoronaSpectrum** — heatmap + DominantCycle + DominantCycleMedian scalars ✅
- **CoronaSignalToNoiseRatio** — heatmap + SNR scalar (uses High/Low for noise) ✅
- **CoronaSwingPosition** — heatmap + SwingPosition scalar 🔄 **IN PROGRESS**
- **CoronaTrendVigor** — heatmap + TrendVigor scalar ⏳

Deliverables in Go (`go/indicators/johnehlers/<pkg>/`) then TypeScript (`ts/indicators/john-ehlers/<pkg>/`). These are the **first heatmap-producing indicators in zpano**.

## Instructions

### Design decisions (confirmed with user)
- Unprimed bars → emit `outputs.NewEmptyHeatmap(time, paramFirst, paramLast, paramRes)`; scalars are NaN.
- Mnemonic style → **zpano convention**: `cspect(…)`, `cspect-dc(…)`, `cspect-dcm(…)`, `csnr(…)`, `csnr-snr(…)`, `cswing(…)`, `cswing-sp(…)`, `ctv(…)`, `ctv-tv(…)`.
- All 4 `core.Type` constants registered (done).
- After all 4 Go indicators are green + tested → **pause for user review** before TS port.

### Architecture
- Corona is a helper package (like `hilberttransformer/`); each of the 4 indicators owns its own Corona instance.
- Follow `trendcyclemode/` + `coronaspectrum/` as templates for the multi-output pattern.

### Algorithm decisions from 3-way reference cross-validation
- **Swing BP2 momentum**: 2-bar (`sample - samplePrev2`) — MBST/WealthLab/NinjaTrader. eSignal 1-bar is a bug.
- **TrendVigor lookback**: FIX the MBST bug — use `DomCycMedian - 1` directly.
- **SNR signal EMA coefficients**: Keep 0.2/0.9 (sums to 1.1) — Ehlers' intentional code.
- **DC weighted average**: N=12..60 indexing.
- **DominantCycleMedian clamp**: Apply `< minPeriod` floor to the median, not to raw DC.

### Defaults
- Corona: `HighPassFilterCutoff=30, MinimalPeriod=6, MaximalPeriod=30, DecibelsLowerThreshold=6, DecibelsUpperThreshold=20`
- CSpectrum: `minRaster=6, maxRaster=20, minParam=6, maxParam=30, hpCutoff=30, BarComponent=Median`
- CSNR: `rasterLength=50, maxRaster=20, minParam=1, maxParam=11, BarComponent=Median`
- CSwing: `rasterLength=50, maxRaster=20, minParam=-5, maxParam=5`
- CTV: `rasterLength=50, maxRaster=20, minParam=-10, maxParam=10`

### Test strategy
- Reuse 252-entry TA-Lib MAMA data (copied locally into each package's test file).
- Tolerance 1e-4.
- Standard tests: metadata, mnemonics, param validation, NaN no-op, numeric snapshots.
- Snapshot capture workflow: write test with `t.Logf("SNAP {%d, %.10f, ...},", …)` at key indices, run, then paste values into `snapshots` slice and remove the log lines.

## Discoveries

### Corona helper bug fixes (vs initial port)
1. `MaximalAmplitudeSquared` must reset to 0 per-bar.
2. First sample is store-only (MBST `if (1 == ++sampleCount)` short-circuit).
3. `DominantCycle` must reset to 0 each bar before the weighted-average loop.
4. `dominantCycleBuffer` must be initialized to `math.MaxFloat64` sentinels.

### Heatmap conventions
- `ParameterResolution = (rasterLength - 1) / (maxParam - minParam)` so `min + (len-1)/res = max`.
  - CSpectrum defaults: `(49-1)/(30-6) = 2.0`
  - CSNR defaults: `(50-1)/(11-1) = 4.9`
- Heatmap axis is emitted even on unprimed bars; only `Values` slice is empty and `ValueMin`/`ValueMax` are NaN.

### CSNR scalar-path handling
- For `UpdateScalar`/`UpdateQuote`/`UpdateTrade`: pass `value` as both High and Low ⇒ H-L = 0 ⇒ noise stays 0 ⇒ `ratio` stays 0 ⇒ SNR = `MinParameterValue`. This is intentional — no additional NaN logic needed.
- `UpdateBar`: uses bar's real `High`/`Low`.
- For the CSNR snapshot test, synthetic H-L `sample * 0.995..1.005` produced saturated SNR everywhere. Switched to varied `frac = 0.005 + 0.03*(1+sin(i*0.37))` to exercise mid-range ratio behaviour (got bar 100: SNR=2.998, vmin=4.20).

### CSwing specifics
- No High/Low needed — single-value `Update(sample, t)`.
- Uses `corona.DominantCycleMedian()` to compute the bandpass filter frequency.
- Two rolling windows: `leadList` (max 50), `positionList` (max 20) — implemented via helper `appendRolling()` that returns `(lowest, highest)`.
- `MinParameterValue`/`MaxParameterValue` defaults are **-5** and **5**; default substitution only applied when **both** are zero (so user can pass 0 for one).
- Added defensive `NaN → 0` in raster update (power of negative argument can produce NaN).

### zpano infrastructure
- `outputs.NewHeatmap(time, parameterFirst, parameterLast, parameterResolution, valueMin, valueMax, values)` / `outputs.NewEmptyHeatmap(...)`.
- `core.ComponentTripleMnemonic(bc, qc, tc)` composes the suffix (omits components at their defaults).
- TCM pattern: separate `*output.go` (Output enum + JSON), `*params.go`, main file with `Update(sample, ...)` tuple + `UpdateScalar/Bar/Quote/Trade` returning `core.Output` slice.
- TCM/Corona indicators use `entities.BarMedianPrice` when `bc == 0` (Ehlers convention).

## Accomplished

### ✅ Completed
1. Full analysis of MBST + 3 reference impls + bug cross-validation.
2. Implementation plan (Phases 0–6).
3. **Phase 1: Corona helper package** — done, 3 tests pass.
4. **4 new `core.Type` constants** registered + JSON round-trip + type_test.go updated.
5. **Phase 2: CoronaSpectrum** — all tests pass. Mnemonics: `cspect(6, 20, 6, 30, 30, hl/2)`, `cspect-dc(30, hl/2)`, `cspect-dcm(30, hl/2)`.
6. **Phase 3: CoronaSignalToNoiseRatio** — all tests pass. Mnemonics: `csnr(50, 20, 1, 11, 30, hl/2)`, `csnr-snr(30, hl/2)`.

### 🔄 In progress (Phase 4: CoronaSwingPosition)
Package `go/indicators/johnehlers/coronaswingposition/` created. Files written:
- `coronaswingpositionparams.go` ✅
- `coronaswingpositionoutput.go` + `coronaswingpositionoutput_test.go` ✅
- `coronaswingposition.go` ✅ (main indicator, 2 outputs, single-sample Update signature, mnemonic `cswing(50, 20, -5, 5, 30, hl/2)`, helper `appendRolling()`)
- `coronaswingposition_test.go` ⏳ **NOT YET WRITTEN**

Next immediate steps for this phase:
1. Write `coronaswingposition_test.go` mirroring the CSpectrum/CSNR test file structure:
   - Inline 252-entry `testCSwingInput()`
   - `TestCoronaSwingPositionUpdate` with snapshot capture at bars 11, 12, 50, 100, 150, 200, 251 — test captures `(i, sp, vmn, vmx)`
   - `TestCoronaSwingPositionPrimesAtBar11` — expects priming index 11
   - `TestCoronaSwingPositionNaNInput`
   - `TestCoronaSwingPositionMetadata` — expects `cswing(50, 20, -5, 5, 30, hl/2)` + `cswing-sp(30, hl/2)`
   - `TestCoronaSwingPositionUpdateEntity` — 4 sub-tests (scalar/bar/quote/trade)
   - `TestNewCoronaSwingPosition` — 6+ param-validation sub-tests
2. Build & run with `t.Logf` snapshots enabled, paste captured values, remove `Logf`, re-run to confirm green.
3. Run `go vet ./indicators/johnehlers/coronaswingposition/...` and full `go test ./indicators/johnehlers/...`.

### ⏳ Remaining
- Phase 4 test file completion.
- Phase 5: CoronaTrendVigor (apply DomCyc-1 lookback fix).
- **Pause for user review.**
- Phase 6: TS port of all 4 to `ts/indicators/john-ehlers/<pkg>/`.
- Update `.opencode/skills/mbst-indicator-conversion/SKILL.md` with Corona learnings.

## Relevant files / directories

### Created / modified this task
- `/home/dev/zpano/go/indicators/johnehlers/corona/` — helper package (doc.go, params.go, corona.go, corona_smoke_test.go) ✅
- `/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/` — ✅ COMPLETE (5 files, all tests pass)
- `/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/` — ✅ COMPLETE (5 files, all tests pass)
- `/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/` — 🔄 4 of 5 files written; test file is next
- `/home/dev/zpano/go/indicators/core/type.go` — 4 new constants + String/IsKnown/MarshalJSON/UnmarshalJSON
- `/home/dev/zpano/go/indicators/core/type_test.go` — 4 tables extended

### MBST source to port (remaining)
- `/home/dev/zpano/mbst-to-convert/john-ehlers/corona/CoronaSwingPosition.cs` — already read
- `/home/dev/zpano/mbst-to-convert/john-ehlers/corona/CoronaTrendVigor.cs` — **next phase**
- Reference impls: `e-signal-*.efs`, `ninja-trader-*.cs`, `wealth-lab-implementation.cs`

### Reference templates in zpano
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/` — multi-output indicator template
- `/home/dev/zpano/go/indicators/johnehlers/coronaspectrum/` — heatmap+scalars pattern (3 outputs)
- `/home/dev/zpano/go/indicators/johnehlers/coronasignaltonoiseratio/` — heatmap+scalar with H/L input (2 outputs, Update(sample, low, high, t))
- Use `coronasignaltonoiseratio_test.go` as the **direct template** for CSwing tests — same 2-output shape, but CSwing uses single-sample `Update(sample, t)` instead of `Update(sample, low, high, t)`.

### Skills
- `.opencode/skills/indicator-architecture` — loaded
- `.opencode/skills/mbst-indicator-conversion` — loaded; to be updated after all phases complete

### Immediate next action
Write `/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition_test.go` by adapting `coronasignaltonoiseratio_test.go`:
- Drop the `makeHL()` helper and the `low, high` args everywhere — CSwing uses `x.Update(input[i], t)` signature.
- Default mnemonic strings: `cswing(50, 20, -5, 5, 30, hl/2)` and `cswing-sp(30, hl/2)`.
- Default param checks: `MinParameterValue`=-5, `MaxParameterValue`=5, `RasterLength`=50.
- Heatmap axis invariants: `ParameterFirst=-5`, `ParameterLast=5`, `ParameterResolution=4.9`.
- Enable `t.Logf` snapshot-capture lines first; run `go test -v -run TestCoronaSwingPositionUpdate$ ./indicators/johnehlers/coronaswingposition/`; paste results; remove the log lines; re-run to confirm; then `go test ./indicators/johnehlers/...`.

---

## User

---

## Assistant (Build · claude-opus-4.7 · 5.2s)

_Thinking:_

 I'm checking back in on the coronaswingposition package to see what we were working on.

**Tool: bash**

**Input:**
```json
{
  "command": "ls go/indicators/johnehlers/coronaswingposition/ go/indicators/johnehlers/coronasignaltonoiseratio/",
  "description": "List current state of packages"
}
```

**Output:**
```
go/indicators/johnehlers/coronasignaltonoiseratio/:
coronasignaltonoiseratio.go
coronasignaltonoiseratio_test.go
coronasignaltonoiseratiooutput.go
coronasignaltonoiseratiooutput_test.go
coronasignaltonoiseratioparams.go

go/indicators/johnehlers/coronaswingposition/:
coronaswingposition.go
coronaswingpositionoutput.go
coronaswingpositionoutput_test.go
coronaswingpositionparams.go

```

---

## Assistant (Build · claude-opus-4.7 · 3.6s)

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

---

## Assistant (Build · claude-opus-4.7 · 3.9s)

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

## Assistant (Build · claude-opus-4.7 · 75.7s)

_Thinking:_

 Now I'm setting up the test file, borrowing the structure from CSNR to validate the main implementation.

Now I'll write the test file for CoronaSwingPosition, adapting from the CSNR test with snapshot-capture logging enabled first.

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition_test.go",
  "content": "//nolint:testpackage\npackage coronaswingposition\n\nimport (\n\t\"math\"\n\t\"testing\"\n\t\"time\"\n\n\t\"zpano/entities\"\n\t\"zpano/indicators/core\"\n\t\"zpano/indicators/core/outputs\"\n)\n\nfunc testCSwingTime() time.Time {\n\treturn time.Date(2021, time.April, 1, 0, 0, 0, 0, &time.Location{})\n}\n\n// testCSwingInput is the 252-entry TA-Lib MAMA reference series.\n//\n//nolint:dupl\nfunc testCSwingInput() []float64 {\n\treturn []float64{\n\t\t92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,\n\t\t94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,\n\t\t88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,\n\t\t85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,\n\t\t83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,\n\t\t89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,\n\t\t89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,\n\t\t88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,\n\t\t103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,\n\t\t120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,\n\t\t114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,\n\t\t114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,\n\t\t124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,\n\t\t137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,\n\t\t123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,\n\t\t122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,\n\t\t123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,\n\t\t130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,\n\t\t127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,\n\t\t121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,\n\t\t106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,\n\t\t95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,\n\t\t94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,\n\t\t103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,\n\t\t106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,\n\t\t109.5300, 108.0600,\n\t}\n}\n\nconst testCSwingTolerance = 1e-4\n\nfunc TestCoronaSwingPositionUpdate(t *testing.T) {\n\tt.Parallel()\n\n\tinput := testCSwingInput()\n\tt0 := testCSwingTime()\n\n\ttype snap struct {\n\t\ti   int\n\t\tsp  float64\n\t\tvmn float64\n\t\tvmx float64\n\t}\n\tsnapshots := []snap{\n\t\t{11, 0, 0, 0},\n\t\t{12, 0, 0, 0},\n\t\t{50, 0, 0, 0},\n\t\t{100, 0, 0, 0},\n\t\t{150, 0, 0, 0},\n\t\t{200, 0, 0, 0},\n\t\t{251, 0, 0, 0},\n\t}\n\n\t_ = snapshots\n\n\tx, err := NewCoronaSwingPositionDefault()\n\tif err != nil {\n\t\tt.Fatalf(\"unexpected error: %v\", err)\n\t}\n\n\tfor i := range input {\n\t\th, sp := x.Update(input[i], t0.Add(time.Duration(i)*time.Minute))\n\n\t\tif h == nil {\n\t\t\tt.Fatalf(\"[%d] heatmap must not be nil\", i)\n\t\t}\n\n\t\tif h.ParameterFirst != -5 || h.ParameterLast != 5 || math.Abs(h.ParameterResolution-4.9) > 1e-9 {\n\t\t\tt.Errorf(\"[%d] heatmap axis incorrect: first=%v last=%v res=%v\",\n\t\t\t\ti, h.ParameterFirst, h.ParameterLast, h.ParameterResolution)\n\t\t}\n\n\t\tif !x.IsPrimed() {\n\t\t\tif !h.IsEmpty() {\n\t\t\t\tt.Errorf(\"[%d] expected empty heatmap before priming, got len=%d\", i, len(h.Values))\n\t\t\t}\n\n\t\t\tif !math.IsNaN(sp) {\n\t\t\t\tt.Errorf(\"[%d] expected NaN sp before priming, got %v\", i, sp)\n\t\t\t}\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif len(h.Values) != 50 {\n\t\t\tt.Errorf(\"[%d] heatmap values length: expected 50, got %d\", i, len(h.Values))\n\t\t}\n\n\t\t// SNAPSHOT CAPTURE (remove once values locked in):\n\t\tswitch i {\n\t\tcase 11, 12, 50, 100, 150, 200, 251:\n\t\t\tt.Logf(\"SNAP {%d, %.10f, %.10f, %.10f},\", i, sp, h.ValueMin, h.ValueMax)\n\t\t}\n\t}\n}\n\nfunc TestCoronaSwingPositionPrimesAtBar11(t *testing.T) {\n\tt.Parallel()\n\n\tx, _ := NewCoronaSwingPositionDefault()\n\n\tif x.IsPrimed() {\n\t\tt.Error(\"expected not primed at start\")\n\t}\n\n\tinput := testCSwingInput()\n\tt0 := testCSwingTime()\n\tprimedAt := -1\n\n\tfor i := range input {\n\t\tx.Update(input[i], t0.Add(time.Duration(i)*time.Minute))\n\n\t\tif x.IsPrimed() && primedAt < 0 {\n\t\t\tprimedAt = i\n\t\t}\n\t}\n\n\tif primedAt != 11 {\n\t\tt.Errorf(\"expected priming at index 11, got %d\", primedAt)\n\t}\n}\n\nfunc TestCoronaSwingPositionNaNInput(t *testing.T) {\n\tt.Parallel()\n\n\tx, _ := NewCoronaSwingPositionDefault()\n\n\th, sp := x.Update(math.NaN(), testCSwingTime())\n\n\tif h == nil || !h.IsEmpty() {\n\t\tt.Errorf(\"expected empty heatmap for NaN input, got %v\", h)\n\t}\n\n\tif !math.IsNaN(sp) {\n\t\tt.Errorf(\"expected NaN sp for NaN input, got %v\", sp)\n\t}\n\n\tif x.IsPrimed() {\n\t\tt.Error(\"NaN input must not prime the indicator\")\n\t}\n}\n\nfunc TestCoronaSwingPositionMetadata(t *testing.T) {\n\tt.Parallel()\n\n\tx, _ := NewCoronaSwingPositionDefault()\n\tmd := x.Metadata()\n\n\tcheck := func(what string, exp, act any) {\n\t\tt.Helper()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"%s: expected %v, actual %v\", what, exp, act)\n\t\t}\n\t}\n\n\tmnVal := \"cswing(50, 20, -5, 5, 30, hl/2)\"\n\tmnSP := \"cswing-sp(30, hl/2)\"\n\n\tcheck(\"Type\", core.CoronaSwingPosition, md.Type)\n\tcheck(\"Mnemonic\", mnVal, md.Mnemonic)\n\tcheck(\"Description\", \"Corona swing position \"+mnVal, md.Description)\n\tcheck(\"len(Outputs)\", 2, len(md.Outputs))\n\n\tcheck(\"Outputs[0].Kind\", int(Value), md.Outputs[0].Kind)\n\tcheck(\"Outputs[0].Type\", outputs.HeatmapType, md.Outputs[0].Type)\n\tcheck(\"Outputs[0].Mnemonic\", mnVal, md.Outputs[0].Mnemonic)\n\n\tcheck(\"Outputs[1].Kind\", int(SwingPosition), md.Outputs[1].Kind)\n\tcheck(\"Outputs[1].Type\", outputs.ScalarType, md.Outputs[1].Type)\n\tcheck(\"Outputs[1].Mnemonic\", mnSP, md.Outputs[1].Mnemonic)\n}\n\n//nolint:funlen\nfunc TestCoronaSwingPositionUpdateEntity(t *testing.T) {\n\tt.Parallel()\n\n\tconst (\n\t\tprimeCount = 50\n\t\tinp        = 100.\n\t\toutputLen  = 2\n\t)\n\n\ttm := testCSwingTime()\n\tinput := testCSwingInput()\n\n\tcheck := func(act core.Output) {\n\t\tt.Helper()\n\n\t\tif len(act) != outputLen {\n\t\t\tt.Errorf(\"len(output): expected %v, actual %v\", outputLen, len(act))\n\n\t\t\treturn\n\t\t}\n\n\t\th, ok := act[0].(*outputs.Heatmap)\n\t\tif !ok {\n\t\t\tt.Errorf(\"output[0] is not a heatmap: %T\", act[0])\n\t\t} else if h.Time != tm {\n\t\t\tt.Errorf(\"output[0].Time: expected %v, actual %v\", tm, h.Time)\n\t\t}\n\n\t\ts, ok := act[1].(entities.Scalar)\n\t\tif !ok {\n\t\t\tt.Errorf(\"output[1] is not a scalar\")\n\t\t} else if s.Time != tm {\n\t\t\tt.Errorf(\"output[1].Time: expected %v, actual %v\", tm, s.Time)\n\t\t}\n\t}\n\n\tprime := func(x *CoronaSwingPosition) {\n\t\tfor i := 0; i < primeCount; i++ {\n\t\t\tx.Update(input[i%len(input)], tm)\n\t\t}\n\t}\n\n\tt.Run(\"update scalar\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\ts := entities.Scalar{Time: tm, Value: inp}\n\t\tx, _ := NewCoronaSwingPositionDefault()\n\t\tprime(x)\n\t\tcheck(x.UpdateScalar(&s))\n\t})\n\n\tt.Run(\"update bar\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tb := entities.Bar{Time: tm, High: inp * 1.005, Low: inp * 0.995, Close: inp}\n\t\tx, _ := NewCoronaSwingPositionDefault()\n\t\tprime(x)\n\t\tcheck(x.UpdateBar(&b))\n\t})\n\n\tt.Run(\"update quote\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tq := entities.Quote{Time: tm, Bid: inp, Ask: inp}\n\t\tx, _ := NewCoronaSwingPositionDefault()\n\t\tprime(x)\n\t\tcheck(x.UpdateQuote(&q))\n\t})\n\n\tt.Run(\"update trade\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tr := entities.Trade{Time: tm, Price: inp}\n\t\tx, _ := NewCoronaSwingPositionDefault()\n\t\tprime(x)\n\t\tcheck(x.UpdateTrade(&r))\n\t})\n}\n\n//nolint:funlen\nfunc TestNewCoronaSwingPosition(t *testing.T) {\n\tt.Parallel()\n\n\tcheck := func(name string, exp, act any) {\n\t\tt.Helper()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"%s: expected %v, actual %v\", name, exp, act)\n\t\t}\n\t}\n\n\tt.Run(\"default\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx, err := NewCoronaSwingPositionDefault()\n\t\tcheck(\"err == nil\", true, err == nil)\n\t\tcheck(\"mnemonic\", \"cswing(50, 20, -5, 5, 30, hl/2)\", x.mnemonic)\n\t\tcheck(\"MinParameterValue\", -5.0, x.minParameterValue)\n\t\tcheck(\"MaxParameterValue\", 5.0, x.maxParameterValue)\n\t\tcheck(\"RasterLength\", 50, x.rasterLength)\n\t})\n\n\tt.Run(\"RasterLength < 2\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSwingPositionParams(&Params{RasterLength: 1})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona swing position parameters: RasterLength should be >= 2\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"MaxParameterValue <= MinParameterValue\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSwingPositionParams(&Params{\n\t\t\tMinParameterValue: 5,\n\t\t\tMaxParameterValue: 5,\n\t\t})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona swing position parameters: MaxParameterValue should be > MinParameterValue\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"HighPassFilterCutoff < 2\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSwingPositionParams(&Params{HighPassFilterCutoff: 1})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona swing position parameters: HighPassFilterCutoff should be >= 2\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"MinimalPeriod < 2\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSwingPositionParams(&Params{MinimalPeriod: 1})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona swing position parameters: MinimalPeriod should be >= 2\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"MaximalPeriod <= MinimalPeriod\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSwingPositionParams(&Params{\n\t\t\tMinimalPeriod: 10,\n\t\t\tMaximalPeriod: 10,\n\t\t})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona swing position parameters: MaximalPeriod should be > MinimalPeriod\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"invalid bar component\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaSwingPositionParams(&Params{\n\t\t\tBarComponent: entities.BarComponent(9999),\n\t\t})\n\t\tif err == nil {\n\t\t\tt.Error(\"expected error\")\n\t\t}\n\t})\n}\n"
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

## Assistant (Build · claude-opus-4.7 · 5.2s)

Unrelated LSP errors. Let me run the tests to capture snapshots.

**Tool: bash**

**Input:**
```json
{
  "command": "go test -v -run TestCoronaSwingPositionUpdate$ ./indicators/johnehlers/coronaswingposition/ 2>&1 | tail -40",
  "description": "Run test to capture snapshots",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
=== RUN   TestCoronaSwingPositionUpdate
=== PAUSE TestCoronaSwingPositionUpdate
=== CONT  TestCoronaSwingPositionUpdate
    coronaswingposition_test.go:114: SNAP {11, 5.0000000000, 20.0000000000, 20.0000000000},
    coronaswingposition_test.go:114: SNAP {12, 5.0000000000, 20.0000000000, 20.0000000000},
    coronaswingposition_test.go:114: SNAP {50, 4.5384908349, 20.0000000000, 20.0000000000},
    coronaswingposition_test.go:114: SNAP {100, -3.8183742675, 3.4957777081, 20.0000000000},
    coronaswingposition_test.go:114: SNAP {150, -1.8516194371, 5.3792287864, 20.0000000000},
    coronaswingposition_test.go:114: SNAP {200, -3.6944428668, 4.2580825738, 20.0000000000},
    coronaswingposition_test.go:114: SNAP {251, -0.8524812061, 4.4822539784, 20.0000000000},
--- PASS: TestCoronaSwingPositionUpdate (0.00s)
PASS
ok  	zpano/indicators/johnehlers/coronaswingposition	0.003s

```

---

## Assistant (Build · claude-opus-4.7 · 7.2s)

Snapshots captured. Now let me lock them in and remove the log lines.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition_test.go",
  "oldString": "\tsnapshots := []snap{\n\t\t{11, 0, 0, 0},\n\t\t{12, 0, 0, 0},\n\t\t{50, 0, 0, 0},\n\t\t{100, 0, 0, 0},\n\t\t{150, 0, 0, 0},\n\t\t{200, 0, 0, 0},\n\t\t{251, 0, 0, 0},\n\t}\n\n\t_ = snapshots\n\n\tx, err := NewCoronaSwingPositionDefault()",
  "newString": "\tsnapshots := []snap{\n\t\t{11, 5.0000000000, 20.0000000000, 20.0000000000},\n\t\t{12, 5.0000000000, 20.0000000000, 20.0000000000},\n\t\t{50, 4.5384908349, 20.0000000000, 20.0000000000},\n\t\t{100, -3.8183742675, 3.4957777081, 20.0000000000},\n\t\t{150, -1.8516194371, 5.3792287864, 20.0000000000},\n\t\t{200, -3.6944428668, 4.2580825738, 20.0000000000},\n\t\t{251, -0.8524812061, 4.4822539784, 20.0000000000},\n\t}\n\n\tx, err := NewCoronaSwingPositionDefault()"
}
```

**Output:**
```
Edit applied successfully.

LSP errors detected in this file, please fix:
<diagnostics file="/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition_test.go">
ERROR [66:2] declared and not used: snapshots
</diagnostics>
```

---

## Assistant (Build · claude-opus-4.7 · 16.6s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingposition_test.go",
  "newString": "\tx, err := NewCoronaSwingPositionDefault()\n\tif err != nil {\n\t\tt.Fatalf(\"unexpected error: %v\", err)\n\t}\n\n\tsi := 0\n\tfor i := range input {\n\t\th, sp := x.Update(input[i], t0.Add(time.Duration(i)*time.Minute))\n\n\t\tif h == nil {\n\t\t\tt.Fatalf(\"[%d] heatmap must not be nil\", i)\n\t\t}\n\n\t\tif h.ParameterFirst != -5 || h.ParameterLast != 5 || math.Abs(h.ParameterResolution-4.9) > 1e-9 {\n\t\t\tt.Errorf(\"[%d] heatmap axis incorrect: first=%v last=%v res=%v\",\n\t\t\t\ti, h.ParameterFirst, h.ParameterLast, h.ParameterResolution)\n\t\t}\n\n\t\tif !x.IsPrimed() {\n\t\t\tif !h.IsEmpty() {\n\t\t\t\tt.Errorf(\"[%d] expected empty heatmap before priming, got len=%d\", i, len(h.Values))\n\t\t\t}\n\n\t\t\tif !math.IsNaN(sp) {\n\t\t\t\tt.Errorf(\"[%d] expected NaN sp before priming, got %v\", i, sp)\n\t\t\t}\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif len(h.Values) != 50 {\n\t\t\tt.Errorf(\"[%d] heatmap values length: expected 50, got %d\", i, len(h.Values))\n\t\t}\n\n\t\tif si < len(snapshots) && snapshots[si].i == i {\n\t\t\tif math.Abs(snapshots[si].sp-sp) > testCSwingTolerance {\n\t\t\t\tt.Errorf(\"[%d] sp: expected %v, got %v\", i, snapshots[si].sp, sp)\n\t\t\t}\n\n\t\t\tif math.Abs(snapshots[si].vmn-h.ValueMin) > testCSwingTolerance {\n\t\t\t\tt.Errorf(\"[%d] vmin: expected %v, got %v\", i, snapshots[si].vmn, h.ValueMin)\n\t\t\t}\n\n\t\t\tif math.Abs(snapshots[si].vmx-h.ValueMax) > testCSwingTolerance {\n\t\t\t\tt.Errorf(\"[%d] vmax: expected %v, got %v\", i, snapshots[si].vmx, h.ValueMax)\n\t\t\t}\n\n\t\t\tsi++\n\t\t}\n\t}\n\n\tif si != len(snapshots) {\n\t\tt.Errorf(\"did not hit all %d snapshots, reached %d\", len(snapshots), si)\n\t}\n}",
  "oldString": "\tx, err := NewCoronaSwingPositionDefault()\n\tif err != nil {\n\t\tt.Fatalf(\"unexpected error: %v\", err)\n\t}\n\n\tfor i := range input {\n\t\th, sp := x.Update(input[i], t0.Add(time.Duration(i)*time.Minute))\n\n\t\tif h == nil {\n\t\t\tt.Fatalf(\"[%d] heatmap must not be nil\", i)\n\t\t}\n\n\t\tif h.ParameterFirst != -5 || h.ParameterLast != 5 || math.Abs(h.ParameterResolution-4.9) > 1e-9 {\n\t\t\tt.Errorf(\"[%d] heatmap axis incorrect: first=%v last=%v res=%v\",\n\t\t\t\ti, h.ParameterFirst, h.ParameterLast, h.ParameterResolution)\n\t\t}\n\n\t\tif !x.IsPrimed() {\n\t\t\tif !h.IsEmpty() {\n\t\t\t\tt.Errorf(\"[%d] expected empty heatmap before priming, got len=%d\", i, len(h.Values))\n\t\t\t}\n\n\t\t\tif !math.IsNaN(sp) {\n\t\t\t\tt.Errorf(\"[%d] expected NaN sp before priming, got %v\", i, sp)\n\t\t\t}\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif len(h.Values) != 50 {\n\t\t\tt.Errorf(\"[%d] heatmap values length: expected 50, got %d\", i, len(h.Values))\n\t\t}\n\n\t\t// SNAPSHOT CAPTURE (remove once values locked in):\n\t\tswitch i {\n\t\tcase 11, 12, 50, 100, 150, 200, 251:\n\t\t\tt.Logf(\"SNAP {%d, %.10f, %.10f, %.10f},\", i, sp, h.ValueMin, h.ValueMax)\n\t\t}\n\t}\n}"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.7s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./indicators/johnehlers/coronaswingposition/ && go vet ./indicators/johnehlers/coronaswingposition/... && go test ./indicators/johnehlers/...",
  "description": "Run full tests and vet",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
ok  	zpano/indicators/johnehlers/coronaswingposition	0.004s
?   	zpano/indicators/johnehlers	[no test files]
ok  	zpano/indicators/johnehlers/centerofgravityoscillator	(cached)
ok  	zpano/indicators/johnehlers/corona	(cached)
ok  	zpano/indicators/johnehlers/coronasignaltonoiseratio	(cached)
ok  	zpano/indicators/johnehlers/coronaspectrum	(cached)
ok  	zpano/indicators/johnehlers/coronaswingposition	(cached)
ok  	zpano/indicators/johnehlers/cybercycle	(cached)
ok  	zpano/indicators/johnehlers/dominantcycle	(cached)
ok  	zpano/indicators/johnehlers/fractaladaptivemovingaverage	(cached)
ok  	zpano/indicators/johnehlers/hilberttransformer	(cached)
ok  	zpano/indicators/johnehlers/hilberttransformerinstantaneoustrendline	(cached)
ok  	zpano/indicators/johnehlers/instantaneoustrendline	(cached)
ok  	zpano/indicators/johnehlers/mesaadaptivemovingaverage	(cached)
ok  	zpano/indicators/johnehlers/roofingfilter	(cached)
ok  	zpano/indicators/johnehlers/sinewave	(cached)
ok  	zpano/indicators/johnehlers/supersmoother	(cached)
ok  	zpano/indicators/johnehlers/trendcyclemode	(cached)
ok  	zpano/indicators/johnehlers/zerolagerrorcorrectingexponentialmovingaverage	(cached)
ok  	zpano/indicators/johnehlers/zerolagexponentialmovingaverage	(cached)

```

---

## Assistant (Build · claude-opus-4.7 · 4.6s)

Phase 4 (CoronaSwingPosition) complete. Moving on to **Phase 5: CoronaTrendVigor**.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/mbst-to-convert/john-ehlers/corona/CoronaTrendVigor.cs"
}
```

**Output:**
```
<path>/home/dev/zpano/mbst-to-convert/john-ehlers/corona/CoronaTrendVigor.cs</path>
<type>file</type>
<content>1: ﻿using System;
2: using Mbs.Numerics;
3: using Mbs.Trading.Data;
4: using Mbs.Trading.Indicators.Abstractions;
5: 
6: // ReSharper disable once CheckNamespace
7: namespace Mbs.Trading.Indicators.JohnEhlers
8: {
9:     /// <summary>
10:     /// The basis of the trend strength Corona indicator is the slope of the momentum
11:     /// taken over a full dominant cycle period. The trend slope is normalized to the
12:     /// amplitude of the dominant cycle, and is scaled to range from -10 to +10.
13:     /// <para />
14:     /// A value of +2 means the trend up-slope is twice the cycle amplitude.
15:     /// A value of -2 means the trend down-slope is twice the cycle amplitude.
16:     /// It is prudent to not trade the trend if the indicator is within the range from -2 to +2.
17:     /// Therefore, when the indicator is in this range you will see it develop its “corona”.
18:     /// </summary>
19:     public sealed class CoronaTrendVigor : Indicator, IHeatMapIndicator
20:     {
21:         #region Members and accessors
22: 
23:         private double bandPassPrevious;
24:         private double bandPassPrevious2;
25:         private double samplePrevious;
26:         private double samplePrevious2;
27:         private readonly int sampleBufferLastIndex;
28:         private readonly int sampleBufferLength;
29:         private readonly double[] sampleBuffer;
30: 
31:         private readonly int rasterLength;
32:         private readonly double rasterStep;
33:         private int sampleCount;
34: 
35:         /// <summary>
36:         /// The previous value of the trend vigor ratio scaled to range [-10, 10].
37:         /// </summary>
38:         private double ratioPrevious;
39: 
40:         /// <summary>
41:         /// The maximal raster value of the heat-map.
42:         /// </summary>
43:         private readonly double maxRasterValue;
44: 
45:         /// <summary>
46:         /// The middle raster value of the heat-map.
47:         /// </summary>
48:         private readonly double midRasterValue;
49: 
50:         /// <summary>
51:         /// The heat-map scaled to [0, maxRasterValue].
52:         /// </summary>
53:         private readonly double[] raster;
54: 
55:         /// <summary>
56:         /// The minimum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns.
57:         /// </summary>
58:         public double MinParameterValue { get; }
59: 
60:         /// <summary>
61:         /// The maximum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns.
62:         /// </summary>
63:         public double MaxParameterValue { get; }
64: 
65:         private double trendVigor = double.NaN;
66: 
67:         /// <summary>
68:         /// The current value of the Corona trend vigor in the range of [MinParameterValue, MaxParameterValue] or <c>NaN</c> if not primed.
69:         /// </summary>
70:         public double TrendVigor => IsPrimed ? trendVigor : double.NaN;
71: 
72:         /// <summary>
73:         /// A line indicator façade to expose a value of the TrendVigor as a separate line indicator with a fictitious update.
74:         /// <para />
75:         /// The line indicator façade do not perform actual updates. It just gets a current value of the TrendVigor from this instance, assuming it is already updated.
76:         /// <para />
77:         /// Resetting the line indicator façade has no effect. This instance should be reset instead.
78:         /// </summary>
79:         public LineIndicatorFacade TrendVigorFacade => new LineIndicatorFacade(CtvName, CtvMoniker, CtvDescription, () => IsPrimed, () => TrendVigor);
80: 
81:         private readonly bool isSlave;
82:         private readonly Corona corona;
83: 
84:         private const string CtvHeatMapName = "CoronaTrendVigorHeatmap";
85:         private const string CtvHeatMapDescription = "Crona trend vigor heatmap";
86:         private const string CtvHeatMapMoniker = "CoronaTV(h)";
87: 
88:         private const string CtvName = "CoronaTrendVigor";
89:         private const string CtvDescription = "Crona trend vigor";
90:         private const string CtvMoniker = "CoronaTV";
91:         #endregion
92: 
93:         #region Construction
94:         /// <summary>
95:         /// Constructs a new instance of the class.
96:         /// </summary>
97:         /// <param name="rasterLength">The number of elements in the heat-map raster. The default value is 50.</param>
98:         /// <param name="maxRasterValue">The maximal raster value of the heat-map. The default value is 20.</param>
99:         /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns. The default value is -10.</param>
100:         /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns. The default value is 10.</param>
101:         /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
102:         /// <param name="highPassFilterCutoff">The high-pass filter cutoff (de-trending period). Suggested values are 20, 30, 100.</param>
103:         /// <param name="minimalPeriod">The minimal period. The default value is 6.</param>
104:         /// <param name="maximalPeriod">The maximal period. The default value is 30.</param>
105:         public CoronaTrendVigor(int rasterLength = 50, double maxRasterValue = 20,
106:             double minParameterValue = -10, double maxParameterValue = 10,
107:             OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice,
108:             int highPassFilterCutoff = 30, int minimalPeriod = 6, int maximalPeriod = 30)
109:             : this(null, rasterLength, maxRasterValue,
110:             minParameterValue, maxParameterValue, ohlcvComponent, highPassFilterCutoff, minimalPeriod, maximalPeriod)
111:         {
112:         }
113: 
114:         /// <summary>
115:         /// Constructs a new instance of the class.
116:         /// </summary>
117:         /// <param name="coronaSpectrum">The parent <c>CoronaSpectrum</c> instance to re-use the corona engine.</param>
118:         /// <param name="rasterLength">The number of elements in the heat-map raster. The default value is 50.</param>
119:         /// <param name="maxRasterValue">The maximal raster value of the heat-map. The default value is 20.</param>
120:         /// <param name="minParameterValue">The minimum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns. The default value is -10.</param>
121:         /// <param name="maxParameterValue">The maximum ordinate (parameter) value of the heat-map. This value is the same for all heat-map columns. The default value is 10.</param>
122:         /// <param name="ohlcvComponent">The Ohlcv component. The original indicator uses the median price (high+low)/2, which is the default.</param>
123:         public CoronaTrendVigor(CoronaSpectrum coronaSpectrum,
124:             int rasterLength = 50, double maxRasterValue = 20, double minParameterValue = -10, double maxParameterValue = 10,
125:             OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
126:             : this(coronaSpectrum.Corona, rasterLength, maxRasterValue,
127:             minParameterValue, maxParameterValue, ohlcvComponent, 30, 6, 30)
128:         {
129:         }
130: 
131:         private CoronaTrendVigor(Corona corona, int rasterLength, double maxRasterValue, double minParameterValue, double maxParameterValue,
132:             OhlcvComponent ohlcvComponent, int highPassFilterCutoff, int minimalPeriod, int maximalPeriod)
133:             : base(CtvHeatMapName, CtvHeatMapDescription, ohlcvComponent)
134:         {
135:             Moniker = CtvHeatMapMoniker;
136:             if (null == corona)
137:                 this.corona = new Corona(highPassFilterCutoff, minimalPeriod, maximalPeriod);
138:             else
139:             {
140:                 this.corona = corona;
141:                 isSlave = true;
142:             }
143:             this.rasterLength = rasterLength;
144:             this.maxRasterValue = maxRasterValue;
145:             MinParameterValue = minParameterValue;
146:             MaxParameterValue = maxParameterValue;
147:             midRasterValue = maxRasterValue / 2;
148:             rasterStep = maxRasterValue / rasterLength;
149:             raster = new double[rasterLength];
150:             for (int i = 0; i < rasterLength; ++i)
151:                 raster[i] = 0;
152:             sampleBufferLength = this.corona.MaximalPeriodTimesTwo;
153:             sampleBuffer = new double[sampleBufferLength];
154:             sampleBufferLastIndex = sampleBufferLength - 1;
155:             for (int i = 0; i < sampleBufferLength; ++i)
156:                 sampleBuffer[i] = 0;
157:         }
158:         #endregion
159: 
160:         #region Reset
161:         /// <inheritdoc />
162:         public override void Reset()
163:         {
164:             lock (Lock)
165:             {
166:                 Primed = false;
167:                 sampleCount = 0;
168:                 samplePrevious = 0;
169:                 samplePrevious2 = 0;
170:                 bandPassPrevious = 0;
171:                 bandPassPrevious2 = 0;
172:                 ratioPrevious = 0;
173:                 trendVigor = double.NaN;
174:                 for (int i = 0; i < rasterLength; ++i)
175:                     raster[i] = 0;
176:                 for (int i = 0; i < sampleBufferLength; ++i)
177:                     sampleBuffer[i] = 0;
178:                 if (!isSlave)
179:                     corona.Reset();
180:             }
181:         }
182:         #endregion
183: 
184:         #region Update
185:         private HeatMap Update(double sample, DateTime dateTime)
186:         {
187:             if (double.IsNaN(sample))
188:                 return null;
189: 
190:             Primed = isSlave ? corona.IsPrimed : corona.Update(sample);
191:             if (1 == ++sampleCount)
192:             {
193:                 samplePrevious = sample;
194:                 sampleBuffer[sampleBufferLastIndex] = sample;
195:                 return null;
196:             }
197: 
198:             // Filter bandpass InPhase component.
199:             double omega = Constants.TwoPi / corona.DominantCycleMedian;
200:             double beta2 = Math.Cos(omega);
201:             double gamma2 = 1 / Math.Cos(omega * 2 * 0.1);
202:             double alpha2 = gamma2 - Math.Sqrt(gamma2 * gamma2 - 1);
203:             double bandPass = 0.5 * (1 - alpha2) * (sample - samplePrevious2) + beta2 * (1 + alpha2) * bandPassPrevious - alpha2 * bandPassPrevious2;
204:             // Quadrature component is derivative of InPhase component divided by omega.
205:             double quadrature2 = (bandPass - bandPassPrevious) / omega;
206:             bandPassPrevious2 = bandPassPrevious;
207:             bandPassPrevious = bandPass;
208:             samplePrevious2 = samplePrevious;
209:             samplePrevious = sample;
210: 
211:             for (int i = 0; i < sampleBufferLastIndex; )
212:                 sampleBuffer[i] = sampleBuffer[++i];
213:             sampleBuffer[sampleBufferLastIndex] = sample;
214:             // Pythagorean theorem to establish cycle amplitude.
215:             double amplitude2 = Math.Sqrt(bandPass * bandPass + quadrature2 * quadrature2);
216:             // Trend amplitude taken over the cycle period.
217:             var cyclePeriod = (int)(corona.DominantCycleMedian - 1);
218:             if (cyclePeriod < sampleBufferLength)
219:                 cyclePeriod = sampleBufferLength;
220:             if (cyclePeriod > sampleBufferLength)
221:                 cyclePeriod = sampleBufferLength;
222:             double trend = sample - sampleBuffer[sampleBufferLength - Math.Min(cyclePeriod, sampleCount)];
223:             double ratio = (Math.Abs(trend) > double.Epsilon && amplitude2 > double.Epsilon) ?
224:                 (0.33 * trend / amplitude2 + 0.67 * ratioPrevious) : 0;
225:             if (ratio > 10)
226:                 ratio = 10;
227:             if (ratio < -10)
228:                 ratio = -10;
229:             ratioPrevious = ratio;
230:             // ratio ∈ [-10, 10], vigor ∈ [0, 1].
231:             double vigor = 0.05 * (ratio + 10);
232:             double width;
233:             if (vigor >= 0.3 && vigor < 0.5)
234:                 width = vigor - (0.3 - 0.01);
235:             else if (vigor >= 0.5 && vigor <= 0.7)
236:                 width = (0.7 + 0.01) - vigor;
237:             else
238:                 width = 0.01;
239:             trendVigor = (MaxParameterValue - MinParameterValue) * vigor + MinParameterValue;
240:             var vigorScaledToRasterLength = (int)Math.Round(rasterLength * vigor);
241:             double vigorScaledToMaxRasterValue = vigor * maxRasterValue;
242:             for (int i = 0; i < rasterLength; ++i)
243:             //Parallel.For(0, rasterLength, i =>
244:             {
245:                 double value = raster[i];
246:                 if (i == vigorScaledToRasterLength)
247:                     value *= 0.5;
248:                 else
249:                 {
250:                     double argument = vigorScaledToMaxRasterValue - rasterStep * i;
251:                     if (i > vigorScaledToRasterLength)
252:                         argument = -argument;
253:                     value = 0.8 * (Math.Pow(argument / width, 0.85) + 0.2 * value);
254:                 }
255:                 if (value < 0)
256:                     value = 0;
257:                 else if (value > maxRasterValue || vigor < 0.3 || vigor > 0.7)
258:                     value = maxRasterValue;
259:                 raster[i] = value;
260:             }//);
261:             double[] intensity;
262:             if (Primed)
263:             {
264:                 intensity = new double[rasterLength];//+1
265:                 for (int i = 0; i < rasterLength; ++i)
266:                 {
267:                     double value = raster[i];
268:                     intensity[i] = value;
269:                 }
270:             }
271:             else
272:             {
273:                 intensity = null;
274:             }
275:             return new HeatMap(dateTime, intensity);
276:         }
277: 
278:         /// <summary>
279:         /// Updates the value of the indicator.
280:         /// </summary>
281:         /// <param name="ohlcv">A new ohlcv.</param>
282:         /// <returns>The new heat-map column of the indicator or <c>null</c>.</returns>
283:         public HeatMap Update(Ohlcv ohlcv)
284:         {
285:             lock (Lock)
286:             {
287:                 return Update(ohlcv.Component(OhlcvComponent), ohlcv.Time);
288:             }
289:         }
290: 
291:         /// <summary>
292:         /// Updates the value of the indicator.
293:         /// </summary>
294:         /// <param name="scalar">A new scalar.</param>
295:         /// <returns>The new heat-map column of the indicator or <c>null</c>.</returns>
296:         public HeatMap Update(Scalar scalar)
297:         {
298:             lock (Lock)
299:             {
300:                 return Update(scalar.Value, scalar.Time);
301:             }
302:         }
303:         #endregion
304:     }
305: }

(End of file - total 305 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 12.5s)

_Thinking:_

 So CoronaTrendVigor uses a rolling buffer of sixty samples with the same bandpass filter as CSwing, then calculates amplitude from the combined BP and Q components, and applies a cycle period adjustment based on the dominant cycle median.

The issue is that the cycle period gets clamped to the buffer length, which defeats the purpose—I should use the dominant cycle median directly instead. From there, the trend is computed as the difference between the current sample and a historical one, smoothed into a ratio, then converted to a vigor value between 0 and 1 that drives the final trend vigor output. The raster updates use a weighted combination of power and value, with special saturation behavior when vigor is outside the 0.3 to 0.7 range. I need to expose the buffer size constant from the corona helper so it's configurable.

**Tool: grep**

**Input:**
```json
{
  "pattern": "MaximalPeriod|maximalPeriod|periodTwice|TimesTwo",
  "path": "/home/dev/zpano/go/indicators/johnehlers/corona"
}
```

**Output:**
```
Found 35 matches
/home/dev/zpano/go/indicators/johnehlers/corona/corona_smoke_test.go:
  Line 47: // values inside the expected [MinimalPeriod, MaximalPeriod] band.
  Line 59: 	if c.MinimalPeriodTimesTwo() != 12 || c.MaximalPeriodTimesTwo() != 60 {
  Line 61: 			c.MinimalPeriodTimesTwo(), c.MaximalPeriodTimesTwo())
  Line 76: 	if primedAt+1 != c.MinimalPeriodTimesTwo() {
  Line 78: 			primedAt+1, c.MinimalPeriodTimesTwo())
  Line 92: 	max := float64(c.MaximalPeriod())
  Line 169: 		{"max <= min", Params{MinimalPeriod: 10, MaximalPeriod: 10}},

/home/dev/zpano/go/indicators/johnehlers/corona/corona.go:
  Line 13: 	defaultMaximalPeriod          = 30
  Line 67: 	maximalPeriod          int
  Line 68: 	minimalPeriodTimesTwo  int
  Line 69: 	maximalPeriodTimesTwo  int
  Line 78: 	// Pre-calculated cos(4π/n) for n = MinimalPeriodTimesTwo..MaximalPeriodTimesTwo.
  Line 125: 		maximalPeriod:          cfg.MaximalPeriod,
  Line 126: 		minimalPeriodTimesTwo:  cfg.MinimalPeriod * 2,
  Line 127: 		maximalPeriodTimesTwo:  cfg.MaximalPeriod * 2,
  Line 132: 	c.filterBankLength = c.maximalPeriodTimesTwo - c.minimalPeriodTimesTwo + 1
  Line 152: 		n := c.minimalPeriodTimesTwo + idx
  Line 167: 	if p.MaximalPeriod <= 0 {
  Line 168: 		p.MaximalPeriod = defaultMaximalPeriod
  Line 190: 	if p.MaximalPeriod <= p.MinimalPeriod {
  Line 191: 		return fmt.Errorf(fmts, invalid, "MaximalPeriod should be > MinimalPeriod")
  Line 206: // MaximalPeriod returns the maximum cycle period covered by the filter bank.
  Line 207: func (c *Corona) MaximalPeriod() int { return c.maximalPeriod }
  Line 209: // MinimalPeriodTimesTwo returns the minimum filter-bank half-period index.
  Line 210: func (c *Corona) MinimalPeriodTimesTwo() int { return c.minimalPeriodTimesTwo }
  Line 212: // MaximalPeriodTimesTwo returns the maximum filter-bank half-period index.
  Line 213: func (c *Corona) MaximalPeriodTimesTwo() int { return c.maximalPeriodTimesTwo }
  Line 222: // meaningful output (sampleCount >= MinimalPeriodTimesTwo).
  Line 297: 		n := c.minimalPeriodTimesTwo + idx
  Line 362: 			n := float64(c.minimalPeriodTimesTwo + idx)
  Line 393: 	if c.sampleCount < c.minimalPeriodTimesTwo {

/home/dev/zpano/go/indicators/johnehlers/corona/params.go:
  Line 16: 	// MaximalPeriod is the maximum cycle period (in bars) covered by the
  Line 18: 	MaximalPeriod int
  Line 36: 		MaximalPeriod:          30,

/home/dev/zpano/go/indicators/johnehlers/corona/doc.go:
  Line 14: //     [minimalPeriod, maximalPeriod] (default [6, 30]) at half-period
```

---

## Assistant (Build · claude-opus-4.7 · 10.4s)

_Thinking:_

 Building the CoronaTrendVigor component based on the CSwingPosition template, with the key adjustment of using the cycle period directly from the dominant cycle median minus one, bypassing the buffer length clamp that was constraining it before. I'm checking how the sample buffer gets initialized and used throughout the implementation.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingpositionoutput.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingpositionoutput.go</path>
<type>file</type>
<content>1: package coronaswingposition
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
12: 	// Value is the Corona swing position heatmap column.
13: 	Value Output = iota + 1
14: 	// SwingPosition is the scalar swing position in [MinParameterValue, MaxParameterValue].
15: 	SwingPosition
16: 	outputLast
17: )
18: 
19: const (
20: 	valueStr         = "value"
21: 	swingPositionStr = "swingPosition"
22: 	unknownStr       = "unknown"
23: )
24: 
25: // String implements the Stringer interface.
26: func (o Output) String() string {
27: 	switch o {
28: 	case Value:
29: 		return valueStr
30: 	case SwingPosition:
31: 		return swingPositionStr
32: 	default:
33: 		return unknownStr
34: 	}
35: }
36: 
37: // IsKnown determines if this output is known.
38: func (o Output) IsKnown() bool {
39: 	return o >= Value && o < outputLast
40: }
41: 
42: // MarshalJSON implements the Marshaler interface.
43: func (o Output) MarshalJSON() ([]byte, error) {
44: 	const (
45: 		errFmt = "cannot marshal '%s': unknown corona swing position output"
46: 		extra  = 2
47: 		dqc    = '"'
48: 	)
49: 
50: 	s := o.String()
51: 	if s == unknownStr {
52: 		return nil, fmt.Errorf(errFmt, s)
53: 	}
54: 
55: 	b := make([]byte, 0, len(s)+extra)
56: 	b = append(b, dqc)
57: 	b = append(b, s...)
58: 	b = append(b, dqc)
59: 
60: 	return b, nil
61: }
62: 
63: // UnmarshalJSON implements the Unmarshaler interface.
64: func (o *Output) UnmarshalJSON(data []byte) error {
65: 	const (
66: 		errFmt = "cannot unmarshal '%s': unknown corona swing position output"
67: 		dqs    = "\""
68: 	)
69: 
70: 	d := bytes.Trim(data, dqs)
71: 	s := string(d)
72: 
73: 	switch s {
74: 	case valueStr:
75: 		*o = Value
76: 	case swingPositionStr:
77: 		*o = SwingPosition
78: 	default:
79: 		return fmt.Errorf(errFmt, s)
80: 	}
81: 
82: 	return nil
83: }

(End of file - total 83 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.5s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingpositionparams.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingpositionparams.go</path>
<type>file</type>
<content>1: package coronaswingposition
2: 
3: import "zpano/entities"
4: 
5: // Params describes parameters to create an instance of the CoronaSwingPosition indicator.
6: type Params struct {
7: 	// RasterLength is the number of elements in the heatmap raster. The default value is 50.
8: 	RasterLength int
9: 
10: 	// MaxRasterValue is the maximal raster value (z) of the heatmap. The default value is 20.
11: 	MaxRasterValue float64
12: 
13: 	// MinParameterValue is the minimal ordinate (y) value of the heatmap. The default value is -5.
14: 	MinParameterValue float64
15: 
16: 	// MaxParameterValue is the maximal ordinate (y) value of the heatmap. The default value is 5.
17: 	MaxParameterValue float64
18: 
19: 	// HighPassFilterCutoff is the high-pass filter cutoff (de-trending period) used by
20: 	// the inner Corona engine. Suggested values are 20, 30, 100. The default value is 30.
21: 	HighPassFilterCutoff int
22: 
23: 	// MinimalPeriod is the minimal period of the inner Corona engine. The default value is 6.
24: 	MinimalPeriod int
25: 
26: 	// MaximalPeriod is the maximal period of the inner Corona engine. The default value is 30.
27: 	MaximalPeriod int
28: 
29: 	// BarComponent indicates the component of a bar to use when updating the indicator with a bar sample.
30: 	//
31: 	// If zero, the default (BarMedianPrice) is used, matching Ehlers' reference which operates on
32: 	// (High+Low)/2. Since this differs from the framework-wide default, it is always shown in the
33: 	// indicator mnemonic.
34: 	BarComponent entities.BarComponent
35: 
36: 	// QuoteComponent indicates the component of a quote to use when updating the indicator with a quote sample.
37: 	//
38: 	// If zero, the default (QuoteMidPrice) is used and the component is not shown in the indicator mnemonic.
39: 	QuoteComponent entities.QuoteComponent
40: 
41: 	// TradeComponent indicates the component of a trade to use when updating the indicator with a trade sample.
42: 	//
43: 	// If zero, the default (TradePrice) is used and the component is not shown in the indicator mnemonic.
44: 	TradeComponent entities.TradeComponent
45: }

(End of file - total 45 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 9.5s)

Now let me create the CoronaTrendVigor package. Start with params and output files:

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigorparams.go",
  "content": "package coronatrendvigor\n\nimport \"zpano/entities\"\n\n// Params describes parameters to create an instance of the CoronaTrendVigor indicator.\ntype Params struct {\n\t// RasterLength is the number of elements in the heatmap raster. The default value is 50.\n\tRasterLength int\n\n\t// MaxRasterValue is the maximal raster value (z) of the heatmap. The default value is 20.\n\tMaxRasterValue float64\n\n\t// MinParameterValue is the minimal ordinate (y) value of the heatmap. The default value is -10.\n\tMinParameterValue float64\n\n\t// MaxParameterValue is the maximal ordinate (y) value of the heatmap. The default value is 10.\n\tMaxParameterValue float64\n\n\t// HighPassFilterCutoff is the high-pass filter cutoff (de-trending period) used by\n\t// the inner Corona engine. Suggested values are 20, 30, 100. The default value is 30.\n\tHighPassFilterCutoff int\n\n\t// MinimalPeriod is the minimal period of the inner Corona engine. The default value is 6.\n\tMinimalPeriod int\n\n\t// MaximalPeriod is the maximal period of the inner Corona engine. The default value is 30.\n\tMaximalPeriod int\n\n\t// BarComponent indicates the component of a bar to use when updating the indicator with a bar sample.\n\t//\n\t// If zero, the default (BarMedianPrice) is used, matching Ehlers' reference which operates on\n\t// (High+Low)/2. Since this differs from the framework-wide default, it is always shown in the\n\t// indicator mnemonic.\n\tBarComponent entities.BarComponent\n\n\t// QuoteComponent indicates the component of a quote to use when updating the indicator with a quote sample.\n\t//\n\t// If zero, the default (QuoteMidPrice) is used and the component is not shown in the indicator mnemonic.\n\tQuoteComponent entities.QuoteComponent\n\n\t// TradeComponent indicates the component of a trade to use when updating the indicator with a trade sample.\n\t//\n\t// If zero, the default (TradePrice) is used and the component is not shown in the indicator mnemonic.\n\tTradeComponent entities.TradeComponent\n}\n"
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

## Assistant (Build · claude-opus-4.7 · 11.3s)

**Tool: write**

**Input:**
```json
{
  "content": "package coronatrendvigor\n\nimport (\n\t\"bytes\"\n\t\"fmt\"\n)\n\n// Output describes the outputs of the indicator.\ntype Output int\n\nconst (\n\t// Value is the Corona trend vigor heatmap column.\n\tValue Output = iota + 1\n\t// TrendVigor is the scalar trend vigor in [MinParameterValue, MaxParameterValue].\n\tTrendVigor\n\toutputLast\n)\n\nconst (\n\tvalueStr      = \"value\"\n\ttrendVigorStr = \"trendVigor\"\n\tunknownStr    = \"unknown\"\n)\n\n// String implements the Stringer interface.\nfunc (o Output) String() string {\n\tswitch o {\n\tcase Value:\n\t\treturn valueStr\n\tcase TrendVigor:\n\t\treturn trendVigorStr\n\tdefault:\n\t\treturn unknownStr\n\t}\n}\n\n// IsKnown determines if this output is known.\nfunc (o Output) IsKnown() bool {\n\treturn o >= Value && o < outputLast\n}\n\n// MarshalJSON implements the Marshaler interface.\nfunc (o Output) MarshalJSON() ([]byte, error) {\n\tconst (\n\t\terrFmt = \"cannot marshal '%s': unknown corona trend vigor output\"\n\t\textra  = 2\n\t\tdqc    = '\"'\n\t)\n\n\ts := o.String()\n\tif s == unknownStr {\n\t\treturn nil, fmt.Errorf(errFmt, s)\n\t}\n\n\tb := make([]byte, 0, len(s)+extra)\n\tb = append(b, dqc)\n\tb = append(b, s...)\n\tb = append(b, dqc)\n\n\treturn b, nil\n}\n\n// UnmarshalJSON implements the Unmarshaler interface.\nfunc (o *Output) UnmarshalJSON(data []byte) error {\n\tconst (\n\t\terrFmt = \"cannot unmarshal '%s': unknown corona trend vigor output\"\n\t\tdqs    = \"\\\"\"\n\t)\n\n\td := bytes.Trim(data, dqs)\n\ts := string(d)\n\n\tswitch s {\n\tcase valueStr:\n\t\t*o = Value\n\tcase trendVigorStr:\n\t\t*o = TrendVigor\n\tdefault:\n\t\treturn fmt.Errorf(errFmt, s)\n\t}\n\n\treturn nil\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigoroutput.go"
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

## Assistant (Build · claude-opus-4.7 · 3.6s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingpositionoutput_test.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/coronaswingposition/coronaswingpositionoutput_test.go</path>
<type>file</type>
<content>1: //nolint:testpackage
2: package coronaswingposition
3: 
4: import "testing"
5: 
6: func TestOutputString(t *testing.T) {
7: 	t.Parallel()
8: 
9: 	tests := []struct {
10: 		o    Output
11: 		text string
12: 	}{
13: 		{Value, valueStr},
14: 		{SwingPosition, swingPositionStr},
15: 		{outputLast, unknownStr},
16: 		{Output(0), unknownStr},
17: 		{Output(9999), unknownStr},
18: 		{Output(-9999), unknownStr},
19: 	}
20: 
21: 	for _, tt := range tests {
22: 		if tt.text != tt.o.String() {
23: 			t.Errorf("'%v'.String(): expected '%v', actual '%v'", tt.o, tt.text, tt.o.String())
24: 		}
25: 	}
26: }
27: 
28: func TestOutputIsKnown(t *testing.T) {
29: 	t.Parallel()
30: 
31: 	tests := []struct {
32: 		o  Output
33: 		ok bool
34: 	}{
35: 		{Value, true},
36: 		{SwingPosition, true},
37: 		{outputLast, false},
38: 		{Output(0), false},
39: 		{Output(9999), false},
40: 		{Output(-9999), false},
41: 	}
42: 
43: 	for _, tt := range tests {
44: 		if tt.ok != tt.o.IsKnown() {
45: 			t.Errorf("'%v'.IsKnown(): expected %v, actual %v", tt.o, tt.ok, tt.o.IsKnown())
46: 		}
47: 	}
48: }
49: 
50: func TestOutputMarshalJSON(t *testing.T) {
51: 	t.Parallel()
52: 
53: 	const dqs = "\""
54: 
55: 	var nilstr string
56: 	tests := []struct {
57: 		o         Output
58: 		json      string
59: 		succeeded bool
60: 	}{
61: 		{Value, dqs + valueStr + dqs, true},
62: 		{SwingPosition, dqs + swingPositionStr + dqs, true},
63: 		{outputLast, nilstr, false},
64: 		{Output(9999), nilstr, false},
65: 		{Output(-9999), nilstr, false},
66: 		{Output(0), nilstr, false},
67: 	}
68: 
69: 	for _, tt := range tests {
70: 		bs, err := tt.o.MarshalJSON()
71: 
72: 		if err != nil && tt.succeeded {
73: 			t.Errorf("'%v'.MarshalJSON(): expected success, got error %v", tt.o, err)
74: 
75: 			continue
76: 		}
77: 
78: 		if err == nil && !tt.succeeded {
79: 			t.Errorf("'%v'.MarshalJSON(): expected error, got success", tt.o)
80: 
81: 			continue
82: 		}
83: 
84: 		if string(bs) != tt.json {
85: 			t.Errorf("'%v'.MarshalJSON(): expected '%v', actual '%v'", tt.o, tt.json, string(bs))
86: 		}
87: 	}
88: }
89: 
90: func TestOutputUnmarshalJSON(t *testing.T) {
91: 	t.Parallel()
92: 
93: 	const dqs = "\""
94: 
95: 	var zero Output
96: 	tests := []struct {
97: 		o         Output
98: 		json      string
99: 		succeeded bool
100: 	}{
101: 		{Value, dqs + valueStr + dqs, true},
102: 		{SwingPosition, dqs + swingPositionStr + dqs, true},
103: 		{zero, dqs + unknownStr + dqs, false},
104: 		{zero, dqs + "foobar" + dqs, false},
105: 	}
106: 
107: 	for _, tt := range tests {
108: 		var o Output
109: 
110: 		err := o.UnmarshalJSON([]byte(tt.json))
111: 		if err != nil && tt.succeeded {
112: 			t.Errorf("UnmarshalJSON('%v'): expected success, got error %v", tt.json, err)
113: 
114: 			continue
115: 		}
116: 
117: 		if err == nil && !tt.succeeded {
118: 			t.Errorf("UnmarshalJSON('%v'): expected error, got success", tt.json)
119: 
120: 			continue
121: 		}
122: 
123: 		if tt.o != o {
124: 			t.Errorf("UnmarshalJSON('%v'): expected %v, actual %v", tt.json, tt.o, o)
125: 		}
126: 	}
127: }

(End of file - total 127 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 14.0s)

**Tool: write**

**Input:**
```json
{
  "content": "//nolint:testpackage\npackage coronatrendvigor\n\nimport \"testing\"\n\nfunc TestOutputString(t *testing.T) {\n\tt.Parallel()\n\n\ttests := []struct {\n\t\to    Output\n\t\ttext string\n\t}{\n\t\t{Value, valueStr},\n\t\t{TrendVigor, trendVigorStr},\n\t\t{outputLast, unknownStr},\n\t\t{Output(0), unknownStr},\n\t\t{Output(9999), unknownStr},\n\t\t{Output(-9999), unknownStr},\n\t}\n\n\tfor _, tt := range tests {\n\t\tif tt.text != tt.o.String() {\n\t\t\tt.Errorf(\"'%v'.String(): expected '%v', actual '%v'\", tt.o, tt.text, tt.o.String())\n\t\t}\n\t}\n}\n\nfunc TestOutputIsKnown(t *testing.T) {\n\tt.Parallel()\n\n\ttests := []struct {\n\t\to  Output\n\t\tok bool\n\t}{\n\t\t{Value, true},\n\t\t{TrendVigor, true},\n\t\t{outputLast, false},\n\t\t{Output(0), false},\n\t\t{Output(9999), false},\n\t\t{Output(-9999), false},\n\t}\n\n\tfor _, tt := range tests {\n\t\tif tt.ok != tt.o.IsKnown() {\n\t\t\tt.Errorf(\"'%v'.IsKnown(): expected %v, actual %v\", tt.o, tt.ok, tt.o.IsKnown())\n\t\t}\n\t}\n}\n\nfunc TestOutputMarshalJSON(t *testing.T) {\n\tt.Parallel()\n\n\tconst dqs = \"\\\"\"\n\n\tvar nilstr string\n\ttests := []struct {\n\t\to         Output\n\t\tjson      string\n\t\tsucceeded bool\n\t}{\n\t\t{Value, dqs + valueStr + dqs, true},\n\t\t{TrendVigor, dqs + trendVigorStr + dqs, true},\n\t\t{outputLast, nilstr, false},\n\t\t{Output(9999), nilstr, false},\n\t\t{Output(-9999), nilstr, false},\n\t\t{Output(0), nilstr, false},\n\t}\n\n\tfor _, tt := range tests {\n\t\tbs, err := tt.o.MarshalJSON()\n\n\t\tif err != nil && tt.succeeded {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected success, got error %v\", tt.o, err)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif err == nil && !tt.succeeded {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected error, got success\", tt.o)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif string(bs) != tt.json {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected '%v', actual '%v'\", tt.o, tt.json, string(bs))\n\t\t}\n\t}\n}\n\nfunc TestOutputUnmarshalJSON(t *testing.T) {\n\tt.Parallel()\n\n\tconst dqs = \"\\\"\"\n\n\tvar zero Output\n\ttests := []struct {\n\t\to         Output\n\t\tjson      string\n\t\tsucceeded bool\n\t}{\n\t\t{Value, dqs + valueStr + dqs, true},\n\t\t{TrendVigor, dqs + trendVigorStr + dqs, true},\n\t\t{zero, dqs + unknownStr + dqs, false},\n\t\t{zero, dqs + \"foobar\" + dqs, false},\n\t}\n\n\tfor _, tt := range tests {\n\t\tvar o Output\n\n\t\terr := o.UnmarshalJSON([]byte(tt.json))\n\t\tif err != nil && tt.succeeded {\n\t\t\tt.Errorf(\"UnmarshalJSON('%v'): expected success, got error %v\", tt.json, err)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif err == nil && !tt.succeeded {\n\t\t\tt.Errorf(\"UnmarshalJSON('%v'): expected error, got success\", tt.json)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif tt.o != o {\n\t\t\tt.Errorf(\"UnmarshalJSON('%v'): expected %v, actual %v\", tt.json, tt.o, o)\n\t\t}\n\t}\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigoroutput_test.go"
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

## Assistant (Build · claude-opus-4.7 · 88.0s)

_Thinking:_

 Now I'm implementing the core bandpass filter algorithm that uses the dominant cycle period to calculate the filter coefficients, applying a series of trigonometric transformations to smooth the price data while preserving the cyclical components.

Continuing with the indicator calculations...

I need to handle the first bar specially—on the initial call, I'll store the sample in the buffer and return early, similar to how CSwing works. Then on subsequent bars, I'll shift the buffer left and append the new sample at the end. Let me trace through the buffer mechanics to make sure the shifting logic is correct.

Now I'm working through the trend calculation—with a 60-element buffer and the cycle period, I'll index back the appropriate number of bars to get the previous sample, then compute the difference. I should also account for the output scaling based on the raster values.

Writing the implementation...

**Tool: write**

**Input:**
```json
{
  "content": "// Package coronatrendvigor implements Ehlers' Corona Trend Vigor heatmap\n// indicator.\n//\n// The Corona trend vigor is computed as the slope of the momentum taken over a\n// full dominant cycle period, normalised by the cycle amplitude. The ratio is\n// scaled into the range [-10, 10]. A value of ±2 means the trend slope equals\n// twice the cycle amplitude; values between -2 and +2 form the \"corona\" and\n// suggest the trend should not be traded.\n//\n// Reference: John Ehlers, \"Measuring Cycle Periods\", Stocks & Commodities,\n// November 2008.\npackage coronatrendvigor\n\n//nolint: gofumpt\nimport (\n\t\"fmt\"\n\t\"math\"\n\t\"sync\"\n\t\"time\"\n\n\t\"zpano/entities\"\n\t\"zpano/indicators/core\"\n\t\"zpano/indicators/core/outputs\"\n\t\"zpano/indicators/johnehlers/corona\"\n)\n\nconst (\n\t// Bandpass filter delta factor in γ = 1/cos(ω · 2 · 0.1).\n\tbpDelta = 0.1\n\n\t// Ratio EMA coefficients (0.33 * current + 0.67 * previous).\n\tratioNewCoef  = 0.33\n\tratioPrevCoef = 0.67\n\n\t// Vigor band thresholds.\n\tvigorMidLow  = 0.3\n\tvigorMidHigh = 0.7\n\tvigorMid     = 0.5\n\twidthEdge    = 0.01\n\n\t// Raster update blend factors.\n\trasterBlendScale    = 0.8\n\trasterBlendPrevious = 0.2\n\trasterBlendHalf     = 0.5\n\trasterBlendExponent = 0.85\n\n\t// Ratio clamp bounds.\n\tratioLimit = 10.0\n\n\t// vigor = vigorScale*(ratio+ratioLimit); with ratioLimit=10 and range 20,\n\t// vigorScale=0.05 maps ratio ∈ [-10,10] to vigor ∈ [0,1].\n\tvigorScale = 0.05\n)\n\n// CoronaTrendVigor is Ehlers' Corona Trend Vigor indicator.\ntype CoronaTrendVigor struct {\n\tmu                  sync.RWMutex\n\tmnemonic            string\n\tdescription         string\n\tmnemonicTV          string\n\tdescriptionTV       string\n\tc                   *corona.Corona\n\trasterLength        int\n\trasterStep          float64\n\tmaxRasterValue      float64\n\tminParameterValue   float64\n\tmaxParameterValue   float64\n\tparameterResolution float64\n\traster              []float64\n\tsampleBuffer        []float64\n\tsampleCount         int\n\tsamplePrev          float64\n\tsamplePrev2         float64\n\tbandPassPrev        float64\n\tbandPassPrev2       float64\n\tratioPrev           float64\n\ttrendVigor          float64\n\tbarFunc             entities.BarFunc\n\tquoteFunc           entities.QuoteFunc\n\ttradeFunc           entities.TradeFunc\n}\n\n// NewCoronaTrendVigorDefault returns an instance created with default parameters.\nfunc NewCoronaTrendVigorDefault() (*CoronaTrendVigor, error) {\n\treturn NewCoronaTrendVigorParams(&Params{})\n}\n\n// NewCoronaTrendVigorParams returns an instance created with the supplied parameters.\n//\n//nolint:funlen,cyclop\nfunc NewCoronaTrendVigorParams(p *Params) (*CoronaTrendVigor, error) {\n\tconst (\n\t\tinvalid      = \"invalid corona trend vigor parameters\"\n\t\tfmtRaster    = \"%s: RasterLength should be >= 2\"\n\t\tfmtMaxRaster = \"%s: MaxRasterValue should be > 0\"\n\t\tfmtMaxParam  = \"%s: MaxParameterValue should be > MinParameterValue\"\n\t\tfmtHP        = \"%s: HighPassFilterCutoff should be >= 2\"\n\t\tfmtMinP      = \"%s: MinimalPeriod should be >= 2\"\n\t\tfmtMaxP      = \"%s: MaximalPeriod should be > MinimalPeriod\"\n\t\tfmtw         = \"%s: %w\"\n\t\tfmtnVal      = \"ctv(%d, %g, %g, %g, %d%s)\"\n\t\tfmtnTV       = \"ctv-tv(%d%s)\"\n\t\tdescrVal     = \"Corona trend vigor \"\n\t\tdescrTV      = \"Corona trend vigor scalar \"\n\t\tdefRaster    = 50\n\t\tdefMaxRast   = 20.0\n\t\tdefMinParam  = -10.0\n\t\tdefMaxParam  = 10.0\n\t\tdefHPCutoff  = 30\n\t\tdefMinPer    = 6\n\t\tdefMaxPer    = 30\n\t)\n\n\tcfg := *p\n\n\tif cfg.RasterLength == 0 {\n\t\tcfg.RasterLength = defRaster\n\t}\n\n\tif cfg.MaxRasterValue == 0 {\n\t\tcfg.MaxRasterValue = defMaxRast\n\t}\n\n\t// MinParameterValue and MaxParameterValue default to -10 and 10; since 0 is a\n\t// valid user value for either, we only substitute when both are zero (the\n\t// unconfigured case).\n\tif cfg.MinParameterValue == 0 && cfg.MaxParameterValue == 0 {\n\t\tcfg.MinParameterValue = defMinParam\n\t\tcfg.MaxParameterValue = defMaxParam\n\t}\n\n\tif cfg.HighPassFilterCutoff == 0 {\n\t\tcfg.HighPassFilterCutoff = defHPCutoff\n\t}\n\n\tif cfg.MinimalPeriod == 0 {\n\t\tcfg.MinimalPeriod = defMinPer\n\t}\n\n\tif cfg.MaximalPeriod == 0 {\n\t\tcfg.MaximalPeriod = defMaxPer\n\t}\n\n\tif cfg.RasterLength < 2 {\n\t\treturn nil, fmt.Errorf(fmtRaster, invalid)\n\t}\n\n\tif cfg.MaxRasterValue <= 0 {\n\t\treturn nil, fmt.Errorf(fmtMaxRaster, invalid)\n\t}\n\n\tif cfg.MaxParameterValue <= cfg.MinParameterValue {\n\t\treturn nil, fmt.Errorf(fmtMaxParam, invalid)\n\t}\n\n\tif cfg.HighPassFilterCutoff < 2 {\n\t\treturn nil, fmt.Errorf(fmtHP, invalid)\n\t}\n\n\tif cfg.MinimalPeriod < 2 {\n\t\treturn nil, fmt.Errorf(fmtMinP, invalid)\n\t}\n\n\tif cfg.MaximalPeriod <= cfg.MinimalPeriod {\n\t\treturn nil, fmt.Errorf(fmtMaxP, invalid)\n\t}\n\n\tbc := cfg.BarComponent\n\tif bc == 0 {\n\t\tbc = entities.BarMedianPrice\n\t}\n\n\tqc := cfg.QuoteComponent\n\tif qc == 0 {\n\t\tqc = entities.DefaultQuoteComponent\n\t}\n\n\ttc := cfg.TradeComponent\n\tif tc == 0 {\n\t\ttc = entities.DefaultTradeComponent\n\t}\n\n\tc, err := corona.NewCorona(&corona.Params{\n\t\tHighPassFilterCutoff: cfg.HighPassFilterCutoff,\n\t\tMinimalPeriod:        cfg.MinimalPeriod,\n\t\tMaximalPeriod:        cfg.MaximalPeriod,\n\t})\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\tbarFunc, err := entities.BarComponentFunc(bc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\tquoteFunc, err := entities.QuoteComponentFunc(qc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\ttradeFunc, err := entities.TradeComponentFunc(tc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\tcomponentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)\n\n\tmnemonicVal := fmt.Sprintf(fmtnVal,\n\t\tcfg.RasterLength, cfg.MaxRasterValue, cfg.MinParameterValue, cfg.MaxParameterValue,\n\t\tcfg.HighPassFilterCutoff, componentMnemonic)\n\tmnemonicTV := fmt.Sprintf(fmtnTV, cfg.HighPassFilterCutoff, componentMnemonic)\n\n\tparamResolution := float64(cfg.RasterLength-1) / (cfg.MaxParameterValue - cfg.MinParameterValue)\n\n\treturn &CoronaTrendVigor{\n\t\tmnemonic:            mnemonicVal,\n\t\tdescription:         descrVal + mnemonicVal,\n\t\tmnemonicTV:          mnemonicTV,\n\t\tdescriptionTV:       descrTV + mnemonicTV,\n\t\tc:                   c,\n\t\trasterLength:        cfg.RasterLength,\n\t\trasterStep:          cfg.MaxRasterValue / float64(cfg.RasterLength),\n\t\tmaxRasterValue:      cfg.MaxRasterValue,\n\t\tminParameterValue:   cfg.MinParameterValue,\n\t\tmaxParameterValue:   cfg.MaxParameterValue,\n\t\tparameterResolution: paramResolution,\n\t\traster:              make([]float64, cfg.RasterLength),\n\t\tsampleBuffer:        make([]float64, c.MaximalPeriodTimesTwo()),\n\t\ttrendVigor:          math.NaN(),\n\t\tbarFunc:             barFunc,\n\t\tquoteFunc:           quoteFunc,\n\t\ttradeFunc:           tradeFunc,\n\t}, nil\n}\n\n// IsPrimed indicates whether the indicator is primed.\nfunc (s *CoronaTrendVigor) IsPrimed() bool {\n\ts.mu.RLock()\n\tdefer s.mu.RUnlock()\n\n\treturn s.c.IsPrimed()\n}\n\n// Metadata describes the output data of the indicator.\nfunc (s *CoronaTrendVigor) Metadata() core.Metadata {\n\treturn core.Metadata{\n\t\tType:        core.CoronaTrendVigor,\n\t\tMnemonic:    s.mnemonic,\n\t\tDescription: s.description,\n\t\tOutputs: []outputs.Metadata{\n\t\t\t{Kind: int(Value), Type: outputs.HeatmapType, Mnemonic: s.mnemonic, Description: s.description},\n\t\t\t{Kind: int(TrendVigor), Type: outputs.ScalarType, Mnemonic: s.mnemonicTV, Description: s.descriptionTV},\n\t\t},\n\t}\n}\n\n// Update feeds the next sample and returns the heatmap column plus the current\n// TrendVigor. On unprimed bars the heatmap is empty and the scalar is NaN.\n//\n//nolint:funlen,cyclop,gocognit\nfunc (s *CoronaTrendVigor) Update(sample float64, t time.Time) (*outputs.Heatmap, float64) {\n\ts.mu.Lock()\n\tdefer s.mu.Unlock()\n\n\tif math.IsNaN(sample) {\n\t\treturn outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()\n\t}\n\n\tprimed := s.c.Update(sample)\n\ts.sampleCount++\n\n\tbufLast := len(s.sampleBuffer) - 1\n\n\tif s.sampleCount == 1 {\n\t\ts.samplePrev = sample\n\t\ts.sampleBuffer[bufLast] = sample\n\n\t\treturn outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()\n\t}\n\n\t// Bandpass InPhase filter at the dominant cycle median period.\n\tomega := 2. * math.Pi / s.c.DominantCycleMedian()\n\tbeta2 := math.Cos(omega)\n\tgamma2 := 1. / math.Cos(omega*2*bpDelta)\n\talpha2 := gamma2 - math.Sqrt(gamma2*gamma2-1.)\n\tbandPass := 0.5*(1-alpha2)*(sample-s.samplePrev2) +\n\t\tbeta2*(1+alpha2)*s.bandPassPrev -\n\t\talpha2*s.bandPassPrev2\n\n\t// Quadrature = derivative / omega.\n\tquadrature2 := (bandPass - s.bandPassPrev) / omega\n\n\ts.bandPassPrev2 = s.bandPassPrev\n\ts.bandPassPrev = bandPass\n\ts.samplePrev2 = s.samplePrev\n\ts.samplePrev = sample\n\n\t// Left-shift sampleBuffer and append the new sample.\n\tfor i := 0; i < bufLast; i++ {\n\t\ts.sampleBuffer[i] = s.sampleBuffer[i+1]\n\t}\n\n\ts.sampleBuffer[bufLast] = sample\n\n\t// Cycle amplitude.\n\tamplitude2 := math.Sqrt(bandPass*bandPass + quadrature2*quadrature2)\n\n\t// Trend amplitude taken over the cycle period. Use DominantCycleMedian-1\n\t// directly (the MBST implementation clamps to sampleBuffer length which\n\t// negates the intent; see the reference impls).\n\tcyclePeriod := int(s.c.DominantCycleMedian() - 1)\n\tif cyclePeriod > len(s.sampleBuffer) {\n\t\tcyclePeriod = len(s.sampleBuffer)\n\t}\n\n\tlookback := cyclePeriod\n\tif s.sampleCount < lookback {\n\t\tlookback = s.sampleCount\n\t}\n\n\ttrend := sample - s.sampleBuffer[len(s.sampleBuffer)-lookback]\n\n\tratio := 0.0\n\tif math.Abs(trend) > 0 && amplitude2 > 0 {\n\t\tratio = ratioNewCoef*trend/amplitude2 + ratioPrevCoef*s.ratioPrev\n\t}\n\n\tif ratio > ratioLimit {\n\t\tratio = ratioLimit\n\t} else if ratio < -ratioLimit {\n\t\tratio = -ratioLimit\n\t}\n\n\ts.ratioPrev = ratio\n\n\t// ratio ∈ [-10, 10] ⇒ vigor ∈ [0, 1].\n\tvigor := vigorScale * (ratio + ratioLimit)\n\n\tvar width float64\n\n\tswitch {\n\tcase vigor >= vigorMidLow && vigor < vigorMid:\n\t\twidth = vigor - (vigorMidLow - widthEdge)\n\tcase vigor >= vigorMid && vigor <= vigorMidHigh:\n\t\twidth = (vigorMidHigh + widthEdge) - vigor\n\tdefault:\n\t\twidth = widthEdge\n\t}\n\n\ts.trendVigor = (s.maxParameterValue-s.minParameterValue)*vigor + s.minParameterValue\n\n\tvigorScaledToRasterLength := int(math.Round(float64(s.rasterLength) * vigor))\n\tvigorScaledToMaxRasterValue := vigor * s.maxRasterValue\n\n\tfor i := 0; i < s.rasterLength; i++ {\n\t\tvalue := s.raster[i]\n\n\t\tif i == vigorScaledToRasterLength {\n\t\t\tvalue *= rasterBlendHalf\n\t\t} else {\n\t\t\targument := vigorScaledToMaxRasterValue - s.rasterStep*float64(i)\n\t\t\tif i > vigorScaledToRasterLength {\n\t\t\t\targument = -argument\n\t\t\t}\n\n\t\t\tif width > 0 {\n\t\t\t\tvalue = rasterBlendScale *\n\t\t\t\t\t(math.Pow(argument/width, rasterBlendExponent) + rasterBlendPrevious*value)\n\t\t\t}\n\t\t}\n\n\t\tswitch {\n\t\tcase value < 0:\n\t\t\tvalue = 0\n\t\tcase value > s.maxRasterValue, vigor < vigorMidLow, vigor > vigorMidHigh:\n\t\t\tvalue = s.maxRasterValue\n\t\t}\n\n\t\tif math.IsNaN(value) {\n\t\t\tvalue = 0\n\t\t}\n\n\t\ts.raster[i] = value\n\t}\n\n\tif !primed {\n\t\treturn outputs.NewEmptyHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution), math.NaN()\n\t}\n\n\tvalues := make([]float64, s.rasterLength)\n\tvalueMin := math.Inf(1)\n\tvalueMax := math.Inf(-1)\n\n\tfor i := 0; i < s.rasterLength; i++ {\n\t\tv := s.raster[i]\n\t\tvalues[i] = v\n\n\t\tif v < valueMin {\n\t\t\tvalueMin = v\n\t\t}\n\n\t\tif v > valueMax {\n\t\t\tvalueMax = v\n\t\t}\n\t}\n\n\theatmap := outputs.NewHeatmap(t, s.minParameterValue, s.maxParameterValue, s.parameterResolution,\n\t\tvalueMin, valueMax, values)\n\n\treturn heatmap, s.trendVigor\n}\n\n// UpdateScalar updates the indicator given the next scalar sample.\nfunc (s *CoronaTrendVigor) UpdateScalar(sample *entities.Scalar) core.Output {\n\treturn s.updateEntity(sample.Time, sample.Value)\n}\n\n// UpdateBar updates the indicator given the next bar sample.\nfunc (s *CoronaTrendVigor) UpdateBar(sample *entities.Bar) core.Output {\n\treturn s.updateEntity(sample.Time, s.barFunc(sample))\n}\n\n// UpdateQuote updates the indicator given the next quote sample.\nfunc (s *CoronaTrendVigor) UpdateQuote(sample *entities.Quote) core.Output {\n\treturn s.updateEntity(sample.Time, s.quoteFunc(sample))\n}\n\n// UpdateTrade updates the indicator given the next trade sample.\nfunc (s *CoronaTrendVigor) UpdateTrade(sample *entities.Trade) core.Output {\n\treturn s.updateEntity(sample.Time, s.tradeFunc(sample))\n}\n\nfunc (s *CoronaTrendVigor) updateEntity(t time.Time, sample float64) core.Output {\n\tconst length = 2\n\n\theatmap, tv := s.Update(sample, t)\n\n\toutput := make([]any, length)\n\toutput[0] = heatmap\n\toutput[1] = entities.Scalar{Time: t, Value: tv}\n\n\treturn output\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor.go"
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

## Assistant (Build · claude-opus-4.7 · 74.5s)

Now the test file:

**Tool: write**

**Input:**
```json
{
  "content": "//nolint:testpackage\npackage coronatrendvigor\n\nimport (\n\t\"math\"\n\t\"testing\"\n\t\"time\"\n\n\t\"zpano/entities\"\n\t\"zpano/indicators/core\"\n\t\"zpano/indicators/core/outputs\"\n)\n\nfunc testCTVTime() time.Time {\n\treturn time.Date(2021, time.April, 1, 0, 0, 0, 0, &time.Location{})\n}\n\n// testCTVInput is the 252-entry TA-Lib MAMA reference series.\n//\n//nolint:dupl\nfunc testCTVInput() []float64 {\n\treturn []float64{\n\t\t92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,\n\t\t94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,\n\t\t88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,\n\t\t85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,\n\t\t83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,\n\t\t89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,\n\t\t89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,\n\t\t88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,\n\t\t103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,\n\t\t120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,\n\t\t114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,\n\t\t114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,\n\t\t124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,\n\t\t137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,\n\t\t123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,\n\t\t122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,\n\t\t123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,\n\t\t130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,\n\t\t127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,\n\t\t121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,\n\t\t106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,\n\t\t95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,\n\t\t94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,\n\t\t103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,\n\t\t106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,\n\t\t109.5300, 108.0600,\n\t}\n}\n\nconst testCTVTolerance = 1e-4\n\nfunc TestCoronaTrendVigorUpdate(t *testing.T) {\n\tt.Parallel()\n\n\tinput := testCTVInput()\n\tt0 := testCTVTime()\n\n\ttype snap struct {\n\t\ti   int\n\t\ttv  float64\n\t\tvmn float64\n\t\tvmx float64\n\t}\n\tsnapshots := []snap{\n\t\t{11, 0, 0, 0},\n\t\t{12, 0, 0, 0},\n\t\t{50, 0, 0, 0},\n\t\t{100, 0, 0, 0},\n\t\t{150, 0, 0, 0},\n\t\t{200, 0, 0, 0},\n\t\t{251, 0, 0, 0},\n\t}\n\n\t_ = snapshots\n\n\tx, err := NewCoronaTrendVigorDefault()\n\tif err != nil {\n\t\tt.Fatalf(\"unexpected error: %v\", err)\n\t}\n\n\tfor i := range input {\n\t\th, tv := x.Update(input[i], t0.Add(time.Duration(i)*time.Minute))\n\n\t\tif h == nil {\n\t\t\tt.Fatalf(\"[%d] heatmap must not be nil\", i)\n\t\t}\n\n\t\tif h.ParameterFirst != -10 || h.ParameterLast != 10 || math.Abs(h.ParameterResolution-2.45) > 1e-9 {\n\t\t\tt.Errorf(\"[%d] heatmap axis incorrect: first=%v last=%v res=%v\",\n\t\t\t\ti, h.ParameterFirst, h.ParameterLast, h.ParameterResolution)\n\t\t}\n\n\t\tif !x.IsPrimed() {\n\t\t\tif !h.IsEmpty() {\n\t\t\t\tt.Errorf(\"[%d] expected empty heatmap before priming, got len=%d\", i, len(h.Values))\n\t\t\t}\n\n\t\t\tif !math.IsNaN(tv) {\n\t\t\t\tt.Errorf(\"[%d] expected NaN tv before priming, got %v\", i, tv)\n\t\t\t}\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif len(h.Values) != 50 {\n\t\t\tt.Errorf(\"[%d] heatmap values length: expected 50, got %d\", i, len(h.Values))\n\t\t}\n\n\t\t// SNAPSHOT CAPTURE:\n\t\tswitch i {\n\t\tcase 11, 12, 50, 100, 150, 200, 251:\n\t\t\tt.Logf(\"SNAP {%d, %.10f, %.10f, %.10f},\", i, tv, h.ValueMin, h.ValueMax)\n\t\t}\n\t}\n}\n\nfunc TestCoronaTrendVigorPrimesAtBar11(t *testing.T) {\n\tt.Parallel()\n\n\tx, _ := NewCoronaTrendVigorDefault()\n\n\tif x.IsPrimed() {\n\t\tt.Error(\"expected not primed at start\")\n\t}\n\n\tinput := testCTVInput()\n\tt0 := testCTVTime()\n\tprimedAt := -1\n\n\tfor i := range input {\n\t\tx.Update(input[i], t0.Add(time.Duration(i)*time.Minute))\n\n\t\tif x.IsPrimed() && primedAt < 0 {\n\t\t\tprimedAt = i\n\t\t}\n\t}\n\n\tif primedAt != 11 {\n\t\tt.Errorf(\"expected priming at index 11, got %d\", primedAt)\n\t}\n}\n\nfunc TestCoronaTrendVigorNaNInput(t *testing.T) {\n\tt.Parallel()\n\n\tx, _ := NewCoronaTrendVigorDefault()\n\n\th, tv := x.Update(math.NaN(), testCTVTime())\n\n\tif h == nil || !h.IsEmpty() {\n\t\tt.Errorf(\"expected empty heatmap for NaN input, got %v\", h)\n\t}\n\n\tif !math.IsNaN(tv) {\n\t\tt.Errorf(\"expected NaN tv for NaN input, got %v\", tv)\n\t}\n\n\tif x.IsPrimed() {\n\t\tt.Error(\"NaN input must not prime the indicator\")\n\t}\n}\n\nfunc TestCoronaTrendVigorMetadata(t *testing.T) {\n\tt.Parallel()\n\n\tx, _ := NewCoronaTrendVigorDefault()\n\tmd := x.Metadata()\n\n\tcheck := func(what string, exp, act any) {\n\t\tt.Helper()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"%s: expected %v, actual %v\", what, exp, act)\n\t\t}\n\t}\n\n\tmnVal := \"ctv(50, 20, -10, 10, 30, hl/2)\"\n\tmnTV := \"ctv-tv(30, hl/2)\"\n\n\tcheck(\"Type\", core.CoronaTrendVigor, md.Type)\n\tcheck(\"Mnemonic\", mnVal, md.Mnemonic)\n\tcheck(\"Description\", \"Corona trend vigor \"+mnVal, md.Description)\n\tcheck(\"len(Outputs)\", 2, len(md.Outputs))\n\n\tcheck(\"Outputs[0].Kind\", int(Value), md.Outputs[0].Kind)\n\tcheck(\"Outputs[0].Type\", outputs.HeatmapType, md.Outputs[0].Type)\n\tcheck(\"Outputs[0].Mnemonic\", mnVal, md.Outputs[0].Mnemonic)\n\n\tcheck(\"Outputs[1].Kind\", int(TrendVigor), md.Outputs[1].Kind)\n\tcheck(\"Outputs[1].Type\", outputs.ScalarType, md.Outputs[1].Type)\n\tcheck(\"Outputs[1].Mnemonic\", mnTV, md.Outputs[1].Mnemonic)\n}\n\n//nolint:funlen\nfunc TestCoronaTrendVigorUpdateEntity(t *testing.T) {\n\tt.Parallel()\n\n\tconst (\n\t\tprimeCount = 50\n\t\tinp        = 100.\n\t\toutputLen  = 2\n\t)\n\n\ttm := testCTVTime()\n\tinput := testCTVInput()\n\n\tcheck := func(act core.Output) {\n\t\tt.Helper()\n\n\t\tif len(act) != outputLen {\n\t\t\tt.Errorf(\"len(output): expected %v, actual %v\", outputLen, len(act))\n\n\t\t\treturn\n\t\t}\n\n\t\th, ok := act[0].(*outputs.Heatmap)\n\t\tif !ok {\n\t\t\tt.Errorf(\"output[0] is not a heatmap: %T\", act[0])\n\t\t} else if h.Time != tm {\n\t\t\tt.Errorf(\"output[0].Time: expected %v, actual %v\", tm, h.Time)\n\t\t}\n\n\t\ts, ok := act[1].(entities.Scalar)\n\t\tif !ok {\n\t\t\tt.Errorf(\"output[1] is not a scalar\")\n\t\t} else if s.Time != tm {\n\t\t\tt.Errorf(\"output[1].Time: expected %v, actual %v\", tm, s.Time)\n\t\t}\n\t}\n\n\tprime := func(x *CoronaTrendVigor) {\n\t\tfor i := 0; i < primeCount; i++ {\n\t\t\tx.Update(input[i%len(input)], tm)\n\t\t}\n\t}\n\n\tt.Run(\"update scalar\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\ts := entities.Scalar{Time: tm, Value: inp}\n\t\tx, _ := NewCoronaTrendVigorDefault()\n\t\tprime(x)\n\t\tcheck(x.UpdateScalar(&s))\n\t})\n\n\tt.Run(\"update bar\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tb := entities.Bar{Time: tm, High: inp * 1.005, Low: inp * 0.995, Close: inp}\n\t\tx, _ := NewCoronaTrendVigorDefault()\n\t\tprime(x)\n\t\tcheck(x.UpdateBar(&b))\n\t})\n\n\tt.Run(\"update quote\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tq := entities.Quote{Time: tm, Bid: inp, Ask: inp}\n\t\tx, _ := NewCoronaTrendVigorDefault()\n\t\tprime(x)\n\t\tcheck(x.UpdateQuote(&q))\n\t})\n\n\tt.Run(\"update trade\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tr := entities.Trade{Time: tm, Price: inp}\n\t\tx, _ := NewCoronaTrendVigorDefault()\n\t\tprime(x)\n\t\tcheck(x.UpdateTrade(&r))\n\t})\n}\n\n//nolint:funlen\nfunc TestNewCoronaTrendVigor(t *testing.T) {\n\tt.Parallel()\n\n\tcheck := func(name string, exp, act any) {\n\t\tt.Helper()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"%s: expected %v, actual %v\", name, exp, act)\n\t\t}\n\t}\n\n\tt.Run(\"default\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx, err := NewCoronaTrendVigorDefault()\n\t\tcheck(\"err == nil\", true, err == nil)\n\t\tcheck(\"mnemonic\", \"ctv(50, 20, -10, 10, 30, hl/2)\", x.mnemonic)\n\t\tcheck(\"MinParameterValue\", -10.0, x.minParameterValue)\n\t\tcheck(\"MaxParameterValue\", 10.0, x.maxParameterValue)\n\t\tcheck(\"RasterLength\", 50, x.rasterLength)\n\t})\n\n\tt.Run(\"RasterLength < 2\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaTrendVigorParams(&Params{RasterLength: 1})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona trend vigor parameters: RasterLength should be >= 2\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"MaxParameterValue <= MinParameterValue\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaTrendVigorParams(&Params{\n\t\t\tMinParameterValue: 5,\n\t\t\tMaxParameterValue: 5,\n\t\t})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona trend vigor parameters: MaxParameterValue should be > MinParameterValue\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"HighPassFilterCutoff < 2\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaTrendVigorParams(&Params{HighPassFilterCutoff: 1})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona trend vigor parameters: HighPassFilterCutoff should be >= 2\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"MinimalPeriod < 2\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaTrendVigorParams(&Params{MinimalPeriod: 1})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona trend vigor parameters: MinimalPeriod should be >= 2\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"MaximalPeriod <= MinimalPeriod\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaTrendVigorParams(&Params{\n\t\t\tMinimalPeriod: 10,\n\t\t\tMaximalPeriod: 10,\n\t\t})\n\t\tif err == nil || err.Error() !=\n\t\t\t\"invalid corona trend vigor parameters: MaximalPeriod should be > MinimalPeriod\" {\n\t\t\tt.Errorf(\"unexpected: %v\", err)\n\t\t}\n\t})\n\n\tt.Run(\"invalid bar component\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\t_, err := NewCoronaTrendVigorParams(&Params{\n\t\t\tBarComponent: entities.BarComponent(9999),\n\t\t})\n\t\tif err == nil {\n\t\t\tt.Error(\"expected error\")\n\t\t}\n\t})\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor_test.go"
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

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: bash**

**Input:**
```json
{
  "command": "go build ./indicators/johnehlers/coronatrendvigor/ 2>&1",
  "description": "Build coronatrendvigor",
  "workdir": "/home/dev/zpano/go"
}
```

---

## Assistant (Build · claude-opus-4.7 · 4.5s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test -v -run TestCoronaTrendVigorUpdate$ ./indicators/johnehlers/coronatrendvigor/ 2>&1 | tail -30",
  "description": "Run snapshot capture",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
=== RUN   TestCoronaTrendVigorUpdate
=== PAUSE TestCoronaTrendVigorUpdate
=== CONT  TestCoronaTrendVigorUpdate
--- FAIL: TestCoronaTrendVigorUpdate (0.00s)
panic: runtime error: index out of range [-9223372036854775748] [recovered, repanicked]

goroutine 7 [running]:
testing.tRunner.func1.2({0x581120, 0x947104800a8})
	/usr/local/go/src/testing/testing.go:1974 +0x232
testing.tRunner.func1()
	/usr/local/go/src/testing/testing.go:1977 +0x349
panic({0x581120?, 0x947104800a8?})
	/usr/local/go/src/runtime/panic.go:860 +0x13a
zpano/indicators/johnehlers/coronatrendvigor.(*CoronaTrendVigor).Update(0x947105d8120, 0x40574b0a3d70a3d7, {0x947104fc310?, 0xdf8475800?, 0x947104fc310?})
	/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor.go:322 +0xb6a
zpano/indicators/johnehlers/coronatrendvigor.TestCoronaTrendVigorUpdate(0x947105c8248)
	/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor_test.go:84 +0x185
testing.tRunner(0x947105c8248, 0x5a16e8)
	/usr/local/go/src/testing/testing.go:2036 +0xea
created by testing.(*T).Run in goroutine 1
	/usr/local/go/src/testing/testing.go:2101 +0x4c5
FAIL	zpano/indicators/johnehlers/coronatrendvigor	0.004s
FAIL

```

---

## Assistant (Build · claude-opus-4.7 · 9.6s)

_Thinking:_

 I'm noticing a potential issue on line 322 where accessing the sample buffer could fail if `DominantCycleMedian` produces a zero or negative cycle period, especially during the initial bars when values are still stabilizing. I need to add a guard to ensure the lookback value stays at least 1 to prevent invalid array access.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor.go",
  "oldString": "\tcyclePeriod := int(s.c.DominantCycleMedian() - 1)\n\tif cyclePeriod > len(s.sampleBuffer) {\n\t\tcyclePeriod = len(s.sampleBuffer)\n\t}\n\n\tlookback := cyclePeriod\n\tif s.sampleCount < lookback {\n\t\tlookback = s.sampleCount\n\t}",
  "newString": "\tcyclePeriod := int(s.c.DominantCycleMedian() - 1)\n\tif cyclePeriod > len(s.sampleBuffer) {\n\t\tcyclePeriod = len(s.sampleBuffer)\n\t}\n\n\tif cyclePeriod < 1 {\n\t\tcyclePeriod = 1\n\t}\n\n\tlookback := cyclePeriod\n\tif s.sampleCount < lookback {\n\t\tlookback = s.sampleCount\n\t}"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.1s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test -v -run TestCoronaTrendVigorUpdate$ ./indicators/johnehlers/coronatrendvigor/ 2>&1 | tail -20",
  "description": "Re-run snapshot capture",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
=== RUN   TestCoronaTrendVigorUpdate
=== PAUSE TestCoronaTrendVigorUpdate
=== CONT  TestCoronaTrendVigorUpdate
    coronatrendvigor_test.go:114: SNAP {11, 5.6512200755, 20.0000000000, 20.0000000000},
    coronatrendvigor_test.go:114: SNAP {12, 6.8379492897, 20.0000000000, 20.0000000000},
    coronatrendvigor_test.go:114: SNAP {50, 2.6145116709, 2.3773561485, 20.0000000000},
    coronatrendvigor_test.go:114: SNAP {100, 2.7536803664, 2.4892742850, 20.0000000000},
    coronatrendvigor_test.go:114: SNAP {150, -6.4606404251, 20.0000000000, 20.0000000000},
    coronatrendvigor_test.go:114: SNAP {200, -10.0000000000, 20.0000000000, 20.0000000000},
    coronatrendvigor_test.go:114: SNAP {251, -0.1894989954, 0.5847573715, 20.0000000000},
--- PASS: TestCoronaTrendVigorUpdate (0.00s)
PASS
ok  	zpano/indicators/johnehlers/coronatrendvigor	0.004s

```

---

## Assistant (Build · claude-opus-4.7 · 20.9s)

Now lock in snapshots:

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/coronatrendvigor/coronatrendvigor_test.go",
  "newString": "\tsnapshots := []snap{\n\t\t{11, 5.6512200755, 20.0000000000, 20.0000000000},\n\t\t{12, 6.8379492897, 20.0000000000, 20.0000000000},\n\t\t{50, 2.6145116709, 2.3773561485, 20.0000000000},\n\t\t{100, 2.7536803664, 2.4892742850, 20.0000000000},\n\t\t{150, -6.4606404251, 20.0000000000, 20.0000000000},\n\t\t{200, -10.0000000000, 20.0000000000, 20.0000000000},\n\t\t{251, -0.1894989954, 0.5847573715, 20.0000000000},\n\t}\n\n\tx, err := NewCoronaTrendVigorDefault()\n\tif err != nil {\n\t\tt.Fatalf(\"unexpected error: %v\", err)\n\t}\n\n\tsi := 0\n\tfor i := range input {\n\t\th, tv := x.Update(input[i], t0.Add(time.Duration(i)*time.Minute))\n\n\t\tif h == nil {\n\t\t\tt.Fatalf(\"[%d] heatmap must not be nil\", i)\n\t\t}\n\n\t\tif h.ParameterFirst != -10 || h.ParameterLast != 10 || math.Abs(h.ParameterResolution-2.45) > 1e-9 {\n\t\t\tt.Errorf(\"[%d] heatmap axis incorrect: first=%v last=%v res=%v\",\n\t\t\t\ti, h.ParameterFirst, h.ParameterLast, h.ParameterResolution)\n\t\t}\n\n\t\tif !x.IsPrimed() {\n\t\t\tif !h.IsEmpty() {\n\t\t\t\tt.Errorf(\"[%d] expected empty heatmap before priming, got len=%d\", i, len(h.Values))\n\t\t\t}\n\n\t\t\tif !math.IsNaN(tv) {\n\t\t\t\tt.Errorf(\"[%d] expected NaN tv before priming, got %v\", i, tv)\n\t\t\t}\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif len(h.Values) != 50 {\n\t\t\tt.Errorf(\"[%d] heatmap values length: expected 50, got %d\", i, len(h.Values))\n\t\t}\n\n\t\tif si < len(snapshots) && snapshots[si].i == i {\n\t\t\tif math.Abs(snapshots[si].tv-tv) > testCTVTolerance {\n\t\t\t\tt.Errorf(\"[%d] tv: expected %v, got %v\", i, snapshots[si].tv, tv)\n\t\t\t}\n\n\t\t\tif math.Abs(snapshots[si].vmn-h.ValueMin) > testCTVTolerance {\n\t\t\t\tt.Errorf(\"[%d] vmin: expected %v, got %v\", i, snapshots[si].vmn, h.ValueMin)\n\t\t\t}\n\n\t\t\tif math.Abs(snapshots[si].vmx-h.ValueMax) > testCTVTolerance {\n\t\t\t\tt.Errorf(\"[%d] vmax: expected %v, got %v\", i, snapshots[si].vmx, h.ValueMax)\n\t\t\t}\n\n\t\t\tsi++\n\t\t}\n\t}\n\n\tif si != len(snapshots) {\n\t\tt.Errorf(\"did not hit all %d snapshots, reached %d\", len(snapshots), si)\n\t}\n}",
  "oldString": "\tsnapshots := []snap{\n\t\t{11, 0, 0, 0},\n\t\t{12, 0, 0, 0},\n\t\t{50, 0, 0, 0},\n\t\t{100, 0, 0, 0},\n\t\t{150, 0, 0, 0},\n\t\t{200, 0, 0, 0},\n\t\t{251, 0, 0, 0},\n\t}\n\n\t_ = snapshots\n\n\tx, err := NewCoronaTrendVigorDefault()\n\tif err != nil {\n\t\tt.Fatalf(\"unexpected error: %v\", err)\n\t}\n\n\tfor i := range input {\n\t\th, tv := x.Update(input[i], t0.Add(time.Duration(i)*time.Minute))\n\n\t\tif h == nil {\n\t\t\tt.Fatalf(\"[%d] heatmap must not be nil\", i)\n\t\t}\n\n\t\tif h.ParameterFirst != -10 || h.ParameterLast != 10 || math.Abs(h.ParameterResolution-2.45) > 1e-9 {\n\t\t\tt.Errorf(\"[%d] heatmap axis incorrect: first=%v last=%v res=%v\",\n\t\t\t\ti, h.ParameterFirst, h.ParameterLast, h.ParameterResolution)\n\t\t}\n\n\t\tif !x.IsPrimed() {\n\t\t\tif !h.IsEmpty() {\n\t\t\t\tt.Errorf(\"[%d] expected empty heatmap before priming, got len=%d\", i, len(h.Values))\n\t\t\t}\n\n\t\t\tif !math.IsNaN(tv) {\n\t\t\t\tt.Errorf(\"[%d] expected NaN tv before priming, got %v\", i, tv)\n\t\t\t}\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif len(h.Values) != 50 {\n\t\t\tt.Errorf(\"[%d] heatmap values length: expected 50, got %d\", i, len(h.Values))\n\t\t}\n\n\t\t// SNAPSHOT CAPTURE:\n\t\tswitch i {\n\t\tcase 11, 12, 50, 100, 150, 200, 251:\n\t\t\tt.Logf(\"SNAP {%d, %.10f, %.10f, %.10f},\", i, tv, h.ValueMin, h.ValueMax)\n\t\t}\n\t}\n}"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.8s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./indicators/johnehlers/coronatrendvigor/ && go vet ./indicators/johnehlers/coronatrendvigor/... && go test ./indicators/johnehlers/...",
  "description": "Final tests",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
ok  	zpano/indicators/johnehlers/coronatrendvigor	0.005s
?   	zpano/indicators/johnehlers	[no test files]
ok  	zpano/indicators/johnehlers/centerofgravityoscillator	(cached)
ok  	zpano/indicators/johnehlers/corona	(cached)
ok  	zpano/indicators/johnehlers/coronasignaltonoiseratio	(cached)
ok  	zpano/indicators/johnehlers/coronaspectrum	(cached)
ok  	zpano/indicators/johnehlers/coronaswingposition	(cached)
ok  	zpano/indicators/johnehlers/coronatrendvigor	(cached)
ok  	zpano/indicators/johnehlers/cybercycle	(cached)
ok  	zpano/indicators/johnehlers/dominantcycle	(cached)
ok  	zpano/indicators/johnehlers/fractaladaptivemovingaverage	(cached)
ok  	zpano/indicators/johnehlers/hilberttransformer	(cached)
ok  	zpano/indicators/johnehlers/hilberttransformerinstantaneoustrendline	(cached)
ok  	zpano/indicators/johnehlers/instantaneoustrendline	(cached)
ok  	zpano/indicators/johnehlers/mesaadaptivemovingaverage	(cached)
ok  	zpano/indicators/johnehlers/roofingfilter	(cached)
ok  	zpano/indicators/johnehlers/sinewave	(cached)
ok  	zpano/indicators/johnehlers/supersmoother	(cached)
ok  	zpano/indicators/johnehlers/trendcyclemode	(cached)
ok  	zpano/indicators/johnehlers/zerolagerrorcorrectingexponentialmovingaverage	(cached)
ok  	zpano/indicators/johnehlers/zerolagexponentialmovingaverage	(cached)

```

---

## Assistant (Build · claude-opus-4.7 · 19.6s)

All four Corona indicators are now green. Per the plan, this is the **pause for user review point** before the TS port.

## Status

✅ **Go Phase Complete** — all 4 Corona indicators implemented & tested:

| Package | Mnemonic | Scalar | Tests |
|---|---|---|---|
| `corona/` (helper) | — | — | ✅ |
| `coronaspectrum/` | `cspect(6, 20, 6, 30, 30, hl/2)` | DC, DCM | ✅ |
| `coronasignaltonoiseratio/` | `csnr(50, 20, 1, 11, 30, hl/2)` | SNR | ✅ |
| `coronaswingposition/` | `cswing(50, 20, -5, 5, 30, hl/2)` | SP | ✅ |
| `coronatrendvigor/` | `ctv(50, 20, -10, 10, 30, hl/2)` | TV | ✅ |

Notes on CTV impl:
- Applied the `DominantCycleMedian - 1` lookback fix (no clamp to sampleBufferLength).
- Added defensive `cyclePeriod >= 1` guard — early bars have DCM ≈ 0 so `int(DCM-1)` produced a huge negative index causing OOB.
- Full `go test ./indicators/johnehlers/...` passes, including all preexisting packages.

## Ready for review

Please review the 4 packages at `go/indicators/johnehlers/{corona,coronaspectrum,coronasignaltonoiseratio,coronaswingposition,coronatrendvigor}/`. Once approved, I'll proceed with:

- **Phase 6**: TypeScript port to `ts/indicators/john-ehlers/<pkg>/`
- Update `.opencode/skills/mbst-indicator-conversion/SKILL.md` with the Corona learnings (heatmap conventions, first-bar store-only pattern, DCM-1 lookback fix, parameter-resolution formula).

Want me to continue to Phase 6 now, or stop here for review?

---

