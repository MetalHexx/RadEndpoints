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
        public static void AddEndpoints(this IServiceCollection services, Type assemblyType)
        {
            var endpointTypes = GetEndpointTypes(assemblyType);

            foreach (var endpointType in endpointTypes)
            {
                services.AddSingleton(endpointType);
            }
        }

        public static void MapEndpoints(this WebApplication app, Type assemblyType)
        {
            var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
            var env = app.Services.GetRequiredService<IWebHostEnvironment>();

            var endpointTypes = GetEndpointTypes(assemblyType);

            foreach (var endpointType in endpointTypes)
            {
                var endpoint = app.Services.GetRequiredService(endpointType) as IRadEndpoint 
                    ?? throw new InvalidOperationException($"Endpoint {endpointType.Name} not found as a registered service.");
                
                if (IsRequestValidatorRegistered(app.Services, endpoint)) endpoint.EnableValidation();

                AddMapper(endpointType, endpoint);

                endpoint.SetLogger(GetLogger(app, endpointType));
                endpoint.SetContext(httpContextAccessor);
                endpoint.SetBuilder(app);
                endpoint.SetEnvironment(env);
                endpoint.Configure();
            }
        }

        private static IEnumerable<Type> GetEndpointTypes(Type assemblyType) => assemblyType.Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(typeof(IRadEndpoint)));

        private static ILogger GetLogger(WebApplication app, Type endpointType)
        {
            var loggerType = typeof(ILogger<>).MakeGenericType(endpointType);
            var logger = (ILogger)app.Services.GetRequiredService(loggerType);
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

            using (var scope = serviceProvider.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;
                var services = scopedProvider.GetServices(validatorType);

                foreach (var service in services)
                {
                    if (service is null) continue;

                    if (validatorType.IsAssignableFrom(service.GetType()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}