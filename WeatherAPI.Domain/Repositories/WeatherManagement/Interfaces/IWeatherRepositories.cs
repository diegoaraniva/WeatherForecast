using System.Linq.Expressions;
using Domain.Models.WeatherManagement;
using Domain.Repositories.Interfaces;

namespace Domain.Repositories.WeatherManagement.Interfaces;

public interface IWeatherRepositories : IBaseRepositories<Weather>
{
    Task<Weather?> GetLastAsync(Expression<Func<Weather, object>> orderBy);
}