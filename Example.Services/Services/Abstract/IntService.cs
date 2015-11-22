using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.DAL;
using Example.DAL.Entities.Abstract;
using Example.DAL.Repositories.Abstract;

namespace Example.Services.Services.Abstract
{
    public class IntService<TEntity> : IIntService<TEntity> where TEntity : class, IEntity<int>
    {
        private readonly IIntRepository<TEntity> _repository;
        private readonly IDbContextProvider _provider;

        public IntService(IIntRepository<TEntity> repository, IDbContextProvider provider)
        {
            _repository = repository;
            _provider = provider;
        }

        public async Task<TEntity> FindByIdAsync(int id)
        {
            return await _repository.FindByIdAsync(id);
        }

        public TEntity FindById(int id)
        {
            return _repository.FindById(id);
        }

        public IEnumerable<TEntity> FindByIds(IEnumerable<int> ids)
        {
            return _repository.FindByIds(ids);
        }

        public async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<int> ids)
        {
            return await _repository.FindByIdsAsync(ids);
        }

        public IEnumerable<TEntity> FindByIds(IEnumerable<string> ids)
        {
            return _repository.FindByIds(ids);
        }

        public async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<string> ids)
        {
            return await _repository.FindByIdsAsync(ids);
        }

        //public async Task<IEnumerable<TEntity>> GetAllAsync()
        //{
        //    return await _repository.GetAllAsync();
        //}

        public async Task<TEntity> Insert(TEntity entity)
        {
            var newItem = _repository.Insert(entity);
            await _provider.SaveChangesAsync();
            return newItem;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            var updateItem = _repository.Update(entity);
            await _provider.SaveChangesAsync();
            return updateItem;
        }

        public TEntity UpdateSync(TEntity entity)
        {
            var updateItem = _repository.Update(entity);
            _provider.SaveChanges();
            return updateItem;
        }

        public virtual async Task Delete(int id, Action<TEntity> beforeDeleteAction = null)
        {
            deleteItemById(id, beforeDeleteAction);
            await _provider.SaveChangesAsync();
        }

        public void DeleteSync(int id, Action<TEntity> beforeDeleteAction = null)
        {
            deleteItemById(id, beforeDeleteAction);
            _provider.SaveChanges();
        }

        private void deleteItemById(int id, Action<TEntity> beforeDeleteAction)
        {
            if (beforeDeleteAction != null)
            {
                var entity = FindById(id);
                beforeDeleteAction(entity);
            }
            _repository.Delete(id);
        }

        public virtual async Task Delete(TEntity entity, Action<TEntity> beforeDeleteAction = null)
        {
            deleteItem(entity, beforeDeleteAction);
            await _provider.SaveChangesAsync();
        }

        public void DeleteSync(TEntity entity, Action<TEntity> beforeDeleteAction = null)
        {
            deleteItem(entity, beforeDeleteAction);
            _provider.SaveChanges();
        }

        private void deleteItem(TEntity entity, Action<TEntity> beforeDeleteAction)
        {
            if (beforeDeleteAction != null)
            {
                beforeDeleteAction(entity);
            }
            _repository.Delete(entity);
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _repository.AsQueryable();
        }
    }
}