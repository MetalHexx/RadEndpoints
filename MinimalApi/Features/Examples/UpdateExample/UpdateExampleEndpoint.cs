using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.UpdateExample
{
    public class UpdateExampleEndpoint : Endpoint<UpdateExampleRequest, UpdateExampleResponse>
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
            Response.Example = (await _service.UpdateExample(r.Example))!;
            Response.Message = "Example updated successfully";

            if(Response.Example is null)
            {
                return NotFound("Could not find and example with the id provided");
            }
            return Ok(Response);
        }
    }
}
