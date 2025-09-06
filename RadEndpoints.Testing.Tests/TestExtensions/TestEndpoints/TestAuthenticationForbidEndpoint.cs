namespace RadEndpoints.Testing.Tests
{
    public class TestAuthenticationForbidEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            SendForbidden();
            return Task.CompletedTask;
        }
    }
}