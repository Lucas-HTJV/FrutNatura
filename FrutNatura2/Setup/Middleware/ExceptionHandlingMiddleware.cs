using System.Net;
using System.Text.Json;
using FrutNatura.Core.Contracts.Common;

namespace FrutNatura.WebApi.Setup.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly JsonSerializerOptions JsonOpts = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (Exception ex)
        {
            await HandleAsync(ctx, ex);
        }
    }

    private static Task HandleAsync(HttpContext ctx, Exception ex)
    {
        var status = ex switch
        {
            ArgumentException => HttpStatusCode.BadRequest,
            InvalidOperationException => HttpStatusCode.BadRequest,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };

        var problem = new ProblemDetailsExt(
            Title: "Erro ao processar a requisição",
            Detail: ex.Message,
            Status: (int)status,
            Instance: ctx.Request.Path);

        ctx.Response.ContentType = "application/problem+json";
        ctx.Response.StatusCode = (int)status;
        return ctx.Response.WriteAsync(JsonSerializer.Serialize(problem, JsonOpts));
    }
}
