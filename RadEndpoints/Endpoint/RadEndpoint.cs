using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RadEndpoints.Mediator;

namespace RadEndpoints
{
    public abstract class RadEndpoint : IRadEndpoint
    {
        protected ILogger Logger { get; private set; } = null!;
        protected IEndpointRouteBuilder RouteBuilder { get; private set; } = null!;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext!;
        private IHttpContextAccessor _httpContextAccessor = null!;
        protected IWebHostEnvironment Env { get; private set; } = null!;
        protected bool HasValidator;

        public abstract void Configure();

        private static readonly Dictionary<Type, string> _routeCache = [];
        protected string SetRoute(string route)
        {
            _routeCache.TryAdd(GetType(), route);
            return route;
        }

        public static string GetRoute<TEndpoint>(TEndpoint endpoint) where TEndpoint: notnull => 
            _routeCache.TryGetValue(endpoint.GetType(), out var route)
                ? route
                : throw new InvalidOperationException($"Route not found for endpoint {typeof(TEndpoint).Name}.");

        public static string GetRoute<TEndpoint>() => 
            _routeCache.TryGetValue(typeof(TEndpoint), out var route)
                ? route
                : throw new InvalidOperationException($"Route not found for endpoint {typeof(TEndpoint).Name}.");

        void IRadEndpoint.EnableValidation()
        {
            HasValidator = true;
        }
        void IRadEndpoint.SetLogger(ILogger logger)
        {
            if (Logger is not null) throw new InvalidOperationException("Logger already set.");
            Logger = logger;
        }
        void IRadEndpoint.SetBuilder(IEndpointRouteBuilder routeBuilder)
        {
            if (RouteBuilder is not null) throw new InvalidOperationException("Route builder already set.");
            RouteBuilder = routeBuilder;
        }
        void IRadEndpoint.SetContext(IHttpContextAccessor contextAccessor)
        {
            if (_httpContextAccessor is not null) throw new InvalidOperationException("Context accessor already set.");
            _httpContextAccessor = contextAccessor;
        }

        void IRadEndpoint.SetEnvironment(IWebHostEnvironment env)
        {
            if (Env is not null) throw new InvalidOperationException("Host environment already set.");
            Env = env;
        }

        public T Service<T>() where T : notnull
        {
            return HttpContext.RequestServices.GetRequiredService<T>();
        }

        protected virtual IResult GetProblemResult(IRadProblem problem)
        {
            return problem switch
            {
                ValidationError v => TypedResults.Problem(title: v.Message, statusCode: StatusCodes.Status400BadRequest),
                ConflictError c => TypedResults.Problem(title: c.Message, statusCode: StatusCodes.Status409Conflict),
                NotFoundError n => TypedResults.Problem(title: n.Message, statusCode: StatusCodes.Status404NotFound),
                ForbiddenError f => TypedResults.Problem(title: f.Message, statusCode: StatusCodes.Status403Forbidden),
                InternalError i => TypedResults.Problem(title: i.Message, statusCode: StatusCodes.Status500InternalServerError),
                ExternalError e => TypedResults.Problem(title: e.Message, statusCode: StatusCodes.Status502BadGateway),
                _ => TypedResults.Problem(title: "Unknown error", statusCode: StatusCodes.Status500InternalServerError)
            };
        }
    }    
}