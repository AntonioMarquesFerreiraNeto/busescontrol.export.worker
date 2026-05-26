using BusesControl.Export.Core.Interfaces;
using BusesControl.Export.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BusesControl.Export.Ioc
{
    public static class CoreDependencyInjection
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<IExportService, ExportService>();
            services.AddTransient<IContractExportService, ContractExportService>();
            services.AddTransient<IFinancialExportService, FinancialExportService>();
            services.AddTransient<ICustomerExportService, CustomerExportService>();
        }
    }
}
