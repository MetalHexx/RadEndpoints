using MinimalApi.Features.ResultEndpoints.WithoutRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithoutRequest;

[Collection("Endpoint")]
public class RedirectWithoutRequestEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_Called_ReturnsRedirect()
    {
        // Act
        var response = await f.Client.GetAsync("/api/norequest/redirect");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Found);
        response.Headers.Location.Should().NotBeNull();
    }
}
