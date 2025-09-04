namespace MinimalApi.Features.ParameterTests.EmptyStringTests
{
    /// <summary>
    /// Test endpoint for verifying custom JSON serialization in RadEndpoints.Testing
    /// </summary>
    public enum TestEnumValue
    {
        FirstOption,
        SecondOption,
        ThirdOption
    }

    public class CustomJsonRequest
    {
        [FromRoute]
        public string Id { get; set; } = string.Empty;

        [FromBody]
        public CustomJsonBody Body { get; set; } = new();
    }

    public class CustomJsonBody
    {
        public string Name { get; set; } = string.Empty;
        public TestEnumValue EnumValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? OptionalField { get; set; }
    }

    public class CustomJsonRequestValidator : AbstractValidator<CustomJsonRequest>
    {
        public CustomJsonRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id is required.");

            RuleFor(x => x.Body.Name)
                .NotEmpty()
                .WithMessage("Name is required.");
        }
    }

    public class CustomJsonResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public TestEnumValue EnumValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? OptionalField { get; set; }
        public string Message { get; set; } = "Custom JSON serialization successful";
    }

    public class CustomJsonEndpoint : RadEndpoint<CustomJsonRequest, CustomJsonResponse>
    {
        public override void Configure()
        {
            Post("/test/custom-json/{id}")
                .Accepts<CustomJsonRequest>("application/json")
                .Produces<CustomJsonResponse>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesValidationProblem(StatusCodes.Status400BadRequest)
                .WithTags("ParameterTests")
                .WithDescription("Test endpoint for custom JSON serialization with enums and date formatting");
        }

        public override async Task Handle(CustomJsonRequest r, CancellationToken ct)
        {
            await Task.CompletedTask;

            Response = new CustomJsonResponse
            {
                Id = r.Id,
                Name = r.Body.Name,
                EnumValue = r.Body.EnumValue,
                CreatedDate = r.Body.CreatedDate,
                OptionalField = r.Body.OptionalField
            };

            Send();
        }
    }
}