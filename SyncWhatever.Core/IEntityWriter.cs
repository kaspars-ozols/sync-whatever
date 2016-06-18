namespace SyncWhatever.Core
{
    /// <summary>
    ///     Provides write access (CRUD) to entity
    /// </summary>
    public interface IEntityWriter<TEntity>
    {
        /// <summary>
        ///     Creates entity in storage, returns created entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity CreateEntity(TEntity entity);

        /// <summary>
        ///     Updates entity in storage, returns updated entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        TEntity UpdateEntity(TEntity entity);

        /// <summary>
        ///     Deletes entity from storage
        /// </summary>
        /// <param name="entity"></param>
        void DeleteEntity(TEntity entity);
    }
}