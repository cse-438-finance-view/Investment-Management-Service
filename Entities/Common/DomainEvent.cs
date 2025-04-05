using System.Text.Json.Serialization;

namespace InvestmentManagementService.Entities.Common
{
    public abstract class DomainEvent : IDomainEvent
    {
        [JsonInclude]
        public Guid Id { get; private set; }

        [JsonInclude]
        public DateTime OccurredOn { get; private set; }

        protected DomainEvent()
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
        }
    }
} 