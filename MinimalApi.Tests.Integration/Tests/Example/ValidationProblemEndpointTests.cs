using MinimalApi.Features.Examples.ValidationProblemExample;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class ValidationProblemExampleEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task Given_ValidationFails_Returns_ValidationProblem()
        {
            // Act
            var r = await f.Client.GetAsync<ValidationProblemExampleEndpoint, ValidationProblemRequest, ValidationProblemDetails>(new ValidationProblemRequest());

            // Assert
            r.Should().BeValidationProblem()
                .WithTitle("Validation Failed")
                .WithStatusCode(HttpStatusCode.BadRequest)
                .WithDetail("One or more validation errors occurred.")
                .WithInstance("/examples/validation-problem")
                .WithKeyAndValue("FieldOne", "FieldOne is required.")
                .WithKeyAndValue("FieldTwo", "FieldTwo must be at least 5 characters.");                                                       
        }

        [Fact]
        public async Task Given_NoRequest_AndValidationFails_Returns_ValidationProblem()
        {
            // Act
            var r = await f.Client.GetAsync<ValidationProblemWithoutRequestExampleEndpoint, ValidationProblemDetails>();

            // Assert
            r.Should().BeValidationProblem()
                .WithTitle("Validation Failed")
                .WithStatusCode(HttpStatusCode.BadRequest)
                .WithDetail("One or more validation errors occurred.")
                .WithInstance("/examples/validation-problem")
                .WithKeyAndValue("FieldOne", "FieldOne is required.")
                .WithKeyAndValue("FieldTwo", "FieldTwo must be at least 5 characters.");
        }
    }
}