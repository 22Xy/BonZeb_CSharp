using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;


namespace Kalman_Filter
{
    public partial class Form1 : Form
    {
        #region Variables
        float px, py, cx, cy, ix, iy;
        float px2, py2, cx2, cy2, ix2, iy2;
        #endregion

        #region Plot Variables
        double[] data_array_X = new double[100];
        double[] data_array_Y = new double[100];
        double[] Kalman_array_Y = new double[100];
        double[] Kalman_array_X = new double[100];
        double[] data_array_X2 = new double[100];
        double[] data_array_Y2 = new double[100];
        double[] Kalman_array_Y2 = new double[100];
        double[] Kalman_array_X2 = new double[100];
        Random rand = new Random();
        #endregion

        #region Kalman Filter and Poins Lists
        PointF[] oup = new PointF[2];
        PointF[] oup2 = new PointF[2];
        private Kalman kal;
        private SyntheticData syntheticData;
        private Kalman kal2;
        private SyntheticData syntheticData2;
        #endregion

        #region Timers
        Timer RandomWave_Timer = new Timer();
        #endregion

        public Form1()
        {
            InitializeComponent();
            InitialiseTimers();
            ix = rand.Next(40, 60);
            iy = rand.Next(40, 60);
            ix2 = rand.Next(10, 20);
            iy2 = rand.Next(10, 20);
            //set up x data for initial condition and scrolling
            data_array_X[0] = 0;
            data_array_X2[0] = 0;
            /*for (int i = 1; i < data_array_X.Length; i++)
            {
                data_array_X[i] = i;
                Kalman_array_X[i] = i;
                //data_array_Y[i] = rand.Next(100);
            }*/
            SetupChart();
        }
        private void InitialiseTimers(int Timer_Interval = 100)
        {
            RandomWave_Timer.Interval = Timer_Interval;
            RandomWave_Timer.Tick += new EventHandler(RandomWave_Timer_Tick);
        }
        public void SetupChart()
        {
            //Data 
            //Type and colour
            chart1.ChartAreas[0].AxisX.Maximum = 100;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 100;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.Series[0].Label = "Object 1";
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            chart1.Series[0].Color = Color.Blue;

            //Axis
            chart1.ChartAreas[0].AxisX.Title = "X axis";
            chart1.ChartAreas[0].AxisY.Title = "Y axis";

            // chart1.ChartAreas[0].AxisX.Minimum = 0;
            //chart1.ChartAreas[0].AxisX.Maximum = data_array_X.Length - 1;

            //Kalman
            //Type and colour
            chart1.Series[1].Label = "Kalman Data 1";
            chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart1.Series[1].Color = Color.Red;

            //Data 2
            chart1.Series[2].Label = "Object 2";
            chart1.Series[2].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            chart1.Series[2].Color = Color.Purple;

            //Kalman 2
            chart1.Series[3].Label = "Kalman Data 2";
            chart1.Series[3].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            chart1.Series[3].Color = Color.Green;

            Update();
        }
        
       
        //Setup Kalman Filter and predict methods
        public void KalmanFilter()
            {
                kal = new Kalman(4, 2, 0);
                syntheticData = new SyntheticData();
                Matrix<float> state = new Matrix<float>(new float[]
                {
                    0.0f, 0.0f, 0.0f, 0.0f
                });
                kal.CorrectedState = state;
                kal.TransitionMatrix = syntheticData.transitionMatrix;
                kal.MeasurementNoiseCovariance = syntheticData.measurementNoise;
                kal.ProcessNoiseCovariance = syntheticData.processNoise;
                kal.ErrorCovariancePost = syntheticData.errorCovariancePost;
                kal.MeasurementMatrix = syntheticData.measurementMatrix;
            }
        public void KalmanFilter2()
        {
            kal2 = new Kalman(4, 2, 0);
            syntheticData2 = new SyntheticData();
            Matrix<float> state = new Matrix<float>(new float[]
            {
                    0.0f, 0.0f, 0.0f, 0.0f
            });
            kal2.CorrectedState = state;
            kal2.TransitionMatrix = syntheticData2.transitionMatrix;
            kal2.MeasurementNoiseCovariance = syntheticData2.measurementNoise;
            kal2.ProcessNoiseCovariance = syntheticData2.processNoise;
            kal2.ErrorCovariancePost = syntheticData2.errorCovariancePost;
            kal2.MeasurementMatrix = syntheticData2.measurementMatrix;
        }
        public PointF[] filterPoints(PointF pt)
        {
            syntheticData.state[0, 0] = pt.X;
            syntheticData.state[1, 0] = pt.Y;
            Matrix<float> prediction = kal.Predict();
            PointF predictPoint = new PointF(prediction[0, 0], prediction[1, 0]);
            PointF measurePoint = new PointF(syntheticData.GetMeasurement()[0, 0],
                syntheticData.GetMeasurement()[1, 0]);
            Matrix<float> estimated = kal.Correct(syntheticData.GetMeasurement());
            PointF estimatedPoint = new PointF(estimated[0, 0], estimated[1, 0]);
            syntheticData.GoToNextState();
            PointF[] results = new PointF[2];
            results[0] = predictPoint;
            results[1] = estimatedPoint;
            px = predictPoint.X;
            py = predictPoint.Y;
            cx = estimatedPoint.X;
            cy = estimatedPoint.Y;
            return results;
        }

