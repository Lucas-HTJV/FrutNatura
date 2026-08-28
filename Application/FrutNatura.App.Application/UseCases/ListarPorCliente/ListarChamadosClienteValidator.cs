using FluentValidation;

namespace FrutNatura.App.Application.UseCases.ListarPorCliente;

public sealed class ListarChamadosClienteValidator : AbstractValidator<ListarChamadosClienteQuery>
{
    public ListarChamadosClienteValidator()
    {
        RuleFor(x => x.ClienteId).NotEmpty();
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(200);
    }
}
