# SMA

[Chart: OHLCV]

A Simple Moving Average (SMA) of length $L \ge 2$ is the arithmetic mean of the last
$L$ observations of a series $x_{1},\, x_{2},\, x_{3},\,\ldots\,,\, x_{k}$,
where $x_{k}$ is the most recent value and $k \ge L$:

$$\tag*{(1)}sma_{k}=\frac{1}{L}\sum_{m=1}^{L}x_{k-L+m}$$

When a next sample $x_{k+1}$ arrives, it will be added to the sum, and the
$x_{k-L}$ sample will be dropped from the sum. Thus, the window of $L$
samples "moves", which explains the "moving average" in its name.

When $k \lt L$, the equation (1) transforms into a cumulative moving average:

$$\tag*{}sma_{k}=\frac{1}{k}\sum_{m=1}^{k}x_{m}$$

Since the effective length $k \lt L$, we consider SMA values undefined in this case.
A scientific way to say this is "an indicator is not primed".
So, the SMA is not primed during the first $L-1$ updates.

The equation (1) can be rewritten in the recurrent form
(the $x_{k}$ is taken out of the summation, the $x_{k-L}$ is added and subtracted),
which gives an efficient method of SMA calculation:

$$\tag*{}sma_{k}=\frac{1}{L}\left[x_{k}+\sum_{m=k-L}^{k-1}x_{m}-x_{k-L}\right]$$

$$\tag*{(2)}=sma_{k-1}+\frac{x_{k}-x_{k-L}}{L}$$

From the digital signal processing (DSP) point of view, SMA is a
finite impulse response (FIR) filter with all $L$ weight coefficients equal:

$$\tag*{(3)}sma_{k}=\sum_{m=1}^{L}w_{m}x_{k-m+1},\ k\ge L,\ \forall m\ w_{m}=\frac{1}{L}$$

The filter is finite because only a finite number of $L$ last samples contribute to its value.

## Step response

Two figures below demonstrate the response of an SMA to the step-up and step-down data.
The step transition is clearly linear.
Both responses touch the step data with the lag equal to the length $L$ of the filter.
The step-up response doesn't overshoot and the step-down response doesn't undershoot the data.

[Chart: OHLCV]
*Step-up response.*

[Chart: OHLCV]
*Step-down response.*

## Frequency response

The figures below show an amplitude and a phase lag of the unit sample response of the SMA as a function
of a period of various signal frequencies.

A period is a duration of a cycle in samples.
The smallest possible period of a cycle is $2$ samples.
To understand this, imagine a cycle of a sinusoid which starts at zero, goes up and peaks at $1$,
continues down and bottoms at $-1$, and then returns back to zero.
We need at least two samples (peak and trough) to represent a cycle.
See more details in the [frequency response article](/frequency-response).

The same charts can be represented as a function of a cycle's frequency.
A period ($\tau$) is an inverse of the cycle's frequency
($\nu$): $\tau = \frac{1}{\nu}$.
The smallest period $\tau = 2$ corresponds to the Nyquist frequency
$\nu = \frac{1}{\tau} = \frac{1}{2}$which is the highest frequency
possible in a signal. Below we use the normalized frequency which has the value of $1$
at the Nyquist frequency.
That is, $0$ corresponds to the infinite $\tau$,
$1$ corresponds to the $\tau = 2$.

[Chart: (a) amplitudePct vs frequency]
[Chart: (b) phaseDegUnwrapped vs frequency]

*An amplitude (a) and a phase lag (b) as a function of a cycle.*

The shape of the amplitude response is different for the even and odd values of $L$.
For the even values of $L$, the amplitude decreases to zero
for smallest period $\tau = 2$. For the odd values of $L$,
it stays above zero.

The shape of the unwrapped frequency response is linear with respect to a normalized frequency,
and is hyperbolic with respect to a period.

This is illusrated in figures below both for the period (in samples) and the normalized frequency.

[Chart: (a) amplitudePct vs period]
[Chart: (b) amplitudePct vs period]
*An amplitude as a function of a period of a cycle for even (a) and odd (b) SMA length.*

