using FluentValidation;

namespace FrutNatura.App.Application.UseCases.Usuarios.AtualizarUsuario;

public sealed class AtualizarUsuarioValidator : AbstractValidator<AtualizarUsuarioCommand>
{
    public AtualizarUsuarioValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome)
            .NotEmpty().MaximumLength(120);

        RuleForEach(x => x.Roles ?? Enumerable.Empty<string>())
            .MaximumLength(64);
    }
}
