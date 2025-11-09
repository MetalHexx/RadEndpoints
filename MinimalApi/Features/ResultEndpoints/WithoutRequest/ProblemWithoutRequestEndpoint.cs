namespace MinimalApi.Features.ResultEndpoints.WithoutRequest;

public class ProblemWithoutRequestResponse
{
    public string Message { get; set; } = "Success";
}

public class ProblemWithoutRequestEndpoint : RadEndpointWithoutRequest<ProblemWithoutRequestResponse>
{
    public override void Configure()
    {
        Get("/api/withoutRequest/problem");
    }

    public override async Task Handle(CancellationToken ct)
    {
        SendInternalError("Internal server error occurred");
    }
}
