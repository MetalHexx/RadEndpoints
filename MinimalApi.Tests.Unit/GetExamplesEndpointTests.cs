using FluentAssertions;
using MinimalApi.Domain.Examples;
using MinimalApi.Features.Examples.GetExamples;
using OneOf;
using RadEndpoints;

namespace MinimalApi.Tests.Unit
{
    public class GetExamplesEndpointTests
    {
        [Fact]
        public async Task Handle_Should_Return_Examples_When_They_Exist()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new GetExamplesRequest();
            var examples = new List<Example>
        {
            new Example("John", "Doe", Id: 1),
            new Example("Jane", "Doe", Id: 2)
        };

            exampleService.GetExamples()
                .Returns(Task.FromResult<OneOf<IEnumerable<Example>, NotFoundError>>(examples));

            var endpoint = EndpointFactory.CreateEndpoint<GetExamplesEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.Data.Should().HaveCount(2);
            endpoint.Response.Data.First().FirstName.Should().Be("John");
            endpoint.Response.Data.Last().LastName.Should().Be("Doe");
            endpoint.Response.Message.Should().Be("Examples retrieved successfully");
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_No_Examples_Exist()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new GetExamplesRequest();
            var notFoundError = new NotFoundError("No examples found");

            exampleService.GetExamples()
                .Returns(Task.FromResult<OneOf<IEnumerable<Example>, NotFoundError>>(notFoundError));

            var endpoint = EndpointFactory.CreateEndpoint<GetExamplesEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().BeNull();
            endpoint.Received(1).SendProblem(notFoundError);
        }
    }
}