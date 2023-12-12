using MinimalApi.Features.Examples.GetExamples;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class GetExamplesEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task When_Called_ReturnsSuccess()
        {
            //Act
            var r = await f.Client.GetAsync<GetExamplesEndpoint, GetExamplesResponse>();

            //Arrange
            r.Http.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Content.Should().BeOfType<GetExamplesResponse>();
            r.Content.Data.Should().NotBeEmpty();
        }
    }
}
