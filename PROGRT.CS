using FrutNatura.App.Application.DependencyInjection;
using FrutNatura.App.Application.UseCases.Mensagens.EnviarMensagem;
using FrutNatura.Core.Abstractions.Common;
using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Core.Abstractions.Security;
using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Core.Security.Policies;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// === LOGS ===
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// === MVC + Views + API ===
builder.Services.AddControllersWithViews()
    .AddJsonOptions(opt =>
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// (Opcional) limites do Kestrel
builder.Services.Configure<KestrelServerOptions>(o =>
{
    o.Limits.MaxRequestBodySize = 10 * 1024 * 1024;
    o.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);
});

// === CORS ===
const string CorsPolicy = "Default";
builder.Services.AddCors(opt =>
{
    var origins = config.GetSection("Cors:Origins").Get<string[]>() ?? Array.Empty<string>();
    opt.AddPolicy(CorsPolicy, p =>
        p.WithOrigins(origins)
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials());
});

// === RATE LIMITING / CACHE / HEALTH ===
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

// === APPLICATION + INFRA ===
builder.Services.AddApplication();
builder.Services.AddInfrastructure(config);

// === CONFIGURAÇÕES JWT ===
builder.Services.Configure<JwtOptions>(config.GetSection("Jwt"));

var jwt = config.GetSection("Jwt");
var key = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key não configurado");
var issuer = jwt["Issuer"] ?? "FrutNatura";
var audience = jwt["Audience"] ?? "FrutNatura.Api";


// === AUTORIZAÇÃO ===
builder.Services.AddAuthorization();

// === DEPENDÊNCIAS ===
// Segurança
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Infraestrutura
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();
builder.Services.AddScoped<IRefreshTokensService, RefreshTokensRepository>();

// === SWAGGER ===
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerConfig.Configure);

var app = builder.Build();


// === FRONT ===

var frontPath = Path.Combine(Directory.GetParent(builder.Environment.ContentRootPath)!.FullName,
    "FrutNatura.VIews.Web", "wwwroot");

if (Directory.Exists(frontPath))
{
    app.UseDefaultFiles(new DefaultFilesOptions
    {
        FileProvider = new PhysicalFileProvider(frontPath),
        DefaultFileNames = new List<string> { "Cliente/Tela de login/Login.html" }
    });

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(frontPath),
        RequestPath = ""
    });
}
else
{
    Console.WriteLine($"⚠ Caminho do front não encontrado: {frontPath}");
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("Default");
app.UseResponseCompression();
app.UseOutputCache();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

// === FALLBACK: redireciona para login.html se rota não existir ===
app.MapFallback(async context =>
{
    context.Response.Redirect("/Cliente/Tela de login/Login.html");
});

// === MIDDLEWARE GLOBAL DE ERROS ===
app.UseMiddleware<FrutNatura.WebApi.Setup.Middleware.ExceptionHandlingMiddleware>();

// === AMBIENTE ===
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(SwaggerConfig.ConfigureUI);
}
else
{
    app.UseHsts();
}

// === PIPELINE ===
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(CorsPolicy);
app.UseResponseCompression();
app.UseOutputCache();
app.UseRateLimiter();

app.UseAuthentication();
app.UseAuthorization();

// === ROTAS ===
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHealthChecks("/health");

app.Run();
