using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyncWhatever.Core.Implementation;
using SyncWhatever.Core.Interfaces;

namespace SyncWhatever.Core.Tests
{
    [TestClass]

    public class SyncStateChangeDetectorTests
    {
        [TestMethod]
        public void ShouldDetectCreates()
        {
            var lastStates = new List<ISyncState>();
            var currentStates = new List<ISyncState> { new SyncState("2", "B") };

            var sut = new SyncStateChangeDetector();

            var changes = sut.DetectChanges(lastStates, currentStates);
            changes.Should().HaveCount(1);
            changes.Should().Contain(x => x.ChangeType == OperationEnum.Create);
        }

        [TestMethod]
        public void ShouldDetectUpdates()
        {
            var lastStates = new List<ISyncState> { new SyncState("1", "A") };
            var currentStates = new List<ISyncState> { new SyncState("1", "AAA") };

            var sut = new SyncStateChangeDetector();

            var changes = sut.DetectChanges(lastStates, currentStates);
            changes.Should().HaveCount(1);
            changes.Should().Contain(x => x.ChangeType == OperationEnum.Update);
        }

        [TestMethod]
        public void ShouldDetectDeletes()
        {
            var lastStates = new List<ISyncState> { new SyncState("1", "A") };
            var currentStates = new List<ISyncState>();

            var sut = new SyncStateChangeDetector();

            var changes = sut.DetectChanges(lastStates, currentStates);
            changes.Should().HaveCount(1);
            changes.Should().Contain(x => x.ChangeType == OperationEnum.Delete);
        }

        [TestMethod]
        public void ShouldDetectNoChanges()
        {
            var lastStates = new List<ISyncState> { new SyncState("1", "A") };
            var currentStates = new List<ISyncState> { new SyncState("1", "A") };

            var sut = new SyncStateChangeDetector();

            var changes = sut.DetectChanges(lastStates, currentStates);
            changes.Should().HaveCount(0);
        }
    }
}