using Microsoft.Extensions.DependencyInjection;
using FrutNatura.Core.Abstractions.Notifications;
using FrutNatura.Infra.Realtime;
using Microsoft.AspNetCore.SignalR;

namespace FrutNatura.Infra.Realtime.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRealtime(this IServiceCollection services)
    {
        services.AddSignalR();
        
        services.AddSingleton<IRealtimeNotifier, SignalRRealtimeNotifier>();
        return services;
    }
}
