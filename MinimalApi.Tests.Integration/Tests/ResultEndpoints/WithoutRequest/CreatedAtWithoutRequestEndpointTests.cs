using MinimalApi.Features.ResultEndpoints.WithoutRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithoutRequest;

[Collection("Endpoint")]
public class CreatedAtWithoutRequestEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_Called_ReturnsCreated()
    {
        // Act
        var r = await f.Client.PostAsync<CreatedAtWithoutRequestEndpoint, CreatedAtWithoutRequestResponse>();

        // Assert
        r.Should().BeSuccessful<CreatedAtWithoutRequestResponse>()
            .WithStatusCode(HttpStatusCode.Created)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("Item created");
        r.Content.ItemId.Should().Be("new-item-123");
        r.Http.Headers.Location.Should().NotBeNull();
    }
}
