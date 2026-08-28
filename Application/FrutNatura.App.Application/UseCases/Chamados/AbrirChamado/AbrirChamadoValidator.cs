using FluentValidation;

namespace FrutNatura.App.Application.UseCases.Chamados.AbrirChamado;

public sealed class AbrirChamadoValidator : AbstractValidator<AbrirChamadoCommand>
{
    public AbrirChamadoValidator()
    {
        RuleFor(x => x.ClienteId).NotEmpty();
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(120);
        RuleFor(x => x.Descricao).NotEmpty().MaximumLength(4000);
        
    }
}
