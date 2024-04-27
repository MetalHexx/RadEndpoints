using MinimalApi.Features.Examples.GetExamples;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class GetExamplesEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task When_Called_ReturnsSuccess()
        {
            //Act
            var r = await f.Client.GetAsync<GetExamplesEndpoint, GetExamplesRequest, GetExamplesResponse>(new());

            //Arrange
            r.Should().BeSuccessful<GetExamplesResponse>()
                .WithStatusCode(HttpStatusCode.OK)
                .WithContentNotNull();

            r.Content.Message.Should().Be("Examples retrieved successfully");
        }
    }
}
