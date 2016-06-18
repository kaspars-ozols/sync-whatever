using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Core.Implementation
{
    public class OneWaySyncTask<TEntityA, TEntityB> : ISyncTask
    {
        private readonly ISyncTaskConfig<TEntityA, TEntityB> _config;

        public OneWaySyncTask(ISyncTaskConfig<TEntityA, TEntityB> config)
        {
            _config = config;
        }

        public void Execute()
        {


        }
    }
}