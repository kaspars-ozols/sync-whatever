namespace SyncWhatever.Core.Interfaces
{
    /// <summary>
    ///     Provides configuration to synchronization task
    /// </summary>
    public interface ISyncTaskConfig<TSourceEntity, TTargetEntity>
    {
        ISyncStateReader CurrentStateReader { get; set; }
        IEntityReader<TSourceEntity> SourceReader { get; set; }
        IEntityWriter<TSourceEntity> SourceWriter { get; set; }

        ISyncStateRepository StateRepository { get; set; }
        ISyncKeyMapRepository KeyMapRepository { get; set; }

        IEntityReader<TTargetEntity> TargetReader { get; set; }
        IEntityWriter<TTargetEntity> TargetWriter { get; set; }
        IEntityMapper<TSourceEntity, TTargetEntity> EntityMapper { get; set; }
        string SyncTaskId { get; set; }
    }
}