namespace RadEndpoints.Testing.Tests
{
    public class TestErrorEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            SendInternalError("Test Error");
            return Task.CompletedTask;
        }
    }
}