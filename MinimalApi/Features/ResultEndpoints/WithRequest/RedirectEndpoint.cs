namespace MinimalApi.Features.ResultEndpoints.WithRequest;

public class RedirectRequest
{
    [FromRoute] public string Id { get; set; } = string.Empty;
}

public class RedirectResponse
{
    public string Message { get; set; } = "Success";
}

public class RedirectEndpoint : RadEndpoint<RedirectRequest, RedirectResponse>
{
    public override void Configure()
    {
        Get("/api/redirect/{id}");
    }

    public override async Task Handle(RedirectRequest req, CancellationToken ct)
    {
        switch (req.Id)
        {
            case "permanent":
                SendRedirect("/api/success/redirected", permanent: true, preserveMethod: false);
                return;
            case "permanent-preserve":
                SendRedirect("/api/success/redirected", permanent: true, preserveMethod: true);
                return;
            case "temporary":
                SendRedirect("/api/success/redirected", permanent: false, preserveMethod: false);
                return;
            case "temporary-preserve":
                SendRedirect("/api/success/redirected", permanent: false, preserveMethod: true);
                return;
            default:
                Response = new RedirectResponse { Message = $"No redirect for {req.Id}" };
                Send();
                break;
        }
    }
}
