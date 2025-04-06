using InvestmentManagementService.Entities.Common;
using InvestmentManagementService.Infrastructure.MessageBroker;

namespace InvestmentManagementService.Infrastructure.Services
{
    public class DomainEventDispatcher
    {
        private readonly IDomainEventPublisher _publisher;
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(IDomainEventPublisher publisher, ILogger<DomainEventDispatcher> logger)
        {
            _publisher = publisher;
            _logger = logger;
        }

        public async Task DispatchEventsAsync(IAggregateRoot aggregate)
        {
            var domainEvents = aggregate.DomainEvents.ToList();
            
            aggregate.ClearDomainEvents();

            foreach (var domainEvent in domainEvents)
            {
                _logger.LogInformation("Dispatching domain event: {EventName}", domainEvent.GetType().Name);
                
                try
                {
                    await _publisher.PublishAsync(domainEvent);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error publishing domain event: {EventName}", domainEvent.GetType().Name);
                    throw;
                }
            }
        }
    }
} 