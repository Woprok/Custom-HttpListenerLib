using System;
using Shared.Networking.DataEntities.BaseEntities;
using Shared.Networking.DataEntities.NamedEntities;
#pragma warning disable 659

namespace Shared.Networking.DataEntities.LobbyEntities
{
    /// <summary>
    /// Standard lobby entity.
    /// </summary>
    [Serializable]
    public sealed class LobbyEntity : NamedEntity
    {
        public LobbyEntity(string lobbyName) : base(lobbyName) { }

        /// <inheritdoc cref="LobbyState"/>
        public LobbyState State { get; set; } = LobbyState.Created;

        /// <summary>
        /// Holds Id for LobbyConfigEntity.
        /// </summary>
        public UniqueEntity LobbyConfig { get; set; } = null;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is LobbyEntity entity && Equals(entity);

        /// <inheritdoc cref="Equals(object)"/>
        public bool Equals(LobbyEntity entity) => entity.Id == Id && entity.Name == Name && entity.State == State && entity.LobbyConfig.Equals(LobbyConfig);

        /// <inheritdoc/>
        public override string ToString() => Name;
    }
}