using AutoFixture;
using MinimalApi.Features.Examples.CreateExample;
using MinimalApi.Features.Examples.DeleteExample;
using MinimalApi.Features.Examples.GetExample;
using MinimalApi.Tests.Integration.Common;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class DeleteExampleEndpointTests
    {
        private readonly EndpointFixture _fixture;
        public DeleteExampleEndpointTests(EndpointFixture f) => _fixture = f;

        /// <summary>
        /// Complex functional use case test that creates an example, then deletes it and verifies it was deleted.
        /// </summary>
        [Fact]
        public async Task Given_ExampleExists_When_Deleted_ReturnsSuccess()
        {
            //Arrange 
            var createRequest = _fixture.DataGenerator.Create<CreateExampleRequest>();
            var (_, createResponse) = await _fixture.Client.PostAsync<CreateExampleEndpoint, CreateExampleRequest, CreateExampleResponse>(createRequest);
            var deleteRequest = new DeleteExampleRequest { Id = createResponse!.Data!.Id };

            //Act
            var (deleteHttpRequest, deleteResponse) = await _fixture.Client.DeleteAsync<DeleteExampleEndpoint, DeleteExampleRequest, DeleteExampleResponse>(deleteRequest);
            var (getHttpResponse, _) = await _fixture.Client.GetAsync<GetExampleEndpoint, GetExampleRequest, GetExampleResponse>(new() 
            { 
                Id = createResponse.Data.Id 
            });

            //Assert
            deleteHttpRequest.StatusCode.Should().Be(HttpStatusCode.OK);
            deleteResponse.Should().NotBeNull();
            deleteResponse!.Message.Should().Be("Example deleted successfully");
            getHttpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
