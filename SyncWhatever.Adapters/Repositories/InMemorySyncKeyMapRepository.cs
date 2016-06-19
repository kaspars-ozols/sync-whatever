using System.Collections.Generic;
using System.Linq;
using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Components.Repositories
{
    public class InMemorySyncKeyMapRepository : ISyncKeyMapRepository
    {
        public List<ISyncKeyMap> Storage = new List<ISyncKeyMap>();

        public ISyncKeyMap Read(string syncTaskId, string sourceKey)
        {
            return Storage
                .FirstOrDefault(x => x.SyncTaskId == syncTaskId && x.SourceKey == sourceKey);
        }

        public void Create(string syncTaskId, string sourceKey, string targetKey)
        {
            var syncKeyMap = new SyncKeyMap(syncTaskId, sourceKey, targetKey);
            Storage.Add(syncKeyMap);
        }

        public void Update(ISyncKeyMap syncKeyMap)
        {
            var existingSyncKeyMap = Storage.Single(x => x.SourceKey == syncKeyMap.SourceKey);
            existingSyncKeyMap.TargetKey = syncKeyMap.TargetKey;
        }

        public void Delete(ISyncKeyMap syncKeyMap)
        {
            var existingSyncKeyMap = Storage.Single(x => x.SourceKey == syncKeyMap.SourceKey);
            Storage.Remove(existingSyncKeyMap);
        }
    }
}