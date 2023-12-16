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
            r.Should().BeSuccessful<GetExampleResponse>()
                .WithStatusCode(HttpStatusCode.OK)
                .WithMessage("Example retrieved successfully");
          
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
            r.Should().BeProblem()
                .WithStatusCode(HttpStatusCode.NotFound)
                .WithMessage("Example not found");
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
            r.Should().BeProblem()
                .WithStatusCode(HttpStatusCode.BadRequest)
                .WithKey("Id");
        }
    }
}