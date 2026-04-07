using Mbs.Trading.Data.Generators;
using Mbs.Trading.Data.Generators.FractionalBrownianMotion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Mbs.UnitTests.Trading.Data.Generators.FractionalBrownianMotion
{
    [TestClass]
    public class FractionalBrownianMotionDataGeneratorTests
    {
        [TestMethod]
        public void FractionalBrownianMotionDataGenerator_Construction_DefaultConstructor_PropertyValuesCorrect()
        {
            var generator = new FractionalBrownianMotionOhlcvGenerator(new FractionalBrownianMotionOhlcvGeneratorParameters());

            Assert.AreEqual(DefaultParameterValues.FbmAmplitude, generator.SampleAmplitude, "default sample amplitude");
            Assert.AreEqual(DefaultParameterValues.FbmMinimalValue, generator.SampleMinimum, "default sample minimum");
            Assert.AreEqual(DefaultParameterValues.FbmHurstExponent, generator.HurstExponent, "default hurst exponent");
            Assert.AreEqual(DefaultParameterValues.FbmAlgorithm, generator.Algorithm, "default algorithm");
        }
    }
}
