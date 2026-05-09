using BusesControl.Export.Core.Entities;

namespace BusesControl.Export.Core.Interfaces
{
    public interface IExportRepository
    {
        Task Update(ExportModel export);
    }
}
