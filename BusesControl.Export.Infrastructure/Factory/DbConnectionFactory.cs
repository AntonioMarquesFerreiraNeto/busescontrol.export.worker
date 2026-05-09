using BusesControl.Export.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BusesControl.Export.Infrastructure.Factory
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BusesControl");
        }

        private readonly string _connectionString;

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
