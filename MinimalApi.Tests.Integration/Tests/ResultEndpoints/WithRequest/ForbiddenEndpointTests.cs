using MinimalApi.Features.ResultEndpoints.WithRequest;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithRequest;

[Collection("Endpoint")]
public class ForbiddenEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_ForbiddenWithMessage_ReturnsProblemDetails()
    {
        // Act
        var r = await f.Client.GetAsync<ForbiddenEndpoint, ForbiddenRequest, ProblemDetails>(new()
        {
            Id = "no-access"
        });

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("Access forbidden to no-access");
        r.Content.Status.Should().Be(403);
    }

    [Fact]
    public async Task When_ForbiddenWithoutMessage_ReturnsForbidden()
    {
        // Act
        var response = await f.Client.GetAsync("/api/forbidden/no-permission");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task When_AccessGranted_ReturnsSuccess()
    {
        // Act
        var r = await f.Client.GetAsync<ForbiddenEndpoint, ForbiddenRequest, ForbiddenResponse>(new()
        {
            Id = "test"
        });

        // Assert
        r.Should().BeSuccessful<ForbiddenResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("Access granted to test");
    }
}
