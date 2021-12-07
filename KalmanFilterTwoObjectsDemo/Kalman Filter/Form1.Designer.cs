namespace Kalman_Filter
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
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
			this.CalculatedData_LBL = new System.Windows.Forms.Label();
			this.CalculatedPrediction_LBL = new System.Windows.Forms.Label();
			this.KalmanCorrection_LBL = new System.Windows.Forms.Label();
			this.CalculatedData_LBL2 = new System.Windows.Forms.Label();
			this.CalculatedPrediction_LBL2 = new System.Windows.Forms.Label();
			this.KalmanCorrection_LBL2 = new System.Windows.Forms.Label();
			this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.Start_BTN2 = new System.Windows.Forms.Button();
			this.timmer_UPDN = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.timmer_UPDN)).BeginInit();
			this.SuspendLayout();
			// 
			// CalculatedData_LBL
			// 
			this.CalculatedData_LBL.AutoSize = true;
			this.CalculatedData_LBL.Location = new System.Drawing.Point(517, 75);
			this.CalculatedData_LBL.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.CalculatedData_LBL.Name = "CalculatedData_LBL";
			this.CalculatedData_LBL.Size = new System.Drawing.Size(171, 25);
			this.CalculatedData_LBL.TabIndex = 3;
			this.CalculatedData_LBL.Text = "Calculated Data:";
			// 
			// CalculatedPrediction_LBL
			// 
			this.CalculatedPrediction_LBL.AutoSize = true;
			this.CalculatedPrediction_LBL.Location = new System.Drawing.Point(517, 30);
			this.CalculatedPrediction_LBL.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.CalculatedPrediction_LBL.Name = "CalculatedPrediction_LBL";
			this.CalculatedPrediction_LBL.Size = new System.Drawing.Size(222, 25);
			this.CalculatedPrediction_LBL.TabIndex = 4;
			this.CalculatedPrediction_LBL.Text = "Calculated Prediction:";
			// 
			// KalmanCorrection_LBL
			// 
			this.KalmanCorrection_LBL.AutoSize = true;
			this.KalmanCorrection_LBL.Location = new System.Drawing.Point(517, 116);
			this.KalmanCorrection_LBL.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.KalmanCorrection_LBL.Name = "KalmanCorrection_LBL";
			this.KalmanCorrection_LBL.Size = new System.Drawing.Size(195, 25);
			this.KalmanCorrection_LBL.TabIndex = 5;
			this.KalmanCorrection_LBL.Text = "Kalman Correction:";
			// 
			// CalculatedData_LBL2
			// 
			this.CalculatedData_LBL2.AutoSize = true;
			this.CalculatedData_LBL2.Location = new System.Drawing.Point(1044, 75);
			this.CalculatedData_LBL2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.CalculatedData_LBL2.Name = "CalculatedData_LBL2";
			this.CalculatedData_LBL2.Size = new System.Drawing.Size(183, 25);
			this.CalculatedData_LBL2.TabIndex = 3;
			this.CalculatedData_LBL2.Text = "Calculated Data2:";
			// 
			// CalculatedPrediction_LBL2
			// 
			this.CalculatedPrediction_LBL2.AutoSize = true;
			this.CalculatedPrediction_LBL2.Location = new System.Drawing.Point(1044, 30);
			this.CalculatedPrediction_LBL2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.CalculatedPrediction_LBL2.Name = "CalculatedPrediction_LBL2";
			this.CalculatedPrediction_LBL2.Size = new System.Drawing.Size(234, 25);
			this.CalculatedPrediction_LBL2.TabIndex = 4;
			this.CalculatedPrediction_LBL2.Text = "Calculated Prediction2:";
			// 
			// KalmanCorrection_LBL2
			// 
			this.KalmanCorrection_LBL2.AutoSize = true;
			this.KalmanCorrection_LBL2.Location = new System.Drawing.Point(1044, 116);
			this.KalmanCorrection_LBL2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.KalmanCorrection_LBL2.Name = "KalmanCorrection_LBL2";
			this.KalmanCorrection_LBL2.Size = new System.Drawing.Size(207, 25);
			this.KalmanCorrection_LBL2.TabIndex = 5;
			this.KalmanCorrection_LBL2.Text = "Kalman Correction2:";
			// 
			// chart1
			// 
			chartArea1.Name = "ChartArea1";
			this.chart1.ChartAreas.Add(chartArea1);
			legend1.Name = "Legend1";
			this.chart1.Legends.Add(legend1);
			this.chart1.Location = new System.Drawing.Point(15, 190);
			this.chart1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.chart1.Name = "chart1";
			series1.ChartArea = "ChartArea1";
			series1.Legend = "Legend1";
			series1.Name = "Object 1";
			series2.ChartArea = "ChartArea1";
			series2.Legend = "Legend1";
			series2.Name = "Kalman Prediction";
			series3.ChartArea = "ChartArea1";
			series3.Legend = "Legend1";
			series3.Name = "Object 2";
			series4.ChartArea = "ChartArea1";
			series4.Legend = "Legend1";
			series4.Name = "Kalman Prediction 2";
			this.chart1.Series.Add(series1);
			this.chart1.Series.Add(series2);
			this.chart1.Series.Add(series3);
			this.chart1.Series.Add(series4);
			this.chart1.Size = new System.Drawing.Size(1585, 816);
			this.chart1.TabIndex = 6;
			this.chart1.Text = "chart1";
			// 
			// Start_BTN2
			// 
			this.Start_BTN2.Location = new System.Drawing.Point(156, 30);
			this.Start_BTN2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.Start_BTN2.Name = "Start_BTN2";
			this.Start_BTN2.Size = new System.Drawing.Size(194, 58);
			this.Start_BTN2.TabIndex = 7;
			this.Start_BTN2.Text = "Start Random";
			this.Start_BTN2.UseVisualStyleBackColor = true;
			this.Start_BTN2.Click += new System.EventHandler(this.Start_BTN2_Click);
			// 
			// timmer_UPDN
			// 
			this.timmer_UPDN.Location = new System.Drawing.Point(222, 128);
			this.timmer_UPDN.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.timmer_UPDN.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.timmer_UPDN.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.timmer_UPDN.Name = "timmer_UPDN";
			this.timmer_UPDN.Size = new System.Drawing.Size(194, 31);
			this.timmer_UPDN.TabIndex = 8;
			this.timmer_UPDN.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.timmer_UPDN.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(128, 130);
			this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 25);
			this.label1.TabIndex = 9;
			this.label1.Text = "Interval";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1617, 1021);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.timmer_UPDN);
			this.Controls.Add(this.Start_BTN2);
			this.Controls.Add(this.chart1);
			this.Controls.Add(this.KalmanCorrection_LBL);
			this.Controls.Add(this.CalculatedPrediction_LBL);
			this.Controls.Add(this.CalculatedData_LBL);
			this.Controls.Add(this.KalmanCorrection_LBL2);
			this.Controls.Add(this.CalculatedPrediction_LBL2);
			this.Controls.Add(this.CalculatedData_LBL2);
			this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.timmer_UPDN)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        // private System.Windows.Forms.Button Start_BTN;
        private System.Windows.Forms.Label CalculatedData_LBL;
        private System.Windows.Forms.Label CalculatedPrediction_LBL;
        private System.Windows.Forms.Label KalmanCorrection_LBL;
        private System.Windows.Forms.Label CalculatedData_LBL2;
        private System.Windows.Forms.Label CalculatedPrediction_LBL2;
        private System.Windows.Forms.Label KalmanCorrection_LBL2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button Start_BTN2;
        private System.Windows.Forms.NumericUpDown timmer_UPDN;
        private System.Windows.Forms.Label label1;
    }
}

