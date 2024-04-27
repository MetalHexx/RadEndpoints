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
            var r = await f.Client.GetAsync<GetEnvironmentEndpoint, GetEnvironmentRequest, GetEnvironmentResponse>(new());

            //Assert
            r.Should().BeSuccessful<GetEnvironmentResponse>()
                .WithStatusCode(HttpStatusCode.OK)
                .WithContentNotNull();

            r.Content.Message.Should().Be("Environment information retrieved successfully");
            r.Content.ApplicationName.Should().NotBeEmpty();
            r.Content.EnvironmentName.Should().NotBeEmpty();
        }
    }
}