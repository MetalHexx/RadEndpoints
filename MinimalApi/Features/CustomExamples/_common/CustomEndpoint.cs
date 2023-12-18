using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;

namespace MinimalApi.Features.CustomExamples._common
{
    public abstract class CustomEndpoint<TRequest, TResponse> : RadEndpoint<TRequest, TResponse>
        where TResponse : RadResponse, new()
        where TRequest : RadRequest
    {
        protected override Ok<string> Ok() => TypedResults.Ok("This is a different implementation of the Ok helper.");
        protected override NotFound<string> NotFound(string message) => TypedResults.NotFound(message);
    }
}
