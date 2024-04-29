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
                .ProducesValidationProblem()
                .WithDocument(tag: Constants.ExamplesTag, desc: "Create a new example.");
        }

        public override async Task Handle(CreateExampleRequest r, CancellationToken ct)
        {
            var result = await s.InsertExample(Map.ToEntity(r));

            result.Switch
            (
                example =>
                {
                    Response = Map.FromEntity(example);
                    Response.Message = "Example created successfully";
                    SendCreatedAt($"/examples/{example.Id}");
                },
                conflict => SendProblem(conflict)
            );
        }
    }
}
