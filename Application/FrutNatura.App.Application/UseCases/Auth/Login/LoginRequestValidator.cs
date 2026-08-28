
using FluentValidation;
using FrutNatura.Core.Contracts.Auth;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.HashPassword).NotEmpty().MinimumLength(6);
    }
}
