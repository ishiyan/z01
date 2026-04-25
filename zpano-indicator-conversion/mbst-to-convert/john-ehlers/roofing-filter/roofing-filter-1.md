# Roofing Filter: A Practical, Equation‑First Guide for Quant Traders

Audience: practitioners with solid math/physics backgrounds and little DSP background. Equations are given in LaTeX and code in both EasyLanguage (from the uploaded pages) and C‑like pseudocode.

---

## 1. What the “Roofing Filter” is

A roofing filter is a wide band‑pass filter that keeps only cycle components whose periods lie between two design periods:
- longest (upper) critical period Λ (in bars): everything with period > Λ is rejected by a high‑pass (HP),
- shortest (lower) critical period λ (in bars): everything with period < λ is rejected by a low‑pass (LP).

In Ehlers’ implementations:
- HP is either a one‑pole (1P) high‑pass or a two‑pole (2P) high‑pass.
- LP is a 2nd‑order low‑pass called the “Super Smoother.”

Three flavors in the pages you supplied:
- Listing 7‑1: 1P‑HP → Super Smoother (HP‑LP roofing filter).
- Listing 7‑2: 1P‑HP → Super Smoother → 1P‑HP (zero‑mean roofing filter).
- Listing 7‑3: 2P‑HP → Super Smoother (roofing filter indicator).

The 2P‑HP version effectively achieves a near zero mean without the extra HP because of steeper long‑period attenuation.

---

## 2. Minimal DSP vocabulary

- “Pole”: For a causal IIR digital filter with transfer function H(z) = B(z)/A(z), poles are the roots of A(z). A “one‑pole” filter has a first‑order denominator (1st‑order recursion), “two‑pole” is 2nd‑order (second‑order recursion). Cascading two one‑pole stages is equivalent to a two‑pole filter.
- “Gain” at frequency ω: ratio |H(e^{jω})| of sinusoidal output amplitude to input amplitude. Expressed in dB as 20 log10 |H(e^{jω})|.
- “Roll‑off 6 dB per octave”: amplitude halves (−6 dB) each time frequency doubles in the stop band. Two cascaded poles give −12 dB per octave, etc.

All periods below are in bars; angular digital frequency corresponding to a design period P is
- ω = 2π / P (radians/sample).

Ehlers uses degrees in Listings 7‑1 and 7‑2. I give both radians and degrees for clarity.

---

## 3. Building blocks and equations

### 3.1 One‑pole high‑pass (1P‑HP)

Let x[n] be the input series (e.g., close or median price) and HP[n] the 1P‑HP output.

Design coefficient (radians):
- ω_HP = 2π / Λ,
- α = (cos ω_HP + sin ω_HP − 1) / cos ω_HP.

Difference equation:
- HP[n] = (1 − α/2) (x[n] − x[n−1]) + (1 − α) HP[n−1].

(Initialization: set HP[0] = 0, and start evaluating from n = 1.)

Degrees form (as in the listings): replace ω_HP by 360°/Λ and use cosd, sind.

### 3.2 Two‑pole high‑pass (2P‑HP)

The two‑pole HP in Listing 7‑3 is the serial connection of two one‑pole HPs with a √2 frequency pre‑warping. Ehlers implements this with a single 2nd‑order recursion using

- ω_2P = (√2) π / Λ  (equivalently 0.707×360°/Λ in degrees),
- α = (cos ω_2P + sin ω_2P − 1) / cos ω_2P.

Difference equation:
- HP[n] = (1 − α/2)^2 (x[n] − 2 x[n−1] + x[n−2])
          + 2 (1 − α) HP[n−1]
          − (1 − α)^2 HP[n−2].

(Initialization: HP[0] = HP[1] = 0.)

This 2P‑HP approximates cascading two identical 1P‑HPs while keeping the desired −12 dB/oct long‑period suppression and low passband distortion.

### 3.3 Super Smoother (2nd‑order low‑pass)

Let SS[n] be the Super Smoother output. The design period is the lower critical period λ (small number of bars).

Define (radians)
- ω_LP = √2 π / λ,
- a = exp(−ω_LP),
- b = 2 a cos ω_LP.

Coefficients:
- c1 = 1 − b − (−a^2) = 1 − b + a^2,
- c2 = b,
- c3 = −a^2.

Difference equation (Ehlers’ zero‑lag form):
- SS[n] = c1 (HP[n] + HP[n−1]) / 2 + c2 SS[n−1] + c3 SS[n−2].

(Initialization: SS[0] = SS[1] = 0.)

Note: In the listings, ω_LP is written with degrees: a = exp(−1.414·π/λ), b = 2a cos(1.414·π/λ). That is the same as the formulas above.

---

## 4. Roofing filter flavors

Let Λ be the “HPPeriod” (longest admissible period) and λ the “LPPeriod” (shortest admissible period).

