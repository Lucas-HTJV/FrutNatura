using FrutNatura.Core.Abstractions.Services;
using FrutNatura.Infra.AI.Extensions;
using FrutNatura.Infra.Email.Extensions;
using FrutNatura.Infra.Persistence;
using FrutNatura.Infra.Persistence.Services;
using FrutNatura.Infra.Realtime.Extensions;
using FrutNatura.Infra.Security.Extensions;
using FrutNatura.Infra.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FrutNatura.Core.Abstractions.Repositories;


namespace FrutNatura.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddPersistence(config);
        services.AddSecurity(config);
        services.AddEmail(config);
        services.AddAi(config);
        services.AddRealtime();

        return services;
    }   
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        
        services.AddScoped<IChamadosService, ChamadoService>();

       

       
        return services;
    }
    

}
