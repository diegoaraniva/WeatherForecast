using Domain.Dto.WeatherManagement.ForecastWeather;
using Domain.Secrets;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly.CircuitBreaker;

namespace Domain.Services;

public class ForecastService : IForecastService
{
    
    private readonly HttpClient _client;
    private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy;
    
    public ForecastService(CircuitBreakerPolicyProvider policyProvider, IOptions<OpenApi> openApi)
    {
        _client = new HttpClient();
        _circuitBreakerPolicy = policyProvider.GetPolicy();
    }
    
    public async Task<ForecastWeatherDTO> GetForecast()
    {
        string apiKey = Environment.GetEnvironmentVariable("APIKEYOpW") ?? string.Empty;
        string lat = Environment.GetEnvironmentVariable("Lat") ?? string.Empty;
        string lon = Environment.GetEnvironmentVariable("Lon") ?? string.Empty;
        string url = $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={apiKey}";

        try
        {
            var response = await _circuitBreakerPolicy.ExecuteAsync(() => _client.GetAsync(url));
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var forecastWeather = JsonConvert.DeserializeObject<ForecastWeatherDTO>(json) ?? new ForecastWeatherDTO();
            return forecastWeather;
        }
        catch (BrokenCircuitException ex)
        {
            throw new BrokenCircuitException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Failure in Forecast endpoint. " + ex.Message);
        }
    }
}