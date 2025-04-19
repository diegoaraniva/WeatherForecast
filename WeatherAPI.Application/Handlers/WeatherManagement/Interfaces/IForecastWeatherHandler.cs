using Application.Dto.WeatherManagement.ForecastWeather;

namespace Application.Handlers.WeatherManagement.Interfaces;

public interface IForecastWeatherHandler
{
    Task<List<ForecastResponseDTO>> GetForecast();
}