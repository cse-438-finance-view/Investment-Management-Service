using InvestmentManagementService.Entities.AppUser;
using InvestmentManagementService.Entities.AppUser.Events;
using InvestmentManagementService.Features.Commands.CreateUser;
using InvestmentManagementService.Infrastructure.Services;
using InvestmentManagementService.ServiceInterfaces;
using Microsoft.AspNetCore.Identity;

namespace InvestmentManagementService.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DomainEventDispatcher _domainEventDispatcher;
        private readonly ILogger<UserService> _logger;

        public UserService(
            UserManager<AppUser> userManager,
            DomainEventDispatcher domainEventDispatcher,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _domainEventDispatcher = domainEventDispatcher;
            _logger = logger;
        }

        public async Task<CreateUserCommandResponse> CreateUserAsync(CreateUserCommandRequest request)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingUserByEmail != null)
            {
                _logger.LogWarning("User creation failed: Email {Email} is already in use", request.Email);
                
                var failedEvent = new UserCreationFailedEvent(
                    request.Email,
                    "Email is already in use",
                    DateTime.UtcNow
                );
                
                var tempUser = new AppUser();
                tempUser.AddDomainEvent(failedEvent);
                await _domainEventDispatcher.DispatchEventsAsync(tempUser);
                
                return new CreateUserCommandResponse
                {
                    Succeeded = false,
                    Message = "Email is already in use."
                };
            }

            if (!string.IsNullOrEmpty(request.username))
            {
                var existingUserByUsername = await _userManager.FindByNameAsync(request.username);
                if (existingUserByUsername != null)
                {
                    _logger.LogWarning("User creation failed: Username {Username} is already in use", request.username);
                    
                    var failedEvent = new UserCreationFailedEvent(
                        request.Email,
                        "Username is already in use",
                        DateTime.UtcNow
                    );
                    
                    var tempUser = new AppUser();
                    tempUser.AddDomainEvent(failedEvent);
                    await _domainEventDispatcher.DispatchEventsAsync(tempUser);
                    
                    return new CreateUserCommandResponse
                    {
                        Succeeded = false,
                        Message = "Username is already in use."
                    };
                }
            }

            var newUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = !string.IsNullOrEmpty(request.username) ? request.username : request.Email,
                Name = request.Name,
                Surname = request.Surname,
                BornDate = request.BornDate,
                CreateDate = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created successfully with ID: {UserId}", newUser.Id);

                if (string.IsNullOrEmpty(newUser.Email))
                {
                    _logger.LogWarning("Cannot create UserCreatedEvent: Email is null or empty for user {UserId}", newUser.Id);
                    
                    var failedEvent = new UserCreationFailedEvent(
                        "unknown",
                        "Email is null or empty",
                        DateTime.UtcNow
                    );
                    
                    newUser.AddDomainEvent(failedEvent);
                    await _domainEventDispatcher.DispatchEventsAsync(newUser);
                    
                    return new CreateUserCommandResponse
                    {
                        Succeeded = true,
                        UserId = newUser.Id,
                        Message = "User created successfully but email notification could not be sent."
                    };
                }

                var userCreatedEvent = new UserCreatedEvent(
                    newUser.Id,
                    newUser.Email,
                    newUser.Name,
                    newUser.Surname
                );

                newUser.AddDomainEvent(userCreatedEvent);

                await _domainEventDispatcher.DispatchEventsAsync(newUser);

                return new CreateUserCommandResponse
                {
                    Succeeded = true,
                    UserId = newUser.Id,
                    Message = "User created successfully"
                };
            }

            _logger.LogWarning("Failed to create user with email: {Email}, Errors: {Errors}", 
                request.Email, 
                string.Join(", ", result.Errors.Select(e => e.Description)));
            
            var creationFailedEvent = new UserCreationFailedEvent(
                request.Email,
                string.Join(", ", result.Errors.Select(e => e.Description)),
                DateTime.UtcNow
            );
            
            var failedUser = new AppUser();
            failedUser.AddDomainEvent(creationFailedEvent);
            await _domainEventDispatcher.DispatchEventsAsync(failedUser);
            
            return new CreateUserCommandResponse
            {
                Succeeded = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }
    }
}
