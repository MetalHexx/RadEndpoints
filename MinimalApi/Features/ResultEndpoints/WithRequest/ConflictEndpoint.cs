namespace MinimalApi.Features.ResultEndpoints.WithRequest;

public class ConflictRequest
{
    [FromRoute] public string Id { get; set; } = string.Empty;
}

public class ConflictResponse
{
    public string Message { get; set; } = "Success";
}

public class ConflictEndpoint : RadEndpoint<ConflictRequest, ConflictResponse>
{
    public override void Configure()
    {
        Get("/api/conflict/{id}");
    }

    public override async Task Handle(ConflictRequest req, CancellationToken ct)
    {
        if (req.Id == "exists")
        {
            SendConflict($"The item {req.Id} already exists.");
            return;
        }

        Response = new ConflictResponse { Message = $"Item {req.Id} is available" };
        Send();
    }
}
