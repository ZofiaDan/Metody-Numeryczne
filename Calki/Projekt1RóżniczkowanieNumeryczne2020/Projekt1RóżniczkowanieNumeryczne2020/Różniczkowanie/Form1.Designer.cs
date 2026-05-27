namespace Rozniczkowanie
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.labelAbout = new System.Windows.Forms.Label();
            this.groupBoxInputs = new System.Windows.Forms.GroupBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonSolve = new System.Windows.Forms.Button();
            this.textBoxV0 = new System.Windows.Forms.TextBox();
            this.labelV0 = new System.Windows.Forms.Label();
            this.textBoxX0 = new System.Windows.Forms.TextBox();
            this.labelX0 = new System.Windows.Forms.Label();
            this.textBoxDt = new System.Windows.Forms.TextBox();
            this.labelDt = new System.Windows.Forms.Label();
            this.textBoxTEnd = new System.Windows.Forms.TextBox();
            this.labelTEnd = new System.Windows.Forms.Label();
            this.textBoxT0 = new System.Windows.Forms.TextBox();
            this.labelT0 = new System.Windows.Forms.Label();
            this.textBoxMu = new System.Windows.Forms.TextBox();
            this.labelMu = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.groupBoxInputs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelAbout
            // 
            this.labelAbout.AutoSize = true;
            this.labelAbout.Location = new System.Drawing.Point(12, 9);
            this.labelAbout.Name = "labelAbout";
            this.labelAbout.Size = new System.Drawing.Size(948, 52);
            this.labelAbout.TabIndex = 0;
            this.labelAbout.Text = "Van der Pol oscillator: x'' - mu(1 - x^2)x' + x = 0\r\nSystem form: x' = v, v' = mu(1 - x^2)v - x\r\nSet parameters below and click Solve to integrate with fixed-step RK4.";
            // 
            // groupBoxInputs
            // 
            this.groupBoxInputs.Controls.Add(this.buttonReset);
            this.groupBoxInputs.Controls.Add(this.buttonSolve);
            this.groupBoxInputs.Controls.Add(this.textBoxV0);
            this.groupBoxInputs.Controls.Add(this.labelV0);
            this.groupBoxInputs.Controls.Add(this.textBoxX0);
            this.groupBoxInputs.Controls.Add(this.labelX0);
            this.groupBoxInputs.Controls.Add(this.textBoxDt);
            this.groupBoxInputs.Controls.Add(this.labelDt);
            this.groupBoxInputs.Controls.Add(this.textBoxTEnd);
            this.groupBoxInputs.Controls.Add(this.labelTEnd);
            this.groupBoxInputs.Controls.Add(this.textBoxT0);
            this.groupBoxInputs.Controls.Add(this.labelT0);
            this.groupBoxInputs.Controls.Add(this.textBoxMu);
            this.groupBoxInputs.Controls.Add(this.labelMu);
            this.groupBoxInputs.Location = new System.Drawing.Point(15, 72);
            this.groupBoxInputs.Name = "groupBoxInputs";
            this.groupBoxInputs.Size = new System.Drawing.Size(1049, 86);
            this.groupBoxInputs.TabIndex = 1;
            this.groupBoxInputs.TabStop = false;
            this.groupBoxInputs.Text = "Parameters";
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(944, 33);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(90, 30);
            this.buttonReset.TabIndex = 13;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonSolve
            // 
            this.buttonSolve.Location = new System.Drawing.Point(848, 33);
            this.buttonSolve.Name = "buttonSolve";
            this.buttonSolve.Size = new System.Drawing.Size(90, 30);
            this.buttonSolve.TabIndex = 12;
            this.buttonSolve.Text = "Solve";
            this.buttonSolve.UseVisualStyleBackColor = true;
            this.buttonSolve.Click += new System.EventHandler(this.buttonSolve_Click);
            // 
            // textBoxV0
            // 
            this.textBoxV0.Location = new System.Drawing.Point(733, 39);
            this.textBoxV0.Name = "textBoxV0";
            this.textBoxV0.Size = new System.Drawing.Size(89, 20);
            this.textBoxV0.TabIndex = 11;
            // 
            // labelV0
            // 
            this.labelV0.AutoSize = true;
            this.labelV0.Location = new System.Drawing.Point(649, 42);
            this.labelV0.Name = "labelV0";
            this.labelV0.Size = new System.Drawing.Size(78, 13);
            this.labelV0.TabIndex = 10;
            this.labelV0.Text = "v0 = x'(0)";
            // 
            // textBoxX0
            // 
            this.textBoxX0.Location = new System.Drawing.Point(554, 39);
            this.textBoxX0.Name = "textBoxX0";
            this.textBoxX0.Size = new System.Drawing.Size(89, 20);
            this.textBoxX0.TabIndex = 9;
            // 
            // labelX0
            // 
            this.labelX0.AutoSize = true;
            this.labelX0.Location = new System.Drawing.Point(521, 42);
            this.labelX0.Name = "labelX0";
            this.labelX0.Size = new System.Drawing.Size(27, 13);
            this.labelX0.TabIndex = 8;
            this.labelX0.Text = "x(0)";
            // 
            // textBoxDt
            // 
            this.textBoxDt.Location = new System.Drawing.Point(426, 39);
            this.textBoxDt.Name = "textBoxDt";
            this.textBoxDt.Size = new System.Drawing.Size(89, 20);
            this.textBoxDt.TabIndex = 7;
            // 
            // labelDt
            // 
            this.labelDt.AutoSize = true;
            this.labelDt.Location = new System.Drawing.Point(406, 42);
            this.labelDt.Name = "labelDt";
            this.labelDt.Size = new System.Drawing.Size(14, 13);
            this.labelDt.TabIndex = 6;
            this.labelDt.Text = "dt";
            // 
            // textBoxTEnd
            // 
            this.textBoxTEnd.Location = new System.Drawing.Point(300, 39);
            this.textBoxTEnd.Name = "textBoxTEnd";
            this.textBoxTEnd.Size = new System.Drawing.Size(89, 20);
            this.textBoxTEnd.TabIndex = 5;
            // 
            // labelTEnd
            // 
            this.labelTEnd.AutoSize = true;
            this.labelTEnd.Location = new System.Drawing.Point(264, 42);
            this.labelTEnd.Name = "labelTEnd";
            this.labelTEnd.Size = new System.Drawing.Size(30, 13);
            this.labelTEnd.TabIndex = 4;
            this.labelTEnd.Text = "tEnd";
            // 
            // textBoxT0
            // 
            this.textBoxT0.Location = new System.Drawing.Point(169, 39);
            this.textBoxT0.Name = "textBoxT0";
            this.textBoxT0.Size = new System.Drawing.Size(89, 20);
            this.textBoxT0.TabIndex = 3;
            // 
            // labelT0
            // 
            this.labelT0.AutoSize = true;
            this.labelT0.Location = new System.Drawing.Point(145, 42);
            this.labelT0.Name = "labelT0";
            this.labelT0.Size = new System.Drawing.Size(18, 13);
            this.labelT0.TabIndex = 2;
            this.labelT0.Text = "t0";
            // 
            // textBoxMu
            // 
            this.textBoxMu.Location = new System.Drawing.Point(43, 39);
            this.textBoxMu.Name = "textBoxMu";
            this.textBoxMu.Size = new System.Drawing.Size(89, 20);
            this.textBoxMu.TabIndex = 1;
            // 
            // labelMu
            // 
            this.labelMu.AutoSize = true;
            this.labelMu.Location = new System.Drawing.Point(13, 42);
            this.labelMu.Name = "labelMu";
            this.labelMu.Size = new System.Drawing.Size(24, 13);
            this.labelMu.TabIndex = 0;
            this.labelMu.Text = "mu";
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(15, 175);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(1049, 457);
            this.chart1.TabIndex = 2;
            this.chart1.Text = "chart1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 647);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.groupBoxInputs);
            this.Controls.Add(this.labelAbout);
            this.Name = "Form1";
            this.Text = "Van der Pol Oscillator Solver";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBoxInputs.ResumeLayout(false);
            this.groupBoxInputs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAbout;
        private System.Windows.Forms.GroupBox groupBoxInputs;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonSolve;
        private System.Windows.Forms.TextBox textBoxV0;
        private System.Windows.Forms.Label labelV0;
        private System.Windows.Forms.TextBox textBoxX0;
        private System.Windows.Forms.Label labelX0;
        private System.Windows.Forms.TextBox textBoxDt;
        private System.Windows.Forms.Label labelDt;
        private System.Windows.Forms.TextBox textBoxTEnd;
        private System.Windows.Forms.Label labelTEnd;
        private System.Windows.Forms.TextBox textBoxT0;
        private System.Windows.Forms.Label labelT0;
        private System.Windows.Forms.TextBox textBoxMu;
        private System.Windows.Forms.Label labelMu;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}

