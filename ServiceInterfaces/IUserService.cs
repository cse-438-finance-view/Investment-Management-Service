using InvestmentManagementService.Features.Commands.CreateUser;

namespace InvestmentManagementService.ServiceInterfaces
{
    public interface IUserService
    {
        Task<CreateUserCommandResponse> CreateUserAsync(CreateUserCommandRequest request);
    }
}
