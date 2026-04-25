# Roofing Filter: Equation‑First Notes for Quant Traders

Audience: practitioners with solid math/physics backgrounds and little DSP background. Equations are provided in TeX. Code is shown in EasyLanguage (as in the book pages) and C‑like pseudocode.

---

## 1. Purpose and concept

A “roofing filter” is a wide band‑pass filter designed to keep only cycle components whose periods lie between two design periods:
- longest (upper) critical period $\,\Lambda\,$ (bars): reject periods $>\Lambda$ via a high‑pass,
- shortest (lower) critical period $\,\lambda\,$ (bars): reject periods $<\lambda$ via a low‑pass (the “Super Smoother”).

Three flavors on the provided pages:
- Listing 7‑1: one‑pole high‑pass $\to$ Super Smoother.
- Listing 7‑2: one‑pole high‑pass $\to$ Super Smoother $\to$ one‑pole high‑pass (zero‑mean).
- Listing 7‑3: two‑pole high‑pass $\to$ Super Smoother (preferred “indicator” formulation).

The two‑pole high‑pass produces steeper long‑period attenuation (about $-12$ dB per octave), largely eliminating low‑frequency bias without the extra one‑pole at the output.

---

## 2. Minimal DSP vocabulary

- Pole. For a causal IIR filter with transfer function $H(z)=\dfrac{B(z)}{A(z)}$, poles are roots of $A(z)$. A “one‑pole” (1P) has a first‑order denominator (first‑order recursion); “two‑pole” (2P) is second‑order (second‑order recursion). Cascading two one‑pole sections yields an effective two‑pole response.
- Gain at angular frequency $\omega$. $\,|H(\mathrm{e}^{j\omega})|$, i.e., the sinusoidal output amplitude divided by input amplitude. In dB: $\,20\log_{10}|H(\mathrm{e}^{j\omega})|$.
- Roll‑off “$6$ dB per octave”. In the stop band, the amplitude halves (−6 dB) when frequency doubles. Two poles yield approximately −12 dB/oct.

All periods are in bars; the digital angular frequency corresponding to a period $P$ is
$$
\omega = \frac{2\pi}{P}\,.
$$

Ehlers frequently expresses angles in degrees in the book; below, formulas are given in radians with the degree equivalents indicated.

---

## 3. Building blocks (equations)

Let $x[n]$ be the input series (e.g., price), and define the intermediate/output sequences as below.

### 3.1 One‑pole high‑pass (1P‑HP)

Design (with $\Lambda$ in bars):
$$
\omega_{\mathrm{HP}}=\frac{2\pi}{\Lambda},\qquad
\alpha=\frac{\cos\omega_{\mathrm{HP}}+\sin\omega_{\mathrm{HP}}-1}{\cos\omega_{\mathrm{HP}}}\,.
$$

Difference equation:
$$
\mathrm{HP}[n]=(1-\tfrac{\alpha}{2})\big(x[n]-x[n-1]\big)\;+\;(1-\alpha)\,\mathrm{HP}[n-1].
$$

(Initialization: $\mathrm{HP}[0]=0$; start from $n\ge 1$.)

Degrees form (as in Listings 7‑1, 7‑2): replace $\cos,\sin$ by $\cosd,\sind$ and set $\omega_{\mathrm{HP}} \leftarrow 360^\circ/\Lambda$.

### 3.2 Two‑pole high‑pass (2P‑HP)

Ehlers implements an equivalent two‑pole HP with a $\sqrt{2}$ pre‑warping:

Design (with $\Lambda$ in bars):
$$
\omega_{2\mathrm{P}}=\frac{\sqrt{2}\,\pi}{\Lambda}\quad\Big(\text{equivalently }0.707\times\frac{360^\circ}{\Lambda}\text{ in degrees}\Big),\qquad
\alpha=\frac{\cos\omega_{2\mathrm{P}}+\sin\omega_{2\mathrm{P}}-1}{\cos\omega_{2\mathrm{P}}}.
$$

Let $\beta=1-\alpha/2$ and $\delta=1-\alpha$. The recursion is
$$
\mathrm{HP}[n]=\beta^2\big(x[n]-2x[n-1]+x[n-2]\big)\;+\;2\delta\,\mathrm{HP}[n-1]\;-\;\delta^2\,\mathrm{HP}[n-2].
$$

(Initialization: $\mathrm{HP}[0]=\mathrm{HP}[1]=0$.)

### 3.3 Super Smoother (2nd‑order low‑pass)

