using System;

namespace Shared.Networking.Advanced.Entities.Messages
{
    /// <summary>
    /// Provides base for message model communication.
    /// </summary>
    [Serializable]
    public class CoreMessage
    {
        protected static long ID = 0;

        public CoreMessage()
        {
            Id = ++ID;
        }

        /// <summary>
        /// Provides base information about the order of the transactions.
        /// </summary>
        public long Id { get; }

        public override string ToString()
        {
            return "CoreMessage: " + Id;
        }
    }
}