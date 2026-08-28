using FrutNatura.App.Application.Common;
using FrutNatura.Core.Domain.Enums;
using MediatR;

namespace FrutNatura.App.Application.UseCases.Chamados.AlterarStatus;

public sealed record AlterarStatusCommand(Guid ChamadoId, StatusChamado NovoStatus) : IRequest<bool>;

