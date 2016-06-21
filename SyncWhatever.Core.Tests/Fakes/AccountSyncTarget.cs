using System.Collections.Generic;
using System.Linq;
using SyncWhatever.Core.Interfaces.Composite;

namespace SyncWhatever.Core.Tests.Fakes
{
    public class AccountSyncTarget : ISyncTarget<Account>
    {
        public List<Account> Storage = new List<Account>();

        public string CreateEntity(Account entity)
        {
            Storage.Add(entity);
            return entity.Id.ToString();
        }

        public string UpdateEntity(Account entity)
        {
            var existingEntity = Storage.Single(x => x.Id == entity.Id);
            existingEntity.Name = entity.Name;
            existingEntity.RegistrationNumber = entity.RegistrationNumber;
            return existingEntity.Id.ToString();
        }

        public void DeleteEntity(Account entity)
        {
            var existingEntity = Storage.Single(x => x.Id == entity.Id);
            Storage.Remove(existingEntity);
        }

        public Account ReadEntity(string entityKey)
        {
            return Storage.FirstOrDefault(x => x.Id.ToString() == entityKey);
        }
    }
}