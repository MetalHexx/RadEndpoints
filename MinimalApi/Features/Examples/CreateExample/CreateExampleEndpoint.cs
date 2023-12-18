using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.CreateExample
{
    public class CreateExampleEndpoint(IExampleService s) : RadEndpoint<CreateExampleRequest, CreateExampleResponse, CreateExampleMapper>
    {
        public override void Configure()
        {
            Post("/examples")
                .Produces<CreateExampleResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status409Conflict)
                .WithDocument(tag: Constants.ExamplesTag, desc: "Create a new example.");
        }

        public override async Task<IResult> Handle(CreateExampleRequest r, CancellationToken ct)
        {
            var entity = Map.ToEntity(r);
            var savedEntity = await s.InsertExample(entity);

            if (savedEntity is null)
            {
                return Conflict("An example with the same first and last name already exists");
            }
            Response = Map.FromEntity(savedEntity!);
            Response.Message = "Example created successfully";
            
            return Created($"/examples/{savedEntity.Id}", Response);
        }
    }
}
