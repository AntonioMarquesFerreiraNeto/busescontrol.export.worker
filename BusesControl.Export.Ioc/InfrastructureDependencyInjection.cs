using BusesControl.Export.Core.Interfaces;
using BusesControl.Export.Infrastructure.Factory;
using BusesControl.Export.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BusesControl.Export.Ioc
{
    public static class InfrastructureDependencyInjection
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();
            services.AddTransient<IExportRepository, ExportRepository>();
            services.AddTransient<IContractRepository, ContractRepository>();
            services.AddTransient<IFinancialRepository, FinancialRepository>();
            services.AddTransient<IStorageRepository, StorageRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
        }
    }
}
