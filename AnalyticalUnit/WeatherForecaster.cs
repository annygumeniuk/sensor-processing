using AnalyticalUnit.Models;

namespace AnalyticalUnit.Utils
{
    public class WeatherForecaster
    {
        // Model parameters
        private int p; // AR order
        private int d; // Integration order
        private int q; // MA order

        // Weather parameter bounds
        private Dictionary<string, (double Min, double Max)> parameterBounds = new Dictionary<string, (double Min, double Max)>
        {
            { "Temperature", (-50.0, 60.0) },          // Realistic global temperature range in °C
            { "Visibility", (0.0, 100.0) },            // Visibility percentage
            { "Humidity", (0.0, 100.0) },              // Humidity percentage
            { "AtmosphericPressure", (870.0, 1085.0) } // Standard atmospheric pressure range in hPa
        };

        /// <summary>
        /// Constructor with configurable ARIMA parameters
        /// </summary>
        public WeatherForecaster(int autoRegressiveOrder = 3, int integrationOrder = 1, int movingAverageOrder = 1)
        {
            p = autoRegressiveOrder;
            d = integrationOrder;
            q = movingAverageOrder;
        }

        /// <summary>
        /// Main forecasting method that processes CSV data and returns forecasts
        /// </summary>
        public ForecastResult ForecastWeather(string csvFilePath, int forecastHours = 24)
        {
            // Parse CSV data
            var sensorData = CsvSensorParser.ParseCsv(csvFilePath);

            // Group by sensor type
            var groupedData = sensorData.GroupBy(x => x.Name).ToDictionary(g => g.Key, g => g.ToList());

            // Create forecasts for each weather parameter
            var forecasts = new Dictionary<string, List<WeatherPoint>>();
            var mseResults = new Dictionary<string, double>();

            foreach (var sensorGroup in groupedData)
            {
                string sensorName = sensorGroup.Key;
                var sortedData = sensorGroup.Value.OrderBy(x => x.DateTime).ToList();

                // Extract time series
                var timePoints = sortedData.Select(x => x.DateTime).ToList();
                var values = sortedData.Select(x => x.Value).ToList();
                var valuesConverted = values.Select(item => Convert.ToDouble(item)).ToList();

                // Check if we have bounds for this parameter
                (double min, double max) bounds = (double.MinValue, double.MaxValue);
                if (parameterBounds.ContainsKey(sensorName))
                {
                    bounds = parameterBounds[sensorName];
                }

                // Generate forecast with improved methods
                var forecastResult = GenerateConstrainedForecast(valuesConverted, timePoints, forecastHours, bounds, sensorName);

                // Calculate MSE for this parameter
                var testSize = Math.Min(values.Count / 4, 6); // Use last quarter or at least 6 points for testing
                var mse = CalculateMseOnTestSet(valuesConverted, timePoints, testSize, bounds, sensorName);

                // Store results
                forecasts.Add(sensorName, forecastResult);
                mseResults.Add(sensorName, mse);
            }

            return new ForecastResult
            {
                Forecasts = forecasts,
                MseValues = mseResults
            };
        }

        /// <summary>
        /// Generate constrained forecast for a specific weather parameter
        /// </summary>
        private List<WeatherPoint> GenerateConstrainedForecast(
            List<double> values,
            List<DateTime> timePoints,
            int forecastHours,
            (double Min, double Max) bounds,
            string parameterName)
        {
            // Create a result list
            var forecastPoints = new List<WeatherPoint>();

            // Handle different parameters with specialized approaches
            switch (parameterName)
            {
                case "Temperature":
                    forecastPoints = ForecastTemperature(values, timePoints, forecastHours);
                    break;

                case "Humidity":
                    forecastPoints = ForecastHumidity(values, timePoints, forecastHours);
                    break;

                case "Visibility":
                    forecastPoints = ForecastVisibility(values, timePoints, forecastHours);
                    break;

                case "AtmosphericPressure":
                    forecastPoints = ForecastPressure(values, timePoints, forecastHours);
                    break;

                default:
                    // Generic ARIMA with bounds for unknown parameters
                    forecastPoints = GenericARIMAForecast(values, timePoints, forecastHours, bounds);
                    break;
            }

            // Apply bounds as a final check
            foreach (var point in forecastPoints)
            {
                point.Value = Math.Max(bounds.Min, Math.Min(bounds.Max, point.Value));
            }

            return forecastPoints;
        }

