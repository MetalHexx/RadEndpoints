using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Lite.LightPut
{
    /// <summary>
    /// This endpoint shows minimal usage of the RadEndpoint abstractions without strong typing.
    /// This will remove a lot of framework conveniences and shortcuts, but will allow for full control 
    /// over the endpoint behavior.
    /// 
    /// Features you still get:
    /// - Assembly scanned endpoint configuration
    /// - Endpoint style class structure
    /// - Endpoint class dependency injection with scoped lifetime
    /// - Endpoint class property conveniences like ILogger, IWebHostEnvironment, IEndpointRouteBuilder, and IHttpContextAccessor
    /// - RadEndpoint RouteBuilder shortcuts
    /// - Optional Routeless integration testing (manually configured - as shown)
    /// - ValidationFilter (manual configuration - as shown)
    /// 
    /// Drawbacks (or benefits?) of not using strong typed RadEndpoint<,,>
    /// - Not available: Global endpoint configuration (for filters and other RouteBuilder functions)
    /// - Not available: Declarative and strongly enforced typing (You could still use Net 8 Union / TypedResults if you prefer...)
    /// - Not available: Send() and Send(TResponse) shortcuts
    /// - Not available: Automatic RadProblem Results (see: RadEndpoint.Send.SendProblem())
    /// - Not available: Assembly scanned Endpoint Model Mappers
    /// - Not available: Assembly scanned  FluentValidators (manual configuration - as shown)
    /// - More verbose (net 8 / out-of-the-box) endpoint configuration
    /// - Loose opinion means you need to find other ways to add architectural guardrails, and automation to your application
    /// 
    /// This approach is only really recommended for an application that has strong
    /// team coding standards and facilities to guardrail the application. You can
    /// combine this approach with the RadEndpoint<,,> but it is recommended to 
    /// only use it sparingly for custom or hard-to-reach use cases.  
    /// 
    /// </summary>
    public class LitePutEndpoint(ICustomPutMapper m) : RadEndpoint
    {
        public override void Configure()
        {
            var route = SetRoute("/lite/{id}"); //This is optional but recommended for strongly typed "routeless" integration testing.

            RouteBuilder
                .MapPut(route, ([AsParameters]CustomPutRequest r, IExampleService s, CancellationToken ct) => Handle(r, s, ct))
                .AddEndpointFilter<RadValidationFilter<CustomPutRequest>>()
                .Produces<CustomPutResponse>(StatusCodes.Status200OK)                
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status409Conflict)
                .ProducesValidationProblem(StatusCodes.Status400BadRequest)                
                .WithDocument
                (
                    tag: "Lite", 
                    desc: "This endpoint shows minimal or (lite) usage of RadEndpoint abstractions.  It's quite a bit more verbose which makes it a bit ironic.  Using the non-generic RadEndpoint type will provide the basic features for configuring your minimal endpoint.  You will have full control over the endpoint like a normal minimal api endpoint, but lose some framework shortcuts and conveniences.  See comments on endpoint for more information."
                );
        }

        public async Task<IResult> Handle(CustomPutRequest r, IExampleService s, CancellationToken ct)
        {
            var result = await s.UpdateExample(m.ToEntity(r));

            return result.Match
            (
                example =>
                {
                    var response = m.FromEntity(example);
                    response.Message = "Example updated successfully";
                    return Results.Ok(response);
                },
                notFound => Results.Problem(title: notFound.Message, statusCode: StatusCodes.Status404NotFound),
                conflict => Results.Problem(title: conflict.Message, statusCode: StatusCodes.Status409Conflict)
            );
        }
    }
}
