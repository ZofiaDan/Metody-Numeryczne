using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MetodaGaussa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            format();
        }

        int N; //liczba równań

        void format()
        {
            N = (int)numericUpDown1.Value;
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
            format();
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
    }
}
