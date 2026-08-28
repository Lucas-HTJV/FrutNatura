using FluentValidation;
using FrutNatura.App.Application.UseCases.Mensagens.EnviarMensagem;

public sealed class EnviarMensagemValidator : AbstractValidator<EnviarMensagemCommand>
{
    public EnviarMensagemValidator()
    {
        RuleFor(x => x.ChamadoId).NotEmpty();
        RuleFor(x => x.AutorId).NotEmpty(); // se for opcional, remova ou adapte
        RuleFor(x => x.Conteudo).NotEmpty().MaximumLength(4000);
    }
}
