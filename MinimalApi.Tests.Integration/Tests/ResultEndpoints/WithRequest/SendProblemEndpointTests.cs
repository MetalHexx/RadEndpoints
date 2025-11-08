using MinimalApi.Features.ResultEndpoints.WithRequest;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithRequest;

[Collection("Endpoint")]
public class SendProblemEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_SendProblemHttpResult_ReturnsCustomProblemDetails()
    {
        // Act
        var r = await f.Client.GetAsync<SendProblemEndpoint, SendProblemRequest, ProblemDetails>(new()
        {
            Type = "http-result"
        });

        // Assert
        r.Http.StatusCode.Should().Be((HttpStatusCode)418);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("Custom HTTP problem");
        r.Content.Status.Should().Be(418);
        r.Content.Detail.Should().Be("This is a custom problem using ProblemHttpResult");
    }

    [Fact]
    public async Task When_SendProblemValidationProblem_ReturnsValidationProblemDetails()
    {
        // Act
        var r = await f.Client.GetAsync<SendProblemEndpoint, SendProblemRequest, ValidationProblemDetails>(new()
        {
            Type = "validation-problem"
        });

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("Custom validation problem");
        r.Content.Status.Should().Be(400);
        r.Content.Detail.Should().Be("Multiple validation errors occurred");
        r.Content.Errors.Should().ContainKey("Field1");
        r.Content.Errors.Should().ContainKey("Field2");
        r.Content.Errors["Field1"].Should().Contain("Field1 is required");
        r.Content.Errors["Field2"].Should().Contain("Field2 must be valid");
    }

    [Fact]
    public async Task When_SendProblemCustomDomainError_ReturnsCustomProblemDetails()
    {
        // Act
        var r = await f.Client.GetAsync<SendProblemEndpoint, SendProblemRequest, ProblemDetails>(new()
        {
            Type = "rad-problem"
        });

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("Custom domain error occurred");
        r.Content.Status.Should().Be(422);
        r.Content.Detail.Should().Be("Error Code: DOMAIN_ERROR_001");
    }

    [Fact]
    public async Task When_SendProblemInternalError_ReturnsInternalServerError()
    {
        // Act
        var r = await f.Client.GetAsync<SendProblemEndpoint, SendProblemRequest, ProblemDetails>(new()
        {
            Type = "internal-error"
        });

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("Internal error via IRadProblem");
        r.Content.Status.Should().Be(500);
    }

    [Fact]
    public async Task When_NoProblemiRequested_ReturnsSuccess()
    {
        // Act
        var r = await f.Client.GetAsync<SendProblemEndpoint, SendProblemRequest, SendProblemResponse>(new()
        {
            Type = "test"
        });

        // Assert
        r.Should().BeSuccessful<SendProblemResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("No problem for test");
    }
}
