using MinimalApi.Features.CustomExamples.CustomPut;

namespace MinimalApi.Tests.Integration.Tests.CustomExamples
{
    [Collection("Endpoint")]
    public class CustomPutEndpointTests(EndpointFixture _fixture): EndpointFixture
    {
        [Fact]
        public async Task When_ExampleUpdated_ReturnsSuccess()
        {
            //Arrange
            var updateRequest = _fixture.DataGenerator.Create<CustomPutRequest>();
            var route = "/custom-examples/1";

            //Act
            var (h, r) = await _fixture.Client.PutAsync<CustomPutRequest, CustomPutResponse>(route, updateRequest);

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Should().BeOfType<CustomPutResponse>();
            r!.Data!.Id.Should().Be(1);
            r!.Message.Should().Be("Example updated successfully");
        }

        [Fact]
        public async Task Given_ExampleDoesNotExist_When_ExampleUpdated_Returns_NotFoundProblem()
        {
            //Arrange
            var updateRequest = _fixture.DataGenerator.Create<CustomPutRequest>();
            var route = "/custom-examples/999";

            //Act
            var (h, r) = await _fixture.Client.PutAsync<CustomPutRequest, ProblemDetails>(route, updateRequest);

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.NotFound);
            r.Should().BeOfType<ProblemDetails>();           
            r!.Title.Should().Be("Could not find and example with the id provided");
        }
    }
}
