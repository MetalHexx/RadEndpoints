using MinimalApi.Features.ResultEndpoints.WithRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithRequest;

[Collection("Endpoint")]
public class FileEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_TestFile_ReturnsFileContent()
    {
        // Act
        var response = await f.Client.GetAsync("/api/withRequest/file/test");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Test file content for test");
        
        // Verify Content-Type header
        response.Content.Headers.ContentType.Should().NotBeNull();
        response.Content.Headers.ContentType!.MediaType.Should().Be("text/plain");
    }

    [Fact]
    public async Task When_DownloadFile_ReturnsFileWithDownloadName()
    {
        // Act
        var response = await f.Client.GetAsync("/api/withRequest/file/download");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Test file content for download");
        
        // Verify Content-Disposition header for download
        response.Content.Headers.ContentDisposition.Should().NotBeNull();
        response.Content.Headers.ContentDisposition!.FileName.Should().Be("download.txt");
    }

    [Fact]
    public async Task When_UnknownFile_ReturnsError()
    {
        // Act
        var r = await f.Client.GetAsync<FileEndpoint, FileRequest, FileResponse>(new()
        {
            Name = "unknown"
        });

        // Assert
        r.Should().BeSuccessful<FileResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("File unknown not found");
    }
}
