using System.Collections.Generic;
using SyncWhatever.Core.State;

namespace SyncWhatever.Core.SyncState
{
    public interface ISyncStateStore : ISyncStateReader
    {
        void Create(string syncTaskId, string entityKey, string entityState);
        void Update(ISyncState syncState);
        void Delete(ISyncState syncState);
    }
}