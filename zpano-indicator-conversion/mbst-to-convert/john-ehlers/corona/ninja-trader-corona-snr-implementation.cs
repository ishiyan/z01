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
	public class CoronaSNR : Indicator
	{
		//Done;
		
		private int Color1;
		private int Color2;
		private int Color3;
		private	int LineR=220;
		private	int LineG =255;
		private	int LineB = 255;
		private		int FuzzR=0;
		private	int	FuzzG=190;
		private	int	FuzzB=190;

		
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
				Name										= "CoronaSNR";
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
			

				AddPlot(Brushes.LightCyan,"SNR" );		
				Plots[0].Width = 1;

					
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
                    HL[bar] = High[bar]-Low[bar];
                }
            }
            else
            {
                HL[0] = High[0] - Low[0];
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

            double snr = 0;
            _avg[0] = 0.1 * Input[0] + 0.9 * _avg[1];
            if (_avg[0] != 0 && maxAmpl > 0)
                _signal[0] = 0.2 * Math.Sqrt(maxAmpl) + 0.9 * _signal[1];
            else 
                _signal[0] = _signal[1];
            if (_avg[0] != 0d)
                _noise[0] = 0.1 * GetMedian(HL, 5) + 0.9*_noise[1];
            if (_signal[0] != 0d || _noise[0] != 0d)
                snr = 20 * Math.Log10(_signal[0] / _noise[0]) + 3.5;
            snr = snr < 1d ? 0d : snr;
            snr = snr > 10d ? 10d : snr;
            snr = snr * 0.1;

            Value[0]=(snr * 10 + 1);

            double cWidth = snr > 0.5 ? 0d : -0.4 * snr + 0.2;
            int snr50 = (int)Math.Round(50 * snr);
            for( int n = 1; n < 51; n++ )
            { 
                bank[n].Raster[1] = bank[n].Raster[0];
                bank[n].Raster[0] = 20d;					
                if( n < snr50 ) 
                    bank[n].Raster[0] = 0.5 * (Math.Pow((20 * snr - 0.4 * n) / cWidth, 0.8) + bank[n].Raster[1]);
                else if( n > snr50 && (0.4 * n - 20 * snr) / cWidth > 1 )  
                    bank[n].Raster[0] = 0.5 * (Math.Pow((-20 * snr + 0.4 * n) / cWidth, 0.8) + bank[n].Raster[1]);
                else if( n == snr50 )
                    bank[n].Raster[0] = 0.5 * bank[n].Raster[1];
                if ( bank[n].Raster[0] > 20 ) bank[n].Raster[0] = 20;
                else if ( bank[n].Raster[0] < 0 ) bank[n].Raster[0] = 0;
                if ( snr > 0.5 ) bank[n].Raster[0] = 20;
            }

            
			
			
			
		 	for( int n = 1; n < 51; n++ )
            { 
				
					Values[n][0] = 10*snr+1;
				PlotBrushes[n][0] = new SolidColorBrush(Color.FromArgb((byte)255, (byte)LineB, (byte)LineG, (byte)LineB));
				
				if(bank[n].Raster[0] <= 10)
				{
					Color1 = (int)(LineR + bank[n].Raster[0]*((FuzzR - LineR)/10));
					Color2 = (int)( LineG + bank[n].Raster[0]*((FuzzG - LineG)/10));
					Color3 = (int)( LineB + bank[n].Raster[0]*((FuzzB-LineB)/10));
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
									Plots[n].Width =3;
					
				}
				if(n >= 2 )
					Values[n][0] = .2*n+1;
//			//	if(n==3)
				//	Values[n][0] = .2*n+1;
				
					
					
				Plots[n].Width =5;
//				PlotBrushes[n][0] = _colors[0];
				PlotBrushes[n][0] = new SolidColorBrush( Color.FromArgb((byte)Color1, (byte)Color1, (byte)Color2, (byte)Color3));
//				PlotBrushes[n][0] = new SolidColorBrush( Color.FromArgb((byte)255, (byte)Color1, (byte)Color2, (byte)Color3));
			}
		
			

        

		}
public override void OnCalculateMinMax()
{
  // make sure to always start fresh values to calculate new min/max values
  
 
  // Finally, set the minimum and maximum Y-Axis values to +/- 50 ticks from the primary close value
  MinValue =0;
  MaxValue = 12;
}
	protected override void OnRender(ChartControl chartControl, ChartScale chartScale)
	{
		base.OnRender(chartControl, chartScale);  ///If you comment this out, and uncomment line 247, you'll see the SMA plotted ontop of the ellipse.
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
//
//			// and DrawRectangle() is used to "Paint" the outline of a Rectangle
//			RenderTarget.DrawRectangle(rect, customDXBrush, 2);
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
		public Series<double> PlotName
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
		private CoronaSNR[] cacheCoronaSNR;
		public CoronaSNR CoronaSNR()
		{
			return CoronaSNR(Input);
		}

		public CoronaSNR CoronaSNR(ISeries<double> input)
		{
			if (cacheCoronaSNR != null)
				for (int idx = 0; idx < cacheCoronaSNR.Length; idx++)
					if (cacheCoronaSNR[idx] != null &&  cacheCoronaSNR[idx].EqualsInput(input))
						return cacheCoronaSNR[idx];
			return CacheIndicator<CoronaSNR>(new CoronaSNR(), input, ref cacheCoronaSNR);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.CoronaSNR CoronaSNR()
		{
			return indicator.CoronaSNR(Input);
		}

		public Indicators.CoronaSNR CoronaSNR(ISeries<double> input )
		{
			return indicator.CoronaSNR(input);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.CoronaSNR CoronaSNR()
		{
			return indicator.CoronaSNR(Input);
		}

		public Indicators.CoronaSNR CoronaSNR(ISeries<double> input )
		{
			return indicator.CoronaSNR(input);
		}
	}
}

#endregion