        public PointF[] filterPoints2(PointF pt)
        {
            syntheticData2.state[0, 0] = pt.X;
            syntheticData2.state[1, 0] = pt.Y;
            Matrix<float> prediction = kal2.Predict();
            PointF predictPoint = new PointF(prediction[0, 0], prediction[1, 0]);
            PointF measurePoint = new PointF(syntheticData2.GetMeasurement()[0, 0],
                syntheticData2.GetMeasurement()[1, 0]);
            Matrix<float> estimated = kal2.Correct(syntheticData2.GetMeasurement());
            PointF estimatedPoint = new PointF(estimated[0, 0], estimated[1, 0]);
            syntheticData2.GoToNextState();
            PointF[] results = new PointF[2];
            results[0] = predictPoint;
            results[1] = estimatedPoint;
            px2 = predictPoint.X;
            py2 = predictPoint.Y;
            cx2 = estimatedPoint.X;
            cy2 = estimatedPoint.Y;
            return results;
        }

        //Initialse Wave Function

        private void Start_BTN2_Click(object sender, EventArgs e)
        {
            if (Start_BTN2.Text == "Start Random")
            {
                KalmanFilter(); //initialize kalman filter
                KalmanFilter2(); //initialize kalman filter
                RandomWave_Timer.Start();
                Start_BTN2.Text = "Stop Random";
                timmer_UPDN.Enabled = false;
            }
            else
            {
                RandomWave_Timer.Stop();
                Start_BTN2.Text = "Start Random";
                timmer_UPDN.Enabled = true;
            }
        }

