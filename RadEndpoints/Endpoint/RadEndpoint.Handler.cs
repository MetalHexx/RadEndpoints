using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Microsoft.VisualBasic;
using RadEndpoints.Endpoint;
using RadEndpoints.Mediator;
using System.Net.Mime;

namespace RadEndpoints
{
    public abstract partial class RadEndpoint<TRequest, TResponse> : RadEndpoint, IRadEndpoint<TRequest, TResponse>
        where TRequest : RadRequest
        where TResponse : RadResponse, new()
    {
        protected virtual void SendProblem(IRadProblem problem) => HttpContext.Items[RadConstants.Context_Key_RadProblem] = problem;
        protected virtual void Send() => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Ok(Response);
        protected virtual void Send(TResponse responseData) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Ok(responseData);
        protected virtual void SendCreatedAt(string uri, TResponse response) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Created(uri, response);
        protected virtual void SendInternalError(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status500InternalServerError);
        protected virtual void SendExternalError(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status502BadGateway);
        protected virtual void SendExternalTimeout(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status504GatewayTimeout);
        protected virtual void SendValidationError(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status400BadRequest);
        protected virtual void SendConflict(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status409Conflict);
        protected virtual void SendNotFound(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status404NotFound);
        protected virtual void SendUnauthorized(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status401Unauthorized);
        protected virtual void SendForbidden(string title) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Problem(title: title, statusCode: StatusCodes.Status403Forbidden);
        protected virtual void SendBytes(RadResponseBytes response) => HttpContext.Items[RadConstants.Context_Key_Result] = TypedResults.Bytes(response.Bytes, response.ContentType, response.FileDownloadName, response.EnableRangeProcessing, response.LastModified);

        async Task<IResult> IRadEndpoint<TRequest, TResponse>.ExecuteHandler(TRequest request, IRadMediator mediator, HttpContext context, CancellationToken ct)
        {
            await mediator.CallHandlerAsync<TRequest, TResponse>(request, ct);

            context.Items.TryGetValue(RadConstants.Context_Key_Result, out var result);

            if (result is IResult r)
            {
                return r;
            }

            context.Items.TryGetValue(RadConstants.Context_Key_RadProblem, out var problem);

            if (problem is IRadProblem p)
            {
                return GetProblemResult(p);
            }
            throw new RadEndpointException("You must call one of the Send() methods before exiting endpoint Handle() method");
        }

        protected virtual IResult GetProblemResult(IRadProblem problem)
        {
            return problem switch
            {
                ValidationError v => TypedResults.Problem(title: v.Message, statusCode: StatusCodes.Status400BadRequest),
                ConflictError c => TypedResults.Problem(title: c.Message, statusCode: StatusCodes.Status409Conflict),
                NotFoundError n => TypedResults.Problem(title: n.Message, statusCode: StatusCodes.Status404NotFound),
                ForbiddenError f => TypedResults.Problem(title: f.Message, statusCode: StatusCodes.Status403Forbidden),
                InternalError i => TypedResults.Problem(title: i.Message, statusCode: StatusCodes.Status500InternalServerError),
                ExternalError e => TypedResults.Problem(title: e.Message, statusCode: StatusCodes.Status502BadGateway),
                _ => TypedResults.Problem(title: "Unknown error", statusCode: StatusCodes.Status500InternalServerError)
            };
        }
    }
}