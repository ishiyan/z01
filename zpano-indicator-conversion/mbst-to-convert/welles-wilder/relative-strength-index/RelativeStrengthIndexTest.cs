using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Mbst.Trading;
using Mbst.Trading.Indicators;

namespace Tests.Indicators
{
    [TestClass]
    public class RelativeStrengthIndexTest
    {
        #region Test data
        /// <summary>
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_RSI.xsl, Data, A3…A27, 25 entries, length=9.
        /// </summary>
        private readonly List<double> input = new List<double>
        {
            91.150,  90.500,  92.550,  94.700,  95.550,  94.000,  91.300, 91.950, 92.450,
            93.800,  92.500,  94.550,  96.750,  97.800,  98.400,  98.150, 96.700, 98.850,
            98.900, 100.500, 102.600, 104.800, 103.800, 103.100, 102.000
        };

        /// <summary>
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_RSI.xsl, RSI, H3…H27, 25 entries, length=9.
        /// </summary>
        private readonly List<double> expected = new List<double>
        {
            double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
            60.64257028112440, 54.26774483378260, 61.45581901651760, 67.60347673886670, 70.15901914813830,
            71.59924009048510, 70.01525894477660, 61.18333613249870, 67.93122493185930, 68.07641783697100,
            72.55046462962620, 77.25688473856160, 81.08011235708990, 74.66196805072280, 70.28087138459060,
            63.67542155063880
        };

        /// <summary>
        /// Input Close test data, length = 252.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_data.c, TA_SREF_close_daily_ref_0_PRIV[252].
        /// </summary>
        private readonly List<double> input2 = new List<double>
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
        /// Output data, length=14.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_rsi.c.
        /// <para><code>
        /// static TA_Test tableTest[] =
        /// {
        ///    /**********************/
        ///    /*      RSI TEST      */
        ///    /**********************/
        ///    { 1, TA_RSI_TEST, 0, 0, 251, 14, TA_COMPATIBILITY_DEFAULT, TA_SUCCESS,      0, 49.14,  14,  252-14 }, /* First Value */
        ///    { 0, TA_RSI_TEST, 0, 0, 251, 14, TA_COMPATIBILITY_DEFAULT, TA_SUCCESS,      1, 52.32,  14,  252-14 },
        ///    { 0, TA_RSI_TEST, 0, 0, 251, 14, TA_COMPATIBILITY_DEFAULT, TA_SUCCESS,      2, 46.07,  14,  252-14 },
        ///    { 0, TA_RSI_TEST, 0, 0, 251, 14, TA_COMPATIBILITY_DEFAULT, TA_SUCCESS, 252-15, 49.63,  14,  252-14 },  /* Last Value */
        /// </code></para>
        /// </summary>
        private readonly List<double> expected2 = new List<double>
        {
            // Begins with 14 double.NaN values.
            49.14, // Index=14 value.
            52.32, // Index=15 value.
            46.07, // Index=16 value.
            49.63  // Index=251 (last) value.
        };
        #endregion

        #region NameTest
        /// <summary>
        /// A test for Name.
        /// </summary>
        [TestMethod]
        public void NameTest()
        {
            var target = new RelativeStrengthIndex(5);
            Assert.AreEqual("RSI", target.Name);
        }
        #endregion

        #region MonikerTest
        /// <summary>
        /// A test for Moniker.
        /// </summary>
        [TestMethod]
        public void MonikerTest()
        {
            var target = new RelativeStrengthIndex(5);
            Assert.AreEqual("RSI5", target.Moniker);
        }
        #endregion

        #region DescriptionTest
        /// <summary>
        /// A test for Description.
        /// </summary>
        [TestMethod]
        public void DescriptionTest()
        {
            var target = new RelativeStrengthIndex(5);
            Assert.AreEqual("Relative Strength Index", target.Description);
        }
        #endregion

