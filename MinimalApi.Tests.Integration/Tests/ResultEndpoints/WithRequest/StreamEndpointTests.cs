using MinimalApi.Features.ResultEndpoints.WithRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithRequest;

[Collection("Endpoint")]
public class StreamEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_TextStream_ReturnsTextContent()
    {
        // Act
        var response = await f.Client.GetAsync("/api/stream/text");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Be("This is a streamed text response");
        
        // Verify Content-Type header
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.MediaType.Should().Be("text/plain");
        
        // Verify Content-Disposition header
        response.Content.Headers.ContentDisposition.Should().NotBeNull();
        response.Content.Headers.ContentDisposition!.FileName.Should().Be("stream.txt");
    }

    [Fact]
    public async Task When_BinaryStream_ReturnsBinaryContent()
    {
        // Act
        var response = await f.Client.GetAsync("/api/stream/binary");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsByteArrayAsync();
        content.Should().NotBeNull();
        content.Should().BeEquivalentTo(new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F });
        
        // Verify Content-Type header
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/octet-stream");
    }

    [Fact]
    public async Task When_UnknownType_ReturnsError()
    {
        // Act
        var r = await f.Client.GetAsync<StreamEndpoint, StreamRequest, StreamResponse>(new()
        {
            Type = "unknown"
        });

        // Assert
        r.Should().BeSuccessful<StreamResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("Unknown stream type: unknown");
    }
}
