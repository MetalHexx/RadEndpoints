using System.Net.Mime;
using System.Reflection;

namespace MinimalApi.Features.Files.GetFileStream
{
    public class GetFileStreamEndpoint : RadEndpoint<GetFileStreamRequest, GetFileStreamResponse>
    {
        public override void Configure()
        {
            Get("/files/stream")
                .Produces<byte[]>(StatusCodes.Status200OK, MediaTypeNames.Image.Jpeg)                
                .WithDocument(tag: "Files", desc: "Example of how to send a file when you already have a handle to a file or memory stream.  \r\n\r\n  We're reading from the disk for convenience, but the specific demonstration here is how to return a memory or file stream.  In real world usage, you may be streaming byte data directly from a database or other source for efficiency.  Otherwise, to read files directly from a disk more efficiently, use SendFile() instead.");
        }

        public override async Task Handle(GetFileStreamRequest r, CancellationToken ct)
        {
            await Task.CompletedTask; //simulate async work

            var fileName = "RadEndpoints.jpg";

            Response = new()
            {
                Stream = GetFileStream(@$"Features\Files\_common\{fileName}"),
                ContentType = MediaTypeNames.Image.Jpeg,
                FileDownloadName = fileName,
                LastModified = DateTimeOffset.UtcNow
            };
            SendStream(Response);
        }

        public static Stream GetFileStream(string relativePath)
        {
            var exeRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filePath = Path.Combine(exeRoot!, relativePath.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            return File.Open(filePath, FileMode.Open, FileAccess.Read);
        }
    }
}
