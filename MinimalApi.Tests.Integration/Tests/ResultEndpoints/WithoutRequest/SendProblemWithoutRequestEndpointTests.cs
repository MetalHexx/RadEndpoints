using MinimalApi.Features.ResultEndpoints.WithoutRequest;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithoutRequest;

[Collection("Endpoint")]
public class SendProblemWithoutRequestEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_Called_ReturnsCustomProblemDetails()
    {
        // Act
        var r = await f.Client.GetAsync<SendProblemWithoutRequestEndpoint, ProblemDetails>();

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("Custom problem without request");
        r.Content.Status.Should().Be(422);
    }
}
