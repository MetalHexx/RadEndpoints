namespace RadEndpoints.Testing.Tests
{
    public class TestParameterlessOkEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            Response = new TestResponse { IntProperty = r.IntProperty };
            Send();
            return Task.CompletedTask;
        }
    }
}