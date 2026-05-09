using BusesControl.Export.Core.Entities;
using BusesControl.Export.Core.Interfaces;
using BusesControl.Export.Core.Responses;
using ClosedXML.Excel;
using EnumsNET;
using Microsoft.Extensions.Logging;

namespace BusesControl.Export.Core.Services
{
    public class ContractExportService : IContractExportService
    {
        public ContractExportService(IContractRepository contractRepository, ILogger<ContractExportService> logger)
        {
            _contractRepository = contractRepository;
            _logger = logger;
        }

        private readonly IContractRepository _contractRepository;
        private readonly ILogger _logger;

        public async Task<ExportResponse> Execute(ExportModel export)
        {
            try
            {
                var contracts = await _contractRepository.GetAll();
                if (!contracts.Any())
                {
                    return ExportResponse.Failed("Nenhum contrato foi encontrado.");
                }

                var sheetBook = new XLWorkbook();
                var sheet = sheetBook.Worksheets.Add("Sample sheet");

                sheet.Cell(1, "A").Value = "Referência";
                sheet.Cell(1, "B").Value = "Situação";
                sheet.Cell(1, "C").Value = "Aprovação";
                sheet.Cell(1, "D").Value = "Aprovador";
                sheet.Cell(1, "E").Value = "Data de início";
                sheet.Cell(1, "F").Value = "Data de término";
                sheet.Cell(1, "G").Value = "Motorista Titular";
                sheet.Cell(1, "H").Value = "Ônibus Titular";
                sheet.Cell(1, "I").Value = "Tipo de pagamento";
                sheet.Cell(1, "J").Value = "Valor Total";
                sheet.Cell(1, "K").Value = "Clientes vinculados";

                var columns = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K" };

                foreach (var column in columns)
                {
                    var col = sheet.Column(column);

                    col.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    col.Width = 25;

                    var title = sheet.Cell(1, column);
                    title.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    title.Style.Font.SetBold();
                    title.Style.Font.FontColor = XLColor.DarkBlue;
                }

                var colApprover = sheet.Column("D");
                var colDriver = sheet.Column("G");
                colApprover.Width = 35;
                colDriver.Width = 35;

                var index = 2;

                foreach (var contract in contracts)
                {
                    sheet.Cell(index, "A").Value = contract.Reference;
                    sheet.Cell(index, "B").Value = contract.Status.AsString(EnumFormat.Description);
                    sheet.Cell(index, "C").Value = contract.IsApproved ? "Aprovado" : "Não aprovado";
                    sheet.Cell(index, "D").Value = contract.ApproverName ?? "Não possui";
                    sheet.Cell(index, "E").Value = contract.StartDate is not null ? contract.StartDate.Value.ToString("dd/MM/yyyy") : "Não possui";
                    sheet.Cell(index, "F").Value = contract.TerminateDate.ToString("dd/MM/yyyy");
                    sheet.Cell(index, "G").Value = contract.DriverName;
                    sheet.Cell(index, "H").Value = $"{contract.BusName} - {contract.LicensePlate}";
                    sheet.Cell(index, "I").Value = contract.PaymentType.AsString(EnumFormat.Description);
                    sheet.Cell(index, "J").Value = contract.TotalPrice.ToString("C2");
                    sheet.Cell(index, "K").Value = $"{contract.CustomersCount}";

                    index++;
                }

                using var stream = new MemoryStream();
                sheetBook.SaveAs(stream);

                var fileResponse = new FileResponse
                {
                    Content = stream.ToArray(),
                    Name = string.Format("{0}.xlsx", Guid.NewGuid()),
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                };

                return ExportResponse.Ok(fileResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError("falha inesperada ao tentar gerar excel, detalhes do erro : {erro}", ex);
                return ExportResponse.Failed("falha inesperada ao processar a exportação, consulte o time de suporte para mais detalhes.");
            }
        }
    }
}
