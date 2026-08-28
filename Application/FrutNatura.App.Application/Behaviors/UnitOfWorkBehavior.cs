using FrutNatura.App.Application.Common;
using FrutNatura.Core.Abstractions.Common;
using MediatR;

namespace FrutNatura.App.Application.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _uow;
    public UnitOfWorkBehavior(IUnitOfWork uow) => _uow = uow;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var resp = await next();
        if (request is ITransactionalRequest) await _uow.SaveChangesAsync(ct);
        return resp;
    }
}