        #region IsPrimedTest
        /// <summary>
        /// A test for IsPrimed.
        /// </summary>
        [TestMethod]
        public void IsPrimedTest()
        {
            var target = new RelativeStrengthIndex(5);
            Assert.IsFalse(target.IsPrimed);
            var scalar = new Scalar(DateTime.Now, 1d);
            for (int i = 1; i < 6; ++i)
            {
                scalar.Value = i;
                target.Update(scalar);
                Assert.IsFalse(target.IsPrimed);
            }
            for (int i = 6; i < 12; ++i)
            {
                scalar.Value = i;
                target.Update(scalar);
                Assert.IsTrue(target.IsPrimed);
            }
        }
        #endregion

        #region TaLibTest
        /// <summary>
        /// A test for Value.
        /// </summary>
        [TestMethod]
        public void TaLibTest()
        {
            double u; const int digits = 1;
            var target = new RelativeStrengthIndex(14);
            for (int i = 0; i < 14; ++i)
            {
                u = target.Update(input2[i]);
                Assert.IsTrue(double.IsNaN(u));
            }
            u = target.Update(input2[14]);
            Assert.IsFalse(double.IsNaN(u));
            Assert.AreEqual(u, target.Value);
            double d = Math.Round(target.Value, digits);
            u = Math.Round(expected2[0], digits);
            Assert.AreEqual(u, d);
            u = target.Update(input2[15]);
            Assert.IsFalse(double.IsNaN(u));
            Assert.AreEqual(u, target.Value);
            d = Math.Round(target.Value, digits);
            u = Math.Round(expected2[1], digits);
            Assert.AreEqual(u, d);
            u = target.Update(input2[16]);
            Assert.IsFalse(double.IsNaN(u));
            Assert.AreEqual(u, target.Value);
            d = Math.Round(target.Value, digits);
            u = Math.Round(expected2[2], digits);
            Assert.AreEqual(u, d);
            for (int i = 17; i < 251; ++i)
                target.Update(input2[i]);
            u = target.Update(input2[251]);
            Assert.IsFalse(double.IsNaN(u));
            Assert.AreEqual(u, target.Value);
            d = Math.Round(target.Value, digits);
            u = Math.Round(expected2[3], digits);
            Assert.AreEqual(u, d);
        }
        #endregion

        #region ValueTest
        /// <summary>
        /// A test for Value.
        /// </summary>
        [TestMethod]
        public void ValueTest()
        {
            double u;
            const int digits = 9;
            var target = new RelativeStrengthIndex(9);
            for (int i = 0; i < 9; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsTrue(double.IsNaN(u));
            }
            for (int i = 9; i < input.Count; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                double d = Math.Round(target.Value, digits);
                u = Math.Round(expected[i], digits);
                Assert.AreEqual(u, d);
            }
        }
        #endregion

        #region LengthTest
        /// <summary>
        /// A test for Length.
        /// </summary>
        [TestMethod]
        public void LengthTest()
        {
            var target = new RelativeStrengthIndex(11);
            Assert.AreEqual(11, target.Length);
            target = new RelativeStrengthIndex(22);
            Assert.AreEqual(22, target.Length);
        }
        #endregion

        #region UpdateTest
        /// <summary>
        /// A test for Update.
        /// </summary>
        [TestMethod]
        public void UpdateTest()
        {
            double u;
            const int digits = 9;
            var scalar = new Scalar(DateTime.Now, 1d);
            var target = new RelativeStrengthIndex(9);
            for (int i = 0; i < 9; ++i)
            {
                scalar.Value = input[i];
                u = target.Update(scalar).Value;
                Assert.IsTrue(double.IsNaN(u));
            }
            for (int i = 9; i < input.Count; ++i)
            {
                scalar.Value = input[i];
                u = target.Update(scalar).Value;
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                double d = Math.Round(target.Value, digits);
                u = Math.Round(expected[i], digits);
                Assert.AreEqual(u, d);
            }
        }
        #endregion

