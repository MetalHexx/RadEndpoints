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

        public async override Task<IResult> Handle(UpdateExampleRequest r, CancellationToken ct)
        {
            var result = await s.UpdateExample(Map.ToEntity(r));

            return result.Match
            (
                example =>
                {
                    var response = Map.FromEntity(example);
                    response.Message = "Example updated successfully";
                    return Ok(response);
                },
                notFound => NotFound(notFound.Message),
                conflict => Conflict(conflict.Message)
            );
        }
    }
}
