using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace RadEndpoints
{
    public static class RadStartupExtensions
    {
        /// <summary>
        /// Registers endpoint classes.
        /// </summary>
        /// <remarks>Endpoint classes are always registered as singletons. 
        /// Only singleton dependencies are safely supported with constructor injection.</remarks>
        /// <param name="services">The service collection where the endpoints will be registered</param>
        /// <param name="assemblyType">The assembly to scan for endpoint classes</param>
        public static void AddEndpoints(this IServiceCollection services, Type assemblyType)
        {
            var endpointTypes = GetEndpointTypes(assemblyType);

            foreach (var endpointType in endpointTypes)
            {
                services.AddSingleton(endpointType);
            }
        }

        /// <summary>
        /// Maps endpoints
        /// </summary>
        /// <param name="app">Web application instance</param>
        /// <param name="assemblyType">The assembly to scan for endpoint classes</param>
        /// <exception cref="InvalidOperationException">Throws an exception if there are any problems mapping endpoints</exception>
        public static void MapEndpoints(this WebApplication app, Type assemblyType)
        {
            using var scope = app.Services.CreateScope();
            var provider = scope.ServiceProvider;

            var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
            var env = app.Services.GetRequiredService<IWebHostEnvironment>();

            var endpointTypes = GetEndpointTypes(assemblyType);

            foreach (var endpointType in endpointTypes)
            {
                var endpoint = provider.GetRequiredService(endpointType) as IRadEndpoint
                    ?? throw new InvalidOperationException($"Endpoint {endpointType.Name} not found as a registered service.");

                if (IsRequestValidatorRegistered(provider, endpoint)) endpoint.EnableValidation();

                AddMapper(endpointType, endpoint);

                endpoint.SetLogger(GetLogger(provider, endpointType));
                endpoint.SetContext(httpContextAccessor);
                endpoint.SetBuilder(app);
                endpoint.SetEnvironment(env);
                endpoint.Configure();
            }
        }

        private static IEnumerable<Type> GetEndpointTypes(Type assemblyType) => assemblyType.Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(typeof(IRadEndpoint)));

        private static ILogger GetLogger(this IServiceProvider serviceProvider, Type endpointType)
        {
            var loggerType = typeof(ILogger<>).MakeGenericType(endpointType);
            var logger = (ILogger)serviceProvider.GetRequiredService(loggerType);
            return logger;
        }

        private static void AddMapper(Type endpointType, IRadEndpoint endpoint)
        {
            var currentType = endpointType;
            var mapperType = typeof(IRadMapper);

            while (currentType != null && currentType != typeof(object))
            {
                if (currentType.IsGenericType)
                {
                    var genericArguments = currentType.GetGenericArguments();
                    foreach (var arg in genericArguments)
                    {
                        if (mapperType.IsAssignableFrom(arg))
                        {
                            var mapper = Activator.CreateInstance(arg);
                            var setMapperMethod = endpoint.GetType().GetMethod("SetMapper", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new[] { arg }, null);
                            setMapperMethod?.Invoke(endpoint, new[] { mapper });
                            return;
                        }
                    }
                }
                currentType = currentType.BaseType;
            }
        }

        private static Type? GetRequestType(IRadEndpoint endpointInstance)
        {
            var endpointType = endpointInstance.GetType();

            while (endpointType != null && endpointType != typeof(object))
            {
                if (endpointType.IsGenericType)
                {
                    var genericArguments = endpointType.GetGenericArguments();
                    var requestType = genericArguments.FirstOrDefault(arg => typeof(RadRequest).IsAssignableFrom(arg));

                    if (requestType != null)
                    {
                        return requestType;
                    }
                }

                endpointType = endpointType.BaseType;
            }

            return null;
        }

        private static bool IsRequestValidatorRegistered(this IServiceProvider serviceProvider, IRadEndpoint endpoint)
        {
            var requestType = GetRequestType(endpoint);

            if (requestType is null) return false;

            var validatorType = typeof(IValidator<>).MakeGenericType(requestType);
            var services = serviceProvider.GetServices(validatorType);

            foreach (var service in services)
            {
                if (service is null) continue;

                if (validatorType.IsAssignableFrom(service.GetType()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}