        public void RandomWave_Timer_Tick(object sender, EventArgs e)
        {
            var val = rand.Next(1, 9);
            var move = rand.Next(4, 8);
            if (val == 1) {
                if (ix + move >= 100) ix = 45; else ix += move;
            } else if (val == 2) {
                if (ix - move <= 0) ix = 5; else ix -= move;
            } else if (val == 3) {
                if (iy + move >= 100) iy = 45; else iy += move;
            } else if (val == 4) {
                if (iy - move <= 0) iy = 5; else iy -= move;
            } else if (val == 5)
            {
                if (ix - move <= 0) ix = 5; else ix -= move;
                if (iy + move >= 100) iy = 45; else iy += move;
            }
            else if (val == 6)
            {
                if (ix + move >= 100) ix = 45; else ix += move;
                if (iy + move >= 100) iy = 45; else iy += move;
            }
            else if (val == 7)
            {
                if (ix - move <= 0) ix = 5; else ix -= move;
                if (iy - move <= 0) iy = 5; else iy -= move;
            }
            else
            {
                if (ix + move >= 100) ix = 45; else ix += move;
                if (iy - move <= 0) iy = 5; else iy -= move;
            }

            val = rand.Next(1, 9);
            move = rand.Next(4, 8);
            if (val == 1)
            {
                if (ix2 + move >= 100) ix2 = 45; else ix2 += move;
            }
            else if (val == 2)
            {
                if (ix2 - move <= 0) ix2 = 5; else ix2 -= move;
            }
            else if (val == 3)
            {
                if (iy2 + move >= 100) iy2 = 45; else iy2 += move;
            }
            else if (val == 4)
            {
                if (iy2 - move <= 0) iy2 = 5; else iy2 -= move;
            }
            else if (val == 5)
            {
                if (ix2 - move <= 0) ix2 = 5; else ix2 -= move;
                if (iy2 + move >= 100) iy2 = 45; else iy2 += move;
            }
            else if (val == 6)
            {
                if (ix2 + move >= 100) ix2 = 45; else ix2 += move;
                if (iy2 + move >= 100) iy2 = 45; else iy2 += move;
            }
            else if (val == 7)
            {
                if (ix2 - move <= 0) ix2 = 5; else ix2 -= move;
                if (iy2 - move <= 0) iy2 = 5; else iy2 -= move;
            }
            else
            {
                if (ix2 + move >= 100) ix2 = 45; else ix2 += move;
                if (iy2 - move <= 0) iy2 = 5; else iy2 -= move;
            }

            //set data
            Array.Copy(data_array_Y, 1, data_array_Y, 0, 1);
            data_array_Y[data_array_Y.Length - 1] = iy;
            Array.Copy(data_array_X, 1, data_array_X, 0, 1);
            data_array_X[data_array_X.Length - 1] = ix;

            Array.Copy(data_array_Y2, 1, data_array_Y2, 0, 1);
            data_array_Y2[data_array_Y2.Length - 1] = iy2;
            Array.Copy(data_array_X2, 1, data_array_X2, 0, 1);
            data_array_X2[data_array_X2.Length - 1] = ix2;

            //display data
            CalculatedData_LBL.Text = "Calculated Data - X:" + ix.ToString() + " Y:" + iy.ToString();
            CalculatedData_LBL2.Text = "Calculated Data - X2:" + ix2.ToString() + " Y2:" + iy2.ToString();

            //update kalman and predict
            PointF inp = new PointF(ix, iy);
            oup = new PointF[2];
            oup = filterPoints(inp);

            PointF[] pts = oup;

            //update kalman and predict
            PointF inp2 = new PointF(ix2, iy2);
            oup2 = new PointF[2];
            oup2 = filterPoints2(inp2);

            PointF[] pts2 = oup2;

            KalmanCorrection_LBL.Text = "Kalman Correction - X:" + ((int)cx).ToString() + " Y:" + ((int)cy).ToString();
            CalculatedPrediction_LBL.Text = "Calculated Prediction - X:" + ((int)px).ToString() + " Y:" + ((int)py).ToString();

            KalmanCorrection_LBL2.Text = "Kalman Correction - X2:" + ((int)cx2).ToString() + " Y2:" + ((int)cy2).ToString();
            CalculatedPrediction_LBL2.Text = "Calculated Prediction - X2:" + ((int)px2).ToString() + " Y2:" + ((int)py2).ToString();

            //Set kalman Data
            Array.Copy(Kalman_array_Y, 1, Kalman_array_Y, 0, 99);
            Kalman_array_Y[Kalman_array_Y.Length - 1] = cy;
            Array.Copy(Kalman_array_X, 1, Kalman_array_X, 0, 99);
            Kalman_array_X[Kalman_array_X.Length - 1] = cx;

            //Set kalman Data
            Array.Copy(Kalman_array_Y2, 1, Kalman_array_Y2, 0, 99);
            Kalman_array_Y2[Kalman_array_Y2.Length - 1] = cy2;
            Array.Copy(Kalman_array_X2, 1, Kalman_array_X2, 0, 99);
            Kalman_array_X2[Kalman_array_X2.Length - 1] = cx2;

            Update();
            this.Refresh();
        }

        double amplitude = 100;
        double frequency = 500;

        public void Update()
        {
            // chart1.ChartAreas[0].AxisX.Minimum = data_array_X[0];
            chart1.Series[0].Points.DataBindXY(data_array_X, data_array_Y);
            chart1.Series[1].Points.DataBindXY(Kalman_array_X, Kalman_array_Y);
            chart1.Series[2].Points.DataBindXY(data_array_X2, data_array_Y2);
            chart1.Series[3].Points.DataBindXY(Kalman_array_X2, Kalman_array_Y2);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            InitialiseTimers((int)timmer_UPDN.Value);
        }


    }
}
