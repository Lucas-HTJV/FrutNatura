using Microsoft.OpenApi.Models;

namespace FrutNatura.WebApi.Setup;

public static class SwaggerConfig
{
    public static void Configure(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions c)
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "FrutNatura API",
            Version = "v1",
            Description = "API da FrutNatura (clientes e staff)."
        });

        var jwtScheme = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Insira o token JWT no formato: Bearer {token}",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        };

        c.AddSecurityDefinition("Bearer", jwtScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            [jwtScheme] = Array.Empty<string>()
        });
    }

    public static void ConfigureUI(Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIOptions ui)
    {
        ui.SwaggerEndpoint("/swagger/v1/swagger.json", "FrutNatura v1");
        ui.DisplayRequestDuration();
        ui.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    }
}
