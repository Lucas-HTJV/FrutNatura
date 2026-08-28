using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using FluentValidation;

namespace FrutNatura.App.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // ✅ Use um tipo dentro da camada Application
            var assembly = typeof(FrutNatura.App.Application.UseCases.Mensagens.EnviarMensagem.EnviarMensagemCommand)
                .Assembly;

            // 🔹 MediatR (v12+)
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
            });

            // 🔹 FluentValidation
            services.AddValidatorsFromAssembly(assembly);

            // 🔹 AutoMapper (se estiver usando)
            services.AddAutoMapper(assembly);

            // 🔹 Behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviors.ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviors.UnitOfWorkBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviors.PerformanceBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviors.UnhandledExceptionBehavior<,>));


            return services;
        }
    }
}
