using System;
using System.Collections.Generic;
using System.Linq;
using SyncWhatever.Core.Implementation;
using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Components.Repositories
{
    public class InMemorySyncStateMapRepository : ISyncStateRepository
    {
        public List<ISyncState> Storage = new List<ISyncState>();

        public void Create(string entityKey, string entityState)
        {
            var syncKeyMap = new SyncState(entityKey, entityState);
            Storage.Add(syncKeyMap);
        }

        public void Update(ISyncState syncState)
        {
            var existingSyncState = Storage.Single(x => x.EntityKey == syncState.EntityKey);
            existingSyncState.EntityState = syncState.EntityState;
        }

        public void Delete(ISyncState syncState)
        {
            var existingSyncState = Storage.Single(x => x.EntityKey == syncState.EntityKey);
            Storage.Remove(existingSyncState);
        }

        public IEnumerable<ISyncState> GetAllStates()
        {
            return Storage;
        }
    }
}
