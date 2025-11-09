namespace MinimalApi.Features.ResultEndpoints.WithRequest;

public class ConflictRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class ConflictResponse
{
    public string Message { get; set; } = "Success";
    public string Id { get; set; } = string.Empty;
}

public class ConflictEndpoint : RadEndpoint<ConflictRequest, ConflictResponse>
{
    public override void Configure()
    {
        Post("/api/withRequest/users");
    }

    public override async Task Handle(ConflictRequest req, CancellationToken ct)
    {
        // Simulate checking if user with email already exists
        if (req.Email == "existing@example.com")
        {
            SendConflict($"A user with email {req.Email} already exists.");
            return;
        }

        // Simulate successful user creation
        Response = new ConflictResponse 
        { 
            Message = "User created successfully",
            Id = Guid.NewGuid().ToString()
        };
        Send();
    }
}