Design (with $\lambda$ in bars):
$$
\omega_{\mathrm{LP}}=\frac{\sqrt{2}\,\pi}{\lambda},\qquad
a=\exp(-\omega_{\mathrm{LP}}),\qquad
b=2a\cos\omega_{\mathrm{LP}}.
$$

Coefficients:
$$
c_1=1-b+a^2,\qquad c_2=b,\qquad c_3=-a^2.
$$

Zero‑lag form (Ehlers Eq. 3‑3 applied to the HP output):
$$
\mathrm{SS}[n]=c_1\,\frac{\mathrm{HP}[n]+\mathrm{HP}[n-1]}{2}\;+\;c_2\,\mathrm{SS}[n-1]\;+\;c_3\,\mathrm{SS}[n-2].
$$

(Initialization: $\mathrm{SS}[0]=\mathrm{SS}[1]=0$.)

---

## 4. Roofing filter flavors on the pages

Let $\Lambda$ denote the HP period and $\lambda$ the LP (smoothing) period.

### 4.1 Listing 7‑1: HP‑LP roofing filter (1P‑HP $\to$ Super Smoother)

Equations:
$$
\begin{aligned}
&\text{Compute } \alpha(\Lambda),\ \mathrm{HP}[n] \text{ via §3.1}.\\
&\text{Compute } \mathrm{SS}[n] \text{ via §3.3 with } \lambda.\\
&\boxed{\ \mathrm{Filt}[n]=\mathrm{SS}[n]\ }.
\end{aligned}
$$

Comment: This band‑pass often exhibits a non‑zero mean in trending markets because financial time series tend to have amplitude increasing roughly like $1/f^{\alpha}$ (spectral dilation). A single high‑pass removes only $\approx6$ dB/oct of long‑period energy.

### 4.2 Listing 7‑2: Zero‑mean roofing filter (1P‑HP $\to$ Super Smoother $\to$ 1P‑HP)

Equations:
$$
\begin{aligned}
&\mathrm{Filt}[n]=\mathrm{SS}[n]\quad\text{(as in §4.1)};\\
&\mathrm{ZM}[n]=(1-\tfrac{\alpha}{2})\big(\mathrm{Filt}[n]-\mathrm{Filt}[n-1]\big)+(1-\alpha)\,\mathrm{ZM}[n-1].
\end{aligned}
$$

Outputs:
- $\mathrm{Filt}[n]$ (dashed in Fig. 7.2) is the plain HP$\to$LP result.
- $\boxed{\ \mathrm{ZM}[n]\ }$ (solid in Fig. 7.2) is the “zero‑mean” roofing filter with an added HP that contributes another $\approx6$ dB/oct long‑period suppression.

### 4.3 Listing 7‑3: Roofing filter indicator (2P‑HP $\to$ Super Smoother)

Equations:
$$
\begin{aligned}
&\text{Compute } \mathrm{HP}[n] \text{ via §3.2 with } \Lambda,\\
&\text{then } \mathrm{SS}[n] \text{ via §3.3 with } \lambda,\\
&\boxed{\ \mathrm{Filt}[n]=\mathrm{SS}[n]\ }.
\end{aligned}
$$

Comment: The 2P‑HP’s $\approx12$ dB/oct suppression of long periods yields a near zero‑mean passband without adding a second 1P‑HP stage.

---

## 5. Parameter definitions and units

- $\Lambda$ (HPPeriod, bars): upper critical period. Rejects periods $>\Lambda$.
- $\lambda$ (LPPeriod, bars): lower critical period. Rejects periods $<\lambda$.
- Angular frequencies:
  - 1P‑HP: $\omega_{\mathrm{HP}}=2\pi/\Lambda$.
  - 2P‑HP: $\omega_{2\mathrm{P}}=\sqrt{2}\,\pi/\Lambda$ (book: $0.707\times 360^\circ/\Lambda$).
  - Super Smoother: $\omega_{\mathrm{LP}}=\sqrt{2}\,\pi/\lambda$.

Use consistent units (radians or degrees) end‑to‑end.

---

## 6. Code listings (from the pages) and C‑like pseudocode

### 6.1 Listing 7‑1: HP‑LP Roofing Filter

