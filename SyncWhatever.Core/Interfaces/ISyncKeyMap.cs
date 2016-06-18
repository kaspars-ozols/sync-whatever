namespace SyncWhatever.Core.Interfaces
{
    /// <summary>
    /// Describes mapping between two entity keys
    /// </summary>
    public interface ISyncKeyMap
    {
        string KeyA { get; set; }
        string KeyB { get; set; }
    }
}