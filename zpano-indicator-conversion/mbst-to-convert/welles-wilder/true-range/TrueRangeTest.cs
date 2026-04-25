using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Mbst.Trading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Mbst.Trading.Indicators;

namespace Tests.Indicators
{
    [TestClass]
    public class TrueRangeTest
    {
        #region Test data
        /// <summary>
        /// Input High test data, length = 252.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_data.c, TA_SREF_high_daily_ref_0_PRIV[252].
        /// </summary>
        private readonly List<double> inputHigh = new List<double>
        {
            93.250000,94.940000,96.375000,96.190000,96.000000,94.720000,95.000000,93.720000,92.470000,92.750000,96.250000,
            99.625000,99.125000,92.750000,91.315000,93.250000,93.405000,90.655000,91.970000,92.250000,90.345000,88.500000,
            88.250000,85.500000,84.440000,84.750000,84.440000,89.405000,88.125000,89.125000,87.155000,87.250000,87.375000,
            88.970000,90.000000,89.845000,86.970000,85.940000,84.750000,85.470000,84.470000,88.500000,89.470000,90.000000,
            92.440000,91.440000,92.970000,91.720000,91.155000,91.750000,90.000000,88.875000,89.000000,85.250000,83.815000,
            85.250000,86.625000,87.940000,89.375000,90.625000,90.750000,88.845000,91.970000,93.375000,93.815000,94.030000,
            94.030000,91.815000,92.000000,91.940000,89.750000,88.750000,86.155000,84.875000,85.940000,99.375000,103.280000,
            105.375000,107.625000,105.250000,104.500000,105.500000,106.125000,107.940000,106.250000,107.000000,108.750000,
            110.940000,110.940000,114.220000,123.000000,121.750000,119.815000,120.315000,119.375000,118.190000,116.690000,
            115.345000,113.000000,118.315000,116.870000,116.750000,113.870000,114.620000,115.310000,116.000000,121.690000,
            119.870000,120.870000,116.750000,116.500000,116.000000,118.310000,121.500000,122.000000,121.440000,125.750000,
            127.750000,124.190000,124.440000,125.750000,124.690000,125.310000,132.000000,131.310000,132.250000,133.880000,
            133.500000,135.500000,137.440000,138.690000,139.190000,138.500000,138.130000,137.500000,138.880000,132.130000,
            129.750000,128.500000,125.440000,125.120000,126.500000,128.690000,126.620000,126.690000,126.000000,123.120000,
            121.870000,124.000000,127.000000,124.440000,122.500000,123.750000,123.810000,124.500000,127.870000,128.560000,
            129.630000,124.870000,124.370000,124.870000,123.620000,124.060000,125.870000,125.190000,125.620000,126.000000,
            128.500000,126.750000,129.750000,132.690000,133.940000,136.500000,137.690000,135.560000,133.560000,135.000000,
            132.380000,131.440000,130.880000,129.630000,127.250000,127.810000,125.000000,126.810000,124.750000,122.810000,
            122.250000,121.060000,120.000000,123.250000,122.750000,119.190000,115.060000,116.690000,114.870000,110.870000,
            107.250000,108.870000,109.000000,108.500000,113.060000,93.000000,94.620000,95.120000,96.000000,95.560000,
            95.310000,99.000000,98.810000,96.810000,95.940000,94.440000,92.940000,93.940000,95.500000,97.060000,97.500000,
            96.250000,96.370000,95.000000,94.870000,98.250000,105.120000,108.440000,109.870000,105.000000,106.000000,
            104.940000,104.500000,104.440000,106.310000,112.870000,116.500000,119.190000,121.000000,122.120000,111.940000,
            112.750000,110.190000,107.940000,109.690000,111.060000,110.440000,110.120000,110.310000,110.440000,110.000000,
            110.750000,110.500000,110.500000,109.500000
        };

