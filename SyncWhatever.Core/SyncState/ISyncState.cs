namespace SyncWhatever.Core.State
{
    /// <summary>
    ///     Describes entity state
    /// </summary>
    public interface ISyncState
    {
        /// <summary>
        ///     Context key allows to map states to certain task
        /// </summary>
        string SyncTaskId { get; set; }
        /// <summary>
        ///     Entity key is anything that allows to uniquely identify entity
        /// </summary>
        string EntityKey { get; set; }

        /// <summary>
        ///     Entity state is any string that defines entity state at given time.
        ///     State should change when entity changes - it can be anything (hash, modification date/time, publishing date, etc)
        /// </summary>
        string EntityState { get; set; }
    }


}