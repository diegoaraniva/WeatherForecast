using Domain.Dto.WeatherManagement.CurrentWeather;
using Domain.Secrets;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly.CircuitBreaker;

namespace Domain.Services;

public class CurrentWeatherService : ICurrentWeatherService
{
    private readonly HttpClient _client;
    private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy;

    public CurrentWeatherService(CircuitBreakerPolicyProvider policyProvider, IOptions<OpenApi> openApi)
    {
        _client = new HttpClient();
        _circuitBreakerPolicy = policyProvider.GetPolicy();
    }
    
    public async Task<CurrentWeatherDTO> GetCurrentWeather()
    {
        string apiKey = Environment.GetEnvironmentVariable("APIKEYOpW") ?? string.Empty;
        string lat = Environment.GetEnvironmentVariable("Lat") ?? string.Empty;
        string lon = Environment.GetEnvironmentVariable("Lon") ?? string.Empty;
        string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}";
        
        try
        {
            var response = await _circuitBreakerPolicy.ExecuteAsync(() => _client.GetAsync(url));
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var currentWeather = JsonConvert.DeserializeObject<CurrentWeatherDTO>(json) ?? new CurrentWeatherDTO();
            return currentWeather;
        }
        catch (BrokenCircuitException ex)
        {
            throw new BrokenCircuitException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Failure in CurrentWeather endpoint. " + ex.Message);
        }
    }
}