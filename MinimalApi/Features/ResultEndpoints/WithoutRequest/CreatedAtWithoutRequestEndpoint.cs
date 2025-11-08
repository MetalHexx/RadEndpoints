namespace MinimalApi.Features.ResultEndpoints.WithoutRequest;

public class CreatedAtWithoutRequestResponse
{
    public string Message { get; set; } = "Created";
    public string? ItemId { get; set; }
}

public class CreatedAtWithoutRequestEndpoint : RadEndpointWithoutRequest<CreatedAtWithoutRequestResponse>
{
    public override void Configure()
    {
        Post("/api/norequest/created");
    }

    public override async Task Handle(CancellationToken ct)
    {
        Response = new CreatedAtWithoutRequestResponse 
        { 
            Message = "Item created",
            ItemId = "new-item-123"
        };
        SendCreatedAt("/api/items/new-item-123");
    }
}
