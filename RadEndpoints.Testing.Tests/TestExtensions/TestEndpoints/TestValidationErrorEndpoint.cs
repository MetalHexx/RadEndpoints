namespace RadEndpoints.Testing.Tests
{
    public class TestValidationErrorEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            SendValidationError("Validation failed");
            return Task.CompletedTask;
        }
    }
}