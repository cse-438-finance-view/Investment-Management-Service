using InvestmentManagementService.Entities.Common;

namespace InvestmentManagementService.Infrastructure.MessageBroker
{
    public interface IDomainEventHandler<T> where T : IDomainEvent
    {
        Task HandleAsync(T domainEvent);
    }
} 