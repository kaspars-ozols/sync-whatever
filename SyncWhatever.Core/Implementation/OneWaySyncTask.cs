using System;
using System.Collections.Generic;
using System.Linq;
using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Core.Implementation
{
    public class OneWaySyncTask<TSourceEntity, TTargetEntity> : ISyncTask
    {
        private readonly IOneWaySyncTaskConfig<TSourceEntity, TTargetEntity> _config;
        private SyncStateChangeDetector _syncStateChangeDetector;

        public OneWaySyncTask(IOneWaySyncTaskConfig<TSourceEntity, TTargetEntity> config)
        {
            _config = config;
            _syncStateChangeDetector = new SyncStateChangeDetector();
        }

        public void Execute()
        {
            // get state changes
            var iterations = GetStateChanges();

            foreach(var iteration in iterations)
            {


                // load source and target items
                // detect data operation
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
    }
}