using MinimalApi.Features.Examples.GetExample;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class GetExampleEndpointTests
    {
        private readonly EndpointFixture _fixture;
        public GetExampleEndpointTests(EndpointFixture fix) => _fixture = fix;

        [Fact]
        public async void When_GetExampleEndpoint_Called_Returns_Success()
        {
            //Act            
            var (h, r) = await _fixture.Client.GetAsync<GetExampleEndpoint, GetExampleRequest, GetExampleResponse>(new()
            {
                Id = 1
            });

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Should().NotBeNull();
            r!.Data!.Id.Should().Be(1);
        }
    }
}
