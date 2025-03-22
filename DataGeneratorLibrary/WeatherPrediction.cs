using Microsoft.ML.Data;
namespace DataGeneratorLibrary
{
    public class WeatherPrediction
    {
        [ColumnName("Score")]
        public float PredictedValue { get; set; }
    }
}
