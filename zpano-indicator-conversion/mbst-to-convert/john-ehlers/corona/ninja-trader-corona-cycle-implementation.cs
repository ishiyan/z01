#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class CoronaCycle : Indicator
	{
		//Done;
		
		private int Color1;
		private int Color2;
		private int Color3;
		private	int LineR=255;
		private	int LineG =255;
		private	int LineB = 0;
		private		int FuzzR=255;
		private	int	FuzzG=0;
		private	int	FuzzB=0;

		     private bool _showDC=true;
		private  Series<double>  _domCyc;
        private  Series<double> _domCycMdn;
        private  Series<double> _smoothHp;
        private  Series<double> HP;
        private  Series<double> HL;
        private  Series<double> _signal;
        private  Series<double> _noise;
        private  Series<double> _avg;

        private SolidColorBrush[] _colors;
        private Dictionary<int, FilterBank[]> _filters;
        private FilterBank[] bank
        {
            get { return _filters[CurrentBar]; }
        }

		
		
		
		
		protected override void OnStateChange()
		{
			if (State == State.SetDefaults)
			{
				Description									= @"Enter the description for your new custom Indicator here.";
				Name										= "CoronaCycle";
				Calculate									= Calculate.OnBarClose;
				IsOverlay									= false;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= false;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				
				
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
			
					AddPlot(new SolidColorBrush( Color.FromArgb((byte)150, (byte)255, (byte)255, (byte)0)),"dominant cycle" );	
				//AddPlot(Brushes.LightCyan,"SNR" );		
				Plots[0].Width = 3;

					
				for (int i = 1; i <= 60; i++)
				{
					string PlotName;
					if(i < 10) 
						PlotName	= "Plot0" + i.ToString();
					else
						PlotName	= "Plot" + i.ToString();
					
					AddPlot(Brushes.Black, PlotName);

				}
					AddPlot(new SolidColorBrush( Color.FromArgb((byte)150, (byte)255, (byte)255, (byte)0)),"Plot62" );	
				
			}
			else if (State == State.Configure)
			{
				Color1=0;
				Color2=0;
				Color3 =0;
				_filters = new Dictionary<int, FilterBank[]>();
				
				
				
			}
			else if (State == State.DataLoaded)
			{
			
			 
				
				
				
				_domCyc = new  Series<double> (this);
	            HP = new  Series<double> (this);
	            _smoothHp = new  Series<double> (this);
	            _domCycMdn = new  Series<double> (this);
	            HL = new  Series<double> (this);
	            _signal = new  Series<double> (this);
	            _noise = new  Series<double> (this);
	            _avg = new  Series<double> (this);
			}
			else if (State == State.Terminated)
			{

			}
		}

		protected override void OnBarUpdate()
		{
		 if (CurrentBar < 12) return;
	
            double alpha = (1 - Math.Sin(twoPi / 30)) / Math.Cos(twoPi / 30);

            if (CurrentBar == 12)
            {           
                FilterBank[] b = new FilterBank[60];
                for (int n = 1; n < 60; n++)
                    b[n] = new FilterBank();
                _filters[12] = b;

                for(int bar = 1; bar < CurrentBar; bar++)
                {
                    HP[bar] = 0.5 * (1 + alpha)* Momentum(Input, 1)[bar] + alpha * HP[bar+1];
//                    HL[bar] = High[bar]-Low[bar];
                }
            }
            else
            {
//                HL[0] = High[0] - Low[0];
                HP[0] = 0.5 * (1 + alpha)* Momentum(Input, 1)[0] + alpha * HP[1];
                FilterBank[] b = new FilterBank[60];
                for (int n = 1; n < 60; n++)
                    b[n] = (FilterBank)_filters[CurrentBar - 1][n].Clone();
                _filters[CurrentBar] = b;
            }
				          
            _smoothHp[0] = (HP[0] + 2 * HP[1] + 3 * HP[2] + 3 * HP[3] + 2 * HP[4] + HP[5]) / 12;


            double maxAmpl = 0d;
            double delta = -0.015 * CurrentBar + 0.5;
            delta = delta < 0.1 ? 0.1 : delta;
			

            for (int n = 11; n < 60; n++)
            {
                double beta = Math.Cos(4*Math.PI / (n+1));
                double gamma = 1 / Math.Cos(8*Math.PI * delta / (n+1));
                double a = gamma - Math.Sqrt(gamma * gamma - 1);
                bank[n].Q[0] = (_smoothHp[0] - _smoothHp[1]) * ((n+1)/4/Math.PI);
                bank[n].I[0]= _smoothHp[0];
                bank[n].R[0]= 0.5 * (1 - a) * (bank[n].I[0]- bank[n].I[2]) + beta * (1 + a) * bank[n].R[1] - a * bank[n].R[2];
                bank[n].Im[0] = 0.5 * (1 - a) * (bank[n].Q[0] - bank[n].Q[2]) + beta * (1 + a) * bank[n].Im[1] - a * bank[n].Im[2];
                bank[n].Amplitude = bank[n].R[0]* bank[n].R[0]+ bank[n].Im[0] * bank[n].Im[0];

                maxAmpl = bank[n].Amplitude > maxAmpl ? bank[n].Amplitude : maxAmpl;
            }
			

            double num = 0; double den = 0;
            for (int n = 11; n < 60; n++)
            {
                bank[n].I[2] = bank[n].I[1];
                bank[n].I[1] = bank[n].I[0];
                bank[n].Q[2] = bank[n].Q[1];
                bank[n].Q[1] = bank[n].Q[0];
                bank[n].R[2] = bank[n].R[1];
                bank[n].R[1] = bank[n].R[0];
                bank[n].Im[2] = bank[n].Im[1];
                bank[n].Im[1] = bank[n].Im[0];
                bank[n].dB[1] = bank[n].dB[0];

                if (maxAmpl != 0 && bank[n].Amplitude / maxAmpl > 0){
                    bank[n].dB[0] = -10 * Math.Log(.01 / (1 - 0.99 * bank[n].Amplitude / maxAmpl)) / Math.Log(10);
                }

                bank[n].dB[0] = 0.33 * bank[n].dB[0] + 0.67 * bank[n].dB[1];
                bank[n].dB[0] = bank[n].dB[0] > 20 ? 20 : bank[n].dB[0];

                if (bank[n].dB[0] <= 6)
                {
                    num += n * (20 - bank[n].dB[0]);
                    den += (20 - bank[n].dB[0]);
                }
                if (den != 0) _domCyc[0]=(0.5 * num / den);
            }


            _domCycMdn[0] = GetMedian(_domCyc, 5);
            _domCycMdn[0] = _domCyc[0] < 6 ? 6 : _domCyc[0];
///Swing Indicator starts differing here.
			/// 
			
//				if (_showDC)				  
//                Values[0][0]=(_domCyc[0]);
//            else
//                Values[0][0]=(_domCycMdn[0]);
				
//					if (_showDC)				  
//                Values[60][0]=(_domCyc[0]);
//            else
//                Values[60][0]=(_domCycMdn[0]);
			  
//			  	PlotBrushes[60][0] = new SolidColorBrush( Color.FromArgb((byte)150, (byte)255, (byte)255, (byte)0));
					
					
						if (_showDC)				  
                Values[0][0]=(_domCyc[0]);
            else
                Values[0][0]=(_domCycMdn[0]);
			  
			  	PlotBrushes[0][0] = new SolidColorBrush( Color.FromArgb((byte)150, (byte)255, (byte)255, (byte)0));	
					  	Plots[0].Width=5;
			
			 for (int n = 11; n < 60; n++)
            { 
				
		
				if(bank[n].dB[0] <= 10)
				{
					
					Color1 = (int)(LineR + bank[n].dB[0]*((FuzzR - LineR)/10));
					Color2 = (int)( LineG + bank[n].dB[0]*((FuzzG - LineG)/10));
					Color3 = (int)( LineB + bank[n].dB[0]*((FuzzB-LineB)/10));
					

					
						
				}
				if(bank[n].dB[0] > 10)
				{
					
					Color1 = (int)(FuzzR*(2-bank[n].dB[0]) /10);
				Color2 = (int)(FuzzG*(2-bank[n].dB[0]) /10);
			Color3 = (int)(FuzzB*(2-bank[n].dB[0]) /10);						
				}
//				if(n==1)
//				{
//					Values[n][0] = n;
//									Plots[n].Width =3;
					
//				}

				if(n >= 12 )
					Values[n][0] = n/2;
//			//	if(n==3)
				//	Values[n][0] = .2*n+1;
				
					PlotBrushes[n][0] = new SolidColorBrush( Color.FromArgb((byte)Color1, (byte)Color1, (byte)Color2, (byte)Color3));
					Plots[n].PlotStyle = PlotStyle.Square;
							Plots[n].Width =5;
				
		
			
			
//						Values[n-11][0] 	= n/2;
//				PlotBrushes[n-11][0] = new SolidColorBrush( Color.FromArgb((byte)255, (byte)Color1, (byte)Color2, (byte)Color3));
//				Plots[n-11].PlotStyle = PlotStyle.Dot;
//				Plots[n-11].Width = 3;
			
			}
			
			if (_showDC)				  
                Values[61][0]=(_domCyc[0]);
            else
                Values[61][0]=(_domCycMdn[0]);
			  
			  	PlotBrushes[61][0] = new SolidColorBrush( Color.FromArgb((byte)150, (byte)255, (byte)255, (byte)0));	
					  	Plots[61].Width=3;
	
				//	Values[61][0] = 30;
//			PlotBrushes[60][0] = new SolidColorBrush( Color.FromArgb((byte)Color1, (byte)Color1, (byte)Color2, (byte)Color3));
//			Plots[60].Width =5;
//				Values[60][0] = 30;

//        	PlotBrushes[60][0] = Brushes.Black;
//						Plots[60].PlotStyle = PlotStyle.Dot;
//		
			
			
			

		}
		
	public override void OnCalculateMinMax()
{
  // make sure to always start fresh values to calculate new min/max values
  
 
  // Finally, set the minimum and maximum Y-Axis values to +/- 50 ticks from the primary close value
  MinValue =6;
  MaxValue = 30;
}
		  
        public const double twoPi = 2 * Math.PI;
        public const double fourPi = 4 * Math.PI;
		
        protected class FilterBank : ICloneable
        {	// current, old, older
            internal double[] I = new double[3];  
            internal double[] Q = new double [3];
            internal double[] R = new double [3];
            internal double[] Im = new double[3];
            internal double Amplitude = 0;
            internal double[] dB = new double[2];
            internal double[] Raster = new double[2];

            public object Clone()
            {
                FilterBank clone = new FilterBank();

                I.CopyTo(clone.I, 0);
                Q.CopyTo(clone.Q, 0);
                R.CopyTo(clone.R, 0);
                Im.CopyTo(clone.Im, 0);
                clone.Amplitude = Amplitude;
                dB.CopyTo(clone.dB, 0);
                Raster.CopyTo(clone.Raster, 0);

                return clone;
            }
        }
   
		
		
			[Browsable(false)]
		[XmlIgnore]
		public Series<double> DominantCycleMedian
		{
			get { return Values[0]; }
		}
		
		
	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private CoronaCycle[] cacheCoronaCycle;
		public CoronaCycle CoronaCycle()
		{
			return CoronaCycle(Input);
		}

		public CoronaCycle CoronaCycle(ISeries<double> input)
		{
			if (cacheCoronaCycle != null)
				for (int idx = 0; idx < cacheCoronaCycle.Length; idx++)
					if (cacheCoronaCycle[idx] != null &&  cacheCoronaCycle[idx].EqualsInput(input))
						return cacheCoronaCycle[idx];
			return CacheIndicator<CoronaCycle>(new CoronaCycle(), input, ref cacheCoronaCycle);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.CoronaCycle CoronaCycle()
		{
			return indicator.CoronaCycle(Input);
		}

		public Indicators.CoronaCycle CoronaCycle(ISeries<double> input )
		{
			return indicator.CoronaCycle(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.CoronaCycle CoronaCycle()
		{
			return indicator.CoronaCycle(Input);
		}

		public Indicators.CoronaCycle CoronaCycle(ISeries<double> input )
		{
			return indicator.CoronaCycle(input);
		}
	}
}

#endregion
