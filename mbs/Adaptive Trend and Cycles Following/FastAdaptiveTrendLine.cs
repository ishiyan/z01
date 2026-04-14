using System.Runtime.Serialization;

namespace Mbst.Trading.Indicators
{
    /// <summary>
    /// Computes the FastAdaptiveTrendLine.
    /// </summary>
    [DataContract]
    public sealed class FastAdaptiveTrendLine : FiniteImpulseResponceFilter
    {
        #region Members and accessors
        private static readonly double[] coefficients =
        {
            // The original values, total sum = 0.9999999999 ≠ 1.
            // 0.0040364019, 0.0130129076, 0.0007860160, 0.0005541115,-0.0047717710,
            //-0.0072003400,-0.0067093714,-0.0023824623, 0.0040444064, 0.0095711419,
            // 0.0110573605, 0.0069480557,-0.0016060704,-0.0108597376,-0.0160483392,
            //-0.0136744850,-0.0036771622, 0.0100299086, 0.0208778257, 0.0226522218,
            // 0.0128149838,-0.0055774838,-0.0244141482,-0.0338917071,-0.0272432537,
            //-0.0047706151, 0.0249252327, 0.0477818607, 0.0502044896, 0.0259609206,
            //-0.0190795053,-0.0670110374,-0.0933058722,-0.0760367731,-0.0054034585,
            // 0.1104506886, 0.2460452079, 0.3658689069, 0.4360409450

            // The normalized values, total sum = 1.
            0.0040364019004036386962421862, 0.0130129076013012957968308448, 0.000786016000078601746116832,  0.0005541115000554108210219855,-0.0047717710004771784587179668,
            -0.0072003400007200276742901798,-0.0067093714006709378328730376,-0.002382462300238249230464677,  0.0040444064004044386936567327, 0.009571141900957106908521166,
            0.0110573605011056964284725581, 0.0069480557006948077557780087,-0.0016060704001606094812392607,-0.0108597376010859964923047548,-0.0160483392016047948163864379,
            -0.0136744850013673955831413446,-0.0036771622003677188122766093, 0.0100299086010029967603395219, 0.0208778257020877932564622982, 0.0226522218022651926833323579,
            0.0128149838012814958607602322,-0.0055774838005577481984727324,-0.0244141482024413921142301306,-0.0338917071033891890529786056,-0.027243253702724291200429054,
            -0.0047706151004770584590913225, 0.0249252327024924919491498371, 0.0477818607047781845664589924, 0.0502044896050203837839498576, 0.0259609206025960916146226454,
            -0.0190795053019079938373197875,-0.0670110374067010783554349176,-0.0933058722093305698622032764,-0.0760367731076036754401222862,-0.0054034585005403482546829043,
            0.1104506886110449643244275786, 0.2460452079246049205273978404, 0.3658689069365868818243430595, 0.4360409450436038591587747509
        };

        private const string fatl = "FATL";
        #endregion

        #region Construction
        /// <summary>
        /// Constructs a new instance of the <see cref="FastAdaptiveTrendLine"/> class.
        /// </summary>
        public FastAdaptiveTrendLine()
            : this(OhlcvComponent.ClosingPrice)
        {
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="FastAdaptiveTrendLine"/> class.
        /// </summary>
        /// <param name="ohlcvComponent">The Ohlcv component.</param>
        public FastAdaptiveTrendLine(OhlcvComponent ohlcvComponent)
            : base(fatl, fatl, "Fast Adaptive Trend Line", coefficients, ohlcvComponent)
        {
            //double[] dblCoeff;
            //double dblTotal, dblTotalOld = Normalize(out dblCoeff, out dblTotal);
            //decimal[] dcmCoeff;
            //decimal dcmTotal, dcmTotalOld = Normalize(out dcmCoeff, out dcmTotal);
            //bool b1 = 1d == dblTotal;
            //bool b2 = decimal.One == dcmTotal;
        }
        #endregion
    }
}
