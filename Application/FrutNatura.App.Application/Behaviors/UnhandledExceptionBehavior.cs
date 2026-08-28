using MediatR;
using Microsoft.Extensions.Logging;

namespace FrutNatura.App.Application.Behaviors;

public sealed class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> _logger;
    public UnhandledExceptionBehavior(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        try { return await next(); }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in {RequestType}. Payload: {@Request}", typeof(TRequest).Name, request);
            throw;
        }
    }
}
