using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SyncWhatever.Core;
using SyncWhatever.Core.Interfaces;
using SyncWhatever.Core.Interfaces.Composite;

namespace SyncWhatever.Components.FileSystem.Abstract
{
    public abstract class AbstractFileRepository<TEntity> : IEntityRepository<TEntity>, ISyncStateReader<TEntity>
    {
        public Func<TEntity, string> EntityKeySelector { get; set; }
        public string BaseDirectory { get; set; }
        public abstract string Extension { get; set; }


        public TEntity ReadEntity(string entityKey)
        {
            var content = ReadFile(entityKey);
            return Deserialize(content);
        }

        public string CreateEntity(TEntity entity)
        {
            return UpdateEntity(entity);
        }

        public string UpdateEntity(TEntity entity)
        {
            var content = Serialize(entity);
            var entityKey = EntityKeySelector(entity);
            WriteFile(entityKey, content);
            return entityKey;
        }

        public void DeleteEntity(TEntity entity)
        {
            var entityKey = EntityKeySelector(entity);
            DeleteFile(entityKey);
        }

        public IEnumerable<ISyncState> GetAllStates()
        {
            return new DirectoryInfo(BaseDirectory)
                .EnumerateFiles()
                .Select(x => new FileSyncState
                {
                    EntityKey = ConvertFileNameToEntityKey(x),
                    EntityState = File.GetLastWriteTimeUtc(x.FullName).ToString("yyyy-MM-dd HH:mm:ss")
                });
        }

        private byte[] ReadFile(string entityKey)
        {
            var entityFile = ConvertEntityKeyToFileName(entityKey);
            var content = File.ReadAllBytes(entityFile.FullName);
            return content;
        }

        private void WriteFile(string entityKey, byte[] content)
        {
            var entityFile = ConvertEntityKeyToFileName(entityKey);
            File.WriteAllBytes(entityFile.FullName, content);
        }

        private void DeleteFile(string entityKey)
        {
            var entityFile = ConvertEntityKeyToFileName(entityKey);
            File.Delete(entityFile.FullName);
        }

        protected abstract byte[] Serialize(TEntity entity);
        protected abstract TEntity Deserialize(byte[] content);

        protected virtual FileInfo ConvertEntityKeyToFileName(string entityKey)
        {
            var fileName = $"{entityKey}{Extension}";
            var fullPath = Path.Combine(BaseDirectory, fileName);
            return new FileInfo(fullPath);
        }

        protected virtual string ConvertFileNameToEntityKey(FileInfo file)
        {
            return Path.GetFileNameWithoutExtension(file.FullName);
        }
    }
}