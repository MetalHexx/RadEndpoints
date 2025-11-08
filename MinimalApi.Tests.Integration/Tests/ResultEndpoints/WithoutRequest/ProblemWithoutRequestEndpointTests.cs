using MinimalApi.Features.ResultEndpoints.WithoutRequest;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithoutRequest;

[Collection("Endpoint")]
public class ProblemWithoutRequestEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_Called_ReturnsProblemDetails()
    {
        // Act
        var r = await f.Client.GetAsync<ProblemWithoutRequestEndpoint, ProblemDetails>();

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("Internal server error occurred");
        r.Content.Status.Should().Be(500);
    }
}
