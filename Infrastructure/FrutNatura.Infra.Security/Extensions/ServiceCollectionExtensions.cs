using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Infra.Security.Jwt;
using FrutNatura.Infra.Security.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FrutNatura.Infra.Security.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration cfg)
    {
        // Bind opções
        services.Configure<JwtOptions>(cfg.GetSection("Jwt"));

        // Token service
        services.AddScoped<ITokenService, JwtTokenService>();

        // JwtBearer
        var tmp = new JwtOptions();
        cfg.GetSection("Jwt").Bind(tmp);
        var keyBytes = Encoding.UTF8.GetBytes(tmp.Key);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = tmp.Issuer,
                    ValidAudience = tmp.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ClockSkew = TimeSpan.FromMinutes(2)
                };

                // Se for usar SignalR com JWT nos headers de WS:
                o.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        // Permite JWT via query ?access_token=.. para hubs em /hubs/*
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.HasValue && path.Value!.StartsWith("/hubs/", StringComparison.OrdinalIgnoreCase))
                        {
                            context.Token = accessToken!;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

      
        return services;
    }
}
