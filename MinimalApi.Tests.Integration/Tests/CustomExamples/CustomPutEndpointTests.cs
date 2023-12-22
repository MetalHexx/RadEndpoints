using MinimalApi.Features.CustomExamples.CustomPut;

namespace MinimalApi.Tests.Integration.Tests.CustomExamples
{
    [Collection("Endpoint")]
    public class CustomPutEndpointTests(RadEndpointFixture f): RadEndpointFixture
    {
        [Fact]
        public async Task Given_ExampleExists_When_RequestValid_ReturnsSuccess()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<CustomPutRequest>();
            updateRequest.Id = 1;

            //Act
            var r = await f.Client.PutAsync<CustomPutEndpoint, CustomPutRequest, CustomPutResponse>(updateRequest);

            //Assert
            r.Should().BeSuccessful<CustomPutResponse>()
                .WithStatusCode(HttpStatusCode.OK)
                .WithMessage("Example updated successfully")
                .WithContentNotNull();
        }

        [Fact]
        public async Task Given_ExampleDoesntExist_ReturnsProblem()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<CustomPutRequest>();
            updateRequest.Id = 999;

            //Act
            var r = await f.Client.PutAsync<CustomPutEndpoint, CustomPutRequest, ProblemDetails>(updateRequest);

            //Assert
            r.Should().BeProblem()
                .WithStatusCode(HttpStatusCode.NotFound)
                .WithMessage("Could not find and example with the id provided");
        }

        [Fact]        
        public async Task When_FirstNameEmpty_ReturnsProblem()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<CustomPutRequest>();
            updateRequest.Data.FirstName = string.Empty;

            //Act
            var r = await f.Client.PutAsync<CustomPutEndpoint, CustomPutRequest, ValidationProblemDetails>(updateRequest);

            //Assert
            r.Should().BeValidationProblem()
                .WithStatusCode(HttpStatusCode.BadRequest)
                .WithMessage("Validation Error")
                .WithKey("Data.FirstName");
        }

        [Fact]
        public async Task When_LastNameEmpty_ReturnsProblem()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<CustomPutRequest>();
            updateRequest.Data.LastName = string.Empty;

            //Act
            var r = await f.Client.PutAsync<CustomPutEndpoint, CustomPutRequest, ValidationProblemDetails>(updateRequest);

            //Assert
            r.Should().BeValidationProblem()
                .WithStatusCode(HttpStatusCode.BadRequest)
                .WithMessage("Validation Error")
                .WithKey("Data.LastName");
        }
    }
}