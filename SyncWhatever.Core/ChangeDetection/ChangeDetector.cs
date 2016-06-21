using System;
using System.Collections.Generic;
using System.Linq;

namespace SyncWhatever.Core.ChangeDetection
{
    public class ChangeDetector : IChangeDetector
    {
        public IEnumerable<Change<T>> DetectChanges<T>(
            IEnumerable<T> before,
            IEnumerable<T> after,
            Func<T, T, bool> keyComparer,
            Func<T, T, bool> valueComparer)
        {
            foreach (var lastState in before)
            {
                var currentState = after
                    .SingleOrDefault(x => keyComparer(x, lastState));

                if (currentState == null)
                {
                    yield return new Change<T>
                    {
                        Type = ChangeType.Removed,
                        Before = lastState,
                    };
                }
                else
                {
                    if (!valueComparer(currentState, lastState))
                    {
                        yield return new Change<T>
                        {
                            Type = ChangeType.Modified,
                            Before = lastState,
                            After = currentState,
                        };
                    }
                }
            }

            foreach (var currentState in after)
            {
                var lastState = before
                    .SingleOrDefault(x => keyComparer(currentState, x));

                if (lastState == null)
                {
                    yield return new Change<T>
                    {
                        Type = ChangeType.Added,
                        After = currentState
                    };
                }
            }
        }
    }
}