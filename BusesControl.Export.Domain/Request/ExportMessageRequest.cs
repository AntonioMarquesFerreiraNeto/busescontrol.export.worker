namespace BusesControl.Export.Core.Request
{
    public class ExportMessageRequest<T>
    {
        public T Content { get; set; }
        public DateTime PublishedAt { get; set; }
    }
}
