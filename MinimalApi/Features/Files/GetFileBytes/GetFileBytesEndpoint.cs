using System.Net.Mime;
using System.Reflection;

namespace MinimalApi.Features.Files.GetFileBytes
{
    public class GetFileBytesEndpoint : RadEndpoint<GetFileBytesRequest, GetFileBytesResponse>
    {
        public override void Configure()
        {
            Get("/files/bytes")
                .Produces<byte[]>(StatusCodes.Status200OK, MediaTypeNames.Image.Jpeg)
                .WithDocument(tag: "Files", desc: "Example of how to send a file when you already have the bytes in memory.  \r\n\r\n  We're reading from the disk for convenience, but the specific demonstration here is the return of type bytes array.  In real world usage, the bytes might have come from a database record or other source.  Otherwise, to read files from a disk more efficiently, use SendFile() instead.");
        }

        public async override Task Handle(GetFileBytesRequest r, CancellationToken ct)
        {
            var fileName = "RadEndpoints.jpg";

            Response = new()
            {
                Bytes = await GetFileBytes(@$"Features\Files\_common\{fileName}"),
                ContentType = MediaTypeNames.Image.Jpeg,
                FileDownloadName = fileName,
                EnableRangeProcessing = false,
                LastModified = DateTimeOffset.UtcNow
            };
            SendBytes(Response);
        }

        public async static Task<byte[]> GetFileBytes(string relativePath)
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
