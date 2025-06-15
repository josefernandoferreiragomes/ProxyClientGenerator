using ApiServer.RequestProcessors.Models;
using System.Text.Json;

namespace ApiServer.RequestProcessors.Forecast
{
    public class ForecastRequestProcessor(ILogger<ForecastRequestProcessor> _logger) : IForecastRequestProcessor
    {
        string[] summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        public WeatherForecastResponse[] GetForecast(WeatherForecastRequest request)
        {
            WeatherForecastResponse[] result = ValidateAndLog<WeatherForecastRequest, WeatherForecastResponse[]>(GetForecastInternal, request);
            return result;
        }

        private WeatherForecastResponse[] GetForecastInternal(WeatherForecastRequest request)
        {
            return Enumerable.Range(1, 5).Select(index =>
                new WeatherForecastResponse
                (
                    DateOnly.FromDateTime(request.StartDate.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
                .ToArray();
        }
        //new delegate method, to centralize logging for each internal method
        public TResponse ValidateAndLog<TRequest, TResponse>(
        Func<TRequest, TResponse> method,
        TRequest request
    )
        {
            if (request == null)
            {
                _logger.Log(LogLevel.Warning,"Validation failed: request is null or empty");
                throw new ArgumentException("Request cannot be null or empty", nameof(request));
            }
            //log request details as json
            _logger.Log(LogLevel.Warning,string.Format("Validating request: {0}", JsonSerializer.Serialize<TRequest>(request)));

            _logger.Log(LogLevel.Warning,"Validation passed, proceeding with method execution");
            var response = method(request);
            // log response details as json
            _logger.Log(LogLevel.Warning,string.Format("Validating request: {0}", JsonSerializer.Serialize<TResponse>(response)));

            return response;
        }
    }
}
