namespace MinimalApi.Features.ResultEndpoints.WithRequest;

public class ForbiddenRequest
{
    [FromRoute] public string Id { get; set; } = string.Empty;
}

public class ForbiddenResponse
{
    public string Message { get; set; } = "Success";
}

public class ForbiddenEndpoint : RadEndpoint<ForbiddenRequest, ForbiddenResponse>
{
    public override void Configure()
    {
        Get("/api/forbidden/{id}");
    }

    public override async Task Handle(ForbiddenRequest req, CancellationToken ct)
    {
        if (req.Id == "no-access")
        {
            SendForbidden($"Access forbidden to {req.Id}");
            return;
        }

        if (req.Id == "no-permission")
        {
            SendForbidden();
            return;
        }

        Response = new ForbiddenResponse { Message = $"Access granted to {req.Id}" };
        Send();
    }
}
