using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6
{
    public enum WeatherStage
    {
        Clear,
        Cloudy,
        Overcast
    }
    class WeatherModel
    {
        Random Rnd = new Random();
        private double[][] QMatrix =
        {
            new[]{-.4, .3,.1},
            new[]{.4,-.8,.4},
            new[]{.1,.4,-.5},
        };
        double[] TheoryProb = { 0.33333, 0.33333, 0.33333 };

        private WeatherStage Stage = WeatherStage.Clear;
        private int Time = 0;

        private double[] GetMatrixProbability(int i)
        {
            double[] Prob = new double[QMatrix[i].Length];
            for (int j = 0; j < 3; j++)
            {
                if (i == j)
                {
                    Prob[j] = 0;
                }
                else
                {
                    Prob[j] = -(QMatrix[i][j] / QMatrix[i][i]);
                }
            }

            return Prob;
        }
        //private int Duration;

        public int GetNextStage()
        {
            int thau = (int)Math.Round(Math.Log(Rnd.NextDouble()) / QMatrix[(int)Stage][(int)Stage] * 24);
            Time += thau;
            double[] Probability = GetMatrixProbability((int)Stage);
            double q = Rnd.NextDouble();
            for (int i = 0; i <= 3; i++)
            {
                q -= Probability[i];
                if (!(q <= 0)) continue;
                Stage = (WeatherStage)i;
                break;
            }

            return (int)Stage;
        }

        public WeatherStage GetCurrentStage()
        {
            return Stage;
        }

        public int GetTime()
        {
            return Time;
        }

        public void DecreaseTime()
        {
            Time--;
        }


        public (bool,double) GetChiSquare(double[] RealPrpbability, int n)
        {

            int[] massP = new int[3];
            int[] massFreq = new int[3];
            for (int i = 0; i < 3; i++)
            {
                massP[i] = (int)(TheoryProb[i] * n);
                massFreq[i] = (int)(RealPrpbability[i] * n);
            }
            double ChiSquareEmperic = 0;

            for (int i = 0; i < 3; i++)
            {
                ChiSquareEmperic += Math.Pow((massFreq[i] - massP[i]), 2) / massP[i];
            }

            return (ChiSquareEmperic < 5.991, ChiSquareEmperic);
        }

    }
}
