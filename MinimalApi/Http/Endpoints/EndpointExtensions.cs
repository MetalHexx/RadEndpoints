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
            var logger = app.Services.GetRequiredService<ILogger<Endpoint>>();

            var endpointTypes = GetEndpointTypes();

            foreach (var endpointType in endpointTypes)
            {
                var endpointInstance = (Endpoint)app.Services.GetRequiredService(endpointType);

                endpointInstance.SetLogger(logger);
                endpointInstance!.SetContext(httpContextAccessor);
                endpointInstance.SetBuilder(app);
                endpointInstance.Configure();
            }
        }

        private static IEnumerable<Type> GetEndpointTypes() => Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Endpoint)));
    }
}
