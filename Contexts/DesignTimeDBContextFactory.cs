using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InvestmentManagementService.Contexts
{
    public class DesignTimeDBContextFactory : IDesignTimeDbContextFactory<InvAPIDbContext>
    {
        public InvAPIDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<InvAPIDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseNpgsql(connectionString);
            return new InvAPIDbContext(builder.Options);
        }
    }
}
