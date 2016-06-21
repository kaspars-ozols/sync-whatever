namespace SyncWhatever.Core.Tests
{
    public class TestItem
    {
        public TestItem(int key, string value)
        {
            Key = key;
            Value = value;
        }

        public int Key { get; }
        public string Value { get; }

        public static bool KeyComparer(TestItem a, TestItem b)
        {
            return a.Key == b.Key;
        }

        public static bool ValueComparer(TestItem a, TestItem b)
        {
            return a.Value == b.Value;
        }
    }
}