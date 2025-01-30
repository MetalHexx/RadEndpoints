using FluentAssertions;
using MinimalApi.Features.Environment.GetEnvironment;

namespace MinimalApi.Tests.Unit
{
    public class GetEnvironmentEndpointTests
    {
        [Fact]
        public async Task Handle_Should_Set_Response_With_Factory_EnvironmentValues()
        {
            // Arrange
            var endpoint = EndpointFactory.CreateEndpoint<GetEnvironmentEndpoint>();

            // Act
            await endpoint.Handle(CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.ApplicationName.Should().Be("TestApp");
            endpoint.Response.EnvironmentName.Should().Be("Development");
            endpoint.Response.Message.Should().Be("Environment information retrieved successfully");
        }
    }
}