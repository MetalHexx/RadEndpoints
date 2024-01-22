using System.Net.Mime;
using System.Reflection;

namespace MinimalApi.Features.Files
{
    public class GetFileRequest : RadRequest { }
    public class GetFileResponse : RadResponseBytes { }
    public class GetFileEndpoint : RadEndpoint<GetFileRequest, GetFileResponse>
    {
        public override void Configure()
        {
            Get("/files")
                .Produces<GetFileResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithDocument(tag: "Files", desc: "Example of how to send file bytes");
        }

        public async override Task Handle(GetFileRequest r, CancellationToken ct)
        {
            var fileName = "RadEndpoints.jpg";

            Response = new GetFileResponse
            {
                Bytes = await GetFileBytes(@$"Features\File\{fileName}"),
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
