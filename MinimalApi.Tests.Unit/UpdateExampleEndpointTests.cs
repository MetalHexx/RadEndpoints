using FluentAssertions;
using MinimalApi.Domain.Examples;
using MinimalApi.Features.Examples.UpdateExample;
using OneOf;
using RadEndpoints;

namespace MinimalApi.Tests.Unit
{
    public class UpdateExampleEndpointTests
    {
        [Fact]
        public async Task Handle_Should_Update_Example_And_Return_Updated_Response()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new UpdateExampleRequest
            {
                Id = 1,
                Data = new ExampleUpdateDto { FirstName = "UpdatedName", LastName = "Doe" }
            };
            var updatedExample = new Example("UpdatedName", "Doe", Id: 1);

            exampleService.UpdateExample(Arg.Any<Example>())
                .Returns(Task.FromResult<OneOf<Example, NotFoundError, ConflictError>>(updatedExample));

            var endpoint = EndpointFactory.CreateEndpoint<UpdateExampleEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.Data.Id.Should().Be(1);
            endpoint.Response.Data.FirstName.Should().Be("UpdatedName");
            endpoint.Response.Data.LastName.Should().Be("Doe");
            endpoint.Response.Message.Should().Be("Example updated successfully");
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Example_Does_Not_Exist()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new UpdateExampleRequest
            {
                Id = 1,
                Data = new ExampleUpdateDto { FirstName = "UpdatedName", LastName = "Doe" }
            };
            var notFoundError = new NotFoundError("Example not found");

            exampleService.UpdateExample(Arg.Any<Example>())
                .Returns(Task.FromResult<OneOf<Example, NotFoundError, ConflictError>>(notFoundError));

            var endpoint = EndpointFactory.CreateEndpoint<UpdateExampleEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().BeNull();
            endpoint.Received(1).SendProblem(notFoundError);
        }

        [Fact]
        public async Task Handle_Should_Return_Conflict_When_Example_Update_Causes_Conflict()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new UpdateExampleRequest
            {
                Id = 1,
                Data = new ExampleUpdateDto { FirstName = "UpdatedName", LastName = "Doe" }
            };
            var conflictError = new ConflictError("Conflict updating example");

            exampleService.UpdateExample(Arg.Any<Example>())
                .Returns(Task.FromResult<OneOf<Example, NotFoundError, ConflictError>>(conflictError));

            var endpoint = EndpointFactory.CreateEndpoint<UpdateExampleEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().BeNull();
            endpoint.Received(1).SendProblem(conflictError);
        }
    }
}