using System.Collections.Generic;
using System.Linq;
using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Core.Implementation
{
    public class SyncStateChangeDetector
    {
        public IEnumerable<SyncStateChange> DetectChanges(
            IEnumerable<ISyncState> syncStatesA,
            IEnumerable<ISyncState> syncStatesB)
        {
            foreach (var syncStateA in syncStatesA)
            {
                var syncStateB = syncStatesB
                    .SingleOrDefault(x => x.EntityKey == syncStateA.EntityKey);

                if (syncStateB == null)
                {
                    yield return new SyncStateChange
                    {
                        LastSyncState = syncStateA,
                        ChangeType = OperationEnum.Delete
                    };
                }
                else
                {
                    if (syncStateB.EntityState != syncStateA.EntityState)
                    {
                        yield return new SyncStateChange
                        {
                            LastSyncState = syncStateA,
                            CurrentSyncState = syncStateB,
                            ChangeType = OperationEnum.Update
                        };
                    }
                }
            }

            foreach (var syncStateB in syncStatesB)
            {
                var syncStateA = syncStatesA
                    .SingleOrDefault(x => x.EntityKey == syncStateB.EntityKey);

                if (syncStateA == null)
                {
                    yield return new SyncStateChange
                    {
                        CurrentSyncState = syncStateB,
                        ChangeType = OperationEnum.Create
                    };
                }
            }
        }
    }
}