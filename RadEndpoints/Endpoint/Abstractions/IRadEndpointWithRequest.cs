using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using RadEndpoints.Mediator;

namespace RadEndpoints
{
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
}