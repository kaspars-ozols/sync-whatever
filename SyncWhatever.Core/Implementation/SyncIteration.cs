using SyncWhatever.Core.Interfaces;
using SyncWhatever.Core.State;

namespace SyncWhatever.Core.Implementation
{
    internal class SyncIteration<TSourceEntity, TTargetEntity>
    {
        public ISyncState LastSyncState { get; set; }
        public ISyncState CurrentSyncState { get; set; }
        public string SourceEntityKey { get; set; }
        public string TargetEntityKey { get; set; }
        public ISyncKeyMap SyncKeyMap { get; set; }
        public TSourceEntity SourceEntity { get; set; }
        public TTargetEntity TargetEntity { get; set; }
        public OperationEnum Operation { get; set; }
    }
}