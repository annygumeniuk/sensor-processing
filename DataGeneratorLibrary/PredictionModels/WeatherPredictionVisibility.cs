using Microsoft.ML.Data;
namespace DataGeneratorLibrary.PredictionModels
{
    public class WeatherPredictionVisibility
    {
        [ColumnName("Score")]
        public float PredictedVisibility { get; set; }
    }
}
