using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

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
        void SetRoute(string route);
    }
    public interface IRadEndpointWithMapper<TMapper> where TMapper : IRadMapper
    {
        void SetMapper(TMapper mapper);
    }

    public interface IRadEndpoint<TRequest, TResponse> : IRadEndpoint
        where TRequest : RadRequest
        where TResponse : RadResponse, new()
    {
        TResponse Response { get; set; }
        RouteHandlerBuilder Get(string route);
        RouteHandlerBuilder Post(string route);
        RouteHandlerBuilder Put(string route);
        RouteHandlerBuilder Patch(string route);
        RouteHandlerBuilder Delete(string route);        
        Task<IResult> Handle(TRequest r, CancellationToken ct);
    }
    public interface IRadEndpointWithoutRequest<TResponse> where TResponse : RadResponse, new()
    {
        TResponse Response { get; set; }
        RouteHandlerBuilder Get(string route);
        RouteHandlerBuilder Post(string route);
        RouteHandlerBuilder Put(string route);
        RouteHandlerBuilder Patch(string route);
        RouteHandlerBuilder Delete(string route);
        Task<IResult> Handle(CancellationToken ct);
    }
}
