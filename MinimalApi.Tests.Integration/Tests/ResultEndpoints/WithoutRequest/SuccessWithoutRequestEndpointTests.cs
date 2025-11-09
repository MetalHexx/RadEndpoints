using MinimalApi.Features.ResultEndpoints.WithoutRequest;

namespace MinimalApi.Tests.Integration.Tests.ResultEndpoints.WithoutRequest;

[Collection("Endpoint")]
public class SuccessWithoutRequestEndpointTests(RadEndpointFixture f)
{
    [Fact]
    public async Task When_Called_ReturnsSuccess()
    {
        // Act
        var r = await f.Client.GetAsync<SuccessWithoutRequestEndpoint, SuccessWithoutRequestResponse>();

        // Assert
        r.Should().BeSuccessful<SuccessWithoutRequestResponse>()
            .WithStatusCode(HttpStatusCode.OK)
            .WithContentNotNull();
        
        r.Content.Message.Should().Be("Operation successful");
        r.Content.Data.Should().Be("sample data");
    }
}
