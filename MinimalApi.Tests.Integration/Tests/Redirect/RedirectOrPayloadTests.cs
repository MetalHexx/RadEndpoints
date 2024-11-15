using MinimalApi.Features.Redirect.WithPayload.RedirectWithPayloadEndpoint;

namespace MinimalApi.Tests.Integration.Tests.Redirect
{
    [Collection("Endpoint")]
    public class RedirectOrPayloadTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task Given_True_ReturnsRedirect()
        {
            //Arrange
            var request = new RedirectOrPayloadRequest { ShouldRedirect = true };

            //Act
            var response = await f.Client.PostAsync<RedirectOrPayloadEndpoint, RedirectOrPayloadRequest>(request);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
        }

        [Fact]
        public async Task Given_False_ReturnsPayload()
        {
            //Arrange
            var request = new RedirectOrPayloadRequest { ShouldRedirect = false };

            //Act
            var response = await f.Client.PostAsync<RedirectOrPayloadEndpoint, RedirectOrPayloadRequest, RedirectOrPayloadResponse>(request);

            //Assert
            response.Should()
                .BeSuccessful<RedirectOrPayloadResponse>()
                .WithStatusCode(HttpStatusCode.OK);
        }
    }
}
