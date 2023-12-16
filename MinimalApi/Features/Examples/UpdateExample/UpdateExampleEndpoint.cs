using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.UpdateExample
{
    public class UpdateExampleEndpoint() : RadEndpoint<UpdateExampleRequest, UpdateExampleResponse, UpdateExampleMapper>
    {
        public override void Configure()
        {
            Put("/examples/{id}")
                .Produces<UpdateExampleResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithDocument(tag: Constants.ExamplesTag, desc: "Update an example.");
        }

        public async override Task<IResult> Handle(UpdateExampleRequest r, CancellationToken ct)
        {
            var example = await Service<IExampleService>().UpdateExample(Map.ToEntity(r));

            if (example is null)
            {
                return NotFound("Example not found");
            }
            Response = Map.FromEntity(example);
            Response.Message = "Example updated successfully";
            
            return Ok(Response);
        }
    }
}
