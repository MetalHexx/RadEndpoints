using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.CustomExamples.CustomPut
{
    public class CustomPutEndpoint(IExampleService s, ICustomPutMapper m) : RadEndpoint
    {
        public override void Configure()
        {
            RouteBuilder
                .MapPut("/custom-examples/{id}", (int id, CustomPutRequest r, CancellationToken ct) => Handle(r, id, ct))
                .Produces<CustomPutResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .AddSwagger(tag: "Custom Examples", desc: "Update an example.");
        }

        public async Task<IResult> Handle(CustomPutRequest r, int id, CancellationToken ct)
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
