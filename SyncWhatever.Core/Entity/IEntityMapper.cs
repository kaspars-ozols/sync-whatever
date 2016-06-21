namespace SyncWhatever.Core.Interfaces
{
    public interface IEntityMapper<TSource, TTarget>
    {
        TTarget MapNew(TSource source);
        TTarget MapExisting(TSource source, TTarget target);
    }
}