using SyncWhatever.Core.State;

namespace SyncWhatever.Core.Interfaces.Composite
{
    public interface ISyncSource<TEntity> : IEntityReader<TEntity>, ISyncStateReader
    {
    }
}