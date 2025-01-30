using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace RadEndpoints.Testing
{
    public static class EndpointFactory

    {
        /// <summary>
        /// Creates a testable RadEndpoint instance with mocked dependencies.
        /// Supports endpoints with or without a mapper.
        /// </summary>
        /// <typeparam name="T">The type of the RadEndpoint to create.</typeparam>
        /// <param name="constructorArgs">Optional constructor arguments.</param>
        /// <returns>A mockable instance of the endpoint.</returns>
        public static T CreateEndpoint<T>(params object[] constructorArgs) where T : RadEndpoint
        {
            var endpoint = Substitute.ForPartsOf<T>(constructorArgs);

            var httpContextAccessor = new FakeHttpContextAccessor();
            var httpContext = httpContextAccessor.HttpContext;

            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            httpContext!.RequestServices = serviceProvider;

            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<T>();

            var routeBuilder = new FakeEndpointRouteBuilder();

            var webHostEnvironment = Substitute.For<IWebHostEnvironment>();
            webHostEnvironment.EnvironmentName.Returns("Development");
            webHostEnvironment.ApplicationName.Returns("TestApp");
            webHostEnvironment.ContentRootPath.Returns(AppContext.BaseDirectory);

            ((IRadEndpoint)endpoint).SetLogger(logger);
            ((IRadEndpoint)endpoint).SetBuilder(routeBuilder);
            ((IRadEndpoint)endpoint).SetContext(httpContextAccessor);
            ((IRadEndpoint)endpoint).SetEnvironment(webHostEnvironment);

            InjectMapperIfRequired(endpoint);

            return endpoint;
        }

        private static void InjectMapperIfRequired<T>(T endpoint) where T : RadEndpoint
        {
            var endpointType = typeof(T);

            if (!typeof(IRadEndpointWithMapper).IsAssignableFrom(endpointType))
                return;

            var baseType = endpointType.BaseType;
            while (baseType != null && !baseType.IsGenericType)
            {
                baseType = baseType.BaseType;
            }

            if (baseType == null || !baseType.IsGenericType) return;

            var genericArguments = baseType.GetGenericArguments();
            if (genericArguments.Length < 2) return;

            var mapperType = genericArguments.Last();

            if (!typeof(IRadMapper).IsAssignableFrom(mapperType)) return;

            var mapperInstance = Substitute.For([mapperType], []);

            ((IRadEndpointWithMapper)endpoint).SetMapper((IRadMapper)mapperInstance);
        }
    }

    public class FakeHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext? HttpContext { get; set; }

        public FakeHttpContextAccessor()
        {
            HttpContext = new DefaultHttpContext();
        }
    }

    public class FakeEndpointRouteBuilder : IEndpointRouteBuilder
    {
        public IServiceProvider ServiceProvider { get; }

        public ICollection<EndpointDataSource> DataSources { get; } = new List<EndpointDataSource>();

        public FakeEndpointRouteBuilder()
        {
            var serviceCollection = new ServiceCollection();
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public IApplicationBuilder CreateApplicationBuilder() => throw new NotImplementedException();
    }
}