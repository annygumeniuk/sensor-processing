using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGeneratorLibrary.PredictionModels
{
    public class WeatherPredictionVisibility
    {
        [ColumnName("Score")]
        public float PredictedVisibility { get; set; }
    }
}
