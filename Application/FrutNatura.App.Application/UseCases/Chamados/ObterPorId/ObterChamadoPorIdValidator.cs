using FluentValidation;

namespace FrutNatura.App.Application.UseCases.Chamados.ObterPorId;

public sealed class ObterChamadoPorIdValidator : AbstractValidator<ObterChamadoPorIdQuery>
{
    public ObterChamadoPorIdValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
