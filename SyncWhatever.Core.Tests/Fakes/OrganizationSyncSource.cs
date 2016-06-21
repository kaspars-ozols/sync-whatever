using System;
using System.Collections.Generic;
using System.Linq;
using SyncWhatever.Components.State;
using SyncWhatever.Core.Interfaces;
using SyncWhatever.Core.Interfaces.Composite;

namespace SyncWhatever.Core.Tests.Fakes
{
    public class OrganizationSyncSource : ISyncSource<Organization>
    {
        public List<Organization> Storage = new List<Organization>();

        public Organization ReadEntity(string entityKey)
        {
            var id = Guid.Parse(entityKey);
            return Storage
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<ISyncState> GetAllStates(string syncTaskId)
        {
            return Storage
                .Select(x => BinaryChecksum.Calculate(x, x.Id, syncTaskId));
        }
    }
}