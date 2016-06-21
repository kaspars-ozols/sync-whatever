namespace SyncWhatever.Core.State
{
    public class SyncState : ISyncState
    {
        public SyncState(string syncTaskId, string entityKey, string entityState)
        {
            SyncTaskId = syncTaskId;
            EntityKey = entityKey;
            EntityState = entityState;
        }

        public string SyncTaskId { get; set; }
        public string EntityKey { get; set; }
        public string EntityState { get; set; }
    }
}