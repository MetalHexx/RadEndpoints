using MinimalApi.Domain.Examples;
using MinimalApi.Features.Examples.UpdateExample;

namespace MinimalApi.Features.Pure.UpdateExample
{
    /// <summary>
    /// This shows an example done in pure minimal api with a static class. 
    /// This is identical to UpdatedExampleEndpoint and LitePutEndpoint. 
    /// </summary>
    public static class PureEndpoints
    {
        public static void MapPureEndpoints(this WebApplication app)
        {
            app.MapPut("/pure/{id}", async ([AsParameters] UpdateExampleRequest r, IExampleService s, IValidator<UpdateExampleRequest> v, CancellationToken ct) =>
            {
                var validationResult = v.Validate(r);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray());

                    return Results.ValidationProblem(errors);
                }

                var entity = new Example(r.Data.FirstName, r.Data.LastName, r.Id);

                var result = await s.UpdateExample(entity);

                return result.Match
                (
                    example =>
                    {
                        var response = new UpdateExampleResponse
                        {
                            Data = new()
                            {
                                Id = example.Id,
                                FirstName = example.FirstName,
                                LastName = example.LastName
                            },
                            Message = "Example updated successfully"
                        };
                        return Results.Ok(response);
                    },
                    notFound => Results.NotFound(notFound.Message),
                    conflict => Results.Conflict(conflict.Message)
                );
            })
            .Produces<UpdateExampleResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest)
            .WithDocument(tag: "Pure Minimal API", desc: "This shows an example done in pure minimal api.  This is identical to UpdatedExampleEndpoint and CustomPutEndpoint but less structured.");
        }
    }
}
