namespace RadEndpoints.Testing.Tests
{
    public class TestAuthenticationChallengeWithoutRequestEndpoint : RadEndpointWithoutRequest<TestResponse>
    {
        public override void Configure() { }

        public override Task Handle(CancellationToken ct)
        {
            SendUnauthorized();
            return Task.CompletedTask;
        }
    }
}