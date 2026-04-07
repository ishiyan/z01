using Mbs.Trading.Data.Generators;
using Mbs.Trading.Data.Generators.GeometricBrownianMotion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mbs.UnitTests.Trading.Data.Generators.GeometricBrownianMotion
{
    [TestClass]
    public class GeometricBrownianMotionDataGeneratorTests
    {
        [TestMethod]
        public void GeometricBrownianMotionDataGenerator_Construction_DefaultConstructor_PropertyValuesCorrect()
        {
            var generator = new GeometricBrownianMotionOhlcvGenerator(new GeometricBrownianMotionOhlcvGeneratorParameters());

            Assert.AreEqual(DefaultParameterValues.GbmAmplitude, generator.SampleAmplitude, "default sample amplitude");
            Assert.AreEqual(DefaultParameterValues.GbmMinimalValue, generator.SampleMinimum, "default sample minimum");
            Assert.AreEqual(DefaultParameterValues.GbmDrift, generator.Drift, "default drift");
            Assert.AreEqual(DefaultParameterValues.GbmVolatility, generator.Volatility, "default volatility");
        }
    }
}
