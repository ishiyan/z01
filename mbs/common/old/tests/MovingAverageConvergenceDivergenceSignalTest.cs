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
    public class MovingAverageConvergenceDivergenceSignalTest
    {
        #region Test data
        /// <summary>
        /// Input Close test data, length = 252.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_data.c, TA_SREF_close_daily_ref_0_PRIV[252].
        /// The same data in TA-Lib (http://ta-lib.org/) tests, test_MACD.xsl, INPUT, A4…A255, 252 entries.
        /// </summary>
        private readonly List<double> input = new List<double>
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
        /// Output data, Signal.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MACD.xsl, U37…U255, 252 entries.
        /// </summary>
        private readonly List<double> expectedSig = new List<double>
        {
            // Begins with 33 double.NaN values.
                     double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,
                     double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,
                     double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,
                     double.NaN,         double.NaN,         double.NaN,-3.1552099196892100,-2.9031377918408000,-2.6773795786201000,-2.4748040112069600,-2.3226892110562200,-2.2158112478047800,-2.1428649359591300,
            -2.0981034773854500,-2.0346690723037500,-1.8993469451818600,-1.7177431962196800,-1.4863313493061200,-1.2379483666502700,-0.9781187695335780,-0.7614347974594420,-0.5495083033270660,-0.3583980086150950,
            -0.2129893517615200,-0.1067929118126260,-0.1028507963213940,-0.1754163892879080,-0.3046547909116570,-0.4290792537376410,-0.5285471921918410,-0.5974183124605590,-0.5982653208668330,-0.5496436631161060,
            -0.4817622247886860,-0.4062638547191630,-0.2736613078469070,-0.1190930598908060, 0.0693318060822932, 0.2729303842632790, 0.4692402403354150, 0.6267266992339290, 0.7227536721457870, 0.7686319665494960,
             0.7659183080698430, 0.6723527038216170, 0.4962580308726020, 0.3004674617509090, 0.1189021785108390, 0.1359444233698150, 0.3181471335783330, 0.6744018517703280, 1.1364481182820300, 1.5821505979900000,
             1.9891229375218000, 2.3808923703978700, 2.7615794359648300, 3.1070755432320500, 3.4063585461282000, 3.6302490218111700, 3.8506918053091900, 4.0596497086325800, 4.2601084130201300, 4.4709193947380800,
             4.8305105571776400, 5.1996480270933500, 5.5296069778541500, 5.8130867135426900, 6.0184242161161400, 6.1263707195284300, 6.1337094393114700, 6.0107355881717600, 5.7784094999591800, 5.5966890535332500,
             5.4095542087155100, 5.2165061733891500, 4.9541646252405600, 4.6787723077610700, 4.3860495958160900, 4.1366333552122200, 3.9898022309558800, 3.8439699001411600, 3.7037098585687100, 3.5379333512167000,
             3.3417930791433200, 3.1482725173381400, 2.9652661908103000, 2.8685853207396600, 2.8165644511627400, 2.7977775774487900, 2.8582469302426200, 2.9366910300339100, 3.0087354719313400, 3.0600718487430000,
             3.0976976240412800, 3.1087627021952000, 3.1283805980940100, 3.2190695656192000, 3.3701337556596200, 3.5633827344524000, 3.7448772007898400, 3.9298320868383300, 4.1172891289086800, 4.3410294571544800,
             4.5727489231585700, 4.7880170301706700, 4.9607248928198400, 5.0713995408056200, 5.1248752133454500, 5.1021696119721400, 4.9194872787321400, 4.6468644443467500, 4.2397226878679700, 3.7749440861138900,
             3.2609229346242600, 2.7874028939548200, 2.3917207188511800, 2.0120941160304600, 1.6629509323018100, 1.2925434352826200, 0.8793305658218650, 0.4450649055014950, 0.0941805051138218,-0.1813169843102150,
            -0.4156084116121720,-0.6567921960124120,-0.8243380977113630,-0.9700075057814990,-1.0562206411694300,-1.0344232973717200,-0.9256835805459120,-0.8399095100419840,-0.7859495634128470,-0.7719531798027950,
            -0.7390620804627770,-0.7332966480825690,-0.7372608264387970,-0.7363411438808090,-0.7138186118722090,-0.6902326406052760,-0.6437858121360470,-0.5410603637512040,-0.4292295124542370,-0.2688719244428390,
            -0.0357952481199881, 0.2107806441079170, 0.5162289197171100, 0.8453771752035180, 1.1283982681026700, 1.3799370746969200, 1.5735546086752900, 1.6861804438285000, 1.6644261266642700, 1.6317347867782700,
             1.5421602213244200, 1.3873210809485300, 1.1456357207302300, 0.9074384906717550, 0.6499371776380690, 0.4003309533883220, 0.1129623624603760,-0.1682709250091590,-0.4823843378624660,-0.7657172188571900,
            -0.9791765505804730,-1.1802507718083400,-1.4053230255233100,-1.6762242799744100,-1.9458629066650400,-2.2639464930031200,-2.6693747971173100,-3.0712602444718100,-3.4289772333817800,-3.7463231817206800,
            -4.0112763866964800,-4.2222735699606200,-4.6340385828412900,-5.0925876852158700,-5.5459320185214900,-5.9336030454373300,-6.2861511186855000,-6.5529246755530400,-6.6842071589432400,-6.7323813814031100,
            -6.7415466916035500,-6.7174361050574300,-6.7032542610302800,-6.7016612086710700,-6.6338958411079400,-6.5181927434747400,-6.3097263472269000,-6.0715735547339300,-5.7995795809422500,-5.5362920617582500,
            -5.2707107440479500,-5.0194812553258100,-4.7111703989843700,-4.2777372707376400,-3.7161505240407500,-3.1240865114206300,-2.5617253766351800,-2.0336454318849600,-1.5630230838460100,-1.1675508896911400,
            -0.8315079876023550,-0.5189192942397580,-0.1309357930202930, 0.3501551439270560, 0.8734780994076330, 1.4203313090675300, 1.8716924726147600, 2.1655532101444700, 2.3515234238493600, 2.4475819967772800,
             2.4407741756589800, 2.3992257217141200, 2.3455353069526600, 2.2689396044601300, 2.1915092750201900, 2.0787965886379400, 1.9550319296950500, 1.8444006331950500, 1.7445540520411300, 1.6398019758095500,
             1.5297404628398700, 1.4045942301721500
        };

        /// <summary>
        /// Output data, Signal, Metastock.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MACD.xsl, Metastock, S37…S255, 252 entries.
        /// </summary>
        private readonly List<double> expectedSigM = new List<double>
        {
            // Begins with 33 double.NaN values.
                     double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,
                     double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,
                     double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,         double.NaN,
                     double.NaN,         double.NaN,         double.NaN,-1.7119209074082100,-1.5714263894545400,-1.4478165597429400,-1.3388664203637500,-1.2708629464707100,-1.2395496633128600,-1.2348709857884000,
            -1.2519091370512300,-1.2470336537350300,-1.1704709748093400,-1.0463956087863600,-0.8716762726511050,-0.6773573333467690,-0.4688921947580490,-0.2978621603032120,-0.1285594207709390, 0.0236846921809804,
             0.1352170056111860, 0.2118405616595970, 0.1940395990256190, 0.1056302248589190,-0.0350043230546712,-0.1702319076736530,-0.2811024630840280,-0.3622574153377650,-0.3784564052423370,-0.3470082701899270,
            -0.2962617659206070,-0.2372111300364670,-0.1234093407310290, 0.0121693524964942, 0.1806521398733660, 0.3644561509087110, 0.5423903277146080, 0.6846789069031200, 0.7698424436767620, 0.8082849702451460,
             0.8013069394614510, 0.7090425505825670, 0.5390873788474780, 0.3503469382307240, 0.1747446006783030, 0.1857503755701550, 0.3525201659937720, 0.6838170151589580, 1.1158181840441900, 1.5337957186294100,
             1.9164460094582300, 2.2856113078926000, 2.6449976164741200, 2.9718084543320400, 3.2555052912234200, 3.4684772232924600, 3.6781378189912800, 3.8769398381930800, 4.0676756936486200, 4.2680650812772400,
             4.6083032434000600, 4.9579052774922400, 5.2710249949145100, 5.5406335835362500, 5.7368050021183200, 5.8412247585790500, 5.8506362414618100, 5.7369115444815600, 5.5195113924611200, 5.3486809364544200,
             5.1720737632316800, 4.9893352901270600, 4.7409709409771200, 4.4797635862459800, 4.2017659935425500, 3.9640397430914800, 3.8225274109560300, 3.6820562099544600, 3.5469246386155400, 3.3879141727143300,
             3.2003898668552200, 3.0152838612903800, 2.8400438137107000, 2.7459880275850900, 2.6941860557840300, 2.6739805666082900, 2.7286784427938600, 2.8008203942028000, 2.8674494770548800, 2.9150111492237200,
             2.9499593284523200, 2.9601276438574000, 2.9784229273965900, 3.0636318413823500, 3.2058766959771300, 3.3881727634237400, 3.5598538335461000, 3.7351139038471900, 3.9130014503389600, 4.1252317455604600,
             4.3452489405277300, 4.5500513954793600, 4.7149976362775600, 4.8216296057303300, 4.8743150274747500, 4.8550743913235300, 4.6849640502963000, 4.4294687059370400, 4.0465717739641700, 3.6083493731205200,
             3.1227364755112900, 2.6741692658178800, 2.2980502660770700, 1.9366447355343600, 1.6036392626678800, 1.2505222709596000, 0.8570152631110560, 0.4434886384244620, 0.1081218281378030,-0.1563205005722740,
            -0.3817647187336460,-0.6133534164528780,-0.7754422315200180,-0.9165327791983460,-1.0013017820935600,-0.9840048084106670,-0.8842394430293040,-0.8052247781812720,-0.7554895571236860,-0.7428950523964870,
            -0.7122923099928460,-0.7069434203126280,-0.7106153863406930,-0.7096225617480470,-0.6882503117637120,-0.6657668539432240,-0.6216999899146330,-0.5245887986173230,-0.4186579048207340,-0.2668693616202260,
            -0.0463909579916328, 0.1872379901139260, 0.4766270471532790, 0.7887909181541120, 1.0580508872465700, 1.2979594841418300, 1.4835203891963900, 1.5928558983708900, 1.5755535951657200, 1.5473465065754700,
             1.4651084951727700, 1.3208943562969800, 1.0943383026257700, 0.8702511991448200, 0.6273555077406100, 0.3912875166704200, 0.1192502707357020,-0.1475020221408380,-0.4455437688376070,-0.7150404481158610,
            -0.9190769858269590,-1.1114622804476500,-1.3263622140368600,-1.5843684689924300,-1.8413219473382600,-2.1439229123106600,-2.5288527198811500,-2.9108573332843300,-3.2516538318127800,-3.5546279861645800,
            -3.8083350605727100,-4.0111550467093600,-4.4024460211855700,-4.8380304358505600,-5.2690983624547100,-5.6387121815513100,-5.9753567494789200,-6.2312977493446500,-6.3595608382203900,-6.4090678943481100,
            -6.4211550129604200,-6.4012318837082000,-6.3899525587496500,-6.3899664985012300,-6.3273557557517200,-6.2191576431958600,-6.0231912226594000,-5.7986076184149000,-5.5415491399962700,-5.2920482564152900,
            -5.0399056621355700,-4.8008600233405100,-4.5078710463004300,-4.0968711792118800,-3.5647437452514500,-3.0030939930092500,-2.4685831157620700,-1.9657492167738400,-1.5165888556453800,-1.1379708735939300,
            -0.8153570940874790,-0.5150806676096020,-0.1442348097548030, 0.3140824756140400, 0.8122946838012470, 1.3328906536289700, 1.7639761641193200, 2.0469319741493200, 2.2279656424788700, 2.3237588686472100,
             2.3220000997874800, 2.2865976421190600, 2.2388705366795500, 2.1688236467909900, 2.0972826858304200, 1.9919853128521700, 1.8757223288124800, 1.7713082095993900, 1.6766650135598900, 1.5771430173111300,
             1.4724039472476600, 1.3532906757492100
        };
        #endregion

        #region NameTest
        /// <summary>
        /// A test for Name.
        /// </summary>
        [TestMethod]
        public void NameTest()
        {
            var target = new MovingAverageConvergenceDivergenceSignal(new MovingAverageConvergenceDivergence());
            Assert.AreEqual("MACDs", target.Name);
        }
        #endregion

        #region MonikerTest
        /// <summary>
        /// A test for Moniker.
        /// </summary>
        [TestMethod]
        public void MonikerTest()
        {
            var target = new MovingAverageConvergenceDivergenceSignal(new MovingAverageConvergenceDivergence());
            Assert.AreEqual("MACDs(12,26,9)", target.Moniker);
        }
        #endregion

        #region ParentTest
        /// <summary>
        /// A test for Parent.
        /// </summary>
        [TestMethod]
        public void ParentTest()
        {
            var parent = new MovingAverageConvergenceDivergence();
            var target = new MovingAverageConvergenceDivergenceSignal(parent);
            Assert.AreEqual(parent, target.Parent);
        }
        #endregion

        #region DescriptionTest
        /// <summary>
        /// A test for Description.
        /// </summary>
        [TestMethod]
        public void DescriptionTest()
        {
            var target = new MovingAverageConvergenceDivergenceSignal(new MovingAverageConvergenceDivergence());
            Assert.AreEqual("Moving Average Convergence/Divergence signal", target.Description);
        }
        #endregion

        #region IsPrimedTest
        /// <summary>
        /// A test for IsPrimed.
        /// </summary>
        [TestMethod]
        public void IsPrimedTest()
        {
            var parent = new MovingAverageConvergenceDivergence();
            var target = new MovingAverageConvergenceDivergenceSignal(parent);
            Assert.IsFalse(target.IsPrimed);
            var scalar = new Scalar(DateTime.Now, 1d);
            for (int i = 1; i <= 33; ++i)
            {
                scalar.Value = i;
                parent.Update(scalar);
                target.Update(scalar);
                Assert.IsFalse(target.IsPrimed);
            }
            scalar.Value = 34.0;
            parent.Update(scalar);
            target.Update(scalar);
            Assert.IsTrue(target.IsPrimed);
            scalar.Value = 35.0;
            parent.Update(scalar);
            target.Update(scalar);
            Assert.IsTrue(target.IsPrimed);
        }
        #endregion

        #region TaLibSignalTest
        /// <summary>
        /// A TA-Lib signal value test.
        /// </summary>
        [TestMethod]
        public void TaLibSignalTest()
        {
            double u;
            const int digits = 9;
            int count = input.Count;
            var parent = new MovingAverageConvergenceDivergence();
            var target = new MovingAverageConvergenceDivergenceSignal(parent);
            for (int i = 0; i < 33; ++i)
            {
                parent.Update(input[i]);
                target.Update(input[i]);
                u = target.Value;
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(u));
            }
            for (int i = 33; i < count; ++i)
            {
                parent.Update(input[i]);
                target.Update(input[i]);
                u = target.Value;
                Assert.IsFalse(double.IsNaN(u));
                double d = Math.Round(u, digits);
                u = Math.Round(expectedSig[i], digits);
                Assert.AreEqual(u, d);
            }
        }
        #endregion

        #region TaLibSignalTestMetastock
        /// <summary>
        /// A TA-Lib Metastock signal value test.
        /// </summary>
        [TestMethod]
        public void TaLibSignalTestMetastock()
        {
            double u;
            const int digits = 9;
            int count = input.Count;
            // The test_MACD.xls uses non-standard alphas for the Metastock data.
            var parent = new MovingAverageConvergenceDivergence(0.075, 0.15, 0.2, false);
            var target = new MovingAverageConvergenceDivergenceSignal(parent);
            for (int i = 0; i < 33; ++i)
            {
                parent.Update(input[i]);
                target.Update(input[i]);
                u = target.Value;
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(u));
            }
            for (int i = 33; i < count; ++i)
            {
                parent.Update(input[i]);
                target.Update(input[i]);
                u = target.Value;
                Assert.IsFalse(double.IsNaN(u));
                double d = Math.Round(u, digits);
                u = Math.Round(expectedSigM[i], digits);
                Assert.AreEqual(u, d);
            }
        }
        #endregion

        #region ValueTest
        /// <summary>
        /// A test for Value.
        /// </summary>
        [TestMethod]
        public void ValueTest()
        {
            const int digits = 9;
            int count = input.Count;
            var parent = new MovingAverageConvergenceDivergence();
            var target = new MovingAverageConvergenceDivergenceSignal(parent);
            for (int i = 0; i < 33; ++i)
            {
                parent.Update(input[i]);
                target.Update(input[i]);
                Assert.IsFalse(target.IsPrimed);
            }
            for (int i = 33; i < count; ++i)
            {
                parent.Update(input[i]);
                double u = target.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                double d = Math.Round(target.Value, digits);
                u = Math.Round(parent.Signal, digits);
                Assert.AreEqual(u, d);
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
            const int digits = 9;
            int count = input.Count;
            var parent = new MovingAverageConvergenceDivergence();
            var target = new MovingAverageConvergenceDivergenceSignal(parent);
            var scalar = new Scalar(DateTime.Now, 1d);
            for (int i = 0; i < 33; ++i)
            {
                scalar.Value = input[i];
                parent.Update(scalar);
                scalar.Value = target.Update(scalar).Value;
                Assert.IsFalse(target.IsPrimed);
            }
            for (int i = 33; i < count; ++i)
            {
                scalar.Value = input[i];
                parent.Update(scalar);
                double u = target.Update(scalar).Value;
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                double d = Math.Round(target.Value, digits);
                u = Math.Round(expectedSig[i], digits);
                Assert.AreEqual(u, d);
            }
        }
        #endregion

        #region MovingAverageConvergenceDivergenceSignalConstructorTest
        /// <summary>
        /// A test for MovingAverageConvergenceDivergenceSignal Constructor.
        /// </summary>
        [TestMethod]
        public void MovingAverageConvergenceDivergenceSignalConstructorTest()
        {
            var parent = new MovingAverageConvergenceDivergence();
            var target = new MovingAverageConvergenceDivergenceSignal(parent);
            Assert.IsNotNull(target);
         }
        #endregion

        #region SerializationTest
        private static void SerializeTo(MovingAverageConvergenceDivergenceSignal instance, string fileName)
        {
            var dcs = new DataContractSerializer(typeof(MovingAverageConvergenceDivergenceSignal),
                null, 65536, false, true, null);
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                dcs.WriteObject(fs, instance);
                fs.Close();
            }
        }

        private static MovingAverageConvergenceDivergenceSignal DeserializeFrom(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            var ser = new DataContractSerializer(typeof(MovingAverageConvergenceDivergenceSignal),
                null, 65536, false, true, null);
            var instance = (MovingAverageConvergenceDivergenceSignal)ser.ReadObject(reader, true);
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
            int count = input.Count;
            var parent = new MovingAverageConvergenceDivergence();
            var source = new MovingAverageConvergenceDivergenceSignal(parent);
            for (int i = 0; i < 33; ++i)
            {
                parent.Update(input[i]);
                source.Update(input[i]);
                Assert.IsFalse(source.IsPrimed);
            }
            for (int i = 33; i < 92; ++i)
            {
                parent.Update(input[i]);
                u = source.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, source.Value);
                d = Math.Round(source.Value, digits);
                u = Math.Round(expectedSig[i], digits);
                Assert.AreEqual(u, d);
            }
            const string fileName = "MovingAverageConvergenceDivergenceSignalTest_1.xml";
            SerializeTo(source, fileName);
            MovingAverageConvergenceDivergenceSignal target = DeserializeFrom(fileName);
            Assert.IsTrue(target.IsPrimed);
            Assert.AreEqual("MACDs", target.Name);
            Assert.AreEqual(source.Value, target.Value);
            Assert.AreEqual("Moving Average Convergence/Divergence signal", target.Description);
            parent = target.Parent;
            for (int i = 92; i < count; ++i)
            {
                parent.Update(input[i]);
                u = target.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                d = Math.Round(target.Value, digits);
                u = Math.Round(expectedSig[i], digits);
                Assert.AreEqual(u, d);
            }
            //FileInfo fi = new FileInfo(fileName);
            //fi.Delete();
        }
        #endregion
    }
}
