using System.Collections.Generic;

namespace SyncWhatever.Core.Interfaces
{
    /// <summary>
    ///     Provides states for given entity
    /// </summary>
    public interface ISyncStateReader
    {
        /// <summary>
        ///     Returns all known states for given entity
        /// </summary>
        /// <returns></returns>
        IEnumerable<ISyncState> GetAllStates();
    }
}