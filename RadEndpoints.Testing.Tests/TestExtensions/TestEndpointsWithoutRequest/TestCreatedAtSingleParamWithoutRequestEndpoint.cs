namespace RadEndpoints.Testing.Tests
{
    public class TestCreatedAtSingleParamWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            Response = new TestResponse { IntProperty = 75 };
            SendCreatedAt("/test/75");
            return Task.CompletedTask;
        }
    }
}