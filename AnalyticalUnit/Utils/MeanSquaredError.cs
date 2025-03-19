using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticalUnit.Utils
{
    public static class MeanSquaredError
    {
        public static double CalculateMSE(List<double> actual, List<double> predicted)
        {
            if (actual.Count != predicted.Count) throw new ArgumentException("Lists must be of equal length");

            double mse = actual.Zip(predicted, (a, p) => Math.Pow(a - p, 2)).Average();
            return mse;
        }
    }
}