        /// <summary>
        /// Specialized forecast for temperature with daily cycles
        /// </summary>
        private List<WeatherPoint> ForecastTemperature(List<double> values, List<DateTime> timePoints, int forecastHours)
        {
            var result = new List<WeatherPoint>();

            // Get the last known temperature
            double lastTemp = values.Last();

            // Calculate daily min/max pattern
            var hourlyStats = CalculateHourlyStats(values, timePoints);

            // Calculate hourly rate of change from historical data
            var hourlyChanges = CalculateAverageHourlyChanges(values, timePoints);

            // Get the last timestamp
            DateTime lastTime = timePoints.Last();

            // Generate forecast points
            for (int i = 0; i < forecastHours; i++)
            {
                DateTime forecastTime = lastTime.AddHours(i + 1);
                int hourOfDay = forecastTime.Hour;

                // Use historical hourly averages as base
                double baseTemp = hourlyStats.ContainsKey(hourOfDay) ? hourlyStats[hourOfDay].Average : lastTemp;

                // Adjust with trend and hourly patterns
                double hourlyChange = hourlyChanges.ContainsKey(hourOfDay) ? hourlyChanges[hourOfDay] : 0;

                // Apply sinusoidal daily pattern (stronger in clear weather)
                double dailyCycle = CalculateDailyCycle(forecastTime, 2.0); // 2°C amplitude

                // Combine the factors
                double forecast = baseTemp + hourlyChange + dailyCycle;

                // Add some slight random variation (weather isn't perfectly predictable)
                Random rnd = new Random(i);
                forecast += (rnd.NextDouble() - 0.5) * 0.3; // Small random component ±0.15°C

                // Add to results
                result.Add(new WeatherPoint
                {
                    Value = forecast,
                    DateTime = forecastTime
                });

                // Update last temperature for next iteration
                lastTemp = forecast;
            }

            return result;
        }

        /// <summary>
        /// Specialized forecast for humidity with inverse relationship to temperature
        /// </summary>
        private List<WeatherPoint> ForecastHumidity(List<double> values, List<DateTime> timePoints, int forecastHours)
        {
            var result = new List<WeatherPoint>();

            // Get the last known humidity
            double lastHumidity = values.Last();

            // Calculate hourly stats
            var hourlyStats = CalculateHourlyStats(values, timePoints);

            // Humidity has inverse relationship with temperature (generally)
            // So we model it with inverse sinusoidal pattern to temperature

            // Get the last timestamp
            DateTime lastTime = timePoints.Last();

            // Generate forecast points
            for (int i = 0; i < forecastHours; i++)
            {
                DateTime forecastTime = lastTime.AddHours(i + 1);
                int hourOfDay = forecastTime.Hour;

                // Base humidity from historical data for this hour
                double baseHumidity = hourlyStats.ContainsKey(hourOfDay) ? hourlyStats[hourOfDay].Average : lastHumidity;

                // Apply inverse daily cycle (humidity decreases during warmest parts of day)
                double dailyCycle = -CalculateDailyCycle(forecastTime, 5.0); // 5% amplitude, inverse pattern

                // Combine factors
                double forecast = baseHumidity + dailyCycle;

                // Add slight random variation
                Random rnd = new Random(i + 100); // Different seed than temperature
                forecast += (rnd.NextDouble() - 0.5) * 2.0; // Random component ±1%

                // Keep within bounds
                forecast = Math.Max(0, Math.Min(100, forecast));

                // Add to results
                result.Add(new WeatherPoint
                {
                    Value = forecast,
                    DateTime = forecastTime
                });

                // Update last humidity for next iteration
                lastHumidity = forecast;
            }

            return result;
        }

