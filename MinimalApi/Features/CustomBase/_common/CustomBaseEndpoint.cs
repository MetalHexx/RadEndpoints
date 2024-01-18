using System.Net;

namespace MinimalApi.Features.CustomBase._common
{
    public abstract class CustomBaseEndpoint<TRequest, TResponse> : RadEndpoint
        where TRequest : CustomBaseRequest
        where TResponse : CustomBaseResponse, new()
    {
        public abstract Task<TResponse> Handle(TRequest r, CancellationToken ct);

        public RouteHandlerBuilder Get(string route)
        {
            SetRoute(route);
            return RouteBuilder!.MapGet(route, async ([AsParameters] TRequest r, CancellationToken ct) => await Handle(r, ct));            
        }
        public TResponse BadRequest(string message)
        {
            return new TResponse 
            {
                Message = message,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        //Add other helpers
    }
}
