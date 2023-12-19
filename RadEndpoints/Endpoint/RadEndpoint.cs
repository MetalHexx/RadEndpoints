using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RadEndpoints.Mediator;
using System.Collections.Concurrent;

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
        protected virtual IResult Ok() => TypedResults.Ok();
        protected virtual IResult Ok<TResponse>(TResponse responseData) => TypedResults.Ok(responseData);
        protected virtual IResult Created(string uri) => TypedResults.Created(uri);
        protected virtual IResult Created<TResponse>(string uri, TResponse response) => TypedResults.Created(uri, response);
        protected virtual IResult ServerError(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status500InternalServerError);
        protected virtual IResult ValidationError(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status400BadRequest);
        protected virtual IResult Conflict(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status409Conflict);
        protected virtual IResult NotFound(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status404NotFound);
        protected virtual IResult Forbidden(string title) => TypedResults.Problem(title: title, statusCode: StatusCodes.Status403Forbidden);

        public abstract void Configure();

        private static readonly ConcurrentDictionary<Type, string> _routeCache = new();
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
    }
    public abstract class RadEndpoint<TRequest, TResponse> : RadEndpoint, IRadEndpoint<TRequest, TResponse>
        where TRequest : RadRequest
        where TResponse : RadResponse, new()
    {
        public TResponse Response { get; set; } = new();

        public abstract Task<IResult> Handle(TRequest r, CancellationToken ct);

        public RouteHandlerBuilder Get(string route)
        {
            SetRoute(route);
            var builder = RouteBuilder!.MapGet(route, async ([AsParameters] TRequest r, IRadMediator m, CancellationToken ct) => await ExecuteHandler(r, m, ct));
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Post(string route)
        {
            SetRoute(route);
            var builder = RouteBuilder!.MapPost(route, async (TRequest r, IRadMediator m, CancellationToken ct) => await ExecuteHandler(r, m, ct));
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Put(string route)
        {
            SetRoute(route);
            var builder = RouteBuilder!.MapPut(route, async ([AsParameters] TRequest r, IRadMediator m, CancellationToken ct) => await ExecuteHandler(r, m, ct));
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Patch(string route)
        {
            var builder = RouteBuilder!.MapPatch(route, async (TRequest r, IRadMediator m, CancellationToken ct) => await ExecuteHandler(r, m, ct));
            SetRoute(route);
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Delete(string route)
        {
            var builder = RouteBuilder!.MapDelete(route, async ([AsParameters] TRequest r, IRadMediator m, CancellationToken ct) => await ExecuteHandler(r, m, ct));
            SetRoute(route);
            return TryAddEndpointFilter(builder);
        }
        private RouteHandlerBuilder TryAddEndpointFilter(RouteHandlerBuilder builder)
        {
            if (HasValidator) builder.AddEndpointFilter<RadValidationFilter<TRequest>>();
            return builder;
        }
        protected async Task<IResult> ExecuteHandler(TRequest r, IRadMediator mediator, CancellationToken ct)
        {
            return await mediator.SendAsync<TRequest, TResponse>(r, ct);            
        }
    }
    public abstract class RadEndpoint<TRequest, TResponse, TMapper> : RadEndpoint<TRequest, TResponse>, IRadEndpointWithMapper
    where TResponse : RadResponse, new()
    where TRequest : RadRequest
    where TMapper : class, IRadMapper
    {
        protected TMapper Map { get; private set; } = default!;
        void IRadEndpointWithMapper.SetMapper(IRadMapper mapper)
        {
            Map = mapper as TMapper ?? throw new InvalidOperationException("Invalid mapper type.");
        }
    }
}