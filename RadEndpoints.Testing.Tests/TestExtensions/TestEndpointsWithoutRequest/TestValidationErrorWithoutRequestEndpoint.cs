namespace RadEndpoints.Testing.Tests
{
    public class TestValidationErrorWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            SendValidationError("Validation failed");
            return Task.CompletedTask;
        }
    }
}