using RadEndpoints;

namespace MinimalApi.Features.ParameterTestEndpoints.EmptyStringTestEndpoints
{
    /// <summary>
    /// Test endpoint combining all parameter types to verify empty string handling
    /// </summary>
    public class CombinedParameterRequest
    {
        [FromRoute]
        public string Id { get; set; } = string.Empty;

        [FromQuery]
        public string? SearchTerm { get; set; }

        [FromHeader(Name = "X-Client-Id")]
        public string? ClientId { get; set; }

        [FromForm]
        public string? FormData { get; set; }
    }

    public class CombinedParameterRequestValidator : AbstractValidator<CombinedParameterRequest>
    {
        public CombinedParameterRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required.");

            RuleFor(x => x.SearchTerm)
                .NotEmpty()
                .WithMessage("SearchTerm cannot be empty.");

            RuleFor(x => x.ClientId)
                .NotEmpty()
                .WithMessage("X-Client-Id header cannot be empty.");

            RuleFor(x => x.FormData)
                .NotEmpty()
                .WithMessage("FormData cannot be empty.");
        }
    }

    public class CombinedParameterResponse
    {
        public string Id { get; set; } = string.Empty;
        public string? SearchTerm { get; set; }
        public string? ClientId { get; set; }
        public string? FormData { get; set; }
        public string Message { get; set; } = "Success";
        public bool AllParametersReceived { get; set; }
    }

    public class CombinedParameterEndpoint : RadEndpoint<CombinedParameterRequest, CombinedParameterResponse>
    {
        public override void Configure()
        {
            Post("/test/combined-params/{id}")
                .Accepts<CombinedParameterRequest>("multipart/form-data")
                .Produces<CombinedParameterResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .DisableAntiforgery()
                .WithTags("ParameterTests")
                .WithDescription("Test endpoint for combined parameter types with empty string handling");
        }

        public override async Task Handle(CombinedParameterRequest r, CancellationToken ct)
        {
            await Task.CompletedTask;
            
            Response = new CombinedParameterResponse
            {
                Id = r.Id,
                SearchTerm = r.SearchTerm,
                ClientId = r.ClientId,
                FormData = r.FormData,
                AllParametersReceived = !string.IsNullOrEmpty(r.Id) && 
                                      r.SearchTerm != null && 
                                      r.ClientId != null && 
                                      r.FormData != null
            };
            
            Send();
        }
    }
}