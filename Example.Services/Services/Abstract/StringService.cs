using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.DAL;
using Example.DAL.Entities.Abstract;
using Example.DAL.Repositories.Abstract;

namespace Example.Services.Services.Abstract
{
    public class StringService<TEntity> : IStringService<TEntity> where TEntity : class, IEntity<string>
    {
        private readonly IStringRepository<TEntity> _accessoryRepository;
        private readonly IDbContextProvider _provider;

        public StringService(IStringRepository<TEntity> accessoryRepository, IDbContextProvider provider)
        {
            _accessoryRepository = accessoryRepository;
            _provider = provider;
        }

        public async Task<TEntity> FindByIdAsync(string id)
        {
            return await _accessoryRepository.FindByIdAsync(id);
        }

        public TEntity FindById(string id)
        {
            return _accessoryRepository.FindById(id);
        }

        public IEnumerable<TEntity> FindByIds(IEnumerable<string> ids)
        {
            return _accessoryRepository.FindByIds(ids);
        }

        public async Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<string> ids)
        {
            return await _accessoryRepository.FindByIdsAsync(ids);
        }

        //public async Task<IEnumerable<TEntity>> GetAllAsync()
        //{
        //    return await _accessoryRepository.GetAllAsync();
        //}

        public async Task<TEntity> Insert(TEntity entity)
        {
            var newItem = _accessoryRepository.Insert(entity);
            await _provider.SaveChangesAsync();
            return newItem;
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            var updateItem = _accessoryRepository.Update(entity);
            await _provider.SaveChangesAsync();
            return updateItem;
        }

        public TEntity UpdateSync(TEntity entity)
        {
            var updateItem = _accessoryRepository.Update(entity);
            _provider.SaveChanges();
            return updateItem;
        }

        public async Task Delete(string id, Action<TEntity> beforeDeleteAction = null)
        {
            deleteItemById(id, beforeDeleteAction);
            await _provider.SaveChangesAsync();
        }

        private void deleteItemById(string id, Action<TEntity> beforeDeleteAction)
        {
            if (beforeDeleteAction != null)
            {
                var entity = FindById(id);
                beforeDeleteAction(entity);
            }

            _accessoryRepository.Delete(id);
        }

        public void DeleteSync(string id, Action<TEntity> beforeDeleteAction = null)
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

            _accessoryRepository.Delete(entity);
        }

        public void DeleteSync(TEntity entity, Action<TEntity> beforeDeleteAction = null)
        {
            deleteItem(entity, beforeDeleteAction);
            _provider.SaveChanges();
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _accessoryRepository.AsQueryable();
        }
    }
}
