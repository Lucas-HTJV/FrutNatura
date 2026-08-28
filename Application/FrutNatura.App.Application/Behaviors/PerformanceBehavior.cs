using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FrutNatura.App.Application.Behaviors;

public sealed class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private readonly int _thresholdMs;
    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger, int thresholdMs = 500)
    { _logger = logger; _thresholdMs = thresholdMs; }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        var resp = await next();
        sw.Stop();
        if (sw.ElapsedMilliseconds > _thresholdMs)
            _logger.LogWarning("Slow {RequestType} took {Elapsed}ms. Payload: {@Request}", typeof(TRequest).Name, sw.ElapsedMilliseconds, request);
        return resp;
    }
}
