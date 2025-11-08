using MinimalApi.Features.ResultEndpoints.WithRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithRequest;

[Collection("Endpoint")]
public class RedirectEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_PermanentRedirect_Returns301()
    {
        // Act
        var response = await f.Client.GetAsync("/api/redirect/permanent");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.MovedPermanently);
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.ToString().Should().EndWith("/api/success/redirected");
    }

    [Fact]
    public async Task When_PermanentPreserveRedirect_Returns308()
    {
        // Act
        var response = await f.Client.GetAsync("/api/redirect/permanent-preserve");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.PermanentRedirect);
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.ToString().Should().EndWith("/api/success/redirected");
    }

    [Fact]
    public async Task When_TemporaryRedirect_Returns302()
    {
        // Act
        var response = await f.Client.GetAsync("/api/redirect/temporary");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Found);
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.ToString().Should().EndWith("/api/success/redirected");
    }

    [Fact]
    public async Task When_TemporaryPreserveRedirect_Returns307()
    {
        // Act
        var response = await f.Client.GetAsync("/api/redirect/temporary-preserve");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.TemporaryRedirect);
        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.ToString().Should().EndWith("/api/success/redirected");
    }

    [Fact]
    public async Task When_NoRedirect_ReturnsSuccess()
    {
        // Act
        var r = await f.Client.GetAsync<RedirectEndpoint, RedirectRequest, RedirectResponse>(new()
        {
            Id = "test"
        });

        // Assert
        r.Should().BeSuccessful<RedirectResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("No redirect for test");
    }
}
