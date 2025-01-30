using Microsoft.EntityFrameworkCore;
using SensorProcessingDemo.Repositories.Interfaces;
using SensorProcessingDemo.Services;
using System.Linq.Expressions;
namespace SensorProcessingDemo.Repositories.Implementations
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class
    {
        private readonly MonitoringSystemContext _context;
        private readonly DbSet<T> _dbSet;

        public EntityRepository(MonitoringSystemContext context)
        { 
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }


        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet.FindAsync(id);
        }
       
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }       
    }
}
