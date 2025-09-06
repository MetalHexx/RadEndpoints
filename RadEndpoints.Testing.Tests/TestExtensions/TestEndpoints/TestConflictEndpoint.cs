namespace RadEndpoints.Testing.Tests
{
    public class TestConflictEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            SendConflict("Resource conflict");
            return Task.CompletedTask;
        }
    }
}