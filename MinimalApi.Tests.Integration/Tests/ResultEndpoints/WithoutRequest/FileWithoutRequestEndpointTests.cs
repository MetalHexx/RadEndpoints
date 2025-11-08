using MinimalApi.Features.ResultEndpoints.WithoutRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithoutRequest;

[Collection("Endpoint")]
public class FileWithoutRequestEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_Called_ReturnsFile()
    {
        // Act
        var response = await f.Client.GetAsync("/api/norequest/file");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Test file content without request");
    }
}
