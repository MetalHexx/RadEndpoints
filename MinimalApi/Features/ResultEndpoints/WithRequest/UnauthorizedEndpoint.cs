namespace MinimalApi.Features.ResultEndpoints.WithRequest;

public class UnauthorizedRequest
{
    [FromRoute] public string Id { get; set; } = string.Empty;
}

public class UnauthorizedResponse
{
    public string Message { get; set; } = "Success";
}

public class UnauthorizedEndpoint : RadEndpoint<UnauthorizedRequest, UnauthorizedResponse>
{
    public override void Configure()
    {
        Get("/api/unauthorized/{id}");
    }

    public override async Task Handle(UnauthorizedRequest req, CancellationToken ct)
    {
        if (req.Id == "auth-required")
        {
            SendUnauthorized($"Authentication required for {req.Id}");
            return;
        }

        if (req.Id == "no-auth")
        {
            SendUnauthorized();
            return;
        }

        Response = new UnauthorizedResponse { Message = $"Authorized access to {req.Id}" };
        Send();
    }
}
