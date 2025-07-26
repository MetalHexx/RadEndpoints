using Microsoft.AspNetCore.Http.HttpResults;
using MinimalApi.Domain.Examples;
using RadEndpoints.Validation;

namespace MinimalApi.Features.WithTypedResults.TypedResultsPut
{
    /// <summary>
    /// This endpoint shows minimal usage of the RadEndpoint abstractions.
    /// This will remove a lot of framework conveniences and shortcuts, 
    /// but will allow for full control over the endpoint behavior.  In this 
    /// example we show usage of Minimal API strongly typed union results.
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
    /// Drawbacks (or benefits?) of not using generic typed RadEndpoint<,,>
    /// - Not available: Global endpoint configuration (for filters and other RouteBuilder functions)
    /// - Not available: Declarative and strongly enforced typing (You could still use Net 8 Union / TypedResults as demonstrated here)
    /// - Not available: Send() and Send(TResponse) shortcuts -- but you can make your own.  See: /CustomBase
    /// - Not available: Automatic RadProblem Results (see: RadEndpoint.Send.SendProblem())
    /// - Not available: Built-in endpoint mappers
    /// - Not available: Automatic FluentValidators (manual configuration - as shown)
    /// - More verbose (net 8 / out-of-the-box) endpoint configuration -- but you can make your own.  See: /CustomBase
    /// - Loose opinion means you need to find other ways to add architectural guardrails, and automation to your application
    /// 
    /// This approach is only really recommended for an application that has strong
    /// team coding standards and facilities to guardrail the application. You can
    /// combine this approach with other endpoints that inherit from RadEndpoint<,,>.  
    /// 
    /// Also consider making your own base endpoint class for customization See: /CustomBase
    /// 
    /// </summary>
    public class TypedResultsPutEndpoint(ICustomPutMapper m, IExampleService s) : RadEndpoint
    {
        public override void Configure()
        {
            var route = SetRoute("/typed-results/{id}"); //This is optional but recommended for strongly typed "routeless" integration testing.

            RouteBuilder
                .MapPut(route, ([AsParameters]CustomPutRequest r) => Handle(r, s))
                .WithRadValidation<CustomPutRequest>()            
                .WithDocument
                (
                    tag: "Minimal Api Style Typed Results", 
                    desc: "This endpoint shows the flexibility of the RadEndpoint base class allowing the use of Minimal API Strongly Typed Union results.  Using the non-generic RadEndpoint type will provide the basic features for configuring your minimal endpoint.  You will have full control over the endpoint like a normal minimal api endpoint, but lose some framework shortcuts and conveniences.  See comments on endpoint for more information."
                );
        }

        public async Task<Results<
            Ok<CustomPutResponse>, 
            NotFound<ProblemDetails>, 
            Conflict<ProblemDetails>, 
            BadRequest<ValidationProblemDetails>>> 
        Handle(CustomPutRequest r, IExampleService s)
        {
            var result = await s.UpdateExample(m.ToEntity(r));

            return result.Match<Results<Ok<CustomPutResponse>, NotFound<ProblemDetails>, Conflict<ProblemDetails>, BadRequest<ValidationProblemDetails>>>
            (
                example =>
                {
                    var response = m.FromEntity(example);
                    response.Message = "Example updated successfully";
                    return Microsoft.AspNetCore.Http.TypedResults.Ok(response);
                },
                notFound => Microsoft.AspNetCore.Http.TypedResults.NotFound(new ProblemDetails
                {
                    Title = notFound.Message
                }),
                conflict => Microsoft.AspNetCore.Http.TypedResults.Conflict(new ProblemDetails
                { 
                    Title = conflict.Message 
                })
            );
        }
    }
}
