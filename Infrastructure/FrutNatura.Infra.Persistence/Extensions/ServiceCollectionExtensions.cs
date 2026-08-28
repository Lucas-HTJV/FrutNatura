using FrutNatura.Core.Abstractions.Common; // se seu IUnitOfWork estiver aqui (ajuste o namespace)
using FrutNatura.Core.Abstractions.Repositories;
using FrutNatura.Infra.Persistence;
using FrutNatura.Infra.Persistence.Db;
using FrutNatura.Infra.Persistence.UniWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FrutNatura.Infra.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection"); 

        services.AddDbContext<FrutNaturaDbContext>(options =>
        {
            options.UseSqlServer(
                connectionString,
                sql =>
                {
                    
                    sql.MigrationsAssembly(typeof(FrutNaturaDbContext).Assembly.FullName);
                
                    sql.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
                });

            
#if DEBUG
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
#endif
        });

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositórios
        services.AddScoped<IChamadosRepository, ChamadosRepository>();
        services.AddScoped<IMensagensRepository, MensagensRepository>();
        services.AddScoped<IUsuariosRepository, UsuariosRepository>();

        return services;
    }
}
