using BusesControl.Export.Core.Enums;

namespace BusesControl.Export.Core.Entities
{
    public class ExportModel
    {
        public Guid Id { get; set; }
        public ExportTypeEnum Type { get; set; }
        public DocumentTypeEnum DocumentType { get; set; }
        public ExportStatusEnum Status { get; set; }
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExportedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string ErrorMessage { get; set; }

        public void Complete(string fileName)
        {
            Status = ExportStatusEnum.Completed;
            ExportedAt = DateTime.UtcNow;
            ExpiresAt = DateTime.UtcNow.AddDays(15);
            Url = fileName;
        }

        public void Fail(string message) 
        {
            Status = ExportStatusEnum.Failed;
            ErrorMessage = message;
        }
    }
}
