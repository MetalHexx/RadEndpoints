using MinimalApi.Features.ResultEndpoints.WithoutRequest;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithoutRequest;

[Collection("Endpoint")]
public class ForbiddenWithoutRequestEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_Called_ReturnsProblemDetails()
    {
        // Act
        var r = await f.Client.GetAsync<ForbiddenWithoutRequestEndpoint, ProblemDetails>();

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("Access forbidden");
        r.Content.Status.Should().Be(403);
    }
}
