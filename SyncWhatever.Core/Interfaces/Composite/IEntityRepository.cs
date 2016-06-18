namespace SyncWhatever.Core.Interfaces.Composite
{
    public interface IEntityRepository<TEntity> : IEntityReader<TEntity>, IEntityWriter<TEntity>
    {
    }
}