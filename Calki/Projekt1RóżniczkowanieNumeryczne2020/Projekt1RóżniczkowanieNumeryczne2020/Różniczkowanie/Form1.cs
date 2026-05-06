using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BIBLIOTEKA_NR1.CalkiPochodne;

namespace Rozniczkowanie
{
    /// <summary>
    /// Delegat dla funkcji rzeczywistej zmiennej rzeczywistej
    /// </summary>
    public delegate double FunkcjaRealeReale(double x);

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

        /// <summary>
        /// Funkcja wyboru z klasy Math w postaci analitycznej 
        /// </summary>
        /// <param name="x">Argoment funkcji</param>
        /// <returns>Zwracana wartość</returns>
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

        /// <summary>
        /// Pochodne w postaci analitycznej funkcji wyboru FunWyboru(x)
        /// </summary>
        /// <param name="x">Argument pochodnej</param>
        /// <param name="NrFun">Zwracana wartość pochodnej</param>
        /// <returns></returns>
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

        /// <summary>
        /// Druga pochodna w postaci analitycznej funkcji wyboru FunWyboru(x)
        /// </summary>
        /// <param name="x">Argument drugiej pochodnej</param>
        /// <param name="NrFun">Zwracana wartość drugiej pochodnej</param>
        /// <returns></returns>
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

        /// <summary>
        /// Metoda podpięta pod zdarzenie Load generowane automatycznie
        /// przy uruchamianiu projektu.
        /// Inicjalizuje kontrolkę TabelaFunkcj typu DataGridView 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //Czyszczenie komponentu TabelaFunkcji typu DataGridView 
            TabelaFunkcji.Rows.Clear();
            TabelaFunkcji.Columns.Clear();
            //Ustalanie liczby wierszy i kolumn komponentu TabelaFunkcji
            TabelaFunkcji.RowCount = 7;
            TabelaFunkcji.ColumnCount = 6;
            //Ustalanie szerokości kolumn komponentu TabelaFunkcji
            for (int i = 1; i <= 5; i++) TabelaFunkcji.Columns[i].Width = 180;
            //Opis poszczególnych kolumn i wierszy dla TabelaFunkcji
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
            //Wstępne wyznaczenie zmiennej x w której oblicza się pochodną funkcji
            //i umieszczenie wyniku w komponencie textBox1.Text
            trackBar1_ValueChanged(sender, e);
        }

        /// <summary>
        /// Metoda podpięta pod zdarzenie ValueChanged komponentu
        /// trackBar1 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            x = trackBar1.Value / 500.0;//Współrzędne suwaka trackBar1.Value dzielimy przez 500 
            textBox1.Text = x.ToString();
        }

        /// <summary>
        /// Metoda klasy formularza podpięta pod zdarzenie CellClick komponentu 
        /// TabelaFunkcji typu DataGridView 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Parametr metody </param>
        private void TabelaFunkcji_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //Parametr metody e ma właściwość RowIndex określającą numer wybranego
            //wiersza po przez kliknięcie 
            int W = e.RowIndex;
            double h0 = 0.05,//Wstępnie zadany krok różniczkowania
                   q = 2.0;  //Iloraz próbnych kroków różniczkowania
            //eps - błąd różniczkowania jako 10 do potęgi n = -(int)numericUpDown1.Value
            eps = Math.Pow(10.0, -(int)numericUpDown1.Value);
            int maxit = 50;
            NrFun = W; //Ustalenie numeru wybranej funkcji NrFun jako zmiennej
                       //globalnej klasy formularza widocznej w funkcjach wyboru 
                       //FunWyboru(double x),PochodnaFun(double x, int NrFun) 

            //Deklaracja i inicjalizacja obiektu do wykonania różniczkowania funkcji
            //FunWyboru(x) w punkcie x
            RozniczkowanieFunkcji Pochodna = new RozniczkowanieFunkcji(FunWyboru, x,h0,q, eps, maxit);
            //Obliczanie pierwszej pochodnej funkcji rzędu czwartego
            xp = Pochodna.PierwszPochodnaR4();
            //Obliczanie drugiej pochodnej funkcji rzędu czwartego
            xp2 = Pochodna.DrugaPochodnaR4();
            //Zapis rozwiązania w komponencie TabelaFunkcji typu DataGridView 
            TabelaFunkcji[2, W].Value = PochodnaFun(x, NrFun).ToString();
            TabelaFunkcji[4, W].Value = DrugaPochodnaFun(x, NrFun).ToString();
            TabelaFunkcji[3, W].Value = xp.ToString();
            TabelaFunkcji[5, W].Value = xp2.ToString();
            //Prezentacja błędu rzeczywistego obliczeń na tle zadanej tolerancji obliczeń
            chart1.Titles.Clear();
            chart1.Titles.Add("Błąd rzeczywisty różniczkowania numerycznego ");
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            chart1.Series[0].LegendText = "Zadana tolerancja Log10(eps";
            chart1.Series[1].LegendText = "błąd opcięcia rzędu 2 Log10(|df(x)/dx-dfnum(x)/dx|";
            chart1.Series[2].LegendText = "błąd opcięcia rzędu 4 Log10(|df(x)/dx-dfnum(x)/dx|";
            chart1.ChartAreas[0].AxisY.Maximum = -5.0;
            chart1.ChartAreas[0].AxisY.Minimum = -16.0;
            chart1.ChartAreas[0].AxisX.Maximum = 2;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Interval = 1;
            chart1.ChartAreas[0].AxisX.Interval = 0.2;
            chart1.ChartAreas[0].AxisX.Title = "x";
            chart1.ChartAreas[0].AxisY.Title = "Log10(|eps|)";
            chart1.Titles.Add("eps= " + eps.ToString("e"));
            double dx = 0.2;
            double logeps;
            for (int i = 1; i <= 10; i++)
            {
                x = i * dx;//Zmienny parametr obliczania pochodnej w przedziale od zera do 2
                //Inicjalizacja obiektu Pochodna dla kolejnych wartości x
                Pochodna = new RozniczkowanieFunkcji(FunWyboru, x, h0, q, eps, maxit);
                //Wykres lini poziomej zadanej tolreancji obliczeń
                chart1.Series[0].Points.AddXY(x, Math.Log10(eps));
                //Obliczanie pochodnej dla błędu obcięcia rzędu 2 przy pomocy 
                //metody PierwszPochodnaR2 klasy RozniczkowanieFunkcjiAbstract
                xp = Pochodna.PierwszPochodnaR2();
                //Wykres punktowy w skali Log10 błędu rzeczywistego jako 
                //różnicy pochodnej dokładnej i obliczonej numerycznie
                logeps = Math.Log10(Math.Abs(PochodnaFun(x, NrFun) - xp));
                chart1.Series[1].Points.AddXY(x, logeps);
                //Obliczanie pochodnej dla błędu obcięcia rzędu 4 przy pomocy 
                //metody PierwszPochodnaR4 klasy RozniczkowanieFunkcjiAbstract
                xp = Pochodna.PierwszPochodnaR4();
                logeps = Math.Log10(Math.Abs(PochodnaFun(x, NrFun) - xp));
                chart1.Series[2].Points.AddXY(x, logeps);
            }

        }//Koniec metody TabelaFunkcji_CellClick

    }//Koniec klasy formularza public partial class Form1

}//Koniec przestrzeni nazw projektu rozniczkowanie
