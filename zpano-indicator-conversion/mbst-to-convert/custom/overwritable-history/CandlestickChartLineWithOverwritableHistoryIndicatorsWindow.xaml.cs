using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Mbst;
using Mbst.Charts;
using Mbst.Controls;
using Mbst.Controls.ControlStyles;
using Mbst.Trading;

namespace Charts
{
    internal sealed partial class CandlestickChartLineWithOverwritableHistoryIndicatorsWindow
    {
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            // Apply aero effect to entire window.
            if (Properties.Settings.Default.Theme == ControlStyles.Glass)
                this.ExtendGlassFrame();
        }

        public CandlestickChartLineWithOverwritableHistoryIndicatorsWindow()
        {
            InitializeComponent();
        }

        private readonly OhlcvSample samples = new OhlcvSample(new DateTime(2010, 2, 3, 0, 0, 0), TimeGranularity.OneDay);
        private long total;
        private ICandlestickChartOhlcvSource source;
        private ICandlestickChartLineIndicator priceLineIndicator, volumeLineIndicator;

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            predefinedPriceLineStyleComboBox.SelectedIndex = (int)PredefinedLineStyle.ChocolateLineFill;
            predefinedVolumeLineStyleComboBox.SelectedIndex = (int)PredefinedLineStyle.MagentaLineFill;
            predefinedPanelLineStyleComboBox.SelectedIndex = (int)PredefinedLineStyle.RedLineFill;

            source = chart1.AddOhlcvSource("test", TimeGranularity.OneDay);

            priceLineIndicator = source.AddPriceLineIndicator(new HighLowLineWithOverwritableHistory(20, 10),
                (PredefinedLineStyle)predefinedPriceLineStyleComboBox.SelectedIndex);
            priceLineIndicator.PhaseShift = priceIndicatorPhaseShiftIntegerSpinner.Value;
            priceLineIndicator.DisplayMode = (LineDisplayMode)priceLineDisplayModeComboBox.SelectedItem;
            priceLineIndicator.SplineTension = priceIndicatorSplineTensionNumericSpinner.Value;
            priceLineIndicator.SplineTolerance = priceIndicatorSplineToleranceNumericSpinner.Value;

            volumeLineIndicator = source.AddVolumeLineIndicator(new HighLowLineWithOverwritableHistory(20, 10, true),
                (PredefinedLineStyle)predefinedVolumeLineStyleComboBox.SelectedIndex);
            volumeLineIndicator.PhaseShift = volumeIndicatorPhaseShiftIntegerSpinner.Value;
            volumeLineIndicator.DisplayMode = (LineDisplayMode)volumeLineDisplayModeComboBox.SelectedItem;
            volumeLineIndicator.SplineTension = volumeIndicatorSplineTensionNumericSpinner.Value;
            volumeLineIndicator.SplineTolerance = volumeIndicatorSplineToleranceNumericSpinner.Value;

