using System.Net.Mime;
using System.Reflection;

namespace MinimalApi.Features.Files.GetFileStream
{
    public class GetFileStreamEndpoint : RadEndpoint<GetFileStreamRequest, GetFileStreamResponse>
    {
        public override void Configure()
        {
            Get("/files/stream")
                .Produces<Stream>(StatusCodes.Status200OK)
                .WithDocument(tag: "Files", desc: "Example of how to send a file stream");
        }

        public override async Task Handle(GetFileStreamRequest r, CancellationToken ct)
        {
            await Task.Delay(1, ct);

            var fileName = "RadEndpoints.jpg";

            Response = new GetFileStreamResponse
            {
                Stream = GetFileStream(@$"Features\Files\GetFileStream\{fileName}"),
                ContentType = MediaTypeNames.Image.Jpeg,
                FileDownloadName = fileName,
                EnableRangeProcessing = false,
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
