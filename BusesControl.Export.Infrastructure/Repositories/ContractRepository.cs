using BusesControl.Export.Core.Entities;
using BusesControl.Export.Core.Interfaces;
using Dapper;

namespace BusesControl.Export.Infrastructure.Repositories
{
    public class ContractRepository : IContractRepository
    {
        public ContractRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private readonly IDbConnectionFactory _connectionFactory;

        public async Task<IEnumerable<ContractModel>> GetAll()
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = @"
                select 
                    contract.Id, contract.ApproverId, contract.BusId, contract.DriverId, contract.Reference, contract.Status, contract.IsApproved, 
                    approver.Name as approverName, contract.StartDate, contract.TerminateDate, 
                    driver.Name as driverName, bus.Name as busName, bus.LicensePlate as licensePlate,
                    contract.PaymentType, contract.TotalPrice, contract.CustomersCount 
                from dbo.contracts contract
                left join dbo.Employees approver on contract.ApproverId = approver.Id
                left join dbo.Employees driver on contract.DriverId = driver.Id
                left join dbo.Buses bus on contract.BusId = bus.Id;
            ";

            return await connection.QueryAsync<ContractModel>(query);
        }
    }
}
