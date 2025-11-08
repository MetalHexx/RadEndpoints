using MinimalApi.Features.ResultEndpoints.WithRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithRequest;

[Collection("Endpoint")]
public class SuccessEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_SendWithResponse_ReturnsCustomResponse()
    {
        // Act
        var r = await f.Client.GetAsync<SuccessEndpoint, SuccessRequest, SuccessResponse>(new()
        {
            Id = "with-response"
        });

        // Assert
        r.Should().BeSuccessful<SuccessResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("Success with custom response");
        r.Content.Data.Should().Be("custom data");
    }

    [Fact]
    public async Task When_SendWithoutParameter_ReturnsResponseProperty()
    {
        // Act
        var r = await f.Client.GetAsync<SuccessEndpoint, SuccessRequest, SuccessResponse>(new()
        {
            Id = "test"
        });

        // Assert
        r.Should().BeSuccessful<SuccessResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("Success for test");
        r.Content.Data.Should().Be("test");
    }
}
