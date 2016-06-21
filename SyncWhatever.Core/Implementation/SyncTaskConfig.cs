using System;
using SyncWhatever.Core.ChangeDetection;
using SyncWhatever.Core.Interfaces;
using SyncWhatever.Core.State;

namespace SyncWhatever.Core.Implementation
{
    public class SyncTaskConfig<TSource, TTarget> : ISyncTaskConfig<TSource, TTarget>
    {
        public ISyncStateReader CurrentStateReader { get; set; }
        public ISyncStateRepository StateRepository { get; set; }
        public ISyncKeyMapRepository KeyMapRepository { get; set; }
        public IEntityReader<TSource> SourceReader { get; set; }
        public IEntityWriter<TSource> SourceWriter { get; set; }
        public IEntityReader<TTarget> TargetReader { get; set; }
        public IEntityWriter<TTarget> TargetWriter { get; set; }
        public IEntityMapper<TSource, TTarget> EntityMapper { get; set; }
        public string SyncTaskId { get; set; }
        public Action<string, TSource, TTarget> NestedTasks { get; set; }
        public IChangeDetector ChangeDetector { get; set; }
    }
}