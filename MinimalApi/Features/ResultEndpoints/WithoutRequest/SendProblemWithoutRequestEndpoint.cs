namespace MinimalApi.Features.ResultEndpoints.WithoutRequest;

public class SendProblemWithoutRequestResponse
{
    public string Message { get; set; } = "Success";
}

public class SendProblemWithoutRequestEndpoint : RadEndpointWithoutRequest<SendProblemWithoutRequestResponse>
{
    public override void Configure()
    {
        Get("/api/norequest/sendproblem");
    }

    public override async Task Handle(CancellationToken ct)
    {
        SendProblem(TypedResults.Problem(
            title: "Custom problem without request",
            statusCode: StatusCodes.Status422UnprocessableEntity,
            detail: "This is a custom problem details response"));
    }
}