### 4.1 Listing 7‑1: HP‑LP roofing filter (1P‑HP → Super Smoother)

Equations:
1) Compute α(Λ) and HP[n] via the 1P‑HP in §3.1.
2) Compute SS[n] via the Super Smoother in §3.3 using λ.

Output Filt[n] = SS[n].

Properties:
- Band‑pass with ~6 dB/oct high‑pass skirt and 2nd‑order low‑pass top cutoff.
- Because market data amplitude typically increases with period ~1/f^α, the output can have a noticeable non‑zero mean in trends (Figure 7.1).

### 4.2 Listing 7‑2: Zero‑mean roofing filter (1P‑HP → Super Smoother → 1P‑HP)

Equations:
1) Compute Filt[n] as in 4.1.
2) Apply another 1P‑HP to Filt[n] with the same α(Λ):
   - ZM[n] = (1 − α/2) (Filt[n] − Filt[n−1]) + (1 − α) ZM[n−1].

Outputs:
- Filt[n]  (dashed in Figure 7.2): HP → SS,
- ZM[n]    (solid in Figure 7.2): HP → SS → HP, which has a nominally zero mean and less low‑frequency contamination.

Interpretation:
- The second HP adds another −6 dB/oct roll‑off to counteract spectral dilation (the 1/f amplitude growth), yielding a near zero‑mean band‑pass.

### 4.3 Listing 7‑3: Roofing filter indicator (2P‑HP → Super Smoother)

Equations:
1) Compute HP[n] via the 2P‑HP in §3.2 with Λ.
2) Compute SS[n] via the Super Smoother in §3.3 with λ.

Output Filt[n] = SS[n].

This “better formulation” produces a zero‑mean output without the extra 1P‑HP because the 2P‑HP steeper skirt removes more long‑period energy. Ehlers suggests simple signals by crossing Filt[n] with Filt[n−2] (not reproduced here as a strategy).

---

## 5. Parameter definitions and mapping

- Λ (HPPeriod): Longest cycle of interest (bars). Frequencies with period > Λ are rejected by the HP.
- λ (LPPeriod): Shortest cycle of interest (bars). Frequencies with period < λ are rejected by the Super Smoother.
- Radian cutoffs used in coefficient derivations:
  - 1P‑HP: ω_HP = 2π/Λ.
  - 2P‑HP: ω_2P = √2 π/Λ (equivalently use 0.707×360°/Λ in degrees).
  - Super Smoother: ω_LP = √2 π/λ.

---

## 6. Code listings (from the pages) and C‑like pseudocode

Below are the core parts of the EasyLanguage listings you uploaded, followed by equivalent C‑style pseudocode. (Variable names kept close to the book.)

### 6.1 Listing 7‑1: HP‑LP Roofing Filter

EasyLanguage (from the page):
```
Vars:
  alpha1(0),
  HP(0),
  a1(0),
  b1(0),
  c1(0),
  c2(0),
  c3(0),
  Filt(0);

// Highpass filter cyclic components whose periods are shorter than 48 bars
alpha1 = (Cosine(360 / 48) + Sine(360 / 48) - 1) / Cosine(360 / 48);
HP = (1 - alpha1 / 2)*(Close - Close[1]) + (1 - alpha1)*HP[1];

// Smooth with a Super Smoother Filter from equation 3-3
a1 = expvalue(-1.414*3.14159 / 10);
b1 = 2*a1*Cosine(1.414*180 / 10);
c2 = b1;
c3 = -a1*a1;
c1 = 1 - c2 - c3;

Filt = c1*(HP + HP[1]) / 2 + c2*Filt[1] + c3*Filt[2];
```

C‑like pseudocode:
```c
// Inputs: double Λ = 48; double λ = 10;
double alpha1 = (cos(2*M_PI/Λ) + sin(2*M_PI/Λ) - 1.0) / cos(2*M_PI/Λ);
HP[n] = (1 - alpha1/2.0) * (x[n] - x[n-1]) + (1 - alpha1) * HP[n-1];

double a = exp(-sqrt(2.0)*M_PI/λ);
double b = 2*a*cos(sqrt(2.0)*M_PI/λ);
double c1 = 1 - b + a*a;
double c2 = b;
double c3 = -a*a;

Filt[n] = c1*(HP[n] + HP[n-1])/2.0 + c2*Filt[n-1] + c3*Filt[n-2];
```

### 6.2 Listing 7‑2: Zero‑Mean Roofing Filter

