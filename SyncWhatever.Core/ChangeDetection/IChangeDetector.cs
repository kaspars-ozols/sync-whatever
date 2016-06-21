using System;
using System.Collections.Generic;

namespace SyncWhatever.Core.ChangeDetection
{
    public interface IChangeDetector
    {
        IEnumerable<Change<T>> DetectChanges<T>(
            IEnumerable<T> before,
            IEnumerable<T> after,
            Func<T, T, bool> keyComparer,
            Func<T, T, bool> valueComparer);
    }
}