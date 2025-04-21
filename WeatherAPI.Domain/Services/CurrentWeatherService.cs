using Domain.Dto.WeatherManagement.CurrentWeather;
using Domain.Secrets;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly.CircuitBreaker;

namespace Domain.Services;

public class CurrentWeatherService : ICurrentWeatherService
{
    private readonly HttpClient _client;
    private readonly OpenApi _openApi;
    private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy;

    public CurrentWeatherService(CircuitBreakerPolicyProvider policyProvider, IOptions<OpenApi> openApi)
    {
        _openApi = openApi.Value;
        _client = new HttpClient();
        _circuitBreakerPolicy = policyProvider.GetPolicy();
    }
    
    public async Task<CurrentWeatherDTO> GetCurrentWeather()
    {
        string apiKey = _openApi.APIKEYOpW;
        string lat = _openApi.Lat;
        string lon = _openApi.Lon;
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