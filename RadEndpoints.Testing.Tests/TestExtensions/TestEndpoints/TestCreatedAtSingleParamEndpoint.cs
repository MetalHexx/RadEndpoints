namespace RadEndpoints.Testing.Tests
{
    public class TestCreatedAtSingleParamEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            Response = new TestResponse { IntProperty = r.IntProperty };
            SendCreatedAt($"/test/{r.IntProperty}");
            return Task.CompletedTask;
        }
    }
}