using MinimalApi.Features.ResultEndpoints.WithoutRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithoutRequest;

[Collection("Endpoint")]
public class StreamWithoutRequestEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_Called_ReturnsStream()
    {
        // Act
        var response = await f.Client.GetAsync("/api/withoutRequest/stream");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("This is a streamed response without request");
    }
}
