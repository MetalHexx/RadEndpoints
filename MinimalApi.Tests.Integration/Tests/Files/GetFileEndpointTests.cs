using MinimalApi.Features.Files;
using System.Reflection;

namespace MinimalApi.Tests.Integration.Tests.Files
{
    [Collection("Endpoint")]
    public class GetFileEndpointTests(RadEndpointFixture f)
    {
        [Fact]
        public async void When_RequestValid_ReturnsSuccess()
        {
            //Arrange 
            var expectedBytes = await GetFileBytes(@"Features\File\RadEndpoints.jpg");

            //Act            
            var r = await f.Client.GetAsync<GetFileEndpoint, GetFileRequest>(new());
            var actualBytes = await r.Content.ReadAsByteArrayAsync();

            //Assert
            r.StatusCode.Should().Be(HttpStatusCode.OK);
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
