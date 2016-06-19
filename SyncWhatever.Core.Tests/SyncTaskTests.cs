using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyncWhatever.Components.Repositories;
using SyncWhatever.Core.Implementation;

namespace SyncWhatever.Core.Tests
{
    [TestClass]
    public class SyncTaskTests
    {
        private readonly Random _random;

        public SyncTaskTests()
        {
            _random = new Random();
        }

        [TestMethod]
        public void ShouldSyncCreated()
        {
            PerformSynchronization(10, 10, CreateRandomEmployee);
        }

        [TestMethod]
        public void ShouldSyncUpdated()
        {
            PerformSynchronization(1, 100, CreateRandomEmployee);
            PerformSynchronization(1, 10, UpdateRandomEmployee);
        }

        [TestMethod]
        public void ShouldSyncDeleted()
        {
            PerformSynchronization(1, 100, CreateRandomEmployee);
            PerformSynchronization(1, 10, DeleteRandomEmployee);
        }


        [TestMethod]
        public void ShouldSyncRandom()
        {
            PerformSynchronization(100, 10, PerformRandomOperation);
        }

        private void PerformSynchronization(int rounds, int manipulationsPerRound,
            Action<InMemoryEntityRepository<Employee>> manipulation)
        {
            var employeeRepository = new InMemoryEntityRepository<Employee>(x => x.Id.ToString(),
                x => string.Join(",", x.FullName, x.Email));
            var userRepository = new InMemoryEntityRepository<User>(x => x.Username, x => x.FullName);
            var keyMapRepository = new InMemorySyncKeyMapRepository();
            var stateRepository = new InMemorySyncStateMapRepository();
            var mapper = new EmployeeUserMapper();

            var config = new SyncTaskConfig<Employee, User>
            {
                SyncTaskId = "UserEmployeeSyncTask",
                SourceReader = employeeRepository,
                SourceWriter = employeeRepository,
                CurrentStateReader = employeeRepository,
                TargetReader = userRepository,
                TargetWriter = userRepository,
                KeyMapRepository = keyMapRepository,
                StateRepository = stateRepository,
                EntityMapper = mapper
            };

            var syncTask = new SyncTask<Employee, User>(config);

            for (var i = 0; i < rounds; i++)
            {
                for (var j = 0; j < manipulationsPerRound; j++)
                {
                    manipulation(employeeRepository);
                }

                syncTask.Execute();

                CheckResults(config.SyncTaskId, employeeRepository, userRepository, keyMapRepository, stateRepository);
            }
        }

        private void CheckResults(string syncTaskId, InMemoryEntityRepository<Employee> employeeRepository,
            InMemoryEntityRepository<User> userRepository, InMemorySyncKeyMapRepository keyMapRepository,
            InMemorySyncStateMapRepository stateRepository)
        {
            var expectedUsers = employeeRepository.Storage
                .Select(x => x.Value)
                .Select(x => new User
                {
                    Username = x.Email,
                    FullName = x.FullName
                });

            userRepository.Storage
                .Select(x => x.Value)
                .ShouldAllBeEquivalentTo(expectedUsers);

            var expectedStates = employeeRepository.GetAllStates();

            stateRepository.Storage
                .ShouldAllBeEquivalentTo(expectedStates);

            var expectedKeyMaps = employeeRepository.Storage
                .Join(userRepository.Storage, x => x.Value.Email, y => y.Value.Username,
                    (x, y) => new SyncKeyMap(syncTaskId, x.Key, y.Key));

            keyMapRepository.Storage
                .ShouldAllBeEquivalentTo(expectedKeyMaps);
        }

        private void PerformRandomOperation(InMemoryEntityRepository<Employee> employeeRepository)
        {
            var operation = _random.Next(0, 3);

            switch (operation)
            {
                case 0:
                    CreateRandomEmployee(employeeRepository);
                    break;
                case 1:
                    UpdateRandomEmployee(employeeRepository);
                    break;
                case 2:
                    DeleteRandomEmployee(employeeRepository);
                    break;
            }
        }

        private void CreateRandomEmployee(InMemoryEntityRepository<Employee> employeeRepository)
        {
            var employee = new Employee(Guid.NewGuid(), $"{Guid.NewGuid()}", $"{Guid.NewGuid()}@example.com");
            employeeRepository.CreateEntity(employee);
        }

        private void UpdateRandomEmployee(InMemoryEntityRepository<Employee> employeeRepository)
        {
            var skip = _random.Next(0, employeeRepository.Storage.Count);
            var employee = employeeRepository.Storage.Skip(skip)
                .Select(x => x.Value)
                .FirstOrDefault();

            if (employee != null)
            {
                employee.FullName = $"{Guid.NewGuid()}";
                employeeRepository.UpdateEntity(employee);
            }
        }

        private void DeleteRandomEmployee(InMemoryEntityRepository<Employee> employeeRepository)
        {
            var skip = _random.Next(0, employeeRepository.Storage.Count);
            var employee = employeeRepository.Storage.Skip(skip)
                .Select(x => x.Value)
                .FirstOrDefault();

            if (employee != null)
            {
                employeeRepository.DeleteEntity(employee);
            }
        }
    }
}