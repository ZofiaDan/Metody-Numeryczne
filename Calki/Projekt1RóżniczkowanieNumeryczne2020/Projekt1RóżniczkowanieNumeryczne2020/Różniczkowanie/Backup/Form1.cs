using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CSBibliotekaStudent.EkstraCalkaRozniczka;

namespace Rozniczkowanie
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int NrFun; //Numer wybranej funkcji do różniczkowania jako numer wiersza
        double x;//Wartość punktu w którym obliczamy pochodną
        double xp;//Wartość pochodnej wybranej funkcji
        double xp2;//Wartość drugiej pochodnej wybranej funkcji
        double eps; //Błąd różniczkowania

        public double FunWyboru(double x)
        {
            switch (NrFun)
            {
                case 1: return Math.Sin(x);
                case 2: return Math.Sinh(x);
                case 3: return Math.Tanh(x);
                case 4: return Math.Atan(x);

                case 5: return Math.Exp(-x * x);
                case 6: return Math.Tan(x);
                default: return 0;
            }
        }
        //----------------------------------------------------------------------------
        double PochodnaFun(double x, int NrFun)
        {
            double a;
            switch (NrFun)
            {
                case 1: return Math.Cos(x);
                case 2: return Math.Cosh(x);
                case 3: { a = Math.Cosh(x); a *= a; return 1.0 / a; };
                case 4: return 1.0 / (1.0 + x * x);
                case 5: return -2.0 * x * Math.Exp(-x * x);
                case 6: a = Math.Cos(x);
                    a *= a;
                    return 1.0 / a;
                default: return 0;
            }
        }
        //----------------------------------------------------------------------------
        double DrugaPochodnaFun(double x, int NrFun)
        {
            double a, b;
            switch (NrFun)
            {
                case 1: return -Math.Sin(x);
                case 2: return Math.Sinh(x);//Cosh(x);
                case 3: a = Math.Cosh(x);
                    b = a * a * a * a;
                    return -2.0 * Math.Sinh(x) * a / b;
                case 4: return -2.0 * x / ((1.0 + x * x) * (1.0 + x * x));


                case 5: return Math.Exp(-x * x) * (-2.0 + 4.0 * x * x);

                case 6: a = Math.Cos(x);
                    b = a * a * a * a;
                    return 2.0 * Math.Sin(x) * a / b;//2*Sin(x)*Cos(x)/Sqr(Sqr(Cos(x)));

                default: return 0;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TabelaFunkcji.Rows.Clear();
            TabelaFunkcji.Columns.Clear();
            TabelaFunkcji.RowCount = 7;
            TabelaFunkcji.ColumnCount = 6;
            for (int i = 1; i <= 5; i++) TabelaFunkcji.Columns[i].Width = 180;
            TabelaFunkcji[0, 0].Value = "   Funkcja";
            TabelaFunkcji[1, 0].Value = " Pochodna funkcji";
            TabelaFunkcji[2, 0].Value = "Pochodna analitycznie";
            TabelaFunkcji[3, 0].Value = "Pochodna numerycznie";
            TabelaFunkcji[4, 0].Value = "Druga pochodna analit.";
            TabelaFunkcji[5, 0].Value = "Druga pochodna numer.";
            TabelaFunkcji[0, 1].Value = "  sin(x)";
            TabelaFunkcji[0, 2].Value = "  sinh(x)";
            TabelaFunkcji[0, 3].Value = "  tanh(x)";
            TabelaFunkcji[0, 4].Value = "  atan(x)";
            TabelaFunkcji[0, 5].Value = "  exp(-x*x)";
            TabelaFunkcji[0, 6].Value = "  tan(x)";

            TabelaFunkcji[1, 1].Value = " cos(x)";
            TabelaFunkcji[1, 2].Value = "  cosh(x)";
            TabelaFunkcji[1, 3].Value = "  1/sqr(Cosh(x))";
            TabelaFunkcji[1, 4].Value = "  1/(1+x*x)";
            TabelaFunkcji[1, 5].Value = "  -2*x*exp(-sqr(x))";
            TabelaFunkcji[1, 6].Value = "  1/sqr(cos(x))";
            trackBar1_ValueChanged(sender, e);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            x = trackBar1.Value / 500.0;
            textBox1.Text = x.ToString();
        }

        private void TabelaFunkcji_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            int W = e.RowIndex;
            double h0 = 0.005, q = 2.0;
            //eps - błąd różniczkowania jako 10 do potęgi n = -SpinEdit1.Value
            eps = Math.Pow(10.0, -(int)numericUpDown1.Value);
            int maxit = 10;
            NrFun = W; // x,  h0,  q, eps, maxit
            TDifferential Pochodna = new TDifferential(FunWyboru, x, h0, q, eps, maxit);
            xp = Pochodna.PierwszPochodnaR4();
            xp2 = Pochodna.DrugaPochodnaR4();
            TabelaFunkcji[2, W].Value = PochodnaFun(x, NrFun).ToString();
            TabelaFunkcji[4, W].Value = DrugaPochodnaFun(x, NrFun).ToString();
            TabelaFunkcji[3, W].Value = xp.ToString();
            TabelaFunkcji[5, W].Value = xp2.ToString();

        }
        //-----------------------------------------------------------------------------

    }
}
