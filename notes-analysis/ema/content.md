# EMA

[Chart: OHLCV]

An Exponential Moving Average (EMA), or Exponentially Weighted Moving Average (EWMA) in academic
parlance, is defined recursively as

$$\tag*{(1)}ema_{k}=\alpha x_{k} + (1-\alpha)ema_{k-1}$$

The parameter $\alpha$, $0\lt\alpha\lt 1$,
determines the aggressiveness of the EMA.
As $\alpha \rightarrow 0$, the last sample makes no impact, making the EMA
line appear smoother. As $\alpha \rightarrow 1$, only the last sample determines
the EMA value, so the input samples just pass through the filter.

In contrast with the [Simple Moving Average](/smaNote.route), the EMA uses all
historical samples, giving them the smaller and smaller weighting factors. This becomes obvious
if we re-write equation (1) as

$$\tag*{(2)}\begin{array}{lcl}ema_{k}&=&\sum\limits_{m=0}^{k}{\alpha(1-\alpha)^{m}x_{k-m}}\\ &=&\alpha\sum\limits_{m=0}^{k}{(1-\alpha)^{m}x_{k-m}}\end{array}$$

Sometimes, instead of $\alpha$, it is convenient to use and equivalent length
$L$ of the SMA of the same smoothness,  which is related to $\alpha$ as

$$\tag*{(3)} L=\frac{2}{\alpha}-1, \qquad \alpha=\frac{2}{L+1}$$

As $\alpha \rightarrow 0$, $L$ becomes infinite,
as $\alpha \rightarrow 1$, $L$ becomes one.
The equation (2) can be written in terms of $L$ as

$$\tag*{(4)}ema_{k}=\frac{2}{L+1}\sum\limits_{m=0}^{k}{ \left(\frac{L-1}{L+1}\right)^{m}x_{k-m}}$$

From the digital signal processing (DSP) point of view, the EMA is an infinite impulse response (IIR)
filter which applies exponentially decreasing weighting factors to the input samples.

## Step response

Two figures below demonstrate the response to the step-up and step-down data.
The transition is clearly not linear.
The step-up response doesn't overshoot and the step-down response doesn't undershoot the data.

[Chart: OHLCV]
*Step-up response.*

[Chart: OHLCV]
*Step-down response.*

The step response of the EMA can be derived analytically.
It is the output of the EMA when a Heaviside step function (Oppenheim et al., 2009, p.13)

$$\tag*{}H_{k} = \begin{cases} 0, & k \lt 0 \\ 1, & k \ge 0 \end{cases}$$

is applied as an input. Using equation (2),

$$\tag*{}ema_{k}^{step}=\alpha\sum\limits_{m=0}^{k-1}{(1-\alpha)^{m}H_{k-m}}$$

$$\tag*{(5)}=\alpha\sum\limits_{m=0}^{k-1}{(1-\alpha)^{m}}$$

To simplify the expression above, we can recall a sum of geometric series

$$\tag*{}\sum_{m=0}^{k-1}x^m = 1 + x + x^2 + \ldots + x^{k-1}$$

Multiplying both sides by $1-x$ and simplifying

$$\tag*{}(1-x)\sum_{m=0}^{k-1}x^m = (1-x)(1 + x + x^2 + \ldots + x^{k-1})$$

$$\tag*{}= 1 + x + x^2 + \ldots + x^{k-1} - x - x^2 - \ldots - x^{k-1} - x^k$$

$$\tag*{}= 1 - x^k$$

gives a well-known identity for a sum of geometric series (Jeffrey and Dai, 2008, 1.2.2.2):

$$\tag*{}\sum_{m=0}^{k-1}x^m = \begin{cases} (1-x^k)/(1-x), & x \neq 1 \\ k, & x = 1 \end{cases}$$

Substituting the sum of geometric series into equaton (5) gives

$$\tag*{}ema_{k}^{step}=\alpha\sum\limits_{m=0}^{k-1}{(1-\alpha)^{m}}$$

$$\tag*{}=\alpha\frac{1-(1-\alpha)^k}{1-(1-\alpha)}$$

$$\tag*{(6)}=1-(1-\alpha)^{k}$$

The unit impulse response $h_{k}$ of the EMA is the output of the EMA when a Kronecker delta function

$$\tag*{}\delta_{k} = \begin{cases} 1, & k=0 \\ 0, & k \neq 0 \end{cases}$$

is applied to the input. Using the equation (2), we get

$$\tag*{}h_{k}=\alpha\sum\limits_{m=0}^{\infty}{(1-\alpha)^{m}x_{k-m}}$$

$$\tag*{}=\alpha\sum\limits_{m=0}^{\infty}{(1-\alpha)^{m}\delta_{k-m}}$$

$$\tag*{(7)}=\alpha(1-\alpha)^{k}$$

## Frequency response

The figures below show an amplitude and a phase lag of the unit sample response of the EMA as a function
of a period of various signal frequencies.

A period is a duration of a cycle in samples.
The smallest possible period of a cycle is $2$ samples.
To understand this, imagine a cycle of a sinusoid which starts at zero, goes up and peaks at $1$,
continues down and bottoms at $-1$, and then returns back to zero.
We need at least two samples (peak and trough) to represent a cycle.
See more details in the [frequency response article](/frequency-response).

[Chart: (a) amplitudePct vs period]
[Chart: (b) phaseDegUnwrapped vs period]

*An amplitude (a) and a phase lag (b) as a function of a period of a cycle.*

The same charts can be represented as a function of a cycle's frequency.
A period ($\tau$) is an inverse of the cycle's frequency
($\nu$): $\tau = \frac{1}{\nu}$.
The smallest period $\tau = 2$ corresponds to the Nyquist frequency
$\nu = \frac{1}{\tau} = \frac{1}{2}$which is the highest frequency
possible in a signal. Below we use the normalized frequency which has the value of $1$
at the Nyquist frequency.
That is, $0$ corresponds to the infinite $\tau$,
$1$ corresponds to the $\tau = 2$.

