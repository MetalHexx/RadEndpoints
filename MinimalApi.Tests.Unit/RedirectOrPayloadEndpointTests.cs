using FluentAssertions;
using MinimalApi.Features.Redirect.WithPayload.RedirectWithPayloadEndpoint;

namespace MinimalApi.Tests.Unit
{
    public class RedirectOrPayloadEndpointTests
    {
        [Fact]
        public async Task Handle_Should_Return_Response_When_Not_Redirecting()
        {
            // Arrange
            var request = new RedirectOrPayloadRequest { ShouldRedirect = false };
            var endpoint = EndpointFactory.CreateEndpoint<RedirectOrPayloadEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_Should_Trigger_Redirect_When_ShouldRedirect_Is_True()
        {
            // Arrange
            var request = new RedirectOrPayloadRequest { ShouldRedirect = true };
            var endpoint = EndpointFactory.CreateEndpoint<RedirectOrPayloadEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Received(1).SendRedirect("http://fake.url/", permanent: false, preserveMethod: false);
            endpoint.Response.Should().BeNull();
        }
    }
}