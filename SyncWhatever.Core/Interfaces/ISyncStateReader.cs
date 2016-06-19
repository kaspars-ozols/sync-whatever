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
        /// <param name="syncTaskId"></param>
        /// <returns></returns>
        IEnumerable<ISyncState> GetAllStates(string syncTaskId);
    }
}