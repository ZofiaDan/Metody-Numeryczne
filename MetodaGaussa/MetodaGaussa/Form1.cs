using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        double[] X, B; //wektor niewiadomych i wyrazów wolnych
        double[,] A; //macierz współczynników

       

        void format()
        {
            
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
            double x;
            for (int i = 0; i < N; i++)
            {
                for (int j=0; j<N; j++)
                {
                    x=random.Next(-100,100)+random.NextDouble();
                    dataGridView1[i,j].Value = x.ToString("0.0");
                }
                x = random.Next(-100, 100) + random.NextDouble();
                dataGridView3[0, i].Value = x.ToString("0.0");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            B = new double[N+1];
            X = new double[N+1];
            A = new double[N+1, N+1];
            int y = Gauss.Rozwiaz(A, B, X, 1e-5);

            for (int i = 0; i < N; i++)
                {
                    B[i+1] = double.Parse(dataGridView3[0,i].Value.ToString());
                    for (int j = 0; j < N; j++)
                    {
                        A[i+1, j+1] = double.Parse(dataGridView1[j, i].Value.ToString());
                }
            }

             for (int j = 0; j < N; j++)
                {
                    dataGridView2[0, j].Value = X[j + 1].ToString("0.00");
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            button3.Enabled = true;
            Random random = new Random();
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
            }
        }
    }
}