            AddIndicatorPaneButtonClick(null, null);
            const int initialCount = 20;
            source.Add(samples.SampleList(initialCount));
            total = initialCount;
            totalTextBlock.Text = total.ToString();
        }

        private void AddSampleButtonClick(object sender, RoutedEventArgs e)
        {
            int count;
            if (!int.TryParse(addTextBox.Text, out count))
                count = 0;
            if (1 == count)
            {
                source.Add(samples.Sample());
                total++;
                totalTextBlock.Text = total.ToString();
            }
            else if (0 < count)
            {
                source.Add(samples.SampleList(count));
                total += count;
                totalTextBlock.Text = total.ToString();
            }
        }

        private int paneNumber;
        private int firstOffset = 20, secondOffset = 10;
        private readonly List<ICandlestickChartLineIndicator> panelIndicatorList = new List<ICandlestickChartLineIndicator>();

        private void AddIndicatorPaneButtonClick(object sender, RoutedEventArgs e)
        {
            chart1.AddIndicatorPane();

            var lineIndicator = source.AddLineIndicator(new HighLowLineWithOverwritableHistory(firstOffset, secondOffset),
                paneNumber, (PredefinedLineStyle)predefinedPanelLineStyleComboBox.SelectedIndex);
            lineIndicator.PhaseShift = panelIndicatorPhaseShiftIntegerSpinner.Value;
            lineIndicator.DisplayMode = (LineDisplayMode)panelLineDisplayModeComboBox.SelectedItem;
            lineIndicator.SplineTension = panelIndicatorSplineTensionNumericSpinner.Value;
            lineIndicator.SplineTolerance = panelIndicatorSplineToleranceNumericSpinner.Value;
            panelIndicatorList.Add(lineIndicator);

            ++paneNumber;
            firstOffset += 10;
            secondOffset += 5;
        }

        #region Phase shift
        private void PhaseShiftChanged(ICandlestickChartLineIndicator lineIndicator, IntegerSpinner integerSpinner)
        {
            if (IsInitialized && null != lineIndicator)
                lineIndicator.PhaseShift = integerSpinner.Value;
        }

        private void PriceIndicatorPhaseShiftChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            PhaseShiftChanged(priceLineIndicator, priceIndicatorPhaseShiftIntegerSpinner);
        }

        private void VolumeIndicatorPhaseShiftChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            PhaseShiftChanged(volumeLineIndicator, volumeIndicatorPhaseShiftIntegerSpinner);
        }

        private void PanelIndicatorPhaseShiftChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            foreach (var lineIndicator in panelIndicatorList)
                PhaseShiftChanged(lineIndicator, panelIndicatorPhaseShiftIntegerSpinner);
        }
        #endregion

        #region Predefined line style
        private void PredefinedLineStyleSelectionChanged(ICandlestickChartLineIndicator lineIndicator, ComboBox comboBox)
        {
            if (IsInitialized && null != lineIndicator)
                lineIndicator.PredefinedStyle = (PredefinedLineStyle)comboBox.SelectedIndex;
        }

        private void PricePredefinedLineStyleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PredefinedLineStyleSelectionChanged(priceLineIndicator, predefinedPriceLineStyleComboBox);
        }

        private void VolumePredefinedLineStyleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PredefinedLineStyleSelectionChanged(volumeLineIndicator, predefinedVolumeLineStyleComboBox);
        }

        private void PanelPredefinedLineStyleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var lineIndicator in panelIndicatorList)
                PredefinedLineStyleSelectionChanged(lineIndicator, predefinedPanelLineStyleComboBox);
        }
        #endregion

        #region Display mode change
        private void DisplayModeChanged(ICandlestickChartLineIndicator lineIndicator, ComboBox comboBox)
        {
            if (IsInitialized && null != lineIndicator)
                lineIndicator.DisplayMode = (LineDisplayMode)comboBox.SelectedItem;
        }

        private void PriceLineDisplayModeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayModeChanged(priceLineIndicator, priceLineDisplayModeComboBox);
        }

        private void VolumeLineDisplayModeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayModeChanged(volumeLineIndicator, volumeLineDisplayModeComboBox);
        }

        private void PanelLineDisplayModeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var lineIndicator in panelIndicatorList)
                DisplayModeChanged(lineIndicator, panelLineDisplayModeComboBox);
        }
        #endregion

        #region Spline tension
        private void UpdateSplineTension(ICandlestickChartLineIndicator lineIndicator, NumericSpinner numericSpinner)
        {
            if (IsInitialized && null != lineIndicator)
                lineIndicator.SplineTension = numericSpinner.Value;
        }

        private void PriceIndicatorSplineTensionChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateSplineTension(priceLineIndicator, priceIndicatorSplineTensionNumericSpinner);
        }

        private void VolumeIndicatorSplineTensionChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateSplineTension(volumeLineIndicator, volumeIndicatorSplineTensionNumericSpinner);
        }

        private void PanelIndicatorSplineTensionChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (var lineIndicator in panelIndicatorList)
                UpdateSplineTension(lineIndicator, panelIndicatorSplineTensionNumericSpinner);
        }
        #endregion

        #region Spline tolerance
        private void UpdateSplineTolerance(ICandlestickChartLineIndicator lineIndicator, NumericSpinner numericSpinner)
        {
            if (IsInitialized && null != lineIndicator)
                lineIndicator.SplineTolerance = numericSpinner.Value;
        }

        private void PriceIndicatorSplineToleranceChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateSplineTolerance(priceLineIndicator, priceIndicatorSplineToleranceNumericSpinner);
        }

        private void VolumeIndicatorSplineToleranceChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            UpdateSplineTolerance(volumeLineIndicator, volumeIndicatorSplineToleranceNumericSpinner);
        }

        private void PanelIndicatorSplineToleranceChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            foreach (var lineIndicator in panelIndicatorList)
                UpdateSplineTolerance(lineIndicator, panelIndicatorSplineToleranceNumericSpinner);
        }
        #endregion

        private void StyleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                string selection = e.AddedItems[0].ToString();
                chart1.ApplyStyle(selection);
            }
        }
    }
}
