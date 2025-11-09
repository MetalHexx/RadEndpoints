namespace MinimalApi.Features.ResultEndpoints.WithoutRequest;

public class ValidationErrorWithoutRequestResponse
{
    public string Message { get; set; } = "Success";
}

public class ValidationErrorWithoutRequestEndpoint : RadEndpointWithoutRequest<ValidationErrorWithoutRequestResponse>
{
    public override void Configure()
    {
        Get("/api/withoutRequest/validation-error");
    }

    public override async Task Handle(CancellationToken ct)
    {
        SendValidationError("Validation failed");
    }
}
