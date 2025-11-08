namespace MinimalApi.Features.ResultEndpoints.WithoutRequest;

public class NotFoundWithoutRequestResponse
{
    public string Message { get; set; } = "Success";
}

public class NotFoundWithoutRequestEndpoint : RadEndpointWithoutRequest<NotFoundWithoutRequestResponse>
{
    public override void Configure()
    {
        Get("/api/withoutRequest/notfound");
    }

    public override async Task Handle(CancellationToken ct)
    {
        SendNotFound("The resource was not found.");
    }
}
