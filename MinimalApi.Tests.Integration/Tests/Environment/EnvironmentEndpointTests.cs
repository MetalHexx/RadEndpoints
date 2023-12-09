using MinimalApi.Features.Environment.GetEnvironment;

namespace MinimalApi.Tests.Integration.Tests.Environment
{
    [Collection("Endpoint")]
    public class EnvironmentEndpointTests(EndpointFixture f)
    {
        [Fact]
        public async Task When_EnvironmentEndpointCalled_Returns_Success()
        {
            //Act            
            var (h, r) = await f.Client.GetAsync<GetEnvironmentEndpoint, GetEnvironmentResponse>();

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Should().BeOfType<GetEnvironmentResponse>();
            r!.Should().NotBeNull();
            r!.ApplicationName.Should().NotBeEmpty();
            r!.EnvironmentName.Should().NotBeEmpty();
        }
    }
}