using SyncWhatever.Core;
using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Components.FileSystem.Abstract
{
    public class FileSyncState : ISyncState
    {
        public string EntityKey { get; set; }
        public string EntityState { get; set; }
    }
}