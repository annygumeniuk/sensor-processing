using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticalUnit.Models
{
    public class ForecastResult
    {
        public Dictionary<string, List<WeatherPoint>> Forecasts { get; set; }
        public Dictionary<string, double> MseValues { get; set; }
    }
}
