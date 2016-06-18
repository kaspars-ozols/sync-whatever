using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Core.Implementation
{
    public class SyncState : ISyncState
    {
        public SyncState(string entityKey, string entityState)
        {
            EntityKey = entityKey;
            EntityState = entityState;
        }
        public string EntityKey { get; set; }
        public string EntityState { get; set; }
    }
}