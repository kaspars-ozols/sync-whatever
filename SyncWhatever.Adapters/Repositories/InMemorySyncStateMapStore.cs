using System;
using System.Collections.Generic;
using System.Linq;
using SyncWhatever.Core.Implementation;
using SyncWhatever.Core.Interfaces;
using SyncWhatever.Core.State;
using SyncWhatever.Core.SyncState;

namespace SyncWhatever.Components.Repositories
{
    public class InMemorySyncStateMapRepository : ISyncStateStore
    {
        public List<ISyncState> Storage = new List<ISyncState>();

        public void Create(string syncTaskId, string entityKey, string entityState)
        {
            var syncKeyMap = new SyncState(syncTaskId, entityKey, entityState);
            Storage.Add(syncKeyMap);
        }

        public void Update(ISyncState syncState)
        {
            var existingSyncState = Storage.Single(x => x.SyncTaskId == syncState.SyncTaskId && x.EntityKey == syncState.EntityKey);
            existingSyncState.EntityState = syncState.EntityState;
        }

        public void Delete(ISyncState syncState)
        {
            var existingSyncState = Storage.Single(x => x.SyncTaskId == syncState.SyncTaskId && x.EntityKey == syncState.EntityKey);
            Storage.Remove(existingSyncState);
        }

        public IEnumerable<ISyncState> GetAllStates(string syncTaskId)
        {
            return Storage.Where(x => x.SyncTaskId == syncTaskId);
        }
    }
}