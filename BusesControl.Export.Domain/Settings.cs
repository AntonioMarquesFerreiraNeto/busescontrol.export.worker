
namespace BusesControl.Export.Domain
{
    public class Settings
    {
        public string ExportQueue {  get; set; }
        public RabbitMq RabbitMq { get; set; }
        public Azure Azure  { get; set; }
    }

    public class RabbitMq
    {
        public string HostName { get; set; } 
        public string UserName { get; set; } 
        public string Password { get; set; }
    }

    public class Azure
    {
        public AzureStorage Storage { get; set; }
    }

    public class AzureStorage 
    {
        public string ContainerName { get; set; }
        public string ConnectionString { get; set; }
    }
}