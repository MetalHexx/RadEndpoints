﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RadEndpoints.Mediator;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace RadEndpoints
{
    public abstract class RadEndpoint<TRequest, TResponse> : RadEndpoint, IRadEndpoint<TRequest, TResponse>
        where TRequest : class
        where TResponse : new()
    {
        public TResponse? Response { get; set; }
        public abstract Task Handle(TRequest r, CancellationToken ct);

        public RouteHandlerBuilder Get(string route)
        {
            SetRoute(route);
            var builder = RouteBuilder!.MapGet(route, async ([AsParameters] TRequest r, IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(r, m, c, ct));
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Post(string route)
        {
            RouteHandlerBuilder builder;

            if (HasRequestModelAttributes())
            {
                builder = RouteBuilder!.MapPost(route, async ([AsParameters] TRequest r, IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(r, m, c, ct));
            }
            else
            {
                builder = RouteBuilder!.MapPost(route, async (TRequest r, IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(r, m, c, ct));
            }
            SetRoute(route);
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Put(string route)
        {
            RouteHandlerBuilder builder;

            if (HasRequestModelAttributes())
            {
                builder = RouteBuilder!.MapPut(route, async ([AsParameters] TRequest r, IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(r, m, c, ct));
            }
            else
            {
                builder = RouteBuilder!.MapPut(route, async (TRequest r, IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(r, m, c, ct));
            }
            SetRoute(route);
            return TryAddEndpointFilter(builder);
        }

        public RouteHandlerBuilder Patch(string route)
        {
            RouteHandlerBuilder builder;

            if(HasRequestModelAttributes())
            {
                builder = RouteBuilder!.MapPatch(route, async ([AsParameters] TRequest r, IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(r, m, c, ct));
            }
            else
            {
                builder = RouteBuilder!.MapPatch(route, async (TRequest r, IRadMediator m, HttpContext c, CancellationToken ct) => await SelfInterface.ExecuteHandler(r, m, c, ct));
            }
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

        public virtual void SendProblem(IRadProblem problem) => HttpContext.Items[RadConstants.Context_Key_RadProblem] = problem;
        public virtual void Send() => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Ok(Response);
        public virtual void Send(TResponse responseData) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Ok(responseData);
        public virtual void SendCreatedAt(string uri) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Created(uri, Response);
        public virtual void SendCreatedAt(string uri, TResponse response) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Created(uri, response);
        public virtual void SendRedirect([StringSyntax("Uri")] string url, bool permanent = false, bool preserveMethod = false) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Redirect(url, permanent, preserveMethod);
        public virtual void SendInternalError(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status500InternalServerError);
        public virtual void SendExternalError(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status502BadGateway);
        public virtual void SendExternalTimeout(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status504GatewayTimeout);
        public virtual void SendValidationError(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status400BadRequest);
        public virtual void SendConflict(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status409Conflict);
        public virtual void SendNotFound(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status404NotFound);
        public virtual void SendUnauthorized(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status401Unauthorized);
        public virtual void SendForbidden(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status403Forbidden);
        public virtual void SendBytes(RadBytes response) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Bytes(response.Bytes, response.ContentType, response.FileDownloadName, response.EnableRangeProcessing, response.LastModified);
        public virtual void SendStream(RadStream response) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Stream(response.Stream, response.ContentType, response.FileDownloadName, response.LastModified, response.EntityTag, response.EnableRangeProcessing);
        public virtual void SendFile(RadFile response) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.PhysicalFile(response.Path, response.ContentType, response.FileDownloadName, response.LastModified, response.EntityTag);

        async Task<IResult> IRadEndpoint<TRequest, TResponse>.ExecuteHandler(TRequest request, IRadMediator mediator, HttpContext context, CancellationToken ct)
        {
            await mediator.CallHandler<TRequest, TResponse>(GetType(), request, ct);

            context.Items.TryGetValue(RadConstants.Context_Key_Result, out var result);

            if (result is IResult r) return r;

            context.Items.TryGetValue(RadConstants.Context_Key_RadProblem, out var problem);

            if (problem is IRadProblem p) return GetProblemResult(p);

            throw new RadEndpointException("You must call one of the Send() methods before exiting endpoint Handle() method");
        }
        private static bool HasRequestModelAttributes()
        {
            return typeof(TRequest)
                .GetProperties()
                .SelectMany(property => property.GetCustomAttributes())
                .Any(attribute => attribute is FromRouteAttribute ||
                                  attribute is FromQueryAttribute ||
                                  attribute is FromHeaderAttribute ||
                                  attribute is FromFormAttribute ||
                                  attribute is FromBodyAttribute);
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
}
