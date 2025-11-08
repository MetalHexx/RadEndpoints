namespace MinimalApi.Features.ResultEndpoints.WithRequest;

public class FileRequest
{
    [FromRoute] public string Name { get; set; } = string.Empty;
}

public class FileResponse
{
    public string Message { get; set; } = "Success";
}

public class FileEndpoint : RadEndpoint<FileRequest, FileResponse>
{
    public override void Configure()
    {
        Get("/api/file/{name}");
    }

    public override async Task Handle(FileRequest req, CancellationToken ct)
    {
        // Create a test file in the temp directory if needed
        var testFilePath = Path.Combine(Path.GetTempPath(), $"rad-test-{req.Name}.txt");
        
        if (req.Name == "test" || req.Name == "download")
        {
            // Ensure test file exists
            if (!System.IO.File.Exists(testFilePath))
            {
                await System.IO.File.WriteAllTextAsync(testFilePath, $"Test file content for {req.Name}", ct);
            }

            SendFile(new RadFile
            {
                Path = testFilePath,
                ContentType = "text/plain",
                FileDownloadName = req.Name == "download" ? $"{req.Name}.txt" : null
            });
            return;
        }

        Response = new FileResponse { Message = $"File {req.Name} not found" };
        Send();
    }
}
