namespace InvestmentManagementService.Entities.Common
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        DateTime OccurredOn { get; }
    }
} 