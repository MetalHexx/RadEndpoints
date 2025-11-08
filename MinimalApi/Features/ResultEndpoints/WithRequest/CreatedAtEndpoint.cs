namespace MinimalApi.Features.ResultEndpoints.WithRequest;

public class CreatedAtRequest
{
    [FromRoute] public string Id { get; set; } = string.Empty;
}

public class CreatedAtResponse
{
    public string Message { get; set; } = "Created";
    public string? ItemId { get; set; }
}

public class CreatedAtEndpoint : RadEndpoint<CreatedAtRequest, CreatedAtResponse>
{
    public override void Configure()
    {
        Post("/api/withRequest/created/{id}");
    }

    public override async Task Handle(CreatedAtRequest req, CancellationToken ct)
    {
        if (req.Id == "with-response")
        {
            SendCreatedAt($"/api/items/{req.Id}", new CreatedAtResponse
            {
                Message = "Item created with custom response",
                ItemId = req.Id
            });
            return;
        }

        Response = new CreatedAtResponse 
        { 
            Message = $"Item {req.Id} created",
            ItemId = req.Id
        };
        SendCreatedAt($"/api/items/{req.Id}");
    }
}
