using BusesControl.Export.Domain;
using BusesControl.Export.Ioc;
using BusesControl.Export.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<Settings>(builder.Configuration);

builder.Services.AddHostedService<ExportWorker>();
CoreDependencyInjection.Register(builder.Services);
InfrastructureDependencyInjection.Register(builder.Services);

var host = builder.Build();
host.Run();
