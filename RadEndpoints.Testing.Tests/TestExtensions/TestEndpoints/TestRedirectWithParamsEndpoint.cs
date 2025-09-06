namespace RadEndpoints.Testing.Tests
{
    public class TestRedirectWithParamsEndpoint : RadEndpoint<TestRequest, TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(TestRequest r, CancellationToken ct)
        {
            SendRedirect("/permanent-redirect", permanent: true, preserveMethod: true);
            return Task.CompletedTask;
        }
    }
}