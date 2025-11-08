using MinimalApi.Features.ResultEndpoints.WithRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithRequest;

[Collection("Endpoint")]
public class ConflictEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_ItemExists_ReturnsConflictString()
    {
        // Act
        var r = await f.Client.GetAsync<ConflictEndpoint, ConflictRequest, string>(new()
        {
            Id = "exists"
        });

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.Conflict);
        r.Content.Should().Be("The item exists already exists.");
    }

    [Fact]
    public async Task When_ItemDoesNotExist_ReturnsSuccess()
    {
        // Act
        var r = await f.Client.GetAsync<ConflictEndpoint, ConflictRequest, ConflictResponse>(new()
        {
            Id = "test"
        });

        // Assert
        r.Should().BeSuccessful<ConflictResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("Item test is available");
    }
}
