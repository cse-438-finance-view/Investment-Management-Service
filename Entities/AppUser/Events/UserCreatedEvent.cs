using InvestmentManagementService.Entities.Common;
using System.Text.Json.Serialization;

namespace InvestmentManagementService.Entities.AppUser.Events
{
    public class UserCreatedEvent : DomainEvent
    {
        [JsonInclude]
        public string UserId { get; private set; }

        [JsonInclude]
        public string Email { get; private set; }

        [JsonInclude]
        public string? Name { get; private set; }

        [JsonInclude]
        public string? Surname { get; private set; }

        // Serialization için paramtresiz constructor gerekebilir
        [JsonConstructor]
        public UserCreatedEvent()
        {
            UserId = string.Empty;
            Email = string.Empty;
        }

        public UserCreatedEvent(string userId, string email, string? name, string? surname)
        {
            UserId = userId;
            Email = email;
            Name = name;
            Surname = surname;
        }
    }
} 