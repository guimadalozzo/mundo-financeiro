using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MundoFinanceiro.Database.Contracts.Persistence.Repositories
{
    public interface IRepository<TEntity> : IEnumerable<TEntity> where TEntity : class
    {
        bool Any(Expression<Func<TEntity, bool>> predicate = null);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate = null);
        int Count(Expression<Func<TEntity, bool>> predicate = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
        
        bool Exists(object id);
        Task<bool> ExistsAsync(object id);
        
        TEntity Get(object id);
        TEntity GetWithoutTracking(object id);
        Task<TEntity> GetAsync(object id);
        Task<TEntity> GetWithoutTrackingAsync(object id);
        IEnumerable<TEntity> GetAll(params string[] include);
        Task<IEnumerable<TEntity>> GetAllAsync(params string[] include);
        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, params string[] include);

        TEntity Single(Expression<Func<TEntity, bool>> predicate, params string[] include);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate, params string[] include);
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, params string[] include);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] include);

        TEntity First(Expression<Func<TEntity, bool>> predicate, params string[] include);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params string[] include);
        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, params string[] include);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] include);

        TEntity Last(Expression<Func<TEntity, bool>> predicate, params string[] include);
        TEntity LastOrDefault(Expression<Func<TEntity, bool>> predicate, params string[] include);

        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}