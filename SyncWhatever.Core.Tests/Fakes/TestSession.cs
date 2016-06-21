using System;
using System.Linq;
using FluentAssertions;
using SyncWhatever.Components.Repositories;
using SyncWhatever.Core.ChangeDetection;
using SyncWhatever.Core.Implementation;

namespace SyncWhatever.Core.Tests.Fakes
{
    public class TestSession
    {
        private readonly AccountSyncTarget _accountSyncTarget;

        private readonly EmployeeToUserMapper _employeeToUserMapper;
        private readonly InMemorySyncKeyMapRepository _keyMapRepository;
        private readonly OrganizationSyncSource _organizationSyncSource;
        private readonly OrganizationToAccountMapper _organizationToAccountMapper;
        private readonly Random _random;
        private readonly InMemorySyncStateMapRepository _stateStore;
        private readonly UserSyncTarget _userSyncTarget;
        private readonly ChangeDetector _changeDetector;


        public TestSession()
        {
            _random = new Random();

            _keyMapRepository = new InMemorySyncKeyMapRepository();
            _stateStore = new InMemorySyncStateMapRepository();

            _organizationSyncSource = new OrganizationSyncSource();
            _accountSyncTarget = new AccountSyncTarget();
            _organizationToAccountMapper = new OrganizationToAccountMapper();

            _userSyncTarget = new UserSyncTarget();
            _employeeToUserMapper = new EmployeeToUserMapper();

            _changeDetector = new ChangeDetector();
        }

        public void Sync()
        {
            var organizationSyncConfig = new SyncTaskConfig<Organization, Account>
            {
                SyncTaskId = "OrganizationToAccountSyncTask",
                SourceReader = _organizationSyncSource,
                CurrentStateReader = _organizationSyncSource,
                TargetReader = _accountSyncTarget,
                TargetWriter = _accountSyncTarget,
                KeyMapRepository = _keyMapRepository,
                StateStore = _stateStore,
                EntityMapper = _organizationToAccountMapper,
                ChangeDetector = _changeDetector,
                NestedTasks = (contextKey, organization, account) =>
                {
                    var syncSource = new EmployeeDataSource(organization);
                    var employeeSyncConfig = new SyncTaskConfig<Employee, User>
                    {
                        SyncTaskId = $"EmployeeToUserSyncTask-{contextKey}",
                        SourceReader = syncSource,
                        CurrentStateReader = syncSource,
                        TargetReader = _userSyncTarget,
                        TargetWriter = _userSyncTarget,
                        KeyMapRepository = _keyMapRepository,
                        StateStore = _stateStore,
                        EntityMapper = _employeeToUserMapper,
                        ChangeDetector = _changeDetector,
                        NestedTasks = null
                    };
                    new SyncTask<Employee, User>(employeeSyncConfig).Execute();
                }
            };
            new SyncTask<Organization, Account>(organizationSyncConfig).Execute();
        }

        public void CheckResults()
        {
            var expectedAccounts = _organizationSyncSource.Storage
                .Select(x => new Account
                {
                    Name = x.Name,
                    RegistrationNumber = x.RegistrationNumber
                });

            _accountSyncTarget.Storage
                .Select(x => new Account {Name = x.Name, RegistrationNumber = x.RegistrationNumber})
                .ShouldAllBeEquivalentTo(expectedAccounts);

            var expectedUsers = _organizationSyncSource.Storage
                .SelectMany(x => x.Employees)
                .Select(x => new User
                {
                    Username = x.Email,
                    FullName = x.FullName
                });

            _userSyncTarget.Storage
                .ShouldAllBeEquivalentTo(expectedUsers);

            //TODO: check states for nested
            //var expectedStates = _organizationSyncSource.GetAllStates("OrganizationToAccountSyncTask");

            //_stateRepository.Storage
            //    .ShouldAllBeEquivalentTo(expectedStates);

            //var expectedKeyMaps = _organizationSyncSource.Storage
            //    .Join(_accountSyncTarget.Storage, x => x.RegistrationNumber, y => y.RegistrationNumber,
            //        (x, y) => new SyncKeyMap("OrganizationToAccountSyncTask", x.Id.ToString(), y.Id.ToString()));

            //_keyMapRepository.Storage
            //    .ShouldAllBeEquivalentTo(expectedKeyMaps);
        }

        public void CreateRandomOrganization()
        {
            var guid = Guid.NewGuid();
            var organization = new Organization(guid, $"{guid}", $"{guid}");
            _organizationSyncSource.Storage.Add(organization);
        }

        public void UpdateRandomOrganization()
        {
            var organization = GetRandomOrganization();

            if (organization != null)
            {
                var guid = Guid.NewGuid();
                organization.Name = $"{guid}";
            }
        }

        public void DeleteRandomOrganization()
        {
            var organization = GetRandomOrganization();

            if (organization != null)
            {
                _organizationSyncSource.Storage.Remove(organization);
            }
        }

        private Organization GetRandomOrganization()
        {
            var skip = _random.Next(0, _organizationSyncSource.Storage.Count);
            return _organizationSyncSource.Storage.Skip(skip)
                .FirstOrDefault();
        }

        private Employee GetRandomEmployee(Organization organization)
        {
            var skip = _random.Next(0, organization.Employees.Count);
            return organization.Employees.Skip(skip)
                .FirstOrDefault();
        }

        public void CreateRandomEmployee()
        {
            var organization = GetRandomOrganization();
            if (organization != null)
            {
                var guid = Guid.NewGuid();
                var employee = new Employee(guid, $"{guid}", $"{guid}");
                organization.Employees.Add(employee);
            }
        }

        public void UpdateRandomEmployee()
        {
            var organization = GetRandomOrganization();
            if (organization != null)
            {
                var employee = GetRandomEmployee(organization);
                var guid = Guid.NewGuid();
                employee.FullName = $"{guid}";
            }
        }

        public void DeleteRandomEmployee()
        {
            var organization = GetRandomOrganization();
            if (organization != null)
            {
                var employee = GetRandomEmployee(organization);
                organization.Employees.Remove(employee);
            }
        }
    }
}