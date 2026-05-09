using BusesControl.Export.Core.Entities;
using BusesControl.Export.Core.Interfaces;
using BusesControl.Export.Core.Responses;
using Microsoft.Extensions.Logging;

namespace BusesControl.Export.Core.Services
{
    public class FinancialExportService : IFinancialExportService
    {
        public FinancialExportService(IFinancialRepository financialRepository, ILogger<FinancialExportService> logger)
        {
            _financialRepository = financialRepository;
            _logger = logger;
        }

        private readonly IFinancialRepository _financialRepository;
        private readonly ILogger _logger;

        public async Task<ExportResponse> Export(ExportModel export)
        {
            try
            {
                await Task.Delay(0);
                return ExportResponse.Ok(new FileResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError("falha inesperada ao tentar gerar excel, detalhes do erro : {erro}", ex);
                return ExportResponse.Failed("falha inesperada ao processar a exportação, consulte o time de suporte para mais detalhes.");
            }
        }
    }
}
