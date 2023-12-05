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
                .Produces<CreateExampleResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status201Created)
                .AddSwagger(tag: Constants.ExamplesTag, desc: "Create a new example.");
        }

        public override async Task<IResult> Handle(CreateExampleRequest r, CancellationToken ct)
        {
            Response.Example = (await _service.InsertExample(r.Example))!;
            Response.Message = "Example created successfully";

            if(Response.Example is null)
            {
                return Conflict("An example with the same first and last name already exists");
            }
            return Created($"/examples/{Response.Example.Id}", Response);
        }
    }
}
