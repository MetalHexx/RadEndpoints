﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RadEndpoints.Mediator;
using System.Diagnostics.CodeAnalysis;

namespace RadEndpoints
{
    public abstract class RadEndpointWithoutRequest<TResponse> : RadEndpoint, IRadEndpointWithoutRequest<TResponse>
        where TResponse : new()
    {
        public TResponse Response { get; set; } = new();
        public abstract Task Handle(CancellationToken ct);

        protected virtual void SendProblem(IRadProblem problem) => HttpContext.Items[RadConstants.Context_Key_RadProblem] = problem;
        protected virtual void Send() => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Ok(Response);
        protected virtual void Send(TResponse responseData) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Ok(responseData);
        protected virtual void SendCreatedAt(string uri) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Created(uri, Response);
        protected virtual void SendCreatedAt(string uri, TResponse response) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Created(uri, response);
        protected virtual void SendRedirect([StringSyntax("Uri")] string url, bool permanent = false, bool preserveMethod = false) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Redirect(url, permanent, preserveMethod);
        protected virtual void SendInternalError(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status500InternalServerError);
        protected virtual void SendExternalError(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status502BadGateway);
        protected virtual void SendExternalTimeout(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status504GatewayTimeout);
        protected virtual void SendValidationError(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status400BadRequest);
        protected virtual void SendConflict(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status409Conflict);
        protected virtual void SendNotFound(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status404NotFound);
        protected virtual void SendUnauthorized(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status401Unauthorized);
        protected virtual void SendForbidden(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status403Forbidden);
        protected virtual void SendBytes(RadBytes response) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Bytes(response.Bytes, response.ContentType, response.FileDownloadName, response.EnableRangeProcessing, response.LastModified);
        protected virtual void SendStream(RadStream response) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Stream(response.Stream, response.ContentType, response.FileDownloadName, response.LastModified, response.EntityTag, response.EnableRangeProcessing);
        protected virtual void SendFile(RadFile response) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.PhysicalFile(response.Path, response.ContentType, response.FileDownloadName, response.LastModified, response.EntityTag);

        async Task<IResult> IRadEndpointWithoutRequest<TResponse>.ExecuteHandler(IRadMediator mediator, HttpContext context, CancellationToken ct)
        {
            await mediator.CallHandler<TResponse>(GetType(), ct);

            context.Items.TryGetValue(RadConstants.Context_Key_Result, out var result);

            if (result is IResult r) return r;

            context.Items.TryGetValue(RadConstants.Context_Key_RadProblem, out var problem);

            if (problem is IRadProblem p) return GetProblemResult(p);

            throw new RadEndpointException("You must call one of the Send() methods before exiting endpoint Handle() method");
        }

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