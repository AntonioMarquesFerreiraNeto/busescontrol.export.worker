using BusesControl.Export.Domain;
using BusesControl.Export.Ioc;
using BusesControl.Export.Worker.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<Settings>(builder.Configuration);
builder.Services.AddHostedService<ExportWorker>();
builder.Services.RegisterServices().RegisterInfrastructures();

var host = builder.Build();
host.Run();
