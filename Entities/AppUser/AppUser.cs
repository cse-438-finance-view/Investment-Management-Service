using InvestmentManagementService.Entities.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvestmentManagementService.Entities.AppUser
{
    public class AppUser : IdentityUser<string>, IAggregateRoot
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateOnly? BornDate { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
        public DateTime? CreateDate { get; set; }

        private readonly List<IDomainEvent> _domainEvents = new();
        
        [NotMapped]
        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
