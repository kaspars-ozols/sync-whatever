using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Core.Implementation
{
    public class SyncTask<TSourceEntity, TTargetEntity> : ISyncTask
    {
        private readonly ISyncTaskConfig<TSourceEntity, TTargetEntity> _config;
        private readonly SyncStateChangeDetector _syncStateChangeDetector;

        public SyncTask(ISyncTaskConfig<TSourceEntity, TTargetEntity> config)
        {
            _config = config;
            _syncStateChangeDetector = new SyncStateChangeDetector();
        }

        public void Execute()
        {
            // get sync state changes
            var changes = GetSyncStateChanges()
                .ToList();

            foreach (var change in changes)
            {
                var iteration = new SyncIteration<TSourceEntity, TTargetEntity>
                {
                    LastSyncState = change.LastSyncState,
                    CurrentSyncState = change.CurrentSyncState
                };

                // load source item
                iteration.SourceEntityKey = ResolveSourceKey(iteration);
                iteration.SourceEntity = ReadSourceEntity(iteration);

                // load sync key map
                iteration.SyncKeyMap = ResolveSyncKeyMap(iteration);

                //load target item
                iteration.TargetEntityKey = ResolveTargetKey(iteration);
                iteration.TargetEntity = ReadTargetEntity(iteration);

                // detect data operation
                iteration.Operation = DetectDataOperation(iteration);

                // perform mapping & store results
                iteration.TargetEntityKey = PerformDataOperation(iteration);

                // store sync map
                UpdateSyncMap(iteration);

                // store sync state
                UpdateSyncState(iteration);
            }
        }

        private ISyncKeyMap ResolveSyncKeyMap(SyncIteration<TSourceEntity, TTargetEntity> iteration)
        {
            if (iteration.SourceEntityKey == null)
                return null;

            return _config.KeyMapRepository.Read(_config.SyncTaskId, iteration.SourceEntityKey);
        }

        private IEnumerable<SyncStateChange> GetSyncStateChanges()
        {
            var lastStates = _config.StateRepository.GetAllStates();
            var currentStates = _config.CurrentStateReader.GetAllStates();
            return _syncStateChangeDetector.DetectChanges(lastStates, currentStates);
        }


        private string ResolveSourceKey(SyncIteration<TSourceEntity, TTargetEntity> iteration)
        {
            return iteration.CurrentSyncState?.EntityKey ?? iteration.LastSyncState?.EntityKey;
        }

        private string ResolveTargetKey(SyncIteration<TSourceEntity, TTargetEntity> iteration)
        {
            if (iteration.SyncKeyMap == null)
                return null;

            return iteration.SyncKeyMap.TargetKey;
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

        private string PerformDataOperation(SyncIteration<TSourceEntity, TTargetEntity> iteration)
        {
            switch (iteration.Operation)
            {
                case OperationEnum.Create:
                    var targetToCreate = _config.EntityMapper.MapNew(iteration.SourceEntity);
                    return _config.TargetWriter.CreateEntity(targetToCreate);
                case OperationEnum.Update:
                    var targetToDelete = _config.EntityMapper.MapExisting(iteration.SourceEntity, iteration.TargetEntity);
                    return _config.TargetWriter.UpdateEntity(targetToDelete);
                case OperationEnum.Delete:
                    _config.TargetWriter.DeleteEntity(iteration.TargetEntity);
                    return null;
                case OperationEnum.None:
                    break;
            }
            return null;
        }

        private void UpdateSyncMap(SyncIteration<TSourceEntity, TTargetEntity> iteration)
        {
            switch (iteration.Operation)
            {
                case OperationEnum.Create:
                case OperationEnum.Update:
                    if (iteration.SyncKeyMap == null)
                    {
                        _config.KeyMapRepository.Create(_config.SyncTaskId, iteration.SourceEntityKey, iteration.TargetEntityKey);
                    }
                    else
                    {
                        var syncKeyMap = iteration.SyncKeyMap;
                        syncKeyMap.TargetKey = iteration.TargetEntityKey;
                        _config.KeyMapRepository.Update(syncKeyMap);

                    }
                    break;

                case OperationEnum.None:
                case OperationEnum.Delete:
                    if (iteration.SyncKeyMap != null)
                    {
                        _config.KeyMapRepository.Delete(iteration.SyncKeyMap);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateSyncState(SyncIteration<TSourceEntity, TTargetEntity> iteration)
        {
            switch (iteration.Operation)
            {
                case OperationEnum.Create:
                case OperationEnum.Update:

                    if (iteration.LastSyncState == null)
                    {
                        _config.StateRepository.Create(iteration.CurrentSyncState.EntityKey, iteration.CurrentSyncState.EntityState);
                    }
                    else
                    {
                        var syncState = iteration.LastSyncState;
                        syncState.EntityState = iteration.CurrentSyncState.EntityState;
                        _config.StateRepository.Update(syncState);
                    }
                    break;

                case OperationEnum.None:
                case OperationEnum.Delete:
                    if (iteration.LastSyncState != null)
                    {
                        _config.StateRepository.Delete(iteration.LastSyncState);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


    }
}