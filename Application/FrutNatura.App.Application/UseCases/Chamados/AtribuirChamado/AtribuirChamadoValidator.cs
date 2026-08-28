using FluentValidation;

namespace FrutNatura.App.Application.UseCases.Chamados.AtribuirChamado;

public sealed class AtribuirChamadoValidator : AbstractValidator<AtribuirChamadoCommand>
{
    public AtribuirChamadoValidator()
    {
        RuleFor(x => x.ChamadoId)
            .NotEmpty().WithMessage("'Chamado Id' deve ser informado.");

        RuleFor(x => x.ResponsavelId)
            .NotEmpty().WithMessage("'Responsável Id' deve ser informado.");
    }
}
