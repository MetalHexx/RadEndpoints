using MinimalApi.Features.ResultEndpoints.WithoutRequest;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithoutRequest;

[Collection("Endpoint")]
public class ValidationErrorWithoutRequestEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_Called_ReturnsValidationProblemDetails()
    {
        // Act
        var r = await f.Client.GetAsync<ValidationErrorWithoutRequestEndpoint, ValidationProblemDetails>();

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("Validation failed");
        r.Content.Status.Should().Be(400);
    }
}
