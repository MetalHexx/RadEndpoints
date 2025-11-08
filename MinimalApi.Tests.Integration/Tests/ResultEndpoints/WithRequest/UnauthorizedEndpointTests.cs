using MinimalApi.Features.ResultEndpoints.WithRequest;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithRequest;

[Collection("Endpoint")]
public class UnauthorizedEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_UnauthorizedWithMessage_ReturnsProblemDetails()
    {
        // Act
        var r = await f.Client.GetAsync<UnauthorizedEndpoint, UnauthorizedRequest, ProblemDetails>(new()
        {
            Id = "auth-required"
        });

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("Authentication required for auth-required");
        r.Content.Status.Should().Be(401);
    }

    [Fact]
    public async Task When_UnauthorizedWithoutMessage_ReturnsUnauthorized()
    {
        // Act
        var response = await f.Client.GetAsync("/api/unauthorized/no-auth");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task When_Authorized_ReturnsSuccess()
    {
        // Act
        var r = await f.Client.GetAsync<UnauthorizedEndpoint, UnauthorizedRequest, UnauthorizedResponse>(new()
        {
            Id = "test"
        });

        // Assert
        r.Should().BeSuccessful<UnauthorizedResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("Authorized access to test");
    }
}
