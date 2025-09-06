using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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
            if (!HasValidator) return builder;
            
            builder.AddEndpointFilter(async (ctx, next) =>
            {
                var request = ctx.Arguments.OfType<TRequest>().FirstOrDefault();

                if (request is null)
                {
                    return TypedResults.Problem
                    (
                        title: "Request body cannot be null", 
                        statusCode: StatusCodes.Status400BadRequest
                    );
                }
                var validator = ctx.HttpContext.RequestServices.GetService<IValidator<TRequest>>();
                
                if (validator is null)
                {
                    return TypedResults.Problem($"No validator registered for {typeof(TRequest).Name}", statusCode: 500);
                }

                var result = await validator.ValidateAsync(request);

                if (!result.IsValid)
                {
                    return TypedResults.ValidationProblem(result.ToDictionary(), title: "Validation Error");
                }

                return await next(ctx);
            });
            return builder;
        }

        public virtual void SendProblem(ProblemHttpResult problem) => HttpContext.Items[RadConstants.Context_Key_RadProblem] = problem;
        public virtual void SendProblem(ValidationProblem problem) => HttpContext.Items[RadConstants.Context_Key_RadProblem] = problem;
        public virtual void SendProblem(IRadProblem problem) => HttpContext.Items[RadConstants.Context_Key_RadProblem] = problem;
        public virtual void Send() => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Ok(Response);
        public virtual void Send(TResponse responseData)
        {
            Response = responseData;
            HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Ok(responseData);
        }
        public virtual void SendCreatedAt(string uri) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Created(uri, Response);
        public virtual void SendCreatedAt(string uri, TResponse response) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Created(uri, response);
        public virtual void SendRedirect([StringSyntax("Uri")] string url, bool permanent = false, bool preserveMethod = false) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Redirect(url, permanent, preserveMethod);
        public virtual void SendInternalError(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status500InternalServerError);
        public virtual void SendExternalError(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status502BadGateway);
        public virtual void SendExternalTimeout(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status504GatewayTimeout);
        public virtual void SendValidationError(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.ValidationProblem(new Dictionary<string, string[]> { { "ValidationError", new[] { title } } }, title: title);
        public virtual void SendConflict(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Conflict(title);
        public virtual void SendNotFound(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.NotFound(title);
        public virtual void SendUnauthorized(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status401Unauthorized);
        public virtual void SendForbidden(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status403Forbidden);
        public virtual void SendUnauthorized() => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Unauthorized();
        public virtual void SendForbidden() => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Forbid();
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

            if(problem is ProblemHttpResult problemResult) return problemResult;

            if(problem is ValidationProblem validationProblem) return validationProblem;

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
