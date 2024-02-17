using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Netflix_Clone.Infrastructure.DataAccess.Data.Contexts;
using Netflix_Clone.Infrastructure.DataAccess.Repositories.IRepositories;
using System.Linq.Expressions;

namespace Netflix_Clone.Infrastructure.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public ApplicationDbContext applicationDbContext { get; set; }
        public DbSet<T> dbSet { get; set; }

        public Repository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
            this.dbSet = this.applicationDbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Remove(T entity)
        {
            dbSet.Attach(entity);
            dbSet.Remove(entity);
        }

        public T? GetFirst(Expression<Func<T, bool>> filter = null!,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null!)
        {
            IQueryable<T> _dbSet = dbSet;

            if (include is not null)
            {
                _dbSet = include(_dbSet);
            }

            if (filter is not null)
            {
                _dbSet = _dbSet.Where(filter);
            }

            return _dbSet.FirstOrDefault();
        }

        public async Task<T?> GetFirstAsync(Expression<Func<T, bool>> filter = null!,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null!)
        {
            IQueryable<T> _dbSet = dbSet;

            if (include is not null)
            {
                _dbSet = include(_dbSet);
            }

            if (filter is not null)
            {
                _dbSet = _dbSet.Where(filter);
            }

            return await _dbSet.FirstOrDefaultAsync();
        }


        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null!,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null!)
        {
            IQueryable<T> _dbSet = dbSet;

            if (include is not null)
            {
                _dbSet = include(_dbSet);
            }

            if (filter is not null)
            {
                _dbSet = _dbSet.Where(filter);
            }

            return _dbSet;
        }

        public T? GetFirstAsNoTracking(Expression<Func<T, bool>> filter = null!,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null!)
        {
            IQueryable<T> _dbSet = dbSet;

            if (include is not null)
            {
                _dbSet = include(_dbSet);
            }

            if (filter is not null)
            {
                _dbSet = _dbSet.Where(filter);
            }

            return _dbSet.AsNoTracking().FirstOrDefault();
        }

        public async Task<T?> GetFirstAsNoTrackingAsync(Expression<Func<T, bool>> filter = null!,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null!)
        {
            IQueryable<T> _dbSet = dbSet;

            if (include is not null)
            {
                _dbSet = include(_dbSet);
            }

            if (filter is not null)
            {
                _dbSet = _dbSet.Where(filter);
            }

            return await _dbSet.AsNoTracking().FirstOrDefaultAsync();
        }

        public IEnumerable<T> GetAllAsNoTracking(Expression<Func<T, bool>> filter = null!,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null!)
        {
            IQueryable<T> _dbSet = dbSet;

            if (include is not null)
            {
                _dbSet = include(_dbSet);
            }

            if (filter is not null)
            {
                _dbSet = _dbSet.Where(filter);
            }

            return _dbSet.AsNoTracking();
        }
    }
}
