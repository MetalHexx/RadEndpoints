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
            var result = await s.InsertExample(Map.ToEntity(r));

            return result.Match
            (
                example =>
                {
                    Response = Map.FromEntity(example);
                    Response.Message = "Example created successfully";
                    return Created($"/examples/{example.Id}", Response);
                },
                conflict => Conflict(conflict.Message)
            );
        }
    }
}
