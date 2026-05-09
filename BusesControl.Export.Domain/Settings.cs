
namespace BusesControl.Export.Domain
{
    public class Settings
    {
        public string ExportQueue {  get; set; }
        public RabbitMq RabbitMq { get; set; }
    }

    public class RabbitMq
    {
        public string HostName { get; set; } 
        public string UserName { get; set; } 
        public string Password { get; set; }
    }
}