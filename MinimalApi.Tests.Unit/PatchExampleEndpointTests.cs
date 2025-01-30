using FluentAssertions;
using MinimalApi.Domain.Examples;
using MinimalApi.Features.Examples._common;
using MinimalApi.Features.Examples.PatchExample;
using OneOf;
using RadEndpoints;

namespace MinimalApi.Tests.Unit
{
    public class PatchExampleEndpointTests
    {
        [Fact]
        public async Task Handle_Should_Patch_Example_And_Return_Updated_Response()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new PatchExampleRequest
            {
                Id = 1,
                Example = new ExampleDto { FirstName = "UpdatedName", LastName = "Doe" }
            };
            var patchedExample = new Example("UpdatedName", "Doe", Id: 1);

            exampleService.PatchExample(1, Arg.Any<Example>())
                .Returns(Task.FromResult<OneOf<Example, NotFoundError, ConflictError>>(patchedExample));

            var endpoint = EndpointFactory.CreateEndpoint<PatchExampleEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.Id.Should().Be(1);
            endpoint.Response.FirstName.Should().Be("UpdatedName");
            endpoint.Response.LastName.Should().Be("Doe");
        }

        [Fact]
        public async Task Handle_Should_Return_NotFound_When_Example_Does_Not_Exist()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new PatchExampleRequest
            {
                Id = 1,
                Example = new ExampleDto { FirstName = "UpdatedName", LastName = "Doe" }
            };
            var notFoundError = new NotFoundError("Example not found");

            exampleService.PatchExample(1, Arg.Any<Example>())
                .Returns(Task.FromResult<OneOf<Example, NotFoundError, ConflictError>>(notFoundError));

            var endpoint = EndpointFactory.CreateEndpoint<PatchExampleEndpoint>(exampleService);

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
            var request = new PatchExampleRequest
            {
                Id = 1,
                Example = new ExampleDto { FirstName = "UpdatedName", LastName = "Doe" }
            };
            var conflictError = new ConflictError("Conflict updating example");

            exampleService.PatchExample(1, Arg.Any<Example>())
                .Returns(Task.FromResult<OneOf<Example, NotFoundError, ConflictError>>(conflictError));

            var endpoint = EndpointFactory.CreateEndpoint<PatchExampleEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().BeNull();
            endpoint.Received(1).SendProblem(conflictError);
        }
    }
}