        /// <summary>
        /// Specialized forecast for visibility with daily patterns
        /// </summary>
        private List<WeatherPoint> ForecastVisibility(List<double> values, List<DateTime> timePoints, int forecastHours)
        {
            var result = new List<WeatherPoint>();

            // Get the last known visibility
            double lastVisibility = values.Last();

            // Calculate hourly stats
            var hourlyStats = CalculateHourlyStats(values, timePoints);

            // Visibility typically increases during daytime and decreases at night
            // Also has relationship with humidity (higher humidity, lower visibility)

            // Get the last timestamp
            DateTime lastTime = timePoints.Last();

            // Generate forecast points
            for (int i = 0; i < forecastHours; i++)
            {
                DateTime forecastTime = lastTime.AddHours(i + 1);
                int hourOfDay = forecastTime.Hour;

                // Base visibility from historical data for this hour
                double baseVisibility = hourlyStats.ContainsKey(hourOfDay) ? hourlyStats[hourOfDay].Average : lastVisibility;

                // Apply daily cycle (visibility better during day)
                double dailyCycle = CalculateDailyCycle(forecastTime, 4.0); // 4% amplitude

                // Combine factors
                double forecast = baseVisibility + dailyCycle;

                // Add slight random variation
                Random rnd = new Random(i + 200); // Different seed
                forecast += (rnd.NextDouble() - 0.5) * 3.0; // Random component ±1.5%

                // Keep within bounds
                forecast = Math.Max(0, Math.Min(100, forecast));

                // Add to results
                result.Add(new WeatherPoint
                {
                    Value = forecast,
                    DateTime = forecastTime
                });

                // Update last visibility for next iteration
                lastVisibility = forecast;
            }

            return result;
        }

        /// <summary>
        /// Specialized forecast for atmospheric pressure with very gradual changes
        /// </summary>
        private List<WeatherPoint> ForecastPressure(List<double> values, List<DateTime> timePoints, int forecastHours)
        {
            var result = new List<WeatherPoint>();

            // Atmospheric pressure changes very slowly and smoothly in most circumstances
            // Use simple moving average with trend detection

            // Calculate the trend over recent values (last 6 hours if available)
            int trendSamples = Math.Min(6, values.Count);
            double trend = 0;

            if (trendSamples > 1)
            {
                var recentValues = values.Skip(values.Count - trendSamples).ToList();
                trend = (recentValues.Last() - recentValues.First()) / (trendSamples - 1);
            }

            // Limit trend to realistic values (pressure typically changes by max 1-2 hPa per hour)
            trend = Math.Max(-1.5, Math.Min(1.5, trend));

            // Get the last known pressure
            double lastPressure = values.Last();

            // Get the last timestamp
            DateTime lastTime = timePoints.Last();

            // Generate forecast points
            for (int i = 0; i < forecastHours; i++)
            {
                DateTime forecastTime = lastTime.AddHours(i + 1);

                // Apply trend with dampening (trend weakens over projection time)
                double dampingFactor = Math.Max(0, 1.0 - (i * 0.05)); // Gradually reduce trend influence
                double forecastTrend = trend * dampingFactor;

                // Apply semi-diurnal cycle (pressure typically has two peaks per day)
                double hourOfDay = forecastTime.Hour;
                double cyclicalComponent = 0.2 * Math.Sin(2 * Math.PI * hourOfDay / 12.0);

                // Calculate forecast with slight randomness
                Random rnd = new Random(i + 300);
                double randomComponent = (rnd.NextDouble() - 0.5) * 0.2; // Very slight random component ±0.1 hPa

                double forecast = lastPressure + forecastTrend + cyclicalComponent + randomComponent;

                // Add to results
                result.Add(new WeatherPoint
                {
                    Value = forecast,
                    DateTime = forecastTime
                });

                // Update last pressure for next iteration
                lastPressure = forecast;
            }

            return result;
        }

