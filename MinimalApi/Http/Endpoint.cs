namespace MinimalApi.Http
{
    public class ApiResponse
    {
        public string Message { get; set; } = string.Empty;
    }

    public abstract class Endpoint
    {
        protected ILogger Logger { get; private set; } = null!;
        protected IEndpointRouteBuilder RouteBuilder { get; private set; } = null!;
        protected HttpContext HttpContext => _httpContextAccessor.HttpContext!;

        private IHttpContextAccessor _httpContextAccessor = null!;

        public abstract void Configure();

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
    }
    public abstract class Endpoint<TRequest, TResponse> : Endpoint where TResponse : ApiResponse, new() where TRequest : class, new()
    {
        public TResponse Response { get; set; } = new();
        public abstract Task<IResult> Handle(TRequest r, CancellationToken ct);
        public RouteHandlerBuilder Get(string route) => RouteBuilder!.MapGet(route, async ([AsParameters] TRequest request, CancellationToken ct) => await Handle(request, ct));
        public RouteHandlerBuilder Post(string route)
        {
            var builder = RouteBuilder!.MapPost(route, async ([AsParameters] TRequest request, CancellationToken ct) => await Handle(request, ct));
            return builder;
        }
        public RouteHandlerBuilder Put(string route) => RouteBuilder!.MapPut(route, async ([AsParameters] TRequest request, CancellationToken ct) => await Handle(request, ct));
        public RouteHandlerBuilder Patch(string route) => RouteBuilder!.MapPatch(route, async ([AsParameters] TRequest request, CancellationToken ct) => await Handle(request, ct));
        public RouteHandlerBuilder Delete(string route) => RouteBuilder!.MapDelete(route, async ([AsParameters] TRequest request, CancellationToken ct) => await Handle(request, ct));
    }

    public abstract class EndpointWithoutRequest<TResponse> : Endpoint where TResponse : ApiResponse, new()
    {
        public TResponse Response { get; set; } = new();
        public abstract Task<IResult> Handle(CancellationToken ct);
        public RouteHandlerBuilder Get(string route) => RouteBuilder!.MapGet(route, async (CancellationToken ct) => await Handle(ct));
        public RouteHandlerBuilder Post(string route) => RouteBuilder!.MapPost(route, async (CancellationToken ct) => await Handle(ct));
        public RouteHandlerBuilder Put(string route) => RouteBuilder!.MapPut(route, async (CancellationToken ct) => await Handle(ct));
        public RouteHandlerBuilder Patch(string route) => RouteBuilder!.MapPatch(route, async (CancellationToken ct) => await Handle(ct));
        public RouteHandlerBuilder Delete(string route) => RouteBuilder!.MapDelete(route, async (CancellationToken ct) => await Handle(ct));
    }

    public abstract class EndpointWithoutRequest : Endpoint
    {
        public abstract Task<IResult> Handle(CancellationToken ct);
        public RouteHandlerBuilder Get(string route) => RouteBuilder!.MapGet(route, async (CancellationToken ct) => await Handle(ct));
        public RouteHandlerBuilder Post(string route) => RouteBuilder!.MapPost(route, async (CancellationToken ct) => await Handle(ct));
        public RouteHandlerBuilder Put(string route) => RouteBuilder!.MapPut(route, async (CancellationToken ct) => await Handle(ct));
        public RouteHandlerBuilder Patch(string route) => RouteBuilder!.MapPatch(route, async (CancellationToken ct) => await Handle(ct));
        public RouteHandlerBuilder Delete(string route) => RouteBuilder!.MapDelete(route, async (CancellationToken ct) => await Handle(ct));
    }
}
