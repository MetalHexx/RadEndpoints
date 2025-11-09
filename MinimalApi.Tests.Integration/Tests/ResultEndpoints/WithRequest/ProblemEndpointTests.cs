using MinimalApi.Features.ResultEndpoints.WithRequest;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithRequest;

[Collection("Endpoint")]
public class ProblemEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_InternalError_ReturnsProblemDetails()
    {
        // Act
        var r = await f.Client.GetAsync<ProblemEndpoint, ProblemRequest, ProblemDetails>(new()
        {
            Id = "internal"
        });

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("Internal server error for internal");
        r.Content.Status.Should().Be(500);
    }

    [Fact]
    public async Task When_ExternalError_ReturnsProblemDetails()
    {
        // Act
        var r = await f.Client.GetAsync<ProblemEndpoint, ProblemRequest, ProblemDetails>(new()
        {
            Id = "external"
        });

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.BadGateway);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("External service error for external");
        r.Content.Status.Should().Be(502);
    }

    [Fact]
    public async Task When_ExternalTimeout_ReturnsProblemDetails()
    {
        // Act
        var r = await f.Client.GetAsync<ProblemEndpoint, ProblemRequest, ProblemDetails>(new()
        {
            Id = "timeout"
        });

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.GatewayTimeout);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("External service timeout for timeout");
        r.Content.Status.Should().Be(504);
    }

    [Fact]
    public async Task When_ValidRequest_ReturnsSuccess()
    {
        // Act
        var r = await f.Client.GetAsync<ProblemEndpoint, ProblemRequest, ProblemResponse>(new()
        {
            Id = "test"
        });

        // Assert
        r.Should().BeSuccessful<ProblemResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("No problem with test");
    }
}
