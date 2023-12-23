namespace MinimalApi.Features.CustomBase._common
{
    public abstract class AwesomeEndpoint<TRequest, TResponse> : RadEndpoint<TRequest, TResponse>
        where TResponse : RadResponse, new()
        where TRequest : RadRequest
    {
        protected override void SendOk() => TypedResults.Ok("This is a different implementation of the Ok helper.");
        protected override void SendNotFound(string message) => TypedResults.NotFound(message);

        //TODO: add more overrides
    }
}
