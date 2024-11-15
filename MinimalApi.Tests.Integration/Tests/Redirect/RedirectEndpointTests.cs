using MinimalApi.Features.Redirect.Temp;

namespace MinimalApi.Tests.Integration.Tests.Redirect
{
    [Collection("Endpoint")]
    public class RedirectEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task ReturnsRedirect() 
        {
            //Act
            var response = await f.Client.GetAsync<TempRedirectEndpoint>();

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.TemporaryRedirect);
                
        }
    }
}
