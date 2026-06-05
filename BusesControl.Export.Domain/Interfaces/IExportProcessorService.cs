using BusesControl.Export.Core.Entities;
using BusesControl.Export.Core.Enums;
using BusesControl.Export.Core.Responses;

namespace BusesControl.Export.Core.Interfaces
{
    public interface IExportProcessorService
    {
        ExportTypeEnum Type { get; }
        Task<ExportResponse> Execute(ExportModel export);
    }
}
