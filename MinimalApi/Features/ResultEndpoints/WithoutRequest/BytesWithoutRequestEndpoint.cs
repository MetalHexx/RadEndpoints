namespace MinimalApi.Features.ResultEndpoints.WithoutRequest;

public class BytesWithoutRequestResponse
{
    public string Message { get; set; } = "Success";
}

public class BytesWithoutRequestEndpoint : RadEndpointWithoutRequest<BytesWithoutRequestResponse>
{
    public override void Configure()
    {
        Get("/api/withoutRequest/bytes");
    }

    public override async Task Handle(CancellationToken ct)
    {
        var bytes = new byte[20];
        for (int i = 0; i < 20; i++)
        {
            bytes[i] = (byte)(i % 256);
        }

        SendBytes(new RadBytes
        {
            Bytes = bytes,
            ContentType = "application/octet-stream",
            FileDownloadName = "data-norequest.bin"
        });
    }
}
