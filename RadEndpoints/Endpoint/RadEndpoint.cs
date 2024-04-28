using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RadEndpoints.Mediator;

namespace RadEndpoints
{
    public abstract partial class RadEndpoint : IRadEndpoint
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
    public abstract partial class RadEndpoint<TRequest, TResponse> : RadEndpoint, IRadEndpoint<TRequest, TResponse>
        where TRequest : class
        where TResponse : new()
    {
        public TResponse Response { get; set; } = new();
        public abstract Task Handle(TRequest r, CancellationToken ct);        

        public RouteHandlerBuilder Get(string route)
        {
            SetRoute(route);
            var builder = RouteBuilder!.MapGet(route, async ([AsParameters] TRequest r, IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(r, m, c, ct));
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Post(string route)
        {
            SetRoute(route);
            var builder = RouteBuilder!.MapPost(route, async (TRequest r, IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(r, m, c, ct));
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Put(string route)
        {
            SetRoute(route);
            var builder = RouteBuilder!.MapPut(route, async ([AsParameters] TRequest r, IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(r, m, c, ct));
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Patch(string route)
        {
            var builder = RouteBuilder!.MapPatch(route, async (TRequest r, IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(r, m, c, ct));
            SetRoute(route);
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Delete(string route)
        {
            var builder = RouteBuilder!.MapDelete(route, async ([AsParameters] TRequest r, IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(r, m, c, ct));
            SetRoute(route);
            return TryAddEndpointFilter(builder);
        }

        private RouteHandlerBuilder TryAddEndpointFilter(RouteHandlerBuilder builder)
        {
            if (HasValidator) builder.AddEndpointFilter<RadValidationFilter<TRequest>>();
            return builder;
        }

        private IRadEndpoint<TRequest, TResponse> SelfInterface => this;
    }

    public abstract class RadEndpoint<TRequest, TResponse, TMapper> : RadEndpoint<TRequest, TResponse>, IRadEndpointWithMapper
        where TRequest : class
        where TResponse : new()
        where TMapper : class, IRadMapper
    {
        protected TMapper Map { get; private set; } = default!;
        void IRadEndpointWithMapper.SetMapper(IRadMapper mapper)
        {
            Map = mapper as TMapper ?? throw new InvalidOperationException("Invalid mapper type.");
        }
    }

    public abstract partial class RadEndpointWithoutRequest<TResponse> : RadEndpoint, IRadEndpointWithoutRequest<TResponse>
        where TResponse : new()
    {
        public TResponse Response { get; set; } = new();
        public abstract Task Handle(CancellationToken ct);

        public RouteHandlerBuilder Get(string route)
        {
            SetRoute(route);
            return RouteBuilder!.MapGet(route, async (IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(m, c, ct));
        }

        public RouteHandlerBuilder Post(string route)
        {
            SetRoute(route);
            return RouteBuilder!.MapPost(route, async (IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(m, c, ct));
        }

        public RouteHandlerBuilder Put(string route)
        {
            SetRoute(route);
            return RouteBuilder!.MapPut(route, async (IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(m, c, ct));
        }

        public RouteHandlerBuilder Patch(string route)
        {
            SetRoute(route);
            return RouteBuilder!.MapPatch(route, async (IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(m, c, ct));
            
        }

        public RouteHandlerBuilder Delete(string route)
        {
            SetRoute(route);
            return RouteBuilder!.MapDelete(route, async (IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(m, c, ct));
        }

        private IRadEndpointWithoutRequest<TResponse> SelfInterface => this;
    }

    public abstract class RadEndpointWithoutRequest<TResponse, TMapper> : RadEndpointWithoutRequest<TResponse>, IRadEndpointWithMapper
        where TResponse : new()
        where TMapper : class, IRadMapper
    {
        protected TMapper Map { get; private set; } = default!;
        void IRadEndpointWithMapper.SetMapper(IRadMapper mapper)
        {
            Map = mapper as TMapper ?? throw new InvalidOperationException("Invalid mapper type.");
        }
    }
}