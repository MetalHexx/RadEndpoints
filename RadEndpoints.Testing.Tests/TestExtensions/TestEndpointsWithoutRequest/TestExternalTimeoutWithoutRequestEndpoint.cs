namespace RadEndpoints.Testing.Tests
{
    public class TestExternalTimeoutWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            SendExternalTimeout("External service timeout");
            return Task.CompletedTask;
        }
    }
}