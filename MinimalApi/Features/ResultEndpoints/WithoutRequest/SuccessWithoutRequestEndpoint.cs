namespace MinimalApi.Features.ResultEndpoints.WithoutRequest;

public class SuccessWithoutRequestResponse
{
    public string Message { get; set; } = "Success";
    public string? Data { get; set; }
}

public class SuccessWithoutRequestEndpoint : RadEndpointWithoutRequest<SuccessWithoutRequestResponse>
{
    public override void Configure()
    {
        Get("/api/norequest/success");
    }

    public override async Task Handle(CancellationToken ct)
    {
        Response = new SuccessWithoutRequestResponse 
        { 
            Message = "Operation successful",
            Data = "sample data"
        };
        Send();
    }
}
