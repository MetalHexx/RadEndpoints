namespace RadEndpoints.Testing.Tests
{
    public class TestRedirectWithParamsWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            SendRedirect("/permanent-redirect", permanent: true, preserveMethod: true);
            return Task.CompletedTask;
        }
    }
}