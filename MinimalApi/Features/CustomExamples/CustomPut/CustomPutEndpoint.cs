using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.CustomExamples.CustomPut
{
    public class CustomPutEndpoint(IExampleService Service, ICustomPutMapper Map) : RadEndpoint
    {
        public override void Configure()
        {
            RouteBuilder
                .MapPut("/custom-examples/{id}", (int id, CustomPutRequest r, CancellationToken ct) => Handle(r, id, ct))
                .Produces<CustomPutResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .AddSwagger(tag: "Custom Examples", desc: "Update an example.");
        }

        public async Task<IResult> Handle(CustomPutRequest r, int id, CancellationToken ct)
        {   
            var example = await Service.UpdateExample(Map.ToEntity(r, id));            

            if (example is null)
            {
                return NotFound("Could not find and example with the id provided");
            }
            var response = Map.FromEntity(example);
            response.Message = "Example updated successfully";

            return Ok(response);
        }
    }
}
