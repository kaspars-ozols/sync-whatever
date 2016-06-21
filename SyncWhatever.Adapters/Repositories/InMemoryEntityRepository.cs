using System;
using System.Collections.Generic;
using System.Linq;
using SyncWhatever.Core.ChangeDetection;
using SyncWhatever.Core.Implementation;
using SyncWhatever.Core.Interfaces;
using SyncWhatever.Core.Interfaces.Composite;
using SyncWhatever.Core.State;

namespace SyncWhatever.Components.Repositories
{
    public class InMemoryEntityRepository<TEntity> : IEntityRepository<TEntity>, ISyncStateReader
    {
        private readonly Func<TEntity, string> _keySelector;
        private readonly Func<TEntity, string> _stateSelector;
        public readonly Dictionary<string, TEntity> Storage = new Dictionary<string, TEntity>();

        public InMemoryEntityRepository(Func<TEntity, string> keySelector, Func<TEntity, string> stateSelector)
        {
            _keySelector = keySelector;
            _stateSelector = stateSelector;
        }

        public TEntity ReadEntity(string entityKey)
        {
            if (Storage.ContainsKey(entityKey))
                return Storage[entityKey];

            return default(TEntity);
        }

        public string CreateEntity(TEntity entity)
        {
            var key = _keySelector(entity);
            Storage.Add(key, entity);
            return key;
        }

        public string UpdateEntity(TEntity entity)
        {
            var key = _keySelector(entity);
            Storage[key] = entity;
            return key;
        }

        public void DeleteEntity(TEntity entity)
        {
            var key = _keySelector(entity);
            Storage.Remove(key);
        }

        public IEnumerable<ISyncState> GetAllStates(string syncTaskId)
        {
            return Storage
                .Select(x => new SyncState(syncTaskId, x.Key, _stateSelector(x.Value)));
        }
    }
}