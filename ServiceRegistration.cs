using FluentValidation;
using FluentValidation.AspNetCore;
using InvestmentManagementService.Contexts;
using InvestmentManagementService.Entities.AppUser;
using InvestmentManagementService.Features.Commands.CreateUser;
using InvestmentManagementService.Infrastructure.MessageBroker;
using InvestmentManagementService.Infrastructure.MessageBroker.RabbitMQ;
using InvestmentManagementService.Infrastructure.Services;
using InvestmentManagementService.ServiceInterfaces;
using InvestmentManagementService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InvestmentManagementService
{
    public static class ServiceRegistration
    {
        public static void AddServices(this IServiceCollection services, IConfiguration _configuration) 
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

            services.AddDbContext<InvAPIDbContext>(options =>
            {
                options.UseNpgsql(_configuration["ConnectionStrings:DefaultConnection"], npgsqlOptions =>
                {
                    npgsqlOptions.CommandTimeout(300);
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                });
                options.EnableSensitiveDataLogging();
            });

            services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<InvAPIDbContext>().AddDefaultTokenProviders();

            services.AddSingleton<IDomainEventPublisher, RabbitMQPublisher>();
            services.AddScoped<DomainEventDispatcher>();

            services.AddScoped<IUserService, UserService>();
            
            // FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            // Spesifik validator'ü kaydet
            services.AddScoped<IValidator<CreateUserCommandRequest>, CreateUserCommandValidator>();
        }
    }
}
