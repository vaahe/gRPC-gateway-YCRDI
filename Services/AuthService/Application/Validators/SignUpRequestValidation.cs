using FluentValidation;

namespace AuthService.Validators
{
    public class SignUpRequestValidation : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidation()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .Length(0, 16).WithMessage("Username must be less than 16 characters");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long");
        }
    }
}
