using MinimalApi.Features.Examples.CreateExample;
using MinimalApi.Features.Examples.GetExample;
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
            r.Should().BeSuccessful<UpdateExampleResponse>()
                .WithStatusCode(HttpStatusCode.OK);

            r.Content.Message.Should().Be("Example updated successfully");
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
            r.Should().BeProblem()
                .WithStatusCode(HttpStatusCode.NotFound)
                .WithMessage("Example not found");
        }

        [Fact]
        public async Task When_FirstNameEmpty_ReturnsProblem()
        {
            //Arrange
            var updateRequest = f.DataGenerator.Create<UpdateExampleRequest>();
            updateRequest.Data.FirstName = string.Empty;

            //Act
            var r = await f.Client.PutAsync<UpdateExampleEndpoint, UpdateExampleRequest, ValidationProblemDetails>(updateRequest);

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
            var updateRequest = f.DataGenerator.Create<UpdateExampleRequest>();
            updateRequest.Data.LastName = string.Empty;

            //Act
            var r = await f.Client.PutAsync<UpdateExampleEndpoint, UpdateExampleRequest, ValidationProblemDetails>(updateRequest);

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
            var r = await f.Client.PutAsync<UpdateExampleEndpoint, UpdateExampleRequest, ProblemDetails>(new UpdateExampleRequest
            {
                Id = createResponse.Content.Data!.Id,
                Data = new ExampleUpdateDto
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