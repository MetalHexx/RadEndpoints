using FluentAssertions;
using MinimalApi.Features.Files.GetFileBytes;
using System.Net.Mime;

namespace MinimalApi.Tests.Unit
{
    public class GetFileBytesEndpointTests
    {
        [Fact]
        public async Task Handle_Should_Return_FileBytesResponse_With_Correct_Values()
        {
            // Arrange
            var request = new GetFileBytesRequest();
            var endpoint = EndpointFactory.CreateEndpoint<GetFileBytesEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.Bytes.Should().NotBeEmpty();
            endpoint.Response.ContentType.Should().Be(MediaTypeNames.Image.Jpeg);
            endpoint.Response.FileDownloadName.Should().Be("RadEndpoints.jpg");
            endpoint.Response.EnableRangeProcessing.Should().BeFalse();
            endpoint.Response.LastModified.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task Handle_Should_Call_SendBytes_With_Correct_Response()
        {
            // Arrange
            var request = new GetFileBytesRequest();
            var endpoint = EndpointFactory.CreateEndpoint<GetFileBytesEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Received(1).SendBytes(Arg.Is<GetFileBytesResponse>(r =>
                r.Bytes.Length > 0 &&
                r.ContentType == MediaTypeNames.Image.Jpeg &&
                r.FileDownloadName == "RadEndpoints.jpg" &&
                r.EnableRangeProcessing == false));
        }
    }
}