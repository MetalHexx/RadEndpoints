using FluentAssertions;
using MinimalApi.Domain.Examples;
using MinimalApi.Features.Examples.SearchExamples;
using OneOf;
using RadEndpoints;

namespace MinimalApi.Tests.Unit
{
    public class SearchExamplesEndpointTests
    {
        [Fact]
        public async Task Handle_Should_Return_Examples_When_Found()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new SearchExamplesRequest { FirstName = "John", LastName = "Doe" };
            var examples = new List<Example>
        {
            new Example("John", "Doe", Id: 1),
            new Example("Jane", "Doe", Id: 2)
        };

            exampleService.FindExamples("John", "Doe")
                .Returns(Task.FromResult<OneOf<IEnumerable<Example>, NotFoundError>>(examples));

            var endpoint = EndpointFactory.CreateEndpoint<SearchExamplesEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.Data.Should().HaveCount(2);
            endpoint.Response.Data.First().FirstName.Should().Be("John");
            endpoint.Response.Data.Last().LastName.Should().Be("Doe");
            endpoint.Response.Message.Should().Be("Examples found successfully");
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_No_Examples_Exist()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new SearchExamplesRequest { FirstName = "John", LastName = "Doe" };
            var notFoundError = new NotFoundError("No examples found");

            exampleService.FindExamples("John", "Doe")
                .Returns(Task.FromResult<OneOf<IEnumerable<Example>, NotFoundError>>(notFoundError));

            var endpoint = EndpointFactory.CreateEndpoint<SearchExamplesEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().BeNull();
            endpoint.Received(1).SendProblem(notFoundError);
        }
    }
}