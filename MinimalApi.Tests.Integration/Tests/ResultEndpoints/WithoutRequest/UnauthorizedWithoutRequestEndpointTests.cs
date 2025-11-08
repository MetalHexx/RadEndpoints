using MinimalApi.Features.ResultEndpoints.WithoutRequest;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithoutRequest;

[Collection("Endpoint")]
public class UnauthorizedWithoutRequestEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_Called_ReturnsProblemDetails()
    {
        // Act
        var r = await f.Client.GetAsync<UnauthorizedWithoutRequestEndpoint, ProblemDetails>();

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("Authentication required");
        r.Content.Status.Should().Be(401);
    }
}
