using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.UpdateExample
{
    public class UpdateExampleEndpoint : Endpoint<UpdateExampleRequest, UpdateExampleResponse, UpdateExampleMapper>
    {
        private readonly IExampleService _service;
        public UpdateExampleEndpoint(IExampleService service) => _service = service;

        public override void Configure()
        {
            Put("/examples")
                .Produces<UpdateExampleResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .AddSwagger(tag: Constants.ExamplesTag, desc: "Update an example.");
        }

        public async override Task<IResult> Handle(UpdateExampleRequest r, CancellationToken ct)
        {
            var example = await _service.UpdateExample(Map.ToEntity(r));

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
