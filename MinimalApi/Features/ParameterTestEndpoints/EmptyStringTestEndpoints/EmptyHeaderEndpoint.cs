using RadEndpoints;

namespace MinimalApi.Features.ParameterTestEndpoints.EmptyStringTestEndpoints
{
    /// <summary>
    /// Test endpoint for verifying empty string handling in headers
    /// </summary>
    public class EmptyHeaderRequest
    {
        [FromRoute]
        public string Id { get; set; } = string.Empty;

        [FromHeader(Name = "X-Required-Header")]
        public string? RequiredHeader { get; set; }

        [FromHeader(Name = "X-Optional-Header")]
        public string? OptionalHeader { get; set; }

        [FromHeader(Name = "Authorization")]
        public string? AuthHeader { get; set; }
    }

    public class EmptyHeaderRequestValidator : AbstractValidator<EmptyHeaderRequest>
    {
        public EmptyHeaderRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required.");

            RuleFor(x => x.RequiredHeader)
                .NotEmpty()
                .WithMessage("X-Required-Header cannot be empty.");

            RuleFor(x => x.AuthHeader)
                .NotEmpty()
                .WithMessage("Authorization header is required.");
        }
    }

    public class EmptyHeaderResponse
    {
        public string Id { get; set; } = string.Empty;
        public string? RequiredHeader { get; set; }
        public string? OptionalHeader { get; set; }
        public string? AuthHeader { get; set; }
        public string Message { get; set; } = "Success";
    }

    public class EmptyHeaderEndpoint : RadEndpoint<EmptyHeaderRequest, EmptyHeaderResponse>
    {
        public override void Configure()
        {
            Post("/test/headers/{id}")
                .Produces<EmptyHeaderResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithTags("ParameterTests")
                .WithDescription("Test endpoint for empty string header handling");
        }

        public override async Task Handle(EmptyHeaderRequest r, CancellationToken ct)
        {
            await Task.CompletedTask;
            
            Response = new EmptyHeaderResponse
            {
                Id = r.Id,
                RequiredHeader = r.RequiredHeader,
                OptionalHeader = r.OptionalHeader,
                AuthHeader = r.AuthHeader
            };
            
            Send();
        }
    }
}