        #region CalculateTest
        /// <summary>
        /// A test for Calculate.
        /// </summary>
        [TestMethod]
        public void CalculateTest()
        {
            List<double> actual = RelativeStrengthIndex.Calculate(input, 9);
            for (int i = 0; i < 9; ++i)
                Assert.IsTrue(double.IsNaN(actual[i]));
            for (int i = 9; i < input.Count; ++i)
                Assert.AreEqual(Math.Round(expected[i], 9), Math.Round(actual[i], 9));
        }
        #endregion

        #region ResetTest
        /// <summary>
        /// A test for Reset.
        /// </summary>
        [TestMethod]
        public void ResetTest()
        {
            double d, u; const int digits = 9;
            var target = new RelativeStrengthIndex(9);
            for (int i = 0; i < 9; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsTrue(double.IsNaN(u));
            }
            for (int i = 9; i < input.Count; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                d = Math.Round(target.Value, digits);
                u = Math.Round(expected[i], digits);
                Assert.AreEqual(u, d);
            }
            target.Reset();
            for (int i = 0; i < 9; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsTrue(double.IsNaN(u));
            }
            for (int i = 9; i < input.Count; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                d = Math.Round(target.Value, digits);
                u = Math.Round(expected[i], digits);
                Assert.AreEqual(u, d);
            }
        }
        #endregion

        #region RelativeStrengthIndexConstructorTest
        /// <summary>
        /// A test for RelativeStrengthIndex Constructor.
        /// </summary>
        [TestMethod]
        public void RelativeStrengthIndexConstructorTest()
        {
            var target = new RelativeStrengthIndex(5);
            Assert.AreEqual(5, target.Length);
            Assert.IsTrue(double.IsNaN(target.Value));
            Assert.IsFalse(target.IsPrimed);
        }

        /// <summary>
        /// A test for RelativeStrengthIndex Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RelativeStrengthIndexConstructorTest2()
        {
            var target = new RelativeStrengthIndex(1);
            Assert.IsNotNull(target);
        }

        /// <summary>
        /// A test for RelativeStrengthIndex Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RelativeStrengthIndexConstructorTest3()
        {
            var target = new RelativeStrengthIndex(-8);
            Assert.IsNotNull(target);
        }
        #endregion

        #region SerializationTest
        private static void SerializeTo(RelativeStrengthIndex instance, string fileName)
        {
            var dcs = new DataContractSerializer(typeof(RelativeStrengthIndex), null, 65536, false, true, null);
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                dcs.WriteObject(fs, instance);
                fs.Close();
            }
        }

        private static RelativeStrengthIndex DeserializeFrom(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            var ser = new DataContractSerializer(typeof(RelativeStrengthIndex), null, 65536, false, true, null);
            var instance = (RelativeStrengthIndex)ser.ReadObject(reader, true);
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
            double d, u; const int digits = 9;
            var source = new RelativeStrengthIndex(9);
            for (int i = 0; i < 9; ++i)
            {
                u = source.Update(input[i]);
                Assert.IsTrue(double.IsNaN(u));
            }
            for (int i = 9; i < 15; ++i)
            {
                u = source.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, source.Value);
                d = Math.Round(source.Value, digits);
                u = Math.Round(expected[i], digits);
                Assert.AreEqual(u, d);
            }
            const string fileName = "RelativeStrengthIndexTest_1.xml";
            SerializeTo(source, fileName);
            RelativeStrengthIndex target = DeserializeFrom(fileName);
            Assert.AreEqual(9, target.Length);
            Assert.IsTrue(target.IsPrimed);
            Assert.AreEqual("RSI", target.Name);
            Assert.AreEqual(source.Value, target.Value);
            Assert.AreEqual("Relative Strength Index", target.Description);
            for (int i = 15; i < input.Count; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                d = Math.Round(target.Value, digits);
                u = Math.Round(expected[i], digits);
                Assert.AreEqual(u, d);
            }
            //FileInfo fi = new FileInfo(fileName);
            //fi.Delete();
        }
        #endregion
    }
}
