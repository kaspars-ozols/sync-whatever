using System;
using System.Collections.Generic;
using System.Linq;
using SyncWhatever.Components.State;
using SyncWhatever.Core.Interfaces;
using SyncWhatever.Core.Interfaces.Composite;
using SyncWhatever.Core.State;

namespace SyncWhatever.Core.Tests.Fakes
{
    public class EmployeeDataSource : ISyncSource<Employee>
    {
        private readonly Organization _parentOrganization;

        public EmployeeDataSource(Organization parentOrganization)
        {
            _parentOrganization = parentOrganization;
        }

        public IEnumerable<ISyncState> GetAllStates(string syncTaskId)
        {
            if (_parentOrganization == null)
                return Enumerable.Empty<ISyncState>();

            return _parentOrganization.Employees
               .Select(x => BinaryChecksum.Calculate(x, x.Id, syncTaskId));
        }

        public Employee ReadEntity(string entityKey)
        {
            if (_parentOrganization == null)
                return null;

            var id = Guid.Parse(entityKey);
            return _parentOrganization.Employees
                .FirstOrDefault(x => x.Id == id);
        }
    }
}