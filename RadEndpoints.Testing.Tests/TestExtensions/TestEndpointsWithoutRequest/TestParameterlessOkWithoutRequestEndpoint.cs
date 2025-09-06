namespace RadEndpoints.Testing.Tests
{
    public class TestParameterlessOkWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            Response = new TestResponse { IntProperty = 50 };
            Send();
            return Task.CompletedTask;
        }
    }
}