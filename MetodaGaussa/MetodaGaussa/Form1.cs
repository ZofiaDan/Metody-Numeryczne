using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BIBLIOTEKA_NR1.Liniowe;

namespace MetodaGaussa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            N = 5;
            format();
        }

        int N; //liczba równań
        bool useComplex = false; //czy używać liczb zespolonych
        
        //Zmienne dla liczb rzeczywistych
        double[] X_double, B_double;
        double[,] A_double;
        
        //Zmienne dla liczb zespolonych
        Complex[] X_complex, B_complex;
        Complex[,] A_complex;

        void format()
        {
            if (useComplex)
            {
                //Inicjalizuj zmienne dla liczb zespolonych
                A_complex = new Complex[N + 1, N + 1];
                B_complex = new Complex[N + 1];
                X_complex = new Complex[N + 1];
            }
            else
            {
                //Inicjalizuj zmienne dla liczb rzeczywistych
                A_double = new double[N + 1, N + 1];
                B_double = new double[N + 1];
                X_double = new double[N + 1];
            }
            
            // macierz
            dataGridView1.ColumnCount = N;
            dataGridView1.RowCount = N;
            // wektor B i X
            dataGridView2.ColumnCount = 1;
            dataGridView2.RowCount = N;

            dataGridView3.ColumnCount = 1;
            dataGridView3.RowCount = N;
        }
        
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            N = (int)numericUpDown1.Value;
            trackBar1.Value = N;
            format();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            Random random = new Random();

            if (useComplex)
            {
                //Generuj liczby zespolone
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        double realPart = random.Next(-100, 100) + random.NextDouble();
                        double imagPart = random.Next(-100, 100) + random.NextDouble();
                        Complex c = new Complex(realPart, imagPart);
                        dataGridView1[i, j].Value = FormatComplex(c);
                    }
                    double realB = random.Next(-100, 100) + random.NextDouble();
                    double imagB = random.Next(-100, 100) + random.NextDouble();
                    Complex cB = new Complex(realB, imagB);
                    dataGridView3[0, i].Value = FormatComplex(cB);
                }
            }
            else
            {
                //Generuj liczby rzeczywiste
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        double x = random.Next(-100, 100) + random.NextDouble();
                        dataGridView1[i, j].Value = x.ToString("0.0");
                    }
                    double xB = random.Next(-100, 100) + random.NextDouble();
                    dataGridView3[0, i].Value = xB.ToString("0.0");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (useComplex)
            {
                B_complex = new Complex[N + 1];
                X_complex = new Complex[N + 1];
                A_complex = new Complex[N + 1, N + 1];

                for (int i = 0; i < N; i++)
                {
                    //Parsuj liczbę zespoloną z DataGridView
                    string valueStr = dataGridView3[0, i].Value.ToString();
                    B_complex[i + 1] = ParseComplex(valueStr);
                    
                    for (int j = 0; j < N; j++)
                    {
                        string cellStr = dataGridView1[j, i].Value.ToString();
                        A_complex[i + 1, j + 1] = ParseComplex(cellStr);
                    }
                }
                
                int y = Gauss.RozwiazComplex(A_complex, B_complex, X_complex, 1e-5);

                for (int j = 0; j < N; j++)
                {
                    dataGridView2[0, j].Value = FormatComplex(X_complex[j + 1]);
                }
            }
            else
            {
                B_double = new double[N + 1];
                X_double = new double[N + 1];
                A_double = new double[N + 1, N + 1];

                for (int i = 0; i < N; i++)
                {
                    B_double[i + 1] = double.Parse(dataGridView3[0, i].Value.ToString());
                    for (int j = 0; j < N; j++)
                    {
                        A_double[i + 1, j + 1] = double.Parse(dataGridView1[j, i].Value.ToString());
                    }
                }
                
                int y = Gauss.Rozwiaz(A_double, B_double, X_double, 1e-5);

                for (int j = 0; j < N; j++)
                {
                    dataGridView2[0, j].Value = X_double[j + 1].ToString("0.00");
                }
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            N = trackBar1.Value;
            numericUpDown1.Value = N;
            format();
        }

        private void radioButtonReal_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonReal.Checked)
            {
                useComplex = false;
                format();
                ClearDataGridViews();
            }
        }

        private void radioButtonComplex_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonComplex.Checked)
            {
                useComplex = true;
                format();
                ClearDataGridViews();
            }
        }

        void ClearDataGridViews()
        {
            //Wyczyść dataGridView1
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    dataGridView1[j, i].Value = null;
                }
            }

            //Wyczyść dataGridView2
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                dataGridView2[0, i].Value = null;
            }

            //Wyczyść dataGridView3
            for (int i = 0; i < dataGridView3.RowCount; i++)
            {
                dataGridView3[0, i].Value = null;
            }
        }

        //Formatuj liczbę zespoloną do postaci "a + bi"
        string FormatComplex(Complex c)
        {
            if (c.Imaginary >= 0)
                return string.Format("{0:0.0} + {1:0.0}i", c.Real, c.Imaginary);
            else
                return string.Format("{0:0.0} - {1:0.0}i", c.Real, -c.Imaginary);
        }

        //Sparsuj liczbę zespoloną z postaci "a + bi" lub "a - bi"
        Complex ParseComplex(string str)
        {
            str = str.Replace(" ", ""); //Usuń spacje
            
            if (!str.Contains("i"))
            {
                //Jeśli brak 'i', to liczba rzeczywista
                return new Complex(double.Parse(str), 0);
            }
            
            //Usuń 'i' z końca
            str = str.Substring(0, str.Length - 1);
            
            int plusIndex = str.LastIndexOf('+');
            int minusIndex = str.LastIndexOf('-');
            
            int splitIndex = Math.Max(plusIndex, minusIndex);
            
            if (splitIndex <= 0)
            {
                //Tylko część urojona (np. "3i")
                return new Complex(0, double.Parse(str));
            }
            
            double real = double.Parse(str.Substring(0, splitIndex));
            double imag = double.Parse(str.Substring(splitIndex));
            
            return new Complex(real, imag);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            button3.Enabled = true;
            Random random = new Random();
            
            if (useComplex)
            {
                double r_value_real, r_value_imag, sumator_real = 0, sumator_imag = 0;
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        r_value_real = random.Next(-21, 21);
                        r_value_imag = random.Next(-21, 21);
                        Complex c = new Complex(r_value_real, r_value_imag);
                        dataGridView1[i, j].Value = FormatComplex(c);
                        sumator_real += r_value_real;
                        sumator_imag += r_value_imag;
                    }
                    Complex sumC = new Complex(sumator_real, sumator_imag);
                    dataGridView3[0, i].Value = FormatComplex(sumC);
                    sumator_real = 0;
                    sumator_imag = 0;
                }
            }
            else
            {
                double r_value, sumator = 0;
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < N; j++)
                    {
                        r_value = random.Next(-21, 21);
                        dataGridView1[i, j].Value = r_value.ToString("0.00");
                        sumator += r_value;
                    }
                    dataGridView3[0, i].Value = sumator;
                    sumator = 0;
                }
            }
        }
    }
}
