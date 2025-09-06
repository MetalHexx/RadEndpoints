namespace RadEndpoints.Testing.Tests
{
    public class TestErrorWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            SendInternalError("Test Error");
            return Task.CompletedTask;
        }
    }
}