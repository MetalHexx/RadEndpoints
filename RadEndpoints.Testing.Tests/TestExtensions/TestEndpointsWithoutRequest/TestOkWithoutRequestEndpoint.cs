namespace RadEndpoints.Testing.Tests
{
    public class TestOkWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            Send(new TestResponse { IntProperty = 42 });
            return Task.CompletedTask;
        }
    }
}