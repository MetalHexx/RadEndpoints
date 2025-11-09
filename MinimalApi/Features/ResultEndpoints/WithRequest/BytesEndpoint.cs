namespace MinimalApi.Features.ResultEndpoints.WithRequest;

public class BytesRequest
{
    [FromRoute] public string Size { get; set; } = "10";
}

public class BytesResponse
{
    public string Message { get; set; } = "Success";
}

public class BytesEndpoint : RadEndpoint<BytesRequest, BytesResponse>
{
    public override void Configure()
    {
        Get("/api/withRequest/bytes/{size}");
    }

    public override async Task Handle(BytesRequest req, CancellationToken ct)
    {
        if (int.TryParse(req.Size, out var size) && size > 0)
        {
            var bytes = new byte[size];
            for (int i = 0; i < size; i++)
            {
                bytes[i] = (byte)(i % 256);
            }

            SendBytes(new RadBytes
            {
                Bytes = bytes,
                ContentType = "application/octet-stream",
                FileDownloadName = $"data-{size}.bin",
                EnableRangeProcessing = true
            });
            return;
        }

        Response = new BytesResponse { Message = "Invalid size parameter" };
        Send();
    }
}
