using Microsoft.ML.Data;
namespace DataGeneratorLibrary
{
    public class WeatherPredictionPressure
    {
        [ColumnName("Score")]
        public float PredictedPressure { get; set; }
    }
}
