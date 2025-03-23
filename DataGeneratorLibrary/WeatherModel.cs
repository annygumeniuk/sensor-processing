using System;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace DataGeneratorLibrary
{
    public class WeatherModel
    {        
        private readonly MLContext mlContext;
        private readonly ITransformer model;
        private readonly PredictionEngine<WeatherData, WeatherPredictionTemp> predictor;

        public WeatherModel()
        {
            mlContext = new MLContext();
            var data = mlContext.Data.LoadFromTextFile<WeatherData>(_trainingFilePath, separatorChar: ',', hasHeader: true);
            
            var pipeline = mlContext.Transforms.Concatenate("Features", new[] { "Humidity", "Pressure", "Visibility" })
                           .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Temperature", maximumNumberOfIterations: 100));

            Console.WriteLine("Training is started.");
            
            model = pipeline.Fit(data);

            Console.WriteLine("Creating prediction for temperature");
            predictor = mlContext.Model.CreatePredictionEngine<WeatherData, WeatherPredictionTemp>(model);
        }

        public WeatherData GenerateFromModel(float humidity, float pressure, float visibility)
        {
            var prediction = predictor.Predict(new WeatherData
            {
                Humidity = humidity,
                Pressure = pressure,
                Visibility = visibility
            });

            return new WeatherData
            {
                Temperature = prediction.PredictedTemperature,
                Humidity = humidity,
                Pressure = pressure,
                Visibility = visibility
            };
        }
    }    
}
