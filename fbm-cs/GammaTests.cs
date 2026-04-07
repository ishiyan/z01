using System;
using System.Collections.Generic;
using Mbs.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ef = Mbs.Numerics.ElementaryFunctions;
using Sf = Mbs.Numerics.SpecialFunctions;

#pragma warning disable S2699 // Tests should include assertions

namespace Mbs.UnitTests.Numerics.SpecialFunctions
{
    [TestClass]
    public class GammaTests
    {
        /// <summary>
        /// Verification against Matlab implementation, gammaln(x).
        /// <para />
        /// Matlab defines gammaln(x) only for positive arguments. All negative arguments do return 'Inf'.
        /// </summary>
        [TestMethod]
        public void Gamma_LnGamma_Matlab_Conformance()
        {
            Doubles.AreEqual(double.PositiveInfinity, Sf.LnGamma(1.7976931348623157e+308), "A1: double.MaxValue");
            Doubles.AreEqual(double.NaN, Sf.LnGamma(-1.7976931348623157e+308), "A2: -double.MaxValue"); // Matlab: PositiveInfinity
            Doubles.AreEqual(double.PositiveInfinity, Sf.LnGamma(4.94065645841247e-324), "A3: double.Epsilon"); // Matlab: 7.444400719213812e+002
            Doubles.AreEqual(double.PositiveInfinity, Sf.LnGamma(-4.94065645841247e-324), "A4: -double.Epsilon");
            Doubles.AreEqual(double.NaN, Sf.LnGamma(double.PositiveInfinity), "A5: double.PositiveInfinity"); // Matlab: PositiveInfinity
            Doubles.AreEqual(double.NaN, Sf.LnGamma(double.NegativeInfinity), "A6: double.NegativeInfinity"); // Matlab: PositiveInfinity
            Doubles.AreEqual(double.NaN, Sf.LnGamma(double.NaN), "A7: double.NaN");

            Doubles.AreEqual(8.902086286508580e+260, Sf.LnGamma(1.5e+258), "B1: 1.5e+258");
            Doubles.AreEqual(7.175147466763045e+210, Sf.LnGamma(1.5e+208), "B2: 1.5e+208");
            Doubles.AreEqual(3.721269827271976e+110, Sf.LnGamma(1.5e+108), "B3: 1.5e+108");
            Doubles.AreEqual(1.994331007526442e+060, Sf.LnGamma(1.5e+58), "B4: 1.5e+58");
            Doubles.AreEqual(2.673921869314945e+009, Sf.LnGamma(1.5e+8), "B5: 1.5e+8");
            Doubles.AreEqual(1.292331932426139e+005, Sf.LnGamma(1.5e+4), "B6: 1.5e+4");
            Doubles.AreEqual(7.098116288810610e+002, Sf.LnGamma(171.63), "B7: 171.63");
            Doubles.AreEqual(7.097602049239979e+002, Sf.LnGamma(171.62), "B8: 171.62");
            Doubles.AreEqual(7.066244499063361e+002, Sf.LnGamma(171.01), "B9: 171.01");
            Doubles.AreEqual(7.065730108584204e+002, Sf.LnGamma(170.99999), "B10: 170.99999");
            Doubles.AreEqual(6.035161654536971e+002, Sf.LnGamma(150.69999), "B11: 150.69999");
            Doubles.AreEqual(5.058892174281633e+002, Sf.LnGamma(130.94999), "B12: 130.94999");
            Doubles.AreEqual(4.578123400647524e+002, Sf.LnGamma(120.99999), "B13: 120.99999");
            Doubles.AreEqual(4.088881200422541e+002, Sf.LnGamma(110.69499), "B14: 110.69499");
            Doubles.AreEqual(3.609520062633630e+002, Sf.LnGamma(100.39499), "B15: 100.39499");
            Doubles.AreEqual(3.154423776266905e+002, Sf.LnGamma(90.39799), "B16: 90.39799");
            Doubles.AreEqual(2.705955892247958e+002, Sf.LnGamma(80.29799), "B17: 80.29799");
            Doubles.AreEqual(2.287293896804920e+002, Sf.LnGamma(70.59799), "B18: 70.59799");
            Doubles.AreEqual(1.861654319910171e+002, Sf.LnGamma(60.39899), "B19: 60.39899");
            Doubles.AreEqual(1.456586616191560e+002, Sf.LnGamma(50.27989), "B20: 50.27989");
            Doubles.AreEqual(1.095549578628605e+002, Sf.LnGamma(40.79299), "B21: 40.79299");
            Doubles.AreEqual(72.941192911714737, Sf.LnGamma(30.49639), "B22: 30.49639");
            Doubles.AreEqual(40.214796064625020, Sf.LnGamma(20.29379), "B23: 20.29379");
            Doubles.AreEqual(13.248335692864650, Sf.LnGamma(10.19739), "B24: 10.19739");
            Doubles.AreEqual(11.007785964089353, Sf.LnGamma(9.18739), "B25: 9.18739");
            Doubles.AreEqual(9.124241421119539, Sf.LnGamma(8.29439), "B26: 8.29439");
            Doubles.AreEqual(7.333429979003761, Sf.LnGamma(7.39639), "B27: 7.39639");
            Doubles.AreEqual(6.184587202208638, Sf.LnGamma(6.78739), "B28: 6.78739");
            Doubles.AreEqual(5.110349346715205, Sf.LnGamma(6.18739), "B29: 6.18739");
            Doubles.AreEqual(4.440578049393235, Sf.LnGamma(5.79439), "B30: 5.79439");
            Doubles.AreEqual(3.637107976544816, Sf.LnGamma(5.29839), "B31: 5.29839");
            Doubles.AreEqual(2.858547685152405, Sf.LnGamma(4.78439), "B32: 4.78439");
            Doubles.AreEqual(2.432086638465418, Sf.LnGamma(4.48439), "B33: 4.48439");
            Doubles.AreEqual(2.028118415532899, Sf.LnGamma(4.18439), "B34: 4.18439");
            Doubles.AreEqual(1.648457401376442, Sf.LnGamma(3.88439), "B35: 3.88439");
            Doubles.AreEqual(1.295234648849047, Sf.LnGamma(3.58439), "B36: 3.58439");
            Doubles.AreEqual(0.970988146737627, Sf.LnGamma(3.28439), "B37: 3.28439");
            Doubles.AreEqual(0.580404025495925, Sf.LnGamma(2.87439), "B38: 2.87439");
            Doubles.AreEqual(0.266836500182561, Sf.LnGamma(2.47439), "B39: 2.47439");
            Doubles.AreEqual(0.083196901128458, Sf.LnGamma(2.17439), 1e-15, "B40: 2.17439"); // 0.0831969011284583
            Doubles.AreEqual(-0.047879247660746, Sf.LnGamma(1.87439), 1e-15, "B41: 1.87439"); // -0.0478792476607464
            Doubles.AreEqual(-0.078139343497167, Sf.LnGamma(1.77439), 1e-15, "B42: 1.77439"); // -0.0781393434971674
            Doubles.AreEqual(-0.100886771584016, Sf.LnGamma(1.67439), "B43: 1.67439");
            Doubles.AreEqual(-0.115536340415188, Sf.LnGamma(1.57439), "B44: 1.57439");
            Doubles.AreEqual(-0.121407844747948, Sf.LnGamma(1.47439), "B45: 1.47439");
            Doubles.AreEqual(-0.117701802472050, Sf.LnGamma(1.37439), "B46: 1.37439");
            Doubles.AreEqual(-0.103466469498841, Sf.LnGamma(1.27439), "B47: 1.27439");
            Doubles.AreEqual(-0.077551962727027, Sf.LnGamma(1.17439), "B48: 1.17439");
            Doubles.AreEqual(-0.038544753311976, Sf.LnGamma(1.07439), 1e-15, "B49: 1.07439"); // -0.0385447533119761
            Doubles.AreEqual(0, Sf.LnGamma(1.0), "B50: 1.0");
            Doubles.AreEqual(0.015328775357685, Sf.LnGamma(0.97439), 1e-15, "B51: 0.97439"); // 0.0153287753576852
            Doubles.AreEqual(0.086349530937999, Sf.LnGamma(0.87439), 1e-15, "B52: 0.87439"); // 0.0863495309379989
            Doubles.AreEqual(0.177540312829119, Sf.LnGamma(0.77439), "B53: 0.77439");
            Doubles.AreEqual(0.293059928815666, Sf.LnGamma(0.67439), "B54: 0.67439");
            Doubles.AreEqual(0.438910330455234, Sf.LnGamma(0.57439), "B55: 0.57439");
            Doubles.AreEqual(0.624317666030854, Sf.LnGamma(0.47439), "B56: 0.47439");
            Doubles.AreEqual(0.864755441665062, Sf.LnGamma(0.37439), "B57: 0.37439");
            Doubles.AreEqual(1.189738357444321, Sf.LnGamma(0.27439), "B58: 0.27439");
            Doubles.AreEqual(1.668909145873741, Sf.LnGamma(0.17439), "B59: 0.17439");
            Doubles.AreEqual(2.559889001466911, Sf.LnGamma(0.07439), "B60: 0.07439");
            Doubles.AreEqual(5.425907891993400, Sf.LnGamma(0.00439), "B61: 0.00439");
            Doubles.AreEqual(7.849138829804745, Sf.LnGamma(0.00039), "B62: 0.00039");
            Doubles.AreEqual(9.315648944885858, Sf.LnGamma(0.00009), "B63: 0.00009");
            Doubles.AreEqual(18.420680738180209, Sf.LnGamma(0.00000001), "B64: 0.00000001");
            Doubles.AreEqual(2.325610943923986e+002, Sf.LnGamma(0.1e-100), "B65: 0.1e-100");
            Doubles.AreEqual(4.628196036918032e+002, Sf.LnGamma(0.1e-200), "B66: 0.1e-200");
            Doubles.AreEqual(6.930781129912077e+002, Sf.LnGamma(0.1e-300), "B67: 0.1e-300");
            Doubles.AreEqual(double.PositiveInfinity, Sf.LnGamma(0.1e-322), "B68: 0.1e-322"); // Matlab: 7.437469247408213e+002
            Doubles.AreEqual(double.PositiveInfinity, Sf.LnGamma(0.1e-323), "B69: 0.1e-323");
        }

        [TestMethod]
        public void Gamma_LnGamma_Properties()
        {
            // Duplication.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-4, 1.0e2, 10))
            {
                Doubles.AreEqual(
                    Sf.LnGamma(2 * x),
                    Sf.LnGamma(x) + Sf.LnGamma(x + 0.5) + (2 * x - 0.5) * Math.Log(2d) - 0.5 * Math.Log(Constants.TwoPi),
                    $"Duplication: {x}");
            }

            // Triplication.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-4, 1.0e2, 5))
            {
                Doubles.AreEqual(
                    Sf.LnGamma(3 * x),
                    Sf.LnGamma(x) + Sf.LnGamma(x + 1 / 3d) + Sf.LnGamma(x + 2 / 3d) + (3 * x - 0.5) * Math.Log(3d) - Math.Log(Constants.TwoPi),
                    $"Triplication: {x}");
            }

            // Ratio inequality.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-4, 1.0e2, 20))
            {
                double lower = Math.Log(Math.Sqrt(x + 0.25));
                double upper = Math.Log((x + 0.5) / Math.Sqrt(x + 0.75));
                double ratio = Sf.LnGamma(x + 1d) - Sf.LnGamma(x + 0.5);

                Assert.IsTrue(lower <= ratio, $"Inequality: lower > ratio, ratio({x})={ratio}, lower={lower}");
                Assert.IsTrue(ratio <= upper, $"Inequality: ratio > upper, ratio({x})={ratio}, upper={upper}");
            }
        }

