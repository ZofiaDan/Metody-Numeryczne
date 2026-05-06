using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSBibliotekaStudent
{

    public delegate double FunkcjaRealeReale(double x);
    public delegate Complex.Complex FunkcjaComplexReale(double x);
    public delegate double FunkcjaRealeRealeParams(double x, params double[] a);
    public delegate void FunNieLinDelegate(double[] F, double[] X);
    public delegate double[] FunWektorWektorDelegate(double[] X);
    public delegate double FunWektorSkalarDelegate(double[] X);

    public delegate void FunNonLinearDiffEquationDelegateD(double[] F, double[] X, double t);

    public delegate double[] FunNonLinearDiffEquationDelegateWD(double[] X, double t);

    public class Global
    {

    }
}