EasyLanguage (from the page):
```pascal
Vars:
  alpha1(0),
  HP(0),
  a1(0),
  b1(0),
  c1(0),
  c2(0),
  c3(0),
  Filt(0);

// Highpass (Λ = 48 in this example)
alpha1 = (Cosine(360 / 48) + Sine(360 / 48) - 1) / Cosine(360 / 48);
HP = (1 - alpha1 / 2)*(Close - Close[1]) + (1 - alpha1)*HP[1];

// Super Smoother (λ = 10)
a1 = expvalue(-1.414*3.14159 / 10);
b1 = 2*a1*Cosine(1.414*180 / 10);
c2 = b1;
c3 = -a1*a1;
c1 = 1 - c2 - c3;

Filt = c1*(HP + HP[1]) / 2 + c2*Filt[1] + c3*Filt[2];
```

C‑like pseudocode:
```c
// Inputs: double Lambda = 48; double lambda = 10;
double alpha = (cos(2*M_PI/Lambda) + sin(2*M_PI/Lambda) - 1.0) / cos(2*M_PI/Lambda);

for (n = 1; n < N; ++n) {
    HP[n] = (1 - alpha/2.0) * (x[n] - x[n-1]) + (1 - alpha) * HP[n-1];
}

double a = exp(-sqrt(2.0)*M_PI/lambda);
double b = 2*a*cos(sqrt(2.0)*M_PI/lambda);
double c1 = 1 - b + a*a, c2 = b, c3 = -a*a;

for (n = 2; n < N; ++n) {
    Filt[n] = c1 * (HP[n] + HP[n-1]) / 2.0 + c2 * Filt[n-1] + c3 * Filt[n-2];
}
```

### 6.2 Listing 7‑2: Zero‑Mean Roofing Filter

EasyLanguage (from the page):
```pascal
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
double alpha = (cos(2*M_PI/Lambda) + sin(2*M_PI/Lambda) - 1.0) / cos(2*M_PI/Lambda);

for (n = 1; n < N; ++n) {
    HP[n] = (1 - alpha/2.0) * (x[n] - x[n-1]) + (1 - alpha) * HP[n-1];
}

double a = exp(-sqrt(2.0)*M_PI/lambda);
double b = 2*a*cos(sqrt(2.0)*M_PI/lambda);
double c1 = 1 - b + a*a, c2 = b, c3 = -a*a;

for (n = 2; n < N; ++n) {
    Filt[n] = c1 * (HP[n] + HP[n-1]) / 2.0 + c2 * Filt[n-1] + c3 * Filt[n-2];
    ZM[n]   = (1 - alpha/2.0) * (Filt[n] - Filt[n-1]) + (1 - alpha) * ZM[n-1];
}
// ZM is the zero‑mean roofing filter.
```

### 6.3 Listing 7‑3: Roofing Filter Indicator (2P‑HP)

EasyLanguage (from the page; core equations):
```pascal
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
double alpha = (cos(M_PI*sqrt(2.0)/Lambda) + sin(M_PI*sqrt(2.0)/Lambda) - 1.0) / cos(M_PI*sqrt(2.0)/Lambda);

for (n = 2; n < N; ++n) {
    double beta = 1 - alpha/2.0, delta = 1 - alpha;
    HP[n] = (beta*beta) * (x[n] - 2*x[n-1] + x[n-2])
          + 2*delta * HP[n-1]
          - (delta*delta) * HP[n-2];
}

double a = exp(-sqrt(2.0)*M_PI/lambda);
double b = 2*a*cos(sqrt(2.0)*M_PI/lambda);
double c1 = 1 - b + a*a, c2 = b, c3 = -a*a;

for (n = 2; n < N; ++n) {
    Filt[n] = c1 * (HP[n] + HP[n-1]) / 2.0 + c2 * Filt[n-1] + c3 * Filt[n-2];
}
```

---

## 7. Practical implementation notes

- Initialization and priming:
  - 1P‑HP needs past $x[n-1]$; 2P‑HP and the Super Smoother need $x[n-1],x[n-2]$ and past outputs. Initialize to zero and treat the first few outputs as warm‑up.
- Numerical stability:
  - With $\lambda>0$, $a=\exp(-\omega_{\mathrm{LP}})\in(0,1)$, giving a stable 2nd‑order LP. For typical $\lambda\gtrsim 5$, coefficients are well‑conditioned.
- Parameter choice:
  - $\Lambda$ sets how much “trend” is removed; larger $\Lambda$ preserves longer cycles.
  - $\lambda$ sets how much high‑frequency noise is removed; smaller $\lambda$ removes more wiggles.
