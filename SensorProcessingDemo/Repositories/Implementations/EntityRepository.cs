using Microsoft.EntityFrameworkCore;
using SensorProcessingDemo.Repositories.Interfaces;
using SensorProcessingDemo.Services;
using System.Linq.Expressions;

namespace SensorProcessingDemo.Repositories.Implementations
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class
    {
        private readonly IDbContextFactory<MonitoringSystemContext> _contextFactory;

        public EntityRepository(IDbContextFactory<MonitoringSystemContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(T entity)
        {
            await using var context = _contextFactory.CreateDbContext();
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            await using var context = _contextFactory.CreateDbContext();
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(object id)
        {
            await using var context = _contextFactory.CreateDbContext();
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            await using var context = _contextFactory.CreateDbContext();
            return await context.Set<T>().FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await using var context = _contextFactory.CreateDbContext();
            await context.SaveChangesAsync();
        }
    }
}
