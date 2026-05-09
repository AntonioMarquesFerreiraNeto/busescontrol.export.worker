using BusesControl.Export.Core.Entities;
using BusesControl.Export.Core.Enums;
using BusesControl.Export.Core.Interfaces;
using BusesControl.Export.Core.Request;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BusesControl.Export.Core.Services
{
    public class ExportService : IExportService
    {
        public ExportService(ILogger<ExportService> logger)
        {
            _logger = logger;
        }

        private readonly ILogger _logger;

        public async Task<bool> Execute(string message)
        {
            _logger.LogInformation("iniciando processamento da mensagem de exportação, request : {message}", message);

            var exportRequest = JsonSerializer.Deserialize<ExportMessageRequest<ExportModel>>(message);

            switch (exportRequest.Content.Type) 
            {
                case ExportTypeEnum.Contracts:
                {
                    //implementar exportação com closed e enviar ao storage.
                }
                break;
                case ExportTypeEnum.Financial:
                {
                    //implementar exportação com closed e enviar ao storage.
                }
                break;
            }

            return await Task.FromResult(true);
        }
    }
}