        /// <summary>
        /// Input Low test data, length = 252.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_data.c, TA_SREF_low_daily_ref_0_PRIV[252].
        /// </summary>
        private readonly List<double> inputLow = new List<double>
        {
            90.750000,91.405000,94.250000,93.500000,92.815000,93.500000,92.000000,89.750000,89.440000,90.625000,92.750000,
            96.315000,96.030000,88.815000,86.750000,90.940000,88.905000,88.780000,89.250000,89.750000,87.500000,86.530000,
            84.625000,82.280000,81.565000,80.875000,81.250000,84.065000,85.595000,85.970000,84.405000,85.095000,85.500000,
            85.530000,87.875000,86.565000,84.655000,83.250000,82.565000,83.440000,82.530000,85.065000,86.875000,88.530000,
            89.280000,90.125000,90.750000,89.000000,88.565000,90.095000,89.000000,86.470000,84.000000,83.315000,82.000000,
            83.250000,84.750000,85.280000,87.190000,88.440000,88.250000,87.345000,89.280000,91.095000,89.530000,91.155000,
            92.000000,90.530000,89.970000,88.815000,86.750000,85.065000,82.030000,81.500000,82.565000,96.345000,96.470000,
            101.155000,104.250000,101.750000,101.720000,101.720000,103.155000,105.690000,103.655000,104.000000,105.530000,
            108.530000,108.750000,107.750000,117.000000,118.000000,116.000000,118.500000,116.530000,116.250000,114.595000,
            110.875000,110.500000,110.720000,112.620000,114.190000,111.190000,109.440000,111.560000,112.440000,117.500000,
            116.060000,116.560000,113.310000,112.560000,114.000000,114.750000,118.870000,119.000000,119.750000,122.620000,
            123.000000,121.750000,121.560000,123.120000,122.190000,122.750000,124.370000,128.000000,129.500000,130.810000,
            130.630000,132.130000,133.880000,135.380000,135.750000,136.190000,134.500000,135.380000,133.690000,126.060000,
            126.870000,123.500000,122.620000,122.750000,123.560000,125.810000,124.620000,124.370000,121.810000,118.190000,
            118.060000,117.560000,121.000000,121.120000,118.940000,119.810000,121.000000,122.000000,124.500000,126.560000,
            123.500000,121.250000,121.060000,122.310000,121.000000,120.870000,122.060000,122.750000,122.690000,122.870000,
            125.500000,124.250000,128.000000,128.380000,130.690000,131.630000,134.380000,132.000000,131.940000,131.940000,
            129.560000,123.750000,126.000000,126.250000,124.370000,121.440000,120.440000,121.370000,121.690000,120.000000,
            119.620000,115.500000,116.750000,119.060000,119.060000,115.060000,111.060000,113.120000,110.000000,105.000000,
            104.690000,103.870000,104.690000,105.440000,107.000000,89.000000,92.500000,92.120000,94.620000,92.810000,
            94.250000,96.250000,96.370000,93.690000,93.500000,90.000000,90.190000,90.500000,92.120000,94.120000,94.870000,
            93.000000,93.870000,93.000000,92.620000,93.560000,98.370000,104.440000,106.000000,101.810000,104.120000,
            103.370000,102.120000,102.250000,103.370000,107.940000,112.500000,115.440000,115.500000,112.250000,107.560000,
            106.560000,106.870000,104.500000,105.750000,108.620000,107.750000,108.060000,108.000000,108.190000,108.120000,
            109.060000,108.750000,108.560000,106.620000
        };

