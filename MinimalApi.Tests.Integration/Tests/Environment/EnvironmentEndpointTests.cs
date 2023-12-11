using MinimalApi.Features.Environment.GetEnvironment;

namespace MinimalApi.Tests.Integration.Tests.Environment
{
    [Collection("Endpoint")]
    public class EnvironmentEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task When_Called_ReturnsSuccess()
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