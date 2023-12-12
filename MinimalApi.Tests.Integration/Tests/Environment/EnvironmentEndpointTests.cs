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
            var r = await f.Client.GetAsync<GetEnvironmentEndpoint, GetEnvironmentResponse>();

            //Assert
            r.Http.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Content.Should().BeOfType<GetEnvironmentResponse>();
            r.Content.Should().NotBeNull();
            r.Content.ApplicationName.Should().NotBeEmpty();
            r.Content.EnvironmentName.Should().NotBeEmpty();
        }
    }
}