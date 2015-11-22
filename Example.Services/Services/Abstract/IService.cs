using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Example.Services.Services.Abstract
{
    public interface IIntService<TEntity> : IService<TEntity, int>
    {

    }

    public interface IGuidService<TEntity> : IService<TEntity, Guid>
    {

    }

    public interface IStringService<TEntity> : IService<TEntity, string>
    {

    }

    public interface IService<TEntity, in TKey>
    {
        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
        /// <returns>Entity</returns>
        Task<TEntity> FindByIdAsync(TKey id);

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity.</param>
        /// <returns>Entity</returns>
        TEntity FindById(TKey id);

        /// <summary>
        /// Gets an entity with given array primary keys.
        /// </summary>
        /// <param name="ids">Array primary keys of the entity.</param>
        /// <returns>Entity</returns>
        IEnumerable<TEntity> FindByIds(IEnumerable<TKey> ids);

        /// <summary>
        /// Gets an entity with given array primary keys.
        /// </summary>
        /// <param name="ids">Array primary keys of the entity.</param>
        /// <returns>Entity</returns>
        Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<TKey> ids);

        /// <summary>
        /// Gets an entity with given array primary keys.
        /// </summary>
        /// <param name="ids">Array primary keys of the entity.</param>
        /// <returns>Entity</returns>
        IEnumerable<TEntity> FindByIds(IEnumerable<string> ids);

        /// <summary>
        /// Gets an entity with given array primary keys.
        /// </summary>
        /// <param name="ids">Array primary keys of the entity.</param>
        /// <returns>Entity</returns>
        Task<IEnumerable<TEntity>> FindByIdsAsync(IEnumerable<string> ids);


        ///// <summary>
        ///// Gets collection of all entities.
        ///// </summary>
        ///// <returns>Collection of entities.</returns>
        //Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        Task<TEntity> Insert(TEntity entity);

        /// <summary>
        /// Updates an existing entity. 
        /// </summary>
        /// <param name="entity">Entity</param>
        Task<TEntity> Update(TEntity entity);
        TEntity UpdateSync(TEntity entity);

        /// <summary>
        /// Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity</param>
        /// <param name="beforeDeleteAction"></param>
        Task Delete(TKey id, Action<TEntity> beforeDeleteAction = null);
        void DeleteSync(TKey id, Action<TEntity> beforeDeleteAction = null);

        /// <summary>
        /// Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        /// <param name="beforeDeleteAction"></param>
        Task Delete(TEntity entity, Action<TEntity> beforeDeleteAction = null);
        void DeleteSync(TEntity entity, Action<TEntity> beforeDeleteAction = null);

        /// <summary>
        /// Gets quarable collection of all entities.
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> AsQueryable();
    }
}
