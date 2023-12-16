using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadEndpoints.Abstractions
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

    public interface IRadEndpoint<TRequest, TResponse> : IRadEndpoint
        where TRequest : RadRequest
        where TResponse : RadResponse, new()
    {
        TResponse Response { get; set; }

        RouteHandlerBuilder Delete(string route);
        RouteHandlerBuilder Get(string route);
        Task<IResult> Handle(TRequest r, CancellationToken ct);
        RouteHandlerBuilder Patch(string route);
        RouteHandlerBuilder Post(string route);
        RouteHandlerBuilder Put(string route);
    }

    public interface IRadEndpointWithMapper<TMapper> where TMapper : IRadMapper
    {
        void SetMapper(TMapper mapper);
    }

    public interface IRadEndpointWithoutRequest<TResponse> where TResponse : RadResponse, new()
    {
        TResponse Response { get; set; }

        RouteHandlerBuilder Delete(string route);
        RouteHandlerBuilder Get(string route);
        Task<IResult> Handle(CancellationToken ct);
        RouteHandlerBuilder Patch(string route);
        RouteHandlerBuilder Post(string route);
        RouteHandlerBuilder Put(string route);
    }
}
