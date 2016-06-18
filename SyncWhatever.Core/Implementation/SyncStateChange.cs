using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Core.Implementation
{
    public class SyncStateChange
    {
        public ISyncState StateA { get; set; }
        public ISyncState StateB { get; set; }
        public OperationEnum ChangeType { get; set; }
    }
}