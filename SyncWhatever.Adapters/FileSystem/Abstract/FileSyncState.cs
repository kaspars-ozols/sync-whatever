using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Components.FileSystem.Abstract
{
    public class FileSyncState : ISyncState
    {
        public string SyncTaskId { get; set; }
        public string EntityKey { get; set; }
        public string EntityState { get; set; }
    }
}