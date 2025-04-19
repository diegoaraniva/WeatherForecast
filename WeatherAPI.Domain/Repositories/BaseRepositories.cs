using System.Linq.Expressions;
using Domain.Contexts;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public class BaseRepositories<T> : IBaseRepositories<T> where T : class
{
    private readonly AppDBContext _context;

    public BaseRepositories(AppDBContext context) => _context = context;

    public async Task<T> AddAsync(T entity)
    {
        await _context.AddAsync(entity ?? throw new ArgumentNullException(nameof(entity)));
        await _context.SaveChangesAsync();
        return entity;
    }
    
    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
    {
        await _context.AddRangeAsync(entities ?? throw new ArgumentNullException(nameof(entities)));
        await _context.SaveChangesAsync();
        return entities;
    }
}