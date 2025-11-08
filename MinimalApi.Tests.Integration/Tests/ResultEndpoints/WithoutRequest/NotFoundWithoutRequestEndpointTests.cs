using MinimalApi.Features.ResultEndpoints.WithoutRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithoutRequest;

[Collection("Endpoint")]
public class NotFoundWithoutRequestEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_Called_ReturnsNotFoundString()
    {
        // Act
        var response = await f.Client.GetAsync<NotFoundWithoutRequestEndpoint, string>();

        // Assert
        response.Http.StatusCode.Should().Be(HttpStatusCode.NotFound);
        response.Content.Should().Be("The resource was not found.");
    }
}
