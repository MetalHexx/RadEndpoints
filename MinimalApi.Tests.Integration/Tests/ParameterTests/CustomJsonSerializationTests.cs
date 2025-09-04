using MinimalApi.Features.ParameterTests.EmptyStringTests;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MinimalApi.Tests.Integration.Tests.ParameterTests
{
    [Collection("Endpoint")]
    public class CustomJsonSerializationTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task CustomJson_WithDefaultSerialization_ShouldWork()
        {
            // Test default serialization behavior (enums as numbers)
            var request = new CustomJsonRequest
            {
                Id = "test123",
                Body = new CustomJsonBody
                {
                    Name = "Test User",
                    EnumValue = TestEnumValue.SecondOption,
                    CreatedDate = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc),
                    OptionalField = "Optional Value"
                }
            };

            var response = await f.Client.PostAsync<CustomJsonEndpoint, CustomJsonRequest, CustomJsonResponse>(request);

            response.Should().BeSuccessful<CustomJsonResponse>()
                .WithStatusCode(HttpStatusCode.OK);

            response.Content.Id.Should().Be("test123");
            response.Content.Name.Should().Be("Test User");
            response.Content.EnumValue.Should().Be(TestEnumValue.SecondOption);
            response.Content.OptionalField.Should().Be("Optional Value");
        }

        [Fact]
        public async Task CustomJson_WithCustomHeaders_ShouldWork()
        {
            // Test that custom headers work with JSON requests
            var options = new RadHttpClientOptions
            {
                Headers = new HeaderDictionary
                {
                    { "X-Test-Header", "CustomJsonTest" },
                    { "Accept", "application/json" }
                }
            };

            var request = new CustomJsonRequest
            {
                Id = "header-test",
                Body = new CustomJsonBody
                {
                    Name = "Header Test User",
                    EnumValue = TestEnumValue.FirstOption,
                    CreatedDate = DateTime.UtcNow,
                    OptionalField = "With Headers"
                }
            };

            var response = await f.Client.PostAsync<CustomJsonEndpoint, CustomJsonRequest, CustomJsonResponse>(
                request, 
                options
            );

            response.Should().BeSuccessful<CustomJsonResponse>()
                .WithStatusCode(HttpStatusCode.OK);

            response.Content.Name.Should().Be("Header Test User");
            response.Content.EnumValue.Should().Be(TestEnumValue.FirstOption);
            response.Content.OptionalField.Should().Be("With Headers");
        }

        [Fact]
        public async Task CustomJson_WithValidationFailure_ShouldTriggerValidation()
        {
            // Test that validation still works with custom options
            var jsonOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var options = new RadHttpClientOptions
            {
                JsonSerializerOptions = jsonOptions
            };

            var request = new CustomJsonRequest
            {
                Id = "valid-id", // Valid ID so route parameter passes
                Body = new CustomJsonBody
                {
                    Name = "", // Empty name should trigger validation
                    EnumValue = TestEnumValue.FirstOption,
                    CreatedDate = DateTime.UtcNow
                }
            };

            var response = await f.Client.PostAsync<CustomJsonEndpoint, CustomJsonRequest, ValidationProblemDetails>(request, options);

            response.Should().BeValidationProblem();
            
            var errors = response.Content.Errors;
            errors.Should().ContainKey("Body.Name")
                .WhoseValue.Should().Contain("Name is required.");
        }

        [Fact]
        public async Task CustomJson_RequestSerializationWithCustomOptions_ShouldProcessRequest()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var options = new RadHttpClientOptions
            {
                JsonSerializerOptions = jsonOptions
            };

            var request = new CustomJsonRequest
            {
                Id = "options-test",
                Body = new CustomJsonBody
                {
                    Name = "Custom Options Test",
                    EnumValue = TestEnumValue.ThirdOption,
                    CreatedDate = DateTime.UtcNow,
                    OptionalField = "Testing custom options"
                }
            };
            var response = await f.Client.PostAsync<CustomJsonEndpoint, CustomJsonRequest, CustomJsonResponse>(
                request, 
                options
            );

            response.Should()
                .BeSuccessful<CustomJsonResponse>()
                .WithStatusCode(HttpStatusCode.OK);

            response.Content.Id.Should().Be("options-test");
            response.Content.Message.Should().Be("Custom JSON serialization successful");
        }
    }
}