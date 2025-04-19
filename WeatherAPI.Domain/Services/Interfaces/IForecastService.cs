using Domain.Dto.WeatherManagement.ForecastWeather;

namespace Domain.Services;

public interface IForecastService
{
    Task<ForecastWeatherDTO> GetForecast();
}