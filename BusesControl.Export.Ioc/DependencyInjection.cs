using BusesControl.Export.Core.Interfaces;
using BusesControl.Export.Core.Services;
using BusesControl.Export.Infrastructure.Factory;
using BusesControl.Export.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BusesControl.Export.Ioc
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IExportService, ExportService>();
            services.AddTransient<IContractExportService, ContractExportService>();
            services.AddTransient<IFinancialExportService, FinancialExportService>();
            services.AddTransient<ICustomerExportService, CustomerExportService>();
        }

        public static void RegisterInfrastructures(this IServiceCollection services)
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
