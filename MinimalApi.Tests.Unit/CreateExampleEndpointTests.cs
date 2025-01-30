using FluentAssertions;
using MinimalApi.Domain.Examples;
using MinimalApi.Features.Examples.CreateExample;
using OneOf;
using RadEndpoints;

namespace MinimalApi.Tests.Unit
{
    public class CreateExampleEndpointTests
    {
        [Fact]
        public async Task Handle_Should_Create_Example_And_Return_Correct_Response()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new CreateExampleRequest { FirstName = "John", LastName = "Doe" };
            var createdExample = new Example("John", "Doe", Id: 1);

            exampleService.InsertExample(Arg.Any<Example>())
                .Returns(Task.FromResult<OneOf<Example, ConflictError>>(createdExample));

            var endpoint = EndpointFactory.CreateEndpoint<CreateExampleEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.Data!.Id.Should().Be(1);
            endpoint.Response.Data.FirstName.Should().Be("John");
            endpoint.Response.Data.LastName.Should().Be("Doe");
            endpoint.Response.Message.Should().Be("Example created successfully");
        }

        [Fact]
        public async Task Handle_Should_Return_Conflict_When_Example_Already_Exists()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var request = new CreateExampleRequest { FirstName = "John", LastName = "Doe" };
            var conflictError = new ConflictError("An example with the same name already exists");

            exampleService.InsertExample(Arg.Any<Example>())
                .Returns(Task.FromResult<OneOf<Example, ConflictError>>(conflictError));

            var endpoint = EndpointFactory.CreateEndpoint<CreateExampleEndpoint>(exampleService);

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().BeNull();
            endpoint.Received(1).SendProblem(conflictError);
        }
    }
}