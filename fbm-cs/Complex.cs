using System;
using System.Globalization;
using System.Runtime;
using System.Runtime.CompilerServices;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
namespace Mbs.Numerics
{
    /// <summary>
    /// Represents a complex number.
    /// </summary>
    public struct Complex : IComparable<Complex>, IEquatable<Complex>
    {
        /// <summary>
        /// The zero complex number value.
        /// </summary>
        public static readonly Complex Zero = new Complex(0d, 0d);

        /// <summary>
        /// The complex number one.
        /// </summary>
        public static readonly Complex One = new Complex(1d, 0d);

        /// <summary>
        /// The imaginary unit complex number.
        /// </summary>
        public static readonly Complex ImaginaryOne = new Complex(0d, 1d);

        /// <summary>
        /// Represents the complex infinity value as a complex number of infinite real and imaginary part.
        /// </summary>
        public static readonly Complex Infinity = new Complex(double.PositiveInfinity, double.PositiveInfinity);

        /// <summary>
        /// Represents a complex value that is not a number.
        /// </summary>
        public static readonly Complex NaN = new Complex(double.NaN, double.NaN);

        /// <summary>
        /// Initializes a new instance of the <see cref="Complex"/> struct.
        /// </summary>
        /// <param name="real">The real part of the complex number.</param>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public Complex(double real)
        {
            this.Real = real;
            Imag = 0d;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Complex"/> struct.
        /// </summary>
        /// <param name="real">The real part of the complex number.</param>
        /// <param name="imaginary">The imaginary part of the complex number.</param>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public Complex(double real, double imaginary)
        {
            this.Real = real;
            Imag = imaginary;
        }

        /// <summary>
        /// Gets or sets the real part of the complex number.
        /// </summary>
        public double Real
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries.")]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries.")]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// Gets or sets the imaginary part of the complex number.
        /// </summary>
        public double Imag
        {
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries.")]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
            [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries.")]
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set;
        }

        /// <summary>
        /// Gets a value indicating whether the complex number is real.
        /// </summary>
        public bool IsReal => Math.Abs(Imag) < double.Epsilon;

        /// <summary>
        /// Gets a value indicating whether the complex number is real and not negative, that is ≥ 0.
        /// </summary>
        public bool IsRealNonNegative => Math.Abs(Imag) < double.Epsilon && Real >= 0d;

        /// <summary>
        /// Gets a value indicating whether the complex number is imaginary.
        /// </summary>
        public bool IsImaginary => Math.Abs(Real) < double.Epsilon;

        /// <summary>
        /// Gets or sets the modulus of the complex number.
        /// If this complex number is zero when the modulus is set, the complex number is assumed to be positive real with an argument of zero.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if an attempt is made to set a negative modulus.
        /// </exception>
        public double Modulus
        {
            get => Math.Sqrt(Real * Real + Imag * Imag);
            set
            {
                if (value < 0d)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Value must not be negative");
                }

                if (double.IsInfinity(value))
                {
                    Real = value;
                    Imag = value;
                }
                else
                {
                    if (IsZero)
                    {
                        Real = value;
                        Imag = 0d;
                    }
                    else
                    {
                        double factor = value / Math.Sqrt(Real * Real + Imag * Imag);
                        Real *= factor;
                        Imag *= factor;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the squared modulus of the complex number.
        /// If the complex number is zero when the squared modulus is set,
        /// the complex number is assumed to be positive real with an argument of zero.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if an attempt is made to set a negative squared modulus.
        /// </exception>
        public double ModulusSquared
        {
            get => Real * Real + Imag * Imag;
            set
            {
                if (value < 0d)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), value, "Value must not be negative");
                }

                if (double.IsInfinity(value))
                {
                    Real = value;
                    Imag = value;
                }
                else
                {
                    if (IsZero)
                    {
                        Real = Math.Sqrt(value);
                        Imag = 0d;
                    }
                    else
                    {
                        double factor = value / (Real * Real + Imag * Imag);
                        Real *= factor;
                        Imag *= factor;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the argument of the complex number.
        /// Argument always returns a value within an interval (-π, π].
        /// If the complex number is zero, the complex number is assumed to be positive real with an argument of zero.
        /// </summary>
        public double Argument
        {
            get
            {
                if (Math.Abs(Imag) < double.Epsilon)
                {
                    return Real < 0d ? Constants.Pi : 0d;
                }

                return Math.Atan2(Imag, Real);
            }

            set
            {
                double modulus = Modulus;
                Real = Math.Cos(value) * modulus;
                Imag = Math.Sin(value) * modulus;
            }
        }

        /// <summary>
        /// Gets the unity of the complex number (same as the argument, but on the unit circle, eᶦᶿ).
        /// </summary>
        public Complex Sign
        {
            get
            {
                if (double.IsPositiveInfinity(Real))
                {
                    if (double.IsPositiveInfinity(Imag))
                    {
                        return new Complex(Constants.HalfSqrt2, Constants.HalfSqrt2);
                    }

                    if (double.IsNegativeInfinity(Imag))
                    {
                        return new Complex(Constants.HalfSqrt2, -Constants.HalfSqrt2);
                    }
                }
                else if (double.IsNegativeInfinity(Real))
                {
                    if (double.IsPositiveInfinity(Imag))
                    {
                        return new Complex(-Constants.HalfSqrt2, -Constants.HalfSqrt2);
                    }

                    if (double.IsNegativeInfinity(Imag))
                    {
                        return new Complex(-Constants.HalfSqrt2, Constants.HalfSqrt2);
                    }
                }

                // Don't replace this with "Modulus"!
                double mod, ar = Math.Abs(Real), ai = Math.Abs(Imag);
                if (ar > ai)
                {
                    double r = Imag / Real;
                    mod = ar * Math.Sqrt(1 + r * r);
                }
                else if (Math.Abs(Imag) > double.Epsilon)
                {
                    double r = Real / Imag;
                    mod = ai * Math.Sqrt(1 + r * r);
                }
                else
                {
                    return Zero;
                }

                return new Complex(Real / mod, Imag / mod);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the complex number is zero.
        /// </summary>
        public bool IsZero => Math.Abs(Real) < double.Epsilon && Math.Abs(Imag) < double.Epsilon;

        /// <summary>
        /// Gets a value indicating whether the complex number is one.
        /// </summary>
        public bool IsOne => Math.Abs(Real - 1d) < double.Epsilon && Math.Abs(Imag) < double.Epsilon;

        /// <summary>
        /// Gets a value indicating whether the complex number is the imaginary unit.
        /// </summary>
        public bool IsImaginaryOne => Math.Abs(Real) < double.Epsilon && Math.Abs(Imag - 1d) < double.Epsilon;

        /// <summary>
        /// Gets a value indicating whether a complex number evaluates to a value that is not a number.
        /// </summary>
        public bool IsNaN => double.IsNaN(Real) || double.IsNaN(Imag);

        /// <summary>
        /// Gets a value indicating whether this complex number evaluates to the complex infinity value or to a directed infinity value.
        /// </summary>
        public bool IsInfinity => double.IsInfinity(Real) || double.IsInfinity(Imag);

        /// <summary>
        /// Gets or sets the conjugate of the complex number.
        /// The semantic of <i>setting the conjugate</i> is such that <c>a.Conjugate = b</c> is equivalent to <c>a = b.Conjugate</c>, where <c>a</c> and <c>b</c> are the complex numbers.
        /// </summary>
        public Complex Conjugate
        {
            get => new Complex(Real, -Imag);
            set => this = value.Conjugate;
        }

        /// <summary>
        /// Implicit conversion of a real double to a complex number.
        /// </summary>
        /// <param name="number">The double number to convert.</param>
        /// <returns>The complex number.</returns>
        public static implicit operator Complex(double number) => new Complex(number, 0d);

        /// <summary>
        /// The unary addition operator.
        /// </summary>
        /// <param name="summand">A complex summand.</param>
        /// <returns>The summand.</returns>
        public static Complex operator +(Complex summand) => summand;

        /// <summary>
        /// The complex addition operator.
        /// </summary>
        /// <param name="lhs">A left-hand side complex summand.</param>
        /// <param name="rhs">A right-hand side complex summand.</param>
        /// <returns>The sum of the two specified complex numbers.</returns>
        public static Complex operator +(Complex lhs, Complex rhs) => new Complex(lhs.Real + rhs.Real, lhs.Imag + rhs.Imag);

        /// <summary>
        /// The complex addition operator.
        /// </summary>
        /// <param name="lhs">A left-hand side complex summand.</param>
        /// <param name="rhs">A right-hand side real summand.</param>
        /// <returns>The sum of the two specified complex and real numbers.</returns>
        public static Complex operator +(Complex lhs, double rhs) => new Complex(lhs.Real + rhs, lhs.Imag);

        /// <summary>
        /// The complex addition operator.
        /// </summary>
        /// <param name="lhs">A left-hand side real summand.</param>
        /// <param name="rhs">A right-hand side complex summand.</param>
        /// <returns>The sum of the two specified real and complex numbers.</returns>
        public static Complex operator +(double lhs, Complex rhs) => new Complex(lhs + rhs.Real, rhs.Imag);

        /// <summary>
        /// The unary negation operator.
        /// </summary>
        /// <param name="subtrahend">A complex subtrahend.</param>
        /// <returns>The negated complex subtrahend.</returns>
        public static Complex operator -(Complex subtrahend) => new Complex(-subtrahend.Real, -subtrahend.Imag);

        /// <summary>
        /// The complex subtraction operator.
        /// </summary>
        /// <param name="lhs">A left-hand side complex minuend.</param>
        /// <param name="rhs">A right-hand side complex subtrahend.</param>
        /// <returns>The difference of the two specified complex numbers.</returns>
        public static Complex operator -(Complex lhs, Complex rhs) => new Complex(lhs.Real - rhs.Real, lhs.Imag - rhs.Imag);

        /// <summary>
        /// The complex subtraction operator.
        /// </summary>
        /// <param name="lhs">A left-hand side complex minuend.</param>
        /// <param name="rhs">A right-hand side real subtrahend.</param>
        /// <returns>The difference of the two specified complex and real numbers.</returns>
        public static Complex operator -(Complex lhs, double rhs) => new Complex(lhs.Real - rhs, lhs.Imag);

        /// <summary>
        /// The complex subtraction operator.
        /// </summary>
        /// <param name="lhs">A left-hand side real minuend.</param>
        /// <param name="rhs">A right-hand side complex subtrahend.</param>
        /// <returns>The difference of the two specified real and complex numbers.</returns>
        public static Complex operator -(double lhs, Complex rhs) => new Complex(lhs - rhs.Real, -rhs.Imag);

        /// <summary>
        /// The complex multiplication operator.
        /// </summary>
        /// <param name="lhs">A left-hand side complex multiplicand.</param>
        /// <param name="rhs">A right-hand side complex multiplier.</param>
        /// <returns>The product of the two specified complex numbers.</returns>
        public static Complex operator *(Complex lhs, Complex rhs) => new Complex(lhs.Real * rhs.Real - lhs.Imag * rhs.Imag, lhs.Imag * rhs.Real + lhs.Real * rhs.Imag);

        /// <summary>
        /// The complex multiplication operator.
        /// </summary>
        /// <param name="lhs">A left-hand side complex multiplicand.</param>
        /// <param name="rhs">A right-hand side real multiplier.</param>
        /// <returns>The product of the two specified complex and real numbers.</returns>
        public static Complex operator *(Complex lhs, double rhs) => new Complex(rhs * lhs.Real, rhs * lhs.Imag);

        /// <summary>
        /// The complex multiplication operator.
        /// </summary>
        /// <param name="lhs">A left-hand side real multiplicand.</param>
        /// <param name="rhs">A right-hand side complex multiplier.</param>
        /// <returns>The product of the two specified real and complex numbers.</returns>
        public static Complex operator *(double lhs, Complex rhs) => new Complex(lhs * rhs.Real, lhs * rhs.Imag);

        /// <summary>
        /// The complex division operator.
        /// </summary>
        /// <param name="lhs">A left-hand side complex dividend.</param>
        /// <param name="rhs">A right-hand side complex divisor.</param>
        /// <returns>The division of the two specified complex numbers.</returns>
        public static Complex operator /(Complex lhs, Complex rhs)
        {
            if (rhs.IsZero)
            {
                return Infinity;
            }

            double z2Mod = rhs.ModulusSquared;
            return new Complex(
                (lhs.Real * rhs.Real + lhs.Imag * rhs.Imag) / z2Mod,
                (lhs.Imag * rhs.Real - lhs.Real * rhs.Imag) / z2Mod);
        }

        /// <summary>
        /// The complex division operator.
        /// </summary>
        /// <param name="lhs">A left-hand side complex dividend.</param>
        /// <param name="rhs">A right-hand side real divisor.</param>
        /// <returns>The division of the two specified complex and real numbers.</returns>
        public static Complex operator /(Complex lhs, double rhs)
        {
            if (Math.Abs(rhs) < double.Epsilon)
            {
                return Infinity;
            }

            return new Complex(lhs.Real / rhs, lhs.Imag / rhs);
        }

        /// <summary>
        /// The complex division operator.
        /// </summary>
        /// <param name="lhs">A left-hand side real dividend.</param>
        /// <param name="rhs">A right-hand side complex divisor.</param>
        /// <returns>The division of the two specified real and complex numbers.</returns>
        public static Complex operator /(double lhs, Complex rhs)
        {
            if (rhs.IsZero)
            {
                return Infinity;
            }

            double zMod = rhs.ModulusSquared;
            return new Complex(lhs * rhs.Real / zMod, -lhs * rhs.Imag / zMod);
        }

        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="lhs">A left-hand side complex number.</param>
        /// <param name="rhs">A right-hand side complex number.</param>
        /// <returns>The equality of the two specified complex numbers.</returns>
        public static bool operator ==(Complex lhs, Complex rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="lhs">A left-hand side complex number.</param>
        /// <param name="rhs">A right-hand side real number.</param>
        /// <returns>The equality of the two specified complex and real numbers.</returns>
        public static bool operator ==(Complex lhs, double rhs)
        {
            return /*!lhs.IsNaN && !double.IsNaN(rhs) &&*/ Math.Abs(lhs.Imag) < double.Epsilon && Math.Abs(rhs - lhs.Real) < double.Epsilon;
        }

        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <param name="lhs">A left-hand side real number.</param>
        /// <param name="rhs">A right-hand side complex number.</param>
        /// <returns>The equality of the two specified real and complex numbers.</returns>
        public static bool operator ==(double lhs, Complex rhs)
        {
            return /*!rhs.IsNaN && !double.IsNaN(lhs) &&*/ Math.Abs(rhs.Imag) < double.Epsilon && Math.Abs(lhs - rhs.Real) < double.Epsilon;
        }

        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="lhs">A left-hand side complex number.</param>
        /// <param name="rhs">A right-hand side complex number.</param>
        /// <returns>The inequality of the two specified complex numbers.</returns>
        public static bool operator !=(Complex lhs, Complex rhs)
        {
            return !lhs.Equals(rhs);
        }

        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="lhs">A left-hand side complex number.</param>
        /// <param name="rhs">A right-hand side real number.</param>
        /// <returns>The inequality of the two specified complex and real numbers.</returns>
        public static bool operator !=(Complex lhs, double rhs)
        {
            return /*lhs.IsNaN || double.IsNaN(rhs) ||*/ Math.Abs(lhs.Imag) > double.Epsilon || Math.Abs(rhs - lhs.Real) > double.Epsilon;
        }

        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <param name="lhs">A left-hand side real number.</param>
        /// <param name="rhs">A right-hand side complex number.</param>
        /// <returns>The inequality of the two specified real and complex numbers.</returns>
        public static bool operator !=(double lhs, Complex rhs)
        {
            return /*rhs.IsNaN || double.IsNaN(lhs) ||*/ Math.Abs(rhs.Imag) > double.Epsilon || Math.Abs(lhs - rhs.Real) > double.Epsilon;
        }

        /// <summary>
        /// The less-then operator.
        /// </summary>
        /// <param name="lhs">A left-hand side.</param>
        /// <param name="rhs">A right-hand side.</param>
        /// <returns>The boolean specifying the less-then relationship.</returns>
        public static bool operator <(Complex lhs, Complex rhs) => lhs.CompareTo(rhs) < 0;

        /// <summary>
        /// The less-or-equal-then operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA1036: Override methods on comparable types.
        /// </remarks>
        /// <param name="lhs">A left-hand side.</param>
        /// <param name="rhs">A right-hand side.</param>
        /// <returns>The boolean specifying the less-or-equal-then relationship.</returns>
        public static bool operator <=(Complex lhs, Complex rhs) => lhs.CompareTo(rhs) <= 0;

        /// <summary>
        /// The greater-then operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA1036: Override methods on comparable types.
        /// </remarks>
        /// <param name="lhs">A left-hand side.</param>
        /// <param name="rhs">A right-hand side.</param>
        /// <returns>The boolean specifying the greater-then relationship.</returns>
        public static bool operator >(Complex lhs, Complex rhs) => lhs.CompareTo(rhs) > 0;

        /// <summary>
        /// The greater-or-equal-then operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA1036: Override methods on comparable types.
        /// </remarks>
        /// <param name="lhs">A left-hand side.</param>
        /// <param name="rhs">A right-hand side.</param>
        /// <returns>The boolean specifying the greater-or-equal-then relationship.</returns>
        public static bool operator >=(Complex lhs, Complex rhs) => lhs.CompareTo(rhs) >= 0;

        /// <summary>
        /// Constructs a new instance of the complex number from its real and imaginary parts.
        /// </summary>
        /// <param name="real">The real part of the complex number.</param>
        /// <param name="imaginary">The imaginary part of the complex number.</param>
        /// <returns>The created instance of the complex number.</returns>
        public static Complex FromRealImaginary(double real, double imaginary)
        {
            return new Complex(real, imaginary);
        }

        /// <summary>
        /// Constructs a new instance of the complex number from its modulus and argument.
        /// </summary>
        /// <param name="modulus">The non-negative modulus of the complex number.</param>
        /// <param name="argument">The argument of the complex number.</param>
        /// <returns>The created instance of the complex number.</returns>
        public static Complex FromModulusArgument(double modulus, double argument)
        {
            if (modulus < 0d)
            {
                throw new ArgumentOutOfRangeException(nameof(modulus), modulus, "Value must not be negative.");
            }

            return new Complex(modulus * Math.Cos(argument), modulus * Math.Sin(argument));
        }

        /// <summary>
        /// The unary addition operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="summand">A complex summand.</param>
        /// <returns>The summand.</returns>
        public static Complex Plus(Complex summand) => summand;

        /// <summary>
        /// The complex addition operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side complex summand.</param>
        /// <param name="rhs">A right-hand side complex summand.</param>
        /// <returns>The sum of the two specified complex numbers.</returns>
        public static Complex Add(Complex lhs, Complex rhs) => lhs + rhs;

        /// <summary>
        /// The complex addition operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side complex summand.</param>
        /// <param name="rhs">A right-hand side real summand.</param>
        /// <returns>The sum of the two specified complex and real numbers.</returns>
        public static Complex Add(Complex lhs, double rhs) => lhs + rhs;

        /// <summary>
        /// The complex addition operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side real summand.</param>
        /// <param name="rhs">A right-hand side complex summand.</param>
        /// <returns>The sum of the two specified real and complex numbers.</returns>
        public static Complex Add(double lhs, Complex rhs) => lhs + rhs;

        /// <summary>
        /// The unary negation operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="subtrahend">A complex subtrahend.</param>
        /// <returns>The negated complex subtrahend.</returns>
        public static Complex Negate(Complex subtrahend) => -subtrahend;

        /// <summary>
        /// The complex subtraction operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side complex minuend.</param>
        /// <param name="rhs">A right-hand side complex subtrahend.</param>
        /// <returns>The difference of the two specified complex numbers.</returns>
        public static Complex Subtract(Complex lhs, Complex rhs) => lhs - rhs;

        /// <summary>
        /// The complex subtraction operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side complex minuend.</param>
        /// <param name="rhs">A right-hand side real subtrahend.</param>
        /// <returns>The difference of the two specified complex and real numbers.</returns>
        public static Complex Subtract(Complex lhs, double rhs) => lhs - rhs;

        /// <summary>
        /// The complex subtraction operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side real minuend.</param>
        /// <param name="rhs">A right-hand side complex subtrahend.</param>
        /// <returns>The difference of the two specified real and complex numbers.</returns>
        public static Complex Subtract(double lhs, Complex rhs) => lhs - rhs;

        /// <summary>
        /// The complex multiplication operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side complex multiplicand.</param>
        /// <param name="rhs">A right-hand side complex multiplier.</param>
        /// <returns>The product of the two specified complex numbers.</returns>
        public static Complex Multiply(Complex lhs, Complex rhs) => lhs * rhs;

        /// <summary>
        /// The complex multiplication operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side complex multiplicand.</param>
        /// <param name="rhs">A right-hand side real multiplier.</param>
        /// <returns>The product of the two specified complex and real numbers.</returns>
        public static Complex Multiply(Complex lhs, double rhs) => lhs * rhs;

        /// <summary>
        /// The complex multiplication operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side real multiplicand.</param>
        /// <param name="rhs">A right-hand side complex multiplier.</param>
        /// <returns>The product of the two specified real and complex numbers.</returns>
        public static Complex Multiply(double lhs, Complex rhs) => lhs * rhs;

        /// <summary>
        /// Implicit conversion of a real double to a complex number.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="number">The double number to convert.</param>
        /// <returns>The complex number.</returns>
        public static Complex ToComplex(double number) => new Complex(number, 0d);

        /// <summary>
        /// The complex division operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side complex dividend.</param>
        /// <param name="rhs">A right-hand side complex divisor.</param>
        /// <returns>The division of the two specified complex numbers.</returns>
        public static Complex Divide(Complex lhs, Complex rhs) => lhs / rhs;

        /// <summary>
        /// The complex division operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side complex dividend.</param>
        /// <param name="rhs">A right-hand side real divisor.</param>
        /// <returns>The division of the two specified complex and real numbers.</returns>
        public static Complex Divide(Complex lhs, double rhs) => lhs / rhs;

        /// <summary>
        /// The complex division operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side real dividend.</param>
        /// <param name="rhs">A right-hand side complex divisor.</param>
        /// <returns>The division of the two specified real and complex numbers.</returns>
        public static Complex Divide(double lhs, Complex rhs) => lhs / rhs;

        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side complex number.</param>
        /// <param name="rhs">A right-hand side complex number.</param>
        /// <returns>The equality of the two specified complex numbers.</returns>
        public static bool Equals(Complex lhs, Complex rhs) => lhs == rhs;

        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side complex number.</param>
        /// <param name="rhs">A right-hand side real number.</param>
        /// <returns>The equality of the two specified complex and real numbers.</returns>
        public static bool Equals(Complex lhs, double rhs) => lhs == rhs;

        /// <summary>
        /// The equality operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side real number.</param>
        /// <param name="rhs">A right-hand side complex number.</param>
        /// <returns>The equality of the two specified real and complex numbers.</returns>
        public static bool Equals(double lhs, Complex rhs) => lhs == rhs;

        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side complex number.</param>
        /// <param name="rhs">A right-hand side complex number.</param>
        /// <returns>The inequality of the two specified complex numbers.</returns>
        public static bool NotEquals(Complex lhs, Complex rhs) => lhs != rhs;

        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side complex number.</param>
        /// <param name="rhs">A right-hand side real number.</param>
        /// <returns>The inequality of the two specified complex and real numbers.</returns>
        public static bool NotEquals(Complex lhs, double rhs) => lhs != rhs;

        /// <summary>
        /// The inequality operator.
        /// </summary>
        /// <remarks>
        /// To satisfy CA2225: Operator overloads have named alternates.
        /// </remarks>
        /// <param name="lhs">A left-hand side real number.</param>
        /// <param name="rhs">A right-hand side complex number.</param>
        /// <returns>The inequality of the two specified real and complex numbers.</returns>
        public static bool NotEquals(double lhs, Complex rhs) => lhs != rhs;

        /// <summary>
        /// The absolute value of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The absolute value of the specified complex number.</returns>
        public static double Abs(Complex number)
        {
            return number.Abs();
        }

        /// <summary>
        /// The inversion of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The inversion of a specified complex number.</returns>
        public static Complex Inv(Complex number)
        {
            return number.Inv();
        }

        /// <summary>
        /// The exponential of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The exponential of the specified complex number.</returns>
        public static Complex Exp(Complex number)
        {
            return number.Exp();
        }

        /// <summary>
        /// The natural logarithm of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The natural logarithm of the specified complex number.</returns>
        public static Complex Log(Complex number)
        {
            return number.Log();
        }

        /// <summary>
        /// Raise a complex number to the given complex power.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <param name="power">A complex power.</param>
        /// <returns>The specified complex power of the specified complex number.</returns>
        public static Complex Pow(Complex number, Complex power)
        {
            return number.Pow(power);
        }

        /// <summary>
        /// Raise a double-precision floating number to the given complex power.
        /// </summary>
        /// <param name="number">A double-precision floating number.</param>
        /// <param name="power">A complex power.</param>
        /// <returns>The specified complex power of the specified double-precision floating number.</returns>
        public static Complex Pow(double number, Complex power)
        {
            return Exp(power * Math.Log(number));
        }

        /// <summary>
        /// Raise a complex number to the given double-precision floating power.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <param name="power">A double-precision floating power.</param>
        /// <returns>The specified double-precision floating power of the specified complex number.</returns>
        public static Complex Pow(Complex number, double power)
        {
            return Exp(power * Log(number));
        }

        /// <summary>
        /// The square (power of 2) of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The square (power of 2) of the specified complex number.</returns>
        public static Complex Square(Complex number)
        {
            return number.Square();
        }

        /// <summary>
        /// The complex square root of a double-precision floating-point number.
        /// </summary>
        /// <param name="number">A double-precision floating-point number.</param>
        /// <returns>The complex square root of a]the specified double-precision floating-point number.</returns>
        public static Complex Sqrt(double number)
        {
            return number >= 0
                ? new Complex(Math.Sqrt(number))
                : new Complex(0, Math.Sqrt(-number));
        }

        /// <summary>
        /// The square root of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The square root of the specified complex number.</returns>
        public static Complex Sqrt(Complex number)
        {
            return number.Sqrt();
        }

        /// <summary>
        /// The cosine of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The cosine of the specified complex number.</returns>
        public static Complex Cos(Complex number)
        {
            return number.Cos();
        }

        /// <summary>
        /// The sine of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The sine of the specified complex number.</returns>
        public static Complex Sin(Complex number)
        {
            return number.Sin();
        }

        /// <summary>
        /// The tangent of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The tangent of the specified complex number.</returns>
        public static Complex Tan(Complex number)
        {
            return number.Tan();
        }

        /// <summary>
        /// The cotangent of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The cotangent of the specified complex number.</returns>
        public static Complex Cot(Complex number)
        {
            return number.Cot();
        }

        /// <summary>
        /// The secant of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The secant of the specified complex number.</returns>
        public static Complex Sec(Complex number)
        {
            return number.Sec();
        }

        /// <summary>
        /// The cosecant of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The cosecant of the specified complex number.</returns>
        public static Complex Csc(Complex number)
        {
            return number.Csc();
        }

        /// <summary>
        /// The arccosine (inverse cosine) of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The arccosine of the specified complex number.</returns>
        public static Complex Acos(Complex number)
        {
            return number.Acos();
        }

        /// <summary>
        /// The arcsine (inverse sine) of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The arcsine of the specified complex number.</returns>
        public static Complex Asin(Complex number)
        {
            return number.Asin();
        }

        /// <summary>
        /// The arctangent of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The arctangent of the specified complex number.</returns>
        public static Complex Atan(Complex number)
        {
            return number.Atan();
        }

        /// <summary>
        /// The arcus cotangent of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The arcus cotangent of the specified complex number.</returns>
        public static Complex Acot(Complex number)
        {
            return number.Acot();
        }

        /// <summary>
        /// The arcus secant of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The arcus secant of the specified complex number.</returns>
        public static Complex Asec(Complex number)
        {
            return number.Asec();
        }

        /// <summary>
        /// The arcus cosecant of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The arcus cosecant of the specified complex number.</returns>
        public static Complex Acsc(Complex number)
        {
            return number.Acsc();
        }

        /// <summary>
        /// The hyperbolic cosine of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The hyperbolic cosine of the specified complex number.</returns>
        public static Complex Cosh(Complex number)
        {
            return number.Cosh();
        }

        /// <summary>
        /// The hyperbolic sine of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The hyperbolic sine of the specified complex number.</returns>
        public static Complex Sinh(Complex number)
        {
            return number.Sinh();
        }

        /// <summary>
        /// The hyperbolic tangent of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The hyperbolic tangent of the specified complex number.</returns>
        public static Complex Tanh(Complex number)
        {
            return number.Tanh();
        }

        /// <summary>
        /// The hyperbolic cotangent of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The hyperbolic cotangent of the specified complex number.</returns>
        public static Complex Coth(Complex number)
        {
            return number.Coth();
        }

        /// <summary>
        /// The hyperbolic secant of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The hyperbolic secant of the specified complex number.</returns>
        public static Complex Sech(Complex number)
        {
            return number.Sech();
        }

        /// <summary>
        /// The hyperbolic cosecant of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The hyperbolic  of the specified complex number.</returns>
        public static Complex Csch(Complex number)
        {
            return number.Csch();
        }

        /// <summary>
        /// The inverse hyperbolic cosine of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The inverse hyperbolic cosine of the specified complex number.</returns>
        public static Complex Acosh(Complex number)
        {
            return number.Acosh();
        }

        /// <summary>
        /// The inverse hyperbolic sine of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The inverse hyperbolic sine of the specified complex number.</returns>
        public static Complex Asinh(Complex number)
        {
            return number.Asinh();
        }

        /// <summary>
        /// The inverse hyperbolic tangent of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The inverse hyperbolic tangent of the specified complex number.</returns>
        public static Complex Atanh(Complex number)
        {
            return number.Atanh();
        }

        /// <summary>
        /// The inverse hyperbolic cotangent of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The inverse hyperbolic cotangent of the specified complex number.</returns>
        public static Complex Acoth(Complex number)
        {
            return number.Acoth();
        }

        /// <summary>
        /// The inverse hyperbolic secant of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The inverse hyperbolic secant of the specified complex number.</returns>
        public static Complex Asech(Complex number)
        {
            return number.Asech();
        }

        /// <summary>
        /// The inverse hyperbolic cosecant of a complex number.
        /// </summary>
        /// <param name="number">A complex number.</param>
        /// <returns>The inverse hyperbolic  of the specified complex number.</returns>
        public static Complex Acsch(Complex number)
        {
            return number.Acsch();
        }

        /// <summary>
        /// The absolute value of this complex number.
        /// </summary>
        /// <returns>The absolute value.</returns>
        public double Abs()
        {
            return Math.Sqrt(Imag * Imag + Real * Real);
        }

        /// <summary>
        /// The inversion of a complex number.
        /// </summary>
        /// <returns>The inversion of a specified complex number.</returns>
        public Complex Inv()
        {
            double mod = Real * Real + Imag * Imag;
            return new Complex(Real / mod, -Imag / mod);
        }

        /// <summary>
        /// The exponential of this complex number.
        /// </summary>
        /// <returns>The exponential of the complex number.</returns>
        public Complex Exp()
        {
            double exp = Math.Exp(Real);
            if (IsReal)
            {
                return new Complex(exp, 0d);
            }

            return new Complex(exp * Math.Cos(Imag), exp * Math.Sin(Imag));
        }

        /// <summary>
        /// The natural logarithm of this complex number.
        /// </summary>
        /// <returns>The natural logarithm of the complex number.</returns>
        public Complex Log()
        {
            if (IsRealNonNegative)
            {
                return new Complex(Math.Log(Real), 0d);
            }

            return new Complex(0.5d * Math.Log(ModulusSquared), Argument);
        }

        /// <summary>
        /// Raise this complex number to the given complex power.
        /// </summary>
        /// <param name="power">A complex power.</param>
        /// <returns>The specified complex power of this complex number.</returns>
        public Complex Pow(Complex power)
        {
            if (IsZero)
            {
                if (power.IsZero)
                {
                    return One;
                }

                if (power.Real > 0)
                {
                    return Zero;
                }

                if (power.Real < 0)
                {
                    return Math.Abs(power.Imag) < double.Epsilon
                        ? new Complex(double.PositiveInfinity, 0d)
                        : new Complex(double.PositiveInfinity, double.PositiveInfinity);
                }

                return NaN;
            }

            return (power * Log()).Exp();
        }

        /// <summary>
        /// Raise this complex number to the given double-precision floating power.
        /// </summary>
        /// <param name="power">A double-precision floating power.</param>
        /// <returns>The specified double-precision floating power of this complex number.</returns>
        public Complex Pow(double power)
        {
            return Exp(power * Log());
        }

        /// <summary>
        /// The square (power of 2) of this complex number.
        /// </summary>
        /// <returns>The (power of 2) root of this complex number.</returns>
        public Complex Square()
        {
            return IsReal
                ? new Complex(Real * Real, 0d)
                : new Complex(Real * Real - Imag * Imag, 2 * Real * Imag);
        }

        /// <summary>
        /// The square root of this complex number.
        /// </summary>
        /// <returns>The square root of the complex number.</returns>
        public Complex Sqrt()
        {
            if (IsRealNonNegative)
            {
                return new Complex(Math.Sqrt(Real), 0d);
            }

            double mod = Modulus;
            if (Imag > 0 || (Math.Abs(Imag) < double.Epsilon && Real < 0))
            {
                return new Complex(
                    Constants.HalfSqrt2 * Math.Sqrt(mod + Real),
                    Constants.HalfSqrt2 * Math.Sqrt(mod - Real));
            }

            return new Complex(
                Constants.HalfSqrt2 * Math.Sqrt(mod + Real),
                -Constants.HalfSqrt2 * Math.Sqrt(mod - Real));
        }

        /// <summary>
        /// The cosine of this complex number.
        /// </summary>
        /// <returns>The cosine of the complex number.</returns>
        public Complex Cos()
        {
            return IsReal
                ? new Complex(Math.Cos(Real), 0d)
                : new Complex(Math.Cos(Real) * Math.Cosh(Imag), -Math.Sin(Real) * Math.Sinh(Imag));
        }

        /// <summary>
        /// The sine of this complex number.
        /// </summary>
        /// <returns>The sine of the complex number.</returns>
        public Complex Sin()
        {
            return IsReal
                ? new Complex(Math.Sin(Real), 0d)
                : new Complex(Math.Sin(Real) * Math.Cosh(Imag), Math.Cos(Real) * Math.Sinh(Imag));
        }

        /// <summary>
        /// The tangent of this complex number.
        /// </summary>
        /// <returns>The tangent of the complex number.</returns>
        public Complex Tan()
        {
            if (IsReal)
            {
                return new Complex(Math.Tan(Real), 0d);
            }

            double cosR = Math.Cos(Real);
            double sinhÌ = Math.Sinh(Imag);
            double denominator = cosR * cosR + sinhÌ * sinhÌ;
            return new Complex(
                Math.Sin(Real) * cosR / denominator,
                sinhÌ * Math.Cosh(Imag) / denominator);
        }

        /// <summary>
        /// The cotangent of this complex number.
        /// </summary>
        /// <returns>The cotangent of the complex number.</returns>
        public Complex Cot()
        {
            if (IsReal)
            {
                return new Complex(1 / Math.Tan(Real), 0d);
            }

            double sinR = Math.Sin(Real);
            double sinhI = Math.Sinh(Imag);
            double denominator = sinR * sinR + sinhI * sinhI;
            return new Complex(
                sinR * Math.Cos(Real) / denominator,
                -sinhI * Math.Cosh(Imag) / denominator);
        }

        /// <summary>
        /// The secant of this complex number.
        /// </summary>
        /// <returns>The secant of the complex number.</returns>
        public Complex Sec()
        {
            if (IsReal)
            {
                return new Complex(1 / Math.Cos(Real), 0d);
            }

            double cosR = Math.Cos(Real);
            double sinhI = Math.Sinh(Imag);
            double denominator = cosR * cosR + sinhI * sinhI;
            return new Complex(
                cosR * Math.Cosh(Imag) / denominator,
                Math.Sin(Real) * sinhI / denominator);
        }

        /// <summary>
        /// The cosecant of this complex number.
        /// </summary>
        /// <returns>The cosecant of the complex number.</returns>
        public Complex Csc()
        {
            if (IsReal)
            {
                return new Complex(1 / Math.Sin(Real), 0d);
            }

            double sinR = Math.Sin(Real);
            double sinhI = Math.Sinh(Imag);
            double denominator = sinR * sinR + sinhI * sinhI;
            return new Complex(
                sinR * Math.Cosh(Imag) / denominator,
                -Math.Cos(Real) * sinhI / denominator);
        }

        /// <summary>
        /// The arccosine (inverse cosine) of this complex number.
        /// </summary>
        /// <returns>The arccosine of the complex number.</returns>
        public Complex Acos()
        {
            return -ImaginaryOne * (this + ImaginaryOne * (1 - Square()).Sqrt()).Log();
        }

        /// <summary>
        /// The arcsine (inverse sine) of this complex number.
        /// </summary>
        /// <returns>The arcsine sine of the complex number.</returns>
        public Complex Asin()
        {
            return -ImaginaryOne * ((1 - Square()).Sqrt() + ImaginaryOne * this).Log();
        }

        /// <summary>
        /// The arctangent of this complex number.
        /// </summary>
        /// <returns>The arctangent of the complex number.</returns>
        public Complex Atan()
        {
            // Imaginary one * this
            var iz = new Complex(-Imag, Real);

            return new Complex(0d, 0.5) * ((1 - iz).Log() - (1 + iz).Log());
        }

        /// <summary>
        /// The arcus cotangent of this complex number.
        /// </summary>
        /// <returns>The arcus cotangent of the complex number.</returns>
        public Complex Acot()
        {
            // Imaginary one * this
            var iz = new Complex(-Imag, Real);

            return new Complex(0d, 0.5) * ((1 + iz).Log() - (1 - iz).Log()) + Constants.PiOver2;
        }

        /// <summary>
        /// The arcus secant of this complex number.
        /// </summary>
        /// <returns>The arcus secant of the complex number.</returns>
        public Complex Asec()
        {
            Complex inv = 1 / this;

            return -ImaginaryOne * (inv + ImaginaryOne * (1 - inv.Square()).Sqrt()).Log();
        }

        /// <summary>
        /// The arcus cosecant of this complex number.
        /// </summary>
        /// <returns>The arcus cosecant of the complex number.</returns>
        public Complex Acsc()
        {
            Complex inv = 1 / this;

            return -ImaginaryOne * (ImaginaryOne * inv + (1 - inv.Square()).Sqrt()).Log();
        }

        /// <summary>
        /// The hyperbolic cosine of this complex number.
        /// </summary>
        /// <returns>The hyperbolic cosine of the complex number.</returns>
        public Complex Cosh()
        {
            return IsReal
                ? new Complex(Math.Cosh(Real), 0d)
                : new Complex(Math.Cosh(Real) * Math.Cos(Imag), Math.Sinh(Real) * Math.Sin(Imag));
        }

        /// <summary>
        /// The hyperbolic sine of this complex number.
        /// </summary>
        /// <returns>The hyperbolic sine of the complex number.</returns>
        public Complex Sinh()
        {
            return IsReal
                ? new Complex(Math.Sinh(Real), 0d)
                : new Complex(Math.Sinh(Real) * Math.Cos(Imag), Math.Cosh(Real) * Math.Sin(Imag));
        }

        /// <summary>
        /// The hyperbolic tangent of this complex number.
        /// </summary>
        /// <returns>The hyperbolic tangent of the complex number.</returns>
        public Complex Tanh()
        {
            if (IsReal)
            {
                return new Complex(Math.Tanh(Real), 0d);
            }

            double cosI = Math.Cos(Imag);
            double sinhR = Math.Sinh(Real);
            double denominator = cosI * cosI + sinhR * sinhR;
            return new Complex(
                Math.Cosh(Real) * sinhR / denominator,
                cosI * Math.Sin(Imag) / denominator);
        }

        /// <summary>
        /// The hyperbolic cotangent of this complex number.
        /// </summary>
        /// <returns>The hyperbolic cotangent of the complex number.</returns>
        public Complex Coth()
        {
            if (IsReal)
            {
                return new Complex(1 / Math.Tanh(Real), 0d);
            }

            double sinI = Math.Sin(Imag);
            double sinhR = Math.Sinh(Real);
            double denominator = sinI * sinI + sinhR * sinhR;
            return new Complex(
                sinhR * Math.Cosh(Real) / denominator,
                -sinI * Math.Cos(Imag) / denominator);
        }

        /// <summary>
        /// The hyperbolic secant of this complex number.
        /// </summary>
        /// <returns>The hyperbolic secant of the complex number.</returns>
        public Complex Sech()
        {
            if (IsReal)
            {
                return new Complex(1 / Math.Cosh(Real), 0d);
            }

            Complex exp = Exp();
            return 2 * exp / (exp.Square() + 1);
        }

        /// <summary>
        /// The hyperbolic cosecant of this complex number.
        /// </summary>
        /// <returns>The hyperbolic cosecant of the complex number.</returns>
        public Complex Csch()
        {
            if (IsReal)
            {
                return new Complex(1 / Math.Sinh(Real), 0d);
            }

            Complex exp = Exp();
            return 2 * exp / (exp.Square() - 1);
        }

        /// <summary>
        /// The inverse hyperbolic cosine of this complex number.
        /// </summary>
        /// <returns>The inverse hyperbolic cosine of the complex number.</returns>
        public Complex Acosh()
        {
            return (this + (this - 1).Sqrt() * (this + 1).Sqrt()).Log();
        }

        /// <summary>
        /// The inverse hyperbolic sine of this complex number.
        /// </summary>
        /// <returns>The inverse hyperbolic sine of the complex number.</returns>
        public Complex Asinh()
        {
            return (this + (Square() + 1).Sqrt()).Log();
        }

        /// <summary>
        /// The inverse hyperbolic tangent of this complex number.
        /// </summary>
        /// <returns>The inverse hyperbolic tangent of the complex number.</returns>
        public Complex Atanh()
        {
            return 0.5 * ((1 + this).Log() - (1 - this).Log());
        }

        /// <summary>
        /// The inverse hyperbolic cotangent of this complex number.
        /// </summary>
        /// <returns>The inverse hyperbolic cotangent of the complex number.</returns>
        public Complex Acoth()
        {
            return 0.5 * ((this + 1).Log() - (this - 1).Log());
        }

        /// <summary>
        /// The inverse hyperbolic secant of this complex number.
        /// </summary>
        /// <returns>The inverse hyperbolic secant of the complex number.</returns>
        public Complex Asech()
        {
            Complex inv = 1d / this;
            return (inv + (inv - 1d).Sqrt() * (inv + 1).Sqrt()).Log();
        }

        /// <summary>
        /// The inverse hyperbolic cosecant of this complex number.
        /// </summary>
        /// <returns>The inverse hyperbolic cosecant of the complex number.</returns>
        public Complex Acsch()
        {
            Complex inv = 1d / this;
            return (inv + (inv.Square() + 1d).Sqrt()).Log();
        }

        /// <summary>
        /// Returns the string that represents this object.
        /// </summary>
        /// <returns>Returns the string that represents the object.</returns>
        public override string ToString()
        {
            return ToString(NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Returns the string that represents this object.
        /// </summary>
        /// <param name="format">The format info.</param>
        /// <returns>Returns the string that represents the object.</returns>
        public string ToString(string format)
        {
            return ToString(format, NumberFormatInfo.CurrentInfo);
        }

        /// <summary>
        /// Returns the string that represents this object.
        /// </summary>
        /// <param name="format">The format info.</param>
        /// <param name="numberFormat">The number format info.</param>
        /// <returns>Returns the string that represents the object.</returns>
        public string ToString(string format, NumberFormatInfo numberFormat)
        {
            if (IsInfinity)
            {
                return "Infinity";
            }

            if (IsNaN)
            {
                return numberFormat.NaNSymbol;
            }

            if (IsReal)
            {
                return Real.ToString(format, numberFormat);
            }

            // There's a difference between the negative sign and the subtraction operator!
            if (IsImaginary)
            {
                if (Math.Abs(Imag - 1) < double.Epsilon)
                {
                    return "i";
                }

                if (Math.Abs(Imag + 1) < double.Epsilon)
                {
                    return string.Concat(numberFormat.NegativeSign, "i");
                }

                return Imag < 0
                    ? string.Concat(numberFormat.NegativeSign, (-Imag).ToString(format, numberFormat), "i")
                    : string.Concat(Imag.ToString(format, numberFormat), "i");
            }

            if (Math.Abs(Imag - 1) < double.Epsilon)
            {
                return string.Concat(Real.ToString(format, numberFormat), "+i");
            }

            if (Math.Abs(Imag + 1) < double.Epsilon)
            {
                return string.Concat(Real.ToString(format, numberFormat), "-i");
            }

            return Imag < 0
                ? string.Concat(Real.ToString(format, numberFormat), "-", (-Imag).ToString(format, numberFormat), "i")
                : string.Concat(Real.ToString(format, numberFormat), "+", Imag.ToString(format, numberFormat), "i");
        }

        /// <summary>
        /// Returns the string that represents this object.
        /// </summary>
        /// <param name="numberFormat">The number format info.</param>
        /// <returns>Returns the string that represents the object.</returns>
        public string ToString(NumberFormatInfo numberFormat)
        {
            if (IsInfinity)
            {
                return "Infinity";
            }

            if (IsNaN)
            {
                return numberFormat.NaNSymbol;
            }

            if (IsReal)
            {
                return Real.ToString(numberFormat);
            }

            // There's a difference between the negative sign and the subtraction operator!
            if (IsImaginary)
            {
                if (Math.Abs(Imag - 1) < double.Epsilon)
                {
                    return "i";
                }

                if (Math.Abs(Imag + 1) < double.Epsilon)
                {
                    return string.Concat(numberFormat.NegativeSign, "i");
                }

                return Imag < 0
                    ? string.Concat(numberFormat.NegativeSign, (-Imag).ToString(numberFormat), "i")
                    : string.Concat(Imag.ToString(numberFormat), "i");
            }

            if (Math.Abs(Imag - 1) < double.Epsilon)
            {
                return string.Concat(Real.ToString(numberFormat), "+i");
            }

            if (Math.Abs(Imag + 1) < double.Epsilon)
            {
                return string.Concat(Real.ToString(numberFormat), "-i");
            }

            return Imag < 0
                ? string.Concat(Real.ToString(numberFormat), "-", (-Imag).ToString(numberFormat), "i")
                : string.Concat(Real.ToString(numberFormat), "+", Imag.ToString(numberFormat), "i");
        }

        /// <summary>
        /// Determines whether the specified instances are considered equal.
        /// </summary>
        /// <param name="obj">The object to compare with this object.</param>
        /// <returns>True if objects are equal, false if not.</returns>
        public override bool Equals(object obj)
        {
            return obj is Complex complex && Equals(complex);
        }

        /// <summary>
        /// Determines whether the specified complex numbers are considered equal.
        /// </summary>
        /// <param name="other">The complex number to compare with this one.</param>
        /// <returns>True if complex numbers are equal, false if not.</returns>
        public bool Equals(Complex other)
        {
            return /*!IsNaN && !other.IsNaN &&*/ (Math.Abs(Real - other.Real) < double.Epsilon) && (Math.Abs(Imag - other.Imag) < double.Epsilon);
        }

        /// <summary>
        /// Compares this complex number with another complex number.
        /// The complex number's modulus takes precedence over the argument.
        /// </summary>
        /// <param name="other">The complex number to compare with.</param>
        /// <returns>Returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the specified value.</returns>
        public int CompareTo(Complex other)
        {
            int result = Modulus.CompareTo(other.Modulus);
            return result != 0
                ? result
                : Argument.CompareTo(other.Argument);
        }

        /// <summary>
        /// Gets the hashcode of the complex number.
        /// </summary>
        /// <returns>The hashcode.</returns>
        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            return Real.GetHashCode() ^ -Imag.GetHashCode();

            // ReSharper restore NonReadonlyMemberInGetHashCode
        }
    }
}
