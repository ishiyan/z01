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
    public class BalanceOfPowerTest
    {
        #region Test data
        /// <summary>
        /// Input Open test data, length = 252.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_BOP.xls, column A.
        /// </summary>
        private readonly List<double> inputOpen = new List<double>
        {
             92.500, 91.500, 95.155, 93.970, 95.500, 94.500, 95.000, 91.500, 91.815, 91.125,
             93.875, 97.500, 98.815, 92.000, 91.125, 91.875, 93.405, 89.750, 89.345, 92.250,
             89.780, 87.940, 87.595, 85.220, 83.500, 83.500, 81.250, 85.125, 88.125, 87.500,
             85.250, 86.000, 87.190, 86.125, 89.000, 88.625, 86.000, 85.500, 84.750, 85.250,
             84.250, 86.750, 86.940, 89.315, 89.940, 90.815, 91.190, 91.345, 89.595, 91.000,
             89.750, 88.750, 88.315, 84.345, 83.500, 84.000, 86.000, 85.530, 87.500, 88.500,
             90.000, 88.655, 89.500, 91.565, 92.000, 93.000, 92.815, 91.750, 92.000, 91.375,
             89.750, 88.750, 85.440, 83.500, 84.875, 98.625, 96.690,102.375,106.000,104.625,
            102.500,104.250,104.000,106.125,106.065,105.940,105.625,108.625,110.250,110.565,
            117.000,120.750,118.000,119.125,119.125,117.815,116.375,115.155,111.250,111.500,
            116.690,116.000,113.620,111.750,114.560,113.620,118.120,119.870,116.620,115.870,
            115.060,115.870,117.500,119.870,119.250,120.190,122.870,123.870,122.250,123.120,
            123.310,124.000,123.000,124.810,130.000,130.880,132.500,131.000,132.500,134.000,
            137.440,135.750,138.310,138.000,136.380,136.500,132.000,127.500,127.620,124.000,
            123.620,125.000,126.370,126.250,125.940,124.000,122.750,120.000,120.000,122.000,
            123.620,121.500,120.120,123.750,122.750,125.000,128.500,128.380,123.870,124.370,
            122.750,123.370,122.000,122.620,125.000,124.250,124.370,125.620,126.500,128.380,
            128.880,131.500,132.500,137.500,134.630,132.000,134.000,132.000,131.380,126.500,
            128.750,127.190,127.500,120.500,126.620,123.000,122.060,121.000,121.000,118.000,
            122.000,122.250,119.120,115.000,113.500,114.000,110.810,106.500,106.440,108.000,
            107.000,108.620, 93.000, 93.750, 94.250, 94.870, 95.500, 94.500, 97.000, 98.500,
             96.750, 95.870, 94.440, 92.750, 90.500, 95.060, 94.620, 97.500, 96.000, 96.000,
             94.620, 94.870, 94.000, 99.000,105.500,108.810,105.000,105.940,104.940,103.690,
            102.560,103.440,109.810,113.000,117.000,116.250,120.500,111.620,108.120,110.190,
            107.750,108.000,110.690,109.060,108.500,109.870,109.120,109.690,109.560,110.440,
            109.690,109.190
        };

        /// <summary>
        /// Input High test data, length = 252.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_BOP.xls, column B.
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
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_BOP.xls, column C.
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
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_BOP.xls, column D.
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
        /// Output data, raw BOP, 252 entries.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_BOP.xsl, column H.
        /// </summary>
        private readonly List<double> expectedBop = new List<double>
        {
           -0.400000000000000, 0.937765205091938,-0.367058823529412, 0.418215613382900,-0.540031397174254, 0.102459016393443,-0.823333333333333, 0.314861460957179,-0.495049504950495, 0.632941176470588,
            0.642857142857143,-0.075528700906344,-0.101777059773828,-0.540025412960610,-0.027382256297919, 0.406926406926406,-0.944444444444444,-0.216000000000001, 0.838235294117648,-0.950000000000000,
           -0.493848857644992,-0.159898477157359,-0.776551724137930,-0.689440993788820, 0.000000000000000,-0.548387096774194, 1.000000000000000, 0.772471910112359,-0.691699604743083,-0.396196513470681,
            0.000000000000000, 0.522041763341067,-0.733333333333333, 0.827034883720930,-0.249411764705883,-0.533536585365853, 0.352051835853131,-0.232342007434944,-0.256292906178490,-0.677339901477832,
           -0.451030927835052,-0.363901018922853, 0.867052023121388, 0.085034013605442, 0.365506329113925,-0.049429657794675, 0.112612612612613,-0.862132352941176, 0.542471042471042,-0.302114803625377,
           -0.719999999999999, 0.027027027027026,-0.806999999999999,-0.436692506459947,-0.446280991735539, 0.375000000000000,-0.183999999999999, 0.248120300751879, 0.659038901601829, 0.356979405034325,
           -0.550000000000000,-0.103333333333334, 0.918215613382900,-0.028508771929824, 0.291715285880980, 0.173913043478261, 0.167487684729066,-0.023346303501947,-0.985221674876847,-0.539200000000001,
           -0.291666666666667,-0.966078697421981,-0.500606060606060, 0.407407407407407, 0.315555555555555,-0.453795379537954, 0.467694566813510, 0.607819905213270, 0.000000000000000,-0.607142857142857,
           -0.034172661870503, 0.091269841269841, 0.715488215488216,-0.055555555555556, 0.000000000000000,-0.438333333333333, 0.931677018633541, 0.286307053941908, 0.114155251141553, 0.337712519319939,
            1.000000000000000,-0.300000000000000, 0.196592398427261, 0.068870523415978,-0.416520210896310,-0.708762886597939,-0.565632458233892,-0.733780760626399,-0.262000000000000, 0.872284397630020,
           -0.162352941176470, 0.000000000000000,-0.604477611940299, 0.386100386100386,-0.432000000000001, 0.668539325842695, 0.568019093078758,-0.853018372703412, 0.088167053364268,-0.180232558139536,
           -0.190355329949239,-0.185000000000002,-0.457865168539324, 0.311787072243344, 0.313333333333333, 0.331360946745564, 0.600638977635782,-0.105263157894737, 0.282786885245901,-0.194444444444446,
           -0.072243346007604,-0.575999999999999, 0.632812500000001, 0.581913499344692, 0.302114803625377, 0.498181818181820,-0.488599348534203, 0.630662020905923, 0.445103857566765, 0.949438202247189,
            0.111782477341391, 0.619186046511627,-0.458874458874459,-0.465564738292011,-0.061320754716979,-0.360308285163778,-0.617792421746294, 0.520833333333334,-0.750000000000000, 0.287234042553193,
           -0.261603375527428, 0.425170068027211, 0.697916666666665,-0.439999999999998,-0.107758620689655,-0.417661097852029,-0.685598377281945,-0.393700787401575, 0.495341614906832, 0.250000000000000,
           -0.430722891566268,-0.615168539325842, 0.809644670050761,-0.935943060498218, 0.248000000000002, 0.703264094955490, 0.000000000000000,-0.735725938009787,-0.256906077348068,-0.791540785498490,
            0.660156249999999,-0.522900763358779, 0.115987460815049, 0.083989501312334,-0.409836065573771,-0.361774744027304, 0.060702875399360, 0.543333333333332,-0.251999999999998, 0.274285714285725,
            0.723897911832947,-0.230769230769231, 0.462012320328542,-0.755287009063444,-0.632022471910112, 0.808641975308641,-0.673202614379085,-0.709219858156030,-0.781534460338100, 0.743852459016393,
           -0.482248520710058,-0.694444444444446,-0.863422291993720, 0.986842105263157,-0.665441176470589, 0.163398692810457,-0.711743772241992, 0.000000000000000,-0.584532374100719, 0.575384615384617,
            0.000000000000000,-0.829268292682928,-0.665859564164650,-0.375000000000000, 0.210084033613446,-0.821355236139630,-0.979557069846677, 0.195312500000000, 0.286000000000001,-0.232018561484919,
            0.039215686274511,-0.267326732673268,-0.500000000000000, 0.089622641509433,-0.126666666666665, 0.456521739130433,-0.909090909090909, 0.415094339622638, 0.454545454545455,-0.717213114754099,
           -0.621794871794870,-0.614754098360656,-0.648648648648648,-0.909090909090909, 1.000000000000000,-0.426035502958580, 0.809523809523809,-0.950570342205325,-0.039999999999999,-0.775999999999999,
            0.000000000000000,-0.497777777777780, 0.852878464818764, 0.731851851851852, 0.592500000000001,-0.710594315245477,-0.156739811912226,-0.500000000000000,-0.477707006369429,-0.264705882352940,
            0.392694063926941, 0.622448979591837, 0.417849898580122, 0.750000000000000,-0.101333333333332, 0.369090909090909,-0.722391084093211,-0.598173515981737, 0.255250403877221,-0.283132530120482,
           -0.218023255813954, 0.302030456852791,-0.282786885245901, 0.052044609665428, 0.786407766990293,-0.809523809523811,-0.222222222222222, 0.031914893617023, 0.147928994082840,-0.822857142857142,
           -0.484536082474226,-0.458333333333332
        };

        /// <summary>
        /// Output data, BOP(smoothed with SMA od period 14), 252 entries.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_BOP.xsl, column I.
        /// </summary>
        private readonly List<double> expectedBop14 = new List<double>
        {
            // Begins with 13 NaN values.
            double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
            double.NaN, double.NaN, double.NaN,
           -0.02097890124822040, 0.00563665187335683,-0.03228040513846690,-0.07352223520382630,-0.11882335044546200,-0.02037572963889770,-0.09555137366700080,-0.07201676826069070,-0.10592819241172900,-0.12603549378226000,
           -0.22049136308650400,-0.26640973043344200,-0.30018533013828900,-0.22148696872587200,-0.12773715993494600,-0.17518839910960100,-0.23255432199510700,-0.16509400453478900,-0.11237673572471300,-0.22463163768549800,
           -0.09770057456257400,-0.08024078220978050,-0.10692921851038700,-0.02631467851102570, 0.00633524908567973,-0.01197138706992670,-0.02118230169161520,-0.12482736796554800,-0.20599686289663400,-0.09465746090631480,
           -0.06028385182944880,-0.03417625689273990,-0.07499564411672150,-0.01457093369201110,-0.13522573631073300,-0.07866267865523830,-0.06213255138806150,-0.13870768252042800,-0.12018132291600100,-0.15951754390325200,
           -0.14232844425911700,-0.14198916310915200,-0.08921051890037700,-0.16428566340904700,-0.15263664289858800,-0.13167003057802300,-0.10264081180452300,-0.14997028413399500,-0.09577035416200670,-0.06893145623973120,
           -0.04938816826147730, 0.02287720930144980, 0.03336906761939510, 0.10297533081432800, 0.13250005959704300, 0.09400429651552070, 0.02870429651552060, 0.02101382032504440,-0.06571467954451700,-0.14854646255936600,
           -0.14494446238986000,-0.08311906556446360,-0.10815206886479400,-0.14033214361975000,-0.09488009525238620,-0.11571690138674200,-0.17150660857396500,-0.18591091904536200,-0.17772405156166300,-0.05624477367844450,
           -0.02169874193241280,-0.00086540859907943, 0.03683068883582400, 0.13913662306722400, 0.13048659781968800, 0.11610086179011700, 0.17263714027996600, 0.21065895693614400, 0.14581467799233900, 0.15985699216571500,
            0.20814366220563200, 0.18083312298950300, 0.12368792814180400, 0.03217930859022500,-0.01626534891483520,-0.03497963462912100, 0.05863591758254710,-0.01950907954673940,-0.03995958339973290,-0.09129050219129370,
           -0.08783422599269030,-0.19011994027840500,-0.12093855986106900,-0.09440808167167660,-0.16025728853734700,-0.12420819823302000,-0.08645603191456300,-0.05965052275137350,-0.02045189699234520,-0.03444226617372550,
           -0.07447778941563090,-0.04050019837921640,-0.01683155932596180, 0.06924819707232970, 0.03415080107267800, 0.08520700716167110, 0.02356530928401820,-0.02216772207929340,-0.00238069545762108, 0.03652255073064560,
            0.09096155483666190, 0.12613799294913400, 0.17493669424783600, 0.17274139567677300, 0.19551817772410000, 0.20493035802648800, 0.24907873341946200, 0.21416041196986200, 0.26590678371317400, 0.21293097341886200,
            0.19356523814403600, 0.19434542323622400, 0.20975197429595400, 0.12042305131407600, 0.11606018231326400, 0.04090912491145180, 0.02584142665226420, 0.04205542472417670, 0.02737742808998300, 0.04543548588283300,
           -0.05381009999196610,-0.06949160699418370,-0.14355211730587300,-0.15974668290640800,-0.15461354355709100,-0.11485194572681900,-0.07125849678654920,-0.05789638748797590,-0.13903937839220300,-0.02763618767429120,
           -0.11500598074939200,-0.07860573964028990,-0.05874188057398430,-0.10859307105017500,-0.12971635233658800,-0.14036974209790300,-0.16707543407265100,-0.07095010355251170,-0.08017867326374060,-0.10727539855601100,
           -0.11913329131941600,-0.11764137517709400,-0.09954181837005590,-0.15303766084515600,-0.04737506128575960,-0.08308934700004530,-0.11373065990502800,-0.06202366620267510,-0.02595532997120680, 0.02539598414855100,
            0.02798553960819720,-0.06431294052823930, 0.03079725509086220,-0.02557346456586170,-0.08223127595645910,-0.10878116129676800,-0.02980778965079000,-0.06859003223003420,-0.15700273064273300,-0.20067575149942700,
           -0.14977886642961000,-0.24901737273700600,-0.22086252105274200,-0.30470224195063800,-0.25075316987467700,-0.24736102003114900,-0.26402226002572300,-0.21593635899864500,-0.22451124717913800,-0.21624875459532000,
           -0.29616678738220500,-0.24671446207338300,-0.25577951862303900,-0.26407485989825100,-0.32061268884561900,-0.25265260481200500,-0.28089669440453200,-0.22725673308192500,-0.24635149970144400,-0.24031347297996400,
           -0.27501075682819100,-0.28405837587581000,-0.19221623074628500,-0.20958989824101700,-0.15315458826797200,-0.13569305820140000,-0.12825433524529000,-0.10269989252730400,-0.16056179241020800,-0.22732241017082600,
           -0.27568472071411100,-0.20705726973371900,-0.21839361046838400,-0.12485619550239800,-0.19915569433916600,-0.19296521814869000,-0.28100248522943500,-0.21606742029437000,-0.28127257153725700,-0.25282021366059200,
           -0.14931557318873900,-0.06258022520339080,-0.06942595498087800,-0.03428960949970500,-0.00506883027892573,-0.11061933073388500,-0.09909578640491070,-0.12886933966183000,-0.01651081667631820, 0.01619274750797600,
            0.12519274750797600, 0.11795465226988100, 0.17987384418907300, 0.06735459069536040,-0.02764722129275300,-0.05173647815866590,-0.02120349350688050,-0.02558088235700390, 0.03170700741819550, 0.04562987321273320,
            0.06825490835690230, 0.09637731571857030,-0.00590645493254737,-0.05162589213271480,-0.10291768544578500,-0.08511323348748660,-0.17025238005520500,-0.15326273708242000,-0.14327415260753400
        };
        #endregion

        #region NameTest
        /// <summary>
        /// A test for Name.
        /// </summary>
        [TestMethod]
        public void NameTest()
        {
            var target = new BalanceOfPower();
            Assert.AreEqual("Bop", target.Name);
            target = new BalanceOfPower(11, new SimpleMovingAverage(11));
            Assert.AreEqual("Bop", target.Name);
        }
        #endregion

        #region MonikerTest
        /// <summary>
        /// A test for Moniker.
        /// </summary>
        [TestMethod]
        public void MonikerTest()
        {
            var target = new BalanceOfPower();
            Assert.AreEqual("Bop", target.Moniker);
            target = new BalanceOfPower(11, new SimpleMovingAverage(11));
            Assert.AreEqual("Bop(SMA11)", target.Moniker);
            target = new BalanceOfPower(11, new ExponentialMovingAverage(11));
            Assert.AreEqual("Bop(EMA11)", target.Moniker);
        }
        #endregion

        #region DescriptionTest
        /// <summary>
        /// A test for Description.
        /// </summary>
        [TestMethod]
        public void DescriptionTest()
        {
            var target = new BalanceOfPower();
            Assert.AreEqual("Balance of Power", target.Description);
            target = new BalanceOfPower(11, new SimpleMovingAverage(11));
            Assert.AreEqual("Balance of Power", target.Description);
            target = new BalanceOfPower(11, new ExponentialMovingAverage(11));
            Assert.AreEqual("Balance of Power", target.Description);
        }
        #endregion

        #region IsPrimedTest
        /// <summary>
        /// A test for IsPrimed.
        /// </summary>
        [TestMethod]
        public void IsPrimedTest()
        {
            var target = new BalanceOfPower(5, new SimpleMovingAverage(5));
            Assert.IsFalse(target.IsPrimed);
            var scalar = new Scalar(DateTime.Now, 1d);
            for (int i = 1; i < 5; ++i)
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

        #region TaLibTestRaw
        /// <summary>
        /// A TA-Lib raw value test.
        /// </summary>
        [TestMethod]
        public void TaLibTestRaw()
        {
            const int digits = 9;
            int count = inputOpen.Count;
            var target = new BalanceOfPower();
            for (int i = 0; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, inputOpen[i], inputHigh[i], inputLow[i], inputClose[i], double.NaN);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedBop[i], digits);
                Assert.AreEqual(u, d);
                Assert.AreEqual(dateTime, scalar.Time);
            }
        }
        #endregion

        #region TaLibTest14
        /// <summary>
        /// A TA-Lib value length 14 test.
        /// </summary>
        [TestMethod]
        public void TaLibTest14()
        {
            const int digits = 9;
            int count = inputOpen.Count;
            var target = new BalanceOfPower(14, new SimpleMovingAverage(14));
            for (int i = 0; i < 13; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, inputOpen[i], inputHigh[i], inputLow[i], inputClose[i], double.NaN);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(scalar.Value));
                Assert.IsTrue(double.IsNaN(target.Value));
                Assert.AreEqual(dateTime, scalar.Time);
            }
            for (int i = 13; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, inputOpen[i], inputHigh[i], inputLow[i], inputClose[i], double.NaN);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedBop14[i], digits);
                Assert.AreEqual(u, d);
                Assert.AreEqual(dateTime, scalar.Time);
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
            int count = inputOpen.Count;
            var target = new BalanceOfPower();
            for (int i = 0; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, inputOpen[i], inputHigh[i], inputLow[i], inputClose[i], double.NaN);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedBop[i], digits);
                Assert.AreEqual(u, d);
                Assert.AreEqual(dateTime, scalar.Time);
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
            var target = new BalanceOfPower();
            Assert.AreEqual(0, target.Length);
            target = new BalanceOfPower(11, new SimpleMovingAverage(11));
            Assert.AreEqual(11, target.Length);
            target = new BalanceOfPower(34, new ExponentialMovingAverage(34));
            Assert.AreEqual(34, target.Length);
        }
        #endregion

        #region MovingAverageIndicatorTest
        /// <summary>
        /// A test for MovingAverageIndicator.
        /// </summary>
        [TestMethod]
        public void MovingAverageIndicatorTest()
        {
            var target = new BalanceOfPower();
            Assert.IsNull(target.MovingAverageIndicator);
            ILineIndicator indicator = new SimpleMovingAverage(11);
            target = new BalanceOfPower(11, indicator);
            Assert.AreEqual(indicator, target.MovingAverageIndicator);
            indicator = new ExponentialMovingAverage(34);
            target = new BalanceOfPower(34, indicator);
            Assert.AreEqual(indicator, target.MovingAverageIndicator);
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
            int count = inputOpen.Count;
            var target = new BalanceOfPower();
            for (int i = 0; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, inputOpen[i], inputHigh[i], inputLow[i], inputClose[i], double.NaN);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedBop[i], digits);
                Assert.AreEqual(u, d);
                Assert.AreEqual(dateTime, scalar.Time);
            }
            target = new BalanceOfPower();
            for (int i = 0; i < count; ++i)
            {
                double d = target.Update(inputClose[i]);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(d));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(d, target.Value);
                d = Math.Round(d, digits);
                Assert.AreEqual(0d, d);
            }
            target = new BalanceOfPower();
            for (int i = 0; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                Scalar scalar = target.Update(inputClose[i], dateTime);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                Assert.AreEqual(0d, d);
                Assert.AreEqual(dateTime, scalar.Time);
            }
            target = new BalanceOfPower();
            for (int i = 0; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                Scalar scalar = target.Update(new Scalar(dateTime, inputClose[i]));
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                Assert.AreEqual(0d, d);
                Assert.AreEqual(dateTime, scalar.Time);
            }
            target = new BalanceOfPower();
            for (int i = 0; i < count; ++i)
            {
                double d = target.Update(inputOpen[i], inputHigh[i], inputLow[i], inputClose[i]);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(d));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(d, target.Value);
                d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedBop[i], digits);
                Assert.AreEqual(u, d);
            }
            double w = target.Update(double.NaN, 1d, 2d, 3d);
            Assert.IsTrue(double.IsNaN(w));
            w = target.Update(1d, double.NaN, 2d, 3d);
            Assert.IsTrue(double.IsNaN(w));
            w = target.Update(1d, 2d, double.NaN, 3d);
            Assert.IsTrue(double.IsNaN(w));
            w = target.Update(1d, 2d, 3d, double.NaN);
            Assert.IsTrue(double.IsNaN(w));
            target = new BalanceOfPower();
            for (int i = 0; i < 10; ++i)
                w = target.Update(0.001, 0.001, 0.001, 0.001);
            Assert.AreEqual(0d, w);
        }
        #endregion

        #region ResetTest
        /// <summary>
        /// A test for Reset.
        /// </summary>
        [TestMethod]
        public void ResetTest()
        {
            const int digits = 9;
            int count = inputOpen.Count;
            var target = new BalanceOfPower(14, new SimpleMovingAverage(14));
            for (int i = 0; i < 13; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, inputOpen[i], inputHigh[i], inputLow[i], inputClose[i], double.NaN);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(scalar.Value));
                Assert.IsTrue(double.IsNaN(target.Value));
                Assert.AreEqual(dateTime, scalar.Time);
            }
            for (int i = 13; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, inputOpen[i], inputHigh[i], inputLow[i], inputClose[i], double.NaN);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedBop14[i], digits);
                Assert.AreEqual(u, d);
                Assert.AreEqual(dateTime, scalar.Time);
            }
            target.Reset();
            for (int i = 0; i < 13; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, inputOpen[i], inputHigh[i], inputLow[i], inputClose[i], double.NaN);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(scalar.Value));
                Assert.IsTrue(double.IsNaN(target.Value));
                Assert.AreEqual(dateTime, scalar.Time);
            }
            for (int i = 13; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, inputOpen[i], inputHigh[i], inputLow[i], inputClose[i], double.NaN);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedBop14[i], digits);
                Assert.AreEqual(u, d);
                Assert.AreEqual(dateTime, scalar.Time);
            }
        }
        #endregion

        #region ToStringTest
        /// <summary>
        /// A test for ToString.
        /// </summary>
        [TestMethod]
        public void ToStringTest()
        {
            var target = new BalanceOfPower();
            Assert.AreEqual("[M:Bop P:True V:NaN]", target.ToString());
            target = new BalanceOfPower(11, new ExponentialMovingAverage(11));
            Assert.AreEqual("[M:Bop(EMA11) P:False V:NaN]", target.ToString());
        }
        #endregion

        #region BalanceOfPowerConstructorTest
        /// <summary>
        /// A test for BalanceOfPower Constructor.
        /// </summary>
        [TestMethod]
        public void BalanceOfPowerConstructorTest()
        {
            var target = new BalanceOfPower();
            Assert.AreEqual(0, target.Length);
            Assert.IsTrue(double.IsNaN(target.Value));
            Assert.IsTrue(target.IsPrimed);
            target = new BalanceOfPower(2, new SimpleMovingAverage(2));
            Assert.AreEqual(2, target.Length);
            Assert.IsTrue(double.IsNaN(target.Value));
            Assert.IsFalse(target.IsPrimed);
            target = new BalanceOfPower(112, new ExponentialMovingAverage(112));
            Assert.AreEqual(112, target.Length);
            Assert.IsTrue(double.IsNaN(target.Value));
            Assert.IsFalse(target.IsPrimed);
        }

        /// <summary>
        /// A test for BalanceOfPower Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BalanceOfPowerConstructorTest2()
        {
            var target = new BalanceOfPower(1, new SimpleMovingAverage(11));
            Assert.IsNotNull(target);
        }

        /// <summary>
        /// A test for BalanceOfPower Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BalanceOfPowerConstructorTest3()
        {
            var target = new BalanceOfPower(0, new SimpleMovingAverage(11));
            Assert.IsNotNull(target);
        }

        /// <summary>
        /// A test for BalanceOfPower Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BalanceOfPowerConstructorTest4()
        {
            var target = new BalanceOfPower(-8, new SimpleMovingAverage(11));
            Assert.IsNotNull(target);
        }
        #endregion

        #region SerializationTest
        private static void SerializeTo(BalanceOfPower instance, string fileName)
        {
            var dcs = new DataContractSerializer(typeof(BalanceOfPower),
                new[] { typeof(SimpleMovingAverage) }, 65536, false, true, null);
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                dcs.WriteObject(fs, instance);
                fs.Close();
            }
        }

        private static BalanceOfPower DeserializeFrom(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            var ser = new DataContractSerializer(typeof(BalanceOfPower),
                new[] { typeof(SimpleMovingAverage) }, 65536, false, true, null);
            var instance = (BalanceOfPower)ser.ReadObject(reader, true);
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
            const int digits = 9;
            int count = inputOpen.Count;
            var source = new BalanceOfPower(14, new SimpleMovingAverage(14));
            for (int i = 0; i < 13; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, inputOpen[i], inputHigh[i], inputLow[i], inputClose[i], double.NaN);
                Scalar scalar = source.Update(ohlcv);
                Assert.IsFalse(source.IsPrimed);
                Assert.IsTrue(double.IsNaN(scalar.Value));
                Assert.IsTrue(double.IsNaN(source.Value));
                Assert.AreEqual(dateTime, scalar.Time);
            }
            for (int i = 13; i < 99; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, inputOpen[i], inputHigh[i], inputLow[i], inputClose[i], double.NaN);
                Scalar scalar = source.Update(ohlcv);
                Assert.IsTrue(source.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(source.Value));
                Assert.AreEqual(scalar.Value, source.Value);
                double d = Math.Round(source.Value, digits);
                double u = Math.Round(expectedBop14[i], digits);
                Assert.AreEqual(u, d);
                Assert.AreEqual(dateTime, scalar.Time);
            }
            const string fileName = "BalanceOfPowerTest_1.xml";
            SerializeTo(source, fileName);
            BalanceOfPower target = DeserializeFrom(fileName);
            Assert.AreEqual(14, target.Length);
            Assert.IsTrue(target.IsPrimed);
            Assert.AreEqual("Bop", target.Name);
            Assert.AreEqual(source.Value, target.Value);
            Assert.AreEqual("Balance of Power", target.Description);
            for (int i = 99; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, inputOpen[i], inputHigh[i], inputLow[i], inputClose[i], double.NaN);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedBop14[i], digits);
                Assert.AreEqual(u, d);
                Assert.AreEqual(dateTime, scalar.Time);
            }
            //FileInfo fi = new FileInfo(fileName);
            //fi.Delete();
        }
        #endregion
    }
}
