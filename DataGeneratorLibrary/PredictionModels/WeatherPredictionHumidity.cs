using Microsoft.ML.Data;
namespace DataGeneratorLibrary
{
    public class WeatherPredictionHumidity
    {
        [ColumnName("Score")]
        public float PredictedHumidity { get; set; }
    }
}