[Chart: (a) amplitudePct vs frequency]
[Chart: (b) amplitudePct vs frequency]
*An amplitude as a function of a normalized frequency of a cycle for even (a) and odd (b) SMA length.*

[Chart: (a) phaseDegUnwrapped vs period]
[Chart: (b) phaseDegUnwrapped vs period]
*A phase as a function of a period of a wave for even (a) and odd (b) SMA length.*

[Chart: (a) phaseDegUnwrapped vs frequency]
[Chart: (b) phaseDegUnwrapped vs frequency]
*A phase as a function of a normalized frequency of a wave for even (a) and odd (b) SMA length.*

The frequency response of an SMA can be derived analytically (Mak, 2021).

The frequency response function of a discrete-time linear time-invariant (DTLTI) system
is the discrete-time Fourier transform (DTFT) of the unit impulse response $h(k)$
(Oppenheim et al., 2009):

$$\tag*{(4)}H(\omega)=\sum_{k=-\infty}^{\infty}h(k)\exp(-i k \omega)$$

where $\omega$ is an angular frequency (in radians per sample),
$\nu$ is a frequency, and $\tau$ is a period of a cycle in samples:

$$\tag*{}\omega = 2\pi\nu = \frac{2\pi}{\tau}$$

The unit impulse response is the output of the SMA when a Kronecker delta function

$$\tag*{}\delta[k] = \begin{cases} 1, & k=0 \\ 0, & k \neq 0 \end{cases}$$

is applied to the input. Since SMA has equal weight coefficients, the unit impulse response
can be obtained from equation (3):

$$\tag*{}h(k)=\sum_{m=0}^{L-1}w_m\delta[k-m]$$

$$\tag*{}=\frac{1}{l}\sum_{m=0}^{L-1}\delta[k-m]$$

$$\tag*{(5)}=\begin{cases} 1/L, & 0 \leq k \lt L \\ 0, & \text{otherwise} \end{cases}$$

Substituting (5) into equation (4) and reducing to the finite sum gives

$$\tag*{}H(\omega)=\sum_{k=0}^{L-1}h(k)\exp(-i k \omega)$$

$$\tag*{(6)}=\frac{1}{L}\sum_{k=0}^{L-1}\exp(-i k \omega)$$

To simplify (6), we can recall a sum of geometric series

$$\tag*{}\sum_{k=0}^{L-1}x^k = 1 + x + x^2 + \ldots + x^{L-1}$$

Multiplying both sides by $1-x$ and simplifying

$$\tag*{}(1-x)\sum_{k=0}^{L-1}x^k = (1-x)(1 + x + x^2 + \ldots + x^{L-1})$$

$$\tag*{}= 1 + x + x^2 + \ldots + x^{L-1} - x - x^2 - \ldots - x^{L-1} - x^L$$

$$\tag*{}= 1 - x^L$$

gives a well-known identity for a sum of geometric series (Jeffrey and Dai, 2008, 1.2.2.2):

$$\tag*{}\sum_{k=0}^{L-1}x^k = \begin{cases} (1-x^L)/(1-x), & x \neq 1 \\ L, & x = 1 \end{cases}$$

Now we can write the frequency response function as

$$\tag*{(7)}H(\omega)=\frac{1}{L}\frac{1 - \exp(-i\omega L)}{1 - \exp(-i \omega)}$$

where we have let $x = \exp(-i \omega)$.
At $\omega = 0$ equation (7) is indeterminate,
but we can use the De l'Hôpital's rule (Jeffrey and Dai, 2008, 1.15.1)

$$\tag*{}\lim_{x\to x_0}{[\frac{f(x)}{g(x)}]}=\frac{f'(x_0)}{g'(x_0)}$$

to show that

$$\tag*{}\lim_{\omega\to 0}{H(\omega)}=\frac{1}{L}\lim_{\omega\to 0}{\frac{1 - \exp(-i\omega L)}{1 - \exp(-i \omega)}}$$

$$\tag*{}\overset{\left[\frac{0}{0}\right]}{\underset{\mathrm{H}}{=}}\ \frac{1}{L}\lim_{\omega\to 0}{\frac{iL\exp(-i\omega L)}{i\exp(-i \omega)}}$$

