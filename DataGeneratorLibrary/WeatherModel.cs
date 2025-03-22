using DataGeneratorLibrary;
using Microsoft.ML;

namespace WeatherML
{
    public class WeatherModel
    {
        private readonly MLContext _mlContext;
        private readonly PredictionEngine<WeatherData, WeatherPrediction> _tempPredictor;
        private readonly PredictionEngine<WeatherData, WeatherPrediction> _humidityPredictor;
        private readonly PredictionEngine<WeatherData, WeatherPrediction> _pressurePredictor;
        private readonly PredictionEngine<WeatherData, WeatherPrediction> _visibilityPredictor;

        public WeatherModel()
        {
            _mlContext = new MLContext();

            _tempPredictor = CreateModel("temperature_model.zip", "Temperature", new[] { "Humidity", "Pressure", "Visibility" });
            _humidityPredictor = CreateModel("humidity_model.zip", "Humidity", new[] { "Temperature", "Pressure", "Visibility" });
            _pressurePredictor = CreateModel("pressure_model.zip", "Pressure", new[] { "Temperature", "Humidity", "Visibility" });
            _visibilityPredictor = CreateModel("visibility_model.zip", "Visibility", new[] { "Temperature", "Humidity", "Pressure" });
        }

        private PredictionEngine<WeatherData, WeatherPrediction> CreateModel(string modelPath, string label, string[] features)
        {
            ITransformer model;

            if (File.Exists(modelPath))
            {
                model = _mlContext.Model.Load(modelPath, out _);
            }
            else
            {
                string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "weather.csv");
                IDataView data = _mlContext.Data.LoadFromTextFile<WeatherData>(dataPath, separatorChar: ',', hasHeader: true);

                var pipeline = _mlContext.Transforms.CopyColumns("Label", label)
                    .Append(_mlContext.Transforms.Concatenate("Features", features))
                    .Append(_mlContext.Regression.Trainers.FastTree());

                model = pipeline.Fit(data);
                _mlContext.Model.Save(model, data.Schema, modelPath);
            }

            return _mlContext.Model.CreatePredictionEngine<WeatherData, WeatherPrediction>(model);
        }

        public WeatherData GenerateNextWeatherData(WeatherData previousData)
        {
            return new WeatherData
            {
                Temperature = _tempPredictor.Predict(previousData).PredictedValue,
                Humidity = _humidityPredictor.Predict(previousData).PredictedValue,
                Pressure = _pressurePredictor.Predict(previousData).PredictedValue,
                Visibility = _visibilityPredictor.Predict(previousData).PredictedValue
            };
        }

        public List<WeatherData> GenerateWeatherSequence(int count, WeatherData startData)
        {
            List<WeatherData> generatedData = new List<WeatherData> { startData };

            for (int i = 1; i < count; i++)
            {
                var nextData = GenerateNextWeatherData(generatedData[i - 1]);
                generatedData.Add(nextData);
            }

            return generatedData;
        }
    }
}
