using MediatR;

namespace InvestmentManagementService.Features.Commands.CreateUser
{
    public class CreateUserCommandRequest : IRequest<CreateUserCommandResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }

        public DateOnly? BornDate { get; set; }

        public string? username { get; set; }
    }
}
