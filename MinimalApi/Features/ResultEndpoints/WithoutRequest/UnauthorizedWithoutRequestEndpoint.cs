namespace MinimalApi.Features.ResultEndpoints.WithoutRequest;

public class UnauthorizedWithoutRequestResponse
{
    public string Message { get; set; } = "Success";
}

public class UnauthorizedWithoutRequestEndpoint : RadEndpointWithoutRequest<UnauthorizedWithoutRequestResponse>
{
    public override void Configure()
    {
        Get("/api/norequest/unauthorized");
    }

    public override async Task Handle(CancellationToken ct)
    {
        SendUnauthorized("Authentication required");
    }
}
