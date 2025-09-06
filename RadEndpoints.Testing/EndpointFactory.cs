using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace RadEndpoints.Testing
{
    /// <summary>
    /// Factory class for creating testable RadEndpoint instances with mocked dependencies.
    /// </summary>
    public static class EndpointFactory
    {
        /// <summary>
        /// Creates a testable RadEndpoint instance with default mocked dependencies.
        /// Supports endpoints with or without a mapper.
        /// 
        /// Use RadEndpointTestExtensions to access TypedResults:
        /// - endpoint.GetResult&lt;Ok&lt;T&gt;&gt;() - Gets Ok results
        /// - endpoint.GetResult&lt;Created&lt;T&gt;&gt;() - Gets Created results  
        /// - endpoint.GetResult&lt;NotFound&lt;T&gt;&gt;() - Gets NotFound results
        /// - endpoint.GetResult&lt;Conflict&lt;T&gt;&gt;() - Gets Conflict results
        /// - endpoint.GetResult&lt;ValidationProblem&gt;() - Gets ValidationProblem results
        /// - endpoint.GetResult&lt;UnauthorizedHttpResult&gt;() - Gets authentication challenge results (parameterless SendUnauthorized())
        /// - endpoint.GetResult&lt;ForbidHttpResult&gt;() - Gets authentication forbid results (parameterless SendForbidden())
        /// - endpoint.GetResult&lt;RedirectHttpResult&gt;() - Gets Redirect results
        /// - endpoint.GetResult&lt;ProblemHttpResult&gt;() - Gets Problem results (for errors with messages: SendUnauthorized(string), SendForbidden(string), SendInternalError, etc.)
        /// - endpoint.GetProblem&lt;T&gt;() - Gets typed problems (e.g., IRadProblem implementations)
        /// - endpoint.GetStatusCode() - Gets the HTTP status code
        /// </summary>
        /// <typeparam name="T">The type of the RadEndpoint to create.</typeparam>
        /// <param name="constructorArgs">Optional constructor arguments.</param>
        /// <returns>A mockable instance of the endpoint.</returns>
        public static T CreateEndpoint<T>(params object[] constructorArgs) where T : RadEndpoint
        {
            return CreateEndpoint<T>(
                logger: null,
                httpContextAccessor: null,
                webHostEnvironment: null,
                constructorArgs);
        }

        /// <summary>
        /// Creates a testable RadEndpoint instance with customizable mocked dependencies.
        /// Supports endpoints with or without a mapper.
        /// 
        /// Use RadEndpointTestExtensions to access TypedResults:
        /// - endpoint.GetResult&lt;Ok&lt;T&gt;&gt;() - Gets Ok results
        /// - endpoint.GetResult&lt;Created&lt;T&gt;&gt;() - Gets Created results  
        /// - endpoint.GetResult&lt;NotFound&lt;T&gt;&gt;() - Gets NotFound results
        /// - endpoint.GetResult&lt;Conflict&lt;T&gt;&gt;() - Gets Conflict results
        /// - endpoint.GetResult&lt;ValidationProblem&gt;() - Gets ValidationProblem results
        /// - endpoint.GetResult&lt;UnauthorizedHttpResult&gt;() - Gets authentication challenge results (parameterless SendUnauthorized())
        /// - endpoint.GetResult&lt;ForbidHttpResult&gt;() - Gets authentication forbid results (parameterless SendForbidden())
        /// - endpoint.GetResult&lt;RedirectHttpResult&gt;() - Gets Redirect results
        /// - endpoint.GetResult&lt;ProblemHttpResult&gt;() - Gets Problem results (for errors with messages: SendUnauthorized(string), SendForbidden(string), SendInternalError, etc.)
        /// - endpoint.GetProblem&lt;T&gt;() - Gets typed problems (e.g., IRadProblem implementations)
        /// - endpoint.GetStatusCode() - Gets the HTTP status code
        /// </summary>
        /// <typeparam name="T">The type of the RadEndpoint to create.</typeparam>
        /// <param name="logger">Optional custom logger. If null, a default logger will be created.</param>
        /// <param name="httpContextAccessor">Optional custom HTTP context accessor. If null, a default HTTP context accessor will be created.</param>
        /// <param name="webHostEnvironment">Optional custom web host environment. If null, a default environment will be created.</param>
        /// <param name="constructorArgs">Optional constructor arguments.</param>
        /// <returns>A mockable instance of the endpoint.</returns>
        public static T CreateEndpoint<T>(
            ILogger<T>? logger = null,
            IHttpContextAccessor? httpContextAccessor = null,
            IWebHostEnvironment? webHostEnvironment = null,
            params object[] constructorArgs) where T : RadEndpoint
        {
            var endpoint = Substitute.ForPartsOf<T>(constructorArgs);

            httpContextAccessor ??= new FakeHttpContextAccessor();
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext != null && httpContext.RequestServices == null)
            {
                var serviceCollection = new ServiceCollection();
                var serviceProvider = serviceCollection.BuildServiceProvider();
                httpContext.RequestServices = serviceProvider;
            }

            logger ??= CreateDefaultLogger<T>();

            var routeBuilder = new FakeEndpointRouteBuilder();

            webHostEnvironment ??= CreateDefaultWebHostEnvironment();

            ((IRadEndpoint)endpoint).SetLogger(logger);
            ((IRadEndpoint)endpoint).SetBuilder(routeBuilder);
            ((IRadEndpoint)endpoint).SetContext(httpContextAccessor);
            ((IRadEndpoint)endpoint).SetEnvironment(webHostEnvironment);


            InjectMapperIfRequired(endpoint);

            return endpoint;
        }

        /// <summary>
        /// Creates a default logger for the specified type.
        /// </summary>
        private static ILogger<T> CreateDefaultLogger<T>()
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            return loggerFactory.CreateLogger<T>();
        }

        /// <summary>
        /// Creates a default web host environment with Development settings.
        /// </summary>
        private static IWebHostEnvironment CreateDefaultWebHostEnvironment()
        {
            var environment = Substitute.For<IWebHostEnvironment>();
            environment.EnvironmentName.Returns("Development");
            environment.ApplicationName.Returns("TestApp");
            environment.ContentRootPath.Returns(AppContext.BaseDirectory);
            return environment;
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