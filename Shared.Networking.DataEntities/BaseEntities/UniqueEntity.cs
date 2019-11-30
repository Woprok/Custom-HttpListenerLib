using System;

namespace Shared.Networking.DataEntities.BaseEntities
{
    /// <summary>
    /// Base entity for any purpose.
    /// GetHashCode() is permanently set to 17^Id.GetHashCode().
    /// Equals & ToString should be provided by derived classes for better functionality.
    /// </summary>
    [Serializable]
    public class UniqueEntity
    {
        /// <summary>
        /// Standard new entity constructor.
        /// </summary>
        public UniqueEntity() { }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public UniqueEntity(UniqueEntity entity)
        {
            Id = entity.Id;
        }

        /// <summary>
        /// Provides unique identifier suitable for all common actions.
        /// </summary>
        public Guid Id { get; } = Guid.NewGuid();

        /// <inheritdoc/>
        public sealed override int GetHashCode() => 17^Id.GetHashCode();

        /// <summary>
        /// Does compare of two objects by all carried values.
        /// </summary>
        public override bool Equals(object obj) => obj is UniqueEntity entity && Equals(entity);

        /// <inheritdoc cref="Equals(object)"/>
        public bool Equals(UniqueEntity entity) => entity.Id == Id;

        /// <inheritdoc/>
        public override string ToString() => Id.ToString();
    }
}