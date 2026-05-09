using BusesControl.Export.Core.Interfaces;
using BusesControl.Export.Infrastructure.Factory;
using Microsoft.Extensions.DependencyInjection;

namespace BusesControl.Export.Ioc
{
    public static class InfrastructureDependencyInjection
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
        }
    }
}
