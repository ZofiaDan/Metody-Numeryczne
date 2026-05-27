using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace Rozniczkowanie
{
    public partial class Form1 : Form
    {
        private const int MaxSimulationSteps = 2000000;
        private const int MaxPlotPoints = 50000;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResetDefaults();
            ConfigureChart();
            SolveAndPlot();
        }

        private void buttonSolve_Click(object sender, EventArgs e)
        {
            SolveAndPlot();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            ResetDefaults();
            SolveAndPlot();
        }

        private void ResetDefaults()
        {
            textBoxMu.Text = "1.0";
            textBoxT0.Text = "0.0";
            textBoxTEnd.Text = "20.0";
            textBoxDt.Text = "0.01";
            textBoxX0.Text = "2.0";
            textBoxV0.Text = "0.0";
        }

        private void ConfigureChart()
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();

            chart1.ChartAreas[0].AxisX.Title = "t";
            chart1.ChartAreas[0].AxisY.Title = "value";
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;

            var xSeries = chart1.Series.Add("x(t)");
            xSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            xSeries.BorderWidth = 2;

            var vSeries = chart1.Series.Add("v(t)");
            vSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            vSeries.BorderWidth = 2;
        }

        private void SolveAndPlot()
        {
            if (!TryReadInputs(out var mu, out var t0, out var tEnd, out var dt, out var x0, out var v0))
            {
                return;
            }

            var rawSteps = (tEnd - t0) / dt;
            var approxSteps = (int)Math.Ceiling(rawSteps) + 1;
            if (approxSteps > MaxSimulationSteps)
            {
                MessageBox.Show(
                    "Too many integration steps. Increase dt or reduce tEnd - t0.",
                    "Input validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            List<VanDerPolPoint> points = VanDerPolSolver.SolveRk4(t0, tEnd, dt, x0, v0, mu);
            int stride = Math.Max(1, (int)Math.Ceiling((double)points.Count / MaxPlotPoints));

            chart1.Series["x(t)"].Points.Clear();
            chart1.Series["v(t)"].Points.Clear();

            for (int i = 0; i < points.Count; i += stride)
            {
                VanDerPolPoint p = points[i];
                chart1.Series["x(t)"].Points.AddXY(p.T, p.X);
                chart1.Series["v(t)"].Points.AddXY(p.T, p.V);
            }

            chart1.Titles.Clear();
            chart1.Titles.Add(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "Van der Pol Oscillator (mu={0}, dt={1}, plotted points={2})",
                    mu,
                    dt,
                    chart1.Series["x(t)"].Points.Count));
        }

        private bool TryReadInputs(
            out double mu,
            out double t0,
            out double tEnd,
            out double dt,
            out double x0,
            out double v0)
        {
            mu = t0 = tEnd = dt = x0 = v0 = 0.0;

            if (!TryParseBox(textBoxMu, out mu, "mu")) return false;
            if (!TryParseBox(textBoxT0, out t0, "t0")) return false;
            if (!TryParseBox(textBoxTEnd, out tEnd, "tEnd")) return false;
            if (!TryParseBox(textBoxDt, out dt, "dt")) return false;
            if (!TryParseBox(textBoxX0, out x0, "x0")) return false;
            if (!TryParseBox(textBoxV0, out v0, "v0")) return false;

            if (dt <= 0.0)
            {
                MessageBox.Show("dt must be greater than 0.", "Input validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (tEnd <= t0)
            {
                MessageBox.Show("tEnd must be greater than t0.", "Input validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private static bool TryParseBox(TextBox box, out double value, string name)
        {
            bool ok = double.TryParse(box.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out value)
                || double.TryParse(box.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out value);

            if (!ok)
            {
                MessageBox.Show(
                    string.Format(CultureInfo.InvariantCulture, "Invalid value for {0}.", name),
                    "Input validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                box.Focus();
                box.SelectAll();
            }

            return ok;
        }
    }
}
