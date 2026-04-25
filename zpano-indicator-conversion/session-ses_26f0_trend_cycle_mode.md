# Split INDICATORS-ARCHITECTURE and CONVERTING-OLD-INDICATOR into skills?

**Session ID:** ses_26f009c55ffeeFyvE85VbCND9m
**Created:** 4/15/2026, 1:55:49 PM
**Updated:** 4/21/2026, 5:04:17 PM

---

## Assistant (Build · claude-opus-4.7 · 3.9s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./indicators/core/...",
  "description": "Test core type registration",
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

## Assistant (Build · claude-opus-4.7 · 3.7s)

Now TS indicator-type:

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/indicator-type.ts"
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
31:   /** Identifies the __Jurik Moving Average__ (JMA) indicator. */
32:   JurikMovingAverage,
33: 
34:   /** Identifies the Ehler's __MESA Adaptive Moving Average__ (MAMA) indicator. */
35:   MesaAdaptiveMovingAverage,
36: 
37:   /** Identifies the Ehler's __Fractal Adaptive Moving Average__ (FRAMA) indicator. */
38:   FractalAdaptiveMovingAverage,
39: 
40:   /** Identifies the Ehler's __Dominant Cycle__ (DC) indicator, exposing raw period, smoothed period and phase. */
41:   DominantCycle,
42: 
43:   /** Identifies the __Momentum__ (MOM) indicator. */
44:   Momentum,
45: 
46:   /** Identifies the __Rate of Change__ (ROC) indicator. */
47:   RateOfChange,
48: 
49:   /** Identifies the __Rate of Change Percent__ (ROCP) indicator. */
50:   RateOfChangePercent,
51: 
52:   /** Identifies the __Relative Strength Index__ (RSI) indicator. */
53:   RelativeStrengthIndex,
54: 
55:   /** Identifies the __Chande Momentum Oscillator__ (CMO) indicator. */
56:   ChandeMomentumOscillator,
57: 
58:   /** Identifies the __Bollinger Bands__ (BB) indicator. */
59:   BollingerBands,
60: 
61:   /** Identifies the __Variance__ (VAR) indicator. */
62:   Variance,
63: 
64:   /** Identifies the __Standard Deviation__ (STDEV) indicator. */
65:   StandardDeviation,
66: 
67:   /** Identifies the __Goertzel power spectrum__ (GOERTZEL) indicator. */
68:   GoertzelSpectrum,
69: 
70:   /** Identifies the Ehler's __Center of Gravity Oscillator__ (COG) indicator. */
71:   CenterOfGravityOscillator,
72: 
73:   /** Identifies the Ehler's __Cyber Cycle__ (CC) indicator. */
74:   CyberCycle,
75: 
76:   /** Identifies the Ehler's __Instantaneous Trend Line__ (iTrend) indicator. */
77:   InstantaneousTrendLine,
78: 
79:   /** Identifies the Ehler's __Super Smoother__ (SS) indicator. */
80:   SuperSmoother,
81: 
82:   /** Identifies the Ehler's __Zero-lag Exponential Moving Average__ (ZEMA) indicator. */
83:   ZeroLagExponentialMovingAverage,
84: 
85:   /** Identifies the Ehler's __Zero-lag Error-Correcting Exponential Moving Average__ (ZECEMA) indicator. */
86:   ZeroLagErrorCorrectingExponentialMovingAverage,
87: 
88:   /** Identifies the Ehler's __Roofing Filter__ indicator. */
89:   RoofingFilter,
90: 
91:   /** Identifies Welles Wilder's __True Range__ (TR) indicator. */
92:   TrueRange,
93: 
94:   /** Identifies Welles Wilder's __Average True Range__ (ATR) indicator. */
95:   AverageTrueRange,
96: 
97:   /** Identifies Welles Wilder's __Normalized Average True Range__ (NATR) indicator. */
98:   NormalizedAverageTrueRange,
99: 
100:   /** Identifies Welles Wilder's __Directional Movement Minus__ (-DM) indicator. */
101:   DirectionalMovementMinus,
102: 
103:   /** Identifies Welles Wilder's __Directional Movement Plus__ (+DM) indicator. */
104:   DirectionalMovementPlus,
105: 
106:   /** Identifies Welles Wilder's __Directional Indicator Minus__ (-DI) indicator. */
107:   DirectionalIndicatorMinus,
108: 
109:   /** Identifies Welles Wilder's __Directional Indicator Plus__ (+DI) indicator. */
110:   DirectionalIndicatorPlus,
111: 
112:   /** Identifies Welles Wilder's __Directional Movement Index__ (DX) indicator. */
113:   DirectionalMovementIndex,
114: 
115:   /** Identifies Welles Wilder's __Average Directional Movement Index__ (ADX) indicator. */
116:   AverageDirectionalMovementIndex,
117: 
118:   /** Identifies Welles Wilder's __Average Directional Movement Index Rating__ (ADXR) indicator. */
119:   AverageDirectionalMovementIndexRating,
120: 
121:   /** Identifies Larry Williams' __Williams %R__ (WILL%R) indicator. */
122:   WilliamsPercentR,
123: 
124:   /** Identifies Gerald Appel's __Percentage Price Oscillator__ (PPO) indicator. */
125:   PercentagePriceOscillator,
126: 
127:   /** Identifies the __Absolute Price Oscillator__ (APO) indicator. */
128:   AbsolutePriceOscillator,
129: 
130:   /** Identifies Donald Lambert's __Commodity Channel Index__ (CCI) indicator. */
131:   CommodityChannelIndex,
132: 
133:   /** Identifies Gene Quong's __Money Flow Index__ (MFI) indicator. */
134:   MoneyFlowIndex,
135: 
136:   /** Identifies Joseph Granville's __On-Balance Volume__ (OBV) indicator. */
137:   OnBalanceVolume,
138: 
139:   /** Identifies Igor Livshin's __Balance of Power__ (BOP) indicator. */
140:   BalanceOfPower,
141: 
142:   /** Identifies the __Rate of Change Ratio__ (ROCR / ROCR100) indicator. */
143:   RateOfChangeRatio,
144: 
145:   /** Identifies __Pearson's Correlation Coefficient__ (CORREL) indicator. */
146:   PearsonsCorrelationCoefficient,
147: 
148:   /** Identifies the __Linear Regression__ (LINREG) indicator. */
149:   LinearRegression,
150: 
151:   /** Identifies Larry Williams' __Ultimate Oscillator__ (ULTOSC) indicator. */
152:   UltimateOscillator,
153: 
154:   /** Identifies Tushar Chande's __Stochastic Relative Strength Index__ (STOCHRSI) indicator. */
155:   StochasticRelativeStrengthIndex,
156: 
157:   /** Identifies George Lane's __Stochastic Oscillator__ (STOCH) indicator. */
158:   Stochastic,
159: 
160:   /** Identifies Tushar Chande's __Aroon__ (AROON) indicator. */
161:   Aroon,
162: 
163:   /** Identifies Marc Chaikin's __Advance-Decline__ (A/D) indicator. */
164:   AdvanceDecline,
165: 
166:   /** Identifies Marc Chaikin's __Advance-Decline Oscillator__ (ADOSC) indicator. */
167:   AdvanceDeclineOscillator,
168: 
169:   /** Identifies Welles Wilder's __Parabolic Stop And Reverse__ (SAR) indicator. */
170:   ParabolicStopAndReverse,
171: 
172:   /** Identifies Jack Hutson's __Triple Exponential Moving Average Oscillator__ (TRIX) indicator. */
173:   TripleExponentialMovingAverageOscillator,
174: 
175:   /** Identifies John Bollinger's __Bollinger Bands Trend__ (BBTrend) indicator. */
176:   BollingerBandsTrend,
177: 
178:   /** Identifies Gerald Appel's __Moving Average Convergence Divergence__ (MACD) indicator. */
179:   MovingAverageConvergenceDivergence,
180: 
181:   /** Identifies the Ehlers __Sine Wave__ (SW) indicator, exposing sine value, lead sine, band, dominant cycle period and phase. */
182:   SineWave,
183: 
184:   /** Identifies the Ehlers __Hilbert Transformer Instantaneous Trend Line__ (HTITL) indicator, exposing trend value and dominant cycle period. */
185:   HilbertTransformerInstantaneousTrendLine,
186: }

(End of file - total 186 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 8.6s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/core/indicator-type.ts",
  "newString": "  /** Identifies the Ehlers __Hilbert Transformer Instantaneous Trend Line__ (HTITL) indicator, exposing trend value and dominant cycle period. */\n  HilbertTransformerInstantaneousTrendLine,\n\n  /** Identifies the Ehlers __Trend / Cycle Mode__ (TCM) indicator, exposing the trend/cycle value (+1 in trend, −1 in cycle),\n   * trend/cycle mode flags, instantaneous trend line, sine wave, lead sine wave, dominant cycle period and phase. */\n  TrendCycleMode,\n}",
  "oldString": "  /** Identifies the Ehlers __Hilbert Transformer Instantaneous Trend Line__ (HTITL) indicator, exposing trend value and dominant cycle period. */\n  HilbertTransformerInstantaneousTrendLine,\n}"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.2s)

Now examine the HTITL template thoroughly and the MBST source to build the TCM:

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleMode.cs"
}
```

**Output:**
```
<path>/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleMode.cs</path>
<type>file</type>
<content>1: ﻿using System;
2: using System.Globalization;
3: using System.Runtime.Serialization;
4: using Mbst.Numerics;
5: 
6: namespace Mbst.Trading.Indicators.JohnEhlers
7: {
8:     /// <summary>
9:     /// <para>
10:     /// The Trend versus Cycle Mode indicator.</para>
11:     /// <para/>
12:     /// See John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 113-118.
13:     /// </summary>
14:     [DataContract]
15:     public sealed class TrendCycleMode : LineIndicator
16:     {
17:         #region Members and accessors
18:         /// <summary>
19:         /// The WMA smoothing length used by underlying Hilbert transformer.
20:         /// </summary>
21:         internal int SmoothingLength => dcp.SmoothingLength;
22: 
23:         /// <summary>
24:         /// The current WMA smoothed price used by underlying Hilbert transformer.
25:         /// </summary>
26:         public double SmoothedPrice => dcp.SmoothedValue;
27: 
28:         /// <summary>
29:         /// The current value of the the smoothed Dominant Cycle Period, or <c>NaN</c> if not primed.
30:         /// </summary>
31:         public double DominantCyclePeriod => dcp.DominantCyclePeriod;
32: 
33:         /// <summary>
34:         /// The current value of the the Dominant Cycle Phase, or <c>NaN</c> if not primed.
35:         /// </summary>
36:         public double DominantCyclePhase => dcp.Value;
37: 
38:         /// <summary>
39:         /// The current value of the the Sine Wave, or <c>NaN</c> if not primed.
40:         /// </summary>
41:         public double SineWave { get { lock (updateLock) { return primed ? sinWave : double.NaN; } } }
42: 
43:         /// <summary>
44:         /// The current value of the the Sine Wave Lead, or <c>NaN</c> if not primed.
45:         /// </summary>
46:         public double SineWaveLead { get { lock (updateLock) { return primed ? sinWaveLead : double.NaN; } } }
47: 
48:         /// <summary>
49:         /// The current value of the the Instantaneous Trend Line, or <c>NaN</c> if not primed.
50:         /// </summary>
51:         public double InstantaneousTrendLine { get { lock (updateLock) { return primed ? trendline : double.NaN; } } }
52: 
53:         /// <summary>
54:         /// The additional WMA smoothing length used to smooth the trend line.
55:         /// </summary>
56:         public int TrendLineSmoothingLength => trendLineSmoothingLength;
57: 
58:         /// <summary>
59:         /// If the WMA smoothed price is separated by more than this percentage from the instantaneous trend line, then the market is in the trend mode.
60:         /// </summary>
61:         public double SeparationPercentage => separationPercentage;
62: 
63:         /// <summary>
64:         /// The multiplier to the dominant cycle period used to determine the window length to calculate the trend line.
65:         /// </summary>
66:         public double CyclePartMultiplier => cyclePartMultiplier;
67: 
68:         /// <summary>
69:         /// The default value of an additional WMA smoothing length used to smooth the trend line.
70:         /// </summary>
71:         public const int DefaultTrendLineSmoothingLength = 4;
72: 
73:         /// <summary>
74:         /// The default value of the multiplier to the dominant cycle period used to determine the window length to calculate the trend line.
75:         /// </summary>
76:         public const double DefaultCyclePartMultiplier = 1;
77: 
78:         /// <summary>
79:         /// The default value of the separation percentage.
80:         /// If the WMA smoothed price is separated by more than this percentage from the instantaneous trend line, then the market is in the trend mode.
81:         /// </summary>
82:         public const double DefaultSeparationPercentage = 1.5;
83: 
84:         /// <summary>
85:         /// The current value of the the Trend versus Cycle Mode, or <c>NaN</c> if not primed.
86:         /// The value equals to <c>-1</c> if in cycle mode, or equals to <c>1</c> in trend mode.
87:         /// </summary>
88:         public double Value { get { lock (updateLock) { return isTrendMode ? 1d : -1d; } } }
89: 
90:         /// <summary>
91:         /// If the trend mode is declared.
92:         /// </summary>
93:         public bool IsTrendMode { get { lock (updateLock) { return isTrendMode; } } }
94: 
95:         /// <summary>
96:         /// If the cycle mode is declared.
97:         /// </summary>
98:         public bool IsCycleMode { get { lock (updateLock) { return !isTrendMode; } } }
99: 
100:         [DataMember]
101:         private double sinWave;
102:         [DataMember]
103:         private double sinWaveLead;
104:         [DataMember]
105:         private double trendline;
106:         [DataMember]
107:         private double trendAverage1;
108:         [DataMember]
109:         private double trendAverage2;
110:         [DataMember]
111:         private double trendAverage3;
112:         [DataMember]
113:         private readonly double coeff0;
114:         [DataMember]
115:         private readonly double coeff1;
116:         [DataMember]
117:         private readonly double coeff2;
118:         [DataMember]
119:         private readonly double coeff3;
120:         [DataMember]
121:         private readonly double alphaEmaPeriodAdditional;
122:         [DataMember]
123:         private readonly double oneMinAlphaEmaPeriodAdditional;
124:         [DataMember]
125:         private readonly double cyclePartMultiplier;
126:         [DataMember]
127:         private readonly double separationPercentage;
128:         [DataMember]
129:         private readonly double separationFactor;
130:         [DataMember]
131:         private double prevDcPhase;
132:         [DataMember]
133:         private double prevSineLeadWaveDifference;
134:         [DataMember]
135:         private readonly DominantCyclePhase dcp;
136:         [DataMember]
137:         private int samplesInTrend;
138:         [DataMember]
139:         private readonly int trendLineSmoothingLength;
140:         [DataMember]
141:         private bool isTrendMode = true;
142: 
143:         #region Variables to track the last htce.MaxCycle input values in a circular buffer.
144:         private readonly int inputLength;
145:         private readonly int inputLengthMin1;
146:         [DataMember]
147:         private readonly double[] input;
148:         #endregion
149: 
150:         private const string tcm = "ehlersTcm";
151:         private const string tcmFull = "John Ehlers Trend/Cycle Mode";
152:         #endregion
153: 
154:         #region Construction
155:         /// <summary>
156:         /// Constructs a new instance of the class.
157:         /// </summary>
158:         /// <param name="estimator">The type of an estimator. The default is homodyne discriminator.</param>
159:         /// <param name="alphaEmaPeriodAdditional">The value of α (0 &lt; α ≤ 1) used in EMA for additional smoothing of the instantaneous period. The default value is <c>DefaultAlphaEmaPeriodAdditional</c>.</param>
160:         /// <param name="trendLineSmoothingLength">The additional WMA smoothing length used to smooth the trend line. The valid values are 2, 3, 4. The default value is <c>DefaultTrendLineSmoothingLength</c>..</param>
161:         /// <param name="cyclePartMultiplier">The multiplier to the dominant cycle period used to determine the window length to calculate the trend line. The typical values are in [0.5, 1.5]. The default value is DefaultCyclePartMultiplier.</param>
162:         /// <param name="separationPercentage">If the WMA smoothed price is separated by more than this percentage from the instantaneous trend line, then the market is in the trend mode. The default value is DefaultSeparationPercentage.</param>
163:         /// <param name="ohlcvComponent">The ohlcv component used to calculate the indicator.</param>
164:         public TrendCycleMode(HilbertTransformerCycleEstimator estimator = HilbertTransformerCycleEstimator.HomodyneDiscriminator,
165:             double alphaEmaPeriodAdditional = JohnEhlers.DominantCyclePhase.DefaultAlphaEmaPeriodAdditional,
166:             int trendLineSmoothingLength = DefaultTrendLineSmoothingLength, double cyclePartMultiplier = DefaultCyclePartMultiplier,
167:             double separationPercentage = DefaultSeparationPercentage, OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
168:             : this(new DominantCyclePhase(estimator, alphaEmaPeriodAdditional, ohlcvComponent),
169:                   estimator, alphaEmaPeriodAdditional, trendLineSmoothingLength, cyclePartMultiplier, separationPercentage, ohlcvComponent) {}
170: 
171:         /// <summary>
172:         /// Constructs a new instance of the class.
173:         /// </summary>
174:         /// <param name="estimator">The type of an estimator. The default is homodyne discriminator.</param>
175:         /// <param name="smoothingLength">The WMA smoothing length. The valid values are 2, 3, 4. The default value is <c>estimator.DefaultSmoothingLength</c>.</param>
176:         /// <param name="warmUpPeriod">The number of updates before the indicator is primed. If less than the real primed period, then will be reset to the real primed period. The default value is <c>estimator.DefaultWarmUpPeriod</c>.</param>
177:         /// <param name="alphaEmaQuadratureInPhase">The value of α (0 &lt; α ≤ 1) used in EMA to smooth the in-phase and quadrature components. The default value is <c>estimator.DefaultAlphaEmaQuadratureInPhase</c>.</param>
178:         /// <param name="alphaEmaPeriod">The value of α (0 &lt; α ≤ 1) used in EMA to smooth the instantaneous period. The default value is <c>estimator.DefaultAlphaEmaPeriod</c>.</param>
179:         /// <param name="alphaEmaPeriodAdditional">The value of α (0 &lt; α ≤ 1) used in EMA for additional smoothing of the instantaneous period. The default value is <c>DefaultAlphaEmaPeriodAdditional</c>.</param>
180:         /// <param name="trendLineSmoothingLength">The additional WMA smoothing length used to smooth the trend line. The valid values are 2, 3, 4. The default value is <c>DefaultTrendLineSmoothingLength</c>..</param>
181:         /// <param name="cyclePartMultiplier">The multiplier to the dominant cycle period used to determine the window length to calculate the trend line. The typical values are in [0.5, 1.5]. The default value is DefaultCyclePartMultiplier.</param>
182:         /// <param name="separationPercentage">If the WMA smoothed price is separated by more than this percentage from the instantaneous trend line, then the market is in the trend mode. The default value is DefaultSeparationPercentage.</param>
183:         /// <param name="ohlcvComponent">The ohlcv component used to calculate the indicator.</param>
184:         public TrendCycleMode(HilbertTransformerCycleEstimator estimator, int smoothingLength, int warmUpPeriod,
185:             double alphaEmaQuadratureInPhase, double alphaEmaPeriod, double alphaEmaPeriodAdditional = JohnEhlers.DominantCyclePhase.DefaultAlphaEmaPeriodAdditional,
186:             int trendLineSmoothingLength = DefaultTrendLineSmoothingLength, double cyclePartMultiplier = DefaultCyclePartMultiplier,
187:             double separationPercentage = DefaultSeparationPercentage, OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
188:             : this(new DominantCyclePhase(estimator, smoothingLength, warmUpPeriod, alphaEmaQuadratureInPhase, alphaEmaPeriod, alphaEmaPeriodAdditional, ohlcvComponent),
189:                   estimator, alphaEmaPeriodAdditional, trendLineSmoothingLength, cyclePartMultiplier, separationPercentage, ohlcvComponent) {}
190: 
191:         private TrendCycleMode(DominantCyclePhase dcp, HilbertTransformerCycleEstimator estimator,
192:             double alphaEmaPeriodAdditional, int trendLineSmoothingLength,
193:             double cyclePartMultiplier, double separationPercentage, OhlcvComponent ohlcvComponent)
194:             : base(tcm + estimator.ShortName(), tcmFull + estimator.FullNameInBrackets(), ohlcvComponent)
195:         {
196:             if (2 > trendLineSmoothingLength || 4 < trendLineSmoothingLength)
197:                 throw new ArgumentOutOfRangeException(nameof(trendLineSmoothingLength));
198:             if (0d >= cyclePartMultiplier || 10d < cyclePartMultiplier)
199:                 throw new ArgumentOutOfRangeException(nameof(cyclePartMultiplier));
200:             if (0d >= separationPercentage || 100d < separationPercentage)
201:                 throw new ArgumentOutOfRangeException(nameof(separationPercentage));
202:             string additionalParameters = string.Format(CultureInfo.InvariantCulture, ",tlsl={0},cpMul={1:0.####},sep={2:0.####}%",
203:                 trendLineSmoothingLength, cyclePartMultiplier, separationPercentage);
204:             moniker = dcp.ComposeMoniker(tcm, additionalParameters);
205:             this.dcp = dcp;
206:             this.alphaEmaPeriodAdditional = alphaEmaPeriodAdditional;
207:             oneMinAlphaEmaPeriodAdditional = 1d - alphaEmaPeriodAdditional;
208:             this.trendLineSmoothingLength = trendLineSmoothingLength;
209:             this.cyclePartMultiplier = cyclePartMultiplier;
210:             this.separationPercentage = separationPercentage;
211:             separationFactor = separationPercentage / 100;
212:             inputLength = dcp.MaxPeriod;
213:             inputLengthMin1 = inputLength - 1;
214:             input = new double[inputLength];
215:             input.Initialize();
216:             if (2 == trendLineSmoothingLength)
217:             {
218:                 coeff0 = 2d / 3d;
219:                 coeff1 = 1d / 3d;
220:             }
221:             else if (3 == trendLineSmoothingLength)
222:             {
223:                 coeff0 = 3d / 6d;
224:                 coeff1 = 2d / 6d;
225:                 coeff2 = 1d / 6d;
226:             }
227:             else //if (4 == trendLineSmoothingLength)
228:             {
229:                 coeff0 = 4d / 10d;
230:                 coeff1 = 3d / 10d;
231:                 coeff2 = 2d / 10d;
232:                 coeff3 = 1d / 10d;
233:             }
234:         }
235:         #endregion
236: 
237:         #region Reset
238:         /// <summary>
239:         /// Resets the indicator.
240:         /// </summary>
241:         public override void Reset()
242:         {
243:             lock (updateLock)
244:             {
245:                 primed = false;
246:                 isTrendMode = true;
247:                 dcp.Reset();
248:                 trendline = 0d;
249:                 trendAverage1 = 0d;
250:                 trendAverage2 = 0d;
251:                 trendAverage3 = 0d;
252:                 prevDcPhase = 0d;
253:                 prevSineLeadWaveDifference = 0d;
254:                 sinWave = 0d;
255:                 sinWaveLead = 0d;
256:                 samplesInTrend = 0;
257:                 input.Initialize();
258:             }
259:         }
260:         #endregion
261: 
262:         #region Update
263:         /// <summary>
264:         /// Updates the value of the indicator.
265:         /// </summary>
266:         /// <param name="sample">A new sample.</param>
267:         /// <returns>The new value of the indicator.</returns>
268:         public override double Update(double sample)
269:         {
270:             if (double.IsNaN(sample))
271:                 return sample;
272:             lock (updateLock)
273:             {
274:                 dcp.Update(sample);
275:                 PushInput(sample);
276:                 if (primed)
277:                 {
278:                     double smoothedPeriod = dcp.DominantCyclePeriod;
279:                     double average = CalculateTrendAverage(smoothedPeriod);
280:                     trendline = Wma(average);
281:                     PushTrendAverage(average);
282: 
283:                     double phase = dcp.RawValue;
284:                     double diff = CalculateSineLeadWaveDifference(phase);
285: 
286:                     // Compute the trend mode, assuming trend by default.
287:                     isTrendMode = true;
288:                     // Measure days in trend from last the crossing of the SineWave and SineWaveLead indicator lines.
289:                     // Condition 1. A cycle mode exists for the half-period of a dominant cycle after the crossing.
290:                     if ((diff > 0 && prevSineLeadWaveDifference < 0) || (diff < 0 && prevSineLeadWaveDifference > 0))
291:                     {
292:                         isTrendMode = false;
293:                         samplesInTrend = 0;
294:                     }
295:                     prevSineLeadWaveDifference = diff;
296:                     if (++samplesInTrend < 0.5 * smoothedPeriod)
297:                         isTrendMode = false;
298: 
299:                     // Cycle mode if delta phase is ±50% of dominant cycle change of phase.
300:                     // Condition 2. A cycle mode exists if the measured phase rate of change is
301:                     // more than 2/3 the phase rate of change of the dominant cycle (360/period)
302:                     // and is less than 1.5 times the phase rate of change of the dominant cycle.
303:                     diff = phase - prevDcPhase;
304:                     prevDcPhase = phase;
305:                     if (Math.Abs(smoothedPeriod) > double.Epsilon)
306:                     {
307:                         const double minFactor = 2d / 3d, maxFactor = 1.5;
308:                         smoothedPeriod = 360d / smoothedPeriod;
309:                         if (diff > minFactor * smoothedPeriod &&
310:                             diff < maxFactor * smoothedPeriod)
311:                             isTrendMode = false;
312:                     }
313: 
314:                     // When the market makes a major reversal, it often does this with great vigor.
315:                     // When this occurs, the prices have a wide separation from the instantaneous trend line.
316:                     // Condition 3. If the WMA smoothed price is separated by more than 1.5%
317:                     // from the instantaneous trend line, then the market is in the trend mode.
318:                     if (Math.Abs(trendline) > double.Epsilon &&
319:                         Math.Abs((dcp.SmoothedValue - trendline) / trendline) >= separationFactor)
320:                         isTrendMode = true;
321: 
322:                     return isTrendMode ? 1d : -1d;
323:                 }
324:                 if (dcp.IsPrimed)
325:                 {
326:                     primed = true;
327:                     double smoothedPeriod = dcp.DominantCyclePeriod;
328:                     trendline = CalculateTrendAverage(smoothedPeriod);
329:                     trendAverage3 = trendline;
330:                     trendAverage2 = trendline;
331:                     trendAverage1 = trendline;
332: 
333:                     prevDcPhase = dcp.RawValue;
334:                     prevSineLeadWaveDifference = CalculateSineLeadWaveDifference(prevDcPhase);
335: 
336:                     isTrendMode = true;
337:                     if (++samplesInTrend < 0.5 * smoothedPeriod)
338:                         isTrendMode = false;
339: 
340:                     return isTrendMode ? 1d : -1d;
341:                 }
342:                 return 0d;
343:             }
344:         }
345:         #endregion
346: 
347:         #region Implementation
348:         private void PushInput(double val)
349:         {
350:             Array.Copy(input, 0, input, 1, inputLengthMin1);
351:             input[0] = val;
352:         }
353:         private void PushTrendAverage(double val)
354:         {
355:             trendAverage3 = trendAverage2;
356:             trendAverage2 = trendAverage1;
357:             trendAverage1 = val;
358:         }
359: 
360:         private double Wma(double val)
361:         {
362:             return coeff0 * val + coeff1 * trendAverage1 + coeff2 * trendAverage2 + coeff3 * trendAverage3;
363:         }
364: 
365:         private double CalculateTrendAverage(double smoothedPeriod)
366:         {
367:             // Compute the trend line as a simple average over the measured dominant cycle period.
368:             int length = (int)Math.Floor(smoothedPeriod * cyclePartMultiplier + 0.5);
369:             if (length > inputLength)
370:                 length = inputLength;
371:             else if (length < 1)
372:                 length = 1;
373:             double temp = 0d;
374:             for (int i = 0; i < length; ++i)
375:                 temp += input[i];
376:             return temp / length;
377:         }
378: 
379:         private double CalculateSineLeadWaveDifference(double phase)
380:         {
381:             const double pi4 = 45d * Constants.Deg2Rad;
382:             phase *= Constants.Deg2Rad;
383:             sinWave = Math.Sin(phase);
384:             sinWaveLead = Math.Sin(phase + pi4);
385:             return sinWave - sinWaveLead;
386:         }
387:         #endregion
388:     }
389: }

(End of file - total 389 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline</path>
<type>directory</type>
<entries>
hilberttransformerinstantaneoustrendline_test.go
hilberttransformerinstantaneoustrendline.go
hilberttransformerinstantaneoustrendlineoutput_test.go
hilberttransformerinstantaneoustrendlineoutput.go
hilberttransformerinstantaneoustrendlineparams.go

(5 entries)
</entries>
```

---

## Assistant (Build · claude-opus-4.7 · 5.1s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendline.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendline.go</path>
<type>file</type>
<content>1: package hilberttransformerinstantaneoustrendline
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
13: 	"zpano/indicators/johnehlers/hilberttransformer"
14: )
15: 
16: // HilbertTransformerInstantaneousTrendLine is Ehlers' Instantaneous Trend Line indicator
17: // built on top of a Hilbert transformer cycle estimator.
18: //
19: // It exposes two outputs:
20: //
21: //   - Value: the instantaneous trend line value, computed as a WMA of simple averages
22: //     over windows whose length tracks the smoothed dominant cycle period.
23: //   - DominantCyclePeriod: the additionally EMA-smoothed dominant cycle period.
24: //
25: // Reference:
26: // John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 107-112.
27: type HilbertTransformerInstantaneousTrendLine struct {
28: 	mu                             sync.RWMutex
29: 	mnemonic                       string
30: 	description                    string
31: 	mnemonicDCP                    string
32: 	descriptionDCP                 string
33: 	htce                           hilberttransformer.CycleEstimator
34: 	alphaEmaPeriodAdditional       float64
35: 	oneMinAlphaEmaPeriodAdditional float64
36: 	cyclePartMultiplier            float64
37: 	trendLineSmoothingLength       int
38: 	coeff0                         float64
39: 	coeff1                         float64
40: 	coeff2                         float64
41: 	coeff3                         float64
42: 	smoothedPeriod                 float64
43: 	value                          float64
44: 	average1                       float64
45: 	average2                       float64
46: 	average3                       float64
47: 	input                          []float64
48: 	inputLength                    int
49: 	inputLengthMin1                int
50: 	primed                         bool
51: 	barFunc                        entities.BarFunc
52: 	quoteFunc                      entities.QuoteFunc
53: 	tradeFunc                      entities.TradeFunc
54: }
55: 
56: // NewHilbertTransformerInstantaneousTrendLineDefault returns an instance of the indicator
57: // created using default values of the parameters.
58: func NewHilbertTransformerInstantaneousTrendLineDefault() (*HilbertTransformerInstantaneousTrendLine, error) {
59: 	const (
60: 		smoothingLength           = 4
61: 		alphaEmaQuadratureInPhase = 0.2
62: 		alphaEmaPeriod            = 0.2
63: 		alphaEmaPeriodAdditional  = 0.33
64: 		warmUpPeriod              = 100
65: 		trendLineSmoothingLength  = 4
66: 		cyclePartMultiplier       = 1.0
67: 	)
68: 
69: 	return newHilbertTransformerInstantaneousTrendLine(
70: 		hilberttransformer.HomodyneDiscriminator,
71: 		&hilberttransformer.CycleEstimatorParams{
72: 			SmoothingLength:           smoothingLength,
73: 			AlphaEmaQuadratureInPhase: alphaEmaQuadratureInPhase,
74: 			AlphaEmaPeriod:            alphaEmaPeriod,
75: 			WarmUpPeriod:              warmUpPeriod,
76: 		},
77: 		alphaEmaPeriodAdditional,
78: 		trendLineSmoothingLength,
79: 		cyclePartMultiplier,
80: 		0, 0, 0)
81: }
82: 
83: // NewHilbertTransformerInstantaneousTrendLineParams returns an instance of the indicator
84: // created using supplied parameters.
85: func NewHilbertTransformerInstantaneousTrendLineParams(
86: 	p *Params,
87: ) (*HilbertTransformerInstantaneousTrendLine, error) {
88: 	return newHilbertTransformerInstantaneousTrendLine(
89: 		p.EstimatorType, &p.EstimatorParams,
90: 		p.AlphaEmaPeriodAdditional,
91: 		p.TrendLineSmoothingLength,
92: 		p.CyclePartMultiplier,
93: 		p.BarComponent, p.QuoteComponent, p.TradeComponent)
94: }
95: 
96: //nolint:funlen,cyclop
97: func newHilbertTransformerInstantaneousTrendLine(
98: 	estimatorType hilberttransformer.CycleEstimatorType,
99: 	estimatorParams *hilberttransformer.CycleEstimatorParams,
100: 	alphaEmaPeriodAdditional float64,
101: 	trendLineSmoothingLength int,
102: 	cyclePartMultiplier float64,
103: 	bc entities.BarComponent, qc entities.QuoteComponent, tc entities.TradeComponent,
104: ) (*HilbertTransformerInstantaneousTrendLine, error) {
105: 	const (
106: 		invalid   = "invalid hilbert transformer instantaneous trend line parameters"
107: 		fmta      = "%s: α for additional smoothing should be in range (0, 1]"
108: 		fmttlsl   = "%s: trend line smoothing length should be 2, 3, or 4"
109: 		fmtcpm    = "%s: cycle part multiplier should be in range (0, 10]"
110: 		fmtw      = "%s: %w"
111: 		fmtn      = "htitl(%.3f, %d, %.3f%s%s)"
112: 		fmtnDCP   = "dcp(%.3f%s%s)"
113: 		four      = 4
114: 		alpha     = 0.2
115: 		descrVal  = "Hilbert transformer instantaneous trend line "
116: 		descrDCP  = "Dominant cycle period "
117: 		tlslTwo   = 2
118: 		tlslThree = 3
119: 		tlslFour  = 4
120: 		cpmMax    = 10.0
121: 	)
122: 
123: 	if alphaEmaPeriodAdditional <= 0. || alphaEmaPeriodAdditional > 1. {
124: 		return nil, fmt.Errorf(fmta, invalid)
125: 	}
126: 
127: 	if trendLineSmoothingLength < tlslTwo || trendLineSmoothingLength > tlslFour {
128: 		return nil, fmt.Errorf(fmttlsl, invalid)
129: 	}
130: 
131: 	if cyclePartMultiplier <= 0. || cyclePartMultiplier > cpmMax {
132: 		return nil, fmt.Errorf(fmtcpm, invalid)
133: 	}
134: 
135: 	// SineWave-style default: BarMedianPrice always shown in mnemonic.
136: 	if bc == 0 {
137: 		bc = entities.BarMedianPrice
138: 	}
139: 
140: 	if qc == 0 {
141: 		qc = entities.DefaultQuoteComponent
142: 	}
143: 
144: 	if tc == 0 {
145: 		tc = entities.DefaultTradeComponent
146: 	}
147: 
148: 	estimator, err := hilberttransformer.NewCycleEstimator(estimatorType, estimatorParams)
149: 	if err != nil {
150: 		return nil, fmt.Errorf(fmtw, invalid, err)
151: 	}
152: 
153: 	estimatorMoniker := ""
154: 	if estimatorType != hilberttransformer.HomodyneDiscriminator ||
155: 		estimatorParams.SmoothingLength != four ||
156: 		estimatorParams.AlphaEmaQuadratureInPhase != alpha ||
157: 		estimatorParams.AlphaEmaPeriod != alpha {
158: 		estimatorMoniker = hilberttransformer.EstimatorMoniker(estimatorType, estimator)
159: 		if len(estimatorMoniker) > 0 {
160: 			estimatorMoniker = ", " + estimatorMoniker
161: 		}
162: 	}
163: 
164: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
165: 
166: 	mnemonic := fmt.Sprintf(fmtn, alphaEmaPeriodAdditional, trendLineSmoothingLength,
167: 		cyclePartMultiplier, estimatorMoniker, componentMnemonic)
168: 	mnemonicDCP := fmt.Sprintf(fmtnDCP, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
169: 
170: 	barFunc, err := entities.BarComponentFunc(bc)
171: 	if err != nil {
172: 		return nil, fmt.Errorf(fmtw, invalid, err)
173: 	}
174: 
175: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
176: 	if err != nil {
177: 		return nil, fmt.Errorf(fmtw, invalid, err)
178: 	}
179: 
180: 	tradeFunc, err := entities.TradeComponentFunc(tc)
181: 	if err != nil {
182: 		return nil, fmt.Errorf(fmtw, invalid, err)
183: 	}
184: 
185: 	var c0, c1, c2, c3 float64
186: 
187: 	switch trendLineSmoothingLength {
188: 	case tlslTwo:
189: 		c0, c1 = 2.0/3.0, 1.0/3.0
190: 	case tlslThree:
191: 		c0, c1, c2 = 3.0/6.0, 2.0/6.0, 1.0/6.0
192: 	default: // tlslFour
193: 		c0, c1, c2, c3 = 4.0/10.0, 3.0/10.0, 2.0/10.0, 1.0/10.0
194: 	}
195: 
196: 	maxPeriod := estimator.MaxPeriod()
197: 
198: 	return &HilbertTransformerInstantaneousTrendLine{
199: 		mnemonic:                       mnemonic,
200: 		description:                    descrVal + mnemonic,
201: 		mnemonicDCP:                    mnemonicDCP,
202: 		descriptionDCP:                 descrDCP + mnemonicDCP,
203: 		htce:                           estimator,
204: 		alphaEmaPeriodAdditional:       alphaEmaPeriodAdditional,
205: 		oneMinAlphaEmaPeriodAdditional: 1. - alphaEmaPeriodAdditional,
206: 		cyclePartMultiplier:            cyclePartMultiplier,
207: 		trendLineSmoothingLength:       trendLineSmoothingLength,
208: 		coeff0:                         c0,
209: 		coeff1:                         c1,
210: 		coeff2:                         c2,
211: 		coeff3:                         c3,
212: 		input:                          make([]float64, maxPeriod),
213: 		inputLength:                    maxPeriod,
214: 		inputLengthMin1:                maxPeriod - 1,
215: 		barFunc:                        barFunc,
216: 		quoteFunc:                      quoteFunc,
217: 		tradeFunc:                      tradeFunc,
218: 	}, nil
219: }
220: 
221: // IsPrimed indicates whether an indicator is primed.
222: func (s *HilbertTransformerInstantaneousTrendLine) IsPrimed() bool {
223: 	s.mu.RLock()
224: 	defer s.mu.RUnlock()
225: 
226: 	return s.primed
227: }
228: 
229: // Metadata describes an output data of the indicator.
230: func (s *HilbertTransformerInstantaneousTrendLine) Metadata() core.Metadata {
231: 	return core.Metadata{
232: 		Type:        core.HilbertTransformerInstantaneousTrendLine,
233: 		Mnemonic:    s.mnemonic,
234: 		Description: s.description,
235: 		Outputs: []outputs.Metadata{
236: 			{
237: 				Kind:        int(Value),
238: 				Type:        outputs.ScalarType,
239: 				Mnemonic:    s.mnemonic,
240: 				Description: s.description,
241: 			},
242: 			{
243: 				Kind:        int(DominantCyclePeriod),
244: 				Type:        outputs.ScalarType,
245: 				Mnemonic:    s.mnemonicDCP,
246: 				Description: s.descriptionDCP,
247: 			},
248: 		},
249: 	}
250: }
251: 
252: // Update updates the value of the indicator given the next sample, returning the
253: // (value, period) pair. Returns NaN values if the indicator is not yet primed.
254: func (s *HilbertTransformerInstantaneousTrendLine) Update(sample float64) (float64, float64) {
255: 	if math.IsNaN(sample) {
256: 		return sample, sample
257: 	}
258: 
259: 	s.mu.Lock()
260: 	defer s.mu.Unlock()
261: 
262: 	s.htce.Update(sample)
263: 	s.pushInput(sample)
264: 
265: 	if s.primed {
266: 		s.smoothedPeriod = s.alphaEmaPeriodAdditional*s.htce.Period() +
267: 			s.oneMinAlphaEmaPeriodAdditional*s.smoothedPeriod
268: 		average := s.calculateAverage()
269: 		s.value = s.coeff0*average + s.coeff1*s.average1 + s.coeff2*s.average2 + s.coeff3*s.average3
270: 		s.average3 = s.average2
271: 		s.average2 = s.average1
272: 		s.average1 = average
273: 
274: 		return s.value, s.smoothedPeriod
275: 	}
276: 
277: 	if s.htce.Primed() {
278: 		s.primed = true
279: 		s.smoothedPeriod = s.htce.Period()
280: 		avg := s.calculateAverage()
281: 		s.value = avg
282: 		s.average1 = avg
283: 		s.average2 = avg
284: 		s.average3 = avg
285: 
286: 		return s.value, s.smoothedPeriod
287: 	}
288: 
289: 	nan := math.NaN()
290: 
291: 	return nan, nan
292: }
293: 
294: // UpdateScalar updates the indicator given the next scalar sample.
295: func (s *HilbertTransformerInstantaneousTrendLine) UpdateScalar(sample *entities.Scalar) core.Output {
296: 	return s.updateEntity(sample.Time, sample.Value)
297: }
298: 
299: // UpdateBar updates the indicator given the next bar sample.
300: func (s *HilbertTransformerInstantaneousTrendLine) UpdateBar(sample *entities.Bar) core.Output {
301: 	return s.updateEntity(sample.Time, s.barFunc(sample))
302: }
303: 
304: // UpdateQuote updates the indicator given the next quote sample.
305: func (s *HilbertTransformerInstantaneousTrendLine) UpdateQuote(sample *entities.Quote) core.Output {
306: 	return s.updateEntity(sample.Time, s.quoteFunc(sample))
307: }
308: 
309: // UpdateTrade updates the indicator given the next trade sample.
310: func (s *HilbertTransformerInstantaneousTrendLine) UpdateTrade(sample *entities.Trade) core.Output {
311: 	return s.updateEntity(sample.Time, s.tradeFunc(sample))
312: }
313: 
314: func (s *HilbertTransformerInstantaneousTrendLine) updateEntity(t time.Time, sample float64) core.Output {
315: 	const length = 2
316: 
317: 	value, period := s.Update(sample)
318: 
319: 	output := make([]any, length)
320: 	output[0] = entities.Scalar{Time: t, Value: value}
321: 	output[1] = entities.Scalar{Time: t, Value: period}
322: 
323: 	return output
324: }
325: 
326: func (s *HilbertTransformerInstantaneousTrendLine) pushInput(value float64) {
327: 	copy(s.input[1:], s.input[:s.inputLengthMin1])
328: 	s.input[0] = value
329: }
330: 
331: func (s *HilbertTransformerInstantaneousTrendLine) calculateAverage() float64 {
332: 	const half = 0.5
333: 
334: 	// Compute the trend line as a simple average over the measured dominant cycle period.
335: 	length := int(math.Floor(s.smoothedPeriod*s.cyclePartMultiplier + half))
336: 	if length > s.inputLength {
337: 		length = s.inputLength
338: 	} else if length < 1 {
339: 		length = 1
340: 	}
341: 
342: 	var sum float64
343: 	for i := 0; i < length; i++ {
344: 		sum += s.input[i]
345: 	}
346: 
347: 	return sum / float64(length)
348: }

(End of file - total 348 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.7s)

_Thinking:_

 I'm checking how SineWave handles wrapping DominantCycle, managing output enums, and configuring parameters.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendlineoutput.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendlineoutput.go</path>
<type>file</type>
<content>1: package hilberttransformerinstantaneoustrendline
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
12: 	// Value is the instantaneous trend line value.
13: 	Value Output = iota + 1
14: 	// DominantCyclePeriod is the smoothed dominant cycle period.
15: 	DominantCyclePeriod
16: 	outputLast
17: )
18: 
19: const (
20: 	valueStr               = "value"
21: 	dominantCyclePeriodStr = "dominantCyclePeriod"
22: 	unknownStr             = "unknown"
23: )
24: 
25: // String implements the Stringer interface.
26: func (o Output) String() string {
27: 	switch o {
28: 	case Value:
29: 		return valueStr
30: 	case DominantCyclePeriod:
31: 		return dominantCyclePeriodStr
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
45: 		errFmt = "cannot marshal '%s': unknown hilbert transformer instantaneous trend line output"
46: 		extra  = 2   // Two bytes for quotes.
47: 		dqc    = '"' // Double quote character.
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
66: 		errFmt = "cannot unmarshal '%s': unknown hilbert transformer instantaneous trend line output"
67: 		dqs    = "\"" // Double quote string.
68: 	)
69: 
70: 	d := bytes.Trim(data, dqs)
71: 	s := string(d)
72: 
73: 	switch s {
74: 	case valueStr:
75: 		*o = Value
76: 	case dominantCyclePeriodStr:
77: 		*o = DominantCyclePeriod
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
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendlineparams.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendlineparams.go</path>
<type>file</type>
<content>1: package hilberttransformerinstantaneoustrendline
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
33: 	// BarComponent indicates the component of a bar to use when updating the indicator with a bar sample.
34: 	//
35: 	// If zero, the default (BarMedianPrice) is used. Since the default differs from the framework
36: 	// default bar component, it is always shown in the indicator mnemonic.
37: 	BarComponent entities.BarComponent
38: 
39: 	// QuoteComponent indicates the component of a quote to use when updating the indicator with a quote sample.
40: 	//
41: 	// If zero, the default (QuoteMidPrice) is used and the component is not shown in the indicator mnemonic.
42: 	QuoteComponent entities.QuoteComponent
43: 
44: 	// TradeComponent indicates the component of a trade to use when updating the indicator with a trade sample.
45: 	//
46: 	// If zero, the default (TradePrice) is used and the component is not shown in the indicator mnemonic.
47: 	TradeComponent entities.TradeComponent
48: }

(End of file - total 48 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.5s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/sinewave/sinewave.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/sinewave/sinewave.go</path>
<type>file</type>
<content>1: package sinewave
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
20: // SineWave is the Ehlers' Sine Wave indicator.
21: //
22: // It exposes five outputs:
23: //
24: //   - Value: the sine wave value, sin(phase·Deg2Rad).
25: //   - Lead: the sine wave lead value, sin((phase+45)·Deg2Rad).
26: //   - Band: a band with Upper=Value and Lower=Lead.
27: //   - DominantCyclePeriod: the smoothed dominant cycle period.
28: //   - DominantCyclePhase: the dominant cycle phase, in degrees.
29: //
30: // Reference:
31: // John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 95-105.
32: type SineWave struct {
33: 	mu                 sync.RWMutex
34: 	mnemonic           string
35: 	description        string
36: 	mnemonicLead       string
37: 	descriptionLead    string
38: 	mnemonicBand       string
39: 	descriptionBand    string
40: 	mnemonicDCP        string
41: 	descriptionDCP     string
42: 	mnemonicDCPhase    string
43: 	descriptionDCPhase string
44: 	dc                 *dominantcycle.DominantCycle
45: 	primed             bool
46: 	value              float64
47: 	lead               float64
48: 	barFunc            entities.BarFunc
49: 	quoteFunc          entities.QuoteFunc
50: 	tradeFunc          entities.TradeFunc
51: }
52: 
53: // NewSineWaveDefault returns an instance of the indicator
54: // created using default values of the parameters.
55: func NewSineWaveDefault() (*SineWave, error) {
56: 	const (
57: 		smoothingLength           = 4
58: 		alphaEmaQuadratureInPhase = 0.2
59: 		alphaEmaPeriod            = 0.2
60: 		alphaEmaPeriodAdditional  = 0.33
61: 		warmUpPeriod              = 100
62: 	)
63: 
64: 	return newSineWave(
65: 		hilberttransformer.HomodyneDiscriminator,
66: 		&hilberttransformer.CycleEstimatorParams{
67: 			SmoothingLength:           smoothingLength,
68: 			AlphaEmaQuadratureInPhase: alphaEmaQuadratureInPhase,
69: 			AlphaEmaPeriod:            alphaEmaPeriod,
70: 			WarmUpPeriod:              warmUpPeriod,
71: 		},
72: 		alphaEmaPeriodAdditional,
73: 		0, 0, 0)
74: }
75: 
76: // NewSineWaveParams returns an instance of the indicator created using supplied parameters.
77: func NewSineWaveParams(p *Params) (*SineWave, error) {
78: 	return newSineWave(
79: 		p.EstimatorType, &p.EstimatorParams,
80: 		p.AlphaEmaPeriodAdditional,
81: 		p.BarComponent, p.QuoteComponent, p.TradeComponent)
82: }
83: 
84: //nolint:funlen
85: func newSineWave(
86: 	estimatorType hilberttransformer.CycleEstimatorType,
87: 	estimatorParams *hilberttransformer.CycleEstimatorParams,
88: 	alphaEmaPeriodAdditional float64,
89: 	bc entities.BarComponent, qc entities.QuoteComponent, tc entities.TradeComponent,
90: ) (*SineWave, error) {
91: 	const (
92: 		invalid    = "invalid sine wave parameters"
93: 		fmta       = "%s: α for additional smoothing should be in range (0, 1]"
94: 		fmtw       = "%s: %w"
95: 		fmtnVal    = "sw(%.3f%s%s)"
96: 		fmtnLead   = "sw-lead(%.3f%s%s)"
97: 		fmtnBand   = "sw-band(%.3f%s%s)"
98: 		fmtnDCP    = "dcp(%.3f%s%s)"
99: 		fmtnDCPha  = "dcph(%.3f%s%s)"
100: 		four       = 4
101: 		alpha      = 0.2
102: 		descrVal   = "Sine wave "
103: 		descrLead  = "Sine wave lead "
104: 		descrBand  = "Sine wave band "
105: 		descrDCP   = "Dominant cycle period "
106: 		descrDCPha = "Dominant cycle phase "
107: 	)
108: 
109: 	if alphaEmaPeriodAdditional <= 0. || alphaEmaPeriodAdditional > 1. {
110: 		return nil, fmt.Errorf(fmta, invalid)
111: 	}
112: 
113: 	// Resolve defaults for component functions.
114: 	// SineWave defaults to BarMedianPrice (not the framework default), so it always shows in the mnemonic.
115: 	if bc == 0 {
116: 		bc = entities.BarMedianPrice
117: 	}
118: 
119: 	if qc == 0 {
120: 		qc = entities.DefaultQuoteComponent
121: 	}
122: 
123: 	if tc == 0 {
124: 		tc = entities.DefaultTradeComponent
125: 	}
126: 
127: 	// Build the inner DominantCycle with explicit components.
128: 	dcParams := &dominantcycle.Params{
129: 		EstimatorType:            estimatorType,
130: 		EstimatorParams:          *estimatorParams,
131: 		AlphaEmaPeriodAdditional: alphaEmaPeriodAdditional,
132: 		BarComponent:             bc,
133: 		QuoteComponent:           qc,
134: 		TradeComponent:           tc,
135: 	}
136: 
137: 	dc, err := dominantcycle.NewDominantCycleParams(dcParams)
138: 	if err != nil {
139: 		return nil, fmt.Errorf(fmtw, invalid, err)
140: 	}
141: 
142: 	// Compose the estimator moniker (same logic as DominantCycle).
143: 	estimator, err := hilberttransformer.NewCycleEstimator(estimatorType, estimatorParams)
144: 	if err != nil {
145: 		return nil, fmt.Errorf(fmtw, invalid, err)
146: 	}
147: 
148: 	estimatorMoniker := ""
149: 	if estimatorType != hilberttransformer.HomodyneDiscriminator ||
150: 		estimatorParams.SmoothingLength != four ||
151: 		estimatorParams.AlphaEmaQuadratureInPhase != alpha ||
152: 		estimatorParams.AlphaEmaPeriod != alpha {
153: 		estimatorMoniker = hilberttransformer.EstimatorMoniker(estimatorType, estimator)
154: 		if len(estimatorMoniker) > 0 {
155: 			estimatorMoniker = ", " + estimatorMoniker
156: 		}
157: 	}
158: 
159: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
160: 
161: 	mnemonicVal := fmt.Sprintf(fmtnVal, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
162: 	mnemonicLead := fmt.Sprintf(fmtnLead, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
163: 	mnemonicBand := fmt.Sprintf(fmtnBand, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
164: 	mnemonicDCP := fmt.Sprintf(fmtnDCP, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
165: 	mnemonicDCPha := fmt.Sprintf(fmtnDCPha, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
166: 
167: 	barFunc, err := entities.BarComponentFunc(bc)
168: 	if err != nil {
169: 		return nil, fmt.Errorf(fmtw, invalid, err)
170: 	}
171: 
172: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
173: 	if err != nil {
174: 		return nil, fmt.Errorf(fmtw, invalid, err)
175: 	}
176: 
177: 	tradeFunc, err := entities.TradeComponentFunc(tc)
178: 	if err != nil {
179: 		return nil, fmt.Errorf(fmtw, invalid, err)
180: 	}
181: 
182: 	nan := math.NaN()
183: 
184: 	return &SineWave{
185: 		mnemonic:           mnemonicVal,
186: 		description:        descrVal + mnemonicVal,
187: 		mnemonicLead:       mnemonicLead,
188: 		descriptionLead:    descrLead + mnemonicLead,
189: 		mnemonicBand:       mnemonicBand,
190: 		descriptionBand:    descrBand + mnemonicBand,
191: 		mnemonicDCP:        mnemonicDCP,
192: 		descriptionDCP:     descrDCP + mnemonicDCP,
193: 		mnemonicDCPhase:    mnemonicDCPha,
194: 		descriptionDCPhase: descrDCPha + mnemonicDCPha,
195: 		dc:                 dc,
196: 		value:              nan,
197: 		lead:               nan,
198: 		barFunc:            barFunc,
199: 		quoteFunc:          quoteFunc,
200: 		tradeFunc:          tradeFunc,
201: 	}, nil
202: }
203: 
204: // IsPrimed indicates whether an indicator is primed.
205: func (s *SineWave) IsPrimed() bool {
206: 	s.mu.RLock()
207: 	defer s.mu.RUnlock()
208: 
209: 	return s.primed
210: }
211: 
212: // Metadata describes an output data of the indicator.
213: func (s *SineWave) Metadata() core.Metadata {
214: 	return core.Metadata{
215: 		Type:        core.SineWave,
216: 		Mnemonic:    s.mnemonic,
217: 		Description: s.description,
218: 		Outputs: []outputs.Metadata{
219: 			{
220: 				Kind:        int(Value),
221: 				Type:        outputs.ScalarType,
222: 				Mnemonic:    s.mnemonic,
223: 				Description: s.description,
224: 			},
225: 			{
226: 				Kind:        int(Lead),
227: 				Type:        outputs.ScalarType,
228: 				Mnemonic:    s.mnemonicLead,
229: 				Description: s.descriptionLead,
230: 			},
231: 			{
232: 				Kind:        int(Band),
233: 				Type:        outputs.BandType,
234: 				Mnemonic:    s.mnemonicBand,
235: 				Description: s.descriptionBand,
236: 			},
237: 			{
238: 				Kind:        int(DominantCyclePeriod),
239: 				Type:        outputs.ScalarType,
240: 				Mnemonic:    s.mnemonicDCP,
241: 				Description: s.descriptionDCP,
242: 			},
243: 			{
244: 				Kind:        int(DominantCyclePhase),
245: 				Type:        outputs.ScalarType,
246: 				Mnemonic:    s.mnemonicDCPhase,
247: 				Description: s.descriptionDCPhase,
248: 			},
249: 		},
250: 	}
251: }
252: 
253: // Update updates the value of the indicator given the next sample, returning
254: // the (value, lead, period, phase) tuple. Returns NaN values if the indicator is not yet primed.
255: func (s *SineWave) Update(sample float64) (float64, float64, float64, float64) {
256: 	if math.IsNaN(sample) {
257: 		return sample, sample, sample, sample
258: 	}
259: 
260: 	// Delegate to the inner DominantCycle. Its own mutex is independent.
261: 	_, period, phase := s.dc.Update(sample)
262: 
263: 	s.mu.Lock()
264: 	defer s.mu.Unlock()
265: 
266: 	if math.IsNaN(phase) {
267: 		nan := math.NaN()
268: 
269: 		return nan, nan, nan, nan
270: 	}
271: 
272: 	const leadOffset = 45.0
273: 
274: 	s.primed = true
275: 	s.value = math.Sin(phase * deg2Rad)
276: 	s.lead = math.Sin((phase + leadOffset) * deg2Rad)
277: 
278: 	return s.value, s.lead, period, phase
279: }
280: 
281: // UpdateScalar updates the indicator given the next scalar sample.
282: func (s *SineWave) UpdateScalar(sample *entities.Scalar) core.Output {
283: 	return s.updateEntity(sample.Time, sample.Value)
284: }
285: 
286: // UpdateBar updates the indicator given the next bar sample.
287: func (s *SineWave) UpdateBar(sample *entities.Bar) core.Output {
288: 	return s.updateEntity(sample.Time, s.barFunc(sample))
289: }
290: 
291: // UpdateQuote updates the indicator given the next quote sample.
292: func (s *SineWave) UpdateQuote(sample *entities.Quote) core.Output {
293: 	return s.updateEntity(sample.Time, s.quoteFunc(sample))
294: }
295: 
296: // UpdateTrade updates the indicator given the next trade sample.
297: func (s *SineWave) UpdateTrade(sample *entities.Trade) core.Output {
298: 	return s.updateEntity(sample.Time, s.tradeFunc(sample))
299: }
300: 
301: func (s *SineWave) updateEntity(time time.Time, sample float64) core.Output {
302: 	const length = 5
303: 
304: 	value, lead, period, phase := s.Update(sample)
305: 
306: 	output := make([]any, length)
307: 	i := 0
308: 	output[i] = entities.Scalar{Time: time, Value: value}
309: 	i++
310: 	output[i] = entities.Scalar{Time: time, Value: lead}
311: 	i++
312: 	output[i] = outputs.Band{Time: time, Upper: value, Lower: lead}
313: 	i++
314: 	output[i] = entities.Scalar{Time: time, Value: period}
315: 	i++
316: 	output[i] = entities.Scalar{Time: time, Value: phase}
317: 
318: 	return output
319: }

(End of file - total 319 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.1s)

_Thinking:_

 Time to examine the MBST test file for reference arrays and then implement the Go TCM version.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleModeTest.cs"
}
```

**Output:**
```
<path>/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleModeTest.cs</path>
<type>file</type>
<content>1: ﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
2: using System;
3: using System.Collections.Generic;
4: using System.IO;
5: using System.Runtime.Serialization;
6: using System.Xml;
7: using Mbst.Trading;
8: using Mbst.Trading.Indicators.JohnEhlers;
9: 
10: namespace Tests.Indicators.JohnEhlers
11: {
12:     [TestClass]
13:     public class TrendCycleModeTest
14:     {
15:         #region Test data
16:         /// <summary>
17:         /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MAMA.xsl, price, D5…D256, 252 entries.
18:         /// </summary>
19:         private readonly List<double> rawInput = new List<double>
20:         {
21:              92.0000,  93.1725,  95.3125,  94.8450,  94.4075,  94.1100,  93.5000,  91.7350,  90.9550,  91.6875,
22:              94.5000,  97.9700,  97.5775,  90.7825,  89.0325,  92.0950,  91.1550,  89.7175,  90.6100,  91.0000,
23:              88.9225,  87.5150,  86.4375,  83.8900,  83.0025,  82.8125,  82.8450,  86.7350,  86.8600,  87.5475,
24:              85.7800,  86.1725,  86.4375,  87.2500,  88.9375,  88.2050,  85.8125,  84.5950,  83.6575,  84.4550,
25:              83.5000,  86.7825,  88.1725,  89.2650,  90.8600,  90.7825,  91.8600,  90.3600,  89.8600,  90.9225,
26:              89.5000,  87.6725,  86.5000,  84.2825,  82.9075,  84.2500,  85.6875,  86.6100,  88.2825,  89.5325,
27:              89.5000,  88.0950,  90.6250,  92.2350,  91.6725,  92.5925,  93.0150,  91.1725,  90.9850,  90.3775,
28:              88.2500,  86.9075,  84.0925,  83.1875,  84.2525,  97.8600,  99.8750, 103.2650, 105.9375, 103.5000,
29:             103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,
30:             120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,
31:             114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,
32:             114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,
33:             124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,
34:             137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,
35:             123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,
36:             122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,
37:             123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,
38:             130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,
39:             127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,
40:             121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,
41:             106.9700, 110.0300,  91.0000,  93.5600,  93.6200,  95.3100,  94.1850,  94.7800,  97.6250,  97.5900,
42:              95.2500,  94.7200,  92.2200,  91.5650,  92.2200,  93.8100,  95.5900,  96.1850,  94.6250,  95.1200,
43:              94.0000,  93.7450,  95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,
44:             103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,
45:             106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,
46:             109.5300, 108.0600
47:         };
48: 
49:         /// <summary>
50:         /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MAMA.xsl, Period Adjustment, X5…X256, 252 entries, smoothed as AI18=0.33*X18+0.67*AI17.
51:         /// </summary>
52:         private readonly List<double> dcPeriod = new List<double>
53:         {
54:              0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,
55:              0.00000000000000,  0.00000000000000,  0.39600000000000,  0.97812000000000,  1.62158040000000,  2.25545086800000,  2.84234568156000,  3.36868456664520,  3.86776291565229,  4.36321983508703,
56:              4.87235783926831,  5.40838035704577,  5.98190550443027,  6.60199641969884,  7.27686930610184,  8.01438731048222,  8.82241286095647,  9.70906731606755, 10.68293087091460, 11.75320502957710,
57:             12.92985285048740, 14.22372743856440, 15.55272286275450, 16.77503611571540, 17.85814025111630, 18.72970387649220, 19.30387978646140, 19.63314544969620, 19.86256979430250, 20.09030968609300,
58:             20.24817834009410, 20.31132870798190, 20.52152604110820, 21.27119054536480, 22.10966835167300, 22.28715460952700, 21.91280773257140, 21.23923470724180, 20.70813161651310, 20.20449150221090,
59:             19.52863321263000, 18.73709250583170, 17.96311275281150, 17.33367762545960, 16.91743352044750, 16.64300564862120, 16.41952162419500, 16.27464914327850, 16.26425245094380, 16.33321577028600,
60:             16.39265551523990, 16.39976990202790, 16.38221107536320, 16.37405059271910, 16.35102942468120, 16.26839438425590, 16.12432207371240, 15.99529098667630, 15.96458956780100, 16.07977539207760,
61:             16.38360881255670, 16.79746341307210, 17.18753188776420, 17.58524022168910, 18.05888760471740, 18.46077773999830, 18.78691120238400, 19.07789869381110, 19.11803073417110, 18.72675385299730,
62:             18.07403190737810, 17.72999456892580, 18.00699920187680, 18.06680349806270, 17.73551482016550, 17.28467833183610, 16.97456900115070, 16.89386283663200, 17.00556420464730, 17.21986959021550,
63:             17.48251598471900, 17.79647268844360, 18.15809655229470, 18.56044162987590, 19.03705300462320, 19.61779465906120, 20.13838368155990, 20.58144279802850, 20.93554178712450, 21.09578565733870,
64:             21.19426268582890, 21.35270550953270, 21.46806615214910, 21.43420235778580, 21.27320458618770, 20.98617884905010, 20.62345107800030, 20.32165030848570, 20.09921951571820, 19.88214300840560,
65:             19.67081622699810, 19.55217428481160, 19.60485773311710, 19.77836095343260, 19.77886122563300, 19.59009982815140, 19.54609435364200, 19.77945658439880, 20.20526697824140, 20.80572859375930,
66:             21.45882440191380, 21.50916115262280, 21.07219135457730, 20.33979206665380, 19.60807769029340, 19.15831017112920, 19.03006205140340, 19.23359250887840, 19.84206353515510, 20.83692898803630,
67:             22.25776348341490, 23.50933063567320, 24.02857349775940, 24.28548086650010, 24.74576845262060, 25.45685387492870, 26.31998583396390, 27.14553013410700, 27.80677101851790, 28.50146147525040,
68:             29.16835938704370, 29.38723525724370, 29.55886298198770, 30.43981360336700, 30.70779370313880, 30.20667454311960, 29.23518282361370, 28.00037502954910, 26.95505291681000, 26.22399862702800,
69:             25.67716809996900, 25.19893752937060, 24.76924271120940, 24.40654607774420, 24.12997738279210, 24.02648590415090, 24.17912316847620, 24.26552607123530, 24.07565548132200, 23.81050493977940,
70:             23.75771490624360, 24.01627030476950, 24.42884190933990, 24.55867905189440, 24.41978729840000, 24.33536819272640, 24.11887925396970, 23.53741527509780, 22.66734716257270, 21.70419061052260,
71:             20.78848949032480, 19.92593130809770, 19.09528115584620, 18.35405205698280, 17.81539769318840, 17.53491540732180, 17.59552736216070, 18.09376127214910, 18.69300796204700, 19.10361709066390,
72:             19.40368660687600, 19.79324964337850, 20.14261316711870, 20.30292592814370, 20.37642955508450, 20.42856373321320, 20.36417897031590, 20.26870923265670, 20.31691792510900, 20.52593924664440,
73:             20.95797078839970, 21.70315565998060, 22.68588957914270, 23.95566588814480, 25.30036991408680, 26.49222048853470, 28.24485763802440, 30.46863151925310, 31.19661794415910, 30.97271495031300,
74:             30.15801520320610, 29.52193986806340, 28.48090879451130, 27.20913817575940, 25.84740758865390, 24.75875079095690, 24.57820671512040, 25.25622655282780, 26.58938264946150, 28.44936832011270,
75:             30.75900691394640, 31.63120735338530, 31.95156902113430, 32.19329221743080, 32.18129930292270, 31.78927079951340, 30.94427836437330, 29.74153261553520, 28.44319750131350, 27.27756983469050,
76:             26.30928991862760, 25.59706087830910, 25.19354035279110, 24.98183319418390, 24.66611779383150, 24.13629363553260, 23.59372342374540, 23.45943359521940, 24.13462330023790, 25.42868068174450,
77:             27.22154743441240, 28.85990121754770, 29.25658159944000, 28.86760790158470, 28.27077502042400, 27.83957963686970, 27.56292753489200, 27.31665028261770, 27.11537844471070, 27.05619511102920,
78:             26.72669604084850, 25.93839467294110, 24.88015320695530, 23.98089561843900, 23.51115215671300, 23.02173482203020, 22.29674643126940, 21.42162141795630, 20.54863761751100, 19.78167187971360,
79:             19.14387361712880, 18.61396641752300
80:         };
81: 
82:         /// <summary>
83:         /// Generated using Excel implementation from TA-Lib (http://ta-lib.org/) tests, test_HT.xsl.
84:         /// </summary>
85:         private readonly List<double> dcPhase = new List<double>
86:         {
87:             double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,
88:             double.NaN,  double.NaN,  double.NaN,
89:             639.09090909090900,  98.05299963194700, 132.00564338345400,  69.61331949528320,  72.53929158292410,  49.31988383698000,  70.57064052204580,  95.44397344939550,  15.53098102018940,  -7.59977335936253,
90:              -0.43908926482948,   0.75159304711349,   6.81682854281200,  10.38041464097730,  14.60138862232650,  23.61952231391600,  34.25173964222910,  45.61939612889610,  54.93203281582900,  62.34730431858280,
91:              59.09332873285360,  64.74787861710400,  71.38960942346380,  78.26591034671290,  95.01873223899610, 100.81260420916600, 122.15965196914300, 155.92351856084900, 203.60327223472200, 237.36293513970900,
92:             244.38539212971100, 260.58654095568100, 254.66267427143100, 253.71116813655900, 126.51252837026600, 108.91084071926300, 120.62704116849900, 135.00432720483700, 148.37778407551400, 166.73764548652900,
93:             192.70016208445300, 227.19271648131900, 250.93069421530800, 272.91238631233800, 306.14557952390500, -30.56440912946640,  -7.29779366562593,  15.79435681229920,  37.27143784018100,  57.28482204365770,
94:              77.89434659794900, 100.26640790428300, 122.46792347519500, 143.34860922315100, 161.97099218663000, 179.04173949124300, 194.28337763382200, 207.12978658551400, 219.12056620855700, 228.99420937560000,
95:             246.39888033834300, 259.83330071373300, 279.43099672242300, 300.38982045382800, -42.95237520761510,  10.71059944380160,  67.91671263897760,  95.69661875079390, 116.17343846855600, 131.69663648791300,
96:             145.22435488266800, 157.96979483912800, 170.90824231177600, 190.12565895164300, 201.99948636973700, 211.84934448916700, 219.50941069822200, 223.55536694633000, 217.70267385839600, 208.61154857738600,
97:             187.20930268786500, 183.53325881754400, 173.77461459542900, 177.09324308654900, 177.36771678646600, 183.46254384786600, 190.38499136923400, 200.15247884572800, 210.26397611554500, 219.92876325408700,
98:             230.19440003342700, 241.78128922383700, 255.09398089013500, 269.90233026686000, 287.31443195349700, -22.67219169276530,  21.31042453594620,  55.99561073880190,  76.28592721136950,  93.25596317796150,
99:             111.58409337397600, 131.21106018529500, 148.25324978238800, 161.34668836868300, 167.25425018604100, 166.56119294454900, 164.79606018404900, 165.59339091940400, 167.71720854219100, 171.16337795073400,
100:             177.90397416349400, 180.76168462321900, 181.18022018527700, 182.90279528585400, 185.03420448405800, 187.45543006582800, 185.33720976352800, 183.30316508274000, 181.31467258646500, 177.91862390253700,
101:             179.98928550766400, 182.00429308763000, 183.62347330064100, 185.57299458485300, 189.35191538410900, 194.23220405450000, 200.11245439803000, 206.61598766532100, 214.60761785986900, 223.17826798412900,
102:             229.59983294953500, 238.43424165042300, 244.29554481830400, 259.55186325451500, 277.88404982202100, 297.46674550039700, -42.41381860502280, -22.05561416691630, -10.76874195668260,   4.55423903808060,
103:              13.05706378617280,  25.49753552893060,  31.99805128632640,  38.60334291217220,  45.70890152749460,  53.48688015996070,  60.82873486408750,  67.21350248296350,  74.98104788942620,  84.48479832948730,
104:              97.41668808537870, 101.86825371917200, 131.27099687518600, 148.43969963128300, 169.23915874881900, 186.79876808562800, 222.27683786450500, 226.80174114624100, 148.12023947867300, 119.78684089821000,
105:             123.91411010078200, 136.35086145099600, 148.39281918279000, 160.94079449625800, 171.89311802023800, 182.21130213571000, 189.94824024493000, 202.58335256387900, 215.72849327557900, 224.87477584909900,
106:             239.93794152235800, 257.07736337068300, 274.90021437724400, 292.81710751479900, 310.18755119542100, -34.09748283129430, -20.55209191423000, -17.48926436788570,  -6.64084597196063,  -3.41661969757843,
107:              -1.11706604110969,  -0.61559202318233,  -0.58343503934742,  -0.97020892968976,  -7.33789387098142, -13.71942088996000, -14.16303590443250,  -9.85074940762922,  -2.93111556519989,   0.35846302482548,
108:              -0.27797938543370,  -4.11254536082527,  -7.19786584394603,  -8.12930435521150,  -7.23274284499956,  -5.60008181003320,  -3.98246383052538,  -1.93459828828531,  -0.91376116945821,   1.11347590999549,
109:               3.48574296192987,   5.87739974191743,   8.51611669495514,  11.77045158406290,  16.98321519660290,  23.12127015453780,  32.37560208179040,  38.09489298723020,  44.37798569415560,  48.60625731428030,
110:              57.00174598372450,  65.55585833338270,  77.53688240972470,  96.95830008657520, 111.55989345666400, 124.32277400743800, 131.41492641407500, 137.73657404096000, 141.46543104438800, 142.80498887855200,
111:             149.73966957805500, 153.53899811794900, 157.79183782289500, 159.94501203849600, 162.96541156987000, 170.96418133015300, 176.71998519472000, 182.65581927371500, 189.05283471879000, 197.04766728263800,
112:             206.92813680000000, 217.32451174036000, 224.38800791349600, 232.20526011220900, 246.06238925337200, 271.09605242709400, 289.11924018406000,  -31.18231981512890, 23.26913353342980,  47.27652706672060
113:         };
114: 
115:         /// <summary>
116:         /// Generated using Excel implementation from TA-Lib (http://ta-lib.org/) tests, test_HT.xsl.
117:         /// </summary>
118:         private readonly List<double> sine = new List<double>
119:         {
120:             double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,
121:             double.NaN, double.NaN,  double.NaN,
122:             -0.9874388886763950, 0.9901389074597280, 0.7430789154373040, 0.9373629963767220, 0.9539229407379340, 0.7583605937417590, 0.9430523282622310, 0.9954894450161860, 0.2677593920226290,-0.1322524693787770,
123:             -0.0076634783681766, 0.0131173971015221, 0.1186956105503800, 0.1801829214581150, 0.2520928115991770, 0.4006612403521310, 0.5628300247542610, 0.7147094931388430, 0.8184710623910560, 0.8857771050315170,
124:              0.8580051054352940, 0.9044393510772560, 0.9477105512269880, 0.9791019835938800, 0.9961661502181070, 0.9822460058918840, 0.8465682109433450, 0.4079557291761370,-0.4004013665430420,-0.8421036878603060,
125:             -0.9017223344864390,-0.9865337683617420,-0.9643853118844240,-0.9598599815079740, 0.8037267766705520, 0.9460240546952540, 0.8605016852751490, 0.7070533756573690, 0.5243161160505560, 0.2294102723810260,
126:             -0.2198489640488240,-0.7336434870569610,-0.9451240721288890,-0.9987083965673280,-0.8075209150605320,-0.5085066436721070,-0.1270264128351010, 0.2721854747475370, 0.6055917784886910, 0.8413676398899490,
127:              0.9777625488346960, 0.9839896992562860, 0.8436921159626520, 0.5969447127483850, 0.3094984574050860, 0.0167240212732023,-0.2467178763145330,-0.4560076441356610,-0.6309543271436050,-0.7546432714599400,
128:             -0.9163549058535820,-0.9842983646399860,-0.9864836587318940,-0.8626035608908470,-0.6813902164376480, 0.1858483908516400, 0.9266383325265240, 0.9950614294774160, 0.8974629492620320, 0.7466772329212920,
129:              0.5703644678660200, 0.3750953336668390, 0.1580160207328650,-0.1758076016904010,-0.3745982816248290,-0.5276875478007350,-0.6362049492364280,-0.6890552092033720,-0.6115639640491650,-0.4788688150780810,
130:             -0.1254943141670760,-0.0616279223840421, 0.1084398125157010, 0.0507107190849194, 0.0459258492372133,-0.0603960127005548,-0.1802614924906120,-0.3445196936393600,-0.5039846767879240,-0.6418346779779860,
131:             -0.7682209569752370,-0.8811490866996920,-0.9663490613441720,-0.9999985470701830,-0.9546858649139780,-0.3854582462377580, 0.3634207386677970, 0.8289947319983170, 0.9714909193370010, 0.9983857647297360,
132:              0.9298786498974830, 0.7522877436715410, 0.5261656913352240, 0.3198410350076260, 0.2206250837743810, 0.2324067219830480, 0.2622555352188150, 0.2488016117878720, 0.2127369252136960, 0.1536174562004720,
133:              0.0365743929645966,-0.0132935129731492,-0.0205972714630062,-0.0506416644795282,-0.0877504363551195,-0.1297549156821220,-0.0930172242876989,-0.0576191763867261,-0.0229433518557876, 0.0363188766621527,
134:              0.0001870031678450,-0.0349743794742610,-0.0631993922084072,-0.0971138050129020,-0.1624979390656570,-0.2458522401310880,-0.3438638176192820,-0.4480085728769030,-0.5679531815591490,-0.6842705623709480,
135:             -0.7615364178876680,-0.8520399314791350,-0.9010432983130380,-0.9834194615045590,-0.9905476870988080,-0.8872786828920670,-0.6744804687446460,-0.3755063881128740,-0.1868453942460660, 0.0794027920237213,
136:              0.2259213674108930, 0.4304722733878400, 0.5298904205645140, 0.6239251933219030, 0.7158012315374360, 0.8037206344012380, 0.8731666378756390, 0.9219544489912210, 0.9658401621307640, 0.9953707336137810,
137:              0.9916336070524670, 0.9786230879164460, 0.7515981300962940, 0.5233956277754490, 0.1867099275394900,-0.1183826185655860,-0.6727134585677300,-0.7289894296051810, 0.5281384062949680, 0.8678795703185170,
138:              0.8298749053132090, 0.6902403611717530, 0.5240926478571590, 0.3265450140990430, 0.1410201458363470,-0.0385849224232995,-0.1727584531294190,-0.3840270653389320,-0.5839449895165980,-0.7055596595779030,
139:             -0.8654833335157070,-0.9746729156329320,-0.9963449765889340,-0.9217474051403660,-0.7639362508931000,-0.5606026149454520,-0.3510588357296890,-0.3005270942524180,-0.1156452933894830,-0.0595959289499457,
140:             -0.0194952452596040,-0.0107439009455394,-0.0101826864310280,-0.0169325310282176,-0.1277205927832380,-0.2371674472604240,-0.2446819018845580,-0.1710822516700410,-0.0511353061733825, 0.0062563192154928,
141:             -0.0048516364950122,-0.0717158401086350,-0.1252962791394740,-0.1414075684951130,-0.1259001773043880,-0.0975843207989124,-0.0694511521424164,-0.0337586943638060,-0.0159474649423128, 0.0194325975213332,
142:              0.0608001695138442, 0.1024001693533800, 0.1480876049487220, 0.2039912059729780, 0.2920915426229240, 0.3926785587852390, 0.5354672113127650, 0.6169657298756830, 0.6993887726683350, 0.7501832875129180,
143:              0.8386871644310250, 0.9103651277237890, 0.9764351312310820, 0.9926345853646890, 0.9300339420371430, 0.8258742381128520, 0.7499387624704700, 0.6725402422076670, 0.6229867037490120, 0.6045297568333980,
144:              0.5039297182908610, 0.4455885766999630, 0.3779726796046290, 0.3429218283962470, 0.2929489553699220, 0.1570518914741900, 0.0572157944020108,-0.0463361936582273,-0.1573451851308570,-0.2931672021738570,
145:             -0.4528725984930980,-0.6063286567677410,-0.6995137850617610,-0.7902112777605310,-0.9139878106324440,-0.9998170324167670,-0.9448389776183910,-0.5177630386017770, 0.3950506584043580, 0.7346367050507850
146:         };
147: 
148:         /// <summary>
149:         /// Generated using Excel implementation from TA-Lib (http://ta-lib.org/) tests, test_HT.xsl.
150:         /// </summary>
151:         private readonly List<double> sineLead = new List<double>
152:         {
153:             double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,
154:             double.NaN, double.NaN,  double.NaN,
155:             -0.5865008756606930, 0.6010760134973850, 0.0522375953518313, 0.9091393120803100, 0.8866939724449600, 0.9971590529781770, 0.9020538171846020, 0.6368324474361340, 0.8706218323553310, 0.6073789821251400,
156:              0.7016671195580980, 0.7163213444559970, 0.7860384941578910, 0.8229422149880850, 0.8625259332042720, 0.9311800857903170, 0.9824560606058480, 0.9999415670942400, 0.9850130504072370, 0.9545149566415440,
157:              0.9699003741647460, 0.9411885260170180, 0.8957923799982550, 0.8361338691916270, 0.6425371253442400, 0.5619014189746250, 0.2222351486408030,-0.3571214380720770,-0.9310766529205810,-0.9768109870729720,
158:             -0.9433073135761630,-0.8132374819975520,-0.8689541000414960,-0.8770525415166500, 0.1475931480924170, 0.4397692495572380, 0.2482327294712000,-0.0000755239717352,-0.2313707009630830,-0.5260305525740280,
159:             -0.8452633448517760,-0.9992677882479890,-0.8993236485622310,-0.6702662015228300,-0.1539244027456790, 0.2492915015754510, 0.6115575080093740, 0.8728740226620090, 0.9909162839717420, 0.9771019730335230,
160:              0.8396734557306670, 0.5697614425887530, 0.2169861504951610,-0.1451956537134670,-0.4535393411417920,-0.6951822190514210,-0.8597041192450010,-0.9517540619903210,-0.9947396502168260,-0.9975710951353530,
161:             -0.9310629460274820,-0.8208173676831810,-0.5816830016568750,-0.2522412838591280, 0.0357301876330212, 0.8262025299696030, 0.9210718625890570, 0.6334265388777150, 0.3227045140978610, 0.0576226338781002,
162:             -0.1775030791814070,-0.3902458029200550,-0.5864888795958400,-0.8204080195335590,-0.9205013506909260,-0.9737752029989330,-0.9954119273819820,-0.9996821539441930,-0.9919003713144100,-0.9593708639942700,
163:             -0.7902545151018210,-0.7493402295361840,-0.6262584572592140,-0.6703391127989300,-0.6738861991115050,-0.7485223840691900,-0.8229875936131110,-0.9074292725287760,-0.9671080150009190,-0.9960855660349680,
164:             -0.9958932520932240,-0.9574138340514830,-0.8652041011461150,-0.7083111287566420,-0.4646190135134790, 0.3799051621364850, 0.9157357095569460, 0.9816417978833290, 0.8545864068220020, 0.6658040149144910,
165:              0.3974026651162090, 0.0660812869143209,-0.2292556000437840,-0.4438015597350790,-0.5336772503175370,-0.5234089018965820,-0.4969142899048420,-0.5089421255844810,-0.5404930402692640,-0.5900897578798300,
166:             -0.6807716781362360,-0.7164442325283580,-0.7215212414254330,-0.7420085481569570,-0.7664280384961050,-0.7928795495979390,-0.7698142295937170,-0.7466749292126270,-0.7231440467849940,-0.6809589449044260,
167:             -0.7069745376146330,-0.7314048006899320,-0.7503819371382670,-0.7724343179777090,-0.8126119368714450,-0.8592475635206920,-0.9071355135227490,-0.9489640525891650,-0.9835954633562350,-0.9994945745179860,
168:             -0.9967791122784410,-0.9726372051055730,-0.9438266487324060,-0.8236131488885440,-0.6034299982731380,-0.3012592866284200, 0.0451220549347129, 0.3898374562298360, 0.5625345127655470, 0.7610204246544270,
169:              0.8485754490678880, 0.9426271321408510, 0.9743624133056830, 0.9937744211043730, 0.9999234595478280, 0.9890496834828740, 0.9620813190845490, 0.9257815160887320, 0.8661907447689560, 0.7717933199927350,
170:              0.6099143741297350, 0.5465660379422910, 0.0650374448708587,-0.2324218737852860,-0.5626485140051470,-0.7858435966184150,-0.9988707502989160,-0.9995056053709990,-0.2269953459568470, 0.2624108067271960,
171:              0.1922802999868170,-0.0235747958138623,-0.2316259847404300,-0.4374421611852380,-0.6003241683068820,-0.7338638764656210,-0.8186335536900920,-0.9244352736103260,-0.9869359601928300,-0.9999976116346720,
172:             -0.9662055929589490,-0.8473318021865150,-0.6441207677355740,-0.3775643212631200,-0.0838943519776584, 0.1891385830833930, 0.4138657569596380, 0.4619148062355390, 0.6205889300081480, 0.6637092720958960,
173:              0.6931871751689060, 0.6994688836282570, 0.6998698746833480, 0.6950322988997850, 0.6110036127434860, 0.5192294559798360, 0.5125969121038930, 0.5757083087460280, 0.6700235759926480, 0.7115168281729820,
174:              0.7036678339992160, 0.6545752979714670, 0.6129364849611150, 0.6000111421090120, 0.6124554033545860, 0.6347294098505560, 0.6562899876128100, 0.6828327376881520, 0.6957402985880160, 0.7207141791649000,
175:              0.7487908163625680, 0.7757975773123150, 0.8040241461526770, 0.8364818144534760, 0.8828100232401670, 0.9279746557928580, 0.9758237828336110, 0.9927466296862280, 0.9999410720652520, 0.9980198650776760,
176:              0.9781412645524530, 0.9363303228513180, 0.8430454008691210, 0.6162348270656310, 0.3977902134753690, 0.1852760304055380, 0.0625305163973752,-0.0477440698701413,-0.1126037294757250,-0.1358018386690990,
177:             -0.2544275858052280,-0.3179500551407050,-0.3873842567862560,-0.4217482646499100,-0.4689384577141290,-0.5872793774815170,-0.6654907471409910,-0.7391119157698760,-0.8095586705039820,-0.8833378646001520,
178:             -0.9506681839279980,-0.9910404297342630,-0.9999429557041830,-0.9921031907559610,-0.9331896469980470,-0.6934514712647360,-0.4364996881408350, 0.2388331163456430, 0.9289332462002390, 0.9992107524543470
179:         };
180: 
181:         /// <summary>
182:         /// Generated using Excel implementation from TA-Lib (http://ta-lib.org/) tests, test_HT.xsl.
183:         /// </summary>
184:         private readonly List<double> itl = new List<double>
185:         {
186:             double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,
187:             double.NaN, double.NaN,  double.NaN,
188:               0.0000000000000,  36.3130000000000,  63.1977500000000,  81.3542500000000,  90.5332083333333,  90.7274166666667,  90.8630416666667,  90.7904791666667,  90.5763791666666,  90.1190625000000,
189:              89.5976625000000,  88.9683964285714,  88.1875857142857,  87.4745535714286,  86.8648353174603,  86.5406809523809,  86.4262830808081,  86.4413116161616,  86.4596270104895,  86.4576728687979,
190:              86.5316151556777,  86.5976538663542,  86.6975181489262,  86.7923007288442,  86.7608821809425,  86.6849764619883,  86.4810361842105,  86.2045236842105,  85.9179875000000,  85.7217875000000,
191:              85.6731214285714,  85.7294946428571,  85.8820685064935,  86.1150389610390,  86.4291412337662,  86.8542435064935,  87.1897624458874,  87.4640642857143,  87.6829172619047,  87.8553064223057,
192:              87.9218177631579,  87.9011081656347,  87.8265154798762,  87.7802132352941,  87.8975367647059,  87.9932665441176,  88.0649329044118,  88.1058593750000,  88.0771718750000,  87.9885156250000,
193:              87.8919218750000,  87.8735468750000,  87.9217812500000,  88.0132343750000,  88.1667968750000,  88.3524062500000,  88.5845312500000,  88.8867187500000,  89.2125000000000,  89.3545928308824,
194:              89.4090386029412,  89.2706643178105,  89.1179705882353,  89.2742091503268,  89.6278143274854,  90.2226447368421,  90.9753333333333,  91.7409342105263,  92.5893245614035,  93.3576615497076,
195:              94.0969948830409,  94.8495138888889,  95.5591111111111,  96.4495179738562,  97.4016168300654,  98.4999640522876,  99.7313676470588, 101.1202794117650, 102.8572647058820, 104.2939607843140,
196:             105.8849452614380, 107.0814910990710, 108.1507799707600, 108.9417928362570, 109.6523348684210, 110.1277241228070, 110.5267148809520, 110.9164619047620, 111.3471666666670, 111.8653571428570,
197:             112.3597857142860, 112.7992380952380, 113.1747142857140, 113.5586666666670, 114.0577738095240, 114.6932047619050, 115.2846898809520, 115.7229029761900, 115.8414500000000, 115.7467000000000,
198:             115.6115500000000, 115.5303375000000, 115.5548000000000, 115.6614000000000, 115.9122375000000, 116.3321625000000, 116.8428375000000, 117.2830500000000, 117.7253922619050, 118.0727228896100,
199:             118.4984101731600, 119.2198956709960, 120.0234311688310, 120.9289954887220, 121.7905789473680, 122.5689105263160, 123.2356894736840, 123.8346386591480, 124.4085501367050, 124.8498862554110,
200:             125.4094954004330, 126.1065170454550, 126.7064925000000, 127.4240825000000, 127.8647176923080, 128.0896789173790, 128.1540621001220, 128.0788455784320, 128.1446725050170, 128.3183694581280,
201:             128.4362103448280, 128.5848821839080, 128.6186973303670, 128.7719790322580, 128.8848929180570, 128.9942553260240, 129.0945314906040, 129.1530999536860, 129.0838581603580, 128.8024011680910,
202:             128.4259584615380, 127.8496453846150, 127.2448900000000, 126.6843541666670, 126.1858333333330, 125.7495416666670, 125.2761041666670, 124.7606666666670, 124.3471666666670, 124.0095208333330,
203:             123.7731875000000, 123.6621125000000, 123.6056416666670, 123.5744416666670, 123.5148250000000, 123.4937083333330, 123.4761041666670, 123.6832443181820, 124.0308714826840, 124.5405766233770,
204:             125.2020811061750, 125.8966645363410, 126.4680156432750, 126.9272675438600, 127.4003611111110, 127.8605833333330, 128.1324342105260, 128.3722748538010, 128.6053611111110, 128.6804526315790,
205:             128.7627552631580, 128.7694289473680, 128.7595250000000, 128.7409750000000, 128.6613250000000, 128.4858250000000, 128.2154750000000, 127.8500845238100, 127.4645440476190, 127.1153875541130,
206:             126.7443323451910, 126.3035666290230, 125.8504999604740, 125.3674468450390, 124.8902561172160, 124.3766410622710, 123.8351980503370, 123.2633556835640, 122.5153494623660, 121.7438575268820,
207:             120.2481227342550, 118.4358502645500, 116.5046713064710, 114.5443467073670, 112.9054354415950, 111.4893484615380, 110.7192281481480, 110.1931396825400, 110.2034782445810, 110.1120396292030,
208:             109.5987628888250, 108.8005075604840, 107.8019843750000, 106.8111562500000, 105.6388140120970, 104.3560227486560, 102.7347166042630, 101.0735459592080,  99.5273900488400,  98.3256981583231,
209:              97.3580796011396,  96.7594046153846,  96.4602600000000,  96.1232166666667,  96.1488191666667,  96.4425624637681,  96.7984085144927,  97.1613937318841,  97.3868053381643,  97.5343687643678,
210:              97.8406773180077,  98.3334693486590,  99.0595665024631,  99.8837727832512, 100.6778885467980, 101.4226269841270, 102.0713564814810, 102.6847440476190, 103.2616851851850, 103.9931695156700,
211:             104.8224465811970, 105.7652606837610, 106.6112275641030, 107.5680471014490, 108.3253364624510, 108.8823498494260, 109.2757501411630, 109.6873409090910, 110.1830037593980, 110.4551917293230
212:         };
213: 
214:         /// <summary>
215:         /// Taken from TA-Lib (http://ta-lib.org/) implementation, test_HT.xsl.
216:         /// The first 63 values are skipped (unstable period according to TA-Lib), total 200 values available (trial version).
217:         /// </summary>
218:         private readonly List<double> tcm = new List<double>
219:         {
220:             // First 63 entries are not available (unstable period).
221:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
222:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
223:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
224:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
225:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
226:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
227:             double.NaN, double.NaN, double.NaN,
228:             1, 1, 1, 1, 1, 1, 1,-1,-1, 1, 1, 1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
229:             1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1, 1, 1, 1,-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
230:             1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
231:             1, 1, 1,-1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1, 1, 1,
232:             1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
233:         };
234:         #endregion
235: 
236:         #region NameTest
237:         /// <summary>
238:         /// A test for Name.
239:         /// </summary>
240:         [TestMethod]
241:         public void NameTest()
242:         {
243:             var target = new TrendCycleMode();
244:             Assert.AreEqual("TCM", target.Name);
245:         }
246:         #endregion
247: 
248:         #region MonikerTest
249:         /// <summary>
250:         /// A test for Moniker.
251:         /// </summary>
252:         [TestMethod]
253:         public void MonikerTest()
254:         {
255:             var target = new TrendCycleMode();
256:             Assert.AreEqual("TCM4", target.Moniker);
257:             //target = new TrendCycleMode(4);
258:             //Assert.AreEqual("TCM4", target.Moniker);
259:             target = new TrendCycleMode(/*3*/);
260:             Assert.AreEqual("TCM3", target.Moniker);
261:             target = new TrendCycleMode(/*2*/);
262:             Assert.AreEqual("TCM2", target.Moniker);
263:         }
264:         #endregion
265: 
266:         #region DescriptionTest
267:         /// <summary>
268:         /// A test for Description.
269:         /// </summary>
270:         [TestMethod]
271:         public void DescriptionTest()
272:         {
273:             var target = new TrendCycleMode();
274:             Assert.AreEqual("Trend versus Cycle Mode", target.Description);
275:         }
276:         #endregion
277: 
278:         #region SmoothingLengthTest
279:         /// <summary>
280:         /// A test for SmoothingLength.
281:         /// </summary>
282:         [TestMethod]
283:         public void SmoothingLengthTest()
284:         {
285:             var target = new TrendCycleMode();
286:             Assert.AreEqual(4, target.SmoothingLength);
287:             //target = new TrendCycleMode(4);
288:             //Assert.AreEqual(4, target.SmoothingLength);
289:             target = new TrendCycleMode(/*3*/);
290:             Assert.AreEqual(3, target.SmoothingLength);
291:             target = new TrendCycleMode(/*2*/);
292:             Assert.AreEqual(2, target.SmoothingLength);
293:         }
294:         #endregion
295: 
296:         #region IsPrimedTest
297:         /// <summary>
298:         /// A test for IsPrimed.
299:         /// </summary>
300:         [TestMethod]
301:         public void IsPrimedTest()
302:         {
303:             var target = new TrendCycleMode();
304:             Assert.IsFalse(target.IsPrimed);
305:             for (int i = 1; i <= 23; ++i)
306:             {
307:                 var scalar = new Scalar(DateTime.Now, i);
308:                 scalar = target.Update(scalar);
309:                 Assert.IsTrue(double.IsNaN(scalar.Value));
310:                 Assert.IsFalse(target.IsPrimed);
311:             }
312:             for (int i = 24; i <= 48; ++i)
313:             {
314:                 var scalar = new Scalar(DateTime.Now, i);
315:                 scalar = target.Update(scalar);
316:                 Assert.IsFalse(double.IsNaN(scalar.Value));
317:                 Assert.IsTrue(target.IsPrimed);
318:             }
319:         }
320:         #endregion
321: 
322:         #region DominantCyclePeriodTest
323:         /// <summary>
324:         /// A test for DominantCyclePeriod.
325:         /// </summary>
326:         [TestMethod]
327:         public void DominantCyclePeriodTest()
328:         {
329:             const int digits = 9;
330:             var target = new TrendCycleMode();
331:             // Tradestation implementation skips first 9 bars.
332:             for (int i = 9; i < rawInput.Count; ++i)
333:             {
334:                 target.Update(rawInput[i]);
335:                 double d = Math.Round(target.DominantCyclePeriod, digits);
336:                 double u = Math.Round(dcPeriod[i], digits);
337:                 if (!double.IsNaN(d))
338:                     Assert.AreEqual(u, d);
339:             }
340:         }
341:         #endregion
342: 
343:         #region DominantCyclePhaseTest
344:         /// <summary>
345:         /// A test for DominantCyclePhase.
346:         /// </summary>
347:         [TestMethod]
348:         public void DominantCyclePhaseTest()
349:         {
350:             const int digits = 8;
351:             var target = new TrendCycleMode();
352:             // Tradestation implementation skips first 9 bars.
353:             for (int i = 9; i < rawInput.Count; ++i)
354:             {
355:                 target.Update(rawInput[i]);
356:                 double d = Math.Round(target.DominantCyclePhase, digits);
357:                 double u = Math.Round(dcPhase[i], digits);
358:                 if (!double.IsNaN(d))
359:                     Assert.AreEqual(u, d);
360:             }
361:         }
362:         #endregion
363: 
364:         #region SineWaveTest
365:         /// <summary>
366:         /// A test for SineWave.
367:         /// </summary>
368:         [TestMethod]
369:         public void SineWaveTest()
370:         {
371:             const int digits = 9;
372:             var target = new TrendCycleMode();
373:             // Tradestation implementation skips first 9 bars.
374:             for (int i = 9; i < rawInput.Count; ++i)
375:             {
376:                 target.Update(rawInput[i]);
377:                 double d = Math.Round(target.SineWave, digits);
378:                 double u = Math.Round(sine[i], digits);
379:                 if (!double.IsNaN(d))
380:                     Assert.AreEqual(u, d);
381:             }
382:         }
383:         #endregion
384: 
385:         #region SineWaveLeadTest
386:         /// <summary>
387:         /// A test for SineWaveLead.
388:         /// </summary>
389:         [TestMethod]
390:         public void SineWaveLeadTest()
391:         {
392:             const int digits = 9;
393:             var target = new TrendCycleMode();
394:             // Tradestation implementation skips first 9 bars.
395:             for (int i = 9; i < rawInput.Count; ++i)
396:             {
397:                 target.Update(rawInput[i]);
398:                 double d = Math.Round(target.SineWaveLead, digits);
399:                 double u = Math.Round(sineLead[i], digits);
400:                 if (!double.IsNaN(d))
401:                     Assert.AreEqual(u, d);
402:             }
403:         }
404:         #endregion
405: 
406:         #region InstantaneousTrendLineTest
407:         /// <summary>
408:         /// A test for InstantaneousTrendLine.
409:         /// </summary>
410:         [TestMethod]
411:         public void InstantaneousTrendLineTest()
412:         {
413:             const int digits = 9;
414:             var target = new TrendCycleMode();
415:             // Tradestation implementation skips first 9 bars.
416:             for (int i = 9; i < rawInput.Count; ++i)
417:             {
418:                 target.Update(rawInput[i]);
419:                 double d = Math.Round(target.InstantaneousTrendLine, digits);
420:                 double u = Math.Round(itl[i], digits);
421:                 if (!double.IsNaN(d))
422:                     Assert.AreEqual(u, d);
423:             }
424:         }
425:         #endregion
426: 
427:         #region UpdateTest
428:         /// <summary>
429:         /// A test for Update.
430:         /// </summary>
431:         [TestMethod]
432:         public void UpdateTest()
433:         {
434:             const int digits = 9;
435:             var target = new TrendCycleMode();
436:             // Tradestation implementation skips first 9 bars.
437:             for (int i = 9; i < tcm.Count; ++i)
438:             {
439:                 target.Update(rawInput[i]);
440:                 double d = Math.Round(target.Value, digits);
441:                 double u = Math.Round(tcm[i], digits);
442:                 if (i != 70 && i != 71 && !double.IsNaN(u))
443:                     Assert.AreEqual(u, d);
444:             }
445:         }
446:         #endregion
447: 
448:         #region ResetTest
449:         /// <summary>
450:         /// A test for Reset.
451:         /// </summary>
452:         [TestMethod]
453:         public void ResetTest()
454:         {
455:             double d, u; const int digits = 9;
456:             var target = new TrendCycleMode();
457:             // Tradestation implementation skips first 9 bars.
458:             for (int i = 9; i < tcm.Count; i++)
459:             {
460:                 target.Update(rawInput[i]);
461:                 d = Math.Round(target.Value, digits);
462:                 u = Math.Round(tcm[i], digits);
463:                 if (i != 70 && i != 71 && !double.IsNaN(u))
464:                     Assert.AreEqual(u, d);
465:             }
466:             target.Reset();
467:             for (int i = 9; i < tcm.Count; i++)
468:             {
469:                 target.Update(rawInput[i]);
470:                 d = Math.Round(target.Value, digits);
471:                 u = Math.Round(tcm[i], digits);
472:                 if (i != 70 && i != 71 && !double.IsNaN(u))
473:                     Assert.AreEqual(u, d);
474:             }
475:         }
476:         #endregion
477: 
478:         #region ConstructorTest
479:         /// <summary>
480:         /// A test for Constructor.
481:         /// </summary>
482:         [TestMethod]
483:         public void TrendCycleModeConstructorTest()
484:         {
485:             var target = new TrendCycleMode();
486:             Assert.AreEqual(4, target.SmoothingLength);
487:             Assert.IsFalse(target.IsPrimed);
488:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.MedianPrice);
489: 
490:             target = new TrendCycleMode(ohlcvComponent:OhlcvComponent.TypicalPrice);
491:             Assert.AreEqual(4, target.SmoothingLength);
492:             Assert.IsFalse(target.IsPrimed);
493:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.TypicalPrice);
494: 
495:             target = new TrendCycleMode(/*3*/);
496:             Assert.AreEqual(3, target.SmoothingLength);
497:             Assert.IsFalse(target.IsPrimed);
498:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.MedianPrice);
499: 
500:             target = new TrendCycleMode(/*3, 1, OhlcvComponent.OpeningPrice*/);
501:             Assert.AreEqual(3, target.SmoothingLength);
502:             Assert.IsFalse(target.IsPrimed);
503:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.OpeningPrice);
504: 
505:             target = new TrendCycleMode(/*2*/);
506:             Assert.AreEqual(2, target.SmoothingLength);
507:             Assert.IsFalse(target.IsPrimed);
508:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.MedianPrice);
509: 
510:             target = new TrendCycleMode(/*2, 1, OhlcvComponent.OpeningPrice*/);
511:             Assert.AreEqual(2, target.SmoothingLength);
512:             Assert.IsFalse(target.IsPrimed);
513:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.OpeningPrice);
514:         }
515: 
516:         /// <summary>
517:         /// A test for constructor exception.
518:         /// </summary>
519:         [TestMethod]
520:         [ExpectedException(typeof(ArgumentOutOfRangeException))]
521:         public void TrendCycleModeConstructorTest2()
522:         {
523:             var target = new TrendCycleMode(/*5*/);
524:             Assert.IsFalse(target.IsPrimed);
525:         }
526: 
527:         /// <summary>
528:         /// A test for constructor exception.
529:         /// </summary>
530:         [TestMethod]
531:         [ExpectedException(typeof(ArgumentOutOfRangeException))]
532:         public void TrendCycleModeConstructorTest3()
533:         {
534:             var target = new TrendCycleMode(/*1*/);
535:             Assert.IsFalse(target.IsPrimed);
536:         }
537: 
538:         /// <summary>
539:         /// A test for constructor exception.
540:         /// </summary>
541:         [TestMethod]
542:         [ExpectedException(typeof(ArgumentOutOfRangeException))]
543:         public void TrendCycleModeConstructorTest4()
544:         {
545:             var target = new TrendCycleMode(/*0*/);
546:             Assert.IsFalse(target.IsPrimed);
547:         }
548: 
549:         /// <summary>
550:         /// A test for constructor exception.
551:         /// </summary>
552:         [TestMethod]
553:         [ExpectedException(typeof(ArgumentOutOfRangeException))]
554:         public void TrendCycleModeConstructorTest5()
555:         {
556:             var target = new TrendCycleMode(/*-8*/);
557:             Assert.IsFalse(target.IsPrimed);
558:         }
559:         #endregion
560: 
561:         #region SerializationTest
562:         private static void SerializeTo(TrendCycleMode instance, string fileName)
563:         {
564:             var dcs = new DataContractSerializer(typeof(TrendCycleMode), null, 65536, false, true, null);
565:             using (var fs = new FileStream(fileName, FileMode.Create))
566:             {
567:                 dcs.WriteObject(fs, instance);
568:                 fs.Close();
569:             }
570:         }
571: 
572:         private static TrendCycleMode DeserializeFrom(string fileName)
573:         {
574:             var fs = new FileStream(fileName, FileMode.Open);
575:             XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
576:             var ser = new DataContractSerializer(typeof(TrendCycleMode), null, 65536, false, true, null);
577:             var instance = (TrendCycleMode)ser.ReadObject(reader, true);
578:             reader.Close();
579:             fs.Close();
580:             return instance;
581:         }
582: 
583:         /// <summary>
584:         /// A test for the serialization.
585:         /// </summary>
586:         [TestMethod]
587:         public void SerializationTest()
588:         {
589:             double d, u; const int digits = 9;
590:             var source = new TrendCycleMode();
591:             // Tradestation-implementation skips first 9 bars.
592:             for (int i = 9; i < 111; ++i)
593:             {
594:                 source.Update(rawInput[i]);
595:                 d = Math.Round(source.Value, digits);
596:                 u = Math.Round(tcm[i], digits);
597:                 if (i != 70 && i != 71 && !double.IsNaN(u))
598:                     Assert.AreEqual(u, d);
599:             }
600:             const string fileName = "TrendCycleMode_1.xml";
601:             SerializeTo(source, fileName);
602:             TrendCycleMode target = DeserializeFrom(fileName);
603:             Assert.AreEqual(source.Value, target.Value);
604:             Assert.AreEqual(source.DominantCyclePeriod, target.DominantCyclePeriod);
605:             // Tradestation-implementation skips first 9 bars.
606:             for (int i = 111; i < tcm.Count; ++i)
607:             {
608:                 target.Update(rawInput[i]);
609:                 d = Math.Round(target.Value, digits);
610:                 u = Math.Round(tcm[i], digits);
611:                 if (!double.IsNaN(u))
612:                     Assert.AreEqual(u, d);
613:             }
614:             //FileInfo fi = new FileInfo(fileName);
615:             //fi.Delete();
616:         }
617:         #endregion
618:     }
619: }

(End of file - total 619 lines)
</content>
```

---

## User

---

## Assistant (Compaction · claude-opus-4.7 · 78.5s)

## Goal

Port MBST's **TrendCycleMode (TCM)** indicator from `mbst-to-convert/john-ehlers/trend-cycle-mode/` into zpano:
- Go: `go/indicators/johnehlers/trendcyclemode/`
- TS: `ts/indicators/john-ehlers/trend-cycle-mode/`

Wraps existing `DominantCycle` (for period + phase) and adds its own raw-sample buffer for trendline, sine/sineLead computation, and trend-vs-cycle mode logic.

## Instructions

### Outputs (user-specified order)
1. **Value** — TCM value (+1 trend / −1 cycle), Scalar
2. **IsTrendMode** — 0/1 Scalar
3. **IsCycleMode** — 0/1 Scalar (= 1 − IsTrendMode; user explicitly requested)
4. **InstantaneousTrendLine** — Scalar
5. **SineWave** — Scalar
6. **SineWaveLead** — Scalar
7. **DominantCyclePeriod** — Scalar
8. **DominantCyclePhase** — Scalar

(Drop `SmoothedPrice` output — only used internally for condition 3.)

### Combined single test file
Merge MBST `TrendCycleModeTest.cs` reference arrays (dcPeriod, dcPhase, sine, sineLead, itl, tcm) into one Go + one TS test. skip=9, settleSkip=177 for numeric refs, tol 1e-4. For tcm value array: skip indices 70 and 71 (MBST known mismatches).

### MBST params / defaults
- `alphaEmaPeriodAdditional=0.33`, `trendLineSmoothingLength=4` ∈{2,3,4}, `cyclePartMultiplier=1.0` ∈(0,10], `separationPercentage=1.5` ∈(0,100].
- Default estimator: HomodyneDiscriminator, smoothingLength=4, αq=αp=0.2, **warmUpPeriod=100**.
- Default BarComponent: **BarMedianPrice** (always shown in mnemonic, SineWave-style).
- MAMA-style constructors: Go `NewTrendCycleModeDefault` / `NewTrendCycleModeParams` → private `newTrendCycleMode`; TS `default()` / `fromParams()`.
- Implement `core.Indicator` directly (multi-output); use `sync.RWMutex` in Go.
- Mnemonic: `tcm(α, tlsl, cpMul, sep%, <estimator>, <component>)` — follow MBST's `,tlsl={0},cpMul={1:0.####},sep={2:0.####}%` suffix; `%.3f` α.

### Key design decision
Expose internals on wrapped `DominantCycle` rather than duplicate phase logic from HTCE:
- Go: `SmoothedPrice() float64` (NaN if not primed) and `MaxPeriod() int`
- TS: `smoothedPrice` and `maxPeriod` getters

Document this pattern ("expose wrapper internals rather than duplicating") in `mbst-indicator-conversion` skill.

### Registration
- `go/indicators/core/type.go` + `type_test.go`: add `TrendCycleMode` enum constant + string + switch cases.
- `ts/indicators/core/indicator-type.ts`: add `TrendCycleMode` member.

### Test commands
- Go: `cd /home/dev/zpano/go && go test ./indicators/johnehlers/trendcyclemode/...`
- TS: from `/home/dev/zpano/ts`: `node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=TrendCycleMode`

## Discoveries

- MBST `dcp.RawValue` is the same `smoothedPhase` but returned unconditionally — behaviorally identical when `primed=true` to zpano `DominantCycle.Phase`. **No duplicate phase logic needed**.
- MBST TCM pushes **raw samples** (not htce.Smoothed) to its input buffer for `CalculateTrendAverage`. TCM needs its own raw-sample buffer alongside the wrapped DominantCycle.
- MBST TCM `Update` flow:
  1. `dcp.Update(sample)`; push raw `sample` to local input buffer.
  2. If primed: `avg = mean(input[0..length-1])` where `length=clamp(floor(smoothedPeriod·cpMul+0.5),[1,maxPeriod])`; `trendline = c0·avg + c1·avg1 + c2·avg2 + c3·avg3`; shift avgs.
  3. `phase = dcp.RawValue`; `sinWave=sin(phase·d2r)`, `sinWaveLead=sin((phase+45)·d2r)`; `diff=sinWave−sinWaveLead`.
  4. Default `isTrendMode=true`, then three conditions:
     - **C1 (cycle)**: sineLead-diff crossing zero → `samplesInTrend=0`; if `++samplesInTrend < 0.5·period` → cycle.
     - **C2 (cycle)**: if `|period|>ε` and `(phase−prevPhase) ∈ (2/3·360/period, 1.5·360/period)` → cycle.
     - **C3 (trend override)**: if `|trendline|>ε` and `|(htce.Smoothed − trendline)/trendline| ≥ separationFactor` → trend.
  5. On initial prime: seed `trendline=avg=avg1=avg2=avg3`, `prevDcPhase=phase`, `prevSineLeadDiff=diff`, increment samplesInTrend, apply C1 only.
- Coefficient table by tlsl: 2→(2/3,1/3); 3→(3/6,2/6,1/6); 4→(4/10,3/10,2/10,1/10).
- `separationFactor = separationPercentage / 100`.
- MBST tcm array: first 63 NaN, then ±1 values. Skip indices 70, 71.
- HTITL is closest algorithmic template (trendline computation identical). SineWave is closest structural template (wraps DominantCycle). Combine both.
- TS `DominantCycle` default maxPeriod is **50** (HomodyneDiscriminator smoothingLength=4; `defaultMaxPeriod=50` in `hilbert-transformer-common.ts`).

## Accomplished

### Done
1. ✅ Added Go `SmoothedPrice()` and `MaxPeriod()` methods on `DominantCycle` + tests (TestDominantCycleSmoothedPrice, TestDominantCycleMaxPeriod). `go test ./indicators/johnehlers/dominantcycle/...` passes.
2. ✅ Added TS `smoothedPrice` and `maxPeriod` getters on `DominantCycle` + tests. `--filter=DominantCycle` runs 16 specs, 0 failures.
3. ✅ Registered `TrendCycleMode` enum in Go (`go/indicators/core/type.go`: added constant after HilbertTransformerInstantaneousTrendLine, added `trendCycleMode = "trendCycleMode"`, added String() case, UnmarshalJSON case) + extended `type_test.go` at 3 test tables. `go test ./indicators/core/...` passes.
4. ✅ Registered `TrendCycleMode` enum in TS `ts/indicators/core/indicator-type.ts` (appended after HilbertTransformerInstantaneousTrendLine).
5. ✅ Read MBST `TrendCycleMode.cs` and `TrendCycleModeTest.cs` fully (including all 6 reference arrays).

### Todo list (next up)
- ⏳ Implement Go TCM (5 files) + combined reference test:
  - `trendcyclemodeparams.go` (mirrors HTITL Params + `SeparationPercentage`)
  - `trendcyclemodeoutput.go` (8 outputs in specified order, enum + String + Un/Marshal)
  - `trendcyclemodeoutput_test.go`
  - `trendcyclemode.go` (wraps `*dominantcycle.DominantCycle`; own `input[]` buffer of MaxPeriod; coeff c0..c3; state `trendline, avg1, avg2, avg3, prevDcPhase, prevSineLeadWaveDifference, samplesInTrend, isTrendMode, sinWave, sinWaveLead`; `Update` returns 8-tuple)
  - `trendcyclemode_test.go` (single file with all 6 MBST reference arrays: rawInput, dcPeriod, dcPhase, sine, sineLead, itl, tcm)
- ⏳ Implement TS TCM (4 files, mirroring Go)
- ⏳ Update `mbst-indicator-conversion` skill doc (describe "expose wrapper internals" pattern)

## Relevant files / directories

### Edited (Go)
- `/home/dev/zpano/go/indicators/johnehlers/dominantcycle/dominantcycle.go` — added `SmoothedPrice()` + `MaxPeriod()` methods (after `IsPrimed()`)
- `/home/dev/zpano/go/indicators/johnehlers/dominantcycle/dominantcycle_test.go` — added `TestDominantCycleSmoothedPrice`, `TestDominantCycleMaxPeriod`
- `/home/dev/zpano/go/indicators/core/type.go` — added `TrendCycleMode` enum + string + switch cases
- `/home/dev/zpano/go/indicators/core/type_test.go` — added `TrendCycleMode` at the 3 test tables

### Edited (TS)
- `/home/dev/zpano/ts/indicators/john-ehlers/dominant-cycle/dominant-cycle.ts` — added `smoothedPrice` + `maxPeriod` getters
- `/home/dev/zpano/ts/indicators/john-ehlers/dominant-cycle/dominant-cycle.spec.ts` — added 2 specs
- `/home/dev/zpano/ts/indicators/core/indicator-type.ts` — appended `TrendCycleMode`

### To create (Go)
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeparams.go`
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput.go`
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput_test.go`
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode.go`
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go`

### To create (TS)
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-params.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-output.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts`

### Reference templates (already read, Go)
- `/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/` (all 5 files; closest algorithmic template)
- `/home/dev/zpano/go/indicators/johnehlers/sinewave/sinewave.go` (closest structural template; wraps DominantCycle)
- `/home/dev/zpano/go/indicators/johnehlers/dominantcycle/dominantcycle.go` (now exposes needed accessors)
- `/home/dev/zpano/go/indicators/johnehlers/hilberttransformer/cycleestimator.go`

### MBST source (already read)
- `/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleMode.cs` (389 lines)
- `/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleModeTest.cs` (619 lines; all 6 reference arrays extracted into context)

### Skills
- `.opencode/skills/mbst-indicator-conversion/SKILL.md` (loaded; needs update)
- `.opencode/skills/indicator-conversion/SKILL.md`, `.opencode/skills/indicator-architecture/SKILL.md`

### Immediate next action
Create Go TCM files. Start with `trendcyclemodeoutput.go` (8 outputs Value=1, IsTrendMode=2, IsCycleMode=3, InstantaneousTrendLine=4, SineWave=5, SineWaveLead=6, DominantCyclePeriod=7, DominantCyclePhase=8), then params, then the main file, then output_test, then the combined reference test. Then mirror in TS. Finally update the skill doc.

---

## User

---

## Assistant (Build · claude-opus-4.7 · 4.4s)

_Thinking:_

 Now I'm moving on to set up the Go TCM files, so let me review the reference templates first to understand the structure I need to follow.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline</path>
<type>directory</type>
<entries>
hilberttransformerinstantaneoustrendline_test.go
hilberttransformerinstantaneoustrendline.go
hilberttransformerinstantaneoustrendlineoutput_test.go
hilberttransformerinstantaneoustrendlineoutput.go
hilberttransformerinstantaneoustrendlineparams.go

(5 entries)
</entries>
```

---

## Assistant (Build · claude-opus-4.7 · 3.4s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendline.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendline.go</path>
<type>file</type>
<content>1: package hilberttransformerinstantaneoustrendline
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
13: 	"zpano/indicators/johnehlers/hilberttransformer"
14: )
15: 
16: // HilbertTransformerInstantaneousTrendLine is Ehlers' Instantaneous Trend Line indicator
17: // built on top of a Hilbert transformer cycle estimator.
18: //
19: // It exposes two outputs:
20: //
21: //   - Value: the instantaneous trend line value, computed as a WMA of simple averages
22: //     over windows whose length tracks the smoothed dominant cycle period.
23: //   - DominantCyclePeriod: the additionally EMA-smoothed dominant cycle period.
24: //
25: // Reference:
26: // John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 107-112.
27: type HilbertTransformerInstantaneousTrendLine struct {
28: 	mu                             sync.RWMutex
29: 	mnemonic                       string
30: 	description                    string
31: 	mnemonicDCP                    string
32: 	descriptionDCP                 string
33: 	htce                           hilberttransformer.CycleEstimator
34: 	alphaEmaPeriodAdditional       float64
35: 	oneMinAlphaEmaPeriodAdditional float64
36: 	cyclePartMultiplier            float64
37: 	trendLineSmoothingLength       int
38: 	coeff0                         float64
39: 	coeff1                         float64
40: 	coeff2                         float64
41: 	coeff3                         float64
42: 	smoothedPeriod                 float64
43: 	value                          float64
44: 	average1                       float64
45: 	average2                       float64
46: 	average3                       float64
47: 	input                          []float64
48: 	inputLength                    int
49: 	inputLengthMin1                int
50: 	primed                         bool
51: 	barFunc                        entities.BarFunc
52: 	quoteFunc                      entities.QuoteFunc
53: 	tradeFunc                      entities.TradeFunc
54: }
55: 
56: // NewHilbertTransformerInstantaneousTrendLineDefault returns an instance of the indicator
57: // created using default values of the parameters.
58: func NewHilbertTransformerInstantaneousTrendLineDefault() (*HilbertTransformerInstantaneousTrendLine, error) {
59: 	const (
60: 		smoothingLength           = 4
61: 		alphaEmaQuadratureInPhase = 0.2
62: 		alphaEmaPeriod            = 0.2
63: 		alphaEmaPeriodAdditional  = 0.33
64: 		warmUpPeriod              = 100
65: 		trendLineSmoothingLength  = 4
66: 		cyclePartMultiplier       = 1.0
67: 	)
68: 
69: 	return newHilbertTransformerInstantaneousTrendLine(
70: 		hilberttransformer.HomodyneDiscriminator,
71: 		&hilberttransformer.CycleEstimatorParams{
72: 			SmoothingLength:           smoothingLength,
73: 			AlphaEmaQuadratureInPhase: alphaEmaQuadratureInPhase,
74: 			AlphaEmaPeriod:            alphaEmaPeriod,
75: 			WarmUpPeriod:              warmUpPeriod,
76: 		},
77: 		alphaEmaPeriodAdditional,
78: 		trendLineSmoothingLength,
79: 		cyclePartMultiplier,
80: 		0, 0, 0)
81: }
82: 
83: // NewHilbertTransformerInstantaneousTrendLineParams returns an instance of the indicator
84: // created using supplied parameters.
85: func NewHilbertTransformerInstantaneousTrendLineParams(
86: 	p *Params,
87: ) (*HilbertTransformerInstantaneousTrendLine, error) {
88: 	return newHilbertTransformerInstantaneousTrendLine(
89: 		p.EstimatorType, &p.EstimatorParams,
90: 		p.AlphaEmaPeriodAdditional,
91: 		p.TrendLineSmoothingLength,
92: 		p.CyclePartMultiplier,
93: 		p.BarComponent, p.QuoteComponent, p.TradeComponent)
94: }
95: 
96: //nolint:funlen,cyclop
97: func newHilbertTransformerInstantaneousTrendLine(
98: 	estimatorType hilberttransformer.CycleEstimatorType,
99: 	estimatorParams *hilberttransformer.CycleEstimatorParams,
100: 	alphaEmaPeriodAdditional float64,
101: 	trendLineSmoothingLength int,
102: 	cyclePartMultiplier float64,
103: 	bc entities.BarComponent, qc entities.QuoteComponent, tc entities.TradeComponent,
104: ) (*HilbertTransformerInstantaneousTrendLine, error) {
105: 	const (
106: 		invalid   = "invalid hilbert transformer instantaneous trend line parameters"
107: 		fmta      = "%s: α for additional smoothing should be in range (0, 1]"
108: 		fmttlsl   = "%s: trend line smoothing length should be 2, 3, or 4"
109: 		fmtcpm    = "%s: cycle part multiplier should be in range (0, 10]"
110: 		fmtw      = "%s: %w"
111: 		fmtn      = "htitl(%.3f, %d, %.3f%s%s)"
112: 		fmtnDCP   = "dcp(%.3f%s%s)"
113: 		four      = 4
114: 		alpha     = 0.2
115: 		descrVal  = "Hilbert transformer instantaneous trend line "
116: 		descrDCP  = "Dominant cycle period "
117: 		tlslTwo   = 2
118: 		tlslThree = 3
119: 		tlslFour  = 4
120: 		cpmMax    = 10.0
121: 	)
122: 
123: 	if alphaEmaPeriodAdditional <= 0. || alphaEmaPeriodAdditional > 1. {
124: 		return nil, fmt.Errorf(fmta, invalid)
125: 	}
126: 
127: 	if trendLineSmoothingLength < tlslTwo || trendLineSmoothingLength > tlslFour {
128: 		return nil, fmt.Errorf(fmttlsl, invalid)
129: 	}
130: 
131: 	if cyclePartMultiplier <= 0. || cyclePartMultiplier > cpmMax {
132: 		return nil, fmt.Errorf(fmtcpm, invalid)
133: 	}
134: 
135: 	// SineWave-style default: BarMedianPrice always shown in mnemonic.
136: 	if bc == 0 {
137: 		bc = entities.BarMedianPrice
138: 	}
139: 
140: 	if qc == 0 {
141: 		qc = entities.DefaultQuoteComponent
142: 	}
143: 
144: 	if tc == 0 {
145: 		tc = entities.DefaultTradeComponent
146: 	}
147: 
148: 	estimator, err := hilberttransformer.NewCycleEstimator(estimatorType, estimatorParams)
149: 	if err != nil {
150: 		return nil, fmt.Errorf(fmtw, invalid, err)
151: 	}
152: 
153: 	estimatorMoniker := ""
154: 	if estimatorType != hilberttransformer.HomodyneDiscriminator ||
155: 		estimatorParams.SmoothingLength != four ||
156: 		estimatorParams.AlphaEmaQuadratureInPhase != alpha ||
157: 		estimatorParams.AlphaEmaPeriod != alpha {
158: 		estimatorMoniker = hilberttransformer.EstimatorMoniker(estimatorType, estimator)
159: 		if len(estimatorMoniker) > 0 {
160: 			estimatorMoniker = ", " + estimatorMoniker
161: 		}
162: 	}
163: 
164: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
165: 
166: 	mnemonic := fmt.Sprintf(fmtn, alphaEmaPeriodAdditional, trendLineSmoothingLength,
167: 		cyclePartMultiplier, estimatorMoniker, componentMnemonic)
168: 	mnemonicDCP := fmt.Sprintf(fmtnDCP, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
169: 
170: 	barFunc, err := entities.BarComponentFunc(bc)
171: 	if err != nil {
172: 		return nil, fmt.Errorf(fmtw, invalid, err)
173: 	}
174: 
175: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
176: 	if err != nil {
177: 		return nil, fmt.Errorf(fmtw, invalid, err)
178: 	}
179: 
180: 	tradeFunc, err := entities.TradeComponentFunc(tc)
181: 	if err != nil {
182: 		return nil, fmt.Errorf(fmtw, invalid, err)
183: 	}
184: 
185: 	var c0, c1, c2, c3 float64
186: 
187: 	switch trendLineSmoothingLength {
188: 	case tlslTwo:
189: 		c0, c1 = 2.0/3.0, 1.0/3.0
190: 	case tlslThree:
191: 		c0, c1, c2 = 3.0/6.0, 2.0/6.0, 1.0/6.0
192: 	default: // tlslFour
193: 		c0, c1, c2, c3 = 4.0/10.0, 3.0/10.0, 2.0/10.0, 1.0/10.0
194: 	}
195: 
196: 	maxPeriod := estimator.MaxPeriod()
197: 
198: 	return &HilbertTransformerInstantaneousTrendLine{
199: 		mnemonic:                       mnemonic,
200: 		description:                    descrVal + mnemonic,
201: 		mnemonicDCP:                    mnemonicDCP,
202: 		descriptionDCP:                 descrDCP + mnemonicDCP,
203: 		htce:                           estimator,
204: 		alphaEmaPeriodAdditional:       alphaEmaPeriodAdditional,
205: 		oneMinAlphaEmaPeriodAdditional: 1. - alphaEmaPeriodAdditional,
206: 		cyclePartMultiplier:            cyclePartMultiplier,
207: 		trendLineSmoothingLength:       trendLineSmoothingLength,
208: 		coeff0:                         c0,
209: 		coeff1:                         c1,
210: 		coeff2:                         c2,
211: 		coeff3:                         c3,
212: 		input:                          make([]float64, maxPeriod),
213: 		inputLength:                    maxPeriod,
214: 		inputLengthMin1:                maxPeriod - 1,
215: 		barFunc:                        barFunc,
216: 		quoteFunc:                      quoteFunc,
217: 		tradeFunc:                      tradeFunc,
218: 	}, nil
219: }
220: 
221: // IsPrimed indicates whether an indicator is primed.
222: func (s *HilbertTransformerInstantaneousTrendLine) IsPrimed() bool {
223: 	s.mu.RLock()
224: 	defer s.mu.RUnlock()
225: 
226: 	return s.primed
227: }
228: 
229: // Metadata describes an output data of the indicator.
230: func (s *HilbertTransformerInstantaneousTrendLine) Metadata() core.Metadata {
231: 	return core.Metadata{
232: 		Type:        core.HilbertTransformerInstantaneousTrendLine,
233: 		Mnemonic:    s.mnemonic,
234: 		Description: s.description,
235: 		Outputs: []outputs.Metadata{
236: 			{
237: 				Kind:        int(Value),
238: 				Type:        outputs.ScalarType,
239: 				Mnemonic:    s.mnemonic,
240: 				Description: s.description,
241: 			},
242: 			{
243: 				Kind:        int(DominantCyclePeriod),
244: 				Type:        outputs.ScalarType,
245: 				Mnemonic:    s.mnemonicDCP,
246: 				Description: s.descriptionDCP,
247: 			},
248: 		},
249: 	}
250: }
251: 
252: // Update updates the value of the indicator given the next sample, returning the
253: // (value, period) pair. Returns NaN values if the indicator is not yet primed.
254: func (s *HilbertTransformerInstantaneousTrendLine) Update(sample float64) (float64, float64) {
255: 	if math.IsNaN(sample) {
256: 		return sample, sample
257: 	}
258: 
259: 	s.mu.Lock()
260: 	defer s.mu.Unlock()
261: 
262: 	s.htce.Update(sample)
263: 	s.pushInput(sample)
264: 
265: 	if s.primed {
266: 		s.smoothedPeriod = s.alphaEmaPeriodAdditional*s.htce.Period() +
267: 			s.oneMinAlphaEmaPeriodAdditional*s.smoothedPeriod
268: 		average := s.calculateAverage()
269: 		s.value = s.coeff0*average + s.coeff1*s.average1 + s.coeff2*s.average2 + s.coeff3*s.average3
270: 		s.average3 = s.average2
271: 		s.average2 = s.average1
272: 		s.average1 = average
273: 
274: 		return s.value, s.smoothedPeriod
275: 	}
276: 
277: 	if s.htce.Primed() {
278: 		s.primed = true
279: 		s.smoothedPeriod = s.htce.Period()
280: 		avg := s.calculateAverage()
281: 		s.value = avg
282: 		s.average1 = avg
283: 		s.average2 = avg
284: 		s.average3 = avg
285: 
286: 		return s.value, s.smoothedPeriod
287: 	}
288: 
289: 	nan := math.NaN()
290: 
291: 	return nan, nan
292: }
293: 
294: // UpdateScalar updates the indicator given the next scalar sample.
295: func (s *HilbertTransformerInstantaneousTrendLine) UpdateScalar(sample *entities.Scalar) core.Output {
296: 	return s.updateEntity(sample.Time, sample.Value)
297: }
298: 
299: // UpdateBar updates the indicator given the next bar sample.
300: func (s *HilbertTransformerInstantaneousTrendLine) UpdateBar(sample *entities.Bar) core.Output {
301: 	return s.updateEntity(sample.Time, s.barFunc(sample))
302: }
303: 
304: // UpdateQuote updates the indicator given the next quote sample.
305: func (s *HilbertTransformerInstantaneousTrendLine) UpdateQuote(sample *entities.Quote) core.Output {
306: 	return s.updateEntity(sample.Time, s.quoteFunc(sample))
307: }
308: 
309: // UpdateTrade updates the indicator given the next trade sample.
310: func (s *HilbertTransformerInstantaneousTrendLine) UpdateTrade(sample *entities.Trade) core.Output {
311: 	return s.updateEntity(sample.Time, s.tradeFunc(sample))
312: }
313: 
314: func (s *HilbertTransformerInstantaneousTrendLine) updateEntity(t time.Time, sample float64) core.Output {
315: 	const length = 2
316: 
317: 	value, period := s.Update(sample)
318: 
319: 	output := make([]any, length)
320: 	output[0] = entities.Scalar{Time: t, Value: value}
321: 	output[1] = entities.Scalar{Time: t, Value: period}
322: 
323: 	return output
324: }
325: 
326: func (s *HilbertTransformerInstantaneousTrendLine) pushInput(value float64) {
327: 	copy(s.input[1:], s.input[:s.inputLengthMin1])
328: 	s.input[0] = value
329: }
330: 
331: func (s *HilbertTransformerInstantaneousTrendLine) calculateAverage() float64 {
332: 	const half = 0.5
333: 
334: 	// Compute the trend line as a simple average over the measured dominant cycle period.
335: 	length := int(math.Floor(s.smoothedPeriod*s.cyclePartMultiplier + half))
336: 	if length > s.inputLength {
337: 		length = s.inputLength
338: 	} else if length < 1 {
339: 		length = 1
340: 	}
341: 
342: 	var sum float64
343: 	for i := 0; i < length; i++ {
344: 		sum += s.input[i]
345: 	}
346: 
347: 	return sum / float64(length)
348: }

(End of file - total 348 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendlineparams.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendlineparams.go</path>
<type>file</type>
<content>1: package hilberttransformerinstantaneoustrendline
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
33: 	// BarComponent indicates the component of a bar to use when updating the indicator with a bar sample.
34: 	//
35: 	// If zero, the default (BarMedianPrice) is used. Since the default differs from the framework
36: 	// default bar component, it is always shown in the indicator mnemonic.
37: 	BarComponent entities.BarComponent
38: 
39: 	// QuoteComponent indicates the component of a quote to use when updating the indicator with a quote sample.
40: 	//
41: 	// If zero, the default (QuoteMidPrice) is used and the component is not shown in the indicator mnemonic.
42: 	QuoteComponent entities.QuoteComponent
43: 
44: 	// TradeComponent indicates the component of a trade to use when updating the indicator with a trade sample.
45: 	//
46: 	// If zero, the default (TradePrice) is used and the component is not shown in the indicator mnemonic.
47: 	TradeComponent entities.TradeComponent
48: }

(End of file - total 48 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.4s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendlineoutput.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendlineoutput.go</path>
<type>file</type>
<content>1: package hilberttransformerinstantaneoustrendline
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
12: 	// Value is the instantaneous trend line value.
13: 	Value Output = iota + 1
14: 	// DominantCyclePeriod is the smoothed dominant cycle period.
15: 	DominantCyclePeriod
16: 	outputLast
17: )
18: 
19: const (
20: 	valueStr               = "value"
21: 	dominantCyclePeriodStr = "dominantCyclePeriod"
22: 	unknownStr             = "unknown"
23: )
24: 
25: // String implements the Stringer interface.
26: func (o Output) String() string {
27: 	switch o {
28: 	case Value:
29: 		return valueStr
30: 	case DominantCyclePeriod:
31: 		return dominantCyclePeriodStr
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
45: 		errFmt = "cannot marshal '%s': unknown hilbert transformer instantaneous trend line output"
46: 		extra  = 2   // Two bytes for quotes.
47: 		dqc    = '"' // Double quote character.
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
66: 		errFmt = "cannot unmarshal '%s': unknown hilbert transformer instantaneous trend line output"
67: 		dqs    = "\"" // Double quote string.
68: 	)
69: 
70: 	d := bytes.Trim(data, dqs)
71: 	s := string(d)
72: 
73: 	switch s {
74: 	case valueStr:
75: 		*o = Value
76: 	case dominantCyclePeriodStr:
77: 		*o = DominantCyclePeriod
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

## Assistant (Build · claude-opus-4.7 · 3.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendlineoutput_test.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendlineoutput_test.go</path>
<type>file</type>
<content>1: //nolint:testpackage
2: package hilberttransformerinstantaneoustrendline
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
16: 		{DominantCyclePeriod, dominantCyclePeriodStr},
17: 		{outputLast, unknownStr},
18: 		{Output(0), unknownStr},
19: 		{Output(9999), unknownStr},
20: 		{Output(-9999), unknownStr},
21: 	}
22: 
23: 	for _, tt := range tests {
24: 		exp := tt.text
25: 		act := tt.o.String()
26: 
27: 		if exp != act {
28: 			t.Errorf("'%v'.String(): expected '%v', actual '%v'", tt.o, exp, act)
29: 		}
30: 	}
31: }
32: 
33: func TestOutputIsKnown(t *testing.T) {
34: 	t.Parallel()
35: 
36: 	tests := []struct {
37: 		o       Output
38: 		boolean bool
39: 	}{
40: 		{Value, true},
41: 		{DominantCyclePeriod, true},
42: 		{outputLast, false},
43: 		{Output(0), false},
44: 		{Output(9999), false},
45: 		{Output(-9999), false},
46: 	}
47: 
48: 	for _, tt := range tests {
49: 		exp := tt.boolean
50: 		act := tt.o.IsKnown()
51: 
52: 		if exp != act {
53: 			t.Errorf("'%v'.IsKnown(): expected '%v', actual '%v'", tt.o, exp, act)
54: 		}
55: 	}
56: }
57: 
58: func TestOutputMarshalJSON(t *testing.T) {
59: 	t.Parallel()
60: 
61: 	const dqs = "\""
62: 
63: 	var nilstr string
64: 	tests := []struct {
65: 		o         Output
66: 		json      string
67: 		succeeded bool
68: 	}{
69: 		{Value, dqs + valueStr + dqs, true},
70: 		{DominantCyclePeriod, dqs + dominantCyclePeriodStr + dqs, true},
71: 		{outputLast, nilstr, false},
72: 		{Output(9999), nilstr, false},
73: 		{Output(-9999), nilstr, false},
74: 		{Output(0), nilstr, false},
75: 	}
76: 
77: 	for _, tt := range tests {
78: 		exp := tt.json
79: 		bs, err := tt.o.MarshalJSON()
80: 
81: 		if err != nil && tt.succeeded {
82: 			t.Errorf("'%v'.MarshalJSON(): expected success '%v', got error %v", tt.o, exp, err)
83: 
84: 			continue
85: 		}
86: 
87: 		if err == nil && !tt.succeeded {
88: 			t.Errorf("'%v'.MarshalJSON(): expected error, got success", tt.o)
89: 
90: 			continue
91: 		}
92: 
93: 		act := string(bs)
94: 		if exp != act {
95: 			t.Errorf("'%v'.MarshalJSON(): expected '%v', actual '%v'", tt.o, exp, act)
96: 		}
97: 	}
98: }
99: 
100: func TestOutputUnmarshalJSON(t *testing.T) {
101: 	t.Parallel()
102: 
103: 	const dqs = "\""
104: 
105: 	var zero Output
106: 	tests := []struct {
107: 		o         Output
108: 		json      string
109: 		succeeded bool
110: 	}{
111: 		{Value, dqs + valueStr + dqs, true},
112: 		{DominantCyclePeriod, dqs + dominantCyclePeriodStr + dqs, true},
113: 		{zero, dqs + unknownStr + dqs, false},
114: 		{zero, dqs + "foobar" + dqs, false},
115: 	}
116: 
117: 	for _, tt := range tests {
118: 		exp := tt.o
119: 		bs := []byte(tt.json)
120: 
121: 		var o Output
122: 
123: 		err := o.UnmarshalJSON(bs)
124: 		if err != nil && tt.succeeded {
125: 			t.Errorf("UnmarshalJSON('%v'): expected success '%v', got error %v", tt.json, exp, err)
126: 
127: 			continue
128: 		}
129: 
130: 		if err == nil && !tt.succeeded {
131: 			t.Errorf("MarshalJSON('%v'): expected error, got success", tt.json)
132: 
133: 			continue
134: 		}
135: 
136: 		if exp != o {
137: 			t.Errorf("MarshalJSON('%v'): expected '%v', actual '%v'", tt.json, exp, o)
138: 		}
139: 	}
140: }

(End of file - total 140 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 7.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/sinewave/sinewave.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/sinewave/sinewave.go</path>
<type>file</type>
<content>1: package sinewave
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
20: // SineWave is the Ehlers' Sine Wave indicator.
21: //
22: // It exposes five outputs:
23: //
24: //   - Value: the sine wave value, sin(phase·Deg2Rad).
25: //   - Lead: the sine wave lead value, sin((phase+45)·Deg2Rad).
26: //   - Band: a band with Upper=Value and Lower=Lead.
27: //   - DominantCyclePeriod: the smoothed dominant cycle period.
28: //   - DominantCyclePhase: the dominant cycle phase, in degrees.
29: //
30: // Reference:
31: // John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 95-105.
32: type SineWave struct {
33: 	mu                 sync.RWMutex
34: 	mnemonic           string
35: 	description        string
36: 	mnemonicLead       string
37: 	descriptionLead    string
38: 	mnemonicBand       string
39: 	descriptionBand    string
40: 	mnemonicDCP        string
41: 	descriptionDCP     string
42: 	mnemonicDCPhase    string
43: 	descriptionDCPhase string
44: 	dc                 *dominantcycle.DominantCycle
45: 	primed             bool
46: 	value              float64
47: 	lead               float64
48: 	barFunc            entities.BarFunc
49: 	quoteFunc          entities.QuoteFunc
50: 	tradeFunc          entities.TradeFunc
51: }
52: 
53: // NewSineWaveDefault returns an instance of the indicator
54: // created using default values of the parameters.
55: func NewSineWaveDefault() (*SineWave, error) {
56: 	const (
57: 		smoothingLength           = 4
58: 		alphaEmaQuadratureInPhase = 0.2
59: 		alphaEmaPeriod            = 0.2
60: 		alphaEmaPeriodAdditional  = 0.33
61: 		warmUpPeriod              = 100
62: 	)
63: 
64: 	return newSineWave(
65: 		hilberttransformer.HomodyneDiscriminator,
66: 		&hilberttransformer.CycleEstimatorParams{
67: 			SmoothingLength:           smoothingLength,
68: 			AlphaEmaQuadratureInPhase: alphaEmaQuadratureInPhase,
69: 			AlphaEmaPeriod:            alphaEmaPeriod,
70: 			WarmUpPeriod:              warmUpPeriod,
71: 		},
72: 		alphaEmaPeriodAdditional,
73: 		0, 0, 0)
74: }
75: 
76: // NewSineWaveParams returns an instance of the indicator created using supplied parameters.
77: func NewSineWaveParams(p *Params) (*SineWave, error) {
78: 	return newSineWave(
79: 		p.EstimatorType, &p.EstimatorParams,
80: 		p.AlphaEmaPeriodAdditional,
81: 		p.BarComponent, p.QuoteComponent, p.TradeComponent)
82: }
83: 
84: //nolint:funlen
85: func newSineWave(
86: 	estimatorType hilberttransformer.CycleEstimatorType,
87: 	estimatorParams *hilberttransformer.CycleEstimatorParams,
88: 	alphaEmaPeriodAdditional float64,
89: 	bc entities.BarComponent, qc entities.QuoteComponent, tc entities.TradeComponent,
90: ) (*SineWave, error) {
91: 	const (
92: 		invalid    = "invalid sine wave parameters"
93: 		fmta       = "%s: α for additional smoothing should be in range (0, 1]"
94: 		fmtw       = "%s: %w"
95: 		fmtnVal    = "sw(%.3f%s%s)"
96: 		fmtnLead   = "sw-lead(%.3f%s%s)"
97: 		fmtnBand   = "sw-band(%.3f%s%s)"
98: 		fmtnDCP    = "dcp(%.3f%s%s)"
99: 		fmtnDCPha  = "dcph(%.3f%s%s)"
100: 		four       = 4
101: 		alpha      = 0.2
102: 		descrVal   = "Sine wave "
103: 		descrLead  = "Sine wave lead "
104: 		descrBand  = "Sine wave band "
105: 		descrDCP   = "Dominant cycle period "
106: 		descrDCPha = "Dominant cycle phase "
107: 	)
108: 
109: 	if alphaEmaPeriodAdditional <= 0. || alphaEmaPeriodAdditional > 1. {
110: 		return nil, fmt.Errorf(fmta, invalid)
111: 	}
112: 
113: 	// Resolve defaults for component functions.
114: 	// SineWave defaults to BarMedianPrice (not the framework default), so it always shows in the mnemonic.
115: 	if bc == 0 {
116: 		bc = entities.BarMedianPrice
117: 	}
118: 
119: 	if qc == 0 {
120: 		qc = entities.DefaultQuoteComponent
121: 	}
122: 
123: 	if tc == 0 {
124: 		tc = entities.DefaultTradeComponent
125: 	}
126: 
127: 	// Build the inner DominantCycle with explicit components.
128: 	dcParams := &dominantcycle.Params{
129: 		EstimatorType:            estimatorType,
130: 		EstimatorParams:          *estimatorParams,
131: 		AlphaEmaPeriodAdditional: alphaEmaPeriodAdditional,
132: 		BarComponent:             bc,
133: 		QuoteComponent:           qc,
134: 		TradeComponent:           tc,
135: 	}
136: 
137: 	dc, err := dominantcycle.NewDominantCycleParams(dcParams)
138: 	if err != nil {
139: 		return nil, fmt.Errorf(fmtw, invalid, err)
140: 	}
141: 
142: 	// Compose the estimator moniker (same logic as DominantCycle).
143: 	estimator, err := hilberttransformer.NewCycleEstimator(estimatorType, estimatorParams)
144: 	if err != nil {
145: 		return nil, fmt.Errorf(fmtw, invalid, err)
146: 	}
147: 
148: 	estimatorMoniker := ""
149: 	if estimatorType != hilberttransformer.HomodyneDiscriminator ||
150: 		estimatorParams.SmoothingLength != four ||
151: 		estimatorParams.AlphaEmaQuadratureInPhase != alpha ||
152: 		estimatorParams.AlphaEmaPeriod != alpha {
153: 		estimatorMoniker = hilberttransformer.EstimatorMoniker(estimatorType, estimator)
154: 		if len(estimatorMoniker) > 0 {
155: 			estimatorMoniker = ", " + estimatorMoniker
156: 		}
157: 	}
158: 
159: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
160: 
161: 	mnemonicVal := fmt.Sprintf(fmtnVal, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
162: 	mnemonicLead := fmt.Sprintf(fmtnLead, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
163: 	mnemonicBand := fmt.Sprintf(fmtnBand, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
164: 	mnemonicDCP := fmt.Sprintf(fmtnDCP, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
165: 	mnemonicDCPha := fmt.Sprintf(fmtnDCPha, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
166: 
167: 	barFunc, err := entities.BarComponentFunc(bc)
168: 	if err != nil {
169: 		return nil, fmt.Errorf(fmtw, invalid, err)
170: 	}
171: 
172: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
173: 	if err != nil {
174: 		return nil, fmt.Errorf(fmtw, invalid, err)
175: 	}
176: 
177: 	tradeFunc, err := entities.TradeComponentFunc(tc)
178: 	if err != nil {
179: 		return nil, fmt.Errorf(fmtw, invalid, err)
180: 	}
181: 
182: 	nan := math.NaN()
183: 
184: 	return &SineWave{
185: 		mnemonic:           mnemonicVal,
186: 		description:        descrVal + mnemonicVal,
187: 		mnemonicLead:       mnemonicLead,
188: 		descriptionLead:    descrLead + mnemonicLead,
189: 		mnemonicBand:       mnemonicBand,
190: 		descriptionBand:    descrBand + mnemonicBand,
191: 		mnemonicDCP:        mnemonicDCP,
192: 		descriptionDCP:     descrDCP + mnemonicDCP,
193: 		mnemonicDCPhase:    mnemonicDCPha,
194: 		descriptionDCPhase: descrDCPha + mnemonicDCPha,
195: 		dc:                 dc,
196: 		value:              nan,
197: 		lead:               nan,
198: 		barFunc:            barFunc,
199: 		quoteFunc:          quoteFunc,
200: 		tradeFunc:          tradeFunc,
201: 	}, nil
202: }
203: 
204: // IsPrimed indicates whether an indicator is primed.
205: func (s *SineWave) IsPrimed() bool {
206: 	s.mu.RLock()
207: 	defer s.mu.RUnlock()
208: 
209: 	return s.primed
210: }
211: 
212: // Metadata describes an output data of the indicator.
213: func (s *SineWave) Metadata() core.Metadata {
214: 	return core.Metadata{
215: 		Type:        core.SineWave,
216: 		Mnemonic:    s.mnemonic,
217: 		Description: s.description,
218: 		Outputs: []outputs.Metadata{
219: 			{
220: 				Kind:        int(Value),
221: 				Type:        outputs.ScalarType,
222: 				Mnemonic:    s.mnemonic,
223: 				Description: s.description,
224: 			},
225: 			{
226: 				Kind:        int(Lead),
227: 				Type:        outputs.ScalarType,
228: 				Mnemonic:    s.mnemonicLead,
229: 				Description: s.descriptionLead,
230: 			},
231: 			{
232: 				Kind:        int(Band),
233: 				Type:        outputs.BandType,
234: 				Mnemonic:    s.mnemonicBand,
235: 				Description: s.descriptionBand,
236: 			},
237: 			{
238: 				Kind:        int(DominantCyclePeriod),
239: 				Type:        outputs.ScalarType,
240: 				Mnemonic:    s.mnemonicDCP,
241: 				Description: s.descriptionDCP,
242: 			},
243: 			{
244: 				Kind:        int(DominantCyclePhase),
245: 				Type:        outputs.ScalarType,
246: 				Mnemonic:    s.mnemonicDCPhase,
247: 				Description: s.descriptionDCPhase,
248: 			},
249: 		},
250: 	}
251: }
252: 
253: // Update updates the value of the indicator given the next sample, returning
254: // the (value, lead, period, phase) tuple. Returns NaN values if the indicator is not yet primed.
255: func (s *SineWave) Update(sample float64) (float64, float64, float64, float64) {
256: 	if math.IsNaN(sample) {
257: 		return sample, sample, sample, sample
258: 	}
259: 
260: 	// Delegate to the inner DominantCycle. Its own mutex is independent.
261: 	_, period, phase := s.dc.Update(sample)
262: 
263: 	s.mu.Lock()
264: 	defer s.mu.Unlock()
265: 
266: 	if math.IsNaN(phase) {
267: 		nan := math.NaN()
268: 
269: 		return nan, nan, nan, nan
270: 	}
271: 
272: 	const leadOffset = 45.0
273: 
274: 	s.primed = true
275: 	s.value = math.Sin(phase * deg2Rad)
276: 	s.lead = math.Sin((phase + leadOffset) * deg2Rad)
277: 
278: 	return s.value, s.lead, period, phase
279: }
280: 
281: // UpdateScalar updates the indicator given the next scalar sample.
282: func (s *SineWave) UpdateScalar(sample *entities.Scalar) core.Output {
283: 	return s.updateEntity(sample.Time, sample.Value)
284: }
285: 
286: // UpdateBar updates the indicator given the next bar sample.
287: func (s *SineWave) UpdateBar(sample *entities.Bar) core.Output {
288: 	return s.updateEntity(sample.Time, s.barFunc(sample))
289: }
290: 
291: // UpdateQuote updates the indicator given the next quote sample.
292: func (s *SineWave) UpdateQuote(sample *entities.Quote) core.Output {
293: 	return s.updateEntity(sample.Time, s.quoteFunc(sample))
294: }
295: 
296: // UpdateTrade updates the indicator given the next trade sample.
297: func (s *SineWave) UpdateTrade(sample *entities.Trade) core.Output {
298: 	return s.updateEntity(sample.Time, s.tradeFunc(sample))
299: }
300: 
301: func (s *SineWave) updateEntity(time time.Time, sample float64) core.Output {
302: 	const length = 5
303: 
304: 	value, lead, period, phase := s.Update(sample)
305: 
306: 	output := make([]any, length)
307: 	i := 0
308: 	output[i] = entities.Scalar{Time: time, Value: value}
309: 	i++
310: 	output[i] = entities.Scalar{Time: time, Value: lead}
311: 	i++
312: 	output[i] = outputs.Band{Time: time, Upper: value, Lower: lead}
313: 	i++
314: 	output[i] = entities.Scalar{Time: time, Value: period}
315: 	i++
316: 	output[i] = entities.Scalar{Time: time, Value: phase}
317: 
318: 	return output
319: }

(End of file - total 319 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.1s)

_Thinking:_

 I'm looking at the MBST source file to understand the exact implementation details.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleMode.cs"
}
```

**Output:**
```
<path>/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleMode.cs</path>
<type>file</type>
<content>1: ﻿using System;
2: using System.Globalization;
3: using System.Runtime.Serialization;
4: using Mbst.Numerics;
5: 
6: namespace Mbst.Trading.Indicators.JohnEhlers
7: {
8:     /// <summary>
9:     /// <para>
10:     /// The Trend versus Cycle Mode indicator.</para>
11:     /// <para/>
12:     /// See John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 113-118.
13:     /// </summary>
14:     [DataContract]
15:     public sealed class TrendCycleMode : LineIndicator
16:     {
17:         #region Members and accessors
18:         /// <summary>
19:         /// The WMA smoothing length used by underlying Hilbert transformer.
20:         /// </summary>
21:         internal int SmoothingLength => dcp.SmoothingLength;
22: 
23:         /// <summary>
24:         /// The current WMA smoothed price used by underlying Hilbert transformer.
25:         /// </summary>
26:         public double SmoothedPrice => dcp.SmoothedValue;
27: 
28:         /// <summary>
29:         /// The current value of the the smoothed Dominant Cycle Period, or <c>NaN</c> if not primed.
30:         /// </summary>
31:         public double DominantCyclePeriod => dcp.DominantCyclePeriod;
32: 
33:         /// <summary>
34:         /// The current value of the the Dominant Cycle Phase, or <c>NaN</c> if not primed.
35:         /// </summary>
36:         public double DominantCyclePhase => dcp.Value;
37: 
38:         /// <summary>
39:         /// The current value of the the Sine Wave, or <c>NaN</c> if not primed.
40:         /// </summary>
41:         public double SineWave { get { lock (updateLock) { return primed ? sinWave : double.NaN; } } }
42: 
43:         /// <summary>
44:         /// The current value of the the Sine Wave Lead, or <c>NaN</c> if not primed.
45:         /// </summary>
46:         public double SineWaveLead { get { lock (updateLock) { return primed ? sinWaveLead : double.NaN; } } }
47: 
48:         /// <summary>
49:         /// The current value of the the Instantaneous Trend Line, or <c>NaN</c> if not primed.
50:         /// </summary>
51:         public double InstantaneousTrendLine { get { lock (updateLock) { return primed ? trendline : double.NaN; } } }
52: 
53:         /// <summary>
54:         /// The additional WMA smoothing length used to smooth the trend line.
55:         /// </summary>
56:         public int TrendLineSmoothingLength => trendLineSmoothingLength;
57: 
58:         /// <summary>
59:         /// If the WMA smoothed price is separated by more than this percentage from the instantaneous trend line, then the market is in the trend mode.
60:         /// </summary>
61:         public double SeparationPercentage => separationPercentage;
62: 
63:         /// <summary>
64:         /// The multiplier to the dominant cycle period used to determine the window length to calculate the trend line.
65:         /// </summary>
66:         public double CyclePartMultiplier => cyclePartMultiplier;
67: 
68:         /// <summary>
69:         /// The default value of an additional WMA smoothing length used to smooth the trend line.
70:         /// </summary>
71:         public const int DefaultTrendLineSmoothingLength = 4;
72: 
73:         /// <summary>
74:         /// The default value of the multiplier to the dominant cycle period used to determine the window length to calculate the trend line.
75:         /// </summary>
76:         public const double DefaultCyclePartMultiplier = 1;
77: 
78:         /// <summary>
79:         /// The default value of the separation percentage.
80:         /// If the WMA smoothed price is separated by more than this percentage from the instantaneous trend line, then the market is in the trend mode.
81:         /// </summary>
82:         public const double DefaultSeparationPercentage = 1.5;
83: 
84:         /// <summary>
85:         /// The current value of the the Trend versus Cycle Mode, or <c>NaN</c> if not primed.
86:         /// The value equals to <c>-1</c> if in cycle mode, or equals to <c>1</c> in trend mode.
87:         /// </summary>
88:         public double Value { get { lock (updateLock) { return isTrendMode ? 1d : -1d; } } }
89: 
90:         /// <summary>
91:         /// If the trend mode is declared.
92:         /// </summary>
93:         public bool IsTrendMode { get { lock (updateLock) { return isTrendMode; } } }
94: 
95:         /// <summary>
96:         /// If the cycle mode is declared.
97:         /// </summary>
98:         public bool IsCycleMode { get { lock (updateLock) { return !isTrendMode; } } }
99: 
100:         [DataMember]
101:         private double sinWave;
102:         [DataMember]
103:         private double sinWaveLead;
104:         [DataMember]
105:         private double trendline;
106:         [DataMember]
107:         private double trendAverage1;
108:         [DataMember]
109:         private double trendAverage2;
110:         [DataMember]
111:         private double trendAverage3;
112:         [DataMember]
113:         private readonly double coeff0;
114:         [DataMember]
115:         private readonly double coeff1;
116:         [DataMember]
117:         private readonly double coeff2;
118:         [DataMember]
119:         private readonly double coeff3;
120:         [DataMember]
121:         private readonly double alphaEmaPeriodAdditional;
122:         [DataMember]
123:         private readonly double oneMinAlphaEmaPeriodAdditional;
124:         [DataMember]
125:         private readonly double cyclePartMultiplier;
126:         [DataMember]
127:         private readonly double separationPercentage;
128:         [DataMember]
129:         private readonly double separationFactor;
130:         [DataMember]
131:         private double prevDcPhase;
132:         [DataMember]
133:         private double prevSineLeadWaveDifference;
134:         [DataMember]
135:         private readonly DominantCyclePhase dcp;
136:         [DataMember]
137:         private int samplesInTrend;
138:         [DataMember]
139:         private readonly int trendLineSmoothingLength;
140:         [DataMember]
141:         private bool isTrendMode = true;
142: 
143:         #region Variables to track the last htce.MaxCycle input values in a circular buffer.
144:         private readonly int inputLength;
145:         private readonly int inputLengthMin1;
146:         [DataMember]
147:         private readonly double[] input;
148:         #endregion
149: 
150:         private const string tcm = "ehlersTcm";
151:         private const string tcmFull = "John Ehlers Trend/Cycle Mode";
152:         #endregion
153: 
154:         #region Construction
155:         /// <summary>
156:         /// Constructs a new instance of the class.
157:         /// </summary>
158:         /// <param name="estimator">The type of an estimator. The default is homodyne discriminator.</param>
159:         /// <param name="alphaEmaPeriodAdditional">The value of α (0 &lt; α ≤ 1) used in EMA for additional smoothing of the instantaneous period. The default value is <c>DefaultAlphaEmaPeriodAdditional</c>.</param>
160:         /// <param name="trendLineSmoothingLength">The additional WMA smoothing length used to smooth the trend line. The valid values are 2, 3, 4. The default value is <c>DefaultTrendLineSmoothingLength</c>..</param>
161:         /// <param name="cyclePartMultiplier">The multiplier to the dominant cycle period used to determine the window length to calculate the trend line. The typical values are in [0.5, 1.5]. The default value is DefaultCyclePartMultiplier.</param>
162:         /// <param name="separationPercentage">If the WMA smoothed price is separated by more than this percentage from the instantaneous trend line, then the market is in the trend mode. The default value is DefaultSeparationPercentage.</param>
163:         /// <param name="ohlcvComponent">The ohlcv component used to calculate the indicator.</param>
164:         public TrendCycleMode(HilbertTransformerCycleEstimator estimator = HilbertTransformerCycleEstimator.HomodyneDiscriminator,
165:             double alphaEmaPeriodAdditional = JohnEhlers.DominantCyclePhase.DefaultAlphaEmaPeriodAdditional,
166:             int trendLineSmoothingLength = DefaultTrendLineSmoothingLength, double cyclePartMultiplier = DefaultCyclePartMultiplier,
167:             double separationPercentage = DefaultSeparationPercentage, OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
168:             : this(new DominantCyclePhase(estimator, alphaEmaPeriodAdditional, ohlcvComponent),
169:                   estimator, alphaEmaPeriodAdditional, trendLineSmoothingLength, cyclePartMultiplier, separationPercentage, ohlcvComponent) {}
170: 
171:         /// <summary>
172:         /// Constructs a new instance of the class.
173:         /// </summary>
174:         /// <param name="estimator">The type of an estimator. The default is homodyne discriminator.</param>
175:         /// <param name="smoothingLength">The WMA smoothing length. The valid values are 2, 3, 4. The default value is <c>estimator.DefaultSmoothingLength</c>.</param>
176:         /// <param name="warmUpPeriod">The number of updates before the indicator is primed. If less than the real primed period, then will be reset to the real primed period. The default value is <c>estimator.DefaultWarmUpPeriod</c>.</param>
177:         /// <param name="alphaEmaQuadratureInPhase">The value of α (0 &lt; α ≤ 1) used in EMA to smooth the in-phase and quadrature components. The default value is <c>estimator.DefaultAlphaEmaQuadratureInPhase</c>.</param>
178:         /// <param name="alphaEmaPeriod">The value of α (0 &lt; α ≤ 1) used in EMA to smooth the instantaneous period. The default value is <c>estimator.DefaultAlphaEmaPeriod</c>.</param>
179:         /// <param name="alphaEmaPeriodAdditional">The value of α (0 &lt; α ≤ 1) used in EMA for additional smoothing of the instantaneous period. The default value is <c>DefaultAlphaEmaPeriodAdditional</c>.</param>
180:         /// <param name="trendLineSmoothingLength">The additional WMA smoothing length used to smooth the trend line. The valid values are 2, 3, 4. The default value is <c>DefaultTrendLineSmoothingLength</c>..</param>
181:         /// <param name="cyclePartMultiplier">The multiplier to the dominant cycle period used to determine the window length to calculate the trend line. The typical values are in [0.5, 1.5]. The default value is DefaultCyclePartMultiplier.</param>
182:         /// <param name="separationPercentage">If the WMA smoothed price is separated by more than this percentage from the instantaneous trend line, then the market is in the trend mode. The default value is DefaultSeparationPercentage.</param>
183:         /// <param name="ohlcvComponent">The ohlcv component used to calculate the indicator.</param>
184:         public TrendCycleMode(HilbertTransformerCycleEstimator estimator, int smoothingLength, int warmUpPeriod,
185:             double alphaEmaQuadratureInPhase, double alphaEmaPeriod, double alphaEmaPeriodAdditional = JohnEhlers.DominantCyclePhase.DefaultAlphaEmaPeriodAdditional,
186:             int trendLineSmoothingLength = DefaultTrendLineSmoothingLength, double cyclePartMultiplier = DefaultCyclePartMultiplier,
187:             double separationPercentage = DefaultSeparationPercentage, OhlcvComponent ohlcvComponent = OhlcvComponent.MedianPrice)
188:             : this(new DominantCyclePhase(estimator, smoothingLength, warmUpPeriod, alphaEmaQuadratureInPhase, alphaEmaPeriod, alphaEmaPeriodAdditional, ohlcvComponent),
189:                   estimator, alphaEmaPeriodAdditional, trendLineSmoothingLength, cyclePartMultiplier, separationPercentage, ohlcvComponent) {}
190: 
191:         private TrendCycleMode(DominantCyclePhase dcp, HilbertTransformerCycleEstimator estimator,
192:             double alphaEmaPeriodAdditional, int trendLineSmoothingLength,
193:             double cyclePartMultiplier, double separationPercentage, OhlcvComponent ohlcvComponent)
194:             : base(tcm + estimator.ShortName(), tcmFull + estimator.FullNameInBrackets(), ohlcvComponent)
195:         {
196:             if (2 > trendLineSmoothingLength || 4 < trendLineSmoothingLength)
197:                 throw new ArgumentOutOfRangeException(nameof(trendLineSmoothingLength));
198:             if (0d >= cyclePartMultiplier || 10d < cyclePartMultiplier)
199:                 throw new ArgumentOutOfRangeException(nameof(cyclePartMultiplier));
200:             if (0d >= separationPercentage || 100d < separationPercentage)
201:                 throw new ArgumentOutOfRangeException(nameof(separationPercentage));
202:             string additionalParameters = string.Format(CultureInfo.InvariantCulture, ",tlsl={0},cpMul={1:0.####},sep={2:0.####}%",
203:                 trendLineSmoothingLength, cyclePartMultiplier, separationPercentage);
204:             moniker = dcp.ComposeMoniker(tcm, additionalParameters);
205:             this.dcp = dcp;
206:             this.alphaEmaPeriodAdditional = alphaEmaPeriodAdditional;
207:             oneMinAlphaEmaPeriodAdditional = 1d - alphaEmaPeriodAdditional;
208:             this.trendLineSmoothingLength = trendLineSmoothingLength;
209:             this.cyclePartMultiplier = cyclePartMultiplier;
210:             this.separationPercentage = separationPercentage;
211:             separationFactor = separationPercentage / 100;
212:             inputLength = dcp.MaxPeriod;
213:             inputLengthMin1 = inputLength - 1;
214:             input = new double[inputLength];
215:             input.Initialize();
216:             if (2 == trendLineSmoothingLength)
217:             {
218:                 coeff0 = 2d / 3d;
219:                 coeff1 = 1d / 3d;
220:             }
221:             else if (3 == trendLineSmoothingLength)
222:             {
223:                 coeff0 = 3d / 6d;
224:                 coeff1 = 2d / 6d;
225:                 coeff2 = 1d / 6d;
226:             }
227:             else //if (4 == trendLineSmoothingLength)
228:             {
229:                 coeff0 = 4d / 10d;
230:                 coeff1 = 3d / 10d;
231:                 coeff2 = 2d / 10d;
232:                 coeff3 = 1d / 10d;
233:             }
234:         }
235:         #endregion
236: 
237:         #region Reset
238:         /// <summary>
239:         /// Resets the indicator.
240:         /// </summary>
241:         public override void Reset()
242:         {
243:             lock (updateLock)
244:             {
245:                 primed = false;
246:                 isTrendMode = true;
247:                 dcp.Reset();
248:                 trendline = 0d;
249:                 trendAverage1 = 0d;
250:                 trendAverage2 = 0d;
251:                 trendAverage3 = 0d;
252:                 prevDcPhase = 0d;
253:                 prevSineLeadWaveDifference = 0d;
254:                 sinWave = 0d;
255:                 sinWaveLead = 0d;
256:                 samplesInTrend = 0;
257:                 input.Initialize();
258:             }
259:         }
260:         #endregion
261: 
262:         #region Update
263:         /// <summary>
264:         /// Updates the value of the indicator.
265:         /// </summary>
266:         /// <param name="sample">A new sample.</param>
267:         /// <returns>The new value of the indicator.</returns>
268:         public override double Update(double sample)
269:         {
270:             if (double.IsNaN(sample))
271:                 return sample;
272:             lock (updateLock)
273:             {
274:                 dcp.Update(sample);
275:                 PushInput(sample);
276:                 if (primed)
277:                 {
278:                     double smoothedPeriod = dcp.DominantCyclePeriod;
279:                     double average = CalculateTrendAverage(smoothedPeriod);
280:                     trendline = Wma(average);
281:                     PushTrendAverage(average);
282: 
283:                     double phase = dcp.RawValue;
284:                     double diff = CalculateSineLeadWaveDifference(phase);
285: 
286:                     // Compute the trend mode, assuming trend by default.
287:                     isTrendMode = true;
288:                     // Measure days in trend from last the crossing of the SineWave and SineWaveLead indicator lines.
289:                     // Condition 1. A cycle mode exists for the half-period of a dominant cycle after the crossing.
290:                     if ((diff > 0 && prevSineLeadWaveDifference < 0) || (diff < 0 && prevSineLeadWaveDifference > 0))
291:                     {
292:                         isTrendMode = false;
293:                         samplesInTrend = 0;
294:                     }
295:                     prevSineLeadWaveDifference = diff;
296:                     if (++samplesInTrend < 0.5 * smoothedPeriod)
297:                         isTrendMode = false;
298: 
299:                     // Cycle mode if delta phase is ±50% of dominant cycle change of phase.
300:                     // Condition 2. A cycle mode exists if the measured phase rate of change is
301:                     // more than 2/3 the phase rate of change of the dominant cycle (360/period)
302:                     // and is less than 1.5 times the phase rate of change of the dominant cycle.
303:                     diff = phase - prevDcPhase;
304:                     prevDcPhase = phase;
305:                     if (Math.Abs(smoothedPeriod) > double.Epsilon)
306:                     {
307:                         const double minFactor = 2d / 3d, maxFactor = 1.5;
308:                         smoothedPeriod = 360d / smoothedPeriod;
309:                         if (diff > minFactor * smoothedPeriod &&
310:                             diff < maxFactor * smoothedPeriod)
311:                             isTrendMode = false;
312:                     }
313: 
314:                     // When the market makes a major reversal, it often does this with great vigor.
315:                     // When this occurs, the prices have a wide separation from the instantaneous trend line.
316:                     // Condition 3. If the WMA smoothed price is separated by more than 1.5%
317:                     // from the instantaneous trend line, then the market is in the trend mode.
318:                     if (Math.Abs(trendline) > double.Epsilon &&
319:                         Math.Abs((dcp.SmoothedValue - trendline) / trendline) >= separationFactor)
320:                         isTrendMode = true;
321: 
322:                     return isTrendMode ? 1d : -1d;
323:                 }
324:                 if (dcp.IsPrimed)
325:                 {
326:                     primed = true;
327:                     double smoothedPeriod = dcp.DominantCyclePeriod;
328:                     trendline = CalculateTrendAverage(smoothedPeriod);
329:                     trendAverage3 = trendline;
330:                     trendAverage2 = trendline;
331:                     trendAverage1 = trendline;
332: 
333:                     prevDcPhase = dcp.RawValue;
334:                     prevSineLeadWaveDifference = CalculateSineLeadWaveDifference(prevDcPhase);
335: 
336:                     isTrendMode = true;
337:                     if (++samplesInTrend < 0.5 * smoothedPeriod)
338:                         isTrendMode = false;
339: 
340:                     return isTrendMode ? 1d : -1d;
341:                 }
342:                 return 0d;
343:             }
344:         }
345:         #endregion
346: 
347:         #region Implementation
348:         private void PushInput(double val)
349:         {
350:             Array.Copy(input, 0, input, 1, inputLengthMin1);
351:             input[0] = val;
352:         }
353:         private void PushTrendAverage(double val)
354:         {
355:             trendAverage3 = trendAverage2;
356:             trendAverage2 = trendAverage1;
357:             trendAverage1 = val;
358:         }
359: 
360:         private double Wma(double val)
361:         {
362:             return coeff0 * val + coeff1 * trendAverage1 + coeff2 * trendAverage2 + coeff3 * trendAverage3;
363:         }
364: 
365:         private double CalculateTrendAverage(double smoothedPeriod)
366:         {
367:             // Compute the trend line as a simple average over the measured dominant cycle period.
368:             int length = (int)Math.Floor(smoothedPeriod * cyclePartMultiplier + 0.5);
369:             if (length > inputLength)
370:                 length = inputLength;
371:             else if (length < 1)
372:                 length = 1;
373:             double temp = 0d;
374:             for (int i = 0; i < length; ++i)
375:                 temp += input[i];
376:             return temp / length;
377:         }
378: 
379:         private double CalculateSineLeadWaveDifference(double phase)
380:         {
381:             const double pi4 = 45d * Constants.Deg2Rad;
382:             phase *= Constants.Deg2Rad;
383:             sinWave = Math.Sin(phase);
384:             sinWaveLead = Math.Sin(phase + pi4);
385:             return sinWave - sinWaveLead;
386:         }
387:         #endregion
388:     }
389: }

(End of file - total 389 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.4s)

_Thinking:_

 Now I'm verifying that the DominantCycle Go wrapper has the accessor methods I need and checking their signatures.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/dominantcycle/dominantcycle.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/dominantcycle/dominantcycle.go</path>
<type>file</type>
<content>1: package dominantcycle
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
13: 	"zpano/indicators/johnehlers/hilberttransformer"
14: )
15: 
16: // DominantCycle (Ehlers' dominant cycle) computes the instantaneous cycle period and phase
17: // derived from a Hilbert transformer cycle estimator.
18: //
19: // It exposes three outputs:
20: //
21: //   - RawPeriod: the raw instantaneous cycle period produced by the Hilbert transformer estimator.
22: //   - Period: the dominant cycle period obtained by additional EMA smoothing of the raw period.
23: //     Periodᵢ = α·RawPeriodᵢ + (1 − α)·Periodᵢ₋₁, 0 < α ≤ 1.
24: //   - Phase: the dominant cycle phase, in degrees.
25: //
26: // The smoothed data are multiplied by the real (cosine) component of the dominant cycle
27: // and independently by the imaginary (sine) component of the dominant cycle. The products
28: // are summed then over one full dominant cycle. The phase angle is computed as the arctangent
29: // of the ratio of the real part to the imaginary part.
30: //
31: // Reference:
32: // John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 52-77.
33: type DominantCycle struct {
34: 	mu                             sync.RWMutex
35: 	mnemonicRawPeriod              string
36: 	descriptionRawPeriod           string
37: 	mnemonicPeriod                 string
38: 	descriptionPeriod              string
39: 	mnemonicPhase                  string
40: 	descriptionPhase               string
41: 	alphaEmaPeriodAdditional       float64
42: 	oneMinAlphaEmaPeriodAdditional float64
43: 	smoothedPeriod                 float64
44: 	smoothedPhase                  float64
45: 	smoothedInput                  []float64
46: 	smoothedInputLengthMin1        int
47: 	htce                           hilberttransformer.CycleEstimator
48: 	primed                         bool
49: 	barFunc                        entities.BarFunc
50: 	quoteFunc                      entities.QuoteFunc
51: 	tradeFunc                      entities.TradeFunc
52: }
53: 
54: // NewDominantCycleDefault returns an instance of the indicator
55: // created using default values of the parameters.
56: func NewDominantCycleDefault() (*DominantCycle, error) {
57: 	const (
58: 		smoothingLength           = 4
59: 		alphaEmaQuadratureInPhase = 0.2
60: 		alphaEmaPeriod            = 0.2
61: 		alphaEmaPeriodAdditional  = 0.33
62: 		warmUpPeriod              = 100
63: 	)
64: 
65: 	return newDominantCycle(
66: 		hilberttransformer.HomodyneDiscriminator,
67: 		&hilberttransformer.CycleEstimatorParams{
68: 			SmoothingLength:           smoothingLength,
69: 			AlphaEmaQuadratureInPhase: alphaEmaQuadratureInPhase,
70: 			AlphaEmaPeriod:            alphaEmaPeriod,
71: 			WarmUpPeriod:              warmUpPeriod,
72: 		},
73: 		alphaEmaPeriodAdditional,
74: 		0, 0, 0)
75: }
76: 
77: // NewDominantCycleParams returns an instance of the indicator created using supplied parameters.
78: func NewDominantCycleParams(p *Params) (*DominantCycle, error) {
79: 	return newDominantCycle(
80: 		p.EstimatorType, &p.EstimatorParams,
81: 		p.AlphaEmaPeriodAdditional,
82: 		p.BarComponent, p.QuoteComponent, p.TradeComponent)
83: }
84: 
85: func newDominantCycle(
86: 	estimatorType hilberttransformer.CycleEstimatorType,
87: 	estimatorParams *hilberttransformer.CycleEstimatorParams,
88: 	alphaEmaPeriodAdditional float64,
89: 	bc entities.BarComponent, qc entities.QuoteComponent, tc entities.TradeComponent,
90: ) (*DominantCycle, error) {
91: 	const (
92: 		invalid  = "invalid dominant cycle parameters"
93: 		fmta     = "%s: α for additional smoothing should be in range (0, 1]"
94: 		fmtw     = "%s: %w"
95: 		fmtnRaw  = "dcp-raw(%.3f%s%s)"
96: 		fmtnPer  = "dcp(%.3f%s%s)"
97: 		fmtnPha  = "dcph(%.3f%s%s)"
98: 		four     = 4
99: 		alpha    = 0.2
100: 		descrRaw = "Dominant cycle raw period "
101: 		descrPer = "Dominant cycle period "
102: 		descrPha = "Dominant cycle phase "
103: 	)
104: 
105: 	if alphaEmaPeriodAdditional <= 0. || alphaEmaPeriodAdditional > 1. {
106: 		return nil, fmt.Errorf(fmta, invalid)
107: 	}
108: 
109: 	estimator, err := hilberttransformer.NewCycleEstimator(estimatorType, estimatorParams)
110: 	if err != nil {
111: 		return nil, err
112: 	}
113: 
114: 	estimatorMoniker := ""
115: 	if estimatorType != hilberttransformer.HomodyneDiscriminator ||
116: 		estimatorParams.SmoothingLength != four ||
117: 		estimatorParams.AlphaEmaQuadratureInPhase != alpha ||
118: 		estimatorParams.AlphaEmaPeriod != alpha {
119: 		estimatorMoniker = hilberttransformer.EstimatorMoniker(estimatorType, estimator)
120: 		if len(estimatorMoniker) > 0 {
121: 			estimatorMoniker = ", " + estimatorMoniker
122: 		}
123: 	}
124: 
125: 	// Resolve defaults for component functions.
126: 	// A zero value means "use default, don't show in mnemonic".
127: 	if bc == 0 {
128: 		bc = entities.DefaultBarComponent
129: 	}
130: 
131: 	if qc == 0 {
132: 		qc = entities.DefaultQuoteComponent
133: 	}
134: 
135: 	if tc == 0 {
136: 		tc = entities.DefaultTradeComponent
137: 	}
138: 
139: 	componentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)
140: 
141: 	mnemonicRawPeriod := fmt.Sprintf(fmtnRaw, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
142: 	mnemonicPeriod := fmt.Sprintf(fmtnPer, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
143: 	mnemonicPhase := fmt.Sprintf(fmtnPha, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)
144: 
145: 	barFunc, err := entities.BarComponentFunc(bc)
146: 	if err != nil {
147: 		return nil, fmt.Errorf(fmtw, invalid, err)
148: 	}
149: 
150: 	quoteFunc, err := entities.QuoteComponentFunc(qc)
151: 	if err != nil {
152: 		return nil, fmt.Errorf(fmtw, invalid, err)
153: 	}
154: 
155: 	tradeFunc, err := entities.TradeComponentFunc(tc)
156: 	if err != nil {
157: 		return nil, fmt.Errorf(fmtw, invalid, err)
158: 	}
159: 
160: 	maxPeriod := estimator.MaxPeriod()
161: 
162: 	return &DominantCycle{
163: 		mnemonicRawPeriod:              mnemonicRawPeriod,
164: 		descriptionRawPeriod:           descrRaw + mnemonicRawPeriod,
165: 		mnemonicPeriod:                 mnemonicPeriod,
166: 		descriptionPeriod:              descrPer + mnemonicPeriod,
167: 		mnemonicPhase:                  mnemonicPhase,
168: 		descriptionPhase:               descrPha + mnemonicPhase,
169: 		alphaEmaPeriodAdditional:       alphaEmaPeriodAdditional,
170: 		oneMinAlphaEmaPeriodAdditional: 1. - alphaEmaPeriodAdditional,
171: 		htce:                           estimator,
172: 		smoothedInput:                  make([]float64, maxPeriod),
173: 		smoothedInputLengthMin1:        maxPeriod - 1,
174: 		barFunc:                        barFunc,
175: 		quoteFunc:                      quoteFunc,
176: 		tradeFunc:                      tradeFunc,
177: 	}, nil
178: }
179: 
180: // IsPrimed indicates whether an indicator is primed.
181: func (s *DominantCycle) IsPrimed() bool {
182: 	s.mu.RLock()
183: 	defer s.mu.RUnlock()
184: 
185: 	return s.primed
186: }
187: 
188: // SmoothedPrice returns the current WMA-smoothed price value produced by the
189: // underlying Hilbert transformer cycle estimator. Returns NaN if the indicator
190: // is not yet primed.
191: //
192: // This accessor is intended for composite indicators (e.g. TrendCycleMode) that
193: // wrap a DominantCycle and need to consult the same smoothed input stream that
194: // drives the dominant-cycle computation.
195: func (s *DominantCycle) SmoothedPrice() float64 {
196: 	s.mu.RLock()
197: 	defer s.mu.RUnlock()
198: 
199: 	if !s.primed {
200: 		return math.NaN()
201: 	}
202: 
203: 	return s.htce.Smoothed()
204: }
205: 
206: // MaxPeriod returns the maximum cycle period supported by the underlying
207: // Hilbert transformer cycle estimator (also the size of the internal
208: // smoothed-input buffer).
209: func (s *DominantCycle) MaxPeriod() int {
210: 	return s.htce.MaxPeriod()
211: }
212: 
213: // Metadata describes an output data of the indicator.
214: func (s *DominantCycle) Metadata() core.Metadata {
215: 	return core.Metadata{
216: 		Type:        core.DominantCycle,
217: 		Mnemonic:    s.mnemonicPeriod,
218: 		Description: s.descriptionPeriod,
219: 		Outputs: []outputs.Metadata{
220: 			{
221: 				Kind:        int(RawPeriod),
222: 				Type:        outputs.ScalarType,
223: 				Mnemonic:    s.mnemonicRawPeriod,
224: 				Description: s.descriptionRawPeriod,
225: 			},
226: 			{
227: 				Kind:        int(Period),
228: 				Type:        outputs.ScalarType,
229: 				Mnemonic:    s.mnemonicPeriod,
230: 				Description: s.descriptionPeriod,
231: 			},
232: 			{
233: 				Kind:        int(Phase),
234: 				Type:        outputs.ScalarType,
235: 				Mnemonic:    s.mnemonicPhase,
236: 				Description: s.descriptionPhase,
237: 			},
238: 		},
239: 	}
240: }
241: 
242: // Update updates the value of the indicator given the next sample, returning the
243: // (rawPeriod, period, phase) triple. Returns NaN values if the indicator is not yet primed.
244: func (s *DominantCycle) Update(sample float64) (float64, float64, float64) {
245: 	if math.IsNaN(sample) {
246: 		return sample, sample, sample
247: 	}
248: 
249: 	s.mu.Lock()
250: 	defer s.mu.Unlock()
251: 
252: 	s.htce.Update(sample)
253: 	s.pushSmoothedInput(s.htce.Smoothed())
254: 
255: 	if s.primed {
256: 		s.smoothedPeriod = s.alphaEmaPeriodAdditional*s.htce.Period() +
257: 			s.oneMinAlphaEmaPeriodAdditional*s.smoothedPeriod
258: 		s.calculateSmoothedPhase()
259: 
260: 		return s.htce.Period(), s.smoothedPeriod, s.smoothedPhase
261: 	}
262: 
263: 	if s.htce.Primed() {
264: 		s.primed = true
265: 		s.smoothedPeriod = s.htce.Period()
266: 		s.calculateSmoothedPhase()
267: 
268: 		return s.htce.Period(), s.smoothedPeriod, s.smoothedPhase
269: 	}
270: 
271: 	nan := math.NaN()
272: 
273: 	return nan, nan, nan
274: }
275: 
276: // UpdateScalar updates the indicator given the next scalar sample.
277: func (s *DominantCycle) UpdateScalar(sample *entities.Scalar) core.Output {
278: 	return s.updateEntity(sample.Time, sample.Value)
279: }
280: 
281: // UpdateBar updates the indicator given the next bar sample.
282: func (s *DominantCycle) UpdateBar(sample *entities.Bar) core.Output {
283: 	return s.updateEntity(sample.Time, s.barFunc(sample))
284: }
285: 
286: // UpdateQuote updates the indicator given the next quote sample.
287: func (s *DominantCycle) UpdateQuote(sample *entities.Quote) core.Output {
288: 	return s.updateEntity(sample.Time, s.quoteFunc(sample))
289: }
290: 
291: // UpdateTrade updates the indicator given the next trade sample.
292: func (s *DominantCycle) UpdateTrade(sample *entities.Trade) core.Output {
293: 	return s.updateEntity(sample.Time, s.tradeFunc(sample))
294: }
295: 
296: func (s *DominantCycle) updateEntity(time time.Time, sample float64) core.Output {
297: 	const length = 3
298: 
299: 	rawPeriod, period, phase := s.Update(sample)
300: 
301: 	output := make([]any, length)
302: 	i := 0
303: 	output[i] = entities.Scalar{Time: time, Value: rawPeriod}
304: 	i++
305: 	output[i] = entities.Scalar{Time: time, Value: period}
306: 	i++
307: 	output[i] = entities.Scalar{Time: time, Value: phase}
308: 
309: 	return output
310: }
311: 
312: func (s *DominantCycle) pushSmoothedInput(value float64) {
313: 	copy(s.smoothedInput[1:], s.smoothedInput[:s.smoothedInputLengthMin1])
314: 	s.smoothedInput[0] = value
315: }
316: 
317: //nolint:cyclop
318: func (s *DominantCycle) calculateSmoothedPhase() {
319: 	const (
320: 		rad2deg    = 180.0 / math.Pi
321: 		twoPi      = 2.0 * math.Pi
322: 		epsilon    = 0.01
323: 		half       = 0.5
324: 		ninety     = 90.0
325: 		oneEighty  = 180.0
326: 		threeSixty = 360.0
327: 	)
328: 
329: 	// The smoothed data are multiplied by the real (cosine) component of the dominant cycle
330: 	// and independently by the imaginary (sine) component of the dominant cycle.
331: 	// The products are summed then over one full dominant cycle.
332: 	length := int(math.Floor(s.smoothedPeriod + half))
333: 	if length > s.smoothedInputLengthMin1 {
334: 		length = s.smoothedInputLengthMin1
335: 	}
336: 
337: 	var realPart, imagPart float64
338: 
339: 	for i := 0; i < length; i++ {
340: 		temp := twoPi * float64(i) / float64(length)
341: 		smoothed := s.smoothedInput[i]
342: 		realPart += smoothed * math.Sin(temp)
343: 		imagPart += smoothed * math.Cos(temp)
344: 	}
345: 
346: 	// We compute the phase angle as the arctangent of the ratio of the real part to the imaginary part.
347: 	// The phase increases from the left to right.
348: 	previous := s.smoothedPhase
349: 	// phase := math.Atan2(realPart, imagPart) * rad2deg
350: 	phase := math.Atan(realPart/imagPart) * rad2deg
351: 	if math.IsNaN(phase) || math.IsInf(phase, 0) {
352: 		phase = previous
353: 	}
354: 
355: 	if math.Abs(imagPart) <= epsilon {
356: 		if realPart > 0 {
357: 			phase += ninety
358: 		} else if realPart < 0 {
359: 			phase -= ninety
360: 		}
361: 	}
362: 
363: 	// Introduce the 90 degree reference shift.
364: 	phase += ninety
365: 	// Compensate for one bar lag of the smoothed input price (weighted moving average).
366: 	// This is done by adding the phase corresponding to a 1-bar lag of the smoothed dominant cycle period.
367: 	phase += threeSixty / s.smoothedPeriod
368: 	// Resolve phase ambiguity when the imaginary part is negative to provide a 360 degree phase presentation.
369: 	if imagPart < 0 {
370: 		phase += oneEighty
371: 	}
372: 	// Perform the cycle wraparound.
373: 	if phase > threeSixty {
374: 		phase -= threeSixty
375: 	}
376: 
377: 	s.smoothedPhase = phase
378: }

(End of file - total 378 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.7s)

_Thinking:_

 Looking at the DominantCycle's Phase output to verify it aligns with MBST's dcp.RawValue, then checking the HTITL test file to understand the expected pattern.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendline_test.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendline_test.go</path>
<type>file</type>
<content>1: //nolint:testpackage
2: package hilberttransformerinstantaneoustrendline
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
16: func testHTITLTime() time.Time {
17: 	return time.Date(2021, time.April, 1, 0, 0, 0, 0, &time.Location{})
18: }
19: 
20: // Input data taken from TA-Lib (http://ta-lib.org/) tests, test_MAMA.xsl, Price, D5…D256, 252 entries.
21: //
22: //nolint:dupl
23: func testHTITLInput() []float64 {
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
54: // Expected period data taken from TA-Lib (http://ta-lib.org/) tests, test_MAMA.xsl, Period Adjustment X5…X256.
55: // 252 entries, smoothed as AI18=0.33*X18+0.67*AI17.
56: //
57: //nolint:dupl
58: func testHTITLExpectedPeriod() []float64 {
59: 	return []float64{
60: 		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
61: 		0, 0, 0.39600000000000, 0.97812000000000, 1.62158040000000, 2.25545086800000, 2.84234568156000, 3.36868456664520, 3.86776291565229, 4.36321983508703,
62: 		4.87235783926831, 5.40838035704577, 5.98190550443027, 6.60199641969884, 7.27686930610184, 8.01438731048222, 8.82241286095647, 9.70906731606755, 10.68293087091460, 11.75320502957710,
63: 		12.92985285048740, 14.22372743856440, 15.55272286275450, 16.77503611571540, 17.85814025111630, 18.72970387649220, 19.30387978646140, 19.63314544969620, 19.86256979430250, 20.09030968609300,
64: 		20.24817834009410, 20.31132870798190, 20.52152604110820, 21.27119054536480, 22.10966835167300, 22.28715460952700, 21.91280773257140, 21.23923470724180, 20.70813161651310, 20.20449150221090,
65: 		19.52863321263000, 18.73709250583170, 17.96311275281150, 17.33367762545960, 16.91743352044750, 16.64300564862120, 16.41952162419500, 16.27464914327850, 16.26425245094380, 16.33321577028600,
66: 		16.39265551523990, 16.39976990202790, 16.38221107536320, 16.37405059271910, 16.35102942468120, 16.26839438425590, 16.12432207371240, 15.99529098667630, 15.96458956780100, 16.07977539207760,
67: 		16.38360881255670, 16.79746341307210, 17.18753188776420, 17.58524022168910, 18.05888760471740, 18.46077773999830, 18.78691120238400, 19.07789869381110, 19.11803073417110, 18.72675385299730,
68: 		18.07403190737810, 17.72999456892580, 18.00699920187680, 18.06680349806270, 17.73551482016550, 17.28467833183610, 16.97456900115070, 16.89386283663200, 17.00556420464730, 17.21986959021550,
69: 		17.48251598471900, 17.79647268844360, 18.15809655229470, 18.56044162987590, 19.03705300462320, 19.61779465906120, 20.13838368155990, 20.58144279802850, 20.93554178712450, 21.09578565733870,
70: 		21.19426268582890, 21.35270550953270, 21.46806615214910, 21.43420235778580, 21.27320458618770, 20.98617884905010, 20.62345107800030, 20.32165030848570, 20.09921951571820, 19.88214300840560,
71: 		19.67081622699810, 19.55217428481160, 19.60485773311710, 19.77836095343260, 19.77886122563300, 19.59009982815140, 19.54609435364200, 19.77945658439880, 20.20526697824140, 20.80572859375930,
72: 		21.45882440191380, 21.50916115262280, 21.07219135457730, 20.33979206665380, 19.60807769029340, 19.15831017112920, 19.03006205140340, 19.23359250887840, 19.84206353515510, 20.83692898803630,
73: 		22.25776348341490, 23.50933063567320, 24.02857349775940, 24.28548086650010, 24.74576845262060, 25.45685387492870, 26.31998583396390, 27.14553013410700, 27.80677101851790, 28.50146147525040,
74: 		29.16835938704370, 29.38723525724370, 29.55886298198770, 30.43981360336700, 30.70779370313880, 30.20667454311960, 29.23518282361370, 28.00037502954910, 26.95505291681000, 26.22399862702800,
75: 		25.67716809996900, 25.19893752937060, 24.76924271120940, 24.40654607774420, 24.12997738279210, 24.02648590415090, 24.17912316847620, 24.26552607123530, 24.07565548132200, 23.81050493977940,
76: 		23.75771490624360, 24.01627030476950, 24.42884190933990, 24.55867905189440, 24.41978729840000, 24.33536819272640, 24.11887925396970, 23.53741527509780, 22.66734716257270, 21.70419061052260,
77: 		20.78848949032480, 19.92593130809770, 19.09528115584620, 18.35405205698280, 17.81539769318840, 17.53491540732180, 17.59552736216070, 18.09376127214910, 18.69300796204700, 19.10361709066390,
78: 		19.40368660687600, 19.79324964337850, 20.14261316711870, 20.30292592814370, 20.37642955508450, 20.42856373321320, 20.36417897031590, 20.26870923265670, 20.31691792510900, 20.52593924664440,
79: 		20.95797078839970, 21.70315565998060, 22.68588957914270, 23.95566588814480, 25.30036991408680, 26.49222048853470, 28.24485763802440, 30.46863151925310, 31.19661794415910, 30.97271495031300,
80: 		30.15801520320610, 29.52193986806340, 28.48090879451130, 27.20913817575940, 25.84740758865390, 24.75875079095690, 24.57820671512040, 25.25622655282780, 26.58938264946150, 28.44936832011270,
81: 		30.75900691394640, 31.63120735338530, 31.95156902113430, 32.19329221743080, 32.18129930292270, 31.78927079951340, 30.94427836437330, 29.74153261553520, 28.44319750131350, 27.27756983469050,
82: 		26.30928991862760, 25.59706087830910, 25.19354035279110, 24.98183319418390, 24.66611779383150, 24.13629363553260, 23.59372342374540, 23.45943359521940, 24.13462330023790, 25.42868068174450,
83: 		27.22154743441240, 28.85990121754770, 29.25658159944000, 28.86760790158470, 28.27077502042400, 27.83957963686970, 27.56292753489200, 27.31665028261770, 27.11537844471070, 27.05619511102920,
84: 		26.72669604084850, 25.93839467294110, 24.88015320695530, 23.98089561843900, 23.51115215671300, 23.02173482203020, 22.29674643126940, 21.42162141795630, 20.54863761751100, 19.78167187971360,
85: 		19.14387361712880, 18.61396641752300,
86: 	}
87: }
88: 
89: // Expected instantaneous trend line values taken from MBST InstantaneousTrendLineTest.cs,
90: // generated using Excel implementation from TA-Lib (http://ta-lib.org/) tests, test_HT.xsl.
91: // 252 entries.
92: //
93: //nolint:dupl
94: func testHTITLExpectedValue() []float64 {
95: 	return []float64{
96: 		math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(),
97: 		math.NaN(), math.NaN(), math.NaN(),
98: 		0.0000000000000, 36.3130000000000, 63.1977500000000, 81.3542500000000, 90.5332083333333, 90.7274166666667, 90.8630416666667, 90.7904791666667, 90.5763791666666, 90.1190625000000,
99: 		89.5976625000000, 88.9683964285714, 88.1875857142857, 87.4745535714286, 86.8648353174603, 86.5406809523809, 86.4262830808081, 86.4413116161616, 86.4596270104895, 86.4576728687979,
100: 		86.5316151556777, 86.5976538663542, 86.6975181489262, 86.7923007288442, 86.7608821809425, 86.6849764619883, 86.4810361842105, 86.2045236842105, 85.9179875000000, 85.7217875000000,
101: 		85.6731214285714, 85.7294946428571, 85.8820685064935, 86.1150389610390, 86.4291412337662, 86.8542435064935, 87.1897624458874, 87.4640642857143, 87.6829172619047, 87.8553064223057,
102: 		87.9218177631579, 87.9011081656347, 87.8265154798762, 87.7802132352941, 87.8975367647059, 87.9932665441176, 88.0649329044118, 88.1058593750000, 88.0771718750000, 87.9885156250000,
103: 		87.8919218750000, 87.8735468750000, 87.9217812500000, 88.0132343750000, 88.1667968750000, 88.3524062500000, 88.5845312500000, 88.8867187500000, 89.2125000000000, 89.3545928308824,
104: 		89.4090386029412, 89.2706643178105, 89.1179705882353, 89.2742091503268, 89.6278143274854, 90.2226447368421, 90.9753333333333, 91.7409342105263, 92.5893245614035, 93.3576615497076,
105: 		94.0969948830409, 94.8495138888889, 95.5591111111111, 96.4495179738562, 97.4016168300654, 98.4999640522876, 99.7313676470588, 101.1202794117650, 102.8572647058820, 104.2939607843140,
106: 		105.8849452614380, 107.0814910990710, 108.1507799707600, 108.9417928362570, 109.6523348684210, 110.1277241228070, 110.5267148809520, 110.9164619047620, 111.3471666666670, 111.8653571428570,
107: 		112.3597857142860, 112.7992380952380, 113.1747142857140, 113.5586666666670, 114.0577738095240, 114.6932047619050, 115.2846898809520, 115.7229029761900, 115.8414500000000, 115.7467000000000,
108: 		115.6115500000000, 115.5303375000000, 115.5548000000000, 115.6614000000000, 115.9122375000000, 116.3321625000000, 116.8428375000000, 117.2830500000000, 117.7253922619050, 118.0727228896100,
109: 		118.4984101731600, 119.2198956709960, 120.0234311688310, 120.9289954887220, 121.7905789473680, 122.5689105263160, 123.2356894736840, 123.8346386591480, 124.4085501367050, 124.8498862554110,
110: 		125.4094954004330, 126.1065170454550, 126.7064925000000, 127.4240825000000, 127.8647176923080, 128.0896789173790, 128.1540621001220, 128.0788455784320, 128.1446725050170, 128.3183694581280,
111: 		128.4362103448280, 128.5848821839080, 128.6186973303670, 128.7719790322580, 128.8848929180570, 128.9942553260240, 129.0945314906040, 129.1530999536860, 129.0838581603580, 128.8024011680910,
112: 		128.4259584615380, 127.8496453846150, 127.2448900000000, 126.6843541666670, 126.1858333333330, 125.7495416666670, 125.2761041666670, 124.7606666666670, 124.3471666666670, 124.0095208333330,
113: 		123.7731875000000, 123.6621125000000, 123.6056416666670, 123.5744416666670, 123.5148250000000, 123.4937083333330, 123.4761041666670, 123.6832443181820, 124.0308714826840, 124.5405766233770,
114: 		125.2020811061750, 125.8966645363410, 126.4680156432750, 126.9272675438600, 127.4003611111110, 127.8605833333330, 128.1324342105260, 128.3722748538010, 128.6053611111110, 128.6804526315790,
115: 		128.7627552631580, 128.7694289473680, 128.7595250000000, 128.7409750000000, 128.6613250000000, 128.4858250000000, 128.2154750000000, 127.8500845238100, 127.4645440476190, 127.1153875541130,
116: 		126.7443323451910, 126.3035666290230, 125.8504999604740, 125.3674468450390, 124.8902561172160, 124.3766410622710, 123.8351980503370, 123.2633556835640, 122.5153494623660, 121.7438575268820,
117: 		120.2481227342550, 118.4358502645500, 116.5046713064710, 114.5443467073670, 112.9054354415950, 111.4893484615380, 110.7192281481480, 110.1931396825400, 110.2034782445810, 110.1120396292030,
118: 		109.5987628888250, 108.8005075604840, 107.8019843750000, 106.8111562500000, 105.6388140120970, 104.3560227486560, 102.7347166042630, 101.0735459592080, 99.5273900488400, 98.3256981583231,
119: 		97.3580796011396, 96.7594046153846, 96.4602600000000, 96.1232166666667, 96.1488191666667, 96.4425624637681, 96.7984085144927, 97.1613937318841, 97.3868053381643, 97.5343687643678,
120: 		97.8406773180077, 98.3334693486590, 99.0595665024631, 99.8837727832512, 100.6778885467980, 101.4226269841270, 102.0713564814810, 102.6847440476190, 103.2616851851850, 103.9931695156700,
121: 		104.8224465811970, 105.7652606837610, 106.6112275641030, 107.5680471014490, 108.3253364624510, 108.8823498494260, 109.2757501411630, 109.6873409090910, 110.1830037593980, 110.4551917293230,
122: 	}
123: }
124: 
125: // Tolerance used to compare floating-point values. Same rationale as SineWave: the trend line is
126: // built on top of an EMA-smoothed period driven by a cascade of WMA/Hilbert/α-filters, whose early
127: // residuals decay but never fully vanish.
128: const testHTITLTolerance = 1e-4
129: 
130: func TestHilbertTransformerInstantaneousTrendLineUpdate(t *testing.T) { //nolint: funlen
131: 	t.Parallel()
132: 
133: 	input := testHTITLInput()
134: 	expPeriod := testHTITLExpectedPeriod()
135: 	expValue := testHTITLExpectedValue()
136: 
137: 	const (
138: 		skip       = 9   // TradeStation implementation skips first 9 bars.
139: 		settleSkip = 177 // Samples required for the EMA to converge past structural reference mismatch.
140: 	)
141: 
142: 	t.Run("reference value (MBST InstantaneousTrendLineTest)", func(t *testing.T) {
143: 		t.Parallel()
144: 
145: 		x := testHTITLCreateDefault()
146: 
147: 		for i := skip; i < len(input); i++ {
148: 			value, _ := x.Update(input[i])
149: 			if math.IsNaN(value) || i < settleSkip {
150: 				continue
151: 			}
152: 
153: 			if math.IsNaN(expValue[i]) {
154: 				continue
155: 			}
156: 
157: 			if math.Abs(expValue[i]-value) > testHTITLTolerance {
158: 				t.Errorf("[%v] value is incorrect: expected %v, actual %v", i, expValue[i], value)
159: 			}
160: 		}
161: 	})
162: 
163: 	t.Run("reference period (test_MAMA.xsl, Period Adjustment)", func(t *testing.T) {
164: 		t.Parallel()
165: 
166: 		x := testHTITLCreateDefault()
167: 
168: 		for i := skip; i < len(input); i++ {
169: 			_, period := x.Update(input[i])
170: 			if math.IsNaN(period) || i < settleSkip {
171: 				continue
172: 			}
173: 
174: 			if math.Abs(expPeriod[i]-period) > testHTITLTolerance {
175: 				t.Errorf("[%v] period is incorrect: expected %v, actual %v", i, expPeriod[i], period)
176: 			}
177: 		}
178: 	})
179: 
180: 	t.Run("NaN input returns NaN pair", func(t *testing.T) {
181: 		t.Parallel()
182: 
183: 		x := testHTITLCreateDefault()
184: 		value, period := x.Update(math.NaN())
185: 
186: 		if !math.IsNaN(value) || !math.IsNaN(period) {
187: 			t.Errorf("expected (NaN, NaN), actual (%v, %v)", value, period)
188: 		}
189: 	})
190: }
191: 
192: func TestHilbertTransformerInstantaneousTrendLineIsPrimed(t *testing.T) {
193: 	t.Parallel()
194: 
195: 	input := testHTITLInput()
196: 
197: 	t.Run("primes somewhere in the input sequence", func(t *testing.T) {
198: 		t.Parallel()
199: 
200: 		x := testHTITLCreateDefault()
201: 
202: 		if x.IsPrimed() {
203: 			t.Error("expected not primed at start")
204: 		}
205: 
206: 		primedAt := -1
207: 
208: 		for i := range input {
209: 			x.Update(input[i])
210: 
211: 			if x.IsPrimed() && primedAt < 0 {
212: 				primedAt = i
213: 			}
214: 		}
215: 
216: 		if primedAt < 0 {
217: 			t.Error("expected indicator to become primed within the input sequence")
218: 		}
219: 
220: 		if !x.IsPrimed() {
221: 			t.Error("expected primed at end")
222: 		}
223: 	})
224: }
225: 
226: func TestHilbertTransformerInstantaneousTrendLineMetadata(t *testing.T) {
227: 	t.Parallel()
228: 
229: 	const (
230: 		descrVal = "Hilbert transformer instantaneous trend line "
231: 		descrDCP = "Dominant cycle period "
232: 	)
233: 
234: 	x := testHTITLCreateDefault()
235: 	act := x.Metadata()
236: 
237: 	check := func(what string, exp, a any) {
238: 		t.Helper()
239: 
240: 		if exp != a {
241: 			t.Errorf("%s is incorrect: expected %v, actual %v", what, exp, a)
242: 		}
243: 	}
244: 
245: 	mnemonic := "htitl(0.330, 4, 1.000, hl/2)"
246: 	mnemonicDCP := "dcp(0.330, hl/2)"
247: 
248: 	check("Type", core.HilbertTransformerInstantaneousTrendLine, act.Type)
249: 	check("Mnemonic", mnemonic, act.Mnemonic)
250: 	check("Description", descrVal+mnemonic, act.Description)
251: 	check("len(Outputs)", 2, len(act.Outputs))
252: 
253: 	check("Outputs[0].Kind", int(Value), act.Outputs[0].Kind)
254: 	check("Outputs[0].Type", outputs.ScalarType, act.Outputs[0].Type)
255: 	check("Outputs[0].Mnemonic", mnemonic, act.Outputs[0].Mnemonic)
256: 	check("Outputs[0].Description", descrVal+mnemonic, act.Outputs[0].Description)
257: 
258: 	check("Outputs[1].Kind", int(DominantCyclePeriod), act.Outputs[1].Kind)
259: 	check("Outputs[1].Type", outputs.ScalarType, act.Outputs[1].Type)
260: 	check("Outputs[1].Mnemonic", mnemonicDCP, act.Outputs[1].Mnemonic)
261: 	check("Outputs[1].Description", descrDCP+mnemonicDCP, act.Outputs[1].Description)
262: }
263: 
264: func TestHilbertTransformerInstantaneousTrendLineUpdateEntity(t *testing.T) { //nolint: funlen
265: 	t.Parallel()
266: 
267: 	const (
268: 		primeCount = 200
269: 		inp        = 100.
270: 		outputLen  = 2
271: 	)
272: 
273: 	tm := testHTITLTime()
274: 	check := func(act core.Output) {
275: 		t.Helper()
276: 
277: 		if len(act) != outputLen {
278: 			t.Errorf("len(output) is incorrect: expected %v, actual %v", outputLen, len(act))
279: 
280: 			return
281: 		}
282: 
283: 		for i := 0; i < outputLen; i++ {
284: 			s, ok := act[i].(entities.Scalar)
285: 			if !ok {
286: 				t.Errorf("output[%d] is not a scalar", i)
287: 
288: 				continue
289: 			}
290: 
291: 			if s.Time != tm {
292: 				t.Errorf("output[%d] time is incorrect: expected %v, actual %v", i, tm, s.Time)
293: 			}
294: 		}
295: 	}
296: 
297: 	input := testHTITLInput()
298: 
299: 	t.Run("update scalar", func(t *testing.T) {
300: 		t.Parallel()
301: 
302: 		s := entities.Scalar{Time: tm, Value: inp}
303: 		x := testHTITLCreateDefault()
304: 
305: 		for i := 0; i < primeCount; i++ {
306: 			x.Update(input[i%len(input)])
307: 		}
308: 
309: 		check(x.UpdateScalar(&s))
310: 	})
311: 
312: 	t.Run("update bar", func(t *testing.T) {
313: 		t.Parallel()
314: 
315: 		b := entities.Bar{Time: tm, High: inp, Low: inp, Close: inp}
316: 		x := testHTITLCreateDefault()
317: 
318: 		for i := 0; i < primeCount; i++ {
319: 			x.Update(input[i%len(input)])
320: 		}
321: 
322: 		check(x.UpdateBar(&b))
323: 	})
324: 
325: 	t.Run("update quote", func(t *testing.T) {
326: 		t.Parallel()
327: 
328: 		q := entities.Quote{Time: tm, Bid: inp, Ask: inp}
329: 		x := testHTITLCreateDefault()
330: 
331: 		for i := 0; i < primeCount; i++ {
332: 			x.Update(input[i%len(input)])
333: 		}
334: 
335: 		check(x.UpdateQuote(&q))
336: 	})
337: 
338: 	t.Run("update trade", func(t *testing.T) {
339: 		t.Parallel()
340: 
341: 		r := entities.Trade{Time: tm, Price: inp}
342: 		x := testHTITLCreateDefault()
343: 
344: 		for i := 0; i < primeCount; i++ {
345: 			x.Update(input[i%len(input)])
346: 		}
347: 
348: 		check(x.UpdateTrade(&r))
349: 	})
350: }
351: 
352: func TestNewHilbertTransformerInstantaneousTrendLine(t *testing.T) { //nolint: funlen,maintidx
353: 	t.Parallel()
354: 
355: 	const (
356: 		bc entities.BarComponent   = entities.BarClosePrice
357: 		qc entities.QuoteComponent = entities.QuoteMidPrice
358: 		tc entities.TradeComponent = entities.TradePrice
359: 
360: 		errAlpha = "invalid hilbert transformer instantaneous trend line parameters: " +
361: 			"α for additional smoothing should be in range (0, 1]"
362: 		errTLSL = "invalid hilbert transformer instantaneous trend line parameters: " +
363: 			"trend line smoothing length should be 2, 3, or 4"
364: 		errCPM = "invalid hilbert transformer instantaneous trend line parameters: " +
365: 			"cycle part multiplier should be in range (0, 10]"
366: 		errBC = "invalid hilbert transformer instantaneous trend line parameters: 9999: unknown bar component"
367: 		errQC = "invalid hilbert transformer instantaneous trend line parameters: 9999: unknown quote component"
368: 		errTC = "invalid hilbert transformer instantaneous trend line parameters: 9999: unknown trade component"
369: 	)
370: 
371: 	check := func(name string, exp, act any) {
372: 		t.Helper()
373: 
374: 		if exp != act {
375: 			t.Errorf("%s is incorrect: expected %v, actual %v", name, exp, act)
376: 		}
377: 	}
378: 
379: 	t.Run("default", func(t *testing.T) {
380: 		t.Parallel()
381: 
382: 		x, err := NewHilbertTransformerInstantaneousTrendLineDefault()
383: 		check("err == nil", true, err == nil)
384: 		check("mnemonic", "htitl(0.330, 4, 1.000, hl/2)", x.mnemonic)
385: 		check("primed", false, x.primed)
386: 		check("trendLineSmoothingLength", 4, x.trendLineSmoothingLength)
387: 	})
388: 
389: 	t.Run("tlsl=2", func(t *testing.T) {
390: 		t.Parallel()
391: 
392: 		params := Params{
393: 			AlphaEmaPeriodAdditional: 0.33,
394: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
395: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
396: 			TrendLineSmoothingLength: 2,
397: 			CyclePartMultiplier:      1.0,
398: 			BarComponent:             bc, QuoteComponent: qc, TradeComponent: tc,
399: 		}
400: 
401: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
402: 		check("err == nil", true, err == nil)
403: 		check("mnemonic", "htitl(0.330, 2, 1.000)", x.mnemonic)
404: 		check("coeff0", 2.0/3.0, x.coeff0)
405: 		check("coeff1", 1.0/3.0, x.coeff1)
406: 	})
407: 
408: 	t.Run("tlsl=3, phase accumulator", func(t *testing.T) {
409: 		t.Parallel()
410: 
411: 		params := Params{
412: 			AlphaEmaPeriodAdditional: 0.5,
413: 			EstimatorType:            hilberttransformer.PhaseAccumulator,
414: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
415: 			TrendLineSmoothingLength: 3,
416: 			CyclePartMultiplier:      0.5,
417: 			BarComponent:             bc, QuoteComponent: qc, TradeComponent: tc,
418: 		}
419: 
420: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
421: 		check("err == nil", true, err == nil)
422: 		check("mnemonic", "htitl(0.500, 3, 0.500, pa(4, 0.200, 0.200))", x.mnemonic)
423: 	})
424: 
425: 	t.Run("α ≤ 0", func(t *testing.T) {
426: 		t.Parallel()
427: 
428: 		params := Params{
429: 			AlphaEmaPeriodAdditional: 0.0,
430: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
431: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
432: 			TrendLineSmoothingLength: 4,
433: 			CyclePartMultiplier:      1.0,
434: 		}
435: 
436: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
437: 		check("x == nil", true, x == nil)
438: 		check("err", errAlpha, err.Error())
439: 	})
440: 
441: 	t.Run("α > 1", func(t *testing.T) {
442: 		t.Parallel()
443: 
444: 		params := Params{
445: 			AlphaEmaPeriodAdditional: 1.00000001,
446: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
447: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
448: 			TrendLineSmoothingLength: 4,
449: 			CyclePartMultiplier:      1.0,
450: 		}
451: 
452: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
453: 		check("x == nil", true, x == nil)
454: 		check("err", errAlpha, err.Error())
455: 	})
456: 
457: 	t.Run("tlsl < 2", func(t *testing.T) {
458: 		t.Parallel()
459: 
460: 		params := Params{
461: 			AlphaEmaPeriodAdditional: 0.33,
462: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
463: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
464: 			TrendLineSmoothingLength: 1,
465: 			CyclePartMultiplier:      1.0,
466: 		}
467: 
468: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
469: 		check("x == nil", true, x == nil)
470: 		check("err", errTLSL, err.Error())
471: 	})
472: 
473: 	t.Run("tlsl > 4", func(t *testing.T) {
474: 		t.Parallel()
475: 
476: 		params := Params{
477: 			AlphaEmaPeriodAdditional: 0.33,
478: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
479: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
480: 			TrendLineSmoothingLength: 5,
481: 			CyclePartMultiplier:      1.0,
482: 		}
483: 
484: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
485: 		check("x == nil", true, x == nil)
486: 		check("err", errTLSL, err.Error())
487: 	})
488: 
489: 	t.Run("cpMul ≤ 0", func(t *testing.T) {
490: 		t.Parallel()
491: 
492: 		params := Params{
493: 			AlphaEmaPeriodAdditional: 0.33,
494: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
495: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
496: 			TrendLineSmoothingLength: 4,
497: 			CyclePartMultiplier:      0.0,
498: 		}
499: 
500: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
501: 		check("x == nil", true, x == nil)
502: 		check("err", errCPM, err.Error())
503: 	})
504: 
505: 	t.Run("cpMul > 10", func(t *testing.T) {
506: 		t.Parallel()
507: 
508: 		params := Params{
509: 			AlphaEmaPeriodAdditional: 0.33,
510: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
511: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
512: 			TrendLineSmoothingLength: 4,
513: 			CyclePartMultiplier:      10.00001,
514: 		}
515: 
516: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
517: 		check("x == nil", true, x == nil)
518: 		check("err", errCPM, err.Error())
519: 	})
520: 
521: 	t.Run("invalid bar component", func(t *testing.T) {
522: 		t.Parallel()
523: 
524: 		params := Params{
525: 			AlphaEmaPeriodAdditional: 0.33,
526: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
527: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
528: 			TrendLineSmoothingLength: 4,
529: 			CyclePartMultiplier:      1.0,
530: 			BarComponent:             entities.BarComponent(9999), QuoteComponent: qc, TradeComponent: tc,
531: 		}
532: 
533: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
534: 		check("x == nil", true, x == nil)
535: 		check("err", errBC, err.Error())
536: 	})
537: 
538: 	t.Run("invalid quote component", func(t *testing.T) {
539: 		t.Parallel()
540: 
541: 		params := Params{
542: 			AlphaEmaPeriodAdditional: 0.33,
543: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
544: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
545: 			TrendLineSmoothingLength: 4,
546: 			CyclePartMultiplier:      1.0,
547: 			BarComponent:             bc, QuoteComponent: entities.QuoteComponent(9999), TradeComponent: tc,
548: 		}
549: 
550: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
551: 		check("x == nil", true, x == nil)
552: 		check("err", errQC, err.Error())
553: 	})
554: 
555: 	t.Run("invalid trade component", func(t *testing.T) {
556: 		t.Parallel()
557: 
558: 		params := Params{
559: 			AlphaEmaPeriodAdditional: 0.33,
560: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
561: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
562: 			TrendLineSmoothingLength: 4,
563: 			CyclePartMultiplier:      1.0,
564: 			BarComponent:             bc, QuoteComponent: qc, TradeComponent: entities.TradeComponent(9999),
565: 		}
566: 
567: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
568: 		check("x == nil", true, x == nil)
569: 		check("err", errTC, err.Error())
570: 	})
571: }
572: 
573: func testHTITLCreateCycleEstimatorParams() hilberttransformer.CycleEstimatorParams {
574: 	const (
575: 		smoothingLength           = 4
576: 		alphaEmaQuadratureInPhase = 0.2
577: 		alphaEmaPeriod            = 0.2
578: 		warmUpPeriod              = 0
579: 	)
580: 
581: 	return hilberttransformer.CycleEstimatorParams{
582: 		SmoothingLength:           smoothingLength,
583: 		AlphaEmaQuadratureInPhase: alphaEmaQuadratureInPhase,
584: 		AlphaEmaPeriod:            alphaEmaPeriod,
585: 		WarmUpPeriod:              warmUpPeriod,
586: 	}
587: }
588: 
589: func testHTITLCreateDefault() *HilbertTransformerInstantaneousTrendLine {
590: 	x, _ := NewHilbertTransformerInstantaneousTrendLineDefault()
591: 
592: 	return x
593: }

(End of file - total 593 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.6s)

_Thinking:_

 I'm pulling the reference test data arrays from the TrendCycleModeTest file to use as my baseline for validation.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleModeTest.cs"
}
```

**Output:**
```
<path>/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleModeTest.cs</path>
<type>file</type>
<content>1: ﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
2: using System;
3: using System.Collections.Generic;
4: using System.IO;
5: using System.Runtime.Serialization;
6: using System.Xml;
7: using Mbst.Trading;
8: using Mbst.Trading.Indicators.JohnEhlers;
9: 
10: namespace Tests.Indicators.JohnEhlers
11: {
12:     [TestClass]
13:     public class TrendCycleModeTest
14:     {
15:         #region Test data
16:         /// <summary>
17:         /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MAMA.xsl, price, D5…D256, 252 entries.
18:         /// </summary>
19:         private readonly List<double> rawInput = new List<double>
20:         {
21:              92.0000,  93.1725,  95.3125,  94.8450,  94.4075,  94.1100,  93.5000,  91.7350,  90.9550,  91.6875,
22:              94.5000,  97.9700,  97.5775,  90.7825,  89.0325,  92.0950,  91.1550,  89.7175,  90.6100,  91.0000,
23:              88.9225,  87.5150,  86.4375,  83.8900,  83.0025,  82.8125,  82.8450,  86.7350,  86.8600,  87.5475,
24:              85.7800,  86.1725,  86.4375,  87.2500,  88.9375,  88.2050,  85.8125,  84.5950,  83.6575,  84.4550,
25:              83.5000,  86.7825,  88.1725,  89.2650,  90.8600,  90.7825,  91.8600,  90.3600,  89.8600,  90.9225,
26:              89.5000,  87.6725,  86.5000,  84.2825,  82.9075,  84.2500,  85.6875,  86.6100,  88.2825,  89.5325,
27:              89.5000,  88.0950,  90.6250,  92.2350,  91.6725,  92.5925,  93.0150,  91.1725,  90.9850,  90.3775,
28:              88.2500,  86.9075,  84.0925,  83.1875,  84.2525,  97.8600,  99.8750, 103.2650, 105.9375, 103.5000,
29:             103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,
30:             120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,
31:             114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,
32:             114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,
33:             124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,
34:             137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,
35:             123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,
36:             122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,
37:             123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,
38:             130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,
39:             127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,
40:             121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,
41:             106.9700, 110.0300,  91.0000,  93.5600,  93.6200,  95.3100,  94.1850,  94.7800,  97.6250,  97.5900,
42:              95.2500,  94.7200,  92.2200,  91.5650,  92.2200,  93.8100,  95.5900,  96.1850,  94.6250,  95.1200,
43:              94.0000,  93.7450,  95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,
44:             103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,
45:             106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,
46:             109.5300, 108.0600
47:         };
48: 
49:         /// <summary>
50:         /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MAMA.xsl, Period Adjustment, X5…X256, 252 entries, smoothed as AI18=0.33*X18+0.67*AI17.
51:         /// </summary>
52:         private readonly List<double> dcPeriod = new List<double>
53:         {
54:              0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,
55:              0.00000000000000,  0.00000000000000,  0.39600000000000,  0.97812000000000,  1.62158040000000,  2.25545086800000,  2.84234568156000,  3.36868456664520,  3.86776291565229,  4.36321983508703,
56:              4.87235783926831,  5.40838035704577,  5.98190550443027,  6.60199641969884,  7.27686930610184,  8.01438731048222,  8.82241286095647,  9.70906731606755, 10.68293087091460, 11.75320502957710,
57:             12.92985285048740, 14.22372743856440, 15.55272286275450, 16.77503611571540, 17.85814025111630, 18.72970387649220, 19.30387978646140, 19.63314544969620, 19.86256979430250, 20.09030968609300,
58:             20.24817834009410, 20.31132870798190, 20.52152604110820, 21.27119054536480, 22.10966835167300, 22.28715460952700, 21.91280773257140, 21.23923470724180, 20.70813161651310, 20.20449150221090,
59:             19.52863321263000, 18.73709250583170, 17.96311275281150, 17.33367762545960, 16.91743352044750, 16.64300564862120, 16.41952162419500, 16.27464914327850, 16.26425245094380, 16.33321577028600,
60:             16.39265551523990, 16.39976990202790, 16.38221107536320, 16.37405059271910, 16.35102942468120, 16.26839438425590, 16.12432207371240, 15.99529098667630, 15.96458956780100, 16.07977539207760,
61:             16.38360881255670, 16.79746341307210, 17.18753188776420, 17.58524022168910, 18.05888760471740, 18.46077773999830, 18.78691120238400, 19.07789869381110, 19.11803073417110, 18.72675385299730,
62:             18.07403190737810, 17.72999456892580, 18.00699920187680, 18.06680349806270, 17.73551482016550, 17.28467833183610, 16.97456900115070, 16.89386283663200, 17.00556420464730, 17.21986959021550,
63:             17.48251598471900, 17.79647268844360, 18.15809655229470, 18.56044162987590, 19.03705300462320, 19.61779465906120, 20.13838368155990, 20.58144279802850, 20.93554178712450, 21.09578565733870,
64:             21.19426268582890, 21.35270550953270, 21.46806615214910, 21.43420235778580, 21.27320458618770, 20.98617884905010, 20.62345107800030, 20.32165030848570, 20.09921951571820, 19.88214300840560,
65:             19.67081622699810, 19.55217428481160, 19.60485773311710, 19.77836095343260, 19.77886122563300, 19.59009982815140, 19.54609435364200, 19.77945658439880, 20.20526697824140, 20.80572859375930,
66:             21.45882440191380, 21.50916115262280, 21.07219135457730, 20.33979206665380, 19.60807769029340, 19.15831017112920, 19.03006205140340, 19.23359250887840, 19.84206353515510, 20.83692898803630,
67:             22.25776348341490, 23.50933063567320, 24.02857349775940, 24.28548086650010, 24.74576845262060, 25.45685387492870, 26.31998583396390, 27.14553013410700, 27.80677101851790, 28.50146147525040,
68:             29.16835938704370, 29.38723525724370, 29.55886298198770, 30.43981360336700, 30.70779370313880, 30.20667454311960, 29.23518282361370, 28.00037502954910, 26.95505291681000, 26.22399862702800,
69:             25.67716809996900, 25.19893752937060, 24.76924271120940, 24.40654607774420, 24.12997738279210, 24.02648590415090, 24.17912316847620, 24.26552607123530, 24.07565548132200, 23.81050493977940,
70:             23.75771490624360, 24.01627030476950, 24.42884190933990, 24.55867905189440, 24.41978729840000, 24.33536819272640, 24.11887925396970, 23.53741527509780, 22.66734716257270, 21.70419061052260,
71:             20.78848949032480, 19.92593130809770, 19.09528115584620, 18.35405205698280, 17.81539769318840, 17.53491540732180, 17.59552736216070, 18.09376127214910, 18.69300796204700, 19.10361709066390,
72:             19.40368660687600, 19.79324964337850, 20.14261316711870, 20.30292592814370, 20.37642955508450, 20.42856373321320, 20.36417897031590, 20.26870923265670, 20.31691792510900, 20.52593924664440,
73:             20.95797078839970, 21.70315565998060, 22.68588957914270, 23.95566588814480, 25.30036991408680, 26.49222048853470, 28.24485763802440, 30.46863151925310, 31.19661794415910, 30.97271495031300,
74:             30.15801520320610, 29.52193986806340, 28.48090879451130, 27.20913817575940, 25.84740758865390, 24.75875079095690, 24.57820671512040, 25.25622655282780, 26.58938264946150, 28.44936832011270,
75:             30.75900691394640, 31.63120735338530, 31.95156902113430, 32.19329221743080, 32.18129930292270, 31.78927079951340, 30.94427836437330, 29.74153261553520, 28.44319750131350, 27.27756983469050,
76:             26.30928991862760, 25.59706087830910, 25.19354035279110, 24.98183319418390, 24.66611779383150, 24.13629363553260, 23.59372342374540, 23.45943359521940, 24.13462330023790, 25.42868068174450,
77:             27.22154743441240, 28.85990121754770, 29.25658159944000, 28.86760790158470, 28.27077502042400, 27.83957963686970, 27.56292753489200, 27.31665028261770, 27.11537844471070, 27.05619511102920,
78:             26.72669604084850, 25.93839467294110, 24.88015320695530, 23.98089561843900, 23.51115215671300, 23.02173482203020, 22.29674643126940, 21.42162141795630, 20.54863761751100, 19.78167187971360,
79:             19.14387361712880, 18.61396641752300
80:         };
81: 
82:         /// <summary>
83:         /// Generated using Excel implementation from TA-Lib (http://ta-lib.org/) tests, test_HT.xsl.
84:         /// </summary>
85:         private readonly List<double> dcPhase = new List<double>
86:         {
87:             double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,
88:             double.NaN,  double.NaN,  double.NaN,
89:             639.09090909090900,  98.05299963194700, 132.00564338345400,  69.61331949528320,  72.53929158292410,  49.31988383698000,  70.57064052204580,  95.44397344939550,  15.53098102018940,  -7.59977335936253,
90:              -0.43908926482948,   0.75159304711349,   6.81682854281200,  10.38041464097730,  14.60138862232650,  23.61952231391600,  34.25173964222910,  45.61939612889610,  54.93203281582900,  62.34730431858280,
91:              59.09332873285360,  64.74787861710400,  71.38960942346380,  78.26591034671290,  95.01873223899610, 100.81260420916600, 122.15965196914300, 155.92351856084900, 203.60327223472200, 237.36293513970900,
92:             244.38539212971100, 260.58654095568100, 254.66267427143100, 253.71116813655900, 126.51252837026600, 108.91084071926300, 120.62704116849900, 135.00432720483700, 148.37778407551400, 166.73764548652900,
93:             192.70016208445300, 227.19271648131900, 250.93069421530800, 272.91238631233800, 306.14557952390500, -30.56440912946640,  -7.29779366562593,  15.79435681229920,  37.27143784018100,  57.28482204365770,
94:              77.89434659794900, 100.26640790428300, 122.46792347519500, 143.34860922315100, 161.97099218663000, 179.04173949124300, 194.28337763382200, 207.12978658551400, 219.12056620855700, 228.99420937560000,
95:             246.39888033834300, 259.83330071373300, 279.43099672242300, 300.38982045382800, -42.95237520761510,  10.71059944380160,  67.91671263897760,  95.69661875079390, 116.17343846855600, 131.69663648791300,
96:             145.22435488266800, 157.96979483912800, 170.90824231177600, 190.12565895164300, 201.99948636973700, 211.84934448916700, 219.50941069822200, 223.55536694633000, 217.70267385839600, 208.61154857738600,
97:             187.20930268786500, 183.53325881754400, 173.77461459542900, 177.09324308654900, 177.36771678646600, 183.46254384786600, 190.38499136923400, 200.15247884572800, 210.26397611554500, 219.92876325408700,
98:             230.19440003342700, 241.78128922383700, 255.09398089013500, 269.90233026686000, 287.31443195349700, -22.67219169276530,  21.31042453594620,  55.99561073880190,  76.28592721136950,  93.25596317796150,
99:             111.58409337397600, 131.21106018529500, 148.25324978238800, 161.34668836868300, 167.25425018604100, 166.56119294454900, 164.79606018404900, 165.59339091940400, 167.71720854219100, 171.16337795073400,
100:             177.90397416349400, 180.76168462321900, 181.18022018527700, 182.90279528585400, 185.03420448405800, 187.45543006582800, 185.33720976352800, 183.30316508274000, 181.31467258646500, 177.91862390253700,
101:             179.98928550766400, 182.00429308763000, 183.62347330064100, 185.57299458485300, 189.35191538410900, 194.23220405450000, 200.11245439803000, 206.61598766532100, 214.60761785986900, 223.17826798412900,
102:             229.59983294953500, 238.43424165042300, 244.29554481830400, 259.55186325451500, 277.88404982202100, 297.46674550039700, -42.41381860502280, -22.05561416691630, -10.76874195668260,   4.55423903808060,
103:              13.05706378617280,  25.49753552893060,  31.99805128632640,  38.60334291217220,  45.70890152749460,  53.48688015996070,  60.82873486408750,  67.21350248296350,  74.98104788942620,  84.48479832948730,
104:              97.41668808537870, 101.86825371917200, 131.27099687518600, 148.43969963128300, 169.23915874881900, 186.79876808562800, 222.27683786450500, 226.80174114624100, 148.12023947867300, 119.78684089821000,
105:             123.91411010078200, 136.35086145099600, 148.39281918279000, 160.94079449625800, 171.89311802023800, 182.21130213571000, 189.94824024493000, 202.58335256387900, 215.72849327557900, 224.87477584909900,
106:             239.93794152235800, 257.07736337068300, 274.90021437724400, 292.81710751479900, 310.18755119542100, -34.09748283129430, -20.55209191423000, -17.48926436788570,  -6.64084597196063,  -3.41661969757843,
107:              -1.11706604110969,  -0.61559202318233,  -0.58343503934742,  -0.97020892968976,  -7.33789387098142, -13.71942088996000, -14.16303590443250,  -9.85074940762922,  -2.93111556519989,   0.35846302482548,
108:              -0.27797938543370,  -4.11254536082527,  -7.19786584394603,  -8.12930435521150,  -7.23274284499956,  -5.60008181003320,  -3.98246383052538,  -1.93459828828531,  -0.91376116945821,   1.11347590999549,
109:               3.48574296192987,   5.87739974191743,   8.51611669495514,  11.77045158406290,  16.98321519660290,  23.12127015453780,  32.37560208179040,  38.09489298723020,  44.37798569415560,  48.60625731428030,
110:              57.00174598372450,  65.55585833338270,  77.53688240972470,  96.95830008657520, 111.55989345666400, 124.32277400743800, 131.41492641407500, 137.73657404096000, 141.46543104438800, 142.80498887855200,
111:             149.73966957805500, 153.53899811794900, 157.79183782289500, 159.94501203849600, 162.96541156987000, 170.96418133015300, 176.71998519472000, 182.65581927371500, 189.05283471879000, 197.04766728263800,
112:             206.92813680000000, 217.32451174036000, 224.38800791349600, 232.20526011220900, 246.06238925337200, 271.09605242709400, 289.11924018406000,  -31.18231981512890, 23.26913353342980,  47.27652706672060
113:         };
114: 
115:         /// <summary>
116:         /// Generated using Excel implementation from TA-Lib (http://ta-lib.org/) tests, test_HT.xsl.
117:         /// </summary>
118:         private readonly List<double> sine = new List<double>
119:         {
120:             double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,
121:             double.NaN, double.NaN,  double.NaN,
122:             -0.9874388886763950, 0.9901389074597280, 0.7430789154373040, 0.9373629963767220, 0.9539229407379340, 0.7583605937417590, 0.9430523282622310, 0.9954894450161860, 0.2677593920226290,-0.1322524693787770,
123:             -0.0076634783681766, 0.0131173971015221, 0.1186956105503800, 0.1801829214581150, 0.2520928115991770, 0.4006612403521310, 0.5628300247542610, 0.7147094931388430, 0.8184710623910560, 0.8857771050315170,
124:              0.8580051054352940, 0.9044393510772560, 0.9477105512269880, 0.9791019835938800, 0.9961661502181070, 0.9822460058918840, 0.8465682109433450, 0.4079557291761370,-0.4004013665430420,-0.8421036878603060,
125:             -0.9017223344864390,-0.9865337683617420,-0.9643853118844240,-0.9598599815079740, 0.8037267766705520, 0.9460240546952540, 0.8605016852751490, 0.7070533756573690, 0.5243161160505560, 0.2294102723810260,
126:             -0.2198489640488240,-0.7336434870569610,-0.9451240721288890,-0.9987083965673280,-0.8075209150605320,-0.5085066436721070,-0.1270264128351010, 0.2721854747475370, 0.6055917784886910, 0.8413676398899490,
127:              0.9777625488346960, 0.9839896992562860, 0.8436921159626520, 0.5969447127483850, 0.3094984574050860, 0.0167240212732023,-0.2467178763145330,-0.4560076441356610,-0.6309543271436050,-0.7546432714599400,
128:             -0.9163549058535820,-0.9842983646399860,-0.9864836587318940,-0.8626035608908470,-0.6813902164376480, 0.1858483908516400, 0.9266383325265240, 0.9950614294774160, 0.8974629492620320, 0.7466772329212920,
129:              0.5703644678660200, 0.3750953336668390, 0.1580160207328650,-0.1758076016904010,-0.3745982816248290,-0.5276875478007350,-0.6362049492364280,-0.6890552092033720,-0.6115639640491650,-0.4788688150780810,
130:             -0.1254943141670760,-0.0616279223840421, 0.1084398125157010, 0.0507107190849194, 0.0459258492372133,-0.0603960127005548,-0.1802614924906120,-0.3445196936393600,-0.5039846767879240,-0.6418346779779860,
131:             -0.7682209569752370,-0.8811490866996920,-0.9663490613441720,-0.9999985470701830,-0.9546858649139780,-0.3854582462377580, 0.3634207386677970, 0.8289947319983170, 0.9714909193370010, 0.9983857647297360,
132:              0.9298786498974830, 0.7522877436715410, 0.5261656913352240, 0.3198410350076260, 0.2206250837743810, 0.2324067219830480, 0.2622555352188150, 0.2488016117878720, 0.2127369252136960, 0.1536174562004720,
133:              0.0365743929645966,-0.0132935129731492,-0.0205972714630062,-0.0506416644795282,-0.0877504363551195,-0.1297549156821220,-0.0930172242876989,-0.0576191763867261,-0.0229433518557876, 0.0363188766621527,
134:              0.0001870031678450,-0.0349743794742610,-0.0631993922084072,-0.0971138050129020,-0.1624979390656570,-0.2458522401310880,-0.3438638176192820,-0.4480085728769030,-0.5679531815591490,-0.6842705623709480,
135:             -0.7615364178876680,-0.8520399314791350,-0.9010432983130380,-0.9834194615045590,-0.9905476870988080,-0.8872786828920670,-0.6744804687446460,-0.3755063881128740,-0.1868453942460660, 0.0794027920237213,
136:              0.2259213674108930, 0.4304722733878400, 0.5298904205645140, 0.6239251933219030, 0.7158012315374360, 0.8037206344012380, 0.8731666378756390, 0.9219544489912210, 0.9658401621307640, 0.9953707336137810,
137:              0.9916336070524670, 0.9786230879164460, 0.7515981300962940, 0.5233956277754490, 0.1867099275394900,-0.1183826185655860,-0.6727134585677300,-0.7289894296051810, 0.5281384062949680, 0.8678795703185170,
138:              0.8298749053132090, 0.6902403611717530, 0.5240926478571590, 0.3265450140990430, 0.1410201458363470,-0.0385849224232995,-0.1727584531294190,-0.3840270653389320,-0.5839449895165980,-0.7055596595779030,
139:             -0.8654833335157070,-0.9746729156329320,-0.9963449765889340,-0.9217474051403660,-0.7639362508931000,-0.5606026149454520,-0.3510588357296890,-0.3005270942524180,-0.1156452933894830,-0.0595959289499457,
140:             -0.0194952452596040,-0.0107439009455394,-0.0101826864310280,-0.0169325310282176,-0.1277205927832380,-0.2371674472604240,-0.2446819018845580,-0.1710822516700410,-0.0511353061733825, 0.0062563192154928,
141:             -0.0048516364950122,-0.0717158401086350,-0.1252962791394740,-0.1414075684951130,-0.1259001773043880,-0.0975843207989124,-0.0694511521424164,-0.0337586943638060,-0.0159474649423128, 0.0194325975213332,
142:              0.0608001695138442, 0.1024001693533800, 0.1480876049487220, 0.2039912059729780, 0.2920915426229240, 0.3926785587852390, 0.5354672113127650, 0.6169657298756830, 0.6993887726683350, 0.7501832875129180,
143:              0.8386871644310250, 0.9103651277237890, 0.9764351312310820, 0.9926345853646890, 0.9300339420371430, 0.8258742381128520, 0.7499387624704700, 0.6725402422076670, 0.6229867037490120, 0.6045297568333980,
144:              0.5039297182908610, 0.4455885766999630, 0.3779726796046290, 0.3429218283962470, 0.2929489553699220, 0.1570518914741900, 0.0572157944020108,-0.0463361936582273,-0.1573451851308570,-0.2931672021738570,
145:             -0.4528725984930980,-0.6063286567677410,-0.6995137850617610,-0.7902112777605310,-0.9139878106324440,-0.9998170324167670,-0.9448389776183910,-0.5177630386017770, 0.3950506584043580, 0.7346367050507850
146:         };
147: 
148:         /// <summary>
149:         /// Generated using Excel implementation from TA-Lib (http://ta-lib.org/) tests, test_HT.xsl.
150:         /// </summary>
151:         private readonly List<double> sineLead = new List<double>
152:         {
153:             double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,
154:             double.NaN, double.NaN,  double.NaN,
155:             -0.5865008756606930, 0.6010760134973850, 0.0522375953518313, 0.9091393120803100, 0.8866939724449600, 0.9971590529781770, 0.9020538171846020, 0.6368324474361340, 0.8706218323553310, 0.6073789821251400,
156:              0.7016671195580980, 0.7163213444559970, 0.7860384941578910, 0.8229422149880850, 0.8625259332042720, 0.9311800857903170, 0.9824560606058480, 0.9999415670942400, 0.9850130504072370, 0.9545149566415440,
157:              0.9699003741647460, 0.9411885260170180, 0.8957923799982550, 0.8361338691916270, 0.6425371253442400, 0.5619014189746250, 0.2222351486408030,-0.3571214380720770,-0.9310766529205810,-0.9768109870729720,
158:             -0.9433073135761630,-0.8132374819975520,-0.8689541000414960,-0.8770525415166500, 0.1475931480924170, 0.4397692495572380, 0.2482327294712000,-0.0000755239717352,-0.2313707009630830,-0.5260305525740280,
159:             -0.8452633448517760,-0.9992677882479890,-0.8993236485622310,-0.6702662015228300,-0.1539244027456790, 0.2492915015754510, 0.6115575080093740, 0.8728740226620090, 0.9909162839717420, 0.9771019730335230,
160:              0.8396734557306670, 0.5697614425887530, 0.2169861504951610,-0.1451956537134670,-0.4535393411417920,-0.6951822190514210,-0.8597041192450010,-0.9517540619903210,-0.9947396502168260,-0.9975710951353530,
161:             -0.9310629460274820,-0.8208173676831810,-0.5816830016568750,-0.2522412838591280, 0.0357301876330212, 0.8262025299696030, 0.9210718625890570, 0.6334265388777150, 0.3227045140978610, 0.0576226338781002,
162:             -0.1775030791814070,-0.3902458029200550,-0.5864888795958400,-0.8204080195335590,-0.9205013506909260,-0.9737752029989330,-0.9954119273819820,-0.9996821539441930,-0.9919003713144100,-0.9593708639942700,
163:             -0.7902545151018210,-0.7493402295361840,-0.6262584572592140,-0.6703391127989300,-0.6738861991115050,-0.7485223840691900,-0.8229875936131110,-0.9074292725287760,-0.9671080150009190,-0.9960855660349680,
164:             -0.9958932520932240,-0.9574138340514830,-0.8652041011461150,-0.7083111287566420,-0.4646190135134790, 0.3799051621364850, 0.9157357095569460, 0.9816417978833290, 0.8545864068220020, 0.6658040149144910,
165:              0.3974026651162090, 0.0660812869143209,-0.2292556000437840,-0.4438015597350790,-0.5336772503175370,-0.5234089018965820,-0.4969142899048420,-0.5089421255844810,-0.5404930402692640,-0.5900897578798300,
166:             -0.6807716781362360,-0.7164442325283580,-0.7215212414254330,-0.7420085481569570,-0.7664280384961050,-0.7928795495979390,-0.7698142295937170,-0.7466749292126270,-0.7231440467849940,-0.6809589449044260,
167:             -0.7069745376146330,-0.7314048006899320,-0.7503819371382670,-0.7724343179777090,-0.8126119368714450,-0.8592475635206920,-0.9071355135227490,-0.9489640525891650,-0.9835954633562350,-0.9994945745179860,
168:             -0.9967791122784410,-0.9726372051055730,-0.9438266487324060,-0.8236131488885440,-0.6034299982731380,-0.3012592866284200, 0.0451220549347129, 0.3898374562298360, 0.5625345127655470, 0.7610204246544270,
169:              0.8485754490678880, 0.9426271321408510, 0.9743624133056830, 0.9937744211043730, 0.9999234595478280, 0.9890496834828740, 0.9620813190845490, 0.9257815160887320, 0.8661907447689560, 0.7717933199927350,
170:              0.6099143741297350, 0.5465660379422910, 0.0650374448708587,-0.2324218737852860,-0.5626485140051470,-0.7858435966184150,-0.9988707502989160,-0.9995056053709990,-0.2269953459568470, 0.2624108067271960,
171:              0.1922802999868170,-0.0235747958138623,-0.2316259847404300,-0.4374421611852380,-0.6003241683068820,-0.7338638764656210,-0.8186335536900920,-0.9244352736103260,-0.9869359601928300,-0.9999976116346720,
172:             -0.9662055929589490,-0.8473318021865150,-0.6441207677355740,-0.3775643212631200,-0.0838943519776584, 0.1891385830833930, 0.4138657569596380, 0.4619148062355390, 0.6205889300081480, 0.6637092720958960,
173:              0.6931871751689060, 0.6994688836282570, 0.6998698746833480, 0.6950322988997850, 0.6110036127434860, 0.5192294559798360, 0.5125969121038930, 0.5757083087460280, 0.6700235759926480, 0.7115168281729820,
174:              0.7036678339992160, 0.6545752979714670, 0.6129364849611150, 0.6000111421090120, 0.6124554033545860, 0.6347294098505560, 0.6562899876128100, 0.6828327376881520, 0.6957402985880160, 0.7207141791649000,
175:              0.7487908163625680, 0.7757975773123150, 0.8040241461526770, 0.8364818144534760, 0.8828100232401670, 0.9279746557928580, 0.9758237828336110, 0.9927466296862280, 0.9999410720652520, 0.9980198650776760,
176:              0.9781412645524530, 0.9363303228513180, 0.8430454008691210, 0.6162348270656310, 0.3977902134753690, 0.1852760304055380, 0.0625305163973752,-0.0477440698701413,-0.1126037294757250,-0.1358018386690990,
177:             -0.2544275858052280,-0.3179500551407050,-0.3873842567862560,-0.4217482646499100,-0.4689384577141290,-0.5872793774815170,-0.6654907471409910,-0.7391119157698760,-0.8095586705039820,-0.8833378646001520,
178:             -0.9506681839279980,-0.9910404297342630,-0.9999429557041830,-0.9921031907559610,-0.9331896469980470,-0.6934514712647360,-0.4364996881408350, 0.2388331163456430, 0.9289332462002390, 0.9992107524543470
179:         };
180: 
181:         /// <summary>
182:         /// Generated using Excel implementation from TA-Lib (http://ta-lib.org/) tests, test_HT.xsl.
183:         /// </summary>
184:         private readonly List<double> itl = new List<double>
185:         {
186:             double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,
187:             double.NaN, double.NaN,  double.NaN,
188:               0.0000000000000,  36.3130000000000,  63.1977500000000,  81.3542500000000,  90.5332083333333,  90.7274166666667,  90.8630416666667,  90.7904791666667,  90.5763791666666,  90.1190625000000,
189:              89.5976625000000,  88.9683964285714,  88.1875857142857,  87.4745535714286,  86.8648353174603,  86.5406809523809,  86.4262830808081,  86.4413116161616,  86.4596270104895,  86.4576728687979,
190:              86.5316151556777,  86.5976538663542,  86.6975181489262,  86.7923007288442,  86.7608821809425,  86.6849764619883,  86.4810361842105,  86.2045236842105,  85.9179875000000,  85.7217875000000,
191:              85.6731214285714,  85.7294946428571,  85.8820685064935,  86.1150389610390,  86.4291412337662,  86.8542435064935,  87.1897624458874,  87.4640642857143,  87.6829172619047,  87.8553064223057,
192:              87.9218177631579,  87.9011081656347,  87.8265154798762,  87.7802132352941,  87.8975367647059,  87.9932665441176,  88.0649329044118,  88.1058593750000,  88.0771718750000,  87.9885156250000,
193:              87.8919218750000,  87.8735468750000,  87.9217812500000,  88.0132343750000,  88.1667968750000,  88.3524062500000,  88.5845312500000,  88.8867187500000,  89.2125000000000,  89.3545928308824,
194:              89.4090386029412,  89.2706643178105,  89.1179705882353,  89.2742091503268,  89.6278143274854,  90.2226447368421,  90.9753333333333,  91.7409342105263,  92.5893245614035,  93.3576615497076,
195:              94.0969948830409,  94.8495138888889,  95.5591111111111,  96.4495179738562,  97.4016168300654,  98.4999640522876,  99.7313676470588, 101.1202794117650, 102.8572647058820, 104.2939607843140,
196:             105.8849452614380, 107.0814910990710, 108.1507799707600, 108.9417928362570, 109.6523348684210, 110.1277241228070, 110.5267148809520, 110.9164619047620, 111.3471666666670, 111.8653571428570,
197:             112.3597857142860, 112.7992380952380, 113.1747142857140, 113.5586666666670, 114.0577738095240, 114.6932047619050, 115.2846898809520, 115.7229029761900, 115.8414500000000, 115.7467000000000,
198:             115.6115500000000, 115.5303375000000, 115.5548000000000, 115.6614000000000, 115.9122375000000, 116.3321625000000, 116.8428375000000, 117.2830500000000, 117.7253922619050, 118.0727228896100,
199:             118.4984101731600, 119.2198956709960, 120.0234311688310, 120.9289954887220, 121.7905789473680, 122.5689105263160, 123.2356894736840, 123.8346386591480, 124.4085501367050, 124.8498862554110,
200:             125.4094954004330, 126.1065170454550, 126.7064925000000, 127.4240825000000, 127.8647176923080, 128.0896789173790, 128.1540621001220, 128.0788455784320, 128.1446725050170, 128.3183694581280,
201:             128.4362103448280, 128.5848821839080, 128.6186973303670, 128.7719790322580, 128.8848929180570, 128.9942553260240, 129.0945314906040, 129.1530999536860, 129.0838581603580, 128.8024011680910,
202:             128.4259584615380, 127.8496453846150, 127.2448900000000, 126.6843541666670, 126.1858333333330, 125.7495416666670, 125.2761041666670, 124.7606666666670, 124.3471666666670, 124.0095208333330,
203:             123.7731875000000, 123.6621125000000, 123.6056416666670, 123.5744416666670, 123.5148250000000, 123.4937083333330, 123.4761041666670, 123.6832443181820, 124.0308714826840, 124.5405766233770,
204:             125.2020811061750, 125.8966645363410, 126.4680156432750, 126.9272675438600, 127.4003611111110, 127.8605833333330, 128.1324342105260, 128.3722748538010, 128.6053611111110, 128.6804526315790,
205:             128.7627552631580, 128.7694289473680, 128.7595250000000, 128.7409750000000, 128.6613250000000, 128.4858250000000, 128.2154750000000, 127.8500845238100, 127.4645440476190, 127.1153875541130,
206:             126.7443323451910, 126.3035666290230, 125.8504999604740, 125.3674468450390, 124.8902561172160, 124.3766410622710, 123.8351980503370, 123.2633556835640, 122.5153494623660, 121.7438575268820,
207:             120.2481227342550, 118.4358502645500, 116.5046713064710, 114.5443467073670, 112.9054354415950, 111.4893484615380, 110.7192281481480, 110.1931396825400, 110.2034782445810, 110.1120396292030,
208:             109.5987628888250, 108.8005075604840, 107.8019843750000, 106.8111562500000, 105.6388140120970, 104.3560227486560, 102.7347166042630, 101.0735459592080,  99.5273900488400,  98.3256981583231,
209:              97.3580796011396,  96.7594046153846,  96.4602600000000,  96.1232166666667,  96.1488191666667,  96.4425624637681,  96.7984085144927,  97.1613937318841,  97.3868053381643,  97.5343687643678,
210:              97.8406773180077,  98.3334693486590,  99.0595665024631,  99.8837727832512, 100.6778885467980, 101.4226269841270, 102.0713564814810, 102.6847440476190, 103.2616851851850, 103.9931695156700,
211:             104.8224465811970, 105.7652606837610, 106.6112275641030, 107.5680471014490, 108.3253364624510, 108.8823498494260, 109.2757501411630, 109.6873409090910, 110.1830037593980, 110.4551917293230
212:         };
213: 
214:         /// <summary>
215:         /// Taken from TA-Lib (http://ta-lib.org/) implementation, test_HT.xsl.
216:         /// The first 63 values are skipped (unstable period according to TA-Lib), total 200 values available (trial version).
217:         /// </summary>
218:         private readonly List<double> tcm = new List<double>
219:         {
220:             // First 63 entries are not available (unstable period).
221:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
222:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
223:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
224:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
225:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
226:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
227:             double.NaN, double.NaN, double.NaN,
228:             1, 1, 1, 1, 1, 1, 1,-1,-1, 1, 1, 1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
229:             1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1, 1, 1, 1,-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
230:             1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
231:             1, 1, 1,-1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1, 1, 1,
232:             1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
233:         };
234:         #endregion
235: 
236:         #region NameTest
237:         /// <summary>
238:         /// A test for Name.
239:         /// </summary>
240:         [TestMethod]
241:         public void NameTest()
242:         {
243:             var target = new TrendCycleMode();
244:             Assert.AreEqual("TCM", target.Name);
245:         }
246:         #endregion
247: 
248:         #region MonikerTest
249:         /// <summary>
250:         /// A test for Moniker.
251:         /// </summary>
252:         [TestMethod]
253:         public void MonikerTest()
254:         {
255:             var target = new TrendCycleMode();
256:             Assert.AreEqual("TCM4", target.Moniker);
257:             //target = new TrendCycleMode(4);
258:             //Assert.AreEqual("TCM4", target.Moniker);
259:             target = new TrendCycleMode(/*3*/);
260:             Assert.AreEqual("TCM3", target.Moniker);
261:             target = new TrendCycleMode(/*2*/);
262:             Assert.AreEqual("TCM2", target.Moniker);
263:         }
264:         #endregion
265: 
266:         #region DescriptionTest
267:         /// <summary>
268:         /// A test for Description.
269:         /// </summary>
270:         [TestMethod]
271:         public void DescriptionTest()
272:         {
273:             var target = new TrendCycleMode();
274:             Assert.AreEqual("Trend versus Cycle Mode", target.Description);
275:         }
276:         #endregion
277: 
278:         #region SmoothingLengthTest
279:         /// <summary>
280:         /// A test for SmoothingLength.
281:         /// </summary>
282:         [TestMethod]
283:         public void SmoothingLengthTest()
284:         {
285:             var target = new TrendCycleMode();
286:             Assert.AreEqual(4, target.SmoothingLength);
287:             //target = new TrendCycleMode(4);
288:             //Assert.AreEqual(4, target.SmoothingLength);
289:             target = new TrendCycleMode(/*3*/);
290:             Assert.AreEqual(3, target.SmoothingLength);
291:             target = new TrendCycleMode(/*2*/);
292:             Assert.AreEqual(2, target.SmoothingLength);
293:         }
294:         #endregion
295: 
296:         #region IsPrimedTest
297:         /// <summary>
298:         /// A test for IsPrimed.
299:         /// </summary>
300:         [TestMethod]
301:         public void IsPrimedTest()
302:         {
303:             var target = new TrendCycleMode();
304:             Assert.IsFalse(target.IsPrimed);
305:             for (int i = 1; i <= 23; ++i)
306:             {
307:                 var scalar = new Scalar(DateTime.Now, i);
308:                 scalar = target.Update(scalar);
309:                 Assert.IsTrue(double.IsNaN(scalar.Value));
310:                 Assert.IsFalse(target.IsPrimed);
311:             }
312:             for (int i = 24; i <= 48; ++i)
313:             {
314:                 var scalar = new Scalar(DateTime.Now, i);
315:                 scalar = target.Update(scalar);
316:                 Assert.IsFalse(double.IsNaN(scalar.Value));
317:                 Assert.IsTrue(target.IsPrimed);
318:             }
319:         }
320:         #endregion
321: 
322:         #region DominantCyclePeriodTest
323:         /// <summary>
324:         /// A test for DominantCyclePeriod.
325:         /// </summary>
326:         [TestMethod]
327:         public void DominantCyclePeriodTest()
328:         {
329:             const int digits = 9;
330:             var target = new TrendCycleMode();
331:             // Tradestation implementation skips first 9 bars.
332:             for (int i = 9; i < rawInput.Count; ++i)
333:             {
334:                 target.Update(rawInput[i]);
335:                 double d = Math.Round(target.DominantCyclePeriod, digits);
336:                 double u = Math.Round(dcPeriod[i], digits);
337:                 if (!double.IsNaN(d))
338:                     Assert.AreEqual(u, d);
339:             }
340:         }
341:         #endregion
342: 
343:         #region DominantCyclePhaseTest
344:         /// <summary>
345:         /// A test for DominantCyclePhase.
346:         /// </summary>
347:         [TestMethod]
348:         public void DominantCyclePhaseTest()
349:         {
350:             const int digits = 8;
351:             var target = new TrendCycleMode();
352:             // Tradestation implementation skips first 9 bars.
353:             for (int i = 9; i < rawInput.Count; ++i)
354:             {
355:                 target.Update(rawInput[i]);
356:                 double d = Math.Round(target.DominantCyclePhase, digits);
357:                 double u = Math.Round(dcPhase[i], digits);
358:                 if (!double.IsNaN(d))
359:                     Assert.AreEqual(u, d);
360:             }
361:         }
362:         #endregion
363: 
364:         #region SineWaveTest
365:         /// <summary>
366:         /// A test for SineWave.
367:         /// </summary>
368:         [TestMethod]
369:         public void SineWaveTest()
370:         {
371:             const int digits = 9;
372:             var target = new TrendCycleMode();
373:             // Tradestation implementation skips first 9 bars.
374:             for (int i = 9; i < rawInput.Count; ++i)
375:             {
376:                 target.Update(rawInput[i]);
377:                 double d = Math.Round(target.SineWave, digits);
378:                 double u = Math.Round(sine[i], digits);
379:                 if (!double.IsNaN(d))
380:                     Assert.AreEqual(u, d);
381:             }
382:         }
383:         #endregion
384: 
385:         #region SineWaveLeadTest
386:         /// <summary>
387:         /// A test for SineWaveLead.
388:         /// </summary>
389:         [TestMethod]
390:         public void SineWaveLeadTest()
391:         {
392:             const int digits = 9;
393:             var target = new TrendCycleMode();
394:             // Tradestation implementation skips first 9 bars.
395:             for (int i = 9; i < rawInput.Count; ++i)
396:             {
397:                 target.Update(rawInput[i]);
398:                 double d = Math.Round(target.SineWaveLead, digits);
399:                 double u = Math.Round(sineLead[i], digits);
400:                 if (!double.IsNaN(d))
401:                     Assert.AreEqual(u, d);
402:             }
403:         }
404:         #endregion
405: 
406:         #region InstantaneousTrendLineTest
407:         /// <summary>
408:         /// A test for InstantaneousTrendLine.
409:         /// </summary>
410:         [TestMethod]
411:         public void InstantaneousTrendLineTest()
412:         {
413:             const int digits = 9;
414:             var target = new TrendCycleMode();
415:             // Tradestation implementation skips first 9 bars.
416:             for (int i = 9; i < rawInput.Count; ++i)
417:             {
418:                 target.Update(rawInput[i]);
419:                 double d = Math.Round(target.InstantaneousTrendLine, digits);
420:                 double u = Math.Round(itl[i], digits);
421:                 if (!double.IsNaN(d))
422:                     Assert.AreEqual(u, d);
423:             }
424:         }
425:         #endregion
426: 
427:         #region UpdateTest
428:         /// <summary>
429:         /// A test for Update.
430:         /// </summary>
431:         [TestMethod]
432:         public void UpdateTest()
433:         {
434:             const int digits = 9;
435:             var target = new TrendCycleMode();
436:             // Tradestation implementation skips first 9 bars.
437:             for (int i = 9; i < tcm.Count; ++i)
438:             {
439:                 target.Update(rawInput[i]);
440:                 double d = Math.Round(target.Value, digits);
441:                 double u = Math.Round(tcm[i], digits);
442:                 if (i != 70 && i != 71 && !double.IsNaN(u))
443:                     Assert.AreEqual(u, d);
444:             }
445:         }
446:         #endregion
447: 
448:         #region ResetTest
449:         /// <summary>
450:         /// A test for Reset.
451:         /// </summary>
452:         [TestMethod]
453:         public void ResetTest()
454:         {
455:             double d, u; const int digits = 9;
456:             var target = new TrendCycleMode();
457:             // Tradestation implementation skips first 9 bars.
458:             for (int i = 9; i < tcm.Count; i++)
459:             {
460:                 target.Update(rawInput[i]);
461:                 d = Math.Round(target.Value, digits);
462:                 u = Math.Round(tcm[i], digits);
463:                 if (i != 70 && i != 71 && !double.IsNaN(u))
464:                     Assert.AreEqual(u, d);
465:             }
466:             target.Reset();
467:             for (int i = 9; i < tcm.Count; i++)
468:             {
469:                 target.Update(rawInput[i]);
470:                 d = Math.Round(target.Value, digits);
471:                 u = Math.Round(tcm[i], digits);
472:                 if (i != 70 && i != 71 && !double.IsNaN(u))
473:                     Assert.AreEqual(u, d);
474:             }
475:         }
476:         #endregion
477: 
478:         #region ConstructorTest
479:         /// <summary>
480:         /// A test for Constructor.
481:         /// </summary>
482:         [TestMethod]
483:         public void TrendCycleModeConstructorTest()
484:         {
485:             var target = new TrendCycleMode();
486:             Assert.AreEqual(4, target.SmoothingLength);
487:             Assert.IsFalse(target.IsPrimed);
488:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.MedianPrice);
489: 
490:             target = new TrendCycleMode(ohlcvComponent:OhlcvComponent.TypicalPrice);
491:             Assert.AreEqual(4, target.SmoothingLength);
492:             Assert.IsFalse(target.IsPrimed);
493:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.TypicalPrice);
494: 
495:             target = new TrendCycleMode(/*3*/);
496:             Assert.AreEqual(3, target.SmoothingLength);
497:             Assert.IsFalse(target.IsPrimed);
498:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.MedianPrice);
499: 
500:             target = new TrendCycleMode(/*3, 1, OhlcvComponent.OpeningPrice*/);
501:             Assert.AreEqual(3, target.SmoothingLength);
502:             Assert.IsFalse(target.IsPrimed);
503:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.OpeningPrice);
504: 
505:             target = new TrendCycleMode(/*2*/);
506:             Assert.AreEqual(2, target.SmoothingLength);
507:             Assert.IsFalse(target.IsPrimed);
508:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.MedianPrice);
509: 
510:             target = new TrendCycleMode(/*2, 1, OhlcvComponent.OpeningPrice*/);
511:             Assert.AreEqual(2, target.SmoothingLength);
512:             Assert.IsFalse(target.IsPrimed);
513:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.OpeningPrice);
514:         }
515: 
516:         /// <summary>
517:         /// A test for constructor exception.
518:         /// </summary>
519:         [TestMethod]
520:         [ExpectedException(typeof(ArgumentOutOfRangeException))]
521:         public void TrendCycleModeConstructorTest2()
522:         {
523:             var target = new TrendCycleMode(/*5*/);
524:             Assert.IsFalse(target.IsPrimed);
525:         }
526: 
527:         /// <summary>
528:         /// A test for constructor exception.
529:         /// </summary>
530:         [TestMethod]
531:         [ExpectedException(typeof(ArgumentOutOfRangeException))]
532:         public void TrendCycleModeConstructorTest3()
533:         {
534:             var target = new TrendCycleMode(/*1*/);
535:             Assert.IsFalse(target.IsPrimed);
536:         }
537: 
538:         /// <summary>
539:         /// A test for constructor exception.
540:         /// </summary>
541:         [TestMethod]
542:         [ExpectedException(typeof(ArgumentOutOfRangeException))]
543:         public void TrendCycleModeConstructorTest4()
544:         {
545:             var target = new TrendCycleMode(/*0*/);
546:             Assert.IsFalse(target.IsPrimed);
547:         }
548: 
549:         /// <summary>
550:         /// A test for constructor exception.
551:         /// </summary>
552:         [TestMethod]
553:         [ExpectedException(typeof(ArgumentOutOfRangeException))]
554:         public void TrendCycleModeConstructorTest5()
555:         {
556:             var target = new TrendCycleMode(/*-8*/);
557:             Assert.IsFalse(target.IsPrimed);
558:         }
559:         #endregion
560: 
561:         #region SerializationTest
562:         private static void SerializeTo(TrendCycleMode instance, string fileName)
563:         {
564:             var dcs = new DataContractSerializer(typeof(TrendCycleMode), null, 65536, false, true, null);
565:             using (var fs = new FileStream(fileName, FileMode.Create))
566:             {
567:                 dcs.WriteObject(fs, instance);
568:                 fs.Close();
569:             }
570:         }
571: 
572:         private static TrendCycleMode DeserializeFrom(string fileName)
573:         {
574:             var fs = new FileStream(fileName, FileMode.Open);
575:             XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
576:             var ser = new DataContractSerializer(typeof(TrendCycleMode), null, 65536, false, true, null);
577:             var instance = (TrendCycleMode)ser.ReadObject(reader, true);
578:             reader.Close();
579:             fs.Close();
580:             return instance;
581:         }
582: 
583:         /// <summary>
584:         /// A test for the serialization.
585:         /// </summary>
586:         [TestMethod]
587:         public void SerializationTest()
588:         {
589:             double d, u; const int digits = 9;
590:             var source = new TrendCycleMode();
591:             // Tradestation-implementation skips first 9 bars.
592:             for (int i = 9; i < 111; ++i)
593:             {
594:                 source.Update(rawInput[i]);
595:                 d = Math.Round(source.Value, digits);
596:                 u = Math.Round(tcm[i], digits);
597:                 if (i != 70 && i != 71 && !double.IsNaN(u))
598:                     Assert.AreEqual(u, d);
599:             }
600:             const string fileName = "TrendCycleMode_1.xml";
601:             SerializeTo(source, fileName);
602:             TrendCycleMode target = DeserializeFrom(fileName);
603:             Assert.AreEqual(source.Value, target.Value);
604:             Assert.AreEqual(source.DominantCyclePeriod, target.DominantCyclePeriod);
605:             // Tradestation-implementation skips first 9 bars.
606:             for (int i = 111; i < tcm.Count; ++i)
607:             {
608:                 target.Update(rawInput[i]);
609:                 d = Math.Round(target.Value, digits);
610:                 u = Math.Round(tcm[i], digits);
611:                 if (!double.IsNaN(u))
612:                     Assert.AreEqual(u, d);
613:             }
614:             //FileInfo fi = new FileInfo(fileName);
615:             //fi.Delete();
616:         }
617:         #endregion
618:     }
619: }

(End of file - total 619 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 8.2s)

_Thinking:_

 Now I'm checking the HTITL sinewave file structure to confirm the output ordering, then I'll generate the five Go files I need.

**Tool: grep**

**Input:**
```json
{
  "pattern": "type Indicator interface",
  "path": "/home/dev/zpano/go/indicators/core"
}
```

**Output:**
```
Found 1 matches
/home/dev/zpano/go/indicators/core/indicator.go:
  Line 8: type Indicator interface {
```

---

## Assistant (Build · claude-opus-4.7 · 3.2s)

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

---

## Assistant (Build · claude-opus-4.7 · 40.1s)

_Thinking:_

 I'm setting up the Go TCM implementation with the standard file structure—output, params, main, and their corresponding tests. The output fields follow a fixed order: Value, IsTrendMode, IsCycleMode, InstantaneousTrendLine, SineWave, SineWaveLead, DominantCyclePeriod, and DominantCyclePhase. I'll use the MBST mnemonic format with parameters for alpha, trend line slope, cycle multiplier, separation percentage, estimator type, and component selection.

For the format string, I'm settling on fixed precision to match zpano conventions: `%.3f` for alpha and cycle multiplier (consistent with HTITL), `%d` for the integer parameter, and `%.3f%%` for the separation percentage, giving output like "tcm(0.330, 4, 1.000, 1.500%, hl/2)". Now I'll generate the five files.

Looking at how other multi-output indicators like SineWave handle mnemonics, I'll use TCM-prefixed variants for each output: the main value gets `tcm(α, tlsl, cpMul, sep%, est, comp)`, trend mode gets `tcm-trend(...)`, cycle mode gets `tcm-cycle(...)`, and the instantaneous trend line gets `tcm-itl(...)` with the appropriate parameters for each.

For the sine and dominant cycle outputs, I'll follow the same pattern—`tcm-sine(...)` and `tcm-sineLead(...)` include all six parameters for consistency, while `dcp(...)` and `dcph(...)` reuse the wrapped DominantCycle's own mnemonics since those are just passthroughs. Even though sine outputs don't technically depend on tlsl and cpMul, including them everywhere keeps the convention uniform.

Now I'm formatting the actual suffix string with the parameter values: when using default estimator and component, it becomes `(0.330, 4, 1.000, 1.500%, hl/2)`.

Writing the output file...

Now I have everything needed. Let me create the 5 Go files.

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput.go",
  "content": "package trendcyclemode\n\nimport (\n\t\"bytes\"\n\t\"fmt\"\n)\n\n// Output describes the outputs of the indicator.\ntype Output int\n\nconst (\n\t// Value is the Trend-versus-Cycle Mode value: +1 in trend mode, -1 in cycle mode.\n\tValue Output = iota + 1\n\t// IsTrendMode is 1 if the trend mode is declared, 0 otherwise.\n\tIsTrendMode\n\t// IsCycleMode is 1 if the cycle mode is declared, 0 otherwise (= 1 - IsTrendMode).\n\tIsCycleMode\n\t// InstantaneousTrendLine is the WMA-smoothed instantaneous trend line.\n\tInstantaneousTrendLine\n\t// SineWave is the sine wave value, sin(phase·Deg2Rad).\n\tSineWave\n\t// SineWaveLead is the sine wave lead value, sin((phase+45)·Deg2Rad).\n\tSineWaveLead\n\t// DominantCyclePeriod is the smoothed dominant cycle period.\n\tDominantCyclePeriod\n\t// DominantCyclePhase is the dominant cycle phase, in degrees.\n\tDominantCyclePhase\n\toutputLast\n)\n\nconst (\n\tvalueStr                  = \"value\"\n\tisTrendModeStr            = \"isTrendMode\"\n\tisCycleModeStr            = \"isCycleMode\"\n\tinstantaneousTrendLineStr = \"instantaneousTrendLine\"\n\tsineWaveStr               = \"sineWave\"\n\tsineWaveLeadStr           = \"sineWaveLead\"\n\tdominantCyclePeriodStr    = \"dominantCyclePeriod\"\n\tdominantCyclePhaseStr     = \"dominantCyclePhase\"\n\tunknownStr                = \"unknown\"\n)\n\n// String implements the Stringer interface.\nfunc (o Output) String() string {\n\tswitch o {\n\tcase Value:\n\t\treturn valueStr\n\tcase IsTrendMode:\n\t\treturn isTrendModeStr\n\tcase IsCycleMode:\n\t\treturn isCycleModeStr\n\tcase InstantaneousTrendLine:\n\t\treturn instantaneousTrendLineStr\n\tcase SineWave:\n\t\treturn sineWaveStr\n\tcase SineWaveLead:\n\t\treturn sineWaveLeadStr\n\tcase DominantCyclePeriod:\n\t\treturn dominantCyclePeriodStr\n\tcase DominantCyclePhase:\n\t\treturn dominantCyclePhaseStr\n\tdefault:\n\t\treturn unknownStr\n\t}\n}\n\n// IsKnown determines if this output is known.\nfunc (o Output) IsKnown() bool {\n\treturn o >= Value && o < outputLast\n}\n\n// MarshalJSON implements the Marshaler interface.\nfunc (o Output) MarshalJSON() ([]byte, error) {\n\tconst (\n\t\terrFmt = \"cannot marshal '%s': unknown trend cycle mode output\"\n\t\textra  = 2   // Two bytes for quotes.\n\t\tdqc    = '\"' // Double quote character.\n\t)\n\n\ts := o.String()\n\tif s == unknownStr {\n\t\treturn nil, fmt.Errorf(errFmt, s)\n\t}\n\n\tb := make([]byte, 0, len(s)+extra)\n\tb = append(b, dqc)\n\tb = append(b, s...)\n\tb = append(b, dqc)\n\n\treturn b, nil\n}\n\n// UnmarshalJSON implements the Unmarshaler interface.\nfunc (o *Output) UnmarshalJSON(data []byte) error {\n\tconst (\n\t\terrFmt = \"cannot unmarshal '%s': unknown trend cycle mode output\"\n\t\tdqs    = \"\\\"\" // Double quote string.\n\t)\n\n\td := bytes.Trim(data, dqs)\n\ts := string(d)\n\n\tswitch s {\n\tcase valueStr:\n\t\t*o = Value\n\tcase isTrendModeStr:\n\t\t*o = IsTrendMode\n\tcase isCycleModeStr:\n\t\t*o = IsCycleMode\n\tcase instantaneousTrendLineStr:\n\t\t*o = InstantaneousTrendLine\n\tcase sineWaveStr:\n\t\t*o = SineWave\n\tcase sineWaveLeadStr:\n\t\t*o = SineWaveLead\n\tcase dominantCyclePeriodStr:\n\t\t*o = DominantCyclePeriod\n\tcase dominantCyclePhaseStr:\n\t\t*o = DominantCyclePhase\n\tdefault:\n\t\treturn fmt.Errorf(errFmt, s)\n\t}\n\n\treturn nil\n}\n"
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

## Assistant (Build · claude-opus-4.7 · 16.0s)

_Thinking:_

 Those LSP errors are just noise from a reference folder, so I can ignore them and move forward with the params file.

**Tool: write**

**Input:**
```json
{
  "content": "package trendcyclemode\n\nimport (\n\t\"zpano/entities\"\n\t\"zpano/indicators/johnehlers/hilberttransformer\"\n)\n\n// Params describes parameters to create an instance of the indicator.\ntype Params struct {\n\t// EstimatorType is the type of cycle estimator to use.\n\t// The default value is hilberttransformer.HomodyneDiscriminator.\n\tEstimatorType hilberttransformer.CycleEstimatorType\n\n\t// EstimatorParams describes parameters to create an instance\n\t// of the Hilbert transformer cycle estimator.\n\tEstimatorParams hilberttransformer.CycleEstimatorParams\n\n\t// AlphaEmaPeriodAdditional is the value of α (0 < α ≤ 1) used in EMA\n\t// for additional smoothing of the instantaneous period.\n\t//\n\t// The default value is 0.33.\n\tAlphaEmaPeriodAdditional float64\n\n\t// TrendLineSmoothingLength is the additional WMA smoothing length used to smooth the trend line.\n\t// The valid values are 2, 3, 4. The default value is 4.\n\tTrendLineSmoothingLength int\n\n\t// CyclePartMultiplier is the multiplier to the dominant cycle period used to determine\n\t// the window length to calculate the trend line. The typical values are in [0.5, 1.5].\n\t// The default value is 1.0. Valid range is (0, 10].\n\tCyclePartMultiplier float64\n\n\t// SeparationPercentage is the threshold (in percent) above which a wide separation between\n\t// the WMA-smoothed price and the instantaneous trend line forces the trend mode.\n\t//\n\t// The default value is 1.5. Valid range is (0, 100].\n\tSeparationPercentage float64\n\n\t// BarComponent indicates the component of a bar to use when updating the indicator with a bar sample.\n\t//\n\t// If zero, the default (BarMedianPrice) is used. Since the default differs from the framework\n\t// default bar component, it is always shown in the indicator mnemonic.\n\tBarComponent entities.BarComponent\n\n\t// QuoteComponent indicates the component of a quote to use when updating the indicator with a quote sample.\n\t//\n\t// If zero, the default (QuoteMidPrice) is used and the component is not shown in the indicator mnemonic.\n\tQuoteComponent entities.QuoteComponent\n\n\t// TradeComponent indicates the component of a trade to use when updating the indicator with a trade sample.\n\t//\n\t// If zero, the default (TradePrice) is used and the component is not shown in the indicator mnemonic.\n\tTradeComponent entities.TradeComponent\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeparams.go"
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

## Assistant (Build · claude-opus-4.7 · 102.2s)

_Thinking:_

 Moving on to the main TCM file now—this is the tricky part. I need to map out the core MBST logic from the Update method: first the raw sample gets pushed into the local buffer, then if everything's primed, I pull the smoothed period and calculate the trend average.

From there it gets into the weighted moving average calculation for the trendline, then I'm computing the phase difference between the sine wave and its 45-degree lead to determine if we're in trend mode. There are three key conditions to check: whether the sine wave difference changes sign, whether we've accumulated enough samples relative to the period, and whether the phase delta stays within acceptable bounds. Finally, there's a separation factor check that compares the smoothed value against the trendline itself. If we're not yet primed but the detector is ready, I initialize the primed state and set up the initial trendline.

Now I'm setting the initial trend averages equal to that trendline, capturing the current phase and computing the initial sine wave difference. I increment the sample counter and apply the minimum sample threshold before returning the trend mode flag. The wrapper around DominantCycle.Update extracts the smoothed period and phase, adds an accessor for the smoothed price, and returns eight values including the trend mode, cycle mode, trendline, sine waves, period, and phase.

**Tool: write**

**Input:**
```json
{
  "content": "package trendcyclemode\n\n//nolint: gofumpt\nimport (\n\t\"fmt\"\n\t\"math\"\n\t\"sync\"\n\t\"time\"\n\n\t\"zpano/entities\"\n\t\"zpano/indicators/core\"\n\t\"zpano/indicators/core/outputs\"\n\t\"zpano/indicators/johnehlers/dominantcycle\"\n\t\"zpano/indicators/johnehlers/hilberttransformer\"\n)\n\n// Deg2Rad converts degrees to radians.\nconst deg2Rad = math.Pi / 180.0\n\n// TrendCycleMode is Ehlers' Trend-versus-Cycle Mode indicator.\n//\n// It wraps a DominantCycle (for instantaneous period / phase / WMA-smoothed price)\n// and exposes eight outputs:\n//\n//   - Value: +1 in trend mode, -1 in cycle mode.\n//   - IsTrendMode: 1 if trend mode is declared, 0 otherwise.\n//   - IsCycleMode: 1 if cycle mode is declared, 0 otherwise.\n//   - InstantaneousTrendLine: WMA-smoothed trend line.\n//   - SineWave: sin(phase·Deg2Rad).\n//   - SineWaveLead: sin((phase+45)·Deg2Rad).\n//   - DominantCyclePeriod: smoothed dominant cycle period.\n//   - DominantCyclePhase: dominant cycle phase, in degrees.\n//\n// Reference:\n// John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 113-118.\ntype TrendCycleMode struct {\n\tmu                         sync.RWMutex\n\tmnemonic                   string\n\tdescription                string\n\tmnemonicTrend              string\n\tdescriptionTrend           string\n\tmnemonicCycle              string\n\tdescriptionCycle           string\n\tmnemonicITL                string\n\tdescriptionITL             string\n\tmnemonicSine               string\n\tdescriptionSine            string\n\tmnemonicSineLead           string\n\tdescriptionSineLead        string\n\tmnemonicDCP                string\n\tdescriptionDCP             string\n\tmnemonicDCPhase            string\n\tdescriptionDCPhase         string\n\tdc                         *dominantcycle.DominantCycle\n\tcyclePartMultiplier        float64\n\tseparationPercentage       float64\n\tseparationFactor           float64\n\ttrendLineSmoothingLength   int\n\tcoeff0                     float64\n\tcoeff1                     float64\n\tcoeff2                     float64\n\tcoeff3                     float64\n\ttrendline                  float64\n\ttrendAverage1              float64\n\ttrendAverage2              float64\n\ttrendAverage3              float64\n\tsinWave                    float64\n\tsinWaveLead                float64\n\tprevDcPhase                float64\n\tprevSineLeadWaveDifference float64\n\tsamplesInTrend             int\n\tisTrendMode                bool\n\tinput                      []float64\n\tinputLength                int\n\tinputLengthMin1            int\n\tprimed                     bool\n\tbarFunc                    entities.BarFunc\n\tquoteFunc                  entities.QuoteFunc\n\ttradeFunc                  entities.TradeFunc\n}\n\n// NewTrendCycleModeDefault returns an instance of the indicator\n// created using default values of the parameters.\nfunc NewTrendCycleModeDefault() (*TrendCycleMode, error) {\n\tconst (\n\t\tsmoothingLength           = 4\n\t\talphaEmaQuadratureInPhase = 0.2\n\t\talphaEmaPeriod            = 0.2\n\t\talphaEmaPeriodAdditional  = 0.33\n\t\twarmUpPeriod              = 100\n\t\ttrendLineSmoothingLength  = 4\n\t\tcyclePartMultiplier       = 1.0\n\t\tseparationPercentage      = 1.5\n\t)\n\n\treturn newTrendCycleMode(\n\t\thilberttransformer.HomodyneDiscriminator,\n\t\t&hilberttransformer.CycleEstimatorParams{\n\t\t\tSmoothingLength:           smoothingLength,\n\t\t\tAlphaEmaQuadratureInPhase: alphaEmaQuadratureInPhase,\n\t\t\tAlphaEmaPeriod:            alphaEmaPeriod,\n\t\t\tWarmUpPeriod:              warmUpPeriod,\n\t\t},\n\t\talphaEmaPeriodAdditional,\n\t\ttrendLineSmoothingLength,\n\t\tcyclePartMultiplier,\n\t\tseparationPercentage,\n\t\t0, 0, 0)\n}\n\n// NewTrendCycleModeParams returns an instance of the indicator created using supplied parameters.\nfunc NewTrendCycleModeParams(p *Params) (*TrendCycleMode, error) {\n\treturn newTrendCycleMode(\n\t\tp.EstimatorType, &p.EstimatorParams,\n\t\tp.AlphaEmaPeriodAdditional,\n\t\tp.TrendLineSmoothingLength,\n\t\tp.CyclePartMultiplier,\n\t\tp.SeparationPercentage,\n\t\tp.BarComponent, p.QuoteComponent, p.TradeComponent)\n}\n\n//nolint:funlen,cyclop\nfunc newTrendCycleMode(\n\testimatorType hilberttransformer.CycleEstimatorType,\n\testimatorParams *hilberttransformer.CycleEstimatorParams,\n\talphaEmaPeriodAdditional float64,\n\ttrendLineSmoothingLength int,\n\tcyclePartMultiplier float64,\n\tseparationPercentage float64,\n\tbc entities.BarComponent, qc entities.QuoteComponent, tc entities.TradeComponent,\n) (*TrendCycleMode, error) {\n\tconst (\n\t\tinvalid       = \"invalid trend cycle mode parameters\"\n\t\tfmta          = \"%s: α for additional smoothing should be in range (0, 1]\"\n\t\tfmttlsl       = \"%s: trend line smoothing length should be 2, 3, or 4\"\n\t\tfmtcpm        = \"%s: cycle part multiplier should be in range (0, 10]\"\n\t\tfmtsep        = \"%s: separation percentage should be in range (0, 100]\"\n\t\tfmtw          = \"%s: %w\"\n\t\tfmtnVal       = \"tcm(%.3f, %d, %.3f, %.3f%%%s%s)\"\n\t\tfmtnTrend     = \"tcm-trend(%.3f, %d, %.3f, %.3f%%%s%s)\"\n\t\tfmtnCycle     = \"tcm-cycle(%.3f, %d, %.3f, %.3f%%%s%s)\"\n\t\tfmtnITL       = \"tcm-itl(%.3f, %d, %.3f, %.3f%%%s%s)\"\n\t\tfmtnSine      = \"tcm-sine(%.3f, %d, %.3f, %.3f%%%s%s)\"\n\t\tfmtnSineLead  = \"tcm-sineLead(%.3f, %d, %.3f, %.3f%%%s%s)\"\n\t\tfmtnDCP       = \"dcp(%.3f%s%s)\"\n\t\tfmtnDCPha     = \"dcph(%.3f%s%s)\"\n\t\tfour          = 4\n\t\talpha         = 0.2\n\t\tdescrVal      = \"Trend versus cycle mode \"\n\t\tdescrTrend    = \"Trend versus cycle mode, is-trend flag \"\n\t\tdescrCycle    = \"Trend versus cycle mode, is-cycle flag \"\n\t\tdescrITL      = \"Trend versus cycle mode instantaneous trend line \"\n\t\tdescrSine     = \"Trend versus cycle mode sine wave \"\n\t\tdescrSineLead = \"Trend versus cycle mode sine wave lead \"\n\t\tdescrDCP      = \"Dominant cycle period \"\n\t\tdescrDCPha    = \"Dominant cycle phase \"\n\t\ttlslTwo       = 2\n\t\ttlslThree     = 3\n\t\ttlslFour      = 4\n\t\tcpmMax        = 10.0\n\t\tsepMax        = 100.0\n\t\tsepDenom      = 100.0\n\t)\n\n\tif alphaEmaPeriodAdditional <= 0. || alphaEmaPeriodAdditional > 1. {\n\t\treturn nil, fmt.Errorf(fmta, invalid)\n\t}\n\n\tif trendLineSmoothingLength < tlslTwo || trendLineSmoothingLength > tlslFour {\n\t\treturn nil, fmt.Errorf(fmttlsl, invalid)\n\t}\n\n\tif cyclePartMultiplier <= 0. || cyclePartMultiplier > cpmMax {\n\t\treturn nil, fmt.Errorf(fmtcpm, invalid)\n\t}\n\n\tif separationPercentage <= 0. || separationPercentage > sepMax {\n\t\treturn nil, fmt.Errorf(fmtsep, invalid)\n\t}\n\n\t// SineWave-style default: BarMedianPrice always shown in mnemonic.\n\tif bc == 0 {\n\t\tbc = entities.BarMedianPrice\n\t}\n\n\tif qc == 0 {\n\t\tqc = entities.DefaultQuoteComponent\n\t}\n\n\tif tc == 0 {\n\t\ttc = entities.DefaultTradeComponent\n\t}\n\n\t// Build the inner DominantCycle with explicit components.\n\tdcParams := &dominantcycle.Params{\n\t\tEstimatorType:            estimatorType,\n\t\tEstimatorParams:          *estimatorParams,\n\t\tAlphaEmaPeriodAdditional: alphaEmaPeriodAdditional,\n\t\tBarComponent:             bc,\n\t\tQuoteComponent:           qc,\n\t\tTradeComponent:           tc,\n\t}\n\n\tdc, err := dominantcycle.NewDominantCycleParams(dcParams)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\t// Compose the estimator moniker (same logic as DominantCycle / HTITL).\n\testimator, err := hilberttransformer.NewCycleEstimator(estimatorType, estimatorParams)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\testimatorMoniker := \"\"\n\tif estimatorType != hilberttransformer.HomodyneDiscriminator ||\n\t\testimatorParams.SmoothingLength != four ||\n\t\testimatorParams.AlphaEmaQuadratureInPhase != alpha ||\n\t\testimatorParams.AlphaEmaPeriod != alpha {\n\t\testimatorMoniker = hilberttransformer.EstimatorMoniker(estimatorType, estimator)\n\t\tif len(estimatorMoniker) > 0 {\n\t\t\testimatorMoniker = \", \" + estimatorMoniker\n\t\t}\n\t}\n\n\tcomponentMnemonic := core.ComponentTripleMnemonic(bc, qc, tc)\n\n\tfmtArgs := []any{\n\t\talphaEmaPeriodAdditional, trendLineSmoothingLength,\n\t\tcyclePartMultiplier, separationPercentage,\n\t\testimatorMoniker, componentMnemonic,\n\t}\n\n\tmnemonicVal := fmt.Sprintf(fmtnVal, fmtArgs...)\n\tmnemonicTrend := fmt.Sprintf(fmtnTrend, fmtArgs...)\n\tmnemonicCycle := fmt.Sprintf(fmtnCycle, fmtArgs...)\n\tmnemonicITL := fmt.Sprintf(fmtnITL, fmtArgs...)\n\tmnemonicSine := fmt.Sprintf(fmtnSine, fmtArgs...)\n\tmnemonicSineLead := fmt.Sprintf(fmtnSineLead, fmtArgs...)\n\tmnemonicDCP := fmt.Sprintf(fmtnDCP, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)\n\tmnemonicDCPha := fmt.Sprintf(fmtnDCPha, alphaEmaPeriodAdditional, estimatorMoniker, componentMnemonic)\n\n\tbarFunc, err := entities.BarComponentFunc(bc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\tquoteFunc, err := entities.QuoteComponentFunc(qc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\ttradeFunc, err := entities.TradeComponentFunc(tc)\n\tif err != nil {\n\t\treturn nil, fmt.Errorf(fmtw, invalid, err)\n\t}\n\n\tvar c0, c1, c2, c3 float64\n\n\tswitch trendLineSmoothingLength {\n\tcase tlslTwo:\n\t\tc0, c1 = 2.0/3.0, 1.0/3.0\n\tcase tlslThree:\n\t\tc0, c1, c2 = 3.0/6.0, 2.0/6.0, 1.0/6.0\n\tdefault: // tlslFour\n\t\tc0, c1, c2, c3 = 4.0/10.0, 3.0/10.0, 2.0/10.0, 1.0/10.0\n\t}\n\n\tmaxPeriod := dc.MaxPeriod()\n\tnan := math.NaN()\n\n\treturn &TrendCycleMode{\n\t\tmnemonic:                 mnemonicVal,\n\t\tdescription:              descrVal + mnemonicVal,\n\t\tmnemonicTrend:            mnemonicTrend,\n\t\tdescriptionTrend:         descrTrend + mnemonicTrend,\n\t\tmnemonicCycle:            mnemonicCycle,\n\t\tdescriptionCycle:         descrCycle + mnemonicCycle,\n\t\tmnemonicITL:              mnemonicITL,\n\t\tdescriptionITL:           descrITL + mnemonicITL,\n\t\tmnemonicSine:             mnemonicSine,\n\t\tdescriptionSine:          descrSine + mnemonicSine,\n\t\tmnemonicSineLead:         mnemonicSineLead,\n\t\tdescriptionSineLead:      descrSineLead + mnemonicSineLead,\n\t\tmnemonicDCP:              mnemonicDCP,\n\t\tdescriptionDCP:           descrDCP + mnemonicDCP,\n\t\tmnemonicDCPhase:          mnemonicDCPha,\n\t\tdescriptionDCPhase:       descrDCPha + mnemonicDCPha,\n\t\tdc:                       dc,\n\t\tcyclePartMultiplier:      cyclePartMultiplier,\n\t\tseparationPercentage:     separationPercentage,\n\t\tseparationFactor:         separationPercentage / sepDenom,\n\t\ttrendLineSmoothingLength: trendLineSmoothingLength,\n\t\tcoeff0:                   c0,\n\t\tcoeff1:                   c1,\n\t\tcoeff2:                   c2,\n\t\tcoeff3:                   c3,\n\t\ttrendline:                nan,\n\t\tsinWave:                  nan,\n\t\tsinWaveLead:              nan,\n\t\tisTrendMode:              true,\n\t\tinput:                    make([]float64, maxPeriod),\n\t\tinputLength:              maxPeriod,\n\t\tinputLengthMin1:          maxPeriod - 1,\n\t\tbarFunc:                  barFunc,\n\t\tquoteFunc:                quoteFunc,\n\t\ttradeFunc:                tradeFunc,\n\t}, nil\n}\n\n// IsPrimed indicates whether an indicator is primed.\nfunc (s *TrendCycleMode) IsPrimed() bool {\n\ts.mu.RLock()\n\tdefer s.mu.RUnlock()\n\n\treturn s.primed\n}\n\n// Metadata describes an output data of the indicator.\nfunc (s *TrendCycleMode) Metadata() core.Metadata {\n\treturn core.Metadata{\n\t\tType:        core.TrendCycleMode,\n\t\tMnemonic:    s.mnemonic,\n\t\tDescription: s.description,\n\t\tOutputs: []outputs.Metadata{\n\t\t\t{Kind: int(Value), Type: outputs.ScalarType, Mnemonic: s.mnemonic, Description: s.description},\n\t\t\t{Kind: int(IsTrendMode), Type: outputs.ScalarType, Mnemonic: s.mnemonicTrend, Description: s.descriptionTrend},\n\t\t\t{Kind: int(IsCycleMode), Type: outputs.ScalarType, Mnemonic: s.mnemonicCycle, Description: s.descriptionCycle},\n\t\t\t{Kind: int(InstantaneousTrendLine), Type: outputs.ScalarType, Mnemonic: s.mnemonicITL, Description: s.descriptionITL},\n\t\t\t{Kind: int(SineWave), Type: outputs.ScalarType, Mnemonic: s.mnemonicSine, Description: s.descriptionSine},\n\t\t\t{Kind: int(SineWaveLead), Type: outputs.ScalarType, Mnemonic: s.mnemonicSineLead, Description: s.descriptionSineLead},\n\t\t\t{Kind: int(DominantCyclePeriod), Type: outputs.ScalarType, Mnemonic: s.mnemonicDCP, Description: s.descriptionDCP},\n\t\t\t{Kind: int(DominantCyclePhase), Type: outputs.ScalarType, Mnemonic: s.mnemonicDCPhase, Description: s.descriptionDCPhase},\n\t\t},\n\t}\n}\n\n// Update updates the value of the indicator given the next sample, returning the\n// (value, isTrendMode, isCycleMode, trendline, sineWave, sineWaveLead, period, phase) tuple.\n// The isTrendMode / isCycleMode values are encoded as 1 / 0 scalars. Returns NaN for all\n// outputs if the indicator is not yet primed.\n//\n//nolint:funlen,cyclop,gocognit\nfunc (s *TrendCycleMode) Update(sample float64) (float64, float64, float64, float64, float64, float64, float64, float64) {\n\tif math.IsNaN(sample) {\n\t\treturn sample, sample, sample, sample, sample, sample, sample, sample\n\t}\n\n\t// Delegate to the inner DominantCycle. Its own mutex is independent.\n\t_, period, phase := s.dc.Update(sample)\n\t// SmoothedPrice tracks the WMA smoothed price inside the Hilbert transformer.\n\tsmoothedPrice := s.dc.SmoothedPrice()\n\n\ts.mu.Lock()\n\tdefer s.mu.Unlock()\n\n\ts.pushInput(sample)\n\n\tif s.primed {\n\t\tsmoothedPeriod := period\n\t\taverage := s.calculateTrendAverage(smoothedPeriod)\n\t\ts.trendline = s.coeff0*average + s.coeff1*s.trendAverage1 +\n\t\t\ts.coeff2*s.trendAverage2 + s.coeff3*s.trendAverage3\n\t\ts.trendAverage3 = s.trendAverage2\n\t\ts.trendAverage2 = s.trendAverage1\n\t\ts.trendAverage1 = average\n\n\t\tdiff := s.calculateSineLeadWaveDifference(phase)\n\n\t\t// Condition 1: a cycle mode exists for the half-period of a dominant cycle\n\t\t// after the SineWave vs SineWaveLead crossing.\n\t\ts.isTrendMode = true\n\n\t\tif (diff > 0 && s.prevSineLeadWaveDifference < 0) ||\n\t\t\t(diff < 0 && s.prevSineLeadWaveDifference > 0) {\n\t\t\ts.isTrendMode = false\n\t\t\ts.samplesInTrend = 0\n\t\t}\n\n\t\ts.prevSineLeadWaveDifference = diff\n\t\ts.samplesInTrend++\n\n\t\tconst half = 0.5\n\t\tif float64(s.samplesInTrend) < half*smoothedPeriod {\n\t\t\ts.isTrendMode = false\n\t\t}\n\n\t\t// Condition 2: cycle mode if the measured phase rate of change is more than 2/3\n\t\t// the phase rate of change of the dominant cycle (360/period) and less than 1.5 times it.\n\t\tphaseDelta := phase - s.prevDcPhase\n\t\ts.prevDcPhase = phase\n\n\t\tif math.Abs(smoothedPeriod) > epsilon {\n\t\t\tconst (\n\t\t\t\tminFactor = 2.0 / 3.0\n\t\t\t\tmaxFactor = 1.5\n\t\t\t\tfullCycle = 360.0\n\t\t\t)\n\n\t\t\tdcRate := fullCycle / smoothedPeriod\n\t\t\tif phaseDelta > minFactor*dcRate && phaseDelta < maxFactor*dcRate {\n\t\t\t\ts.isTrendMode = false\n\t\t\t}\n\t\t}\n\n\t\t// Condition 3: if the WMA smoothed price is separated by more than the separation\n\t\t// percentage from the instantaneous trend line, force the trend mode.\n\t\tif math.Abs(s.trendline) > epsilon &&\n\t\t\tmath.Abs((smoothedPrice-s.trendline)/s.trendline) >= s.separationFactor {\n\t\t\ts.isTrendMode = true\n\t\t}\n\n\t\treturn s.mode(), s.isTrendFloat(), s.isCycleFloat(), s.trendline, s.sinWave, s.sinWaveLead, period, phase\n\t}\n\n\tif s.dc.IsPrimed() {\n\t\ts.primed = true\n\t\tsmoothedPeriod := period\n\t\ts.trendline = s.calculateTrendAverage(smoothedPeriod)\n\t\ts.trendAverage1 = s.trendline\n\t\ts.trendAverage2 = s.trendline\n\t\ts.trendAverage3 = s.trendline\n\n\t\ts.prevDcPhase = phase\n\t\ts.prevSineLeadWaveDifference = s.calculateSineLeadWaveDifference(phase)\n\n\t\ts.isTrendMode = true\n\t\ts.samplesInTrend++\n\n\t\tconst half = 0.5\n\t\tif float64(s.samplesInTrend) < half*smoothedPeriod {\n\t\t\ts.isTrendMode = false\n\t\t}\n\n\t\treturn s.mode(), s.isTrendFloat(), s.isCycleFloat(), s.trendline, s.sinWave, s.sinWaveLead, period, phase\n\t}\n\n\tnan := math.NaN()\n\n\treturn nan, nan, nan, nan, nan, nan, nan, nan\n}\n\n// UpdateScalar updates the indicator given the next scalar sample.\nfunc (s *TrendCycleMode) UpdateScalar(sample *entities.Scalar) core.Output {\n\treturn s.updateEntity(sample.Time, sample.Value)\n}\n\n// UpdateBar updates the indicator given the next bar sample.\nfunc (s *TrendCycleMode) UpdateBar(sample *entities.Bar) core.Output {\n\treturn s.updateEntity(sample.Time, s.barFunc(sample))\n}\n\n// UpdateQuote updates the indicator given the next quote sample.\nfunc (s *TrendCycleMode) UpdateQuote(sample *entities.Quote) core.Output {\n\treturn s.updateEntity(sample.Time, s.quoteFunc(sample))\n}\n\n// UpdateTrade updates the indicator given the next trade sample.\nfunc (s *TrendCycleMode) UpdateTrade(sample *entities.Trade) core.Output {\n\treturn s.updateEntity(sample.Time, s.tradeFunc(sample))\n}\n\nfunc (s *TrendCycleMode) updateEntity(t time.Time, sample float64) core.Output {\n\tconst length = 8\n\n\tvalue, trend, cycle, itl, sine, sineLead, period, phase := s.Update(sample)\n\n\toutput := make([]any, length)\n\ti := 0\n\toutput[i] = entities.Scalar{Time: t, Value: value}\n\ti++\n\toutput[i] = entities.Scalar{Time: t, Value: trend}\n\ti++\n\toutput[i] = entities.Scalar{Time: t, Value: cycle}\n\ti++\n\toutput[i] = entities.Scalar{Time: t, Value: itl}\n\ti++\n\toutput[i] = entities.Scalar{Time: t, Value: sine}\n\ti++\n\toutput[i] = entities.Scalar{Time: t, Value: sineLead}\n\ti++\n\toutput[i] = entities.Scalar{Time: t, Value: period}\n\ti++\n\toutput[i] = entities.Scalar{Time: t, Value: phase}\n\n\treturn output\n}\n\n// epsilon is the floating-point equality threshold (matches C#'s double.Epsilon behaviour for zero checks).\nconst epsilon = 1e-308\n\nfunc (s *TrendCycleMode) pushInput(value float64) {\n\tcopy(s.input[1:], s.input[:s.inputLengthMin1])\n\ts.input[0] = value\n}\n\nfunc (s *TrendCycleMode) calculateTrendAverage(smoothedPeriod float64) float64 {\n\tconst half = 0.5\n\n\tlength := int(math.Floor(smoothedPeriod*s.cyclePartMultiplier + half))\n\tif length > s.inputLength {\n\t\tlength = s.inputLength\n\t} else if length < 1 {\n\t\tlength = 1\n\t}\n\n\tvar sum float64\n\tfor i := 0; i < length; i++ {\n\t\tsum += s.input[i]\n\t}\n\n\treturn sum / float64(length)\n}\n\nfunc (s *TrendCycleMode) calculateSineLeadWaveDifference(phase float64) float64 {\n\tconst leadOffset = 45.0\n\n\tp := phase * deg2Rad\n\ts.sinWave = math.Sin(p)\n\ts.sinWaveLead = math.Sin(p + leadOffset*deg2Rad)\n\n\treturn s.sinWave - s.sinWaveLead\n}\n\nfunc (s *TrendCycleMode) mode() float64 {\n\tif s.isTrendMode {\n\t\treturn 1.0\n\t}\n\n\treturn -1.0\n}\n\nfunc (s *TrendCycleMode) isTrendFloat() float64 {\n\tif s.isTrendMode {\n\t\treturn 1.0\n\t}\n\n\treturn 0.0\n}\n\nfunc (s *TrendCycleMode) isCycleFloat() float64 {\n\tif s.isTrendMode {\n\t\treturn 0.0\n\t}\n\n\treturn 1.0\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode.go"
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

## Assistant (Compaction · claude-opus-4.7 · 84.7s)

## Goal

Port MBST's **TrendCycleMode (TCM)** indicator from `mbst-to-convert/john-ehlers/trend-cycle-mode/` into zpano:
- Go: `go/indicators/johnehlers/trendcyclemode/`
- TS: `ts/indicators/john-ehlers/trend-cycle-mode/`

Wraps existing `DominantCycle` (for period + phase + smoothed price) and adds its own raw-sample buffer for trendline, sine/sineLead computation, and trend-vs-cycle mode logic.

## Instructions

### Outputs (user-specified order)
1. **Value** — TCM value (+1 trend / −1 cycle), Scalar
2. **IsTrendMode** — 0/1 Scalar
3. **IsCycleMode** — 0/1 Scalar (= 1 − IsTrendMode; user explicitly requested)
4. **InstantaneousTrendLine** — Scalar
5. **SineWave** — Scalar
6. **SineWaveLead** — Scalar
7. **DominantCyclePeriod** — Scalar
8. **DominantCyclePhase** — Scalar

(Drop `SmoothedPrice` output — only used internally for condition 3.)

### Combined single test file
Merge MBST `TrendCycleModeTest.cs` 6 reference arrays (rawInput, dcPeriod, dcPhase, sine, sineLead, itl, tcm) into one Go + one TS test file. skip=9, settleSkip=177 for numeric refs, tol 1e-4. For tcm value array: skip indices 70 and 71 (MBST known mismatches).

### MBST params / defaults
- `alphaEmaPeriodAdditional=0.33`, `trendLineSmoothingLength=4` ∈{2,3,4}, `cyclePartMultiplier=1.0` ∈(0,10], `separationPercentage=1.5` ∈(0,100].
- Default estimator: HomodyneDiscriminator, smoothingLength=4, αq=αp=0.2, **warmUpPeriod=100**.
- Default BarComponent: **BarMedianPrice** (always shown in mnemonic, SineWave-style).
- MAMA-style constructors: Go `NewTrendCycleModeDefault` / `NewTrendCycleModeParams` → private `newTrendCycleMode`; TS `default()` / `fromParams()`.
- Implement `core.Indicator` directly (multi-output); use `sync.RWMutex` in Go.
- Mnemonic format chosen: `tcm(α, tlsl, cpMul, sep%, <estimator>, <component>)` using `%.3f` for α, cpMul, sep and `%d` for tlsl. All 6 "tcm-*" outputs share the same suffix; DCP/DCPhase reuse `dcp(...)`, `dcph(...)` pattern.

### Key design decision
Expose internals on wrapped `DominantCycle` rather than duplicate phase logic from HTCE:
- Go: `SmoothedPrice() float64` (NaN if not primed) and `MaxPeriod() int`
- TS: `smoothedPrice` and `maxPeriod` getters

Document this pattern ("expose wrapper internals rather than duplicating") in `mbst-indicator-conversion` skill.

### Registration
- `go/indicators/core/type.go` + `type_test.go`: add `TrendCycleMode` enum constant + string + switch cases. ✅
- `ts/indicators/core/indicator-type.ts`: add `TrendCycleMode` member. ✅

### Test commands
- Go: `cd /home/dev/zpano/go && go test ./indicators/johnehlers/trendcyclemode/...`
- TS: from `/home/dev/zpano/ts`: `node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=TrendCycleMode`

## Discoveries

- MBST `dcp.RawValue` is the same `smoothedPhase` but returned unconditionally — behaviorally identical when `primed=true` to zpano `DominantCycle.Phase`. **No duplicate phase logic needed**.
- MBST TCM pushes **raw samples** (not htce.Smoothed) to its input buffer for `CalculateTrendAverage`. TCM needs its own raw-sample buffer alongside the wrapped DominantCycle.
- MBST TCM `Update` flow:
  1. `dcp.Update(sample)`; push raw `sample` to local input buffer.
  2. If primed: `avg = mean(input[0..length-1])` where `length=clamp(floor(smoothedPeriod·cpMul+0.5),[1,maxPeriod])`; `trendline = c0·avg + c1·avg1 + c2·avg2 + c3·avg3`; shift avgs.
  3. `phase = dcp.RawValue`; `sinWave=sin(phase·d2r)`, `sinWaveLead=sin((phase+45)·d2r)`; `diff=sinWave−sinWaveLead`.
  4. Default `isTrendMode=true`, then three conditions:
     - **C1 (cycle)**: sineLead-diff crossing zero → `samplesInTrend=0`; if `++samplesInTrend < 0.5·period` → cycle.
     - **C2 (cycle)**: if `|period|>ε` and `(phase−prevPhase) ∈ (2/3·360/period, 1.5·360/period)` → cycle.
     - **C3 (trend override)**: if `|trendline|>ε` and `|(htce.Smoothed − trendline)/trendline| ≥ separationFactor` → trend.
  5. On initial prime: seed `trendline=avg=avg1=avg2=avg3`, `prevDcPhase=phase`, `prevSineLeadDiff=diff`, increment samplesInTrend, apply C1-halfPeriod rule only.
- Coefficient table by tlsl: 2→(2/3,1/3); 3→(3/6,2/6,1/6); 4→(4/10,3/10,2/10,1/10).
- `separationFactor = separationPercentage / 100`.
- MBST tcm array: first 63 NaN, then ±1 values. Skip indices 70, 71.
- HTITL is closest algorithmic template; SineWave is closest structural template.
- TS `DominantCycle` default maxPeriod is **50** (HomodyneDiscriminator smoothingLength=4; `defaultMaxPeriod=50` in `hilbert-transformer-common.ts`).

## Accomplished

### Done
1. ✅ Added Go `SmoothedPrice()` and `MaxPeriod()` methods on `DominantCycle` + tests. `go test ./indicators/johnehlers/dominantcycle/...` passes.
2. ✅ Added TS `smoothedPrice` and `maxPeriod` getters on `DominantCycle` + tests. 16 specs, 0 failures.
3. ✅ Registered `TrendCycleMode` enum in Go `core/type.go` + `type_test.go`. Tests pass.
4. ✅ Registered `TrendCycleMode` enum in TS `core/indicator-type.ts`.
5. ✅ Read MBST `TrendCycleMode.cs` (389 lines) and `TrendCycleModeTest.cs` (619 lines) fully; all 6 reference arrays available in context.
6. ✅ Created Go TCM files (NOT YET COMPILED / TESTED):
   - `trendcyclemodeoutput.go` (8 outputs enum + String + IsKnown + Marshal/UnmarshalJSON)
   - `trendcyclemodeparams.go` (Params struct with SeparationPercentage)
   - `trendcyclemode.go` (main file; wraps `*dominantcycle.DominantCycle`; own raw `input[]` buffer; coeff c0..c3; full state; 8-tuple `Update`; returns bool-encoded 0/1 floats for IsTrendMode/IsCycleMode via `isTrendFloat()`/`isCycleFloat()` helpers; `mode()` helper returns ±1; uses `epsilon = 1e-308`)

### In progress / Todo
- ⏳ Create Go `trendcyclemodeoutput_test.go` (mirror HTITL pattern with 8 entries).
- ⏳ Create Go `trendcyclemode_test.go` — combined reference test with all 6 MBST arrays (rawInput, dcPeriod, dcPhase, sine, sineLead, itl, tcm) + usual sub-tests (IsPrimed, Metadata, UpdateEntity, NewTrendCycleMode parameter validation).
- ⏳ Run `cd /home/dev/zpano/go && go test ./indicators/johnehlers/trendcyclemode/...` and fix any issues.
- ⏳ Mirror all of the above in TS:
  - `ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-params.ts`
  - `ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-output.ts`
  - `ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.ts`
  - `ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts`
- ⏳ Update `.opencode/skills/mbst-indicator-conversion/SKILL.md` (describe "expose wrapper internals" pattern).

## Relevant files / directories

### Created (Go) — pending test
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput.go`
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeparams.go`
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode.go`

### Edited (Go)
- `/home/dev/zpano/go/indicators/johnehlers/dominantcycle/dominantcycle.go` — added `SmoothedPrice()` + `MaxPeriod()` methods
- `/home/dev/zpano/go/indicators/johnehlers/dominantcycle/dominantcycle_test.go` — added 2 tests
- `/home/dev/zpano/go/indicators/core/type.go` — added `TrendCycleMode` enum
- `/home/dev/zpano/go/indicators/core/type_test.go` — added at 3 test tables

### Edited (TS)
- `/home/dev/zpano/ts/indicators/john-ehlers/dominant-cycle/dominant-cycle.ts` — added `smoothedPrice` + `maxPeriod` getters
- `/home/dev/zpano/ts/indicators/john-ehlers/dominant-cycle/dominant-cycle.spec.ts` — added 2 specs
- `/home/dev/zpano/ts/indicators/core/indicator-type.ts` — appended `TrendCycleMode`

### To create (Go)
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput_test.go`
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go`

### To create (TS)
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-params.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-output.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts`

### Reference templates (already read, Go) — use these as patterns
- `/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/` (all 5 files, closest algorithmic template; test structure is ideal reference)
- `/home/dev/zpano/go/indicators/johnehlers/sinewave/sinewave.go` (closest structural template; wraps DominantCycle)
- `/home/dev/zpano/go/indicators/johnehlers/dominantcycle/dominantcycle.go` (wrapped indicator)
- `/home/dev/zpano/go/indicators/core/indicator.go` (`core.Indicator` interface definition)

### MBST source (already read, extracted into context)
- `/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleMode.cs` (389 lines)
- `/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleModeTest.cs` (619 lines, all 6 reference arrays)

### Skills
- `.opencode/skills/mbst-indicator-conversion/SKILL.md` (loaded; needs post-completion update)
- `.opencode/skills/indicator-conversion/SKILL.md`, `.opencode/skills/indicator-architecture/SKILL.md`

### Implementation notes for Go main file (`trendcyclemode.go`)
- Uses `Update(sample float64) (float64, float64, float64, float64, float64, float64, float64, float64)` returning `(value, isTrend01, isCycle01, trendline, sinWave, sinWaveLead, period, phase)`.
- Prime logic mirrors MBST exactly including applying the `samplesInTrend < 0.5·smoothedPeriod` check on first prime (sets isTrendMode=false initially).
- Mnemonic sub-outputs: `tcm`, `tcm-trend`, `tcm-cycle`, `tcm-itl`, `tcm-sine`, `tcm-sineLead`, `dcp`, `dcph`.
- `epsilon = 1e-308` used for zero comparisons (matches C# `double.Epsilon` semantic).

### Immediate next action
1. Create `trendcyclemodeoutput_test.go` mirroring HTITL's pattern for 8 outputs.
2. Create `trendcyclemode_test.go` — combine rawInput + 6 reference arrays; test dcPeriod/dcPhase/sine/sineLead/itl with tol=1e-4 skip=9 settleSkip=177; test tcm value as exact ±1 match skipping indices 70/71 AND any MBST NaN positions; plus IsPrimed, Metadata, UpdateEntity, NewTrendCycleMode parameter-validation sub-tests.
3. Run `go test ./indicators/johnehlers/trendcyclemode/...` and iterate on any discrepancies.
4. Mirror in TS.
5. Update skill doc.

---

## User

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

_Thinking:_

 Let me check what I've built so far with these Go files.

**Tool: bash**

**Input:**
```json
{
  "command": "ls /home/dev/zpano/go/indicators/johnehlers/trendcyclemode/ 2>&1",
  "description": "Check Go TCM dir state"
}
```

**Output:**
```
trendcyclemode.go
trendcyclemodeoutput.go
trendcyclemodeparams.go

```

---

## Assistant (Build · claude-opus-4.7 · 3.4s)

**Tool: bash**

**Input:**
```json
{
  "command": "cd /home/dev/zpano/go && go build ./indicators/johnehlers/trendcyclemode/... 2>&1",
  "description": "Compile Go TCM package"
}
```

---

## Assistant (Build · claude-opus-4.7 · 9.7s)

_Thinking:_

 Now I'm checking the main file and the HTML test file to understand what test cases I need to create.

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

## Assistant (Build · claude-opus-4.7 · 3.9s)

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

## Assistant (Build · claude-opus-4.7 · 5.5s)

**Tool: bash**

**Input:**
```json
{
  "command": "ls /home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/",
  "description": "List HTITL dir"
}
```

**Output:**
```
hilberttransformerinstantaneoustrendline.go
hilberttransformerinstantaneoustrendline_test.go
hilberttransformerinstantaneoustrendlineoutput.go
hilberttransformerinstantaneoustrendlineoutput_test.go
hilberttransformerinstantaneoustrendlineparams.go

```

---

## Assistant (Build · claude-opus-4.7 · 3.4s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendlineoutput_test.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendlineoutput_test.go</path>
<type>file</type>
<content>1: //nolint:testpackage
2: package hilberttransformerinstantaneoustrendline
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
16: 		{DominantCyclePeriod, dominantCyclePeriodStr},
17: 		{outputLast, unknownStr},
18: 		{Output(0), unknownStr},
19: 		{Output(9999), unknownStr},
20: 		{Output(-9999), unknownStr},
21: 	}
22: 
23: 	for _, tt := range tests {
24: 		exp := tt.text
25: 		act := tt.o.String()
26: 
27: 		if exp != act {
28: 			t.Errorf("'%v'.String(): expected '%v', actual '%v'", tt.o, exp, act)
29: 		}
30: 	}
31: }
32: 
33: func TestOutputIsKnown(t *testing.T) {
34: 	t.Parallel()
35: 
36: 	tests := []struct {
37: 		o       Output
38: 		boolean bool
39: 	}{
40: 		{Value, true},
41: 		{DominantCyclePeriod, true},
42: 		{outputLast, false},
43: 		{Output(0), false},
44: 		{Output(9999), false},
45: 		{Output(-9999), false},
46: 	}
47: 
48: 	for _, tt := range tests {
49: 		exp := tt.boolean
50: 		act := tt.o.IsKnown()
51: 
52: 		if exp != act {
53: 			t.Errorf("'%v'.IsKnown(): expected '%v', actual '%v'", tt.o, exp, act)
54: 		}
55: 	}
56: }
57: 
58: func TestOutputMarshalJSON(t *testing.T) {
59: 	t.Parallel()
60: 
61: 	const dqs = "\""
62: 
63: 	var nilstr string
64: 	tests := []struct {
65: 		o         Output
66: 		json      string
67: 		succeeded bool
68: 	}{
69: 		{Value, dqs + valueStr + dqs, true},
70: 		{DominantCyclePeriod, dqs + dominantCyclePeriodStr + dqs, true},
71: 		{outputLast, nilstr, false},
72: 		{Output(9999), nilstr, false},
73: 		{Output(-9999), nilstr, false},
74: 		{Output(0), nilstr, false},
75: 	}
76: 
77: 	for _, tt := range tests {
78: 		exp := tt.json
79: 		bs, err := tt.o.MarshalJSON()
80: 
81: 		if err != nil && tt.succeeded {
82: 			t.Errorf("'%v'.MarshalJSON(): expected success '%v', got error %v", tt.o, exp, err)
83: 
84: 			continue
85: 		}
86: 
87: 		if err == nil && !tt.succeeded {
88: 			t.Errorf("'%v'.MarshalJSON(): expected error, got success", tt.o)
89: 
90: 			continue
91: 		}
92: 
93: 		act := string(bs)
94: 		if exp != act {
95: 			t.Errorf("'%v'.MarshalJSON(): expected '%v', actual '%v'", tt.o, exp, act)
96: 		}
97: 	}
98: }
99: 
100: func TestOutputUnmarshalJSON(t *testing.T) {
101: 	t.Parallel()
102: 
103: 	const dqs = "\""
104: 
105: 	var zero Output
106: 	tests := []struct {
107: 		o         Output
108: 		json      string
109: 		succeeded bool
110: 	}{
111: 		{Value, dqs + valueStr + dqs, true},
112: 		{DominantCyclePeriod, dqs + dominantCyclePeriodStr + dqs, true},
113: 		{zero, dqs + unknownStr + dqs, false},
114: 		{zero, dqs + "foobar" + dqs, false},
115: 	}
116: 
117: 	for _, tt := range tests {
118: 		exp := tt.o
119: 		bs := []byte(tt.json)
120: 
121: 		var o Output
122: 
123: 		err := o.UnmarshalJSON(bs)
124: 		if err != nil && tt.succeeded {
125: 			t.Errorf("UnmarshalJSON('%v'): expected success '%v', got error %v", tt.json, exp, err)
126: 
127: 			continue
128: 		}
129: 
130: 		if err == nil && !tt.succeeded {
131: 			t.Errorf("MarshalJSON('%v'): expected error, got success", tt.json)
132: 
133: 			continue
134: 		}
135: 
136: 		if exp != o {
137: 			t.Errorf("MarshalJSON('%v'): expected '%v', actual '%v'", tt.json, exp, o)
138: 		}
139: 	}
140: }

(End of file - total 140 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 16.5s)

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput_test.go",
  "content": "//nolint:testpackage\npackage trendcyclemode\n\nimport (\n\t\"testing\"\n)\n\nfunc TestOutputString(t *testing.T) {\n\tt.Parallel()\n\n\ttests := []struct {\n\t\to    Output\n\t\ttext string\n\t}{\n\t\t{Value, valueStr},\n\t\t{IsTrendMode, isTrendModeStr},\n\t\t{IsCycleMode, isCycleModeStr},\n\t\t{InstantaneousTrendLine, instantaneousTrendLineStr},\n\t\t{SineWave, sineWaveStr},\n\t\t{SineWaveLead, sineWaveLeadStr},\n\t\t{DominantCyclePeriod, dominantCyclePeriodStr},\n\t\t{DominantCyclePhase, dominantCyclePhaseStr},\n\t\t{outputLast, unknownStr},\n\t\t{Output(0), unknownStr},\n\t\t{Output(9999), unknownStr},\n\t\t{Output(-9999), unknownStr},\n\t}\n\n\tfor _, tt := range tests {\n\t\texp := tt.text\n\t\tact := tt.o.String()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"'%v'.String(): expected '%v', actual '%v'\", tt.o, exp, act)\n\t\t}\n\t}\n}\n\nfunc TestOutputIsKnown(t *testing.T) {\n\tt.Parallel()\n\n\ttests := []struct {\n\t\to       Output\n\t\tboolean bool\n\t}{\n\t\t{Value, true},\n\t\t{IsTrendMode, true},\n\t\t{IsCycleMode, true},\n\t\t{InstantaneousTrendLine, true},\n\t\t{SineWave, true},\n\t\t{SineWaveLead, true},\n\t\t{DominantCyclePeriod, true},\n\t\t{DominantCyclePhase, true},\n\t\t{outputLast, false},\n\t\t{Output(0), false},\n\t\t{Output(9999), false},\n\t\t{Output(-9999), false},\n\t}\n\n\tfor _, tt := range tests {\n\t\texp := tt.boolean\n\t\tact := tt.o.IsKnown()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"'%v'.IsKnown(): expected '%v', actual '%v'\", tt.o, exp, act)\n\t\t}\n\t}\n}\n\nfunc TestOutputMarshalJSON(t *testing.T) {\n\tt.Parallel()\n\n\tconst dqs = \"\\\"\"\n\n\tvar nilstr string\n\ttests := []struct {\n\t\to         Output\n\t\tjson      string\n\t\tsucceeded bool\n\t}{\n\t\t{Value, dqs + valueStr + dqs, true},\n\t\t{IsTrendMode, dqs + isTrendModeStr + dqs, true},\n\t\t{IsCycleMode, dqs + isCycleModeStr + dqs, true},\n\t\t{InstantaneousTrendLine, dqs + instantaneousTrendLineStr + dqs, true},\n\t\t{SineWave, dqs + sineWaveStr + dqs, true},\n\t\t{SineWaveLead, dqs + sineWaveLeadStr + dqs, true},\n\t\t{DominantCyclePeriod, dqs + dominantCyclePeriodStr + dqs, true},\n\t\t{DominantCyclePhase, dqs + dominantCyclePhaseStr + dqs, true},\n\t\t{outputLast, nilstr, false},\n\t\t{Output(9999), nilstr, false},\n\t\t{Output(-9999), nilstr, false},\n\t\t{Output(0), nilstr, false},\n\t}\n\n\tfor _, tt := range tests {\n\t\texp := tt.json\n\t\tbs, err := tt.o.MarshalJSON()\n\n\t\tif err != nil && tt.succeeded {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected success '%v', got error %v\", tt.o, exp, err)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif err == nil && !tt.succeeded {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected error, got success\", tt.o)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tact := string(bs)\n\t\tif exp != act {\n\t\t\tt.Errorf(\"'%v'.MarshalJSON(): expected '%v', actual '%v'\", tt.o, exp, act)\n\t\t}\n\t}\n}\n\nfunc TestOutputUnmarshalJSON(t *testing.T) {\n\tt.Parallel()\n\n\tconst dqs = \"\\\"\"\n\n\tvar zero Output\n\ttests := []struct {\n\t\to         Output\n\t\tjson      string\n\t\tsucceeded bool\n\t}{\n\t\t{Value, dqs + valueStr + dqs, true},\n\t\t{IsTrendMode, dqs + isTrendModeStr + dqs, true},\n\t\t{IsCycleMode, dqs + isCycleModeStr + dqs, true},\n\t\t{InstantaneousTrendLine, dqs + instantaneousTrendLineStr + dqs, true},\n\t\t{SineWave, dqs + sineWaveStr + dqs, true},\n\t\t{SineWaveLead, dqs + sineWaveLeadStr + dqs, true},\n\t\t{DominantCyclePeriod, dqs + dominantCyclePeriodStr + dqs, true},\n\t\t{DominantCyclePhase, dqs + dominantCyclePhaseStr + dqs, true},\n\t\t{zero, dqs + unknownStr + dqs, false},\n\t\t{zero, dqs + \"foobar\" + dqs, false},\n\t}\n\n\tfor _, tt := range tests {\n\t\texp := tt.o\n\t\tbs := []byte(tt.json)\n\n\t\tvar o Output\n\n\t\terr := o.UnmarshalJSON(bs)\n\t\tif err != nil && tt.succeeded {\n\t\t\tt.Errorf(\"UnmarshalJSON('%v'): expected success '%v', got error %v\", tt.json, exp, err)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif err == nil && !tt.succeeded {\n\t\t\tt.Errorf(\"MarshalJSON('%v'): expected error, got success\", tt.json)\n\n\t\t\tcontinue\n\t\t}\n\n\t\tif exp != o {\n\t\t\tt.Errorf(\"MarshalJSON('%v'): expected '%v', actual '%v'\", tt.json, exp, o)\n\t\t}\n\t}\n}\n"
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

 Those errors seem unrelated to what I'm working on, so let me look at how the existing sinewave test is structured to use as a reference for the main test.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendline_test.go"
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/hilberttransformerinstantaneoustrendline_test.go</path>
<type>file</type>
<content>1: //nolint:testpackage
2: package hilberttransformerinstantaneoustrendline
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
16: func testHTITLTime() time.Time {
17: 	return time.Date(2021, time.April, 1, 0, 0, 0, 0, &time.Location{})
18: }
19: 
20: // Input data taken from TA-Lib (http://ta-lib.org/) tests, test_MAMA.xsl, Price, D5…D256, 252 entries.
21: //
22: //nolint:dupl
23: func testHTITLInput() []float64 {
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
54: // Expected period data taken from TA-Lib (http://ta-lib.org/) tests, test_MAMA.xsl, Period Adjustment X5…X256.
55: // 252 entries, smoothed as AI18=0.33*X18+0.67*AI17.
56: //
57: //nolint:dupl
58: func testHTITLExpectedPeriod() []float64 {
59: 	return []float64{
60: 		0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
61: 		0, 0, 0.39600000000000, 0.97812000000000, 1.62158040000000, 2.25545086800000, 2.84234568156000, 3.36868456664520, 3.86776291565229, 4.36321983508703,
62: 		4.87235783926831, 5.40838035704577, 5.98190550443027, 6.60199641969884, 7.27686930610184, 8.01438731048222, 8.82241286095647, 9.70906731606755, 10.68293087091460, 11.75320502957710,
63: 		12.92985285048740, 14.22372743856440, 15.55272286275450, 16.77503611571540, 17.85814025111630, 18.72970387649220, 19.30387978646140, 19.63314544969620, 19.86256979430250, 20.09030968609300,
64: 		20.24817834009410, 20.31132870798190, 20.52152604110820, 21.27119054536480, 22.10966835167300, 22.28715460952700, 21.91280773257140, 21.23923470724180, 20.70813161651310, 20.20449150221090,
65: 		19.52863321263000, 18.73709250583170, 17.96311275281150, 17.33367762545960, 16.91743352044750, 16.64300564862120, 16.41952162419500, 16.27464914327850, 16.26425245094380, 16.33321577028600,
66: 		16.39265551523990, 16.39976990202790, 16.38221107536320, 16.37405059271910, 16.35102942468120, 16.26839438425590, 16.12432207371240, 15.99529098667630, 15.96458956780100, 16.07977539207760,
67: 		16.38360881255670, 16.79746341307210, 17.18753188776420, 17.58524022168910, 18.05888760471740, 18.46077773999830, 18.78691120238400, 19.07789869381110, 19.11803073417110, 18.72675385299730,
68: 		18.07403190737810, 17.72999456892580, 18.00699920187680, 18.06680349806270, 17.73551482016550, 17.28467833183610, 16.97456900115070, 16.89386283663200, 17.00556420464730, 17.21986959021550,
69: 		17.48251598471900, 17.79647268844360, 18.15809655229470, 18.56044162987590, 19.03705300462320, 19.61779465906120, 20.13838368155990, 20.58144279802850, 20.93554178712450, 21.09578565733870,
70: 		21.19426268582890, 21.35270550953270, 21.46806615214910, 21.43420235778580, 21.27320458618770, 20.98617884905010, 20.62345107800030, 20.32165030848570, 20.09921951571820, 19.88214300840560,
71: 		19.67081622699810, 19.55217428481160, 19.60485773311710, 19.77836095343260, 19.77886122563300, 19.59009982815140, 19.54609435364200, 19.77945658439880, 20.20526697824140, 20.80572859375930,
72: 		21.45882440191380, 21.50916115262280, 21.07219135457730, 20.33979206665380, 19.60807769029340, 19.15831017112920, 19.03006205140340, 19.23359250887840, 19.84206353515510, 20.83692898803630,
73: 		22.25776348341490, 23.50933063567320, 24.02857349775940, 24.28548086650010, 24.74576845262060, 25.45685387492870, 26.31998583396390, 27.14553013410700, 27.80677101851790, 28.50146147525040,
74: 		29.16835938704370, 29.38723525724370, 29.55886298198770, 30.43981360336700, 30.70779370313880, 30.20667454311960, 29.23518282361370, 28.00037502954910, 26.95505291681000, 26.22399862702800,
75: 		25.67716809996900, 25.19893752937060, 24.76924271120940, 24.40654607774420, 24.12997738279210, 24.02648590415090, 24.17912316847620, 24.26552607123530, 24.07565548132200, 23.81050493977940,
76: 		23.75771490624360, 24.01627030476950, 24.42884190933990, 24.55867905189440, 24.41978729840000, 24.33536819272640, 24.11887925396970, 23.53741527509780, 22.66734716257270, 21.70419061052260,
77: 		20.78848949032480, 19.92593130809770, 19.09528115584620, 18.35405205698280, 17.81539769318840, 17.53491540732180, 17.59552736216070, 18.09376127214910, 18.69300796204700, 19.10361709066390,
78: 		19.40368660687600, 19.79324964337850, 20.14261316711870, 20.30292592814370, 20.37642955508450, 20.42856373321320, 20.36417897031590, 20.26870923265670, 20.31691792510900, 20.52593924664440,
79: 		20.95797078839970, 21.70315565998060, 22.68588957914270, 23.95566588814480, 25.30036991408680, 26.49222048853470, 28.24485763802440, 30.46863151925310, 31.19661794415910, 30.97271495031300,
80: 		30.15801520320610, 29.52193986806340, 28.48090879451130, 27.20913817575940, 25.84740758865390, 24.75875079095690, 24.57820671512040, 25.25622655282780, 26.58938264946150, 28.44936832011270,
81: 		30.75900691394640, 31.63120735338530, 31.95156902113430, 32.19329221743080, 32.18129930292270, 31.78927079951340, 30.94427836437330, 29.74153261553520, 28.44319750131350, 27.27756983469050,
82: 		26.30928991862760, 25.59706087830910, 25.19354035279110, 24.98183319418390, 24.66611779383150, 24.13629363553260, 23.59372342374540, 23.45943359521940, 24.13462330023790, 25.42868068174450,
83: 		27.22154743441240, 28.85990121754770, 29.25658159944000, 28.86760790158470, 28.27077502042400, 27.83957963686970, 27.56292753489200, 27.31665028261770, 27.11537844471070, 27.05619511102920,
84: 		26.72669604084850, 25.93839467294110, 24.88015320695530, 23.98089561843900, 23.51115215671300, 23.02173482203020, 22.29674643126940, 21.42162141795630, 20.54863761751100, 19.78167187971360,
85: 		19.14387361712880, 18.61396641752300,
86: 	}
87: }
88: 
89: // Expected instantaneous trend line values taken from MBST InstantaneousTrendLineTest.cs,
90: // generated using Excel implementation from TA-Lib (http://ta-lib.org/) tests, test_HT.xsl.
91: // 252 entries.
92: //
93: //nolint:dupl
94: func testHTITLExpectedValue() []float64 {
95: 	return []float64{
96: 		math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(),
97: 		math.NaN(), math.NaN(), math.NaN(),
98: 		0.0000000000000, 36.3130000000000, 63.1977500000000, 81.3542500000000, 90.5332083333333, 90.7274166666667, 90.8630416666667, 90.7904791666667, 90.5763791666666, 90.1190625000000,
99: 		89.5976625000000, 88.9683964285714, 88.1875857142857, 87.4745535714286, 86.8648353174603, 86.5406809523809, 86.4262830808081, 86.4413116161616, 86.4596270104895, 86.4576728687979,
100: 		86.5316151556777, 86.5976538663542, 86.6975181489262, 86.7923007288442, 86.7608821809425, 86.6849764619883, 86.4810361842105, 86.2045236842105, 85.9179875000000, 85.7217875000000,
101: 		85.6731214285714, 85.7294946428571, 85.8820685064935, 86.1150389610390, 86.4291412337662, 86.8542435064935, 87.1897624458874, 87.4640642857143, 87.6829172619047, 87.8553064223057,
102: 		87.9218177631579, 87.9011081656347, 87.8265154798762, 87.7802132352941, 87.8975367647059, 87.9932665441176, 88.0649329044118, 88.1058593750000, 88.0771718750000, 87.9885156250000,
103: 		87.8919218750000, 87.8735468750000, 87.9217812500000, 88.0132343750000, 88.1667968750000, 88.3524062500000, 88.5845312500000, 88.8867187500000, 89.2125000000000, 89.3545928308824,
104: 		89.4090386029412, 89.2706643178105, 89.1179705882353, 89.2742091503268, 89.6278143274854, 90.2226447368421, 90.9753333333333, 91.7409342105263, 92.5893245614035, 93.3576615497076,
105: 		94.0969948830409, 94.8495138888889, 95.5591111111111, 96.4495179738562, 97.4016168300654, 98.4999640522876, 99.7313676470588, 101.1202794117650, 102.8572647058820, 104.2939607843140,
106: 		105.8849452614380, 107.0814910990710, 108.1507799707600, 108.9417928362570, 109.6523348684210, 110.1277241228070, 110.5267148809520, 110.9164619047620, 111.3471666666670, 111.8653571428570,
107: 		112.3597857142860, 112.7992380952380, 113.1747142857140, 113.5586666666670, 114.0577738095240, 114.6932047619050, 115.2846898809520, 115.7229029761900, 115.8414500000000, 115.7467000000000,
108: 		115.6115500000000, 115.5303375000000, 115.5548000000000, 115.6614000000000, 115.9122375000000, 116.3321625000000, 116.8428375000000, 117.2830500000000, 117.7253922619050, 118.0727228896100,
109: 		118.4984101731600, 119.2198956709960, 120.0234311688310, 120.9289954887220, 121.7905789473680, 122.5689105263160, 123.2356894736840, 123.8346386591480, 124.4085501367050, 124.8498862554110,
110: 		125.4094954004330, 126.1065170454550, 126.7064925000000, 127.4240825000000, 127.8647176923080, 128.0896789173790, 128.1540621001220, 128.0788455784320, 128.1446725050170, 128.3183694581280,
111: 		128.4362103448280, 128.5848821839080, 128.6186973303670, 128.7719790322580, 128.8848929180570, 128.9942553260240, 129.0945314906040, 129.1530999536860, 129.0838581603580, 128.8024011680910,
112: 		128.4259584615380, 127.8496453846150, 127.2448900000000, 126.6843541666670, 126.1858333333330, 125.7495416666670, 125.2761041666670, 124.7606666666670, 124.3471666666670, 124.0095208333330,
113: 		123.7731875000000, 123.6621125000000, 123.6056416666670, 123.5744416666670, 123.5148250000000, 123.4937083333330, 123.4761041666670, 123.6832443181820, 124.0308714826840, 124.5405766233770,
114: 		125.2020811061750, 125.8966645363410, 126.4680156432750, 126.9272675438600, 127.4003611111110, 127.8605833333330, 128.1324342105260, 128.3722748538010, 128.6053611111110, 128.6804526315790,
115: 		128.7627552631580, 128.7694289473680, 128.7595250000000, 128.7409750000000, 128.6613250000000, 128.4858250000000, 128.2154750000000, 127.8500845238100, 127.4645440476190, 127.1153875541130,
116: 		126.7443323451910, 126.3035666290230, 125.8504999604740, 125.3674468450390, 124.8902561172160, 124.3766410622710, 123.8351980503370, 123.2633556835640, 122.5153494623660, 121.7438575268820,
117: 		120.2481227342550, 118.4358502645500, 116.5046713064710, 114.5443467073670, 112.9054354415950, 111.4893484615380, 110.7192281481480, 110.1931396825400, 110.2034782445810, 110.1120396292030,
118: 		109.5987628888250, 108.8005075604840, 107.8019843750000, 106.8111562500000, 105.6388140120970, 104.3560227486560, 102.7347166042630, 101.0735459592080, 99.5273900488400, 98.3256981583231,
119: 		97.3580796011396, 96.7594046153846, 96.4602600000000, 96.1232166666667, 96.1488191666667, 96.4425624637681, 96.7984085144927, 97.1613937318841, 97.3868053381643, 97.5343687643678,
120: 		97.8406773180077, 98.3334693486590, 99.0595665024631, 99.8837727832512, 100.6778885467980, 101.4226269841270, 102.0713564814810, 102.6847440476190, 103.2616851851850, 103.9931695156700,
121: 		104.8224465811970, 105.7652606837610, 106.6112275641030, 107.5680471014490, 108.3253364624510, 108.8823498494260, 109.2757501411630, 109.6873409090910, 110.1830037593980, 110.4551917293230,
122: 	}
123: }
124: 
125: // Tolerance used to compare floating-point values. Same rationale as SineWave: the trend line is
126: // built on top of an EMA-smoothed period driven by a cascade of WMA/Hilbert/α-filters, whose early
127: // residuals decay but never fully vanish.
128: const testHTITLTolerance = 1e-4
129: 
130: func TestHilbertTransformerInstantaneousTrendLineUpdate(t *testing.T) { //nolint: funlen
131: 	t.Parallel()
132: 
133: 	input := testHTITLInput()
134: 	expPeriod := testHTITLExpectedPeriod()
135: 	expValue := testHTITLExpectedValue()
136: 
137: 	const (
138: 		skip       = 9   // TradeStation implementation skips first 9 bars.
139: 		settleSkip = 177 // Samples required for the EMA to converge past structural reference mismatch.
140: 	)
141: 
142: 	t.Run("reference value (MBST InstantaneousTrendLineTest)", func(t *testing.T) {
143: 		t.Parallel()
144: 
145: 		x := testHTITLCreateDefault()
146: 
147: 		for i := skip; i < len(input); i++ {
148: 			value, _ := x.Update(input[i])
149: 			if math.IsNaN(value) || i < settleSkip {
150: 				continue
151: 			}
152: 
153: 			if math.IsNaN(expValue[i]) {
154: 				continue
155: 			}
156: 
157: 			if math.Abs(expValue[i]-value) > testHTITLTolerance {
158: 				t.Errorf("[%v] value is incorrect: expected %v, actual %v", i, expValue[i], value)
159: 			}
160: 		}
161: 	})
162: 
163: 	t.Run("reference period (test_MAMA.xsl, Period Adjustment)", func(t *testing.T) {
164: 		t.Parallel()
165: 
166: 		x := testHTITLCreateDefault()
167: 
168: 		for i := skip; i < len(input); i++ {
169: 			_, period := x.Update(input[i])
170: 			if math.IsNaN(period) || i < settleSkip {
171: 				continue
172: 			}
173: 
174: 			if math.Abs(expPeriod[i]-period) > testHTITLTolerance {
175: 				t.Errorf("[%v] period is incorrect: expected %v, actual %v", i, expPeriod[i], period)
176: 			}
177: 		}
178: 	})
179: 
180: 	t.Run("NaN input returns NaN pair", func(t *testing.T) {
181: 		t.Parallel()
182: 
183: 		x := testHTITLCreateDefault()
184: 		value, period := x.Update(math.NaN())
185: 
186: 		if !math.IsNaN(value) || !math.IsNaN(period) {
187: 			t.Errorf("expected (NaN, NaN), actual (%v, %v)", value, period)
188: 		}
189: 	})
190: }
191: 
192: func TestHilbertTransformerInstantaneousTrendLineIsPrimed(t *testing.T) {
193: 	t.Parallel()
194: 
195: 	input := testHTITLInput()
196: 
197: 	t.Run("primes somewhere in the input sequence", func(t *testing.T) {
198: 		t.Parallel()
199: 
200: 		x := testHTITLCreateDefault()
201: 
202: 		if x.IsPrimed() {
203: 			t.Error("expected not primed at start")
204: 		}
205: 
206: 		primedAt := -1
207: 
208: 		for i := range input {
209: 			x.Update(input[i])
210: 
211: 			if x.IsPrimed() && primedAt < 0 {
212: 				primedAt = i
213: 			}
214: 		}
215: 
216: 		if primedAt < 0 {
217: 			t.Error("expected indicator to become primed within the input sequence")
218: 		}
219: 
220: 		if !x.IsPrimed() {
221: 			t.Error("expected primed at end")
222: 		}
223: 	})
224: }
225: 
226: func TestHilbertTransformerInstantaneousTrendLineMetadata(t *testing.T) {
227: 	t.Parallel()
228: 
229: 	const (
230: 		descrVal = "Hilbert transformer instantaneous trend line "
231: 		descrDCP = "Dominant cycle period "
232: 	)
233: 
234: 	x := testHTITLCreateDefault()
235: 	act := x.Metadata()
236: 
237: 	check := func(what string, exp, a any) {
238: 		t.Helper()
239: 
240: 		if exp != a {
241: 			t.Errorf("%s is incorrect: expected %v, actual %v", what, exp, a)
242: 		}
243: 	}
244: 
245: 	mnemonic := "htitl(0.330, 4, 1.000, hl/2)"
246: 	mnemonicDCP := "dcp(0.330, hl/2)"
247: 
248: 	check("Type", core.HilbertTransformerInstantaneousTrendLine, act.Type)
249: 	check("Mnemonic", mnemonic, act.Mnemonic)
250: 	check("Description", descrVal+mnemonic, act.Description)
251: 	check("len(Outputs)", 2, len(act.Outputs))
252: 
253: 	check("Outputs[0].Kind", int(Value), act.Outputs[0].Kind)
254: 	check("Outputs[0].Type", outputs.ScalarType, act.Outputs[0].Type)
255: 	check("Outputs[0].Mnemonic", mnemonic, act.Outputs[0].Mnemonic)
256: 	check("Outputs[0].Description", descrVal+mnemonic, act.Outputs[0].Description)
257: 
258: 	check("Outputs[1].Kind", int(DominantCyclePeriod), act.Outputs[1].Kind)
259: 	check("Outputs[1].Type", outputs.ScalarType, act.Outputs[1].Type)
260: 	check("Outputs[1].Mnemonic", mnemonicDCP, act.Outputs[1].Mnemonic)
261: 	check("Outputs[1].Description", descrDCP+mnemonicDCP, act.Outputs[1].Description)
262: }
263: 
264: func TestHilbertTransformerInstantaneousTrendLineUpdateEntity(t *testing.T) { //nolint: funlen
265: 	t.Parallel()
266: 
267: 	const (
268: 		primeCount = 200
269: 		inp        = 100.
270: 		outputLen  = 2
271: 	)
272: 
273: 	tm := testHTITLTime()
274: 	check := func(act core.Output) {
275: 		t.Helper()
276: 
277: 		if len(act) != outputLen {
278: 			t.Errorf("len(output) is incorrect: expected %v, actual %v", outputLen, len(act))
279: 
280: 			return
281: 		}
282: 
283: 		for i := 0; i < outputLen; i++ {
284: 			s, ok := act[i].(entities.Scalar)
285: 			if !ok {
286: 				t.Errorf("output[%d] is not a scalar", i)
287: 
288: 				continue
289: 			}
290: 
291: 			if s.Time != tm {
292: 				t.Errorf("output[%d] time is incorrect: expected %v, actual %v", i, tm, s.Time)
293: 			}
294: 		}
295: 	}
296: 
297: 	input := testHTITLInput()
298: 
299: 	t.Run("update scalar", func(t *testing.T) {
300: 		t.Parallel()
301: 
302: 		s := entities.Scalar{Time: tm, Value: inp}
303: 		x := testHTITLCreateDefault()
304: 
305: 		for i := 0; i < primeCount; i++ {
306: 			x.Update(input[i%len(input)])
307: 		}
308: 
309: 		check(x.UpdateScalar(&s))
310: 	})
311: 
312: 	t.Run("update bar", func(t *testing.T) {
313: 		t.Parallel()
314: 
315: 		b := entities.Bar{Time: tm, High: inp, Low: inp, Close: inp}
316: 		x := testHTITLCreateDefault()
317: 
318: 		for i := 0; i < primeCount; i++ {
319: 			x.Update(input[i%len(input)])
320: 		}
321: 
322: 		check(x.UpdateBar(&b))
323: 	})
324: 
325: 	t.Run("update quote", func(t *testing.T) {
326: 		t.Parallel()
327: 
328: 		q := entities.Quote{Time: tm, Bid: inp, Ask: inp}
329: 		x := testHTITLCreateDefault()
330: 
331: 		for i := 0; i < primeCount; i++ {
332: 			x.Update(input[i%len(input)])
333: 		}
334: 
335: 		check(x.UpdateQuote(&q))
336: 	})
337: 
338: 	t.Run("update trade", func(t *testing.T) {
339: 		t.Parallel()
340: 
341: 		r := entities.Trade{Time: tm, Price: inp}
342: 		x := testHTITLCreateDefault()
343: 
344: 		for i := 0; i < primeCount; i++ {
345: 			x.Update(input[i%len(input)])
346: 		}
347: 
348: 		check(x.UpdateTrade(&r))
349: 	})
350: }
351: 
352: func TestNewHilbertTransformerInstantaneousTrendLine(t *testing.T) { //nolint: funlen,maintidx
353: 	t.Parallel()
354: 
355: 	const (
356: 		bc entities.BarComponent   = entities.BarClosePrice
357: 		qc entities.QuoteComponent = entities.QuoteMidPrice
358: 		tc entities.TradeComponent = entities.TradePrice
359: 
360: 		errAlpha = "invalid hilbert transformer instantaneous trend line parameters: " +
361: 			"α for additional smoothing should be in range (0, 1]"
362: 		errTLSL = "invalid hilbert transformer instantaneous trend line parameters: " +
363: 			"trend line smoothing length should be 2, 3, or 4"
364: 		errCPM = "invalid hilbert transformer instantaneous trend line parameters: " +
365: 			"cycle part multiplier should be in range (0, 10]"
366: 		errBC = "invalid hilbert transformer instantaneous trend line parameters: 9999: unknown bar component"
367: 		errQC = "invalid hilbert transformer instantaneous trend line parameters: 9999: unknown quote component"
368: 		errTC = "invalid hilbert transformer instantaneous trend line parameters: 9999: unknown trade component"
369: 	)
370: 
371: 	check := func(name string, exp, act any) {
372: 		t.Helper()
373: 
374: 		if exp != act {
375: 			t.Errorf("%s is incorrect: expected %v, actual %v", name, exp, act)
376: 		}
377: 	}
378: 
379: 	t.Run("default", func(t *testing.T) {
380: 		t.Parallel()
381: 
382: 		x, err := NewHilbertTransformerInstantaneousTrendLineDefault()
383: 		check("err == nil", true, err == nil)
384: 		check("mnemonic", "htitl(0.330, 4, 1.000, hl/2)", x.mnemonic)
385: 		check("primed", false, x.primed)
386: 		check("trendLineSmoothingLength", 4, x.trendLineSmoothingLength)
387: 	})
388: 
389: 	t.Run("tlsl=2", func(t *testing.T) {
390: 		t.Parallel()
391: 
392: 		params := Params{
393: 			AlphaEmaPeriodAdditional: 0.33,
394: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
395: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
396: 			TrendLineSmoothingLength: 2,
397: 			CyclePartMultiplier:      1.0,
398: 			BarComponent:             bc, QuoteComponent: qc, TradeComponent: tc,
399: 		}
400: 
401: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
402: 		check("err == nil", true, err == nil)
403: 		check("mnemonic", "htitl(0.330, 2, 1.000)", x.mnemonic)
404: 		check("coeff0", 2.0/3.0, x.coeff0)
405: 		check("coeff1", 1.0/3.0, x.coeff1)
406: 	})
407: 
408: 	t.Run("tlsl=3, phase accumulator", func(t *testing.T) {
409: 		t.Parallel()
410: 
411: 		params := Params{
412: 			AlphaEmaPeriodAdditional: 0.5,
413: 			EstimatorType:            hilberttransformer.PhaseAccumulator,
414: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
415: 			TrendLineSmoothingLength: 3,
416: 			CyclePartMultiplier:      0.5,
417: 			BarComponent:             bc, QuoteComponent: qc, TradeComponent: tc,
418: 		}
419: 
420: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
421: 		check("err == nil", true, err == nil)
422: 		check("mnemonic", "htitl(0.500, 3, 0.500, pa(4, 0.200, 0.200))", x.mnemonic)
423: 	})
424: 
425: 	t.Run("α ≤ 0", func(t *testing.T) {
426: 		t.Parallel()
427: 
428: 		params := Params{
429: 			AlphaEmaPeriodAdditional: 0.0,
430: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
431: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
432: 			TrendLineSmoothingLength: 4,
433: 			CyclePartMultiplier:      1.0,
434: 		}
435: 
436: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
437: 		check("x == nil", true, x == nil)
438: 		check("err", errAlpha, err.Error())
439: 	})
440: 
441: 	t.Run("α > 1", func(t *testing.T) {
442: 		t.Parallel()
443: 
444: 		params := Params{
445: 			AlphaEmaPeriodAdditional: 1.00000001,
446: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
447: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
448: 			TrendLineSmoothingLength: 4,
449: 			CyclePartMultiplier:      1.0,
450: 		}
451: 
452: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
453: 		check("x == nil", true, x == nil)
454: 		check("err", errAlpha, err.Error())
455: 	})
456: 
457: 	t.Run("tlsl < 2", func(t *testing.T) {
458: 		t.Parallel()
459: 
460: 		params := Params{
461: 			AlphaEmaPeriodAdditional: 0.33,
462: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
463: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
464: 			TrendLineSmoothingLength: 1,
465: 			CyclePartMultiplier:      1.0,
466: 		}
467: 
468: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
469: 		check("x == nil", true, x == nil)
470: 		check("err", errTLSL, err.Error())
471: 	})
472: 
473: 	t.Run("tlsl > 4", func(t *testing.T) {
474: 		t.Parallel()
475: 
476: 		params := Params{
477: 			AlphaEmaPeriodAdditional: 0.33,
478: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
479: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
480: 			TrendLineSmoothingLength: 5,
481: 			CyclePartMultiplier:      1.0,
482: 		}
483: 
484: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
485: 		check("x == nil", true, x == nil)
486: 		check("err", errTLSL, err.Error())
487: 	})
488: 
489: 	t.Run("cpMul ≤ 0", func(t *testing.T) {
490: 		t.Parallel()
491: 
492: 		params := Params{
493: 			AlphaEmaPeriodAdditional: 0.33,
494: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
495: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
496: 			TrendLineSmoothingLength: 4,
497: 			CyclePartMultiplier:      0.0,
498: 		}
499: 
500: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
501: 		check("x == nil", true, x == nil)
502: 		check("err", errCPM, err.Error())
503: 	})
504: 
505: 	t.Run("cpMul > 10", func(t *testing.T) {
506: 		t.Parallel()
507: 
508: 		params := Params{
509: 			AlphaEmaPeriodAdditional: 0.33,
510: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
511: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
512: 			TrendLineSmoothingLength: 4,
513: 			CyclePartMultiplier:      10.00001,
514: 		}
515: 
516: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
517: 		check("x == nil", true, x == nil)
518: 		check("err", errCPM, err.Error())
519: 	})
520: 
521: 	t.Run("invalid bar component", func(t *testing.T) {
522: 		t.Parallel()
523: 
524: 		params := Params{
525: 			AlphaEmaPeriodAdditional: 0.33,
526: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
527: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
528: 			TrendLineSmoothingLength: 4,
529: 			CyclePartMultiplier:      1.0,
530: 			BarComponent:             entities.BarComponent(9999), QuoteComponent: qc, TradeComponent: tc,
531: 		}
532: 
533: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
534: 		check("x == nil", true, x == nil)
535: 		check("err", errBC, err.Error())
536: 	})
537: 
538: 	t.Run("invalid quote component", func(t *testing.T) {
539: 		t.Parallel()
540: 
541: 		params := Params{
542: 			AlphaEmaPeriodAdditional: 0.33,
543: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
544: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
545: 			TrendLineSmoothingLength: 4,
546: 			CyclePartMultiplier:      1.0,
547: 			BarComponent:             bc, QuoteComponent: entities.QuoteComponent(9999), TradeComponent: tc,
548: 		}
549: 
550: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
551: 		check("x == nil", true, x == nil)
552: 		check("err", errQC, err.Error())
553: 	})
554: 
555: 	t.Run("invalid trade component", func(t *testing.T) {
556: 		t.Parallel()
557: 
558: 		params := Params{
559: 			AlphaEmaPeriodAdditional: 0.33,
560: 			EstimatorType:            hilberttransformer.HomodyneDiscriminator,
561: 			EstimatorParams:          testHTITLCreateCycleEstimatorParams(),
562: 			TrendLineSmoothingLength: 4,
563: 			CyclePartMultiplier:      1.0,
564: 			BarComponent:             bc, QuoteComponent: qc, TradeComponent: entities.TradeComponent(9999),
565: 		}
566: 
567: 		x, err := NewHilbertTransformerInstantaneousTrendLineParams(&params)
568: 		check("x == nil", true, x == nil)
569: 		check("err", errTC, err.Error())
570: 	})
571: }
572: 
573: func testHTITLCreateCycleEstimatorParams() hilberttransformer.CycleEstimatorParams {
574: 	const (
575: 		smoothingLength           = 4
576: 		alphaEmaQuadratureInPhase = 0.2
577: 		alphaEmaPeriod            = 0.2
578: 		warmUpPeriod              = 0
579: 	)
580: 
581: 	return hilberttransformer.CycleEstimatorParams{
582: 		SmoothingLength:           smoothingLength,
583: 		AlphaEmaQuadratureInPhase: alphaEmaQuadratureInPhase,
584: 		AlphaEmaPeriod:            alphaEmaPeriod,
585: 		WarmUpPeriod:              warmUpPeriod,
586: 	}
587: }
588: 
589: func testHTITLCreateDefault() *HilbertTransformerInstantaneousTrendLine {
590: 	x, _ := NewHilbertTransformerInstantaneousTrendLineDefault()
591: 
592: 	return x
593: }

(End of file - total 593 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.6s)

_Thinking:_

 I'm going to pull the reference arrays from the MBST TCM test file to use as my baseline data.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleModeTest.cs"
}
```

**Output:**
```
<path>/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleModeTest.cs</path>
<type>file</type>
<content>1: ﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
2: using System;
3: using System.Collections.Generic;
4: using System.IO;
5: using System.Runtime.Serialization;
6: using System.Xml;
7: using Mbst.Trading;
8: using Mbst.Trading.Indicators.JohnEhlers;
9: 
10: namespace Tests.Indicators.JohnEhlers
11: {
12:     [TestClass]
13:     public class TrendCycleModeTest
14:     {
15:         #region Test data
16:         /// <summary>
17:         /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MAMA.xsl, price, D5…D256, 252 entries.
18:         /// </summary>
19:         private readonly List<double> rawInput = new List<double>
20:         {
21:              92.0000,  93.1725,  95.3125,  94.8450,  94.4075,  94.1100,  93.5000,  91.7350,  90.9550,  91.6875,
22:              94.5000,  97.9700,  97.5775,  90.7825,  89.0325,  92.0950,  91.1550,  89.7175,  90.6100,  91.0000,
23:              88.9225,  87.5150,  86.4375,  83.8900,  83.0025,  82.8125,  82.8450,  86.7350,  86.8600,  87.5475,
24:              85.7800,  86.1725,  86.4375,  87.2500,  88.9375,  88.2050,  85.8125,  84.5950,  83.6575,  84.4550,
25:              83.5000,  86.7825,  88.1725,  89.2650,  90.8600,  90.7825,  91.8600,  90.3600,  89.8600,  90.9225,
26:              89.5000,  87.6725,  86.5000,  84.2825,  82.9075,  84.2500,  85.6875,  86.6100,  88.2825,  89.5325,
27:              89.5000,  88.0950,  90.6250,  92.2350,  91.6725,  92.5925,  93.0150,  91.1725,  90.9850,  90.3775,
28:              88.2500,  86.9075,  84.0925,  83.1875,  84.2525,  97.8600,  99.8750, 103.2650, 105.9375, 103.5000,
29:             103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,
30:             120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,
31:             114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,
32:             114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,
33:             124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,
34:             137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,
35:             123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,
36:             122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,
37:             123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,
38:             130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,
39:             127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,
40:             121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,
41:             106.9700, 110.0300,  91.0000,  93.5600,  93.6200,  95.3100,  94.1850,  94.7800,  97.6250,  97.5900,
42:              95.2500,  94.7200,  92.2200,  91.5650,  92.2200,  93.8100,  95.5900,  96.1850,  94.6250,  95.1200,
43:              94.0000,  93.7450,  95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,
44:             103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,
45:             106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,
46:             109.5300, 108.0600
47:         };
48: 
49:         /// <summary>
50:         /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MAMA.xsl, Period Adjustment, X5…X256, 252 entries, smoothed as AI18=0.33*X18+0.67*AI17.
51:         /// </summary>
52:         private readonly List<double> dcPeriod = new List<double>
53:         {
54:              0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,  0.00000000000000,
55:              0.00000000000000,  0.00000000000000,  0.39600000000000,  0.97812000000000,  1.62158040000000,  2.25545086800000,  2.84234568156000,  3.36868456664520,  3.86776291565229,  4.36321983508703,
56:              4.87235783926831,  5.40838035704577,  5.98190550443027,  6.60199641969884,  7.27686930610184,  8.01438731048222,  8.82241286095647,  9.70906731606755, 10.68293087091460, 11.75320502957710,
57:             12.92985285048740, 14.22372743856440, 15.55272286275450, 16.77503611571540, 17.85814025111630, 18.72970387649220, 19.30387978646140, 19.63314544969620, 19.86256979430250, 20.09030968609300,
58:             20.24817834009410, 20.31132870798190, 20.52152604110820, 21.27119054536480, 22.10966835167300, 22.28715460952700, 21.91280773257140, 21.23923470724180, 20.70813161651310, 20.20449150221090,
59:             19.52863321263000, 18.73709250583170, 17.96311275281150, 17.33367762545960, 16.91743352044750, 16.64300564862120, 16.41952162419500, 16.27464914327850, 16.26425245094380, 16.33321577028600,
60:             16.39265551523990, 16.39976990202790, 16.38221107536320, 16.37405059271910, 16.35102942468120, 16.26839438425590, 16.12432207371240, 15.99529098667630, 15.96458956780100, 16.07977539207760,
61:             16.38360881255670, 16.79746341307210, 17.18753188776420, 17.58524022168910, 18.05888760471740, 18.46077773999830, 18.78691120238400, 19.07789869381110, 19.11803073417110, 18.72675385299730,
62:             18.07403190737810, 17.72999456892580, 18.00699920187680, 18.06680349806270, 17.73551482016550, 17.28467833183610, 16.97456900115070, 16.89386283663200, 17.00556420464730, 17.21986959021550,
63:             17.48251598471900, 17.79647268844360, 18.15809655229470, 18.56044162987590, 19.03705300462320, 19.61779465906120, 20.13838368155990, 20.58144279802850, 20.93554178712450, 21.09578565733870,
64:             21.19426268582890, 21.35270550953270, 21.46806615214910, 21.43420235778580, 21.27320458618770, 20.98617884905010, 20.62345107800030, 20.32165030848570, 20.09921951571820, 19.88214300840560,
65:             19.67081622699810, 19.55217428481160, 19.60485773311710, 19.77836095343260, 19.77886122563300, 19.59009982815140, 19.54609435364200, 19.77945658439880, 20.20526697824140, 20.80572859375930,
66:             21.45882440191380, 21.50916115262280, 21.07219135457730, 20.33979206665380, 19.60807769029340, 19.15831017112920, 19.03006205140340, 19.23359250887840, 19.84206353515510, 20.83692898803630,
67:             22.25776348341490, 23.50933063567320, 24.02857349775940, 24.28548086650010, 24.74576845262060, 25.45685387492870, 26.31998583396390, 27.14553013410700, 27.80677101851790, 28.50146147525040,
68:             29.16835938704370, 29.38723525724370, 29.55886298198770, 30.43981360336700, 30.70779370313880, 30.20667454311960, 29.23518282361370, 28.00037502954910, 26.95505291681000, 26.22399862702800,
69:             25.67716809996900, 25.19893752937060, 24.76924271120940, 24.40654607774420, 24.12997738279210, 24.02648590415090, 24.17912316847620, 24.26552607123530, 24.07565548132200, 23.81050493977940,
70:             23.75771490624360, 24.01627030476950, 24.42884190933990, 24.55867905189440, 24.41978729840000, 24.33536819272640, 24.11887925396970, 23.53741527509780, 22.66734716257270, 21.70419061052260,
71:             20.78848949032480, 19.92593130809770, 19.09528115584620, 18.35405205698280, 17.81539769318840, 17.53491540732180, 17.59552736216070, 18.09376127214910, 18.69300796204700, 19.10361709066390,
72:             19.40368660687600, 19.79324964337850, 20.14261316711870, 20.30292592814370, 20.37642955508450, 20.42856373321320, 20.36417897031590, 20.26870923265670, 20.31691792510900, 20.52593924664440,
73:             20.95797078839970, 21.70315565998060, 22.68588957914270, 23.95566588814480, 25.30036991408680, 26.49222048853470, 28.24485763802440, 30.46863151925310, 31.19661794415910, 30.97271495031300,
74:             30.15801520320610, 29.52193986806340, 28.48090879451130, 27.20913817575940, 25.84740758865390, 24.75875079095690, 24.57820671512040, 25.25622655282780, 26.58938264946150, 28.44936832011270,
75:             30.75900691394640, 31.63120735338530, 31.95156902113430, 32.19329221743080, 32.18129930292270, 31.78927079951340, 30.94427836437330, 29.74153261553520, 28.44319750131350, 27.27756983469050,
76:             26.30928991862760, 25.59706087830910, 25.19354035279110, 24.98183319418390, 24.66611779383150, 24.13629363553260, 23.59372342374540, 23.45943359521940, 24.13462330023790, 25.42868068174450,
77:             27.22154743441240, 28.85990121754770, 29.25658159944000, 28.86760790158470, 28.27077502042400, 27.83957963686970, 27.56292753489200, 27.31665028261770, 27.11537844471070, 27.05619511102920,
78:             26.72669604084850, 25.93839467294110, 24.88015320695530, 23.98089561843900, 23.51115215671300, 23.02173482203020, 22.29674643126940, 21.42162141795630, 20.54863761751100, 19.78167187971360,
79:             19.14387361712880, 18.61396641752300
80:         };
81: 
82:         /// <summary>
83:         /// Generated using Excel implementation from TA-Lib (http://ta-lib.org/) tests, test_HT.xsl.
84:         /// </summary>
85:         private readonly List<double> dcPhase = new List<double>
86:         {
87:             double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,
88:             double.NaN,  double.NaN,  double.NaN,
89:             639.09090909090900,  98.05299963194700, 132.00564338345400,  69.61331949528320,  72.53929158292410,  49.31988383698000,  70.57064052204580,  95.44397344939550,  15.53098102018940,  -7.59977335936253,
90:              -0.43908926482948,   0.75159304711349,   6.81682854281200,  10.38041464097730,  14.60138862232650,  23.61952231391600,  34.25173964222910,  45.61939612889610,  54.93203281582900,  62.34730431858280,
91:              59.09332873285360,  64.74787861710400,  71.38960942346380,  78.26591034671290,  95.01873223899610, 100.81260420916600, 122.15965196914300, 155.92351856084900, 203.60327223472200, 237.36293513970900,
92:             244.38539212971100, 260.58654095568100, 254.66267427143100, 253.71116813655900, 126.51252837026600, 108.91084071926300, 120.62704116849900, 135.00432720483700, 148.37778407551400, 166.73764548652900,
93:             192.70016208445300, 227.19271648131900, 250.93069421530800, 272.91238631233800, 306.14557952390500, -30.56440912946640,  -7.29779366562593,  15.79435681229920,  37.27143784018100,  57.28482204365770,
94:              77.89434659794900, 100.26640790428300, 122.46792347519500, 143.34860922315100, 161.97099218663000, 179.04173949124300, 194.28337763382200, 207.12978658551400, 219.12056620855700, 228.99420937560000,
95:             246.39888033834300, 259.83330071373300, 279.43099672242300, 300.38982045382800, -42.95237520761510,  10.71059944380160,  67.91671263897760,  95.69661875079390, 116.17343846855600, 131.69663648791300,
96:             145.22435488266800, 157.96979483912800, 170.90824231177600, 190.12565895164300, 201.99948636973700, 211.84934448916700, 219.50941069822200, 223.55536694633000, 217.70267385839600, 208.61154857738600,
97:             187.20930268786500, 183.53325881754400, 173.77461459542900, 177.09324308654900, 177.36771678646600, 183.46254384786600, 190.38499136923400, 200.15247884572800, 210.26397611554500, 219.92876325408700,
98:             230.19440003342700, 241.78128922383700, 255.09398089013500, 269.90233026686000, 287.31443195349700, -22.67219169276530,  21.31042453594620,  55.99561073880190,  76.28592721136950,  93.25596317796150,
99:             111.58409337397600, 131.21106018529500, 148.25324978238800, 161.34668836868300, 167.25425018604100, 166.56119294454900, 164.79606018404900, 165.59339091940400, 167.71720854219100, 171.16337795073400,
100:             177.90397416349400, 180.76168462321900, 181.18022018527700, 182.90279528585400, 185.03420448405800, 187.45543006582800, 185.33720976352800, 183.30316508274000, 181.31467258646500, 177.91862390253700,
101:             179.98928550766400, 182.00429308763000, 183.62347330064100, 185.57299458485300, 189.35191538410900, 194.23220405450000, 200.11245439803000, 206.61598766532100, 214.60761785986900, 223.17826798412900,
102:             229.59983294953500, 238.43424165042300, 244.29554481830400, 259.55186325451500, 277.88404982202100, 297.46674550039700, -42.41381860502280, -22.05561416691630, -10.76874195668260,   4.55423903808060,
103:              13.05706378617280,  25.49753552893060,  31.99805128632640,  38.60334291217220,  45.70890152749460,  53.48688015996070,  60.82873486408750,  67.21350248296350,  74.98104788942620,  84.48479832948730,
104:              97.41668808537870, 101.86825371917200, 131.27099687518600, 148.43969963128300, 169.23915874881900, 186.79876808562800, 222.27683786450500, 226.80174114624100, 148.12023947867300, 119.78684089821000,
105:             123.91411010078200, 136.35086145099600, 148.39281918279000, 160.94079449625800, 171.89311802023800, 182.21130213571000, 189.94824024493000, 202.58335256387900, 215.72849327557900, 224.87477584909900,
106:             239.93794152235800, 257.07736337068300, 274.90021437724400, 292.81710751479900, 310.18755119542100, -34.09748283129430, -20.55209191423000, -17.48926436788570,  -6.64084597196063,  -3.41661969757843,
107:              -1.11706604110969,  -0.61559202318233,  -0.58343503934742,  -0.97020892968976,  -7.33789387098142, -13.71942088996000, -14.16303590443250,  -9.85074940762922,  -2.93111556519989,   0.35846302482548,
108:              -0.27797938543370,  -4.11254536082527,  -7.19786584394603,  -8.12930435521150,  -7.23274284499956,  -5.60008181003320,  -3.98246383052538,  -1.93459828828531,  -0.91376116945821,   1.11347590999549,
109:               3.48574296192987,   5.87739974191743,   8.51611669495514,  11.77045158406290,  16.98321519660290,  23.12127015453780,  32.37560208179040,  38.09489298723020,  44.37798569415560,  48.60625731428030,
110:              57.00174598372450,  65.55585833338270,  77.53688240972470,  96.95830008657520, 111.55989345666400, 124.32277400743800, 131.41492641407500, 137.73657404096000, 141.46543104438800, 142.80498887855200,
111:             149.73966957805500, 153.53899811794900, 157.79183782289500, 159.94501203849600, 162.96541156987000, 170.96418133015300, 176.71998519472000, 182.65581927371500, 189.05283471879000, 197.04766728263800,
112:             206.92813680000000, 217.32451174036000, 224.38800791349600, 232.20526011220900, 246.06238925337200, 271.09605242709400, 289.11924018406000,  -31.18231981512890, 23.26913353342980,  47.27652706672060
113:         };
114: 
115:         /// <summary>
116:         /// Generated using Excel implementation from TA-Lib (http://ta-lib.org/) tests, test_HT.xsl.
117:         /// </summary>
118:         private readonly List<double> sine = new List<double>
119:         {
120:             double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,
121:             double.NaN, double.NaN,  double.NaN,
122:             -0.9874388886763950, 0.9901389074597280, 0.7430789154373040, 0.9373629963767220, 0.9539229407379340, 0.7583605937417590, 0.9430523282622310, 0.9954894450161860, 0.2677593920226290,-0.1322524693787770,
123:             -0.0076634783681766, 0.0131173971015221, 0.1186956105503800, 0.1801829214581150, 0.2520928115991770, 0.4006612403521310, 0.5628300247542610, 0.7147094931388430, 0.8184710623910560, 0.8857771050315170,
124:              0.8580051054352940, 0.9044393510772560, 0.9477105512269880, 0.9791019835938800, 0.9961661502181070, 0.9822460058918840, 0.8465682109433450, 0.4079557291761370,-0.4004013665430420,-0.8421036878603060,
125:             -0.9017223344864390,-0.9865337683617420,-0.9643853118844240,-0.9598599815079740, 0.8037267766705520, 0.9460240546952540, 0.8605016852751490, 0.7070533756573690, 0.5243161160505560, 0.2294102723810260,
126:             -0.2198489640488240,-0.7336434870569610,-0.9451240721288890,-0.9987083965673280,-0.8075209150605320,-0.5085066436721070,-0.1270264128351010, 0.2721854747475370, 0.6055917784886910, 0.8413676398899490,
127:              0.9777625488346960, 0.9839896992562860, 0.8436921159626520, 0.5969447127483850, 0.3094984574050860, 0.0167240212732023,-0.2467178763145330,-0.4560076441356610,-0.6309543271436050,-0.7546432714599400,
128:             -0.9163549058535820,-0.9842983646399860,-0.9864836587318940,-0.8626035608908470,-0.6813902164376480, 0.1858483908516400, 0.9266383325265240, 0.9950614294774160, 0.8974629492620320, 0.7466772329212920,
129:              0.5703644678660200, 0.3750953336668390, 0.1580160207328650,-0.1758076016904010,-0.3745982816248290,-0.5276875478007350,-0.6362049492364280,-0.6890552092033720,-0.6115639640491650,-0.4788688150780810,
130:             -0.1254943141670760,-0.0616279223840421, 0.1084398125157010, 0.0507107190849194, 0.0459258492372133,-0.0603960127005548,-0.1802614924906120,-0.3445196936393600,-0.5039846767879240,-0.6418346779779860,
131:             -0.7682209569752370,-0.8811490866996920,-0.9663490613441720,-0.9999985470701830,-0.9546858649139780,-0.3854582462377580, 0.3634207386677970, 0.8289947319983170, 0.9714909193370010, 0.9983857647297360,
132:              0.9298786498974830, 0.7522877436715410, 0.5261656913352240, 0.3198410350076260, 0.2206250837743810, 0.2324067219830480, 0.2622555352188150, 0.2488016117878720, 0.2127369252136960, 0.1536174562004720,
133:              0.0365743929645966,-0.0132935129731492,-0.0205972714630062,-0.0506416644795282,-0.0877504363551195,-0.1297549156821220,-0.0930172242876989,-0.0576191763867261,-0.0229433518557876, 0.0363188766621527,
134:              0.0001870031678450,-0.0349743794742610,-0.0631993922084072,-0.0971138050129020,-0.1624979390656570,-0.2458522401310880,-0.3438638176192820,-0.4480085728769030,-0.5679531815591490,-0.6842705623709480,
135:             -0.7615364178876680,-0.8520399314791350,-0.9010432983130380,-0.9834194615045590,-0.9905476870988080,-0.8872786828920670,-0.6744804687446460,-0.3755063881128740,-0.1868453942460660, 0.0794027920237213,
136:              0.2259213674108930, 0.4304722733878400, 0.5298904205645140, 0.6239251933219030, 0.7158012315374360, 0.8037206344012380, 0.8731666378756390, 0.9219544489912210, 0.9658401621307640, 0.9953707336137810,
137:              0.9916336070524670, 0.9786230879164460, 0.7515981300962940, 0.5233956277754490, 0.1867099275394900,-0.1183826185655860,-0.6727134585677300,-0.7289894296051810, 0.5281384062949680, 0.8678795703185170,
138:              0.8298749053132090, 0.6902403611717530, 0.5240926478571590, 0.3265450140990430, 0.1410201458363470,-0.0385849224232995,-0.1727584531294190,-0.3840270653389320,-0.5839449895165980,-0.7055596595779030,
139:             -0.8654833335157070,-0.9746729156329320,-0.9963449765889340,-0.9217474051403660,-0.7639362508931000,-0.5606026149454520,-0.3510588357296890,-0.3005270942524180,-0.1156452933894830,-0.0595959289499457,
140:             -0.0194952452596040,-0.0107439009455394,-0.0101826864310280,-0.0169325310282176,-0.1277205927832380,-0.2371674472604240,-0.2446819018845580,-0.1710822516700410,-0.0511353061733825, 0.0062563192154928,
141:             -0.0048516364950122,-0.0717158401086350,-0.1252962791394740,-0.1414075684951130,-0.1259001773043880,-0.0975843207989124,-0.0694511521424164,-0.0337586943638060,-0.0159474649423128, 0.0194325975213332,
142:              0.0608001695138442, 0.1024001693533800, 0.1480876049487220, 0.2039912059729780, 0.2920915426229240, 0.3926785587852390, 0.5354672113127650, 0.6169657298756830, 0.6993887726683350, 0.7501832875129180,
143:              0.8386871644310250, 0.9103651277237890, 0.9764351312310820, 0.9926345853646890, 0.9300339420371430, 0.8258742381128520, 0.7499387624704700, 0.6725402422076670, 0.6229867037490120, 0.6045297568333980,
144:              0.5039297182908610, 0.4455885766999630, 0.3779726796046290, 0.3429218283962470, 0.2929489553699220, 0.1570518914741900, 0.0572157944020108,-0.0463361936582273,-0.1573451851308570,-0.2931672021738570,
145:             -0.4528725984930980,-0.6063286567677410,-0.6995137850617610,-0.7902112777605310,-0.9139878106324440,-0.9998170324167670,-0.9448389776183910,-0.5177630386017770, 0.3950506584043580, 0.7346367050507850
146:         };
147: 
148:         /// <summary>
149:         /// Generated using Excel implementation from TA-Lib (http://ta-lib.org/) tests, test_HT.xsl.
150:         /// </summary>
151:         private readonly List<double> sineLead = new List<double>
152:         {
153:             double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,
154:             double.NaN, double.NaN,  double.NaN,
155:             -0.5865008756606930, 0.6010760134973850, 0.0522375953518313, 0.9091393120803100, 0.8866939724449600, 0.9971590529781770, 0.9020538171846020, 0.6368324474361340, 0.8706218323553310, 0.6073789821251400,
156:              0.7016671195580980, 0.7163213444559970, 0.7860384941578910, 0.8229422149880850, 0.8625259332042720, 0.9311800857903170, 0.9824560606058480, 0.9999415670942400, 0.9850130504072370, 0.9545149566415440,
157:              0.9699003741647460, 0.9411885260170180, 0.8957923799982550, 0.8361338691916270, 0.6425371253442400, 0.5619014189746250, 0.2222351486408030,-0.3571214380720770,-0.9310766529205810,-0.9768109870729720,
158:             -0.9433073135761630,-0.8132374819975520,-0.8689541000414960,-0.8770525415166500, 0.1475931480924170, 0.4397692495572380, 0.2482327294712000,-0.0000755239717352,-0.2313707009630830,-0.5260305525740280,
159:             -0.8452633448517760,-0.9992677882479890,-0.8993236485622310,-0.6702662015228300,-0.1539244027456790, 0.2492915015754510, 0.6115575080093740, 0.8728740226620090, 0.9909162839717420, 0.9771019730335230,
160:              0.8396734557306670, 0.5697614425887530, 0.2169861504951610,-0.1451956537134670,-0.4535393411417920,-0.6951822190514210,-0.8597041192450010,-0.9517540619903210,-0.9947396502168260,-0.9975710951353530,
161:             -0.9310629460274820,-0.8208173676831810,-0.5816830016568750,-0.2522412838591280, 0.0357301876330212, 0.8262025299696030, 0.9210718625890570, 0.6334265388777150, 0.3227045140978610, 0.0576226338781002,
162:             -0.1775030791814070,-0.3902458029200550,-0.5864888795958400,-0.8204080195335590,-0.9205013506909260,-0.9737752029989330,-0.9954119273819820,-0.9996821539441930,-0.9919003713144100,-0.9593708639942700,
163:             -0.7902545151018210,-0.7493402295361840,-0.6262584572592140,-0.6703391127989300,-0.6738861991115050,-0.7485223840691900,-0.8229875936131110,-0.9074292725287760,-0.9671080150009190,-0.9960855660349680,
164:             -0.9958932520932240,-0.9574138340514830,-0.8652041011461150,-0.7083111287566420,-0.4646190135134790, 0.3799051621364850, 0.9157357095569460, 0.9816417978833290, 0.8545864068220020, 0.6658040149144910,
165:              0.3974026651162090, 0.0660812869143209,-0.2292556000437840,-0.4438015597350790,-0.5336772503175370,-0.5234089018965820,-0.4969142899048420,-0.5089421255844810,-0.5404930402692640,-0.5900897578798300,
166:             -0.6807716781362360,-0.7164442325283580,-0.7215212414254330,-0.7420085481569570,-0.7664280384961050,-0.7928795495979390,-0.7698142295937170,-0.7466749292126270,-0.7231440467849940,-0.6809589449044260,
167:             -0.7069745376146330,-0.7314048006899320,-0.7503819371382670,-0.7724343179777090,-0.8126119368714450,-0.8592475635206920,-0.9071355135227490,-0.9489640525891650,-0.9835954633562350,-0.9994945745179860,
168:             -0.9967791122784410,-0.9726372051055730,-0.9438266487324060,-0.8236131488885440,-0.6034299982731380,-0.3012592866284200, 0.0451220549347129, 0.3898374562298360, 0.5625345127655470, 0.7610204246544270,
169:              0.8485754490678880, 0.9426271321408510, 0.9743624133056830, 0.9937744211043730, 0.9999234595478280, 0.9890496834828740, 0.9620813190845490, 0.9257815160887320, 0.8661907447689560, 0.7717933199927350,
170:              0.6099143741297350, 0.5465660379422910, 0.0650374448708587,-0.2324218737852860,-0.5626485140051470,-0.7858435966184150,-0.9988707502989160,-0.9995056053709990,-0.2269953459568470, 0.2624108067271960,
171:              0.1922802999868170,-0.0235747958138623,-0.2316259847404300,-0.4374421611852380,-0.6003241683068820,-0.7338638764656210,-0.8186335536900920,-0.9244352736103260,-0.9869359601928300,-0.9999976116346720,
172:             -0.9662055929589490,-0.8473318021865150,-0.6441207677355740,-0.3775643212631200,-0.0838943519776584, 0.1891385830833930, 0.4138657569596380, 0.4619148062355390, 0.6205889300081480, 0.6637092720958960,
173:              0.6931871751689060, 0.6994688836282570, 0.6998698746833480, 0.6950322988997850, 0.6110036127434860, 0.5192294559798360, 0.5125969121038930, 0.5757083087460280, 0.6700235759926480, 0.7115168281729820,
174:              0.7036678339992160, 0.6545752979714670, 0.6129364849611150, 0.6000111421090120, 0.6124554033545860, 0.6347294098505560, 0.6562899876128100, 0.6828327376881520, 0.6957402985880160, 0.7207141791649000,
175:              0.7487908163625680, 0.7757975773123150, 0.8040241461526770, 0.8364818144534760, 0.8828100232401670, 0.9279746557928580, 0.9758237828336110, 0.9927466296862280, 0.9999410720652520, 0.9980198650776760,
176:              0.9781412645524530, 0.9363303228513180, 0.8430454008691210, 0.6162348270656310, 0.3977902134753690, 0.1852760304055380, 0.0625305163973752,-0.0477440698701413,-0.1126037294757250,-0.1358018386690990,
177:             -0.2544275858052280,-0.3179500551407050,-0.3873842567862560,-0.4217482646499100,-0.4689384577141290,-0.5872793774815170,-0.6654907471409910,-0.7391119157698760,-0.8095586705039820,-0.8833378646001520,
178:             -0.9506681839279980,-0.9910404297342630,-0.9999429557041830,-0.9921031907559610,-0.9331896469980470,-0.6934514712647360,-0.4364996881408350, 0.2388331163456430, 0.9289332462002390, 0.9992107524543470
179:         };
180: 
181:         /// <summary>
182:         /// Generated using Excel implementation from TA-Lib (http://ta-lib.org/) tests, test_HT.xsl.
183:         /// </summary>
184:         private readonly List<double> itl = new List<double>
185:         {
186:             double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,  double.NaN,
187:             double.NaN, double.NaN,  double.NaN,
188:               0.0000000000000,  36.3130000000000,  63.1977500000000,  81.3542500000000,  90.5332083333333,  90.7274166666667,  90.8630416666667,  90.7904791666667,  90.5763791666666,  90.1190625000000,
189:              89.5976625000000,  88.9683964285714,  88.1875857142857,  87.4745535714286,  86.8648353174603,  86.5406809523809,  86.4262830808081,  86.4413116161616,  86.4596270104895,  86.4576728687979,
190:              86.5316151556777,  86.5976538663542,  86.6975181489262,  86.7923007288442,  86.7608821809425,  86.6849764619883,  86.4810361842105,  86.2045236842105,  85.9179875000000,  85.7217875000000,
191:              85.6731214285714,  85.7294946428571,  85.8820685064935,  86.1150389610390,  86.4291412337662,  86.8542435064935,  87.1897624458874,  87.4640642857143,  87.6829172619047,  87.8553064223057,
192:              87.9218177631579,  87.9011081656347,  87.8265154798762,  87.7802132352941,  87.8975367647059,  87.9932665441176,  88.0649329044118,  88.1058593750000,  88.0771718750000,  87.9885156250000,
193:              87.8919218750000,  87.8735468750000,  87.9217812500000,  88.0132343750000,  88.1667968750000,  88.3524062500000,  88.5845312500000,  88.8867187500000,  89.2125000000000,  89.3545928308824,
194:              89.4090386029412,  89.2706643178105,  89.1179705882353,  89.2742091503268,  89.6278143274854,  90.2226447368421,  90.9753333333333,  91.7409342105263,  92.5893245614035,  93.3576615497076,
195:              94.0969948830409,  94.8495138888889,  95.5591111111111,  96.4495179738562,  97.4016168300654,  98.4999640522876,  99.7313676470588, 101.1202794117650, 102.8572647058820, 104.2939607843140,
196:             105.8849452614380, 107.0814910990710, 108.1507799707600, 108.9417928362570, 109.6523348684210, 110.1277241228070, 110.5267148809520, 110.9164619047620, 111.3471666666670, 111.8653571428570,
197:             112.3597857142860, 112.7992380952380, 113.1747142857140, 113.5586666666670, 114.0577738095240, 114.6932047619050, 115.2846898809520, 115.7229029761900, 115.8414500000000, 115.7467000000000,
198:             115.6115500000000, 115.5303375000000, 115.5548000000000, 115.6614000000000, 115.9122375000000, 116.3321625000000, 116.8428375000000, 117.2830500000000, 117.7253922619050, 118.0727228896100,
199:             118.4984101731600, 119.2198956709960, 120.0234311688310, 120.9289954887220, 121.7905789473680, 122.5689105263160, 123.2356894736840, 123.8346386591480, 124.4085501367050, 124.8498862554110,
200:             125.4094954004330, 126.1065170454550, 126.7064925000000, 127.4240825000000, 127.8647176923080, 128.0896789173790, 128.1540621001220, 128.0788455784320, 128.1446725050170, 128.3183694581280,
201:             128.4362103448280, 128.5848821839080, 128.6186973303670, 128.7719790322580, 128.8848929180570, 128.9942553260240, 129.0945314906040, 129.1530999536860, 129.0838581603580, 128.8024011680910,
202:             128.4259584615380, 127.8496453846150, 127.2448900000000, 126.6843541666670, 126.1858333333330, 125.7495416666670, 125.2761041666670, 124.7606666666670, 124.3471666666670, 124.0095208333330,
203:             123.7731875000000, 123.6621125000000, 123.6056416666670, 123.5744416666670, 123.5148250000000, 123.4937083333330, 123.4761041666670, 123.6832443181820, 124.0308714826840, 124.5405766233770,
204:             125.2020811061750, 125.8966645363410, 126.4680156432750, 126.9272675438600, 127.4003611111110, 127.8605833333330, 128.1324342105260, 128.3722748538010, 128.6053611111110, 128.6804526315790,
205:             128.7627552631580, 128.7694289473680, 128.7595250000000, 128.7409750000000, 128.6613250000000, 128.4858250000000, 128.2154750000000, 127.8500845238100, 127.4645440476190, 127.1153875541130,
206:             126.7443323451910, 126.3035666290230, 125.8504999604740, 125.3674468450390, 124.8902561172160, 124.3766410622710, 123.8351980503370, 123.2633556835640, 122.5153494623660, 121.7438575268820,
207:             120.2481227342550, 118.4358502645500, 116.5046713064710, 114.5443467073670, 112.9054354415950, 111.4893484615380, 110.7192281481480, 110.1931396825400, 110.2034782445810, 110.1120396292030,
208:             109.5987628888250, 108.8005075604840, 107.8019843750000, 106.8111562500000, 105.6388140120970, 104.3560227486560, 102.7347166042630, 101.0735459592080,  99.5273900488400,  98.3256981583231,
209:              97.3580796011396,  96.7594046153846,  96.4602600000000,  96.1232166666667,  96.1488191666667,  96.4425624637681,  96.7984085144927,  97.1613937318841,  97.3868053381643,  97.5343687643678,
210:              97.8406773180077,  98.3334693486590,  99.0595665024631,  99.8837727832512, 100.6778885467980, 101.4226269841270, 102.0713564814810, 102.6847440476190, 103.2616851851850, 103.9931695156700,
211:             104.8224465811970, 105.7652606837610, 106.6112275641030, 107.5680471014490, 108.3253364624510, 108.8823498494260, 109.2757501411630, 109.6873409090910, 110.1830037593980, 110.4551917293230
212:         };
213: 
214:         /// <summary>
215:         /// Taken from TA-Lib (http://ta-lib.org/) implementation, test_HT.xsl.
216:         /// The first 63 values are skipped (unstable period according to TA-Lib), total 200 values available (trial version).
217:         /// </summary>
218:         private readonly List<double> tcm = new List<double>
219:         {
220:             // First 63 entries are not available (unstable period).
221:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
222:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
223:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
224:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
225:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
226:             double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
227:             double.NaN, double.NaN, double.NaN,
228:             1, 1, 1, 1, 1, 1, 1,-1,-1, 1, 1, 1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
229:             1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1,-1, 1, 1, 1,-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
230:             1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
231:             1, 1, 1,-1, 1, 1, 1,-1,-1,-1,-1,-1,-1,-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,-1,-1, 1, 1,
232:             1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
233:         };
234:         #endregion
235: 
236:         #region NameTest
237:         /// <summary>
238:         /// A test for Name.
239:         /// </summary>
240:         [TestMethod]
241:         public void NameTest()
242:         {
243:             var target = new TrendCycleMode();
244:             Assert.AreEqual("TCM", target.Name);
245:         }
246:         #endregion
247: 
248:         #region MonikerTest
249:         /// <summary>
250:         /// A test for Moniker.
251:         /// </summary>
252:         [TestMethod]
253:         public void MonikerTest()
254:         {
255:             var target = new TrendCycleMode();
256:             Assert.AreEqual("TCM4", target.Moniker);
257:             //target = new TrendCycleMode(4);
258:             //Assert.AreEqual("TCM4", target.Moniker);
259:             target = new TrendCycleMode(/*3*/);
260:             Assert.AreEqual("TCM3", target.Moniker);
261:             target = new TrendCycleMode(/*2*/);
262:             Assert.AreEqual("TCM2", target.Moniker);
263:         }
264:         #endregion
265: 
266:         #region DescriptionTest
267:         /// <summary>
268:         /// A test for Description.
269:         /// </summary>
270:         [TestMethod]
271:         public void DescriptionTest()
272:         {
273:             var target = new TrendCycleMode();
274:             Assert.AreEqual("Trend versus Cycle Mode", target.Description);
275:         }
276:         #endregion
277: 
278:         #region SmoothingLengthTest
279:         /// <summary>
280:         /// A test for SmoothingLength.
281:         /// </summary>
282:         [TestMethod]
283:         public void SmoothingLengthTest()
284:         {
285:             var target = new TrendCycleMode();
286:             Assert.AreEqual(4, target.SmoothingLength);
287:             //target = new TrendCycleMode(4);
288:             //Assert.AreEqual(4, target.SmoothingLength);
289:             target = new TrendCycleMode(/*3*/);
290:             Assert.AreEqual(3, target.SmoothingLength);
291:             target = new TrendCycleMode(/*2*/);
292:             Assert.AreEqual(2, target.SmoothingLength);
293:         }
294:         #endregion
295: 
296:         #region IsPrimedTest
297:         /// <summary>
298:         /// A test for IsPrimed.
299:         /// </summary>
300:         [TestMethod]
301:         public void IsPrimedTest()
302:         {
303:             var target = new TrendCycleMode();
304:             Assert.IsFalse(target.IsPrimed);
305:             for (int i = 1; i <= 23; ++i)
306:             {
307:                 var scalar = new Scalar(DateTime.Now, i);
308:                 scalar = target.Update(scalar);
309:                 Assert.IsTrue(double.IsNaN(scalar.Value));
310:                 Assert.IsFalse(target.IsPrimed);
311:             }
312:             for (int i = 24; i <= 48; ++i)
313:             {
314:                 var scalar = new Scalar(DateTime.Now, i);
315:                 scalar = target.Update(scalar);
316:                 Assert.IsFalse(double.IsNaN(scalar.Value));
317:                 Assert.IsTrue(target.IsPrimed);
318:             }
319:         }
320:         #endregion
321: 
322:         #region DominantCyclePeriodTest
323:         /// <summary>
324:         /// A test for DominantCyclePeriod.
325:         /// </summary>
326:         [TestMethod]
327:         public void DominantCyclePeriodTest()
328:         {
329:             const int digits = 9;
330:             var target = new TrendCycleMode();
331:             // Tradestation implementation skips first 9 bars.
332:             for (int i = 9; i < rawInput.Count; ++i)
333:             {
334:                 target.Update(rawInput[i]);
335:                 double d = Math.Round(target.DominantCyclePeriod, digits);
336:                 double u = Math.Round(dcPeriod[i], digits);
337:                 if (!double.IsNaN(d))
338:                     Assert.AreEqual(u, d);
339:             }
340:         }
341:         #endregion
342: 
343:         #region DominantCyclePhaseTest
344:         /// <summary>
345:         /// A test for DominantCyclePhase.
346:         /// </summary>
347:         [TestMethod]
348:         public void DominantCyclePhaseTest()
349:         {
350:             const int digits = 8;
351:             var target = new TrendCycleMode();
352:             // Tradestation implementation skips first 9 bars.
353:             for (int i = 9; i < rawInput.Count; ++i)
354:             {
355:                 target.Update(rawInput[i]);
356:                 double d = Math.Round(target.DominantCyclePhase, digits);
357:                 double u = Math.Round(dcPhase[i], digits);
358:                 if (!double.IsNaN(d))
359:                     Assert.AreEqual(u, d);
360:             }
361:         }
362:         #endregion
363: 
364:         #region SineWaveTest
365:         /// <summary>
366:         /// A test for SineWave.
367:         /// </summary>
368:         [TestMethod]
369:         public void SineWaveTest()
370:         {
371:             const int digits = 9;
372:             var target = new TrendCycleMode();
373:             // Tradestation implementation skips first 9 bars.
374:             for (int i = 9; i < rawInput.Count; ++i)
375:             {
376:                 target.Update(rawInput[i]);
377:                 double d = Math.Round(target.SineWave, digits);
378:                 double u = Math.Round(sine[i], digits);
379:                 if (!double.IsNaN(d))
380:                     Assert.AreEqual(u, d);
381:             }
382:         }
383:         #endregion
384: 
385:         #region SineWaveLeadTest
386:         /// <summary>
387:         /// A test for SineWaveLead.
388:         /// </summary>
389:         [TestMethod]
390:         public void SineWaveLeadTest()
391:         {
392:             const int digits = 9;
393:             var target = new TrendCycleMode();
394:             // Tradestation implementation skips first 9 bars.
395:             for (int i = 9; i < rawInput.Count; ++i)
396:             {
397:                 target.Update(rawInput[i]);
398:                 double d = Math.Round(target.SineWaveLead, digits);
399:                 double u = Math.Round(sineLead[i], digits);
400:                 if (!double.IsNaN(d))
401:                     Assert.AreEqual(u, d);
402:             }
403:         }
404:         #endregion
405: 
406:         #region InstantaneousTrendLineTest
407:         /// <summary>
408:         /// A test for InstantaneousTrendLine.
409:         /// </summary>
410:         [TestMethod]
411:         public void InstantaneousTrendLineTest()
412:         {
413:             const int digits = 9;
414:             var target = new TrendCycleMode();
415:             // Tradestation implementation skips first 9 bars.
416:             for (int i = 9; i < rawInput.Count; ++i)
417:             {
418:                 target.Update(rawInput[i]);
419:                 double d = Math.Round(target.InstantaneousTrendLine, digits);
420:                 double u = Math.Round(itl[i], digits);
421:                 if (!double.IsNaN(d))
422:                     Assert.AreEqual(u, d);
423:             }
424:         }
425:         #endregion
426: 
427:         #region UpdateTest
428:         /// <summary>
429:         /// A test for Update.
430:         /// </summary>
431:         [TestMethod]
432:         public void UpdateTest()
433:         {
434:             const int digits = 9;
435:             var target = new TrendCycleMode();
436:             // Tradestation implementation skips first 9 bars.
437:             for (int i = 9; i < tcm.Count; ++i)
438:             {
439:                 target.Update(rawInput[i]);
440:                 double d = Math.Round(target.Value, digits);
441:                 double u = Math.Round(tcm[i], digits);
442:                 if (i != 70 && i != 71 && !double.IsNaN(u))
443:                     Assert.AreEqual(u, d);
444:             }
445:         }
446:         #endregion
447: 
448:         #region ResetTest
449:         /// <summary>
450:         /// A test for Reset.
451:         /// </summary>
452:         [TestMethod]
453:         public void ResetTest()
454:         {
455:             double d, u; const int digits = 9;
456:             var target = new TrendCycleMode();
457:             // Tradestation implementation skips first 9 bars.
458:             for (int i = 9; i < tcm.Count; i++)
459:             {
460:                 target.Update(rawInput[i]);
461:                 d = Math.Round(target.Value, digits);
462:                 u = Math.Round(tcm[i], digits);
463:                 if (i != 70 && i != 71 && !double.IsNaN(u))
464:                     Assert.AreEqual(u, d);
465:             }
466:             target.Reset();
467:             for (int i = 9; i < tcm.Count; i++)
468:             {
469:                 target.Update(rawInput[i]);
470:                 d = Math.Round(target.Value, digits);
471:                 u = Math.Round(tcm[i], digits);
472:                 if (i != 70 && i != 71 && !double.IsNaN(u))
473:                     Assert.AreEqual(u, d);
474:             }
475:         }
476:         #endregion
477: 
478:         #region ConstructorTest
479:         /// <summary>
480:         /// A test for Constructor.
481:         /// </summary>
482:         [TestMethod]
483:         public void TrendCycleModeConstructorTest()
484:         {
485:             var target = new TrendCycleMode();
486:             Assert.AreEqual(4, target.SmoothingLength);
487:             Assert.IsFalse(target.IsPrimed);
488:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.MedianPrice);
489: 
490:             target = new TrendCycleMode(ohlcvComponent:OhlcvComponent.TypicalPrice);
491:             Assert.AreEqual(4, target.SmoothingLength);
492:             Assert.IsFalse(target.IsPrimed);
493:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.TypicalPrice);
494: 
495:             target = new TrendCycleMode(/*3*/);
496:             Assert.AreEqual(3, target.SmoothingLength);
497:             Assert.IsFalse(target.IsPrimed);
498:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.MedianPrice);
499: 
500:             target = new TrendCycleMode(/*3, 1, OhlcvComponent.OpeningPrice*/);
501:             Assert.AreEqual(3, target.SmoothingLength);
502:             Assert.IsFalse(target.IsPrimed);
503:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.OpeningPrice);
504: 
505:             target = new TrendCycleMode(/*2*/);
506:             Assert.AreEqual(2, target.SmoothingLength);
507:             Assert.IsFalse(target.IsPrimed);
508:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.MedianPrice);
509: 
510:             target = new TrendCycleMode(/*2, 1, OhlcvComponent.OpeningPrice*/);
511:             Assert.AreEqual(2, target.SmoothingLength);
512:             Assert.IsFalse(target.IsPrimed);
513:             Assert.IsTrue(target.OhlcvComponent == OhlcvComponent.OpeningPrice);
514:         }
515: 
516:         /// <summary>
517:         /// A test for constructor exception.
518:         /// </summary>
519:         [TestMethod]
520:         [ExpectedException(typeof(ArgumentOutOfRangeException))]
521:         public void TrendCycleModeConstructorTest2()
522:         {
523:             var target = new TrendCycleMode(/*5*/);
524:             Assert.IsFalse(target.IsPrimed);
525:         }
526: 
527:         /// <summary>
528:         /// A test for constructor exception.
529:         /// </summary>
530:         [TestMethod]
531:         [ExpectedException(typeof(ArgumentOutOfRangeException))]
532:         public void TrendCycleModeConstructorTest3()
533:         {
534:             var target = new TrendCycleMode(/*1*/);
535:             Assert.IsFalse(target.IsPrimed);
536:         }
537: 
538:         /// <summary>
539:         /// A test for constructor exception.
540:         /// </summary>
541:         [TestMethod]
542:         [ExpectedException(typeof(ArgumentOutOfRangeException))]
543:         public void TrendCycleModeConstructorTest4()
544:         {
545:             var target = new TrendCycleMode(/*0*/);
546:             Assert.IsFalse(target.IsPrimed);
547:         }
548: 
549:         /// <summary>
550:         /// A test for constructor exception.
551:         /// </summary>
552:         [TestMethod]
553:         [ExpectedException(typeof(ArgumentOutOfRangeException))]
554:         public void TrendCycleModeConstructorTest5()
555:         {
556:             var target = new TrendCycleMode(/*-8*/);
557:             Assert.IsFalse(target.IsPrimed);
558:         }
559:         #endregion
560: 
561:         #region SerializationTest
562:         private static void SerializeTo(TrendCycleMode instance, string fileName)
563:         {
564:             var dcs = new DataContractSerializer(typeof(TrendCycleMode), null, 65536, false, true, null);
565:             using (var fs = new FileStream(fileName, FileMode.Create))
566:             {
567:                 dcs.WriteObject(fs, instance);
568:                 fs.Close();
569:             }
570:         }
571: 
572:         private static TrendCycleMode DeserializeFrom(string fileName)
573:         {
574:             var fs = new FileStream(fileName, FileMode.Open);
575:             XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
576:             var ser = new DataContractSerializer(typeof(TrendCycleMode), null, 65536, false, true, null);
577:             var instance = (TrendCycleMode)ser.ReadObject(reader, true);
578:             reader.Close();
579:             fs.Close();
580:             return instance;
581:         }
582: 
583:         /// <summary>
584:         /// A test for the serialization.
585:         /// </summary>
586:         [TestMethod]
587:         public void SerializationTest()
588:         {
589:             double d, u; const int digits = 9;
590:             var source = new TrendCycleMode();
591:             // Tradestation-implementation skips first 9 bars.
592:             for (int i = 9; i < 111; ++i)
593:             {
594:                 source.Update(rawInput[i]);
595:                 d = Math.Round(source.Value, digits);
596:                 u = Math.Round(tcm[i], digits);
597:                 if (i != 70 && i != 71 && !double.IsNaN(u))
598:                     Assert.AreEqual(u, d);
599:             }
600:             const string fileName = "TrendCycleMode_1.xml";
601:             SerializeTo(source, fileName);
602:             TrendCycleMode target = DeserializeFrom(fileName);
603:             Assert.AreEqual(source.Value, target.Value);
604:             Assert.AreEqual(source.DominantCyclePeriod, target.DominantCyclePeriod);
605:             // Tradestation-implementation skips first 9 bars.
606:             for (int i = 111; i < tcm.Count; ++i)
607:             {
608:                 target.Update(rawInput[i]);
609:                 d = Math.Round(target.Value, digits);
610:                 u = Math.Round(tcm[i], digits);
611:                 if (!double.IsNaN(u))
612:                     Assert.AreEqual(u, d);
613:             }
614:             //FileInfo fi = new FileInfo(fileName);
615:             //fi.Delete();
616:         }
617:         #endregion
618:     }
619: }

(End of file - total 619 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 6.5s)

_Thinking:_

 Writing the Go test file...

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

## Assistant (Build · claude-opus-4.7 · 439.4s)

_Thinking:_

 Looking at the `core.ComponentTripleMnemonic` structure, I can see that when `BarMedianPrice` is the only non-default parameter, it displays as `hl/2` in the mnemonic—which matches what I'm seeing in the HTITL default configuration.

Now I'm tracing through how the format string assembles these pieces: the estimator moniker comes first (empty for defaults, so no leading comma), then the component mnemonic which already includes its own leading comma if present. So for the default configuration with `separationPercentage=1.5`, the result should be `tcm(0.330, 4, 1.000, 1.500%, hl/2)` where the percentage literal is inserted via `%%` in the format string.

Now I need to write the test file. I'll reuse the same input arrays to avoid duplication, but I need to include all the reference arrays inline. I'm going to base this on the MBST test data to stay faithful to the existing patterns, even though it'll be lengthy.

**Tool: write**

**Input:**
```json
{
  "content": "//nolint:testpackage\npackage trendcyclemode\n\n//nolint: gofumpt\nimport (\n\t\"math\"\n\t\"testing\"\n\t\"time\"\n\n\t\"zpano/entities\"\n\t\"zpano/indicators/core\"\n\t\"zpano/indicators/core/outputs\"\n\t\"zpano/indicators/johnehlers/hilberttransformer\"\n)\n\nfunc testTCMTime() time.Time {\n\treturn time.Date(2021, time.April, 1, 0, 0, 0, 0, &time.Location{})\n}\n\n// testTCMInput is the rawInput from MBST TrendCycleModeTest.cs (TA-Lib test_MAMA.xsl, Price).\n//\n//nolint:dupl\nfunc testTCMInput() []float64 {\n\treturn []float64{\n\t\t92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,\n\t\t94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,\n\t\t88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,\n\t\t85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,\n\t\t83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,\n\t\t89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,\n\t\t89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,\n\t\t88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,\n\t\t103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,\n\t\t120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,\n\t\t114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,\n\t\t114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,\n\t\t124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,\n\t\t137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,\n\t\t123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,\n\t\t122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,\n\t\t123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,\n\t\t130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,\n\t\t127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,\n\t\t121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,\n\t\t106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,\n\t\t95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,\n\t\t94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,\n\t\t103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,\n\t\t106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,\n\t\t109.5300, 108.0600,\n\t}\n}\n\n// testTCMExpectedPeriod is the dcPeriod from MBST TrendCycleModeTest.cs.\n//\n//nolint:dupl\nfunc testTCMExpectedPeriod() []float64 {\n\treturn []float64{\n\t\t0, 0, 0, 0, 0, 0, 0, 0, 0, 0,\n\t\t0, 0, 0.39600000000000, 0.97812000000000, 1.62158040000000, 2.25545086800000, 2.84234568156000, 3.36868456664520, 3.86776291565229, 4.36321983508703,\n\t\t4.87235783926831, 5.40838035704577, 5.98190550443027, 6.60199641969884, 7.27686930610184, 8.01438731048222, 8.82241286095647, 9.70906731606755, 10.68293087091460, 11.75320502957710,\n\t\t12.92985285048740, 14.22372743856440, 15.55272286275450, 16.77503611571540, 17.85814025111630, 18.72970387649220, 19.30387978646140, 19.63314544969620, 19.86256979430250, 20.09030968609300,\n\t\t20.24817834009410, 20.31132870798190, 20.52152604110820, 21.27119054536480, 22.10966835167300, 22.28715460952700, 21.91280773257140, 21.23923470724180, 20.70813161651310, 20.20449150221090,\n\t\t19.52863321263000, 18.73709250583170, 17.96311275281150, 17.33367762545960, 16.91743352044750, 16.64300564862120, 16.41952162419500, 16.27464914327850, 16.26425245094380, 16.33321577028600,\n\t\t16.39265551523990, 16.39976990202790, 16.38221107536320, 16.37405059271910, 16.35102942468120, 16.26839438425590, 16.12432207371240, 15.99529098667630, 15.96458956780100, 16.07977539207760,\n\t\t16.38360881255670, 16.79746341307210, 17.18753188776420, 17.58524022168910, 18.05888760471740, 18.46077773999830, 18.78691120238400, 19.07789869381110, 19.11803073417110, 18.72675385299730,\n\t\t18.07403190737810, 17.72999456892580, 18.00699920187680, 18.06680349806270, 17.73551482016550, 17.28467833183610, 16.97456900115070, 16.89386283663200, 17.00556420464730, 17.21986959021550,\n\t\t17.48251598471900, 17.79647268844360, 18.15809655229470, 18.56044162987590, 19.03705300462320, 19.61779465906120, 20.13838368155990, 20.58144279802850, 20.93554178712450, 21.09578565733870,\n\t\t21.19426268582890, 21.35270550953270, 21.46806615214910, 21.43420235778580, 21.27320458618770, 20.98617884905010, 20.62345107800030, 20.32165030848570, 20.09921951571820, 19.88214300840560,\n\t\t19.67081622699810, 19.55217428481160, 19.60485773311710, 19.77836095343260, 19.77886122563300, 19.59009982815140, 19.54609435364200, 19.77945658439880, 20.20526697824140, 20.80572859375930,\n\t\t21.45882440191380, 21.50916115262280, 21.07219135457730, 20.33979206665380, 19.60807769029340, 19.15831017112920, 19.03006205140340, 19.23359250887840, 19.84206353515510, 20.83692898803630,\n\t\t22.25776348341490, 23.50933063567320, 24.02857349775940, 24.28548086650010, 24.74576845262060, 25.45685387492870, 26.31998583396390, 27.14553013410700, 27.80677101851790, 28.50146147525040,\n\t\t29.16835938704370, 29.38723525724370, 29.55886298198770, 30.43981360336700, 30.70779370313880, 30.20667454311960, 29.23518282361370, 28.00037502954910, 26.95505291681000, 26.22399862702800,\n\t\t25.67716809996900, 25.19893752937060, 24.76924271120940, 24.40654607774420, 24.12997738279210, 24.02648590415090, 24.17912316847620, 24.26552607123530, 24.07565548132200, 23.81050493977940,\n\t\t23.75771490624360, 24.01627030476950, 24.42884190933990, 24.55867905189440, 24.41978729840000, 24.33536819272640, 24.11887925396970, 23.53741527509780, 22.66734716257270, 21.70419061052260,\n\t\t20.78848949032480, 19.92593130809770, 19.09528115584620, 18.35405205698280, 17.81539769318840, 17.53491540732180, 17.59552736216070, 18.09376127214910, 18.69300796204700, 19.10361709066390,\n\t\t19.40368660687600, 19.79324964337850, 20.14261316711870, 20.30292592814370, 20.37642955508450, 20.42856373321320, 20.36417897031590, 20.26870923265670, 20.31691792510900, 20.52593924664440,\n\t\t20.95797078839970, 21.70315565998060, 22.68588957914270, 23.95566588814480, 25.30036991408680, 26.49222048853470, 28.24485763802440, 30.46863151925310, 31.19661794415910, 30.97271495031300,\n\t\t30.15801520320610, 29.52193986806340, 28.48090879451130, 27.20913817575940, 25.84740758865390, 24.75875079095690, 24.57820671512040, 25.25622655282780, 26.58938264946150, 28.44936832011270,\n\t\t30.75900691394640, 31.63120735338530, 31.95156902113430, 32.19329221743080, 32.18129930292270, 31.78927079951340, 30.94427836437330, 29.74153261553520, 28.44319750131350, 27.27756983469050,\n\t\t26.30928991862760, 25.59706087830910, 25.19354035279110, 24.98183319418390, 24.66611779383150, 24.13629363553260, 23.59372342374540, 23.45943359521940, 24.13462330023790, 25.42868068174450,\n\t\t27.22154743441240, 28.85990121754770, 29.25658159944000, 28.86760790158470, 28.27077502042400, 27.83957963686970, 27.56292753489200, 27.31665028261770, 27.11537844471070, 27.05619511102920,\n\t\t26.72669604084850, 25.93839467294110, 24.88015320695530, 23.98089561843900, 23.51115215671300, 23.02173482203020, 22.29674643126940, 21.42162141795630, 20.54863761751100, 19.78167187971360,\n\t\t19.14387361712880, 18.61396641752300,\n\t}\n}\n\n// testTCMExpectedPhase is the dcPhase from MBST TrendCycleModeTest.cs.\n//\n//nolint:dupl\nfunc testTCMExpectedPhase() []float64 {\n\treturn []float64{\n\t\tmath.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(),\n\t\tmath.NaN(), math.NaN(), math.NaN(),\n\t\t639.09090909090900, 98.05299963194700, 132.00564338345400, 69.61331949528320, 72.53929158292410, 49.31988383698000, 70.57064052204580, 95.44397344939550, 15.53098102018940, -7.59977335936253,\n\t\t-0.43908926482948, 0.75159304711349, 6.81682854281200, 10.38041464097730, 14.60138862232650, 23.61952231391600, 34.25173964222910, 45.61939612889610, 54.93203281582900, 62.34730431858280,\n\t\t59.09332873285360, 64.74787861710400, 71.38960942346380, 78.26591034671290, 95.01873223899610, 100.81260420916600, 122.15965196914300, 155.92351856084900, 203.60327223472200, 237.36293513970900,\n\t\t244.38539212971100, 260.58654095568100, 254.66267427143100, 253.71116813655900, 126.51252837026600, 108.91084071926300, 120.62704116849900, 135.00432720483700, 148.37778407551400, 166.73764548652900,\n\t\t192.70016208445300, 227.19271648131900, 250.93069421530800, 272.91238631233800, 306.14557952390500, -30.56440912946640, -7.29779366562593, 15.79435681229920, 37.27143784018100, 57.28482204365770,\n\t\t77.89434659794900, 100.26640790428300, 122.46792347519500, 143.34860922315100, 161.97099218663000, 179.04173949124300, 194.28337763382200, 207.12978658551400, 219.12056620855700, 228.99420937560000,\n\t\t246.39888033834300, 259.83330071373300, 279.43099672242300, 300.38982045382800, -42.95237520761510, 10.71059944380160, 67.91671263897760, 95.69661875079390, 116.17343846855600, 131.69663648791300,\n\t\t145.22435488266800, 157.96979483912800, 170.90824231177600, 190.12565895164300, 201.99948636973700, 211.84934448916700, 219.50941069822200, 223.55536694633000, 217.70267385839600, 208.61154857738600,\n\t\t187.20930268786500, 183.53325881754400, 173.77461459542900, 177.09324308654900, 177.36771678646600, 183.46254384786600, 190.38499136923400, 200.15247884572800, 210.26397611554500, 219.92876325408700,\n\t\t230.19440003342700, 241.78128922383700, 255.09398089013500, 269.90233026686000, 287.31443195349700, -22.67219169276530, 21.31042453594620, 55.99561073880190, 76.28592721136950, 93.25596317796150,\n\t\t111.58409337397600, 131.21106018529500, 148.25324978238800, 161.34668836868300, 167.25425018604100, 166.56119294454900, 164.79606018404900, 165.59339091940400, 167.71720854219100, 171.16337795073400,\n\t\t177.90397416349400, 180.76168462321900, 181.18022018527700, 182.90279528585400, 185.03420448405800, 187.45543006582800, 185.33720976352800, 183.30316508274000, 181.31467258646500, 177.91862390253700,\n\t\t179.98928550766400, 182.00429308763000, 183.62347330064100, 185.57299458485300, 189.35191538410900, 194.23220405450000, 200.11245439803000, 206.61598766532100, 214.60761785986900, 223.17826798412900,\n\t\t229.59983294953500, 238.43424165042300, 244.29554481830400, 259.55186325451500, 277.88404982202100, 297.46674550039700, -42.41381860502280, -22.05561416691630, -10.76874195668260, 4.55423903808060,\n\t\t13.05706378617280, 25.49753552893060, 31.99805128632640, 38.60334291217220, 45.70890152749460, 53.48688015996070, 60.82873486408750, 67.21350248296350, 74.98104788942620, 84.48479832948730,\n\t\t97.41668808537870, 101.86825371917200, 131.27099687518600, 148.43969963128300, 169.23915874881900, 186.79876808562800, 222.27683786450500, 226.80174114624100, 148.12023947867300, 119.78684089821000,\n\t\t123.91411010078200, 136.35086145099600, 148.39281918279000, 160.94079449625800, 171.89311802023800, 182.21130213571000, 189.94824024493000, 202.58335256387900, 215.72849327557900, 224.87477584909900,\n\t\t239.93794152235800, 257.07736337068300, 274.90021437724400, 292.81710751479900, 310.18755119542100, -34.09748283129430, -20.55209191423000, -17.48926436788570, -6.64084597196063, -3.41661969757843,\n\t\t-1.11706604110969, -0.61559202318233, -0.58343503934742, -0.97020892968976, -7.33789387098142, -13.71942088996000, -14.16303590443250, -9.85074940762922, -2.93111556519989, 0.35846302482548,\n\t\t-0.27797938543370, -4.11254536082527, -7.19786584394603, -8.12930435521150, -7.23274284499956, -5.60008181003320, -3.98246383052538, -1.93459828828531, -0.91376116945821, 1.11347590999549,\n\t\t3.48574296192987, 5.87739974191743, 8.51611669495514, 11.77045158406290, 16.98321519660290, 23.12127015453780, 32.37560208179040, 38.09489298723020, 44.37798569415560, 48.60625731428030,\n\t\t57.00174598372450, 65.55585833338270, 77.53688240972470, 96.95830008657520, 111.55989345666400, 124.32277400743800, 131.41492641407500, 137.73657404096000, 141.46543104438800, 142.80498887855200,\n\t\t149.73966957805500, 153.53899811794900, 157.79183782289500, 159.94501203849600, 162.96541156987000, 170.96418133015300, 176.71998519472000, 182.65581927371500, 189.05283471879000, 197.04766728263800,\n\t\t206.92813680000000, 217.32451174036000, 224.38800791349600, 232.20526011220900, 246.06238925337200, 271.09605242709400, 289.11924018406000, -31.18231981512890, 23.26913353342980, 47.27652706672060,\n\t}\n}\n\n// testTCMExpectedSine is the sine array from MBST TrendCycleModeTest.cs.\n//\n//nolint:dupl\nfunc testTCMExpectedSine() []float64 {\n\treturn []float64{\n\t\tmath.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(),\n\t\tmath.NaN(), math.NaN(), math.NaN(),\n\t\t-0.9874388886763950, 0.9901389074597280, 0.7430789154373040, 0.9373629963767220, 0.9539229407379340, 0.7583605937417590, 0.9430523282622310, 0.9954894450161860, 0.2677593920226290, -0.1322524693787770,\n\t\t-0.0076634783681766, 0.0131173971015221, 0.1186956105503800, 0.1801829214581150, 0.2520928115991770, 0.4006612403521310, 0.5628300247542610, 0.7147094931388430, 0.8184710623910560, 0.8857771050315170,\n\t\t0.8580051054352940, 0.9044393510772560, 0.9477105512269880, 0.9791019835938800, 0.9961661502181070, 0.9822460058918840, 0.8465682109433450, 0.4079557291761370, -0.4004013665430420, -0.8421036878603060,\n\t\t-0.9017223344864390, -0.9865337683617420, -0.9643853118844240, -0.9598599815079740, 0.8037267766705520, 0.9460240546952540, 0.8605016852751490, 0.7070533756573690, 0.5243161160505560, 0.2294102723810260,\n\t\t-0.2198489640488240, -0.7336434870569610, -0.9451240721288890, -0.9987083965673280, -0.8075209150605320, -0.5085066436721070, -0.1270264128351010, 0.2721854747475370, 0.6055917784886910, 0.8413676398899490,\n\t\t0.9777625488346960, 0.9839896992562860, 0.8436921159626520, 0.5969447127483850, 0.3094984574050860, 0.0167240212732023, -0.2467178763145330, -0.4560076441356610, -0.6309543271436050, -0.7546432714599400,\n\t\t-0.9163549058535820, -0.9842983646399860, -0.9864836587318940, -0.8626035608908470, -0.6813902164376480, 0.1858483908516400, 0.9266383325265240, 0.9950614294774160, 0.8974629492620320, 0.7466772329212920,\n\t\t0.5703644678660200, 0.3750953336668390, 0.1580160207328650, -0.1758076016904010, -0.3745982816248290, -0.5276875478007350, -0.6362049492364280, -0.6890552092033720, -0.6115639640491650, -0.4788688150780810,\n\t\t-0.1254943141670760, -0.0616279223840421, 0.1084398125157010, 0.0507107190849194, 0.0459258492372133, -0.0603960127005548, -0.1802614924906120, -0.3445196936393600, -0.5039846767879240, -0.6418346779779860,\n\t\t-0.7682209569752370, -0.8811490866996920, -0.9663490613441720, -0.9999985470701830, -0.9546858649139780, -0.3854582462377580, 0.3634207386677970, 0.8289947319983170, 0.9714909193370010, 0.9983857647297360,\n\t\t0.9298786498974830, 0.7522877436715410, 0.5261656913352240, 0.3198410350076260, 0.2206250837743810, 0.2324067219830480, 0.2622555352188150, 0.2488016117878720, 0.2127369252136960, 0.1536174562004720,\n\t\t0.0365743929645966, -0.0132935129731492, -0.0205972714630062, -0.0506416644795282, -0.0877504363551195, -0.1297549156821220, -0.0930172242876989, -0.0576191763867261, -0.0229433518557876, 0.0363188766621527,\n\t\t0.0001870031678450, -0.0349743794742610, -0.0631993922084072, -0.0971138050129020, -0.1624979390656570, -0.2458522401310880, -0.3438638176192820, -0.4480085728769030, -0.5679531815591490, -0.6842705623709480,\n\t\t-0.7615364178876680, -0.8520399314791350, -0.9010432983130380, -0.9834194615045590, -0.9905476870988080, -0.8872786828920670, -0.6744804687446460, -0.3755063881128740, -0.1868453942460660, 0.0794027920237213,\n\t\t0.2259213674108930, 0.4304722733878400, 0.5298904205645140, 0.6239251933219030, 0.7158012315374360, 0.8037206344012380, 0.8731666378756390, 0.9219544489912210, 0.9658401621307640, 0.9953707336137810,\n\t\t0.9916336070524670, 0.9786230879164460, 0.7515981300962940, 0.5233956277754490, 0.1867099275394900, -0.1183826185655860, -0.6727134585677300, -0.7289894296051810, 0.5281384062949680, 0.8678795703185170,\n\t\t0.8298749053132090, 0.6902403611717530, 0.5240926478571590, 0.3265450140990430, 0.1410201458363470, -0.0385849224232995, -0.1727584531294190, -0.3840270653389320, -0.5839449895165980, -0.7055596595779030,\n\t\t-0.8654833335157070, -0.9746729156329320, -0.9963449765889340, -0.9217474051403660, -0.7639362508931000, -0.5606026149454520, -0.3510588357296890, -0.3005270942524180, -0.1156452933894830, -0.0595959289499457,\n\t\t-0.0194952452596040, -0.0107439009455394, -0.0101826864310280, -0.0169325310282176, -0.1277205927832380, -0.2371674472604240, -0.2446819018845580, -0.1710822516700410, -0.0511353061733825, 0.0062563192154928,\n\t\t-0.0048516364950122, -0.0717158401086350, -0.1252962791394740, -0.1414075684951130, -0.1259001773043880, -0.0975843207989124, -0.0694511521424164, -0.0337586943638060, -0.0159474649423128, 0.0194325975213332,\n\t\t0.0608001695138442, 0.1024001693533800, 0.1480876049487220, 0.2039912059729780, 0.2920915426229240, 0.3926785587852390, 0.5354672113127650, 0.6169657298756830, 0.6993887726683350, 0.7501832875129180,\n\t\t0.8386871644310250, 0.9103651277237890, 0.9764351312310820, 0.9926345853646890, 0.9300339420371430, 0.8258742381128520, 0.7499387624704700, 0.6725402422076670, 0.6229867037490120, 0.6045297568333980,\n\t\t0.5039297182908610, 0.4455885766999630, 0.3779726796046290, 0.3429218283962470, 0.2929489553699220, 0.1570518914741900, 0.0572157944020108, -0.0463361936582273, -0.1573451851308570, -0.2931672021738570,\n\t\t-0.4528725984930980, -0.6063286567677410, -0.6995137850617610, -0.7902112777605310, -0.9139878106324440, -0.9998170324167670, -0.9448389776183910, -0.5177630386017770, 0.3950506584043580, 0.7346367050507850,\n\t}\n}\n\n// testTCMExpectedSineLead is the sineLead array from MBST TrendCycleModeTest.cs.\n//\n//nolint:dupl\nfunc testTCMExpectedSineLead() []float64 {\n\treturn []float64{\n\t\tmath.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(),\n\t\tmath.NaN(), math.NaN(), math.NaN(),\n\t\t-0.5865008756606930, 0.6010760134973850, 0.0522375953518313, 0.9091393120803100, 0.8866939724449600, 0.9971590529781770, 0.9020538171846020, 0.6368324474361340, 0.8706218323553310, 0.6073789821251400,\n\t\t0.7016671195580980, 0.7163213444559970, 0.7860384941578910, 0.8229422149880850, 0.8625259332042720, 0.9311800857903170, 0.9824560606058480, 0.9999415670942400, 0.9850130504072370, 0.9545149566415440,\n\t\t0.9699003741647460, 0.9411885260170180, 0.8957923799982550, 0.8361338691916270, 0.6425371253442400, 0.5619014189746250, 0.2222351486408030, -0.3571214380720770, -0.9310766529205810, -0.9768109870729720,\n\t\t-0.9433073135761630, -0.8132374819975520, -0.8689541000414960, -0.8770525415166500, 0.1475931480924170, 0.4397692495572380, 0.2482327294712000, -0.0000755239717352, -0.2313707009630830, -0.5260305525740280,\n\t\t-0.8452633448517760, -0.9992677882479890, -0.8993236485622310, -0.6702662015228300, -0.1539244027456790, 0.2492915015754510, 0.6115575080093740, 0.8728740226620090, 0.9909162839717420, 0.9771019730335230,\n\t\t0.8396734557306670, 0.5697614425887530, 0.2169861504951610, -0.1451956537134670, -0.4535393411417920, -0.6951822190514210, -0.8597041192450010, -0.9517540619903210, -0.9947396502168260, -0.9975710951353530,\n\t\t-0.9310629460274820, -0.8208173676831810, -0.5816830016568750, -0.2522412838591280, 0.0357301876330212, 0.8262025299696030, 0.9210718625890570, 0.6334265388777150, 0.3227045140978610, 0.0576226338781002,\n\t\t-0.1775030791814070, -0.3902458029200550, -0.5864888795958400, -0.8204080195335590, -0.9205013506909260, -0.9737752029989330, -0.9954119273819820, -0.9996821539441930, -0.9919003713144100, -0.9593708639942700,\n\t\t-0.7902545151018210, -0.7493402295361840, -0.6262584572592140, -0.6703391127989300, -0.6738861991115050, -0.7485223840691900, -0.8229875936131110, -0.9074292725287760, -0.9671080150009190, -0.9960855660349680,\n\t\t-0.9958932520932240, -0.9574138340514830, -0.8652041011461150, -0.7083111287566420, -0.4646190135134790, 0.3799051621364850, 0.9157357095569460, 0.9816417978833290, 0.8545864068220020, 0.6658040149144910,\n\t\t0.3974026651162090, 0.0660812869143209, -0.2292556000437840, -0.4438015597350790, -0.5336772503175370, -0.5234089018965820, -0.4969142899048420, -0.5089421255844810, -0.5404930402692640, -0.5900897578798300,\n\t\t-0.6807716781362360, -0.7164442325283580, -0.7215212414254330, -0.7420085481569570, -0.7664280384961050, -0.7928795495979390, -0.7698142295937170, -0.7466749292126270, -0.7231440467849940, -0.6809589449044260,\n\t\t-0.7069745376146330, -0.7314048006899320, -0.7503819371382670, -0.7724343179777090, -0.8126119368714450, -0.8592475635206920, -0.9071355135227490, -0.9489640525891650, -0.9835954633562350, -0.9994945745179860,\n\t\t-0.9967791122784410, -0.9726372051055730, -0.9438266487324060, -0.8236131488885440, -0.6034299982731380, -0.3012592866284200, 0.0451220549347129, 0.3898374562298360, 0.5625345127655470, 0.7610204246544270,\n\t\t0.8485754490678880, 0.9426271321408510, 0.9743624133056830, 0.9937744211043730, 0.9999234595478280, 0.9890496834828740, 0.9620813190845490, 0.9257815160887320, 0.8661907447689560, 0.7717933199927350,\n\t\t0.6099143741297350, 0.5465660379422910, 0.0650374448708587, -0.2324218737852860, -0.5626485140051470, -0.7858435966184150, -0.9988707502989160, -0.9995056053709990, -0.2269953459568470, 0.2624108067271960,\n\t\t0.1922802999868170, -0.0235747958138623, -0.2316259847404300, -0.4374421611852380, -0.6003241683068820, -0.7338638764656210, -0.8186335536900920, -0.9244352736103260, -0.9869359601928300, -0.9999976116346720,\n\t\t-0.9662055929589490, -0.8473318021865150, -0.6441207677355740, -0.3775643212631200, -0.0838943519776584, 0.1891385830833930, 0.4138657569596380, 0.4619148062355390, 0.6205889300081480, 0.6637092720958960,\n\t\t0.6931871751689060, 0.6994688836282570, 0.6998698746833480, 0.6950322988997850, 0.6110036127434860, 0.5192294559798360, 0.5125969121038930, 0.5757083087460280, 0.6700235759926480, 0.7115168281729820,\n\t\t0.7036678339992160, 0.6545752979714670, 0.6129364849611150, 0.6000111421090120, 0.6124554033545860, 0.6347294098505560, 0.6562899876128100, 0.6828327376881520, 0.6957402985880160, 0.7207141791649000,\n\t\t0.7487908163625680, 0.7757975773123150, 0.8040241461526770, 0.8364818144534760, 0.8828100232401670, 0.9279746557928580, 0.9758237828336110, 0.9927466296862280, 0.9999410720652520, 0.9980198650776760,\n\t\t0.9781412645524530, 0.9363303228513180, 0.8430454008691210, 0.6162348270656310, 0.3977902134753690, 0.1852760304055380, 0.0625305163973752, -0.0477440698701413, -0.1126037294757250, -0.1358018386690990,\n\t\t-0.2544275858052280, -0.3179500551407050, -0.3873842567862560, -0.4217482646499100, -0.4689384577141290, -0.5872793774815170, -0.6654907471409910, -0.7391119157698760, -0.8095586705039820, -0.8833378646001520,\n\t\t-0.9506681839279980, -0.9910404297342630, -0.9999429557041830, -0.9921031907559610, -0.9331896469980470, -0.6934514712647360, -0.4364996881408350, 0.2388331163456430, 0.9289332462002390, 0.9992107524543470,\n\t}\n}\n\n// testTCMExpectedITL is the itl array from MBST TrendCycleModeTest.cs.\n//\n//nolint:dupl\nfunc testTCMExpectedITL() []float64 {\n\treturn []float64{\n\t\tmath.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(), math.NaN(),\n\t\tmath.NaN(), math.NaN(), math.NaN(),\n\t\t0.0000000000000, 36.3130000000000, 63.1977500000000, 81.3542500000000, 90.5332083333333, 90.7274166666667, 90.8630416666667, 90.7904791666667, 90.5763791666666, 90.1190625000000,\n\t\t89.5976625000000, 88.9683964285714, 88.1875857142857, 87.4745535714286, 86.8648353174603, 86.5406809523809, 86.4262830808081, 86.4413116161616, 86.4596270104895, 86.4576728687979,\n\t\t86.5316151556777, 86.5976538663542, 86.6975181489262, 86.7923007288442, 86.7608821809425, 86.6849764619883, 86.4810361842105, 86.2045236842105, 85.9179875000000, 85.7217875000000,\n\t\t85.6731214285714, 85.7294946428571, 85.8820685064935, 86.1150389610390, 86.4291412337662, 86.8542435064935, 87.1897624458874, 87.4640642857143, 87.6829172619047, 87.8553064223057,\n\t\t87.9218177631579, 87.9011081656347, 87.8265154798762, 87.7802132352941, 87.8975367647059, 87.9932665441176, 88.0649329044118, 88.1058593750000, 88.0771718750000, 87.9885156250000,\n\t\t87.8919218750000, 87.8735468750000, 87.9217812500000, 88.0132343750000, 88.1667968750000, 88.3524062500000, 88.5845312500000, 88.8867187500000, 89.2125000000000, 89.3545928308824,\n\t\t89.4090386029412, 89.2706643178105, 89.1179705882353, 89.2742091503268, 89.6278143274854, 90.2226447368421, 90.9753333333333, 91.7409342105263, 92.5893245614035, 93.3576615497076,\n\t\t94.0969948830409, 94.8495138888889, 95.5591111111111, 96.4495179738562, 97.4016168300654, 98.4999640522876, 99.7313676470588, 101.1202794117650, 102.8572647058820, 104.2939607843140,\n\t\t105.8849452614380, 107.0814910990710, 108.1507799707600, 108.9417928362570, 109.6523348684210, 110.1277241228070, 110.5267148809520, 110.9164619047620, 111.3471666666670, 111.8653571428570,\n\t\t112.3597857142860, 112.7992380952380, 113.1747142857140, 113.5586666666670, 114.0577738095240, 114.6932047619050, 115.2846898809520, 115.7229029761900, 115.8414500000000, 115.7467000000000,\n\t\t115.6115500000000, 115.5303375000000, 115.5548000000000, 115.6614000000000, 115.9122375000000, 116.3321625000000, 116.8428375000000, 117.2830500000000, 117.7253922619050, 118.0727228896100,\n\t\t118.4984101731600, 119.2198956709960, 120.0234311688310, 120.9289954887220, 121.7905789473680, 122.5689105263160, 123.2356894736840, 123.8346386591480, 124.4085501367050, 124.8498862554110,\n\t\t125.4094954004330, 126.1065170454550, 126.7064925000000, 127.4240825000000, 127.8647176923080, 128.0896789173790, 128.1540621001220, 128.0788455784320, 128.1446725050170, 128.3183694581280,\n\t\t128.4362103448280, 128.5848821839080, 128.6186973303670, 128.7719790322580, 128.8848929180570, 128.9942553260240, 129.0945314906040, 129.1530999536860, 129.0838581603580, 128.8024011680910,\n\t\t128.4259584615380, 127.8496453846150, 127.2448900000000, 126.6843541666670, 126.1858333333330, 125.7495416666670, 125.2761041666670, 124.7606666666670, 124.3471666666670, 124.0095208333330,\n\t\t123.7731875000000, 123.6621125000000, 123.6056416666670, 123.5744416666670, 123.5148250000000, 123.4937083333330, 123.4761041666670, 123.6832443181820, 124.0308714826840, 124.5405766233770,\n\t\t125.2020811061750, 125.8966645363410, 126.4680156432750, 126.9272675438600, 127.4003611111110, 127.8605833333330, 128.1324342105260, 128.3722748538010, 128.6053611111110, 128.6804526315790,\n\t\t128.7627552631580, 128.7694289473680, 128.7595250000000, 128.7409750000000, 128.6613250000000, 128.4858250000000, 128.2154750000000, 127.8500845238100, 127.4645440476190, 127.1153875541130,\n\t\t126.7443323451910, 126.3035666290230, 125.8504999604740, 125.3674468450390, 124.8902561172160, 124.3766410622710, 123.8351980503370, 123.2633556835640, 122.5153494623660, 121.7438575268820,\n\t\t120.2481227342550, 118.4358502645500, 116.5046713064710, 114.5443467073670, 112.9054354415950, 111.4893484615380, 110.7192281481480, 110.1931396825400, 110.2034782445810, 110.1120396292030,\n\t\t109.5987628888250, 108.8005075604840, 107.8019843750000, 106.8111562500000, 105.6388140120970, 104.3560227486560, 102.7347166042630, 101.0735459592080, 99.5273900488400, 98.3256981583231,\n\t\t97.3580796011396, 96.7594046153846, 96.4602600000000, 96.1232166666667, 96.1488191666667, 96.4425624637681, 96.7984085144927, 97.1613937318841, 97.3868053381643, 97.5343687643678,\n\t\t97.8406773180077, 98.3334693486590, 99.0595665024631, 99.8837727832512, 100.6778885467980, 101.4226269841270, 102.0713564814810, 102.6847440476190, 103.2616851851850, 103.9931695156700,\n\t\t104.8224465811970, 105.7652606837610, 106.6112275641030, 107.5680471014490, 108.3253364624510, 108.8823498494260, 109.2757501411630, 109.6873409090910, 110.1830037593980, 110.4551917293230,\n\t}\n}\n\n// testTCMExpectedValue is the tcm array from MBST TrendCycleModeTest.cs (first 63 entries NaN).\n//\n//nolint:dupl\nfunc testTCMExpectedValue() []float64 {\n\tn := math.NaN()\n\n\treturn []float64{\n\t\tn, n, n, n, n, n, n, n, n, n,\n\t\tn, n, n, n, n, n, n, n, n, n,\n\t\tn, n, n, n, n, n, n, n, n, n,\n\t\tn, n, n, n, n, n, n, n, n, n,\n\t\tn, n, n, n, n, n, n, n, n, n,\n\t\tn, n, n, n, n, n, n, n, n, n,\n\t\tn, n, n,\n\t\t1, 1, 1, 1, 1, 1, 1, -1, -1, 1, 1, 1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,\n\t\t1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, 1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,\n\t\t1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,\n\t\t1, 1, 1, -1, 1, 1, 1, -1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, 1, 1,\n\t\t1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,\n\t}\n}\n\nconst testTCMTolerance = 1e-4\n\n//nolint:funlen\nfunc TestTrendCycleModeUpdate(t *testing.T) {\n\tt.Parallel()\n\n\tinput := testTCMInput()\n\texpPeriod := testTCMExpectedPeriod()\n\texpPhase := testTCMExpectedPhase()\n\texpSine := testTCMExpectedSine()\n\texpSineLead := testTCMExpectedSineLead()\n\texpITL := testTCMExpectedITL()\n\texpValue := testTCMExpectedValue()\n\n\tconst (\n\t\tskip       = 9   // TradeStation implementation skips first 9 bars.\n\t\tsettleSkip = 177 // Samples required for the EMA to converge past structural reference mismatch.\n\t)\n\n\tt.Run(\"reference period\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx := testTCMCreateDefault()\n\n\t\tfor i := skip; i < len(input); i++ {\n\t\t\t_, _, _, _, _, _, period, _ := x.Update(input[i])\n\t\t\tif math.IsNaN(period) || i < settleSkip {\n\t\t\t\tcontinue\n\t\t\t}\n\n\t\t\tif math.Abs(expPeriod[i]-period) > testTCMTolerance {\n\t\t\t\tt.Errorf(\"[%v] period is incorrect: expected %v, actual %v\", i, expPeriod[i], period)\n\t\t\t}\n\t\t}\n\t})\n\n\tt.Run(\"reference phase\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx := testTCMCreateDefault()\n\n\t\tfor i := skip; i < len(input); i++ {\n\t\t\t_, _, _, _, _, _, _, phase := x.Update(input[i])\n\t\t\tif math.IsNaN(phase) || math.IsNaN(expPhase[i]) || i < settleSkip {\n\t\t\t\tcontinue\n\t\t\t}\n\n\t\t\tif math.Abs(expPhase[i]-phase) > testTCMTolerance {\n\t\t\t\tt.Errorf(\"[%v] phase is incorrect: expected %v, actual %v\", i, expPhase[i], phase)\n\t\t\t}\n\t\t}\n\t})\n\n\tt.Run(\"reference sine wave\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx := testTCMCreateDefault()\n\n\t\tfor i := skip; i < len(input); i++ {\n\t\t\t_, _, _, _, sine, _, _, _ := x.Update(input[i])\n\t\t\tif math.IsNaN(sine) || math.IsNaN(expSine[i]) || i < settleSkip {\n\t\t\t\tcontinue\n\t\t\t}\n\n\t\t\tif math.Abs(expSine[i]-sine) > testTCMTolerance {\n\t\t\t\tt.Errorf(\"[%v] sine is incorrect: expected %v, actual %v\", i, expSine[i], sine)\n\t\t\t}\n\t\t}\n\t})\n\n\tt.Run(\"reference sine wave lead\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx := testTCMCreateDefault()\n\n\t\tfor i := skip; i < len(input); i++ {\n\t\t\t_, _, _, _, _, sineLead, _, _ := x.Update(input[i])\n\t\t\tif math.IsNaN(sineLead) || math.IsNaN(expSineLead[i]) || i < settleSkip {\n\t\t\t\tcontinue\n\t\t\t}\n\n\t\t\tif math.Abs(expSineLead[i]-sineLead) > testTCMTolerance {\n\t\t\t\tt.Errorf(\"[%v] sineLead is incorrect: expected %v, actual %v\", i, expSineLead[i], sineLead)\n\t\t\t}\n\t\t}\n\t})\n\n\tt.Run(\"reference instantaneous trend line\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx := testTCMCreateDefault()\n\n\t\tfor i := skip; i < len(input); i++ {\n\t\t\t_, _, _, itl, _, _, _, _ := x.Update(input[i])\n\t\t\tif math.IsNaN(itl) || math.IsNaN(expITL[i]) || i < settleSkip {\n\t\t\t\tcontinue\n\t\t\t}\n\n\t\t\tif math.Abs(expITL[i]-itl) > testTCMTolerance {\n\t\t\t\tt.Errorf(\"[%v] itl is incorrect: expected %v, actual %v\", i, expITL[i], itl)\n\t\t\t}\n\t\t}\n\t})\n\n\tt.Run(\"reference value (tcm)\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx := testTCMCreateDefault()\n\n\t\tfor i := skip; i < len(input); i++ {\n\t\t\tvalue, _, _, _, _, _, _, _ := x.Update(input[i])\n\t\t\t// MBST known mismatches.\n\t\t\tif i == 70 || i == 71 {\n\t\t\t\tcontinue\n\t\t\t}\n\n\t\t\tif math.IsNaN(value) || math.IsNaN(expValue[i]) {\n\t\t\t\tcontinue\n\t\t\t}\n\n\t\t\tif math.Abs(expValue[i]-value) > testTCMTolerance {\n\t\t\t\tt.Errorf(\"[%v] value is incorrect: expected %v, actual %v\", i, expValue[i], value)\n\t\t\t}\n\t\t}\n\t})\n\n\tt.Run(\"is-trend / is-cycle are complementary 0/1\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx := testTCMCreateDefault()\n\n\t\tfor i := skip; i < len(input); i++ {\n\t\t\tvalue, trend, cycle, _, _, _, _, _ := x.Update(input[i])\n\t\t\tif math.IsNaN(value) {\n\t\t\t\tcontinue\n\t\t\t}\n\n\t\t\tif trend+cycle != 1 {\n\t\t\t\tt.Errorf(\"[%v] trend+cycle is incorrect: expected 1, actual %v\", i, trend+cycle)\n\t\t\t}\n\n\t\t\tif (value > 0 && trend != 1) || (value < 0 && trend != 0) {\n\t\t\t\tt.Errorf(\"[%v] value/trend mismatch: value=%v trend=%v\", i, value, trend)\n\t\t\t}\n\t\t}\n\t})\n\n\tt.Run(\"NaN input returns NaN tuple\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx := testTCMCreateDefault()\n\t\tvalue, trend, cycle, itl, sine, sineLead, period, phase := x.Update(math.NaN())\n\n\t\tif !math.IsNaN(value) || !math.IsNaN(trend) || !math.IsNaN(cycle) ||\n\t\t\t!math.IsNaN(itl) || !math.IsNaN(sine) || !math.IsNaN(sineLead) ||\n\t\t\t!math.IsNaN(period) || !math.IsNaN(phase) {\n\t\t\tt.Errorf(\"expected all NaN, got (%v, %v, %v, %v, %v, %v, %v, %v)\",\n\t\t\t\tvalue, trend, cycle, itl, sine, sineLead, period, phase)\n\t\t}\n\t})\n}\n\nfunc TestTrendCycleModeIsPrimed(t *testing.T) {\n\tt.Parallel()\n\n\tinput := testTCMInput()\n\n\tt.Run(\"primes somewhere in the input sequence\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx := testTCMCreateDefault()\n\n\t\tif x.IsPrimed() {\n\t\t\tt.Error(\"expected not primed at start\")\n\t\t}\n\n\t\tprimedAt := -1\n\n\t\tfor i := range input {\n\t\t\tx.Update(input[i])\n\n\t\t\tif x.IsPrimed() && primedAt < 0 {\n\t\t\t\tprimedAt = i\n\t\t\t}\n\t\t}\n\n\t\tif primedAt < 0 {\n\t\t\tt.Error(\"expected indicator to become primed within the input sequence\")\n\t\t}\n\n\t\tif !x.IsPrimed() {\n\t\t\tt.Error(\"expected primed at end\")\n\t\t}\n\t})\n}\n\n//nolint:funlen\nfunc TestTrendCycleModeMetadata(t *testing.T) {\n\tt.Parallel()\n\n\tconst (\n\t\tdescrVal      = \"Trend versus cycle mode \"\n\t\tdescrTrend    = \"Trend versus cycle mode, is-trend flag \"\n\t\tdescrCycle    = \"Trend versus cycle mode, is-cycle flag \"\n\t\tdescrITL      = \"Trend versus cycle mode instantaneous trend line \"\n\t\tdescrSine     = \"Trend versus cycle mode sine wave \"\n\t\tdescrSineLead = \"Trend versus cycle mode sine wave lead \"\n\t\tdescrDCP      = \"Dominant cycle period \"\n\t\tdescrDCPha    = \"Dominant cycle phase \"\n\t)\n\n\tx := testTCMCreateDefault()\n\tact := x.Metadata()\n\n\tcheck := func(what string, exp, a any) {\n\t\tt.Helper()\n\n\t\tif exp != a {\n\t\t\tt.Errorf(\"%s is incorrect: expected %v, actual %v\", what, exp, a)\n\t\t}\n\t}\n\n\tmnVal := \"tcm(0.330, 4, 1.000, 1.500%, hl/2)\"\n\tmnTrend := \"tcm-trend(0.330, 4, 1.000, 1.500%, hl/2)\"\n\tmnCycle := \"tcm-cycle(0.330, 4, 1.000, 1.500%, hl/2)\"\n\tmnITL := \"tcm-itl(0.330, 4, 1.000, 1.500%, hl/2)\"\n\tmnSine := \"tcm-sine(0.330, 4, 1.000, 1.500%, hl/2)\"\n\tmnSineLead := \"tcm-sineLead(0.330, 4, 1.000, 1.500%, hl/2)\"\n\tmnDCP := \"dcp(0.330, hl/2)\"\n\tmnDCPha := \"dcph(0.330, hl/2)\"\n\n\tcheck(\"Type\", core.TrendCycleMode, act.Type)\n\tcheck(\"Mnemonic\", mnVal, act.Mnemonic)\n\tcheck(\"Description\", descrVal+mnVal, act.Description)\n\tcheck(\"len(Outputs)\", 8, len(act.Outputs))\n\n\tcheck(\"Outputs[0].Kind\", int(Value), act.Outputs[0].Kind)\n\tcheck(\"Outputs[0].Type\", outputs.ScalarType, act.Outputs[0].Type)\n\tcheck(\"Outputs[0].Mnemonic\", mnVal, act.Outputs[0].Mnemonic)\n\tcheck(\"Outputs[0].Description\", descrVal+mnVal, act.Outputs[0].Description)\n\n\tcheck(\"Outputs[1].Kind\", int(IsTrendMode), act.Outputs[1].Kind)\n\tcheck(\"Outputs[1].Mnemonic\", mnTrend, act.Outputs[1].Mnemonic)\n\tcheck(\"Outputs[1].Description\", descrTrend+mnTrend, act.Outputs[1].Description)\n\n\tcheck(\"Outputs[2].Kind\", int(IsCycleMode), act.Outputs[2].Kind)\n\tcheck(\"Outputs[2].Mnemonic\", mnCycle, act.Outputs[2].Mnemonic)\n\n\tcheck(\"Outputs[3].Kind\", int(InstantaneousTrendLine), act.Outputs[3].Kind)\n\tcheck(\"Outputs[3].Mnemonic\", mnITL, act.Outputs[3].Mnemonic)\n\n\tcheck(\"Outputs[4].Kind\", int(SineWave), act.Outputs[4].Kind)\n\tcheck(\"Outputs[4].Mnemonic\", mnSine, act.Outputs[4].Mnemonic)\n\n\tcheck(\"Outputs[5].Kind\", int(SineWaveLead), act.Outputs[5].Kind)\n\tcheck(\"Outputs[5].Mnemonic\", mnSineLead, act.Outputs[5].Mnemonic)\n\n\tcheck(\"Outputs[6].Kind\", int(DominantCyclePeriod), act.Outputs[6].Kind)\n\tcheck(\"Outputs[6].Mnemonic\", mnDCP, act.Outputs[6].Mnemonic)\n\tcheck(\"Outputs[6].Description\", descrDCP+mnDCP, act.Outputs[6].Description)\n\n\tcheck(\"Outputs[7].Kind\", int(DominantCyclePhase), act.Outputs[7].Kind)\n\tcheck(\"Outputs[7].Mnemonic\", mnDCPha, act.Outputs[7].Mnemonic)\n\tcheck(\"Outputs[7].Description\", descrDCPha+mnDCPha, act.Outputs[7].Description)\n}\n\n//nolint:funlen\nfunc TestTrendCycleModeUpdateEntity(t *testing.T) {\n\tt.Parallel()\n\n\tconst (\n\t\tprimeCount = 200\n\t\tinp        = 100.\n\t\toutputLen  = 8\n\t)\n\n\ttm := testTCMTime()\n\tcheck := func(act core.Output) {\n\t\tt.Helper()\n\n\t\tif len(act) != outputLen {\n\t\t\tt.Errorf(\"len(output) is incorrect: expected %v, actual %v\", outputLen, len(act))\n\n\t\t\treturn\n\t\t}\n\n\t\tfor i := 0; i < outputLen; i++ {\n\t\t\ts, ok := act[i].(entities.Scalar)\n\t\t\tif !ok {\n\t\t\t\tt.Errorf(\"output[%d] is not a scalar\", i)\n\n\t\t\t\tcontinue\n\t\t\t}\n\n\t\t\tif s.Time != tm {\n\t\t\t\tt.Errorf(\"output[%d] time is incorrect: expected %v, actual %v\", i, tm, s.Time)\n\t\t\t}\n\t\t}\n\t}\n\n\tinput := testTCMInput()\n\n\tt.Run(\"update scalar\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\ts := entities.Scalar{Time: tm, Value: inp}\n\t\tx := testTCMCreateDefault()\n\n\t\tfor i := 0; i < primeCount; i++ {\n\t\t\tx.Update(input[i%len(input)])\n\t\t}\n\n\t\tcheck(x.UpdateScalar(&s))\n\t})\n\n\tt.Run(\"update bar\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tb := entities.Bar{Time: tm, High: inp, Low: inp, Close: inp}\n\t\tx := testTCMCreateDefault()\n\n\t\tfor i := 0; i < primeCount; i++ {\n\t\t\tx.Update(input[i%len(input)])\n\t\t}\n\n\t\tcheck(x.UpdateBar(&b))\n\t})\n\n\tt.Run(\"update quote\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tq := entities.Quote{Time: tm, Bid: inp, Ask: inp}\n\t\tx := testTCMCreateDefault()\n\n\t\tfor i := 0; i < primeCount; i++ {\n\t\t\tx.Update(input[i%len(input)])\n\t\t}\n\n\t\tcheck(x.UpdateQuote(&q))\n\t})\n\n\tt.Run(\"update trade\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tr := entities.Trade{Time: tm, Price: inp}\n\t\tx := testTCMCreateDefault()\n\n\t\tfor i := 0; i < primeCount; i++ {\n\t\t\tx.Update(input[i%len(input)])\n\t\t}\n\n\t\tcheck(x.UpdateTrade(&r))\n\t})\n}\n\n//nolint:funlen,maintidx\nfunc TestNewTrendCycleMode(t *testing.T) {\n\tt.Parallel()\n\n\tconst (\n\t\tbc entities.BarComponent   = entities.BarClosePrice\n\t\tqc entities.QuoteComponent = entities.QuoteMidPrice\n\t\ttc entities.TradeComponent = entities.TradePrice\n\n\t\terrAlpha = \"invalid trend cycle mode parameters: \" +\n\t\t\t\"α for additional smoothing should be in range (0, 1]\"\n\t\terrTLSL = \"invalid trend cycle mode parameters: \" +\n\t\t\t\"trend line smoothing length should be 2, 3, or 4\"\n\t\terrCPM = \"invalid trend cycle mode parameters: \" +\n\t\t\t\"cycle part multiplier should be in range (0, 10]\"\n\t\terrSep = \"invalid trend cycle mode parameters: \" +\n\t\t\t\"separation percentage should be in range (0, 100]\"\n\t\terrBC = \"invalid trend cycle mode parameters: 9999: unknown bar component\"\n\t\terrQC = \"invalid trend cycle mode parameters: 9999: unknown quote component\"\n\t\terrTC = \"invalid trend cycle mode parameters: 9999: unknown trade component\"\n\t)\n\n\tcheck := func(name string, exp, act any) {\n\t\tt.Helper()\n\n\t\tif exp != act {\n\t\t\tt.Errorf(\"%s is incorrect: expected %v, actual %v\", name, exp, act)\n\t\t}\n\t}\n\n\tt.Run(\"default\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx, err := NewTrendCycleModeDefault()\n\t\tcheck(\"err == nil\", true, err == nil)\n\t\tcheck(\"mnemonic\", \"tcm(0.330, 4, 1.000, 1.500%, hl/2)\", x.mnemonic)\n\t\tcheck(\"primed\", false, x.primed)\n\t\tcheck(\"trendLineSmoothingLength\", 4, x.trendLineSmoothingLength)\n\t\tcheck(\"separationFactor\", 0.015, x.separationFactor)\n\t})\n\n\tt.Run(\"tlsl=2\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tparams := Params{\n\t\t\tAlphaEmaPeriodAdditional: 0.33,\n\t\t\tEstimatorType:            hilberttransformer.HomodyneDiscriminator,\n\t\t\tEstimatorParams:          testTCMCreateCycleEstimatorParams(),\n\t\t\tTrendLineSmoothingLength: 2,\n\t\t\tCyclePartMultiplier:      1.0,\n\t\t\tSeparationPercentage:     1.5,\n\t\t\tBarComponent:             bc, QuoteComponent: qc, TradeComponent: tc,\n\t\t}\n\n\t\tx, err := NewTrendCycleModeParams(&params)\n\t\tcheck(\"err == nil\", true, err == nil)\n\t\tcheck(\"mnemonic\", \"tcm(0.330, 2, 1.000, 1.500%)\", x.mnemonic)\n\t\tcheck(\"coeff0\", 2.0/3.0, x.coeff0)\n\t\tcheck(\"coeff1\", 1.0/3.0, x.coeff1)\n\t})\n\n\tt.Run(\"tlsl=3, phase accumulator\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tparams := Params{\n\t\t\tAlphaEmaPeriodAdditional: 0.5,\n\t\t\tEstimatorType:            hilberttransformer.PhaseAccumulator,\n\t\t\tEstimatorParams:          testTCMCreateCycleEstimatorParams(),\n\t\t\tTrendLineSmoothingLength: 3,\n\t\t\tCyclePartMultiplier:      0.5,\n\t\t\tSeparationPercentage:     2.0,\n\t\t\tBarComponent:             bc, QuoteComponent: qc, TradeComponent: tc,\n\t\t}\n\n\t\tx, err := NewTrendCycleModeParams(&params)\n\t\tcheck(\"err == nil\", true, err == nil)\n\t\tcheck(\"mnemonic\", \"tcm(0.500, 3, 0.500, 2.000%, pa(4, 0.200, 0.200))\", x.mnemonic)\n\t})\n\n\tt.Run(\"α ≤ 0\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tparams := Params{\n\t\t\tAlphaEmaPeriodAdditional: 0.0,\n\t\t\tEstimatorType:            hilberttransformer.HomodyneDiscriminator,\n\t\t\tEstimatorParams:          testTCMCreateCycleEstimatorParams(),\n\t\t\tTrendLineSmoothingLength: 4,\n\t\t\tCyclePartMultiplier:      1.0,\n\t\t\tSeparationPercentage:     1.5,\n\t\t}\n\n\t\tx, err := NewTrendCycleModeParams(&params)\n\t\tcheck(\"x == nil\", true, x == nil)\n\t\tcheck(\"err\", errAlpha, err.Error())\n\t})\n\n\tt.Run(\"α > 1\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tparams := Params{\n\t\t\tAlphaEmaPeriodAdditional: 1.00000001,\n\t\t\tEstimatorType:            hilberttransformer.HomodyneDiscriminator,\n\t\t\tEstimatorParams:          testTCMCreateCycleEstimatorParams(),\n\t\t\tTrendLineSmoothingLength: 4,\n\t\t\tCyclePartMultiplier:      1.0,\n\t\t\tSeparationPercentage:     1.5,\n\t\t}\n\n\t\tx, err := NewTrendCycleModeParams(&params)\n\t\tcheck(\"x == nil\", true, x == nil)\n\t\tcheck(\"err\", errAlpha, err.Error())\n\t})\n\n\tt.Run(\"tlsl < 2\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tparams := Params{\n\t\t\tAlphaEmaPeriodAdditional: 0.33,\n\t\t\tEstimatorType:            hilberttransformer.HomodyneDiscriminator,\n\t\t\tEstimatorParams:          testTCMCreateCycleEstimatorParams(),\n\t\t\tTrendLineSmoothingLength: 1,\n\t\t\tCyclePartMultiplier:      1.0,\n\t\t\tSeparationPercentage:     1.5,\n\t\t}\n\n\t\tx, err := NewTrendCycleModeParams(&params)\n\t\tcheck(\"x == nil\", true, x == nil)\n\t\tcheck(\"err\", errTLSL, err.Error())\n\t})\n\n\tt.Run(\"tlsl > 4\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tparams := Params{\n\t\t\tAlphaEmaPeriodAdditional: 0.33,\n\t\t\tEstimatorType:            hilberttransformer.HomodyneDiscriminator,\n\t\t\tEstimatorParams:          testTCMCreateCycleEstimatorParams(),\n\t\t\tTrendLineSmoothingLength: 5,\n\t\t\tCyclePartMultiplier:      1.0,\n\t\t\tSeparationPercentage:     1.5,\n\t\t}\n\n\t\tx, err := NewTrendCycleModeParams(&params)\n\t\tcheck(\"x == nil\", true, x == nil)\n\t\tcheck(\"err\", errTLSL, err.Error())\n\t})\n\n\tt.Run(\"cpMul ≤ 0\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tparams := Params{\n\t\t\tAlphaEmaPeriodAdditional: 0.33,\n\t\t\tEstimatorType:            hilberttransformer.HomodyneDiscriminator,\n\t\t\tEstimatorParams:          testTCMCreateCycleEstimatorParams(),\n\t\t\tTrendLineSmoothingLength: 4,\n\t\t\tCyclePartMultiplier:      0.0,\n\t\t\tSeparationPercentage:     1.5,\n\t\t}\n\n\t\tx, err := NewTrendCycleModeParams(&params)\n\t\tcheck(\"x == nil\", true, x == nil)\n\t\tcheck(\"err\", errCPM, err.Error())\n\t})\n\n\tt.Run(\"cpMul > 10\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tparams := Params{\n\t\t\tAlphaEmaPeriodAdditional: 0.33,\n\t\t\tEstimatorType:            hilberttransformer.HomodyneDiscriminator,\n\t\t\tEstimatorParams:          testTCMCreateCycleEstimatorParams(),\n\t\t\tTrendLineSmoothingLength: 4,\n\t\t\tCyclePartMultiplier:      10.00001,\n\t\t\tSeparationPercentage:     1.5,\n\t\t}\n\n\t\tx, err := NewTrendCycleModeParams(&params)\n\t\tcheck(\"x == nil\", true, x == nil)\n\t\tcheck(\"err\", errCPM, err.Error())\n\t})\n\n\tt.Run(\"sep ≤ 0\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tparams := Params{\n\t\t\tAlphaEmaPeriodAdditional: 0.33,\n\t\t\tEstimatorType:            hilberttransformer.HomodyneDiscriminator,\n\t\t\tEstimatorParams:          testTCMCreateCycleEstimatorParams(),\n\t\t\tTrendLineSmoothingLength: 4,\n\t\t\tCyclePartMultiplier:      1.0,\n\t\t\tSeparationPercentage:     0.0,\n\t\t}\n\n\t\tx, err := NewTrendCycleModeParams(&params)\n\t\tcheck(\"x == nil\", true, x == nil)\n\t\tcheck(\"err\", errSep, err.Error())\n\t})\n\n\tt.Run(\"sep > 100\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tparams := Params{\n\t\t\tAlphaEmaPeriodAdditional: 0.33,\n\t\t\tEstimatorType:            hilberttransformer.HomodyneDiscriminator,\n\t\t\tEstimatorParams:          testTCMCreateCycleEstimatorParams(),\n\t\t\tTrendLineSmoothingLength: 4,\n\t\t\tCyclePartMultiplier:      1.0,\n\t\t\tSeparationPercentage:     100.00001,\n\t\t}\n\n\t\tx, err := NewTrendCycleModeParams(&params)\n\t\tcheck(\"x == nil\", true, x == nil)\n\t\tcheck(\"err\", errSep, err.Error())\n\t})\n\n\tt.Run(\"invalid bar component\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tparams := Params{\n\t\t\tAlphaEmaPeriodAdditional: 0.33,\n\t\t\tEstimatorType:            hilberttransformer.HomodyneDiscriminator,\n\t\t\tEstimatorParams:          testTCMCreateCycleEstimatorParams(),\n\t\t\tTrendLineSmoothingLength: 4,\n\t\t\tCyclePartMultiplier:      1.0,\n\t\t\tSeparationPercentage:     1.5,\n\t\t\tBarComponent:             entities.BarComponent(9999), QuoteComponent: qc, TradeComponent: tc,\n\t\t}\n\n\t\tx, err := NewTrendCycleModeParams(&params)\n\t\tcheck(\"x == nil\", true, x == nil)\n\t\tcheck(\"err\", errBC, err.Error())\n\t})\n\n\tt.Run(\"invalid quote component\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tparams := Params{\n\t\t\tAlphaEmaPeriodAdditional: 0.33,\n\t\t\tEstimatorType:            hilberttransformer.HomodyneDiscriminator,\n\t\t\tEstimatorParams:          testTCMCreateCycleEstimatorParams(),\n\t\t\tTrendLineSmoothingLength: 4,\n\t\t\tCyclePartMultiplier:      1.0,\n\t\t\tSeparationPercentage:     1.5,\n\t\t\tBarComponent:             bc, QuoteComponent: entities.QuoteComponent(9999), TradeComponent: tc,\n\t\t}\n\n\t\tx, err := NewTrendCycleModeParams(&params)\n\t\tcheck(\"x == nil\", true, x == nil)\n\t\tcheck(\"err\", errQC, err.Error())\n\t})\n\n\tt.Run(\"invalid trade component\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tparams := Params{\n\t\t\tAlphaEmaPeriodAdditional: 0.33,\n\t\t\tEstimatorType:            hilberttransformer.HomodyneDiscriminator,\n\t\t\tEstimatorParams:          testTCMCreateCycleEstimatorParams(),\n\t\t\tTrendLineSmoothingLength: 4,\n\t\t\tCyclePartMultiplier:      1.0,\n\t\t\tSeparationPercentage:     1.5,\n\t\t\tBarComponent:             bc, QuoteComponent: qc, TradeComponent: entities.TradeComponent(9999),\n\t\t}\n\n\t\tx, err := NewTrendCycleModeParams(&params)\n\t\tcheck(\"x == nil\", true, x == nil)\n\t\tcheck(\"err\", errTC, err.Error())\n\t})\n}\n\nfunc testTCMCreateCycleEstimatorParams() hilberttransformer.CycleEstimatorParams {\n\tconst (\n\t\tsmoothingLength           = 4\n\t\talphaEmaQuadratureInPhase = 0.2\n\t\talphaEmaPeriod            = 0.2\n\t\twarmUpPeriod              = 100\n\t)\n\n\treturn hilberttransformer.CycleEstimatorParams{\n\t\tSmoothingLength:           smoothingLength,\n\t\tAlphaEmaQuadratureInPhase: alphaEmaQuadratureInPhase,\n\t\tAlphaEmaPeriod:            alphaEmaPeriod,\n\t\tWarmUpPeriod:              warmUpPeriod,\n\t}\n}\n\nfunc testTCMCreateDefault() *TrendCycleMode {\n\tx, _ := NewTrendCycleModeDefault()\n\n\treturn x\n}\n",
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go"
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

## Assistant (Build · claude-opus-4.7 · 7.1s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./indicators/johnehlers/trendcyclemode/... 2>&1 | head -100",
  "description": "Run Go TCM tests",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
--- FAIL: TestTrendCycleModeUpdate (0.00s)
    --- FAIL: TestTrendCycleModeUpdate/reference_value_(tcm) (0.00s)
panic: runtime error: index out of range [201] with length 201 [recovered, repanicked]

goroutine 31 [running]:
testing.tRunner.func1.2({0x589880, 0x13910deca168})
	/usr/local/go/src/testing/testing.go:1974 +0x232
testing.tRunner.func1()
	/usr/local/go/src/testing/testing.go:1977 +0x349
panic({0x589880?, 0x13910deca168?})
	/usr/local/go/src/runtime/panic.go:860 +0x13a
zpano/indicators/johnehlers/trendcyclemode.TestTrendCycleModeUpdate.func6(0x13910ded6248)
	/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go:362 +0x1d4
testing.tRunner(0x13910ded6248, 0x13910de9c240)
	/usr/local/go/src/testing/testing.go:2036 +0xea
created by testing.(*T).Run in goroutine 17
	/usr/local/go/src/testing/testing.go:2101 +0x4c5
FAIL	zpano/indicators/johnehlers/trendcyclemode	0.006s
FAIL

```

---

## Assistant (Build · claude-opus-4.7 · 10.9s)

_Thinking:_

 I'm noticing the tcm array has 201 entries total (63 NaN values followed by 138 data values), but the input has 252 entries, so I need to guard the loop to only iterate over the minimum length to avoid index out of bounds errors.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go",
  "oldString": "\tt.Run(\"reference value (tcm)\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx := testTCMCreateDefault()\n\n\t\tfor i := skip; i < len(input); i++ {\n\t\t\tvalue, _, _, _, _, _, _, _ := x.Update(input[i])\n\t\t\t// MBST known mismatches.\n\t\t\tif i == 70 || i == 71 {\n\t\t\t\tcontinue\n\t\t\t}\n\n\t\t\tif math.IsNaN(value) || math.IsNaN(expValue[i]) {\n\t\t\t\tcontinue\n\t\t\t}",
  "newString": "\tt.Run(\"reference value (tcm)\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx := testTCMCreateDefault()\n\t\tlimit := len(expValue)\n\n\t\tfor i := skip; i < len(input); i++ {\n\t\t\tvalue, _, _, _, _, _, _, _ := x.Update(input[i])\n\t\t\tif i >= limit {\n\t\t\t\tcontinue\n\t\t\t}\n\t\t\t// MBST known mismatches.\n\t\t\tif i == 70 || i == 71 {\n\t\t\t\tcontinue\n\t\t\t}\n\n\t\t\tif math.IsNaN(value) || math.IsNaN(expValue[i]) {\n\t\t\t\tcontinue\n\t\t\t}"
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./indicators/johnehlers/trendcyclemode/... 2>&1 | head -100",
  "description": "Run Go TCM tests",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
--- FAIL: TestTrendCycleModeUpdate (0.00s)
    --- FAIL: TestTrendCycleModeUpdate/reference_phase (0.00s)
        trendcyclemode_test.go:294: [187] phase is incorrect: expected -34.0974828312943, actual 325.90250894195697
        trendcyclemode_test.go:294: [188] phase is incorrect: expected -20.55209191423, actual 339.4478995771415
        trendcyclemode_test.go:294: [189] phase is incorrect: expected -17.4892643678857, actual 342.51072667037954
        trendcyclemode_test.go:294: [190] phase is incorrect: expected -6.64084597196063, actual 353.3591441577747
        trendcyclemode_test.go:294: [191] phase is incorrect: expected -3.41661969757843, actual 356.5833687126411
        trendcyclemode_test.go:294: [192] phase is incorrect: expected -1.11706604110969, actual 358.8829205174652
        trendcyclemode_test.go:294: [193] phase is incorrect: expected -0.61559202318233, actual 359.3843926765895
        trendcyclemode_test.go:294: [194] phase is incorrect: expected -0.58343503934742, actual 359.4165484345072
        trendcyclemode_test.go:294: [195] phase is incorrect: expected -0.97020892968976, actual 359.0297741527109
        trendcyclemode_test.go:294: [196] phase is incorrect: expected -7.33789387098142, actual 352.66208955842046
        trendcyclemode_test.go:294: [197] phase is incorrect: expected -13.71942088996, actual 346.280563331386
        trendcyclemode_test.go:294: [198] phase is incorrect: expected -14.1630359044325, actual 345.83694845410747
        trendcyclemode_test.go:294: [199] phase is incorrect: expected -9.85074940762922, actual 350.1492346946027
        trendcyclemode_test.go:294: [200] phase is incorrect: expected -2.93111556519989, actual 357.06886801233827
        trendcyclemode_test.go:294: [202] phase is incorrect: expected -0.2779793854337, actual 359.7220056895498
        trendcyclemode_test.go:294: [203] phase is incorrect: expected -4.11254536082527, actual 355.8874397146144
        trendcyclemode_test.go:294: [204] phase is incorrect: expected -7.19786584394603, actual 352.8021201588465
        trendcyclemode_test.go:294: [205] phase is incorrect: expected -8.1293043552115, actual 351.8706835817102
        trendcyclemode_test.go:294: [206] phase is incorrect: expected -7.23274284499956, actual 352.7672485836801
        trendcyclemode_test.go:294: [207] phase is incorrect: expected -5.6000818100332, actual 354.399912366064
        trendcyclemode_test.go:294: [208] phase is incorrect: expected -3.98246383052538, actual 356.01753226652295
        trendcyclemode_test.go:294: [209] phase is incorrect: expected -1.93459828828531, actual 358.0653990600411
        trendcyclemode_test.go:294: [210] phase is incorrect: expected -0.91376116945821, actual 359.0862369649398
        trendcyclemode_test.go:294: [249] phase is incorrect: expected -31.1823198151289, actual 328.8176789432034
--- FAIL: TestNewTrendCycleMode (0.00s)
    trendcyclemode_test.go:835: err is incorrect: expected invalid trend cycle mode parameters: 9999: unknown bar component, actual invalid trend cycle mode parameters: invalid dominant cycle parameters: 9999: unknown bar component
    trendcyclemode_test.go:871: err is incorrect: expected invalid trend cycle mode parameters: 9999: unknown trade component, actual invalid trend cycle mode parameters: invalid dominant cycle parameters: 9999: unknown trade component
    trendcyclemode_test.go:853: err is incorrect: expected invalid trend cycle mode parameters: 9999: unknown quote component, actual invalid trend cycle mode parameters: invalid dominant cycle parameters: 9999: unknown quote component
FAIL
FAIL	zpano/indicators/johnehlers/trendcyclemode	0.003s
FAIL

```

---

## Assistant (Build · claude-opus-4.7 · 13.9s)

_Thinking:_

 I'm spotting two issues to fix: the phase wrapping differs between MBST and zpano, so I need to compare them modulo 360 since sine and sineLead already handle this correctly with their degree-to-radian conversion. Second, error messages from bar/quote/trade components are getting double-wrapped by DominantCycle, which adds its own prefix on top of the original error.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go",
  "newString": "\tt.Run(\"reference phase\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx := testTCMCreateDefault()\n\n\t\tfor i := skip; i < len(input); i++ {\n\t\t\t_, _, _, _, _, _, _, phase := x.Update(input[i])\n\t\t\tif math.IsNaN(phase) || math.IsNaN(expPhase[i]) || i < settleSkip {\n\t\t\t\tcontinue\n\t\t\t}\n\n\t\t\t// MBST wraps phase into (-180, 180]; zpano into [0, 360). Compare modulo 360.\n\t\t\td := math.Mod(expPhase[i]-phase, 360.0)\n\t\t\tif d > 180 {\n\t\t\t\td -= 360\n\t\t\t} else if d < -180 {\n\t\t\t\td += 360\n\t\t\t}\n\n\t\t\tif math.Abs(d) > testTCMTolerance {\n\t\t\t\tt.Errorf(\"[%v] phase is incorrect: expected %v, actual %v\", i, expPhase[i], phase)\n\t\t\t}\n\t\t}\n\t})",
  "oldString": "\tt.Run(\"reference phase\", func(t *testing.T) {\n\t\tt.Parallel()\n\n\t\tx := testTCMCreateDefault()\n\n\t\tfor i := skip; i < len(input); i++ {\n\t\t\t_, _, _, _, _, _, _, phase := x.Update(input[i])\n\t\t\tif math.IsNaN(phase) || math.IsNaN(expPhase[i]) || i < settleSkip {\n\t\t\t\tcontinue\n\t\t\t}\n\n\t\t\tif math.Abs(expPhase[i]-phase) > testTCMTolerance {\n\t\t\t\tt.Errorf(\"[%v] phase is incorrect: expected %v, actual %v\", i, expPhase[i], phase)\n\t\t\t}\n\t\t}\n\t})"
}
```

**Output:**
```
Edit applied successfully.
```

---

## User

---

## Assistant (Compaction · claude-opus-4.7 · 64.2s)

## Goal

Port MBST's **TrendCycleMode (TCM)** indicator from `mbst-to-convert/john-ehlers/trend-cycle-mode/` into zpano:
- Go: `go/indicators/johnehlers/trendcyclemode/`
- TS: `ts/indicators/john-ehlers/trend-cycle-mode/`

Wraps existing `DominantCycle` (for period + phase + smoothed price) and adds raw-sample buffer for trendline, sine/sineLead, and trend-vs-cycle mode logic.

## Instructions

### Outputs (order)
1. **Value** (+1 trend / −1 cycle, Scalar)
2. **IsTrendMode** (0/1 Scalar)
3. **IsCycleMode** (0/1 Scalar = 1 − IsTrendMode)
4. **InstantaneousTrendLine** (Scalar)
5. **SineWave** (Scalar)
6. **SineWaveLead** (Scalar)
7. **DominantCyclePeriod** (Scalar)
8. **DominantCyclePhase** (Scalar)

(Drop SmoothedPrice output.)

### Combined single test file
6 MBST reference arrays (rawInput, dcPeriod, dcPhase, sine, sineLead, itl, tcm) merged into one test file, skip=9, settleSkip=177, tol 1e-4. For tcm value: skip indices 70, 71 (MBST known mismatches). tcm array is 201 entries (shorter than input 252) — loop must guard `i >= len(expValue)`.

### MBST params / defaults
- `alphaEmaPeriodAdditional=0.33`, `trendLineSmoothingLength=4` ∈{2,3,4}, `cyclePartMultiplier=1.0` ∈(0,10], `separationPercentage=1.5` ∈(0,100].
- Default estimator HomodyneDiscriminator, smoothingLength=4, αq=αp=0.2, **warmUpPeriod=100**.
- Default BarComponent **BarMedianPrice** (always shown in mnemonic).
- Go `NewTrendCycleModeDefault` / `NewTrendCycleModeParams` → private `newTrendCycleMode`; TS `default()` / `fromParams()`.
- Implement `core.Indicator` directly; use `sync.RWMutex`.
- Mnemonic: `tcm(α, tlsl, cpMul, sep%, <estimator>, <component>)` using `%.3f` for α/cpMul/sep and `%d` for tlsl.

### Test commands
- Go: `cd /home/dev/zpano/go && go test ./indicators/johnehlers/trendcyclemode/...`
- TS: from `/home/dev/zpano/ts`: `node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=TrendCycleMode`

## Discoveries

- MBST `dcp.RawValue` ≡ zpano `DominantCycle.Phase` when primed — no duplicate phase logic needed.
- MBST TCM pushes **raw samples** (not htce.Smoothed) to its own input buffer.
- **Phase comparison:** MBST wraps phase into (-180, 180]; zpano wraps into [0, 360). Tests must compare modulo 360: `d = math.Mod(exp-act, 360); if d>180 d-=360 else if d<-180 d+=360; |d| < tol`.
- **Error message wrapping:** When DominantCycle is created inside `newTrendCycleMode`, bar/quote/trade component errors get double-wrapped: `"invalid trend cycle mode parameters: invalid dominant cycle parameters: 9999: unknown bar component"`. Test expected strings need to include the `invalid dominant cycle parameters:` prefix.
- Coefficient table by tlsl: 2→(2/3,1/3); 3→(3/6,2/6,1/6); 4→(4/10,3/10,2/10,1/10).
- TS `DominantCycle` default maxPeriod is **50**.
- Default mnemonic: `tcm(0.330, 4, 1.000, 1.500%, hl/2)`.
- HTITL is closest algorithmic template; SineWave closest structural template.

## Accomplished

### Done
1. ✅ Added Go `SmoothedPrice()` and `MaxPeriod()` methods on `DominantCycle` + tests.
2. ✅ Added TS `smoothedPrice` and `maxPeriod` getters on `DominantCycle` + tests.
3. ✅ Registered `TrendCycleMode` enum in Go `core/type.go` + `type_test.go`.
4. ✅ Registered `TrendCycleMode` enum in TS `core/indicator-type.ts`.
5. ✅ Go files created and compiling:
   - `trendcyclemodeoutput.go`
   - `trendcyclemodeparams.go` (with `SeparationPercentage`)
   - `trendcyclemode.go` (wraps `*dominantcycle.DominantCycle`; own raw `input[]` buffer; full state; 8-tuple `Update`; bool→0/1 float helpers; epsilon=1e-308)
   - `trendcyclemodeoutput_test.go` (8-output version of HTITL template)
6. ✅ Created `trendcyclemode_test.go` with all 6 MBST reference arrays and sub-tests.

### In progress
- ⏳ Go tests running. Two remaining failures just fixed (need to re-run):
  1. Phase test: applied mod-360 comparison fix.
  2. Invalid bar/quote/trade component error strings — **NEEDS FIX**: test expects `"invalid trend cycle mode parameters: 9999: unknown bar component"` but actual is `"invalid trend cycle mode parameters: invalid dominant cycle parameters: 9999: unknown bar component"`. Update `errBC`, `errQC`, `errTC` constants in `TestNewTrendCycleMode` to include `invalid dominant cycle parameters: ` prefix.

### Todo
- ⏳ Fix `errBC/errQC/errTC` strings and re-run Go tests until passing.
- ⏳ Mirror in TS:
  - `trend-cycle-mode-params.ts`
  - `trend-cycle-mode-output.ts`
  - `trend-cycle-mode.ts`
  - `trend-cycle-mode.spec.ts`
- ⏳ Update `.opencode/skills/mbst-indicator-conversion/SKILL.md` — document "expose wrapper internals rather than duplicating" pattern + phase wrap-around-360 comparison trick + error-wrapping depth note.

## Relevant files / directories

### Go — created/editing
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode.go`
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeparams.go`
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput.go`
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemodeoutput_test.go`
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go` ← currently failing with 3 error-string mismatches; phase test already patched to use mod-360

### Go — edited
- `/home/dev/zpano/go/indicators/johnehlers/dominantcycle/dominantcycle.go` — added `SmoothedPrice()`, `MaxPeriod()`
- `/home/dev/zpano/go/indicators/johnehlers/dominantcycle/dominantcycle_test.go` — added 2 tests
- `/home/dev/zpano/go/indicators/core/type.go` + `type_test.go` — added `TrendCycleMode` enum

### TS — edited
- `/home/dev/zpano/ts/indicators/john-ehlers/dominant-cycle/dominant-cycle.ts` — added `smoothedPrice`, `maxPeriod` getters
- `/home/dev/zpano/ts/indicators/john-ehlers/dominant-cycle/dominant-cycle.spec.ts` — added 2 specs
- `/home/dev/zpano/ts/indicators/core/indicator-type.ts` — appended `TrendCycleMode`

### TS — to create
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-params.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-output.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.ts`
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts`

### Reference templates (Go)
- `/home/dev/zpano/go/indicators/johnehlers/hilberttransformerinstantaneoustrendline/` (all 5 files; ideal test-structure reference)
- `/home/dev/zpano/go/indicators/johnehlers/sinewave/sinewave.go` (wraps DominantCycle)

### MBST source
- `/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleMode.cs`
- `/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleModeTest.cs`

### Skills
- `.opencode/skills/mbst-indicator-conversion/SKILL.md` (needs post-completion update)

### Immediate next action
1. In `trendcyclemode_test.go`, update error constants:
   ```go
   errBC = "invalid trend cycle mode parameters: invalid dominant cycle parameters: 9999: unknown bar component"
   errQC = "invalid trend cycle mode parameters: invalid dominant cycle parameters: 9999: unknown quote component"
   errTC = "invalid trend cycle mode parameters: invalid dominant cycle parameters: 9999: unknown trade component"
   ```
2. Re-run `go test ./indicators/johnehlers/trendcyclemode/...`; iterate if needed.
3. Mirror the whole package in TS.
4. Update skill doc.

---

## User

---

## Assistant (Build · claude-opus-4.7 · 4.5s)

_Thinking:_

 Now I need to update the error message strings in the test file and run the tests again to see if they pass.

**Tool: bash**

**Input:**
```json
{
  "command": "ls go/indicators/johnehlers/trendcyclemode/",
  "description": "List trendcyclemode files"
}
```

**Output:**
```
trendcyclemode.go
trendcyclemode_test.go
trendcyclemodeoutput.go
trendcyclemodeoutput_test.go
trendcyclemodeparams.go

```

---

## Assistant (Build · claude-opus-4.7 · 4.0s)

**Tool: grep**

**Input:**
```json
{
  "pattern": "errBC|errQC|errTC|unknown bar|unknown quote|unknown trade",
  "path": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go"
}
```

**Output:**
```
Found 6 matches
/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go:
  Line 630: 		errBC = "invalid trend cycle mode parameters: 9999: unknown bar component"
  Line 631: 		errQC = "invalid trend cycle mode parameters: 9999: unknown quote component"
  Line 632: 		errTC = "invalid trend cycle mode parameters: 9999: unknown trade component"
  Line 843: 		check("err", errBC, err.Error())
  Line 861: 		check("err", errQC, err.Error())
  Line 879: 		check("err", errTC, err.Error())
```

---

## Assistant (Build · claude-opus-4.7 · 3.8s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go",
  "offset": 625,
  "limit": 15
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go</path>
<type>file</type>
<content>625: 			"trend line smoothing length should be 2, 3, or 4"
626: 		errCPM = "invalid trend cycle mode parameters: " +
627: 			"cycle part multiplier should be in range (0, 10]"
628: 		errSep = "invalid trend cycle mode parameters: " +
629: 			"separation percentage should be in range (0, 100]"
630: 		errBC = "invalid trend cycle mode parameters: 9999: unknown bar component"
631: 		errQC = "invalid trend cycle mode parameters: 9999: unknown quote component"
632: 		errTC = "invalid trend cycle mode parameters: 9999: unknown trade component"
633: 	)
634: 
635: 	check := func(name string, exp, act any) {
636: 		t.Helper()
637: 
638: 		if exp != act {
639: 			t.Errorf("%s is incorrect: expected %v, actual %v", name, exp, act)

(Showing lines 625-639 of 903. Use offset=640 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.3s)

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go",
  "oldString": "\t\terrBC = \"invalid trend cycle mode parameters: 9999: unknown bar component\"\n\t\terrQC = \"invalid trend cycle mode parameters: 9999: unknown quote component\"\n\t\terrTC = \"invalid trend cycle mode parameters: 9999: unknown trade component\"",
  "newString": "\t\terrBC = \"invalid trend cycle mode parameters: \" +\n\t\t\t\"invalid dominant cycle parameters: 9999: unknown bar component\"\n\t\terrQC = \"invalid trend cycle mode parameters: \" +\n\t\t\t\"invalid dominant cycle parameters: 9999: unknown quote component\"\n\t\terrTC = \"invalid trend cycle mode parameters: \" +\n\t\t\t\"invalid dominant cycle parameters: 9999: unknown trade component\""
}
```

**Output:**
```
Edit applied successfully.
```

---

## Assistant (Build · claude-opus-4.7 · 4.4s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./indicators/johnehlers/trendcyclemode/... 2>&1 | tail -50",
  "description": "Run trendcyclemode tests",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
ok  	zpano/indicators/johnehlers/trendcyclemode	0.003s

```

---

## Assistant (Build · claude-opus-4.7 · 4.0s)

Go tests pass. Now mirroring in TypeScript. Let me look at the Go implementation and similar TS indicators to follow the pattern.

**Tool: bash**

**Input:**
```json
{
  "command": "ls ts/indicators/john-ehlers/",
  "description": "List TS john-ehlers indicators"
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
zero-lag-error-correcting-exponential-moving-average
zero-lag-exponential-moving-average

```

---

## Assistant (Build · claude-opus-4.7 · 4.9s)

**Tool: bash**

**Input:**
```json
{
  "command": "ls ts/indicators/john-ehlers/sine-wave/ ts/indicators/john-ehlers/hilbert-transformer-instantaneous-trend-line/",
  "description": "List TS template directories"
}
```

**Output:**
```
ts/indicators/john-ehlers/hilbert-transformer-instantaneous-trend-line/:
hilbert-transformer-instantaneous-trend-line-output.ts
hilbert-transformer-instantaneous-trend-line-params.ts
hilbert-transformer-instantaneous-trend-line.spec.ts
hilbert-transformer-instantaneous-trend-line.ts

ts/indicators/john-ehlers/sine-wave/:
sine-wave-output.ts
sine-wave-params.ts
sine-wave.spec.ts
sine-wave.ts

```

---

## Assistant (Build · claude-opus-4.7 · 3.2s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/sine-wave/sine-wave.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/sine-wave/sine-wave.ts</path>
<type>file</type>
<content>1: import { Bar } from '../../../entities/bar';
2: import { BarComponent, DefaultBarComponent, barComponentValue } from '../../../entities/bar-component';
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
13: import { Band } from '../../core/outputs/band';
14: import { OutputType } from '../../core/outputs/output-type';
15: import { DominantCycle } from '../dominant-cycle/dominant-cycle';
16: import { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';
17: import { createEstimator, estimatorMoniker } from '../hilbert-transformer/hilbert-transformer-common';
18: import { SineWaveOutput } from './sine-wave-output';
19: import { SineWaveParams } from './sine-wave-params';
20: 
21: const DEFAULT_ALPHA_EMA_PERIOD_ADDITIONAL = 0.33;
22: const DEFAULT_SMOOTHING_LENGTH = 4;
23: const DEFAULT_ALPHA_EMA_QI = 0.2;
24: const DEFAULT_ALPHA_EMA_PERIOD = 0.2;
25: // MBST's DominantCyclePeriod default warm-up is MaxPeriod * 2 = 100.
26: const DEFAULT_WARM_UP_PERIOD = 100;
27: 
28: const DEG2RAD = Math.PI / 180.0;
29: const LEAD_OFFSET = 45.0;
30: 
31: /** __Sine Wave__ (Ehlers) computes a clear sine wave representation of the dominant cycle phase.
32:  *
33:  * It exposes five outputs:
34:  *
35:  *	- Value: the sine wave value, sin(phase·Deg2Rad).
36:  *	- Lead: the sine wave lead value, sin((phase+45)·Deg2Rad).
37:  *	- Band: a band with Value as the upper line and Lead as the lower line.
38:  *	- DominantCyclePeriod: the smoothed dominant cycle period.
39:  *	- DominantCyclePhase: the dominant cycle phase, in degrees.
40:  *
41:  * Reference:
42:  *
43:  *	John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 95-105.
44:  */
45: export class SineWave implements Indicator {
46:   private readonly dc: DominantCycle;
47:   private primed = false;
48:   private value = Number.NaN;
49:   private lead = Number.NaN;
50: 
51:   private readonly mnemonicValue: string;
52:   private readonly descriptionValue: string;
53:   private readonly mnemonicLead: string;
54:   private readonly descriptionLead: string;
55:   private readonly mnemonicBand: string;
56:   private readonly descriptionBand: string;
57:   private readonly mnemonicDCP: string;
58:   private readonly descriptionDCP: string;
59:   private readonly mnemonicDCPhase: string;
60:   private readonly descriptionDCPhase: string;
61: 
62:   private readonly barComponentFunc: (bar: Bar) => number;
63:   private readonly quoteComponentFunc: (quote: Quote) => number;
64:   private readonly tradeComponentFunc: (trade: Trade) => number;
65: 
66:   /** Creates an instance using default parameters (α=0.33, HomodyneDiscriminator cycle
67:    * estimator with smoothingLength=4, αq=0.2, αp=0.2, warmUpPeriod=100, BarComponent.Median). */
68:   public static default(): SineWave {
69:     return new SineWave({
70:       alphaEmaPeriodAdditional: DEFAULT_ALPHA_EMA_PERIOD_ADDITIONAL,
71:       estimatorType: HilbertTransformerCycleEstimatorType.HomodyneDiscriminator,
72:       estimatorParams: {
73:         smoothingLength: DEFAULT_SMOOTHING_LENGTH,
74:         alphaEmaQuadratureInPhase: DEFAULT_ALPHA_EMA_QI,
75:         alphaEmaPeriod: DEFAULT_ALPHA_EMA_PERIOD,
76:         warmUpPeriod: DEFAULT_WARM_UP_PERIOD,
77:       },
78:     });
79:   }
80: 
81:   /** Creates an instance based on the given parameters. */
82:   public static fromParams(params: SineWaveParams): SineWave {
83:     return new SineWave(params);
84:   }
85: 
86:   private constructor(params: SineWaveParams) {
87:     const alpha = params.alphaEmaPeriodAdditional;
88:     if (alpha <= 0 || alpha > 1) {
89:       throw new Error('invalid sine wave parameters: α for additional smoothing should be in range (0, 1]');
90:     }
91: 
92:     // SineWave defaults to BarComponent.Median (not the framework default), so it always
93:     // shows in the mnemonic.
94:     const bc = params.barComponent ?? BarComponent.Median;
95:     const qc = params.quoteComponent ?? DefaultQuoteComponent;
96:     const tc = params.tradeComponent ?? DefaultTradeComponent;
97: 
98:     this.barComponentFunc = barComponentValue(bc);
99:     this.quoteComponentFunc = quoteComponentValue(qc);
100:     this.tradeComponentFunc = tradeComponentValue(tc);
101: 
102:     // Inner DominantCycle built with explicit components so its mnemonic contains hl/2.
103:     this.dc = DominantCycle.fromParams({
104:       alphaEmaPeriodAdditional: alpha,
105:       estimatorType: params.estimatorType,
106:       estimatorParams: params.estimatorParams,
107:       barComponent: bc,
108:       quoteComponent: qc,
109:       tradeComponent: tc,
110:     });
111: 
112:     // Compose estimator moniker (same logic as DominantCycle).
113:     const effectiveType = params.estimatorType ?? HilbertTransformerCycleEstimatorType.HomodyneDiscriminator;
114:     const htce = createEstimator(params.estimatorType, params.estimatorParams);
115:     let em = '';
116:     const isDefaultHd = effectiveType === HilbertTransformerCycleEstimatorType.HomodyneDiscriminator
117:       && htce.smoothingLength === DEFAULT_SMOOTHING_LENGTH
118:       && htce.alphaEmaQuadratureInPhase === DEFAULT_ALPHA_EMA_QI
119:       && htce.alphaEmaPeriod === DEFAULT_ALPHA_EMA_PERIOD;
120:     if (!isDefaultHd) {
121:       const moniker = estimatorMoniker(effectiveType, htce);
122:       if (moniker.length > 0) {
123:         em = ', ' + moniker;
124:       }
125:     }
126: 
127:     const cm = componentTripleMnemonic(bc, qc, tc);
128:     const a = alpha.toFixed(3);
129: 
130:     this.mnemonicValue = `sw(${a}${em}${cm})`;
131:     this.mnemonicLead = `sw-lead(${a}${em}${cm})`;
132:     this.mnemonicBand = `sw-band(${a}${em}${cm})`;
133:     this.mnemonicDCP = `dcp(${a}${em}${cm})`;
134:     this.mnemonicDCPhase = `dcph(${a}${em}${cm})`;
135: 
136:     this.descriptionValue = 'Sine wave ' + this.mnemonicValue;
137:     this.descriptionLead = 'Sine wave lead ' + this.mnemonicLead;
138:     this.descriptionBand = 'Sine wave band ' + this.mnemonicBand;
139:     this.descriptionDCP = 'Dominant cycle period ' + this.mnemonicDCP;
140:     this.descriptionDCPhase = 'Dominant cycle phase ' + this.mnemonicDCPhase;
141: 
142:     // Silence unused-variable warning for DefaultBarComponent import (kept for consistency).
143:     void DefaultBarComponent;
144:   }
145: 
146:   /** Indicates whether the indicator is primed. */
147:   public isPrimed(): boolean { return this.primed; }
148: 
149:   /** Describes the output data of the indicator. */
150:   public metadata(): IndicatorMetadata {
151:     return {
152:       type: IndicatorType.SineWave,
153:       mnemonic: this.mnemonicValue,
154:       description: this.descriptionValue,
155:       outputs: [
156:         {
157:           kind: SineWaveOutput.Value,
158:           type: OutputType.Scalar,
159:           mnemonic: this.mnemonicValue,
160:           description: this.descriptionValue,
161:         },
162:         {
163:           kind: SineWaveOutput.Lead,
164:           type: OutputType.Scalar,
165:           mnemonic: this.mnemonicLead,
166:           description: this.descriptionLead,
167:         },
168:         {
169:           kind: SineWaveOutput.Band,
170:           type: OutputType.Band,
171:           mnemonic: this.mnemonicBand,
172:           description: this.descriptionBand,
173:         },
174:         {
175:           kind: SineWaveOutput.DominantCyclePeriod,
176:           type: OutputType.Scalar,
177:           mnemonic: this.mnemonicDCP,
178:           description: this.descriptionDCP,
179:         },
180:         {
181:           kind: SineWaveOutput.DominantCyclePhase,
182:           type: OutputType.Scalar,
183:           mnemonic: this.mnemonicDCPhase,
184:           description: this.descriptionDCPhase,
185:         },
186:       ],
187:     };
188:   }
189: 
190:   /** Updates the indicator given the next sample value. Returns the quadruple
191:    * (value, lead, period, phase). Returns all-NaN if not yet primed. */
192:   public update(sample: number): [number, number, number, number] {
193:     if (Number.isNaN(sample)) {
194:       return [sample, sample, sample, sample];
195:     }
196: 
197:     const [, period, phase] = this.dc.update(sample);
198: 
199:     if (Number.isNaN(phase)) {
200:       return [Number.NaN, Number.NaN, Number.NaN, Number.NaN];
201:     }
202: 
203:     this.primed = true;
204:     this.value = Math.sin(phase * DEG2RAD);
205:     this.lead = Math.sin((phase + LEAD_OFFSET) * DEG2RAD);
206:     return [this.value, this.lead, period, phase];
207:   }
208: 
209:   /** Updates an indicator given the next scalar sample. */
210:   public updateScalar(sample: Scalar): IndicatorOutput {
211:     return this.updateEntity(sample.time, sample.value);
212:   }
213: 
214:   /** Updates an indicator given the next bar sample. */
215:   public updateBar(sample: Bar): IndicatorOutput {
216:     return this.updateEntity(sample.time, this.barComponentFunc(sample));
217:   }
218: 
219:   /** Updates an indicator given the next quote sample. */
220:   public updateQuote(sample: Quote): IndicatorOutput {
221:     return this.updateEntity(sample.time, this.quoteComponentFunc(sample));
222:   }
223: 
224:   /** Updates an indicator given the next trade sample. */
225:   public updateTrade(sample: Trade): IndicatorOutput {
226:     return this.updateEntity(sample.time, this.tradeComponentFunc(sample));
227:   }
228: 
229:   private updateEntity(time: Date, sample: number): IndicatorOutput {
230:     const [value, lead, period, phase] = this.update(sample);
231: 
232:     const sv = new Scalar();
233:     sv.time = time;
234:     sv.value = value;
235: 
236:     const sl = new Scalar();
237:     sl.time = time;
238:     sl.value = lead;
239: 
240:     const band = new Band();
241:     band.time = time;
242:     band.upper = value;
243:     band.lower = lead;
244: 
245:     const sp = new Scalar();
246:     sp.time = time;
247:     sp.value = period;
248: 
249:     const sph = new Scalar();
250:     sph.time = time;
251:     sph.value = phase;
252: 
253:     return [sv, sl, band, sp, sph];
254:   }
255: }

(End of file - total 255 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.9s)

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

## Assistant (Build · claude-opus-4.7 · 3.8s)

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

## Assistant (Build · claude-opus-4.7 · 4.3s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/sine-wave/sine-wave-params.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/sine-wave/sine-wave-params.ts</path>
<type>file</type>
<content>1: import { BarComponent } from '../../../entities/bar-component';
2: import { QuoteComponent } from '../../../entities/quote-component';
3: import { TradeComponent } from '../../../entities/trade-component';
4: import { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';
5: import { HilbertTransformerCycleEstimatorParams } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-params';
6: 
7: /** Describes parameters to create an instance of the SineWave indicator. */
8: export interface SineWaveParams {
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
24:   /** A component of a bar to use when updating the indicator with a bar sample.
25:    *
26:    * If _undefined_, the default (BarComponent.Median, hl/2) is used.
27:    */
28:   barComponent?: BarComponent;
29: 
30:   /** A component of a quote to use when updating the indicator with a quote sample.
31:    *
32:    * If _undefined_, a default value is used and the component is not shown in the indicator mnemonic.
33:    */
34:   quoteComponent?: QuoteComponent;
35: 
36:   /** A component of a trade to use when updating the indicator with a trade sample.
37:    *
38:    * If _undefined_, a default value is used and the component is not shown in the indicator mnemonic.
39:    */
40:   tradeComponent?: TradeComponent;
41: }

(End of file - total 41 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 3.7s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/sine-wave/sine-wave-output.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/sine-wave/sine-wave-output.ts</path>
<type>file</type>
<content>1: /** Enumerates outputs of the SineWave indicator. */
2: export enum SineWaveOutput {
3:   /** The sine wave value, sin(phase·Deg2Rad). */
4:   Value = 0,
5:   /** The sine wave lead value, sin((phase+45)·Deg2Rad). */
6:   Lead = 1,
7:   /** The band formed by the sine wave (upper) and the lead sine wave (lower). */
8:   Band = 2,
9:   /** The smoothed dominant cycle period. */
10:   DominantCyclePeriod = 3,
11:   /** The dominant cycle phase, in degrees. */
12:   DominantCyclePhase = 4,
13: }

(End of file - total 13 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.4s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/dominant-cycle/dominant-cycle.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/dominant-cycle/dominant-cycle.ts</path>
<type>file</type>
<content>1: import { Bar } from '../../../entities/bar';
2: import { BarComponent, DefaultBarComponent, barComponentValue } from '../../../entities/bar-component';
3: import { Quote } from '../../../entities/quote';
4: import { QuoteComponent, DefaultQuoteComponent, quoteComponentValue } from '../../../entities/quote-component';
5: import { Scalar } from '../../../entities/scalar';
6: import { Trade } from '../../../entities/trade';
7: import { TradeComponent, DefaultTradeComponent, tradeComponentValue } from '../../../entities/trade-component';
8: import { componentTripleMnemonic } from '../../core/component-triple-mnemonic';
9: import { Indicator } from '../../core/indicator';
10: import { IndicatorMetadata } from '../../core/indicator-metadata';
11: import { IndicatorOutput } from '../../core/indicator-output';
12: import { IndicatorType } from '../../core/indicator-type';
13: import { OutputType } from '../../core/outputs/output-type';
14: import { HilbertTransformerCycleEstimator } from '../hilbert-transformer/hilbert-transformer-cycle-estimator';
15: import { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';
16: import { createEstimator, estimatorMoniker } from '../hilbert-transformer/hilbert-transformer-common';
17: import { DominantCycleOutput } from './dominant-cycle-output';
18: import { DominantCycleParams } from './dominant-cycle-params';
19: 
20: const DEFAULT_ALPHA_EMA_PERIOD_ADDITIONAL = 0.33;
21: const DEFAULT_SMOOTHING_LENGTH = 4;
22: const DEFAULT_ALPHA_EMA_QI = 0.2;
23: const DEFAULT_ALPHA_EMA_PERIOD = 0.2;
24: // MBST's DominantCyclePeriod default warm-up is MaxPeriod * 2 = 100. The HTCE's internal
25: // auto-default (smoothingLengthPlus3HtLength, = 25 for smoothingLength=4) is shorter and would
26: // prime earlier than the MBST reference. We pass it explicitly so `default()` matches MBST.
27: const DEFAULT_WARM_UP_PERIOD = 100;
28: 
29: /** __Dominant Cycle__ (Ehlers) computes the instantaneous cycle period and phase derived
30:  * from a Hilbert transformer cycle estimator.
31:  *
32:  * It exposes three outputs:
33:  *
34:  *	- RawPeriod: the raw instantaneous cycle period produced by the Hilbert transformer estimator.
35:  *	- Period: the dominant cycle period obtained by additional EMA smoothing of the raw period.
36:  *	  Periodᵢ = α·RawPeriodᵢ + (1 − α)·Periodᵢ₋₁, 0 < α ≤ 1.
37:  *	- Phase: the dominant cycle phase, in degrees.
38:  *
39:  * The smoothed data are multiplied by the real (cosine) component of the dominant cycle and
40:  * independently by the imaginary (sine) component of the dominant cycle. The products are
41:  * summed then over one full dominant cycle. The phase angle is computed as the arctangent of
42:  * the ratio of the real part to the imaginary part.
43:  *
44:  * Reference:
45:  *
46:  *	John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 52-77.
47:  */
48: export class DominantCycle implements Indicator {
49:   private readonly htce: HilbertTransformerCycleEstimator;
50:   private readonly alphaEmaPeriodAdditional: number;
51:   private readonly oneMinAlphaEmaPeriodAdditional: number;
52:   private readonly smoothedInput: number[];
53:   private readonly smoothedInputLengthMin1: number;
54:   private smoothedPeriod = 0;
55:   private smoothedPhase = 0;
56:   private primed = false;
57: 
58:   private readonly mnemonicRawPeriod: string;
59:   private readonly descriptionRawPeriod: string;
60:   private readonly mnemonicPeriod: string;
61:   private readonly descriptionPeriod: string;
62:   private readonly mnemonicPhase: string;
63:   private readonly descriptionPhase: string;
64: 
65:   private readonly barComponentFunc: (bar: Bar) => number;
66:   private readonly quoteComponentFunc: (quote: Quote) => number;
67:   private readonly tradeComponentFunc: (trade: Trade) => number;
68: 
69:   /** Creates an instance using default parameters (α=0.33, HomodyneDiscriminator cycle
70:    * estimator with smoothingLength=4, αq=0.2, αp=0.2, warmUpPeriod=100). */
71:   public static default(): DominantCycle {
72:     return new DominantCycle({
73:       alphaEmaPeriodAdditional: DEFAULT_ALPHA_EMA_PERIOD_ADDITIONAL,
74:       estimatorType: HilbertTransformerCycleEstimatorType.HomodyneDiscriminator,
75:       estimatorParams: {
76:         smoothingLength: DEFAULT_SMOOTHING_LENGTH,
77:         alphaEmaQuadratureInPhase: DEFAULT_ALPHA_EMA_QI,
78:         alphaEmaPeriod: DEFAULT_ALPHA_EMA_PERIOD,
79:         warmUpPeriod: DEFAULT_WARM_UP_PERIOD,
80:       },
81:     });
82:   }
83: 
84:   /** Creates an instance based on the given parameters. */
85:   public static fromParams(params: DominantCycleParams): DominantCycle {
86:     return new DominantCycle(params);
87:   }
88: 
89:   private constructor(params: DominantCycleParams) {
90:     const alpha = params.alphaEmaPeriodAdditional;
91:     if (alpha <= 0 || alpha > 1) {
92:       throw new Error('invalid dominant cycle parameters: α for additional smoothing should be in range (0, 1]');
93:     }
94: 
95:     this.alphaEmaPeriodAdditional = alpha;
96:     this.oneMinAlphaEmaPeriodAdditional = 1 - alpha;
97: 
98:     this.htce = createEstimator(params.estimatorType, params.estimatorParams);
99: 
100:     const effectiveType = params.estimatorType ?? HilbertTransformerCycleEstimatorType.HomodyneDiscriminator;
101:     let em = '';
102:     const isDefaultHd = effectiveType === HilbertTransformerCycleEstimatorType.HomodyneDiscriminator
103:       && this.htce.smoothingLength === DEFAULT_SMOOTHING_LENGTH
104:       && this.htce.alphaEmaQuadratureInPhase === DEFAULT_ALPHA_EMA_QI
105:       && this.htce.alphaEmaPeriod === DEFAULT_ALPHA_EMA_PERIOD;
106:     if (!isDefaultHd) {
107:       const moniker = estimatorMoniker(effectiveType, this.htce);
108:       if (moniker.length > 0) {
109:         em = ', ' + moniker;
110:       }
111:     }
112: 
113:     const bc = params.barComponent ?? DefaultBarComponent;
114:     const qc = params.quoteComponent ?? DefaultQuoteComponent;
115:     const tc = params.tradeComponent ?? DefaultTradeComponent;
116: 
117:     this.barComponentFunc = barComponentValue(bc);
118:     this.quoteComponentFunc = quoteComponentValue(qc);
119:     this.tradeComponentFunc = tradeComponentValue(tc);
120: 
121:     const cm = componentTripleMnemonic(params.barComponent, params.quoteComponent, params.tradeComponent);
122:     const a = alpha.toFixed(3);
123: 
124:     this.mnemonicRawPeriod = `dcp-raw(${a}${em}${cm})`;
125:     this.mnemonicPeriod = `dcp(${a}${em}${cm})`;
126:     this.mnemonicPhase = `dcph(${a}${em}${cm})`;
127: 
128:     this.descriptionRawPeriod = 'Dominant cycle raw period ' + this.mnemonicRawPeriod;
129:     this.descriptionPeriod = 'Dominant cycle period ' + this.mnemonicPeriod;
130:     this.descriptionPhase = 'Dominant cycle phase ' + this.mnemonicPhase;
131: 
132:     const maxPeriod = this.htce.maxPeriod;
133:     this.smoothedInput = new Array<number>(maxPeriod).fill(0);
134:     this.smoothedInputLengthMin1 = maxPeriod - 1;
135:   }
136: 
137:   /** Indicates whether the indicator is primed. */
138:   public isPrimed(): boolean { return this.primed; }
139: 
140:   /** The current WMA-smoothed price value produced by the underlying Hilbert transformer
141:    * cycle estimator. Returns NaN if the indicator is not yet primed.
142:    *
143:    * This accessor is intended for composite indicators (e.g. TrendCycleMode) that wrap a
144:    * DominantCycle and need to consult the same smoothed input stream that drives the
145:    * dominant-cycle computation. */
146:   public get smoothedPrice(): number {
147:     return this.primed ? this.htce.smoothed : Number.NaN;
148:   }
149: 
150:   /** The maximum cycle period supported by the underlying Hilbert transformer cycle
151:    * estimator (also the size of the internal smoothed-input buffer). */
152:   public get maxPeriod(): number {
153:     return this.htce.maxPeriod;
154:   }
155: 
156:   /** Describes the output data of the indicator. */
157:   public metadata(): IndicatorMetadata {
158:     return {
159:       type: IndicatorType.DominantCycle,
160:       mnemonic: this.mnemonicPeriod,
161:       description: this.descriptionPeriod,
162:       outputs: [
163:         {
164:           kind: DominantCycleOutput.RawPeriod,
165:           type: OutputType.Scalar,
166:           mnemonic: this.mnemonicRawPeriod,
167:           description: this.descriptionRawPeriod,
168:         },
169:         {
170:           kind: DominantCycleOutput.Period,
171:           type: OutputType.Scalar,
172:           mnemonic: this.mnemonicPeriod,
173:           description: this.descriptionPeriod,
174:         },
175:         {
176:           kind: DominantCycleOutput.Phase,
177:           type: OutputType.Scalar,
178:           mnemonic: this.mnemonicPhase,
179:           description: this.descriptionPhase,
180:         },
181:       ],
182:     };
183:   }
184: 
185:   /** Updates the indicator given the next sample value. Returns the triple
186:    * (rawPeriod, period, phase). Returns (NaN, NaN, NaN) if not yet primed. */
187:   public update(sample: number): [number, number, number] {
188:     if (Number.isNaN(sample)) {
189:       return [sample, sample, sample];
190:     }
191: 
192:     this.htce.update(sample);
193:     this.pushSmoothedInput(this.htce.smoothed);
194: 
195:     if (this.primed) {
196:       this.smoothedPeriod = this.alphaEmaPeriodAdditional * this.htce.period
197:         + this.oneMinAlphaEmaPeriodAdditional * this.smoothedPeriod;
198:       this.calculateSmoothedPhase();
199:       return [this.htce.period, this.smoothedPeriod, this.smoothedPhase];
200:     }
201: 
202:     if (this.htce.primed) {
203:       this.primed = true;
204:       this.smoothedPeriod = this.htce.period;
205:       this.calculateSmoothedPhase();
206:       return [this.htce.period, this.smoothedPeriod, this.smoothedPhase];
207:     }
208: 
209:     return [Number.NaN, Number.NaN, Number.NaN];
210:   }
211: 
212:   /** Updates an indicator given the next scalar sample. */
213:   public updateScalar(sample: Scalar): IndicatorOutput {
214:     return this.updateEntity(sample.time, sample.value);
215:   }
216: 
217:   /** Updates an indicator given the next bar sample. */
218:   public updateBar(sample: Bar): IndicatorOutput {
219:     return this.updateEntity(sample.time, this.barComponentFunc(sample));
220:   }
221: 
222:   /** Updates an indicator given the next quote sample. */
223:   public updateQuote(sample: Quote): IndicatorOutput {
224:     return this.updateEntity(sample.time, this.quoteComponentFunc(sample));
225:   }
226: 
227:   /** Updates an indicator given the next trade sample. */
228:   public updateTrade(sample: Trade): IndicatorOutput {
229:     return this.updateEntity(sample.time, this.tradeComponentFunc(sample));
230:   }
231: 
232:   private updateEntity(time: Date, sample: number): IndicatorOutput {
233:     const [rawPeriod, period, phase] = this.update(sample);
234: 
235:     const s1 = new Scalar();
236:     s1.time = time;
237:     s1.value = rawPeriod;
238: 
239:     const s2 = new Scalar();
240:     s2.time = time;
241:     s2.value = period;
242: 
243:     const s3 = new Scalar();
244:     s3.time = time;
245:     s3.value = phase;
246: 
247:     return [s1, s2, s3];
248:   }
249: 
250:   private pushSmoothedInput(value: number): void {
251:     for (let i = this.smoothedInputLengthMin1; i > 0; i--) {
252:       this.smoothedInput[i] = this.smoothedInput[i - 1];
253:     }
254:     this.smoothedInput[0] = value;
255:   }
256: 
257:   private calculateSmoothedPhase(): void {
258:     const rad2deg = 180.0 / Math.PI;
259:     const twoPi = 2.0 * Math.PI;
260:     const epsilon = 0.01;
261: 
262:     // The smoothed data are multiplied by the real (cosine) component of the dominant cycle
263:     // and independently by the imaginary (sine) component of the dominant cycle. The products
264:     // are summed over one full dominant cycle.
265:     let length = Math.floor(this.smoothedPeriod + 0.5);
266:     if (length > this.smoothedInputLengthMin1) {
267:       length = this.smoothedInputLengthMin1;
268:     }
269: 
270:     let realPart = 0;
271:     let imagPart = 0;
272: 
273:     for (let i = 0; i < length; i++) {
274:       const temp = (twoPi * i) / length;
275:       const smoothed = this.smoothedInput[i];
276:       realPart += smoothed * Math.sin(temp);
277:       imagPart += smoothed * Math.cos(temp);
278:     }
279: 
280:     // We compute the phase angle as the arctangent of the ratio of the real part to the imaginary part.
281:     // The phase increases from left to right.
282:     const previous = this.smoothedPhase;
283:     let phase = Math.atan(realPart / imagPart) * rad2deg;
284:     if (Number.isNaN(phase) || !Number.isFinite(phase)) {
285:       phase = previous;
286:     }
287: 
288:     if (Math.abs(imagPart) <= epsilon) {
289:       if (realPart > 0) {
290:         phase += 90;
291:       } else if (realPart < 0) {
292:         phase -= 90;
293:       }
294:     }
295: 
296:     // 90 degree reference shift.
297:     phase += 90;
298:     // Compensate for one-bar lag of the WMA-smoothed input price by adding the phase
299:     // corresponding to a 1-bar lag of the smoothed dominant cycle period.
300:     phase += 360 / this.smoothedPeriod;
301:     // Resolve phase ambiguity when the imaginary part is negative.
302:     if (imagPart < 0) {
303:       phase += 180;
304:     }
305:     // Cycle wraparound.
306:     if (phase > 360) {
307:       phase -= 360;
308:     }
309: 
310:     this.smoothedPhase = phase;
311:   }
312: }

(End of file - total 312 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.4s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/sine-wave/sine-wave.spec.ts",
  "limit": 80
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/sine-wave/sine-wave.spec.ts</path>
<type>file</type>
<content>1: import { } from 'jasmine';
2: 
3: import { Bar } from '../../../entities/bar';
4: import { BarComponent } from '../../../entities/bar-component';
5: import { Quote } from '../../../entities/quote';
6: import { Scalar } from '../../../entities/scalar';
7: import { Trade } from '../../../entities/trade';
8: import { IndicatorType } from '../../core/indicator-type';
9: import { Band } from '../../core/outputs/band';
10: import { OutputType } from '../../core/outputs/output-type';
11: import { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';
12: import { SineWave } from './sine-wave';
13: import { SineWaveOutput } from './sine-wave-output';
14: 
15: /* eslint-disable max-len */
16: // Input data taken from TA-Lib (http://ta-lib.org/) tests, test_MAMA.xsl, Price, D5…D256, 252 entries.
17: const input = [
18:   92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,
19:   94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,
20:   88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,
21:   85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,
22:   83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,
23:   89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,
24:   89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,
25:   88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,
26:   103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,
27:   120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,
28:   114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,
29:   114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,
30:   124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,
31:   137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,
32:   123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,
33:   122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,
34:   123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,
35:   130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,
36:   127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,
37:   121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,
38:   106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,
39:   95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,
40:   94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,
41:   103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,
42:   106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,
43:   109.5300, 108.0600,
44: ];
45: 
46: // Expected sine wave values taken from MBST SineWaveTest.cs. 252 entries.
47: const expectedSine = [
48:   Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN,
49:   Number.NaN, Number.NaN, Number.NaN,
50:   -0.9874388886763950, 0.9901389074597280, 0.7430789154373040, 0.9373629963767220, 0.9539229407379340, 0.7583605937417590, 0.9430523282622310, 0.9954894450161860, 0.2677593920226290, -0.1322524693787770,
51:   -0.0076634783681766, 0.0131173971015221, 0.1186956105503800, 0.1801829214581150, 0.2520928115991770, 0.4006612403521310, 0.5628300247542610, 0.7147094931388430, 0.8184710623910560, 0.8857771050315170,
52:   0.8580051054352940, 0.9044393510772560, 0.9477105512269880, 0.9791019835938800, 0.9961661502181070, 0.9822460058918840, 0.8465682109433450, 0.4079557291761370, -0.4004013665430420, -0.8421036878603060,
53:   -0.9017223344864390, -0.9865337683617420, -0.9643853118844240, -0.9598599815079740, 0.8037267766705520, 0.9460240546952540, 0.8605016852751490, 0.7070533756573690, 0.5243161160505560, 0.2294102723810260,
54:   -0.2198489640488240, -0.7336434870569610, -0.9451240721288890, -0.9987083965673280, -0.8075209150605320, -0.5085066436721070, -0.1270264128351010, 0.2721854747475370, 0.6055917784886910, 0.8413676398899490,
55:   0.9777625488346960, 0.9839896992562860, 0.8436921159626520, 0.5969447127483850, 0.3094984574050860, 0.0167240212732023, -0.2467178763145330, -0.4560076441356610, -0.6309543271436050, -0.7546432714599400,
56:   -0.9163549058535820, -0.9842983646399860, -0.9864836587318940, -0.8626035608908470, -0.6813902164376480, 0.1858483908516400, 0.9266383325265240, 0.9950614294774160, 0.8974629492620320, 0.7466772329212920,
57:   0.5703644678660200, 0.3750953336668390, 0.1580160207328650, -0.1758076016904010, -0.3745982816248290, -0.5276875478007350, -0.6362049492364280, -0.6890552092033720, -0.6115639640491650, -0.4788688150780810,
58:   -0.1254943141670760, -0.0616279223840421, 0.1084398125157010, 0.0507107190849194, 0.0459258492372133, -0.0603960127005548, -0.1802614924906120, -0.3445196936393600, -0.5039846767879240, -0.6418346779779860,
59:   -0.7682209569752370, -0.8811490866996920, -0.9663490613441720, -0.9999985470701830, -0.9546858649139780, -0.3854582462377580, 0.3634207386677970, 0.8289947319983170, 0.9714909193370010, 0.9983857647297360,
60:   0.9298786498974830, 0.7522877436715410, 0.5261656913352240, 0.3198410350076260, 0.2206250837743810, 0.2324067219830480, 0.2622555352188150, 0.2488016117878720, 0.2127369252136960, 0.1536174562004720,
61:   0.0365743929645966, -0.0132935129731492, -0.0205972714630062, -0.0506416644795282, -0.0877504363551195, -0.1297549156821220, -0.0930172242876989, -0.0576191763867261, -0.0229433518557876, 0.0363188766621527,
62:   0.0001870031678450, -0.0349743794742610, -0.0631993922084072, -0.0971138050129020, -0.1624979390656570, -0.2458522401310880, -0.3438638176192820, -0.4480085728769030, -0.5679531815591490, -0.6842705623709480,
63:   -0.7615364178876680, -0.8520399314791350, -0.9010432983130380, -0.9834194615045590, -0.9905476870988080, -0.8872786828920670, -0.6744804687446460, -0.3755063881128740, -0.1868453942460660, 0.0794027920237213,
64:   0.2259213674108930, 0.4304722733878400, 0.5298904205645140, 0.6239251933219030, 0.7158012315374360, 0.8037206344012380, 0.8731666378756390, 0.9219544489912210, 0.9658401621307640, 0.9953707336137810,
65:   0.9916336070524670, 0.9786230879164460, 0.7515981300962940, 0.5233956277754490, 0.1867099275394900, -0.1183826185655860, -0.6727134585677300, -0.7289894296051810, 0.5281384062949680, 0.8678795703185170,
66:   0.8298749053132090, 0.6902403611717530, 0.5240926478571590, 0.3265450140990430, 0.1410201458363470, -0.0385849224232995, -0.1727584531294190, -0.3840270653389320, -0.5839449895165980, -0.7055596595779030,
67:   -0.8654833335157070, -0.9746729156329320, -0.9963449765889340, -0.9217474051403660, -0.7639362508931000, -0.5606026149454520, -0.3510588357296890, -0.3005270942524180, -0.1156452933894830, -0.0595959289499457,
68:   -0.0194952452596040, -0.0107439009455394, -0.0101826864310280, -0.0169325310282176, -0.1277205927832380, -0.2371674472604240, -0.2446819018845580, -0.1710822516700410, -0.0511353061733825, 0.0062563192154928,
69:   -0.0048516364950122, -0.0717158401086350, -0.1252962791394740, -0.1414075684951130, -0.1259001773043880, -0.0975843207989124, -0.0694511521424164, -0.0337586943638060, -0.0159474649423128, 0.0194325975213332,
70:   0.0608001695138442, 0.1024001693533800, 0.1480876049487220, 0.2039912059729780, 0.2920915426229240, 0.3926785587852390, 0.5354672113127650, 0.6169657298756830, 0.6993887726683350, 0.7501832875129180,
71:   0.8386871644310250, 0.9103651277237890, 0.9764351312310820, 0.9926345853646890, 0.9300339420371430, 0.8258742381128520, 0.7499387624704700, 0.6725402422076670, 0.6229867037490120, 0.6045297568333980,
72:   0.5039297182908610, 0.4455885766999630, 0.3779726796046290, 0.3429218283962470, 0.2929489553699220, 0.1570518914741900, 0.0572157944020108, -0.0463361936582273, -0.1573451851308570, -0.2931672021738570,
73:   -0.4528725984930980, -0.6063286567677410, -0.6995137850617610, -0.7902112777605310, -0.9139878106324440, -0.9998170324167670, -0.9448389776183910, -0.5177630386017770, 0.3950506584043580, 0.7346367050507850,
74: ];
75: 
76: // Expected sine wave lead values taken from MBST SineWaveTest.cs. 252 entries.
77: const expectedSineLead = [
78:   Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN,
79:   Number.NaN, Number.NaN, Number.NaN,
80:   -0.5865008756606930, 0.6010760134973850, 0.0522375953518313, 0.9091393120803100, 0.8866939724449600, 0.9971590529781770, 0.9020538171846020, 0.6368324474361340, 0.8706218323553310, 0.6073789821251400,

(Showing lines 1-80 of 294. Use offset=81 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.4s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/sine-wave/sine-wave.spec.ts",
  "offset": 81
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/sine-wave/sine-wave.spec.ts</path>
<type>file</type>
<content>81:   0.7016671195580980, 0.7163213444559970, 0.7860384941578910, 0.8229422149880850, 0.8625259332042720, 0.9311800857903170, 0.9824560606058480, 0.9999415670942400, 0.9850130504072370, 0.9545149566415440,
82:   0.9699003741647460, 0.9411885260170180, 0.8957923799982550, 0.8361338691916270, 0.6425371253442400, 0.5619014189746250, 0.2222351486408030, -0.3571214380720770, -0.9310766529205810, -0.9768109870729720,
83:   -0.9433073135761630, -0.8132374819975520, -0.8689541000414960, -0.8770525415166500, 0.1475931480924170, 0.4397692495572380, 0.2482327294712000, -0.0000755239717352, -0.2313707009630830, -0.5260305525740280,
84:   -0.8452633448517760, -0.9992677882479890, -0.8993236485622310, -0.6702662015228300, -0.1539244027456790, 0.2492915015754510, 0.6115575080093740, 0.8728740226620090, 0.9909162839717420, 0.9771019730335230,
85:   0.8396734557306670, 0.5697614425887530, 0.2169861504951610, -0.1451956537134670, -0.4535393411417920, -0.6951822190514210, -0.8597041192450010, -0.9517540619903210, -0.9947396502168260, -0.9975710951353530,
86:   -0.9310629460274820, -0.8208173676831810, -0.5816830016568750, -0.2522412838591280, 0.0357301876330212, 0.8262025299696030, 0.9210718625890570, 0.6334265388777150, 0.3227045140978610, 0.0576226338781002,
87:   -0.1775030791814070, -0.3902458029200550, -0.5864888795958400, -0.8204080195335590, -0.9205013506909260, -0.9737752029989330, -0.9954119273819820, -0.9996821539441930, -0.9919003713144100, -0.9593708639942700,
88:   -0.7902545151018210, -0.7493402295361840, -0.6262584572592140, -0.6703391127989300, -0.6738861991115050, -0.7485223840691900, -0.8229875936131110, -0.9074292725287760, -0.9671080150009190, -0.9960855660349680,
89:   -0.9958932520932240, -0.9574138340514830, -0.8652041011461150, -0.7083111287566420, -0.4646190135134790, 0.3799051621364850, 0.9157357095569460, 0.9816417978833290, 0.8545864068220020, 0.6658040149144910,
90:   0.3974026651162090, 0.0660812869143209, -0.2292556000437840, -0.4438015597350790, -0.5336772503175370, -0.5234089018965820, -0.4969142899048420, -0.5089421255844810, -0.5404930402692640, -0.5900897578798300,
91:   -0.6807716781362360, -0.7164442325283580, -0.7215212414254330, -0.7420085481569570, -0.7664280384961050, -0.7928795495979390, -0.7698142295937170, -0.7466749292126270, -0.7231440467849940, -0.6809589449044260,
92:   -0.7069745376146330, -0.7314048006899320, -0.7503819371382670, -0.7724343179777090, -0.8126119368714450, -0.8592475635206920, -0.9071355135227490, -0.9489640525891650, -0.9835954633562350, -0.9994945745179860,
93:   -0.9967791122784410, -0.9726372051055730, -0.9438266487324060, -0.8236131488885440, -0.6034299982731380, -0.3012592866284200, 0.0451220549347129, 0.3898374562298360, 0.5625345127655470, 0.7610204246544270,
94:   0.8485754490678880, 0.9426271321408510, 0.9743624133056830, 0.9937744211043730, 0.9999234595478280, 0.9890496834828740, 0.9620813190845490, 0.9257815160887320, 0.8661907447689560, 0.7717933199927350,
95:   0.6099143741297350, 0.5465660379422910, 0.0650374448708587, -0.2324218737852860, -0.5626485140051470, -0.7858435966184150, -0.9988707502989160, -0.9995056053709990, -0.2269953459568470, 0.2624108067271960,
96:   0.1922802999868170, -0.0235747958138623, -0.2316259847404300, -0.4374421611852380, -0.6003241683068820, -0.7338638764656210, -0.8186335536900920, -0.9244352736103260, -0.9869359601928300, -0.9999976116346720,
97:   -0.9662055929589490, -0.8473318021865150, -0.6441207677355740, -0.3775643212631200, -0.0838943519776584, 0.1891385830833930, 0.4138657569596380, 0.4619148062355390, 0.6205889300081480, 0.6637092720958960,
98:   0.6931871751689060, 0.6994688836282570, 0.6998698746833480, 0.6950322988997850, 0.6110036127434860, 0.5192294559798360, 0.5125969121038930, 0.5757083087460280, 0.6700235759926480, 0.7115168281729820,
99:   0.7036678339992160, 0.6545752979714670, 0.6129364849611150, 0.6000111421090120, 0.6124554033545860, 0.6347294098505560, 0.6562899876128100, 0.6828327376881520, 0.6957402985880160, 0.7207141791649000,
100:   0.7487908163625680, 0.7757975773123150, 0.8040241461526770, 0.8364818144534760, 0.8828100232401670, 0.9279746557928580, 0.9758237828336110, 0.9927466296862280, 0.9999410720652520, 0.9980198650776760,
101:   0.9781412645524530, 0.9363303228513180, 0.8430454008691210, 0.6162348270656310, 0.3977902134753690, 0.1852760304055380, 0.0625305163973752, -0.0477440698701413, -0.1126037294757250, -0.1358018386690990,
102:   -0.2544275858052280, -0.3179500551407050, -0.3873842567862560, -0.4217482646499100, -0.4689384577141290, -0.5872793774815170, -0.6654907471409910, -0.7391119157698760, -0.8095586705039820, -0.8833378646001520,
103:   -0.9506681839279980, -0.9910404297342630, -0.9999429557041830, -0.9921031907559610, -0.9331896469980470, -0.6934514712647360, -0.4364996881408350, 0.2388331163456430, 0.9289332462002390, 0.9992107524543470,
104: ];
105: 
106: const tolerance = 1e-4;
107: 
108: describe('SineWave', () => {
109:   const time = new Date(2021, 3, 1);
110:   const skip = 9;
111:   const settleSkip = 177;
112: 
113:   it('should have correct output enum values', () => {
114:     expect(SineWaveOutput.Value).toBe(0);
115:     expect(SineWaveOutput.Lead).toBe(1);
116:     expect(SineWaveOutput.Band).toBe(2);
117:     expect(SineWaveOutput.DominantCyclePeriod).toBe(3);
118:     expect(SineWaveOutput.DominantCyclePhase).toBe(4);
119:   });
120: 
121:   it('should return expected mnemonic for default params (BarComponent.Median)', () => {
122:     const sw = SineWave.default();
123:     expect(sw.metadata().mnemonic).toBe('sw(0.330, hl/2)');
124:   });
125: 
126:   it('should return expected mnemonic for explicit components matching Close default', () => {
127:     const sw = SineWave.fromParams({ alphaEmaPeriodAdditional: 0.5, barComponent: BarComponent.Close });
128:     expect(sw.metadata().mnemonic).toBe('sw(0.500)');
129:   });
130: 
131:   it('should return expected mnemonic for phase accumulator', () => {
132:     const sw = SineWave.fromParams({
133:       alphaEmaPeriodAdditional: 0.5,
134:       estimatorType: HilbertTransformerCycleEstimatorType.PhaseAccumulator,
135:       estimatorParams: { smoothingLength: 4, alphaEmaQuadratureInPhase: 0.2, alphaEmaPeriod: 0.2 },
136:       barComponent: BarComponent.Close,
137:     });
138:     expect(sw.metadata().mnemonic).toBe('sw(0.500, pa(4, 0.200, 0.200))');
139:   });
140: 
141:   it('should return expected metadata', () => {
142:     const sw = SineWave.default();
143:     const meta = sw.metadata();
144:     const mn = 'sw(0.330, hl/2)';
145:     const mnLead = 'sw-lead(0.330, hl/2)';
146:     const mnBand = 'sw-band(0.330, hl/2)';
147:     const mnDCP = 'dcp(0.330, hl/2)';
148:     const mnDCPha = 'dcph(0.330, hl/2)';
149: 
150:     expect(meta.type).toBe(IndicatorType.SineWave);
151:     expect(meta.mnemonic).toBe(mn);
152:     expect(meta.description).toBe('Sine wave ' + mn);
153:     expect(meta.outputs.length).toBe(5);
154: 
155:     expect(meta.outputs[0].kind).toBe(SineWaveOutput.Value);
156:     expect(meta.outputs[0].type).toBe(OutputType.Scalar);
157:     expect(meta.outputs[0].mnemonic).toBe(mn);
158:     expect(meta.outputs[0].description).toBe('Sine wave ' + mn);
159: 
160:     expect(meta.outputs[1].kind).toBe(SineWaveOutput.Lead);
161:     expect(meta.outputs[1].type).toBe(OutputType.Scalar);
162:     expect(meta.outputs[1].mnemonic).toBe(mnLead);
163:     expect(meta.outputs[1].description).toBe('Sine wave lead ' + mnLead);
164: 
165:     expect(meta.outputs[2].kind).toBe(SineWaveOutput.Band);
166:     expect(meta.outputs[2].type).toBe(OutputType.Band);
167:     expect(meta.outputs[2].mnemonic).toBe(mnBand);
168:     expect(meta.outputs[2].description).toBe('Sine wave band ' + mnBand);
169: 
170:     expect(meta.outputs[3].kind).toBe(SineWaveOutput.DominantCyclePeriod);
171:     expect(meta.outputs[3].type).toBe(OutputType.Scalar);
172:     expect(meta.outputs[3].mnemonic).toBe(mnDCP);
173:     expect(meta.outputs[3].description).toBe('Dominant cycle period ' + mnDCP);
174: 
175:     expect(meta.outputs[4].kind).toBe(SineWaveOutput.DominantCyclePhase);
176:     expect(meta.outputs[4].type).toBe(OutputType.Scalar);
177:     expect(meta.outputs[4].mnemonic).toBe(mnDCPha);
178:     expect(meta.outputs[4].description).toBe('Dominant cycle phase ' + mnDCPha);
179:   });
180: 
181:   it('should throw if α is out of range (0, 1]', () => {
182:     expect(() => SineWave.fromParams({ alphaEmaPeriodAdditional: 0 })).toThrow();
183:     expect(() => SineWave.fromParams({ alphaEmaPeriodAdditional: -0.1 })).toThrow();
184:     expect(() => SineWave.fromParams({ alphaEmaPeriodAdditional: 1.0001 })).toThrow();
185:     expect(() => SineWave.fromParams({ alphaEmaPeriodAdditional: 0.5 })).not.toThrow();
186:     expect(() => SineWave.fromParams({ alphaEmaPeriodAdditional: 1.0 })).not.toThrow();
187:   });
188: 
189:   it('should return NaN quadruple for NaN input', () => {
190:     const sw = SineWave.default();
191:     const [v, l, p, h] = sw.update(Number.NaN);
192:     expect(Number.isNaN(v)).toBe(true);
193:     expect(Number.isNaN(l)).toBe(true);
194:     expect(Number.isNaN(p)).toBe(true);
195:     expect(Number.isNaN(h)).toBe(true);
196:   });
197: 
198:   it('should match reference sine (MBST SineWaveTest) past settle window', () => {
199:     const sw = SineWave.default();
200:     for (let i = skip; i < input.length; i++) {
201:       const [value] = sw.update(input[i]);
202:       if (Number.isNaN(value) || i < settleSkip) continue;
203:       if (Number.isNaN(expectedSine[i])) continue;
204:       expect(Math.abs(expectedSine[i] - value))
205:         .withContext(`sine[${i}]: expected ${expectedSine[i]}, actual ${value}`)
206:         .toBeLessThan(tolerance);
207:     }
208:   });
209: 
210:   it('should match reference sine lead (MBST SineWaveLeadTest) past settle window', () => {
211:     const sw = SineWave.default();
212:     for (let i = skip; i < input.length; i++) {
213:       const [, lead] = sw.update(input[i]);
214:       if (Number.isNaN(lead) || i < settleSkip) continue;
215:       if (Number.isNaN(expectedSineLead[i])) continue;
216:       expect(Math.abs(expectedSineLead[i] - lead))
217:         .withContext(`sineLead[${i}]: expected ${expectedSineLead[i]}, actual ${lead}`)
218:         .toBeLessThan(tolerance);
219:     }
220:   });
221: 
222:   it('should become primed within the input sequence', () => {
223:     const sw = SineWave.default();
224:     expect(sw.isPrimed()).toBe(false);
225: 
226:     let primedAt = -1;
227:     for (let i = 0; i < input.length; i++) {
228:       sw.update(input[i]);
229:       if (sw.isPrimed() && primedAt < 0) {
230:         primedAt = i;
231:       }
232:     }
233:     expect(primedAt).toBeGreaterThanOrEqual(0);
234:     expect(sw.isPrimed()).toBe(true);
235:   });
236: 
237:   const primeCount = 200;
238:   const value = 100.0;
239: 
240:   function checkOutput(out: any[]): void {
241:     expect(out.length).toBe(5);
242:     // indices 0, 1, 3, 4 are scalars; 2 is a band.
243:     for (const i of [0, 1, 3, 4]) {
244:       const s = out[i] as Scalar;
245:       expect(s.time).toEqual(time);
246:     }
247:     const b = out[2] as Band;
248:     expect(b.time).toEqual(time);
249:   }
250: 
251:   it('should produce 5-element output via updateScalar', () => {
252:     const sw = SineWave.default();
253:     for (let i = 0; i < primeCount; i++) sw.update(input[i % input.length]);
254:     const s = new Scalar();
255:     s.time = time;
256:     s.value = value;
257:     checkOutput(sw.updateScalar(s) as any[]);
258:   });
259: 
260:   it('should produce 5-element output via updateBar', () => {
261:     const sw = SineWave.default();
262:     for (let i = 0; i < primeCount; i++) sw.update(input[i % input.length]);
263:     const bar = new Bar({ time, open: value, high: value, low: value, close: value, volume: 0 });
264:     checkOutput(sw.updateBar(bar) as any[]);
265:   });
266: 
267:   it('should produce 5-element output via updateQuote', () => {
268:     const sw = SineWave.default();
269:     for (let i = 0; i < primeCount; i++) sw.update(input[i % input.length]);
270:     const q = new Quote({ time, bid: value, ask: value, bidSize: 0, askSize: 0 });
271:     checkOutput(sw.updateQuote(q) as any[]);
272:   });
273: 
274:   it('should produce 5-element output via updateTrade', () => {
275:     const sw = SineWave.default();
276:     for (let i = 0; i < primeCount; i++) sw.update(input[i % input.length]);
277:     const t = new Trade({ time, price: value, volume: 0 });
278:     checkOutput(sw.updateTrade(t) as any[]);
279:   });
280: 
281:   it('should order band as {upper: value, lower: lead}', () => {
282:     const sw = SineWave.default();
283:     for (let i = 0; i < primeCount; i++) sw.update(input[i % input.length]);
284:     const s = new Scalar();
285:     s.time = time;
286:     s.value = input[0];
287:     const out = sw.updateScalar(s) as any[];
288:     const sv = out[SineWaveOutput.Value] as Scalar;
289:     const sl = out[SineWaveOutput.Lead] as Scalar;
290:     const band = out[SineWaveOutput.Band] as Band;
291:     expect(band.upper).toBe(sv.value);
292:     expect(band.lower).toBe(sl.value);
293:   });
294: });

(End of file - total 294 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.7s)

_Thinking:_

 Now I need to check the Go test file to pull the MBST reference data arrays that I can use.

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go",
  "offset": 1,
  "limit": 80
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

(Showing lines 1-80 of 906. Use offset=81 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.7s)

_Thinking:_

 I need to understand the structure of this test file first before deciding whether to write a conversion script or just port it directly in TypeScript.

**Tool: bash**

**Input:**
```json
{
  "command": "wc -l go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go",
  "description": "Check file line count"
}
```

**Output:**
```
906 go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go

```

---

## Assistant (Build · claude-opus-4.7 · 5.8s)

**Tool: grep**

**Input:**
```json
{
  "path": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go",
  "pattern": "^func |^// test"
}
```

**Output:**
```
Found 22 matches
/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go:
  Line 16: func testTCMTime() time.Time {
  Line 20: // testTCMInput is the rawInput from MBST TrendCycleModeTest.cs (TA-Lib test_MAMA.xsl, Price).
  Line 23: func testTCMInput() []float64 {
  Line 54: // testTCMExpectedPeriod is the dcPeriod from MBST TrendCycleModeTest.cs.
  Line 57: func testTCMExpectedPeriod() []float64 {
  Line 88: // testTCMExpectedPhase is the dcPhase from MBST TrendCycleModeTest.cs.
  Line 91: func testTCMExpectedPhase() []float64 {
  Line 122: // testTCMExpectedSine is the sine array from MBST TrendCycleModeTest.cs.
  Line 125: func testTCMExpectedSine() []float64 {
  Line 156: // testTCMExpectedSineLead is the sineLead array from MBST TrendCycleModeTest.cs.
  Line 159: func testTCMExpectedSineLead() []float64 {
  Line 190: // testTCMExpectedITL is the itl array from MBST TrendCycleModeTest.cs.
  Line 193: func testTCMExpectedITL() []float64 {
  Line 224: // testTCMExpectedValue is the tcm array from MBST TrendCycleModeTest.cs (first 63 entries NaN).
  Line 227: func testTCMExpectedValue() []float64 {
  Line 249: func TestTrendCycleModeUpdate(t *testing.T) {
  Line 420: func TestTrendCycleModeIsPrimed(t *testing.T) {
  Line 455: func TestTrendCycleModeMetadata(t *testing.T) {
  Line 525: func TestTrendCycleModeUpdateEntity(t *testing.T) {
  Line 614: func TestNewTrendCycleMode(t *testing.T) {
  Line 886: func testTCMCreateCycleEstimatorParams() hilberttransformer.CycleEstimatorParams {
  Line 902: func testTCMCreateDefault() *TrendCycleMode {
```

---

## Assistant (Build · claude-opus-4.7 · 4.8s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go",
  "limit": 170,
  "offset": 88
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go</path>
<type>file</type>
<content>88: // testTCMExpectedPhase is the dcPhase from MBST TrendCycleModeTest.cs.
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

(Showing lines 88-257 of 906. Use offset=258 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 4.7s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go",
  "limit": 370,
  "offset": 258
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go</path>
<type>file</type>
<content>258: 	expValue := testTCMExpectedValue()
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

(Showing lines 258-627 of 906. Use offset=628 to continue.)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.7s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go",
  "offset": 628
}
```

**Output:**
```
<path>/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go</path>
<type>file</type>
<content>628: 		errSep = "invalid trend cycle mode parameters: " +
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

## Assistant (Build · claude-opus-4.7 · 11.2s)

_Thinking:_

 I'm verifying that all six arrays are present in the data and checking the line ranges to confirm completeness—the phase data spans lines 92-119, the sine data goes through line 153, and the sineLead data extends beyond line 187.

Now I need to examine the HTITL TS files as a structural reference and look at what an output enum with 8 kinds looks like, plus check the IndicatorType for TrendCycleMode.

**Tool: grep**

**Input:**
```json
{
  "path": "/home/dev/zpano/ts/indicators/core/indicator-type.ts",
  "pattern": "TrendCycleMode"
}
```

**Output:**
```
Found 1 matches
/home/dev/zpano/ts/indicators/core/indicator-type.ts:
  Line 189:   TrendCycleMode,
```

---

## Assistant (Build · claude-opus-4.7 · 4.8s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/hilbert-transformer-instantaneous-trend-line/hilbert-transformer-instantaneous-trend-line.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/hilbert-transformer-instantaneous-trend-line/hilbert-transformer-instantaneous-trend-line.ts</path>
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
14: import { HilbertTransformerCycleEstimator } from '../hilbert-transformer/hilbert-transformer-cycle-estimator';
15: import { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';
16: import { createEstimator, estimatorMoniker } from '../hilbert-transformer/hilbert-transformer-common';
17: import { HilbertTransformerInstantaneousTrendLineOutput } from './hilbert-transformer-instantaneous-trend-line-output';
18: import { HilbertTransformerInstantaneousTrendLineParams } from './hilbert-transformer-instantaneous-trend-line-params';
19: 
20: const DEFAULT_ALPHA_EMA_PERIOD_ADDITIONAL = 0.33;
21: const DEFAULT_SMOOTHING_LENGTH = 4;
22: const DEFAULT_ALPHA_EMA_QI = 0.2;
23: const DEFAULT_ALPHA_EMA_PERIOD = 0.2;
24: // MBST's DominantCyclePeriod default warm-up is MaxPeriod * 2 = 100.
25: const DEFAULT_WARM_UP_PERIOD = 100;
26: const DEFAULT_TREND_LINE_SMOOTHING_LENGTH = 4;
27: const DEFAULT_CYCLE_PART_MULTIPLIER = 1.0;
28: const MAX_CYCLE_PART_MULTIPLIER = 10.0;
29: 
30: /** __Hilbert Transformer Instantaneous Trend Line__ (Ehlers) computes a smoothed trend line
31:  * derived from averaging over a window whose length tracks the smoothed dominant cycle period
32:  * produced by a Hilbert transformer cycle estimator.
33:  *
34:  * It exposes two outputs:
35:  *
36:  *	- Value: the instantaneous trend line value, a length-`trendLineSmoothingLength` WMA of
37:  *	  simple averages over the last `round(smoothedPeriod·cyclePartMultiplier)` raw input samples.
38:  *	- DominantCyclePeriod: the additionally EMA-smoothed dominant cycle period.
39:  *
40:  * Reference:
41:  *
42:  *	John Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 107-112.
43:  */
44: export class HilbertTransformerInstantaneousTrendLine implements Indicator {
45:   private readonly htce: HilbertTransformerCycleEstimator;
46:   private readonly alphaEmaPeriodAdditional: number;
47:   private readonly oneMinAlphaEmaPeriodAdditional: number;
48:   private readonly cyclePartMultiplier: number;
49:   private readonly trendLineSmoothingLength: number;
50:   private readonly coeff0: number;
51:   private readonly coeff1: number;
52:   private readonly coeff2: number;
53:   private readonly coeff3: number;
54:   private readonly input: number[];
55:   private readonly inputLength: number;
56:   private readonly inputLengthMin1: number;
57:   private smoothedPeriod = 0;
58:   private value = Number.NaN;
59:   private average1 = 0;
60:   private average2 = 0;
61:   private average3 = 0;
62:   private primed = false;
63: 
64:   private readonly mnemonicValue: string;
65:   private readonly descriptionValue: string;
66:   private readonly mnemonicDCP: string;
67:   private readonly descriptionDCP: string;
68: 
69:   private readonly barComponentFunc: (bar: Bar) => number;
70:   private readonly quoteComponentFunc: (quote: Quote) => number;
71:   private readonly tradeComponentFunc: (trade: Trade) => number;
72: 
73:   /** Creates an instance using default parameters (α=0.33, trendLineSmoothingLength=4,
74:    * cyclePartMultiplier=1.0, HomodyneDiscriminator cycle estimator with smoothingLength=4,
75:    * αq=0.2, αp=0.2, warmUpPeriod=100, BarComponent.Median). */
76:   public static default(): HilbertTransformerInstantaneousTrendLine {
77:     return new HilbertTransformerInstantaneousTrendLine({
78:       alphaEmaPeriodAdditional: DEFAULT_ALPHA_EMA_PERIOD_ADDITIONAL,
79:       trendLineSmoothingLength: DEFAULT_TREND_LINE_SMOOTHING_LENGTH,
80:       cyclePartMultiplier: DEFAULT_CYCLE_PART_MULTIPLIER,
81:       estimatorType: HilbertTransformerCycleEstimatorType.HomodyneDiscriminator,
82:       estimatorParams: {
83:         smoothingLength: DEFAULT_SMOOTHING_LENGTH,
84:         alphaEmaQuadratureInPhase: DEFAULT_ALPHA_EMA_QI,
85:         alphaEmaPeriod: DEFAULT_ALPHA_EMA_PERIOD,
86:         warmUpPeriod: DEFAULT_WARM_UP_PERIOD,
87:       },
88:     });
89:   }
90: 
91:   /** Creates an instance based on the given parameters. */
92:   public static fromParams(
93:     params: HilbertTransformerInstantaneousTrendLineParams,
94:   ): HilbertTransformerInstantaneousTrendLine {
95:     return new HilbertTransformerInstantaneousTrendLine(params);
96:   }
97: 
98:   private constructor(params: HilbertTransformerInstantaneousTrendLineParams) {
99:     const alpha = params.alphaEmaPeriodAdditional;
100:     if (alpha <= 0 || alpha > 1) {
101:       throw new Error(
102:         'invalid hilbert transformer instantaneous trend line parameters: '
103:           + 'α for additional smoothing should be in range (0, 1]',
104:       );
105:     }
106: 
107:     const tlsl = params.trendLineSmoothingLength ?? DEFAULT_TREND_LINE_SMOOTHING_LENGTH;
108:     if (tlsl < 2 || tlsl > 4 || !Number.isInteger(tlsl)) {
109:       throw new Error(
110:         'invalid hilbert transformer instantaneous trend line parameters: '
111:           + 'trend line smoothing length should be 2, 3, or 4',
112:       );
113:     }
114: 
115:     const cpm = params.cyclePartMultiplier ?? DEFAULT_CYCLE_PART_MULTIPLIER;
116:     if (cpm <= 0 || cpm > MAX_CYCLE_PART_MULTIPLIER) {
117:       throw new Error(
118:         'invalid hilbert transformer instantaneous trend line parameters: '
119:           + 'cycle part multiplier should be in range (0, 10]',
120:       );
121:     }
122: 
123:     this.alphaEmaPeriodAdditional = alpha;
124:     this.oneMinAlphaEmaPeriodAdditional = 1 - alpha;
125:     this.cyclePartMultiplier = cpm;
126:     this.trendLineSmoothingLength = tlsl;
127: 
128:     // Default to BarComponent.Median (MBST default; always shown in mnemonic).
129:     const bc = params.barComponent ?? BarComponent.Median;
130:     const qc = params.quoteComponent ?? DefaultQuoteComponent;
131:     const tc = params.tradeComponent ?? DefaultTradeComponent;
132: 
133:     this.barComponentFunc = barComponentValue(bc);
134:     this.quoteComponentFunc = quoteComponentValue(qc);
135:     this.tradeComponentFunc = tradeComponentValue(tc);
136: 
137:     this.htce = createEstimator(params.estimatorType, params.estimatorParams);
138: 
139:     const effectiveType = params.estimatorType ?? HilbertTransformerCycleEstimatorType.HomodyneDiscriminator;
140:     let em = '';
141:     const isDefaultHd = effectiveType === HilbertTransformerCycleEstimatorType.HomodyneDiscriminator
142:       && this.htce.smoothingLength === DEFAULT_SMOOTHING_LENGTH
143:       && this.htce.alphaEmaQuadratureInPhase === DEFAULT_ALPHA_EMA_QI
144:       && this.htce.alphaEmaPeriod === DEFAULT_ALPHA_EMA_PERIOD;
145:     if (!isDefaultHd) {
146:       const moniker = estimatorMoniker(effectiveType, this.htce);
147:       if (moniker.length > 0) {
148:         em = ', ' + moniker;
149:       }
150:     }
151: 
152:     const cm = componentTripleMnemonic(bc, qc, tc);
153:     const a = alpha.toFixed(3);
154:     const c = cpm.toFixed(3);
155: 
156:     this.mnemonicValue = `htitl(${a}, ${tlsl}, ${c}${em}${cm})`;
157:     this.mnemonicDCP = `dcp(${a}${em}${cm})`;
158: 
159:     this.descriptionValue = 'Hilbert transformer instantaneous trend line ' + this.mnemonicValue;
160:     this.descriptionDCP = 'Dominant cycle period ' + this.mnemonicDCP;
161: 
162:     // WMA coefficients.
163:     let c0 = 0, c1 = 0, c2 = 0, c3 = 0;
164:     if (tlsl === 2) {
165:       c0 = 2 / 3;
166:       c1 = 1 / 3;
167:     } else if (tlsl === 3) {
168:       c0 = 3 / 6;
169:       c1 = 2 / 6;
170:       c2 = 1 / 6;
171:     } else { // tlsl === 4
172:       c0 = 4 / 10;
173:       c1 = 3 / 10;
174:       c2 = 2 / 10;
175:       c3 = 1 / 10;
176:     }
177:     this.coeff0 = c0;
178:     this.coeff1 = c1;
179:     this.coeff2 = c2;
180:     this.coeff3 = c3;
181: 
182:     const maxPeriod = this.htce.maxPeriod;
183:     this.input = new Array<number>(maxPeriod).fill(0);
184:     this.inputLength = maxPeriod;
185:     this.inputLengthMin1 = maxPeriod - 1;
186:   }
187: 
188:   /** Indicates whether the indicator is primed. */
189:   public isPrimed(): boolean { return this.primed; }
190: 
191:   /** Describes the output data of the indicator. */
192:   public metadata(): IndicatorMetadata {
193:     return {
194:       type: IndicatorType.HilbertTransformerInstantaneousTrendLine,
195:       mnemonic: this.mnemonicValue,
196:       description: this.descriptionValue,
197:       outputs: [
198:         {
199:           kind: HilbertTransformerInstantaneousTrendLineOutput.Value,
200:           type: OutputType.Scalar,
201:           mnemonic: this.mnemonicValue,
202:           description: this.descriptionValue,
203:         },
204:         {
205:           kind: HilbertTransformerInstantaneousTrendLineOutput.DominantCyclePeriod,
206:           type: OutputType.Scalar,
207:           mnemonic: this.mnemonicDCP,
208:           description: this.descriptionDCP,
209:         },
210:       ],
211:     };
212:   }
213: 
214:   /** Updates the indicator given the next sample value. Returns the pair
215:    * (value, period). Returns (NaN, NaN) if not yet primed. */
216:   public update(sample: number): [number, number] {
217:     if (Number.isNaN(sample)) {
218:       return [sample, sample];
219:     }
220: 
221:     this.htce.update(sample);
222:     this.pushInput(sample);
223: 
224:     if (this.primed) {
225:       this.smoothedPeriod = this.alphaEmaPeriodAdditional * this.htce.period
226:         + this.oneMinAlphaEmaPeriodAdditional * this.smoothedPeriod;
227:       const average = this.calculateAverage();
228:       this.value = this.coeff0 * average
229:         + this.coeff1 * this.average1
230:         + this.coeff2 * this.average2
231:         + this.coeff3 * this.average3;
232:       this.average3 = this.average2;
233:       this.average2 = this.average1;
234:       this.average1 = average;
235:       return [this.value, this.smoothedPeriod];
236:     }
237: 
238:     if (this.htce.primed) {
239:       this.primed = true;
240:       this.smoothedPeriod = this.htce.period;
241:       const avg = this.calculateAverage();
242:       this.value = avg;
243:       this.average1 = avg;
244:       this.average2 = avg;
245:       this.average3 = avg;
246:       return [this.value, this.smoothedPeriod];
247:     }
248: 
249:     return [Number.NaN, Number.NaN];
250:   }
251: 
252:   /** Updates an indicator given the next scalar sample. */
253:   public updateScalar(sample: Scalar): IndicatorOutput {
254:     return this.updateEntity(sample.time, sample.value);
255:   }
256: 
257:   /** Updates an indicator given the next bar sample. */
258:   public updateBar(sample: Bar): IndicatorOutput {
259:     return this.updateEntity(sample.time, this.barComponentFunc(sample));
260:   }
261: 
262:   /** Updates an indicator given the next quote sample. */
263:   public updateQuote(sample: Quote): IndicatorOutput {
264:     return this.updateEntity(sample.time, this.quoteComponentFunc(sample));
265:   }
266: 
267:   /** Updates an indicator given the next trade sample. */
268:   public updateTrade(sample: Trade): IndicatorOutput {
269:     return this.updateEntity(sample.time, this.tradeComponentFunc(sample));
270:   }
271: 
272:   private updateEntity(time: Date, sample: number): IndicatorOutput {
273:     const [value, period] = this.update(sample);
274: 
275:     const sv = new Scalar();
276:     sv.time = time;
277:     sv.value = value;
278: 
279:     const sp = new Scalar();
280:     sp.time = time;
281:     sp.value = period;
282: 
283:     return [sv, sp];
284:   }
285: 
286:   private pushInput(value: number): void {
287:     for (let i = this.inputLengthMin1; i > 0; i--) {
288:       this.input[i] = this.input[i - 1];
289:     }
290:     this.input[0] = value;
291:   }
292: 
293:   private calculateAverage(): number {
294:     // Compute simple average over a window tracking the smoothed dominant cycle period.
295:     let length = Math.floor(this.smoothedPeriod * this.cyclePartMultiplier + 0.5);
296:     if (length > this.inputLength) {
297:       length = this.inputLength;
298:     } else if (length < 1) {
299:       length = 1;
300:     }
301: 
302:     let sum = 0;
303:     for (let i = 0; i < length; i++) {
304:       sum += this.input[i];
305:     }
306:     return sum / length;
307:   }
308: }

(End of file - total 308 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 15.2s)

_Thinking:_

 Now I'm deciding how to handle error wrapping when DominantCycle throws in the TypeScript version—whether to catch and rewrap with our own prefix like the Go version does, or let the error propagate as-is.

Looking at the existing SineWave implementation, it doesn't do any special error wrapping, so I'll keep it simple and match the Go behavior: catch DominantCycle errors and wrap them with the "invalid trend cycle mode parameters:" prefix.

For the output enum, I'll follow TypeScript convention and start at 0 instead of 1 like Go does, so the outputs will be indexed 0 through 7. Now I'm ready to write the four TypeScript files.

**Tool: bash**

**Input:**
```json
{
  "command": "mkdir -p ts/indicators/john-ehlers/trend-cycle-mode",
  "description": "Create TS trend-cycle-mode directory"
}
```

---

## Assistant (Build · claude-opus-4.7 · 8.8s)

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-output.ts",
  "content": "/** Enumerates outputs of the TrendCycleMode indicator. */\nexport enum TrendCycleModeOutput {\n  /** +1 in trend mode, -1 in cycle mode. */\n  Value = 0,\n  /** 1 if the trend mode is declared, 0 otherwise. */\n  IsTrendMode = 1,\n  /** 1 if the cycle mode is declared, 0 otherwise (= 1 − IsTrendMode). */\n  IsCycleMode = 2,\n  /** The WMA-smoothed instantaneous trend line. */\n  InstantaneousTrendLine = 3,\n  /** The sine wave value, sin(phase·Deg2Rad). */\n  SineWave = 4,\n  /** The sine wave lead value, sin((phase+45)·Deg2Rad). */\n  SineWaveLead = 5,\n  /** The smoothed dominant cycle period. */\n  DominantCyclePeriod = 6,\n  /** The dominant cycle phase, in degrees. */\n  DominantCyclePhase = 7,\n}\n"
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

## Assistant (Build · claude-opus-4.7 · 32.3s)

Unrelated LSP noise from mbst-to-convert, ignoring.

**Tool: write**

**Input:**
```json
{
  "content": "import { BarComponent } from '../../../entities/bar-component';\nimport { QuoteComponent } from '../../../entities/quote-component';\nimport { TradeComponent } from '../../../entities/trade-component';\nimport { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';\nimport { HilbertTransformerCycleEstimatorParams } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-params';\n\n/** Describes parameters to create an instance of the TrendCycleMode indicator. */\nexport interface TrendCycleModeParams {\n  /** The type of cycle estimator to use.\n   *\n   * The default value is HilbertTransformerCycleEstimatorType.HomodyneDiscriminator.\n   */\n  estimatorType?: HilbertTransformerCycleEstimatorType;\n\n  /** Parameters to create an instance of the Hilbert transformer cycle estimator. */\n  estimatorParams?: HilbertTransformerCycleEstimatorParams;\n\n  /** The value of α (0 < α ≤ 1) used in EMA for additional smoothing of the instantaneous period.\n   *\n   * The default value is 0.33.\n   */\n  alphaEmaPeriodAdditional: number;\n\n  /** The additional WMA smoothing length used to smooth the trend line.\n   *\n   * The valid values are 2, 3, 4. The default value is 4.\n   */\n  trendLineSmoothingLength?: number;\n\n  /** The multiplier to the dominant cycle period used to determine the window length to\n   * calculate the trend line. Typical values are in [0.5, 1.5].\n   *\n   * The default value is 1.0. Valid range is (0, 10].\n   */\n  cyclePartMultiplier?: number;\n\n  /** The threshold (in percent) above which a wide separation between the WMA-smoothed\n   * price and the instantaneous trend line forces the trend mode.\n   *\n   * The default value is 1.5. Valid range is (0, 100].\n   */\n  separationPercentage?: number;\n\n  /** A component of a bar to use when updating the indicator with a bar sample.\n   *\n   * If _undefined_, the default (BarComponent.Median, hl/2) is used. Since the default\n   * differs from the framework default bar component, it is always shown in the mnemonic.\n   */\n  barComponent?: BarComponent;\n\n  /** A component of a quote to use when updating the indicator with a quote sample.\n   *\n   * If _undefined_, a default value is used and the component is not shown in the indicator mnemonic.\n   */\n  quoteComponent?: QuoteComponent;\n\n  /** A component of a trade to use when updating the indicator with a trade sample.\n   *\n   * If _undefined_, a default value is used and the component is not shown in the indicator mnemonic.\n   */\n  tradeComponent?: TradeComponent;\n}\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-params.ts"
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

## Assistant (Build · claude-opus-4.7 · 9.8s)

_Thinking:_

 Writing the TrendCycleMode implementation, mapping Go's DominantCycle wrapper to TypeScript with the smoothed price getter and error handling for invalid parameters.

**Tool: grep**

**Input:**
```json
{
  "path": "/home/dev/zpano/ts/entities/bar-component.ts",
  "pattern": "barComponentValue|throw"
}
```

**Output:**
```
Found 1 matches
/home/dev/zpano/ts/entities/bar-component.ts:
  Line 37: export const barComponentValue = (component: BarComponent): (bar: Bar) => number => {
```

---

## Assistant (Build · claude-opus-4.7 · 3.9s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/entities/bar-component.ts",
  "offset": 30
}
```

**Output:**
```
<path>/home/dev/zpano/ts/entities/bar-component.ts</path>
<type>file</type>
<content>30:   Average,
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

## Assistant (Build · claude-opus-4.7 · 87.6s)

_Thinking:_

 I see that `barComponentValue` defaults silently to "close" rather than throwing an error, so there's no natural error path to test in TypeScript. I'll skip testing those invalid component scenarios in TS, following the pattern from SineWave, and focus on the other error cases like invalid α, tlsl, cpm, and sep.

Now I'm moving on to write the trend-cycle-mode.ts file.

**Tool: write**

**Input:**
```json
{
  "content": "import { Bar } from '../../../entities/bar';\nimport { BarComponent, barComponentValue } from '../../../entities/bar-component';\nimport { Quote } from '../../../entities/quote';\nimport { DefaultQuoteComponent, quoteComponentValue } from '../../../entities/quote-component';\nimport { Scalar } from '../../../entities/scalar';\nimport { Trade } from '../../../entities/trade';\nimport { DefaultTradeComponent, tradeComponentValue } from '../../../entities/trade-component';\nimport { componentTripleMnemonic } from '../../core/component-triple-mnemonic';\nimport { Indicator } from '../../core/indicator';\nimport { IndicatorMetadata } from '../../core/indicator-metadata';\nimport { IndicatorOutput } from '../../core/indicator-output';\nimport { IndicatorType } from '../../core/indicator-type';\nimport { OutputType } from '../../core/outputs/output-type';\nimport { DominantCycle } from '../dominant-cycle/dominant-cycle';\nimport { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';\nimport { createEstimator, estimatorMoniker } from '../hilbert-transformer/hilbert-transformer-common';\nimport { TrendCycleModeOutput } from './trend-cycle-mode-output';\nimport { TrendCycleModeParams } from './trend-cycle-mode-params';\n\nconst DEFAULT_ALPHA_EMA_PERIOD_ADDITIONAL = 0.33;\nconst DEFAULT_SMOOTHING_LENGTH = 4;\nconst DEFAULT_ALPHA_EMA_QI = 0.2;\nconst DEFAULT_ALPHA_EMA_PERIOD = 0.2;\n// MBST's DominantCyclePeriod default warm-up is MaxPeriod * 2 = 100.\nconst DEFAULT_WARM_UP_PERIOD = 100;\nconst DEFAULT_TREND_LINE_SMOOTHING_LENGTH = 4;\nconst DEFAULT_CYCLE_PART_MULTIPLIER = 1.0;\nconst DEFAULT_SEPARATION_PERCENTAGE = 1.5;\nconst MAX_CYCLE_PART_MULTIPLIER = 10.0;\nconst MAX_SEPARATION_PERCENTAGE = 100.0;\n\nconst DEG2RAD = Math.PI / 180.0;\nconst LEAD_OFFSET = 45.0;\nconst FULL_CYCLE = 360.0;\nconst EPSILON = 1e-308;\n\n/** __Trend versus Cycle Mode__ (Ehlers) classifies the market as in-trend or in-cycle based\n * on the behaviour of the instantaneous dominant cycle period/phase and a WMA-smoothed\n * instantaneous trend line.\n *\n * It exposes eight outputs:\n *\n *\t- Value: +1 in trend mode, -1 in cycle mode.\n *\t- IsTrendMode: 1 if the trend mode is declared, 0 otherwise.\n *\t- IsCycleMode: 1 if the cycle mode is declared, 0 otherwise (= 1 − IsTrendMode).\n *\t- InstantaneousTrendLine: the WMA-smoothed instantaneous trend line.\n *\t- SineWave: sin(phase·Deg2Rad).\n *\t- SineWaveLead: sin((phase+45)·Deg2Rad).\n *\t- DominantCyclePeriod: the smoothed dominant cycle period.\n *\t- DominantCyclePhase: the dominant cycle phase, in degrees.\n *\n * Reference:\n *\n *\tJohn Ehlers, Rocket Science for Traders, Wiley, 2001, 0471405671, pp 113-118.\n */\nexport class TrendCycleMode implements Indicator {\n  private readonly dc: DominantCycle;\n  private readonly cyclePartMultiplier: number;\n  private readonly separationFactor: number;\n  private readonly coeff0: number;\n  private readonly coeff1: number;\n  private readonly coeff2: number;\n  private readonly coeff3: number;\n  private readonly input: number[];\n  private readonly inputLength: number;\n  private readonly inputLengthMin1: number;\n\n  private trendline = Number.NaN;\n  private trendAverage1 = 0;\n  private trendAverage2 = 0;\n  private trendAverage3 = 0;\n  private sinWave = Number.NaN;\n  private sinWaveLead = Number.NaN;\n  private prevDcPhase = 0;\n  private prevSineLeadWaveDifference = 0;\n  private samplesInTrend = 0;\n  private isTrendMode = true;\n  private primed = false;\n\n  private readonly mnemonicValue: string;\n  private readonly descriptionValue: string;\n  private readonly mnemonicTrend: string;\n  private readonly descriptionTrend: string;\n  private readonly mnemonicCycle: string;\n  private readonly descriptionCycle: string;\n  private readonly mnemonicITL: string;\n  private readonly descriptionITL: string;\n  private readonly mnemonicSine: string;\n  private readonly descriptionSine: string;\n  private readonly mnemonicSineLead: string;\n  private readonly descriptionSineLead: string;\n  private readonly mnemonicDCP: string;\n  private readonly descriptionDCP: string;\n  private readonly mnemonicDCPhase: string;\n  private readonly descriptionDCPhase: string;\n\n  private readonly barComponentFunc: (bar: Bar) => number;\n  private readonly quoteComponentFunc: (quote: Quote) => number;\n  private readonly tradeComponentFunc: (trade: Trade) => number;\n\n  /** Creates an instance using default parameters (α=0.33, trendLineSmoothingLength=4,\n   * cyclePartMultiplier=1.0, separationPercentage=1.5, HomodyneDiscriminator cycle estimator\n   * with smoothingLength=4, αq=0.2, αp=0.2, warmUpPeriod=100, BarComponent.Median). */\n  public static default(): TrendCycleMode {\n    return new TrendCycleMode({\n      alphaEmaPeriodAdditional: DEFAULT_ALPHA_EMA_PERIOD_ADDITIONAL,\n      trendLineSmoothingLength: DEFAULT_TREND_LINE_SMOOTHING_LENGTH,\n      cyclePartMultiplier: DEFAULT_CYCLE_PART_MULTIPLIER,\n      separationPercentage: DEFAULT_SEPARATION_PERCENTAGE,\n      estimatorType: HilbertTransformerCycleEstimatorType.HomodyneDiscriminator,\n      estimatorParams: {\n        smoothingLength: DEFAULT_SMOOTHING_LENGTH,\n        alphaEmaQuadratureInPhase: DEFAULT_ALPHA_EMA_QI,\n        alphaEmaPeriod: DEFAULT_ALPHA_EMA_PERIOD,\n        warmUpPeriod: DEFAULT_WARM_UP_PERIOD,\n      },\n    });\n  }\n\n  /** Creates an instance based on the given parameters. */\n  public static fromParams(params: TrendCycleModeParams): TrendCycleMode {\n    return new TrendCycleMode(params);\n  }\n\n  private constructor(params: TrendCycleModeParams) {\n    const alpha = params.alphaEmaPeriodAdditional;\n    if (alpha <= 0 || alpha > 1) {\n      throw new Error('invalid trend cycle mode parameters: α for additional smoothing should be in range (0, 1]');\n    }\n\n    const tlsl = params.trendLineSmoothingLength ?? DEFAULT_TREND_LINE_SMOOTHING_LENGTH;\n    if (tlsl < 2 || tlsl > 4 || !Number.isInteger(tlsl)) {\n      throw new Error('invalid trend cycle mode parameters: trend line smoothing length should be 2, 3, or 4');\n    }\n\n    const cpm = params.cyclePartMultiplier ?? DEFAULT_CYCLE_PART_MULTIPLIER;\n    if (cpm <= 0 || cpm > MAX_CYCLE_PART_MULTIPLIER) {\n      throw new Error('invalid trend cycle mode parameters: cycle part multiplier should be in range (0, 10]');\n    }\n\n    const sep = params.separationPercentage ?? DEFAULT_SEPARATION_PERCENTAGE;\n    if (sep <= 0 || sep > MAX_SEPARATION_PERCENTAGE) {\n      throw new Error('invalid trend cycle mode parameters: separation percentage should be in range (0, 100]');\n    }\n\n    this.cyclePartMultiplier = cpm;\n    this.separationFactor = sep / 100.0;\n\n    // TCM defaults to BarComponent.Median (MBST default; always shown in mnemonic).\n    const bc = params.barComponent ?? BarComponent.Median;\n    const qc = params.quoteComponent ?? DefaultQuoteComponent;\n    const tc = params.tradeComponent ?? DefaultTradeComponent;\n\n    this.barComponentFunc = barComponentValue(bc);\n    this.quoteComponentFunc = quoteComponentValue(qc);\n    this.tradeComponentFunc = tradeComponentValue(tc);\n\n    // Inner DominantCycle built with explicit components so its mnemonic contains hl/2.\n    this.dc = DominantCycle.fromParams({\n      alphaEmaPeriodAdditional: alpha,\n      estimatorType: params.estimatorType,\n      estimatorParams: params.estimatorParams,\n      barComponent: bc,\n      quoteComponent: qc,\n      tradeComponent: tc,\n    });\n\n    // Compose estimator moniker (same logic as DominantCycle / SineWave).\n    const effectiveType = params.estimatorType ?? HilbertTransformerCycleEstimatorType.HomodyneDiscriminator;\n    const htce = createEstimator(params.estimatorType, params.estimatorParams);\n    let em = '';\n    const isDefaultHd = effectiveType === HilbertTransformerCycleEstimatorType.HomodyneDiscriminator\n      && htce.smoothingLength === DEFAULT_SMOOTHING_LENGTH\n      && htce.alphaEmaQuadratureInPhase === DEFAULT_ALPHA_EMA_QI\n      && htce.alphaEmaPeriod === DEFAULT_ALPHA_EMA_PERIOD;\n    if (!isDefaultHd) {\n      const moniker = estimatorMoniker(effectiveType, htce);\n      if (moniker.length > 0) {\n        em = ', ' + moniker;\n      }\n    }\n\n    const cm = componentTripleMnemonic(bc, qc, tc);\n    const a = alpha.toFixed(3);\n    const c = cpm.toFixed(3);\n    const s = sep.toFixed(3);\n    const tail = `${a}, ${tlsl}, ${c}, ${s}%${em}${cm}`;\n\n    this.mnemonicValue = `tcm(${tail})`;\n    this.mnemonicTrend = `tcm-trend(${tail})`;\n    this.mnemonicCycle = `tcm-cycle(${tail})`;\n    this.mnemonicITL = `tcm-itl(${tail})`;\n    this.mnemonicSine = `tcm-sine(${tail})`;\n    this.mnemonicSineLead = `tcm-sineLead(${tail})`;\n    this.mnemonicDCP = `dcp(${a}${em}${cm})`;\n    this.mnemonicDCPhase = `dcph(${a}${em}${cm})`;\n\n    this.descriptionValue = 'Trend versus cycle mode ' + this.mnemonicValue;\n    this.descriptionTrend = 'Trend versus cycle mode, is-trend flag ' + this.mnemonicTrend;\n    this.descriptionCycle = 'Trend versus cycle mode, is-cycle flag ' + this.mnemonicCycle;\n    this.descriptionITL = 'Trend versus cycle mode instantaneous trend line ' + this.mnemonicITL;\n    this.descriptionSine = 'Trend versus cycle mode sine wave ' + this.mnemonicSine;\n    this.descriptionSineLead = 'Trend versus cycle mode sine wave lead ' + this.mnemonicSineLead;\n    this.descriptionDCP = 'Dominant cycle period ' + this.mnemonicDCP;\n    this.descriptionDCPhase = 'Dominant cycle phase ' + this.mnemonicDCPhase;\n\n    // WMA coefficients.\n    let c0 = 0, c1 = 0, c2 = 0, c3 = 0;\n    if (tlsl === 2) {\n      c0 = 2 / 3; c1 = 1 / 3;\n    } else if (tlsl === 3) {\n      c0 = 3 / 6; c1 = 2 / 6; c2 = 1 / 6;\n    } else { // tlsl === 4\n      c0 = 4 / 10; c1 = 3 / 10; c2 = 2 / 10; c3 = 1 / 10;\n    }\n    this.coeff0 = c0;\n    this.coeff1 = c1;\n    this.coeff2 = c2;\n    this.coeff3 = c3;\n\n    const maxPeriod = this.dc.maxPeriod;\n    this.input = new Array<number>(maxPeriod).fill(0);\n    this.inputLength = maxPeriod;\n    this.inputLengthMin1 = maxPeriod - 1;\n  }\n\n  /** Indicates whether the indicator is primed. */\n  public isPrimed(): boolean { return this.primed; }\n\n  /** Describes the output data of the indicator. */\n  public metadata(): IndicatorMetadata {\n    return {\n      type: IndicatorType.TrendCycleMode,\n      mnemonic: this.mnemonicValue,\n      description: this.descriptionValue,\n      outputs: [\n        {\n          kind: TrendCycleModeOutput.Value,\n          type: OutputType.Scalar,\n          mnemonic: this.mnemonicValue,\n          description: this.descriptionValue,\n        },\n        {\n          kind: TrendCycleModeOutput.IsTrendMode,\n          type: OutputType.Scalar,\n          mnemonic: this.mnemonicTrend,\n          description: this.descriptionTrend,\n        },\n        {\n          kind: TrendCycleModeOutput.IsCycleMode,\n          type: OutputType.Scalar,\n          mnemonic: this.mnemonicCycle,\n          description: this.descriptionCycle,\n        },\n        {\n          kind: TrendCycleModeOutput.InstantaneousTrendLine,\n          type: OutputType.Scalar,\n          mnemonic: this.mnemonicITL,\n          description: this.descriptionITL,\n        },\n        {\n          kind: TrendCycleModeOutput.SineWave,\n          type: OutputType.Scalar,\n          mnemonic: this.mnemonicSine,\n          description: this.descriptionSine,\n        },\n        {\n          kind: TrendCycleModeOutput.SineWaveLead,\n          type: OutputType.Scalar,\n          mnemonic: this.mnemonicSineLead,\n          description: this.descriptionSineLead,\n        },\n        {\n          kind: TrendCycleModeOutput.DominantCyclePeriod,\n          type: OutputType.Scalar,\n          mnemonic: this.mnemonicDCP,\n          description: this.descriptionDCP,\n        },\n        {\n          kind: TrendCycleModeOutput.DominantCyclePhase,\n          type: OutputType.Scalar,\n          mnemonic: this.mnemonicDCPhase,\n          description: this.descriptionDCPhase,\n        },\n      ],\n    };\n  }\n\n  /** Updates the indicator given the next sample value. Returns the 8-tuple\n   * (value, isTrendMode, isCycleMode, trendline, sineWave, sineWaveLead, period, phase).\n   * isTrendMode / isCycleMode are encoded as 1 / 0 scalars. Returns all-NaN if not primed. */\n  public update(sample: number): [number, number, number, number, number, number, number, number] {\n    if (Number.isNaN(sample)) {\n      return [sample, sample, sample, sample, sample, sample, sample, sample];\n    }\n\n    const [, period, phase] = this.dc.update(sample);\n    const smoothedPrice = this.dc.smoothedPrice;\n\n    this.pushInput(sample);\n\n    if (this.primed) {\n      const smoothedPeriod = period;\n      const average = this.calculateTrendAverage(smoothedPeriod);\n      this.trendline = this.coeff0 * average\n        + this.coeff1 * this.trendAverage1\n        + this.coeff2 * this.trendAverage2\n        + this.coeff3 * this.trendAverage3;\n      this.trendAverage3 = this.trendAverage2;\n      this.trendAverage2 = this.trendAverage1;\n      this.trendAverage1 = average;\n\n      const diff = this.calculateSineLeadWaveDifference(phase);\n\n      // Condition 1: a cycle mode exists for the half-period of a dominant cycle after\n      // the SineWave vs SineWaveLead crossing.\n      this.isTrendMode = true;\n\n      if ((diff > 0 && this.prevSineLeadWaveDifference < 0)\n        || (diff < 0 && this.prevSineLeadWaveDifference > 0)) {\n        this.isTrendMode = false;\n        this.samplesInTrend = 0;\n      }\n\n      this.prevSineLeadWaveDifference = diff;\n      this.samplesInTrend++;\n\n      if (this.samplesInTrend < 0.5 * smoothedPeriod) {\n        this.isTrendMode = false;\n      }\n\n      // Condition 2: cycle mode if the measured phase rate of change is more than 2/3\n      // the phase rate of change of the dominant cycle (360/period) and less than 1.5×.\n      const phaseDelta = phase - this.prevDcPhase;\n      this.prevDcPhase = phase;\n\n      if (Math.abs(smoothedPeriod) > EPSILON) {\n        const dcRate = FULL_CYCLE / smoothedPeriod;\n        if (phaseDelta > (2.0 / 3.0) * dcRate && phaseDelta < 1.5 * dcRate) {\n          this.isTrendMode = false;\n        }\n      }\n\n      // Condition 3: if the WMA smoothed price is separated by more than the separation\n      // percentage from the instantaneous trend line, force the trend mode.\n      if (Math.abs(this.trendline) > EPSILON\n        && Math.abs((smoothedPrice - this.trendline) / this.trendline) >= this.separationFactor) {\n        this.isTrendMode = true;\n      }\n\n      return [\n        this.mode(), this.isTrendFloat(), this.isCycleFloat(),\n        this.trendline, this.sinWave, this.sinWaveLead, period, phase,\n      ];\n    }\n\n    if (this.dc.isPrimed()) {\n      this.primed = true;\n      const smoothedPeriod = period;\n      this.trendline = this.calculateTrendAverage(smoothedPeriod);\n      this.trendAverage1 = this.trendline;\n      this.trendAverage2 = this.trendline;\n      this.trendAverage3 = this.trendline;\n\n      this.prevDcPhase = phase;\n      this.prevSineLeadWaveDifference = this.calculateSineLeadWaveDifference(phase);\n\n      this.isTrendMode = true;\n      this.samplesInTrend++;\n\n      if (this.samplesInTrend < 0.5 * smoothedPeriod) {\n        this.isTrendMode = false;\n      }\n\n      return [\n        this.mode(), this.isTrendFloat(), this.isCycleFloat(),\n        this.trendline, this.sinWave, this.sinWaveLead, period, phase,\n      ];\n    }\n\n    const n = Number.NaN;\n    return [n, n, n, n, n, n, n, n];\n  }\n\n  /** Updates an indicator given the next scalar sample. */\n  public updateScalar(sample: Scalar): IndicatorOutput {\n    return this.updateEntity(sample.time, sample.value);\n  }\n\n  /** Updates an indicator given the next bar sample. */\n  public updateBar(sample: Bar): IndicatorOutput {\n    return this.updateEntity(sample.time, this.barComponentFunc(sample));\n  }\n\n  /** Updates an indicator given the next quote sample. */\n  public updateQuote(sample: Quote): IndicatorOutput {\n    return this.updateEntity(sample.time, this.quoteComponentFunc(sample));\n  }\n\n  /** Updates an indicator given the next trade sample. */\n  public updateTrade(sample: Trade): IndicatorOutput {\n    return this.updateEntity(sample.time, this.tradeComponentFunc(sample));\n  }\n\n  private updateEntity(time: Date, sample: number): IndicatorOutput {\n    const [value, trend, cycle, itl, sine, sineLead, period, phase] = this.update(sample);\n\n    const out: Scalar[] = [];\n    for (const v of [value, trend, cycle, itl, sine, sineLead, period, phase]) {\n      const s = new Scalar();\n      s.time = time;\n      s.value = v;\n      out.push(s);\n    }\n    return out;\n  }\n\n  private pushInput(value: number): void {\n    for (let i = this.inputLengthMin1; i > 0; i--) {\n      this.input[i] = this.input[i - 1];\n    }\n    this.input[0] = value;\n  }\n\n  private calculateTrendAverage(smoothedPeriod: number): number {\n    let length = Math.floor(smoothedPeriod * this.cyclePartMultiplier + 0.5);\n    if (length > this.inputLength) {\n      length = this.inputLength;\n    } else if (length < 1) {\n      length = 1;\n    }\n\n    let sum = 0;\n    for (let i = 0; i < length; i++) {\n      sum += this.input[i];\n    }\n    return sum / length;\n  }\n\n  private calculateSineLeadWaveDifference(phase: number): number {\n    const p = phase * DEG2RAD;\n    this.sinWave = Math.sin(p);\n    this.sinWaveLead = Math.sin(p + LEAD_OFFSET * DEG2RAD);\n    return this.sinWave - this.sinWaveLead;\n  }\n\n  private mode(): number { return this.isTrendMode ? 1.0 : -1.0; }\n  private isTrendFloat(): number { return this.isTrendMode ? 1.0 : 0.0; }\n  private isCycleFloat(): number { return this.isTrendMode ? 0.0 : 1.0; }\n}\n",
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.ts"
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

## Assistant (Compaction · claude-opus-4.7 · 67.7s)

## Goal

Port MBST's **TrendCycleMode (TCM)** indicator from `mbst-to-convert/john-ehlers/trend-cycle-mode/` into zpano:
- Go: `go/indicators/johnehlers/trendcyclemode/` ✅ complete and passing.
- TS: `ts/indicators/john-ehlers/trend-cycle-mode/` ⏳ in progress.

Wraps existing `DominantCycle` (for period + phase + smoothed price) and adds raw-sample buffer for trendline, sine/sineLead, and trend-vs-cycle mode logic.

## Instructions

### Outputs (order, 8 total)
1. **Value** (+1 trend / −1 cycle, Scalar)
2. **IsTrendMode** (0/1 Scalar)
3. **IsCycleMode** (0/1 Scalar = 1 − IsTrendMode)
4. **InstantaneousTrendLine** (Scalar)
5. **SineWave** (Scalar)
6. **SineWaveLead** (Scalar)
7. **DominantCyclePeriod** (Scalar)
8. **DominantCyclePhase** (Scalar)

### Combined single test file
6 MBST reference arrays (rawInput, dcPeriod, dcPhase, sine, sineLead, itl, tcm-value) merged into one test file, skip=9, settleSkip=177, tol 1e-4. For tcm value: skip indices 70, 71 (MBST known mismatches). tcm array is 201 entries — loop must guard `i >= limit`.

### MBST params / defaults
- `alphaEmaPeriodAdditional=0.33`, `trendLineSmoothingLength=4` ∈{2,3,4}, `cyclePartMultiplier=1.0` ∈(0,10], `separationPercentage=1.5` ∈(0,100].
- Default estimator HomodyneDiscriminator, smoothingLength=4, αq=αp=0.2, **warmUpPeriod=100**.
- Default BarComponent **BarMedianPrice** (always shown in mnemonic).
- Go `NewTrendCycleModeDefault` / `NewTrendCycleModeParams` → private `newTrendCycleMode`; TS `default()` / `fromParams()`.
- Implement `core.Indicator` directly; Go uses `sync.RWMutex`.
- Mnemonic: `tcm(α, tlsl, cpMul, sep%, <estimator>, <component>)` using `%.3f` for α/cpMul/sep and `%d` for tlsl. Default: `tcm(0.330, 4, 1.000, 1.500%, hl/2)`.

### Test commands
- Go: `cd /home/dev/zpano/go && go test ./indicators/johnehlers/trendcyclemode/...` ✅ passing.
- TS: from `/home/dev/zpano/ts`: `node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=TrendCycleMode`

## Discoveries

- MBST `dcp.RawValue` ≡ zpano `DominantCycle.Phase` when primed — no duplicate phase logic.
- MBST TCM pushes **raw samples** (not htce.Smoothed) to its own input buffer.
- **Phase comparison:** MBST wraps phase into (-180, 180]; zpano wraps into [0, 360). Tests must compare modulo 360: `d = (exp-act) mod 360; if d>180 d-=360 else if d<-180 d+=360; |d|<tol`.
- **Go error wrapping depth:** When DominantCycle created inside `newTrendCycleMode`, bar/quote/trade component errors get double-wrapped: `"invalid trend cycle mode parameters: invalid dominant cycle parameters: 9999: unknown bar component"`.
- Coefficient table by tlsl: 2→(2/3,1/3); 3→(3/6,2/6,1/6); 4→(4/10,3/10,2/10,1/10).
- TS `DominantCycle` default maxPeriod is **50**.
- **TS `barComponentValue` / etc. do NOT throw** on unknown components (silently default to close). So TS won't have invalid-component error paths — skip those test cases in TS spec (SineWave spec also omits them).
- TS enum convention: outputs start at 0 (SineWaveOutput pattern), unlike Go which starts at 1.
- HTITL is closest algorithmic template; SineWave closest structural template for wrapping DominantCycle.

## Accomplished

### Done
1. ✅ Go `SmoothedPrice()` / `MaxPeriod()` methods + tests on `DominantCycle`.
2. ✅ TS `smoothedPrice` / `maxPeriod` getters + specs on `DominantCycle`.
3. ✅ Registered `TrendCycleMode` in Go `core/type.go` + test, and TS `core/indicator-type.ts`.
4. ✅ **Go package complete and `go test` passes:**
   - `trendcyclemode.go`, `trendcyclemodeparams.go`, `trendcyclemodeoutput.go`, `trendcyclemodeoutput_test.go`, `trendcyclemode_test.go`.
   - Fixed final 2 failures: phase mod-360 comparison and errBC/errQC/errTC strings now include `invalid dominant cycle parameters:` prefix.
5. ✅ TS: `trend-cycle-mode-output.ts` (enum starts at 0).
6. ✅ TS: `trend-cycle-mode-params.ts`.
7. ✅ TS: `trend-cycle-mode.ts` — full implementation wrapping TS DominantCycle; uses `dc.smoothedPrice` / `dc.maxPeriod` getters; produces 8-element Scalar array from updateEntity.

### In progress / Todo
- ⏳ **TS `trend-cycle-mode.spec.ts`** — NOT YET CREATED. Needs to port data arrays from Go test file (`trendcyclemode_test.go` lines 23-244) and all test cases. Use SineWave spec as structural template but with 8-tuple outputs and mod-360 phase comparison. Omit "invalid bar/quote/trade component" tests (TS component funcs don't throw). Data arrays to port: testTCMInput (252), testTCMExpectedPeriod (252), testTCMExpectedPhase (252, NaN for first 12), testTCMExpectedSine (252, NaN for first 12), testTCMExpectedSineLead (252, NaN for first 12), testTCMExpectedITL (252, NaN for first 12), testTCMExpectedValue (201 total: 63 NaN + 138 ±1 entries).
- ⏳ Run TS tests and iterate.
- ⏳ Update `.opencode/skills/mbst-indicator-conversion/SKILL.md` with these patterns:
  - Expose wrapper internals via accessors (`SmoothedPrice()`, `MaxPeriod()`) rather than duplicating logic
  - Phase wrap-around-360 comparison trick
  - Go error-wrapping depth when wrapping sub-indicators
  - TS component funcs don't throw — skip those test paths

## Relevant files / directories

### Go — all complete
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/` (5 files, tests passing)
- `/home/dev/zpano/go/indicators/johnehlers/dominantcycle/dominantcycle.go` + `_test.go` — added SmoothedPrice/MaxPeriod
- `/home/dev/zpano/go/indicators/core/type.go` + `type_test.go` — added TrendCycleMode enum

### TS — in progress
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-output.ts` ✅
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode-params.ts` ✅
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.ts` ✅
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts` ⏳ **TO CREATE**
- `/home/dev/zpano/ts/indicators/john-ehlers/dominant-cycle/dominant-cycle.ts` + `.spec.ts` — smoothedPrice / maxPeriod getters (done)
- `/home/dev/zpano/ts/indicators/core/indicator-type.ts` — TrendCycleMode added (done)

### Reference templates
- `/home/dev/zpano/ts/indicators/john-ehlers/sine-wave/sine-wave.spec.ts` — structural template for spec
- `/home/dev/zpano/ts/indicators/john-ehlers/hilbert-transformer-instantaneous-trend-line/` — algorithmic reference
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go` — source of data arrays & test cases to port

### MBST source
- `/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleMode.cs` / `TrendCycleModeTest.cs`

### Skills
- `/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md` — needs post-completion update

### Immediate next action
1. Create `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts` by:
   - Copying data arrays from Go test file (lines 23-244) and translating to TS (replace `math.NaN()` with `Number.NaN`, drop Go nolint tags).
   - Mirroring sub-test structure of `TestTrendCycleModeUpdate` (reference period/phase/sine/sineLead/itl/value, is-trend+is-cycle complementary, NaN input), `TestTrendCycleModeIsPrimed`, `TestTrendCycleModeMetadata` (check 8 outputs and all mnemonics including `tcm(0.330, 4, 1.000, 1.500%, hl/2)` etc.), `TestTrendCycleModeUpdateEntity` (8-element scalar output), `TestNewTrendCycleMode` (throw tests for α, tlsl, cpm, sep — skip the three invalid-component tests).
   - Include mod-360 phase comparison helper.
   - Include `tcm(0.330, 4, 1.000, 1.500%, hl/2)` as default mnemonic.
2. Run `node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=TrendCycleMode` from `/home/dev/zpano/ts` and iterate until green.
3. Update the mbst-indicator-conversion skill with the new patterns.

---

## User

---

## Assistant (Build · claude-opus-4.7 · 7.4s)

_Thinking:_

 Now I'm moving on to set up the TypeScript spec file.

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

## Assistant (Build · claude-opus-4.7 · 5.9s)

_Thinking:_

 I'm examining the trend-cycle-mode.ts file to understand how its API works and what an example specification looks like.

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

## Assistant (Build · claude-opus-4.7 · 5.8s)

**Tool: read**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/sine-wave/sine-wave.spec.ts"
}
```

**Output:**
```
<path>/home/dev/zpano/ts/indicators/john-ehlers/sine-wave/sine-wave.spec.ts</path>
<type>file</type>
<content>1: import { } from 'jasmine';
2: 
3: import { Bar } from '../../../entities/bar';
4: import { BarComponent } from '../../../entities/bar-component';
5: import { Quote } from '../../../entities/quote';
6: import { Scalar } from '../../../entities/scalar';
7: import { Trade } from '../../../entities/trade';
8: import { IndicatorType } from '../../core/indicator-type';
9: import { Band } from '../../core/outputs/band';
10: import { OutputType } from '../../core/outputs/output-type';
11: import { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';
12: import { SineWave } from './sine-wave';
13: import { SineWaveOutput } from './sine-wave-output';
14: 
15: /* eslint-disable max-len */
16: // Input data taken from TA-Lib (http://ta-lib.org/) tests, test_MAMA.xsl, Price, D5…D256, 252 entries.
17: const input = [
18:   92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,
19:   94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,
20:   88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,
21:   85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,
22:   83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,
23:   89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,
24:   89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,
25:   88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,
26:   103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,
27:   120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,
28:   114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,
29:   114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,
30:   124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,
31:   137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,
32:   123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,
33:   122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,
34:   123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,
35:   130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,
36:   127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,
37:   121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,
38:   106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,
39:   95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,
40:   94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,
41:   103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,
42:   106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,
43:   109.5300, 108.0600,
44: ];
45: 
46: // Expected sine wave values taken from MBST SineWaveTest.cs. 252 entries.
47: const expectedSine = [
48:   Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN,
49:   Number.NaN, Number.NaN, Number.NaN,
50:   -0.9874388886763950, 0.9901389074597280, 0.7430789154373040, 0.9373629963767220, 0.9539229407379340, 0.7583605937417590, 0.9430523282622310, 0.9954894450161860, 0.2677593920226290, -0.1322524693787770,
51:   -0.0076634783681766, 0.0131173971015221, 0.1186956105503800, 0.1801829214581150, 0.2520928115991770, 0.4006612403521310, 0.5628300247542610, 0.7147094931388430, 0.8184710623910560, 0.8857771050315170,
52:   0.8580051054352940, 0.9044393510772560, 0.9477105512269880, 0.9791019835938800, 0.9961661502181070, 0.9822460058918840, 0.8465682109433450, 0.4079557291761370, -0.4004013665430420, -0.8421036878603060,
53:   -0.9017223344864390, -0.9865337683617420, -0.9643853118844240, -0.9598599815079740, 0.8037267766705520, 0.9460240546952540, 0.8605016852751490, 0.7070533756573690, 0.5243161160505560, 0.2294102723810260,
54:   -0.2198489640488240, -0.7336434870569610, -0.9451240721288890, -0.9987083965673280, -0.8075209150605320, -0.5085066436721070, -0.1270264128351010, 0.2721854747475370, 0.6055917784886910, 0.8413676398899490,
55:   0.9777625488346960, 0.9839896992562860, 0.8436921159626520, 0.5969447127483850, 0.3094984574050860, 0.0167240212732023, -0.2467178763145330, -0.4560076441356610, -0.6309543271436050, -0.7546432714599400,
56:   -0.9163549058535820, -0.9842983646399860, -0.9864836587318940, -0.8626035608908470, -0.6813902164376480, 0.1858483908516400, 0.9266383325265240, 0.9950614294774160, 0.8974629492620320, 0.7466772329212920,
57:   0.5703644678660200, 0.3750953336668390, 0.1580160207328650, -0.1758076016904010, -0.3745982816248290, -0.5276875478007350, -0.6362049492364280, -0.6890552092033720, -0.6115639640491650, -0.4788688150780810,
58:   -0.1254943141670760, -0.0616279223840421, 0.1084398125157010, 0.0507107190849194, 0.0459258492372133, -0.0603960127005548, -0.1802614924906120, -0.3445196936393600, -0.5039846767879240, -0.6418346779779860,
59:   -0.7682209569752370, -0.8811490866996920, -0.9663490613441720, -0.9999985470701830, -0.9546858649139780, -0.3854582462377580, 0.3634207386677970, 0.8289947319983170, 0.9714909193370010, 0.9983857647297360,
60:   0.9298786498974830, 0.7522877436715410, 0.5261656913352240, 0.3198410350076260, 0.2206250837743810, 0.2324067219830480, 0.2622555352188150, 0.2488016117878720, 0.2127369252136960, 0.1536174562004720,
61:   0.0365743929645966, -0.0132935129731492, -0.0205972714630062, -0.0506416644795282, -0.0877504363551195, -0.1297549156821220, -0.0930172242876989, -0.0576191763867261, -0.0229433518557876, 0.0363188766621527,
62:   0.0001870031678450, -0.0349743794742610, -0.0631993922084072, -0.0971138050129020, -0.1624979390656570, -0.2458522401310880, -0.3438638176192820, -0.4480085728769030, -0.5679531815591490, -0.6842705623709480,
63:   -0.7615364178876680, -0.8520399314791350, -0.9010432983130380, -0.9834194615045590, -0.9905476870988080, -0.8872786828920670, -0.6744804687446460, -0.3755063881128740, -0.1868453942460660, 0.0794027920237213,
64:   0.2259213674108930, 0.4304722733878400, 0.5298904205645140, 0.6239251933219030, 0.7158012315374360, 0.8037206344012380, 0.8731666378756390, 0.9219544489912210, 0.9658401621307640, 0.9953707336137810,
65:   0.9916336070524670, 0.9786230879164460, 0.7515981300962940, 0.5233956277754490, 0.1867099275394900, -0.1183826185655860, -0.6727134585677300, -0.7289894296051810, 0.5281384062949680, 0.8678795703185170,
66:   0.8298749053132090, 0.6902403611717530, 0.5240926478571590, 0.3265450140990430, 0.1410201458363470, -0.0385849224232995, -0.1727584531294190, -0.3840270653389320, -0.5839449895165980, -0.7055596595779030,
67:   -0.8654833335157070, -0.9746729156329320, -0.9963449765889340, -0.9217474051403660, -0.7639362508931000, -0.5606026149454520, -0.3510588357296890, -0.3005270942524180, -0.1156452933894830, -0.0595959289499457,
68:   -0.0194952452596040, -0.0107439009455394, -0.0101826864310280, -0.0169325310282176, -0.1277205927832380, -0.2371674472604240, -0.2446819018845580, -0.1710822516700410, -0.0511353061733825, 0.0062563192154928,
69:   -0.0048516364950122, -0.0717158401086350, -0.1252962791394740, -0.1414075684951130, -0.1259001773043880, -0.0975843207989124, -0.0694511521424164, -0.0337586943638060, -0.0159474649423128, 0.0194325975213332,
70:   0.0608001695138442, 0.1024001693533800, 0.1480876049487220, 0.2039912059729780, 0.2920915426229240, 0.3926785587852390, 0.5354672113127650, 0.6169657298756830, 0.6993887726683350, 0.7501832875129180,
71:   0.8386871644310250, 0.9103651277237890, 0.9764351312310820, 0.9926345853646890, 0.9300339420371430, 0.8258742381128520, 0.7499387624704700, 0.6725402422076670, 0.6229867037490120, 0.6045297568333980,
72:   0.5039297182908610, 0.4455885766999630, 0.3779726796046290, 0.3429218283962470, 0.2929489553699220, 0.1570518914741900, 0.0572157944020108, -0.0463361936582273, -0.1573451851308570, -0.2931672021738570,
73:   -0.4528725984930980, -0.6063286567677410, -0.6995137850617610, -0.7902112777605310, -0.9139878106324440, -0.9998170324167670, -0.9448389776183910, -0.5177630386017770, 0.3950506584043580, 0.7346367050507850,
74: ];
75: 
76: // Expected sine wave lead values taken from MBST SineWaveTest.cs. 252 entries.
77: const expectedSineLead = [
78:   Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN,
79:   Number.NaN, Number.NaN, Number.NaN,
80:   -0.5865008756606930, 0.6010760134973850, 0.0522375953518313, 0.9091393120803100, 0.8866939724449600, 0.9971590529781770, 0.9020538171846020, 0.6368324474361340, 0.8706218323553310, 0.6073789821251400,
81:   0.7016671195580980, 0.7163213444559970, 0.7860384941578910, 0.8229422149880850, 0.8625259332042720, 0.9311800857903170, 0.9824560606058480, 0.9999415670942400, 0.9850130504072370, 0.9545149566415440,
82:   0.9699003741647460, 0.9411885260170180, 0.8957923799982550, 0.8361338691916270, 0.6425371253442400, 0.5619014189746250, 0.2222351486408030, -0.3571214380720770, -0.9310766529205810, -0.9768109870729720,
83:   -0.9433073135761630, -0.8132374819975520, -0.8689541000414960, -0.8770525415166500, 0.1475931480924170, 0.4397692495572380, 0.2482327294712000, -0.0000755239717352, -0.2313707009630830, -0.5260305525740280,
84:   -0.8452633448517760, -0.9992677882479890, -0.8993236485622310, -0.6702662015228300, -0.1539244027456790, 0.2492915015754510, 0.6115575080093740, 0.8728740226620090, 0.9909162839717420, 0.9771019730335230,
85:   0.8396734557306670, 0.5697614425887530, 0.2169861504951610, -0.1451956537134670, -0.4535393411417920, -0.6951822190514210, -0.8597041192450010, -0.9517540619903210, -0.9947396502168260, -0.9975710951353530,
86:   -0.9310629460274820, -0.8208173676831810, -0.5816830016568750, -0.2522412838591280, 0.0357301876330212, 0.8262025299696030, 0.9210718625890570, 0.6334265388777150, 0.3227045140978610, 0.0576226338781002,
87:   -0.1775030791814070, -0.3902458029200550, -0.5864888795958400, -0.8204080195335590, -0.9205013506909260, -0.9737752029989330, -0.9954119273819820, -0.9996821539441930, -0.9919003713144100, -0.9593708639942700,
88:   -0.7902545151018210, -0.7493402295361840, -0.6262584572592140, -0.6703391127989300, -0.6738861991115050, -0.7485223840691900, -0.8229875936131110, -0.9074292725287760, -0.9671080150009190, -0.9960855660349680,
89:   -0.9958932520932240, -0.9574138340514830, -0.8652041011461150, -0.7083111287566420, -0.4646190135134790, 0.3799051621364850, 0.9157357095569460, 0.9816417978833290, 0.8545864068220020, 0.6658040149144910,
90:   0.3974026651162090, 0.0660812869143209, -0.2292556000437840, -0.4438015597350790, -0.5336772503175370, -0.5234089018965820, -0.4969142899048420, -0.5089421255844810, -0.5404930402692640, -0.5900897578798300,
91:   -0.6807716781362360, -0.7164442325283580, -0.7215212414254330, -0.7420085481569570, -0.7664280384961050, -0.7928795495979390, -0.7698142295937170, -0.7466749292126270, -0.7231440467849940, -0.6809589449044260,
92:   -0.7069745376146330, -0.7314048006899320, -0.7503819371382670, -0.7724343179777090, -0.8126119368714450, -0.8592475635206920, -0.9071355135227490, -0.9489640525891650, -0.9835954633562350, -0.9994945745179860,
93:   -0.9967791122784410, -0.9726372051055730, -0.9438266487324060, -0.8236131488885440, -0.6034299982731380, -0.3012592866284200, 0.0451220549347129, 0.3898374562298360, 0.5625345127655470, 0.7610204246544270,
94:   0.8485754490678880, 0.9426271321408510, 0.9743624133056830, 0.9937744211043730, 0.9999234595478280, 0.9890496834828740, 0.9620813190845490, 0.9257815160887320, 0.8661907447689560, 0.7717933199927350,
95:   0.6099143741297350, 0.5465660379422910, 0.0650374448708587, -0.2324218737852860, -0.5626485140051470, -0.7858435966184150, -0.9988707502989160, -0.9995056053709990, -0.2269953459568470, 0.2624108067271960,
96:   0.1922802999868170, -0.0235747958138623, -0.2316259847404300, -0.4374421611852380, -0.6003241683068820, -0.7338638764656210, -0.8186335536900920, -0.9244352736103260, -0.9869359601928300, -0.9999976116346720,
97:   -0.9662055929589490, -0.8473318021865150, -0.6441207677355740, -0.3775643212631200, -0.0838943519776584, 0.1891385830833930, 0.4138657569596380, 0.4619148062355390, 0.6205889300081480, 0.6637092720958960,
98:   0.6931871751689060, 0.6994688836282570, 0.6998698746833480, 0.6950322988997850, 0.6110036127434860, 0.5192294559798360, 0.5125969121038930, 0.5757083087460280, 0.6700235759926480, 0.7115168281729820,
99:   0.7036678339992160, 0.6545752979714670, 0.6129364849611150, 0.6000111421090120, 0.6124554033545860, 0.6347294098505560, 0.6562899876128100, 0.6828327376881520, 0.6957402985880160, 0.7207141791649000,
100:   0.7487908163625680, 0.7757975773123150, 0.8040241461526770, 0.8364818144534760, 0.8828100232401670, 0.9279746557928580, 0.9758237828336110, 0.9927466296862280, 0.9999410720652520, 0.9980198650776760,
101:   0.9781412645524530, 0.9363303228513180, 0.8430454008691210, 0.6162348270656310, 0.3977902134753690, 0.1852760304055380, 0.0625305163973752, -0.0477440698701413, -0.1126037294757250, -0.1358018386690990,
102:   -0.2544275858052280, -0.3179500551407050, -0.3873842567862560, -0.4217482646499100, -0.4689384577141290, -0.5872793774815170, -0.6654907471409910, -0.7391119157698760, -0.8095586705039820, -0.8833378646001520,
103:   -0.9506681839279980, -0.9910404297342630, -0.9999429557041830, -0.9921031907559610, -0.9331896469980470, -0.6934514712647360, -0.4364996881408350, 0.2388331163456430, 0.9289332462002390, 0.9992107524543470,
104: ];
105: 
106: const tolerance = 1e-4;
107: 
108: describe('SineWave', () => {
109:   const time = new Date(2021, 3, 1);
110:   const skip = 9;
111:   const settleSkip = 177;
112: 
113:   it('should have correct output enum values', () => {
114:     expect(SineWaveOutput.Value).toBe(0);
115:     expect(SineWaveOutput.Lead).toBe(1);
116:     expect(SineWaveOutput.Band).toBe(2);
117:     expect(SineWaveOutput.DominantCyclePeriod).toBe(3);
118:     expect(SineWaveOutput.DominantCyclePhase).toBe(4);
119:   });
120: 
121:   it('should return expected mnemonic for default params (BarComponent.Median)', () => {
122:     const sw = SineWave.default();
123:     expect(sw.metadata().mnemonic).toBe('sw(0.330, hl/2)');
124:   });
125: 
126:   it('should return expected mnemonic for explicit components matching Close default', () => {
127:     const sw = SineWave.fromParams({ alphaEmaPeriodAdditional: 0.5, barComponent: BarComponent.Close });
128:     expect(sw.metadata().mnemonic).toBe('sw(0.500)');
129:   });
130: 
131:   it('should return expected mnemonic for phase accumulator', () => {
132:     const sw = SineWave.fromParams({
133:       alphaEmaPeriodAdditional: 0.5,
134:       estimatorType: HilbertTransformerCycleEstimatorType.PhaseAccumulator,
135:       estimatorParams: { smoothingLength: 4, alphaEmaQuadratureInPhase: 0.2, alphaEmaPeriod: 0.2 },
136:       barComponent: BarComponent.Close,
137:     });
138:     expect(sw.metadata().mnemonic).toBe('sw(0.500, pa(4, 0.200, 0.200))');
139:   });
140: 
141:   it('should return expected metadata', () => {
142:     const sw = SineWave.default();
143:     const meta = sw.metadata();
144:     const mn = 'sw(0.330, hl/2)';
145:     const mnLead = 'sw-lead(0.330, hl/2)';
146:     const mnBand = 'sw-band(0.330, hl/2)';
147:     const mnDCP = 'dcp(0.330, hl/2)';
148:     const mnDCPha = 'dcph(0.330, hl/2)';
149: 
150:     expect(meta.type).toBe(IndicatorType.SineWave);
151:     expect(meta.mnemonic).toBe(mn);
152:     expect(meta.description).toBe('Sine wave ' + mn);
153:     expect(meta.outputs.length).toBe(5);
154: 
155:     expect(meta.outputs[0].kind).toBe(SineWaveOutput.Value);
156:     expect(meta.outputs[0].type).toBe(OutputType.Scalar);
157:     expect(meta.outputs[0].mnemonic).toBe(mn);
158:     expect(meta.outputs[0].description).toBe('Sine wave ' + mn);
159: 
160:     expect(meta.outputs[1].kind).toBe(SineWaveOutput.Lead);
161:     expect(meta.outputs[1].type).toBe(OutputType.Scalar);
162:     expect(meta.outputs[1].mnemonic).toBe(mnLead);
163:     expect(meta.outputs[1].description).toBe('Sine wave lead ' + mnLead);
164: 
165:     expect(meta.outputs[2].kind).toBe(SineWaveOutput.Band);
166:     expect(meta.outputs[2].type).toBe(OutputType.Band);
167:     expect(meta.outputs[2].mnemonic).toBe(mnBand);
168:     expect(meta.outputs[2].description).toBe('Sine wave band ' + mnBand);
169: 
170:     expect(meta.outputs[3].kind).toBe(SineWaveOutput.DominantCyclePeriod);
171:     expect(meta.outputs[3].type).toBe(OutputType.Scalar);
172:     expect(meta.outputs[3].mnemonic).toBe(mnDCP);
173:     expect(meta.outputs[3].description).toBe('Dominant cycle period ' + mnDCP);
174: 
175:     expect(meta.outputs[4].kind).toBe(SineWaveOutput.DominantCyclePhase);
176:     expect(meta.outputs[4].type).toBe(OutputType.Scalar);
177:     expect(meta.outputs[4].mnemonic).toBe(mnDCPha);
178:     expect(meta.outputs[4].description).toBe('Dominant cycle phase ' + mnDCPha);
179:   });
180: 
181:   it('should throw if α is out of range (0, 1]', () => {
182:     expect(() => SineWave.fromParams({ alphaEmaPeriodAdditional: 0 })).toThrow();
183:     expect(() => SineWave.fromParams({ alphaEmaPeriodAdditional: -0.1 })).toThrow();
184:     expect(() => SineWave.fromParams({ alphaEmaPeriodAdditional: 1.0001 })).toThrow();
185:     expect(() => SineWave.fromParams({ alphaEmaPeriodAdditional: 0.5 })).not.toThrow();
186:     expect(() => SineWave.fromParams({ alphaEmaPeriodAdditional: 1.0 })).not.toThrow();
187:   });
188: 
189:   it('should return NaN quadruple for NaN input', () => {
190:     const sw = SineWave.default();
191:     const [v, l, p, h] = sw.update(Number.NaN);
192:     expect(Number.isNaN(v)).toBe(true);
193:     expect(Number.isNaN(l)).toBe(true);
194:     expect(Number.isNaN(p)).toBe(true);
195:     expect(Number.isNaN(h)).toBe(true);
196:   });
197: 
198:   it('should match reference sine (MBST SineWaveTest) past settle window', () => {
199:     const sw = SineWave.default();
200:     for (let i = skip; i < input.length; i++) {
201:       const [value] = sw.update(input[i]);
202:       if (Number.isNaN(value) || i < settleSkip) continue;
203:       if (Number.isNaN(expectedSine[i])) continue;
204:       expect(Math.abs(expectedSine[i] - value))
205:         .withContext(`sine[${i}]: expected ${expectedSine[i]}, actual ${value}`)
206:         .toBeLessThan(tolerance);
207:     }
208:   });
209: 
210:   it('should match reference sine lead (MBST SineWaveLeadTest) past settle window', () => {
211:     const sw = SineWave.default();
212:     for (let i = skip; i < input.length; i++) {
213:       const [, lead] = sw.update(input[i]);
214:       if (Number.isNaN(lead) || i < settleSkip) continue;
215:       if (Number.isNaN(expectedSineLead[i])) continue;
216:       expect(Math.abs(expectedSineLead[i] - lead))
217:         .withContext(`sineLead[${i}]: expected ${expectedSineLead[i]}, actual ${lead}`)
218:         .toBeLessThan(tolerance);
219:     }
220:   });
221: 
222:   it('should become primed within the input sequence', () => {
223:     const sw = SineWave.default();
224:     expect(sw.isPrimed()).toBe(false);
225: 
226:     let primedAt = -1;
227:     for (let i = 0; i < input.length; i++) {
228:       sw.update(input[i]);
229:       if (sw.isPrimed() && primedAt < 0) {
230:         primedAt = i;
231:       }
232:     }
233:     expect(primedAt).toBeGreaterThanOrEqual(0);
234:     expect(sw.isPrimed()).toBe(true);
235:   });
236: 
237:   const primeCount = 200;
238:   const value = 100.0;
239: 
240:   function checkOutput(out: any[]): void {
241:     expect(out.length).toBe(5);
242:     // indices 0, 1, 3, 4 are scalars; 2 is a band.
243:     for (const i of [0, 1, 3, 4]) {
244:       const s = out[i] as Scalar;
245:       expect(s.time).toEqual(time);
246:     }
247:     const b = out[2] as Band;
248:     expect(b.time).toEqual(time);
249:   }
250: 
251:   it('should produce 5-element output via updateScalar', () => {
252:     const sw = SineWave.default();
253:     for (let i = 0; i < primeCount; i++) sw.update(input[i % input.length]);
254:     const s = new Scalar();
255:     s.time = time;
256:     s.value = value;
257:     checkOutput(sw.updateScalar(s) as any[]);
258:   });
259: 
260:   it('should produce 5-element output via updateBar', () => {
261:     const sw = SineWave.default();
262:     for (let i = 0; i < primeCount; i++) sw.update(input[i % input.length]);
263:     const bar = new Bar({ time, open: value, high: value, low: value, close: value, volume: 0 });
264:     checkOutput(sw.updateBar(bar) as any[]);
265:   });
266: 
267:   it('should produce 5-element output via updateQuote', () => {
268:     const sw = SineWave.default();
269:     for (let i = 0; i < primeCount; i++) sw.update(input[i % input.length]);
270:     const q = new Quote({ time, bid: value, ask: value, bidSize: 0, askSize: 0 });
271:     checkOutput(sw.updateQuote(q) as any[]);
272:   });
273: 
274:   it('should produce 5-element output via updateTrade', () => {
275:     const sw = SineWave.default();
276:     for (let i = 0; i < primeCount; i++) sw.update(input[i % input.length]);
277:     const t = new Trade({ time, price: value, volume: 0 });
278:     checkOutput(sw.updateTrade(t) as any[]);
279:   });
280: 
281:   it('should order band as {upper: value, lower: lead}', () => {
282:     const sw = SineWave.default();
283:     for (let i = 0; i < primeCount; i++) sw.update(input[i % input.length]);
284:     const s = new Scalar();
285:     s.time = time;
286:     s.value = input[0];
287:     const out = sw.updateScalar(s) as any[];
288:     const sv = out[SineWaveOutput.Value] as Scalar;
289:     const sl = out[SineWaveOutput.Lead] as Scalar;
290:     const band = out[SineWaveOutput.Band] as Band;
291:     expect(band.upper).toBe(sv.value);
292:     expect(band.lower).toBe(sl.value);
293:   });
294: });

(End of file - total 294 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 5.7s)

_Thinking:_

 I need to examine the TrendCycleModeOutput enum and its parameters, then reference an existing spec to understand the output structure before composing the spec file.

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

## Assistant (Build · claude-opus-4.7 · 9.5s)

_Thinking:_

 I'm laying out the test specification now—pulling the data arrays from the Go test file and setting up validation checks for phase wrapping at 360 degrees, enum values, metadata outputs, alpha constraints, and ensuring NaN inputs propagate correctly through all eight return values.

Writing test spec...

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

## Assistant (Build · claude-opus-4.7 · 403.4s)

**Tool: write**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/trend-cycle-mode.spec.ts",
  "content": "import { } from 'jasmine';\n\nimport { Bar } from '../../../entities/bar';\nimport { Quote } from '../../../entities/quote';\nimport { Scalar } from '../../../entities/scalar';\nimport { Trade } from '../../../entities/trade';\nimport { IndicatorType } from '../../core/indicator-type';\nimport { OutputType } from '../../core/outputs/output-type';\nimport { HilbertTransformerCycleEstimatorType } from '../hilbert-transformer/hilbert-transformer-cycle-estimator-type';\nimport { TrendCycleMode } from './trend-cycle-mode';\nimport { TrendCycleModeOutput } from './trend-cycle-mode-output';\n\n/* eslint-disable max-len */\n// Input from MBST TrendCycleModeTest.cs (TA-Lib test_MAMA.xsl, Price). 252 entries.\nconst input = [\n  92.0000, 93.1725, 95.3125, 94.8450, 94.4075, 94.1100, 93.5000, 91.7350, 90.9550, 91.6875,\n  94.5000, 97.9700, 97.5775, 90.7825, 89.0325, 92.0950, 91.1550, 89.7175, 90.6100, 91.0000,\n  88.9225, 87.5150, 86.4375, 83.8900, 83.0025, 82.8125, 82.8450, 86.7350, 86.8600, 87.5475,\n  85.7800, 86.1725, 86.4375, 87.2500, 88.9375, 88.2050, 85.8125, 84.5950, 83.6575, 84.4550,\n  83.5000, 86.7825, 88.1725, 89.2650, 90.8600, 90.7825, 91.8600, 90.3600, 89.8600, 90.9225,\n  89.5000, 87.6725, 86.5000, 84.2825, 82.9075, 84.2500, 85.6875, 86.6100, 88.2825, 89.5325,\n  89.5000, 88.0950, 90.6250, 92.2350, 91.6725, 92.5925, 93.0150, 91.1725, 90.9850, 90.3775,\n  88.2500, 86.9075, 84.0925, 83.1875, 84.2525, 97.8600, 99.8750, 103.2650, 105.9375, 103.5000,\n  103.1100, 103.6100, 104.6400, 106.8150, 104.9525, 105.5000, 107.1400, 109.7350, 109.8450, 110.9850,\n  120.0000, 119.8750, 117.9075, 119.4075, 117.9525, 117.2200, 115.6425, 113.1100, 111.7500, 114.5175,\n  114.7450, 115.4700, 112.5300, 112.0300, 113.4350, 114.2200, 119.5950, 117.9650, 118.7150, 115.0300,\n  114.5300, 115.0000, 116.5300, 120.1850, 120.5000, 120.5950, 124.1850, 125.3750, 122.9700, 123.0000,\n  124.4350, 123.4400, 124.0300, 128.1850, 129.6550, 130.8750, 132.3450, 132.0650, 133.8150, 135.6600,\n  137.0350, 137.4700, 137.3450, 136.3150, 136.4400, 136.2850, 129.0950, 128.3100, 126.0000, 124.0300,\n  123.9350, 125.0300, 127.2500, 125.6200, 125.5300, 123.9050, 120.6550, 119.9650, 120.7800, 124.0000,\n  122.7800, 120.7200, 121.7800, 122.4050, 123.2500, 126.1850, 127.5600, 126.5650, 123.0600, 122.7150,\n  123.5900, 122.3100, 122.4650, 123.9650, 123.9700, 124.1550, 124.4350, 127.0000, 125.5000, 128.8750,\n  130.5350, 132.3150, 134.0650, 136.0350, 133.7800, 132.7500, 133.4700, 130.9700, 127.5950, 128.4400,\n  127.9400, 125.8100, 124.6250, 122.7200, 124.0900, 123.2200, 121.4050, 120.9350, 118.2800, 118.3750,\n  121.1550, 120.9050, 117.1250, 113.0600, 114.9050, 112.4350, 107.9350, 105.9700, 106.3700, 106.8450,\n  106.9700, 110.0300, 91.0000, 93.5600, 93.6200, 95.3100, 94.1850, 94.7800, 97.6250, 97.5900,\n  95.2500, 94.7200, 92.2200, 91.5650, 92.2200, 93.8100, 95.5900, 96.1850, 94.6250, 95.1200,\n  94.0000, 93.7450, 95.9050, 101.7450, 106.4400, 107.9350, 103.4050, 105.0600, 104.1550, 103.3100,\n  103.3450, 104.8400, 110.4050, 114.5000, 117.3150, 118.2500, 117.1850, 109.7500, 109.6550, 108.5300,\n  106.2200, 107.7200, 109.8400, 109.0950, 109.0900, 109.1550, 109.3150, 109.0600, 109.9050, 109.6250,\n  109.5300, 108.0600,\n];\n\nconst expectedPeriod = [\n  0, 0, 0, 0, 0, 0, 0, 0, 0, 0,\n  0, 0, 0.39600000000000, 0.97812000000000, 1.62158040000000, 2.25545086800000, 2.84234568156000, 3.36868456664520, 3.86776291565229, 4.36321983508703,\n  4.87235783926831, 5.40838035704577, 5.98190550443027, 6.60199641969884, 7.27686930610184, 8.01438731048222, 8.82241286095647, 9.70906731606755, 10.68293087091460, 11.75320502957710,\n  12.92985285048740, 14.22372743856440, 15.55272286275450, 16.77503611571540, 17.85814025111630, 18.72970387649220, 19.30387978646140, 19.63314544969620, 19.86256979430250, 20.09030968609300,\n  20.24817834009410, 20.31132870798190, 20.52152604110820, 21.27119054536480, 22.10966835167300, 22.28715460952700, 21.91280773257140, 21.23923470724180, 20.70813161651310, 20.20449150221090,\n  19.52863321263000, 18.73709250583170, 17.96311275281150, 17.33367762545960, 16.91743352044750, 16.64300564862120, 16.41952162419500, 16.27464914327850, 16.26425245094380, 16.33321577028600,\n  16.39265551523990, 16.39976990202790, 16.38221107536320, 16.37405059271910, 16.35102942468120, 16.26839438425590, 16.12432207371240, 15.99529098667630, 15.96458956780100, 16.07977539207760,\n  16.38360881255670, 16.79746341307210, 17.18753188776420, 17.58524022168910, 18.05888760471740, 18.46077773999830, 18.78691120238400, 19.07789869381110, 19.11803073417110, 18.72675385299730,\n  18.07403190737810, 17.72999456892580, 18.00699920187680, 18.06680349806270, 17.73551482016550, 17.28467833183610, 16.97456900115070, 16.89386283663200, 17.00556420464730, 17.21986959021550,\n  17.48251598471900, 17.79647268844360, 18.15809655229470, 18.56044162987590, 19.03705300462320, 19.61779465906120, 20.13838368155990, 20.58144279802850, 20.93554178712450, 21.09578565733870,\n  21.19426268582890, 21.35270550953270, 21.46806615214910, 21.43420235778580, 21.27320458618770, 20.98617884905010, 20.62345107800030, 20.32165030848570, 20.09921951571820, 19.88214300840560,\n  19.67081622699810, 19.55217428481160, 19.60485773311710, 19.77836095343260, 19.77886122563300, 19.59009982815140, 19.54609435364200, 19.77945658439880, 20.20526697824140, 20.80572859375930,\n  21.45882440191380, 21.50916115262280, 21.07219135457730, 20.33979206665380, 19.60807769029340, 19.15831017112920, 19.03006205140340, 19.23359250887840, 19.84206353515510, 20.83692898803630,\n  22.25776348341490, 23.50933063567320, 24.02857349775940, 24.28548086650010, 24.74576845262060, 25.45685387492870, 26.31998583396390, 27.14553013410700, 27.80677101851790, 28.50146147525040,\n  29.16835938704370, 29.38723525724370, 29.55886298198770, 30.43981360336700, 30.70779370313880, 30.20667454311960, 29.23518282361370, 28.00037502954910, 26.95505291681000, 26.22399862702800,\n  25.67716809996900, 25.19893752937060, 24.76924271120940, 24.40654607774420, 24.12997738279210, 24.02648590415090, 24.17912316847620, 24.26552607123530, 24.07565548132200, 23.81050493977940,\n  23.75771490624360, 24.01627030476950, 24.42884190933990, 24.55867905189440, 24.41978729840000, 24.33536819272640, 24.11887925396970, 23.53741527509780, 22.66734716257270, 21.70419061052260,\n  20.78848949032480, 19.92593130809770, 19.09528115584620, 18.35405205698280, 17.81539769318840, 17.53491540732180, 17.59552736216070, 18.09376127214910, 18.69300796204700, 19.10361709066390,\n  19.40368660687600, 19.79324964337850, 20.14261316711870, 20.30292592814370, 20.37642955508450, 20.42856373321320, 20.36417897031590, 20.26870923265670, 20.31691792510900, 20.52593924664440,\n  20.95797078839970, 21.70315565998060, 22.68588957914270, 23.95566588814480, 25.30036991408680, 26.49222048853470, 28.24485763802440, 30.46863151925310, 31.19661794415910, 30.97271495031300,\n  30.15801520320610, 29.52193986806340, 28.48090879451130, 27.20913817575940, 25.84740758865390, 24.75875079095690, 24.57820671512040, 25.25622655282780, 26.58938264946150, 28.44936832011270,\n  30.75900691394640, 31.63120735338530, 31.95156902113430, 32.19329221743080, 32.18129930292270, 31.78927079951340, 30.94427836437330, 29.74153261553520, 28.44319750131350, 27.27756983469050,\n  26.30928991862760, 25.59706087830910, 25.19354035279110, 24.98183319418390, 24.66611779383150, 24.13629363553260, 23.59372342374540, 23.45943359521940, 24.13462330023790, 25.42868068174450,\n  27.22154743441240, 28.85990121754770, 29.25658159944000, 28.86760790158470, 28.27077502042400, 27.83957963686970, 27.56292753489200, 27.31665028261770, 27.11537844471070, 27.05619511102920,\n  26.72669604084850, 25.93839467294110, 24.88015320695530, 23.98089561843900, 23.51115215671300, 23.02173482203020, 22.29674643126940, 21.42162141795630, 20.54863761751100, 19.78167187971360,\n  19.14387361712880, 18.61396641752300,\n];\n\nconst expectedPhase = [\n  Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN,\n  Number.NaN, Number.NaN, Number.NaN,\n  639.09090909090900, 98.05299963194700, 132.00564338345400, 69.61331949528320, 72.53929158292410, 49.31988383698000, 70.57064052204580, 95.44397344939550, 15.53098102018940, -7.59977335936253,\n  -0.43908926482948, 0.75159304711349, 6.81682854281200, 10.38041464097730, 14.60138862232650, 23.61952231391600, 34.25173964222910, 45.61939612889610, 54.93203281582900, 62.34730431858280,\n  59.09332873285360, 64.74787861710400, 71.38960942346380, 78.26591034671290, 95.01873223899610, 100.81260420916600, 122.15965196914300, 155.92351856084900, 203.60327223472200, 237.36293513970900,\n  244.38539212971100, 260.58654095568100, 254.66267427143100, 253.71116813655900, 126.51252837026600, 108.91084071926300, 120.62704116849900, 135.00432720483700, 148.37778407551400, 166.73764548652900,\n  192.70016208445300, 227.19271648131900, 250.93069421530800, 272.91238631233800, 306.14557952390500, -30.56440912946640, -7.29779366562593, 15.79435681229920, 37.27143784018100, 57.28482204365770,\n  77.89434659794900, 100.26640790428300, 122.46792347519500, 143.34860922315100, 161.97099218663000, 179.04173949124300, 194.28337763382200, 207.12978658551400, 219.12056620855700, 228.99420937560000,\n  246.39888033834300, 259.83330071373300, 279.43099672242300, 300.38982045382800, -42.95237520761510, 10.71059944380160, 67.91671263897760, 95.69661875079390, 116.17343846855600, 131.69663648791300,\n  145.22435488266800, 157.96979483912800, 170.90824231177600, 190.12565895164300, 201.99948636973700, 211.84934448916700, 219.50941069822200, 223.55536694633000, 217.70267385839600, 208.61154857738600,\n  187.20930268786500, 183.53325881754400, 173.77461459542900, 177.09324308654900, 177.36771678646600, 183.46254384786600, 190.38499136923400, 200.15247884572800, 210.26397611554500, 219.92876325408700,\n  230.19440003342700, 241.78128922383700, 255.09398089013500, 269.90233026686000, 287.31443195349700, -22.67219169276530, 21.31042453594620, 55.99561073880190, 76.28592721136950, 93.25596317796150,\n  111.58409337397600, 131.21106018529500, 148.25324978238800, 161.34668836868300, 167.25425018604100, 166.56119294454900, 164.79606018404900, 165.59339091940400, 167.71720854219100, 171.16337795073400,\n  177.90397416349400, 180.76168462321900, 181.18022018527700, 182.90279528585400, 185.03420448405800, 187.45543006582800, 185.33720976352800, 183.30316508274000, 181.31467258646500, 177.91862390253700,\n  179.98928550766400, 182.00429308763000, 183.62347330064100, 185.57299458485300, 189.35191538410900, 194.23220405450000, 200.11245439803000, 206.61598766532100, 214.60761785986900, 223.17826798412900,\n  229.59983294953500, 238.43424165042300, 244.29554481830400, 259.55186325451500, 277.88404982202100, 297.46674550039700, -42.41381860502280, -22.05561416691630, -10.76874195668260, 4.55423903808060,\n  13.05706378617280, 25.49753552893060, 31.99805128632640, 38.60334291217220, 45.70890152749460, 53.48688015996070, 60.82873486408750, 67.21350248296350, 74.98104788942620, 84.48479832948730,\n  97.41668808537870, 101.86825371917200, 131.27099687518600, 148.43969963128300, 169.23915874881900, 186.79876808562800, 222.27683786450500, 226.80174114624100, 148.12023947867300, 119.78684089821000,\n  123.91411010078200, 136.35086145099600, 148.39281918279000, 160.94079449625800, 171.89311802023800, 182.21130213571000, 189.94824024493000, 202.58335256387900, 215.72849327557900, 224.87477584909900,\n  239.93794152235800, 257.07736337068300, 274.90021437724400, 292.81710751479900, 310.18755119542100, -34.09748283129430, -20.55209191423000, -17.48926436788570, -6.64084597196063, -3.41661969757843,\n  -1.11706604110969, -0.61559202318233, -0.58343503934742, -0.97020892968976, -7.33789387098142, -13.71942088996000, -14.16303590443250, -9.85074940762922, -2.93111556519989, 0.35846302482548,\n  -0.27797938543370, -4.11254536082527, -7.19786584394603, -8.12930435521150, -7.23274284499956, -5.60008181003320, -3.98246383052538, -1.93459828828531, -0.91376116945821, 1.11347590999549,\n  3.48574296192987, 5.87739974191743, 8.51611669495514, 11.77045158406290, 16.98321519660290, 23.12127015453780, 32.37560208179040, 38.09489298723020, 44.37798569415560, 48.60625731428030,\n  57.00174598372450, 65.55585833338270, 77.53688240972470, 96.95830008657520, 111.55989345666400, 124.32277400743800, 131.41492641407500, 137.73657404096000, 141.46543104438800, 142.80498887855200,\n  149.73966957805500, 153.53899811794900, 157.79183782289500, 159.94501203849600, 162.96541156987000, 170.96418133015300, 176.71998519472000, 182.65581927371500, 189.05283471879000, 197.04766728263800,\n  206.92813680000000, 217.32451174036000, 224.38800791349600, 232.20526011220900, 246.06238925337200, 271.09605242709400, 289.11924018406000, -31.18231981512890, 23.26913353342980, 47.27652706672060,\n];\n\nconst expectedSine = [\n  Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN,\n  Number.NaN, Number.NaN, Number.NaN,\n  -0.9874388886763950, 0.9901389074597280, 0.7430789154373040, 0.9373629963767220, 0.9539229407379340, 0.7583605937417590, 0.9430523282622310, 0.9954894450161860, 0.2677593920226290, -0.1322524693787770,\n  -0.0076634783681766, 0.0131173971015221, 0.1186956105503800, 0.1801829214581150, 0.2520928115991770, 0.4006612403521310, 0.5628300247542610, 0.7147094931388430, 0.8184710623910560, 0.8857771050315170,\n  0.8580051054352940, 0.9044393510772560, 0.9477105512269880, 0.9791019835938800, 0.9961661502181070, 0.9822460058918840, 0.8465682109433450, 0.4079557291761370, -0.4004013665430420, -0.8421036878603060,\n  -0.9017223344864390, -0.9865337683617420, -0.9643853118844240, -0.9598599815079740, 0.8037267766705520, 0.9460240546952540, 0.8605016852751490, 0.7070533756573690, 0.5243161160505560, 0.2294102723810260,\n  -0.2198489640488240, -0.7336434870569610, -0.9451240721288890, -0.9987083965673280, -0.8075209150605320, -0.5085066436721070, -0.1270264128351010, 0.2721854747475370, 0.6055917784886910, 0.8413676398899490,\n  0.9777625488346960, 0.9839896992562860, 0.8436921159626520, 0.5969447127483850, 0.3094984574050860, 0.0167240212732023, -0.2467178763145330, -0.4560076441356610, -0.6309543271436050, -0.7546432714599400,\n  -0.9163549058535820, -0.9842983646399860, -0.9864836587318940, -0.8626035608908470, -0.6813902164376480, 0.1858483908516400, 0.9266383325265240, 0.9950614294774160, 0.8974629492620320, 0.7466772329212920,\n  0.5703644678660200, 0.3750953336668390, 0.1580160207328650, -0.1758076016904010, -0.3745982816248290, -0.5276875478007350, -0.6362049492364280, -0.6890552092033720, -0.6115639640491650, -0.4788688150780810,\n  -0.1254943141670760, -0.0616279223840421, 0.1084398125157010, 0.0507107190849194, 0.0459258492372133, -0.0603960127005548, -0.1802614924906120, -0.3445196936393600, -0.5039846767879240, -0.6418346779779860,\n  -0.7682209569752370, -0.8811490866996920, -0.9663490613441720, -0.9999985470701830, -0.9546858649139780, -0.3854582462377580, 0.3634207386677970, 0.8289947319983170, 0.9714909193370010, 0.9983857647297360,\n  0.9298786498974830, 0.7522877436715410, 0.5261656913352240, 0.3198410350076260, 0.2206250837743810, 0.2324067219830480, 0.2622555352188150, 0.2488016117878720, 0.2127369252136960, 0.1536174562004720,\n  0.0365743929645966, -0.0132935129731492, -0.0205972714630062, -0.0506416644795282, -0.0877504363551195, -0.1297549156821220, -0.0930172242876989, -0.0576191763867261, -0.0229433518557876, 0.0363188766621527,\n  0.0001870031678450, -0.0349743794742610, -0.0631993922084072, -0.0971138050129020, -0.1624979390656570, -0.2458522401310880, -0.3438638176192820, -0.4480085728769030, -0.5679531815591490, -0.6842705623709480,\n  -0.7615364178876680, -0.8520399314791350, -0.9010432983130380, -0.9834194615045590, -0.9905476870988080, -0.8872786828920670, -0.6744804687446460, -0.3755063881128740, -0.1868453942460660, 0.0794027920237213,\n  0.2259213674108930, 0.4304722733878400, 0.5298904205645140, 0.6239251933219030, 0.7158012315374360, 0.8037206344012380, 0.8731666378756390, 0.9219544489912210, 0.9658401621307640, 0.9953707336137810,\n  0.9916336070524670, 0.9786230879164460, 0.7515981300962940, 0.5233956277754490, 0.1867099275394900, -0.1183826185655860, -0.6727134585677300, -0.7289894296051810, 0.5281384062949680, 0.8678795703185170,\n  0.8298749053132090, 0.6902403611717530, 0.5240926478571590, 0.3265450140990430, 0.1410201458363470, -0.0385849224232995, -0.1727584531294190, -0.3840270653389320, -0.5839449895165980, -0.7055596595779030,\n  -0.8654833335157070, -0.9746729156329320, -0.9963449765889340, -0.9217474051403660, -0.7639362508931000, -0.5606026149454520, -0.3510588357296890, -0.3005270942524180, -0.1156452933894830, -0.0595959289499457,\n  -0.0194952452596040, -0.0107439009455394, -0.0101826864310280, -0.0169325310282176, -0.1277205927832380, -0.2371674472604240, -0.2446819018845580, -0.1710822516700410, -0.0511353061733825, 0.0062563192154928,\n  -0.0048516364950122, -0.0717158401086350, -0.1252962791394740, -0.1414075684951130, -0.1259001773043880, -0.0975843207989124, -0.0694511521424164, -0.0337586943638060, -0.0159474649423128, 0.0194325975213332,\n  0.0608001695138442, 0.1024001693533800, 0.1480876049487220, 0.2039912059729780, 0.2920915426229240, 0.3926785587852390, 0.5354672113127650, 0.6169657298756830, 0.6993887726683350, 0.7501832875129180,\n  0.8386871644310250, 0.9103651277237890, 0.9764351312310820, 0.9926345853646890, 0.9300339420371430, 0.8258742381128520, 0.7499387624704700, 0.6725402422076670, 0.6229867037490120, 0.6045297568333980,\n  0.5039297182908610, 0.4455885766999630, 0.3779726796046290, 0.3429218283962470, 0.2929489553699220, 0.1570518914741900, 0.0572157944020108, -0.0463361936582273, -0.1573451851308570, -0.2931672021738570,\n  -0.4528725984930980, -0.6063286567677410, -0.6995137850617610, -0.7902112777605310, -0.9139878106324440, -0.9998170324167670, -0.9448389776183910, -0.5177630386017770, 0.3950506584043580, 0.7346367050507850,\n];\n\nconst expectedSineLead = [\n  Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN,\n  Number.NaN, Number.NaN, Number.NaN,\n  -0.5865008756606930, 0.6010760134973850, 0.0522375953518313, 0.9091393120803100, 0.8866939724449600, 0.9971590529781770, 0.9020538171846020, 0.6368324474361340, 0.8706218323553310, 0.6073789821251400,\n  0.7016671195580980, 0.7163213444559970, 0.7860384941578910, 0.8229422149880850, 0.8625259332042720, 0.9311800857903170, 0.9824560606058480, 0.9999415670942400, 0.9850130504072370, 0.9545149566415440,\n  0.9699003741647460, 0.9411885260170180, 0.8957923799982550, 0.8361338691916270, 0.6425371253442400, 0.5619014189746250, 0.2222351486408030, -0.3571214380720770, -0.9310766529205810, -0.9768109870729720,\n  -0.9433073135761630, -0.8132374819975520, -0.8689541000414960, -0.8770525415166500, 0.1475931480924170, 0.4397692495572380, 0.2482327294712000, -0.0000755239717352, -0.2313707009630830, -0.5260305525740280,\n  -0.8452633448517760, -0.9992677882479890, -0.8993236485622310, -0.6702662015228300, -0.1539244027456790, 0.2492915015754510, 0.6115575080093740, 0.8728740226620090, 0.9909162839717420, 0.9771019730335230,\n  0.8396734557306670, 0.5697614425887530, 0.2169861504951610, -0.1451956537134670, -0.4535393411417920, -0.6951822190514210, -0.8597041192450010, -0.9517540619903210, -0.9947396502168260, -0.9975710951353530,\n  -0.9310629460274820, -0.8208173676831810, -0.5816830016568750, -0.2522412838591280, 0.0357301876330212, 0.8262025299696030, 0.9210718625890570, 0.6334265388777150, 0.3227045140978610, 0.0576226338781002,\n  -0.1775030791814070, -0.3902458029200550, -0.5864888795958400, -0.8204080195335590, -0.9205013506909260, -0.9737752029989330, -0.9954119273819820, -0.9996821539441930, -0.9919003713144100, -0.9593708639942700,\n  -0.7902545151018210, -0.7493402295361840, -0.6262584572592140, -0.6703391127989300, -0.6738861991115050, -0.7485223840691900, -0.8229875936131110, -0.9074292725287760, -0.9671080150009190, -0.9960855660349680,\n  -0.9958932520932240, -0.9574138340514830, -0.8652041011461150, -0.7083111287566420, -0.4646190135134790, 0.3799051621364850, 0.9157357095569460, 0.9816417978833290, 0.8545864068220020, 0.6658040149144910,\n  0.3974026651162090, 0.0660812869143209, -0.2292556000437840, -0.4438015597350790, -0.5336772503175370, -0.5234089018965820, -0.4969142899048420, -0.5089421255844810, -0.5404930402692640, -0.5900897578798300,\n  -0.6807716781362360, -0.7164442325283580, -0.7215212414254330, -0.7420085481569570, -0.7664280384961050, -0.7928795495979390, -0.7698142295937170, -0.7466749292126270, -0.7231440467849940, -0.6809589449044260,\n  -0.7069745376146330, -0.7314048006899320, -0.7503819371382670, -0.7724343179777090, -0.8126119368714450, -0.8592475635206920, -0.9071355135227490, -0.9489640525891650, -0.9835954633562350, -0.9994945745179860,\n  -0.9967791122784410, -0.9726372051055730, -0.9438266487324060, -0.8236131488885440, -0.6034299982731380, -0.3012592866284200, 0.0451220549347129, 0.3898374562298360, 0.5625345127655470, 0.7610204246544270,\n  0.8485754490678880, 0.9426271321408510, 0.9743624133056830, 0.9937744211043730, 0.9999234595478280, 0.9890496834828740, 0.9620813190845490, 0.9257815160887320, 0.8661907447689560, 0.7717933199927350,\n  0.6099143741297350, 0.5465660379422910, 0.0650374448708587, -0.2324218737852860, -0.5626485140051470, -0.7858435966184150, -0.9988707502989160, -0.9995056053709990, -0.2269953459568470, 0.2624108067271960,\n  0.1922802999868170, -0.0235747958138623, -0.2316259847404300, -0.4374421611852380, -0.6003241683068820, -0.7338638764656210, -0.8186335536900920, -0.9244352736103260, -0.9869359601928300, -0.9999976116346720,\n  -0.9662055929589490, -0.8473318021865150, -0.6441207677355740, -0.3775643212631200, -0.0838943519776584, 0.1891385830833930, 0.4138657569596380, 0.4619148062355390, 0.6205889300081480, 0.6637092720958960,\n  0.6931871751689060, 0.6994688836282570, 0.6998698746833480, 0.6950322988997850, 0.6110036127434860, 0.5192294559798360, 0.5125969121038930, 0.5757083087460280, 0.6700235759926480, 0.7115168281729820,\n  0.7036678339992160, 0.6545752979714670, 0.6129364849611150, 0.6000111421090120, 0.6124554033545860, 0.6347294098505560, 0.6562899876128100, 0.6828327376881520, 0.6957402985880160, 0.7207141791649000,\n  0.7487908163625680, 0.7757975773123150, 0.8040241461526770, 0.8364818144534760, 0.8828100232401670, 0.9279746557928580, 0.9758237828336110, 0.9927466296862280, 0.9999410720652520, 0.9980198650776760,\n  0.9781412645524530, 0.9363303228513180, 0.8430454008691210, 0.6162348270656310, 0.3977902134753690, 0.1852760304055380, 0.0625305163973752, -0.0477440698701413, -0.1126037294757250, -0.1358018386690990,\n  -0.2544275858052280, -0.3179500551407050, -0.3873842567862560, -0.4217482646499100, -0.4689384577141290, -0.5872793774815170, -0.6654907471409910, -0.7391119157698760, -0.8095586705039820, -0.8833378646001520,\n  -0.9506681839279980, -0.9910404297342630, -0.9999429557041830, -0.9921031907559610, -0.9331896469980470, -0.6934514712647360, -0.4364996881408350, 0.2388331163456430, 0.9289332462002390, 0.9992107524543470,\n];\n\nconst expectedITL = [\n  Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN, Number.NaN,\n  Number.NaN, Number.NaN, Number.NaN,\n  0.0000000000000, 36.3130000000000, 63.1977500000000, 81.3542500000000, 90.5332083333333, 90.7274166666667, 90.8630416666667, 90.7904791666667, 90.5763791666666, 90.1190625000000,\n  89.5976625000000, 88.9683964285714, 88.1875857142857, 87.4745535714286, 86.8648353174603, 86.5406809523809, 86.4262830808081, 86.4413116161616, 86.4596270104895, 86.4576728687979,\n  86.5316151556777, 86.5976538663542, 86.6975181489262, 86.7923007288442, 86.7608821809425, 86.6849764619883, 86.4810361842105, 86.2045236842105, 85.9179875000000, 85.7217875000000,\n  85.6731214285714, 85.7294946428571, 85.8820685064935, 86.1150389610390, 86.4291412337662, 86.8542435064935, 87.1897624458874, 87.4640642857143, 87.6829172619047, 87.8553064223057,\n  87.9218177631579, 87.9011081656347, 87.8265154798762, 87.7802132352941, 87.8975367647059, 87.9932665441176, 88.0649329044118, 88.1058593750000, 88.0771718750000, 87.9885156250000,\n  87.8919218750000, 87.8735468750000, 87.9217812500000, 88.0132343750000, 88.1667968750000, 88.3524062500000, 88.5845312500000, 88.8867187500000, 89.2125000000000, 89.3545928308824,\n  89.4090386029412, 89.2706643178105, 89.1179705882353, 89.2742091503268, 89.6278143274854, 90.2226447368421, 90.9753333333333, 91.7409342105263, 92.5893245614035, 93.3576615497076,\n  94.0969948830409, 94.8495138888889, 95.5591111111111, 96.4495179738562, 97.4016168300654, 98.4999640522876, 99.7313676470588, 101.1202794117650, 102.8572647058820, 104.2939607843140,\n  105.8849452614380, 107.0814910990710, 108.1507799707600, 108.9417928362570, 109.6523348684210, 110.1277241228070, 110.5267148809520, 110.9164619047620, 111.3471666666670, 111.8653571428570,\n  112.3597857142860, 112.7992380952380, 113.1747142857140, 113.5586666666670, 114.0577738095240, 114.6932047619050, 115.2846898809520, 115.7229029761900, 115.8414500000000, 115.7467000000000,\n  115.6115500000000, 115.5303375000000, 115.5548000000000, 115.6614000000000, 115.9122375000000, 116.3321625000000, 116.8428375000000, 117.2830500000000, 117.7253922619050, 118.0727228896100,\n  118.4984101731600, 119.2198956709960, 120.0234311688310, 120.9289954887220, 121.7905789473680, 122.5689105263160, 123.2356894736840, 123.8346386591480, 124.4085501367050, 124.8498862554110,\n  125.4094954004330, 126.1065170454550, 126.7064925000000, 127.4240825000000, 127.8647176923080, 128.0896789173790, 128.1540621001220, 128.0788455784320, 128.1446725050170, 128.3183694581280,\n  128.4362103448280, 128.5848821839080, 128.6186973303670, 128.7719790322580, 128.8848929180570, 128.9942553260240, 129.0945314906040, 129.1530999536860, 129.0838581603580, 128.8024011680910,\n  128.4259584615380, 127.8496453846150, 127.2448900000000, 126.6843541666670, 126.1858333333330, 125.7495416666670, 125.2761041666670, 124.7606666666670, 124.3471666666670, 124.0095208333330,\n  123.7731875000000, 123.6621125000000, 123.6056416666670, 123.5744416666670, 123.5148250000000, 123.4937083333330, 123.4761041666670, 123.6832443181820, 124.0308714826840, 124.5405766233770,\n  125.2020811061750, 125.8966645363410, 126.4680156432750, 126.9272675438600, 127.4003611111110, 127.8605833333330, 128.1324342105260, 128.3722748538010, 128.6053611111110, 128.6804526315790,\n  128.7627552631580, 128.7694289473680, 128.7595250000000, 128.7409750000000, 128.6613250000000, 128.4858250000000, 128.2154750000000, 127.8500845238100, 127.4645440476190, 127.1153875541130,\n  126.7443323451910, 126.3035666290230, 125.8504999604740, 125.3674468450390, 124.8902561172160, 124.3766410622710, 123.8351980503370, 123.2633556835640, 122.5153494623660, 121.7438575268820,\n  120.2481227342550, 118.4358502645500, 116.5046713064710, 114.5443467073670, 112.9054354415950, 111.4893484615380, 110.7192281481480, 110.1931396825400, 110.2034782445810, 110.1120396292030,\n  109.5987628888250, 108.8005075604840, 107.8019843750000, 106.8111562500000, 105.6388140120970, 104.3560227486560, 102.7347166042630, 101.0735459592080, 99.5273900488400, 98.3256981583231,\n  97.3580796011396, 96.7594046153846, 96.4602600000000, 96.1232166666667, 96.1488191666667, 96.4425624637681, 96.7984085144927, 97.1613937318841, 97.3868053381643, 97.5343687643678,\n  97.8406773180077, 98.3334693486590, 99.0595665024631, 99.8837727832512, 100.6778885467980, 101.4226269841270, 102.0713564814810, 102.6847440476190, 103.2616851851850, 103.9931695156700,\n  104.8224465811970, 105.7652606837610, 106.6112275641030, 107.5680471014490, 108.3253364624510, 108.8823498494260, 109.2757501411630, 109.6873409090910, 110.1830037593980, 110.4551917293230,\n];\n\n// Expected tcm value array from MBST. First 63 entries NaN, then 138 ±1 entries (201 total).\nconst N = Number.NaN;\nconst expectedValue = [\n  N, N, N, N, N, N, N, N, N, N,\n  N, N, N, N, N, N, N, N, N, N,\n  N, N, N, N, N, N, N, N, N, N,\n  N, N, N, N, N, N, N, N, N, N,\n  N, N, N, N, N, N, N, N, N, N,\n  N, N, N, N, N, N, N, N, N, N,\n  N, N, N,\n  1, 1, 1, 1, 1, 1, 1, -1, -1, 1, 1, 1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,\n  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, -1, 1, 1, 1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,\n  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,\n  1, 1, 1, -1, 1, 1, 1, -1, -1, -1, -1, -1, -1, -1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, -1, -1, 1, 1,\n  1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,\n];\n\nconst tolerance = 1e-4;\nconst skip = 9;\nconst settleSkip = 177;\n\n// MBST wraps phase into (-180, 180]; zpano into [0, 360). Compare modulo 360.\nfunction phaseDelta(expected: number, actual: number): number {\n  let d = (expected - actual) % 360;\n  if (d > 180) d -= 360;\n  else if (d < -180) d += 360;\n  return d;\n}\n\ndescribe('TrendCycleMode', () => {\n  const time = new Date(2021, 3, 1);\n\n  it('should have correct output enum values', () => {\n    expect(TrendCycleModeOutput.Value).toBe(0);\n    expect(TrendCycleModeOutput.IsTrendMode).toBe(1);\n    expect(TrendCycleModeOutput.IsCycleMode).toBe(2);\n    expect(TrendCycleModeOutput.InstantaneousTrendLine).toBe(3);\n    expect(TrendCycleModeOutput.SineWave).toBe(4);\n    expect(TrendCycleModeOutput.SineWaveLead).toBe(5);\n    expect(TrendCycleModeOutput.DominantCyclePeriod).toBe(6);\n    expect(TrendCycleModeOutput.DominantCyclePhase).toBe(7);\n  });\n\n  it('should return expected mnemonic for default params', () => {\n    const x = TrendCycleMode.default();\n    expect(x.metadata().mnemonic).toBe('tcm(0.330, 4, 1.000, 1.500%, hl/2)');\n  });\n\n  it('should return expected mnemonic for phase accumulator, tlsl=3', () => {\n    const x = TrendCycleMode.fromParams({\n      alphaEmaPeriodAdditional: 0.5,\n      estimatorType: HilbertTransformerCycleEstimatorType.PhaseAccumulator,\n      estimatorParams: { smoothingLength: 4, alphaEmaQuadratureInPhase: 0.2, alphaEmaPeriod: 0.2 },\n      trendLineSmoothingLength: 3,\n      cyclePartMultiplier: 0.5,\n      separationPercentage: 2.0,\n    });\n    expect(x.metadata().mnemonic).toBe('tcm(0.500, 3, 0.500, 2.000%, pa(4, 0.200, 0.200), hl/2)');\n  });\n\n  it('should return expected metadata', () => {\n    const x = TrendCycleMode.default();\n    const meta = x.metadata();\n    const mn = 'tcm(0.330, 4, 1.000, 1.500%, hl/2)';\n    const mnTrend = 'tcm-trend(0.330, 4, 1.000, 1.500%, hl/2)';\n    const mnCycle = 'tcm-cycle(0.330, 4, 1.000, 1.500%, hl/2)';\n    const mnITL = 'tcm-itl(0.330, 4, 1.000, 1.500%, hl/2)';\n    const mnSine = 'tcm-sine(0.330, 4, 1.000, 1.500%, hl/2)';\n    const mnSineLead = 'tcm-sineLead(0.330, 4, 1.000, 1.500%, hl/2)';\n    const mnDCP = 'dcp(0.330, hl/2)';\n    const mnDCPha = 'dcph(0.330, hl/2)';\n\n    expect(meta.type).toBe(IndicatorType.TrendCycleMode);\n    expect(meta.mnemonic).toBe(mn);\n    expect(meta.description).toBe('Trend versus cycle mode ' + mn);\n    expect(meta.outputs.length).toBe(8);\n\n    expect(meta.outputs[0].kind).toBe(TrendCycleModeOutput.Value);\n    expect(meta.outputs[0].type).toBe(OutputType.Scalar);\n    expect(meta.outputs[0].mnemonic).toBe(mn);\n    expect(meta.outputs[0].description).toBe('Trend versus cycle mode ' + mn);\n\n    expect(meta.outputs[1].kind).toBe(TrendCycleModeOutput.IsTrendMode);\n    expect(meta.outputs[1].mnemonic).toBe(mnTrend);\n    expect(meta.outputs[1].description).toBe('Trend versus cycle mode, is-trend flag ' + mnTrend);\n\n    expect(meta.outputs[2].kind).toBe(TrendCycleModeOutput.IsCycleMode);\n    expect(meta.outputs[2].mnemonic).toBe(mnCycle);\n    expect(meta.outputs[2].description).toBe('Trend versus cycle mode, is-cycle flag ' + mnCycle);\n\n    expect(meta.outputs[3].kind).toBe(TrendCycleModeOutput.InstantaneousTrendLine);\n    expect(meta.outputs[3].mnemonic).toBe(mnITL);\n\n    expect(meta.outputs[4].kind).toBe(TrendCycleModeOutput.SineWave);\n    expect(meta.outputs[4].mnemonic).toBe(mnSine);\n\n    expect(meta.outputs[5].kind).toBe(TrendCycleModeOutput.SineWaveLead);\n    expect(meta.outputs[5].mnemonic).toBe(mnSineLead);\n\n    expect(meta.outputs[6].kind).toBe(TrendCycleModeOutput.DominantCyclePeriod);\n    expect(meta.outputs[6].mnemonic).toBe(mnDCP);\n    expect(meta.outputs[6].description).toBe('Dominant cycle period ' + mnDCP);\n\n    expect(meta.outputs[7].kind).toBe(TrendCycleModeOutput.DominantCyclePhase);\n    expect(meta.outputs[7].mnemonic).toBe(mnDCPha);\n    expect(meta.outputs[7].description).toBe('Dominant cycle phase ' + mnDCPha);\n  });\n\n  it('should throw if α is out of range (0, 1]', () => {\n    expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0 })).toThrowError(/α for additional smoothing/);\n    expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: -0.1 })).toThrowError(/α for additional smoothing/);\n    expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 1.0001 })).toThrowError(/α for additional smoothing/);\n    expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.5 })).not.toThrow();\n    expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 1.0 })).not.toThrow();\n  });\n\n  it('should throw if trend line smoothing length is not 2, 3, or 4', () => {\n    expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 1 }))\n      .toThrowError(/trend line smoothing length/);\n    expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 5 }))\n      .toThrowError(/trend line smoothing length/);\n    expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 2 })).not.toThrow();\n    expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 3 })).not.toThrow();\n    expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, trendLineSmoothingLength: 4 })).not.toThrow();\n  });\n\n  it('should throw if cycle part multiplier is out of range (0, 10]', () => {\n    expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, cyclePartMultiplier: 0 }))\n      .toThrowError(/cycle part multiplier/);\n    expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, cyclePartMultiplier: 10.00001 }))\n      .toThrowError(/cycle part multiplier/);\n  });\n\n  it('should throw if separation percentage is out of range (0, 100]', () => {\n    expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, separationPercentage: 0 }))\n      .toThrowError(/separation percentage/);\n    expect(() => TrendCycleMode.fromParams({ alphaEmaPeriodAdditional: 0.33, separationPercentage: 100.00001 }))\n      .toThrowError(/separation percentage/);\n  });\n\n  it('should return NaN 8-tuple for NaN input', () => {\n    const x = TrendCycleMode.default();\n    const out = x.update(Number.NaN);\n    expect(out.length).toBe(8);\n    for (const v of out) {\n      expect(Number.isNaN(v)).toBe(true);\n    }\n  });\n\n  it('should match reference period past settle window', () => {\n    const x = TrendCycleMode.default();\n    for (let i = skip; i < input.length; i++) {\n      const [, , , , , , period] = x.update(input[i]);\n      if (Number.isNaN(period) || i < settleSkip) continue;\n      expect(Math.abs(expectedPeriod[i] - period))\n        .withContext(`period[${i}]: expected ${expectedPeriod[i]}, actual ${period}`)\n        .toBeLessThan(tolerance);\n    }\n  });\n\n  it('should match reference phase (mod 360) past settle window', () => {\n    const x = TrendCycleMode.default();\n    for (let i = skip; i < input.length; i++) {\n      const [, , , , , , , phase] = x.update(input[i]);\n      if (Number.isNaN(phase) || Number.isNaN(expectedPhase[i]) || i < settleSkip) continue;\n      const d = phaseDelta(expectedPhase[i], phase);\n      expect(Math.abs(d))\n        .withContext(`phase[${i}]: expected ${expectedPhase[i]}, actual ${phase}`)\n        .toBeLessThan(tolerance);\n    }\n  });\n\n  it('should match reference sine wave past settle window', () => {\n    const x = TrendCycleMode.default();\n    for (let i = skip; i < input.length; i++) {\n      const [, , , , sine] = x.update(input[i]);\n      if (Number.isNaN(sine) || Number.isNaN(expectedSine[i]) || i < settleSkip) continue;\n      expect(Math.abs(expectedSine[i] - sine))\n        .withContext(`sine[${i}]: expected ${expectedSine[i]}, actual ${sine}`)\n        .toBeLessThan(tolerance);\n    }\n  });\n\n  it('should match reference sine wave lead past settle window', () => {\n    const x = TrendCycleMode.default();\n    for (let i = skip; i < input.length; i++) {\n      const [, , , , , sineLead] = x.update(input[i]);\n      if (Number.isNaN(sineLead) || Number.isNaN(expectedSineLead[i]) || i < settleSkip) continue;\n      expect(Math.abs(expectedSineLead[i] - sineLead))\n        .withContext(`sineLead[${i}]: expected ${expectedSineLead[i]}, actual ${sineLead}`)\n        .toBeLessThan(tolerance);\n    }\n  });\n\n  it('should match reference instantaneous trend line past settle window', () => {\n    const x = TrendCycleMode.default();\n    for (let i = skip; i < input.length; i++) {\n      const [, , , itl] = x.update(input[i]);\n      if (Number.isNaN(itl) || Number.isNaN(expectedITL[i]) || i < settleSkip) continue;\n      expect(Math.abs(expectedITL[i] - itl))\n        .withContext(`itl[${i}]: expected ${expectedITL[i]}, actual ${itl}`)\n        .toBeLessThan(tolerance);\n    }\n  });\n\n  it('should match reference tcm value (except MBST known mismatches)', () => {\n    const x = TrendCycleMode.default();\n    const limit = expectedValue.length;\n    for (let i = skip; i < input.length; i++) {\n      const [value] = x.update(input[i]);\n      if (i >= limit) continue;\n      // MBST known mismatches.\n      if (i === 70 || i === 71) continue;\n      if (Number.isNaN(value) || Number.isNaN(expectedValue[i])) continue;\n      expect(Math.abs(expectedValue[i] - value))\n        .withContext(`value[${i}]: expected ${expectedValue[i]}, actual ${value}`)\n        .toBeLessThan(tolerance);\n    }\n  });\n\n  it('should have complementary is-trend / is-cycle flags (0/1) aligned with value sign', () => {\n    const x = TrendCycleMode.default();\n    for (let i = skip; i < input.length; i++) {\n      const [value, trend, cycle] = x.update(input[i]);\n      if (Number.isNaN(value)) continue;\n      expect(trend + cycle).toBe(1);\n      if (value > 0) expect(trend).toBe(1);\n      if (value < 0) expect(trend).toBe(0);\n    }\n  });\n\n  it('should become primed within the input sequence', () => {\n    const x = TrendCycleMode.default();\n    expect(x.isPrimed()).toBe(false);\n\n    let primedAt = -1;\n    for (let i = 0; i < input.length; i++) {\n      x.update(input[i]);\n      if (x.isPrimed() && primedAt < 0) primedAt = i;\n    }\n    expect(primedAt).toBeGreaterThanOrEqual(0);\n    expect(x.isPrimed()).toBe(true);\n  });\n\n  const primeCount = 200;\n  const value = 100.0;\n\n  function checkOutput(out: any[]): void {\n    expect(out.length).toBe(8);\n    for (let i = 0; i < 8; i++) {\n      const s = out[i] as Scalar;\n      expect(s.time).toEqual(time);\n    }\n  }\n\n  it('should produce 8-element scalar output via updateScalar', () => {\n    const x = TrendCycleMode.default();\n    for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);\n    const s = new Scalar();\n    s.time = time;\n    s.value = value;\n    checkOutput(x.updateScalar(s) as any[]);\n  });\n\n  it('should produce 8-element scalar output via updateBar', () => {\n    const x = TrendCycleMode.default();\n    for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);\n    const bar = new Bar({ time, open: value, high: value, low: value, close: value, volume: 0 });\n    checkOutput(x.updateBar(bar) as any[]);\n  });\n\n  it('should produce 8-element scalar output via updateQuote', () => {\n    const x = TrendCycleMode.default();\n    for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);\n    const q = new Quote({ time, bid: value, ask: value, bidSize: 0, askSize: 0 });\n    checkOutput(x.updateQuote(q) as any[]);\n  });\n\n  it('should produce 8-element scalar output via updateTrade', () => {\n    const x = TrendCycleMode.default();\n    for (let i = 0; i < primeCount; i++) x.update(input[i % input.length]);\n    const t = new Trade({ time, price: value, volume: 0 });\n    checkOutput(x.updateTrade(t) as any[]);\n  });\n});\n"
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

## Assistant (Build · claude-opus-4.7 · 5.7s)

_Thinking:_

 The LSP errors are showing up in unrelated MBST C files, so I'll skip past those and run the TypeScript tests instead.

**Tool: bash**

**Input:**
```json
{
  "command": "node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=TrendCycleMode 2>&1 | tail -60",
  "workdir": "/home/dev/zpano/ts"
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

## Assistant (Build · claude-opus-4.7 · 4.7s)

**Tool: bash**

**Input:**
```json
{
  "command": "node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=TrendCycleMode 2>&1 | tail -80",
  "workdir": "/home/dev/zpano/ts",
  "description": "Run TrendCycleMode jasmine tests"
}
```

**Output:**
```
Started
.....................


Ran 21 of 749 specs
21 specs, 0 failures
Finished in 0.067 seconds

```

---

## Assistant (Build · claude-opus-4.7 · 4.7s)

All 21 TS specs pass on first try. Now let me update the skill and verify Go still passes.

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./indicators/johnehlers/trendcyclemode/... 2>&1 | tail -20",
  "description": "Run Go TrendCycleMode tests",
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
ok  	zpano/indicators/johnehlers/trendcyclemode	(cached)

```

---

## Assistant (Build · claude-opus-4.7 · 3.9s)

Both test suites green. Now the skill update.

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

(End of file - total 981 lines)
</content>
```

---

## Assistant (Build · claude-opus-4.7 · 38.1s)

_Thinking:_

 The skill needs a few more refinements: exposing wrapper internals through accessor methods so I can access things like smoothed prices and max periods without duplicating logic, especially when composing indicators together. I should also handle error wrapping more carefully for sub-indicators by adding a double prefix to component errors. And I need to make sure TypeScript component functions don't throw, which means skipping certain test patterns.

**Tool: edit**

**Input:**
```json
{
  "filePath": "/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md",
  "oldString": "Same helper applies in TS, using `(a - b) % 360` (JavaScript's `%` is remainder, which\npreserves the sign of the dividend — same semantics as `math.Mod` for this purpose).",
  "newString": "Same helper applies in TS, using `(a - b) % 360` (JavaScript's `%` is remainder, which\npreserves the sign of the dividend — same semantics as `math.Mod` for this purpose).\n\n---\n\n## Exposing Wrapped-Indicator Internals via Accessors\n\nWhen a new indicator wraps an existing merged indicator (e.g., `TrendCycleMode` wraps\n`DominantCycle`) and needs access to internal state the inner indicator already computes\n— **add read-only accessors on the inner indicator** rather than duplicating the\ncomputation in the outer.\n\nCommon examples on `DominantCycle`:\n\n| Need | Add to inner | Avoid |\n|---|---|---|\n| WMA-smoothed price (`htce.Smoothed`) for the trendline | `SmoothedPrice() float64` / `get smoothedPrice(): number` | Re-instantiating a second HTCE in the outer |\n| `MaxPeriod` constant for sizing the raw-input buffer | `MaxPeriod() int` / `get maxPeriod(): number` | Hard-coding `50` in the outer |\n\nRules:\n\n1. Accessors are **read-only** and do not acquire locks when returning immutable\n   constants (e.g., `MaxPeriod`). For mutable state (e.g., `SmoothedPrice` updated each\n   `Update` call), use a `RLock`/`RUnlock` in Go; TS has no locking.\n2. Add matching unit tests on the inner indicator confirming the accessor returns the\n   expected value after priming.\n3. Prefer accessors over widening the inner's `Update()` return tuple — adding more\n   return values would force every existing caller to change.\n\n---\n\n## Go Error-Wrapping Depth When Wrapping Sub-Indicators\n\nWhen an outer constructor (e.g., `newTrendCycleMode`) internally calls another\nindicator's constructor (`dominantcycle.NewDominantCycleParams`), any validation error\nreturned by the inner gets **double-wrapped** by the outer's own `fmt.Errorf(\"invalid X\nparameters: %w\", err)` prefix.\n\nFor component errors (bar/quote/trade), the expected error string in tests becomes:\n\n```\ninvalid trend cycle mode parameters: invalid dominant cycle parameters: 9999: unknown bar component\n```\n\nNot:\n```\ninvalid trend cycle mode parameters: 9999: unknown bar component\n```\n\nWrite the Go tests' expected error constants with the full double-prefix so the assertion\nmatches reality. TypeScript doesn't have this issue because TS component helpers\nsilently default to close (see next section).\n\n---\n\n## TS Component Helpers Don't Throw\n\nA cross-platform portability trap: Go's `entities.BarComponentValue(c)`,\n`QuoteComponentValue(c)`, `TradeComponentValue(c)` return an error for unknown\ncomponent enum values (typically `9999` in tests). **TypeScript's equivalents\n(`barComponentValue`, `quoteComponentValue`, `tradeComponentValue`) do not throw** — they\nsilently fall back to the close/mid/price default.\n\nImplications:\n\n1. **Skip \"invalid bar/quote/trade component\" tests in TS specs** — there's no error\n   path to exercise. Port all other param validation tests (α, length, ranges, etc.)\n   as usual.\n2. **Existing TS specs follow this convention** (SineWave, DominantCycle, CenterOfGravity\n   all omit invalid-component tests). Don't add them to new indicators either.\n3. **Go specs should keep the component error tests** — they exercise a real code path.\n\n---\n\n## Tuple-Output Indicator Spec Template (8 Outputs, TrendCycleMode Pattern)\n\nWhen the outer indicator emits more than the typical 3-5 outputs (e.g., TrendCycleMode's\n8-output tuple), the TS spec pattern is:\n\n1. **Destructure with positional blanks** — use `const [, , , , sine] = x.update(v)` for\n   5th element; avoid naming unused tuple elements.\n2. **Single `checkOutput(out: any[])` helper** verifies `out.length === 8` and each is a\n   `Scalar` with matching `time`. No `Band`-specific branch needed when all outputs are\n   Scalar.\n3. **Data arrays ported verbatim from Go test file** — replace `math.NaN()` with\n   `Number.NaN`, drop Go `//nolint` tags.\n4. **Phase array comparison uses mod-360 helper** (see above). Don't skip NaN-expected\n   indices globally; skip per-element with `if (Number.isNaN(expected[i])) continue`.\n5. **Reference-value array can be shorter than input array** — e.g., TCM's `expectedValue`\n   has 201 entries while `input` has 252. Guard with `if (i >= limit) continue` inside the\n   loop instead of shortening the outer iteration range (keeps the indicator fed with the\n   full input sequence)."
}
```

**Output:**
```
Edit applied successfully.
```

---

## User

---

## Assistant (Compaction · claude-opus-4.7 · 41.0s)

## Goal

Port MBST's **TrendCycleMode (TCM)** indicator from `mbst-to-convert/john-ehlers/trend-cycle-mode/` into zpano:
- Go: `go/indicators/johnehlers/trendcyclemode/` ✅ complete
- TS: `ts/indicators/john-ehlers/trend-cycle-mode/` ✅ complete

Wraps existing `DominantCycle` (for period + phase + smoothed price) and adds raw-sample buffer for trendline, sine/sineLead, and trend-vs-cycle mode logic.

## Instructions

### Outputs (order, 8 total)
1. **Value** (+1 trend / −1 cycle, Scalar)
2. **IsTrendMode** (0/1 Scalar)
3. **IsCycleMode** (0/1 Scalar = 1 − IsTrendMode)
4. **InstantaneousTrendLine** (Scalar)
5. **SineWave** (Scalar)
6. **SineWaveLead** (Scalar)
7. **DominantCyclePeriod** (Scalar)
8. **DominantCyclePhase** (Scalar)

### Combined single test file
6 MBST reference arrays merged. skip=9, settleSkip=177, tol 1e-4. For tcm value: skip indices 70, 71 (MBST known mismatches). tcm array is 201 entries — loop must guard `i >= limit`.

### MBST params / defaults
- `alphaEmaPeriodAdditional=0.33`, `trendLineSmoothingLength=4` ∈{2,3,4}, `cyclePartMultiplier=1.0` ∈(0,10], `separationPercentage=1.5` ∈(0,100].
- Default estimator HomodyneDiscriminator, smoothingLength=4, αq=αp=0.2, **warmUpPeriod=100**.
- Default BarComponent **BarMedianPrice** (always shown in mnemonic).
- Go `NewTrendCycleModeDefault` / `NewTrendCycleModeParams` → private `newTrendCycleMode`; TS `default()` / `fromParams()`.
- Mnemonic default: `tcm(0.330, 4, 1.000, 1.500%, hl/2)`.

### Test commands
- Go: `cd /home/dev/zpano/go && go test ./indicators/johnehlers/trendcyclemode/...` ✅
- TS from `/home/dev/zpano/ts`: `node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json --filter=TrendCycleMode` ✅

## Discoveries

- MBST `dcp.RawValue` ≡ zpano `DominantCycle.Phase` — no duplicate phase logic.
- MBST TCM pushes **raw samples** (not htce.Smoothed) to its own input buffer.
- **Phase comparison:** MBST wraps into (-180, 180]; zpano into [0, 360). Compare modulo 360.
- **Go error wrapping depth:** Component errors get double-wrapped: `"invalid trend cycle mode parameters: invalid dominant cycle parameters: 9999: unknown bar component"`.
- **TS component funcs don't throw** on unknown components (silently default to close) — so TS specs skip "invalid component" tests.
- Coefficient table by tlsl: 2→(2/3,1/3); 3→(3/6,2/6,1/6); 4→(4/10,3/10,2/10,1/10).
- TS enum convention: outputs start at 0 (SineWaveOutput pattern), unlike Go which starts at 1.

## Accomplished

### Done
1. ✅ Go `SmoothedPrice()` / `MaxPeriod()` methods + tests on `DominantCycle`.
2. ✅ TS `smoothedPrice` / `maxPeriod` getters + specs on `DominantCycle`.
3. ✅ Registered `TrendCycleMode` in Go `core/type.go` + TS `core/indicator-type.ts`.
4. ✅ **Go package complete, tests pass.**
5. ✅ **TS package complete — all 21 specs pass on first run:**
   - `trend-cycle-mode-output.ts`, `trend-cycle-mode-params.ts`, `trend-cycle-mode.ts`, `trend-cycle-mode.spec.ts`.
6. ✅ **Skill documentation updated** — appended 4 new sections to `.opencode/skills/mbst-indicator-conversion/SKILL.md`:
   - Exposing Wrapped-Indicator Internals via Accessors
   - Go Error-Wrapping Depth When Wrapping Sub-Indicators
   - TS Component Helpers Don't Throw
   - Tuple-Output Indicator Spec Template (8 Outputs, TrendCycleMode Pattern)

### Remaining / Optional
- None explicitly. Could re-run full TS test suite to confirm no regressions elsewhere.
- Could update project-level indicator index/README if one exists.

## Relevant files / directories

### Go — complete
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/` (5 files, tests passing)
- `/home/dev/zpano/go/indicators/johnehlers/dominantcycle/dominantcycle.go` + `_test.go` — SmoothedPrice/MaxPeriod
- `/home/dev/zpano/go/indicators/core/type.go` + `type_test.go` — TrendCycleMode enum

### TS — complete
- `/home/dev/zpano/ts/indicators/john-ehlers/trend-cycle-mode/` (4 files, 21 specs passing)
- `/home/dev/zpano/ts/indicators/john-ehlers/dominant-cycle/dominant-cycle.ts` + `.spec.ts` — smoothedPrice / maxPeriod getters
- `/home/dev/zpano/ts/indicators/core/indicator-type.ts` — TrendCycleMode added

### Reference templates used
- `/home/dev/zpano/ts/indicators/john-ehlers/sine-wave/sine-wave.spec.ts` — spec template
- `/home/dev/zpano/ts/indicators/john-ehlers/hilbert-transformer-instantaneous-trend-line/` — algorithmic reference
- `/home/dev/zpano/go/indicators/johnehlers/trendcyclemode/trendcyclemode_test.go` — source of data arrays ported to TS

### MBST source
- `/home/dev/zpano/mbst-to-convert/john-ehlers/trend-cycle-mode/TrendCycleMode.cs` / `TrendCycleModeTest.cs`

### Skills
- `/home/dev/zpano/.opencode/skills/mbst-indicator-conversion/SKILL.md` — updated with 4 new post-completion sections

### Immediate next action
Task is complete. If continuing: optionally run the full TS test suite (`node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json` from `/home/dev/zpano/ts`) to confirm no regressions in other indicators from the `DominantCycle` accessor additions.

---

## User

---

## Assistant (Build · claude-opus-4.7 · 6.3s)

_Thinking:_

 The task is done, so I should run the full TypeScript test suite to make sure nothing broke.

**Tool: bash**

**Input:**
```json
{
  "command": "node --import tsx/esm ./node_modules/.bin/jasmine --config=jasmine.json 2>&1 | tail -40",
  "description": "Run full TS test suite",
  "workdir": "/home/dev/zpano/ts",
  "timeout": 300000
}
```

**Output:**
```
Started
.............................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................................


749 specs, 0 failures
Finished in 0.656 seconds

```

---

## Assistant (Build · claude-opus-4.7 · 5.7s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./... 2>&1 | tail -20",
  "description": "Run full Go test suite",
  "timeout": 300000,
  "workdir": "/home/dev/zpano/go"
}
```

**Output:**
```
ok  	zpano/indicators/perrykaufman/kaufmanadaptivemovingaverage	0.011s
?   	zpano/indicators/timtillson	[no test files]
ok  	zpano/indicators/timtillson/t2exponentialmovingaverage	0.012s
ok  	zpano/indicators/timtillson/t3exponentialmovingaverage	0.012s
?   	zpano/indicators/tusharchande	[no test files]
ok  	zpano/indicators/tusharchande/aroon	0.014s
ok  	zpano/indicators/tusharchande/chandemomentumoscillator	0.019s
ok  	zpano/indicators/tusharchande/stochasticrelativestrengthindex	0.008s
ok  	zpano/indicators/welleswilder/averagedirectionalmovementindex	0.003s
ok  	zpano/indicators/welleswilder/averagedirectionalmovementindexrating	0.003s
ok  	zpano/indicators/welleswilder/averagetruerange	0.002s
ok  	zpano/indicators/welleswilder/directionalindicatorminus	0.002s
ok  	zpano/indicators/welleswilder/directionalindicatorplus	0.003s
ok  	zpano/indicators/welleswilder/directionalmovementindex	0.003s
ok  	zpano/indicators/welleswilder/directionalmovementminus	0.002s
ok  	zpano/indicators/welleswilder/directionalmovementplus	0.002s
ok  	zpano/indicators/welleswilder/normalizedaveragetruerange	0.002s
ok  	zpano/indicators/welleswilder/parabolicstopandreverse	0.002s
ok  	zpano/indicators/welleswilder/relativestrengthindex	0.002s
ok  	zpano/indicators/welleswilder/truerange	0.003s

```

---

## Assistant (Build · claude-opus-4.7 · 4.0s)

**Tool: bash**

**Input:**
```json
{
  "command": "go test ./... 2>&1 | grep -E \"(FAIL|---)\" | head -20",
  "description": "Check for any Go test failures",
  "workdir": "/home/dev/zpano/go"
}
```

---

## Assistant (Build · claude-opus-4.7 · 3.3s)

All tests pass, no regressions:

- **TS**: 749 specs, 0 failures
- **Go**: all packages pass, no failures

The TrendCycleMode port is complete and verified. Nothing more to do unless you want me to tackle something else.

---

