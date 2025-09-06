namespace RadEndpoints.Testing.Tests
{
    public class TestUnauthorizedEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            SendUnauthorized("Unauthorized access");
            return Task.CompletedTask;
        }
    }
}