using FluentValidation;

namespace FrutNatura.App.Application.UseCases.Usuarios.CriarUsuario;

public sealed class CriarUsuarioValidator : AbstractValidator<CriarUsuarioCommand>
{
    public CriarUsuarioValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().MaximumLength(120);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress().WithMessage("E-mail inválido.");

        RuleForEach(x => x.Roles ?? Enumerable.Empty<string>())
            .MaximumLength(64);
    }
}
