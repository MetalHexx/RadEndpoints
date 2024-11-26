using MinimalApi.Features.Forms.PostForm;

namespace MinimalApi.Tests.Integration.Tests.Forms
{
    [Collection("Endpoint")]
    public class FormPostEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task When_FormPosted_ReturnsSuccess() 
        {
            //Arrange
            var expectedHeaderValue = "http://login.dev.radancy.net/";
            var request = new PostFormRequest { TestFormField = "Test" };
            RadHttpClientOptions options = new();
            options.Headers.Add("Referer", expectedHeaderValue);

            //Act
            var response = await f.Client.PostAsync<PostFormEndpoint, PostFormRequest, PostFormResponse>(request, options);

            //Assert
            response.Should().BeSuccessful<PostFormResponse>()
                .WithStatusCode(HttpStatusCode.OK);
            response.Content.HeaderValue.Should().Be(expectedHeaderValue);
        }
    }
}