        /// <summary>
        /// Generic ARIMA forecast implementation for any parameter
        /// </summary>
        private List<WeatherPoint> GenericARIMAForecast(
            List<double> values,
            List<DateTime> timePoints,
            int forecastHours,
            (double Min, double Max) bounds)
        {
            var result = new List<WeatherPoint>();

            // Apply differencing if d > 0
            var differenced = values;
            var originalMean = values.Average();

            for (int i = 0; i < d; i++)
            {
                differenced = ApplyDifferencing(differenced);
            }

            // Estimate AR coefficients
            var arCoefficients = EstimateARCoefficients(differenced, p);

            // Estimate MA coefficients
            var maCoefficients = EstimateMACoefficients(differenced, q);

            // Generate ARIMA forecast
            var forecastValues = new List<double>();
            var errorTerms = CalculateErrorTerms(differenced, arCoefficients, maCoefficients);

            // Use last p values from original series
            var lastValues = values.Skip(Math.Max(0, values.Count - p)).ToList();
            while (lastValues.Count < p)
            {
                lastValues.Add(values.Last()); // Pad if needed
            }

            // Use last q error terms
            var lastErrors = errorTerms.Skip(Math.Max(0, errorTerms.Count - q)).ToList();
            while (lastErrors.Count < q)
            {
                lastErrors.Add(0); // Pad with zeros if needed
            }

            // Get the last timestamp
            DateTime lastTime = timePoints.Last();

            // Generate forecast
            for (int i = 0; i < forecastHours; i++)
            {
                // Calculate AR component
                double arComponent = 0;
                for (int j = 0; j < p; j++)
                {
                    if (j < lastValues.Count)
                    {
                        arComponent += arCoefficients[j] * lastValues[lastValues.Count - 1 - j];
                    }
                }

                // Calculate MA component
                double maComponent = 0;
                for (int j = 0; j < q; j++)
                {
                    if (j < lastErrors.Count)
                    {
                        maComponent += maCoefficients[j] * lastErrors[lastErrors.Count - 1 - j];
                    }
                }

                // Combine components
                double forecast = arComponent + maComponent;

                // Undo differencing
                if (d > 0)
                {
                    // Simple approach: add back the mean for each level of differencing
                    forecast += originalMean;
                }

                // Get forecast time
                DateTime forecastTime = lastTime.AddHours(i + 1);

                // Add seasonal component (sinusoidal pattern)
                double dailyCycle = CalculateDailyCycle(forecastTime, (bounds.Max - bounds.Min) * 0.02);
                forecast += dailyCycle;

                // Apply damping for longer term forecasts to prevent divergence
                double dampingFactor = Math.Max(0, 1.0 - (i * 0.05));
                double meanReversion = (originalMean - forecast) * (1 - dampingFactor);
                forecast += meanReversion;

                // Add small random component
                Random rnd = new Random(i + 400);
                double randomComponent = (rnd.NextDouble() - 0.5) * (bounds.Max - bounds.Min) * 0.01;
                forecast += randomComponent;

                // Keep within bounds
                forecast = Math.Max(bounds.Min, Math.Min(bounds.Max, forecast));

                forecastValues.Add(forecast);

                // Update last values and errors for next iteration
                lastValues.RemoveAt(0);
                lastValues.Add(forecast);

                double error = 0; // We don't know the actual error, so assume 0
                lastErrors.RemoveAt(0);
                lastErrors.Add(error);

                // Add to results with timestamp
                result.Add(new WeatherPoint
                {
                    Value = forecast,
                    DateTime = forecastTime
                });
            }

            return result;
        }

        /// <summary>
        /// Apply first-order differencing to a time series
        /// </summary>
        private List<double> ApplyDifferencing(List<double> values)
        {
            var result = new List<double>();
            for (int i = 1; i < values.Count; i++)
            {
                result.Add(values[i] - values[i - 1]);
            }
            return result;
        }

        /// <summary>
        /// Estimate AR coefficients using lag correlation
        /// </summary>
        private double[] EstimateARCoefficients(List<double> values, int order)
        {
            if (order == 0)
                return new double[0];

            // Simple implementation using correlation
            var coefficients = new double[order];

            for (int i = 0; i < order; i++)
            {
                // Use lag correlation as AR coefficient
                coefficients[i] = CalculateLagCorrelation(values, i + 1);
            }

            return coefficients;
        }

        /// <summary>
        /// Calculate correlation between a series and its lagged version
        /// </summary>
        private double CalculateLagCorrelation(List<double> values, int lag)
        {
            if (values.Count <= lag)
                return 0;

            // Calculate correlation coefficient
            var original = values.Skip(lag).ToList();
            var lagged = values.Take(values.Count - lag).ToList();

            // Simple correlation implementation
            double sumProduct = 0;
            double sumOriginal = 0;
            double sumLagged = 0;
            double sumOriginalSquared = 0;
            double sumLaggedSquared = 0;

            for (int i = 0; i < original.Count; i++)
            {
                sumProduct += original[i] * lagged[i];
                sumOriginal += original[i];
                sumLagged += lagged[i];
                sumOriginalSquared += original[i] * original[i];
                sumLaggedSquared += lagged[i] * lagged[i];
            }

            double n = original.Count;
            double numerator = sumProduct - (sumOriginal * sumLagged) / n;
            double denominator = Math.Sqrt((sumOriginalSquared - (sumOriginal * sumOriginal) / n) *
                                          (sumLaggedSquared - (sumLagged * sumLagged) / n));

            return denominator == 0 ? 0 : numerator / denominator;
        }

