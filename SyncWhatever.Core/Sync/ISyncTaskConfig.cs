using System;
using SyncWhatever.Core.ChangeDetection;
using SyncWhatever.Core.State;
using SyncWhatever.Core.SyncState;

namespace SyncWhatever.Core.Interfaces
{
    /// <summary>
    ///     Provides configuration to synchronization task
    /// </summary>
    public interface ISyncTaskConfig<TSource, TTarget>
    {
        ISyncStateReader CurrentStateReader { get; set; }
        IEntityReader<TSource> SourceReader { get; set; }
        IEntityWriter<TSource> SourceWriter { get; set; }

        ISyncStateStore StateStore { get; set; }
        ISyncKeyMapRepository KeyMapRepository { get; set; }

        IEntityReader<TTarget> TargetReader { get; set; }
        IEntityWriter<TTarget> TargetWriter { get; set; }
        IEntityMapper<TSource, TTarget> EntityMapper { get; set; }
        string SyncTaskId { get; set; }
        Action<string, TSource, TTarget> NestedTasks { get; set; }
        IChangeDetector ChangeDetector { get; set; }
    }
}