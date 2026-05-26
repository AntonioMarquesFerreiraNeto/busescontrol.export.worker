using BusesControl.Export.Core.Entities;
using BusesControl.Export.Core.Interfaces;
using Dapper;

namespace BusesControl.Export.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public CustomerRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        private readonly IDbConnectionFactory _connectionFactory;

        public async Task<IEnumerable<CustomerModel>> GetAll()
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = @"
                SELECT 
                    customer.Name, customer.PhoneNumber, customer.Email, 
                    customer.Cpf, customer.Cnpj, customer.Type, 
                    customer.City, customer.State, customer.Active
                FROM dbo.Customers customer
            ";

            return await connection.QueryAsync<CustomerModel>(query);
        }
    }
}