        /// <summary>
        /// Estimate MA coefficients with decreasing weights
        /// </summary>
        private double[] EstimateMACoefficients(List<double> values, int order)
        {
            if (order == 0)
                return new double[0];

            // Simplified MA coefficients - in a real implementation these would be estimated
            // from residuals after fitting the AR component
            var coefficients = new double[order];

            // Simple approximation: decreasing weights
            for (int i = 0; i < order; i++)
            {
                coefficients[i] = 0.5 / (i + 1);
            }

            return coefficients;
        }

        /// <summary>
        /// Calculate error terms (residuals) between actual and predicted values
        /// </summary>
        private List<double> CalculateErrorTerms(List<double> values, double[] arCoefficients, double[] maCoefficients)
        {
            var errors = new List<double>();

            // For simplicity, just use differences between actual and AR prediction
            for (int i = Math.Max(arCoefficients.Length, maCoefficients.Length); i < values.Count; i++)
            {
                double prediction = 0;

                // AR component
                for (int j = 0; j < arCoefficients.Length; j++)
                {
                    prediction += arCoefficients[j] * values[i - j - 1];
                }

                // We don't use previous errors for this calculation

                errors.Add(values[i] - prediction);
            }

            return errors;
        }

        /// <summary>
        /// Calculate MSE using a test set (last n values)
        /// </summary>
        private double CalculateMseOnTestSet(
            List<double> values,
            List<DateTime> timePoints,
            int testSize,
            (double Min, double Max) bounds,
            string parameterName)
        {
            if (values.Count <= testSize)
                return 0;

            // Split into train/test
            var trainValues = values.Take(values.Count - testSize).ToList();
            var trainTimes = timePoints.Take(timePoints.Count - testSize).ToList();
            var testValues = values.Skip(values.Count - testSize).ToList();
            var testTimes = timePoints.Skip(timePoints.Count - testSize).ToList();

            // Generate one-step-ahead forecasts
            var forecasts = new List<double>();

            // For each test point
            for (int i = 0; i < testSize; i++)
            {
                // Create data up to this point
                var currentValues = trainValues.Concat(testValues.Take(i)).ToList();
                var currentTimes = trainTimes.Concat(testTimes.Take(i)).ToList();

                // Generate forecast for next hour
                var forecast = GenerateConstrainedForecast(
                    currentValues,
                    currentTimes,
                    1, // Just forecast the next hour
                    bounds,
                    parameterName);

                if (forecast.Count > 0)
                {
                    forecasts.Add(forecast[0].Value);
                }
                else
                {
                    // Fallback if forecast fails
                    forecasts.Add(currentValues.Last());
                }
            }

            // Calculate MSE
            return MeanSquaredError.CalculateMSE(testValues, forecasts);
        }

        /// <summary>
        /// Calculate hourly statistics from historical data
        /// </summary>
        private Dictionary<int, (double Min, double Max, double Average)> CalculateHourlyStats(
            List<double> values,
            List<DateTime> timePoints)
        {
            var result = new Dictionary<int, (double Min, double Max, double Average)>();

            // Group by hour of day
            var hourlyGroups = values.Zip(timePoints, (v, t) => new { Value = v, Time = t })
                              .GroupBy(x => x.Time.Hour);

            foreach (var group in hourlyGroups)
            {
                int hour = group.Key;
                var hourValues = group.Select(x => x.Value).ToList();

                result[hour] = (
                    Min: hourValues.Min(),
                    Max: hourValues.Max(),
                    Average: hourValues.Average()
                );
            }

            return result;
        }

        /// <summary>
        /// Calculate average hourly changes for identifying patterns
        /// </summary>
        private Dictionary<int, double> CalculateAverageHourlyChanges(
            List<double> values,
            List<DateTime> timePoints)
        {
            var result = new Dictionary<int, double>();

            for (int i = 1; i < values.Count; i++)
            {
                int hour = timePoints[i].Hour;
                double change = values[i] - values[i - 1];

                if (!result.ContainsKey(hour))
                {
                    result[hour] = change;
                }
                else
                {
                    // Average with existing change
                    result[hour] = (result[hour] + change) / 2;
                }
            }

            return result;
        }

        /// <summary>
        /// Calculate sinusoidal daily cycle component
        /// </summary>
        private double CalculateDailyCycle(DateTime time, double amplitude)
        {
            // Peak at 2PM (14), lowest at 2AM (2)
            double hourOfDay = time.Hour;
            double phase = 2 * Math.PI * ((hourOfDay - 14) % 24) / 24;

            return amplitude * Math.Sin(phase);
        }
    }    
}