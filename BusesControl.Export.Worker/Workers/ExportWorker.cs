using BusesControl.Export.Core.Interfaces;
using BusesControl.Export.Domain;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace BusesControl.Export.Worker.Workers
{
    public class ExportWorker : BackgroundService
    {
        public ExportWorker(ILogger<ExportWorker> logger, IOptions<Settings> options, IExportService exportService)
        {
            _logger = logger;
            _settings = options.Value;
            _exportService = exportService;
        }

        private readonly ILogger<ExportWorker> _logger;
        private readonly Settings _settings;
        private readonly IExportService _exportService;
        private IConnection _connection;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("iniciando execução do worker, nome : {name}", nameof(ExportWorker));

            await CreateConnection();

            using var channel = await _connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: _settings.ExportQueue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken
            );

            var consumer = new AsyncEventingBasicConsumer(channel);
            
            consumer.ReceivedAsync += async (model, ea) => 
            {
                try
                {
                    _logger.LogInformation("recebendo mensagem da fila, delivery tag : {delivery}", ea.DeliveryTag);

                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    await _exportService.Execute(message);
                    await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);

                    _logger.LogInformation("mensagem processada com sucesso, delivery tag : {delivery}", ea.DeliveryTag);
                }
                catch (Exception ex)
                {
                    await channel.BasicNackAsync(ea.DeliveryTag, false, false, stoppingToken);
                    _logger.LogError("falha inesperada ao executar exportação, detalhes do erro : {erro}", ex);
                }
            };

            await channel.BasicConsumeAsync(
                queue: _settings.ExportQueue,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken
            );

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task CreateConnection()
        {
            if (_connection is null)
            {
                _logger.LogInformation("criando contexto de conexão com o node, host name : {host}", _settings.RabbitMq.HostName);

                var factory = new ConnectionFactory
                {
                    HostName = _settings.RabbitMq.HostName,
                    UserName = _settings.RabbitMq.UserName,
                    Password = _settings.RabbitMq.Password,
                };

                _connection = await factory.CreateConnectionAsync();
            }
        }
    }
}
