namespace BusesControl.Export.Core.Interfaces
{
    public interface IExportService
    {
        Task<bool> Execute(string message);
    }
}