EasyLanguage (from the page):
```
Vars:
  alpha1(0),
  HP(0),
  a1(0),
  b1(0),
  c1(0),
  c2(0),
  c3(0),
  Filt(0),
  Filt2(0);

alpha1 = (Cosine(360 / 48) + Sine(360 / 48) - 1) / Cosine(360 / 48);
HP = (1 - alpha1 / 2)*(Close - Close[1]) + (1 - alpha1)*HP[1];

a1 = expvalue(-1.414*3.14159 / 10);
b1 = 2*a1*Cosine(1.414*180 / 10);
c2 = b1;
c3 = -a1*a1;
c1 = 1 - c2 - c3;

Filt  = c1*(HP + HP[1]) / 2 + c2*Filt[1] + c3*Filt[2];
Filt2 = (1 - alpha1 / 2)*(Filt - Filt[1]) + (1 - alpha1)*Filt2[1];
```

C‑like pseudocode:
```c
double alpha1 = (cos(2*M_PI/Λ) + sin(2*M_PI/Λ) - 1.0) / cos(2*M_PI/Λ);
HP[n] = (1 - alpha1/2.0) * (x[n] - x[n-1]) + (1 - alpha1) * HP[n-1];

double a = exp(-sqrt(2.0)*M_PI/λ);
double b = 2*a*cos(sqrt(2.0)*M_PI/λ);
double c1 = 1 - b + a*a, c2 = b, c3 = -a*a;

Filt[n]  = c1*(HP[n] + HP[n-1])/2.0 + c2*Filt[n-1] + c3*Filt[n-2];
Filt2[n] = (1 - alpha1/2.0) * (Filt[n] - Filt[n-1]) + (1 - alpha1) * Filt2[n-1];
// Filt2 is the zero‑mean roofing filter.
```

### 6.3 Listing 7‑3: Roofing Filter Indicator (2P‑HP)

EasyLanguage (from the page; core equations):
```
Inputs: LPPeriod(40), HPPeriod(80);
Vars: alpha1(0), HP(0), a1(0), b1(0), c1(0), c2(0), c3(0), Filt(0);

alpha1 = (Cosine(.707*360 / HPPeriod) + Sine(.707*360 / HPPeriod) - 1) / Cosine(.707*360 / HPPeriod);
HP = (1 - alpha1 / 2)*(1 - alpha1 / 2)*(Close - 2*Close[1] + Close[2])
   + 2*(1 - alpha1)*HP[1] - (1 - alpha1)*(1 - alpha1)*HP[2];

a1 = expvalue(-1.414*3.14159 / LPPeriod);
b1 = 2*a1*Cosine(1.414*180 / LPPeriod);
c2 = b1;  c3 = -a1*a1;  c1 = 1 - c2 - c3;

Filt = c1*(HP + HP[1]) / 2 + c2*Filt[1] + c3*Filt[2];
```

C‑like pseudocode:
```c
// Inputs: Λ = HPPeriod; λ = LPPeriod;
double alpha1 = (cos(M_PI*sqrt(2.0)/Λ) + sin(M_PI*sqrt(2.0)/Λ) - 1.0) / cos(M_PI*sqrt(2.0)/Λ);

HP[n] = pow(1 - alpha1/2.0, 2) * (x[n] - 2*x[n-1] + x[n-2])
      + 2*(1 - alpha1) * HP[n-1]
      - pow(1 - alpha1, 2) * HP[n-2];

double a = exp(-sqrt(2.0)*M_PI/λ);
double b = 2*a*cos(sqrt(2.0)*M_PI/λ);
double c1 = 1 - b + a*a, c2 = b, c3 = -a*a;

Filt[n] = c1*(HP[n] + HP[n-1])/2.0 + c2*Filt[n-1] + c3*Filt[n-2];
// Filt is the roofing filter indicator output.
```

---

## 7. Practical notes for implementation

- Units: Internally pick either radians with cos/sin or degrees with cosd/sind; do not mix. The equations above are consistent in radians.
- Initialization and priming:
  - 1P‑HP needs at least one past sample; 2P‑HP and Super Smoother need two. Initialize histories to zero and treat the first few outputs as warm‑up.
- Numerical stability:
  - The exponential a = exp(−√2 π / λ) is strictly between 0 and 1 for λ > 0, ensuring a stable 2nd‑order LP. For typical λ ≥ 5, coefficients are well conditioned.
- Parameter selection:
  - Λ controls the HP skirt: larger Λ preserves longer cycles; smaller Λ removes more of the trend.
  - λ controls the LP skirt: larger λ preserves longer periods (wider passband); smaller λ removes faster wiggles.
- Zero mean:
  - If you use 1P‑HP → SS (Listing 7‑1), expect a non‑zero mean in trends due to spectral dilation (market amplitude grows with period). To enforce near zero mean, either add the second 1P‑HP (Listing 7‑2) or use the 2P‑HP front‑end (Listing 7‑3).
- Signals (from Listing 7‑3 narrative):
  - A simple timing signal is Filt[n] crossing Filt[n−2]. This is beyond the filter’s definition but easy to add.

