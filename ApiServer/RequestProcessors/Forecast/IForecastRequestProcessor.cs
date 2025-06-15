using ApiServer.RequestProcessors.Models;

namespace ApiServer.RequestProcessors.Forecast
{
    public interface IForecastRequestProcessor
    {
        WeatherForecastResponse[] GetForecast(WeatherForecastRequest request);
    }
}