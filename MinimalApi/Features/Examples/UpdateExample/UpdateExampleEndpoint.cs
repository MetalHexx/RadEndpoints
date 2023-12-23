using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.UpdateExample
{
    public class UpdateExampleEndpoint(IExampleService s) : RadEndpoint<UpdateExampleRequest, UpdateExampleResponse, UpdateExampleMapper>
    {
        public override void Configure()
        {            
            Put("/examples/{id}")
                .Produces<UpdateExampleResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status409Conflict)
                .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                .WithDocument(tag: Constants.ExamplesTag, desc: "Update an example.");
        }

        public async override Task Handle(UpdateExampleRequest r, CancellationToken ct)
        {
            var result = await s.UpdateExample(Map.ToEntity(r));

            result.Switch
            (
                example =>
                {
                    Response = Map.FromEntity(example);
                    Response.Message = "Example updated successfully";
                    Send();
                },
                notFound => SendProblem(notFound),
                conflict => SendProblem(conflict)
            );
        }
    }
}