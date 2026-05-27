using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace CalkowanieTest
{
    public partial class Form1 : Form
    {
        private const double FresnelInfinityLimit = 0.5;
        private const double DefaultStartStep = 0.1;
        private const double MinSafeEps = 1e-8;
        private const double NearZeroThreshold = 1e-12;

        public Form1()
        {
            InitializeComponent();
        }

        private double FresnelCosIntegrand(double t)
        {
            return Math.Cos((Math.PI * t * t) / 2.0);
        }

        private double FresnelSinIntegrand(double t)
        {
            return Math.Sin((Math.PI * t * t) / 2.0);
        }

        private double Eps()
        {
            return Math.Pow(10.0, -(double)numericUpDown1.Value);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Tabela.RowCount = 2;
            Tabela.Rows[0].HeaderCell.Value = "C(x)";
            Tabela.Rows[1].HeaderCell.Value = "S(x)";
            comboBox1.SelectedIndex = 0;
            RenderCornuSpiral();
        }

        private double IntegrateBySelectedMethod(Func<double, double> fun, double a, double b, double eps)
        {
            var interval = Math.Abs(b - a);
            var baseSteps = Math.Max(80, (int)Math.Ceiling(interval / DefaultStartStep));
            var toleranceBoost = Math.Max(1.0, -Math.Log10(Math.Max(eps, MinSafeEps)));
            var steps = Math.Max(80, (int)(baseSteps * toleranceBoost * 8.0));
            if (steps % 2 != 0)
            {
                steps++;
            }

            switch (comboBox1.SelectedIndex)
            {
                case 1:
                    return IntegrateSimpson(fun, a, b, steps);
                case 2:
                    return IntegrateMidpointRectangle(fun, a, b, steps);
                default:
                    return IntegrateTrapezoid(fun, a, b, steps);
            }
        }

        private bool TryIntegrateBySelectedMethod(Func<double, double> fun, double a, double b, double eps, out double value)
        {
            value = 0.0;
            try
            {
                value = IntegrateBySelectedMethod(fun, a, b, eps);
                return IsFinite(value);
            }
            catch
            {
                return false;
            }
        }

        private double IntegrateTrapezoid(Func<double, double> fun, double a, double b, int n)
        {
            var h = (b - a) / n;
            var sum = 0.5 * (fun(a) + fun(b));
            for (var i = 1; i < n; i++)
            {
                sum += fun(a + i * h);
            }

            return sum * h;
        }

        private double IntegrateMidpointRectangle(Func<double, double> fun, double a, double b, int n)
        {
            var h = (b - a) / n;
            var sum = 0.0;
            for (var i = 0; i < n; i++)
            {
                var midpoint = a + (i + 0.5) * h;
                sum += fun(midpoint);
            }

            return sum * h;
        }

        private double IntegrateSimpson(Func<double, double> fun, double a, double b, int n)
        {
            if (n % 2 != 0)
            {
                n++;
            }

            var h = (b - a) / n;
            var sumOdd = 0.0;
            var sumEven = 0.0;
            for (var i = 1; i < n; i++)
            {
                var value = fun(a + i * h);
                if (i % 2 == 0)
                {
                    sumEven += value;
                }
                else
                {
                    sumOdd += value;
                }
            }

            return (h / 3.0) * (fun(a) + fun(b) + 4.0 * sumOdd + 2.0 * sumEven);
        }

        private bool IsFinite(double value)
        {
            return !(double.IsNaN(value) || double.IsInfinity(value));
        }

        private double FresnelC(double t, double eps)
        {
            if (Math.Abs(t) < NearZeroThreshold)
            {
                return 0.0;
            }

            var sign = Math.Sign(t);
            var absT = Math.Abs(t);
            var value = IntegrateBySelectedMethod(FresnelCosIntegrand, 0.0, absT, eps);
            return sign < 0 ? -value : value;
        }

        private double FresnelS(double t, double eps)
        {
            if (Math.Abs(t) < NearZeroThreshold)
            {
                return 0.0;
            }

            var sign = Math.Sign(t);
            var absT = Math.Abs(t);
            var value = IntegrateBySelectedMethod(FresnelSinIntegrand, 0.0, absT, eps);
            return sign < 0 ? -value : value;
        }

        private void RenderCornuSpiral()
        {
            var tMax = (double)numericUpDown2.Value;
            var samples = (int)numericUpDown3.Value;
            var eps = Math.Max(Eps(), MinSafeEps);
            var series = chart1.Series["CornuSpiral"];
            series.Points.Clear();
            var area = chart1.ChartAreas[0];
            area.AxisX.Minimum = -0.8;
            area.AxisX.Maximum = 0.8;
            area.AxisY.Minimum = -0.8;
            area.AxisY.Maximum = 0.8;

            if (samples <= 0 || tMax <= 0.0)
            {
                return;
            }

            var validPoints = 0;
            for (var i = 0; i <= samples; i++)
            {
                var t = -tMax + (2.0 * tMax * i) / samples;
                if (Math.Abs(t) < NearZeroThreshold)
                {
                    series.Points.AddXY(0.0, 0.0);
                    validPoints++;
                    continue;
                }

                if (!TryIntegrateBySelectedMethod(FresnelCosIntegrand, 0.0, Math.Abs(t), eps, out var cValue))
                {
                    continue;
                }

                if (!TryIntegrateBySelectedMethod(FresnelSinIntegrand, 0.0, Math.Abs(t), eps, out var sValue))
                {
                    continue;
                }

                var c = t < 0 ? -cValue : cValue;
                var s = t < 0 ? -sValue : sValue;
                if (!IsFinite(c) || !IsFinite(s))
                {
                    continue;
                }

                series.Points.AddXY(c, s);
                validPoints++;
            }

            if (validPoints == 0)
            {
                MessageBox.Show(
                    "Nie udało się narysować spirali dla podanych parametrów. Spróbuj mniejszego zakresu |t| lub mniejszej dokładności.",
                    "Błąd obliczeń",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            FillResultTable(tMax, eps);
        }

        private void FillResultTable(double tMax, double eps)
        {
            if (!TryIntegrateBySelectedMethod(FresnelCosIntegrand, 0.0, tMax, eps, out var cAtTMax))
            {
                cAtTMax = double.NaN;
            }

            if (!TryIntegrateBySelectedMethod(FresnelSinIntegrand, 0.0, tMax, eps, out var sAtTMax))
            {
                sAtTMax = double.NaN;
            }

            Tabela[0, 0].Value = FresnelInfinityLimit.ToString("F6");
            Tabela[1, 0].Value = IsFinite(cAtTMax) ? cAtTMax.ToString("F6") : "n/a";
            Tabela[2, 0].Value = IsFinite(cAtTMax)
                ? Math.Abs(cAtTMax - FresnelInfinityLimit).ToString("E2")
                : "n/a";

            Tabela[0, 1].Value = FresnelInfinityLimit.ToString("F6");
            Tabela[1, 1].Value = IsFinite(sAtTMax) ? sAtTMax.ToString("F6") : "n/a";
            Tabela[2, 1].Value = IsFinite(sAtTMax)
                ? Math.Abs(sAtTMax - FresnelInfinityLimit).ToString("E2")
                : "n/a";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RenderCornuSpiral();
        }

        private void Inputs_ValueChanged(object sender, EventArgs e)
        {
            if (!IsHandleCreated)
            {
                return;
            }

            RenderCornuSpiral();
        }
    }
}
