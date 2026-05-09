using BusesControl.Export.Core.Entities;
using BusesControl.Export.Core.Responses;

namespace BusesControl.Export.Core.Interfaces
{
    public interface IFinancialExportService
    {
        Task<ExportResponse> Export(ExportModel export);
    }
}
