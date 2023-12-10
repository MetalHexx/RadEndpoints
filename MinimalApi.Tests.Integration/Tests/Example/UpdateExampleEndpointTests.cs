using MinimalApi.Features.Examples.CreateExample;
using MinimalApi.Features.Examples.UpdateExample;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class UpdateExampleEndpointTests(EndpointFixture f)

    {
        [Fact]
        public async Task Given_ExampleExists_When_Called_Returns_Success()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<UpdateExampleRequest>();
            updateRequest.Id = 1;

            //Act
            var (h, r) = await f.Client.PutAsync<UpdateExampleEndpoint, UpdateExampleRequest, UpdateExampleResponse>(updateRequest);

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Should().BeOfType<UpdateExampleResponse>();
            r!.Data!.Id.Should().Be(updateRequest.Id);
            r!.Data!.FirstName.Should().Be(updateRequest.FirstName);
            r!.Data!.LastName.Should().Be(updateRequest.LastName);
            r!.Message.Should().Be("Example updated successfully");
        }

        [Fact]
        public async Task Given_DoesNotExist_When_Called_Returns_NotFound()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<UpdateExampleRequest>();
            updateRequest.Id = 999;

            //Act
            var (h, r) = await f.Client.PutAsync<UpdateExampleEndpoint, UpdateExampleRequest, ProblemDetails>(updateRequest);

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.NotFound);
            r.Should().BeOfType<ProblemDetails>();            
            r!.Title.Should().Be("Example not found");
        }

        [Fact]
        public async Task When_Called_With_FirstNameEmpty_Returns_BadRequest()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<UpdateExampleRequest>();
            updateRequest.FirstName = string.Empty;

            //Act
            var (h, r) = await f.Client.PutAsync<UpdateExampleEndpoint, UpdateExampleRequest, ProblemDetails>(updateRequest);

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            r.Should().BeOfType<ProblemDetails>();
            r!.Extensions.Should().ContainKey("FirstName");
        }

        [Fact]
        public async Task When_Called_With_LastNameEmpty_Returns_BadRequest()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<UpdateExampleRequest>();
            updateRequest.LastName = string.Empty;

            //Act
            var (h, r) = await f.Client.PutAsync<UpdateExampleEndpoint, UpdateExampleRequest, ProblemDetails>(updateRequest);

            //Assert
            h.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            r.Should().BeOfType<ProblemDetails>();
            r!.Extensions.Should().ContainKey("LastName");
        }
    }
}
