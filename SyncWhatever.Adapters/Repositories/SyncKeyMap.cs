using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Components.Repositories
{
    public class SyncKeyMap : ISyncKeyMap
    {
        public SyncKeyMap(string syncTaskId, string sourceKey, string targetKey)
        {
            SyncTaskId = syncTaskId;
            SourceKey = sourceKey;
            TargetKey = targetKey;
        }

        public string SyncTaskId { get; set; }
        public string SourceKey { get; set; }
        public string TargetKey { get; set; }
    }
}