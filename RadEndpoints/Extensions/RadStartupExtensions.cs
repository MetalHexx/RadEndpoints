using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RadEndpoints.Mediator;
using System.Reflection;

namespace RadEndpoints
{
    public static class RadStartupExtensions
    {
        /// <summary>
        /// Scans assembly and registers endpoint classes as scoped services.
        /// </summary>
        /// <param name="services">The service collection where the endpoints will be registered</param>
        /// <param name="assemblyType">Assembly to scan</param>
        public static void AddRadEndpoints(this IServiceCollection services, Type assemblyType)
        {
            services.AddHttpContextAccessor();
            services.AddScopedAsSelfAndTypeOf<IRadEndpoint>(assemblyType.Assembly);
            services.AddScopedAsSelfAndTypeOf<IRadMapper>(assemblyType.Assembly);
            services.AddScoped<IRadMediator, RadMediator>();
        }

        /// <summary>
        /// Scans an assembly for a specific type and registers a single instance as both itself and as the type it implements.
        /// </summary>
        /// <typeparam name="T">Type to register as</typeparam>
        /// <param name="services">Application Service collection</param>
        /// <param name="assembly">Assembly to scan</param>
        private static void AddScopedAsSelfAndTypeOf<T> (this IServiceCollection services, Assembly assembly) where T : class
        {
            var types = assembly.FindConcreteImplementationsOf<T>();

            foreach (var endpointType in types)
            {
                services.AddScoped(endpointType);
                services.AddScoped(typeof(T), serviceProvider => serviceProvider.GetRequiredService(endpointType));
            }
        }

        /// <summary>
        /// Maps endpoints
        /// </summary>
        /// <param name="app">Web application instance</param>
        /// <param name="assemblyType">The assembly to scan for endpoint classes</param>
        /// <exception cref="InvalidOperationException">Throws an exception if there are any problems mapping endpoints</exception>
        public static void MapRadEndpoints(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var provider = scope.ServiceProvider;

            var endpoints = provider.GetServices<IRadEndpoint>();

            foreach (var endpoint in endpoints)
            {
                if (provider.IsValidatorRegistered(endpoint))
                {
                    endpoint.EnableValidation();
                }
                endpoint.SetBuilder(app);
                endpoint.Configure();
            }
            TriggerMediatorRegistrations(provider);
        }

        private static void TriggerMediatorRegistrations(IServiceProvider provider) => provider.GetRequiredService<IRadMediator>();
    }
}