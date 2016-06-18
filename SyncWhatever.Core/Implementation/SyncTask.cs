using System;
using System.Collections.Generic;
using System.Linq;
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
            var changes = GetSyncStateChanges();

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

        private IEnumerable<SyncStateChange> GetSyncStateChanges()
        {
            var lastStates = _config.LastStateReader.GetAllStates();
            var currentStates = _config.CurrentStateReader.GetAllStates();
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

            //TODO: implement key map
            //iteration.SyncKeyMap = _config.KeyMapReader.GetBySourceKey(Context, stateChange.SourceKey);

            // stateChange.SyncKeyMap?.TargetKey;

            //TODO: implement fallback
            //if (iteration.SourceEntity != null && _config.tar)
            //{
            //    iteration.TargetEntity = _config.TargetReader.ReadEntity(iteration.SourceEntity);
            //}
            //TODO: implement properly
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

        private string PerformDataOperation(SyncIteration<TSourceEntity, TTargetEntity> iteration)
        {
            switch (iteration.Operation)
            {
                case OperationEnum.Create:
                    //iteration.TargetEntity = null; // TODO: mapper.map
                    return _config.TargetWriter.CreateEntity(iteration.TargetEntity);
                    break;
                case OperationEnum.Update:
                    //iteration.TargetEntity = null; // TODO: mapper.map
                    return _config.TargetWriter.UpdateEntity(iteration.TargetEntity);
                    break;
                case OperationEnum.Delete:
                    _config.TargetWriter.DeleteEntity(iteration.TargetEntity);
                    return null;
                    break;
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
                        //TODO: _config.KeyMapWriter.Create
                    }
                    else
                    {
                        var syncKeyMap = iteration.SyncKeyMap;
                        syncKeyMap.KeyB = iteration.TargetEntityKey;
                        //TODO: _config.KeyMapWriter.Update

                    }
                    break;

                case OperationEnum.None:
                case OperationEnum.Delete:
                    if (iteration.SyncKeyMap != null)
                    {
                        //TODO: _config.KeyMapWriter.Delete
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
                        //SyncStateStorage.Create(Context, iteration.CurrentSyncState.EntityKey, iteration.CurrentSyncState.EntityState);
                    }
                    else
                    {
                        var syncState = iteration.LastSyncState;
                        //syncState.Context = Context;
                        syncState.EntityState = iteration.CurrentSyncState.EntityState;
                        //SyncStateStorage.Update(syncState);
                    }
                    break;

                case OperationEnum.None:
                case OperationEnum.Delete:
                    if (iteration.LastSyncState != null)
                    {
                        //SyncStateStorage.Delete(iteration.LastSyncState);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


    }
}