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
            var route = "/custom-examples/1";

            //Act
            var r = await f.Client.PutAsync<CustomPutRequest, CustomPutResponse>(route, updateRequest);

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
            var route = "/custom-examples/999";

            //Act
            var r = await f.Client.PutAsync<CustomPutRequest, ProblemDetails>(route, updateRequest);

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
            updateRequest.FirstName = string.Empty;

            var route = "/custom-examples/1";

            //Act
            var r = await f.Client.PutAsync<CustomPutRequest, ProblemDetails>(route, updateRequest);

            //Assert
            r.Should().BeProblem()
                .WithStatusCode(HttpStatusCode.BadRequest)
                .WithMessage("Validation Error")
                .WithKey("FirstName");
        }

        [Fact]
        public async Task When_LastNameEmpty_ReturnsProblem()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<CustomPutRequest>();
            updateRequest.LastName = string.Empty;

            var route = "/custom-examples/1";

            //Act
            var r = await f.Client.PutAsync<CustomPutRequest, ProblemDetails>(route, updateRequest);

            //Assert
            r.Should().BeProblem()
                .WithStatusCode(HttpStatusCode.BadRequest)
                .WithMessage("Validation Error")
                .WithKey("LastName");
        }
    }
}