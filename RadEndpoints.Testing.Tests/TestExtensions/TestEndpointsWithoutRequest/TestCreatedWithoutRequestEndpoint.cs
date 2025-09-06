namespace RadEndpoints.Testing.Tests
{
    public class TestCreatedWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            var response = new TestResponse { IntProperty = 100 };
            SendCreatedAt("/test/100", response);
            return Task.CompletedTask;
        }
    }
}