namespace SyncWhatever.Core.Interfaces
{
    /// <summary>
    ///     Provides configuration to synchronization task
    /// </summary>
    public interface ISyncTaskConfig<TSourceEntity, TTargetEntity>
    {
        ISyncStateReader<TSourceEntity> CurrentStateReader { get; set; }
        ISyncStateReader<TSourceEntity> LastStateReader { get; set; }


        ISyncKeyMapReader<TSourceEntity> KeyMapReader { get; set; }
        ISyncKeyMapWriter<TSourceEntity> KeyMapWriter { get; set; }


        IEntityReader<TSourceEntity> SourceReader { get; set; }
        IEntityWriter<TSourceEntity> SourceWriter { get; set; }


        IEntityReader<TTargetEntity> TargetReader { get; set; }
        IEntityWriter<TTargetEntity> TargetWriter { get; set; }

    }
}