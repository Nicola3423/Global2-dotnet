// Services/RabbitMqService.cs
using RabbitMQ.Client;
using Sessions_app.Models;
using System.Text;
using System.Text.Json;

namespace Sessions_app.Services
{
    public class RabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string ExchangeName = "riscos_events";
        private const string QueueName = "riscos_cadastrados";

        public RabbitMqService()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true // Importante para async
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Garante a criação da exchange e fila
            _channel.ExchangeDeclare(
                exchange: ExchangeName,
                type: ExchangeType.Fanout,
                durable: true,
                autoDelete: false
            );

            _channel.QueueDeclare(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _channel.QueueBind(
                queue: QueueName,
                exchange: ExchangeName,
                routingKey: ""
            );
        }

        public void PublishRiscoCreated(Risco risco)
        {
            var message = JsonSerializer.Serialize(new
            {
                Id = risco.Id,
                Descricao = risco.Descricao,
                Nivel = risco.Nivel,
                EventType = "RISCO_CREATED"
            });

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(
                exchange: ExchangeName,
                routingKey: "",
                basicProperties: null,
                body: body
            );
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}