using FrutNatura.Core.Contracts.Chamados;

namespace FrutNatura.App.Application.UseCases.Chamados.ObterPorId;
public sealed record ObterChamadoPorIdQuery(Guid Id) : IRequest<ChamadoDto?>;