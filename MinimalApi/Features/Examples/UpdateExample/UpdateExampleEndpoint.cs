using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.UpdateExample
{
    public class UpdateExampleEndpoint(IExampleService s) : RadEndpoint<UpdateExampleRequest, UpdateExampleResponse, UpdateExampleMapper>
    {
        public override void Configure()
        {
            Put("/examples")
                .Produces<UpdateExampleResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .AddSwagger(tag: Constants.ExamplesTag, desc: "Update an example.");
        }

        public async override Task<IResult> Handle(UpdateExampleRequest r, CancellationToken ct)
        {
            var example = await s.UpdateExample(Map.ToEntity(r));

            if (example is null)
            {
                return NotFound("Could not find and example with the id provided");
            }
            Response = Map.FromEntity(example);
            Response.Message = "Example updated successfully";
            
            return Ok(Response);
        }
    }
}
