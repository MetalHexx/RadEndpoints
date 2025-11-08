namespace MinimalApi.Features.ResultEndpoints.WithRequest;

public class ProblemRequest
{
    [FromRoute] public string Id { get; set; } = string.Empty;
}

public class ProblemResponse
{
    public string Message { get; set; } = "Success";
}

public class ProblemEndpoint : RadEndpoint<ProblemRequest, ProblemResponse>
{
    public override void Configure()
    {
        Get("/api/problem/{id}");
    }

    public override async Task Handle(ProblemRequest req, CancellationToken ct)
    {
        switch (req.Id)
        {
            case "internal":
                SendInternalError($"Internal server error for {req.Id}");
                return;
            case "external":
                SendExternalError($"External service error for {req.Id}");
                return;
            case "timeout":
                SendExternalTimeout($"External service timeout for {req.Id}");
                return;
            default:
                Response = new ProblemResponse { Message = $"No problem with {req.Id}" };
                Send();
                break;
        }
    }
}
