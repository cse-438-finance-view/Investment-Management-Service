using FluentValidation;

namespace InvestmentManagementService.Features.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommandRequest>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email address cannot be empty.")
                .EmailAddress().WithMessage("Please enter a valid email address.")
                .MaximumLength(100).WithMessage("Email address can be at most 100 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.Name)
                .MinimumLength(2).When(x => !string.IsNullOrEmpty(x.Name)).WithMessage("Name must be at least 2 characters.")
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Name)).WithMessage("Name can be at most 50 characters.");

            RuleFor(x => x.Surname)
                .MinimumLength(2).When(x => !string.IsNullOrEmpty(x.Surname)).WithMessage("Surname must be at least 2 characters.")
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.Surname)).WithMessage("Surname can be at most 50 characters.");

            RuleFor(x => x.username)
                .MinimumLength(3).When(x => !string.IsNullOrEmpty(x.username)).WithMessage("Username must be at least 3 characters.")
                .MaximumLength(30).When(x => !string.IsNullOrEmpty(x.username)).WithMessage("Username can be at most 30 characters.")
                .Matches("^[a-zA-Z0-9._-]+$").When(x => !string.IsNullOrEmpty(x.username)).WithMessage("Username can only contain letters, numbers, and ._- characters.");
        }
    }
} 