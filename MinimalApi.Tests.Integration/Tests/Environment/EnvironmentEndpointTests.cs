using MinimalApi.Features.Environment.GetApplicationName;
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
            r.Should().BeSuccessful<GetEnvironmentResponse>()
                .WithStatusCode(HttpStatusCode.OK)
                .WithContentNotNull();

            r.Content.Message.Should().Be("Environment information retrieved successfully");
            r.Content.ApplicationName.Should().NotBeEmpty();
            r.Content.EnvironmentName.Should().NotBeEmpty();
        }
    }

    [Collection("Endpoint")]
    public class GetApplicationNameEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task When_Called_ReturnsSuccess()
        {
            //Act            
            var r = await f.Client.GetAsync<GetApplicationNameEndpoint, GetApplicationNameResponse>();

            //Assert
            r.Should().BeSuccessful<GetApplicationNameResponse>()
                .WithStatusCode(HttpStatusCode.OK)
                .WithContentNotNull();

            r.Content.Message.Should().Be("Application name retrieved successfully");
            r.Content.ApplicationName.Should().NotBeEmpty();
        }
    }
}