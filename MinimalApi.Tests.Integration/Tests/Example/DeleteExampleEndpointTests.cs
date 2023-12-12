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
            var getResult = await f.Client.GetAsync<GetExampleEndpoint, GetExampleRequest, ProblemDetails>(new() 
            { 
                Id = createResult.Content.Data.Id 
            });

            //Assert
            deleteResult.Should().BeSuccessful<DeleteExampleResponse>()
                .WithStatusCode(HttpStatusCode.OK)
                .WithMessage("Example deleted successfully");

            getResult.Should().BeProblem()
                .WithStatusCode(HttpStatusCode.NotFound)
                .WithMessage("Example not found");
        }
    }
}
