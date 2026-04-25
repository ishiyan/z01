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
    public class CommodityChannelIndexTest
    {
        #region Test data
        /// <summary>
        /// Input test data, length = 252.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_CCI.xsl, TYPPRICE, F4…F255, 252 entries.
        /// </summary>
        private readonly List<double> input = new List<double>
        {
            91.83333333333330, 93.72000000000000, 95.00000000000000, 94.92833333333330, 94.19833333333330, 94.28166666666670, 93.17666666666670, 92.07333333333330, 90.74166666666670, 91.94833333333330,
            95.04166666666670, 97.73000000000000, 97.88500000000000, 90.48000000000000, 89.68833333333330, 92.33500000000000, 90.48833333333330, 89.59333333333330, 90.94833333333330, 90.62500000000000,
            88.74000000000000, 87.55166666666670, 85.88500000000000, 83.59333333333330, 83.16833333333330, 82.33333333333330, 83.37666666666670, 87.57333333333330, 86.69833333333330, 87.11500000000000,
            85.60333333333330, 86.49000000000000, 86.23000000000000, 87.82333333333330, 88.78166666666670, 87.76166666666670, 86.14666666666670, 84.68833333333330, 83.83500000000000, 84.26166666666670,
            83.45833333333330, 86.35500000000000, 88.51166666666670, 89.32333333333330, 90.93833333333330, 90.77166666666670, 91.72000000000000, 89.90666666666670, 90.24000000000000, 90.78166666666670,
            89.34333333333330, 88.05333333333330, 85.76000000000000, 84.02166666666670, 82.83500000000000, 84.41666666666670, 85.67666666666670, 86.47000000000000, 88.50166666666670, 89.44833333333330,
            89.20833333333330, 88.23000000000000, 91.07333333333330, 91.99000000000000, 92.19833333333330, 92.89500000000000, 93.06166666666670, 91.35500000000000, 90.65666666666670, 90.14833333333330,
            88.45833333333330, 86.33500000000000, 83.85333333333330, 83.75000000000000, 84.81500000000000, 97.65666666666670, 99.87500000000000,103.82333333333300,105.95833333333300,103.16666666666700,
           102.87500000000000,103.93833333333300,105.13500000000000,106.54333333333300,105.32333333333300,105.20833333333300,107.63500000000000,109.59500000000000,110.06333333333300,111.57333333333300,
           121.00000000000000,119.79166666666700,118.18833333333300,119.35500000000000,117.94833333333300,116.96000000000000,115.49166666666700,112.69833333333300,111.36500000000000,115.72000000000000,
           115.16333333333300,115.64666666666700,112.35333333333300,112.60333333333300,113.27000000000000,114.81333333333300,119.89666666666700,117.51666666666700,118.14333333333300,115.10333333333300,
           114.45666666666700,115.16666666666700,116.31000000000000,120.35333333333300,120.39666666666700,120.64666666666700,124.37333333333300,124.70666666666700,122.96000000000000,122.85333333333300,
           123.99666666666700,123.14666666666700,124.22666666666700,128.54000000000000,130.10333333333300,131.33333333333300,131.89666666666700,132.31333333333300,133.87666666666700,136.23333333333300,
           137.29333333333300,137.60666666666700,137.31333333333300,136.31333333333300,136.37666666666700,135.73333333333300,128.81333333333300,128.54000000000000,125.29000000000000,124.29000000000000,
           123.62333333333300,125.43666666666700,127.62666666666700,125.53666666666700,125.58333333333300,123.35333333333300,120.22666666666700,119.47666666666700,121.58333333333300,123.83333333333300,
           122.58333333333300,120.25000000000000,122.29000000000000,121.97666666666700,123.29000000000000,126.58000000000000,127.87333333333300,125.66666666666700,123.02000000000000,122.39333333333300,
           123.87333333333300,122.20666666666700,122.43333333333300,123.62333333333300,123.98000000000000,123.83333333333300,124.47666666666700,127.08333333333300,125.62333333333300,128.87000000000000,
           131.02333333333300,131.79333333333300,134.29333333333300,135.69000000000000,133.31333333333300,132.93666666666700,132.96000000000000,130.64666666666700,126.85333333333300,129.00333333333300,
           127.66666666666700,125.60333333333300,123.75000000000000,123.48000000000000,123.72666666666700,123.31333333333300,120.95666666666700,120.95666666666700,118.10333333333300,118.87333333333300,
           121.43666666666700,120.33333333333300,116.87333333333300,113.20666666666700,114.68666666666700,111.62333333333300,106.97666666666700,106.31333333333300,106.87000000000000,106.89666666666700,
           107.02000000000000,109.02000000000000, 91.00000000000000, 93.68666666666670, 93.70333333333330, 95.37333333333330, 93.79000000000000, 94.83333333333330, 97.83333333333330, 97.31000000000000,
            95.10333333333330, 94.60333333333330, 92.00000000000000, 91.12666666666670, 92.79333333333330, 93.74666666666670, 96.06000000000000, 95.79000000000000, 95.04000000000000, 94.76666666666670,
            94.20666666666670, 93.74666666666670, 96.60333333333330,102.47666666666700,106.91666666666700,107.31000000000000,103.77000000000000,105.04000000000000,104.16666666666700,103.22666666666700,
           103.37000000000000,104.98333333333300,110.89333333333300,115.00000000000000,117.08333333333300,118.26000000000000,115.91333333333300,109.50000000000000,109.67000000000000,108.77000000000000,
           106.48000000000000,108.21000000000000,109.89333333333300,109.13000000000000,109.43333333333300,108.77000000000000,109.08333333333300,109.29000000000000,109.87333333333300,109.41666666666700,
           109.27000000000000,107.99666666666700
        };

        /// <summary>
        /// Output data, CCI, length 11.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_CCI.xsl, CCI(11), I4…I255, 252 entries.
        /// </summary>
        private readonly List<double> expected11 = new List<double>
        {
            // Begins with 10 double.NaN values.
                       double.NaN,            double.NaN,            double.NaN,            double.NaN,            double.NaN,            double.NaN,            double.NaN,            double.NaN,            double.NaN,            double.NaN,
             87.92686612269590000, 180.00543014506300000, 143.51909625299600000,-113.86697828415700000,-111.06449704386800000, -26.77393308817710000, -70.77933765298790000, -83.15662884298380000, -41.14421072579150000, -49.63059588626750000,
            -86.45142995179210000,-105.62757994334900000,-157.69826899398900000,-190.52514361636000000,-142.83642984792700000,-122.44480555866900000, -79.95100040832990000,  22.03829204162510000,   7.76557506513423000,  32.38905944739060000,
             -0.00558772731890175,  43.84607294486250000,  40.35152301318800000,  92.89237534914090000, 113.47786811201500000,  66.34495119466630000, -35.58485536222500000,-155.46808768317800000,-165.22704112475700000,-108.29973950709100000,
           -116.51714591658300000,  17.64389315482070000, 101.64736672283300000, 105.86229468268100000, 132.33469865418400000, 111.05242915541600000, 109.44171523306700000,  54.27446220508200000,  53.34560378234870000,  59.93022007514800000,
              5.10476810062002000, -88.06663130625930000,-203.00626304801600000,-195.13396097352000000,-154.33625698530800000, -87.77348777348760000, -45.42195658287500000, -16.12518801342170000,  47.09277556571440000,  80.19578377185170000,
             83.20934786388110000,  57.22343258290950000, 127.39645267303800000, 120.86633056174900000, 110.22436152137400000, 113.95252633059400000,  98.78376467260620000,  34.37465811328310000,  -5.90669386812285000, -41.26294445651440000,
           -116.37160409189400000,-170.07275515326000000,-194.38967344642800000,-134.91742859167900000, -86.77028107473450000, 155.16963259584500000, 159.05201666023500000, 163.52305119331800000, 131.41041954964500000,  82.40398439104530000,
             66.31126911461760000,  64.27160741782140000,  66.73342325036430000,  74.95000068648950000,  57.50623992224960000,  54.94543037927350000, 130.49672213817500000, 188.30055098962700000, 150.11760803353400000, 143.00406767125900000,
            237.94418271901800000, 155.72275883299300000,  99.16458234750680000,  88.82413250696210000,  57.37684263575670000,  35.53057918185510000,   4.23723385761249000, -57.67600334891190000, -91.02813127930360000, -15.92481192889010000,
            -43.11797752809070000, -18.52182983162060000,-110.01519112723900000, -80.15072859744930000, -43.80428575722000000,  26.41827526061330000, 199.62042526522200000,  96.63269745184580000,  97.82935942582180000, -14.29327238482230000,
            -33.95630892138310000,  -7.29758541103879000,  32.00694444444400000, 134.32956665610600000, 107.96111447200400000,  96.93086089993870000, 160.15428339423500000, 132.04987642391200000,  76.23963954584590000,  63.10488285526630000,
             77.93086188018160000,  47.23681247230250000,  68.19241073066870000, 204.99628828712400000, 196.39916004747600000, 156.22420115243700000, 122.30021151642000000, 100.93661612649000000, 104.17527342560700000, 121.18100952826600000,
            120.73602666818200000, 110.83133133133200000,  90.28929031405870000,  58.02244104108510000,  48.09296202380180000,  20.44411533468580000,-176.04461049639900000,-149.82822094270000000,-164.77844914400700000,-126.99520418080700000,
           -104.70786044556500000, -68.19538745643600000, -32.68224760704600000, -56.05325923353010000, -47.41084693295380000, -88.79419605786420000,-191.33613750843800000,-161.54423995278200000, -75.67862234771170000,   5.22936144028251000,
            -33.34881111304540000, -90.85496606877090000, -21.22937182427280000, -18.84278644842040000,  51.51406391237290000, 193.75664552607700000, 176.77438268978100000,  80.50165769878760000, -19.49654389765850000, -47.14085136256300000,
              9.83705683915765000, -52.27248959444560000, -55.78838221553690000, -12.20768925881830000,  -4.91714836223522000, -14.32910464370390000,  31.41122420818590000, 205.83364231862600000, 112.97803308334700000, 193.87335722819700000,
            180.47665270256400000, 142.50174982501700000, 148.10971753499300000, 132.98372407309500000,  74.94184114323690000,  56.65930831493750000,  48.37761819269940000, -17.94848290402370000,-121.40370790768100000, -82.12962475197070000,
           -114.69982391441600000,-130.36334723113100000,-129.76351611589800000,-110.30223749257900000, -90.75197770586130000, -85.90273039585350000,-117.89795169406700000,-105.62095370937900000,-160.61136098617300000,-113.91493262311600000,
            -32.58598955519480000, -53.34176681425340000,-148.90360516111300000,-189.00018843037500000,-114.41899741897700000,-145.76335203562700000,-175.08110300081200000,-137.73487192980200000,-103.81322158826300000, -84.81013135045820000,
            -69.38902647832200000, -30.84797908980770000,-260.14419496690500000,-165.45453212193200000,-112.62660718971400000, -72.30342025433180000, -70.58651431543540000, -50.12846272933890000, -13.81854436689940000, -11.86042106692690000,
            -33.27695522414750000, -32.35294117647040000,-110.71406575268300000,-151.55591994931600000, -68.62510694654420000, -28.31153263053950000,  65.34208825230470000,  46.24524554848720000,  14.77952852082660000,  17.24631019866210000,
              5.09441267572072000, -13.60796684604480000, 119.85801161970000000, 258.34131800276700000, 232.70945899944900000, 149.91505088189400000,  72.13683194075340000,  72.49726185488220000,  50.70147574614530000,  30.86476337510630000,
             25.61980309746160000,  48.97705979035980000, 182.49451421635000000, 207.73437298952600000, 171.12257409008500000, 131.18223638825300000,  79.06502670652220000,  -3.45712043378994000,  -7.24473476308840000, -27.91808016082430000,
            -71.59269814510050000, -54.98058444918360000, -36.37642306085670000, -46.39398357762240000, -34.61217069232220000, -43.54962348378950000, -23.91690523379530000,  37.82234957020310000,  91.96545479871730000,  49.36683532001210000,
             29.72200571577000000,-169.65514382823800000
        };

        /// <summary>
        /// Output data, CCI, length 2.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_CCI.xsl, CCI(2), L4…L255, 252 entries.
        /// </summary>
        private readonly List<double> expected2 = new List<double>
        {
                    double.NaN, 66.66666666666670, 66.66666666666740,-66.66666666667990,-66.66666666666670, 66.66666666666670,-66.66666666666670,-66.66666666666670,-66.66666666666600, 66.66666666666750,
             66.66666666666670, 66.66666666666670, 66.66666666666060,-66.66666666666680,-66.66666666666790, 66.66666666666700,-66.66666666666620,-66.66666666666560, 66.66666666666670,-66.66666666666370,
            -66.66666666666670,-66.66666666666750,-66.66666666666720,-66.66666666666670,-66.66666666666440,-66.66666666666670, 66.66666666666760, 66.66666666666690,-66.66666666666670, 66.66666666666890,
            -66.66666666666730, 66.66666666666560,-66.66666666667030, 66.66666666666670, 66.66666666666670,-66.66666666666760,-66.66666666666610,-66.66666666666600,-66.66666666666560, 66.66666666666670,
            -66.66666666666670, 66.66666666666670, 66.66666666666620, 66.66666666666780, 66.66666666666730,-66.66666666666100, 66.66666666666670,-66.66666666666670, 66.66666666666380, 66.66666666666490,
            -66.66666666666670,-66.66666666666670,-66.66666666666710,-66.66666666666610,-66.66666666666670, 66.66666666666670, 66.66666666666670, 66.66666666666670, 66.66666666666710, 66.66666666666770,
            -66.66666666666670,-66.66666666666670, 66.66666666666670, 66.66666666666770, 66.66666666667120, 66.66666666666670, 66.66666666667230,-66.66666666666720,-66.66666666666670,-66.66666666666670,
            -66.66666666666670,-66.66666666666670,-66.66666666666670,-66.66666666667580, 66.66666666666670, 66.66666666666660, 66.66666666666620, 66.66666666666690, 66.66666666666670,-66.66666666666670,
            -66.66666666666990, 66.66666666666670, 66.66666666666670, 66.66666666666600,-66.66666666666670,-66.66666666666670, 66.66666666666710, 66.66666666666720, 66.66666666666870, 66.66666666666730,
             66.66666666666680,-66.66666666666750,-66.66666666666730, 66.66666666666590,-66.66666666666670,-66.66666666666670,-66.66666666666600,-66.66666666666630,-66.66666666666600, 66.66666666666690,
            -66.66666666666670, 66.66666666666670,-66.66666666666670, 66.66666666666670, 66.66666666666670, 66.66666666666730, 66.66666666666690,-66.66666666666670, 66.66666666666670,-66.66666666666670,
            -66.66666666666670, 66.66666666666670, 66.66666666666580, 66.66666666666640, 66.66666666666670, 66.66666666666670, 66.66666666666690, 66.66666666666950,-66.66666666666670,-66.66666666666670,
             66.66666666666580,-66.66666666666550, 66.66666666666670, 66.66666666666690, 66.66666666666670, 66.66666666666670, 66.66666666666670, 66.66666666666210, 66.66666666666670, 66.66666666666590,
             66.66666666666670, 66.66666666666670,-66.66666666666020,-66.66666666666670, 66.66666666666670,-66.66666666666960,-66.66666666666640,-66.66666666667360,-66.66666666666640,-66.66666666666670,
            -66.66666666666810, 66.66666666666670, 66.66666666666670,-66.66666666666710, 66.66666666666670,-66.66666666666670,-66.66666666666670,-66.66666666666670, 66.66666666666670, 66.66666666666670,
            -66.66666666666670,-66.66666666666630, 66.66666666666620,-66.66666666666670, 66.66666666666670, 66.66666666666670, 66.66666666666740,-66.66666666666710,-66.66666666666670,-66.66666666666670,
             66.66666666666730,-66.66666666666610, 66.66666666667080, 66.66666666666670, 66.66666666666400,-66.66666666666670, 66.66666666666670, 66.66666666666670,-66.66666666666600, 66.66666666666670,
             66.66666666666760, 66.66666666666670, 66.66666666666670, 66.66666666666530,-66.66666666666670,-66.66666666666670, 66.66666666674790,-66.66666666666670,-66.66666666666640, 66.66666666666620,
            -66.66666666666740,-66.66666666666670,-66.66666666666720,-66.66666666667020, 66.66666666666670,-66.66666666666900,-66.66666666666630, 66.66666666666670,-66.66666666666670, 66.66666666666540,
             66.66666666666670,-66.66666666666580,-66.66666666666640,-66.66666666666640, 66.66666666666730,-66.66666666666670,-66.66666666666690,-66.66666666666810, 66.66666666666670, 66.66666666670220,
             66.66666666666670, 66.66666666666670,-66.66666666666660, 66.66666666666670, 66.66666666672350, 66.66666666666720,-66.66666666666730, 66.66666666666670, 66.66666666666670,-66.66666666666490,
            -66.66666666666710,-66.66666666666670,-66.66666666666700,-66.66666666666670, 66.66666666666610, 66.66666666666570, 66.66666666666670,-66.66666666667020,-66.66666666666670,-66.66666666666670,
            -66.66666666666670,-66.66666666666460, 66.66666666666630, 66.66666666666670, 66.66666666666670, 66.66666666666430,-66.66666666666640, 66.66666666666670,-66.66666666666670,-66.66666666666670,
             66.66666666666000, 66.66666666666610, 66.66666666666670, 66.66666666666690, 66.66666666666710, 66.66666666666750,-66.66666666666710,-66.66666666666680, 66.66666666666110,-66.66666666666670,
            -66.66666666666670, 66.66666666666670, 66.66666666666670,-66.66666666666540, 66.66666666666670,-66.66666666666520, 66.66666666666670, 66.66666666666670, 66.66666666666510,-66.66666666666870,
            -66.66666666666670,-66.66666666666590
        };
        #endregion

        #region NameTest
        /// <summary>
        /// A test for Name.
        /// </summary>
        [TestMethod]
        public void NameTest()
        {
            var target = new CommodityChannelIndex(12);
            Assert.AreEqual("CCI", target.Name);
        }
        #endregion

        #region MonikerTest
        /// <summary>
        /// A test for Moniker.
        /// </summary>
        [TestMethod]
        public void MonikerTest()
        {
            var target = new CommodityChannelIndex(12);
            Assert.AreEqual("CCI12", target.Moniker);
        }
        #endregion

        #region DescriptionTest
        /// <summary>
        /// A test for Description.
        /// </summary>
        [TestMethod]
        public void DescriptionTest()
        {
            var target = new CommodityChannelIndex(12);
            Assert.AreEqual("Commodity Channel Index", target.Description);
        }
        #endregion

        #region IsPrimedTest
        /// <summary>
        /// A test for IsPrimed.
        /// </summary>
        [TestMethod]
        public void IsPrimedTest()
        {
            var target = new CommodityChannelIndex(5);
            Assert.IsFalse(target.IsPrimed);
            var scalar = new Scalar(DateTime.Now, 1d);
            for (int i = 1; i <= 4; ++i)
            {
                scalar.Value = i;
                target.Update(scalar);
                Assert.IsFalse(target.IsPrimed);
            }
            scalar.Value = 5d;
            target.Update(scalar);
            Assert.IsTrue(target.IsPrimed);
            scalar.Value = 6d;
            target.Update(scalar);
            Assert.IsTrue(target.IsPrimed);
        }
        #endregion

        #region TaLibTest11
        /// <summary>
        /// A TA-Lib value test length 11.
        /// </summary>
        [TestMethod]
        public void TaLibTest11()
        {
            double u;
            const int digits = 9;
            int count = input.Count;
            var target = new CommodityChannelIndex(11);
            for (int i = 0; i < 10; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(u));
            }
            for (int i = 10; i < count; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                double d = Math.Round(target.Value, digits);
                u = Math.Round(expected11[i], digits);
                Assert.AreEqual(u, d);
            }
        }
        #endregion

        #region TaLibTest2
        /// <summary>
        /// A TA-Lib value test length 2.
        /// </summary>
        [TestMethod]
        public void TaLibTest2()
        {
            const int digits = 8;
            int count = input.Count;
            var target = new CommodityChannelIndex(2);
            double u = target.Update(input[0]);
            Assert.IsTrue(double.IsNaN(u));
            Assert.IsFalse(target.IsPrimed);
            for (int i = 1; i < count; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                double d = Math.Round(target.Value, digits);
                u = Math.Round(expected2[i], digits);
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
            double u;
            const int digits = 9;
            int count = input.Count;
            var target = new CommodityChannelIndex(11);
            for (int i = 0; i < 10; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(u));
            }
            for (int i = 10; i < count; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                double d = Math.Round(target.Value, digits);
                u = Math.Round(expected11[i], digits);
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
            var target = new CommodityChannelIndex(7);
            Assert.AreEqual(7, target.Length);
            target = new CommodityChannelIndex(33);
            Assert.AreEqual(33, target.Length);
        }
        #endregion

        #region InverseScalingFactorTest
        /// <summary>
        /// A test for InverseScalingFactor.
        /// </summary>
        [TestMethod]
        public void InverseScalingFactorTest()
        {
            var target = new CommodityChannelIndex(7);
            Assert.AreEqual(CommodityChannelIndex.DefaultInverseScalingFactor, target.InverseScalingFactor);
            target = new CommodityChannelIndex(7, 1234.56);
            Assert.AreEqual(1234.56, target.InverseScalingFactor);
        }
        #endregion

        #region OhlcvComponentTest
        /// <summary>
        /// A test for OhlcvComponent.
        /// </summary>
        [TestMethod]
        public void OhlcvComponentTest()
        {
            var target = new CommodityChannelIndex(123, CommodityChannelIndex.DefaultInverseScalingFactor, OhlcvComponent.WeightedPrice);
            Assert.AreEqual(target.OhlcvComponent, OhlcvComponent.WeightedPrice);
            target.OhlcvComponent = OhlcvComponent.HighPrice;
            Assert.AreEqual(target.OhlcvComponent, OhlcvComponent.HighPrice);
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
            int count = input.Count;
            var target = new CommodityChannelIndex(11);
            var scalar = new Scalar(DateTime.Now, 1d);
            for (int i = 0; i < 10; ++i)
            {
                scalar.Value = input[i];
                u = target.Update(scalar).Value;
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(u));
            }
            for (int i = 10; i < count; ++i)
            {
                scalar.Value = input[i];
                u = target.Update(scalar).Value;
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                double d = Math.Round(target.Value, digits);
                u = Math.Round(expected11[i], digits);
                Assert.AreEqual(u, d);
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
            double d, u; const int digits = 9;
            int count = input.Count;
            var target = new CommodityChannelIndex(11);
            for (int i = 0; i < 10; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(u));
            }
            for (int i = 10; i < count; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                d = Math.Round(target.Value, digits);
                u = Math.Round(expected11[i], digits);
                Assert.AreEqual(u, d);
            }
            target.Reset();
            for (int i = 0; i < 10; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(u));
            }
            for (int i = 10; i < count; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                d = Math.Round(target.Value, digits);
                u = Math.Round(expected11[i], digits);
                Assert.AreEqual(u, d);
            }
        }
        #endregion

        #region CommodityChannelIndexConstructorTest
        /// <summary>
        /// A test for CommodityChannelIndex Constructor.
        /// </summary>
        [TestMethod]
        public void CommodityChannelIndexConstructorTest()
        {
            var target = new CommodityChannelIndex(123);
            Assert.AreEqual(target.OhlcvComponent, OhlcvComponent.TypicalPrice);
            Assert.AreEqual(target.InverseScalingFactor, CommodityChannelIndex.DefaultInverseScalingFactor);
            Assert.AreEqual(target.Length, 123);
            Assert.IsTrue(double.IsNaN(target.Value));
            Assert.IsFalse(target.IsPrimed);
            target = new CommodityChannelIndex(321, 0.567);
            Assert.AreEqual(target.OhlcvComponent, OhlcvComponent.TypicalPrice);
            Assert.AreEqual(target.InverseScalingFactor, 0.567);
            Assert.AreEqual(target.Length, 321);
            Assert.IsTrue(double.IsNaN(target.Value));
            Assert.IsFalse(target.IsPrimed);
            target = new CommodityChannelIndex(234, 0.876, OhlcvComponent.LowPrice);
            Assert.AreEqual(target.OhlcvComponent, OhlcvComponent.LowPrice);
            Assert.AreEqual(target.InverseScalingFactor, 0.876);
            Assert.AreEqual(target.Length, 234);
            Assert.IsTrue(double.IsNaN(target.Value));
            Assert.IsFalse(target.IsPrimed);
        }

        /// <summary>
        /// A test for CommodityChannelIndex Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CommodityChannelIndexConstructorTest2()
        {
            var target = new CommodityChannelIndex(1);
            Assert.IsNotNull(target);
        }

        /// <summary>
        /// A test for CommodityChannelIndex Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CommodityChannelIndexConstructorTest3()
        {
            var target = new CommodityChannelIndex(-8);
            Assert.IsNotNull(target);
        }
        #endregion

        #region SerializationTest
        private static void SerializeTo(CommodityChannelIndex instance, string fileName)
        {
            var dcs = new DataContractSerializer(typeof(CommodityChannelIndex),
                null, 65536, false, true, null);
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                dcs.WriteObject(fs, instance);
                fs.Close();
            }
        }

        private static CommodityChannelIndex DeserializeFrom(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            var ser = new DataContractSerializer(typeof(CommodityChannelIndex),
                null, 65536, false, true, null);
            var instance = (CommodityChannelIndex)ser.ReadObject(reader, true);
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
            var source = new CommodityChannelIndex(11);
            for (int i = 0; i < 10; ++i)
            {
                u = source.Update(input[i]);
                Assert.IsFalse(source.IsPrimed);
                Assert.IsTrue(double.IsNaN(u));
            }
            for (int i = 10; i < 99; ++i)
            {
                u = source.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, source.Value);
                d = Math.Round(source.Value, digits);
                u = Math.Round(expected11[i], digits);
                Assert.AreEqual(u, d);
            }
            const string fileName = "CommodityChannelIndexTest_1.xml";
            SerializeTo(source, fileName);
            CommodityChannelIndex target = DeserializeFrom(fileName);
            Assert.AreEqual(11, target.Length);
            Assert.IsTrue(target.IsPrimed);
            Assert.AreEqual("CCI", target.Name);
            Assert.AreEqual(source.Value, target.Value);
            Assert.AreEqual("Commodity Channel Index", target.Description);
            for (int i = 99; i < count; ++i)
            {
                u = target.Update(input[i]);
                Assert.IsFalse(double.IsNaN(u));
                Assert.AreEqual(u, target.Value);
                d = Math.Round(target.Value, digits);
                u = Math.Round(expected11[i], digits);
                Assert.AreEqual(u, d);
            }
            //FileInfo fi = new FileInfo(fileName);
            //fi.Delete();
        }
        #endregion
    }
}
