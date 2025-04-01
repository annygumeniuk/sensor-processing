using SensorProcessingDemo.Services;
using System.Linq.Expressions;

namespace SensorProcessingDemo.Repositories.Interfaces
{
    public interface IEntityRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetFirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(object id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(object id);
        Task SaveChangesAsync();
        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, TResult>> selector);
        Task ExecuteInTransactionAsync(Func<MonitoringSystemContext, Task> action);
    }
}