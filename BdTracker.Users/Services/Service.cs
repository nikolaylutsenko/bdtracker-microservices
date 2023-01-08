using BdTracker.Back.Services.Interfaces;
using BdTracker.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using BdTracker.Back.Data;

namespace GameStore.Core.Services
{
    public class Service<T> : IService<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Service(AppDbContext context)
        {
            _dbSet = context.Set<T>();
            _context = context;
        }

        public async Task<T> AddAsync(T item)
        {
            var entity = _dbSet.Add(item);

            await _context.SaveChangesAsync();

            return entity.Entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var item = await _dbSet.FindAsync(id);
            if (item == null)
            {
                throw new ArgumentNullException($"No item of type [{typeof(T).Name}] with id: [{id}] found");
            }

            _dbSet.Remove(item);

            await _context.SaveChangesAsync();
        }

        public async Task<T?> GetAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task UpdateAsync(T item)
        {
            _context.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
