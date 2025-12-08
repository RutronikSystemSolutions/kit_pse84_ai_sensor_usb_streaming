using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;

namespace ov7675.Views
{
    public partial class RawRadarSignalsView : UserControl
    {
        /// <summary>
        /// X Axis
        /// </summary>
        LinearAxis xAxis = new LinearAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            Position = AxisPosition.Bottom,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            PositionAtZeroCrossing = true,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Unit = "Sample index"
        };

        /// <summary>
        /// Y Axis for amplitude
        /// </summary>
        private LinearAxis yAxis = new LinearAxis
        {
            MajorGridlineStyle = LineStyle.Dot,
            AxislineStyle = LineStyle.Solid,
            AxislineColor = OxyColors.Gray,
            FontSize = 10,
            TextColor = OxyColors.Gray,
            Position = AxisPosition.Left,
            IsPanEnabled = false,
            IsZoomEnabled = true,
            Minimum = -1,
            Maximum = 1,
            Unit = "ADC tick",
            Key = "Amp",
        };

        private LineSeries timeSignalAntenna0LineSeries = new LineSeries();

        public RawRadarSignalsView()
        {
            InitializeComponent();
            InitPlot();
        }

        private void InitPlot()
        {
            // Raw signals plot
            var timeModel = new PlotModel
            {
                PlotType = PlotType.XY,
                PlotAreaBorderThickness = new OxyThickness(0),
            };

            // Set the axes
            timeModel.Axes.Add(xAxis);
            timeModel.Axes.Add(yAxis);

            // Add series
            timeSignalAntenna0LineSeries.Title = "Antenna 0";
            timeSignalAntenna0LineSeries.YAxisKey = yAxis.Key;

            timeModel.Series.Add(timeSignalAntenna0LineSeries);

            plotView.Model = timeModel;
            plotView.InvalidatePlot(true);
        }

        public void updateData(double[] signal)
        {
            timeSignalAntenna0LineSeries.Points.Clear();
            for (int i = 0; i < signal.Length; ++i)
            {
                timeSignalAntenna0LineSeries.Points.Add(new DataPoint(i, signal[i]));
            }
            plotView.InvalidatePlot(true);
        }
    }
}
