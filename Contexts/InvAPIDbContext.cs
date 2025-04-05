using InvestmentManagementService.Entities.AppUser;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvestmentManagementService.Contexts
{
    public class InvAPIDbContext : IdentityDbContext<AppUser,AppRole,string>
    {
        public InvAPIDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUser>(entity =>
            {
                entity.ToTable("Users");
            });
            builder.Entity<AppRole>(entity =>
            {
                entity.ToTable("Roles");
            });
        }
    }
    
}

