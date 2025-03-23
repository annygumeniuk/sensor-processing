using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGeneratorLibrary
{
    public class WeatherPredictionPressure
    {
        [ColumnName("Score")]
        public float PredictedPressure { get; set; }
    }
}
