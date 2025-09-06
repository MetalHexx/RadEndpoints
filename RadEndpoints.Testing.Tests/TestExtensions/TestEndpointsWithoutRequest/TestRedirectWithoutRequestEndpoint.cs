namespace RadEndpoints.Testing.Tests
{
    public class TestRedirectWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            SendRedirect("/redirected");
            return Task.CompletedTask;
        }
    }
}