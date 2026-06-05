using BusesControl.Export.Core.Entities;
using BusesControl.Export.Core.Interfaces;
using BusesControl.Export.Core.Request;
using BusesControl.Export.Core.Responses;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BusesControl.Export.Core.Services
{
    public class ExportService : IExportService
    {
        public ExportService(
            IEnumerable<IExportProcessorService> exportProcessorService,
            IExportRepository exportRepository,
            IStorageRepository storageRepository,
            ILogger<ExportService> logger
        )
        {
            _exportProcessorService = exportProcessorService;
            _exportRepository = exportRepository;
            _storageRepository = storageRepository;
            _logger = logger;
        }

        private readonly IEnumerable<IExportProcessorService> _exportProcessorService;
        private readonly IExportRepository _exportRepository;
        private readonly IStorageRepository _storageRepository;
        private readonly ILogger _logger;

        private async Task Update(ExportModel exportModel, ExportResponse exportResponse)
        {
            var success = exportResponse.Success;
            var messageError = exportResponse.Message;

            if (success)
            {
                await _storageRepository.Upload(exportResponse.File.Name, exportResponse.File.ContentType, exportResponse.File.Content);
                exportModel.Complete(exportResponse.File.Name);
            }
            else
            {
                exportModel.Fail(messageError);
            }

            await _exportRepository.Update(exportModel);
        }

        public async Task<bool> Execute(string message)
        {
            _logger.LogInformation("iniciando processamento da exportação, request : {message}", message);

            var exportRequest = JsonSerializer.Deserialize<ExportMessageRequest<ExportModel>>(message);

            var exportModel = exportRequest.Content;

            var exportProcessorService = _exportProcessorService.First(x => x.Type == exportModel.Type);
            var exportResponse = await exportProcessorService.Execute(exportModel);

            await Update(exportModel, exportResponse);

            _logger.LogInformation("exportação processada com sucesso, entity Id : {entityId}", exportModel.Id);

            return true;
        }
    }
}
