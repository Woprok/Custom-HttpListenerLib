using System;
using System.Collections.Generic;

namespace Shared.Networking.Advanced.Entities.Messages
{
    /// <summary>
    /// Type of change in data.
    /// </summary>
    [Serializable]
    public enum DataChange
    {
        /// <summary>
        /// Completely new data.
        /// </summary>
        New,

        /// <summary>
        /// Update of existing data.
        /// </summary>
        Updated,

        /// <summary>
        /// Existing data were deleted.
        /// </summary>
        Deleted
    }

    /// <summary>
    /// Message that contains type of change in data that are permanently stored.
    /// </summary>
    [Serializable]
    public class DataMessage : CoreMessage
    {
        public DataChange DataChange { get; set; }
    }

    /// <summary>
    /// Message that contains bundled data of single type.
    /// </summary>
    [Serializable]
    public class DataBundleMessage<T> : DataMessage
    {
        public DataBundle<T> DataBundle { get; set; }
    }

    /// <summary>
    /// Message that contains bundled data of single type.
    /// </summary>
    [Serializable]
    public class DataBundle<T>
    {
        public List<T> Data { get; set; }
    }
}