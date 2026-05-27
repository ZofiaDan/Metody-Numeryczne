using System;

namespace ProjektRowNieLin
{
    internal static class EquationSystems
    {
        // Geometry system:
        // F1(x1,x2)=x1^2 + x2^2 - 26 = 0  (circle)
        // F2(x1,x2)=3*x1^2 + 25*x2^2 - 100 = 0 (ellipse)
        public static double[] Geometry(double[] X)
        {
            double x1k = X[1] * X[1];
            double x2k = X[2] * X[2];

            double[] F = new double[3];
            F[1] = x1k + x2k - 26.0;
            F[2] = 3.0 * x1k + 25.0 * x2k - 100.0;
            return F;
        }

        // Economy system (2-good market equilibrium):
        // X[1]=p1, X[2]=p2
        // F1(p1,p2) = (100 + 2*p2 - 3*p1) - (-50 + 2*p1) = 0  -> 150 + 2*p2 - 5*p1 = 0
        // F2(p1,p2) = (80 - 1.5*p2 + p1) - (-20 + 1.5*p2) = 0 -> 100 + p1 - 3*p2 = 0
        public static double[] Economy(double[] X)
        {
            double p1 = X[1];
            double p2 = X[2];

            double Qd1 = 100 + 2 * p2 - 3 * p1;
            double Qs1 = -50 + 2 * p1;
            double Qd2 = 80 - 1.5 * p2 + p1;
            double Qs2 = -20 + 1.5 * p2;

            double[] F = new double[3];
            F[1] = Qd1 - Qs1;
            F[2] = Qd2 - Qs2;
            return F;
        }
    }
}

