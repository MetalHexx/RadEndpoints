using MinimalApi.Features.CustomExamples.CustomPut;
using MinimalApi.Tests.Integration.Common;

namespace MinimalApi.Tests.Integration.Tests.CustomExamples
{
    [Collection("Endpoint")]
    public class CustomPutEndpointTests(EndpointFixture Fixture): EndpointFixture
    {
        [Fact]
        public async Task When_ExampleUpdated_ReturnsSuccess()
        {
            //Arrange
            var updateRequest = Fixture.DataGenerator.Create<CustomPutRequest>();
            var route = "/custom-examples/1";

            //Act
            var (h, r) = await Fixture.Client.PutAsync<CustomPutRequest, CustomPutResponse>(route, updateRequest);

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Should().NotBeNull();
            r!.Data.Should().NotBeNull();
            r!.Data!.Id.Should().Be(1);
            r!.Message.Should().Be("Example updated successfully");
        }
    }
}
