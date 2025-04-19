using System.Linq.Expressions;

namespace Domain.Repositories.Interfaces;

public interface IBaseRepositories<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
}