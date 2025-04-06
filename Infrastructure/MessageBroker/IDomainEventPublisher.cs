using InvestmentManagementService.Entities.Common;

namespace InvestmentManagementService.Infrastructure.MessageBroker
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync<T>(T domainEvent) where T : IDomainEvent;
    }
} 