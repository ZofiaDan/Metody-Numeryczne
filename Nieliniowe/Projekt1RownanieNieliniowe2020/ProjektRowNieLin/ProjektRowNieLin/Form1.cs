using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BIBLIOTEKA_NR1.NieLiniowe;

namespace ProjektRowNieLin
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Konstruktor klasy formularza generowany automatycznie
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Deklaracja obiektu do rozwiązywania układu równań nieliniowych
        /// jako pole klasy formularza- delaruje pojektant
        /// </summary>
        Metoda_Newtona_Abstr RNL;

        /// <summary>
        /// Wektory do zadawania warunków początkowych 
        /// i rozwiązania układu równań nieliniowych - delaruje pojektant
        /// </summary>
        double[] X0,X;

        /// <summary>
        /// Zmienna przechowująca wybrany typ problemu (0=Geometria, 1=Ekonomia)
        /// </summary>
        int problemType = 0;

        /// <summary>
        /// Metoda definiująca metodę, która dowolnemu wektorowi X
        /// przyporządkowuje wektor F do zapisu układu 
        /// dwóch równań nieliniowych - delaruje pojektant
        /// </summary>
        /// <param name="X">Argument funkcji jako wekto X</param>
        /// <returns>Zwraca wektor</returns>
        public double[] FunW(double[] X)
        {
            double x1k, x2k;
            double[] F = new double[3];
            x1k = X[1] * X[1];
            x2k = X[2] * X[2];
            //Na płaszczyźnie x10x2 przedstawia okrąg
            F[1] = x1k + x2k - 26.0; 
            //Przedstawia elipsę przecinającą okrąg w czterech punktach
            F[2] = 3 * x1k + 25 * x2k - 100.0; 
            return F;
        }

        /// <summary>
        /// Metoda definiująca układ równań dla równowagi rynku dwóch towarów
        /// X[1] = p1 (cena towaru 1), X[2] = p2 (cena towaru 2)
        /// Funkcje popytu i podaży przykładowe (realistyczne dla edukacji)
        /// </summary>
        /// <param name="X">Wektor cen: X[1]=p1, X[2]=p2</param>
        /// <returns>Wektor równań: F[1]=Qd1-Qs1, F[2]=Qd2-Qs2</returns>
        public double[] FunW_Ekonomia(double[] X)
        {
            double p1 = X[1];  // Cena towaru 1
            double p2 = X[2];  // Cena towaru 2
            double[] F = new double[3];

            // Rynek towaru 1
            // Popyt: Qd1 = 100 + 2*p2 - 3*p1 (maleję z ceną p1, rosną z ceną p2)
            // Podaż: Qs1 = -50 + 2*p1 (rosną z ceną p1)
            double Qd1 = 100 + 2 * p2 - 3 * p1;
            double Qs1 = -50 + 2 * p1;
            F[1] = Qd1 - Qs1; // równowaga: Qd1 = Qs1

            // Rynek towaru 2
            // Popyt: Qd2 = 80 - 1.5*p2 + p1 (maleją z ceną p2, rosną z ceną p1)
            // Podaż: Qs2 = -20 + 1.5*p2 (rosną z ceną p2)
            double Qd2 = 80 - 1.5 * p2 + p1;
            double Qs2 = -20 + 1.5 * p2;
            F[2] = Qd2 - Qs2; // równowaga: Qd2 = Qs2

            return F;
        }

        /// <summary>
        /// Metoda ustalająca wstępne parametry projektu 
        /// wywoływana automatycznie po uruchomieniu programu
        /// Nagłówek metody generowany automatycznie-nie kopiować
        /// Instrukcje w metodzie pisze projektant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            X0 = new double[3];
            X = new double[3];
            problemType = 0; // Domyślnie geometria
            
            //Ustalenie wymiarów tabeli
            dataGridView1.RowCount = 2;
            dataGridView1.ColumnCount = 2;
            
            //Opis pierwszej i drugiej kolumny tabeli
            dataGridView1.Columns[0].HeaderCell.Value = "Rozwiązanie";
            dataGridView1.Columns[1].HeaderCell.Value = "Warunek początkowy";
            
            // Ustaw ComboBox na pierwszą opcję (Geometria)
            comboBoxProblem.SelectedIndex = 0;
            
            // Zaktualizuj UI dla problemów geometrycznych
            UpdateProblemUI();
            
            //Wywołanie metody do wykresu 
            button2_Click(sender, e);
        }//Form1_Load

        /// <summary>
        /// Metoda aktualizująca interfejs na podstawie wybranego typu problemu
        /// </summary>
        private void UpdateProblemUI()
        {
            if (problemType == 0) // Geometria
            {
                labelVar1.Text = "x1 =";
                labelVar2.Text = "x2 =";
                dataGridView1.Rows[0].HeaderCell.Value = "x1 =";
                dataGridView1.Rows[1].HeaderCell.Value = "x2 =";
                //Warunki początkowe dla geometrii
                X0[1] = -6.0; X0[2] = 6.0;
            }
            else // Ekonomia
            {
                labelVar1.Text = "p1 (cena 1) =";
                labelVar2.Text = "p2 (cena 2) =";
                dataGridView1.Rows[0].HeaderCell.Value = "p1 =";
                dataGridView1.Rows[1].HeaderCell.Value = "p2 =";
                //Warunki początkowe dla ekonomii (rozsądne ceny)
                X0[1] = 50.0; X0[2] = 40.0;
            }
            
            //Zaktualizuj wartości wyświetlane w tabeli
            dataGridView1[1, 0].Value = X0[1].ToString();
            dataGridView1[1, 1].Value = X0[2].ToString();
        }

        /// <summary>
        /// Metoda podpięta pod zdarzenie Click przycisku button1
        /// realizująca rozwiązywanie układu równaań nieliniowych
        /// Nagłówek metody generowany automatycznie-nie kopiować
        /// Instrukcje w metodzie pisze projektant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            int blad = 0;
            //Odczyt dowolnie ustalonych warunków początkowych iteracji 
            //z drugiej kolumny tabeli dataGridView1 
            X0[1] = double.Parse(dataGridView1[1, 0].Value.ToString());
            X0[2] = double.Parse(dataGridView1[1, 1].Value.ToString());
            //Czyszczenie wykresu punktowego dla formatki chart1
            chart1.Series[2].Points.Clear();
            //Odczyt parametru eps określających dokładność iteracji w metodzie Newtona
            double eps = double.Parse(textBox2.Text);
            
            //Inicjalizacja obiektu RNL typu Metoda_Newtona_Abstr do 
            //rozwiązywania układu równań nieliniowych
            //Wybierz odpowiednią funkcję równań na podstawie typu problemu
            if (problemType == 0) // Geometria
                RNL = new MetodaNewtona(FunW, 2, X0, eps, 1E-8, 1E-20, 100);
            else // Ekonomia
                RNL = new MetodaNewtona(FunW_Ekonomia, 2, X0, eps, 1E-8, 1E-20, 100);
            
            //Wywołanie metody rozwiązywania układu równań nieliniowych  
            blad = RNL.Rozwiaz();
            if (blad == 0)
            {
                for (int i = 1; i <= 2; i++)
                {
                    X[i] = RNL.X[i];
                    //Zapis rozwiązania w pierwszej kolumnie(zerowy indeks) tabelki dataGridView1 
                    dataGridView1[0, i - 1].Value = X[i].ToString("F16");
                }
                //Zapis ilości iteracji Newtona, które zwraca obiekt RNL 
                textBox1.Text ="Nite= "+ RNL.Nite.ToString();
                //Ilustracja graficzna procesu iteracyjnego w postaci punktów na komponencie chart1
                //chart1.Series[2] jest utalony na typ Point wykresu punktowego
                for (int j = 0; j <= RNL.Nite; j++)
                    chart1.Series[2].Points.AddXY(RNL.XX[j][1], RNL.XX[j][2]);
                button2.Enabled = true;
            }
            else RNL.PiszKomunikat(blad);
        }//button1_Click

        /// <summary>
        /// Metoda do ilustracji położenia możliwych punktów rozwiązania
        /// w zależności od wybranego punktu początkowego iteracji 
        /// Nagłówek metody generowany automatycznie-nie kopiować
        /// Instrukcje w metodzie pisze projektant
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            double t, dt, x1, x2, r, a, b;
            int N = 100;

            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            
            if (problemType == 0) // Geometria
            {
                // Rysuj okrąg i elipsę
                r = Math.Sqrt(26.0);
                //Parametry elipsy a, b
                a = 10.0 / Math.Sqrt(3.0);
                b = 2.0;
                //Krok obliczeń dla wykresu ciągłego
                dt = 2 * Math.PI / N;

                chart1.ChartAreas[0].AxisX.Interval = 1;
                chart1.ChartAreas[0].AxisY.Interval = 1;
                //Ustalenie przedziałów zmienności na osi X oraz Y 
                chart1.ChartAreas[0].AxisX.Minimum = -6;
                chart1.ChartAreas[0].AxisX.Maximum = 6;
                chart1.ChartAreas[0].AxisY.Minimum = -6;
                chart1.ChartAreas[0].AxisY.Maximum = 6;
                
                for (int i = 0; i <= N; i++)
                {
                    t = i * dt;//Parametr do zapisu parametrycznego okręgu i elipsy
                    //Zapis parametryczny elipsy
                    x1 = a * Math.Cos(t);
                    x2 = b * Math.Sin(t);
                    //chart1.Series[0] wybrano jako typ Spline wykresu ciągłego 
                    chart1.Series[0].Points.AddXY(x1, x2);
                    //Zapis parametryczny okręgu
                    x1 = r * Math.Cos(t);
                    x2 = r * Math.Sin(t);
                    //chart1.Series[1] wybrano jako typ Spline wykresu ciągłego
                    chart1.Series[1].Points.AddXY(x1, x2);
                }
            }
            else // Ekonomia - rysuj krzywe popytu i podaży
            {
                // Dla towaru 1 (Series 0) i towaru 2 (Series 1)
                chart1.ChartAreas[0].AxisX.Interval = 10;
                chart1.ChartAreas[0].AxisY.Interval = 10;
                //Ustalenie przedziałów zmienności (ceny i ilości)
                chart1.ChartAreas[0].AxisX.Minimum = 0;
                chart1.ChartAreas[0].AxisX.Maximum = 100;
                chart1.ChartAreas[0].AxisY.Minimum = 0;
                chart1.ChartAreas[0].AxisY.Maximum = 100;
                
                // Rysuj krzywe dla towaru 1 (Series 0 = Popyt, Series 1 = Podaż)
                // Przy p2 = średnia cena równowagi (używamy uproszczenia p2 = 40)
                double p2_fixed = 40.0; // Przyjmujemy stałą cenę towaru 2
                
                for (int i = 0; i <= N; i++)
                {
                    double p1 = (double)i * 100.0 / N; // Cena od 0 do 100
                    
                    // Popyt na towar 1: Qd1 = 100 + 2*p2 - 3*p1
                    double Qd1 = 100 + 2 * p2_fixed - 3 * p1;
                    if (Qd1 >= 0 && Qd1 <= 100)
                        chart1.Series[0].Points.AddXY(p1, Qd1);
                    
                    // Podaż towaru 1: Qs1 = -50 + 2*p1
                    double Qs1 = -50 + 2 * p1;
                    if (Qs1 >= 0 && Qs1 <= 100)
                        chart1.Series[1].Points.AddXY(p1, Qs1);
                }
            }
        }//button2_Click

        /// <summary>
        /// EventHandler dla ComboBox - zmienia typ problemu i aktualizuje interfejs
        /// </summary>
        private void comboBoxProblem_SelectedIndexChanged(object sender, EventArgs e)
        {
            problemType = comboBoxProblem.SelectedIndex;
            UpdateProblemUI();
            button2_Click(sender, e); // Odśwież wykres
        }//comboBoxProblem_SelectedIndexChanged

    }//Koniec public partial class Form1
}//namespace ProjektRowNieLin
