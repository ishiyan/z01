# WMA

[Chart: OHLCV]

(a)
![](assets/wma-weights-5.svg)
(b)
![](assets/sma-weights-5.svg)

*Weighting factors of the (a) WMA and (b) SMA.*

A Weighted Moving Average (WMA) or, in academic parlance, Linear Weighted Moving Average (LWMA)
of length $L \ge 2$ is the weighted mean of the last
$L$ observations of a series $x_{1},\, x_{2},\, x_{3},\,\ldots\,,\, x_{k}$,
where $x_{k}$ is the most recent value and $k \ge L$.

It is similar to the [Simple Moving Average](/smaNote.route), but the multiplier
weighting coefficients linearly descend from $L$ at the last to $1$
at the first element of the moving average window, so the earlier samples have less impact on the WMA.

$$\tag*{(1)}wma_{k}=\frac{\sum\limits_{m=1}^{L}{m x_{k-L+m}}}{\sum\limits_{m=1}^{L}{m}}$$

The sum of the data and the weighting coefficient products is divided by the sum
of the weighting coefficients to normalize the averaging process.

The WMA has a reduced lag which results from the most recent data being the most heavily weighted.

The WMA coefficient values form a triangle across the width of the moving average window,
resulting a center of the gravity being $\frac{1}{3}$ across the window.

That is, the bar lag of the WMA is $\frac{L}{3}$.

The denominator in the equation (1) is a triangular number:

$$\tag*{(2)}\sum\limits_{m=1}^{L}{m}=1+2+\ldots+L=\frac{L(L+1)}{2}$$

This can be easily shown using the induction. Assuming that for a certain $n$

$$\tag*{}\sum\limits_{m=1}^{n}{m}=\frac{n(n+1)}{2}$$

and adding $n+1$ to the both sides gives

$$\tag*{}\sum\limits_{m=1}^{n}{m+(n+1)}=\frac{n(n+1)}{2}+n+1$$

$$\tag*{}=\frac{n(n+1)+2n+2}{2}=\frac{n^2+3n+2}{2}$$

$$\tag*{}=\frac{(n+1)(n+2)}{2}$$

Now we can substitute the summation in the denominator of the equation (1) with the triangular number:

$$\tag*{}wma_{k}=\frac{2}{L(L+1)}\sum\limits_{m=1}^{L}{m x_{k-L+m}}$$

$$\tag*{(3)}=\frac{2}{L(L+1)}q_{k},\, q_{k}=\sum\limits_{m=1}^{L}{m x_{k-L+m}}$$

We can write the difference between $q_{k+1}$ and $q_{k}$ as

$$\tag*{}q_{k+1}-q_{k}=\sum\limits_{m=1}^{L}{m x_{k+1-L+m}}-\sum\limits_{m=1}^{L}{m x_{k-L+m}}$$

$$\tag*{}=x_{k-L+2}+2x_{k-L+3}+\,\ldots\,+(L-2)x_{k-1}+(L-1)x_{k}+Lx_{k+1}$$

$$\tag*{}-x_{k-L+1}-2x_{k-L+2}-3x_{k-L+3}-\,\ldots\,-(L-1)x_{k-1}-Lx_{k}$$

$$\tag*{}=-x_{k-L+1}-x_{k-L+2}-x_{k-L+3}-\,\ldots\,-x_{k-1}-x_{k}+Lx_{k+1}$$

$$\tag*{}=Lx_{k+1}-\sum\limits_{n=k-L+1}^{k}{x_{n}}$$

Denoting

$$\tag*{(4)}s_{k}=\sum\limits_{n=k-L+1}^{k}{x_{n}}$$

we get

$$\tag*{(5)}s_{k+1}=s_{k}+x_{k+1}-x_{k-L+1}$$

$$\tag*{(6)}q_{k+1}=q_{k}+Lx_{k+1}-s_{k}$$

and

$$\tag*{(7)}wma_{k+1}=\frac{2}{L(L+1)}q_{k+1}$$

which gives an efficient formula to calculate WMA.

The WMA is not primed during the first $L-1$ updates.

From the digital signal processing (DSP) point of view, WMA is a finite impulse response (FIR)
filter with the weight coefficients

$$\tag*{(8)}w_{k}=\frac{2k}{L(L+1)},\ \forall k=1,\, 2,\,\ldots\,,\, L$$

The filter is finite because only a finite number of $L$ last samples
contribute to its value.

## Step response

The two figures below demonstrate the response to the step-up and step-down data.
The transition is clearly not linear.
Both responses touch the step data with the lag equal to the length $L$ of the filter.
The step-up response doesn't overshoot and the step-down response doesn't undershoot the data.

[Chart: OHLCV]
*Step-up response.*

[Chart: OHLCV]
*Step-down response.*

## Frequency response

The figure below shows an amplitude and a phase lag of the unit sample response of an WMA as a function
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

[Chart: (a) amplitudePct vs frequency]
[Chart: (b) phaseDegUnwrapped vs frequency]
*An amplitude (a) and a phase lag (b) as a function of a normalized frequency of a cycle.*
