namespace SyncWhatever.Core
{
    /// <summary>
    /// Provides read access to entity
    /// </summary>
    public interface IEntityReader<out TEntity>
    {
        /// <summary>
        /// Provides entity by entity key
        /// </summary>
        /// <param name="entityKey"></param>
        /// <returns></returns>
        TEntity GetEntity(string entityKey);
    }
}