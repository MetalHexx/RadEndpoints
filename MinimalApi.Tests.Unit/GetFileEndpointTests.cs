using FluentAssertions;
using MinimalApi.Features.Files.GetFile;
using System.Net.Mime;

namespace MinimalApi.Tests.Unit
{
    public class GetFileEndpointTests
    {
        [Fact]
        public async Task Handle_Should_Return_FileResponse_With_Correct_Values()
        {
            // Arrange
            var request = new GetFileRequest();
            var endpoint = EndpointFactory.CreateEndpoint<GetFileEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Response.Should().NotBeNull();
            endpoint.Response!.Path.Should().EndWith(@"Features\Files\_common\RadEndpoints.jpg");
            endpoint.Response.ContentType.Should().Be(MediaTypeNames.Image.Jpeg);
            endpoint.Response.FileDownloadName.Should().Be("RadEndpoints.jpg");
            endpoint.Response.LastModified.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task Handle_Should_Call_SendFile_With_Correct_Response()
        {
            // Arrange
            var request = new GetFileRequest();
            var endpoint = EndpointFactory.CreateEndpoint<GetFileEndpoint>();

            // Act
            await endpoint.Handle(request, CancellationToken.None);

            // Assert
            endpoint.Received(1).SendFile(Arg.Is<GetFileResponse>(r =>
                r.Path.EndsWith(@"Features\Files\_common\RadEndpoints.jpg") &&
                r.ContentType == MediaTypeNames.Image.Jpeg &&
                r.FileDownloadName == "RadEndpoints.jpg"));
        }
    }
}