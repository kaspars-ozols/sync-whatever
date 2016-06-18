using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Core.Implementation
{
    public class SyncStateChange
    {
        public ISyncState LastSyncState { get; set; }
        public ISyncState CurrentSyncState { get; set; }
        public OperationEnum ChangeType { get; set; }
    }
}