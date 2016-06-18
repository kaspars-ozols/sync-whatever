namespace SyncWhatever.Core.Composite
{
    public interface IEntityRepository<TEntity> : IEntityReader<TEntity>, IEntityWriter<TEntity>
    {
    }
}