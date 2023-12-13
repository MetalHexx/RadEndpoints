using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace RadEndpoints
{
    public abstract class RadEndpoint
    {
        protected ILogger Logger { get; private set; } = null!;
        protected IEndpointRouteBuilder RouteBuilder { get; private set; } = null!;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext!;
        private IHttpContextAccessor _httpContextAccessor = null!;
        protected IWebHostEnvironment Env { get; private set; } = null!;
        protected bool HasValidator;

        protected static Ok Ok() => TypedResults.Ok();
        protected static Ok<TResponse> Ok<TResponse>(TResponse responseData) => TypedResults.Ok(responseData);
        protected static Created Created(string uri) => TypedResults.Created(uri);
        protected static Created<TResponse> Created<TResponse>(string uri, TResponse response) => TypedResults.Created(uri, response);
        protected static ProblemHttpResult ServerError(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status500InternalServerError);
        protected static ProblemHttpResult ValidationError(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status400BadRequest);
        protected static ProblemHttpResult Conflict(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status409Conflict);
        protected static ProblemHttpResult NotFound(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status404NotFound);
        protected static ProblemHttpResult Forbidden(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status403Forbidden);

        public abstract void Configure();

        private static readonly ConcurrentDictionary<Type, string> _routeCache = new();
        public void SetRoute(string route) => _routeCache.TryAdd(GetType(), route);

        public static string GetRoute<TEndpoint>() => _routeCache.TryGetValue(typeof(TEndpoint), out var route)
            ? route
            : throw new InvalidOperationException($"Route not found for endpoint {typeof(TEndpoint).Name}.");

        public void EnableValidation()
        {
            HasValidator = true;
        }
        public void SetLogger(ILogger logger)
        {
            if (Logger is not null) throw new InvalidOperationException("Logger already set.");
            Logger = logger;
        }
        public void SetBuilder(IEndpointRouteBuilder routeBuilder)
        {
            if (RouteBuilder is not null) throw new InvalidOperationException("Route builder already set.");
            RouteBuilder = routeBuilder;
        }
        public void SetContext(IHttpContextAccessor contextAccessor)
        {
            if (_httpContextAccessor is not null) throw new InvalidOperationException("Context accessor already set.");
            _httpContextAccessor = contextAccessor;
        }

        public void SetEnvironment(IWebHostEnvironment env)
        {
            if (Env is not null) throw new InvalidOperationException("Host environment already set.");
            Env = env;
        }
    }
    public abstract class RadEndpoint<TRequest, TResponse> : RadEndpoint
        where TResponse : RadResponse, new()
        where TRequest : RadRequest
    {
        public TResponse Response { get; set; } = new();

        public abstract Task<IResult> Handle(TRequest r, CancellationToken ct);

        public static Ok<TResponse> Ok(TResponse response) => TypedResults.Ok(response);
        protected static Created<TResponse> Created(string uri, TResponse response) => TypedResults.Created(uri, response);

        public RouteHandlerBuilder Get(string route)
        {
            SetRoute(route);
            var builder = RouteBuilder!.MapGet(route, async ([AsParameters] TRequest request, CancellationToken ct) => await Handle(request, ct));
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Post(string route)
        {
            SetRoute(route);
            var builder = RouteBuilder!.MapPost(route, async (TRequest request, CancellationToken ct) => await Handle(request, ct));
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Put(string route)
        {
            SetRoute(route);
            var builder = RouteBuilder!.MapPut(route, async ([AsParameters] TRequest request, CancellationToken ct) => await Handle(request, ct));
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Patch(string route)
        {
            var builder = RouteBuilder!.MapPatch(route, async (TRequest request, CancellationToken ct) => await Handle(request, ct));
            SetRoute(route);
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Delete(string route)
        {
            var builder = RouteBuilder!.MapDelete(route, async ([AsParameters] TRequest request, CancellationToken ct) => await Handle(request, ct));
            SetRoute(route);
            return TryAddEndpointFilter(builder);
        }
        private RouteHandlerBuilder TryAddEndpointFilter(RouteHandlerBuilder builder)
        {
            if (HasValidator) builder.AddEndpointFilter<RadValidationFilter<TRequest>>();
            return builder;
        }
    }

    public abstract class RadEndpoint<TRequest, TResponse, TMapper> : RadEndpoint<TRequest, TResponse>
        where TResponse : RadResponse, new()
        where TRequest : RadRequest
        where TMapper : IRadMapper
    {
        protected TMapper Map { get; private set; } = default!;
        public void SetMapper(TMapper mapper)
        {
            Map = mapper;
        }
    }

    public abstract class RadEndpointWithoutRequest<TResponse> : RadEndpoint
        where TResponse : RadResponse, new()
    {
        public TResponse Response { get; set; } = new();
        public abstract Task<IResult> Handle(CancellationToken ct);

        public static Ok<TResponse> Ok(TResponse response) => TypedResults.Ok(response);
        protected static Created<TResponse> Created(string uri, TResponse response) => TypedResults.Created(uri, response);

        public RouteHandlerBuilder Get(string route)
        {
            SetRoute(route);
            return RouteBuilder!.MapGet(route, async (CancellationToken ct) => await Handle(ct));
        }
        public RouteHandlerBuilder Post(string route)
        {
            SetRoute(route);
            return RouteBuilder!.MapPost(route, async (CancellationToken ct) => await Handle(ct));
        }
        public RouteHandlerBuilder Put(string route)
        {
            SetRoute(route);
            return RouteBuilder!.MapPut(route, async (CancellationToken ct) => await Handle(ct));
        }
        public RouteHandlerBuilder Patch(string route)
        {
            SetRoute(route);
            return RouteBuilder!.MapPatch(route, async (CancellationToken ct) => await Handle(ct));
        }
        public RouteHandlerBuilder Delete(string route)
        {
            SetRoute(route);
            return RouteBuilder!.MapDelete(route, async (CancellationToken ct) => await Handle(ct));
        }
    }

    public abstract class RadEndpointWithoutRequest<TResponse, TMapper> : RadEndpointWithoutRequest<TResponse>
        where TResponse : RadResponse, new()
        where TMapper : IRadMapper
    {
        protected TMapper Map { get; private set; } = default!;
        public void SetMapper(TMapper mapper)
        {
            Map = mapper;
        }
    }
}