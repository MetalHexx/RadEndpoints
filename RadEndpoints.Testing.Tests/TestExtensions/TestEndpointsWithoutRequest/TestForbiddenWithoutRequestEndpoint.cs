namespace RadEndpoints.Testing.Tests
{
    public class TestForbiddenWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            SendForbidden("Forbidden access");
            return Task.CompletedTask;
        }
    }
}