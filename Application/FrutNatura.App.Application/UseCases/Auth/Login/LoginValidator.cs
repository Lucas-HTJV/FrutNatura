using FluentValidation;
using FrutNatura.App.Application.UseCases.Auth.Login;

namespace FrutNatura.App.Application.Validators.Auth;

public sealed class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("E-mail é obrigatório.")
            .EmailAddress().WithMessage("E-mail inválido.");

    }
}
