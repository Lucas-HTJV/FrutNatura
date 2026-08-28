using FrutNatura.App.Application.DependencyInjection;
using FrutNatura.Core.Abstractions.Common;
using FrutNatura.Core.Abstractions.Security;
using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Infra;
using FrutNatura.Infra.Persistence;
using FrutNatura.Infra.Persistence.UniWork;
using FrutNatura.Infra.Repositories;
using FrutNatura.Infra.Security;
using FrutNatura.Infra.Security.Auth;
using FrutNatura.Infra.Security.Hashing;
using FrutNatura.Infra.Security.Jwt;
using FrutNatura.Infra.Security.Options;
using FrutNatura.WebApi.Setup;
using FrutNatura.WebApi.Setup.Middleware;
using Microsoft.AspNetCore.RateLimiting;

using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.FileProviders;
using Serilog;
using Serilog.Events;
using System.Text.Json.Serialization;

// IA
using FrutNatura.Infra.AI.Extensions;

// ================== BOOT & CONFIG ==================
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.WebHost.UseUrls("http://0.0.0.0:5000");


builder.Services.AddAi(config);

builder.Services.AddSwaggerGen(options =>
{
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());  
});

// Logs (Serilog)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

// MVC + JSON (enums como string)
builder.Services.AddControllersWithViews()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Kestrel (opcional)
builder.Services.Configure<KestrelServerOptions>(o =>
{
    o.Limits.MaxRequestBodySize = 10 * 1024 * 1024;
    o.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);
});

// CORS
const string CorsPolicy = "Default";
builder.Services.AddCors(opt =>
{
    var origins = config.GetSection("Cors:Origins").Get<string[]>() ?? Array.Empty<string>();
    opt.AddPolicy(CorsPolicy, p =>
        p.WithOrigins(
            "http://localhost:7000",     // front no IIS
            "https://localhost:7167"     // front no VS
        ) 
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials());
});

// Rate Limiter / Cache / Health
builder.Services.AddRateLimiter(o =>
{
    o.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromSeconds(1);
        opt.PermitLimit = 50;
        opt.QueueLimit = 0;
    });
});
builder.Services.AddResponseCompression();
builder.Services.AddOutputCache(o => o.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(30));
builder.Services.AddHealthChecks();

// Application + Infra
builder.Services.AddApplication();
builder.Services.AddInfrastructure(config);

// JWT Options (se usar Auth/JWT, registrar AddAuthentication no AddInfrastructure)
builder.Services.Configure<JwtOptions>(config.GetSection("Jwt"));

// DI (segurança/infra)
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
builder.Services.AddScoped<IRefreshTokensService, RefreshTokensRepository>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerConfig.Configure);




var app = builder.Build();

app.MapGet("/", () => Results.Redirect("/swagger"));

// ================== PIPELINE ==================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseResponseCompression();

app.UseRouting();

app.UseCors(CorsPolicy);

app.UseOutputCache();
app.UseRateLimiter();

// Se você registrou autenticação JWT em AddInfrastructure, mantenha estes:
app.UseAuthentication();
app.UseAuthorization();


app.UseSwagger();
app.UseSwaggerUI(SwaggerConfig.ConfigureUI);


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Middleware global de exceções
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Rotas MVC/API
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Health
app.MapHealthChecks("/health");

app.Run();
