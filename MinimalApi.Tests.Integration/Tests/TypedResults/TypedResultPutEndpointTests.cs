using MinimalApi.Features.WithTypedResults.TypedResultsPut;
using MinimalApi.Features.Examples.CreateExample;
using MinimalApi.Features.Examples.GetExample;

namespace MinimalApi.Tests.Integration.Tests.CustomExamples
{
    [Collection("Endpoint")]
    public class TypedResultPutEndpointTests(RadEndpointFixture f): RadEndpointFixture
    {
        [Fact]
        public async Task Given_ExampleExists_When_RequestValid_ReturnsSuccess()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<CustomPutRequest>();
            updateRequest.Id = 1;

            //Act
            var r = await f.Client.PutAsync<TypedResultsPutEndpoint, CustomPutRequest, CustomPutResponse>(updateRequest);

            //Assert
            r.Should().BeSuccessful<CustomPutResponse>()
                .WithStatusCode(HttpStatusCode.OK)
                .WithContentNotNull();

            r.Content.Message.Should().Be("Example updated successfully");
        }

        [Fact]
        public async Task Given_ExampleDoesntExist_ReturnsNotFound()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<CustomPutRequest>();
            updateRequest.Id = 999;

            //Act
            var r = await f.Client.PutAsync<TypedResultsPutEndpoint, CustomPutRequest>(updateRequest);

            //Assert
            r.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]        
        public async Task When_FirstNameEmpty_ReturnsProblem()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<CustomPutRequest>();
            updateRequest.Data.FirstName = string.Empty;

            //Act
            var r = await f.Client.PutAsync<TypedResultsPutEndpoint, CustomPutRequest, ValidationProblemDetails>(updateRequest);

            //Assert
            r.Should().BeValidationProblem()
                .WithStatusCode(HttpStatusCode.BadRequest)
                .WithTitle("Validation Error")
                .WithKey("Data.FirstName");
        }

        [Fact]
        public async Task When_LastNameEmpty_ReturnsProblem()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<CustomPutRequest>();
            updateRequest.Data.LastName = string.Empty;

            //Act
            var r = await f.Client.PutAsync<TypedResultsPutEndpoint, CustomPutRequest, ValidationProblemDetails>(updateRequest);

            //Assert
            r.Should().BeValidationProblem()
                .WithStatusCode(HttpStatusCode.BadRequest)
                .WithTitle("Validation Error")
                .WithKey("Data.LastName");
        }

        [Fact]
        public async Task Given_ExampleExists_And_UpdateWithSameName_ReturnsConflict()
        {
            //Arrange            
            var getResponse = await f.Client.GetAsync<GetExampleEndpoint, GetExampleRequest, GetExampleResponse>(new() { Id = 1 });
            var createRequest = f.DataGenerator.Create<CreateExampleRequest>();
            var createResponse = await f.Client.PostAsync<CreateExampleEndpoint, CreateExampleRequest, CreateExampleResponse>(createRequest);

            //Act
            var r = await f.Client.PutAsync<TypedResultsPutEndpoint, CustomPutRequest, ProblemDetails>(new CustomPutRequest
            {
                Id = createResponse.Content.Data!.Id,
                Data = new CustomPutDto 
                { 
                    FirstName = getResponse.Content.Data!.FirstName, 
                    LastName = getResponse.Content.Data!.LastName 
                }
            });

            //Assert
            r.Should().BeProblem()
                .WithStatusCode(HttpStatusCode.Conflict)
                .WithMessage("Example with the same first and last name already exists");
        }
    }
}