using MinimalApi.Features.ResultEndpoints.WithRequest;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithRequest;

[Collection("Endpoint")]
public class ValidationErrorEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_ValidationFails_ReturnsValidationProblemDetails()
    {
        // Act
        var r = await f.Client.GetAsync<ValidationErrorEndpoint, ValidationErrorRequest, ValidationProblemDetails>(new()
        {
            Id = "invalid"
        });

        // Assert
        r.Http.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        r.Content.Should().NotBeNull();
        r.Content.Title.Should().Be("Validation failed for invalid");
        r.Content.Status.Should().Be(400);
        r.Content.Errors.Should().ContainKey("ValidationError");
        r.Content.Errors["ValidationError"].Should().Contain("Validation failed for invalid");
    }

    [Fact]
    public async Task When_ValidationPasses_ReturnsSuccess()
    {
        // Act
        var r = await f.Client.GetAsync<ValidationErrorEndpoint, ValidationErrorRequest, ValidationErrorResponse>(new()
        {
            Id = "test"
        });

        // Assert
        r.Should().BeSuccessful<ValidationErrorResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("Validation passed for test");
    }
}
