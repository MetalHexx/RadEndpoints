using MinimalApi.Domain.Examples;

namespace MinimalApi.Features.Examples.ValidationProblemExample
{
    public class ValidationProblemExampleEndpoint : RadEndpoint<ValidationProblemRequest, ValidationProblemResult>
    {
        public override void Configure()
        {
            Get("/examples/validation-problem")
                .ProducesValidationProblem()
                .WithDocument(tag: Constants.ExamplesTag, desc: "Example of how to return a ValidationProblemDetails response using SendProblem with validation errors.");
        }

        public override Task Handle(ValidationProblemRequest r, CancellationToken ct)
        {
            var errors = new Dictionary<string, string[]>
            {
                { "FieldOne", new[] { "FieldOne is required." } },
                { "FieldTwo", new[] { "FieldTwo must be at least 5 characters." } }
            };

            var validationProblem = new ValidationProblemDetails(errors)
            {
                Title = "Validation Failed",
                Detail = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://example.com/validation-error",
                Instance = "/examples/validation-problem"
            };

            validationProblem.Extensions["trackingId"] = Guid.NewGuid().ToString();
            validationProblem.Extensions["timestamp"] = DateTime.UtcNow;

            SendProblem(TypedResults.Problem(validationProblem));

            return Task.CompletedTask;
        }
    }

    public class ValidationProblemWithoutRequestExampleEndpoint : RadEndpointWithoutRequest<ValidationProblemResult>
    {
        public override void Configure()
        {
            Get("/examples/validation-problem/WithoutRuest")
                .ProducesValidationProblem()
                .WithDocument(tag: Constants.ExamplesTag, desc: "Example of how to return a ValidationProblemDetails response using SendProblem with validation errors.");
        }

        public override Task Handle(CancellationToken ct)
        {
            var errors = new Dictionary<string, string[]>
            {
                { "FieldOne", new[] { "FieldOne is required." } },
                { "FieldTwo", new[] { "FieldTwo must be at least 5 characters." } }
            };

            var validationProblem = new ValidationProblemDetails(errors)
            {
                Title = "Validation Failed",
                Detail = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://example.com/validation-error",
                Instance = "/examples/validation-problem"
            };

            validationProblem.Extensions["trackingId"] = Guid.NewGuid().ToString();
            validationProblem.Extensions["timestamp"] = DateTime.UtcNow;

            SendProblem(TypedResults.Problem(validationProblem));

            return Task.CompletedTask;
        }
    }
}
