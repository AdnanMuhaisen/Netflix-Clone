using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        Task AddAsync(T entity);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        IEnumerable<T> GetAllAsNoTracking(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        T? GetFirst(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        T? GetFirstAsNoTracking(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        Task<T?> GetFirstAsNoTrackingAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        Task<T?> GetFirstAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        void Remove(T entity);
    }
}