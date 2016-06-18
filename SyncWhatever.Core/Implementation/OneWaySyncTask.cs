using System;
using System.Collections.Generic;
using System.Linq;
using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Core.Implementation
{
    public class OneWaySyncTask<TSourceEntity, TTargetEntity> : ISyncTask
    {
        private readonly IOneWaySyncTaskConfig<TSourceEntity, TTargetEntity> _config;
        private readonly SyncStateChangeDetector _syncStateChangeDetector;

        public OneWaySyncTask(IOneWaySyncTaskConfig<TSourceEntity, TTargetEntity> config)
        {
            _config = config;
            _syncStateChangeDetector = new SyncStateChangeDetector();
        }

        public void Execute()
        {
            // get state changes
            var changes = GetStateChanges();

            foreach (var change in changes)
            {
                var iteration = new SyncIteration<TSourceEntity, TTargetEntity>();

                // load source item
                iteration.SourceEntityKey = ResolveSourceKey(iteration);
                iteration.SourceEntity = ReadSourceEntity(iteration);

                //load target item
                iteration.TargetEntityKey = ResolveTargetKey(iteration);
                iteration.TargetEntity = ReadTargetEntity(iteration);

                // detect data operation
                iteration.Operation = DetectDataOperation(iteration);

                // perform mapping
                // store results
                // store sync state
                // store sync map
            }
        }

        private IEnumerable<SyncStateChange> GetStateChanges()
        {
            var lastStates = _config.LastSourceSyncStateReader.GetAllStates();
            var currentStates = _config.SourceSyncStateReader.GetAllStates();
            return _syncStateChangeDetector.DetectChanges(lastStates, currentStates);
        }


        private string ResolveSourceKey(SyncIteration<TSourceEntity, TTargetEntity> iteration)
        {
            return iteration.CurrentSyncState?.EntityKey ?? iteration.LastSyncState?.EntityKey;
        }

        private string ResolveTargetKey(SyncIteration<TSourceEntity, TTargetEntity> iteration)
        {
            if (iteration.SourceEntityKey == null)
                return null;

            //stateChange.SyncKeyMap = SyncKeyMapStorage.GetBySourceKey(Context, stateChange.SourceKey);

            // stateChange.SyncKeyMap?.TargetKey;

            //TODO: implement fallback
            //if (iteration.SourceEntity != null && _config.tar)
            //{
            //    iteration.TargetEntity = _config.TargetReader.ReadEntity(iteration.SourceEntity);
            //}
            //TODO: implements
            return null;
        }

        private TSourceEntity ReadSourceEntity(SyncIteration<TSourceEntity, TTargetEntity> iteration)
        {
            if (iteration.SourceEntityKey == null)
                return default(TSourceEntity);

            return _config.SourceReader.ReadEntity(iteration.SourceEntityKey);
        }

        private TTargetEntity ReadTargetEntity(SyncIteration<TSourceEntity, TTargetEntity> iteration)
        {
            if (iteration.TargetEntityKey == null)
                return default(TTargetEntity);

            return _config.TargetReader.ReadEntity(iteration.TargetEntityKey);
        }

        private OperationEnum DetectDataOperation(SyncIteration<TSourceEntity, TTargetEntity> iteration)
        {
            if (iteration.SourceEntity != null && iteration.TargetEntity == null)
            {
                return OperationEnum.Create;
            }
            if (iteration.SourceEntity != null && iteration.TargetEntity != null)
            {
                return OperationEnum.Update;
            }
            if (iteration.SourceEntity == null && iteration.TargetEntity != null)
            {
                return OperationEnum.Delete;
            }
            return OperationEnum.None;
        }
    }
}