namespace MinimalApi.Features.ResultEndpoints.WithoutRequest;

public class RedirectWithoutRequestResponse
{
    public string Message { get; set; } = "Success";
}

public class RedirectWithoutRequestEndpoint : RadEndpointWithoutRequest<RedirectWithoutRequestResponse>
{
    public override void Configure()
    {
        Get("/api/withoutRequest/redirect");
    }

    public override async Task Handle(CancellationToken ct)
    {
        SendRedirect("/api/success/redirected", permanent: false, preserveMethod: false);
    }
}
