using InvestmentManagementService.Entities.Common;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace InvestmentManagementService.Infrastructure.MessageBroker.RabbitMQ
{
    public class RabbitMQPublisher : IDomainEventPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMQPublisher> _logger;

        public RabbitMQPublisher(IConfiguration configuration, ILogger<RabbitMQPublisher> logger)
        {
            _logger = logger;

            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = configuration["RabbitMQ:Host"] ?? "localhost",
                    Port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672"),
                    UserName = configuration["RabbitMQ:Username"] ?? "guest",
                    Password = configuration["RabbitMQ:Password"] ?? "guest"
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _logger.LogInformation("RabbitMQ connection established");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not connect to RabbitMQ");
                throw;
            }
        }

        public Task PublishAsync<T>(T domainEvent) where T : IDomainEvent
        {
            var eventType = domainEvent.GetType().Name;
            var exchange = "investment_exchange";
            var routingKey = eventType;

            _channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);

            var jsonOptions = new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = null,  // Property adları olduğu gibi korunur (CamelCase yapılmaz)
                WriteIndented = true,         // Daha okunabilir JSON
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never, // Null değerleri dahil et
                IncludeFields = true          // Private field'lar dahil edilir
            };
            
            var message = JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), jsonOptions);
            
            _logger.LogInformation("Publishing event {EventType} with content: {Content}", eventType, message);
            
            var properties = domainEvent.GetType().GetProperties();
            foreach (var prop in properties) 
            {
                var value = prop.GetValue(domainEvent);
                _logger.LogDebug("Property: {PropertyName} = {PropertyValue}", prop.Name, value);
            }
            
            var body = Encoding.UTF8.GetBytes(message);

            var basicProperties = _channel.CreateBasicProperties();
            basicProperties.Persistent = true;
            basicProperties.MessageId = domainEvent.Id.ToString();
            basicProperties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            basicProperties.Headers = new Dictionary<string, object>
            {
                { "event_type", eventType }
            };

            _channel.BasicPublish(exchange, routingKey, basicProperties, body);

            _logger.LogInformation("Domain event {EventType} published to RabbitMQ", eventType);

            return Task.CompletedTask;
        }
    }
} 