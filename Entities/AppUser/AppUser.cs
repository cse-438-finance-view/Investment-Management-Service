
using Microsoft.AspNetCore.Identity;

namespace InvestmentManagementService.Entities.AppUser
{
    public class AppUser : IdentityUser<string>
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateOnly? BornDate { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}
