using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.GetExample
{
    public class GetExampleEndpoint() : RadEndpoint<GetExampleRequest, GetExampleResponse, GetExampleMapper>
    {
        public override void Configure()
        {
            Get("/examples/{id}")
                .Produces<GetExampleResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithDocument(tag: Constants.ExamplesTag, desc: "Get an example by id");
        }

        public async override Task<IResult> Handle(GetExampleRequest r, CancellationToken ct)
        {
            var exampleEntity = await Service<IExampleService>().GetExample(r.Id);

            if (exampleEntity is null)
            {
                return NotFound("Example not found");
            }
            Response = Map.FromEntity(exampleEntity);
            Response.Message = "Example retrieved successfully";

            return Ok(Response);
        }
    }
}
