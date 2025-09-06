namespace RadEndpoints.Testing.Tests
{
    public class TestNotFoundEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            SendNotFound("Resource not found");
            return Task.CompletedTask;
        }
    }
}