        /// <summary>
        /// Input Close test data, length = 252.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_data.c, TA_SREF_close_daily_ref_0_PRIV[252].
        /// </summary>
        private readonly List<double> inputClose = new List<double>
        {
            91.500000,94.815000,94.375000,95.095000,93.780000,94.625000,92.530000,92.750000,90.315000,92.470000,96.125000,
            97.250000,98.500000,89.875000,91.000000,92.815000,89.155000,89.345000,91.625000,89.875000,88.375000,87.625000,
            84.780000,83.000000,83.500000,81.375000,84.440000,89.250000,86.375000,86.250000,85.250000,87.125000,85.815000,
            88.970000,88.470000,86.875000,86.815000,84.875000,84.190000,83.875000,83.375000,85.500000,89.190000,89.440000,
            91.095000,90.750000,91.440000,89.000000,91.000000,90.500000,89.030000,88.815000,84.280000,83.500000,82.690000,
            84.750000,85.655000,86.190000,88.940000,89.280000,88.625000,88.500000,91.970000,91.500000,93.250000,93.500000,
            93.155000,91.720000,90.000000,89.690000,88.875000,85.190000,83.375000,84.875000,85.940000,97.250000,99.875000,
            104.940000,106.000000,102.500000,102.405000,104.595000,106.125000,106.000000,106.065000,104.625000,108.625000,
            109.315000,110.500000,112.750000,123.000000,119.625000,118.750000,119.250000,117.940000,116.440000,115.190000,
            111.875000,110.595000,118.125000,116.000000,116.000000,112.000000,113.750000,112.940000,116.000000,120.500000,
            116.620000,117.000000,115.250000,114.310000,115.500000,115.870000,120.690000,120.190000,120.750000,124.750000,
            123.370000,122.940000,122.560000,123.120000,122.560000,124.620000,129.250000,131.000000,132.250000,131.000000,
            132.810000,134.000000,137.380000,137.810000,137.880000,137.250000,136.310000,136.250000,134.630000,128.250000,
            129.000000,123.870000,124.810000,123.000000,126.250000,128.380000,125.370000,125.690000,122.250000,119.370000,
            118.500000,123.190000,123.500000,122.190000,119.310000,123.310000,121.120000,123.370000,127.370000,128.500000,
            123.870000,122.940000,121.750000,124.440000,122.000000,122.370000,122.940000,124.000000,123.190000,124.560000,
            127.250000,125.870000,128.860000,132.000000,130.750000,134.750000,135.000000,132.380000,133.310000,131.940000,
            130.000000,125.370000,130.130000,127.120000,125.190000,122.000000,125.000000,123.000000,123.500000,120.060000,
            121.000000,117.750000,119.870000,122.000000,119.190000,116.370000,113.500000,114.250000,110.000000,105.060000,
            107.000000,107.870000,107.000000,107.120000,107.000000,91.000000,93.940000,93.870000,95.500000,93.000000,
            94.940000,98.250000,96.750000,94.810000,94.370000,91.560000,90.250000,93.940000,93.620000,97.000000,95.000000,
            95.870000,94.060000,94.620000,93.750000,98.000000,103.940000,107.870000,106.060000,104.500000,105.000000,
            104.190000,103.060000,103.420000,105.270000,111.870000,116.000000,116.620000,118.280000,113.370000,109.000000,
            109.700000,109.250000,107.000000,109.190000,110.000000,109.200000,110.120000,108.000000,108.620000,109.750000,
            109.810000,109.000000,108.750000,107.870000
        };

        /// <summary>
        /// Output data.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_trange.c.
        /// <para><code>
        /// static TA_Test tableTest[] =
        /// {
        ///   /* TRANGE TEST */
        ///   { 1, 0, 0, 251, 0,  0, TA_SUCCESS,   0,  3.535,  1,  251 }, /* First Value */
        ///   { 0, 0, 0, 251, 0,  0, TA_SUCCESS,  12,  9.685,  1,  251 },
        ///   { 0, 0, 0, 251, 0,  0, TA_SUCCESS,  40,  5.125,  1,  251 },
        ///   { 0, 0, 0, 251, 0,  0, TA_SUCCESS, 250,  2.88,   1,  251 }, /* Last Value */
        /// </code></para>
        /// </summary>
        private readonly List<double> expected = new List<double>
        {
            3.535, // Index=0 (1) value.
            9.685, // Index=12 (13) value.
            5.125, // Index=40 (41) value.
            2.88   // Index=250 (251) value (last).
        };

