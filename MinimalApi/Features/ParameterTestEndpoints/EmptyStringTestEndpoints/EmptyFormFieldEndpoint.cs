using RadEndpoints;

namespace MinimalApi.Features.ParameterTestEndpoints.EmptyStringTestEndpoints
{
    /// <summary>
    /// Test endpoint for verifying empty string handling in form data
    /// </summary>
    public class EmptyFormFieldRequest
    {
        [FromRoute]
        public string Id { get; set; } = string.Empty;

        [FromForm]
        public string? RequiredField { get; set; }

        [FromForm]
        public string? OptionalField { get; set; }

        [FromForm]
        public string? Description { get; set; }
    }

    public class EmptyFormFieldRequestValidator : AbstractValidator<EmptyFormFieldRequest>
    {
        public EmptyFormFieldRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required.");

            RuleFor(x => x.RequiredField)
                .NotEmpty()
                .WithMessage("RequiredField cannot be empty.");

            RuleFor(x => x.Description)
                .MaximumLength(100)
                .WithMessage("Description must be 100 characters or less.");
        }
    }

    public class EmptyFormFieldResponse
    {
        public string Id { get; set; } = string.Empty;
        public string? RequiredField { get; set; }
        public string? OptionalField { get; set; }
        public string? Description { get; set; }
        public string Message { get; set; } = "Success";
    }

    public class EmptyFormFieldEndpoint : RadEndpoint<EmptyFormFieldRequest, EmptyFormFieldResponse>
    {
        public override void Configure()
        {
            Post("/test/form-fields/{id}")
                .Accepts<EmptyFormFieldRequest>("multipart/form-data")
                .Produces<EmptyFormFieldResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .DisableAntiforgery()
                .WithTags("ParameterTests")
                .WithDescription("Test endpoint for empty string form field handling");
        }

        public override async Task Handle(EmptyFormFieldRequest r, CancellationToken ct)
        {
            await Task.CompletedTask;
            
            Response = new EmptyFormFieldResponse
            {
                Id = r.Id,
                RequiredField = r.RequiredField,
                OptionalField = r.OptionalField,
                Description = r.Description
            };
            
            Send();
        }
    }
}