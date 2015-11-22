using System;
using System.ComponentModel.DataAnnotations;

namespace Example.DAL.Entities.Abstract
{
    /// <summary>
    /// Basic implementation of IEntity interface.
    /// An entity can inherit this class of directly implement to IEntity interface.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity.</typeparam>
    public abstract class Entity<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        [Key]
        public virtual TKey Id { get; set; }
    }

    /// <summary>
    /// A shortcut of <see cref="Entity{TKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    public abstract class IntEntity : Entity<int>, IIntEntity
    {

    }

    /// <summary>
    /// A shortcut of <see cref="Entity{TKey}"/> for most used primary key type (<see cref="Guid"/>).
    /// </summary>
    public abstract class GuidEntity : Entity<Guid>, IGuidEntity
    {

    }

    /// <summary>
    /// A shortcut of <see cref="Entity{TKey}"/> for most used primary key type (<see cref="string"/>).
    /// </summary>
    public abstract class StringEntity : Entity<string>, IStringEntity
    {

    }
}
