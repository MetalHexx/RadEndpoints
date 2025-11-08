using MinimalApi.Features.ResultEndpoints.WithRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithRequest;

[Collection("Endpoint")]
public class CreatedAtEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_CreatedAtWithResponse_ReturnsCreatedWithLocationHeader()
    {
        // Act
        var r = await f.Client.PostAsync<CreatedAtEndpoint, CreatedAtRequest, CreatedAtResponse>(new()
        {
            Id = "with-response"
        });

        // Assert
        r.Should().BeSuccessful<CreatedAtResponse>()
            .WithStatusCode(HttpStatusCode.Created)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("Item created with custom response");
        r.Content.ItemId.Should().Be("with-response");
        r.Http.Headers.Location.Should().NotBeNull();
        r.Http.Headers.Location!.ToString().Should().EndWith("/api/items/with-response");
    }

    [Fact]
    public async Task When_CreatedAtWithoutResponse_ReturnsCreatedWithResponseProperty()
    {
        // Act
        var r = await f.Client.PostAsync<CreatedAtEndpoint, CreatedAtRequest, CreatedAtResponse>(new()
        {
            Id = "test"
        });

        // Assert
        r.Should().BeSuccessful<CreatedAtResponse>()
            .WithStatusCode(HttpStatusCode.Created)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("Item test created");
        r.Content.ItemId.Should().Be("test");
        r.Http.Headers.Location.Should().NotBeNull();
        r.Http.Headers.Location!.ToString().Should().EndWith("/api/items/test");
    }
}
