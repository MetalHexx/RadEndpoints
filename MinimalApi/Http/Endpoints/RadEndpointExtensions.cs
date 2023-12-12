using System.Reflection;

namespace MinimalApi.Http.Endpoints
{
    public static class RadEndpointExtensions
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

            var endpointTypes = GetEndpointTypes();

            foreach (var endpointType in endpointTypes)
            {
                var endpoint = (RadEndpoint)app.Services.GetRequiredService(endpointType);

                if (IsRequestValidatorRegistered(app.Services, endpoint)) endpoint.EnableValidation();
                AddMapper(endpointType, endpoint);

                endpoint.SetLogger(GetLogger(app, endpointType));
                endpoint.SetContext(httpContextAccessor);
                endpoint.SetBuilder(app);
                endpoint.SetEnvironment(env);
                endpoint.Configure();
            }
        }

        private static ILogger GetLogger(WebApplication app, Type endpointType)
        {
            var loggerType = typeof(ILogger<>).MakeGenericType(endpointType);
            var logger = (ILogger)app.Services.GetRequiredService(loggerType);
            return logger;
        }

        private static void AddMapper(Type endpointType, RadEndpoint endpoint)
        {
            if (endpointType.BaseType is null) return;

            if (!IsSubclassOfRawGeneric(typeof(RadEndpoint<,,>), endpointType) && !IsSubclassOfRawGeneric(typeof(RadEndpointWithoutRequest<,>), endpointType)) return;
            
            var genericArguments = endpointType.BaseType.GetGenericArguments();

            foreach (var arg in genericArguments)
            {
                if (typeof(IRadMapper).IsAssignableFrom(arg))
                {
                    var mapper = Activator.CreateInstance(arg);
                    var setMapperMethod = endpoint.GetType().GetMethod("SetMapper", [arg]);
                    setMapperMethod?.Invoke(endpoint, [mapper]);
                    break;
                }
            }
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }

        public static RouteHandlerBuilder AddSwagger(this RouteHandlerBuilder routeBuilder, string tag, string desc) => routeBuilder
            .WithTags(tag)
            .WithDescription(desc)
            .WithOpenApi();

        private static Type? GetRequestType(RadEndpoint endpointInstance)
        {
            var currentType = endpointInstance.GetType();

            while (currentType != null && currentType != typeof(object))
            {
                if (currentType.IsGenericType &&
                    currentType.GetGenericTypeDefinition() == typeof(RadEndpoint<,>))
                {
                    var requestType = currentType.GetGenericArguments()[0];
                    return requestType;
                }

                currentType = currentType.BaseType;
            }

            return null;
        }

        private static bool IsRequestValidatorRegistered(this IServiceProvider serviceProvider, RadEndpoint endpoint)
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

        private static IEnumerable<Type> GetEndpointTypes() => Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(RadEndpoint)));
    }
}
