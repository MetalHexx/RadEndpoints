using MinimalApi.Features.Examples.CreateExample;
using MinimalApi.Features.Examples.DeleteExample;
using MinimalApi.Features.Examples.GetExample;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class DeleteExampleEndpointTests(RadEndpointFixture f)
    {
        /// <summary>
        /// Complex functional use case test that creates an example, then deletes it and verifies it was deleted.
        /// </summary>
        [Fact]
        public async Task Given_ExampleExists_ReturnsSuccess()
        {
            //Arrange 
            var createRequest = f.DataGenerator.Create<CreateExampleRequest>();
            var createResult = await f.Client.PostAsync<CreateExampleEndpoint, CreateExampleRequest, CreateExampleResponse>(createRequest);
            var deleteRequest = new DeleteExampleRequest { Id = createResult.Content.Data!.Id };

            //Act
            var deleteResult = await f.Client.DeleteAsync<DeleteExampleEndpoint, DeleteExampleRequest, DeleteExampleResponse>(deleteRequest);
            var getResult = await f.Client.GetAsync<GetExampleEndpoint, GetExampleRequest, GetExampleResponse>(new() 
            { 
                Id = createResult.Content.Data.Id 
            });

            //Assert
            deleteResult.Http.StatusCode.Should().Be(HttpStatusCode.OK);
            deleteResult.Content.Should().BeOfType<DeleteExampleResponse>();
            deleteResult.Content.Message.Should().Be("Example deleted successfully");
            getResult.Http.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
