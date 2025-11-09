namespace MinimalApi.Features.ResultEndpoints.WithRequest;

public class StreamRequest
{
    [FromRoute] public string Type { get; set; } = string.Empty;
}

public class StreamResponse
{
    public string Message { get; set; } = "Success";
}

public class StreamEndpoint : RadEndpoint<StreamRequest, StreamResponse>
{
    public override void Configure()
    {
        Get("/api/withRequest/stream/{type}");
    }

    public override async Task Handle(StreamRequest req, CancellationToken ct)
    {
        if (req.Type == "text")
        {
            var content = "This is a streamed text response";
            var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

            SendStream(new RadStream
            {
                Stream = stream,
                ContentType = "text/plain",
                FileDownloadName = "stream.txt",
                EnableRangeProcessing = true
            });
            return;
        }

        if (req.Type == "binary")
        {
            var bytes = new byte[] { 0x48, 0x65, 0x6C, 0x6C, 0x6F }; // "Hello"
            var stream = new MemoryStream(bytes);

            SendStream(new RadStream
            {
                Stream = stream,
                ContentType = "application/octet-stream",
                FileDownloadName = "stream.bin"
            });
            return;
        }

        Response = new StreamResponse { Message = $"Unknown stream type: {req.Type}" };
        Send();
    }
}
