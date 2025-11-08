using MinimalApi.Features.ResultEndpoints.WithRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithRequest;

[Collection("Endpoint")]
public class BytesEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_ValidSize_ReturnsByteArray()
    {
        // Act
        var response = await f.Client.GetAsync("/api/bytes/10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsByteArrayAsync();
        content.Should().NotBeNull();
        content.Should().HaveCount(10);
        
        // Verify Content-Type header
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.MediaType.Should().Be("application/octet-stream");
        
        // Verify Content-Disposition header for download
        response.Content.Headers.ContentDisposition.Should().NotBeNull();
        response.Content.Headers.ContentDisposition!.FileName.Should().Be("data-10.bin");
    }

    [Fact]
    public async Task When_LargerSize_ReturnsCorrectByteCount()
    {
        // Act
        var response = await f.Client.GetAsync("/api/bytes/100");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsByteArrayAsync();
        content.Should().NotBeNull();
        content.Should().HaveCount(100);
    }

    [Fact]
    public async Task When_InvalidSize_ReturnsError()
    {
        // Act
        var r = await f.Client.GetAsync<BytesEndpoint, BytesRequest, BytesResponse>(new()
        {
            Size = "invalid"
        });

        // Assert
        r.Should().BeSuccessful<BytesResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("Invalid size parameter");
    }
}