        /// <summary>
        /// True range test data, length = 252.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_trange.xls, F4:F255.
        /// </summary>
        private readonly List<double> expectedTr = new List<double>
        {
            double.NaN/*2.500*/, 3.535, 2.125, 2.690, 3.185, 1.220, 3.000, 3.970, 3.310, 2.435,
            3.780, 3.500, 3.095, 9.685, 4.565, 2.310, 4.500, 1.875, 2.720, 2.500,
            2.845, 1.970, 3.625, 3.220, 2.875, 3.875, 3.190, 5.340, 3.655, 3.155,
            2.750, 2.155, 1.875, 3.440, 2.125, 3.280, 2.315, 3.565, 2.310, 2.030,
            1.940, 5.125, 3.970, 1.470, 3.160, 1.315, 2.220, 2.720, 2.590, 1.655,
            1.500, 2.560, 5.000, 1.935, 1.815, 2.560, 1.875, 2.660, 3.185, 2.185,
            2.500, 1.500, 3.470, 2.280, 4.285, 2.875, 2.030, 2.625, 2.030, 3.125,
            3.000, 3.810, 4.125, 3.375, 3.375,13.435, 6.810, 5.500, 3.375, 4.250,
            2.780, 3.780, 2.970, 2.250, 2.595, 3.000, 4.125, 2.410, 2.190, 6.470,
           10.250, 5.000, 3.815, 1.815, 2.845, 1.940, 2.095, 4.470, 2.500, 7.720,
            5.505, 2.560, 4.810, 5.180, 3.750, 3.560, 5.690, 4.440, 4.310, 3.690,
            3.940, 2.000, 3.560, 5.630, 3.000, 1.690, 5.000, 4.750, 2.440, 2.880,
            3.190, 2.500, 2.750, 7.630, 3.310, 2.750, 3.070, 2.870, 3.370, 3.560,
            3.310, 3.440, 2.310, 3.630, 2.120, 5.190, 8.570, 2.880, 5.500, 2.820,
            2.370, 3.500, 2.880, 3.760, 2.320, 4.190, 4.930, 3.810, 6.440, 6.000,
            3.320, 3.560, 4.440, 2.810, 3.380, 4.500, 2.000, 6.130, 3.620, 3.310,
            3.120, 3.440, 3.190, 3.810, 2.440, 2.930, 3.130, 3.940, 3.000, 3.880,
            4.310, 3.250, 5.750, 3.310, 3.560, 1.620, 3.060, 2.820, 7.690, 5.510,
            3.880, 2.880, 6.370, 4.560, 5.440, 3.060, 3.500, 2.630, 5.560, 3.250,
            4.190, 3.690, 4.130, 5.310, 3.570, 4.870, 5.870, 2.560, 5.000, 4.310,
            3.060, 6.060,18.000, 3.620, 3.000, 2.130, 2.750, 2.310, 4.060, 2.440,
            3.120, 2.440, 4.440, 2.750, 3.690, 3.380, 3.440, 2.630, 3.250, 2.500,
            2.000, 2.250, 4.690, 7.120, 4.500, 3.870, 4.250, 1.880, 1.630, 2.380,
            2.190, 2.940, 7.600, 4.630, 3.750, 5.500, 9.870, 5.810, 6.190, 3.320,
            4.750, 3.940, 2.440, 2.690, 2.060, 2.310, 2.440, 1.880, 1.690, 1.750,
            1.940, 2.880
        };
        #endregion

        #region NameTest
        /// <summary>
        /// A test for Name.
        /// </summary>
        [TestMethod]
        public void NameTest()
        {
            var target = new TrueRange();
            Assert.AreEqual("tr", target.Name);
        }
        #endregion

        #region MonikerTest
        /// <summary>
        /// A test for Moniker.
        /// </summary>
        [TestMethod]
        public void MonikerTest()
        {
            var target = new TrueRange();
            Assert.AreEqual("tr", target.Moniker);
        }
        #endregion

        #region DescriptionTest
        /// <summary>
        /// A test for Description.
        /// </summary>
        [TestMethod]
        public void DescriptionTest()
        {
            var target = new TrueRange();
            Assert.AreEqual("True Range", target.Description);
        }
        #endregion

