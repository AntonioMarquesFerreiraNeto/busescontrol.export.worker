using BusesControl.Export.Core.Entities;
using BusesControl.Export.Core.Interfaces;
using Dapper;

namespace BusesControl.Export.Infrastructure.Repositories
{
    public class ExportRepository : IExportRepository
    {
        public ExportRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private readonly IDbConnectionFactory _connectionFactory;

        public async Task Update(ExportModel export)
        {
            using var connection = _connectionFactory.CreateConnection();
            connection.Open();

            var execute = @$"
                UPDATE dbo.Exports SET 
                    {nameof(ExportModel.Url)} = @{nameof(ExportModel.Url)}, 
                    {nameof(ExportModel.ExportedAt)} = @{nameof(ExportModel.ExportedAt)}, 
                    {nameof(ExportModel.ExpiresAt)} = @{nameof(ExportModel.ExpiresAt)}, 
                    {nameof(ExportModel.Status)} = @{nameof(ExportModel.Status)},
                    {nameof(ExportModel.ErrorMessage)} = @{nameof(ExportModel.ErrorMessage)} 
                WHERE 
                    {nameof(ExportModel.Id)} = @{nameof(ExportModel.Id)};
            ";

            await connection.ExecuteAsync(execute, export);
        }
    }
}
