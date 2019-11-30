using System;
using System.Collections.Generic;
using Shared.Networking.DataEntities.BaseEntities;
using Shared.Networking.DataEntities.NamedEntities;
#pragma warning disable 659

namespace Shared.Networking.DataEntities.LobbyEntities
{
    /// <summary>
    /// ToDo should fulfill:
    /// Provides configuration for lobbies.
    /// Can be reused by any lobby.
    /// Can be saved & edited.
    /// </summary>
    [Serializable]
    public class LobbyConfigEntity : NamedEntity
    {
        public LobbyConfigEntity(string configName) : base(configName) { }
        
        /// <summary>
        /// Max amount of players.
        /// </summary>
        public int MaxPlayerCount { get; set; }

        /// <summary>
        /// All players in the lobby
        /// ToDo this makes no sense!
        /// </summary>
        public HashSet<UniqueEntity> CurrentPlayers { get; set; } = new HashSet<UniqueEntity>();
        
        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is LobbyConfigEntity entity && Equals(entity);

        /// <inheritdoc cref="Equals(object)"/>
        public bool Equals(LobbyConfigEntity entity) => entity.Id == Id 
                                                        && entity.Name == Name
                                                        && entity.MaxPlayerCount == MaxPlayerCount 
                                                        && entity.CurrentPlayers.SetEquals(CurrentPlayers);
    }
}