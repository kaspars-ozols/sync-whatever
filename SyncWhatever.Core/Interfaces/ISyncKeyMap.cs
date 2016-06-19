namespace SyncWhatever.Core.Interfaces
{
    /// <summary>
    /// Describes mapping between two entity keys
    /// </summary>
    public interface ISyncKeyMap
    {
        string SyncTaskId { get; set; }
        string SourceKey { get; set; }
        string TargetKey { get; set; }
    }
}