        [TestMethod]
        public void Gamma_LnGamma_ComplexArgument_Properties()
        {
            // Conjugation.
            foreach (Complex z in GenerateComplexLogarithmicSamples(1e-4, 1e+4, 1000))
            {
                if (z.Real <= 0)
                {
                    continue;
                }

                Complex actual = Sf.LnGamma(z.Conjugate);
                Complex expected = Sf.LnGamma(z).Conjugate;

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected.Real, actual.Real),
                    1e-5,
                    $"Conjugation Re: z={z}, actual={actual}, expected={expected}");
                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected.Imag, actual.Imag),
                    1e-5,
                    $"Conjugation Im: z={z}, actual={actual}, expected={expected}");
            }

            // Agreement.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1e-4, 1e+2, 1000))
            {
                Complex actual = Sf.LnGamma(new Complex(x));
                double expected = Sf.LnGamma(x);

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected, actual.Real),
                    1e-5,
                    $"Agreement Re: x={x}, actual={actual}, expected={expected}");
            }
        }

        /// <summary>
        /// Maximal value of the argument of the gamma function.
        /// </summary>
        [TestMethod]
        public void Gamma_Gamma_MaxGammaArgument_PositiveInfinity()
        {
            Assert.IsFalse(double.IsPositiveInfinity(Sf.Gamma(Sf.MaxGammaArgument)));

            Assert.IsTrue(double.IsPositiveInfinity(Sf.Gamma(Sf.MaxGammaArgument + 2e-14)));
        }

        /// <summary>
        /// Verification against Maple implementation.
        /// </summary>
        [TestMethod]
        public void Gamma_Gamma_Maple_Conformance()
        {
            // Ensure poles return NaN.
            Assert.IsTrue(double.IsNaN(Sf.Gamma(0.0)), "A1: gamma(0) should be NaN");
            Assert.IsTrue(double.IsNaN(Sf.Gamma(-1.0)), "A2: gamma(-1) should be NaN");
            Assert.IsTrue(double.IsNaN(Sf.Gamma(-2.0)), "A3: gamma(-2) should be NaN");
            Assert.IsTrue(double.IsNaN(Sf.Gamma(-20.0)), "A4: gamma(-20) should be NaN");
            Assert.IsFalse(double.IsNaN(Sf.Gamma(-20.0000000001)), "A5: gamma(-20.0000000001) should not be NaN");

            // Compare with Maple: "evalf(GAMMA(x),20);"
            Assert.AreEqual(999.42377248459546611, Sf.Gamma(0.001), 1e-15, "B1: gamma(0.001)");
            Assert.AreEqual(99.432585119150603714, Sf.Gamma(0.01), 1e-14, "B2: gamma(0.01)");
            Assert.AreEqual(9.5135076986687318363, Sf.Gamma(0.1), 1e-13, "B3: gamma(0.1)");
            Assert.AreEqual(4.5908437119988030532, Sf.Gamma(0.2), 1e-13, "B4: gamma(0.2)");
            Assert.AreEqual(2.2181595437576882231, Sf.Gamma(0.4), 1e-13, "B5: gamma(0.4)");
            Assert.AreEqual(1.4891922488128171024, Sf.Gamma(0.6), 1e-13, "B6: gamma(0.6)");
            Assert.AreEqual(1.0686287021193193549, Sf.Gamma(0.9), 1e-14, "B7: gamma(0.9)");
            Assert.AreEqual(1.0005782056293586480, Sf.Gamma(0.999), 1e-15, "B8: gamma(0.999)");
            Assert.AreEqual(1.0, Sf.Gamma(1.0), 1e-13, "B9: gamma(1.0)");
            Assert.AreEqual(.99942377248459546611, Sf.Gamma(1.001), 1e-15, "B10: gamma(1.001)");
            Assert.AreEqual(.88622692545275801365, Sf.Gamma(1.5), 1e-14, "B11: gamma(1.5)");
            Assert.AreEqual(.96176583190738741941, Sf.Gamma(1.9), 1e-14, "B12: gamma(1.9)");
            Assert.AreEqual(1.0, Sf.Gamma(2.0), 1e-15, "B13: gamma(2.0)");
            Assert.AreEqual(362880.0, Sf.Gamma(10.0), 1e-12, "B14: gamma(10.0)");
            Assert.AreEqual(1159686.4489708177739, Sf.Gamma(10.51), 1e-8, "B15: gamma(10.51)");
            Assert.AreEqual(.93326215443944152682e156, Sf.Gamma(100), 1e-13 * 1e+155, "B16: gamma(100)"); // 9.3326215443944E+155
            Assert.AreEqual(-100.58719796441077919, Sf.Gamma(-0.01), 1e-12, "B17: gamma(-0.01)");
            Assert.AreEqual(-10.686287021193193549, Sf.Gamma(-0.1), 1e-14, "B18: gamma(-0.1)");
            Assert.AreEqual(-3.5449077018110320546, Sf.Gamma(-0.5), 1e-14, "B19: gamma(-0.5)");
            Assert.AreEqual(4.8509571405220973902, Sf.Gamma(-1.2), 1e-14, "B20: gamma(-1.2)");
            Assert.AreEqual(-49.547903041431840399, Sf.Gamma(-2.01), 1e-11, "B21: gamma(-2.01)");
            Assert.AreEqual(-.10234011287149294961e-155, Sf.Gamma(-100.01), 1e-9, "B22: gamma(-100.01)");
        }

        /// <summary>
        /// Verification against Matlab implementation.
        /// </summary>
        [TestMethod]
        public void Gamma_Gamma_Matlab_Conformance()
        {
            Doubles.AreEqual(double.PositiveInfinity, Sf.Gamma(1.7976931348623157e+308), "A1: double.MaxValue");
            Doubles.AreEqual(double.NaN, Sf.Gamma(-1.7976931348623157e+308), "A2: -double.MaxValue");
            Doubles.AreEqual(double.PositiveInfinity, Sf.Gamma(4.94065645841247e-324), "A3: double.Epsilon");
            Doubles.AreEqual(double.NegativeInfinity, Sf.Gamma(-4.94065645841247e-324), "A4: -double.Epsilon");
            Doubles.AreEqual(double.NaN, Sf.Gamma(double.PositiveInfinity), "A5: double.PositiveInfinity");
            Doubles.AreEqual(double.NaN, Sf.Gamma(double.NegativeInfinity), "A6: double.NegativeInfinity");
            Doubles.AreEqual(double.NaN, Sf.Gamma(double.NaN), "A7: double.NaN");

            Doubles.AreEqual(double.PositiveInfinity, Sf.Gamma(1.5e+208), "B1: 1.5e+208");
            Doubles.AreEqual(double.PositiveInfinity, Sf.Gamma(1.5e+108), "B2: 1.5e+108");
            Doubles.AreEqual(double.PositiveInfinity, Sf.Gamma(1.5e+58), "B3: 1.5e+58");
            Doubles.AreEqual(double.PositiveInfinity, Sf.Gamma(1.5e+8), "B4: 1.5e+8");
            Doubles.AreEqual(double.PositiveInfinity, Sf.Gamma(1.5e+4), "B5: 1.5e+4");
            Doubles.AreEqual(double.PositiveInfinity, Sf.Gamma(171.63), "B6: 171.63");
            Doubles.AreEqual(1.757682678997857e+308, Sf.Gamma(171.62), 0.000000000001e+308, "B7: 171.62"); // 1.75768267899766E+308
            Doubles.AreEqual(7.64010579165693E+306, Sf.Gamma(171.01), 0.00000000000001e+306, "B8: 171.01"); // 7.64010579165693E+306
            Doubles.AreEqual(7.257042685410633e+306, Sf.Gamma(170.99999), "B9: 170.99999");
            Doubles.AreEqual(1.269814833259059e+262, Sf.Gamma(150.69999), 1e-12 * 1e+262, "B10: 150.69999"); // 1.2698148332592E+262
            Doubles.AreEqual(5.068688281175276e+219, Sf.Gamma(130.94999), 0.000000000001e+219, "B11: 130.94999"); // 5.06868828117614E+219
            Doubles.AreEqual(6.689182383389275e+198, Sf.Gamma(120.99999), 0.000000000001e+198, "B12: 120.99999"); // 6.68918238338966E+198
            Doubles.AreEqual(3.783156001490633e+177, Sf.Gamma(110.69499), 0.000000000001e+177, "B13: 110.69499"); // 3.78315600149042E+177
            Doubles.AreEqual(5.747309063385497e+156, Sf.Gamma(100.39499), "B14: 100.39499");
            Doubles.AreEqual(9.882890003872385e+136, Sf.Gamma(90.39799), 0.00000000001e+136, "B15: 90.39799"); // 9.88289000387126E+136
            Doubles.AreEqual(3.297396918078480e+117, Sf.Gamma(80.29799), 0.000000000001e+117, "B16: 80.29799"); // 3.29739691807829E+117
            Doubles.AreEqual(2.167263851621540e+099, Sf.Gamma(70.59799), 0.000000000001e+99, "B17: 70.59799"); // 2.1672638516216E+99
            Doubles.AreEqual(7.089569018730520e+080, Sf.Gamma(60.39899), 1e-12 * 1e+80, "B18: 60.39899"); // 7.08956901873072E+80
            Doubles.AreEqual(1.814483330011223e+063, Sf.Gamma(50.27989), 0.000000000001e+63, "B19: 50.27989"); // 1.81448333001148E+63
            Doubles.AreEqual(3.794142735345910e+047, Sf.Gamma(40.79299), 0.00000000001e+47, "B20: 40.79299"); // 3.79414273534607E+47
            Doubles.AreEqual(4.763844588498432e+031, Sf.Gamma(30.49639), 0.000000000001e+31, "B21: 30.49639"); // 4.76384458849836E+31
            Doubles.AreEqual(2.917857119617840e+017, Sf.Gamma(20.29379), 0.000000000001e+17, "B22: 20.29379"); // 2.91785711961776E+17
            Doubles.AreEqual(5.671253833190003e+005, Sf.Gamma(10.19739), "B23: 10.19739");
            Doubles.AreEqual(6.034213917396429e+004, Sf.Gamma(9.18739), "B24: 9.18739");
            Doubles.AreEqual(9.175034389769795e+003, Sf.Gamma(8.29439), "B25: 8.29439");
            Doubles.AreEqual(1.530622782581078e+003, Sf.Gamma(7.39639), "B26: 7.39639");
            Doubles.AreEqual(4.852126275616346e+002, Sf.Gamma(6.78739), "B27: 6.78739");
            Doubles.AreEqual(1.657282413786532e+002, Sf.Gamma(6.18739), "B28: 6.18739");
            Doubles.AreEqual(84.823959943549639, Sf.Gamma(5.79439), "B29: 5.79439");
            Doubles.AreEqual(37.981833382931889, Sf.Gamma(5.29839), "B30: 5.29839");
            Doubles.AreEqual(17.436185707923812, Sf.Gamma(4.78439), "B31: 4.78439");
            Doubles.AreEqual(11.382608706110156, Sf.Gamma(4.48439), "B32: 4.48439");
            Doubles.AreEqual(7.599773282010379, Sf.Gamma(4.18439), "B33: 4.18439");
            Doubles.AreEqual(5.198953739373128, Sf.Gamma(3.88439), "B34: 3.88439");
            Doubles.AreEqual(3.651852776664410, Sf.Gamma(3.58439), "B35: 3.58439");
            Doubles.AreEqual(2.640552423879314, Sf.Gamma(3.28439), "B36: 3.28439");
            Doubles.AreEqual(1.786760181605854, Sf.Gamma(2.87439), "B37: 2.87439");
            Doubles.AreEqual(1.305826926411821, Sf.Gamma(2.47439), "B38: 2.47439");
            Doubles.AreEqual(1.086755770920483, Sf.Gamma(2.17439), "B39: 2.17439");
            Doubles.AreEqual(0.953248887161078, Sf.Gamma(1.87439), "B40: 1.87439");
            Doubles.AreEqual(0.924835547745009, Sf.Gamma(1.77439), "B41: 1.77439");
            Doubles.AreEqual(0.904035389586068, Sf.Gamma(1.67439), "B42: 1.67439");
            Doubles.AreEqual(0.890888196401863, Sf.Gamma(1.57439), "B43: 1.57439");
            Doubles.AreEqual(0.885672668976201, Sf.Gamma(1.47439), "B44: 1.47439");
            Doubles.AreEqual(0.888961099096745, Sf.Gamma(1.37439), "B45: 1.37439");
            Doubles.AreEqual(0.901706256897661, Sf.Gamma(1.27439), "B46: 1.27439");
            Doubles.AreEqual(0.925378937934147, Sf.Gamma(1.17439), "B47: 1.17439");
            Doubles.AreEqual(0.962188642647735, Sf.Gamma(1.07439), "B48: 1.07439");
            Doubles.AreEqual(1, Sf.Gamma(1.0), "B49: 1.0");
            Doubles.AreEqual(1.015446863646070, Sf.Gamma(0.97439), "B50: 0.97439");
            Doubles.AreEqual(1.090187315912897, Sf.Gamma(0.87439), "B51: 0.87439");
            Doubles.AreEqual(1.194276201584485, Sf.Gamma(0.77439), "B52: 0.77439");
            Doubles.AreEqual(1.340523123987704, Sf.Gamma(0.67439), "B53: 0.67439");
            Doubles.AreEqual(1.551016202235176, Sf.Gamma(0.57439), "B54: 0.57439");
            Doubles.AreEqual(1.866971624562492, Sf.Gamma(0.47439), "B55: 0.47439");
            Doubles.AreEqual(2.374425329460576, Sf.Gamma(0.37439), "B56: 0.37439");
            Doubles.AreEqual(3.286221279557057, Sf.Gamma(0.27439), "B57: 0.27439");
            Doubles.AreEqual(5.306376156512114, Sf.Gamma(0.17439), "B58: 0.17439");
            Doubles.AreEqual(12.934381538482791, Sf.Gamma(0.07439), "B59: 0.07439");
            Doubles.AreEqual(2.272175416864083e+002, Sf.Gamma(0.00439), "B60: 0.00439");
            Doubles.AreEqual(2.563525734031532e+003, Sf.Gamma(0.00039), "B61: 0.00039");
            Doubles.AreEqual(1.111053398445390e+004, Sf.Gamma(0.00009), "B62: 0.00009");
            Doubles.AreEqual(9.999999942278434e+007, Sf.Gamma(0.00000001), "B63: 0.00000001");
            Doubles.AreEqual(1.000000000000000e+101, Sf.Gamma(0.1e-100), "B64: 0.1e-100");
            Doubles.AreEqual(1.000000000000000e+201, Sf.Gamma(0.1e-200), "B65: 0.1e-200");
            Doubles.AreEqual(9.999999999999999e+300, Sf.Gamma(0.1e-300), "B66: 0.1e-300");
            Doubles.AreEqual(1.000000000000000e+307, Sf.Gamma(0.1e-306), "B67: 0.1e-306");
            Doubles.AreEqual(double.PositiveInfinity, Sf.Gamma(0.1e-307), "B68: 0.1e-307");
            Doubles.AreEqual(double.NegativeInfinity, Sf.Gamma(-0.1e-308), "B69: -0.1e-308");
            Doubles.AreEqual(double.NegativeInfinity, Sf.Gamma(-0.1e-307), "B70: -0.1e-307"); // Matlab: -1.000000000000000e+308
            Doubles.AreEqual(-1.000000000000000e+307, Sf.Gamma(-0.1e-306), "B71: -0.1e-306");
            Doubles.AreEqual(-9.999999999999999e+300, Sf.Gamma(-0.1e-300), "B72: -0.1e-300");
            Doubles.AreEqual(-1.000000000000000e+201, Sf.Gamma(-0.1e-200), "B73: -0.1e-200");
            Doubles.AreEqual(-1.000000000000000e+101, Sf.Gamma(-0.1e-100), "B74: -0.1e-100");
            Doubles.AreEqual(-9.081827263645446e+050, Sf.Gamma(-0.11011e-50), "B75: -0.11011e-50");
            Doubles.AreEqual(-9.081827263645445e+025, Sf.Gamma(-0.11011e-25), "B76: -0.11011e-25");
            Doubles.AreEqual(-9.065361254646576e+012, Sf.Gamma(-0.11031e-12), "B77: -0.11031e-12");
            Doubles.AreEqual(-9.065361831861770e+006, Sf.Gamma(-0.11031e-6), "B78: -0.11031e-6"); // -9065361,83186177
            Doubles.AreEqual(-1.111168841579840e+004, Sf.Gamma(-0.00009), "B79: -0.00009");
            Doubles.AreEqual(-2.564680165637390e+003, Sf.Gamma(-0.00039), "B80: -0.00039");
            Doubles.AreEqual(-2.283720079949958e+002, Sf.Gamma(-0.00439), "B81: -0.00439");
            Doubles.AreEqual(-14.098917098755750, Sf.Gamma(-0.07439), "B82: -0.07439");
            Doubles.AreEqual(-6.517878013220583, Sf.Gamma(-0.17439), "B83: -0.17439");
            Doubles.AreEqual(-4.589353174096498, Sf.Gamma(-0.27439), "B84: -0.27439");
            Doubles.AreEqual(-3.828224886844075, Sf.Gamma(-0.37439), "B85: -0.37439");
            Doubles.AreEqual(-3.558637787146482, Sf.Gamma(-0.47439), "B86: -0.47439");
            Doubles.AreEqual(-3.624901291600805, Sf.Gamma(-0.57439), "B87: -0.57439");
            Doubles.AreEqual(-4.070893444718144, Sf.Gamma(-0.67439), "B88: -0.67439");
            Doubles.AreEqual(-5.218772530620159, Sf.Gamma(-0.77439), "B89: -0.77439");
            Doubles.AreEqual(-8.572353214956886, Sf.Gamma(-0.87439), "B90: -0.87439");
            Doubles.AreEqual(-39.506544726673326, Sf.Gamma(-0.97439), 1e-12, "B91: -0.97439"); // -39.5065447266735
            Doubles.AreEqual(-1.786838403251360e+002, Sf.Gamma(-0.99439), 1e-11, "B92: -0.99439"); // -178.683840325139
            Doubles.AreEqual(-1.639767908040820e+003, Sf.Gamma(-0.99939), 1e-10, "B93: -0.99939"); // -1639.76790804079
            Doubles.AreEqual(-1.000004227984658e+005, Sf.Gamma(-0.99999), 1e-6, "B94: -0.99999"); // -100000.422798909
            Doubles.AreEqual(-9.925165529114934e+013, Sf.Gamma(-0.99999999999999), 1e+13, "B95: -0.99999999999999"); // -100079991719345
            Doubles.AreEqual(double.NaN, Sf.Gamma(-1.0 + 0.1e-100), "B95: -1.0 + 0.1e-100");
            Doubles.AreEqual(double.NaN, Sf.Gamma(-1.0 - 0.1e-100), "B96: -1.0 - 0.1e-100");
            Doubles.AreEqual(1.000799917193440e+014, Sf.Gamma(-1.00000000000001), "B97: -1.00000000000001");
            Doubles.AreEqual(2.363271801207338, Sf.Gamma(-1.50000000000001), "B98: -1.50000000000001");
            Doubles.AreEqual(5.000046140083309e+004, Sf.Gamma(-1.99999), 1e-5, "B99: -1.99999"); // 50000.4614012062
            Doubles.AreEqual(4.962582764557491e+013, Sf.Gamma(-1.99999999999999), 1e+13, "B100: -1.99999999999999"); // 50039995859672.6
            Doubles.AreEqual(double.NaN, Sf.Gamma(-2.0 + 0.1e-100), "B101: -2.0 + 0.1e-100");
            Doubles.AreEqual(double.NaN, Sf.Gamma(-2.0 - 0.1e-100), "B102: -2.0 - 0.1e-100");
            Doubles.AreEqual(-4.895216986272233e+013, Sf.Gamma(-2.00000000000001), "B103: -2.00000000000001");
            Doubles.AreEqual(-0.945308720482931, Sf.Gamma(-2.50000000000001), "B104: -2.50000000000001");
            Doubles.AreEqual(-1.666687602319778e+004, Sf.Gamma(-2.99999), 1e-6, "B105: -2.99999"); // -16666.8760233222
            Doubles.AreEqual(-1.631306994741727e+013, Sf.Gamma(-2.99999999999999), 1e+10, "B106: -2.99999999999999"); // -16317389954241.1
            Doubles.AreEqual(double.NaN, Sf.Gamma(-3.0 + 0.1e-100), "B107: -3.0 + 0.1e-100");
            Doubles.AreEqual(double.NaN, Sf.Gamma(-3.0 - 0.1e-100), "B108: -3.0 - 0.1e-100");
            Doubles.AreEqual(1.631738995424071e+013, Sf.Gamma(-3.00000000000001), "B109: -3.00000000000001");
            Doubles.AreEqual(0.270088205852265, Sf.Gamma(-3.50000000000001), "B110: -3.50000000000001");
            Doubles.AreEqual(4.166729422622997e+003, Sf.Gamma(-3.99999), 1e-7, "B111: -3.99999"); // 4166.72942265409
            Doubles.AreEqual(4.078267486854332e+012, Sf.Gamma(-3.99999999999999), 1e+10, "B112: -3.99999999999999"); // 4079347488560.29
            Doubles.AreEqual(double.NaN, Sf.Gamma(-4.0 + 0.1e-100), "B113: -4.0 + 0.1e-100");
            Doubles.AreEqual(double.NaN, Sf.Gamma(-4.0 - 0.1e-100), "B114: -4.0 - 0.1e-100");
            Doubles.AreEqual(-4.264772374403816e+012, Sf.Gamma(-4.00000000000001), "B115: -4.00000000000001");
            Doubles.AreEqual(-0.060019601300503, Sf.Gamma(-4.50000000000001), 1e-15, "B116: -4.50000000000001"); // -0.0600196013005033
            Doubles.AreEqual(-8.333475512550419e+002, Sf.Gamma(-4.99999), 1e-7, "B117: -4.99999"); // -833.347551262929
            Doubles.AreEqual(-8.509755246164594e+011, Sf.Gamma(-4.99999999999999), 1e+11, "B118: -4.99999999999999"); // -852954474880.79
            Doubles.AreEqual(double.NaN, Sf.Gamma(-5.0 + 0.1e-100), "B119: -5.0 + 0.1e-100");
            Doubles.AreEqual(double.NaN, Sf.Gamma(-5.0 - 0.1e-100), "B120: -5.0 - 0.1e-100");
            Doubles.AreEqual(8.529544748807616e+011, Sf.Gamma(-5.00000000000001), "B121: -5.00000000000001");
            Doubles.AreEqual(0.010912654781910, Sf.Gamma(-5.50000000000001), 1e-13, "B122: -5.50000000000001"); // 0.0109126547819097
            Doubles.AreEqual(1.388914900283237e+002, Sf.Gamma(-5.99999), 1e-8, "B123: -5.99999"); // 138.891490029638
            Doubles.AreEqual(1.418292541027434e+011, Sf.Gamma(-5.99999999999999), 1e+10, "B124: -5.99999999999999"); // 142159079146.799
            Doubles.AreEqual(double.NaN, Sf.Gamma(-6.0 + 0.1e-100), "B125: -6.0 + 0.1e-100");
            Doubles.AreEqual(double.NaN, Sf.Gamma(-6.0 - 0.1e-100), "B126: -6.0 - 0.1e-100");
            Doubles.AreEqual(-1.421590791467934e+011, Sf.Gamma(-6.00000000000001), "B127: -6.00000000000001");
            Doubles.AreEqual(-0.001678869966448, Sf.Gamma(-6.50000000000001), 1e-14, "B128: -6.50000000000001"); // -0.00167886996644764
            Doubles.AreEqual(-19.841669777860208, Sf.Gamma(-6.99999), 1e-9, "B129: -6.99999"); // -19.841669778048
            Doubles.AreEqual(-2.026132201467766e+010, Sf.Gamma(-6.99999999999999), 1e+9, "B130: -6.99999999999999"); // -20308439878.1141
            Doubles.AreEqual(double.NaN, Sf.Gamma(-7.0 + 0.1e-100), "B131: -7.0 + 0.1e-100");
            Doubles.AreEqual(double.NaN, Sf.Gamma(-7.0 - 0.1e-100), "B132: -7.0 - 0.1e-100");
            Doubles.AreEqual(2.585565262259785e+008, Sf.Gamma(-9.00000000000001), "B133: -9.00000000000001");
            Doubles.AreEqual(2.772127911575034e-006, Sf.Gamma(-9.50000000000001), "B134: -9.50000000000001");
            Doubles.AreEqual(0.027557967316759, Sf.Gamma(-9.99999), 1e-12, "B135: -9.99999"); // 0.0275579673170201
            Doubles.AreEqual(2.589771429832638e+007, Sf.Gamma(-9.99999999999999), 1e+6, "B136: -9.99999999999999"); // 2585565.6225991
            Doubles.AreEqual(double.NaN, Sf.Gamma(-10.0 + 0.1e-100), "B137: -10.0 + 0.1e-100");
            Doubles.AreEqual(double.NaN, Sf.Gamma(-10.0 - 0.1e-100), "B138: -10.0 - 0.1e-100");
            Doubles.AreEqual(-2.585565262259783e+007, Sf.Gamma(-10.00000000000001), "B139: -10.00000000000001");
            Doubles.AreEqual(-2.640121820547649e-007, Sf.Gamma(-10.50000000000001), "B140: -10.50000000000001");
            Doubles.AreEqual(-0.002505272033589, Sf.Gamma(-10.99999), 1e-12, "B141: -10.99999"); // 0.00250527203361277
            Doubles.AreEqual(-2.354337663484219e+006, Sf.Gamma(-10.99999999999999), 1e+4, "B142: -10.99999999999999"); // -2350513.87478174
            Doubles.AreEqual(double.NaN, Sf.Gamma(-11.0 + 0.1e-100), "B143: -11.0 + 0.1e-100");
            Doubles.AreEqual(double.NaN, Sf.Gamma(-11.0 - 0.1e-100), "B144: -11.0 - 0.1e-100");
            Doubles.AreEqual(2.350513874781619e+006, Sf.Gamma(-11.00000000000001), "B145: -11.00000000000001");
            Doubles.AreEqual(double.NaN, Sf.Gamma(-50.0 + 0.1e-100), "B146: -11.0 + 0.1e-100");
            Doubles.AreEqual(double.NaN, Sf.Gamma(-50.0 - 0.1e-100), "B147: -11.0 - 0.1e-100");
            Doubles.AreEqual(-4.627377427363089e-051, Sf.Gamma(-50.00000000000001), 1e+11 * 1e-51, "B148: -50.00000000000001"); // -4.62737742736297E-51
            Doubles.AreEqual(-1.449954393907714e-065, Sf.Gamma(-50.50000000000001), 1e+12 * 1e-65, "B149: -50.50000000000001"); // -1.44995439390769E-65
            Doubles.AreEqual(-6.447213757625308e-062, Sf.Gamma(-50.99999), 1e+10 * 1e-62, "B150: -50.99999"); // -6.44721375767418E-62
            Doubles.AreEqual(-9.071433208130201e-053, Sf.Gamma(-50.99999999999999), 1e+2 * 1e-53, "B151: -50.99999999999999"); // -9.07328907326149E-53
        }

        [TestMethod]
        public void Gamma_GammaSign_SanityCheck()
        {
            Assert.AreEqual(1, Sf.GammaSign(1.7976931348623157e+308), "A1: double.MaxValue");
            Assert.AreEqual(1, Sf.GammaSign(-1.7976931348623157e+308), "A2: -double.MaxValue");
            Assert.AreEqual(1, Sf.GammaSign(4.94065645841247e-324), "A3: double.Epsilon");
            Assert.AreEqual(-1, Sf.GammaSign(-4.94065645841247e-324), "A4: -double.Epsilon");
            Assert.AreEqual(1, Sf.GammaSign(double.PositiveInfinity), "A5: double.PositiveInfinity");
            Assert.AreEqual(1, Sf.GammaSign(double.NegativeInfinity), "A6: double.NegativeInfinity");

            Assert.AreEqual(1, Sf.GammaSign(1.5e+208), "B1: : 1.5e+208");
            Assert.AreEqual(1, Sf.GammaSign(1.5e+108), "B2: : 1.5e+108");
            Assert.AreEqual(1, Sf.GammaSign(1.5e+58),  "B3: 1.5e+58");
            Assert.AreEqual(1, Sf.GammaSign(1.5e+8),  "B4: 1.5e+8");
            Assert.AreEqual(1, Sf.GammaSign(1.5e+4),  "B5: 1.5e+4");
            Assert.AreEqual(1, Sf.GammaSign(171.63),  "B6: 171.63");
            Assert.AreEqual(1, Sf.GammaSign(171.62),  "B7: 171.62");
            Assert.AreEqual(1, Sf.GammaSign(171.01),  "B8: 171.01");
            Assert.AreEqual(1, Sf.GammaSign(170.99999),  "B9: 170.99999");
            Assert.AreEqual(1, Sf.GammaSign(150.69999),  "B10: 150.69999");
            Assert.AreEqual(1, Sf.GammaSign(130.94999),  "B11: 130.94999");
            Assert.AreEqual(1, Sf.GammaSign(120.99999),  "B12: 120.99999");
            Assert.AreEqual(1, Sf.GammaSign(110.69499),  "B13: 110.69499");
            Assert.AreEqual(1, Sf.GammaSign(100.39499),  "B14: 100.39499");
            Assert.AreEqual(1, Sf.GammaSign(90.39799),  "B15: 90.39799");
            Assert.AreEqual(1, Sf.GammaSign(80.29799),  "B16: 80.29799");
            Assert.AreEqual(1, Sf.GammaSign(70.59799),  "B17: 70.59799");
            Assert.AreEqual(1, Sf.GammaSign(60.39899),  "B18: 60.39899");
            Assert.AreEqual(1, Sf.GammaSign(50.27989),  "B19: 50.27989");
            Assert.AreEqual(1, Sf.GammaSign(40.79299),  "B20: 40.79299");
            Assert.AreEqual(1, Sf.GammaSign(30.49639),  "B21: 30.49639");
            Assert.AreEqual(1, Sf.GammaSign(20.29379),  "B22: 20.29379");
            Assert.AreEqual(1, Sf.GammaSign(10.19739),  "B23: 10.19739");
            Assert.AreEqual(1, Sf.GammaSign(9.18739),  "B24: 9.18739");
            Assert.AreEqual(1, Sf.GammaSign(8.29439),  "B25: 8.29439");
            Assert.AreEqual(1, Sf.GammaSign(7.39639),  "B26: 7.39639");
            Assert.AreEqual(1, Sf.GammaSign(6.78739),  "B27: 6.78739");
            Assert.AreEqual(1, Sf.GammaSign(6.18739),  "B28: 6.18739");
            Assert.AreEqual(1, Sf.GammaSign(5.79439),  "B29: 5.79439");
            Assert.AreEqual(1, Sf.GammaSign(5.29839),  "B30: 5.29839");
            Assert.AreEqual(1, Sf.GammaSign(4.78439),  "B31: 4.78439");
            Assert.AreEqual(1, Sf.GammaSign(4.48439),  "B32: 4.48439");
            Assert.AreEqual(1, Sf.GammaSign(4.18439),  "B33: 4.18439");
            Assert.AreEqual(1, Sf.GammaSign(3.88439),  "B34: 3.88439");
            Assert.AreEqual(1, Sf.GammaSign(3.58439),  "B35: 3.58439");
            Assert.AreEqual(1, Sf.GammaSign(3.28439),  "B36: 3.28439");
            Assert.AreEqual(1, Sf.GammaSign(2.87439),  "B37: 2.87439");
            Assert.AreEqual(1, Sf.GammaSign(2.47439),  "B38: 2.47439");
            Assert.AreEqual(1, Sf.GammaSign(2.17439),  "B39: 2.17439");
            Assert.AreEqual(1, Sf.GammaSign(1.87439),  "B40: 1.87439");
            Assert.AreEqual(1, Sf.GammaSign(1.77439),  "B41: 1.77439");
            Assert.AreEqual(1, Sf.GammaSign(1.67439),  "B42: 1.67439");
            Assert.AreEqual(1, Sf.GammaSign(1.57439),  "B43: 1.57439");
            Assert.AreEqual(1, Sf.GammaSign(1.47439),  "B44: 1.47439");
            Assert.AreEqual(1, Sf.GammaSign(1.37439),  "B45: 1.37439");
            Assert.AreEqual(1, Sf.GammaSign(1.27439),  "B46: 1.27439");
            Assert.AreEqual(1, Sf.GammaSign(1.17439),  "B47: 1.17439");
            Assert.AreEqual(1, Sf.GammaSign(1.07439),  "B48: 1.07439");
            Assert.AreEqual(1, Sf.GammaSign(1.0),  "B49: 1.0");
            Assert.AreEqual(1, Sf.GammaSign(0.97439),  "B50: 0.97439");
            Assert.AreEqual(1, Sf.GammaSign(0.87439),  "B51: 0.87439");
            Assert.AreEqual(1, Sf.GammaSign(0.77439),  "B52: 0.77439");
            Assert.AreEqual(1, Sf.GammaSign(0.67439),  "B53: 0.67439");
            Assert.AreEqual(1, Sf.GammaSign(0.57439),  "B54: 0.57439");
            Assert.AreEqual(1, Sf.GammaSign(0.47439),  "B55: 0.47439");
            Assert.AreEqual(1, Sf.GammaSign(0.37439),  "B56: 0.37439");
            Assert.AreEqual(1, Sf.GammaSign(0.27439),  "B57: 0.27439");
            Assert.AreEqual(1, Sf.GammaSign(0.17439),  "B58: 0.17439");
            Assert.AreEqual(1, Sf.GammaSign(0.07439),  "B59: 0.07439");
            Assert.AreEqual(1, Sf.GammaSign(0.00439),  "B60: 0.00439");
            Assert.AreEqual(1, Sf.GammaSign(0.00039),  "B61: 0.00039");
            Assert.AreEqual(1, Sf.GammaSign(0.00009),  "B62: 0.00009");
            Assert.AreEqual(1, Sf.GammaSign(0.00000001),  "B63: 0.00000001");
            Assert.AreEqual(1, Sf.GammaSign(0.1e-100),  "B64: 0.1e-100");
            Assert.AreEqual(1, Sf.GammaSign(0.1e-200),  "B65: 0.1e-200");
            Assert.AreEqual(1, Sf.GammaSign(0.1e-300),  "B66: 0.1e-300");
            Assert.AreEqual(1, Sf.GammaSign(0.1e-306),  "B67: 0.1e-306");
            Assert.AreEqual(1, Sf.GammaSign(0.1e-307),  "B68: 0.1e-307");
            Assert.AreEqual(-1, Sf.GammaSign(-0.1e-308),  "B69: -0.1e-308");
            Assert.AreEqual(-1, Sf.GammaSign(-0.1e-307),  "B70: -0.1e-307");
            Assert.AreEqual(-1, Sf.GammaSign(-0.1e-306),  "B71: -0.1e-306");
            Assert.AreEqual(-1, Sf.GammaSign(-0.1e-300),  "B72: -0.1e-300");
            Assert.AreEqual(-1, Sf.GammaSign(-0.1e-200),  "B73: -0.1e-200");
            Assert.AreEqual(-1, Sf.GammaSign(-0.1e-100),  "B74: -0.1e-100");
            Assert.AreEqual(-1, Sf.GammaSign(-0.11011e-50),  "B75: -0.11011e-50");
            Assert.AreEqual(-1, Sf.GammaSign(-0.11011e-25),  "B76: -0.11011e-25");
            Assert.AreEqual(-1, Sf.GammaSign(-0.11031e-12),  "B77: -0.11031e-12");
            Assert.AreEqual(-1, Sf.GammaSign(-0.11031e-6),  "B78: -0.11031e-6");
            Assert.AreEqual(-1, Sf.GammaSign(-0.00009),  "B79: -0.00009");
            Assert.AreEqual(-1, Sf.GammaSign(-0.00039),  "B80: -0.00039");
            Assert.AreEqual(-1, Sf.GammaSign(-0.00439),  "B81: -0.00439");
            Assert.AreEqual(-1, Sf.GammaSign(-0.07439),  "B82: -0.07439");
            Assert.AreEqual(-1, Sf.GammaSign(-0.17439),  "B83: -0.17439");
            Assert.AreEqual(-1, Sf.GammaSign(-0.27439),  "B84: -0.27439");
            Assert.AreEqual(-1, Sf.GammaSign(-0.37439),  "B85: -0.37439");
            Assert.AreEqual(-1, Sf.GammaSign(-0.47439),  "B86: -0.47439");
            Assert.AreEqual(-1, Sf.GammaSign(-0.57439),  "B87: -0.57439");
            Assert.AreEqual(-1, Sf.GammaSign(-0.67439),  "B88: -0.67439");
            Assert.AreEqual(-1, Sf.GammaSign(-0.77439),  "B89: -0.77439");
            Assert.AreEqual(-1, Sf.GammaSign(-0.87439),  "B90: -0.87439");
            Assert.AreEqual(-1, Sf.GammaSign(-0.97439),  "B91: -0.97439");
            Assert.AreEqual(-1, Sf.GammaSign(-0.99439),  "B92: -0.99439");
            Assert.AreEqual(-1, Sf.GammaSign(-0.99939),  "B93: -0.99939");
            Assert.AreEqual(-1, Sf.GammaSign(-0.99999),  "B94: -0.99999");
            Assert.AreEqual(-1, Sf.GammaSign(-0.99999999999999),  "B95: -0.99999999999999");
            Assert.AreEqual(1, Sf.GammaSign(-1.00000000000001),  "B96: -1.00000000000001");
            Assert.AreEqual(1, Sf.GammaSign(-1.50000000000001),  "B97: -1.50000000000001");
            Assert.AreEqual(1, Sf.GammaSign(-1.99999),  "B98: -1.99999");
            Assert.AreEqual(1, Sf.GammaSign(-1.99999999999999),  "B99: -1.99999999999999");
            Assert.AreEqual(-1, Sf.GammaSign(-2.00000000000001),  "B100: -2.00000000000001");
            Assert.AreEqual(-1, Sf.GammaSign(-2.50000000000001),  "B101: -2.50000000000001");
            Assert.AreEqual(-1, Sf.GammaSign(-2.99999),  "B102: -2.99999");
            Assert.AreEqual(-1, Sf.GammaSign(-2.99999999999999),  "B103: -2.99999999999999");
            Assert.AreEqual(1, Sf.GammaSign(-3.00000000000001),  "B104: -3.00000000000001");
            Assert.AreEqual(1, Sf.GammaSign(-3.50000000000001),  "B105: -3.50000000000001");
            Assert.AreEqual(1, Sf.GammaSign(-3.99999),  "B106: -3.99999");
            Assert.AreEqual(1, Sf.GammaSign(-3.99999999999999),  "B107: -3.99999999999999");
            Assert.AreEqual(-1, Sf.GammaSign(-4.00000000000001),  "B108: -4.00000000000001");
            Assert.AreEqual(-1, Sf.GammaSign(-4.50000000000001),  "B109: -4.50000000000001");
            Assert.AreEqual(-1, Sf.GammaSign(-4.99999),  "B110: -4.99999");
            Assert.AreEqual(-1, Sf.GammaSign(-4.99999999999999),  "B111: -4.99999999999999");
            Assert.AreEqual(1, Sf.GammaSign(-5.00000000000001),  "B112: -5.00000000000001");
            Assert.AreEqual(1, Sf.GammaSign(-5.50000000000001),  "B113: -5.50000000000001");
            Assert.AreEqual(1, Sf.GammaSign(-5.99999),  "B114: -5.99999");
            Assert.AreEqual(1, Sf.GammaSign(-5.99999999999999),  "B115: -5.99999999999999");
            Assert.AreEqual(-1, Sf.GammaSign(-6.00000000000001),  "B116: -6.00000000000001");
            Assert.AreEqual(-1, Sf.GammaSign(-6.50000000000001),  "B117: -6.50000000000001");
            Assert.AreEqual(-1, Sf.GammaSign(-6.99999),  "B118: -6.99999");
            Assert.AreEqual(-1, Sf.GammaSign(-6.99999999999999),  "B119: -6.99999999999999");
            Assert.AreEqual(1, Sf.GammaSign(-9.00000000000001),  "B120: -9.00000000000001");
            Assert.AreEqual(1, Sf.GammaSign(-9.50000000000001),  "B121: -9.50000000000001");
            Assert.AreEqual(1, Sf.GammaSign(-9.99999),  "B122: -9.99999");
            Assert.AreEqual(1, Sf.GammaSign(-9.99999999999999),  "B123: -9.99999999999999");
            Assert.AreEqual(-1, Sf.GammaSign(-10.00000000000001),  "B124: -10.00000000000001");
            Assert.AreEqual(-1, Sf.GammaSign(-10.50000000000001),  "B125: -10.50000000000001");
            Assert.AreEqual(-1, Sf.GammaSign(-10.99999),  "B126: -10.99999");
            Assert.AreEqual(-1, Sf.GammaSign(-10.99999999999999),  "B127: -10.99999999999999");
            Assert.AreEqual(1, Sf.GammaSign(-11.00000000000001),  "B128: -11.00000000000001");
            Assert.AreEqual(-1, Sf.GammaSign(-50.00000000000001),  "B129: -50.00000000000001");
            Assert.AreEqual(-1, Sf.GammaSign(-50.50000000000001),  "B130: -50.50000000000001");
            Assert.AreEqual(-1, Sf.GammaSign(-50.99999),  "B131: -50.99999");
            Assert.AreEqual(-1, Sf.GammaSign(-50.99999999999999),  "B132: -50.99999999999999");
        }

        [TestMethod]
        public void Gamma_Gamma_Properties()
        {
            // Recursion.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-2, 1.0e1, 20))
            {
                Doubles.AreEqual(
                    Sf.Gamma(x + 1),
                    x * Sf.Gamma(x),
                    $"Recursion: {x}");
            }

            // Special cases.
            Doubles.AreEqual(Constants.SqrtPi, Sf.Gamma(0.5), "Special cases: 0.5");
            Doubles.AreEqual(1d, Sf.Gamma(1.0), "Special cases: 1.0");
            Doubles.AreEqual(Constants.SqrtPi / 2d, Sf.Gamma(1.5), "Special cases: 1.5");
            Doubles.AreEqual(1d, Sf.Gamma(2.0), "Special cases: 2.0");
            Doubles.AreEqual(2d, Sf.Gamma(3.0), "Special cases: 3.0");
            Doubles.AreEqual(6d, Sf.Gamma(4.0), "Special cases: 4.0");

            // Reflection.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-2, 1.0e1, 20))
            {
                // Don't try a large near-integer, because trig functions of large numbers aren't accurate enough.
                if (x > 20 && Math.Abs(x - Math.Round(x)) < 0.05)
                {
                    continue;
                }

                double positive = Sf.Gamma(x);
                double negative = Sf.Gamma(-x);

                double minx = -x;
                Doubles.AreEqual(
                    Constants.Pi / Math.Sin(Constants.Pi * x),
                    minx * negative * positive,
                    1e-13,
                    $"Reflection: x={x}, G(x)={positive}, G(-x)={negative}, {Constants.Pi / Math.Sin(Constants.Pi * x)} ?= {minx * negative * positive}");
            }

            // Inequality.
            foreach (double x in GenerateLogUniformRealPositiveSamples(2.0, 1.0e2, 10))
            {
                // For x >= 2.
                double lower = Math.Pow(x / Constants.E, x - 1);
                double upper = Math.Pow(x / 2, x - 1);
                double value = Sf.Gamma(x);

                Assert.IsTrue(lower <= value && value <= upper, $"Inequality: G({x})={value}, lower={lower}, upper={upper}");
            }

            // Trott identity.
            double g1 = Sf.Gamma(1 / 24d);
            double g5 = Sf.Gamma(5 / 24d);
            double g7 = Sf.Gamma(7 / 24d);
            double g11 = Sf.Gamma(11 / 24d);

            Doubles.AreEqual(Math.Sqrt(3d) * Math.Sqrt(2d + Math.Sqrt(3d)), (g1 * g11) / (g5 * g7), "Trott identity");
        }

        [TestMethod]
        public void Gamma_Gamma_ComplexArgument_Properties()
        {
            // Conjugation.
            foreach (Complex z in GenerateComplexLogarithmicSamples(1e-4, 1e+4, 1000))
            {
                Complex actual = Sf.Gamma(z.Conjugate);
                Complex expected = Sf.Gamma(z).Conjugate;

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected.Real, actual.Real),
                    1e-5,
                    $"Conjugation Re: z={z}, actual={actual}, expected={expected}");
                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected.Imag, actual.Imag),
                    1e-5,
                    $"Conjugation Im: z={z}, actual={actual}, expected={expected}");
            }

            // Recurrence.
            foreach (Complex z in GenerateComplexLogarithmicSamples(1e-4, 1e+2, 1000))
            {
                Complex actual = Sf.Gamma(z) * z;
                Complex expected = Sf.Gamma(z + 1d);

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected.Real, actual.Real),
                    1e-5,
                    $"Recurrence Re: z={z}, actual={actual}, expected={expected}");
                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected.Imag, actual.Imag),
                    1e-5,
                    $"Recurrence Im: z={z}, actual={actual}, expected={expected}");
            }

            // Duplication.
            foreach (Complex z in GenerateComplexLogarithmicSamples(-1e+4, 1e+4, 1000))
            {
                Complex actual = Sf.Gamma(2 * z);
                Complex expected = Complex.Pow(2d, 2 * z - 0.5) * Sf.Gamma(z) * Sf.Gamma(z + 0.5) / Math.Sqrt(Constants.TwoPi);

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected.Real, actual.Real),
                    1e-5,
                    $"Duplication Re: z={z}, actual={actual}, expected={expected}");
                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected.Imag, actual.Imag),
                    1e-5,
                    $"Duplication Im: z={z}, actual={actual}, expected={expected}");
            }

            // Inequality.
            foreach (Complex z in GenerateComplexLogarithmicSamples(1e-4, 1e+2, 1000))
            {
                double actual = Complex.Abs(Sf.Gamma(z));
                double expected = Math.Abs(Sf.Gamma(z.Real));

                Assert.IsTrue(actual <= expected, $"Inequality Re: z={z}, actual={actual}, expected={expected}");
            }

            // Known lines.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1e-4, 1e+2, 1000))
            {
                Complex actual = Sf.Gamma(new Complex(0d, x)) * Sf.Gamma(new Complex(0d, -x));
                double expected = Constants.Pi / x / Math.Sinh(Constants.Pi * x);

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected, actual.Real),
                    1e-5,
                    $"Known lines Re 1: x={x}, actual={actual}, expected={expected}");

                actual = Sf.Gamma(new Complex(0.5, x)) * Sf.Gamma(new Complex(0.5, -x));
                expected = Constants.Pi / Math.Cosh(Constants.Pi * x);

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected, actual.Real),
                    1e-5,
                    $"Known lines Re 2: x={x}, actual={actual}, expected={expected}");

                actual = Sf.Gamma(new Complex(1d, x)) * Sf.Gamma(new Complex(1d, -x));
                expected = Constants.Pi * x / Math.Sinh(Constants.Pi * x);

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected, actual.Real),
                    1e-5,
                    $"Known lines Re 3: x={x}, actual={actual}, expected={expected}");
            }
        }

        /// <summary>
        /// Verification against Boost implementation.
        /// </summary>
        [TestMethod]
        public void Gamma_Gamma_Boost_Conformance()
        {
            // At integer and half integer values.
            Assert.AreEqual(1, Sf.Gamma(1), 1e-14, "A1: 1");
            Assert.AreEqual(1, Sf.Gamma(2), 1e-14, "A2: 2");
            Assert.AreEqual(2, Sf.Gamma(3), 1e-14, "A3: 3");
            Assert.AreEqual(6, Sf.Gamma(4), 1e-14, "A4: 4");
            Assert.AreEqual(24, Sf.Gamma(5), 1e-14, "A5: 5");
            Assert.AreEqual(120, Sf.Gamma(6), 1e-14, "6: 6");
            Assert.AreEqual(720, Sf.Gamma(7), 1e-14, "A7: 7");
            Assert.AreEqual(5040, Sf.Gamma(8), 1e-14, "A8: 8");
            Assert.AreEqual(40320, Sf.Gamma(9), 1e-14, "A9: 9");
            Assert.AreEqual(362880, Sf.Gamma(10), 1e-14, "A10: 10");
            Assert.AreEqual(3628800, Sf.Gamma(11), 1e-8, "A11: 11");
            Doubles.AreEqual(39916800, Sf.Gamma(12), 1e-7, "A12: 12");
            Doubles.AreEqual(479001600, Sf.Gamma(13), 1e-1, "A13: 13");
            Doubles.AreEqual(4790016006227020800.0, Sf.Gamma(14), 1e-1, "A14: 14");
            Doubles.AreEqual(622702080087178291200.0, Sf.Gamma(15), 1e-14, "A15: 15");
            Doubles.AreEqual(871782912001307674368000.0, Sf.Gamma(16), 1e-14, "A16: 16");
            Doubles.AreEqual(130767436800020922789888000.0, Sf.Gamma(17), 1e-14, "A17: 17");
            Doubles.AreEqual(20922789888000355687428096000.0, Sf.Gamma(18), 1e-14, "A18: 18");
            Doubles.AreEqual(3556874280960006402373705728000.0, Sf.Gamma(19), 1e-14, "A19: 19");
            Doubles.AreEqual(6402373705728000121645100408832000.0, Sf.Gamma(20), 1e-14, "A20: 20");
            Doubles.AreEqual(0.243290200817664e19, Sf.Gamma(21), 1e-14, "A21: 21");
            Doubles.AreEqual(0.5109094217170944e20, Sf.Gamma(22), 1e-14, "A22: 22");
            Doubles.AreEqual(0.112400072777760768e22, Sf.Gamma(23), 1e-14, "A23: 23");
            Doubles.AreEqual(0.2585201673888497664e23, Sf.Gamma(24), 1e-14, "A24: 24");
            Doubles.AreEqual(0.62044840173323943936e24, Sf.Gamma(25), 1e-14, "A25: 25");
            Doubles.AreEqual(0.15511210043330985984e26, Sf.Gamma(26), 1e-14, "A26: 26");
            Doubles.AreEqual(0.403291461126605635584e27, Sf.Gamma(27), 1e-14, "A27: 27");
            Doubles.AreEqual(0.10888869450418352160768e29, Sf.Gamma(28), 1e-14, "A28: 28");
            Doubles.AreEqual(0.304888344611713860501504e30, Sf.Gamma(29), 1e-14, "A29: 29");
            Doubles.AreEqual(0.8841761993739701954543616e31, Sf.Gamma(30), 1e-14, "A30: 30");
            Doubles.AreEqual(0.26525285981219105863630848e33, Sf.Gamma(31), 1e-14, "A31: 31");
            Doubles.AreEqual(0.822283865417792281772556288e34, Sf.Gamma(32), 1e-14, "A32: 32");
            Doubles.AreEqual(0.26313083693369353016721801216e36, Sf.Gamma(33), 1e-14, "A33: 33");
            Doubles.AreEqual(0.868331761881188649551819440128e37, Sf.Gamma(34), 1e-14, "A34: 34");
            Doubles.AreEqual(0.29523279903960414084761860964352e39, Sf.Gamma(35), 1e-14, "A35: 35");
            Doubles.AreEqual(0.103331479663861449296666513375232e41, Sf.Gamma(36), 1e-14, "A36: 36");
            Doubles.AreEqual(0.3719933267899012174679994481508352e42, Sf.Gamma(37), 1e-14, "A37: 37");
            Doubles.AreEqual(0.137637530912263450463159795815809024e44, Sf.Gamma(38), 1e-14, "A38: 38");
            Doubles.AreEqual(0.5230226174666011117600072241000742912e45, Sf.Gamma(39), 1e-14, "A39: 39");
            Doubles.AreEqual(0.203978820811974433586402817399028973568e47, Sf.Gamma(40), 1e-14, "A40: 40");
        }

        /// <summary>
        /// Verification against Maple implementation.
        /// </summary>
        [TestMethod]
        public void Gamma_Digamma_Maple_Conformance()
        {
            // Ensure poles return NaN.
            Doubles.AreEqual(double.NaN, Sf.Digamma(0d), "A1: 0");
            Doubles.AreEqual(double.NaN, Sf.Digamma(-1d), "A2: -1");
            Doubles.AreEqual(double.NaN, Sf.Digamma(-2d), "A3: -2");
            Doubles.AreEqual(double.NaN, Sf.Digamma(-20d), "A4: -20");
            Assert.IsFalse(double.IsNaN(Sf.Digamma(-20.0000000001)), "A5: -20.0000000001");

            // Compare with Maple: "evalf(Psi(x),20);"
            Doubles.AreEqual(-1000.5755719318103005, Sf.Digamma(0.001), 1e-15, "B1: 0.001");
            Doubles.AreEqual(-100.56088545786867450, Sf.Digamma(0.01), 1e-15, "B2: 0.01");
            Doubles.AreEqual(-10.423754940411076795, Sf.Digamma(0.1), 1e-15, "B3: 0.1");
            Doubles.AreEqual(-5.2890398965921882955, Sf.Digamma(0.2), 1e-15, "B4: 0.2");
            Doubles.AreEqual(-2.5613845445851161457, Sf.Digamma(0.4), 1e-15, "B5: 0.4");
            Doubles.AreEqual(-1.5406192138931904148, Sf.Digamma(0.6), 1e-15, "B6: 0.6");
            Doubles.AreEqual(-.75492694994705139189, Sf.Digamma(0.9), 1e-15, "B7: 0.9");
            Doubles.AreEqual(-.57886180210864542646, Sf.Digamma(0.999), 1e-15, "B8: 0.999");
            Doubles.AreEqual(-.57721566490153286061, Sf.Digamma(1.0), 1e-15, "B9: 1.0");
            Doubles.AreEqual(-.57557193181030047147, Sf.Digamma(1.001), 1e-14, "B10: 1.001");
            Doubles.AreEqual(.36489973978576520559e-1, Sf.Digamma(1.5), 1e-14, "B11: 1.5");
            Doubles.AreEqual(.35618416116405971922, Sf.Digamma(1.9), 1e-15, "B12: 1.9");
            Doubles.AreEqual(.42278433509846713939, Sf.Digamma(2.0), 1e-15, "B13: 2.0");
            Doubles.AreEqual(2.2517525890667211076, Sf.Digamma(10.0), 1e-15, "B14: 10.0");
            Doubles.AreEqual(2.3039997054324985520, Sf.Digamma(10.51), 1e-15, "B15: 10.51");
            Doubles.AreEqual(4.6001618527380874002, Sf.Digamma(100), 1e-15, "B16: 100");
            Doubles.AreEqual(99.406213695944404856, Sf.Digamma(-0.01), 1e-15, "B17: -0.01");
            Doubles.AreEqual(9.2450730500529486081, Sf.Digamma(-0.1), 1e-15, "B18: -0.1");
            Doubles.AreEqual(.36489973978576520559e-1, Sf.Digamma(-0.5), 1e-14, "B19: -0.5");
            Doubles.AreEqual(4.8683247666271948739, Sf.Digamma(-1.2), 1e-15, "B20: -1.2");
            Doubles.AreEqual(100.89382514365634023, Sf.Digamma(-2.01), 1e-10, "B21: -2.01");
            Doubles.AreEqual(104.57736050326787844, Sf.Digamma(-100.01), 1e-9, "B22: -100.01");
        }

        /// <summary>
        /// Verification against Matlab implementation, psi(x).
        /// <para />
        /// Matlab defines psi(x) only for positive arguments. All negative arguments do return error.
        /// </summary>
        [TestMethod]
        public void Gamma_Digamma_Matlab_Conformance()
        {
            Doubles.AreEqual(7.097827128933840e+002, Sf.Digamma(1.7976931348623157e+308), "A1: double.MaxValue");
            Doubles.AreEqual(double.NaN, Sf.Digamma(-1.7976931348623157e+308), "A2: -double.MaxValue"); // Matlab: error
            Doubles.AreEqual(double.NegativeInfinity, Sf.Digamma(4.94065645841247e-324), "A3: double.Epsilon");
            Doubles.AreEqual(double.PositiveInfinity, Sf.Digamma(-4.94065645841247e-324), "A4: -double.Epsilon"); // Matlab: error
            Doubles.AreEqual(double.PositiveInfinity, Sf.Digamma(double.PositiveInfinity), "A5: double.PositiveInfinity"); // Matlab: PositiveInfinity
            Doubles.AreEqual(double.NaN, Sf.Digamma(double.NegativeInfinity), "A6: double.NegativeInfinity"); // Matlab: error
            Doubles.AreEqual(double.NaN, Sf.Digamma(double.NaN), "A7: double.NaN");

            Doubles.AreEqual(7.096016737502742e+002, Sf.Digamma(1.5e+308), "B1: 1.5e+308");
            Doubles.AreEqual(4.793431644508697e+002, Sf.Digamma(1.5e+208), "B2: 1.5e+208");
            Doubles.AreEqual(2.490846551514651e+002, Sf.Digamma(1.5e+108), "B3: 1.5e+108");
            Doubles.AreEqual(1.339554005017628e+002, Sf.Digamma(1.5e+58), "B4: 1.5e+58");
            Doubles.AreEqual(18.826145848727197, Sf.Digamma(1.5e+8), "B5: 1.5e+8");
            Doubles.AreEqual(9.615772146380644, Sf.Digamma(1.5e+4), "B6: 1.5e+4");
            Doubles.AreEqual(5.142424924356619, Sf.Digamma(171.63), "B7: 171.63");
            Doubles.AreEqual(5.142366487707702, Sf.Digamma(171.62), "B8: 171.62");
            Doubles.AreEqual(5.138795379162858, Sf.Digamma(171.01), "B9: 171.01");
            Doubles.AreEqual(5.138736671373624, Sf.Digamma(170.99999), "B10: 170.99999");
            Doubles.AreEqual(5.011969519661495, Sf.Digamma(150.69999), "B11: 150.69999");
            Doubles.AreEqual(4.870992383402932, Sf.Digamma(130.94999), "B12: 130.94999");
            Doubles.AreEqual(4.791652539464783, Sf.Digamma(120.99999), "B13: 120.99999");
            Doubles.AreEqual(4.702254864191299, Sf.Digamma(110.69499), "B14: 110.69499");
            Doubles.AreEqual(4.604123709604880, Sf.Digamma(100.39499), "B15: 100.39499");
            Doubles.AreEqual(4.500925297124417, Sf.Digamma(90.6), "B16: 90.6");
            Doubles.AreEqual(4.499922251819592, Sf.Digamma(90.50967), "B17: 90.50967");
            Doubles.AreEqual(4.499922229509996, Sf.Digamma(90.5096679919), "B18: 90.5096679919");
            Doubles.AreEqual(4.499922229509751, Sf.Digamma(90.50966799187808), "B19: 90.50966799187808");
            Doubles.AreEqual(4.499922140721554, Sf.Digamma(90.50966), "B20: 90.50966");
            Doubles.AreEqual(4.499814814252058, Sf.Digamma(90.5), "B21: 90.5");
            Doubles.AreEqual(4.498680738653970, Sf.Digamma(90.39799), "B22: 90.39799");
            Doubles.AreEqual(4.379504859409657, Sf.Digamma(80.29799), "B23: 80.29799");
            Doubles.AreEqual(4.249902599647831, Sf.Digamma(70.59799), "B24: 70.59799");
            Doubles.AreEqual(4.092671256119783, Sf.Digamma(60.39899), "B25: 60.39899");
            Doubles.AreEqual(3.907627900408392, Sf.Digamma(50.27989), "B26: 50.27989");
            Doubles.AreEqual(3.696203169967753, Sf.Digamma(40.79299), "B27: 40.79299");
            Doubles.AreEqual(3.401123339527348, Sf.Digamma(30.49639), "B28: 30.49639");
            Doubles.AreEqual(2.985474553075439, Sf.Digamma(20.29379), "B29: 20.29379");
            Doubles.AreEqual(2.272299034001287, Sf.Digamma(10.19739), "B30: 10.19739");
            Doubles.AreEqual(2.162423367888524, Sf.Digamma(9.18739), "B31: 9.18739");
            Doubles.AreEqual(2.054088128066248, Sf.Digamma(8.29439), "B32: 8.29439");
            Doubles.AreEqual(1.931870980408186, Sf.Digamma(7.39639), "B33: 7.39639");
            Doubles.AreEqual(1.839595452678525, Sf.Digamma(6.78739), "B34: 6.78739");
            Doubles.AreEqual(1.739532723397098, Sf.Digamma(6.18739), "B35: 6.18739");
            Doubles.AreEqual(1.668125129891636, Sf.Digamma(5.79439), "B36: 5.79439");
            Doubles.AreEqual(1.570076654624605, Sf.Digamma(5.29839), "B37: 5.29839");
            Doubles.AreEqual(1.457227053601419, Sf.Digamma(4.78439), "B38: 4.78439");
            Doubles.AreEqual(1.384980808382726, Sf.Digamma(4.48439), "B39: 4.48439");
            Doubles.AreEqual(1.307136263770839, Sf.Digamma(4.18439), "B40: 4.18439");
            Doubles.AreEqual(1.222758163273770, Sf.Digamma(3.88439), "B41: 3.88439");
            Doubles.AreEqual(1.130657136416539, Sf.Digamma(3.58439), "B42: 3.58439");
            Doubles.AreEqual(1.029289197991135, Sf.Digamma(3.28439), "B43: 3.28439");
            Doubles.AreEqual(0.871920116807941, Sf.Digamma(2.87439), "B44: 2.87439");
            Doubles.AreEqual(0.690520486190416, Sf.Digamma(2.47439), "B45: 2.47439");
            Doubles.AreEqual(0.529514488787371, Sf.Digamma(2.17439), "B46: 2.17439");
            Doubles.AreEqual(0.429213552032315, Sf.Digamma(2.01), "B47: 2.01");
            Doubles.AreEqual(0.422784980032332, Sf.Digamma(2.000001), "B48: 2.000001");
            Doubles.AreEqual(0.422784335098474, Sf.Digamma(2.00000000000001), "B49: 2.00000000000001");
            Doubles.AreEqual(0.422784335098467, Sf.Digamma(2.0), "B50: 2.0");
            Doubles.AreEqual(0.422784335098403, Sf.Digamma(1.99999999999990), "B51: 1.99999999999990");
            Doubles.AreEqual(0.422783690164199, Sf.Digamma(1.999999), "B52: 1.999999");
            Doubles.AreEqual(0.356184161164060, Sf.Digamma(1.9), "B53: 1.9");
            Doubles.AreEqual(0.338413215896177, Sf.Digamma(1.87439), "B54: 1.87439");
            Doubles.AreEqual(0.265943682217851, Sf.Digamma(1.77439), "B55: 1.77439");
            Doubles.AreEqual(0.188032554450039, Sf.Digamma(1.67439), "B56: 1.67439");
            Doubles.AreEqual(0.103829129339165, Sf.Digamma(1.57439), "B57: 1.57439");
            Doubles.AreEqual(0.012273889292716, Sf.Digamma(1.47439), "B58: 1.47439");
            Doubles.AreEqual(-0.087973481913981, Sf.Digamma(1.37439), 1e-15, "B59: 1.37439"); // -0.0879734819139815
            Doubles.AreEqual(-0.198638986562736, Sf.Digamma(1.27439), "B60: 1.27439");
            Doubles.AreEqual(-0.321991399375845, Sf.Digamma(1.17439), "B61: 1.17439");
            Doubles.AreEqual(-0.461085090605629, Sf.Digamma(1.07439), "B62: 1.07439");
            Doubles.AreEqual(-0.577215664901532, Sf.Digamma(1.0), "B63: 1.0");
            Doubles.AreEqual(-0.620149459150202, Sf.Digamma(0.97439), "B64: 0.97439");
            Doubles.AreEqual(-0.805241217480233, Sf.Digamma(0.87439), "B65: 0.87439");
            Doubles.AreEqual(-1.025395307180256, Sf.Digamma(0.77439), "B66: 0.77439");
            Doubles.AreEqual(-1.294788958324469, Sf.Digamma(0.67439), "B67: 0.67439");
            Doubles.AreEqual(-1.637148255364608, Sf.Digamma(0.57439), "B68: 0.57439");
            Doubles.AreEqual(-2.095696346167559, Sf.Digamma(0.47439), "B69: 0.47439");
            Doubles.AreEqual(-2.758984993973599, Sf.Digamma(0.37439), "B70: 0.37439");
            Doubles.AreEqual(-3.843086670516232, Sf.Digamma(0.27439), "B71: 0.27439");
            Doubles.AreEqual(-6.056265153604873, Sf.Digamma(0.17439), "B72: 0.17439");
            Doubles.AreEqual(-13.903752115743416, Sf.Digamma(0.07439), "B73: 0.07439");
            Doubles.AreEqual(-2.283604502811453e+002, Sf.Digamma(0.00439), "B74: 0.00439");
            Doubles.AreEqual(-2.564679138425948e+003, Sf.Digamma(0.00039), "B75: 0.00039");
            Doubles.AreEqual(-1.111168817874168e+004, Sf.Digamma(0.00009), "B76: 0.00009");
            Doubles.AreEqual(-1.000000005772157e+008, Sf.Digamma(0.00000001), "B77: 0.00000001");
            Doubles.AreEqual(-1.000000000000000e+101, Sf.Digamma(0.1e-100), "B78: 0.1e-100");
            Doubles.AreEqual(-1.000000000000000e+201, Sf.Digamma(0.1e-200), "B79: 0.1e-200");
            Doubles.AreEqual(-9.999999999999999e+300, Sf.Digamma(0.1e-300), "B80: 0.1e-300");
            Doubles.AreEqual(double.NegativeInfinity, Sf.Digamma(0.1e-322), "B81: 0.1e-322");
            Doubles.AreEqual(double.NaN, Sf.Digamma(0.1e-323), "B82: 0.1e-323"); // Matlab: -Inf
        }

        [TestMethod]
        public void Gamma_Digamma_Properties()
        {
            // Recurrence.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-1, 1.0e2, 10))
            {
                Doubles.AreEqual(
                    Sf.Digamma(x) + 1 / x,
                    Sf.Digamma(x + 1),
                    $"Recurrence: {x}");
            }

            // Special cases.
            Doubles.AreEqual(
                -Constants.EulerGamma,
                Sf.Digamma(1d),
                "Special cases: 1.0");
            Doubles.AreEqual(
                1d - Constants.EulerGamma,
                Sf.Digamma(2d),
                "Special cases: 2.0");
            Doubles.AreEqual(
                -2d * Math.Log(2d) - Constants.EulerGamma,
                Sf.Digamma(1 / 2d),
                "Special cases: 1/2");
            Doubles.AreEqual(
                -3d * Math.Log(3d) / 2d - Constants.Pi / 2d / Math.Sqrt(3d) - Constants.EulerGamma,
                Sf.Digamma(1 / 3d),
                "Special cases: 1/3");
            Doubles.AreEqual(
                -3d * Math.Log(2d) - Constants.Pi / 2d - Constants.EulerGamma,
                Sf.Digamma(1 / 4d),
                "Special cases: 1/4");
            Doubles.AreEqual(
                -3d * Math.Log(3d) / 2d - 2d * Math.Log(2d) - Constants.Pi / 2d * Math.Sqrt(3d) - Constants.EulerGamma,
                Sf.Digamma(1 / 6d),
                "Special cases: 1/6");

            // Reflection.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-0, 1.0e1, 20))
            {
                Doubles.AreEqual(
                    Constants.Pi / Math.Tan(Constants.Pi * x) + Sf.Digamma(x),
                    Sf.Digamma(1 - x),
                    1e-12,
                    $"Reflection: x={x}, Psi(x)={Sf.Digamma(x)}, Psi(1-x)={Sf.Digamma(1 - x)}, {Constants.Pi / Math.Tan(Constants.Pi * x) + Sf.Digamma(x)} ?= {Sf.Digamma(1 - x)}");
            }

            // Duplication.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-4, 1.0e2, 20))
            {
                Doubles.AreEqual(
                    Sf.Digamma(2 * x),
                    Sf.Digamma(x) / 2 + Sf.Digamma(x + 0.5) / 2 + Math.Log(2d),
                    $"Duplication: x={x}, Psi(x)={Sf.Digamma(x)}, Psi(2x)={Sf.Digamma(2 * x)}, {Sf.Digamma(2 * x)} ?= {Sf.Digamma(x) / 2 + Sf.Digamma(x + 0.5) / 2 + Math.Log(2d)}");
            }
        }

        [TestMethod]
        public void Gamma_Digamma_ComplexArgument_Properties()
        {
            // Conjugation.
            foreach (Complex z in GenerateComplexLogarithmicSamples(1e-4, 1e+4, 1000))
            {
                Complex actual = Sf.Digamma(z.Conjugate);
                Complex expected = Sf.Digamma(z).Conjugate;

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected.Real, actual.Real),
                    1e-5,
                    $"Conjugation Re: z={z}, actual={actual}, expected={expected}");
                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected.Imag, actual.Imag),
                    1e-5,
                    $"Conjugation Im: z={z}, actual={actual}, expected={expected}");
            }

            // Recurrence.
            foreach (Complex z in GenerateComplexLogarithmicSamples(1e-4, 1e+2, 1000))
            {
                Complex actual = Sf.Digamma(z) + 1 / z;
                Complex expected = Sf.Digamma(z + 1);

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected.Real, actual.Real),
                    1e-5,
                    $"Recurrence Re: z={z}, actual={actual}, expected={expected}");
                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected.Imag, actual.Imag),
                    1e-5,
                    $"Recurrence Im: z={z}, actual={actual}, expected={expected}");
            }

            // Duplication.
            foreach (Complex z in GenerateComplexLogarithmicSamples(-1e+4, 1e+4, 1000))
            {
                Complex actual = Sf.Digamma(2 * z);
                Complex expected = Sf.Digamma(z) / 2 + Sf.Digamma(z + 0.5) / 2 + Constants.Ln2;

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected.Real, actual.Real),
                    1e-5,
                    $"Duplication Re: z={z}, actual={actual}, expected={expected}");
                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected.Imag, actual.Imag),
                    1e-5,
                    $"Duplication Im: z={z}, actual={actual}, expected={expected}");
            }

            // Imaginary parts.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1e-4, 1e+2, 1000))
            {
                double actual = Sf.Digamma(new Complex(0d, x)).Imag;
                double expected = (Constants.Pi / Math.Tanh(Constants.Pi * x) + 1 / x) / 2;

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected, actual),
                    1e-5,
                    $"Imaginary parts 1: x={x}, actual={actual}, expected={expected}");

                actual = Sf.Digamma(new Complex(0.5, x)).Imag;
                expected = Constants.Pi * Math.Tanh(Constants.Pi * x) / 2;

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected, actual),
                    1e-5,
                    $"Imaginary parts 2: x={x}, actual={actual}, expected={expected}");

                actual = Sf.Digamma(new Complex(1d, x)).Imag;
                expected = (Constants.Pi / Math.Tanh(Constants.Pi * x) - 1 / x) / 2;

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected, actual),
                    1e-5,
                    $"Imaginary parts 3: x={x}, actual={actual}, expected={expected}");
            }
        }

        [TestMethod]
        public void Gamma_Polygamma_Properties()
        {
            // Digamma agreement.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-4, 1.0e2, 100))
            {
                Doubles.AreEqual(
                    Sf.Polygamma(0, x),
                    Sf.Digamma(x),
                    $"Digamma agreement: {x}");
            }

            // Recurrence.
            foreach (int n in GenerateLogUniformIntegerPositiveSamples(1, 100, 7))
            {
                foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-0, 1.0e4, 1000))
                {
                    double expected = Sf.Polygamma(n, x) + Ef.Pow(-1d, n) * Sf.Factorial(n) / Ef.Pow(x, n + 1);
                    double actual = Sf.Polygamma(n, x + 1);

                    Doubles.AreEqual(
                        1,
                        Math.Abs(expected / actual),
                        1e-2,
                        $"Recurrence: {x}");
                }
            }

            // Special cases.
            Doubles.AreEqual(
                Constants.Pi * Constants.Pi + 8 * Constants.Catalan,
                Sf.Polygamma(1, 1 / 4d),
                1.0e-3,
                "Special cases: 1/4");
            Doubles.AreEqual(
                Constants.Pi * Constants.Pi / 2,
                Sf.Polygamma(1, 1 / 2d),
                1.0e-3,
                "Special cases: 1/2");
            Doubles.AreEqual(
                Constants.Pi * Constants.Pi / 6,
                Sf.Polygamma(1, 1d),
                1.0e-3,
                "Special cases: 1");

            // Reflection.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-02, 1.0e4, 1000))
            {
                double expected = Sf.Polygamma(2, 1d - x) - Sf.Polygamma(2, x);
                double actual = 2d * Ef.Pow(Constants.Pi / Math.Sin(Constants.Pi * x), 3) * Math.Cos(Constants.Pi * x);

                Doubles.AreEqual(
                    1,
                    Math.Abs(expected / actual),
                    1e-5,
                    $"Reflection: n={2} x={x}, {expected} ?= {actual}");
            }

            // Duplication.
            foreach (int n in GenerateLogUniformIntegerPositiveSamples(1, 100, 7))
            {
                foreach (double x in GenerateLogUniformRealPositiveSamples(1.0E-2, 1.0E4, 1000))
                {
                    double expected = Ef.Pow(2d, n + 1) * Sf.Polygamma(n, 2 * x);
                    double actual = Sf.Polygamma(n, x) + Sf.Polygamma(n, x + 0.5);

                    Doubles.AreEqual(
                        1,
                        Math.Abs(expected / actual),
                        1e-1,
                        $"Duplication 1: n={n} x={x}, {expected} ?= {actual}");

                    expected = Ef.Pow(3d, n + 1) * Sf.Polygamma(n, 3 * x);
                    actual = Sf.Polygamma(n, x) + Sf.Polygamma(n, x + 1 / 3d) + Sf.Polygamma(n, x + 2 / 3d);

                    Doubles.AreEqual(
                        1,
                        Math.Abs(expected / actual),
                        1e-1,
                        "Duplication 2: n={n} x={x}, {expected} ?= {actual}");
                }
            }
        }

        [TestMethod]
        public void Gamma_RegularizedGammaP_Properties()
        {
            // Recurrence.
            foreach (double a in GenerateLogUniformRealPositiveSamples(1.0e-4, 1.0e2, 500))
            {
                foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-4, 1.0e2, 500))
                {
                    double expected = Sf.RegularizedGammaP(a + 1, x) + Math.Pow(x, a) * Math.Exp(-x) / Sf.Gamma(a + 1);
                    double actual = Sf.RegularizedGammaP(a, x);

                    Doubles.AreEqual(
                        1,
                        Doubles.SafeRatio(expected, actual),
                        1e-3,
                        $"Recurrence: a={a}, x={x}, actual={actual}, expected={expected}");
                }
            }

            // Unitarity.
            foreach (double a in GenerateLogUniformRealPositiveSamples(1.0e-4, 1.0e4, 500))
            {
                foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-4, 1.0e4, 500))
                {
                    const double expected = 1;
                    double actual = Sf.RegularizedGammaP(a, x) + Sf.RegularizedGammaQ(a, x);

                    Doubles.AreEqual(
                        1,
                        Doubles.SafeRatio(expected, actual),
                        1e-4,
                        $"Unitarity: a={a}, x={x}, actual={actual}, expected={expected}");
                }
            }
        }

        [TestMethod]
        public void Gamma_RegularizedGammaQ_Properties()
        {
            // Exponential.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-04, 1.0e4, 2000))
            {
                double expected = Math.Exp(-x);
                double actual = Sf.RegularizedGammaQ(1d, x);

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected, actual),
                    1e-4,
                    $"Exponential: x={x}, actual={actual}, expected={expected}");
            }

            // Unitarity.
            foreach (double a in GenerateLogUniformRealPositiveSamples(1.0e-4, 1.0e4, 500))
            {
                foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-4, 1.0e4, 500))
                {
                    const double expected = 1;
                    double actual = Sf.RegularizedGammaP(a, x) + Sf.RegularizedGammaQ(a, x);

                    Doubles.AreEqual(
                        1,
                        Doubles.SafeRatio(expected, actual),
                        1e-4,
                        $"Unitarity: a={a}, x={x}, actual={actual}, expected={expected}");
                }
            }
        }

        [TestMethod]
        public void Gamma_IncompleteGammaUpper_Properties()
        {
            // Inequality.
            foreach (double a in GenerateLogUniformRealPositiveSamples(1.0e-4, 1d, 500))
            {
                foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-4, 10d, 500))
                {
                    double expected = Math.Exp(-x) * Math.Pow(x, a - 1);
                    double actual = Sf.IncompleteGammaUpper(a, x);

                    Assert.IsTrue(actual <= expected, $"Inequality: a={a}, x={x}, actual={actual}, expected={expected}");
                }
            }

            // Erfc.
            foreach (double x in GenerateLogUniformRealPositiveSamples(1.0e-04, 1.0e4, 2000))
            {
                double expected = Constants.SqrtPi * Sf.Erfc(x);
                double actual = Sf.IncompleteGammaUpper(0.5, x * x);

                Doubles.AreEqual(
                    1,
                    Doubles.SafeRatio(expected, actual),
                    1e-3,
                    $"Erfc: x={x}, actual={actual}, expected={expected}");
            }
        }

        /// <summary>
        /// Verification against Maple implementation.
        /// </summary>
        [TestMethod]
        public void Gamma_HarmonicNumber_Maple_Conformance()
        {
            // Exact.
            double sum = 0;
            for (int i = 1; i < 32; ++i)
            {
                sum += 1d / i;
                Doubles.AreEqual(sum, Sf.HarmonicNumber(i), $"Exact: H {i}");
            }

            // Approximation.
            for (int i = 32; i < 90; ++i)
            {
                sum += 1d / i;
                Doubles.AreEqual(sum, Sf.HarmonicNumber(i), $"Approximation: H {i}");
            }

            // Compare with Maple: "evalf(sum(1/k,k=1..x),20)"
            Doubles.AreEqual(
                12.090146129863427948,
                Sf.HarmonicNumber(100000),
                1e-10,
                "H 100000"); // 12.0901461298127
            Doubles.AreEqual(
                18.997896413853898325,
                Sf.HarmonicNumber(100000000),
                1e-10,
                "H 100000000"); // 18.9978964138137
        }

        /// <summary>
        /// Generates <c>n</c> positive real numbers distributed log-uniformly between <c>a</c> and <c>b</c>.
        /// </summary>
        /// <param name="a">The lower bound, must be positive.</param>
        /// <param name="b">The upper bound, must be positive.</param>
        /// <param name="n">Number of samples to generate.</param>
        private static IEnumerable<double> GenerateLogUniformRealPositiveSamples(double a, double b, int n)
        {
            double la = Math.Log(a);
            double lb = Math.Log(b);
            var rng = new Random(123456789);
            for (int i = 0; i < n; ++i)
            {
                yield return Math.Exp(la + (lb - la) * rng.NextDouble());
            }
        }

        /// <summary>
        /// Generates <c>n</c> positive integer numbers distributed log-uniformly between <c>a</c> and <c>b</c>.
        /// </summary>
        /// <param name="a">The lower bound, must be positive.</param>
        /// <param name="b">The upper bound, must be positive.</param>
        /// <param name="n">Number of samples to generate.</param>
        private static IEnumerable<int> GenerateLogUniformIntegerPositiveSamples(int a, int b, int n)
        {
            double la = Math.Log(a);
            double lb = Math.Log(b);
            var rng = new Random(1);
            for (int i = 0; i < n; ++i)
            {
                yield return (int)Math.Round(Math.Exp(la + (lb - la) * rng.NextDouble()));
            }
        }

        /// <summary>
        /// Generates <c>n</c> complex numbers distributed logarithmically between 10^<c>a</c> and 10^<c>b</c> in all four quadrants.
        /// </summary>
        /// <param name="a">The lower value, must be positive.</param>
        /// <param name="b">The upper value, must be positive.</param>
        /// <param name="n">Number of numbers to generate.</param>
        private static IEnumerable<Complex> GenerateComplexLogarithmicSamples(double a, double b, int n)
        {
            var rng = new Random(1);
            double la = Math.Log(a);
            double lb = Math.Log(b);
            for (int i = 0; i < n; ++i)
            {
                double re = Math.Exp(la + (lb - la) * rng.NextDouble());
                double im = Math.Exp(la + (lb - la) * rng.NextDouble());
                if (rng.NextDouble() < 0.5)
                {
                    re = -re;
                }

                if (rng.NextDouble() < 0.5)
                {
                    im = -im;
                }

                yield return new Complex(re, im);
            }
        }
    }
}
