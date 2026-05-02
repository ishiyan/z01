# LinkedIn Content Calendar: Moving Average Series

## Series Definition

- **Subject**: Moving averages and DSP-based trading indicators
- **Audience**: Trading practitioners on LinkedIn (not quants/academics — drop derivations, use memory/lag metaphors, add trading implications)
- **Cadence**: 2x/week (Tuesday + Thursday)
- **Total posts**: 13 (one per article)
- **Goal**: Authority building, drive traffic to full articles
- **Format**: LinkedIn native post, 900-2500 chars, 210-char hook above fold, zero emoji, zero exclamation marks, no bold, 0-2 niche hashtags

## Topic Clusters

### Cluster 1: Foundations (how moving averages work and how to compare them)

```
Frequency Response (pillar — how to evaluate any MA)
├── EMA (spoke)
├── SMA (spoke)
└── WMA (spoke)
```

### Cluster 2: Multi-Stage Smoothers (stacking filters for less lag)

```
DEMA (pillar — introduces double-smoothing)
├── TEMA (spoke)
├── T3EMA (spoke)
├── T2EMA (spoke)
└── TRIMA (spoke)
```

### Cluster 3: Adaptive Filters (MAs that adjust to market conditions)

```
KAMA (pillar — most well-known adaptive MA)
├── FRAMA (spoke)
└── JMA (spoke)
```

### Cluster 4: Advanced Concepts

```
HTCE (pillar — Hilbert Transform cycle estimation, standalone)
```

## Schedule

### Week 1

**Tuesday — EMA**
- Cluster: Foundations (spoke)
- Rationale: Warm-up post. Everyone knows EMA; easy entry point to establish voice and attract followers. High engagement potential from recognition.
- Source: `testdata/ema/content.md`
- Distribution: LinkedIn Day 0, X Day 1

**Thursday — SMA**
- Cluster: Foundations (spoke)
- Rationale: Natural companion to EMA. Sets up the "which is better" question that the pillar will answer.
- Source: `testdata/sma/content.md`
- Distribution: LinkedIn Day 0, X Day 1

### Week 2

**Tuesday — Frequency Response**
- Cluster: Foundations (pillar)
- Rationale: Now that followers have seen EMA and SMA, introduce the framework for comparing *any* MA. Reframes the prior two posts. Links back to both.
- Source: `testdata/frequency-response/content.md`
- Distribution: LinkedIn Day 0, X Day 1, Medium Day 3

**Thursday — WMA**
- Cluster: Foundations (spoke)
- Rationale: Completes the foundations cluster. Less well-known than EMA/SMA, so benefits from the pillar post having just established the evaluation framework.
- Source: `testdata/wma/content.md`
- Distribution: LinkedIn Day 0, X Day 1

### Week 3

**Tuesday — DEMA**
- Cluster: Multi-Stage (pillar)
- Rationale: Introduces the concept of double-smoothing. Opens the largest cluster. "What if you could subtract the lag?" is a strong hook angle.
- Source: `testdata/dema/content.md`
- Distribution: LinkedIn Day 0, X Day 1, Medium Day 3

**Thursday — TEMA**
- Cluster: Multi-Stage (spoke)
- Rationale: Natural extension of DEMA — triple smoothing. Post can reference the DEMA post directly.
- Source: `testdata/tema/content.md`
- Distribution: LinkedIn Day 0, X Day 1

### Week 4

**Tuesday — T3EMA**
- Cluster: Multi-Stage (spoke)
- Rationale: Tillson's T3 is a practitioner favorite. The volume factor adds a tuning knob traders care about.
- Source: `testdata/t3ema/content.md`
- Distribution: LinkedIn Day 0, X Day 1

**Thursday — T2EMA**
- Cluster: Multi-Stage (spoke)
- Rationale: Comparison piece to T3. Highlights the tradeoff between smoothness and responsiveness.
- Source: `testdata/t2ema/content.md`
- Distribution: LinkedIn Day 0, X Day 1

### Week 5

**Tuesday — TRIMA**
- Cluster: Multi-Stage (spoke)
- Rationale: Triangular MA completes the multi-stage cluster. Good "did you know SMA of SMA = TRIMA" angle.
- Source: `testdata/trima/content.md`
- Distribution: LinkedIn Day 0, X Day 1

**Thursday — KAMA**
- Cluster: Adaptive (pillar)
- Rationale: Most famous adaptive filter. Strong standalone post. "An MA that knows when to listen and when to ignore" hook angle.
- Source: `testdata/kama/content.md`
- Distribution: LinkedIn Day 0, X Day 1, Medium Day 3

### Week 6

**Tuesday — FRAMA**
- Cluster: Adaptive (spoke)
- Rationale: Fractal dimension — high curiosity factor for the LinkedIn audience. "Your market data has a fractal dimension, and it changes your MA" angle.
- Source: `testdata/frama/content.md`
- Distribution: LinkedIn Day 0, X Day 1

**Thursday — JMA**
- Cluster: Adaptive (spoke)
- Rationale: Jurik's proprietary filter. The "black box" mystique drives engagement. Post can honestly discuss what's known vs. unknown.
- Source: `testdata/jma/content.md`
- Distribution: LinkedIn Day 0, X Day 1

### Week 7

**Tuesday — HTCE**
- Cluster: Advanced (pillar)
- Rationale: Grand finale. Most novel topic — Hilbert Transform for cycle estimation. Links back to frequency response and adaptive filter posts. Ends the series on a high note.
- Source: `testdata/htce/content.md`
- Distribution: LinkedIn Day 0, X Day 1, Medium Day 3

## Content Mix

| Type | Count | Ratio |
|------|-------|-------|
| New content (LinkedIn adaptations) | 13 | 100% |

Note: All 13 posts are new LinkedIn adaptations of existing blog articles. Refreshes and repurposed content can be planned after the initial series completes (e.g., a comparison table post combining all 13 MAs, or a "which MA for which market regime" roundup).

## Post-Series Options

After the 7-week run, consider:

1. **Roundup post**: "13 moving averages compared — which one for which market" (repurposed)
2. **Poll post**: "Which MA do you actually use in production?" (conversational)
3. **Refresh cycle**: Update any posts that got strong engagement with follow-up insights from comments
4. **Series 2 tease**: Oscillators, or a deep-dive into one MA's parameter sensitivity

## Hashtag Bank

Use 0-2 per post, selected from:

- #quantitativetrading
- #algorithmictrading
- #technicalanalysis
- #signalprocessing
- #movingaverages
- #tradingsystems
