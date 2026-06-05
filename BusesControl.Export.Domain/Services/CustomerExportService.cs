using BusesControl.Export.Core.Entities;
using BusesControl.Export.Core.Enums;
using BusesControl.Export.Core.Interfaces;
using BusesControl.Export.Core.Responses;
using ClosedXML.Excel;
using EnumsNET;
using Microsoft.Extensions.Logging;

namespace BusesControl.Export.Core.Services
{
    public class CustomerExportService : IExportProcessorService
    {
        public CustomerExportService(ICustomerRepository customerRepository, ILogger<ContractExportService> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger _logger;

        public ExportTypeEnum Type => ExportTypeEnum.Customers;

        public async Task<ExportResponse> Execute(ExportModel export)
        {
            try
            {
                var customers = await _customerRepository.GetAll();
                if (!customers.Any())
                {
                    return ExportResponse.Failed("Nenhum contrato foi encontrado.");
                }

                var sheetBook = new XLWorkbook();
                var sheet = sheetBook.Worksheets.Add("Sample sheet");

                sheet.Cell(1, "A").Value = "Nome";
                sheet.Cell(1, "B").Value = "Celular";
                sheet.Cell(1, "C").Value = "E-mail";
                sheet.Cell(1, "D").Value = "CPF/CNPJ";
                sheet.Cell(1, "E").Value = "Tipo";
                sheet.Cell(1, "F").Value = "Cidade";
                sheet.Cell(1, "G").Value = "Estado";
                sheet.Cell(1, "H").Value = "Ativo";

                var columns = new[] { "A", "B", "C", "D", "E", "F", "G", "H" };

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

                var colName = sheet.Column("A");
                var colEmail = sheet.Column("C");
                colName.Width = 35;
                colEmail.Width = 35;

                var index = 2;

                foreach (var customer in customers)
                {
                    sheet.Cell(index, "A").Value = customer.Name;
                    sheet.Cell(index, "B").Value = customer.PhoneNumber;
                    sheet.Cell(index, "C").Value = customer.Email;
                    sheet.Cell(index, "D").Value = customer.Document;
                    sheet.Cell(index, "E").Value = customer.Type.AsString(EnumFormat.Description);
                    sheet.Cell(index, "F").Value = customer.City;
                    sheet.Cell(index, "G").Value = customer.State;
                    sheet.Cell(index, "H").Value = customer.Active ? "Sim" : "Não";

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
