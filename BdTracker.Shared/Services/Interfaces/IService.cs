using BdTracker.Shared.Entities;

namespace BdTracker.Shared.Services.Interfaces;

public interface IService<T> where T : BaseEntity
{
    Task<T?> GetAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task DeleteAsync(Guid id);
    Task<T> AddAsync(T item);
    Task UpdateAsync(T item);
}