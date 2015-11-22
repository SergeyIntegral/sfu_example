using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.DAL;
using Example.DAL.Entities.Abstract;
using Example.DAL.Repositories.Abstract;

namespace Example.Services.Services.Abstract
{
    public class GuidService<TEntity> : IGuidService<TEntity> where TEntity : class, IGuidEntity
    {
        private readonly IGuidRepository<TEntity> _repository;
        private readonly IDbContextProvider _provider;

        public GuidService(IGuidRepository<TEntity> repository, IDbContextProvider provider)
        {
            _repository = repository;
            _provider = provider;
        }

        public async Task<TEntity> FindByIdAsync(Guid id)
        {
            return await _repository.FindByIdAsync(id);
        }

        public TEntity FindById(Guid id)
        {
            return _repository.FindById(id);
        }

        public IEnumerable<TEntity> FindByIds(IEnumerable<Guid> ids)
        {
            return _repository.FindByIds(ids);
        }

        public async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<Guid> ids)
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
        //    return await Task.Factory.StartNew(() => _repository.AsQueryable());
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

        public async Task Delete(Guid id, Action<TEntity> beforeDeleteAction = null)
        {
            deleteItemById(id, beforeDeleteAction);
            await _provider.SaveChangesAsync();
        }

        private void deleteItemById(Guid id, Action<TEntity> beforeDeleteAction)
        {
            if (beforeDeleteAction != null)
            {
                var entity = FindById(id);
                beforeDeleteAction(entity);
            }

            _repository.Delete(id);
        }

        public void DeleteSync(Guid id, Action<TEntity> beforeDeleteAction = null)
        {
            deleteItemById(id, beforeDeleteAction);
            _provider.SaveChanges();
        }

        public async Task Delete(TEntity entity, Action<TEntity> beforeDeleteAction = null)
        {
            deleteItem(entity, beforeDeleteAction);
            await _provider.SaveChangesAsync();
        }

        private void deleteItem(TEntity entity, Action<TEntity> beforeDeleteAction)
        {
            if (beforeDeleteAction != null)
            {
                beforeDeleteAction(entity);
            }
            _repository.Delete(entity);
        }

        public void DeleteSync(TEntity entity, Action<TEntity> beforeDeleteAction = null)
        {
            deleteItem(entity, beforeDeleteAction);
            _provider.SaveChanges();
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _repository.AsQueryable();
        }
    }
}