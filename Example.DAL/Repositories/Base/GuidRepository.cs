using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Example.DAL.Entities.Abstract;
using Example.DAL.Repositories.Abstract;

namespace Example.DAL.Repositories.Base
{
    public class GuidRepository<TEntity> : IGuidRepository<TEntity> where TEntity : class, IGuidEntity
    {
        private readonly IDbContextProvider _dbContextProvider;

        public GuidRepository(IDbContextProvider dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        protected DbContext DbContext { get { return _dbContextProvider.Context; } }

        protected DbSet<TEntity> Set { get { return DbContext.Set<TEntity>(); } }

        public virtual TEntity FindById(Guid id)
        {
            return Set.Find(id);
        }

        public virtual IEnumerable<TEntity> FindByIds(IEnumerable<Guid> ids)
        {
            return Set.Where(e => ids.Contains(e.Id)).ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<Guid> ids)
        {
            return await Set.Where(e => ids.Contains(e.Id)).ToListAsync();
        }

        public virtual IEnumerable<TEntity> FindByIds(IEnumerable<string> ids)
        {
            var list = ids.Select(i => new Guid(i));
            return FindByIds(list);
        }

        public virtual async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<string> ids)
        {
            var list = ids.Select(i => new Guid(i));
            return await FindByIdsAsync(list);
        }

        public virtual Task<TEntity> FindByIdAsync(Guid id)
        {
            return Set.FindAsync(id);
        }

        //public virtual IEnumerable<TEntity> GetAll()
        //{
        //    return Set.ToList();
        //}

        //public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        //{
        //    return await Set.ToListAsync();
        //}

        public virtual TEntity Insert(TEntity entity)
        {
            return Set.Add(entity);
        }

        public virtual TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual void Delete(Guid id)
        {
            var entity = Set.Find(id);

            if (entity == null)
            {
                return;
            }

            Delete(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Set.Remove(entity);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return Set.AsQueryable();
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            if (!Set.Local.Contains(entity))
            {
                Set.Attach(entity);
            }
        }
    }
}