        #region IsPrimedTest
        /// <summary>
        /// A test for IsPrimed.
        /// </summary>
        [TestMethod]
        public void IsPrimedTest()
        {
            var target = new TrueRange();
            Assert.IsFalse(target.IsPrimed);
            target.Update(inputClose[0], inputHigh[0], inputLow[0]);
            Assert.IsFalse(target.IsPrimed);
            target.Update(inputClose[1], inputHigh[1], inputLow[1]);
            Assert.IsTrue(target.IsPrimed);
            target.Update(inputClose[2], inputHigh[2], inputLow[2]);
            Assert.IsTrue(target.IsPrimed);
        }
        #endregion

        #region TaLibTest
        /// <summary>
        /// A TA-Lib data test.
        /// </summary>
        [TestMethod]
        public void TaLibTestSmall()
        {
            const int dec = 3;
            var target = new TrueRange();
            target.Update(inputClose[0], inputHigh[0], inputLow[0]);
            double d = target.Update(inputClose[1], inputHigh[1], inputLow[1]);
            Assert.AreEqual(expected[0], Math.Round(d, dec)); // Index = 1.
            for (int i = 2; i < 14; ++i)
                d = target.Update(inputClose[i], inputHigh[i], inputLow[i]);
            Assert.AreEqual(expected[1], Math.Round(d, dec)); // Index = 13.
            for (int i = 14; i < 42; ++i)
                d = target.Update(inputClose[i], inputHigh[i], inputLow[i]);
            Assert.AreEqual(expected[2], Math.Round(d, dec)); // Index = 41.
            for (int i = 42; i < 252; ++i)
                d = target.Update(inputClose[i], inputHigh[i], inputLow[i]);
            Assert.AreEqual(expected[3], Math.Round(d, dec)); // Index = 251.
        }

        /// <summary>
        /// A TA-Lib data test.
        /// </summary>
        [TestMethod]
        public void TaLibTestBig()
        {
            const int dec = 3;
            var target = new TrueRange();
            for (int i = 0; i < inputClose.Count; ++i)
            {
                double d = target.Update(inputClose[i], inputHigh[i], inputLow[i]);
                Assert.AreEqual(expectedTr[i], Math.Round(d, dec));
            }
        }
        #endregion

        #region UpdateTest
        /// <summary>
        /// A test for Update.
        /// </summary>
        [TestMethod]
        public void UpdateTest()
        {
            const int dec = 3;
            var target = new TrueRange();
            for (int i = 0; i < inputClose.Count; ++i)
            {
                double d = target.Update(inputClose[i], inputHigh[i], inputLow[i]);
                Assert.AreEqual(expectedTr[i], Math.Round(d, dec));
            }
        }

        /// <summary>
        /// A test for Update.
        /// </summary>
        [TestMethod]
        public void UpdateTest2()
        {
            const int dec = 3;
            DateTime dateTime = DateTime.Now;
            var target = new TrueRange();
            for (int i = 0; i < inputClose.Count; ++i)
            {
                double d = target.Update(inputClose[i], inputHigh[i], inputLow[i], dateTime).Value;
                Assert.AreEqual(expectedTr[i], Math.Round(d, dec));
            }
        }

        /// <summary>
        /// A test for Update.
        /// </summary>
        [TestMethod]
        public void UpdateTest3()
        {
            const int dec = 3;
            DateTime dateTime = DateTime.Now;
            var ohlcv = new Ohlcv(dateTime, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);
            var target = new TrueRange();
            for (int i = 0; i < inputClose.Count; ++i)
            {
                ohlcv.Close = inputClose[i];
                ohlcv.High = inputHigh[i];
                ohlcv.Low = inputLow[i];
                double d = target.Update(ohlcv).Value;
                Assert.AreEqual(expectedTr[i], Math.Round(d, dec));
            }
        }

        /// <summary>
        /// A test for Update.
        /// </summary>
        [TestMethod]
        public void UpdateTest4()
        {
            DateTime dateTime = DateTime.Now;
            var scalar = new Scalar(dateTime, double.NaN);
            var target1 = new TrueRange();
            var target2 = new TrueRange();
            var target3 = new TrueRange();
            foreach (double t in inputClose)
            {
                double d1 = target1.Update(t);
                double d2 = target2.Update(t, dateTime).Value;
                scalar.Value = t;
                double d3 = target3.Update(scalar).Value;
                Assert.AreEqual(d1, d2);
                Assert.AreEqual(d1, d3);
            }
            double z = target1.Update(double.NaN);
            Assert.IsTrue(double.IsNaN(z));
        }
        #endregion

