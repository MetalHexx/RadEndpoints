namespace RadEndpoints.Testing.Tests
{
    public class TestConflictWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            SendConflict("Resource conflict");
            return Task.CompletedTask;
        }
    }
}