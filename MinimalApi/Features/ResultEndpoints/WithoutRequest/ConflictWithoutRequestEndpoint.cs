namespace MinimalApi.Features.ResultEndpoints.WithoutRequest;

public class ConflictWithoutRequestResponse
{
    public string Message { get; set; } = "Success";
}

public class ConflictWithoutRequestEndpoint : RadEndpointWithoutRequest<ConflictWithoutRequestResponse>
{
    public override void Configure()
    {
        Get("/api/norequest/conflict");
    }

    public override async Task Handle(CancellationToken ct)
    {
        SendConflict("The resource already exists.");
    }
}
