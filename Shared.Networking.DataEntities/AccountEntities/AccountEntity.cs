using System;
using Shared.Networking.DataEntities.NamedEntities;
#pragma warning disable 659

namespace Shared.Networking.DataEntities.AccountEntities
{
    /// <summary>
    /// Standard account entity.
    /// </summary>
    [Serializable]
    public sealed class AccountEntity : NamedEntity
    {
        /// <summary>
        /// Standard new entity constructor.
        /// </summary>
        public AccountEntity(string username, string password) : base(username)
        {
            Password = password;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        public AccountEntity(AccountEntity entity) : base(entity)
        {
            Password = entity.Password;
        }
        
        /// <summary>
        /// String that contains standard characters.
        /// </summary>
        public string Password { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is AccountEntity entity && Equals(entity);

        /// <inheritdoc cref="Equals(object)"/>
        public bool Equals(AccountEntity entity) => entity.Id == Id && entity.Name == Name && entity.Password == Password;
    }
}