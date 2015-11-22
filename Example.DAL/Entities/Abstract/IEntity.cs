using System;

namespace Example.DAL.Entities.Abstract
{
    /// <summary>
    /// Defines interface for base entity type. Entities in the system should implement this interface.
    /// </summary>
    /// <typeparam name="TKey">Type of the primary key of the entity</typeparam>
    public interface IEntity<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        TKey Id { get; set; }
    }

    /// <summary>
    /// A shortcut of <see cref="IEntity{TKey}"/> for most used primary key type (<see cref="int"/>).
    /// </summary>
    public interface IIntEntity : IEntity<int>
    {

    }

    /// <summary>
    /// A shortcut of <see cref="IEntity{TKey}"/> for most used primary key type (<see cref="Guid"/>).
    /// </summary>
    public interface IGuidEntity : IEntity<Guid>
    {

    }

    /// <summary>
    /// A shortcut of <see cref="IEntity{TKey}"/> for most used primary key type (<see cref="string"/>).
    /// </summary>
    public interface IStringEntity : IEntity<string>
    {

    }
}
