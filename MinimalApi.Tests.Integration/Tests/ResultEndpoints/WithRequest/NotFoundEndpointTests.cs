using MinimalApi.Features.ResultEndpoints.WithRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithRequest;

[Collection("Endpoint")]
public class NotFoundEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_ItemNotFound_ReturnsNotFoundString()
    {
        // Act
        var r = await f.Client.GetAsync<NotFoundEndpoint, NotFoundRequest, string>(new()
        {
            Id = "missing"
        });

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.NotFound);
        r.Content.Should().Be("The item missing was not found.");
    }

    [Fact]
    public async Task When_ItemFound_ReturnsSuccess()
    {
        // Act
        var r = await f.Client.GetAsync<NotFoundEndpoint, NotFoundRequest, NotFoundResponse>(new()
        {
            Id = "test"
        });

        // Assert
        r.Should().BeSuccessful<NotFoundResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("Found item test");
    }
}
