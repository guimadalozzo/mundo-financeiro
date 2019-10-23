using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MundoFinanceiro.Database.Contracts.Persistence.Repositories;

namespace MundoFinanceiro.Database.Persistence.Repositories
{
    internal abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _entity;
        protected readonly DataContext Context;

        protected Repository(DataContext context)
        {
            _entity = context.Set<TEntity>();
            Context = context;
        }

        public bool Any(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate != null
                ? _entity.Any(predicate)
                : _entity.Any();
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return predicate != null
                ? _entity.AnyAsync(predicate)
                : _entity.AnyAsync();
        }

        public int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate != null
                ? _entity.Count(predicate)
                : _entity.Count();
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            return predicate != null
                ? _entity.CountAsync(predicate)
                : _entity.CountAsync();
        }

        public bool Exists(object id) => Get(id) != null;

        public async Task<bool> ExistsAsync(object id) => await GetAsync(id) != null;

        public TEntity Get(object id) => _entity.Find(id);

        public TEntity GetWithoutTracking(object id)
        {
            var entityWithoutTracking = Get(id);
            Context.Entry(entityWithoutTracking).State = EntityState.Detached;

            return entityWithoutTracking;
        }

        public Task<TEntity> GetAsync(object id) => _entity.FindAsync(id);

        public async Task<TEntity> GetWithoutTrackingAsync(object id)
        {
            var entity = await GetAsync(id);
            Context.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public IEnumerable<TEntity> GetAll(params string[] include) => GetEntityWithIncludeProperties(include).ToList();

        public async Task<IEnumerable<TEntity>> GetAllAsync(params string[] include)
        {
            return await GetEntityWithIncludeProperties(include).ToListAsync().ConfigureAwait(false);
        }

        public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            return GetEntityWithIncludeProperties(include).Where(predicate);
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            return GetEntityWithIncludeProperties(include).Single(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            return GetEntityWithIncludeProperties(include).SingleOrDefault(predicate);
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            return GetEntityWithIncludeProperties(include).SingleAsync(predicate);
        }

        public Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            return GetEntityWithIncludeProperties(include).SingleOrDefaultAsync(predicate);
        }

        public TEntity First(Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            return GetEntityWithIncludeProperties(include).First(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            return GetEntityWithIncludeProperties(include).FirstOrDefault(predicate);
        }

        public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            return GetEntityWithIncludeProperties(include).FirstAsync(predicate);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            return GetEntityWithIncludeProperties(include).FirstOrDefaultAsync(predicate);
        }

        public TEntity Last(Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            return GetEntityWithIncludeProperties(include).Last(predicate);
        }

        public TEntity LastOrDefault(Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            return GetEntityWithIncludeProperties(include).LastOrDefault(predicate);
        }

        private IQueryable<TEntity> GetEntityWithIncludeProperties(string[] includeProperties)
        {
            var query = _entity.AsQueryable();

            foreach (var include in includeProperties)
                query = query.Include(include);

            return query;
        }

        public void Add(TEntity entity) => this._entity.Add(entity);

        public void AddRange(IEnumerable<TEntity> entities) => _entity.AddRange(entities);

        public void Remove(TEntity entity) => this._entity.Remove(entity);

        public void RemoveRange(IEnumerable<TEntity> entities) => _entity.RemoveRange(entities);
        
        public IEnumerator<TEntity> GetEnumerator() => _entity.AsQueryable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}