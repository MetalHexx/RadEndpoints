using Microsoft.Extensions.DependencyInjection;
using MinimalApi.Features.Examples.CreateExample;
using MinimalApi.Http.Filters;
using System.Reflection;

namespace MinimalApi.Http.Endpoints
{
    public static class EndpointExtensions
    {
        public static void AddEndpoints(this IServiceCollection services)
        {
            var endpointTypes = GetEndpointTypes();

            foreach (var endpointType in endpointTypes)
            {
                services.AddSingleton(endpointType);
            }
        }

        public static void MapEndpoints(this WebApplication app)
        {
            var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
            var env = app.Services.GetRequiredService<IWebHostEnvironment>();
            var logger = app.Services.GetRequiredService<ILogger<Endpoint>>();

            var endpointTypes = GetEndpointTypes();

            foreach (var endpointType in endpointTypes)
            {
                var endpoint = (Endpoint)app.Services.GetRequiredService(endpointType);

                if (IsRequestValidatorRegistered(app.Services, endpoint)) endpoint.EnableValidation();

                endpoint.SetLogger(logger);
                endpoint.SetContext(httpContextAccessor);
                endpoint.SetBuilder(app);
                endpoint.SetEnvironment(env);
                endpoint.Configure();
            }
        }

        public static RouteHandlerBuilder AddSwagger(this RouteHandlerBuilder routeBuilder, string tag, string desc) => routeBuilder
            .WithTags(tag)
            .WithDescription(desc)
            .WithOpenApi();

        private static Type? GetRequestType(Endpoint endpointInstance)
        {
            var currentType = endpointInstance.GetType();

            while (currentType != null && currentType != typeof(object))
            {
                if (currentType.IsGenericType &&
                    currentType.GetGenericTypeDefinition() == typeof(Endpoint<,>))
                {
                    var requestType = currentType.GetGenericArguments()[0];
                    return requestType;
                }

                currentType = currentType.BaseType;
            }

            return null;
        }

        private static bool IsRequestValidatorRegistered(this IServiceProvider serviceProvider, Endpoint endpoint)
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

        private static IEnumerable<Type> GetEndpointTypes() => Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Endpoint)));
    }
}
