namespace RadEndpoints.Testing.Tests
{
    public class TestRedirectEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            SendRedirect("/redirected");
            return Task.CompletedTask;
        }
    }
}