---

## 8. Block‑diagram illustrations (SVG)

### 8.1 Listing 7‑1: 1P‑HP → Super Smoother
<svg width="560" height="90" xmlns="http://www.w3.org/2000/svg">
  <rect x="20" y="20" width="140" height="50" fill="#eef" stroke="#334" />
  <text x="90" y="50" font-size="14" text-anchor="middle" dominant-baseline="middle">1P High‑Pass (Λ)</text>
  <line x1="0" y1="45" x2="20" y2="45" stroke="#000"/>
  <line x1="160" y1="45" x2="200" y2="45" stroke="#000"/>
  <rect x="200" y="20" width="170" height="50" fill="#efe" stroke="#334" />
  <text x="285" y="50" font-size="14" text-anchor="middle" dominant-baseline="middle">Super Smoother (λ)</text>
  <line x1="370" y1="45" x2="560" y2="45" stroke="#000"/>
</svg>

### 8.2 Listing 7‑2: 1P‑HP → Super Smoother → 1P‑HP
<svg width="740" height="90" xmlns="http://www.w3.org/2000/svg">
  <rect x="20" y="20" width="140" height="50" fill="#eef" stroke="#334" />
  <text x="90" y="50" font-size="14" text-anchor="middle" dominant-baseline="middle">1P High‑Pass (Λ)</text>
  <line x1="0" y1="45" x2="20" y2="45" stroke="#000"/>
  <line x1="160" y1="45" x2="200" y2="45" stroke="#000"/>
  <rect x="200" y="20" width="170" height="50" fill="#efe" stroke="#334" />
  <text x="285" y="50" font-size="14" text-anchor="middle" dominant-baseline="middle">Super Smoother (λ)</text>
  <line x1="370" y1="45" x2="410" y2="45" stroke="#000"/>
  <rect x="410" y="20" width="140" height="50" fill="#eef" stroke="#334" />
  <text x="480" y="50" font-size="14" text-anchor="middle" dominant-baseline="middle">1P High‑Pass (Λ)</text>
  <line x1="550" y1="45" x2="740" y2="45" stroke="#000"/>
</svg>

### 8.3 Listing 7‑3: 2P‑HP → Super Smoother
<svg width="560" height="90" xmlns="http://www.w3.org/2000/svg">
  <rect x="20" y="20" width="140" height="50" fill="#fee" stroke="#334" />
  <text x="90" y="50" font-size="14" text-anchor="middle" dominant-baseline="middle">2P High‑Pass (Λ)</text>
  <line x1="0" y1="45" x2="20" y2="45" stroke="#000"/>
  <line x1="160" y1="45" x2="200" y2="45" stroke="#000"/>
  <rect x="200" y="20" width="170" height="50" fill="#efe" stroke="#334" />
  <text x="285" y="50" font-size="14" text-anchor="middle" dominant-baseline="middle">Super Smoother (λ)</text>
  <line x1="370" y1="45" x2="560" y2="45" stroke="#000"/>
</svg>

---

## 9. Quick reference (equations)

- 1P‑HP:
  - ω_HP = 2π/Λ,
  - α = (cos ω_HP + sin ω_HP − 1)/cos ω_HP,
  - HP[n] = (1 − α/2)(x[n] − x[n−1]) + (1 − α)HP[n−1].

- 2P‑HP:
  - ω_2P = √2 π/Λ,
  - α = (cos ω_2P + sin ω_2P − 1)/cos ω_2P,
  - HP[n] = (1 − α/2)^2 (x[n] − 2x[n−1] + x[n−2]) + 2(1 − α)HP[n−1] − (1 − α)^2 HP[n−2].

- Super Smoother:
  - ω_LP = √2 π/λ,
  - a = e^{−ω_LP}, b = 2a cos ω_LP,
  - c1 = 1 − b + a^2, c2 = b, c3 = −a^2,
  - SS[n] = c1 (HP[n] + HP[n−1])/2 + c2 SS[n−1] + c3 SS[n−2].

- Roofing filter outputs:
  - Listing 7‑1: Filt[n] = SS[n].
  - Listing 7‑2: Filt[n] = SS[n], ZM[n] = 1P‑HP(Filt; α(Λ)).
  - Listing 7‑3: Filt[n] = SS[n] with HP computed by 2P‑HP.

---

## 10. How to use in a pipeline

- Compute Filt[n] (or ZM[n]) at every bar.
- If chaining to other indicators (RSI, stochastic, etc.), feed them with Filt[n] instead of raw prices to restrict their frequency content.
- For simple cycle timing (per the book’s discussion), use Filt[n] cross Filt[n−2] for peaks/troughs; choose Λ and λ for responsiveness vs noise.

---

This document is a Markdown file with embedded LaTeX and SVG and can be copied as‑is into your notes or documentation system.