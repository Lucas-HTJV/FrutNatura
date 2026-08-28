using FluentValidation;

namespace FrutNatura.App.Application.UseCases.Mensagens.ListarMensagens
{
    public sealed class ListarMensagensValidator : AbstractValidator<ListarMensagensQuery>
    {
        public ListarMensagensValidator()
        {
            RuleFor(x => x.ChamadoId)
                .NotEmpty()
                .WithMessage("ChamadoId é obrigatório.");
        }
    }
}
