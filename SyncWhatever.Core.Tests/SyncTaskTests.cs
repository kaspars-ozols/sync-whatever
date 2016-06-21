using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyncWhatever.Components.Repositories;
using SyncWhatever.Core.Implementation;
using SyncWhatever.Core.Tests.Fakes;

namespace SyncWhatever.Core.Tests
{
    [TestClass]
    public class SyncTaskTests
    {
        [TestMethod]
        public void ShouldSyncEmpty()
        {
            var session = new TestSession();
            session.Sync();
            session.CheckResults();
        }

        [TestMethod]
        public void ShouldSyncCreatedOrganization()
        {
            var session = new TestSession();
            session.CreateRandomOrganization();
            session.Sync();
            session.CheckResults();
        }

        [TestMethod]
        public void ShouldSyncUpdatedOrganization()
        {
            var session = new TestSession();
            session.CreateRandomOrganization();
            session.UpdateRandomOrganization();
            session.Sync();
            session.CheckResults();
        }

        [TestMethod]
        public void ShouldSyncDeletedOrganization()
        {
            var session = new TestSession();
            session.CreateRandomOrganization();
            session.DeleteRandomOrganization();
            session.Sync();
            session.CheckResults();
        }

        [TestMethod]
        public void ShouldSyncNestedInserts()
        {
            var session = new TestSession();
            session.CreateRandomOrganization();
            session.CreateRandomEmployee();
            session.Sync();
            session.CheckResults();
        }

        [TestMethod]
        public void ShouldSyncNestedUpdates()
        {
            var session = new TestSession();
            session.CreateRandomOrganization();
            session.CreateRandomEmployee();
            session.Sync();
            session.CheckResults();
            session.UpdateRandomOrganization();
            session.UpdateRandomEmployee();
            session.Sync();
            session.CheckResults();
        }

        [TestMethod]
        public void ShouldSyncNestedDeletes()
        {
            var session = new TestSession();
            session.CreateRandomOrganization();
            session.CreateRandomEmployee();
            session.Sync();
            session.CheckResults();
            session.DeleteRandomEmployee();
            session.DeleteRandomOrganization();
            session.Sync();
            session.CheckResults();
        }
    }
}