using System.Collections.Generic;

namespace SyncWhatever.Core.Interfaces
{
    public interface ISyncKeyMapRepository
    {
        ISyncKeyMap Read(string syncTaskId, string sourceKey);
        void Create(string syncTaskId, string sourceKey, string targetKey);
        void Update(ISyncKeyMap syncKeyMap);
        void Delete(ISyncKeyMap keyMap);
    }
}