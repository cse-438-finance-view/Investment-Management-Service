using InvestmentManagementService.Entities.Common;
using InvestmentManagementService.Entities.AppUser.Events;
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
        private readonly IConfiguration _configuration;

        public RabbitMQPublisher(IConfiguration configuration, ILogger<RabbitMQPublisher> logger)
        {
            _logger = logger;
            _configuration = configuration;

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

                // Saga servisi tarafından kullanılan exchange'i oluştur
                _channel.ExchangeDeclare("user.events", ExchangeType.Topic, durable: true);

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
            var exchange = "user.events"; // Saga servisinin beklediği exchange adı
            string routingKey;

            // Event tipine göre routing key belirle
            if (domainEvent is UserCreatedEvent)
            {
                routingKey = "user.created";
            }
            else if (domainEvent is UserCreationFailedEvent)
            {
                routingKey = "user.creation.failed";
            }
            else
            {
                // Diğer event tipleri için default olarak event tipini kullan
                routingKey = eventType.ToLower();
            }

            var jsonOptions = new JsonSerializerOptions 
            { 
                PropertyNamingPolicy = null,  
                WriteIndented = true,         
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never, 
                IncludeFields = true         
            };
            
            var message = JsonSerializer.Serialize(domainEvent, domainEvent.GetType(), jsonOptions);
            
            _logger.LogInformation("Publishing event {EventType} with routing key {RoutingKey} and content: {Content}", 
                eventType, routingKey, message);
            
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

            _logger.LogInformation("Domain event {EventType} published to RabbitMQ with routing key {RoutingKey}", eventType, routingKey);

            return Task.CompletedTask;
        }
    }
} 