using System.Collections.Generic;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyncWhatever.Core.ChangeDetection;

namespace SyncWhatever.Core.Tests
{
    [TestClass]
    public class ChangeDetectorTests
    {
        [TestMethod]
        public void ShouldDetectAddedItems()
        {
            var before = new List<TestItem> {new TestItem(1, "A")};
            var after = new List<TestItem> {new TestItem(1, "A"), new TestItem(2, "B")};

            var sut = new ChangeDetector();
            var changes = sut.DetectChanges(before, after, TestItem.KeyComparer, TestItem.ValueComparer);

            var expected = new List<Change<TestItem>>
            {
                new Change<TestItem>
                {
                    Type = ChangeType.Added,
                    Before = null,
                    After = new TestItem(2, "B")
                }
            };
            changes.ShouldAllBeEquivalentTo(expected);
        }

        [TestMethod]
        public void ShouldDetectModifiedItems()
        {
            var before = new List<TestItem> {new TestItem(1, "A"), new TestItem(2, "B")};
            var after = new List<TestItem> {new TestItem(1, "A"), new TestItem(2, "BB")};

            var sut = new ChangeDetector();
            var changes = sut.DetectChanges(before, after, TestItem.KeyComparer, TestItem.ValueComparer);

            var expected = new List<Change<TestItem>>
            {
                new Change<TestItem>
                {
                    Type = ChangeType.Modified,
                    Before = new TestItem(2, "B"),
                    After = new TestItem(2, "BB")
                }
            };
            changes.ShouldAllBeEquivalentTo(expected);
        }

        [TestMethod]
        public void ShouldDetectRemovedItems()
        {
            var before = new List<TestItem> {new TestItem(1, "A"), new TestItem(2, "B")};
            var after = new List<TestItem> {new TestItem(1, "A")};

            var sut = new ChangeDetector();
            var changes = sut.DetectChanges(before, after, TestItem.KeyComparer, TestItem.ValueComparer);

            var expected = new List<Change<TestItem>>
            {
                new Change<TestItem>
                {
                    Type = ChangeType.Removed,
                    Before = new TestItem(2, "B"),
                    After = null
                }
            };
            changes.ShouldAllBeEquivalentTo(expected);
        }

        [TestMethod]
        public void ShouldDetectNoChangedItems()
        {
            var before = new List<TestItem> { new TestItem(1, "A"), new TestItem(2, "B") };
            var after = new List<TestItem> { new TestItem(1, "A"), new TestItem(2, "B") };

            var sut = new ChangeDetector();
            var changes = sut.DetectChanges(before, after, TestItem.KeyComparer, TestItem.ValueComparer);

            var expected = new List<Change<TestItem>>();
            changes.ShouldAllBeEquivalentTo(expected);
        }


    }
}