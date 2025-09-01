using RadEndpoints;

namespace MinimalApi.Features.ParameterTestEndpoints.EmptyStringTestEndpoints
{
    /// <summary>
    /// Test endpoint for verifying empty string handling in query parameters
    /// </summary>
    public class EmptyQueryParameterRequest
    {
        [FromRoute]
        public string Id { get; set; } = string.Empty;

        [FromQuery]
        public string? RequiredParam { get; set; }

        [FromQuery] 
        public string? OptionalParam { get; set; }
    }

    public class EmptyQueryParameterRequestValidator : AbstractValidator<EmptyQueryParameterRequest>
    {
        public EmptyQueryParameterRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required.");

            RuleFor(x => x.RequiredParam)
                .NotEmpty()
                .WithMessage("RequiredParam cannot be empty.");
        }
    }

    public class EmptyQueryParameterResponse
    {
        public string Id { get; set; } = string.Empty;
        public string? RequiredParam { get; set; }
        public string? OptionalParam { get; set; }
        public string Message { get; set; } = "Success";
    }

    public class EmptyQueryParameterEndpoint : RadEndpoint<EmptyQueryParameterRequest, EmptyQueryParameterResponse>
    {
        public override void Configure()
        {
            Get("/test/query-params/{id}")
                .Produces<EmptyQueryParameterResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithTags("ParameterTests")
                .WithDescription("Test endpoint for empty string query parameter handling");
        }

        public override async Task Handle(EmptyQueryParameterRequest r, CancellationToken ct)
        {
            await Task.CompletedTask;
            
            Response = new EmptyQueryParameterResponse
            {
                Id = r.Id,
                RequiredParam = r.RequiredParam,
                OptionalParam = r.OptionalParam
            };
            
            Send();
        }
    }
}