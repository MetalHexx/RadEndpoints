namespace RadEndpoints.Testing.Tests
{
    public class TestAuthenticationForbidWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            SendForbidden();
            return Task.CompletedTask;
        }
    }
}