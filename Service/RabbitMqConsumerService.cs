// Services/RabbitMqConsumerService.cs
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Sessions_app.Services
{
    public class RabbitMqConsumerService : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RabbitMqConsumerService> _logger;
        private const string QueueName = "riscos_cadastrados";

        public RabbitMqConsumerService(
            IServiceProvider serviceProvider,
            ILogger<RabbitMqConsumerService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Garante que a fila existe
            _channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation($"Mensagem recebida: {message}");

                    using var scope = _serviceProvider.CreateScope();
                    await ProcessMessage(message, scope.ServiceProvider);

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar mensagem");
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume(
                queue: QueueName,
                autoAck: false,
                consumer: consumer
            );

            return Task.CompletedTask;
        }

        private async Task ProcessMessage(string message, IServiceProvider serviceProvider)
        {
            var riscoEvent = JsonSerializer.Deserialize<RiscoEvent>(message);

            if (riscoEvent?.EventType == "RISCO_CREATED")
            {
                var emailService = serviceProvider.GetRequiredService<EmailService>();
                await emailService.EnviarEmailNotificacaoRisco(riscoEvent);
            }
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }

    public class RiscoEvent
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int Nivel { get; set; }
        public string EventType { get; set; }
    }
}