using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mbst.Trading.Indicators;

namespace Tests.Indicators
{
    /// <summary>
    /// This is a test class for SlowTrendLineMomentumTest and is intended to contain all SlowTrendLineMomentumTest Unit Tests.
    /// </summary>
    [TestClass]
    public class SlowTrendLineMomentumTest
    {
        #region NameTest
        /// <summary>
        /// A test for Name.
        /// </summary>
        [TestMethod]
        public void NameTest()
        {
            var target = new SlowTrendLineMomentum();
            Assert.AreEqual("STLM", target.Name);
        }
        #endregion

        #region MonikerTest
        /// <summary>
        /// A test for Moniker.
        /// </summary>
        [TestMethod]
        public void MonikerTest()
        {
            var target = new SlowTrendLineMomentum();
            Assert.AreEqual("STLM", target.Moniker);
        }
        #endregion

        #region DescriptionTest
        /// <summary>
        /// A test for Description.
        /// </summary>
        [TestMethod]
        public void DescriptionTest()
        {
            var target = new SlowTrendLineMomentum();
            Assert.AreEqual("Slow Trend Line Momentum", target.Description);
        }
        #endregion

        #region IsPrimedTest
        /// <summary>
        /// A test for IsPrimed.
        /// </summary>
        [TestMethod]
        public void IsPrimedTest()
        {
            var target = new SlowTrendLineMomentum();
            Assert.IsFalse(target.IsPrimed);
            for (int i = 0; i < target.Length - 1; i++)
            {
                target.Update(1d);
                Assert.IsFalse(target.IsPrimed);
            }
            for (int i = 0; i < 100; i++)
            {
                target.Update(1d);
                Assert.IsTrue(target.IsPrimed);
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
            var target = new SlowTrendLineMomentum();
            Assert.IsFalse(target.IsPrimed);
            for (int i = 0; i < target.Length - 1; i++)
            {
                target.Update(1d);
                Assert.IsFalse(target.IsPrimed);
            }
            for (int i = 0; i < 100; i++)
            {
                target.Update(1d);
                Assert.IsTrue(target.IsPrimed);
            }
            target.Reset();
            Assert.IsFalse(target.IsPrimed);
            for (int i = 0; i < target.Length - 1; i++)
            {
                target.Update(1d);
                Assert.IsFalse(target.IsPrimed);
            }
            for (int i = 0; i < 100; i++)
            {
                target.Update(1d);
                Assert.IsTrue(target.IsPrimed);
            }
        }
        #endregion

        #region SlowTrendLineMomentumConstructorTest
        /// <summary>
        /// A test for SlowTrendLineMomentum Constructor.
        /// </summary>
        [TestMethod]
        public void SlowTrendLineMomentumConstructorTest()
        {
            var target = new SlowTrendLineMomentum();
            Assert.IsTrue(double.IsNaN(target.Value));
            Assert.IsFalse(target.IsPrimed);
        }
        #endregion

        #region SerializationTest
        private static void SerializeTo(SlowTrendLineMomentum instance, string fileName)
        {
            var dcs = new DataContractSerializer(typeof(SlowTrendLineMomentum), null, 65536, false, true, null);
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                dcs.WriteObject(fs, instance);
                fs.Close();
            }
        }

        private static SlowTrendLineMomentum DeserializeFrom(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            var ser = new DataContractSerializer(typeof(SlowTrendLineMomentum), null, 65536, false, true, null);
            var instance = (SlowTrendLineMomentum)ser.ReadObject(reader, true);
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
            const int dec = 6;
            var source = new SlowTrendLineMomentum();
            Assert.IsFalse(source.IsPrimed);
            for (int i = 0; i < source.Length - 1; i++)
            {
                source.Update(1d);
                Assert.IsFalse(source.IsPrimed);
            }
            for (int i = 0; i < 20; i++)
            {
                source.Update(1d);
                Assert.IsTrue(source.IsPrimed);
            }
            const string fileName = "SlowTrendLineMomentumTest_1.xml";
            SerializeTo(source, fileName);
            SlowTrendLineMomentum target = DeserializeFrom(fileName);
            Assert.IsTrue(target.IsPrimed);
            Assert.AreEqual(Math.Round(source.Value, dec), Math.Round(target.Value, dec));
            Assert.AreEqual("STLM", target.Name);
            Assert.AreEqual("Slow Trend Line Momentum", target.Description);
            for (int i = 0; i < 20; i++)
            {
                target.Update(1d);
                Assert.IsTrue(target.IsPrimed);
            }
            //FileInfo fi = new FileInfo(fileName);
            //fi.Delete();
        }
        #endregion
    }
}
