namespace InvestmentManagementService.Features.Commands.CreateUser
{
    public class CreateUserCommandResponse
    {
        public bool Succeeded { get; set; }
        public string? UserId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