- Zero mean:
  - 1P‑HP $\to$ SS (Listing 7‑1) often exhibits non‑zero mean in trends due to spectral dilation (amplitude $\sim 1/f^{\alpha}$). Enforce near zero‑mean either by adding the final 1P‑HP (Listing 7‑2) or by using the 2P‑HP front‑end (Listing 7‑3).
- Simple timing signal (from the text around Listing 7‑3):
  - Cross $\mathrm{Filt}[n]$ with $\mathrm{Filt}[n-2]$ to flag peaks/troughs.

---

## 8. Block‑diagram illustrations (SVG)

### 8.1 Listing 7‑1: 1P‑HP $\to$ Super Smoother
<svg width="560" height="90" xmlns="http://www.w3.org/2000/svg">
  <rect x="20" y="20" width="140" height="50" fill="#eef" stroke="#334" />
  <text x="90" y="50" font-size="14" text-anchor="middle" dominant-baseline="middle">1P High‑Pass (Λ)</text>
  <line x1="0" y1="45" x2="20" y2="45" stroke="#000"/>
  <line x1="160" y1="45" x2="200" y2="45" stroke="#000"/>
  <rect x="200" y="20" width="170" height="50" fill="#efe" stroke="#334" />
  <text x="285" y="50" font-size="14" text-anchor="middle" dominant-baseline="middle">Super Smoother (λ)</text>
  <line x1="370" y1="45" x2="560" y2="45" stroke="#000"/>
</svg>

### 8.2 Listing 7‑2: 1P‑HP $\to$ Super Smoother $\to$ 1P‑HP
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

### 8.3 Listing 7‑3: 2P‑HP $\to$ Super Smoother
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
$$
\omega_{\mathrm{HP}}=\frac{2\pi}{\Lambda},\quad
\alpha=\frac{\cos\omega_{\mathrm{HP}}+\sin\omega_{\mathrm{HP}}-1}{\cos\omega_{\mathrm{HP}}},\quad
\mathrm{HP}[n]=(1-\tfrac{\alpha}{2})(x[n]-x[n-1])+(1-\alpha)\mathrm{HP}[n-1].
$$

- 2P‑HP:
$$
\omega_{2\mathrm{P}}=\frac{\sqrt{2}\pi}{\Lambda},\quad
\alpha=\frac{\cos\omega_{2\mathrm{P}}+\sin\omega_{2\mathrm{P}}-1}{\cos\omega_{2\mathrm{P}}},\quad
\mathrm{HP}[n]=(1-\tfrac{\alpha}{2})^2(x[n]-2x[n-1]+x[n-2])+2(1-\alpha)\mathrm{HP}[n-1]-(1-\alpha)^2\mathrm{HP}[n-2].
$$

- Super Smoother:
$$
\omega_{\mathrm{LP}}=\frac{\sqrt{2}\pi}{\lambda},\quad
a=e^{-\omega_{\mathrm{LP}}},\quad
b=2a\cos\omega_{\mathrm{LP}},\quad
c_1=1-b+a^2,\ c_2=b,\ c_3=-a^2,
$$
$$
\mathrm{SS}[n]=c_1\frac{\mathrm{HP}[n]+\mathrm{HP}[n-1]}{2}+c_2\mathrm{SS}[n-1]+c_3\mathrm{SS}[n-2].
$$

- Roofing filter outputs:
  - Listing 7‑1: $\ \mathrm{Filt}[n]=\mathrm{SS}[n]$.
  - Listing 7‑2: $\ \mathrm{Filt}[n]=\mathrm{SS}[n],\ \ \mathrm{ZM}[n]=(1-\tfrac{\alpha}{2})(\mathrm{Filt}[n]-\mathrm{Filt}[n-1])+(1-\alpha)\mathrm{ZM}[n-1]$.
  - Listing 7‑3: $\ \mathrm{Filt}[n]=\mathrm{SS}[n]$ with 2P‑HP front‑end.

---

## 10. Usage in indicator pipelines

- Compute $\mathrm{Filt}[n]$ (or $\mathrm{ZM}[n]$) at each bar and feed downstream indicators with it to constrain their frequency content.
- For simple cycle timing (per the book discussion), use crossings of $\mathrm{Filt}[n]$ and $\mathrm{Filt}[n-2]$ to flag peaks/troughs.
- Choose $(\Lambda,\lambda)$ for the desired passband width and responsiveness, balancing noise rejection and lag.

---

This Markdown uses inline math $...$ and display equations $$...$$ and previews correctly in VSCode and most MathJax/KaTeX renderers.