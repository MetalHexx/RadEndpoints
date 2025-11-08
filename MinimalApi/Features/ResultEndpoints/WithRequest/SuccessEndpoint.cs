namespace MinimalApi.Features.ResultEndpoints.WithRequest;

public class SuccessRequest
{
    [FromRoute] public string Id { get; set; } = string.Empty;
}

public class SuccessResponse
{
    public string Message { get; set; } = "Success";
    public string? Data { get; set; }
}

public class SuccessEndpoint : RadEndpoint<SuccessRequest, SuccessResponse>
{
    public override void Configure()
    {
        Get("/api/withRequest/success/{id}");
    }

    public override async Task Handle(SuccessRequest req, CancellationToken ct)
    {
        if (req.Id == "with-response")
        {
            Send(new SuccessResponse 
            { 
                Message = "Success with custom response",
                Data = "custom data"
            });
            return;
        }

        Response = new SuccessResponse 
        { 
            Message = $"Success for {req.Id}",
            Data = req.Id
        };
        Send();
    }
}
