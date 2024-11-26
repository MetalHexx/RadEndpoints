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
            var request = new PostFormRequest { TestFormField = "Test" };

            //Act
            var response = await f.Client.PostAsync<PostFormEndpoint, PostFormRequest, PostFormResponse>(request);

            //Assert
            response.Should().BeSuccessful<PostFormResponse>()
                .WithStatusCode(HttpStatusCode.OK);
        }
    }
}
