using System;
using Shared.Networking.DataEntities.BaseEntities;
#pragma warning disable 659

namespace Shared.Networking.DataEntities.NamedEntities
{
    /// <summary>
    /// Standard entity used for all objects that carries string representing user friendly name.
    /// </summary>
    [Serializable]
    public class NamedEntity : UniqueEntity
    {
        /// <summary>
        /// Standard new entity constructor.
        /// </summary>
        public NamedEntity(string name) : base()
        {
            Name = name;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public NamedEntity(NamedEntity entity) : base(entity)
        {
            Name = entity.Name;
        }

        /// <summary>
        /// String that contains standard characters and represents name that can be displayed to user.
        /// </summary>
        public string Name { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is NamedEntity entity && Equals(entity);

        /// <inheritdoc cref="Equals(object)"/>
        public bool Equals(NamedEntity entity) => entity.Id == Id && entity.Name == Name;

        /// <inheritdoc/>
        public override string ToString() => Name;
    }
}