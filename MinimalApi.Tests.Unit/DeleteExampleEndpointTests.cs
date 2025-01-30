using FluentAssertions;
using MinimalApi.Domain.Examples;
using MinimalApi.Features.Examples.DeleteExample;
using OneOf;
using OneOf.Types;
using RadEndpoints;

namespace MinimalApi.Tests.Unit
{
    public class DeleteExampleEndpointTests
    {
        [Fact]
        public async Task Handle_Should_Delete_Example_And_Return_Success_Response()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new DeleteExampleRequest { Id = 1 };

            exampleService.DeleteExample(1)
                .Returns(Task.FromResult<OneOf<None, NotFoundError>>(new None()));

            var endpoint = EndpointFactory.CreateEndpoint<DeleteExampleEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.Message.Should().Be("Example deleted successfully");
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Example_Does_Not_Exist()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new DeleteExampleRequest { Id = 1 };
            var notFoundError = new NotFoundError("Example not found");

            exampleService.DeleteExample(1)
                .Returns(Task.FromResult<OneOf<None, NotFoundError>>(notFoundError));

            var endpoint = EndpointFactory.CreateEndpoint<DeleteExampleEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().BeNull();
            endpoint.Received(1).SendProblem(notFoundError);
        }
    }
}