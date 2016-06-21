namespace SyncWhatever.Core.Interfaces
{
    /// <summary>
    ///     Provides write access (CRUD) to entity
    /// </summary>
    public interface IEntityWriter<TEntity>
    {
        /// <summary>
        ///     Creates entity in storage, returns created entity key
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string CreateEntity(TEntity entity);

        /// <summary>
        ///     Updates entity in storage, returns updated entity key
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        string UpdateEntity(TEntity entity);

        /// <summary>
        ///     Deletes entity from storage
        /// </summary>
        /// <param name="entity"></param>
        void DeleteEntity(TEntity entity);
    }
}