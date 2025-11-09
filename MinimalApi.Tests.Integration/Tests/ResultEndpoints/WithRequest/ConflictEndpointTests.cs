using MinimalApi.Features.ResultEndpoints.WithRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithRequest;

[Collection("Endpoint")]
public class ConflictEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_UserEmailAlreadyExists_ReturnsConflict()
    {
        // Act
        var r = await f.Client.PostAsync<ConflictEndpoint, ConflictRequest, string>(new()
        {
            Name = "John Doe",
            Email = "existing@example.com"
        });

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.Conflict);
        r.Content.Should().Be("A user with email existing@example.com already exists.");
    }

    [Fact]
    public async Task When_UserEmailIsUnique_CreatesUser()
    {
        // Act
        var r = await f.Client.PostAsync<ConflictEndpoint, ConflictRequest, ConflictResponse>(new()
        {
            Name = "Jane Doe",
            Email = "jane@example.com"
        });

        // Assert
        r.Should().BeSuccessful<ConflictResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("User created successfully");
        r.Content.Id.Should().NotBeNullOrEmpty();
    }
}
