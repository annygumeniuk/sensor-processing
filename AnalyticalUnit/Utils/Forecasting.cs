using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticalUnit.Utils
{
    /// <summary>
    /// This class implements calculation of AutoRegressive Integrated Moving Average
    /// </summary>
    public static class Forecasting
    {
        public static List<double> MovingAverageForecast(List<double> data, int windowSize)
        {
            var forecast = new List<double>();
            for (int i = 0; i < data.Count - windowSize; i++)
            {
                var avg = data.Skip(i).Take(windowSize).Average();
                forecast.Add(avg);
            }
            return forecast;
        }
    }
}
