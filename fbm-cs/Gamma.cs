using System;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace Mbs.Numerics
{
    /// <summary>
    /// Contains assorted special functions.
    /// </summary>
    public static partial class SpecialFunctions
    {
        /// <summary>
        /// A double value indicating the largest argument for the gamma function that will not result in an overflow.
        /// </summary>
        public const double MaxGammaArgument = 171.6243769563027257;

        private const int BernoulliLen = 8;
        private const int LanczosDlen = 11;
        private const double LanczosR = 10.900511;

        private static readonly double[] Bernoulli =
        {
            1d / 6,
            -1d / 30,
            1d / 42,
            -1d / 30,
            5d / 66,
            -691d / 2730,
            7d / 6,
            -3617d / 510,
        };

        private static readonly double[] LanczosCoefficients =
        {
            2.5066282746363426, 43.876640844001564, -29.441488119961249, 2.617298932416928, -0.000840299612359108,
            0.00042715899834181971, -0.00023054552812795247, 8.4040594453044693E-05, -1.4981925182237168E-05,
        };

        private static readonly double[] LanczosCoefficientsLarge =
        {
            2.5066282746313515, 347.05125593395849, -461.89006878944838, 168.64100065122091,
            -15.57186206508017, 0.14115928520728782, 1.2869686141588626E-05, -3.2349738354906731E-06,
        };

        private static readonly double[] PsiChebyshevSeriesApproximation =
        {
            -0.038057080835217923, 0.49141539302938714,  -0.056815747821244732, 0.008357821225914313,
            -0.001333232857994342, 0.000220313287069308, -3.7040238178456E-05,  6.283793654854E-06,
            -1.071263908506E-06,   1.83128394654E-07,    -3.1353509361E-08,     5.372808776E-09,
            -9.21168141E-10,       1.57981265E-10,       -2.7098646E-11,        4.648722E-12,
            -7.97527E-13,          1.36827E-13,          -2.3475E-14,           4.027E-15,
            -6.91E-16,             1.18E-16,             -2E-17,
        };

        private static readonly double[] PsiChebyshevSeriesApproximation2 =
        {
            -0.0204749044678185, -0.0101801271534859, 5.59718725387E-05, -1.291717657E-06,
            5.72858606E-08,      -3.8213539E-09,      3.397434E-10,      -3.74838E-11,
            4.899E-12,           -7.344E-13,          1.233E-13,         -2.28E-14,
            4.5E-15,             -9E-16,              2E-16,
        };

        private static readonly double[] PsiValues =
        {
            double.PositiveInfinity, 0.57721566490153287, 0.42278433509846713, 0.92278433509846713,
            1.2561176684318005,      1.5061176684318005,  1.7061176684318005,  1.8727843350984672,
            2.01564147795561,        2.14064147795561,    2.2517525890667209,  2.351752589066721,
            2.4426616799758119,      2.5259950133091453,  2.6029180902322224,  2.6743466616607936,
            2.7410133283274605,      2.8035133283274605,  2.862336857739225,   2.9178924132947808,
            2.9705239922421489,      3.0205239922421492,  3.0681430398611966,  3.113597585315742,
            3.1570758461853075,      3.198742512851974,   3.238742512851974,   3.2772040513135123,
            3.3142410883505495,      3.3499553740648351,  3.3844381326855251,  3.4177714660188583,
            3.4500295305349873,      3.4812795305349873,  3.5115825608380176,  3.5409943255439,
            3.5695657541153283,      3.5973435318931064,  3.6243705589201332,  3.6506863483938177,
            3.6763273740348432,      3.7013273740348431,  3.7257176179372822,  3.7495271417468059,
            3.7727829557002943,      3.7955102284275672,  3.8177324506497894,  3.8394715810845721,
            3.8607481768292526,      3.8815815101625861,  3.901989673427892,   3.9219896734278921,
            3.9415975165651469,      3.9608282857959165,  3.9796962103242182,  3.9982147288427368,
            4.0163965470245548,      4.0342536898816981,  4.0517975495308205,  4.0690389288411657,
            4.0859880813835385,      4.1026547480502051,  4.1190481906731558,  4.1351772229312207,
            4.1510502388042365,      4.1666752388042365,  4.1820598541888518,  4.1972113693403665,
            4.2121367424746952,      4.2268426248276363,  4.2413353784508248,  4.255621092736539,
            4.269705599778792,       4.2835944886676813,  4.2972931188046672,  4.3108066323181813,
            4.3241399656515149,      4.3372978603883565,  4.35028487337537,    4.3631053861958824,
            4.3757636140439837,      4.3882636140439839,  4.40060929305633,    4.4128044150075487,
            4.4248526077786332,      4.4367573696833951,  4.448522075565748,   4.4601499825424922,
            4.471644235416055,       4.4830078717796917,  4.4942438268358718,  4.5053549379469828,
            4.5163439489359938,      4.5272135141533854,  4.5379662023254284,  4.5486045001977686,
            4.5591308159872419,      4.5695474826539089,  4.5798567610044243,  4.5900608426370777,
            4.6001618527380872,
        };

        /// <summary>
        /// These values are from Table 8.5 of Glendon Ralph Pugh's 2004 PhD thesis,
        /// available at http://web.viu.ca/pughg/phdThesis/phdThesis.pdf
        /// He claims that they "guarantee 16 digit floating point accuracy in the right-half plane".
        /// </summary>
        private static readonly double[] LanczosD =
        {
            2.48574089138753565546e-5,
            1.05142378581721974210,
            -3.45687097222016235469,
            4.51227709466894823700,
            -2.98285225323576655721,
            1.05639711577126713077,
            -1.95428773191645869583e-1,
            1.70970543404441224307e-2,
            -5.71926117404305781283e-4,
            4.63399473359905636708e-6,
            -2.71994908488607703910e-9,
        };

        /// <summary>
        /// The natural logarithm of the gamma function:
        /// <para><c>lnΓ(x) = ln∫̥˚˚ tˣ⁻¹e⁻ᵗdt</c>.</para>
        /// <para>Because <c>Γ(x)</c> grows rapidly for increasing positive <c>x</c>, it is often necessary to
        /// work with its logarithm in order to avoid overflow. This function returns accurate
        /// values of <c>lnΓ(x)</c> even for values of x which would cause <c>Γ(x)</c> to overflow.</para>
        /// </summary>
        /// <param name="x">The argument, which must be positive.</param>
        /// <returns>The natural logarithm of the gamma function <c>lnΓ(x)</c>.</returns>
        /// <seealso href="http://mathworld.wolfram.com/LogGammaFunction.html" />
        public static double LnGamma(double x)
        {
            double p, q, z;
            if (x < -34d)
            {
                q = -x;
                double w = LnGamma(q);
                p = (int)Math.Floor(q);
                z = q - p;
                if (z > 0.5)
                {
                    p += 1d;
                    z = p - q;
                }

                z = q * Math.Sin(Constants.Pi * z);
                return Constants.LnPi - Math.Log(z) - w;
            }

            if (x < 13d)
            {
                z = 1d;
                p = 0d;
                double u = x;
                while (u >= 3d)
                {
                    p -= 1d;
                    u = x + p;
                    z *= u;
                }

                while (u < 2d)
                {
                    z /= u;
                    p += 1d;
                    u = x + p;
                }

                if (z < 0d)
                {
                    z = -z;
                }

                if (Math.Abs(u - 2d) < double.Epsilon)
                {
                    return Math.Log(z);
                }

                p -= 2d;
                x += p;
                double b = -1378.25152569120859100;
                b = -38801.6315134637840924 + x * b;
                b = -331612.992738871184744 + x * b;
                b = -1162370.97492762307383 + x * b;
                b = -1721737.00820839662146 + x * b;
                b = -853555.664245765465627 + x * b;
                double c = 1;
                c = -351.815701436523470549 + x * c;
                c = -17064.2106651881159223 + x * c;
                c = -220528.590553854454839 + x * c;
                c = -1139334.44367982507207 + x * c;
                c = -2532523.07177582951285 + x * c;
                c = -2018891.41433532773231 + x * c;
                return Math.Log(z) + x * b / c;
            }

            q = (x - 0.5) * Math.Log(x) - x + 0.91893853320467274178;
            if (x > 100000000d)
            {
                return q;
            }

            p = 1d / (x * x);
            if (x >= 1000d)
            {
                q += ((7.9365079365079365079365 * 0.0001 * p - 2.7777777777777777777778 * 0.001) * p + 0.0833333333333333333333) / x;
            }
            else
            {
                double a = 8.11614167470508450300 * 0.0001;
                a = -(5.95061904284301438324 * 0.0001) + p * a;
                a = 7.93650340457716943945 * 0.0001 + p * a;
                a = -(2.77777777730099687205 * 0.001) + p * a;
                a = 8.33333333333331927722 * 0.01 + p * a;
                q += a / x;
            }

            return q;
        }

        /// <summary>
        /// The natural logarithm of the complex gamma function, <c>lnΓ(x)</c>.
        /// </summary>
        /// <param name="z">The complex argument, which must have a non-negative real part.</param>
        /// <returns>The complex value <c>lnΓ(x)</c>.</returns>
        /// <seealso href="http://mathworld.wolfram.com/LogGammaFunction.html" />
        public static Complex LnGamma(Complex z)
        {
            bool flag = false;
            if (z.Real < 0d)
            {
                z = 1d - z;
                flag = true;
            }

            z -= 1d;
            double num;
            double[] lanczos;
            if (z.Real > 50d)
            {
                num = 5.5407;
                lanczos = LanczosCoefficientsLarge;
            }
            else
            {
                num = 3.6998328;
                lanczos = LanczosCoefficients;
            }

            int length = lanczos.Length;
            Complex c = lanczos[0];
            for (int i = 1; i < length; ++i)
            {
                c += lanczos[i] / (z + i);
            }

            Complex p = z + 0.5;
            Complex q = p + num;
            c = p * Complex.Log(q) - q + Complex.Log(c);
            if (flag)
            {
                c = Complex.Log(Constants.Pi / Complex.Sin(Constants.Pi * (1d - z))) - c - new Complex(0d, Constants.Pi);
            }

            return c - new Complex(0d, Math.Floor(c.Imag / Constants.TwoPi + 0.5) * Constants.TwoPi);
        }

        /// <summary>
        /// The sign of the gamma function:
        /// <para><c>Γ(x) = ∫̥˚˚ tˣ⁻¹e⁻ᵗdt</c>.</para>
        /// </summary>
        /// <param name="x">A real number.</param>
        /// <returns>The sign of the Gamma function at <c>x</c>.</returns>
        public static int GammaSign(double x)
        {
            if (x > 0d)
            {
                return 1;
            }

            return (int)Math.Floor(x) % 2 == 0 ? 1 : -1;
        }

        /// <summary>
        /// The gamma function:
        /// <para><c>Γ(x) = ∫̥˚˚ tˣ⁻¹e⁻ᵗdt</c>.</para>
        /// <para>The gamma function is a generalization of the factorial to arbitrary real values.</para>
        /// <para>For positive integer arguments, this integral evaluates to <c>Γ(n+1) = n!</c>, but it can also be evaluated for non-integer z.</para>
        /// <para>Because <c>Γ(x)</c> grows beyond the largest value that can be represented by a <see cref="double" /> at quite
        /// moderate values of <c>x</c>, it may be useful to work with the <c>ln Γ(x)</c>.</para>
        /// </summary>
        /// <param name="x">The argument.</param>
        /// <returns>The value of <c>Γ(x)</c>.</returns>
        /// <seealso href="http://en.wikipedia.org/wiki/Gamma_function" />
        /// <seealso href="http://mathworld.wolfram.com/GammaFunction.html" />
        public static double Gamma(double x)
        {
            if (double.IsNaN(x))
            {
                return x;
            }

            double g = Math.Round(x);
            if (x <= 0d && Math.Abs(x - g) < double.Epsilon)
            {
                return double.NaN;
            }

            if (Math.Abs(x) > 10d)
            {
                double num = Math.Exp(LnGamma(x));
                return (x < 0d && (((int)Math.Floor(x)) % 2) != 0) ? -num : num;
            }

            if (Math.Abs(x) < Constants.NormalizedDoubleMin)
            {
                return x < 0d ? double.NegativeInfinity : double.PositiveInfinity;
            }

            int q;
            double p = x - g;
            if (Math.Abs(p) <= 0.1)
            {
                if (Math.Abs(p) < double.Epsilon)
                {
                    return Factorial((int)x - 1);
                }

                q = (int)Math.Round(x) - 1;
                g = (1d + p * (0.43034126526705851 + p * (0.22266730777422281 + p * (0.040851751767329046 + p * 0.011565315597477564)))) / (1d + p * (1.0075569301685881 - p * (0.18481104418026204 + p * (0.15487522431232129 - p * 0.0375661264693936))));
            }
            else
            {
                p = Math.Floor(x);
                q = (int)p - 1;
                p = x - p;
                g = 1d / (1d + p) + (-1.3782221508780498E-14 + p * (0.42278433509901897 + p * (0.041374380109200512 - p * (0.0061763349932353441 - p * (0.0052668405087916989 - p * 7.82874302978088e-05))))) / (1d + p * (0.12374721683674332 - p * (0.23024261024083342 - p * (0.022635112892944842 + p * (0.01354793760588476 - p * (0.0036190944658071257 - p * 0.00027330395814104293))))));
            }

            ++p;
            while (q > 0)
            {
                g *= p;
                ++p;
                --q;
            }

            while (q < 0)
            {
                g /= x;
                ++x;
                ++q;
            }

            return g;
        }

        /// <summary>
        /// The complex gamma function, <c>Γ(z)</c>.
        /// </summary>
        /// <param name="z">The complex argument.</param>
        /// <returns>The complex value of <c>Γ(z)</c>.</returns>
        /// <seealso href="http://en.wikipedia.org/wiki/Gamma_function" />
        /// <seealso href="http://mathworld.wolfram.com/GammaFunction.html" />
        public static Complex Gamma(Complex z)
        {
            if (z.IsNaN)
            {
                return z;
            }

            if (z.Real < 0.5)
            {
                return Constants.Pi / Gamma(1d - z) / Complex.Sin(Constants.Pi * z);
            }

            return z.IsReal
                ? new Complex(Gamma(z.Real))
                : Complex.Exp(LnGamma(z));
        }

        /// <summary>
        /// The psi function, <c>ψ(x)</c>, also called the digamma function, is the logarithmic derivative of the gamma function:
        /// <para><c>ψ(x) = d/dx ln Γ(x) = Γʹ(x)/Γ(x)</c>.</para>
        /// </summary>
        /// <param name="x">The real argument.</param>
        /// <returns>The value of <c>ψ(x)</c>.</returns>
        /// <seealso href="http://en.wikipedia.org/wiki/Digamma_function" />
        /// <seealso href="http://mathworld.wolfram.com/DigammaFunction.html" />
        public static double Digamma(double x)
        {
            // Check for the poles at zero and negative integers.
            if (x <= 0d && Math.Abs(x - Math.Ceiling(x)) < double.Epsilon)
            {
                return double.NaN;
            }

            double d = Math.Abs(x);
            if (d >= 2d)
            {
                double c = x * x;
                c = d < 8192d
                    ? EvaluateChebyshevSeries(8d / c - 1d, PsiChebyshevSeriesApproximation2)
                    : -1d / (12d * c) * (1d - 1d / (10d * c));
                if (x >= 0d)
                {
                    return Math.Log(d) - 0.5 / x + c;
                }

                double cos = Constants.Pi * x;
                double sin = Math.Sin(cos);
                cos = Math.Cos(cos);
                if (Math.Abs(sin) >= 4.4501477170144025E-162)
                {
                    return Math.Log(d) - 0.5 / x + c - Constants.Pi * cos / sin;
                }

                return sin >= 0d ? double.NegativeInfinity : double.PositiveInfinity;
            }

            if (x < -1d)
            {
                return EvaluateChebyshevSeries(2d * x + 3d, PsiChebyshevSeriesApproximation) - 1d / x - 1d / (x + 1d) - 1d / (x + 2d);
            }

            if (x < 0d)
            {
                return EvaluateChebyshevSeries(2d * x + 1d, PsiChebyshevSeriesApproximation) - 1d / x - 1d / (x + 1d);
            }

            return x < 1d
                ? EvaluateChebyshevSeries(2d * x - 1d, PsiChebyshevSeriesApproximation) - 1d / x
                : EvaluateChebyshevSeries(2d * x - 3d, PsiChebyshevSeriesApproximation);
        }

        /// <summary>
        /// Evaluates the digamma function for an integer argument.
        /// <para />
        /// The psi function, <c>ψ(x)</c>, also called the digamma function, is the logarithmic derivative of the gamma function:
        /// <para />
        /// ψ(x) = d/dx ln Γ(x) = Γʹ(x)/Γ(x).
        /// </summary>
        /// <param name="n">An integer argument.</param>
        /// <returns>The value of <c>ψ(x)</c>.</returns>
        /// <seealso href="http://en.wikipedia.org/wiki/Digamma_function" />
        /// <seealso href="http://mathworld.wolfram.com/DigammaFunction.html" />
        public static double Digamma(int n)
        {
            if (n <= 0)
            {
                return double.NaN; // Or throw argument out of range exception.
            }

            if (n < PsiValues.Length)
            {
                return PsiValues[n];
            }

            double nn = n * n;
            nn = (-0.083333333333333329 + (0.0083333333333333332 + (-0.003968253968253968 + 0.0041666666666666666 / nn) / nn) / nn) / nn;
            return Math.Log(n) - 0.5 / n + nn;
        }

        /// <summary>
        /// The complex psi function, <c>ψ(z)</c>, also called the digamma function, is the logarithmic derivative of the gamma function:
        /// <para><c>ψ(z) = d/dz ln Γ(z) = Γʹ(z)/Γ(z)</c>.</para>
        /// </summary>
        /// <param name="z">The complex argument.</param>
        /// <returns>The value of <c>ψ(z)</c>.</returns>
        /// <seealso href="http://en.wikipedia.org/wiki/Digamma_function" />
        /// <seealso href="http://mathworld.wolfram.com/DigammaFunction.html" />
        public static Complex Digamma(Complex z)
        {
            if (z.Real < 0.5)
            {
                // Reduce z.Re in order to handle large real values.
                return LanczosPsi(1d - z) - Constants.Pi / Complex.Tan(Constants.Pi * z);
            }

            return LanczosPsi(z);
        }

        /// <summary>
        /// The <paramref name="n" />th Harmonic Number, Hⁿ, is the sum of the reciprocals from 1 up to <paramref name="n" />.
        /// </summary>
        /// <param name="n">The degree of the coefficient.</param>
        /// <returns>The <paramref name="n" />th Harmonic Number.</returns>
        public static double HarmonicNumber(int n)
        {
            return n >= 0x7fffffff
                ? Digamma(n + 1d) + Constants.EulerGamma
                : Digamma(n + 1) + Constants.EulerGamma;
        }

        /// <summary>
        /// The polygamma function, <c>ψ⁽ⁿ⁾(x)</c>, which gives higher logarithmic derivatives of the gamma function:
        /// <para><c>ψ⁽ⁿ⁾(x) = dⁿ/dxⁿ ψ(x) = dⁿ/dxⁿ lnΓ(x)</c>.</para>
        /// </summary>
        /// <param name="n">The order, which must be non-negative.</param>
        /// <param name="x">The argument.</param>
        /// <returns>The value of <c>ψ⁽ⁿ⁾(x)</c>.</returns>
        /// <seealso href="http://en.wikipedia.org/wiki/Polygamma_function"/>
        /// <seealso href="http://mathworld.wolfram.com/PolygammaFunction.html"/>
        public static double Polygamma(int n, double x)
        {
            if (n < 0)
            {
                return double.NaN;
            }

            // For n=0, use normal digamma algorithm.
            if (n == 0)
            {
                return Digamma(x);
            }

            // For small x, use the reflection formula.
            if (x <= 0d)
            {
                if (Math.Abs(x - Math.Ceiling(x)) < double.Epsilon)
                {
                    return double.NaN;
                }

                // Use the reflection formula.
                // This requires that we compute the nth derivative of cot(πx)
                // The algorithm is O(n²), so for large n and not-so-negative x this is probably sub-optimal.
                double y = Constants.Pi * x;
                double d = -EvaluateCotDerivative(ComputeCotDerivative(n), y) * ElementaryFunctions.Pow(Constants.Pi, n + 1);
                return n % 2 == 0
                    ? d + Polygamma(n, 1d - x)
                    : d - Polygamma(n, 1d - x);
            }

            // Compute the minimum x required for our asymptotic series to converge
            // The original approximation was to say that, for 1 << k << n, the factorials approach e²ᵏ,
            // so to achieve convergence to 10⁻¹⁶ at the 8'th term we should need x ~ 10 e ~ 27.
            // Unfortunately this estimate is both crude and an underestimate; for now i use an empirical relationship instead.
            double xm = 16d + 2 * n;

            // By repeatedly using ψ(n,x) = ψ(n,x+1) - (-1)ⁿ n! / xⁿ⁺¹,
            // increase x until it is large enough to use the asymptotic series.
            // Keep track of the accumulated shifts in the variable s.
            double s = 0d;
            while (x < xm)
            {
                s += 1d / ElementaryFunctions.Pow(x, n + 1);
                x += 1d;
            }

            // Now that x is big enough, use the asymptotic series
            // ψ(n,x) = - (-1)ⁿ (n-1)! / xⁿ ( 1 + n/2 x + ∑₁˚˚ (2i+n-1)!/[(2k)!(n-1)!Β₂ᵢx²ⁱ]).
            double t = 1d + n / (2d * x);
            double x2 = x * x;
            double t1 = n * (n + 1) / 2d / x2;
            for (int i = 1; i < BernoulliLen; ++i)
            {
                double tPrev = t;
                t += Bernoulli[i] * t1;
                if (Math.Abs(t - tPrev) < double.Epsilon)
                {
                    double g = Factorial(n - 1) * (t / ElementaryFunctions.Pow(x, n) + n * s);
                    if (n % 2 == 0)
                    {
                        g = -g;
                    }

                    return g;
                }

                int i2 = 2 * i;
                t1 *= 1d * (n + i2) * (n + i2 + 1) / (i2 + 2) / (i2 + 1) / x2;
            }

            double q = Factorial(n - 1) * (t / ElementaryFunctions.Pow(x, n) + n * s);
            if (n % 2 == 0)
            {
                q = -q;
            }

            return q;
        }

        /// <summary>
        /// The normalized (regularized) lower (left) incomplete gamma function:
        /// <para><c>P(a,x) = γ(a,x)/Γ(x), γ(a,x) = ∫̽˳ tᵃ⁻¹e⁻ᵗdt</c>.</para>
        /// <para>The lower incomplete gamma function is obtained by carrying out the gamma function integration from zero to some
        /// finite value <c>x</c>, instead of to infinity. The function is normalized by dividing by the complete integral, so the
        /// function ranges from 0 to 1 as <c>x</c> ranges from 0 to infinity.</para>
        /// <para>This function changes rapidly from 0 to 1 around the point <c>x=a</c>. For large values of <c>x</c>, this function becomes 1 within floating point precision. To determine its deviation from 1
        /// in this region, use the complementary function <c>Q(a,x) = 1 - P(a,x)</c> (<see cref="RegularizedGammaQ"/>).</para>
        /// <para>For <c>a=ν/2</c> and <c>x=χ²/2</c>, this function is the CDF of the <c>χ²</c> distribution with <c>ν</c> degrees of freedom.</para>
        /// </summary>
        /// <param name="a">The shape parameter, which must be positive.</param>
        /// <param name="x">The argument, which must be non-negative.</param>
        /// <returns>The value of <c>γ(a,x)/Γ(x)</c>.</returns>
        public static double RegularizedGammaP(double a, double x)
        {
            if (a <= 0d || x < 0d)
            {
                return double.NaN;
            }

            return x <= 0.5 * a
                ? IncompleteGammaPSeries(a, x)
                : 1d - RegularizedGammaQ(a, x);
        }

        /// <summary>
        /// The normalized (regularized) upper (right) incomplete gamma function:
        /// <para><c>Q(a,x) = Γ(a,x)/Γ(x), Γ(a,x) = ∫̊ₓ̊ tᵃ⁻¹e⁻ᵗdt</c>.</para>
        /// <para>The upper incomplete gamma function is obtained by carrying out the gamma function integration from a finite value <c>x</c>
        /// to infinity. The function is normalized by dividing by the complete integral, so the
        /// function ranges from 1 to 0 as <c>x</c> ranges from 0 to infinity.</para>
        /// <para>This function changes rapidly from 1 to 0 around the point <c>x=a</c>.</para>
        /// <para>This function is the complement of the lower incomplete gamma function <c>P(a,x) = 1 - Q(a,x)</c> (<see cref="RegularizedGammaP"/>).</para>
        /// </summary>
        /// <param name="a">The shape parameter, which must be positive.</param>
        /// <param name="x">The argument, which must be non-negative.</param>
        /// <returns>The value of <c>Γ(a,x)/Γ(x)</c>.</returns>
        public static double RegularizedGammaQ(double a, double x)
        {
            if (a <= 0d || x < 0d)
            {
                return double.NaN;
            }

            if (Math.Abs(x) < double.Epsilon)
            {
                return 1d;
            }

            if (x <= 0.8 * a)
            {
                return 1d - IncompleteGammaPSeries(a, x);
            }

            if (a < 0.2 && x < 10d)
            {
                return IncompleteGammaQSeries(a, x);
            }

            if (a < 0.9 * x && x >= 10d)
            {
                return IncompleteGammaqLargeX(a, x);
            }

            return a > 100d && Math.Abs(x / a - 1d) < 0.15
                ? IncompleteGammaqLargeAx(a, x)
                : DominantPart(a, x) * a * IncompleteGammaContinuedFraction(a, x);
        }

        /// <summary>
        /// The full (non-normalized / non-regularized) lower incomplete Gamma function:
        /// <para><c>γ(a,x) = ∫̽˳ tᵃ⁻¹e⁻ᵗdt</c>.</para>
        /// <para>The lower incomplete gamma function is obtained by carrying out the gamma function integration from zero to some
        /// finite value <c>x</c>, instead of to infinity. Like the gamma function itself, this function gets large very quickly. For most
        /// purposes, you will prefer to use the regularized incomplete gamma functions <c>Q(a,x) = Γ(a,x)/Γ(x)</c> (<see cref="RegularizedGammaQ"/>)
        /// and <c>P(a,x) = γ(a,x)/Γ(x)</c> (<see cref="RegularizedGammaP"/>).</para>
        /// </summary>
        /// <param name="a">The shape parameter, which must be positive.</param>
        /// <param name="x">The argument, which must be non-negative.</param>
        /// <returns>The value of <c>γ(a,x)</c>.</returns>
        /// <seealso href="http://en.wikipedia.org/wiki/Incomplete_Gamma_function"/>
        public static double IncompleteGammaLower(double a, double x)
        {
            return RegularizedGammaP(a, x) * Gamma(a);
        }

        /// <summary>
        /// The full (non-normalized / non-regularized) upper incomplete Gamma function:
        /// <para><c>Γ(a,x) = ∫̊ₓ̊ tᵃ⁻¹e⁻ᵗdt</c>.</para>
        /// <para>The upper incomplete gamma function is obtained by carrying out the gamma function integration from finite value <c>x</c>
        /// to infinity. Like the gamma function itself, this function gets large very quickly. For most
        /// purposes, you will prefer to use the regularized incomplete gamma functions <c>Q(a,x) = Γ(a,x)/Γ(x)</c> (<see cref="RegularizedGammaQ"/>)
        /// and <c>P(a,x) = γ(a,x)/Γ(x)</c> (<see cref="RegularizedGammaP"/>).</para>
        /// </summary>
        /// <param name="a">The shape parameter, which must be positive.</param>
        /// <param name="x">The argument, which must be non-negative.</param>
        /// <returns>The value of <c>Γ(a,x)</c>.</returns>
        /// <seealso href="http://en.wikipedia.org/wiki/Incomplete_Gamma_function"/>
        public static double IncompleteGammaUpper(double a, double x)
        {
            if (x < 0d)
            {
                return double.NaN;
            }

            if (Math.Abs(x) < double.Epsilon)
            {
                return Gamma(a);
            }

            if (Math.Abs(a) < double.Epsilon)
            {
                return -ExponentialIntegral(-x);
            }

            if (a > 0d)
            {
                return Gamma(a) * RegularizedGammaQ(a, x);
            }

            if (x > 0.25)
            {
                return Math.Exp(a * Math.Log(x) - x) * IncompleteGammaContinuedFraction(a, x);
            }

            if (a > -0.5)
            {
                return Gamma(a) * IncompleteGammaQSeries(a, x);
            }

            double a1 = a - Math.Floor(a);
            double g = IncompleteGammaUpper(a1, x);
            for (double i = a1; i > a; --i)
            {
                double p = i - 1d;
                double q = Math.Exp(p * Math.Log(x) - x);
                g = (g - q) / p;
            }

            return g;
        }

        /// <summary>
        /// Evaluates the incomplete Gamma function between two arguments.
        /// </summary>
        /// <param name="a">The shape parameter, which must be positive.</param>
        /// <param name="x1">The first argument, which must be non-negative.</param>
        /// <param name="x2">The second argument, which must be non-negative.</param>
        /// <returns>The value of <c>γ(a,x1) - γ(a,x2)</c>.</returns>
        /// <seealso href="http://en.wikipedia.org/wiki/Incomplete_Gamma_function"/>
        public static double IncompleteGamma(double a, double x1, double x2)
        {
            return Gamma(a) * (RegularizedGammaQ(a, x1) - RegularizedGammaQ(a, x2));
        }

        /// <summary>
        /// The inverse of the regularized Gamma function P(a, x).
        /// </summary>
        /// <param name="a">The parameter of the Gamma function.</param>
        /// <param name="p">The value of the regularized Gamma function.</param>
        /// <returns>The value <c>x</c> for which <c>P(a,x)</c> equals <c>p</c>.</returns>
        public static double InverseRegularizedGammaP(double a, double p)
        {
            return InverseRegularizedGamma(a, p, 1d - p);
        }

        /// <summary>
        /// The inverse of the regularized Gamma function Q(a, x).
        /// </summary>
        /// <param name="a">The parameter of the Gamma function.</param>
        /// <param name="q">The value of the regularized Gamma function.</param>
        /// <returns>The value <c>x</c> for which <c>Q(a,x)</c> equals <c>p</c>.</returns>
        public static double InverseRegularizedGammaQ(double a, double q)
        {
            return InverseRegularizedGamma(a, 1d - q, q);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double[] ComputeCotDerivative(int n)
        {
            // The nth derivative of cot(x) can be obtained by noting that cot(x)=cos(x)/sin(x).
            // Define rᵢ=(c/s)ⁱ. Then by explicit differentiation Dᵪrᵢ=-i(rᵢ₋₁ + rᵢ₊₁).
            // Since the result is expressed in terms of r's, differentiation can be repeated using the same formula.
            // The next two methods simply implement this machinery.
            // Is there a way we can turn this into a recursion formula so it is O(n) instead of O(n²)?

            // Make an array to hold coefficients of 1, r, r₂, …, rᵢ₊₁.
            var p = new double[n + 2];

            // Start with one power of r.
            p[1] = 1d;

            // Differentiate n times.
            for (int i = 1; i <= n; ++i)
            {
                // Only even or odd powers of r appear at any given order; this fact allows us to use just one array:
                // the entries of one parity are the source (and are set to zero after being used), the of the other parity are the target.
                for (int j = i; j >= 0; j -= 2)
                {
                    // Add -j times our coefficient to the coefficient above.
                    p[j + 1] += -j * p[j];

                    // Same for the coefficient below; we need not add since no one else has addressed it yet (since we are moving down).
                    if (j > 0)
                    {
                        p[j - 1] = -j * p[j];
                    }

                    // We are done with this coefficient; make it zero for the next time.
                    p[j] = 0d;
                }
            }

            return p;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double EvaluateCotDerivative(double[] p, double x)
        {
            // Compute cot(x), which is what our polynomial is in powers of.
            double r = 1d / Math.Tan(x);

            // Compute its square, which we will multiply by to move ahead by powers of two as we evaluate.
            double r2 = r * r;

            // Compute the first term and set the index of the next term.
            int i;
            double rp;
            if (p.Length % 2 == 0)
            {
                i = 1;
                rp = r;
            }
            else
            {
                i = 0;
                rp = 1d;
            }

            double f = 0d;
            while (i < p.Length)
            {
                // Add the current term.
                f += p[i] * rp;

                // Prepare for next term.
                i += 2;
                rp *= r2;
            }

            return f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Complex LanczosPsi(Complex z)
        {
            // Compute the Lanczos series.
            Complex s0 = LanczosD[0];
            Complex s1 = 0d;
            for (int i = 1; i < LanczosDlen; ++i)
            {
                Complex zi = z + i;
                Complex st = LanczosD[i] / zi;
                s0 += st;
                s1 += st / zi;
            }

            // Compute the leading terms.
            Complex zz = z + LanczosR + 0.5;
            Complex t = Complex.Log(zz) - LanczosR / zz - 1d / z;
            return t - s1 / s0;
        }

        /// <summary>
        /// Evaluates continued fraction for incomplete gamma function by modified Lentz’s method.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double IncompleteGammaContinuedFraction(double a, double x)
        {
            // A number near the smallest representable floating-point number.
            const double fpMin = 1.1016237598729697e-47;

            double d = 1d - a + x;
            if (Math.Abs(d) < fpMin)
            {
                d = fpMin;
            }

            double p = 0d;
            double q = d;
            int i = 1;
            double r = 0d;
            while (Math.Abs(r - 1d) > double.Epsilon)
            {
                double u = i * (a - i);
                double v = 2 * i + 1d - a + x;
                p = v + u * p;
                if (Math.Abs(p) < fpMin)
                {
                    p = fpMin;
                }

                q = v + u / q;
                if (Math.Abs(q) < fpMin)
                {
                    q = fpMin;
                }

                p = 1d / p;
                r = p * q;
                if (Math.Abs(r - 1d) < double.Epsilon)
                {
                    break;
                }

                d *= r;
                ++i;
            }

            return 1d / d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double DominantPart(double a, double x)
        {
            if (a < 100d)
            {
                return Math.Exp(a * Math.Log(x) - x - LnGamma(a + 1d));
            }

            double q = 1d / (a * a);
            return Math.Exp(a * Math.Log(x) - x - (a + 0.5) * Math.Log(a) + a * (1d + q * (-0.083333333333333329 +
                q * (0.0027777777777777779 + q * (-0.00079365079365079365 + q * -0.12440476190476191))))) / Constants.Sqrt2Pi;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double IncompleteGammaPSeries(double a, double x)
        {
            double d = 1d;
            double p = 1d;
            for (int i = 1; i < 1000; ++i)
            {
                p *= x / (a + i);
                double q = d;
                d += p;
                if (Math.Abs(d - q) < double.Epsilon)
                {
                    break;
                }
            }

            return DominantPart(a, x) * d;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double IncompleteGammaQSeries(double a, double x)
        {
            double l = Math.Log(x);
            double p = a;
            double r = 0.0;
            double q = -Constants.EulerGamma - l;
            r += q * p;
            p *= a;
            q = 0.6558780715202539 + l * (-Constants.EulerGamma + l * -0.5);
            r += q * p;
            p *= a;
            q = 0.042002635034095237 + l * (0.6558780715202539 + l * (-0.28860783245076643 + l * -0.16666666666666666));
            r += q * p;
            p *= a;
            q = -0.16653861138229148 + l * (0.042002635034095237 + l * (0.32793903576012695 + l * (-0.096202610816922149 + l * -0.041666666666666664)));
            r += q * p;
            p *= a;
            q = 0.042197734555544333 + l * (-0.16653861138229148 + l * (0.021001317517047619 + l * (0.10931301192004231 + l * (-0.024050652704230537 + l * -0.0083333333333333332))));
            r += q * p;
            p *= a;
            q = 0.009621971527876973 + l * (0.042197734555544333 + l * (-0.08326930569114574 + l * (0.0070004391723492059 + l * (0.027328252980010577 + l * (-0.0048101305408461068 + l * -0.0013888888888888889)))));
            r += q * p;
            p *= a;
            q = -0.0072189432466631 + l * (0.009621971527876973 + l * (0.021098867277772167 + l * (-0.027756435230381914 + l * (0.0017501097930873015 + l * (0.0054656505960021156 + l * (-0.00080168842347435123 + l * -0.00019841269841269841))))));
            r += q * p;
            p *= a;
            q = 0.0011651675918590652 + l * (-0.0072189432466631 + l * (0.0048109857639384865 + l * (0.0070329557592573892 + l * (-0.0069391088075954786 + l * (0.00035002195861746032 + l * (0.00091094176600035263 + l * (-0.00011452691763919303 + l * -2.48015873015873E-05)))))));
            r += q * p;
            p *= a;
            q = 0.00021524167411495098 + l * (0.0011651675918590652 + l * (-0.00360947162333155 + l * (0.0016036619213128289 + l * (0.0017582389398143473 + l * (-0.0013878217615190959 + l * (5.8336993102910053E-05 + l * (0.00013013453800005036 + l * (-1.4315864704899129E-05 + l * -2.7557319223985893E-06))))))));
            r += q * p;

            double v = Math.Pow(x, a) / Gamma(a);
            double u = 1.0;
            for (int i = 1; i < 1000; ++i)
            {
                u *= -x / i;
                double d = r;
                r -= u * v / (a + i);
                if (Math.Abs(r - d) < double.Epsilon)
                {
                    return r;
                }
            }

            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double IncompleteGammaqLargeX(double a, double x)
        {
            double p = 1d;
            double q = 1d;
            double r = 1d;

            // was 2.2250738585072014e-16, is it Constants.SqrtDoubleMin?
            for (int i = 1; Math.Abs(q) > 2.2250738585072014e-16; ++i)
            {
                q *= (a - i) / x;
                if (Math.Abs(q / r) > 1d)
                {
                    break;
                }

                p += q;
                r = q;
            }

            return DominantPart(a, x) * a / x * p;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double IncompleteGammaqLargeAx(double a, double x)
        {
            double p = x / a - 1d;
            double q = Constants.Sqrt2 * Math.Sqrt(p - ElementaryFunctions.Log1PlusX(p));
            if (p < 0d)
            {
                q = -q;
            }

            double r = 0.5 * Erfc(q * Math.Sqrt(0.5 * a));
            double u = 1d / a;
            u = Constants.OneOverSqrt2Pi * Math.Sqrt(u) * Math.Exp(-0.5 * a * q * q);

            // was 2.2250738585072014e-16, is it Constants.SqrtDoubleMin?
            double d = r * 2.2250738585072014E-16 / u;
            int i = 0;
            double v = 0d;
            double s = 1d;
            p = 1d;
            while (Math.Abs(p) > d)
            {
                switch (i)
                {
                    case 0:
                        p = -0.33333333333333331 + q * (0.083333333333333329 + q * (-0.014814814814814815 + q * (0.0011574074074074073 + q * (0.00035273368606701942 + q * (-0.0001787551440329218 + q * (3.9192631785224377E-05 + q * (-2.185448510679992E-06 + q * (-1.85406221071516E-06 + q * (8.2967113409530865E-07 + q * (-1.7665952736826078E-07 + q * (6.7078535434014984E-09 + q * 1.0261809784240309E-08)))))))))));
                        break;

                    case 1:
                        p = -0.0018518518518518519 + q * (-0.003472222222222222 + q * (0.0026455026455026454 + q * (-0.00099022633744855963 + q * (0.00020576131687242798 + q * (-4.018775720164609E-07 + q * (-1.8098550334489977E-05 + q * (7.64916091608111E-06 + q * (-1.6120900894563447E-06 + q * (4.647127802807434E-09 + q * (1.3786334469157209E-07 + q * (-5.7525456035177047E-08 + q * 1.1951628599778148E-08)))))))))));
                        break;

                    case 2:
                        p = 0.0041335978835978834 + q * (-0.0026813271604938273 + q * (0.0007716049382716049 + q * (2.0093878600823047E-06 + q * (-0.0001073665322636516 + q * (5.2923448829120125E-05 + q * (-1.2760635188618728E-05 + q * (3.4235787340961378E-08 + q * (1.3721957309062934E-06 + q * (-6.2989921383800548E-07 + q * (1.4280614206064243E-07 + q * (-2.0477098421990866E-10 + q * -1.409252991086752E-08)))))))))));
                        break;

                    case 3:
                        p = 0.00064943415637860077 + q * (0.00022947209362139917 + q * (-0.0004691894943952557 + q * (0.00026772063206283885 + q * (-7.5618016718839766E-05 + q * (-2.3965051138672968E-07 + q * (1.1082654115347303E-05 + q * (-5.6749528269915965E-06 + q * (1.4230900732435883E-06 + q * (-2.7861080291528143E-11 + q * (-1.6958404091930278E-07 + q * (8.0994649053880827E-08 + q * -1.9111168485973655E-08)))))))))));
                        break;

                    case 4:
                        p = -0.00086188829091671173 + q * (0.00078403922172006662 + q * (-0.00029907248030319018 + q * (-1.4638452578843418E-06 + q * (6.6414982154651219E-05 + q * (-3.9683650471794347E-05 + q * (1.1375726970678419E-05 + q * (2.5074972262375329E-10 + q * (-1.6954149536558305E-06 + q * (8.9075075322053094E-07 + q * (-2.2929348340008049E-07 + q * (2.9567941375440492E-11 + q * 2.8865829742708783E-08)))))))))));
                        break;

                    case 5:
                        p = -0.00033679855336635813 + q * (-6.9728137583658571E-05 + q * (0.00027727532449593918 + q * (-0.00019932570516188847 + q * (6.797780477937208E-05 + q * (1.4190629206439671E-07 + q * (-1.3594048189768693E-05 + q * (8.018470256334202E-06 + q * (-2.2914811765080952E-06 + q * (-3.2524735512984538E-10 + q * (3.4652846491085265E-07 + q * (-1.8447187191171344E-07 + q * 4.8240967037894184E-08)))))))))));
                        break;

                    case 6:
                        p = 0.00053130793646399225 + q * (-0.00059216643735369393 + q * (0.0002708782096718045 + q * (7.9023532326603281E-07 + q * (-8.1539693675619691E-05 + q * (5.61168275310625E-05 + q * (-1.8329116582843375E-05 + q * (-3.0796134506033047E-09 + q * (3.4651553688036091E-06 + q * (-2.0291327396058603E-06 + q * (5.7887928631490039E-07 + q * (2.3386306738266568E-13 + q * -8.828600746330484E-08)))))))))));
                        break;

                    case 7:
                        p = 0.00034436760689237765 + q * (5.1717909082605919E-05 + q * (-0.00033493161081142234 + q * (0.00028126951547632369 + q * (-0.00010976582244684731 + q * (-1.2741009095484485E-07 + q * (2.7744451511563645E-05 + q * (-1.8263488805711332E-05 + q * (5.7876949497350525E-06 + q * (4.93875893393627E-10 + q * (-1.0595367014026043E-06 + q * (6.1667143761104078E-07 + q * -1.7562973359060463E-07)))))))))));
                        break;

                    default:
                        return double.NaN;
                }

                p *= s;
                v += p;
                ++i;
                s /= a;
            }

            return r + (u * v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double InverseRegularizedGamma(double a, double p, double q)
        {
            if (a <= 0d)
            {
                return double.NaN;
            }

            if (Math.Abs(p) < double.Epsilon)
            {
                return 0d;
            }

            if (Math.Abs(p - 1d) < double.Epsilon)
            {
                return double.PositiveInfinity;
            }

            if (Math.Abs(a - 1d) < double.Epsilon)
            {
                if (q < 0.9)
                {
                    return -Math.Log(q);
                }

                return -ElementaryFunctions.Log1PlusX(-p);
            }

            double x;
            if (a < 1d)
            {
                double u = Gamma(a + 1d);
                double d = q * u / a;
                if (d > 0.6 || (d >= 0.45 && a >= 0.3))
                {
                    double v = d * q > 1e-08 ? Math.Pow(p * u, 1d / a) : Math.Exp(-q / a - Constants.EulerGamma);
                    x = v / (1d - v / (a + 1d));
                }
                else if (a < 0.3 && d >= 0.35 && d <= 0.6)
                {
                    double v = Math.Exp(-Constants.EulerGamma - d);
                    double w = v * Math.Exp(v);
                    x = v * Math.Exp(w);
                }
                else if (d > 0.01)
                {
                    double w = -Math.Log(d);
                    double v = w - (1d - a) * Math.Log(w);
                    x = w - (1d - a) * Math.Log(v);
                    if (d >= 0.15 && (d < 0.35 || (d < 0.45 && a >= 0.3)))
                    {
                        x -= ElementaryFunctions.Log1PlusX((1d - a) / (1d + v));
                    }
                    else
                    {
                        x -= Math.Log(v * v + 2d * (3d - a) * v + (2d - a) * (3d - a)) / (v * v + (5d - a) * v + 2d);
                    }
                }
                else
                {
                    x = Eq25(a, -Math.Log(d));
                }
            }
            else
            {
                double v = q;
                bool flag = false;
                if (p < 0.5)
                {
                    v = p;
                    flag = true;
                }

                v = Constants.Sqrt2 * Math.Sqrt(-Math.Log(v));
                v -= (3.31125922108741 + v * (11.6616720288968 + v * (4.28342155967104 + v * 0.213623493715853))) / (1d + v * (6.61053765625462 + v * (6.40691597760039 + v * (1.27364489782223 + v * 0.036117081018842))));
                if (flag)
                {
                    v = -v;
                }

                double w = v * v;
                double y = Math.Sqrt(a);
                v = a + v * y + (w - 1d) / 3d + v * (w - 7d) / (36d * y) - (3d * w * w + 7d * w - 16.0) / (810d * a) +
                    v * (9d * w * w + 256d * w - 433d) / (38880d * a * y);

                if (a >= 500d && Math.Abs(1d - v / a) < 1e-06)
                {
                    return v;
                }

                if (p > 0.5)
                {
                    if (v <= 3d * a)
                    {
                        x = v;
                    }
                    else
                    {
                        w = -Math.Log(q * Gamma(a + 1d) / a);
                        y = Math.Max(2d, a * (a - 1d));
                        if (w >= Constants.Ln10 * y)
                        {
                            x = Eq25(a, w);
                        }
                        else
                        {
                            x = 1d - a;
                            double u = w - x * Math.Log(v) - ElementaryFunctions.Log1PlusX(x / (1d + v));
                            x = w - x * Math.Log(u) - ElementaryFunctions.Log1PlusX(x / (1d + u));
                        }
                    }
                }
                else
                {
                    double u = a + 1d;
                    double d = u + 1d;
                    x = v;
                    w = Math.Log(p * Gamma(u));
                    if (v < 0.15 * u)
                    {
                        x = Math.Exp((w + x) / a);
                        x = Math.Exp((w + x - Math.Log(1d + x / u)) / a);
                        x = Math.Exp((w + x - Math.Log(1d + x / u * (1d + x / d))) / a);
                        x = Math.Exp((w + x - Math.Log(1d + x / u * (1d + x / d * (1d + x / (d + 1d))))) / a);
                    }

                    if (0.01 * u < x && x <= 0.7 * u)
                    {
                        u = 0d;
                        y = 1d;
                        for (int i = 1; y > 0.0001; ++i)
                        {
                            y *= x / (a + i);
                            u += y;
                        }

                        x = Math.Exp(((w + x) - ElementaryFunctions.Log1PlusX(u)) / a);
                        x *= 1d - (a * Math.Log(x) - x - w + ElementaryFunctions.Log1PlusX(u)) / (a - x);
                    }
                }
            }

            const double eps = 1e-10;
            double r;
            do
            {
                double u;
                do
                {
                    r = p < 0.5
                        ? RegularizedGammaP(a, x) - p
                        : q - RegularizedGammaQ(a, x);
                    double v = r / Rcomp(a, x);
                    double w = 0.5 * (a - 1d - x);
                    u = v * (1d + w * v);
                    if (Math.Abs(v) <= 0.1 && Math.Abs(w * v) <= 0.1)
                    {
                        x *= 1d - u;
                        if (Math.Abs(w) > 1d && Math.Abs(w * v * v) < eps)
                        {
                            return x;
                        }
                    }
                    else
                    {
                        u = v;
                        x *= 1d - u;
                    }
                }
                while (Math.Abs(u) > eps);
            }
            while ((p >= 0.5 || Math.Abs(r) >= eps * p) && (p < 0.5 || Math.Abs(r) >= eps * q));
            return x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double Eq25(double a, double y)
        {
            double p = (a - 1d) * Math.Log(y);
            double q = (a - 1d) * (1d + p);
            double r = (a - 1d) * ((p * 0.5 + a - 2d) * p + 1.5 * a - 2.5);
            double u = (a - 1d) * (((p / 3d + 2.5 - 1.5 * a) * p + (a - 6d) * a + 7d) * p +
                ((11d * a - 46d) * a + 47d) / 6d);
            double v = -(a - 1d) * ((((((-p / 4d + (11d * a - 17d) / 6d) * p + (-3d * a + 13d) * a - 13d) * p) +
                0.5 * (((2d * a - 25d) * a + 72d) * a - 61d)) * p) + (((25d * a - 195d) * a + 477d) * a - 379d) / 12d);
            return y + p + (q + (r + (u + v / y) / y) / y) / y;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double Rcomp(double a, double x)
        {
            if (a < 20d)
            {
                return Math.Exp(a * Math.Log(x) - x) / Gamma(a);
            }

            double p = x / a;
            if (Math.Abs(p) < double.Epsilon)
            {
                return 0d;
            }

            double q = 1d / (a * a);
            double d = (((0.75 * q - 1d) * q + 3.5) * q - 105d) / (a * 1260d) - a * Rlog(p);
            return Constants.OneOverSqrt2Pi * Math.Sqrt(a) * Math.Exp(d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double Rlog(double x)
        {
            return x - 1d - Math.Log(x);
        }
    }
}
