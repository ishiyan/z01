using System;
using System.Globalization;
using Mbs.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mbs.UnitTests.Numerics
{
    [TestClass]
    public class ComplexTests
    {
        [TestMethod]
        public void Complex_Constructor_RealImaginary_Instance()
        {
            const double real = 3.5;
            const double imaginary = 4.6;
            var target = new Complex(real, imaginary);

            Assert.AreEqual(real, target.Real, "#1");
            Assert.AreEqual(imaginary, target.Imag, "#2");
        }

        [TestMethod]
        public void Complex_Constructor_RealOnly_Instance()
        {
            const double real = 7.7;
            var target = new Complex(real);

            Assert.AreEqual(real, target.Real, "#1");
            Assert.AreEqual(0, target.Imag, "#2");
        }

        [TestMethod]
        public void Complex_FromModulus_ValidArguments_Success()
        {
            double modulus = 8.8;
            double argument = 9.9;
            Complex target = Complex.FromModulusArgument(modulus, argument);
            Assert.AreEqual(modulus, target.Modulus, "#1");

            double input = Normalize2Pi(argument);
            double output = Normalize2Pi(target.Argument);
            Assert.AreEqual(input, output, 1e-5, "#2");

            modulus = 1;
            argument = 0;
            target = Complex.FromModulusArgument(modulus, argument);
            Assert.AreEqual(modulus, target.Modulus, "#3");

            output = target.Argument;
            Assert.AreEqual(argument, output, 1e-5, "#4");

            modulus = 0;
            argument = 0;
            target = Complex.FromModulusArgument(modulus, argument);
            Assert.AreEqual(modulus, target.Modulus, "#5");

            output = target.Argument;
            Assert.AreEqual(argument, output, 1e-5, "#6");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Complex_FromModulus_InvalidArguments_Exception()
        {
            Complex.FromModulusArgument(-1, 1);

            // Should throw.
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Complex_FromRealImaginary_ValidArguments_Success()
        {
            double real = 8.8;
            double imaginary = 9.9;
            Complex target = Complex.FromRealImaginary(real, imaginary);
            Assert.AreEqual(real, target.Real, "#1");
            Assert.AreEqual(imaginary, target.Imag, "#2");

            real = 0;
            target = Complex.FromRealImaginary(real, imaginary);
            Assert.AreEqual(real, target.Real, "#3");
            Assert.AreEqual(imaginary, target.Imag, "B2");

            imaginary = 0;
            target = Complex.FromRealImaginary(real, imaginary);
            Assert.AreEqual(real, target.Real, "#4");
            Assert.AreEqual(imaginary, target.Imag, "#5");

            real = -9;
            target = Complex.FromRealImaginary(real, imaginary);
            Assert.AreEqual(real, target.Real, "#6");
            Assert.AreEqual(imaginary, target.Imag, "#7");

            imaginary = -8;
            target = Complex.FromRealImaginary(real, imaginary);
            Assert.AreEqual(real, target.Real, "#8");
            Assert.AreEqual(imaginary, target.Imag, "#9");
        }

        [TestMethod]
        public void Complex_Abs_ValidArguments_Success()
        {
            var target = new Complex(0, 0);
            Assert.AreEqual(0, target.Abs(), "#1");

            target = new Complex(0, 1);
            Assert.AreEqual(1, target.Abs(), "#2");

            target = new Complex(1, 5);
            Assert.AreEqual(Math.Sqrt(26), target.Abs(), "#3");

            // Static
            Assert.AreEqual(Math.Sqrt(26.0), Complex.Abs(target), "#4");

            // Matlab
            target = new Complex(1.1, 1.2);
            Assert.AreEqual(1.627882059609971, target.Abs(), 1e-15, "#5");
            target = new Complex(123.456, 789.123);
            Assert.AreEqual(798.7217870228657, target.Abs(), 1e-15, "#6");
        }

        [TestMethod]
        public void Complex_Inv_Matlab_Conformance()
        {
            var number = new Complex(123.456, 789.123);
            Complex target = number.Inv();
            Assert.AreEqual(0.000193517898700, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-0.001236954257192, target.Imag, 1e-15, "#1 imag");

            number = new Complex(1.2345, -1.6789);
            target = number.Inv();
            Assert.AreEqual(0.284270451697757, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(0.386603208874334, target.Imag, 1e-15, "#2 imag");

            number = new Complex(1.2345, 1.6789);
            target = number.Inv();
            Assert.AreEqual(0.284270451697757, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-0.386603208874334, target.Imag, 1e-15, "#3 imag");

            number = new Complex(-1.2345, 1.6789);
            target = number.Inv();
            Assert.AreEqual(-0.284270451697757, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-0.386603208874334, target.Imag, 1e-15, "#4 imag");

            number = new Complex(-1.2345, -1.6789);
            target = number.Inv();
            Assert.AreEqual(-0.284270451697757, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(0.386603208874334, target.Imag, 1e-15, "#5 imag");

            // Static
            target = Complex.Inv(number);
            Assert.AreEqual(-0.284270451697757, target.Real, 1e-15, "#6 real");
            Assert.AreEqual(0.386603208874334, target.Imag, 1e-15, "#6 imag");
        }

        [TestMethod]
        public void Complex_Exp_Matlab_Conformance()
        {
            var number = new Complex(1, 3);
            Complex target = number.Exp();
            Assert.AreEqual(Math.Exp(1), target.Abs(), 1e-4, "#1 a");
            Assert.AreEqual(3, target.Argument, 1e-4, "#1 b");

            // Static
            target = Complex.Exp(number);
            Assert.AreEqual(Math.Exp(1), target.Abs(), 1e-4, "#2 a");
            Assert.AreEqual(3, target.Argument, 1e-4, "#2 b");
        }

        [TestMethod]
        public void Complex_Exp_KnownProperties_Conformance()
        {
            // exp(0) = 1
            var number = Complex.Zero;
            Complex target = number.Exp();
            Assert.AreEqual(1, target.Real, "#1 real");
            Assert.AreEqual(0, target.Imag, "#1 imag");

            // exp(1) = e
            number = Complex.One;
            target = number.Exp();
            Assert.AreEqual(Constants.E, target.Real, "#2 real");
            Assert.AreEqual(0, target.Imag, "#2 imag");

            // exp(i) = cos(1) + sin(1) * i
            number = Complex.ImaginaryOne;
            target = number.Exp();
            Assert.AreEqual(Math.Cos(1), target.Real, "#3 real");
            Assert.AreEqual(Math.Sin(1), target.Imag, "#3 imag");

            // exp(-1) = 1/e
            number = -Complex.One;
            target = number.Exp();
            Assert.AreEqual(1 / Constants.E, target.Real, "#4 real");
            Assert.AreEqual(0, target.Imag, "#4 imag");

            // exp(-i) = cos(1) - sin(1) * i
            number = -Complex.ImaginaryOne;
            target = number.Exp();
            Assert.AreEqual(Math.Cos(1), target.Real, "#5 real");
            Assert.AreEqual(-Math.Sin(1), target.Imag, "#5 imag");

            // exp(i+1) = e * cos(1) + e * sin(1) * i
            number = Complex.One + Complex.ImaginaryOne;
            target = number.Exp();
            Assert.AreEqual(Constants.E * Math.Cos(1), target.Real, "#6 real");
            Assert.AreEqual(Constants.E * Math.Sin(1), target.Imag, "#6 imag");
        }

        [TestMethod]
        public void Complex_Log_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Log();
            Assert.AreEqual(-1.151292546497023, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(1.249045772398254, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Log();
            Assert.AreEqual(0.407682406642097, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(1.637364490570721, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Log();
            Assert.AreEqual(-0.539404830685965, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-2.601173153319209, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Log();
            Assert.AreEqual(0.262364264467491, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-1.176005207095135, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Log(number);
            Assert.AreEqual(0.262364264467491, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-1.176005207095135, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Log_KnownProperties_Conformance()
        {
            // ln(0) = -infinity
            var number = Complex.Zero;
            Complex target = number.Log();
            Assert.AreEqual(double.NegativeInfinity, target.Real, "#1 real");
            Assert.AreEqual(0, target.Imag, "#1 imag");

            // ln(1) = 0
            number = Complex.One;
            target = number.Log();
            Assert.AreEqual(0, target.Real, "#2 real");
            Assert.AreEqual(0, target.Imag, "#2 imag");

            // ln(i) = Pi/2 * i
            number = Complex.ImaginaryOne;
            target = number.Log();
            Assert.AreEqual(0, target.Real, "#3 real");
            Assert.AreEqual(Constants.PiOver2, target.Imag, "#3 imag");

            // ln(-1) = Pi * i
            number = -Complex.One;
            target = number.Log();
            Assert.AreEqual(0, target.Real, "#4 real");
            Assert.AreEqual(Constants.Pi, target.Imag, "#4 imag");

            // ln(-i) = -Pi/2 * i
            number = -Complex.ImaginaryOne;
            target = number.Log();
            Assert.AreEqual(0, target.Real, "#5 real");
            Assert.AreEqual(-Constants.PiOver2, target.Imag, "#5 imag");

            // ln(i+1) = ln(2)/2 + Pi/4 * i
            number = Complex.One + Complex.ImaginaryOne;
            target = number.Log();
            Assert.AreEqual(Constants.Ln2 / 2, target.Real, "#6 real");
            Assert.AreEqual(Constants.PiOver4, target.Imag, "#6 imag");
        }

        [TestMethod]
        public void Complex_Pow_Matlab_Conformance()
        {
            var number = new Complex(0.5, -Math.Sqrt(3) / 2);
            Complex target = number.Pow(2d);
            Assert.AreEqual(0.5, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(Math.Sqrt(3) / 2, target.Imag, 1e-15, "#1 imag");

            number = new Complex(3, 2);
            target = number.Pow(-2d);
            Complex expected = 1d / (number * number);
            Assert.AreEqual(expected.Real, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(expected.Imag, target.Imag, 1e-15, "#2 imag");

            // Static
            target = Complex.Pow(number, -2d);
            Assert.AreEqual(expected.Real, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(expected.Imag, target.Imag, 1e-15, "#3 imag");
        }

        [TestMethod]
        public void Complex_Pow_KnownProperties_Conformance()
        {
            // (1)^(1) = 1
            var number = Complex.One;
            Complex target = number.Pow(number);
            Assert.AreEqual(1, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(0, target.Imag, 1e-15, "#1 imag");

            // (i)^(1) = i
            number = Complex.ImaginaryOne;
            target = number.Pow(Complex.One);
            Assert.AreEqual(0, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(1, target.Imag, 1e-15, "#2 imag");

            // (1)^(-1) = 1
            number = Complex.One;
            target = number.Pow(-Complex.One);
            Assert.AreEqual(1, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(0, target.Imag, 1e-15, "#3 imag");

            // (i)^(-1) = -i
            number = Complex.ImaginaryOne;
            target = number.Pow(-Complex.One);
            Assert.AreEqual(0, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-1, target.Imag, 1e-15, "#4 imag");

            // (i)^(-i) = exp(Pi/2)
            number = Complex.ImaginaryOne;
            target = number.Pow(-Complex.ImaginaryOne);
            Assert.AreEqual(Math.Exp(Constants.PiOver2), target.Real, 1e-15, "#5 real");
            Assert.AreEqual(0, target.Imag, 1e-15, "#5 imag");

            // (0)^(0) = 1
            Assert.AreEqual(1, Math.Pow(0d, 0d), "#6 (0)^(0) = 1 (.Net Sanity Check)");
            number = Complex.Zero;
            target = number.Pow(Complex.Zero);
            Assert.AreEqual(1, target.Real, 1e-15, "#6 real");
            Assert.AreEqual(0, target.Imag, 1e-15, "#6 imag");

            // (0)^(2) = 0
            Assert.AreEqual(0, Math.Pow(0d, 2d), "#7 (0)^(2) = 0 (.Net Sanity Check)");
            number = Complex.Zero;
            target = number.Pow(new Complex(2, 0));
            Assert.AreEqual(0, target.Real, 1e-15, "#7 real");
            Assert.AreEqual(0, target.Imag, 1e-15, "#7 imag");

            // (0)^(-2) = infinity
            Assert.AreEqual(double.PositiveInfinity, Math.Pow(0d, -2d), "#8 (0)^(-2) = infinity (.Net Sanity Check)");
            number = Complex.Zero;
            target = number.Pow(Complex.FromRealImaginary(-2d, 0d));
            Assert.IsTrue(double.IsPositiveInfinity(target.Real), "#8 real");
            Assert.AreEqual(0, target.Imag, 1e-15, "#8 imag");

            // (0)^(ImaginaryOne) = NaN
            number = Complex.Zero;
            target = number.Pow(Complex.ImaginaryOne);
            Assert.IsTrue(double.IsNaN(target.Real), "#9 real");
            Assert.IsTrue(double.IsNaN(target.Imag), "#9 imag");

            // (0)^(-ImaginaryOne) = NaN
            number = Complex.Zero;
            target = number.Pow(-Complex.ImaginaryOne);
            Assert.IsTrue(double.IsNaN(target.Real), "#10 real");
            Assert.IsTrue(double.IsNaN(target.Imag), "#10 imag");

            // (0)^(1+ImaginaryOne) = 0
            number = Complex.Zero;
            target = number.Pow(Complex.One + Complex.ImaginaryOne);
            Assert.AreEqual(0, target.Real, 1e-15, "#11 real");
            Assert.AreEqual(0, target.Imag, 1e-15, "#11 imag");

            // (0)^(1-ImaginaryOne) = 0
            number = Complex.Zero;
            target = number.Pow(Complex.One - Complex.ImaginaryOne);
            Assert.AreEqual(0, target.Real, 1e-15, "#12 real");
            Assert.AreEqual(0, target.Imag, 1e-15, "#12 imag");

            // (0)^(-1+ImaginaryOne) = infinity + infinity * i
            number = Complex.Zero;
            target = number.Pow(new Complex(-1d, 1d));
            Assert.IsTrue(double.IsPositiveInfinity(target.Real), "#13 real");
            Assert.IsTrue(double.IsPositiveInfinity(target.Imag), "#13 imag");
        }

        [TestMethod]
        public void Complex_Square_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Square();
            Assert.AreEqual(-0.080000000000000, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(0.060000000000000, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Square();
            Assert.AreEqual(-2.240000000000000, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-0.300000000000000, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -1.3);
            target = number.Square();
            Assert.AreEqual(-1.440000000000000, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(1.300000000000000, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.6, -1.1);
            target = number.Square();
            Assert.AreEqual(-0.850000000000000, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-1.320000000000000, target.Imag, 1e-15, "#4 imag");

            // Static
            number = new Complex(0.678, -1.123);
            target = Complex.Square(number);
            Assert.AreEqual(-0.801445000000000, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-1.522788000000000, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Sqrt_Matlab_Conformance()
        {
            var number = new Complex(1, 4);
            Complex target = number.Sqrt();
            Assert.AreEqual(1.600485180440241, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(1.249621067687653, target.Imag, 1e-15, "#1 imag");

            number = new Complex(2.3, -3.6);
            target = number.Sqrt();
            Assert.AreEqual(1.812733001941925, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-0.992975798461062, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-3.1, -2.5);
            target = number.Sqrt();
            Assert.AreEqual(0.664252041904267, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-1.881815818610841, target.Imag, 1e-15, "#3 imag");

            number = new Complex(-1.9, 5.3);
            target = number.Sqrt();
            Assert.AreEqual(1.365700425441777, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(1.940396261605307, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Sqrt(number);
            Assert.AreEqual(1.365700425441777, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(1.940396261605307, target.Imag, 1e-15, "#5 imag");

            target = Complex.Sqrt(9.7);
            Assert.AreEqual(3.114482300479487, target.Real, 1e-15, "#6 real");
            Assert.AreEqual(0, target.Imag, 1e-15, "#6 imag");

            target = Complex.Sqrt(0d);
            Assert.AreEqual(0, target.Real, 1e-15, "#7 real");
            Assert.AreEqual(0, target.Imag, 1e-15, "#7 imag");

            target = Complex.Sqrt(1d);
            Assert.AreEqual(1, target.Real, 1e-15, "#8 real");
            Assert.AreEqual(0, target.Imag, 1e-15, "#8 imag");

            target = Complex.Sqrt(-7);
            Assert.AreEqual(0, target.Real, 1e-15, "#9 real");
            Assert.AreEqual(2.645751311064591, target.Imag, 1e-15, "#9 imag");

            target = Complex.Sqrt(-0.005);
            Assert.AreEqual(0, target.Real, 1e-15, "#10 real");
            Assert.AreEqual(0.070710678118655, target.Imag, 1e-15, "#10 imag");
        }

        [TestMethod]
        public void Complex_Cos_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Cos();
            Assert.AreEqual(1.040116175683759, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-0.030401301333123, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Cos();
            Assert.AreEqual(2.340657365607109, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(0.212573242998012, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Cos();
            Assert.AreEqual(0.917370851271881, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-0.145994805701806, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Cos();
            Assert.AreEqual(1.588999751473591, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(0.723674323320711, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Cos(number);
            Assert.AreEqual(1.588999751473591, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(0.723674323320711, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Sin_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Sin();
            Assert.AreEqual(0.104359715418003, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(0.302998960391594, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Sin();
            Assert.AreEqual(-0.234849089242584, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(2.118641926860268, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Sin();
            Assert.AreEqual(-0.501161980159946, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-0.267241699270951, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Sin();
            Assert.AreEqual(0.868074520591187, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-1.324676963357129, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Sin(number);
            Assert.AreEqual(0.868074520591187, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-1.324676963357129, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Tan_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Tan();
            Assert.AreEqual(0.091741590289446, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(0.293994104958268, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Tan();
            Assert.AreEqual(-0.017982821488705, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(0.906781413088994, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Tan();
            Assert.AreEqual(-0.487592316492139, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-0.368910396825564, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Tan();
            Assert.AreEqual(0.138008291862926, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-0.896507390427591, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Tan(number);
            Assert.AreEqual(0.138008291862926, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-0.896507390427591, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Cot_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Cot();
            Assert.AreEqual(0.967237808425478, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-3.099599787541052, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Cot();
            Assert.AreEqual(-0.021861595026880, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-1.102368059612034, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Cot();
            Assert.AreEqual(-1.304276747265860, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(0.986810571310472, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Cot();
            Assert.AreEqual(0.167735809112832, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(1.089618532909340, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Cot(number);
            Assert.AreEqual(0.167735809112832, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(1.089618532909340, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Sec_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Sec();
            Assert.AreEqual(0.960610393774748, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(0.028077446277266, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Sec();
            Assert.AreEqual(0.423735494587800, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-0.038482705577257, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Sec();
            Assert.AreEqual(1.063145340789472, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(0.169194058483704, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Sec();
            Assert.AreEqual(0.521218545691264, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-0.237377304814261, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Sec(number);
            Assert.AreEqual(0.521218545691264, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-0.237377304814261, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Cosec_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Csc();
            Assert.AreEqual(1.016167538541131, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-2.950350204850528, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Csc();
            Assert.AreEqual(-0.051685639257015, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-0.466271181632630, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Csc();
            Assert.AreEqual(-1.553598232470387, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(0.828447184874691, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Csc();
            Assert.AreEqual(0.346077725103826, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(0.528112712793212, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Csc(number);
            Assert.AreEqual(0.346077725103826, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(0.528112712793212, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Acos_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Acos();
            Assert.AreEqual(1.474903367519443, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-0.296999023408390, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Acos();
            Assert.AreEqual(1.626235699131362, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-1.196042838552249, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Acos();
            Assert.AreEqual(2.063835567380515, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(0.334299817774938, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Acos();
            Assert.AreEqual(1.255110336239894, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(1.055304915437955, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Acos(number);
            Assert.AreEqual(1.255110336239894, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(1.055304915437955, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Asin_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Asin();
            Assert.AreEqual(0.095892959275454, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(0.296999023408390, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Asin();
            Assert.AreEqual(-0.055439372336465, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(1.196042838552249, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Asin();
            Assert.AreEqual(-0.493039240585618, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-0.334299817774938, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Asin();
            Assert.AreEqual(0.315685990555002, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-1.055304915437955, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Asin(number);
            Assert.AreEqual(0.315685990555002, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-1.055304915437955, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Atan_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Atan();
            Assert.AreEqual(0.109334472936971, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(0.305943857905529, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Atan();
            Assert.AreEqual(-1.492087890431601, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(0.795313458269654, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Atan();
            Assert.AreEqual(-0.493711659900520, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-0.240948266464790, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Atan();
            Assert.AreEqual(1.087389652523947, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-0.716288046641012, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Atan(number);
            Assert.AreEqual(1.087389652523947, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-0.716288046641012, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Acot_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Acot();
            Assert.AreEqual(1.461461853857926, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-0.305943857905529, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Acot();
            Assert.AreEqual(Constants.Pi + -0.078708436363295, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-0.795313458269654, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Acot();
            Assert.AreEqual(Constants.Pi + -1.077084666894376, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(0.240948266464790, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Acot();
            Assert.AreEqual(0.483406674270949, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(0.716288046641012, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Acot(number);
            Assert.AreEqual(0.483406674270949, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(0.716288046641012, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Asec_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Asec();
            Assert.AreEqual(1.263192677264185, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(1.864161544157883, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Asec();
            Assert.AreEqual(1.607663511869202, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(0.623065008994206, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Asec();
            Assert.AreEqual(2.517873627505525, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-1.200699546128141, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Asec();
            Assert.AreEqual(1.329643731985686, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-0.678053948499639, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Asec(number);
            Assert.AreEqual(1.329643731985686, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-0.678053948499639, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Acosec_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Acsc();
            Assert.AreEqual(0.307603649530711, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-1.864161544157883, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Acsc();
            Assert.AreEqual(-0.036867185074306, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-0.623065008994206, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Acsc();
            Assert.AreEqual(-0.947077300710628, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(1.200699546128141, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Acsc();
            Assert.AreEqual(0.241152594809211, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(0.678053948499639, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Acsc(number);
            Assert.AreEqual(0.241152594809211, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(0.678053948499639, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Cosh_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Cosh();
            Assert.AreEqual(0.960117153467032, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(0.029601298666459, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Cosh();
            Assert.AreEqual(0.071091182512645, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-0.099915830969216, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Cosh();
            Assert.AreEqual(1.077262230647136, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(0.153994192369766, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Cosh();
            Assert.AreEqual(0.408604012641776, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-0.485681192234205, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Cosh(number);
            Assert.AreEqual(0.408604012641776, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-0.485681192234205, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Sinh_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Sinh();
            Assert.AreEqual(0.095692951291080, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(0.296999039439359, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Sinh();
            Assert.AreEqual(-0.007085515596552, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(1.002486619151843, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Sinh();
            Assert.AreEqual(-0.497821359650232, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-0.333236258274482, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Sinh();
            Assert.AreEqual(0.188822924767051, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-1.050991473923866, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Sinh(number);
            Assert.AreEqual(0.188822924767051, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-1.050991473923866, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Tanh_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Tanh();
            Assert.AreEqual(0.109101411029079, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(0.305972552334617, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Tanh();
            Assert.AreEqual(-6.694628865714378, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(4.692385204651136, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Tanh();
            Assert.AreEqual(-0.496197065773508, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-0.238405083338123, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Tanh();
            Assert.AreEqual(1.458632584854141, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-0.838369302507491, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Tanh(number);
            Assert.AreEqual(1.458632584854141, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-0.838369302507491, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Coth_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Coth();
            Assert.AreEqual(1.033917851082447, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-2.899600296789029, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Coth();
            Assert.AreEqual(-0.100164212730932, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-0.070206889624792, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Coth();
            Assert.AreEqual(-1.637351930074588, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(0.786689503563990, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Coth();
            Assert.AreEqual(0.515331906039676, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(0.296194158222197, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Coth(number);
            Assert.AreEqual(0.515331906039676, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(0.296194158222197, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Sech_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Sech();
            Assert.AreEqual(1.040550471593828, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-0.032081132157620, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Sech();
            Assert.AreEqual(4.727709664840343, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(6.644607995649714, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Sech();
            Assert.AreEqual(0.909689950634526, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-0.130039803930286, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Sech();
            Assert.AreEqual(1.014299730743966, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(1.205632561769404, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Sech(number);
            Assert.AreEqual(1.014299730743966, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(1.205632561769404, target.Imag, 1e-15, "#4 imag");
        }

        [TestMethod]
        public void Complex_Cosech_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Csch();
            Assert.AreEqual(0.982821247207556, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-3.050349711478128, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Csch();
            Assert.AreEqual(-0.007050056448563, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-0.997469719407415, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Csch();
            Assert.AreEqual(-1.387181647643423, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(0.928564459613600, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Csch();
            Assert.AreEqual(0.165599691781259, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(0.921730580972834, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Csch(number);
            Assert.AreEqual(0.165599691781259, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(0.921730580972834, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Acosh_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Acosh();
            Assert.AreEqual(0.296999023408390, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(1.474903367519443, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Acosh();
            Assert.AreEqual(1.196042838552249, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(1.626235699131362, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Acosh();
            Assert.AreEqual(0.334299817774938, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-2.063835567380515, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Acosh();
            Assert.AreEqual(1.055304915437955, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-1.255110336239894, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Acosh(number);
            Assert.AreEqual(1.055304915437955, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-1.255110336239894, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Asinh_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Asinh();
            Assert.AreEqual(0.104581498839370, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(0.302981107347608, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Asinh();
            Assert.AreEqual(-0.967727114204379, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(1.481869611649372, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Asinh();
            Assert.AreEqual(-0.497902942830288, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-0.269555641424950, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Asinh();
            Assert.AreEqual(0.864263503049688, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-1.032909408267385, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Asinh(number);
            Assert.AreEqual(0.864263503049688, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-1.032909408267385, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Atanh_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Atanh();
            Assert.AreEqual(0.091931195031329, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(0.294001301773784, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Atanh();
            Assert.AreEqual(-0.030713418276336, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(0.984212159158513, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Atanh();
            Assert.AreEqual(-0.482240147685385, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(-0.368907530060232, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Atanh();
            Assert.AreEqual(0.195224482279363, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(-0.925373074659344, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Atanh(number);
            Assert.AreEqual(0.195224482279363, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-0.925373074659344, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Acoth_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Acoth();
            Assert.AreEqual(0.091931195031329, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-1.276795025021113, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Acoth();
            Assert.AreEqual(-0.030713418276336, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-0.586584167636384, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Acoth();
            Assert.AreEqual(-0.482240147685385, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(1.201888796734664, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Acoth();
            Assert.AreEqual(0.195224482279363, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(0.645423252135553, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Acoth(number);
            Assert.AreEqual(0.195224482279363, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(0.645423252135553, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Asech_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Asech();
            Assert.AreEqual(1.864161544157883, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-1.263192677264185, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Asech();
            Assert.AreEqual(0.623065008994206, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-1.607663511869202, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Asech();
            Assert.AreEqual(1.200699546128141, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(2.517873627505525, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Asech();
            Assert.AreEqual(0.678053948499639, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(1.329643731985686, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Asech(number);
            Assert.AreEqual(0.678053948499639, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(1.329643731985686, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Acosech_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Acsch();
            Assert.AreEqual(1.824198702193883, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-1.233095217529344, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Acsch();
            Assert.AreEqual(-0.059040931232830, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-0.724233722076607, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Acsch();
            Assert.AreEqual(-1.276772226623233, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(0.474289102065753, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Acsch();
            Assert.AreEqual(0.384546338338748, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(0.721631195207434, target.Imag, 1e-15, "#4 imag");

            // Static
            target = Complex.Acsch(number);
            Assert.AreEqual(0.384546338338748, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(0.721631195207434, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Real_Accessor()
        {
            const double expected = 12.3;
            var target = new Complex(expected, 45.6);
            Assert.AreEqual(expected, target.Real, "#1");

            target = new Complex(expected);
            Assert.AreEqual(expected, target.Real, "#2");

            target.Imag += expected;
            Assert.AreEqual(expected * 2, target.Real, "#3");
        }

        [TestMethod]
        public void Complex_Imag_Accessor()
        {
            const double expected = 45.6;
            var target = new Complex(12.3, expected);
            Assert.AreEqual(expected, target.Imag, "#1");

            target = new Complex(78.9);
            Assert.AreEqual(0, target.Imag, "#2");

            target.Imag = expected;
            Assert.AreEqual(expected, target.Imag, "#3");
        }

        [TestMethod]
        public void Complex_IsImaginaryOne_Correctness()
        {
            Assert.IsFalse(new Complex(2, 3).IsImaginaryOne, "#1");

            Assert.IsFalse(Complex.Infinity.IsImaginaryOne, "#2");
            Assert.IsFalse(Complex.NaN.IsImaginaryOne, "#3");
            Assert.IsFalse(Complex.One.IsImaginaryOne, "#4");
            Assert.IsFalse(Complex.Zero.IsImaginaryOne, "#5");

            Assert.IsTrue(Complex.ImaginaryOne.IsImaginaryOne, "#6");
            Assert.IsTrue(new Complex(0, 1).IsImaginary, "#7");
        }

        [TestMethod]
        public void Complex_IsImaginary_Correctness()
        {
            Assert.IsFalse(new Complex(2, 3).IsImaginary, "#1");
            Assert.IsFalse(new Complex(0, 0).IsImaginary, "#2");

            Assert.IsFalse(Complex.Infinity.IsImaginary, "#3");
            Assert.IsFalse(Complex.NaN.IsImaginary, "#4");
            Assert.IsFalse(Complex.One.IsImaginary, "#5");
            Assert.IsFalse(Complex.Zero.IsImaginary, "#6");

            Assert.IsTrue(Complex.ImaginaryOne.IsImaginary, "#7");
            Assert.IsTrue(new Complex(0, 4).IsImaginary, "#8");
        }

        [TestMethod]
        public void Complex_IsInfinity_Correctness()
        {
            Assert.IsFalse(new Complex(2, 3).IsInfinity, "#1");

            Assert.IsFalse(Complex.NaN.IsInfinity, "#2");
            Assert.IsFalse(Complex.One.IsInfinity, "#3");
            Assert.IsFalse(Complex.ImaginaryOne.IsInfinity, "#4");
            Assert.IsFalse(Complex.Zero.IsInfinity, "#5");

            Assert.IsTrue(Complex.Infinity.IsInfinity, "#6");
        }

        [TestMethod]
        public void Complex_IsNaN_Correctness()
        {
            Assert.IsFalse(new Complex(2, 3).IsNaN, "#1");

            Assert.IsFalse(Complex.Infinity.IsNaN, "#2");
            Assert.IsFalse(Complex.One.IsNaN, "#3");
            Assert.IsFalse(Complex.ImaginaryOne.IsNaN, "#4");
            Assert.IsFalse(Complex.Zero.IsNaN, "#5");

            Assert.IsTrue(Complex.NaN.IsNaN, "#6");
        }

        [TestMethod]
        public void Complex_IsOne_Correctness()
        {
            Assert.IsFalse(new Complex(2, 3).IsOne, "#1");

            Assert.IsFalse(Complex.ImaginaryOne.IsOne, "#2");
            Assert.IsFalse(Complex.Zero.IsOne, "#3");
            Assert.IsFalse(Complex.Infinity.IsOne, "#4");
            Assert.IsFalse(Complex.NaN.IsOne, "#5");

            Assert.IsTrue(Complex.One.IsOne, "#6");
        }

        [TestMethod]
        public void Complex_IsReal_Correctness()
        {
            Assert.IsFalse(new Complex(2, 3).IsReal, "#1");
            Assert.IsTrue(new Complex(2, 0).IsReal, "#2");
            Assert.IsTrue(new Complex(-2, 0).IsReal, "#3");

            Assert.IsFalse(Complex.ImaginaryOne.IsReal, "#4");
            Assert.IsFalse(Complex.Infinity.IsReal, "#5");
            Assert.IsFalse(Complex.NaN.IsReal, "#6");

            Assert.IsTrue(Complex.One.IsReal, "#7");
            Assert.IsTrue(Complex.Zero.IsReal, "#8");
        }

        [TestMethod]
        public void Complex_IsRealNonNegative_Correctness()
        {
            Assert.IsFalse(new Complex(2, 3).IsRealNonNegative, "#1");
            Assert.IsTrue(new Complex(2, 0).IsRealNonNegative, "#2");
            Assert.IsFalse(new Complex(-2, 0).IsRealNonNegative, "#3");

            Assert.IsFalse(Complex.ImaginaryOne.IsRealNonNegative, "#4");
            Assert.IsFalse(Complex.Infinity.IsRealNonNegative, "#5");
            Assert.IsFalse(Complex.NaN.IsRealNonNegative, "#6");

            Assert.IsTrue(Complex.One.IsRealNonNegative, "#7");
            Assert.IsTrue(Complex.Zero.IsRealNonNegative, "#8");
        }

        [TestMethod]
        public void Complex_IsZero_Correctness()
        {
            Assert.IsFalse(new Complex(2, 3).IsZero, "#1");
            Assert.IsFalse(new Complex(2, 0).IsZero, "#2");
            Assert.IsFalse(new Complex(-2, 0).IsZero, "#3");

            Assert.IsFalse(Complex.ImaginaryOne.IsZero, "#4");
            Assert.IsFalse(Complex.One.IsZero, "#7");
            Assert.IsFalse(Complex.Infinity.IsZero, "#5");
            Assert.IsFalse(Complex.NaN.IsZero, "#6");

            Assert.IsTrue(Complex.Zero.IsZero, "#8");
            Assert.IsTrue(new Complex(0, 0).IsZero, "#9");
        }

        [TestMethod]
        public void Complex_One_Correctness()
        {
            Assert.IsFalse(Complex.One.IsNaN, "#1");
            Assert.IsFalse(Complex.One.IsInfinity, "#2");
            Assert.IsFalse(Complex.One.IsImaginary, "#3");
            Assert.IsFalse(Complex.One.IsImaginaryOne, "#4");
            Assert.IsFalse(Complex.One.IsZero, "#5");

            Assert.IsTrue(Complex.One.IsReal, "#6");
            Assert.IsTrue(Complex.One.IsRealNonNegative, "#7");
            Assert.IsTrue(Complex.One.IsOne, "#8");

            Assert.AreEqual(1, Complex.One.Real, "#9 real");
            Assert.AreEqual(0, Complex.One.Imag, "#9 imag");
        }

        [TestMethod]
        public void Complex_Zero_Correctness()
        {
            Assert.IsFalse(Complex.Zero.IsNaN, "#1");
            Assert.IsFalse(Complex.Zero.IsInfinity, "#2");
            Assert.IsFalse(Complex.Zero.IsImaginary, "#3");
            Assert.IsFalse(Complex.Zero.IsImaginaryOne, "#4");
            Assert.IsFalse(Complex.Zero.IsOne, "#5");

            Assert.IsTrue(Complex.Zero.IsReal, "#6");
            Assert.IsTrue(Complex.Zero.IsRealNonNegative, "#7");
            Assert.IsTrue(Complex.Zero.IsZero, "#8");

            Assert.AreEqual(0, Complex.Zero.Real, "#9 real");
            Assert.AreEqual(0, Complex.Zero.Imag, "#9 imag");
        }

        [TestMethod]
        public void Complex_ImaginaryOne_Correctness()
        {
            Assert.IsFalse(Complex.ImaginaryOne.IsNaN, "#1");
            Assert.IsFalse(Complex.ImaginaryOne.IsInfinity, "#2");
            Assert.IsFalse(Complex.ImaginaryOne.IsReal, "#3");
            Assert.IsFalse(Complex.ImaginaryOne.IsRealNonNegative, "#4");
            Assert.IsFalse(Complex.ImaginaryOne.IsZero, "#5");
            Assert.IsFalse(Complex.ImaginaryOne.IsOne, "#6");

            Assert.IsTrue(Complex.ImaginaryOne.IsImaginary, "#7");
            Assert.IsTrue(Complex.ImaginaryOne.IsImaginaryOne, "#8");

            Assert.AreEqual(0, Complex.ImaginaryOne.Real, "#9 real");
            Assert.AreEqual(1, Complex.ImaginaryOne.Imag, "#9 imag");
        }

        [TestMethod]
        public void Complex_Infinity_Correctness()
        {
            Assert.IsFalse(Complex.Infinity.IsNaN, "#1");
            Assert.IsFalse(Complex.Infinity.IsImaginary, "#2");
            Assert.IsFalse(Complex.Infinity.IsImaginaryOne, "#3");
            Assert.IsFalse(Complex.Infinity.IsZero, "#4");
            Assert.IsFalse(Complex.Infinity.IsOne, "#5");
            Assert.IsFalse(Complex.Infinity.IsReal, "#6");
            Assert.IsFalse(Complex.Infinity.IsRealNonNegative, "#7");

            Assert.IsTrue(Complex.Infinity.IsInfinity, "#8");

            Assert.IsFalse(double.IsNegativeInfinity(Complex.Infinity.Real), "#9 real");
            Assert.IsFalse(double.IsNegativeInfinity(Complex.Infinity.Imag), "#9 imag");

            Assert.IsTrue(double.IsPositiveInfinity(Complex.Infinity.Real), "#10 real");
            Assert.IsTrue(double.IsPositiveInfinity(Complex.Infinity.Imag), "#10 imag");

            Assert.IsTrue(double.IsInfinity(Complex.Infinity.Real), "#11 real");
            Assert.IsTrue(double.IsInfinity(Complex.Infinity.Imag), "#11 imag");
        }

        [TestMethod]
        public void Complex_NaN_Correctness()
        {
            Assert.IsFalse(Complex.NaN.IsInfinity, "#1");
            Assert.IsFalse(Complex.NaN.IsImaginary, "#2");
            Assert.IsFalse(Complex.NaN.IsImaginaryOne, "#3");
            Assert.IsFalse(Complex.NaN.IsZero, "#4");
            Assert.IsFalse(Complex.NaN.IsOne, "#5");
            Assert.IsFalse(Complex.NaN.IsReal, "#6");
            Assert.IsFalse(Complex.NaN.IsRealNonNegative, "#7");

            Assert.IsTrue(Complex.NaN.IsNaN, "#8");

            Assert.IsTrue(double.IsNaN(Complex.NaN.Real), "#9 real");
            Assert.IsTrue(double.IsNaN(Complex.NaN.Imag), "#9 imag");
        }

        [TestMethod]
        public void Complex_Argument_Correctness()
        {
            Assert.AreEqual(1.373, new Complex(1, 5).Argument, 0.001, "#1");
            Assert.AreEqual(0.785, new Complex(1, 1).Argument, 0.001, "#2");
            Assert.AreEqual(2.356, new Complex(-1, 1).Argument, 0.001, "#3");
            Assert.AreEqual(3.927, new Complex(-1, -1).Argument, 0.001, "#4");
            Assert.AreEqual(5.498, new Complex(1, -1).Argument, 0.001, "#5");
        }

        [TestMethod]
        public void Complex_Conjugate_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Conjugate;
            Assert.AreEqual(0.100000000000000, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-0.300000000000000, target.Imag, 1e-15, "#1 imag");

            number = new Complex(-0.1, 1.5);
            target = number.Conjugate;
            Assert.AreEqual(-0.100000000000000, target.Real, 1e-15, "#2 real");
            Assert.AreEqual(-1.500000000000000, target.Imag, 1e-15, "#2 imag");

            number = new Complex(-0.5, -0.3);
            target = number.Conjugate;
            Assert.AreEqual(-0.500000000000000, target.Real, 1e-15, "#3 real");
            Assert.AreEqual(0.300000000000000, target.Imag, 1e-15, "#3 imag");

            number = new Complex(0.5, -1.2);
            target = number.Conjugate;
            Assert.AreEqual(0.500000000000000, target.Real, 1e-15, "#4 real");
            Assert.AreEqual(1.200000000000000, target.Imag, 1e-15, "#4 imag");

            number = new Complex(123.4, 567.8);
            target.Conjugate = number;
            Assert.AreEqual(123.4, target.Real, 1e-15, "#5 real");
            Assert.AreEqual(-567.8, target.Imag, 1e-15, "#5 imag");
        }

        [TestMethod]
        public void Complex_Modulus_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            double target = number.Modulus;
            Assert.AreEqual(0.316227766016838, target, 1e-15, "#1");

            number = new Complex(-0.1, 1.5);
            target = number.Modulus;
            Assert.AreEqual(1.503329637837291, target, 1e-15, "#2");

            number = new Complex(-0.5, -0.3);
            target = number.Modulus;
            Assert.AreEqual(0.583095189484530, target, 1e-15, "#3");

            number = new Complex(0.5, -1.2);
            target = number.Modulus;
            Assert.AreEqual(1.300000000000000, target, 1e-15, "#4");
        }

        [TestMethod]
        public void Complex_ModulusSquared_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            double target = number.ModulusSquared;
            Assert.AreEqual(0.100000000000000, target, 1e-15, "#1");

            number = new Complex(-0.1, 1.5);
            target = number.ModulusSquared;
            Assert.AreEqual(2.260000000000000, target, 1e-15, "#2");

            number = new Complex(-0.5, -0.3);
            target = number.ModulusSquared;
            Assert.AreEqual(0.340000000000000, target, 1e-15, "#3");

            number = new Complex(0.5, -1.2);
            target = number.ModulusSquared;
            Assert.AreEqual(1.690000000000000, target, 1e-15, "#4");
        }

        [TestMethod]
        public void Complex_Sign_Matlab_Conformance()
        {
            var number = new Complex(0.1, 0.3);
            Complex target = number.Sign;

            // Assert.AreEqual(1.249045772398254, target.Real, 1e-15, "#1 real")
            // Assert.AreEqual(0, target.Imag, 1e-15, "#1 imag")
            Assert.IsFalse(target.IsNaN);

            number = new Complex(-0.1, 1.5);
            target = number.Sign;

            // Assert.AreEqual(1.637364490570721, target.Real, 1e-15, "#2 real")
            // Assert.AreEqual(0, target.Imag, 1e-15, "#2 imag")
            Assert.IsFalse(target.IsNaN);

            number = new Complex(-0.5, -0.3);
            target = number.Sign;

            // Assert.AreEqual(-2.601173153319209, target.Real, 1e-15, "#3 real")
            // Assert.AreEqual(0, target.Imag, 1e-15, "#3 imag")
            Assert.IsFalse(target.IsNaN);

            number = new Complex(0.5, -1.2);
            target = number.Sign;

            // Assert.AreEqual(-1.176005207095135, target.Real, 1e-15, "#4 real")
            // Assert.AreEqual(0, target.Imag, 1e-15, "#4 imag")
            Assert.IsFalse(target.IsNaN);
        }

        [TestMethod]
        public void Complex_Operator_Addition_DoubleComplex_Correctness()
        {
            const double lhs = 1;
            const double rhsR = 1;
            const double rhsI = 5;
            var rhs = new Complex(rhsR, rhsI);

            Complex target = lhs + rhs;

            Assert.AreEqual(lhs + rhsR, target.Real, "#1 real");
            Assert.AreEqual(rhsI, target.Imag, "#1 imag");
        }

        [TestMethod]
        public void Complex_Operator_Addition_ComplexDouble_Correctness()
        {
            const double rhs = 1;
            const double lhsR = 5;
            const double lhsI = 7;
            var lhs = new Complex(lhsR, lhsI);

            Complex target = lhs + rhs;

            Assert.AreEqual(lhsR + rhs, target.Real, "#1 real");
            Assert.AreEqual(lhsI, target.Imag, "#1 imag");
        }

        [TestMethod]
        public void Complex_Operator_Addition_ComplexComplex_Correctness()
        {
            var lhs = new Complex(2, 3);
            var rhs = new Complex(1, 5);

            Complex target = lhs + rhs;

            Assert.AreEqual(lhs.Real + rhs.Real, target.Real, "#1 real");
            Assert.AreEqual(lhs.Imag + rhs.Imag, target.Imag, "#1 imag");
        }

        [TestMethod]
        public void Complex_Operator_Division_DoubleComplex_Correctness()
        {
            const double lhs = 7.5;
            var rhs = new Complex(4, 7);

            Complex target = lhs / rhs;

            Assert.AreEqual(0.461538461538462, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-0.807692307692308, target.Imag, 1e-15, "#1 imag");
        }

        [TestMethod]
        public void Complex_Operator_Division_ComplexDouble_Correctness()
        {
            var lhs = new Complex(4, 7);
            const double rhs = 7.5;

            Complex target = lhs / rhs;

            Assert.AreEqual(0.533333333333333, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(0.933333333333333, target.Imag, 1e-15, "#1 imag");
        }

        [TestMethod]
        public void Complex_Operator_Division_ComplexComplex_Correctness()
        {
            var lhs = new Complex(3, 5);
            var rhs = new Complex(4, 7);

            Complex target = lhs / rhs;

            Assert.AreEqual(0.723076923076923, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(-0.015384615384615, target.Imag, 1e-15, "#1 imag");
        }

        [TestMethod]
        public void Complex_Operator_Equality_DoubleComplex_Correctness()
        {
            // ReSharper disable once ConvertToConstant.Local
            var lhs = 4.5;
            var rhs = new Complex(4.5, 0);
            Assert.IsTrue(lhs == rhs, "#1");

            rhs = new Complex(4.5, 6);
            Assert.IsFalse(lhs == rhs, "#2");

            rhs = new Complex(8, 0);
            Assert.IsFalse(lhs == rhs, "#3");

            rhs = new Complex(7, 6);
            Assert.IsFalse(lhs == rhs, "#4");
        }

        [TestMethod]
        public void Complex_Operator_Equality_ComplexDouble_Correctness()
        {
            var lhs = new Complex(4.5, 0);
            double rhs = 4.5;
            Assert.IsTrue(lhs == rhs, "#1");

            rhs = 5.4;
            Assert.IsFalse(lhs == rhs, "#2");
        }

        [TestMethod]
        public void Complex_Operator_Equality_ComplexComplex_Correctness()
        {
            var lhs = new Complex(3, 7);
            var rhs = new Complex(3, 7);
            Assert.IsTrue(lhs == rhs, "#1");

            rhs = new Complex(3, 6);
            Assert.IsFalse(lhs == rhs, "#2");

            rhs = new Complex(4, 7);
            Assert.IsFalse(lhs == rhs, "#3");

            rhs = new Complex(2, 1);
            Assert.IsFalse(lhs == rhs, "#4");
        }

        [TestMethod]
        public void Complex_Operator_Implicit_Correctness()
        {
            Complex target = 2.34;
            Assert.AreEqual(2.34, target.Real, "#1 real");
            Assert.AreEqual(0, target.Imag, "#1 imag");
        }

        [TestMethod]
        public void Complex_Operator_Inequality_DoubleComplex_Correctness()
        {
            // ReSharper disable once ConvertToConstant.Local
            double lhs = 4.5;
            var rhs = new Complex(4.5, 0);
            Assert.IsFalse(lhs != rhs, "#1");

            rhs = new Complex(4.5, 6);
            Assert.IsTrue(lhs != rhs, "#2");

            rhs = new Complex(8, 0);
            Assert.IsTrue(lhs != rhs, "#3");

            rhs = new Complex(7, 6);
            Assert.IsTrue(lhs != rhs, "#4");
        }

        [TestMethod]
        public void Complex_Operator_Inequality_ComplexDouble_Correctness()
        {
            var lhs = new Complex(4.5, 0);
            double rhs = 4.5;
            Assert.IsFalse(lhs != rhs, "#1");

            rhs = 5.4;
            Assert.IsTrue(lhs != rhs, "#2");
        }

        [TestMethod]
        public void Complex_Operator_Inequality_ComplexComplex_Correctness()
        {
            var lhs = new Complex(3, 7);
            var rhs = new Complex(3, 7);
            Assert.IsFalse(lhs != rhs, "#1");

            rhs = new Complex(3, 6);
            Assert.IsTrue(lhs != rhs, "#2");

            rhs = new Complex(4, 7);
            Assert.IsTrue(lhs != rhs, "#3");

            rhs = new Complex(2, 1);
            Assert.IsTrue(lhs != rhs, "#4");
        }

        [TestMethod]
        public void Complex_Operator_Multiplication_DoubleComplex_Correctness()
        {
            // ReSharper disable once ConvertToConstant.Local
            double lhs = 7.5;
            var rhs = new Complex(4, 7);

            Complex target = lhs * rhs;

            Assert.AreEqual(30.000000000000000, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(52.500000000000000, target.Imag, 1e-15, "#1 imag");
        }

        [TestMethod]
        public void Complex_Operator_Multiplication_ComplexDouble_Correctness()
        {
            var lhs = new Complex(4, 7);
            const double rhs = 7.5;

            Complex target = lhs * rhs;

            Assert.AreEqual(30.000000000000000, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(52.500000000000000, target.Imag, 1e-15, "#1 imag");
        }

        [TestMethod]
        public void Complex_Operator_Multiplication_ComplexComplex_Correctness()
        {
            var lhs = new Complex(3, 5);
            var rhs = new Complex(4, 7);

            Complex target = lhs * rhs;

            Assert.AreEqual(-23.000000000000000, target.Real, 1e-15, "#1 real");
            Assert.AreEqual(41.000000000000000, target.Imag, 1e-15, "#1 imag");
        }

        [TestMethod]
        public void Complex_Operator_Subtraction_DoubleComplex_Correctness()
        {
            // ReSharper disable once ConvertToConstant.Local
            double lhs = 1;
            var rhs = new Complex(2, 5);

            Complex target = lhs - rhs;

            Assert.AreEqual(-1, target.Real, "#1 real");
            Assert.AreEqual(-5, target.Imag, "#1 imag");
        }

        [TestMethod]
        public void Complex_Operator_Subtraction_ComplexDouble_Correctness()
        {
            var lhs = new Complex(5, 7);
            const double rhs = 1;

            Complex target = lhs - rhs;

            Assert.AreEqual(4, target.Real, "#1 real");
            Assert.AreEqual(7, target.Imag, "#1 imag");
        }

        [TestMethod]
        public void Complex_Operator_Subtraction_ComplexComplex_Correctness()
        {
            var lhs = new Complex(2, 3);
            var rhs = new Complex(1, 5);

            Complex target = lhs - rhs;

            Assert.AreEqual(1, target.Real, "#1 real");
            Assert.AreEqual(-2, target.Imag, "#1 imag");
        }

        [TestMethod]
        public void Complex_Operator_UnaryNegation_Correctness()
        {
            var subtrahend = new Complex(2, 3);
            Complex target = -subtrahend;
            Assert.AreEqual(-2, target.Real, "#1 real");
            Assert.AreEqual(-3, target.Imag, "#1 imag");

            subtrahend = new Complex(-2, -3);
            target = -subtrahend;
            Assert.AreEqual(2, target.Real, "#2 real");
            Assert.AreEqual(3, target.Imag, "#2 imag");
        }

        [TestMethod]
        public void Complex_Operator_UnaryPlus_Correctness()
        {
            var subtrahend = new Complex(2, 3);
            Complex target = +subtrahend;
            Assert.AreEqual(2, target.Real, "#1 real");
            Assert.AreEqual(3, target.Imag, "#1 imag");

            subtrahend = new Complex(-2, -3);
            target = +subtrahend;
            Assert.AreEqual(-2, target.Real, "#2 real");
            Assert.AreEqual(-3, target.Imag, "#2 imag");
        }

        [TestMethod]
        public void Complex_ToString_Correctness()
        {
            Assert.AreEqual("Infinity", new Complex(0, double.PositiveInfinity).ToString(), "#1");

            Assert.AreEqual("Infinity", new Complex(0, double.NegativeInfinity).ToString(), "#2");

            Assert.AreEqual("i", new Complex(0, 1).ToString(), "#3");

            Assert.AreEqual("-i", new Complex(0, -1).ToString(), "#4");

            Assert.AreEqual("1", new Complex(1, 0).ToString(), "#5");

            Assert.AreEqual("-1", new Complex(-1, 0).ToString(), "#6");

            Assert.AreEqual("1+i", new Complex(1, 1).ToString(), "#7");

            Assert.AreEqual("-1+i", new Complex(-1, 1).ToString(), "#8");

            Assert.AreEqual("1-i", new Complex(1, -1).ToString(), "#9");

            Assert.AreEqual("-1-i", new Complex(-1, -1).ToString(), "#10");

            Assert.AreEqual("1+2i", new Complex(1, 2).ToString(), "#11");

            Assert.AreEqual("-1+2i", new Complex(-1, 2).ToString(), "#12");

            Assert.AreEqual("1-2i", new Complex(1, -2).ToString(), "#13");

            Assert.AreEqual("-1-2i", new Complex(-1, -2).ToString(), "#14");
        }

        [TestMethod]
        public void Complex_ToString_NumberFormatInfo_Correctness()
        {
            NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;

            Assert.AreEqual("Infinity", new Complex(0, double.PositiveInfinity).ToString(nfi), "#1");

            Assert.AreEqual("Infinity", new Complex(0, double.NegativeInfinity).ToString(nfi), "#2");

            Assert.AreEqual("i", new Complex(0, 1).ToString(nfi), "#3");

            Assert.AreEqual("-i", new Complex(0, -1).ToString(nfi), "#4");

            Assert.AreEqual("1", new Complex(1, 0).ToString(nfi), "#5");

            Assert.AreEqual("-1", new Complex(-1, 0).ToString(nfi), "#6");

            Assert.AreEqual("1+i", new Complex(1, 1).ToString(nfi), "#7");

            Assert.AreEqual("-1+i", new Complex(-1, 1).ToString(nfi), "#8");

            Assert.AreEqual("1-i", new Complex(1, -1).ToString(nfi), "#9");

            Assert.AreEqual("-1-i", new Complex(-1, -1).ToString(nfi), "#10");

            Assert.AreEqual("1+2i", new Complex(1, 2).ToString(nfi), "#11");

            Assert.AreEqual("-1+2i", new Complex(-1, 2).ToString(nfi), "#12");

            Assert.AreEqual("1-2i", new Complex(1, -2).ToString(nfi), "#13");

            Assert.AreEqual("-1-2i", new Complex(-1, -2).ToString(nfi), "#14");
        }

        [TestMethod]
        public void Complex_ToString_Format_NumberFormatInfo_Correctness()
        {
            const string fmt = "0.0##e+00";
            NumberFormatInfo nfi = CultureInfo.InvariantCulture.NumberFormat;

            Assert.AreEqual("Infinity", new Complex(0, double.PositiveInfinity).ToString(fmt, nfi), "#1");

            Assert.AreEqual("Infinity", new Complex(0, double.NegativeInfinity).ToString(fmt, nfi), "#2");

            Assert.AreEqual("i", new Complex(0, 1).ToString(fmt, nfi), "#3");

            Assert.AreEqual("-i", new Complex(0, -1).ToString(fmt, nfi), "#4");

            Assert.AreEqual("1.0e+00", new Complex(1, 0).ToString(fmt, nfi), "#5");

            Assert.AreEqual("-1.0e+00", new Complex(-1, 0).ToString(fmt, nfi), "#6");

            Assert.AreEqual("1.0e+00+i", new Complex(1, 1).ToString(fmt, nfi), "#7");

            Assert.AreEqual("-1.0e+00+i", new Complex(-1, 1).ToString(fmt, nfi), "#8");

            Assert.AreEqual("1.0e+00-i", new Complex(1, -1).ToString(fmt, nfi), "#9");

            Assert.AreEqual("-1.0e+00-i", new Complex(-1, -1).ToString(fmt, nfi), "#10");

            Assert.AreEqual("1.0e+00+2.0e+00i", new Complex(1, 2).ToString(fmt, nfi), "#11");

            Assert.AreEqual("-1.0e+00+2.0e+00i", new Complex(-1, 2).ToString(fmt, nfi), "#12");

            Assert.AreEqual("1.0e+00-2.0e+00i", new Complex(1, -2).ToString(fmt, nfi), "#13");

            Assert.AreEqual("-1.0e+00-2.0e+00i", new Complex(-1, -2).ToString(fmt, nfi), "#14");
        }

        [TestMethod]
        public void Complex_ToString_Format_Correctness()
        {
            const string fmt = "0.0##e+00";

            Assert.AreEqual("Infinity", new Complex(0, double.PositiveInfinity).ToString(fmt), "#1");

            Assert.AreEqual("Infinity", new Complex(0, double.NegativeInfinity).ToString(fmt), "#2");

            Assert.AreEqual("i", new Complex(0, 1).ToString(fmt), "#3");

            Assert.AreEqual("-i", new Complex(0, -1).ToString(fmt), "#4");

            Assert.AreEqual("1,0e+00", new Complex(1, 0).ToString(fmt), "#5");

            Assert.AreEqual("-1,0e+00", new Complex(-1, 0).ToString(fmt), "#6");

            Assert.AreEqual("1,0e+00+i", new Complex(1, 1).ToString(fmt), "#7");

            Assert.AreEqual("-1,0e+00+i", new Complex(-1, 1).ToString(fmt), "#8");

            Assert.AreEqual("1,0e+00-i", new Complex(1, -1).ToString(fmt), "#9");

            Assert.AreEqual("-1,0e+00-i", new Complex(-1, -1).ToString(fmt), "#10");

            Assert.AreEqual("1,0e+00+2,0e+00i", new Complex(1, 2).ToString(fmt), "#11");

            Assert.AreEqual("-1,0e+00+2,0e+00i", new Complex(-1, 2).ToString(fmt), "#12");

            Assert.AreEqual("1,0e+00-2,0e+00i", new Complex(1, -2).ToString(fmt), "#13");

            Assert.AreEqual("-1,0e+00-2,0e+00i", new Complex(-1, -2).ToString(fmt), "#14");
        }

        [TestMethod]
        public void Complex_Equals_Correctness()
        {
            var lhs = new Complex(3, 7);
            var rhs = new Complex(3, 7);
            Assert.IsTrue(lhs.Equals(rhs), "#1");

            rhs = new Complex(3, 6);
            Assert.IsFalse(lhs.Equals(rhs), "#2");

            rhs = new Complex(4, 7);
            Assert.IsFalse(lhs.Equals(rhs), "#3");

            rhs = new Complex(2, 1);
            Assert.IsFalse(lhs.Equals(rhs), "#4");
        }

        [TestMethod]
        public void Complex_CompareTo_Correctness()
        {
            var lhs = Complex.FromModulusArgument(0.456, 0.345);
            var rhs = Complex.FromModulusArgument(0.457, 0.345);
            Assert.IsTrue(lhs.CompareTo(rhs) < 0, "#1");

            rhs = Complex.FromModulusArgument(0.455, 0.345);
            Assert.IsTrue(lhs.CompareTo(rhs) > 0, "#2");

            rhs = Complex.FromModulusArgument(0.456, 0.346);
            Assert.IsTrue(lhs.CompareTo(rhs) < 0, "#3");

            rhs = Complex.FromModulusArgument(0.456, 0.344);
            Assert.IsTrue(lhs.CompareTo(rhs) > 0, "#4");

            rhs = Complex.FromModulusArgument(0.456, 0.345);
            Assert.IsTrue(lhs.CompareTo(rhs) == 0, "#5");
        }

        [TestMethod]
        public void Complex_GetHashCode_Correctness()
        {
            var target = new Complex(0, 0);
            Assert.AreEqual(0, target.GetHashCode(), "#1");
            Assert.AreNotEqual(Complex.One.GetHashCode(), Complex.ImaginaryOne.GetHashCode(), "#2");
            Assert.AreNotEqual(Complex.One.GetHashCode(), (-Complex.ImaginaryOne).GetHashCode(), "#3");
            Assert.AreNotEqual((-Complex.One).GetHashCode(), Complex.ImaginaryOne.GetHashCode(), "#4");
            Assert.AreNotEqual((-Complex.One).GetHashCode(), (-Complex.ImaginaryOne).GetHashCode(), "#5");
        }

        private static double Normalize2Pi(double value)
        {
            while (value < 0d)
            {
                value += Constants.Pi * 2d;
            }

            return Math.IEEERemainder(value, Constants.Pi * 2d);
        }
    }
}
