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
            r.Http.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Content.Should().BeOfType<CustomPutResponse>();
            r.Content.Data!.Id.Should().Be(1);
            r.Content.Message.Should().Be("Example updated successfully");
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
            r.Http.StatusCode.Should().Be(HttpStatusCode.NotFound);
            r.Content.Should().BeOfType<ProblemDetails>();
            r.Content.Title.Should().Be("Could not find and example with the id provided");
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
            r.Http.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            r.Content.Should().BeOfType<ProblemDetails>();
            r.Content.Extensions.Should().ContainKey("FirstName");
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
            r.Http.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            r.Content.Should().BeOfType<ProblemDetails>();
            r.Content.Extensions.Should().ContainKey("LastName");
        }
    }
}