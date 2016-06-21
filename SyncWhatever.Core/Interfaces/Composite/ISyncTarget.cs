namespace SyncWhatever.Core.Interfaces.Composite
{
    public interface ISyncTarget<TEntity> : IEntityReader<TEntity>, IEntityWriter<TEntity>
    {
    }
}