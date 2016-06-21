using System;
using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Core.Tests
{
    public class OrganizationToAccountMapper : IEntityMapper<Organization, Account>
    {
        public Account MapNew(Organization source)
        {
            var target = new Account()
            {
                Id = Guid.NewGuid()
            };
            return MapExisting(source, target);
        }

        public Account MapExisting(Organization source, Account target)
        {
            target.Name = source.Name;
            target.RegistrationNumber = source.RegistrationNumber;
            return target;
        }
    }
}