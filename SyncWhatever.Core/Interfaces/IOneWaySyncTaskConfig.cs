namespace SyncWhatever.Core.Interfaces
{
    /// <summary>
    ///     Provides configuration to synchronization task
    /// </summary>
    public interface IOneWaySyncTaskConfig<TSourceEntity, TTargetEntity>
    {
        ISyncStateReader<TSourceEntity> SourceSyncStateReader { get; set; }
        ISyncStateReader<TSourceEntity> LastSourceSyncStateReader { get; set; }



        IEntityReader<TSourceEntity> SourceReader { get; set; }
        IEntityWriter<TSourceEntity> SourceWriter { get; set; }


        IEntityReader<TTargetEntity> TargetReader { get; set; }
        IEntityWriter<TTargetEntity> TargetWriter { get; set; }

        SyncTypeEnum SyncType { get; set; }
    }
}