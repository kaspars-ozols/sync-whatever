namespace SyncWhatever.Core.State
{
    public interface ISyncStateRepository: ISyncStateReader
    {
        void Create(string syncTaskId, string entityKey, string entityState);
        void Update(ISyncState syncState);
        void Delete(ISyncState syncState);
    }
}