using Microsoft.AspNetCore.Http;
using MinimalApi.Domain.Examples;
using MinimalApi.Features.CustomExamples.CustomPut;
using MinimalApi.Features.Examples.UpdateExample;
using System.ComponentModel.DataAnnotations;

namespace MinimalApi.Features.Pure.UpdateExample
{
    /// <summary>
    /// This shows an example done in pure minimal api with a static class. 
    /// This is identical to UpdatedExampleEndpoint and CustomPutEndpoint. 
    /// RadEndpoints looks a bit cleaner 
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

                var example = await s.UpdateExample(entity);

                if (example is null)
                {
                    return Results.Problem(title: "Example was not found", statusCode: StatusCodes.Status404NotFound);
                }
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
            })
            .Produces<UpdateExampleResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithDocument(tag: "Pure Minimal API", desc: "Update an example. \r\n\r\n This shows an example done in pure minimal api.  This is identical to UpdatedExampleEndpoint and CustomPutEndpoint.");
        }
    }
}
