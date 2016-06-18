using SyncWhatever.Core;

namespace SyncWhatever.Components.FileSystem.Abstract
{
    public class FileSyncState : ISyncState
    {
        public string EntityKey { get; set; }
        public string EntityState { get; set; }
    }
}