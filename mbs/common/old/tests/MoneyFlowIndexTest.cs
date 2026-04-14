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
    public class MoneyFlowIndexTest
    {
        #region Test data
        /// <summary>
        /// Input High test data, length = 252.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MF.xls, column B.
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
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MF.xls, column C.
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
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MF.xls, column D.
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
        /// Input Volume test data, length = 252.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MF.xsl, column E.
        /// </summary>
        private readonly List<double> inputVolume = new List<double>
        {
            4077500, 4955900, 4775300, 4155300, 4593100, 3631300, 3382800, 4954200, 4500000, 3397500,
            4204500, 6321400,10203600,19043900,11692000, 9553300, 8920300, 5970900, 5062300, 3705600,
            5865600, 5603000, 5811900, 8483800, 5995200, 5408800, 5430500, 6283800, 5834800, 4515500,
            4493300, 4346100, 3700300, 4600200, 4557200, 4323600, 5237500, 7404100, 4798400, 4372800,
            3872300,10750800, 5804800, 3785500, 5014800, 3507700, 4298800, 4842500, 3952200, 3304700,
            3462000, 7253900, 9753100, 5953000, 5011700, 5910800, 4916900, 4135000, 4054200, 3735300,
            2921900, 2658400, 4624400, 4372200, 5831600, 4268600, 3059200, 4495500, 3425000, 3630800,
            4168100, 5966900, 7692800, 7362500, 6581300,19587700,10378600, 9334700,10467200, 5671400,
            5645000, 4518600, 4519500, 5569700, 4239700, 4175300, 4995300, 4776600, 4190000, 6035300,
           12168900, 9040800, 5780300, 4320800, 3899100, 3221400, 3455500, 4304200, 4703900, 8316300,
           10553900, 6384800, 7163300, 7007800, 5114100, 5263800, 6666100, 7398400, 5575000, 4852300,
            4298100, 4900500, 4887700, 6964800, 4679200, 9165000, 6469800, 6792000, 4423800, 5231900,
            4565600, 6235200, 5225900, 8261400, 5912500, 3545600, 5714500, 6653900, 6094500, 4799200,
            5050800, 5648900, 4726300, 5585600, 5124800, 7630200,14311600, 8793600, 8874200, 6966600,
            5525500, 6515500, 5291900, 5711700, 4327700, 4568000, 6859200, 5757500, 7367000, 6144100,
            4052700, 5849700, 5544700, 5032200, 4400600, 4894100, 5140000, 6610900, 7585200, 5963100,
            6045500, 8443300, 6464700, 6248300, 4357200, 4774700, 6216900, 6266900, 5584800, 5284500,
            7554500, 7209500, 8424800, 5094500, 4443600, 4591100, 5658400, 6094100,14862200, 7544700,
            6985600, 8093000, 7590000, 7451300, 7078000, 7105300, 8778800, 6643900,10563900, 7043100,
            6438900, 8057700,14240000,17872300, 7831100, 8277700,15017800,14183300,13921100, 9683000,
            9187300,11380500,69447300,26673600,13768400,11371600, 9872200, 9450500,11083300, 9552800,
           11108400,10374200,16701900,13741900, 8523600, 9551900, 8680500, 7151700, 9673100, 6264700,
            8541600, 8358000,18720800,19683100,13682500,10668100, 9710600, 3113100, 5682000, 5763600,
            5340000, 6220800,14680500, 9933000,11329500, 8145300,16644700,12593800, 7138100, 7442300,
            9442300, 7123600, 7680600, 4839800, 4775500, 4008800, 4533600, 3741100, 4084800, 2685200,
            3438000, 2870500
        };

        /// <summary>
        /// Output data, MFI, 252 entries.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MF.xsl, column K.
        /// </summary>
        private readonly List<double> expectedMfi = new List<double>
        {
            // Begins with 14 NaN values.
            double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
            double.NaN, double.NaN, double.NaN, double.NaN,
            42.892339191984400, 45.607156851091500, 38.878793951593400, 38.290129948202000, 43.122883231836800, 39.472063174605500, 38.619504683535500, 38.480240953756300, 38.117382822517500, 33.493804027688200,
            29.220342754571200, 23.520352146535000, 19.081931442350100, 28.481290408467600, 30.490812178544800, 25.775155811166700, 27.381160459828800, 33.714897815218800, 27.346236954569300, 33.230236653265300,
            40.118024000754600, 40.823048765546300, 41.135108269566700, 41.680829047530400, 42.339545647269900, 49.001919128375700, 42.392172404098500, 45.867453946118200, 53.997002568739900, 53.588498226773500,
            60.228835056232500, 54.754579072612200, 60.351233895099000, 53.680943493294200, 53.327997144285700, 58.838359573789600, 60.213410800320400, 60.097703903620900, 56.094189206003500, 49.441225266874700,
            48.766676072630400, 45.280879088501500, 44.466732201976600, 44.639607885232800, 43.763066748750100, 49.078132475724300, 43.620818288478700, 45.125370367976700, 45.726730247415200, 46.652805486780400,
            53.587014163017200, 62.395368203121500, 73.992133516387600, 75.233585066283000, 76.739092816158000, 69.478067069803000, 61.791039405717700, 52.979119553844600, 43.611085952147000, 35.632094488668100,
            43.132069849049600, 58.558612786918400, 61.622886855899200, 64.070474573453800, 66.240508852609600, 60.836814969402700, 56.163526421896500, 60.526841459144100, 64.219258150120700, 68.268188287812500,
            67.762758475020000, 68.255367180145800, 74.169912621879500, 79.958351174098000, 79.766574398741300, 76.948814878544800, 78.024986385746000, 66.829423996542400, 57.693477934536300, 63.899566273645300,
            64.773628555603800, 60.088928403603700, 55.132852436560800, 48.895793014737500, 48.457969622396200, 56.199374948151300, 46.859474107550700, 47.995759390064600, 41.971670804312500, 42.610662519661100,
            37.280126307767600, 45.847228642327700, 53.909440152338700, 46.556511995229400, 52.329762223590400, 51.377532200329000, 50.904222971411900, 56.061920872684300, 61.387710037561000, 60.912248924847500,
            71.100278345574400, 72.184058056313500, 80.399963566853100, 80.515149594892200, 74.904684087431900, 68.481225516631800, 67.717679698804500, 68.440193555062700, 68.408830319584900, 75.573128494188300,
            81.010664199402400, 80.826774345443100, 81.168947637919100, 81.245132900086000, 81.688875940994300, 80.879355218425200, 80.669011621145200, 80.534864532093900, 79.701177080956400, 78.780045284268800,
            79.051033840647100, 77.065944806840600, 63.831290718231500, 54.562696176753600, 46.770498008688100, 41.618305153003300, 35.869938038196300, 35.547087430032400, 34.816485828319300, 29.392596702896100,
            28.532501849189100, 22.610761929506100, 22.287085626136000, 22.421382137723100, 23.653109915982100, 30.466816048698600, 34.275225748947100, 35.689403537495200, 44.091275513979800, 45.241681051659700,
            51.608309269494300, 50.581636957184600, 50.485404665744000, 49.875703386910600, 42.352927398873300, 41.646500766605900, 49.530926863595900, 47.867674833431800, 47.331811032466300, 47.391959326311600,
            52.489788275380300, 53.073778093167800, 53.515508782535700, 60.260033160276800, 54.190880840068300, 54.457242157662000, 55.845104249410000, 63.853565749535800, 72.800838543904700, 79.228436235412000,
            73.565658287475200, 76.487189107132600, 76.400128755798000, 68.940655109333700, 56.653167948395200, 62.793860016393300, 55.979475215441400, 48.717429670408200, 47.819892167528300, 41.767172161477900,
            41.257187492959700, 34.156880635363700, 25.558893314722900, 21.337922958584500, 20.249577219401300, 26.302212477127900, 26.468571764037900, 26.122801624778700, 26.565090844618700, 17.687510587310800,
            24.044011125948200, 24.202696170228300, 23.103025879725400, 22.213969197816800, 25.210640416611600, 31.412015035001700, 37.724747125035200, 42.215013664095700, 32.604406046786600, 37.282270695486000,
            38.585983766176300, 42.820594099402800, 44.147604863667400, 50.287951685924600, 50.692411629161800, 50.680013446112800, 51.927950477513600, 53.181132635422700, 46.257450161681700, 41.114616150641800,
            40.597286990830600, 39.645289911455000, 58.383535609990900, 48.273526416074600, 40.263533215303500, 33.635443700142000, 33.942981201094800, 27.417051677325700, 31.206368431967500, 42.421829079077100,
            50.820450787400300, 57.657991643241500, 59.691077910231700, 65.938534386077600, 61.184231419957100, 55.942197744715000, 55.008036152423900, 60.065503010643300, 68.672164665302100, 73.945187872402500,
            80.199366102364600, 85.581304822458100, 73.118251304665500, 62.572280531032600, 60.757153390120200, 53.881595106361000, 53.890465195840400, 55.338949333747200, 60.237735832562900, 60.518802776206900,
            60.437554224385300, 56.727638360871000, 52.935722699624500, 50.106574450994700, 46.239260209445200, 40.103335987977600, 46.950189199635600, 53.199678850628700
        };

        /// <summary>
        /// Output data, MFI(volume=1), 252 entries.
        /// Taken from TA-Lib (http://ta-lib.org/) tests, test_MF.xsl, column S.
        /// </summary>
        private readonly List<double> expectedMfiVolume1 = new List<double>
        {
            // Begins with 14 NaN values.
            double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN,
            double.NaN, double.NaN, double.NaN, double.NaN,
            50.77504399035770, 50.72298127159680, 43.61862993264350, 43.79768067843300, 50.92286786476000, 43.77382669827870, 43.92457350732410, 44.07928046757150, 44.24666781344970, 37.30092831254890,
            30.10207527008440, 22.61440852345010, 21.70072862017890, 28.89577725208690, 28.96642707421910, 28.66191997415870, 28.77736596842310, 36.00088727110400, 28.59417142699070, 35.96613933834180,
            43.34968267127290, 43.34211182879160, 43.33268201830060, 43.29326543666250, 43.26930261777790, 50.18478070157260, 43.27056970768640, 43.21322438390980, 50.48112703438240, 50.57155694482350,
            57.83649828545080, 50.53751237468070, 57.80206526858570, 50.54316216319050, 50.60189942056410, 57.85274918998110, 57.70289852136650, 57.54599326599330, 57.45661630729300, 50.66797935740020,
            50.69347912968940, 50.61619409844550, 50.50271994250770, 50.38798420171120, 50.28958118244630, 57.63277446167010, 50.26156848548850, 50.33047512101520, 50.36429649240360, 50.41325574782620,
            57.80475886688480, 65.11346812955040, 72.23487129309050, 71.81019646827430, 71.36271110821940, 64.34343888292610, 57.42195176314960, 50.58472358521430, 43.74148570127370, 36.80285878435170,
            43.72394206797880, 51.15769562936780, 51.49698314586790, 51.94577554086970, 52.45730642031500, 44.91396204403470, 37.48901776962960, 44.97564091923150, 52.33674971046420, 59.56016271153110,
            58.82859185583420, 58.03092367506310, 64.65902696186350, 71.10236794002740, 71.59979001861200, 71.86671289063380, 72.26243836720110, 64.66183659754650, 57.21976161791330, 64.33981686580760,
            63.71865722865270, 56.58951166217800, 49.58519403537630, 42.69601120587870, 42.53447361719360, 49.45558572878720, 42.55997225857320, 42.77441729669260, 35.93366415011810, 35.97429022616100,
            35.66813377831250, 42.90260262724080, 50.28900659213960, 42.93967739132950, 50.26510235422470, 50.32307627001950, 50.35545195545190, 57.42610922790310, 64.44707527498250, 64.54870958520400,
            71.74529375226780, 71.83190989742680, 78.87607396812840, 79.03054299098170, 71.76691300282510, 64.55879569206860, 64.64544469732360, 64.42914196963450, 64.55728184852620, 71.59936550211500,
            78.52111446168890, 78.72137551327990, 78.91094768128640, 79.05413588470390, 79.21320883694030, 79.39415601822890, 79.54177479095970, 79.68706988625730, 79.06230654382580, 78.48526365637730,
            78.62872819459510, 78.09925314044810, 71.27822331015930, 64.41817107969060, 57.62270814011560, 50.78692439761030, 43.89796523912710, 43.68905927923440, 43.49783662684030, 36.30683246559720,
            35.89659439886330, 28.55165582861650, 28.82469595521090, 29.09889787032090, 28.50129336377140, 35.79959614810960, 35.92799777391840, 36.10029154968690, 43.24947653889840, 43.30753132410340,
            50.47125827507090, 50.50410144280310, 50.51118142929770, 50.50737383304020, 43.28969552904470, 43.31384257223820, 50.40461377600230, 50.32495474947220, 50.34938599485810, 50.34335227557690,
            57.47142166036730, 57.35259043838190, 57.40633308232550, 64.54012641256460, 57.37892119357310, 57.43485511579080, 57.51155535894070, 64.82356965055960, 72.01615400300580, 79.10599073159670,
            71.76130019590920, 71.33327898134490, 71.50004605323750, 64.41811005271980, 57.50202906091580, 64.40944881889760, 57.48668221699830, 50.57466648901320, 50.62659755426890, 43.69200152463610,
            43.46522175100960, 36.36091398614710, 29.12900162020220, 23.31566125219890, 23.53203172666260, 31.05126148506300, 30.55881526535870, 30.75541139043100, 30.94807672993870, 23.07939399446740,
            30.60219090864540, 30.87813824286350, 31.21586169271670, 31.56924077928990, 30.80000889145750, 38.34833518749510, 45.99627011280410, 49.72669862926900, 50.59558730302000, 49.78013600330290,
            48.85051717271500, 56.15181854177270, 57.04562693943220, 64.40105415586050, 63.97703553978730, 64.63083191333400, 65.18340190002540, 65.73770372755150, 58.60977066061100, 51.35682472562700,
            50.83735140234150, 50.26716716742050, 57.34122731593660, 50.17446251447560, 43.05406404591310, 35.87468768792190, 35.86340815566800, 28.73088677316870, 28.66462233129820, 36.26935462415200,
            43.92916760995410, 51.45018791202020, 51.00633755674240, 58.11260520460480, 50.95948111261430, 43.91379343206210, 44.20530468396340, 51.33427677324710, 58.51253833454030, 65.61870713388580,
            72.54077978913870, 79.24332377204100, 71.87342900358490, 64.82463999300750, 64.88801542568970, 57.81073624845820, 57.70849693391660, 57.79580513092140, 64.71118743187360, 64.46427532656680,
            64.60299678517320, 57.70324254659580, 57.65401589080910, 57.49796706866360,57.29927401698360, 49.91761406864770, 50.13481443638830, 50.18422766430790
        };
        #endregion

        #region NameTest
        /// <summary>
        /// A test for Name.
        /// </summary>
        [TestMethod]
        public void NameTest()
        {
            var target = new MoneyFlowIndex(12);
            Assert.AreEqual("MFI", target.Name);
        }
        #endregion

        #region MonikerTest
        /// <summary>
        /// A test for Moniker.
        /// </summary>
        [TestMethod]
        public void MonikerTest()
        {
            var target = new MoneyFlowIndex(12);
            Assert.AreEqual("MFI(12)", target.Moniker);
            target = new MoneyFlowIndex(34);
            Assert.AreEqual("MFI(34)", target.Moniker);
        }
        #endregion

        #region DescriptionTest
        /// <summary>
        /// A test for Description.
        /// </summary>
        [TestMethod]
        public void DescriptionTest()
        {
            var target = new MoneyFlowIndex();
            Assert.AreEqual("Money Flow Index", target.Description);
        }
        #endregion

        #region IsPrimedTest
        /// <summary>
        /// A test for IsPrimed.
        /// </summary>
        [TestMethod]
        public void IsPrimedTest()
        {
            var target = new MoneyFlowIndex(5);
            Assert.IsFalse(target.IsPrimed);
            var scalar = new Scalar(DateTime.Now, 1d);
            for (int i = 1; i <= 5; ++i)
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

        #region TaLibTest
        /// <summary>
        /// A TA-Lib value test length 14.
        /// </summary>
        [TestMethod]
        public void TaLibTest()
        {
            const int digits = 9;
            int count = inputHigh.Count;
            var target = new MoneyFlowIndex();
            for (int i = 0; i < 14; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, double.NaN, inputHigh[i], inputLow[i], inputClose[i], inputVolume[i]);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(scalar.Value));
                Assert.IsTrue(double.IsNaN(target.Value));
                Assert.AreEqual(dateTime, scalar.Time);
            }
            for (int i = 14; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, double.NaN, inputHigh[i], inputLow[i], inputClose[i], inputVolume[i]);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedMfi[i], digits);
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
            int count = inputHigh.Count;
            var target = new MoneyFlowIndex();
            for (int i = 0; i < 14; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, double.NaN, inputHigh[i], inputLow[i], inputClose[i], inputVolume[i]);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(scalar.Value));
                Assert.IsTrue(double.IsNaN(target.Value));
                Assert.AreEqual(dateTime, scalar.Time);
            }
            for (int i = 14; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, double.NaN, inputHigh[i], inputLow[i], inputClose[i], inputVolume[i]);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedMfi[i], digits);
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
            var target = new MoneyFlowIndex();
            Assert.AreEqual(14, target.Length);
            target = new MoneyFlowIndex(7);
            Assert.AreEqual(7, target.Length);
            target = new MoneyFlowIndex(33);
            Assert.AreEqual(33, target.Length);
            target = new MoneyFlowIndex(1);
            Assert.AreEqual(1, target.Length);
        }
        #endregion

        #region OhlcvComponentTest
        /// <summary>
        /// A test for OhlcvComponent.
        /// </summary>
        [TestMethod]
        public void OhlcvComponentTest()
        {
            var target = new MoneyFlowIndex();
            Assert.AreEqual(target.OhlcvComponent, OhlcvComponent.TypicalPrice);
            target = new MoneyFlowIndex(123);
            Assert.AreEqual(target.OhlcvComponent, OhlcvComponent.TypicalPrice);
            target = new MoneyFlowIndex(123, OhlcvComponent.WeightedPrice);
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
            const int digits = 9;
            int count = inputHigh.Count;
            var target = new MoneyFlowIndex();
            for (int i = 0; i < 14; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, double.NaN, inputHigh[i], inputLow[i], inputClose[i], inputVolume[i]);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(scalar.Value));
                Assert.IsTrue(double.IsNaN(target.Value));
                Assert.AreEqual(dateTime, scalar.Time);
            }
            for (int i = 14; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, double.NaN, inputHigh[i], inputLow[i], inputClose[i], inputVolume[i]);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedMfi[i], digits);
                Assert.AreEqual(u, d);
                Assert.AreEqual(dateTime, scalar.Time);
            }
            target = new MoneyFlowIndex();
            for (int i = 0; i < 14; ++i)
            {
                double d = target.Update((inputHigh[i] + inputLow[i] + inputClose[i]) / 3, inputVolume[i]);
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(d));
                Assert.IsTrue(double.IsNaN(target.Value));
            }
            for (int i = 14; i < count; ++i)
            {
                double d = target.Update((inputHigh[i] + inputLow[i] + inputClose[i]) / 3, inputVolume[i]);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(d));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(d, target.Value);
                d = Math.Round(d, digits);
                double u = Math.Round(expectedMfi[i], digits);
                Assert.AreEqual(u, d);
            }
            target = new MoneyFlowIndex();
            for (int i = 0; i < 14; ++i)
            {
                DateTime dateTime = DateTime.Now;
                Scalar scalar = target.Update(new Scalar(dateTime, (inputHigh[i] + inputLow[i] + inputClose[i])/3));
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(scalar.Value));
                Assert.IsTrue(double.IsNaN(target.Value));
                Assert.AreEqual(dateTime, scalar.Time);
            }
            for (int i = 14; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                Scalar scalar = target.Update(new Scalar(dateTime, (inputHigh[i] + inputLow[i] + inputClose[i]) / 3));
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedMfiVolume1[i], digits);
                Assert.AreEqual(u, d);
                Assert.AreEqual(dateTime, scalar.Time);
            }
            target = new MoneyFlowIndex();
            for (int i = 0; i < 14; ++i)
            {
                DateTime dateTime = DateTime.Now;
                Scalar scalar = target.Update(new Scalar(dateTime, (inputHigh[i] + inputLow[i] + inputClose[i]) / 3));
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(scalar.Value));
                Assert.IsTrue(double.IsNaN(target.Value));
                Assert.AreEqual(dateTime, scalar.Time);
            }
            for (int i = 14; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                Scalar scalar = target.Update((inputHigh[i] + inputLow[i] + inputClose[i]) / 3, dateTime);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedMfiVolume1[i], digits);
                Assert.AreEqual(u, d);
                Assert.AreEqual(dateTime, scalar.Time);
            }
            target = new MoneyFlowIndex();
            for (int i = 0; i < 14; ++i)
            {
                double d = target.Update((inputHigh[i] + inputLow[i] + inputClose[i]) / 3);
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(d));
                Assert.IsTrue(double.IsNaN(target.Value));
            }
            for (int i = 14; i < count; ++i)
            {
                double d = target.Update((inputHigh[i] + inputLow[i] + inputClose[i]) / 3);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(d));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(d, target.Value);
                d = Math.Round(d, digits);
                double u = Math.Round(expectedMfiVolume1[i], digits);
                Assert.AreEqual(u, d);
            }
            double w = target.Update(double.NaN, 987);
            Assert.IsTrue(double.IsNaN(w));
            w = target.Update(789, double.NaN);
            Assert.IsTrue(double.IsNaN(w));
            w = target.Update(double.NaN, double.NaN);
            Assert.IsTrue(double.IsNaN(w));
            target = new MoneyFlowIndex(2);
            for (int i = 0; i < 10; ++i)
                w = target.Update(0.001, 0.5);
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
            int count = inputHigh.Count;
            var target = new MoneyFlowIndex();
            for (int i = 0; i < 14; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, double.NaN, inputHigh[i], inputLow[i], inputClose[i], inputVolume[i]);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(scalar.Value));
                Assert.IsTrue(double.IsNaN(target.Value));
                Assert.AreEqual(dateTime, scalar.Time);
            }
            for (int i = 14; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, double.NaN, inputHigh[i], inputLow[i], inputClose[i], inputVolume[i]);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedMfi[i], digits);
                Assert.AreEqual(u, d);
                Assert.AreEqual(dateTime, scalar.Time);
            }
            target.Reset();
            for (int i = 0; i < 14; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, double.NaN, inputHigh[i], inputLow[i], inputClose[i], inputVolume[i]);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsFalse(target.IsPrimed);
                Assert.IsTrue(double.IsNaN(scalar.Value));
                Assert.IsTrue(double.IsNaN(target.Value));
                Assert.AreEqual(dateTime, scalar.Time);
            }
            for (int i = 14; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, double.NaN, inputHigh[i], inputLow[i], inputClose[i], inputVolume[i]);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(target.Value, digits);
                double u = Math.Round(expectedMfi[i], digits);
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
            var target = new MoneyFlowIndex(5);
            Assert.AreEqual("[M:MFI(5) P:False V:NaN]", target.ToString());
            target = new MoneyFlowIndex(13);
            Assert.AreEqual("[M:MFI(13) P:False V:NaN]", target.ToString());
        }
        #endregion

        #region MoneyFlowIndexConstructorTest
        /// <summary>
        /// A test for MoneyFlowIndex Constructor.
        /// </summary>
        [TestMethod]
        public void MoneyFlowIndexConstructorTest()
        {
            var target = new MoneyFlowIndex();
            Assert.AreEqual(target.OhlcvComponent, OhlcvComponent.TypicalPrice);
            Assert.AreEqual(14, target.Length);
            Assert.IsTrue(double.IsNaN(target.Value));
            Assert.IsFalse(target.IsPrimed);
            target = new MoneyFlowIndex(321);
            Assert.AreEqual(target.OhlcvComponent, OhlcvComponent.TypicalPrice);
            Assert.AreEqual(321, target.Length);
            Assert.IsTrue(double.IsNaN(target.Value));
            Assert.IsFalse(target.IsPrimed);
            target = new MoneyFlowIndex(1, OhlcvComponent.LowPrice);
            Assert.AreEqual(target.OhlcvComponent, OhlcvComponent.LowPrice);
            Assert.AreEqual(1, target.Length);
            Assert.IsTrue(double.IsNaN(target.Value));
            Assert.IsFalse(target.IsPrimed);
        }

        /// <summary>
        /// A test for MoneyFlowIndex Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MoneyFlowIndexConstructorTest2()
        {
            var target = new MoneyFlowIndex(0);
            Assert.IsNotNull(target);
        }

        /// <summary>
        /// A test for MoneyFlowIndex Constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MoneyFlowIndexConstructorTest3()
        {
            var target = new MoneyFlowIndex(-8);
            Assert.IsNotNull(target);
        }
        #endregion

        #region SerializationTest
        private static void SerializeTo(MoneyFlowIndex instance, string fileName)
        {
            var dcs = new DataContractSerializer(typeof(MoneyFlowIndex),
                null, 65536, false, true, null);
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                dcs.WriteObject(fs, instance);
                fs.Close();
            }
        }

        private static MoneyFlowIndex DeserializeFrom(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
            var ser = new DataContractSerializer(typeof(MoneyFlowIndex),
                null, 65536, false, true, null);
            var instance = (MoneyFlowIndex)ser.ReadObject(reader, true);
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
            int count = inputHigh.Count;
            var source = new MoneyFlowIndex();
            for (int i = 0; i < 14; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, double.NaN, inputHigh[i], inputLow[i], inputClose[i], inputVolume[i]);
                Scalar scalar = source.Update(ohlcv);
                Assert.IsFalse(source.IsPrimed);
                Assert.IsTrue(double.IsNaN(scalar.Value));
                Assert.IsTrue(double.IsNaN(source.Value));
                Assert.AreEqual(dateTime, scalar.Time);
            }
            for (int i = 14; i < 99; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, double.NaN, inputHigh[i], inputLow[i], inputClose[i], inputVolume[i]);
                Scalar scalar = source.Update(ohlcv);
                Assert.IsTrue(source.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(source.Value));
                Assert.AreEqual(scalar.Value, source.Value);
                double d = Math.Round(source.Value, digits);
                double u = Math.Round(expectedMfi[i], digits);
                Assert.AreEqual(u, d);
                Assert.AreEqual(dateTime, scalar.Time);
            }
            const string fileName = "MoneyFlowIndexTest_1.xml";
            SerializeTo(source, fileName);
            MoneyFlowIndex target = DeserializeFrom(fileName);
            Assert.AreEqual(14, target.Length);
            Assert.IsTrue(target.IsPrimed);
            Assert.AreEqual("MFI", target.Name);
            Assert.AreEqual(source.Value, target.Value);
            Assert.AreEqual("Money Flow Index", target.Description);
            for (int i = 99; i < count; ++i)
            {
                DateTime dateTime = DateTime.Now;
                var ohlcv = new Ohlcv(dateTime, double.NaN, inputHigh[i], inputLow[i], inputClose[i], inputVolume[i]);
                Scalar scalar = target.Update(ohlcv);
                Assert.IsTrue(target.IsPrimed);
                Assert.IsFalse(double.IsNaN(scalar.Value));
                Assert.IsFalse(double.IsNaN(target.Value));
                Assert.AreEqual(scalar.Value, target.Value);
                double d = Math.Round(scalar.Value, digits);
                double u = Math.Round(expectedMfi[i], digits);
                Assert.AreEqual(u, d);
                Assert.AreEqual(dateTime, scalar.Time);
            }
            //FileInfo fi = new FileInfo(fileName);
            //fi.Delete();
        }
        #endregion
    }
}
