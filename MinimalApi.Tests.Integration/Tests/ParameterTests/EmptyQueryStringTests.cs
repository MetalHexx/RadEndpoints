using MinimalApi.Features.ParameterTestEndpoints.EmptyStringTestEndpoints;

namespace MinimalApi.Tests.Integration.Tests.ParameterTests
{
    [Collection("Endpoint")]
    public class EmptyQueryStringTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task EmptyQueryString_WhenEmptyPath_ShouldTriggerValidation()
        {
            var request = new EmptyQueryStringRequest
            {
                DeviceId = "ABC12345",
                StorageType = "SD",
                Path = "",
                Filter = "photos",
                SortBy = "name"
            };

            var response = await f.Client.GetAsync<EmptyQueryStringEndpoint, EmptyQueryStringRequest, ValidationProblemDetails>(request);

            response.Should().BeValidationProblem()
                .WithKeyAndValue("Path", "Path cannot be empty.");
        }

        [Fact]
        public async Task EmptyQueryString_WhenEmptyFilter_ShouldTriggerValidation()
        {
            var request = new EmptyQueryStringRequest
            {
                DeviceId = "ABC12345",
                StorageType = "USB",
                Path = "/documents",
                Filter = "",
                SortBy = "date"
            };

            var response = await f.Client.GetAsync<EmptyQueryStringEndpoint, EmptyQueryStringRequest, ValidationProblemDetails>(request);

            response.Should().BeValidationProblem()
                .WithKeyAndValue("Filter", "Filter cannot be empty when provided.");
        }

        [Fact]
        public async Task EmptyQueryString_WhenEmptySortBy_ShouldTriggerValidation()
        {
            var request = new EmptyQueryStringRequest
            {
                DeviceId = "ABC12345",
                StorageType = "EMMC",
                Path = "/music",
                Filter = "mp3",
                SortBy = ""
            };

            var response = await f.Client.GetAsync<EmptyQueryStringEndpoint, EmptyQueryStringRequest, ValidationProblemDetails>(request);

            response.Should().BeValidationProblem()
                .WithKeyAndValue("SortBy", "SortBy cannot be empty when provided.");
        }

        [Fact]
        public async Task EmptyQueryString_WhenAllEmptyQueryParams_ShouldTriggerMultipleValidations()
        {
            var request = new EmptyQueryStringRequest
            {
                DeviceId = "ABC12345",
                StorageType = "FLASH",
                Path = "",     
                Filter = "",   
                SortBy = ""    
            };

            var response = await f.Client.GetAsync<EmptyQueryStringEndpoint, EmptyQueryStringRequest, ValidationProblemDetails>(request);

            response.Should().BeValidationProblem();
            
            var errors = response.Content.Errors;
            errors.Should().ContainKey("Path")
                .WhoseValue.Should().Contain("Path cannot be empty.");
            errors.Should().ContainKey("Filter")
                .WhoseValue.Should().Contain("Filter cannot be empty when provided.");
            errors.Should().ContainKey("SortBy")
                .WhoseValue.Should().Contain("SortBy cannot be empty when provided.");
        }

        [Fact]
        public async Task EmptyQueryString_WhenValidRequest_ShouldReturnSuccess()
        {
            var request = new EmptyQueryStringRequest
            {
                DeviceId = "DEF67890",
                StorageType = "SD",
                Path = "/pictures",
                Filter = "jpeg",
                SortBy = "size"
            };

            var response = await f.Client.GetAsync<EmptyQueryStringEndpoint, EmptyQueryStringRequest, EmptyQueryStringResponse>(request);

            response.Should().BeSuccessful<EmptyQueryStringResponse>()
                .WithStatusCode(HttpStatusCode.OK);

            response.Content.DeviceId.Should().Be("DEF67890");
            response.Content.StorageType.Should().Be("SD");
            response.Content.Path.Should().Be("/pictures");
            response.Content.Filter.Should().Be("jpeg");
            response.Content.SortBy.Should().Be("size");
            response.Content.AllQueryParametersReceived.Should().BeTrue();
            response.Content.ProcessedParametersCount.Should().Be(3);
            response.Content.Message.Should().Contain("Processed 3 query string parameters");
        }

