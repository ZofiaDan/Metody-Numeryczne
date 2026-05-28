using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BIBLIOTEKA_NR1.NieLiniowe;
using ProjektRowNieLin.Models;

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
            BuildModernLayout();
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

        private ProblemDefinition selectedProblem;

        private TextBox textBoxEquations;
        private TextBox textBoxNewtonInfo;
        private DataGridView dataGridViewIterations;

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
            
            //Ustalenie wymiarów tabeli
            dataGridView1.RowCount = 2;
            dataGridView1.ColumnCount = 2;
            
            //Opis pierwszej i drugiej kolumny tabeli
            dataGridView1.Columns[0].HeaderCell.Value = "Rozwiązanie";
            dataGridView1.Columns[1].HeaderCell.Value = "Warunek początkowy";
            
            // Ustaw ComboBox na katalog dostępnych problemów
            comboBoxProblem.Items.Clear();
            comboBoxProblem.DataSource = ProblemCatalog.Problems;
            comboBoxProblem.DisplayMember = "DisplayName";
            comboBoxProblem.SelectedIndex = 0;
            selectedProblem = ProblemCatalog.Problems[0];
            
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
            if (selectedProblem == null) return;

            dataGridView1.Rows[0].HeaderCell.Value = selectedProblem.Variable1RowHeader;
            dataGridView1.Rows[1].HeaderCell.Value = selectedProblem.Variable2RowHeader;

            // Warunki początkowe dla danego układu
            X0[1] = selectedProblem.DefaultInitialGuess[1];
            X0[2] = selectedProblem.DefaultInitialGuess[2];
            
            //Zaktualizuj wartości wyświetlane w tabeli
            dataGridView1[1, 0].Value = X0[1].ToString();
            dataGridView1[1, 1].Value = X0[2].ToString();

            UpdateEquationAndMethodText();
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
            RNL = new MetodaNewtona(selectedProblem.Function, 2, X0, eps, 1E-8, 1E-20, 100);
            
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

                // Styling Newton trajectory series
                if (chart1.Legends.Count > 0)
                    chart1.Legends[0].Enabled = true;
                chart1.Series[2].Name = "Newton iterates";
                chart1.Series[2].MarkerSize = 9;

                //Ilustracja graficzna procesu iteracyjnego w postaci punktów na komponencie chart1
                //chart1.Series[2] jest utalony na typ Point wykresu punktowego
                for (int j = 0; j <= RNL.Nite; j++)
                {
                    chart1.Series[2].Points.AddXY(RNL.XX[j][1], RNL.XX[j][2]);
                    var pt = chart1.Series[2].Points[j];
                    if (j == 0)
                        pt.Color = Color.Green;
                    else if (j == RNL.Nite)
                        pt.Color = Color.Red;
                    else
                        pt.Color = Color.DarkRed;
                }

                UpdateIterationsGrid();
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
            if (selectedProblem == null) return;
            selectedProblem.DrawCurves(chart1);
        }//button2_Click

        /// <summary>
        /// EventHandler dla ComboBox - zmienia typ problemu i aktualizuje interfejs
        /// </summary>
        private void comboBoxProblem_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedProblem = comboBoxProblem.SelectedItem as ProblemDefinition;
            UpdateProblemUI();
            button2_Click(sender, e); // Odśwież wykres
        }//comboBoxProblem_SelectedIndexChanged

        private void BuildModernLayout()
        {
            // Recreate a cleaner layout without changing any numerical logic.
            SuspendLayout();

            // Root layout: left panel (controls) + right panel (visualization).
            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 2;
            root.RowCount = 1;
            root.Padding = new Padding(10);
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 560));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // Left column layouts with scroll
            var leftScroll = new Panel();
            leftScroll.Dock = DockStyle.Fill;
            leftScroll.AutoScroll = true;
            leftScroll.BorderStyle = BorderStyle.None;

            var left = new TableLayoutPanel();
            left.Dock = DockStyle.Top;
            left.AutoSize = true;
            left.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            left.ColumnCount = 1;
            left.RowCount = 3;
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 240));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 360));
            left.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));

            // Right column layouts
            var right = new TableLayoutPanel();
            right.Dock = DockStyle.Fill;
            right.ColumnCount = 1;
            right.RowCount = 2;
            right.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
            right.RowStyles.Add(new RowStyle(SizeType.Percent, 30));

            // Problem group
            var groupProblem = new GroupBox();
            groupProblem.Text = "Problem";
            groupProblem.Dock = DockStyle.Fill;

            var problemLayout = new TableLayoutPanel();
            problemLayout.Dock = DockStyle.Fill;
            problemLayout.ColumnCount = 1;
            problemLayout.RowCount = 3;
            problemLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            problemLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            problemLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            comboBoxProblem.Dock = DockStyle.Top;
            comboBoxProblem.Margin = new Padding(10, 8, 10, 8);

            var topButtonsLayout = new FlowLayoutPanel();
            topButtonsLayout.Dock = DockStyle.Fill;
            topButtonsLayout.FlowDirection = FlowDirection.LeftToRight;
            topButtonsLayout.WrapContents = false;
            topButtonsLayout.Margin = new Padding(6, 4, 6, 4);

            button1.Margin = new Padding(6);
            button2.Margin = new Padding(6);
            topButtonsLayout.Controls.Add(button1);
            topButtonsLayout.Controls.Add(button2);

            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Margin = new Padding(10, 0, 10, 10);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.RowHeadersWidth = 90;

            problemLayout.Controls.Add(comboBoxProblem, 0, 0);
            problemLayout.Controls.Add(topButtonsLayout, 0, 1);
            problemLayout.Controls.Add(dataGridView1, 0, 2);
            groupProblem.Controls.Add(problemLayout);

            // Equations + method group
            var groupEquations = new GroupBox();
            groupEquations.Text = "Równania i metoda";
            groupEquations.Dock = DockStyle.Fill;

            var equationsLayout = new TableLayoutPanel();
            equationsLayout.Dock = DockStyle.Fill;
            equationsLayout.ColumnCount = 1;
            equationsLayout.RowCount = 4;
            equationsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
            equationsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110));
            equationsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110));
            equationsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var labelEq = new Label();
            labelEq.Text = "Układ równań F(X)=0 (dla wybranej wersji modelu)";
            labelEq.Dock = DockStyle.Top;
            labelEq.AutoSize = true;
            labelEq.Padding = new Padding(6, 8, 6, 0);

            textBoxEquations = new TextBox();
            textBoxEquations.Dock = DockStyle.Fill;
            textBoxEquations.Multiline = true;
            textBoxEquations.ReadOnly = true;
            textBoxEquations.ScrollBars = ScrollBars.Vertical;
            textBoxEquations.BorderStyle = BorderStyle.FixedSingle;
            textBoxEquations.Font = new Font("Segoe UI", 9F);

            var labelNewton = new Label();
            labelNewton.Text = "Opis metody Newtona";
            labelNewton.Dock = DockStyle.Top;
            labelNewton.AutoSize = true;
            labelNewton.Padding = new Padding(6, 8, 6, 0);

            textBoxNewtonInfo = new TextBox();
            textBoxNewtonInfo.Dock = DockStyle.Fill;
            textBoxNewtonInfo.Multiline = true;
            textBoxNewtonInfo.ReadOnly = true;
            textBoxNewtonInfo.ScrollBars = ScrollBars.Vertical;
            textBoxNewtonInfo.BorderStyle = BorderStyle.FixedSingle;
            textBoxNewtonInfo.Font = new Font("Segoe UI", 9F);

            // Static illustrations
            pictureBox1.Dock = DockStyle.Top;
            pictureBox1.Margin = new Padding(10, 8, 10, 0);
            pictureBox1.Height = 110;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            equationsLayout.Controls.Add(labelEq, 0, 0);
            equationsLayout.Controls.Add(textBoxEquations, 0, 1);
            equationsLayout.Controls.Add(labelNewton, 0, 2);
            equationsLayout.Controls.Add(textBoxNewtonInfo, 0, 3);

            // Put pictures in a separate layout below text areas.
            var picturesLayout = new TableLayoutPanel();
            picturesLayout.Dock = DockStyle.Bottom;
            picturesLayout.ColumnCount = 1;
            picturesLayout.RowCount = 1;
            picturesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 110));
            //picturesLayout.Controls.Add(pictureBox1, 0, 0);

            groupEquations.Controls.Add(equationsLayout);
            groupEquations.Controls.Add(picturesLayout);

            // Parameters group
            var groupParameters = new GroupBox();
            groupParameters.Text = "Parametry i uruchomienie";
            groupParameters.Dock = DockStyle.Fill;

            var parametersLayout = new TableLayoutPanel();
            parametersLayout.Dock = DockStyle.Fill;
            parametersLayout.ColumnCount = 2;
            parametersLayout.RowCount = 3;
            parametersLayout.Padding = new Padding(10);
            parametersLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            parametersLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            parametersLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            parametersLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35));
            parametersLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var labelNite = new Label();
            labelNite.Text = "Liczba iteracji";
            labelNite.Dock = DockStyle.Fill;
            labelNite.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            textBox1.Dock = DockStyle.Fill;
            textBox1.ReadOnly = true;
            textBox1.Margin = new Padding(6, 6, 6, 6);

            label1.Dock = DockStyle.Fill;
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            label1.Margin = new Padding(6, 6, 6, 6);

            textBox2.Dock = DockStyle.Fill;
            textBox2.Margin = new Padding(6, 6, 6, 6);

            parametersLayout.Controls.Add(labelNite, 0, 0);
            parametersLayout.Controls.Add(textBox1, 1, 0);
            parametersLayout.Controls.Add(label1, 0, 1);
            parametersLayout.Controls.Add(textBox2, 1, 1);

            groupParameters.Controls.Add(parametersLayout);

            // Visualization group
            var groupVisualization = new GroupBox();
            groupVisualization.Text = "Wykres";
            groupVisualization.Dock = DockStyle.Fill;

            chart1.Dock = DockStyle.Fill;
            chart1.Margin = new Padding(10, 0, 10, 10);

            groupVisualization.Controls.Add(chart1);

            // Iterations grid group
            var groupIterations = new GroupBox();
            groupIterations.Text = "Historia iteracji Newtona";
            groupIterations.Dock = DockStyle.Fill;

            dataGridViewIterations = new DataGridView();
            dataGridViewIterations.Dock = DockStyle.Fill;
            dataGridViewIterations.ReadOnly = true;
            dataGridViewIterations.AllowUserToAddRows = false;
            dataGridViewIterations.AllowUserToDeleteRows = false;
            dataGridViewIterations.RowHeadersVisible = false;
            dataGridViewIterations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewIterations.Margin = new Padding(10);

            groupIterations.Controls.Add(dataGridViewIterations);

            // Assemble
            leftScroll.Controls.Add(left);
            left.Controls.Add(groupProblem, 0, 0);
            left.Controls.Add(groupEquations, 0, 1);
            left.Controls.Add(groupParameters, 0, 2);

            right.Controls.Add(groupVisualization, 0, 0);
            right.Controls.Add(groupIterations, 0, 1);

            root.Controls.Add(leftScroll, 0, 0);
            root.Controls.Add(right, 1, 0);

            // Move existing controls under the new root
            Controls.Clear();
            Controls.Add(root);

            ResumeLayout();
        }

        private void UpdateEquationAndMethodText()
        {
            if (selectedProblem == null) return;

            textBoxEquations.Text =
                $"{selectedProblem.Description}{Environment.NewLine}{Environment.NewLine}" +
                $"{selectedProblem.Equation1Text}{Environment.NewLine}" +
                $"{selectedProblem.Equation2Text}";

            textBoxNewtonInfo.Text =
                "Metoda Newtona (układ równań nieliniowych):" + Environment.NewLine +
                "1) Liczymy macierz Jacobiego J(Xk) numerycznie (różniczkowanie skończone)." + Environment.NewLine +
                "2) Rozwiązujemy układ liniowy: J(Xk) · dX = F(Xk)." + Environment.NewLine +
                "3) Aktualizujemy: X(k+1) = X(k) − dX." + Environment.NewLine + Environment.NewLine +
                "Stop kryterium: iteracja kończy się gdy suma |dX_i| < eps lub gdy przekroczono limit iteracji.";
        }

        private void UpdateIterationsGrid()
        {
            if (selectedProblem == null || RNL == null) return;
            if (dataGridViewIterations == null) return;

            dataGridViewIterations.Columns.Clear();
            dataGridViewIterations.Rows.Clear();

            string v1 = selectedProblem.Variable1Caption;
            string v2 = selectedProblem.Variable2Caption;

            dataGridViewIterations.Columns.Add("k", "k");
            dataGridViewIterations.Columns.Add("x1", v1);
            dataGridViewIterations.Columns.Add("x2", v2);
            dataGridViewIterations.Columns.Add("fnorm", "||F(X)||");
            dataGridViewIterations.Columns.Add("step", "||dX||");

            for (int k = 0; k <= RNL.Nite; k++)
            {
                double[] x = RNL.XX[k];
                double[] f = RNL.FunWektorWektor(x);
                double fnorm = Math.Sqrt(f[1] * f[1] + f[2] * f[2]);

                double step = 0.0;
                if (k > 0)
                {
                    double[] prev = RNL.XX[k - 1];
                    double dx1 = x[1] - prev[1];
                    double dx2 = x[2] - prev[2];
                    step = Math.Sqrt(dx1 * dx1 + dx2 * dx2);
                }

                dataGridViewIterations.Rows.Add(
                    k,
                    x[1].ToString("F6"),
                    x[2].ToString("F6"),
                    fnorm.ToString("F6"),
                    step.ToString("F6"));
            }
        }

    }//Koniec public partial class Form1
}//namespace ProjektRowNieLin
