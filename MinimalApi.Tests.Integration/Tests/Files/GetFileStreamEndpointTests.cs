using MinimalApi.Features.Files.GetFileStream;
using System.Net.Mime;
using System.Reflection;

namespace MinimalApi.Tests.Integration.Tests.Files
{
    [Collection("Endpoint")]
    public class GetFileStreamEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async void When_Called_ReturnsFile()
        {
            //Arrange 
            var expectedFileName = "RadEndpoints.jpg";
            var expectedBytes = await GetFileBytes(@$"Features\Files\_common\{expectedFileName}");

            //Act            
            var r = await f.Client.GetAsync<GetFileStreamEndpoint, GetFileStreamRequest>(new());
            var actualBytes = await r.Content.ReadAsByteArrayAsync();

            //Assert
            r.StatusCode.Should().Be(HttpStatusCode.OK);
            r.Content.Headers.ContentType!.MediaType.Should().Be(MediaTypeNames.Image.Jpeg);
            r.Content.Headers.ContentDisposition!.FileName.Should().Be(expectedFileName);
            actualBytes.Should().BeEquivalentTo(expectedBytes);
        }

        private async static Task<byte[]> GetFileBytes(string relativePath)
        {
            var exeRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(exeRoot!, relativePath.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            using var fileStream = File.Open(filePath, FileMode.Open);
            var bytes = new byte[fileStream.Length];
            var _ = await fileStream.ReadAsync(bytes.AsMemory(0, (int)fileStream.Length));
            return bytes;
        }
    }
}
