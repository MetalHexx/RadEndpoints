using MinimalApi.Features.ParameterTestEndpoints.EmptyStringTestEndpoints;

namespace MinimalApi.Tests.Integration.Tests.ParameterTests
{
    [Collection("Endpoint")]
    public class EmptyStringParameterTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task EmptyQueryParameter_WhenEmptyString_ShouldTriggerValidation()
        {
            var request = new EmptyQueryParameterRequest
            {
                Id = "test-id",
                RequiredParam = "",
                OptionalParam = ""
            };

            var response = await f.Client.GetAsync<EmptyQueryParameterEndpoint, EmptyQueryParameterRequest, ValidationProblemDetails>(request);

            response.Should().BeValidationProblem()
                .WithKeyAndValue("RequiredParam", "RequiredParam cannot be empty.");
        }

        [Fact]
        public async Task EmptyQueryParameter_WhenValidData_ShouldSucceed()
        {
            var request = new EmptyQueryParameterRequest
            {
                Id = "test-id",
                RequiredParam = "valid-data",
                OptionalParam = ""
            };

            var response = await f.Client.GetAsync<EmptyQueryParameterEndpoint, EmptyQueryParameterRequest, EmptyQueryParameterResponse>(request);

            response.Should().BeSuccessful<EmptyQueryParameterResponse>()
                .WithStatusCode(HttpStatusCode.OK);

            response.Content.Id.Should().Be("test-id");
            response.Content.RequiredParam.Should().Be("valid-data");
            response.Content.OptionalParam.Should().Be("");
        }

        [Fact]
        public async Task EmptyFormField_WhenEmptyString_ShouldTriggerValidation()
        {
            var request = new EmptyFormFieldRequest
            {
                Id = "test-id",
                RequiredField = "", 
                OptionalField = "", 
                Description = ""    
            };

            var response = await f.Client.PostAsync<EmptyFormFieldEndpoint, EmptyFormFieldRequest, ValidationProblemDetails>(request);

            response.Should().BeValidationProblem()
                .WithKeyAndValue("RequiredField", "RequiredField cannot be empty.");
        }

        [Fact]
        public async Task EmptyFormField_WhenValidData_ShouldSucceed()
        {
            var request = new EmptyFormFieldRequest
            {
                Id = "test-id",
                RequiredField = "valid-data",
                OptionalField = "",
                Description = "Short description"
            };

            var response = await f.Client.PostAsync<EmptyFormFieldEndpoint, EmptyFormFieldRequest, EmptyFormFieldResponse>(request);

            response.Should().BeSuccessful<EmptyFormFieldResponse>()
                .WithStatusCode(HttpStatusCode.OK);

            response.Content.Id.Should().Be("test-id");
            response.Content.RequiredField.Should().Be("valid-data");
            response.Content.OptionalField.Should().Be("");
            response.Content.Description.Should().Be("Short description");
        }

        [Fact]
        public async Task EmptyHeader_WhenEmptyString_ShouldTriggerValidation()
        {
            var request = new EmptyHeaderRequest
            {
                Id = "test-id",
                RequiredHeader = "", 
                OptionalHeader = "", 
                AuthHeader = ""      
            };

            var response = await f.Client.PostAsync<EmptyHeaderEndpoint, EmptyHeaderRequest, ValidationProblemDetails>(request);

            response.Should().BeValidationProblem();
            
            var errors = response.Content.Errors;
            errors.Should().ContainKey("RequiredHeader")
                .WhoseValue.Should().Contain("X-Required-Header cannot be empty.");
            errors.Should().ContainKey("AuthHeader")
                .WhoseValue.Should().Contain("Authorization header is required.");
        }
    }
}