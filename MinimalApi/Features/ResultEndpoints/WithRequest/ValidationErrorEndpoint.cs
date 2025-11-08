namespace MinimalApi.Features.ResultEndpoints.WithRequest;

public class ValidationErrorRequest
{
    [FromRoute] public string Id { get; set; } = string.Empty;
}

public class ValidationErrorResponse
{
    public string Message { get; set; } = "Success";
}

public class ValidationErrorEndpoint : RadEndpoint<ValidationErrorRequest, ValidationErrorResponse>
{
    public override void Configure()
    {
        Get("/api/validation-error/{id}");
    }

    public override async Task Handle(ValidationErrorRequest req, CancellationToken ct)
    {
        if (req.Id == "invalid")
        {
            SendValidationError($"Validation failed for {req.Id}");
            return;
        }

        Response = new ValidationErrorResponse { Message = $"Validation passed for {req.Id}" };
        Send();
    }
}
