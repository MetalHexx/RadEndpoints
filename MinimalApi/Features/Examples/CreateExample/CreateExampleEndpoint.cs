using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.CreateExample
{
    public class CreateExampleEndpoint : Endpoint<CreateExampleRequest, CreateExampleResponse, CreateExampleMapper>
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
            var entity = Map.ToEntity(r);
            var savedEntity = await _service.InsertExample(entity);

            if (savedEntity is null)
            {
                return Conflict("An example with the same first and last name already exists");
            }
            Response = Map.FromEntity(savedEntity!);
            Response.Message = "Example created successfully";
            
            return Created($"/examples/{Response.Id}", Response);
        }
    }
}
