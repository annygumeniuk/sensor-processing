using Microsoft.ML;
using DataGeneratorLibrary.PredictionModels;

namespace DataGeneratorLibrary
{    
    public class WeatherModel
    {
        public void Generate()
        {
            var mlContext = new MLContext();
            string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "weather.csv");

            // Load the dataset
            IDataView dataView = mlContext.Data.LoadFromTextFile<WeatherData>(
                dataPath, separatorChar: ',', hasHeader: true);

            // Get the last known weather data
            var lastWeather = mlContext.Data.CreateEnumerable<WeatherData>(dataView, reuseRowObject: false).Last();

            // Predict each parameter separately
            float predictedTemp = TrainAndPredict<WeatherData, WeatherPredictionTemp>(mlContext, dataView, "Temperature", lastWeather);
            float predictedHumidity = TrainAndPredict<WeatherData, WeatherPredictionHumidity>(mlContext, dataView, "Humidity", lastWeather);
            float predictedPressure = TrainAndPredict<WeatherData, WeatherPredictionPressure>(mlContext, dataView, "Pressure", lastWeather);
            float predictedVisibility = TrainAndPredict<WeatherData, WeatherPredictionVisibility>(mlContext, dataView, "Visibility", lastWeather);

            Console.WriteLine($"Generated Weather Data:");
            Console.WriteLine($"Temperature: {predictedTemp:F2}");
            Console.WriteLine($"Humidity: {predictedHumidity:F2}");
            Console.WriteLine($"Pressure: {predictedPressure:F2}");
            Console.WriteLine($"Visibility: {predictedVisibility:F2}");
        }

        /// <summary>
        /// Generic method to train a model and predict a single weather parameter
        /// </summary>
        static float TrainAndPredict<TData, TPrediction>(MLContext mlContext, IDataView dataView, string labelColumn, WeatherData lastWeather)
        where TData : class
        where TPrediction : class, new()
        {
            // Create training pipeline
            var pipeline = mlContext.Transforms.Concatenate("Features", new[] { "Temperature", "Humidity", "Pressure", "Visibility" })
                .Append(mlContext.Regression.Trainers.FastTree(labelColumnName: labelColumn));

            // Train the model
            var model = pipeline.Fit(dataView);

            // Create a prediction engine
            var predictionEngine = mlContext.Model.CreatePredictionEngine<WeatherData, TPrediction>(model);

            // Predict the value
            var prediction = predictionEngine.Predict(lastWeather);

            // Get the predicted value using reflection
            return (float)typeof(TPrediction).GetProperty($"Predicted{labelColumn}").GetValue(prediction);
        }
    }    
}
