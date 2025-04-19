using Application.Dto.WeatherManagement.CurrentWeather;
using Application.Handlers.WeatherManagement.Interfaces;
using Domain.Models.WeatherManagement;
using Domain.Repositories.WeatherManagement.Interfaces;
using Domain.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Handlers.WeatherManagement;

public class CurrentWeatherHandler(ICurrentWeatherService currentWeatherService, 
    IWeatherRepositories weatherRepositories,
    IMemoryCache memoryCache) : ICurrentWeatherHandler
{
    public async Task<WeatherResponseDTO> GetCurrentWeather()
    {
        if (memoryCache.TryGetValue("CurrentWeather", out WeatherResponseDTO? weatherResponse))
            return weatherResponse ?? new WeatherResponseDTO();

        try
        {
            var current = await currentWeatherService.GetCurrentWeather();

            await weatherRepositories.AddAsync(new Weather()
            {
                Location = current.Name,
                Timestamp = current.Dt,
                Temperature = current.Main.Temp,
                FeelsLike = current.Main.Feels_Like,
                TempMin = current.Main.Temp_Min,
                TempMax = current.Main.Temp_Max,
                Pressure = current.Main.Pressure,
                Humidity = current.Main.Humidity,
                WeatherMain = current.Weather[0].Main,
                WeatherDescription = current.Weather[0].Description,
                WindSpeed = current.Wind.Speed,
                Cloudiness = current.Clouds.All,
                Icon = current.Weather[0].Icon,
            });

            var result = await weatherRepositories.GetLastAsync(x => x.Id) ?? new Weather();

            var response = new WeatherResponseDTO()
            {
                Location = result.Location,
                Timestamp = result.Timestamp,
                Temperature = result.Temperature,
                FeelsLike = result.FeelsLike,
                TempMin = result.TempMin,
                TempMax = result.TempMax,
                Pressure = result.Pressure,
                Humidity = result.Humidity,
                WeatherMain = result.WeatherMain,
                WeatherDescription = result.WeatherDescription,
                WindSpeed = result.WindSpeed,
                Cloudiness = result.Cloudiness,
                Icon = result.Icon,
                CreationDate = result.CreationDate,
                LastUpdate = result.LastUpdate
            };
            
            memoryCache.Set("CurrentWeather", response, TimeSpan.FromMinutes(10));

            return response;
        }
        catch(Exception ex)
        {
            throw new Exception("Error getting current weather. " + ex.Message);
        }
    }
}