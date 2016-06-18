using System.Collections.Generic;
using System.Linq;
using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Core.Implementation
{
    public class SyncStateChangeDetector
    {
        public IEnumerable<SyncStateChange> DetectChanges(
            IEnumerable<ISyncState> lastStates,
            IEnumerable<ISyncState> currentStates)
        {
            foreach (var lastState in lastStates)
            {
                var currentState = currentStates
                    .SingleOrDefault(x => x.EntityKey == lastState.EntityKey);

                if (currentState == null)
                {
                    yield return new SyncStateChange
                    {
                        LastSyncState = lastState,
                        ChangeType = OperationEnum.Delete
                    };
                }
                else
                {
                    if (currentState.EntityState != lastState.EntityState)
                    {
                        yield return new SyncStateChange
                        {
                            LastSyncState = lastState,
                            CurrentSyncState = currentState,
                            ChangeType = OperationEnum.Update
                        };
                    }
                }
            }

            foreach (var currentState in currentStates)
            {
                var lastState = lastStates
                    .SingleOrDefault(x => x.EntityKey == currentState.EntityKey);

                if (lastState == null)
                {
                    yield return new SyncStateChange
                    {
                        CurrentSyncState = currentState,
                        ChangeType = OperationEnum.Create
                    };
                }
            }
        }
    }
}