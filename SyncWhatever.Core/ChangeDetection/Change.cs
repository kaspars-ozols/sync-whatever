namespace SyncWhatever.Core.ChangeDetection
{
    public class Change<T>
    {
        public ChangeType Type { get; set; }
        public T Before { get; set; }
        public T After { get; set; }
    }
}