namespace RadEndpoints.Testing.Tests
{
    public class TestOkEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            Send(new TestResponse { IntProperty = r.IntProperty + 1 });
            return Task.CompletedTask;
        }
    }
}