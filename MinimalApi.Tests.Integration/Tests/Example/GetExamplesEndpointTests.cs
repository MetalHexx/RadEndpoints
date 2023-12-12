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
            var r = await f.Client.GetAsync<GetExamplesEndpoint, GetExamplesResponse>();

            //Arrange
            r.Should().BeSuccessful<GetExamplesResponse>()
                .WithStatusCode(HttpStatusCode.OK)
                .WithMessage("Examples retrieved successfully")
                .WithContentNotNull();
        }
    }
}
