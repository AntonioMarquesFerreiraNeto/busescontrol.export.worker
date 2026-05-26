using BusesControl.Export.Core.Entities;
using BusesControl.Export.Core.Enums;
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
            IContractExportService contractExportService, 
            IFinancialExportService financialExportService, 
            ICustomerExportService customerExportService,
            IExportRepository exportRepository,
            IStorageRepository storageRepository,
            ILogger<ExportService> logger
        )
        {
            _contractExportService = contractExportService;
            _financialExportService = financialExportService;
            _customerExportService = customerExportService;
            _exportRepository = exportRepository;
            _storageRepository = storageRepository;
            _logger = logger;
        }

        private readonly IContractExportService _contractExportService;
        private readonly IFinancialExportService _financialExportService;
        private readonly ICustomerExportService _customerExportService;
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

                exportModel.Status = ExportStatusEnum.Completed;
                exportModel.ExpiresAt = DateTime.UtcNow.AddDays(30);
                exportModel.ExportedAt = DateTime.UtcNow;
                exportModel.Url = exportResponse.File.Name;
            }
            else
            {
                exportModel.Status = ExportStatusEnum.Failed;
                exportModel.ErrorMessage = messageError;
            }

            await _exportRepository.Update(exportModel);
        }

        public async Task<bool> Execute(string message)
        {
            _logger.LogInformation("iniciando processamento da exportação, request : {message}", message);

            var exportRequest = JsonSerializer.Deserialize<ExportMessageRequest<ExportModel>>(message);
            var exportResponse = new ExportResponse();

            var exportModel = exportRequest.Content;

            switch (exportRequest.Content.Type) 
            {
                case ExportTypeEnum.Contracts:
                    exportResponse = await _contractExportService.Execute(exportModel);
                break;
                case ExportTypeEnum.Financial:
                    exportResponse = await _financialExportService.Execute(exportModel);
                break;
                case ExportTypeEnum.Customers:
                    exportResponse = await _customerExportService.Execute(exportModel);
                break;
            }

            await Update(exportModel, exportResponse);

            _logger.LogInformation("exportação processada com sucesso, entity Id : {entityId}", exportModel.Id);

            return true;
        }
    }
}
