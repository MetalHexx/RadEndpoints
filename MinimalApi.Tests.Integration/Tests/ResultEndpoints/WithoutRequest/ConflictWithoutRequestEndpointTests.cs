using MinimalApi.Features.ResultEndpoints.WithoutRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithoutRequest;

[Collection("Endpoint")]
public class ConflictWithoutRequestEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_Called_ReturnsConflictString()
    {
        // Act
        var response = await f.Client.GetAsync<ConflictWithoutRequestEndpoint, string>();

        // Assert
        response.Http.StatusCode.Should().Be(HttpStatusCode.Conflict);
        response.Content.Should().Be("The resource already exists.");
    }
}
