namespace RadEndpoints.Testing.Tests
{
    public class TestExternalErrorEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            SendExternalError("External service error");
            return Task.CompletedTask;
        }
    }
}