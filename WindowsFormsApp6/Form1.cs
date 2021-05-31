using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        WeatherModel CurrentWeather;
        int[] Duration;
        int Point = 0;
        public Form1()
        {
            InitializeComponent();
            CurrentWeather = new WeatherModel();
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 10;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
        }

        private void Start_Click(object sender, EventArgs e)
        {


            if (timer1.Enabled)
            {
                double SumDuration = Duration.Sum();
                double[] RealProb = new double[3];
                for (int i = 0; i < 3; i++)
                {
                    RealProb[i] = Duration[i] / SumDuration;
                }
                chart1.Series[1].Points.AddXY(0, RealProb[0]);
                chart1.Series[1].Points.AddXY(1, RealProb[1]);
                chart1.Series[1].Points.AddXY(2, RealProb[2]);
                (bool, double) Result = CurrentWeather.GetChiSquare(RealProb, Point);
                label1.Visible = true;
                ChiValue.Text = $"ChiSquare = {Result.Item2}";
                if (Result.Item1)
                {
                    ChiBool.ForeColor = Color.Green;
                    ChiValue.Text += "< 5.991";
                    ChiBool.Text = $" is {false}";
                }
                else
                {
                    ChiBool.ForeColor = Color.Red; 
                    ChiValue.Text += "> 5.991";
                    ChiBool.Text = $" is {true}";
                }
                timer1.Stop();
                Start.Text = "Restart";
            }
            else
            {
                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();
                ChiBool.Text = "";
                ChiValue.Text = "";
                label1.Visible = false;
                chart1.ChartAreas[0].AxisX.ScrollBar.Axis.ScaleView.Position = 0;
                Point = 0;
                CurrentWeather.GetNextStage();
                chart1.Series[0].Points.AddXY(Point, (int)CurrentWeather.GetCurrentStage() + 1);
                Duration = new int[3];
                Duration[(int)CurrentWeather.GetCurrentStage()] += CurrentWeather.GetTime();
                Point++;
                timer1.Start();
                Start.Text = "Stop";
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (CurrentWeather.GetTime() == 0)
            {
                CurrentWeather.GetNextStage();
                chart1.Series[0].Points.AddXY(Point, (int)CurrentWeather.GetCurrentStage() + 1);
                Duration[(int)CurrentWeather.GetCurrentStage()] += CurrentWeather.GetTime();
                Point++;
            }
            else
            {
                chart1.Series[0].Points.AddXY(Point, (int)CurrentWeather.GetCurrentStage() + 1);
                Point++;
                CurrentWeather.DecreaseTime();
            }
            //CurrentWeather.GetNextStage();
            //chart1.Series[0].Points.AddXY(Point, (int)CurrentWeather.GetCurrentStage() + 1);
            //Duration[(int)CurrentWeather.GetCurrentStage()] += CurrentWeather.GetTime();
            //Point++;
            Console.WriteLine(CurrentWeather.GetTime());
            Console.WriteLine(CurrentWeather.GetCurrentStage());
            //CurrentWeather.GetNextStage();
            //chart1.Series[0].Points.AddXY(Point, CurrentWeather.GetCurrentStage());
            //Point++;
            if(Point > 10)
            {
                chart1.ChartAreas[0].AxisX.ScrollBar.Axis.ScaleView.Position = Point - 10;
            }
            

        }
    }
}
