using Microsoft.ML.Data;
namespace DataGeneratorLibrary
{
    public class WeatherPredictionTemp
    {
        [ColumnName("Score")]
        public float PredictedTemperature { get; set; }
    }
}