        [Fact]
        public async Task EmptyQueryString_WithNullQueryParams_ShouldSucceedWithPartialData()
        {
            var request = new EmptyQueryStringRequest
            {
                DeviceId = "GHI12345",
                StorageType = "USB",
                Path = "/videos",
                Filter = null, 
                SortBy = null  
            };

            var response = await f.Client.GetAsync<EmptyQueryStringEndpoint, EmptyQueryStringRequest, EmptyQueryStringResponse>(request);

            response.Should().BeSuccessful<EmptyQueryStringResponse>()
                .WithStatusCode(HttpStatusCode.OK);

            response.Content.Path.Should().Be("/videos");
            response.Content.Filter.Should().BeNull();
            response.Content.SortBy.Should().BeNull();
            response.Content.AllQueryParametersReceived.Should().BeFalse();
            response.Content.ProcessedParametersCount.Should().Be(1); 
        }

        [Fact]
        public async Task EmptyQueryString_WhenInvalidDeviceId_ShouldTriggerValidation()
        {
            var request = new EmptyQueryStringRequest
            {
                DeviceId = "123",
                StorageType = "SD",
                Path = "/documents",
                Filter = "pdf",
                SortBy = "name"
            };

            var response = await f.Client.GetAsync<EmptyQueryStringEndpoint, EmptyQueryStringRequest, ValidationProblemDetails>(request);

            response.Should().BeValidationProblem()
                .WithKeyAndValue("DeviceId", "DeviceId must be exactly 8 characters long.");
        }

        [Fact]
        public async Task EmptyQueryString_WhenInvalidStorageType_ShouldTriggerValidation()
        {
            var request = new EmptyQueryStringRequest
            {
                DeviceId = "JKL45678",
                StorageType = "INVALID",
                Path = "/documents",
                Filter = "pdf",
                SortBy = "name"
            };

            var response = await f.Client.GetAsync<EmptyQueryStringEndpoint, EmptyQueryStringRequest, ValidationProblemDetails>(request);

            response.Should().BeValidationProblem()
                .WithKeyAndValue("StorageType", "StorageType must be one of: SD, USB, EMMC, FLASH.");
        }

        [Fact]
        public async Task EmptyQueryString_WhenInvalidPath_ShouldTriggerValidation()
        {
            var request = new EmptyQueryStringRequest
            {
                DeviceId = "MNO78901",
                StorageType = "EMMC",
                Path = "invalid-path",
                Filter = "txt",
                SortBy = "date"
            };

            var response = await f.Client.GetAsync<EmptyQueryStringEndpoint, EmptyQueryStringRequest, ValidationProblemDetails>(request);

            response.Should().BeValidationProblem()
                .WithKeyAndValue("Path", "Path must start with '/'.");
        }

        [Fact]
        public async Task EmptyQueryString_WithMixedValidAndEmptyParams_ShouldPreserveNonEmptyValues()
        {
            var request = new EmptyQueryStringRequest
            {
                DeviceId = "PQR23456",
                StorageType = "FLASH",
                Path = "/downloads",     
                Filter = null,         
                SortBy = "name"      
            };

            var response = await f.Client.GetAsync<EmptyQueryStringEndpoint, EmptyQueryStringRequest, EmptyQueryStringResponse>(request);

            response.Should().BeSuccessful<EmptyQueryStringResponse>()
                .WithStatusCode(HttpStatusCode.OK);

            response.Content.Path.Should().Be("/downloads");
            response.Content.Filter.Should().BeNull();
            response.Content.SortBy.Should().Be("name");
            response.Content.ProcessedParametersCount.Should().Be(2);
            response.Content.AllQueryParametersReceived.Should().BeFalse();
        }
    }
}