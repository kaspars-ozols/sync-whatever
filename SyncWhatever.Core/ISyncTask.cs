namespace SyncWhatever.Core
{
    /// <summary>
    /// Describes syncronization task
    /// </summary>
    public interface ISyncTask
    {
        /// <summary>
        /// Will start synchronization process
        /// </summary>
        void Execute();
    }
}