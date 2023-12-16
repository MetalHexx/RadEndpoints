using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.CustomExamples.CustomPut
{
    /// <summary>
    /// This endpoint is an example of how one can use all the features of MinimalApi endpoint mapping 
    /// to create a custom endpoint with none of the restrictions of the more convenient generic 
    /// RadEndpoint types.
    /// 
    /// 
    /// </summary>
    public class CustomPutEndpoint(ICustomPutMapper m) : RadEndpoint
    {
        public override void Configure()
        {
            RouteBuilder
                .MapPut("/custom-examples/{id}", (int id, CustomPutRequest r, IExampleService s, CancellationToken ct) => Handle(r, id, s, ct))
                .AddEndpointFilter<RadValidationFilter<CustomPutRequest>>()
                .Produces<CustomPutResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .WithDocument(tag: "Custom Examples", desc: "Update an example.");
        }

        public async Task<IResult> Handle(CustomPutRequest r, int id, IExampleService s, CancellationToken ct)
        {
            var example = await s.UpdateExample(m.ToEntity(r, id));            

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
