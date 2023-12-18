using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.CustomExamples.CustomPut
{
    /// <summary>
    /// This endpoint shows minimal usage of RadEndpoint abstractions.  The framework stays out of the
    /// way to allow you to use all native minimal API functionality. This will allow for very custom
    /// use cases or scenarios that are not yet supported by the framework.
    /// </summary>
    public class CustomPutEndpoint(ICustomPutMapper m) : RadEndpoint
    {
        public override void Configure()
        {
            var route = SetRoute("/custom-examples/{id}"); //This is optional but recommended for strongly typed "routeless" integration testing.

            RouteBuilder
                .MapPut(route, ([AsParameters]CustomPutRequest r, IExampleService s, CancellationToken ct) => Handle(r, s, ct))
                .AddEndpointFilter<RadValidationFilter<CustomPutRequest>>()
                .Produces<CustomPutResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithDocument
                (
                    tag: "Custom Examples", 
                    desc: "Update an example. \r\n\r\nThis endpoint shows minimal usage of RadEndpoint abstractions.  The framework stays out of the way to allow you to use all native minimal API functionality. This will allow for very custom use cases or scenarios that are not yet supported by the framework."
                );
        }

        public async Task<IResult> Handle(CustomPutRequest r, IExampleService s, CancellationToken ct)
        {
            var example = await s.UpdateExample(m.ToEntity(r));            

            if (example is null)
            {
                return NotFound("Could not find and example with the id provided");
            }
            var response = m.FromEntity(example);
            response.Message = "Example updated successfully";

            return Ok(response);
        }
    }
}
