using MinimalApi.Features.Examples.CreateExample;
using MinimalApi.Features.Examples.DeleteExample;
using MinimalApi.Features.Examples.GetExample;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class DeleteExampleEndpointTests(EndpointFixture f)
    {
        /// <summary>
        /// Complex functional use case test that creates an example, then deletes it and verifies it was deleted.
        /// </summary>
        [Fact]
        public async Task Given_ExampleExists_ReturnsSuccess()
        {
            //Arrange 
            var createRequest = f.DataGenerator.Create<CreateExampleRequest>();
            var (_, createResponse) = await f.Client.PostAsync<CreateExampleEndpoint, CreateExampleRequest, CreateExampleResponse>(createRequest);
            var deleteRequest = new DeleteExampleRequest { Id = createResponse!.Data!.Id };

            //Act
            var (deleteHttpRequest, deleteResponse) = await f.Client.DeleteAsync<DeleteExampleEndpoint, DeleteExampleRequest, DeleteExampleResponse>(deleteRequest);
            var (getHttpResponse, _) = await f.Client.GetAsync<GetExampleEndpoint, GetExampleRequest, GetExampleResponse>(new() 
            { 
                Id = createResponse.Data.Id 
            });

            //Assert
            deleteHttpRequest.StatusCode.Should().Be(HttpStatusCode.OK);
            deleteResponse.Should().BeOfType<DeleteExampleResponse>();
            deleteResponse!.Message.Should().Be("Example deleted successfully");
            getHttpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
