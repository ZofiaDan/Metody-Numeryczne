using System;
using System.Collections.Generic;

namespace Rozniczkowanie
{
    public struct VanDerPolPoint
    {
        public double T;
        public double X;
        public double V;
    }

    public static class VanDerPolSolver
    {
        public static List<VanDerPolPoint> SolveRk4(double t0, double tEnd, double dt, double x0, double v0, double mu)
        {
            var points = new List<VanDerPolPoint>();

            double t = t0;
            double x = x0;
            double v = v0;

            points.Add(new VanDerPolPoint { T = t, X = x, V = v });

            while (t < tEnd)
            {
                double h = Math.Min(dt, tEnd - t);

                double k1x = h * v;
                double k1v = h * VDot(x, v, mu);

                double k2x = h * (v + 0.5 * k1v);
                double k2v = h * VDot(x + 0.5 * k1x, v + 0.5 * k1v, mu);

                double k3x = h * (v + 0.5 * k2v);
                double k3v = h * VDot(x + 0.5 * k2x, v + 0.5 * k2v, mu);

                double k4x = h * (v + k3v);
                double k4v = h * VDot(x + k3x, v + k3v, mu);

                x += (k1x + 2.0 * k2x + 2.0 * k3x + k4x) / 6.0;
                v += (k1v + 2.0 * k2v + 2.0 * k3v + k4v) / 6.0;
                t += h;

                points.Add(new VanDerPolPoint { T = t, X = x, V = v });
            }

            return points;
        }

        private static double VDot(double x, double v, double mu)
        {
            return mu * (1.0 - x * x) * v - x;
        }
    }
}
