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
    public class MovingAverageConvergenceDivergenceHistogramTest
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
        /// Output data, Histogram.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MACD.xsl, W37…W255, 252 entries.
        /// </summary>
        private readonly List<double> expectedHist = new List<double>
        {
            // Begins with 33 double.NaN values.
                     double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,
                     double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,
                     double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,
                     double.NaN,          double.NaN,          double.NaN, 0.97171700440055400, 1.00828851139361000, 0.90303285288283500, 0.81030226965255400, 0.60845920060295500, 0.42751185300574200, 0.29178524738262200,
            0.17904583429469900, 0.25373762032681000, 0.54128850848754300, 0.72641499584872500, 0.92564738765426900, 0.99353193062339900, 1.03931838846675000, 0.86673588829654400, 0.84770597652950400, 0.76444117884788700,
            0.58163462741430100, 0.42478575979557600, 0.01576846196492720,-0.29026237186605900,-0.51695360649499300,-0.49769785130393900,-0.39787175381679800,-0.27548448107487200,-0.00338803362509665, 0.19448663100290800,
            0.27152575330967900, 0.30199348027809200, 0.53041018748902500, 0.61827299182440300, 0.75369946389239900, 0.81439431272394500, 0.78523942428854300, 0.62994583559405500, 0.38410789164743000, 0.18351317761483700,
           -0.01085463391861240,-0.37426241699290600,-0.70437869179605800,-0.78316227648677400,-0.72626113296027800, 0.06816897943590220, 0.72881084083407400, 1.42501887276798000, 1.84818506604679000, 1.78280991883191000,
            1.62788935812720000, 1.56707773150428000, 1.52274826226783000, 1.38198442906887000, 1.19713201158459000, 0.89556190273187200, 0.88177113399209600, 0.83583161329355000, 0.80183481755020700, 0.84324392687180300,
            1.43836464975826000, 1.47654987966281000, 1.31983580304319000, 1.13391894275418000, 0.82135001029379100, 0.43178601364915500, 0.02935487913218540,-0.49189540455886400,-0.92930435285032200,-0.72688178570372000,
           -0.74853937927096800,-0.77219214130543700,-1.04936619259433000,-1.10156926991797000,-1.17089084777991000,-0.99766496241549400,-0.58732449702535700,-0.58332932325886900,-0.56104016628982300,-0.66310602940804600,
           -0.78456108829349600,-0.77408224722073200,-0.73202530611137000,-0.38672348028254900,-0.20808347830769100,-0.07514749485580330, 0.24187741117530800, 0.31377639916516800, 0.28817776758974500, 0.20534550724663700,
            0.15050310119309200, 0.04426031261568490, 0.07847158359522320, 0.36275587010076600, 0.60425676016169200, 0.77299591517113600, 0.72597786534972700, 0.73981954419397800, 0.74982816828138500, 0.89496131298322000,
            0.92687786401632400, 0.86107242804843500, 0.69083145059666200, 0.44269859194311000, 0.21390269015933800,-0.09082240549327560,-0.73072933295996300,-1.09049133754159000,-1.62856702591511000,-1.85911440701631000,
           -2.05608460595854000,-1.89408016267775000,-1.58272870041454000,-1.51850641128288000,-1.39657273491460000,-1.48162998807676000,-1.65285147784303000,-1.73706264128148000,-1.40353760155069000,-1.10198995769615000,
           -0.93716570920782900,-0.96473513760096000,-0.67018360679580200,-0.58267763228054600,-0.34485254155170800, 0.08718937519084440, 0.43495886730321300, 0.34309628201571400, 0.21583978651654500, 0.05598553444021250,
            0.13156439736007000, 0.02306172952083100,-0.01585671342491150, 0.00367873023195531, 0.09009012803439990, 0.09434388506773170, 0.18578731387691700, 0.41090179353937100, 0.44732340518786800, 0.64143035204559300,
            0.93230670529140200, 0.98630356891162000, 1.22179310243677000, 1.31659302194563000, 1.13208437159662000, 1.00615522637699000, 0.77447013591347900, 0.45050334061282100,-0.08701726865689820,-0.13076535954401500,
           -0.35829826181540100,-0.61935656150353300,-0.96674144087321200,-0.95278892023390400,-1.03000525213475000,-0.99842489699898600,-1.14947436371179000,-1.12493314987814000,-1.25645365141323000,-1.13333152397890000,
           -0.85383732689313100,-0.80429688491147100,-0.90028901485986100,-1.08360501780442000,-1.07855450676250000,-1.27233434535231000,-1.62171321645676000,-1.60754178941801000,-1.43086795563989000,-1.26938379335560000,
           -1.05981281990320000,-0.84398873305653800,-1.64706005152268000,-1.83419640949831000,-1.81337733322247000,-1.55068410766337000,-1.41019229299269000,-1.06709422747016000,-0.52512993356080400,-0.19269688983946200,
           -0.03666124080177190, 0.09644234618448790, 0.05672737610860870, 0.00637220943682326, 0.27106147025255500, 0.46281239053277500, 0.83386558499134900, 0.95261116997190200, 1.08797589516673000, 1.05315007673600000,
            1.06232527084120000, 1.00491795488854000, 1.23324342536579000, 1.73373251298689000, 2.24634698678759000, 2.36825605048046000, 2.24944453914182000, 2.11231977900086000, 1.88248939215583000, 1.58188877661947000,
            1.34417160835513000, 1.25035477345039000, 1.55193400487786000, 1.92436374778940000, 2.09329182192231000, 2.18741283863958000, 1.80544465418891000, 1.17544295011886000, 0.74388085481953800, 0.38423429171169900,
           -0.02723128447320900,-0.16619381577943900,-0.21476165904585000,-0.30638280997011600,-0.30972131775976000,-0.45085074552899300,-0.49505863577155500,-0.44252518600002100,-0.39938632461565600,-0.41900830492632600,
           -0.44024605187873100,-0.50058493067088000
        };

        /// <summary>
        /// Output data, Histogram, Metastock.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MACD.xsl, X37…X255, 252 entries.
        /// </summary>
        private readonly List<double> expectedHistM = new List<double>
        {
            // Begins with 33 double.NaN values.
                     double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,
                     double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,
                     double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,          double.NaN,
                     double.NaN,          double.NaN,          double.NaN, 0.49345537746962800, 0.56197807181468900, 0.49443931884640900, 0.43580055751673900, 0.27201389557218000, 0.12525313263141200, 0.01871471009780960,
           -0.06815260505129680, 0.01950193326479080, 0.30625071570277600, 0.49630146409191600, 0.69887734454100700, 0.77727575721734500, 0.83386055435487900, 0.68412013781935000, 0.67721095812909200, 0.60897645180767600,
            0.44612925372082200, 0.30649422419364400,-0.07120385053591290,-0.35363749666679800,-0.56253819165436200,-0.54091033847592500,-0.44348222164150000,-0.32461980901494800,-0.06479595961828920, 0.12579254020964000,
            0.20298601707728100, 0.23620254353656000, 0.45520715722175000, 0.54231477291009400, 0.67393114950748900, 0.73521604414137900, 0.71173670722358600, 0.56915431675404800, 0.34065414709457100, 0.15377010627353400,
           -0.02791212313477960,-0.36905755551553900,-0.67982068694035500,-0.75496176246701500,-0.70240935020968600, 0.04402309956740790, 0.66707916169446800, 1.32518739666074000, 1.72800467554092000, 1.67191013834090000,
            1.53060116331528000, 1.47666119373748000, 1.43754523432609000, 1.30724335143167000, 1.13478734756551000, 0.85188772827617100, 0.83864238279525400, 0.79520807680719100, 0.76294342182217600, 0.80155755051446600,
            1.36095264849130000, 1.39840813636869000, 1.25247886968908000, 1.07843435448696000, 0.78468567432829700, 0.41767902584292200, 0.03764593153104560,-0.45489878792101400,-0.86960060808177400,-0.68332182402680700,
           -0.70642869289095200,-0.73095389241846000,-0.99345739659976800,-1.04482941892457000,-1.11199037081373000,-0.95090500180428900,-0.56604932854179600,-0.56188480400626400,-0.54052628535568700,-0.63604186360484300,
           -0.75009722343644200,-0.74042402225936900,-0.70096019031870500,-0.37622314450242800,-0.20720788720426200,-0.08082195670294820, 0.21879150474227300, 0.28856780563576400, 0.26651633140830800, 0.19024668867536700,
            0.13979271691437800, 0.04067326162032090, 0.07318113415678380, 0.34083565594304500, 0.56897941837909700, 0.72918426978645500, 0.68672428048942900, 0.70104028120434800, 0.71155018596709100, 0.84892118088600400,
            0.88006877986909600, 0.81920981980648800, 0.65978496319281700, 0.42652787781107500, 0.21074168697768300,-0.07696254460488560,-0.68044136410893900,-1.02198137743702000,-1.53158772789148000,-1.75288960337460000,
           -1.94245159043693000,-1.79426883877362000,-1.50447599896325000,-1.44562212217085000,-1.33202189146591000,-1.41246796683315000,-1.57402803139415000,-1.65410649874638000,-1.34146724114664000,-1.05776931484030000,
           -0.90177687264548800,-0.92635479087693100,-0.64835526026855800,-0.56436219071331300,-0.33907601158087100, 0.06918789473159090, 0.39906146152545100, 0.31605865939212900, 0.19894088423034400, 0.05037801890879700,
            0.12241096961456200, 0.02139555872087070,-0.01468786411225960, 0.00397129837058530, 0.08548899993734150, 0.08993383128195220, 0.17626745611436100, 0.38844476518924200, 0.42372357518635800, 0.60715417280202900,
            0.88191361451437400, 0.93451579242223400, 1.15755622815741000, 1.24865548400333000, 1.07703987636985000, 0.95963438758103000, 0.74224362021824200, 0.43734203669799500,-0.06920921282069560,-0.11282835436098900,
           -0.32895204561081300,-0.57685655550315700,-0.90622421468483600,-0.89634841392379100,-0.97158276561683700,-0.94427196428076200,-1.08814898373887000,-1.06700917150616000,-1.19216698678707000,-1.07798671711302000,
           -0.81614615084439200,-0.76954117848278200,-0.85959973435681100,-1.03202501982228000,-1.02781391338333000,-1.21040385988959000,-1.53971923028196000,-1.52801845361274000,-1.36318599411377000,-1.21189661740723000,
           -1.01482829763251000,-0.81127994454657600,-1.56516389790485000,-1.74233765865996000,-1.72427170641662000,-1.47845527638639000,-1.34657827171043000,-1.02376399946294000,-0.51305235550295900,-0.19802822451085300,
           -0.04834847444924680, 0.07969251700888690, 0.04511729983420930,-0.00005575900633570, 0.25044297099804600, 0.43279245022342400, 0.78386568214587600, 0.89833441697799500, 1.02823391367451000, 0.99800353432393200,
            1.00857037711889000, 0.95618255518023800, 1.17195590816032000, 1.64399946835419000, 2.12850973584171000, 2.24659900896883000, 2.13804350898872000, 2.01133559595292000, 1.79664144451381000, 1.51447192820582000,
            1.29045511802579000, 1.20110570591151000, 1.48338343141920000, 1.83326914147537000, 1.99284883274883000, 2.08238387931088000, 1.72434204196140000, 1.13182324012001000, 0.72413467331820900, 0.38317290467336300,
           -0.00703507543892901,-0.14160983067368500,-0.19090842175804600,-0.28018755955425200,-0.28616384384224800,-0.42118949191302800,-0.46505193615874000,-0.41765647685238800,-0.37857278415797500,-0.39808798499505600,
           -0.41895628025388200,-0.47645308599377500
        };
        #endregion

        #region NameTest
        /// <summary>
        /// A test for Name.
        /// </summary>
        [TestMethod]
        public void NameTest()
        {
            var target = new MovingAverageConvergenceDivergenceHistogram(new MovingAverageConvergenceDivergence());
            Assert.AreEqual("MACDh", target.Name);
        }
        #endregion

        #region MonikerTest
        /// <summary>
        /// A test for Moniker.
        /// </summary>
        [TestMethod]
        public void MonikerTest()
        {
            var target = new MovingAverageConvergenceDivergenceHistogram(new MovingAverageConvergenceDivergence());
            Assert.AreEqual("MACDh(12,26,9)", target.Moniker);
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
            var target = new MovingAverageConvergenceDivergenceHistogram(parent);
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
            var target = new MovingAverageConvergenceDivergenceHistogram(new MovingAverageConvergenceDivergence());
            Assert.AreEqual("Moving Average Convergence/Divergence histogram", target.Description);
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
            var target = new MovingAverageConvergenceDivergenceHistogram(parent);
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

        #region TaLibHistogramTest
        /// <summary>
        /// A TA-Lib histogram value test.
        /// </summary>
        [TestMethod]
        public void TaLibHistogramTest()
        {
            double u;
            const int digits = 9;
            int count = input.Count;
            var parent = new MovingAverageConvergenceDivergence();
            var target = new MovingAverageConvergenceDivergenceHistogram(parent);
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
                u = Math.Round(expectedHist[i], digits);
                Assert.AreEqual(u, d);
            }
        }
        #endregion

        #region TaLibHistogramTestMetastock
        /// <summary>
        /// A TA-Lib Metastock histogram value test.
        /// </summary>
        [TestMethod]
        public void TaLibHistogramTestMetastock()
        {
            double u;
            const int digits = 9;
            int count = input.Count;
            // The test_MACD.xls uses non-standard alphas for the Metastock data.
            var parent = new MovingAverageConvergenceDivergence(0.075, 0.15, 0.2, false);
            var target = new MovingAverageConvergenceDivergenceHistogram(parent);
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
                u = Math.Round(expectedHistM[i], digits);
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
            var target = new MovingAverageConvergenceDivergenceHistogram(parent);
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
                u = Math.Round(parent.Histogram, digits);
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
            var target = new MovingAverageConvergenceDivergenceHistogram(parent);
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
                u = Math.Round(expectedHist[i], digits);
                Assert.AreEqual(u, d);
            }
        }
        #endregion

        #region MovingAverageConvergenceDivergenceHistogramConstructorTest
        /// <summary>
        /// A test for MovingAverageConvergenceDivergenceHistogram Constructor.
        /// </summary>
        [TestMethod]
        public void MovingAverageConvergenceDivergenceHistogramConstructorTest()
        {
            var parent = new MovingAverageConvergenceDivergence();
            var target = new MovingAverageConvergenceDivergenceHistogram(parent);
            Assert.IsNotNull(target);
         }
        #endregion

        #region SerializationTest
        private static void SerializeTo(MovingAverageConvergenceDivergenceHistogram instance, string fileName)
        {
            var dcs = new DataContractSerializer(typeof(MovingAverageConvergenceDivergenceHistogram),
                null, 65536, false, true, null);
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                dcs.WriteObject(fs, instance);
                fs.Close();
            }
        }

        private static MovingAverageConvergenceDivergenceHistogram DeserializeFrom(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            var ser = new DataContractSerializer(typeof(MovingAverageConvergenceDivergenceHistogram),
                null, 65536, false, true, null);
            var instance = (MovingAverageConvergenceDivergenceHistogram)ser.ReadObject(reader, true);
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
            var source = new MovingAverageConvergenceDivergenceHistogram(parent);
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
                u = Math.Round(expectedHist[i], digits);
                Assert.AreEqual(u, d);
            }
            const string fileName = "MovingAverageConvergenceDivergenceHistogramTest_1.xml";
            SerializeTo(source, fileName);
            MovingAverageConvergenceDivergenceHistogram target = DeserializeFrom(fileName);
            Assert.IsTrue(target.IsPrimed);
            Assert.AreEqual("MACDh", target.Name);
            Assert.AreEqual(source.Value, target.Value);
            Assert.AreEqual("Moving Average Convergence/Divergence histogram", target.Description);
            parent = target.Parent;
            for (int i = 92; i < count; ++i)
            {
                parent.Update(input[i]);
                u = target.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                d = Math.Round(target.Value, digits);
                u = Math.Round(expectedHist[i], digits);
                Assert.AreEqual(u, d);
            }
            //FileInfo fi = new FileInfo(fileName);
            //fi.Delete();
        }
        #endregion
    }
}
