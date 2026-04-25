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


using System.Collections;
using System.Collections.Generic;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators
{
	public class CoronaTrendVigor : Indicator
	{
		
	
		ArrayList leadList = new ArrayList(51);	
		ArrayList posList = new ArrayList(21);		  
		private  Series<double>  _bandPass;		
		private int Color1;
		private int Color2;
		private int Color3;
		private	int LineR=64;
		private	int LineG =128;
		private	int LineB = 255;
		private		int FuzzR=0;
		private	int	FuzzG=0;
		private	int	FuzzB=255;		
		private  Series<double>  _domCyc;
        private  Series<double> _domCycMdn;
        private  Series<double> _smoothHp;
        private  Series<double> HP;			
		private  Series<double> _ratio;
        private  Series<double> _signal;
        private  Series<double> _noise;
        private  Series<double> _avg;
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
				Name										= "CoronaTrendVigor";
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

				AddPlot(new SolidColorBrush( Color.FromArgb((byte)150, (byte)60, (byte)120, (byte)255)),"vigor" );	

				Plots[0].Width = 3;
				
				for (int i = 1; i <= 50; i++)
				{
					string PlotName;
					if(i < 10) 
						PlotName	= "Plot0" + i.ToString();
					else
						PlotName	= "Plot" + i.ToString();
					
					AddPlot(Brushes.Black, PlotName);
				}

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

				_bandPass = new  Series<double> (this);
				_domCyc = new  Series<double> (this);
	            HP = new  Series<double> (this);
	            _smoothHp = new  Series<double> (this);
	            _domCycMdn = new  Series<double> (this);
	            _signal = new  Series<double> (this);
	            _noise = new  Series<double> (this);
	            _avg = new  Series<double> (this);
				_ratio = new  Series<double> (this);
				
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
                //    HL[bar] = High[bar]-Low[bar];
                }
            }
            else
            {
               // HL[0] = High[0] - Low[0];
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

		
			double delta2 = .1;
            double beta2 = Math.Cos(twoPi / _domCycMdn[0]);
            double g2 = 1 / Math.Cos(fourPi * delta2 / _domCycMdn[0]);
            double alpha2 = g2 - Math.Sqrt(g2 * g2 - 1);
            _bandPass[0] = 0.5 * (1 - alpha2) * (Input[0] - Input[2])
                           + beta2 * (1 + alpha2) * _bandPass[1] - alpha2 * _bandPass[2];
            double Q2 = (_domCycMdn[0] / twoPi) * (_bandPass[0] - _bandPass[1]);
           
			 //Pythagorean theorem to establish cycle amplitude
            double Ampl2 = Math.Sqrt(_bandPass[0] * _bandPass[0] + Q2 * Q2);
			
            int cycPeriod = (int)(_domCycMdn[0] - 1);
            if( cycPeriod < 12 ) cycPeriod = 12;
            double Trend = Input[0] - Input[Math.Min(cycPeriod, CurrentBar)];
            if( Trend != 0 && Ampl2 != 0 ) 
                _ratio[0] = 0.33 * Trend /Ampl2 + 0.67 * _ratio[1];
            if( _ratio[0] > 10 ) _ratio[0] = 10d;
            if( _ratio[0] < -10 ) _ratio[0] = -10d;

            double tv = 0.05 * (_ratio[0] + 10d);
            double widthTV = 0;
            if (tv < 0.3 || tv > 0.7) widthTV = 0.01;
            if (tv >= 0.3 && tv < 0.5) widthTV = tv - 0.3;
            if (tv >= 0.5 && tv <= 0.7) widthTV = 0.7 - tv;
			
			
			int tv50 = (int)Math.Round(50 * tv);
			
			for (int n = 1; n < 51; n++)
            {
                bank[n].Raster[1] = bank[n].Raster[0]; 
                bank[n].Raster[0] = 20d;
                if (n < tv50) 
                    bank[n].Raster[0] = 0.8 * (Math.Pow((20 * tv - 0.4 * n) / widthTV, 0.85) + 0.2 * bank[n].Raster[1]);
             else if( n > tv50 ) 
                    bank[n].Raster[0] = 0.8 * (Math.Pow((-20 * tv + 0.4 * n) / widthTV, 0.85) + 0.2 * bank[n].Raster[1]);
                else if (n == tv50)
                    bank[n].Raster[0] = 0.5 * bank[n].Raster[1];
				
				if( bank[n].Raster[0] < 0 ) bank[n].Raster[0] = 0;
				              
				if( bank[n].Raster[0] > 20 || tv < 0.3 || tv > 0.7 ) bank[n].Raster[0] = 20;

            }
			
				Values[0][0]= (20d * tv - 10d);
			

		 	for( int n = 1; n < 51; n++ )
            { 
				
	
				if(bank[n].Raster[0] <= 10)
				{
					Color1 = (int)(LineR + bank[n].Raster[0]*(FuzzR - LineR)/10);
					Color2 = (int)( LineG + bank[n].Raster[0]*(FuzzG - LineG)/10);
					Color3 = (int)( LineB + bank[n].Raster[0]*(FuzzB-LineB)/10);
				}
				if(bank[n].Raster[0] > 10)
				{
					
					Color1 = (int)(FuzzR*(2-bank[n].Raster[0]) /10);
					Color2 = (int)(FuzzG*(2-bank[n].Raster[0]) /10);
					
					
					Color3 = (int)(FuzzB*(2-bank[n].Raster[0]) /10);				
				}
				
				if(n==1)
				{
					Values[n][0] = n;
					Plots[n].Width =5;
					PlotBrushes[n][0] = new SolidColorBrush( Color.FromArgb((byte)0, (byte)Color1, (byte)Color2, (byte)Color3));
				}
				if(n >= 2 )
				{
					Values[n][0] = .4*n-10;
							
					Plots[n].Width =5;
				
					PlotBrushes[n][0] = new SolidColorBrush( Color.FromArgb((byte)Color3, (byte)Color1, (byte)Color2, (byte)Color3));
				}
					

			}
      

			
		}
		
	public override void OnCalculateMinMax()
	{
	
	  MinValue =-10;
	  MaxValue = 10;
	}
	protected override void OnRender(ChartControl chartControl, ChartScale chartScale)
	{
		base.OnRender(chartControl, chartScale);  //If you comment this out, and uncomment line 247, you'll see the SMA plotted ontop of the ellipse.
		///Because the SMA plot will be set after the elispse is drawn/filled in.

		// 1.1 - SharpDX Vectors and Charting RenderTarget Coordinates

		// The SharpDX SDK uses "Vector2" objects to describe a two-dimensional point of a device (X and Y coordinates)
		SharpDX.Vector2 startPoint;
		SharpDX.Vector2 endPoint;
		// However, those concepts will not be discussed or used in this sample
		// Notes:  RenderTarget is always the full ChartPanel, so we need to be mindful which sub-ChartPanel we're dealing with
		// Always use ChartPanel X, Y, W, H - as chartScale and chartControl properties WPF units, so they can be drastically different depending on DPI set
		startPoint = new SharpDX.Vector2(ChartPanel.X, ChartPanel.Y);
		endPoint = new SharpDX.Vector2(ChartPanel.X + ChartPanel.W, ChartPanel.Y + ChartPanel.H);

	
		SharpDX.Direct2D1.SolidColorBrush customDXBrush = new SharpDX.Direct2D1.SolidColorBrush(RenderTarget,SharpDX.Color.Black);
	
		customDXBrush.Opacity = .45f;
		// These Vector2 objects are equivalent with WPF System.Windows.Point and can be used interchangeably depending on your requirements
//		// For convenience, NinjaTrader provides a "ToVector2()" extension method to convert from WPF Points to SharpDX.Vector2
//		SharpDX.Vector2 startPoint1 = new System.Windows.Point(ChartPanel.X, ChartPanel.Y + ChartPanel.H).ToVector2();
//		SharpDX.Vector2 endPoint1 = new System.Windows.Point(ChartPanel.X + ChartPanel.W, ChartPanel.Y).ToVector2();

		// SharpDX.Vector2 objects contain X/Y properties which are helpful to recalculate new properties based on the initial vector
		float width = endPoint.X - startPoint.X;
		float height = endPoint.Y - startPoint.Y;
		
		SharpDX.Vector2 center = (startPoint + endPoint) / 2;
		
		// SharpDX namespace consists of several shapes you can use to draw objects more complicated than lines
		// For example, we can use the RectangleF object to draw a rectangle that covers the entire chart area
		SharpDX.RectangleF rect = new SharpDX.RectangleF(startPoint.X, startPoint.Y, width, height);

		// The RenderTarget consists of two commands related to Rectangles.
		// The FillRectangle() method is used to "Paint" the area of a Rectangle
		RenderTarget.FillRectangle(rect, customDXBrush);

	}


		// For our custom script, we need a
		      // Keep cntMax fifo samples and find the Highest and Lowest lead for samples in the list
        private void PhaseList(ref ArrayList list, int cntMax, double lead, out double H, out double L)
        {
            H = lead; L = lead;
            if (list.Count < cntMax)
                list.Add(lead);
            else
            {
                list.RemoveAt(0);
                list.Add(lead);
            }
            for (int n = 0; n < list.Count - 1; n++)
            {
                double val = (double)list[n];
                if (val > H) H = val;
                if (val < L) L = val;
            }
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
		public Series<double> SwingPsn
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
		private CoronaTrendVigor[] cacheCoronaTrendVigor;
		public CoronaTrendVigor CoronaTrendVigor()
		{
			return CoronaTrendVigor(Input);
		}

		public CoronaTrendVigor CoronaTrendVigor(ISeries<double> input)
		{
			if (cacheCoronaTrendVigor != null)
				for (int idx = 0; idx < cacheCoronaTrendVigor.Length; idx++)
					if (cacheCoronaTrendVigor[idx] != null &&  cacheCoronaTrendVigor[idx].EqualsInput(input))
						return cacheCoronaTrendVigor[idx];
			return CacheIndicator<CoronaTrendVigor>(new CoronaTrendVigor(), input, ref cacheCoronaTrendVigor);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.CoronaTrendVigor CoronaTrendVigor()
		{
			return indicator.CoronaTrendVigor(Input);
		}

		public Indicators.CoronaTrendVigor CoronaTrendVigor(ISeries<double> input )
		{
			return indicator.CoronaTrendVigor(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.CoronaTrendVigor CoronaTrendVigor()
		{
			return indicator.CoronaTrendVigor(Input);
		}

		public Indicators.CoronaTrendVigor CoronaTrendVigor(ISeries<double> input )
		{
			return indicator.CoronaTrendVigor(input);
		}
	}
}

#endregion
