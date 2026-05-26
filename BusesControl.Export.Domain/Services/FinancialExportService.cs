using BusesControl.Export.Core.Entities;
using BusesControl.Export.Core.Interfaces;
using BusesControl.Export.Core.Responses;
using ClosedXML.Excel;
using EnumsNET;
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

        public async Task<ExportResponse> Execute(ExportModel export)
        {
            try
            {
                var financialRecords = await _financialRepository.GetAll();
                if (!financialRecords.Any())
                {
                    return default!;
                }

                using var sheetBook = new XLWorkbook();
                var sheet = sheetBook.Worksheets.Add("Sample Sheet");

                sheet.Cell(1, "A").Value = "Referência";
                sheet.Cell(1, "B").Value = "Tipo";
                sheet.Cell(1, "C").Value = "Credor/Devedor";
                sheet.Cell(1, "D").Value = "Valor Total";
                sheet.Cell(1, "E").Value = "Valor Pago";
                sheet.Cell(1, "F").Value = "Tipo de pagamento";
                sheet.Cell(1, "G").Value = "Data de vencimento";
                sheet.Cell(1, "H").Value = "Status";

                var columns = new[] { "A", "B", "C", "D", "E", "F", "G", "H" };

                foreach (var column in columns)
                {
                    var col = sheet.Column(column);
                    col.Width = 20;
                    col.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    var title = sheet.Cell(1, column);
                    title.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                    title.Style.Font.SetBold();
                    title.Style.Font.FontColor = XLColor.DarkBlue;
                }

                var columnOne = sheet.Column("A");
                var columnTwo = sheet.Column("C");
                columnOne.Width = 25;
                columnTwo.Width = 40;

                var index = 2;

                foreach (var financial in financialRecords)
                {
                    var name = financial.CustomerName ?? financial.SupplierName;

                    sheet.Cell(index, "A").Value = financial.Reference;
                    sheet.Cell(index, "B").Value = financial.Type.AsString(EnumFormat.Description);
                    sheet.Cell(index, "C").Value = name;
                    sheet.Cell(index, "D").Value = financial.TotalPrice.ToString("C2");
                    sheet.Cell(index, "E").Value = financial.TotalPaid.ToString("C2");
                    sheet.Cell(index, "F").Value = financial.PaymentType.AsString(EnumFormat.Description);
                    sheet.Cell(index, "G").Value = financial.TerminateDate.ToString("dd/MM/yyyy");
                    sheet.Cell(index, "H").Value = financial.Active ? "Ativa" : "Inativa";

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
