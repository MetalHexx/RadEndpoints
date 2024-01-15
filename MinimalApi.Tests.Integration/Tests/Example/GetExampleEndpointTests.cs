using MinimalApi.Features.Examples.GetExample;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class GetExampleEndpointTests(RadEndpointFixture f)
    {
        // This test is implemented only to demonstrate an issue, and it is not meant to stay part of the suite.
        [Fact]
        public async void When_EndpointDoesNotUseSend_ReturnsCorrectResponse()
        {
            //Act            

            // GetExampleEndpoint has a 'special' part of the code, which returns "Rando" as a First Name
            // when ID is in the request set to 99. 
            var r = await f.Client.GetAsync<GetExampleEndpoint, GetExampleRequest, GetExampleResponse>(new()
            {
                Id = 99
            });

            //Assert
            r.Should().BeSuccessful<GetExampleResponse>();            
          
            // This assertion tries to assert that the name is Rando, as expected, but
            // it wont be. This happens when endpoint does not use Send to response back, but
            // the current implementation will send the Response back anyway.
            r.Content.Data!.FirstName.Should().Be("Rando");
        }
        
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
            var r = await f.Client.GetAsync<GetExampleEndpoint, GetExampleRequest, ValidationProblemDetails>(new()
            {
                Id = 0
            });

            //Assert
            r.Should().BeValidationProblem()
                .WithStatusCode(HttpStatusCode.BadRequest)
                .WithKey("Id");
        }
    }
}