$$\tag*{}=\frac{1}{L}\frac{iL}{i} = 1$$

The frequency response $H(\omega)$ is zero when

$$\tag*{}\exp(-i\omega L)=\cos(\omega L) - i\cdot\sin{\omega L} = 1$$

$$\tag*{}\omega L=2\pi m\quad m=1,2,3,\ldots$$

$$\tag*{}\omega=\frac{2\pi m}{L}$$

Thus, $H(\omega)$ is zero at cycle periods

$$\tag*{(8)}\tau=\frac{2\pi}{\omega}=\frac{2\pi L}{2\pi m} = L/m,\quad m=1,2,3,\ldots$$

Now let's recall the well nown formula for $\exp(i\theta)-\exp(-i\theta)$:

$$\tag*{}\exp(i\theta)-\exp(-i\theta)$$

$$\tag*{}=\cos(\theta)+i\cdot\sin(\theta)-\cos(-\theta)- i\cdot\sin(-\theta)$$

$$\tag*{}=\cos(\theta)+i\cdot\sin(\theta)-\cos(\theta)+i\cdot\sin(\theta)$$

$$\tag*{}=2i\cdot\sin(\theta)$$

Equipped with this, we can transform equation (7) into:

$$\tag*{}H(\omega)=\frac{1}{L}\frac{1 - \exp(-i\omega L)}{1 - \exp(-i \omega)}$$

$$\tag*{}=\frac{1}{L}\frac{\exp(-i\omega L/2)}{\exp(-i\omega/2)}\frac{\exp(i\omega L/2) - \exp(-i\omega L/2)}{\exp(i\omega/2) - \exp(-i\omega/2)}$$

$$\tag*{}=\frac{1}{l}\frac{\exp(-i\omega L/2)}{\exp(-i\omega/2)}\frac{2i\sin(\omega L/2)}{2i\sin(\omega /2)}$$

$$\tag*{(9)}=\frac{\exp(-i\omega\frac{L-1}{2})}{L}\frac{\sin(\omega L/2)}{\sin(\omega /2)}$$

The magnitude of the frequency response is the modulus of $H(\omega)$:

$$\tag*{}|H(\omega)|=\left|\frac{\exp(-i\omega\frac{L-1}{2})}{L}\frac{\sin(\omega L/2)}{\sin(\omega /2)}\right|$$

$$\tag*{(10)}=\frac{1}{L}\frac{|\sin(\omega L/2)|}{|\sin(\omega /2)|}$$

Here $\omega$ is an angular frequency in radians per sample.
In terms of cycle period (recall $\omega=2\pi/\tau$) this will be:

$$\tag*{(11)}|H(\tau)|=\frac{1}{L}\frac{|\sin(\pi L/\tau)|}{|\sin(\pi /\tau)|}$$

The phase (in radians) of the frequency response is the complex argument of the exponent in equation (9):

$$\tag*{(12)}\phi(\omega)=\arg(H(\omega))=-\frac{L-1}{2}\omega$$

$$\tag*{(13)}\phi(\tau)=-\pi\frac{L-1}{\tau}$$

And, finally, the number of samples lagging behind the signal is independent of angular frequency $\omega$:

$$\tag*{(14)}bar\ lag=\frac{\phi(\omega)}{\omega}=-\frac{L-1}{2}$$

---

## References
Jeffrey, A., & Dai, H. H. (2008).
*Handbook of mathematical formulas and integrals*. (4th ed., p. 592). San Diego, CA: Elsevier/Academic Press.
[google books](https://books.google.com/books?id=JokQD5nK4LMC)

Mak, D. K. (2021).
*Trading Tactics in the Financial Market: Mathematical Methods to Improve Performance* (p. ix+269).
doi:10.1007/978-3-030-70622-7
[google books](https://books.google.com/books?id=q9Q6EAAAQBAJ)

Oppenheim, A. V., Schafer, R. W., Yoder, M. A., & Padgett, W. T. (2009).
*Discrete-time signal processing*. (3rd ed., p. 1120). Upper Saddle River, NJ: Pearson.
[google books](https://books.google.com/books?id=EaMuAAAAQBAJ)
