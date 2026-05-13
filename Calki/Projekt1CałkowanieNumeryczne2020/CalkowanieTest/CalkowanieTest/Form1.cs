using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BIBLIOTEKA_NR1;
using BIBLIOTEKA_NR1.CalkiPochodne;


namespace CalkowanieTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Deklaracja obiektu do całkowania funkcji 
        /// rzeczywistej zmiennej rzeczywistej
        /// </summary>
        CalkowanieFunkcji Calka;
        double CalkaFun1;//Zmienna do wyrażenia pierwszej całki testującej
        double CalkaFun2;//Zmienna do wyrażenia drugiej całki testującej
        double fi = Math.PI / 6;//Zmienna pomocnicza do Fun2
        double Ca; //Do przechowywania obliczonej całki
        /// <summary>
        /// Funkcja podcałkowa pierwszej całki
        /// </summary>
        /// <param name="x">Argument funkcji</param>
        /// <returns>Zwracana wartość</returns>
        public double Fun1(double x)
        {
            return 1.0 / (5 + 3 * Math.Sin(x));
        }

        /// <summary>
        /// Funkcja podcałkowa drugiej całki
        /// </summary>
        /// <param name="x">Argument funkcji</param>
        /// <returns>Zwracana wartość</returns>
        public double Fun2(double x)
        {
            return 1.0 / (1 + 2 * x * Math.Cos(fi) + x * x);
        }

        /// <summary>
        /// Funkcja wyznaczająca dokładność iteracji w ekstrapolacji
        /// całkowania na podstawie zadanego parametru komponentu
        /// numericUpDown1.Value
        /// </summary>
        /// <returns>Zwracana wartośc </returns>
        double eps()
        {
            return Math.Pow(10.0, -(double)numericUpDown1.Value);
        }

        /// <summary>
        /// Metoda podpięta pod zdarzenie Load generowane automatycznie
        /// przy uruchamianiu projektu.
        /// Inicjalizuje kontrolkę Tabela typu DataGridView 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //Inicjalizacja Tabela typu DataGridView
            Tabela.RowCount = 2;
            //Opi wierszy tabeli
            Tabela.Rows[0].HeaderCell.Value = "Całka1";
            Tabela.Rows[1].HeaderCell.Value = "Całka2";
            //Wstpnie ustalona zerowa (pierwsza) pozycja wiersza
            comboBox1.SelectedIndex = 0;
            //Podanie dokładnych wartości dwóch całek testujących 
            //metodę numerycznego całkowania 
            CalkaFun1 = 0.5 * (Math.Atan(2.0) - Math.Atan(0.75));
            CalkaFun2 = 0.5 * fi / Math.Sin(fi);
        }

        /// <summary>
        /// Metoda klasy formularza realizująca całkowanie funkcji Fun(x) 
        /// w przedziale od a do b z dokładnościa procesu ekstrapolacji eps
        /// W klasie formularza mamy dwie funkcje do całkowania Fun1(x) oraz Fun2(x)
        /// Metoda umożliwia optymalizację kodu przy całkowaniu tych dwóch funkcji
        /// </summary>
        /// <param name="Fun">Przekazanu egzemplarz funkcji do całkowania</param>
        /// <param name="a">Początek przedziału całkowania</param>
        /// <param name="b">Koniec przedziału całkowania</param>
        /// <param name="eps">Dokładność zadanej tolerancji obliczeń procesu 
        /// ekstrapolacji przy całkowaniu</param>
        /// <returns>Zwracana wartość całki</returns>
        private double Calkowanie(FunkcjaRealeReale Fun, double a, double b, double eps)
        {
            double h0 = 0.5 * Math.PI / 10;  //Wstępnie ustalony krok całkowania
            double Ca = 0;
            //Inicjalizacja obiektu Calka do całkowania funkcji Fun1 
            //podanej przez konstruktora w przedziale od 0 do Math.PI / 2
            Calka = new CalkowanieFunkcji(Fun, a, b, h0, 2.0, eps, 50);
            //Wybór metody w zależności od właściwoci SelectedIndex komponentu comboBox1
            switch (comboBox1.SelectedIndex)
            {
                case 0:   //Wybór metody Aitkena dla trapezów;
                    Ca = Calka.MetodaAitkenaDlaTrapezow();
                    break;
                case 1:   //Wybór metody Aitkena Simpsona
                    Ca = Calka.MetodaAitkenaSimpsona();

                    break;
                case 2:   //Wybór metody Aitkena dla prostokątów
                    Ca = Calka.MetodaAitkenaDlaProstokatow();
                    break;
            }
            return Ca;
        }

        /// <summary>
        /// Metoda podpięta pod zdarzenie Click komponentu 
        /// pictureBox1 klasy PictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //Zastosowanie metody klasy formularza do całkowania Fun1(x)
            //w przedziale od 0 do PI / 2
            Ca = Calkowanie(Fun1, 0, Math.PI / 2, eps());
            //Wydruk wyników obliczeń
            //Wynik dokładny z obliczeń analitycznych
            Tabela[0, 0].Value = CalkaFun1.ToString();
            //Wynik przybliżony w oparciu o ekstrapolację całkową
            Tabela[1,0].Value = Ca.ToString();
            //Błąd rzeczywisty
            Tabela[2, 0].Value = Math.Abs(Ca - CalkaFun1).ToString("E");
        }

        /// <summary>
        /// Metoda podpięta pod zdarzenie Click komponentu 
        /// pictureBox2 klasy PictureBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //Zastosowanie metody klasy formularza do całkowania Fun2(x)
            //w przedziale od 0 do 1
            Ca = Calkowanie(Fun2, 0, 1, eps());
            //Wydruk wyników obliczeń
            //Wynik dokładny z obliczeń analitycznych
            Tabela[0, 1].Value = CalkaFun2.ToString();
            //Wynik przybliżony w oparciu o ekstrapolację całkową
            Tabela[1, 1].Value = Ca.ToString();
            //Błąd rzeczywisty
            Tabela[2, 1].Value = Math.Abs(Ca - CalkaFun2).ToString("E");
        }

        /// <summary>
        /// Metoda klasy formularza podpięta pod zdarzen Click
        /// komponentu button1 realizująca graficznie zależność 
        /// błędu rzeczywistego całkowania od zadanej tolerancji
        /// ekstrapolacji całkowania dla Fun1(x) oraz Fun2(x)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            double eps1;
            double df;
            double Ca = 0;
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisY.Interval = 1;
            //Ustalenie na komponencie chart1 przedziałów zmienności na osi X oraz Y 
            chart1.ChartAreas[0].AxisX.Minimum = -12;
            chart1.ChartAreas[0].AxisX.Maximum = -3;
            chart1.ChartAreas[0].AxisY.Minimum = -16;
            chart1.ChartAreas[0].AxisY.Maximum = -2;
            chart1.ChartAreas[0].AxisX.Title = "Tolerancja obliczen Log10(eps) ";
            chart1.ChartAreas[0].AxisY.Title = "Błąd rzeczywisty Log10(|blad|) ";
            for (int i=3; i<=12; i++)
            {
                //Zadawana tolerancja obliczeń ekstrapolacji całkowania
                eps1 = Math.Pow(10.0, -i); 
                //Całkowanie Fun1(x) w przedziale od 0 do PI/2
                Ca = Calkowanie(Fun1, 0, Math.PI / 2, eps1);
                //df- błąd rzeczywisty całkowania Fun1 dla zadanego eps
                df = Math.Abs(Ca - CalkaFun1);
                //Kontynuacja zapisu graficznego w skali logarytmicznej 
                chart1.Series[0].Points.AddXY(Math.Log10(eps1),Math.Log10(df));
                //Całkowanie Fun2(x) w przedziale od 0 do 1
                Ca = Calkowanie(Fun2, 0, 1, eps1);
                //df- błąd rzeczywisty całkowania Fun2 dla zadanego eps
                df = Math.Abs(Ca - CalkaFun2);
                //Kontynuacja zapisu graficznego w skali logarytmicznej 
                chart1.Series[1].Points.AddXY(Math.Log10(eps1), Math.Log10(df));
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }//Koniec public partial class Form1 : Form 
}//Koniec CalkowanieTest
