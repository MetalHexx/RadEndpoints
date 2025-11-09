namespace MinimalApi.Features.ResultEndpoints.WithoutRequest;

public class StreamWithoutRequestResponse
{
    public string Message { get; set; } = "Success";
}

public class StreamWithoutRequestEndpoint : RadEndpointWithoutRequest<StreamWithoutRequestResponse>
{
    public override void Configure()
    {
        Get("/api/withoutRequest/stream");
    }

    public override async Task Handle(CancellationToken ct)
    {
        var content = "This is a streamed response without request";
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

        SendStream(new RadStream
        {
            Stream = stream,
            ContentType = "text/plain",
            FileDownloadName = "stream-norequest.txt"
        });
    }
}
