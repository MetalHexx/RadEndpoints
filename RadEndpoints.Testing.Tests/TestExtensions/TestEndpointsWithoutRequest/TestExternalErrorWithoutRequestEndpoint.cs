namespace RadEndpoints.Testing.Tests
{
    public class TestExternalErrorWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            SendExternalError("External service error");
            return Task.CompletedTask;
        }
    }
}