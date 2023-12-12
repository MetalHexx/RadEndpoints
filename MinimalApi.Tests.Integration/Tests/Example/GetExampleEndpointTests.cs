using MinimalApi.Features.Examples.GetExample;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class GetExampleEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async void When_RequestValid_ReturnsSuccess()
        {
            //Act            
            var r = await f.Client.GetAsync<GetExampleEndpoint, GetExampleRequest, GetExampleResponse>(new()
            {
                Id = 1
            });

            //Assert
            r.Http.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Content.Should().BeOfType<GetExampleResponse>();           
            r.Content.Data!.Id.Should().Be(1);
        }

        [Fact]
        public async void Given_ExampleNonExistant_ReturnsProblem()
        {
            //Act            
            var r = await f.Client.GetAsync<GetExampleEndpoint, GetExampleRequest, ProblemDetails>(new()
            {
                Id = 999
            });

            //Assert
            r.Http.StatusCode.Should().Be(HttpStatusCode.NotFound);
            r.Content.Should().BeOfType<ProblemDetails>();
            r.Content.Title.Should().Be("Example not found");
        }
        [Fact]
        public async void When_IdInvalid_ReturnsProblem()
        {
            //Act            
            var r = await f.Client.GetAsync<GetExampleEndpoint, GetExampleRequest, ProblemDetails>(new()
            {
                Id = 0
            });

            //Assert
            r.Http.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            r.Content.Should().BeOfType<ProblemDetails>();
            r.Content.Extensions.Should().ContainKey("Id");
        }
    }
}