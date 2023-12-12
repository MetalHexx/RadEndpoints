using MinimalApi.Features.Examples.UpdateExample;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class UpdateExampleEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task Given_ExampleExists_ReturnsSuccess()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<UpdateExampleRequest>();
            updateRequest.Id = 1;

            //Act
            var r = await f.Client.PutAsync<UpdateExampleEndpoint, UpdateExampleRequest, UpdateExampleResponse>(updateRequest);

            //Assert
            r.Http.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Content.Should().BeOfType<UpdateExampleResponse>();
            r.Content.Data!.Id.Should().Be(updateRequest.Id);
            r.Content.Data.FirstName.Should().Be(updateRequest.Data.FirstName);
            r.Content.Data.LastName.Should().Be(updateRequest.Data.LastName);
            r.Content.Message.Should().Be("Example updated successfully");
        }

        [Fact]
        public async Task Given_DoesNotExist_Returns_Problem()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<UpdateExampleRequest>();
            updateRequest.Id = 999;

            //Act
            var r = await f.Client.PutAsync<UpdateExampleEndpoint, UpdateExampleRequest, ProblemDetails>(updateRequest);

            //Assert
            r.Http.StatusCode.Should().Be(HttpStatusCode.NotFound);
            r.Content.Should().BeOfType<ProblemDetails>();            
            r.Content.Title.Should().Be("Example not found");
        }

        [Fact]
        public async Task When_FirstNameEmpty_ReturnsProblem()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<UpdateExampleRequest>();
            updateRequest.Data.FirstName = string.Empty;

            //Act
            var r = await f.Client.PutAsync<UpdateExampleEndpoint, UpdateExampleRequest, ProblemDetails>(updateRequest);

            //Assert
            r.Http.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            r.Content.Should().BeOfType<ProblemDetails>();
            r.Content.Extensions.Should().ContainKey("Data.FirstName");
        }

        [Fact]
        public async Task When_LastNameEmpty_ReturnsProblem()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<UpdateExampleRequest>();
            updateRequest.Data.LastName = string.Empty;

            //Act
            var r = await f.Client.PutAsync<UpdateExampleEndpoint, UpdateExampleRequest, ProblemDetails>(updateRequest);

            //Assert
            r.Http.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            r.Content.Should().BeOfType<ProblemDetails>();
            r.Content.Extensions.Should().ContainKey("Data.LastName");
        }
    }
}