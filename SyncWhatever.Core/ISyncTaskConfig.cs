namespace SyncWhatever.Core
{
    /// <summary>
    /// Provides configuration to synchronization task
    /// </summary>
    public interface ISyncTaskConfig<TSourceEntity, TTargetEntity>
    {
        ISyncStateReader<TSourceEntity> SourceSyncStateReader { get; set; }
        IEntityReader<TSourceEntity> SourceReader { get; set; }
        IEntityWriter<TSourceEntity> SourceWriter { get; set; }


        ISyncStateReader<TTargetEntity> TargetSyncStateReader { get; set; }
        IEntityReader<TTargetEntity> TargetReader { get; set; }
        IEntityWriter<TTargetEntity> TargetWriter { get; set; }
    }
}