using MinimalApi.Features.Examples.CreateExample;
using MinimalApi.Features.Examples.GetExample;
using MinimalApi.Features.Examples.PatchExample;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class PatchExampleEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task Given_ExampleExists_ReturnsSuccess()
        {
            //Arrange
            var request = f.DataGenerator.Create<PatchExampleRequest>();
            request.Id = 1;

            //Act
            var r = await f.Client.PatchAsync<PatchExampleEndpoint, PatchExampleRequest, PatchExampleResponse>(request);

            //Assert
            r.Should().BeSuccessful<PatchExampleResponse>()
                .WithStatusCode(HttpStatusCode.OK);

            r.Content.Id.Should().Be(request.Id);
            r.Content.FirstName.Should().Be(request.Example.FirstName);
        }

        [Fact]
        public async Task Given_DoesNotExist_Returns_Problem()
        {
            //Arrange
            var request = f.DataGenerator.Create<PatchExampleRequest>();
            request.Id = 999;

            //Act
            var r = await f.Client.PatchAsync<PatchExampleEndpoint, PatchExampleRequest, ProblemDetails>(request);

            //Assert
            r.Should().BeProblem()
                .WithStatusCode(HttpStatusCode.NotFound)
                .WithMessage("Example not found");
        }

        [Fact]
        public async Task Given_ExampleExists_And_UpdateWithSameName_ReturnsConflict()
        {   
            //Act
            var r = await f.Client.PatchAsync<PatchExampleEndpoint, PatchExampleRequest, ProblemDetails>(new PatchExampleRequest
            {
                Id = 7,
                Example = new() 
                { 
                    FirstName = "Luke"
                }
            });

            //Assert
            r.Should().BeProblem()
                .WithStatusCode(HttpStatusCode.Conflict)
                .WithMessage("Example with the same first and last name already exists");
        }
    }
}