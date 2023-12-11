using MinimalApi.Features.Examples.CreateExample;

namespace MinimalApi.Tests.Integration.Tests.Example
{
    [Collection("Endpoint")]
    public class CreateExampleEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async Task Given_ExampleDoesNotExist_ReturnsSuccess()
        {
            //Arrange
            var createRequest = f.DataGenerator.Create<CreateExampleRequest>();

            //Act
            var (h, r) = await f.Client.PostAsync<CreateExampleEndpoint, CreateExampleRequest, CreateExampleResponse>(createRequest);

            //Assert
            h.Should().BeSuccessful();
            r.Should().BeRadResponse<CreateExampleResponse>().WithMessage("Example created successfully");
            r!.Data!.Id.Should().BeGreaterThan(0);
            r!.Data!.FirstName.Should().Be(createRequest.FirstName);
            r!.Data!.LastName.Should().Be(createRequest.LastName);
        }

        [Fact]
        public async Task Given_ExampleExists_ReturnsProblem()
        {
            //Arrange
            var createRequest = f.DataGenerator.Create<CreateExampleRequest>();

            //Act
            var (_, _) = await f.Client.PostAsync<CreateExampleEndpoint, CreateExampleRequest, CreateExampleResponse>(createRequest);
            var (h, r) = await f.Client.PostAsync<CreateExampleEndpoint, CreateExampleRequest, ProblemDetails>(createRequest);

            //Assert
            h.Should().HaveClientError();
            r.Should().BeProblem().WithTitle("An example with the same first and last name already exists");
        }

        [Fact]
        public async Task When_FirstNameEmpty_ReturnsProblem()
        {
            //Arrange
            var createRequest = f.DataGenerator.Create<CreateExampleRequest>();
            createRequest.FirstName = string.Empty;

            //Act
            var (h, r) = await f.Client.PostAsync<CreateExampleEndpoint, CreateExampleRequest, ProblemDetails>(createRequest);

            //Assert
            h.Should().HaveClientError();
            r.Should().BeProblem().WithKey("FirstName");
        }

        [Fact]
        public async Task When_LastNameEmpty_ReturnsProblem()
        {
            //Arrange
            var createRequest = f.DataGenerator.Create<CreateExampleRequest>();
            createRequest.LastName = string.Empty;

            //Act
            var (h, r) = await f.Client.PostAsync<CreateExampleEndpoint, CreateExampleRequest, ProblemDetails>(createRequest);

            //Assert
            h.Should().HaveClientError();
            r.Should().BeProblem().WithTitle("Validation Error").WithKey("LastName");
        }
    }
}