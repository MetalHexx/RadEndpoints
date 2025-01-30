using FluentAssertions;
using MinimalApi.Domain.Examples;
using MinimalApi.Features.Examples.GetExampleChild;
using OneOf;
using RadEndpoints;

namespace MinimalApi.Tests.Unit
{
    public class SearchChildExampleEndpointTests
    {
        [Fact]
        public async Task Handle_Should_Return_Children_When_Found()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new SearchChildExampleRequest { ParentId = 1, FirstName = "John", LastName = "Doe" };
            var children = new List<Example>
        {
            new Example("John", "Doe", Id: 2, ParentId: 1),
            new Example("Jane", "Doe", Id: 3, ParentId: 1)
        };

            exampleService.SearchChildExample(1, "John", "Doe")
                .Returns(Task.FromResult<OneOf<IEnumerable<Example>, NotFoundError>>(children));

            var endpoint = EndpointFactory.CreateEndpoint<SearchChildExampleEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.Data.Should().HaveCount(2);
            endpoint.Response.Data.First().FirstName.Should().Be("John");
            endpoint.Response.Data.Last().LastName.Should().Be("Doe");
            endpoint.Response.Message.Should().Be("Children found");
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_No_Children_Exist()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new SearchChildExampleRequest { ParentId = 1, FirstName = "John", LastName = "Doe" };
            var notFoundError = new NotFoundError("No children found");

            exampleService.SearchChildExample(1, "John", "Doe")
                .Returns(Task.FromResult<OneOf<IEnumerable<Example>, NotFoundError>>(notFoundError));

            var endpoint = EndpointFactory.CreateEndpoint<SearchChildExampleEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().BeNull();
            endpoint.Received(1).SendProblem(notFoundError);
        }
    }
}