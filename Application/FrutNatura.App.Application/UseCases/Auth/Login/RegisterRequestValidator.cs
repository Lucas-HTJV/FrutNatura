
using FluentValidation;
using FrutNatura.Core.Contracts.Auth;

public sealed class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.HashPassword).NotEmpty().MinimumLength(6);
    }
}
