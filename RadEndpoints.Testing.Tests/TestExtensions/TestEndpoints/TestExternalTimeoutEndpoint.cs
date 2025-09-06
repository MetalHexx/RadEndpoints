namespace RadEndpoints.Testing.Tests
{
    public class TestExternalTimeoutEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            SendExternalTimeout("External service timeout");
            return Task.CompletedTask;
        }
    }
}