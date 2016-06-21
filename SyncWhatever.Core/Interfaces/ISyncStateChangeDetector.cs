using System.Collections.Generic;
using SyncWhatever.Core.Implementation;

namespace SyncWhatever.Core.Interfaces
{
    public interface ISyncStateChangeDetector
    {
        IEnumerable<SyncStateChange> DetectChanges(IEnumerable<ISyncState> lastStates, IEnumerable<ISyncState> currentStates);
    }
}