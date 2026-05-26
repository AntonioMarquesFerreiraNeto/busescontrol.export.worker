using BusesControl.Export.Core.Entities;
using BusesControl.Export.Core.Responses;

namespace BusesControl.Export.Core.Interfaces
{
    public interface ICustomerExportService
    {
        Task<ExportResponse> Execute(ExportModel export);
    }
}
