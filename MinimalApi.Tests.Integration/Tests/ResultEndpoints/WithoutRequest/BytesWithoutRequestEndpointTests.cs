using MinimalApi.Features.ResultEndpoints.WithoutRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithoutRequest;

[Collection("Endpoint")]
public class BytesWithoutRequestEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_Called_ReturnsByteArray()
    {
        // Act
        var response = await f.Client.GetAsync("/api/norequest/bytes");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsByteArrayAsync();
        content.Should().NotBeNull();
        content.Should().HaveCount(20);
        response.Content.Headers.ContentDisposition.Should().NotBeNull();
    }
}
