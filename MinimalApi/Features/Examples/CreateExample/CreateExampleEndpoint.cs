using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.CreateExample
{
    public class CreateExampleEndpoint : Endpoint<CreateExampleRequest, CreateExampleResponse>
    {
        private readonly IExampleService _service;

        public CreateExampleEndpoint(IExampleService service) => _service = service;

        public override void Configure()
        {
            Post("/examples")

                .AddEndpointFilter<ValidationFilter<CreateExampleRequest>>()
                .Produces<CreateExampleResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status409Conflict)

                .WithTags(ExamplesConstants.ExamplesTag)
                .WithDescription("Create a new example.")
                .WithOpenApi();
        }

        public override async Task<IResult> Handle(CreateExampleRequest r, CancellationToken ct)
        {
            Response.Example = await _service.InsertExample(r.Example);
            Response.Message = "Example created successfully";

            if(Response.Example is null)
            {
                return Conflict("An example with the same first and last name already exists");
            }
            return Ok(Response);
        }
    }
}
