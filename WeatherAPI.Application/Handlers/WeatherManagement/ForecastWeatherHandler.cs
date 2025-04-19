using Application.Dto.WeatherManagement.ForecastWeather;
using Application.Handlers.WeatherManagement.Interfaces;
using Domain.Models.WeatherManagement;
using Domain.Repositories.WeatherManagement.interfaces;
using Domain.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Handlers.WeatherManagement;

public class ForecastWeatherHandler(IForecastService forecastService, 
    IForecastRepositories forecastRepositories,
    IMemoryCache memoryCache)  : IForecastWeatherHandler
{
    public async Task<List<ForecastResponseDTO>> GetForecast()
    {
        if (memoryCache.TryGetValue("CurrentWeather", out List<ForecastResponseDTO>? forecastResponse))
            return forecastResponse ?? new List<ForecastResponseDTO>();

        try
        {
            var current = await forecastService.GetForecast();

            var request = await forecastRepositories.AddRangeAsync(
                current.List.Select(f => new Forecast()
                {
                    CityName = "Antiguo Cuscatlan",
                    ForecastTime = f.Dt_Txt,
                    Temperature = f.Main.Temp,
                    FeelsLike = f.Main.Feels_Like,
                    TempMin = f.Main.Temp_Min,
                    TempMax = f.Main.Temp_Max,
                    Pressure = f.Main.Pressure,
                    Humidity = f.Main.Humidity,
                    WeatherMain = f.Weather[0].Main,
                    WeatherDescription = f.Weather[0].Description,
                    WindSpeed = f.Wind.Speed,
                    Cloudiness = f.Clouds.All,
                    Icon = f.Weather[0].Icon
                }).ToList());

            var result = request.Where(f=>f.ForecastTime.Hour == 12).ToList();

            var response = result.Select(f=>new ForecastResponseDTO()
            {
                CityName = f.CityName,
                ForecastTime = f.ForecastTime,
                Temperature = f.Temperature,
                FeelsLike = f.FeelsLike,
                TempMin = f.TempMin,
                TempMax = f.TempMax,
                Pressure = f.Pressure,
                Humidity = f.Humidity,
                WeatherMain = f.WeatherMain,
                WeatherDescription = f.WeatherDescription,
                WindSpeed = f.WindSpeed,
                Cloudiness = f.Cloudiness,
                Icon = f.Icon,
                CreationDate = f.CreationDate,
                LastUpdate = f.LastUpdate
            }).ToList();
            
            memoryCache.Set("CurrentWeather", response, TimeSpan.FromMinutes(10));

            return response;
        }
        catch(Exception ex)
        {
            throw new Exception("Error getting forecast. " + ex.Message);
        }
    }
}