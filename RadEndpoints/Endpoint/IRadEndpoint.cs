using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using RadEndpoints.Mediator;

namespace RadEndpoints
{
    public interface IRadEndpoint
    {
        void Configure();
        void EnableValidation();
        void SetBuilder(IEndpointRouteBuilder routeBuilder);
        void SetContext(IHttpContextAccessor contextAccessor);
        void SetEnvironment(IWebHostEnvironment env);
        void SetLogger(ILogger logger);
        T Service<T>() where T : notnull;
    }

    public interface IRadEndpointWithoutRequest<TResponse> : IRadEndpoint
        where TResponse : new()
    {
        TResponse Response { get; set; }
        RouteHandlerBuilder Get(string route);
        RouteHandlerBuilder Post(string route);
        RouteHandlerBuilder Put(string route);
        RouteHandlerBuilder Patch(string route);
        RouteHandlerBuilder Delete(string route);
        Task Handle(CancellationToken ct);
        Task<IResult> ExecuteHandler(IRadMediator mediator, HttpContext context, CancellationToken ct);
    }

    public interface IRadEndpoint<TRequest, TResponse> : IRadEndpoint
        where TRequest : class
        where TResponse : new()
    {
        TResponse Response { get; set; }
        RouteHandlerBuilder Get(string route);
        RouteHandlerBuilder Post(string route);
        RouteHandlerBuilder Put(string route);
        RouteHandlerBuilder Patch(string route);
        RouteHandlerBuilder Delete(string route);        
        Task Handle(TRequest r, CancellationToken ct);
        Task<IResult> ExecuteHandler(TRequest request, IRadMediator mediator, HttpContext context, CancellationToken ct);
    }
    public interface IRadEndpointWithMapper
    {
        void SetMapper(IRadMapper mapper);
    }
}
