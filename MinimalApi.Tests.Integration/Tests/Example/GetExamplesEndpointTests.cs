using MinimalApi.Features.Examples.GetExamples;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class GetExamplesEndpointTests
    {
        private readonly EndpointFixture _fixture;
        public GetExamplesEndpointTests(EndpointFixture fix) => _fixture = fix;

        [Fact]
        public async Task When_GetExamplesCalled_Returns_Success()
        {
            //Act
            var (h, r) = await _fixture.Client.GetAsync<GetExamplesEndpoint, GetExamplesResponse>();

            //Arrange
            h.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Should().NotBeNull();
            r!.Data.Should().NotBeEmpty();
        }
    }
}
