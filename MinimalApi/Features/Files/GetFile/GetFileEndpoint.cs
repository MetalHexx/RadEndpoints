using System.Net.Mime;
using System.Reflection;

namespace MinimalApi.Features.Files.GetFile
{
    public class GetFileEndpoint : RadEndpoint<GetFileRequest, GetFileResponse>
    {
        public override void Configure()
        {
            Get("/files")
                .Produces<byte[]>(StatusCodes.Status200OK, MediaTypeNames.Image.Jpeg)
                .WithDocument(tag: "Files", desc: "Example of how to send a file when you need to fetch the file directly from the disk.  \r\n\r\n  As you can see we don't need to convert the file to a stream and bytes.  Using SendFile() in this case is the most effient approach.");
        }

        public override async Task Handle(GetFileRequest r, CancellationToken ct)
        {
            await Task.CompletedTask; //simulate async work

            var fileName = "RadEndpoints.jpg";

            Response = new GetFileResponse
            {
                Path = GetRootedPath(@$"Features\Files\_common\{fileName}"),
                ContentType = MediaTypeNames.Image.Jpeg,
                FileDownloadName = fileName,
                LastModified = DateTimeOffset.UtcNow
            };
            SendFile(Response);
        }

        private static string GetRootedPath(string relativePath)
        {
            var exeRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Path.Combine(exeRoot!, relativePath.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
        }
    }
}
