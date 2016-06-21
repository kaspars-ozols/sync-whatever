using System.Collections.Generic;
using System.Linq;
using SyncWhatever.Core.Interfaces.Composite;

namespace SyncWhatever.Core.Tests.Fakes
{
    public class UserSyncTarget : ISyncTarget<User>
    {
        public List<User> Storage = new List<User>();

        public string CreateEntity(User entity)
        {
            Storage.Add(entity);
            return entity.Username;
        }

        public string UpdateEntity(User entity)
        {
            var existingEntity = Storage.Single(x => x.Username == entity.Username);
            existingEntity.FullName = entity.FullName;
            return existingEntity.Username;
        }

        public void DeleteEntity(User entity)
        {
            var existingEntity = Storage.Single(x => x.Username == entity.Username);
            Storage.Remove(existingEntity);
        }

        public User ReadEntity(string entityKey)
        {
            return Storage.FirstOrDefault(x => x.Username.ToString() == entityKey);
        }
    }
}