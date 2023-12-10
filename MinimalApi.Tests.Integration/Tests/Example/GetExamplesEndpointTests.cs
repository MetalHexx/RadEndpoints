using MinimalApi.Features.Examples.GetExamples;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class GetExamplesEndpointTests(EndpointFixture f)
    {
        [Fact]
        public async Task When_Called_ReturnsSuccess()
        {
            //Act
            var (h, r) = await f.Client.GetAsync<GetExamplesEndpoint, GetExamplesResponse>();

            //Arrange
            h.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Should().BeOfType<GetExamplesResponse>();
            r!.Data.Should().NotBeEmpty();
        }
    }
}
