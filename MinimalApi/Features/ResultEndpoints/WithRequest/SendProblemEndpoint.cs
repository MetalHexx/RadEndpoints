namespace MinimalApi.Features.ResultEndpoints.WithRequest;

// Custom IRadProblem implementation for testing
public class CustomDomainError : IRadProblem
{
    public string Message { get; set; } = string.Empty;
    public string ErrorCode { get; set; } = string.Empty;
}

public class SendProblemRequest
{
    [FromRoute] public string Type { get; set; } = string.Empty;
}

public class SendProblemResponse
{
    public string Message { get; set; } = "Success";
}

public class SendProblemEndpoint : RadEndpoint<SendProblemRequest, SendProblemResponse>
{
    public override void Configure()
    {
        Get("/api/withRequest/sendproblem/{type}");
    }

    public override async Task Handle(SendProblemRequest req, CancellationToken ct)
    {
        switch (req.Type)
        {
            case "http-result":
                SendProblem(TypedResults.Problem(
                    title: "Custom HTTP problem",
                    statusCode: StatusCodes.Status418ImATeapot,
                    detail: "This is a custom problem using ProblemHttpResult"));
                return;
                
            case "validation-problem":
                var errors = new Dictionary<string, string[]>
                {
                    { "Field1", new[] { "Field1 is required" } },
                    { "Field2", new[] { "Field2 must be valid" } }
                };
                SendProblem(TypedResults.ValidationProblem(
                    errors,
                    title: "Custom validation problem",
                    detail: "Multiple validation errors occurred"));
                return;
                
            case "rad-problem":
                SendProblem(new CustomDomainError
                {
                    Message = "Custom domain error occurred",
                    ErrorCode = "DOMAIN_ERROR_001"
                });
                return;
                
            case "internal-error":
                SendProblem(new InternalError("Internal error via IRadProblem", null));
                return;
                
            default:
                Response = new SendProblemResponse { Message = $"No problem for {req.Type}" };
                Send();
                break;
        }
    }

    // Override GetProblemResult to handle custom domain error
    protected override IResult GetProblemResult(IRadProblem problem)
    {
        if (problem is CustomDomainError customError)
        {
            return TypedResults.Problem(
                title: customError.Message,
                statusCode: StatusCodes.Status422UnprocessableEntity,
                detail: $"Error Code: {customError.ErrorCode}");
        }

        return base.GetProblemResult(problem);
    }
}
