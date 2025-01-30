using FluentAssertions;
using MinimalApi.Domain.Examples;
using MinimalApi.Features.Environment.GetApplicationName;
using MinimalApi.Features.Examples.GetExample;
using OneOf;
using RadEndpoints;

namespace MinimalApi.Tests.Unit
{
    public class FactoryUnitTests
    {
        [Fact]
        public async Task When_TestFactoryUsed_WithoutConstructorParams_SendCalled()
        {
            var endpoint = EndpointFactory.CreateEndpoint<GetApplicationNameEndpoint>();
            await endpoint.Handle(CancellationToken.None);
            endpoint.Received(1).Send();
        }

        [Fact]
        public async Task When_TestFactoryUsed_WithConstructorParams_Response_Has_CorrectData()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();
            var example = new Example("John", "Doe", Id: 1);

            exampleService.GetExample(1).Returns(Task.FromResult<OneOf<Example, NotFoundError>>(example));

            var endpoint = EndpointFactory.CreateEndpoint<GetExampleEndpoint>(exampleService);

            var request = new GetExampleRequest { Id = 1 };

            // Act
            await endpoint.Handle(request, CancellationToken.None);


            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.Data.FirstName.Should().Be("John");
            endpoint.Response.Data.LastName.Should().Be("Doe");
            endpoint.Response.Message.Should().Be("Example retrieved successfully");
        }

        [Fact]
        public async Task When_TestFactoryUsed_WithConstructorParams_SendProblemCalled_WithNotFound()
        {
            // Arrange
            var exampleService = Substitute.For<IExampleService>();

            exampleService.GetExample(1).Returns(Task.FromResult<OneOf<Example, NotFoundError>>(new NotFoundError("Example not found")));

            var endpoint = EndpointFactory.CreateEndpoint<GetExampleEndpoint>(exampleService);

            var request = new GetExampleRequest { Id = 1 };

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Received(1).SendProblem(Arg.Any<NotFoundError>());
        }

    }
}