using Domain.Dto.WeatherManagement.CurrentWeather;
using Newtonsoft.Json;
using Polly.CircuitBreaker;

namespace Domain.Services;

public class CurrentWeatherService : ICurrentWeatherService
{
    private readonly HttpClient _client;
    private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy;

    public CurrentWeatherService(CircuitBreakerPolicyProvider policyProvider)
    {
        _client = new HttpClient();
        _circuitBreakerPolicy = policyProvider.GetPolicy();
    }
    
    public async Task<CurrentWeatherDTO> GetCurrentWeather()
    {
        string apiKey = "91dc4115f3501094f7a6d21a54476583";
        string lat = "13.6920";
        string lon = "-89.2182";
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