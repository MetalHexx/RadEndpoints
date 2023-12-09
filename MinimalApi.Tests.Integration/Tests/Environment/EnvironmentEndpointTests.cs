using MinimalApi.Features.Environment.GetEnvironment;

namespace MinimalApi.Tests.Integration.Tests.Environment
{
    [Collection("Endpoint")]
    public class EnvironmentEndpointTests
    {
        private readonly EndpointFixture _fixture;
        public EnvironmentEndpointTests(EndpointFixture fix) => _fixture = fix;

        [Fact]
        public async Task When_EnvironmentEndpointCalled_Returns_Success()
        {
            //Act            
            var (h, r) = await _fixture.Client.GetAsync<GetEnvironmentEndpoint, GetEnvironmentResponse>();

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Should().NotBeNull();            
        }
    }
}