using System;

namespace Shared.Networking.DataEntities.LobbyEntities
{
    /// <summary>
    /// Represent all base states for lobbies.
    /// </summary>
    [Serializable]
    public enum LobbyState
    {
        /// <summary>
        /// Represents new lobby.
        /// Lobby that is show only to owner, allowing him to load or create new game.
        /// </summary>
        Created = 0,

        /// <summary>
        /// Represents lobby properly configured for playing.
        /// Players are joining.
        /// Rules are being set.
        /// </summary>
        Preparing = 1,

        /// <summary>
        /// Represents lobby where every member is ready to play.
        /// All data are synchronized.
        /// </summary>
        Ready = 2,

        /// <summary>
        /// Represents lobby currently in state of play.
        /// Game is being played.
        /// </summary>
        Running = 3,

        /// <summary>
        /// Represents lobby final state of lobby.
        /// Game is at leader-board screen.
        /// </summary>
        Finished = 4,

        /// <summary>
        /// Represents lobby that was ended before running.
        /// This is lobby that never reached Running state.
        /// </summary>
        Canceled = 5,
    }
}