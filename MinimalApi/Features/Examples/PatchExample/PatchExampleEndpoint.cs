using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.PatchExample
{
    public class PatchExampleEndpoint(IExampleService s) : RadEndpoint<PatchExampleRequest, PatchExampleResponse, PatchExampleMapper>
    {
        public override void Configure()
        {            
            Patch("/examples/{id}")
                .Produces<PatchExampleResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status409Conflict)
                .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                .WithDocument(tag: Constants.ExamplesTag, desc: "Patch an example.");
        }

        public async override Task Handle(PatchExampleRequest r, CancellationToken ct)
        {
            var result = await s.PatchExample(r.Id, Map.ToEntity(r));

            result.Switch
            (
                example =>
                {
                    Response = Map.FromEntity(example);
                    Send();
                },
                notFound => SendProblem(notFound),
                conflict => SendProblem(conflict)
            );
        }
    }
}