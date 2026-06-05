using BusesControl.Export.Core.Interfaces;
using BusesControl.Export.Core.Services;
using BusesControl.Export.Infrastructure.Factory;
using BusesControl.Export.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BusesControl.Export.Ioc
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IExportService, ExportService>();
            services.AddTransient<IExportProcessorService, ContractExportService>();
            services.AddTransient<IExportProcessorService, FinancialExportService>();
            services.AddTransient<IExportProcessorService, CustomerExportService>();

            return services;
        }

        public static IServiceCollection RegisterInfrastructures(this IServiceCollection services)
        {
            services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();
            services.AddTransient<IExportRepository, ExportRepository>();
            services.AddTransient<IContractRepository, ContractRepository>();
            services.AddTransient<IFinancialRepository, FinancialRepository>();
            services.AddTransient<IStorageRepository, StorageRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();

            return services;
        }
    }
}
