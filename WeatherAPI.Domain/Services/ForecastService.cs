using Domain.Dto.WeatherManagement.ForecastWeather;
using Newtonsoft.Json;
using Polly.CircuitBreaker;

namespace Domain.Services;

public class ForecastService : IForecastService
{
    private readonly HttpClient _client;
    private readonly AsyncCircuitBreakerPolicy<HttpResponseMessage> _circuitBreakerPolicy;
    
    public ForecastService(CircuitBreakerPolicyProvider policyProvider)
    {
        _client = new HttpClient();
        _circuitBreakerPolicy = policyProvider.GetPolicy();
    }
    
    public async Task<ForecastWeatherDTO> GetForecast()
    {
        string apiKey = "91dc4115f3501094f7a6d21a54476583";
        string lat = "13.6920";
        string lon = "-89.2182";
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