using System.Collections.Generic;

namespace SyncWhatever.Core
{
    /// <summary>
    /// Provides states for given entity
    /// </summary>
    public interface ISyncStateReader<TEntity>
    {
        /// <summary>
        /// Returns all known states for given entity
        /// </summary>
        /// <returns></returns>
        IEnumerable<ISyncState> GetAllStates();

    }
}