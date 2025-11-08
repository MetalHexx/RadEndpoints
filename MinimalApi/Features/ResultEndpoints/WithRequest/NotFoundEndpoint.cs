namespace MinimalApi.Features.ResultEndpoints.WithRequest;

public class NotFoundRequest
{
    [FromRoute] public string Id { get; set; } = string.Empty;
}

public class NotFoundResponse
{
    public string Message { get; set; } = "Success";
}

public class NotFoundEndpoint : RadEndpoint<NotFoundRequest, NotFoundResponse>
{
    public override void Configure()
    {
        Get("/api/withRequest/notfound/{id}");
    }

    public override async Task Handle(NotFoundRequest req, CancellationToken ct)
    {
        if (req.Id == "missing")
        {
            SendNotFound($"The item {req.Id} was not found.");
            return;
        }

        Response = new NotFoundResponse { Message = $"Found item {req.Id}" };
        Send();
    }
}
