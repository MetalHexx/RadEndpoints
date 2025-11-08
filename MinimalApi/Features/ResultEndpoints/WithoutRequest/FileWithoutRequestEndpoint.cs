namespace MinimalApi.Features.ResultEndpoints.WithoutRequest;

public class FileWithoutRequestResponse
{
    public string Message { get; set; } = "Success";
}

public class FileWithoutRequestEndpoint : RadEndpointWithoutRequest<FileWithoutRequestResponse>
{
    public override void Configure()
    {
        Get("/api/withoutRequest/file");
    }

    public override async Task Handle(CancellationToken ct)
    {
        var testFilePath = Path.Combine(Path.GetTempPath(), "rad-test-norequest.txt");
        
        if (!System.IO.File.Exists(testFilePath))
        {
            await System.IO.File.WriteAllTextAsync(testFilePath, "Test file content without request", ct);
        }

        SendFile(new RadFile
        {
            Path = testFilePath,
            ContentType = "text/plain",
            FileDownloadName = "file-norequest.txt"
        });
    }
}
