namespace MinimalApi.Features.ResultEndpoints.WithoutRequest;

public class ForbiddenWithoutRequestResponse
{
    public string Message { get; set; } = "Success";
}

public class ForbiddenWithoutRequestEndpoint : RadEndpointWithoutRequest<ForbiddenWithoutRequestResponse>
{
    public override void Configure()
    {
        Get("/api/norequest/forbidden");
    }

    public override async Task Handle(CancellationToken ct)
    {
        SendForbidden("Access forbidden");
    }
}
