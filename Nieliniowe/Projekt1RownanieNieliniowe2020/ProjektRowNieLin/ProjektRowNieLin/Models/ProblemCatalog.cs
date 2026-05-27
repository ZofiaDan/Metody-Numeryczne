using BIBLIOTEKA_NR1.NieLiniowe;
using System;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace ProjektRowNieLin.Models
{
    internal static class ProblemCatalog
    {
        public static IReadOnlyList<ProblemDefinition> Problems { get; } = new List<ProblemDefinition>
        {
            CreateGeometry(),
            CreateEconomy()
        };

        private static ProblemDefinition CreateGeometry()
        {
            FunWektorWektor fun = EquationSystems.Geometry;

            return new ProblemDefinition(
                displayName: "Geometria (Okrąg + Elipsa)",
                description: "Wyznaczenie punktów przecięcia okręgu i elipsy (układ F(X)=0).",
                variable1Caption: "x1",
                variable2Caption: "x2",
                variable1RowHeader: "x1 =",
                variable2RowHeader: "x2 =",
                equation1Text: "F1(x1,x2) = x1^2 + x2^2 - 26 = 0  (okrąg)",
                equation2Text: "F2(x1,x2) = 3*x1^2 + 25*x2^2 - 100 = 0  (elipsa)",
                defaultInitialGuess: new double[] { 0, -6.0, 6.0 },
                function: fun,
                drawCurves: chart =>
                {
                    chart.Series[0].Points.Clear(); // F1=0 (okrąg)
                    chart.Series[1].Points.Clear(); // F2=0 (elipsa)

                    // Visual style + legend
                    chart.Series[0].Name = "Okrąg (F1=0)";
                    chart.Series[1].Name = "Elipsa (F2=0)";

                    chart.Legends[0].Enabled = true;

                    chart.ChartAreas[0].AxisX.Title = "x1";
                    chart.ChartAreas[0].AxisY.Title = "x2";
                    chart.ChartAreas[0].AxisX.Interval = 1;
                    chart.ChartAreas[0].AxisY.Interval = 1;
                    chart.ChartAreas[0].AxisX.Minimum = -6;
                    chart.ChartAreas[0].AxisX.Maximum = 6;
                    chart.ChartAreas[0].AxisY.Minimum = -6;
                    chart.ChartAreas[0].AxisY.Maximum = 6;

                    // Okrąg
                    double r = Math.Sqrt(26.0);

                    // Elipsa: 3*x1^2 + 25*x2^2 = 100  =>  x1 = a*cos(t), x2=b*sin(t)
                    double a = Math.Sqrt(100.0 / 3.0);
                    double b = Math.Sqrt(100.0 / 25.0);

                    int N = 200;
                    double dt = 2 * Math.PI / N;
                    for (int i = 0; i <= N; i++)
                    {
                        double t = i * dt;

                        // Okrąg
                        double x1 = r * Math.Cos(t);
                        double x2 = r * Math.Sin(t);
                        chart.Series[0].Points.AddXY(x1, x2);

                        // Elipsa
                        double xe = a * Math.Cos(t);
                        double ye = b * Math.Sin(t);
                        chart.Series[1].Points.AddXY(xe, ye);
                    }
                }
            );
        }

        private static ProblemDefinition CreateEconomy()
        {
            FunWektorWektor fun = EquationSystems.Economy;

            return new ProblemDefinition(
                displayName: "Ekonomia (Równowaga rynku)",
                description: "Równowaga rynkowa dla dwóch towarów: popyt = podaż w modelu edukacyjnym.",
                variable1Caption: "p1 (cena 1)",
                variable2Caption: "p2 (cena 2)",
                variable1RowHeader: "p1 =",
                variable2RowHeader: "p2 =",
                equation1Text: "F1(p1,p2) = (100 + 2*p2 - 3*p1) - (-50 + 2*p1) = 0  -> 150 + 2*p2 - 5*p1 = 0",
                equation2Text: "F2(p1,p2) = (80 - 1.5*p2 + p1) - (-20 + 1.5*p2) = 0 -> 100 + p1 - 3*p2 = 0",
                defaultInitialGuess: new double[] { 0, 50.0, 40.0 },
                function: fun,
                drawCurves: chart =>
                {
                    chart.Series[0].Points.Clear(); // F1=0
                    chart.Series[1].Points.Clear(); // F2=0

                    // Visual style + legend
                    chart.Series[0].Name = "Równowaga towaru 1 (F1=0)";
                    chart.Series[1].Name = "Równowaga towaru 2 (F2=0)";
                    chart.Legends[0].Enabled = true;

                    chart.ChartAreas[0].AxisX.Title = "p1";
                    chart.ChartAreas[0].AxisY.Title = "p2";
                    chart.ChartAreas[0].AxisX.Interval = 10;
                    chart.ChartAreas[0].AxisY.Interval = 10;
                    chart.ChartAreas[0].AxisX.Minimum = 0;
                    chart.ChartAreas[0].AxisX.Maximum = 100;
                    chart.ChartAreas[0].AxisY.Minimum = 0;
                    chart.ChartAreas[0].AxisY.Maximum = 100;

                    int N = 200;
                    for (int i = 0; i <= N; i++)
                    {
                        double p1 = (double)i * 100.0 / N;

                        // F1: 150 + 2*p2 - 5*p1 = 0 => p2 = (5*p1 - 150)/2 = 2.5*p1 - 75
                        double p2_1 = 2.5 * p1 - 75.0;

                        // F2: 100 + p1 - 3*p2 = 0 => p2 = (p1+100)/3
                        double p2_2 = (p1 + 100.0) / 3.0;

                        if (p2_1 >= 0 && p2_1 <= 100)
                            chart.Series[0].Points.AddXY(p1, p2_1);

                        if (p2_2 >= 0 && p2_2 <= 100)
                            chart.Series[1].Points.AddXY(p1, p2_2);
                    }
                }
            );
        }
    }
}

