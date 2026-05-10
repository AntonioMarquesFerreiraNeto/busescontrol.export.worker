using BusesControl.Export.Core.Entities;
using BusesControl.Export.Core.Interfaces;
using Dapper;

namespace BusesControl.Export.Infrastructure.Repositories
{
    public class FinancialRepository : IFinancialRepository
    {
        public FinancialRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private readonly IDbConnectionFactory _connectionFactory;

        public async Task<IEnumerable<FinancialModel>> GetAll()
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = @"
                SELECT 
                    financial.Id, financial.Reference, financial.[Type], 
                    customer.Name AS customerName, supplier.Name AS supplierName,
                    financial.TotalPrice, financial.PaymentType, financial.TerminateDate, 
                    financial.Active,
                    CASE 
                        WHEN financial.[Type] = 1
                        THEN ISNULL((
                            SELECT SUM(inv.TotalPrice) 
                            FROM dbo.Invoices inv 
                            WHERE inv.FinancialId = financial.Id 
                              AND inv.Status = 2
                        ), 0)
                        ELSE ISNULL((
                            SELECT SUM(exp.TotalPrice) 
                            FROM dbo.InvoicesExpense exp 
                            WHERE exp.FinancialId = financial.Id 
                              AND exp.Status = 2
                        ), 0)
                    END AS TotalPaid
                FROM dbo.Financials financial
                    LEFT JOIN dbo.Customers customer ON financial.CustomerId = customer.Id
                    LEFT JOIN dbo.Suppliers supplier ON financial.SupplierId = supplier.Id;
            ";

            return await connection.QueryAsync<FinancialModel>(query);
        }
    }
}
