﻿namespace SyncWhatever.Core.Interfaces
{
    public interface ISyncStateWriter<TEntity>
    {
        ISyncState Insert(ISyncState syncState);
        ISyncState Update(ISyncState syncState);
        void Delete(ISyncState syncState);
    }
}