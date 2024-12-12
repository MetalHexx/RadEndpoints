using MinimalApi.Features.WithoutRequest.PostWithoutRequest;

namespace MinimalApi.Tests.Integration.Tests.WithoutRequest
{
    public class PostWithoutRequestEndpointTests(RadEndpointFixture f) : IClassFixture<RadEndpointFixture>
    {
        [Fact]
        public async Task When_Called_ReturnsSuccess()
        {
            //Act            
            var r = await f.Client.PostAsync<PostWithoutRequestEndpoint, PostWithoutRequestResponse>();
            //Assert
            r.Should().BeSuccessful<PostWithoutRequestResponse>()
                .WithStatusCode(HttpStatusCode.OK)
                .WithContentNotNull();

            r.Content.Message.Should().Be("Success!");
        }
    }
}
