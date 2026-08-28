using FluentValidation;
using FrutNatura.Core.Domain.Enums;

namespace FrutNatura.App.Application.UseCases.Chamados.AlterarStatus;

public sealed class AlterarStatusValidator : AbstractValidator<AlterarStatusCommand>
{
    public AlterarStatusValidator()
    {
        RuleFor(x => x.ChamadoId)
            .NotEmpty();


        RuleFor(x => x.NovoStatus)
            .IsInEnum()
            .WithMessage("Status inválido.");

    }
}