        #region ValueFacadeTest
        /// <summary>
        /// A test for ValueFacade.
        /// </summary>
        [TestMethod]
        public void ValueFacadeTest()
        {
            var target = new TrueRange();
            var facade = target.ValueFacade;
            Assert.AreEqual("tr", facade.Name);
            Assert.AreEqual("tr", facade.Moniker);
            Assert.AreEqual("True Range", facade.Description);
            for (int i = 0; i < inputClose.Count; ++i)
            {
                double d = target.Update(inputClose[i], inputHigh[i], inputLow[i]);
                double u = facade.Update(inputClose[i]);
                Assert.AreEqual(d, u);
                Assert.AreEqual(target.IsPrimed, facade.IsPrimed);
            }
        }
        #endregion

        #region ResetTest
        /// <summary>
        /// A test for Reset.
        /// </summary>
        [TestMethod]
        public void ResetTest()
        {
            const int dec = 3;
            var target = new TrueRange();
            for (int i = 0; i < inputClose.Count; ++i)
            {
                double d = target.Update(inputClose[i], inputHigh[i], inputLow[i]);
                Assert.AreEqual(expectedTr[i], Math.Round(d, dec));
            }
            target.Reset();
            for (int i = 0; i < inputClose.Count; ++i)
            {
                double d = target.Update(inputClose[i], inputHigh[i], inputLow[i]);
                Assert.AreEqual(expectedTr[i], Math.Round(d, dec));
            }
        }
        #endregion

        #region ConstructorTest
        /// <summary>
        /// A test for TrueRange Constructor.
        /// </summary>
        [TestMethod]
        public void TrueRangeConstructorTest()
        {
            var target = new TrueRange();
            Assert.IsTrue(double.IsNaN(target.Value));
            Assert.IsFalse(target.IsPrimed);
        }
        #endregion

        #region ToStringTest
        /// <summary>
        /// A test for ToString.
        /// </summary>
        [TestMethod]
        public void ToStringTest()
        {
            var target = new TrueRange();
            Assert.AreEqual("[M:tr P:False V:NaN]", target.ToString());
        }
        #endregion

        #region SerializationTest
        private static void SerializeTo(TrueRange instance, string fileName)
        {
            var dcs = new DataContractSerializer(typeof(TrueRange), null, 65536, false, true, null);
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                dcs.WriteObject(fs, instance);
                fs.Close();
            }
        }

        private static TrueRange DeserializeFrom(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            var ser = new DataContractSerializer(typeof(TrueRange), null, 65536, false, true, null);
            var instance = (TrueRange)ser.ReadObject(reader, true);
            reader.Close();
            fs.Close();
            return instance;
        }

        /// <summary>
        /// A test for the serialization.
        /// </summary>
        [TestMethod]
        public void SerializationTest()
        {
            const int dec = 3;
            var source = new TrueRange();
            for (int i = 0; i < 111; ++i)
            {
                double d = source.Update(inputClose[i], inputHigh[i], inputLow[i]);
                Assert.AreEqual(expectedTr[i], Math.Round(d, dec));
            }
            const string fileName = "TrueRangeTest_1.xml";
            SerializeTo(source, fileName);
            TrueRange target = DeserializeFrom(fileName);
            Assert.IsTrue(target.IsPrimed);
            Assert.AreEqual(expectedTr[110], Math.Round(target.Value, dec));
            Assert.AreEqual("tr", target.Name);
            for (int i = 111; i < inputClose.Count; ++i)
            {
                double d = target.Update(inputClose[i], inputHigh[i], inputLow[i]);
                Assert.AreEqual(expectedTr[i], Math.Round(d, dec));
            }
            //FileInfo fi = new FileInfo(fileName);
            //fi.Delete();
        }
        #endregion
    }
}
