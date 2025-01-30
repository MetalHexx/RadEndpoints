using FluentAssertions;
using MinimalApi.Features.WithoutRequest.PostWithoutRequest;

namespace MinimalApi.Tests.Unit
{
    public class PostWithoutRequestEndpointTests
    {
        [Fact]
        public async Task Handle_Should_Set_Response_With_Success_Message()
        {
            // Arrange
            var endpoint = EndpointFactory.CreateEndpoint<PostWithoutRequestEndpoint>();

            // Act
            await endpoint.Handle(CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.Message.Should().Be("Success!");
        }
    }
}