[Chart: (a) amplitudePct vs period]
[Chart: (b) amplitudePct vs frequency]
*An amplitude as a function of a period of a cycle (a) and as a function of a normalized frequency of a cycle (b) for various EMA lengths.*

[Chart: (a) phaseDeg vs period]
[Chart: (b) phaseDeg vs frequency]
*A phase as a function of a period of a wave (a) and as a function of a normalized frequency of a wave for various EMA lengths.*

The frequency response of the EMA can be derived analytically from the transfer function of the EMA.
The transfer function is defined using the unilateral Z-transform (Oppenheim et al., 2009, p.99)

$$\tag*{}Z\{x[n]\}\Leftrightarrow\sum\limits_{n=0}^{\infty}{x[n]z^{-n}}$$

where $z$ is a complex variable. If

$$\tag*{}X(z)=Z\{x[n]\}, \qquad Y(z)=Z\{y[n]\}$$

are Z-transforms of the input $x[n]$ and the output $y[n]$
respectively, then the transfer function (sometimes called system function) is defined as
(Oppenheim et al., 2009, p.131)

$$\tag*{}H(z)=\frac{Y(z)}{X(z)}$$

Applying Z-transorm to the equation (1) and using the linearity and the translation (time shift)
properties of the Z-transform translation (Oppenheim et al., 2009, pp.124-125)

$$\tag*{}Z\{y(n - m)\}\Leftrightarrow z^{-m}Y(z)$$

$$\tag*{}Z\{ax[n]+by[n]\}\Leftrightarrow aX(z)+bY(z)$$

we get:

$$\tag*{}Z\{y[n]\}=Z\{\alpha x[n]+(1-\alpha)y[n-1]\}$$

$$\tag*{}=\alpha Z\{x[n]\}+(1-\alpha)Z\{y[n-1]\}$$

$$\tag*{}=\alpha Z\{x[n]\}+(1-\alpha)z^{-1}Z\{y[n]\}$$

$$\tag*{}Y(z)=\alpha X(z)+(1-\alpha)z^{-1}Y(z)$$

$$\tag*{}Y(z)(1-(1-\alpha)z^{-1})=\alpha X(z)$$

$$\tag*{(8)}H(z)=\frac{Y(z)}{X(z)}=\frac{\alpha}{1-(1-\alpha)z^{-1}}$$

The frequency response function is the transfer function where z is located on the unit circle,
$z=exp(-i\omega)$. Here $\omega$ is an angular frequency
(in radians per sample), $\nu$ is a frequency, and $\tau$
is a period of a cycle in samples:

$$\tag*{}\omega = 2\pi\nu = \frac{2\pi}{\tau}$$

Then, the equation (8) becomes

$$\tag*{(9)}H(\omega)=\frac{\alpha}{1-(1-\alpha)exp(-i\omega)}$$

Let's calculate the squared modulus of the $H(\omega)$ using the Euler's formula
for writing the exponential as sines and cosines: $exp(ix)=\cos x+i\sin x$.

$$\tag*{}\left|H(\omega)\right|^{2}=\frac{\alpha^{2}}{\left|1-(1-\alpha)exp(-i\omega)\right|^{2}}$$

$$\tag*{}=\frac{\alpha^{2}}{\left|1-(1-\alpha)(\cos(-\omega)+i\sin(-\omega))\right|^{2}}$$

$$\tag*{}=\frac{\alpha^{2}}{(1-(1-\alpha)\cos(\omega))^{2}+((1-\alpha)\sin(\omega))^{2}}$$

$$\tag*{}=\frac{\alpha^{2}}{1-2(1-\alpha)\cos(\omega)+(1-\alpha)^{2}\cos^{2}(\omega)+(1-\alpha)^{2}\sin^{2}(\omega)}$$

$$\tag*{(10)}=\frac{\alpha^{2}}{1-2(1-\alpha)\cos(\omega)+(1-\alpha)^{2}}$$

and

$$\tag*{(11)}\left|H(\omega)\right|=\frac{\alpha}{\sqrt{1-2(1-\alpha)\cos(\omega)+(1-\alpha)^{2}}}$$

(Mak, 2006, p.15) mentions that the phase of the $H(\omega)$ can be derived as

$$\tag*{(12)}\phi(\omega)=\tan^{-1}\left( \frac{-(1-\alpha)\sin\omega}{1-(1-\alpha)\cos\omega} \right)$$

We can also express the phase lag $\phi(\omega)$ in the equation above
in terms of a lag in a number of bars (data points)

$$\tag*{(13)}bar\ lag=\frac{\phi(\omega)}{\omega}$$

I wasn't able to derive both equations (12) and (13), and (Mak, 2006, p.15) don't have the simplified
formula for the equation (13).

---

## References
Jeffrey, A., & Dai, H. H. (2008).
*Handbook of mathematical formulas and integrals*. (4th ed., p. 592). San Diego, CA: Elsevier/Academic Press.
[google books](https://books.google.com/books?id=JokQD5nK4LMC)

Mak, D. K. (2006).
*Mathematical Techniques in Financial Market Trading* (p. 320).
doi:10.1142/6055
[google books](https://books.google.com/books?id=18fICgAAQBAJ)

Oppenheim, A. V., Schafer, R. W., Yoder, M. A., & Padgett, W. T. (2009).
*Discrete-time signal processing*. (3rd ed., p. 1120). Upper Saddle River, NJ: Pearson.
[google books](https://books.google.com/books?id=EaMuAAAAQBAJ)
